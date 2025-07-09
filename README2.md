# 🛍️ Cosmética Bella - Ecommerce de Cosméticos

Una aplicación web completa de ecommerce para la venta de productos cosméticos, desarrollada con **ASP.NET Core MVC** y **MySQL**.

## 🚀 Características

- **Catálogo de productos** con filtros por categoría, marca y precio
- **Sistema de carrito de compras** con sesiones
- **Autenticación y autorización** de usuarios
- **Panel de administración** completo
- **Gestión de pedidos** y estados
- **Diseño responsive** y moderno
- **6 entidades principales** (Usuario, Producto, Categoría, Marca, Pedido, DetallePedido)

## 🛠️ Tecnologías Utilizadas

- **Backend**: ASP.NET Core 8.0 MVC
- **Frontend**: HTML5, CSS3, Bootstrap 5, JavaScript/jQuery
- **Base de datos**: MySQL
- **ORM**: Entity Framework Core
- **Autenticación**: ASP.NET Core Identity

## 📋 Requisitos Previos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [MySQL Server 8.0 o superior](https://dev.mysql.com/downloads/mysql/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) o [VS Code](https://code.visualstudio.com/)

## 🔧 Instalación y Configuración

### 1. Clonar el repositorio
```bash
git clone https://github.com/tu-usuario/cosmeticos-app.git
cd cosmeticos-app
```

### 2. Instalar MySQL

#### Windows:
1. Descargar MySQL Installer desde [mysql.com](https://dev.mysql.com/downloads/installer/)
2. Ejecutar el instalador y seleccionar "MySQL Server"
3. Configurar la contraseña root durante la instalación
4. Instalar MySQL Workbench (opcional pero recomendado)

#### macOS:
```bash
brew install mysql
brew services start mysql
mysql_secure_installation
```

#### Ubuntu/Debian:
```bash
sudo apt update
sudo apt install mysql-server
sudo mysql_secure_installation
```

### 3. Configurar la base de datos

1. **Conectar a MySQL**:
```bash
mysql -u root -p
```

2. **Crear la base de datos**:
```sql
CREATE DATABASE cosmeticos_db;
CREATE USER 'cosmeticos_user'@'localhost' IDENTIFIED BY 'tu_contraseña_segura';
GRANT ALL PRIVILEGES ON cosmeticos_db.* TO 'cosmeticos_user'@'localhost';
FLUSH PRIVILEGES;
EXIT;
```

### 4. Configurar la aplicación

1. **Actualizar la cadena de conexión**:
Editar `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=cosmeticos_db;Uid=cosmeticos_user;Pwd=tu_contraseña_segura;"
  }
}
```

2. **Restaurar paquetes NuGet**:
```bash
dotnet restore
```

3. **Crear y aplicar migraciones**:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 5. Ejecutar la aplicación

```bash
dotnet run
```

La aplicación estará disponible en: `https://localhost:5001`

## 👤 Credenciales de Administrador

- **Email**: admin@cosmeticos.com
- **Contraseña**: Admin123!

## 🏗️ Estructura del Proyecto

```
CosmeticosApp/
├── Controllers/           # Controladores MVC
│   ├── HomeController.cs
│   ├── ProductosController.cs
│   ├── CarritoController.cs
│   └── AdminController.cs
├── Models/               # Modelos de datos
│   ├── Usuario.cs
│   ├── Producto.cs
│   ├── Categoria.cs
│   ├── Marca.cs
│   ├── Pedido.cs
│   └── DetallePedido.cs
├── Views/               # Vistas Razor
│   ├── Home/
│   ├── Productos/
│   ├── Carrito/
│   └── Admin/
├── Data/                # Contexto de base de datos
│   └── ApplicationDbContext.cs
├── wwwroot/             # Archivos estáticos
│   ├── css/
│   ├── js/
│   └── images/
└── Program.cs           # Configuración de la aplicación
```

## 🗃️ Entidades del Sistema

### Usuario (hereda de IdentityUser)
- Información personal del usuario
- Historial de pedidos
- Roles (Admin/Cliente)

### Producto
- Información del producto
- Precio y stock
- Relaciones con Categoría y Marca

### Categoría
- Organización de productos
- Descripción e imagen

### Marca
- Información de la marca
- Logo y descripción

### Pedido
- Información del pedido
- Estado y fecha
- Dirección de entrega

### DetallePedido
- Items específicos del pedido
- Cantidad y precios

## 🎨 Funcionalidades Principales

### Para Clientes:
- **Navegación del catálogo** con filtros avanzados
- **Carrito de compras** persistente
- **Proceso de checkout** completo
- **Gestión de perfil** de usuario

### Para Administradores:
- **Dashboard** con estadísticas
- **Gestión de productos** (CRUD completo)
- **Gestión de categorías y marcas**
- **Seguimiento de pedidos**
- **Gestión de usuarios**

## 🔍 Comandos Útiles

### Entity Framework
```bash
# Crear nueva migración
dotnet ef migrations add NombreMigracion

# Aplicar migraciones
dotnet ef database update

# Eliminar última migración
dotnet ef migrations remove

# Ver migraciones aplicadas
dotnet ef database update --verbose
```

### Desarrollo
```bash
# Ejecutar en modo desarrollo
dotnet run --environment Development

# Ejecutar con hot reload
dotnet watch run

# Compilar el proyecto
dotnet build

# Ejecutar tests
dotnet test
```

## 🚨 Solución de Problemas

### Error de conexión a MySQL
- Verificar que MySQL esté ejecutándose
- Comprobar la cadena de conexión
- Verificar permisos de usuario

### Error de migraciones
```bash
# Eliminar base de datos y recrear
dotnet ef database drop
dotnet ef database update
```

### Puerto ocupado
```bash
# Cambiar puerto en launchSettings.json
"applicationUrl": "https://localhost:5002;http://localhost:5001"
```

## 📱 Responsive Design

La aplicación está completamente optimizada para:
- **Desktop** (1200px+)
- **Tablet** (768px - 1199px)
- **Mobile** (< 768px)

## 🔐 Seguridad

- Autenticación ASP.NET Core Identity
- Autorización basada en roles
- Validación de datos del lado servidor
- Protección CSRF
- Sanitización de datos

## 🎯 Próximas Mejoras

- [ ] Sistema de reseñas y calificaciones
- [ ] Integración con pasarelas de pago
- [ ] Notificaciones por email
- [ ] Sistema de wishlist
- [ ] Reportes avanzados
- [ ] API REST para móviles

## 📄 Licencia

Este proyecto fue desarrollado como proyecto universitario.

## 👥 Contribuciones

Las contribuciones son bienvenidas. Para contribuir:

1. Fork el proyecto
2. Crea una rama para tu feature
3. Commit tus cambios
4. Push a la rama
5. Abre un Pull Request

## 📞 Soporte

Para soporte o preguntas, contacta a través de:
- Email: soporte@cosmeticabella.com
- Issues en GitHub

---

⭐ **¡Si te gustó el proyecto, no olvides darle una estrella!** ⭐ 