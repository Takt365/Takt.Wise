// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcStatDtos.cs
// 创建时间：2026-07-01
// 创建人：Takt365(Cursor AI)
// 功能描述：设变（EngineeringChange）统计 DTO（看板 stat / 部门执行统计）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变技术课主表统计（看板 TaktEcTes/stat）
/// </summary>
public class TaktEcTeStatDto
{
    /// <summary>
    /// 统计月份（yyyy-MM）
    /// </summary>
    public string StatMonth { get; set; } = string.Empty;

    /// <summary>
    /// 设变主表数量（distinct EcTeId）
    /// </summary>
    public int EcCount { get; set; }

    /// <summary>
    /// 设变明细数量
    /// </summary>
    public int EcDetailCount { get; set; }

    /// <summary>
    /// 工厂代码（筛选条件回显）
    /// </summary>
    public string? PlantCode { get; set; }
}

/// <summary>
/// 设变技术课主表统计查询 DTO
/// </summary>
public class TaktEcTeStatQueryDto
{
    /// <summary>
    /// 录入日期（范围-开始；默认当月 1 日）
    /// </summary>
    public DateTime? EcEntryDateStart { get; set; }

    /// <summary>
    /// 录入日期（范围-结束；默认当月最后一日）
    /// </summary>
    public DateTime? EcEntryDateEnd { get; set; }

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string? PlantCode { get; set; }
}

/// <summary>
/// 设变部门执行统计（设变单数 + 明细数 + 部门行数）
/// </summary>
public class TaktEcExecStatDto
{
    /// <summary>
    /// 统计月份（yyyy-MM）
    /// </summary>
    public string StatMonth { get; set; } = string.Empty;

    /// <summary>
    /// 设变主表数量（distinct EcTeId）
    /// </summary>
    public int EcCount { get; set; }

    /// <summary>
    /// 设变明细数量
    /// </summary>
    public int EcDetailCount { get; set; }

    /// <summary>
    /// 设变部门执行行数量
    /// </summary>
    public int EcExecCount { get; set; }

    /// <summary>
    /// 部门编码（筛选条件回显）
    /// </summary>
    public string? DeptCode { get; set; }
}

/// <summary>
/// 设变部门执行统计查询 DTO
/// </summary>
public class TaktEcExecStatQueryDto
{
    /// <summary>
    /// 录入日期（范围-开始；默认当月 1 日）
    /// </summary>
    public DateTime? EcEntryDateStart { get; set; }

    /// <summary>
    /// 录入日期（范围-结束；默认当月最后一日）
    /// </summary>
    public DateTime? EcEntryDateEnd { get; set; }

    /// <summary>
    /// 部门编码
    /// </summary>
    public string? DeptCode { get; set; }

    /// <summary>
    /// 是否实施（0=否 1=是）
    /// </summary>
    public int? IsImplemented { get; set; }
}
