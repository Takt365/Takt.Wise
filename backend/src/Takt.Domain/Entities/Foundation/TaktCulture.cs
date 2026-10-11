// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Foundation
// 文件名称：TaktCulture.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：区域文化实体，定义系统支持的多语言区域文化
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Foundation;

/// <summary>
/// 区域文化实体
/// 定义系统支持的多语言区域文化，如：zh-CN（简体中文）、en-US（美式英文）、ja-JP（日文）等
/// 租户级实体：区域文化定义在租户内共享，不需要公司隔离
/// 特例：继承组合 4：无关联工厂、无语言（TaktTenantCoreEntityBase）
/// </summary>
[SugarTable("takt_foundation_culture", "区域表")]
[SugarIndex("ix_culture_tenant", nameof(TenantCode), OrderByType.Asc, false)]
[SugarIndex("ix_culture_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_culture_culture_unique", nameof(TenantCode), OrderByType.Asc, nameof(CultureCode), OrderByType.Asc, true)]
public class TaktCulture : TaktTenantCoreEntityBase
{
    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    [SugarColumn(ColumnName = "culture_code", ColumnDescription = "区域文化", ColumnDataType = "varchar", Length = 5, IsNullable = false, DefaultValue = "mul")]
    public string CultureCode { get; set; } = "mul";

    /// <summary>
    /// 本地化名称（用该语言显示的自身名称，如：中文、English；nvarchar Length=40）
    /// </summary>
    [SugarColumn(ColumnName = "native_name", ColumnDescription = "本地化名称", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string NativeName { get; set; } = string.Empty;

    /// <summary>
    /// 语言图标（flag-icons：fi-cn / fi-us / fi-jp，前端解析为 fi fi-xx）
    /// </summary>
    [SugarColumn(ColumnName = "icon", ColumnDescription = "语言图标", ColumnDataType = "varchar", Length = 50, IsNullable = true)]
    public string? Icon { get; set; }

    /// <summary>
    /// 默认语言（字典 sys_yes_no；1=是 0=否）
    /// </summary>
    [SugarColumn(ColumnName = "is_default", ColumnDescription = "默认语言", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsDefault { get; set; } = 0;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 翻译列表（一对多关联）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktTranslation.CultureCode))]
    public List<TaktTranslation>? TranslationList { get; set; }
}
