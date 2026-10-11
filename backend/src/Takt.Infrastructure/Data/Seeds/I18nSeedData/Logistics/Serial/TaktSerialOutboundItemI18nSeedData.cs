// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Serial
// 文件名称：TaktSerialOutboundItemI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSerialOutboundItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktSerialOutboundItem 实体国际化翻译种子（键前缀 entity.serialoutbounditem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSerialOutboundItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSerialOutboundItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 serialoutbounditem 实体翻译...", tenantCode);

        foreach (var item in GetSerialOutboundItemTranslations())
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

        TaktLogger.Information("TaktSerialOutboundItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSerialOutboundItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.serialoutbounditem._self / entity.serialoutbounditem.{{field}}；ResourceGroup=Serial；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSerialOutboundItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.serialoutbounditem._self
            new TranslationSeedItem("entity.serialoutbounditem._self", "en-US", "Serial Outbound Item Information_us", "实体名称"),
            // entity.serialoutbounditem._self
            new TranslationSeedItem("entity.serialoutbounditem._self", "ja-JP", "序列号出库明细信息_jp", "实体名称"),
            // entity.serialoutbounditem._self
            new TranslationSeedItem("entity.serialoutbounditem._self", "zh-CN", "序列号出库明细信息", "实体名称"),
            // entity.serialoutbounditem._self
            new TranslationSeedItem("entity.serialoutbounditem._self", "zh-HK", "序列号出库明细信息_hk", "实体名称"),

            // entity.serialoutbounditem.outboundid
            new TranslationSeedItem("entity.serialoutbounditem.outboundid", "en-US", "出库ID_us", "出库主表 ID（选项 TaktSerialOutbounds/options；DictValue=Id）"),
            // entity.serialoutbounditem.outboundid
            new TranslationSeedItem("entity.serialoutbounditem.outboundid", "ja-JP", "出库ID_jp", "出库主表 ID（选项 TaktSerialOutbounds/options；DictValue=Id）"),
            // entity.serialoutbounditem.outboundid
            new TranslationSeedItem("entity.serialoutbounditem.outboundid", "zh-CN", "出库ID", "出库主表 ID（选项 TaktSerialOutbounds/options；DictValue=Id）"),
            // entity.serialoutbounditem.outboundid
            new TranslationSeedItem("entity.serialoutbounditem.outboundid", "zh-HK", "出库ID_hk", "出库主表 ID（选项 TaktSerialOutbounds/options；DictValue=Id）"),

            // entity.serialoutbounditem.outboundcode
            new TranslationSeedItem("entity.serialoutbounditem.outboundcode", "en-US", "出库单号_us", "出库单号（冗余：按对应 Id 取主数据名称联动）"),
            // entity.serialoutbounditem.outboundcode
            new TranslationSeedItem("entity.serialoutbounditem.outboundcode", "ja-JP", "出库单号_jp", "出库单号（冗余：按对应 Id 取主数据名称联动）"),
            // entity.serialoutbounditem.outboundcode
            new TranslationSeedItem("entity.serialoutbounditem.outboundcode", "zh-CN", "出库单号", "出库单号（冗余：按对应 Id 取主数据名称联动）"),
            // entity.serialoutbounditem.outboundcode
            new TranslationSeedItem("entity.serialoutbounditem.outboundcode", "zh-HK", "出库单号_hk", "出库单号（冗余：按对应 Id 取主数据名称联动）"),

            // entity.serialoutbounditem.linenumber
            new TranslationSeedItem("entity.serialoutbounditem.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.serialoutbounditem.linenumber
            new TranslationSeedItem("entity.serialoutbounditem.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.serialoutbounditem.linenumber
            new TranslationSeedItem("entity.serialoutbounditem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.serialoutbounditem.linenumber
            new TranslationSeedItem("entity.serialoutbounditem.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.serialoutbounditem.outboundserialcode
            new TranslationSeedItem("entity.serialoutbounditem.outboundserialcode", "en-US", "出库序列号_us", "出库序列号（租户+公司内唯一）"),
            // entity.serialoutbounditem.outboundserialcode
            new TranslationSeedItem("entity.serialoutbounditem.outboundserialcode", "ja-JP", "出库序列号_jp", "出库序列号（租户+公司内唯一）"),
            // entity.serialoutbounditem.outboundserialcode
            new TranslationSeedItem("entity.serialoutbounditem.outboundserialcode", "zh-CN", "出库序列号", "出库序列号（租户+公司内唯一）"),
            // entity.serialoutbounditem.outboundserialcode
            new TranslationSeedItem("entity.serialoutbounditem.outboundserialcode", "zh-HK", "出库序列号_hk", "出库序列号（租户+公司内唯一）"),

            // entity.serialoutbounditem.referenceinboundid
            new TranslationSeedItem("entity.serialoutbounditem.referenceinboundid", "en-US", "关联入库ID_us", "关联入库主表 ID（选项 TaktSerialInbounds/options；DictValue=Id）"),
            // entity.serialoutbounditem.referenceinboundid
            new TranslationSeedItem("entity.serialoutbounditem.referenceinboundid", "ja-JP", "关联入库ID_jp", "关联入库主表 ID（选项 TaktSerialInbounds/options；DictValue=Id）"),
            // entity.serialoutbounditem.referenceinboundid
            new TranslationSeedItem("entity.serialoutbounditem.referenceinboundid", "zh-CN", "关联入库ID", "关联入库主表 ID（选项 TaktSerialInbounds/options；DictValue=Id）"),
            // entity.serialoutbounditem.referenceinboundid
            new TranslationSeedItem("entity.serialoutbounditem.referenceinboundid", "zh-HK", "关联入库ID_hk", "关联入库主表 ID（选项 TaktSerialInbounds/options；DictValue=Id）"),

            // entity.serialoutbounditem.referenceinboundcode
            new TranslationSeedItem("entity.serialoutbounditem.referenceinboundcode", "en-US", "关联入库单号_us", "关联入库单号（选项 TaktSerialInbounds/options；DictValue=InboundCode）"),
            // entity.serialoutbounditem.referenceinboundcode
            new TranslationSeedItem("entity.serialoutbounditem.referenceinboundcode", "ja-JP", "关联入库单号_jp", "关联入库单号（选项 TaktSerialInbounds/options；DictValue=InboundCode）"),
            // entity.serialoutbounditem.referenceinboundcode
            new TranslationSeedItem("entity.serialoutbounditem.referenceinboundcode", "zh-CN", "关联入库单号", "关联入库单号（选项 TaktSerialInbounds/options；DictValue=InboundCode）"),
            // entity.serialoutbounditem.referenceinboundcode
            new TranslationSeedItem("entity.serialoutbounditem.referenceinboundcode", "zh-HK", "关联入库单号_hk", "关联入库单号（选项 TaktSerialInbounds/options；DictValue=InboundCode）"),

            // entity.serialoutbounditem.referenceinboundlinenumber
            new TranslationSeedItem("entity.serialoutbounditem.referenceinboundlinenumber", "en-US", "关联入库行号_us", "关联入库行号（对应 TaktSerialInboundItem.LineNumber）"),
            // entity.serialoutbounditem.referenceinboundlinenumber
            new TranslationSeedItem("entity.serialoutbounditem.referenceinboundlinenumber", "ja-JP", "关联入库行号_jp", "关联入库行号（对应 TaktSerialInboundItem.LineNumber）"),
            // entity.serialoutbounditem.referenceinboundlinenumber
            new TranslationSeedItem("entity.serialoutbounditem.referenceinboundlinenumber", "zh-CN", "关联入库行号", "关联入库行号（对应 TaktSerialInboundItem.LineNumber）"),
            // entity.serialoutbounditem.referenceinboundlinenumber
            new TranslationSeedItem("entity.serialoutbounditem.referenceinboundlinenumber", "zh-HK", "关联入库行号_hk", "关联入库行号（对应 TaktSerialInboundItem.LineNumber）"),

            // entity.serialoutbounditem.isobsolete
            new TranslationSeedItem("entity.serialoutbounditem.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.serialoutbounditem.isobsolete
            new TranslationSeedItem("entity.serialoutbounditem.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.serialoutbounditem.isobsolete
            new TranslationSeedItem("entity.serialoutbounditem.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.serialoutbounditem.isobsolete
            new TranslationSeedItem("entity.serialoutbounditem.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.serialoutbounditem.outbound
            new TranslationSeedItem("entity.serialoutbounditem.outbound", "en-US", "出库主表_us", "出库主表"),
            // entity.serialoutbounditem.outbound
            new TranslationSeedItem("entity.serialoutbounditem.outbound", "ja-JP", "出库主表_jp", "出库主表"),
            // entity.serialoutbounditem.outbound
            new TranslationSeedItem("entity.serialoutbounditem.outbound", "zh-CN", "出库主表", "出库主表"),
            // entity.serialoutbounditem.outbound
            new TranslationSeedItem("entity.serialoutbounditem.outbound", "zh-HK", "出库主表_hk", "出库主表"),
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
