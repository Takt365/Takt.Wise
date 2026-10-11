// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Interfaces
// 文件名称：ITaktTableCloneProvider.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：跨租户/跨库物理表数据克隆提供者（Infrastructure 实现）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Models.Code;

namespace Takt.Domain.Interfaces;

/// <summary>
/// 物理表数据克隆提供者
/// </summary>
public interface ITaktTableCloneProvider
{
    /// <summary>
    /// 获取目标整表备份预览（克隆前备份窗口）
    /// </summary>
    Task<TaktCloneTargetBackupPreview> GetTargetBackupPreviewAsync(
        TaktTableCloneOptions options,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 将源表数据克隆到目标表（先备份并清空目标整表，再克隆）
    /// </summary>
    /// <param name="options">克隆选项</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>克隆结果</returns>
    Task<TaktTableCloneResult> CloneTableAsync(
        TaktTableCloneOptions options,
        CancellationToken cancellationToken = default);
}
