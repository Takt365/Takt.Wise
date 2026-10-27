// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Procurement
// 文件名称：TaktPurchaseGroupI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPurchaseGroup 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Procurement;

/// <summary>
/// TaktPurchaseGroup 实体国际化翻译种子（键前缀 entity.purchasegroup.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPurchaseGroupI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPurchaseGroup 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 purchasegroup 实体翻译...", tenantCode);

        foreach (var item in GetPurchaseGroupTranslations())
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

        TaktLogger.Information("TaktPurchaseGroup 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPurchaseGroup 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.purchasegroup._self / entity.purchasegroup.{{field}}；ResourceGroup=Procurement；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPurchaseGroupTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.purchasegroup._self
            new TranslationSeedItem("entity.purchasegroup._self", "en-US", "Purchase Group Information_us", "实体名称"),
            // entity.purchasegroup._self
            new TranslationSeedItem("entity.purchasegroup._self", "ja-JP", "Takt采购组主数据信息_jp", "实体名称"),
            // entity.purchasegroup._self
            new TranslationSeedItem("entity.purchasegroup._self", "zh-CN", "Takt采购组主数据信息", "实体名称"),
            // entity.purchasegroup._self
            new TranslationSeedItem("entity.purchasegroup._self", "zh-HK", "Takt采购组主数据信息_hk", "实体名称"),

            // entity.purchasegroup.code
            new TranslationSeedItem("entity.purchasegroup.code", "en-US", "采购组编码_us", "采购组编码（与员工编码同长 6；前端表单选择编码规则后自动通过 TaktNumbering 采购组编码规则生成并展示，非手输；单据类型菜单：采购组）"),
            // entity.purchasegroup.code
            new TranslationSeedItem("entity.purchasegroup.code", "ja-JP", "采购组编码_jp", "采购组编码（与员工编码同长 6；前端表单选择编码规则后自动通过 TaktNumbering 采购组编码规则生成并展示，非手输；单据类型菜单：采购组）"),
            // entity.purchasegroup.code
            new TranslationSeedItem("entity.purchasegroup.code", "zh-CN", "采购组编码", "采购组编码（与员工编码同长 6；前端表单选择编码规则后自动通过 TaktNumbering 采购组编码规则生成并展示，非手输；单据类型菜单：采购组）"),
            // entity.purchasegroup.code
            new TranslationSeedItem("entity.purchasegroup.code", "zh-HK", "采购组编码_hk", "采购组编码（与员工编码同长 6；前端表单选择编码规则后自动通过 TaktNumbering 采购组编码规则生成并展示，非手输；单据类型菜单：采购组）"),

            // entity.purchasegroup.name
            new TranslationSeedItem("entity.purchasegroup.name", "en-US", "采购组名称_us", "采购组名称"),
            // entity.purchasegroup.name
            new TranslationSeedItem("entity.purchasegroup.name", "ja-JP", "采购组名称_jp", "采购组名称"),
            // entity.purchasegroup.name
            new TranslationSeedItem("entity.purchasegroup.name", "zh-CN", "采购组名称", "采购组名称"),
            // entity.purchasegroup.name
            new TranslationSeedItem("entity.purchasegroup.name", "zh-HK", "采购组名称_hk", "采购组名称"),

            // entity.purchasegroup.description
            new TranslationSeedItem("entity.purchasegroup.description", "en-US", "采购组描述_us", "采购组描述"),
            // entity.purchasegroup.description
            new TranslationSeedItem("entity.purchasegroup.description", "ja-JP", "采购组描述_jp", "采购组描述"),
            // entity.purchasegroup.description
            new TranslationSeedItem("entity.purchasegroup.description", "zh-CN", "采购组描述", "采购组描述"),
            // entity.purchasegroup.description
            new TranslationSeedItem("entity.purchasegroup.description", "zh-HK", "采购组描述_hk", "采购组描述"),

            // entity.purchasegroup.category
            new TranslationSeedItem("entity.purchasegroup.category", "en-US", "采购组类别_us", "采购组类别（字典 logistics_procurement_purchase_group_category；1=通用）"),
            // entity.purchasegroup.category
            new TranslationSeedItem("entity.purchasegroup.category", "ja-JP", "采购组类别_jp", "采购组类别（字典 logistics_procurement_purchase_group_category；1=通用）"),
            // entity.purchasegroup.category
            new TranslationSeedItem("entity.purchasegroup.category", "zh-CN", "采购组类别", "采购组类别（字典 logistics_procurement_purchase_group_category；1=通用）"),
            // entity.purchasegroup.category
            new TranslationSeedItem("entity.purchasegroup.category", "zh-HK", "采购组类别_hk", "采购组类别（字典 logistics_procurement_purchase_group_category；1=通用）"),

            // entity.purchasegroup.contactphone
            new TranslationSeedItem("entity.purchasegroup.contactphone", "en-US", "联系电话_us", "联系电话"),
            // entity.purchasegroup.contactphone
            new TranslationSeedItem("entity.purchasegroup.contactphone", "ja-JP", "联系电话_jp", "联系电话"),
            // entity.purchasegroup.contactphone
            new TranslationSeedItem("entity.purchasegroup.contactphone", "zh-CN", "联系电话", "联系电话"),
            // entity.purchasegroup.contactphone
            new TranslationSeedItem("entity.purchasegroup.contactphone", "zh-HK", "联系电话_hk", "联系电话"),

            // entity.purchasegroup.contactemail
            new TranslationSeedItem("entity.purchasegroup.contactemail", "en-US", "联系邮箱_us", "联系邮箱"),
            // entity.purchasegroup.contactemail
            new TranslationSeedItem("entity.purchasegroup.contactemail", "ja-JP", "联系邮箱_jp", "联系邮箱"),
            // entity.purchasegroup.contactemail
            new TranslationSeedItem("entity.purchasegroup.contactemail", "zh-CN", "联系邮箱", "联系邮箱"),
            // entity.purchasegroup.contactemail
            new TranslationSeedItem("entity.purchasegroup.contactemail", "zh-HK", "联系邮箱_hk", "联系邮箱"),

            // entity.purchasegroup.isbuiltin
            new TranslationSeedItem("entity.purchasegroup.isbuiltin", "en-US", "内置_us", "内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）"),
            // entity.purchasegroup.isbuiltin
            new TranslationSeedItem("entity.purchasegroup.isbuiltin", "ja-JP", "内置_jp", "内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）"),
            // entity.purchasegroup.isbuiltin
            new TranslationSeedItem("entity.purchasegroup.isbuiltin", "zh-CN", "内置", "内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）"),
            // entity.purchasegroup.isbuiltin
            new TranslationSeedItem("entity.purchasegroup.isbuiltin", "zh-HK", "内置_hk", "内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）"),

            // entity.purchasegroup.sortorder
            new TranslationSeedItem("entity.purchasegroup.sortorder", "en-US", "排序号_us", "排序号（回填）（越小越靠前）"),
            // entity.purchasegroup.sortorder
            new TranslationSeedItem("entity.purchasegroup.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（越小越靠前）"),
            // entity.purchasegroup.sortorder
            new TranslationSeedItem("entity.purchasegroup.sortorder", "zh-CN", "排序号", "排序号（回填）（越小越靠前）"),
            // entity.purchasegroup.sortorder
            new TranslationSeedItem("entity.purchasegroup.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（越小越靠前）"),

            // entity.purchasegroup.groupstatus
            new TranslationSeedItem("entity.purchasegroup.groupstatus", "en-US", "采购组状态_us", "采购组状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.purchasegroup.groupstatus
            new TranslationSeedItem("entity.purchasegroup.groupstatus", "ja-JP", "采购组状态_jp", "采购组状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.purchasegroup.groupstatus
            new TranslationSeedItem("entity.purchasegroup.groupstatus", "zh-CN", "采购组状态", "采购组状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.purchasegroup.groupstatus
            new TranslationSeedItem("entity.purchasegroup.groupstatus", "zh-HK", "采购组状态_hk", "采购组状态（字典 sys_normal_disable；1=启用，0=禁用）"),
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
        translation.ResourceGroup = "Procurement";
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
