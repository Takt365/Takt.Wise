// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Workflow.FlowEngine
// 文件名称：TaktFlowEngineService.cs
// 创建时间：2026-06-03
// 创建人：Takt365(Cursor AI)
// 功能描述：流程引擎运行时（发起、审批、待办、加签、挂起等）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using SqlSugar;
using Takt.Application.Dtos.Workflow;
using Takt.Application.Services.Workflow.FlowEngine.Business;
using Takt.Domain.Entities.Workflow;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Services.Workflow.FlowEngine;

/// <summary>
/// 流程引擎运行时服务（唯一流程推进编排入口）
/// 负责发起/草稿/审批办结、待办与我发起/已办查询、撤回/转办/加签/减签/挂起/恢复/终止/撤销审批
/// 流程定义以启动时 TaktFlowInstance.ProcessContentSnapshot 快照为准；节点路由见 TaktFlowProcessNavigator
/// </summary>
public class TaktFlowEngineService : TaktServiceBase, ITaktFlowEngineService
{
    private readonly ITaktCompanyRepository<TaktFlowInstance> _flowInstanceRepository;
    private readonly ITaktCompanyRepository<TaktFlowScheme> _flowSchemeRepository;
    private readonly ITaktCompanyRepository<TaktFlowTask> _flowTaskRepository;
    private readonly ITaktCompanyRepository<TaktFlowTransition> _flowTransitionRepository;
    private readonly ITaktCompanyRepository<TaktFlowAddSign> _flowAddSignRepository;
    private readonly TaktFlowApproverResolverService _approverResolver;
    private readonly TaktApprovalFlowBusinessService _approvalFlowBusinessService;
    private readonly ITaktApprovalFlowDataGateway _approvalFlowDataGateway;
    private readonly ITaktCompanyRepository<TaktFlowForm> _flowFormRepository;
    private readonly ITaktWorkflowSignalRNotifier _workflowSignalRNotifier;

