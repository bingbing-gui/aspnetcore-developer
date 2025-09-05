AspNetCore Skill
==============================
这个仓库是学习 ASP.NET Core 的宝库，采用最新的 .NET 8 版本，涵盖了从 ASP.NET Identity 到 Entity Framework Core 的所有核心知识点。这里不仅有丰富的学习资料和代码示例，还有许多优秀的第三方开源库，帮助你深入掌握 ASP.NET Core。

| 一级目录                | 子目录项                                     | 子目录项数量         |
|------------------------|----------------------------------------------|----------------------|
| 01-Basics              | Authentication                              | 2                    |
| ↑                      | Configuration                               | 1                    |
| ↑                      | DI                                           | 1                    |
| ↑                      | GlobalizationLocalization                   | 3                    |
| ↑                      | HttpClient                                  | 3                    |
| ↑                      | Identity                                    | 1                    |
| ↑                      | IdentityEndpoint                            | 1                    |
| ↑                      | ModelBinding                                | 2                    |
| ↑                      | Options                                     | 1                    |
| ↑                      | Routing                                     | 5                    |
| 02-WebAPI              | Controllers                                 | 3                    |
| ↑                      | OpenAPI                                     | 2                    |
| 03-MVC                 | Actions                                     | 1                    |
| ↑                      | Controllers                                 | 1                    |
| ↑                      | Cookies                                     | 1                    |
| ↑                      | CORS                                        | 1                    |
| ↑                      | Filters                                     | 1                    |
| ↑                      | ModelValidation                             | 1                    |
| ↑                      | OpenRedirectAttacks                         | 1                    |
| ↑                      | TagHelpers                                  | 3                    |
| ↑                      | Views                                       | 1                    |
| ↑                      | XSRF                                        | 1                    |
| ↑                      | XSS                                         | 1                    |
| 04-RazorPages          |                                            |                      |
| 05-Blazor          |                                            |                      |
| 06-DataAccess          |                                            |                      |
| 07-Communications      |                                            |                      |
| 08-Deployment         |                                            |                      |
| 09-AI-Agent          |                                            |                      |
| 10-Third-Party            | AspNetCore-Integrated-Azure-AI              | 1                    |
| ↑                      | aspnetcore-knowledge-point                  | 1                    |

# [Chapter 01](https://github.com/bingbing-gui/aspnetcore-skill/tree/master/src/Chapter01)
**项目描述**：ASP.NET Core Identity 是一个用于 ASP.NET Core 应用程序的身份验证和授权系统。它提供了一整套用于管理用户帐户、角色和权限的 API 和服务，允许开发者轻松地实现用户注册、登录、角色管理等功能。以下是对 ASP.NET Core Identity 的简单描述：

> **主要特性**
> - **用户管理**：支持用户注册、登录、注销、密码重置等基本用户操作。
> - **角色管理**：支持定义角色，并将用户分配到不同的角色，以实现基于角色的访问控制。
> - **认证和授权**：支持多种认证方式（如密码、OAuth、OpenID Connect 等）以及基于角色和声明的授权。
> - **安全特性**：内置支持密码哈希、账户锁定、双因素认证等安全功能。
> - **可扩展性**：通过接口和服务的方式设计，易于扩展和定制，可以集成到各种数据存储（如 SQL Server、MySQL、MongoDB 等）中。

> **基本组件**
> - **UserManager**：处理用户相关的操作，如创建用户、验证用户密码、获取用户信息等。
> - **RoleManager**：处理角色相关的操作，如创建角色、删除角色、分配角色等。
> - **SignInManager**：处理用户登录、登出和锁定等操作。
> - **IdentityUser**：默认的用户实体类，包含用户的基本信息，如用户名、密码哈希、电子邮件等。
> - **IdentityRole**：默认的角色实体类，包含角色的基本信息，如角色名称。

**代码示例**：[Asp.Net Core Identity](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter01)

