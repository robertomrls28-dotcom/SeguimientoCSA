# SeguimientoCSA - Sistema de Gestión de Condominio

Aplicación web para dar seguimiento a las actividades del administrador, proveedores y comité de vigilancia de un condominio.

## Módulos

- **Dashboard** — Resumen general con estadísticas y actividad reciente
- **Actividades** — Gestión de actividades (mantenimiento, limpieza, reparaciones, etc.)
- **Proveedores** — Directorio de proveedores de servicios
- **Pagos** — Registro y control de pagos a proveedores
- **Comité de Vigilancia** — Gestión de miembros del comité
- **Reportes** — Reportes con filtros por fecha, categoría, proveedor y estado

## Tecnologías

- ASP.NET Core MVC (.NET 10)
- Entity Framework Core con SQLite
- Bootstrap 5

## Requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

## Cómo ejecutar

```bash
cd SeguimientoCSA
dotnet run
```

Abrir en el navegador: **http://localhost:5000**

La base de datos se crea automáticamente con datos de ejemplo.
