// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange.Quartz
// 文件名称：TaktEcBomDateReminderScanJobHandler.cs
// 创建时间：2026-10-08
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz 按 BOM 生效日前 30 天、后 30 天提醒尚未实施的设变部门
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Interfaces;
using Takt.Shared.Constants;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange.Quartz;

/// <summary>
/// BOM 生效日前后 30 天未实施提醒
/// </summary>
public sealed class TaktEcBomDateReminderScanJobHandler : ITaktQuartzJobHandler
{
    private readonly TaktEcHandoffMailService _handoffMailService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="handoffMailService">设变实施邮件</param>
    public TaktEcBomDateReminderScanJobHandler(TaktEcHandoffMailService handoffMailService)
    {
        _handoffMailService = handoffMailService;
    }

    /// <inheritdoc />
    public string HandlerKey => TaktEcFlowConstants.QuartzHandlerEcBomDateReminderScan;

    /// <inheritdoc />
    public async Task ExecuteAsync(TaktQuartzJobContext context, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        var task = context.Task;
        var sent = await _handoffMailService.SendBomDateRemindersAsync(
            task.TenantCode ?? string.Empty,
            task.CompanyCode ?? string.Empty,
            cancellationToken);
        context.ExecuteMessage = $"BOM生效日前后30天未实施提醒已发送 {sent} 封";
    }
}
