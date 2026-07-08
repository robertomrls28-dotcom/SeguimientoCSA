namespace SeguimientoCSA.Models;

public class ReporteViewModel
{
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public int? CategoriaId { get; set; }
    public int? ProveedorId { get; set; }
    public EstadoActividad? Estado { get; set; }

    public List<Actividad> Actividades { get; set; } = [];
    public List<Pago> Pagos { get; set; } = [];
    public List<CategoriaActividad> Categorias { get; set; } = [];
    public List<Proveedor> Proveedores { get; set; } = [];

    public decimal TotalCostoEstimado { get; set; }
    public decimal TotalCostoReal { get; set; }
    public decimal TotalPagos { get; set; }
    public int TotalActividadesCompletadas { get; set; }
    public int TotalActividadesPendientes { get; set; }
}
