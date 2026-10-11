---
name: 02-frontend
description: >-
  新建或审查 Takt 前端页面（Vue3、Ant Design Vue 4、Tailwind、API/types/locales、权限）。
  用于 frontend views/components/api、CRUD 页、i18n、v-permission、
  或用户提到 02-frontend、标准 CRUD 页时。
---

# Takt 前端开发

完整规范：`.cursor/rules/02-frontend.mdc`

## 技术栈（强制）

Vue 3 `<script setup>` + TS | Ant Design Vue 4 `a-*` | Tailwind 4 | Pinia | vue-i18n

❌ 大块 scoped CSS | ❌ 硬编码中文 | ❌ `getList` 无实体前缀

## 目录分层（config / bootstrap / composables）

| 目录 | 职责 | 禁止 |
|------|------|------|
| `config/` | `VITE_*` 解析、OAuth/空闲配置 | Vue 生命周期、Pinia 副作用 |
| `bootstrap/` | `main.ts` 全局注册、登出/空闲/EventBus | 被 `composables/` import |
| `composables/` | 全局 `useXxx`（响应式 + 生命周期） | import `bootstrap/`；纯函数应放 `utils/` |

列表页：`loadData` 定义后调用 `useTableRefresh(loadData)`（与 `bootstrap` 的 `table:refresh` 事件解耦）。

## 标准 CRUD 页（前端专责）

**前置（强制）**：先完成 [12-crud](../12-crud/SKILL.md) 基线清单（页面壳、API 映射、权限）。

```
- [ ] 1. 目录：api/、types/、views/、locales/ 与后端模块一致
- [ ] 2. API：import request from '@/api/request'；getXxxList…；路径段与控制器复数一致（12-crud §三）
- [ ] 3. types：id: string；TaktPagedQuery / TaktPagedResult
- [ ] 4. Vue：8 行 HTML 文件头；分区注释；t('…') 全文案
- [ ] 5. UI：页面壳见 `13-vue-view`；表单见 `14-vue-form`（12-crud §五 摘要）
- [ ] 6. 大列表 virtual；双端分页（07-overflow-vue、08-overflow-fullstack）
- [ ] 7. 权限：v-permission + ToolsBar *-permission
- [ ] 8. locales：export default { page: {…} }；列标题 entity.*
- [ ] 9. loading：async/await + 表格 loading
- [ ] 10. 租户/公司切换：`useTableRefresh(loadData)`（见 `13-vue-view` §2.4）
- [ ] 11. 空行：03-format-blank-lines
```

## 参照模块

| 场景 | 参照 |
|------|------|
| 单表 CRUD | `views/identity/user/index.vue`、`api/identity/user.api.ts`、`components/UserForm.vue` |
| 登录/验证码 | `views/login/index.vue`、`TaktCaptcha*` |
| 紧凑弹窗 | `components/business/TaktModal` |
| 表格组件 | `TaktSingleTable` |
| 全局样式 | `styles/global.css` |
| **全栈 CRUD 映射** | [12-crud](../12-crud/SKILL.md) |

## auto-import 注意

- 勿重复 import：`vue`、`pinia`、`ref`、`computed`（已 auto-import）
- 勿重复 import 已注册组件（`<TaktModal />`）
- **仍须**显式：vue-i18n、message、api、types、composables

## API 与后端对齐

| 前端 | 后端 |
|------|------|
| getUserList | GetUserListAsync |
| TaktUsers（路径） | TaktUsersController |

导入/导出：`responseType: 'blob'`；FormData 导入。完整映射见 `12-crud` §三。

## 交叉规则

- **CRUD 基线（强制）**：`12-crud`
- **代码生成**：`15-codegen`（Vue 步骤见 `13-vue-view` / `14-vue-form`）
- **视图/表单（强制）**：`13-vue-view`、`14-vue-form`
- utils：`05-utils-vue`
- 溢出/虚拟列表/主键：`07-overflow-vue`、`08-overflow-fullstack`
- 主子表：`10-master-detail`（takt-master-detail-table、展开行）
- 树表：`11-tree-table`（takt-tree-left/right-table）
- 工作流页/设计器：`09-workflow`
