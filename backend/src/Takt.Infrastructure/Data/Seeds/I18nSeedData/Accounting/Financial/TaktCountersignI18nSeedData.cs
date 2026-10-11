// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Financial
// 文件名称：TaktCountersignI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktCountersign 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Financial;

/// <summary>
/// TaktCountersign 实体国际化翻译种子（键前缀 entity.countersign.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktCountersignI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktCountersign 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 countersign 实体翻译...", tenantCode);

        foreach (var item in GetCountersignTranslations())
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

        TaktLogger.Information("TaktCountersign 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktCountersign 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.countersign._self / entity.countersign.{{field}}；ResourceGroup=Financial；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetCountersignTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.countersign._self
            new TranslationSeedItem("entity.countersign._self", "en-US", "Countersign Information_us", "实体名称"),
            // entity.countersign._self
            new TranslationSeedItem("entity.countersign._self", "ja-JP", "会签单信息_jp", "实体名称"),
            // entity.countersign._self
            new TranslationSeedItem("entity.countersign._self", "zh-CN", "会签单信息", "实体名称"),
            // entity.countersign._self
            new TranslationSeedItem("entity.countersign._self", "zh-HK", "会签单信息_hk", "实体名称"),

            // entity.countersign.code
            new TranslationSeedItem("entity.countersign.code", "en-US", "会签编码_us", "会签编码"),
            // entity.countersign.code
            new TranslationSeedItem("entity.countersign.code", "ja-JP", "会签编码_jp", "会签编码"),
            // entity.countersign.code
            new TranslationSeedItem("entity.countersign.code", "zh-CN", "会签编码", "会签编码"),
            // entity.countersign.code
            new TranslationSeedItem("entity.countersign.code", "zh-HK", "会签编码_hk", "会签编码"),

            // entity.countersign.purchaseinquiryid
            new TranslationSeedItem("entity.countersign.purchaseinquiryid", "en-US", "来源采购询价ID_us", "来源采购询价（选项 TaktPurchaseInquiries/options；DictValue=Id）"),
            // entity.countersign.purchaseinquiryid
            new TranslationSeedItem("entity.countersign.purchaseinquiryid", "ja-JP", "来源采购询价ID_jp", "来源采购询价（选项 TaktPurchaseInquiries/options；DictValue=Id）"),
            // entity.countersign.purchaseinquiryid
            new TranslationSeedItem("entity.countersign.purchaseinquiryid", "zh-CN", "来源采购询价ID", "来源采购询价（选项 TaktPurchaseInquiries/options；DictValue=Id）"),
            // entity.countersign.purchaseinquiryid
            new TranslationSeedItem("entity.countersign.purchaseinquiryid", "zh-HK", "来源采购询价ID_hk", "来源采购询价（选项 TaktPurchaseInquiries/options；DictValue=Id）"),

            // entity.countersign.purchaseinquirycode
            new TranslationSeedItem("entity.countersign.purchaseinquirycode", "en-US", "来源采购询价编码_us", "来源采购询价编码（冗余：按 PurchaseInquiryId 取 TaktPurchaseInquiry.PurchaseInquiryCode 联动）"),
            // entity.countersign.purchaseinquirycode
            new TranslationSeedItem("entity.countersign.purchaseinquirycode", "ja-JP", "来源采购询价编码_jp", "来源采购询价编码（冗余：按 PurchaseInquiryId 取 TaktPurchaseInquiry.PurchaseInquiryCode 联动）"),
            // entity.countersign.purchaseinquirycode
            new TranslationSeedItem("entity.countersign.purchaseinquirycode", "zh-CN", "来源采购询价编码", "来源采购询价编码（冗余：按 PurchaseInquiryId 取 TaktPurchaseInquiry.PurchaseInquiryCode 联动）"),
            // entity.countersign.purchaseinquirycode
            new TranslationSeedItem("entity.countersign.purchaseinquirycode", "zh-HK", "来源采购询价编码_hk", "来源采购询价编码（冗余：按 PurchaseInquiryId 取 TaktPurchaseInquiry.PurchaseInquiryCode 联动）"),

            // entity.countersign.businesstype
            new TranslationSeedItem("entity.countersign.businesstype", "en-US", "会签业务类型_us", "会签业务类型（字典 accounting_financial_countersign_business_type：inquiry/pr/expense/standalone）"),
            // entity.countersign.businesstype
            new TranslationSeedItem("entity.countersign.businesstype", "ja-JP", "会签业务类型_jp", "会签业务类型（字典 accounting_financial_countersign_business_type：inquiry/pr/expense/standalone）"),
            // entity.countersign.businesstype
            new TranslationSeedItem("entity.countersign.businesstype", "zh-CN", "会签业务类型", "会签业务类型（字典 accounting_financial_countersign_business_type：inquiry/pr/expense/standalone）"),
            // entity.countersign.businesstype
            new TranslationSeedItem("entity.countersign.businesstype", "zh-HK", "会签业务类型_hk", "会签业务类型（字典 accounting_financial_countersign_business_type：inquiry/pr/expense/standalone）"),

            // entity.countersign.businesskey
            new TranslationSeedItem("entity.countersign.businesskey", "en-US", "会签业务键_us", "会签业务键（如 inquiry:123、pr:456、expense:789）"),
            // entity.countersign.businesskey
            new TranslationSeedItem("entity.countersign.businesskey", "ja-JP", "会签业务键_jp", "会签业务键（如 inquiry:123、pr:456、expense:789）"),
            // entity.countersign.businesskey
            new TranslationSeedItem("entity.countersign.businesskey", "zh-CN", "会签业务键", "会签业务键（如 inquiry:123、pr:456、expense:789）"),
            // entity.countersign.businesskey
            new TranslationSeedItem("entity.countersign.businesskey", "zh-HK", "会签业务键_hk", "会签业务键（如 inquiry:123、pr:456、expense:789）"),

            // entity.countersign.stepno
            new TranslationSeedItem("entity.countersign.stepno", "en-US", "会签步骤序号_us", "会签步骤序号（询价=1，PR=2，报销=3）"),
            // entity.countersign.stepno
            new TranslationSeedItem("entity.countersign.stepno", "ja-JP", "会签步骤序号_jp", "会签步骤序号（询价=1，PR=2，报销=3）"),
            // entity.countersign.stepno
            new TranslationSeedItem("entity.countersign.stepno", "zh-CN", "会签步骤序号", "会签步骤序号（询价=1，PR=2，报销=3）"),
            // entity.countersign.stepno
            new TranslationSeedItem("entity.countersign.stepno", "zh-HK", "会签步骤序号_hk", "会签步骤序号（询价=1，PR=2，报销=3）"),

            // entity.countersign.depts
            new TranslationSeedItem("entity.countersign.depts", "en-US", "会签部门_us", "会签部门（选项 TaktDepts/tree-options；DictValue=Id；多选 JSON）"),
            // entity.countersign.depts
            new TranslationSeedItem("entity.countersign.depts", "ja-JP", "会签部门_jp", "会签部门（选项 TaktDepts/tree-options；DictValue=Id；多选 JSON）"),
            // entity.countersign.depts
            new TranslationSeedItem("entity.countersign.depts", "zh-CN", "会签部门", "会签部门（选项 TaktDepts/tree-options；DictValue=Id；多选 JSON）"),
            // entity.countersign.depts
            new TranslationSeedItem("entity.countersign.depts", "zh-HK", "会签部门_hk", "会签部门（选项 TaktDepts/tree-options；DictValue=Id；多选 JSON）"),

            // entity.countersign.financedept
            new TranslationSeedItem("entity.countersign.financedept", "en-US", "财务部门_us", "财务部门 JSON"),
            // entity.countersign.financedept
            new TranslationSeedItem("entity.countersign.financedept", "ja-JP", "财务部门_jp", "财务部门 JSON"),
            // entity.countersign.financedept
            new TranslationSeedItem("entity.countersign.financedept", "zh-CN", "财务部门", "财务部门 JSON"),
            // entity.countersign.financedept
            new TranslationSeedItem("entity.countersign.financedept", "zh-HK", "财务部门_hk", "财务部门 JSON"),

            // entity.countersign.budgetreviewcomment
            new TranslationSeedItem("entity.countersign.budgetreviewcomment", "en-US", "预算审核意见_us", "预算审核意见"),
            // entity.countersign.budgetreviewcomment
            new TranslationSeedItem("entity.countersign.budgetreviewcomment", "ja-JP", "预算审核意见_jp", "预算审核意见"),
            // entity.countersign.budgetreviewcomment
            new TranslationSeedItem("entity.countersign.budgetreviewcomment", "zh-CN", "预算审核意见", "预算审核意见"),
            // entity.countersign.budgetreviewcomment
            new TranslationSeedItem("entity.countersign.budgetreviewcomment", "zh-HK", "预算审核意见_hk", "预算审核意见"),

            // entity.countersign.executiveoffice
            new TranslationSeedItem("entity.countersign.executiveoffice", "en-US", "总经室_us", "总经室 JSON"),
            // entity.countersign.executiveoffice
            new TranslationSeedItem("entity.countersign.executiveoffice", "ja-JP", "总经室_jp", "总经室 JSON"),
            // entity.countersign.executiveoffice
            new TranslationSeedItem("entity.countersign.executiveoffice", "zh-CN", "总经室", "总经室 JSON"),
            // entity.countersign.executiveoffice
            new TranslationSeedItem("entity.countersign.executiveoffice", "zh-HK", "总经室_hk", "总经室 JSON"),

            // entity.countersign.applicantby
            new TranslationSeedItem("entity.countersign.applicantby", "en-US", "申请人ID_us", "申请人（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.countersign.applicantby
            new TranslationSeedItem("entity.countersign.applicantby", "ja-JP", "申请人ID_jp", "申请人（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.countersign.applicantby
            new TranslationSeedItem("entity.countersign.applicantby", "zh-CN", "申请人ID", "申请人（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.countersign.applicantby
            new TranslationSeedItem("entity.countersign.applicantby", "zh-HK", "申请人ID_hk", "申请人（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.countersign.applicantname
            new TranslationSeedItem("entity.countersign.applicantname", "en-US", "申请人名称_us", "申请人名称（冗余：按 ApplicantBy 取 TaktEmployee.EmployeeName 联动）"),
            // entity.countersign.applicantname
            new TranslationSeedItem("entity.countersign.applicantname", "ja-JP", "申请人名称_jp", "申请人名称（冗余：按 ApplicantBy 取 TaktEmployee.EmployeeName 联动）"),
            // entity.countersign.applicantname
            new TranslationSeedItem("entity.countersign.applicantname", "zh-CN", "申请人名称", "申请人名称（冗余：按 ApplicantBy 取 TaktEmployee.EmployeeName 联动）"),
            // entity.countersign.applicantname
            new TranslationSeedItem("entity.countersign.applicantname", "zh-HK", "申请人名称_hk", "申请人名称（冗余：按 ApplicantBy 取 TaktEmployee.EmployeeName 联动）"),

            // entity.countersign.applicationdeptid
            new TranslationSeedItem("entity.countersign.applicationdeptid", "en-US", "申请部门ID_us", "申请部门（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.countersign.applicationdeptid
            new TranslationSeedItem("entity.countersign.applicationdeptid", "ja-JP", "申请部门ID_jp", "申请部门（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.countersign.applicationdeptid
            new TranslationSeedItem("entity.countersign.applicationdeptid", "zh-CN", "申请部门ID", "申请部门（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.countersign.applicationdeptid
            new TranslationSeedItem("entity.countersign.applicationdeptid", "zh-HK", "申请部门ID_hk", "申请部门（选项 TaktDepts/tree-options；DictValue=Id）"),

            // entity.countersign.applicationdeptname
            new TranslationSeedItem("entity.countersign.applicationdeptname", "en-US", "申请部门名称_us", "申请部门名称（冗余：按 ApplicationDeptId 取 TaktDept.DeptName1 联动）"),
            // entity.countersign.applicationdeptname
            new TranslationSeedItem("entity.countersign.applicationdeptname", "ja-JP", "申请部门名称_jp", "申请部门名称（冗余：按 ApplicationDeptId 取 TaktDept.DeptName1 联动）"),
            // entity.countersign.applicationdeptname
            new TranslationSeedItem("entity.countersign.applicationdeptname", "zh-CN", "申请部门名称", "申请部门名称（冗余：按 ApplicationDeptId 取 TaktDept.DeptName1 联动）"),
            // entity.countersign.applicationdeptname
            new TranslationSeedItem("entity.countersign.applicationdeptname", "zh-HK", "申请部门名称_hk", "申请部门名称（冗余：按 ApplicationDeptId 取 TaktDept.DeptName1 联动）"),

            // entity.countersign.costbearerdeptid
            new TranslationSeedItem("entity.countersign.costbearerdeptid", "en-US", "经费负担部门ID_us", "经费负担部门（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.countersign.costbearerdeptid
            new TranslationSeedItem("entity.countersign.costbearerdeptid", "ja-JP", "经费负担部门ID_jp", "经费负担部门（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.countersign.costbearerdeptid
            new TranslationSeedItem("entity.countersign.costbearerdeptid", "zh-CN", "经费负担部门ID", "经费负担部门（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.countersign.costbearerdeptid
            new TranslationSeedItem("entity.countersign.costbearerdeptid", "zh-HK", "经费负担部门ID_hk", "经费负担部门（选项 TaktDepts/tree-options；DictValue=Id）"),

            // entity.countersign.costbearerdeptname
            new TranslationSeedItem("entity.countersign.costbearerdeptname", "en-US", "经费负担部门名称_us", "经费负担部门名称（冗余：按 CostBearerDeptId 取 TaktDept.DeptName1 联动）"),
            // entity.countersign.costbearerdeptname
            new TranslationSeedItem("entity.countersign.costbearerdeptname", "ja-JP", "经费负担部门名称_jp", "经费负担部门名称（冗余：按 CostBearerDeptId 取 TaktDept.DeptName1 联动）"),
            // entity.countersign.costbearerdeptname
            new TranslationSeedItem("entity.countersign.costbearerdeptname", "zh-CN", "经费负担部门名称", "经费负担部门名称（冗余：按 CostBearerDeptId 取 TaktDept.DeptName1 联动）"),
            // entity.countersign.costbearerdeptname
            new TranslationSeedItem("entity.countersign.costbearerdeptname", "zh-HK", "经费负担部门名称_hk", "经费负担部门名称（冗余：按 CostBearerDeptId 取 TaktDept.DeptName1 联动）"),

            // entity.countersign.isbudget
            new TranslationSeedItem("entity.countersign.isbudget", "en-US", "预算否_us", "预算否（字典 sys_yes_no）"),
            // entity.countersign.isbudget
            new TranslationSeedItem("entity.countersign.isbudget", "ja-JP", "预算否_jp", "预算否（字典 sys_yes_no）"),
            // entity.countersign.isbudget
            new TranslationSeedItem("entity.countersign.isbudget", "zh-CN", "预算否", "预算否（字典 sys_yes_no）"),
            // entity.countersign.isbudget
            new TranslationSeedItem("entity.countersign.isbudget", "zh-HK", "预算否_hk", "预算否（字典 sys_yes_no）"),

            // entity.countersign.budgetitemid
            new TranslationSeedItem("entity.countersign.budgetitemid", "en-US", "预算项目ID_us", "预算项目（选项 TaktBudgetActuals/options；DictValue=Id）"),
            // entity.countersign.budgetitemid
            new TranslationSeedItem("entity.countersign.budgetitemid", "ja-JP", "预算项目ID_jp", "预算项目（选项 TaktBudgetActuals/options；DictValue=Id）"),
            // entity.countersign.budgetitemid
            new TranslationSeedItem("entity.countersign.budgetitemid", "zh-CN", "预算项目ID", "预算项目（选项 TaktBudgetActuals/options；DictValue=Id）"),
            // entity.countersign.budgetitemid
            new TranslationSeedItem("entity.countersign.budgetitemid", "zh-HK", "预算项目ID_hk", "预算项目（选项 TaktBudgetActuals/options；DictValue=Id）"),

            // entity.countersign.budgetitem
            new TranslationSeedItem("entity.countersign.budgetitem", "en-US", "预算项目名称_us", "预算项目名称（冗余：按 BudgetItemId 取 TaktBudgetActual.BudgetItemName 联动）"),
            // entity.countersign.budgetitem
            new TranslationSeedItem("entity.countersign.budgetitem", "ja-JP", "预算项目名称_jp", "预算项目名称（冗余：按 BudgetItemId 取 TaktBudgetActual.BudgetItemName 联动）"),
            // entity.countersign.budgetitem
            new TranslationSeedItem("entity.countersign.budgetitem", "zh-CN", "预算项目名称", "预算项目名称（冗余：按 BudgetItemId 取 TaktBudgetActual.BudgetItemName 联动）"),
            // entity.countersign.budgetitem
            new TranslationSeedItem("entity.countersign.budgetitem", "zh-HK", "预算项目名称_hk", "预算项目名称（冗余：按 BudgetItemId 取 TaktBudgetActual.BudgetItemName 联动）"),

            // entity.countersign.budgetamount
            new TranslationSeedItem("entity.countersign.budgetamount", "en-US", "预算金额_us", "预算金额（按 BudgetItemId 取 TaktBudgetActual.BudgetAmount 联动）"),
            // entity.countersign.budgetamount
            new TranslationSeedItem("entity.countersign.budgetamount", "ja-JP", "预算金额_jp", "预算金额（按 BudgetItemId 取 TaktBudgetActual.BudgetAmount 联动）"),
            // entity.countersign.budgetamount
            new TranslationSeedItem("entity.countersign.budgetamount", "zh-CN", "预算金额", "预算金额（按 BudgetItemId 取 TaktBudgetActual.BudgetAmount 联动）"),
            // entity.countersign.budgetamount
            new TranslationSeedItem("entity.countersign.budgetamount", "zh-HK", "预算金额_hk", "预算金额（按 BudgetItemId 取 TaktBudgetActual.BudgetAmount 联动）"),

            // entity.countersign.applicationamount
            new TranslationSeedItem("entity.countersign.applicationamount", "en-US", "申请金额_us", "申请金额"),
            // entity.countersign.applicationamount
            new TranslationSeedItem("entity.countersign.applicationamount", "ja-JP", "申请金额_jp", "申请金额"),
            // entity.countersign.applicationamount
            new TranslationSeedItem("entity.countersign.applicationamount", "zh-CN", "申请金额", "申请金额"),
            // entity.countersign.applicationamount
            new TranslationSeedItem("entity.countersign.applicationamount", "zh-HK", "申请金额_hk", "申请金额"),

            // entity.countersign.title
            new TranslationSeedItem("entity.countersign.title", "en-US", "标题_us", "标题"),
            // entity.countersign.title
            new TranslationSeedItem("entity.countersign.title", "ja-JP", "标题_jp", "标题"),
            // entity.countersign.title
            new TranslationSeedItem("entity.countersign.title", "zh-CN", "标题", "标题"),
            // entity.countersign.title
            new TranslationSeedItem("entity.countersign.title", "zh-HK", "标题_hk", "标题"),

            // entity.countersign.applicationreason
            new TranslationSeedItem("entity.countersign.applicationreason", "en-US", "申请原因_us", "申请原因"),
            // entity.countersign.applicationreason
            new TranslationSeedItem("entity.countersign.applicationreason", "ja-JP", "申请原因_jp", "申请原因"),
            // entity.countersign.applicationreason
            new TranslationSeedItem("entity.countersign.applicationreason", "zh-CN", "申请原因", "申请原因"),
            // entity.countersign.applicationreason
            new TranslationSeedItem("entity.countersign.applicationreason", "zh-HK", "申请原因_hk", "申请原因"),

            // entity.countersign.budgetusagedescription
            new TranslationSeedItem("entity.countersign.budgetusagedescription", "en-US", "预算使用说明_us", "预算使用说明"),
            // entity.countersign.budgetusagedescription
            new TranslationSeedItem("entity.countersign.budgetusagedescription", "ja-JP", "预算使用说明_jp", "预算使用说明"),
            // entity.countersign.budgetusagedescription
            new TranslationSeedItem("entity.countersign.budgetusagedescription", "zh-CN", "预算使用说明", "预算使用说明"),
            // entity.countersign.budgetusagedescription
            new TranslationSeedItem("entity.countersign.budgetusagedescription", "zh-HK", "预算使用说明_hk", "预算使用说明"),

            // entity.countersign.targetandexpectedbenefit
            new TranslationSeedItem("entity.countersign.targetandexpectedbenefit", "en-US", "目标与预期效益_us", "目标与预期效益"),
            // entity.countersign.targetandexpectedbenefit
            new TranslationSeedItem("entity.countersign.targetandexpectedbenefit", "ja-JP", "目标与预期效益_jp", "目标与预期效益"),
            // entity.countersign.targetandexpectedbenefit
            new TranslationSeedItem("entity.countersign.targetandexpectedbenefit", "zh-CN", "目标与预期效益", "目标与预期效益"),
            // entity.countersign.targetandexpectedbenefit
            new TranslationSeedItem("entity.countersign.targetandexpectedbenefit", "zh-HK", "目标与预期效益_hk", "目标与预期效益"),

            // entity.countersign.filename
            new TranslationSeedItem("entity.countersign.filename", "en-US", "文件名称_us", "文件名称（原始文件名，长度对齐 TaktFile.FileName）"),
            // entity.countersign.filename
            new TranslationSeedItem("entity.countersign.filename", "ja-JP", "文件名称_jp", "文件名称（原始文件名，长度对齐 TaktFile.FileName）"),
            // entity.countersign.filename
            new TranslationSeedItem("entity.countersign.filename", "zh-CN", "文件名称", "文件名称（原始文件名，长度对齐 TaktFile.FileName）"),
            // entity.countersign.filename
            new TranslationSeedItem("entity.countersign.filename", "zh-HK", "文件名称_hk", "文件名称（原始文件名，长度对齐 TaktFile.FileName）"),

            // entity.countersign.accessurl
            new TranslationSeedItem("entity.countersign.accessurl", "en-US", "访问地址_us", "访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）"),
            // entity.countersign.accessurl
            new TranslationSeedItem("entity.countersign.accessurl", "ja-JP", "访问地址_jp", "访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）"),
            // entity.countersign.accessurl
            new TranslationSeedItem("entity.countersign.accessurl", "zh-CN", "访问地址", "访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）"),
            // entity.countersign.accessurl
            new TranslationSeedItem("entity.countersign.accessurl", "zh-HK", "访问地址_hk", "访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）"),

            // entity.countersign.status
            new TranslationSeedItem("entity.countersign.status", "en-US", "会签单状态_us", "会签单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）"),
            // entity.countersign.status
            new TranslationSeedItem("entity.countersign.status", "ja-JP", "会签单状态_jp", "会签单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）"),
            // entity.countersign.status
            new TranslationSeedItem("entity.countersign.status", "zh-CN", "会签单状态", "会签单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）"),
            // entity.countersign.status
            new TranslationSeedItem("entity.countersign.status", "zh-HK", "会签单状态_hk", "会签单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）"),

            // entity.countersign.details
            new TranslationSeedItem("entity.countersign.details", "en-US", "会签单明细列表_us", "会签单明细列表（主子表关系）"),
            // entity.countersign.details
            new TranslationSeedItem("entity.countersign.details", "ja-JP", "会签单明细列表_jp", "会签单明细列表（主子表关系）"),
            // entity.countersign.details
            new TranslationSeedItem("entity.countersign.details", "zh-CN", "会签单明细列表", "会签单明细列表（主子表关系）"),
            // entity.countersign.details
            new TranslationSeedItem("entity.countersign.details", "zh-HK", "会签单明细列表_hk", "会签单明细列表（主子表关系）"),
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
        translation.ResourceGroup = "Financial";
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
