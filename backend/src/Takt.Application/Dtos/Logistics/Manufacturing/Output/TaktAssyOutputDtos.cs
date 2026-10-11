// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Output
// 文件名称：TaktAssyOutputDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：AssyOutput 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktAssyOutput 生成，请按需审阅）
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
// AssyOutput 响应 DTO
// ========================================

/// <summary>
/// 组立日报（产出）主表实体 <para>业务唯一键：TenantCode+CompanyCode+TeamCode+ProdCategory+ProdDate+ProdOrderCode。</para> 达成率(%) = 明细实际生产数量合计 ÷ 主表标准产能合计 × 100%。
/// 对应前端 TaktAssyOutputDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktAssyOutputDto : TaktCompanyDtoBase
{
    /// <summary>
    /// AssyOutputID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyOutputId { get; set; }

    /// <summary>
    /// 生产类别（字典 logistics_manufacturing_prod_category；存 DictValue：EPP/FPP/RWP/MDP/CPP）
    /// </summary>
    public string ProdCategory { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期
    /// </summary>
    public DateTime ProdDate { get; set; }

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    public string TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 直接人员
    /// </summary>
    public int DirectLabor { get; set; } = 0;

    /// <summary>
    /// 间接人员
    /// </summary>
    public int IndirectLabor { get; set; } = 0;

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int ShiftNo { get; set; } = 0;

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
    /// 标准工时(分钟)（回填：按 MaterialCode 查询 TaktStandardOperationTime 汇总转换工时）
    /// </summary>
    public decimal StdMinutes { get; set; }

    /// <summary>
    /// 标准产能（计算结果：利用标准生产稼动率计算出小时产能，DirectLabor人数*60分钟/StdMinutes标准工时*标准生产稼动率）
    /// </summary>
    public decimal StdCapacity { get; set; }

    /// <summary>
    /// 组立日报明细列表
    /// （子表：TaktAssyOutputDetail）
    /// </summary>
    public List<TaktAssyOutputDetailDto>? AssyOutputDetails { get; set; }

}

// ========================================
// AssyOutput 查询 DTO
// ========================================

/// <summary>
/// AssyOutput 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktAssyOutputQueryDto : TaktPagedQuery
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
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 直接人员
    /// </summary>
    public int? DirectLabor { get; set; }

    /// <summary>
    /// 间接人员
    /// </summary>
    public int? IndirectLabor { get; set; }

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int? ShiftNo { get; set; }

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
    /// 标准工时(分钟)（回填：按 MaterialCode 查询 TaktStandardOperationTime 汇总转换工时）
    /// </summary>
    public decimal? StdMinutes { get; set; }

    /// <summary>
    /// 标准产能（计算结果：利用标准生产稼动率计算出小时产能，DirectLabor人数*60分钟/StdMinutes标准工时*标准生产稼动率）
    /// </summary>
    public decimal? StdCapacity { get; set; }

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
// 创建AssyOutput DTO
// ========================================

/// <summary>
/// 创建AssyOutput DTO
/// </summary>
public class TaktAssyOutputCreateDto
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
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）不能为空")]
    public string TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 直接人员
    /// </summary>
    public int DirectLabor { get; set; } = 0;

    /// <summary>
    /// 间接人员
    /// </summary>
    public int IndirectLabor { get; set; } = 0;

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int ShiftNo { get; set; } = 0;

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
    /// 标准工时(分钟)（回填：按 MaterialCode 查询 TaktStandardOperationTime 汇总转换工时）
    /// </summary>
    public decimal StdMinutes { get; set; }

    /// <summary>
    /// 标准产能（计算结果：利用标准生产稼动率计算出小时产能，DirectLabor人数*60分钟/StdMinutes标准工时*标准生产稼动率）
    /// </summary>
    public decimal StdCapacity { get; set; }

    /// <summary>
    /// 组立日报明细列表（子表，级联保存）
    /// </summary>
    public List<TaktAssyOutputDetailCreateDto>? AssyOutputDetails { get; set; }

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
// 更新AssyOutput DTO
// ========================================

/// <summary>
/// 更新AssyOutput DTO
/// 继承 TaktAssyOutputCreateDto，添加 AssyOutputId 字段
/// </summary>
public class TaktAssyOutputUpdateDto : TaktAssyOutputCreateDto
{
    /// <summary>
    /// AssyOutputID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyOutputId { get; set; }

    /// <summary>
    /// 组立日报明细列表（子表，级联保存）
    /// </summary>
    public new List<TaktAssyOutputDetailUpdateDto>? AssyOutputDetails { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// AssyOutput 导入模板行 DTO
/// </summary>
public class TaktAssyOutputTemplateDto
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
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 直接人员
    /// </summary>
    public int? DirectLabor { get; set; }

    /// <summary>
    /// 间接人员
    /// </summary>
    public int? IndirectLabor { get; set; }

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int? ShiftNo { get; set; }

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
    /// 标准工时(分钟)（回填：按 MaterialCode 查询 TaktStandardOperationTime 汇总转换工时）
    /// </summary>
    public decimal? StdMinutes { get; set; }

    /// <summary>
    /// 标准产能（计算结果：利用标准生产稼动率计算出小时产能，DirectLabor人数*60分钟/StdMinutes标准工时*标准生产稼动率）
    /// </summary>
    public decimal? StdCapacity { get; set; }

    /// <summary>
    /// 组立日报明细列表（子表，级联保存）
    /// </summary>
    public List<TaktAssyOutputDetailCreateDto>? AssyOutputDetails { get; set; }

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
/// AssyOutput 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktAssyOutputImportDto
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
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 直接人员
    /// </summary>
    public int? DirectLabor { get; set; }

    /// <summary>
    /// 间接人员
    /// </summary>
    public int? IndirectLabor { get; set; }

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int? ShiftNo { get; set; }

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
    /// 标准工时(分钟)（回填：按 MaterialCode 查询 TaktStandardOperationTime 汇总转换工时）
    /// </summary>
    public decimal? StdMinutes { get; set; }

    /// <summary>
    /// 标准产能（计算结果：利用标准生产稼动率计算出小时产能，DirectLabor人数*60分钟/StdMinutes标准工时*标准生产稼动率）
    /// </summary>
    public decimal? StdCapacity { get; set; }

    /// <summary>
    /// 组立日报明细列表（子表，级联保存）
    /// </summary>
    public List<TaktAssyOutputDetailCreateDto>? AssyOutputDetails { get; set; }

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
/// AssyOutput 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktAssyOutputExportDto
{
    /// <summary>
    /// AssyOutputID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyOutputId { get; set; }

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
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    public string TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 直接人员
    /// </summary>
    public int DirectLabor { get; set; } = 0;

    /// <summary>
    /// 间接人员
    /// </summary>
    public int IndirectLabor { get; set; } = 0;

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int ShiftNo { get; set; } = 0;

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
    /// 标准工时(分钟)（回填：按 MaterialCode 查询 TaktStandardOperationTime 汇总转换工时）
    /// </summary>
    public decimal StdMinutes { get; set; }

    /// <summary>
    /// 标准产能（计算结果：利用标准生产稼动率计算出小时产能，DirectLabor人数*60分钟/StdMinutes标准工时*标准生产稼动率）
    /// </summary>
    public decimal StdCapacity { get; set; }

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
