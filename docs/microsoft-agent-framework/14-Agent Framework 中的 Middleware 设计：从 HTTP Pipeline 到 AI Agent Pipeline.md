## 序言

在构建企业级 AI 应用时，直接让大语言模型（LLM）与用户或系统交互往往存在风险。需要处理隐私泄露（PII）、内容合规性（Guardrails）、函数调用的审计以及“人在回路”（Human-in-the-Loop）的审批流程。

Microsoft Agent Framework 引入了强大的中间件（Middleware）机制，允许开发者像洋葱皮一样层层包裹代理（Agent），在消息发送前后、函数调用前后进行拦截和处理。


## 从 ASP.NET Core Middleware 说起

如果你有 ASP.NET Core 的开发经验，那么你其实已经掌握了理解 Agent Middleware 所需的 **80% 关键知识**。

因为无论是 Web 应用还是 AI Agent，本质上都面临同一个问题：

> **如何在不侵入核心业务逻辑的前提下，引入日志、审计、监控等横切能力？**

---

## ASP.NET Core 中的 Middleware Pipeline

在一个典型的 Web 应用中，请求会依次经过一条中间件管道（Pipeline）：

Request → Logging → Authentication → Authorization → MVC → Response


这种设计的核心价值在于：

- 将日志、鉴权、异常处理、限流等横切关注点从业务代码中剥离
- 每个中间件只关注自己的职责
- 是否继续执行后续逻辑，由当前中间件通过 `next()` 决定

从设计角度看，这本质上是一种 **AOP（面向切面编程）模型**。

---


## 一个更“真实”的 ASP.NET Core Middleware 示例

为了让这个类比更贴近后续的 Agent 场景，我们来看一个带计数与耗时统计的中间件示例。

```csharp
public sealed class RequestMetricsMiddleware
{
    private static long _requestCount = 0;

    private readonly RequestDelegate _next;
    private readonly ILogger<RequestMetricsMiddleware> _logger;

    public RequestMetricsMiddleware(
        RequestDelegate next,
        ILogger<RequestMetricsMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var requestId = Interlocked.Increment(ref _requestCount);
        var start = Stopwatch.GetTimestamp();

        _logger.LogInformation(
            "Request #{RequestId} started: {Method} {Path}",
            requestId,
            context.Request.Method,
            context.Request.Path);

        await _next(context);

        var elapsedMs =
            (Stopwatch.GetTimestamp() - start) * 1000d / Stopwatch.Frequency;

        _logger.LogInformation(
            "Request #{RequestId} finished in {Elapsed} ms",
            requestId,
            elapsedMs);
    }
}
```
### 这个中间件做了什么？

在请求进入时：
- 记录请求编号
- 开始计时

在请求完成后：
- 统计请求耗时
- 输出统一的审计日志

整个过程中：
- 业务 Controller 完全不需要感知它的存在
- 中间件以“切面”的形式横跨整个请求生命周期

### Middleware 注册（形成 HTTP Pipeline）

```csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseMiddleware<RequestMetricsMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
```
需要注意的是：
- Middleware 按注册顺序执行 Before
- 按相反顺序执行 After
- 共同构成一条完整的请求处理管道

## 从 Web Middleware 到 Agent Middleware

如果你将上面的示例换一个视角来看：

| Web 世界 | AI Agent 世界 |
|---|---|
| HttpContext | Prompt / Agent Context |
| HTTP Request | 用户输入 |
| HTTP Response | Agent / LLM 输出 |
| RequestMetricsMiddleware | Agent / LLM 中间件 |

那么你会发现：ASP.NET Core Middleware 的核心思想，可以被几乎原封不动地迁移到 AI Agent 的执行模型中。

区别只在于：
- 请求不再是 HTTP Request
- 上下文不再是 HttpContext

管道中流转的不再是 HTTP 数据，而是：
- Prompt
- 函数调用决策
- 推理结果

Microsoft Agent Framework 的 Middleware 机制，正是基于这一思想，将成熟的 Pipeline / AOP 模型引入到了 AI Agent 的执行生命周期中。

## Agent Framework 中的 Middleware

在进入具体代码前，需要明确：Agent Framework 不存在一条统一的 Middleware Pipeline。一次完整的 Agent Run 中，框架刻意划分并治理了三个本质不同的执行边界，每个边界对应一类不可替代的 Middleware。可以把一次 Agent Run 理解为三条并行存在、但在时间上相互嵌套的 Pipeline。

