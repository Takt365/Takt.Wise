// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Serial
// 文件名称：TaktSerialInboundItemI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSerialInboundItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktSerialInboundItem 实体国际化翻译种子（键前缀 entity.serialinbounditem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSerialInboundItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSerialInboundItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 serialinbounditem 实体翻译...", tenantCode);

        foreach (var item in GetSerialInboundItemTranslations())
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

        TaktLogger.Information("TaktSerialInboundItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSerialInboundItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.serialinbounditem._self / entity.serialinbounditem.{{field}}；ResourceGroup=Serial；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSerialInboundItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.serialinbounditem._self
            new TranslationSeedItem("entity.serialinbounditem._self", "en-US", "Serial Inbound Item Information_us", "实体名称"),
            // entity.serialinbounditem._self
            new TranslationSeedItem("entity.serialinbounditem._self", "ja-JP", "序列号入库明细信息_jp", "实体名称"),
            // entity.serialinbounditem._self
            new TranslationSeedItem("entity.serialinbounditem._self", "zh-CN", "序列号入库明细信息", "实体名称"),
            // entity.serialinbounditem._self
            new TranslationSeedItem("entity.serialinbounditem._self", "zh-HK", "序列号入库明细信息_hk", "实体名称"),

            // entity.serialinbounditem.inboundid
            new TranslationSeedItem("entity.serialinbounditem.inboundid", "en-US", "入库ID_us", "入库主表 ID（选项 TaktSerialInbounds/options；DictValue=Id）"),
            // entity.serialinbounditem.inboundid
            new TranslationSeedItem("entity.serialinbounditem.inboundid", "ja-JP", "入库ID_jp", "入库主表 ID（选项 TaktSerialInbounds/options；DictValue=Id）"),
            // entity.serialinbounditem.inboundid
            new TranslationSeedItem("entity.serialinbounditem.inboundid", "zh-CN", "入库ID", "入库主表 ID（选项 TaktSerialInbounds/options；DictValue=Id）"),
            // entity.serialinbounditem.inboundid
            new TranslationSeedItem("entity.serialinbounditem.inboundid", "zh-HK", "入库ID_hk", "入库主表 ID（选项 TaktSerialInbounds/options；DictValue=Id）"),

            // entity.serialinbounditem.inboundcode
            new TranslationSeedItem("entity.serialinbounditem.inboundcode", "en-US", "入库单号_us", "入库单号（冗余：按对应 Id 取主数据名称联动）"),
            // entity.serialinbounditem.inboundcode
            new TranslationSeedItem("entity.serialinbounditem.inboundcode", "ja-JP", "入库单号_jp", "入库单号（冗余：按对应 Id 取主数据名称联动）"),
            // entity.serialinbounditem.inboundcode
            new TranslationSeedItem("entity.serialinbounditem.inboundcode", "zh-CN", "入库单号", "入库单号（冗余：按对应 Id 取主数据名称联动）"),
            // entity.serialinbounditem.inboundcode
            new TranslationSeedItem("entity.serialinbounditem.inboundcode", "zh-HK", "入库单号_hk", "入库单号（冗余：按对应 Id 取主数据名称联动）"),

            // entity.serialinbounditem.linenumber
            new TranslationSeedItem("entity.serialinbounditem.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.serialinbounditem.linenumber
            new TranslationSeedItem("entity.serialinbounditem.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.serialinbounditem.linenumber
            new TranslationSeedItem("entity.serialinbounditem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.serialinbounditem.linenumber
            new TranslationSeedItem("entity.serialinbounditem.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.serialinbounditem.inboundserialcode
            new TranslationSeedItem("entity.serialinbounditem.inboundserialcode", "en-US", "入库序列号_us", "入库序列号（租户+公司内唯一）"),
            // entity.serialinbounditem.inboundserialcode
            new TranslationSeedItem("entity.serialinbounditem.inboundserialcode", "ja-JP", "入库序列号_jp", "入库序列号（租户+公司内唯一）"),
            // entity.serialinbounditem.inboundserialcode
            new TranslationSeedItem("entity.serialinbounditem.inboundserialcode", "zh-CN", "入库序列号", "入库序列号（租户+公司内唯一）"),
            // entity.serialinbounditem.inboundserialcode
            new TranslationSeedItem("entity.serialinbounditem.inboundserialcode", "zh-HK", "入库序列号_hk", "入库序列号（租户+公司内唯一）"),

            // entity.serialinbounditem.isobsolete
            new TranslationSeedItem("entity.serialinbounditem.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.serialinbounditem.isobsolete
            new TranslationSeedItem("entity.serialinbounditem.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.serialinbounditem.isobsolete
            new TranslationSeedItem("entity.serialinbounditem.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.serialinbounditem.isobsolete
            new TranslationSeedItem("entity.serialinbounditem.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.serialinbounditem.inbound
            new TranslationSeedItem("entity.serialinbounditem.inbound", "en-US", "入库主表_us", "入库主表"),
            // entity.serialinbounditem.inbound
            new TranslationSeedItem("entity.serialinbounditem.inbound", "ja-JP", "入库主表_jp", "入库主表"),
            // entity.serialinbounditem.inbound
            new TranslationSeedItem("entity.serialinbounditem.inbound", "zh-CN", "入库主表", "入库主表"),
            // entity.serialinbounditem.inbound
            new TranslationSeedItem("entity.serialinbounditem.inbound", "zh-HK", "入库主表_hk", "入库主表"),
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
