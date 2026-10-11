// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Identity
// 文件名称：TaktUserPreference.cs
// 创建时间：2026-09-11
// 创建人：Takt365(Cursor AI)
// 功能描述：用户个人偏好实体，持久化前端 AppSetting JSON（布局、主题等）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Identity;

/// <summary>
/// 用户个人偏好实体
/// 公司级：租户+公司+UserId 唯一一行，PreferenceJson 存完整 AppSetting
/// </summary>
[SugarTable("takt_identity_user_preference", "用户个人偏好表")]
[SugarIndex("ix_user_preference_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_user_preference_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_user_preference_user_id_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(UserId), OrderByType.Asc, true)]
public class TaktUserPreference : TaktCompanyEntityBase
{
    /// <summary>
    /// 用户 ID（关联 TaktUser.Id；唯一索引：租户+公司内唯一，见 ix_user_preference_user_id_unique）
    /// </summary>
    [SugarColumn(ColumnName = "user_id", ColumnDescription = "用户ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }

    /// <summary>
    /// 偏好 JSON（完整 AppSetting 序列化；超长配置用 ntext）
    /// </summary>
    [SugarColumn(ColumnName = "preference_json", ColumnDescription = "偏好JSON", ColumnDataType = "ntext", IsNullable = false)]
    public string PreferenceJson { get; set; } = "{}";

    /// <summary>
    /// 用户布局（冗余热字段；如 side、top、mix、content；≠ PreferenceJson 内 layout 键名）
    /// </summary>
    [SugarColumn(ColumnName = "user_layout", ColumnDescription = "用户布局", ColumnDataType = "varchar", Length = 40, IsNullable = false, DefaultValue = "side")]
    public string UserLayout { get; set; } = "side";

    /// <summary>
    /// 用户主题（冗余热字段；如 light、dark；≠ PreferenceJson 内 theme 键名）
    /// </summary>
    [SugarColumn(ColumnName = "user_theme", ColumnDescription = "用户主题", ColumnDataType = "varchar", Length = 40, IsNullable = false, DefaultValue = "light")]
    public string UserTheme { get; set; } = "light";

    /// <summary>
    /// 用户颜色（冗余热字段；themeColor.type 如 blue、green、custom；自定义色值见 PreferenceJson）
    /// </summary>
    [SugarColumn(ColumnName = "user_color", ColumnDescription = "用户颜色", ColumnDataType = "varchar", Length = 40, IsNullable = false, DefaultValue = "blue")]
    public string UserColor { get; set; } = "blue";

    /// <summary>
    /// 用户界面语言（冗余热字段；BCP47 如 zh-CN、en-US、ja-JP；≠ 基类 CultureCode）
    /// </summary>
    [SugarColumn(ColumnName = "user_locale", ColumnDescription = "用户界面语言", ColumnDataType = "varchar", Length = 5, IsNullable = false, DefaultValue = "zh-CN")]
    public string UserLocale { get; set; } = "zh-CN";

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "user_preference_status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int UserPreferenceStatus { get; set; } = 1;
}
