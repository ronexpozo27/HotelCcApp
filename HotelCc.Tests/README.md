# Pruebas Unitarias del Proyecto HotelCc

Este proyecto contiene 60 pruebas unitarias organizadas y estructuradas para el proyecto HotelCc.

## Estructura de Pruebas

### 1. **Modelos (30 pruebas)**
   - **HabitacionModelTests.cs** (10 pruebas)
	 - Creación de habitación con datos válidos
	 - Propiedades nulas
	 - Valores por defecto
	 - Actualización de propiedades
	 - Independencia de instancias
	 - Precios altos

   - **UsuarioModelTests.cs** (10 pruebas)
	 - Creación de usuario con datos válidos
	 - Propiedades nulas
	 - Roles válidos (Admin, User)
	 - Formato de email
	 - Independencia de instancias

   - **ReservaModelTests.cs** (10 pruebas)
	 - Creación de reserva con datos válidos
	 - Propiedades nulas
	 - Fechas y totales
	 - Actualización de propiedades
	 - Valores por defecto

### 2. **Base de Datos (6 pruebas)**
   - **AppDbContextTests.cs**
	 - Agregar usuarios
	 - Agregar habitaciones
	 - Agregar reservas
	 - Agregar múltiples registros
	 - Consultas de datos

### 3. **Controladores (12 pruebas)**
   - **HabitacionesControllerTests.cs** (6 pruebas)
	 - Index devuelve ViewResult
	 - Details con ID nulo/inválido
	 - Create GET/POST
	 - Validación de modelo

   - **AuthControllerTests.cs** (6 pruebas)
	 - Login GET/POST
	 - Register GET/POST
	 - Autenticación correcta/incorrecta
	 - Redirección por rol

   - **ReservasControllerTests.cs** (múltiples pruebas)
	 - Index devuelve ViewResult
	 - Details con ID nulo/inválido
	 - Create con SelectList

### 4. **ViewModels (8 pruebas)**
   - **LoginViewModelTests.cs** (7 pruebas)
	 - Creación con datos válidos
	 - Propiedades nulas/vacías
	 - Actualización de propiedades

   - **HabitacionEstadoViewModelTests.cs** (8 pruebas)
	 - Estados: Disponible/Ocupada
	 - Propiedades de habitación

### 5. **Integración (8 pruebas)**
   - **HabitacionReservaIntegrationTests.cs**
	 - Crear y verificar habitación en BD
	 - Crear y verificar usuario en BD
	 - Crear reserva con usuario y habitación
	 - Actualizar precio de habitación
	 - Eliminar habitación
	 - Consultas por filtro (tipo)
	 - Consultas por rango de fechas

### 6. **Validación (10 pruebas)**
   - **DataValidationTests.cs**
	 - Formato de email
	 - Números alfanuméricos
	 - Precios positivos
	 - Fechas válidas
	 - Campos no vacíos
	 - Roles válidos
	 - Montos válidos

### 7. **Casos Límite (10 pruebas)**
   - **EdgeCaseTests.cs**
	 - Strings muy largos
	 - Precios muy altos/cero
	 - Fechas futuras/pasadas
	 - IDs máximos/mínimos
	 - Totales muy grandes
	 - Strings vacíos

### 8. **Rendimiento (6 pruebas)**
   - **BulkOperationTests.cs**
	 - Inserción de 50 habitaciones
	 - Inserción de 30 usuarios
	 - Consulta ordenada por rendimiento
	 - Filtrado por tipo
	 - Agregación de precios

## Total: 60 Pruebas Unitarias

## Ejecución de Pruebas

```bash
# Ejecutar todas las pruebas
dotnet test HotelCc.Tests

# Ejecutar pruebas de una categoría específica
dotnet test HotelCc.Tests --filter "Category=HabitacionModel"

# Ejecutar con verbose
dotnet test HotelCc.Tests --verbosity=normal
```

## Herramientas Utilizadas

- **MSTest**: Framework de pruebas unitarias
- **Moq**: Librería para mocking (preparada para uso)
- **Entity Framework Core In-Memory**: Para pruebas de base de datos
- **.NET 10**: Versión objetivo del framework

## Patrones Aplicados

- **Arrange-Act-Assert (AAA)**: Estructura consistente de pruebas
- **Test Categories**: Organización por categoría
- **In-Memory Database**: Para pruebas sin dependencias externas
- **Integration Tests**: Pruebas de flujos completos
- **Edge Cases**: Validación de comportamientos límite
- **Performance Tests**: Validación de operaciones en volumen