**文章链接**
- [ASP.NET Core Identity 配置](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486141&idx=1&sn=f77635080994c6295cb801e846427a15&chksm=9f005597a877dc816f5fe96bbe0f9ef0ac4d82ef4e6148a4325764ed0dcee75cdad9a49c428e&token=1757261675&lang=zh_CN&scene=21#wechat_redirect)
- [ASP.NET Core Identity 用户管理](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486148&idx=1&sn=dae55b414e123c6718e470c21c8c8c21&chksm=9f0055eea877dcf876b2eff0e9fbe3a3f5b66271e4efe2639b7a2caf12ea722e123980e53f9a&scene=21&token=1757261675&lang=zh_CN#wechat_redirect)
- [ASP.NET Core Identity中用户名、密码、邮件策略](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486177&idx=1&sn=44c5277b6882deb798db1944d4163e04&chksm=9f0055cba877dcdd3365625bbc08c897e653cdda085a739e86181e75f1b9ede86d7b3fd1f4dd&token=1757261675&lang=zh_CN&scene=21#wechat_redirect)
- [ASP.NET Core Identity身份认证](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486183&idx=1&sn=baeb28f24399a9b0203f33185e1399a6&chksm=9f0055cda877dcdb80b411805d45d4f0f356dca2351d08209cac5aa0ac670fbc391a91dccb2b&token=1757261675&lang=zh_CN&scene=21#wechat_redirect)
- [ASP.NET Core Identity角色管理](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486194&idx=1&sn=a213c72dd0564c31a7624c6d99f0d277&chksm=9f0055d8a877dcce3f041eba5e28b3ebe8020fa92979699b2f3cbf2a9a4e16206fa340261c37&token=1757261675&lang=zh_CN&scene=21#wechat_redirect)
- [ASP.NET Core Identity客户自定义属性](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486202&idx=1&sn=7ec06927330a57a8a78143d88f10f987&chksm=9f0055d0a877dcc62b307d4453943c0cc165b1d9234c3d073a832deb235151970d79f64eef8a&token=1757261675&lang=zh_CN&scene=21#wechat_redirect)
- [ASP.NET Core  Identity如何使用Claim](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486208&idx=1&sn=846fd71c89106c6c8779d48987156e08&chksm=9f00542aa877dd3cd27d54dd26a92c9dc42ae23c4dcf01e84a8175f31c16847e2dae408ed118&token=1757261675&lang=zh_CN&scene=21#wechat_redirect)
- [ASP.NET Core  Identity如何使用Policy](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486215&idx=1&sn=9bd90b0c1d2d5583b8da324cbb56c5a6&chksm=9f00542da877dd3be47107268d4def7a92c1bea4b7568a9b4dbb9788754d0942bc378daefc38&token=1757261675&lang=zh_CN&scene=21#wechat_redirect)
- [ASP.NET Core Identity 2FA 认证](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486232&idx=1&sn=05688acdeb382499c36a7ce054da2565&chksm=9f005432a877dd2444657c506e98dfc7683934a7a699f811cb6c8268cc018693db0e9ef061a2&token=1757261675&lang=zh_CN&scene=21#wechat_redirect)
- [ASP.NET Core Identity 如何执行用户的电子邮件确认](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486233&idx=1&sn=70654867aaecbcf62c8b918a0fe7253b&chksm=9f005433a877dd25f010752f766b94a0801212f04db84f093fa7a9ed48f62925dfd1599d94d7&token=1757261675&lang=zh_CN&scene=21&poc_token=HINgdWajvh5dE70RFqobJbUTHjpyz-TBnv7oNgsG)
- [ASP.NET Core Identity 重置密码功能](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486238&idx=1&sn=32fae2f0fa6c50982e160c892c1314bc&chksm=9f005434a877dd220737e9f85ad05045c67084765190c6a91e7f724628024848409b6238203a&token=1650340614&lang=zh_CN&scene=21#wechat_redirect)
- [ASP.NET Core Identity Lockout](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486301&idx=1&sn=be1c52b47c25f41121f2866c23e63a0e&chksm=9f005477a877dd611facc1f9c2d3aa202f97ef03c6bd4421a33942c7ffb723a34d6252069ca5&token=438614949&lang=zh_CN&scene=21#wechat_redirect)
# [Chapter 02](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter02)

**项目描述**：Asp.Net Core Identity API 终结点

**代码示例**：[Asp.Net Core Identity API endpoints](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter02)

**文章链接**：[Asp.Net Core Identity API 终结点](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486916&idx=1&sn=f1ce92a76d20abb7888eca0b39bbbfe7&chksm=9f0052eea877dbf878b21b25219f4d5ddb82cfc3fff811dfed7c5144ad45b872cb1c1168460f&token=2146175377&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 03](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter03)

**项目描述**：AspNet Core 配置系统

**代码示例**：[AspNet Core Configuration](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter03) 

**文章链接**
- [ASP.NET Core 配置系列一](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486815&idx=1&sn=1a4f471b2a8431d771c11d511e8efc5c&chksm=9f005275a877db63515a5304c166b561d1ac476d7d6980c2c26e192d60cdafecd1ba322196c1&token=2146175377&lang=zh_CN#rd)
- [ASP.NET Core 配置系列二](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486833&idx=1&sn=a4de2c6202d559e9968f7170ede99cea&chksm=9f00525ba877db4df4a50a50590103760fd2e4603969b2b2ecfb7a54b414bfba05eecefe1af0&token=2146175377&lang=zh_CN#rd)
- [ASP.NET Core 配置系列三](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486845&idx=1&sn=a9b153b5cff58c5732d51d439bcc5952&chksm=9f005257a877db41dc77a468a9f203deea091969ada466ff4b2730f6066fbaa094484f0d3a8d&token=2146175377&lang=zh_CN#rd)
- [ASP.NET Core 配置系列四](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486847&idx=1&sn=81c1ba7182a6aab6c8958e211b5a9dcf&chksm=9f005255a877db43953e2215a262faedc31c7c9d9083950429a88e5f6a912d7f817a87ab4717&token=2146175377&lang=zh_CN#rd)
- [ASP.NET Core 配置系列五](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486858&idx=1&sn=e722b863483710384138d6e885b7c95e&chksm=9f0052a0a877dbb62fd8057492b092de70c3516fb0a7b5135cfdd5d7e4b26c91af36930c243d&token=2146175377&lang=zh_CN#rd)
# [Chapter 04](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter04)

**项目描述**：AspNet Core Options模式

**代码示例**：[AspNet Core Options Pattern](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter04)

**文章链接**：[ASP.NET Core中Options模式](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247487805&idx=1&sn=5792825947dd874cc53d188c060848c0&chksm=9f004e17a877c701a325fc9b4444a9beb95c11b4c14f967b94be302e8db56fff20ca41484e33&token=1468182562&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 05](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter05)

**项目描述**：在Asp.Net Core中使用依赖注入

**代码示例**：[AspNet Core DI](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter05)

**文章链接**
- [ASP.NET Core 依赖注入系列一](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486869&idx=1&sn=26ff00ea2ee847d23329958268cf9c66&chksm=9f0052bfa877dba97d4f16d813080159a645162d8c42b501a2ae71e721578d5f7c069a71ea0d&token=2146175377&lang=zh_CN#rd)
- [ASP.NET Core 依赖注入系列二](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486889&idx=1&sn=f37abf5047811527b8fbe74dabea45c8&chksm=9f005283a877db95d4a81909348c2c2b4a4ee9095b5524d93ad5b4dd35545c7e9a202a57fd59&token=2146175377&lang=zh_CN#rd)
- [ASP.NET Core 依赖注入系列三](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486896&idx=1&sn=b91b23179f4343ba4baa15c5f8ba2d99&chksm=9f00529aa877db8c8c9803bd7c6fa8913b012a422c312b4d08959e2f2c6e6c29368782d649cd&token=2146175377&lang=zh_CN&scene=21#wechat_redirect)
# [Chapter 06](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter06)

**项目描述**：Asp.Net Core 控制器

**代码示例**：[Asp.Net Core 控制器](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter06)

**文章链接**：[ASP.NET Core Controllers](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486901&idx=1&sn=fff8d045f2f5b06dcca07251f67cc034&chksm=9f00529fa877db8906cfaf50bfc1335bff05fff1eeae61d5c02582cd499d36b588187bf0c75d&token=2146175377&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 07](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter07)

**项目描述**：Asp.Net Core Actions

**代码示例**：[Asp.Net Core Actions](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter07)

**文章链接**：[ASP.NET Core Actions](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486916&idx=1&sn=f1ce92a76d20abb7888eca0b39bbbfe7&chksm=9f0052eea877dbf878b21b25219f4d5ddb82cfc3fff811dfed7c5144ad45b872cb1c1168460f&token=2146175377&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 08](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter08)

**项目描述**：Asp.Net Core 视图

**代码示例**：[AspNetCore.Views](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter08)

**文章链接**
- [ASP.NET Core Views系列一](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486961&idx=1&sn=af2c5824f8e56ec6c99e3a77a6a03d0f&chksm=9f0052dba877dbcd2aa41bc181bca8cfeccbb70b98d2b53ee494bc85b6762fa731c322b26c53&token=2146175377&lang=zh_CN&scene=21#wechat_redirect)
- [ASP.NET Core Views系列一](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486966&idx=1&sn=f790d2784b4c2fcd7e53fda17f851401&chksm=9f0052dca877dbca29e604f786d6eaf90fa498b130cb05e1b29763cf8ba61ec3b37fa1ea3ae2&token=2146175377&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 09](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter09)

**项目描述**：Asp.Net Core URL 路由

**代码示例**：[Asp.Net Core URLRouting](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter09)

**文章链接**：[ASP.NET Core 路由](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486984&idx=1&sn=999b344fe347e79bb93b298dc9e43217&chksm=9f005122a877d834057dcf752d1909f04a96cb06862fe486baea23026bcfbd72875c0cb37dca&token=2146175377&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 10](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter10)

**项目描述**：Asp.Net Core 路由约束

**代码示例**：[Asp.Net Core RouteConstraint](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter10)

**文章链接**：[ASP.NET Core 路由约束](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486990&idx=1&sn=feb21fd9aeb98d0e163520516d32ebd2&chksm=9f005124a877d8320c8a1f32d019c94a9c7ed57b8b3b1b21707bd83c8c6f93074f7e46187cc6&token=2146175377&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 11](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter11)

**项目描述**：Asp.Net Core Attribute路由

**代码示例**：[Asp.Net Core AttributeRoute](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter11)

**文章链接**：[ASP.NET Core Attribute 路由](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247487003&idx=1&sn=6205c6245822eb612ff31da7b2d02046&chksm=9f005131a877d82776c4948e0aefa2858c7f7dfeb46b274d5e8ee5f6e6fe65e4354679873621&token=2146175377&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 12](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter12)

**项目描述**：ASP.NET Core 路由生成外部链接

**代码示例**：[Asp.Net Core RouteLinks](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter12)

**文章链接**：[ASP.NET Core 路由生成外部链接](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247487004&idx=1&sn=5b187126022538c22df4493ecbad82fc&chksm=9f005136a877d8208b660272e137566718e83b1d3a82225cbadad1abe16dd1dccbf2b1412c0e&token=2146175377&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 13](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter13)

**项目描述**：Asp.Net Core Area

**代码示例**：[AspNetCore.Areas](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter13)

**文章链接**：[ASP.NET Core Areas](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247487005&idx=1&sn=479514da1cade0b523591ea324160a15&chksm=9f005137a877d821dc58f128ab8bed9c5a59345d3a7eff81344e4ed874c8fb11ea95a9951e38&token=2146175377&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 14](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter14)

**项目描述**：Asp.Net Core Tag Helpers 入门

**代码示例**：[Asp.Net Core TagHelpers](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter14)

**文章链接**：[Asp.Net Core Tag Helpers 入门](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247487026&idx=1&sn=f384a4d9aa77e3ce488b613b4616e228&chksm=9f005118a877d80ebd88c159b3852cbeae1c9fcfc14b414c7d2aac5a973205994e077dd7b62b&token=2146175377&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 15](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter15)

**项目描述**：ASP.NET Core 内置的Tag Helpers

**代码示例**：[Asp.Net Core BuiltInTagHelpers](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter15)

**文章链接**：[ASP.NET Core 内置的Tag Helpers](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247487064&idx=1&sn=3882bec53987f5e52e5877607f4cef0d&chksm=9f005172a877d864d3b53e83648e84b57da89ba4584a4a8cd88c7d4e1448ec20cea1f4a6c510&token=2146175377&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 16](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter16)

**项目描述**：ASP.NET Core 自定义Tag Helpers

**代码示例**：[Asp.Net Core CustomTagHelpers](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter16)

**文章链接**：[ASP.NET Core 自定义Tag Helpers](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247487076&idx=1&sn=2c1a4b79f13d7192f58bb75f4cb5cd30&chksm=9f00514ea877d8582a17815e942f4fe6c13857e87dcf43694dff961b68b34b179f2c1499552d&token=2146175377&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 17](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter17)

**项目描述**：ASP.NET Core模型绑定 - 基础篇

**代码示例**：[Asp.Net Core ModelBinding](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter17)

**文章链接**：[ASP.NET Core模型绑定 - 基础篇](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247487093&idx=1&sn=29067d940aa2e9567848de63d454bc4f&chksm=9f00515fa877d849815b3112fd3ba73917a12fa62ea3f229374f3d48d1791af2960252906023&token=2146175377&lang=zh_CN&scene=21#wechat_redirect)
  
# [Chapter 18](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter18)

**项目描述**：ASP.NET Core模型绑定 - 高级篇

**代码示例**：[Asp.Net Core ModelBinding Advanced](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter18)

**文章链接**：[ASP.NET Core模型绑定 - 高级篇](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247487130&idx=1&sn=aebf769dd1cd2ce2e4975539d037100b&chksm=9f0051b0a877d8a6b47ee847627a121a79a95f0fcf4071678d7fe510899b8f6946db70c9ea5d&token=2146175377&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 19](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter19)

**项目描述**：ASP.NET Core 模型验证

**代码示例**：[Asp.Net Core ModelValidation](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter19)

**文章链接**
- [ASP.NET Core 模型验证系列一](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247487169&idx=1&sn=811199ee67ba99e1edd38f2ca343182f&chksm=9f0051eba877d8fddc91cc2ce5f8eb2a82bb6b82bd87867e239434d1f7cf9d6ceb692fa5c598&token=2146175377&lang=zh_CN&scene=21#wechat_redirect)
- [ASP.NET Core 模型验证系列二](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247487171&idx=1&sn=8ab5881423081dce5d5c1df3f6040181&chksm=9f0051e9a877d8ff4f6feba723e79b76ce35e1dfbd011b1585331d4ebfecb2eeb70e84568ee5&token=2146175377&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 20](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter20)

**项目描述**：ASP.NET Core 过滤器

**代码示例**：[Asp.Net Core Filters](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter20)

**文章链接**
- [ASP.NET Core 过滤器](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247487185&idx=1&sn=14f52c9b455558855918483d4e1188bb&chksm=9f0051fba877d8ed34cb8b03cb1c54098e0fe6c7fa502f0cd8b9eda480530dca0cf902d352cd&token=2146175377&lang=zh_CN&scene=21#wechat_redirect)
- [ASP.NET Core 过滤器高级篇](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247487200&idx=1&sn=4ef08760d010c34dbfd96b485f0e6888&chksm=9f0051caa877d8dcbeefd5491948cf00ee341ba416fccd5ac4722b229303bba722aa8925bfa2&token=2146175377&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 21](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter21)

**项目描述**：ASP.NET Core 全球化和本地化

**代码示例**：[Asp.Net Core GlobalizationLocalization](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter21)

**文章链接**：[ASP.NET Core 全球化和本地化系列一](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247487256&idx=1&sn=2d94e80754c7d9fbee7c406a27128f7e&chksm=9f005032a877d9247e81f5af8f3d8b3028612edd6fe5eeaccf30da27bec1e527f168385a0623&token=2146175377&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 22](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter22)

**项目描述**：ASP.NET Core 全球化和本地化

**代码示例**：[Asp.Net Core GlobalLocalResFiles](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter22)

**文章链接**：[ASP.NET Core 全球化和本地化系列二](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247487271&idx=1&sn=0bdf2957d886b16c91b8e7e173346a45&chksm=9f00500da877d91b4fe30a1f52f11dd790472dd4def8957d5421c4b10d38bc506471ea7ab72d&token=2146175377&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 23](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter23)

**项目描述**：在ASP.NET Core 使用PO文件指定本地化

**代码示例**：[Asp.Net Core GlobalLocalPO](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter23)

**文章链接**：[在ASP.NET Core 使用PO文件指定本地化](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247487314&idx=1&sn=cf5796d612435b36a014d1ee7ce81e62&chksm=9f005078a877d96e38d28036566a92a155857269829bbcff635d75ddd958aa3f5a9a669e3ebd&token=2146175377&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 24](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter24)

**项目描述**：ASP.NET Core 启用CORS

**代码示例**：[Asp.Net Core CORS](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter24)

**文章链接**：[ASP.NET Core 启用CORS](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247487319&idx=1&sn=b3f2ee161acaef7223099c5286bd98f9&chksm=9f00507da877d96b77e5bc7ddeb7a1f5117928da5f5be140fe2e8d95f03ea77725e85735726a&token=2146175377&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 25](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter25)

**项目描述**：Asp.Net Core Cookie

**代码示例**：[Asp.Net Core Cookie](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter25)

**文章链接**：[ASP.NET Core Cookie 认证](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247487335&idx=1&sn=9a2b6e1dbdc3a9d34edf22632aaceb04&chksm=9f00504da877d95b13d9eec3b9a01cf45d956cba4b91749b74b5355b29ea7a68c7ecd177851a&token=2146175377&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 26](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter26)

**项目描述**：ASP.NET Core XSRF/CSRF攻击

**代码示例**：[Asp.Net Core XSRF](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter26)

**文章链接**：[ASP.NET Core XSRF/CSRF攻击](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247487341&idx=1&sn=1d8d46df360a53cc3b8c76e3eda405de&chksm=9f005047a877d951317336e88d478ca58ebdcc82583edc3711894ad586ca0fe1bc3dbb066f15&token=2146175377&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 27](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter27)

**项目描述**：ASP.NET Core 开放重定向攻击

**代码示例**：[Asp.Net Core OpenRedirectAttacks](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter27)

**文章链接**：[ASP.NET Core 开放重定向攻击](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247487345&idx=1&sn=b4b700289a2aff5b12603efab8a937ce&chksm=9f00505ba877d94d3d2eec522cc638e771b0c576741c10b9ba9c28e367e2e626d578835c8cab&token=2146175377&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 28](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter28)

**项目描述**：ASP.NET Core XSS攻击

**代码示例**：[Asp.Net Core XSS](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter28)

**文章链接**：[ASP.NET Core XSS攻击](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247487450&idx=1&sn=701a5d35e3d2e51b0f9dea374de77765&chksm=9f0050f0a877d9e692c6c5b7d5e983448f5d46925520de5132a17223325bb750e170bd487482&token=2146175377&lang=zh_CN&scene=21#wechat_redirect)


# [Chapter 29](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter29)

**项目描述**：ASP.NET Core XSS攻击

**代码示例**：[Asp.Net Core APIControllers](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter29)

**文章链接**
- [ASP.NET Core API Controllers 系列一](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247487467&idx=1&sn=28d1602053ce15f754af8cd94032758c&chksm=9f0050c1a877d9d77c37edd6862b3f7af444063329e6b3c07e113e775cff7cbdfdf92aff9070&token=2146175377&lang=zh_CN&scene=21#wechat_redirect)
- [ASP.NET Core API Controllers 系列二](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247487550&idx=1&sn=553f2aaa7303167cd0d96fe274ab45cf&chksm=9f004f14a877c6021612f55564bed68e8f730fd63f8589f7b278ad014757f06dfbab53fd126e&token=2146175377&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 30](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter30)

**项目描述**：ASP.NET Core API 返回类型

**代码示例**：[Asp.Net Core APIController ActionReturnTypes](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter30)

**文章链接**：[ASP.NET Core API 返回类型](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247487695&idx=1&sn=6fd98c58f0c7063a72eaeb3a1ef650fe&chksm=9f004fe5a877c6f343410008390be97d7a67dd2db265415304b0c6571fa2d394914d7bb077bc&token=1193472&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 31](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter31)

**项目描述**：ASP.NET Core 指定Action的响应类型

**代码示例**：[Asp.Net Core FormatResponseOutputData](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter31)

**文章链接**：[ASP.NET Core 指定Action的响应类型](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247487766&idx=1&sn=3df7d17a1a603702aae77ae1233aa9a1&chksm=9f004e3ca877c72af99503d682478b2dae6b079603953ac764d571adfbcf591d885f3907c98e&token=1193472&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 32](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter32)

**项目描述**：ASP.NET Core API Basic认证

**代码示例**：[AspNetCore API BasicAuthentication](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter32)

**文章链接**：[ASP.NET Core API Basic认证](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247487777&idx=1&sn=f64b4e7663dba23b241027e9ad777b97&chksm=9f004e0ba877c71d5e11b92a3c93dc81032f9c475c171de3c273d9cfb5e3f0fdbf2d0fb896ab&token=1468182562&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 33](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter33)

**项目描述**：ASP.NET Core API JWT认证

**代码示例**：[Asp.Net Core API JWT Authentication](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter33)

**文章链接**：
- [ASP.NET Core API Basic认证](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247487790&idx=1&sn=faf38bd3b0c64db5eb83b6ae6d0dfc28&chksm=9f004e04a877c712ea159eeaa902cbc1bf086a92e0f6fc30b017b689907716d816fba73cad51&token=1468182562&lang=zh_CN&scene=21#wechat_redirect)
- [ASP.NET Core API 刷新 access token](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247487803&idx=1&sn=669242339e308d701def8073776e5ddc&chksm=9f004e11a877c7077bfe4aef8bd3641f8cf8308c5d91cdea7f34dbe975e0e97fd50e71c81ee8&token=1468182562&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 34](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter34)

**项目描述**：ASP.NET Core HttpClient正确使用方法之系列一

**代码示例**：[Asp.Net Core HttpClient](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter34)

**文章链接**：[ASP.NET Core HttpClient正确使用方法之系列一](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247487588&idx=1&sn=4b621a443e862a4f82a319d5409c86b1&chksm=9f004f4ea877c658490f3678cf2ad9aa7bae24810328d57bf18039920f5111b93e8d9935fe86&token=2146175377&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 35](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter35)

**项目描述**：ASP.NET Core HttpClient使用http动词系列二

**代码示例**：[Asp.Net Core HttpClientWithHttpVerb](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter35)

**文章链接**：[ASP.NET Core HttpClient使用http动词系列二](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247487594&idx=1&sn=25307ab8b4fe0dd1590975bf77eb3aad&chksm=9f004f40a877c656aadd1e666ee6a347bcebeb57ae37b3a8b6bee2d9555940db07b67e5aab2e&token=2146175377&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 36](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter36)

**项目描述**：ASP.NET Core HttpClient组件拓展系列三

**代码示例**：[Asp.Net Core HttpClientHander](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter36)

**文章链接**：[ASP.NET Core HttpClient组件拓展系列三](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247487648&idx=1&sn=bb71d7ebec5ba3d6d09cf15062859d3f&chksm=9f004f8aa877c69ccd6ab7cf845ab7e4e9f86e393df6157a5a465158e6e5477cc58c31c0c240&token=2146175377&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 37](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter37)

**项目描述**：ASP.NET Core 使用Swagger/OpenAPI文档化API

**代码示例**：[Asp.Net Core Swashbuckle](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter37)

**文章链接**
- [ASP.NET Core 使用Swagger/OpenAPI文档化API](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247487665&idx=1&sn=bb6836e776d7bbc36e547703429a2fb6&chksm=9f004f9ba877c68db2e472d8578e48001363e13aa1f20a3bf29b8732641f4613e2fb9a7e4593&token=2146175377&lang=zh_CN&scene=21#wechat_redirect)
- [ASP.NET Core 中使用Swashbuckle](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247487666&idx=1&sn=71d91f7cbe59b809c7c7e7e054747985&chksm=9f004f98a877c68e42058c0f283c03ee45a6f303c65041bf182c8ee98239a75503039c0bc1fc&token=2146175377&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 38](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter38)

**项目描述**：ASP.NET Core 使用NSwag

**代码示例**：[Asp.Net Core NSwag](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter38)

**文章链接**：[ASP.NET Core 使用NSwag](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247487669&idx=1&sn=dd4569527a3a840c8a508936a64d235b&chksm=9f004f9fa877c689de806c882211353676f7e619bc0c532d35abc0c2ed0145dcef363b5c6ffb&token=2146175377&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 39](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter39)

**项目描述**：Entity Framework Core DbContext 

**代码示例**：[EFCoreDbContext ](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter38)

**文章链接**
- [Entity FrameworkCore介绍](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486352&idx=1&sn=7c6b71ba2d48c6a8572d4777154c0f06&chksm=9f0054baa877ddacd9054e90745ee5db786d35fc09e082a5ce5ee2f0125dd0860bab3493d753&token=1170622278&lang=zh_CN&scene=21#wechat_redirect)|
- [Entity Framework Core安装](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486360&idx=1&sn=aab93026b7506e9e8c1e19d9d099ed24&chksm=9f0054b2a877dda464ddf13c6129e3fa028778336ae08313e508c5fd7befce96125df7c76902&token=1170622278&lang=zh_CN#rd)
- [Entity Framework Core数据库优先](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486484&idx=1&sn=e0911921eac2c1c8f535829c137f35a3&chksm=9f00533ea877da280c08908716e802cd67f34fdcc5e55112a87f66f419437836c78b4e07bccc&token=1170622278&lang=zh_CN&scene=21#wechat_redirect)|
- [Entity Framework Core DbContext](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486561&idx=1&sn=3638a0ea6a034e1b1021a6a9e25b1fba&chksm=9f00534ba877da5d80c0884378079c2e1226c79fc13dafdd0145ca3a74901f1f008cf93054d8&token=1170622278&lang=zh_CN&scene=21#wechat_redirect)|

# [Chapter 40](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter40)

**项目描述**：Entity Framework Core DbContext 

**代码示例**：[EFCoreCodeFirst](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter40)

**文章链接**：[Entity Framework Core 代码优先](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486562&idx=1&sn=240686fb2d56093677a120816354dbec&chksm=9f005348a877da5eba68ea35875761088c31de6b0ef3a2082e39dfe925a6ede194e94ed6f765&token=1170622278&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 41](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter41)

**项目描述**：Entity Framework Core-Migrations

**代码示例**：[EFCoreMigration](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter41)

**文章链接**：
- [Entity Framework Core 代码优先](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486562&idx=1&sn=240686fb2d56093677a120816354dbec&chksm=9f005348a877da5eba68ea35875761088c31de6b0ef3a2082e39dfe925a6ede194e94ed6f765&token=1170622278&lang=zh_CN&scene=21#wechat_redirect)
- [Entity Framework Core-Migrations](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486566&idx=1&sn=526492ab605d4a57048eb91c75c2c65a&chksm=9f00534ca877da5a196cc1b3ff2fbc5f947baa5eb0b21f655dfd5aeb8d3e9fa86919d6150f67&token=1170622278&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 42](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter42)

**项目描述**：Entity Framework Core 插入数据

**代码示例**：[EFCoreInsertRecords](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter42)

**文章链接**：[Entity Framework Core 插入数据](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486636&idx=1&sn=1ed995d089324fc520d5e0f120e8f05e&chksm=9f005386a877da90c3c7a3c9818ae3d2f029b1e4fc473319af6f6ca58dc50b2db54289323905&token=1170622278&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 43](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter43)

**项目描述**：Entity Framework Core 读取数据

**代码示例**：[EFCoreInsertRecords](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter43)

**文章链接**：[Entity Framework Core 读取数据](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486663&idx=1&sn=8ccb4afc723bd79752a8ffe608c6e03c&chksm=9f0053eda877dafb81a94456c6519550345412f6f840ad5978715d0a072a69ae5a234e7069ed&token=1170622278&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 44](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter44)

**项目描述**：Entity Framework Core 更新数据

**代码示例**：[EFCoreUpdateRecords](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter44)

**文章链接**：[Entity Framework Core 更新数据](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486674&idx=1&sn=691160617db74f82bbd31ddda31a9333&chksm=9f0053f8a877daeed0713d3571442fcc65fe9577d355a27a3650033baf9c3246b8f55009ac6a&token=1170622278&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 45](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter45)

**项目描述**：Entity Framework Core 删除数据

**代码示例**：[EFCoreUpdateRecords](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter45)

**文章链接**：[Entity Framework Core 删除数据](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486676&idx=1&sn=7a0d10ab748af23dc1bc1ecb671d7194&chksm=9f0053fea877dae88694419c6f83a8dc6ca9a51fe8f567ed751f52a567d390b823ef88b8e3fd&token=1170622278&lang=zh_CN#rd)

# [Chapter 46](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter46)

**项目描述**：Entity Framework Core 约定

**代码示例**：[EFCoreConventions](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter46)

**文章链接**：[Entity Framework Core 约定](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486677&idx=1&sn=4e82e2c40bc45603e8588086489bf19c&chksm=9f0053ffa877dae90feafe62f9e3e5fccb85c564a3ebaa1914eb3bf8207fb44c703525361666&token=1170622278&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 47](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter47)

**项目描述**：Entity Framework Core 配置

**代码示例**：[EFCoreConfiguration](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter47)

**文章链接**：[Entity Framework Core 配置](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486678&idx=1&sn=c3fa83be65d505dde5dd9431e68cd57b&chksm=9f0053fca877daea1de89f77137a4b69abd0904246bf5eac8841b2adc789fcd9333f9d8c3581&token=1170622278&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 48](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter48)

**项目描述**：Entity Framework Core-使用Fluent API配置一对一关系

**代码示例**：[EFCoreFluentAPIOneToOne](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter48)

**文章链接**： 
- [Entity Framework Core-Fluent API](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486679&idx=1&sn=3314f5325411b0c5ba8a3ef81aa15922&chksm=9f0053fda877daeb6c21a92ec3bcc319eb0eb45f69814dc1d7081a821ef26bcfdf5b190e30fa&token=1170622278&lang=zh_CN&scene=21#wechat_redirect)
- [Entity Framework Core-使用Fluent API配置一对一关系](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486695&idx=1&sn=47c3631d10360417b63f606c918e8679&chksm=9f0053cda877dadb2f95dbbb9ee8d9ae2c10e83e01e697cdbd1a8cd12fa8d6aee558821c6a83&token=304031109&lang=zh_CN#rd)

# [Chapter 49](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter49)

**项目描述**：Entity Framework Core-使用Fluent API配置一对多关系

**代码示例**：[EFCoreFluentAPIOneToMany](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter48)

**文章链接**：[Entity Framework Core-使用Fluent API配置一对多关系](https://mp.weixin.qq.com/s/4gqWH93LdBe2_8KUU6oUKA)

# [Chapter 50](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter50)

**项目描述**：Entity Framework Core-使用Fluent API配置多对多关系

**代码示例**：[EFCoreFluentAPIManyToMany](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter50)

**文章链接**：[Entity Framework Core-使用Fluent API配置多对多关系](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486697&idx=1&sn=b96286d4a852a865e731d67c4df25fab&chksm=9f0053c3a877dad53f37e173109dd2291412e085e1b959949e95dbc159d49e94baa709ee180f&token=323179610&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 51](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter51)

**项目描述**：Entity Framework Core-使用FromSqlRaw() 执行原生SQL查询

**代码示例**：[EFCoreExecuteRawSql](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter51)

**文章链接**：[Entity Framework Core-使用FromSqlRaw() 执行原生SQL查询](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486699&idx=1&sn=48713b8379f12e180270a7e8c2c100df&chksm=9f0053c1a877dad7fdd7a50fd97946d0fa74368f93d35eba0b25339d177968bffc254d95a40d&token=323179610&lang=zh_CN&scene=21#wechat_redirect)

# [Chapter 52](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter52)

**项目描述**：Entity Framework Core执行存储过程

**代码示例**：[EFCoreExecuteStoredProcedures](https://github.com/bingbing-gui/AspNetCore-Skill/tree/master/src/Chapter52)

**文章链接**：[Entity Framework Core执行存储过程](https://mp.weixin.qq.com/s?__biz=MzA3NDM1MzIyMQ==&mid=2247486707&idx=1&sn=fb77e56489b8140a9a66d5d8c8018c29&chksm=9f0053d9a877dacfdb794538d7fc28f43c74b75bd5ddcfc13e91717054eebddcfd254c4351e2&token=323179610&lang=zh_CN&scene=21#wechat_redirect)




