# TaktWise

[English](README.md) · [简体中文](README.zh-CN.md) · [日本語](README.ja-JP.md) · [繁體中文（香港）](README.zh-HK.md)

節拍智控 · 前後端分離企業平台（.NET 9 + Vue 3）。

> ⚠️ **重要聲明**：本項目為 AI 智能生成（使用 Cursor AI 等 AI 輔助開發工具），代碼由 AI 自動生成並優化。
>
> 🚫 **不接受任何 Issue**：由於本項目是 AI 生成項目，我們不接受任何形式的 Issue、Bug 報告或功能請求。如有需要，請 Fork 後自行修改。

---

## 項目概覽

| 指標 | 約數 | 說明 |
|------|------|------|
| 領域實體 | ~320 | `backend/src/Takt.Domain/Entities/`（含 `[SugarTable]`） |
| API 控制器 | ~350 | `backend/src/Takt.WebApi/Controllers/` |
| 前端列表頁 | ~300 | `frontend/src/views/**/index.vue` |
| 支援語言 | 4 | `zh-CN` / `zh-HK` / `en-US` / `ja-JP` |
| Cursor 規則 / Skill | 18 / 18 | `.cursor/rules/`、`.cursor/skills/`（00～17） |

覆蓋 **身份與權限、基礎平台、人力資源、物流製造、客服、財務會計、日常辦公、工作流、統計日誌與快速查詢、代碼生成** 等業務域，採用 DDD 分層 + 多租戶分庫 + 公司級數據隔離。

---

## 技術棧

### 後端

| 類別 | 選型 |
|------|------|
| 運行時 | .NET 9 |
| ORM | SqlSugar 5.x（業務庫）+ EF Core（OpenIddict 認證庫） |
| 資料庫 | SQL Server（`Database:DbType = 1`） |
| DI | Autofac（應用服務 / 驗證器自動掃描註冊） |
| 認證 | OpenIddict 7.x（OAuth 2.0 / OIDC） |
| 驗證 | FluentValidation |
| 日誌 | Serilog（控制台 + 分級檔案） |
| API 文件 | Scalar（開發環境 `/scalar`） |
| 實時通訊 | SignalR（雙 Hub） |
| 調度 | Quartz（同步任務、會議提醒等） |

### 前端

| 類別 | 選型 |
|------|------|
| 構建 | Vite 8（Rolldown） |
| 框架 | Vue 3 Composition API + TypeScript |
| UI | Ant Design Vue 4.x |
| 樣式 | Tailwind CSS 4.x |
| 狀態 / 路由 / 請求 | Pinia、vue-router 5、Axios |
| 國際化 | vue-i18n（靜態 `locales/**` + 後端動態種子） |
| 工作流設計 | **主路徑** `takt-flow-ant-designer`（AntFlow 風格樹）；LogicFlow 為實驗視圖 |
| 表單設計 | `@form-create/ant-design-vue` + antd-designer |
| 富文本 | `@umoteam/editor` |
| 圖表 | ECharts 6 |
| 實時通訊 | `@microsoft/signalr` |
| PWA | vite-plugin-pwa（可由 `VITE_PWA_ENABLED` 開關） |

**生產構建產物目錄**（`frontend/dist`）：

```text
assets/js/{業務領域}/     # 入口與分包 chunk
assets/css/{業務領域}/    # 樣式（與 views 領域對齊）
assets/img/{業務領域}/    # 圖片
assets/other/{業務領域}/  # 無擴展名或未識別類型
```

業務領域取自 `src/views|api|locales|types` 首段；三方依賴 → `vendor`；公共組件 → `shared`；入口 → `app`。配置見 `frontend/vite.config.ts`。

---

## 架構分層

```
Takt.WebApi          → 控制器、Program.cs、OpenIddict、中間件
Takt.Application     → DTO、應用服務、FluentValidation 驗證器
Takt.Infrastructure  → 倉儲、種子、SignalR、緩存、多庫映射、Quartz
Takt.Domain          → 實體、倉儲接口
Takt.Shared          → 分頁、異常、Options、工具類、枚舉/常量
```

依賴方向：`WebApi → Application + Infrastructure + Shared`；`Application → Domain + Shared`；`Domain → Shared`。控制器僅注入 `ITaktXxxService`，禁止直接訪問 SqlSugar。

**CRUD 表形態**（均須先滿足 `12-crud`）：

