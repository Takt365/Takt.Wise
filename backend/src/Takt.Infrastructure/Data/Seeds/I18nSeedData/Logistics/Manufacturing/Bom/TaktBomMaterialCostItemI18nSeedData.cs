// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Bom
// 文件名称：TaktBomMaterialCostItemI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktBomMaterialCostItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktBomMaterialCostItem 实体国际化翻译种子（键前缀 entity.bommaterialcostitem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktBomMaterialCostItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktBomMaterialCostItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 bommaterialcostitem 实体翻译...", tenantCode);

        foreach (var item in GetBomMaterialCostItemTranslations())
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

        TaktLogger.Information("TaktBomMaterialCostItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktBomMaterialCostItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.bommaterialcostitem._self / entity.bommaterialcostitem.{{field}}；ResourceGroup=Bom；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetBomMaterialCostItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.bommaterialcostitem._self
            new TranslationSeedItem("entity.bommaterialcostitem._self", "en-US", "Bom Material Cost Item Information_us", "实体名称"),
            // entity.bommaterialcostitem._self
            new TranslationSeedItem("entity.bommaterialcostitem._self", "ja-JP", "BOM 物料成本明细行信息_jp", "实体名称"),
            // entity.bommaterialcostitem._self
            new TranslationSeedItem("entity.bommaterialcostitem._self", "zh-CN", "BOM 物料成本明细行信息", "实体名称"),
            // entity.bommaterialcostitem._self
            new TranslationSeedItem("entity.bommaterialcostitem._self", "zh-HK", "BOM 物料成本明细行信息_hk", "实体名称"),

            // entity.bommaterialcostitem.bomlevel
            new TranslationSeedItem("entity.bommaterialcostitem.bomlevel", "en-US", "层级_us", "层级（BOM 展开层级，如 01/02）"),
            // entity.bommaterialcostitem.bomlevel
            new TranslationSeedItem("entity.bommaterialcostitem.bomlevel", "ja-JP", "层级_jp", "层级（BOM 展开层级，如 01/02）"),
            // entity.bommaterialcostitem.bomlevel
            new TranslationSeedItem("entity.bommaterialcostitem.bomlevel", "zh-CN", "层级", "层级（BOM 展开层级，如 01/02）"),
            // entity.bommaterialcostitem.bomlevel
            new TranslationSeedItem("entity.bommaterialcostitem.bomlevel", "zh-HK", "层级_hk", "层级（BOM 展开层级，如 01/02）"),

            // entity.bommaterialcostitem.bomitemcode
            new TranslationSeedItem("entity.bommaterialcostitem.bomitemcode", "en-US", "BOM项目号_us", "BOM 项目号（子件行项目号，如 0010）"),
            // entity.bommaterialcostitem.bomitemcode
            new TranslationSeedItem("entity.bommaterialcostitem.bomitemcode", "ja-JP", "BOM项目号_jp", "BOM 项目号（子件行项目号，如 0010）"),
            // entity.bommaterialcostitem.bomitemcode
            new TranslationSeedItem("entity.bommaterialcostitem.bomitemcode", "zh-CN", "BOM项目号", "BOM 项目号（子件行项目号，如 0010）"),
            // entity.bommaterialcostitem.bomitemcode
            new TranslationSeedItem("entity.bommaterialcostitem.bomitemcode", "zh-HK", "BOM项目号_hk", "BOM 项目号（子件行项目号，如 0010）"),

            // entity.bommaterialcostitem.productcode
            new TranslationSeedItem("entity.bommaterialcostitem.productcode", "en-US", "产品编码_us", "产品编码（父件物料编码，选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）；导入时 18 位纯数字自动归一化为后 10 位"),
            // entity.bommaterialcostitem.productcode
            new TranslationSeedItem("entity.bommaterialcostitem.productcode", "ja-JP", "产品编码_jp", "产品编码（父件物料编码，选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）；导入时 18 位纯数字自动归一化为后 10 位"),
            // entity.bommaterialcostitem.productcode
            new TranslationSeedItem("entity.bommaterialcostitem.productcode", "zh-CN", "产品编码", "产品编码（父件物料编码，选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）；导入时 18 位纯数字自动归一化为后 10 位"),
            // entity.bommaterialcostitem.productcode
            new TranslationSeedItem("entity.bommaterialcostitem.productcode", "zh-HK", "产品编码_hk", "产品编码（父件物料编码，选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）；导入时 18 位纯数字自动归一化为后 10 位"),

            // entity.bommaterialcostitem.linenumber
            new TranslationSeedItem("entity.bommaterialcostitem.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.bommaterialcostitem.linenumber
            new TranslationSeedItem("entity.bommaterialcostitem.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.bommaterialcostitem.linenumber
            new TranslationSeedItem("entity.bommaterialcostitem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.bommaterialcostitem.linenumber
            new TranslationSeedItem("entity.bommaterialcostitem.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.bommaterialcostitem.productdescription
            new TranslationSeedItem("entity.bommaterialcostitem.productdescription", "en-US", "产品描述_us", "产品描述"),
            // entity.bommaterialcostitem.productdescription
            new TranslationSeedItem("entity.bommaterialcostitem.productdescription", "ja-JP", "产品描述_jp", "产品描述"),
            // entity.bommaterialcostitem.productdescription
            new TranslationSeedItem("entity.bommaterialcostitem.productdescription", "zh-CN", "产品描述", "产品描述"),
            // entity.bommaterialcostitem.productdescription
            new TranslationSeedItem("entity.bommaterialcostitem.productdescription", "zh-HK", "产品描述_hk", "产品描述"),

            // entity.bommaterialcostitem.componentcode
            new TranslationSeedItem("entity.bommaterialcostitem.componentcode", "en-US", "组件编码_us", "组件编码（子件物料编码，选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）；导入时 18 位纯数字自动归一化为后 10 位"),
            // entity.bommaterialcostitem.componentcode
            new TranslationSeedItem("entity.bommaterialcostitem.componentcode", "ja-JP", "组件编码_jp", "组件编码（子件物料编码，选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）；导入时 18 位纯数字自动归一化为后 10 位"),
            // entity.bommaterialcostitem.componentcode
            new TranslationSeedItem("entity.bommaterialcostitem.componentcode", "zh-CN", "组件编码", "组件编码（子件物料编码，选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）；导入时 18 位纯数字自动归一化为后 10 位"),
            // entity.bommaterialcostitem.componentcode
            new TranslationSeedItem("entity.bommaterialcostitem.componentcode", "zh-HK", "组件编码_hk", "组件编码（子件物料编码，选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）；导入时 18 位纯数字自动归一化为后 10 位"),

            // entity.bommaterialcostitem.componentdescription
            new TranslationSeedItem("entity.bommaterialcostitem.componentdescription", "en-US", "组件描述_us", "组件描述"),
            // entity.bommaterialcostitem.componentdescription
            new TranslationSeedItem("entity.bommaterialcostitem.componentdescription", "ja-JP", "组件描述_jp", "组件描述"),
            // entity.bommaterialcostitem.componentdescription
            new TranslationSeedItem("entity.bommaterialcostitem.componentdescription", "zh-CN", "组件描述", "组件描述"),
            // entity.bommaterialcostitem.componentdescription
            new TranslationSeedItem("entity.bommaterialcostitem.componentdescription", "zh-HK", "组件描述_hk", "组件描述"),

            // entity.bommaterialcostitem.componentquantity
            new TranslationSeedItem("entity.bommaterialcostitem.componentquantity", "en-US", "组件数量_us", "组件数量"),
            // entity.bommaterialcostitem.componentquantity
            new TranslationSeedItem("entity.bommaterialcostitem.componentquantity", "ja-JP", "组件数量_jp", "组件数量"),
            // entity.bommaterialcostitem.componentquantity
            new TranslationSeedItem("entity.bommaterialcostitem.componentquantity", "zh-CN", "组件数量", "组件数量"),
            // entity.bommaterialcostitem.componentquantity
            new TranslationSeedItem("entity.bommaterialcostitem.componentquantity", "zh-HK", "组件数量_hk", "组件数量"),

            // entity.bommaterialcostitem.batchindicator
            new TranslationSeedItem("entity.bommaterialcostitem.batchindicator", "en-US", "批量标识_us", "批量标识（空或 X）"),
            // entity.bommaterialcostitem.batchindicator
            new TranslationSeedItem("entity.bommaterialcostitem.batchindicator", "ja-JP", "批量标识_jp", "批量标识（空或 X）"),
            // entity.bommaterialcostitem.batchindicator
            new TranslationSeedItem("entity.bommaterialcostitem.batchindicator", "zh-CN", "批量标识", "批量标识（空或 X）"),
            // entity.bommaterialcostitem.batchindicator
            new TranslationSeedItem("entity.bommaterialcostitem.batchindicator", "zh-HK", "批量标识_hk", "批量标识（空或 X）"),

            // entity.bommaterialcostitem.productionrelated
            new TranslationSeedItem("entity.bommaterialcostitem.productionrelated", "en-US", "生产相关_us", "生产相关（空或 X）"),
            // entity.bommaterialcostitem.productionrelated
            new TranslationSeedItem("entity.bommaterialcostitem.productionrelated", "ja-JP", "生产相关_jp", "生产相关（空或 X）"),
            // entity.bommaterialcostitem.productionrelated
            new TranslationSeedItem("entity.bommaterialcostitem.productionrelated", "zh-CN", "生产相关", "生产相关（空或 X）"),
            // entity.bommaterialcostitem.productionrelated
            new TranslationSeedItem("entity.bommaterialcostitem.productionrelated", "zh-HK", "生产相关_hk", "生产相关（空或 X）"),

            // entity.bommaterialcostitem.pcbsectindicator
            new TranslationSeedItem("entity.bommaterialcostitem.pcbsectindicator", "en-US", "PCB SECT标识_us", "PCB SECT 标识（空才可参与成本合计与零价清单；非空一律排除；打标写入 X）"),
            // entity.bommaterialcostitem.pcbsectindicator
            new TranslationSeedItem("entity.bommaterialcostitem.pcbsectindicator", "ja-JP", "PCB SECT标识_jp", "PCB SECT 标识（空才可参与成本合计与零价清单；非空一律排除；打标写入 X）"),
            // entity.bommaterialcostitem.pcbsectindicator
            new TranslationSeedItem("entity.bommaterialcostitem.pcbsectindicator", "zh-CN", "PCB SECT标识", "PCB SECT 标识（空才可参与成本合计与零价清单；非空一律排除；打标写入 X）"),
            // entity.bommaterialcostitem.pcbsectindicator
            new TranslationSeedItem("entity.bommaterialcostitem.pcbsectindicator", "zh-HK", "PCB SECT标识_hk", "PCB SECT 标识（空才可参与成本合计与零价清单；非空一律排除；打标写入 X）"),

            // entity.bommaterialcostitem.purchasetype
            new TranslationSeedItem("entity.bommaterialcostitem.purchasetype", "en-US", "采购类型_us", "采购类型（F=外部采购，E=自制生产）；仅生产相关=X、PCB SECT 标识为空且 F 行参与产品 BOM 材料成本汇总，行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数"),
            // entity.bommaterialcostitem.purchasetype
            new TranslationSeedItem("entity.bommaterialcostitem.purchasetype", "ja-JP", "采购类型_jp", "采购类型（F=外部采购，E=自制生产）；仅生产相关=X、PCB SECT 标识为空且 F 行参与产品 BOM 材料成本汇总，行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数"),
            // entity.bommaterialcostitem.purchasetype
            new TranslationSeedItem("entity.bommaterialcostitem.purchasetype", "zh-CN", "采购类型", "采购类型（F=外部采购，E=自制生产）；仅生产相关=X、PCB SECT 标识为空且 F 行参与产品 BOM 材料成本汇总，行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数"),
            // entity.bommaterialcostitem.purchasetype
            new TranslationSeedItem("entity.bommaterialcostitem.purchasetype", "zh-HK", "采购类型_hk", "采购类型（F=外部采购，E=自制生产）；仅生产相关=X、PCB SECT 标识为空且 F 行参与产品 BOM 材料成本汇总，行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数"),

            // entity.bommaterialcostitem.specialprocurementtype
            new TranslationSeedItem("entity.bommaterialcostitem.specialprocurementtype", "en-US", "特殊采购类_us", "特殊采购类（空或业务码，最长 50）"),
            // entity.bommaterialcostitem.specialprocurementtype
            new TranslationSeedItem("entity.bommaterialcostitem.specialprocurementtype", "ja-JP", "特殊采购类_jp", "特殊采购类（空或业务码，最长 50）"),
            // entity.bommaterialcostitem.specialprocurementtype
            new TranslationSeedItem("entity.bommaterialcostitem.specialprocurementtype", "zh-CN", "特殊采购类", "特殊采购类（空或业务码，最长 50）"),
            // entity.bommaterialcostitem.specialprocurementtype
            new TranslationSeedItem("entity.bommaterialcostitem.specialprocurementtype", "zh-HK", "特殊采购类_hk", "特殊采购类（空或业务码，最长 50）"),

            // entity.bommaterialcostitem.profitcentercode
            new TranslationSeedItem("entity.bommaterialcostitem.profitcentercode", "en-US", "利润中心_us", "利润中心（选项 TaktProfitCenters/options；DictValue=Id）"),
            // entity.bommaterialcostitem.profitcentercode
            new TranslationSeedItem("entity.bommaterialcostitem.profitcentercode", "ja-JP", "利润中心_jp", "利润中心（选项 TaktProfitCenters/options；DictValue=Id）"),
            // entity.bommaterialcostitem.profitcentercode
            new TranslationSeedItem("entity.bommaterialcostitem.profitcentercode", "zh-CN", "利润中心", "利润中心（选项 TaktProfitCenters/options；DictValue=Id）"),
            // entity.bommaterialcostitem.profitcentercode
            new TranslationSeedItem("entity.bommaterialcostitem.profitcentercode", "zh-HK", "利润中心_hk", "利润中心（选项 TaktProfitCenters/options；DictValue=Id）"),

            // entity.bommaterialcostitem.movingaverageprice
            new TranslationSeedItem("entity.bommaterialcostitem.movingaverageprice", "en-US", "移动平均价_us", "移动平均价（5 位小数；回填口径=工厂+核算月+组件→全部产品明细强制写回）"),
            // entity.bommaterialcostitem.movingaverageprice
            new TranslationSeedItem("entity.bommaterialcostitem.movingaverageprice", "ja-JP", "移动平均价_jp", "移动平均价（5 位小数；回填口径=工厂+核算月+组件→全部产品明细强制写回）"),
            // entity.bommaterialcostitem.movingaverageprice
            new TranslationSeedItem("entity.bommaterialcostitem.movingaverageprice", "zh-CN", "移动平均价", "移动平均价（5 位小数；回填口径=工厂+核算月+组件→全部产品明细强制写回）"),
            // entity.bommaterialcostitem.movingaverageprice
            new TranslationSeedItem("entity.bommaterialcostitem.movingaverageprice", "zh-HK", "移动平均价_hk", "移动平均价（5 位小数；回填口径=工厂+核算月+组件→全部产品明细强制写回）"),

            // entity.bommaterialcostitem.movingpriceunit
            new TranslationSeedItem("entity.bommaterialcostitem.movingpriceunit", "en-US", "移动价格单位_us", "移动价格单位"),
            // entity.bommaterialcostitem.movingpriceunit
            new TranslationSeedItem("entity.bommaterialcostitem.movingpriceunit", "ja-JP", "移动价格单位_jp", "移动价格单位"),
            // entity.bommaterialcostitem.movingpriceunit
            new TranslationSeedItem("entity.bommaterialcostitem.movingpriceunit", "zh-CN", "移动价格单位", "移动价格单位"),
            // entity.bommaterialcostitem.movingpriceunit
            new TranslationSeedItem("entity.bommaterialcostitem.movingpriceunit", "zh-HK", "移动价格单位_hk", "移动价格单位"),

            // entity.bommaterialcostitem.movingpricecurrencycode
            new TranslationSeedItem("entity.bommaterialcostitem.movingpricecurrencycode", "en-US", "移动价格货币_us", "移动价格货币（字典 accounting_financial_currency_code；如 CNY/USD）"),
            // entity.bommaterialcostitem.movingpricecurrencycode
            new TranslationSeedItem("entity.bommaterialcostitem.movingpricecurrencycode", "ja-JP", "移动价格货币_jp", "移动价格货币（字典 accounting_financial_currency_code；如 CNY/USD）"),
            // entity.bommaterialcostitem.movingpricecurrencycode
            new TranslationSeedItem("entity.bommaterialcostitem.movingpricecurrencycode", "zh-CN", "移动价格货币", "移动价格货币（字典 accounting_financial_currency_code；如 CNY/USD）"),
            // entity.bommaterialcostitem.movingpricecurrencycode
            new TranslationSeedItem("entity.bommaterialcostitem.movingpricecurrencycode", "zh-HK", "移动价格货币_hk", "移动价格货币（字典 accounting_financial_currency_code；如 CNY/USD）"),

            // entity.bommaterialcostitem.purchaseorganization
            new TranslationSeedItem("entity.bommaterialcostitem.purchaseorganization", "en-US", "采购组织_us", "采购组织"),
            // entity.bommaterialcostitem.purchaseorganization
            new TranslationSeedItem("entity.bommaterialcostitem.purchaseorganization", "ja-JP", "采购组织_jp", "采购组织"),
            // entity.bommaterialcostitem.purchaseorganization
            new TranslationSeedItem("entity.bommaterialcostitem.purchaseorganization", "zh-CN", "采购组织", "采购组织"),
            // entity.bommaterialcostitem.purchaseorganization
            new TranslationSeedItem("entity.bommaterialcostitem.purchaseorganization", "zh-HK", "采购组织_hk", "采购组织"),

            // entity.bommaterialcostitem.purchasegroup
            new TranslationSeedItem("entity.bommaterialcostitem.purchasegroup", "en-US", "采购组_us", "采购组（选项 TaktPurchaseGroups/options；DictValue=Id）"),
            // entity.bommaterialcostitem.purchasegroup
            new TranslationSeedItem("entity.bommaterialcostitem.purchasegroup", "ja-JP", "采购组_jp", "采购组（选项 TaktPurchaseGroups/options；DictValue=Id）"),
            // entity.bommaterialcostitem.purchasegroup
            new TranslationSeedItem("entity.bommaterialcostitem.purchasegroup", "zh-CN", "采购组", "采购组（选项 TaktPurchaseGroups/options；DictValue=Id）"),
            // entity.bommaterialcostitem.purchasegroup
            new TranslationSeedItem("entity.bommaterialcostitem.purchasegroup", "zh-HK", "采购组_hk", "采购组（选项 TaktPurchaseGroups/options；DictValue=Id）"),

            // entity.bommaterialcostitem.suppliercode
            new TranslationSeedItem("entity.bommaterialcostitem.suppliercode", "en-US", "供应商_us", "供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.bommaterialcostitem.suppliercode
            new TranslationSeedItem("entity.bommaterialcostitem.suppliercode", "ja-JP", "供应商_jp", "供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.bommaterialcostitem.suppliercode
            new TranslationSeedItem("entity.bommaterialcostitem.suppliercode", "zh-CN", "供应商", "供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.bommaterialcostitem.suppliercode
            new TranslationSeedItem("entity.bommaterialcostitem.suppliercode", "zh-HK", "供应商_hk", "供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）"),

            // entity.bommaterialcostitem.netpurchaseprice
            new TranslationSeedItem("entity.bommaterialcostitem.netpurchaseprice", "en-US", "净价_us", "净价（采购价格，5 位小数）"),
            // entity.bommaterialcostitem.netpurchaseprice
            new TranslationSeedItem("entity.bommaterialcostitem.netpurchaseprice", "ja-JP", "净价_jp", "净价（采购价格，5 位小数）"),
            // entity.bommaterialcostitem.netpurchaseprice
            new TranslationSeedItem("entity.bommaterialcostitem.netpurchaseprice", "zh-CN", "净价", "净价（采购价格，5 位小数）"),
            // entity.bommaterialcostitem.netpurchaseprice
            new TranslationSeedItem("entity.bommaterialcostitem.netpurchaseprice", "zh-HK", "净价_hk", "净价（采购价格，5 位小数）"),

            // entity.bommaterialcostitem.purchasepriceunit
            new TranslationSeedItem("entity.bommaterialcostitem.purchasepriceunit", "en-US", "采购价格单位_us", "采购价格单位"),
            // entity.bommaterialcostitem.purchasepriceunit
            new TranslationSeedItem("entity.bommaterialcostitem.purchasepriceunit", "ja-JP", "采购价格单位_jp", "采购价格单位"),
            // entity.bommaterialcostitem.purchasepriceunit
            new TranslationSeedItem("entity.bommaterialcostitem.purchasepriceunit", "zh-CN", "采购价格单位", "采购价格单位"),
            // entity.bommaterialcostitem.purchasepriceunit
            new TranslationSeedItem("entity.bommaterialcostitem.purchasepriceunit", "zh-HK", "采购价格单位_hk", "采购价格单位"),

            // entity.bommaterialcostitem.purchasecurrencycode
            new TranslationSeedItem("entity.bommaterialcostitem.purchasecurrencycode", "en-US", "采购货币_us", "采购货币（字典 accounting_financial_currency_code；如 CNY/USD）"),
            // entity.bommaterialcostitem.purchasecurrencycode
            new TranslationSeedItem("entity.bommaterialcostitem.purchasecurrencycode", "ja-JP", "采购货币_jp", "采购货币（字典 accounting_financial_currency_code；如 CNY/USD）"),
            // entity.bommaterialcostitem.purchasecurrencycode
            new TranslationSeedItem("entity.bommaterialcostitem.purchasecurrencycode", "zh-CN", "采购货币", "采购货币（字典 accounting_financial_currency_code；如 CNY/USD）"),
            // entity.bommaterialcostitem.purchasecurrencycode
            new TranslationSeedItem("entity.bommaterialcostitem.purchasecurrencycode", "zh-HK", "采购货币_hk", "采购货币（字典 accounting_financial_currency_code；如 CNY/USD）"),

            // entity.bommaterialcostitem.costingdate
            new TranslationSeedItem("entity.bommaterialcostitem.costingdate", "en-US", "核算日期_us", "核算日期"),
            // entity.bommaterialcostitem.costingdate
            new TranslationSeedItem("entity.bommaterialcostitem.costingdate", "ja-JP", "核算日期_jp", "核算日期"),
            // entity.bommaterialcostitem.costingdate
            new TranslationSeedItem("entity.bommaterialcostitem.costingdate", "zh-CN", "核算日期", "核算日期"),
            // entity.bommaterialcostitem.costingdate
            new TranslationSeedItem("entity.bommaterialcostitem.costingdate", "zh-HK", "核算日期_hk", "核算日期"),
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
