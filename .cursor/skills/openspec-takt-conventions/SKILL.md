---
name: openspec-takt-conventions
description: >-
  TaktWise × OpenSpec 桥接：在 propose/apply/explore/update/sync/archive 前，
  按变更范围强制加载 .cursor/rules/00～17.mdc、.cursor/skills/00～17 与 openspec/config.yaml。
  由 openspec-* 技能与 opsx-* 命令引用；也可在不确定规则编号时单独读取。
---

# TaktWise · OpenSpec 约定桥接

**权威细则（强制）**：`.cursor/rules/00-project.mdc`～`17-field-length.mdc`  
**实现清单**：`.cursor/skills/00-project`～`17-field-length` 同名 `SKILL.md`  
**OpenSpec 项目上下文**：`openspec/config.yaml`（`context` / `rules` / `operations`）  
**命令入口**：`.cursor/commands/opsx-*.md`（须关联本桥接 + 下表 rules）

本文件**不替代** 00～17；只规定 **何时 Read 哪一份**，避免 OpenSpec / opsx 流程绕开仓库规范。

---

## 一、每次 OpenSpec / opsx 动作开始（强制）

1. `openspec context --json` → 确认 `root.path`
2. Read `openspec/config.yaml`（应用 `context`；产物规则见 `rules`）
3. Read 本桥接
4. 按 §二 路由：**优先 Read 命中的 `.cursor/rules/*.mdc`**，再读同名 `.cursor/skills/*/SKILL.md` 作检查清单
5. 领域语义：代码前缀 `Logistics` / `logistics` = **后勤**（`menu.logistics._self`），**不是**运输「物流」；其下 Manufacturing = **生产执行**

---

## 二、按变更主题路由（命中即读 · Rule + Skill）

| 主题 / 关键词 | Rule（权威） | Skill（清单） |
|---|---|---|
| 命名、文件头、Git、全新写法、角色/UI 选型 | `00-project.mdc` | `00-project` |
| 实体、DTO、Service、Controller、种子、SqlSugar | `01-backend.mdc` | `01-backend` |
| Vue 页、api/types、locales、Ant Design Vue、Tailwind | `02-frontend.mdc` | `02-frontend` |
| 写入后空行、隔行空行 | `03-format-blank-lines.mdc` | `03-format-blank-lines` |
| C# Helper / Utils | `04-utils-csharp.mdc` | `04-utils-csharp` |
| 前端 `utils/` | `05-utils-vue.mdc` | `05-utils-vue` |
| C# 分页/OOM/算术 | `06-overflow-csharp.mdc` | `06-overflow-csharp` |
| Vue 虚拟列表/主键 string/递归 | `07-overflow-vue.mdc` | `07-overflow-vue` |
| 前后端联调溢出 | `08-overflow-fullstack.mdc` | `08-overflow-fullstack` |
| 审批流 / FlowEngine / 设计器 | `09-workflow.mdc` | `09-workflow` |
| 主子表 OneToMany | `10-master-detail.mdc` | `10-master-detail`（先 `12-crud`） |
| 树表 ParentId 懒加载 | `11-tree-table.mdc` | `11-tree-table`（先 `12-crud`） |
| 任意列表 CRUD | `12-crud.mdc` | `12-crud` |
| `index.vue` 列表壳 | `13-vue-view.mdc` | `13-vue-view` |
| `*Form.vue` | `14-vue-form.mdc` | `14-vue-form` |
| `generate-all` / `scripts/*.cjs` | `15-codegen.mdc` | `15-codegen` |
| Permission `:` / I18nKey `.` / 四处一致 | `16-permission-i18n.mdc` | `16-permission-i18n` |
| `[SugarColumn] Length` | `17-field-length.mdc` | `17-field-length` |

路径前缀：Rule → `.cursor/rules/`；Skill → `.cursor/skills/<name>/SKILL.md`。

**默认基线**（几乎所有业务变更）：`00-project` + `12-crud`；有前端再加 `02-frontend`；有实体字段再加 `17-field-length`；有菜单/按钮再加 `16-permission-i18n`。

---

## 三、按 OpenSpec 阶段怎么用

| 阶段 | 命令 / Skill | 对 00～17 的用法 |
|---|---|---|
| 探索 | `/opsx-explore` · `openspec-explore` | 用路由表对照现状；结论须引用真实路径；不写业务代码 |
| 提案 | `/opsx-propose` · `openspec-propose` | proposal/specs/design/tasks 对齐 `openspec/config.yaml` `rules`；tasks 标明 skill/rule 编号 |
| 修订计划 | `/opsx-update` · `openspec-update-change` | 改 artifacts 时重核路由；禁止顺带改业务代码 |
| 落地 | `/opsx-apply` · `openspec-apply-change` | **实现前** Read 命中 `.mdc` + skill；写完跑 `03` 自检 |
| 同步主规范 | `/opsx-sync` · `openspec-sync-specs` | 主 spec 措辞与仓库术语一致（后勤≠物流；Permission≠I18nKey） |
| 归档 | `/opsx-archive` · `openspec-archive-change` | 归档摘要用简体中文；capability 路径与域目录一致 |

---

## 四、硬约束速记（规划与落地均不得违反）

- 控制器复数 / 服务单数；方法带实体前缀（`GetXxxListAsync` ↔ `getXxxList`）
- Domain 实体属性禁 C# enum → `int`/`int?`；Length 查 `17-field-length`
- Permission 冒号、I18nKey 点号；静态 locales 根键必须 `page`
- 列表双端分页；树一层懒加载；前端 long id 用 `string`
- 禁止兼容迁移种子（`SoftDeleteObsolete*`、双入口）；`scripts/` 仅 `.cjs`
- UI：`a-*` → `Takt*`；禁止平行自研 UI
- Agent 批量改写：Grep + StrReplace 或 `node scripts/*.cjs`，禁止 PowerShell 查找替换

---

## 五、Capability 路径建议

与代码域对齐的 kebab 路径，例如：

`identity/…` · `foundation/…` · `human-resource/…` · `logistics/manufacturing/…` · `logistics/materials/…` · `accounting/…` · `workflow/…` · `routine/…`

---

## 六、自检（apply / archive 前）

```
- [ ] 已读 openspec/config.yaml context
- [ ] 已按 §二 Read 命中的 .mdc + 00～17 SKILL
- [ ] Permission 四处 / entity·menu 种子 / Length / 分页与空行已对照
- [ ] 文档与产物中 Logistics 语义为「后勤」
```
