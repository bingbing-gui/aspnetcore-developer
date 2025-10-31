## 🏗 152. 如何构建大型 ASP.NET Core 解决方案（分层、Clean Architecture、Onion Architecture）

- 采用 分层架构（Layered Architecture）：
    - Presentation 层：API 或 UI
    - Application 层：业务逻辑与服务
    - Domain 层：领域实体与业务规则
    - Infrastructure 层：数据访问与外部服务集成
- Clean Architecture（整洁架构）
    - 强调 关注点分离（Separation of Concerns）
    - 所有依赖关系应当 指向领域层（Domain）
- Onion Architecture（洋葱架构）
    - 以领域核心为中心，外层依赖内层
    - 所有依赖都指向核心的 Domain
- 使用多个独立项目（Projects）来隔离职责，提高可维护性与可测试性。

## 🧩 153. 依赖反转与 SOLID 原则在 ASP.NET Core 中的应用

- 应用 依赖反转原则（Dependency Inversion Principle）：
    - 面向抽象（接口）编程，而非实现。
- 遵循 SOLID 原则：
    - S - 单一职责原则（Single Responsibility）：每个类只做一件事。
    - O - 开放封闭原则（Open/Closed）：类对扩展开放，对修改封闭。
    - L - 里氏替换原则（Liskov Substitution）：子类应能替代父类使用。
    - I - 接口隔离原则（Interface Segregation）：使用多个小而专一的接口。
    - D - 依赖反转原则（Dependency Inversion）：依赖抽象，不依赖具体实现。

## 🧹 154. 资源清理与释放（如 DbContext）

- 通过 依赖注入（DI） 管理 `DbContext` 的生命周期，通常为 Scoped。
- 避免手动释放，由 DI 容器自动处理。
- 对手动创建的 `IDisposable` 对象，应及时释放。
- 对于短生命周期资源，使用 `using` 语句（或 C# 8 的 `using var`）。

## 🧪 155. 可测试性设计（单元测试与集成测试）

- 使用 依赖注入（DI） 使代码松耦合。
- 使用 Mock 框架（如 `Moq`） 模拟外部依赖进行单元测试。
- 在集成测试中使用 `EF Core InMemory` 数据库 替代真实数据库。
- 层间隔离，确保测试可聚焦且独立。

## 📊 156. 日志（Logging）、追踪（Tracing）与异常报告（Exception Reporting）

- Logging：记录应用事件与信息。
- Tracing：追踪请求或操作在系统中的执行路径（跨服务/组件）。
- Exception Reporting：捕获异常并通知相关人员或系统。
- 结合 结构化日志（Structured Logging） 与 分布式追踪（Distributed Tracing） 进行诊断。

## 💾 157. 数据访问：EF Core 最佳实践（No Tracking、Lazy Loading、Migrations）

- 对只读数据使用 No Tracking 查询 提升性能。
- 谨慎使用 Lazy Loading，建议使用显式加载（Explicit Loading）。
- 通过版本控制系统管理 数据库迁移（Migrations）。
- 使用 异步查询（`async/await`） 避免阻塞线程。

## 🧱 158. 在生产环境安全地执行数据库迁移

- 在 CI/CD 流程中自动执行迁移前，先进行数据库备份。
- 使用 事务性迁移（Transactional Migration）（若数据库支持）。
- 在 维护窗口（Maintenance Window） 执行迁移以避免影响用户。
- 在 预发布环境（Staging） 完全验证迁移后再应用至生产环境。

## 🔢 159. API 版本控制与向后兼容

- 维护多个 API 版本，制定明确的 弃用策略（Deprecation Policy）。
- 避免破坏性修改（Breaking Changes），优先采用 增量式变更。
- 清晰地向 API 使用者传达版本生命周期信息。

## 🚦 160. 限流与节流策略（Rate Limiting / Throttling）

- 通过限制请求频率（每秒/每分钟）保护 API 免受滥用。
- 可使用第三方库 `AspNetCoreRateLimit` 或 API Gateway（如 Azure APIM）。
- 支持基于 IP、用户 或 客户端 Token 的限流策略。

## ⚙️ 161. 异常与错误处理模式（Error / Exception Handling Patterns）

- 使用 全局异常处理中间件（Exception Handling Middleware）。
- 返回有意义的 HTTP 状态码与错误信息。
- 避免泄露敏感信息。
- 对临时性错误（如网络超时）实现 重试策略（Retry Policy）。

## 📴 162. 优雅关闭（Graceful Shutdown）

- 处理系统关闭信号（如 `SIGTERM`），确保正在执行的请求完成。
- 正确释放资源与连接。
- 使用 `IHostApplicationLifetime` 事件挂钩应用启动与停止过程。

## 🔐 163. 安全地管理机密信息（Secrets）

- 使用安全存储服务：
    - Azure Key Vault
    - AWS Secrets Manager
    - HashiCorp Vault
- 不要在代码或配置文件中存储密钥。
- 开发阶段可使用 `User Secrets` 或 环境变量（Environment Variables）。

## ⚙️ 164. 在 CI/CD 中使用配置与环境变量

- 在流水线中通过 环境变量 或 密钥存储 注入配置。
- 使用多环境配置文件：
    - `appsettings.Development.json`
    - `appsettings.Production.json` 等。
- 通过 CI/CD 工具的 秘密管理（Secret Management） 保护敏感数据。

## 🌍 165. 国际化与本地化（Internationalization / Localization）

- 使用 `.resx` 资源文件存放多语言字符串。
- 配置 `RequestLocalizationMiddleware` 以支持多语言请求。
- 支持多种文化（Culture）与回退文化（Fallback Culture）。
- 本地化日期、时间、货币、数据格式等。

## 🧰 166. 输入验证与安全漏洞防护（Validation & Security）

- 使用 参数化查询 或 ORM 防止 SQL 注入。
- 对用户输入进行清理和编码，防止 XSS 攻击。
- 在服务端严格验证输入数据。
- 使用内置验证特性（如 `[Required]`, `[StringLength]`）或自定义验证器。