### 一次 Agent Run 的三条治理边界
- 模型推理边界（LLM Call）：Agent 与大语言模型之间的请求与响应
- 行为决策与执行边界（Function / Tool Call）：模型已决定“调用某个函数”，但系统尚未或刚刚执行
- Agent 运行生命周期边界（Agent Run）：一次完整 Agent 运行的整体输入与最终输出

对比来看：ASP.NET Core 只有一条 HTTP Pipeline，而 Agent Framework 同时治理模型调用、行为执行、业务输入输出三个层面。

---

## 1) Agent Run Middleware（Agent 生命周期级）

作用范围
- 作用于一次完整的 Agent 运行过程
- 可拦截并修改输入消息 IEnumerable<ChatMessage> 与最终输出 AgentRunResponse
- 最外层、最贴近业务语义

典型用途
- 输入/输出双向的 PII 脱敏
- 输入/输出双向的内容合规（Guardrails）
- 统一审计、结果转换、业务级兜底

与 ASP.NET Core 的核心差异
- 处理对象：HTTP Request/Response → Agent 输入消息/最终输出
- 上下文：HttpContext → messages/thread/options
- next：RequestDelegate(context) → innerAgent.RunAsync(...)
- 调用次数：每请求一次 → 每次 Agent Run 一次（Run 内部可能多次 LLM 推理与函数调用）

注意
- Agent Run Middleware 不直接治理函数参数或返回值，治理的是“整体输入”和“整体输出”。

定义
```csharp
async Task<AgentRunResponse> CustomAgentRunMiddleware(
    IEnumerable<ChatMessage> messages,
    AgentThread? thread,
    AgentRunOptions? options,
    AIAgent innerAgent,
    CancellationToken ct)
{
    // Before：输入治理 / 审计
    var response = await innerAgent.RunAsync(messages, thread, options, ct);

    // After：输出治理 / 结果转换
    return response;
}
```

注册
```csharp
var agent = originalAgent
    .AsBuilder()
    .Use(runFunc: CustomAgentRunMiddleware, runStreamingFunc: null)
    .Build();
```

执行模型
```
Messages
  → AgentRunMiddleware (Before)
      → innerAgent.RunAsync(...)
  ← AgentRunMiddleware (After)
AgentRunResponse
```

### 1.1) Agent Run Streaming Middleware（流式生命周期级）

Streaming 场景下，Agent 持续输出 AgentRunResponseUpdate。

作用范围
- 拦截 Streaming 过程中的每一条 Update

典型用途
- Token/Update 级统计
- 流式过滤、采样、缓存
- SSE/gRPC 类流式治理

定义
```csharp
async IAsyncEnumerable<AgentRunResponseUpdate> CustomAgentRunStreamingMiddleware(
    IEnumerable<ChatMessage> messages,
    AgentThread? thread,
    AgentRunOptions? options,
    AIAgent innerAgent,
    CancellationToken ct)
{
    // Before：流式开始前

    await foreach (var update in innerAgent.RunStreamingAsync(messages, thread, options, ct))
    {
        // During：逐条 update 拦截
        yield return update;
    }

    // After：流式结束后
}
```

注册建议

```csharp
var agent = originalAgent
    .AsBuilder()
    .Use(
        runFunc: CustomAgentRunMiddleware,
        runStreamingFunc: CustomAgentRunStreamingMiddleware)
    .Build();
```

建议非流式与流式 Middleware 始终成对注册，否则 Streaming 可能退化为非流式执行。
---

## 2) Function Calling Middleware（行为决策/执行级）

作用范围
- 作用于一次函数调用（一次 Run 内可能 0..N 次）

触发时机
- LLM 已决定调用某个函数
- 系统即将执行真实函数，或已拿到函数结果

可治理内容
- 函数名
- 结构化参数（FunctionInvocationContext）
- 函数返回值

典型用途
- 参数校验、权限控制
- 函数调用审计
- Mock/覆写结果
- Human-in-the-loop（人工审批后再执行）

与 ASP.NET Core 的差异
- 治理对象：HTTP 数据流 → 模型行为到系统执行
- 调用次数：每请求一次 → 按函数调用次数
- next：进入下游中间件 → next(context) 直至真实函数
- 关注点：数据传输 → 行为是否被允许

一句话总结
- Agent Run Middleware 管“说什么”
- Function Calling Middleware 管“做不做”

