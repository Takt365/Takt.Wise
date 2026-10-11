---
name: 10-master-detail
description: >-
  主子表 OneToMany 扩展（级联 Fill/Save、左右/上下 UI）；须先满足 12-crud（主表+子表双实体 CRUD）。
  用于 master-detail、TaktMasterDetailTableLr/Tb、*Panel、use-*-master-context、sub 模板。
---

# 主子表（Master-Detail）

完整规范：`.cursor/rules/10-master-detail.mdc`（后端与数据层）；**本章 Skill 侧重当前前端两种布局与交付路径**。

**前置（强制）**：[12-crud](../12-crud/SKILL.md) — 主表与子表实体各须完整 CRUD（参照 `TaktDictTypes` + `TaktDictDatas`）。

---

## 一、UI 布局选型（当前工程）

| 布局 | 组件 | 场景 | 工程现状 |
|------|------|------|----------|
| **左右（LR）** | `TaktMasterDetailTableLr` + `#detail` 插槽 | 从表列多、需独立 QueryBar/ToolsBar/权限/列设置 | **主流**（`generate-vue-master-detail-from-api` 默认） |
| **上下（TB）** | `TaktMasterDetailTableTb` | 从表列少、主从同页内嵌双表、从表走组件 props | 组件已就绪，简单主子表可选用 |
| **弹窗上下** | `TaktEditableTable`（在 `*Form.vue`） | 新建/编辑一次级联提交子表行 | 生成脚本与手工表单共用 |
| **展开行** | `TaktSingleTable` + `expandedRowRender` | 子表预览、行数极少 | 遗留/特例（如 `dict/index.vue`） |
| **独立窗口** | `DictDataWindow` 等 | 子表 CRUD 极复杂、独立权限 | 手工增强 |

### 1.1 左右布局（LR，标准列表页）

```text
index.vue（h-full flex flex-col min-h-0）
 → TaktQueryBar + TaktToolsBar（:show-expand="false"）
 → TaktMasterDetailTableLr
      左 ~2/5：主表分页 + 行选
      右 ~3/5：#detail → *Panel.vue（子表完整 CRUD 壳）
 → TaktModal + *Form.vue
```

**标准参照**：

| 层 | 文件 |
|----|------|
| 列表壳 | `views/logistics/manufacturing/aps/work-center/index.vue` |
| 右侧从表面板 | `.../work-center/components/WorkCenterResourcePanel.vue` |
| 主表上下文 | `.../work-center/composables/useWorkCenterMasterContext.ts` |
| LR 组件 | `components/business/TaktMasterDetailTableLr/index.vue` |
| 共享 scroll.y | `composables/useTaktMasterDetailLrScrollY.ts`（`scrollLayout="masterDetailLr"`） |

**主表页必备**：

- `provideXxxMasterContext()` → `selectedMasterRow: Ref<MasterDto | null>`
- `v-model:selected-master-key` + `@master-select` → `syncMasterSelection`
- `#detail` 内 panel 根节点：`class="h-full min-h-0 flex-1 flex-col overflow-hidden"`

**从表 panel 必备**：

- `useXxxMasterContext()` → `watch(masterXxxId)` 切换主行时 `reload()`（`pageIndex=1` + `getYyyList`）
- 子表 `getYyyList({ xxxId, pageIndex, pageSize })`；`xxxId` **string**
- 子表 `TaktSingleTable`：`scroll-layout="masterDetailLr"`、`show-pagination="true"`
- `defineExpose({ reload, loadData })`；主表刷新后可选 `panelRef.value?.reload?.()`
- 无选中主行：工具栏 `create-disabled`、空态由 LR 组件 `#detail-empty` 承担

### 1.2 上下布局（TB，列表页内嵌双表）

```text
TaktMasterDetailTableTb
 上 ~45% max-h：主表分页 + 虚拟滚动
 下 flex-1：从表（props 传 detailColumns / detailDataSource，或 detail-* 插槽）
```

**组件**：`components/business/TaktMasterDetailTableTb/index.vue`

