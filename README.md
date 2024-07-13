# Evaluación Individual: Postable - RESTful API para Gestión de Posts

- Enlace de API desplegada: https://postable-api.somee.com/swagger/index.html

### Objetivo

Desarrollar una RESTful API para una red social que permita a los usuarios interactuar con publicaciones (Posts). Esta API debe ser capaz de manejar diferentes operaciones dependiendo de si el usuario está registrado o no.

### Requerimientos Técnicos

#### Tecnologías y Herramientas

- **Lenguaje:** C#.
- **Framework:** .Net Entity Framework Core
- **Autenticación/Autorización:** JWT.
- **Base de Datos:** MS SQL Server.

#### Consideraciones adicionales

- Historial adecuado de migraciones de la BD.
- Validación de datos de entrada.
- Uso adecuado de códigos de status en las respuestas del servidor.
- El repositorio deberá incluir:
    - Código fuente de la API.
    - Guía de cómo realizar la configuración y levantamiento de la aplicación en archivo `README.md`
- Bonus: Incluir testing de principales endpoints.

## Esquema de Base de Datos

1. **Users**

- `Id`: Identificador único del usuario. _Restricción_: Clave primaria.
- `Username`: Apodo de usuario. _Restricción_: Único, no nulo.
- `Password`: Contraseña del usuario, almacenada de manera segura. _Restricción_: No nulo.
- `Email`: Email del usuario. _Restricción_: Único, puede ser nulo.
- `FirstName`: Nombre del usuario. _Restricción_: Puede ser nulo.
- `LastName`: Apellido del usuario. _Restricción_: Puede ser nulo.
- `Role`: Rol del usuario, con valores 'user' o 'admin'. _Restricción_: No nulo, "user" por defecto.
- `CreatedAt`: Fecha y hora de creación del usuario. _Restricción_: No nulo.

2. **Posts**

- `Id`: Identificador único del post. _Restricción_: Clave primaria.
- `UserId`: Identificador del usuario que creó el post. _Restricción_: Clave foránea, no nulo.
- `Content`: Contenido del post. _Restricción_: No nulo.
- `CreatedAt`: Fecha y hora de creación del post. _Restricción_: No nulo.

3. **Likes**

- `Id`: Identificador único del like. _Restricción_: Clave primaria.
- `PostId`: Identificador del post al que se le dio like. _Restricción_: Clave foránea, no nulo.
- `UserId`: Identificador del usuario que dio like. _Restricción_: Clave foránea, no nulo.
- `CreatedAt`: Fecha y hora en que se dio el like. _Restricción_: No nulo.

### Restricciones y Relaciones Adicionales

- **Unicidad en Likes:** La combinación de `PostId` y `UserId` en la tabla `Likes` debe ser única para evitar likes duplicados.
- **Restricciones de Datos:** Deberás aplicar restricciones adecuadas en cuanto a longitud y formato de los datos según tu criterio (por ejemplo, definir una longitud máxima de `Username` o validar el formato de `Email`).

## Especificación de API

### Visualización de Posts

- **GET `/posts` (Ver Todos los Posts con Paginación y Filtros)**
    - **Descripción:** Retorna una lista de posts disponibles en la plataforma, con opciones de filtrado por usuario y ordenación.
    - **Parámetros Query:**
        - `username`: Filtrar posts por nombre de usuario (opcional).
        - `orderBy`: Criterio de ordenación, opciones: `createdAt`, `likesCount` (opcional, por defecto `createdAt`).
        - `order`: Dirección de la ordenación, opciones: `asc`, `desc` (opcional, por defecto `asc`).
    - **Respuesta:**
        - **200 OK:** Lista paginada de posts en formato JSON.
    - **Ejemplo de Respuesta:**
      ```json
      [
        {
          "id": 1,
          "content": "Este es un post",
          "createdAt": "2024-01-19 07:37:16-08",
          "username": "usuario1",
          "likesCount": 5
        },
        ...
      ]
      ```
    - **Ejemplo de Uso:**
        - Para obtener la lista de posts filtrando por el usuario 'usuarioEjemplo', ordenados por número de likes en orden descendente:
            - `GET /posts?username=usuarioEjemplo&orderBy=likesCount&order=desc`

### Interacción de Usuarios Registrados

- **POST `/posts` (Crear Nuevo Post)**

    - **Descripción:** Permite a un usuario registrado crear un nuevo post.
    - **Body:**
        - `content`: Texto del post.
    - **Respuesta:**
        - **201 Created:** Post creado exitosamente.
        - **400 Bad Request:** Si falta información o el formato es incorrecto.
        - **401 Unauthorized:** Si el usuario no está autenticado.
    - **Ejemplo de Respuesta:**
      ```json
      {
        "id": 10,
        "content": "Mi post actualizado",
        "createdAt": "2024-01-19 10:37:16-08",
        "username": "mi-usuario",
        "likesCount": 0
      }
      ```

