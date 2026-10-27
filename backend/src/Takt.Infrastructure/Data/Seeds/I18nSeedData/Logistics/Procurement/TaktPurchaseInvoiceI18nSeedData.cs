// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Procurement
// 文件名称：TaktPurchaseInvoiceI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPurchaseInvoice 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktPurchaseInvoice 实体国际化翻译种子（键前缀 entity.purchaseinvoice.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPurchaseInvoiceI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPurchaseInvoice 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 purchaseinvoice 实体翻译...", tenantCode);

        foreach (var item in GetPurchaseInvoiceTranslations())
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

        TaktLogger.Information("TaktPurchaseInvoice 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPurchaseInvoice 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.purchaseinvoice._self / entity.purchaseinvoice.{{field}}；ResourceGroup=Procurement；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPurchaseInvoiceTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.purchaseinvoice._self
            new TranslationSeedItem("entity.purchaseinvoice._self", "en-US", "Purchase Invoice Information_us", "实体名称"),
            // entity.purchaseinvoice._self
            new TranslationSeedItem("entity.purchaseinvoice._self", "ja-JP", "Takt采购发票主表信息_jp", "实体名称"),
            // entity.purchaseinvoice._self
            new TranslationSeedItem("entity.purchaseinvoice._self", "zh-CN", "Takt采购发票主表信息", "实体名称"),
            // entity.purchaseinvoice._self
            new TranslationSeedItem("entity.purchaseinvoice._self", "zh-HK", "Takt采购发票主表信息_hk", "实体名称"),

            // entity.purchaseinvoice.code
            new TranslationSeedItem("entity.purchaseinvoice.code", "en-US", "发票凭证编号_us", "发票凭证编号（前端表单选择编码规则后自动通过 TaktNumbering 发票凭证编号规则生成并展示，非手输；单据类型菜单：采购发票）"),
            // entity.purchaseinvoice.code
            new TranslationSeedItem("entity.purchaseinvoice.code", "ja-JP", "发票凭证编号_jp", "发票凭证编号（前端表单选择编码规则后自动通过 TaktNumbering 发票凭证编号规则生成并展示，非手输；单据类型菜单：采购发票）"),
            // entity.purchaseinvoice.code
            new TranslationSeedItem("entity.purchaseinvoice.code", "zh-CN", "发票凭证编号", "发票凭证编号（前端表单选择编码规则后自动通过 TaktNumbering 发票凭证编号规则生成并展示，非手输；单据类型菜单：采购发票）"),
            // entity.purchaseinvoice.code
            new TranslationSeedItem("entity.purchaseinvoice.code", "zh-HK", "发票凭证编号_hk", "发票凭证编号（前端表单选择编码规则后自动通过 TaktNumbering 发票凭证编号规则生成并展示，非手输；单据类型菜单：采购发票）"),

            // entity.purchaseinvoice.fiscalyear
            new TranslationSeedItem("entity.purchaseinvoice.fiscalyear", "en-US", "会计年度_us", "会计年度"),
            // entity.purchaseinvoice.fiscalyear
            new TranslationSeedItem("entity.purchaseinvoice.fiscalyear", "ja-JP", "会计年度_jp", "会计年度"),
            // entity.purchaseinvoice.fiscalyear
            new TranslationSeedItem("entity.purchaseinvoice.fiscalyear", "zh-CN", "会计年度", "会计年度"),
            // entity.purchaseinvoice.fiscalyear
            new TranslationSeedItem("entity.purchaseinvoice.fiscalyear", "zh-HK", "会计年度_hk", "会计年度"),

            // entity.purchaseinvoice.documenttype
            new TranslationSeedItem("entity.purchaseinvoice.documenttype", "en-US", "凭证类型_us", "凭证类型（字典 logistics_purchase_invoice_document_type）"),
            // entity.purchaseinvoice.documenttype
            new TranslationSeedItem("entity.purchaseinvoice.documenttype", "ja-JP", "凭证类型_jp", "凭证类型（字典 logistics_purchase_invoice_document_type）"),
            // entity.purchaseinvoice.documenttype
            new TranslationSeedItem("entity.purchaseinvoice.documenttype", "zh-CN", "凭证类型", "凭证类型（字典 logistics_purchase_invoice_document_type）"),
            // entity.purchaseinvoice.documenttype
            new TranslationSeedItem("entity.purchaseinvoice.documenttype", "zh-HK", "凭证类型_hk", "凭证类型（字典 logistics_purchase_invoice_document_type）"),

            // entity.purchaseinvoice.documentdate
            new TranslationSeedItem("entity.purchaseinvoice.documentdate", "en-US", "凭证日期_us", "凭证日期"),
            // entity.purchaseinvoice.documentdate
            new TranslationSeedItem("entity.purchaseinvoice.documentdate", "ja-JP", "凭证日期_jp", "凭证日期"),
            // entity.purchaseinvoice.documentdate
            new TranslationSeedItem("entity.purchaseinvoice.documentdate", "zh-CN", "凭证日期", "凭证日期"),
            // entity.purchaseinvoice.documentdate
            new TranslationSeedItem("entity.purchaseinvoice.documentdate", "zh-HK", "凭证日期_hk", "凭证日期"),

            // entity.purchaseinvoice.postingdate
            new TranslationSeedItem("entity.purchaseinvoice.postingdate", "en-US", "过帐日期_us", "过帐日期"),
            // entity.purchaseinvoice.postingdate
            new TranslationSeedItem("entity.purchaseinvoice.postingdate", "ja-JP", "过帐日期_jp", "过帐日期"),
            // entity.purchaseinvoice.postingdate
            new TranslationSeedItem("entity.purchaseinvoice.postingdate", "zh-CN", "过帐日期", "过帐日期"),
            // entity.purchaseinvoice.postingdate
            new TranslationSeedItem("entity.purchaseinvoice.postingdate", "zh-HK", "过帐日期_hk", "过帐日期"),

            // entity.purchaseinvoice.transactioneventtype
            new TranslationSeedItem("entity.purchaseinvoice.transactioneventtype", "en-US", "交易类型_us", "交易类型（字典 logistics_purchase_invoice_transaction_event_type）"),
            // entity.purchaseinvoice.transactioneventtype
            new TranslationSeedItem("entity.purchaseinvoice.transactioneventtype", "ja-JP", "交易类型_jp", "交易类型（字典 logistics_purchase_invoice_transaction_event_type）"),
            // entity.purchaseinvoice.transactioneventtype
            new TranslationSeedItem("entity.purchaseinvoice.transactioneventtype", "zh-CN", "交易类型", "交易类型（字典 logistics_purchase_invoice_transaction_event_type）"),
            // entity.purchaseinvoice.transactioneventtype
            new TranslationSeedItem("entity.purchaseinvoice.transactioneventtype", "zh-HK", "交易类型_hk", "交易类型（字典 logistics_purchase_invoice_transaction_event_type）"),

            // entity.purchaseinvoice.referencecode
            new TranslationSeedItem("entity.purchaseinvoice.referencecode", "en-US", "参照_us", "参照"),
            // entity.purchaseinvoice.referencecode
            new TranslationSeedItem("entity.purchaseinvoice.referencecode", "ja-JP", "参照_jp", "参照"),
            // entity.purchaseinvoice.referencecode
            new TranslationSeedItem("entity.purchaseinvoice.referencecode", "zh-CN", "参照", "参照"),
            // entity.purchaseinvoice.referencecode
            new TranslationSeedItem("entity.purchaseinvoice.referencecode", "zh-HK", "参照_hk", "参照"),

            // entity.purchaseinvoice.suppliercode
            new TranslationSeedItem("entity.purchaseinvoice.suppliercode", "en-US", "出票方_us", "出票方（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.purchaseinvoice.suppliercode
            new TranslationSeedItem("entity.purchaseinvoice.suppliercode", "ja-JP", "出票方_jp", "出票方（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.purchaseinvoice.suppliercode
            new TranslationSeedItem("entity.purchaseinvoice.suppliercode", "zh-CN", "出票方", "出票方（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.purchaseinvoice.suppliercode
            new TranslationSeedItem("entity.purchaseinvoice.suppliercode", "zh-HK", "出票方_hk", "出票方（选项 TaktSuppliers/options；DictValue=SupplierCode）"),

            // entity.purchaseinvoice.currencycode
            new TranslationSeedItem("entity.purchaseinvoice.currencycode", "en-US", "货币_us", "货币（字典 accounting_financial_currency_code）"),
            // entity.purchaseinvoice.currencycode
            new TranslationSeedItem("entity.purchaseinvoice.currencycode", "ja-JP", "货币_jp", "货币（字典 accounting_financial_currency_code）"),
            // entity.purchaseinvoice.currencycode
            new TranslationSeedItem("entity.purchaseinvoice.currencycode", "zh-CN", "货币", "货币（字典 accounting_financial_currency_code）"),
            // entity.purchaseinvoice.currencycode
            new TranslationSeedItem("entity.purchaseinvoice.currencycode", "zh-HK", "货币_hk", "货币（字典 accounting_financial_currency_code）"),

            // entity.purchaseinvoice.exchangerate
            new TranslationSeedItem("entity.purchaseinvoice.exchangerate", "en-US", "汇率_us", "汇率"),
            // entity.purchaseinvoice.exchangerate
            new TranslationSeedItem("entity.purchaseinvoice.exchangerate", "ja-JP", "汇率_jp", "汇率"),
            // entity.purchaseinvoice.exchangerate
            new TranslationSeedItem("entity.purchaseinvoice.exchangerate", "zh-CN", "汇率", "汇率"),
            // entity.purchaseinvoice.exchangerate
            new TranslationSeedItem("entity.purchaseinvoice.exchangerate", "zh-HK", "汇率_hk", "汇率"),

            // entity.purchaseinvoice.grossamount
            new TranslationSeedItem("entity.purchaseinvoice.grossamount", "en-US", "总发票金额_us", "总发票金额"),
            // entity.purchaseinvoice.grossamount
            new TranslationSeedItem("entity.purchaseinvoice.grossamount", "ja-JP", "总发票金额_jp", "总发票金额"),
            // entity.purchaseinvoice.grossamount
            new TranslationSeedItem("entity.purchaseinvoice.grossamount", "zh-CN", "总发票金额", "总发票金额"),
            // entity.purchaseinvoice.grossamount
            new TranslationSeedItem("entity.purchaseinvoice.grossamount", "zh-HK", "总发票金额_hk", "总发票金额"),

            // entity.purchaseinvoice.vatamount
            new TranslationSeedItem("entity.purchaseinvoice.vatamount", "en-US", "增值税金额_us", "增值税金额"),
            // entity.purchaseinvoice.vatamount
            new TranslationSeedItem("entity.purchaseinvoice.vatamount", "ja-JP", "增值税金额_jp", "增值税金额"),
            // entity.purchaseinvoice.vatamount
            new TranslationSeedItem("entity.purchaseinvoice.vatamount", "zh-CN", "增值税金额", "增值税金额"),
            // entity.purchaseinvoice.vatamount
            new TranslationSeedItem("entity.purchaseinvoice.vatamount", "zh-HK", "增值税金额_hk", "增值税金额"),

            // entity.purchaseinvoice.taxjurisdictioncode
            new TranslationSeedItem("entity.purchaseinvoice.taxjurisdictioncode", "en-US", "税务代码_us", "税务代码"),
            // entity.purchaseinvoice.taxjurisdictioncode
            new TranslationSeedItem("entity.purchaseinvoice.taxjurisdictioncode", "ja-JP", "税务代码_jp", "税务代码"),
            // entity.purchaseinvoice.taxjurisdictioncode
            new TranslationSeedItem("entity.purchaseinvoice.taxjurisdictioncode", "zh-CN", "税务代码", "税务代码"),
            // entity.purchaseinvoice.taxjurisdictioncode
            new TranslationSeedItem("entity.purchaseinvoice.taxjurisdictioncode", "zh-HK", "税务代码_hk", "税务代码"),

            // entity.purchaseinvoice.cashdiscountdays1
            new TranslationSeedItem("entity.purchaseinvoice.cashdiscountdays1", "en-US", "天数 1_us", "天数 1（现金折扣天数）"),
            // entity.purchaseinvoice.cashdiscountdays1
            new TranslationSeedItem("entity.purchaseinvoice.cashdiscountdays1", "ja-JP", "天数 1_jp", "天数 1（现金折扣天数）"),
            // entity.purchaseinvoice.cashdiscountdays1
            new TranslationSeedItem("entity.purchaseinvoice.cashdiscountdays1", "zh-CN", "天数 1", "天数 1（现金折扣天数）"),
            // entity.purchaseinvoice.cashdiscountdays1
            new TranslationSeedItem("entity.purchaseinvoice.cashdiscountdays1", "zh-HK", "天数 1_hk", "天数 1（现金折扣天数）"),

            // entity.purchaseinvoice.invoiceflag
            new TranslationSeedItem("entity.purchaseinvoice.invoiceflag", "en-US", "发票_us", "发票"),
            // entity.purchaseinvoice.invoiceflag
            new TranslationSeedItem("entity.purchaseinvoice.invoiceflag", "ja-JP", "发票_jp", "发票"),
            // entity.purchaseinvoice.invoiceflag
            new TranslationSeedItem("entity.purchaseinvoice.invoiceflag", "zh-CN", "发票", "发票"),
            // entity.purchaseinvoice.invoiceflag
            new TranslationSeedItem("entity.purchaseinvoice.invoiceflag", "zh-HK", "发票_hk", "发票"),

            // entity.purchaseinvoice.headertext
            new TranslationSeedItem("entity.purchaseinvoice.headertext", "en-US", "凭证抬头文本_us", "凭证抬头文本"),
            // entity.purchaseinvoice.headertext
            new TranslationSeedItem("entity.purchaseinvoice.headertext", "ja-JP", "凭证抬头文本_jp", "凭证抬头文本"),
            // entity.purchaseinvoice.headertext
            new TranslationSeedItem("entity.purchaseinvoice.headertext", "zh-CN", "凭证抬头文本", "凭证抬头文本"),
            // entity.purchaseinvoice.headertext
            new TranslationSeedItem("entity.purchaseinvoice.headertext", "zh-HK", "凭证抬头文本_hk", "凭证抬头文本"),

            // entity.purchaseinvoice.reversaldocumentcode
            new TranslationSeedItem("entity.purchaseinvoice.reversaldocumentcode", "en-US", "冲销者_us", "冲销者（冲销凭证编号）"),
            // entity.purchaseinvoice.reversaldocumentcode
            new TranslationSeedItem("entity.purchaseinvoice.reversaldocumentcode", "ja-JP", "冲销者_jp", "冲销者（冲销凭证编号）"),
            // entity.purchaseinvoice.reversaldocumentcode
            new TranslationSeedItem("entity.purchaseinvoice.reversaldocumentcode", "zh-CN", "冲销者", "冲销者（冲销凭证编号）"),
            // entity.purchaseinvoice.reversaldocumentcode
            new TranslationSeedItem("entity.purchaseinvoice.reversaldocumentcode", "zh-HK", "冲销者_hk", "冲销者（冲销凭证编号）"),

            // entity.purchaseinvoice.reversalfiscalyear
            new TranslationSeedItem("entity.purchaseinvoice.reversalfiscalyear", "en-US", "年_us", "年（冲销会计年度）"),
            // entity.purchaseinvoice.reversalfiscalyear
            new TranslationSeedItem("entity.purchaseinvoice.reversalfiscalyear", "ja-JP", "年_jp", "年（冲销会计年度）"),
            // entity.purchaseinvoice.reversalfiscalyear
            new TranslationSeedItem("entity.purchaseinvoice.reversalfiscalyear", "zh-CN", "年", "年（冲销会计年度）"),
            // entity.purchaseinvoice.reversalfiscalyear
            new TranslationSeedItem("entity.purchaseinvoice.reversalfiscalyear", "zh-HK", "年_hk", "年（冲销会计年度）"),

            // entity.purchaseinvoice.taxcode
            new TranslationSeedItem("entity.purchaseinvoice.taxcode", "en-US", "税码_us", "税码（选项 TaktTaxCodes/options；DictValue=TaxCode）"),
            // entity.purchaseinvoice.taxcode
            new TranslationSeedItem("entity.purchaseinvoice.taxcode", "ja-JP", "税码_jp", "税码（选项 TaktTaxCodes/options；DictValue=TaxCode）"),
            // entity.purchaseinvoice.taxcode
            new TranslationSeedItem("entity.purchaseinvoice.taxcode", "zh-CN", "税码", "税码（选项 TaktTaxCodes/options；DictValue=TaxCode）"),
            // entity.purchaseinvoice.taxcode
            new TranslationSeedItem("entity.purchaseinvoice.taxcode", "zh-HK", "税码_hk", "税码（选项 TaktTaxCodes/options；DictValue=TaxCode）"),

            // entity.purchaseinvoice.supplyingcountry
            new TranslationSeedItem("entity.purchaseinvoice.supplyingcountry", "en-US", "供货国家_us", "供货国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.purchaseinvoice.supplyingcountry
            new TranslationSeedItem("entity.purchaseinvoice.supplyingcountry", "ja-JP", "供货国家_jp", "供货国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.purchaseinvoice.supplyingcountry
            new TranslationSeedItem("entity.purchaseinvoice.supplyingcountry", "zh-CN", "供货国家", "供货国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.purchaseinvoice.supplyingcountry
            new TranslationSeedItem("entity.purchaseinvoice.supplyingcountry", "zh-HK", "供货国家_hk", "供货国家（字典 sys_country_code；DictValue=ISO alpha-2）"),

            // entity.purchaseinvoice.taxexchangerate
            new TranslationSeedItem("entity.purchaseinvoice.taxexchangerate", "en-US", "税率_us", "税率（税务汇率）"),
            // entity.purchaseinvoice.taxexchangerate
            new TranslationSeedItem("entity.purchaseinvoice.taxexchangerate", "ja-JP", "税率_jp", "税率（税务汇率）"),
            // entity.purchaseinvoice.taxexchangerate
            new TranslationSeedItem("entity.purchaseinvoice.taxexchangerate", "zh-CN", "税率", "税率（税务汇率）"),
            // entity.purchaseinvoice.taxexchangerate
            new TranslationSeedItem("entity.purchaseinvoice.taxexchangerate", "zh-HK", "税率_hk", "税率（税务汇率）"),

            // entity.purchaseinvoice.baselinedate
            new TranslationSeedItem("entity.purchaseinvoice.baselinedate", "en-US", "付款基准日期_us", "付款基准日期"),
            // entity.purchaseinvoice.baselinedate
            new TranslationSeedItem("entity.purchaseinvoice.baselinedate", "ja-JP", "付款基准日期_jp", "付款基准日期"),
            // entity.purchaseinvoice.baselinedate
            new TranslationSeedItem("entity.purchaseinvoice.baselinedate", "zh-CN", "付款基准日期", "付款基准日期"),
            // entity.purchaseinvoice.baselinedate
            new TranslationSeedItem("entity.purchaseinvoice.baselinedate", "zh-HK", "付款基准日期_hk", "付款基准日期"),

            // entity.purchaseinvoice.exchangeratedate
            new TranslationSeedItem("entity.purchaseinvoice.exchangeratedate", "en-US", "换算日期_us", "换算日期"),
            // entity.purchaseinvoice.exchangeratedate
            new TranslationSeedItem("entity.purchaseinvoice.exchangeratedate", "ja-JP", "换算日期_jp", "换算日期"),
            // entity.purchaseinvoice.exchangeratedate
            new TranslationSeedItem("entity.purchaseinvoice.exchangeratedate", "zh-CN", "换算日期", "换算日期"),
            // entity.purchaseinvoice.exchangeratedate
            new TranslationSeedItem("entity.purchaseinvoice.exchangeratedate", "zh-HK", "换算日期_hk", "换算日期"),

            // entity.purchaseinvoice.transactioncode
            new TranslationSeedItem("entity.purchaseinvoice.transactioncode", "en-US", "事务代码_us", "事务代码"),
            // entity.purchaseinvoice.transactioncode
            new TranslationSeedItem("entity.purchaseinvoice.transactioncode", "ja-JP", "事务代码_jp", "事务代码"),
            // entity.purchaseinvoice.transactioncode
            new TranslationSeedItem("entity.purchaseinvoice.transactioncode", "zh-CN", "事务代码", "事务代码"),
            // entity.purchaseinvoice.transactioncode
            new TranslationSeedItem("entity.purchaseinvoice.transactioncode", "zh-HK", "事务代码_hk", "事务代码"),

            // entity.purchaseinvoice.postedby
            new TranslationSeedItem("entity.purchaseinvoice.postedby", "en-US", "过账人_us", "过账人（当前登录用户对应的 EmployeeCode）"),
            // entity.purchaseinvoice.postedby
            new TranslationSeedItem("entity.purchaseinvoice.postedby", "ja-JP", "过账人_jp", "过账人（当前登录用户对应的 EmployeeCode）"),
            // entity.purchaseinvoice.postedby
            new TranslationSeedItem("entity.purchaseinvoice.postedby", "zh-CN", "过账人", "过账人（当前登录用户对应的 EmployeeCode）"),
            // entity.purchaseinvoice.postedby
            new TranslationSeedItem("entity.purchaseinvoice.postedby", "zh-HK", "过账人_hk", "过账人（当前登录用户对应的 EmployeeCode）"),

            // entity.purchaseinvoice.items
            new TranslationSeedItem("entity.purchaseinvoice.items", "en-US", "采购发票明细列表_us", "采购发票明细列表（主子表关系）"),
            // entity.purchaseinvoice.items
            new TranslationSeedItem("entity.purchaseinvoice.items", "ja-JP", "采购发票明细列表_jp", "采购发票明细列表（主子表关系）"),
            // entity.purchaseinvoice.items
            new TranslationSeedItem("entity.purchaseinvoice.items", "zh-CN", "采购发票明细列表", "采购发票明细列表（主子表关系）"),
            // entity.purchaseinvoice.items
            new TranslationSeedItem("entity.purchaseinvoice.items", "zh-HK", "采购发票明细列表_hk", "采购发票明细列表（主子表关系）"),
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
