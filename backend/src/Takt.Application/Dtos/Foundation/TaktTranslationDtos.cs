// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Foundation
// 文件名称：TaktTranslationDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：Translation 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktTranslation 生成，请按需审阅）
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
// Translation 响应 DTO
// ========================================

/// <summary>
/// 翻译实体 存储系统界面的多语言翻译文本 租户级实体：翻译数据在租户内共享，不需要公司隔离 特例：继承组合 4：无关联工厂、无语言（TaktTenantCoreEntityBase）
/// 对应前端 TaktTranslationDto
/// 继承 TaktTenantCoreDtoBase
/// </summary>
public class TaktTranslationDto : TaktTenantCoreDtoBase
{
    /// <summary>
    /// TranslationID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TranslationId { get; set; }

    /// <summary>
    /// 区域文化（选项 TaktCultures/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CultureId { get; set; }

    /// <summary>
    /// 区域文化（选项 TaktCultures/options；DictValue=Id）
    /// </summary>
    public string? CultureName { get; set; }

    /// <summary>
    /// 翻译键（唯一索引：租户内键+文化唯一，见 ix_translation_key_culture_unique；如 common.confirm）
    /// </summary>
    public string I18nKey { get; set; } = string.Empty;

    /// <summary>
    /// 翻译文本（该语言下的显示文本）
    /// </summary>
    public string TranslationText { get; set; } = string.Empty;

    /// <summary>
    /// 资源分组（选项 TaktMenus/tree-options,DictValue=Id）
    /// </summary>
    public string ResourceGroup { get; set; } = string.Empty;

    /// <summary>
    /// 资源类别（字典 sys_resource_type；frontend=前端 backend=后端）
    /// </summary>
    public string ResourceType { get; set; } = string.Empty;

    /// <summary>
    /// 上下文注释（帮助翻译人员理解使用场景）
    /// </summary>
    public string? ContextNote { get; set; } = string.Empty;

    /// <summary>
    /// 文化（多对一关联）
    /// （主表：TaktCulture）
    /// </summary>
    public TaktCultureDto? Culture { get; set; }

}

// ========================================
// Translation 查询 DTO
// ========================================

/// <summary>
/// Translation 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktTranslationQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化（选项 TaktCultures/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CultureId { get; set; }

    /// <summary>
    /// 翻译键（唯一索引：租户内键+文化唯一，见 ix_translation_key_culture_unique；如 common.confirm）
    /// </summary>
    public string? I18nKey { get; set; } = string.Empty;

    /// <summary>
    /// 翻译文本（该语言下的显示文本）
    /// </summary>
    public string? TranslationText { get; set; } = string.Empty;

    /// <summary>
    /// 资源分组（选项 TaktMenus/tree-options,DictValue=Id）
    /// </summary>
    public string? ResourceGroup { get; set; } = string.Empty;

    /// <summary>
    /// 资源类别（字典 sys_resource_type；frontend=前端 backend=后端）
    /// </summary>
    public string? ResourceType { get; set; } = string.Empty;

    /// <summary>
    /// 上下文注释（帮助翻译人员理解使用场景）
    /// </summary>
    public string? ContextNote { get; set; } = string.Empty;

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
// 创建Translation DTO
// ========================================

