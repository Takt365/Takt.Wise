---
name: 15-codegen
description: >-
  Takt 全栈代码生成（generate-all 流水线、单表/主子表/树表脚本识别、排除列表、生成后审阅）。
  用于 node scripts/generate-all、generate-vue-all-from-api、新建实体后生成代码，
  或用户提到 15-codegen、代码生成、generate-all 时。
---

# 全栈代码生成

完整规范：`.cursor/rules/15-codegen.mdc`

**入口**：`node scripts/generate-all.cjs --<Entity>`（实体名不带 `Takt` 前缀；**已禁用 `--all`**）

## 新建实体工作流（推荐）

```
1. 写好 Takt.Domain/Entities/**/TaktXxx.cs
2. node scripts/generate-all.cjs --Xxx
3. dotnet build
4. 人工：QueryExpression、菜单+按钮 Permission、Mapster
5. 对照 12-crud + 10/11（形态）+ 13/14（Vue）+ 16-permission-i18n
```

## 流水线（10 步，不可跳序；权威源 `generate-all.cjs` → `PIPELINE`）

| 步 | `key` | 脚本 | `alwaysAll` | 产出 |
|----|-------|------|-------------|------|
| 0 | `entity-rbac-nav` | `generate-entity-rbac-navigations.cjs` | — | RBAC `[Navigate]` |
| 1 | `dtos` | `generate-dtos-from-entity.cjs` | — | `TaktXxxDtos.cs` |
| 2 | `validators` | `generate-validators-from-entity.cjs` | ✅ | `TaktXxx*Validator.cs` |
| 3 | `services` | `generate-services-from-dtos.cjs` | — | Service |
| 4 | `controllers` | `generate-controllers-from-services.cjs` | — | Controller |
| 5 | `frontend` | `generate-from-backend.cjs` | — | `types/` + `api/` |
| 6 | `i18n` | `generate-entity-i18n-seed.cjs` | ✅ | `TaktXxxI18nSeedData.cs` |
| 7 | `dict-i18n` | `generate-dict-i18n-seed.cjs` | ✅ | `TaktDictI18nSeedData.cs` |
| 8 | `menu-i18n` | `generate-menu-i18n-seed.cjs` | ✅ | 菜单 `menu.*` |
| 9 | `vue` | `generate-vue-all-from-api.cjs` | — | `index.vue` + `*Form.vue` |

**文档与代码不一致时以 `PIPELINE` 为准并回写本文。** `validate-*` 一次性校验脚本**不属于**流水线，完成后删除。详见规则 §四。

**写入策略**：不存在则创建，已存在则覆盖；`TaktAuth`/`TaktRbac`/`TaktFlowEngine` 等须 `--force` 才覆盖。

## 三种形态 × 生成范围

| 形态 | 实体特征 | 后端脚本 | Vue 脚本 |
|------|----------|----------|----------|
| 单表 `crud` | 无 OneToMany、无 ParentId | 标准 CRUD + Options | ✅ |
| 主子表 `sub` | `[Navigate] OneToMany` | Fill/Save 级联 | ✅ 展开行 + 表单 Tab |
| 树表 `tree` | 含 `ParentId` | tree / tree-options API | ❌ 手工 13/14 |

识别：`generate-services-from-dtos.cjs` → `identifyCrudType`。

## 租户实体基类四组合（生成脚本强制）

| 组合 | Domain | Application DTO | Query/Create/Template/Import 必须字段 |
|------|--------|-----------------|--------------------------------------|
| 1 默认 | `TaktTenantEntityBase` | `TaktTenantDtoBase` | `RelatedPlant` + `CultureCode` |
| 2 | `TaktTenantCultureEntityBase` | `TaktTenantCultureDtoBase` | `CultureCode` |
| 3 | `TaktTenantPlantEntityBase` | `TaktTenantPlantDtoBase` | `RelatedPlant` |
| 4 | `TaktTenantCoreEntityBase` | `TaktTenantCoreDtoBase` | 仅 `TenantCode` |

权威解析：`generate-script-common.cjs` → `ENTITY_CLASS_HEADER_REGEX` / `ENTITY_BASE_TO_DTO_BASE`。

