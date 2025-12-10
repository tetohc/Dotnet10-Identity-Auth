# 🔐 Proyecto IdentityNET10 – Sistema de Autenticación y Autorización

Este repositorio contiene el desarrollo de una aplicación web construida con **.NET 10** y **C# 14**, utilizando **ASP.NET Core Identity** para implementar un sistema completo de **autenticación y autorización**.  
Actualmente el proyecto se encuentra en la etapa de **autenticación**, con funcionalidades de registro, inicio de sesión, confirmación de correo y recuperación de contraseña.  
A futuro se integrarán **Roles, Claims, OAuth y MFA/2FA**.

---

## 🧱 Arquitectura

La aplicación está basada en el patrón MVC de ASP.NET Core, extendida con las capacidades de **Identity** para la gestión de usuarios:

- **Identity**: manejo de usuarios, contraseñas, roles y claims.
- **ViewModels**: usados en las vistas y controladores para validar y presentar datos.
- **AppUser (Modelo de dominio)**: entidad personalizada que extiende la clase base de Identity.
- **AccountController**: controlador principal para el flujo de autenticación y login externo.
- **Servicios externos**: integración con Facebook y Google Login.

---

## 🎯 Objetivos del proyecto

- Implementar un sistema completo de **autenticación y autorización** con ASP.NET Core Identity.
- Integrar login externo con **Facebook** y **Google**.
- Incorporar confirmación de correo electrónico y recuperación de contraseña.
- En proceso, añadir seguridad avanzada con **MFA/2FA** y manejo de cookies.
- Aplicar buenas prácticas de arquitectura y seguridad en aplicaciones web modernas.

---

## 🛠️ Tecnologías utilizadas

| Tecnología            | Uso principal                                      |
|-----------------------|----------------------------------------------------|
| **C# 14**             | Lenguaje de programación                          |
| **.NET 10**           | Framework principal                               |
| **ASP.NET Core Identity** | Autenticación y autorización                   |
| **Bootstrap 5**       | Estilos responsivos                               |
| **Bootstrap Icons**   | Iconos en vistas                             |

---

## 🚀 Funcionalidades actuales

- Registro de usuarios con validación.
- Inicio de sesión con credenciales locales.
- Confirmación de correo electrónico.
- Recuperación de contraseña vía correo.
- Login externo con **Facebook** y **Google**.
- Vistas personalizadas para mejorar la experiencia de usuario.

---

## 📌 Funcionalidades futuras

- Autorización con **Roles y Claims**.
- Integración con **OAuth**.
- Seguridad avanzada con **MFA/2FA**.
- Manejo de cookies en Identity .NET 10.

---

## 🖼️ Material visual

A continuación se presentan capturas de pantalla que ilustran el funcionamiento del sistema desde la perspectiva del usuario:

### 📌 Vista de Registro
![Registro](https://raw.githubusercontent.com/tetohc/MediaResources/refs/heads/main/images/covers/IdentityNET10/registro_1.png)

### 📌 Vista de Inicio de sesión
![Login](https://raw.githubusercontent.com/tetohc/MediaResources/refs/heads/main/images/covers/IdentityNET10/login_1.png)

### 📌 Confirmación de registro
![Confirmación de Registro](https://raw.githubusercontent.com/tetohc/MediaResources/refs/heads/main/images/covers/IdentityNET10/confirmacion%20de%20registro.png)

### 📌 Confirmación de correo
![Confirmación de Correo](https://raw.githubusercontent.com/tetohc/MediaResources/refs/heads/main/images/covers/IdentityNET10/confirmacion%20de%20correo.png)

### 📌 Recuperación de contraseña
![Recuperación de Contraseña](https://raw.githubusercontent.com/tetohc/MediaResources/refs/heads/main/images/covers/IdentityNET10/recuperar%20password%201.png)

![Recuperación de Contraseña](https://raw.githubusercontent.com/tetohc/MediaResources/refs/heads/main/images/covers/IdentityNET10/recuperacion%20de%20password%202.png)

---

## 📜 Estado del proyecto

Este proyecto es de práctica personal, enfocado en aprender y experimentar con:
- ASP.NET Core Identity.
- Integración de login externo.
- Recuperación de contraseña y confirmación de correo.
- Buenas prácticas de autenticación y autorización.


