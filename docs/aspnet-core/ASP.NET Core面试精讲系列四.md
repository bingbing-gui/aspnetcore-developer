# ASP.NET Core 面试精讲系列（四）

## 51. 什么是 REST？如何在 ASP.NET Core 设计 RESTful API

---

### REST 定义
- **REST（Representational State Transfer）** 是 Roy Fielding 在 2000 年提出的一种架构风格  
- 特点：不是协议，而是一组约束，包括：
    - 客户端-服务器
    - 无状态
    - 可缓存
    - 分层系统
    - 统一接口
    - （可选）按需代码
---

### RESTful API 设计原则
- 使用 **HTTP 方法** 表达操作：  
    - `GET` → 查询  
    - `POST` → 创建（或非幂等操作）  
    - `PUT` → 更新（幂等）  
    - `PATCH` → 部分更新  
    - `DELETE` → 删除  

- **URI 设计**：使用名词，表示资源  
    - ✅ `/users`  
    - ✅ `/users/{id}`  
    - ❌ `/GetUsers`  

- **返回状态码**：  
    - `200 OK`：成功  
    - `201 Created`：资源已创建  
    - `204 No Content`：删除成功，无返回体  
    - `400 Bad Request`：请求无效  
    - `401 Unauthorized` / `403 Forbidden`：权限错误  
    - `404 Not Found`：资源不存在  
    - `500 Internal Server Error`：服务器错误  

- **无状态通信**：每个请求必须包含处理所需的全部信息，服务端不保存客户端状态。  

- **HATEOAS（可选）**：响应中包含指向其他相关资源的链接，驱动客户端发现 API。  

### 总结

- **REST** 是一种架构风格，不是协议。
- RESTful API 的核心原则包括：资源导向、HTTP 方法、无状态通信、合理使用状态码。
- 在 ASP.NET Core 中，结合 `[ApiController]`、路由和 `ActionResult`，可以优雅地实现 RESTful API。

---

## 52. [ApiController] 特性及其优势

`[ApiController]` 是 ASP.NET Core 2.1 引入的特性，用于标记 **Web API 控制器**，使其具备更符合 REST API 的默认行为。  
通常应用在继承自 `ControllerBase` 的类上。  

---

### 主要优势

1. **自动模型验证**
     - 控制器方法执行前会检查 `ModelState`  
     - 如果验证失败，自动返回 `400 Bad Request`，无需显式写 `if (!ModelState.IsValid)`  

2. **自动参数来源推断**
     - 自动根据参数类型和上下文推断绑定来源：  
         - 复杂类型（class） → 默认 `[FromBody]`  
         - 简单类型（int, string 等） → 默认 `[FromRoute]` 或 `[FromQuery]`  
         - 可通过 `[FromBody]`、`[FromForm]`、`[FromQuery]`、`[FromRoute]`、`[FromHeader]` 显式指定覆盖  

3. **改进错误响应**
     - 模型验证失败时返回的 `400` 包含详细的错误信息（`ProblemDetails` 标准格式）  
     - 更便于客户端消费错误响应  

4. **改进参数绑定行为**
     - 对于 `[FromBody]` 参数，要求最多只能有一个，并且是复杂类型  
     - 避免传统 ASP.NET MVC 里可能的多体参数混淆  

5. **更好的约定性**
     - API 控制器默认返回 **一致的 HTTP 状态码与响应格式**  
     - 配合 `ActionResult<T>` 可以自动生成 Swagger/OpenAPI 更规范的文档  

---

### 示例

```csharp
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
        [HttpPost]
        public ActionResult<User> CreateUser(User input)
        {
                // 如果模型验证失败，会自动返回 400
                return CreatedAtAction(nameof(GetUser), new { id = input.Id }, input);
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
                var user = new User { Id = id, Name = "Alice" };
                if (user == null) return NotFound();
                return Ok(user);
        }
}
```
### 总结

- `[ApiController]` 提供了 Web API 的默认行为增强，包括自动模型验证、参数绑定和标准化错误处理。
- 推荐在所有 API 控制器上使用，可减少样板代码、提升一致性和可维护性。
- 结合 `ControllerBase` 和 `ActionResult<T>`，可实现更规范、易用的 RESTful API。

