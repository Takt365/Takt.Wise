---
name: 11-tree-table
description: >-
  树表 ParentId 扩展（前端懒加载+virtual，右侧 getXxxList 服务端分页）；须先满足 12-crud。
  用于部门/菜单/行政区划树、tree 模板、takt-tree-left-table、generate-vue-tree-from-api。
---

# 树表（Tree Table）

完整规范：`.cursor/rules/11-tree-table.mdc`

**前置（强制）**：[12-crud](../12-crud/SKILL.md) — 节点实体须完整 CRUD。

## 标准参照

| 层 | 文件 |
|----|------|
| 前端大数据页 | `views/foundation/admin-division/index.vue` |
| 生成脚本 | `scripts/gen/generate-vue-tree-from-api.cjs` |
| 懒加载工具 | `frontend/src/composables/useLazyTree.ts` |

## 交付顺序

```
1. 12-crud 基线（节点 CRUD + 页面壳 + 权限）
2. 本章扩展（左树懒加载+virtual，右表 list 分页；脚本 regenerate）
```

## 树表扩展清单

```
- [ ] 1. 后端：GetXxxTreeAsync(parentId) / TreeOptions(parentId) 仅一层（禁止全表 Build；禁止按阈值回退全量）
- [ ] 2. 前端：左树懒加载+virtual；右表/右树按 parentId 一层；禁止 getXxxTree('0') 当全量
- [ ] 3. 表单 TaktTreeSelect 默认懒加载（禁止 :lazy="false"）
- [ ] 4. 流水线：services → controllers → from-backend → generate-vue-tree
```

## 强制范围（须全部懒加载）

菜单、部门、科目、成本中心、利润中心、成本要素、行政区划及一切 ParentId 树。

## 与 CRUD / 代码生成

- 后端脚本：`generate-services-from-dtos.cjs`、`generate-controllers-from-services.cjs`
- 前端脚本：`generate-vue-tree-from-api.cjs`
- 参照：`TaktAdminDivisionService` + `admin-division/index.vue` + `TaktLazyTreeHelper`
- 详见 [15-codegen](../15-codegen/SKILL.md)

## 禁止

- 只有 tree 接口、缺少节点 Create/Update/Delete（违反 12-crud）
- 万级节点不拍平、不分页
- 整表递归构树 / 前端一次拉全量树 / 按总数阈值回退 list 拼树 / TaktTreeSelect `:lazy="false"`

## 交叉规则

- CRUD 基线：`12-crud`（强制前置）
- 代码生成：`15-codegen`（ParentId → tree API；Vue 须手工 13/14）
- 视图/表单：`13-vue-view`、`14-vue-form`
- 外键明细：`10-master-detail`
