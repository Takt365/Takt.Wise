// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktEcAttachmentTypeConstants.cs
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：设变附件类别常量（与 TaktEcAttachment.AttachmentType 对齐）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// 设变附件文件类别（TaktEcAttachment.AttachmentType）
/// </summary>
public static class TaktEcAttachmentTypeConstants
{
    /// <summary>
    /// 联络（技联 No.，DictValue=TL）
    /// </summary>
    public const string Liaison = "TL";
    /// <summary>
    /// EPP
    /// </summary>
    public const string Epp = "EPP";
    /// <summary>
    /// FPP（P番 No.）
    /// </summary>
    public const string Fpp = "FPP";
    /// <summary>
    /// 外部联络（DictValue=EL）
    /// </summary>
    public const string ExternalLiaison = "EL";
    /// <summary>
    /// TCJ 技联
    /// </summary>
    public const string Tcj = "TCJ";
    /// <summary>
    /// 源 PDF（DictValue=源PDF）
    /// </summary>
    public const string SourcePdf = "源PDF";
    /// <summary>
    /// EC（DictValue=EC）
    /// </summary>
    public const string Ec = "EC";
}
