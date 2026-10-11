// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Bom
// 文件名称：TaktBomCostOptionDtos.cs
// 创建时间：2026-08-21
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 成本查询栏共用选项 DTO（工厂 / 期间 / 机种 / 产品 / 物料）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 成本查询栏选项查询（五页共用：工厂 + 期间；物料类型/机种/关键字按接口选用）
/// <para>头表 takt_logistics_manufacturing_bom_material_cost（IsDeleted=0）。物料接口读明细表组件。</para>
/// </summary>
public class TaktBomCostOptionDto
{
    /// <summary>
    /// 工厂代码（必填；空则返回空列表）
    /// </summary>
    [Required]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 期间起 yyyy-MM（必填；单月时与止相同）
    /// </summary>
    public string? PeriodStart { get; set; }

    /// <summary>
    /// 期间止 yyyy-MM（可空=与起相同）
    /// </summary>
    public string? PeriodEnd { get; set; }

    /// <summary>
    /// 物料类型（本表 MaterialType；空=不过滤）
    /// </summary>
    public string? MaterialType { get; set; }

    /// <summary>
    /// 机种编码（产品/物料选项可空过滤；空=不过滤）
    /// </summary>
    public string? ModelCode { get; set; }

    /// <summary>
    /// 机种编码多选（逗号分隔；物料选项可空过滤；空=不过滤）
    /// </summary>
    public string? ModelCodes { get; set; }

    /// <summary>
    /// 产品编码（物料选项可空过滤；空=不过滤）
    /// </summary>
    public string? ProductCode { get; set; }

    /// <summary>
    /// 远程搜索关键字（机种：编码/名称；物料：组件编码/描述；可空=全量去重）
    /// </summary>
    public string? Keyword { get; set; }
}
