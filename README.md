# Proyecto ASP.NET

Este proyecto es una aplicación desarrollada en **ASP.NET Core** con Entity Framework para la gestión de productos.

## 📌 Requisitos Previos

Antes de ejecutar el proyecto, asegúrate de tener instalado:

- [.NET SDK](https://dotnet.microsoft.com/en-us/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) o [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Visual Studio](https://visualstudio.microsoft.com/) o [Visual Studio Code](https://code.visualstudio.com/)

## 🚀 Configuración del Proyecto

### 1️⃣ Clonar el repositorio
```sh
    git clone https://github.com/Mendoza727/loopy.git
    cd tu-repositorio
```

### 2️⃣ Configurar la cadena de conexión
Abre `appsettings.json` y edita la sección `ConnectionStrings`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=MiBaseDeDatos;Trusted_Connection=True;"
}
```
Si usas **SQL Server con autenticación de usuario**, usa:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=MiBaseDeDatos;User Id=tu_usuario;Password=tu_contraseña;"
}
```

### 3️⃣ Instalar dependencias
Ejecuta:
```sh
    dotnet restore
```

## 🛠️ Migraciones con Entity Framework

### 4️⃣ Crear una nueva migración
Si necesitas generar una migración, usa el siguiente comando:
```sh
    dotnet ef migrations add NombreDeLaMigracion
```

### 5️⃣ Aplicar las migraciones a la base de datos
```sh
    dotnet ef database update
```

## ▶️ Ejecutar el Proyecto
Para ejecutar la aplicación en modo desarrollo:
```sh
    dotnet run
```

También puedes ejecutarlo desde Visual Studio presionando `F5` o usando `Ctrl + F5`.

## 📜 Licencia
Este proyecto está bajo la licencia [MIT](LICENSE).

