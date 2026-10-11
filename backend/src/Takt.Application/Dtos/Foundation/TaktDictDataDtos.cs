// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Foundation
// 文件名称：TaktDictDataDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：DictData 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktDictData 生成，请按需审阅）
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
// DictData 响应 DTO
// ========================================

/// <summary>
/// 字典数据实体 字典类型的具体数据项，如：订单状态下的“待支付”、“已完成”等 租户级实体：字典数据在租户内共享；CultureCode 默认 mul（IETF BCP 47 / ISO 639-3，多种语言内容，各 UI 语言均加载）；区域专用项用 zh-CN、ja-JP 等（与 TaktCulture.CultureCode 一致） GetDataDictAll 下发全量；服务/前端按「mul + Accept-Language」过滤；TaktTenantScopeFillHelper 不对 DictData 回填公司主档语言 组合 2：无关联工厂、有语言（TaktTenantCultureEntityBase）
/// 对应前端 TaktDictDataDto
/// 继承 TaktTenantCultureDtoBase
/// </summary>
public class TaktDictDataDto : TaktTenantCultureDtoBase
{
    /// <summary>
    /// DictDataID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DictDataId { get; set; }

    /// <summary>
    /// 字典类型（选项 TaktDictTypes/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DictTypeId { get; set; }

    /// <summary>
    /// 字典类型（选项 TaktDictTypes/options；DictValue=Id）
    /// </summary>
    public string? DictTypeName { get; set; }

    /// <summary>
    /// 字典类型编码（冗余，与 TaktDictType.DictTypeCode 对齐；varchar Length=140）
    /// </summary>
    public string DictTypeCode { get; set; } = string.Empty;

    /// <summary>
    /// 字典项标签（唯一索引：租户内 DictTypeId+CultureCode+DictLabel+I18nKey 唯一；sys_culture_code 等区域文化项用本族语，同语言多地区才加括号，如 English (US)、中文 (简体)）
    /// </summary>
    public string DictLabel { get; set; } = string.Empty;

    /// <summary>
    /// 字典项值（实际存储值，如：0, 1, 2）
    /// </summary>
    public string DictValue { get; set; } = string.Empty;

    /// <summary>
    /// 国际化键（与 DictTypeCode 段对应，如 dict.accounting.controlling.cost.center.type.f）
    /// </summary>
    public string I18nKey { get; set; } = string.Empty;

    /// <summary>
    /// 扩展标签（用于存储额外的显示文本，如：副标题、简短描述等）
    /// </summary>
    public string? ExtLabel { get; set; } = string.Empty;

    /// <summary>
    /// 扩展值（用于存储额外的业务数据，如：编码、标识符等）
    /// </summary>
    public string? ExtValue { get; set; } = string.Empty;

    /// <summary>
    /// 列表样式类（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于下拉列表选项中显示的颜色标识
    /// </summary>
    public int ListClass { get; set; } = 0;

    /// <summary>
    /// CSS 类名（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于数据表格中字典值显示的颜色标签
    /// </summary>
    public int CssClass { get; set; } = 0;

    /// <summary>
    /// 是否默认项（1=是，0=否）
    /// </summary>
    public int IsDefault { get; set; } = 0;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 字典类型（多对一关联）
    /// （主表：TaktDictType）
    /// </summary>
    public TaktDictTypeDto? DictType { get; set; }

}

// ========================================
// DictData 查询 DTO
// ========================================

/// <summary>
/// DictData 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktDictDataQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 字典类型（选项 TaktDictTypes/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DictTypeId { get; set; }

    /// <summary>
    /// 字典类型编码（冗余，与 TaktDictType.DictTypeCode 对齐；varchar Length=140）
    /// </summary>
    public string? DictTypeCode { get; set; } = string.Empty;

    /// <summary>
    /// 字典项标签（唯一索引：租户内 DictTypeId+CultureCode+DictLabel+I18nKey 唯一；sys_culture_code 等区域文化项用本族语，同语言多地区才加括号，如 English (US)、中文 (简体)）
    /// </summary>
    public string? DictLabel { get; set; } = string.Empty;

    /// <summary>
    /// 字典项值（实际存储值，如：0, 1, 2）
    /// </summary>
    public string? DictValue { get; set; } = string.Empty;

    /// <summary>
    /// 国际化键（与 DictTypeCode 段对应，如 dict.accounting.controlling.cost.center.type.f）
    /// </summary>
    public string? I18nKey { get; set; } = string.Empty;

    /// <summary>
    /// 扩展标签（用于存储额外的显示文本，如：副标题、简短描述等）
    /// </summary>
    public string? ExtLabel { get; set; } = string.Empty;

    /// <summary>
    /// 扩展值（用于存储额外的业务数据，如：编码、标识符等）
    /// </summary>
    public string? ExtValue { get; set; } = string.Empty;

    /// <summary>
    /// 列表样式类（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于下拉列表选项中显示的颜色标识
    /// </summary>
    public int? ListClass { get; set; }

    /// <summary>
    /// CSS 类名（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于数据表格中字典值显示的颜色标签
    /// </summary>
    public int? CssClass { get; set; }

    /// <summary>
    /// 是否默认项（1=是，0=否）
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
// 创建DictData DTO
// ========================================

