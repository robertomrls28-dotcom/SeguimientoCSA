using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SeguimientoCSA.Data;
using SeguimientoCSA.Models;

namespace SeguimientoCSA.Controllers;

public class ActividadesController : Controller
{
    private readonly CondominioDbContext _context;

    public ActividadesController(CondominioDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(EstadoActividad? estado, int? categoriaId)
    {
        ViewData["Title"] = "Actividades";
        ViewBag.Categorias = await _context.Categorias.ToListAsync();
        ViewBag.EstadoFiltro = estado;
        ViewBag.CategoriaFiltro = categoriaId;

        var query = _context.Actividades
            .Include(a => a.Categoria)
            .Include(a => a.Proveedor)
            .Include(a => a.Supervisor)
            .AsQueryable();

        if (estado.HasValue)
            query = query.Where(a => a.Estado == estado.Value);

        if (categoriaId.HasValue)
            query = query.Where(a => a.CategoriaId == categoriaId.Value);

        var actividades = await query.OrderByDescending(a => a.FechaCreacion).ToListAsync();
        return View(actividades);
    }

    public async Task<IActionResult> Details(int id)
    {
        var actividad = await _context.Actividades
            .Include(a => a.Categoria)
            .Include(a => a.Proveedor)
            .Include(a => a.Supervisor)
            .Include(a => a.Pagos).ThenInclude(p => p.Proveedor)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (actividad == null) return NotFound();

        ViewData["Title"] = actividad.Titulo;
        return View(actividad);
    }

    public async Task<IActionResult> Create()
    {
        ViewData["Title"] = "Nueva Actividad";
        await CargarListasDesplegables();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Actividad actividad)
    {
        ModelState.Remove(nameof(Actividad.Categoria));
        if (ModelState.IsValid)
        {
            actividad.FechaCreacion = DateTime.Now;
            _context.Actividades.Add(actividad);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Actividad creada exitosamente.";
            return RedirectToAction(nameof(Index));
        }
        await CargarListasDesplegables();
        return View(actividad);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var actividad = await _context.Actividades.FindAsync(id);
        if (actividad == null) return NotFound();

        ViewData["Title"] = "Editar Actividad";
        await CargarListasDesplegables();
        return View(actividad);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Actividad actividad)
    {
        if (id != actividad.Id) return NotFound();

        ModelState.Remove(nameof(Actividad.Categoria));
        if (ModelState.IsValid)
        {
            _context.Update(actividad);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Actividad actualizada exitosamente.";
            return RedirectToAction(nameof(Index));
        }
        await CargarListasDesplegables();
        return View(actividad);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var actividad = await _context.Actividades.FindAsync(id);
        if (actividad != null)
        {
            _context.Actividades.Remove(actividad);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Actividad eliminada.";
        }
        return RedirectToAction(nameof(Index));
    }

    private async Task CargarListasDesplegables()
    {
        ViewBag.Categorias = new SelectList(await _context.Categorias.ToListAsync(), "Id", "Nombre");
        ViewBag.Proveedores = new SelectList(await _context.Proveedores.Where(p => p.Activo).ToListAsync(), "Id", "Nombre");
        ViewBag.Supervisores = new SelectList(await _context.MiembrosComite.Where(m => m.Activo).ToListAsync(), "Id", "Nombre");
    }
}
