# TaktWise

[English](README.md) · [简体中文](README.zh-CN.md) · [日本語](README.ja-JP.md) · [繁體中文（香港）](README.zh-HK.md)

节拍智控 · 前后端分离企业平台（.NET 9 + Vue 3）。

> ⚠️ **重要声明**：本项目为 AI 智能生成（使用 Cursor AI 等 AI 辅助开发工具），代码由 AI 自动生成并优化。
>
> 🚫 **不接受任何 Issue**：由于本项目是 AI 生成项目，我们不接受任何形式的 Issue、Bug 报告或功能请求。如有需要，请 Fork 后自行修改。

---

## 项目概览

| 指标 | 约数 | 说明 |
|------|------|------|
| 领域实体 | ~320 | `backend/src/Takt.Domain/Entities/`（含 `[SugarTable]`） |
| API 控制器 | ~350 | `backend/src/Takt.WebApi/Controllers/` |
| 前端列表页 | ~300 | `frontend/src/views/**/index.vue` |
| 支持语言 | 4 | `zh-CN` / `zh-HK` / `en-US` / `ja-JP` |
| Cursor 规则 / Skill | 18 / 18 | `.cursor/rules/`、`.cursor/skills/`（00～17） |

覆盖 **身份与权限、基础平台、人力资源、物流制造、客服、财务会计、日常办公、工作流、统计日志与快速查询、代码生成** 等业务域，采用 DDD 分层 + 多租户分库 + 公司级数据隔离。

---

## 技术栈

### 后端

| 类别 | 选型 |
|------|------|
| 运行时 | .NET 9 |
| ORM | SqlSugar 5.x（业务库）+ EF Core（OpenIddict 认证库） |
| 数据库 | SQL Server（`Database:DbType = 1`） |
| DI | Autofac（应用服务 / 验证器自动扫描注册） |
| 认证 | OpenIddict 7.x（OAuth 2.0 / OIDC） |
| 验证 | FluentValidation |
| 日志 | Serilog（控制台 + 分级文件） |
| API 文档 | Scalar（开发环境 `/scalar`） |
| 实时通信 | SignalR（双 Hub） |
| 调度 | Quartz（同步任务、会议提醒等） |

### 前端

| 类别 | 选型 |
|------|------|
| 构建 | Vite 8（Rolldown） |
| 框架 | Vue 3 Composition API + TypeScript |
| UI | Ant Design Vue 4.x |
| 样式 | Tailwind CSS 4.x |
| 状态 / 路由 / 请求 | Pinia、vue-router 5、Axios |
| 国际化 | vue-i18n（静态 `locales/**` + 后端动态种子） |
| 工作流设计 | **主路径** `takt-flow-ant-designer`（AntFlow 风格树）；LogicFlow 为实验视图 |
| 表单设计 | `@form-create/ant-design-vue` + antd-designer |
| 富文本 | `@umoteam/editor` |
| 图表 | ECharts 6 |
| 实时通信 | `@microsoft/signalr` |
| PWA | vite-plugin-pwa（可由 `VITE_PWA_ENABLED` 开关） |

**生产构建产物目录**（`frontend/dist`）：

```text
assets/js/{业务领域}/     # 入口与分包 chunk
assets/css/{业务领域}/    # 样式（与 views 领域对齐）
assets/img/{业务领域}/    # 图片
assets/other/{业务领域}/  # 无扩展名或未识别类型
```

业务领域取自 `src/views|api|locales|types` 首段；三方依赖 → `vendor`；公共组件 → `shared`；入口 → `app`。配置见 `frontend/vite.config.ts`。

---

## 架构分层

```
Takt.WebApi          → 控制器、Program.cs、OpenIddict、中间件
Takt.Application     → DTO、应用服务、FluentValidation 验证器
Takt.Infrastructure  → 仓储、种子、SignalR、缓存、多库映射、Quartz
Takt.Domain          → 实体、仓储接口
Takt.Shared          → 分页、异常、Options、工具类、枚举/常量
```

依赖方向：`WebApi → Application + Infrastructure + Shared`；`Application → Domain + Shared`；`Domain → Shared`。控制器仅注入 `ITaktXxxService`，禁止直接访问 SqlSugar。

**CRUD 表形态**（均须先满足 `12-crud`）：

