## 模型绑定与验证

## 71. 模型绑定如何工作：会用哪些数据源

ASP.NET Core 会把请求里的值自动映射到你的参数/模型，依据参数类型与特性选择数据源：

- 路由（route）
- 查询字符串（query）
- 表单（form，含文件 IFormFile）
- 请求体（JSON 等 application/json）
- 请求头（headers）
- 服务容器（[FromServices]）

常用特性可显式指定来源：`[FromRoute]`、`[FromQuery]`、`[FromForm]`、`[FromBody]`、`[FromHeader]`、`[FromServices]`。

---

## 72. 绑定复杂类型 vs 简单类型

**简单类型**（int/string/bool/DateTime/...）  
默认从 路由 / 查询 / 表单 绑定；可用 `[FromRoute]` / `[FromQuery]` / `[FromForm]` 指定。

**复杂类型**（自定义类/DTO）  
在带 `[ApiController]` 的控制器里默认从 JSON Body 绑定；否则显式加 `[FromBody]`。  
同一 Action 仅允许一个 `[FromBody]` 参数。

```csharp
[HttpPost]
public IActionResult Create([FromBody] Product product) => Ok();

[HttpGet("{id}")]
public IActionResult Get([FromRoute] int id, [FromQuery] int page = 1) => Ok();
```

---

## 73. 自定义模型绑定器

当内置 `[FromQuery]`/`[FromRoute]`/`[FromBody]`/`[FromHeader]` 无法满足（如特殊解析、校验或多源合并）时：

1. 实现 `IModelBinder`
2. 用 `[ModelBinder(BinderType=...)]` 标注参数
3. （可选）全局注册 `IModelBinderProvider`

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
public sealed class UserIdFromHeaderBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext ctx)
    {
        if (ctx.HttpContext.Request.Headers.TryGetValue("X-User-Id", out var v)
            && long.TryParse(v, out var id))
            ctx.Result = ModelBindingResult.Success(id);
        else
            ctx.ModelState.AddModelError(ctx.FieldName, "Missing or invalid X-User-Id header.");
        return Task.CompletedTask;
    }
}
[HttpGet("me")]
public IActionResult GetMe(
    [ModelBinder(BinderType = typeof(UserIdFromHeaderBinder))] long userId)
    => Ok(userId);
```

---

## 74. 多个绑定源特性（`[FromBody]`、`[FromQuery]` 等）

一个 Action 只能绑定一个 `[FromBody]` 参数。若需接收多个字段，请封装为一个 DTO（或使用 multipart/form-data / `[FromForm]`）。

常见绑定源：

- `[FromBody]`：请求体（JSON/XML）
- `[FromQuery]`：查询字符串
- `[FromRoute]`：路由参数
- `[FromForm]`：表单字段 / 文件上传
- `[FromHeader]`：HTTP 请求头

> 小贴士：在带 `[ApiController]` 的控制器里，复杂类型默认从 Body 绑定，简单类型默认从 Route/Query 绑定。

---

## 75. 模型验证：数据注解

数据注解验证（MVC & API 通用）

```csharp
public class User 
{
    [Required]
    [StringLength(50)]
    [EmailAddress]
    public string Email { get; set; }
}
```

- 适用于 MVC 与 Web API 的输入验证。
- 带 `[ApiController]` 时，验证失败会自动返回 400。
- 传统 MVC 中，使用 `ModelState.IsValid` 判断并处理。

---

## 76. 服务端验证与客户端验证（非侵入式）

- **服务端验证**：始终执行（安全基线）。API 场景下配合 `[ApiController]` 可自动返回 400；MVC 用 `ModelState.IsValid` 判断。
- **客户端验证**：HTML5 + jQuery 非侵入式，仅提升体验，不可替代服务端验证。

---

## 77. 自定义验证特性

继承 `ValidationAttribute` 定义规则：

```csharp
public sealed class MustBeEvenAttribute : ValidationAttribute
{
    public MustBeEvenAttribute() => ErrorMessage = "Number must be even.";

    public override bool IsValid(object value)
    {
        if (value is null) return true;              // 交给 [Required]
        if (value is int i) return (i % 2) == 0;
        return int.TryParse(value.ToString(), out var n) && (n % 2) == 0;
    }
}
```

使用方式：

```csharp
[MustBeEven]
public int Number { get; set; }
```

---

## 78. IValidatableObject 接口

用于模型级/跨字段业务校验；在数据注解之后执行，错误进入 ModelState（API 场景下 `[ApiController]` 会自动返回 400）。

```csharp
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Product : IValidatableObject
{
    public string Name  { get; set; }
    public decimal Price { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (Price < 0)
            yield return new ValidationResult("价格必须为非负数", new[] { nameof(Price) });

        if (string.IsNullOrWhiteSpace(Name))
            yield return new ValidationResult("名称必填", new[] { nameof(Name) });

        // 跨字段示例：名称包含“高级”时，价格需≥1000
        if (Name?.Contains("高级") == true && Price < 1000)
            yield return new ValidationResult("“高级”商品价格需 ≥ 1000",
                                              new[] { nameof(Name), nameof(Price) });
    }
}
```

---

## 79. 使用 FluentValidation

安装：

```shell
dotnet add package FluentValidation.AspNetCore
```

验证器：

```csharp
using FluentValidation;
public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(50);
    }
}
```

注册：

```csharp
// Program.cs
builder.Services
    .AddFluentValidationAutoValidation()
    .AddValidatorsFromAssemblyContaining<UserValidator>();
