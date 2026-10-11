// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Maintenance
// 文件名称：TaktEquipmentDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：Equipment 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktEquipment 生成，请按需审阅）
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
// Equipment 响应 DTO
// ========================================

/// <summary>
/// Takt工厂设备实体
/// 对应前端 TaktEquipmentDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktEquipmentDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EquipmentID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EquipmentId { get; set; }

    /// <summary>
    /// 设备编码（唯一索引：租户+公司+工厂内唯一，见 ix_equipment_code_unique）
    /// </summary>
    public string EquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备名称
    /// </summary>
    public string EquipmentName { get; set; } = string.Empty;

    /// <summary>
    /// 登录设备（0=生产设备，1=检测设备，2=辅助设备，3=办公设备，4=其他设备）
    /// </summary>
    public int EquipmentType { get; set; } = 0;

    /// <summary>
    /// 设备型号
    /// </summary>
    public string? EquipmentModel { get; set; } = string.Empty;

    /// <summary>
    /// 设备规格
    /// </summary>
    public string? EquipSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 设备品牌
    /// </summary>
    public string? EquipBrand { get; set; } = string.Empty;

    /// <summary>
    /// 制造商
    /// </summary>
    public string? Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// 经销商
    /// </summary>
    public string? DealerBy { get; set; } = string.Empty;

    /// <summary>
    /// 序列号/出厂编码
    /// </summary>
    public string? SerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 所属车间
    /// </summary>
    public string? WorkshopBy { get; set; } = string.Empty;

    /// <summary>
    /// 所属产线
    /// </summary>
    public string? ProductionLineBy { get; set; } = string.Empty;

    /// <summary>
    /// 所属工位
    /// </summary>
    public string? WorkstationBy { get; set; } = string.Empty;

    /// <summary>
    /// 所属部门
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 设备位置（详细位置描述）
    /// </summary>
    public string? EquipmentLocation { get; set; } = string.Empty;

    /// <summary>
    /// 负责人
    /// </summary>
    public string? ResponsibleUserName { get; set; } = string.Empty;

    /// <summary>
    /// 操作人
    /// </summary>
    public string? OperatorEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 购买日期
    /// </summary>
    public DateTime? PurchaseDate { get; set; }

    /// <summary>
    /// 安装日期
    /// </summary>
    public DateTime? InstallationDate { get; set; }

    /// <summary>
    /// 启用日期
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// 保修开始日期
    /// </summary>
    public DateTime? WarrantyStartDate { get; set; }

    /// <summary>
    /// 保修结束日期
    /// </summary>
    public DateTime? WarrantyEndDate { get; set; }

    /// <summary>
    /// 设备原值（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal EquipmentOriginalValue { get; set; }

    /// <summary>
    /// 设备技术参数（JSON格式，存储设备技术参数配置）
    /// </summary>
    public string? TechnicalParameters { get; set; } = string.Empty;

    /// <summary>
    /// 设备图片（JSON格式，存储设备图片URL列表）
    /// </summary>
    public string? EquipmentImages { get; set; } = string.Empty;

    /// <summary>
    /// 设备文档（JSON格式，存储设备文档ID列表）
    /// </summary>
    public string? EquipmentDocuments { get; set; } = string.Empty;

    /// <summary>
    /// 是否关键设备（0=否，1=是）
    /// </summary>
    public int IsCritical { get; set; } = 0;

    /// <summary>
    /// 保修状态（0=无保修，1=保修期内，2=保修期外，3=延保中）
    /// </summary>
    public int WarrantyStatus { get; set; } = 0;

    /// <summary>
    /// 设备状态（字典 sys_equipment_status）
    /// </summary>
    public int EquipmentStatus { get; set; } = 0;

    /// <summary>
    /// 维护通知单列表
    /// （子表：TaktMaintenanceNotification）
    /// </summary>
    public List<TaktMaintenanceNotificationDto>? MaintenanceNotifications { get; set; }

    /// <summary>
    /// 维护工单列表
    /// （子表：TaktMaintenanceWorkOrder）
    /// </summary>
    public List<TaktMaintenanceWorkOrderDto>? MaintenanceWorkOrders { get; set; }

    /// <summary>
    /// 维护履历列表（由维护工单完工归档生成，只读）
    /// （子表：TaktMaintenanceHistory）
    /// </summary>
    public List<TaktMaintenanceHistoryDto>? MaintenanceHistories { get; set; }

}

