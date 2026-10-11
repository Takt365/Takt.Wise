// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Interfaces
// 文件名称：ITaktDataCloneProvider.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：公司级数据克隆提供者（Infrastructure 实现）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Models.Code;

namespace Takt.Domain.Interfaces;

/// <summary>
/// 公司级数据克隆提供者
/// </summary>
public interface ITaktDataCloneProvider
{
    /// <summary>
    /// 获取目标公司数据备份预览（克隆前备份窗口）
    /// </summary>
    Task<TaktCloneTargetBackupPreview> GetTargetBackupPreviewAsync(
        TaktDataCloneOptions options,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 按公司范围克隆数据（先备份并清空目标公司数据，再克隆）
    /// </summary>
    /// <param name="options">克隆选项</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>克隆结果</returns>
    Task<TaktDataCloneResult> CloneDataAsync(
        TaktDataCloneOptions options,
        CancellationToken cancellationToken = default);
}
