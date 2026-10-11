// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktBomOperationMessageConstants.cs
// 创建时间：2026-08-17
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 业务操作结果落库在线消息的 MessageType/MessageGroup（导出不落库）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// BOM 业务操作结果消息常量（落库 TaktMessage；与 Quartz 执行完成同为 system/reminder）
/// </summary>
public static class TaktBomOperationMessageConstants
{
    /// <summary>
    /// 消息类型（字典/约定：system）
    /// </summary>
    public const string MessageType = "system";

    /// <summary>
    /// 消息分组（字典/约定：reminder）
    /// </summary>
    public const string MessageGroup = "reminder";
}