---

## 53. Web API 路由约定（特性路由、路由模板）


### 特性路由（推荐方式）

ASP.NET Core 支持 **属性路由**（Attribute Routing），可直接在控制器和方法上定义路由：

```csharp
[ApiController]
[Route("api/[controller]")] // 控制器级路由前缀
public class ProductsController : ControllerBase
{
        [HttpGet("{id:int}")] // 方法级路由，带类型约束
        public IActionResult Get(int id) => Ok(new { Id = id, Name = "Book" });
}
```

- `[controller]` 会自动替换为控制器类名（去掉 "Controller" 后缀），如 `ProductsController` → `Products`。
- 最终路由示例：`GET /api/products/5`
- ASP.NET Core 路由默认**不区分大小写**，但推荐统一使用小写 URI（如 `/api/products/5`），符合 REST 社区规范。

---

### 路由模板语法

- **占位符**：`{id}` → `/api/products/5`
- **类型约束**：`{id:int}` → 必须为整数
- **可选参数**：`{id?}` → `/api/products` 或 `/api/products/5`
- **默认值**：`{id=1}` → `/api/products` 时 `id` 默认为 1
- **捕获所有**：`{*path}` → 捕获剩余路径

---

### 集中路由（传统方式）

除了特性路由，还可在 `Program.cs` 或 `Startup.cs` 中统一配置集中路由：

```csharp
app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
```

- 示例：`/Products/Get/5`
- 优点：集中管理路由规则
- 缺点：不直观，Web API 推荐使用特性路由

---

### 总结

- **特性路由**：推荐用于 Web API，灵活直观，支持路由模板和约束
- **集中路由**：适合传统 MVC 项目，统一配置
- **大小写**：实际路由匹配不区分大小写，最佳实践是统一采用小写 URI（如 `/api/products/5`）

---

## 54. API 版本管理（URL、Header、Media Type）

API 版本管理可通过 `Microsoft.AspNetCore.Mvc.Versioning` 包实现。  
安装：
```bash
dotnet add package Microsoft.AspNetCore.Mvc.Versioning
```

## 54. API 版本管理（URL、Header、Media Type）

使用 `Microsoft.AspNetCore.Mvc.Versioning` 包。

```csharp
services.AddApiVersioning(options =>
{
        options.ReportApiVersions = true; // 返回响应头 api-supported-versions
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.DefaultApiVersion = new ApiVersion(1, 0);

        // 指定版本读取方式（可组合）
        options.ApiVersionReader = ApiVersionReader.Combine(
                new HeaderApiVersionReader("X-API-Version"),
                new QueryStringApiVersionReader("api-version")
        );
});
```

### 支持的版本管理方式

- **URL Path**：`/api/v1/products`
- **Query String**：`/api/products?api-version=1.0`
- **Header**：`X-API-Version: 1.0`
- **Media Type**：`Accept: application/vnd.company.v1+json`


---

## 55. 内容协商（JSON、XML）

ASP.NET Core 支持 **内容协商（Content Negotiation）**，根据客户端请求头自动选择响应格式（如 JSON 或 XML）。  
内容协商由框架的输入/输出格式化器实现。

---

### 默认行为

- 默认仅支持 **JSON**（基于 `System.Text.Json`）。
- 客户端请求头 `Accept: application/json` 时返回 JSON。
- 如果请求的格式不被支持，默认返回 JSON，除非启用 `ReturnHttpNotAcceptable`。

```csharp
services.AddControllers(options =>
{
        options.ReturnHttpNotAcceptable = true; // 不支持的 Accept 返回 406 Not Acceptable
});
```
---

### 启用 XML 支持

可通过添加 XML 格式化器支持 XML 响应：

```csharp
services.AddControllers()
                .AddXmlSerializerFormatters(); // 使用 XmlSerializer
// 或
// .AddXmlDataContractSerializerFormatters(); // 使用 DataContractSerializer
```

---

### 请求头示例

```
Accept: application/xml
Accept: application/json
```

---

### 可选：使用 Newtonsoft.Json

如需使用 Newtonsoft.Json：

```csharp
services.AddControllers()
                .AddNewtonsoftJson();
```

---

### 总结