| 形态 | 规则 | 说明 |
|------|------|------|
| 单表 | `12-crud` + `13/14` | 标准 QueryBar / ToolsBar / 表格 / 分页 / 弹窗 |
| 主子表 | + `10-master-detail` | OneToMany 级联 Fill/Save；展开行 / 抽屉 |
| 树表 | + `11-tree-table` | `ParentId` 懒加载一层；左树右表 |

---

## 平台能力

### 租户 → 公司 → 业务数据

```
租户 Tenant（按租户分库） → 公司 Company（公司代码） → 业务数据（部门/员工/制造/销售等）
```

| 层级 | 后端 | 前端 |
|------|------|------|
| **租户** | 独立业务库 `ConnectionStrings:Tenant_{code}`；`TaktTenant`；租户级实体基类 | `useTenantStore`、`takt-tenant-toggle`；登录前后均可选租户 |
| **公司** | `TaktCompany`；`TaktCompanyEntityBase` 等；仓储 `Where(TenantCode, CompanyCode)` | `takt-company-toggle`；与租户联动切换可访问公司 |
| **贯通** | `ITaktUserContext` + 请求头 `X-Tenant-Code` / `X-Company-Code` | `api/request.ts` 自动附加头 |

实体基类按「关联工厂 × 语言」四组合选型（见 `01-backend` / `TaktEntityBase`）；数据隔离仅用 **TenantCode + CompanyCode**，不用 CultureCode 作租户隔离。

**启动 Init 开关**（`appsettings.json` → `Init` / `Database`）：

1. `InitDb` — 按 `TenantCodes` 顺序建各租户业务库表
2. `SeedData` — 按 `TenantCodes` 顺序切换租户库执行全部种子；公司/工厂主档等按 `CompanyCodes` / `PlantCodes` 同序写入
3. `CompanyCodes` / `PlantCodes` / `CultureCodes` 列表顺序须与配置映射一致；`CompanyCodes` 首项仍作为演示账号主公司等默认归属

### 全局 SignalR

登录后维持 **双 Hub**（`AddTaktSignalR`，JWT 经 `TaktSignalRTokenMiddleware` 鉴权）：

| Hub | 路径 | 职责 |
|-----|------|------|
| **TaktConnectHub** | `/hubs/TaktConnectHub` | 连接/断开、在线用户、在线统计、强退 `ForceLogout` |
| **TaktNotificationHub** | `/hubs/TaktNotificationHub` | 私信、公司内广播、消息/在线统计推送 |

推送按 **公司 + 用户** 分组（`TaktSignalRGroupNames`）。调度入口：`ITaktSignalRDispatchService`。前端：`utils/taktSignalr.ts`、`stores/foundation/signalr.ts`。

### 审批工作流

自研 **AntFlow 风格树形 JSON 引擎**（非 BPMN）：

```
表单（FrmData） + 流程节点 + 审批人解析 + 条件网关 + 异常动作（驳回/撤回/转办/加签…）
```

| 落点 | 说明 |
|------|------|
| 引擎 | `TaktFlowEngineService` / `TaktFlowEngineController`（运行时） |
| 定义 CRUD | `TaktFlowScheme` / `TaktFlowForm` 等 |
| 前端设计器 | `components/business/TaktFlowAntDesigner/` |
| 规范 | `.cursor/rules/09-workflow.mdc` |

### 其他能力

| 能力 | 说明 | 配置 / 入口 |
|------|------|-------------|
| **权限（RBAC）** | 权限码 `领域:目录:…:实体:操作`（冒号）；`[TaktPermission]` 与菜单/前端四处一致 | `16-permission-i18n`、`Takt.WebApi/Filters/` |
| **翻译键** | I18nKey 点号分段：`menu.*` / `entity.*` / `common.page.*` | 后端种子 + `mergeDynamicLocaleMessages` |
| **日志** | Serilog；`TaktLoginLog` / `TaktOperLog` / `TaktDeltaLog` | `TaktLogging`、`Serilog` |
| **本地化** | 库表 + I18n 种子；静态 `locales/**` 根键必须为 `page` | `Localization`、`02-frontend` §6.2 |
| **验证码** | `Slider` / `Behavior` | `Captcha`、`ITaktCaptchaService` |
| **缓存** | `ITaktCacheService`（Memory / Redis） | `Cache:Provider` |
| **安全** | 限流、CSRF、XSS、RSA 密码传输 | `Security`、`PasswordPolicy:Transport` |
| **字段长度** | Domain `SugarColumn` Length 专项表（物料/单据/工厂等） | `17-field-length` |
| **代码生成** | 实体 → DTO/服务/控制器/前端/i18n 流水线（单实体） | `scripts/gen/generate-all.cjs` |
| **分析扩展** | Trend / Stat / Explosion 等与 CRUD **独立** 服务+控制器+前端 API，避免生成覆盖 | 见 `generate-entity-exclusions` 注释 |

