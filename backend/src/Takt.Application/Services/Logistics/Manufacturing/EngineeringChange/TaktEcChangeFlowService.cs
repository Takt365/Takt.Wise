// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcChangeFlowService.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Cursor AI)
// 功能描述：工程变更通知与执行监测流程；通知派发后各部门执行，技术通过看板/任务进度监控
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Entities.Identity;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Constants;
using Takt.Shared.Exceptions;
using Takt.Shared.Models.Logistics.Manufacturing;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 工程变更通知与执行监测流程服务（阶段一通知派发 → 阶段二部门执行 → 技术看板/任务监控）
/// </summary>
public class TaktEcChangeFlowService : TaktServiceBase, ITaktEcChangeFlowService
{
    private readonly ITaktApprovalRepository<TaktEcNotification> _ecNotificationRepository;
    private readonly ITaktCompanyRepository<TaktEcNotificationDelivery> _deliveryRepository;
    private readonly ITaktCompanyRepository<TaktEcExecutionTask> _taskRepository;
    private readonly ITaktCompanyRepository<TaktEcTe> _ecTeRepository;
    private readonly TaktEcExecDeptAccess _ecExecDeptAccess;
    private readonly ITaktTenantRepository<TaktUser> _userRepository;
    private readonly ITaktSignalRDispatchService _signalRDispatchService;

    /// <summary>
    /// 初始化工程变更流程服务
    /// </summary>
    public TaktEcChangeFlowService(
        ITaktApprovalRepository<TaktEcNotification> ecNotificationRepository,
        ITaktCompanyRepository<TaktEcNotificationDelivery> deliveryRepository,
        ITaktCompanyRepository<TaktEcExecutionTask> taskRepository,
        ITaktCompanyRepository<TaktEcTe> ecTeRepository,
        TaktEcExecDeptAccess ecExecDeptAccess,
        ITaktTenantRepository<TaktUser> userRepository,
        ITaktSignalRDispatchService signalRDispatchService,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _ecNotificationRepository = ecNotificationRepository;
        _deliveryRepository = deliveryRepository;
        _taskRepository = taskRepository;
        _ecTeRepository = ecTeRepository;
        _ecExecDeptAccess = ecExecDeptAccess;
        _userRepository = userRepository;
        _signalRDispatchService = signalRDispatchService;
    }

    /// <summary>
    /// 派发变更通知：解析部门、持久化投递记录、SignalR 推送
    /// </summary>
    /// <param name="ecNotificationId">通知单 ID</param>
    /// <returns>异步任务</returns>
    public async Task DispatchEcNotificationAsync(long ecNotificationId)
    {
        EnsureThreeLayerContext();
        var notification = await LoadNotificationAsync(ecNotificationId);
        if (notification.EcNotificationStatus >= 2)
        {
            throw new TaktBusinessException("通知单已确认完成，无法重复派发");
        }
        var deptPairs = ParseDeptPairs(notification);
        if (deptPairs.Count == 0)
        {
            throw new TaktBusinessException("通知单未配置目标部门");
        }
        var priority = ResolvePriority(notification);
        var now = DateTime.Now;
        foreach (var (deptCode, deptName) in deptPairs)
        {
            var delivery = await EnsureDeliveryAsync(notification, deptCode, deptName, priority);
            if (delivery.DeliveryStatus >= TaktEcFlowConstants.DeliveryStatusSent)
            {
                continue;
            }
            delivery.DeliveryStatus = TaktEcFlowConstants.DeliveryStatusSent;
            delivery.SentAt = now;
            await _deliveryRepository.UpdateAsync(delivery);
            await _signalRDispatchService.PushEcChangeNotificationAsync(new TaktEcChangeNotificationPush
            {
                CompanyCode = CurrentCompanyCode,
                DeliveryId = delivery.Id,
                EcNotificationId = notification.Id,
                EcNotificationCode = notification.EcNotificationCode,
                EcTeId = notification.EcTeId,
                EcCode = notification.EcCode,
                EcTitle = notification.EcTitle,
                DeptCode = deptCode,
                Priority = priority,
                PushedAt = now
            });
        }
        if (notification.EcNotificationStatus == 0)
        {
            notification.EcNotificationStatus = 1;
            await _ecNotificationRepository.UpdateAsync(notification);
        }
    }

