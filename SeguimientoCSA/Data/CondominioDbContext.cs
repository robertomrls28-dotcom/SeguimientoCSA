using Microsoft.EntityFrameworkCore;
using SeguimientoCSA.Models;

namespace SeguimientoCSA.Data;

public class CondominioDbContext : DbContext
{
    public CondominioDbContext(DbContextOptions<CondominioDbContext> options) : base(options) { }

    public DbSet<Actividad> Actividades => Set<Actividad>();
    public DbSet<CategoriaActividad> Categorias => Set<CategoriaActividad>();
    public DbSet<Proveedor> Proveedores => Set<Proveedor>();
    public DbSet<MiembroComite> MiembrosComite => Set<MiembroComite>();
    public DbSet<Pago> Pagos => Set<Pago>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Actividad>(entity =>
        {
            entity.HasOne(a => a.Categoria)
                  .WithMany(c => c.Actividades)
                  .HasForeignKey(a => a.CategoriaId);

            entity.HasOne(a => a.Proveedor)
                  .WithMany(p => p.Actividades)
                  .HasForeignKey(a => a.ProveedorId)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(a => a.Supervisor)
                  .WithMany(m => m.ActividadesSupervisadas)
                  .HasForeignKey(a => a.SupervisorId)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.Property(a => a.CostoEstimado).HasColumnType("decimal(18,2)");
            entity.Property(a => a.CostoReal).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasOne(p => p.Actividad)
                  .WithMany(a => a.Pagos)
                  .HasForeignKey(p => p.ActividadId)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(p => p.Proveedor)
                  .WithMany(p => p.Pagos)
                  .HasForeignKey(p => p.ProveedorId);

            entity.Property(p => p.Monto).HasColumnType("decimal(18,2)");
        });

        // Datos semilla
        modelBuilder.Entity<CategoriaActividad>().HasData(
            new CategoriaActividad { Id = 1, Nombre = "Mantenimiento", Descripcion = "Actividades de mantenimiento general", Icono = "bi-wrench", ColorHex = "#0d6efd" },
            new CategoriaActividad { Id = 2, Nombre = "Limpieza", Descripcion = "Servicios de limpieza y jardinería", Icono = "bi-droplet", ColorHex = "#198754" },
            new CategoriaActividad { Id = 3, Nombre = "Seguridad", Descripcion = "Vigilancia y sistemas de seguridad", Icono = "bi-shield-check", ColorHex = "#dc3545" },
            new CategoriaActividad { Id = 4, Nombre = "Reparaciones", Descripcion = "Reparaciones de infraestructura", Icono = "bi-tools", ColorHex = "#fd7e14" },
            new CategoriaActividad { Id = 5, Nombre = "Administración", Descripcion = "Gestión administrativa del condominio", Icono = "bi-clipboard-data", ColorHex = "#6f42c1" },
            new CategoriaActividad { Id = 6, Nombre = "Eventos", Descripcion = "Asambleas y eventos del condominio", Icono = "bi-calendar-event", ColorHex = "#20c997" }
        );

        modelBuilder.Entity<MiembroComite>().HasData(
            new MiembroComite { Id = 1, Nombre = "Carlos Mendoza", Cargo = "Presidente del Comité", Telefono = "555-100-2000", Email = "carlos.mendoza@correo.com", NumeroDepto = "A-101", FechaInicio = new DateTime(2024, 1, 15), Activo = true },
            new MiembroComite { Id = 2, Nombre = "María López", Cargo = "Secretaria del Comité", Telefono = "555-200-3000", Email = "maria.lopez@correo.com", NumeroDepto = "B-203", FechaInicio = new DateTime(2024, 1, 15), Activo = true },
            new MiembroComite { Id = 3, Nombre = "Jorge Ramírez", Cargo = "Tesorero del Comité", Telefono = "555-300-4000", Email = "jorge.ramirez@correo.com", NumeroDepto = "C-305", FechaInicio = new DateTime(2024, 1, 15), Activo = true }
        );

