// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Bom
// 文件名称：TaktBomPriceDeltaTrendDtos.cs
// 创建时间：2026-08-13
// 创建人：Takt365(Cursor AI)
// 功能描述：成本差异推移 DTO（产品月成本 + 0价格组 + 价格差异组 PriceDeltaTrend）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Bom;

/// <summary>
/// 成本差异推移查询
/// </summary>
public class TaktBomPriceDeltaTrendQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 工厂代码（必填）
    /// </summary>
    [Required]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（必选语义；空=默认 FERT）。与工厂/期间/机种/产品共同限定主表产品，再算 0价格组与价格差异组
    /// </summary>
    public string? MaterialType { get; set; }

    /// <summary>
    /// 机种编码（可选）
    /// </summary>
    public string? ModelCode { get; set; }

    /// <summary>
    /// 产品编码（可选）
    /// </summary>
    public string? ProductCode { get; set; }

    /// <summary>
    /// 核算日起
    /// </summary>
    public DateTime? CostingDateStart { get; set; }

    /// <summary>
    /// 核算日止
    /// </summary>
    public DateTime? CostingDateEnd { get; set; }

    /// <summary>
    /// 关注月 yyyy-MM（兼容旧参数；空则用基准月 BasePeriod；再空则期间止）
    /// </summary>
    public string? FocusPeriod { get; set; }

    /// <summary>
    /// 基准月 yyyy-MM（关注月；默认期间止）。必须落在核算期间列内。差异=基准月−比较月；0价格组按此月
    /// </summary>
    public string? BasePeriod { get; set; }

    /// <summary>
    /// 比较月 yyyy-MM（基期；默认基准月减一月且须仍在期间列内）。须与基准月同属核算期间列，可不相邻
    /// </summary>
    public string? ComparePeriod { get; set; }

    /// <summary>
    /// 差异选项：all=全部（默认）；gt1/gt5/gt10/gt50/gt100=仅保留 |差异|≥该值的产品行（差异=基准月−比较月，主表月成本）。界面为 >=1 等符号（四语不翻译）。改变产品行集合与分页总数。
    /// </summary>
    public string? PriceDeltaOption { get; set; }
}

/// <summary>
/// 成本差异推移行
/// </summary>
public class TaktBomPriceDeltaTrendDto
{
    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码
    /// </summary>
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品编码
    /// </summary>
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品描述
    /// </summary>
    public string ProductDescription { get; set; } = string.Empty;

    /// <summary>
    /// 各月产品成本（键 yyyy-MM；仅查询期间内）
    /// </summary>
    public Dictionary<string, decimal> PeriodCosts { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// 差异：价格差异组 Summary Var + 组件差异 Summary Var。
    /// 比较月/基准月任一产品成本为 0，或环比差异为 0 → 差异=0，且 0价格组/价格差异组/组件差异组全空。
    /// </summary>
    public decimal? PriceDelta { get; set; }

    /// <summary>
    /// 0价格组：关注月移动平均价=0；格式 物料:用量:可替代:替代价, …；用量=期间末日快照按 BuildComponentKey 去重后合计；可替代按末字母 Z→A 逆推
    /// </summary>
    public string ZeroPriceGroup { get; set; } = string.Empty;

    /// <summary>
    /// 价格差异组：同编码行成本变动；格式 组件:用量:基期价→关注价,Diff:差价×用量, …,Summary Var:Diff 合计；用量同合并合计口径
    /// </summary>
    public string PriceDeltaTrend { get; set; } = string.Empty;

    /// <summary>
    /// 组件差异：new/remove/version；格式 …,Summary Var:N-{新增}-R-{删除}={净值}；用量同合并合计口径
    /// </summary>
    public string ComponentDeltaGroup { get; set; } = string.Empty;

    /// <summary>
    /// 改修：该产品在基准月内实际开始、工单类别 ZDTB 的生产工单号清单（逗号分隔）。按产品编码匹配 TaktProductionOrder.MaterialCode。
    /// </summary>
    public string ReworkOrderGroup { get; set; } = string.Empty;

    /// <summary>
    /// 建议替代价格：仅本产品明细 ExtField._bk.mp（无 scope）；格式 零价组件:用量→源替代:单价×用量；禁止主表履历串产品
    /// </summary>
    public string ReplaceComponentGroup { get; set; } = string.Empty;

    /// <summary>
    /// 基准月（关注月）
    /// </summary>
    public string? BasePeriod { get; set; }

    /// <summary>
    /// 比较月（基期）
    /// </summary>
    public string? ComparePeriod { get; set; }
}

/// <summary>
/// 成本差异推移结果
/// </summary>
public class TaktBomPriceDeltaTrendResultDto
{
    /// <summary>
    /// 分页行
    /// </summary>
    public TaktPagedResult<TaktBomPriceDeltaTrendDto> Paged { get; set; } =
        TaktPagedResult<TaktBomPriceDeltaTrendDto>.Create(
            new List<TaktBomPriceDeltaTrendDto>(), 0, 1, 20);

    /// <summary>
    /// 期间列顺序 yyyy-MM
    /// </summary>
    public List<string> PeriodOrder { get; set; } = new();

    /// <summary>
    /// 基准月（关注月）
    /// </summary>
    public string? BasePeriod { get; set; }

    /// <summary>
    /// 比较月（基期）
    /// </summary>
    public string? ComparePeriod { get; set; }
}