```

>  相比数据注解：规则更清晰、可单元测试，支持条件/规则集/本地化等高级用法。

---

## 80. API 与 MVC 的验证（错误响应格式）

- 有 `[ApiController]`（Web API）：模型验证自动执行；ModelState 无效时自动返回 400 BadRequest，负载为 ValidationProblemDetails（application/problem+json）。
- 无 `[ApiController]`（MVC/Razor Pages）：需手动检查。

```csharp
if (!ModelState.IsValid) return View(model); // 回显表单与错误
```

示例（API 自动 400）：

```csharp
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    [HttpPost]
    public IActionResult Create(User dto) => Ok(); // 无需手动判 ModelState
}
```

---

## 81. 模型状态：检查 ModelState.IsValid

用于确认模型绑定 + 验证是否通过；未通过则返回错误或回显页面。

```csharp
// Web API
if (!ModelState.IsValid)
    return BadRequest(ModelState);

// MVC
// if (!ModelState.IsValid) return View(model);
```

> 小贴士：带 `[ApiController]` 时，验证失败会自动返回 400，无需手动判断。

---

## 82. 绑定嵌套对象和集合

ASP.NET Core 原生支持嵌套属性与集合的模型绑定（JSON 或表单）。

```csharp   
public class Order {
    public Customer Customer { get; set; }
    public List<Product> Products { get; set; }
}
public class Customer { public string Name { get; set; } }
public class Product  { public int Id { get; set; } public string Name { get; set; } }
```

JSON 示例（`[FromBody]`）：

```json
{ "customer": { "name": "Alice" }, "products": [ { "id": 1, "name": "Pen" } ] }
```

表单键名（`[FromForm]`）：

```
Customer.Name=Alice
Products[0].Id=1
Products[0].Name=Pen
```

要点：属性名匹配 + 索引标记（[0]、[1]）即可完成集合绑定。

---

## 83. 处理缺失或无效数据

- 必填字段用 `[Required]`；范围/格式用 `[Range]`/`[StringLength]`/`[EmailAddress]` 等。
- 可选值使用可空类型（string?、int?、DateTime?）。
- 在 API 中（`[ApiController]`）验证失败会自动返回 400；在 MVC 中手动检查 ModelState：

```csharp
if (!ModelState.IsValid) return ValidationProblem(ModelState);
```

- 需要自定义提示时，设置 ErrorMessage 或使用 FluentValidation 定制错误信息。
- 仅对“缺少绑定值”严格要求时，可用 `[BindRequired]`（常见于 Query/Route）。

---

## 84. 输入数据的清理

模型绑定不会清洗输入，只是把原始数据填到模型里。

🛡 防止攻击（XSS、注入），需清理：

- 输出编码：默认 Razor 会 HTML 编码；谨慎使用 `@Html.Raw`。
- 输入校验/规范化：长度、格式、白名单（必要时再做“去标签”）。
- 防注入：始终使用参数化查询/ORM，不拼接 SQL。
- 文件上传：校验扩展名与 MIME、限制大小、病毒/恶意扫描、随机文件名、存放在非 Web 根目录。
- （可选）开启 CSP、HttpOnly/SameSite Cookie，降低 XSS 风险。

---

## 85. 文件绑定（IFormFile）

用于表单文件上传（需 `enctype="multipart/form-data"`；不要用 `[FromBody]`）。

```csharp
[HttpPost]
public async Task<IActionResult> Upload([FromForm] IFormFile file)
{
    if (file == null || file.Length == 0) return BadRequest("Empty file.");

    var uploads = Path.Combine(builder.Environment.ContentRootPath, "uploads");
    Directory.CreateDirectory(uploads);

    var safeName = Path.GetRandomFileName() + Path.GetExtension(file.FileName);
    var path = Path.Combine(uploads, safeName);

    await using var fs = System.IO.File.Create(path);
    await file.CopyToAsync(fs);

    return Ok(new { file = safeName, size = file.Length });
}
```

**要点**

- 多文件：`List<IFormFile> files`
- 放到非 Web 根目录，使用随机文件名，白名单校验扩展名/MIME，限制大小
- 需要时做病毒/恶意扫描与异步流式写入

