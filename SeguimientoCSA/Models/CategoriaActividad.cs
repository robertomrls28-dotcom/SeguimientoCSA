namespace SeguimientoCSA.Models;

public class CategoriaActividad
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? Icono { get; set; }
    public string ColorHex { get; set; } = "#0d6efd";

    public ICollection<Actividad> Actividades { get; set; } = [];
}