        modelBuilder.Entity<Proveedor>().HasData(
            new Proveedor { Id = 1, Nombre = "Limpieza Total S.A.", Contacto = "Roberto Sánchez", Telefono = "555-111-2222", Email = "contacto@limpiezatotal.com", Especialidad = "Limpieza", Activo = true, FechaRegistro = new DateTime(2024, 2, 1) },
            new Proveedor { Id = 2, Nombre = "Electro Servicios MX", Contacto = "Ana Torres", Telefono = "555-333-4444", Email = "info@electroservicios.com", Especialidad = "Electricidad", Activo = true, FechaRegistro = new DateTime(2024, 2, 15) },
            new Proveedor { Id = 3, Nombre = "Jardines Verdes", Contacto = "Pedro Flores", Telefono = "555-555-6666", Email = "ventas@jardinesverdes.com", Especialidad = "Jardinería", Activo = true, FechaRegistro = new DateTime(2024, 3, 1) },
            new Proveedor { Id = 4, Nombre = "Seguridad Integral", Contacto = "Luis Herrera", Telefono = "555-777-8888", Email = "info@seguridadintegral.com", Especialidad = "Vigilancia", Activo = true, FechaRegistro = new DateTime(2024, 3, 10) }
        );

        modelBuilder.Entity<Actividad>().HasData(
            new Actividad { Id = 1, Titulo = "Mantenimiento de elevadores", Descripcion = "Servicio trimestral de mantenimiento preventivo a los 3 elevadores", FechaCreacion = new DateTime(2024, 6, 1), FechaInicio = new DateTime(2024, 6, 5), Estado = EstadoActividad.EnProgreso, Prioridad = 1, CostoEstimado = 15000, CategoriaId = 1, ProveedorId = 2, SupervisorId = 1, CreadoPor = "Administrador" },
            new Actividad { Id = 2, Titulo = "Limpieza profunda de áreas comunes", Descripcion = "Limpieza profunda de lobby, pasillos y estacionamiento", FechaCreacion = new DateTime(2024, 6, 10), FechaInicio = new DateTime(2024, 6, 12), FechaFin = new DateTime(2024, 6, 12), Estado = EstadoActividad.Completada, Prioridad = 2, CostoEstimado = 8000, CostoReal = 7500, CategoriaId = 2, ProveedorId = 1, SupervisorId = 2, CreadoPor = "Administrador" },
            new Actividad { Id = 3, Titulo = "Reparación de cerca perimetral", Descripcion = "Reparar sección dañada de la cerca del lado norte", FechaCreacion = new DateTime(2024, 6, 15), Estado = EstadoActividad.Pendiente, Prioridad = 1, CostoEstimado = 25000, CategoriaId = 4, SupervisorId = 3, CreadoPor = "Comité de Vigilancia" },
            new Actividad { Id = 4, Titulo = "Poda de árboles y jardinería", Descripcion = "Poda trimestral de árboles y mantenimiento de jardines", FechaCreacion = new DateTime(2024, 6, 20), FechaInicio = new DateTime(2024, 6, 25), Estado = EstadoActividad.EnProgreso, Prioridad = 3, CostoEstimado = 5000, CategoriaId = 2, ProveedorId = 3, SupervisorId = 1, CreadoPor = "Administrador" },
            new Actividad { Id = 5, Titulo = "Instalación de cámaras nuevas", Descripcion = "Instalar 4 cámaras HD en estacionamiento subterráneo", FechaCreacion = new DateTime(2024, 7, 1), Estado = EstadoActividad.Pendiente, Prioridad = 1, CostoEstimado = 35000, CategoriaId = 3, ProveedorId = 4, SupervisorId = 3, CreadoPor = "Comité de Vigilancia" }
        );

        modelBuilder.Entity<Pago>().HasData(
            new Pago { Id = 1, Concepto = "Anticipo mantenimiento elevadores", Monto = 7500, FechaPago = new DateTime(2024, 6, 5), NumeroFactura = "FAC-001", MetodoPago = "Transferencia", ActividadId = 1, ProveedorId = 2 },
            new Pago { Id = 2, Concepto = "Limpieza profunda áreas comunes", Monto = 7500, FechaPago = new DateTime(2024, 6, 12), NumeroFactura = "FAC-002", MetodoPago = "Cheque", ActividadId = 2, ProveedorId = 1 },
            new Pago { Id = 3, Concepto = "Anticipo poda y jardinería", Monto = 2500, FechaPago = new DateTime(2024, 6, 25), NumeroFactura = "FAC-003", MetodoPago = "Transferencia", ActividadId = 4, ProveedorId = 3 }
        );
    }
}
