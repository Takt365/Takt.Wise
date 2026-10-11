# 工作流参照路径

规则全文：`.cursor/rules/09-workflow.mdc`

## 后端

- `backend/src/Takt.Application/Services/Workflow/FlowEngine/TaktFlowEngineService.cs`
- `TaktFlowProcessNavigator.cs`、`TaktFlowConditionEvaluator.cs`、`TaktFlowApproverResolver.cs`
- `TaktFlowProcessModels.cs`
- `backend/src/Takt.WebApi/Controllers/Workflow/TaktFlowEngineController.cs`
- `backend/src/Takt.Shared/Enums/TaktFlowEnums.cs`
- `backend/src/Takt.Domain/Entities/Workflow/`
- `backend/src/Takt.Infrastructure/Data/Seeds/EntitySeedData/Workflow/TaktLeaveWorkflowSeedData.cs`

## 前端

- `frontend/src/components/business/TaktFlowAntDesigner/`
- `frontend/src/components/business/TaktFlowAntDesigner/config/taktFlowTree.ts`
- `frontend/src/components/business/TaktFlowAntDesigner/config/taktFlowDesignValidate.ts`
- `frontend/src/views/workflow/scheme/components/SchemeForm.vue`
- `frontend/src/views/workflow/todo/index.vue`
- `frontend/src/api/workflow/instance.ts`
- `frontend/src/types/workflow/`

## 权限示例

`workflow:todo:list`、`workflow:todo:approve`、`workflow:instance:initiate`、`workflow:instance:withdraw`、`workflow:scheme:design`
