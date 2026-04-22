Biblioteca Digital



Sistema web para la gestión de documentos digitales y manuscritos escaneados, desarrollado con arquitectura desacoplada basada en Web Forms + Web API.



Descripción



Biblioteca Digital es una solución orientada a la organización, consulta y administración de documentos digitales.  

El sistema permite autenticación de usuarios, control de roles, gestión de libros, subida y descarga de archivos y visualización de métricas mediante dashboard.



Tecnologías utilizadas



\- ASP.NET Web Forms

\- ASP.NET Web API

\- C#

\- SQL Server

\- Stored Procedures

\- HttpClient

\- JSON

Arquitectura



La solución está compuesta por dos proyectos principales:



1\. BibliotecaDigital.Web

Capa de presentación desarrollada en ASP.NET Web Forms.  

Se encarga de:

\- Login y registro de usuarios

\- Visualización de módulos

\- Gestión de libros y usuarios

\- Consumo de la API mediante HttpClient



2\. BibliotecaDigital.API

Backend desarrollado en ASP.NET Web API.  

Se encarga de:

\- Exponer endpoints REST

\- Aplicar lógica de negocio

\- Gestionar autenticación

\- Validar datos

\- Acceder a SQL Server mediante procedimientos almacenados



\## Funcionalidades principales



\- Autenticación de usuarios

\- Hash de contraseñas con SHA256

\- Control de acceso por roles

\- Gestión de libros

\- Subida de archivos con validación

\- Descarga y visualización de archivos

\- Gestión de categorías

\- Dashboard con métricas del sistema



&#x20;Flujo general del sistema



1\. El usuario interactúa con la interfaz en Web Forms.

2\. El frontend envía solicitudes HTTP a la API usando HttpClient.

3\. La API recibe los datos en formato JSON o multipart/form-data.

4\. La capa de negocio procesa la lógica del sistema.

5\. La capa de datos ejecuta procedimientos almacenados en SQL Server.

6\. La API devuelve la respuesta al frontend.



\## Módulo de libros



El sistema permite:

\- Registrar libros con metadata

\- Subir archivos físicos al servidor

\- Editar información y reemplazar archivos

\- Eliminar registros y archivos asociados

\- Descargar o visualizar documentos desde la aplicación



&#x20;Seguridad



\- Contraseñas almacenadas mediante hash SHA256

\- Validaciones en frontend y backend

\- Control de acceso según rol de usuario



&#x20;Mejoras futuras



\- Migración del frontend a React

\- Implementación de autenticación basada en tokens

\- Mejor manejo global de excepciones

\- Login centralizado

\- Despliegue en infraestructura cloud



&#x20;Autor



\*\*Rony Dubon\*\*