- ASP.NET Core 通过请求头 `Accept` 实现内容协商。
- 默认仅支持 JSON，可扩展支持 XML。
- 设置 `ReturnHttpNotAcceptable = true` 可强制严格遵守客户端请求格式。
- 可根据需求选择序列化器（System.Text.Json、XmlSerializer、DataContractSerializer、Newtonsoft.Json）。


---

## 56. 参数绑定：Body、Query、Route、Form

| 来源   | 特性           |
| ------ | -------------- |
| Body   | `[FromBody]`   |
| Query  | `[FromQuery]`  |
| Route  | `[FromRoute]`  |
| Form   | `[FromForm]`   |
| Header | `[FromHeader]` |

ASP.NET Core 会自动推断参数来源：

- **简单类型参数**（如 `int`、`string`）：默认来自 **Route** 或 **Query**
- **复杂类型参数**（如类）：默认来自 **Body**（除非另有特性指定）
- **特殊类型**：
        - `IFormFile` 默认来自 **Form**
        - `CancellationToken`、`HttpContext` 等由框架自动注入

**总结：**

- ASP.NET Core 支持多种参数绑定来源
- 可通过特性明确指定来源，提升可读性与可维护性
- 框架提供自动推断机制，但在复杂场景下推荐显式标注特性

---

## 57. 大文件上传/下载（流式、分块）

---

### 上传

- **常见方式**
    - `IFormFile`：适合小文件/中等文件，ASP.NET Core 会自动缓冲
    - `Request.Body`：直接读取请求流，适合大文件上传
    - `PipeReader` / `Stream`：更底层的流式读取，避免内存溢出

- **注意事项**
    - 配置 `FormOptions.MultipartBodyLengthLimit` 控制最大上传大小
    - 对于超大文件（GB 级别），建议 **分块上传 + 断点续传**
    - 禁用默认缓冲：`HttpContext.Request.BodyReader` 或 `EnableBuffering(false)`

---

### 下载

- **常见方式**
    - `FileStreamResult`：返回流式文件（推荐大文件下载）
    - `PhysicalFileResult`：返回磁盘上的文件
    - `File(byte[], contentType, fileName)`：适合小文件（不推荐大文件）

- **最佳实践**
    - 使用流式写出（`Response.Body.WriteAsync`）
    - 支持 Range 请求（`206 Partial Content`），便于断点续传
    - 避免将文件全部加载到内存

---

### ✅ 总结
- **上传**：小文件可用 `IFormFile`，大文件推荐 `Request.Body`/流式处理，并结合分块上传  
- **下载**：推荐 `FileStreamResult`，支持断点续传，避免一次性写入内存  
- **核心原则**：启用流式处理，避免大文件导致内存溢出


---

## 58. 错误处理 / 全局异常处理

- **生产**用 `UseExceptionHandler`，**开发**用 `UseDeveloperExceptionPage`（避免泄露堆栈）。
- 中间件放在管道**前部**（在 `UseHttpsRedirection`/`UseHsts` 之后、`UseRouting` 之前）。
- 统一返回 **RFC 7807** 的 `ProblemDetails`（`Content-Type: application/problem+json`）。
- 启用 `[ApiController]` 时，**模型验证失败**会自动返回 `400 ProblemDetails`。

```csharp
app.UseExceptionHandler(errorApp =>
{
        errorApp.Run(async context =>
        {
                var feature = context.Features.Get<IExceptionHandlerFeature>();
                var ex = feature?.Error;

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/problem+json";

                var problem = new ProblemDetails
                {
                        Status = 500,
                        Title = "An unexpected error occurred",
                        Detail = "Please contact support with the trace ID",
                        Instance = context.Request.Path
                };
                problem.Extensions["traceId"] = context.TraceIdentifier;

                await context.Response.WriteAsJsonAsync(problem);
        });
});
```

---

## 59. 返回合适的 HTTP 状态码