| 形態 | 規則 | 說明 |
|------|------|------|
| 單表 | `12-crud` + `13/14` | 標準 QueryBar / ToolsBar / 表格 / 分頁 / 彈窗 |
| 主子表 | + `10-master-detail` | OneToMany 級聯 Fill/Save；展開行 / 抽屜 |
| 樹表 | + `11-tree-table` | `ParentId` 懶加載一層；左樹右表 |

---

## 平台能力

### 租戶 → 公司 → 業務數據

```
租戶 Tenant（按租戶分庫） → 公司 Company（公司代碼） → 業務數據（部門/員工/製造/銷售等）
```

| 層級 | 後端 | 前端 |
|------|------|------|
| **租戶** | 獨立業務庫 `ConnectionStrings:Tenant_{code}`；`TaktTenant`；租戶級實體基類 | `useTenantStore`、`takt-tenant-toggle`；登錄前後均可選租戶 |
| **公司** | `TaktCompany`；`TaktCompanyEntityBase` 等；倉儲 `Where(TenantCode, CompanyCode)` | `takt-company-toggle`；與租戶聯動切換可訪問公司 |
| **貫通** | `ITaktUserContext` + 請求頭 `X-Tenant-Code` / `X-Company-Code` | `api/request.ts` 自動附加頭 |

實體基類按「關聯工廠 × 語言」四組合選型（見 `01-backend` / `TaktEntityBase`）；數據隔離僅用 **TenantCode + CompanyCode**，不用 CultureCode 作租戶隔離。

**啟動 Init 開關**（`appsettings.json` → `Init` / `Database`）：

1. `InitDb` — 按 `TenantCodes` 順序建各租戶業務庫表
2. `SeedData` — 按 `TenantCodes` 順序切換租戶庫執行全部種子；公司/工廠主檔等按 `CompanyCodes` / `PlantCodes` 同序寫入
3. `CompanyCodes` / `PlantCodes` / `CultureCodes` 列表順序須與配置映射一致；`CompanyCodes` 首項仍作為演示帳號主公司等默認歸屬

### 全局 SignalR

登錄後維持 **雙 Hub**（`AddTaktSignalR`，JWT 經 `TaktSignalRTokenMiddleware` 鑑權）：

| Hub | 路徑 | 職責 |
|-----|------|------|
| **TaktConnectHub** | `/hubs/TaktConnectHub` | 連接/斷開、在線用戶、在線統計、強退 `ForceLogout` |
| **TaktNotificationHub** | `/hubs/TaktNotificationHub` | 私信、公司內廣播、消息/在線統計推送 |

推送按 **公司 + 用戶** 分組（`TaktSignalRGroupNames`）。調度入口：`ITaktSignalRDispatchService`。前端：`utils/taktSignalr.ts`、`stores/foundation/signalr.ts`。

### 審批工作流

自研 **AntFlow 風格樹形 JSON 引擎**（非 BPMN）：

```
表單（FrmData） + 流程節點 + 審批人解析 + 條件網關 + 異常動作（駁回/撤回/轉辦/加簽…）
```

| 落點 | 說明 |
|------|------|
| 引擎 | `TaktFlowEngineService` / `TaktFlowEngineController`（運行時） |
| 定義 CRUD | `TaktFlowScheme` / `TaktFlowForm` 等 |
| 前端設計器 | `components/business/TaktFlowAntDesigner/` |
| 規範 | `.cursor/rules/09-workflow.mdc` |

### 其他能力

| 能力 | 說明 | 配置 / 入口 |
|------|------|-------------|
| **權限（RBAC）** | 權限碼 `領域:目錄:…:實體:操作`（冒號）；`[TaktPermission]` 與菜單/前端四處一致 | `16-permission-i18n`、`Takt.WebApi/Filters/` |
| **翻譯鍵** | I18nKey 點號分段：`menu.*` / `entity.*` / `common.page.*` | 後端種子 + `mergeDynamicLocaleMessages` |
| **日誌** | Serilog；`TaktLoginLog` / `TaktOperLog` / `TaktDeltaLog` | `TaktLogging`、`Serilog` |
| **本地化** | 庫表 + I18n 種子；靜態 `locales/**` 根鍵必須為 `page` | `Localization`、`02-frontend` §6.2 |
| **驗證碼** | `Slider` / `Behavior` | `Captcha`、`ITaktCaptchaService` |
| **緩存** | `ITaktCacheService`（Memory / Redis） | `Cache:Provider` |
| **安全** | 限流、CSRF、XSS、RSA 密碼傳輸 | `Security`、`PasswordPolicy:Transport` |
| **字段長度** | Domain `SugarColumn` Length 專項表（物料/單據/工廠等） | `17-field-length` |
| **代碼生成** | 實體 → DTO/服務/控制器/前端/i18n 流水線（單實體） | `scripts/gen/generate-all.cjs` |
| **分析擴展** | Trend / Stat / Explosion 等與 CRUD **獨立** 服務+控制器+前端 API，避免生成覆蓋 | 見 `generate-entity-exclusions` 註釋 |

