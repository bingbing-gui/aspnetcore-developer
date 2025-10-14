## 101. 什么是身份验证（Authentication）与授权（Authorization）？

- 身份验证（Authentication）：用于确认用户的真实身份，即“你是谁”。
- 授权（Authorization）：在身份验证通过后，判断该用户是否有权限执行某项操作，即“你能做什么”。

在 ASP.NET Core 中，这两者通常由中间件与 `[Authorize]` 特性协同实现，可基于角色（Roles）或策略（Policies）进行权限控制。

## 102. 如何配置 ASP.NET Core Identity

ASP.NET Core Identity 提供一整套用户管理功能，包括注册、登录、角色、声明、令牌与双因素认证等。

```bash
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
```

```csharp
services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
```

该配置启用基于 Entity Framework Core 的用户存储，并注册默认的令牌提供程序，即可实现常见的身份管理功能。

## 103. JWT Bearer Tokens：是什么及如何配置

JWT（JSON Web Token）是一种轻量级、URL安全的令牌格式，用于无状态身份验证。

示例配置：

```csharp
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("your-secret-key"))
        };
    });
```

使用 `[Authorize]` 特性即可保护 API 端点，实现基于令牌的访问控制。

## 104. 角色授权 vs 策略授权

- 基于角色（Role-based），通过角色控制访问权限，简单直观：

```csharp
[Authorize(Roles = "Admin")]
```

- 基于策略（Policy-based），通过自定义策略和声明（Claims）实现更灵活的控制：


```csharp
services.AddAuthorization(options =>
{
    options.AddPolicy("CanEdit", policy =>
        policy.RequireClaim("EditPermission"));
});
```

使用方式：

```csharp
[Authorize(Policy = "CanEdit")]
```

Role-based 适合简单权限；Policy-based 更灵活，可扩展复杂逻辑与声明验证。

## 105. 使用 Claims

Claims（声明）表示用户的属性或权限，例如邮箱、角色、操作权限等。

添加声明示例：

```csharp
var claims = new List<Claim>
{
    new Claim(ClaimTypes.Name, user.UserName),
    new Claim("Permission", "Edit")
};
```

在代码中访问声明：

```csharp
var permission = User.FindFirst("Permission")?.Value;
```

声明使认证系统更灵活，可基于用户属性或自定义权限进行授权控制。

## 106. 基于 Cookie 的身份验证

Cookie 认证常用于传统 MVC 应用，实现基于会话的登录机制。

配置示例：

```csharp
services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
    });
```

登录时创建会话：

```csharp
await HttpContext.SignInAsync(principal);
```

浏览器会保存认证 Cookie，并在每次请求时自动携带，从而维持用户的登录状态。

## 107. 外部登录（OAuth / OpenID Connect）

ASP.NET Core 支持通过内置提供程序集成第三方登录：

```csharp
services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = "...";
        options.ClientSecret = "...";
    });
```

还支持：
- Facebook
- Microsoft
- Twitter
- OpenID Connect
- Azure AD

可使用 `RemoteAuthenticationHandler<T>` 或 Identity 脚手架（Scaffolding）实现外部认证集成。

## 108. 刷新令牌（Refresh Tokens）

刷新令牌用于在 JWT 过期后，无需重新登录即可获取新的访问令牌。

基本流程：
- 同时签发 Access Token 与 Refresh Token。
- 将刷新令牌安全存储（如数据库或 HTTP-only Cookie）。
- 当访问令牌过期时，客户端使用刷新令牌请求新的访问令牌。

注意：刷新令牌机制需自行实现，ASP.NET Core Identity 并未内置此功能。

## 109. 令牌过期与吊销（Token Expiration & Revocation）

- JWT 通常有效期为 5–30 分钟，过期后将失效。
- 过期令牌无法继续访问受保护资源。
- 吊销（Revocation）通常通过维护“黑名单”（存储已吊销令牌的数据库）实现。

设置过期时间示例：
```csharp
Expires = DateTime.UtcNow.AddMinutes(30);
```

建议配合刷新令牌（Refresh Token）机制，安全地续签访问令牌。

## 110. 使用 [Authorize] 与 [AllowAnonymous] 保护路由

