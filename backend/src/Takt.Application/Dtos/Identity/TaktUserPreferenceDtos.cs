// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Identity
// 文件名称：TaktUserPreferenceDtos.cs
// 创建时间：2026-09-11
// 创建人：Takt365(Auto Generated)
// 功能描述：UserPreference 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktUserPreference 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Identity;

// ========================================
// UserPreference 响应 DTO
// ========================================

/// <summary>
/// 用户个人偏好实体 公司级：租户+公司+UserId 唯一一行，PreferenceJson 存完整 AppSetting
/// 对应前端 TaktUserPreferenceDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktUserPreferenceDto : TaktCompanyDtoBase
{
    /// <summary>
    /// UserPreferenceID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserPreferenceId { get; set; }

    /// <summary>
    /// 用户 ID（关联 TaktUser.Id；唯一索引：租户+公司内唯一，见 ix_user_preference_user_id_unique）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }

    /// <summary>
    /// 用户 名称（填充字段）
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// 偏好 JSON（完整 AppSetting 序列化；超长配置用 ntext）
    /// </summary>
    public string PreferenceJson { get; set; } = string.Empty;

    /// <summary>
    /// 用户布局（冗余热字段；如 side、top、mix、content）
    /// </summary>
    public string UserLayout { get; set; } = string.Empty;

    /// <summary>
    /// 用户主题（冗余热字段；如 light、dark）
    /// </summary>
    public string UserTheme { get; set; } = string.Empty;

    /// <summary>
    /// 用户颜色（冗余热字段；themeColor.type 如 blue、green、custom）
    /// </summary>
    public string UserColor { get; set; } = string.Empty;

    /// <summary>
    /// 用户界面语言（冗余热字段；BCP47 如 zh-CN、en-US、ja-JP；≠ 基类 CultureCode）
    /// </summary>
    public string UserLocale { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int UserPreferenceStatus { get; set; } = 0;

}

// ========================================
// UserPreference 查询 DTO
// ========================================

/// <summary>
/// UserPreference 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktUserPreferenceQueryDto : TaktPagedQuery
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
    /// 用户 ID（关联 TaktUser.Id；唯一索引：租户+公司内唯一，见 ix_user_preference_user_id_unique）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UserId { get; set; }

    /// <summary>
    /// 偏好 JSON（完整 AppSetting 序列化；超长配置用 ntext）
    /// </summary>
    public string? PreferenceJson { get; set; } = string.Empty;

    /// <summary>
    /// 用户布局（冗余热字段；如 side、top、mix、content）
    /// </summary>
    public string? UserLayout { get; set; } = string.Empty;

    /// <summary>
    /// 用户主题（冗余热字段；如 light、dark）
    /// </summary>
    public string? UserTheme { get; set; } = string.Empty;

    /// <summary>
    /// 用户颜色（冗余热字段；themeColor.type 如 blue、green、custom）
    /// </summary>
    public string? UserColor { get; set; } = string.Empty;

    /// <summary>
    /// 用户界面语言（冗余热字段；BCP47 如 zh-CN、en-US、ja-JP；≠ 基类 CultureCode）
    /// </summary>
    public string? UserLocale { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int? UserPreferenceStatus { get; set; }

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
// 创建UserPreference DTO
// ========================================

/// <summary>
/// 创建UserPreference DTO
/// </summary>
public class TaktUserPreferenceCreateDto
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
    /// 用户 ID（关联 TaktUser.Id；唯一索引：租户+公司内唯一，见 ix_user_preference_user_id_unique）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }

    /// <summary>
    /// 偏好 JSON（完整 AppSetting 序列化；超长配置用 ntext）
    /// </summary>
    [Required(ErrorMessage = "偏好JSON不能为空")]
    public string PreferenceJson { get; set; } = string.Empty;

    /// <summary>
    /// 用户布局（冗余热字段；如 side、top、mix、content）
    /// </summary>
    [Required(ErrorMessage = "用户布局不能为空")]
    public string UserLayout { get; set; } = string.Empty;

    /// <summary>
    /// 用户主题（冗余热字段；如 light、dark）
    /// </summary>
    [Required(ErrorMessage = "用户主题不能为空")]
    public string UserTheme { get; set; } = string.Empty;

    /// <summary>
    /// 用户颜色（冗余热字段；themeColor.type 如 blue、green、custom）
    /// </summary>
    [Required(ErrorMessage = "用户颜色不能为空")]
    public string UserColor { get; set; } = "blue";

    /// <summary>
    /// 用户界面语言（冗余热字段；BCP47 如 zh-CN、en-US、ja-JP；≠ 基类 CultureCode）
    /// </summary>
    [Required(ErrorMessage = "用户界面语言不能为空")]
    [MaxLength(5, ErrorMessage = "界面语言长度不能超过5")]
    public string UserLocale { get; set; } = "zh-CN";

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int UserPreferenceStatus { get; set; } = 0;

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
// 更新UserPreference DTO
// ========================================

/// <summary>
/// 更新UserPreference DTO
/// 继承 TaktUserPreferenceCreateDto，添加 UserPreferenceId 字段
/// </summary>
public class TaktUserPreferenceUpdateDto : TaktUserPreferenceCreateDto
{
    /// <summary>
    /// UserPreferenceID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserPreferenceId { get; set; }

}

// ========================================
// UserPreference 状态 DTO
// ========================================

/// <summary>
/// UserPreference 状态更新 DTO
/// </summary>
public class TaktUserPreferenceStatusDto
{
    /// <summary>
    /// UserPreferenceID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserPreferenceId { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    [Required(ErrorMessage = "状态（字典 sys_normal_disable；1=启用 0=禁用）不能为空")]
    public int UserPreferenceStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// UserPreference 导入模板行 DTO
/// </summary>
public class TaktUserPreferenceTemplateDto
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
    /// 用户 ID（关联 TaktUser.Id；唯一索引：租户+公司内唯一，见 ix_user_preference_user_id_unique）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UserId { get; set; }

    /// <summary>
    /// 偏好 JSON（完整 AppSetting 序列化；超长配置用 ntext）
    /// </summary>
    public string? PreferenceJson { get; set; } = string.Empty;

    /// <summary>
    /// 用户布局（冗余热字段；如 side、top、mix、content）
    /// </summary>
    public string? UserLayout { get; set; } = string.Empty;

    /// <summary>
    /// 用户主题（冗余热字段；如 light、dark）
    /// </summary>
    public string? UserTheme { get; set; } = string.Empty;

    /// <summary>
    /// 用户颜色（冗余热字段；themeColor.type 如 blue、green、custom）
    /// </summary>
    public string? UserColor { get; set; } = string.Empty;

    /// <summary>
    /// 用户界面语言（冗余热字段；BCP47 如 zh-CN、en-US、ja-JP；≠ 基类 CultureCode）
    /// </summary>
    public string? UserLocale { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int? UserPreferenceStatus { get; set; }

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
/// UserPreference 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktUserPreferenceImportDto
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
    /// 用户 ID（关联 TaktUser.Id；唯一索引：租户+公司内唯一，见 ix_user_preference_user_id_unique）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UserId { get; set; }

    /// <summary>
    /// 偏好 JSON（完整 AppSetting 序列化；超长配置用 ntext）
    /// </summary>
    public string? PreferenceJson { get; set; } = string.Empty;

    /// <summary>
    /// 用户布局（冗余热字段；如 side、top、mix、content）
    /// </summary>
    public string? UserLayout { get; set; } = string.Empty;

    /// <summary>
    /// 用户主题（冗余热字段；如 light、dark）
    /// </summary>
    public string? UserTheme { get; set; } = string.Empty;

    /// <summary>
    /// 用户颜色（冗余热字段；themeColor.type 如 blue、green、custom）
    /// </summary>
    public string? UserColor { get; set; } = string.Empty;

    /// <summary>
    /// 用户界面语言（冗余热字段；BCP47 如 zh-CN、en-US、ja-JP；≠ 基类 CultureCode）
    /// </summary>
    public string? UserLocale { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int? UserPreferenceStatus { get; set; }

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
/// UserPreference 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktUserPreferenceExportDto
{
    /// <summary>
    /// UserPreferenceID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserPreferenceId { get; set; }

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
    /// 用户 ID（关联 TaktUser.Id；唯一索引：租户+公司内唯一，见 ix_user_preference_user_id_unique）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }

    /// <summary>
    /// 偏好 JSON（完整 AppSetting 序列化；超长配置用 ntext）
    /// </summary>
    public string PreferenceJson { get; set; } = string.Empty;

    /// <summary>
    /// 用户布局（冗余热字段；如 side、top、mix、content）
    /// </summary>
    public string UserLayout { get; set; } = string.Empty;

    /// <summary>
    /// 用户主题（冗余热字段；如 light、dark）
    /// </summary>
    public string UserTheme { get; set; } = string.Empty;

    /// <summary>
    /// 用户颜色（冗余热字段；themeColor.type 如 blue、green、custom）
    /// </summary>
    public string UserColor { get; set; } = string.Empty;

    /// <summary>
    /// 用户界面语言（冗余热字段；BCP47 如 zh-CN、en-US、ja-JP；≠ 基类 CultureCode）
    /// </summary>
    public string UserLocale { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int UserPreferenceStatus { get; set; } = 0;

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
// UserPreference 当前用户 Upsert DTO
// ========================================

/// <summary>
/// 当前登录用户偏好 Upsert DTO（仅 PreferenceJson；UserId 由服务端注入）
/// </summary>
public class TaktUserPreferenceMeUpsertDto
{
    /// <summary>
    /// 偏好 JSON（完整 AppSetting 序列化；超长配置用 ntext）
    /// </summary>
    [Required(ErrorMessage = "偏好JSON不能为空")]
    public string PreferenceJson { get; set; } = "{}";
}