前端：`v-permission`、`takt-captcha-*`、`takt-modal`、標準 CRUD 殼（`13-vue-view` / `14-vue-form`）。

---

## 業務模組

後端 `Controllers/` 與 `Domain/Entities/` 按相同領域劃分；前端 `views/`、`api/`、`types/`、`locales/` 與之對齊。

| 領域 | 子模組 | 主要能力 |
|------|--------|----------|
| **Identity** | 用戶、角色、菜單、租戶、RBAC、認證 | 登錄鑑權、權限分配、多租戶/多公司授權 |
| **Foundation** | 字典、翻譯、系統屬性、編碼、消息、在線、文化、行政區劃、文件 | 平台基礎數據、站內消息、動態 i18n |
| **HumanResource** | Organization、Personnel、Attendance、Talent、Benefits、Compensation、Performance、Training | 組織、人事、考勤、薪酬福利、績效培訓 |
| **Logistics · Materials** | 物料、供應商、廠商、工廠等 | 物料主數據 |
| **Logistics · Procurement** | 請購 / 採購訂單 / 採購價格等 | 採購業務 |
| **Logistics · Sales** | 客戶、銷售訂單 / 價格等 | 銷售管理 |
| **Logistics · CustomerService** | 請求 / 訂單 / 工單 / 合同及 Stat | 客服與統計 |
| **Logistics · Quality** | Operation（IQC/IPQC/FQC）、Complaint、Cost；含獨立 Trend | 檢驗、客訴、質量成本與趨勢 |
| **Logistics · Manufacturing** | Bom（含 Explosion / 成本分析 Trend）、Aps、Mds/Mps/Mrp、Output、Defect、EngineeringChange、LaborHour、Sop | 製造、計劃、產出、不良、ECN |
| **Logistics · Serial** | 產品序列號入出庫 | 序列號追溯 |
| **Logistics · Maintenance** | 設備、保養工單 | 設備維護 |
| **Accounting** | Financial、Controlling | 財務 / 成本主檔 |
| **Routine** | Announcement、NewsCenter、HelpDesk、DocumentCenter、**MeetingCenter**、VisitorCenter | 公告、新聞、服務台、文檔、會議、訪客 |
| **Workflow** | 方案 / 表單 / 實例 / 任務 / 變量 / 加簽 + **Engine** | 審批定義與運行時 |
| **Statistics** | Logging、**QuickQuery**（可配置快速查詢） | 審計日誌、自助查詢 |
| **Code** | Generator、Database（備份等） | 代碼生成元數據、庫維護 |

---

## 項目結構

```
TaktWise/
├── backend/
│   ├── TaktWise.slnx
│   └── src/
│       ├── Takt.Shared/           # 公共類型、Options、異常、Enums/Constants
│       ├── Takt.Domain/           # 實體、倉儲接口（~320）
│       ├── Takt.Application/      # DTO、應用服務、驗證器
│       ├── Takt.Infrastructure/   # 倉儲、種子、SignalR、中間件、Quartz
│       └── Takt.WebApi/           # API 入口（Program.cs、Controllers ~350）
├── frontend/
│   ├── src/
│   │   ├── api/                   # 按後端模組劃分 REST 客戶端
│   │   ├── types/                 # 與 api 一一對應的 TS 類型（主鍵 string）
│   │   ├── views/                 # 頁面（單表 / 主子 / 樹表）
│   │   ├── components/            # common/ + business/（takt-modal、流程設計器等）
│   │   ├── stores/ / composables/ / bootstrap/ / config/
│   │   ├── locales/               # 靜態 i18n（export default { page: … }）
│   │   ├── router/ / styles/ / utils/
│   ├── vite.config.ts             # 產物 assets/{js|css|img|other}/{領域}/
│   └── package.json
├── scripts/
│   ├── gen/                       # 代碼生成流水線（generate-all 等，僅 .cjs）
│   └── sync/                      # 外部數據同步腳本
├── .cursor/
│   ├── rules/                     # 00-project … 17-field-length
│   └── skills/                    # 與規則同名的實現清單
├── LICENSE
├── README.md / README.zh-CN.md / README.ja-JP.md / README.zh-HK.md
```

