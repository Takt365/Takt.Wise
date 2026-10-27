// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Bom
// 文件名称：TaktBomMaterialUsageDtos.cs
// 创建时间：2026-10-21
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 通用物料 / 专用物料查询与行 DTO
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Bom;

/// <summary>
/// 通用物料 / 专用物料查询（工厂 + 核算年月 yyyy-MM；物料与机种二选一，均可空；取该月成品 FERT）
/// </summary>
public class TaktBomMaterialUsageQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 工厂代码（必填）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（可选；有值时机种条件忽略，只保留该物料）
    /// </summary>
    public string? ComponentCode { get; set; }

    /// <summary>
    /// 机种编码（可选；只过滤结果行。是否专用仍按该工厂该年月全部机种计数。已选物料时忽略）
    /// </summary>
    public string? ModelCode { get; set; }

    /// <summary>
    /// 核算年月（yyyy-MM）
    /// </summary>
    public string? CostingPeriod { get; set; }
}

/// <summary>
/// 通用物料或专用物料行（一个机种的一个物料）
/// </summary>
public class TaktBomMaterialUsageDto
{
    /// <summary>
    /// 行键（机种|物料）
    /// </summary>
    public string UsageKey { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 核算年月（yyyy-MM）
    /// </summary>
    public string CostingPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码
    /// </summary>
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码
    /// </summary>
    public string ComponentCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述
    /// </summary>
    public string ComponentDescription { get; set; } = string.Empty;

    /// <summary>
    /// 该机种中用到此物料的成品数
    /// </summary>
    public int ProductCount { get; set; }

    /// <summary>
    /// 用量清单：首行 Net 为行成本合计，其后 成品:用量-行成本
    /// </summary>
    public string UsageText { get; set; } = string.Empty;

    /// <summary>
    /// 行成本合计
    /// </summary>
    public decimal LineCost { get; set; }
}
