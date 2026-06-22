# 📚 Biblioteca Digital

Sistema web para la administración, consulta y gestión de libros digitales desarrollado con ASP.NET Web Forms, ASP.NET Web API, SQL Server y autenticación JWT.

---

## 🚀 Descripción

Biblioteca Digital es una aplicación empresarial desarrollada como proyecto académico y posteriormente evolucionada como proyecto de portafolio profesional.

El sistema permite:

* Administración de usuarios y roles.
* Gestión de libros digitales.
* Gestión de categorías.
* Autenticación mediante JWT.
* Dashboard con indicadores operativos.
* Control de acceso basado en roles.
* Historial de accesos.
* Descarga y visualización de documentos digitales.
* Interfaz moderna con modo claro y oscuro.

---

## 🏗 Arquitectura

```text
┌─────────────────────┐
│ ASP.NET Web Forms   │
│ Frontend            │
└──────────┬──────────┘
           │ HttpClient
           ▼
┌─────────────────────┐
│ ASP.NET Web API     │
│ Backend REST        │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│ SQL Server          │
│ Stored Procedures   │
└─────────────────────┘
```

---

## 🔐 Seguridad

### Autenticación JWT

El sistema utiliza JSON Web Tokens para:

* Inicio de sesión seguro.
* Validación de identidad.
* Control de acceso.
* Protección de endpoints.

### Roles Implementados

| Rol           | Permisos             |
| ------------- | -------------------- |
| Administrador | Acceso total         |
| Bibliotecario | Gestión de libros    |
| Ejecutivo     | Consulta y dashboard |
| User          | Consulta de libros   |

### Super Administrador

Cuenta especial con permisos avanzados para administración del sistema.

---

## 📊 Dashboard

Incluye indicadores operativos en tiempo real:

* Total de libros
* Total de usuarios
* Total de categorías
* Historial de accesos
* Distribución de libros por categoría
* Gráficos interactivos con Chart.js

---

## 🛠 Tecnologías Utilizadas

### Backend

* ASP.NET Web API (.NET Framework 4.8)
* C#
* JWT Authentication
* SQL Server
* Stored Procedures

### Frontend

* ASP.NET Web Forms
* Bootstrap 5
* Bootstrap Icons
* Chart.js
* JavaScript
* CSS3

### Herramientas

* Visual Studio 2022
* Git
* GitHub

---

## 📂 Estructura del Proyecto

```text
BibliotecaDigital
│
├── BibliotecaDigital.API
│   ├── Controllers
│   ├── Models
│   ├── Entidades
│   ├── Negocio
│   └── Helpers
│
├── BibliotecaDigital.Web
│   ├── AdminLibros
│   ├── AdminUsuarios
│   ├── Dashboard
│   ├── Home
│   ├── Login
│   └── Models
│
└── SQL
    ├── Tablas
    ├── Relaciones
    └── Stored Procedures
```

---

## 📸 Capturas

### Inicio

Agregar captura de Home.aspx

### Dashboard

Agregar captura del Dashboard con Chart.js

### Administración de Libros

Agregar captura de AdminLibros.aspx

### Administración de Usuarios

Agregar captura de AdminUsuarios.aspx

---

## ⚙ Instalación

### 1. Clonar repositorio

```bash
git clone https://github.com/rdubon89/biblioteca-digital.git
```

### 2. Configurar SQL Server

Crear la base de datos y ejecutar los scripts SQL.

### 3. Configurar Web.config

Actualizar:

```xml
<connectionStrings>
```

con los valores locales.

### 4. Ejecutar

Abrir la solución en Visual Studio y ejecutar:

```text
BibliotecaDigital.API
BibliotecaDigital.Web
```

---

## 📈 Mejoras Implementadas

* JWT Authentication
* Dashboard profesional
* Chart.js
* Roles y permisos
* Dark Mode
* Diseño responsivo
* API desacoplada
* Arquitectura multicapa

---

## 👨‍💻 Autor

Rony Alexander Dubón Torres

* IT Support Engineer
* Security & Infrastructure Enthusiast
* Full Stack .NET Learner
* Data & Cloud Technologies Student

GitHub:
https://github.com/rdubon89

LinkedIn:
www.linkedin.com/in/rony-alexander-dubón-torres-b15898108

---

## 📄 Licencia

Proyecto desarrollado con fines académicos y de portafolio profesional.
