// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Serial
// 文件名称：TaktSerialInboundI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSerialInbound 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktSerialInbound 实体国际化翻译种子（键前缀 entity.serialinbound.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSerialInboundI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSerialInbound 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 serialinbound 实体翻译...", tenantCode);

        foreach (var item in GetSerialInboundTranslations())
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

        TaktLogger.Information("TaktSerialInbound 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSerialInbound 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.serialinbound._self / entity.serialinbound.{{field}}；ResourceGroup=Serial；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSerialInboundTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.serialinbound._self
            new TranslationSeedItem("entity.serialinbound._self", "en-US", "Serial Inbound Information_us", "实体名称"),
            // entity.serialinbound._self
            new TranslationSeedItem("entity.serialinbound._self", "ja-JP", "序列号入库主表信息_jp", "实体名称"),
            // entity.serialinbound._self
            new TranslationSeedItem("entity.serialinbound._self", "zh-CN", "序列号入库主表信息", "实体名称"),
            // entity.serialinbound._self
            new TranslationSeedItem("entity.serialinbound._self", "zh-HK", "序列号入库主表信息_hk", "实体名称"),

            // entity.serialinbound.inboundcode
            new TranslationSeedItem("entity.serialinbound.inboundcode", "en-US", "入库单号_us", "入库单号（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 入库单号规则生成并展示，非手输；单据类型菜单：序列号入库）"),
            // entity.serialinbound.inboundcode
            new TranslationSeedItem("entity.serialinbound.inboundcode", "ja-JP", "入库单号_jp", "入库单号（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 入库单号规则生成并展示，非手输；单据类型菜单：序列号入库）"),
            // entity.serialinbound.inboundcode
            new TranslationSeedItem("entity.serialinbound.inboundcode", "zh-CN", "入库单号", "入库单号（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 入库单号规则生成并展示，非手输；单据类型菜单：序列号入库）"),
            // entity.serialinbound.inboundcode
            new TranslationSeedItem("entity.serialinbound.inboundcode", "zh-HK", "入库单号_hk", "入库单号（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 入库单号规则生成并展示，非手输；单据类型菜单：序列号入库）"),

            // entity.serialinbound.inbounddate
            new TranslationSeedItem("entity.serialinbound.inbounddate", "en-US", "入库日期_us", "入库日期"),
            // entity.serialinbound.inbounddate
            new TranslationSeedItem("entity.serialinbound.inbounddate", "ja-JP", "入库日期_jp", "入库日期"),
            // entity.serialinbound.inbounddate
            new TranslationSeedItem("entity.serialinbound.inbounddate", "zh-CN", "入库日期", "入库日期"),
            // entity.serialinbound.inbounddate
            new TranslationSeedItem("entity.serialinbound.inbounddate", "zh-HK", "入库日期_hk", "入库日期"),

            // entity.serialinbound.inboundtype
            new TranslationSeedItem("entity.serialinbound.inboundtype", "en-US", "入库类型_us", "入库类型（字典 logistics_materials_inbound_type；0=采购入库 1=生产入库 2=退货入库 3=调拨入库 4=序列号入库 5=其他）"),
            // entity.serialinbound.inboundtype
            new TranslationSeedItem("entity.serialinbound.inboundtype", "ja-JP", "入库类型_jp", "入库类型（字典 logistics_materials_inbound_type；0=采购入库 1=生产入库 2=退货入库 3=调拨入库 4=序列号入库 5=其他）"),
            // entity.serialinbound.inboundtype
            new TranslationSeedItem("entity.serialinbound.inboundtype", "zh-CN", "入库类型", "入库类型（字典 logistics_materials_inbound_type；0=采购入库 1=生产入库 2=退货入库 3=调拨入库 4=序列号入库 5=其他）"),
            // entity.serialinbound.inboundtype
            new TranslationSeedItem("entity.serialinbound.inboundtype", "zh-HK", "入库类型_hk", "入库类型（字典 logistics_materials_inbound_type；0=采购入库 1=生产入库 2=退货入库 3=调拨入库 4=序列号入库 5=其他）"),

            // entity.serialinbound.warehousecode
            new TranslationSeedItem("entity.serialinbound.warehousecode", "en-US", "仓库编码_us", "仓库编码（选项 TaktWarehouses/options；DictValue=Id）"),
            // entity.serialinbound.warehousecode
            new TranslationSeedItem("entity.serialinbound.warehousecode", "ja-JP", "仓库编码_jp", "仓库编码（选项 TaktWarehouses/options；DictValue=Id）"),
            // entity.serialinbound.warehousecode
            new TranslationSeedItem("entity.serialinbound.warehousecode", "zh-CN", "仓库编码", "仓库编码（选项 TaktWarehouses/options；DictValue=Id）"),
            // entity.serialinbound.warehousecode
            new TranslationSeedItem("entity.serialinbound.warehousecode", "zh-HK", "仓库编码_hk", "仓库编码（选项 TaktWarehouses/options；DictValue=Id）"),

            // entity.serialinbound.locationcode
            new TranslationSeedItem("entity.serialinbound.locationcode", "en-US", "库位编码_us", "库位编码（选项 TaktStorageLocations/options；DictValue=Id）"),
            // entity.serialinbound.locationcode
            new TranslationSeedItem("entity.serialinbound.locationcode", "ja-JP", "库位编码_jp", "库位编码（选项 TaktStorageLocations/options；DictValue=Id）"),
            // entity.serialinbound.locationcode
            new TranslationSeedItem("entity.serialinbound.locationcode", "zh-CN", "库位编码", "库位编码（选项 TaktStorageLocations/options；DictValue=Id）"),
            // entity.serialinbound.locationcode
            new TranslationSeedItem("entity.serialinbound.locationcode", "zh-HK", "库位编码_hk", "库位编码（选项 TaktStorageLocations/options；DictValue=Id）"),

            // entity.serialinbound.totalquantity
            new TranslationSeedItem("entity.serialinbound.totalquantity", "en-US", "总数量_us", "总数量"),
            // entity.serialinbound.totalquantity
            new TranslationSeedItem("entity.serialinbound.totalquantity", "ja-JP", "总数量_jp", "总数量"),
            // entity.serialinbound.totalquantity
            new TranslationSeedItem("entity.serialinbound.totalquantity", "zh-CN", "总数量", "总数量"),
            // entity.serialinbound.totalquantity
            new TranslationSeedItem("entity.serialinbound.totalquantity", "zh-HK", "总数量_hk", "总数量"),

            // entity.serialinbound.items
            new TranslationSeedItem("entity.serialinbound.items", "en-US", "序列号入库明细列表_us", "序列号入库明细列表（主子表关系）"),
            // entity.serialinbound.items
            new TranslationSeedItem("entity.serialinbound.items", "ja-JP", "序列号入库明细列表_jp", "序列号入库明细列表（主子表关系）"),
            // entity.serialinbound.items
            new TranslationSeedItem("entity.serialinbound.items", "zh-CN", "序列号入库明细列表", "序列号入库明细列表（主子表关系）"),
            // entity.serialinbound.items
            new TranslationSeedItem("entity.serialinbound.items", "zh-HK", "序列号入库明细列表_hk", "序列号入库明细列表（主子表关系）"),
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
