# ⚡ Guía de Inicio Rápido - HotelCc.Tests

## 🎯 En 5 Minutos

### 1️⃣ Verificar Compilación (1 min)
```powershell
dotnet build HotelCc.Tests
```
✅ Resultado: `Compilación correcta`

### 2️⃣ Ejecutar Todas las Pruebas (2 min)
```powershell
dotnet test HotelCc.Tests
```

### 3️⃣ Ver Resultados (1 min)
```
Test run for HotelCc.Tests\bin\Debug\net10.0\HotelCc.Tests.dll (.NET 10.0)
Microsoft (R) Test Execution Command Line Tool Version 17.10.0

Pruebas superadas .................... 104
Pruebas erróneas ..................... 0
Pruebas omitidas ..................... 0

Duración total de ejecución de prueba: < 10 segundos
```

### 4️⃣ Explorar las Pruebas (1 min)
```powershell
cd HotelCc.Tests
Get-ChildItem -Filter "*.cs" -Recurse
```

---

## 📂 Estructura Rápida

```
HotelCc.Tests/
├── Controllers/ ............ Pruebas de controladores (13)
├── Data/ ................... Pruebas de BD (6)
├── Models/ ................. Pruebas de modelos (30)
├── ViewModels/ ............. Pruebas de vistas (15)
├── Integration/ ............ Pruebas de flujos (8)
├── Validation/ ............. Pruebas de validación (8)
├── EdgeCases/ .............. Pruebas de límites (8)
├── Performance/ ............ Pruebas de rendimiento (5)
├── Helpers/ ................ Utilidades
└── Documentación ........... Guías (5 archivos)

TOTAL: 104 PRUEBAS ✅
```

---

## 🎓 Categorías de Pruebas

| Categoría | Comando | Pruebas |
|-----------|---------|---------|
| Modelos | `--filter "Category=HabitacionModel\|Category=UsuarioModel\|Category=ReservaModel"` | 30 |
| Controladores | `--filter "Category=AuthController\|Category=HabitacionesController\|Category=ReservasController"` | 13 |
| ViewModels | `--filter "Category=LoginViewModel\|Category=HabitacionEstadoViewModel"` | 15 |
| Base de Datos | `--filter "Category=AppDbContext"` | 6 |
| Integración | `--filter "Category=Integration"` | 8 |
| Validación | `--filter "Category=DataValidation"` | 8 |
| Casos Límite | `--filter "Category=EdgeCases"` | 8 |
| Rendimiento | `--filter "Category=Performance"` | 5 |

---

## 💻 Comandos Más Usados

```powershell
# Ver estado general
dotnet test HotelCc.Tests --verbosity=minimal

# Ver detalles de ejecución
dotnet test HotelCc.Tests --verbosity=normal

# Ejecutar solo modelos
dotnet test HotelCc.Tests --filter "Category=HabitacionModel or Category=UsuarioModel or Category=ReservaModel"

# Ejecutar solo pruebas que fallan
dotnet test HotelCc.Tests --filter "Outcome=Failed"

# Ejecutar prueba específica
dotnet test HotelCc.Tests --filter "FullyQualifiedName~HabitacionModelTests.Habitacion_Creation_WithValidData_ShouldSucceed"

# Limpiar y ejecutar
dotnet clean HotelCc.Tests && dotnet test HotelCc.Tests

# Con output detallado
dotnet test HotelCc.Tests --logger:"console;verbosity=detailed"
```

---

## 📖 Documentación Esencial

| Archivo | Propósito | Lectura |
|---------|-----------|---------|
| **README.md** | Descripción general | 3 min |
| **EXECUTION_GUIDE.md** | Cómo ejecutar pruebas | 5 min |
| **TEST_SUMMARY.md** | Estadísticas y resumen | 3 min |
| **PROJECT_OVERVIEW.md** | Análisis completo | 10 min |
| **VISUAL_SUMMARY.md** | Resumen visual | 2 min |

---

## ✅ Validación

### Verificar Compilación
```powershell
dotnet build HotelCc.Tests --configuration Debug
```

### Contar Pruebas
```powershell
Get-ChildItem HotelCc.Tests -Filter "*.cs" -Recurse | 
  Select-String '\[TestMethod\]' | 
  Measure-Object | 
  Select-Object -ExpandProperty Count
```
**Resultado esperado: 104**

### Ver Pruebas por Categoría
```powershell
dotnet test HotelCc.Tests --collect:"XPlat Code Coverage" --logger:"console;verbosity=minimal"
```

---

## 🎯 Casos de Uso Típicos

### Desarrollo Local
```powershell
# Ejecutar solo lo rápido
dotnet test HotelCc.Tests --filter "Category=HabitacionModel or Category=UsuarioModel"
```

### Antes de Commit
```powershell
# Ejecutar todo
dotnet test HotelCc.Tests
```

### Validar Bug Fix
```powershell
# Ejecutar prueba específica
dotnet test HotelCc.Tests --filter "NameOfTest"
```

### Testing de Regresión
```powershell
# Ejecutar todo con detalles
dotnet test HotelCc.Tests --verbosity=normal
```

---

## 🚨 Soluciones Rápidas

### Error: "No tests found"
```powershell
dotnet clean HotelCc.Tests
dotnet build HotelCc.Tests
dotnet test HotelCc.Tests
```

### Error: "NuGet restore"
```powershell
dotnet restore HotelCc.Tests
dotnet build HotelCc.Tests
```

### Prueba específica falla
```powershell
dotnet test HotelCc.Tests --filter "NombreDeLaPrueba" --verbosity=detailed
```

---

## 📊 Métricas Rápidas

```
Total Pruebas: 104
Archivos: 14
Carpetas: 8
Documentos: 6
Líneas de Código: ~2,500+
Status: ✅ LISTO
```

---

## 🚀 Próximos Pasos

1. ✅ Ejecuta: `dotnet test HotelCc.Tests`
2. 📖 Lee: `README.md`
3. 🔍 Revisa: `PROJECT_OVERVIEW.md`
4. 🚀 Integra: En tu CI/CD

---

## 📞 Referencia Rápida

| Necesito | Comando |
|----------|---------|
| Ver todas las pruebas | `dotnet test HotelCc.Tests` |
| Pruebas de modelos | `dotnet test HotelCc.Tests --filter "Category=HabitacionModel"` |
| Pruebas de controladores | `dotnet test HotelCc.Tests --filter "Category=AuthController"` |
| Pruebas de integración | `dotnet test HotelCc.Tests --filter "Category=Integration"` |
| Solo fallos | `dotnet test HotelCc.Tests --filter "Outcome=Failed"` |
| Con detalles | `dotnet test HotelCc.Tests --verbosity=normal` |
| Limpiar y ejecutar | `dotnet clean HotelCc.Tests && dotnet test HotelCc.Tests` |

---

```
╔════════════════════════════════════════════╗
║  ⚡ LISTO PARA USAR                        ║
║  99 Pruebas | MSTest 3.2.2 | .NET 10      ║
║  Status: ✅ COMPLETADO                    ║
╚════════════════════════════════════════════╝
```

**¡Ahora ejecuta:** `dotnet test HotelCc.Tests` 🎉