- **PATCH `/posts/:id` (Editar Post Existente)**

    - **Descripción:** Permite a un usuario registrado editar un post existente.
    - **Parámetros URL:**
        - `id`: Id del post a editar.
    - **Body:**
        - `content`: Texto actualizado del post. (El campo es opcional, pero se debe enviar al menos un campo para actualizar)
    - **Respuesta:**
        - **200 OK:** Post actualizado exitosamente. Devuelve el post actualizado.
        - **400 Bad Request:** Si falta información, el formato es incorrecto o no se envía ningún campo para actualizar.
        - **401 Unauthorized:** Si el usuario no está autenticado o no es el propietario del post.
        - **404 Not Found:** Si el post no existe.
    - **Ejemplo de Respuesta:**
      ```json
      {
        "id": 10,
        "content": "Mi post actualizado",
        "createdAt": "2024-01-19 10:37:16-08",
        "username": "mi-usuario",
        "likesCount": 0
      }
      ```

- **POST `/posts/:postId/like` (Dar Like a un Post)**

    - **Descripción:** Permite a un usuario registrado dar "Like" a un post.
    - **Parámetros deURL:**
        - `postId`: Id del post a dar like.
    - **Respuesta:**
        - **200 OK:** Like registrado.
        - **404 Not Found:** Si el post no existe.
        - **401 Unauthorized:** Si el usuario no está autenticado.
    - **Ejemplo de Respuesta:**
      ```json
      {
        "id": 15,
        "content": "Mi nuevo post",
        "createdAt": "2024-01-19 10:37:16-08",
        "username": "usuario",
        "likesCount": 1
      }
      ```

- **DELETE `/posts/:postId/like` (Eliminar Like de un Post)**
    - **Descripción:** Permite a un usuario eliminar su "like" de un post.
    - **Parámetros de URL:**
        - `postId`: ID del post a remover like.
    - **Respuesta:**
        - **200 OK:** Like eliminado.
        - **404 Not Found:** Si el post no existe o no tenía like previamente.
        - **401 Unauthorized:** Si el usuario no está autenticado.
    - **Ejemplo de Respuesta:**
      ```json
      {
        "id": 15,
        "content": "Mi nuevo post",
        "createdAt": "2024-01-19 10:37:16-08",
        "username": "usuario",
        "likesCount": 0
      }
      ```

#### Gestión de Perfil de Usuario

- **GET `/me` (Ver Perfil de Usuario)**

    - **Descripción:** Muestra el perfil del usuario autenticado.
    - **Respuesta:**
        - **200 OK:** Información del perfil en formato JSON.
        - **401 Unauthorized:** Si el usuario no está autenticado.
    - **Ejemplo de Respuesta:**
      ```json
      {
        "id": 2,
        "username": "miUsuario",
        "email": "miemail@example.com",
        "firstName": "Nombre",
        "lastName": "Apellido",
        "createdAt": "2024-01-19 10:37:16-08"
      }
      ```

- **PATCH `/me` (Editar Cuenta de Usuario)**

    - **Descripción:** Permite al usuario editar su información de perfil.
    - **Body:**
        - `email`, `firstName`, `lastName`: Campos opcionales para actualizar.
    - **Respuesta:**
        - **200 OK:** Perfil actualizado.
        - **400 Bad Request:** Si el formato es incorrecto.
        - **401 Unauthorized:** Si el usuario no está autenticado.
    - **Ejemplo de Respuesta:**
      ```json
      {
        "id": 2,
        "username": "miUsuario",
        "email": "nuevo@mail.com",
        "firstName": "Nombre",
        "lastName": "Apellido",
        "createdAt": "2024-01-19 10:37:16-08"
      }
      ```

- **DELETE `/me` (Eliminar Cuenta de Usuario)**
    - **Descripción:** Permite al usuario eliminar su cuenta.
    - **Respuesta:**
        - **204 No Content:** Cuenta eliminada exitosamente.
        - **401 Unauthorized:** Si el usuario no está autenticado.

#### Registro y Autenticación de Usuarios

- **POST `/signup` (Crear Cuenta)**
- **Descripción:** Permite a un nuevo usuario registrarse en la plataforma.
- **Body:**
    - `username`, `password`: Campos requeridos para el registro.
- **Respuesta:**
    - **201 Created:** Cuenta creada.
    - **400 Bad Request:** Si falta información o el formato es incorrecto.
- **Ejemplo de Respuesta:**

  ```json
  {
    "id": 20,
    "username": "nuevoUsuario",
    "email": "un-mail@example.com",
    "firstName": "Nombre",
    "lastName": "Apellido",
    "createdAt": "2024-01-19 10:37:16-08"
  }
  ```

- **POST `/login` (Iniciar Sesión)**
    - **Descripción:** Permite a un usuario existente iniciar sesión.
    - **Body:**
        - `username`, `password`: Credenciales requeridas para el inicio de sesión.
    - **Respuesta:**
        - **200 OK:** Sesión iniciada, retorna token JWT.
        - **401 Unauthorized:** Credenciales incorrectas.
    - **Ejemplo de Respuesta:**
      ```json
      {
        "token": "eyJhbGciOiJIUzI1NiIsInR5..."
      }
      ```