## 单脚本命令（调试/局部生成）

```bash
node scripts/generate-dtos-from-entity.cjs --Holiday
node scripts/generate-services-from-dtos.cjs --Holiday
node scripts/generate-from-backend.cjs --Holiday
node scripts/generate-vue-crud-from-api.cjs --Holiday --view-path human-resource/attendance-leave/holiday
node scripts/generate-vue-all-from-api.cjs --CostCenter
node scripts/generate-all.cjs --Holiday --dry-run
```

## 排除（不生成或跳过）

| 类别 | 示例 |
|------|------|
| 手工 CRUD | User、Online、Message、DictType、DictData |
| RBAC 八表 | UserRole、RoleMenu、EmployeeDept… |
| 独立服务 | TaktAuth、TaktRbac、TaktFlowEngine |
| Vue 排除 | User、Menu、Dept、Dict*、Culture、Translation、workflow/** |

树表 **Dept**：后端可生成，Vue 须参照 `dept/index.vue` 手工。

## 列表无查询条件则空表（防大数据卡顿）

- 后端：`HasAnyListQueryFilter` —— 无业务条件时 `GetXxxListAsync` / `ExportXxxAsync` 返回空；**有条件时正常过滤分页**
- 前端：`hasAnyListQueryFilter` —— 无参/重置静默空表；有参才请求
- QueryExpression：字符串条件局部变量捕获后再 `Contains`；KeyWords 禁止日期 `ToString().Contains`

## 生成后必检

```
- [ ] dotnet build
- [ ] QueryExpression / 无参空表 + 有参过滤
- [ ] 权限四处一致（16-permission-i18n）
- [ ] entity.* i18n 种子与列标题 t('entity.*')
- [ ] 树表/排除实体：13-vue-view + 14-vue-form 手工页
- [ ] 12-crud 11 项能力 + 08 双端分页
- [ ] 表单 TaktModal `:width="formModalWidthPx"`（`useTaktContentModalWidth`）；导入弹窗仍 `:width="600"`
- [ ] 「xxx内容」表单为 takt-rich-editor（非 textarea）
- [ ] fileName + accessUrl 成对时为 takt-upload-file（排除 TaktFile）
- [ ] 「自动通过 TaktNumbering」无 MIME：规则下拉 + 只读编码；CreateDto.NumberingRuleCode；Service 注入 ITaktNumberingGenerator
- [ ] 「自动通过 TaktNumbering」含 MIME：不生成表单规则控件（上传引擎 MIME→FD-F*）
- [ ] 03-format-blank-lines
```

## 命名（脚本强制）

| 层 | 规则 |
|----|------|
| 服务 | 单数 `TaktHolidayService` |
| 控制器 | 复数 `TaktHolidaysController` |
| API 路由 | `api/TaktHolidays`（复数） |
| 前端 api 文件 | kebab 单数 `holiday.ts` |
| 前端 types | 同模块 `{entity}.d.ts` |

复数特例：`generate-script-common.cjs` → `CONTROLLER_PLURAL_OVERRIDES`（User→Users 等）。

## 删改 `scripts/` 与批量改写（强制）

- **`scripts/` 仅 `.cjs`**；❌ 禁止 PowerShell 查找/替换/批量写回
- 批量替换：**Grep 定位 → StrReplace 逐文件**；或写一次性 **`scripts/<task>.cjs`** 用 `node` 跑
- 删脚本前：**Read** `PIPELINE` + **Grep 工具**（禁止 Shell/`Select-String`/`git grep`）

完整分类见 `15-codegen.mdc` §4.2；工具约束见 `00-project.mdc` §6.2。

## 交叉规则

| 场景 | 规则 / Skill |
|------|----------------|
| CRUD 基线 | `12-crud` |
| 主子表 | `10-master-detail` |
| 树表 | `11-tree-table` |
| Vue 视图/表单 | `13-vue-view`、`14-vue-form` |
| 权限/翻译键 | `16-permission-i18n` |
| 后端/前端专域 | `01-backend`、`02-frontend` |