// ========================================
// Equipment 查询 DTO
// ========================================

/// <summary>
/// Equipment 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktEquipmentQueryDto : TaktPagedQuery
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
    /// 设备编码（唯一索引：租户+公司+工厂内唯一，见 ix_equipment_code_unique）
    /// </summary>
    public string? EquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备名称
    /// </summary>
    public string? EquipmentName { get; set; } = string.Empty;

    /// <summary>
    /// 登录设备（0=生产设备，1=检测设备，2=辅助设备，3=办公设备，4=其他设备）
    /// </summary>
    public int? EquipmentType { get; set; }

    /// <summary>
    /// 设备型号
    /// </summary>
    public string? EquipmentModel { get; set; } = string.Empty;

    /// <summary>
    /// 设备规格
    /// </summary>
    public string? EquipSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 设备品牌
    /// </summary>
    public string? EquipBrand { get; set; } = string.Empty;

    /// <summary>
    /// 制造商
    /// </summary>
    public string? Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// 经销商
    /// </summary>
    public string? DealerBy { get; set; } = string.Empty;

    /// <summary>
    /// 序列号/出厂编码
    /// </summary>
    public string? SerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 所属车间
    /// </summary>
    public string? WorkshopBy { get; set; } = string.Empty;

    /// <summary>
    /// 所属产线
    /// </summary>
    public string? ProductionLineBy { get; set; } = string.Empty;

    /// <summary>
    /// 所属工位
    /// </summary>
    public string? WorkstationBy { get; set; } = string.Empty;

    /// <summary>
    /// 所属部门
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 设备位置（详细位置描述）
    /// </summary>
    public string? EquipmentLocation { get; set; } = string.Empty;

    /// <summary>
    /// 负责人
    /// </summary>
    public string? ResponsibleUserName { get; set; } = string.Empty;

    /// <summary>
    /// 操作人
    /// </summary>
    public string? OperatorEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 购买日期（范围查询-开始）
    /// </summary>
    public DateTime? PurchaseDateStart { get; set; }

    /// <summary>
    /// 购买日期（范围查询-结束）
    /// </summary>
    public DateTime? PurchaseDateEnd { get; set; }

    /// <summary>
    /// 安装日期（范围查询-开始）
    /// </summary>
    public DateTime? InstallationDateStart { get; set; }

    /// <summary>
    /// 安装日期（范围查询-结束）
    /// </summary>
    public DateTime? InstallationDateEnd { get; set; }

    /// <summary>
    /// 启用日期（范围查询-开始）
    /// </summary>
    public DateTime? StartDateStart { get; set; }

    /// <summary>
    /// 启用日期（范围查询-结束）
    /// </summary>
    public DateTime? StartDateEnd { get; set; }

    /// <summary>
    /// 保修开始日期（范围查询-开始）
    /// </summary>
    public DateTime? WarrantyStartDateStart { get; set; }

    /// <summary>
    /// 保修开始日期（范围查询-结束）
    /// </summary>
    public DateTime? WarrantyStartDateEnd { get; set; }

    /// <summary>
    /// 保修结束日期（范围查询-开始）
    /// </summary>
    public DateTime? WarrantyEndDateStart { get; set; }

    /// <summary>
    /// 保修结束日期（范围查询-结束）
    /// </summary>
    public DateTime? WarrantyEndDateEnd { get; set; }

    /// <summary>
    /// 设备原值（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? EquipmentOriginalValue { get; set; }

    /// <summary>
    /// 设备技术参数（JSON格式，存储设备技术参数配置）
    /// </summary>
    public string? TechnicalParameters { get; set; } = string.Empty;

    /// <summary>
    /// 设备图片（JSON格式，存储设备图片URL列表）
    /// </summary>
    public string? EquipmentImages { get; set; } = string.Empty;

    /// <summary>
    /// 设备文档（JSON格式，存储设备文档ID列表）
    /// </summary>
    public string? EquipmentDocuments { get; set; } = string.Empty;

    /// <summary>
    /// 是否关键设备（0=否，1=是）
    /// </summary>
    public int? IsCritical { get; set; }

    /// <summary>
    /// 保修状态（0=无保修，1=保修期内，2=保修期外，3=延保中）
    /// </summary>
    public int? WarrantyStatus { get; set; }

    /// <summary>
    /// 设备状态（字典 sys_equipment_status）
    /// </summary>
    public int? EquipmentStatus { get; set; }

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
// 创建Equipment DTO
// ========================================

