namespace SeguimientoCSA.Models;

public class DashboardViewModel
{
    public int TotalActividades { get; set; }
    public int ActividadesPendientes { get; set; }
    public int ActividadesEnProgreso { get; set; }
    public int ActividadesVencidas { get; set; }
    public int ActividadesCompletadas { get; set; }
    public int TotalProveedores { get; set; }
    public int TotalMiembrosComite { get; set; }
    public decimal TotalPagos { get; set; }
    public decimal PagosMesActual { get; set; }
    public List<Actividad> ActividadesRecientes { get; set; } = [];
    public List<Pago> PagosRecientes { get; set; } = [];
}
