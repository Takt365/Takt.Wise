// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：ITaktEcTePersistBackgroundService.cs
// 创建时间：2026-09-08
// 创建人：Takt365(Cursor AI)
// 功能描述：设变技术课主表新增/更新后台任务调度（含子表与各部门执行派生）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变技术课主表新增/更新后台任务调度接口
/// </summary>
public interface ITaktEcTePersistBackgroundService
{
    /// <summary>
    /// 提交后台新增（立即返回；完成后 SignalR 通知触发用户）
    /// </summary>
    /// <param name="dto">创建 DTO</param>
    /// <returns>已提交回执</returns>
    Task<TaktEcTeSubmittedDto> EnqueueCreateAsync(TaktEcTeCreateDto dto);

    /// <summary>
    /// 提交后台更新（立即返回；完成后 SignalR 通知触发用户）
    /// </summary>
    /// <param name="id">主表主键</param>
    /// <param name="dto">更新 DTO</param>
    /// <returns>已提交回执</returns>
    Task<TaktEcTeSubmittedDto> EnqueueUpdateAsync(long id, TaktEcTeUpdateDto dto);
}
