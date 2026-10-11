# TaktWise

[English](README.md) · [简体中文](README.zh-CN.md) · [日本語](README.ja-JP.md) · [繁體中文（香港）](README.zh-HK.md)

TaktWise · フロント／バックエンド分離型エンタープライズプラットフォーム（.NET 9 + Vue 3）。

> ⚠️ **重要な声明**：本プロジェクトは AI により生成されています（Cursor AI 等の AI 支援開発ツールを使用）。コードは AI により自動生成・最適化されています。
>
> 🚫 **Issue は受け付けません**：AI 生成プロジェクトのため、Issue・バグ報告・機能要望は一切受け付けません。必要に応じて Fork して自行改修してください。

---

## プロジェクト概要

| 指標 | 概数 | 説明 |
|------|------|------|
| ドメインエンティティ | ~320 | `backend/src/Takt.Domain/Entities/`（`[SugarTable]` 付き） |
| API コントローラ | ~350 | `backend/src/Takt.WebApi/Controllers/` |
| フロント一覧ページ | ~300 | `frontend/src/views/**/index.vue` |
| 対応 UI 言語 | 4 | `zh-CN` / `zh-HK` / `en-US` / `ja-JP` |
| Cursor rules / skills | 18 / 18 | `.cursor/rules/`、`.cursor/skills/`（00～17） |

**認証・権限、基盤プラットフォーム、人事、物流・製造、カスタマーサービス、会計、日常業務、ワークフロー、統計ログ／クイッククエリ、コード生成** 等の業務ドメインをカバー。DDD レイヤリング + マルチテナント DB 分離 + 会社単位データ分離を採用。

---

## 技術スタック

### バックエンド

| カテゴリ | 選定 |
|----------|------|
| ランタイム | .NET 9 |
| ORM | SqlSugar 5.x（業務 DB）+ EF Core（OpenIddict 認証 DB） |
| データベース | SQL Server（`Database:DbType = 1`） |
| DI | Autofac（アプリケーションサービス／バリデータ自動スキャン登録） |
| 認証 | OpenIddict 7.x（OAuth 2.0 / OIDC） |
| バリデーション | FluentValidation |
| ログ | Serilog（コンソール + ローテーションファイル） |
| API ドキュメント | Scalar（開発環境 `/scalar`） |
| リアルタイム | SignalR（デュアル Hub） |
| スケジューリング | Quartz（同期ジョブ、会議リマインド等） |

### フロントエンド

| カテゴリ | 選定 |
|----------|------|
| ビルド | Vite 8（Rolldown） |
| フレームワーク | Vue 3 Composition API + TypeScript |
| UI | Ant Design Vue 4.x |
| スタイル | Tailwind CSS 4.x |
| 状態 / ルータ / HTTP | Pinia、vue-router 5、Axios |
| 国際化 | vue-i18n（静的 `locales/**` + バックエンド動的シード） |
| ワークフロー設計 | **主経路** `takt-flow-ant-designer`（AntFlow 風ツリー）；LogicFlow は実験ビュー |
| フォーム設計 | `@form-create/ant-design-vue` + antd-designer |
| リッチテキスト | `@umoteam/editor` |
| チャート | ECharts 6 |
| リアルタイム | `@microsoft/signalr` |
| PWA | vite-plugin-pwa（`VITE_PWA_ENABLED` で切替） |

**本番ビルド出力**（`frontend/dist`）：

```text
assets/js/{業務領域}/     # エントリ／チャンク JS
assets/css/{業務領域}/    # スタイル（views 領域と整合）
assets/img/{業務領域}/    # 画像
assets/other/{業務領域}/  # 拡張子なし／未分類
```

業務領域は `src/views|api|locales|types` の先頭セグメント；サードパーティ → `vendor`；共通 UI → `shared`；エントリ → `app`。設定は `frontend/vite.config.ts`。

---

## アーキテクチャ層

```
Takt.WebApi          → コントローラ、Program.cs、OpenIddict、ミドルウェア
Takt.Application     → DTO、アプリケーションサービス、FluentValidation
Takt.Infrastructure  → リポジトリ、シード、SignalR、キャッシュ、マルチ DB、Quartz
Takt.Domain          → エンティティ、リポジトリインタフェース
Takt.Shared          → ページング、例外、Options、ヘルパ、Enums/Constants
```