    /// <summary>
    /// 确认通知投递（部门用户点击确认）
    /// </summary>
    /// <param name="dto">确认参数</param>
    /// <returns>异步任务</returns>
    public async Task ConfirmEcNotificationDeliveryAsync(TaktEcNotificationConfirmDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        EnsureThreeLayerContext();
        var delivery = await ResolveDeliveryForConfirmAsync(dto);
        if (delivery.DeliveryStatus >= TaktEcFlowConstants.DeliveryStatusConfirmed)
        {
            return;
        }
        delivery.DeliveryStatus = TaktEcFlowConstants.DeliveryStatusConfirmed;
        delivery.ConfirmedAt = DateTime.Now;
        await _deliveryRepository.UpdateAsync(delivery);
        var notification = await LoadNotificationAsync(delivery.EcNotificationId);
        await NotifyInitiatorDeliveryConfirmedAsync(notification, delivery);
        var allDeliveries = await _deliveryRepository.GetListAsync(x =>
            x.EcNotificationId == notification.Id && x.IsDeleted == 0);
        if (allDeliveries.All(x => x.DeliveryStatus >= TaktEcFlowConstants.DeliveryStatusConfirmed))
        {
            notification.EcNotificationStatus = 2;
            await _ecNotificationRepository.UpdateAsync(notification);
            await CreateExecutionTasksAsync(notification, delivery.EcDeptGroupCode);
            await TryCloseEcChangeAsync(notification.Id);
        }
        else
        {
            await CreateExecutionTasksAsync(notification, delivery.EcDeptGroupCode);
        }
    }

    /// <summary>
    /// 上报执行任务进度
    /// </summary>
    /// <param name="dto">进度参数</param>
    /// <returns>异步任务</returns>
    public async Task ReportEcExecutionTaskProgressAsync(TaktEcExecutionTaskProgressReportDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        EnsureThreeLayerContext();
        if (dto.TaskId <= 0)
        {
            throw new TaktBusinessException("任务 ID 无效");
        }
        dto.ProgressPercent = Math.Clamp(dto.ProgressPercent, 0, 100);
        var task = await _taskRepository.GetByIdAsync(dto.TaskId);
        if (task == null
            || task.TenantCode != CurrentTenantCode
            || task.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("执行任务不存在");
        }
        task.TaskStatus = dto.TaskStatus;
        task.ProgressPercent = dto.ProgressPercent;
        task.LastProgressRemark = dto.ProgressRemark;
        if (dto.TaskStatus == TaktEcFlowConstants.TaskStatusCompleted)
        {
            task.ProgressPercent = 100;
            task.CompletedAt = DateTime.Now;
        }
        await _taskRepository.UpdateAsync(task);
        var push = new TaktEcExecutionTaskProgressPush
        {
            CompanyCode = CurrentCompanyCode,
            TaskId = task.Id,
            EcCode = task.EcCode,
            DeptCode = task.DeptCode,
            TaskStatus = task.TaskStatus,
            ProgressPercent = task.ProgressPercent,
            ProgressRemark = task.LastProgressRemark,
            ReporterUserName = CurrentUserName,
            ReportedAt = DateTime.Now
        };
        await _signalRDispatchService.PushEcExecutionTaskProgressAsync(push);
        if (task.TaskStatus == TaktEcFlowConstants.TaskStatusBlocked)
        {
            await _signalRDispatchService.PushEcExecutionTaskAlertAsync(new TaktEcExecutionTaskAlertPush
            {
                CompanyCode = CurrentCompanyCode,
                TaskId = task.Id,
                EcCode = task.EcCode,
                DeptCode = task.DeptCode,
                AlertType = "blocked",
                Message = task.LastProgressRemark ?? $"设变 {task.EcCode} 部门 {task.DeptCode} 任务阻塞"
            });
        }
        if (task.TaskStatus == TaktEcFlowConstants.TaskStatusCompleted)
        {
            await TryCloseEcChangeAsync(task.EcNotificationId);
        }
    }

    /// <summary>
    /// 扫描超时/阻塞任务并推送预警（定时任务入口）
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <returns>处理任务数</returns>
    public async Task<int> ScanOverdueEcExecutionTasksAsync(string tenantCode, string companyCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(companyCode);
        var now = DateTime.Now;
        var tasks = await _taskRepository.GetListAsync(x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.IsDeleted == 0
            && (x.TaskStatus == TaktEcFlowConstants.TaskStatusPending
                || x.TaskStatus == TaktEcFlowConstants.TaskStatusInProgress
                || x.TaskStatus == TaktEcFlowConstants.TaskStatusBlocked)
            && x.DueDate != null
            && x.DueDate < now);
        var count = 0;
        foreach (var task in tasks)
        {
            if (task.TaskStatus == TaktEcFlowConstants.TaskStatusOverdue)
            {
                continue;
            }
            task.TaskStatus = TaktEcFlowConstants.TaskStatusOverdue;
            await _taskRepository.UpdateAsync(task);
            await _signalRDispatchService.PushEcExecutionTaskAlertAsync(new TaktEcExecutionTaskAlertPush
            {
                CompanyCode = companyCode,
                TaskId = task.Id,
                EcCode = task.EcCode,
                DeptCode = task.DeptCode,
                AlertType = "overdue",
                Message = $"设变 {task.EcCode} 部门 {task.DeptCode} 任务已超时"
            });
            count++;
        }
        return count;
    }