### 常用状态码（精炼）
- **200 OK**：成功返回资源/结果  
- **201 Created**：创建成功，需携带 `Location`（指向新资源）  
- **204 No Content**：成功但无响应体（常见于 `PUT`/`DELETE`）  
- **400 Bad Request**：请求无效（参数/模型错误）  
- **401 Unauthorized**：未通过认证（缺少/无效令牌）  
- **403 Forbidden**：已认证但无权限  
- **404 Not Found**：资源不存在  
- **409 Conflict**：资源冲突（并发/唯一键等）  
- **412 Precondition Failed**：条件请求失败（如 ETag/If-Match 不满足）  
- **422 Unprocessable Entity**：语义校验失败（可选）  
- **500 Internal Server Error**：服务器错误

### 返回示例
```csharp
// 200
return Ok(result);

// 201（包含 Location）
return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);

// 204
return NoContent();

// 400（模型验证错误）
return ValidationProblem(ModelState);

// 401/403（由认证/授权中间件触发）
[Authorize] // 未认证 → 401；无权限 → 403

// 404
return NotFound();

// 409
return Conflict(new { message = "Duplicate name" });

// 412（ETag/并发）
return StatusCode(StatusCodes.Status412PreconditionFailed);
```


---

## 60. ActionResult<T> vs IActionResult

---

### 1. ActionResult&lt;T&gt;

- 返回类型结合了数据模型和 HTTP 状态码
- 适合强类型 API，便于 Swagger/OpenAPI 自动生成文档

```csharp
public ActionResult<Product> Get(int id)
{
        var product = _service.Find(id);
        if (product == null) return NotFound();
        return Ok(product);
}
```

---

### 2. IActionResult

- 更灵活，可返回任意类型结果（如 JSON、文件、错误等）
- 适合复杂场景（可能返回多种类型）

```csharp
public IActionResult Get(int id)
{
        var product = _service.Find(id);
        if (product == null) return NotFound();
        return Ok(product);
}
```

---

### ✅ 推荐原则

- 简单、强类型 API → 用 `ActionResult<T>`（更清晰、利于文档生成）
- 复杂/多类型返回 → 用 `IActionResult`（灵活度高）

---

## 61. 异步 API（async/await）

---

### 核心要点
- ASP.NET Core 对 **异步 I/O** 一等支持；`await` 期间会释放线程，提升并发吞吐量与可扩展性。
- **I/O 场景用异步，CPU 场景用同步**（异步对纯 CPU 计算无益）。
- **避免阻塞**：不要使用 `.Result` / `.Wait()` / `Task.Run()` 包装 I/O。

---

### 控制器示例
```csharp
[HttpGet("{id:int}")]
public async Task<ActionResult<Product>> GetAsync(int id, CancellationToken ct)
{
        // I/O：数据库/HTTP/文件 等均应使用异步 API，并传递取消令牌
        var product = await _repo.GetAsync(id, ct);
        if (product is null) return NotFound();
        return Ok(product);
}
```


---

## 62. Web API 的 CORS

ASP.NET Core 原生支持 **CORS（跨域资源共享）**，用于允许受信任的前端域访问 API。

---

### 配置示例

```csharp
services.AddCors(options =>
{
        options.AddPolicy("AllowFrontend", builder =>
                builder.WithOrigins("https://frontend.com")  // 只允许指定域
                             .AllowAnyHeader()                     // 允许所有请求头
                             .AllowAnyMethod());                   // 允许所有 HTTP 方法
});

app.UseCors("AllowFrontend");
```

---

### 注意事项

- **最小授权原则**：只开放必要的域、方法和头信息。
- **安全警告**：不要同时使用 `.AllowAnyOrigin()` 和 `.AllowCredentials()`，否则会触发运行时错误。
- **环境区分**：开发阶段可宽松（如 `AllowAnyOrigin`），生产环境应严格配置域名白名单。

---

---

## 63. 限流 / 节流

### .NET 8 之前
ASP.NET Core 无内置限流功能，可借助：
- **AspNetCoreRateLimit**（常用库，支持 IP、ClientId 等策略）  
- **YARP** 或 **Azure API Management**（网关层实现）

---

### .NET 8+
新增 **RateLimiterMiddleware**，可在管道中直接配置限流策略：

```csharp
builder.Services.AddRateLimiter(options =>
{
        options.AddFixedWindowLimiter("fixed", opt =>
        {
                opt.PermitLimit = 5;              // 每窗口最多 5 次
                opt.Window = TimeSpan.FromSeconds(10);
        });
});

app.UseRateLimiter();
```

