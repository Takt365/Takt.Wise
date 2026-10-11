// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Serial
// 文件名称：TaktSerialOutboundI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSerialOutbound 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktSerialOutbound 实体国际化翻译种子（键前缀 entity.serialoutbound.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSerialOutboundI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSerialOutbound 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 serialoutbound 实体翻译...", tenantCode);

        foreach (var item in GetSerialOutboundTranslations())
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

        TaktLogger.Information("TaktSerialOutbound 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSerialOutbound 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.serialoutbound._self / entity.serialoutbound.{{field}}；ResourceGroup=Serial；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSerialOutboundTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.serialoutbound._self
            new TranslationSeedItem("entity.serialoutbound._self", "en-US", "Serial Outbound Information_us", "实体名称"),
            // entity.serialoutbound._self
            new TranslationSeedItem("entity.serialoutbound._self", "ja-JP", "序列号出库主表信息_jp", "实体名称"),
            // entity.serialoutbound._self
            new TranslationSeedItem("entity.serialoutbound._self", "zh-CN", "序列号出库主表信息", "实体名称"),
            // entity.serialoutbound._self
            new TranslationSeedItem("entity.serialoutbound._self", "zh-HK", "序列号出库主表信息_hk", "实体名称"),

            // entity.serialoutbound.outboundcode
            new TranslationSeedItem("entity.serialoutbound.outboundcode", "en-US", "出库单号_us", "出库单号（租户+公司+工厂内唯一）"),
            // entity.serialoutbound.outboundcode
            new TranslationSeedItem("entity.serialoutbound.outboundcode", "ja-JP", "出库单号_jp", "出库单号（租户+公司+工厂内唯一）"),
            // entity.serialoutbound.outboundcode
            new TranslationSeedItem("entity.serialoutbound.outboundcode", "zh-CN", "出库单号", "出库单号（租户+公司+工厂内唯一）"),
            // entity.serialoutbound.outboundcode
            new TranslationSeedItem("entity.serialoutbound.outboundcode", "zh-HK", "出库单号_hk", "出库单号（租户+公司+工厂内唯一）"),

            // entity.serialoutbound.shippinginvoicecode
            new TranslationSeedItem("entity.serialoutbound.shippinginvoicecode", "en-US", "发货单号_us", "发货单号"),
            // entity.serialoutbound.shippinginvoicecode
            new TranslationSeedItem("entity.serialoutbound.shippinginvoicecode", "ja-JP", "发货单号_jp", "发货单号"),
            // entity.serialoutbound.shippinginvoicecode
            new TranslationSeedItem("entity.serialoutbound.shippinginvoicecode", "zh-CN", "发货单号", "发货单号"),
            // entity.serialoutbound.shippinginvoicecode
            new TranslationSeedItem("entity.serialoutbound.shippinginvoicecode", "zh-HK", "发货单号_hk", "发货单号"),

            // entity.serialoutbound.outbounddate
            new TranslationSeedItem("entity.serialoutbound.outbounddate", "en-US", "装车日期_us", "装车日期"),
            // entity.serialoutbound.outbounddate
            new TranslationSeedItem("entity.serialoutbound.outbounddate", "ja-JP", "装车日期_jp", "装车日期"),
            // entity.serialoutbound.outbounddate
            new TranslationSeedItem("entity.serialoutbound.outbounddate", "zh-CN", "装车日期", "装车日期"),
            // entity.serialoutbound.outbounddate
            new TranslationSeedItem("entity.serialoutbound.outbounddate", "zh-HK", "装车日期_hk", "装车日期"),

            // entity.serialoutbound.destination
            new TranslationSeedItem("entity.serialoutbound.destination", "en-US", "仕向地_us", "仕向地（选项 TaktModelDestinations/options；DictValue=DestinationCode）"),
            // entity.serialoutbound.destination
            new TranslationSeedItem("entity.serialoutbound.destination", "ja-JP", "仕向地_jp", "仕向地（选项 TaktModelDestinations/options；DictValue=DestinationCode）"),
            // entity.serialoutbound.destination
            new TranslationSeedItem("entity.serialoutbound.destination", "zh-CN", "仕向地", "仕向地（选项 TaktModelDestinations/options；DictValue=DestinationCode）"),
            // entity.serialoutbound.destination
            new TranslationSeedItem("entity.serialoutbound.destination", "zh-HK", "仕向地_hk", "仕向地（选项 TaktModelDestinations/options；DictValue=DestinationCode）"),

            // entity.serialoutbound.destinationport
            new TranslationSeedItem("entity.serialoutbound.destinationport", "en-US", "目的地港_us", "目的地港（字典 logistics_serial_destination_port_code；DictValue 为港口/运输编码，如 ACE_AIR、VIE）"),
            // entity.serialoutbound.destinationport
            new TranslationSeedItem("entity.serialoutbound.destinationport", "ja-JP", "目的地港_jp", "目的地港（字典 logistics_serial_destination_port_code；DictValue 为港口/运输编码，如 ACE_AIR、VIE）"),
            // entity.serialoutbound.destinationport
            new TranslationSeedItem("entity.serialoutbound.destinationport", "zh-CN", "目的地港", "目的地港（字典 logistics_serial_destination_port_code；DictValue 为港口/运输编码，如 ACE_AIR、VIE）"),
            // entity.serialoutbound.destinationport
            new TranslationSeedItem("entity.serialoutbound.destinationport", "zh-HK", "目的地港_hk", "目的地港（字典 logistics_serial_destination_port_code；DictValue 为港口/运输编码，如 ACE_AIR、VIE）"),

            // entity.serialoutbound.outboundtype
            new TranslationSeedItem("entity.serialoutbound.outboundtype", "en-US", "出库类型_us", "出库类型（字典 logistics_materials_outbound_type；0=销售出库 1=生产领料 2=退货出库 3=调拨出库 4=报废出库 5=序列号出库 6=其他）"),
            // entity.serialoutbound.outboundtype
            new TranslationSeedItem("entity.serialoutbound.outboundtype", "ja-JP", "出库类型_jp", "出库类型（字典 logistics_materials_outbound_type；0=销售出库 1=生产领料 2=退货出库 3=调拨出库 4=报废出库 5=序列号出库 6=其他）"),
            // entity.serialoutbound.outboundtype
            new TranslationSeedItem("entity.serialoutbound.outboundtype", "zh-CN", "出库类型", "出库类型（字典 logistics_materials_outbound_type；0=销售出库 1=生产领料 2=退货出库 3=调拨出库 4=报废出库 5=序列号出库 6=其他）"),
            // entity.serialoutbound.outboundtype
            new TranslationSeedItem("entity.serialoutbound.outboundtype", "zh-HK", "出库类型_hk", "出库类型（字典 logistics_materials_outbound_type；0=销售出库 1=生产领料 2=退货出库 3=调拨出库 4=报废出库 5=序列号出库 6=其他）"),

            // entity.serialoutbound.warehousecode
            new TranslationSeedItem("entity.serialoutbound.warehousecode", "en-US", "仓库编码_us", "仓库编码（选项 TaktWarehouses/options；DictValue=Id）"),
            // entity.serialoutbound.warehousecode
            new TranslationSeedItem("entity.serialoutbound.warehousecode", "ja-JP", "仓库编码_jp", "仓库编码（选项 TaktWarehouses/options；DictValue=Id）"),
            // entity.serialoutbound.warehousecode
            new TranslationSeedItem("entity.serialoutbound.warehousecode", "zh-CN", "仓库编码", "仓库编码（选项 TaktWarehouses/options；DictValue=Id）"),
            // entity.serialoutbound.warehousecode
            new TranslationSeedItem("entity.serialoutbound.warehousecode", "zh-HK", "仓库编码_hk", "仓库编码（选项 TaktWarehouses/options；DictValue=Id）"),

            // entity.serialoutbound.locationcode
            new TranslationSeedItem("entity.serialoutbound.locationcode", "en-US", "库位编码_us", "库位编码（选项 TaktStorageLocations/options；DictValue=Id）"),
            // entity.serialoutbound.locationcode
            new TranslationSeedItem("entity.serialoutbound.locationcode", "ja-JP", "库位编码_jp", "库位编码（选项 TaktStorageLocations/options；DictValue=Id）"),
            // entity.serialoutbound.locationcode
            new TranslationSeedItem("entity.serialoutbound.locationcode", "zh-CN", "库位编码", "库位编码（选项 TaktStorageLocations/options；DictValue=Id）"),
            // entity.serialoutbound.locationcode
            new TranslationSeedItem("entity.serialoutbound.locationcode", "zh-HK", "库位编码_hk", "库位编码（选项 TaktStorageLocations/options；DictValue=Id）"),

            // entity.serialoutbound.totalquantity
            new TranslationSeedItem("entity.serialoutbound.totalquantity", "en-US", "总数量_us", "总数量"),
            // entity.serialoutbound.totalquantity
            new TranslationSeedItem("entity.serialoutbound.totalquantity", "ja-JP", "总数量_jp", "总数量"),
            // entity.serialoutbound.totalquantity
            new TranslationSeedItem("entity.serialoutbound.totalquantity", "zh-CN", "总数量", "总数量"),
            // entity.serialoutbound.totalquantity
            new TranslationSeedItem("entity.serialoutbound.totalquantity", "zh-HK", "总数量_hk", "总数量"),

            // entity.serialoutbound.items
            new TranslationSeedItem("entity.serialoutbound.items", "en-US", "序列号出库明细列表_us", "序列号出库明细列表（主子表关系）"),
            // entity.serialoutbound.items
            new TranslationSeedItem("entity.serialoutbound.items", "ja-JP", "序列号出库明细列表_jp", "序列号出库明细列表（主子表关系）"),
            // entity.serialoutbound.items
            new TranslationSeedItem("entity.serialoutbound.items", "zh-CN", "序列号出库明细列表", "序列号出库明细列表（主子表关系）"),
            // entity.serialoutbound.items
            new TranslationSeedItem("entity.serialoutbound.items", "zh-HK", "序列号出库明细列表_hk", "序列号出库明细列表（主子表关系）"),
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
