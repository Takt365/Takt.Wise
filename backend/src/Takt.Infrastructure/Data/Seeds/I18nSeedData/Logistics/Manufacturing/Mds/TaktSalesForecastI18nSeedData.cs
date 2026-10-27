// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Mds
// 文件名称：TaktSalesForecastI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSalesForecast 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Mds;

/// <summary>
/// TaktSalesForecast 实体国际化翻译种子（键前缀 entity.salesforecast.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSalesForecastI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSalesForecast 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 salesforecast 实体翻译...", tenantCode);

        foreach (var item in GetSalesForecastTranslations())
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

        TaktLogger.Information("TaktSalesForecast 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSalesForecast 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.salesforecast._self / entity.salesforecast.{{field}}；ResourceGroup=Mds；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSalesForecastTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.salesforecast._self
            new TranslationSeedItem("entity.salesforecast._self", "en-US", "Sales Forecast Information_us", "实体名称"),
            // entity.salesforecast._self
            new TranslationSeedItem("entity.salesforecast._self", "ja-JP", "Takt销售预测信息_jp", "实体名称"),
            // entity.salesforecast._self
            new TranslationSeedItem("entity.salesforecast._self", "zh-CN", "Takt销售预测信息", "实体名称"),
            // entity.salesforecast._self
            new TranslationSeedItem("entity.salesforecast._self", "zh-HK", "Takt销售预测信息_hk", "实体名称"),

            // entity.salesforecast.code
            new TranslationSeedItem("entity.salesforecast.code", "en-US", "销售预测编码_us", "销售预测编码（租户+公司+工厂内与接收版本号组合业务唯一；前端表单选择编码规则后自动通过 TaktNumbering 销售预测编码规则生成并展示，非手输；单据类型菜单：销售预测）"),
            // entity.salesforecast.code
            new TranslationSeedItem("entity.salesforecast.code", "ja-JP", "销售预测编码_jp", "销售预测编码（租户+公司+工厂内与接收版本号组合业务唯一；前端表单选择编码规则后自动通过 TaktNumbering 销售预测编码规则生成并展示，非手输；单据类型菜单：销售预测）"),
            // entity.salesforecast.code
            new TranslationSeedItem("entity.salesforecast.code", "zh-CN", "销售预测编码", "销售预测编码（租户+公司+工厂内与接收版本号组合业务唯一；前端表单选择编码规则后自动通过 TaktNumbering 销售预测编码规则生成并展示，非手输；单据类型菜单：销售预测）"),
            // entity.salesforecast.code
            new TranslationSeedItem("entity.salesforecast.code", "zh-HK", "销售预测编码_hk", "销售预测编码（租户+公司+工厂内与接收版本号组合业务唯一；前端表单选择编码规则后自动通过 TaktNumbering 销售预测编码规则生成并展示，非手输；单据类型菜单：销售预测）"),

            // entity.salesforecast.plandate
            new TranslationSeedItem("entity.salesforecast.plandate", "en-US", "计划编制日期_us", "计划编制日期（客户侧业务计划日；与接收日期分离）"),
            // entity.salesforecast.plandate
            new TranslationSeedItem("entity.salesforecast.plandate", "ja-JP", "计划编制日期_jp", "计划编制日期（客户侧业务计划日；与接收日期分离）"),
            // entity.salesforecast.plandate
            new TranslationSeedItem("entity.salesforecast.plandate", "zh-CN", "计划编制日期", "计划编制日期（客户侧业务计划日；与接收日期分离）"),
            // entity.salesforecast.plandate
            new TranslationSeedItem("entity.salesforecast.plandate", "zh-HK", "计划编制日期_hk", "计划编制日期（客户侧业务计划日；与接收日期分离）"),

            // entity.salesforecast.receivedate
            new TranslationSeedItem("entity.salesforecast.receivedate", "en-US", "接收日期_us", "接收日期（我方收到该版客户销售预测的日期）"),
            // entity.salesforecast.receivedate
            new TranslationSeedItem("entity.salesforecast.receivedate", "ja-JP", "接收日期_jp", "接收日期（我方收到该版客户销售预测的日期）"),
            // entity.salesforecast.receivedate
            new TranslationSeedItem("entity.salesforecast.receivedate", "zh-CN", "接收日期", "接收日期（我方收到该版客户销售预测的日期）"),
            // entity.salesforecast.receivedate
            new TranslationSeedItem("entity.salesforecast.receivedate", "zh-HK", "接收日期_hk", "接收日期（我方收到该版客户销售预测的日期）"),

            // entity.salesforecast.receiveversionno
            new TranslationSeedItem("entity.salesforecast.receiveversionno", "en-US", "接收版本号_us", "接收版本号（同工厂+预测编码下递增；从 1 起）"),
            // entity.salesforecast.receiveversionno
            new TranslationSeedItem("entity.salesforecast.receiveversionno", "ja-JP", "接收版本号_jp", "接收版本号（同工厂+预测编码下递增；从 1 起）"),
            // entity.salesforecast.receiveversionno
            new TranslationSeedItem("entity.salesforecast.receiveversionno", "zh-CN", "接收版本号", "接收版本号（同工厂+预测编码下递增；从 1 起）"),
            // entity.salesforecast.receiveversionno
            new TranslationSeedItem("entity.salesforecast.receiveversionno", "zh-HK", "接收版本号_hk", "接收版本号（同工厂+预测编码下递增；从 1 起）"),

            // entity.salesforecast.salesproduct
            new TranslationSeedItem("entity.salesforecast.salesproduct", "en-US", "产品_us", "产品（四阶第 1 层；仅允许固定字面量 Product，长度固定 7；服务层写入强制覆盖）"),
            // entity.salesforecast.salesproduct
            new TranslationSeedItem("entity.salesforecast.salesproduct", "ja-JP", "产品_jp", "产品（四阶第 1 层；仅允许固定字面量 Product，长度固定 7；服务层写入强制覆盖）"),
            // entity.salesforecast.salesproduct
            new TranslationSeedItem("entity.salesforecast.salesproduct", "zh-CN", "产品", "产品（四阶第 1 层；仅允许固定字面量 Product，长度固定 7；服务层写入强制覆盖）"),
            // entity.salesforecast.salesproduct
            new TranslationSeedItem("entity.salesforecast.salesproduct", "zh-HK", "产品_hk", "产品（四阶第 1 层；仅允许固定字面量 Product，长度固定 7；服务层写入强制覆盖）"),

            // entity.salesforecast.productcategorycode
            new TranslationSeedItem("entity.salesforecast.productcategorycode", "en-US", "产品类别_us", "产品类别（字典 logistics_manufacturing_mds_product_category；DictValue=CAD/ISD/PAD；四阶第 2 层）"),
            // entity.salesforecast.productcategorycode
            new TranslationSeedItem("entity.salesforecast.productcategorycode", "ja-JP", "产品类别_jp", "产品类别（字典 logistics_manufacturing_mds_product_category；DictValue=CAD/ISD/PAD；四阶第 2 层）"),
            // entity.salesforecast.productcategorycode
            new TranslationSeedItem("entity.salesforecast.productcategorycode", "zh-CN", "产品类别", "产品类别（字典 logistics_manufacturing_mds_product_category；DictValue=CAD/ISD/PAD；四阶第 2 层）"),
            // entity.salesforecast.productcategorycode
            new TranslationSeedItem("entity.salesforecast.productcategorycode", "zh-HK", "产品类别_hk", "产品类别（字典 logistics_manufacturing_mds_product_category；DictValue=CAD/ISD/PAD；四阶第 2 层）"),

            // entity.salesforecast.profitcentercode
            new TranslationSeedItem("entity.salesforecast.profitcentercode", "en-US", "利润中心_us", "利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode；四阶第 3 层）"),
            // entity.salesforecast.profitcentercode
            new TranslationSeedItem("entity.salesforecast.profitcentercode", "ja-JP", "利润中心_jp", "利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode；四阶第 3 层）"),
            // entity.salesforecast.profitcentercode
            new TranslationSeedItem("entity.salesforecast.profitcentercode", "zh-CN", "利润中心", "利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode；四阶第 3 层）"),
            // entity.salesforecast.profitcentercode
            new TranslationSeedItem("entity.salesforecast.profitcentercode", "zh-HK", "利润中心_hk", "利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode；四阶第 3 层）"),

            // entity.salesforecast.modelcode
            new TranslationSeedItem("entity.salesforecast.modelcode", "en-US", "机种编码_us", "机种编码（关联 TaktModelDestination.ModelCode；四阶第 4 层）"),
            // entity.salesforecast.modelcode
            new TranslationSeedItem("entity.salesforecast.modelcode", "ja-JP", "机种编码_jp", "机种编码（关联 TaktModelDestination.ModelCode；四阶第 4 层）"),
            // entity.salesforecast.modelcode
            new TranslationSeedItem("entity.salesforecast.modelcode", "zh-CN", "机种编码", "机种编码（关联 TaktModelDestination.ModelCode；四阶第 4 层）"),
            // entity.salesforecast.modelcode
            new TranslationSeedItem("entity.salesforecast.modelcode", "zh-HK", "机种编码_hk", "机种编码（关联 TaktModelDestination.ModelCode；四阶第 4 层）"),

            // entity.salesforecast.materialcode
            new TranslationSeedItem("entity.salesforecast.materialcode", "en-US", "物料编码_us", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode；具体 SKU）"),
            // entity.salesforecast.materialcode
            new TranslationSeedItem("entity.salesforecast.materialcode", "ja-JP", "物料编码_jp", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode；具体 SKU）"),
            // entity.salesforecast.materialcode
            new TranslationSeedItem("entity.salesforecast.materialcode", "zh-CN", "物料编码", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode；具体 SKU）"),
            // entity.salesforecast.materialcode
            new TranslationSeedItem("entity.salesforecast.materialcode", "zh-HK", "物料编码_hk", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode；具体 SKU）"),

            // entity.salesforecast.materialdescription
            new TranslationSeedItem("entity.salesforecast.materialdescription", "en-US", "物料描述_us", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.salesforecast.materialdescription
            new TranslationSeedItem("entity.salesforecast.materialdescription", "ja-JP", "物料描述_jp", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.salesforecast.materialdescription
            new TranslationSeedItem("entity.salesforecast.materialdescription", "zh-CN", "物料描述", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.salesforecast.materialdescription
            new TranslationSeedItem("entity.salesforecast.materialdescription", "zh-HK", "物料描述_hk", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),

            // entity.salesforecast.customercode
            new TranslationSeedItem("entity.salesforecast.customercode", "en-US", "客户编码_us", "客户编码（选项 TaktCustomers/options；汇总计划时可为空，DictValue=Id）"),
            // entity.salesforecast.customercode
            new TranslationSeedItem("entity.salesforecast.customercode", "ja-JP", "客户编码_jp", "客户编码（选项 TaktCustomers/options；汇总计划时可为空，DictValue=Id）"),
            // entity.salesforecast.customercode
            new TranslationSeedItem("entity.salesforecast.customercode", "zh-CN", "客户编码", "客户编码（选项 TaktCustomers/options；汇总计划时可为空，DictValue=Id）"),
            // entity.salesforecast.customercode
            new TranslationSeedItem("entity.salesforecast.customercode", "zh-HK", "客户编码_hk", "客户编码（选项 TaktCustomers/options；汇总计划时可为空，DictValue=Id）"),

            // entity.salesforecast.customername1
            new TranslationSeedItem("entity.salesforecast.customername1", "en-US", "客户名称1_us", "客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）"),
            // entity.salesforecast.customername1
            new TranslationSeedItem("entity.salesforecast.customername1", "ja-JP", "客户名称1_jp", "客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）"),
            // entity.salesforecast.customername1
            new TranslationSeedItem("entity.salesforecast.customername1", "zh-CN", "客户名称1", "客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）"),
            // entity.salesforecast.customername1
            new TranslationSeedItem("entity.salesforecast.customername1", "zh-HK", "客户名称1_hk", "客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）"),

            // entity.salesforecast.plannergroupcode
            new TranslationSeedItem("entity.salesforecast.plannergroupcode", "en-US", "计划组编码_us", "计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）"),
            // entity.salesforecast.plannergroupcode
            new TranslationSeedItem("entity.salesforecast.plannergroupcode", "ja-JP", "计划组编码_jp", "计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）"),
            // entity.salesforecast.plannergroupcode
            new TranslationSeedItem("entity.salesforecast.plannergroupcode", "zh-CN", "计划组编码", "计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）"),
            // entity.salesforecast.plannergroupcode
            new TranslationSeedItem("entity.salesforecast.plannergroupcode", "zh-HK", "计划组编码_hk", "计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）"),

            // entity.salesforecast.plannergroupname
            new TranslationSeedItem("entity.salesforecast.plannergroupname", "en-US", "计划组名称_us", "计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）"),
            // entity.salesforecast.plannergroupname
            new TranslationSeedItem("entity.salesforecast.plannergroupname", "ja-JP", "计划组名称_jp", "计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）"),
            // entity.salesforecast.plannergroupname
            new TranslationSeedItem("entity.salesforecast.plannergroupname", "zh-CN", "计划组名称", "计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）"),
            // entity.salesforecast.plannergroupname
            new TranslationSeedItem("entity.salesforecast.plannergroupname", "zh-HK", "计划组名称_hk", "计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）"),

            // entity.salesforecast.totalquantity
            new TranslationSeedItem("entity.salesforecast.totalquantity", "en-US", "计划总数量_us", "计划总数量（基本单位数量；通常汇总版本 002）"),
            // entity.salesforecast.totalquantity
            new TranslationSeedItem("entity.salesforecast.totalquantity", "ja-JP", "计划总数量_jp", "计划总数量（基本单位数量；通常汇总版本 002）"),
            // entity.salesforecast.totalquantity
            new TranslationSeedItem("entity.salesforecast.totalquantity", "zh-CN", "计划总数量", "计划总数量（基本单位数量；通常汇总版本 002）"),
            // entity.salesforecast.totalquantity
            new TranslationSeedItem("entity.salesforecast.totalquantity", "zh-HK", "计划总数量_hk", "计划总数量（基本单位数量；通常汇总版本 002）"),

            // entity.salesforecast.totalamount
            new TranslationSeedItem("entity.salesforecast.totalamount", "en-US", "计划总金额_us", "计划总金额"),
            // entity.salesforecast.totalamount
            new TranslationSeedItem("entity.salesforecast.totalamount", "ja-JP", "计划总金额_jp", "计划总金额"),
            // entity.salesforecast.totalamount
            new TranslationSeedItem("entity.salesforecast.totalamount", "zh-CN", "计划总金额", "计划总金额"),
            // entity.salesforecast.totalamount
            new TranslationSeedItem("entity.salesforecast.totalamount", "zh-HK", "计划总金额_hk", "计划总金额"),

            // entity.salesforecast.convertedquantity
            new TranslationSeedItem("entity.salesforecast.convertedquantity", "en-US", "已转生产销售数量_us", "已转生产/销售数量（基本单位数量）"),
            // entity.salesforecast.convertedquantity
            new TranslationSeedItem("entity.salesforecast.convertedquantity", "ja-JP", "已转生产销售数量_jp", "已转生产/销售数量（基本单位数量）"),
            // entity.salesforecast.convertedquantity
            new TranslationSeedItem("entity.salesforecast.convertedquantity", "zh-CN", "已转生产销售数量", "已转生产/销售数量（基本单位数量）"),
            // entity.salesforecast.convertedquantity
            new TranslationSeedItem("entity.salesforecast.convertedquantity", "zh-HK", "已转生产销售数量_hk", "已转生产/销售数量（基本单位数量）"),

            // entity.salesforecast.convertedamount
            new TranslationSeedItem("entity.salesforecast.convertedamount", "en-US", "已转生产销售金额_us", "已转生产/销售金额"),
            // entity.salesforecast.convertedamount
            new TranslationSeedItem("entity.salesforecast.convertedamount", "ja-JP", "已转生产销售金额_jp", "已转生产/销售金额"),
            // entity.salesforecast.convertedamount
            new TranslationSeedItem("entity.salesforecast.convertedamount", "zh-CN", "已转生产销售金额", "已转生产/销售金额"),
            // entity.salesforecast.convertedamount
            new TranslationSeedItem("entity.salesforecast.convertedamount", "zh-HK", "已转生产销售金额_hk", "已转生产/销售金额"),

            // entity.salesforecast.planstatus
            new TranslationSeedItem("entity.salesforecast.planstatus", "en-US", "计划状态_us", "计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）"),
            // entity.salesforecast.planstatus
            new TranslationSeedItem("entity.salesforecast.planstatus", "ja-JP", "计划状态_jp", "计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）"),
            // entity.salesforecast.planstatus
            new TranslationSeedItem("entity.salesforecast.planstatus", "zh-CN", "计划状态", "计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）"),
            // entity.salesforecast.planstatus
            new TranslationSeedItem("entity.salesforecast.planstatus", "zh-HK", "计划状态_hk", "计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）"),

            // entity.salesforecast.convertedstatus
            new TranslationSeedItem("entity.salesforecast.convertedstatus", "en-US", "转单状态_us", "转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）"),
            // entity.salesforecast.convertedstatus
            new TranslationSeedItem("entity.salesforecast.convertedstatus", "ja-JP", "转单状态_jp", "转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）"),
            // entity.salesforecast.convertedstatus
            new TranslationSeedItem("entity.salesforecast.convertedstatus", "zh-CN", "转单状态", "转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）"),
            // entity.salesforecast.convertedstatus
            new TranslationSeedItem("entity.salesforecast.convertedstatus", "zh-HK", "转单状态_hk", "转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）"),

            // entity.salesforecast.plandescription
            new TranslationSeedItem("entity.salesforecast.plandescription", "en-US", "计划说明_us", "计划说明"),
            // entity.salesforecast.plandescription
            new TranslationSeedItem("entity.salesforecast.plandescription", "ja-JP", "计划说明_jp", "计划说明"),
            // entity.salesforecast.plandescription
            new TranslationSeedItem("entity.salesforecast.plandescription", "zh-CN", "计划说明", "计划说明"),
            // entity.salesforecast.plandescription
            new TranslationSeedItem("entity.salesforecast.plandescription", "zh-HK", "计划说明_hk", "计划说明"),

            // entity.salesforecast.items
            new TranslationSeedItem("entity.salesforecast.items", "en-US", "销售预测明细列表_us", "销售预测明细列表（主子表；一行=财年×月计划量 001/002/增减；维度在主表）"),
            // entity.salesforecast.items
            new TranslationSeedItem("entity.salesforecast.items", "ja-JP", "销售预测明细列表_jp", "销售预测明细列表（主子表；一行=财年×月计划量 001/002/增减；维度在主表）"),
            // entity.salesforecast.items
            new TranslationSeedItem("entity.salesforecast.items", "zh-CN", "销售预测明细列表", "销售预测明细列表（主子表；一行=财年×月计划量 001/002/增减；维度在主表）"),
            // entity.salesforecast.items
            new TranslationSeedItem("entity.salesforecast.items", "zh-HK", "销售预测明细列表_hk", "销售预测明细列表（主子表；一行=财年×月计划量 001/002/增减；维度在主表）"),
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
        translation.ResourceGroup = "Mds";
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