---

## 64. API 文档：Swagger / OpenAPI

ASP.NET Core 常用 **Swashbuckle.AspNetCore** 或 **NSwag** 生成和托管 API 文档，基于 **OpenAPI 规范**。

---

### 配置示例（Swashbuckle）
```csharp
// Program.cs / Startup.cs
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
{
        c.SwaggerDoc("v1", new OpenApiInfo
        {
                Title = "My API",
                Version = "v1",
                Description = "示例 API 文档"
        });
});

app.UseSwagger();
app.UseSwaggerUI(c =>
{
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
});
```

---

## 65. Web API 测试（单元 / 集成）

### 1. 单元测试（Unit Test）

- **目标**：验证控制器逻辑是否正确，依赖项由 Mock 替代  
- **工具**：Moq、NSubstitute、FakeItEasy  
- **特点**：运行快、无真实外部依赖  

```csharp
var mockRepo = new Mock<IProductRepository>();
mockRepo.Setup(r => r.Get(1)).Returns(new Product { Id = 1, Name = "Book" });

var controller = new ProductsController(mockRepo.Object);
var result = controller.Get(1) as OkObjectResult;

Assert.NotNull(result);
Assert.Equal(200, result.StatusCode);
```

---

### 2. 集成测试（Integration Test）

- **目标**：验证 API 的端到端行为，包括路由、过滤器、中间件、依赖注入、数据库等  
- **工具**：`WebApplicationFactory<TEntryPoint>` + 内置 `TestServer` + `HttpClient`  
- **特点**：运行在内存中，接近真实环境  

```csharp
public class ProductsApiTests : IClassFixture<WebApplicationFactory<Program>>
{
        private readonly HttpClient _client;

        public ProductsApiTests(WebApplicationFactory<Program> factory)
        {
                _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetProducts_ReturnsOk()
        {
                var response = await _client.GetAsync("/api/products");
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                Assert.Contains("Book", body);
        }
}
```

---

### 3. 最佳实践

- 单元测试：快、隔离、覆盖业务逻辑  
- 集成测试：验证系统整体行为，确保中间件/路由/过滤器正确配置  
- 测试命名应清晰：`MethodName_Scenario_ExpectedResult`  
- 数据层建议使用 InMemory DB 或 Testcontainers（轻量容器化数据库）  

---

✅ **总结**  
- 单元测试：Mock 依赖，验证控制器逻辑  
- 集成测试：使用 WebApplicationFactory，模拟真实请求  
- 两者结合：保证逻辑正确性 + 系统一致性

---

## 66. API 认证/授权（JWT、OAuth2/OIDC）

- **概念区分**：**Authentication** 认证“你是谁”；**Authorization** 授权“你能做什么”。
- **JWT Bearer（无状态认证）**
```csharp
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options => { ... });
    app.UseAuthentication();
```

### 端点保护

- `[Authorize]`：仅允许已认证用户访问
- `[Authorize(Roles = "Admin")]`：按角色授权，仅管理员可访问
- `[Authorize(Policy = "Scope.Read")]`：按策略或 Scope 授权

---
### OAuth2/OIDC 支持

- 常见身份提供方：IdentityServer、Azure AD (Entra ID)、Auth0 等
- 推荐授权方式：
        - **授权码流 + PKCE**：适用于前端 SPA 或原生 App
        - **Client Credentials**：适用于服务间调用（后台 API）


---

## 67. API 安全：防伪、CORS、HTTPS 等

- **强制 HTTPS + HSTS**
    - 在生产启用：
        ```csharp
        app.UseHttpsRedirection();
        app.UseHsts();
        ```

- **CORS（最小授权原则）**
    - 只允许受信任来源/方法/头；**不要**把 `AllowAnyOrigin` 与 `AllowCredentials` 一起用。
        ```csharp
        services.AddCors(o => o.AddPolicy("api", b =>
                b.WithOrigins("https://example.com")
                 .WithMethods("GET","POST","PUT","DELETE")
                 .WithHeaders("Content-Type","Authorization")
                 .AllowCredentials()));
        app.UseCors("api");
        ```

