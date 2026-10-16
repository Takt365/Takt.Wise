// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Mps
// 文件名称：TaktMasterProductionScheduleI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktMasterProductionSchedule 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Mps;

/// <summary>
/// TaktMasterProductionSchedule 实体国际化翻译种子（键前缀 entity.masterproductionschedule.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktMasterProductionScheduleI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktMasterProductionSchedule 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 masterproductionschedule 实体翻译...", tenantCode);

        foreach (var item in GetMasterProductionScheduleTranslations())
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

        TaktLogger.Information("TaktMasterProductionSchedule 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktMasterProductionSchedule 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.masterproductionschedule._self / entity.masterproductionschedule.{{field}}；ResourceGroup=Mps；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetMasterProductionScheduleTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.masterproductionschedule._self
            new TranslationSeedItem("entity.masterproductionschedule._self", "en-US", "Master Production Schedule Information_us", "实体名称"),
            // entity.masterproductionschedule._self
            new TranslationSeedItem("entity.masterproductionschedule._self", "ja-JP", "主生产计划 MPS 头表信息_jp", "实体名称"),
            // entity.masterproductionschedule._self
            new TranslationSeedItem("entity.masterproductionschedule._self", "zh-CN", "主生产计划 MPS 头表信息", "实体名称"),
            // entity.masterproductionschedule._self
            new TranslationSeedItem("entity.masterproductionschedule._self", "zh-HK", "主生产计划 MPS 头表信息_hk", "实体名称"),

            // entity.masterproductionschedule.mpscode
            new TranslationSeedItem("entity.masterproductionschedule.mpscode", "en-US", "MPS编码_us", "MPS 编码（前端表单选择编码规则后自动通过 TaktNumbering MPS编码规则生成并展示，非手输；单据类型菜单：MPS 计划维护）"),
            // entity.masterproductionschedule.mpscode
            new TranslationSeedItem("entity.masterproductionschedule.mpscode", "ja-JP", "MPS编码_jp", "MPS 编码（前端表单选择编码规则后自动通过 TaktNumbering MPS编码规则生成并展示，非手输；单据类型菜单：MPS 计划维护）"),
            // entity.masterproductionschedule.mpscode
            new TranslationSeedItem("entity.masterproductionschedule.mpscode", "zh-CN", "MPS编码", "MPS 编码（前端表单选择编码规则后自动通过 TaktNumbering MPS编码规则生成并展示，非手输；单据类型菜单：MPS 计划维护）"),
            // entity.masterproductionschedule.mpscode
            new TranslationSeedItem("entity.masterproductionschedule.mpscode", "zh-HK", "MPS编码_hk", "MPS 编码（前端表单选择编码规则后自动通过 TaktNumbering MPS编码规则生成并展示，非手输；单据类型菜单：MPS 计划维护）"),

            // entity.masterproductionschedule.masterdemandscheduleid
            new TranslationSeedItem("entity.masterproductionschedule.masterdemandscheduleid", "en-US", "来源MDS头表ID_us", "来源 MDS 头表 ID（Demand 层上游，关联 TaktMasterDemandSchedule.Id）"),
            // entity.masterproductionschedule.masterdemandscheduleid
            new TranslationSeedItem("entity.masterproductionschedule.masterdemandscheduleid", "ja-JP", "来源MDS头表ID_jp", "来源 MDS 头表 ID（Demand 层上游，关联 TaktMasterDemandSchedule.Id）"),
            // entity.masterproductionschedule.masterdemandscheduleid
            new TranslationSeedItem("entity.masterproductionschedule.masterdemandscheduleid", "zh-CN", "来源MDS头表ID", "来源 MDS 头表 ID（Demand 层上游，关联 TaktMasterDemandSchedule.Id）"),
            // entity.masterproductionschedule.masterdemandscheduleid
            new TranslationSeedItem("entity.masterproductionschedule.masterdemandscheduleid", "zh-HK", "来源MDS头表ID_hk", "来源 MDS 头表 ID（Demand 层上游，关联 TaktMasterDemandSchedule.Id）"),

            // entity.masterproductionschedule.mdscode
            new TranslationSeedItem("entity.masterproductionschedule.mdscode", "en-US", "来源MDS编码_us", "来源 MDS 编码（冗余）"),
            // entity.masterproductionschedule.mdscode
            new TranslationSeedItem("entity.masterproductionschedule.mdscode", "ja-JP", "来源MDS编码_jp", "来源 MDS 编码（冗余）"),
            // entity.masterproductionschedule.mdscode
            new TranslationSeedItem("entity.masterproductionschedule.mdscode", "zh-CN", "来源MDS编码", "来源 MDS 编码（冗余）"),
            // entity.masterproductionschedule.mdscode
            new TranslationSeedItem("entity.masterproductionschedule.mdscode", "zh-HK", "来源MDS编码_hk", "来源 MDS 编码（冗余）"),

            // entity.masterproductionschedule.planperiodstart
            new TranslationSeedItem("entity.masterproductionschedule.planperiodstart", "en-US", "计划周期开始_us", "计划周期开始"),
            // entity.masterproductionschedule.planperiodstart
            new TranslationSeedItem("entity.masterproductionschedule.planperiodstart", "ja-JP", "计划周期开始_jp", "计划周期开始"),
            // entity.masterproductionschedule.planperiodstart
            new TranslationSeedItem("entity.masterproductionschedule.planperiodstart", "zh-CN", "计划周期开始", "计划周期开始"),
            // entity.masterproductionschedule.planperiodstart
            new TranslationSeedItem("entity.masterproductionschedule.planperiodstart", "zh-HK", "计划周期开始_hk", "计划周期开始"),

            // entity.masterproductionschedule.planperiodend
            new TranslationSeedItem("entity.masterproductionschedule.planperiodend", "en-US", "计划周期结束_us", "计划周期结束"),
            // entity.masterproductionschedule.planperiodend
            new TranslationSeedItem("entity.masterproductionschedule.planperiodend", "ja-JP", "计划周期结束_jp", "计划周期结束"),
            // entity.masterproductionschedule.planperiodend
            new TranslationSeedItem("entity.masterproductionschedule.planperiodend", "zh-CN", "计划周期结束", "计划周期结束"),
            // entity.masterproductionschedule.planperiodend
            new TranslationSeedItem("entity.masterproductionschedule.planperiodend", "zh-HK", "计划周期结束_hk", "计划周期结束"),

            // entity.masterproductionschedule.buckettype
            new TranslationSeedItem("entity.masterproductionschedule.buckettype", "en-US", "时间桶粒度_us", "时间桶粒度（字典 mps_time_bucket_type；0=日，1=周，2=月）"),
            // entity.masterproductionschedule.buckettype
            new TranslationSeedItem("entity.masterproductionschedule.buckettype", "ja-JP", "时间桶粒度_jp", "时间桶粒度（字典 mps_time_bucket_type；0=日，1=周，2=月）"),
            // entity.masterproductionschedule.buckettype
            new TranslationSeedItem("entity.masterproductionschedule.buckettype", "zh-CN", "时间桶粒度", "时间桶粒度（字典 mps_time_bucket_type；0=日，1=周，2=月）"),
            // entity.masterproductionschedule.buckettype
            new TranslationSeedItem("entity.masterproductionschedule.buckettype", "zh-HK", "时间桶粒度_hk", "时间桶粒度（字典 mps_time_bucket_type；0=日，1=周，2=月）"),

            // entity.masterproductionschedule.schedulestatus
            new TranslationSeedItem("entity.masterproductionschedule.schedulestatus", "en-US", "计划状态_us", "计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）"),
            // entity.masterproductionschedule.schedulestatus
            new TranslationSeedItem("entity.masterproductionschedule.schedulestatus", "ja-JP", "计划状态_jp", "计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）"),
            // entity.masterproductionschedule.schedulestatus
            new TranslationSeedItem("entity.masterproductionschedule.schedulestatus", "zh-CN", "计划状态", "计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）"),
            // entity.masterproductionschedule.schedulestatus
            new TranslationSeedItem("entity.masterproductionschedule.schedulestatus", "zh-HK", "计划状态_hk", "计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）"),

            // entity.masterproductionschedule.lines
            new TranslationSeedItem("entity.masterproductionschedule.lines", "en-US", "MPS 明细行_us", "MPS 明细行"),
            // entity.masterproductionschedule.lines
            new TranslationSeedItem("entity.masterproductionschedule.lines", "ja-JP", "MPS 明细行_jp", "MPS 明细行"),
            // entity.masterproductionschedule.lines
            new TranslationSeedItem("entity.masterproductionschedule.lines", "zh-CN", "MPS 明细行", "MPS 明细行"),
            // entity.masterproductionschedule.lines
            new TranslationSeedItem("entity.masterproductionschedule.lines", "zh-HK", "MPS 明细行_hk", "MPS 明细行"),
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
        translation.ResourceGroup = "Mps";
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
