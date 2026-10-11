// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Aps
// 文件名称：TaktWorkCenterResourceI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktWorkCenterResource 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Aps;

/// <summary>
/// TaktWorkCenterResource 实体国际化翻译种子（键前缀 entity.workcenterresource.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktWorkCenterResourceI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktWorkCenterResource 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 workcenterresource 实体翻译...", tenantCode);

        foreach (var item in GetWorkCenterResourceTranslations())
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

        TaktLogger.Information("TaktWorkCenterResource 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktWorkCenterResource 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.workcenterresource._self / entity.workcenterresource.{{field}}；ResourceGroup=Aps；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetWorkCenterResourceTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.workcenterresource._self
            new TranslationSeedItem("entity.workcenterresource._self", "en-US", "Work Center Resource Information_us", "实体名称"),
            // entity.workcenterresource._self
            new TranslationSeedItem("entity.workcenterresource._self", "ja-JP", "工作中心资源信息_jp", "实体名称"),
            // entity.workcenterresource._self
            new TranslationSeedItem("entity.workcenterresource._self", "zh-CN", "工作中心资源信息", "实体名称"),
            // entity.workcenterresource._self
            new TranslationSeedItem("entity.workcenterresource._self", "zh-HK", "工作中心资源信息_hk", "实体名称"),

            // entity.workcenterresource.workcenterid
            new TranslationSeedItem("entity.workcenterresource.workcenterid", "en-US", "工作中心ID_us", "工作中心 ID（主子表关系，关联 TaktWorkCenter.Id，选项 TaktWorkCenters/options）"),
            // entity.workcenterresource.workcenterid
            new TranslationSeedItem("entity.workcenterresource.workcenterid", "ja-JP", "工作中心ID_jp", "工作中心 ID（主子表关系，关联 TaktWorkCenter.Id，选项 TaktWorkCenters/options）"),
            // entity.workcenterresource.workcenterid
            new TranslationSeedItem("entity.workcenterresource.workcenterid", "zh-CN", "工作中心ID", "工作中心 ID（主子表关系，关联 TaktWorkCenter.Id，选项 TaktWorkCenters/options）"),
            // entity.workcenterresource.workcenterid
            new TranslationSeedItem("entity.workcenterresource.workcenterid", "zh-HK", "工作中心ID_hk", "工作中心 ID（主子表关系，关联 TaktWorkCenter.Id，选项 TaktWorkCenters/options）"),

            // entity.workcenterresource.workcentercode
            new TranslationSeedItem("entity.workcenterresource.workcentercode", "en-US", "工作中心编码_us", "工作中心编码（关联 TaktWorkCenter.WorkCenterCode，冗余；选项 TaktWorkCenters/options，DictValue=WorkCenterCode）"),
            // entity.workcenterresource.workcentercode
            new TranslationSeedItem("entity.workcenterresource.workcentercode", "ja-JP", "工作中心编码_jp", "工作中心编码（关联 TaktWorkCenter.WorkCenterCode，冗余；选项 TaktWorkCenters/options，DictValue=WorkCenterCode）"),
            // entity.workcenterresource.workcentercode
            new TranslationSeedItem("entity.workcenterresource.workcentercode", "zh-CN", "工作中心编码", "工作中心编码（关联 TaktWorkCenter.WorkCenterCode，冗余；选项 TaktWorkCenters/options，DictValue=WorkCenterCode）"),
            // entity.workcenterresource.workcentercode
            new TranslationSeedItem("entity.workcenterresource.workcentercode", "zh-HK", "工作中心编码_hk", "工作中心编码（关联 TaktWorkCenter.WorkCenterCode，冗余；选项 TaktWorkCenters/options，DictValue=WorkCenterCode）"),

            // entity.workcenterresource.resourcecode
            new TranslationSeedItem("entity.workcenterresource.resourcecode", "en-US", "资源编码_us", "资源编码"),
            // entity.workcenterresource.resourcecode
            new TranslationSeedItem("entity.workcenterresource.resourcecode", "ja-JP", "资源编码_jp", "资源编码"),
            // entity.workcenterresource.resourcecode
            new TranslationSeedItem("entity.workcenterresource.resourcecode", "zh-CN", "资源编码", "资源编码"),
            // entity.workcenterresource.resourcecode
            new TranslationSeedItem("entity.workcenterresource.resourcecode", "zh-HK", "资源编码_hk", "资源编码"),

            // entity.workcenterresource.resourcename
            new TranslationSeedItem("entity.workcenterresource.resourcename", "en-US", "资源名称_us", "资源名称"),
            // entity.workcenterresource.resourcename
            new TranslationSeedItem("entity.workcenterresource.resourcename", "ja-JP", "资源名称_jp", "资源名称"),
            // entity.workcenterresource.resourcename
            new TranslationSeedItem("entity.workcenterresource.resourcename", "zh-CN", "资源名称", "资源名称"),
            // entity.workcenterresource.resourcename
            new TranslationSeedItem("entity.workcenterresource.resourcename", "zh-HK", "资源名称_hk", "资源名称"),

            // entity.workcenterresource.resourcetype
            new TranslationSeedItem("entity.workcenterresource.resourcetype", "en-US", "资源类型_us", "资源类型（字典 work_center_resource_type；0=设备，1=人员，2=模具）"),
            // entity.workcenterresource.resourcetype
            new TranslationSeedItem("entity.workcenterresource.resourcetype", "ja-JP", "资源类型_jp", "资源类型（字典 work_center_resource_type；0=设备，1=人员，2=模具）"),
            // entity.workcenterresource.resourcetype
            new TranslationSeedItem("entity.workcenterresource.resourcetype", "zh-CN", "资源类型", "资源类型（字典 work_center_resource_type；0=设备，1=人员，2=模具）"),
            // entity.workcenterresource.resourcetype
            new TranslationSeedItem("entity.workcenterresource.resourcetype", "zh-HK", "资源类型_hk", "资源类型（字典 work_center_resource_type；0=设备，1=人员，2=模具）"),

            // entity.workcenterresource.parallelcapacity
            new TranslationSeedItem("entity.workcenterresource.parallelcapacity", "en-US", "并行能力_us", "并行能力（可同时加工任务数）"),
            // entity.workcenterresource.parallelcapacity
            new TranslationSeedItem("entity.workcenterresource.parallelcapacity", "ja-JP", "并行能力_jp", "并行能力（可同时加工任务数）"),
            // entity.workcenterresource.parallelcapacity
            new TranslationSeedItem("entity.workcenterresource.parallelcapacity", "zh-CN", "并行能力", "并行能力（可同时加工任务数）"),
            // entity.workcenterresource.parallelcapacity
            new TranslationSeedItem("entity.workcenterresource.parallelcapacity", "zh-HK", "并行能力_hk", "并行能力（可同时加工任务数）"),

            // entity.workcenterresource.efficiencyrate
            new TranslationSeedItem("entity.workcenterresource.efficiencyrate", "en-US", "效率系数_us", "效率系数（1.0=标准）"),
            // entity.workcenterresource.efficiencyrate
            new TranslationSeedItem("entity.workcenterresource.efficiencyrate", "ja-JP", "效率系数_jp", "效率系数（1.0=标准）"),
            // entity.workcenterresource.efficiencyrate
            new TranslationSeedItem("entity.workcenterresource.efficiencyrate", "zh-CN", "效率系数", "效率系数（1.0=标准）"),
            // entity.workcenterresource.efficiencyrate
            new TranslationSeedItem("entity.workcenterresource.efficiencyrate", "zh-HK", "效率系数_hk", "效率系数（1.0=标准）"),

            // entity.workcenterresource.resourcestatus
            new TranslationSeedItem("entity.workcenterresource.resourcestatus", "en-US", "资源状态_us", "状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.workcenterresource.resourcestatus
            new TranslationSeedItem("entity.workcenterresource.resourcestatus", "ja-JP", "资源状态_jp", "状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.workcenterresource.resourcestatus
            new TranslationSeedItem("entity.workcenterresource.resourcestatus", "zh-CN", "资源状态", "状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.workcenterresource.resourcestatus
            new TranslationSeedItem("entity.workcenterresource.resourcestatus", "zh-HK", "资源状态_hk", "状态（字典 sys_normal_disable；1=启用，0=禁用）"),
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
        translation.ResourceGroup = "Aps";
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
