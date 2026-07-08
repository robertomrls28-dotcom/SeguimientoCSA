using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeguimientoCSA.Data;
using SeguimientoCSA.Models;

namespace SeguimientoCSA.Controllers;

public class ReportesController : Controller
{
    private readonly CondominioDbContext _context;

    public ReportesController(CondominioDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(DateTime? fechaInicio, DateTime? fechaFin, int? categoriaId, int? proveedorId, EstadoActividad? estado)
    {
        ViewData["Title"] = "Reportes";

        var queryActividades = _context.Actividades
            .Include(a => a.Categoria)
            .Include(a => a.Proveedor)
            .Include(a => a.Supervisor)
            .AsQueryable();

        var queryPagos = _context.Pagos
            .Include(p => p.Proveedor)
            .Include(p => p.Actividad)
            .AsQueryable();

        if (fechaInicio.HasValue)
        {
            queryActividades = queryActividades.Where(a => a.FechaCreacion >= fechaInicio.Value);
            queryPagos = queryPagos.Where(p => p.FechaPago >= fechaInicio.Value);
        }

        if (fechaFin.HasValue)
        {
            var fin = fechaFin.Value.AddDays(1);
            queryActividades = queryActividades.Where(a => a.FechaCreacion < fin);
            queryPagos = queryPagos.Where(p => p.FechaPago < fin);
        }

        if (categoriaId.HasValue)
            queryActividades = queryActividades.Where(a => a.CategoriaId == categoriaId.Value);

        if (proveedorId.HasValue)
        {
            queryActividades = queryActividades.Where(a => a.ProveedorId == proveedorId.Value);
            queryPagos = queryPagos.Where(p => p.ProveedorId == proveedorId.Value);
        }

        if (estado.HasValue)
            queryActividades = queryActividades.Where(a => a.Estado == estado.Value);

        var actividades = await queryActividades.OrderByDescending(a => a.FechaCreacion).ToListAsync();
        var pagos = await queryPagos.OrderByDescending(p => p.FechaPago).ToListAsync();

        var model = new ReporteViewModel
        {
            FechaInicio = fechaInicio,
            FechaFin = fechaFin,
            CategoriaId = categoriaId,
            ProveedorId = proveedorId,
            Estado = estado,
            Actividades = actividades,
            Pagos = pagos,
            Categorias = await _context.Categorias.ToListAsync(),
            Proveedores = await _context.Proveedores.OrderBy(p => p.Nombre).ToListAsync(),
            TotalCostoEstimado = actividades.Sum(a => a.CostoEstimado ?? 0),
            TotalCostoReal = actividades.Sum(a => a.CostoReal ?? 0),
            TotalPagos = pagos.Sum(p => p.Monto),
            TotalActividadesCompletadas = actividades.Count(a => a.Estado == EstadoActividad.Completada),
            TotalActividadesPendientes = actividades.Count(a => a.Estado == EstadoActividad.Pendiente)
        };

        return View(model);
    }
}
