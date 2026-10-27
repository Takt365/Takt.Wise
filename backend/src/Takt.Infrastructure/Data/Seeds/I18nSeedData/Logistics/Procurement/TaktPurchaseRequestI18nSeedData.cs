// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Procurement
// 文件名称：TaktPurchaseRequestI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPurchaseRequest 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktPurchaseRequest 实体国际化翻译种子（键前缀 entity.purchaserequest.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPurchaseRequestI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPurchaseRequest 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 purchaserequest 实体翻译...", tenantCode);

        foreach (var item in GetPurchaseRequestTranslations())
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

        TaktLogger.Information("TaktPurchaseRequest 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPurchaseRequest 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.purchaserequest._self / entity.purchaserequest.{{field}}；ResourceGroup=Procurement；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPurchaseRequestTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.purchaserequest._self
            new TranslationSeedItem("entity.purchaserequest._self", "en-US", "Purchase Request Information_us", "实体名称"),
            // entity.purchaserequest._self
            new TranslationSeedItem("entity.purchaserequest._self", "ja-JP", "Takt采购申请信息_jp", "实体名称"),
            // entity.purchaserequest._self
            new TranslationSeedItem("entity.purchaserequest._self", "zh-CN", "Takt采购申请信息", "实体名称"),
            // entity.purchaserequest._self
            new TranslationSeedItem("entity.purchaserequest._self", "zh-HK", "Takt采购申请信息_hk", "实体名称"),

            // entity.purchaserequest.code
            new TranslationSeedItem("entity.purchaserequest.code", "en-US", "采购申请编码_us", "采购申请编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 采购申请编码规则生成并展示，非手输；单据类型菜单：采购申请）"),
            // entity.purchaserequest.code
            new TranslationSeedItem("entity.purchaserequest.code", "ja-JP", "采购申请编码_jp", "采购申请编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 采购申请编码规则生成并展示，非手输；单据类型菜单：采购申请）"),
            // entity.purchaserequest.code
            new TranslationSeedItem("entity.purchaserequest.code", "zh-CN", "采购申请编码", "采购申请编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 采购申请编码规则生成并展示，非手输；单据类型菜单：采购申请）"),
            // entity.purchaserequest.code
            new TranslationSeedItem("entity.purchaserequest.code", "zh-HK", "采购申请编码_hk", "采购申请编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 采购申请编码规则生成并展示，非手输；单据类型菜单：采购申请）"),

            // entity.purchaserequest.purchaseinquiryid
            new TranslationSeedItem("entity.purchaserequest.purchaseinquiryid", "en-US", "来源采购询价ID_us", "来源采购询价 ID（选项 TaktPurchaseInquirys/options；DictValue=Id）"),
            // entity.purchaserequest.purchaseinquiryid
            new TranslationSeedItem("entity.purchaserequest.purchaseinquiryid", "ja-JP", "来源采购询价ID_jp", "来源采购询价 ID（选项 TaktPurchaseInquirys/options；DictValue=Id）"),
            // entity.purchaserequest.purchaseinquiryid
            new TranslationSeedItem("entity.purchaserequest.purchaseinquiryid", "zh-CN", "来源采购询价ID", "来源采购询价 ID（选项 TaktPurchaseInquirys/options；DictValue=Id）"),
            // entity.purchaserequest.purchaseinquiryid
            new TranslationSeedItem("entity.purchaserequest.purchaseinquiryid", "zh-HK", "来源采购询价ID_hk", "来源采购询价 ID（选项 TaktPurchaseInquirys/options；DictValue=Id）"),

            // entity.purchaserequest.purchaseinquirycode
            new TranslationSeedItem("entity.purchaserequest.purchaseinquirycode", "en-US", "来源采购询价编码_us", "来源采购询价编码（冗余）"),
            // entity.purchaserequest.purchaseinquirycode
            new TranslationSeedItem("entity.purchaserequest.purchaseinquirycode", "ja-JP", "来源采购询价编码_jp", "来源采购询价编码（冗余）"),
            // entity.purchaserequest.purchaseinquirycode
            new TranslationSeedItem("entity.purchaserequest.purchaseinquirycode", "zh-CN", "来源采购询价编码", "来源采购询价编码（冗余）"),
            // entity.purchaserequest.purchaseinquirycode
            new TranslationSeedItem("entity.purchaserequest.purchaseinquirycode", "zh-HK", "来源采购询价编码_hk", "来源采购询价编码（冗余）"),

            // entity.purchaserequest.purchaseplanid
            new TranslationSeedItem("entity.purchaserequest.purchaseplanid", "en-US", "来源采购计划ID_us", "来源采购计划 ID（MRP 下推，关联 TaktPurchasePlan.Id）"),
            // entity.purchaserequest.purchaseplanid
            new TranslationSeedItem("entity.purchaserequest.purchaseplanid", "ja-JP", "来源采购计划ID_jp", "来源采购计划 ID（MRP 下推，关联 TaktPurchasePlan.Id）"),
            // entity.purchaserequest.purchaseplanid
            new TranslationSeedItem("entity.purchaserequest.purchaseplanid", "zh-CN", "来源采购计划ID", "来源采购计划 ID（MRP 下推，关联 TaktPurchasePlan.Id）"),
            // entity.purchaserequest.purchaseplanid
            new TranslationSeedItem("entity.purchaserequest.purchaseplanid", "zh-HK", "来源采购计划ID_hk", "来源采购计划 ID（MRP 下推，关联 TaktPurchasePlan.Id）"),

            // entity.purchaserequest.purchaseplancode
            new TranslationSeedItem("entity.purchaserequest.purchaseplancode", "en-US", "来源采购计划编码_us", "来源采购计划编码（冗余）"),
            // entity.purchaserequest.purchaseplancode
            new TranslationSeedItem("entity.purchaserequest.purchaseplancode", "ja-JP", "来源采购计划编码_jp", "来源采购计划编码（冗余）"),
            // entity.purchaserequest.purchaseplancode
            new TranslationSeedItem("entity.purchaserequest.purchaseplancode", "zh-CN", "来源采购计划编码", "来源采购计划编码（冗余）"),
            // entity.purchaserequest.purchaseplancode
            new TranslationSeedItem("entity.purchaserequest.purchaseplancode", "zh-HK", "来源采购计划编码_hk", "来源采购计划编码（冗余）"),

            // entity.purchaserequest.chainscheme
            new TranslationSeedItem("entity.purchaserequest.chainscheme", "en-US", "采购链路方案_us", "采购链路方案（字典 logistics_procurement_chain_scheme；1=方案一，2=方案二）"),
            // entity.purchaserequest.chainscheme
            new TranslationSeedItem("entity.purchaserequest.chainscheme", "ja-JP", "采购链路方案_jp", "采购链路方案（字典 logistics_procurement_chain_scheme；1=方案一，2=方案二）"),
            // entity.purchaserequest.chainscheme
            new TranslationSeedItem("entity.purchaserequest.chainscheme", "zh-CN", "采购链路方案", "采购链路方案（字典 logistics_procurement_chain_scheme；1=方案一，2=方案二）"),
            // entity.purchaserequest.chainscheme
            new TranslationSeedItem("entity.purchaserequest.chainscheme", "zh-HK", "采购链路方案_hk", "采购链路方案（字典 logistics_procurement_chain_scheme；1=方案一，2=方案二）"),

            // entity.purchaserequest.podecision
            new TranslationSeedItem("entity.purchaserequest.podecision", "en-US", "PO生成决策_us", "PO 生成决策（方案一：null=待决策，1=生成 PO，0=暂不生成 PO）"),
            // entity.purchaserequest.podecision
            new TranslationSeedItem("entity.purchaserequest.podecision", "ja-JP", "PO生成决策_jp", "PO 生成决策（方案一：null=待决策，1=生成 PO，0=暂不生成 PO）"),
            // entity.purchaserequest.podecision
            new TranslationSeedItem("entity.purchaserequest.podecision", "zh-CN", "PO生成决策", "PO 生成决策（方案一：null=待决策，1=生成 PO，0=暂不生成 PO）"),
            // entity.purchaserequest.podecision
            new TranslationSeedItem("entity.purchaserequest.podecision", "zh-HK", "PO生成决策_hk", "PO 生成决策（方案一：null=待决策，1=生成 PO，0=暂不生成 PO）"),

            // entity.purchaserequest.countersignid
            new TranslationSeedItem("entity.purchaserequest.countersignid", "en-US", "PR会签单ID_us", "PR 会签单 ID（选项 TaktCountersigns/options；DictValue=Id）"),
            // entity.purchaserequest.countersignid
            new TranslationSeedItem("entity.purchaserequest.countersignid", "ja-JP", "PR会签单ID_jp", "PR 会签单 ID（选项 TaktCountersigns/options；DictValue=Id）"),
            // entity.purchaserequest.countersignid
            new TranslationSeedItem("entity.purchaserequest.countersignid", "zh-CN", "PR会签单ID", "PR 会签单 ID（选项 TaktCountersigns/options；DictValue=Id）"),
            // entity.purchaserequest.countersignid
            new TranslationSeedItem("entity.purchaserequest.countersignid", "zh-HK", "PR会签单ID_hk", "PR 会签单 ID（选项 TaktCountersigns/options；DictValue=Id）"),

            // entity.purchaserequest.countersigncode
            new TranslationSeedItem("entity.purchaserequest.countersigncode", "en-US", "PR会签编码_us", "PR 会签编码（冗余）"),
            // entity.purchaserequest.countersigncode
            new TranslationSeedItem("entity.purchaserequest.countersigncode", "ja-JP", "PR会签编码_jp", "PR 会签编码（冗余）"),
            // entity.purchaserequest.countersigncode
            new TranslationSeedItem("entity.purchaserequest.countersigncode", "zh-CN", "PR会签编码", "PR 会签编码（冗余）"),
            // entity.purchaserequest.countersigncode
            new TranslationSeedItem("entity.purchaserequest.countersigncode", "zh-HK", "PR会签编码_hk", "PR 会签编码（冗余）"),

            // entity.purchaserequest.requestdate
            new TranslationSeedItem("entity.purchaserequest.requestdate", "en-US", "申请日期_us", "申请日期"),
            // entity.purchaserequest.requestdate
            new TranslationSeedItem("entity.purchaserequest.requestdate", "ja-JP", "申请日期_jp", "申请日期"),
            // entity.purchaserequest.requestdate
            new TranslationSeedItem("entity.purchaserequest.requestdate", "zh-CN", "申请日期", "申请日期"),
            // entity.purchaserequest.requestdate
            new TranslationSeedItem("entity.purchaserequest.requestdate", "zh-HK", "申请日期_hk", "申请日期"),

            // entity.purchaserequest.requiredarrivaldate
            new TranslationSeedItem("entity.purchaserequest.requiredarrivaldate", "en-US", "要求到货日期_us", "要求到货日期"),
            // entity.purchaserequest.requiredarrivaldate
            new TranslationSeedItem("entity.purchaserequest.requiredarrivaldate", "ja-JP", "要求到货日期_jp", "要求到货日期"),
            // entity.purchaserequest.requiredarrivaldate
            new TranslationSeedItem("entity.purchaserequest.requiredarrivaldate", "zh-CN", "要求到货日期", "要求到货日期"),
            // entity.purchaserequest.requiredarrivaldate
            new TranslationSeedItem("entity.purchaserequest.requiredarrivaldate", "zh-HK", "要求到货日期_hk", "要求到货日期"),

            // entity.purchaserequest.requestemployeeid
            new TranslationSeedItem("entity.purchaserequest.requestemployeeid", "en-US", "申请人员工ID_us", "申请人员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.purchaserequest.requestemployeeid
            new TranslationSeedItem("entity.purchaserequest.requestemployeeid", "ja-JP", "申请人员工ID_jp", "申请人员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.purchaserequest.requestemployeeid
            new TranslationSeedItem("entity.purchaserequest.requestemployeeid", "zh-CN", "申请人员工ID", "申请人员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.purchaserequest.requestemployeeid
            new TranslationSeedItem("entity.purchaserequest.requestemployeeid", "zh-HK", "申请人员工ID_hk", "申请人员工（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.purchaserequest.requestemployeename
            new TranslationSeedItem("entity.purchaserequest.requestemployeename", "en-US", "申请人名称_us", "申请人名称（冗余：按 RequestEmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.purchaserequest.requestemployeename
            new TranslationSeedItem("entity.purchaserequest.requestemployeename", "ja-JP", "申请人名称_jp", "申请人名称（冗余：按 RequestEmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.purchaserequest.requestemployeename
            new TranslationSeedItem("entity.purchaserequest.requestemployeename", "zh-CN", "申请人名称", "申请人名称（冗余：按 RequestEmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.purchaserequest.requestemployeename
            new TranslationSeedItem("entity.purchaserequest.requestemployeename", "zh-HK", "申请人名称_hk", "申请人名称（冗余：按 RequestEmployeeId 取 TaktEmployee.EmployeeName 联动）"),

            // entity.purchaserequest.suppliercode
            new TranslationSeedItem("entity.purchaserequest.suppliercode", "en-US", "供应商编码_us", "供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；一单一供应商，明细禁止再挂供应商）"),
            // entity.purchaserequest.suppliercode
            new TranslationSeedItem("entity.purchaserequest.suppliercode", "ja-JP", "供应商编码_jp", "供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；一单一供应商，明细禁止再挂供应商）"),
            // entity.purchaserequest.suppliercode
            new TranslationSeedItem("entity.purchaserequest.suppliercode", "zh-CN", "供应商编码", "供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；一单一供应商，明细禁止再挂供应商）"),
            // entity.purchaserequest.suppliercode
            new TranslationSeedItem("entity.purchaserequest.suppliercode", "zh-HK", "供应商编码_hk", "供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；一单一供应商，明细禁止再挂供应商）"),

            // entity.purchaserequest.suppliername1
            new TranslationSeedItem("entity.purchaserequest.suppliername1", "en-US", "供应商名称1_us", "供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）"),
            // entity.purchaserequest.suppliername1
            new TranslationSeedItem("entity.purchaserequest.suppliername1", "ja-JP", "供应商名称1_jp", "供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）"),
            // entity.purchaserequest.suppliername1
            new TranslationSeedItem("entity.purchaserequest.suppliername1", "zh-CN", "供应商名称1", "供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）"),
            // entity.purchaserequest.suppliername1
            new TranslationSeedItem("entity.purchaserequest.suppliername1", "zh-HK", "供应商名称1_hk", "供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）"),

            // entity.purchaserequest.type
            new TranslationSeedItem("entity.purchaserequest.type", "en-US", "采购申请类型_us", "采购申请类型（字典 logistics_procurement_purchase_order_type；与采购订单/询价共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议；申请默认 B-NB）"),
            // entity.purchaserequest.type
            new TranslationSeedItem("entity.purchaserequest.type", "ja-JP", "采购申请类型_jp", "采购申请类型（字典 logistics_procurement_purchase_order_type；与采购订单/询价共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议；申请默认 B-NB）"),
            // entity.purchaserequest.type
            new TranslationSeedItem("entity.purchaserequest.type", "zh-CN", "采购申请类型", "采购申请类型（字典 logistics_procurement_purchase_order_type；与采购订单/询价共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议；申请默认 B-NB）"),
            // entity.purchaserequest.type
            new TranslationSeedItem("entity.purchaserequest.type", "zh-HK", "采购申请类型_hk", "采购申请类型（字典 logistics_procurement_purchase_order_type；与采购订单/询价共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议；申请默认 B-NB）"),

            // entity.purchaserequest.pricingprocedure
            new TranslationSeedItem("entity.purchaserequest.pricingprocedure", "en-US", "定价过程_us", "定价过程（字典 logistics_procurement_pricing_procedure；DictValue=ZRM001/ZRM002/RM0000～RMREGU；ExtLabel=A；ExtValue=M；默认 ZRM001）"),
            // entity.purchaserequest.pricingprocedure
            new TranslationSeedItem("entity.purchaserequest.pricingprocedure", "ja-JP", "定价过程_jp", "定价过程（字典 logistics_procurement_pricing_procedure；DictValue=ZRM001/ZRM002/RM0000～RMREGU；ExtLabel=A；ExtValue=M；默认 ZRM001）"),
            // entity.purchaserequest.pricingprocedure
            new TranslationSeedItem("entity.purchaserequest.pricingprocedure", "zh-CN", "定价过程", "定价过程（字典 logistics_procurement_pricing_procedure；DictValue=ZRM001/ZRM002/RM0000～RMREGU；ExtLabel=A；ExtValue=M；默认 ZRM001）"),
            // entity.purchaserequest.pricingprocedure
            new TranslationSeedItem("entity.purchaserequest.pricingprocedure", "zh-HK", "定价过程_hk", "定价过程（字典 logistics_procurement_pricing_procedure；DictValue=ZRM001/ZRM002/RM0000～RMREGU；ExtLabel=A；ExtValue=M；默认 ZRM001）"),

            // entity.purchaserequest.pricingconditioncode
            new TranslationSeedItem("entity.purchaserequest.pricingconditioncode", "en-US", "定价条件编码_us", "定价条件编码"),
            // entity.purchaserequest.pricingconditioncode
            new TranslationSeedItem("entity.purchaserequest.pricingconditioncode", "ja-JP", "定价条件编码_jp", "定价条件编码"),
            // entity.purchaserequest.pricingconditioncode
            new TranslationSeedItem("entity.purchaserequest.pricingconditioncode", "zh-CN", "定价条件编码", "定价条件编码"),
            // entity.purchaserequest.pricingconditioncode
            new TranslationSeedItem("entity.purchaserequest.pricingconditioncode", "zh-HK", "定价条件编码_hk", "定价条件编码"),

            // entity.purchaserequest.currencycode
            new TranslationSeedItem("entity.purchaserequest.currencycode", "en-US", "结算币种_us", "结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）"),
            // entity.purchaserequest.currencycode
            new TranslationSeedItem("entity.purchaserequest.currencycode", "ja-JP", "结算币种_jp", "结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）"),
            // entity.purchaserequest.currencycode
            new TranslationSeedItem("entity.purchaserequest.currencycode", "zh-CN", "结算币种", "结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）"),
            // entity.purchaserequest.currencycode
            new TranslationSeedItem("entity.purchaserequest.currencycode", "zh-HK", "结算币种_hk", "结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）"),

            // entity.purchaserequest.taxcode
            new TranslationSeedItem("entity.purchaserequest.taxcode", "en-US", "税码_us", "税码（选项 TaktTaxCodes/options；DictValue=TaxCode）"),
            // entity.purchaserequest.taxcode
            new TranslationSeedItem("entity.purchaserequest.taxcode", "ja-JP", "税码_jp", "税码（选项 TaktTaxCodes/options；DictValue=TaxCode）"),
            // entity.purchaserequest.taxcode
            new TranslationSeedItem("entity.purchaserequest.taxcode", "zh-CN", "税码", "税码（选项 TaktTaxCodes/options；DictValue=TaxCode）"),
            // entity.purchaserequest.taxcode
            new TranslationSeedItem("entity.purchaserequest.taxcode", "zh-HK", "税码_hk", "税码（选项 TaktTaxCodes/options；DictValue=TaxCode）"),

            // entity.purchaserequest.taxrate
            new TranslationSeedItem("entity.purchaserequest.taxrate", "en-US", "税率_us", "税率（百分比整数；一单一税率；由税码 TaxCode 回填，如 J2→13）"),
            // entity.purchaserequest.taxrate
            new TranslationSeedItem("entity.purchaserequest.taxrate", "ja-JP", "税率_jp", "税率（百分比整数；一单一税率；由税码 TaxCode 回填，如 J2→13）"),
            // entity.purchaserequest.taxrate
            new TranslationSeedItem("entity.purchaserequest.taxrate", "zh-CN", "税率", "税率（百分比整数；一单一税率；由税码 TaxCode 回填，如 J2→13）"),
            // entity.purchaserequest.taxrate
            new TranslationSeedItem("entity.purchaserequest.taxrate", "zh-HK", "税率_hk", "税率（百分比整数；一单一税率；由税码 TaxCode 回填，如 J2→13）"),

            // entity.purchaserequest.taxamount
            new TranslationSeedItem("entity.purchaserequest.taxamount", "en-US", "税费_us", "税费（精确到分）"),
            // entity.purchaserequest.taxamount
            new TranslationSeedItem("entity.purchaserequest.taxamount", "ja-JP", "税费_jp", "税费（精确到分）"),
            // entity.purchaserequest.taxamount
            new TranslationSeedItem("entity.purchaserequest.taxamount", "zh-CN", "税费", "税费（精确到分）"),
            // entity.purchaserequest.taxamount
            new TranslationSeedItem("entity.purchaserequest.taxamount", "zh-HK", "税费_hk", "税费（精确到分）"),

            // entity.purchaserequest.totalquantity
            new TranslationSeedItem("entity.purchaserequest.totalquantity", "en-US", "申请总数量_us", "申请总数量（基本单位数量）"),
            // entity.purchaserequest.totalquantity
            new TranslationSeedItem("entity.purchaserequest.totalquantity", "ja-JP", "申请总数量_jp", "申请总数量（基本单位数量）"),
            // entity.purchaserequest.totalquantity
            new TranslationSeedItem("entity.purchaserequest.totalquantity", "zh-CN", "申请总数量", "申请总数量（基本单位数量）"),
            // entity.purchaserequest.totalquantity
            new TranslationSeedItem("entity.purchaserequest.totalquantity", "zh-HK", "申请总数量_hk", "申请总数量（基本单位数量）"),

            // entity.purchaserequest.totalamount
            new TranslationSeedItem("entity.purchaserequest.totalamount", "en-US", "申请总金额_us", "申请总金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaserequest.totalamount
            new TranslationSeedItem("entity.purchaserequest.totalamount", "ja-JP", "申请总金额_jp", "申请总金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaserequest.totalamount
            new TranslationSeedItem("entity.purchaserequest.totalamount", "zh-CN", "申请总金额", "申请总金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaserequest.totalamount
            new TranslationSeedItem("entity.purchaserequest.totalamount", "zh-HK", "申请总金额_hk", "申请总金额（精确到分，存储为整数，单位为分）"),

            // entity.purchaserequest.convertedquantity
            new TranslationSeedItem("entity.purchaserequest.convertedquantity", "en-US", "已转订单数量_us", "已转订单数量（基本单位数量）"),
            // entity.purchaserequest.convertedquantity
            new TranslationSeedItem("entity.purchaserequest.convertedquantity", "ja-JP", "已转订单数量_jp", "已转订单数量（基本单位数量）"),
            // entity.purchaserequest.convertedquantity
            new TranslationSeedItem("entity.purchaserequest.convertedquantity", "zh-CN", "已转订单数量", "已转订单数量（基本单位数量）"),
            // entity.purchaserequest.convertedquantity
            new TranslationSeedItem("entity.purchaserequest.convertedquantity", "zh-HK", "已转订单数量_hk", "已转订单数量（基本单位数量）"),

            // entity.purchaserequest.convertedamount
            new TranslationSeedItem("entity.purchaserequest.convertedamount", "en-US", "已转订单金额_us", "已转订单金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaserequest.convertedamount
            new TranslationSeedItem("entity.purchaserequest.convertedamount", "ja-JP", "已转订单金额_jp", "已转订单金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaserequest.convertedamount
            new TranslationSeedItem("entity.purchaserequest.convertedamount", "zh-CN", "已转订单金额", "已转订单金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaserequest.convertedamount
            new TranslationSeedItem("entity.purchaserequest.convertedamount", "zh-HK", "已转订单金额_hk", "已转订单金额（精确到分，存储为整数，单位为分）"),

            // entity.purchaserequest.requestreason
            new TranslationSeedItem("entity.purchaserequest.requestreason", "en-US", "申请原因_us", "申请原因"),
            // entity.purchaserequest.requestreason
            new TranslationSeedItem("entity.purchaserequest.requestreason", "ja-JP", "申请原因_jp", "申请原因"),
            // entity.purchaserequest.requestreason
            new TranslationSeedItem("entity.purchaserequest.requestreason", "zh-CN", "申请原因", "申请原因"),
            // entity.purchaserequest.requestreason
            new TranslationSeedItem("entity.purchaserequest.requestreason", "zh-HK", "申请原因_hk", "申请原因"),

            // entity.purchaserequest.requeststatus
            new TranslationSeedItem("entity.purchaserequest.requeststatus", "en-US", "申请状态_us", "申请状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）"),
            // entity.purchaserequest.requeststatus
            new TranslationSeedItem("entity.purchaserequest.requeststatus", "ja-JP", "申请状态_jp", "申请状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）"),
            // entity.purchaserequest.requeststatus
            new TranslationSeedItem("entity.purchaserequest.requeststatus", "zh-CN", "申请状态", "申请状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）"),
            // entity.purchaserequest.requeststatus
            new TranslationSeedItem("entity.purchaserequest.requeststatus", "zh-HK", "申请状态_hk", "申请状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）"),

            // entity.purchaserequest.convertedstatus
            new TranslationSeedItem("entity.purchaserequest.convertedstatus", "en-US", "转订单状态_us", "转订单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）"),
            // entity.purchaserequest.convertedstatus
            new TranslationSeedItem("entity.purchaserequest.convertedstatus", "ja-JP", "转订单状态_jp", "转订单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）"),
            // entity.purchaserequest.convertedstatus
            new TranslationSeedItem("entity.purchaserequest.convertedstatus", "zh-CN", "转订单状态", "转订单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）"),
            // entity.purchaserequest.convertedstatus
            new TranslationSeedItem("entity.purchaserequest.convertedstatus", "zh-HK", "转订单状态_hk", "转订单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）"),

            // entity.purchaserequest.items
            new TranslationSeedItem("entity.purchaserequest.items", "en-US", "采购申请明细列表_us", "采购申请明细列表（主子表关系，一个申请可以有多个明细）"),
            // entity.purchaserequest.items
            new TranslationSeedItem("entity.purchaserequest.items", "ja-JP", "采购申请明细列表_jp", "采购申请明细列表（主子表关系，一个申请可以有多个明细）"),
            // entity.purchaserequest.items
            new TranslationSeedItem("entity.purchaserequest.items", "zh-CN", "采购申请明细列表", "采购申请明细列表（主子表关系，一个申请可以有多个明细）"),
            // entity.purchaserequest.items
            new TranslationSeedItem("entity.purchaserequest.items", "zh-HK", "采购申请明细列表_hk", "采购申请明细列表（主子表关系，一个申请可以有多个明细）"),
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
