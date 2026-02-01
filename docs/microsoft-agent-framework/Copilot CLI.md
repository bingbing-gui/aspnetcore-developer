## 什么是 GitHub Copilot CLI？

GitHub Copilot CLI 将 AI 驱动的编码辅助直接带到你的命令行，使你能够通过自然语言对话来构建、调试并理解代码。它由与 GitHub Copilot 编码代理相同的智能体执行框架驱动，在提供智能辅助的同时，仍与您的 GitHub 工作流保持深度集成。

---

## 使用 GitHub Copilot CLI 你可以做什么？

通过 GitHub Copilot CLI，你可以在本地以同步方式与一个理解你的代码和 GitHub 上下文的 AI 代理一起工作。

- **终端原生开发**：直接在命令行中与 Copilot 编码代理工作——无需切换上下文。
- **开箱即用的 GitHub 集成**：使用自然语言访问你的仓库、issue 和 pull request，并使用你现有的 GitHub 账户完成认证。
- **代理能力**：与一个 AI 协作者一起构建、编辑、调试和重构代码，它可以规划并执行复杂任务。
- **基于 MCP 的可扩展性**：利用这样一个事实：该编码代理默认随附 GitHub 的 MCP 服务器，并支持自定义 MCP 服务器以扩展能力。
- **完全控制**：在执行前预览每一个操作——没有你的明确批准，什么都不会发生。

---

## 安装 GitHub Copilot CLI

### Windows（使用 WinGet 安装）

```powershell
winget install GitHub.Copilot
```
### macOS / Linux（使用 Homebrew 安装）

```bash
brew install copilot-cli
```

### macOS / Linux / Windows（使用 npm 安装）

```bash
npm install -g @github/copilot
```

## 授权
你也可以使用启用了 “Copilot Requests” 权限的 细粒度个人访问令牌（fine-grained PAT） 进行身份验证。

1. 访问：[https://github.com/settings/personal-access-tokens/new](https://github.com/settings/personal-access-tokens/new)

2. 在 “Permissions（权限）” 下，点击 “add permissions（添加权限）”，并选择 “Copilot Requests”。


3. 生成你的 token。


4. 通过环境变量将该 token 添加到环境中：`GH_TOKEN` 或 `GITHUB_TOKEN`（按优先级顺序）。



## 设置本地环境变量

我们把刚才 github 上面生成的 key，设置到本地环境变量 `GITHUB_TOKEN` 里面。在命令行终端执行如下代码：

```powershell
[System.Environment]::SetEnvironmentVariable("GITHUB_TOKEN", "你刚才生成的key", "User")
```

## 验证

我们在Window Terminal中使用Copilot，打开Terminal，输入copilot命令
```bash
copilot
```
如下所示：



我们可以使用/model来切换模型


选择完模型之后，我们可以和他对话，我们问：
你可以为我们做什么。如下输出


## 总结：

我们介绍了Github Copilot CLI 是什么以及如何安装使用。但是前提你需要购买订阅在Github上，
否则你无法使用这个工具。能够够在命令行中使用AI助手，能够大大提高我们的工作效率。

