// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials
// 文件名称：TaktMaterialMovingPriceI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktMaterialMovingPrice 实体字段国际化种子（已对齐前端 locales：src/locales/logistics/materials/material-moving-price）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials;

/// <summary>
/// TaktMaterialMovingPrice 实体国际化翻译种子（键前缀 entity.materialmovingprice.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktMaterialMovingPriceI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktMaterialMovingPrice 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 materialmovingprice 实体翻译...", tenantCode);

        foreach (var item in GetMaterialMovingPriceTranslations())
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

        TaktLogger.Information("TaktMaterialMovingPrice 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktMaterialMovingPrice 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.materialmovingprice._self / entity.materialmovingprice.{{field}}；ResourceGroup=Materials；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetMaterialMovingPriceTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.materialmovingprice._self
            new TranslationSeedItem("entity.materialmovingprice._self", "en-US", "Material Moving Price Information_us", "实体名称"),
            // entity.materialmovingprice._self
            new TranslationSeedItem("entity.materialmovingprice._self", "ja-JP", "移动价格信息_jp", "实体名称"),
            // entity.materialmovingprice._self
            new TranslationSeedItem("entity.materialmovingprice._self", "zh-CN", "移动价格信息", "实体名称"),
            // entity.materialmovingprice._self
            new TranslationSeedItem("entity.materialmovingprice._self", "zh-HK", "移动价格信息_hk", "实体名称"),

            // entity.materialmovingprice.valuationperiod
            new TranslationSeedItem("entity.materialmovingprice.valuationperiod", "en-US", "评估期间_us", "评估期间（yyyy-MM；与工厂+物料编码构成业务唯一键）"),
            // entity.materialmovingprice.valuationperiod
            new TranslationSeedItem("entity.materialmovingprice.valuationperiod", "ja-JP", "评估期间_jp", "评估期间（yyyy-MM；与工厂+物料编码构成业务唯一键）"),
            // entity.materialmovingprice.valuationperiod
            new TranslationSeedItem("entity.materialmovingprice.valuationperiod", "zh-CN", "评估期间", "评估期间（yyyy-MM；与工厂+物料编码构成业务唯一键）"),
            // entity.materialmovingprice.valuationperiod
            new TranslationSeedItem("entity.materialmovingprice.valuationperiod", "zh-HK", "评估期间_hk", "评估期间（yyyy-MM；与工厂+物料编码构成业务唯一键）"),

            // entity.materialmovingprice.materialcode
            new TranslationSeedItem("entity.materialmovingprice.materialcode", "en-US", "物料编码_us", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.materialmovingprice.materialcode
            new TranslationSeedItem("entity.materialmovingprice.materialcode", "ja-JP", "物料编码_jp", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.materialmovingprice.materialcode
            new TranslationSeedItem("entity.materialmovingprice.materialcode", "zh-CN", "物料编码", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.materialmovingprice.materialcode
            new TranslationSeedItem("entity.materialmovingprice.materialcode", "zh-HK", "物料编码_hk", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),

            // entity.materialmovingprice.valuation
            new TranslationSeedItem("entity.materialmovingprice.valuation", "en-US", "评估类别_us", "评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）"),
            // entity.materialmovingprice.valuation
            new TranslationSeedItem("entity.materialmovingprice.valuation", "ja-JP", "评估类别_jp", "评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）"),
            // entity.materialmovingprice.valuation
            new TranslationSeedItem("entity.materialmovingprice.valuation", "zh-CN", "评估类别", "评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）"),
            // entity.materialmovingprice.valuation
            new TranslationSeedItem("entity.materialmovingprice.valuation", "zh-HK", "评估类别_hk", "评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）"),

            // entity.materialmovingprice.stockquantity
            new TranslationSeedItem("entity.materialmovingprice.stockquantity", "en-US", "库存数量_us", "库存数量（基本单位，4 位小数）"),
            // entity.materialmovingprice.stockquantity
            new TranslationSeedItem("entity.materialmovingprice.stockquantity", "ja-JP", "库存数量_jp", "库存数量（基本单位，4 位小数）"),
            // entity.materialmovingprice.stockquantity
            new TranslationSeedItem("entity.materialmovingprice.stockquantity", "zh-CN", "库存数量", "库存数量（基本单位，4 位小数）"),
            // entity.materialmovingprice.stockquantity
            new TranslationSeedItem("entity.materialmovingprice.stockquantity", "zh-HK", "库存数量_hk", "库存数量（基本单位，4 位小数）"),

            // entity.materialmovingprice.stockamount
            new TranslationSeedItem("entity.materialmovingprice.stockamount", "en-US", "库存金额_us", "库存金额（与币种一致，2 位小数）"),
            // entity.materialmovingprice.stockamount
            new TranslationSeedItem("entity.materialmovingprice.stockamount", "ja-JP", "库存金额_jp", "库存金额（与币种一致，2 位小数）"),
            // entity.materialmovingprice.stockamount
            new TranslationSeedItem("entity.materialmovingprice.stockamount", "zh-CN", "库存金额", "库存金额（与币种一致，2 位小数）"),
            // entity.materialmovingprice.stockamount
            new TranslationSeedItem("entity.materialmovingprice.stockamount", "zh-HK", "库存金额_hk", "库存金额（与币种一致，2 位小数）"),

            // entity.materialmovingprice.pricecontrol
            new TranslationSeedItem("entity.materialmovingprice.pricecontrol", "en-US", "价格控制_us", "价格控制（字典 logistics_materials_price_control；S=标准价格，V=移动平均价格/周期单价；默认 V）"),
            // entity.materialmovingprice.pricecontrol
            new TranslationSeedItem("entity.materialmovingprice.pricecontrol", "ja-JP", "价格控制_jp", "价格控制（字典 logistics_materials_price_control；S=标准价格，V=移动平均价格/周期单价；默认 V）"),
            // entity.materialmovingprice.pricecontrol
            new TranslationSeedItem("entity.materialmovingprice.pricecontrol", "zh-CN", "价格控制", "价格控制（字典 logistics_materials_price_control；S=标准价格，V=移动平均价格/周期单价；默认 V）"),
            // entity.materialmovingprice.pricecontrol
            new TranslationSeedItem("entity.materialmovingprice.pricecontrol", "zh-HK", "价格控制_hk", "价格控制（字典 logistics_materials_price_control；S=标准价格，V=移动平均价格/周期单价；默认 V）"),

            // entity.materialmovingprice.movingprice
            new TranslationSeedItem("entity.materialmovingprice.movingprice", "en-US", "移动价格_us", "移动价格（decimal，5 位小数；相对价格单位）"),
            // entity.materialmovingprice.movingprice
            new TranslationSeedItem("entity.materialmovingprice.movingprice", "ja-JP", "移动价格_jp", "移动价格（decimal，5 位小数；相对价格单位）"),
            // entity.materialmovingprice.movingprice
            new TranslationSeedItem("entity.materialmovingprice.movingprice", "zh-CN", "移动价格", "移动价格（decimal，5 位小数；相对价格单位）"),
            // entity.materialmovingprice.movingprice
            new TranslationSeedItem("entity.materialmovingprice.movingprice", "zh-HK", "移动价格_hk", "移动价格（decimal，5 位小数；相对价格单位）"),

            // entity.materialmovingprice.priceunit
            new TranslationSeedItem("entity.materialmovingprice.priceunit", "en-US", "价格单位_us", "价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）"),
            // entity.materialmovingprice.priceunit
            new TranslationSeedItem("entity.materialmovingprice.priceunit", "ja-JP", "价格单位_jp", "价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）"),
            // entity.materialmovingprice.priceunit
            new TranslationSeedItem("entity.materialmovingprice.priceunit", "zh-CN", "价格单位", "价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）"),
            // entity.materialmovingprice.priceunit
            new TranslationSeedItem("entity.materialmovingprice.priceunit", "zh-HK", "价格单位_hk", "价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）"),

            // entity.materialmovingprice.currencycode
            new TranslationSeedItem("entity.materialmovingprice.currencycode", "en-US", "币种_us", "币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),
            // entity.materialmovingprice.currencycode
            new TranslationSeedItem("entity.materialmovingprice.currencycode", "ja-JP", "币种_jp", "币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),
            // entity.materialmovingprice.currencycode
            new TranslationSeedItem("entity.materialmovingprice.currencycode", "zh-CN", "币种", "币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),
            // entity.materialmovingprice.currencycode
            new TranslationSeedItem("entity.materialmovingprice.currencycode", "zh-HK", "币种_hk", "币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),
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
        translation.ResourceGroup = "Materials";
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
