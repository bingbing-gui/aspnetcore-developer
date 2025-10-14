## 86. appsettings.json 与 appsettings.{Environment}.json 的作用

- `appsettings.json`：所有环境通用的基础配置。
- `appsettings.{Environment}.json`：按环境（如 Development、Staging、Production）覆盖配置。
- 加载顺序：先加载基础配置，后加载环境专属配置，后者会覆盖前者。
- 未指定环境时，默认使用 Production。
- 设置运行环境示例：

    ```bash
    # Linux / macOS
    export ASPNETCORE_ENVIRONMENT=Development

    # Windows PowerShell
    $env:ASPNETCORE_ENVIRONMENT = "Development"

    :: Windows CMD（持久化，需新开终端生效）
    setx ASPNETCORE_ENVIRONMENT Development
    ```
---

## 87. 基于环境的配置（Development / Staging / Production）

运行环境由 `ASPNETCORE_ENVIRONMENT` 环境变量决定，未设置时默认为 `Production`。
环境名称约定为：`Development`、`Staging`、`Production`（也可自定义，但需与配置文件名一致）。
配置文件加载顺序：先加载 `appsettings.json`，再加载 `appsettings.{Environment}.json`，后者会覆盖前者。
在代码中可通过注入 `IWebHostEnvironment` 判断当前环境：

```csharp
var env = app.Services.GetRequiredService<IWebHostEnvironment>();
if (env.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
}
```
常见用途：切换连接字符串、日志级别、功能开关，或仅在开发环境启用 Swagger、详细错误页等。

---

## 88. 通过环境变量与命令行覆盖配置

ASP.NET Core 配置支持分层覆盖，优先级从低到高依次为：

1. `appsettings.json`
2. `appsettings.{Environment}.json`
3. User Secrets（仅开发环境）
4. 环境变量
5. 命令行参数（最高优先级）

### 环境变量覆盖示例

环境变量使用双下划线 `__` 表示层级：

- **Linux/macOS**
    ```bash
    export MyApp__Logging__LogLevel__Default=Warning
    ```
- **Windows PowerShell**
    ```powershell
    $env:MyApp__Logging__LogLevel__Default = "Warning"
    ```

### 命令行参数覆盖示例

命令行参数使用冒号 `:` 表示层级：

```bash
dotnet run --Logging:LogLevel:Default=Debug
# 或
dotnet YourApp.dll --ConnectionStrings:Default="Host=127.0.0.1;..."
```

> 提示：环境变量用 `__`，命令行用 `:`，都可映射到配置树的对应层级，实现灵活覆盖。

---

## 89. 使用 IConfiguration 读取配置

通过依赖注入获取 `IConfiguration`，可使用键路径（如 `"MySettings:ApiKey"`）或 `GetValue<T>` 方法读取配置项，嵌套层级用冒号 `:` 分隔。

访问子节可用 `GetSection("MySettings")`，如需大量配置建议使用 Options 模式。

```csharp
public class MyService
{
    private readonly string _apiKey;
    private readonly int _timeout;

    public MyService(IConfiguration config)
    {
        _apiKey  = config["MySettings:ApiKey"];                // 读取字符串
        _timeout = config.GetValue<int>("MySettings:Timeout", 30); // 读取整型并设置默认值
        var section = config.GetSection("MySettings");          // 获取子节
    }
}
```
---

## 90. 将配置节绑定到 POCO（Options 模式）

定义 POCO 类并注册绑定：

```csharp
public class MySettings
{
    public string ApiKey { get; set; }
    public int Timeout { get; set; }
}

// Program.cs
builder.Services.Configure<MySettings>(
    builder.Configuration.GetSection("MySettings"));
```

在代码中通过依赖注入使用：

```csharp
public class MyService
{
    private readonly MySettings _cfg;
    public MyService(IOptions<MySettings> options)
    {
        _cfg = options.Value; // 可访问 _cfg.ApiKey / _cfg.Timeout
    }
}
```

> 提示：如需热重载可用 `IOptionsSnapshot<MySettings>`；如需数据注解校验可用  
> `AddOptions<MySettings>().Bind(...).ValidateDataAnnotations().ValidateOnStart();`

---

## 91. Options 模式（IOptions<T>/IOptionsSnapshot<T>/IOptionsMonitor<T>）

- **IOptions<T>**：应用启动时读取一次配置，适合单例服务使用，配置不会随文件热更而变化。
- **IOptionsSnapshot<T>**：每次请求获取最新配置，仅可在Scoped或Transient服务中注入，需配合`reloadOnChange: true` 使用。
- **IOptionsMonitor<T>**：支持配置变更通知（`OnChange`），可在单例服务中使用，线程安全，实时获取最新配置。

