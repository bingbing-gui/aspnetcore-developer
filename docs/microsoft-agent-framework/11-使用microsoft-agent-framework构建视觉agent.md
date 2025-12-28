随着大语言模型（LLM）的发展，模型的能力已经不再局限于纯文本处理，多模态（Multimodality）——即同时理解文本、图像甚至音频的能力——正在成为标配。

本文将通过一段简练的 C# 代码示例，解析如何利用 Microsoft Agent Framework 和 Azure OpenAI 构建一个能够理解图像内容的 AI Agent。

## 1. 核心依赖与准备工作

在开始编写逻辑之前，代码首先引入必要的命名空间：

```csharp
using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Extensions.AI;
using OpenAI.Chat;
```

这里利用了 `Microsoft.Extensions.AI` 这一通用抽象层，以及 Azure 的官方 SDK。

代码从环境变量中加载配置：

```csharp
var endpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT") ...
var deploymentName = System.Environment.GetEnvironmentVariable("AZURE_OPENAI_DEPLOYMENT_NAME") ?? "gpt-4o";
```

> 注意：为了获得最佳的图像识别效果，这里默认建议使用 `gpt-4o` 这样具备原生视觉能力的模型。

## 2. 创建“视觉”Agent

这是整个示例的核心部分。我们不再仅仅创建一个简单的 Chat Client，而是将其封装为一个 Agent：

```csharp
var agent = new AzureOpenAIClient(new Uri(endpoint), new AzureCliCredential())
    .GetChatClient(deploymentName)
    .CreateAIAgent(
        name: "VisionAgent",
        instructions: "You are a helpful agent that can analyze images");
```

- 身份验证：使用 `AzureCliCredential()`，通过本地 Azure CLI 登录状态进行鉴权，避免在代码中硬编码 API Key。
- 指令（Instructions）：植入系统提示词，明确告知它“你是一个乐于助人的代理，可以分析图像”。

## 3. 构建多模态消息（Multimodal Message）

传统的聊天消息通常只有字符串文本。但在多模态场景下，一条消息（`ChatMessage`）可以包含多种类型的内容项（Content Items）。

```csharp
ChatMessage message = new(ChatRole.User, [
    new TextContent("What do you see in this image?"), // 文本内容
    new UriContent("https://.../boardwalk.jpg", "image/jpeg") // 图片内容
]);
```

在这个列表中，同时传入了：

- `TextContent`：用户的提问。
- `UriContent`：指向一张互联网图片的 URL（MIME 类型为 `image/jpeg`）。

框架会自动将这个结构转换为底层模型（如 GPT-4o）能理解的 JSON 格式。

## 4. 运行与流式输出

最后，启动一个新的对话线程，并让 Agent 处理消息：

```csharp
var thread = agent.GetNewThread();

await foreach (var update in agent.RunStreamingAsync(message, thread))
{
    Console.WriteLine(update);
}
```

- Thread（线程）：代表一次对话的上下文容器。
- `RunStreamingAsync`：异步流式方法，可逐字（或逐块）输出 Agent 的思考过程和回复，提升用户体验。

## 总结

这段仅 20 行左右的代码，清晰地展示了下一代 AI 应用开发的范式：

- 抽象化：通过 Agent Framework 封装底层 API 调用。
- 安全性：使用 Token 鉴权而非 Key。
- 多模态：像处理文本一样自然地处理图像数据。

通过这种方式，开发者可以轻松地将“视觉理解”能力集成到自动化工作流、数据分析或客户服务机器人中。