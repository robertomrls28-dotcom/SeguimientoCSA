namespace SeguimientoCSA.Models;

public class Proveedor
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Contacto { get; set; }
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public string? Especialidad { get; set; }
    public string? Direccion { get; set; }
    public string? RFC { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime FechaRegistro { get; set; } = DateTime.Now;
    public string? Notas { get; set; }

    public ICollection<Actividad> Actividades { get; set; } = [];
    public ICollection<Pago> Pagos { get; set; } = [];
}
