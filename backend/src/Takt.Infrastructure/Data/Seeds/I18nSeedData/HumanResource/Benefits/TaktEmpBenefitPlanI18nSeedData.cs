// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Benefits
// 文件名称：TaktEmpBenefitPlanI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEmpBenefitPlan 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktEmpBenefitPlan 实体国际化翻译种子（键前缀 entity.empbenefitplan.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEmpBenefitPlanI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEmpBenefitPlan 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 empbenefitplan 实体翻译...", tenantCode);

        foreach (var item in GetEmpBenefitPlanTranslations())
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

        TaktLogger.Information("TaktEmpBenefitPlan 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEmpBenefitPlan 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.empbenefitplan._self / entity.empbenefitplan.{{field}}；ResourceGroup=Benefits；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEmpBenefitPlanTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.empbenefitplan._self
            new TranslationSeedItem("entity.empbenefitplan._self", "en-US", "Emp Benefit Plan Information_us", "实体名称"),
            // entity.empbenefitplan._self
            new TranslationSeedItem("entity.empbenefitplan._self", "ja-JP", "员工福利方案信息_jp", "实体名称"),
            // entity.empbenefitplan._self
            new TranslationSeedItem("entity.empbenefitplan._self", "zh-CN", "员工福利方案信息", "实体名称"),
            // entity.empbenefitplan._self
            new TranslationSeedItem("entity.empbenefitplan._self", "zh-HK", "员工福利方案信息_hk", "实体名称"),

            // entity.empbenefitplan.employeeid
            new TranslationSeedItem("entity.empbenefitplan.employeeid", "en-US", "员工ID_us", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.empbenefitplan.employeeid
            new TranslationSeedItem("entity.empbenefitplan.employeeid", "ja-JP", "员工ID_jp", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.empbenefitplan.employeeid
            new TranslationSeedItem("entity.empbenefitplan.employeeid", "zh-CN", "员工ID", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.empbenefitplan.employeeid
            new TranslationSeedItem("entity.empbenefitplan.employeeid", "zh-HK", "员工ID_hk", "员工（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.empbenefitplan.employeename
            new TranslationSeedItem("entity.empbenefitplan.employeename", "en-US", "员工姓名_us", "员工姓名"),
            // entity.empbenefitplan.employeename
            new TranslationSeedItem("entity.empbenefitplan.employeename", "ja-JP", "员工姓名_jp", "员工姓名"),
            // entity.empbenefitplan.employeename
            new TranslationSeedItem("entity.empbenefitplan.employeename", "zh-CN", "员工姓名", "员工姓名"),
            // entity.empbenefitplan.employeename
            new TranslationSeedItem("entity.empbenefitplan.employeename", "zh-HK", "员工姓名_hk", "员工姓名"),

            // entity.empbenefitplan.benefititemid
            new TranslationSeedItem("entity.empbenefitplan.benefititemid", "en-US", "福利项目ID_us", "福利项目（选项 TaktBenefitItems/options；DictValue=Id）"),
            // entity.empbenefitplan.benefititemid
            new TranslationSeedItem("entity.empbenefitplan.benefititemid", "ja-JP", "福利项目ID_jp", "福利项目（选项 TaktBenefitItems/options；DictValue=Id）"),
            // entity.empbenefitplan.benefititemid
            new TranslationSeedItem("entity.empbenefitplan.benefititemid", "zh-CN", "福利项目ID", "福利项目（选项 TaktBenefitItems/options；DictValue=Id）"),
            // entity.empbenefitplan.benefititemid
            new TranslationSeedItem("entity.empbenefitplan.benefititemid", "zh-HK", "福利项目ID_hk", "福利项目（选项 TaktBenefitItems/options；DictValue=Id）"),

            // entity.empbenefitplan.plancode
            new TranslationSeedItem("entity.empbenefitplan.plancode", "en-US", "方案编码_us", "方案编码（前端表单选择编码规则后自动通过 TaktNumbering 方案编码规则生成并展示，非手输；单据类型菜单：员工福利方案）"),
            // entity.empbenefitplan.plancode
            new TranslationSeedItem("entity.empbenefitplan.plancode", "ja-JP", "方案编码_jp", "方案编码（前端表单选择编码规则后自动通过 TaktNumbering 方案编码规则生成并展示，非手输；单据类型菜单：员工福利方案）"),
            // entity.empbenefitplan.plancode
            new TranslationSeedItem("entity.empbenefitplan.plancode", "zh-CN", "方案编码", "方案编码（前端表单选择编码规则后自动通过 TaktNumbering 方案编码规则生成并展示，非手输；单据类型菜单：员工福利方案）"),
            // entity.empbenefitplan.plancode
            new TranslationSeedItem("entity.empbenefitplan.plancode", "zh-HK", "方案编码_hk", "方案编码（前端表单选择编码规则后自动通过 TaktNumbering 方案编码规则生成并展示，非手输；单据类型菜单：员工福利方案）"),

            // entity.empbenefitplan.enrollmentdate
            new TranslationSeedItem("entity.empbenefitplan.enrollmentdate", "en-US", "参与日期_us", "参保/参与日期"),
            // entity.empbenefitplan.enrollmentdate
            new TranslationSeedItem("entity.empbenefitplan.enrollmentdate", "ja-JP", "参与日期_jp", "参保/参与日期"),
            // entity.empbenefitplan.enrollmentdate
            new TranslationSeedItem("entity.empbenefitplan.enrollmentdate", "zh-CN", "参与日期", "参保/参与日期"),
            // entity.empbenefitplan.enrollmentdate
            new TranslationSeedItem("entity.empbenefitplan.enrollmentdate", "zh-HK", "参与日期_hk", "参保/参与日期"),

            // entity.empbenefitplan.expirydate
            new TranslationSeedItem("entity.empbenefitplan.expirydate", "en-US", "失效日期_us", "失效日期"),
            // entity.empbenefitplan.expirydate
            new TranslationSeedItem("entity.empbenefitplan.expirydate", "ja-JP", "失效日期_jp", "失效日期"),
            // entity.empbenefitplan.expirydate
            new TranslationSeedItem("entity.empbenefitplan.expirydate", "zh-CN", "失效日期", "失效日期"),
            // entity.empbenefitplan.expirydate
            new TranslationSeedItem("entity.empbenefitplan.expirydate", "zh-HK", "失效日期_hk", "失效日期"),

            // entity.empbenefitplan.empbenefitstatus
            new TranslationSeedItem("entity.empbenefitplan.empbenefitstatus", "en-US", "状态_us", "状态（字典 humanresource_benefits_emp_benefit_plan_status；0=待生效 1=生效中 2=已失效）"),
            // entity.empbenefitplan.empbenefitstatus
            new TranslationSeedItem("entity.empbenefitplan.empbenefitstatus", "ja-JP", "状态_jp", "状态（字典 humanresource_benefits_emp_benefit_plan_status；0=待生效 1=生效中 2=已失效）"),
            // entity.empbenefitplan.empbenefitstatus
            new TranslationSeedItem("entity.empbenefitplan.empbenefitstatus", "zh-CN", "状态", "状态（字典 humanresource_benefits_emp_benefit_plan_status；0=待生效 1=生效中 2=已失效）"),
            // entity.empbenefitplan.empbenefitstatus
            new TranslationSeedItem("entity.empbenefitplan.empbenefitstatus", "zh-HK", "状态_hk", "状态（字典 humanresource_benefits_emp_benefit_plan_status；0=待生效 1=生效中 2=已失效）"),
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