/// <summary>
/// 创建Translation DTO
/// </summary>
public class TaktTranslationCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化（选项 TaktCultures/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CultureId { get; set; }

    /// <summary>
    /// 翻译键（唯一索引：租户内键+文化唯一，见 ix_translation_key_culture_unique；如 common.confirm）
    /// </summary>
    [Required(ErrorMessage = "翻译键（唯一索引：租户内键+文化唯一，见 ix_translation_key_culture_unique；如 common.confirm）不能为空")]
    public string I18nKey { get; set; } = string.Empty;

    /// <summary>
    /// 翻译文本（该语言下的显示文本）
    /// </summary>
    [Required(ErrorMessage = "翻译文本（该语言下的显示文本）不能为空")]
    public string TranslationText { get; set; } = string.Empty;

    /// <summary>
    /// 资源分组（选项 TaktMenus/tree-options,DictValue=Id）
    /// </summary>
    [Required(ErrorMessage = "资源分组（选项 TaktMenus/tree-options,DictValue=Id）不能为空")]
    public string ResourceGroup { get; set; } = string.Empty;

    /// <summary>
    /// 资源类别（字典 sys_resource_type；frontend=前端 backend=后端）
    /// </summary>
    [Required(ErrorMessage = "资源类别（字典 sys_resource_type；frontend=前端 backend=后端）不能为空")]
    public string ResourceType { get; set; } = string.Empty;

    /// <summary>
    /// 上下文注释（帮助翻译人员理解使用场景）
    /// </summary>
    public string? ContextNote { get; set; } = string.Empty;

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
// 更新Translation DTO
// ========================================

/// <summary>
/// 更新Translation DTO
/// 继承 TaktTranslationCreateDto，添加 TranslationId 字段
/// </summary>
public class TaktTranslationUpdateDto : TaktTranslationCreateDto
{
    /// <summary>
    /// TranslationID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TranslationId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Translation 导入模板行 DTO
/// </summary>
public class TaktTranslationTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化（选项 TaktCultures/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CultureId { get; set; }

    /// <summary>
    /// 翻译键（唯一索引：租户内键+文化唯一，见 ix_translation_key_culture_unique；如 common.confirm）
    /// </summary>
    public string? I18nKey { get; set; } = string.Empty;

    /// <summary>
    /// 翻译文本（该语言下的显示文本）
    /// </summary>
    public string? TranslationText { get; set; } = string.Empty;

    /// <summary>
    /// 资源分组（选项 TaktMenus/tree-options,DictValue=Id）
    /// </summary>
    public string? ResourceGroup { get; set; } = string.Empty;

    /// <summary>
    /// 资源类别（字典 sys_resource_type；frontend=前端 backend=后端）
    /// </summary>
    public string? ResourceType { get; set; } = string.Empty;

    /// <summary>
    /// 上下文注释（帮助翻译人员理解使用场景）
    /// </summary>
    public string? ContextNote { get; set; } = string.Empty;

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
/// Translation 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktTranslationImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化（选项 TaktCultures/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CultureId { get; set; }

    /// <summary>
    /// 翻译键（唯一索引：租户内键+文化唯一，见 ix_translation_key_culture_unique；如 common.confirm）
    /// </summary>
    public string? I18nKey { get; set; } = string.Empty;

    /// <summary>
    /// 翻译文本（该语言下的显示文本）
    /// </summary>
    public string? TranslationText { get; set; } = string.Empty;

    /// <summary>
    /// 资源分组（选项 TaktMenus/tree-options,DictValue=Id）
    /// </summary>
    public string? ResourceGroup { get; set; } = string.Empty;

    /// <summary>
    /// 资源类别（字典 sys_resource_type；frontend=前端 backend=后端）
    /// </summary>
    public string? ResourceType { get; set; } = string.Empty;

