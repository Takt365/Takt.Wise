// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Defect
// 文件名称：TaktAssyBatchDefectDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：AssyBatchDefect 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktAssyBatchDefect 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Defect;

// ========================================
// AssyBatchDefect 响应 DTO
// ========================================

/// <summary>
/// 组立批量不良统计实体（统计维度：生产类别+批次）
/// 对应前端 TaktAssyBatchDefectDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktAssyBatchDefectDto : TaktCompanyDtoBase
{
    /// <summary>
    /// AssyBatchDefectID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyBatchDefectId { get; set; }


    /// <summary>
    /// 生产类别（统计维度，字典 logistics_manufacturing_prod_category，存 DictValue：EPP/FPP/RWP/MDP/CPP）
    /// </summary>
    public string ProdCategory { get; set; } = string.Empty;

    /// <summary>
    /// 批次（统计维度）
    /// </summary>
    public string BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期组（与生产工单组一一对应，yyyy-MM-dd 逗号分隔，取同工单最早生产日期）
    /// </summary>
    public string? ProdDateGroup { get; set; } = string.Empty;

    /// <summary>
    /// 生产工单组（同批次 Distinct 工单号逗号分隔，与生产日期组、生产物料组、订单数量组一一对应）
    /// </summary>
    public string? ProdOrderGroup { get; set; } = string.Empty;

    /// <summary>
    /// 机种（取最近日报）
    /// </summary>
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产物料组（与生产工单组一一对应，逗号分隔，同工单取最近日报物料编码）
    /// </summary>
    public string? MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 批次工单总数量（同批次下各生产工单订单数量汇总：同工单取最大订单数量再合计）
    /// </summary>
    public decimal BatchOrderQty { get; set; }

    /// <summary>
    /// 订单数量组（与生产工单组一一对应，逗号分隔，同工单取最大订单数量）
    /// </summary>
    public string? ProdOrderQtyGroup { get; set; } = string.Empty;

    /// <summary>
    /// 累计生实实绩（汇总 TaktAssyDefect.ProdActualQty）
    /// </summary>
    public decimal ProdActualQty { get; set; }

    /// <summary>
    /// 累计无不良数量（汇总 TaktAssyDefect.GoodQuantity）
    /// </summary>
    public decimal GoodQuantity { get; set; }

    /// <summary>
    /// 累计不良数量（计算：累计生实实绩 - 累计无不良数量）
    /// </summary>
    public decimal DefectQty { get; set; }

    /// <summary>
    /// 不良率（%，计算：累计不良数量 ÷ 累计生实实绩 × 100）
    /// </summary>
    public decimal DefectRatePercent { get; set; }

    /// <summary>
    /// 直行率（%，计算：累计无不良数量 ÷ 累计生实实绩 × 100）
    /// </summary>
    public decimal YieldRatePercent { get; set; }

    /// <summary>
    /// 最近生产日期（关联日报最大 ProdDate）
    /// </summary>
    public DateTime? LastProdDate { get; set; }

    /// <summary>
    /// 关联组立不良日报笔数
    /// </summary>
    public int ReportCount { get; set; } = 0;

    /// <summary>
    /// 批次状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成；批次工单总数量与累计生实实绩相等时为已完成）
    /// </summary>
    public int BatchStatus { get; set; } = 0;

}

// ========================================
// AssyBatchDefect 查询 DTO
// ========================================