依存方向：`WebApi → Application + Infrastructure + Shared`；`Application → Domain + Shared`；`Domain → Shared`。コントローラは `ITaktXxxService` のみ注入し、SqlSugar 直接アクセスは禁止。

**CRUD テーブル形態**（いずれも先に `12-crud` を満たすこと）：

| 形態 | ルール | 説明 |
|------|--------|------|
| 単表 | `12-crud` + `13/14` | 標準 QueryBar / ToolsBar / テーブル / ページング / モーダル |
| 主従 | + `10-master-detail` | OneToMany Fill/Save；展開行／ドロワ |
| ツリー | + `11-tree-table` | `ParentId` 1 階層遅延；左ツリー＋右テーブル |

---

## プラットフォーム能力

### テナント → 会社 → 業務データ

```
テナント Tenant（テナント別 DB） → 会社 Company（会社コード） → 業務データ（部門／従業員／製造／販売など）
```

| 層 | バックエンド | フロント |
|----|--------------|----------|
| **テナント** | 業務 DB `ConnectionStrings:Tenant_{code}`；`TaktTenant`；テナント系基底 | `useTenantStore`、`takt-tenant-toggle`；ログイン前後で選択可 |
| **会社** | `TaktCompany`；`TaktCompanyEntityBase` 等；`Where(TenantCode, CompanyCode)` | `takt-company-toggle`；テナント連動で切替 |
| **貫通** | `ITaktUserContext` + ヘッダ `X-Tenant-Code` / `X-Company-Code` | `api/request.ts` が自動付与 |

エンティティ基底は「関連工場 × 言語」4 組合せ（`01-backend` / `TaktEntityBase`）。データ分離は **TenantCode + CompanyCode** のみ（CultureCode はテナント分離に使わない）。

**起動 Init**（`appsettings.json` → `Init` / `Database`）：

1. `InitDb` — `TenantCodes` 順に各テナント業務 DB の表作成
2. `SeedData` — `TenantCodes` 順にシード実行；会社／工場マスタ等は `CompanyCodes` / `PlantCodes` 同順
3. `CompanyCodes` / `PlantCodes` / `CultureCodes` の順序は設定マップと一致；先頭 `CompanyCodes` はデモ帳号の主会社等の既定

### グローバル SignalR

ログイン後 **デュアル Hub**（`AddTaktSignalR`、JWT は `TaktSignalRTokenMiddleware`）：

| Hub | パス | 役割 |
|-----|------|------|
| **TaktConnectHub** | `/hubs/TaktConnectHub` | 接続／切断、オンライン、統計、強制ログアウト |
| **TaktNotificationHub** | `/hubs/TaktNotificationHub` | 私信、社内ブロードキャスト、メッセージ／オンライン統計 |

配信は **会社 + ユーザ** グループ（`TaktSignalRGroupNames`）。入口：`ITaktSignalRDispatchService`。フロント：`utils/taktSignalr.ts`、`stores/foundation/signalr.ts`。

### 承認ワークフロー

自作 **AntFlow 風ツリー JSON エンジン**（BPMN ではない）：

```
フォーム（FrmData） + プロセスノード + 承認者解決 + 条件ゲートウェイ + 例外動作（却下／撤回／転送／加算…）
```

| 配置 | 説明 |
|------|------|
| エンジン | `TaktFlowEngineService` / `TaktFlowEngineController`（ランタイム） |
| 定義 CRUD | `TaktFlowScheme` / `TaktFlowForm` 等 |
| フロント設計器 | `components/business/TaktFlowAntDesigner/` |
| 規約 | `.cursor/rules/09-workflow.mdc` |

### その他の能力

| 能力 | 説明 | 設定／入口 |
|------|------|------------|
| **RBAC** | 権限コード `領域:ディレクトリ:…:エンティティ:操作`（コロン）；`[TaktPermission]` をメニュー／フロントと 4 箇所一致 | `16-permission-i18n`、`Takt.WebApi/Filters/` |
| **翻訳キー** | I18nKey はドット：`menu.*` / `entity.*` / `common.page.*` | バックエンドシード + `mergeDynamicLocaleMessages` |
| **ログ** | Serilog；`TaktLoginLog` / `TaktOperLog` / `TaktDeltaLog` | `TaktLogging`、`Serilog` |
| **ローカライズ** | DB + I18n シード；静的 `locales/**` のルートは必ず `page` | `Localization`、`02-frontend` §6.2 |
| **CAPTCHA** | `Slider` / `Behavior` | `Captcha`、`ITaktCaptchaService` |
| **キャッシュ** | `ITaktCacheService`（Memory / Redis） | `Cache:Provider` |
| **セキュリティ** | レート制限、CSRF、XSS、RSA パスワード伝送 | `Security`、`PasswordPolicy:Transport` |
| **フィールド長** | Domain `SugarColumn` Length（品目／伝票／工場など） | `17-field-length` |
| **コード生成** | エンティティ → DTO／サービス／コントローラ／フロント／i18n（単体） | `scripts/gen/generate-all.cjs` |
| **分析拡張** | Trend / Stat / Explosion は CRUD と**独立**のサービス＋コントローラ＋フロント API | `generate-entity-exclusions` のコメント参照 |

