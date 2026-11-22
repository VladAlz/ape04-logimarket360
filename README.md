# LogiMarket 360

Aplicación de escritorio (Windows Forms, .NET 8) para ventas y logística de accesorios de computación con kits anidados.
APE 04 de Patrones de Software: Clean Architecture, SOLID y los 23 patrones GoF.

## Requisitos

- Windows 10/11 con el SDK de .NET 8 (8.0.1xx o superior; también compila con el SDK 10 instalado en paralelo).
- Visual Studio 2022 o posterior con la carga de trabajo "Desarrollo de escritorio de .NET".
- Conexión a nuget.org la primera vez, para restaurar los paquetes.

Paquetes usados: `Microsoft.EntityFrameworkCore.Sqlite` 8.0.10, `Microsoft.EntityFrameworkCore.Design` 8.0.10,
`Microsoft.Extensions.DependencyInjection` 8.0.1, `xunit` 2.9.2, `xunit.runner.visualstudio` 2.8.2 y `Microsoft.NET.Test.Sdk` 17.11.1.

## Estructura

| Proyecto | Referencias | Contenido |
|---|---|---|
| LogiMarket.Domain | ninguna | Entidades, estados, Composite del catálogo, políticas, fábricas de entrega |
| LogiMarket.Application | Domain | Casos de uso, puertos, DTO, cadena de validación, fachada, comandos, eventos |
| LogiMarket.Infrastructure | Application, Domain | EF Core + SQLite, migraciones, repositorios, pasarelas simuladas, Outbox, salidas |
| LogiMarket.WinForms | Application, Domain, Infrastructure (solo registro DI) | Pantallas, Mediator y composition root |
| LogiMarket.Tests | Domain, Application, Infrastructure | Pruebas xUnit con base SQLite aislada |

## Compilar y ejecutar

```bash
dotnet restore LogiMarket360.slnx
dotnet build LogiMarket360.slnx
dotnet run --project LogiMarket.WinForms
```

En Visual Studio: abrir `LogiMarket360.slnx`, dejar `LogiMarket.WinForms` como proyecto de inicio y presionar F5.

Al arrancar, la aplicación aplica las migraciones y carga la semilla (no duplica datos si se vuelve a ejecutar).
La base queda en `%LOCALAPPDATA%\LogiMarket360\logimarket.db`; los comprobantes TXT en `...\LogiMarket360\comprobantes`
y los avisos simulados en `...\LogiMarket360\avisos` (`email.log`, `sms.log`).

## Cuentas de demostración

Datos ficticios, solo para pruebas locales. Las contraseñas se guardan con PBKDF2-SHA256.

| Usuario | Contraseña | Rol |
|---|---|---|
| admin | Admin#2026 | ADMIN |
| ventas | Ventas#2026 | VENTAS |
| bodega | Bodega#2026 | BODEGA |
| consulta | Consulta#2026 | CONSULTA |

Clientes de prueba: Ana Torres (VIP), Luis Mena (NORMAL) y Carmen Ruiz (MAYORISTA).

## Pruebas automatizadas

```bash
dotnet test LogiMarket360.slnx
```

Cada prueba crea su propia base SQLite en una carpeta temporal, así que no toca la base de la aplicación.

## Restablecer los datos de prueba

Para volver al catálogo inicial (stock 8/5/20/4/30, sin pedidos):

```bash
dotnet run --project LogiMarket.WinForms -- --reiniciar-datos
```

En Visual Studio se puede usar el perfil de inicio "Base de prueba reiniciada". También basta con cerrar la aplicación
y borrar la carpeta `%LOCALAPPDATA%\LogiMarket360`. Con `--datos=RUTA` se usa otra carpeta de datos.

## Migraciones

La migración inicial está en `LogiMarket.Infrastructure/Persistencia/Migraciones`. Para crear una nueva:

```bash
dotnet tool install --global dotnet-ef --version 8.0.10
dotnet ef migrations add NombreMigracion --project LogiMarket.Infrastructure --startup-project LogiMarket.Infrastructure --output-dir Persistencia/Migraciones
```

## Notas

- El cobro, el correo y el SMS son simulados; no salen del equipo.
- Los comprobantes son internos de demostración, sin validez tributaria.
- La pestaña "Auditoría y demostración" permite cambiar el proveedor de pago (LEGADO / NOVAPAY), forzar un rechazo de cobro
  y un fallo del notificador para probar el Outbox. Esas opciones no cambian las reglas de negocio.
