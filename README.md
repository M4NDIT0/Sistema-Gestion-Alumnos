#  Sistema de Gestión de Alumnos

Sistema completo de gestión estudiantil construido con arquitectura full-stack moderna, permitiendo operaciones CRUD completas sobre registros de alumnos.

##  Tabla de Contenidos

- [Características](#características)
- [Tecnologías](#tecnologías)
- [Arquitectura](#arquitectura)
- [Requisitos Previos](#requisitos-previos)
- [Instalación](#instalación)
- [Configuración](#configuración)
- [Uso](#uso)
- [Estructura del Proyecto](#estructura-del-proyecto)
- [API Endpoints](#api-endpoints)
- [Probar con Postman](#-probar-endpoints-con-postman)
- [Despliegue](#-despliegue)

## ✨ Características

### Frontend
-**Interfaz moderna y responsiva** con Vue 3 Composition API
-**CRUD completo** (Crear, Leer, Actualizar, Eliminar)
-**Modo oscuro/claro** automático según preferencias del sistema
-**Notificaciones elegantes** con SweetAlert2
-**Validación de formularios** en tiempo real
-**Diseño mobile-first** totalmente adaptable
-**Animaciones suaves** y transiciones fluidas
-**TypeScript** para tipado estático

### Backend
-**API RESTful** con ASP.NET Core 9
-**Entity Framework Core** para acceso a datos
-**SQL Server** como base de datos
-**Arquitectura limpia** con separación de responsabilidades
-**DTOs** para transferencia de datos
-**CORS** configurado para desarrollo
-**Manejo de errores** robusto

##  Tecnologías

### Backend
| Tecnología | Versión | Descripción |
|-----------|---------|-------------|
| ![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=flat&logo=dotnet) | 9.0 | Framework principal |
| ![Entity Framework](https://img.shields.io/badge/EF_Core-9.0-512BD4?style=flat) | 9.0 | ORM para acceso a datos |
| ![SQL Server](https://img.shields.io/badge/SQL_Server-2019+-CC2927?style=flat&logo=microsoft-sql-server) | 2019+ | Base de datos |
| ![C#](https://img.shields.io/badge/C%23-12.0-239120?style=flat&logo=c-sharp) | 12.0 | Lenguaje de programación |

### Frontend
| Tecnología | Versión | Descripción |
|-----------|---------|-------------|
| ![Vue.js](https://img.shields.io/badge/Vue.js-3.5-4FC08D?style=flat&logo=vue.js) | 3.5 | Framework progresivo |
| ![TypeScript](https://img.shields.io/badge/TypeScript-5.0-3178C6?style=flat&logo=typescript) | 5.0 | Superset tipado de JavaScript |
| ![Vite](https://img.shields.io/badge/Vite-7.1-646CFF?style=flat&logo=vite) | 7.1 | Build tool ultrarrápido |
| ![Axios](https://img.shields.io/badge/Axios-1.13-5A29E4?style=flat) | 1.13 | Cliente HTTP |
| ![SweetAlert2](https://img.shields.io/badge/SweetAlert2-11.26-F27474?style=flat) | 11.26 | Alertas personalizadas |

##  Arquitectura

```
┌─────────────────┐         ┌─────────────────┐         ┌─────────────────┐
│                 │         │                 │         │                 │
│   Vue 3 SPA     │────────▶│  ASP.NET Core   │────────▶│   SQL Server    │
│   (Frontend)    │  HTTPS  │  Web API        │   EF    │   Database      │
│                 │◀────────│  (Backend)      │◀────────│                 │
└─────────────────┘         └─────────────────┘         └─────────────────┘
     Vite Dev                    Port: 7001              ProyectoPt DB
```

### Capas de la Aplicación

**Frontend:**
- **Presentación**: Componentes Vue
- **Servicios**: Capa de comunicación HTTP
- **Tipos**: Interfaces TypeScript

**Backend:**
- **Controllers**: Manejo de peticiones HTTP
- **Data**: DbContext y configuración EF Core
- **Models**: Entidades de dominio
- **DTOs**: Objetos de transferencia de datos

##  Requisitos Previos

Asegúrate de tener instalado:

- **Node.js** >= 20.19.0 o >= 22.12.0
- **npm** >= 9.0.0
- **.NET SDK** >= 9.0
- **SQL Server** 2019+ (Express, Developer o Enterprise)
- **Git** para control de versiones

##  Instalación

### 1. Clonar el Repositorio

```bash
git clone https://github.com/tu-usuario/sistema-gestion-alumnos.git
cd sistema-gestion-alumnos
```

### 2. Configurar Backend

```bash
cd PruebaTecnicaBe

# Restaurar paquetes NuGet
dotnet restore

# Aplicar migraciones a la base de datos
dotnet ef database update

# Ejecutar el proyecto
dotnet run
```

El backend estará disponible en: `https://localhost:7001`

### 3. Configurar Frontend

```bash
cd PruebaTecnicaFe

# Instalar dependencias
npm install

# Ejecutar en modo desarrollo
npm run dev
```

El frontend estará disponible en: `http://localhost:5173`

##  Configuración

### Backend - appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=TU_SERVIDOR;Database=ProyectoPt;User ID=tu_usuario;Password=tu_password;Trusted_Connection=true;TrustServerCertificate=true;MultipleActiveResultSets=true"
  }
}
```

### Frontend - AlumnoService.ts

```typescript
const API_URL = "https://localhost:7001/api/Alumnos";
```

### Variables de Entorno

Crea un archivo `.env` en la raíz del frontend:

```env
VITE_API_URL=https://localhost:7001/api
```

## 💻 Uso

### Agregar un Alumno

1. Completa el formulario con los datos del alumno
2. Haz clic en "Guardar Alumno"
3. Verás una notificación de éxito

### Editar un Alumno

1. Haz clic en el botón "Editar" en la fila del alumno
2. El formulario se llenará automáticamente
3. Modifica los campos necesarios
4. Haz clic en "Actualizar Alumno"

### Eliminar un Alumno

1. Haz clic en el botón "Eliminar" en la fila del alumno
2. Confirma la acción en el diálogo
3. El alumno será eliminado permanentemente

## 📁 Estructura del Proyecto

```
ProyectoPt/
│
├── PruebaTecnicaBe/                 # Backend .NET
│   ├── Controllers/
│   │   └── AlumnosController.cs     # Endpoints de la API
│   ├── Data/
│   │   └── PruebaTecnicaBeDbContext.cs  # Contexto de EF Core
│   ├── Models/
│   │   └── Alumno.cs                # Modelo de dominio
│   ├── DTOs/
│   │   └── AlumnoCreateDTO.cs       # DTO para crear/actualizar
│   ├── Migrations/                  # Migraciones de BD
│   ├── Program.cs                   # Configuración de la app
│   └── appsettings.json             # Configuración
│
└── PruebaTecnicaFe/                 # Frontend Vue
    ├── src/
    │   ├── components/              # Componentes Vue
    │   ├── services/
    │   │   └── AlumnoService.ts     # Cliente HTTP
    │   ├── types/
    │   │   └── Alumno.ts            # Tipos TypeScript
    │   ├── App.vue                  # Componente principal
    │   └── main.ts                  # Entry point
    ├── public/                      # Assets estáticos
    ├── package.json                 # Dependencias npm
    └── vite.config.js               # Configuración Vite
```

## 🔌 API Endpoints

### Base URL: `https://localhost:7001/api/Alumnos`

| Método | Endpoint | Descripción | Body |
|--------|----------|-------------|------|
| `GET` | `/` | Obtener todos los alumnos | - |
| `GET` | `/{id}` | Obtener alumno por ID | - |
| `POST` | `/` | Crear nuevo alumno | `AlumnoCreateDTO` |
| `PUT` | `/{id}` | Actualizar alumno | `AlumnoCreateDTO` |
| `DELETE` | `/{id}` | Eliminar alumno | - |

### Modelos de Datos

**Alumno**
```typescript
{
  id: number;
  nombre: string;
  apellido: string;
  email: string;
  fechaNacimiento: string; // ISO 8601
}
```

**AlumnoCreateDTO**
```typescript
{
  nombre: string;
  apellido: string;
  email: string;
  fechaNacimiento: string; // ISO 8601
}
```

## 🧪 Probar Endpoints con Postman

### Configuración Inicial

1. **Descarga e instala** [Postman](https://www.postman.com/downloads/)
2. **Asegúrate** de que el backend esté corriendo en `https://localhost:7001`

### 1. Obtener Todos los Alumnos (GET)

**Método:** `GET`  
**URL:** `https://localhost:7001/api/Alumnos`  
**Headers:** Ninguno requerido

**Respuesta esperada (200 OK):**
```json
[
  {
    "id": 1,
    "nombre": "Juan",
    "apellido": "Pérez",
    "email": "juan.perez@email.com",
    "fechaNacimiento": "2000-05-15"
  }
]
```

### 2. Obtener Alumno por ID (GET)

**Método:** `GET`  
**URL:** `https://localhost:7001/api/Alumnos/1`  
**Headers:** Ninguno requerido

**Respuesta esperada (200 OK):**
```json
{
  "id": 1,
  "nombre": "Juan",
  "apellido": "Pérez",
  "email": "juan.perez@email.com",
  "fechaNacimiento": "2000-05-15"
}
```

### 3. Crear Nuevo Alumno (POST)

**Método:** `POST`  
**URL:** `https://localhost:7001/api/Alumnos`  
**Headers:**
- `Content-Type`: `application/json`

**Body (raw JSON):**
```json
{
  "nombre": "María",
  "apellido": "García",
  "email": "maria.garcia@email.com",
  "fechaNacimiento": "1999-08-20"
}
```

**Respuesta esperada (201 Created):**
```json
{
  "id": 2,
  "nombre": "María",
  "apellido": "García",
  "email": "maria.garcia@email.com",
  "fechaNacimiento": "1999-08-20"
}
```

### 4. Actualizar Alumno (PUT)

**Método:** `PUT`  
**URL:** `https://localhost:7001/api/Alumnos/1`  
**Headers:**
- `Content-Type`: `application/json`

**Body (raw JSON):**
```json
{
  "nombre": "Juan Carlos",
  "apellido": "Pérez López",
  "email": "juancarlos.perez@email.com",
  "fechaNacimiento": "2000-05-15"
}
```

**Respuesta esperada (204 No Content)**

### 5. Eliminar Alumno (DELETE)

**Método:** `DELETE`  
**URL:** `https://localhost:7001/api/Alumnos/1`  
**Headers:** Ninguno requerido

**Respuesta esperada (204 No Content)**

### Errores Comunes

| Código | Error | Solución |
|--------|-------|----------|
| 400 | Bad Request | Verifica que el JSON sea válido y los campos requeridos estén presentes |
| 404 | Not Found | El ID del alumno no existe en la base de datos |
| 500 | Internal Server Error | Revisa que el backend esté corriendo y la BD conectada |

### Colección de Postman

Puedes importar esta colección completa en Postman:

1. Abre Postman
2. Click en **Import**
3. Copia y pega la URL del archivo `postman_collection.json` (si lo agregas al repo)
4. Todos los endpoints estarán listos para usar

### Probar con cURL (alternativa)

```bash
# GET - Listar todos
curl -X GET https://localhost:7001/api/Alumnos

# POST - Crear nuevo
curl -X POST https://localhost:7001/api/Alumnos \
  -H "Content-Type: application/json" \
  -d '{"nombre":"Pedro","apellido":"Martínez","email":"pedro@email.com","fechaNacimiento":"2001-03-10"}'

# PUT - Actualizar
curl -X PUT https://localhost:7001/api/Alumnos/1 \
  -H "Content-Type: application/json" \
  -d '{"nombre":"Pedro","apellido":"Martínez Silva","email":"pedro.martinez@email.com","fechaNacimiento":"2001-03-10"}'

# DELETE - Eliminar
curl -X DELETE https://localhost:7001/api/Alumnos/1
```

## 🚀 Despliegue

### Backend

```bash
cd PruebaTecnicaBe
dotnet publish -c Release -o ./publish
```

### Frontend

```bash
cd PruebaTecnicaFe
npm run build
```

Los archivos compilados estarán en `dist/`

---

**Desarrollado con ❤️ usando Vue 3 y .NET 9**
