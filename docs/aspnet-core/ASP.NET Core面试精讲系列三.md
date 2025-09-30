## 31. ASP.NET Core 中的 MVC 是什么？与“老版”ASP.NET 的 MVC 有何不同？

---

### MVC 的定义

- **MVC（模型-视图-控制器）**是一种经典的三层模式：
    - **Model（模型）**：封装数据与业务逻辑
    - **View（视图）**：负责 UI 渲染
    - **Controller（控制器）**：处理请求与应用流程
- 优点：实现 **关注点分离**，便于维护与测试。

---

### ASP.NET Core MVC 与 ASP.NET MVC 的主要区别

| 特性           | ASP.NET MVC（旧版）         | ASP.NET Core MVC           |
| -------------- | -------------------------- | -------------------------- |
| **平台**       | 仅限 Windows / .NET Framework | 跨平台 (.NET Core/.NET 5+) |
| **依赖注入**   | 需要第三方容器              | 内置依赖注入               |
| **MVC & Web API** | 两套独立框架             | 已统一为一个管道           |
| **请求处理**   | 基于 System.Web / HttpHandler | 基于 Middleware 中间件管道 |
| **性能**       | 较重，依赖 IIS              | 轻量级，高性能，可自托管   |
| **配置**       | Web.config                  | appsettings.json + 强类型配置 |
| **部署**       | IIS 为主                    | Kestrel，自托管，跨平台部署 |

---

## 32. 什么是 Razor Pages？何时使用 Razor Pages 而不是 MVC？

- **Razor Pages** 是 ASP.NET Core 2.0 引入的 **基于页面的编程模型**，建立在 MVC 框架之上。
- 每个 `.cshtml` 文件对应一个 `PageModel` 类，负责处理请求逻辑（类似于控制器 + 视图的结合）。
- **特点**：代码组织更直观，样板代码更少，适合页面驱动的开发模式。

---

### 推荐使用 Razor Pages 的场景

- 以页面为中心的应用
- CRUD 类型的后台管理系统
- 表单驱动的 UI 界面
- 中小型项目，追求简洁性

### 推荐使用 MVC 的场景

- 大型、复杂、模块化应用
- 基于控制器和路由的清晰分层结构
- 需要构建 API 与多端交互的项目
- 团队协作开发、需要严格分层时

---

## 33. MVC / Razor Pages 的路由机制

- **MVC 路由：**
    - 支持属性路由和传统路由
    - 示例：
```csharp
endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
```
- **Razor Pages 路由：**
    - 基于文件夹结构
    - 例如：URL `/Products/Edit` 映射到 `/Pages/Products/Edit.cshtml`

---

## 34. Controller、Action、View 和 View Component

- **Controller（控制器）**：处理 HTTP 请求
- **Action（方法）**：控制器中的方法，返回结果（如视图或 JSON）
- **View（视图）**：`.cshtml` 文件，渲染 HTML
- **View Component（视图组件）**：带有逻辑的可复用小型视图（类似带代码的部分视图）

---

## 35. Tag Helper 与 HTML Helper 的区别

- **Tag Helper**：HTML 风格语法，易读易维护
```html
<form asp-controller="Home" asp-action="Login"></form>
```
- **HTML Helper**：Razor 中的 C# 方法
```csharp
@Html.BeginForm("Login", "Home")
```
- **建议**：现代 ASP.NET Core 应用优先使用 Tag Helper

---

## 36. ViewData、ViewBag、TempData 的区别与用途

| 类型      | 生命周期              | 动态类型 | 用途                                |
|-----------|-----------------------|----------|-------------------------------------|
| ViewData  | 当前请求（存放在字典） | 否       | 向视图传递数据，基于键值访问         |
| ViewBag   | 当前请求（ViewData 包装器） | 是   | ViewData 的动态包装器，语法更简洁    |
| TempData  | 跨请求（依赖 Session/Cookie，读取一次后即清除） | 否 | 在重定向后临时保存数据，如提示消息   |