/// <summary>
/// 创建Equipment DTO
/// </summary>
public class TaktEquipmentCreateDto
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
    /// 设备编码（唯一索引：租户+公司+工厂内唯一，见 ix_equipment_code_unique）
    /// </summary>
    [Required(ErrorMessage = "设备编码（唯一索引：租户+公司+工厂内唯一，见 ix_equipment_code_unique）不能为空")]
    public string EquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备名称
    /// </summary>
    [Required(ErrorMessage = "设备名称不能为空")]
    public string EquipmentName { get; set; } = string.Empty;

    /// <summary>
    /// 登录设备（0=生产设备，1=检测设备，2=辅助设备，3=办公设备，4=其他设备）
    /// </summary>
    public int EquipmentType { get; set; } = 0;

    /// <summary>
    /// 设备型号
    /// </summary>
    public string? EquipmentModel { get; set; } = string.Empty;

    /// <summary>
    /// 设备规格
    /// </summary>
    public string? EquipSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 设备品牌
    /// </summary>
    public string? EquipBrand { get; set; } = string.Empty;

    /// <summary>
    /// 制造商
    /// </summary>
    public string? Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// 经销商
    /// </summary>
    public string? DealerBy { get; set; } = string.Empty;

    /// <summary>
    /// 序列号/出厂编码
    /// </summary>
    public string? SerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 所属车间
    /// </summary>
    public string? WorkshopBy { get; set; } = string.Empty;

    /// <summary>
    /// 所属产线
    /// </summary>
    public string? ProductionLineBy { get; set; } = string.Empty;

    /// <summary>
    /// 所属工位
    /// </summary>
    public string? WorkstationBy { get; set; } = string.Empty;

    /// <summary>
    /// 所属部门
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 设备位置（详细位置描述）
    /// </summary>
    public string? EquipmentLocation { get; set; } = string.Empty;

    /// <summary>
    /// 负责人
    /// </summary>
    public string? ResponsibleUserName { get; set; } = string.Empty;

    /// <summary>
    /// 操作人
    /// </summary>
    public string? OperatorEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 购买日期
    /// </summary>
    public DateTime? PurchaseDate { get; set; }

    /// <summary>
    /// 安装日期
    /// </summary>
    public DateTime? InstallationDate { get; set; }

    /// <summary>
    /// 启用日期
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// 保修开始日期
    /// </summary>
    public DateTime? WarrantyStartDate { get; set; }

    /// <summary>
    /// 保修结束日期
    /// </summary>
    public DateTime? WarrantyEndDate { get; set; }

    /// <summary>
    /// 设备原值（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal EquipmentOriginalValue { get; set; }

    /// <summary>
    /// 设备技术参数（JSON格式，存储设备技术参数配置）
    /// </summary>
    public string? TechnicalParameters { get; set; } = string.Empty;

    /// <summary>
    /// 设备图片（JSON格式，存储设备图片URL列表）
    /// </summary>
    public string? EquipmentImages { get; set; } = string.Empty;

    /// <summary>
    /// 设备文档（JSON格式，存储设备文档ID列表）
    /// </summary>
    public string? EquipmentDocuments { get; set; } = string.Empty;

    /// <summary>
    /// 是否关键设备（0=否，1=是）
    /// </summary>
    public int IsCritical { get; set; } = 0;

    /// <summary>
    /// 保修状态（0=无保修，1=保修期内，2=保修期外，3=延保中）
    /// </summary>
    public int WarrantyStatus { get; set; } = 0;

    /// <summary>
    /// 设备状态（字典 sys_equipment_status）
    /// </summary>
    public int EquipmentStatus { get; set; } = 0;

    /// <summary>
    /// 维护通知单列表（子表，级联保存）
    /// </summary>
    public List<TaktMaintenanceNotificationCreateDto>? MaintenanceNotifications { get; set; }

    /// <summary>
    /// 维护工单列表（子表，级联保存）
    /// </summary>
    public List<TaktMaintenanceWorkOrderCreateDto>? MaintenanceWorkOrders { get; set; }

    /// <summary>
    /// 维护履历列表（由维护工单完工归档生成，只读）（子表，级联保存）
    /// </summary>
    public List<TaktMaintenanceHistoryCreateDto>? MaintenanceHistories { get; set; }

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
// 更新Equipment DTO
// ========================================

