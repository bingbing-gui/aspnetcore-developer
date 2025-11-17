微软开源的agent-framework 以简洁方式帮助构建具备多轮对话能力的智能 Agent。我们一如既往的沿用上一节中我们的基础配置。如果你没有看上一节，请转到上一节

---

## 一、简化多轮对话

1. **创建带上下文记忆的 Agent**  
   利用微软 agent-framework，结合 Azure OpenAI 服务，创建了一个能够记忆对话上下文的智能 Agent。每次回复都基于上一次的内容，真正实现“多轮对话”，让 Agent 能够理解和跟进用户的上下文需求。

2. **自定义 Agent 个性与功能**  
   代码中通过 instructions（角色设定）给 Agent 加入了个性：“你擅长讲笑话”，让 AI 每次回答都能契合这个人设，体验丝滑的定制化智能服务。

3. **多轮对话与上下文 Thread**  
   利用 `AgentThread` 对象维护对话上下文，将每轮交互加入 thread，实现连续多轮交谈，比如先让 AI 讲一个海盗笑话，再要求加入表情并模仿鹦鹉风格，AI 都能准确响应。

4. **流式输出体验**  
   示例还展示了多轮对话的流式输出方式（Streaming），更适合输出长文本或逐步构建回复，让用户实时看到生成过程，带来更好的交互体验。


---

## 二、代码示例

创建一个 Console 应用项目，并添加以下 NuGet 包：

```bash
dotnet add package Azure.AI.OpenAI
dotnet add package Microsoft.Agents.AI.OpenAI
dotnet add package Microsoft.Extensions.AI.OpenAI
dotnet add package Azure.Identity
```

```csharp
// Copyright (c) Microsoft. All rights reserved.

// This sample shows how to create and use a simple AI agent with a multi-turn conversation.

using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Agents.AI;
using OpenAI;
using System.Text;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);


var endpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT") ?? throw new InvalidOperationException("AZURE_OPENAI_ENDPOINT is not set.");
var deploymentName = Environment.GetEnvironmentVariable("AZURE_OPENAI_DEPLOYMENT_NAME") ?? "gpt-4o-mini";

AIAgent agent = new AzureOpenAIClient(
    new Uri(endpoint),
    new AzureCliCredential())
    .GetChatClient(deploymentName)
    .CreateAIAgent(instructions: "你是一位江湖说书人，擅长用幽默、接地气的方式讲笑话和故事。", name: "Joker");


AgentThread thread = agent.GetNewThread();
Console.WriteLine(await agent.RunAsync("给我讲一个发生在茶馆里的段子，轻松一点的那种。", thread));
Console.WriteLine(await agent.RunAsync("现在把这个段子加上一些表情符号，并用说书人的语气再讲一遍。", thread));

// Invoke the agent with a multi-turn conversation and streaming, where the context is preserved in the thread object.
thread = agent.GetNewThread();

Console.WriteLine(await agent.RunAsync("再讲一个关于江湖侠客的小笑话，要幽默一点。", thread));
Console.WriteLine(await agent.RunAsync("给这个江湖侠客的小笑话加些表情符号，再添加点夸张的江湖腔。", thread));

//await foreach (var update in agent("再讲一个关于江湖侠客的小笑话，要幽默一点。", thread))
//{
//    Console.WriteLine(update);
//}
//await foreach (var update in agent.RunStreamingAsync("给这个江湖侠客的小笑话加些表情符号，再添加点夸张的江湖腔。", thread))
//{
//    Console.WriteLine(update);
//}
```

要点：

- `AgentThread` 负责上下文关联。
- `RunAsync` 返回完整结果。
- `RunStreamingAsync` 提供逐步生成的流式体验。

---

## 三、从示例可以学到什么

1. 上下文线程 Thread 非常关键, 框架通过 Thread 封装了上下文管理，减轻了开发者负担。
2. Agent 角色设定简单直接, 只需一段说明文字，就能让 Agent 带上特定风格。
3. 流式输出易于集成，使用 RunStreamingAsync 即可获取实时生成内容。
4. 适用场景广泛：聊天助手、客服问答、教学互动、内容生成、工具型界面等。

---

## 四、结语

agent-framework 将“对话、记忆、角色”封装得轻量，便于快速构建多轮对话应用。示例代码简洁易读，适合入门与扩展。建议直接运行官方示例加深理解。 
