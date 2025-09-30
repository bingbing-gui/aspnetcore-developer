
## 16. 什么是依赖注入？为什么要使用？

- **定义**  
    依赖注入（Dependency Injection, DI）是一种设计模式：将依赖项（服务/对象）通过外部传递（注入）给类，而不是在类内部直接创建。  

- **优点**  
    - 促进 **松耦合**（类不再依赖具体实现，而是依赖接口/抽象）  
    - 提高 **可测试性**（可以方便替换依赖，例如单元测试中的 Mock）  
    - 实现 **关注点分离**（业务逻辑与对象创建解耦）  

---

## 17. ASP.NET Core 内置 DI 容器：能力与局限

- **能力**  
    - 构造函数注入（官方推荐）  
    - 生命周期管理：瞬态 (Transient)、作用域 (Scoped)、单例 (Singleton)  
    - 支持 `IEnumerable<T>` 注入  
    - 支持开放泛型（如 `IRepository<T> → Repository<T>`）  
    - 可通过 `IServiceProvider` 手动解析  

- **局限**  
    - 不支持 **命名注册**（无法根据名称区分不同实现）  
    - **属性注入**支持有限（需手工扩展或第三方容器）  
    - 功能相对基础，不如 **Autofac、StructureMap** 等第三方容器强大  

---

## 18. 不同服务生命周期：瞬态、作用域、单例

| 生命周期   | 描述                          | 使用场景示例             |
|------------|-------------------------------|--------------------------|
| 瞬态       | 每次解析都会创建新实例        | 轻量级、无状态服务       |
| 作用域     | 每个请求/作用域内一个实例     | 数据库上下文、工作单元   |
| 单例       | 应用程序整个生命周期一个实例 | 配置、日志、缓存服务     |

---

## 19. 在 ConfigureServices 注册服务

```csharp
public void ConfigureServices(IServiceCollection services)
{
        services.AddTransient<IMyService, MyService>();   // 瞬态
        services.AddScoped<IRepository, Repository>();   // 作用域
        services.AddSingleton<ILoggerService, LoggerService>(); // 单例
}
```

---

## 20. 在控制器、Razor Pages、中间件中解析依赖

- **控制器**：构造函数注入（推荐）
- **Razor Pages**：在 PageModel 构造函数注入
- **中间件**：
    - 构造函数注入（推荐）
    - 或通过 `IApplicationBuilder.ApplicationServices` 手动解析（不推荐，除非特殊场景）

---

## 21. 构造函数注入 vs 属性注入

- **构造函数注入**
    - ASP.NET Core 原生支持
    - 推荐方式，保证依赖不可变、清晰

- **属性注入**
    - 内置容器不直接支持
    - 只能通过手工赋值或第三方容器实现

**结论**：推荐构造函数注入

---

## 22. 使用 IServiceProvider / IServiceScopeFactory

- `IServiceProvider`：手动解析服务（一般少用）
- `IServiceScopeFactory`：创建新的作用域，常用于后台任务

**示例：**

```csharp
using (var scope = serviceScopeFactory.CreateScope())
{
        var scopedService = scope.ServiceProvider.GetRequiredService<MyScopedService>();
}
```

---

## 23. 作用域服务在后台任务中的行为

- 默认情况：作用域服务与 HTTP 请求生命周期绑定
- 后台任务：没有请求上下文，不能直接注入 Scoped 服务
- 解决方法：通过 `IServiceScopeFactory` 手动创建作用域来解析 Scoped 服务

---

## 24. 覆盖默认 DI 行为（移除或替换服务）

- **替换服务**

    ```csharp
    services.AddSingleton<IService, CustomImplementation>();
    ```

- **移除服务**

    ```csharp
    var descriptor = services.First(x => x.ServiceType == typeof(IMyService));
    services.Remove(descriptor);
    ```

---

## 25. Options 模式（IOptions, IOptionsSnapshot, IOptionsMonitor）

- `IOptions<T>`  
    单例模式，读取应用启动时的配置快照。适用于单例服务。

- `IOptionsSnapshot<T>`  
    每次请求都会重新计算配置值。生命周期：Scoped（作用域）。适合 Web 应用中的多请求场景。

- `IOptionsMonitor<T>`  
    单例模式，可监听配置变化（回调通知）。适合后台服务、长生命周期组件。

**注册示例：**

```csharp
services.Configure<MySettings>(Configuration.GetSection("MySettings"));
```

---

## 26. IHostedService / BackgroundService 与 DI

- **IHostedService**  
    ASP.NET Core 提供的后台任务接口

- **BackgroundService**  
    `IHostedService` 的抽象基类，简化使用

- **DI 特点**  
    可直接构造函数注入单例/瞬态服务  
    若需要 Scoped 服务，则必须用 `IServiceScopeFactory` 创建新作用域

---

## 27. 注入配置提供者（IConfiguration）

- `IConfiguration` 已自动注册，可通过构造函数注入

**示例：**

```csharp
public class MyService
{
        private readonly IConfiguration _config;
        public MyService(IConfiguration config)
        {
                var key = config["MyKey"];
        }
}
```

---

## 28. 注入日志（ILogger）

- ASP.NET Core 内置日志框架，支持结构化日志

**示例：**

```csharp
public class MyService
{
        private readonly ILogger<MyService> _logger;
        public MyService(ILogger<MyService> logger) => _logger = logger;

        public void DoSomething() 
                => _logger.LogInformation("执行了操作");
}
```

---

## 29. 处理循环依赖

- **定义**：当两个服务直接或间接依赖彼此时产生循环引用

- **常见解决方法**：
    - 重构以消除循环依赖
    - 使用 `Lazy<T>` 或工厂模式延迟创建
    - 拆分服务职责，降低耦合

---

## 30. 在单元测试中模拟依赖

- 可使用 Mock 框架（如 Moq）来替代真实依赖

**示例：**

```csharp
var mockService = new Mock<IMyService>();
mockService.Setup(s => s.Get()).Returns("test");

var controller = new MyController(mockService.Object);
```

**优点**：可隔离测试目标，验证逻辑，而不是依赖外部服务