定义
```csharp
async ValueTask<object?> CustomFunctionCallingMiddleware(
    AIAgent agent,
    FunctionInvocationContext context,
    Func<FunctionInvocationContext, CancellationToken, ValueTask<object?>> next,
    CancellationToken ct)
{
    // Before：参数校验 / 审计
    var result = await next(context, ct);

    // After：结果处理 / 覆写
    return result;
}
```

注册
```csharp
var agent = originalAgent
    .AsBuilder()
    .Use(CustomFunctionCallingMiddleware)
    .Build();
```

执行模型
```
MW2.Before
  MW1.Before
    Function Execution
  MW1.After
MW2.After
```

---

## 3) IChatClient Middleware（模型推理级）

作用范围
- 直接包裹对 LLM 的推理调用
- 拦截发送给模型的 Prompt 与模型返回的原始响应

触发时机
- Agent 每一次调用模型推理时（一次 Agent Run 内可能多次）

典型用途
- Prompt 审计与日志
- Token/Latency 统计
- 模型调用配额与限流
- 调试真实发送给模型的内容

与 ASP.NET Core 的类比
- 角色：HttpClient DelegatingHandler ↔ IChatClient Middleware
- 请求对象：HttpRequestMessage ↔ ChatMessage 序列
- 响应对象：HttpResponseMessage ↔ ChatResponse

定义
```csharp
async Task<ChatResponse> CustomChatClientMiddleware(
    IEnumerable<ChatMessage> messages,
    ChatOptions? options,
    IChatClient innerClient,
    CancellationToken ct)
{
    // Before：Prompt 审查 / 统计
    var response = await innerClient.GetResponseAsync(messages, options, ct);

    // After：模型输出审计 / 统计
    return response;
}
```

注册（示例）
```csharp
var agent = new AzureOpenAIClient(new Uri(endpoint), new AzureCliCredential())
    .GetChatClient(deploymentName)
    .AsIChatClient()
    .AsBuilder()
        .Use(getResponseFunc: CustomChatClientMiddleware, getStreamingResponseFunc: null)
    .BuildAIAgent(instructions: "你是一个乐于助人的助手。");
```


---

## 示例

下面的示例将完成三件事情：

1. 在 LLM 推理阶段记录请求与响应
2. 在函数调用阶段审计并覆盖函数结果
3. 在 Agent 执行阶段进行隐私脱敏与内容合规过滤

示例中会同时出现三类 Middleware，这是有意为之，
目的是让你看到它们在一次 Agent Run 中是如何协同工作的。

---

### 定义可被 LLM 调用的函数（Tools）

首先，定义两个可被 LLM 调用的函数，作为 Agent 的工具（Tools）：

```csharp
[Description("获取指定位置的天气。")]
static string GetWeather(
    [Description("用于查询天气的地点。")] string location)
    => $"{location} 的天气是多云，最高气温为 15°C。";

[Description("当前的日期时间偏移量。")]
static string GetDateTime()
    => DateTimeOffset.Now.ToString();
```

说明：
- 使用 [Description] 为函数及其参数提供语义信息，帮助模型理解用途。
- 这些元数据也便于在 Function Calling Middleware 中进行拦截与审计。

---

### IChatClient Middleware（LLM 推理级）

IChatClient Middleware 是最底层、最靠近模型的一层，用于拦截 Agent 与 LLM 间的请求与响应。

```csharp
async Task<ChatResponse> ChatClientMiddleware(IEnumerable<ChatMessage> message, ChatOptions? options, IChatClient innerChatClient, CancellationToken cancellationToken)
{
    Console.WriteLine("Chat Client 中间件 - 运行前聊天");
    var response = await innerChatClient.GetResponseAsync(message, options, cancellationToken);
    Console.WriteLine("Chat Client 中间件 - 运行后聊天");
    return response;
}
```

典型用途：
- Prompt 日志与审计
- Token 统计与调用监控
- 调试模型行为

它的角色类似于 ASP.NET Core 中的 HttpClient DelegatingHandler。

---

### 构建 originalAgent（基础 Agent）

将 IChatClient Middleware 注入到底层 ChatClient，并构建 Agent（包含工具函数）：

