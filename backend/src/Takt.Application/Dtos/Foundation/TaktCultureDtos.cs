// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Foundation
// 文件名称：TaktCultureDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：Culture 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktCulture 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Foundation;

// ========================================
// Culture 响应 DTO
// ========================================

/// <summary>
/// 区域文化实体 定义系统支持的多语言区域文化，如：zh-CN（简体中文）、en-US（美式英文）、ja-JP（日文）等 租户级实体：区域文化定义在租户内共享，不需要公司隔离 特例：继承组合 4：无关联工厂、无语言（TaktTenantCoreEntityBase）
/// 对应前端 TaktCultureDto
/// 继承 TaktTenantCoreDtoBase
/// </summary>
public class TaktCultureDto : TaktTenantCoreDtoBase
{
    /// <summary>
    /// CultureID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CultureId { get; set; }

    /// <summary>
    /// 本地化名称（用该语言显示的自身名称，如：中文、English）
    /// </summary>
    public string NativeName { get; set; } = string.Empty;

    /// <summary>
    /// 语言图标（flag-icons：fi-cn / fi-us / fi-jp，前端解析为 fi fi-xx）
    /// </summary>
    public string? Icon { get; set; } = string.Empty;

    /// <summary>
    /// 默认语言（字典 sys_yes_no；1=是 0=否）
    /// </summary>
    public int IsDefault { get; set; } = 0;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 翻译列表（一对多关联）
    /// （子表：TaktTranslation）
    /// </summary>
    public List<TaktTranslationDto>? TranslationList { get; set; }

}

// ========================================
// Culture 查询 DTO
// ========================================

/// <summary>
/// Culture 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktCultureQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 本地化名称（用该语言显示的自身名称，如：中文、English）
    /// </summary>
    public string? NativeName { get; set; } = string.Empty;

    /// <summary>
    /// 语言图标（flag-icons：fi-cn / fi-us / fi-jp，前端解析为 fi fi-xx）
    /// </summary>
    public string? Icon { get; set; } = string.Empty;

    /// <summary>
    /// 默认语言（字典 sys_yes_no；1=是 0=否）
    /// </summary>
    public int? IsDefault { get; set; }

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
// 创建Culture DTO
// ========================================

/// <summary>
/// 创建Culture DTO
/// </summary>
public class TaktCultureCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 本地化名称（用该语言显示的自身名称，如：中文、English）
    /// </summary>
    [Required(ErrorMessage = "本地化名称（用该语言显示的自身名称，如：中文、English）不能为空")]
    public string NativeName { get; set; } = string.Empty;

    /// <summary>
    /// 语言图标（flag-icons：fi-cn / fi-us / fi-jp，前端解析为 fi fi-xx）
    /// </summary>
    public string? Icon { get; set; } = string.Empty;

    /// <summary>
    /// 默认语言（字典 sys_yes_no；1=是 0=否）
    /// </summary>
    public int IsDefault { get; set; } = 0;

    /// <summary>
    /// 翻译列表（一对多关联）（子表，级联保存）
    /// </summary>
    public List<TaktTranslationCreateDto>? TranslationList { get; set; }

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
// 更新Culture DTO
// ========================================

/// <summary>
/// 更新Culture DTO
/// 继承 TaktCultureCreateDto，添加 CultureId 字段
/// </summary>
public class TaktCultureUpdateDto : TaktCultureCreateDto
{
    /// <summary>
    /// CultureID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CultureId { get; set; }

    /// <summary>
    /// 翻译列表（一对多关联）（子表，级联保存）
    /// </summary>
    public new List<TaktTranslationUpdateDto>? TranslationList { get; set; }

}

// ========================================
// Culture 排序 DTO
// ========================================

/// <summary>
/// Culture 排序更新 DTO
/// </summary>
public class TaktCultureSortDto
{
    /// <summary>
    /// CultureID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CultureId { get; set; }

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [Required(ErrorMessage = "排序号（回填）不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Culture 导入模板行 DTO
/// </summary>
public class TaktCultureTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 本地化名称（用该语言显示的自身名称，如：中文、English）
    /// </summary>
    public string? NativeName { get; set; } = string.Empty;

    /// <summary>
    /// 语言图标（flag-icons：fi-cn / fi-us / fi-jp，前端解析为 fi fi-xx）
    /// </summary>
    public string? Icon { get; set; } = string.Empty;

    /// <summary>
    /// 默认语言（字典 sys_yes_no；1=是 0=否）
    /// </summary>
    public int? IsDefault { get; set; }

    /// <summary>
    /// 翻译列表（一对多关联）（子表，级联保存）
    /// </summary>
    public List<TaktTranslationCreateDto>? TranslationList { get; set; }

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
/// Culture 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktCultureImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 本地化名称（用该语言显示的自身名称，如：中文、English）
    /// </summary>
    public string? NativeName { get; set; } = string.Empty;

    /// <summary>
    /// 语言图标（flag-icons：fi-cn / fi-us / fi-jp，前端解析为 fi fi-xx）
    /// </summary>
    public string? Icon { get; set; } = string.Empty;

    /// <summary>
    /// 默认语言（字典 sys_yes_no；1=是 0=否）
    /// </summary>
    public int? IsDefault { get; set; }

    /// <summary>
    /// 翻译列表（一对多关联）（子表，级联保存）
    /// </summary>
    public List<TaktTranslationCreateDto>? TranslationList { get; set; }

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
/// Culture 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktCultureExportDto
{
    /// <summary>
    /// CultureID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CultureId { get; set; }

    /// <summary>
    /// 本地化名称（用该语言显示的自身名称，如：中文、English）
    /// </summary>
    public string NativeName { get; set; } = string.Empty;

    /// <summary>
    /// 语言图标（flag-icons：fi-cn / fi-us / fi-jp，前端解析为 fi fi-xx）
    /// </summary>
    public string? Icon { get; set; } = string.Empty;

    /// <summary>
    /// 默认语言（字典 sys_yes_no；1=是 0=否）
    /// </summary>
    public int IsDefault { get; set; } = 0;

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
