// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Procurement
// 文件名称：TaktPurchaseForecastI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPurchaseForecast 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Procurement;

/// <summary>
/// TaktPurchaseForecast 实体国际化翻译种子（键前缀 entity.purchaseforecast.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPurchaseForecastI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPurchaseForecast 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 purchaseforecast 实体翻译...", tenantCode);

        foreach (var item in GetPurchaseForecastTranslations())
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

        TaktLogger.Information("TaktPurchaseForecast 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPurchaseForecast 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.purchaseforecast._self / entity.purchaseforecast.{{field}}；ResourceGroup=Procurement；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPurchaseForecastTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.purchaseforecast._self
            new TranslationSeedItem("entity.purchaseforecast._self", "en-US", "Purchase Forecast Information_us", "实体名称"),
            // entity.purchaseforecast._self
            new TranslationSeedItem("entity.purchaseforecast._self", "ja-JP", "Takt采购预测信息_jp", "实体名称"),
            // entity.purchaseforecast._self
            new TranslationSeedItem("entity.purchaseforecast._self", "zh-CN", "Takt采购预测信息", "实体名称"),
            // entity.purchaseforecast._self
            new TranslationSeedItem("entity.purchaseforecast._self", "zh-HK", "Takt采购预测信息_hk", "实体名称"),

            // entity.purchaseforecast.code
            new TranslationSeedItem("entity.purchaseforecast.code", "en-US", "采购预测编码_us", "采购预测编码（租户+公司+工厂内与发出版本号组合业务唯一）"),
            // entity.purchaseforecast.code
            new TranslationSeedItem("entity.purchaseforecast.code", "ja-JP", "采购预测编码_jp", "采购预测编码（租户+公司+工厂内与发出版本号组合业务唯一）"),
            // entity.purchaseforecast.code
            new TranslationSeedItem("entity.purchaseforecast.code", "zh-CN", "采购预测编码", "采购预测编码（租户+公司+工厂内与发出版本号组合业务唯一）"),
            // entity.purchaseforecast.code
            new TranslationSeedItem("entity.purchaseforecast.code", "zh-HK", "采购预测编码_hk", "采购预测编码（租户+公司+工厂内与发出版本号组合业务唯一）"),

            // entity.purchaseforecast.plandate
            new TranslationSeedItem("entity.purchaseforecast.plandate", "en-US", "计划编制日期_us", "计划编制日期（业务计划日；与发出日期分离）"),
            // entity.purchaseforecast.plandate
            new TranslationSeedItem("entity.purchaseforecast.plandate", "ja-JP", "计划编制日期_jp", "计划编制日期（业务计划日；与发出日期分离）"),
            // entity.purchaseforecast.plandate
            new TranslationSeedItem("entity.purchaseforecast.plandate", "zh-CN", "计划编制日期", "计划编制日期（业务计划日；与发出日期分离）"),
            // entity.purchaseforecast.plandate
            new TranslationSeedItem("entity.purchaseforecast.plandate", "zh-HK", "计划编制日期_hk", "计划编制日期（业务计划日；与发出日期分离）"),

            // entity.purchaseforecast.senddate
            new TranslationSeedItem("entity.purchaseforecast.senddate", "en-US", "发出日期_us", "发出日期（我方将该版采购预测发给供应商的日期；对应销售预测的接收日期）"),
            // entity.purchaseforecast.senddate
            new TranslationSeedItem("entity.purchaseforecast.senddate", "ja-JP", "发出日期_jp", "发出日期（我方将该版采购预测发给供应商的日期；对应销售预测的接收日期）"),
            // entity.purchaseforecast.senddate
            new TranslationSeedItem("entity.purchaseforecast.senddate", "zh-CN", "发出日期", "发出日期（我方将该版采购预测发给供应商的日期；对应销售预测的接收日期）"),
            // entity.purchaseforecast.senddate
            new TranslationSeedItem("entity.purchaseforecast.senddate", "zh-HK", "发出日期_hk", "发出日期（我方将该版采购预测发给供应商的日期；对应销售预测的接收日期）"),

            // entity.purchaseforecast.sendversionno
            new TranslationSeedItem("entity.purchaseforecast.sendversionno", "en-US", "发出版本号_us", "发出版本号（同工厂+预测编码下递增；从 1 起；对应销售预测的接收版本号）"),
            // entity.purchaseforecast.sendversionno
            new TranslationSeedItem("entity.purchaseforecast.sendversionno", "ja-JP", "发出版本号_jp", "发出版本号（同工厂+预测编码下递增；从 1 起；对应销售预测的接收版本号）"),
            // entity.purchaseforecast.sendversionno
            new TranslationSeedItem("entity.purchaseforecast.sendversionno", "zh-CN", "发出版本号", "发出版本号（同工厂+预测编码下递增；从 1 起；对应销售预测的接收版本号）"),
            // entity.purchaseforecast.sendversionno
            new TranslationSeedItem("entity.purchaseforecast.sendversionno", "zh-HK", "发出版本号_hk", "发出版本号（同工厂+预测编码下递增；从 1 起；对应销售预测的接收版本号）"),

            // entity.purchaseforecast.salesproduct
            new TranslationSeedItem("entity.purchaseforecast.salesproduct", "en-US", "产品_us", "产品（四阶第 1 层；仅允许固定字面量 Product，长度固定 7；服务层写入强制覆盖）"),
            // entity.purchaseforecast.salesproduct
            new TranslationSeedItem("entity.purchaseforecast.salesproduct", "ja-JP", "产品_jp", "产品（四阶第 1 层；仅允许固定字面量 Product，长度固定 7；服务层写入强制覆盖）"),
            // entity.purchaseforecast.salesproduct
            new TranslationSeedItem("entity.purchaseforecast.salesproduct", "zh-CN", "产品", "产品（四阶第 1 层；仅允许固定字面量 Product，长度固定 7；服务层写入强制覆盖）"),
            // entity.purchaseforecast.salesproduct
            new TranslationSeedItem("entity.purchaseforecast.salesproduct", "zh-HK", "产品_hk", "产品（四阶第 1 层；仅允许固定字面量 Product，长度固定 7；服务层写入强制覆盖）"),

            // entity.purchaseforecast.productcategorycode
            new TranslationSeedItem("entity.purchaseforecast.productcategorycode", "en-US", "产品类别_us", "产品类别（字典 logistics_manufacturing_mds_product_category；DictValue=CAD/ISD/PAD；四阶第 2 层）"),
            // entity.purchaseforecast.productcategorycode
            new TranslationSeedItem("entity.purchaseforecast.productcategorycode", "ja-JP", "产品类别_jp", "产品类别（字典 logistics_manufacturing_mds_product_category；DictValue=CAD/ISD/PAD；四阶第 2 层）"),
            // entity.purchaseforecast.productcategorycode
            new TranslationSeedItem("entity.purchaseforecast.productcategorycode", "zh-CN", "产品类别", "产品类别（字典 logistics_manufacturing_mds_product_category；DictValue=CAD/ISD/PAD；四阶第 2 层）"),
            // entity.purchaseforecast.productcategorycode
            new TranslationSeedItem("entity.purchaseforecast.productcategorycode", "zh-HK", "产品类别_hk", "产品类别（字典 logistics_manufacturing_mds_product_category；DictValue=CAD/ISD/PAD；四阶第 2 层）"),

            // entity.purchaseforecast.profitcentercode
            new TranslationSeedItem("entity.purchaseforecast.profitcentercode", "en-US", "利润中心_us", "利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode；四阶第 3 层）"),
            // entity.purchaseforecast.profitcentercode
            new TranslationSeedItem("entity.purchaseforecast.profitcentercode", "ja-JP", "利润中心_jp", "利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode；四阶第 3 层）"),
            // entity.purchaseforecast.profitcentercode
            new TranslationSeedItem("entity.purchaseforecast.profitcentercode", "zh-CN", "利润中心", "利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode；四阶第 3 层）"),
            // entity.purchaseforecast.profitcentercode
            new TranslationSeedItem("entity.purchaseforecast.profitcentercode", "zh-HK", "利润中心_hk", "利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode；四阶第 3 层）"),

            // entity.purchaseforecast.modelcode
            new TranslationSeedItem("entity.purchaseforecast.modelcode", "en-US", "机种编码_us", "机种编码（关联 TaktModelDestination.ModelCode；四阶第 4 层）"),
            // entity.purchaseforecast.modelcode
            new TranslationSeedItem("entity.purchaseforecast.modelcode", "ja-JP", "机种编码_jp", "机种编码（关联 TaktModelDestination.ModelCode；四阶第 4 层）"),
            // entity.purchaseforecast.modelcode
            new TranslationSeedItem("entity.purchaseforecast.modelcode", "zh-CN", "机种编码", "机种编码（关联 TaktModelDestination.ModelCode；四阶第 4 层）"),
            // entity.purchaseforecast.modelcode
            new TranslationSeedItem("entity.purchaseforecast.modelcode", "zh-HK", "机种编码_hk", "机种编码（关联 TaktModelDestination.ModelCode；四阶第 4 层）"),

            // entity.purchaseforecast.materialcode
            new TranslationSeedItem("entity.purchaseforecast.materialcode", "en-US", "物料编码_us", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode；具体 SKU）"),
            // entity.purchaseforecast.materialcode
            new TranslationSeedItem("entity.purchaseforecast.materialcode", "ja-JP", "物料编码_jp", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode；具体 SKU）"),
            // entity.purchaseforecast.materialcode
            new TranslationSeedItem("entity.purchaseforecast.materialcode", "zh-CN", "物料编码", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode；具体 SKU）"),
            // entity.purchaseforecast.materialcode
            new TranslationSeedItem("entity.purchaseforecast.materialcode", "zh-HK", "物料编码_hk", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode；具体 SKU）"),

            // entity.purchaseforecast.materialdescription
            new TranslationSeedItem("entity.purchaseforecast.materialdescription", "en-US", "物料描述_us", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.purchaseforecast.materialdescription
            new TranslationSeedItem("entity.purchaseforecast.materialdescription", "ja-JP", "物料描述_jp", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.purchaseforecast.materialdescription
            new TranslationSeedItem("entity.purchaseforecast.materialdescription", "zh-CN", "物料描述", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.purchaseforecast.materialdescription
            new TranslationSeedItem("entity.purchaseforecast.materialdescription", "zh-HK", "物料描述_hk", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),

            // entity.purchaseforecast.suppliercode
            new TranslationSeedItem("entity.purchaseforecast.suppliercode", "en-US", "供应商编码_us", "供应商编码（选项 TaktSuppliers/options；汇总计划时可为空，DictValue=SupplierCode）"),
            // entity.purchaseforecast.suppliercode
            new TranslationSeedItem("entity.purchaseforecast.suppliercode", "ja-JP", "供应商编码_jp", "供应商编码（选项 TaktSuppliers/options；汇总计划时可为空，DictValue=SupplierCode）"),
            // entity.purchaseforecast.suppliercode
            new TranslationSeedItem("entity.purchaseforecast.suppliercode", "zh-CN", "供应商编码", "供应商编码（选项 TaktSuppliers/options；汇总计划时可为空，DictValue=SupplierCode）"),
            // entity.purchaseforecast.suppliercode
            new TranslationSeedItem("entity.purchaseforecast.suppliercode", "zh-HK", "供应商编码_hk", "供应商编码（选项 TaktSuppliers/options；汇总计划时可为空，DictValue=SupplierCode）"),

            // entity.purchaseforecast.suppliername1
            new TranslationSeedItem("entity.purchaseforecast.suppliername1", "en-US", "供应商名称1_us", "供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）"),
            // entity.purchaseforecast.suppliername1
            new TranslationSeedItem("entity.purchaseforecast.suppliername1", "ja-JP", "供应商名称1_jp", "供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）"),
            // entity.purchaseforecast.suppliername1
            new TranslationSeedItem("entity.purchaseforecast.suppliername1", "zh-CN", "供应商名称1", "供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）"),
            // entity.purchaseforecast.suppliername1
            new TranslationSeedItem("entity.purchaseforecast.suppliername1", "zh-HK", "供应商名称1_hk", "供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）"),

            // entity.purchaseforecast.plannergroupcode
            new TranslationSeedItem("entity.purchaseforecast.plannergroupcode", "en-US", "计划组编码_us", "计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）"),
            // entity.purchaseforecast.plannergroupcode
            new TranslationSeedItem("entity.purchaseforecast.plannergroupcode", "ja-JP", "计划组编码_jp", "计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）"),
            // entity.purchaseforecast.plannergroupcode
            new TranslationSeedItem("entity.purchaseforecast.plannergroupcode", "zh-CN", "计划组编码", "计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）"),
            // entity.purchaseforecast.plannergroupcode
            new TranslationSeedItem("entity.purchaseforecast.plannergroupcode", "zh-HK", "计划组编码_hk", "计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）"),

            // entity.purchaseforecast.plannergroupname
            new TranslationSeedItem("entity.purchaseforecast.plannergroupname", "en-US", "计划组名称_us", "计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）"),
            // entity.purchaseforecast.plannergroupname
            new TranslationSeedItem("entity.purchaseforecast.plannergroupname", "ja-JP", "计划组名称_jp", "计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）"),
            // entity.purchaseforecast.plannergroupname
            new TranslationSeedItem("entity.purchaseforecast.plannergroupname", "zh-CN", "计划组名称", "计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）"),
            // entity.purchaseforecast.plannergroupname
            new TranslationSeedItem("entity.purchaseforecast.plannergroupname", "zh-HK", "计划组名称_hk", "计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）"),

            // entity.purchaseforecast.totalquantity
            new TranslationSeedItem("entity.purchaseforecast.totalquantity", "en-US", "计划总数量_us", "计划总数量（基本单位数量；通常汇总版本 002）"),
            // entity.purchaseforecast.totalquantity
            new TranslationSeedItem("entity.purchaseforecast.totalquantity", "ja-JP", "计划总数量_jp", "计划总数量（基本单位数量；通常汇总版本 002）"),
            // entity.purchaseforecast.totalquantity
            new TranslationSeedItem("entity.purchaseforecast.totalquantity", "zh-CN", "计划总数量", "计划总数量（基本单位数量；通常汇总版本 002）"),
            // entity.purchaseforecast.totalquantity
            new TranslationSeedItem("entity.purchaseforecast.totalquantity", "zh-HK", "计划总数量_hk", "计划总数量（基本单位数量；通常汇总版本 002）"),

            // entity.purchaseforecast.totalamount
            new TranslationSeedItem("entity.purchaseforecast.totalamount", "en-US", "计划总金额_us", "计划总金额"),
            // entity.purchaseforecast.totalamount
            new TranslationSeedItem("entity.purchaseforecast.totalamount", "ja-JP", "计划总金额_jp", "计划总金额"),
            // entity.purchaseforecast.totalamount
            new TranslationSeedItem("entity.purchaseforecast.totalamount", "zh-CN", "计划总金额", "计划总金额"),
            // entity.purchaseforecast.totalamount
            new TranslationSeedItem("entity.purchaseforecast.totalamount", "zh-HK", "计划总金额_hk", "计划总金额"),

            // entity.purchaseforecast.convertedquantity
            new TranslationSeedItem("entity.purchaseforecast.convertedquantity", "en-US", "已转采购数量_us", "已转采购数量（基本单位数量）"),
            // entity.purchaseforecast.convertedquantity
            new TranslationSeedItem("entity.purchaseforecast.convertedquantity", "ja-JP", "已转采购数量_jp", "已转采购数量（基本单位数量）"),
            // entity.purchaseforecast.convertedquantity
            new TranslationSeedItem("entity.purchaseforecast.convertedquantity", "zh-CN", "已转采购数量", "已转采购数量（基本单位数量）"),
            // entity.purchaseforecast.convertedquantity
            new TranslationSeedItem("entity.purchaseforecast.convertedquantity", "zh-HK", "已转采购数量_hk", "已转采购数量（基本单位数量）"),

            // entity.purchaseforecast.convertedamount
            new TranslationSeedItem("entity.purchaseforecast.convertedamount", "en-US", "已转采购金额_us", "已转采购金额"),
            // entity.purchaseforecast.convertedamount
            new TranslationSeedItem("entity.purchaseforecast.convertedamount", "ja-JP", "已转采购金额_jp", "已转采购金额"),
            // entity.purchaseforecast.convertedamount
            new TranslationSeedItem("entity.purchaseforecast.convertedamount", "zh-CN", "已转采购金额", "已转采购金额"),
            // entity.purchaseforecast.convertedamount
            new TranslationSeedItem("entity.purchaseforecast.convertedamount", "zh-HK", "已转采购金额_hk", "已转采购金额"),

            // entity.purchaseforecast.planstatus
            new TranslationSeedItem("entity.purchaseforecast.planstatus", "en-US", "计划状态_us", "计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）"),
            // entity.purchaseforecast.planstatus
            new TranslationSeedItem("entity.purchaseforecast.planstatus", "ja-JP", "计划状态_jp", "计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）"),
            // entity.purchaseforecast.planstatus
            new TranslationSeedItem("entity.purchaseforecast.planstatus", "zh-CN", "计划状态", "计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）"),
            // entity.purchaseforecast.planstatus
            new TranslationSeedItem("entity.purchaseforecast.planstatus", "zh-HK", "计划状态_hk", "计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）"),

            // entity.purchaseforecast.convertedstatus
            new TranslationSeedItem("entity.purchaseforecast.convertedstatus", "en-US", "转换状态_us", "转换状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）"),
            // entity.purchaseforecast.convertedstatus
            new TranslationSeedItem("entity.purchaseforecast.convertedstatus", "ja-JP", "转换状态_jp", "转换状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）"),
            // entity.purchaseforecast.convertedstatus
            new TranslationSeedItem("entity.purchaseforecast.convertedstatus", "zh-CN", "转换状态", "转换状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）"),
            // entity.purchaseforecast.convertedstatus
            new TranslationSeedItem("entity.purchaseforecast.convertedstatus", "zh-HK", "转换状态_hk", "转换状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）"),

            // entity.purchaseforecast.plandescription
            new TranslationSeedItem("entity.purchaseforecast.plandescription", "en-US", "计划说明_us", "计划说明"),
            // entity.purchaseforecast.plandescription
            new TranslationSeedItem("entity.purchaseforecast.plandescription", "ja-JP", "计划说明_jp", "计划说明"),
            // entity.purchaseforecast.plandescription
            new TranslationSeedItem("entity.purchaseforecast.plandescription", "zh-CN", "计划说明", "计划说明"),
            // entity.purchaseforecast.plandescription
            new TranslationSeedItem("entity.purchaseforecast.plandescription", "zh-HK", "计划说明_hk", "计划说明"),

            // entity.purchaseforecast.items
            new TranslationSeedItem("entity.purchaseforecast.items", "en-US", "采购预测明细列表_us", "采购预测明细列表（主子表；一行=财年×月计划量 001/002/增减；维度在主表）"),
            // entity.purchaseforecast.items
            new TranslationSeedItem("entity.purchaseforecast.items", "ja-JP", "采购预测明细列表_jp", "采购预测明细列表（主子表；一行=财年×月计划量 001/002/增减；维度在主表）"),
            // entity.purchaseforecast.items
            new TranslationSeedItem("entity.purchaseforecast.items", "zh-CN", "采购预测明细列表", "采购预测明细列表（主子表；一行=财年×月计划量 001/002/增减；维度在主表）"),
            // entity.purchaseforecast.items
            new TranslationSeedItem("entity.purchaseforecast.items", "zh-HK", "采购预测明细列表_hk", "采购预测明细列表（主子表；一行=财年×月计划量 001/002/增减；维度在主表）"),
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
        translation.ResourceGroup = "Procurement";
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