/// <summary>
/// AssyBatchDefect 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktAssyBatchDefectQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（取最近日报，关联 TaktPlant.PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产类别（统计维度，字典 logistics_manufacturing_prod_category，存 DictValue：EPP/FPP/RWP/MDP/CPP）
    /// </summary>
    public string? ProdCategory { get; set; } = string.Empty;

    /// <summary>
    /// 批次（统计维度）
    /// </summary>
    public string? BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期组（与生产工单组一一对应，yyyy-MM-dd 逗号分隔，取同工单最早生产日期）
    /// </summary>
    public string? ProdDateGroup { get; set; } = string.Empty;

    /// <summary>
    /// 生产工单组（同批次 Distinct 工单号逗号分隔，与生产日期组、生产物料组、订单数量组一一对应）
    /// </summary>
    public string? ProdOrderGroup { get; set; } = string.Empty;

    /// <summary>
    /// 机种（取最近日报）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产物料组（与生产工单组一一对应，逗号分隔，同工单取最近日报物料编码）
    /// </summary>
    public string? MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 批次工单总数量（同批次下各生产工单订单数量汇总：同工单取最大订单数量再合计）
    /// </summary>
    public decimal? BatchOrderQty { get; set; }

    /// <summary>
    /// 订单数量组（与生产工单组一一对应，逗号分隔，同工单取最大订单数量）
    /// </summary>
    public string? ProdOrderQtyGroup { get; set; } = string.Empty;

    /// <summary>
    /// 累计生实实绩（汇总 TaktAssyDefect.ProdActualQty）
    /// </summary>
    public decimal? ProdActualQty { get; set; }

    /// <summary>
    /// 累计无不良数量（汇总 TaktAssyDefect.GoodQuantity）
    /// </summary>
    public decimal? GoodQuantity { get; set; }

    /// <summary>
    /// 累计不良数量（计算：累计生实实绩 - 累计无不良数量）
    /// </summary>
    public decimal? DefectQty { get; set; }

    /// <summary>
    /// 不良率（%，计算：累计不良数量 ÷ 累计生实实绩 × 100）
    /// </summary>
    public decimal? DefectRatePercent { get; set; }

    /// <summary>
    /// 直行率（%，计算：累计无不良数量 ÷ 累计生实实绩 × 100）
    /// </summary>
    public decimal? YieldRatePercent { get; set; }

    /// <summary>
    /// 最近生产日期（关联日报最大 ProdDate）（范围查询-开始）
    /// </summary>
    public DateTime? LastProdDateStart { get; set; }

    /// <summary>
    /// 最近生产日期（关联日报最大 ProdDate）（范围查询-结束）
    /// </summary>
    public DateTime? LastProdDateEnd { get; set; }

    /// <summary>
    /// 关联组立不良日报笔数
    /// </summary>
    public int? ReportCount { get; set; }

    /// <summary>
    /// 批次状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成；批次工单总数量与累计生实实绩相等时为已完成）
    /// </summary>
    public int? BatchStatus { get; set; }

    /// <summary>
    /// 创建时间（范围查询-开始）
    /// </summary>
    public DateTime? CreatedAtStart { get; set; }

    /// <summary>
    /// 创建时间（范围查询-结束）
    /// </summary>
    public DateTime? CreatedAtEnd { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建AssyBatchDefect DTO
// ========================================

/// <summary>
/// 创建AssyBatchDefect DTO
/// </summary>
public class TaktAssyBatchDefectCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（取最近日报，关联 TaktPlant.PlantCode）
    /// </summary>
    [Required(ErrorMessage = "工厂代码（取最近日报，关联 TaktPlant.PlantCode）不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产类别（统计维度，字典 logistics_manufacturing_prod_category，存 DictValue：EPP/FPP/RWP/MDP/CPP）
    /// </summary>
    [Required(ErrorMessage = "生产类别（统计维度，字典 logistics_manufacturing_prod_category，存 DictValue：EPP/FPP/RWP/MDP/CPP）不能为空")]
    public string ProdCategory { get; set; } = string.Empty;

    /// <summary>
    /// 批次（统计维度）
    /// </summary>
    [Required(ErrorMessage = "批次（统计维度）不能为空")]
    public string BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期组（与生产工单组一一对应，yyyy-MM-dd 逗号分隔，取同工单最早生产日期）
    /// </summary>
    public string? ProdDateGroup { get; set; } = string.Empty;

    /// <summary>
    /// 生产工单组（同批次 Distinct 工单号逗号分隔，与生产日期组、生产物料组、订单数量组一一对应）
    /// </summary>
    public string? ProdOrderGroup { get; set; } = string.Empty;

    /// <summary>
    /// 机种（取最近日报）
    /// </summary>
    [Required(ErrorMessage = "机种（取最近日报）不能为空")]
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产物料组（与生产工单组一一对应，逗号分隔，同工单取最近日报物料编码）
    /// </summary>
    public string? MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 批次工单总数量（同批次下各生产工单订单数量汇总：同工单取最大订单数量再合计）
    /// </summary>
    public decimal BatchOrderQty { get; set; }

    /// <summary>
    /// 订单数量组（与生产工单组一一对应，逗号分隔，同工单取最大订单数量）
    /// </summary>
    public string? ProdOrderQtyGroup { get; set; } = string.Empty;

    /// <summary>
    /// 累计生实实绩（汇总 TaktAssyDefect.ProdActualQty）
    /// </summary>
    public decimal ProdActualQty { get; set; }

    /// <summary>
    /// 累计无不良数量（汇总 TaktAssyDefect.GoodQuantity）
    /// </summary>
    public decimal GoodQuantity { get; set; }

    /// <summary>
    /// 累计不良数量（计算：累计生实实绩 - 累计无不良数量）
    /// </summary>
    public decimal DefectQty { get; set; }

    /// <summary>
    /// 不良率（%，计算：累计不良数量 ÷ 累计生实实绩 × 100）
    /// </summary>
    public decimal DefectRatePercent { get; set; }

    /// <summary>
    /// 直行率（%，计算：累计无不良数量 ÷ 累计生实实绩 × 100）
    /// </summary>
    public decimal YieldRatePercent { get; set; }

    /// <summary>
    /// 最近生产日期（关联日报最大 ProdDate）
    /// </summary>
    public DateTime? LastProdDate { get; set; }

    /// <summary>
    /// 关联组立不良日报笔数
    /// </summary>
    public int ReportCount { get; set; } = 0;

    /// <summary>
    /// 批次状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成；批次工单总数量与累计生实实绩相等时为已完成）
    /// </summary>
    public int BatchStatus { get; set; } = 0;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 更新AssyBatchDefect DTO
// ========================================

/// <summary>
/// 更新AssyBatchDefect DTO
/// 继承 TaktAssyBatchDefectCreateDto，添加 AssyBatchDefectId 字段
/// </summary>
public class TaktAssyBatchDefectUpdateDto : TaktAssyBatchDefectCreateDto
{
    /// <summary>
    /// AssyBatchDefectID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyBatchDefectId { get; set; }

}

// ========================================
// AssyBatchDefect 状态 DTO
// ========================================

/// <summary>
/// AssyBatchDefect 状态更新 DTO
/// </summary>
public class TaktAssyBatchDefectStatusDto
{
    /// <summary>
    /// AssyBatchDefectID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyBatchDefectId { get; set; }

    /// <summary>
    /// 批次状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成；批次工单总数量与累计生实实绩相等时为已完成）
    /// </summary>
    [Required(ErrorMessage = "批次状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成；批次工单总数量与累计生实实绩相等时为已完成）不能为空")]
    public int BatchStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// AssyBatchDefect 导入模板行 DTO
/// </summary>
public class TaktAssyBatchDefectTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（取最近日报，关联 TaktPlant.PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产类别（统计维度，字典 logistics_manufacturing_prod_category，存 DictValue：EPP/FPP/RWP/MDP/CPP）
    /// </summary>
    public string? ProdCategory { get; set; } = string.Empty;

    /// <summary>
    /// 批次（统计维度）
    /// </summary>
    public string? BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期组（与生产工单组一一对应，yyyy-MM-dd 逗号分隔，取同工单最早生产日期）
    /// </summary>
    public string? ProdDateGroup { get; set; } = string.Empty;

    /// <summary>
    /// 生产工单组（同批次 Distinct 工单号逗号分隔，与生产日期组、生产物料组、订单数量组一一对应）
    /// </summary>
    public string? ProdOrderGroup { get; set; } = string.Empty;

    /// <summary>
    /// 机种（取最近日报）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产物料组（与生产工单组一一对应，逗号分隔，同工单取最近日报物料编码）
    /// </summary>
    public string? MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 批次工单总数量（同批次下各生产工单订单数量汇总：同工单取最大订单数量再合计）
    /// </summary>
    public decimal? BatchOrderQty { get; set; }

    /// <summary>
    /// 订单数量组（与生产工单组一一对应，逗号分隔，同工单取最大订单数量）
    /// </summary>
    public string? ProdOrderQtyGroup { get; set; } = string.Empty;

    /// <summary>
    /// 累计生实实绩（汇总 TaktAssyDefect.ProdActualQty）
    /// </summary>
    public decimal? ProdActualQty { get; set; }

    /// <summary>
    /// 累计无不良数量（汇总 TaktAssyDefect.GoodQuantity）
    /// </summary>
    public decimal? GoodQuantity { get; set; }

    /// <summary>
    /// 累计不良数量（计算：累计生实实绩 - 累计无不良数量）
    /// </summary>
    public decimal? DefectQty { get; set; }

    /// <summary>
    /// 不良率（%，计算：累计不良数量 ÷ 累计生实实绩 × 100）
    /// </summary>
    public decimal? DefectRatePercent { get; set; }

    /// <summary>
    /// 直行率（%，计算：累计无不良数量 ÷ 累计生实实绩 × 100）
    /// </summary>
    public decimal? YieldRatePercent { get; set; }

    /// <summary>
    /// 最近生产日期（关联日报最大 ProdDate）
    /// </summary>
    public DateTime? LastProdDate { get; set; }

    /// <summary>
    /// 关联组立不良日报笔数
    /// </summary>
    public int? ReportCount { get; set; }

    /// <summary>
    /// 批次状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成；批次工单总数量与累计生实实绩相等时为已完成）
    /// </summary>
    public int? BatchStatus { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// AssyBatchDefect 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktAssyBatchDefectImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（取最近日报，关联 TaktPlant.PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产类别（统计维度，字典 logistics_manufacturing_prod_category，存 DictValue：EPP/FPP/RWP/MDP/CPP）
    /// </summary>
    public string? ProdCategory { get; set; } = string.Empty;

    /// <summary>
    /// 批次（统计维度）
    /// </summary>
    public string? BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期组（与生产工单组一一对应，yyyy-MM-dd 逗号分隔，取同工单最早生产日期）
    /// </summary>
    public string? ProdDateGroup { get; set; } = string.Empty;

    /// <summary>
    /// 生产工单组（同批次 Distinct 工单号逗号分隔，与生产日期组、生产物料组、订单数量组一一对应）
    /// </summary>
    public string? ProdOrderGroup { get; set; } = string.Empty;

    /// <summary>
    /// 机种（取最近日报）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产物料组（与生产工单组一一对应，逗号分隔，同工单取最近日报物料编码）
    /// </summary>
    public string? MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 批次工单总数量（同批次下各生产工单订单数量汇总：同工单取最大订单数量再合计）
    /// </summary>
    public decimal? BatchOrderQty { get; set; }

    /// <summary>
    /// 订单数量组（与生产工单组一一对应，逗号分隔，同工单取最大订单数量）
    /// </summary>
    public string? ProdOrderQtyGroup { get; set; } = string.Empty;

    /// <summary>
    /// 累计生实实绩（汇总 TaktAssyDefect.ProdActualQty）
    /// </summary>
    public decimal? ProdActualQty { get; set; }

    /// <summary>
    /// 累计无不良数量（汇总 TaktAssyDefect.GoodQuantity）
    /// </summary>
    public decimal? GoodQuantity { get; set; }

    /// <summary>
    /// 累计不良数量（计算：累计生实实绩 - 累计无不良数量）
    /// </summary>
    public decimal? DefectQty { get; set; }

    /// <summary>
    /// 不良率（%，计算：累计不良数量 ÷ 累计生实实绩 × 100）
    /// </summary>
    public decimal? DefectRatePercent { get; set; }

    /// <summary>
    /// 直行率（%，计算：累计无不良数量 ÷ 累计生实实绩 × 100）
    /// </summary>
    public decimal? YieldRatePercent { get; set; }

    /// <summary>
    /// 最近生产日期（关联日报最大 ProdDate）
    /// </summary>
    public DateTime? LastProdDate { get; set; }

    /// <summary>
    /// 关联组立不良日报笔数
    /// </summary>
    public int? ReportCount { get; set; }

    /// <summary>
    /// 批次状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成；批次工单总数量与累计生实实绩相等时为已完成）
    /// </summary>
    public int? BatchStatus { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// AssyBatchDefect 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktAssyBatchDefectExportDto
{
    /// <summary>
    /// AssyBatchDefectID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyBatchDefectId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（取最近日报，关联 TaktPlant.PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产类别（统计维度，字典 logistics_manufacturing_prod_category，存 DictValue：EPP/FPP/RWP/MDP/CPP）
    /// </summary>
    public string ProdCategory { get; set; } = string.Empty;

    /// <summary>
    /// 批次（统计维度）
    /// </summary>
    public string BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期组（与生产工单组一一对应，yyyy-MM-dd 逗号分隔，取同工单最早生产日期）
    /// </summary>
    public string? ProdDateGroup { get; set; } = string.Empty;

    /// <summary>
    /// 生产工单组（同批次 Distinct 工单号逗号分隔，与生产日期组、生产物料组、订单数量组一一对应）
    /// </summary>
    public string? ProdOrderGroup { get; set; } = string.Empty;

    /// <summary>
    /// 机种（取最近日报）
    /// </summary>
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产物料组（与生产工单组一一对应，逗号分隔，同工单取最近日报物料编码）
    /// </summary>
    public string? MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 批次工单总数量（同批次下各生产工单订单数量汇总：同工单取最大订单数量再合计）
    /// </summary>
    public decimal BatchOrderQty { get; set; }

    /// <summary>
    /// 订单数量组（与生产工单组一一对应，逗号分隔，同工单取最大订单数量）
    /// </summary>
    public string? ProdOrderQtyGroup { get; set; } = string.Empty;

    /// <summary>
    /// 累计生实实绩（汇总 TaktAssyDefect.ProdActualQty）
    /// </summary>
    public decimal ProdActualQty { get; set; }

    /// <summary>
    /// 累计无不良数量（汇总 TaktAssyDefect.GoodQuantity）
    /// </summary>
    public decimal GoodQuantity { get; set; }

    /// <summary>
    /// 累计不良数量（计算：累计生实实绩 - 累计无不良数量）
    /// </summary>
    public decimal DefectQty { get; set; }

    /// <summary>
    /// 不良率（%，计算：累计不良数量 ÷ 累计生实实绩 × 100）
    /// </summary>
    public decimal DefectRatePercent { get; set; }

    /// <summary>
    /// 直行率（%，计算：累计无不良数量 ÷ 累计生实实绩 × 100）
    /// </summary>
    public decimal YieldRatePercent { get; set; }

    /// <summary>
    /// 最近生产日期（关联日报最大 ProdDate）
    /// </summary>
    public DateTime? LastProdDate { get; set; }

    /// <summary>
    /// 关联组立不良日报笔数
    /// </summary>
    public int ReportCount { get; set; } = 0;

    /// <summary>
    /// 批次状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成；批次工单总数量与累计生实实绩相等时为已完成）
    /// </summary>
    public int BatchStatus { get; set; } = 0;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
