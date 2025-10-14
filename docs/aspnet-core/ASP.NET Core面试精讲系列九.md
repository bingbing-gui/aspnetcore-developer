
## 125. 实现 API 版本控制

API 版本控制允许同一接口支持多个版本，方便客户端平滑迁移与向后兼容。

常见的版本标识方式：

- URL 路径：`/api/v1/products`
- 查询字符串：`/api/products?api-version=1.0`
- 请求头：`api-version: 1.0`
- 媒体类型：`Accept: application/json;v=1`

在 ASP.NET Core 中，可通过安装 NuGet 包实现：

```bash
dotnet add package Microsoft.AspNetCore.Mvc.Versioning
```

配置示例：

```csharp
services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true; // 未指定时使用默认版本
    options.DefaultApiVersion = new ApiVersion(1, 0);   // 默认版本 1.0
    options.ReportApiVersions = true;                   // 在响应头中显示支持的版本
});
```


## 126. 语义化版本/版本协商

- 语义化版本（SemVer）
    - 格式：MAJOR.MINOR.PATCH（例如：1.2.0）。
    - MAJOR（主版本）：有破坏性变更，不兼容旧版本。
    - MINOR（次版本）：新增功能，但保持向后兼容。
    - PATCH（修订版）：修复缺陷，完全向后兼容。

- 版本协商（Version Negotiation）
    - 客户端与服务器可通过请求头或 URL 协调所使用的 API 版本。
    - 服务器应同时支持多个版本，并在响应中返回受支持版本信息，确保客户端平滑升级。


## 127. 弃用策略（Deprecation Strategy）

为了保证 API 平滑演进，应在弃用旧版本时提供清晰的迁移指引。常见做法包括：

- 在文档与响应头中标记弃用（如 api-deprecated-versions 或自定义 Warning 头）。
- 返回提示信息，提醒客户端该版本即将下线。
- 保留旧版本一段过渡期，为客户端预留迁移时间。
- 可选：引入“日落策略”（Sunset Policy），通过 Sunset 响应头或通知端点说明下线时间与替代版本。


## 128. CORS：是什么？

CORS（Cross-Origin Resource Sharing，跨域资源共享）是一种浏览器安全机制，用于在遵守同源策略的前提下，受控地放行跨域资源访问，降低跨站风险（如 XSS、CSRF）。

当网页对不同源（协议、域名或端口不同）的服务器发起请求时：
- 简单请求：浏览器直接发送，请求完成后根据响应头判断是否放行响应。
- 需预检的请求：浏览器会先发送一个 OPTIONS 预检请求，确认目标服务器是否允许该跨域访问。

服务器通过响应头声明授权范围，常见字段包括：
- Access-Control-Allow-Origin：允许的来源（如 https://example.com 或 *）
- Access-Control-Allow-Methods：允许的方法（如 GET, POST, PUT）
- Access-Control-Allow-Headers：允许的自定义请求头
- Access-Control-Expose-Headers：可被前端代码访问的响应头
- Access-Control-Allow-Credentials：是否允许携带凭据（Cookie、Authorization 等）
- Access-Control-Max-Age：预检结果在浏览器中的缓存时长

## 129. 在 ASP.NET Core 中设置 CORS 策略

在 Startup.cs 或 Program.cs 中可通过 AddCors 注册跨域策略：

```csharp
services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins("https://example.com") // 允许的前端来源
               .AllowAnyHeader()                   // 允许所有请求头
               .AllowAnyMethod();                  // 允许所有 HTTP 方法
    });
});
```

启用中间件：

```csharp
app.UseCors("AllowSpecificOrigin");
```

## 130. 预检请求（Preflight Request）

- 当跨域请求使用非简单方法（如 PUT、DELETE）或携带自定义请求头时，浏览器会先发送一个 `OPTIONS` 请求，称为“预检请求”。
- 服务器需在响应中声明允许的来源（Origin）、方法（Methods）和请求头（Headers）。
- 在 ASP.NET Core 中，若已正确配置 CORS 策略（通过 `AddCors` 与 `UseCors`），框架会自动处理预检请求，无需手动编写逻辑。


## 131. 全局配置与按端点配置 CORS

全局 CORS：在请求管道早期调用 app.UseCors(...)，为所有端点统一应用策略。

按端点 CORS：在控制器或 Action 上使用特性选择性启用：

[EnableCors("PolicyName")]
[DisableCors]

适用于只需特定接口开放跨域的场景。

## 132. 处理跨域凭据


若需要在跨域请求中携带 Cookie 或身份凭据：

builder.WithOrigins("https://example.com")
       .AllowCredentials()
       .AllowAnyHeader()
       .AllowAnyMethod();


客户端必须以：

fetch(url, { credentials: 'include' })


方式发送请求。

⚠️ 注意：启用 .AllowCredentials() 时，不得与 .AllowAnyOrigin() 同时使用，否则浏览器会阻止请求。

## 133. CORS 的安全影响


错误的 CORS 配置可能导致 CSRF 或 敏感数据泄露。

切勿同时使用 .AllowAnyOrigin() 与 .AllowCredentials()。

严格限制允许的来源（仅可信域）。

校验 Origin 与 Access-Control-* 请求头，避免过宽策略。

始终使用 HTTPS 确保跨域通信安全。
