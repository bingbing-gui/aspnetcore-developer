## 模型绑定与验证

## 71. 模型绑定如何工作：考虑哪些数据源

ASP.NET Core 的模型绑定会将传入的 HTTP 数据映射到 C# 参数或模型属性。

🔍 **考虑的数据源：**

```
● 路由数据
```
```
● 查询字符串
```
```
● 表单数据
```
```
● 请求头
```
```
● JSON 请求体（application/json）
```
```
● 上传的文件
```
```
● 服务（通过 [FromServices]）
```
ASP.NET Core 会根据参数类型和特性（如 [FromBody]、[FromQuery] 等）**自动绑定**数据。

## 72. 绑定复杂类型与简单类型

```
● 简单类型（int、string、bool、DateTime 等）：从路由、查询字符串或表单字段绑定。
```
```
● 复杂类型（自定义类）：可从多个来源绑定，如查询/表单字段或 JSON 请求体。
```
public IActionResult Create([FromBody] Product product)

## 73. 自定义模型绑定器

当默认绑定无法满足需求（如自定义格式、请求头等）时，可使用**自定义模型绑定器**。

public class CustomBinder : IModelBinder {
public Task BindModelAsync(ModelBindingContext context) {
// 自定义逻辑
}
}

通过 [ModelBinder] 特性或在 Startup 中全局注册。

## 74. 多个绑定源特性（[FromBody]、[FromQuery] 等）

一个 action 方法**不能绑定多个 [FromBody] 参数**。

常见绑定源：

```
● [FromBody] — 用于 JSON/XML 请求体
```
```
● [FromQuery] — 查询字符串
```
```
● [FromRoute] — 路由参数
```
```
● [FromForm] — 表单字段和文件上传
```
```
● [FromHeader] — HTTP 请求头
```
## 75. 模型验证：数据注解

为模型添加特性：

public class User {
[Required]
[StringLength(50)]
[EmailAddress]
public string Email { get; set; }
}

在 **MVC 和 API** 中均可用于验证。

## 76. 服务端验证与客户端验证（非侵入式）

```
● 服务端：始终在服务器端执行，使用 ModelState.IsValid。
```
```
● 客户端：HTML5 + jQuery 非侵入式验证（用于 MVC/Razor Pages）。
```
客户端验证**提升用户体验**，但服务端验证**对安全至关重要**。

# https://www.linkedin.com/in/sandeeppal

## 77. 自定义验证特性

继承 ValidationAttribute 创建自定义规则：

public class MustBeEvenAttribute : ValidationAttribute {
public override bool IsValid(object value) {
return (int)value % 2 == 0;
}
}

使用方式：

[MustBeEven]
public int Number { get; set; }

## 78. IValidatableObject 接口

用于模型内部**跨字段验证**：

public class Product : IValidatableObject {
public string Name { get; set; }
public decimal Price { get; set; }

public IEnumerable<ValidationResult> Validate(ValidationContext context) {
if (Price < 0) {
yield return new ValidationResult("价格必须为正数");
}
}
}

## 79. 使用 FluentValidation

```
● 安装 FluentValidation.AspNetCore
```
```
● 创建验证器类：
```
public class UserValidator : AbstractValidator<User> {
public UserValidator() {
RuleFor(x => x.Email).NotEmpty().EmailAddress();
}
}

注册方式：

services.AddFluentValidationAutoValidation();

✅ 比数据注解**更易读、更易测试**。

## 80. API 与 MVC 的验证（错误响应格式）

```
● [ApiController] 自动处理验证：
```
```
○ 如果 ModelState 无效，返回 400 BadRequest 和 ValidationProblemDetails。
```
```
● MVC（无 [ApiController]）需手动检查 ModelState.IsValid。
```
if (!ModelState.IsValid) return View(model);

## 81. 模型状态：检查 ModelState.IsValid

用于**检查绑定模型是否通过验证**：

# https://www.linkedin.com/in/sandeeppal

if (!ModelState.IsValid) {
return BadRequest(ModelState);
}

MVC 会根据验证特性自动将错误添加到 ModelState。

## 82. 绑定嵌套对象和集合

ASP.NET Core 支持**绑定嵌套属性**：

public class Order {
public Customer Customer { get; set; }
public List<Product> Products { get; set; }
}

只要属性名匹配，JSON 或表单数据都能无缝绑定。

## 83. 处理缺失或无效数据

```
● 对非空字段使用 [Required]。
```
```
● 可选值使用可空类型。
```
```
● 使用 ModelState 报告和处理缺失/无效字段。
```
```
● 如有需要，返回自定义错误信息。
```
## 84. 输入数据的清理

模型绑定**不会清理输入数据**，只绑定原始数据。

🛡 防止攻击（XSS、注入），需清理：

# https://www.linkedin.com/in/sandeeppal

```
● 字符串：输出时 HTML 编码（@Html.Encode）
```
```
● 使用前手动清理输入
```
```
● 上传文件使用杀毒/恶意软件扫描
```
## 85. 文件绑定（IFormFile）

用于表单**文件上传**（不能用 [FromBody]）：

public IActionResult Upload(IFormFile file)
{
var path = Path.Combine("uploads", file.FileName);
using var stream = new FileStream(path, FileMode.Create);
file.CopyTo(stream);
}

📝 多文件上传：

List<IFormFile> files
