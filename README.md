# TaktWise

[English](README.md) · [简体中文](README.zh-CN.md) · [日本語](README.ja-JP.md) · [繁體中文（香港）](README.zh-HK.md)

TaktWise · Full-stack enterprise platform (.NET 9 + Vue 3).

> ⚠️ **Important notice**: This project is AI-generated (using Cursor AI and other AI-assisted development tools). Code is automatically generated and optimized by AI.
>
> 🚫 **No issues accepted**: Because this is an AI-generated project, we do not accept issues, bug reports, or feature requests. Fork the repository and modify it yourself if needed.

---

## Overview

| Metric | Approx. | Notes |
|--------|---------|-------|
| Domain entities | ~320 | `backend/src/Takt.Domain/Entities/` (with `[SugarTable]`) |
| API controllers | ~350 | `backend/src/Takt.WebApi/Controllers/` |
| Frontend list pages | ~300 | `frontend/src/views/**/index.vue` |
| Supported UI locales | 4 | `zh-CN` / `zh-HK` / `en-US` / `ja-JP` |
| Cursor rules / skills | 18 / 18 | `.cursor/rules/`, `.cursor/skills/` (00–17) |

Covers **identity & permissions, foundation platform, human resources, logistics & manufacturing, customer service, accounting, routine office, workflow, statistics & logging / quick query, and code generation**, using DDD layering, multi-tenant database sharding, and company-level data isolation.

---

## Tech stack

### Backend

| Category | Choice |
|----------|--------|
| Runtime | .NET 9 |
| ORM | SqlSugar 5.x (business DB) + EF Core (OpenIddict auth DB) |
| Database | SQL Server (`Database:DbType = 1`) |
| DI | Autofac (auto-scan application services / validators) |
| Auth | OpenIddict 7.x (OAuth 2.0 / OIDC) |
| Validation | FluentValidation |
| Logging | Serilog (console + rolling files) |
| API docs | Scalar (dev: `/scalar`) |
| Real-time | SignalR (dual Hub) |
| Scheduling | Quartz (sync jobs, meeting reminders, etc.) |

### Frontend

| Category | Choice |
|----------|--------|
| Build | Vite 8 (Rolldown) |
| Framework | Vue 3 Composition API + TypeScript |
| UI | Ant Design Vue 4.x |
| Styling | Tailwind CSS 4.x |
| State / router / HTTP | Pinia, vue-router 5, Axios |
| i18n | vue-i18n (static `locales/**` + dynamic backend seeds) |
| Workflow designer | **Primary** `takt-flow-ant-designer` (AntFlow-style tree); LogicFlow is experimental |
| Form designer | `@form-create/ant-design-vue` + antd-designer |
| Rich text | `@umoteam/editor` |
| Charts | ECharts 6 |
| Real-time | `@microsoft/signalr` |
| PWA | vite-plugin-pwa (toggle via `VITE_PWA_ENABLED`) |

**Production build output** (`frontend/dist`):

```text
assets/js/{domain}/     # entry & chunk JS
assets/css/{domain}/    # styles (aligned with views domains)
assets/img/{domain}/    # images
assets/other/{domain}/  # no extension or unrecognized types
```

Domain is taken from the first segment of `src/views|api|locales|types`; third-party → `vendor`; shared UI → `shared`; entry → `app`. See `frontend/vite.config.ts`.

---

## Architecture layers

```
Takt.WebApi          → Controllers, Program.cs, OpenIddict, middleware
Takt.Application     → DTOs, application services, FluentValidation validators
Takt.Infrastructure  → Repositories, seeds, SignalR, cache, multi-DB mapping, Quartz
Takt.Domain          → Entities, repository interfaces
Takt.Shared          → Paging, exceptions, Options, helpers, enums/constants
```

Dependency flow: `WebApi → Application + Infrastructure + Shared`; `Application → Domain + Shared`; `Domain → Shared`. Controllers inject only `ITaktXxxService`; direct SqlSugar access in controllers is forbidden.

**CRUD table shapes** (all must satisfy `12-crud` first):

