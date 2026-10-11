// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Mds
// 文件名称：TaktMasterDemandScheduleI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktMasterDemandSchedule 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Mds;

/// <summary>
/// TaktMasterDemandSchedule 实体国际化翻译种子（键前缀 entity.masterdemandschedule.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktMasterDemandScheduleI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktMasterDemandSchedule 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 masterdemandschedule 实体翻译...", tenantCode);

        foreach (var item in GetMasterDemandScheduleTranslations())
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

        TaktLogger.Information("TaktMasterDemandSchedule 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktMasterDemandSchedule 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.masterdemandschedule._self / entity.masterdemandschedule.{{field}}；ResourceGroup=Mds；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetMasterDemandScheduleTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.masterdemandschedule._self
            new TranslationSeedItem("entity.masterdemandschedule._self", "en-US", "Master Demand Schedule Information_us", "实体名称"),
            // entity.masterdemandschedule._self
            new TranslationSeedItem("entity.masterdemandschedule._self", "ja-JP", "主需求计划 MDS 头表信息_jp", "实体名称"),
            // entity.masterdemandschedule._self
            new TranslationSeedItem("entity.masterdemandschedule._self", "zh-CN", "主需求计划 MDS 头表信息", "实体名称"),
            // entity.masterdemandschedule._self
            new TranslationSeedItem("entity.masterdemandschedule._self", "zh-HK", "主需求计划 MDS 头表信息_hk", "实体名称"),

            // entity.masterdemandschedule.mdscode
            new TranslationSeedItem("entity.masterdemandschedule.mdscode", "en-US", "MDS编码_us", "MDS 编码（租户+公司+工厂内业务唯一）"),
            // entity.masterdemandschedule.mdscode
            new TranslationSeedItem("entity.masterdemandschedule.mdscode", "ja-JP", "MDS编码_jp", "MDS 编码（租户+公司+工厂内业务唯一）"),
            // entity.masterdemandschedule.mdscode
            new TranslationSeedItem("entity.masterdemandschedule.mdscode", "zh-CN", "MDS编码", "MDS 编码（租户+公司+工厂内业务唯一）"),
            // entity.masterdemandschedule.mdscode
            new TranslationSeedItem("entity.masterdemandschedule.mdscode", "zh-HK", "MDS编码_hk", "MDS 编码（租户+公司+工厂内业务唯一）"),

            // entity.masterdemandschedule.planperiodstart
            new TranslationSeedItem("entity.masterdemandschedule.planperiodstart", "en-US", "计划周期开始_us", "计划周期开始"),
            // entity.masterdemandschedule.planperiodstart
            new TranslationSeedItem("entity.masterdemandschedule.planperiodstart", "ja-JP", "计划周期开始_jp", "计划周期开始"),
            // entity.masterdemandschedule.planperiodstart
            new TranslationSeedItem("entity.masterdemandschedule.planperiodstart", "zh-CN", "计划周期开始", "计划周期开始"),
            // entity.masterdemandschedule.planperiodstart
            new TranslationSeedItem("entity.masterdemandschedule.planperiodstart", "zh-HK", "计划周期开始_hk", "计划周期开始"),

            // entity.masterdemandschedule.planperiodend
            new TranslationSeedItem("entity.masterdemandschedule.planperiodend", "en-US", "计划周期结束_us", "计划周期结束"),
            // entity.masterdemandschedule.planperiodend
            new TranslationSeedItem("entity.masterdemandschedule.planperiodend", "ja-JP", "计划周期结束_jp", "计划周期结束"),
            // entity.masterdemandschedule.planperiodend
            new TranslationSeedItem("entity.masterdemandschedule.planperiodend", "zh-CN", "计划周期结束", "计划周期结束"),
            // entity.masterdemandschedule.planperiodend
            new TranslationSeedItem("entity.masterdemandschedule.planperiodend", "zh-HK", "计划周期结束_hk", "计划周期结束"),

            // entity.masterdemandschedule.buckettype
            new TranslationSeedItem("entity.masterdemandschedule.buckettype", "en-US", "时间桶粒度_us", "时间桶粒度（字典 mps_time_bucket_type；0=日，1=周，2=月）"),
            // entity.masterdemandschedule.buckettype
            new TranslationSeedItem("entity.masterdemandschedule.buckettype", "ja-JP", "时间桶粒度_jp", "时间桶粒度（字典 mps_time_bucket_type；0=日，1=周，2=月）"),
            // entity.masterdemandschedule.buckettype
            new TranslationSeedItem("entity.masterdemandschedule.buckettype", "zh-CN", "时间桶粒度", "时间桶粒度（字典 mps_time_bucket_type；0=日，1=周，2=月）"),
            // entity.masterdemandschedule.buckettype
            new TranslationSeedItem("entity.masterdemandschedule.buckettype", "zh-HK", "时间桶粒度_hk", "时间桶粒度（字典 mps_time_bucket_type；0=日，1=周，2=月）"),

            // entity.masterdemandschedule.schedulestatus
            new TranslationSeedItem("entity.masterdemandschedule.schedulestatus", "en-US", "计划状态_us", "计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）"),
            // entity.masterdemandschedule.schedulestatus
            new TranslationSeedItem("entity.masterdemandschedule.schedulestatus", "ja-JP", "计划状态_jp", "计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）"),
            // entity.masterdemandschedule.schedulestatus
            new TranslationSeedItem("entity.masterdemandschedule.schedulestatus", "zh-CN", "计划状态", "计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）"),
            // entity.masterdemandschedule.schedulestatus
            new TranslationSeedItem("entity.masterdemandschedule.schedulestatus", "zh-HK", "计划状态_hk", "计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）"),

            // entity.masterdemandschedule.lines
            new TranslationSeedItem("entity.masterdemandschedule.lines", "en-US", "MDS 明细行_us", "MDS 明细行（按物料与时间桶）"),
            // entity.masterdemandschedule.lines
            new TranslationSeedItem("entity.masterdemandschedule.lines", "ja-JP", "MDS 明细行_jp", "MDS 明细行（按物料与时间桶）"),
            // entity.masterdemandschedule.lines
            new TranslationSeedItem("entity.masterdemandschedule.lines", "zh-CN", "MDS 明细行", "MDS 明细行（按物料与时间桶）"),
            // entity.masterdemandschedule.lines
            new TranslationSeedItem("entity.masterdemandschedule.lines", "zh-HK", "MDS 明细行_hk", "MDS 明细行（按物料与时间桶）"),
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
        translation.ResourceGroup = "Mds";
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
