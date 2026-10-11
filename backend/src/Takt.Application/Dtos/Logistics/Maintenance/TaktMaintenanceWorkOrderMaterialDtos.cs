// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Maintenance
// 文件名称：TaktMaintenanceWorkOrderMaterialDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：MaintenanceWorkOrderMaterial 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktMaintenanceWorkOrderMaterial 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Maintenance;

// ========================================
// MaintenanceWorkOrderMaterial 响应 DTO
// ========================================

/// <summary>
/// 维护工单领料明细实体（主子表：挂载于维护工单）
/// 对应前端 TaktMaintenanceWorkOrderMaterialDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktMaintenanceWorkOrderMaterialDto : TaktCompanyDtoBase
{
    /// <summary>
    /// MaintenanceWorkOrderMaterialID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaintenanceWorkOrderMaterialId { get; set; }

    /// <summary>
    /// 维护工单ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaintenanceWorkOrderId { get; set; }

    /// <summary>
    /// 维护工单名称（填充字段）
    /// </summary>
    public string? MaintenanceWorkOrderName { get; set; }

    /// <summary>
    /// 维护工单号（冗余）
    /// </summary>
    public string WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（步长10：10/20/30…）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 物料ID（关联工厂物料主数据，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialId { get; set; }

    /// <summary>
    /// 物料名称（填充字段）
    /// </summary>
    public string? MaterialName { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 需求数量
    /// </summary>
    public decimal RequiredQuantity { get; set; }

    /// <summary>
    /// 已领数量
    /// </summary>
    public decimal IssuedQuantity { get; set; }

    /// <summary>
    /// 单位
    /// </summary>
    public string MaterialUnit { get; set; } = string.Empty;

    /// <summary>
    /// 单价
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// 金额
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// 仓库编码
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 库位
    /// </summary>
    public string? StorageLocation { get; set; } = string.Empty;

    /// <summary>
    /// 领料状态（0=待领料，1=部分领料，2=已领料）
    /// </summary>
    public int IssueStatus { get; set; } = 0;

    /// <summary>
    /// 领料时间
    /// </summary>
    public DateTime? IssueTime { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 维护工单（主表）
    /// （主表：TaktMaintenanceWorkOrder）
    /// </summary>
    public TaktMaintenanceWorkOrderDto? MaintenanceWorkOrder { get; set; }

    /// <summary>
    /// 物料（工厂物料主数据）
    /// （主表：TaktMaterialPlant）
    /// </summary>
    public TaktMaterialPlantDto? MaterialPlant { get; set; }

}

// ========================================
// MaintenanceWorkOrderMaterial 查询 DTO
// ========================================

/// <summary>
/// MaintenanceWorkOrderMaterial 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktMaintenanceWorkOrderMaterialQueryDto : TaktPagedQuery
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
    /// 维护工单ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaintenanceWorkOrderId { get; set; }

    /// <summary>
    /// 维护工单号（冗余）
    /// </summary>
    public string? WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（步长10：10/20/30…）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 物料ID（关联工厂物料主数据，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaterialId { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 需求数量
    /// </summary>
    public decimal? RequiredQuantity { get; set; }

    /// <summary>
    /// 已领数量
    /// </summary>
    public decimal? IssuedQuantity { get; set; }

    /// <summary>
    /// 单位
    /// </summary>
    public string? MaterialUnit { get; set; } = string.Empty;

    /// <summary>
    /// 单价
    /// </summary>
    public decimal? UnitPrice { get; set; }

    /// <summary>
    /// 金额
    /// </summary>
    public decimal? Amount { get; set; }

    /// <summary>
    /// 仓库编码
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 库位
    /// </summary>
    public string? StorageLocation { get; set; } = string.Empty;

    /// <summary>
    /// 领料状态（0=待领料，1=部分领料，2=已领料）
    /// </summary>
    public int? IssueStatus { get; set; }

    /// <summary>
    /// 领料时间（范围查询-开始）
    /// </summary>
    public DateTime? IssueTimeStart { get; set; }

    /// <summary>
    /// 领料时间（范围查询-结束）
    /// </summary>
    public DateTime? IssueTimeEnd { get; set; }

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
// 创建MaintenanceWorkOrderMaterial DTO
// ========================================

/// <summary>
/// 创建MaintenanceWorkOrderMaterial DTO
/// </summary>
public class TaktMaintenanceWorkOrderMaterialCreateDto
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
    /// 维护工单ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaintenanceWorkOrderId { get; set; }

    /// <summary>
    /// 维护工单号（冗余）
    /// </summary>
    [Required(ErrorMessage = "维护工单号（冗余）不能为空")]
    public string WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（步长10：10/20/30…）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 物料ID（关联工厂物料主数据，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialId { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    [Required(ErrorMessage = "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）不能为空")]
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 需求数量
    /// </summary>
    public decimal RequiredQuantity { get; set; }

    /// <summary>
    /// 已领数量
    /// </summary>
    public decimal IssuedQuantity { get; set; }

    /// <summary>
    /// 单位
    /// </summary>
    [Required(ErrorMessage = "单位不能为空")]
    public string MaterialUnit { get; set; } = string.Empty;

    /// <summary>
    /// 单价
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// 金额
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// 仓库编码
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 库位
    /// </summary>
    public string? StorageLocation { get; set; } = string.Empty;

    /// <summary>
    /// 领料状态（0=待领料，1=部分领料，2=已领料）
    /// </summary>
    public int IssueStatus { get; set; } = 0;

    /// <summary>
    /// 领料时间
    /// </summary>
    public DateTime? IssueTime { get; set; }

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
// 更新MaintenanceWorkOrderMaterial DTO
// ========================================

/// <summary>
/// 更新MaintenanceWorkOrderMaterial DTO
/// 继承 TaktMaintenanceWorkOrderMaterialCreateDto，添加 MaintenanceWorkOrderMaterialId 字段
/// </summary>
public class TaktMaintenanceWorkOrderMaterialUpdateDto : TaktMaintenanceWorkOrderMaterialCreateDto
{
    /// <summary>
    /// MaintenanceWorkOrderMaterialID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaintenanceWorkOrderMaterialId { get; set; }

}

// ========================================
// MaintenanceWorkOrderMaterial 状态 DTO
// ========================================

/// <summary>
/// MaintenanceWorkOrderMaterial 状态更新 DTO
/// </summary>
public class TaktMaintenanceWorkOrderMaterialStatusDto
{
    /// <summary>
    /// MaintenanceWorkOrderMaterialID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaintenanceWorkOrderMaterialId { get; set; }

    /// <summary>
    /// 领料状态（0=待领料，1=部分领料，2=已领料）
    /// </summary>
    [Required(ErrorMessage = "领料状态（0=待领料，1=部分领料，2=已领料）不能为空")]
    public int IssueStatus { get; set; } = 0;
}

// ========================================
// MaintenanceWorkOrderMaterial 作废 DTO
// ========================================

/// <summary>
/// MaintenanceWorkOrderMaterial 作废/撤销作废 DTO
/// </summary>
public class TaktMaintenanceWorkOrderMaterialObsoleteDto
{
    /// <summary>
    /// MaintenanceWorkOrderMaterialID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaintenanceWorkOrderMaterialId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// MaintenanceWorkOrderMaterial 导入模板行 DTO
/// </summary>
public class TaktMaintenanceWorkOrderMaterialTemplateDto
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
    /// 维护工单ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaintenanceWorkOrderId { get; set; }

    /// <summary>
    /// 维护工单号（冗余）
    /// </summary>
    public string? WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（步长10：10/20/30…）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 物料ID（关联工厂物料主数据，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaterialId { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 需求数量
    /// </summary>
    public decimal? RequiredQuantity { get; set; }

    /// <summary>
    /// 已领数量
    /// </summary>
    public decimal? IssuedQuantity { get; set; }

    /// <summary>
    /// 单位
    /// </summary>
    public string? MaterialUnit { get; set; } = string.Empty;

    /// <summary>
    /// 单价
    /// </summary>
    public decimal? UnitPrice { get; set; }

    /// <summary>
    /// 金额
    /// </summary>
    public decimal? Amount { get; set; }

    /// <summary>
    /// 仓库编码
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 库位
    /// </summary>
    public string? StorageLocation { get; set; } = string.Empty;

    /// <summary>
    /// 领料状态（0=待领料，1=部分领料，2=已领料）
    /// </summary>
    public int? IssueStatus { get; set; }

    /// <summary>
    /// 领料时间
    /// </summary>
    public DateTime? IssueTime { get; set; }

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
/// MaintenanceWorkOrderMaterial 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktMaintenanceWorkOrderMaterialImportDto
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
    /// 维护工单ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaintenanceWorkOrderId { get; set; }

    /// <summary>
    /// 维护工单号（冗余）
    /// </summary>
    public string? WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（步长10：10/20/30…）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 物料ID（关联工厂物料主数据，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaterialId { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 需求数量
    /// </summary>
    public decimal? RequiredQuantity { get; set; }

    /// <summary>
    /// 已领数量
    /// </summary>
    public decimal? IssuedQuantity { get; set; }

    /// <summary>
    /// 单位
    /// </summary>
    public string? MaterialUnit { get; set; } = string.Empty;

    /// <summary>
    /// 单价
    /// </summary>
    public decimal? UnitPrice { get; set; }

    /// <summary>
    /// 金额
    /// </summary>
    public decimal? Amount { get; set; }

    /// <summary>
    /// 仓库编码
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 库位
    /// </summary>
    public string? StorageLocation { get; set; } = string.Empty;

    /// <summary>
    /// 领料状态（0=待领料，1=部分领料，2=已领料）
    /// </summary>
    public int? IssueStatus { get; set; }

    /// <summary>
    /// 领料时间
    /// </summary>
    public DateTime? IssueTime { get; set; }

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
/// MaintenanceWorkOrderMaterial 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktMaintenanceWorkOrderMaterialExportDto
{
    /// <summary>
    /// MaintenanceWorkOrderMaterialID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaintenanceWorkOrderMaterialId { get; set; }

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
    /// 维护工单ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaintenanceWorkOrderId { get; set; }

    /// <summary>
    /// 维护工单号（冗余）
    /// </summary>
    public string WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（步长10：10/20/30…）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 物料ID（关联工厂物料主数据，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialId { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 需求数量
    /// </summary>
    public decimal RequiredQuantity { get; set; }

    /// <summary>
    /// 已领数量
    /// </summary>
    public decimal IssuedQuantity { get; set; }

    /// <summary>
    /// 单位
    /// </summary>
    public string MaterialUnit { get; set; } = string.Empty;

    /// <summary>
    /// 单价
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// 金额
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// 仓库编码
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 库位
    /// </summary>
    public string? StorageLocation { get; set; } = string.Empty;

    /// <summary>
    /// 领料状态（0=待领料，1=部分领料，2=已领料）
    /// </summary>
    public int IssueStatus { get; set; } = 0;

    /// <summary>
    /// 领料时间
    /// </summary>
    public DateTime? IssueTime { get; set; }

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
