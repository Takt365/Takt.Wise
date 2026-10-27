// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcTePersistMessageHelper.cs
// 创建时间：2026-09-08
// 创建人：Takt365(Cursor AI)
// 功能描述：设变技术课后台保存操作结果落库消息正文
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Services.Foundation;
using Takt.Shared.Helpers;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变技术课后台保存消息正文与安全落库（失败仅记日志，不阻断业务）
/// </summary>
public static class TaktEcTePersistMessageHelper
{
    /// <summary>
    /// 尝试向当前用户落库并推送操作消息
    /// </summary>
    /// <param name="messageService">在线消息服务</param>
    /// <param name="content">正文</param>
    /// <returns>是否落库成功</returns>
    public static async Task<bool> TryNotifyAsync(ITaktMessageService messageService, string content)
    {
        ArgumentNullException.ThrowIfNull(messageService);
        if (string.IsNullOrWhiteSpace(content))
        {
            return false;
        }
        try
        {
            await messageService.CreateAndSendSelfOperationMessageAsync(content.Trim());
            TaktLogger.Information("[EcTePersist] 操作消息已落库 ContentLength={Length}", content.Trim().Length);
            return true;
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[EcTePersist] 操作消息落库失败");
            return false;
        }
    }

    /// <summary>
    /// 已提交后台保存
    /// </summary>
    /// <param name="ecCode">设变单号</param>
    /// <param name="isUpdate">是否更新</param>
    /// <param name="detailCount">明细行数</param>
    /// <returns>正文</returns>
    public static string BuildJobSubmitted(string ecCode, bool isUpdate, int detailCount)
    {
        var action = isUpdate ? "更新" : "新增";
        return $"已提交设变 {ecCode} 后台{action}（明细 {detailCount} 行，含各部门执行派生），完成后将通知您";
    }

    /// <summary>
    /// 后台保存完成
    /// </summary>
    /// <param name="ecCode">设变单号</param>
    /// <param name="isUpdate">是否更新</param>
    /// <param name="success">是否成功</param>
    /// <param name="durationMs">耗时毫秒</param>
    /// <param name="detailCount">明细行数</param>
    /// <param name="errorMessage">失败摘要</param>
    /// <param name="deptExecSummary">各部门派生摘要（如：采购课 12 条；部管课 10 条）</param>
    /// <returns>正文</returns>
    public static string BuildJobCompleted(
        string ecCode,
        bool isUpdate,
        bool success,
        long durationMs,
        int detailCount,
        string? errorMessage,
        string? deptExecSummary = null)
    {
        var action = isUpdate ? "更新" : "新增";
        if (success)
        {
            var summary = string.IsNullOrWhiteSpace(deptExecSummary)
                ? string.Empty
                : $"；{deptExecSummary.Trim()}";
            return $"设变 {ecCode} 后台{action}完成（明细 {detailCount} 行，耗时 {FormatDuration(durationMs)}{summary}）";
        }
        var err = string.IsNullOrWhiteSpace(errorMessage) ? "未知错误" : errorMessage.Trim();
        return $"设变 {ecCode} 后台{action}失败（明细 {detailCount} 行）：{err}";
    }

    /// <summary>
    /// 格式化耗时
    /// </summary>
    /// <param name="ms">毫秒</param>
    /// <returns>可读耗时</returns>
    private static string FormatDuration(long ms)
    {
        if (ms < 1000)
        {
            return $"{ms}ms";
        }
        var totalSeconds = (int)Math.Round(ms / 1000.0);
        if (totalSeconds < 60)
        {
            return $"{totalSeconds}s";
        }
        var minutes = totalSeconds / 60;
        var seconds = totalSeconds % 60;
        return seconds > 0 ? $"{minutes}m {seconds}s" : $"{minutes}m";
    }
}