    /// <summary>
    /// 加载通知单
    /// </summary>
    private async Task<TaktEcNotification> LoadNotificationAsync(long ecNotificationId)
    {
        var notification = await _ecNotificationRepository.GetByIdAsync(ecNotificationId);
        if (notification == null
            || notification.TenantCode != CurrentTenantCode
            || notification.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("工程变更通知单不存在");
        }
        return notification;
    }

    /// <summary>
    /// 解析通知目标部门
    /// </summary>
    private static List<(string Code, string? Name)> ParseDeptPairs(TaktEcNotification notification)
    {
        var codes = (notification.EcNotificationDeptCodes ?? string.Empty)
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var names = (notification.EcNotificationDeptNames ?? string.Empty)
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var result = new List<(string, string?)>();
        for (var i = 0; i < codes.Length; i++)
        {
            var code = codes[i];
            if (string.IsNullOrWhiteSpace(code))
            {
                continue;
            }
            var name = i < names.Length ? names[i] : null;
            result.Add((code, name));
        }
        return result;
    }

    /// <summary>
    /// 解析通知优先级
    /// </summary>
    private static int ResolvePriority(TaktEcNotification notification)
    {
        return notification.EcNotificationMethod == 1
            ? TaktEcFlowConstants.PriorityUrgent
            : TaktEcFlowConstants.PriorityNormal;
    }

    /// <summary>
    /// 幂等创建投递记录
    /// </summary>
    private async Task<TaktEcNotificationDelivery> EnsureDeliveryAsync(
        TaktEcNotification notification,
        string deptCode,
        string? deptName,
        int priority)
    {
        var existing = await _deliveryRepository.FirstAsync(x =>
            x.EcNotificationId == notification.Id
            && x.EcDeptGroupCode == deptCode
            && x.IsDeleted == 0);
        if (existing != null)
        {
            return existing;
        }
        var delivery = new TaktEcNotificationDelivery
        {
            EcNotificationId = notification.Id,
            EcNotificationCode = notification.EcNotificationCode,
            EcTeId = notification.EcTeId,
            EcCode = notification.EcCode,
            EcDeptGroupCode = deptCode,
            EcDeptGroupName = deptName,
            Priority = priority,
            DeliveryStatus = TaktEcFlowConstants.DeliveryStatusPending
        };
        return await _deliveryRepository.CreateAsync(delivery);
    }

    /// <summary>
    /// 确认时解析投递记录
    /// </summary>
    private async Task<TaktEcNotificationDelivery> ResolveDeliveryForConfirmAsync(TaktEcNotificationConfirmDto dto)
    {
        if (dto.DeliveryId.HasValue && dto.DeliveryId.Value > 0)
        {
            var byId = await _deliveryRepository.GetByIdAsync(dto.DeliveryId.Value);
            if (byId == null
                || byId.TenantCode != CurrentTenantCode
                || byId.CompanyCode != CurrentCompanyCode)
            {
                throw new TaktBusinessException("通知投递记录不存在");
            }
            return byId;
        }
        if (!dto.EcNotificationId.HasValue || string.IsNullOrWhiteSpace(dto.EcDeptGroupCode))
        {
            throw new TaktBusinessException("请提供 deliveryId 或 ecNotificationId+ecDeptGroupCode");
        }
        var delivery = await _deliveryRepository.FirstAsync(x =>
            x.EcNotificationId == dto.EcNotificationId.Value
            && x.EcDeptGroupCode == dto.EcDeptGroupCode.Trim()
            && x.IsDeleted == 0);
        if (delivery == null)
        {
            throw new TaktBusinessException("通知投递记录不存在");
        }
        return delivery;
    }

