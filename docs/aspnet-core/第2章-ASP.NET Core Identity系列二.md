# 🧱 ASP.NET Core Identity 系列之一

作者：桂兵兵（Bill Gui）

---

`ASP.NET Core Identity` 提供了一组工具包和 API，帮助我们在应用程序中创建**认证和授权功能**，包括账户创建、登录（用户名和密码）、角色与权限管理等功能。它支持使用 SQL Server 或其他第三方数据库存储用户数据、角色和配置信息。

本系列中我们使用 Visual Studio 中自带的 **LocalDB** 作为演示数据库，你也可以从官网下载安装：

🔗 [SQL Server Express LocalDB 下载](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb?view=sql-server-ver16)

---

## 1️⃣ 创建项目

学习 ASP.NET Core Identity 最好的方式是**通过项目实践**。我们将创建一个 ASP.NET Core MVC 项目，命名为 `Identity`，并安装必要的包。

---

## 2️⃣ 配置项目

安装以下 NuGet 包：

```bash
Microsoft.AspNetCore.Identity.EntityFrameworkCore
Microsoft.EntityFrameworkCore.Design
Microsoft.EntityFrameworkCore.SqlServer
```

---

## 3️⃣ 配置中间件

在 `Program.cs` 中添加认证和授权中间件，添加位置为 `app.UseRouting()` 之后：

```csharp
app.UseAuthentication();
app.UseAuthorization();
```

---

## 4️⃣ 设置 ASP.NET Core Identity

### 🔹 定义 User 类

在 `Models` 文件夹下创建 `AppUser.cs`：

```csharp
namespace Identity.Models {
    public class AppUser : IdentityUser { }
}
```

> `AppUser` 继承自 `IdentityUser`，可添加自定义字段。

常用属性如下：

| 名称            | 描述                           |
|-----------------|--------------------------------|
| `Id`            | 用户唯一 ID                    |
| `UserName`      | 用户名                         |
| `Email`         | 用户邮箱                       |
| `PasswordHash`  | 用户密码的 Hash 值             |
| `PhoneNumber`   | 用户电话号码                   |
| `SecurityStamp` | 用户数据变更时的随机标记       |

---

### 🔹 创建 DbContext

在 `Models` 文件夹中添加 `AppIdentityDbContext.cs`：

```csharp
namespace Identity.Models {
    public class AppIdentityDbContext : IdentityDbContext<AppUser> {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
            : base(options) { }
    }
}
```

---

### 🔹 添加数据库连接字符串

在 `appsettings.json` 中添加：

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\MSSQLLocalDB;Database=IdentityDB;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

---

### 🔹 注册 DbContext

```csharp
builder.Services.AddDbContext<AppIdentityDbContext>(options =>
    options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));
```

---

## 5️⃣ 添加 Identity 服务

```csharp
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AppIdentityDbContext>()
    .AddDefaultTokenProviders();
```

说明：

- `AddIdentity`：注册用户和角色类
- `AddEntityFrameworkStores`：使用 EF Core 作为存储方式
- `AddDefaultTokenProviders`：启用默认 Token 提供器（支持找回密码、双因子认证等）

---

## 6️⃣ 使用 EF Migration 创建数据库

### 🔧 安装 CLI 工具

```bash
dotnet tool install --global dotnet-ef
```

### 🔧 添加 Migration 和创建数据库

进入项目目录（含 `.csproj` 文件），运行以下命令：

```bash
dotnet ef migrations add InitDBCommand
dotnet ef database update
```

### 💡 或者使用 Visual Studio 控制台命令

```bash
Add-Migration InitDBCommand
Update-Database
```

---

## 7️⃣ 查看数据库结构

数据库共包含 8 张表：

| 表名                    | 描述                                 |
|-------------------------|--------------------------------------|
| `_EFMigrationsHistory`  | Migration 记录                       |
| `AspNetRoles`           | 存储所有角色                         |
| `AspNetUsers`           | 存储所有用户                         |
| `AspNetUserRoles`       | 用户与角色的映射                     |
| `AspNetUserClaims`      | 用户 Claims                          |
| `AspNetRoleClaims`      | 角色 Claims                          |
| `AspNetUserLogins`      | 用户外部登录信息                     |
| `AspNetUserTokens`      | 外部认证 Token（如重置密码 Token）   |

---

## ✅ 总结

本节介绍了 ASP.NET Core Identity 的初始化和配置，包括：

- 创建用户类
- 配置 DbContext
- 配置连接字符串
- 注册 Identity 服务
- 使用 Migration 创建数据库

我们现在可以开始：添加用户、管理用户、添加角色、角色授权、用户认证等操作。

👉 下一节将继续深入探讨 Identity 功能的使用方式。

---

**源代码地址**  
[🔗 GitHub - aspnetcore-developer/src/01-Basics/Identity](https://github.com/bingbing-gui/aspnetcore-developer/tree/master/src/01-Basics/Identity)

---

© 2025 桂兵兵（Bill Gui），本文内容为作者原创，允许非商业转载，须注明作者及来源，商业用途请联系作者授权。