---

### 示例

```csharp
// Controller
ViewData["Count"] = 5;
ViewBag.Message = "Hello";
TempData["Success"] = "保存成功";
return RedirectToAction("Index");
```

---

## 37. MVC/Razor Pages 的模型绑定

- **定义**
    ASP.NET Core 的模型绑定会自动将 **HTTP 请求中的数据**（如表单字段、查询字符串、路由数据、请求体、Header、Cookie）映射到控制器方法参数或模型对象上。
```csharp
public IActionResult Submit(User user) { ... }
```
- **支持的数据类型**
    - 简单类型：`int`、`string`、`bool`、`DateTime` 等
    - 复杂类型：自定义类、集合、嵌套对象
    - 特殊类型：`IFormFile`、枚举、字典
- **数据来源属性**
    - `[FromQuery]`：查询字符串
    - `[FromRoute]`：路由参数
    - `[FromForm]`：表单字段
    - `[FromBody]`：请求体（如 JSON/XML）
    - `[FromHeader]`：请求头
- **验证配合**
    - 模型绑定完成后，会自动触发数据注解验证和 `IValidatableObject.Validate()` 方法
    - 验证结果存入 `ModelState`

---

## 38. 使用数据注解进行模型验证

- **在模型属性上添加注解**
```csharp
public class User
{
        [Required(ErrorMessage = "邮箱必填")]
        [EmailAddress(ErrorMessage = "邮箱格式不正确")]
        public string Email { get; set; }

        [Range(18, 60, ErrorMessage = "年龄必须在 18~60 岁之间")]
        public int Age { get; set; }
}
```
- **验证触发时机**
    - 在模型绑定完成后，ASP.NET Core 会自动执行注解验证
    - 验证结果存放在 `ModelState` 中
    - 控制器中可用 `ModelState.IsValid` 检查：
```csharp
if (!ModelState.IsValid)
{
        return View(model); // 验证失败，返回错误
}
```
- **常见注解**
    - `[Required]`、`[StringLength]`、`[Range]`、`[RegularExpression]`
    - `[EmailAddress]`、`[Phone]`、`[Url]`
    - 以及自定义验证属性（继承 `ValidationAttribute`）
- **客户端验证（可选）**
    - 配合 jQuery Validation + Unobtrusive Validation，可以在页面端自动生成 `data-val-*` 属性，实现前端即时验证

---

## 39. 自定义验证（IValidatableObject、自定义验证器）

1. **IValidatableObject（跨属性验证）**
     - 用于在**模型类**中实现复杂或多属性关联的验证逻辑
     - 实现接口：`IValidatableObject`
     - 需实现方法：`IEnumerable<ValidationResult> Validate(ValidationContext context)`

     **示例：**
```csharp
public class UserModel : IValidatableObject
{
        public string Email { get; set; }
        public int Age { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext context)
        {
                if (Age < 18)
                {
                        yield return new ValidationResult(
                                "年龄必须大于 18",
                                new[] { nameof(Age) }
                        );
                }
        }
}
```

2. **自定义验证属性（ValidationAttribute）**
     - 适用于**单个属性**的自定义验证
     - 通过继承 `ValidationAttribute` 并重写 `IsValid` 方法实现

     **示例：**
```csharp
public class MyCustomAttribute : ValidationAttribute
{
        public override bool IsValid(object value)
        {
                if (value is string str && !string.IsNullOrEmpty(str))
                {
                        return str.StartsWith("A"); // 必须以 A 开头
                }
                return false;
        }

        public override string FormatErrorMessage(string name)
        {
                return $"{name} 必须以字母 A 开头";
        }
}

public class Product
{
        [MyCustom]
        public string Code { get; set; }
}
```

3. **验证触发时机**
     - ASP.NET Core 在**模型绑定**和 `ModelState` 验证时自动触发
     - 包括：
         - 属性上的 `ValidationAttribute`
         - 模型类中的 `IValidatableObject.Validate()`