フロント：`v-permission`、`takt-captcha-*`、`takt-modal`、標準 CRUD シェル（`13-vue-view` / `14-vue-form`）。

---

## 業務モジュール

バックエンド `Controllers/` と `Domain/Entities/` は同一ドメイン構成；フロント `views/`、`api/`、`types/`、`locales/` も整合。

| ドメイン | サブモジュール | 主な能力 |
|----------|----------------|----------|
| **Identity** | ユーザ、ロール、メニュー、テナント、RBAC、認証 | ログイン、権限付与、マルチテナント／会社 |
| **Foundation** | 辞書、翻訳、システムプロパティ、採番、メッセージ、オンライン、文化、行政区、ファイル | 基盤マスタ、社内メッセージ、動的 i18n |
| **HumanResource** | Organization、Personnel、Attendance、Talent、Benefits、Compensation、Performance、Training | 組織、人事、勤怠、給与福利、業績・研修 |
| **Logistics · Materials** | 品目、仕入先、メーカ、工場など | 品目マスタ |
| **Logistics · Procurement** | 購買依頼／発注／価格など | 購買 |
| **Logistics · Sales** | 得意先、受注／価格など | 販売 |
| **Logistics · CustomerService** | 依頼／注文／チケット／契約＋ Stat | CS と統計 |
| **Logistics · Quality** | Operation（IQC/IPQC/FQC）、Complaint、Cost；独立 Trend | 検査、クレーム、品質コスト／トレンド |
| **Logistics · Manufacturing** | Bom（Explosion／原価分析 Trend）、Aps、Mds/Mps/Mrp、Output、Defect、EngineeringChange、LaborHour、Sop | 製造、計画、出来高、不良、ECN |
| **Logistics · Serial** | 製品シリアル入出庫 | シリアル追跡 |
| **Logistics · Maintenance** | 設備、保全作業指示 | 設備保全 |
| **Accounting** | Financial、Controlling | 会計／原価マスタ |
| **Routine** | Announcement、NewsCenter、HelpDesk、DocumentCenter、**MeetingCenter**、VisitorCenter | お知らせ、ニュース、ヘルプデスク、文書、会議、来訪 |
| **Workflow** | 方案／フォーム／インスタンス／タスク／変数／加算＋ **Engine** | 承認定義とランタイム |
| **Statistics** | Logging、**QuickQuery**（設定可能なクイッククエリ） | 監査ログ、セルフサービス照会 |
| **Code** | Generator、Database（バックアップ等） | コード生成メタ、DB 保守 |

---

## プロジェクト構成

```
TaktWise/
├── backend/
│   ├── TaktWise.slnx
│   └── src/
│       ├── Takt.Shared/           # 共有型、Options、例外、Enums/Constants
│       ├── Takt.Domain/           # エンティティ、リポジトリ IF（~320）
│       ├── Takt.Application/      # DTO、アプリサービス、バリデータ
│       ├── Takt.Infrastructure/   # リポジトリ、シード、SignalR、ミドルウェア、Quartz
│       └── Takt.WebApi/           # API 入口（Program.cs、Controllers ~350）
├── frontend/
│   ├── src/
│   │   ├── api/                   # バックエンドモジュール別 REST
│   │   ├── types/                 # api 対応 TS 型（主キー string）
│   │   ├── views/                 # ページ（単表／主従／ツリー）
│   │   ├── components/            # common/ + business/（takt-modal、フロー設計器など）
│   │   ├── stores/ / composables/ / bootstrap/ / config/
│   │   ├── locales/               # 静的 i18n（export default { page: … }）
│   │   ├── router/ / styles/ / utils/
│   ├── vite.config.ts             # 出力 assets/{js|css|img|other}/{領域}/
│   └── package.json
├── scripts/
│   ├── gen/                       # コード生成パイプライン（.cjs のみ）
│   └── sync/                      # 外部データ同期
├── .cursor/
│   ├── rules/                     # 00-project … 17-field-length
│   └── skills/                    # ルール同名の実装チェックリスト
├── LICENSE
├── README.md / README.zh-CN.md / README.ja-JP.md / README.zh-HK.md
```

