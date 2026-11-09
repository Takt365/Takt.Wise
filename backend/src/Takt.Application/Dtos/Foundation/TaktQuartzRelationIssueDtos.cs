// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Foundation
// 文件名称：TaktQuartzRelationIssueDtos.cs
// 创建时间：2026-11-04
// 创建人：Takt365(Cursor AI)
// 功能描述：同步前主子表关系清单（孤儿明细 / 空头凭证）分页与导出入参
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Models;

namespace Takt.Application.Dtos.Foundation;

/// <summary>
/// 主子表关系问题行（只含定位键，不回传整行）
/// </summary>
public class TaktQuartzRelationIssueRowDto
{
    /// <summary>
    /// 问题类型：orphan 孤儿明细，empty 空头凭证
    /// </summary>
    public string IssueKind { get; set; } = string.Empty;

    /// <summary>
    /// 业务编码（单据号 / 条件号）
    /// </summary>
    public string BusinessCode { get; set; } = string.Empty;

    /// <summary>
    /// 明细行号；空头凭证为空
    /// </summary>
    public string LineNumber { get; set; } = string.Empty;

    /// <summary>
    /// 源表主键（雪花或数值，字符串避免精度丢失）
    /// </summary>
    public string RowId { get; set; } = string.Empty;
}

/// <summary>
/// 关系清单分页查询
/// </summary>
public class TaktQuartzRelationIssueQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 脚本文件名，如 sync_po.sql
    /// </summary>
    public string Script { get; set; } = string.Empty;

    /// <summary>
    /// 源库名，如 zTakt_900_Dev；设变脚本忽略此值，固定查 Sap_Data
    /// </summary>
    public string SourceDatabase { get; set; } = string.Empty;

    /// <summary>
    /// orphan 或 empty
    /// </summary>
    public string IssueKind { get; set; } = string.Empty;
}