    /// <summary>
    /// 初始化流程引擎服务
    /// </summary>
    /// <param name="flowInstanceRepository">流程实例仓储</param>
    /// <param name="flowSchemeRepository">流程方案仓储</param>
    /// <param name="flowTaskRepository">流程任务仓储</param>
    /// <param name="flowTransitionRepository">流程流转记录仓储</param>
    /// <param name="flowAddSignRepository">加签记录仓储</param>
    /// <param name="approverResolver">审批人解析服务</param>
    /// <param name="approvalFlowBusinessService">审批业务通用回写</param>
    /// <param name="approvalFlowDataGateway">审批业务表网关</param>
    /// <param name="flowFormRepository">流程表单仓储</param>
    /// <param name="workflowSignalRNotifier">工作流 SignalR 推送编排</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktFlowEngineService(
        ITaktCompanyRepository<TaktFlowInstance> flowInstanceRepository,
        ITaktCompanyRepository<TaktFlowScheme> flowSchemeRepository,
        ITaktCompanyRepository<TaktFlowTask> flowTaskRepository,
        ITaktCompanyRepository<TaktFlowTransition> flowTransitionRepository,
        ITaktCompanyRepository<TaktFlowAddSign> flowAddSignRepository,
        TaktFlowApproverResolverService approverResolver,
        TaktApprovalFlowBusinessService approvalFlowBusinessService,
        ITaktApprovalFlowDataGateway approvalFlowDataGateway,
        ITaktCompanyRepository<TaktFlowForm> flowFormRepository,
        ITaktWorkflowSignalRNotifier workflowSignalRNotifier,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _flowInstanceRepository = flowInstanceRepository;
        _flowSchemeRepository = flowSchemeRepository;
        _flowTaskRepository = flowTaskRepository;
        _flowTransitionRepository = flowTransitionRepository;
        _flowAddSignRepository = flowAddSignRepository;
        _approverResolver = approverResolver;
        _approvalFlowBusinessService = approvalFlowBusinessService;
        _approvalFlowDataGateway = approvalFlowDataGateway;
        _flowFormRepository = flowFormRepository;
        _workflowSignalRNotifier = workflowSignalRNotifier;
    }

    /// <summary>
    /// 发起流程并立即进入运行态
    /// 加载已发布方案、创建 Running 实例、记录「发起」流转并推进至首审批节点
    /// </summary>
    /// <param name="dto">发起参数（ProcessKey、BusinessType/BusinessKey、FrmData 等）</param>
    /// <returns>创建后的实例详情（含任务与流转）</returns>
    /// <exception cref="TaktBusinessException">三层上下文缺失、方案未发布或推进失败时抛出</exception>
    public async Task<TaktFlowInstanceDetailDto> StartFlowInstanceAsync(TaktFlowStartDto dto)
    {
        EnsureThreeLayerContext();
        var scheme = await LoadPublishedSchemeAsync(dto.ProcessKey);
        var userId = GetCurrentUserIdForApproval();
        var UserName = CurrentUserName;
        var instance = await CreateInstanceEntityAsync(scheme, dto, userId, UserName, TaktFlowInstanceStatus.Running);
        await DispatchFlowStartedAndSyncInstanceAsync(instance);
        await RecordTransitionAsync(instance, null, null, "发起", userId, UserName, TaktFlowActionType.Start, null);
        await AdvanceInstanceAsync(instance, null);
        await NotifyWorkflowRuntimeChangeAsync(instance, "start");
        return (await GetFlowInstanceDetailByIdAsync(instance.Id))!;
    }

    /// <summary>
    /// 保存流程草稿（Draft 状态，不创建待办、不推进节点）
    /// </summary>
    /// <param name="dto">与发起相同的表单与业务键参数</param>
    /// <returns>草稿实例详情</returns>
    /// <exception cref="TaktBusinessException">三层上下文缺失或方案未发布时抛出</exception>
    public async Task<TaktFlowInstanceDetailDto> CreateFlowInstanceDraftAsync(TaktFlowStartDto dto)
    {
        EnsureThreeLayerContext();
        var scheme = await LoadPublishedSchemeAsync(dto.ProcessKey);
        var userId = GetCurrentUserIdForApproval();
        var instance = await CreateInstanceEntityAsync(scheme, dto, userId, CurrentUserName, TaktFlowInstanceStatus.Draft);
        return (await GetFlowInstanceDetailByIdAsync(instance.Id))!;
    }

    /// <summary>
    /// 从草稿提交并启动流程（仅发起人、仅 Draft 可提交）
    /// 状态改为 Running 后记录「发起」并推进至首审批节点
    /// </summary>
    /// <param name="instanceId">草稿实例主键 ID</param>
    /// <returns>启动后的实例详情</returns>
    /// <exception cref="TaktBusinessException">非草稿、非发起人或推进失败时抛出</exception>
    public async Task<TaktFlowInstanceDetailDto> StartFlowInstanceFromDraftAsync(long instanceId)
    {
        EnsureThreeLayerContext();
        var instance = await LoadInstanceOrThrowAsync(instanceId);
        if (instance.InstanceStatus != (int)TaktFlowInstanceStatus.Draft)
        {
            ThrowBusinessException("仅草稿状态可提交启动");
        }
        if (instance.StartUserId != GetCurrentUserIdForApproval())
        {
            ThrowBusinessException("仅发起人可提交草稿");
        }
        instance.InstanceStatus = (int)TaktFlowInstanceStatus.Running;
        instance.StartTime = DateTime.Now;
        await _flowInstanceRepository.UpdateAsync(instance);
        await DispatchFlowStartedAndSyncInstanceAsync(instance);
        var userId = GetCurrentUserIdForApproval();
        await RecordTransitionAsync(instance, null, null, "发起", userId, CurrentUserName, TaktFlowActionType.Start, null);
        await AdvanceInstanceAsync(instance, null);
        await NotifyWorkflowRuntimeChangeAsync(instance, "start");
        return (await GetFlowInstanceDetailByIdAsync(instance.Id))!;
    }

    /// <summary>
    /// 办结当前用户待办（通过或驳回）
    /// 通过：处理会签/或签、加签组、并行网关聚合后解析下一节点并创建待办；驳回：按 nodeRejectStep 重入或实例 Rejected
    /// </summary>
    /// <param name="dto">办结参数（FlowInstanceId、Approved、Comment、FrmData、NodeRejectStep 等）</param>
    /// <returns>表示异步办结完成的任务</returns>
    /// <exception cref="TaktBusinessException">无待办、快照无效或状态不允许时抛出</exception>
    public async Task CompleteFlowInstanceTaskAsync(TaktFlowCompleteTaskDto dto)
    {
        EnsureThreeLayerContext();
        var userId = GetCurrentUserIdForApproval();
        var instance = await LoadInstanceOrThrowAsync(dto.FlowInstanceId);
        EnsureInstanceRunnable(instance);
        if (!string.IsNullOrWhiteSpace(dto.FrmData))
        {
            instance.FrmData = dto.FrmData;
            await _flowInstanceRepository.UpdateAsync(instance);
        }
        var pendingTask = await _flowTaskRepository.FirstAsync(x =>
            x.InstanceId == instance.Id
            && x.AssigneeUserId == userId
            && x.TaskStatus == (int)TaktFlowTaskStatus.Pending);
        if (pendingTask == null)
        {
            ThrowBusinessException("当前用户无待办任务");
        }
        var root = TaktFlowProcessNavigator.ParseRoot(instance.ProcessContentSnapshot);
        if (root == null)
        {
            ThrowBusinessException("流程设计快照无效");
        }
        var currentNode = TaktFlowProcessNavigator.FindNode(root, pendingTask.TaskDefinitionKey)
            ?? TaktFlowProcessNavigator.FindNode(root, instance.CurrentActivityId);
        if (!dto.Approved)
        {
            await HandleRejectAsync(instance, pendingTask, currentNode, dto, userId);
            await NotifyWorkflowRuntimeChangeAsync(instance, "reject");
            return;
        }
        pendingTask.TaskStatus = (int)TaktFlowTaskStatus.Completed;
        pendingTask.CompletedAt = DateTime.Now;
        pendingTask.Comment = dto.Comment;
        await _flowTaskRepository.UpdateAsync(pendingTask);
        if (pendingTask.IsAddSign == 1 && pendingTask.AddSignId.HasValue)
        {
            var addHandled = await TryCompleteAddSignGroupAsync(instance, pendingTask);
            if (!addHandled)
            {
                await NotifyWorkflowRuntimeChangeAsync(instance, "complete");
                return;
            }
        }
        if (currentNode != null && !await IsNodeApprovalCompleteAsync(instance, currentNode, pendingTask))
        {
            await NotifyWorkflowRuntimeChangeAsync(instance, "complete");
            return;
        }
        if (currentNode != null)
        {
            var parentParallel = TaktFlowProcessNavigator.FindParentParallelGateway(root, currentNode.NodeId);
            if (parentParallel != null)
            {
                var branchIds = TaktFlowProcessNavigator.CollectParallelApproverNodes(parentParallel)
                    .Select(x => x.NodeId)
                    .ToHashSet(StringComparer.Ordinal);
                var otherPending = await _flowTaskRepository.FirstAsync(x =>
                    x.InstanceId == instance.Id
                    && x.TaskStatus == (int)TaktFlowTaskStatus.Pending
                    && x.IsAddSign == 0
                    && branchIds.Contains(x.TaskDefinitionKey)
                    && x.Id != pendingTask.Id);
                if (otherPending != null)
                {
                    await NotifyWorkflowRuntimeChangeAsync(instance, "complete");
                    return;
                }
            }
        }
        var fromName = instance.CurrentActivityName ?? pendingTask.TaskName;
        await RecordTransitionAsync(
            instance,
            instance.CurrentActivityId,
            fromName,
            dto.Comment,
            userId,
            CurrentUserName,
            TaktFlowActionType.Approve,
            null);
        if (currentNode == null)
        {
            await CompleteInstanceAsync(instance, TaktFlowInstanceStatus.Completed);
            await NotifyWorkflowRuntimeChangeAsync(instance, "complete");
            return;
        }
        var nextNodes = TaktFlowProcessNavigator.ResolveAfterNodeCompleted(root, currentNode, instance.FrmData);
        if (nextNodes.Count == 0)
        {
            if (!await HasOtherPendingTasksAsync(instance.Id))
            {
                await CompleteInstanceAsync(instance, TaktFlowInstanceStatus.Completed);
            }
            await NotifyWorkflowRuntimeChangeAsync(instance, "complete");
            return;
        }
        await EnterExecutionNodesAsync(instance, root, nextNodes);
        await NotifyWorkflowRuntimeChangeAsync(instance, "complete");
    }

    /// <summary>
    /// 获取当前用户待办数量（Running 实例 + Pending 任务）
    /// </summary>
    /// <returns>待办数量 DTO</returns>
    public async Task<TaktFlowTodoCountDto> GetFlowInstanceTodoCountAsync()
    {
        EnsureThreeLayerContext();
        var userId = GetCurrentUserIdForApproval();
        var todoCount = await _workflowSignalRNotifier.CountTodoForUserAsync(
            CurrentTenantCode,
            CurrentCompanyCode,
            userId);
        return new TaktFlowTodoCountDto { TodoCount = todoCount };
    }

    /// <summary>
    /// 获取工作流待办统计（数据看板）
    /// </summary>
    /// <returns>待办统计</returns>
    public async Task<TaktWorkflowTodoStatDto> GetWorkflowTodoStatAsync()
    {
        var count = await GetFlowInstanceTodoCountAsync();
        return new TaktWorkflowTodoStatDto
        {
            PendingTodoCount = count.TodoCount,
        };
    }

    /// <summary>
    /// 获取我发起的流程统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>我发起的流程统计</returns>
    public async Task<TaktWorkflowMyInstanceStatDto> GetWorkflowMyInstanceStatAsync(TaktWorkflowMyInstanceStatQueryDto queryDto)
    {
        EnsureThreeLayerContext();
        var (start, end, statMonth) = TaktStatMonthRangeHelper.ResolveMonthRange(
            queryDto.StartTimeStart,
            queryDto.StartTimeEnd);
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;
        var userId = GetCurrentUserIdForApproval();
        Expression<Func<TaktFlowInstance, bool>> basePredicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.StartUserId == userId
            && ((x.StartTime != null && x.StartTime >= start && x.StartTime <= end)
                || (x.StartTime == null && x.CreatedAt >= start && x.CreatedAt <= end));
        var monthMyInstanceCount = await _flowInstanceRepository.CountAsync(basePredicate);
        Expression<Func<TaktFlowInstance, bool>> runningPredicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.StartUserId == userId
            && x.InstanceStatus == (int)TaktFlowInstanceStatus.Running
            && ((x.StartTime != null && x.StartTime >= start && x.StartTime <= end)
                || (x.StartTime == null && x.CreatedAt >= start && x.CreatedAt <= end));
        var monthMyRunningCount = await _flowInstanceRepository.CountAsync(runningPredicate);
        Expression<Func<TaktFlowInstance, bool>> completedPredicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.StartUserId == userId
            && x.InstanceStatus == (int)TaktFlowInstanceStatus.Completed
            && ((x.StartTime != null && x.StartTime >= start && x.StartTime <= end)
                || (x.StartTime == null && x.CreatedAt >= start && x.CreatedAt <= end));
        var monthMyCompletedCount = await _flowInstanceRepository.CountAsync(completedPredicate);
        return new TaktWorkflowMyInstanceStatDto
        {
            StatMonth = statMonth,
            MonthMyInstanceCount = monthMyInstanceCount,
            MonthMyRunningCount = monthMyRunningCount,
            MonthMyCompletedCount = monthMyCompletedCount,
        };
    }

    /// <summary>
    /// 获取已办任务统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>已办任务统计</returns>
    public async Task<TaktWorkflowProcessedStatDto> GetWorkflowProcessedStatAsync(TaktWorkflowProcessedStatQueryDto queryDto)
    {
        EnsureThreeLayerContext();
        var (start, end, statMonth) = TaktStatMonthRangeHelper.ResolveMonthRange(
            queryDto.CompletedAtStart,
            queryDto.CompletedAtEnd);
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;
        var userId = GetCurrentUserIdForApproval();
        Expression<Func<TaktFlowTask, bool>> predicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.AssigneeUserId == userId
            && x.TaskStatus == (int)TaktFlowTaskStatus.Completed
            && x.CompletedAt != null
            && x.CompletedAt >= start
            && x.CompletedAt <= end;
        var monthProcessedTaskCount = await _flowTaskRepository.CountAsync(predicate);
        return new TaktWorkflowProcessedStatDto
        {
            StatMonth = statMonth,
            MonthProcessedTaskCount = monthProcessedTaskCount,
        };
    }

    /// <summary>
    /// 获取当前用户待办列表（Pending 任务 + Running 实例，内存过滤后客户端分页）
    /// </summary>
    /// <param name="query">关键字/流程名等筛选与分页参数</param>
    /// <returns>待办项分页结果</returns>
    public async Task<TaktPagedResult<TaktFlowTodoItemDto>> GetFlowInstanceTodoListAsync(TaktFlowTodoQueryDto query)
    {
        EnsureThreeLayerContext();
        var userId = GetCurrentUserIdForApproval();
        var tasks = await _flowTaskRepository.GetListAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.AssigneeUserId == userId
            && x.TaskStatus == (int)TaktFlowTaskStatus.Pending);
        var instanceIds = tasks.Select(x => x.InstanceId).Distinct().ToList();
        if (instanceIds.Count == 0)
        {
            return TaktPagedResult<TaktFlowTodoItemDto>.Create(new List<TaktFlowTodoItemDto>(), 0, query.PageIndex, query.PageSize);
        }
        var instances = await _flowInstanceRepository.GetListAsync(x => instanceIds.Contains(x.Id));
        var instanceMap = instances.ToDictionary(x => x.Id);
        var items = new List<TaktFlowTodoItemDto>();
        foreach (var task in tasks.OrderByDescending(x => x.CreatedAt))
        {
            if (!instanceMap.TryGetValue(task.InstanceId, out var inst))
            {
                continue;
            }
            if (inst.InstanceStatus != (int)TaktFlowInstanceStatus.Running)
            {
                continue;
            }
            if (!MatchesTodoQuery(inst, task, query))
            {
                continue;
            }
            items.Add(new TaktFlowTodoItemDto
            {
                FlowInstanceId = inst.Id,
                InstanceCode = inst.InstanceCode,
                ProcessName = inst.ProcessName,
                ProcessTitle = inst.ProcessTitle,
                TaskName = task.TaskName ?? inst.CurrentActivityName,
                StartUserName = inst.StartUserName,
                StartTime = inst.StartTime ?? default,
                FlowTaskId = task.Id
            });
        }
        var page = items
            .Skip((query.PageIndex - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();
        return TaktPagedResult<TaktFlowTodoItemDto>.Create(page, items.Count, query.PageIndex, query.PageSize);
    }

    /// <summary>
    /// 获取当前用户发起的流程实例列表（服务端分页，按租户/公司隔离）
    /// </summary>
    /// <param name="query">状态/关键字等筛选与分页参数</param>
    /// <returns>实例摘要分页结果</returns>
    public async Task<TaktPagedResult<TaktFlowInstanceListItemDto>> GetFlowInstanceMyListAsync(TaktFlowTodoQueryDto query)
    {
        EnsureThreeLayerContext();
        var userId = GetCurrentUserIdForApproval();
        var exp = Expressionable.Create<TaktFlowInstance>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.StartUserId == userId);
        exp = ApplyFlowInstanceListQuery(exp, query);
        var (data, total) = await _flowInstanceRepository.GetPagedAsync(query.PageIndex, query.PageSize, exp.ToExpression());
        var list = data.Select(MapListItem).ToList();
        return TaktPagedResult<TaktFlowInstanceListItemDto>.Create(list, total, query.PageIndex, query.PageSize);
    }

    /// <summary>
    /// 获取当前用户已办流程列表（Completed 任务关联实例，内存过滤后客户端分页）
    /// </summary>
    /// <param name="query">与待办共用的筛选与分页参数</param>
    /// <returns>已办实例摘要分页结果</returns>
    public async Task<TaktPagedResult<TaktFlowInstanceListItemDto>> GetFlowInstanceProcessedListAsync(TaktFlowTodoQueryDto query)
    {
        EnsureThreeLayerContext();
        var userId = GetCurrentUserIdForApproval();
        var doneTasks = await _flowTaskRepository.GetListAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.AssigneeUserId == userId
            && x.TaskStatus == (int)TaktFlowTaskStatus.Completed);
        var instanceIds = doneTasks.Select(x => x.InstanceId).Distinct().ToList();
        if (instanceIds.Count == 0)
        {
            return TaktPagedResult<TaktFlowInstanceListItemDto>.Create(new List<TaktFlowInstanceListItemDto>(), 0, query.PageIndex, query.PageSize);
        }
        var instances = await _flowInstanceRepository.GetListAsync(x => instanceIds.Contains(x.Id));
        var items = instances
            .Where(x => MatchesTodoQuery(x, null, query))
            .OrderByDescending(x => x.UpdatedAt)
            .Select(MapListItem)
            .ToList();
        var page = items.Skip((query.PageIndex - 1) * query.PageSize).Take(query.PageSize).ToList();
        return TaktPagedResult<TaktFlowInstanceListItemDto>.Create(page, items.Count, query.PageIndex, query.PageSize);
    }

    /// <summary>
    /// 按 ID 获取流程实例详情（含任务、流转历史；须属于当前租户与公司）
    /// </summary>
    /// <param name="instanceId">实例主键 ID</param>
    /// <returns>详情 DTO；不存在或越权时返回 null</returns>
    public async Task<TaktFlowInstanceDetailDto?> GetFlowInstanceDetailByIdAsync(long instanceId)
    {
        EnsureThreeLayerContext();
        var instance = await _flowInstanceRepository.GetByIdAsync(instanceId);
        if (instance == null || instance.TenantCode != CurrentTenantCode || instance.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return await MapDetailAsync(instance);
    }

    /// <summary>
    /// 撤回流程（仅发起人、仅 Running 可撤回）
    /// 取消全部待办并将实例置为 Terminated，记录 Revoke 流转
    /// </summary>
    /// <param name="instanceCode">实例业务编码</param>
    /// <returns>表示异步撤回完成的任务</returns>
    /// <exception cref="TaktBusinessException">实例不存在、非发起人或状态不允许时抛出</exception>
    public async Task RevokeFlowInstanceAsync(string instanceCode)
    {
        EnsureThreeLayerContext();
        var userId = GetCurrentUserIdForApproval();
        var instance = await _flowInstanceRepository.FirstAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.InstanceCode == instanceCode);
        if (instance == null)
        {
            ThrowBusinessException("流程实例不存在");
        }
        if (instance.StartUserId != userId)
        {
            ThrowBusinessException("仅发起人可撤回");
        }
        if (instance.InstanceStatus != (int)TaktFlowInstanceStatus.Running)
        {
            ThrowBusinessException("仅运行中流程可撤回");
        }
        await CancelPendingTasksAsync(instance.Id);
        instance.InstanceStatus = (int)TaktFlowInstanceStatus.Terminated;
        instance.DeleteReason = "发起人撤回";
        instance.EndTime = DateTime.Now;
        instance.DurationMs = (long)((instance.EndTime!.Value - (instance.StartTime ?? instance.EndTime.Value)).TotalMilliseconds);
        await _flowInstanceRepository.UpdateAsync(instance);
        await RecordTransitionAsync(instance, instance.CurrentActivityId, instance.CurrentActivityName, "撤回", userId, CurrentUserName, TaktFlowActionType.Revoke, null);
        await DispatchFlowTerminalAsync(instance);
        await NotifyWorkflowRuntimeChangeAsync(instance, "revoke");
    }

    /// <summary>
    /// 转办当前用户待办至指定用户（原办理人写入 OwnerUserId）
    /// </summary>
    /// <param name="dto">转办参数（FlowInstanceId、ToUserId、ToUserName、Comment）</param>
    /// <returns>表示异步转办完成的任务</returns>
    /// <exception cref="TaktBusinessException">实例不可运行或无当前用户待办时抛出</exception>
    public async Task TransferFlowInstanceAsync(TaktFlowTransferDto dto)
    {
        EnsureThreeLayerContext();
        var userId = GetCurrentUserIdForApproval();
        var instance = await LoadInstanceOrThrowAsync(dto.FlowInstanceId);
        EnsureInstanceRunnable(instance);
        var task = await _flowTaskRepository.FirstAsync(x =>
            x.InstanceId == instance.Id
            && x.AssigneeUserId == userId
            && x.TaskStatus == (int)TaktFlowTaskStatus.Pending);
        if (task == null)
        {
            ThrowBusinessException("当前用户无待办任务");
        }
        task.OwnerUserId = userId;
        task.OwnerUserName = task.AssigneeUserName ?? CurrentUserName;
        task.AssigneeUserId = dto.ToUserId;
        task.AssigneeUserName = dto.ToUserName;
        await _flowTaskRepository.UpdateAsync(task);
        await RecordTransitionAsync(instance, instance.CurrentActivityId, instance.CurrentActivityName, dto.Comment, userId, CurrentUserName, TaktFlowActionType.Transfer, dto.ToUserName);
        await NotifyWorkflowRuntimeChangeAsync(
            instance,
            "transfer",
            new[]
            {
                new TaktWorkflowSignalRUserTarget { UserId = userId, UserName = CurrentUserName ?? string.Empty },
                new TaktWorkflowSignalRUserTarget { UserId = dto.ToUserId, UserName = dto.ToUserName ?? string.Empty },
            });
    }

    /// <summary>
    /// 在当前节点加签审批人（sequential / all / one 模式）
    /// 创建 TaktFlowAddSign 与加签待办任务，并记录 AddSign 流转
    /// </summary>
    /// <param name="dto">加签参数（审批人列表、ApproveType、ReturnToSignNode、Reason）</param>
    /// <returns>表示异步加签完成的任务</returns>
    /// <exception cref="TaktBusinessException">实例不可运行时抛出</exception>
    public async Task AddFlowInstanceApproversAsync(TaktFlowAddApproversDto dto)
    {
        EnsureThreeLayerContext();
        var userId = GetCurrentUserIdForApproval();
        var instance = await LoadInstanceOrThrowAsync(dto.FlowInstanceId);
        EnsureInstanceRunnable(instance);
        var nodeId = instance.CurrentActivityId ?? string.Empty;
        var signType = string.IsNullOrWhiteSpace(dto.ApproveType) ? "sequential" : dto.ApproveType.Trim().ToLowerInvariant();
        var sort = 0;
        foreach (var approver in dto.Approvers)
        {
            var addSign = new TaktFlowAddSign
            {
                InstanceId = instance.Id,
                NodeId = nodeId,
                SignUserId = approver.ApproverUserId,
                SignUserName = approver.ApproverUserName,
                SignType = signType,
                ReturnToSignNode = dto.ReturnToSignNode ? 1 : 0,
                Reason = dto.Reason,
                IsHandled = 0
            };
            addSign = await _flowAddSignRepository.CreateAsync(addSign);
            var isPending = signType == "all" || signType == "one" || sort == 0;
            var task = new TaktFlowTask
            {
                InstanceId = instance.Id,
                TaskDefinitionKey = nodeId,
                TaskName = instance.CurrentActivityName,
                AssigneeUserId = approver.ApproverUserId,
                AssigneeUserName = approver.ApproverUserName,
                TaskStatus = isPending ? (int)TaktFlowTaskStatus.Pending : (int)TaktFlowTaskStatus.Cancelled,
                SignType = signType == "all" ? (int)TaktFlowSignType.All : (int)TaktFlowSignType.Any,
                IsAddSign = 1,
                AddSignId = addSign.Id,
                SortOrder = sort++
            };
            if (!isPending)
            {
                task.TaskStatus = (int)TaktFlowTaskStatus.Cancelled;
            }
            await _flowTaskRepository.CreateAsync(task);
        }
        await RecordTransitionAsync(instance, nodeId, instance.CurrentActivityName, dto.Reason, userId, CurrentUserName, TaktFlowActionType.AddSign, null);
        await NotifyWorkflowRuntimeChangeAsync(instance, "addsign");
    }

    /// <summary>
    /// 减签：取消指定加签记录及其 Pending 待办，标记加签已处理并记录 ReduceSign 流转
    /// </summary>
    /// <param name="dto">减签参数（FlowInstanceId、FlowAddSignId）</param>
    /// <returns>表示异步减签完成的任务</returns>
    /// <exception cref="TaktBusinessException">加签记录不存在或不属于该实例时抛出</exception>
    public async Task ReduceFlowInstanceApprovalAsync(TaktFlowReduceApprovalDto dto)
    {
        EnsureThreeLayerContext();
        var userId = GetCurrentUserIdForApproval();
        var instance = await LoadInstanceOrThrowAsync(dto.FlowInstanceId);
        var addSign = await _flowAddSignRepository.GetByIdAsync(dto.FlowAddSignId);
        if (addSign == null || addSign.InstanceId != instance.Id)
        {
            ThrowBusinessException("加签记录不存在");
        }
        addSign.IsHandled = 1;
        await _flowAddSignRepository.UpdateAsync(addSign);
        var tasks = await _flowTaskRepository.GetListAsync(x =>
            x.InstanceId == instance.Id && x.AddSignId == addSign.Id);
        foreach (var task in tasks)
        {
            if (task.TaskStatus == (int)TaktFlowTaskStatus.Pending)
            {
                task.TaskStatus = (int)TaktFlowTaskStatus.Cancelled;
                await _flowTaskRepository.UpdateAsync(task);
            }
        }
        await RecordTransitionAsync(instance, instance.CurrentActivityId, instance.CurrentActivityName, "减签", userId, CurrentUserName, TaktFlowActionType.ReduceSign, addSign.SignUserName);
        await NotifyWorkflowRuntimeChangeAsync(instance, "reducesign");
    }

    /// <summary>
    /// 挂起运行中流程（Running → Suspended，写入挂起原因）
    /// </summary>
    /// <param name="dto">操作参数（FlowInstanceId、Reason）</param>
    /// <returns>表示异步挂起完成的任务</returns>
    /// <exception cref="TaktBusinessException">非 Running 状态时抛出</exception>
    public async Task SuspendFlowInstanceAsync(TaktFlowInstanceOperateDto dto)
    {
        EnsureThreeLayerContext();
        var instance = await LoadInstanceOrThrowAsync(dto.FlowInstanceId);
        if (instance.InstanceStatus != (int)TaktFlowInstanceStatus.Running)
        {
            ThrowBusinessException("仅运行中流程可挂起");
        }
        instance.InstanceStatus = (int)TaktFlowInstanceStatus.Suspended;
        instance.DeleteReason = dto.Reason;
        await _flowInstanceRepository.UpdateAsync(instance);
        await RecordTransitionAsync(instance, instance.CurrentActivityId, instance.CurrentActivityName, dto.Reason, GetCurrentUserIdForApproval(), CurrentUserName, TaktFlowActionType.Suspend, null);
        await NotifyWorkflowRuntimeChangeAsync(instance, "suspend");
    }

    /// <summary>
    /// 恢复已挂起流程（Suspended → Running，清除挂起原因）
    /// </summary>
    /// <param name="dto">操作参数（FlowInstanceId、Reason）</param>
    /// <returns>表示异步恢复完成的任务</returns>
    /// <exception cref="TaktBusinessException">非 Suspended 状态时抛出</exception>
    public async Task ResumeFlowInstanceAsync(TaktFlowInstanceOperateDto dto)
    {
        EnsureThreeLayerContext();
        var instance = await LoadInstanceOrThrowAsync(dto.FlowInstanceId);
        if (instance.InstanceStatus != (int)TaktFlowInstanceStatus.Suspended)
        {
            ThrowBusinessException("仅已挂起流程可恢复");
        }
        instance.InstanceStatus = (int)TaktFlowInstanceStatus.Running;
        instance.DeleteReason = null;
        await _flowInstanceRepository.UpdateAsync(instance);
        await RecordTransitionAsync(instance, instance.CurrentActivityId, instance.CurrentActivityName, dto.Reason, GetCurrentUserIdForApproval(), CurrentUserName, TaktFlowActionType.Resume, null);
        await NotifyWorkflowRuntimeChangeAsync(instance, "resume");
    }

    /// <summary>
    /// 终止流程（取消全部待办，实例置 Terminated 并计算耗时，记录 Terminate 流转）
    /// </summary>
    /// <param name="dto">操作参数（FlowInstanceId、Reason）</param>
    /// <returns>表示异步终止完成的任务</returns>
    public async Task TerminateFlowInstanceAsync(TaktFlowInstanceOperateDto dto)
    {
        EnsureThreeLayerContext();
        var instance = await LoadInstanceOrThrowAsync(dto.FlowInstanceId);
        await CancelPendingTasksAsync(instance.Id);
        instance.InstanceStatus = (int)TaktFlowInstanceStatus.Terminated;
        instance.DeleteReason = dto.Reason;
        instance.EndTime = DateTime.Now;
        instance.DurationMs = (long)((instance.EndTime!.Value - (instance.StartTime ?? instance.EndTime.Value)).TotalMilliseconds);
        await _flowInstanceRepository.UpdateAsync(instance);
        await RecordTransitionAsync(instance, instance.CurrentActivityId, instance.CurrentActivityName, dto.Reason, GetCurrentUserIdForApproval(), CurrentUserName, TaktFlowActionType.Terminate, null);
        await DispatchFlowTerminalAsync(instance);
        await NotifyWorkflowRuntimeChangeAsync(instance, "terminate");
    }

    /// <summary>
    /// 撤销当前用户最近一次已办审批（Completed → Pending，回退实例当前节点并删除对应 Approve 流转）
    /// </summary>
    /// <param name="dto">操作参数（FlowInstanceId）</param>
    /// <returns>表示异步撤销完成的任务</returns>
    /// <exception cref="TaktBusinessException">非 Running 或无已办任务时抛出</exception>
    public async Task UndoFlowInstanceVerificationAsync(TaktFlowInstanceOperateDto dto)
    {
        EnsureThreeLayerContext();
        var userId = GetCurrentUserIdForApproval();
        var instance = await LoadInstanceOrThrowAsync(dto.FlowInstanceId);
        if (instance.InstanceStatus != (int)TaktFlowInstanceStatus.Running)
        {
            ThrowBusinessException("仅运行中流程可撤销审批");
        }
        var lastTask = await _flowTaskRepository.FirstAsync(x =>
            x.InstanceId == instance.Id
            && x.AssigneeUserId == userId
            && x.TaskStatus == (int)TaktFlowTaskStatus.Completed
            && x.IsAddSign == 0);
        if (lastTask == null)
        {
            ThrowBusinessException("未找到可撤销的已办任务");
        }
        lastTask.TaskStatus = (int)TaktFlowTaskStatus.Pending;
        lastTask.CompletedAt = null;
        await _flowTaskRepository.UpdateAsync(lastTask);
        instance.CurrentActivityId = lastTask.TaskDefinitionKey;
        instance.CurrentActivityName = lastTask.TaskName;
        await _flowInstanceRepository.UpdateAsync(instance);
        var transitions = await _flowTransitionRepository.GetListAsync(
            x => x.InstanceId == instance.Id,
            x => x.TransitionTime,
            true);
        var lastTransition = transitions.FirstOrDefault(x => x.ActionType == (int)TaktFlowActionType.Approve && x.TransitionUserId == userId);
        if (lastTransition != null)
        {
            await _flowTransitionRepository.DeleteAsync(lastTransition.Id);
        }
        await NotifyWorkflowRuntimeChangeAsync(instance, "undo");
    }

    /// <summary>
    /// 可发起流程方案列表（已发布、最新版、未挂起）
    /// </summary>
    /// <returns>方案摘要列表</returns>
    public async Task<List<TaktFlowStartableSchemeDto>> GetStartableSchemeListAsync()
    {
        EnsureThreeLayerContext();
        var schemes = await _flowSchemeRepository.GetListAsync(s =>
            s.TenantCode == CurrentTenantCode
            && s.CompanyCode == CurrentCompanyCode
            && s.IsLatest == 1
            && s.ProcessStatus == 1
            && s.SuspensionState == (int)TaktFlowSuspensionState.Active);
        var ordered = schemes.OrderBy(s => s.SortOrder).ThenBy(s => s.ProcessKey).ToList();
        var formIds = ordered.Where(s => s.FormId.HasValue).Select(s => s.FormId!.Value).Distinct().ToList();
        var forms = await _flowFormRepository.GetListAsync(f => formIds.Contains(f.Id));
        var formMap = forms.ToDictionary(f => f.Id);
        return ordered.Select(s =>
        {
            var dto = new TaktFlowStartableSchemeDto
            {
                ProcessKey = s.ProcessKey,
                ProcessName = s.ProcessName,
                FormId = s.FormId.GetValueOrDefault(),
                FormCode = s.FormCode ?? string.Empty
            };
            if (s.FormId.HasValue && formMap.TryGetValue(s.FormId.Value, out var form))
            {
                dto.IsDatasource = form.IsDatasource;
                dto.RelatedTableName = form.RelatedTableName;
            }
            return dto;
        }).ToList();
    }

    /// <summary>
    /// 审批业务表白名单
    /// </summary>
    /// <returns>物理表名列表</returns>
    public IReadOnlyList<string> GetApprovalFlowTableNames()
    {
        return _approvalFlowDataGateway.GetAllowedTableNames();
    }

    /// <summary>
    /// 推进流程到下一执行节点
    /// </summary>
    /// <param name="instance">实例</param>
    /// <param name="fromNodeId">来源节点</param>
    /// <returns>任务</returns>
    private async Task AdvanceInstanceAsync(TaktFlowInstance instance, string? fromNodeId)
    {
        var root = TaktFlowProcessNavigator.ParseRoot(instance.ProcessContentSnapshot);
        if (root == null)
        {
            ThrowBusinessException("流程设计快照无效");
        }
        var nextNodes = TaktFlowProcessNavigator.ResolveNextExecutionNodes(root, fromNodeId, instance.FrmData);
        if (nextNodes.Count == 0)
        {
            await CompleteInstanceAsync(instance, TaktFlowInstanceStatus.Completed);
            return;
        }
        await EnterExecutionNodesAsync(instance, root, nextNodes);
    }

    /// <summary>
    /// 进入执行节点并创建待办
    /// </summary>
    /// <param name="instance">实例</param>
    /// <param name="root">流程根</param>
    /// <param name="nodes">节点列表</param>
    /// <returns>任务</returns>
    private async Task EnterExecutionNodesAsync(TaktFlowInstance instance, TaktFlowTreeNode root, List<TaktFlowTreeNode> nodes)
    {
        if (nodes.Count == 0)
        {
            await CompleteInstanceAsync(instance, TaktFlowInstanceStatus.Completed);
            return;
        }
        if (nodes.Count == 1)
        {
            await EnterApproverNodeAsync(instance, nodes[0]);
            return;
        }
        var parallelGateway = TaktFlowProcessNavigator.FindParentParallelGateway(root, nodes[0].NodeId);
        instance.CurrentActivityId = parallelGateway?.NodeId ?? nodes[0].NodeId;
        instance.CurrentActivityName = parallelGateway != null
            ? TaktFlowProcessNavigator.GetNodeDisplayName(parallelGateway)
            : TaktFlowProcessNavigator.GetNodeDisplayName(nodes[0]);
        await _flowInstanceRepository.UpdateAsync(instance);
        foreach (var node in nodes)
        {
            await CreateTasksForApproverNodeAsync(instance, node, activateAll: true);
        }
    }

    /// <summary>
    /// 进入单个审批节点
    /// </summary>
    /// <param name="instance">实例</param>
    /// <param name="node">审批节点</param>
    /// <returns>任务</returns>
    private async Task EnterApproverNodeAsync(TaktFlowInstance instance, TaktFlowTreeNode node)
    {
        instance.CurrentActivityId = node.NodeId;
        instance.CurrentActivityName = TaktFlowProcessNavigator.GetNodeDisplayName(node);
        await _flowInstanceRepository.UpdateAsync(instance);
        var setType = node.SetType ?? 1;
        var sequentialChain = setType == 6;
        await CreateTasksForApproverNodeAsync(instance, node, activateAll: !sequentialChain, onlyFirstForChain: sequentialChain);
    }

    /// <summary>
    /// 为审批节点创建任务
    /// </summary>
    /// <param name="instance">实例</param>
    /// <param name="node">节点</param>
    /// <param name="activateAll">是否全部激活</param>
    /// <param name="onlyFirstForChain">层层审批仅激活首个</param>
    /// <returns>任务</returns>
    private async Task CreateTasksForApproverNodeAsync(
        TaktFlowInstance instance,
        TaktFlowTreeNode node,
        bool activateAll,
        bool onlyFirstForChain = false)
    {
        var approvers = await _approverResolver.ResolveApproversAsync(
            node,
            instance.StartUserId,
            instance.TenantCode,
            instance.CompanyCode);
        if (approvers.Count == 0)
        {
            ThrowBusinessException($"节点「{TaktFlowProcessNavigator.GetNodeDisplayName(node)}」未解析到审批人");
        }
        var signType = node.SignType == 2 ? TaktFlowSignType.All : TaktFlowSignType.Any;
        var sort = 0;
        foreach (var approver in approvers)
        {
            var active = activateAll || (onlyFirstForChain && sort == 0);
            var task = new TaktFlowTask
            {
                InstanceId = instance.Id,
                TaskDefinitionKey = node.NodeId,
                TaskName = TaktFlowProcessNavigator.GetNodeDisplayName(node),
                AssigneeUserId = approver.UserId,
                AssigneeUserName = approver.UserName,
                TaskStatus = active ? (int)TaktFlowTaskStatus.Pending : (int)TaktFlowTaskStatus.Cancelled,
                SignType = (int)signType,
                SortOrder = sort++
            };
            await _flowTaskRepository.CreateAsync(task);
        }
    }

    /// <summary>
    /// 判断节点审批是否已全部完成
    /// </summary>
    /// <param name="instance">实例</param>
    /// <param name="node">节点</param>
    /// <param name="completedTask">刚完成的任务</param>
    /// <returns>是否可流转</returns>
    private async Task<bool> IsNodeApprovalCompleteAsync(TaktFlowInstance instance, TaktFlowTreeNode node, TaktFlowTask completedTask)
    {
        var nodeTasks = await _flowTaskRepository.GetListAsync(x =>
            x.InstanceId == instance.Id
            && x.TaskDefinitionKey == node.NodeId
            && x.IsAddSign == 0);
        var pending = nodeTasks.Where(x => x.TaskStatus == (int)TaktFlowTaskStatus.Pending).ToList();
        if (node.SignType == 2)
        {
            return pending.Count == 0;
        }
        foreach (var other in pending)
        {
            other.TaskStatus = (int)TaktFlowTaskStatus.Cancelled;
            await _flowTaskRepository.UpdateAsync(other);
        }
        return true;
    }

    /// <summary>
    /// 处理加签组完成
    /// </summary>
    /// <param name="instance">实例</param>
    /// <param name="task">任务</param>
    /// <returns>是否继续主流程</returns>
    private async Task<bool> TryCompleteAddSignGroupAsync(TaktFlowInstance instance, TaktFlowTask task)
    {
        var addSign = await _flowAddSignRepository.GetByIdAsync(task.AddSignId!.Value);
        if (addSign == null)
        {
            return true;
        }
        var groupTasks = await _flowTaskRepository.GetListAsync(x =>
            x.InstanceId == instance.Id && x.AddSignId == addSign.Id);
        var signType = addSign.SignType;
        if (signType == "one")
        {
            foreach (var other in groupTasks.Where(x => x.Id != task.Id && x.TaskStatus == (int)TaktFlowTaskStatus.Pending))
            {
                other.TaskStatus = (int)TaktFlowTaskStatus.Cancelled;
                await _flowTaskRepository.UpdateAsync(other);
            }
            addSign.IsHandled = 1;
            await _flowAddSignRepository.UpdateAsync(addSign);
            return addSign.ReturnToSignNode != 1;
        }
        if (signType == "all")
        {
            if (groupTasks.Any(x => x.TaskStatus == (int)TaktFlowTaskStatus.Pending))
            {
                return false;
            }
            addSign.IsHandled = 1;
            await _flowAddSignRepository.UpdateAsync(addSign);
            return addSign.ReturnToSignNode != 1;
        }
        var next = groupTasks
            .Where(x => x.TaskStatus == (int)TaktFlowTaskStatus.Cancelled)
            .OrderBy(x => x.SortOrder)
            .FirstOrDefault();
        if (next != null)
        {
            next.TaskStatus = (int)TaktFlowTaskStatus.Pending;
            await _flowTaskRepository.UpdateAsync(next);
            return false;
        }
        addSign.IsHandled = 1;
        await _flowAddSignRepository.UpdateAsync(addSign);
        return addSign.ReturnToSignNode != 1;
    }

    /// <summary>
    /// 驳回处理
    /// </summary>
    private async Task HandleRejectAsync(
        TaktFlowInstance instance,
        TaktFlowTask pendingTask,
        TaktFlowTreeNode? currentNode,
        TaktFlowCompleteTaskDto dto,
        long userId)
    {
        pendingTask.TaskStatus = (int)TaktFlowTaskStatus.Completed;
        pendingTask.CompletedAt = DateTime.Now;
        pendingTask.Comment = dto.Comment;
        await _flowTaskRepository.UpdateAsync(pendingTask);
        await CancelPendingTasksAsync(instance.Id, pendingTask.Id);
        await RecordTransitionAsync(
            instance,
            instance.CurrentActivityId,
            instance.CurrentActivityName,
            dto.Comment,
            userId,
            CurrentUserName,
            TaktFlowActionType.Reject,
            dto.NodeRejectStep);
        var root = TaktFlowProcessNavigator.ParseRoot(instance.ProcessContentSnapshot);
        if (root != null && !string.IsNullOrWhiteSpace(dto.NodeRejectStep))
        {
            var rejectNode = TaktFlowProcessNavigator.FindNode(root, dto.NodeRejectStep);
            if (rejectNode != null)
            {
                instance.InstanceStatus = (int)TaktFlowInstanceStatus.Running;
                await EnterApproverNodeAsync(instance, rejectNode);
                return;
            }
        }
        instance.InstanceStatus = (int)TaktFlowInstanceStatus.Rejected;
        instance.EndTime = DateTime.Now;
        instance.DurationMs = (long)((instance.EndTime!.Value - (instance.StartTime ?? instance.EndTime.Value)).TotalMilliseconds);
        await _flowInstanceRepository.UpdateAsync(instance);
        await DispatchFlowTerminalAsync(instance);
    }

    /// <summary>
    /// 完成流程实例
    /// </summary>
    /// <param name="instance">实例</param>
    /// <param name="status">终态</param>
    /// <returns>任务</returns>
    private async Task CompleteInstanceAsync(TaktFlowInstance instance, TaktFlowInstanceStatus status)
    {
        await CancelPendingTasksAsync(instance.Id);
        instance.InstanceStatus = (int)status;
        instance.EndTime = DateTime.Now;
        instance.CurrentActivityId = string.Empty;
        instance.CurrentActivityName = string.Empty;
        instance.DurationMs = (long)((instance.EndTime!.Value - (instance.StartTime ?? instance.EndTime.Value)).TotalMilliseconds);
        await _flowInstanceRepository.UpdateAsync(instance);
        await DispatchFlowTerminalAsync(instance);
    }

    /// <summary>
    /// 推送工作流运行时 SignalR 变更（失败抛异常，阻断主流程）
    /// </summary>
    /// <param name="instance">流程实例</param>
    /// <param name="actionType">动作类型</param>
    /// <param name="additionalUsers">额外通知用户</param>
    /// <returns>任务</returns>
    /// <exception cref="TaktBusinessException">实例不存在或推送失败时抛出</exception>
    private async Task NotifyWorkflowRuntimeChangeAsync(
        TaktFlowInstance instance,
        string actionType,
        IReadOnlyCollection<TaktWorkflowSignalRUserTarget>? additionalUsers = null)
    {
        var fresh = await _flowInstanceRepository.GetByIdAsync(instance.Id);
        if (fresh == null)
        {
            ThrowBusinessException("流程实例不存在，无法推送工作流实时通知");
        }

        await _workflowSignalRNotifier.NotifyInstanceProgressedAsync(fresh, actionType, additionalUsers);
    }

    /// <summary>
    /// 流程启动后分发业务回调并回写 BusinessKey 等变更
    /// </summary>
    /// <param name="instance">流程实例</param>
    /// <returns>异步任务</returns>
    private async Task DispatchFlowStartedAndSyncInstanceAsync(TaktFlowInstance instance)
    {
        await _approvalFlowBusinessService.OnFlowStartedAsync(instance);
        await _flowInstanceRepository.UpdateAsync(instance);
    }

    /// <summary>
    /// 流程终态业务回写
    /// </summary>
    /// <param name="instance">流程实例</param>
    /// <returns>异步任务</returns>
    private async Task DispatchFlowTerminalAsync(TaktFlowInstance instance)
    {
        await _approvalFlowBusinessService.OnFlowTerminalAsync(instance);
    }

    /// <summary>
    /// 取消实例待办
    /// </summary>
    /// <param name="instanceId">实例 ID</param>
    /// <param name="exceptTaskId">排除任务</param>
    /// <returns>任务</returns>
    private async Task CancelPendingTasksAsync(long instanceId, long? exceptTaskId = null)
    {
        var pending = await _flowTaskRepository.GetListAsync(x =>
            x.InstanceId == instanceId && x.TaskStatus == (int)TaktFlowTaskStatus.Pending);
        foreach (var task in pending)
        {
            if (exceptTaskId.HasValue && task.Id == exceptTaskId.Value)
            {
                continue;
            }
            task.TaskStatus = (int)TaktFlowTaskStatus.Cancelled;
            await _flowTaskRepository.UpdateAsync(task);
        }
    }

    /// <summary>
    /// 是否仍有其他待办
    /// </summary>
    /// <param name="instanceId">实例 ID</param>
    /// <returns>是否存在</returns>
    private async Task<bool> HasOtherPendingTasksAsync(long instanceId)
    {
        return await _flowTaskRepository.FirstAsync(x =>
            x.InstanceId == instanceId && x.TaskStatus == (int)TaktFlowTaskStatus.Pending) != null;
    }

    /// <summary>
    /// 记录流转历史
    /// </summary>
    private async Task RecordTransitionAsync(
        TaktFlowInstance instance,
        string? fromNodeId,
        string? fromNodeName,
        string? comment,
        long userId,
        string? UserName,
        TaktFlowActionType actionType,
        string? toNodeNameOverride)
    {
        var transition = new TaktFlowTransition
        {
            InstanceId = instance.Id,
            FromNodeId = fromNodeId,
            FromNodeName = fromNodeName,
            ToNodeId = instance.CurrentActivityId,
            ToNodeName = toNodeNameOverride ?? instance.CurrentActivityName,
            TransitionUserId = userId,
            TransitionUserName = UserName,
            TransitionTime = DateTime.Now,
            TransitionComment = comment,
            ActionType = (int)actionType
        };
        await _flowTransitionRepository.CreateAsync(transition);
    }

    /// <summary>
    /// 加载已发布流程定义
    /// </summary>
    /// <param name="processKey">流程键</param>
    /// <returns>定义</returns>
    private async Task<TaktFlowScheme> LoadPublishedSchemeAsync(string processKey)
    {
        var scheme = await _flowSchemeRepository.FirstAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.ProcessKey == processKey
            && x.IsLatest == 1
            && x.ProcessStatus == 1
            && x.SuspensionState == (int)TaktFlowSuspensionState.Active);
        if (scheme == null)
        {
            ThrowBusinessException($"流程「{processKey}」未发布或不可用");
        }
        if (string.IsNullOrWhiteSpace(scheme.ProcessContent))
        {
            ThrowBusinessException("流程设计内容为空");
        }
        if (scheme.FormId <= 0 || string.IsNullOrWhiteSpace(scheme.FormCode))
        {
            ThrowBusinessException("流程未关联表单，无法发起");
        }
        return scheme;
    }

    /// <summary>
    /// 创建流程实例实体
    /// </summary>
    private async Task<TaktFlowInstance> CreateInstanceEntityAsync(
        TaktFlowScheme scheme,
        TaktFlowStartDto dto,
        long startUserId,
        string? startUserName,
        TaktFlowInstanceStatus status)
    {
        var instance = new TaktFlowInstance
        {
            InstanceCode = GenerateInstanceCode(),
            ProcessDefinitionId = scheme.Id,
            ProcessKey = scheme.ProcessKey,
            ProcessName = scheme.ProcessName,
            DefinitionVersion = scheme.DefinitionVersion,
            ProcessTitle = dto.ProcessTitle ?? string.Empty,
            InstanceStatus = (int)status,
            StartUserId = startUserId,
            StartUserName = startUserName ?? string.Empty,
            StartTime = DateTime.Now,
            BusinessKey = dto.BusinessKey ?? string.Empty,
            BusinessType = string.IsNullOrWhiteSpace(dto.BusinessType) ? scheme.ProcessKey : dto.BusinessType!,
            FrmData = dto.FrmData ?? string.Empty,
            FormId = scheme.FormId,
            FormCode = scheme.FormCode,
            ProcessContentSnapshot = scheme.ProcessContent ?? string.Empty
        };
        return await _flowInstanceRepository.CreateAsync(instance);
    }

    /// <summary>
    /// 生成实例编码
    /// </summary>
    /// <returns>编码</returns>
    private static string GenerateInstanceCode()
    {
        return $"WF{DateTime.Now:yyyyMMddHHmmss}{Random.Shared.Next(1000, 9999)}";
    }

    /// <summary>
    /// 加载实例或抛错
    /// </summary>
    /// <param name="instanceId">实例 ID</param>
    /// <returns>实例</returns>
    private async Task<TaktFlowInstance> LoadInstanceOrThrowAsync(long instanceId)
    {
        var instance = await _flowInstanceRepository.GetByIdAsync(instanceId);
        if (instance == null || instance.TenantCode != CurrentTenantCode || instance.CompanyCode != CurrentCompanyCode)
        {
            ThrowBusinessException("流程实例不存在");
        }
        return instance;
    }

    /// <summary>
    /// 校验实例可审批
    /// </summary>
    /// <param name="instance">实例</param>
    private static void EnsureInstanceRunnable(TaktFlowInstance instance)
    {
        if (instance.InstanceStatus != (int)TaktFlowInstanceStatus.Running)
        {
            throw new TaktBusinessException("流程未处于运行中，无法操作");
        }
    }

    /// <summary>
    /// 映射列表项
    /// </summary>
    /// <param name="entity">实体</param>
    /// <returns>列表 DTO</returns>
    private static TaktFlowInstanceListItemDto MapListItem(TaktFlowInstance entity)
    {
        return new TaktFlowInstanceListItemDto
        {
            FlowInstanceId = entity.Id,
            InstanceCode = entity.InstanceCode,
            ProcessName = entity.ProcessName,
            ProcessTitle = entity.ProcessTitle,
            InstanceStatus = (TaktFlowInstanceStatus)entity.InstanceStatus,
            CurrentActivityName = entity.CurrentActivityName,
            StartUserId = entity.StartUserId,
            StartUserName = entity.StartUserName,
            StartTime = entity.StartTime ?? default,
            FrmData = entity.FrmData
        };
    }

    /// <summary>
    /// 映射详情
    /// </summary>
    /// <param name="instance">实例</param>
    /// <returns>详情 DTO</returns>
    private async Task<TaktFlowInstanceDetailDto> MapDetailAsync(TaktFlowInstance instance)
    {
        var userId = CurrentUserId;
        var transitions = await _flowTransitionRepository.GetListAsync(
            x => x.InstanceId == instance.Id,
            x => x.TransitionTime,
            false);
        var history = transitions.Select(t => new TaktFlowHistoryItemDto
        {
            FromNodeName = t.FromNodeName ?? "-",
            ToNodeName = t.ToNodeName ?? "-",
            TransitionUserName = t.TransitionUserName ?? "-",
            TransitionTime = t.TransitionTime,
            TransitionComment = t.TransitionComment
        }).ToList();
        var pendingAdds = await _flowAddSignRepository.GetListAsync(x =>
            x.InstanceId == instance.Id && x.IsHandled == 0);
        var canVerify = userId.HasValue && await _flowTaskRepository.FirstAsync(x =>
            x.InstanceId == instance.Id
            && x.AssigneeUserId == userId.Value
            && x.TaskStatus == (int)TaktFlowTaskStatus.Pending) != null;
        return new TaktFlowInstanceDetailDto
        {
            FlowInstanceId = instance.Id,
            InstanceCode = instance.InstanceCode,
            ProcessDefinitionId = instance.ProcessDefinitionId,
            ProcessKey = instance.ProcessKey,
            ProcessName = instance.ProcessName,
            ProcessTitle = instance.ProcessTitle,
            InstanceStatus = (TaktFlowInstanceStatus)instance.InstanceStatus,
            CurrentActivityId = instance.CurrentActivityId,
            CurrentActivityName = instance.CurrentActivityName,
            StartUserId = instance.StartUserId,
            StartUserName = instance.StartUserName,
            StartTime = instance.StartTime ?? default,
            EndTime = instance.EndTime,
            FrmData = instance.FrmData,
            History = history,
            PendingAddApprovers = pendingAdds.Select(p => new TaktFlowPendingAddApproverDto
            {
                FlowAddSignId = p.Id,
                ApproverUserName = p.SignUserName ?? p.SignUserId.ToString()
            }).ToList(),
            CanVerify = canVerify
        };
    }

    /// <summary>
    /// 待办/已办列表是否满足查询条件
    /// </summary>
    /// <param name="instance">流程实例</param>
    /// <param name="task">待办任务（已办可为 null）</param>
    /// <param name="query">查询条件</param>
    /// <returns>是否匹配</returns>
    private static bool MatchesTodoQuery(TaktFlowInstance instance, TaktFlowTask? task, TaktFlowTodoQueryDto query)
    {
        if (!string.IsNullOrWhiteSpace(query.InstanceCode)
            && !instance.InstanceCode.Contains(query.InstanceCode, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }
        if (!string.IsNullOrWhiteSpace(query.ProcessKey)
            && !(instance.ProcessKey?.Contains(query.ProcessKey, StringComparison.OrdinalIgnoreCase) == true))
        {
            return false;
        }
        if (!string.IsNullOrWhiteSpace(query.ProcessName)
            && !(instance.ProcessName?.Contains(query.ProcessName, StringComparison.OrdinalIgnoreCase) == true))
        {
            return false;
        }
        if (!string.IsNullOrWhiteSpace(query.ProcessTitle)
            && !(instance.ProcessTitle?.Contains(query.ProcessTitle, StringComparison.OrdinalIgnoreCase) == true))
        {
            return false;
        }
        if (query.ProcessDefinitionId.HasValue
            && instance.ProcessDefinitionId != query.ProcessDefinitionId.Value)
        {
            return false;
        }
        if (!string.IsNullOrWhiteSpace(query.TaskName))
        {
            var nodeName = task?.TaskName ?? instance.CurrentActivityName;
            if (nodeName?.Contains(query.TaskName, StringComparison.OrdinalIgnoreCase) != true)
            {
                return false;
            }
        }
        if (!string.IsNullOrWhiteSpace(query.StartUserName)
            && !(instance.StartUserName?.Contains(query.StartUserName, StringComparison.OrdinalIgnoreCase) == true))
        {
            return false;
        }
        if (query.StartTimeStart.HasValue && instance.StartTime < query.StartTimeStart)
        {
            return false;
        }
        if (query.StartTimeEnd.HasValue && instance.StartTime > query.StartTimeEnd)
        {
            return false;
        }
        if (!string.IsNullOrWhiteSpace(query.KeyWords))
        {
            var kw = query.KeyWords;
            if (!(instance.ProcessName?.Contains(kw, StringComparison.OrdinalIgnoreCase) == true
                || instance.ProcessTitle?.Contains(kw, StringComparison.OrdinalIgnoreCase) == true
                || instance.InstanceCode.Contains(kw, StringComparison.OrdinalIgnoreCase)
                || (instance.ProcessKey?.Contains(kw, StringComparison.OrdinalIgnoreCase) == true)))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 为「我的流程」列表追加实例查询条件
    /// </summary>
    /// <param name="exp">表达式</param>
    /// <param name="query">查询 DTO</param>
    /// <returns>追加条件后的表达式</returns>
    private static Expressionable<TaktFlowInstance> ApplyFlowInstanceListQuery(
        Expressionable<TaktFlowInstance> exp,
        TaktFlowInstanceQueryDto query)
    {
        if (!string.IsNullOrWhiteSpace(query.InstanceCode))
        {
            var code = query.InstanceCode;
            exp = exp.And(x => x.InstanceCode.Contains(code));
        }
        if (!string.IsNullOrWhiteSpace(query.ProcessKey))
        {
            var key = query.ProcessKey;
            exp = exp.And(x => x.ProcessKey != null && x.ProcessKey.Contains(key));
        }
        if (!string.IsNullOrWhiteSpace(query.ProcessName))
        {
            var name = query.ProcessName;
            exp = exp.And(x => x.ProcessName != null && x.ProcessName.Contains(name));
        }
        if (!string.IsNullOrWhiteSpace(query.ProcessTitle))
        {
            var title = query.ProcessTitle;
            exp = exp.And(x => x.ProcessTitle != null && x.ProcessTitle.Contains(title));
        }
        if (query.ProcessDefinitionId.HasValue)
        {
            var defId = query.ProcessDefinitionId.Value;
            exp = exp.And(x => x.ProcessDefinitionId == defId);
        }
        if (query.InstanceStatus.HasValue)
        {
            var status = query.InstanceStatus.Value;
            exp = exp.And(x => x.InstanceStatus == (int)status);
        }
        if (!string.IsNullOrWhiteSpace(query.StartUserName))
        {
            var UserName = query.StartUserName;
            exp = exp.And(x => x.StartUserName != null && x.StartUserName.Contains(UserName));
        }
        if (query.StartTimeStart.HasValue)
        {
            var start = query.StartTimeStart.Value;
            exp = exp.And(x => x.StartTime != null && x.StartTime >= start);
        }
        if (query.StartTimeEnd.HasValue)
        {
            var end = query.StartTimeEnd.Value;
            exp = exp.And(x => x.StartTime != null && x.StartTime <= end);
        }
        if (!string.IsNullOrWhiteSpace(query.KeyWords))
        {
            var kw = query.KeyWords;
            exp = exp.And(x =>
                x.InstanceCode.Contains(kw)
                || (x.ProcessKey != null && x.ProcessKey.Contains(kw))
                || (x.ProcessName != null && x.ProcessName.Contains(kw))
                || (x.ProcessTitle != null && x.ProcessTitle.Contains(kw)));
        }
        return exp;
    }

    /// <summary>
    /// 为「我的流程」列表追加查询条件（与待办/已办共用 TaktFlowTodoQueryDto）
    /// </summary>
    /// <param name="exp">表达式</param>
    /// <param name="query">查询 DTO</param>
    /// <returns>追加条件后的表达式</returns>
    private static Expressionable<TaktFlowInstance> ApplyFlowInstanceListQuery(
        Expressionable<TaktFlowInstance> exp,
        TaktFlowTodoQueryDto query)
    {
        if (!string.IsNullOrWhiteSpace(query.InstanceCode))
        {
            var code = query.InstanceCode;
            exp = exp.And(x => x.InstanceCode.Contains(code));
        }
        if (!string.IsNullOrWhiteSpace(query.ProcessKey))
        {
            var key = query.ProcessKey;
            exp = exp.And(x => x.ProcessKey != null && x.ProcessKey.Contains(key));
        }
        if (!string.IsNullOrWhiteSpace(query.ProcessName))
        {
            var name = query.ProcessName;
            exp = exp.And(x => x.ProcessName != null && x.ProcessName.Contains(name));
        }
        if (!string.IsNullOrWhiteSpace(query.ProcessTitle))
        {
            var title = query.ProcessTitle;
            exp = exp.And(x => x.ProcessTitle != null && x.ProcessTitle.Contains(title));
        }
        if (query.ProcessDefinitionId.HasValue)
        {
            var defId = query.ProcessDefinitionId.Value;
            exp = exp.And(x => x.ProcessDefinitionId == defId);
        }
        if (!string.IsNullOrWhiteSpace(query.TaskName))
        {
            var taskName = query.TaskName;
            exp = exp.And(x => x.CurrentActivityName != null && x.CurrentActivityName.Contains(taskName));
        }
        if (!string.IsNullOrWhiteSpace(query.StartUserName))
        {
            var UserName = query.StartUserName;
            exp = exp.And(x => x.StartUserName != null && x.StartUserName.Contains(UserName));
        }
        if (query.StartTimeStart.HasValue)
        {
            var start = query.StartTimeStart.Value;
            exp = exp.And(x => x.StartTime != null && x.StartTime >= start);
        }
        if (query.StartTimeEnd.HasValue)
        {
            var end = query.StartTimeEnd.Value;
            exp = exp.And(x => x.StartTime != null && x.StartTime <= end);
        }
        if (!string.IsNullOrWhiteSpace(query.KeyWords))
        {
            var kw = query.KeyWords;
            exp = exp.And(x =>
                x.InstanceCode.Contains(kw)
                || (x.ProcessKey != null && x.ProcessKey.Contains(kw))
                || (x.ProcessName != null && x.ProcessName.Contains(kw))
                || (x.ProcessTitle != null && x.ProcessTitle.Contains(kw)));
        }
        return exp;
    }
}