| Shape | Rules | Notes |
|-------|-------|-------|
| Single table | `12-crud` + `13/14` | Standard QueryBar / ToolsBar / table / pagination / modal |
| Master–detail | + `10-master-detail` | OneToMany Fill/Save; expand row / drawer |
| Tree table | + `11-tree-table` | Lazy one-level `ParentId`; left tree + right table |

---

## Platform capabilities

### Tenant → Company → Business data

```
Tenant (per-tenant DB) → Company (company code) → Business data (dept/employee/manufacturing/sales, etc.)
```

| Layer | Backend | Frontend |
|-------|---------|----------|
| **Tenant** | Separate business DB `ConnectionStrings:Tenant_{code}`; `TaktTenant`; tenant-scoped entity bases | `useTenantStore`, `takt-tenant-toggle`; tenant selectable before and after login |
| **Company** | `TaktCompany`; `TaktCompanyEntityBase`, etc.; repository `Where(TenantCode, CompanyCode)` | `takt-company-toggle`; switches accessible companies with tenant |
| **End-to-end** | `ITaktUserContext` + headers `X-Tenant-Code` / `X-Company-Code` | `api/request.ts` attaches headers automatically |

Entity bases follow the four plant × culture combinations (see `01-backend` / `TaktEntityBase`). Isolation uses **TenantCode + CompanyCode** only — not CultureCode.

**Startup Init flags** (`appsettings.json` → `Init` / `Database`):

1. `InitDb` — Create business DB tables for each tenant in `TenantCodes` order
2. `SeedData` — Run all seeds per tenant DB in `TenantCodes` order; company/plant masters written per `CompanyCodes` / `PlantCodes`
3. `CompanyCodes` / `PlantCodes` / `CultureCodes` order must match config mapping; first `CompanyCodes` entry remains the demo account primary company

### Global SignalR

After login, **dual Hub** connections (`AddTaktSignalR`, JWT via `TaktSignalRTokenMiddleware`):

| Hub | Path | Role |
|-----|------|------|
| **TaktConnectHub** | `/hubs/TaktConnectHub` | Connect/disconnect, online users, online stats, `ForceLogout` |
| **TaktNotificationHub** | `/hubs/TaktNotificationHub` | Direct messages, company broadcast, message/online stats push |

Push targets **company + user** groups (`TaktSignalRGroupNames`). Dispatch: `ITaktSignalRDispatchService`. Frontend: `utils/taktSignalr.ts`, `stores/foundation/signalr.ts`.

### Approval workflow

In-house **AntFlow-style tree JSON engine** (not BPMN):

```
Form (FrmData) + process nodes + approver resolution + conditional gateways + exception actions (reject/withdraw/transfer/add-sign…)
```

| Location | Notes |
|----------|-------|
| Engine | `TaktFlowEngineService` / `TaktFlowEngineController` (runtime) |
| Definition CRUD | `TaktFlowScheme` / `TaktFlowForm`, etc. |
| Frontend designer | `components/business/TaktFlowAntDesigner/` |
| Spec | `.cursor/rules/09-workflow.mdc` |

### Other capabilities

| Capability | Description | Config / entry |
|------------|-------------|----------------|
| **RBAC** | Permission `domain:path:…:entity:action` (colons); `[TaktPermission]` aligned in four places with menus/frontend | `16-permission-i18n`, `Takt.WebApi/Filters/` |
| **i18n keys** | Dot-separated I18nKey: `menu.*` / `entity.*` / `common.page.*` | Backend seeds + `mergeDynamicLocaleMessages` |
| **Logging** | Serilog; `TaktLoginLog` / `TaktOperLog` / `TaktDeltaLog` | `TaktLogging`, `Serilog` |
| **Localization** | DB + I18n seeds; static `locales/**` root key must be `page` | `Localization`, `02-frontend` §6.2 |
| **Captcha** | `Slider` / `Behavior` | `Captcha`, `ITaktCaptchaService` |
| **Cache** | `ITaktCacheService` (Memory / Redis) | `Cache:Provider` |
| **Security** | Rate limit, CSRF, XSS, RSA password transport | `Security`, `PasswordPolicy:Transport` |
| **Field lengths** | Domain `SugarColumn` Length tables (material/docs/plant, etc.) | `17-field-length` |
| **Code generation** | Entity → DTO/service/controller/frontend/i18n pipeline (single entity) | `scripts/gen/generate-all.cjs` |
| **Analytics extensions** | Trend / Stat / Explosion as **independent** service + controller + frontend API (not hung on CRUD) | See `generate-entity-exclusions` comments |

