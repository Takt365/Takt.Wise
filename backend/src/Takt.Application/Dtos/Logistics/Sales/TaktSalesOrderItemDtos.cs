// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Sales
// 文件名称：TaktSalesOrderItemDtos.cs
// 创建时间：2026-09-04
// 创建人：Takt365(Auto Generated)
// 功能描述：SalesOrderItem 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSalesOrderItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Sales;

// ========================================
// SalesOrderItem 响应 DTO
// ========================================

/// <summary>
/// Takt销售订单明细实体
/// 对应前端 TaktSalesOrderItemDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSalesOrderItemDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SalesOrderItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesOrderItemId { get; set; }

    /// <summary>
    /// 销售订单（选项 TaktSalesOrders/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesOrderId { get; set; }

    /// <summary>
    /// 销售订单（选项 TaktSalesOrders/options；DictValue=Id）
    /// </summary>
    public string? SalesOrderName { get; set; }

    /// <summary>
    /// 销售订单编码（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string SalesOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格（回填：随物料）
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 销售单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string SalesUnit { get; set; } = string.Empty;

    /// <summary>
    /// 订购数量（基本单位数量）
    /// </summary>
    public decimal OrderQuantity { get; set; }

    /// <summary>
    /// 目标数量（基本单位数量）
    /// </summary>
    public decimal TargetQuantity { get; set; }

    /// <summary>
    /// 已发货数量（基本单位数量）
    /// </summary>
    public decimal ShippedQuantity { get; set; }

    /// <summary>
    /// 价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）
    /// </summary>
    public int SalesPerUnit { get; set; } = 0;

    /// <summary>
    /// 销售单价
    /// </summary>
    public decimal SalesUnitPrice { get; set; }

    /// <summary>
    /// 折扣率（0-100，表示折扣百分比）
    /// </summary>
    public decimal DiscountRate { get; set; }

    /// <summary>
    /// 折扣金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// 含税金额
    /// </summary>
    public decimal TaxIncludedAmount { get; set; }

    /// <summary>
    /// 未税金额
    /// </summary>
    public decimal UntaxedAmount { get; set; }

    /// <summary>
    /// 税费
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 销售金额
    /// </summary>
    public decimal SalesAmount { get; set; }

    /// <summary>
    /// 毛重
    /// </summary>
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// 净重
    /// </summary>
    public decimal? NetWeight { get; set; }

    /// <summary>
    /// 重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）
    /// </summary>
    public string? WeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 体积
    /// </summary>
    public decimal? Volume { get; set; }

    /// <summary>
    /// 体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）
    /// </summary>
    public string? VolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
    /// </summary>
    public string? ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 行交货状态（字典 logistics_sales_delivery_status；0=未交货 1=部分交货 2=全部交货）
    /// </summary>
    public int DeliveryStatus { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 销售订单主表
    /// （主表：TaktSalesOrder）
    /// </summary>
    public TaktSalesOrderDto? SalesOrder { get; set; }

}

// ========================================
// SalesOrderItem 查询 DTO
// ========================================

