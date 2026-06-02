# 📚 Book Manager — C#

> Gestor de libros personal con perfiles de usuario, muro social y chat integrado. Desarrollado en **C#** con **Windows Forms**.
>
> ---
>
> ## 🚀 Funcionalidades
>
> - 📖 **Gestión de libros** por estado: Leídos / Pendientes de leer / Pendientes de comprar
> - - 👤 **Perfiles de usuario** con avatar, bio y estadísticas de lectura
>   - - 🤝 **Sistema de amigos**: buscar usuarios, enviar solicitudes y ver su muro
>     - - 📋 **Muro personal**: publica reseñas, citas y actualizaciones de lectura
>       - - 💬 **Chat en tiempo real** entre amigos (TCP/IP con Sockets)
>         - - ⭐ **Valoración y reseñas** de libros (1-5 estrellas)
>           - - 🔍 **Buscador** de libros por título, autor o género
>            
>             - ---
>
> ## 🛠️ Tecnologías
>
> | Capa | Tecnología |
> |---|---|
> | Lenguaje | C# (.NET 6 / .NET Framework 4.8) |
> | UI | Windows Forms |
> | Base de datos | SQLite (Microsoft.Data.Sqlite) |
> | Chat | TCP Sockets (System.Net.Sockets) |
> | ORM | ADO.NET |
>
> ---
>
> ## 📁 Estructura del proyecto
>
> ```
> BookManagerCSharp/
> ├── Forms/
> │   ├── FormLogin.cs          # Inicio de sesión y registro
> │   ├── FormMain.cs           # Ventana principal con la biblioteca
> │   ├── FormProfile.cs        # Perfil del usuario
> │   ├── FormFriends.cs        # Lista de amigos y muro
> │   └── FormChat.cs           # Ventana de chat
> ├── Models/
> │   ├── Book.cs               # Modelo de libro
> │   ├── User.cs               # Modelo de usuario
> │   └── Message.cs            # Modelo de mensaje de chat
> ├── Data/
> │   ├── DatabaseManager.cs    # Conexión y CRUD con SQLite
> │   └── BookManager.db        # Base de datos SQLite
> ├── Services/
> │   ├── ChatServer.cs         # Servidor TCP para el chat
> │   └── ChatClient.cs         # Cliente TCP para el chat
> └── BookManagerCSharp.sln
> ```
>
> ---
>
> ## 🗄️ Esquema de base de datos
>
> ```sql
> CREATE TABLE Users (
>     Id       INTEGER PRIMARY KEY AUTOINCREMENT,
>     Username TEXT    NOT NULL UNIQUE,
>     Password TEXT    NOT NULL,
>     Bio      TEXT    DEFAULT '',
>     Avatar   TEXT    DEFAULT ''
> );
>
> CREATE TABLE Books (
>     Id       INTEGER PRIMARY KEY AUTOINCREMENT,
>     UserId   INTEGER NOT NULL,
>     Title    TEXT    NOT NULL,
>     Author   TEXT    DEFAULT '',
>     Genre    TEXT    DEFAULT '',
>     Status   INTEGER NOT NULL DEFAULT 0,
>     Rating   INTEGER DEFAULT 0,
>     Review   TEXT    DEFAULT '',
>     AddedAt  TEXT    DEFAULT (datetime('now'))
> );
> ```
>
> ---
>
> ## ▶️ Cómo ejecutar
>
> 1. Clona el repositorio:
> 2.    ```bash
>          git clone https://github.com/LluisDam/book-manager-csharp.git
>          ```
>       2. Abre `BookManagerCSharp.sln` en **Visual Studio 2019/2022**
>       3. 3. Restaura los paquetes NuGet (`Microsoft.Data.Sqlite`)
>          4. 4. Compila y ejecuta con `F5`
>            
>             5. ---
>            
>             6. ## 🔄 Diferencias con la versión VB.NET
>            
>             7. | Característica | VB.NET | C# |
> |---|---|---|
> | Sintaxis | Visual Basic .NET | C# |
> | Properties | `Public Property X As T` | `public T X { get; set; }` |
> | Null-safety | Implícito | Nullable Reference Types |
> | Pattern matching | Select Case | switch expressions |
> | LINQ | Disponible | Disponible + más idiomático |
>
> ---
>
> ## 📄 Licencia
>
> MIT License — Desarrollado por **Lluis Soberats**
