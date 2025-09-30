
ASP.NET Core 面试题精讲

## ASP.NET Core 中间件管道

## 1.ASP.NET Core 中的中间件是什么？

**中间件是 HTTP 请求管道中的一个组件，可以：**
- 处理请求，
- 将请求传递给下一个中间件，
- 或者中断管道（短路）。
- 中间件可以在下一个中间件执行前后执行操作，如日志、认证、错误处理等。
- 中间件按照在 Program.cs 中添加的顺序执行。

---

## 2. 如何配置中间件管道（在 Program.cs / Startup.cs 中）？

- 在 ASP.NET Core 6+（最小主机模型）中，中间件在 Program.cs 中添加：

    ```csharp
    var builder = WebApplication.CreateBuilder(args);
    var app = builder.Build();
    app.UseMiddleware<YourMiddleware>();
    app.UseRouting();
    app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    app.Run();
    ```
- 在旧版本（例如 .NET Core 3.1）中，使用 Startup.cs：

    ```csharp
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
            app.UseMiddleware<YourMiddleware>();
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
    ```
---

## 3. app.Use、app.UseMiddleware、app.Run 和 app.Map 的区别**

| 方法 | 说明 |
|------|------|
| app.Use | 添加可以调用 next 的中间件 |
| app.UseMiddleware<T>() | 添加自定义中间件类 |
| app.Run | 终止中间件，不调用 next，结束管道 |
| app.Map | 根据 URL 路径分支管道（如 /api） |

**示例：**
```csharp
app.Use(async (context, next) => {
        await next(); // 调用下一个中间件
});
app.Run(async context => {
        await context.Response.WriteAsync("Hello World"); // 终止管道
});
```
---

## 4. 如何编写自定义中间件？**

- 创建一个类，构造函数接收 RequestDelegate，包含 Invoke 或 InvokeAsync 方法。

```csharp
public class MyCustomMiddleware
{
        private readonly RequestDelegate _next;
        public MyCustomMiddleware(RequestDelegate next) => _next = next;
        public async Task InvokeAsync(HttpContext context)
        {
                // 前置逻辑
                await _next(context);
                // 后置逻辑
        }
}
```
- 注册方式：`app.UseMiddleware<MyCustomMiddleware>();`

---

## 5. 什么是 RequestDelegate？

- RequestDelegate 是一个委托，表示管道中的下一个中间件。

```csharp
public delegate Task RequestDelegate(HttpContext context);
```

- 在自定义中间件中用于传递控制权。

---

## 6. 中间件顺序为什么重要？

- 中间件按添加顺序执行，顺序会影响行为。
- 例如：`app.UseAuthentication()` 必须在授权之前。
- 日志、错误处理、安全中间件应放在管道前面。

---

## 7. 如何短路管道？

- 在中间件中不调用 `await next()` 即可短路。

```csharp
if (!context.User.Identity.IsAuthenticated)
{
        context.Response.StatusCode = 401;
        return; // 短路
}
await next(); // 仅认证通过时调用
```

---

## 8. UseStaticFiles 的作用及其在管道中的位置**

- `app.UseStaticFiles()` 用于从 wwwroot 提供静态文件。
- 必须在路由或 endpoints 之前添加，否则静态文件会被控制器逻辑处理。

---

## 9. 如何使用中间件处理异常（UseExceptionHandler, UseDeveloperExceptionPage）

- `UseDeveloperExceptionPage()`：开发环境显示详细错误。
- `UseExceptionHandler("/Error")`：生产环境自定义错误页面。

- 也可内联处理：
```csharp
app.UseExceptionHandler(errorApp => {
        errorApp.Run(async context => {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("An error occurred");
        });
});
```

---

## 10. 如何启用 HTTPS 重定向中间件

- `app.UseHttpsRedirection();` 将 HTTP 请求重定向到 HTTPS。
- 应在认证或路由之前添加。
- HTTPS 端口可在 launchSettings.json 或 appsettings.json 配置。

--

## 11. 如何使用自定义文件提供程序或选项（如缓存、目录浏览）提供静态文件？**

- 示例：

```csharp
app.UseStaticFiles(new StaticFileOptions
{
        FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "MyFiles")),
        RequestPath = "/Files",
        OnPrepareResponse = ctx => {
                ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=600");
        }
});
app.UseDirectoryBrowser(new DirectoryBrowserOptions
{
        FileProvider = new PhysicalFileProvider("path"),
        RequestPath = "/browse"
});
```
---

## 12. 终止中间件与非终止中间件的区别**

| 类型 | 说明 |
|------|------|
| 终止（Terminal） | 结束管道，不调用 next()，如 app.Run() |
| 非终止（Non-Terminal） | 调用 next()，允许后续中间件执行，如 app.Use() |

---

## 13. 如何集成认证/授权中间件**

- `app.UseAuthentication();` 验证用户身份
- `app.UseAuthorization();` 应用策略/角色
- 顺序：路由后，endpoints 前

---

## 14. 如何通过中间件启用和配置 CORS

- 配置服务：
```csharp
builder.Services.AddCors(options => {
        options.AddPolicy("MyPolicy", policy => {
                policy.WithOrigins("https://example.com")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
        });
});
```
- 使用中间件：`app.UseCors("MyPolicy");`，应在路由/endpoints 之前

---

## 15. 使用中间件进行日志或性能监控（如测量执行时间）**

- 示例自定义中间件：

```csharp
public class TimingMiddleware
{
        private readonly RequestDelegate _next;
        public TimingMiddleware(RequestDelegate next) => _next = next;
        public async Task InvokeAsync(HttpContext context)
        {
                var sw = Stopwatch.StartNew();
                await _next(context);
                sw.Stop();
                Console.WriteLine($"Request took {sw.ElapsedMilliseconds} ms");
        }
}
```
- 注册：`app.UseMiddleware<TimingMiddleware>();`



