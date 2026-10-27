// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Financial
// 文件名称：TaktExpenseI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktExpense 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktExpense 实体国际化翻译种子（键前缀 entity.expense.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktExpenseI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktExpense 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 expense 实体翻译...", tenantCode);

        foreach (var item in GetExpenseTranslations())
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

        TaktLogger.Information("TaktExpense 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktExpense 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.expense._self / entity.expense.{{field}}；ResourceGroup=Financial；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetExpenseTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.expense._self
            new TranslationSeedItem("entity.expense._self", "en-US", "Expense Information_us", "实体名称"),
            // entity.expense._self
            new TranslationSeedItem("entity.expense._self", "ja-JP", "费用单信息_jp", "实体名称"),
            // entity.expense._self
            new TranslationSeedItem("entity.expense._self", "zh-CN", "费用单信息", "实体名称"),
            // entity.expense._self
            new TranslationSeedItem("entity.expense._self", "zh-HK", "费用单信息_hk", "实体名称"),

            // entity.expense.code
            new TranslationSeedItem("entity.expense.code", "en-US", "费用单编码_us", "费用单编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 费用单编码规则生成并展示，非手输；单据类型菜单：费用管理）"),
            // entity.expense.code
            new TranslationSeedItem("entity.expense.code", "ja-JP", "费用单编码_jp", "费用单编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 费用单编码规则生成并展示，非手输；单据类型菜单：费用管理）"),
            // entity.expense.code
            new TranslationSeedItem("entity.expense.code", "zh-CN", "费用单编码", "费用单编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 费用单编码规则生成并展示，非手输；单据类型菜单：费用管理）"),
            // entity.expense.code
            new TranslationSeedItem("entity.expense.code", "zh-HK", "费用单编码_hk", "费用单编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 费用单编码规则生成并展示，非手输；单据类型菜单：费用管理）"),

            // entity.expense.title
            new TranslationSeedItem("entity.expense.title", "en-US", "费用标题_us", "费用标题"),
            // entity.expense.title
            new TranslationSeedItem("entity.expense.title", "ja-JP", "费用标题_jp", "费用标题"),
            // entity.expense.title
            new TranslationSeedItem("entity.expense.title", "zh-CN", "费用标题", "费用标题"),
            // entity.expense.title
            new TranslationSeedItem("entity.expense.title", "zh-HK", "费用标题_hk", "费用标题"),

            // entity.expense.type
            new TranslationSeedItem("entity.expense.type", "en-US", "费用类型_us", "费用类型（字典 accounting_financial_expense_type：1=月结供应商除原材料外的费用，2=月结供应商货款及公司其他费用，3=杂项购置费用）"),
            // entity.expense.type
            new TranslationSeedItem("entity.expense.type", "ja-JP", "费用类型_jp", "费用类型（字典 accounting_financial_expense_type：1=月结供应商除原材料外的费用，2=月结供应商货款及公司其他费用，3=杂项购置费用）"),
            // entity.expense.type
            new TranslationSeedItem("entity.expense.type", "zh-CN", "费用类型", "费用类型（字典 accounting_financial_expense_type：1=月结供应商除原材料外的费用，2=月结供应商货款及公司其他费用，3=杂项购置费用）"),
            // entity.expense.type
            new TranslationSeedItem("entity.expense.type", "zh-HK", "费用类型_hk", "费用类型（字典 accounting_financial_expense_type：1=月结供应商除原材料外的费用，2=月结供应商货款及公司其他费用，3=杂项购置费用）"),

            // entity.expense.suppliercode
            new TranslationSeedItem("entity.expense.suppliercode", "en-US", "供应商编码_us", "供应商编码（选项 TaktSuppliers/options；整单唯一，DictValue=Id）"),
            // entity.expense.suppliercode
            new TranslationSeedItem("entity.expense.suppliercode", "ja-JP", "供应商编码_jp", "供应商编码（选项 TaktSuppliers/options；整单唯一，DictValue=Id）"),
            // entity.expense.suppliercode
            new TranslationSeedItem("entity.expense.suppliercode", "zh-CN", "供应商编码", "供应商编码（选项 TaktSuppliers/options；整单唯一，DictValue=Id）"),
            // entity.expense.suppliercode
            new TranslationSeedItem("entity.expense.suppliercode", "zh-HK", "供应商编码_hk", "供应商编码（选项 TaktSuppliers/options；整单唯一，DictValue=Id）"),

            // entity.expense.suppliername1
            new TranslationSeedItem("entity.expense.suppliername1", "en-US", "供应商名称1_us", "供应商名称（整单唯一）"),
            // entity.expense.suppliername1
            new TranslationSeedItem("entity.expense.suppliername1", "ja-JP", "供应商名称1_jp", "供应商名称（整单唯一）"),
            // entity.expense.suppliername1
            new TranslationSeedItem("entity.expense.suppliername1", "zh-CN", "供应商名称1", "供应商名称（整单唯一）"),
            // entity.expense.suppliername1
            new TranslationSeedItem("entity.expense.suppliername1", "zh-HK", "供应商名称1_hk", "供应商名称（整单唯一）"),

            // entity.expense.applicantby
            new TranslationSeedItem("entity.expense.applicantby", "en-US", "申请人ID_us", "申请人（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.expense.applicantby
            new TranslationSeedItem("entity.expense.applicantby", "ja-JP", "申请人ID_jp", "申请人（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.expense.applicantby
            new TranslationSeedItem("entity.expense.applicantby", "zh-CN", "申请人ID", "申请人（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.expense.applicantby
            new TranslationSeedItem("entity.expense.applicantby", "zh-HK", "申请人ID_hk", "申请人（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.expense.applicantname
            new TranslationSeedItem("entity.expense.applicantname", "en-US", "申请人名称_us", "申请人名称（冗余：按 ApplicantBy 取 TaktEmployee.EmployeeName 联动）"),
            // entity.expense.applicantname
            new TranslationSeedItem("entity.expense.applicantname", "ja-JP", "申请人名称_jp", "申请人名称（冗余：按 ApplicantBy 取 TaktEmployee.EmployeeName 联动）"),
            // entity.expense.applicantname
            new TranslationSeedItem("entity.expense.applicantname", "zh-CN", "申请人名称", "申请人名称（冗余：按 ApplicantBy 取 TaktEmployee.EmployeeName 联动）"),
            // entity.expense.applicantname
            new TranslationSeedItem("entity.expense.applicantname", "zh-HK", "申请人名称_hk", "申请人名称（冗余：按 ApplicantBy 取 TaktEmployee.EmployeeName 联动）"),

            // entity.expense.applicationdeptid
            new TranslationSeedItem("entity.expense.applicationdeptid", "en-US", "申请部门ID_us", "申请部门（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.expense.applicationdeptid
            new TranslationSeedItem("entity.expense.applicationdeptid", "ja-JP", "申请部门ID_jp", "申请部门（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.expense.applicationdeptid
            new TranslationSeedItem("entity.expense.applicationdeptid", "zh-CN", "申请部门ID", "申请部门（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.expense.applicationdeptid
            new TranslationSeedItem("entity.expense.applicationdeptid", "zh-HK", "申请部门ID_hk", "申请部门（选项 TaktDepts/tree-options；DictValue=Id）"),

            // entity.expense.applicationdeptname
            new TranslationSeedItem("entity.expense.applicationdeptname", "en-US", "申请部门名称_us", "申请部门名称（冗余：按 ApplicationDeptId 取 TaktDept.DeptName1 联动）"),
            // entity.expense.applicationdeptname
            new TranslationSeedItem("entity.expense.applicationdeptname", "ja-JP", "申请部门名称_jp", "申请部门名称（冗余：按 ApplicationDeptId 取 TaktDept.DeptName1 联动）"),
            // entity.expense.applicationdeptname
            new TranslationSeedItem("entity.expense.applicationdeptname", "zh-CN", "申请部门名称", "申请部门名称（冗余：按 ApplicationDeptId 取 TaktDept.DeptName1 联动）"),
            // entity.expense.applicationdeptname
            new TranslationSeedItem("entity.expense.applicationdeptname", "zh-HK", "申请部门名称_hk", "申请部门名称（冗余：按 ApplicationDeptId 取 TaktDept.DeptName1 联动）"),

            // entity.expense.costbearerdeptid
            new TranslationSeedItem("entity.expense.costbearerdeptid", "en-US", "经费负担部门ID_us", "经费负担部门（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.expense.costbearerdeptid
            new TranslationSeedItem("entity.expense.costbearerdeptid", "ja-JP", "经费负担部门ID_jp", "经费负担部门（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.expense.costbearerdeptid
            new TranslationSeedItem("entity.expense.costbearerdeptid", "zh-CN", "经费负担部门ID", "经费负担部门（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.expense.costbearerdeptid
            new TranslationSeedItem("entity.expense.costbearerdeptid", "zh-HK", "经费负担部门ID_hk", "经费负担部门（选项 TaktDepts/tree-options；DictValue=Id）"),

            // entity.expense.costbearerdeptname
            new TranslationSeedItem("entity.expense.costbearerdeptname", "en-US", "经费负担部门名称_us", "经费负担部门名称（冗余：按 CostBearerDeptId 取 TaktDept.DeptName1 联动）"),
            // entity.expense.costbearerdeptname
            new TranslationSeedItem("entity.expense.costbearerdeptname", "ja-JP", "经费负担部门名称_jp", "经费负担部门名称（冗余：按 CostBearerDeptId 取 TaktDept.DeptName1 联动）"),
            // entity.expense.costbearerdeptname
            new TranslationSeedItem("entity.expense.costbearerdeptname", "zh-CN", "经费负担部门名称", "经费负担部门名称（冗余：按 CostBearerDeptId 取 TaktDept.DeptName1 联动）"),
            // entity.expense.costbearerdeptname
            new TranslationSeedItem("entity.expense.costbearerdeptname", "zh-HK", "经费负担部门名称_hk", "经费负担部门名称（冗余：按 CostBearerDeptId 取 TaktDept.DeptName1 联动）"),

            // entity.expense.costcenter
            new TranslationSeedItem("entity.expense.costcenter", "en-US", "成本中心_us", "成本中心（关联 TaktCostCenter.CostCenterCode，选项 TaktCostCenters/tree-options）"),
            // entity.expense.costcenter
            new TranslationSeedItem("entity.expense.costcenter", "ja-JP", "成本中心_jp", "成本中心（关联 TaktCostCenter.CostCenterCode，选项 TaktCostCenters/tree-options）"),
            // entity.expense.costcenter
            new TranslationSeedItem("entity.expense.costcenter", "zh-CN", "成本中心", "成本中心（关联 TaktCostCenter.CostCenterCode，选项 TaktCostCenters/tree-options）"),
            // entity.expense.costcenter
            new TranslationSeedItem("entity.expense.costcenter", "zh-HK", "成本中心_hk", "成本中心（关联 TaktCostCenter.CostCenterCode，选项 TaktCostCenters/tree-options）"),

            // entity.expense.countersignid
            new TranslationSeedItem("entity.expense.countersignid", "en-US", "关联会签单ID_us", "关联会签单（选项 TaktCountersigns/options；DictValue=Id）"),
            // entity.expense.countersignid
            new TranslationSeedItem("entity.expense.countersignid", "ja-JP", "关联会签单ID_jp", "关联会签单（选项 TaktCountersigns/options；DictValue=Id）"),
            // entity.expense.countersignid
            new TranslationSeedItem("entity.expense.countersignid", "zh-CN", "关联会签单ID", "关联会签单（选项 TaktCountersigns/options；DictValue=Id）"),
            // entity.expense.countersignid
            new TranslationSeedItem("entity.expense.countersignid", "zh-HK", "关联会签单ID_hk", "关联会签单（选项 TaktCountersigns/options；DictValue=Id）"),

            // entity.expense.purchaseordercode
            new TranslationSeedItem("entity.expense.purchaseordercode", "en-US", "来源采购订单编码_us", "来源采购订单编码（选项 TaktPurchaseOrders/options；采购链路自动生成时写入，DictValue=Id）"),
            // entity.expense.purchaseordercode
            new TranslationSeedItem("entity.expense.purchaseordercode", "ja-JP", "来源采购订单编码_jp", "来源采购订单编码（选项 TaktPurchaseOrders/options；采购链路自动生成时写入，DictValue=Id）"),
            // entity.expense.purchaseordercode
            new TranslationSeedItem("entity.expense.purchaseordercode", "zh-CN", "来源采购订单编码", "来源采购订单编码（选项 TaktPurchaseOrders/options；采购链路自动生成时写入，DictValue=Id）"),
            // entity.expense.purchaseordercode
            new TranslationSeedItem("entity.expense.purchaseordercode", "zh-HK", "来源采购订单编码_hk", "来源采购订单编码（选项 TaktPurchaseOrders/options；采购链路自动生成时写入，DictValue=Id）"),

            // entity.expense.purchaserequestcode
            new TranslationSeedItem("entity.expense.purchaserequestcode", "en-US", "来源采购申请编码_us", "来源采购申请编码（选项 TaktPurchaseRequests/options；采购链路自动生成时写入，DictValue=Id）"),
            // entity.expense.purchaserequestcode
            new TranslationSeedItem("entity.expense.purchaserequestcode", "ja-JP", "来源采购申请编码_jp", "来源采购申请编码（选项 TaktPurchaseRequests/options；采购链路自动生成时写入，DictValue=Id）"),
            // entity.expense.purchaserequestcode
            new TranslationSeedItem("entity.expense.purchaserequestcode", "zh-CN", "来源采购申请编码", "来源采购申请编码（选项 TaktPurchaseRequests/options；采购链路自动生成时写入，DictValue=Id）"),
            // entity.expense.purchaserequestcode
            new TranslationSeedItem("entity.expense.purchaserequestcode", "zh-HK", "来源采购申请编码_hk", "来源采购申请编码（选项 TaktPurchaseRequests/options；采购链路自动生成时写入，DictValue=Id）"),

            // entity.expense.amount
            new TranslationSeedItem("entity.expense.amount", "en-US", "费用金额_us", "费用金额"),
            // entity.expense.amount
            new TranslationSeedItem("entity.expense.amount", "ja-JP", "费用金额_jp", "费用金额"),
            // entity.expense.amount
            new TranslationSeedItem("entity.expense.amount", "zh-CN", "费用金额", "费用金额"),
            // entity.expense.amount
            new TranslationSeedItem("entity.expense.amount", "zh-HK", "费用金额_hk", "费用金额"),

            // entity.expense.taxrate
            new TranslationSeedItem("entity.expense.taxrate", "en-US", "税率_us", "税率（字典 accounting_financial_tax_rate_param；整单统一税率）"),
            // entity.expense.taxrate
            new TranslationSeedItem("entity.expense.taxrate", "ja-JP", "税率_jp", "税率（字典 accounting_financial_tax_rate_param；整单统一税率）"),
            // entity.expense.taxrate
            new TranslationSeedItem("entity.expense.taxrate", "zh-CN", "税率", "税率（字典 accounting_financial_tax_rate_param；整单统一税率）"),
            // entity.expense.taxrate
            new TranslationSeedItem("entity.expense.taxrate", "zh-HK", "税率_hk", "税率（字典 accounting_financial_tax_rate_param；整单统一税率）"),

            // entity.expense.taxamount
            new TranslationSeedItem("entity.expense.taxamount", "en-US", "税额_us", "税额（整单合计）"),
            // entity.expense.taxamount
            new TranslationSeedItem("entity.expense.taxamount", "ja-JP", "税额_jp", "税额（整单合计）"),
            // entity.expense.taxamount
            new TranslationSeedItem("entity.expense.taxamount", "zh-CN", "税额", "税额（整单合计）"),
            // entity.expense.taxamount
            new TranslationSeedItem("entity.expense.taxamount", "zh-HK", "税额_hk", "税额（整单合计）"),

            // entity.expense.date
            new TranslationSeedItem("entity.expense.date", "en-US", "费用发生日期_us", "费用发生日期"),
            // entity.expense.date
            new TranslationSeedItem("entity.expense.date", "ja-JP", "费用发生日期_jp", "费用发生日期"),
            // entity.expense.date
            new TranslationSeedItem("entity.expense.date", "zh-CN", "费用发生日期", "费用发生日期"),
            // entity.expense.date
            new TranslationSeedItem("entity.expense.date", "zh-HK", "费用发生日期_hk", "费用发生日期"),

            // entity.expense.applicationreason
            new TranslationSeedItem("entity.expense.applicationreason", "en-US", "申请原因_us", "申请原因"),
            // entity.expense.applicationreason
            new TranslationSeedItem("entity.expense.applicationreason", "ja-JP", "申请原因_jp", "申请原因"),
            // entity.expense.applicationreason
            new TranslationSeedItem("entity.expense.applicationreason", "zh-CN", "申请原因", "申请原因"),
            // entity.expense.applicationreason
            new TranslationSeedItem("entity.expense.applicationreason", "zh-HK", "申请原因_hk", "申请原因"),

            // entity.expense.filename
            new TranslationSeedItem("entity.expense.filename", "en-US", "文件名称_us", "文件名称（原始文件名，长度对齐 TaktFile.FileName）"),
            // entity.expense.filename
            new TranslationSeedItem("entity.expense.filename", "ja-JP", "文件名称_jp", "文件名称（原始文件名，长度对齐 TaktFile.FileName）"),
            // entity.expense.filename
            new TranslationSeedItem("entity.expense.filename", "zh-CN", "文件名称", "文件名称（原始文件名，长度对齐 TaktFile.FileName）"),
            // entity.expense.filename
            new TranslationSeedItem("entity.expense.filename", "zh-HK", "文件名称_hk", "文件名称（原始文件名，长度对齐 TaktFile.FileName）"),

            // entity.expense.accessurl
            new TranslationSeedItem("entity.expense.accessurl", "en-US", "访问地址_us", "访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）"),
            // entity.expense.accessurl
            new TranslationSeedItem("entity.expense.accessurl", "ja-JP", "访问地址_jp", "访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）"),
            // entity.expense.accessurl
            new TranslationSeedItem("entity.expense.accessurl", "zh-CN", "访问地址", "访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）"),
            // entity.expense.accessurl
            new TranslationSeedItem("entity.expense.accessurl", "zh-HK", "访问地址_hk", "访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）"),

            // entity.expense.status
            new TranslationSeedItem("entity.expense.status", "en-US", "费用单状态_us", "费用单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）"),
            // entity.expense.status
            new TranslationSeedItem("entity.expense.status", "ja-JP", "费用单状态_jp", "费用单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）"),
            // entity.expense.status
            new TranslationSeedItem("entity.expense.status", "zh-CN", "费用单状态", "费用单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）"),
            // entity.expense.status
            new TranslationSeedItem("entity.expense.status", "zh-HK", "费用单状态_hk", "费用单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）"),

            // entity.expense.details
            new TranslationSeedItem("entity.expense.details", "en-US", "费用单明细列表_us", "费用单明细列表（主子表关系）"),
            // entity.expense.details
            new TranslationSeedItem("entity.expense.details", "ja-JP", "费用单明细列表_jp", "费用单明细列表（主子表关系）"),
            // entity.expense.details
            new TranslationSeedItem("entity.expense.details", "zh-CN", "费用单明细列表", "费用单明细列表（主子表关系）"),
            // entity.expense.details
            new TranslationSeedItem("entity.expense.details", "zh-HK", "费用单明细列表_hk", "费用单明细列表（主子表关系）"),
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