- **认证（Authentication）**
    - API 优先用 **JWT Bearer**；统一加 `[Authorize]`。
```csharp
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddJwtBearer();
app.UseAuthentication();
app.UseAuthorization();
```

- **授权（Authorization）**
    - 使用 **角色/策略/作用域** 控制访问：
```csharp
[Authorize(Policy = "Products.Read")]
```

- **防伪（CSRF）**
    - **Token-based**（如 Bearer/JWT）通常**不需要**防伪。
    - 若使用 **Cookie 认证**（浏览器自动附带），需启用：
```csharp
[ValidateAntiForgeryToken] // 或前后端同步自定义 CSRF 令牌
```
        并配置安全 Cookie（`SameSite`、`Secure`、`HttpOnly`）。

---

## 68. DTO 与映射（如 AutoMapper）

- **DTO（Data Transfer Object）** 用于将内部领域模型与外部 API 合约解耦，避免直接暴露数据库实体。
- 主要作用：隐藏敏感字段、减少传输数据量、保持 API 版本兼容性。

---

### AutoMapper 示例

```csharp
// 配置映射关系
CreateMap<Product, ProductDto>();

// 执行映射
var dto = _mapper.Map<ProductDto>(product);
```

---

### 总结

- 使用 DTO 可提升安全性和可维护性。
- AutoMapper 能简化对象转换，减少手写映射代码。
- 推荐在 API 层使用 DTO，并通过映射工具实现模型转换。

---

## 69. 版本管理陷阱与兼容性

在设计和维护 API 时，版本管理是确保 **向后兼容性** 和 **平滑演进** 的关键。常见陷阱及应对原则如下：

---

### 1. 避免破坏性变更
- 不要直接修改或移除已有端点的请求/响应结构  
- 避免随意更改字段名称、数据类型或语义  
- 如需调整，应通过 **新增版本** 来实现，而不是直接覆盖旧版本  

---

### 2. 合约变更需新版本
- 接口契约（Contract）是客户端与服务端的约定  
- 一旦修改会影响客户端 → 应发布新版本（例如 v2），保持旧版本仍然可用  
- 新版本可通过 **URL、Header、Media Type** 等方式区分  

---

### 3. 保持旧版本可用
- 不要立即移除旧版本，应提供一段过渡期  
- 在文档中标注 **弃用（Deprecated）**，并给出下线时间表  
- 在响应头中返回 `api-deprecated-versions` 以提示客户端升级  

---

### 4. 确保文档与客户端更新
- API 文档必须和版本演进保持一致（Swagger / OpenAPI）  
- 客户端 SDK、调用代码需要及时更新，避免使用已废弃端点  
- 建议在新旧版本并行时提供 **迁移指南**，降低升级风险  

---

### ✅ 总结
- **不破坏旧版本**：保持兼容性  
- **新增而非修改**：合约变化用新版本  
- **提供过渡期**：旧版本标记弃用而非立刻删除  
- **同步更新文档和客户端**：确保开发者清晰了解变化


---

## 70. 并发处理 / 乐观并发

---

### 乐观并发控制

- 假设并发冲突较少，不加锁
- 通过 **RowVersion**（数据库字段）或 **ETag**（HTTP 头）检测数据是否被其他请求修改
- 如果版本不一致，则抛出并发异常

---

### EF Core 示例（RowVersion）

```csharp
public class Product
{
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] RowVersion { get; set; } // 必须为 byte[]
}

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
        modelBuilder.Entity<Product>()
                .Property(p => p.RowVersion)
                .IsRowVersion();
}
```

- EF Core 更新时会在 WHERE 子句中包含 RowVersion
- 如果没有更新到任何记录，会抛出 `DbUpdateConcurrencyException`
- 应捕获异常并返回合适的 HTTP 状态码

---

### Web API 示例（ETag）

- 响应时返回 ETag：

```
ETag: "v1-xyz123"
```

- 客户端更新时发送：

```
If-Match: "v1-xyz123"
```

- 如果 ETag 不匹配，返回 `412 Precondition Failed`

---

### 常见状态码

- `409 Conflict`：资源修改冲突
- `412 Precondition Failed`：并发条件（如 If-Match）未满足，常用于 ETag 并发控制

