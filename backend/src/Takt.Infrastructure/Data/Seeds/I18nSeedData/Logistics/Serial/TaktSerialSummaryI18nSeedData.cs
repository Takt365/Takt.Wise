// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Serial
// 文件名称：TaktSerialSummaryI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSerialSummary 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Serial;

/// <summary>
/// TaktSerialSummary 实体国际化翻译种子（键前缀 entity.serialsummary.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSerialSummaryI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSerialSummary 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 serialsummary 实体翻译...", tenantCode);

        foreach (var item in GetSerialSummaryTranslations())
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

        TaktLogger.Information("TaktSerialSummary 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSerialSummary 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.serialsummary._self / entity.serialsummary.{{field}}；ResourceGroup=Serial；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSerialSummaryTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.serialsummary._self
            new TranslationSeedItem("entity.serialsummary._self", "en-US", "Serial Summary Information_us", "实体名称"),
            // entity.serialsummary._self
            new TranslationSeedItem("entity.serialsummary._self", "ja-JP", "序列号汇总信息_jp", "实体名称"),
            // entity.serialsummary._self
            new TranslationSeedItem("entity.serialsummary._self", "zh-CN", "序列号汇总信息", "实体名称"),
            // entity.serialsummary._self
            new TranslationSeedItem("entity.serialsummary._self", "zh-HK", "序列号汇总信息_hk", "实体名称"),

            // entity.serialsummary.inboundcode
            new TranslationSeedItem("entity.serialsummary.inboundcode", "en-US", "入库单号_us", "入库单号"),
            // entity.serialsummary.inboundcode
            new TranslationSeedItem("entity.serialsummary.inboundcode", "ja-JP", "入库单号_jp", "入库单号"),
            // entity.serialsummary.inboundcode
            new TranslationSeedItem("entity.serialsummary.inboundcode", "zh-CN", "入库单号", "入库单号"),
            // entity.serialsummary.inboundcode
            new TranslationSeedItem("entity.serialsummary.inboundcode", "zh-HK", "入库单号_hk", "入库单号"),

            // entity.serialsummary.inbounddate
            new TranslationSeedItem("entity.serialsummary.inbounddate", "en-US", "入库日期_us", "入库日期"),
            // entity.serialsummary.inbounddate
            new TranslationSeedItem("entity.serialsummary.inbounddate", "ja-JP", "入库日期_jp", "入库日期"),
            // entity.serialsummary.inbounddate
            new TranslationSeedItem("entity.serialsummary.inbounddate", "zh-CN", "入库日期", "入库日期"),
            // entity.serialsummary.inbounddate
            new TranslationSeedItem("entity.serialsummary.inbounddate", "zh-HK", "入库日期_hk", "入库日期"),

            // entity.serialsummary.materialcode
            new TranslationSeedItem("entity.serialsummary.materialcode", "en-US", "产品物料_us", "产品物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.serialsummary.materialcode
            new TranslationSeedItem("entity.serialsummary.materialcode", "ja-JP", "产品物料_jp", "产品物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.serialsummary.materialcode
            new TranslationSeedItem("entity.serialsummary.materialcode", "zh-CN", "产品物料", "产品物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.serialsummary.materialcode
            new TranslationSeedItem("entity.serialsummary.materialcode", "zh-HK", "产品物料_hk", "产品物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),

            // entity.serialsummary.inboundserialcode
            new TranslationSeedItem("entity.serialsummary.inboundserialcode", "en-US", "入库序列号_us", "入库序列号（计算后的业务序号；租户+公司+工厂内唯一）"),
            // entity.serialsummary.inboundserialcode
            new TranslationSeedItem("entity.serialsummary.inboundserialcode", "ja-JP", "入库序列号_jp", "入库序列号（计算后的业务序号；租户+公司+工厂内唯一）"),
            // entity.serialsummary.inboundserialcode
            new TranslationSeedItem("entity.serialsummary.inboundserialcode", "zh-CN", "入库序列号", "入库序列号（计算后的业务序号；租户+公司+工厂内唯一）"),
            // entity.serialsummary.inboundserialcode
            new TranslationSeedItem("entity.serialsummary.inboundserialcode", "zh-HK", "入库序列号_hk", "入库序列号（计算后的业务序号；租户+公司+工厂内唯一）"),

            // entity.serialsummary.inboundquantity
            new TranslationSeedItem("entity.serialsummary.inboundquantity", "en-US", "入库数量_us", "入库数量"),
            // entity.serialsummary.inboundquantity
            new TranslationSeedItem("entity.serialsummary.inboundquantity", "ja-JP", "入库数量_jp", "入库数量"),
            // entity.serialsummary.inboundquantity
            new TranslationSeedItem("entity.serialsummary.inboundquantity", "zh-CN", "入库数量", "入库数量"),
            // entity.serialsummary.inboundquantity
            new TranslationSeedItem("entity.serialsummary.inboundquantity", "zh-HK", "入库数量_hk", "入库数量"),

            // entity.serialsummary.productinboundserialcode
            new TranslationSeedItem("entity.serialsummary.productinboundserialcode", "en-US", "产品入库序列号_us", "产品入库序列号（原始扫描号码）"),
            // entity.serialsummary.productinboundserialcode
            new TranslationSeedItem("entity.serialsummary.productinboundserialcode", "ja-JP", "产品入库序列号_jp", "产品入库序列号（原始扫描号码）"),
            // entity.serialsummary.productinboundserialcode
            new TranslationSeedItem("entity.serialsummary.productinboundserialcode", "zh-CN", "产品入库序列号", "产品入库序列号（原始扫描号码）"),
            // entity.serialsummary.productinboundserialcode
            new TranslationSeedItem("entity.serialsummary.productinboundserialcode", "zh-HK", "产品入库序列号_hk", "产品入库序列号（原始扫描号码）"),

            // entity.serialsummary.outboundcode
            new TranslationSeedItem("entity.serialsummary.outboundcode", "en-US", "出库单号_us", "出库单号（未出库时为空）"),
            // entity.serialsummary.outboundcode
            new TranslationSeedItem("entity.serialsummary.outboundcode", "ja-JP", "出库单号_jp", "出库单号（未出库时为空）"),
            // entity.serialsummary.outboundcode
            new TranslationSeedItem("entity.serialsummary.outboundcode", "zh-CN", "出库单号", "出库单号（未出库时为空）"),
            // entity.serialsummary.outboundcode
            new TranslationSeedItem("entity.serialsummary.outboundcode", "zh-HK", "出库单号_hk", "出库单号（未出库时为空）"),

            // entity.serialsummary.shippinginvoicecode
            new TranslationSeedItem("entity.serialsummary.shippinginvoicecode", "en-US", "发货单号_us", "发货单号（未出库时为空）"),
            // entity.serialsummary.shippinginvoicecode
            new TranslationSeedItem("entity.serialsummary.shippinginvoicecode", "ja-JP", "发货单号_jp", "发货单号（未出库时为空）"),
            // entity.serialsummary.shippinginvoicecode
            new TranslationSeedItem("entity.serialsummary.shippinginvoicecode", "zh-CN", "发货单号", "发货单号（未出库时为空）"),
            // entity.serialsummary.shippinginvoicecode
            new TranslationSeedItem("entity.serialsummary.shippinginvoicecode", "zh-HK", "发货单号_hk", "发货单号（未出库时为空）"),

            // entity.serialsummary.loadingdate
            new TranslationSeedItem("entity.serialsummary.loadingdate", "en-US", "装车日期_us", "装车日期（未装车时为空）"),
            // entity.serialsummary.loadingdate
            new TranslationSeedItem("entity.serialsummary.loadingdate", "ja-JP", "装车日期_jp", "装车日期（未装车时为空）"),
            // entity.serialsummary.loadingdate
            new TranslationSeedItem("entity.serialsummary.loadingdate", "zh-CN", "装车日期", "装车日期（未装车时为空）"),
            // entity.serialsummary.loadingdate
            new TranslationSeedItem("entity.serialsummary.loadingdate", "zh-HK", "装车日期_hk", "装车日期（未装车时为空）"),

            // entity.serialsummary.destination
            new TranslationSeedItem("entity.serialsummary.destination", "en-US", "仕向地_us", "仕向地（选项 TaktModelDestinations/options；DictValue=DestinationCode）"),
            // entity.serialsummary.destination
            new TranslationSeedItem("entity.serialsummary.destination", "ja-JP", "仕向地_jp", "仕向地（选项 TaktModelDestinations/options；DictValue=DestinationCode）"),
            // entity.serialsummary.destination
            new TranslationSeedItem("entity.serialsummary.destination", "zh-CN", "仕向地", "仕向地（选项 TaktModelDestinations/options；DictValue=DestinationCode）"),
            // entity.serialsummary.destination
            new TranslationSeedItem("entity.serialsummary.destination", "zh-HK", "仕向地_hk", "仕向地（选项 TaktModelDestinations/options；DictValue=DestinationCode）"),

            // entity.serialsummary.destinationport
            new TranslationSeedItem("entity.serialsummary.destinationport", "en-US", "目的地港_us", "目的地港（字典 logistics_serial_destination_port_code；DictValue 为港口/运输编码，如 ACE_AIR、VIE）"),
            // entity.serialsummary.destinationport
            new TranslationSeedItem("entity.serialsummary.destinationport", "ja-JP", "目的地港_jp", "目的地港（字典 logistics_serial_destination_port_code；DictValue 为港口/运输编码，如 ACE_AIR、VIE）"),
            // entity.serialsummary.destinationport
            new TranslationSeedItem("entity.serialsummary.destinationport", "zh-CN", "目的地港", "目的地港（字典 logistics_serial_destination_port_code；DictValue 为港口/运输编码，如 ACE_AIR、VIE）"),
            // entity.serialsummary.destinationport
            new TranslationSeedItem("entity.serialsummary.destinationport", "zh-HK", "目的地港_hk", "目的地港（字典 logistics_serial_destination_port_code；DictValue 为港口/运输编码，如 ACE_AIR、VIE）"),

            // entity.serialsummary.outbounddate
            new TranslationSeedItem("entity.serialsummary.outbounddate", "en-US", "出库日期_us", "出库日期（未出库时为空）"),
            // entity.serialsummary.outbounddate
            new TranslationSeedItem("entity.serialsummary.outbounddate", "ja-JP", "出库日期_jp", "出库日期（未出库时为空）"),
            // entity.serialsummary.outbounddate
            new TranslationSeedItem("entity.serialsummary.outbounddate", "zh-CN", "出库日期", "出库日期（未出库时为空）"),
            // entity.serialsummary.outbounddate
            new TranslationSeedItem("entity.serialsummary.outbounddate", "zh-HK", "出库日期_hk", "出库日期（未出库时为空）"),

            // entity.serialsummary.outboundserialcode
            new TranslationSeedItem("entity.serialsummary.outboundserialcode", "en-US", "出库序列号_us", "出库序列号（计算后的业务序号；未出库时为空）"),
            // entity.serialsummary.outboundserialcode
            new TranslationSeedItem("entity.serialsummary.outboundserialcode", "ja-JP", "出库序列号_jp", "出库序列号（计算后的业务序号；未出库时为空）"),
            // entity.serialsummary.outboundserialcode
            new TranslationSeedItem("entity.serialsummary.outboundserialcode", "zh-CN", "出库序列号", "出库序列号（计算后的业务序号；未出库时为空）"),
            // entity.serialsummary.outboundserialcode
            new TranslationSeedItem("entity.serialsummary.outboundserialcode", "zh-HK", "出库序列号_hk", "出库序列号（计算后的业务序号；未出库时为空）"),

            // entity.serialsummary.outboundquantity
            new TranslationSeedItem("entity.serialsummary.outboundquantity", "en-US", "出库数量_us", "出库数量"),
            // entity.serialsummary.outboundquantity
            new TranslationSeedItem("entity.serialsummary.outboundquantity", "ja-JP", "出库数量_jp", "出库数量"),
            // entity.serialsummary.outboundquantity
            new TranslationSeedItem("entity.serialsummary.outboundquantity", "zh-CN", "出库数量", "出库数量"),
            // entity.serialsummary.outboundquantity
            new TranslationSeedItem("entity.serialsummary.outboundquantity", "zh-HK", "出库数量_hk", "出库数量"),

            // entity.serialsummary.productoutboundserialcode
            new TranslationSeedItem("entity.serialsummary.productoutboundserialcode", "en-US", "产品出库序列号_us", "产品出库序列号（原始扫描号码；未出库时为空）"),
            // entity.serialsummary.productoutboundserialcode
            new TranslationSeedItem("entity.serialsummary.productoutboundserialcode", "ja-JP", "产品出库序列号_jp", "产品出库序列号（原始扫描号码；未出库时为空）"),
            // entity.serialsummary.productoutboundserialcode
            new TranslationSeedItem("entity.serialsummary.productoutboundserialcode", "zh-CN", "产品出库序列号", "产品出库序列号（原始扫描号码；未出库时为空）"),
            // entity.serialsummary.productoutboundserialcode
            new TranslationSeedItem("entity.serialsummary.productoutboundserialcode", "zh-HK", "产品出库序列号_hk", "产品出库序列号（原始扫描号码；未出库时为空）"),
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
        translation.ResourceGroup = "Serial";
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
