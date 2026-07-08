namespace SeguimientoCSA.Models;

public class Actividad
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public DateTime FechaCreacion { get; set; } = DateTime.Now;
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public EstadoActividad Estado { get; set; } = EstadoActividad.Pendiente;
    public int Prioridad { get; set; } = 2; // 1=Alta, 2=Media, 3=Baja
    public decimal? CostoEstimado { get; set; }
    public decimal? CostoReal { get; set; }
    public string? Observaciones { get; set; }
    public string? CreadoPor { get; set; }

    public int CategoriaId { get; set; }
    public CategoriaActividad Categoria { get; set; } = null!;

    public int? ProveedorId { get; set; }
    public Proveedor? Proveedor { get; set; }

    public int? SupervisorId { get; set; }
    public MiembroComite? Supervisor { get; set; }

    public ICollection<Pago> Pagos { get; set; } = [];
}
