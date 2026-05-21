# Guía de Ejecución de Pruebas - HotelCc.Tests

## 🚀 Ejecución Rápida

### Ejecutar todas las pruebas
```powershell
dotnet test HotelCc.Tests\HotelCc.Tests.csproj
```

### Ejecutar pruebas con salida detallada
```powershell
dotnet test HotelCc.Tests\HotelCc.Tests.csproj --verbosity=normal
```

### Ejecutar pruebas de una categoría específica
```powershell
dotnet test HotelCc.Tests --filter "Category=HabitacionModel"
dotnet test HotelCc.Tests --filter "Category=UsuarioModel"
dotnet test HotelCc.Tests --filter "Category=ReservaModel"
dotnet test HotelCc.Tests --filter "Category=Integration"
```

## 📋 Categorías de Pruebas Disponibles

### Modelos
- `HabitacionModel` - 10 pruebas
- `UsuarioModel` - 10 pruebas
- `ReservaModel` - 10 pruebas

### Base de Datos
- `AppDbContext` - 6 pruebas

### Controladores
- `HabitacionesController` - 6 pruebas
- `AuthController` - 7 pruebas
- `ReservasController` - 7 pruebas

### ViewModels
- `LoginViewModel` - 7 pruebas
- `HabitacionEstadoViewModel` - 8 pruebas

### Integración
- `Integration` - 8 pruebas

### Validación
- `DataValidation` - 8 pruebas

### Casos Límite
- `EdgeCases` - 8 pruebas

### Rendimiento
- `Performance` - 5 pruebas

## 📊 Ejemplos de Ejecución

### Ejecutar solo pruebas de modelos
```powershell
dotnet test HotelCc.Tests --filter "Category=HabitacionModel|Category=UsuarioModel|Category=ReservaModel"
```

### Ejecutar solo pruebas de controladores
```powershell
dotnet test HotelCc.Tests --filter "Category=HabitacionesController|Category=AuthController|Category=ReservasController"
```

### Ejecutar solo pruebas de integración
```powershell
dotnet test HotelCc.Tests --filter "Category=Integration"
```

### Ejecutar solo pruebas de rendimiento
```powershell
dotnet test HotelCc.Tests --filter "Category=Performance"
```

### Ejecutar solo pruebas que hayan fallado
```powershell
dotnet test HotelCc.Tests --filter "Outcome=Failed"
```

## 🔍 Opciones Avanzadas

### Ejecutar con reporte de cobertura
```powershell
dotnet test HotelCc.Tests --collect:"XPlat Code Coverage"
```

### Ejecutar solo pruebas por nombre
```powershell
dotnet test HotelCc.Tests --filter "FullyQualifiedName~HotelCc.Tests.Models.HabitacionModelTests"
```

### Ejecutar pruebas en paralelo
```powershell
dotnet test HotelCc.Tests --parallel
```

### Ejecutar con Logger personalizado
```powershell
dotnet test HotelCc.Tests --logger:"console;verbosity=detailed"
```

## 📁 Estructura de Carpetas del Proyecto

```
HotelCc.Tests/
├── Controllers/
│   ├── AuthControllerTests.cs
│   ├── HabitacionesControllerTests.cs
│   └── ReservasControllerTests.cs
├── Data/
│   └── AppDbContextTests.cs
├── EdgeCases/
│   └── EdgeCaseTests.cs
├── Helpers/
│   └── TestDatabaseHelper.cs
├── Integration/
│   └── HabitacionReservaIntegrationTests.cs
├── Models/
│   ├── HabitacionModelTests.cs
│   ├── ReservaModelTests.cs
│   └── UsuarioModelTests.cs
├── Performance/
│   └── BulkOperationTests.cs
├── Validation/
│   └── DataValidationTests.cs
├── ViewModels/
│   ├── HabitacionEstadoViewModelTests.cs
│   └── LoginViewModelTests.cs
├── HotelCc.Tests.csproj
├── README.md
└── TEST_SUMMARY.md
```

## ✅ Verificación de Compilación

Antes de ejecutar las pruebas, asegúrate de compilar el proyecto:

```powershell
dotnet build HotelCc.Tests\HotelCc.Tests.csproj
```

## 📦 Dependencias Requeridas

El proyecto de pruebas incluye las siguientes dependencias:
- **MSTest.TestFramework** v3.2.2
- **Microsoft.NET.Test.Sdk** v17.10.0
- **MSTest.TestAdapter** v3.2.2
- **Moq** v4.20.70 (preparada para uso)
- **Microsoft.EntityFrameworkCore** v10.0.0
- **Microsoft.EntityFrameworkCore.InMemory** v10.0.0

## 🎯 Casos de Uso Comunes

### Para desarrollo local
```powershell
# Ejecutar solo pruebas rápidas (models, validation)
dotnet test HotelCc.Tests --filter "Category=HabitacionModel|Category=UsuarioModel|Category=ReservaModel|Category=DataValidation"
```

### Antes de hacer commit
```powershell
# Ejecutar todas las pruebas
dotnet test HotelCc.Tests
```

### Validar controladores
```powershell
# Ejecutar pruebas de controladores
dotnet test HotelCc.Tests --filter "Category=HabitacionesController|Category=AuthController|Category=ReservasController"
```

### Validar integración
```powershell
# Ejecutar pruebas de integración
dotnet test HotelCc.Tests --filter "Category=Integration"
```

## 🔧 Solución de Problemas

### Las pruebas no se encuentran
```powershell
# Reconstruir el proyecto
dotnet clean HotelCc.Tests
dotnet build HotelCc.Tests
```

### Error de dependencias
```powershell
# Restaurar paquetes NuGet
dotnet restore HotelCc.Tests
```

### Ejecutar una prueba específica
```powershell
# Buscar por nombre de prueba
dotnet test HotelCc.Tests --filter "Habitacion_Creation_WithValidData_ShouldSucceed"
```

## 📈 Total de Pruebas: 104

Con esta cobertura de pruebas, tienes una base sólida para:
- ✅ Validar lógica de modelos
- ✅ Verificar operaciones de base de datos
- ✅ Probar acciones de controladores
- ✅ Validar flujos de integración
- ✅ Detectar casos límite
- ✅ Asegurar rendimiento
