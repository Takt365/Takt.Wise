// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Interfaces
// 文件名称：ITaktQuartzJobHandler.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz 程序集任务处理器接口（HandlerKey 与 TaktQuartzTask.ClassName 对齐）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.Foundation;

namespace Takt.Domain.Interfaces;

/// <summary>
/// Quartz 任务执行上下文
/// </summary>
public class TaktQuartzJobContext
{
    /// <summary>
    /// 定时任务实体
    /// </summary>
    public TaktQuartzTask Task { get; set; } = null!;

    /// <summary>
    /// 执行参数
    /// </summary>
    public string? ExecuteParams { get; set; }

    /// <summary>
    /// 触发用户
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// 执行摘要（Handler 写入；落库 TaktQuartzLog.ExecuteMessage 并随完成消息推送）
    /// </summary>
    public string? ExecuteMessage { get; set; }
}

/// <summary>
/// Quartz 程序集任务处理器接口
/// </summary>
public interface ITaktQuartzJobHandler
{
    /// <summary>
    /// 处理器键（与 TaktQuartzTask.ClassName 匹配）
    /// </summary>
    string HandlerKey { get; }

    /// <summary>
    /// 执行任务逻辑
    /// </summary>
    /// <param name="context">执行上下文</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>任务</returns>
    Task ExecuteAsync(TaktQuartzJobContext context, CancellationToken cancellationToken = default);
}
