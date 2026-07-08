using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeguimientoCSA.Data;
using SeguimientoCSA.Models;

namespace SeguimientoCSA.Controllers;

public class ComiteController : Controller
{
    private readonly CondominioDbContext _context;

    public ComiteController(CondominioDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Comité de Vigilancia";
        var miembros = await _context.MiembrosComite
            .Include(m => m.ActividadesSupervisadas)
            .OrderBy(m => m.Nombre)
            .ToListAsync();
        return View(miembros);
    }

    public async Task<IActionResult> Details(int id)
    {
        var miembro = await _context.MiembrosComite
            .Include(m => m.ActividadesSupervisadas).ThenInclude(a => a.Categoria)
            .Include(m => m.ActividadesSupervisadas).ThenInclude(a => a.Proveedor)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (miembro == null) return NotFound();

        ViewData["Title"] = miembro.Nombre;
        return View(miembro);
    }

    public IActionResult Create()
    {
        ViewData["Title"] = "Nuevo Miembro del Comité";
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MiembroComite miembro)
    {
        if (ModelState.IsValid)
        {
            _context.MiembrosComite.Add(miembro);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Miembro del comité registrado exitosamente.";
            return RedirectToAction(nameof(Index));
        }
        return View(miembro);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var miembro = await _context.MiembrosComite.FindAsync(id);
        if (miembro == null) return NotFound();

        ViewData["Title"] = "Editar Miembro";
        return View(miembro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, MiembroComite miembro)
    {
        if (id != miembro.Id) return NotFound();

        if (ModelState.IsValid)
        {
            _context.Update(miembro);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Miembro actualizado exitosamente.";
            return RedirectToAction(nameof(Index));
        }
        return View(miembro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var miembro = await _context.MiembrosComite.FindAsync(id);
        if (miembro != null)
        {
            _context.MiembrosComite.Remove(miembro);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Miembro eliminado.";
        }
        return RedirectToAction(nameof(Index));
    }
}
