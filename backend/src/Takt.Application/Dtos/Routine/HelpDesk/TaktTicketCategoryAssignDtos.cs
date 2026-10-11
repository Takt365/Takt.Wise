// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Routine.HelpDesk
// 文件名称：TaktTicketCategoryAssignDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TicketCategoryAssign 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktTicketCategoryAssign 生成，请按需审阅）
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
// TicketCategoryAssign 响应 DTO
// ========================================

/// <summary>
/// 工单分类默认处理人（按 CategoryCode 自动分配处理人）
/// 对应前端 TaktTicketCategoryAssignDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktTicketCategoryAssignDto : TaktCompanyDtoBase
{
    /// <summary>
    /// TicketCategoryAssignID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TicketCategoryAssignId { get; set; }

    /// <summary>
    /// 分类编码（与 TaktTicket.CategoryCode 对应）
    /// </summary>
    public string CategoryCode { get; set; } = string.Empty;

    /// <summary>
    /// 默认处理人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssigneeId { get; set; }

    /// <summary>
    /// 默认处理人姓名
    /// </summary>
    public string? AssigneeName { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

}

// ========================================
// TicketCategoryAssign 查询 DTO
// ========================================

/// <summary>
/// TicketCategoryAssign 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktTicketCategoryAssignQueryDto : TaktPagedQuery
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
    /// 分类编码（与 TaktTicket.CategoryCode 对应）
    /// </summary>
    public string? CategoryCode { get; set; } = string.Empty;

    /// <summary>
    /// 默认处理人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssigneeId { get; set; }

    /// <summary>
    /// 默认处理人姓名
    /// </summary>
    public string? AssigneeName { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int? SortOrder { get; set; }

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
// 创建TicketCategoryAssign DTO
// ========================================

/// <summary>
/// 创建TicketCategoryAssign DTO
/// </summary>
public class TaktTicketCategoryAssignCreateDto
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
    /// 分类编码（与 TaktTicket.CategoryCode 对应）
    /// </summary>
    [Required(ErrorMessage = "分类编码（与 TaktTicket.CategoryCode 对应）不能为空")]
    public string CategoryCode { get; set; } = string.Empty;

    /// <summary>
    /// 默认处理人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssigneeId { get; set; }

    /// <summary>
    /// 默认处理人姓名
    /// </summary>
    public string? AssigneeName { get; set; } = string.Empty;

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
// 更新TicketCategoryAssign DTO
// ========================================

/// <summary>
/// 更新TicketCategoryAssign DTO
/// 继承 TaktTicketCategoryAssignCreateDto，添加 TicketCategoryAssignId 字段
/// </summary>
public class TaktTicketCategoryAssignUpdateDto : TaktTicketCategoryAssignCreateDto
{
    /// <summary>
    /// TicketCategoryAssignID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TicketCategoryAssignId { get; set; }

}

// ========================================
// TicketCategoryAssign 排序 DTO
// ========================================

/// <summary>
/// TicketCategoryAssign 排序更新 DTO
/// </summary>
public class TaktTicketCategoryAssignSortDto
{
    /// <summary>
    /// TicketCategoryAssignID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TicketCategoryAssignId { get; set; }

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [Required(ErrorMessage = "排序号不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// TicketCategoryAssign 导入模板行 DTO
/// </summary>
public class TaktTicketCategoryAssignTemplateDto
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
    /// 分类编码（与 TaktTicket.CategoryCode 对应）
    /// </summary>
    public string? CategoryCode { get; set; } = string.Empty;

    /// <summary>
    /// 默认处理人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssigneeId { get; set; }

    /// <summary>
    /// 默认处理人姓名
    /// </summary>
    public string? AssigneeName { get; set; } = string.Empty;

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
/// TicketCategoryAssign 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktTicketCategoryAssignImportDto
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
    /// 分类编码（与 TaktTicket.CategoryCode 对应）
    /// </summary>
    public string? CategoryCode { get; set; } = string.Empty;

    /// <summary>
    /// 默认处理人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssigneeId { get; set; }

    /// <summary>
    /// 默认处理人姓名
    /// </summary>
    public string? AssigneeName { get; set; } = string.Empty;

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
/// TicketCategoryAssign 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktTicketCategoryAssignExportDto
{
    /// <summary>
    /// TicketCategoryAssignID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TicketCategoryAssignId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 分类编码（与 TaktTicket.CategoryCode 对应）
    /// </summary>
    public string CategoryCode { get; set; } = string.Empty;

    /// <summary>
    /// 默认处理人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssigneeId { get; set; }

    /// <summary>
    /// 默认处理人姓名
    /// </summary>
    public string? AssigneeName { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

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