```csharp
// 创建Azure OpenAI客户端并获取ChatCliet对象
var azureOpenAIClient = new AzureOpenAIClient(new Uri(endpoint), new AzureCliCredential())
    .GetChatClient(deploymentName);

// 构建 Agent 时注入底层中间件
var originalAgent = azureOpenAIClient.AsIChatClient()
    .AsBuilder()
    .Use(getResponseFunc: ChatClientMiddleware, getStreamingResponseFunc: null)
    .BuildAIAgent(instructions: "你是一个帮助人们查找信息的 AI 助手。", tools: [AIFunctionFactory.Create(GetDateTime, name: nameof(GetDateTime))]);

// 添加中间件在agent级别，并在其上构建一个新的代理
var middlewareEnabledAgent = originalAgent
    .AsBuilder()
    .Use(FunctionCallMiddleware)
    .Use(FunctionCallOverrideWeather)
    .Use(PIIMiddleware, null)
    .Use(GuardrailMiddleware, null)
    .Build();

var thread = middlewareEnabledAgent.GetNewThread();
```

此时：
- Agent 已具备 LLM 推理级中间件能力
- 尚未引入 Agent Run 与 Function Calling 两类中间件

---

### Function Calling Middleware（函数调用级）

当 Agent 触发函数调用时，该类中间件会介入执行流程。

1) FunctionCallMiddleware（审计型）：记录函数执行前后信息

```csharp
async ValueTask<object?> FunctionCallMiddleware(AIAgent agent, FunctionInvocationContext context, Func<FunctionInvocationContext, CancellationToken, ValueTask<object?>> next, CancellationToken cancellationToken)
{
    Console.WriteLine($"函数: {context!.Function.Name} - 中间件 1 执行前");
    var result = await next(context, cancellationToken);
    Console.WriteLine($"函数: {context!.Function.Name} - 中间件 1 执行后");
    return result;
}
```

2) FunctionCallOverrideWeather（结果覆盖）：对指定函数结果进行覆写

```csharp
async ValueTask<object?> FunctionCallOverrideWeather(AIAgent agent, FunctionInvocationContext context, Func<FunctionInvocationContext, CancellationToken, ValueTask<object?>> next, CancellationToken cancellationToken)
{
    Console.WriteLine($"函数: {context!.Function.Name} - 中间件 2 执行前");

    var result = await next(context, cancellationToken);

    if (context.Function.Name == nameof(GetWeather))
    {
        // Override the result of the GetWeather function
        result = "天气晴朗，最高气温25°C。";
    }
    Console.WriteLine($"函数: {context!.Function.Name} - 中间件 2 执行后");
    return result;
}
```

注册顺序与执行顺序：

```csharp
var agentWithFunctionCalling = originalAgent
    .AsBuilder()
        .Use(FunctionCallMiddleware)          // 先审计
        .Use(FunctionCallOverrideWeather)     // 后覆写
    .Build();

/*
执行链：
FunctionCallMiddleware (Before)
  → FunctionCallOverrideWeather (Before)
    → 实际函数执行或被覆盖
  ← FunctionCallOverrideWeather (After)
← FunctionCallMiddleware (After)
*/
```

### Agent Run Middleware（Agent 执行级）

在 Agent 执行层面，引入两种中间件，形成典型的企业合规治理组合。

1) PIIMiddleware（个人信息脱敏）
- 执行前：过滤输入消息
- 执行后：过滤输出消息

示例规则：
```
123-456-7890  → [已屏蔽: PII]
john@xxx.com  → [已屏蔽: PII]
John Doe      → [已屏蔽: PII]
```

示例实现（省略具体正则与消息结构处理）：
```csharp
async Task<AgentRunResponse> PIIMiddleware(IEnumerable<ChatMessage> messages, AgentThread? thread, AgentRunOptions? options, AIAgent innerAgent, CancellationToken cancellationToken)
{
    var filteredMessages = FilterMessages(messages);
    Console.WriteLine("Pii 中间件 - 运行前过滤消息");

    var response = await innerAgent.RunAsync(filteredMessages, thread, options, cancellationToken).ConfigureAwait(false);

    response.Messages = FilterMessages(response.Messages);

    Console.WriteLine("Pii 中间件 - 运行后过滤消息");

    return response;

    static IList<ChatMessage> FilterMessages(IEnumerable<ChatMessage> messages)
    {
        return messages.Select(m => new ChatMessage(m.Role, FilterPii(m.Text))).ToList();
    }

    static string FilterPii(string content)
    {
        Regex[] piiPatterns =
        [
            new(@"\b\d{3}-\d{3}-\d{4}\b", RegexOptions.Compiled), // 电话号码( 123-456-7890)
            new(@"\b[\w\.-]+@[\w\.-]+\.\w+\b", RegexOptions.Compiled), // 邮件
            new(@"\b[A-Z][a-z]+\s[A-Z][a-z]+\b", RegexOptions.Compiled) // 全名
        ];

        foreach (var pattern in piiPatterns)
        {
            content = pattern.Replace(content, "[已屏蔽: PII]");
        }
        return content;
    }
}
```