- 主表：`showMasterPagination`、超阈值自动 `virtual`
- 从表：默认 `show-pagination="false"` + `virtual`（大数据内嵌展示）
- 适用：从表无需独立 QueryBar/弹窗、列数少、同一 index 内完成主从浏览

与 LR 二选一；**复杂子表 CRUD 仍用 LR + panel**。

### 1.3 弹窗上下（表单级联）

```text
TaktModal → *Form.vue
  上：a-form 主表字段
  下：TaktEditableTable（子表行编辑，随 createXxx/updateXxx 一次提交）
```

**标准参照**：`WorkCenterForm.vue`（生成）、`RoutingForm.vue`（手工）

- `getValues()` 返回主表字段 + `items` / `dictDataList` 等子表数组
- 与后端 `TaktXxxCreateDto` / `UpdateDto` 字段名对齐

---

## 二、后端（与规则文件一致）

**标准参照**：`TaktDictTypeService.cs`（Fill + Save）、`TaktDictDataService.cs`（子表独立分页）

| 能力 | 要求 |
|------|------|
| 主表 list | **禁止**默认加载全量子表 |
| Fill | `FillXxxDetailsAsync` — 仅详情/显式需求 |
| Save | `SaveXxxChildrenAsync` — 先删后插或文档化策略 |
| 子表 list | `GetYyyListAsync`，`QueryDto` 含主表 Id + 分页 clamp |
| 权限 | 主表、子表各自 `领域:…:实体:操作` |

---

## 三、代码生成

| 脚本 | 产出 |
|------|------|
| `generate-vue-master-detail-from-api.cjs` | `index.vue`（**LR** + `*Panel.vue` + `use-*-master-context.ts`）+ `*Form.vue`（**TaktEditableTable** 上下） |
| `generate-vue-master-detail-layout.cjs` | 布局片段（被上者 require） |
| `generate-all.cjs --Xxx` | 实体含 `OneToMany` → 全流程 sub 模板 |

生成后须人工核对：子表 QueryExpression、权限四处一致、`03-format-blank-lines`。

---

## 四、交付顺序

```text
1. 12-crud 基线（主表 + 子表双控制器/双 API）
2. 选定 UI：LR（默认）/ TB / 展开行（特例）
3. LR：master-context + panel + scrollLayout=masterDetailLr
4. 表单：TaktEditableTable 级联或子表独立弹窗
5. 06/07/08 双端分页；外键 string
```

---

## 五、交付清单

```
- [ ] 1. 主表 GetXxxListAsync 不加载全量子表
- [ ] 2. FillXxxDetailsAsync + SaveXxxChildrenAsync（成对、XML 完整）
- [ ] 3. 子表 GetYyyListAsync：QueryDto 含主表 Id + 分页
- [ ] 4. 列表：TaktMasterDetailTableLr（或 Tb）+ :show-expand="false"
- [ ] 5. provide/use-*-master-context；panel watch 主键 reload
- [ ] 6. 子表 getYyyList({ xxxId, pageIndex, pageSize })；panel defineExpose reload
- [ ] 7. 表单 getValues 含子表数组（TaktEditableTable 或 Tab 行编辑）
- [ ] 8. 主键/外键 string；权限主从分离
```

---

## 六、禁止

- 跳过 12-crud；主表 list 塞全量 items
- 子表无独立 list 且无 pageSize 上限
- LR 页仍开 `show-expand` 与右栏 panel 双轨重复加载
- panel 内一次性拉全量子表再在浏览器分页

---

## 七、交叉规则

| 主题 | 文件 |
|------|------|
| CRUD 基线 | [12-crud](../12-crud/SKILL.md) |
| 列表壳分区 | [13-vue-view](../13-vue-view/SKILL.md) §主子表 |
| 表单级联 | [14-vue-form](../14-vue-form/SKILL.md) §主子表 |
| 代码生成 | [15-codegen](../15-codegen/SKILL.md) |
| 溢出联调 | [07-overflow-vue](../07-overflow-vue/SKILL.md)、[08-overflow-fullstack](../08-overflow-fullstack/SKILL.md) |
| 树形父子 | [11-tree-table](../11-tree-table/SKILL.md)（非本章） |