---

## 編譯與運行

### 環境要求

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) LTS（前端）
- SQL Server（連接字符串在本地 `appsettings*.json`，倉庫僅提供 `appsettings.*.Example.json` 模板）

### 後端

```bash
dotnet restore backend/TaktWise.slnx
dotnet build backend/TaktWise.slnx -c Release
dotnet run --project backend/src/Takt.WebApi/Takt.WebApi.csproj
```

| 項 | 值 |
|----|-----|
| HTTP | `http://localhost:60070` |
| HTTPS | `https://localhost:60071` |
| API 文件 | `https://localhost:60071/scalar` |

首次啟動前將 `backend/src/Takt.WebApi/appsettings.Example.json` 等 Example 文件複製為 `appsettings.json` / `appsettings.Development.json` / `appsettings.Production.json`（勿提交 Git），並填寫 `ConnectionStrings`（OpenIddict + `Tenant_*`）。`Init:InitDb` / `Init:SeedData` 見本地 `appsettings.json`。

### 前端

```bash
cd frontend
cp env.example .env
cp env.development.example .env.development
cp env.production.example .env.production
npm install
npm run dev         # 開發（默認 https://localhost:60081）
npm run build       # 生產（vue-tsc + vite build）
npm run build:vite  # 僅 Vite 打包（跳過 typecheck）
```

（Windows 可用 `copy env.example .env` 等。`.env*` 已由 `.gitignore` 排除。）

| 項 | 說明 |
|----|------|
| 開發地址 | `.env.development` 中 `VITE_APP_ORIGIN` |
| API 代理 | `/api` → `VITE_API_PROXY_TARGET`（本機後端 HTTPS） |
| OAuth 回調 | `{VITE_APP_ORIGIN}/auth/callback`，須與後端 `OpenIddict:SpaRedirectUris` 一致 |
| PWA | `VITE_PWA_ENABLED`；超大 vendor 不預緩存，見 `vite.config.ts` workbox 配置 |

須與後端 `Cors`、`OpenIddict:SpaRedirectUris` 中的前端地址一致。

### 默認帳號（種子）

種子用戶（各租戶）：`admin`（超級管理員）、`guest`、`demo`。初始密碼見 `PasswordPolicy:DefaultPassword`（默認 `Takt@123456`）。生產環境務必修改。

---

## 代碼生成

新增實體後，在倉庫根目錄按**單實體**執行（已禁用全量 `--all`）：

```bash
node scripts/gen/generate-all.cjs --Holiday
node scripts/gen/generate-all.cjs --CostCenter --dry-run
node scripts/gen/generate-from-backend.cjs --Holiday
node scripts/gen/generate-vue-all-from-api.cjs --CostCenter
```

流水線步驟（DTO → Validator → Service → Controller → 前端 api/types → i18n → Vue）以 `scripts/gen/generate-all.cjs` 內 `PIPELINE` 為準；約定見 `.cursor/rules/15-codegen.mdc`。

生成後須：編譯後端、核對權限碼四處一致、樹表/排除實體 Vue 手工對齊、`*Trend` / `*Stat` / `*Explosion` 保持獨立棧勿掛回 CRUD。

---

## 開發規範

倉庫內 `.cursor/rules/`（00～17）與 `.cursor/skills/` 定義完整約定，摘要：

- **命名 / 權限 / i18n**：`00-project`、`16-permission-i18n`（Permission 冒號、I18nKey 點號）
- **後端 / 前端**：`01-backend`、`02-frontend`；CRUD 基線 `12-crud`
- **主子表 / 樹表 / 工作流**：`10` / `11` / `09`
- **視圖 / 表單**：`13-vue-view`、`14-vue-form`
- **溢出安全**：`06` / `07` / `08`（分頁 + 虛擬列表 + 主鍵 string）
- **字段長度**：`17-field-length`
- **格式**：禁止「隔行空行」，見 `03-format-blank-lines.mdc`
- **腳本**：僅 `.cjs`；禁止用 PowerShell 做全庫查找替換（`00-project` §6）

---

## 許可

[MIT License](LICENSE) · Copyright (c) 2026 Takt Technologies Co., Ltd.

**維護者**：TaktWise（Cursor AI 等）
