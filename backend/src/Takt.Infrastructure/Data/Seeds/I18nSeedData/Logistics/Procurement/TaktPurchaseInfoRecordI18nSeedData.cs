// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Procurement
// 文件名称：TaktPurchaseInfoRecordI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPurchaseInfoRecord 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktPurchaseInfoRecord 实体国际化翻译种子（键前缀 entity.purchaseinforecord.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPurchaseInfoRecordI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPurchaseInfoRecord 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 purchaseinforecord 实体翻译...", tenantCode);

        foreach (var item in GetPurchaseInfoRecordTranslations())
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

        TaktLogger.Information("TaktPurchaseInfoRecord 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPurchaseInfoRecord 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.purchaseinforecord._self / entity.purchaseinforecord.{{field}}；ResourceGroup=Procurement；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPurchaseInfoRecordTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.purchaseinforecord._self
            new TranslationSeedItem("entity.purchaseinforecord._self", "en-US", "Purchase Info Record Information_us", "实体名称"),
            // entity.purchaseinforecord._self
            new TranslationSeedItem("entity.purchaseinforecord._self", "ja-JP", "Takt采购信息记录信息_jp", "实体名称"),
            // entity.purchaseinforecord._self
            new TranslationSeedItem("entity.purchaseinforecord._self", "zh-CN", "Takt采购信息记录信息", "实体名称"),
            // entity.purchaseinforecord._self
            new TranslationSeedItem("entity.purchaseinforecord._self", "zh-HK", "Takt采购信息记录信息_hk", "实体名称"),

            // entity.purchaseinforecord.code
            new TranslationSeedItem("entity.purchaseinforecord.code", "en-US", "定价记录号_us", "定价记录号（唯一索引；长度 20）"),
            // entity.purchaseinforecord.code
            new TranslationSeedItem("entity.purchaseinforecord.code", "ja-JP", "定价记录号_jp", "定价记录号（唯一索引；长度 20）"),
            // entity.purchaseinforecord.code
            new TranslationSeedItem("entity.purchaseinforecord.code", "zh-CN", "定价记录号", "定价记录号（唯一索引；长度 20）"),
            // entity.purchaseinforecord.code
            new TranslationSeedItem("entity.purchaseinforecord.code", "zh-HK", "定价记录号_hk", "定价记录号（唯一索引；长度 20）"),

            // entity.purchaseinforecord.pricetype
            new TranslationSeedItem("entity.purchaseinforecord.pricetype", "en-US", "条件类型_us", "条件类型（字典 logistics_procurement_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）"),
            // entity.purchaseinforecord.pricetype
            new TranslationSeedItem("entity.purchaseinforecord.pricetype", "ja-JP", "条件类型_jp", "条件类型（字典 logistics_procurement_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）"),
            // entity.purchaseinforecord.pricetype
            new TranslationSeedItem("entity.purchaseinforecord.pricetype", "zh-CN", "条件类型", "条件类型（字典 logistics_procurement_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）"),
            // entity.purchaseinforecord.pricetype
            new TranslationSeedItem("entity.purchaseinforecord.pricetype", "zh-HK", "条件类型_hk", "条件类型（字典 logistics_procurement_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）"),

            // entity.purchaseinforecord.suppliercode
            new TranslationSeedItem("entity.purchaseinforecord.suppliercode", "en-US", "供应商_us", "供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.purchaseinforecord.suppliercode
            new TranslationSeedItem("entity.purchaseinforecord.suppliercode", "ja-JP", "供应商_jp", "供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.purchaseinforecord.suppliercode
            new TranslationSeedItem("entity.purchaseinforecord.suppliercode", "zh-CN", "供应商", "供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.purchaseinforecord.suppliercode
            new TranslationSeedItem("entity.purchaseinforecord.suppliercode", "zh-HK", "供应商_hk", "供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）"),

            // entity.purchaseinforecord.materialcode
            new TranslationSeedItem("entity.purchaseinforecord.materialcode", "en-US", "物料_us", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）"),
            // entity.purchaseinforecord.materialcode
            new TranslationSeedItem("entity.purchaseinforecord.materialcode", "ja-JP", "物料_jp", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）"),
            // entity.purchaseinforecord.materialcode
            new TranslationSeedItem("entity.purchaseinforecord.materialcode", "zh-CN", "物料", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）"),
            // entity.purchaseinforecord.materialcode
            new TranslationSeedItem("entity.purchaseinforecord.materialcode", "zh-HK", "物料_hk", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）"),

            // entity.purchaseinforecord.materialdescription
            new TranslationSeedItem("entity.purchaseinforecord.materialdescription", "en-US", "物料描述_us", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.purchaseinforecord.materialdescription
            new TranslationSeedItem("entity.purchaseinforecord.materialdescription", "ja-JP", "物料描述_jp", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.purchaseinforecord.materialdescription
            new TranslationSeedItem("entity.purchaseinforecord.materialdescription", "zh-CN", "物料描述", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.purchaseinforecord.materialdescription
            new TranslationSeedItem("entity.purchaseinforecord.materialdescription", "zh-HK", "物料描述_hk", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),

            // entity.purchaseinforecord.purchasegroup
            new TranslationSeedItem("entity.purchaseinforecord.purchasegroup", "en-US", "采购组_us", "采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）"),
            // entity.purchaseinforecord.purchasegroup
            new TranslationSeedItem("entity.purchaseinforecord.purchasegroup", "ja-JP", "采购组_jp", "采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）"),
            // entity.purchaseinforecord.purchasegroup
            new TranslationSeedItem("entity.purchaseinforecord.purchasegroup", "zh-CN", "采购组", "采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）"),
            // entity.purchaseinforecord.purchasegroup
            new TranslationSeedItem("entity.purchaseinforecord.purchasegroup", "zh-HK", "采购组_hk", "采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）"),

            // entity.purchaseinforecord.taxcode
            new TranslationSeedItem("entity.purchaseinforecord.taxcode", "en-US", "税码_us", "税码（字典 accounting_financial_tax_code；DictValue=J0～J8/L1/X0～X3；中国）"),
            // entity.purchaseinforecord.taxcode
            new TranslationSeedItem("entity.purchaseinforecord.taxcode", "ja-JP", "税码_jp", "税码（字典 accounting_financial_tax_code；DictValue=J0～J8/L1/X0～X3；中国）"),
            // entity.purchaseinforecord.taxcode
            new TranslationSeedItem("entity.purchaseinforecord.taxcode", "zh-CN", "税码", "税码（字典 accounting_financial_tax_code；DictValue=J0～J8/L1/X0～X3；中国）"),
            // entity.purchaseinforecord.taxcode
            new TranslationSeedItem("entity.purchaseinforecord.taxcode", "zh-HK", "税码_hk", "税码（字典 accounting_financial_tax_code；DictValue=J0～J8/L1/X0～X3；中国）"),

            // entity.purchaseinforecord.grbasedinvoiceinspection
            new TranslationSeedItem("entity.purchaseinforecord.grbasedinvoiceinspection", "en-US", "基于收货的发票检验_us", "基于收货的发票检验（字典 sys_yes_no；0=否 1=是）"),
            // entity.purchaseinforecord.grbasedinvoiceinspection
            new TranslationSeedItem("entity.purchaseinforecord.grbasedinvoiceinspection", "ja-JP", "基于收货的发票检验_jp", "基于收货的发票检验（字典 sys_yes_no；0=否 1=是）"),
            // entity.purchaseinforecord.grbasedinvoiceinspection
            new TranslationSeedItem("entity.purchaseinforecord.grbasedinvoiceinspection", "zh-CN", "基于收货的发票检验", "基于收货的发票检验（字典 sys_yes_no；0=否 1=是）"),
            // entity.purchaseinforecord.grbasedinvoiceinspection
            new TranslationSeedItem("entity.purchaseinforecord.grbasedinvoiceinspection", "zh-HK", "基于收货的发票检验_hk", "基于收货的发票检验（字典 sys_yes_no；0=否 1=是）"),

            // entity.purchaseinforecord.pricingdatecontrol
            new TranslationSeedItem("entity.purchaseinforecord.pricingdatecontrol", "en-US", "定价日期控制_us", "定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）"),
            // entity.purchaseinforecord.pricingdatecontrol
            new TranslationSeedItem("entity.purchaseinforecord.pricingdatecontrol", "ja-JP", "定价日期控制_jp", "定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）"),
            // entity.purchaseinforecord.pricingdatecontrol
            new TranslationSeedItem("entity.purchaseinforecord.pricingdatecontrol", "zh-CN", "定价日期控制", "定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）"),
            // entity.purchaseinforecord.pricingdatecontrol
            new TranslationSeedItem("entity.purchaseinforecord.pricingdatecontrol", "zh-HK", "定价日期控制_hk", "定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）"),

            // entity.purchaseinforecord.validfrom
            new TranslationSeedItem("entity.purchaseinforecord.validfrom", "en-US", "有效起始日_us", "有效起始日"),
            // entity.purchaseinforecord.validfrom
            new TranslationSeedItem("entity.purchaseinforecord.validfrom", "ja-JP", "有效起始日_jp", "有效起始日"),
            // entity.purchaseinforecord.validfrom
            new TranslationSeedItem("entity.purchaseinforecord.validfrom", "zh-CN", "有效起始日", "有效起始日"),
            // entity.purchaseinforecord.validfrom
            new TranslationSeedItem("entity.purchaseinforecord.validfrom", "zh-HK", "有效起始日_hk", "有效起始日"),

            // entity.purchaseinforecord.validto
            new TranslationSeedItem("entity.purchaseinforecord.validto", "en-US", "有效截至日_us", "有效截至日"),
            // entity.purchaseinforecord.validto
            new TranslationSeedItem("entity.purchaseinforecord.validto", "ja-JP", "有效截至日_jp", "有效截至日"),
            // entity.purchaseinforecord.validto
            new TranslationSeedItem("entity.purchaseinforecord.validto", "zh-CN", "有效截至日", "有效截至日"),
            // entity.purchaseinforecord.validto
            new TranslationSeedItem("entity.purchaseinforecord.validto", "zh-HK", "有效截至日_hk", "有效截至日"),

            // entity.purchaseinforecord.purchaseinquiryid
            new TranslationSeedItem("entity.purchaseinforecord.purchaseinquiryid", "en-US", "来源采购询价ID_us", "来源采购询价 ID（选项 TaktPurchaseInquirys/options；DictValue=Id）"),
            // entity.purchaseinforecord.purchaseinquiryid
            new TranslationSeedItem("entity.purchaseinforecord.purchaseinquiryid", "ja-JP", "来源采购询价ID_jp", "来源采购询价 ID（选项 TaktPurchaseInquirys/options；DictValue=Id）"),
            // entity.purchaseinforecord.purchaseinquiryid
            new TranslationSeedItem("entity.purchaseinforecord.purchaseinquiryid", "zh-CN", "来源采购询价ID", "来源采购询价 ID（选项 TaktPurchaseInquirys/options；DictValue=Id）"),
            // entity.purchaseinforecord.purchaseinquiryid
            new TranslationSeedItem("entity.purchaseinforecord.purchaseinquiryid", "zh-HK", "来源采购询价ID_hk", "来源采购询价 ID（选项 TaktPurchaseInquirys/options；DictValue=Id）"),

            // entity.purchaseinforecord.purchaseinquirycode
            new TranslationSeedItem("entity.purchaseinforecord.purchaseinquirycode", "en-US", "来源采购询价编码_us", "来源采购询价编码（冗余）"),
            // entity.purchaseinforecord.purchaseinquirycode
            new TranslationSeedItem("entity.purchaseinforecord.purchaseinquirycode", "ja-JP", "来源采购询价编码_jp", "来源采购询价编码（冗余）"),
            // entity.purchaseinforecord.purchaseinquirycode
            new TranslationSeedItem("entity.purchaseinforecord.purchaseinquirycode", "zh-CN", "来源采购询价编码", "来源采购询价编码（冗余）"),
            // entity.purchaseinforecord.purchaseinquirycode
            new TranslationSeedItem("entity.purchaseinforecord.purchaseinquirycode", "zh-HK", "来源采购询价编码_hk", "来源采购询价编码（冗余）"),

            // entity.purchaseinforecord.variablekey
            new TranslationSeedItem("entity.purchaseinforecord.variablekey", "en-US", "可变关键字_us", "可变关键字"),
            // entity.purchaseinforecord.variablekey
            new TranslationSeedItem("entity.purchaseinforecord.variablekey", "ja-JP", "可变关键字_jp", "可变关键字"),
            // entity.purchaseinforecord.variablekey
            new TranslationSeedItem("entity.purchaseinforecord.variablekey", "zh-CN", "可变关键字", "可变关键字"),
            // entity.purchaseinforecord.variablekey
            new TranslationSeedItem("entity.purchaseinforecord.variablekey", "zh-HK", "可变关键字_hk", "可变关键字"),

            // entity.purchaseinforecord.items
            new TranslationSeedItem("entity.purchaseinforecord.items", "en-US", "定价条件行列表_us", "定价条件行列表（主子表关系）"),
            // entity.purchaseinforecord.items
            new TranslationSeedItem("entity.purchaseinforecord.items", "ja-JP", "定价条件行列表_jp", "定价条件行列表（主子表关系）"),
            // entity.purchaseinforecord.items
            new TranslationSeedItem("entity.purchaseinforecord.items", "zh-CN", "定价条件行列表", "定价条件行列表（主子表关系）"),
            // entity.purchaseinforecord.items
            new TranslationSeedItem("entity.purchaseinforecord.items", "zh-HK", "定价条件行列表_hk", "定价条件行列表（主子表关系）"),
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
