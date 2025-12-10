在构建企业级 AI 应用时，我们不仅关注它“能不能跑通”
，更关注它“跑得稳不稳”。当你的 AI Agent 在生产环境中每天处理成千上万次请求时，是否遇到过这些问题：

- 🤔 这个 Agent 回答为什么这么慢？是网络卡了还是模型推理慢？
- 💸 今天的 Token 消耗量是多少？有没有异常激增？
- 🐛 调用链在哪里断开了？

今天，我们使用microsoft-agent-framework看看如何利用 OpenTelemetry 为你的 AI Agent 赋予“可观测性”超能力。

## 什么是可观测性？

简单来说，就是让你的代码“自己说话”。通过收集 Logs（日志）、Metrics（指标）和 Traces（分布式追踪），可以清晰地看到 AI Agent 内部发生了什么。

## 实战：3 步实现监控
基于 Microsoft.Agents.AI，集成监控很简单。

### 第一步：准备 Tracer

首先，配置 OpenTelemetry 的收集器。在开发阶段输出到控制台；在生产环境发往 Azure Application Insights。

使用 CLI 安装所需依赖（在项目目录运行）：

    ```bash
    dotnet add package Azure.AI.OpenAI --version 2.1.0
    dotnet add package Azure.Identity --version 1.18.0-beta.2
    dotnet add package Azure.Monitor.OpenTelemetry.Exporter --version 1.6.0-beta.1
    dotnet add package Microsoft.Agents.AI.OpenAI --version 1.0.0-preview.251125.1
    dotnet add package Microsoft.Extensions.AI.OpenAI --version 10.1.0-preview.1.25608.1
    dotnet add package OpenTelemetry --version 1.14.0
    dotnet add package OpenTelemetry.Exporter.Console --version 1.14.0
    ```


```csharp
// 定义数据源名称
string sourceName = Guid.NewGuid().ToString("N");

var tracerProviderBuilder = Sdk.CreateTracerProviderBuilder()
    .AddSource(sourceName) // 注册数据源
    .AddConsoleExporter(); // 输出到控制台，方便调试

// 如果配置了连接字符串，则发送到 Azure Monitor
if (!string.IsNullOrWhiteSpace(appInsightsStr))
{
    tracerProviderBuilder.AddAzureMonitorTraceExporter(
        options => options.ConnectionString = appInsightsStr);
}

using var tracerProvider = tracerProviderBuilder.Build();

```
### 第二步：注入 Agent
微软的 Agent Framework 采用流畅的 Builder 模式，只需调用 `.UseOpenTelemetry()`。

```csharp
AIAgent agent = new AzureOpenAIClient(new Uri(endpoint), new AzureCliCredential())
    .GetChatClient(deploymentName)
    .CreateAIAgent(instructions: "你是一位江湖说书人，擅长用幽默、接地气的方式讲笑话和故事。", name: "Joker")
    .AsBuilder()
    .UseOpenTelemetry(sourceName: sourceName)
    .Build();
```

### 第三步：像往常一样运行
无需修改业务逻辑代码，无论是普通调用还是流式调用，监控数据都会自动采集。

```csharp
// 运行 Agent
Console.WriteLine(await agent.RunAsync("给我讲一个发生在茶馆里的段子，轻松一点的那种。"));
```

## 效果展示

运行后，控制台会打印出海盗笑话，并输出详细的追踪信息（Activity），包括：


### 📌 调用核心信息

- **模型（gen_ai.request.model）**：gpt-4o  
- **返回模型版本（gen_ai.response.model）**：gpt-4o-2024-11-20  
- **Agent 名称（gen_ai.agent.name）**：Joker  
- **Agent ID（gen_ai.agent.id）**：ceb8e71a294b4d9c9391b50410bad3cd  
- **调用耗时（Activity.Duration）**：17.77 秒  
- **Finish Reason（gen_ai.response.finish_reasons）**：stop  

### 📈 Token 使用（计费关键）
- **输入 Tokens（gen_ai.usage.input_tokens）**：57  
- **输出 Tokens（gen_ai.usage.output_tokens）**：419  
- **总 Tokens（推导）**：476  

### 🌐 服务端信息

- **Endpoint（server.address）**：maf.cognitiveservices.azure.com  
- **Port（server.port）**：443  

### 🧾 响应信息
- **Response ID（gen_ai.response.id）**：chatcmpl-ClF1Rn0e2I6iSRXpTwSfl3iVbg0VS  
- **Trace ID（Activity.TraceId）**：c427b1eac74950961541c0204a685ede  
- **Span ID（Activity.SpanId）**：cdef95766cffb01d  


## 总结

通过 OpenTelemetry 的标准化协议，可以轻松将 AI Agent 的健康状况可视化。这既有助于快速定位性能瓶颈，也为后续的成本优化提供数据支持。