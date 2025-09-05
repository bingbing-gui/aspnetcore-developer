# 第3章-Semantic-Kernel核心组件

Semantic Kernel 提供了许多不同的组件，这些组件可以单独使用，也可以组合使用。本文概述了这些不同的组件，并解释了它们之间的关系。

## AI 服务连接器（AI Service Connectors）

AI 服务连接器提供了一个抽象层，通过统一的接口暴露来自不同提供商的多种 AI 服务类型。支持的服务包含聊天补全、文本生成、嵌入生成、文本转图像、图像转文本、文本转音频、音频转文本。

默认情况下，当你注册一个**AI 代码实现（插件、技能、函数等）**到 Kernel（核心调度器） 后，Kernel 只会使用 Chat Completion 或 Text Generation。如果要调用嵌入、向量搜索、图像生成等其他服务，需要手动注册并调用。


## 向量存储连接器（Vector Store Connectors）

Semantic Kernel 的向量存储连接器提供了一个抽象层，通过通用接口暴露来自不同提供商的向量存储。Kernel 不会自动 使用任何已注册的向量存储，但可以通过插件将**向量搜索（Vector Search）**暴露给 Kernel。在这种情况下，该插件将可用于 Prompt Templates（提示模板） 和 Chat Completion AI Model（聊天补全 AI 模型）

## 函数和插件(Functions and Plugins)

插件（Plugins）可以理解为一组命名的功能模块，其中每个插件包含一个或多个可调用的函数。这些插件可以被注册到 Kernel（核心调度器），然后 Kernel 可以用两种方式使用它们：

1. 让 Chat Completion AI 直接调用插件，这意味着 AI 可以在对话过程中自主选择并执行插件中的函数，比如调用某个 API、查询数据库、计算数据等。

2. 在模板渲染过程中调用插件，也就是说，当系统解析一个提示模板（Prompt Template）时，可以在模板里直接调用插件中的函数，以增强 AI 的生成能力。


插件的表现形态，常见的有：

1. 本地代码（Native Code）：直接用 .NET、Python、Java等编写的函数，可以作为插件。
2. OpenAPI 规范（OpenAPI specs）：如果某个服务有 OpenAPI 描述，Kernel 可以直接调用它，作为插件的一部分。
3. ITextSearch 实现（用于 RAG 场景）：当需要 AI 在**检索增强生成（RAG）**场景下查找信息时，可以用 ITextSearch 让 AI 直接查询文档数据库或知识库。
4. 提示模板（Prompt Templates）：插件的功能可以用提示模板描述，这样 AI 可以利用这些模板来完成更复杂的任务。

![](/docs/SemanticKernel/Materials/plugins-from-sources.png)

## 提示模板 (Prompt Templates)

提示模板（Prompt Templates）是开发人员或提示工程师创建的一种预设模板，它可以结合 AI 的上下文、指令、用户输入和函数输出，帮助 AI 生成更精准的回答。例如，模板里可以包含：
1. AI 该如何思考或回答（比如“请用简洁的方式回答问题”）。
2. 用户输入的占位符（比如“{用户问题}”）。
3. 调用某些插件或函数（比如在 AI 处理之前先查询数据库或调用 API）。

提示模板的两种使用方式

1. 作为 Chat Completion 的起点，你可以让 Kernel（核心调度器） 先渲染这个模板，然后用它的结果去调用 AI，让 AI 按照模板提供的格式和信息进行对话。

2. 作为插件函数使用，你可以把模板注册成一个插件函数，这样它就可以像普通的函数一样被调用，不管是 AI 选择调用它，还是模板内部引用它，都可以实现更复杂的 AI 交互。

当我们使用提示模板（Prompt Templates）时，它会先被渲染（处理），然后执行其中的硬编码函数（如果有的话）。处理后的提示会被传递给 Chat Completion AI，然后 AI 生成结果并返回。

如果这个提示模板被注册成了一个插件函数，那么 AI 可能会选择调用它。在这种情况下，调用这个函数的并不是用户，而是 Semantic Kernel 代表 AI 进行调用。

假设有两个提示模板：

模板 A 已经注册为插件（它可以被 AI 或其他模板调用）。
模板 B 由用户输入，并且要经过 Kernel 处理。
如果 B 里面有一个对 A 的硬编码调用，整个流程会变成这样：

1. B 开始渲染，但它发现需要 A 的数据。
2. A 被渲染。
3. A 的结果被传递给 AI 进行处理。
4. AI 生成 A 的结果，并返回给 B。
5. B 继续渲染，现在它有 A 的数据了。
6. B 的渲染结果被传递给 AI。
7. AI 最终返回 B 的结果给用户。

这个流程相当于B 依赖 A，必须先执行 A 才能完成。


如果 B 里没有明确调用 A，但AI 开启了函数调用功能，AI 可能会觉得“我需要 A 来提供数据或执行某个任务”，然后自己选择去调用 A。这意味着AI 可能会在后台自动调用插件，而用户并不知道这个调用过程。

为什么要把提示模板做成插件？

1. 让 AI 通过自然语言调用功能，而不是写代码。比如，我们可以用人类语言描述一个功能，AI 就可以使用它，而不需要编程。
2. 让 AI 可以单独思考每个功能，而不是一次处理所有任务，这样可以提高 AI 的成功率，让它更聪明。

![](/docs/SemanticKernel/Materials/template-function-execution.png)


## 过滤器

过滤器（Filters）提供了一种在聊天补全（Chat Completion）流程中，在特定事件发生前后执行自定义操作的方法。这些事件包括：

1. 函数调用前后
2. 提示渲染前后
过滤器需要注册到内核（Kernel），才能在聊天补全流程中被调用。

需要注意的是，由于提示模板（Prompt Templates）在执行前始终会被转换为 KernelFunctions，因此对于一个提示模板，函数过滤器（Function Filters）和提示过滤器（Prompt Filters）都会被调用。
当存在多个过滤器时，它们是嵌套执行的，函数过滤器是外层过滤器，提示过滤器是内层过滤器。

![](/docs/SemanticKernel/Materials/filters-overview.png)

