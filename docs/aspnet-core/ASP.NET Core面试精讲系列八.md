## 116. 什么是过滤器（Filters）

过滤器是在 MVC 或 Razor Pages 管道中执行的组件，可在控制器方法执行的不同阶段插入逻辑。
类型包括：
- Authorization Filter：最先执行，用于身份验证与授权。
- Resource Filter：在模型绑定前运行，可控制缓存或资源访问。
- Action Filter：在 Action 执行前后执行。
- Exception Filter：处理 Action 中未捕获的异常。
- Result Filter：在结果生成前后执行。

---

## 117. Filters 与 Middleware 的区别

- 执行阶段
    - Middleware：全局 HTTP 管道
    - Filter：MVC/Razor 内部
- 作用范围
    - Middleware：所有请求
    - Filter：控制器 / Action
- 典型用途
    - Middleware：日志、认证、CORS
    - Filter：授权、验证、结果处理
- 可否中断
    - Middleware：可短路整个请求
    - Filter：可短路 MVC 执行

简言之：Middleware 处理全局逻辑，Filter 处理 MVC 层逻辑。

---

## 118. 执行顺序

1. Authorization Filters
2. Resource Filters
3. 模型绑定
4. Action Filters
5. Exception Filters
6. Result Filters

同类型过滤器可通过 Order 属性控制优先级。

---

## 119. 自定义过滤器

示例：

```csharp
public class CustomActionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context) { }
    public void OnActionExecuted(ActionExecutedContext context) { }
}
```

注册方式：

```csharp
services.AddControllers(options => options.Filters.Add<CustomActionFilter>());
```

或使用：
```csharp
[ServiceFilter(typeof(CustomActionFilter))]
```

---

## 120. 全局过滤器 vs 局部过滤器

- 全局：在 AddControllers 中注册，对所有控制器生效。
- 局部：在控制器或 Action 上标注，仅限局部使用。

全局适合日志/异常，局部适合业务逻辑控制。

---

## 121. Filter Context

过滤器可通过上下文访问：
- HttpContext 与请求信息
- Action 参数与返回结果
- 可提前终止或修改执行结果

---

## 122. 短路（Short-circuiting）

过滤器可提前返回结果，阻止后续执行：

```csharp
if (!IsAuthorized())
{
        context.Result = new UnauthorizedResult();
}
```

---

## 123. 特性过滤器 vs 服务过滤器

- 特性过滤器（Attribute）：简单直接，参数化方便。
- 服务过滤器（ServiceFilter/TypeFilter）：从 DI 容器解析，支持依赖注入。

---

## 124. Filters 与 Middleware 的结合

- Middleware：处理全局跨域、认证、日志。
- Filter：处理 MVC 层授权、验证、结果包装。

二者结合可实现全局到局部的精细化控制。