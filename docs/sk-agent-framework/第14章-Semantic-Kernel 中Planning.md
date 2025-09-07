当你拥有多个插件时，接下来就需要一种方式让你的 AI 代理组合使用这些插件，来完成用户的请求——这就是**规划（Planning）**的用途。

## 1. Planning的演进

在 Semantic Kernel 的早期阶段，曾引入“Planner”的概念，使用提示词（Prompt）引导 AI 决定要调用哪些函数。但自那之后，OpenAI 引入了**原生函数调用（Function Calling）**功能；其他 AI 模型如 Gemini、Claude、Mistral 也将其作为核心功能支持，这使得函数调用成为一个跨模型通用特性。

因此，Semantic Kernel 也随之演进，将函数调用作为主要的任务规划与执行方式。

> **重要提示**  
> 函数调用仅适用于 OpenAI 模型 0613 或更新版本。如果你使用的是旧版本模型（如 0314），此功能将无法使用并返回错误。我们建议使用最新版本的 OpenAI 模型以充分利用此特性。

---

## 2. 函数调用是如何创建Planning的？

从本质上讲，函数调用只是 AI 以正确参数调用函数的一种方式。例如，用户希望打开一个灯泡：

| 角色 | 消息内容 |
| ---- | -------- |
| 🔵 User | 请打开 1 号灯 |
| 🔴 Assistant (function call)	| `Lights.change_state(1, { "isOn": true })` |
| 🟢 Tool | `{ "id": 1, "name": "台灯", "isOn": true, "brightness": 100, "hex": "FF0000" }` |
| 🔴 Assistant | 灯已打开 |

但如果用户不知道灯的 ID 呢？或者用户希望同时打开所有灯？这时就需要**“规划”能力**。

现代 LLM 模型具备逐步调用函数并基于结果做出决策的能力。AI 可以先调用函数获取状态，再决定执行哪个动作。

### 例子：用户想“切换所有灯的状态”

| 角色 | 消息内容 |
| ---- | -------- |
| 🔵 User | 请切换所有灯的状态 |
| 🔴 Assistant (function call)| `Lights.get_lights()` |
| 🟢 Tool | `{ lights: [{ id: 1, isOn: true }, { id: 2, isOn: false }] }` |
| 🔴 Assistant (function call) | `Lights.change_state(1, { "isOn": false })`<br/>`Lights.change_state(2, { "isOn": true })` |
| 🟢 Tool | 灯状态更新成功 |
| 🔴 Assistant | 所有灯的状态已切换 |

> 💡 **说明：并行调用**  
> 上述例子展示了**并行函数调用**——AI 可同时发出多个函数调用请求。此功能从 OpenAI 模型 1106 版本开始支持，大大加快了处理复杂任务的效率。

---

## 3. 自动规划循环（Function Calling Loop）

如果不使用 Semantic Kernel，想要支持函数调用规划，你需要自己实现一个“调用循环”：

1. 为每个函数创建 JSON schema
2. 提供给 LLM 当前的聊天历史和函数 schema
3. 解析 LLM 返回内容，判断是回复消息还是调用函数
4. 如果是函数调用，提取函数名和参数
5. 执行对应函数
6. 将结果返回给 LLM
7. 重复第 2～6 步，直到任务完成或需要用户输入

✅ **Semantic Kernel 帮你自动完成这个循环**  
你只需专注于构建插件来满足用户需求，其余步骤由 Semantic Kernel 自动处理。

> ℹ️ 想深入了解函数调用循环的执行机制，请参考 Function Calling 工作原理 文档。

---

## 4. 如何在 Semantic Kernel 中启用函数调用

你需要完成以下 3 步：

### 1. 注册插件

```csharp
var builder = Kernel.CreateBuilder()
    .AddAzureOpenAIChatCompletion(modelId, endpoint, apiKey);

builder.Plugins.AddFromType<LightsPlugin>("Lights");
var kernel = builder.Build();
```

### 2. 启用自动函数调用

```csharp
var settings = new OpenAIPromptExecutionSettings
{
    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
};
```

### 3. 使用 ChatCompletion 启动交互

```csharp
var chatService = kernel.GetRequiredService<IChatCompletionService>();
var history = new ChatHistory();

string? userInput;
do {
    Console.Write("User > ");
    userInput = Console.ReadLine();
    history.AddUserMessage(userInput);

    var result = await chatService.GetChatMessageContentAsync(
        history,
        executionSettings: settings,
        kernel: kernel);

    Console.WriteLine("Assistant > " + result);
    history.AddMessage(result.Role, result.Content ?? string.Empty);
} while (userInput is not null);
```

✅ 所有函数调用过程和结果都会自动写入 ChatHistory，你可以随时查看 AI 调用了哪些函数以及它是如何得出最终结论的。

---

## 5. 什么情况废弃了旧的规划器？

Stepwise Planner 和 Handlebars Planner 这两个旧有的规划器已经被弃用并移除，不再支持 .NET / Python / Java。

请改用**函数调用（Function Calling）**，它提供了更强大、可控、且简洁的开发体验。

> 🔁 如需将旧方案迁移到函数调用，请参考官方的 Stepwise Planner 迁移指南。

---

## 6. 结语

| 项目 | 推荐做法 |
| ---- | -------- |
| 新项目Planning | 使用函数调用 Function Calling |
| 旧代码迁移 | 放弃 Stepwise / Handlebars Planner |
| 多插件协调执行 | 利用函数调用实现自动规划和执行 |
| 多模型支持 | 兼容 OpenAI、Gemini、Claude、Mistral 等 |

