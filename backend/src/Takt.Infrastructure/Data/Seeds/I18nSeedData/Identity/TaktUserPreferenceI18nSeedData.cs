// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Identity
// 文件名称：TaktUserPreferenceI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktUserPreference 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Identity;

/// <summary>
/// TaktUserPreference 实体国际化翻译种子（键前缀 entity.userpreference.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktUserPreferenceI18nSeedData : ITaktSeedDataCoordinator
{
    /// <summary>
    /// 执行顺序（实体翻译种子，位于部门翻译之后）
    /// </summary>
    public int Order => 52;

    /// <summary>
    /// 初始化实体字段翻译种子
    /// </summary>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化 TaktUserPreference 实体国际化翻译种子...");

        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过实体国际化翻译种子初始化");
            return (0, 0);
        }

        var repository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktTranslation>>();
        var cultureRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCulture>>();
        var cultureIdByCode = (await cultureRepository.GetListAsync(c => c.TenantCode == tenantCode))
            .ToDictionary(c => c.CultureCode, c => c.Id);
        int insertCount = 0;
        int updateCount = 0;

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 userpreference 实体翻译...", tenantCode);

        foreach (var item in GetUserPreferenceTranslations())
        {
            if (!cultureIdByCode.TryGetValue(item.CultureCode, out var cultureId))
            {
                TaktLogger.Warning("未找到区域文化 {CultureCode}，跳过翻译 {I18nKey}", item.CultureCode, item.I18nKey);
                continue;
            }

            var (translation, i, u) = await CreateOrUpdateTranslationAsync(
                repository,
                tenantCode,
                cultureId,
                item);
            insertCount += i;
            updateCount += u;
        }

        TaktLogger.Information("TaktUserPreference 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktUserPreference 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.userpreference._self / entity.userpreference.{{field}}；ResourceGroup=Identity；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetUserPreferenceTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.userpreference._self
            new TranslationSeedItem("entity.userpreference._self", "en-US", "User Preference Information_us", "实体名称"),
            // entity.userpreference._self
            new TranslationSeedItem("entity.userpreference._self", "ja-JP", "用户个人偏好信息_jp", "实体名称"),
            // entity.userpreference._self
            new TranslationSeedItem("entity.userpreference._self", "zh-CN", "用户个人偏好信息", "实体名称"),
            // entity.userpreference._self
            new TranslationSeedItem("entity.userpreference._self", "zh-HK", "用户个人偏好信息_hk", "实体名称"),

            // entity.userpreference.userid
            new TranslationSeedItem("entity.userpreference.userid", "en-US", "用户ID_us", "用户 ID（关联 TaktUser.Id；唯一索引：租户+公司内唯一，见 ix_user_preference_user_id_unique）"),
            // entity.userpreference.userid
            new TranslationSeedItem("entity.userpreference.userid", "ja-JP", "用户ID_jp", "用户 ID（关联 TaktUser.Id；唯一索引：租户+公司内唯一，见 ix_user_preference_user_id_unique）"),
            // entity.userpreference.userid
            new TranslationSeedItem("entity.userpreference.userid", "zh-CN", "用户ID", "用户 ID（关联 TaktUser.Id；唯一索引：租户+公司内唯一，见 ix_user_preference_user_id_unique）"),
            // entity.userpreference.userid
            new TranslationSeedItem("entity.userpreference.userid", "zh-HK", "用户ID_hk", "用户 ID（关联 TaktUser.Id；唯一索引：租户+公司内唯一，见 ix_user_preference_user_id_unique）"),

            // entity.userpreference.preferencejson
            new TranslationSeedItem("entity.userpreference.preferencejson", "en-US", "偏好JSON_us", "偏好 JSON（完整 AppSetting 序列化；超长配置用 ntext）"),
            // entity.userpreference.preferencejson
            new TranslationSeedItem("entity.userpreference.preferencejson", "ja-JP", "偏好JSON_jp", "偏好 JSON（完整 AppSetting 序列化；超长配置用 ntext）"),
            // entity.userpreference.preferencejson
            new TranslationSeedItem("entity.userpreference.preferencejson", "zh-CN", "偏好JSON", "偏好 JSON（完整 AppSetting 序列化；超长配置用 ntext）"),
            // entity.userpreference.preferencejson
            new TranslationSeedItem("entity.userpreference.preferencejson", "zh-HK", "偏好JSON_hk", "偏好 JSON（完整 AppSetting 序列化；超长配置用 ntext）"),

            // entity.userpreference.userlayout
            new TranslationSeedItem("entity.userpreference.userlayout", "en-US", "用户布局_us", "用户布局（冗余热字段；如 side、top、mix、content；≠ PreferenceJson 内 layout 键名）"),
            // entity.userpreference.userlayout
            new TranslationSeedItem("entity.userpreference.userlayout", "ja-JP", "用户布局_jp", "用户布局（冗余热字段；如 side、top、mix、content；≠ PreferenceJson 内 layout 键名）"),
            // entity.userpreference.userlayout
            new TranslationSeedItem("entity.userpreference.userlayout", "zh-CN", "用户布局", "用户布局（冗余热字段；如 side、top、mix、content；≠ PreferenceJson 内 layout 键名）"),
            // entity.userpreference.userlayout
            new TranslationSeedItem("entity.userpreference.userlayout", "zh-HK", "用户布局_hk", "用户布局（冗余热字段；如 side、top、mix、content；≠ PreferenceJson 内 layout 键名）"),

            // entity.userpreference.usertheme
            new TranslationSeedItem("entity.userpreference.usertheme", "en-US", "用户主题_us", "用户主题（冗余热字段；如 light、dark；≠ PreferenceJson 内 theme 键名）"),
            // entity.userpreference.usertheme
            new TranslationSeedItem("entity.userpreference.usertheme", "ja-JP", "用户主题_jp", "用户主题（冗余热字段；如 light、dark；≠ PreferenceJson 内 theme 键名）"),
            // entity.userpreference.usertheme
            new TranslationSeedItem("entity.userpreference.usertheme", "zh-CN", "用户主题", "用户主题（冗余热字段；如 light、dark；≠ PreferenceJson 内 theme 键名）"),
            // entity.userpreference.usertheme
            new TranslationSeedItem("entity.userpreference.usertheme", "zh-HK", "用户主题_hk", "用户主题（冗余热字段；如 light、dark；≠ PreferenceJson 内 theme 键名）"),

            // entity.userpreference.usercolor
            new TranslationSeedItem("entity.userpreference.usercolor", "en-US", "用户颜色_us", "用户颜色（冗余热字段；themeColor.type 如 blue、green、custom；自定义色值见 PreferenceJson）"),
            // entity.userpreference.usercolor
            new TranslationSeedItem("entity.userpreference.usercolor", "ja-JP", "用户颜色_jp", "用户颜色（冗余热字段；themeColor.type 如 blue、green、custom；自定义色值见 PreferenceJson）"),
            // entity.userpreference.usercolor
            new TranslationSeedItem("entity.userpreference.usercolor", "zh-CN", "用户颜色", "用户颜色（冗余热字段；themeColor.type 如 blue、green、custom；自定义色值见 PreferenceJson）"),
            // entity.userpreference.usercolor
            new TranslationSeedItem("entity.userpreference.usercolor", "zh-HK", "用户颜色_hk", "用户颜色（冗余热字段；themeColor.type 如 blue、green、custom；自定义色值见 PreferenceJson）"),

            // entity.userpreference.userlocale
            new TranslationSeedItem("entity.userpreference.userlocale", "en-US", "用户界面语言_us", "用户界面语言（冗余热字段；BCP47 如 zh-CN、en-US、ja-JP；≠ 基类 CultureCode）"),
            // entity.userpreference.userlocale
            new TranslationSeedItem("entity.userpreference.userlocale", "ja-JP", "用户界面语言_jp", "用户界面语言（冗余热字段；BCP47 如 zh-CN、en-US、ja-JP；≠ 基类 CultureCode）"),
            // entity.userpreference.userlocale
            new TranslationSeedItem("entity.userpreference.userlocale", "zh-CN", "用户界面语言", "用户界面语言（冗余热字段；BCP47 如 zh-CN、en-US、ja-JP；≠ 基类 CultureCode）"),
            // entity.userpreference.userlocale
            new TranslationSeedItem("entity.userpreference.userlocale", "zh-HK", "用户界面语言_hk", "用户界面语言（冗余热字段；BCP47 如 zh-CN、en-US、ja-JP；≠ 基类 CultureCode）"),

            // entity.userpreference.status
            new TranslationSeedItem("entity.userpreference.status", "en-US", "状态_us", "状态（字典 sys_normal_disable；1=启用 0=禁用）"),
            // entity.userpreference.status
            new TranslationSeedItem("entity.userpreference.status", "ja-JP", "状态_jp", "状态（字典 sys_normal_disable；1=启用 0=禁用）"),
            // entity.userpreference.status
            new TranslationSeedItem("entity.userpreference.status", "zh-CN", "状态", "状态（字典 sys_normal_disable；1=启用 0=禁用）"),
            // entity.userpreference.status
            new TranslationSeedItem("entity.userpreference.status", "zh-HK", "状态_hk", "状态（字典 sys_normal_disable；1=启用 0=禁用）"),
        };
    }

    /// <summary>
    /// 填充 TaktTranslation 全部业务字段（含租户基类字段）
    /// </summary>
    private static void ApplyTranslationFields(
        TaktTranslation translation,
        string tenantCode,
        long cultureId,
        TranslationSeedItem item)
    {
        translation.TenantCode = tenantCode;
        translation.CultureId = cultureId;
        translation.CultureCode = item.CultureCode;
        translation.I18nKey = item.I18nKey;
        translation.TranslationText = item.TranslationText;
        translation.ResourceGroup = "Identity";
        translation.ResourceType = "frontend";
        translation.ContextNote = item.ContextNote;
        translation.ExtField = null;
        translation.Remark = null;
        translation.IsDeleted = 0;
        translation.DeletedBy = null;
        translation.DeletedAt = null;
    }

    private static async Task<(TaktTranslation Translation, int InsertCount, int UpdateCount)> CreateOrUpdateTranslationAsync(
        ITaktTenantSeedRepository<TaktTranslation> repository,
        string tenantCode,
        long cultureId,
        TranslationSeedItem item)
    {
        var translation = await repository.FirstAsync(t =>
            t.TenantCode == tenantCode &&
            t.I18nKey == item.I18nKey &&
            t.CultureCode == item.CultureCode);

        if (translation == null)
        {
            translation = new TaktTranslation();
            ApplyTranslationFields(translation, tenantCode, cultureId, item);
            translation = await repository.CreateAsync(translation);
            return (translation, 1, 0);
        }

        ApplyTranslationFields(translation, tenantCode, cultureId, item);
        await repository.UpdateAsync(translation);
        return (translation, 0, 1);
    }

    /// <summary>
    /// 翻译种子项（对应 TaktTranslation 全部可写字段，CultureId 由 SeedAsync 解析）
    /// </summary>
    private sealed record TranslationSeedItem(
        string I18nKey,
        string CultureCode,
        string TranslationText,
        string? ContextNote);
}