/// <summary>
/// 更新Equipment DTO
/// 继承 TaktEquipmentCreateDto，添加 EquipmentId 字段
/// </summary>
public class TaktEquipmentUpdateDto : TaktEquipmentCreateDto
{
    /// <summary>
    /// EquipmentID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EquipmentId { get; set; }

    /// <summary>
    /// 维护通知单列表（子表，级联保存）
    /// </summary>
    public new List<TaktMaintenanceNotificationUpdateDto>? MaintenanceNotifications { get; set; }

    /// <summary>
    /// 维护工单列表（子表，级联保存）
    /// </summary>
    public new List<TaktMaintenanceWorkOrderUpdateDto>? MaintenanceWorkOrders { get; set; }

    /// <summary>
    /// 维护履历列表（由维护工单完工归档生成，只读）（子表，级联保存）
    /// </summary>
    public new List<TaktMaintenanceHistoryUpdateDto>? MaintenanceHistories { get; set; }

}

// ========================================
// Equipment 状态 DTO
// ========================================

/// <summary>
/// Equipment 状态更新 DTO
/// </summary>
public class TaktEquipmentStatusDto
{
    /// <summary>
    /// EquipmentID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EquipmentId { get; set; }

    /// <summary>
    /// 保修状态（0=无保修，1=保修期内，2=保修期外，3=延保中）
    /// </summary>
    [Required(ErrorMessage = "保修状态（0=无保修，1=保修期内，2=保修期外，3=延保中）不能为空")]
    public int WarrantyStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Equipment 导入模板行 DTO
/// </summary>
public class TaktEquipmentTemplateDto
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
    /// 设备编码（唯一索引：租户+公司+工厂内唯一，见 ix_equipment_code_unique）
    /// </summary>
    public string? EquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备名称
    /// </summary>
    public string? EquipmentName { get; set; } = string.Empty;

    /// <summary>
    /// 登录设备（0=生产设备，1=检测设备，2=辅助设备，3=办公设备，4=其他设备）
    /// </summary>
    public int? EquipmentType { get; set; }

    /// <summary>
    /// 设备型号
    /// </summary>
    public string? EquipmentModel { get; set; } = string.Empty;

    /// <summary>
    /// 设备规格
    /// </summary>
    public string? EquipSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 设备品牌
    /// </summary>
    public string? EquipBrand { get; set; } = string.Empty;

    /// <summary>
    /// 制造商
    /// </summary>
    public string? Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// 经销商
    /// </summary>
    public string? DealerBy { get; set; } = string.Empty;

    /// <summary>
    /// 序列号/出厂编码
    /// </summary>
    public string? SerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 所属车间
    /// </summary>
    public string? WorkshopBy { get; set; } = string.Empty;

    /// <summary>
    /// 所属产线
    /// </summary>
    public string? ProductionLineBy { get; set; } = string.Empty;

    /// <summary>
    /// 所属工位
    /// </summary>
    public string? WorkstationBy { get; set; } = string.Empty;

    /// <summary>
    /// 所属部门
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 设备位置（详细位置描述）
    /// </summary>
    public string? EquipmentLocation { get; set; } = string.Empty;

    /// <summary>
    /// 负责人
    /// </summary>
    public string? ResponsibleUserName { get; set; } = string.Empty;

    /// <summary>
    /// 操作人
    /// </summary>
    public string? OperatorEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 购买日期
    /// </summary>
    public DateTime? PurchaseDate { get; set; }

    /// <summary>
    /// 安装日期
    /// </summary>
    public DateTime? InstallationDate { get; set; }

    /// <summary>
    /// 启用日期
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// 保修开始日期
    /// </summary>
    public DateTime? WarrantyStartDate { get; set; }

    /// <summary>
    /// 保修结束日期
    /// </summary>
    public DateTime? WarrantyEndDate { get; set; }

    /// <summary>
    /// 设备原值（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? EquipmentOriginalValue { get; set; }

    /// <summary>
    /// 设备技术参数（JSON格式，存储设备技术参数配置）
    /// </summary>
    public string? TechnicalParameters { get; set; } = string.Empty;

    /// <summary>
    /// 设备图片（JSON格式，存储设备图片URL列表）
    /// </summary>
    public string? EquipmentImages { get; set; } = string.Empty;

