---
name: 09-workflow
description: >-
  实现或审查 Takt 自研审批工作流（AntFlow 树形引擎：表单+节点+权限+条件+异常）。
  用于新建业务流程、改 TaktFlowEngine、流程设计器、待办/发起/驳回/转办/加签、
  workflow 权限种子，或用户提到 09-workflow、工作流/BPM/审批流时。
---

# Takt 审批工作流

完整规范：`.cursor/rules/09-workflow.mdc`。细则变更以 mdc 为准。

## 核心公式

```
审批工作流 = 表单（数据） + 流程节点（路由） + 权限（谁审） + 条件分支 + 异常处理
```

## 主链路

```text
发起申请 → 表单填写 → 校验 → 逐级审批 → (会签/或签) → 终审 → 归档/回写业务系统
                     ↓
              驳回 / 撤回 / 加签 / 转办
```

## 新建业务流程（检查清单）

```
- [ ] 1. 读参照：TaktLeaveWorkflowSeedData、TaktFlowEngineService、taktFlowTree.ts
- [ ] 2. 方案：TaktFlowScheme（ProcessKey、ProcessContent 树）
- [ ] 3. 表单：TaktFlowForm（FormContent）
- [ ] 4. 设计器：TaktFlowAntDesigner 编辑 → taktFlowDesignValidate.ts 通过
- [ ] 5. 发布：SchemeStatus=Published；启动用 ProcessContentSnapshot
- [ ] 6. 菜单/权限：workflow:{page}:{action}，与 TaktFlowEngineController 一致
- [ ] 7. i18n：entity.flow*、menu.workflow.*、locales/workflow/*.page.*
- [ ] 8. 业务集成：BusinessType/BusinessKey + startFlowInstance
- [ ] 9. 回写：Completed/Rejected/Terminated 后在业务 Application 服务更新单据（引擎不自动回写）
- [ ] 10. 列表分页；设计器深度≤10、分支≤20（07-overflow-vue）
- [ ] 11. 空行：03-format-blank-lines
```

## 运行时改动的唯一入口

| 层 | 文件 |
|----|------|
| 引擎 | `backend/.../FlowEngine/TaktFlowEngineService.cs` |
| API | `TaktFlowEngineController.cs`（`api/TaktFlowEngine`） |
| 前端运行时 | `frontend/src/api/workflow/instance.ts`、`views/workflow/todo` |

❌ 业务 Controller 直接改 `TaktFlowTask`  
❌ 在 `TaktFlowSchemesController` 写推进逻辑

## nodeType / setType / signType（必须与前后端一致）

| nodeType | 含义 |
|----------|------|
| 1 | 发起人 |
| 2 | 网关 |
| 3 | 条件分支项 |
| 4 | 审批人 |
| 6 | 抄送（引擎当前跳过） |
| 7 | 并行网关 |

| setType | 审批人 |
|---------|--------|
| 1~6 | 指定成员/主管/角色/部门/发起人/层层审批 |

| signType | 含义 |
|----------|------|
| 1 | 或签 |
| 2 | 会签 |

枚举源：`TaktFlowEnums.cs` ↔ `taktFlowTree.ts`。

## 异常动作映射

| 动作 | API | 权限 |
|------|-----|------|
| 通过 | `complete` approved=true | `workflow:todo:approve` |
| 驳回 | `complete` approved=false + 可选 nodeRejectStep | `workflow:todo:approve` |
| 撤回 | `withdraw` | `workflow:instance:withdraw` |
| 转办 | `transfer` | `workflow:todo:transfer` |
| 加签/减签 | `add-sign` / `reduce-sign` | `workflow:todo:addsign` / `reducesign` |

## 禁止假装已实现

未实现前 **勿** 加 API/按钮/文案：delegate、urge、抄送通知、独立 return API、BPMN/子流程。

退回场景用 **驳回 + nodeRejectStep**。

## 交付前自检

- [ ] `nodeType`/`setType`/`signType` 前后端与枚举一致
- [ ] 权限：种子、`[Authorize]`、`v-permission` 三处一致（新代码用 `workflow:scheme:*` 非 `flowscheme`）
- [ ] 条件分支字段在实例 `FrmData` 中存在
- [ ] 流程 Id 前端类型为 `string`
- [ ] 未完成能力（§十）未写入 UI

详细能力边界与目录结构见 [reference.md](reference.md)。