前端：`v-permission`、`takt-captcha-*`、`takt-modal`、标准 CRUD 壳（`13-vue-view` / `14-vue-form`）。

---

## 业务模块

后端 `Controllers/` 与 `Domain/Entities/` 按相同领域划分；前端 `views/`、`api/`、`types/`、`locales/` 与之对齐。

| 领域 | 子模块 | 主要能力 |
|------|--------|----------|
| **Identity** | 用户、角色、菜单、租户、RBAC、认证 | 登录鉴权、权限分配、多租户/多公司授权 |
| **Foundation** | 字典、翻译、系统属性、编码、消息、在线、文化、行政区划、文件 | 平台基础数据、站内消息、动态 i18n |
| **HumanResource** | Organization、Personnel、Attendance、Talent、Benefits、Compensation、Performance、Training | 组织、人事、考勤、薪酬福利、绩效培训 |
| **Logistics · Materials** | 物料、供应商、厂商、工厂等 | 物料主数据 |
| **Logistics · Procurement** | 请购 / 采购订单 / 采购价格等 | 采购业务 |
| **Logistics · Sales** | 客户、销售订单 / 价格等 | 销售管理 |
| **Logistics · CustomerService** | 请求 / 订单 / 工单 / 合同及 Stat | 客服与统计 |
| **Logistics · Quality** | Operation（IQC/IPQC/FQC）、Complaint、Cost；含独立 Trend | 检验、客诉、质量成本与趋势 |
| **Logistics · Manufacturing** | Bom（含 Explosion / 成本分析 Trend）、Aps、Mds/Mps/Mrp、Output、Defect、EngineeringChange、LaborHour、Sop | 制造、计划、产出、不良、ECN |
| **Logistics · Serial** | 产品序列号入出库 | 序列号追溯 |
| **Logistics · Maintenance** | 设备、保养工单 | 设备维护 |
| **Accounting** | Financial、Controlling | 财务 / 成本主档 |
| **Routine** | Announcement、NewsCenter、HelpDesk、DocumentCenter、**MeetingCenter**、VisitorCenter | 公告、新闻、服务台、文档、会议、访客 |
| **Workflow** | 方案 / 表单 / 实例 / 任务 / 变量 / 加签 + **Engine** | 审批定义与运行时 |
| **Statistics** | Logging、**QuickQuery**（可配置快速查询） | 审计日志、自助查询 |
| **Code** | Generator、Database（备份等） | 代码生成元数据、库维护 |

---

## 项目结构

```
TaktWise/
├── backend/
│   ├── TaktWise.slnx
│   └── src/
│       ├── Takt.Shared/           # 公共类型、Options、异常、Enums/Constants
│       ├── Takt.Domain/           # 实体、仓储接口（~320）
│       ├── Takt.Application/      # DTO、应用服务、验证器
│       ├── Takt.Infrastructure/   # 仓储、种子、SignalR、中间件、Quartz
│       └── Takt.WebApi/           # API 入口（Program.cs、Controllers ~350）
├── frontend/
│   ├── src/
│   │   ├── api/                   # 按后端模块划分 REST 客户端
│   │   ├── types/                 # 与 api 一一对应的 TS 类型（主键 string）
│   │   ├── views/                 # 页面（单表 / 主子 / 树表）
│   │   ├── components/            # common/ + business/（takt-modal、流程设计器等）
│   │   ├── stores/ / composables/ / bootstrap/ / config/
│   │   ├── locales/               # 静态 i18n（export default { page: … }）
│   │   ├── router/ / styles/ / utils/
│   ├── vite.config.ts             # 产物 assets/{js|css|img|other}/{领域}/
│   └── package.json
├── scripts/
│   ├── gen/                       # 代码生成流水线（generate-all 等，仅 .cjs）
│   └── sync/                      # 外部数据同步脚本
├── .cursor/
│   ├── rules/                     # 00-project … 17-field-length
│   └── skills/                    # 与规则同名的实现清单
├── LICENSE
├── README.md / README.zh-CN.md / README.ja-JP.md / README.zh-HK.md
```

