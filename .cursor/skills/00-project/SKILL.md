---
name: 00-project
description: >-
  TaktWise 项目通用约定与规则 Skill 索引（Agent 角色：C#+Ant Design Vue 20年+资深工程师；
  命名、文件头、i18n 键、权限码、Git 提交）。
  用于新建文件、不确定命名/翻译键/权限、选型 .cursor/rules 对应 skill，
  或用户提到 00-project、Takt 命名规范时。
---

# Takt 项目通用约定

完整规范：`.cursor/rules/00-project.mdc`

## Agent 角色（强制）

**定位**：20 年+ **C# / .NET** + **Ant Design Vue** 资深全栈工程师（本仓：.NET 9 / SqlSugar / DDD；Vue 3 / Ant Design Vue 4 / TS）。

### UI 选型（全组件通用）

1. **优先** Ant Design Vue 4 原生 `a-*`（先读官方文档）  
2. **其次** 仓库内基于 `a-*` 的 `Takt*`（`TaktModal`、`TaktTransfer`、`TaktSingleTable`…）  
3. **仅当**二者皆无覆盖时，才新建封装，且**必须**以 `a-*` 为内核  

| 做 | 不做 |
|----|------|
| 先对齐 `a-*` / `Takt*` 与参照页，再编码 | 原生 HTML/自绘重复实现已有组件；平行 UI 体系 |
| 短直可维护；复杂处只注释「为什么」 | 边角补丁叠成屎山、不符合组件语义的旁路 |
| 发现抽象错了就重写 | 在错误模型上继续兼容打补丁 |

细则见 `00-project` **§〇 / §0.1** 与 **§6.0**。

## 命名速查

| 范围 | 规则 |
|------|------|
| C# 类/文件 | `Takt` + PascalCase；控制器**复数**、服务**单数** |
| 前端文件 | 见 `00-project` §1.1：`views`=kebab+`index.vue`；组件/布局 PascalCase；`*.api.ts` / `*.types.ts` / `*.constants.ts` / `use*Store.ts` / `useXxx.ts` / config·utils camelCase |
| 后端方法 | `GetXxxListAsync`（禁止 `GetListAsync`） |
| 前端 API | `getXxxList`（禁止 `getList`） |
| 主键（前端 types） | `id: string` |

## 枚举 vs 字典（§1.8）

### Domain 实体（硬约束）

- **`Takt.Domain/Entities/**`：属性**一律 `int` / `int?`**，**禁止任何 C# enum**（含 `TaktYesNo`、`TaktCommonStatus`、流程/引擎枚举）
- 注释写字典码或数值语义；`DefaultValue` 与 C# 初值一致；外键 `long` 禁止 `= 0`
- 服务层可用 `Takt.Shared.Enums` 比较，写实体前转 `int`

### DTO / 服务 / 前端

- 有字典：`int` + `dict-type`；禁止同语义平行枚举
- 引擎/runtime：DTO 用 `int`；引擎内可用专用枚举
- 字典数值比较：`Takt.Shared.Constants.TaktDictValueConstants`

## 规则分层（避免重复阅读）

| 层次 | 权威源 |
|------|--------|
| 命名/权限/i18n | `00-project` §1 |
| **全新写法（禁兼容迁移）** | `00-project` §1.9 |
| CRUD 全栈基线 | `12-crud` |
| 后端/前端专域 | `01-backend` / `02-frontend` |
| 表形态扩展 | `10-master-detail` / `11-tree-table`（**叠加** 12-crud） |
| Vue 视图/表单 | `13-vue-view` / `14-vue-form` |
| 代码生成 | `15-codegen` |
| 权限/翻译键 | `16-permission-i18n` |
| 通用字段长度 | `17-field-length` |
| 溢出 | `06` / `07` / `08` |
| 工具类 | `04` / `05` |
| 空行 | `03-format-blank-lines` |

§1.4/§1.5 与 `12-crud` 能力表**同源**：命名以本章为准；CRUD 联调与页面壳以 `12-crud` 为准。

## i18n 键（动态，后端种子）

- 分隔符：小写 + **点号** `.`
- 结构：`用途.目录[.子目录…].末段`
- 菜单：`menu.{领域}.{目录…}.{项}`
- 实体字段：`entity.{实体}.{字段}`
- 部门：`org.dept.{编码小写}`
- ❌ 段内下划线拼假一段（`quote_a`）

## 静态 locales（前端）

- 路径决定前缀：`locales/identity/user/zh-CN.ts` → `identity.user.page.*`
- 文件内：`export default { page: { … } }`
- 引用：`t('identity.user.page.title')`（**必须有 `.page`**）

## 权限码

- 结构：`领域:目录[:子目录…]:实体:操作`（**冒号**分隔）
- 末段必须是操作：`list`、`create`、`update`…
- 菜单种子、`[Authorize]`、`v-permission` 三者一致

## 全新写法（§1.9）

- 只写**当前目标结构**；结构调整时改种子/代码为目标态
- ❌ `SoftDeleteObsolete*`、历史 MenuCode 软删清理、双入口/隐藏桩「兼容旧菜单」
- 库内残留旧行：重建库或手工清；**禁止**在种子写兼容迁移

## 文件头（强制）

| 类型 | 要求 |
|------|------|
| C# | 八段 `// =====` 块 + XML 注释 |
| TS（非 locales） | 同 C# 块注释 |
| Vue | 顶部 **8 行 HTML 注释**（无创建时间/创建人） |

## Git 提交

`<type>(<scope>): <subject>` — 如 `feat(identity): 用户列表分页查询`

## C# XML 注释（强制）

- ❌ **全局禁止** `<see cref="…"/>`；用纯文字写类型/字段（见 `00-project` §二 §3）
- ✅ `summary` / `param` / `returns`；交叉引用写「TaktXxx 主键」「关联表单 FormCode」等

