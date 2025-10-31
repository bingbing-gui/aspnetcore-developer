## 134. 托管：Kestrel、IIS、反向代理场景
- `Kestrel` 是 ASP.NET Core 的默认跨平台 Web 服务器，轻量且快速。
- `IIS` 在 Windows 上作为反向代理，将请求转发给 Kestrel。
- 反向代理可提升安全性，管理 SSL，进行负载均衡。
- 在 Linux 上，通常使用 `Nginx` 或 `Apache` 作为 Kestrel 的反向代理。

## 135. InProcess vs OutOfProcess 托管模式

- InProcess 模式：应用在 IIS 的工作进程（`w3wp.exe`）中运行，性能更好。
- OutOfProcess 模式：应用在独立进程中运行，IIS 作为代理转发请求。
- 自 ASP.NET Core 3.0+ 起，默认采用 InProcess 模式进行 IIS 托管。

## 136. 健康检查（Health Checks）

- 提供用于报告应用健康状态的 HTTP 端点。
- 使用包：`Microsoft.AspNetCore.Diagnostics.HealthChecks`。
- 可配置数据库、外部服务、依赖项等检查项。
- 常用于 Kubernetes、负载均衡器的健康探测。

## 137. 日志记录（Logging）

- ASP.NET Core 内置日志系统，提供多种日志提供程序（Console、Debug、EventSource）。
- 第三方库如 `Serilog`、`NLog` 提供结构化日志和丰富的输出管道（Sink）。
- 可通过 `appsettings.json` 或代码进行配置。

## 138. 性能调优（Performance Tuning）：缓存与压缩
- 内存缓存（In-Memory Caching）：将数据存储在服务器内存中，加快访问速度。
- 分布式缓存（Distributed Caching）：使用 `Redis`、`SQL` 等外部存储，支持多服务器。
- 响应压缩（Response Compression）：使用 `Gzip` 或 `Brotli` 中间件减小响应体积。

## 139. 多线程与并发（Threading / Concurrency / Deadlocks）
- 避免在异步代码中使用阻塞调用（例如 `.Result`、`.Wait()`），以防死锁。
- 正确使用 `async/await` 模式。
- 使用锁（`lock`）或并发集合（`ConcurrentDictionary` 等）保护共享数据。
- 调整线程池参数可防止线程饥饿。

## 140. 内存泄漏与依赖释放（Memory Leaks & Disposal）
- 未正确释放 `IDisposable` 对象会导致内存泄漏。
- 正确使用依赖注入生命周期（Scoped / Transient / Singleton）。
- 注意静态引用、事件未解绑等导致对象无法回收。
- 可使用 `dotMemory`、`PerfView` 等工具分析内存。

## 141. 可扩展性（Scalability）
- 设计为无状态（Stateless）应用，使任意实例可独立处理请求。
- 使用分布式缓存或外部会话存储来共享状态。
- 通过负载均衡器分发流量到多个实例。

## 142. 部署（Deployment）
- 使用 Docker 容器化应用，实现一致性部署。
- 使用 Azure App Service 或 Azure Kubernetes Service（AKS）进行云端托管。
- 配置 CI/CD 管道（GitHub Actions、Azure DevOps）实现自动构建与部署。

## 143. 监控、指标与追踪（Monitoring, Metrics, Tracing）

- 监控应用健康状态、请求指标与错误日志。
- 使用 OpenTelemetry 实现分布式追踪（Distributed Tracing）。
- 可与 Application Insights、Prometheus、Grafana 等集成。

## 144. 安全最佳实践（Security Best Practices）

- 强制启用 HTTPS。
- 使用防伪令牌（Anti-Forgery Token）防止 CSRF 攻击。
- 对输入进行清理以防止 XSS。
- 遵循 OWASP 安全开发指南。

## 145. 中间件（Middleware）

- 使用 Response Compression Middleware 压缩响应。
- 使用 Response Caching Middleware 缓存 GET 请求响应。
- 中间件可按管道顺序组合，形成分层处理逻辑。

## 146. 响应缓存与输出缓存（Response Caching / Output Caching）

- Response Caching 根据响应头缓存 HTTP 响应。
- Output Caching（ASP.NET Core 7 新增）可缓存整个端点输出内容。

## 147. 分布式缓存与会话状态（Distributed Caching / Session State）

- 在多服务器环境下使用 `Redis` 或 `SQL Server` 作为分布式缓存/会话存储。
- 避免依赖“黏性会话”（Sticky Session），提升可伸缩性。

## 148. SignalR（实时通信）

- 实现实时双向通信（客户端与服务器）。
- 支持 WebSockets、Server-Sent Events、长轮询（Long Polling）。

## 149. 后台任务与托管服务（Background Tasks / Hosted Services）

- 通过 `IHostedService` 或 `BackgroundService` 实现后台任务。
- 常用于定时任务、队列处理、消息消费等场景。

## 150. 文件与流处理（Files & Streaming）
- 使用 `FileStreamResult` 高效地流式传输大文件。
- 使用异步 IO（async）避免阻塞主线程。

## 151. .NET 版本与新特性（Version Updates）

- 关注最新的改进，例如 Minimal APIs、Source Generators、性能优化等。
- 保持 SDK 与依赖的最新版本。