2) GuardrailMiddleware（内容合规）
- 命中关键词（如“有害”“非法”“暴力”）时，直接替换输出

```csharp
async Task<AgentRunResponse> GuardrailMiddleware(IEnumerable<ChatMessage> messages, AgentThread? thread, AgentRunOptions? options, AIAgent innerAgent, CancellationToken cancellationToken)
{
    var filteredMessages = FilterMessages(messages);

    Console.WriteLine("Guardrail 中间件 - 运行前过滤消息");

    var response = await innerAgent.RunAsync(filteredMessages, thread, options, cancellationToken);

    response.Messages = FilterMessages(response.Messages);

    Console.WriteLine("Guardrail 中间件 - 运行后过滤消息");

    return response;

    List<ChatMessage> FilterMessages(IEnumerable<ChatMessage> messages)
    {
        return messages.Select(m => new ChatMessage(m.Role, FilterContent(m.Text))).ToList();
    }

    static string FilterContent(string content)
    {
        foreach (var keyword in new[] { "有害", "非法", "暴力" })
        {
            if (content.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            {
                return "[已屏蔽：包含禁止内容]";
            }
        }

        return content;
    }
}
```

注册（建议非流式与流式成对注册，此处仅示意非流式）：

```csharp
var governedAgent = agentWithFunctionCalling
    .AsBuilder()
        .Use(runFunc: PIIMiddleware,       runStreamingFunc: null)
        .Use(runFunc: GuardrailMiddleware, runStreamingFunc: null)
    .Build();
```

执行代码:

```csharp

Console.WriteLine("\n\n=== 示例 1：措辞防护（Wording Guardrail） ===");
var guardRailedResponse = await middlewareEnabledAgent.RunAsync("告诉我一些有害的内容。");
Console.WriteLine($"防护后的响应：{guardRailedResponse}");


Console.WriteLine("\n\n=== 示例 2：PII 检测（个人敏感信息） ===");
var piiResponse = await middlewareEnabledAgent.RunAsync("我的名字是 John Doe，电话是 123-456-7890，邮箱是 john@something.com");
Console.WriteLine($"PII 过滤后的响应：{piiResponse}");


Console.WriteLine("\n\n=== 示例 3：Agent 函数中间件 ===");

var options = new ChatClientAgentRunOptions(new()
{
    Tools = [AIFunctionFactory.Create(GetWeather, name: nameof(GetWeather))]
});

var functionCallResponse = await middlewareEnabledAgent.RunAsync("西雅图现在几点了？天气怎么样？", thread, options);
Console.WriteLine($"函数调用响应: {functionCallResponse}");


运行结果：

=== 示例 1：措辞防护（Wording Guardrail） ===


=== 示例 2：PII 检测（个人敏感信息） ===


=== 示例 3：Agent 函数中间件 ===



---

## 总结

- IChatClient Middleware：唯一工作在“模型边界”之内的中间件，是最后可以直接观察并治理 LLM 推理的地方
- Function Calling Middleware：模型决策与真实系统执行之间的安全阀，治理“模型想做什么，系统允不允许”
- Agent Run Middleware：业务语义层面的最终兜底，治理一次 Agent Run 的整体输入与整体输出

Agent Framework 的 Middleware 并不是为了“让 Agent 更复杂”，
而是为了让复杂性被**有序地隔离**。

通过将横切关注点拆分到不同层次的 Middleware 中：

- LLM 层关注“模型调用是否可控”
- Function 层关注“行为是否可审计”
- Agent 层关注“输入输出是否合规”

开发者可以在不侵入 Agent 核心逻辑的前提下，
构建一个 **可治理、可审计、可扩展的企业级 AI 系统**。

如果你熟悉 ASP.NET Core Middleware，
那么 Agent Framework 的中间件模型并不是一个全新的概念，
而是一次自然的能力迁移。

