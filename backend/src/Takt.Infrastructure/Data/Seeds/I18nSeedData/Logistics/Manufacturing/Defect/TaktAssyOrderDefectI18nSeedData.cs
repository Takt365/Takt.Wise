// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Defect
// 文件名称：TaktAssyOrderDefectI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktAssyOrderDefect 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktAssyOrderDefect 实体国际化翻译种子（键前缀 entity.assyorderdefect.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktAssyOrderDefectI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktAssyOrderDefect 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 assyorderdefect 实体翻译...", tenantCode);

        foreach (var item in GetAssyOrderDefectTranslations())
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

        TaktLogger.Information("TaktAssyOrderDefect 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktAssyOrderDefect 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.assyorderdefect._self / entity.assyorderdefect.{{field}}；ResourceGroup=Defect；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetAssyOrderDefectTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.assyorderdefect._self
            new TranslationSeedItem("entity.assyorderdefect._self", "en-US", "Assy Order Defect Information_us", "实体名称"),
            // entity.assyorderdefect._self
            new TranslationSeedItem("entity.assyorderdefect._self", "ja-JP", "组立工单不良统计信息_jp", "实体名称"),
            // entity.assyorderdefect._self
            new TranslationSeedItem("entity.assyorderdefect._self", "zh-CN", "组立工单不良统计信息", "实体名称"),
            // entity.assyorderdefect._self
            new TranslationSeedItem("entity.assyorderdefect._self", "zh-HK", "组立工单不良统计信息_hk", "实体名称"),

            // entity.assyorderdefect.prodcategory
            new TranslationSeedItem("entity.assyorderdefect.prodcategory", "en-US", "生产类别_us", "生产类别（统计维度，字典 logistics_manufacturing_prod_category，存 DictValue：EPP/FPP/RWP/MDP/CPP）"),
            // entity.assyorderdefect.prodcategory
            new TranslationSeedItem("entity.assyorderdefect.prodcategory", "ja-JP", "生产类别_jp", "生产类别（统计维度，字典 logistics_manufacturing_prod_category，存 DictValue：EPP/FPP/RWP/MDP/CPP）"),
            // entity.assyorderdefect.prodcategory
            new TranslationSeedItem("entity.assyorderdefect.prodcategory", "zh-CN", "生产类别", "生产类别（统计维度，字典 logistics_manufacturing_prod_category，存 DictValue：EPP/FPP/RWP/MDP/CPP）"),
            // entity.assyorderdefect.prodcategory
            new TranslationSeedItem("entity.assyorderdefect.prodcategory", "zh-HK", "生产类别_hk", "生产类别（统计维度，字典 logistics_manufacturing_prod_category，存 DictValue：EPP/FPP/RWP/MDP/CPP）"),

            // entity.assyorderdefect.prodordercode
            new TranslationSeedItem("entity.assyorderdefect.prodordercode", "en-US", "工单号_us", "工单号（统计维度，选项 TaktProductionOrders/options）"),
            // entity.assyorderdefect.prodordercode
            new TranslationSeedItem("entity.assyorderdefect.prodordercode", "ja-JP", "工单号_jp", "工单号（统计维度，选项 TaktProductionOrders/options）"),
            // entity.assyorderdefect.prodordercode
            new TranslationSeedItem("entity.assyorderdefect.prodordercode", "zh-CN", "工单号", "工单号（统计维度，选项 TaktProductionOrders/options）"),
            // entity.assyorderdefect.prodordercode
            new TranslationSeedItem("entity.assyorderdefect.prodordercode", "zh-HK", "工单号_hk", "工单号（统计维度，选项 TaktProductionOrders/options）"),

            // entity.assyorderdefect.proddategroup
            new TranslationSeedItem("entity.assyorderdefect.proddategroup", "en-US", "生产日期组_us", "生产日期组（汇总日报去重生产日期，yyyy-MM-dd 逗号分隔升序排列）"),
            // entity.assyorderdefect.proddategroup
            new TranslationSeedItem("entity.assyorderdefect.proddategroup", "ja-JP", "生产日期组_jp", "生产日期组（汇总日报去重生产日期，yyyy-MM-dd 逗号分隔升序排列）"),
            // entity.assyorderdefect.proddategroup
            new TranslationSeedItem("entity.assyorderdefect.proddategroup", "zh-CN", "生产日期组", "生产日期组（汇总日报去重生产日期，yyyy-MM-dd 逗号分隔升序排列）"),
            // entity.assyorderdefect.proddategroup
            new TranslationSeedItem("entity.assyorderdefect.proddategroup", "zh-HK", "生产日期组_hk", "生产日期组（汇总日报去重生产日期，yyyy-MM-dd 逗号分隔升序排列）"),

            // entity.assyorderdefect.modelcode
            new TranslationSeedItem("entity.assyorderdefect.modelcode", "en-US", "机种_us", "机种（取最近日报）"),
            // entity.assyorderdefect.modelcode
            new TranslationSeedItem("entity.assyorderdefect.modelcode", "ja-JP", "机种_jp", "机种（取最近日报）"),
            // entity.assyorderdefect.modelcode
            new TranslationSeedItem("entity.assyorderdefect.modelcode", "zh-CN", "机种", "机种（取最近日报）"),
            // entity.assyorderdefect.modelcode
            new TranslationSeedItem("entity.assyorderdefect.modelcode", "zh-HK", "机种_hk", "机种（取最近日报）"),

            // entity.assyorderdefect.materialcode
            new TranslationSeedItem("entity.assyorderdefect.materialcode", "en-US", "物料编码_us", "物料编码（取最近日报）"),
            // entity.assyorderdefect.materialcode
            new TranslationSeedItem("entity.assyorderdefect.materialcode", "ja-JP", "物料编码_jp", "物料编码（取最近日报）"),
            // entity.assyorderdefect.materialcode
            new TranslationSeedItem("entity.assyorderdefect.materialcode", "zh-CN", "物料编码", "物料编码（取最近日报）"),
            // entity.assyorderdefect.materialcode
            new TranslationSeedItem("entity.assyorderdefect.materialcode", "zh-HK", "物料编码_hk", "物料编码（取最近日报）"),

            // entity.assyorderdefect.batchcode
            new TranslationSeedItem("entity.assyorderdefect.batchcode", "en-US", "批次_us", "批次（一工单一批次，取最近日报）"),
            // entity.assyorderdefect.batchcode
            new TranslationSeedItem("entity.assyorderdefect.batchcode", "ja-JP", "批次_jp", "批次（一工单一批次，取最近日报）"),
            // entity.assyorderdefect.batchcode
            new TranslationSeedItem("entity.assyorderdefect.batchcode", "zh-CN", "批次", "批次（一工单一批次，取最近日报）"),
            // entity.assyorderdefect.batchcode
            new TranslationSeedItem("entity.assyorderdefect.batchcode", "zh-HK", "批次_hk", "批次（一工单一批次，取最近日报）"),

            // entity.assyorderdefect.prodorderqty
            new TranslationSeedItem("entity.assyorderdefect.prodorderqty", "en-US", "工单数量_us", "工单数量（取最近日报）"),
            // entity.assyorderdefect.prodorderqty
            new TranslationSeedItem("entity.assyorderdefect.prodorderqty", "ja-JP", "工单数量_jp", "工单数量（取最近日报）"),
            // entity.assyorderdefect.prodorderqty
            new TranslationSeedItem("entity.assyorderdefect.prodorderqty", "zh-CN", "工单数量", "工单数量（取最近日报）"),
            // entity.assyorderdefect.prodorderqty
            new TranslationSeedItem("entity.assyorderdefect.prodorderqty", "zh-HK", "工单数量_hk", "工单数量（取最近日报）"),

            // entity.assyorderdefect.prodactualqty
            new TranslationSeedItem("entity.assyorderdefect.prodactualqty", "en-US", "累计生实实绩_us", "累计生实实绩（汇总 TaktAssyDefect.ProdActualQty）"),
            // entity.assyorderdefect.prodactualqty
            new TranslationSeedItem("entity.assyorderdefect.prodactualqty", "ja-JP", "累计生实实绩_jp", "累计生实实绩（汇总 TaktAssyDefect.ProdActualQty）"),
            // entity.assyorderdefect.prodactualqty
            new TranslationSeedItem("entity.assyorderdefect.prodactualqty", "zh-CN", "累计生实实绩", "累计生实实绩（汇总 TaktAssyDefect.ProdActualQty）"),
            // entity.assyorderdefect.prodactualqty
            new TranslationSeedItem("entity.assyorderdefect.prodactualqty", "zh-HK", "累计生实实绩_hk", "累计生实实绩（汇总 TaktAssyDefect.ProdActualQty）"),

            // entity.assyorderdefect.goodquantity
            new TranslationSeedItem("entity.assyorderdefect.goodquantity", "en-US", "累计无不良数量_us", "累计无不良数量（汇总 TaktAssyDefect.GoodQuantity）"),
            // entity.assyorderdefect.goodquantity
            new TranslationSeedItem("entity.assyorderdefect.goodquantity", "ja-JP", "累计无不良数量_jp", "累计无不良数量（汇总 TaktAssyDefect.GoodQuantity）"),
            // entity.assyorderdefect.goodquantity
            new TranslationSeedItem("entity.assyorderdefect.goodquantity", "zh-CN", "累计无不良数量", "累计无不良数量（汇总 TaktAssyDefect.GoodQuantity）"),
            // entity.assyorderdefect.goodquantity
            new TranslationSeedItem("entity.assyorderdefect.goodquantity", "zh-HK", "累计无不良数量_hk", "累计无不良数量（汇总 TaktAssyDefect.GoodQuantity）"),

            // entity.assyorderdefect.defectqty
            new TranslationSeedItem("entity.assyorderdefect.defectqty", "en-US", "累计不良数量_us", "累计不良数量（计算：累计生实实绩 - 累计无不良数量）"),
            // entity.assyorderdefect.defectqty
            new TranslationSeedItem("entity.assyorderdefect.defectqty", "ja-JP", "累计不良数量_jp", "累计不良数量（计算：累计生实实绩 - 累计无不良数量）"),
            // entity.assyorderdefect.defectqty
            new TranslationSeedItem("entity.assyorderdefect.defectqty", "zh-CN", "累计不良数量", "累计不良数量（计算：累计生实实绩 - 累计无不良数量）"),
            // entity.assyorderdefect.defectqty
            new TranslationSeedItem("entity.assyorderdefect.defectqty", "zh-HK", "累计不良数量_hk", "累计不良数量（计算：累计生实实绩 - 累计无不良数量）"),

            // entity.assyorderdefect.defectratepercent
            new TranslationSeedItem("entity.assyorderdefect.defectratepercent", "en-US", "不良率_us", "不良率（%，计算：累计不良数量 ÷ 累计生实实绩 × 100）"),
            // entity.assyorderdefect.defectratepercent
            new TranslationSeedItem("entity.assyorderdefect.defectratepercent", "ja-JP", "不良率_jp", "不良率（%，计算：累计不良数量 ÷ 累计生实实绩 × 100）"),
            // entity.assyorderdefect.defectratepercent
            new TranslationSeedItem("entity.assyorderdefect.defectratepercent", "zh-CN", "不良率", "不良率（%，计算：累计不良数量 ÷ 累计生实实绩 × 100）"),
            // entity.assyorderdefect.defectratepercent
            new TranslationSeedItem("entity.assyorderdefect.defectratepercent", "zh-HK", "不良率_hk", "不良率（%，计算：累计不良数量 ÷ 累计生实实绩 × 100）"),

            // entity.assyorderdefect.yieldratepercent
            new TranslationSeedItem("entity.assyorderdefect.yieldratepercent", "en-US", "直行率_us", "直行率（%，计算：累计无不良数量 ÷ 累计生实实绩 × 100）"),
            // entity.assyorderdefect.yieldratepercent
            new TranslationSeedItem("entity.assyorderdefect.yieldratepercent", "ja-JP", "直行率_jp", "直行率（%，计算：累计无不良数量 ÷ 累计生实实绩 × 100）"),
            // entity.assyorderdefect.yieldratepercent
            new TranslationSeedItem("entity.assyorderdefect.yieldratepercent", "zh-CN", "直行率", "直行率（%，计算：累计无不良数量 ÷ 累计生实实绩 × 100）"),
            // entity.assyorderdefect.yieldratepercent
            new TranslationSeedItem("entity.assyorderdefect.yieldratepercent", "zh-HK", "直行率_hk", "直行率（%，计算：累计无不良数量 ÷ 累计生实实绩 × 100）"),

            // entity.assyorderdefect.lastproddate
            new TranslationSeedItem("entity.assyorderdefect.lastproddate", "en-US", "最近生产日期_us", "最近生产日期（关联日报最大 ProdDate）"),
            // entity.assyorderdefect.lastproddate
            new TranslationSeedItem("entity.assyorderdefect.lastproddate", "ja-JP", "最近生产日期_jp", "最近生产日期（关联日报最大 ProdDate）"),
            // entity.assyorderdefect.lastproddate
            new TranslationSeedItem("entity.assyorderdefect.lastproddate", "zh-CN", "最近生产日期", "最近生产日期（关联日报最大 ProdDate）"),
            // entity.assyorderdefect.lastproddate
            new TranslationSeedItem("entity.assyorderdefect.lastproddate", "zh-HK", "最近生产日期_hk", "最近生产日期（关联日报最大 ProdDate）"),

            // entity.assyorderdefect.reportcount
            new TranslationSeedItem("entity.assyorderdefect.reportcount", "en-US", "日报笔数_us", "关联组立不良日报笔数"),
            // entity.assyorderdefect.reportcount
            new TranslationSeedItem("entity.assyorderdefect.reportcount", "ja-JP", "日报笔数_jp", "关联组立不良日报笔数"),
            // entity.assyorderdefect.reportcount
            new TranslationSeedItem("entity.assyorderdefect.reportcount", "zh-CN", "日报笔数", "关联组立不良日报笔数"),
            // entity.assyorderdefect.reportcount
            new TranslationSeedItem("entity.assyorderdefect.reportcount", "zh-HK", "日报笔数_hk", "关联组立不良日报笔数"),

            // entity.assyorderdefect.orderstatus
            new TranslationSeedItem("entity.assyorderdefect.orderstatus", "en-US", "工单状态_us", "工单状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成；工单数量与累计生实实绩相等时为已完成）"),
            // entity.assyorderdefect.orderstatus
            new TranslationSeedItem("entity.assyorderdefect.orderstatus", "ja-JP", "工单状态_jp", "工单状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成；工单数量与累计生实实绩相等时为已完成）"),
            // entity.assyorderdefect.orderstatus
            new TranslationSeedItem("entity.assyorderdefect.orderstatus", "zh-CN", "工单状态", "工单状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成；工单数量与累计生实实绩相等时为已完成）"),
            // entity.assyorderdefect.orderstatus
            new TranslationSeedItem("entity.assyorderdefect.orderstatus", "zh-HK", "工单状态_hk", "工单状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成；工单数量与累计生实实绩相等时为已完成）"),
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
