namespace SeguimientoCSA.Models;

public class MiembroComite
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Cargo { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public string? NumeroDepto { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public bool Activo { get; set; } = true;
    public string? Notas { get; set; }

    public ICollection<Actividad> ActividadesSupervisadas { get; set; } = [];
}