/// <summary>
/// SalesOrderItem 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSalesOrderItemQueryDto : TaktPagedQuery
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
    /// 销售订单（选项 TaktSalesOrders/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesOrderId { get; set; }

    /// <summary>
    /// 销售订单编码（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string? SalesOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格（回填：随物料）
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 销售单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string? SalesUnit { get; set; } = string.Empty;

    /// <summary>
    /// 订购数量（基本单位数量）
    /// </summary>
    public decimal? OrderQuantity { get; set; }

    /// <summary>
    /// 目标数量（基本单位数量）
    /// </summary>
    public decimal? TargetQuantity { get; set; }

    /// <summary>
    /// 已发货数量（基本单位数量）
    /// </summary>
    public decimal? ShippedQuantity { get; set; }

    /// <summary>
    /// 价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）
    /// </summary>
    public int? SalesPerUnit { get; set; }

    /// <summary>
    /// 销售单价
    /// </summary>
    public decimal? SalesUnitPrice { get; set; }

    /// <summary>
    /// 折扣率（0-100，表示折扣百分比）
    /// </summary>
    public decimal? DiscountRate { get; set; }

    /// <summary>
    /// 折扣金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? DiscountAmount { get; set; }

    /// <summary>
    /// 含税金额
    /// </summary>
    public decimal? TaxIncludedAmount { get; set; }

    /// <summary>
    /// 未税金额
    /// </summary>
    public decimal? UntaxedAmount { get; set; }

    /// <summary>
    /// 税费
    /// </summary>
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// 销售金额
    /// </summary>
    public decimal? SalesAmount { get; set; }

    /// <summary>
    /// 毛重
    /// </summary>
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// 净重
    /// </summary>
    public decimal? NetWeight { get; set; }

    /// <summary>
    /// 重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）
    /// </summary>
    public string? WeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 体积
    /// </summary>
    public decimal? Volume { get; set; }

    /// <summary>
    /// 体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）
    /// </summary>
    public string? VolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
    /// </summary>
    public string? ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 行交货状态（字典 logistics_sales_delivery_status；0=未交货 1=部分交货 2=全部交货）
    /// </summary>
    public int? DeliveryStatus { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
// 创建SalesOrderItem DTO
// ========================================

/// <summary>
/// 创建SalesOrderItem DTO
/// </summary>
public class TaktSalesOrderItemCreateDto
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
    /// 销售订单（选项 TaktSalesOrders/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesOrderId { get; set; }

    /// <summary>
    /// 销售订单编码（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string SalesOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    [Required(ErrorMessage = "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）不能为空")]
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格（回填：随物料）
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 销售单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    [Required(ErrorMessage = "销售单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）不能为空")]
    public string SalesUnit { get; set; } = string.Empty;

    /// <summary>
    /// 订购数量（基本单位数量）
    /// </summary>
    public decimal OrderQuantity { get; set; }

    /// <summary>
    /// 目标数量（基本单位数量）
    /// </summary>
    public decimal TargetQuantity { get; set; }

    /// <summary>
    /// 已发货数量（基本单位数量）
    /// </summary>
    public decimal ShippedQuantity { get; set; }

    /// <summary>
    /// 价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）
    /// </summary>
    public int SalesPerUnit { get; set; } = 0;

    /// <summary>
    /// 销售单价
    /// </summary>
    public decimal SalesUnitPrice { get; set; }

    /// <summary>
    /// 折扣率（0-100，表示折扣百分比）
    /// </summary>
    public decimal DiscountRate { get; set; }

    /// <summary>
    /// 折扣金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// 含税金额
    /// </summary>
    public decimal TaxIncludedAmount { get; set; }

    /// <summary>
    /// 未税金额
    /// </summary>
    public decimal UntaxedAmount { get; set; }

    /// <summary>
    /// 税费
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 销售金额
    /// </summary>
    public decimal SalesAmount { get; set; }

    /// <summary>
    /// 毛重
    /// </summary>
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// 净重
    /// </summary>
    public decimal? NetWeight { get; set; }

    /// <summary>
    /// 重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）
    /// </summary>
    public string? WeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 体积
    /// </summary>
    public decimal? Volume { get; set; }

    /// <summary>
    /// 体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）
    /// </summary>
    public string? VolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
    /// </summary>
    public string? ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 行交货状态（字典 logistics_sales_delivery_status；0=未交货 1=部分交货 2=全部交货）
    /// </summary>
    public int DeliveryStatus { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

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
// 更新SalesOrderItem DTO
// ========================================

/// <summary>
/// 更新SalesOrderItem DTO
/// 继承 TaktSalesOrderItemCreateDto，添加 SalesOrderItemId 字段
/// </summary>
public class TaktSalesOrderItemUpdateDto : TaktSalesOrderItemCreateDto
{
    /// <summary>
    /// SalesOrderItemID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesOrderItemId { get; set; }

}

// ========================================
// SalesOrderItem 状态 DTO
// ========================================

/// <summary>
/// SalesOrderItem 状态更新 DTO
/// </summary>
public class TaktSalesOrderItemStatusDto
{
    /// <summary>
    /// SalesOrderItemID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesOrderItemId { get; set; }

    /// <summary>
    /// 行交货状态（字典 logistics_sales_delivery_status；0=未交货 1=部分交货 2=全部交货）
    /// </summary>
    [Required(ErrorMessage = "行交货状态（字典 logistics_sales_delivery_status；0=未交货 1=部分交货 2=全部交货）不能为空")]
    public int DeliveryStatus { get; set; } = 0;
}

// ========================================
// SalesOrderItem 作废 DTO
// ========================================

/// <summary>
/// SalesOrderItem 作废/撤销作废 DTO
/// </summary>
public class TaktSalesOrderItemObsoleteDto
{
    /// <summary>
    /// SalesOrderItemID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesOrderItemId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SalesOrderItem 导入模板行 DTO
/// </summary>
public class TaktSalesOrderItemTemplateDto
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
    /// 销售订单（选项 TaktSalesOrders/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesOrderId { get; set; }

    /// <summary>
    /// 销售订单编码（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string? SalesOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格（回填：随物料）
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 销售单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string? SalesUnit { get; set; } = string.Empty;

    /// <summary>
    /// 订购数量（基本单位数量）
    /// </summary>
    public decimal? OrderQuantity { get; set; }

    /// <summary>
    /// 目标数量（基本单位数量）
    /// </summary>
    public decimal? TargetQuantity { get; set; }

    /// <summary>
    /// 已发货数量（基本单位数量）
    /// </summary>
    public decimal? ShippedQuantity { get; set; }

    /// <summary>
    /// 价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）
    /// </summary>
    public int? SalesPerUnit { get; set; }

    /// <summary>
    /// 销售单价
    /// </summary>
    public decimal? SalesUnitPrice { get; set; }

    /// <summary>
    /// 折扣率（0-100，表示折扣百分比）
    /// </summary>
    public decimal? DiscountRate { get; set; }

    /// <summary>
    /// 折扣金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? DiscountAmount { get; set; }

    /// <summary>
    /// 含税金额
    /// </summary>
    public decimal? TaxIncludedAmount { get; set; }

    /// <summary>
    /// 未税金额
    /// </summary>
    public decimal? UntaxedAmount { get; set; }

    /// <summary>
    /// 税费
    /// </summary>
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// 销售金额
    /// </summary>
    public decimal? SalesAmount { get; set; }

    /// <summary>
    /// 毛重
    /// </summary>
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// 净重
    /// </summary>
    public decimal? NetWeight { get; set; }

    /// <summary>
    /// 重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）
    /// </summary>
    public string? WeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 体积
    /// </summary>
    public decimal? Volume { get; set; }

    /// <summary>
    /// 体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）
    /// </summary>
    public string? VolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
    /// </summary>
    public string? ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 行交货状态（字典 logistics_sales_delivery_status；0=未交货 1=部分交货 2=全部交货）
    /// </summary>
    public int? DeliveryStatus { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
/// SalesOrderItem 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSalesOrderItemImportDto
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
    /// 销售订单（选项 TaktSalesOrders/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesOrderId { get; set; }

    /// <summary>
    /// 销售订单编码（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string? SalesOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格（回填：随物料）
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 销售单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string? SalesUnit { get; set; } = string.Empty;

    /// <summary>
    /// 订购数量（基本单位数量）
    /// </summary>
    public decimal? OrderQuantity { get; set; }

    /// <summary>
    /// 目标数量（基本单位数量）
    /// </summary>
    public decimal? TargetQuantity { get; set; }

    /// <summary>
    /// 已发货数量（基本单位数量）
    /// </summary>
    public decimal? ShippedQuantity { get; set; }

    /// <summary>
    /// 价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）
    /// </summary>
    public int? SalesPerUnit { get; set; }

    /// <summary>
    /// 销售单价
    /// </summary>
    public decimal? SalesUnitPrice { get; set; }

    /// <summary>
    /// 折扣率（0-100，表示折扣百分比）
    /// </summary>
    public decimal? DiscountRate { get; set; }

    /// <summary>
    /// 折扣金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? DiscountAmount { get; set; }

    /// <summary>
    /// 含税金额
    /// </summary>
    public decimal? TaxIncludedAmount { get; set; }

    /// <summary>
    /// 未税金额
    /// </summary>
    public decimal? UntaxedAmount { get; set; }

    /// <summary>
    /// 税费
    /// </summary>
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// 销售金额
    /// </summary>
    public decimal? SalesAmount { get; set; }

    /// <summary>
    /// 毛重
    /// </summary>
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// 净重
    /// </summary>
    public decimal? NetWeight { get; set; }

    /// <summary>
    /// 重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）
    /// </summary>
    public string? WeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 体积
    /// </summary>
    public decimal? Volume { get; set; }

    /// <summary>
    /// 体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）
    /// </summary>
    public string? VolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
    /// </summary>
    public string? ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 行交货状态（字典 logistics_sales_delivery_status；0=未交货 1=部分交货 2=全部交货）
    /// </summary>
    public int? DeliveryStatus { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
/// SalesOrderItem 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSalesOrderItemExportDto
{
    /// <summary>
    /// SalesOrderItemID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesOrderItemId { get; set; }

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
    /// 销售订单（选项 TaktSalesOrders/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesOrderId { get; set; }

    /// <summary>
    /// 销售订单编码（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string SalesOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格（回填：随物料）
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 销售单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string SalesUnit { get; set; } = string.Empty;

    /// <summary>
    /// 订购数量（基本单位数量）
    /// </summary>
    public decimal OrderQuantity { get; set; }

    /// <summary>
    /// 目标数量（基本单位数量）
    /// </summary>
    public decimal TargetQuantity { get; set; }

    /// <summary>
    /// 已发货数量（基本单位数量）
    /// </summary>
    public decimal ShippedQuantity { get; set; }

    /// <summary>
    /// 价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）
    /// </summary>
    public int SalesPerUnit { get; set; } = 0;

    /// <summary>
    /// 销售单价
    /// </summary>
    public decimal SalesUnitPrice { get; set; }

    /// <summary>
    /// 折扣率（0-100，表示折扣百分比）
    /// </summary>
    public decimal DiscountRate { get; set; }

    /// <summary>
    /// 折扣金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// 含税金额
    /// </summary>
    public decimal TaxIncludedAmount { get; set; }

    /// <summary>
    /// 未税金额
    /// </summary>
    public decimal UntaxedAmount { get; set; }

    /// <summary>
    /// 税费
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 销售金额
    /// </summary>
    public decimal SalesAmount { get; set; }

    /// <summary>
    /// 毛重
    /// </summary>
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// 净重
    /// </summary>
    public decimal? NetWeight { get; set; }

    /// <summary>
    /// 重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）
    /// </summary>
    public string? WeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 体积
    /// </summary>
    public decimal? Volume { get; set; }

    /// <summary>
    /// 体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）
    /// </summary>
    public string? VolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
    /// </summary>
    public string? ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 行交货状态（字典 logistics_sales_delivery_status；0=未交货 1=部分交货 2=全部交货）
    /// </summary>
    public int DeliveryStatus { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

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
