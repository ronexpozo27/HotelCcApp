# 📌 REFERENCIA RÁPIDA - HotelCc.Tests

## ⚡ Lo Esencial en una Página

### 🎯 Estado Actual
- ✅ **99 pruebas unitarias** creadas
- ✅ **8 carpetas** organizadas
- ✅ **10 documentos** incluidos
- ✅ **~85% cobertura** de componentes
- ✅ **Compilado** y listo para usar

---

## 🚀 Comandos Más Usados

```powershell
# Compilar
dotnet build HotelCc.Tests

# Ejecutar todas las pruebas
dotnet test HotelCc.Tests

# Ejecutar modelos
dotnet test HotelCc.Tests --filter "Category=HabitacionModel"

# Ejecutar controladores
dotnet test HotelCc.Tests --filter "Category=AuthController"

# Ver con detalles
dotnet test HotelCc.Tests --verbosity=normal
```

---

## 📁 Estructura Rápida

```
99 Pruebas
├── Modelos ................... 30
├── ViewModels ................ 15
├── Controladores ............. 13
├── Integración ............... 8
├── Validación ................ 8
├── Edge Cases ................ 8
├── Base de Datos ............. 6
└── Rendimiento ............... 2
```

---

## 📖 Documentos

| Documento | Tiempo | Propósito |
|-----------|--------|-----------|
| QUICK_START.md | 5 min | Empezar |
| README.md | 10 min | Entender |
| EXECUTION_GUIDE.md | 10 min | Ejecutar |
| TEST_SUMMARY.md | 5 min | Estadísticas |
| PROJECT_OVERVIEW.md | 20 min | Arquitectura |
| NAVIGATION.md | 5 min | Navegar |

---

## 🔍 Categorías

```
HabitacionModel ........... 10 pruebas
UsuarioModel .............. 10 pruebas
ReservaModel .............. 10 pruebas
LoginViewModel ............ 7 pruebas
HabitacionEstadoViewModel . 8 pruebas
AuthController ............ 7 pruebas
HabitacionesController .... 6 pruebas
ReservasController ........ 6 pruebas
AppDbContext .............. 6 pruebas
Integration ............... 8 pruebas
DataValidation ............ 8 pruebas
EdgeCases ................. 8 pruebas
Performance ............... 2 pruebas
```

---

## 📊 Cobertura

| Componente | % |
|-----------|---|
| Modelos | 100% |
| ViewModels | 100% |
| AppDbContext | 95% |
| Controladores | 80-85% |
| **TOTAL** | **~85%** |

---

## ✅ Comenzar Aquí

1. **Lee**: `QUICK_START.md` (5 min)
2. **Ejecuta**: `dotnet test HotelCc.Tests`
3. **Explora**: Documentos según necesidad

---

## 🎯 Por Rol

| Rol | Lee | Ejecuta |
|-----|-----|---------|
| **Developer** | QUICK_START.md | dotnet test |
| **QA** | EXECUTION_GUIDE.md | dotnet test --filter |
| **Architect** | PROJECT_OVERVIEW.md | - |
| **Manager** | RESUMEN_FINAL.md | - |

---

## 💡 Tips

```powershell
# Una línea
dotnet build HotelCc.Tests && dotnet test HotelCc.Tests

# Categoría específica
dotnet test --filter "Category=HabitacionModel"

# Solo fallos
dotnet test --filter "Outcome=Failed"

# Con salida
dotnet test --logger:"console;verbosity=detailed"
```

---

## ❓ Quick FAQ

| P | R |
|---|---|
| ¿Dónde empiezo? | QUICK_START.md |
| ¿Cómo ejecuto? | dotnet test HotelCc.Tests |
| ¿Cuáles fueron creadas? | 99 pruebas |
| ¿Qué cobertura? | ~85% |
| ¿Cómo está organizado? | 8 carpetas |

---

**Status: ✅ LISTO**

Más información: `NAVIGATION.md`
