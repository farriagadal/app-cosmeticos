# 🛍️ Cosmetics App - Guía de Instalación Completa

Esta guía te llevará paso a paso para instalar y ejecutar la aplicación de ecommerce de cosméticos en tu máquina Windows local.

## 📋 Requisitos Previos

Antes de comenzar, necesitas instalar los siguientes programas:

**Elige UNA de estas dos opciones:**

### Opción A: Visual Studio 2022 Community (Completo)
- Descarga desde: https://visualstudio.microsoft.com/es/vs/community/
- Durante la instalación, selecciona:
  - ✅ **Desarrollo de ASP.NET y web**
  - ✅ **Desarrollo multiplataforma de .NET**
  - ✅ **Git para Windows** (si no lo tienes instalado)

**✅ Ventajas:**
- Todo integrado (debugger, NuGet, Entity Framework)
- Interfaz visual para Entity Framework
- Mejor IntelliSense y refactoring
- Ideal para principiantes

**❌ Desventajas:**
- Consume más recursos (RAM/CPU)
- Instalación más pesada (~3GB)
- Menos personalizable

### Opción B: Visual Studio Code (Ligero)
- Descarga desde: https://code.visualstudio.com/
- **Instala .NET SDK 6.0 o superior** desde: https://dotnet.microsoft.com/download
- **Instala Git para Windows** desde: https://git-scm.com/download/win
- **Extensiones requeridas en VS Code:**
  - C# (Microsoft)
  - C# Dev Kit (Microsoft)
  - .NET Install Tool (Microsoft)

**✅ Ventajas:**
- Muy ligero y rápido
- Altamente personalizable
- Mejor para desarrollo multiplataforma
- Gratis y open source

**❌ Desventajas:**
- Requiere más configuración manual
- Necesita comandos de terminal
- Menos herramientas visuales integradas

### 🎯 ¿Cuál elegir?

**Recomendamos Visual Studio 2022 si:**
- Eres nuevo en desarrollo .NET
- Prefieres interfaces gráficas
- Quieres todo funcionando sin configuración
- No te importa usar más recursos del sistema

**Recomendamos Visual Studio Code si:**
- Ya tienes experiencia en desarrollo
- Te gusta usar terminal/línea de comandos
- Tu PC tiene recursos limitados
- Prefieres un editor más minimalista y rápido

### 2. MySQL Server
- Descarga desde: https://dev.mysql.com/downloads/mysql/
- Selecciona: **MySQL Installer for Windows**
- Durante la instalación:
  - Tipo de instalación: **Developer Default**
  - Configuración del servidor: **Standalone MySQL Server**
  - Método de autenticación: **Use Strong Password Encryption**
  - Configura una contraseña para el usuario **root** (anótala, la necesitarás)

### 3. MySQL Workbench (Opcional pero recomendado)
- Se instala automáticamente con MySQL Installer
- Te permitirá administrar la base de datos de forma visual

## 🚀 Instalación del Proyecto

### Paso 1: Obtener el Código
```bash
# Si tienes Git instalado:
git clone https://github.com/farriagadal/app-cosmeticos

# O descarga el ZIP del proyecto y descomprímelo
```

### Paso 2: Configurar la Base de Datos

#### 2.1 Crear la Base de Datos
1. Abre **MySQL Workbench**
2. Conecta con tu servidor local usando:
   - Host: `localhost`
   - Port: `3306`
   - Usuario: `root`
   - Contraseña: (la que configuraste durante la instalación)

3. Ejecuta el siguiente comando para crear la base de datos:
```sql
CREATE DATABASE cosmeticos_db;
```

#### 2.2 Configurar la Cadena de Conexión
1. Navega a la carpeta del proyecto
2. Abre el archivo `appsettings.json`
3. Modifica la cadena de conexión con tu configuración:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=cosmeticos_db;Uid=root;Pwd=TU_CONTRASEÑA_AQUI;"
  }
}
```

**⚠️ Importante:** Reemplaza `TU_CONTRASEÑA_AQUI` con la contraseña que configuraste para MySQL.

### Paso 3: Abrir el Proyecto

#### 🔴 Si elegiste Visual Studio 2022:

1. Abre **Visual Studio 2022**
2. Selecciona **Abrir un proyecto o solución**
3. Navega a la carpeta del proyecto y selecciona `CosmeticosApp.csproj`
4. Espera a que Visual Studio cargue el proyecto

#### 🔵 Si elegiste Visual Studio Code:

1. Abre **Visual Studio Code**
2. Ve a **File** → **Open Folder**
3. Selecciona la carpeta del proyecto completa
4. Instala las extensiones requeridas si no las tienes:
   - Presiona `Ctrl+Shift+X`
   - Busca e instala: **C#**, **C# Dev Kit**, **.NET Install Tool**
5. Presiona `Ctrl+Shift+P` y busca **".NET: Restore"** para restaurar dependencias

### Paso 4: Restaurar Dependencias y Crear la Base de Datos

#### 🔴 Si usas Visual Studio 2022:

1. Ve a **Herramientas** → **Administrador de paquetes NuGet** → **Consola del Administrador de paquetes**
2. Ejecuta los siguientes comandos uno por uno:

```bash
# Restaurar dependencias
dotnet restore

# Crear las migraciones (si no existen)
dotnet ef migrations add InitialCreate

# Aplicar las migraciones y crear las tablas
dotnet ef database update
```

#### 🔵 Si usas Visual Studio Code:

1. Abre la terminal integrada: **View** → **Terminal** (o `Ctrl+` `)
2. Ejecuta los siguientes comandos uno por uno:

```bash
# Restaurar dependencias
dotnet restore

# Instalar herramientas Entity Framework (solo la primera vez)
dotnet tool install --global dotnet-ef

# Crear las migraciones (si no existen)
dotnet ef migrations add InitialCreate

# Aplicar las migraciones y crear las tablas
dotnet ef database update
```

### Paso 5: Ejecutar la Aplicación

#### 🔴 Si usas Visual Studio 2022:

1. Presiona **F5** o haz clic en el botón **▶️ Ejecutar**
2. La aplicación se abrirá en tu navegador predeterminado

#### 🔵 Si usas Visual Studio Code:

1. En la terminal, ejecuta:
```bash
dotnet run
```

2. Verás algo como:
```
Now listening on: http://localhost:5000
```

Abrir http://localhost:5000 en el navegador y listo
