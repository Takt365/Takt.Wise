// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Sop
// 文件名称：TaktSopAckDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：SopAck 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSopAck 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Sop;

// ========================================
// SopAck 响应 DTO
// ========================================

/// <summary>
/// SOP 确认实体
/// 对应前端 TaktSopAckDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSopAckDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SopAckID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopAckId { get; set; }


    /// <summary>
    /// SOP 主档 ID（选项 TaktSopDocs/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopId { get; set; }

    /// <summary>
    /// SOP 主档 名称（填充字段）
    /// </summary>
    public string? SopName { get; set; }

    /// <summary>
    /// SOP 版本 ID（选项 TaktSopRevisions/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RevisionId { get; set; }

    /// <summary>
    /// SOP 版本 名称（填充字段）
    /// </summary>
    public string? RevisionName { get; set; }

    /// <summary>
    /// 工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? WorkstationId { get; set; }

    /// <summary>
    /// 工位 名称（填充字段）
    /// </summary>
    public string? WorkstationName { get; set; }

    /// <summary>
    /// 确认人 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AcknowledgedBy { get; set; }

    /// <summary>
    /// 确认时间
    /// </summary>
    public DateTime AcknowledgedAt { get; set; }

    /// <summary>
    /// 确认意见
    /// </summary>
    public string? AckComment { get; set; } = string.Empty;

    /// <summary>
    /// SOP 主档
    /// （主表：TaktSopDoc）
    /// </summary>
    public TaktSopDocDto? SopDoc { get; set; }

    /// <summary>
    /// SOP 版本
    /// （主表：TaktSopRevision）
    /// </summary>
    public TaktSopRevisionDto? Revision { get; set; }

    /// <summary>
    /// 工位
    /// （主表：TaktSopWorkstation）
    /// </summary>
    public TaktSopWorkstationDto? Workstation { get; set; }

}

// ========================================
// SopAck 查询 DTO
// ========================================

/// <summary>
/// SopAck 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSopAckQueryDto : TaktPagedQuery
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// SOP 主档 ID（选项 TaktSopDocs/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SopId { get; set; }

    /// <summary>
    /// SOP 版本 ID（选项 TaktSopRevisions/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RevisionId { get; set; }

    /// <summary>
    /// 工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? WorkstationId { get; set; }

    /// <summary>
    /// 确认人 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AcknowledgedBy { get; set; }

    /// <summary>
    /// 确认时间（范围查询-开始）
    /// </summary>
    public DateTime? AcknowledgedAtStart { get; set; }

    /// <summary>
    /// 确认时间（范围查询-结束）
    /// </summary>
    public DateTime? AcknowledgedAtEnd { get; set; }

    /// <summary>
    /// 确认意见
    /// </summary>
    public string? AckComment { get; set; } = string.Empty;

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
// 创建SopAck DTO
// ========================================

/// <summary>
/// 创建SopAck DTO
/// </summary>
public class TaktSopAckCreateDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "工厂代码（选项 TaktPlants/options；DictValue=PlantCode）不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// SOP 主档 ID（选项 TaktSopDocs/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopId { get; set; }

    /// <summary>
    /// SOP 版本 ID（选项 TaktSopRevisions/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RevisionId { get; set; }

    /// <summary>
    /// 工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? WorkstationId { get; set; }

    /// <summary>
    /// 确认人 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AcknowledgedBy { get; set; }

    /// <summary>
    /// 确认时间
    /// </summary>
    public DateTime AcknowledgedAt { get; set; }

    /// <summary>
    /// 确认意见
    /// </summary>
    public string? AckComment { get; set; } = string.Empty;

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
// 更新SopAck DTO
// ========================================

/// <summary>
/// 更新SopAck DTO
/// 继承 TaktSopAckCreateDto，添加 SopAckId 字段
/// </summary>
public class TaktSopAckUpdateDto : TaktSopAckCreateDto
{
    /// <summary>
    /// SopAckID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopAckId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SopAck 导入模板行 DTO
/// </summary>
public class TaktSopAckTemplateDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// SOP 主档 ID（选项 TaktSopDocs/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SopId { get; set; }

    /// <summary>
    /// SOP 版本 ID（选项 TaktSopRevisions/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RevisionId { get; set; }

    /// <summary>
    /// 工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? WorkstationId { get; set; }

    /// <summary>
    /// 确认人 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AcknowledgedBy { get; set; }

    /// <summary>
    /// 确认时间
    /// </summary>
    public DateTime? AcknowledgedAt { get; set; }

    /// <summary>
    /// 确认意见
    /// </summary>
    public string? AckComment { get; set; } = string.Empty;

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
/// SopAck 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSopAckImportDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// SOP 主档 ID（选项 TaktSopDocs/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SopId { get; set; }

    /// <summary>
    /// SOP 版本 ID（选项 TaktSopRevisions/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RevisionId { get; set; }

    /// <summary>
    /// 工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? WorkstationId { get; set; }

    /// <summary>
    /// 确认人 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AcknowledgedBy { get; set; }

    /// <summary>
    /// 确认时间
    /// </summary>
    public DateTime? AcknowledgedAt { get; set; }

    /// <summary>
    /// 确认意见
    /// </summary>
    public string? AckComment { get; set; } = string.Empty;

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
/// SopAck 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSopAckExportDto
{
    /// <summary>
    /// SopAckID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopAckId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// SOP 主档 ID（选项 TaktSopDocs/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopId { get; set; }

    /// <summary>
    /// SOP 版本 ID（选项 TaktSopRevisions/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RevisionId { get; set; }

    /// <summary>
    /// 工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? WorkstationId { get; set; }

    /// <summary>
    /// 确认人 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AcknowledgedBy { get; set; }

    /// <summary>
    /// 确认时间
    /// </summary>
    public DateTime AcknowledgedAt { get; set; }

    /// <summary>
    /// 确认意见
    /// </summary>
    public string? AckComment { get; set; } = string.Empty;

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