- `[Authorize]`：要求用户已通过身份验证。
- `[Authorize(Roles = "Admin")]`：仅允许特定角色访问。
- `[AllowAnonymous]`：允许匿名访问，无需登录。

```csharp
[Authorize]
public IActionResult Dashboard() { }

[AllowAnonymous]
public IActionResult Login() { }
```

## 111. 自定义授权策略与处理程序（Custom Authorization Policies & Handlers）

目标：基于业务规则（如“年满 18 岁”）做精细化授权。

1) 定义 Requirement 与 Handler
```csharp
public sealed class MinimumAgeRequirement : IAuthorizationRequirement
{
    public int Age { get; }
    public MinimumAgeRequirement(int age) => Age = age;
}

public sealed class MinimumAgeHandler : AuthorizationHandler<MinimumAgeRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        MinimumAgeRequirement requirement)
    {
        // 例：从声明中取出生日期并计算年龄
        var dob = context.User.FindFirst("dob")?.Value; // 形如 2000-01-01
        if (DateTime.TryParse(dob, out var birth))
        {
            var age = (int)((DateTime.UtcNow - birth).TotalDays / 365.2422);
            if (age >= requirement.Age)
                context.Succeed(requirement); // 满足则标记成功
        }

        // 不满足时可不显式 Fail（留空即失败），除非要“立即且明确地”拒绝
        return Task.CompletedTask;
    }
}
```

2) 在 Program.cs 注册策略与处理程序
```csharp
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AgePolicy", policy =>
        policy.Requirements.Add(new MinimumAgeRequirement(18)));
});

// 注册自定义 Handler（可 Singleton/Scoped，通常建议 Singleton）
builder.Services.AddSingleton<IAuthorizationHandler, MinimumAgeHandler>();
```
说明：AgePolicy 来自 AddPolicy("AgePolicy", ...) 中自定义的策略名，并与 Requirement 绑定。

3) 控制器/端点上使用
```csharp
[Authorize(Policy = "AgePolicy")]
[HttpGet("/adult-content")]
public IActionResult GetAdultContent() => Ok("Access granted.");
```

4) 进阶用法（可选）

- 复合规则（断言）：
```csharp
options.AddPolicy("EditOrAdmin", p =>
    p.RequireAssertion(ctx =>
        ctx.User.IsInRole("Admin") ||
        ctx.User.HasClaim("Permission", "Edit")));
```

- 多 Requirement（全部满足才通过）：
```csharp
options.AddPolicy("StrictPolicy", p =>
{
    p.RequireAuthenticatedUser();
    p.RequireClaim("tenant");
    p.Requirements.Add(new MinimumAgeRequirement(21));
});
```


## 112. 数据保护（Data Protection）

ASP.NET Core 内置 Data Protection API，用于：
- 加密身份验证 Cookie
- 保护敏感令牌（如密码重置令牌）

配置示例：

```csharp
services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("path"))
    .SetApplicationName("AppName");
```

该机制被 Identity 与 Cookie 中间件自动使用。

## 113. 多租户身份验证（Multi-tenant Auth）

多租户场景可能包含：
- 独立数据库或用户存储
- 每租户自定义角色与策略

常见策略：
- 在 Claims 中保存 Tenant ID
- 按租户过滤数据
- 使用中间件解析当前租户

可结合 IdentityServer4 或 Azure AD B2C 实现联合认证。

## 114. 密码存储安全（Securing Password Storage）

ASP.NET Core Identity 默认使用 PBKDF2 进行密码哈希。

最佳实践：
- 不存储明文密码
- 使用盐 + 哈希
- 使用 `PasswordHasher<T>` 或 Identity 默认实现

示例：
```csharp
PasswordHasher<T>.HashPassword(user, password);
```

可通过自定义哈希器改用 Argon2 或 Bcrypt。

## 115. 双因素认证（Two-Factor Authentication, 2FA）

ASP.NET Core Identity 原生支持多种 2FA 方式：
- 短信（SMS）
- 邮件（Email）
- 身份验证器应用（TOTP）

启用示例：
```csharp
services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
});
```

使用 `SignInManager<T>` 可处理验证码生成与验证。