    /// <summary>
    /// 通知发起人：某部门已确认
    /// </summary>
    private async Task NotifyInitiatorDeliveryConfirmedAsync(
        TaktEcNotification notification,
        TaktEcNotificationDelivery delivery)
    {
        var initiatorUserName = await ResolveInitiatorUserNameAsync(notification);
        if (string.IsNullOrWhiteSpace(initiatorUserName))
        {
            return;
        }
        await _signalRDispatchService.PushEcNotificationConfirmedToUserAsync(
            CurrentCompanyCode,
            initiatorUserName,
            new
            {
                EcNotificationId = notification.Id.ToString(),
                EcCode = notification.EcCode,
                EcDeptGroupCode = delivery.EcDeptGroupCode,
                ConfirmedByGroupName = delivery.ConfirmedByGroupName,
                ConfirmedAt = delivery.ConfirmedAt
            });
    }

    /// <summary>
    /// 为已确认部门创建执行任务
    /// </summary>
    private async Task CreateExecutionTasksAsync(TaktEcNotification notification, string deptCode)
    {
        var existing = await _taskRepository.FirstAsync(x =>
            x.EcNotificationId == notification.Id
            && x.DeptCode == deptCode
            && x.IsDeleted == 0);
        if (existing != null)
        {
            return;
        }
        var ec = await _ecTeRepository.GetByIdAsync(notification.EcTeId);
        var ecExec = await _ecExecDeptAccess.FirstBaseByEcCodeAndDeptAsync(notification.EcCode, deptCode);
        var dueDate = ec?.EcEntryDate ?? ec?.EcIssueDate ?? DateTime.Now.AddDays(7);
        var task = new TaktEcExecutionTask
        {
            EcNotificationId = notification.Id,
            EcTeId = notification.EcTeId,
            EcCode = notification.EcCode,
            EcExecId = ecExec?.Id,
            EcDetailId = ecExec != null && ecExec.EcDetailId > 0 ? ecExec.EcDetailId : null,
            DeptCode = deptCode,
            TaskTitle = $"设变实施-{notification.EcCode}-{deptCode}",
            TaskStatus = TaktEcFlowConstants.TaskStatusPending,
            DueDate = dueDate
        };
        task = await _taskRepository.CreateAsync(task);
        await _signalRDispatchService.PushEcExecutionTaskAssignedAsync(new TaktEcExecutionTaskAssignedPush
        {
            CompanyCode = CurrentCompanyCode,
            TaskId = task.Id,
            EcCode = task.EcCode,
            DeptCode = task.DeptCode,
            TaskTitle = task.TaskTitle,
            DueDate = task.DueDate
        });
    }

    /// <summary>
    /// 全部任务完成时闭环推送
    /// </summary>
    private async Task TryCloseEcChangeAsync(long ecNotificationId)
    {
        var tasks = await _taskRepository.GetListAsync(x =>
            x.EcNotificationId == ecNotificationId && x.IsDeleted == 0);
        if (tasks.Count == 0 || tasks.Any(x => x.TaskStatus != TaktEcFlowConstants.TaskStatusCompleted))
        {
            return;
        }
        var notification = await LoadNotificationAsync(ecNotificationId);
        var ec = await _ecTeRepository.GetByIdAsync(notification.EcTeId);
        if (ec != null && ec.ChangeStatus != 5)
        {
            ec.ChangeStatus = 5;
            await _ecTeRepository.UpdateAsync(ec);
        }
        var closedPush = new TaktEcChangeClosedPush
        {
            CompanyCode = CurrentCompanyCode,
            EcTeId = notification.EcTeId,
            EcCode = notification.EcCode,
            EcNotificationId = notification.Id,
            ClosedAt = DateTime.Now
        };
        await _signalRDispatchService.PushEcChangeClosedAsync(closedPush);
        var initiatorUserName = await ResolveInitiatorUserNameAsync(notification);
        if (!string.IsNullOrWhiteSpace(initiatorUserName))
        {
            await _signalRDispatchService.PushEcChangeClosedToUserAsync(
                CurrentCompanyCode,
                initiatorUserName,
                closedPush);
        }
    }

    /// <summary>
    /// 解析通知发起人登录名
    /// </summary>
    private async Task<string?> ResolveInitiatorUserNameAsync(TaktEcNotification notification)
    {
        long? userId = notification.InitiatorId;
        if (userId is null or <= 0)
        {
            userId = notification.CreatedBy;
        }
        if (!userId.HasValue || userId.Value <= 0)
        {
            return null;
        }
        var user = await _userRepository.GetByIdAsync(userId.Value);
        return user?.UserName;
    }
}
