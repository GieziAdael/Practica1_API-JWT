🧩 Practica - API REST Inicio de sesion basico, Implementando Autenticacion y Autorizacion JWT
Objetivo: Implementar buenas practicas en la creacion de una API REST + seguridad con JWT

Esta es una practica a nivel junior, una elaboracion de una API RESTful basica, puedes usarlo en Postman.

------------------------------------------------------------
🚀 Caracteristicas Principales
------------------------------------------------------------
I) Entidad 'User' y 'Objeto'
II) Arquitectura modular con separacion de capas
III) Repositorio / IRepositorio de la entidad Tarea
IV) Integracion de AutoMapper
V) Implementacion de Login, Autenticacion y Autorizacion con JWT

------------------------------------------------------------
🧰 Logica
------------------------------------------------------------
Todos Pueden crear usuario
Solo administrador puede ver la lista total de usuarios
Todos pueden ver objetos
Solo administrador y moderador puede crear o eliminar objeto

------------------------------------------------------------
🧰 Tecnologías utilizadas
------------------------------------------------------------
- .NET 8.0 / ASP.NET Core Web API
- Entity Framework Core
- C# 12
- Swagger y Postman (Para probar el Token)
- SQL Server LocalDB

------------------------------------------------------------
🧑‍💻 Autor
------------------------------------------------------------
Desarrollador: GieziAdael <br>
Rol: Backend Developer (.NET Junior) <br>
Contacto: giezi.tlaxcoapan@gmail.com <br>

------------------------------------------------------------
📘 Descripción Ampliada
------------------------------------------------------------
El día lunes 27 de Octubre, 2025, inicie con el tema sobre implementación de JWT en una API REST, un curso de Udemy donde le doy seguimiento a una actividad en concreto, donde trata sobre la creación de una API profesional, segura y escalable.
En la sección de JWT, fue acompañada con teoría y principalmente la practica. El día de hoy martes 28 de Octubre 2025, me propuse en crear una API basica, donde repasaria algunos conceptos de buenas practicas y principalmente la implementación de JWT.
URL: https://github.com/GieziAdael/Practica1_API-JWT

------------------------------------------------------------
📊 Evaluación Técnica (Nivel Junior Backend .NET)
------------------------------------------------------------
Estructura del proyecto: 8.0/10
Configuración / Dependencias: 7.5/10
Modelos y DTOs: 7.0/10
Repositorios / Lógica: 8.5/10
Autenticación / JWT: 7.0/10
Seguridad (hash, validaciones): 8.5/10
Manejo de errores / respuestas: 6.5/10
Documentación / Pruebas: 6.0/10

Puntaje total: 78 / 100
Nivel: Junior sólido, con buena comprensión de arquitectura y seguridad básica.

------------------------------------------------------------
🧭 Próximos pasos sugeridos
------------------------------------------------------------
- Mover SecretKey a user-secrets o variables de entorno.
- Mejorar DTOs (renombrar PasswordHash -> Password).
- Agregar validaciones con DataAnnotations ([Required], [EmailAddress]).
- Implementar middleware global de manejo de errores.
- Añadir pruebas unitarias (mínimo para PasswordHash, Login, Repositories).
- Usar constantes o enum para roles (admin, modd, viewer).
- Configurar expiración de tokens y refresh tokens.

------------------------------------------------------------
🧾 Evaluador Técnico
------------------------------------------------------------
ChatGPT (GPT-5)
Revisión técnica: 28/Oct/2025
