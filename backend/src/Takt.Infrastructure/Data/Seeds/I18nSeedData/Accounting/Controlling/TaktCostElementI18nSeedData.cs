// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Controlling
// 文件名称：TaktCostElementI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktCostElement 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Controlling;

/// <summary>
/// TaktCostElement 实体国际化翻译种子（键前缀 entity.costelement.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktCostElementI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktCostElement 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 costelement 实体翻译...", tenantCode);

        foreach (var item in GetCostElementTranslations())
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

        TaktLogger.Information("TaktCostElement 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktCostElement 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.costelement._self / entity.costelement.{{field}}；ResourceGroup=Controlling；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetCostElementTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.costelement._self
            new TranslationSeedItem("entity.costelement._self", "en-US", "Cost Element Information_us", "实体名称"),
            // entity.costelement._self
            new TranslationSeedItem("entity.costelement._self", "ja-JP", "成本要素信息_jp", "实体名称"),
            // entity.costelement._self
            new TranslationSeedItem("entity.costelement._self", "zh-CN", "成本要素信息", "实体名称"),
            // entity.costelement._self
            new TranslationSeedItem("entity.costelement._self", "zh-HK", "成本要素信息_hk", "实体名称"),

            // entity.costelement.code
            new TranslationSeedItem("entity.costelement.code", "en-US", "成本要素编码_us", "成本要素编码（4位，租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 成本要素编码规则生成并展示，非手输；单据类型菜单：成本要素）"),
            // entity.costelement.code
            new TranslationSeedItem("entity.costelement.code", "ja-JP", "成本要素编码_jp", "成本要素编码（4位，租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 成本要素编码规则生成并展示，非手输；单据类型菜单：成本要素）"),
            // entity.costelement.code
            new TranslationSeedItem("entity.costelement.code", "zh-CN", "成本要素编码", "成本要素编码（4位，租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 成本要素编码规则生成并展示，非手输；单据类型菜单：成本要素）"),
            // entity.costelement.code
            new TranslationSeedItem("entity.costelement.code", "zh-HK", "成本要素编码_hk", "成本要素编码（4位，租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 成本要素编码规则生成并展示，非手输；单据类型菜单：成本要素）"),

            // entity.costelement.name
            new TranslationSeedItem("entity.costelement.name", "en-US", "成本要素名称_us", "成本要素名称"),
            // entity.costelement.name
            new TranslationSeedItem("entity.costelement.name", "ja-JP", "成本要素名称_jp", "成本要素名称"),
            // entity.costelement.name
            new TranslationSeedItem("entity.costelement.name", "zh-CN", "成本要素名称", "成本要素名称"),
            // entity.costelement.name
            new TranslationSeedItem("entity.costelement.name", "zh-HK", "成本要素名称_hk", "成本要素名称"),

            // entity.costelement.type
            new TranslationSeedItem("entity.costelement.type", "en-US", "成本要素类型_us", "成本要素类型（字典 accounting_controlling_cost_element_type；0=初级，1=次级）"),
            // entity.costelement.type
            new TranslationSeedItem("entity.costelement.type", "ja-JP", "成本要素类型_jp", "成本要素类型（字典 accounting_controlling_cost_element_type；0=初级，1=次级）"),
            // entity.costelement.type
            new TranslationSeedItem("entity.costelement.type", "zh-CN", "成本要素类型", "成本要素类型（字典 accounting_controlling_cost_element_type；0=初级，1=次级）"),
            // entity.costelement.type
            new TranslationSeedItem("entity.costelement.type", "zh-HK", "成本要素类型_hk", "成本要素类型（字典 accounting_controlling_cost_element_type；0=初级，1=次级）"),

            // entity.costelement.category
            new TranslationSeedItem("entity.costelement.category", "en-US", "成本要素类别_us", "成本要素类别（字典 accounting_controlling_cost_element_category-KATYP，整型存 1/3/4/11…）"),
            // entity.costelement.category
            new TranslationSeedItem("entity.costelement.category", "ja-JP", "成本要素类别_jp", "成本要素类别（字典 accounting_controlling_cost_element_category-KATYP，整型存 1/3/4/11…）"),
            // entity.costelement.category
            new TranslationSeedItem("entity.costelement.category", "zh-CN", "成本要素类别", "成本要素类别（字典 accounting_controlling_cost_element_category-KATYP，整型存 1/3/4/11…）"),
            // entity.costelement.category
            new TranslationSeedItem("entity.costelement.category", "zh-HK", "成本要素类别_hk", "成本要素类别（字典 accounting_controlling_cost_element_category-KATYP，整型存 1/3/4/11…）"),

            // entity.costelement.parentid
            new TranslationSeedItem("entity.costelement.parentid", "en-US", "父级ID_us", "父级 ID"),
            // entity.costelement.parentid
            new TranslationSeedItem("entity.costelement.parentid", "ja-JP", "父级ID_jp", "父级 ID"),
            // entity.costelement.parentid
            new TranslationSeedItem("entity.costelement.parentid", "zh-CN", "父级ID", "父级 ID"),
            // entity.costelement.parentid
            new TranslationSeedItem("entity.costelement.parentid", "zh-HK", "父级ID_hk", "父级 ID"),

            // entity.costelement.level
            new TranslationSeedItem("entity.costelement.level", "en-US", "成本要素层级_us", "成本要素层级"),
            // entity.costelement.level
            new TranslationSeedItem("entity.costelement.level", "ja-JP", "成本要素层级_jp", "成本要素层级"),
            // entity.costelement.level
            new TranslationSeedItem("entity.costelement.level", "zh-CN", "成本要素层级", "成本要素层级"),
            // entity.costelement.level
            new TranslationSeedItem("entity.costelement.level", "zh-HK", "成本要素层级_hk", "成本要素层级"),

            // entity.costelement.validfrom
            new TranslationSeedItem("entity.costelement.validfrom", "en-US", "生效日期_us", "生效日期"),
            // entity.costelement.validfrom
            new TranslationSeedItem("entity.costelement.validfrom", "ja-JP", "生效日期_jp", "生效日期"),
            // entity.costelement.validfrom
            new TranslationSeedItem("entity.costelement.validfrom", "zh-CN", "生效日期", "生效日期"),
            // entity.costelement.validfrom
            new TranslationSeedItem("entity.costelement.validfrom", "zh-HK", "生效日期_hk", "生效日期"),

            // entity.costelement.validto
            new TranslationSeedItem("entity.costelement.validto", "en-US", "失效日期_us", "失效日期"),
            // entity.costelement.validto
            new TranslationSeedItem("entity.costelement.validto", "ja-JP", "失效日期_jp", "失效日期"),
            // entity.costelement.validto
            new TranslationSeedItem("entity.costelement.validto", "zh-CN", "失效日期", "失效日期"),
            // entity.costelement.validto
            new TranslationSeedItem("entity.costelement.validto", "zh-HK", "失效日期_hk", "失效日期"),

            // entity.costelement.sortorder
            new TranslationSeedItem("entity.costelement.sortorder", "en-US", "排序号_us", "排序号（回填）"),
            // entity.costelement.sortorder
            new TranslationSeedItem("entity.costelement.sortorder", "ja-JP", "排序号_jp", "排序号（回填）"),
            // entity.costelement.sortorder
            new TranslationSeedItem("entity.costelement.sortorder", "zh-CN", "排序号", "排序号（回填）"),
            // entity.costelement.sortorder
            new TranslationSeedItem("entity.costelement.sortorder", "zh-HK", "排序号_hk", "排序号（回填）"),

            // entity.costelement.status
            new TranslationSeedItem("entity.costelement.status", "en-US", "成本要素状态_us", "成本要素状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.costelement.status
            new TranslationSeedItem("entity.costelement.status", "ja-JP", "成本要素状态_jp", "成本要素状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.costelement.status
            new TranslationSeedItem("entity.costelement.status", "zh-CN", "成本要素状态", "成本要素状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.costelement.status
            new TranslationSeedItem("entity.costelement.status", "zh-HK", "成本要素状态_hk", "成本要素状态（字典 sys_normal_disable；1=启用，0=禁用）"),
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
        translation.ResourceGroup = "Controlling";
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