/// <summary>
/// 创建DictData DTO
/// </summary>
public class TaktDictDataCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 字典类型（选项 TaktDictTypes/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DictTypeId { get; set; }

    /// <summary>
    /// 字典类型编码（冗余，与 TaktDictType.DictTypeCode 对齐；varchar Length=140）
    /// </summary>
    public string DictTypeCode { get; set; } = string.Empty;

    /// <summary>
    /// 字典项标签（唯一索引：租户内 DictTypeId+CultureCode+DictLabel+I18nKey 唯一；sys_culture_code 等区域文化项用本族语，同语言多地区才加括号，如 English (US)、中文 (简体)）
    /// </summary>
    [Required(ErrorMessage = "字典项标签（唯一索引：租户内 DictTypeId+CultureCode+DictLabel+I18nKey 唯一；sys_culture_code 等区域文化项用本族语，同语言多地区才加括号，如 English (US)、中文 (简体)）不能为空")]
    public string DictLabel { get; set; } = string.Empty;

    /// <summary>
    /// 字典项值（实际存储值，如：0, 1, 2）
    /// </summary>
    [Required(ErrorMessage = "字典项值（实际存储值，如：0, 1, 2）不能为空")]
    public string DictValue { get; set; } = string.Empty;

    /// <summary>
    /// 国际化键（与 DictTypeCode 段对应，如 dict.accounting.controlling.cost.center.type.f）
    /// </summary>
    [Required(ErrorMessage = "国际化键不能为空")]
    public string I18nKey { get; set; } = string.Empty;

    /// <summary>
    /// 扩展标签（用于存储额外的显示文本，如：副标题、简短描述等）
    /// </summary>
    public string? ExtLabel { get; set; } = string.Empty;

    /// <summary>
    /// 扩展值（用于存储额外的业务数据，如：编码、标识符等）
    /// </summary>
    public string? ExtValue { get; set; } = string.Empty;

    /// <summary>
    /// 列表样式类（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于下拉列表选项中显示的颜色标识
    /// </summary>
    public int ListClass { get; set; } = 0;

    /// <summary>
    /// CSS 类名（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于数据表格中字典值显示的颜色标签
    /// </summary>
    public int CssClass { get; set; } = 0;

    /// <summary>
    /// 是否默认项（1=是，0=否）
    /// </summary>
    public int IsDefault { get; set; } = 0;

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
// 更新DictData DTO
// ========================================

/// <summary>
/// 更新DictData DTO
/// 继承 TaktDictDataCreateDto，添加 DictDataId 字段
/// </summary>
public class TaktDictDataUpdateDto : TaktDictDataCreateDto
{
    /// <summary>
    /// DictDataID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DictDataId { get; set; }

}

// ========================================
// DictData 排序 DTO
// ========================================

/// <summary>
/// DictData 排序更新 DTO
/// </summary>
public class TaktDictDataSortDto
{
    /// <summary>
    /// DictDataID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DictDataId { get; set; }

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
/// DictData 导入模板行 DTO
/// </summary>
public class TaktDictDataTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 字典类型（选项 TaktDictTypes/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DictTypeId { get; set; }

    /// <summary>
    /// 字典类型编码（冗余，与 TaktDictType.DictTypeCode 对齐；varchar Length=140）
    /// </summary>
    public string? DictTypeCode { get; set; } = string.Empty;

    /// <summary>
    /// 字典项标签（唯一索引：租户内 DictTypeId+CultureCode+DictLabel+I18nKey 唯一；sys_culture_code 等区域文化项用本族语，同语言多地区才加括号，如 English (US)、中文 (简体)）
    /// </summary>
    public string? DictLabel { get; set; } = string.Empty;

    /// <summary>
    /// 字典项值（实际存储值，如：0, 1, 2）
    /// </summary>
    public string? DictValue { get; set; } = string.Empty;

    /// <summary>
    /// 国际化键（与 DictTypeCode 段对应，如 dict.accounting.controlling.cost.center.type.f）
    /// </summary>
    public string? I18nKey { get; set; } = string.Empty;

    /// <summary>
    /// 扩展标签（用于存储额外的显示文本，如：副标题、简短描述等）
    /// </summary>
    public string? ExtLabel { get; set; } = string.Empty;

    /// <summary>
    /// 扩展值（用于存储额外的业务数据，如：编码、标识符等）
    /// </summary>
    public string? ExtValue { get; set; } = string.Empty;

