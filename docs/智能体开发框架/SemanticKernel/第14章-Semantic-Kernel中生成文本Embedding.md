

ITextEmbeddingGenerationService 定义在 Microsoft.SemanticKernel.Abstractions 包里

你不需要自己实现它

只要通过 SK 的扩展方法注册一个 Embedding 生成器，就会自动把 ITextEmbeddingGenerationService 加入到依赖注入（DI）容器里

2. 如何配置这个接口
✅ 如果你用 OpenAI
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

var builder = WebApplication.CreateBuilder(args);

// 注册 OpenAI Embedding 服务
builder.Services.AddOpenAITextEmbeddingGeneration(
    modelId: "text-embedding-3-small", // 你要用的 OpenAI 模型
    apiKey: builder.Configuration["OpenAI:ApiKey"] // 从配置里读取 key
);


这样之后，你就可以在代码里直接注入使用：

public class HotelService
{
    private readonly ITextEmbeddingGenerationService _embeddingService;

    public HotelService(ITextEmbeddingGenerationService embeddingService)
    {
        _embeddingService = embeddingService;
    }

    public async Task AddHotelAsync(string description)
    {
        var vector = await _embeddingService.GenerateEmbeddingAsync(description);
        // 把 vector 存入数据库
    }
}

✅ 如果你用 Azure OpenAI
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;

var builder = WebApplication.CreateBuilder(args);

// 注册 Azure OpenAI Embedding 服务
builder.Services.AddAzureOpenAITextEmbeddingGeneration(
    deploymentName: "embeddings", // 你在 Azure Portal 里配置的部署名
    endpoint: builder.Configuration["AzureOpenAI:Endpoint"], // Azure 资源 Endpoint
    apiKey: builder.Configuration["AzureOpenAI:ApiKey"]      // Azure OpenAI key
);


用法和 OpenAI 一样：

public class SearchService
{
    private readonly ITextEmbeddingGenerationService _embeddingService;

    public SearchService(ITextEmbeddingGenerationService embeddingService)
    {
        _embeddingService = embeddingService;
    }

    public async Task SearchAsync(string query)
    {
        var queryVector = await _embeddingService.GenerateEmbeddingAsync(query);
        // 用向量查询数据库
    }
}

3. 总结

ITextEmbeddingGenerationService 是 SK 的统一接口，用来生成向量

你不手动 new 它，而是通过 AddOpenAITextEmbeddingGeneration 或 AddAzureOpenAITextEmbeddingGeneration 把它注册到 DI

用法：注入进来直接 GenerateEmbeddingAsync("文本") 即可