// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Defect
// 文件名称：TaktAssyBatchDefectI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktAssyBatchDefect 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktAssyBatchDefect 实体国际化翻译种子（键前缀 entity.assybatchdefect.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktAssyBatchDefectI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktAssyBatchDefect 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 assybatchdefect 实体翻译...", tenantCode);

        foreach (var item in GetAssyBatchDefectTranslations())
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

        TaktLogger.Information("TaktAssyBatchDefect 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktAssyBatchDefect 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.assybatchdefect._self / entity.assybatchdefect.{{field}}；ResourceGroup=Defect；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetAssyBatchDefectTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.assybatchdefect._self
            new TranslationSeedItem("entity.assybatchdefect._self", "en-US", "Assy Batch Defect Information_us", "实体名称"),
            // entity.assybatchdefect._self
            new TranslationSeedItem("entity.assybatchdefect._self", "ja-JP", "组立批量不良统计信息_jp", "实体名称"),
            // entity.assybatchdefect._self
            new TranslationSeedItem("entity.assybatchdefect._self", "zh-CN", "组立批量不良统计信息", "实体名称"),
            // entity.assybatchdefect._self
            new TranslationSeedItem("entity.assybatchdefect._self", "zh-HK", "组立批量不良统计信息_hk", "实体名称"),

            // entity.assybatchdefect.prodcategory
            new TranslationSeedItem("entity.assybatchdefect.prodcategory", "en-US", "生产类别_us", "生产类别（统计维度，字典 logistics_manufacturing_prod_category，存 DictValue：EPP/FPP/RWP/MDP/CPP）"),
            // entity.assybatchdefect.prodcategory
            new TranslationSeedItem("entity.assybatchdefect.prodcategory", "ja-JP", "生产类别_jp", "生产类别（统计维度，字典 logistics_manufacturing_prod_category，存 DictValue：EPP/FPP/RWP/MDP/CPP）"),
            // entity.assybatchdefect.prodcategory
            new TranslationSeedItem("entity.assybatchdefect.prodcategory", "zh-CN", "生产类别", "生产类别（统计维度，字典 logistics_manufacturing_prod_category，存 DictValue：EPP/FPP/RWP/MDP/CPP）"),
            // entity.assybatchdefect.prodcategory
            new TranslationSeedItem("entity.assybatchdefect.prodcategory", "zh-HK", "生产类别_hk", "生产类别（统计维度，字典 logistics_manufacturing_prod_category，存 DictValue：EPP/FPP/RWP/MDP/CPP）"),

            // entity.assybatchdefect.batchcode
            new TranslationSeedItem("entity.assybatchdefect.batchcode", "en-US", "批次_us", "批次（统计维度）"),
            // entity.assybatchdefect.batchcode
            new TranslationSeedItem("entity.assybatchdefect.batchcode", "ja-JP", "批次_jp", "批次（统计维度）"),
            // entity.assybatchdefect.batchcode
            new TranslationSeedItem("entity.assybatchdefect.batchcode", "zh-CN", "批次", "批次（统计维度）"),
            // entity.assybatchdefect.batchcode
            new TranslationSeedItem("entity.assybatchdefect.batchcode", "zh-HK", "批次_hk", "批次（统计维度）"),

            // entity.assybatchdefect.proddategroup
            new TranslationSeedItem("entity.assybatchdefect.proddategroup", "en-US", "生产日期组_us", "生产日期组（与生产工单组一一对应，yyyy-MM-dd 逗号分隔，取同工单最早生产日期）"),
            // entity.assybatchdefect.proddategroup
            new TranslationSeedItem("entity.assybatchdefect.proddategroup", "ja-JP", "生产日期组_jp", "生产日期组（与生产工单组一一对应，yyyy-MM-dd 逗号分隔，取同工单最早生产日期）"),
            // entity.assybatchdefect.proddategroup
            new TranslationSeedItem("entity.assybatchdefect.proddategroup", "zh-CN", "生产日期组", "生产日期组（与生产工单组一一对应，yyyy-MM-dd 逗号分隔，取同工单最早生产日期）"),
            // entity.assybatchdefect.proddategroup
            new TranslationSeedItem("entity.assybatchdefect.proddategroup", "zh-HK", "生产日期组_hk", "生产日期组（与生产工单组一一对应，yyyy-MM-dd 逗号分隔，取同工单最早生产日期）"),

            // entity.assybatchdefect.prodordergroup
            new TranslationSeedItem("entity.assybatchdefect.prodordergroup", "en-US", "生产工单组_us", "生产工单组（同批次 Distinct 工单号逗号分隔，与生产日期组、生产物料组、订单数量组一一对应）"),
            // entity.assybatchdefect.prodordergroup
            new TranslationSeedItem("entity.assybatchdefect.prodordergroup", "ja-JP", "生产工单组_jp", "生产工单组（同批次 Distinct 工单号逗号分隔，与生产日期组、生产物料组、订单数量组一一对应）"),
            // entity.assybatchdefect.prodordergroup
            new TranslationSeedItem("entity.assybatchdefect.prodordergroup", "zh-CN", "生产工单组", "生产工单组（同批次 Distinct 工单号逗号分隔，与生产日期组、生产物料组、订单数量组一一对应）"),
            // entity.assybatchdefect.prodordergroup
            new TranslationSeedItem("entity.assybatchdefect.prodordergroup", "zh-HK", "生产工单组_hk", "生产工单组（同批次 Distinct 工单号逗号分隔，与生产日期组、生产物料组、订单数量组一一对应）"),

            // entity.assybatchdefect.modelcode
            new TranslationSeedItem("entity.assybatchdefect.modelcode", "en-US", "机种_us", "机种（取最近日报）"),
            // entity.assybatchdefect.modelcode
            new TranslationSeedItem("entity.assybatchdefect.modelcode", "ja-JP", "机种_jp", "机种（取最近日报）"),
            // entity.assybatchdefect.modelcode
            new TranslationSeedItem("entity.assybatchdefect.modelcode", "zh-CN", "机种", "机种（取最近日报）"),
            // entity.assybatchdefect.modelcode
            new TranslationSeedItem("entity.assybatchdefect.modelcode", "zh-HK", "机种_hk", "机种（取最近日报）"),

            // entity.assybatchdefect.materialgroup
            new TranslationSeedItem("entity.assybatchdefect.materialgroup", "en-US", "生产物料组_us", "生产物料组（与生产工单组一一对应，逗号分隔，同工单取最近日报物料编码）"),
            // entity.assybatchdefect.materialgroup
            new TranslationSeedItem("entity.assybatchdefect.materialgroup", "ja-JP", "生产物料组_jp", "生产物料组（与生产工单组一一对应，逗号分隔，同工单取最近日报物料编码）"),
            // entity.assybatchdefect.materialgroup
            new TranslationSeedItem("entity.assybatchdefect.materialgroup", "zh-CN", "生产物料组", "生产物料组（与生产工单组一一对应，逗号分隔，同工单取最近日报物料编码）"),
            // entity.assybatchdefect.materialgroup
            new TranslationSeedItem("entity.assybatchdefect.materialgroup", "zh-HK", "生产物料组_hk", "生产物料组（与生产工单组一一对应，逗号分隔，同工单取最近日报物料编码）"),

            // entity.assybatchdefect.batchorderqty
            new TranslationSeedItem("entity.assybatchdefect.batchorderqty", "en-US", "批次工单总数量_us", "批次工单总数量（同批次下各生产工单订单数量汇总：同工单取最大订单数量再合计）"),
            // entity.assybatchdefect.batchorderqty
            new TranslationSeedItem("entity.assybatchdefect.batchorderqty", "ja-JP", "批次工单总数量_jp", "批次工单总数量（同批次下各生产工单订单数量汇总：同工单取最大订单数量再合计）"),
            // entity.assybatchdefect.batchorderqty
            new TranslationSeedItem("entity.assybatchdefect.batchorderqty", "zh-CN", "批次工单总数量", "批次工单总数量（同批次下各生产工单订单数量汇总：同工单取最大订单数量再合计）"),
            // entity.assybatchdefect.batchorderqty
            new TranslationSeedItem("entity.assybatchdefect.batchorderqty", "zh-HK", "批次工单总数量_hk", "批次工单总数量（同批次下各生产工单订单数量汇总：同工单取最大订单数量再合计）"),

            // entity.assybatchdefect.prodorderqtygroup
            new TranslationSeedItem("entity.assybatchdefect.prodorderqtygroup", "en-US", "订单数量组_us", "订单数量组（与生产工单组一一对应，逗号分隔，同工单取最大订单数量）"),
            // entity.assybatchdefect.prodorderqtygroup
            new TranslationSeedItem("entity.assybatchdefect.prodorderqtygroup", "ja-JP", "订单数量组_jp", "订单数量组（与生产工单组一一对应，逗号分隔，同工单取最大订单数量）"),
            // entity.assybatchdefect.prodorderqtygroup
            new TranslationSeedItem("entity.assybatchdefect.prodorderqtygroup", "zh-CN", "订单数量组", "订单数量组（与生产工单组一一对应，逗号分隔，同工单取最大订单数量）"),
            // entity.assybatchdefect.prodorderqtygroup
            new TranslationSeedItem("entity.assybatchdefect.prodorderqtygroup", "zh-HK", "订单数量组_hk", "订单数量组（与生产工单组一一对应，逗号分隔，同工单取最大订单数量）"),

            // entity.assybatchdefect.prodactualqty
            new TranslationSeedItem("entity.assybatchdefect.prodactualqty", "en-US", "累计生实实绩_us", "累计生实实绩（汇总 TaktAssyDefect.ProdActualQty）"),
            // entity.assybatchdefect.prodactualqty
            new TranslationSeedItem("entity.assybatchdefect.prodactualqty", "ja-JP", "累计生实实绩_jp", "累计生实实绩（汇总 TaktAssyDefect.ProdActualQty）"),
            // entity.assybatchdefect.prodactualqty
            new TranslationSeedItem("entity.assybatchdefect.prodactualqty", "zh-CN", "累计生实实绩", "累计生实实绩（汇总 TaktAssyDefect.ProdActualQty）"),
            // entity.assybatchdefect.prodactualqty
            new TranslationSeedItem("entity.assybatchdefect.prodactualqty", "zh-HK", "累计生实实绩_hk", "累计生实实绩（汇总 TaktAssyDefect.ProdActualQty）"),

            // entity.assybatchdefect.goodquantity
            new TranslationSeedItem("entity.assybatchdefect.goodquantity", "en-US", "累计无不良数量_us", "累计无不良数量（汇总 TaktAssyDefect.GoodQuantity）"),
            // entity.assybatchdefect.goodquantity
            new TranslationSeedItem("entity.assybatchdefect.goodquantity", "ja-JP", "累计无不良数量_jp", "累计无不良数量（汇总 TaktAssyDefect.GoodQuantity）"),
            // entity.assybatchdefect.goodquantity
            new TranslationSeedItem("entity.assybatchdefect.goodquantity", "zh-CN", "累计无不良数量", "累计无不良数量（汇总 TaktAssyDefect.GoodQuantity）"),
            // entity.assybatchdefect.goodquantity
            new TranslationSeedItem("entity.assybatchdefect.goodquantity", "zh-HK", "累计无不良数量_hk", "累计无不良数量（汇总 TaktAssyDefect.GoodQuantity）"),

            // entity.assybatchdefect.defectqty
            new TranslationSeedItem("entity.assybatchdefect.defectqty", "en-US", "累计不良数量_us", "累计不良数量（计算：累计生实实绩 - 累计无不良数量）"),
            // entity.assybatchdefect.defectqty
            new TranslationSeedItem("entity.assybatchdefect.defectqty", "ja-JP", "累计不良数量_jp", "累计不良数量（计算：累计生实实绩 - 累计无不良数量）"),
            // entity.assybatchdefect.defectqty
            new TranslationSeedItem("entity.assybatchdefect.defectqty", "zh-CN", "累计不良数量", "累计不良数量（计算：累计生实实绩 - 累计无不良数量）"),
            // entity.assybatchdefect.defectqty
            new TranslationSeedItem("entity.assybatchdefect.defectqty", "zh-HK", "累计不良数量_hk", "累计不良数量（计算：累计生实实绩 - 累计无不良数量）"),

            // entity.assybatchdefect.defectratepercent
            new TranslationSeedItem("entity.assybatchdefect.defectratepercent", "en-US", "不良率_us", "不良率（%，计算：累计不良数量 ÷ 累计生实实绩 × 100）"),
            // entity.assybatchdefect.defectratepercent
            new TranslationSeedItem("entity.assybatchdefect.defectratepercent", "ja-JP", "不良率_jp", "不良率（%，计算：累计不良数量 ÷ 累计生实实绩 × 100）"),
            // entity.assybatchdefect.defectratepercent
            new TranslationSeedItem("entity.assybatchdefect.defectratepercent", "zh-CN", "不良率", "不良率（%，计算：累计不良数量 ÷ 累计生实实绩 × 100）"),
            // entity.assybatchdefect.defectratepercent
            new TranslationSeedItem("entity.assybatchdefect.defectratepercent", "zh-HK", "不良率_hk", "不良率（%，计算：累计不良数量 ÷ 累计生实实绩 × 100）"),

            // entity.assybatchdefect.yieldratepercent
            new TranslationSeedItem("entity.assybatchdefect.yieldratepercent", "en-US", "直行率_us", "直行率（%，计算：累计无不良数量 ÷ 累计生实实绩 × 100）"),
            // entity.assybatchdefect.yieldratepercent
            new TranslationSeedItem("entity.assybatchdefect.yieldratepercent", "ja-JP", "直行率_jp", "直行率（%，计算：累计无不良数量 ÷ 累计生实实绩 × 100）"),
            // entity.assybatchdefect.yieldratepercent
            new TranslationSeedItem("entity.assybatchdefect.yieldratepercent", "zh-CN", "直行率", "直行率（%，计算：累计无不良数量 ÷ 累计生实实绩 × 100）"),
            // entity.assybatchdefect.yieldratepercent
            new TranslationSeedItem("entity.assybatchdefect.yieldratepercent", "zh-HK", "直行率_hk", "直行率（%，计算：累计无不良数量 ÷ 累计生实实绩 × 100）"),

            // entity.assybatchdefect.lastproddate
            new TranslationSeedItem("entity.assybatchdefect.lastproddate", "en-US", "最近生产日期_us", "最近生产日期（关联日报最大 ProdDate）"),
            // entity.assybatchdefect.lastproddate
            new TranslationSeedItem("entity.assybatchdefect.lastproddate", "ja-JP", "最近生产日期_jp", "最近生产日期（关联日报最大 ProdDate）"),
            // entity.assybatchdefect.lastproddate
            new TranslationSeedItem("entity.assybatchdefect.lastproddate", "zh-CN", "最近生产日期", "最近生产日期（关联日报最大 ProdDate）"),
            // entity.assybatchdefect.lastproddate
            new TranslationSeedItem("entity.assybatchdefect.lastproddate", "zh-HK", "最近生产日期_hk", "最近生产日期（关联日报最大 ProdDate）"),

            // entity.assybatchdefect.reportcount
            new TranslationSeedItem("entity.assybatchdefect.reportcount", "en-US", "日报笔数_us", "关联组立不良日报笔数"),
            // entity.assybatchdefect.reportcount
            new TranslationSeedItem("entity.assybatchdefect.reportcount", "ja-JP", "日报笔数_jp", "关联组立不良日报笔数"),
            // entity.assybatchdefect.reportcount
            new TranslationSeedItem("entity.assybatchdefect.reportcount", "zh-CN", "日报笔数", "关联组立不良日报笔数"),
            // entity.assybatchdefect.reportcount
            new TranslationSeedItem("entity.assybatchdefect.reportcount", "zh-HK", "日报笔数_hk", "关联组立不良日报笔数"),

            // entity.assybatchdefect.batchstatus
            new TranslationSeedItem("entity.assybatchdefect.batchstatus", "en-US", "批次状态_us", "批次状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成；批次工单总数量与累计生实实绩相等时为已完成）"),
            // entity.assybatchdefect.batchstatus
            new TranslationSeedItem("entity.assybatchdefect.batchstatus", "ja-JP", "批次状态_jp", "批次状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成；批次工单总数量与累计生实实绩相等时为已完成）"),
            // entity.assybatchdefect.batchstatus
            new TranslationSeedItem("entity.assybatchdefect.batchstatus", "zh-CN", "批次状态", "批次状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成；批次工单总数量与累计生实实绩相等时为已完成）"),
            // entity.assybatchdefect.batchstatus
            new TranslationSeedItem("entity.assybatchdefect.batchstatus", "zh-HK", "批次状态_hk", "批次状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成；批次工单总数量与累计生实实绩相等时为已完成）"),
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