    /// <summary>
    /// 设备文档（JSON格式，存储设备文档ID列表）
    /// </summary>
    public string? EquipmentDocuments { get; set; } = string.Empty;

    /// <summary>
    /// 是否关键设备（0=否，1=是）
    /// </summary>
    public int? IsCritical { get; set; }

    /// <summary>
    /// 保修状态（0=无保修，1=保修期内，2=保修期外，3=延保中）
    /// </summary>
    public int? WarrantyStatus { get; set; }

    /// <summary>
    /// 设备状态（字典 sys_equipment_status）
    /// </summary>
    public int? EquipmentStatus { get; set; }

    /// <summary>
    /// 维护通知单列表（子表，级联保存）
    /// </summary>
    public List<TaktMaintenanceNotificationCreateDto>? MaintenanceNotifications { get; set; }

    /// <summary>
    /// 维护工单列表（子表，级联保存）
    /// </summary>
    public List<TaktMaintenanceWorkOrderCreateDto>? MaintenanceWorkOrders { get; set; }

    /// <summary>
    /// 维护履历列表（由维护工单完工归档生成，只读）（子表，级联保存）
    /// </summary>
    public List<TaktMaintenanceHistoryCreateDto>? MaintenanceHistories { get; set; }

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
/// Equipment 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktEquipmentImportDto
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
    /// 设备编码（唯一索引：租户+公司+工厂内唯一，见 ix_equipment_code_unique）
    /// </summary>
    public string? EquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备名称
    /// </summary>
    public string? EquipmentName { get; set; } = string.Empty;

    /// <summary>
    /// 登录设备（0=生产设备，1=检测设备，2=辅助设备，3=办公设备，4=其他设备）
    /// </summary>
    public int? EquipmentType { get; set; }

    /// <summary>
    /// 设备型号
    /// </summary>
    public string? EquipmentModel { get; set; } = string.Empty;

    /// <summary>
    /// 设备规格
    /// </summary>
    public string? EquipSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 设备品牌
    /// </summary>
    public string? EquipBrand { get; set; } = string.Empty;

    /// <summary>
    /// 制造商
    /// </summary>
    public string? Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// 经销商
    /// </summary>
    public string? DealerBy { get; set; } = string.Empty;

    /// <summary>
    /// 序列号/出厂编码
    /// </summary>
    public string? SerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 所属车间
    /// </summary>
    public string? WorkshopBy { get; set; } = string.Empty;

    /// <summary>
    /// 所属产线
    /// </summary>
    public string? ProductionLineBy { get; set; } = string.Empty;

    /// <summary>
    /// 所属工位
    /// </summary>
    public string? WorkstationBy { get; set; } = string.Empty;

    /// <summary>
    /// 所属部门
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 设备位置（详细位置描述）
    /// </summary>
    public string? EquipmentLocation { get; set; } = string.Empty;

    /// <summary>
    /// 负责人
    /// </summary>
    public string? ResponsibleUserName { get; set; } = string.Empty;

    /// <summary>
    /// 操作人
    /// </summary>
    public string? OperatorEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 购买日期
    /// </summary>
    public DateTime? PurchaseDate { get; set; }

    /// <summary>
    /// 安装日期
    /// </summary>
    public DateTime? InstallationDate { get; set; }

    /// <summary>
    /// 启用日期
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// 保修开始日期
    /// </summary>
    public DateTime? WarrantyStartDate { get; set; }

    /// <summary>
    /// 保修结束日期
    /// </summary>
    public DateTime? WarrantyEndDate { get; set; }

    /// <summary>
    /// 设备原值（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? EquipmentOriginalValue { get; set; }

    /// <summary>
    /// 设备技术参数（JSON格式，存储设备技术参数配置）
    /// </summary>
    public string? TechnicalParameters { get; set; } = string.Empty;

    /// <summary>
    /// 设备图片（JSON格式，存储设备图片URL列表）
    /// </summary>
    public string? EquipmentImages { get; set; } = string.Empty;

    /// <summary>
    /// 设备文档（JSON格式，存储设备文档ID列表）
    /// </summary>
    public string? EquipmentDocuments { get; set; } = string.Empty;

    /// <summary>
    /// 是否关键设备（0=否，1=是）
    /// </summary>
    public int? IsCritical { get; set; }