---

## 40. 布局、部分视图、视图组件

- **布局（Layout）**
    - 用于定义共享结构（如头部、底部、导航栏）
    - 默认文件：`_Layout.cshtml`（放在 `Views/Shared/`）
    - 配置方式：
```csharp
@{
        Layout = "_Layout";
}
```
- **部分视图（Partial View）**
    - 可复用的 UI 片段（如 `_LoginPartial.cshtml`）
    - 调用方式：
```csharp
@await Html.PartialAsync("_LoginPartial")
@await Html.PartialAsync("_LoginPartial", model)
```
- **视图组件（View Component）**
    - 类似于小型 Controller，包含逻辑并返回视图片段
    - 示例：
```csharp
public class CartViewComponent : ViewComponent
{
        public IViewComponentResult Invoke()
                => View("Cart", model);
}
```
    - 调用方式：
```csharp
@await Component.InvokeAsync("Cart", new { userId = 1 })
```

---

## 41. 从控制器向视图如何传递数据

- **Model（强类型，推荐）**
    控制器中传递模型对象到视图：
```csharp
public IActionResult Index()
{
        var model = new MyViewModel { Name = "Alice" };
        return View(model);
}
```
    视图中接收并使用模型：
```csharp
@model MyViewModel
<h1>@Model.Name</h1>
```

- **ViewBag（动态属性）**
    控制器中设置：
```csharp
ViewBag.Message = "Hello";
```
    视图中使用：
```csharp
<p>@ViewBag.Message</p>
```

- **ViewData（字典存储）**
    控制器中设置：
```csharp
ViewData["Count"] = 5;
```
    视图中使用：
```csharp
<p>@ViewData["Count"]</p>
```

- **TempData（跨请求传递数据）**
    控制器中设置并重定向：
```csharp
TempData["Success"] = "保存成功";
return RedirectToAction("Index");
```
    视图中使用：
```csharp
<p>@TempData["Success"]</p>
```

---

## 42. ASP.NET Core MVC 中的过滤器类型及其生命周期

ASP.NET Core MVC 中，过滤器一共有 **5 大类**：

1. **Authorization Filter（授权过滤器）**
     - 在执行 **Action 之前**运行
     - 用于 **认证、权限检查**
     - 接口：`IAuthorizationFilter` / `IAsyncAuthorizationFilter`

2. **Resource Filter（资源过滤器）**
     - 在 **授权通过之后、Action 执行之前**运行
     - 常用于 **缓存、请求短路**
     - 接口：`IResourceFilter` / `IAsyncResourceFilter`

3. **Action Filter（操作过滤器）**
     - 在 **Action 方法调用前后**运行
     - 可用于日志、参数校验等
     - 接口：`IActionFilter` / `IAsyncActionFilter`

4. **Result Filter（结果过滤器）**
     - 在 **ActionResult 执行前后**触发
     - 不局限于视图结果，如 `ViewResult`、`JsonResult`、`FileResult`、`ObjectResult`
     - 接口：`IResultFilter` / `IAsyncResultFilter`

5. **Exception Filter（异常过滤器）**
     - 在 **Action 或 Result 执行过程中出现未捕获异常时**触发
     - 仅限 **MVC 管道内的异常**（不包括中间件层）
     - 接口：`IExceptionFilter` / `IAsyncExceptionFilter`

### 过滤器的应用方式

1. **全局应用**
     在 `Program.cs` 或 `Startup.cs` 中注册：
```csharp
services.AddControllers(options =>
{
        options.Filters.Add<LogActionFilter>(); // 或 new LogActionFilter()
});
```

2. **特性应用**（需要继承 Attribute）
```csharp
[LogActionFilter]
public class HomeController : Controller
{
        public IActionResult Index() => View();
}
```

3. **ServiceFilter 应用**（支持依赖注入）
```csharp
[ServiceFilter(typeof(LogActionFilter))]
public IActionResult About() => View();
```

