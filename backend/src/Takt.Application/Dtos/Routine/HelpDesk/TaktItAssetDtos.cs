// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Routine.HelpDesk
// 文件名称：TaktItAssetDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：ItAsset 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktItAsset 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Routine.HelpDesk;

// ========================================
// ItAsset 响应 DTO
// ========================================

/// <summary>
/// 服务台 IT 设备保修扩展实体（与财务 TaktAsset 按 AssetCode 一对一扩展）
/// 对应前端 TaktItAssetDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktItAssetDto : TaktCompanyDtoBase
{
    /// <summary>
    /// ItAssetID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ItAssetId { get; set; }

    /// <summary>
    /// 资产号码（选项 TaktAssets/options；DictValue=AssetCode）
    /// </summary>
    public string AssetCode { get; set; } = string.Empty;

    /// <summary>
    /// 保修类型（字典 sys_warranty_type；0=原厂保修 1=延长保修 2=上门保修 3=寄修保修 4=维保合同 5=付费保养）
    /// </summary>
    public int WarrantyType { get; set; } = 0;

    /// <summary>
    /// 保修开始日期
    /// </summary>
    public DateTime? WarrantyStartDate { get; set; }

    /// <summary>
    /// 保修到期日
    /// </summary>
    public DateTime? WarrantyExpiryDate { get; set; }

    /// <summary>
    /// 保修服务商/厂商
    /// </summary>
    public string? WarrantyProvider { get; set; } = string.Empty;

    /// <summary>
    /// 保修合同编码
    /// </summary>
    public string? WarrantyContractCode { get; set; } = string.Empty;

    /// <summary>
    /// 服务电话
    /// </summary>
    public string? ServiceHotline { get; set; } = string.Empty;

    /// <summary>
    /// 服务邮箱
    /// </summary>
    public string? ServiceEmail { get; set; } = string.Empty;

    /// <summary>
    /// 维保到期日
    /// </summary>
    public DateTime? MaintenanceExpiryDate { get; set; }

    /// <summary>
    /// 上次维保日期
    /// </summary>
    public DateTime? LastMaintenanceDate { get; set; }

    /// <summary>
    /// 下次维保日期
    /// </summary>
    public DateTime? NextMaintenanceDate { get; set; }

    /// <summary>
    /// 保修/维保说明
    /// </summary>
    public string? WarrantyRemark { get; set; } = string.Empty;

}

// ========================================
// ItAsset 查询 DTO
// ========================================

/// <summary>
/// ItAsset 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktItAssetQueryDto : TaktPagedQuery
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 资产号码（选项 TaktAssets/options；DictValue=AssetCode）
    /// </summary>
    public string? AssetCode { get; set; } = string.Empty;

    /// <summary>
    /// 保修类型（字典 sys_warranty_type；0=原厂保修 1=延长保修 2=上门保修 3=寄修保修 4=维保合同 5=付费保养）
    /// </summary>
    public int? WarrantyType { get; set; }

    /// <summary>
    /// 保修开始日期（范围查询-开始）
    /// </summary>
    public DateTime? WarrantyStartDateStart { get; set; }

    /// <summary>
    /// 保修开始日期（范围查询-结束）
    /// </summary>
    public DateTime? WarrantyStartDateEnd { get; set; }

    /// <summary>
    /// 保修到期日（范围查询-开始）
    /// </summary>
    public DateTime? WarrantyExpiryDateStart { get; set; }

    /// <summary>
    /// 保修到期日（范围查询-结束）
    /// </summary>
    public DateTime? WarrantyExpiryDateEnd { get; set; }

    /// <summary>
    /// 保修服务商/厂商
    /// </summary>
    public string? WarrantyProvider { get; set; } = string.Empty;

    /// <summary>
    /// 保修合同编码
    /// </summary>
    public string? WarrantyContractCode { get; set; } = string.Empty;

    /// <summary>
    /// 服务电话
    /// </summary>
    public string? ServiceHotline { get; set; } = string.Empty;

    /// <summary>
    /// 服务邮箱
    /// </summary>
    public string? ServiceEmail { get; set; } = string.Empty;

    /// <summary>
    /// 维保到期日（范围查询-开始）
    /// </summary>
    public DateTime? MaintenanceExpiryDateStart { get; set; }

    /// <summary>
    /// 维保到期日（范围查询-结束）
    /// </summary>
    public DateTime? MaintenanceExpiryDateEnd { get; set; }

    /// <summary>
    /// 上次维保日期（范围查询-开始）
    /// </summary>
    public DateTime? LastMaintenanceDateStart { get; set; }

    /// <summary>
    /// 上次维保日期（范围查询-结束）
    /// </summary>
    public DateTime? LastMaintenanceDateEnd { get; set; }

    /// <summary>
    /// 下次维保日期（范围查询-开始）
    /// </summary>
    public DateTime? NextMaintenanceDateStart { get; set; }

    /// <summary>
    /// 下次维保日期（范围查询-结束）
    /// </summary>
    public DateTime? NextMaintenanceDateEnd { get; set; }

    /// <summary>
    /// 保修/维保说明
    /// </summary>
    public string? WarrantyRemark { get; set; } = string.Empty;

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
// 创建ItAsset DTO
// ========================================

