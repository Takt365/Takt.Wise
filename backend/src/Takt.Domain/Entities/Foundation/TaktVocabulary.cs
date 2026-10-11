// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Foundation
// 文件名称：TaktVocabulary.cs
// 创建时间：2026-04-21
// 创建人：Takt365(Cursor AI)
// 功能描述：敏感词实体（租户级共享词库）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Foundation;

/// <summary>
/// 敏感词实体（租户内共享，供新闻、公告评论等模块引用）
/// 组合 4：无关联工厂、无语言（TaktTenantCoreEntityBase；仅租户）
/// </summary>
[SugarTable("takt_foundation_vocabulary", "敏感词表")]
[SugarIndex("ix_vocabulary_tenant", nameof(TenantCode), OrderByType.Asc, false)]
[SugarIndex("ix_vocabulary_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_vocabulary_word_text_unique", nameof(TenantCode), OrderByType.Asc, nameof(WordText), OrderByType.Asc, true)]
[SugarIndex("ix_vocabulary_word_category", nameof(TenantCode), OrderByType.Asc, nameof(WordCategory), OrderByType.Asc, false)]
public class TaktVocabulary : TaktTenantCoreEntityBase
{
    /// <summary>
    /// 敏感词文本（租户内唯一）
    /// </summary>
    [SugarColumn(ColumnName = "word_text", ColumnDescription = "敏感词文本", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string WordText { get; set; } = string.Empty;

    /// <summary>
    /// 词性类别（字典 sys_word_category：1=政治敏感，2=暴力恐怖，3=色情低俗，4=广告营销，5=辱骂歧视）
    /// </summary>
    [SugarColumn(ColumnName = "word_category", ColumnDescription = "词性类别", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int WordCategory { get; set; } = 0;

    /// <summary>
    /// 过滤等级（字典 sys_word_filter_level：1=低，2=中，3=高）
    /// </summary>
    [SugarColumn(ColumnName = "filter_level", ColumnDescription = "过滤等级", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int FilterLevel { get; set; } = 1;

    /// <summary>
    /// 替换文本（默认 *Takt*）
    /// </summary>
    [SugarColumn(ColumnName = "replace_text", ColumnDescription = "替换文本", ColumnDataType = "nvarchar", Length = 6, IsNullable = false, DefaultValue = "*Takt*")]
    public string ReplaceText { get; set; } = "*Takt*";

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "vocabulary_status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int VocabularyStatus { get; set; } = 1;
}
