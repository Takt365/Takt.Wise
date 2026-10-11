// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Bom
// 文件名称：TaktStandardOperationTimeI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktStandardOperationTime 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Bom;

/// <summary>
/// TaktStandardOperationTime 实体国际化翻译种子（键前缀 entity.standardoperationtime.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktStandardOperationTimeI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktStandardOperationTime 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 standardoperationtime 实体翻译...", tenantCode);

        foreach (var item in GetStandardOperationTimeTranslations())
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

        TaktLogger.Information("TaktStandardOperationTime 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktStandardOperationTime 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.standardoperationtime._self / entity.standardoperationtime.{{field}}；ResourceGroup=Bom；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetStandardOperationTimeTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.standardoperationtime._self
            new TranslationSeedItem("entity.standardoperationtime._self", "en-US", "Standard Operation Time Information_us", "实体名称"),
            // entity.standardoperationtime._self
            new TranslationSeedItem("entity.standardoperationtime._self", "ja-JP", "标准工序时间信息_jp", "实体名称"),
            // entity.standardoperationtime._self
            new TranslationSeedItem("entity.standardoperationtime._self", "zh-CN", "标准工序时间信息", "实体名称"),
            // entity.standardoperationtime._self
            new TranslationSeedItem("entity.standardoperationtime._self", "zh-HK", "标准工序时间信息_hk", "实体名称"),

            // entity.standardoperationtime.materialcode
            new TranslationSeedItem("entity.standardoperationtime.materialcode", "en-US", "物料编码_us", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.standardoperationtime.materialcode
            new TranslationSeedItem("entity.standardoperationtime.materialcode", "ja-JP", "物料编码_jp", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.standardoperationtime.materialcode
            new TranslationSeedItem("entity.standardoperationtime.materialcode", "zh-CN", "物料编码", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.standardoperationtime.materialcode
            new TranslationSeedItem("entity.standardoperationtime.materialcode", "zh-HK", "物料编码_hk", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),

            // entity.standardoperationtime.workcenter
            new TranslationSeedItem("entity.standardoperationtime.workcenter", "en-US", "工作中心_us", "工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode，ExtValue=PlantCode）"),
            // entity.standardoperationtime.workcenter
            new TranslationSeedItem("entity.standardoperationtime.workcenter", "ja-JP", "工作中心_jp", "工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode，ExtValue=PlantCode）"),
            // entity.standardoperationtime.workcenter
            new TranslationSeedItem("entity.standardoperationtime.workcenter", "zh-CN", "工作中心", "工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode，ExtValue=PlantCode）"),
            // entity.standardoperationtime.workcenter
            new TranslationSeedItem("entity.standardoperationtime.workcenter", "zh-HK", "工作中心_hk", "工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode，ExtValue=PlantCode）"),

            // entity.standardoperationtime.operationdesc
            new TranslationSeedItem("entity.standardoperationtime.operationdesc", "en-US", "工序描述_us", "工序描述"),
            // entity.standardoperationtime.operationdesc
            new TranslationSeedItem("entity.standardoperationtime.operationdesc", "ja-JP", "工序描述_jp", "工序描述"),
            // entity.standardoperationtime.operationdesc
            new TranslationSeedItem("entity.standardoperationtime.operationdesc", "zh-CN", "工序描述", "工序描述"),
            // entity.standardoperationtime.operationdesc
            new TranslationSeedItem("entity.standardoperationtime.operationdesc", "zh-HK", "工序描述_hk", "工序描述"),

            // entity.standardoperationtime.standardminutes
            new TranslationSeedItem("entity.standardoperationtime.standardminutes", "en-US", "标准工时_us", "标准工时（分钟）"),
            // entity.standardoperationtime.standardminutes
            new TranslationSeedItem("entity.standardoperationtime.standardminutes", "ja-JP", "标准工时_jp", "标准工时（分钟）"),
            // entity.standardoperationtime.standardminutes
            new TranslationSeedItem("entity.standardoperationtime.standardminutes", "zh-CN", "标准工时", "标准工时（分钟）"),
            // entity.standardoperationtime.standardminutes
            new TranslationSeedItem("entity.standardoperationtime.standardminutes", "zh-HK", "标准工时_hk", "标准工时（分钟）"),

            // entity.standardoperationtime.timeunit
            new TranslationSeedItem("entity.standardoperationtime.timeunit", "en-US", "工时单位_us", "工时单位（字典 logistics_manufacturing_time_unit；默认 MIN）"),
            // entity.standardoperationtime.timeunit
            new TranslationSeedItem("entity.standardoperationtime.timeunit", "ja-JP", "工时单位_jp", "工时单位（字典 logistics_manufacturing_time_unit；默认 MIN）"),
            // entity.standardoperationtime.timeunit
            new TranslationSeedItem("entity.standardoperationtime.timeunit", "zh-CN", "工时单位", "工时单位（字典 logistics_manufacturing_time_unit；默认 MIN）"),
            // entity.standardoperationtime.timeunit
            new TranslationSeedItem("entity.standardoperationtime.timeunit", "zh-HK", "工时单位_hk", "工时单位（字典 logistics_manufacturing_time_unit；默认 MIN）"),

            // entity.standardoperationtime.standardshorts
            new TranslationSeedItem("entity.standardoperationtime.standardshorts", "en-US", "标准点数_us", "标准点数"),
            // entity.standardoperationtime.standardshorts
            new TranslationSeedItem("entity.standardoperationtime.standardshorts", "ja-JP", "标准点数_jp", "标准点数"),
            // entity.standardoperationtime.standardshorts
            new TranslationSeedItem("entity.standardoperationtime.standardshorts", "zh-CN", "标准点数", "标准点数"),
            // entity.standardoperationtime.standardshorts
            new TranslationSeedItem("entity.standardoperationtime.standardshorts", "zh-HK", "标准点数_hk", "标准点数"),

            // entity.standardoperationtime.pointsunit
            new TranslationSeedItem("entity.standardoperationtime.pointsunit", "en-US", "点数单位_us", "点数单位（字典 logistics_manufacturing_points_unit；默认 SHORT）"),
            // entity.standardoperationtime.pointsunit
            new TranslationSeedItem("entity.standardoperationtime.pointsunit", "ja-JP", "点数单位_jp", "点数单位（字典 logistics_manufacturing_points_unit；默认 SHORT）"),
            // entity.standardoperationtime.pointsunit
            new TranslationSeedItem("entity.standardoperationtime.pointsunit", "zh-CN", "点数单位", "点数单位（字典 logistics_manufacturing_points_unit；默认 SHORT）"),
            // entity.standardoperationtime.pointsunit
            new TranslationSeedItem("entity.standardoperationtime.pointsunit", "zh-HK", "点数单位_hk", "点数单位（字典 logistics_manufacturing_points_unit；默认 SHORT）"),

            // entity.standardoperationtime.pointstominutesrate
            new TranslationSeedItem("entity.standardoperationtime.pointstominutesrate", "en-US", "转换汇率_us", "点数转分钟汇率（decimal，精度 3 位小数；可选值参见字典 logistics_manufacturing_points_to_minutes_rate：普通=1，AI=0.028，SMT=0.045）"),
            // entity.standardoperationtime.pointstominutesrate
            new TranslationSeedItem("entity.standardoperationtime.pointstominutesrate", "ja-JP", "转换汇率_jp", "点数转分钟汇率（decimal，精度 3 位小数；可选值参见字典 logistics_manufacturing_points_to_minutes_rate：普通=1，AI=0.028，SMT=0.045）"),
            // entity.standardoperationtime.pointstominutesrate
            new TranslationSeedItem("entity.standardoperationtime.pointstominutesrate", "zh-CN", "转换汇率", "点数转分钟汇率（decimal，精度 3 位小数；可选值参见字典 logistics_manufacturing_points_to_minutes_rate：普通=1，AI=0.028，SMT=0.045）"),
            // entity.standardoperationtime.pointstominutesrate
            new TranslationSeedItem("entity.standardoperationtime.pointstominutesrate", "zh-HK", "转换汇率_hk", "点数转分钟汇率（decimal，精度 3 位小数；可选值参见字典 logistics_manufacturing_points_to_minutes_rate：普通=1，AI=0.028，SMT=0.045）"),

            // entity.standardoperationtime.convertedminutes
            new TranslationSeedItem("entity.standardoperationtime.convertedminutes", "en-US", "转换工时_us", "转换后标准工时（分钟）"),
            // entity.standardoperationtime.convertedminutes
            new TranslationSeedItem("entity.standardoperationtime.convertedminutes", "ja-JP", "转换工时_jp", "转换后标准工时（分钟）"),
            // entity.standardoperationtime.convertedminutes
            new TranslationSeedItem("entity.standardoperationtime.convertedminutes", "zh-CN", "转换工时", "转换后标准工时（分钟）"),
            // entity.standardoperationtime.convertedminutes
            new TranslationSeedItem("entity.standardoperationtime.convertedminutes", "zh-HK", "转换工时_hk", "转换后标准工时（分钟）"),

            // entity.standardoperationtime.effectivedate
            new TranslationSeedItem("entity.standardoperationtime.effectivedate", "en-US", "生效日期_us", "生效日期"),
            // entity.standardoperationtime.effectivedate
            new TranslationSeedItem("entity.standardoperationtime.effectivedate", "ja-JP", "生效日期_jp", "生效日期"),
            // entity.standardoperationtime.effectivedate
            new TranslationSeedItem("entity.standardoperationtime.effectivedate", "zh-CN", "生效日期", "生效日期"),
            // entity.standardoperationtime.effectivedate
            new TranslationSeedItem("entity.standardoperationtime.effectivedate", "zh-HK", "生效日期_hk", "生效日期"),

            // entity.standardoperationtime.expirydate
            new TranslationSeedItem("entity.standardoperationtime.expirydate", "en-US", "失效日期_us", "失效日期"),
            // entity.standardoperationtime.expirydate
            new TranslationSeedItem("entity.standardoperationtime.expirydate", "ja-JP", "失效日期_jp", "失效日期"),
            // entity.standardoperationtime.expirydate
            new TranslationSeedItem("entity.standardoperationtime.expirydate", "zh-CN", "失效日期", "失效日期"),
            // entity.standardoperationtime.expirydate
            new TranslationSeedItem("entity.standardoperationtime.expirydate", "zh-HK", "失效日期_hk", "失效日期"),
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
        translation.ResourceGroup = "Bom";
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
