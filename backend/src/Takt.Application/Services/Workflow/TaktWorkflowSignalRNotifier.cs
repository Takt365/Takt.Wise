// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Workflow
// 文件名称：TaktWorkflowSignalRNotifier.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Cursor AI)
// 功能描述：工作流 SignalR 推送编排（方案变更、实例推进、待办计数）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.Workflow;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;
using Takt.Shared.Models.Workflow;

namespace Takt.Application.Services.Workflow;

/// <summary>
/// 工作流 SignalR 推送目标用户
/// </summary>
public sealed class TaktWorkflowSignalRUserTarget
{
    /// <summary>
    /// 用户 ID
    /// </summary>
    public long UserId { get; init; }

    /// <summary>
    /// 登录用户名
    /// </summary>
    public string UserName { get; init; } = string.Empty;
}

/// <summary>
/// 工作流 SignalR 推送编排接口
/// </summary>
public interface ITaktWorkflowSignalRNotifier
{
    /// <summary>
    /// 推送流程定义变更
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="scheme">流程定义实体</param>
    /// <param name="changeType">变更类型</param>
    /// <param name="operatorUserName">操作人用户名</param>
    /// <returns>任务</returns>
    Task NotifySchemeChangedAsync(
        string tenantCode,
        string companyCode,
        TaktFlowScheme scheme,
        string changeType,
        string? operatorUserName);

    /// <summary>
    /// 推送流程实例推进与相关用户待办计数
    /// </summary>
    /// <param name="instance">流程实例</param>
    /// <param name="actionType">动作类型</param>
    /// <param name="additionalUsers">额外通知用户</param>
    /// <returns>任务</returns>
    Task NotifyInstanceProgressedAsync(
        TaktFlowInstance instance,
        string actionType,
        IReadOnlyCollection<TaktWorkflowSignalRUserTarget>? additionalUsers = null);

    /// <summary>
    /// 统计指定用户待办数量（Running 实例 + Pending 任务）
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="userId">用户 ID</param>
    /// <returns>待办数量</returns>
    Task<int> CountTodoForUserAsync(string tenantCode, string companyCode, long userId);
}

/// <summary>
/// 工作流 SignalR 推送编排实现
/// </summary>
public class TaktWorkflowSignalRNotifier : ITaktWorkflowSignalRNotifier
{
    private readonly ITaktSignalRDispatchService _signalRDispatchService;
    private readonly ITaktCompanyRepository<TaktFlowTask> _flowTaskRepository;
    private readonly ITaktCompanyRepository<TaktFlowInstance> _flowInstanceRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="signalRDispatchService">SignalR 调度服务</param>
    /// <param name="flowTaskRepository">流程任务仓储</param>
    /// <param name="flowInstanceRepository">流程实例仓储</param>
    public TaktWorkflowSignalRNotifier(
        ITaktSignalRDispatchService signalRDispatchService,
        ITaktCompanyRepository<TaktFlowTask> flowTaskRepository,
        ITaktCompanyRepository<TaktFlowInstance> flowInstanceRepository)
    {
        _signalRDispatchService = signalRDispatchService;
        _flowTaskRepository = flowTaskRepository;
        _flowInstanceRepository = flowInstanceRepository;
    }

    /// <summary>
    /// 推送流程定义变更
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="scheme">流程定义实体</param>
    /// <param name="changeType">变更类型</param>
    /// <param name="operatorUserName">操作人用户名</param>
    /// <returns>任务</returns>
    public async Task NotifySchemeChangedAsync(
        string tenantCode,
        string companyCode,
        TaktFlowScheme scheme,
        string changeType,
        string? operatorUserName)
    {
        ArgumentNullException.ThrowIfNull(scheme);
        var push = new TaktSignalRFlowSchemeChangedPush
        {
            TenantCode = tenantCode.Trim(),
            CompanyCode = companyCode.Trim(),
            FlowSchemeId = scheme.Id,
            ProcessKey = scheme.ProcessKey ?? string.Empty,
            ProcessName = scheme.ProcessName ?? string.Empty,
            ChangeType = changeType.Trim(),
            OperatorUserName = operatorUserName?.Trim(),
            ChangedAt = DateTime.Now,
        };
        await _signalRDispatchService.PushFlowSchemeChangedAsync(push);
    }