---

## 编译与运行

### 环境要求

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) LTS（前端）
- SQL Server（连接字符串在本地 `appsettings*.json`，仓库仅提供 `appsettings.*.Example.json` 模板）

### 后端

```bash
dotnet restore backend/TaktWise.slnx
dotnet build backend/TaktWise.slnx -c Release
dotnet run --project backend/src/Takt.WebApi/Takt.WebApi.csproj
```

| 项 | 值 |
|----|-----|
| HTTP | `http://localhost:60070` |
| HTTPS | `https://localhost:60071` |
| API 文档 | `https://localhost:60071/scalar` |

首次启动前将 `backend/src/Takt.WebApi/appsettings.Example.json` 等 Example 文件复制为 `appsettings.json` / `appsettings.Development.json` / `appsettings.Production.json`（勿提交 Git），并填写 `ConnectionStrings`（OpenIddict + `Tenant_*`）。`Init:InitDb` / `Init:SeedData` 见本地 `appsettings.json`。

### 前端

```bash
cd frontend
cp env.example .env
cp env.development.example .env.development
cp env.production.example .env.production
npm install
npm run dev         # 开发（默认 https://localhost:60081）
npm run build       # 生产（vue-tsc + vite build）
npm run build:vite  # 仅 Vite 打包（跳过 typecheck）
```

（Windows 可用 `copy env.example .env` 等。`.env*` 已由 `.gitignore` 排除。）

| 项 | 说明 |
|----|------|
| 开发地址 | `.env.development` 中 `VITE_APP_ORIGIN` |
| API 代理 | `/api` → `VITE_API_PROXY_TARGET`（本机后端 HTTPS） |
| OAuth 回调 | `{VITE_APP_ORIGIN}/auth/callback`，须与后端 `OpenIddict:SpaRedirectUris` 一致 |
| PWA | `VITE_PWA_ENABLED`；超大 vendor 不预缓存，见 `vite.config.ts` workbox 配置 |

须与后端 `Cors`、`OpenIddict:SpaRedirectUris` 中的前端地址一致。

### 默认账号（种子）

种子用户（各租户）：`admin`（超级管理员）、`guest`、`demo`。初始密码见 `PasswordPolicy:DefaultPassword`（默认 `Takt@123456`）。生产环境务必修改。

---

## 代码生成

新增实体后，在仓库根目录按**单实体**执行（已禁用全量 `--all`）：

```bash
node scripts/gen/generate-all.cjs --Holiday
node scripts/gen/generate-all.cjs --CostCenter --dry-run
node scripts/gen/generate-from-backend.cjs --Holiday
node scripts/gen/generate-vue-all-from-api.cjs --CostCenter
```

流水线步骤（DTO → Validator → Service → Controller → 前端 api/types → i18n → Vue）以 `scripts/gen/generate-all.cjs` 内 `PIPELINE` 为准；约定见 `.cursor/rules/15-codegen.mdc`。

生成后须：编译后端、核对权限码四处一致、树表/排除实体 Vue 手工对齐、`*Trend` / `*Stat` / `*Explosion` 保持独立栈勿挂回 CRUD。

---

## 开发规范

仓库内 `.cursor/rules/`（00～17）与 `.cursor/skills/` 定义完整约定，摘要：

- **命名 / 权限 / i18n**：`00-project`、`16-permission-i18n`（Permission 冒号、I18nKey 点号）
- **后端 / 前端**：`01-backend`、`02-frontend`；CRUD 基线 `12-crud`
- **主子表 / 树表 / 工作流**：`10` / `11` / `09`
- **视图 / 表单**：`13-vue-view`、`14-vue-form`
- **溢出安全**：`06` / `07` / `08`（分页 + 虚拟列表 + 主键 string）
- **字段长度**：`17-field-length`
- **格式**：禁止「隔行空行」，见 `03-format-blank-lines.mdc`
- **脚本**：仅 `.cjs`；禁止用 PowerShell 做全库查找替换（`00-project` §6）

---

## 许可

[MIT License](LICENSE) · Copyright (c) 2026 Takt Technologies Co., Ltd.

**维护者**：TaktWise（Cursor AI 等）
