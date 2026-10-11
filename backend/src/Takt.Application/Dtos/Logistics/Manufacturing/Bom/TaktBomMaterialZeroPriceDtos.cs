// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Bom
// 文件名称：TaktBomMaterialZeroPriceDtos.cs
// 创建时间：2026-08-13
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 组件零价格清单 DTO（独立模块；与成本分析 DTO 分离）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Bom;

/// <summary>
/// 组件零价格合并查询（工厂+核算月；机种可选多选，空=全部）
/// </summary>
public class TaktBomMaterialZeroPriceQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 工厂代码（必填）
    /// </summary>
    [Required]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（兼容单值；与 ModelCodes 合并；空=全部机种）
    /// </summary>
    public string? ModelCode { get; set; }

    /// <summary>
    /// 机种编码多选（逗号/分号分隔；与 ModelCode 合并；空=全部机种）
    /// </summary>
    public string? ModelCodes { get; set; }

    /// <summary>
    /// 核算日期起（须与止同月）
    /// </summary>
    public DateTime? CostingDateStart { get; set; }

    /// <summary>
    /// 核算日期止（须与起同月）
    /// </summary>
    public DateTime? CostingDateEnd { get; set; }
}

/// <summary>
/// 组件零价格合并行
/// </summary>
public class TaktBomMaterialZeroPriceDto
{
    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 涉及机种（多个时逗号分隔）
    /// </summary>
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 组件编码
    /// </summary>
    public string ComponentCode { get; set; } = string.Empty;

    /// <summary>
    /// 组件描述
    /// </summary>
    public string ComponentDescription { get; set; } = string.Empty;

    /// <summary>
    /// 共用产品编码（逗号分隔）
    /// </summary>
    public string ProductCodes { get; set; } = string.Empty;

    /// <summary>
    /// 产品数
    /// </summary>
    public int ProductCount { get; set; }

    /// <summary>
    /// 移动平均价（零价清单恒为 0）
    /// </summary>
    public decimal MovingAveragePrice { get; set; }

    /// <summary>
    /// 建议代替组件
    /// </summary>
    public string? SuggestedComponentCode { get; set; }

    /// <summary>
    /// 建议代替组件的移动价格
    /// </summary>
    public decimal? SuggestedMovingPrice { get; set; }

    /// <summary>
    /// 核算月（yyyy-MM）
    /// </summary>
    public string CostingPeriod { get; set; } = string.Empty;
}

/// <summary>
/// 组件零价格合并结果
/// </summary>
public class TaktBomMaterialZeroPriceResultDto
{
    /// <summary>
    /// 分页行
    /// </summary>
    public TaktPagedResult<TaktBomMaterialZeroPriceDto> Paged { get; set; } =
        TaktPagedResult<TaktBomMaterialZeroPriceDto>.Create(
            new List<TaktBomMaterialZeroPriceDto>(), 0, 1, 20);

    /// <summary>
    /// 涉及产品编码列表
    /// </summary>
    public List<string> ProductCodes { get; set; } = new();

    /// <summary>
    /// 组件总数（合并后）
    /// </summary>
    public int ComponentCount { get; set; }

    /// <summary>
    /// 核算月（yyyy-MM）
    /// </summary>
    public string CostingPeriod { get; set; } = string.Empty;
}

/// <summary>
/// 回填移动平均价请求（当前查询条件；ComponentCode 有值=操作列单条，空=批量全部零价组件）
/// </summary>
public class TaktBomMaterialZeroPriceMovingBackfillDto
{
    /// <summary>
    /// 工厂代码（必填）
    /// </summary>
    [Required]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 零价组件编码（操作列单条必填；批量回填时为空）
    /// </summary>
    public string? ComponentCode { get; set; }

    /// <summary>
    /// 机种编码（兼容单值；与 ModelCodes 合并；与列表查询一致）
    /// </summary>
    public string? ModelCode { get; set; }

    /// <summary>
    /// 机种编码多选（逗号/分号分隔；与 ModelCode 合并）
    /// </summary>
    public string? ModelCodes { get; set; }

    /// <summary>
    /// 核算日期起（须与止同月）
    /// </summary>
    public DateTime? CostingDateStart { get; set; }

    /// <summary>
    /// 核算日期止（须与起同月）
    /// </summary>
    public DateTime? CostingDateEnd { get; set; }
}

/// <summary>
/// 手工替换更新移动平均价（原组件零价 → 指定新组件价/单位/币种回填到原组件明细）
/// </summary>
public class TaktBomMaterialZeroPriceManualMovingDto
{
    /// <summary>
    /// 工厂代码（必填）
    /// </summary>
    [Required]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 原零价组件编码（必填；回填目标）
    /// </summary>
    [Required]
    public string ComponentCode { get; set; } = string.Empty;

    /// <summary>
    /// 替换新组件编码（必填；写入 ExtField.source_component_code）
    /// </summary>
    [Required]
    public string SourceComponentCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（兼容单值；与 ModelCodes 合并）
    /// </summary>
    public string? ModelCode { get; set; }

    /// <summary>
    /// 机种编码多选（逗号/分号分隔）
    /// </summary>
    public string? ModelCodes { get; set; }

    /// <summary>
    /// 核算日期起（须与止同月）
    /// </summary>
    public DateTime? CostingDateStart { get; set; }

    /// <summary>
    /// 核算日期止（须与起同月）
    /// </summary>
    public DateTime? CostingDateEnd { get; set; }