Frontend: `v-permission`, `takt-captcha-*`, `takt-modal`, standard CRUD shell (`13-vue-view` / `14-vue-form`).

---

## Business modules

Backend `Controllers/` and `Domain/Entities/` share the same domain layout; frontend `views/`, `api/`, `types/`, `locales/` align accordingly.

| Domain | Submodules | Main capabilities |
|--------|------------|-------------------|
| **Identity** | Users, roles, menus, tenants, RBAC, auth | Login, permission assignment, multi-tenant/company authorization |
| **Foundation** | Dict, translation, system property, numbering, messages, online, culture, admin division, files | Platform master data, in-app messages, dynamic i18n |
| **HumanResource** | Organization, Personnel, Attendance, Talent, Benefits, Compensation, Performance, Training | Org, HR, attendance, payroll/benefits, performance & training |
| **Logistics · Materials** | Material, supplier, manufacturer, plant, etc. | Material master data |
| **Logistics · Procurement** | Purchase request / order / price, etc. | Procurement |
| **Logistics · Sales** | Customer, sales order / price, etc. | Sales |
| **Logistics · CustomerService** | Request / order / ticket / contract + Stat | Customer service & stats |
| **Logistics · Quality** | Operation (IQC/IPQC/FQC), Complaint, Cost; independent Trend | Inspection, complaints, quality cost & trends |
| **Logistics · Manufacturing** | Bom (Explosion / cost-analysis Trend), Aps, Mds/Mps/Mrp, Output, Defect, EngineeringChange, LaborHour, Sop | MES, planning, output, defects, ECN |
| **Logistics · Serial** | Product serial inbound/outbound | Serial traceability |
| **Logistics · Maintenance** | Equipment, maintenance work orders | Equipment upkeep |
| **Accounting** | Financial, Controlling | Finance / cost masters |
| **Routine** | Announcement, NewsCenter, HelpDesk, DocumentCenter, **MeetingCenter**, VisitorCenter | Announcements, news, help desk, documents, meetings, visitors |
| **Workflow** | Scheme / form / instance / task / variable / add-sign + **Engine** | Approval definition & runtime |
| **Statistics** | Logging, **QuickQuery** (configurable quick query) | Audit logs, self-service query |
| **Code** | Generator, Database (backup, etc.) | Codegen metadata, DB maintenance |

---

## Project structure

```
TaktWise/
├── backend/
│   ├── TaktWise.slnx
│   └── src/
│       ├── Takt.Shared/           # Shared types, Options, exceptions, Enums/Constants
│       ├── Takt.Domain/           # Entities, repo interfaces (~320)
│       ├── Takt.Application/      # DTOs, application services, validators
│       ├── Takt.Infrastructure/   # Repositories, seeds, SignalR, middleware, Quartz
│       └── Takt.WebApi/           # API entry (Program.cs, Controllers ~350)
├── frontend/
│   ├── src/
│   │   ├── api/                   # REST clients by backend module
│   │   ├── types/                 # TS types aligned with api/ (IDs as string)
│   │   ├── views/                 # Pages (single / master-detail / tree)
│   │   ├── components/            # common/ + business/ (takt-modal, flow designer, …)
│   │   ├── stores/ / composables/ / bootstrap/ / config/
│   │   ├── locales/               # Static i18n (export default { page: … })
│   │   ├── router/ / styles/ / utils/
│   ├── vite.config.ts             # Output assets/{js|css|img|other}/{domain}/
│   └── package.json
├── scripts/
│   ├── gen/                       # Codegen pipeline (generate-all, etc.; .cjs only)
│   └── sync/                      # External data sync scripts
├── .cursor/
│   ├── rules/                     # 00-project … 17-field-length
│   └── skills/                    # Checklists matching rule names
├── LICENSE
├── README.md / README.zh-CN.md / README.ja-JP.md / README.zh-HK.md
```

