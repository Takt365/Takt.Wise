// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Output
// 文件名称：TaktPcbaOutputDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：PcbaOutput 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPcbaOutput 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Output;

// ========================================
// PcbaOutput 响应 DTO
// ========================================

/// <summary>
/// PCBA日报实体 达成率(%) = 明细当日完成数量合计 ÷ 明细人员标准产能合计 × 100%。
/// 对应前端 TaktPcbaOutputDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktPcbaOutputDto : TaktCompanyDtoBase
{
    /// <summary>
    /// PcbaOutputID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaOutputId { get; set; }

    /// <summary>
    /// 生产类别（字典 logistics_manufacturing_prod_category；存 DictValue：EPP/FPP/RWP/MDP/CPP）
    /// </summary>
    public string ProdCategory { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期
    /// </summary>
    public DateTime ProdDate { get; set; }

    /// <summary>
    /// 工单类别（回填：随工单）
    /// </summary>
    public string? ProdOrderType { get; set; } = string.Empty;

    /// <summary>
    /// 工单号（选项 TaktProductionOrders/options；DictValue=ProdOrderCode，ExtValue=PlantCode）
    /// </summary>
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种（回填：随工单）
    /// </summary>
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（回填：随工单）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次（回填：随工单）
    /// </summary>
    public string? BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 工单数量（回填：随工单）
    /// </summary>
    public decimal ProdOrderQty { get; set; }

    /// <summary>
    /// 序列号（回填：随工单）
    /// </summary>
    public string? SerialCode { get; set; } = string.Empty;

    /// <summary>
    /// PCBA明细列表
    /// （子表：TaktPcbaOutputDetail）
    /// </summary>
    public List<TaktPcbaOutputDetailDto>? PcbaOutputDetails { get; set; }

}

// ========================================
// PcbaOutput 查询 DTO
// ========================================

/// <summary>
/// PcbaOutput 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPcbaOutputQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产类别（字典 logistics_manufacturing_prod_category；存 DictValue：EPP/FPP/RWP/MDP/CPP）
    /// </summary>
    public string? ProdCategory { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期（范围查询-开始）
    /// </summary>
    public DateTime? ProdDateStart { get; set; }

    /// <summary>
    /// 生产日期（范围查询-结束）
    /// </summary>
    public DateTime? ProdDateEnd { get; set; }

    /// <summary>
    /// 工单类别（回填：随工单）
    /// </summary>
    public string? ProdOrderType { get; set; } = string.Empty;

    /// <summary>
    /// 工单号（选项 TaktProductionOrders/options；DictValue=ProdOrderCode，ExtValue=PlantCode）
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种（回填：随工单）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（回填：随工单）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次（回填：随工单）
    /// </summary>
    public string? BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 工单数量（回填：随工单）
    /// </summary>
    public decimal? ProdOrderQty { get; set; }

    /// <summary>
    /// 序列号（回填：随工单）
    /// </summary>
    public string? SerialCode { get; set; } = string.Empty;

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
// 创建PcbaOutput DTO
// ========================================

/// <summary>
/// 创建PcbaOutput DTO
/// </summary>
public class TaktPcbaOutputCreateDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产类别（字典 logistics_manufacturing_prod_category；存 DictValue：EPP/FPP/RWP/MDP/CPP）
    /// </summary>
    [Required(ErrorMessage = "生产类别（字典 logistics_manufacturing_prod_category；存 DictValue：EPP/FPP/RWP/MDP/CPP）不能为空")]
    public string ProdCategory { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期
    /// </summary>
    public DateTime ProdDate { get; set; }

    /// <summary>
    /// 工单类别（回填：随工单）
    /// </summary>
    public string? ProdOrderType { get; set; } = string.Empty;

    /// <summary>
    /// 工单号（选项 TaktProductionOrders/options；DictValue=ProdOrderCode，ExtValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "工单号（选项 TaktProductionOrders/options；DictValue=ProdOrderCode，ExtValue=PlantCode）不能为空")]
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种（回填：随工单）
    /// </summary>
    [Required(ErrorMessage = "机种（回填：随工单）不能为空")]
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（回填：随工单）
    /// </summary>
    [Required(ErrorMessage = "物料编码（回填：随工单）不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次（回填：随工单）
    /// </summary>
    public string? BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 工单数量（回填：随工单）
    /// </summary>
    public decimal ProdOrderQty { get; set; }

    /// <summary>
    /// 序列号（回填：随工单）
    /// </summary>
    public string? SerialCode { get; set; } = string.Empty;

    /// <summary>
    /// PCBA明细列表（子表，级联保存）
    /// </summary>
    public List<TaktPcbaOutputDetailCreateDto>? PcbaOutputDetails { get; set; }

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
// 更新PcbaOutput DTO
// ========================================

/// <summary>
/// 更新PcbaOutput DTO
/// 继承 TaktPcbaOutputCreateDto，添加 PcbaOutputId 字段
/// </summary>
public class TaktPcbaOutputUpdateDto : TaktPcbaOutputCreateDto
{
    /// <summary>
    /// PcbaOutputID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaOutputId { get; set; }

    /// <summary>
    /// PCBA明细列表（子表，级联保存）
    /// </summary>
    public new List<TaktPcbaOutputDetailUpdateDto>? PcbaOutputDetails { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// PcbaOutput 导入模板行 DTO
/// </summary>
public class TaktPcbaOutputTemplateDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产类别（字典 logistics_manufacturing_prod_category；存 DictValue：EPP/FPP/RWP/MDP/CPP）
    /// </summary>
    public string? ProdCategory { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期
    /// </summary>
    public DateTime? ProdDate { get; set; }

    /// <summary>
    /// 工单类别（回填：随工单）
    /// </summary>
    public string? ProdOrderType { get; set; } = string.Empty;

    /// <summary>
    /// 工单号（选项 TaktProductionOrders/options；DictValue=ProdOrderCode，ExtValue=PlantCode）
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种（回填：随工单）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（回填：随工单）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次（回填：随工单）
    /// </summary>
    public string? BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 工单数量（回填：随工单）
    /// </summary>
    public decimal? ProdOrderQty { get; set; }

    /// <summary>
    /// 序列号（回填：随工单）
    /// </summary>
    public string? SerialCode { get; set; } = string.Empty;

    /// <summary>
    /// PCBA明细列表（子表，级联保存）
    /// </summary>
    public List<TaktPcbaOutputDetailCreateDto>? PcbaOutputDetails { get; set; }

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
/// PcbaOutput 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPcbaOutputImportDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产类别（字典 logistics_manufacturing_prod_category；存 DictValue：EPP/FPP/RWP/MDP/CPP）
    /// </summary>
    public string? ProdCategory { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期
    /// </summary>
    public DateTime? ProdDate { get; set; }

    /// <summary>
    /// 工单类别（回填：随工单）
    /// </summary>
    public string? ProdOrderType { get; set; } = string.Empty;

    /// <summary>
    /// 工单号（选项 TaktProductionOrders/options；DictValue=ProdOrderCode，ExtValue=PlantCode）
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种（回填：随工单）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（回填：随工单）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次（回填：随工单）
    /// </summary>
    public string? BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 工单数量（回填：随工单）
    /// </summary>
    public decimal? ProdOrderQty { get; set; }

    /// <summary>
    /// 序列号（回填：随工单）
    /// </summary>
    public string? SerialCode { get; set; } = string.Empty;

    /// <summary>
    /// PCBA明细列表（子表，级联保存）
    /// </summary>
    public List<TaktPcbaOutputDetailCreateDto>? PcbaOutputDetails { get; set; }

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
/// PcbaOutput 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPcbaOutputExportDto
{
    /// <summary>
    /// PcbaOutputID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaOutputId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产类别（字典 logistics_manufacturing_prod_category；存 DictValue：EPP/FPP/RWP/MDP/CPP）
    /// </summary>
    public string ProdCategory { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期
    /// </summary>
    public DateTime ProdDate { get; set; }

    /// <summary>
    /// 工单类别（回填：随工单）
    /// </summary>
    public string? ProdOrderType { get; set; } = string.Empty;

    /// <summary>
    /// 工单号（选项 TaktProductionOrders/options；DictValue=ProdOrderCode，ExtValue=PlantCode）
    /// </summary>
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种（回填：随工单）
    /// </summary>
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（回填：随工单）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次（回填：随工单）
    /// </summary>
    public string? BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 工单数量（回填：随工单）
    /// </summary>
    public decimal ProdOrderQty { get; set; }

    /// <summary>
    /// 序列号（回填：随工单）
    /// </summary>
    public string? SerialCode { get; set; } = string.Empty;

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
