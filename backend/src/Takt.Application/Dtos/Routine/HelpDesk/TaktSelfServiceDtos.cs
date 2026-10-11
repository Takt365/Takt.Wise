// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Routine.HelpDesk
// 文件名称：TaktSelfServiceDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：SelfService 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSelfService 生成，请按需审阅）
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
// SelfService 响应 DTO
// ========================================

/// <summary>
/// 服务台自助服务项实体
/// 对应前端 TaktSelfServiceDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSelfServiceDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SelfServiceID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SelfServiceId { get; set; }

    /// <summary>
    /// 自助服务名称
    /// </summary>
    public string ServiceName { get; set; } = string.Empty;

    /// <summary>
    /// 服务类型（字典 routine_self_service_type；0=链接 1=表单 2=知识引导）
    /// </summary>
    public int ServiceType { get; set; } = 0;

    /// <summary>
    /// 描述
    /// </summary>
    public string? SelfServiceDescription { get; set; } = string.Empty;

    /// <summary>
    /// 链接地址或表单编码
    /// </summary>
    public string? LinkOrCode { get; set; } = string.Empty;

    /// <summary>
    /// 图标或图片 URL
    /// </summary>
    public string? IconUrl { get; set; } = string.Empty;

    /// <summary>
    /// 附件（JSON 列表形式，由 TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 自助服务状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int SelfServiceStatus { get; set; } = 0;

}

// ========================================
// SelfService 查询 DTO
// ========================================

/// <summary>
/// SelfService 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSelfServiceQueryDto : TaktPagedQuery
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
    /// 自助服务名称
    /// </summary>
    public string? ServiceName { get; set; } = string.Empty;

    /// <summary>
    /// 服务类型（字典 routine_self_service_type；0=链接 1=表单 2=知识引导）
    /// </summary>
    public int? ServiceType { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string? SelfServiceDescription { get; set; } = string.Empty;

    /// <summary>
    /// 链接地址或表单编码
    /// </summary>
    public string? LinkOrCode { get; set; } = string.Empty;

    /// <summary>
    /// 图标或图片 URL
    /// </summary>
    public string? IconUrl { get; set; } = string.Empty;

    /// <summary>
    /// 附件（JSON 列表形式，由 TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 自助服务状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int? SelfServiceStatus { get; set; }

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
// 创建SelfService DTO
// ========================================

/// <summary>
/// 创建SelfService DTO
/// </summary>
public class TaktSelfServiceCreateDto
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
    /// 自助服务名称
    /// </summary>
    [Required(ErrorMessage = "自助服务名称不能为空")]
    public string ServiceName { get; set; } = string.Empty;

    /// <summary>
    /// 服务类型（字典 routine_self_service_type；0=链接 1=表单 2=知识引导）
    /// </summary>
    public int ServiceType { get; set; } = 0;

    /// <summary>
    /// 描述
    /// </summary>
    public string? SelfServiceDescription { get; set; } = string.Empty;

    /// <summary>
    /// 链接地址或表单编码
    /// </summary>
    public string? LinkOrCode { get; set; } = string.Empty;

    /// <summary>
    /// 图标或图片 URL
    /// </summary>
    public string? IconUrl { get; set; } = string.Empty;

    /// <summary>
    /// 附件（JSON 列表形式，由 TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

    /// <summary>
    /// 自助服务状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int SelfServiceStatus { get; set; } = 0;

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
// 更新SelfService DTO
// ========================================

/// <summary>
/// 更新SelfService DTO
/// 继承 TaktSelfServiceCreateDto，添加 SelfServiceId 字段
/// </summary>
public class TaktSelfServiceUpdateDto : TaktSelfServiceCreateDto
{
    /// <summary>
    /// SelfServiceID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SelfServiceId { get; set; }

}

// ========================================
// SelfService 状态 DTO
// ========================================

/// <summary>
/// SelfService 状态更新 DTO
/// </summary>
public class TaktSelfServiceStatusDto
{
    /// <summary>
    /// SelfServiceID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SelfServiceId { get; set; }

    /// <summary>
    /// 自助服务状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    [Required(ErrorMessage = "自助服务状态（字典 sys_normal_disable；1=启用 0=禁用）不能为空")]
    public int SelfServiceStatus { get; set; } = 0;
}

// ========================================
// SelfService 排序 DTO
// ========================================

/// <summary>
/// SelfService 排序更新 DTO
/// </summary>
public class TaktSelfServiceSortDto
{
    /// <summary>
    /// SelfServiceID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SelfServiceId { get; set; }

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    [Required(ErrorMessage = "排序号（越小越靠前）不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SelfService 导入模板行 DTO
/// </summary>
public class TaktSelfServiceTemplateDto
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
    /// 自助服务名称
    /// </summary>
    public string? ServiceName { get; set; } = string.Empty;

    /// <summary>
    /// 服务类型（字典 routine_self_service_type；0=链接 1=表单 2=知识引导）
    /// </summary>
    public int? ServiceType { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string? SelfServiceDescription { get; set; } = string.Empty;

    /// <summary>
    /// 链接地址或表单编码
    /// </summary>
    public string? LinkOrCode { get; set; } = string.Empty;

    /// <summary>
    /// 图标或图片 URL
    /// </summary>
    public string? IconUrl { get; set; } = string.Empty;

    /// <summary>
    /// 附件（JSON 列表形式，由 TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

    /// <summary>
    /// 自助服务状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int? SelfServiceStatus { get; set; }

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
/// SelfService 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSelfServiceImportDto
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
    /// 自助服务名称
    /// </summary>
    public string? ServiceName { get; set; } = string.Empty;

    /// <summary>
    /// 服务类型（字典 routine_self_service_type；0=链接 1=表单 2=知识引导）
    /// </summary>
    public int? ServiceType { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string? SelfServiceDescription { get; set; } = string.Empty;

    /// <summary>
    /// 链接地址或表单编码
    /// </summary>
    public string? LinkOrCode { get; set; } = string.Empty;

    /// <summary>
    /// 图标或图片 URL
    /// </summary>
    public string? IconUrl { get; set; } = string.Empty;

    /// <summary>
    /// 附件（JSON 列表形式，由 TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

    /// <summary>
    /// 自助服务状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int? SelfServiceStatus { get; set; }

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
/// SelfService 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSelfServiceExportDto
{
    /// <summary>
    /// SelfServiceID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SelfServiceId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 自助服务名称
    /// </summary>
    public string ServiceName { get; set; } = string.Empty;

    /// <summary>
    /// 服务类型（字典 routine_self_service_type；0=链接 1=表单 2=知识引导）
    /// </summary>
    public int ServiceType { get; set; } = 0;

    /// <summary>
    /// 描述
    /// </summary>
    public string? SelfServiceDescription { get; set; } = string.Empty;

    /// <summary>
    /// 链接地址或表单编码
    /// </summary>
    public string? LinkOrCode { get; set; } = string.Empty;

    /// <summary>
    /// 图标或图片 URL
    /// </summary>
    public string? IconUrl { get; set; } = string.Empty;

    /// <summary>
    /// 附件（JSON 列表形式，由 TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 自助服务状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int SelfServiceStatus { get; set; } = 0;

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
