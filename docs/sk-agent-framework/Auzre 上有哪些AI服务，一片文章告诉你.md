## Azure AI Foundry

### 官方文档  
[https://learn.microsoft.com/zh-cn/azure/ai-foundry/](https://learn.microsoft.com/zh-cn/azure/ai-foundry/)

### Azure AI Foundry 简介 

Azure AI Foundry 是一项面向企业级 AI 运维、模型构建者与应用开发的统一 Azure 平台即服务（PaaS）。它将生产级基础设施与友好界面相结合，让开发者专注于构建应用，而非管理底层资源。  
Azure AI Foundry 在统一管理域下整合 Agent、模型与工具，并内置企业就绪能力：链路追踪、监控、评测，以及可自定义的企业级设置。平台通过统一的基于角色的访问控制（RBAC）、网络与策略，  
在同一 Azure 资源提供程序命名空间下实现简化管理。

**面向开发者的价值**

- 在企业级平台上构建生成式 AI 应用与 AI 代理。
- 借助前沿 AI 工具与机器学习模型进行探索、构建、测试与部署，并以负责任 AI 实践为基础。
- 在完整的应用开发生命周期中实现团队协作。
- 跨模型提供商使用一致的 API 契约进行开发。

**持续交付与规模化**

借助 Azure AI Foundry，你可以探索多样的模型、服务与能力，快速构建满足业务目标的 AI 应用。平台支持从概念验证（PoC）平滑扩展到生产级应用，并通过持续监控与优化，保障长期成功。

---

## Azure OpenAI Service

### 官方文档  
[https://learn.microsoft.com/en-us/azure/ai-foundry/openai/overview](https://learn.microsoft.com/en-us/azure/ai-foundry/openai/overview)

### 简介 
Azure OpenAI 提供对 OpenAI 语言模型的 REST API 访问（包括 GPT-5 系列、GPT-4 系列、GPT-3.5-Turbo、各种 o* 与 o*-mini 模型，以及 Embeddings 向量模型）。  
适用于内容生成、摘要、图像理解、语义搜索、自然语言到代码转换等场景，并支持多语言 SDK（Python/C#/JS/Java/Go 等）。

---

## Azure AI Search

### 官方文档  
[https://learn.microsoft.com/azure/search/](https://learn.microsoft.com/azure/search/)

### Azure AI Search 概述  

Azure AI Search 是一套可扩展的搜索基础设施，可为异构内容建立索引，并通过 API、应用程序与 AI 代理实现检索。该平台与 Azure 的 AI 技术栈（OpenAI、AI Foundry、Machine Learning）原生集成，并支持可扩展的架构以集成第三方与开源模型。

该服务既能处理传统搜索工作负载，也支持用于会话式 AI 应用的 RAG（检索增强生成）模式。因此，它既适用于企业级搜索场景，也适用于依赖聊天补全模型进行动态内容生成的面向客户的 AI 体验。

---

## Azure AI Video Indexer

### 官方文档  
[https://learn.microsoft.com/en-us/azure/azure-video-indexer/](https://learn.microsoft.com/en-us/azure/azure-video-indexer/)

### Azure AI Video Indexer 概述  

Azure AI Video Indexer 是 Azure AI 服务的一部分，是一款基于多项 Azure AI 能力（如 Face、Translator、Azure AI Vision、Speech）构建的云端应用。  
它使用专门的视频与音频模型，帮助你从视频中自动提取洞察。

**能力与价值**

- 对视频与音频内容运行 30+ 个 AI 模型，生成丰富的结构化洞察。
- 支持人物与人脸、语音与文字、场景与物体、语言翻译与情绪等多维分析。
- 产出可搜索、可视化、可集成的元数据，便于下游检索、编辑与内容管理。

---

## Azure AI Vision

### 官方文档  
[https://learn.microsoft.com/en-us/azure/ai-services/computer-vision](https://learn.microsoft.com/en-us/azure/ai-services/computer-vision)

### Azure AI Vision 概述

Azure AI Vision 提供先进的图像处理算法，可根据你关注的视觉特征对图像进行分析并返回信息。下表列出主要产品类别。

| 服务 | 说明 |
| --- | --- |
| 光学字符识别（OCR） | OCR 服务用于从图像中提取文本。你可以使用 Read API 从照片和文档中提取印刷体和手写体文本。该服务基于深度学习模型，适用于多种表面与背景上的文字，如商务文档、发票、收据、海报、名片、信件、白板等。OCR API 支持多种语言的印刷体文本提取。要开始使用，请参阅 OCR 快速入门。 |
| 图像分析（Image Analysis） | 图像分析服务可从图像中提取多种视觉特征，例如物体、人脸、成人内容以及自动生成的文字描述等。要开始使用，请参阅 图像分析快速入门。 |
| 人脸（Face） | 人脸服务提供用于在图像中检测、识别与分析人脸的 AI 算法。人脸识别可用于多种场景，如身份识别、免接触门禁以及出于隐私目的的人脸模糊处理。要开始使用，请参阅 人脸服务快速入门。 |

---

## Custom Vision

### 官方文档  
[https://learn.microsoft.com/en-us/azure/ai-services/custom-vision-service/](https://learn.microsoft.com/en-us/azure/ai-services/custom-vision-service/)

### Custom Vision 概述

Azure AI Custom Vision 是一项图像识别服务，允许你构建、部署并改进自己的图像识别模型。图像识别器会根据图像的视觉特征为其打上标签，每个标签代表一个分类或对象。  
Custom Vision 允许你自定义标签并训练自定义模型来检测这些标签。  
你可以通过客户端 SDK、REST API，或 Custom Vision Web 门户使用该服务。

> **公告**
>
> Microsoft 宣布计划退役 Azure Custom Vision 服务。  
> Microsoft 将在 2028 年 9 月 25 日之前为所有现有的 Azure Custom Vision 客户提供全面支持。在此支持期内，建议客户开始规划并执行向替代解决方案的迁移。根据你的使用场景，推荐以下迁移路径：

**自定义图像分类与目标检测模型**

- 使用 Azure Machine Learning AutoML，可基于经典机器学习技术训练这两类自定义模型。  
- 了解更多关于 Azure Machine Learning AutoML 的信息，并评估其对自定义模型训练的支持。

**基于生成式 AI 的方案**

- Microsoft 也在投资基于生成式 AI 的解决方案，通过提示工程等技术在自定义场景中提升准确率。  
- 若要使用生成式模型，你可以在 Azure AI Foundry 的模型目录中选择模型，构建满足自定义视觉需求的解决方案。  
- 若需要托管式的生成式图像分类方案，Azure AI Content Understanding（当前为公开预览）提供创建自定义分类工作流的能力；它还支持处理任意类型的非结构化数据（图像、文档、音频、视频），并按预定义或用户自定义的格式提取结构化洞察。  
- 了解更多 Azure AI Foundry 模型与 Azure AI Content Understanding（公开预览） 的信息，评估它们是否适合作为你的替代路径。

---

## Speech 服务

### 官方地址  
[https://learn.microsoft.com/en-us/azure/ai-services/speech-service/](https://learn.microsoft.com/en-us/azure/ai-services/speech-service/)

### Speech 服务概述

Speech 服务通过一个 Speech 资源提供语音转文本（STT）与文本转语音（TTS）能力。你可以以高准确率将语音转写为文本、合成自然逼真的语音、对口语音频进行翻译，并在对话中进行说话人识别。

---

## Azure AI Translator

### 官方文档  
[https://learn.microsoft.com/en-us/azure/ai-services/translator/](https://learn.microsoft.com/en-us/azure/ai-services/translator/)

### Azure AI Translator 概述

Azure AI Translator 是一项基于云的神经机器翻译服务，隶属于 Azure AI 服务家族，可在任何操作系统上使用。Translator 为众多 Microsoft 产品与服务提供支持，  
被全球成千上万家企业用于语言翻译及其他与语言相关的操作。通过本概览，你将了解 Translator 如何在其支持的所有语言范围内，帮助你为应用构建智能的多语言解决方案。

---

## Azure AI Language

### 官方文档  
[https://learn.microsoft.com/en-us/azure/ai-services/language-service/](https://learn.microsoft.com/en-us/azure/ai-services/language-service/)

### Azure AI Language 概述

Azure AI Language 是一项基于云的服务，提供用于理解与分析文本的自然语言处理（NLP）功能。  
你可以通过基于网页的 Language Studio、REST API 和客户端库使用该服务，帮助构建智能应用。

---

## Azure AI Content Safety

### 官方文档  
[https://learn.microsoft.com/en-us/azure/ai-services/content-safety/](https://learn.microsoft.com/en-us/azure/ai-services/content-safety/)

### Azure AI Content Safety 概述

Azure AI Content Safety 是一项用于在应用和服务中检测有害内容（包括用户生成与 AI 生成内容）的 AI 服务。它提供文本与图像 API，用于识别潜在有害素材。交互式的 Content Safety Studio 允许你在不同模态下查看、探索并试用示例代码，以检测有害内容。

内容过滤软件有助于你的应用遵循法规，或维护预期的用户环境。

---

## Azure AI Anomaly Detector API

### 官方文档  
[https://learn.microsoft.com/en-us/azure/ai-services/anomaly-detector/](https://learn.microsoft.com/en-us/azure/ai-services/anomaly-detector/)

### Azure AI Anomaly Detector API 概述

Anomaly Detector 是一项提供一组 API 的 AI 服务，即使几乎不具备机器学习（ML）知识，也能对时间序列数据进行监控与异常检测，支持批量验证与实时推断两种模式。

> **公告**  
> 自 2023 年 9 月 20 日起，你将无法创建新的 Anomaly Detector 资源。Anomaly Detector 服务将于 2026 年 10 月 1 日退役。

---

## Azure Bot Framework

### 资料地址  
[https://learn.microsoft.com/en-us/azure/bot-service/index-bf-sdk?view=azure-bot-service-4.0](https://learn.microsoft.com/en-us/azure/bot-service/index-bf-sdk?view=azure-bot-service-4.0)

### Azure Bot Framework 简介  

Microsoft Bot Framework 和 Azure AI Bot Service 是一组库、工具与服务，帮助你构建、测试、部署并管理智能机器人。  
Bot Framework 提供模块化、可扩展的 SDK，用于开发机器人并连接各类 AI 服务。借助该框架，开发者可以创建能够语音交互、理解自然语言、回答问题等功能的机器人。

---

## Azure AI Metrics Advisor

### 官方文档  
[https://learn.microsoft.com/en-us/azure/ai-services/metrics-advisor/](https://learn.microsoft.com/en-us/azure/ai-services/metrics-advisor/)

### Azure AI Metrics Advisor 概述

Metrics Advisor 隶属于 Azure AI 服务，用 AI 对时间序列数据进行数据监控与异常检测。该服务自动将模型应用到你的数据上，  
并提供一组 API 与基于 Web 的工作区，用于数据接入、异常检测与诊断——无需机器学习背景。开发者可在此之上构建 AIOps、预测性维护、业务监控等应用。

> **公告**  
> 自 2023 年 9 月 20 日起，将无法创建新的 Metrics Advisor 资源。Metrics Advisor 服务将于 2026 年 10 月 1 日退役。

---