    /// <summary>
    /// 保修状态（0=无保修，1=保修期内，2=保修期外，3=延保中）
    /// </summary>
    public int? WarrantyStatus { get; set; }

    /// <summary>
    /// 设备状态（字典 sys_equipment_status）
    /// </summary>
    public int? EquipmentStatus { get; set; }

    /// <summary>
    /// 维护通知单列表（子表，级联保存）
    /// </summary>
    public List<TaktMaintenanceNotificationCreateDto>? MaintenanceNotifications { get; set; }

    /// <summary>
    /// 维护工单列表（子表，级联保存）
    /// </summary>
    public List<TaktMaintenanceWorkOrderCreateDto>? MaintenanceWorkOrders { get; set; }

    /// <summary>
    /// 维护履历列表（由维护工单完工归档生成，只读）（子表，级联保存）
    /// </summary>
    public List<TaktMaintenanceHistoryCreateDto>? MaintenanceHistories { get; set; }

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
/// Equipment 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktEquipmentExportDto
{
    /// <summary>
    /// EquipmentID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EquipmentId { get; set; }

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
    /// 设备编码（唯一索引：租户+公司+工厂内唯一，见 ix_equipment_code_unique）
    /// </summary>
    public string EquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备名称
    /// </summary>
    public string EquipmentName { get; set; } = string.Empty;

    /// <summary>
    /// 登录设备（0=生产设备，1=检测设备，2=辅助设备，3=办公设备，4=其他设备）
    /// </summary>
    public int EquipmentType { get; set; } = 0;

    /// <summary>
    /// 设备型号
    /// </summary>
    public string? EquipmentModel { get; set; } = string.Empty;

    /// <summary>
    /// 设备规格
    /// </summary>
    public string? EquipSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 设备品牌
    /// </summary>
    public string? EquipBrand { get; set; } = string.Empty;

    /// <summary>
    /// 制造商
    /// </summary>
    public string? Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// 经销商
    /// </summary>
    public string? DealerBy { get; set; } = string.Empty;

    /// <summary>
    /// 序列号/出厂编码
    /// </summary>
    public string? SerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 所属车间
    /// </summary>
    public string? WorkshopBy { get; set; } = string.Empty;

    /// <summary>
    /// 所属产线
    /// </summary>
    public string? ProductionLineBy { get; set; } = string.Empty;

    /// <summary>
    /// 所属工位
    /// </summary>
    public string? WorkstationBy { get; set; } = string.Empty;

    /// <summary>
    /// 所属部门
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 设备位置（详细位置描述）
    /// </summary>
    public string? EquipmentLocation { get; set; } = string.Empty;

    /// <summary>
    /// 负责人
    /// </summary>
    public string? ResponsibleUserName { get; set; } = string.Empty;

    /// <summary>
    /// 操作人
    /// </summary>
    public string? OperatorEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 购买日期
    /// </summary>
    public DateTime? PurchaseDate { get; set; }

    /// <summary>
    /// 安装日期
    /// </summary>
    public DateTime? InstallationDate { get; set; }

    /// <summary>
    /// 启用日期
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// 保修开始日期
    /// </summary>
    public DateTime? WarrantyStartDate { get; set; }

    /// <summary>
    /// 保修结束日期
    /// </summary>
    public DateTime? WarrantyEndDate { get; set; }

    /// <summary>
    /// 设备原值（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal EquipmentOriginalValue { get; set; }

    /// <summary>
    /// 设备技术参数（JSON格式，存储设备技术参数配置）
    /// </summary>
    public string? TechnicalParameters { get; set; } = string.Empty;

    /// <summary>
    /// 设备图片（JSON格式，存储设备图片URL列表）
    /// </summary>
    public string? EquipmentImages { get; set; } = string.Empty;

    /// <summary>
    /// 设备文档（JSON格式，存储设备文档ID列表）
    /// </summary>
    public string? EquipmentDocuments { get; set; } = string.Empty;

    /// <summary>
    /// 是否关键设备（0=否，1=是）
    /// </summary>
    public int IsCritical { get; set; } = 0;

    /// <summary>
    /// 保修状态（0=无保修，1=保修期内，2=保修期外，3=延保中）
    /// </summary>
    public int WarrantyStatus { get; set; } = 0;

    /// <summary>
    /// 设备状态（字典 sys_equipment_status）
    /// </summary>
    public int EquipmentStatus { get; set; } = 0;

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
