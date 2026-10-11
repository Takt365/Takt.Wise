// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Foundation
// 文件名称：TaktTranslation.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：翻译实体，存储多语言翻译文本
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Foundation;

/// <summary>
/// 翻译实体
/// 存储系统界面的多语言翻译文本
/// 租户级实体：翻译数据在租户内共享，不需要公司隔离
/// 特例：继承组合 4：无关联工厂、无语言（TaktTenantCoreEntityBase）
/// </summary>
[SugarTable("takt_foundation_translation", "翻译表")]
[SugarIndex("ix_translation_tenant", nameof(TenantCode), OrderByType.Asc, false)]
[SugarIndex("ix_translation_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_translation_key_culture_unique", nameof(TenantCode), OrderByType.Asc, nameof(I18nKey), OrderByType.Asc, nameof(CultureCode), OrderByType.Asc, true)]
public class TaktTranslation : TaktTenantCoreEntityBase
{
    /// <summary>
    /// 区域文化（选项 TaktCultures/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "culture_id", ColumnDescription = "文化ID", ColumnDataType = "bigint", IsNullable = false)]
    public long CultureId { get; set; }

    /// <summary>
    /// 区域文化编码（选项 TaktCultures/options，DictValue=CultureCode）
    /// </summary>
    [SugarColumn(ColumnName = "culture_code", ColumnDescription = "区域文化", ColumnDataType = "varchar", Length = 5, IsNullable = false, DefaultValue = "mul")]
    public string CultureCode { get; set; } = "mul";

    /// <summary>
    /// 翻译键（唯一索引：租户内键+文化唯一，见 ix_translation_key_culture_unique；varchar Length=140；如 common.confirm）
    /// </summary>
    [SugarColumn(ColumnName = "i18n_key", ColumnDescription = "翻译键", ColumnDataType = "varchar", Length = 140, IsNullable = false, DefaultValue = "")]
    public string I18nKey { get; set; } = string.Empty;

    /// <summary>
    /// 翻译文本（该语言下的显示文本）
    /// </summary>
    [SugarColumn(ColumnName = "translation_text", ColumnDescription = "翻译文本", ColumnDataType = "nvarchar", Length = 2000, IsNullable = false)]
    public string TranslationText { get; set; } = string.Empty;

    /// <summary>
    /// 资源分组（选项 TaktMenus/tree-options,DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "resource_group", ColumnDescription = "资源分组", ColumnDataType = "varchar", Length = 40, IsNullable = false, DefaultValue = "")]
    public string ResourceGroup { get; set; } = string.Empty;

    /// <summary>
    /// 资源类别（字典 sys_resource_type；frontend=前端 backend=后端）
    /// </summary>
    [SugarColumn(ColumnName = "resource_type", ColumnDescription = "资源类别", ColumnDataType = "varchar", Length = 40, IsNullable = false, DefaultValue = "frontend")]
    public string ResourceType { get; set; } = "frontend";

    /// <summary>
    /// 上下文注释（帮助翻译人员理解使用场景）
    /// </summary>
    [SugarColumn(ColumnName = "context_note", ColumnDescription = "上下文注释", ColumnDataType = "ntext", IsNullable = true)]
    public string? ContextNote { get; set; }

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 文化（多对一关联）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(CultureCode))]
    public TaktCulture? Culture { get; set; }
}