    /// <summary>
    /// 新组件移动平均价（原值，须 &gt; 0；回填到原组件 MovingAveragePrice）
    /// </summary>
    [Required]
    public decimal MovingAveragePrice { get; set; }

    /// <summary>
    /// 价格单位（字典 logistics_materials_price_unit_param；默认 1000；回填到 MovingPriceUnit）
    /// </summary>
    public int MovingPriceUnit { get; set; } = 1000;

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；默认 CNY；回填到 MovingPriceCurrencyCode）
    /// </summary>
    public string MovingPriceCurrencyCode { get; set; } = "CNY";
}

/// <summary>
/// 回填移动平均价结果
/// </summary>
public class TaktBomMaterialZeroPriceMovingBackfillResultDto
{
    /// <summary>
    /// 扫描明细行数
    /// </summary>
    public int ScannedRowCount { get; set; }

    /// <summary>
    /// 更新明细行数
    /// </summary>
    public int UpdatedRowCount { get; set; }

    /// <summary>
    /// 无建议代替价跳过明细行数
    /// </summary>
    public int SkippedNoPriceCount { get; set; }

    /// <summary>
    /// 已有移动价未变化明细行数
    /// </summary>
    public int UnchangedRowCount { get; set; }

    /// <summary>
    /// 处理组件数（单条=0或1；批量=有建议源并尝试回填的组件数）
    /// </summary>
    public int ComponentProcessedCount { get; set; }

    /// <summary>
    /// 建议代替组件（单条时有值）
    /// </summary>
    public string? SourceComponentCode { get; set; }

    /// <summary>
    /// 源价评估期间 yyyy-MM（单条时有值）
    /// </summary>
    public string? ValuationPeriod { get; set; }

    /// <summary>
    /// 履历可读串（单条时有值；yyyy-MM：价格：单位:币种）
    /// </summary>
    public string? PriceInfo { get; set; }

    /// <summary>
    /// 更新产品月成本的主表行数（该组件涉及产品在各机种下）
    /// </summary>
    public int ProductMonthlyCostUpdatedCount { get; set; }

    /// <summary>
    /// 更新机种月成本的主表行数
    /// </summary>
    public int ModelMonthlyAverageUpdatedCount { get; set; }

    /// <summary>
    /// 核算月 yyyy-MM
    /// </summary>
    public string ProcessedMonth { get; set; } = string.Empty;

    /// <summary>
    /// 本次读写的明细物理表（选中库中的年分表，不存在则基表）
    /// </summary>
    public string ItemPhysicalTable { get; set; } = string.Empty;
}

/// <summary>
/// PCB SECT 整树 ExtField 打标请求（工厂+核算月；机种可选，空=全部）
/// </summary>
public class TaktBomMaterialZeroPricePcbSectMarkDto
{
    /// <summary>
    /// 工厂代码（必填）
    /// </summary>
    [Required]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（兼容单值；与 ModelCodes 合并）
    /// </summary>
    public string? ModelCode { get; set; }

    /// <summary>
    /// 机种编码多选（逗号/分号分隔；与 ModelCode 合并）
    /// </summary>
    public string? ModelCodes { get; set; }

    /// <summary>
    /// 核算日期起（须与止同月）
    /// </summary>
    public DateTime? CostingDateStart { get; set; }

    /// <summary>
    /// 核算日期止（须与起同月）
    /// </summary>
    public DateTime? CostingDateEnd { get; set; }
}

/// <summary>
/// PCB SECT 整树 ExtField 打标结果
/// </summary>
public class TaktBomMaterialZeroPricePcbSectMarkResultDto
{
    /// <summary>
    /// 扫描明细行数（范围内全量展开）
    /// </summary>
    public int ScannedRowCount { get; set; }

    /// <summary>
    /// PCB SECT 整树行数
    /// </summary>
    public int PcbSectRowCount { get; set; }

    /// <summary>
    /// 新写入 PcbSectIndicator=X 的行数
    /// </summary>
    public int UpdatedRowCount { get; set; }

    /// <summary>
    /// 已有标识未变化行数
    /// </summary>
    public int UnchangedRowCount { get; set; }

    /// <summary>
    /// 保留字段（标识列写入恒为 0；兼容旧客户端）
    /// </summary>
    public int SkippedOverflowCount { get; set; }

    /// <summary>
    /// 核算月 yyyy-MM
    /// </summary>
    public string ProcessedMonth { get; set; } = string.Empty;
}

/// <summary>
/// 去掉纯数字编码前导 0（当前工厂；成本汇总产品编码，明细产品编码与组件编码）
/// </summary>
public class TaktBomMaterialZeroPriceStripLeadingZeroDto
{
    /// <summary>
    /// 工厂代码（必填）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;
}

/// <summary>
/// 去前导 0 结果
/// </summary>
public class TaktBomMaterialZeroPriceStripLeadingZeroResultDto
{
    /// <summary>
    /// 成本汇总表产品编码更新行数
    /// </summary>
    public int HeaderProductUpdatedCount { get; set; }

    /// <summary>
    /// 明细表产品编码更新行数
    /// </summary>
    public int ItemProductUpdatedCount { get; set; }

    /// <summary>
    /// 明细表组件编码更新行数
    /// </summary>
    public int ItemComponentUpdatedCount { get; set; }

    /// <summary>
    /// 去掉前导 0 后与已有唯一键冲突而跳过的行数
    /// </summary>
    public int SkippedConflictCount { get; set; }
}
