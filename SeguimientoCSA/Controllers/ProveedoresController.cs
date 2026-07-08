using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeguimientoCSA.Data;
using SeguimientoCSA.Models;

namespace SeguimientoCSA.Controllers;

public class ProveedoresController : Controller
{
    private readonly CondominioDbContext _context;

    public ProveedoresController(CondominioDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Proveedores";
        var proveedores = await _context.Proveedores
            .Include(p => p.Actividades)
            .Include(p => p.Pagos)
            .OrderBy(p => p.Nombre)
            .ToListAsync();
        return View(proveedores);
    }

    public async Task<IActionResult> Details(int id)
    {
        var proveedor = await _context.Proveedores
            .Include(p => p.Actividades).ThenInclude(a => a.Categoria)
            .Include(p => p.Pagos)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (proveedor == null) return NotFound();

        ViewData["Title"] = proveedor.Nombre;
        return View(proveedor);
    }

    public IActionResult Create()
    {
        ViewData["Title"] = "Nuevo Proveedor";
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Proveedor proveedor)
    {
        if (ModelState.IsValid)
        {
            proveedor.FechaRegistro = DateTime.Now;
            _context.Proveedores.Add(proveedor);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Proveedor registrado exitosamente.";
            return RedirectToAction(nameof(Index));
        }
        return View(proveedor);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var proveedor = await _context.Proveedores.FindAsync(id);
        if (proveedor == null) return NotFound();

        ViewData["Title"] = "Editar Proveedor";
        return View(proveedor);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Proveedor proveedor)
    {
        if (id != proveedor.Id) return NotFound();

        if (ModelState.IsValid)
        {
            _context.Update(proveedor);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Proveedor actualizado exitosamente.";
            return RedirectToAction(nameof(Index));
        }
        return View(proveedor);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var proveedor = await _context.Proveedores.FindAsync(id);
        if (proveedor != null)
        {
            _context.Proveedores.Remove(proveedor);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Proveedor eliminado.";
        }
        return RedirectToAction(nameof(Index));
    }
}
