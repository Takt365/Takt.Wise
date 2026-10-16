// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales
// 文件名称：TaktSalesQuotationI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSalesQuotation 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales;

/// <summary>
/// TaktSalesQuotation 实体国际化翻译种子（键前缀 entity.salesquotation.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSalesQuotationI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSalesQuotation 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 salesquotation 实体翻译...", tenantCode);

        foreach (var item in GetSalesQuotationTranslations())
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

        TaktLogger.Information("TaktSalesQuotation 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSalesQuotation 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.salesquotation._self / entity.salesquotation.{{field}}；ResourceGroup=Sales；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSalesQuotationTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.salesquotation._self
            new TranslationSeedItem("entity.salesquotation._self", "en-US", "Sales Quotation Information_us", "实体名称"),
            // entity.salesquotation._self
            new TranslationSeedItem("entity.salesquotation._self", "ja-JP", "Takt销售报价信息_jp", "实体名称"),
            // entity.salesquotation._self
            new TranslationSeedItem("entity.salesquotation._self", "zh-CN", "Takt销售报价信息", "实体名称"),
            // entity.salesquotation._self
            new TranslationSeedItem("entity.salesquotation._self", "zh-HK", "Takt销售报价信息_hk", "实体名称"),

            // entity.salesquotation.code
            new TranslationSeedItem("entity.salesquotation.code", "en-US", "销售报价编码_us", "销售报价编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 销售报价编码规则生成并展示，非手输；单据类型菜单：销售报价）"),
            // entity.salesquotation.code
            new TranslationSeedItem("entity.salesquotation.code", "ja-JP", "销售报价编码_jp", "销售报价编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 销售报价编码规则生成并展示，非手输；单据类型菜单：销售报价）"),
            // entity.salesquotation.code
            new TranslationSeedItem("entity.salesquotation.code", "zh-CN", "销售报价编码", "销售报价编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 销售报价编码规则生成并展示，非手输；单据类型菜单：销售报价）"),
            // entity.salesquotation.code
            new TranslationSeedItem("entity.salesquotation.code", "zh-HK", "销售报价编码_hk", "销售报价编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 销售报价编码规则生成并展示，非手输；单据类型菜单：销售报价）"),

            // entity.salesquotation.customercode
            new TranslationSeedItem("entity.salesquotation.customercode", "en-US", "客户编码_us", "客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）"),
            // entity.salesquotation.customercode
            new TranslationSeedItem("entity.salesquotation.customercode", "ja-JP", "客户编码_jp", "客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）"),
            // entity.salesquotation.customercode
            new TranslationSeedItem("entity.salesquotation.customercode", "zh-CN", "客户编码", "客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）"),
            // entity.salesquotation.customercode
            new TranslationSeedItem("entity.salesquotation.customercode", "zh-HK", "客户编码_hk", "客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）"),

            // entity.salesquotation.customername1
            new TranslationSeedItem("entity.salesquotation.customername1", "en-US", "客户名称1_us", "客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）"),
            // entity.salesquotation.customername1
            new TranslationSeedItem("entity.salesquotation.customername1", "ja-JP", "客户名称1_jp", "客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）"),
            // entity.salesquotation.customername1
            new TranslationSeedItem("entity.salesquotation.customername1", "zh-CN", "客户名称1", "客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）"),
            // entity.salesquotation.customername1
            new TranslationSeedItem("entity.salesquotation.customername1", "zh-HK", "客户名称1_hk", "客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）"),

            // entity.salesquotation.quotationdate
            new TranslationSeedItem("entity.salesquotation.quotationdate", "en-US", "报价日期_us", "报价日期"),
            // entity.salesquotation.quotationdate
            new TranslationSeedItem("entity.salesquotation.quotationdate", "ja-JP", "报价日期_jp", "报价日期"),
            // entity.salesquotation.quotationdate
            new TranslationSeedItem("entity.salesquotation.quotationdate", "zh-CN", "报价日期", "报价日期"),
            // entity.salesquotation.quotationdate
            new TranslationSeedItem("entity.salesquotation.quotationdate", "zh-HK", "报价日期_hk", "报价日期"),

            // entity.salesquotation.validuntildate
            new TranslationSeedItem("entity.salesquotation.validuntildate", "en-US", "报价有效期至_us", "报价有效期至"),
            // entity.salesquotation.validuntildate
            new TranslationSeedItem("entity.salesquotation.validuntildate", "ja-JP", "报价有效期至_jp", "报价有效期至"),
            // entity.salesquotation.validuntildate
            new TranslationSeedItem("entity.salesquotation.validuntildate", "zh-CN", "报价有效期至", "报价有效期至"),
            // entity.salesquotation.validuntildate
            new TranslationSeedItem("entity.salesquotation.validuntildate", "zh-HK", "报价有效期至_hk", "报价有效期至"),

            // entity.salesquotation.salesgroup
            new TranslationSeedItem("entity.salesquotation.salesgroup", "en-US", "销售组_us", "销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）"),
            // entity.salesquotation.salesgroup
            new TranslationSeedItem("entity.salesquotation.salesgroup", "ja-JP", "销售组_jp", "销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）"),
            // entity.salesquotation.salesgroup
            new TranslationSeedItem("entity.salesquotation.salesgroup", "zh-CN", "销售组", "销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）"),
            // entity.salesquotation.salesgroup
            new TranslationSeedItem("entity.salesquotation.salesgroup", "zh-HK", "销售组_hk", "销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）"),

            // entity.salesquotation.type
            new TranslationSeedItem("entity.salesquotation.type", "en-US", "销售报价类型_us", "销售报价类型（字典 logistics_sales_order_type；与销售订单共用；DictValue=AG/QT/AEBQ/ZQT/Z800/Z801/Z850/Z851/ZCR/ZDR/ZOR/ZOR1；ExtLabel=凭证类别 A询价/B报价/C订单/H退货/K贷项/L借项；报价默认 AEBQ）"),
            // entity.salesquotation.type
            new TranslationSeedItem("entity.salesquotation.type", "ja-JP", "销售报价类型_jp", "销售报价类型（字典 logistics_sales_order_type；与销售订单共用；DictValue=AG/QT/AEBQ/ZQT/Z800/Z801/Z850/Z851/ZCR/ZDR/ZOR/ZOR1；ExtLabel=凭证类别 A询价/B报价/C订单/H退货/K贷项/L借项；报价默认 AEBQ）"),
            // entity.salesquotation.type
            new TranslationSeedItem("entity.salesquotation.type", "zh-CN", "销售报价类型", "销售报价类型（字典 logistics_sales_order_type；与销售订单共用；DictValue=AG/QT/AEBQ/ZQT/Z800/Z801/Z850/Z851/ZCR/ZDR/ZOR/ZOR1；ExtLabel=凭证类别 A询价/B报价/C订单/H退货/K贷项/L借项；报价默认 AEBQ）"),
            // entity.salesquotation.type
            new TranslationSeedItem("entity.salesquotation.type", "zh-HK", "销售报价类型_hk", "销售报价类型（字典 logistics_sales_order_type；与销售订单共用；DictValue=AG/QT/AEBQ/ZQT/Z800/Z801/Z850/Z851/ZCR/ZDR/ZOR/ZOR1；ExtLabel=凭证类别 A询价/B报价/C订单/H退货/K贷项/L借项；报价默认 AEBQ）"),

            // entity.salesquotation.pricingprocedure
            new TranslationSeedItem("entity.salesquotation.pricingprocedure", "en-US", "定价过程_us", "定价过程（字典 logistics_sales_pricing_procedure；DictValue=Z10010～Z91001/ZCAA01/ZVAA97/ZVAA98/ZVAA99；ExtLabel=A；ExtValue=V；默认 ZVAA99）"),
            // entity.salesquotation.pricingprocedure
            new TranslationSeedItem("entity.salesquotation.pricingprocedure", "ja-JP", "定价过程_jp", "定价过程（字典 logistics_sales_pricing_procedure；DictValue=Z10010～Z91001/ZCAA01/ZVAA97/ZVAA98/ZVAA99；ExtLabel=A；ExtValue=V；默认 ZVAA99）"),
            // entity.salesquotation.pricingprocedure
            new TranslationSeedItem("entity.salesquotation.pricingprocedure", "zh-CN", "定价过程", "定价过程（字典 logistics_sales_pricing_procedure；DictValue=Z10010～Z91001/ZCAA01/ZVAA97/ZVAA98/ZVAA99；ExtLabel=A；ExtValue=V；默认 ZVAA99）"),
            // entity.salesquotation.pricingprocedure
            new TranslationSeedItem("entity.salesquotation.pricingprocedure", "zh-HK", "定价过程_hk", "定价过程（字典 logistics_sales_pricing_procedure；DictValue=Z10010～Z91001/ZCAA01/ZVAA97/ZVAA98/ZVAA99；ExtLabel=A；ExtValue=V；默认 ZVAA99）"),

            // entity.salesquotation.pricingconditioncode
            new TranslationSeedItem("entity.salesquotation.pricingconditioncode", "en-US", "定价条件编码_us", "定价条件编码"),
            // entity.salesquotation.pricingconditioncode
            new TranslationSeedItem("entity.salesquotation.pricingconditioncode", "ja-JP", "定价条件编码_jp", "定价条件编码"),
            // entity.salesquotation.pricingconditioncode
            new TranslationSeedItem("entity.salesquotation.pricingconditioncode", "zh-CN", "定价条件编码", "定价条件编码"),
            // entity.salesquotation.pricingconditioncode
            new TranslationSeedItem("entity.salesquotation.pricingconditioncode", "zh-HK", "定价条件编码_hk", "定价条件编码"),

            // entity.salesquotation.totalquantity
            new TranslationSeedItem("entity.salesquotation.totalquantity", "en-US", "报价总数量_us", "报价总数量（基本单位数量）"),
            // entity.salesquotation.totalquantity
            new TranslationSeedItem("entity.salesquotation.totalquantity", "ja-JP", "报价总数量_jp", "报价总数量（基本单位数量）"),
            // entity.salesquotation.totalquantity
            new TranslationSeedItem("entity.salesquotation.totalquantity", "zh-CN", "报价总数量", "报价总数量（基本单位数量）"),
            // entity.salesquotation.totalquantity
            new TranslationSeedItem("entity.salesquotation.totalquantity", "zh-HK", "报价总数量_hk", "报价总数量（基本单位数量）"),

            // entity.salesquotation.totalamount
            new TranslationSeedItem("entity.salesquotation.totalamount", "en-US", "报价总金额_us", "报价总金额"),
            // entity.salesquotation.totalamount
            new TranslationSeedItem("entity.salesquotation.totalamount", "ja-JP", "报价总金额_jp", "报价总金额"),
            // entity.salesquotation.totalamount
            new TranslationSeedItem("entity.salesquotation.totalamount", "zh-CN", "报价总金额", "报价总金额"),
            // entity.salesquotation.totalamount
            new TranslationSeedItem("entity.salesquotation.totalamount", "zh-HK", "报价总金额_hk", "报价总金额"),

            // entity.salesquotation.discountamount
            new TranslationSeedItem("entity.salesquotation.discountamount", "en-US", "折扣金额_us", "折扣金额"),
            // entity.salesquotation.discountamount
            new TranslationSeedItem("entity.salesquotation.discountamount", "ja-JP", "折扣金额_jp", "折扣金额"),
            // entity.salesquotation.discountamount
            new TranslationSeedItem("entity.salesquotation.discountamount", "zh-CN", "折扣金额", "折扣金额"),
            // entity.salesquotation.discountamount
            new TranslationSeedItem("entity.salesquotation.discountamount", "zh-HK", "折扣金额_hk", "折扣金额"),

            // entity.salesquotation.currencycode
            new TranslationSeedItem("entity.salesquotation.currencycode", "en-US", "结算币种_us", "结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）"),
            // entity.salesquotation.currencycode
            new TranslationSeedItem("entity.salesquotation.currencycode", "ja-JP", "结算币种_jp", "结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）"),
            // entity.salesquotation.currencycode
            new TranslationSeedItem("entity.salesquotation.currencycode", "zh-CN", "结算币种", "结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）"),
            // entity.salesquotation.currencycode
            new TranslationSeedItem("entity.salesquotation.currencycode", "zh-HK", "结算币种_hk", "结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）"),

            // entity.salesquotation.taxcode
            new TranslationSeedItem("entity.salesquotation.taxcode", "en-US", "税码_us", "税码（选项 TaktTaxCodes/options；DictValue=TaxCode）"),
            // entity.salesquotation.taxcode
            new TranslationSeedItem("entity.salesquotation.taxcode", "ja-JP", "税码_jp", "税码（选项 TaktTaxCodes/options；DictValue=TaxCode）"),
            // entity.salesquotation.taxcode
            new TranslationSeedItem("entity.salesquotation.taxcode", "zh-CN", "税码", "税码（选项 TaktTaxCodes/options；DictValue=TaxCode）"),
            // entity.salesquotation.taxcode
            new TranslationSeedItem("entity.salesquotation.taxcode", "zh-HK", "税码_hk", "税码（选项 TaktTaxCodes/options；DictValue=TaxCode）"),

            // entity.salesquotation.taxrate
            new TranslationSeedItem("entity.salesquotation.taxrate", "en-US", "税率_us", "税率（百分比整数；一单一税率；由税码 TaxCode 回填，如 J2→13）"),
            // entity.salesquotation.taxrate
            new TranslationSeedItem("entity.salesquotation.taxrate", "ja-JP", "税率_jp", "税率（百分比整数；一单一税率；由税码 TaxCode 回填，如 J2→13）"),
            // entity.salesquotation.taxrate
            new TranslationSeedItem("entity.salesquotation.taxrate", "zh-CN", "税率", "税率（百分比整数；一单一税率；由税码 TaxCode 回填，如 J2→13）"),
            // entity.salesquotation.taxrate
            new TranslationSeedItem("entity.salesquotation.taxrate", "zh-HK", "税率_hk", "税率（百分比整数；一单一税率；由税码 TaxCode 回填，如 J2→13）"),

            // entity.salesquotation.taxamount
            new TranslationSeedItem("entity.salesquotation.taxamount", "en-US", "税费_us", "税费"),
            // entity.salesquotation.taxamount
            new TranslationSeedItem("entity.salesquotation.taxamount", "ja-JP", "税费_jp", "税费"),
            // entity.salesquotation.taxamount
            new TranslationSeedItem("entity.salesquotation.taxamount", "zh-CN", "税费", "税费"),
            // entity.salesquotation.taxamount
            new TranslationSeedItem("entity.salesquotation.taxamount", "zh-HK", "税费_hk", "税费"),

            // entity.salesquotation.actualamount
            new TranslationSeedItem("entity.salesquotation.actualamount", "en-US", "报价实付金额_us", "报价实付金额"),
            // entity.salesquotation.actualamount
            new TranslationSeedItem("entity.salesquotation.actualamount", "ja-JP", "报价实付金额_jp", "报价实付金额"),
            // entity.salesquotation.actualamount
            new TranslationSeedItem("entity.salesquotation.actualamount", "zh-CN", "报价实付金额", "报价实付金额"),
            // entity.salesquotation.actualamount
            new TranslationSeedItem("entity.salesquotation.actualamount", "zh-HK", "报价实付金额_hk", "报价实付金额"),

            // entity.salesquotation.salesordercode
            new TranslationSeedItem("entity.salesquotation.salesordercode", "en-US", "销售订单编码_us", "关联销售订单编码（报价转订单后回填；选项 TaktSalesOrders/options，DictValue=SalesOrderCode）"),
            // entity.salesquotation.salesordercode
            new TranslationSeedItem("entity.salesquotation.salesordercode", "ja-JP", "销售订单编码_jp", "关联销售订单编码（报价转订单后回填；选项 TaktSalesOrders/options，DictValue=SalesOrderCode）"),
            // entity.salesquotation.salesordercode
            new TranslationSeedItem("entity.salesquotation.salesordercode", "zh-CN", "销售订单编码", "关联销售订单编码（报价转订单后回填；选项 TaktSalesOrders/options，DictValue=SalesOrderCode）"),
            // entity.salesquotation.salesordercode
            new TranslationSeedItem("entity.salesquotation.salesordercode", "zh-HK", "销售订单编码_hk", "关联销售订单编码（报价转订单后回填；选项 TaktSalesOrders/options，DictValue=SalesOrderCode）"),

            // entity.salesquotation.quotationstatus
            new TranslationSeedItem("entity.salesquotation.quotationstatus", "en-US", "报价状态_us", "报价状态（字典 logistics_sales_quotation_status；0=草稿 1=已发送 2=已接受 3=已拒绝 4=已过期 5=已作废）"),
            // entity.salesquotation.quotationstatus
            new TranslationSeedItem("entity.salesquotation.quotationstatus", "ja-JP", "报价状态_jp", "报价状态（字典 logistics_sales_quotation_status；0=草稿 1=已发送 2=已接受 3=已拒绝 4=已过期 5=已作废）"),
            // entity.salesquotation.quotationstatus
            new TranslationSeedItem("entity.salesquotation.quotationstatus", "zh-CN", "报价状态", "报价状态（字典 logistics_sales_quotation_status；0=草稿 1=已发送 2=已接受 3=已拒绝 4=已过期 5=已作废）"),
            // entity.salesquotation.quotationstatus
            new TranslationSeedItem("entity.salesquotation.quotationstatus", "zh-HK", "报价状态_hk", "报价状态（字典 logistics_sales_quotation_status；0=草稿 1=已发送 2=已接受 3=已拒绝 4=已过期 5=已作废）"),

            // entity.salesquotation.items
            new TranslationSeedItem("entity.salesquotation.items", "en-US", "销售报价明细列表_us", "销售报价明细列表（主子表关系）"),
            // entity.salesquotation.items
            new TranslationSeedItem("entity.salesquotation.items", "ja-JP", "销售报价明细列表_jp", "销售报价明细列表（主子表关系）"),
            // entity.salesquotation.items
            new TranslationSeedItem("entity.salesquotation.items", "zh-CN", "销售报价明细列表", "销售报价明细列表（主子表关系）"),
            // entity.salesquotation.items
            new TranslationSeedItem("entity.salesquotation.items", "zh-HK", "销售报价明细列表_hk", "销售报价明细列表（主子表关系）"),
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
        translation.ResourceGroup = "Sales";
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
