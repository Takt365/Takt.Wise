// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Bom
// 文件名称：TaktBomMaterialCostI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktBomMaterialCost 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktBomMaterialCost 实体国际化翻译种子（键前缀 entity.bommaterialcost.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktBomMaterialCostI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktBomMaterialCost 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 bommaterialcost 实体翻译...", tenantCode);

        foreach (var item in GetBomMaterialCostTranslations())
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

        TaktLogger.Information("TaktBomMaterialCost 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktBomMaterialCost 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.bommaterialcost._self / entity.bommaterialcost.{{field}}；ResourceGroup=Bom；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetBomMaterialCostTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.bommaterialcost._self
            new TranslationSeedItem("entity.bommaterialcost._self", "en-US", "Bom Material Cost Information_us", "实体名称"),
            // entity.bommaterialcost._self
            new TranslationSeedItem("entity.bommaterialcost._self", "ja-JP", "BOM 物料成本汇总表信息_jp", "实体名称"),
            // entity.bommaterialcost._self
            new TranslationSeedItem("entity.bommaterialcost._self", "zh-CN", "BOM 物料成本汇总表信息", "实体名称"),
            // entity.bommaterialcost._self
            new TranslationSeedItem("entity.bommaterialcost._self", "zh-HK", "BOM 物料成本汇总表信息_hk", "实体名称"),

            // entity.bommaterialcost.modelcode
            new TranslationSeedItem("entity.bommaterialcost.modelcode", "en-US", "机种编码_us", "机种编码（选项 TaktModelDestinations/model-options；DictValue=ModelCode） <para>分析/成本推移查询栏「机种」下拉：须用 TaktBomCostOptions/model-options（本表 ModelCode 去重，可按 PlantCode/MaterialType 过滤），❌ 勿用 TaktModelDestinations/model-options。</para>"),
            // entity.bommaterialcost.modelcode
            new TranslationSeedItem("entity.bommaterialcost.modelcode", "ja-JP", "机种编码_jp", "机种编码（选项 TaktModelDestinations/model-options；DictValue=ModelCode） <para>分析/成本推移查询栏「机种」下拉：须用 TaktBomCostOptions/model-options（本表 ModelCode 去重，可按 PlantCode/MaterialType 过滤），❌ 勿用 TaktModelDestinations/model-options。</para>"),
            // entity.bommaterialcost.modelcode
            new TranslationSeedItem("entity.bommaterialcost.modelcode", "zh-CN", "机种编码", "机种编码（选项 TaktModelDestinations/model-options；DictValue=ModelCode） <para>分析/成本推移查询栏「机种」下拉：须用 TaktBomCostOptions/model-options（本表 ModelCode 去重，可按 PlantCode/MaterialType 过滤），❌ 勿用 TaktModelDestinations/model-options。</para>"),
            // entity.bommaterialcost.modelcode
            new TranslationSeedItem("entity.bommaterialcost.modelcode", "zh-HK", "机种编码_hk", "机种编码（选项 TaktModelDestinations/model-options；DictValue=ModelCode） <para>分析/成本推移查询栏「机种」下拉：须用 TaktBomCostOptions/model-options（本表 ModelCode 去重，可按 PlantCode/MaterialType 过滤），❌ 勿用 TaktModelDestinations/model-options。</para>"),

            // entity.bommaterialcost.modelmonthlyaveragecost
            new TranslationSeedItem("entity.bommaterialcost.modelmonthlyaveragecost", "en-US", "机种月成本_us", "机种月平均材料成本（同工厂+核算月+机种+物料类型下各产品金额算术平均； 固定口径：核算月 &lt; 2026-06 用产品月成本，≥ 2026-06 用产品月计算；比较后有变化才更新并记 ExtField 前后值）"),
            // entity.bommaterialcost.modelmonthlyaveragecost
            new TranslationSeedItem("entity.bommaterialcost.modelmonthlyaveragecost", "ja-JP", "机种月成本_jp", "机种月平均材料成本（同工厂+核算月+机种+物料类型下各产品金额算术平均； 固定口径：核算月 &lt; 2026-06 用产品月成本，≥ 2026-06 用产品月计算；比较后有变化才更新并记 ExtField 前后值）"),
            // entity.bommaterialcost.modelmonthlyaveragecost
            new TranslationSeedItem("entity.bommaterialcost.modelmonthlyaveragecost", "zh-CN", "机种月成本", "机种月平均材料成本（同工厂+核算月+机种+物料类型下各产品金额算术平均； 固定口径：核算月 &lt; 2026-06 用产品月成本，≥ 2026-06 用产品月计算；比较后有变化才更新并记 ExtField 前后值）"),
            // entity.bommaterialcost.modelmonthlyaveragecost
            new TranslationSeedItem("entity.bommaterialcost.modelmonthlyaveragecost", "zh-HK", "机种月成本_hk", "机种月平均材料成本（同工厂+核算月+机种+物料类型下各产品金额算术平均； 固定口径：核算月 &lt; 2026-06 用产品月成本，≥ 2026-06 用产品月计算；比较后有变化才更新并记 ExtField 前后值）"),

            // entity.bommaterialcost.materialtype
            new TranslationSeedItem("entity.bommaterialcost.materialtype", "en-US", "物料类型_us", "物料类型（存 ROH/HALB/FERT 等码） <para>CRUD 表单：字典 logistics_materials_material_type。</para> <para>分析/推移查询栏：本表 MaterialType 去重 options（TaktBomCostOptions/material-type-options，含全部类型），❌ 勿与 CRUD 字典下拉混用；查询栏可空=不过滤。</para>"),
            // entity.bommaterialcost.materialtype
            new TranslationSeedItem("entity.bommaterialcost.materialtype", "ja-JP", "物料类型_jp", "物料类型（存 ROH/HALB/FERT 等码） <para>CRUD 表单：字典 logistics_materials_material_type。</para> <para>分析/推移查询栏：本表 MaterialType 去重 options（TaktBomCostOptions/material-type-options，含全部类型），❌ 勿与 CRUD 字典下拉混用；查询栏可空=不过滤。</para>"),
            // entity.bommaterialcost.materialtype
            new TranslationSeedItem("entity.bommaterialcost.materialtype", "zh-CN", "物料类型", "物料类型（存 ROH/HALB/FERT 等码） <para>CRUD 表单：字典 logistics_materials_material_type。</para> <para>分析/推移查询栏：本表 MaterialType 去重 options（TaktBomCostOptions/material-type-options，含全部类型），❌ 勿与 CRUD 字典下拉混用；查询栏可空=不过滤。</para>"),
            // entity.bommaterialcost.materialtype
            new TranslationSeedItem("entity.bommaterialcost.materialtype", "zh-HK", "物料类型_hk", "物料类型（存 ROH/HALB/FERT 等码） <para>CRUD 表单：字典 logistics_materials_material_type。</para> <para>分析/推移查询栏：本表 MaterialType 去重 options（TaktBomCostOptions/material-type-options，含全部类型），❌ 勿与 CRUD 字典下拉混用；查询栏可空=不过滤。</para>"),

            // entity.bommaterialcost.productcode
            new TranslationSeedItem("entity.bommaterialcost.productcode", "en-US", "产品编码_us", "产品编码（父件物料编码；本表业务主键之一） <para>分析/成本推移查询栏「物料」下拉：须用 TaktBomCostOptions/product-options（本表 ProductCode 去重，可按 PlantCode/MaterialType/ModelCode 过滤），❌ 勿用 TaktMaterialPlants/options 或字典 logistics_materials_material_type。</para> <para>导入时 18 位纯数字自动归一化为后 10 位。</para>"),
            // entity.bommaterialcost.productcode
            new TranslationSeedItem("entity.bommaterialcost.productcode", "ja-JP", "产品编码_jp", "产品编码（父件物料编码；本表业务主键之一） <para>分析/成本推移查询栏「物料」下拉：须用 TaktBomCostOptions/product-options（本表 ProductCode 去重，可按 PlantCode/MaterialType/ModelCode 过滤），❌ 勿用 TaktMaterialPlants/options 或字典 logistics_materials_material_type。</para> <para>导入时 18 位纯数字自动归一化为后 10 位。</para>"),
            // entity.bommaterialcost.productcode
            new TranslationSeedItem("entity.bommaterialcost.productcode", "zh-CN", "产品编码", "产品编码（父件物料编码；本表业务主键之一） <para>分析/成本推移查询栏「物料」下拉：须用 TaktBomCostOptions/product-options（本表 ProductCode 去重，可按 PlantCode/MaterialType/ModelCode 过滤），❌ 勿用 TaktMaterialPlants/options 或字典 logistics_materials_material_type。</para> <para>导入时 18 位纯数字自动归一化为后 10 位。</para>"),
            // entity.bommaterialcost.productcode
            new TranslationSeedItem("entity.bommaterialcost.productcode", "zh-HK", "产品编码_hk", "产品编码（父件物料编码；本表业务主键之一） <para>分析/成本推移查询栏「物料」下拉：须用 TaktBomCostOptions/product-options（本表 ProductCode 去重，可按 PlantCode/MaterialType/ModelCode 过滤），❌ 勿用 TaktMaterialPlants/options 或字典 logistics_materials_material_type。</para> <para>导入时 18 位纯数字自动归一化为后 10 位。</para>"),

            // entity.bommaterialcost.productdescription
            new TranslationSeedItem("entity.bommaterialcost.productdescription", "en-US", "产品描述_us", "产品描述"),
            // entity.bommaterialcost.productdescription
            new TranslationSeedItem("entity.bommaterialcost.productdescription", "ja-JP", "产品描述_jp", "产品描述"),
            // entity.bommaterialcost.productdescription
            new TranslationSeedItem("entity.bommaterialcost.productdescription", "zh-CN", "产品描述", "产品描述"),
            // entity.bommaterialcost.productdescription
            new TranslationSeedItem("entity.bommaterialcost.productdescription", "zh-HK", "产品描述_hk", "产品描述"),

            // entity.bommaterialcost.productmonthlycost
            new TranslationSeedItem("entity.bommaterialcost.productmonthlycost", "en-US", "产品月成本_us", "产品月成本（外部系统计算后的月成本；合计/重算/零价回填不得覆盖）"),
            // entity.bommaterialcost.productmonthlycost
            new TranslationSeedItem("entity.bommaterialcost.productmonthlycost", "ja-JP", "产品月成本_jp", "产品月成本（外部系统计算后的月成本；合计/重算/零价回填不得覆盖）"),
            // entity.bommaterialcost.productmonthlycost
            new TranslationSeedItem("entity.bommaterialcost.productmonthlycost", "zh-CN", "产品月成本", "产品月成本（外部系统计算后的月成本；合计/重算/零价回填不得覆盖）"),
            // entity.bommaterialcost.productmonthlycost
            new TranslationSeedItem("entity.bommaterialcost.productmonthlycost", "zh-HK", "产品月成本_hk", "产品月成本（外部系统计算后的月成本；合计/重算/零价回填不得覆盖）"),

            // entity.bommaterialcost.productmonthlycalculation
            new TranslationSeedItem("entity.bommaterialcost.productmonthlycalculation", "en-US", "产品月计算_us", "产品月计算（本系统按明细合计：生产相关=X、PCB SECT 标识为空、采购类型=F；行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数）"),
            // entity.bommaterialcost.productmonthlycalculation
            new TranslationSeedItem("entity.bommaterialcost.productmonthlycalculation", "ja-JP", "产品月计算_jp", "产品月计算（本系统按明细合计：生产相关=X、PCB SECT 标识为空、采购类型=F；行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数）"),
            // entity.bommaterialcost.productmonthlycalculation
            new TranslationSeedItem("entity.bommaterialcost.productmonthlycalculation", "zh-CN", "产品月计算", "产品月计算（本系统按明细合计：生产相关=X、PCB SECT 标识为空、采购类型=F；行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数）"),
            // entity.bommaterialcost.productmonthlycalculation
            new TranslationSeedItem("entity.bommaterialcost.productmonthlycalculation", "zh-HK", "产品月计算_hk", "产品月计算（本系统按明细合计：生产相关=X、PCB SECT 标识为空、采购类型=F；行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数）"),

            // entity.bommaterialcost.latestpurchasecost
            new TranslationSeedItem("entity.bommaterialcost.latestpurchasecost", "en-US", "最近采购成本_us", "最近采购成本（与产品月计算同一快照口径：生产相关=X、PCB SECT 标识为空、采购类型=F、用量 &gt; 0.001；行金额=组件数量×(净价÷采购价格单位)）"),
            // entity.bommaterialcost.latestpurchasecost
            new TranslationSeedItem("entity.bommaterialcost.latestpurchasecost", "ja-JP", "最近采购成本_jp", "最近采购成本（与产品月计算同一快照口径：生产相关=X、PCB SECT 标识为空、采购类型=F、用量 &gt; 0.001；行金额=组件数量×(净价÷采购价格单位)）"),
            // entity.bommaterialcost.latestpurchasecost
            new TranslationSeedItem("entity.bommaterialcost.latestpurchasecost", "zh-CN", "最近采购成本", "最近采购成本（与产品月计算同一快照口径：生产相关=X、PCB SECT 标识为空、采购类型=F、用量 &gt; 0.001；行金额=组件数量×(净价÷采购价格单位)）"),
            // entity.bommaterialcost.latestpurchasecost
            new TranslationSeedItem("entity.bommaterialcost.latestpurchasecost", "zh-HK", "最近采购成本_hk", "最近采购成本（与产品月计算同一快照口径：生产相关=X、PCB SECT 标识为空、采购类型=F、用量 &gt; 0.001；行金额=组件数量×(净价÷采购价格单位)）"),

            // entity.bommaterialcost.currencycode
            new TranslationSeedItem("entity.bommaterialcost.currencycode", "en-US", "币种_us", "币种（字典 accounting_financial_currency_code；如 CNY/USD）"),
            // entity.bommaterialcost.currencycode
            new TranslationSeedItem("entity.bommaterialcost.currencycode", "ja-JP", "币种_jp", "币种（字典 accounting_financial_currency_code；如 CNY/USD）"),
            // entity.bommaterialcost.currencycode
            new TranslationSeedItem("entity.bommaterialcost.currencycode", "zh-CN", "币种", "币种（字典 accounting_financial_currency_code；如 CNY/USD）"),
            // entity.bommaterialcost.currencycode
            new TranslationSeedItem("entity.bommaterialcost.currencycode", "zh-HK", "币种_hk", "币种（字典 accounting_financial_currency_code；如 CNY/USD）"),

            // entity.bommaterialcost.costingperiod
            new TranslationSeedItem("entity.bommaterialcost.costingperiod", "en-US", "核算期间_us", "核算期间（yyyy-MM；由核算日期推导；展示/筛选用，不参与唯一匹配）"),
            // entity.bommaterialcost.costingperiod
            new TranslationSeedItem("entity.bommaterialcost.costingperiod", "ja-JP", "核算期间_jp", "核算期间（yyyy-MM；由核算日期推导；展示/筛选用，不参与唯一匹配）"),
            // entity.bommaterialcost.costingperiod
            new TranslationSeedItem("entity.bommaterialcost.costingperiod", "zh-CN", "核算期间", "核算期间（yyyy-MM；由核算日期推导；展示/筛选用，不参与唯一匹配）"),
            // entity.bommaterialcost.costingperiod
            new TranslationSeedItem("entity.bommaterialcost.costingperiod", "zh-HK", "核算期间_hk", "核算期间（yyyy-MM；由核算日期推导；展示/筛选用，不参与唯一匹配）"),

            // entity.bommaterialcost.costingdate
            new TranslationSeedItem("entity.bommaterialcost.costingdate", "en-US", "核算日期_us", "核算日期（必须与本次成本合计/重算所用明细 TaktBomMaterialCostItem.CostingDate 一致；与 ProductCode 构成业务唯一键之一）"),
            // entity.bommaterialcost.costingdate
            new TranslationSeedItem("entity.bommaterialcost.costingdate", "ja-JP", "核算日期_jp", "核算日期（必须与本次成本合计/重算所用明细 TaktBomMaterialCostItem.CostingDate 一致；与 ProductCode 构成业务唯一键之一）"),
            // entity.bommaterialcost.costingdate
            new TranslationSeedItem("entity.bommaterialcost.costingdate", "zh-CN", "核算日期", "核算日期（必须与本次成本合计/重算所用明细 TaktBomMaterialCostItem.CostingDate 一致；与 ProductCode 构成业务唯一键之一）"),
            // entity.bommaterialcost.costingdate
            new TranslationSeedItem("entity.bommaterialcost.costingdate", "zh-HK", "核算日期_hk", "核算日期（必须与本次成本合计/重算所用明细 TaktBomMaterialCostItem.CostingDate 一致；与 ProductCode 构成业务唯一键之一）"),
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