---

## ビルドと実行

### 要件

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) LTS（フロント）
- SQL Server（接続文字列はローカル `appsettings*.json`；リポジトリは `appsettings.*.Example.json` のみ）

### バックエンド

```bash
dotnet restore backend/TaktWise.slnx
dotnet build backend/TaktWise.slnx -c Release
dotnet run --project backend/src/Takt.WebApi/Takt.WebApi.csproj
```

| 項目 | 値 |
|------|-----|
| HTTP | `http://localhost:60070` |
| HTTPS | `https://localhost:60071` |
| API ドキュメント | `https://localhost:60071/scalar` |

初回前に `appsettings.Example.json` 等を `appsettings.json` / `Development` / `Production` にコピー（Git にコミットしない）し、`ConnectionStrings`（OpenIddict + `Tenant_*`）を記入。`Init:InitDb` / `Init:SeedData` はローカル `appsettings.json` を参照。

### フロントエンド

```bash
cd frontend
cp env.example .env
cp env.development.example .env.development
cp env.production.example .env.production
npm install
npm run dev         # 開発（既定 https://localhost:60081）
npm run build       # 本番（vue-tsc + vite build）
npm run build:vite  # Vite のみ（typecheck 省略）
```

（Windows は `copy env.example .env` 等。`.env*` は `.gitignore` 済み。）

| 項目 | 説明 |
|------|------|
| 開発オリジン | `.env.development` の `VITE_APP_ORIGIN` |
| API プロキシ | `/api` → `VITE_API_PROXY_TARGET`（ローカル HTTPS バックエンド） |
| OAuth コールバック | `{VITE_APP_ORIGIN}/auth/callback`（`OpenIddict:SpaRedirectUris` と一致） |
| PWA | `VITE_PWA_ENABLED`；大型 vendor は precache 除外（`vite.config.ts` workbox） |

バックエンド `Cors` / `OpenIddict:SpaRedirectUris` と揃えること。

### 既定アカウント（シード）

テナントごと：`admin`（スーパー管理者）、`guest`、`demo`。初期パスワードは `PasswordPolicy:DefaultPassword`（既定 `Takt@123456`）。本番では必ず変更。

---

## コード生成

エンティティ追加後、リポジトリルートで**単体**実行（`--all` は無効）：

```bash
node scripts/gen/generate-all.cjs --Holiday
node scripts/gen/generate-all.cjs --CostCenter --dry-run
node scripts/gen/generate-from-backend.cjs --Holiday
node scripts/gen/generate-vue-all-from-api.cjs --CostCenter
```

パイプライン（DTO → Validator → Service → Controller → フロント api/types → i18n → Vue）は `scripts/gen/generate-all.cjs` の `PIPELINE` が正。規約：`.cursor/rules/15-codegen.mdc`。

生成後：バックエンドビルド、権限コード 4 箇所一致、ツリー／除外 Vue は手直し、`*Trend` / `*Stat` / `*Explosion` は独立スタックのまま（CRUD にぶら下げない）。

---

## 開発規約

`.cursor/rules/`（00～17）と `.cursor/skills/` の要約：

- **命名／権限／i18n**：`00-project`、`16-permission-i18n`（Permission は `:`、I18nKey は `.`）
- **バック／フロント**：`01-backend`、`02-frontend`；CRUD 基線 `12-crud`
- **主従／ツリー／WF**：`10` / `11` / `09`
- **ビュー／フォーム**：`13-vue-view`、`14-vue-form`
- **オーバーフロー**：`06` / `07` / `08`（ページング＋仮想リスト＋ ID string）
- **フィールド長**：`17-field-length`
- **書式**：1 行おき空行禁止（`03-format-blank-lines.mdc`）
- **スクリプト**：`.cjs` のみ；リポジトリ横断の PowerShell 置換禁止（`00-project` §6）

---

## ライセンス

[MIT License](LICENSE) · Copyright (c) 2026 Takt Technologies Co., Ltd.

**メンテナー**：TaktWise（Cursor AI 等）