/// <summary>
/// 创建ItAsset DTO
/// </summary>
public class TaktItAssetCreateDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 资产号码（选项 TaktAssets/options；DictValue=AssetCode）
    /// </summary>
    [Required(ErrorMessage = "资产号码（选项 TaktAssets/options；DictValue=AssetCode）不能为空")]
    public string AssetCode { get; set; } = string.Empty;

    /// <summary>
    /// 保修类型（字典 sys_warranty_type；0=原厂保修 1=延长保修 2=上门保修 3=寄修保修 4=维保合同 5=付费保养）
    /// </summary>
    public int WarrantyType { get; set; } = 0;

    /// <summary>
    /// 保修开始日期
    /// </summary>
    public DateTime? WarrantyStartDate { get; set; }

    /// <summary>
    /// 保修到期日
    /// </summary>
    public DateTime? WarrantyExpiryDate { get; set; }

    /// <summary>
    /// 保修服务商/厂商
    /// </summary>
    public string? WarrantyProvider { get; set; } = string.Empty;

    /// <summary>
    /// 保修合同编码
    /// </summary>
    public string? WarrantyContractCode { get; set; } = string.Empty;

    /// <summary>
    /// 服务电话
    /// </summary>
    public string? ServiceHotline { get; set; } = string.Empty;

    /// <summary>
    /// 服务邮箱
    /// </summary>
    public string? ServiceEmail { get; set; } = string.Empty;

    /// <summary>
    /// 维保到期日
    /// </summary>
    public DateTime? MaintenanceExpiryDate { get; set; }

    /// <summary>
    /// 上次维保日期
    /// </summary>
    public DateTime? LastMaintenanceDate { get; set; }

    /// <summary>
    /// 下次维保日期
    /// </summary>
    public DateTime? NextMaintenanceDate { get; set; }

    /// <summary>
    /// 保修/维保说明
    /// </summary>
    public string? WarrantyRemark { get; set; } = string.Empty;

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
// 更新ItAsset DTO
// ========================================

/// <summary>
/// 更新ItAsset DTO
/// 继承 TaktItAssetCreateDto，添加 ItAssetId 字段
/// </summary>
public class TaktItAssetUpdateDto : TaktItAssetCreateDto
{
    /// <summary>
    /// ItAssetID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ItAssetId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// ItAsset 导入模板行 DTO
/// </summary>
public class TaktItAssetTemplateDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 资产号码（选项 TaktAssets/options；DictValue=AssetCode）
    /// </summary>
    public string? AssetCode { get; set; } = string.Empty;

    /// <summary>
    /// 保修类型（字典 sys_warranty_type；0=原厂保修 1=延长保修 2=上门保修 3=寄修保修 4=维保合同 5=付费保养）
    /// </summary>
    public int? WarrantyType { get; set; }

    /// <summary>
    /// 保修开始日期
    /// </summary>
    public DateTime? WarrantyStartDate { get; set; }

    /// <summary>
    /// 保修到期日
    /// </summary>
    public DateTime? WarrantyExpiryDate { get; set; }

    /// <summary>
    /// 保修服务商/厂商
    /// </summary>
    public string? WarrantyProvider { get; set; } = string.Empty;

    /// <summary>
    /// 保修合同编码
    /// </summary>
    public string? WarrantyContractCode { get; set; } = string.Empty;

    /// <summary>
    /// 服务电话
    /// </summary>
    public string? ServiceHotline { get; set; } = string.Empty;

