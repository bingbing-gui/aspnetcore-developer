# Scalar：现代化的开源 API 开发者体验平台

**Scalar** 是一个现代化的开源开发者体验平台，专为 API 而生。  
通过内置的交互式 **Playground**，你可以创建世界级的 API 文档，并且能够无缝切换为功能齐全的 API 客户端。  

---

## 🚀 官方资源
- Scalar 官方文档: https://scalar.com/#api-docs  
- Scalar GitHub 地址: https://github.com/scalar/scalar  

---

## ⚙️ 在 ASP.NET Core 中使用 Scalar

### 1. 安装 NuGet 包

```bash
dotnet add package Scalar.AspNetCore
```

### 2. 添加 using 指令

```csharp
using Scalar.AspNetCore;
```

### 3. 在 Program.cs 中配置

根据不同的 OpenAPI 生成器，有两种配置方式：

```csharp

var builder = WebApplication.CreateBuilder();

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
}

app.MapGet("/", () => "Hello world!");

app.Run();
```

## 运行效果

启动 ASP.NET Core 项目后，访问 `/scalar` 路径。

> ![Scalar API Reference 示例](/aspnetcore-developer/docs/ASP.Net%20Core/Materials/scalar-01.png)

> ![Scalar API Playground 示例](/aspnetcore-developer/docs/ASP.Net%20Core/Materials/scalar-01.png)

## 🧑‍💻 示例项目

ASP.NET Core 集成 Scalar 示例（GitHub Demo）: https://github.com/bingbing-gui/aspnetcore-developer/tree/master/src/02-WebAPI/OpenAPI/Scalar
---

## 📌 总结

通过 Scalar.AspNetCore NuGet 包，你可以在 ASP.NET Core 项目中轻松集成 Scalar，快速生成现代化的 API 文档，并为开发者提供交互式的 API 调试体验。