using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SeguimientoCSA.Data;
using SeguimientoCSA.Models;

namespace SeguimientoCSA.Controllers;

public class PagosController : Controller
{
    private readonly CondominioDbContext _context;

    public PagosController(CondominioDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Pagos";
        var pagos = await _context.Pagos
            .Include(p => p.Proveedor)
            .Include(p => p.Actividad)
            .OrderByDescending(p => p.FechaPago)
            .ToListAsync();
        return View(pagos);
    }

    public async Task<IActionResult> Create()
    {
        ViewData["Title"] = "Nuevo Pago";
        await CargarListasDesplegables();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Pago pago)
    {
        ModelState.Remove(nameof(Pago.Proveedor));
        if (ModelState.IsValid)
        {
            _context.Pagos.Add(pago);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Pago registrado exitosamente.";
            return RedirectToAction(nameof(Index));
        }
        await CargarListasDesplegables();
        return View(pago);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var pago = await _context.Pagos.FindAsync(id);
        if (pago == null) return NotFound();

        ViewData["Title"] = "Editar Pago";
        await CargarListasDesplegables();
        return View(pago);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Pago pago)
    {
        if (id != pago.Id) return NotFound();

        ModelState.Remove(nameof(Pago.Proveedor));
        if (ModelState.IsValid)
        {
            _context.Update(pago);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Pago actualizado exitosamente.";
            return RedirectToAction(nameof(Index));
        }
        await CargarListasDesplegables();
        return View(pago);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var pago = await _context.Pagos.FindAsync(id);
        if (pago != null)
        {
            _context.Pagos.Remove(pago);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Pago eliminado.";
        }
        return RedirectToAction(nameof(Index));
    }

    private async Task CargarListasDesplegables()
    {
        ViewBag.Proveedores = new SelectList(await _context.Proveedores.Where(p => p.Activo).ToListAsync(), "Id", "Nombre");
        ViewBag.Actividades = new SelectList(await _context.Actividades.Where(a => a.Estado != EstadoActividad.Cancelada).ToListAsync(), "Id", "Titulo");
    }
}
