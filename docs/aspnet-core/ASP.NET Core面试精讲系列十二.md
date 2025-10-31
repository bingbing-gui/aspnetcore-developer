## 167. `IActionResult` 与 `ActionResult<T>` 的区别

- `IActionResult`：表示动作方法的非泛型结果；可返回任意 HTTP 响应（如 `Ok`、`NotFound`、`Redirect` 等）。
- `ActionResult<T>`：将结果与强类型值 `T` 结合；既可返回类型化数据，也可返回 HTTP 响应；提高可读性并有利于生成更好的 OpenAPI 文档。

## 168. 在新 .NET 版本与 Minimal Hosting 模型下，`Startup` 的角色是什么？

- .NET 5 及更早：`Startup` 负责配置服务与中间件。
- .NET 6+ Minimal Hosting：在 `Program.cs` 中以顶层语句合并服务注册与中间件配置，流程更精简。
- `Startup` 仍可用于组织结构，但不再是必需。

## 169. 将 ASP.NET Core 应用从 .NET 5 迁移到 .NET 8（或更新）的方法

- 在项目文件中将目标框架更新为 `net8.0`。
- 升级 NuGet 包与依赖项。
- 需要时可采用 Minimal APIs。
- 如需可用 Minimal Hosting 替代 `Startup`。
- 充分测试以应对 API 变更或弃用项。
- 检查并更新中间件与路由模式。

## 170. 什么是 Minimal API？与 Controllers 有何不同？

- Minimal API：使用顶层语句定义的轻量端点，无需控制器类。
- 适合微服务或简单 API；仪式性更少、文件更少。
- Controllers：功能更丰富（过滤器、模型绑定、`ActionResult` 等）。

## 171. 如何处理版本冲突（多个依赖需要同一库的不同版本）？

- 在 .NET Framework 中使用 绑定重定向（binding redirects）。
- 使用程序集版本统一与强命名程序集。
- 在 .NET Core 中通过 NuGet 包管理 与 中央包版本 解决。
- 评估升级或整合依赖。

## 172. 什么是 Endpoint Routing？

- 引入自 ASP.NET Core 3.0。
- 集中式路由系统，将路由匹配与中间件解耦。
- 将请求路由到由 Controllers、Razor Pages、Minimal APIs 定义的端点。
- 支持基于路由的中间件筛选。

## 173. 传统路由（Conventional）与特性路由（Attribute）的区别

- 传统路由：集中定义（通常在 `Startup`/`Program`），全局应用路由模板。
- 特性路由：在控制器/动作上用特性（``[Route]``、``[HttpGet]`` 等）直接声明路由。
- 特性路由更灵活、显式。

## 174. 如何启用与自定义 OpenAPI / Swagger UI

- 添加 `Swashbuckle.AspNetCore` NuGet 包。
- 注册服务：`builder.Services.AddSwaggerGen()`。
- 启用中间件：`app.UseSwagger()`、`app.UseSwaggerUI()`。
- 可自定义：API 信息、文档过滤器、UI 主题等。

## 175. 什么是内容协商（Content Negotiation），ASP.NET Core 如何选择媒体类型？

- 根据 `Accept` 请求头自动选择响应格式（JSON、XML）。
- 通过 MVC 选项中的 格式化器（formatters） 配置。
- 无匹配时回退到默认格式化器。

## 176. WebAPI 中的 `ProblemDetails` 是什么？

- RFC 7807 定义的标准化错误响应格式。
- 包含 `status`、`title`、`detail`、`instance` 等属性。
- ASP.NET Core 默认在错误响应中使用该结构。

## 177. 如何返回自定义错误响应 / 使用自定义中间件处理错误

- 实现自定义异常处理中间件。
- 捕获异常、设置 HTTP 状态码并返回自定义 JSON 负载。
- 使用 `UseExceptionHandler()` 或全局过滤器。
- 自定义 `ProblemDetails` 或自定义错误模型。

## 178. 在 API 控制器/动作中使用 `CancellationToken`

- 在 Action 方法中接受 `CancellationToken` 参数。
- 将令牌传递给异步调用以支持请求取消。
- 提升响应性与资源管理效率。

## 179. 默认文件上传大小限制及其配置

- 默认最大请求体大小为 30 MB。
- 可通过 ``[RequestSizeLimit]`` 或 `KestrelServerOptions.Limits.MaxRequestBodySize` 配置。
- 使用 IIS 时，需调整 `maxAllowedContentLength`。

## 180. 如何启用 Gzip 或 Brotli 压缩

- 添加 ResponseCompression 中间件。
- 在 `Program.cs` 中注册：`builder.Services.AddResponseCompression()`。
- 配置支持的 MIME 类型 与压缩级别，并在管道中调用 `app.UseResponseCompression()`。

## 181. JSON 选项：`System.Text.Json` 与 `Newtonsoft.Json`

- `System.Text.Json`：.NET Core 3+ 的默认实现，内置、性能优、但功能相对精简。
- `Newtonsoft.Json`：更成熟，支持高级场景（如多态反序列化等）。
- 可通过：`builder.Services.AddControllers().AddNewtonsoftJson();` 切换为 Newtonsoft 的序列化实现。



