// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Cost
// 文件名称：TaktCostGroupI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktCostGroup 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Cost;

/// <summary>
/// TaktCostGroup 实体国际化翻译种子（键前缀 entity.costgroup.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktCostGroupI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktCostGroup 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 costgroup 实体翻译...", tenantCode);

        foreach (var item in GetCostGroupTranslations())
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

        TaktLogger.Information("TaktCostGroup 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktCostGroup 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.costgroup._self / entity.costgroup.{{field}}；ResourceGroup=Cost；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetCostGroupTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.costgroup._self
            new TranslationSeedItem("entity.costgroup._self", "en-US", "Cost Group Information_us", "实体名称"),
            // entity.costgroup._self
            new TranslationSeedItem("entity.costgroup._self", "ja-JP", "成本组主数据信息_jp", "实体名称"),
            // entity.costgroup._self
            new TranslationSeedItem("entity.costgroup._self", "zh-CN", "成本组主数据信息", "实体名称"),
            // entity.costgroup._self
            new TranslationSeedItem("entity.costgroup._self", "zh-HK", "成本组主数据信息_hk", "实体名称"),

            // entity.costgroup.code
            new TranslationSeedItem("entity.costgroup.code", "en-US", "成本组编码_us", "成本组编码（与员工编码同长 6；前端表单选择编码规则后自动通过 TaktNumbering 成本组编码规则生成并展示，非手输；单据类型菜单：品质成本）"),
            // entity.costgroup.code
            new TranslationSeedItem("entity.costgroup.code", "ja-JP", "成本组编码_jp", "成本组编码（与员工编码同长 6；前端表单选择编码规则后自动通过 TaktNumbering 成本组编码规则生成并展示，非手输；单据类型菜单：品质成本）"),
            // entity.costgroup.code
            new TranslationSeedItem("entity.costgroup.code", "zh-CN", "成本组编码", "成本组编码（与员工编码同长 6；前端表单选择编码规则后自动通过 TaktNumbering 成本组编码规则生成并展示，非手输；单据类型菜单：品质成本）"),
            // entity.costgroup.code
            new TranslationSeedItem("entity.costgroup.code", "zh-HK", "成本组编码_hk", "成本组编码（与员工编码同长 6；前端表单选择编码规则后自动通过 TaktNumbering 成本组编码规则生成并展示，非手输；单据类型菜单：品质成本）"),

            // entity.costgroup.name
            new TranslationSeedItem("entity.costgroup.name", "en-US", "成本组名称_us", "成本组名称"),
            // entity.costgroup.name
            new TranslationSeedItem("entity.costgroup.name", "ja-JP", "成本组名称_jp", "成本组名称"),
            // entity.costgroup.name
            new TranslationSeedItem("entity.costgroup.name", "zh-CN", "成本组名称", "成本组名称"),
            // entity.costgroup.name
            new TranslationSeedItem("entity.costgroup.name", "zh-HK", "成本组名称_hk", "成本组名称"),

            // entity.costgroup.description
            new TranslationSeedItem("entity.costgroup.description", "en-US", "成本组描述_us", "成本组描述"),
            // entity.costgroup.description
            new TranslationSeedItem("entity.costgroup.description", "ja-JP", "成本组描述_jp", "成本组描述"),
            // entity.costgroup.description
            new TranslationSeedItem("entity.costgroup.description", "zh-CN", "成本组描述", "成本组描述"),
            // entity.costgroup.description
            new TranslationSeedItem("entity.costgroup.description", "zh-HK", "成本组描述_hk", "成本组描述"),

            // entity.costgroup.category
            new TranslationSeedItem("entity.costgroup.category", "en-US", "成本组类别_us", "成本组类别（字典 logistics_quality_cost_group_category；1=记录组 2=废弃组 3=对应组）"),
            // entity.costgroup.category
            new TranslationSeedItem("entity.costgroup.category", "ja-JP", "成本组类别_jp", "成本组类别（字典 logistics_quality_cost_group_category；1=记录组 2=废弃组 3=对应组）"),
            // entity.costgroup.category
            new TranslationSeedItem("entity.costgroup.category", "zh-CN", "成本组类别", "成本组类别（字典 logistics_quality_cost_group_category；1=记录组 2=废弃组 3=对应组）"),
            // entity.costgroup.category
            new TranslationSeedItem("entity.costgroup.category", "zh-HK", "成本组类别_hk", "成本组类别（字典 logistics_quality_cost_group_category；1=记录组 2=废弃组 3=对应组）"),

            // entity.costgroup.contactphone
            new TranslationSeedItem("entity.costgroup.contactphone", "en-US", "联系电话_us", "联系电话"),
            // entity.costgroup.contactphone
            new TranslationSeedItem("entity.costgroup.contactphone", "ja-JP", "联系电话_jp", "联系电话"),
            // entity.costgroup.contactphone
            new TranslationSeedItem("entity.costgroup.contactphone", "zh-CN", "联系电话", "联系电话"),
            // entity.costgroup.contactphone
            new TranslationSeedItem("entity.costgroup.contactphone", "zh-HK", "联系电话_hk", "联系电话"),

            // entity.costgroup.contactemail
            new TranslationSeedItem("entity.costgroup.contactemail", "en-US", "联系邮箱_us", "联系邮箱"),
            // entity.costgroup.contactemail
            new TranslationSeedItem("entity.costgroup.contactemail", "ja-JP", "联系邮箱_jp", "联系邮箱"),
            // entity.costgroup.contactemail
            new TranslationSeedItem("entity.costgroup.contactemail", "zh-CN", "联系邮箱", "联系邮箱"),
            // entity.costgroup.contactemail
            new TranslationSeedItem("entity.costgroup.contactemail", "zh-HK", "联系邮箱_hk", "联系邮箱"),

            // entity.costgroup.isbuiltin
            new TranslationSeedItem("entity.costgroup.isbuiltin", "en-US", "内置_us", "内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）"),
            // entity.costgroup.isbuiltin
            new TranslationSeedItem("entity.costgroup.isbuiltin", "ja-JP", "内置_jp", "内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）"),
            // entity.costgroup.isbuiltin
            new TranslationSeedItem("entity.costgroup.isbuiltin", "zh-CN", "内置", "内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）"),
            // entity.costgroup.isbuiltin
            new TranslationSeedItem("entity.costgroup.isbuiltin", "zh-HK", "内置_hk", "内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）"),

            // entity.costgroup.sortorder
            new TranslationSeedItem("entity.costgroup.sortorder", "en-US", "排序号_us", "排序号（回填）（越小越靠前）"),
            // entity.costgroup.sortorder
            new TranslationSeedItem("entity.costgroup.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（越小越靠前）"),
            // entity.costgroup.sortorder
            new TranslationSeedItem("entity.costgroup.sortorder", "zh-CN", "排序号", "排序号（回填）（越小越靠前）"),
            // entity.costgroup.sortorder
            new TranslationSeedItem("entity.costgroup.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（越小越靠前）"),

            // entity.costgroup.groupstatus
            new TranslationSeedItem("entity.costgroup.groupstatus", "en-US", "成本组状态_us", "成本组状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.costgroup.groupstatus
            new TranslationSeedItem("entity.costgroup.groupstatus", "ja-JP", "成本组状态_jp", "成本组状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.costgroup.groupstatus
            new TranslationSeedItem("entity.costgroup.groupstatus", "zh-CN", "成本组状态", "成本组状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.costgroup.groupstatus
            new TranslationSeedItem("entity.costgroup.groupstatus", "zh-HK", "成本组状态_hk", "成本组状态（字典 sys_normal_disable；1=启用，0=禁用）"),
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
        translation.ResourceGroup = "Cost";
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
