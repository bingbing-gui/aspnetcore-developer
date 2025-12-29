dotnet-platform/
│
├─ README.md
├─ CONTRIBUTING.md
├─ LICENSE
│
├─ docs/                                   # 文档主线（认知 / 原理 / 指南）
│   ├─ 00-dotnet-fundamentals/             # .NET 本体（根基）
│   │   ├─ overview.md
│   │   ├─ runtime-and-clr.md
│   │   ├─ csharp-basics.md
│   │   ├─ project-structure.md
│   │   ├─ build-and-cli.md
│   │   └─ awesome-dotnet.md
│   │
│   ├─ 10-web/                             # Web 能力
│   │   ├─ aspnet-core/
│   │   │   ├─ hosting.md
│   │   │   ├─ middleware.md
│   │   │   ├─ routing.md
│   │   │   ├─ dependency-injection.md
│   │   │   ├─ auth-and-security.md
│   │   │   └─ webapi-best-practices.md
│   │   │
│   │   ├─ mvc.md
│   │   └─ minimal-api.md
│   │
│   ├─ 20-console-and-worker/              # Console / Worker
│   │   ├─ console-app.md
│   │   ├─ worker-service.md
│   │   └─ background-jobs.md
│   │
│   ├─ 30-desktop/                         # 桌面（预留）
│   │   ├─ wpf.md
│   │   └─ maui.md
│   │
│   ├─ 40-cloud/                           # 云 & 部署
│   │   ├─ deployment-overview.md
│   │   ├─ containerization.md
│   │   └─ azure/
│   │       ├─ azure-identity.md
│   │       ├─ azure-storage.md
│   │       └─ azure-app-service.md
│   │
│   ├─ 50-ai/                              # AI / Agent
│   │   ├─ ai-fundamentals/
│   │   ├─ semantic-kernel/
│   │   ├─ agent-framework/
│   │   ├─ mcp-integration.md
│   │   └─ third-party.md                  # 第三方 AI 项目说明（只写文档）
│   │
│   ├─ 90-roadmap.md                       # 学习路径
│   └─ SUMMARY.md                          # 文档索引
│
├─ src/                                    # 工程主线（可运行代码）
│   ├─ web/
│   │   ├─ WebApi.Basic/
│   │   ├─ WebApi.Auth/
│   │   ├─ WebApi.Advanced/
│   │   └─ WebApi.CloudReady/
│   │
│   ├─ console/
│   │   ├─ Console.Basic/
│   │   ├─ Console.Tools/
│   │   └─ Console.BatchJobs/
│   │
│   ├─ worker/
│   │   ├─ Worker.Jobs/
│   │   └─ Worker.EventConsumer/
│   │
│   ├─ desktop/                            # 预留
│   │
│   ├─ cloud/
│   │   ├─ Azure.Identity/
│   │   ├─ Azure.Storage/
│   │   └─ Azure.Deployment/
│   │
│   ├─ ai/
│   │   ├─ Ai.Agent/
│   │   ├─ Ai.SemanticKernel/
│   │   ├─ Ai.McpTools/
│   │   └─ Ai.Agent.BasedOnThirdParty/
│   │
│   └─ shared/                             # 通用基础设施
│       ├─ Common/
│       ├─ Security/
│       ├─ Observability/
│       └─ Extensions/
│
├─ samples/                                # 示例 / 教学 / 可快速运行
│   ├─ webapi-hello-world/
│   ├─ console-hello-world/
│   ├─ worker-basic/
│   ├─ ai-agent-demo/
│   └─ third-party/
│       └─ mcp-server-demo/
│
├─ tools/                                  # 工具 / 脚本 / 自动化
│   ├─ scripts/
│   │   ├─ build.ps1
│   │   ├─ format.ps1
│   │   └─ clean.sh
│   │
│   ├─ generators/
│   │   └─ project-scaffold/
│   │
│   ├─ dev-env/
│   │   ├─ docker-compose.yml
│   │   └─ devcontainer.json
│   │
│   └─ third-party/
│       └─ openapi-generator/
│
└─ roadmap/                                # 长期规划（可选）
    ├─ learning-path.md
    └─ milestones.md