    /// <summary>
    /// 推送流程实例推进与相关用户待办计数
    /// </summary>
    /// <param name="instance">流程实例</param>
    /// <param name="actionType">动作类型</param>
    /// <param name="additionalUsers">额外通知用户</param>
    /// <returns>任务</returns>
    public async Task NotifyInstanceProgressedAsync(
        TaktFlowInstance instance,
        string actionType,
        IReadOnlyCollection<TaktWorkflowSignalRUserTarget>? additionalUsers = null)
    {
        ArgumentNullException.ThrowIfNull(instance);
        var users = await CollectAffectedUsersAsync(instance, additionalUsers);
        if (users.Count == 0)
        {
            return;
        }

        var push = new TaktSignalRFlowInstanceProgressedPush
        {
            TenantCode = instance.TenantCode,
            CompanyCode = instance.CompanyCode,
            FlowInstanceId = instance.Id,
            InstanceCode = instance.InstanceCode ?? string.Empty,
            ProcessName = instance.ProcessName ?? string.Empty,
            InstanceStatus = (int)instance.InstanceStatus,
            ActionType = actionType.Trim(),
            CurrentActivityName = instance.CurrentActivityName,
            StartUserName = instance.StartUserName,
            ProgressedAt = DateTime.Now,
        };

        foreach (var user in users)
        {
            if (string.IsNullOrWhiteSpace(user.UserName))
            {
                continue;
            }

            await _signalRDispatchService.PushFlowInstanceProgressedToUserAsync(push, user.UserName);
            var todoCount = await CountTodoForUserAsync(instance.TenantCode, instance.CompanyCode, user.UserId);
            await _signalRDispatchService.PushFlowTodoCountToUserAsync(new TaktSignalRFlowTodoCountPush
            {
                TenantCode = instance.TenantCode,
                CompanyCode = instance.CompanyCode,
                UserName = user.UserName,
                UserId = user.UserId,
                TodoCount = todoCount,
                UpdatedAt = DateTime.Now,
            });
        }
    }

    /// <summary>
    /// 统计指定用户待办数量（Running 实例 + Pending 任务）
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="userId">用户 ID</param>
    /// <returns>待办数量</returns>
    public async Task<int> CountTodoForUserAsync(string tenantCode, string companyCode, long userId)
    {
        if (userId <= 0)
        {
            return 0;
        }

        var tasks = await _flowTaskRepository.GetListAsync(x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.AssigneeUserId == userId
            && x.TaskStatus == (int)TaktFlowTaskStatus.Pending);
        if (tasks.Count == 0)
        {
            return 0;
        }

        var instanceIds = tasks.Select(x => x.InstanceId).Distinct().ToList();
        var instances = await _flowInstanceRepository.GetListAsync(x =>
            instanceIds.Contains(x.Id)
            && x.InstanceStatus == (int)TaktFlowInstanceStatus.Running);
        var runningInstanceIds = instances.Select(x => x.Id).ToHashSet();
        return tasks.Count(task => runningInstanceIds.Contains(task.InstanceId));
    }

    /// <summary>
    /// 收集实例相关通知用户（待办办理人 + 发起人 + 额外用户）
    /// </summary>
    /// <param name="instance">流程实例</param>
    /// <param name="additionalUsers">额外用户</param>
    /// <returns>去重后的用户列表</returns>
    private async Task<IReadOnlyList<TaktWorkflowSignalRUserTarget>> CollectAffectedUsersAsync(
        TaktFlowInstance instance,
        IReadOnlyCollection<TaktWorkflowSignalRUserTarget>? additionalUsers)
    {
        var map = new Dictionary<long, TaktWorkflowSignalRUserTarget>();
        void TryAdd(long userId, string? UserName)
        {
            if (userId <= 0 || string.IsNullOrWhiteSpace(UserName))
            {
                return;
            }

            var normalizedName = UserName.Trim();
            if (!map.ContainsKey(userId))
            {
                map[userId] = new TaktWorkflowSignalRUserTarget
                {
                    UserId = userId,
                    UserName = normalizedName,
                };
            }
        }

        TryAdd(instance.StartUserId, instance.StartUserName);
        if (additionalUsers != null)
        {
            foreach (var user in additionalUsers)
            {
                TryAdd(user.UserId, user.UserName);
            }
        }

        var pendingTasks = await _flowTaskRepository.GetListAsync(x =>
            x.InstanceId == instance.Id
            && x.TaskStatus == (int)TaktFlowTaskStatus.Pending);
        foreach (var task in pendingTasks)
        {
            TryAdd(task.AssigneeUserId, task.AssigneeUserName);
        }

        return map.Values.ToList();
    }
}
