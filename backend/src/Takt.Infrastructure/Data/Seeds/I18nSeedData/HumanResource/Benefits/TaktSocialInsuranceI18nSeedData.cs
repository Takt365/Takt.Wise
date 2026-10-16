// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Benefits
// 文件名称：TaktSocialInsuranceI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSocialInsurance 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktSocialInsurance 实体国际化翻译种子（键前缀 entity.socialinsurance.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSocialInsuranceI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSocialInsurance 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 socialinsurance 实体翻译...", tenantCode);

        foreach (var item in GetSocialInsuranceTranslations())
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

        TaktLogger.Information("TaktSocialInsurance 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSocialInsurance 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.socialinsurance._self / entity.socialinsurance.{{field}}；ResourceGroup=Benefits；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSocialInsuranceTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.socialinsurance._self
            new TranslationSeedItem("entity.socialinsurance._self", "en-US", "Social Insurance Information_us", "实体名称"),
            // entity.socialinsurance._self
            new TranslationSeedItem("entity.socialinsurance._self", "ja-JP", "社保与公积金月度缴纳流水信息_jp", "实体名称"),
            // entity.socialinsurance._self
            new TranslationSeedItem("entity.socialinsurance._self", "zh-CN", "社保与公积金月度缴纳流水信息", "实体名称"),
            // entity.socialinsurance._self
            new TranslationSeedItem("entity.socialinsurance._self", "zh-HK", "社保与公积金月度缴纳流水信息_hk", "实体名称"),

            // entity.socialinsurance.benefititemid
            new TranslationSeedItem("entity.socialinsurance.benefititemid", "en-US", "福利项目ID_us", "福利项目（选项 TaktBenefitItems/options；通常 benefit_type 为社保/公积金，DictValue=Id）"),
            // entity.socialinsurance.benefititemid
            new TranslationSeedItem("entity.socialinsurance.benefititemid", "ja-JP", "福利项目ID_jp", "福利项目（选项 TaktBenefitItems/options；通常 benefit_type 为社保/公积金，DictValue=Id）"),
            // entity.socialinsurance.benefititemid
            new TranslationSeedItem("entity.socialinsurance.benefititemid", "zh-CN", "福利项目ID", "福利项目（选项 TaktBenefitItems/options；通常 benefit_type 为社保/公积金，DictValue=Id）"),
            // entity.socialinsurance.benefititemid
            new TranslationSeedItem("entity.socialinsurance.benefititemid", "zh-HK", "福利项目ID_hk", "福利项目（选项 TaktBenefitItems/options；通常 benefit_type 为社保/公积金，DictValue=Id）"),

            // entity.socialinsurance.employeeid
            new TranslationSeedItem("entity.socialinsurance.employeeid", "en-US", "员工ID_us", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.socialinsurance.employeeid
            new TranslationSeedItem("entity.socialinsurance.employeeid", "ja-JP", "员工ID_jp", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.socialinsurance.employeeid
            new TranslationSeedItem("entity.socialinsurance.employeeid", "zh-CN", "员工ID", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.socialinsurance.employeeid
            new TranslationSeedItem("entity.socialinsurance.employeeid", "zh-HK", "员工ID_hk", "员工（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.socialinsurance.employeename
            new TranslationSeedItem("entity.socialinsurance.employeename", "en-US", "员工姓名_us", "员工姓名"),
            // entity.socialinsurance.employeename
            new TranslationSeedItem("entity.socialinsurance.employeename", "ja-JP", "员工姓名_jp", "员工姓名"),
            // entity.socialinsurance.employeename
            new TranslationSeedItem("entity.socialinsurance.employeename", "zh-CN", "员工姓名", "员工姓名"),
            // entity.socialinsurance.employeename
            new TranslationSeedItem("entity.socialinsurance.employeename", "zh-HK", "员工姓名_hk", "员工姓名"),

            // entity.socialinsurance.payperiod
            new TranslationSeedItem("entity.socialinsurance.payperiod", "en-US", "缴纳期间_us", "缴纳期间（如 2026-06）"),
            // entity.socialinsurance.payperiod
            new TranslationSeedItem("entity.socialinsurance.payperiod", "ja-JP", "缴纳期间_jp", "缴纳期间（如 2026-06）"),
            // entity.socialinsurance.payperiod
            new TranslationSeedItem("entity.socialinsurance.payperiod", "zh-CN", "缴纳期间", "缴纳期间（如 2026-06）"),
            // entity.socialinsurance.payperiod
            new TranslationSeedItem("entity.socialinsurance.payperiod", "zh-HK", "缴纳期间_hk", "缴纳期间（如 2026-06）"),

            // entity.socialinsurance.socialsecuritybase
            new TranslationSeedItem("entity.socialinsurance.socialsecuritybase", "en-US", "社保缴纳基数_us", "社保缴纳基数（元）"),
            // entity.socialinsurance.socialsecuritybase
            new TranslationSeedItem("entity.socialinsurance.socialsecuritybase", "ja-JP", "社保缴纳基数_jp", "社保缴纳基数（元）"),
            // entity.socialinsurance.socialsecuritybase
            new TranslationSeedItem("entity.socialinsurance.socialsecuritybase", "zh-CN", "社保缴纳基数", "社保缴纳基数（元）"),
            // entity.socialinsurance.socialsecuritybase
            new TranslationSeedItem("entity.socialinsurance.socialsecuritybase", "zh-HK", "社保缴纳基数_hk", "社保缴纳基数（元）"),

            // entity.socialinsurance.pensionamount
            new TranslationSeedItem("entity.socialinsurance.pensionamount", "en-US", "养老保险_us", "养老保险（元）"),
            // entity.socialinsurance.pensionamount
            new TranslationSeedItem("entity.socialinsurance.pensionamount", "ja-JP", "养老保险_jp", "养老保险（元）"),
            // entity.socialinsurance.pensionamount
            new TranslationSeedItem("entity.socialinsurance.pensionamount", "zh-CN", "养老保险", "养老保险（元）"),
            // entity.socialinsurance.pensionamount
            new TranslationSeedItem("entity.socialinsurance.pensionamount", "zh-HK", "养老保险_hk", "养老保险（元）"),

            // entity.socialinsurance.medicalamount
            new TranslationSeedItem("entity.socialinsurance.medicalamount", "en-US", "医疗保险_us", "医疗保险（元）"),
            // entity.socialinsurance.medicalamount
            new TranslationSeedItem("entity.socialinsurance.medicalamount", "ja-JP", "医疗保险_jp", "医疗保险（元）"),
            // entity.socialinsurance.medicalamount
            new TranslationSeedItem("entity.socialinsurance.medicalamount", "zh-CN", "医疗保险", "医疗保险（元）"),
            // entity.socialinsurance.medicalamount
            new TranslationSeedItem("entity.socialinsurance.medicalamount", "zh-HK", "医疗保险_hk", "医疗保险（元）"),

            // entity.socialinsurance.unemploymentamount
            new TranslationSeedItem("entity.socialinsurance.unemploymentamount", "en-US", "失业保险_us", "失业保险（元）"),
            // entity.socialinsurance.unemploymentamount
            new TranslationSeedItem("entity.socialinsurance.unemploymentamount", "ja-JP", "失业保险_jp", "失业保险（元）"),
            // entity.socialinsurance.unemploymentamount
            new TranslationSeedItem("entity.socialinsurance.unemploymentamount", "zh-CN", "失业保险", "失业保险（元）"),
            // entity.socialinsurance.unemploymentamount
            new TranslationSeedItem("entity.socialinsurance.unemploymentamount", "zh-HK", "失业保险_hk", "失业保险（元）"),

            // entity.socialinsurance.injuryamount
            new TranslationSeedItem("entity.socialinsurance.injuryamount", "en-US", "工伤保险_us", "工伤保险（元）"),
            // entity.socialinsurance.injuryamount
            new TranslationSeedItem("entity.socialinsurance.injuryamount", "ja-JP", "工伤保险_jp", "工伤保险（元）"),
            // entity.socialinsurance.injuryamount
            new TranslationSeedItem("entity.socialinsurance.injuryamount", "zh-CN", "工伤保险", "工伤保险（元）"),
            // entity.socialinsurance.injuryamount
            new TranslationSeedItem("entity.socialinsurance.injuryamount", "zh-HK", "工伤保险_hk", "工伤保险（元）"),

            // entity.socialinsurance.maternityamount
            new TranslationSeedItem("entity.socialinsurance.maternityamount", "en-US", "生育保险_us", "生育保险（元）"),
            // entity.socialinsurance.maternityamount
            new TranslationSeedItem("entity.socialinsurance.maternityamount", "ja-JP", "生育保险_jp", "生育保险（元）"),
            // entity.socialinsurance.maternityamount
            new TranslationSeedItem("entity.socialinsurance.maternityamount", "zh-CN", "生育保险", "生育保险（元）"),
            // entity.socialinsurance.maternityamount
            new TranslationSeedItem("entity.socialinsurance.maternityamount", "zh-HK", "生育保险_hk", "生育保险（元）"),

            // entity.socialinsurance.housingfundbase
            new TranslationSeedItem("entity.socialinsurance.housingfundbase", "en-US", "公积金缴纳基数_us", "公积金缴纳基数（元）"),
            // entity.socialinsurance.housingfundbase
            new TranslationSeedItem("entity.socialinsurance.housingfundbase", "ja-JP", "公积金缴纳基数_jp", "公积金缴纳基数（元）"),
            // entity.socialinsurance.housingfundbase
            new TranslationSeedItem("entity.socialinsurance.housingfundbase", "zh-CN", "公积金缴纳基数", "公积金缴纳基数（元）"),
            // entity.socialinsurance.housingfundbase
            new TranslationSeedItem("entity.socialinsurance.housingfundbase", "zh-HK", "公积金缴纳基数_hk", "公积金缴纳基数（元）"),

            // entity.socialinsurance.housingfundamount
            new TranslationSeedItem("entity.socialinsurance.housingfundamount", "en-US", "公积金_us", "公积金（元）"),
            // entity.socialinsurance.housingfundamount
            new TranslationSeedItem("entity.socialinsurance.housingfundamount", "ja-JP", "公积金_jp", "公积金（元）"),
            // entity.socialinsurance.housingfundamount
            new TranslationSeedItem("entity.socialinsurance.housingfundamount", "zh-CN", "公积金", "公积金（元）"),
            // entity.socialinsurance.housingfundamount
            new TranslationSeedItem("entity.socialinsurance.housingfundamount", "zh-HK", "公积金_hk", "公积金（元）"),

            // entity.socialinsurance.totalamount
            new TranslationSeedItem("entity.socialinsurance.totalamount", "en-US", "缴纳合计_us", "缴纳合计（元）"),
            // entity.socialinsurance.totalamount
            new TranslationSeedItem("entity.socialinsurance.totalamount", "ja-JP", "缴纳合计_jp", "缴纳合计（元）"),
            // entity.socialinsurance.totalamount
            new TranslationSeedItem("entity.socialinsurance.totalamount", "zh-CN", "缴纳合计", "缴纳合计（元）"),
            // entity.socialinsurance.totalamount
            new TranslationSeedItem("entity.socialinsurance.totalamount", "zh-HK", "缴纳合计_hk", "缴纳合计（元）"),

            // entity.socialinsurance.paystatus
            new TranslationSeedItem("entity.socialinsurance.paystatus", "en-US", "缴纳状态_us", "缴纳状态（字典 humanresource_benefits_social_insurance_pay_status；0=待缴纳 1=已缴纳 2=已补缴）"),
            // entity.socialinsurance.paystatus
            new TranslationSeedItem("entity.socialinsurance.paystatus", "ja-JP", "缴纳状态_jp", "缴纳状态（字典 humanresource_benefits_social_insurance_pay_status；0=待缴纳 1=已缴纳 2=已补缴）"),
            // entity.socialinsurance.paystatus
            new TranslationSeedItem("entity.socialinsurance.paystatus", "zh-CN", "缴纳状态", "缴纳状态（字典 humanresource_benefits_social_insurance_pay_status；0=待缴纳 1=已缴纳 2=已补缴）"),
            // entity.socialinsurance.paystatus
            new TranslationSeedItem("entity.socialinsurance.paystatus", "zh-HK", "缴纳状态_hk", "缴纳状态（字典 humanresource_benefits_social_insurance_pay_status；0=待缴纳 1=已缴纳 2=已补缴）"),
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
