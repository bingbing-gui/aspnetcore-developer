在 ASP.NET Core 开发中，解析 User-Agent 字符串是常见需求。User-Agent 包含了客户端浏览器、操作系统等信息，但其格式复杂，手动解析容易出错。推荐使用第三方库来简化这一过程。

## 推荐库：UAParser

UAParser 是一个流行的 User-Agent 解析库，支持 .NET 平台。它可以将 User-Agent 字符串解析为结构化信息，如浏览器名称、版本、操作系统等。

Github 地址：[UAParser]()

图片


## 安装方法

通过 NuGet 安装 UAParser：

```bash
dotnet add package UAParser
```

## 使用示例

在 ASP.NET Core 控制器中使用 UAParser 解析 User-Agent：

```csharp
public class HomeController : Controller
{
    public IActionResult Index()
    {
        var uaParser = Parser.GetDefault();
        var userAgent = Request.Headers["User-Agent"].ToString();
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

        ClientInfo c = uaParser.Parse(userAgent);
        Console.WriteLine(c.ToString());
        Console.WriteLine(c.UA.Family); // 浏览器名称
        Console.WriteLine(c.UA.Major);  // 主版本号
        Console.WriteLine(c.UA.Minor);  // 次版本号
        Console.WriteLine(c.OS.Family); // 操作系统名称
        Console.WriteLine(c.OS.Major);  // 操作系统主版本号
        Console.WriteLine(c.OS.Minor);  // 操作系统次版本号
        Console.WriteLine(c.Device.Family); // 设备类型

        return View();
    }
}
```

示例输出：



## Demo 地址

GitHub：[UAParser](https://github.com/ua-parser/uap-csharp)

## 总结

使用 UAParser 可以轻松解析 User-Agent 字符串，获取浏览器和操作系统等信息，简化 ASP.NET Core 应用中的用户手动解析工作。