## 写入前必检

```
- [ ] 角色/UI：C#+Ant Design Vue 20年+标准；优先 `a-*` → `Takt*`（全组件通用，禁平行 UI）
- [ ] 空行：遵守 03-format-blank-lines（无隔行空行）
- [ ] 命名：C# 有 Takt 前缀；方法带实体前缀
- [ ] XML：无 <see cref>（00-project §二 §3）
- [ ] i18n/权限：键格式符合上表
- [ ] 实体/DTO：Domain 实体禁 enum（00-project §1.8.1）
- [ ] scripts/：仅 .cjs；禁止 PowerShell（00-project §6.1）
- [ ] Agent 查找/替换：Grep + StrReplace 或 node scripts/*.cjs；禁止 Shell/PowerShell（00-project §6.2）
```

## 规则与 Skill 索引（00~17）

规则源：`.cursor/rules/`。实现任务优先打开对应 **skill**（检查清单），细则以 **mdc** 为准。

| 编码 | 规则 | Skill | 何时用 |
|------|------|-------|--------|
| 00 | `00-project.mdc` | 本文件 | 命名、i18n、权限、文件头、Git |
| 01 | `01-backend.mdc` | [01-backend](../01-backend/SKILL.md) | 后端分层、DDD、种子 |
| 02 | `02-frontend.mdc` | [02-frontend](../02-frontend/SKILL.md) | 前端技术栈、UI 组件 |
| 03 | `03-format-blank-lines.mdc` | [03-format-blank-lines](../03-format-blank-lines/SKILL.md) | 空行格式、写入后自检 |
| 04 | `04-utils-csharp.mdc` | [04-utils-csharp](../04-utils-csharp/SKILL.md) | `Helpers/`、`Utils/` |
| 05 | `05-utils-vue.mdc` | [05-utils-vue](../05-utils-vue/SKILL.md) | `frontend/src/utils/` |
| 06 | `06-overflow-csharp.mdc` | [06-overflow-csharp](../06-overflow-csharp/SKILL.md) | 后端分页/导出/算术 |
| 07 | `07-overflow-vue.mdc` | [07-overflow-vue](../07-overflow-vue/SKILL.md) | virtual/主键 string |
| 08 | `08-overflow-fullstack.mdc` | [08-overflow-fullstack](../08-overflow-fullstack/SKILL.md) | 前后端联调 |
| 09 | `09-workflow.mdc` | [09-workflow](../09-workflow/SKILL.md) | 审批工作流 |
| 10 | `10-master-detail.mdc` | [10-master-detail](../10-master-detail/SKILL.md) | 主子表 OneToMany |
| 11 | `11-tree-table.mdc` | [11-tree-table](../11-tree-table/SKILL.md) | 树表 ParentId |
| 12 | `12-crud.mdc` | [12-crud](../12-crud/SKILL.md) | **CRUD 全栈基线**（单表/主子表/树表均须） |
| 13 | `13-vue-view.mdc` | [13-vue-view](../13-vue-view/SKILL.md) | **index.vue** 三种视图壳 |
| 14 | `14-vue-form.mdc` | [14-vue-form](../14-vue-form/SKILL.md) | `*Form.vue` 三种表单 |
| 15 | `15-codegen.mdc` | [15-codegen](../15-codegen/SKILL.md) | **generate-all** 全栈生成 |
| 16 | `16-permission-i18n.mdc` | [16-permission-i18n](../16-permission-i18n/SKILL.md) | **Permission + I18nKey** 对齐 |
| 17 | `17-field-length.mdc` | [17-field-length](../17-field-length/SKILL.md) | **通用字段 Length**（编码/描述） |

## 快速选型

| 任务 | Skill |
|------|-------|
| **任何列表模块（先过 CRUD 基线）** | **`12-crud`** |
| 单表 | `12-crud` → `15-codegen` → `13-vue-view` + `14-vue-form` |
| 主子表 | `12-crud` → `15-codegen` → `10-master-detail` → `13-vue-view` + `14-vue-form` |
| 树表 | `12-crud` → `15-codegen`（后端）→ `11-tree-table` → `13-vue-view` + `14-vue-form`（Vue 手工） |
| 新建实体全栈 | **`15-codegen`** + `12-crud` |
| 删/清理 `scripts/` | **`15-codegen`** §4.2 + **`00-project`** §6.1～§6.2（仅 `.cjs`；禁止 PowerShell 查找/替换） |
| 批量查找替换 | **Grep → StrReplace** 或 **`node scripts/*.cjs`**；禁止 PowerShell（`00-project` §6.2） |
| 新建后端 | `01-backend` + `12-crud` |
| 新建前端页 | `02-frontend` + `12-crud` + `13-vue-view` + `14-vue-form` |
| 文件空行/行数翻倍 | `03-format-blank-lines` |
| C# Helper | `04-utils-csharp` |
| 前端 utils | `05-utils-vue` |
| 列表/导出/大数组 | `06-overflow-csharp` + `07-overflow-vue` + `08-overflow-fullstack` |
| 审批/流程 | `09-workflow` |
| 权限/翻译键对齐 | **`16-permission-i18n`** |
| 实体字段 Length / 统一编码长度 | **`17-field-length`** |

## 推荐组合

- **全栈 CRUD 基线**：`12-crud` + `15-codegen` + `13-vue-view` + `14-vue-form` + `08-overflow-fullstack`
- **主子表**：`12-crud` → `10-master-detail` → `13-vue-view` / `14-vue-form`
- **树表**：`12-crud` → `11-tree-table` → `13-vue-view` / `14-vue-form`
- **任意写文件**：末尾跑 `03-format-blank-lines`
