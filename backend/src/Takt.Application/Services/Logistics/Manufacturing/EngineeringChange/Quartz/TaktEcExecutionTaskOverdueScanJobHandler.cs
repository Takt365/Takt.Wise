// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange.Quartz
// 文件名称：TaktEcExecutionTaskOverdueScanJobHandler.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz 扫描工程变更执行任务超时并 SignalR 预警
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Interfaces;
using Takt.Shared.Constants;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange.Quartz;

/// <summary>
/// 工程变更执行任务超时扫描处理器
/// </summary>
public class TaktEcExecutionTaskOverdueScanJobHandler : ITaktQuartzJobHandler
{
    private readonly ITaktEcChangeFlowService _ecChangeFlowService;

    /// <summary>
    /// 初始化扫描处理器
    /// </summary>
    /// <param name="ecChangeFlowService">工程变更流程服务</param>
    public TaktEcExecutionTaskOverdueScanJobHandler(ITaktEcChangeFlowService ecChangeFlowService)
    {
        _ecChangeFlowService = ecChangeFlowService;
    }

    /// <summary>
    /// 处理器键（与 TaktQuartzTask.ClassName 匹配）
    /// </summary>
    public string HandlerKey => TaktEcFlowConstants.QuartzHandlerEcTaskOverdueScan;

    /// <summary>
    /// 执行任务逻辑
    /// </summary>
    /// <param name="context">执行上下文</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>任务</returns>
    public async Task ExecuteAsync(TaktQuartzJobContext context, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        var task = context.Task;
        await _ecChangeFlowService.ScanOverdueEcExecutionTasksAsync(task.TenantCode, task.CompanyCode);
    }
}
