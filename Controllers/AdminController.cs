using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CosmeticosApp.Data;
using CosmeticosApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace CosmeticosApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdminController> _logger;

        public AdminController(ApplicationDbContext context, ILogger<AdminController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Dashboard principal del admin
        public async Task<IActionResult> Index()
        {
            var modelo = new AdminDashboardViewModel
            {
                TotalProductos = await _context.Productos.CountAsync(),
                TotalUsuarios = await _context.Users.CountAsync(),
                TotalPedidos = await _context.Pedidos.CountAsync(),
                TotalCategorias = await _context.Categorias.CountAsync(),
                TotalMarcas = await _context.Marcas.CountAsync(),
                PedidosRecientes = await _context.Pedidos
                    .Include(p => p.Usuario)
                    .OrderByDescending(p => p.FechaPedido)
                    .Take(5)
                    .ToListAsync(),
                ProductosBajoStock = await _context.Productos
                    .Where(p => p.Stock < 10)
                    .OrderBy(p => p.Stock)
                    .Take(5)
                    .ToListAsync()
            };

            return View(modelo);
        }

        #region Productos

        // GET: Admin/Productos
        public async Task<IActionResult> Productos()
        {
            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Marca)
                .ToListAsync();

            return View(productos);
        }

        // GET: Admin/CrearProducto
        public async Task<IActionResult> CrearProducto()
        {
            ViewBag.Categorias = await _context.Categorias.Where(c => c.Activa).ToListAsync();
            ViewBag.Marcas = await _context.Marcas.Where(m => m.Activa).ToListAsync();
            return View();
        }

        // POST: Admin/CrearProducto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearProducto(Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Productos.Add(producto);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Producto creado correctamente";
                return RedirectToAction(nameof(Productos));
            }

            var errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            _logger.LogWarning("Valores recibidos: CategoriaId={CategoriaId}, MarcaId={MarcaId}", Request.Form["CategoriaId"], Request.Form["MarcaId"]);
            _logger.LogWarning("Errores al crear producto: {Errores}", string.Join(" | ", errores));
            TempData["ErrorMessage"] = string.Join(" | ", errores);

            ViewBag.Categorias = await _context.Categorias.Where(c => c.Activa).ToListAsync();
            ViewBag.Marcas = await _context.Marcas.Where(m => m.Activa).ToListAsync();
            return View(producto);
        }

        // GET: Admin/EditarProducto/5
        public async Task<IActionResult> EditarProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            ViewBag.Categorias = await _context.Categorias.Where(c => c.Activa).ToListAsync();
            ViewBag.Marcas = await _context.Marcas.Where(m => m.Activa).ToListAsync();
            return View(producto);
        }

        // POST: Admin/EditarProducto/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarProducto(int id, Producto producto)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Productos));
            }

            ViewBag.Categorias = await _context.Categorias.Where(c => c.Activa).ToListAsync();
            ViewBag.Marcas = await _context.Marcas.Where(m => m.Activa).ToListAsync();
            return View(producto);
        }

        // POST: Admin/EliminarProducto/5
        [HttpPost, ActionName("EliminarProducto")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                producto.Activo = false; // Desactivar en lugar de eliminar
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Productos));
        }

        // POST: Admin/ActivarProducto/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivarProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                producto.Activo = true;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Productos));
        }

        #endregion

        #region Pedidos

        // GET: Admin/Pedidos
        public async Task<IActionResult> Pedidos()
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Usuario)
                .OrderByDescending(p => p.FechaPedido)
                .ToListAsync();

            return View(pedidos);
        }

        // GET: Admin/DetallePedido/5
        public async Task<IActionResult> DetallePedido(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Usuario)
                .Include(p => p.DetallesPedidos)
                .ThenInclude(dp => dp.Producto)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // POST: Admin/ActualizarEstadoPedido
        [HttpPost]
        public async Task<IActionResult> ActualizarEstadoPedido(int id, string estado)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido != null)
            {
                pedido.Estado = estado;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(DetallePedido), new { id });
        }

        #endregion

        #region Categorias

        // GET: Admin/Categorias
        public async Task<IActionResult> Categorias()
        {
            return View(await _context.Categorias.ToListAsync());
        }

        // GET: Admin/CrearCategoria
        public IActionResult CrearCategoria()
        {
            return View();
        }

        // POST: Admin/CrearCategoria
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearCategoria(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _context.Categorias.Add(categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Categorias));
            }
            return View(categoria);
        }

        // GET: Admin/EditarCategoria/5
        public async Task<IActionResult> EditarCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        // POST: Admin/EditarCategoria/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarCategoria(int id, Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriaExists(categoria.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Categorias));
            }
            return View(categoria);
        }

        #endregion

        #region Marcas

        // GET: Admin/Marcas
        public async Task<IActionResult> Marcas()
        {
            return View(await _context.Marcas.ToListAsync());
        }

        // GET: Admin/CrearMarca
        public IActionResult CrearMarca()
        {
            return View();
        }

        // POST: Admin/CrearMarca
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearMarca(Marca marca)
        {
            if (ModelState.IsValid)
            {
                _context.Marcas.Add(marca);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Marcas));
            }
            return View(marca);
        }

        // GET: Admin/EditarMarca/5
        public async Task<IActionResult> EditarMarca(int id)
        {
            var marca = await _context.Marcas.FindAsync(id);
            if (marca == null)
            {
                return NotFound();
            }
            return View(marca);
        }

        // POST: Admin/EditarMarca/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarMarca(int id, Marca marca)
        {
            if (id != marca.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(marca);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MarcaExists(marca.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Marcas));
            }
            return View(marca);
        }

        #endregion

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }

        private bool CategoriaExists(int id)
        {
            return _context.Categorias.Any(e => e.Id == id);
        }

        private bool MarcaExists(int id)
        {
            return _context.Marcas.Any(e => e.Id == id);
        }
    }

    public class AdminDashboardViewModel
    {
        public int TotalProductos { get; set; }
        public int TotalUsuarios { get; set; }
        public int TotalPedidos { get; set; }
        public int TotalCategorias { get; set; }
        public int TotalMarcas { get; set; }
        public List<Pedido> PedidosRecientes { get; set; } = new List<Pedido>();
        public List<Producto> ProductosBajoStock { get; set; } = new List<Producto>();
    }
} 