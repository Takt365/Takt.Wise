// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Defect
// 文件名称：TaktPcbaRepairI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPcbaRepair 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Defect;

/// <summary>
/// TaktPcbaRepair 实体国际化翻译种子（键前缀 entity.pcbarepair.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPcbaRepairI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPcbaRepair 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 pcbarepair 实体翻译...", tenantCode);

        foreach (var item in GetPcbaRepairTranslations())
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

        TaktLogger.Information("TaktPcbaRepair 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPcbaRepair 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.pcbarepair._self / entity.pcbarepair.{{field}}；ResourceGroup=Defect；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPcbaRepairTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.pcbarepair._self
            new TranslationSeedItem("entity.pcbarepair._self", "en-US", "Pcba Repair Information_us", "实体名称"),
            // entity.pcbarepair._self
            new TranslationSeedItem("entity.pcbarepair._self", "ja-JP", "PCBA改修日报信息_jp", "实体名称"),
            // entity.pcbarepair._self
            new TranslationSeedItem("entity.pcbarepair._self", "zh-CN", "PCBA改修日报信息", "实体名称"),
            // entity.pcbarepair._self
            new TranslationSeedItem("entity.pcbarepair._self", "zh-HK", "PCBA改修日报信息_hk", "实体名称"),

            // entity.pcbarepair.prodcategory
            new TranslationSeedItem("entity.pcbarepair.prodcategory", "en-US", "生产类别_us", "生产类别（字典 logistics_manufacturing_prod_category；存 DictValue：EPP/FPP/RWP/MDP/CPP）"),
            // entity.pcbarepair.prodcategory
            new TranslationSeedItem("entity.pcbarepair.prodcategory", "ja-JP", "生产类别_jp", "生产类别（字典 logistics_manufacturing_prod_category；存 DictValue：EPP/FPP/RWP/MDP/CPP）"),
            // entity.pcbarepair.prodcategory
            new TranslationSeedItem("entity.pcbarepair.prodcategory", "zh-CN", "生产类别", "生产类别（字典 logistics_manufacturing_prod_category；存 DictValue：EPP/FPP/RWP/MDP/CPP）"),
            // entity.pcbarepair.prodcategory
            new TranslationSeedItem("entity.pcbarepair.prodcategory", "zh-HK", "生产类别_hk", "生产类别（字典 logistics_manufacturing_prod_category；存 DictValue：EPP/FPP/RWP/MDP/CPP）"),

            // entity.pcbarepair.proddate
            new TranslationSeedItem("entity.pcbarepair.proddate", "en-US", "生产日期_us", "生产日期"),
            // entity.pcbarepair.proddate
            new TranslationSeedItem("entity.pcbarepair.proddate", "ja-JP", "生产日期_jp", "生产日期"),
            // entity.pcbarepair.proddate
            new TranslationSeedItem("entity.pcbarepair.proddate", "zh-CN", "生产日期", "生产日期"),
            // entity.pcbarepair.proddate
            new TranslationSeedItem("entity.pcbarepair.proddate", "zh-HK", "生产日期_hk", "生产日期"),

            // entity.pcbarepair.teamcode
            new TranslationSeedItem("entity.pcbarepair.teamcode", "en-US", "生产班组_us", "生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）"),
            // entity.pcbarepair.teamcode
            new TranslationSeedItem("entity.pcbarepair.teamcode", "ja-JP", "生产班组_jp", "生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）"),
            // entity.pcbarepair.teamcode
            new TranslationSeedItem("entity.pcbarepair.teamcode", "zh-CN", "生产班组", "生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）"),
            // entity.pcbarepair.teamcode
            new TranslationSeedItem("entity.pcbarepair.teamcode", "zh-HK", "生产班组_hk", "生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）"),

            // entity.pcbarepair.shiftno
            new TranslationSeedItem("entity.pcbarepair.shiftno", "en-US", "班次_us", "班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）"),
            // entity.pcbarepair.shiftno
            new TranslationSeedItem("entity.pcbarepair.shiftno", "ja-JP", "班次_jp", "班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）"),
            // entity.pcbarepair.shiftno
            new TranslationSeedItem("entity.pcbarepair.shiftno", "zh-CN", "班次", "班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）"),
            // entity.pcbarepair.shiftno
            new TranslationSeedItem("entity.pcbarepair.shiftno", "zh-HK", "班次_hk", "班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）"),

            // entity.pcbarepair.prodordertype
            new TranslationSeedItem("entity.pcbarepair.prodordertype", "en-US", "工单类别_us", "工单类别（回填：随工单）"),
            // entity.pcbarepair.prodordertype
            new TranslationSeedItem("entity.pcbarepair.prodordertype", "ja-JP", "工单类别_jp", "工单类别（回填：随工单）"),
            // entity.pcbarepair.prodordertype
            new TranslationSeedItem("entity.pcbarepair.prodordertype", "zh-CN", "工单类别", "工单类别（回填：随工单）"),
            // entity.pcbarepair.prodordertype
            new TranslationSeedItem("entity.pcbarepair.prodordertype", "zh-HK", "工单类别_hk", "工单类别（回填：随工单）"),

            // entity.pcbarepair.prodordercode
            new TranslationSeedItem("entity.pcbarepair.prodordercode", "en-US", "工单号_us", "工单号（选项 TaktProductionOrders/options；DictValue=ProdOrderCode，ExtValue=PlantCode）"),
            // entity.pcbarepair.prodordercode
            new TranslationSeedItem("entity.pcbarepair.prodordercode", "ja-JP", "工单号_jp", "工单号（选项 TaktProductionOrders/options；DictValue=ProdOrderCode，ExtValue=PlantCode）"),
            // entity.pcbarepair.prodordercode
            new TranslationSeedItem("entity.pcbarepair.prodordercode", "zh-CN", "工单号", "工单号（选项 TaktProductionOrders/options；DictValue=ProdOrderCode，ExtValue=PlantCode）"),
            // entity.pcbarepair.prodordercode
            new TranslationSeedItem("entity.pcbarepair.prodordercode", "zh-HK", "工单号_hk", "工单号（选项 TaktProductionOrders/options；DictValue=ProdOrderCode，ExtValue=PlantCode）"),

            // entity.pcbarepair.prodorderqty
            new TranslationSeedItem("entity.pcbarepair.prodorderqty", "en-US", "工单数量_us", "工单数量"),
            // entity.pcbarepair.prodorderqty
            new TranslationSeedItem("entity.pcbarepair.prodorderqty", "ja-JP", "工单数量_jp", "工单数量"),
            // entity.pcbarepair.prodorderqty
            new TranslationSeedItem("entity.pcbarepair.prodorderqty", "zh-CN", "工单数量", "工单数量"),
            // entity.pcbarepair.prodorderqty
            new TranslationSeedItem("entity.pcbarepair.prodorderqty", "zh-HK", "工单数量_hk", "工单数量"),

            // entity.pcbarepair.modelcode
            new TranslationSeedItem("entity.pcbarepair.modelcode", "en-US", "机种_us", "机种"),
            // entity.pcbarepair.modelcode
            new TranslationSeedItem("entity.pcbarepair.modelcode", "ja-JP", "机种_jp", "机种"),
            // entity.pcbarepair.modelcode
            new TranslationSeedItem("entity.pcbarepair.modelcode", "zh-CN", "机种", "机种"),
            // entity.pcbarepair.modelcode
            new TranslationSeedItem("entity.pcbarepair.modelcode", "zh-HK", "机种_hk", "机种"),

            // entity.pcbarepair.batchcode
            new TranslationSeedItem("entity.pcbarepair.batchcode", "en-US", "批次_us", "批次"),
            // entity.pcbarepair.batchcode
            new TranslationSeedItem("entity.pcbarepair.batchcode", "ja-JP", "批次_jp", "批次"),
            // entity.pcbarepair.batchcode
            new TranslationSeedItem("entity.pcbarepair.batchcode", "zh-CN", "批次", "批次"),
            // entity.pcbarepair.batchcode
            new TranslationSeedItem("entity.pcbarepair.batchcode", "zh-HK", "批次_hk", "批次"),

            // entity.pcbarepair.materialcode
            new TranslationSeedItem("entity.pcbarepair.materialcode", "en-US", "物料编码_us", "物料编码"),
            // entity.pcbarepair.materialcode
            new TranslationSeedItem("entity.pcbarepair.materialcode", "ja-JP", "物料编码_jp", "物料编码"),
            // entity.pcbarepair.materialcode
            new TranslationSeedItem("entity.pcbarepair.materialcode", "zh-CN", "物料编码", "物料编码"),
            // entity.pcbarepair.materialcode
            new TranslationSeedItem("entity.pcbarepair.materialcode", "zh-HK", "物料编码_hk", "物料编码"),

            // entity.pcbarepair.details
            new TranslationSeedItem("entity.pcbarepair.details", "en-US", "PCBA改修明细列表_us", "PCBA改修明细列表"),
            // entity.pcbarepair.details
            new TranslationSeedItem("entity.pcbarepair.details", "ja-JP", "PCBA改修明细列表_jp", "PCBA改修明细列表"),
            // entity.pcbarepair.details
            new TranslationSeedItem("entity.pcbarepair.details", "zh-CN", "PCBA改修明细列表", "PCBA改修明细列表"),
            // entity.pcbarepair.details
            new TranslationSeedItem("entity.pcbarepair.details", "zh-HK", "PCBA改修明细列表_hk", "PCBA改修明细列表"),
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
        translation.ResourceGroup = "Defect";
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
