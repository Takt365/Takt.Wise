// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Sop
// 文件名称：TaktSopDocI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSopDoc 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktSopDoc 实体国际化翻译种子（键前缀 entity.sopdoc.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSopDocI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSopDoc 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 sopdoc 实体翻译...", tenantCode);

        foreach (var item in GetSopDocTranslations())
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

        TaktLogger.Information("TaktSopDoc 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSopDoc 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.sopdoc._self / entity.sopdoc.{{field}}；ResourceGroup=Sop；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSopDocTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.sopdoc._self
            new TranslationSeedItem("entity.sopdoc._self", "en-US", "Sop Doc Information_us", "实体名称"),
            // entity.sopdoc._self
            new TranslationSeedItem("entity.sopdoc._self", "ja-JP", "SOP 文档头信息_jp", "实体名称"),
            // entity.sopdoc._self
            new TranslationSeedItem("entity.sopdoc._self", "zh-CN", "SOP 文档头信息", "实体名称"),
            // entity.sopdoc._self
            new TranslationSeedItem("entity.sopdoc._self", "zh-HK", "SOP 文档头信息_hk", "实体名称"),

            // entity.sopdoc.sopcode
            new TranslationSeedItem("entity.sopdoc.sopcode", "en-US", "SOP编码_us", "SOP 编码"),
            // entity.sopdoc.sopcode
            new TranslationSeedItem("entity.sopdoc.sopcode", "ja-JP", "SOP编码_jp", "SOP 编码"),
            // entity.sopdoc.sopcode
            new TranslationSeedItem("entity.sopdoc.sopcode", "zh-CN", "SOP编码", "SOP 编码"),
            // entity.sopdoc.sopcode
            new TranslationSeedItem("entity.sopdoc.sopcode", "zh-HK", "SOP编码_hk", "SOP 编码"),

            // entity.sopdoc.sopname
            new TranslationSeedItem("entity.sopdoc.sopname", "en-US", "SOP名称_us", "SOP 名称"),
            // entity.sopdoc.sopname
            new TranslationSeedItem("entity.sopdoc.sopname", "ja-JP", "SOP名称_jp", "SOP 名称"),
            // entity.sopdoc.sopname
            new TranslationSeedItem("entity.sopdoc.sopname", "zh-CN", "SOP名称", "SOP 名称"),
            // entity.sopdoc.sopname
            new TranslationSeedItem("entity.sopdoc.sopname", "zh-HK", "SOP名称_hk", "SOP 名称"),

            // entity.sopdoc.materialcode
            new TranslationSeedItem("entity.sopdoc.materialcode", "en-US", "物料编码_us", "产品/物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.sopdoc.materialcode
            new TranslationSeedItem("entity.sopdoc.materialcode", "ja-JP", "物料编码_jp", "产品/物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.sopdoc.materialcode
            new TranslationSeedItem("entity.sopdoc.materialcode", "zh-CN", "物料编码", "产品/物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.sopdoc.materialcode
            new TranslationSeedItem("entity.sopdoc.materialcode", "zh-HK", "物料编码_hk", "产品/物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),

            // entity.sopdoc.routingitemid
            new TranslationSeedItem("entity.sopdoc.routingitemid", "en-US", "工序ID_us", "工艺路线明细 ID（选项 TaktRoutingItems/options；DictValue=Id）"),
            // entity.sopdoc.routingitemid
            new TranslationSeedItem("entity.sopdoc.routingitemid", "ja-JP", "工序ID_jp", "工艺路线明细 ID（选项 TaktRoutingItems/options；DictValue=Id）"),
            // entity.sopdoc.routingitemid
            new TranslationSeedItem("entity.sopdoc.routingitemid", "zh-CN", "工序ID", "工艺路线明细 ID（选项 TaktRoutingItems/options；DictValue=Id）"),
            // entity.sopdoc.routingitemid
            new TranslationSeedItem("entity.sopdoc.routingitemid", "zh-HK", "工序ID_hk", "工艺路线明细 ID（选项 TaktRoutingItems/options；DictValue=Id）"),

            // entity.sopdoc.workstationid
            new TranslationSeedItem("entity.sopdoc.workstationid", "en-US", "工位ID_us", "工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）"),
            // entity.sopdoc.workstationid
            new TranslationSeedItem("entity.sopdoc.workstationid", "ja-JP", "工位ID_jp", "工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）"),
            // entity.sopdoc.workstationid
            new TranslationSeedItem("entity.sopdoc.workstationid", "zh-CN", "工位ID", "工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）"),
            // entity.sopdoc.workstationid
            new TranslationSeedItem("entity.sopdoc.workstationid", "zh-HK", "工位ID_hk", "工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）"),

            // entity.sopdoc.currentrevisionid
            new TranslationSeedItem("entity.sopdoc.currentrevisionid", "en-US", "当前版本ID_us", "当前生效版本 ID（选项 TaktSopRevisions/options；DictValue=Id）"),
            // entity.sopdoc.currentrevisionid
            new TranslationSeedItem("entity.sopdoc.currentrevisionid", "ja-JP", "当前版本ID_jp", "当前生效版本 ID（选项 TaktSopRevisions/options；DictValue=Id）"),
            // entity.sopdoc.currentrevisionid
            new TranslationSeedItem("entity.sopdoc.currentrevisionid", "zh-CN", "当前版本ID", "当前生效版本 ID（选项 TaktSopRevisions/options；DictValue=Id）"),
            // entity.sopdoc.currentrevisionid
            new TranslationSeedItem("entity.sopdoc.currentrevisionid", "zh-HK", "当前版本ID_hk", "当前生效版本 ID（选项 TaktSopRevisions/options；DictValue=Id）"),

            // entity.sopdoc.sopstatus
            new TranslationSeedItem("entity.sopdoc.sopstatus", "en-US", "文档状态_us", "状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）"),
            // entity.sopdoc.sopstatus
            new TranslationSeedItem("entity.sopdoc.sopstatus", "ja-JP", "文档状态_jp", "状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）"),
            // entity.sopdoc.sopstatus
            new TranslationSeedItem("entity.sopdoc.sopstatus", "zh-CN", "文档状态", "状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）"),
            // entity.sopdoc.sopstatus
            new TranslationSeedItem("entity.sopdoc.sopstatus", "zh-HK", "文档状态_hk", "状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）"),

            // entity.sopdoc.routingitem
            new TranslationSeedItem("entity.sopdoc.routingitem", "en-US", "工序_us", "工序"),
            // entity.sopdoc.routingitem
            new TranslationSeedItem("entity.sopdoc.routingitem", "ja-JP", "工序_jp", "工序"),
            // entity.sopdoc.routingitem
            new TranslationSeedItem("entity.sopdoc.routingitem", "zh-CN", "工序", "工序"),
            // entity.sopdoc.routingitem
            new TranslationSeedItem("entity.sopdoc.routingitem", "zh-HK", "工序_hk", "工序"),

            // entity.sopdoc.workstation
            new TranslationSeedItem("entity.sopdoc.workstation", "en-US", "工位_us", "工位"),
            // entity.sopdoc.workstation
            new TranslationSeedItem("entity.sopdoc.workstation", "ja-JP", "工位_jp", "工位"),
            // entity.sopdoc.workstation
            new TranslationSeedItem("entity.sopdoc.workstation", "zh-CN", "工位", "工位"),
            // entity.sopdoc.workstation
            new TranslationSeedItem("entity.sopdoc.workstation", "zh-HK", "工位_hk", "工位"),

            // entity.sopdoc.revisions
            new TranslationSeedItem("entity.sopdoc.revisions", "en-US", "版本列表_us", "版本列表"),
            // entity.sopdoc.revisions
            new TranslationSeedItem("entity.sopdoc.revisions", "ja-JP", "版本列表_jp", "版本列表"),
            // entity.sopdoc.revisions
            new TranslationSeedItem("entity.sopdoc.revisions", "zh-CN", "版本列表", "版本列表"),
            // entity.sopdoc.revisions
            new TranslationSeedItem("entity.sopdoc.revisions", "zh-HK", "版本列表_hk", "版本列表"),
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
