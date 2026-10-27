// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales
// 文件名称：TaktSalesConditionI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSalesCondition 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktSalesCondition 实体国际化翻译种子（键前缀 entity.salescondition.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSalesConditionI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSalesCondition 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 salescondition 实体翻译...", tenantCode);

        foreach (var item in GetSalesConditionTranslations())
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

        TaktLogger.Information("TaktSalesCondition 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSalesCondition 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.salescondition._self / entity.salescondition.{{field}}；ResourceGroup=Sales；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSalesConditionTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.salescondition._self
            new TranslationSeedItem("entity.salescondition._self", "en-US", "Sales Condition Information_us", "实体名称"),
            // entity.salescondition._self
            new TranslationSeedItem("entity.salescondition._self", "ja-JP", "Takt销售条件信息_jp", "实体名称"),
            // entity.salescondition._self
            new TranslationSeedItem("entity.salescondition._self", "zh-CN", "Takt销售条件信息", "实体名称"),
            // entity.salescondition._self
            new TranslationSeedItem("entity.salescondition._self", "zh-HK", "Takt销售条件信息_hk", "实体名称"),

            // entity.salescondition.code
            new TranslationSeedItem("entity.salescondition.code", "en-US", "定价记录号_us", "定价记录号（租户+公司+工厂内唯一；长度 20；前端表单选择编码规则后自动通过 TaktNumbering 定价记录号规则生成并展示，非手输；单据类型菜单：销售条件）"),
            // entity.salescondition.code
            new TranslationSeedItem("entity.salescondition.code", "ja-JP", "定价记录号_jp", "定价记录号（租户+公司+工厂内唯一；长度 20；前端表单选择编码规则后自动通过 TaktNumbering 定价记录号规则生成并展示，非手输；单据类型菜单：销售条件）"),
            // entity.salescondition.code
            new TranslationSeedItem("entity.salescondition.code", "zh-CN", "定价记录号", "定价记录号（租户+公司+工厂内唯一；长度 20；前端表单选择编码规则后自动通过 TaktNumbering 定价记录号规则生成并展示，非手输；单据类型菜单：销售条件）"),
            // entity.salescondition.code
            new TranslationSeedItem("entity.salescondition.code", "zh-HK", "定价记录号_hk", "定价记录号（租户+公司+工厂内唯一；长度 20；前端表单选择编码规则后自动通过 TaktNumbering 定价记录号规则生成并展示，非手输；单据类型菜单：销售条件）"),

            // entity.salescondition.pricetype
            new TranslationSeedItem("entity.salescondition.pricetype", "en-US", "条件类型_us", "条件类型（字典 logistics_procurement_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）"),
            // entity.salescondition.pricetype
            new TranslationSeedItem("entity.salescondition.pricetype", "ja-JP", "条件类型_jp", "条件类型（字典 logistics_procurement_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）"),
            // entity.salescondition.pricetype
            new TranslationSeedItem("entity.salescondition.pricetype", "zh-CN", "条件类型", "条件类型（字典 logistics_procurement_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）"),
            // entity.salescondition.pricetype
            new TranslationSeedItem("entity.salescondition.pricetype", "zh-HK", "条件类型_hk", "条件类型（字典 logistics_procurement_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）"),

            // entity.salescondition.customercode
            new TranslationSeedItem("entity.salescondition.customercode", "en-US", "客户_us", "客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）"),
            // entity.salescondition.customercode
            new TranslationSeedItem("entity.salescondition.customercode", "ja-JP", "客户_jp", "客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）"),
            // entity.salescondition.customercode
            new TranslationSeedItem("entity.salescondition.customercode", "zh-CN", "客户", "客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）"),
            // entity.salescondition.customercode
            new TranslationSeedItem("entity.salescondition.customercode", "zh-HK", "客户_hk", "客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）"),

            // entity.salescondition.materialcode
            new TranslationSeedItem("entity.salescondition.materialcode", "en-US", "物料_us", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）"),
            // entity.salescondition.materialcode
            new TranslationSeedItem("entity.salescondition.materialcode", "ja-JP", "物料_jp", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）"),
            // entity.salescondition.materialcode
            new TranslationSeedItem("entity.salescondition.materialcode", "zh-CN", "物料", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）"),
            // entity.salescondition.materialcode
            new TranslationSeedItem("entity.salescondition.materialcode", "zh-HK", "物料_hk", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）"),

            // entity.salescondition.materialdescription
            new TranslationSeedItem("entity.salescondition.materialdescription", "en-US", "物料描述_us", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.salescondition.materialdescription
            new TranslationSeedItem("entity.salescondition.materialdescription", "ja-JP", "物料描述_jp", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.salescondition.materialdescription
            new TranslationSeedItem("entity.salescondition.materialdescription", "zh-CN", "物料描述", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.salescondition.materialdescription
            new TranslationSeedItem("entity.salescondition.materialdescription", "zh-HK", "物料描述_hk", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),

            // entity.salescondition.salesgroup
            new TranslationSeedItem("entity.salescondition.salesgroup", "en-US", "销售组_us", "销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）"),
            // entity.salescondition.salesgroup
            new TranslationSeedItem("entity.salescondition.salesgroup", "ja-JP", "销售组_jp", "销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）"),
            // entity.salescondition.salesgroup
            new TranslationSeedItem("entity.salescondition.salesgroup", "zh-CN", "销售组", "销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）"),
            // entity.salescondition.salesgroup
            new TranslationSeedItem("entity.salescondition.salesgroup", "zh-HK", "销售组_hk", "销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）"),

            // entity.salescondition.taxcode
            new TranslationSeedItem("entity.salescondition.taxcode", "en-US", "税码_us", "税码（选项 TaktTaxCodes/options；DictValue=TaxCode）"),
            // entity.salescondition.taxcode
            new TranslationSeedItem("entity.salescondition.taxcode", "ja-JP", "税码_jp", "税码（选项 TaktTaxCodes/options；DictValue=TaxCode）"),
            // entity.salescondition.taxcode
            new TranslationSeedItem("entity.salescondition.taxcode", "zh-CN", "税码", "税码（选项 TaktTaxCodes/options；DictValue=TaxCode）"),
            // entity.salescondition.taxcode
            new TranslationSeedItem("entity.salescondition.taxcode", "zh-HK", "税码_hk", "税码（选项 TaktTaxCodes/options；DictValue=TaxCode）"),

            // entity.salescondition.grbasedinvoiceinspection
            new TranslationSeedItem("entity.salescondition.grbasedinvoiceinspection", "en-US", "基于收货的发票检验_us", "基于收货的发票检验（字典 sys_yes_no；0=否 1=是）"),
            // entity.salescondition.grbasedinvoiceinspection
            new TranslationSeedItem("entity.salescondition.grbasedinvoiceinspection", "ja-JP", "基于收货的发票检验_jp", "基于收货的发票检验（字典 sys_yes_no；0=否 1=是）"),
            // entity.salescondition.grbasedinvoiceinspection
            new TranslationSeedItem("entity.salescondition.grbasedinvoiceinspection", "zh-CN", "基于收货的发票检验", "基于收货的发票检验（字典 sys_yes_no；0=否 1=是）"),
            // entity.salescondition.grbasedinvoiceinspection
            new TranslationSeedItem("entity.salescondition.grbasedinvoiceinspection", "zh-HK", "基于收货的发票检验_hk", "基于收货的发票检验（字典 sys_yes_no；0=否 1=是）"),

            // entity.salescondition.pricingdatecontrol
            new TranslationSeedItem("entity.salescondition.pricingdatecontrol", "en-US", "定价日期控制_us", "定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）"),
            // entity.salescondition.pricingdatecontrol
            new TranslationSeedItem("entity.salescondition.pricingdatecontrol", "ja-JP", "定价日期控制_jp", "定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）"),
            // entity.salescondition.pricingdatecontrol
            new TranslationSeedItem("entity.salescondition.pricingdatecontrol", "zh-CN", "定价日期控制", "定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）"),
            // entity.salescondition.pricingdatecontrol
            new TranslationSeedItem("entity.salescondition.pricingdatecontrol", "zh-HK", "定价日期控制_hk", "定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）"),

            // entity.salescondition.validfrom
            new TranslationSeedItem("entity.salescondition.validfrom", "en-US", "有效起始日_us", "有效起始日"),
            // entity.salescondition.validfrom
            new TranslationSeedItem("entity.salescondition.validfrom", "ja-JP", "有效起始日_jp", "有效起始日"),
            // entity.salescondition.validfrom
            new TranslationSeedItem("entity.salescondition.validfrom", "zh-CN", "有效起始日", "有效起始日"),
            // entity.salescondition.validfrom
            new TranslationSeedItem("entity.salescondition.validfrom", "zh-HK", "有效起始日_hk", "有效起始日"),

            // entity.salescondition.validto
            new TranslationSeedItem("entity.salescondition.validto", "en-US", "有效截至日_us", "有效截至日"),
            // entity.salescondition.validto
            new TranslationSeedItem("entity.salescondition.validto", "ja-JP", "有效截至日_jp", "有效截至日"),
            // entity.salescondition.validto
            new TranslationSeedItem("entity.salescondition.validto", "zh-CN", "有效截至日", "有效截至日"),
            // entity.salescondition.validto
            new TranslationSeedItem("entity.salescondition.validto", "zh-HK", "有效截至日_hk", "有效截至日"),

            // entity.salescondition.salesquotationid
            new TranslationSeedItem("entity.salescondition.salesquotationid", "en-US", "来源销售报价ID_us", "来源销售报价 ID（选项 TaktSalesQuotations/options；DictValue=Id；对应采购侧来源询价）"),
            // entity.salescondition.salesquotationid
            new TranslationSeedItem("entity.salescondition.salesquotationid", "ja-JP", "来源销售报价ID_jp", "来源销售报价 ID（选项 TaktSalesQuotations/options；DictValue=Id；对应采购侧来源询价）"),
            // entity.salescondition.salesquotationid
            new TranslationSeedItem("entity.salescondition.salesquotationid", "zh-CN", "来源销售报价ID", "来源销售报价 ID（选项 TaktSalesQuotations/options；DictValue=Id；对应采购侧来源询价）"),
            // entity.salescondition.salesquotationid
            new TranslationSeedItem("entity.salescondition.salesquotationid", "zh-HK", "来源销售报价ID_hk", "来源销售报价 ID（选项 TaktSalesQuotations/options；DictValue=Id；对应采购侧来源询价）"),

            // entity.salescondition.salesquotationcode
            new TranslationSeedItem("entity.salescondition.salesquotationcode", "en-US", "来源销售报价编码_us", "来源销售报价编码（冗余）"),
            // entity.salescondition.salesquotationcode
            new TranslationSeedItem("entity.salescondition.salesquotationcode", "ja-JP", "来源销售报价编码_jp", "来源销售报价编码（冗余）"),
            // entity.salescondition.salesquotationcode
            new TranslationSeedItem("entity.salescondition.salesquotationcode", "zh-CN", "来源销售报价编码", "来源销售报价编码（冗余）"),
            // entity.salescondition.salesquotationcode
            new TranslationSeedItem("entity.salescondition.salesquotationcode", "zh-HK", "来源销售报价编码_hk", "来源销售报价编码（冗余）"),

            // entity.salescondition.variablekey
            new TranslationSeedItem("entity.salescondition.variablekey", "en-US", "可变关键字_us", "可变关键字"),
            // entity.salescondition.variablekey
            new TranslationSeedItem("entity.salescondition.variablekey", "ja-JP", "可变关键字_jp", "可变关键字"),
            // entity.salescondition.variablekey
            new TranslationSeedItem("entity.salescondition.variablekey", "zh-CN", "可变关键字", "可变关键字"),
            // entity.salescondition.variablekey
            new TranslationSeedItem("entity.salescondition.variablekey", "zh-HK", "可变关键字_hk", "可变关键字"),

            // entity.salescondition.items
            new TranslationSeedItem("entity.salescondition.items", "en-US", "定价条件行列表_us", "定价条件行列表（主子表关系）"),
            // entity.salescondition.items
            new TranslationSeedItem("entity.salescondition.items", "ja-JP", "定价条件行列表_jp", "定价条件行列表（主子表关系）"),
            // entity.salescondition.items
            new TranslationSeedItem("entity.salescondition.items", "zh-CN", "定价条件行列表", "定价条件行列表（主子表关系）"),
            // entity.salescondition.items
            new TranslationSeedItem("entity.salescondition.items", "zh-HK", "定价条件行列表_hk", "定价条件行列表（主子表关系）"),
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
