// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.LaborHour
// 文件名称：TaktPcbaRepairLaborHourI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPcbaRepairLaborHour 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.LaborHour;

/// <summary>
/// TaktPcbaRepairLaborHour 实体国际化翻译种子（键前缀 entity.pcbarepairlaborhour.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPcbaRepairLaborHourI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPcbaRepairLaborHour 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 pcbarepairlaborhour 实体翻译...", tenantCode);

        foreach (var item in GetPcbaRepairLaborHourTranslations())
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

        TaktLogger.Information("TaktPcbaRepairLaborHour 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPcbaRepairLaborHour 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.pcbarepairlaborhour._self / entity.pcbarepairlaborhour.{{field}}；ResourceGroup=LaborHour；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPcbaRepairLaborHourTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.pcbarepairlaborhour._self
            new TranslationSeedItem("entity.pcbarepairlaborhour._self", "en-US", "Pcba Repair Labor Hour Information_us", "实体名称"),
            // entity.pcbarepairlaborhour._self
            new TranslationSeedItem("entity.pcbarepairlaborhour._self", "ja-JP", "PCBA改修工数统计信息_jp", "实体名称"),
            // entity.pcbarepairlaborhour._self
            new TranslationSeedItem("entity.pcbarepairlaborhour._self", "zh-CN", "PCBA改修工数统计信息", "实体名称"),
            // entity.pcbarepairlaborhour._self
            new TranslationSeedItem("entity.pcbarepairlaborhour._self", "zh-HK", "PCBA改修工数统计信息_hk", "实体名称"),

            // entity.pcbarepairlaborhour.proddate
            new TranslationSeedItem("entity.pcbarepairlaborhour.proddate", "en-US", "生产日期_us", "生产日期"),
            // entity.pcbarepairlaborhour.proddate
            new TranslationSeedItem("entity.pcbarepairlaborhour.proddate", "ja-JP", "生产日期_jp", "生产日期"),
            // entity.pcbarepairlaborhour.proddate
            new TranslationSeedItem("entity.pcbarepairlaborhour.proddate", "zh-CN", "生产日期", "生产日期"),
            // entity.pcbarepairlaborhour.proddate
            new TranslationSeedItem("entity.pcbarepairlaborhour.proddate", "zh-HK", "生产日期_hk", "生产日期"),

            // entity.pcbarepairlaborhour.teamcode
            new TranslationSeedItem("entity.pcbarepairlaborhour.teamcode", "en-US", "生产班组_us", "生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）"),
            // entity.pcbarepairlaborhour.teamcode
            new TranslationSeedItem("entity.pcbarepairlaborhour.teamcode", "ja-JP", "生产班组_jp", "生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）"),
            // entity.pcbarepairlaborhour.teamcode
            new TranslationSeedItem("entity.pcbarepairlaborhour.teamcode", "zh-CN", "生产班组", "生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）"),
            // entity.pcbarepairlaborhour.teamcode
            new TranslationSeedItem("entity.pcbarepairlaborhour.teamcode", "zh-HK", "生产班组_hk", "生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）"),

            // entity.pcbarepairlaborhour.shiftno
            new TranslationSeedItem("entity.pcbarepairlaborhour.shiftno", "en-US", "班次_us", "班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）"),
            // entity.pcbarepairlaborhour.shiftno
            new TranslationSeedItem("entity.pcbarepairlaborhour.shiftno", "ja-JP", "班次_jp", "班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）"),
            // entity.pcbarepairlaborhour.shiftno
            new TranslationSeedItem("entity.pcbarepairlaborhour.shiftno", "zh-CN", "班次", "班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）"),
            // entity.pcbarepairlaborhour.shiftno
            new TranslationSeedItem("entity.pcbarepairlaborhour.shiftno", "zh-HK", "班次_hk", "班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）"),

            // entity.pcbarepairlaborhour.stdcapacity
            new TranslationSeedItem("entity.pcbarepairlaborhour.stdcapacity", "en-US", "标准产能_us", "标准产能（统计：TaktPcbaOutput.StdCapacity 合计）"),
            // entity.pcbarepairlaborhour.stdcapacity
            new TranslationSeedItem("entity.pcbarepairlaborhour.stdcapacity", "ja-JP", "标准产能_jp", "标准产能（统计：TaktPcbaOutput.StdCapacity 合计）"),
            // entity.pcbarepairlaborhour.stdcapacity
            new TranslationSeedItem("entity.pcbarepairlaborhour.stdcapacity", "zh-CN", "标准产能", "标准产能（统计：TaktPcbaOutput.StdCapacity 合计）"),
            // entity.pcbarepairlaborhour.stdcapacity
            new TranslationSeedItem("entity.pcbarepairlaborhour.stdcapacity", "zh-HK", "标准产能_hk", "标准产能（统计：TaktPcbaOutput.StdCapacity 合计）"),

            // entity.pcbarepairlaborhour.prodactualqty
            new TranslationSeedItem("entity.pcbarepairlaborhour.prodactualqty", "en-US", "实际生产数量_us", "实际生产数量（统计：TaktPcbaOutputDetail.DailyCompletedQty 合计）"),
            // entity.pcbarepairlaborhour.prodactualqty
            new TranslationSeedItem("entity.pcbarepairlaborhour.prodactualqty", "ja-JP", "实际生产数量_jp", "实际生产数量（统计：TaktPcbaOutputDetail.DailyCompletedQty 合计）"),
            // entity.pcbarepairlaborhour.prodactualqty
            new TranslationSeedItem("entity.pcbarepairlaborhour.prodactualqty", "zh-CN", "实际生产数量", "实际生产数量（统计：TaktPcbaOutputDetail.DailyCompletedQty 合计）"),
            // entity.pcbarepairlaborhour.prodactualqty
            new TranslationSeedItem("entity.pcbarepairlaborhour.prodactualqty", "zh-HK", "实际生产数量_hk", "实际生产数量（统计：TaktPcbaOutputDetail.DailyCompletedQty 合计）"),

            // entity.pcbarepairlaborhour.inputminutes
            new TranslationSeedItem("entity.pcbarepairlaborhour.inputminutes", "en-US", "投入工时_us", "投入工时(分钟)（统计：TaktPcbaOutputDetail.InputMinutes 合计）"),
            // entity.pcbarepairlaborhour.inputminutes
            new TranslationSeedItem("entity.pcbarepairlaborhour.inputminutes", "ja-JP", "投入工时_jp", "投入工时(分钟)（统计：TaktPcbaOutputDetail.InputMinutes 合计）"),
            // entity.pcbarepairlaborhour.inputminutes
            new TranslationSeedItem("entity.pcbarepairlaborhour.inputminutes", "zh-CN", "投入工时", "投入工时(分钟)（统计：TaktPcbaOutputDetail.InputMinutes 合计）"),
            // entity.pcbarepairlaborhour.inputminutes
            new TranslationSeedItem("entity.pcbarepairlaborhour.inputminutes", "zh-HK", "投入工时_hk", "投入工时(分钟)（统计：TaktPcbaOutputDetail.InputMinutes 合计）"),

            // entity.pcbarepairlaborhour.downtimeminutes
            new TranslationSeedItem("entity.pcbarepairlaborhour.downtimeminutes", "en-US", "停线损失工时_us", "停线损失工时(分钟)（统计：TaktPcbaOutputDetail.DowntimeMinutes 合计）"),
            // entity.pcbarepairlaborhour.downtimeminutes
            new TranslationSeedItem("entity.pcbarepairlaborhour.downtimeminutes", "ja-JP", "停线损失工时_jp", "停线损失工时(分钟)（统计：TaktPcbaOutputDetail.DowntimeMinutes 合计）"),
            // entity.pcbarepairlaborhour.downtimeminutes
            new TranslationSeedItem("entity.pcbarepairlaborhour.downtimeminutes", "zh-CN", "停线损失工时", "停线损失工时(分钟)（统计：TaktPcbaOutputDetail.DowntimeMinutes 合计）"),
            // entity.pcbarepairlaborhour.downtimeminutes
            new TranslationSeedItem("entity.pcbarepairlaborhour.downtimeminutes", "zh-HK", "停线损失工时_hk", "停线损失工时(分钟)（统计：TaktPcbaOutputDetail.DowntimeMinutes 合计）"),

            // entity.pcbarepairlaborhour.confirmminutes
            new TranslationSeedItem("entity.pcbarepairlaborhour.confirmminutes", "en-US", "报工工时_us", "报工工时(分钟)（统计：TaktPcbaOutputDetail.ConfirmMinutes 合计）"),
            // entity.pcbarepairlaborhour.confirmminutes
            new TranslationSeedItem("entity.pcbarepairlaborhour.confirmminutes", "ja-JP", "报工工时_jp", "报工工时(分钟)（统计：TaktPcbaOutputDetail.ConfirmMinutes 合计）"),
            // entity.pcbarepairlaborhour.confirmminutes
            new TranslationSeedItem("entity.pcbarepairlaborhour.confirmminutes", "zh-CN", "报工工时", "报工工时(分钟)（统计：TaktPcbaOutputDetail.ConfirmMinutes 合计）"),
            // entity.pcbarepairlaborhour.confirmminutes
            new TranslationSeedItem("entity.pcbarepairlaborhour.confirmminutes", "zh-HK", "报工工时_hk", "报工工时(分钟)（统计：TaktPcbaOutputDetail.ConfirmMinutes 合计）"),

            // entity.pcbarepairlaborhour.actualminutes
            new TranslationSeedItem("entity.pcbarepairlaborhour.actualminutes", "en-US", "实际工时_us", "实际工时(分钟)（统计：TaktPcbaOutputDetail.ActualMinutes 合计）"),
            // entity.pcbarepairlaborhour.actualminutes
            new TranslationSeedItem("entity.pcbarepairlaborhour.actualminutes", "ja-JP", "实际工时_jp", "实际工时(分钟)（统计：TaktPcbaOutputDetail.ActualMinutes 合计）"),
            // entity.pcbarepairlaborhour.actualminutes
            new TranslationSeedItem("entity.pcbarepairlaborhour.actualminutes", "zh-CN", "实际工时", "实际工时(分钟)（统计：TaktPcbaOutputDetail.ActualMinutes 合计）"),
            // entity.pcbarepairlaborhour.actualminutes
            new TranslationSeedItem("entity.pcbarepairlaborhour.actualminutes", "zh-HK", "实际工时_hk", "实际工时(分钟)（统计：TaktPcbaOutputDetail.ActualMinutes 合计）"),
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
        translation.ResourceGroup = "LaborHour";
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
