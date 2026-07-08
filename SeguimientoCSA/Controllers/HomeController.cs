using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeguimientoCSA.Data;
using SeguimientoCSA.Models;

namespace SeguimientoCSA.Controllers;

public class HomeController : Controller
{
    private readonly CondominioDbContext _context;

    public HomeController(CondominioDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var now = DateTime.Now;
        var today = DateTime.Today;
        var inicioMes = new DateTime(now.Year, now.Month, 1);

        var model = new DashboardViewModel
        {
            TotalActividades = await _context.Actividades.CountAsync(),
            ActividadesPendientes = await _context.Actividades.CountAsync(a => a.Estado == EstadoActividad.Pendiente),
            ActividadesEnProgreso = await _context.Actividades.CountAsync(a => a.Estado == EstadoActividad.EnProgreso),
            ActividadesVencidas = await _context.Actividades.CountAsync(a =>
                a.FechaFin.HasValue
                && a.FechaFin.Value.Date < today
                && a.Estado != EstadoActividad.Completada
                && a.Estado != EstadoActividad.Cancelada),
            ActividadesCompletadas = await _context.Actividades.CountAsync(a => a.Estado == EstadoActividad.Completada),
            TotalProveedores = await _context.Proveedores.CountAsync(p => p.Activo),
            TotalMiembrosComite = await _context.MiembrosComite.CountAsync(m => m.Activo),
            TotalPagos = await _context.Pagos.SumAsync(p => p.Monto),
            PagosMesActual = await _context.Pagos.Where(p => p.FechaPago >= inicioMes).SumAsync(p => p.Monto),
            ActividadesRecientes = await _context.Actividades
                .Include(a => a.Categoria)
                .Include(a => a.Proveedor)
                .Include(a => a.Supervisor)
                .OrderByDescending(a => a.FechaCreacion)
                .Take(5)
                .ToListAsync(),
            PagosRecientes = await _context.Pagos
                .Include(p => p.Proveedor)
                .Include(p => p.Actividad)
                .OrderByDescending(p => p.FechaPago)
                .Take(5)
                .ToListAsync()
        };

        ViewData["Title"] = "Dashboard";
        return View(model);
    }
}
