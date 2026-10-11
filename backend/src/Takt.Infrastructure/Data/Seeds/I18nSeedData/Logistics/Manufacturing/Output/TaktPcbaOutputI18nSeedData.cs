// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Output
// 文件名称：TaktPcbaOutputI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPcbaOutput 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Output;

/// <summary>
/// TaktPcbaOutput 实体国际化翻译种子（键前缀 entity.pcbaoutput.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPcbaOutputI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPcbaOutput 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 pcbaoutput 实体翻译...", tenantCode);

        foreach (var item in GetPcbaOutputTranslations())
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

        TaktLogger.Information("TaktPcbaOutput 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPcbaOutput 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.pcbaoutput._self / entity.pcbaoutput.{{field}}；ResourceGroup=Output；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPcbaOutputTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.pcbaoutput._self
            new TranslationSeedItem("entity.pcbaoutput._self", "en-US", "Pcba Output Information_us", "实体名称"),
            // entity.pcbaoutput._self
            new TranslationSeedItem("entity.pcbaoutput._self", "ja-JP", "PCBA日报信息_jp", "实体名称"),
            // entity.pcbaoutput._self
            new TranslationSeedItem("entity.pcbaoutput._self", "zh-CN", "PCBA日报信息", "实体名称"),
            // entity.pcbaoutput._self
            new TranslationSeedItem("entity.pcbaoutput._self", "zh-HK", "PCBA日报信息_hk", "实体名称"),

            // entity.pcbaoutput.prodcategory
            new TranslationSeedItem("entity.pcbaoutput.prodcategory", "en-US", "生产类别_us", "生产类别（字典 logistics_manufacturing_prod_category；存 DictValue：EPP/FPP/RWP/MDP/CPP）"),
            // entity.pcbaoutput.prodcategory
            new TranslationSeedItem("entity.pcbaoutput.prodcategory", "ja-JP", "生产类别_jp", "生产类别（字典 logistics_manufacturing_prod_category；存 DictValue：EPP/FPP/RWP/MDP/CPP）"),
            // entity.pcbaoutput.prodcategory
            new TranslationSeedItem("entity.pcbaoutput.prodcategory", "zh-CN", "生产类别", "生产类别（字典 logistics_manufacturing_prod_category；存 DictValue：EPP/FPP/RWP/MDP/CPP）"),
            // entity.pcbaoutput.prodcategory
            new TranslationSeedItem("entity.pcbaoutput.prodcategory", "zh-HK", "生产类别_hk", "生产类别（字典 logistics_manufacturing_prod_category；存 DictValue：EPP/FPP/RWP/MDP/CPP）"),

            // entity.pcbaoutput.proddate
            new TranslationSeedItem("entity.pcbaoutput.proddate", "en-US", "生产日期_us", "生产日期"),
            // entity.pcbaoutput.proddate
            new TranslationSeedItem("entity.pcbaoutput.proddate", "ja-JP", "生产日期_jp", "生产日期"),
            // entity.pcbaoutput.proddate
            new TranslationSeedItem("entity.pcbaoutput.proddate", "zh-CN", "生产日期", "生产日期"),
            // entity.pcbaoutput.proddate
            new TranslationSeedItem("entity.pcbaoutput.proddate", "zh-HK", "生产日期_hk", "生产日期"),

            // entity.pcbaoutput.prodordertype
            new TranslationSeedItem("entity.pcbaoutput.prodordertype", "en-US", "工单类别_us", "工单类别（回填：随工单）"),
            // entity.pcbaoutput.prodordertype
            new TranslationSeedItem("entity.pcbaoutput.prodordertype", "ja-JP", "工单类别_jp", "工单类别（回填：随工单）"),
            // entity.pcbaoutput.prodordertype
            new TranslationSeedItem("entity.pcbaoutput.prodordertype", "zh-CN", "工单类别", "工单类别（回填：随工单）"),
            // entity.pcbaoutput.prodordertype
            new TranslationSeedItem("entity.pcbaoutput.prodordertype", "zh-HK", "工单类别_hk", "工单类别（回填：随工单）"),

            // entity.pcbaoutput.prodordercode
            new TranslationSeedItem("entity.pcbaoutput.prodordercode", "en-US", "工单号_us", "工单号（选项 TaktProductionOrders/options；DictValue=ProdOrderCode，ExtValue=PlantCode）"),
            // entity.pcbaoutput.prodordercode
            new TranslationSeedItem("entity.pcbaoutput.prodordercode", "ja-JP", "工单号_jp", "工单号（选项 TaktProductionOrders/options；DictValue=ProdOrderCode，ExtValue=PlantCode）"),
            // entity.pcbaoutput.prodordercode
            new TranslationSeedItem("entity.pcbaoutput.prodordercode", "zh-CN", "工单号", "工单号（选项 TaktProductionOrders/options；DictValue=ProdOrderCode，ExtValue=PlantCode）"),
            // entity.pcbaoutput.prodordercode
            new TranslationSeedItem("entity.pcbaoutput.prodordercode", "zh-HK", "工单号_hk", "工单号（选项 TaktProductionOrders/options；DictValue=ProdOrderCode，ExtValue=PlantCode）"),

            // entity.pcbaoutput.modelcode
            new TranslationSeedItem("entity.pcbaoutput.modelcode", "en-US", "机种_us", "机种（回填：随工单）"),
            // entity.pcbaoutput.modelcode
            new TranslationSeedItem("entity.pcbaoutput.modelcode", "ja-JP", "机种_jp", "机种（回填：随工单）"),
            // entity.pcbaoutput.modelcode
            new TranslationSeedItem("entity.pcbaoutput.modelcode", "zh-CN", "机种", "机种（回填：随工单）"),
            // entity.pcbaoutput.modelcode
            new TranslationSeedItem("entity.pcbaoutput.modelcode", "zh-HK", "机种_hk", "机种（回填：随工单）"),

            // entity.pcbaoutput.materialcode
            new TranslationSeedItem("entity.pcbaoutput.materialcode", "en-US", "物料编码_us", "物料编码（回填：随工单）"),
            // entity.pcbaoutput.materialcode
            new TranslationSeedItem("entity.pcbaoutput.materialcode", "ja-JP", "物料编码_jp", "物料编码（回填：随工单）"),
            // entity.pcbaoutput.materialcode
            new TranslationSeedItem("entity.pcbaoutput.materialcode", "zh-CN", "物料编码", "物料编码（回填：随工单）"),
            // entity.pcbaoutput.materialcode
            new TranslationSeedItem("entity.pcbaoutput.materialcode", "zh-HK", "物料编码_hk", "物料编码（回填：随工单）"),

            // entity.pcbaoutput.batchcode
            new TranslationSeedItem("entity.pcbaoutput.batchcode", "en-US", "批次_us", "批次（回填：随工单）"),
            // entity.pcbaoutput.batchcode
            new TranslationSeedItem("entity.pcbaoutput.batchcode", "ja-JP", "批次_jp", "批次（回填：随工单）"),
            // entity.pcbaoutput.batchcode
            new TranslationSeedItem("entity.pcbaoutput.batchcode", "zh-CN", "批次", "批次（回填：随工单）"),
            // entity.pcbaoutput.batchcode
            new TranslationSeedItem("entity.pcbaoutput.batchcode", "zh-HK", "批次_hk", "批次（回填：随工单）"),

            // entity.pcbaoutput.prodorderqty
            new TranslationSeedItem("entity.pcbaoutput.prodorderqty", "en-US", "工单数量_us", "工单数量（回填：随工单）"),
            // entity.pcbaoutput.prodorderqty
            new TranslationSeedItem("entity.pcbaoutput.prodorderqty", "ja-JP", "工单数量_jp", "工单数量（回填：随工单）"),
            // entity.pcbaoutput.prodorderqty
            new TranslationSeedItem("entity.pcbaoutput.prodorderqty", "zh-CN", "工单数量", "工单数量（回填：随工单）"),
            // entity.pcbaoutput.prodorderqty
            new TranslationSeedItem("entity.pcbaoutput.prodorderqty", "zh-HK", "工单数量_hk", "工单数量（回填：随工单）"),

            // entity.pcbaoutput.serialcode
            new TranslationSeedItem("entity.pcbaoutput.serialcode", "en-US", "序列号_us", "序列号（回填：随工单）"),
            // entity.pcbaoutput.serialcode
            new TranslationSeedItem("entity.pcbaoutput.serialcode", "ja-JP", "序列号_jp", "序列号（回填：随工单）"),
            // entity.pcbaoutput.serialcode
            new TranslationSeedItem("entity.pcbaoutput.serialcode", "zh-CN", "序列号", "序列号（回填：随工单）"),
            // entity.pcbaoutput.serialcode
            new TranslationSeedItem("entity.pcbaoutput.serialcode", "zh-HK", "序列号_hk", "序列号（回填：随工单）"),

            // entity.pcbaoutput.details
            new TranslationSeedItem("entity.pcbaoutput.details", "en-US", "PCBA明细列表_us", "PCBA明细列表"),
            // entity.pcbaoutput.details
            new TranslationSeedItem("entity.pcbaoutput.details", "ja-JP", "PCBA明细列表_jp", "PCBA明细列表"),
            // entity.pcbaoutput.details
            new TranslationSeedItem("entity.pcbaoutput.details", "zh-CN", "PCBA明细列表", "PCBA明细列表"),
            // entity.pcbaoutput.details
            new TranslationSeedItem("entity.pcbaoutput.details", "zh-HK", "PCBA明细列表_hk", "PCBA明细列表"),
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
        translation.ResourceGroup = "Output";
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
