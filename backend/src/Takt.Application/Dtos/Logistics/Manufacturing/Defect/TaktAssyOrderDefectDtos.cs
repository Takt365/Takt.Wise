// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Defect
// 文件名称：TaktAssyOrderDefectDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：AssyOrderDefect 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktAssyOrderDefect 生成，请按需审阅）
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
// AssyOrderDefect 响应 DTO
// ========================================

/// <summary>
/// 组立工单不良统计实体（统计维度：生产类别+工单号）
/// 对应前端 TaktAssyOrderDefectDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktAssyOrderDefectDto : TaktCompanyDtoBase
{
    /// <summary>
    /// AssyOrderDefectID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyOrderDefectId { get; set; }


    /// <summary>
    /// 生产类别（统计维度，字典 logistics_manufacturing_prod_category，存 DictValue：EPP/FPP/RWP/MDP/CPP）
    /// </summary>
    public string ProdCategory { get; set; } = string.Empty;

    /// <summary>
    /// 工单号（统计维度，选项 TaktProductionOrders/options）
    /// </summary>
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期组（汇总日报去重生产日期，yyyy-MM-dd 逗号分隔升序排列）
    /// </summary>
    public string? ProdDateGroup { get; set; } = string.Empty;

    /// <summary>
    /// 机种（取最近日报）
    /// </summary>
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（取最近日报）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次（一工单一批次，取最近日报）
    /// </summary>
    public string? BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 工单数量（取最近日报）
    /// </summary>
    public decimal ProdOrderQty { get; set; }

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
    /// 工单状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成；工单数量与累计生实实绩相等时为已完成）
    /// </summary>
    public int OrderStatus { get; set; } = 0;

}

// ========================================
// AssyOrderDefect 查询 DTO
// ========================================

/// <summary>
/// AssyOrderDefect 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktAssyOrderDefectQueryDto : TaktPagedQuery
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
    /// 工单号（统计维度，选项 TaktProductionOrders/options）
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期组（汇总日报去重生产日期，yyyy-MM-dd 逗号分隔升序排列）
    /// </summary>
    public string? ProdDateGroup { get; set; } = string.Empty;

    /// <summary>
    /// 机种（取最近日报）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（取最近日报）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次（一工单一批次，取最近日报）
    /// </summary>
    public string? BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 工单数量（取最近日报）
    /// </summary>
    public decimal? ProdOrderQty { get; set; }

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
    /// 工单状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成；工单数量与累计生实实绩相等时为已完成）
    /// </summary>
    public int? OrderStatus { get; set; }

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
// 创建AssyOrderDefect DTO
// ========================================

/// <summary>
/// 创建AssyOrderDefect DTO
/// </summary>
public class TaktAssyOrderDefectCreateDto
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
    /// 工单号（统计维度，选项 TaktProductionOrders/options）
    /// </summary>
    [Required(ErrorMessage = "工单号（统计维度，选项 TaktProductionOrders/options）不能为空")]
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期组（汇总日报去重生产日期，yyyy-MM-dd 逗号分隔升序排列）
    /// </summary>
    public string? ProdDateGroup { get; set; } = string.Empty;

    /// <summary>
    /// 机种（取最近日报）
    /// </summary>
    [Required(ErrorMessage = "机种（取最近日报）不能为空")]
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（取最近日报）
    /// </summary>
    [Required(ErrorMessage = "物料编码（取最近日报）不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次（一工单一批次，取最近日报）
    /// </summary>
    public string? BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 工单数量（取最近日报）
    /// </summary>
    public decimal ProdOrderQty { get; set; }

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
    /// 工单状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成；工单数量与累计生实实绩相等时为已完成）
    /// </summary>
    public int OrderStatus { get; set; } = 0;

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
// 更新AssyOrderDefect DTO
// ========================================

/// <summary>
/// 更新AssyOrderDefect DTO
/// 继承 TaktAssyOrderDefectCreateDto，添加 AssyOrderDefectId 字段
/// </summary>
public class TaktAssyOrderDefectUpdateDto : TaktAssyOrderDefectCreateDto
{
    /// <summary>
    /// AssyOrderDefectID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyOrderDefectId { get; set; }

}

// ========================================
// AssyOrderDefect 状态 DTO
// ========================================

/// <summary>
/// AssyOrderDefect 状态更新 DTO
/// </summary>
public class TaktAssyOrderDefectStatusDto
{
    /// <summary>
    /// AssyOrderDefectID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyOrderDefectId { get; set; }

    /// <summary>
    /// 工单状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成；工单数量与累计生实实绩相等时为已完成）
    /// </summary>
    [Required(ErrorMessage = "工单状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成；工单数量与累计生实实绩相等时为已完成）不能为空")]
    public int OrderStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// AssyOrderDefect 导入模板行 DTO
/// </summary>
public class TaktAssyOrderDefectTemplateDto
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
    /// 工单号（统计维度，选项 TaktProductionOrders/options）
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期组（汇总日报去重生产日期，yyyy-MM-dd 逗号分隔升序排列）
    /// </summary>
    public string? ProdDateGroup { get; set; } = string.Empty;

    /// <summary>
    /// 机种（取最近日报）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（取最近日报）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次（一工单一批次，取最近日报）
    /// </summary>
    public string? BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 工单数量（取最近日报）
    /// </summary>
    public decimal? ProdOrderQty { get; set; }

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
    /// 工单状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成；工单数量与累计生实实绩相等时为已完成）
    /// </summary>
    public int? OrderStatus { get; set; }

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
/// AssyOrderDefect 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktAssyOrderDefectImportDto
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
    /// 工单号（统计维度，选项 TaktProductionOrders/options）
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期组（汇总日报去重生产日期，yyyy-MM-dd 逗号分隔升序排列）
    /// </summary>
    public string? ProdDateGroup { get; set; } = string.Empty;

    /// <summary>
    /// 机种（取最近日报）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（取最近日报）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次（一工单一批次，取最近日报）
    /// </summary>
    public string? BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 工单数量（取最近日报）
    /// </summary>
    public decimal? ProdOrderQty { get; set; }

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
    /// 工单状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成；工单数量与累计生实实绩相等时为已完成）
    /// </summary>
    public int? OrderStatus { get; set; }

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
/// AssyOrderDefect 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktAssyOrderDefectExportDto
{
    /// <summary>
    /// AssyOrderDefectID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyOrderDefectId { get; set; }

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
    /// 工单号（统计维度，选项 TaktProductionOrders/options）
    /// </summary>
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期组（汇总日报去重生产日期，yyyy-MM-dd 逗号分隔升序排列）
    /// </summary>
    public string? ProdDateGroup { get; set; } = string.Empty;

    /// <summary>
    /// 机种（取最近日报）
    /// </summary>
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（取最近日报）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次（一工单一批次，取最近日报）
    /// </summary>
    public string? BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 工单数量（取最近日报）
    /// </summary>
    public decimal ProdOrderQty { get; set; }

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
    /// 工单状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成；工单数量与累计生实实绩相等时为已完成）
    /// </summary>
    public int OrderStatus { get; set; } = 0;

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
