// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Benefits
// 文件名称：TaktBenefitItemI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktBenefitItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Benefits;

/// <summary>
/// TaktBenefitItem 实体国际化翻译种子（键前缀 entity.benefititem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktBenefitItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktBenefitItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 benefititem 实体翻译...", tenantCode);

        foreach (var item in GetBenefitItemTranslations())
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

        TaktLogger.Information("TaktBenefitItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktBenefitItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.benefititem._self / entity.benefititem.{{field}}；ResourceGroup=Benefits；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetBenefitItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.benefititem._self
            new TranslationSeedItem("entity.benefititem._self", "en-US", "Benefit Item Information_us", "实体名称"),
            // entity.benefititem._self
            new TranslationSeedItem("entity.benefititem._self", "ja-JP", "福利项目信息_jp", "实体名称"),
            // entity.benefititem._self
            new TranslationSeedItem("entity.benefititem._self", "zh-CN", "福利项目信息", "实体名称"),
            // entity.benefititem._self
            new TranslationSeedItem("entity.benefititem._self", "zh-HK", "福利项目信息_hk", "实体名称"),

            // entity.benefititem.itemcode
            new TranslationSeedItem("entity.benefititem.itemcode", "en-US", "福利项目编码_us", "福利项目编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 福利项目编码规则生成并展示，非手输；单据类型菜单：福利项目）"),
            // entity.benefititem.itemcode
            new TranslationSeedItem("entity.benefititem.itemcode", "ja-JP", "福利项目编码_jp", "福利项目编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 福利项目编码规则生成并展示，非手输；单据类型菜单：福利项目）"),
            // entity.benefititem.itemcode
            new TranslationSeedItem("entity.benefititem.itemcode", "zh-CN", "福利项目编码", "福利项目编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 福利项目编码规则生成并展示，非手输；单据类型菜单：福利项目）"),
            // entity.benefititem.itemcode
            new TranslationSeedItem("entity.benefititem.itemcode", "zh-HK", "福利项目编码_hk", "福利项目编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 福利项目编码规则生成并展示，非手输；单据类型菜单：福利项目）"),

            // entity.benefititem.itemname
            new TranslationSeedItem("entity.benefititem.itemname", "en-US", "福利项目名称_us", "福利项目名称"),
            // entity.benefititem.itemname
            new TranslationSeedItem("entity.benefititem.itemname", "ja-JP", "福利项目名称_jp", "福利项目名称"),
            // entity.benefititem.itemname
            new TranslationSeedItem("entity.benefititem.itemname", "zh-CN", "福利项目名称", "福利项目名称"),
            // entity.benefititem.itemname
            new TranslationSeedItem("entity.benefititem.itemname", "zh-HK", "福利项目名称_hk", "福利项目名称"),

            // entity.benefititem.benefitcategory
            new TranslationSeedItem("entity.benefititem.benefitcategory", "en-US", "福利大类_us", "福利大类（字典 humanresource_benefits_benefit_category；1=保险 2=补贴 3=休假 4=其他）"),
            // entity.benefititem.benefitcategory
            new TranslationSeedItem("entity.benefititem.benefitcategory", "ja-JP", "福利大类_jp", "福利大类（字典 humanresource_benefits_benefit_category；1=保险 2=补贴 3=休假 4=其他）"),
            // entity.benefititem.benefitcategory
            new TranslationSeedItem("entity.benefititem.benefitcategory", "zh-CN", "福利大类", "福利大类（字典 humanresource_benefits_benefit_category；1=保险 2=补贴 3=休假 4=其他）"),
            // entity.benefititem.benefitcategory
            new TranslationSeedItem("entity.benefititem.benefitcategory", "zh-HK", "福利大类_hk", "福利大类（字典 humanresource_benefits_benefit_category；1=保险 2=补贴 3=休假 4=其他）"),

            // entity.benefititem.benefittype
            new TranslationSeedItem("entity.benefititem.benefittype", "en-US", "福利类型_us", "福利类型（字典 humanresource_benefits_benefit_type；1=社保 2=公积金 3=商业保险 4=年假额度 5=餐补 6=培训补贴 7=员工折扣）"),
            // entity.benefititem.benefittype
            new TranslationSeedItem("entity.benefititem.benefittype", "ja-JP", "福利类型_jp", "福利类型（字典 humanresource_benefits_benefit_type；1=社保 2=公积金 3=商业保险 4=年假额度 5=餐补 6=培训补贴 7=员工折扣）"),
            // entity.benefititem.benefittype
            new TranslationSeedItem("entity.benefititem.benefittype", "zh-CN", "福利类型", "福利类型（字典 humanresource_benefits_benefit_type；1=社保 2=公积金 3=商业保险 4=年假额度 5=餐补 6=培训补贴 7=员工折扣）"),
            // entity.benefititem.benefittype
            new TranslationSeedItem("entity.benefititem.benefittype", "zh-HK", "福利类型_hk", "福利类型（字典 humanresource_benefits_benefit_type；1=社保 2=公积金 3=商业保险 4=年假额度 5=餐补 6=培训补贴 7=员工折扣）"),

            // entity.benefititem.paymentcycle
            new TranslationSeedItem("entity.benefititem.paymentcycle", "en-US", "发放周期_us", "发放周期（字典 humanresource_benefits_benefit_payment_cycle；1=月度 2=季度 3=年度 4=一次性）"),
            // entity.benefititem.paymentcycle
            new TranslationSeedItem("entity.benefititem.paymentcycle", "ja-JP", "发放周期_jp", "发放周期（字典 humanresource_benefits_benefit_payment_cycle；1=月度 2=季度 3=年度 4=一次性）"),
            // entity.benefititem.paymentcycle
            new TranslationSeedItem("entity.benefititem.paymentcycle", "zh-CN", "发放周期", "发放周期（字典 humanresource_benefits_benefit_payment_cycle；1=月度 2=季度 3=年度 4=一次性）"),
            // entity.benefititem.paymentcycle
            new TranslationSeedItem("entity.benefititem.paymentcycle", "zh-HK", "发放周期_hk", "发放周期（字典 humanresource_benefits_benefit_payment_cycle；1=月度 2=季度 3=年度 4=一次性）"),

            // entity.benefititem.defaultamount
            new TranslationSeedItem("entity.benefititem.defaultamount", "en-US", "默认金额_us", "默认金额或补贴标准（元）"),
            // entity.benefititem.defaultamount
            new TranslationSeedItem("entity.benefititem.defaultamount", "ja-JP", "默认金额_jp", "默认金额或补贴标准（元）"),
            // entity.benefititem.defaultamount
            new TranslationSeedItem("entity.benefititem.defaultamount", "zh-CN", "默认金额", "默认金额或补贴标准（元）"),
            // entity.benefititem.defaultamount
            new TranslationSeedItem("entity.benefititem.defaultamount", "zh-HK", "默认金额_hk", "默认金额或补贴标准（元）"),

            // entity.benefititem.maxamount
            new TranslationSeedItem("entity.benefititem.maxamount", "en-US", "金额上限_us", "金额上限（元，0 表示不限制）"),
            // entity.benefititem.maxamount
            new TranslationSeedItem("entity.benefititem.maxamount", "ja-JP", "金额上限_jp", "金额上限（元，0 表示不限制）"),
            // entity.benefititem.maxamount
            new TranslationSeedItem("entity.benefititem.maxamount", "zh-CN", "金额上限", "金额上限（元，0 表示不限制）"),
            // entity.benefititem.maxamount
            new TranslationSeedItem("entity.benefititem.maxamount", "zh-HK", "金额上限_hk", "金额上限（元，0 表示不限制）"),

            // entity.benefititem.employerratio
            new TranslationSeedItem("entity.benefititem.employerratio", "en-US", "公司承担比例_us", "公司承担比例（%，如公积金单位缴存比例）"),
            // entity.benefititem.employerratio
            new TranslationSeedItem("entity.benefititem.employerratio", "ja-JP", "公司承担比例_jp", "公司承担比例（%，如公积金单位缴存比例）"),
            // entity.benefititem.employerratio
            new TranslationSeedItem("entity.benefititem.employerratio", "zh-CN", "公司承担比例", "公司承担比例（%，如公积金单位缴存比例）"),
            // entity.benefititem.employerratio
            new TranslationSeedItem("entity.benefititem.employerratio", "zh-HK", "公司承担比例_hk", "公司承担比例（%，如公积金单位缴存比例）"),

            // entity.benefititem.employeeratio
            new TranslationSeedItem("entity.benefititem.employeeratio", "en-US", "个人承担比例_us", "个人承担比例（%，如公积金个人缴存比例）"),
            // entity.benefititem.employeeratio
            new TranslationSeedItem("entity.benefititem.employeeratio", "ja-JP", "个人承担比例_jp", "个人承担比例（%，如公积金个人缴存比例）"),
            // entity.benefititem.employeeratio
            new TranslationSeedItem("entity.benefititem.employeeratio", "zh-CN", "个人承担比例", "个人承担比例（%，如公积金个人缴存比例）"),
            // entity.benefititem.employeeratio
            new TranslationSeedItem("entity.benefititem.employeeratio", "zh-HK", "个人承担比例_hk", "个人承担比例（%，如公积金个人缴存比例）"),

            // entity.benefititem.ismandatory
            new TranslationSeedItem("entity.benefititem.ismandatory", "en-US", "是否强制福利_us", "是否强制福利（字典 sys_yes_no；0=否 1=是）"),
            // entity.benefititem.ismandatory
            new TranslationSeedItem("entity.benefititem.ismandatory", "ja-JP", "是否强制福利_jp", "是否强制福利（字典 sys_yes_no；0=否 1=是）"),
            // entity.benefititem.ismandatory
            new TranslationSeedItem("entity.benefititem.ismandatory", "zh-CN", "是否强制福利", "是否强制福利（字典 sys_yes_no；0=否 1=是）"),
            // entity.benefititem.ismandatory
            new TranslationSeedItem("entity.benefititem.ismandatory", "zh-HK", "是否强制福利_hk", "是否强制福利（字典 sys_yes_no；0=否 1=是）"),

            // entity.benefititem.sortorder
            new TranslationSeedItem("entity.benefititem.sortorder", "en-US", "排序号_us", "排序号（回填）"),
            // entity.benefititem.sortorder
            new TranslationSeedItem("entity.benefititem.sortorder", "ja-JP", "排序号_jp", "排序号（回填）"),
            // entity.benefititem.sortorder
            new TranslationSeedItem("entity.benefititem.sortorder", "zh-CN", "排序号", "排序号（回填）"),
            // entity.benefititem.sortorder
            new TranslationSeedItem("entity.benefititem.sortorder", "zh-HK", "排序号_hk", "排序号（回填）"),

            // entity.benefititem.itemstatus
            new TranslationSeedItem("entity.benefititem.itemstatus", "en-US", "状态_us", "状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）"),
            // entity.benefititem.itemstatus
            new TranslationSeedItem("entity.benefititem.itemstatus", "ja-JP", "状态_jp", "状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）"),
            // entity.benefititem.itemstatus
            new TranslationSeedItem("entity.benefititem.itemstatus", "zh-CN", "状态", "状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）"),
            // entity.benefititem.itemstatus
            new TranslationSeedItem("entity.benefititem.itemstatus", "zh-HK", "状态_hk", "状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）"),
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
        translation.ResourceGroup = "Benefits";
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
