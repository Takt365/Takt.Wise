// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Foundation
// 文件名称：TaktSystemPropertyI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSystemProperty 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Foundation;

/// <summary>
/// TaktSystemProperty 实体国际化翻译种子（键前缀 entity.systemproperty.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSystemPropertyI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSystemProperty 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 systemproperty 实体翻译...", tenantCode);

        foreach (var item in GetSystemPropertyTranslations())
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

        TaktLogger.Information("TaktSystemProperty 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSystemProperty 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.systemproperty._self / entity.systemproperty.{{field}}；ResourceGroup=Foundation；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSystemPropertyTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.systemproperty._self
            new TranslationSeedItem("entity.systemproperty._self", "en-US", "System Property Information_us", "实体名称"),
            // entity.systemproperty._self
            new TranslationSeedItem("entity.systemproperty._self", "ja-JP", "系统属性信息_jp", "实体名称"),
            // entity.systemproperty._self
            new TranslationSeedItem("entity.systemproperty._self", "zh-CN", "系统属性信息", "实体名称"),
            // entity.systemproperty._self
            new TranslationSeedItem("entity.systemproperty._self", "zh-HK", "系统属性信息_hk", "实体名称"),

            // entity.systemproperty.key
            new TranslationSeedItem("entity.systemproperty.key", "en-US", "属性键_us", "属性键（唯一索引：租户+公司内唯一，见 ix_system_property_key_unique；如 system.siteName, upload.maxSize）"),
            // entity.systemproperty.key
            new TranslationSeedItem("entity.systemproperty.key", "ja-JP", "属性键_jp", "属性键（唯一索引：租户+公司内唯一，见 ix_system_property_key_unique；如 system.siteName, upload.maxSize）"),
            // entity.systemproperty.key
            new TranslationSeedItem("entity.systemproperty.key", "zh-CN", "属性键", "属性键（唯一索引：租户+公司内唯一，见 ix_system_property_key_unique；如 system.siteName, upload.maxSize）"),
            // entity.systemproperty.key
            new TranslationSeedItem("entity.systemproperty.key", "zh-HK", "属性键_hk", "属性键（唯一索引：租户+公司内唯一，见 ix_system_property_key_unique；如 system.siteName, upload.maxSize）"),

            // entity.systemproperty.value
            new TranslationSeedItem("entity.systemproperty.value", "en-US", "属性值_us", "属性值（字符串形式，复杂对象用JSON；超长配置用 ntext）"),
            // entity.systemproperty.value
            new TranslationSeedItem("entity.systemproperty.value", "ja-JP", "属性值_jp", "属性值（字符串形式，复杂对象用JSON；超长配置用 ntext）"),
            // entity.systemproperty.value
            new TranslationSeedItem("entity.systemproperty.value", "zh-CN", "属性值", "属性值（字符串形式，复杂对象用JSON；超长配置用 ntext）"),
            // entity.systemproperty.value
            new TranslationSeedItem("entity.systemproperty.value", "zh-HK", "属性值_hk", "属性值（字符串形式，复杂对象用JSON；超长配置用 ntext）"),

            // entity.systemproperty.name
            new TranslationSeedItem("entity.systemproperty.name", "en-US", "属性名称_us", "属性名称（显示名称，如：站点名称、最大上传大小）"),
            // entity.systemproperty.name
            new TranslationSeedItem("entity.systemproperty.name", "ja-JP", "属性名称_jp", "属性名称（显示名称，如：站点名称、最大上传大小）"),
            // entity.systemproperty.name
            new TranslationSeedItem("entity.systemproperty.name", "zh-CN", "属性名称", "属性名称（显示名称，如：站点名称、最大上传大小）"),
            // entity.systemproperty.name
            new TranslationSeedItem("entity.systemproperty.name", "zh-HK", "属性名称_hk", "属性名称（显示名称，如：站点名称、最大上传大小）"),

            // entity.systemproperty.description
            new TranslationSeedItem("entity.systemproperty.description", "en-US", "属性描述_us", "属性描述"),
            // entity.systemproperty.description
            new TranslationSeedItem("entity.systemproperty.description", "ja-JP", "属性描述_jp", "属性描述"),
            // entity.systemproperty.description
            new TranslationSeedItem("entity.systemproperty.description", "zh-CN", "属性描述", "属性描述"),
            // entity.systemproperty.description
            new TranslationSeedItem("entity.systemproperty.description", "zh-HK", "属性描述_hk", "属性描述"),

            // entity.systemproperty.group
            new TranslationSeedItem("entity.systemproperty.group", "en-US", "属性类别_us", "属性类别（字典 sys_resource_type；frontend=前端 backend=后端）"),
            // entity.systemproperty.group
            new TranslationSeedItem("entity.systemproperty.group", "ja-JP", "属性类别_jp", "属性类别（字典 sys_resource_type；frontend=前端 backend=后端）"),
            // entity.systemproperty.group
            new TranslationSeedItem("entity.systemproperty.group", "zh-CN", "属性类别", "属性类别（字典 sys_resource_type；frontend=前端 backend=后端）"),
            // entity.systemproperty.group
            new TranslationSeedItem("entity.systemproperty.group", "zh-HK", "属性类别_hk", "属性类别（字典 sys_resource_type；frontend=前端 backend=后端）"),

            // entity.systemproperty.valuetype
            new TranslationSeedItem("entity.systemproperty.valuetype", "en-US", "值类型_us", "值类型（字典 code_generator_display_type；input=文本框 select=下拉框 switch=开关 等）"),
            // entity.systemproperty.valuetype
            new TranslationSeedItem("entity.systemproperty.valuetype", "ja-JP", "值类型_jp", "值类型（字典 code_generator_display_type；input=文本框 select=下拉框 switch=开关 等）"),
            // entity.systemproperty.valuetype
            new TranslationSeedItem("entity.systemproperty.valuetype", "zh-CN", "值类型", "值类型（字典 code_generator_display_type；input=文本框 select=下拉框 switch=开关 等）"),
            // entity.systemproperty.valuetype
            new TranslationSeedItem("entity.systemproperty.valuetype", "zh-HK", "值类型_hk", "值类型（字典 code_generator_display_type；input=文本框 select=下拉框 switch=开关 等）"),

            // entity.systemproperty.isbuiltin
            new TranslationSeedItem("entity.systemproperty.isbuiltin", "en-US", "内置_us", "内置（字典 sys_yes_no；0=否 1=是）"),
            // entity.systemproperty.isbuiltin
            new TranslationSeedItem("entity.systemproperty.isbuiltin", "ja-JP", "内置_jp", "内置（字典 sys_yes_no；0=否 1=是）"),
            // entity.systemproperty.isbuiltin
            new TranslationSeedItem("entity.systemproperty.isbuiltin", "zh-CN", "内置", "内置（字典 sys_yes_no；0=否 1=是）"),
            // entity.systemproperty.isbuiltin
            new TranslationSeedItem("entity.systemproperty.isbuiltin", "zh-HK", "内置_hk", "内置（字典 sys_yes_no；0=否 1=是）"),

            // entity.systemproperty.isreadonly
            new TranslationSeedItem("entity.systemproperty.isreadonly", "en-US", "只读_us", "只读（字典 sys_yes_no；0=否 1=是）"),
            // entity.systemproperty.isreadonly
            new TranslationSeedItem("entity.systemproperty.isreadonly", "ja-JP", "只读_jp", "只读（字典 sys_yes_no；0=否 1=是）"),
            // entity.systemproperty.isreadonly
            new TranslationSeedItem("entity.systemproperty.isreadonly", "zh-CN", "只读", "只读（字典 sys_yes_no；0=否 1=是）"),
            // entity.systemproperty.isreadonly
            new TranslationSeedItem("entity.systemproperty.isreadonly", "zh-HK", "只读_hk", "只读（字典 sys_yes_no；0=否 1=是）"),

            // entity.systemproperty.isencrypted
            new TranslationSeedItem("entity.systemproperty.isencrypted", "en-US", "加密_us", "加密（字典 sys_yes_no；0=否 1=是）"),
            // entity.systemproperty.isencrypted
            new TranslationSeedItem("entity.systemproperty.isencrypted", "ja-JP", "加密_jp", "加密（字典 sys_yes_no；0=否 1=是）"),
            // entity.systemproperty.isencrypted
            new TranslationSeedItem("entity.systemproperty.isencrypted", "zh-CN", "加密", "加密（字典 sys_yes_no；0=否 1=是）"),
            // entity.systemproperty.isencrypted
            new TranslationSeedItem("entity.systemproperty.isencrypted", "zh-HK", "加密_hk", "加密（字典 sys_yes_no；0=否 1=是）"),

            // entity.systemproperty.sortorder
            new TranslationSeedItem("entity.systemproperty.sortorder", "en-US", "排序号_us", "排序号（回填）"),
            // entity.systemproperty.sortorder
            new TranslationSeedItem("entity.systemproperty.sortorder", "ja-JP", "排序号_jp", "排序号（回填）"),
            // entity.systemproperty.sortorder
            new TranslationSeedItem("entity.systemproperty.sortorder", "zh-CN", "排序号", "排序号（回填）"),
            // entity.systemproperty.sortorder
            new TranslationSeedItem("entity.systemproperty.sortorder", "zh-HK", "排序号_hk", "排序号（回填）"),

            // entity.systemproperty.status
            new TranslationSeedItem("entity.systemproperty.status", "en-US", "状态_us", "状态（字典 sys_normal_disable；1=启用 0=禁用）"),
            // entity.systemproperty.status
            new TranslationSeedItem("entity.systemproperty.status", "ja-JP", "状态_jp", "状态（字典 sys_normal_disable；1=启用 0=禁用）"),
            // entity.systemproperty.status
            new TranslationSeedItem("entity.systemproperty.status", "zh-CN", "状态", "状态（字典 sys_normal_disable；1=启用 0=禁用）"),
            // entity.systemproperty.status
            new TranslationSeedItem("entity.systemproperty.status", "zh-HK", "状态_hk", "状态（字典 sys_normal_disable；1=启用 0=禁用）"),
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
        translation.ResourceGroup = "Foundation";
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
