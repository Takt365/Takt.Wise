// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Compensation
// 文件名称：TaktPayslipI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPayslip 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Compensation;

/// <summary>
/// TaktPayslip 实体国际化翻译种子（键前缀 entity.payslip.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPayslipI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPayslip 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 payslip 实体翻译...", tenantCode);

        foreach (var item in GetPayslipTranslations())
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

        TaktLogger.Information("TaktPayslip 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPayslip 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.payslip._self / entity.payslip.{{field}}；ResourceGroup=Compensation；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPayslipTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.payslip._self
            new TranslationSeedItem("entity.payslip._self", "en-US", "Payslip Information_us", "实体名称"),
            // entity.payslip._self
            new TranslationSeedItem("entity.payslip._self", "ja-JP", "员工工资条信息_jp", "实体名称"),
            // entity.payslip._self
            new TranslationSeedItem("entity.payslip._self", "zh-CN", "员工工资条信息", "实体名称"),
            // entity.payslip._self
            new TranslationSeedItem("entity.payslip._self", "zh-HK", "员工工资条信息_hk", "实体名称"),

            // entity.payslip.employeeid
            new TranslationSeedItem("entity.payslip.employeeid", "en-US", "员工ID_us", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.payslip.employeeid
            new TranslationSeedItem("entity.payslip.employeeid", "ja-JP", "员工ID_jp", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.payslip.employeeid
            new TranslationSeedItem("entity.payslip.employeeid", "zh-CN", "员工ID", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.payslip.employeeid
            new TranslationSeedItem("entity.payslip.employeeid", "zh-HK", "员工ID_hk", "员工（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.payslip.employeename
            new TranslationSeedItem("entity.payslip.employeename", "en-US", "员工姓名_us", "员工姓名"),
            // entity.payslip.employeename
            new TranslationSeedItem("entity.payslip.employeename", "ja-JP", "员工姓名_jp", "员工姓名"),
            // entity.payslip.employeename
            new TranslationSeedItem("entity.payslip.employeename", "zh-CN", "员工姓名", "员工姓名"),
            // entity.payslip.employeename
            new TranslationSeedItem("entity.payslip.employeename", "zh-HK", "员工姓名_hk", "员工姓名"),

            // entity.payslip.payperiod
            new TranslationSeedItem("entity.payslip.payperiod", "en-US", "发薪期间_us", "发薪期间（如 2026-06）"),
            // entity.payslip.payperiod
            new TranslationSeedItem("entity.payslip.payperiod", "ja-JP", "发薪期间_jp", "发薪期间（如 2026-06）"),
            // entity.payslip.payperiod
            new TranslationSeedItem("entity.payslip.payperiod", "zh-CN", "发薪期间", "发薪期间（如 2026-06）"),
            // entity.payslip.payperiod
            new TranslationSeedItem("entity.payslip.payperiod", "zh-HK", "发薪期间_hk", "发薪期间（如 2026-06）"),

            // entity.payslip.basesalary
            new TranslationSeedItem("entity.payslip.basesalary", "en-US", "基本工资_us", "基本工资（元）"),
            // entity.payslip.basesalary
            new TranslationSeedItem("entity.payslip.basesalary", "ja-JP", "基本工资_jp", "基本工资（元）"),
            // entity.payslip.basesalary
            new TranslationSeedItem("entity.payslip.basesalary", "zh-CN", "基本工资", "基本工资（元）"),
            // entity.payslip.basesalary
            new TranslationSeedItem("entity.payslip.basesalary", "zh-HK", "基本工资_hk", "基本工资（元）"),

            // entity.payslip.positionsalary
            new TranslationSeedItem("entity.payslip.positionsalary", "en-US", "岗位工资_us", "岗位工资（元）"),
            // entity.payslip.positionsalary
            new TranslationSeedItem("entity.payslip.positionsalary", "ja-JP", "岗位工资_jp", "岗位工资（元）"),
            // entity.payslip.positionsalary
            new TranslationSeedItem("entity.payslip.positionsalary", "zh-CN", "岗位工资", "岗位工资（元）"),
            // entity.payslip.positionsalary
            new TranslationSeedItem("entity.payslip.positionsalary", "zh-HK", "岗位工资_hk", "岗位工资（元）"),

            // entity.payslip.bonusamount
            new TranslationSeedItem("entity.payslip.bonusamount", "en-US", "绩效奖金_us", "绩效/奖金（元）"),
            // entity.payslip.bonusamount
            new TranslationSeedItem("entity.payslip.bonusamount", "ja-JP", "绩效奖金_jp", "绩效/奖金（元）"),
            // entity.payslip.bonusamount
            new TranslationSeedItem("entity.payslip.bonusamount", "zh-CN", "绩效奖金", "绩效/奖金（元）"),
            // entity.payslip.bonusamount
            new TranslationSeedItem("entity.payslip.bonusamount", "zh-HK", "绩效奖金_hk", "绩效/奖金（元）"),

            // entity.payslip.overtimepay
            new TranslationSeedItem("entity.payslip.overtimepay", "en-US", "加班费_us", "加班费（元）"),
            // entity.payslip.overtimepay
            new TranslationSeedItem("entity.payslip.overtimepay", "ja-JP", "加班费_jp", "加班费（元）"),
            // entity.payslip.overtimepay
            new TranslationSeedItem("entity.payslip.overtimepay", "zh-CN", "加班费", "加班费（元）"),
            // entity.payslip.overtimepay
            new TranslationSeedItem("entity.payslip.overtimepay", "zh-HK", "加班费_hk", "加班费（元）"),

            // entity.payslip.allowancetotal
            new TranslationSeedItem("entity.payslip.allowancetotal", "en-US", "津贴合计_us", "津贴合计（元）"),
            // entity.payslip.allowancetotal
            new TranslationSeedItem("entity.payslip.allowancetotal", "ja-JP", "津贴合计_jp", "津贴合计（元）"),
            // entity.payslip.allowancetotal
            new TranslationSeedItem("entity.payslip.allowancetotal", "zh-CN", "津贴合计", "津贴合计（元）"),
            // entity.payslip.allowancetotal
            new TranslationSeedItem("entity.payslip.allowancetotal", "zh-HK", "津贴合计_hk", "津贴合计（元）"),

            // entity.payslip.grossamount
            new TranslationSeedItem("entity.payslip.grossamount", "en-US", "应发合计_us", "应发合计（元）"),
            // entity.payslip.grossamount
            new TranslationSeedItem("entity.payslip.grossamount", "ja-JP", "应发合计_jp", "应发合计（元）"),
            // entity.payslip.grossamount
            new TranslationSeedItem("entity.payslip.grossamount", "zh-CN", "应发合计", "应发合计（元）"),
            // entity.payslip.grossamount
            new TranslationSeedItem("entity.payslip.grossamount", "zh-HK", "应发合计_hk", "应发合计（元）"),

            // entity.payslip.socialsecuritydeduction
            new TranslationSeedItem("entity.payslip.socialsecuritydeduction", "en-US", "社保扣款_us", "社保扣款（元）"),
            // entity.payslip.socialsecuritydeduction
            new TranslationSeedItem("entity.payslip.socialsecuritydeduction", "ja-JP", "社保扣款_jp", "社保扣款（元）"),
            // entity.payslip.socialsecuritydeduction
            new TranslationSeedItem("entity.payslip.socialsecuritydeduction", "zh-CN", "社保扣款", "社保扣款（元）"),
            // entity.payslip.socialsecuritydeduction
            new TranslationSeedItem("entity.payslip.socialsecuritydeduction", "zh-HK", "社保扣款_hk", "社保扣款（元）"),

            // entity.payslip.housingfunddeduction
            new TranslationSeedItem("entity.payslip.housingfunddeduction", "en-US", "公积金扣款_us", "公积金扣款（元）"),
            // entity.payslip.housingfunddeduction
            new TranslationSeedItem("entity.payslip.housingfunddeduction", "ja-JP", "公积金扣款_jp", "公积金扣款（元）"),
            // entity.payslip.housingfunddeduction
            new TranslationSeedItem("entity.payslip.housingfunddeduction", "zh-CN", "公积金扣款", "公积金扣款（元）"),
            // entity.payslip.housingfunddeduction
            new TranslationSeedItem("entity.payslip.housingfunddeduction", "zh-HK", "公积金扣款_hk", "公积金扣款（元）"),

            // entity.payslip.taxdeduction
            new TranslationSeedItem("entity.payslip.taxdeduction", "en-US", "个税扣款_us", "个税扣款（元）"),
            // entity.payslip.taxdeduction
            new TranslationSeedItem("entity.payslip.taxdeduction", "ja-JP", "个税扣款_jp", "个税扣款（元）"),
            // entity.payslip.taxdeduction
            new TranslationSeedItem("entity.payslip.taxdeduction", "zh-CN", "个税扣款", "个税扣款（元）"),
            // entity.payslip.taxdeduction
            new TranslationSeedItem("entity.payslip.taxdeduction", "zh-HK", "个税扣款_hk", "个税扣款（元）"),

            // entity.payslip.otherdeduction
            new TranslationSeedItem("entity.payslip.otherdeduction", "en-US", "其他扣款_us", "其他扣款（元）"),
            // entity.payslip.otherdeduction
            new TranslationSeedItem("entity.payslip.otherdeduction", "ja-JP", "其他扣款_jp", "其他扣款（元）"),
            // entity.payslip.otherdeduction
            new TranslationSeedItem("entity.payslip.otherdeduction", "zh-CN", "其他扣款", "其他扣款（元）"),
            // entity.payslip.otherdeduction
            new TranslationSeedItem("entity.payslip.otherdeduction", "zh-HK", "其他扣款_hk", "其他扣款（元）"),

            // entity.payslip.netamount
            new TranslationSeedItem("entity.payslip.netamount", "en-US", "实发金额_us", "实发金额（元）"),
            // entity.payslip.netamount
            new TranslationSeedItem("entity.payslip.netamount", "ja-JP", "实发金额_jp", "实发金额（元）"),
            // entity.payslip.netamount
            new TranslationSeedItem("entity.payslip.netamount", "zh-CN", "实发金额", "实发金额（元）"),
            // entity.payslip.netamount
            new TranslationSeedItem("entity.payslip.netamount", "zh-HK", "实发金额_hk", "实发金额（元）"),

            // entity.payslip.formulasetcode
            new TranslationSeedItem("entity.payslip.formulasetcode", "en-US", "公式方案编码_us", "公式方案编码（关联 TaktSalaryFormula.SetCode，核算时按同编码多行步骤顺序执行）"),
            // entity.payslip.formulasetcode
            new TranslationSeedItem("entity.payslip.formulasetcode", "ja-JP", "公式方案编码_jp", "公式方案编码（关联 TaktSalaryFormula.SetCode，核算时按同编码多行步骤顺序执行）"),
            // entity.payslip.formulasetcode
            new TranslationSeedItem("entity.payslip.formulasetcode", "zh-CN", "公式方案编码", "公式方案编码（关联 TaktSalaryFormula.SetCode，核算时按同编码多行步骤顺序执行）"),
            // entity.payslip.formulasetcode
            new TranslationSeedItem("entity.payslip.formulasetcode", "zh-HK", "公式方案编码_hk", "公式方案编码（关联 TaktSalaryFormula.SetCode，核算时按同编码多行步骤顺序执行）"),

            // entity.payslip.issuedate
            new TranslationSeedItem("entity.payslip.issuedate", "en-US", "发放日期_us", "发放日期"),
            // entity.payslip.issuedate
            new TranslationSeedItem("entity.payslip.issuedate", "ja-JP", "发放日期_jp", "发放日期"),
            // entity.payslip.issuedate
            new TranslationSeedItem("entity.payslip.issuedate", "zh-CN", "发放日期", "发放日期"),
            // entity.payslip.issuedate
            new TranslationSeedItem("entity.payslip.issuedate", "zh-HK", "发放日期_hk", "发放日期"),

            // entity.payslip.issuestatus
            new TranslationSeedItem("entity.payslip.issuestatus", "en-US", "发放状态_us", "发放状态（字典 humanresource_compensation_payslip_issue_status；0=待发放 1=已发放 2=已确认）"),
            // entity.payslip.issuestatus
            new TranslationSeedItem("entity.payslip.issuestatus", "ja-JP", "发放状态_jp", "发放状态（字典 humanresource_compensation_payslip_issue_status；0=待发放 1=已发放 2=已确认）"),
            // entity.payslip.issuestatus
            new TranslationSeedItem("entity.payslip.issuestatus", "zh-CN", "发放状态", "发放状态（字典 humanresource_compensation_payslip_issue_status；0=待发放 1=已发放 2=已确认）"),
            // entity.payslip.issuestatus
            new TranslationSeedItem("entity.payslip.issuestatus", "zh-HK", "发放状态_hk", "发放状态（字典 humanresource_compensation_payslip_issue_status；0=待发放 1=已发放 2=已确认）"),
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
        translation.ResourceGroup = "Compensation";
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
