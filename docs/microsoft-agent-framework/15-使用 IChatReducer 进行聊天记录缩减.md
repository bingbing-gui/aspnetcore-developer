# 使用ChatReduction 进行聊天记录缩减（Microsoft Agent Framework）

这段代码的核心目的是演示在Microsoft Agent Framework中，当本地存储聊天记录时，如何通过 IChatReducer 策略来防止上下文（Token）超出模型限制。

## 1. 核心概念：聊天记录缩减（Chat Reduction）

在使用大语言模型（LLM）进行多轮对话时，随着对话次数增加，发送给模型的 ChatHistory（上下文）会越来越长，会带来：
- 成本增加：Token 消耗变大。
- 溢出风险：超过模型的最大上下文窗口限制（Context Window Limit）。

通过自动“遗忘”旧消息，仅保留最近几条，可以有效控制上下文长度与成本。

## 2. 代码关键实现步骤

### 引入必要的依赖

- Microsoft.Extensions.AI：通用 AI 抽象
- Azure.AI.OpenAI：连接 Azure OpenAI 服务

### 配置 Agent 与缩减策略（Reducer）

```csharp
AIAgent agent = new AzureOpenAIClient(/* ... */)
    .GetChatClient(deploymentName)
    .CreateAIAgent(new ChatClientAgentOptions
    {
        // ... 其他配置 ...

        // 关键点：自定义 ChatMessageStoreFactory
        ChatMessageStoreFactory = ctx => new InMemoryChatMessageStore(
            new MessageCountingChatReducer(2), // 只保留最后 2 条非系统消息
            ctx.SerializedState,
            ctx.JsonSerializerOptions)
    });
```

说明：
- InMemoryChatMessageStore：聊天记录保存在内存中。
- MessageCountingChatReducer(2)：基于“消息数量”的缩减策略；参数2表示仅保留最近2条非系统消息，历史不会无限增长。

### 验证缩减效果

通过连续多轮对话验证策略是否生效：
1. 第一次对话（“Pirate joke”）：历史较短，正常回答；记录当前历史数。
2. 第二次对话（“Robot joke”）：加入新对话，旧对话仍在（若未超过限制）。
3. 第三次对话（“Lemur joke”）：超过阈值后，缩减器开始工作，会移除最早的“Pirate”相关记录，保持总数不超出设定。

### 演示“遗忘”现象

```csharp
// 历史已被缩减，最初的消息已不存在
Console.WriteLine(await agent.RunAsync("Tell me the joke about the pirate again...", thread));
```

- 用户意图：让 Agent 复述最开始的海盗笑话。
- 预期结果：Agent 无法复述，可能产生新笑话或出现幻觉。
- 原因：仅保留最近 2 条消息，“Pirate”对话已被移除，Agent 无从得知。

## 3. 技术总结与适用场景

- 适用场景：
  - Client-side history（客户端管理历史）的场景，例如使用标准的 OpenAI Chat Completion API。
  - 若使用 Server-side history（如 Azure Foundry Agents），无需在客户端重复管理，服务端会处理。

- 可扩展性：
  - IChatReducer 可自定义实现更复杂逻辑：
    - TokenCountingChatReducer：按 Token 数量缩减。
    - SummaryChatReducer：将旧消息进行摘要保留，而非直接删除。

总之，该示例体现了 LLM 应用中的上下文管理（Context Management）设计模式，通过合理的缩减策略在成本、性能与效果间取得平衡。