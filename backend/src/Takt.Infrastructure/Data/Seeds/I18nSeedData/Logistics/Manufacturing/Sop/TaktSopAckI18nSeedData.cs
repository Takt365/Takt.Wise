// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Sop
// 文件名称：TaktSopAckI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSopAck 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Sop;

/// <summary>
/// TaktSopAck 实体国际化翻译种子（键前缀 entity.sopack.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSopAckI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSopAck 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 sopack 实体翻译...", tenantCode);

        foreach (var item in GetSopAckTranslations())
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

        TaktLogger.Information("TaktSopAck 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSopAck 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.sopack._self / entity.sopack.{{field}}；ResourceGroup=Sop；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSopAckTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.sopack._self
            new TranslationSeedItem("entity.sopack._self", "en-US", "Sop Ack Information_us", "实体名称"),
            // entity.sopack._self
            new TranslationSeedItem("entity.sopack._self", "ja-JP", "SOP 确认信息_jp", "实体名称"),
            // entity.sopack._self
            new TranslationSeedItem("entity.sopack._self", "zh-CN", "SOP 确认信息", "实体名称"),
            // entity.sopack._self
            new TranslationSeedItem("entity.sopack._self", "zh-HK", "SOP 确认信息_hk", "实体名称"),

            // entity.sopack.sopid
            new TranslationSeedItem("entity.sopack.sopid", "en-US", "SOP主档ID_us", "SOP 主档 ID（选项 TaktSopDocs/options；DictValue=Id）"),
            // entity.sopack.sopid
            new TranslationSeedItem("entity.sopack.sopid", "ja-JP", "SOP主档ID_jp", "SOP 主档 ID（选项 TaktSopDocs/options；DictValue=Id）"),
            // entity.sopack.sopid
            new TranslationSeedItem("entity.sopack.sopid", "zh-CN", "SOP主档ID", "SOP 主档 ID（选项 TaktSopDocs/options；DictValue=Id）"),
            // entity.sopack.sopid
            new TranslationSeedItem("entity.sopack.sopid", "zh-HK", "SOP主档ID_hk", "SOP 主档 ID（选项 TaktSopDocs/options；DictValue=Id）"),

            // entity.sopack.revisionid
            new TranslationSeedItem("entity.sopack.revisionid", "en-US", "SOP版本ID_us", "SOP 版本 ID（选项 TaktSopRevisions/options；DictValue=Id）"),
            // entity.sopack.revisionid
            new TranslationSeedItem("entity.sopack.revisionid", "ja-JP", "SOP版本ID_jp", "SOP 版本 ID（选项 TaktSopRevisions/options；DictValue=Id）"),
            // entity.sopack.revisionid
            new TranslationSeedItem("entity.sopack.revisionid", "zh-CN", "SOP版本ID", "SOP 版本 ID（选项 TaktSopRevisions/options；DictValue=Id）"),
            // entity.sopack.revisionid
            new TranslationSeedItem("entity.sopack.revisionid", "zh-HK", "SOP版本ID_hk", "SOP 版本 ID（选项 TaktSopRevisions/options；DictValue=Id）"),

            // entity.sopack.workstationid
            new TranslationSeedItem("entity.sopack.workstationid", "en-US", "工位ID_us", "工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）"),
            // entity.sopack.workstationid
            new TranslationSeedItem("entity.sopack.workstationid", "ja-JP", "工位ID_jp", "工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）"),
            // entity.sopack.workstationid
            new TranslationSeedItem("entity.sopack.workstationid", "zh-CN", "工位ID", "工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）"),
            // entity.sopack.workstationid
            new TranslationSeedItem("entity.sopack.workstationid", "zh-HK", "工位ID_hk", "工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）"),

            // entity.sopack.acknowledgedby
            new TranslationSeedItem("entity.sopack.acknowledgedby", "en-US", "确认人ID_us", "确认人 ID（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.sopack.acknowledgedby
            new TranslationSeedItem("entity.sopack.acknowledgedby", "ja-JP", "确认人ID_jp", "确认人 ID（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.sopack.acknowledgedby
            new TranslationSeedItem("entity.sopack.acknowledgedby", "zh-CN", "确认人ID", "确认人 ID（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.sopack.acknowledgedby
            new TranslationSeedItem("entity.sopack.acknowledgedby", "zh-HK", "确认人ID_hk", "确认人 ID（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.sopack.acknowledgedbyname
            new TranslationSeedItem("entity.sopack.acknowledgedbyname", "en-US", "确认人名称_us", "确认人名称（冗余：按 AcknowledgedBy 取 TaktEmployee.EmployeeName 联动）"),
            // entity.sopack.acknowledgedbyname
            new TranslationSeedItem("entity.sopack.acknowledgedbyname", "ja-JP", "确认人名称_jp", "确认人名称（冗余：按 AcknowledgedBy 取 TaktEmployee.EmployeeName 联动）"),
            // entity.sopack.acknowledgedbyname
            new TranslationSeedItem("entity.sopack.acknowledgedbyname", "zh-CN", "确认人名称", "确认人名称（冗余：按 AcknowledgedBy 取 TaktEmployee.EmployeeName 联动）"),
            // entity.sopack.acknowledgedbyname
            new TranslationSeedItem("entity.sopack.acknowledgedbyname", "zh-HK", "确认人名称_hk", "确认人名称（冗余：按 AcknowledgedBy 取 TaktEmployee.EmployeeName 联动）"),

            // entity.sopack.acknowledgedat
            new TranslationSeedItem("entity.sopack.acknowledgedat", "en-US", "确认时间_us", "确认时间"),
            // entity.sopack.acknowledgedat
            new TranslationSeedItem("entity.sopack.acknowledgedat", "ja-JP", "确认时间_jp", "确认时间"),
            // entity.sopack.acknowledgedat
            new TranslationSeedItem("entity.sopack.acknowledgedat", "zh-CN", "确认时间", "确认时间"),
            // entity.sopack.acknowledgedat
            new TranslationSeedItem("entity.sopack.acknowledgedat", "zh-HK", "确认时间_hk", "确认时间"),

            // entity.sopack.ackcomment
            new TranslationSeedItem("entity.sopack.ackcomment", "en-US", "确认意见_us", "确认意见"),
            // entity.sopack.ackcomment
            new TranslationSeedItem("entity.sopack.ackcomment", "ja-JP", "确认意见_jp", "确认意见"),
            // entity.sopack.ackcomment
            new TranslationSeedItem("entity.sopack.ackcomment", "zh-CN", "确认意见", "确认意见"),
            // entity.sopack.ackcomment
            new TranslationSeedItem("entity.sopack.ackcomment", "zh-HK", "确认意见_hk", "确认意见"),

            // entity.sopack.sopdoc
            new TranslationSeedItem("entity.sopack.sopdoc", "en-US", "SOP 主档_us", "SOP 主档"),
            // entity.sopack.sopdoc
            new TranslationSeedItem("entity.sopack.sopdoc", "ja-JP", "SOP 主档_jp", "SOP 主档"),
            // entity.sopack.sopdoc
            new TranslationSeedItem("entity.sopack.sopdoc", "zh-CN", "SOP 主档", "SOP 主档"),
            // entity.sopack.sopdoc
            new TranslationSeedItem("entity.sopack.sopdoc", "zh-HK", "SOP 主档_hk", "SOP 主档"),

            // entity.sopack.revision
            new TranslationSeedItem("entity.sopack.revision", "en-US", "SOP 版本_us", "SOP 版本"),
            // entity.sopack.revision
            new TranslationSeedItem("entity.sopack.revision", "ja-JP", "SOP 版本_jp", "SOP 版本"),
            // entity.sopack.revision
            new TranslationSeedItem("entity.sopack.revision", "zh-CN", "SOP 版本", "SOP 版本"),
            // entity.sopack.revision
            new TranslationSeedItem("entity.sopack.revision", "zh-HK", "SOP 版本_hk", "SOP 版本"),

            // entity.sopack.workstation
            new TranslationSeedItem("entity.sopack.workstation", "en-US", "工位_us", "工位"),
            // entity.sopack.workstation
            new TranslationSeedItem("entity.sopack.workstation", "ja-JP", "工位_jp", "工位"),
            // entity.sopack.workstation
            new TranslationSeedItem("entity.sopack.workstation", "zh-CN", "工位", "工位"),
            // entity.sopack.workstation
            new TranslationSeedItem("entity.sopack.workstation", "zh-HK", "工位_hk", "工位"),
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
        translation.ResourceGroup = "Sop";
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