    /// <summary>
    /// 列表样式类（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于下拉列表选项中显示的颜色标识
    /// </summary>
    public int? ListClass { get; set; }

    /// <summary>
    /// CSS 类名（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于数据表格中字典值显示的颜色标签
    /// </summary>
    public int? CssClass { get; set; }

    /// <summary>
    /// 是否默认项（1=是，0=否）
    /// </summary>
    public int? IsDefault { get; set; }

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
/// DictData 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktDictDataImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 字典类型（选项 TaktDictTypes/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DictTypeId { get; set; }

    /// <summary>
    /// 字典类型编码（冗余，与 TaktDictType.DictTypeCode 对齐；varchar Length=140）
    /// </summary>
    public string? DictTypeCode { get; set; } = string.Empty;

    /// <summary>
    /// 字典项标签（唯一索引：租户内 DictTypeId+CultureCode+DictLabel+I18nKey 唯一；sys_culture_code 等区域文化项用本族语，同语言多地区才加括号，如 English (US)、中文 (简体)）
    /// </summary>
    public string? DictLabel { get; set; } = string.Empty;

    /// <summary>
    /// 字典项值（实际存储值，如：0, 1, 2）
    /// </summary>
    public string? DictValue { get; set; } = string.Empty;

    /// <summary>
    /// 国际化键（与 DictTypeCode 段对应，如 dict.accounting.controlling.cost.center.type.f）
    /// </summary>
    public string? I18nKey { get; set; } = string.Empty;

    /// <summary>
    /// 扩展标签（用于存储额外的显示文本，如：副标题、简短描述等）
    /// </summary>
    public string? ExtLabel { get; set; } = string.Empty;

    /// <summary>
    /// 扩展值（用于存储额外的业务数据，如：编码、标识符等）
    /// </summary>
    public string? ExtValue { get; set; } = string.Empty;

    /// <summary>
    /// 列表样式类（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于下拉列表选项中显示的颜色标识
    /// </summary>
    public int? ListClass { get; set; }

    /// <summary>
    /// CSS 类名（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于数据表格中字典值显示的颜色标签
    /// </summary>
    public int? CssClass { get; set; }

    /// <summary>
    /// 是否默认项（1=是，0=否）
    /// </summary>
    public int? IsDefault { get; set; }

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
/// DictData 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktDictDataExportDto
{
    /// <summary>
    /// DictDataID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DictDataId { get; set; }

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 字典类型（选项 TaktDictTypes/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DictTypeId { get; set; }

    /// <summary>
    /// 字典类型编码（冗余，与 TaktDictType.DictTypeCode 对齐；varchar Length=140）
    /// </summary>
    public string DictTypeCode { get; set; } = string.Empty;

    /// <summary>
    /// 字典项标签（唯一索引：租户内 DictTypeId+CultureCode+DictLabel+I18nKey 唯一；sys_culture_code 等区域文化项用本族语，同语言多地区才加括号，如 English (US)、中文 (简体)）
    /// </summary>
    public string DictLabel { get; set; } = string.Empty;

    /// <summary>
    /// 字典项值（实际存储值，如：0, 1, 2）
    /// </summary>
    public string DictValue { get; set; } = string.Empty;

    /// <summary>
    /// 国际化键（与 DictTypeCode 段对应，如 dict.accounting.controlling.cost.center.type.f）
    /// </summary>
    public string I18nKey { get; set; } = string.Empty;

    /// <summary>
    /// 扩展标签（用于存储额外的显示文本，如：副标题、简短描述等）
    /// </summary>
    public string? ExtLabel { get; set; } = string.Empty;

    /// <summary>
    /// 扩展值（用于存储额外的业务数据，如：编码、标识符等）
    /// </summary>
    public string? ExtValue { get; set; } = string.Empty;

    /// <summary>
    /// 列表样式类（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于下拉列表选项中显示的颜色标识
    /// </summary>
    public int ListClass { get; set; } = 0;

    /// <summary>
    /// CSS 类名（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于数据表格中字典值显示的颜色标签
    /// </summary>
    public int CssClass { get; set; } = 0;

    /// <summary>
    /// 是否默认项（1=是，0=否）
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

// <takt:hand-maintained-begin>
// ========================================
// 公司区域下全量字典（登录/下拉缓存）
// ========================================

/// <summary>
/// 当前登录 UI 语言下全部字典数据响应 DTO（CultureCode mul=多种语言内容 + 匹配 Accept-Language；含 DictTypeCode 供前端分组）
/// 对应前端 DataDictAll；Items 为扁平列表，按 DictTypeCode 供前端分组
/// </summary>
public class TaktDataDictAllDto
{
    /// <summary>
    /// 字典项列表（已按 DictTypeCode、SortOrder 排序）
    /// </summary>
    public List<TaktSelectOption> Items { get; set; } = new();
}
// <takt:hand-maintained-end>
