// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcAttachmentDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：EcAttachment 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktEcAttachment 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;

// ========================================
// EcAttachment 响应 DTO
// ========================================

/// <summary>
/// 设变附件实体（技术阶段一 ②，隶属 TaktEcTe）。文件类别见字典 logistics_manufacturing_ec_attachment_type；与主表、明细保存后由系统生成 TaktEcNotification。
/// 对应前端 TaktEcAttachmentDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktEcAttachmentDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EcAttachmentID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcAttachmentId { get; set; }

    /// <summary>
    /// 设变主表ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcTeId { get; set; }

    /// <summary>
    /// 设变主表名称（填充字段）
    /// </summary>
    public string? EcName { get; set; }

    /// <summary>
    /// 设变单号（冗余字段,便于查询）
    /// </summary>
    public string EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 文件类别（字典 logistics_manufacturing_ec_attachment_type；TL=联络，EPP=EPP，FPP=FPP，EL=外部联络，TCJ=TCJ，源PDF=源PDF，EC=EC）
    /// </summary>
    public string AttachmentType { get; set; } = string.Empty;

    /// <summary>
    /// 文件编码（如联络编码等）
    /// </summary>
    public string DocCode { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（等于文件编码 DocCode + 原扩展名，与源文件名无关）
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（URL）
    /// </summary>
    public string AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 设变主表（多对一）
    /// （主表：TaktEcTe）
    /// </summary>
    public TaktEcTeDto? EcTe { get; set; }

}

// ========================================
// EcAttachment 查询 DTO
// ========================================

/// <summary>
/// EcAttachment 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktEcAttachmentQueryDto : TaktPagedQuery
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
    /// 设变主表ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcTeId { get; set; }

    /// <summary>
    /// 设变单号（冗余字段,便于查询）
    /// </summary>
    public string? EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 文件类别（字典 logistics_manufacturing_ec_attachment_type；TL=联络，EPP=EPP，FPP=FPP，EL=外部联络，TCJ=TCJ，源PDF=源PDF，EC=EC）
    /// </summary>
    public string? AttachmentType { get; set; } = string.Empty;

    /// <summary>
    /// 文件编码（如联络编码等）
    /// </summary>
    public string? DocCode { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（URL）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

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
// 创建EcAttachment DTO
// ========================================

/// <summary>
/// 创建EcAttachment DTO
/// </summary>
public class TaktEcAttachmentCreateDto
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
    /// 设变主表ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcTeId { get; set; }

    /// <summary>
    /// 设变单号（冗余字段,便于查询）
    /// </summary>
    public string EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 文件类别（字典 logistics_manufacturing_ec_attachment_type；TL=联络，EPP=EPP，FPP=FPP，EL=外部联络，TCJ=TCJ，源PDF=源PDF，EC=EC）
    /// </summary>
    [Required(ErrorMessage = "文件类别（字典 logistics_manufacturing_ec_attachment_type；TL=联络，EPP=EPP，FPP=FPP，EL=外部联络，TCJ=TCJ，源PDF=源PDF，EC=EC）不能为空")]
    public string AttachmentType { get; set; } = string.Empty;

    /// <summary>
    /// 文件编码（如联络编码等）
    /// </summary>
    [Required(ErrorMessage = "文件编码（如联络编码等）不能为空")]
    public string DocCode { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（上传后强制等于文件编码 DocCode + 原扩展名，与源文件名无关）
    /// </summary>
    [Required(ErrorMessage = "文件名称不能为空")]
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（URL）
    /// </summary>
    [Required(ErrorMessage = "访问地址（URL）不能为空")]
    public string AccessUrl { get; set; } = string.Empty;

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
// 更新EcAttachment DTO
// ========================================

/// <summary>
/// 更新EcAttachment DTO
/// 继承 TaktEcAttachmentCreateDto，添加 EcAttachmentId 字段
/// </summary>
public class TaktEcAttachmentUpdateDto : TaktEcAttachmentCreateDto
{
    /// <summary>
    /// EcAttachmentID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcAttachmentId { get; set; }

}

// ========================================
// EcAttachment 作废 DTO
// ========================================

/// <summary>
/// EcAttachment 作废/撤销作废 DTO
/// </summary>
public class TaktEcAttachmentObsoleteDto
{
    /// <summary>
    /// EcAttachmentID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcAttachmentId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// EcAttachment 导入模板行 DTO
/// </summary>
public class TaktEcAttachmentTemplateDto
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
    /// 设变主表ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcTeId { get; set; }

    /// <summary>
    /// 设变单号（冗余字段,便于查询）
    /// </summary>
    public string? EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 文件类别（字典 logistics_manufacturing_ec_attachment_type；TL=联络，EPP=EPP，FPP=FPP，EL=外部联络，TCJ=TCJ，源PDF=源PDF，EC=EC）
    /// </summary>
    public string? AttachmentType { get; set; } = string.Empty;

    /// <summary>
    /// 文件编码（如联络编码等）
    /// </summary>
    public string? DocCode { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（URL）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

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
/// EcAttachment 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktEcAttachmentImportDto
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
    /// 设变主表ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcTeId { get; set; }

    /// <summary>
    /// 设变单号（冗余字段,便于查询）
    /// </summary>
    public string? EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 文件类别（字典 logistics_manufacturing_ec_attachment_type；TL=联络，EPP=EPP，FPP=FPP，EL=外部联络，TCJ=TCJ，源PDF=源PDF，EC=EC）
    /// </summary>
    public string? AttachmentType { get; set; } = string.Empty;

    /// <summary>
    /// 文件编码（如联络编码等）
    /// </summary>
    public string? DocCode { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（URL）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

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
/// EcAttachment 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktEcAttachmentExportDto
{
    /// <summary>
    /// EcAttachmentID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcAttachmentId { get; set; }

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
    /// 设变主表ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcTeId { get; set; }

    /// <summary>
    /// 设变单号（冗余字段,便于查询）
    /// </summary>
    public string EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 文件类别（字典 logistics_manufacturing_ec_attachment_type；TL=联络，EPP=EPP，FPP=FPP，EL=外部联络，TCJ=TCJ，源PDF=源PDF，EC=EC）
    /// </summary>
    public string AttachmentType { get; set; } = string.Empty;

    /// <summary>
    /// 文件编码（如联络编码等）
    /// </summary>
    public string DocCode { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（等于文件编码 DocCode + 原扩展名，与源文件名无关）
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（URL）
    /// </summary>
    public string AccessUrl { get; set; } = string.Empty;

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