    /// <summary>
    /// 服务邮箱
    /// </summary>
    public string? ServiceEmail { get; set; } = string.Empty;

    /// <summary>
    /// 维保到期日
    /// </summary>
    public DateTime? MaintenanceExpiryDate { get; set; }

    /// <summary>
    /// 上次维保日期
    /// </summary>
    public DateTime? LastMaintenanceDate { get; set; }

    /// <summary>
    /// 下次维保日期
    /// </summary>
    public DateTime? NextMaintenanceDate { get; set; }

    /// <summary>
    /// 保修/维保说明
    /// </summary>
    public string? WarrantyRemark { get; set; } = string.Empty;

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
/// ItAsset 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktItAssetImportDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 资产号码（选项 TaktAssets/options；DictValue=AssetCode）
    /// </summary>
    public string? AssetCode { get; set; } = string.Empty;

    /// <summary>
    /// 保修类型（字典 sys_warranty_type；0=原厂保修 1=延长保修 2=上门保修 3=寄修保修 4=维保合同 5=付费保养）
    /// </summary>
    public int? WarrantyType { get; set; }

    /// <summary>
    /// 保修开始日期
    /// </summary>
    public DateTime? WarrantyStartDate { get; set; }

    /// <summary>
    /// 保修到期日
    /// </summary>
    public DateTime? WarrantyExpiryDate { get; set; }

    /// <summary>
    /// 保修服务商/厂商
    /// </summary>
    public string? WarrantyProvider { get; set; } = string.Empty;

    /// <summary>
    /// 保修合同编码
    /// </summary>
    public string? WarrantyContractCode { get; set; } = string.Empty;

    /// <summary>
    /// 服务电话
    /// </summary>
    public string? ServiceHotline { get; set; } = string.Empty;

    /// <summary>
    /// 服务邮箱
    /// </summary>
    public string? ServiceEmail { get; set; } = string.Empty;

    /// <summary>
    /// 维保到期日
    /// </summary>
    public DateTime? MaintenanceExpiryDate { get; set; }

    /// <summary>
    /// 上次维保日期
    /// </summary>
    public DateTime? LastMaintenanceDate { get; set; }

    /// <summary>
    /// 下次维保日期
    /// </summary>
    public DateTime? NextMaintenanceDate { get; set; }

    /// <summary>
    /// 保修/维保说明
    /// </summary>
    public string? WarrantyRemark { get; set; } = string.Empty;

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
/// ItAsset 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktItAssetExportDto
{
    /// <summary>
    /// ItAssetID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ItAssetId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 资产号码（选项 TaktAssets/options；DictValue=AssetCode）
    /// </summary>
    public string AssetCode { get; set; } = string.Empty;

    /// <summary>
    /// 保修类型（字典 sys_warranty_type；0=原厂保修 1=延长保修 2=上门保修 3=寄修保修 4=维保合同 5=付费保养）
    /// </summary>
    public int WarrantyType { get; set; } = 0;

    /// <summary>
    /// 保修开始日期
    /// </summary>
    public DateTime? WarrantyStartDate { get; set; }

    /// <summary>
    /// 保修到期日
    /// </summary>
    public DateTime? WarrantyExpiryDate { get; set; }

    /// <summary>
    /// 保修服务商/厂商
    /// </summary>
    public string? WarrantyProvider { get; set; } = string.Empty;

    /// <summary>
    /// 保修合同编码
    /// </summary>
    public string? WarrantyContractCode { get; set; } = string.Empty;

    /// <summary>
    /// 服务电话
    /// </summary>
    public string? ServiceHotline { get; set; } = string.Empty;

    /// <summary>
    /// 服务邮箱
    /// </summary>
    public string? ServiceEmail { get; set; } = string.Empty;

    /// <summary>
    /// 维保到期日
    /// </summary>
    public DateTime? MaintenanceExpiryDate { get; set; }

    /// <summary>
    /// 上次维保日期
    /// </summary>
    public DateTime? LastMaintenanceDate { get; set; }

    /// <summary>
    /// 下次维保日期
    /// </summary>
    public DateTime? NextMaintenanceDate { get; set; }

    /// <summary>
    /// 保修/维保说明
    /// </summary>
    public string? WarrantyRemark { get; set; } = string.Empty;

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
