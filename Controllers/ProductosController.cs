using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CosmeticosApp.Data;
using CosmeticosApp.Models;

namespace CosmeticosApp.Controllers
{
    public class ProductosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Productos
        public async Task<IActionResult> Index(string? buscar, int? categoriaId, int? marcaId, decimal? precioMin, decimal? precioMax)
        {
            var productos = _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Marca)
                .Where(p => p.Activo);

            // Filtros
            if (!string.IsNullOrEmpty(buscar))
            {
                productos = productos.Where(p => p.Nombre.Contains(buscar) || p.Descripcion.Contains(buscar));
            }

            if (categoriaId.HasValue)
            {
                productos = productos.Where(p => p.CategoriaId == categoriaId.Value);
            }

            if (marcaId.HasValue)
            {
                productos = productos.Where(p => p.MarcaId == marcaId.Value);
            }

            if (precioMin.HasValue)
            {
                productos = productos.Where(p => p.Precio >= precioMin.Value);
            }

            if (precioMax.HasValue)
            {
                productos = productos.Where(p => p.Precio <= precioMax.Value);
            }

            // Datos para filtros
            ViewBag.Categorias = await _context.Categorias.Where(c => c.Activa).ToListAsync();
            ViewBag.Marcas = await _context.Marcas.Where(m => m.Activa).ToListAsync();
            ViewBag.Buscar = buscar;
            ViewBag.CategoriaId = categoriaId;
            ViewBag.MarcaId = marcaId;
            ViewBag.PrecioMin = precioMin;
            ViewBag.PrecioMax = precioMax;

            return View(await productos.ToListAsync());
        }

        // GET: Productos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Marca)
                .FirstOrDefaultAsync(m => m.Id == id && m.Activo);

            if (producto == null)
            {
                return NotFound();
            }

            // Productos relacionados
            var productosRelacionados = await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Marca)
                .Where(p => p.CategoriaId == producto.CategoriaId && p.Id != producto.Id && p.Activo)
                .Take(4)
                .ToListAsync();

            ViewBag.ProductosRelacionados = productosRelacionados;

            return View(producto);
        }

        // GET: Productos/Categoria/5
        public async Task<IActionResult> Categoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null || !categoria.Activa)
            {
                return NotFound();
            }

            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Marca)
                .Where(p => p.CategoriaId == id && p.Activo)
                .ToListAsync();

            ViewBag.Categoria = categoria;
            return View(productos);
        }

        // GET: Productos/Marca/5
        public async Task<IActionResult> Marca(int id)
        {
            var marca = await _context.Marcas.FindAsync(id);
            if (marca == null || !marca.Activa)
            {
                return NotFound();
            }

            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Marca)
                .Where(p => p.MarcaId == id && p.Activo)
                .ToListAsync();

            ViewBag.Marca = marca;
            return View(productos);
        }
    }
} 