---

## Build & run

### Requirements

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) LTS (frontend)
- SQL Server (connection strings in local `appsettings*.json`; repo provides `appsettings.*.Example.json` templates only)

### Backend

```bash
dotnet restore backend/TaktWise.slnx
dotnet build backend/TaktWise.slnx -c Release
dotnet run --project backend/src/Takt.WebApi/Takt.WebApi.csproj
```

| Item | Value |
|------|-------|
| HTTP | `http://localhost:60070` |
| HTTPS | `https://localhost:60071` |
| API docs | `https://localhost:60071/scalar` |

Before first run, copy `backend/src/Takt.WebApi/appsettings.Example.json` (and related Example files) to `appsettings.json` / `appsettings.Development.json` / `appsettings.Production.json` (do not commit), and fill `ConnectionStrings` (OpenIddict + `Tenant_*`). See `Init:InitDb` / `Init:SeedData` in local `appsettings.json`.

### Frontend

```bash
cd frontend
cp env.example .env
cp env.development.example .env.development
cp env.production.example .env.production
npm install
npm run dev         # Dev (default https://localhost:60081)
npm run build       # Production (vue-tsc + vite build)
npm run build:vite  # Vite only (skip typecheck)
```

(On Windows use `copy env.example .env`, etc. `.env*` is gitignored.)

| Item | Notes |
|------|-------|
| Dev origin | `VITE_APP_ORIGIN` in `.env.development` |
| API proxy | `/api` → `VITE_API_PROXY_TARGET` (local backend HTTPS) |
| OAuth callback | `{VITE_APP_ORIGIN}/auth/callback`, must match backend `OpenIddict:SpaRedirectUris` |
| PWA | `VITE_PWA_ENABLED`; large vendor chunks excluded from precache — see `vite.config.ts` workbox |

Must align with backend `Cors` and `OpenIddict:SpaRedirectUris`.

### Default accounts (seed)

Seed users (per tenant): `admin` (super admin), `guest`, `demo`. Initial password: `PasswordPolicy:DefaultPassword` (default `Takt@123456`). Change in production.

---

## Code generation

After adding an entity, run **one entity at a time** from the repo root (`--all` is disabled):

```bash
node scripts/gen/generate-all.cjs --Holiday
node scripts/gen/generate-all.cjs --CostCenter --dry-run
node scripts/gen/generate-from-backend.cjs --Holiday
node scripts/gen/generate-vue-all-from-api.cjs --CostCenter
```

Pipeline steps (DTO → Validator → Service → Controller → frontend api/types → i18n → Vue) follow `PIPELINE` in `scripts/gen/generate-all.cjs`; conventions: `.cursor/rules/15-codegen.mdc`.

After generation: build the backend, verify permission codes in four places, hand-align tree/excluded Vue pages, and keep `*Trend` / `*Stat` / `*Explosion` as independent stacks (do not hang them on CRUD).

---

## Development conventions

Summarized from `.cursor/rules/` (00–17) and `.cursor/skills/`:

- **Naming / permissions / i18n**: `00-project`, `16-permission-i18n` (Permission uses `:`, I18nKey uses `.`)
- **Backend / frontend**: `01-backend`, `02-frontend`; CRUD baseline `12-crud`
- **Master–detail / tree / workflow**: `10` / `11` / `09`
- **Views / forms**: `13-vue-view`, `14-vue-form`
- **Overflow safety**: `06` / `07` / `08` (paging + virtual lists + string IDs)
- **Field lengths**: `17-field-length`
- **Formatting**: No “blank line every other line”; see `03-format-blank-lines.mdc`
- **Scripts**: `.cjs` only; no PowerShell for repo-wide find/replace (`00-project` §6)

---

## License

[MIT License](LICENSE) · Copyright (c) 2026 Takt Technologies Co., Ltd.

**Maintainer**: TaktWise (Cursor AI, etc.)
