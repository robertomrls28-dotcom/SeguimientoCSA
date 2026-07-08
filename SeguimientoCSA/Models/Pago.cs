namespace SeguimientoCSA.Models;

public class Pago
{
    public int Id { get; set; }
    public string Concepto { get; set; } = string.Empty;
    public decimal Monto { get; set; }
    public DateTime FechaPago { get; set; } = DateTime.Now;
    public string? NumeroFactura { get; set; }
    public string? MetodoPago { get; set; }
    public string? Comprobante { get; set; }
    public string? Notas { get; set; }

    public int? ActividadId { get; set; }
    public Actividad? Actividad { get; set; }

    public int ProveedorId { get; set; }
    public Proveedor Proveedor { get; set; } = null!;
}
