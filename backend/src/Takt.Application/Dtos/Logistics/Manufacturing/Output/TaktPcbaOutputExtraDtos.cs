// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Output
// 文件名称：TaktPcbaOutputExtraDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA日报附加 DTO（非实体 CRUD；独立文件，generate-dtos 不会覆盖）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Logistics.Manufacturing.Output;

// ========================================
// PCBA 默认明细预览（非实体 CRUD）
// ========================================

/// <summary>
/// 按标准工时生成的默认明细预览行
/// </summary>
public class TaktPcbaOutputDefaultDetailDto
{
    /// <summary>
    /// 行号（10 起递增）
    /// </summary>
    public int LineNumber { get; set; }

    /// <summary>
    /// 工作中心
    /// </summary>
    public string WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 工序描述
    /// </summary>
    public string? OperationDesc { get; set; }

    /// <summary>
    /// 标准工时（短）
    /// </summary>
    public int StandardShorts { get; set; }

    /// <summary>
    /// 标准工时（分钟；ConvertedMinutes 优先，否则 StandardMinutes）
    /// </summary>
    public decimal StandardMinutes { get; set; }
}