    /// <summary>
    /// 上下文注释（帮助翻译人员理解使用场景）
    /// </summary>
    public string? ContextNote { get; set; } = string.Empty;

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
/// Translation 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktTranslationExportDto
{
    /// <summary>
    /// TranslationID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TranslationId { get; set; }

    /// <summary>
    /// 区域文化（选项 TaktCultures/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CultureId { get; set; }

    /// <summary>
    /// 翻译键（唯一索引：租户内键+文化唯一，见 ix_translation_key_culture_unique；如 common.confirm）
    /// </summary>
    public string I18nKey { get; set; } = string.Empty;

    /// <summary>
    /// 翻译文本（该语言下的显示文本）
    /// </summary>
    public string TranslationText { get; set; } = string.Empty;

    /// <summary>
    /// 资源分组（选项 TaktMenus/tree-options,DictValue=Id）
    /// </summary>
    public string ResourceGroup { get; set; } = string.Empty;

    /// <summary>
    /// 资源类别（字典 sys_resource_type；frontend=前端 backend=后端）
    /// </summary>
    public string ResourceType { get; set; } = string.Empty;

    /// <summary>
    /// 上下文注释（帮助翻译人员理解使用场景）
    /// </summary>
    public string? ContextNote { get; set; } = string.Empty;

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

// ========================================
// Translation 转置 DTO（多语言表格：行=业务键，列=各语言文本）
// ========================================

/// <summary>
/// Translation转置行 DTO
/// </summary>
public class TaktTranslationTransposedDto
{
    /// <summary>
    /// 翻译ID（分组内首条记录 Id，新建为 0）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TranslationId { get; set; }

    /// <summary>
    /// 翻译键（转置行键）
    /// </summary>
    public required string I18nKey { get; set; } = string.Empty;

    /// <summary>
    /// 资源分组（关联 TaktMenu.Id）
    /// </summary>
    public string ResourceGroup { get; set; } = string.Empty;

    /// <summary>
    /// 资源类别（字典 sys_resource_type）
    /// </summary>
    public string ResourceType { get; set; } = string.Empty;

    /// <summary>
    /// 上下文注释
    /// </summary>
    public string? ContextNote { get; set; }

    /// <summary>
    /// 各语言文本；键为 CultureCode（如 zh-CN、en-US），值对应该语言下的显示文本
    /// </summary>
    public Dictionary<string, string> Translations { get; set; } = new();
}

/// <summary>
/// Translation转置分页查询 DTO
/// </summary>
public class TaktTranslationTransposedQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 文化ID
    /// </summary>
    public long? CultureId { get; set; }

    /// <summary>
    /// 文化编码
    /// </summary>
    public string? CultureCode { get; set; }

    /// <summary>
    /// 翻译键
    /// </summary>
    public string? I18nKey { get; set; }

    /// <summary>
    /// 翻译文本
    /// </summary>
    public string? TranslationText { get; set; }

    /// <summary>
    /// 资源分组
    /// </summary>
    public string? ResourceGroup { get; set; }

    /// <summary>
    /// 资源类别
    /// </summary>
    public string? ResourceType { get; set; }

    /// <summary>
    /// 上下文注释
    /// </summary>
    public string? ContextNote { get; set; }

}

/// <summary>
/// Translation转置分页结果 DTO（含语言列顺序）
/// </summary>
public class TaktTranslationTransposedResultDto
{
    /// <summary>
    /// 分页数据
    /// </summary>
    public TaktPagedResult<TaktTranslationTransposedDto> Paged { get; set; } = null!;

    /// <summary>
    /// 语言列顺序（表头从左到右），如 zh-CN、en-US 等
    /// </summary>
    public IReadOnlyList<string> CultureCodeOrder { get; set; } = Array.Empty<string>();
}

/// <summary>
/// Translation转置批量保存 DTO
/// </summary>
public class TaktTranslationTransposedBatchDto
{
    /// <summary>
    /// 转置行数据
    /// </summary>
    public List<TaktTranslationTransposedDto> Rows { get; set; } = new();
}

// <takt:hand-maintained-begin>
// ========================================
// 前端 i18n 消息包
// ========================================

/// <summary>
/// 指定语言的前端翻译消息包 DTO
/// </summary>
public class TaktTranslationMessagesDto
{
    /// <summary>
    /// 区域文化编码（BCP47，如 zh-CN、en-US）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 扁平翻译：I18nKey → TranslationText（仅 resource_type=frontend）
    /// </summary>
    public Dictionary<string, string> Messages { get; set; } = new();
}
// <takt:hand-maintained-end>