4. **TypeFilter 应用**（支持构造函数参数）
```csharp
[TypeFilter(typeof(LogActionFilter), Arguments = new object[] { "参数值" })]
public IActionResult Contact() => View();
```

---

## 43. Razor Page 处理器（OnGet、OnPost 等）

- Razor Pages 使用处理器方法来响应请求：
```csharp
public class IndexModel : PageModel
{
        public void OnGet()
        {
                // 处理 GET 请求
        }

        public IActionResult OnPost()
        {
                // 处理 POST 请求
                return RedirectToPage("Success");
        }
}
```

### 常见处理器方法

- **OnGet / OnGetAsync**：处理 `HTTP GET`
- **OnPost / OnPostAsync**：处理 `HTTP POST`
- **OnPut / OnPutAsync**：处理 `HTTP PUT`
- **OnDelete / OnDeleteAsync**：处理 `HTTP DELETE`
- **OnPatch / OnPatchAsync**：处理 `HTTP PATCH`
    > 支持同步或异步（推荐使用 `Async` 后缀版本）

- 可用 `asp-page-handler="Update"` 指定自定义方法

### 自定义处理器

- 可以通过 **Handler 名称**区分多个方法：
```csharp
public IActionResult OnPostUpdate() { ... }
public IActionResult OnPostDelete() { ... }
```
- 页面中通过 `asp-page-handler` 指定调用哪个处理器：
```html
<form method="post" asp-page-handler="Update">
        <button type="submit">更新</button>
</form>

<form method="post" asp-page-handler="Delete">
        <button type="submit">删除</button>
</form>
```

---

## 44. Razor Pages 的依赖注入

- **在 PageModel 构造函数中注入服务**（常见方式）：
```csharp
public class IndexModel : PageModel
{
        private readonly IMyService _service;
        public IndexModel(IMyService service)
        {
                _service = service;
        }
}
```

- **在处理方法中注入服务**（通过 `[FromServices]`）：
```csharp
public IActionResult OnGet([FromServices] IMyService service)
{
        service.DoSomething();
        return Page();
}
```

- **在 Razor 视图中通过 `@inject` 注入服务**：
```csharp
@inject ILogger<IndexModel> Logger

<p>@Logger.GetType().Name</p>
```
    > 语法：`@inject <ServiceType> <VariableName>`

---

## 45. Razor 类库（RCL）

- **RCL（Razor Class Library）** 是 ASP.NET Core 支持的一种可复用类库，能够包含：
    - Razor 视图（Views）、页面（Pages）
    - Blazor 组件
    - 静态文件（需放在 `wwwroot` 文件夹下）

- **主要用途**：
    用于在多个项目间共享 **UI 组件、页面和静态资源**。

- **静态文件访问方式**：
    RCL 中的静态文件通过如下路径访问：
    ```
    _content/{库名}/{文件路径}
    ```

- **创建 RCL 的命令**：
```bash
dotnet new razorclasslib --support-pages-and-views
```
    > 默认创建的是 Blazor 组件库，如需支持 MVC / Razor Pages，请加上 `--support-pages-and-views` 参数。

---

## 46. MVC 的 Area

- **Area** 用于将大型应用按模块划分（如 `Admin`、`Customer`），方便管理 Controllers、Views、Models。
- 每个 Area 必须位于 `Areas/{AreaName}/` 文件夹下，并包含自己的 `Controllers/Views/Models`。

```csharp
[Area("Admin")]
public class DashboardController : Controller
{
        public IActionResult Index() => View();
}
```

- 启用 Area 需要在路由配置中添加规则：
```csharp
app.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
```

---

## 47. 静态资源组织、打包与压缩

- **静态资源存放位置**
    默认放在 `wwwroot/` 文件夹中，通过 `app.UseStaticFiles()` 对外公开。
    > 在 .NET 6+，写在 `Program.cs`；在 .NET 5 及之前，写在 `Startup.cs`。

