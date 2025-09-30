# 安装与体验 Windows Terminal Canary

**Windows Terminal Canary** 是 Windows Terminal 的 **每夜构建版本**。  
它包含来自主分支的最新代码，让你能够 **第一时间体验即将发布的新功能**，在进入 **Windows Terminal Preview** 之前抢先试用。  

⚠️ **注意**：Canary 版本是最不稳定的发行渠道，可能会存在尚未修复的错误和兼容性问题，仅推荐尝鲜用户和开发者使用。  

---

## 分发形式

Windows Terminal Canary 提供两种分发方式，适配不同用户需求：

### 1. App Installer 分发版
- 支持 **自动更新**  
- 仅支持 **Windows 11**（受平台限制）  
- 提供架构：`x64`、`arm64`、`x86`  

### 2. Portable ZIP 分发版
- **便携应用**，无需安装  
- 不会自动更新，也不会自动检查更新  
- 支持 **Windows 10 (19041+) 和 Windows 11**  
- 提供架构：`x64`、`arm64`、`x86`  


---

## 下载地址

👉 [从 GitHub 获取 Windows Terminal Canary](https://github.com/microsoft/terminal?tab=readme-ov-file#installing-windows-terminal-canary)





---

## 配置 GitHub Copilot in Windows Terminal Canary

借助 **GitHub Copilot**，你可以在 Windows Terminal Canary 中通过聊天来执行命令，让命令行操作更自然、更智能。以下是基本步骤：

### 1. 打开 Windows Terminal Canary Settings
在终端顶部菜单中找到 **Settings（设置）**。



### 2. 找到 Terminal Chat (Experimental)
在设置中找到 **Terminal Chat（实验性功能）**。这是 Canary 版本独有的新特性。 


### 3. 使用 GitHub 账号进行认证
点击 **Authenticate via GitHub**，并授权 Copilot 访问。



### 4. 通过聊天驱动命令行
完成配置后，你可以直接在终端中输入自然语言，与 Copilot 聊天来执行命令，例如：  

```bash
# 普通命令
list files in this folder  

# Copilot 会自动转换成实际命令：
ls
```

## 总结

- **Canary 版本**：功能最新，风险最高。
- **安装方式**：App Installer（自动更新，仅支持 Win11） / Portable ZIP（便携，支持 Win10/11）。
- **Copilot 集成**：通过聊天驱动命令行，带来更自然的命令行体验。

如果你是开发者、尝鲜用户，或希望第一时间体验 Windows Terminal 的最新实验特性，Canary 版本非常适合你。

