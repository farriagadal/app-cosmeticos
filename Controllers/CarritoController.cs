using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CosmeticosApp.Data;
using CosmeticosApp.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CosmeticosApp.Controllers
{
    public class CarritoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Usuario> _userManager;

        public CarritoController(ApplicationDbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Carrito
        public async Task<IActionResult> Index()
        {
            var carrito = ObtenerCarrito();
            var productosCarrito = new List<CarritoItem>();

            foreach (var item in carrito)
            {
                var producto = await _context.Productos
                    .Include(p => p.Categoria)
                    .Include(p => p.Marca)
                    .FirstOrDefaultAsync(p => p.Id == item.ProductoId);

                if (producto != null)
                {
                    productosCarrito.Add(new CarritoItem
                    {
                        ProductoId = producto.Id,
                        Nombre = producto.Nombre,
                        Precio = producto.Precio,
                        Cantidad = item.Cantidad,
                        ImagenUrl = producto.ImagenUrl,
                        Stock = producto.Stock
                    });
                }
            }

            return View(productosCarrito);
        }

        // GET: Carrito/GetCount - Para el contador AJAX
        [HttpGet]
        public IActionResult GetCount()
        {
            var carrito = ObtenerCarrito();
            var totalItems = carrito.Sum(c => c.Cantidad);
            return Json(totalItems);
        }

        // POST: Carrito/Agregar
        [HttpPost]
        public async Task<IActionResult> Agregar(int productoId, int cantidad = 1)
        {
            var producto = await _context.Productos.FindAsync(productoId);
            if (producto == null || !producto.Activo)
            {
                return NotFound();
            }

            var carrito = ObtenerCarrito();
            var itemExistente = carrito.FirstOrDefault(c => c.ProductoId == productoId);

            if (itemExistente != null)
            {
                itemExistente.Cantidad += cantidad;
            }
            else
            {
                carrito.Add(new CarritoSession { ProductoId = productoId, Cantidad = cantidad });
            }

            GuardarCarrito(carrito);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, mensaje = "Producto agregado al carrito", cantidadTotal = carrito.Sum(c => c.Cantidad) });
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Carrito/Actualizar
        [HttpPost]
        public IActionResult Actualizar(int productoId, int cantidad)
        {
            var carrito = ObtenerCarrito();
            var item = carrito.FirstOrDefault(c => c.ProductoId == productoId);

            if (item != null)
            {
                if (cantidad <= 0)
                {
                    carrito.Remove(item);
                }
                else
                {
                    item.Cantidad = cantidad;
                }
            }

            GuardarCarrito(carrito);
            return RedirectToAction(nameof(Index));
        }

        // POST: Carrito/Eliminar
        [HttpPost]
        public IActionResult Eliminar(int productoId)
        {
            var carrito = ObtenerCarrito();
            var item = carrito.FirstOrDefault(c => c.ProductoId == productoId);

            if (item != null)
            {
                carrito.Remove(item);
            }

            GuardarCarrito(carrito);
            return RedirectToAction(nameof(Index));
        }

        // GET: Carrito/Checkout
        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            var carrito = ObtenerCarrito();
            if (!carrito.Any())
            {
                return RedirectToAction(nameof(Index));
            }

            var usuario = await _userManager.GetUserAsync(User);
            var modelo = new CheckoutViewModel
            {
                DireccionEntrega = usuario?.Direccion ?? "",
                CarritoItems = new List<CarritoItem>()
            };

            foreach (var item in carrito)
            {
                var producto = await _context.Productos.FindAsync(item.ProductoId);
                if (producto != null)
                {
                    modelo.CarritoItems.Add(new CarritoItem
                    {
                        ProductoId = producto.Id,
                        Nombre = producto.Nombre,
                        Precio = producto.Precio,
                        Cantidad = item.Cantidad,
                        ImagenUrl = producto.ImagenUrl
                    });
                }
            }

            modelo.Total = modelo.CarritoItems.Sum(c => c.Precio * c.Cantidad);

            return View(modelo);
        }

        // POST: Carrito/ProcesarPedido
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ProcesarPedido(CheckoutViewModel modelo)
        {
            var carrito = ObtenerCarrito();
            if (!carrito.Any())
            {
                return RedirectToAction(nameof(Index));
            }

            var usuario = await _userManager.GetUserAsync(User);
            var pedido = new Pedido
            {
                UsuarioId = usuario.Id,
                DireccionEntrega = modelo.DireccionEntrega,
                Notas = modelo.Notas,
                Estado = "Pendiente",
                Total = 0
            };

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            decimal total = 0;
            foreach (var item in carrito)
            {
                var producto = await _context.Productos.FindAsync(item.ProductoId);
                if (producto != null)
                {
                    var detalle = new DetallePedido
                    {
                        PedidoId = pedido.Id,
                        ProductoId = producto.Id,
                        Cantidad = item.Cantidad,
                        PrecioUnitario = producto.Precio,
                        Subtotal = producto.Precio * item.Cantidad
                    };

                    _context.DetallesPedidos.Add(detalle);
                    total += detalle.Subtotal;

                    // Actualizar stock
                    producto.Stock -= item.Cantidad;
                }
            }

            pedido.Total = total;
            await _context.SaveChangesAsync();

            // Limpiar carrito
            HttpContext.Session.Remove("Carrito");

            return RedirectToAction("Confirmacion", new { id = pedido.Id });
        }

        // GET: Carrito/Confirmacion
        public async Task<IActionResult> Confirmacion(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.DetallesPedidos)
                .ThenInclude(dp => dp.Producto)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        private List<CarritoSession> ObtenerCarrito()
        {
            var carritoJson = HttpContext.Session.GetString("Carrito");
            return string.IsNullOrEmpty(carritoJson) ? new List<CarritoSession>() : JsonSerializer.Deserialize<List<CarritoSession>>(carritoJson) ?? new List<CarritoSession>();
        }

        private void GuardarCarrito(List<CarritoSession> carrito)
        {
            HttpContext.Session.SetString("Carrito", JsonSerializer.Serialize(carrito));
        }
    }

    public class CarritoSession
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
    }

    public class CarritoItem
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public string? ImagenUrl { get; set; }
        public int Stock { get; set; }
    }

    public class CheckoutViewModel
    {
        public string DireccionEntrega { get; set; } = string.Empty;
        public string? Notas { get; set; }
        public decimal Total { get; set; }
        public List<CarritoItem> CarritoItems { get; set; } = new List<CarritoItem>();
    }
} 