---
name: 12-crud
description: >-
  Takt CRUD 全栈基线（单表/主子表/树表均须遵守）：命名/路由/权限/标准能力/页面壳。
  用于任何 CRUD 模块、getXxxList、TaktXxxsController；主子表/树表叠加 10/11 前必先满足本章。
---

# CRUD 全栈基线

完整规范：`.cursor/rules/12-crud.mdc`

**单表、主子表、树表均须先满足本章**，再叠加扩展规则。

| 表形态 | 基线（本章） | 叠加 |
|--------|--------------|------|
| 单表 | ✅ | — |
| 主子表 | ✅ 主表+子表各一套 CRUD | `10-master-detail` |
| 树表 | ✅ 节点完整 CRUD | `11-tree-table` |

细则：后端 `01-backend`；前端 `02-frontend`；视图 `13-vue-view`；表单 `14-vue-form`；生成 `15-codegen`；溢出 `06`/`07`/`08`。

## 全栈参照

| 层 | 文件 |
|----|------|
| 后端 CRUD | `TaktLoginLogService.cs`、`TaktLoginLogsController.cs` |
| 后端（扩展） | `TaktUsersController.cs` |
| 主子表 | `TaktDictTypesController` + `TaktDictDatasController` |
| 树表 | `TaktDeptsController`（list + tree 并存） |
| 前端 API | `api/identity/user.api.ts` |
| 前端单表 | `views/identity/user/index.vue` + `components/UserForm.vue` |
| 前端主子表 | `views/foundation/dict/index.vue` |
| 前端树表 | `views/human-resource/organization/dept/index.vue` |

## CRUD 基线检查清单（通用）

```
- [ ] 1. 命名：服务单数；控制器复数；API_BASE 与控制器一致
- [ ] 2. 方法：GetXxxListAsync … ExportXxxAsync（实体前缀+Async）
- [ ] 3. 路由/前端 API：与 12-crud 能力映射表一一对应
- [ ] 4. 权限：菜单 + TaktPermission + v-permission + ToolsBar 一致
- [ ] 5. i18n：entity.* / menu.*（00-project）
- [ ] 6. 页面壳：见 `13-vue-view`；表单 `14-vue-form`；defineExpose validate/getValues
- [ ] 7. 主键 string；列表双端分页（08）
- [ ] 8. 空行：03-format-blank-lines
```

## 能力映射（必对齐）

| 前端 | 后端 |
|------|------|
| getXxxList | GetXxxListAsync |
| getXxxById | GetXxxByIdAsync |
| createXxx | CreateXxxAsync |
| updateXxx | UpdateXxxAsync |
| deleteXxxById | DeleteXxxByIdAsync |
| deleteXxxBatch | DeleteXxxBatchAsync |
| updateXxxStatus | UpdateXxxStatusAsync |
| getXxxTemplate / importXxxData / exportXxxData | GetXxxTemplateAsync / ImportXxxAsync / ExportXxxAsync |

## 三种表形态交付顺序

1. **先**跑通本章 CRUD 基线（含权限、分页、页面壳）
2. **再**按形态叠加：
   - 主子表 → [10-master-detail](../10-master-detail/SKILL.md)（Fill/Save、展开/抽屉）
   - 树表 → [11-tree-table](../11-tree-table/SKILL.md)（ParentId、tree API、左树右表）

## 推荐组合

- 单表：`15-codegen` → `12-crud` + `13-vue-view` + `14-vue-form` + `08-overflow-fullstack`
- 主子表：`15-codegen` → `12-crud` + `10-master-detail` + `13-vue-view` + `14-vue-form`
- 树表：`15-codegen`（后端）→ `12-crud` + `11-tree-table` + 手工 `13-vue-view` + `14-vue-form`
