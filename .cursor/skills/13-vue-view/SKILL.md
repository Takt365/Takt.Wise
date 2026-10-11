---
name: 13-vue-view
description: >-
  新建或审查 Takt 列表页 index.vue（单表/主子表/树表三种视图壳）。
  用于 views/**/index.vue、QueryBar/ToolsBar/表格/分页/弹窗分区，
  或用户提到 13-vue-view、标准 CRUD 页壳、左树右表时。
---

# Vue 列表视图（index.vue）

完整规范：`.cursor/rules/13-vue-view.mdc`

**前置（强制）**：[12-crud](../12-crud/SKILL.md)

| 形态 | 参照 index.vue |
|------|----------------|
| 单表 | `views/identity/user/index.vue` |
| 主子表 | `views/foundation/dict/index.vue` |
| 树表 | `views/human-resource/organization/dept/index.vue` |

## 选型

| 表形态 | 本章 | 叠加 |
|--------|------|------|
| 单表 | §单表清单 | — |
| 主子表 | §主子表增量 | [10-master-detail](../10-master-detail/SKILL.md) |
| 树表 | §树表增量 | [11-tree-table](../11-tree-table/SKILL.md) |

表单弹窗内组件 → [14-vue-form](../14-vue-form/SKILL.md)

## 单表清单

```
- [ ] 8 行 HTML 头；模板分区：QueryBar → ToolsBar → SingleTable → Pagination → Modal → QueryDrawer → ColumnDrawer
- [ ] TaktToolsBar *-permission 与菜单一致
- [ ] TaktSingleTable :pagination="false" + 外置 TaktPagination
- [ ] row-key string；loadData/handleSearch/CRUD/import/export 对齐 12-crud §六
- [ ] `useTableRefresh(loadData)`（`loadData` 定义之后；订阅租户/公司切换）
- [ ] TaktModal `:width="formModalWidthPx"`（`useTaktContentModalWidth`）+ <XxxForm ref="formRef" />（勿双层 a-modal）
- [ ] 列标题 entity.*；loading 态；07/08 分页
- [ ] 03-format-blank-lines
```

## 主子表增量

```
- [ ] 主表 getXxxList 分页；禁止 list 带全量子表
- [ ] 子表：展开 @expand 懒加载 或 DictDataWindow / takt-master-detail-table
- [ ] 子表 getYyyList({ xxxId, pageIndex, pageSize })；外键 string
- [ ] 展开区 async + loading；子表独立 API
```

## 树表增量

```
- [ ] 左树右表：TaktTreeLeft/Right QueryBar、ToolsBar、Table
- [ ] getXxxTree → fullTree → 选中节点 getSubtree → flatten → 客户端分页 slice
- [ ] TaktTreeRightTable showPagination + virtual（行数大）
- [ ] 树拖拽 drop → updateXxx(parentId, sortOrder)；Id string
- [ ] 树深 ≤10；11-tree-table 细则
```

## 代码生成（generate-vue-*-from-api）

| 形态 | 脚本 | 说明 |
|------|------|------|
| 单表 | `generate-vue-crud-from-api.cjs` | 标准壳 |
| 主子表 | `generate-vue-master-detail-from-api.cjs` | 展开行 + `:show-expand` |
| 树表 | `generate-vue-tree-from-api.cjs` | 左树右表；`Dept` 等在排除列表须手工 |

排除实体 / `workflow/**`：见 [15-codegen](../15-codegen/SKILL.md) §4.3。

## 交叉规则

- CRUD 基线：`12-crud`
- 代码生成：`15-codegen`（单表/主子表自动生成；树表 Vue 手工）
- 表单：`14-vue-form`
- UI 栈/i18n：`02-frontend`
- 溢出：`07-overflow-vue`、`08-overflow-fullstack`