- **自定义静态文件夹**
    可以配置额外路径：
```csharp
app.UseStaticFiles(new StaticFileOptions
{
        FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "MyStaticFiles")),
        RequestPath = "/Static"
});
```

- **打包与压缩方式**
    - **WebOptimizer**：ASP.NET Core 常用打包/压缩库
    - **Gulp / Webpack**：前端常用构建工具
    - **ResponseCompressionMiddleware**：启用 gzip / brotli 压缩传输
```csharp
app.UseResponseCompression();
```

---

## 48. 支持多视图引擎 / 自定义视图引擎

- **ASP.NET Core 默认视图引擎**：Razor
    > ASP.NET Core 仅内置 Razor 视图引擎，不再支持 WebForms 或 ASPX。

- **自定义/扩展视图引擎**：
    可通过实现 `IViewEngine` 接口添加或替换视图引擎。例如：

```csharp
public class MyCustomViewEngine : IViewEngine
{
        public ViewEngineResult FindView(ActionContext context, string viewName, bool isMainPage)
        {
                // 实现自定义视图查找逻辑
                return ViewEngineResult.NotFound(viewName, Array.Empty<string>());
        }

        public ViewEngineResult GetView(string executingFilePath, string viewPath, bool isMainPage)
        {
                // 实现自定义视图获取逻辑
                return ViewEngineResult.NotFound(viewPath, Array.Empty<string>());
        }
}
```

- **注册自定义视图引擎**：

```csharp
services.Configure<MvcViewOptions>(options =>
{
        options.ViewEngines.Clear(); // 可选：移除默认 Razor
        options.ViewEngines.Add(new MyCustomViewEngine());
});
```

- **常见应用场景**：
    - 从数据库或远程服务动态加载视图
    - 支持非 Razor 模板语法（如 Handlebars、Mustache 等）
    - 修改 Razor 默认的视图查找规则

---

## 49. 视图中的本地化（Localization）与全球化（Globalization）

- **服务注入**
    - 在控制器或服务层使用 `IStringLocalizer<T>`
    - 在 Razor 视图中使用 `IViewLocalizer`（根据视图路径自动匹配资源文件）

```csharp
@inject IViewLocalizer Localizer
<h1>@Localizer["Welcome"]</h1>
```

- **资源文件**
    - 在 `Resources` 文件夹下创建 `.resx` 文件
    - 命名方式：按类名或视图路径命名，例如：
    ```
    Resources/Views/Home/Index.en.resx
    Resources/Views/Home/Index.fr.resx
    Resources/Views/Home/Index.ja.resx
    ```
    - 不同语言使用不同后缀（如 `.en.resx`、`.fr.resx`、`.ja.resx`）

- **本地化配置（Program.cs / Startup.cs）**

```csharp
services.AddLocalization(options => options.ResourcesPath = "Resources");
services.AddControllersWithViews()
        .AddViewLocalization()
        .AddDataAnnotationsLocalization();

var supportedCultures = new[] { "en", "fr", "ja" };
app.UseRequestLocalization(new RequestLocalizationOptions
{
        DefaultRequestCulture = new RequestCulture("en"),
        SupportedCultures = supportedCultures,
        SupportedUICultures = supportedCultures
});
```

---

## 50. Razor Pages 与 MVC 的性能对比

- **性能**
    - Razor Pages 与 MVC 都基于 **ASP.NET Core MVC 框架和 Razor 视图引擎**，底层实现相同
    - 性能几乎没有差异，影响性能的主要是 **数据访问、路由设计、模型绑定** 等

- **简洁性**
    - Razor Pages 遵循 **“页面即端点”** 模式，适合 CRUD、表单驱动的 UI
    - 样板代码更少，开发体验更接近传统 WebForms

- **可维护性**
    - Razor Pages → 更直观，适合中小型、以页面为中心的应用
    - MVC → 控制器与视图分离更彻底，适合大型、模块化、团队协作的项目

---