```csharp
// 单例服务注入 IOptions<T>
public class SvcWithOptions(IOptions<MySettings> opt) {
    MySettings cfg = opt.Value; // 启动时快照
}
// Scoped/Transient 服务注入 IOptionsSnapshot<T>
public class SvcWithSnapshot(IOptionsSnapshot<MySettings> snap) {
    MySettings cfg = snap.Value; // 每请求最新
}
// 单例服务注入 IOptionsMonitor<T>，支持变更通知
public class SvcWithMonitor : IDisposable
{
    private readonly IOptionsMonitor<MySettings> _mon;
    private readonly IDisposable _sub;

    public SvcWithMonitor(IOptionsMonitor<MySettings> mon)
    {
        _mon = mon;
        _sub = _mon.OnChange(newCfg => {
            // 配置变更时回调处理
        });
    }
    public void Dispose() => _sub.Dispose();
}
```

> 小贴士：如需数据注解校验和启动时校验，可用  
> `builder.Services.AddOptions<MySettings>().Bind(...).ValidateDataAnnotations().ValidateOnStart();`

---

## 92. 机密管理（User Secrets、Azure Key Vault）

### 本地开发：User Secrets（不入库）

- 初始化 User Secrets（仅开发环境，数据存储于本地，不会提交到版本库）：
    ```bash
    dotnet user-secrets init
    dotnet user-secrets set "MySettings:ApiKey" "secret"
    ```
- 读取方式：
    ```csharp
    var apiKey = builder.Configuration["MySettings:ApiKey"];
    ```

### 生产环境：Azure Key Vault（配合托管身份）

- 通过 Azure Key Vault 管理机密，推荐结合托管身份（Managed Identity）：
    ```csharp
    using Azure.Identity;
    using Azure.Security.KeyVault.Secrets;

    var kvUri = new Uri(builder.Configuration["KeyVaultUri"]); // 例如 https://<name>.vault.azure.net/
    var client = new SecretClient(kvUri, new DefaultAzureCredential());
    builder.Configuration.AddAzureKeyVault(client, new Azure.Extensions.AspNetCore.Configuration.Secrets.AzureKeyVaultConfigurationOptions());
    ```
- 在 Azure 上为应用（App Service / Container Apps / VM / Functions）启用托管身份，并在 Key Vault 中授予访问权限，无需在代码中存放凭据。

> 最佳实践：开发用 User Secrets，生产用 Key Vault，敏感信息不入库、不写入配置文件。

---

## 93. 日志配置（设置日志级别）

在 `appsettings.json` 中可按类别设置日志级别，开发环境可用 `appsettings.Development.json` 覆盖：

```json
{
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft": "Warning",
            "Microsoft.Hosting.Lifetime": "Information",
            "Microsoft.AspNetCore": "Warning"
        }
    }
}
```

在代码中可进一步筛选和调整日志输出：

```csharp
var builder = WebApplication.CreateBuilder(args);

// 添加日志筛选器
builder.Logging.AddFilter("System.Net.Http.HttpClient", LogLevel.Warning);

// 使用 ILogger<T> 记录日志
var app = builder.Build();
app.MapGet("/ping", (ILoggerFactory lf) => {
        var log = lf.CreateLogger("Demo");
        log.LogInformation("pong");
        return "pong";
});
```

常用日志提供程序包括 Console、Debug、EventSource、Azure Monitor（App Insights）等。

---

## 94. 连接字符串管理（精简）

### 配置文件示例（appsettings.json）

```json
{
    "ConnectionStrings": {
        "DefaultConnection": "Server=.;Database=AppDb;Trusted_Connection=True;"
    }
}
```

### 读取连接字符串

```csharp
var conn = builder.Configuration.GetConnectionString("DefaultConnection");
```

### 用于 DbContext（SQL Server / PostgreSQL）

```csharp
// SQL Server
builder.Services.AddDbContext<AppDbContext>(
        o => o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// PostgreSQL
// builder.Services.AddDbContext<AppDbContext>(
//     o => o.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
```

### 覆盖方式（生产常用）

- 环境变量覆盖：  
    `ConnectionStrings__DefaultConnection="Host=127.0.0.1;..."`

---

## 95. 配置变更自动重载（视来源而定）

JSON 文件支持热重载（文件修改后自动更新配置树）：

```csharp
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
```

IOptionsMonitor<T> 自动跟随变更（适合单例读取），可订阅回调：

```csharp
public class MySvc(IOptionsMonitor<MySettings> mon)
{
    private readonly IDisposable _sub = mon.OnChange(newCfg => { /* 配置变更时执行 */ });
}
```
并非所有来源都支持热重载：如环境变量、命令行不支持；JSON/INI 等文件源支持。

---

## 96. 强类型配置（Options + POCO）

使用 POCO 类承载配置，结合 Options 模式实现类型安全和数据校验。

### 1. 定义 POCO（支持数据注解）

```csharp
public class MySettings
{
    [Required] 
    public string ApiKey { get; set; }
    [Range(1, 120)] 
    public int Timeout { get; set; } = 30;
}
```

### 2. 绑定与校验（Program.cs）

```csharp
builder.Services.AddOptions<MySettings>()
    .Bind(builder.Configuration.GetSection("MySettings"))
    .ValidateDataAnnotations() // 按数据注解校验
    .ValidateOnStart();        // 启动时校验，失败抛异常
```

### 3. 使用配置

```csharp
public class MySvc(IOptions<MySettings> opt)
{
    private readonly MySettings _cfg = opt.Value;
}
```

---

## 97. 配置验证（IValidateOptions<T>，精简）

可对已绑定的配置进行业务级校验，支持与 `ValidateDataAnnotations()` 叠加，并可针对命名选项（name 参数）实现差异化验证。

### 示例：自定义验证器

```csharp
using Microsoft.Extensions.Options;

public sealed class MySettingsValidator : IValidateOptions<MySettings>
{
    public ValidateOptionsResult Validate(string name, MySettings options)
    {
        if (options is null)
            return ValidateOptionsResult.Fail("Options instance is null.");

        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(options.ApiKey))
            errors.Add("ApiKey is required.");

        if (options.Timeout is < 1 or > 120)
            errors.Add("Timeout must be between 1 and 120.");

        return errors.Count == 0
            ? ValidateOptionsResult.Success
            : ValidateOptionsResult.Fail(errors);
    }
}
```

### 注册方式（推荐组合）

```csharp
// 绑定 + 数据注解校验
builder.Services.AddOptions<MySettings>()
    .Bind(builder.Configuration.GetSection("MySettings"))
    .ValidateDataAnnotations()
    .ValidateOnStart(); // 生产环境建议启用，开发可按需关闭

// 业务规则校验
builder.Services.AddSingleton<IValidateOptions<MySettings>, MySettingsValidator>();
```

### 小贴士

- 简单规则可用委托：
    ```csharp
    .Validate(s => Uri.IsWellFormedUriString(s.Endpoint, UriKind.Absolute), "Endpoint invalid")
    ```
- 按名称校验时，可在 `Validate(string name, ...)` 中判断 name 是否为目标配置名。
- 可与数据注解校验叠加，确保配置安全、合规。

---

## 98. 配置提供程序（JSON / XML / INI / 环境变量 / Azure 等）

ASP.NET Core 支持多种配置源，并按添加顺序依次加载，后添加的会覆盖前面的。常见配置来源包括：

- **JSON 文件**：如 `appsettings.json`、`appsettings.{Environment}.json`（默认推荐）。
- **环境变量**：支持层级结构，使用双下划线 `__` 表示嵌套。
- **命令行参数**：层级用冒号 `:` 分隔，优先级高于环境变量。
- **INI 文件**：可选，适合简单配置。
- **XML 文件**：可选，兼容部分旧系统。
- **内存配置**：通过代码动态添加，适合测试或临时数据。
- **Azure App Configuration**：集中管理云端配置。
- **Azure Key Vault**：安全存储机密和敏感信息。
- **User Secrets**：本地开发专用，避免机密入库。

> 配置源可灵活组合，按实际需求和安全要求选择合适的提供程序。

---

## 99. 默认值与可选配置

POCO 默认值与可空类型：
```csharp
public class MySettings
{
    public string ApiKey { get; set; } = ""; // 默认空，不硬编码密钥
    public int Timeout { get; set; } = 30;   // 默认值
    public string? Region { get; set; }      // 可选配置
}
```
读取配置时可设置回退值：
```csharp
var apiKey = config.GetValue("MySettings:ApiKey", "fallback");
// 或
var apiKey2 = config["MySettings:ApiKey"] ?? "fallback";
```

> 建议：重要项用 `IOptions<T>` + 校验（如 `ValidateDataAnnotations()` 或自定义 `IValidateOptions<T>`）；机密项不要设置硬编码默认值。

---

## 100. 屏蔽敏感配置数据


- 不要记录密钥、令牌、连接串等敏感信息。
- 序列化时可用特性忽略：

```csharp
public class MySettings
{
    [JsonIgnore] 
    public string ApiKey { get; set; } = "";
    public string Endpoint { get; set; } = "";
}
```

- 日志中脱敏（如仅保留末 4 位）：

```csharp
string Mask(string s) => string.IsNullOrEmpty(s) ? "" : new string('*', Math.Max(0, s.Length - 4)) + s[^4..];
_logger.LogInformation("Token: {Token}", Mask(token));
```

- 避免直接“倾倒配置”（如将 `Configuration.AsEnumerable()` 写入日志）。
- 机密存储推荐用 User Secrets、环境变量或 Azure Key Vault，避免写入版本库。

```csharp
string Mask(string s) => string.IsNullOrEmpty(s) ? "" : new string('*', Math.Max(0, s.Length - 4)) + s[^4..];
_logger.LogInformation("Token: {Token}", Mask(token));
```
- 避免直接“倾倒配置”（如将 `Configuration.AsEnumerable()` 写入日志）。
- 机密存储推荐用 User Secrets、环境变量或 Azure Key Vault，避免写入版本库。

---
