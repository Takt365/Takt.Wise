// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds
// 文件名称：TaktMenuLevel4SeedData.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt 四级菜单种子数据。
//           在三级菜单已存在的前提下，主要扩展生产制造（BOM/排程/工程设变/产出/OPH工数/生产不良）等更细页面。
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Identity;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// Takt 四级菜单种子数据。
/// <para>
/// 父级通常来自 TaktMenuLevel3SeedData 中的 BOM、排程、工程设变、产出、OPH工数、生产不良等三级节点。
/// 由 TaktMenuSeedData 统一协调调用，不直接注册为 ITaktSeedDataCoordinator。
/// </para>
/// </summary>
public class TaktMenuLevel4SeedData
{
    /// <summary>
    /// 初始化四级菜单种子数据。
    /// <para>
    /// 写入 BOM 子项、排程子项、工程设变相关部门视图、产出/OPH工数/生产不良下的 PCBA/Assembly 目录等。
    /// </para>
    /// </summary>
    /// <param name="serviceProvider">服务提供者，用于解析 ITaktRepository{TaktMenu}。</param>
    /// <param name="specifiedTenantCode">租户编码（由协调器传入）。</param>
    /// <returns>元组：(InsertCount, UpdateCount)，分别为本次新增与更新的四级菜单条数。</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? specifiedTenantCode = null)
    {
        var menuRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktMenu>>();
        var sqlSugarContext = serviceProvider.GetRequiredService<TaktSeedContext>();

        // 四级菜单:基于三级菜单的ParentId
        // 注意:菜单为租户级实体,由协调器指定租户,必须传入租户编码
        if (string.IsNullOrWhiteSpace(specifiedTenantCode))
        {
            TaktLogger.Warning("未指定租户编码,跳过四级菜单种子数据初始化");
            return (0, 0);
        }
        
        var tenantCode = specifiedTenantCode;

        int insertCount = 0;
        int updateCount = 0;

        // 获取三级父菜单(使用仓储查询,自动应用租户过滤)
        // 注意:三级菜单已在 TaktMenuLevel3SeedData 中初始化
        var manufacturingBomMenu = await menuRepository.FirstAsync(m => m.MenuCode == "LOGISTICS_MANUFACTURING_BOM");
        var manufacturingMdsMenu = await menuRepository.FirstAsync(m => m.MenuCode == "LOGISTICS_MANUFACTURING_MDS");
        var manufacturingMpsMenu = await menuRepository.FirstAsync(m => m.MenuCode == "LOGISTICS_MANUFACTURING_MPS");
        var manufacturingMrpMenu = await menuRepository.FirstAsync(m => m.MenuCode == "LOGISTICS_MANUFACTURING_MRP");
        var manufacturingApsMenu = await menuRepository.FirstAsync(m => m.MenuCode == "LOGISTICS_MANUFACTURING_APS");
        var manufacturingEngineeringChangeMenu = await menuRepository.FirstAsync(m => m.MenuCode == "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE");
        var manufacturingOutputMenu = await menuRepository.FirstAsync(m => m.MenuCode == "LOGISTICS_MANUFACTURING_OUTPUT");
        var manufacturingLaborHourMenu = await menuRepository.FirstAsync(m => m.MenuCode == "LOGISTICS_MANUFACTURING_LABOR_HOUR");
        var manufacturingDefectMenu = await menuRepository.FirstAsync(m => m.MenuCode == "LOGISTICS_MANUFACTURING_DEFECT");
        var manufacturingSopMenu = await menuRepository.FirstAsync(m => m.MenuCode == "LOGISTICS_MANUFACTURING_SOP");
        var qualityCostMenu = await menuRepository.FirstAsync(m => m.MenuCode == "LOGISTICS_QUALITY_COST");
        var qualityAssuranceMenu = await menuRepository.FirstAsync(m => m.MenuCode == "LOGISTICS_QUALITY_OPERATION");
        var qualityComplaintMenu = await menuRepository.FirstAsync(m => m.MenuCode == "LOGISTICS_QUALITY_COMPLAINT");

        // ========== BOM 下的四级菜单 ==========
        if (manufacturingBomMenu != null)
        {
            var (insertBOM2, updateBOM2) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_BOM_BILL_OF_MATERIAL", menu =>
            {
                menu.MenuName = "物料清单";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_BOM_BILL_OF_MATERIAL";
                menu.I18nKey = "menu.logistics.manufacturing.bom.bill.of.material";
                menu.Icon = "RiFileList2Line";
                menu.ParentId = manufacturingBomMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:bom:bill:of:material:list";
                menu.RoutePath = "/logistics/manufacturing/bom/bill-of-material";
                menu.ComponentPath = "logistics/manufacturing/bom/bill-of-material/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertBOM2;
            updateCount += updateBOM2;

            var (insertBOM5, updateBOM5) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_BOM_ROUTING", menu =>
            {
                menu.MenuName = "工艺路线";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_BOM_ROUTING";
                menu.I18nKey = "menu.logistics.manufacturing.bom.routing";
                menu.Icon = "RiRouteLine";
                menu.ParentId = manufacturingBomMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:bom:routing:list";
                menu.RoutePath = "/logistics/manufacturing/bom/routing";
                menu.ComponentPath = "logistics/manufacturing/bom/routing/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertBOM5;
            updateCount += updateBOM5;

            var (insertBOM9, updateBOM9) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_BOM_STANDARD_OPERATION_TIME", menu =>
            {
                menu.MenuName = "标准工时";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_BOM_STANDARD_OPERATION_TIME";
                menu.I18nKey = "menu.logistics.manufacturing.bom.standard.operation.time";
                menu.Icon = "RiTimerLine";
                menu.ParentId = manufacturingBomMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:bom:standard:operation:time:list";
                menu.RoutePath = "/logistics/manufacturing/bom/standard-operation-time";
                menu.ComponentPath = "logistics/manufacturing/bom/standard-operation-time/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertBOM9;
            updateCount += updateBOM9;

            // 主子表：菜单挂主表 material-cost（左主右从）；明细无独立导航（对齐 bill-of-material）
            var (insertBOM10, updateBOM10) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_BOM_MATERIAL_COST", menu =>
            {
                menu.MenuName = "材料成本";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_BOM_MATERIAL_COST";
                menu.I18nKey = "menu.logistics.manufacturing.bom.material.cost";
                menu.Icon = "RiCoinLine";
                menu.ParentId = manufacturingBomMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:bom:material:cost:list";
                menu.RoutePath = "/logistics/manufacturing/bom/material-cost";
                menu.ComponentPath = "logistics/manufacturing/bom/material-cost/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertBOM10;
            updateCount += updateBOM10;

            // 零材料成本
            var (insertBOM10z, updateBOM10z) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_BOM_MATERIAL_ZEROPRICE", menu =>
            {
                menu.MenuName = "零材料成本";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_BOM_MATERIAL_ZEROPRICE";
                menu.I18nKey = "menu.logistics.manufacturing.bom.material.zeroprice";
                menu.Icon = "RiPriceTag3Line";
                menu.ParentId = manufacturingBomMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:bom:material:zeroprice:list";
                menu.RoutePath = "/logistics/manufacturing/bom/material-zero-price";
                menu.ComponentPath = "logistics/manufacturing/bom/material-zero-price/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertBOM10z;
            updateCount += updateBOM10z;

            // 机种成本推移：按组件编码合并后核算月单价转置
            var (insertBOM13, updateBOM13) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_BOM_MODEL_COST_TREND", menu =>
            {
                menu.MenuName = "机种成本推移";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_BOM_MODEL_COST_TREND";
                menu.I18nKey = "menu.logistics.manufacturing.bom.model.cost.trend";
                menu.Icon = "RiLineChartLine";
                menu.ParentId = manufacturingBomMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:bom:model:cost:trend:list";
                menu.RoutePath = "/logistics/manufacturing/bom/model-cost-trend";
                menu.ComponentPath = "logistics/manufacturing/bom/model-cost-trend/index";
                menu.SortOrder = 9;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertBOM13;
            updateCount += updateBOM13;

            // 成本差异推移：产品月成本 + 0价格组 + 价格差异组
            var (insertBOM15, updateBOM15) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_BOM_PRICE_DELTA_TREND", menu =>
            {
                menu.MenuName = "成本差异推移";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_BOM_PRICE_DELTA_TREND";
                menu.I18nKey = "menu.logistics.manufacturing.bom.pricedelta.trend";
                menu.Icon = "RiLineChartLine";
                menu.ParentId = manufacturingBomMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:bom:pricedelta:trend:list";
                menu.RoutePath = "/logistics/manufacturing/bom/price-delta-trend";
                menu.ComponentPath = "logistics/manufacturing/bom/price-delta-trend/index";
                menu.SortOrder = 10;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertBOM15;
            updateCount += updateBOM15;

            // 通用物料：期间止月成品中，同一机种各成品用量与单价一致的组件
            var (insertBOM17, updateBOM17) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_BOM_COMMON_MATERIAL", menu =>
            {
                menu.MenuName = "通用物料";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_BOM_COMMON_MATERIAL";
                menu.I18nKey = "menu.logistics.manufacturing.bom.common.material";
                menu.Icon = "RiStackLine";
                menu.ParentId = manufacturingBomMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:bom:common:material:list";
                menu.RoutePath = "/logistics/manufacturing/bom/common-material";
                menu.ComponentPath = "logistics/manufacturing/bom/common-material/index";
                menu.SortOrder = 11;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertBOM17;
            updateCount += updateBOM17;

            // 专用物料：期间止月成品中，同一机种各成品用量或单价不一致的组件
            var (insertBOM18, updateBOM18) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_BOM_DEDICATED_MATERIAL", menu =>
            {
                menu.MenuName = "专用物料";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_BOM_DEDICATED_MATERIAL";
                menu.I18nKey = "menu.logistics.manufacturing.bom.dedicated.material";
                menu.Icon = "RiFocus2Line";
                menu.ParentId = manufacturingBomMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:bom:dedicated:material:list";
                menu.RoutePath = "/logistics/manufacturing/bom/dedicated-material";
                menu.ComponentPath = "logistics/manufacturing/bom/dedicated-material/index";
                menu.SortOrder = 12;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertBOM18;
            updateCount += updateBOM18;

            var (insertBOM16, updateBOM16) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_BOM_GROUP", menu =>
            {
                menu.MenuName = "BOM组";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_BOM_GROUP";
                menu.I18nKey = "menu.logistics.manufacturing.bom.group";
                menu.Icon = "RiGroupLine";
                menu.ParentId = manufacturingBomMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:bom:group:list";
                menu.RoutePath = "/logistics/manufacturing/bom/group";
                menu.ComponentPath = "logistics/manufacturing/bom/group/index";
                menu.SortOrder = 13;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertBOM16;
            updateCount += updateBOM16;
        }

        // ========== MDS计划下的四级菜单 ==========
        if (manufacturingMdsMenu != null)
        {
            var (insertDMD1, updateDMD1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_MDS_SALES_FORECAST", menu =>
            {
                menu.MenuName = "销售预测";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_MDS_SALES_FORECAST";
                menu.I18nKey = "menu.logistics.manufacturing.mds.sales.forecast";
                menu.Icon = "RiLineChartLine";
                menu.ParentId = manufacturingMdsMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:mds:sales:forecast:list";
                menu.RoutePath = "/logistics/manufacturing/mds/sales-forecast";
                menu.ComponentPath = "logistics/manufacturing/mds/sales-forecast/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertDMD1;
            updateCount += updateDMD1;

            var (insertDMD0, updateDMD0) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_MDS_MASTER_DEMAND_SCHEDULE", menu =>
            {
                menu.MenuName = "主需求计划";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_MDS_MASTER_DEMAND_SCHEDULE";
                menu.I18nKey = "menu.logistics.manufacturing.mds.master.demand.schedule";
                menu.Icon = "RiBarChartGroupedLine";
                menu.ParentId = manufacturingMdsMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:mds:master:demand:schedule:list";
                menu.RoutePath = "/logistics/manufacturing/mds/master-demand-schedule";
                menu.ComponentPath = "logistics/manufacturing/mds/master-demand-schedule/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertDMD0;
            updateCount += updateDMD0;
        }

        // ========== MRP计划下的四级菜单 ==========
        if (manufacturingMrpMenu != null)
        {
            var (insertPLN0, updatePLN0) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_MRP_PARAMETER", menu =>
            {
                menu.MenuName = "MRP 参数设置";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_MRP_PARAMETER";
                menu.I18nKey = "menu.logistics.manufacturing.mrp.parameter.setting";
                menu.Icon = "RiSettings3Line";
                menu.ParentId = manufacturingMrpMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:mrp:parameter:list";
                menu.RoutePath = "/logistics/manufacturing/mrp/parameter-setting";
                menu.ComponentPath = "error/coming-soon";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertPLN0;
            updateCount += updatePLN0;

            var (insertPLN0b, updatePLN0b) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_MRP_PERIOD_SCHEME", menu =>
            {
                menu.MenuName = "MRP 周期方案";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_MRP_PERIOD_SCHEME";
                menu.I18nKey = "menu.logistics.manufacturing.mrp.period.scheme";
                menu.Icon = "RiCalendar2Line";
                menu.ParentId = manufacturingMrpMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:mrp:period:scheme:list";
                menu.RoutePath = "/logistics/manufacturing/mrp/period-scheme";
                menu.ComponentPath = "error/coming-soon";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertPLN0b;
            updateCount += updatePLN0b;

            var (insertPLN1, updatePLN1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_MRP_MATERIAL_REQUIREMENTS_PLANNING", menu =>
            {
                menu.MenuName = "MRP运算向导";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_MRP_MATERIAL_REQUIREMENTS_PLANNING";
                menu.I18nKey = "menu.logistics.manufacturing.mrp.run.wizard";
                menu.Icon = "RiGuideLine";
                menu.ParentId = manufacturingMrpMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:mrp:material:requirements:list";
                menu.RoutePath = "/logistics/manufacturing/mrp/material-requirements-planning";
                menu.ComponentPath = "logistics/manufacturing/mrp/material-requirements-planning/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertPLN1;
            updateCount += updatePLN1;

            var (insertPLN1c, updatePLN1c) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_MRP_PLANNED_ORDER", menu =>
            {
                menu.MenuName = "计划订单";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_MRP_PLANNED_ORDER";
                menu.I18nKey = "menu.logistics.manufacturing.mrp.planned.order";
                menu.Icon = "RiFilePaper2Line";
                menu.ParentId = manufacturingMrpMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:mrp:planned:order:list";
                menu.RoutePath = "/logistics/manufacturing/mrp/planned-order";
                menu.ComponentPath = "logistics/manufacturing/aps/planned-order/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertPLN1c;
            updateCount += updatePLN1c;

            var (insertPLN1b, updatePLN1b) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_MRP_SUPPLY_DEMAND_TRACE", menu =>
            {
                menu.MenuName = "供需追溯";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_MRP_SUPPLY_DEMAND_TRACE";
                menu.I18nKey = "menu.logistics.manufacturing.mrp.supply.demand.trace";
                menu.Icon = "RiGitMergeLine";
                menu.ParentId = manufacturingMrpMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:mrp:supply:demand:trace:list";
                menu.RoutePath = "/logistics/manufacturing/mrp/supply-demand-trace";
                menu.ComponentPath = "error/coming-soon";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertPLN1b;
            updateCount += updatePLN1b;

            var (insertPLN2, updatePLN2) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_MRP_PRODUCTION_PLAN", menu =>
            {
                menu.MenuName = "生产计划";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_MRP_PRODUCTION_PLAN";
                menu.I18nKey = "menu.logistics.manufacturing.mrp.production.plan";
                menu.Icon = "RiCalendarCheckLine";
                menu.ParentId = manufacturingMrpMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:mrp:production:plan:list";
                menu.RoutePath = "/logistics/manufacturing/mrp/production-plan";
                menu.ComponentPath = "logistics/manufacturing/mrp/production-plan/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertPLN2;
            updateCount += updatePLN2;

            var (insertPLN3, updatePLN3) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_MRP_PURCHASE_PLAN", menu =>
            {
                menu.MenuName = "采购计划";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_MRP_PURCHASE_PLAN";
                menu.I18nKey = "menu.logistics.manufacturing.mrp.purchase.plan";
                menu.Icon = "RiShoppingCartLine";
                menu.ParentId = manufacturingMrpMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:mrp:purchase:plan:list";
                menu.RoutePath = "/logistics/manufacturing/mrp/purchase-plan";
                menu.ComponentPath = "logistics/manufacturing/mrp/purchase-plan/index";
                menu.SortOrder = 7;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertPLN3;
            updateCount += updatePLN3;

            var (insertPLN4, updatePLN4) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_MRP_HISTORY", menu =>
            {
                menu.MenuName = "MRP 历史记录";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_MRP_HISTORY";
                menu.I18nKey = "menu.logistics.manufacturing.mrp.history";
                menu.Icon = "RiHistoryLine";
                menu.ParentId = manufacturingMrpMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:mrp:history:list";
                menu.RoutePath = "/logistics/manufacturing/mrp/history";
                menu.ComponentPath = "error/coming-soon";
                menu.SortOrder = 8;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertPLN4;
            updateCount += updatePLN4;
        }

        // ========== MPS计划下的四级菜单 ==========
        if (manufacturingMpsMenu != null)
        {
            var (insertMPS0, updateMPS0) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_MPS_PARAMETER", menu =>
            {
                menu.MenuName = "MPS 参数设置";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_MPS_PARAMETER";
                menu.I18nKey = "menu.logistics.manufacturing.mps.parameter.setting";
                menu.Icon = "RiSettings3Line";
                menu.ParentId = manufacturingMpsMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:mps:parameter:list";
                menu.RoutePath = "/logistics/manufacturing/mps/parameter-setting";
                menu.ComponentPath = "error/coming-soon";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertMPS0;
            updateCount += updateMPS0;

            var (insertMPS1, updateMPS1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_MPS_PERIOD_SCHEME", menu =>
            {
                menu.MenuName = "MPS 周期方案";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_MPS_PERIOD_SCHEME";
                menu.I18nKey = "menu.logistics.manufacturing.mps.period.scheme";
                menu.Icon = "RiCalendar2Line";
                menu.ParentId = manufacturingMpsMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:mps:period:scheme:list";
                menu.RoutePath = "/logistics/manufacturing/mps/period-scheme";
                menu.ComponentPath = "error/coming-soon";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertMPS1;
            updateCount += updateMPS1;

            var (insertMPS2, updateMPS2) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_MPS_MASTER_PRODUCTION_SCHEDULE", menu =>
            {
                menu.MenuName = "MPS 计划维护";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_MPS_MASTER_PRODUCTION_SCHEDULE";
                menu.I18nKey = "menu.logistics.manufacturing.mps.plan.maintenance";
                menu.Icon = "RiCalendarTodoLine";
                menu.ParentId = manufacturingMpsMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:mps:master:production:schedule:list";
                menu.RoutePath = "/logistics/manufacturing/mps/master-production-schedule";
                menu.ComponentPath = "logistics/manufacturing/mps/master-production-schedule/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertMPS2;
            updateCount += updateMPS2;

            var (insertMPS3, updateMPS3) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_MPS_RUN_WIZARD", menu =>
            {
                menu.MenuName = "MPS运算向导";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_MPS_RUN_WIZARD";
                menu.I18nKey = "menu.logistics.manufacturing.mps.run.wizard";
                menu.Icon = "RiGuideLine";
                menu.ParentId = manufacturingMpsMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:mps:run:wizard:list";
                menu.RoutePath = "/logistics/manufacturing/mps/run-wizard";
                menu.ComponentPath = "error/coming-soon";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertMPS3;
            updateCount += updateMPS3;

            var (insertMPS5, updateMPS5) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_MPS_ROUGH_CUT_CAPACITY", menu =>
            {
                menu.MenuName = "粗能力计划";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_MPS_ROUGH_CUT_CAPACITY";
                menu.I18nKey = "menu.logistics.manufacturing.mps.rough.cut.capacity";
                menu.Icon = "RiDashboardLine";
                menu.ParentId = manufacturingMpsMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:mps:rough:cut:capacity:list";
                menu.RoutePath = "/logistics/manufacturing/mps/rough-cut-capacity";
                menu.ComponentPath = "error/coming-soon";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertMPS5;
            updateCount += updateMPS5;

            var (insertMPS6, updateMPS6) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_MPS_DETAIL", menu =>
            {
                menu.MenuName = "MPS 明细";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_MPS_DETAIL";
                menu.I18nKey = "menu.logistics.manufacturing.mps.detail";
                menu.Icon = "RiListCheck2";
                menu.ParentId = manufacturingMpsMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:mps:detail:list";
                menu.RoutePath = "/logistics/manufacturing/mps/detail";
                menu.ComponentPath = "error/coming-soon";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertMPS6;
            updateCount += updateMPS6;

            var (insertMPS7, updateMPS7) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_MPS_RELEASE", menu =>
            {
                menu.MenuName = "MPS 下达";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_MPS_RELEASE";
                menu.I18nKey = "menu.logistics.manufacturing.mps.release";
                menu.Icon = "RiSendPlane2Line";
                menu.ParentId = manufacturingMpsMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:mps:release:list";
                menu.RoutePath = "/logistics/manufacturing/mps/release";
                menu.ComponentPath = "error/coming-soon";
                menu.SortOrder = 7;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertMPS7;
            updateCount += updateMPS7;

            var (insertMPS8, updateMPS8) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_MPS_PRODUCTION_TEAM", menu =>
            {
                menu.MenuName = "生产班组";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_MPS_PRODUCTION_TEAM";
                menu.I18nKey = "menu.logistics.manufacturing.mps.production.team";
                menu.Icon = "RiTeamLine";
                menu.ParentId = manufacturingMpsMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:mps:production:team:list";
                menu.RoutePath = "/logistics/manufacturing/mps/production-team";
                menu.ComponentPath = "logistics/manufacturing/mps/production-team/index";
                menu.SortOrder = 8;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertMPS8;
            updateCount += updateMPS8;

            var (insertMPS9, updateMPS9) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_MPS_STANDARD_OPERATION_RATE", menu =>
            {
                menu.MenuName = "标准稼动率";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_MPS_STANDARD_OPERATION_RATE";
                menu.I18nKey = "menu.logistics.manufacturing.mps.standard.operation.rate";
                menu.Icon = "RiBarChartLine";
                menu.ParentId = manufacturingMpsMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:mps:standard:operation:rate:list";
                menu.RoutePath = "/logistics/manufacturing/mps/standard-operation-rate";
                menu.ComponentPath = "logistics/manufacturing/mps/standard-operation-rate/index";
                menu.SortOrder = 9;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertMPS9;
            updateCount += updateMPS9;

            var (insertMPS10, updateMPS10) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_MPS_PERSONNEL_OPERATION_RATE", menu =>
            {
                menu.MenuName = "人员稼动率";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_MPS_PERSONNEL_OPERATION_RATE";
                menu.I18nKey = "menu.logistics.manufacturing.mps.personnel.operation.rate";
                menu.Icon = "RiUserLine";
                menu.ParentId = manufacturingMpsMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:mps:personnel:operation:rate:list";
                menu.RoutePath = "/logistics/manufacturing/mps/personnel-operation-rate";
                menu.ComponentPath = "logistics/manufacturing/mps/personnel-operation-rate/index";
                menu.SortOrder = 10;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertMPS10;
            updateCount += updateMPS10;

            var (insertMPS11, updateMPS11) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_MPS_EQUIPMENT_OPERATION_RATE", menu =>
            {
                menu.MenuName = "设备稼动率";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_MPS_EQUIPMENT_OPERATION_RATE";
                menu.I18nKey = "menu.logistics.manufacturing.mps.equipment.operation.rate";
                menu.Icon = "RiPulseLine";
                menu.ParentId = manufacturingMpsMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:mps:equipment:operation:rate:list";
                menu.RoutePath = "/logistics/manufacturing/mps/equipment-operation-rate";
                menu.ComponentPath = "logistics/manufacturing/mps/equipment-operation-rate/index";
                menu.SortOrder = 11;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertMPS11;
            updateCount += updateMPS11;

            var (insertMPS12, updateMPS12) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_MPS_PRODUCTION_EQUIPMENT", menu =>
            {
                menu.MenuName = "生产设备";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_MPS_PRODUCTION_EQUIPMENT";
                menu.I18nKey = "menu.logistics.manufacturing.mps.production.equipment";
                menu.Icon = "RiCpuLine";
                menu.ParentId = manufacturingMpsMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:mps:production:equipment:list";
                menu.RoutePath = "/logistics/manufacturing/mps/production-equipment";
                menu.ComponentPath = "logistics/manufacturing/mps/production-equipment/index";
                menu.SortOrder = 12;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertMPS12;
            updateCount += updateMPS12;
        }

        // ========== APS排程下的四级菜单 ==========
        if (manufacturingApsMenu != null)
        {
            var (insertAPS0, updateAPS0) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_APS_PARAMETER", menu =>
            {
                menu.MenuName = "APS 参数设置";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_APS_PARAMETER";
                menu.I18nKey = "menu.logistics.manufacturing.aps.parameter.setting";
                menu.Icon = "RiSettings3Line";
                menu.ParentId = manufacturingApsMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:aps:parameter:list";
                menu.RoutePath = "/logistics/manufacturing/aps/parameter-setting";
                menu.ComponentPath = "error/coming-soon";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAPS0;
            updateCount += updateAPS0;

            var (insertAPS1, updateAPS1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_APS_SCHEDULE_RULE", menu =>
            {
                menu.MenuName = "排程规则";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_APS_SCHEDULE_RULE";
                menu.I18nKey = "menu.logistics.manufacturing.aps.schedule.rule";
                menu.Icon = "RiListSettingsLine";
                menu.ParentId = manufacturingApsMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:aps:schedule:rule:list";
                menu.RoutePath = "/logistics/manufacturing/aps/schedule-rule";
                menu.ComponentPath = "error/coming-soon";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAPS1;
            updateCount += updateAPS1;

            var (insertAPS2, updateAPS2) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_APS_SCHEDULE", menu =>
            {
                menu.MenuName = "高级排程";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_APS_SCHEDULE";
                menu.I18nKey = "menu.logistics.manufacturing.aps.advanced.schedule";
                menu.Icon = "RiCalendarScheduleLine";
                menu.ParentId = manufacturingApsMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:aps:schedule:list";
                menu.RoutePath = "/logistics/manufacturing/aps/aps-schedule";
                menu.ComponentPath = "logistics/manufacturing/aps/aps-schedule/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAPS2;
            updateCount += updateAPS2;

            var (insertAPS3, updateAPS3) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_APS_RESOURCE_LOAD", menu =>
            {
                menu.MenuName = "资源负载";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_APS_RESOURCE_LOAD";
                menu.I18nKey = "menu.logistics.manufacturing.aps.resource.load";
                menu.Icon = "RiStackLine";
                menu.ParentId = manufacturingApsMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:aps:resource:load:list";
                menu.RoutePath = "/logistics/manufacturing/aps/resource-load";
                menu.ComponentPath = "error/coming-soon";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAPS3;
            updateCount += updateAPS3;

            var (insertAPS4, updateAPS4) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_APS_ORDER_SPLIT_MERGE", menu =>
            {
                menu.MenuName = "订单拆分与合并";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_APS_ORDER_SPLIT_MERGE";
                menu.I18nKey = "menu.logistics.manufacturing.aps.order.split.merge";
                menu.Icon = "RiSplitCellsHorizontal";
                menu.ParentId = manufacturingApsMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:aps:order:split:merge:list";
                menu.RoutePath = "/logistics/manufacturing/aps/order-split-merge";
                menu.ComponentPath = "error/coming-soon";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAPS4;
            updateCount += updateAPS4;

            var (insertAPS6, updateAPS6) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_APS_PRODUCTION_ORDER", menu =>
            {
                menu.MenuName = "生产工单";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_APS_PRODUCTION_ORDER";
                menu.I18nKey = "menu.logistics.manufacturing.aps.production.order";
                menu.Icon = "RiFileList3Line";
                menu.ParentId = manufacturingApsMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:aps:production:order:list";
                menu.RoutePath = "/logistics/manufacturing/aps/production-order";
                menu.ComponentPath = "logistics/manufacturing/aps/production-order/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAPS6;
            updateCount += updateAPS6;

            var (insertAPS7, updateAPS7) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_APS_GROUP", menu =>
            {
                menu.MenuName = "APS组";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_APS_GROUP";
                menu.I18nKey = "menu.logistics.manufacturing.aps.group";
                menu.Icon = "RiGroupLine";
                menu.ParentId = manufacturingApsMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:aps:group:list";
                menu.RoutePath = "/logistics/manufacturing/aps/group";
                menu.ComponentPath = "logistics/manufacturing/aps/group/index";
                menu.SortOrder = 7;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAPS7;
            updateCount += updateAPS7;
        }

        // ========== 设变下的四级菜单 ==========
        // 顺序：…制造技术课 / 设变通知 / 旧品管制…；设变组置末
        if (manufacturingEngineeringChangeMenu != null)
        {
            var (insertECN1, updateECN1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_KANBAN", menu =>
            {
                menu.MenuName = "设变看板";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_KANBAN";
                menu.I18nKey = "menu.logistics.manufacturing.engineering.change.kanban";
                menu.Icon = "RiDashboardLine";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineering:change:kanban:list";
                menu.RoutePath = "/logistics/manufacturing/engineering-change/ec-kanban";
                menu.ComponentPath = "logistics/manufacturing/engineering-change/ec-kanban/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECN1;
            updateCount += updateECN1;

            var (insertECN3, updateECN3) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_IMPL_CTRL", menu =>
            {
                menu.MenuName = "实施管控";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_IMPL_CTRL";
                menu.I18nKey = "menu.logistics.manufacturing.engineering.change.implctrl";
                menu.Icon = "RiCheckboxCircleLine";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineering:change:implctrl:list";
                menu.RoutePath = "/logistics/manufacturing/engineering-change/ec-impl-ctrl";
                menu.ComponentPath = "logistics/manufacturing/engineering-change/ec-impl-ctrl/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECN3;
            updateCount += updateECN3;

            var (insertECN4, updateECN4) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_TE", menu =>
            {
                menu.MenuName = "技术部门";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_TE";
                menu.I18nKey = "menu.logistics.manufacturing.engineering.change.te";
                menu.Icon = "RiCpuLine";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineering:change:te:list";
                menu.RoutePath = "/logistics/manufacturing/engineering-change/ec-te";
                menu.ComponentPath = "logistics/manufacturing/engineering-change/ec-te/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECN4;
            updateCount += updateECN4;


            var (insertECN5, updateECN5) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_PC", menu =>
            {
                menu.MenuName = "生管部门";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_PC";
                menu.I18nKey = "menu.logistics.manufacturing.engineering.change.pc";
                menu.Icon = "RiSettings3Line";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineering:change:pc:list";
                menu.RoutePath = "/logistics/manufacturing/engineering-change/ec-pc";
                menu.ComponentPath = "logistics/manufacturing/engineering-change/ec-pc/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECN5;
            updateCount += updateECN5;

            var (insertECN6, updateECN6) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_PUR", menu =>
            {
                menu.MenuName = "采购部门";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_PUR";
                menu.I18nKey = "menu.logistics.manufacturing.engineering.change.mp";
                menu.Icon = "RiShoppingCart2Line";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineering:change:pur:list";
                menu.RoutePath = "/logistics/manufacturing/engineering-change/ec-pur";
                menu.ComponentPath = "logistics/manufacturing/engineering-change/ec-pur/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECN6;
            updateCount += updateECN6;

            var (insertECN7, updateECN7) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_IQC", menu =>
            {
                menu.MenuName = "受检部门";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_IQC";
                menu.I18nKey = "menu.logistics.manufacturing.engineering.change.iqc";
                menu.Icon = "RiSearchEyeLine";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineering:change:iqc:list";
                menu.RoutePath = "/logistics/manufacturing/engineering-change/ec-iqc";
                menu.ComponentPath = "logistics/manufacturing/engineering-change/ec-iqc/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECN7;
            updateCount += updateECN7;

            var (insertECN8, updateECN8) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_MC", menu =>
            {
                menu.MenuName = "部管部门";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_MC";
                menu.I18nKey = "menu.logistics.manufacturing.engineering.change.mc";
                menu.Icon = "RiArchiveDrawerLine";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineering:change:mc:list";
                menu.RoutePath = "/logistics/manufacturing/engineering-change/ec-mc";
                menu.ComponentPath = "logistics/manufacturing/engineering-change/ec-mc/index";
                menu.SortOrder = 7;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECN8;
            updateCount += updateECN8;

            var (insertECN9Pcba, updateECN9Pcba) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_MTL", menu =>
            {
                menu.MenuName = "电子物料";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_MTL";
                menu.I18nKey = "menu.logistics.manufacturing.engineering.change.mtl";
                menu.Icon = "RiCpuLine";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineering:change:mtl:list";
                menu.RoutePath = "/logistics/manufacturing/engineering-change/ec-mtl";
                menu.ComponentPath = "logistics/manufacturing/engineering-change/ec-mtl/index";
                menu.SortOrder = 8;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECN9Pcba;
            updateCount += updateECN9Pcba;

            var (insertECN9, updateECN9) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_MFG2", menu =>
            {
                menu.MenuName = "制造二课";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_MFG2";
                menu.I18nKey = "menu.logistics.manufacturing.engineering.change.pcba";
                menu.Icon = "RiSeedlingLine";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineering:change:mfg2:list";
                menu.RoutePath = "/logistics/manufacturing/engineering-change/ec-mfg2";
                menu.ComponentPath = "logistics/manufacturing/engineering-change/ec-mfg2/index";
                menu.SortOrder = 9;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECN9;
            updateCount += updateECN9;

            var (insertECN10, updateECN10) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_MFG1", menu =>
            {
                menu.MenuName = "制造一课";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_MFG1";
                menu.I18nKey = "menu.logistics.manufacturing.engineering.change.assy";
                menu.Icon = "RiPlantLine";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineering:change:mfg1:list";
                menu.RoutePath = "/logistics/manufacturing/engineering-change/ec-mfg1";
                menu.ComponentPath = "logistics/manufacturing/engineering-change/ec-mfg1/index";
                menu.SortOrder = 10;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECN10;
            updateCount += updateECN10;

            var (insertECN11, updateECN11) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_QC", menu =>
            {
                menu.MenuName = "品管部门";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_QC";
                menu.I18nKey = "menu.logistics.manufacturing.engineering.change.qa";
                menu.Icon = "RiShieldCheckLine";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineering:change:qc:list";
                menu.RoutePath = "/logistics/manufacturing/engineering-change/ec-qc";
                menu.ComponentPath = "logistics/manufacturing/engineering-change/ec-qc/index";
                menu.SortOrder = 11;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECN11;
            updateCount += updateECN11;

            var (insertECN11Te, updateECN11Te) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_ME", menu =>
            {
                menu.MenuName = "制造技术课";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_ME";
                menu.I18nKey = "menu.logistics.manufacturing.engineering.change.me";
                menu.Icon = "RiToolsLine";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineering:change:me:list";
                menu.RoutePath = "/logistics/manufacturing/engineering-change/ec-me";
                menu.ComponentPath = "logistics/manufacturing/engineering-change/ec-me/index";
                menu.SortOrder = 12;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECN11Te;
            updateCount += updateECN11Te;

            var (insertECNNotification, updateECNNotification) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_NOTIFICATION", menu =>
            {
                menu.MenuName = "设变通知";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_NOTIFICATION";
                menu.I18nKey = "menu.logistics.manufacturing.engineering.change.notification";
                menu.Icon = "RiNotificationLine";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineering:change:notification:list";
                menu.RoutePath = "/logistics/manufacturing/engineering-change/ec-notification";
                menu.ComponentPath = "logistics/manufacturing/engineering-change/ec-notification/index";
                menu.SortOrder = 13;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECNNotification;
            updateCount += updateECNNotification;

            var (insertECN12, updateECN12) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_LEGACY_PRODUCT", menu =>
            {
                menu.MenuName = "旧品管制";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_LEGACY_PRODUCT";
                menu.I18nKey = "menu.logistics.manufacturing.engineering.change.legacy.product";
                menu.Icon = "RiTimeLine";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineering:change:legacy:product:list";
                menu.RoutePath = "/logistics/manufacturing/engineering-change/legacy-product";
                menu.ComponentPath = "logistics/manufacturing/engineering-change/legacy-product/index";
                menu.SortOrder = 14;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECN12;
            updateCount += updateECN12;

            var (insertECNSourceEc, updateECNSourceEc) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_SOURCE_EC", menu =>
            {
                menu.MenuName = "设变来源";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_SOURCE_EC";
                menu.I18nKey = "menu.logistics.manufacturing.engineering.change.source.ec";
                menu.Icon = "RiInboxArchiveLine";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineering:change:source:ec:list";
                menu.RoutePath = "/logistics/manufacturing/engineering-change/source-ec";
                menu.ComponentPath = "logistics/manufacturing/engineering-change/source-ec/index";
                menu.SortOrder = 15;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECNSourceEc;
            updateCount += updateECNSourceEc;

            var (insertECNMonthlyTrend, updateECNMonthlyTrend) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_MONTHLY_TREND", menu =>
            {
                menu.MenuName = "设变推移";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_MONTHLY_TREND";
                menu.I18nKey = "menu.logistics.manufacturing.engineering.change.monthly.trend";
                menu.Icon = "RiLineChartLine";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineering:change:monthly:trend:list";
                menu.RoutePath = "/logistics/manufacturing/engineering-change/ec-monthly-trend";
                menu.ComponentPath = "logistics/manufacturing/engineering-change/ec-monthly-trend/index";
                menu.SortOrder = 16;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECNMonthlyTrend;
            updateCount += updateECNMonthlyTrend;

            var (insertECNStat, updateECNStat) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_STAT", menu =>
            {
                menu.MenuName = "设变统计";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_STAT";
                menu.I18nKey = "menu.logistics.manufacturing.engineering.change.stat";
                menu.Icon = "RiBarChartLine";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineering:change:stat:list";
                menu.RoutePath = "/logistics/manufacturing/engineering-change/ec-stat";
                menu.ComponentPath = "logistics/manufacturing/engineering-change/ec-stat/index";
                menu.SortOrder = 17;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECNStat;
            updateCount += updateECNStat;

            var (insertECNGroup, updateECNGroup) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_EC_GROUP", menu =>
            {
                menu.MenuName = "设变组";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_EC_GROUP";
                menu.I18nKey = "menu.logistics.manufacturing.engineering.change.ec.group";
                menu.Icon = "RiGroupLine";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineering:change:ec:group:list";
                menu.RoutePath = "/logistics/manufacturing/engineering-change/ec-group";
                menu.ComponentPath = "logistics/manufacturing/engineering-change/ec-group/index";
                menu.SortOrder = 18;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECNGroup;
            updateCount += updateECNGroup;
        }

        // ========== OPH管理下的四级菜单 ==========
        if (manufacturingOutputMenu != null)
        {
            var (insertOUT2, updateOUT2) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_OUTPUT_PCBA", menu =>
            {
                menu.MenuName = "PCBA日报";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_OUTPUT_PCBA";
                menu.I18nKey = "menu.logistics.manufacturing.output.pcba";
                menu.Icon = "RiCpuLine";
                menu.ParentId = manufacturingOutputMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:output:mfg2:list";
                menu.RoutePath = "/logistics/manufacturing/output/pcba-output";
                menu.ComponentPath = "logistics/manufacturing/output/pcba-output/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertOUT2;
            updateCount += updateOUT2;

            var (insertOUT4, updateOUT4) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_OUTPUT_ASSY", menu =>
            {
                menu.MenuName = "组立日报";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_OUTPUT_ASSY";
                menu.I18nKey = "menu.logistics.manufacturing.output.assy";
                menu.Icon = "RiSettings4Line";
                menu.ParentId = manufacturingOutputMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:output:mfg1:list";
                menu.RoutePath = "/logistics/manufacturing/output/assy-output";
                menu.ComponentPath = "logistics/manufacturing/output/assy-output/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertOUT4;
            updateCount += updateOUT4;

            var (insertOUT6, updateOUT6) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_OUTPUT_PRODUCTION_CHANGEOVER", menu =>
            {
                menu.MenuName = "生产切换";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_OUTPUT_PRODUCTION_CHANGEOVER";
                menu.I18nKey = "menu.logistics.manufacturing.output.production.changeover";
                menu.Icon = "RiRefreshLine";
                menu.ParentId = manufacturingOutputMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:output:production:changeover:list";
                menu.RoutePath = "/logistics/manufacturing/output/production-changeover";
                menu.ComponentPath = "logistics/manufacturing/output/production-changeover/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertOUT6;
            updateCount += updateOUT6;

            var (insertOUT7, updateOUT7) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_OUTPUT_CHANGEOVER_STAT", menu =>
            {
                menu.MenuName = "切换统计";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_OUTPUT_CHANGEOVER_STAT";
                menu.I18nKey = "menu.logistics.manufacturing.output.changeover.stat";
                menu.Icon = "RiBarChartLine";
                menu.ParentId = manufacturingOutputMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:output:changeover:stat:list";
                menu.RoutePath = "/logistics/manufacturing/output/changeover-stat";
                menu.ComponentPath = "logistics/manufacturing/output/changeover-stat/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertOUT7;
            updateCount += updateOUT7;

            var (insertOUT8, updateOUT8) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_OUTPUT_PRODUCTION_MONTHLY", menu =>
            {
                menu.MenuName = "生产统计";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_OUTPUT_PRODUCTION_MONTHLY";
                menu.I18nKey = "menu.logistics.manufacturing.output.production.monthly";
                menu.Icon = "RiLineChartLine";
                menu.ParentId = manufacturingOutputMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:output:production:monthly:list";
                menu.RoutePath = "/logistics/manufacturing/output/production-monthly";
                menu.ComponentPath = "logistics/manufacturing/output/production-monthly/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertOUT8;
            updateCount += updateOUT8;

            var (insertOUT9, updateOUT9) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_OUTPUT_PRODUCTION_TREND", menu =>
            {
                menu.MenuName = "生产推移";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_OUTPUT_PRODUCTION_TREND";
                menu.I18nKey = "menu.logistics.manufacturing.output.production.trend";
                menu.Icon = "RiLineChartLine";
                menu.ParentId = manufacturingOutputMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:output:production:trend:list";
                menu.RoutePath = "/logistics/manufacturing/output/production-trend";
                menu.ComponentPath = "logistics/manufacturing/output/production-trend/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertOUT9;
            updateCount += updateOUT9;
        }

        // ========== OPH工数下的四级菜单（对齐 LaborHour 实体）==========
        if (manufacturingLaborHourMenu != null)
        {
            var (insertLH1, updateLH1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_LABOR_HOUR_ASSY", menu =>
            {
                menu.MenuName = "组立工数";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_LABOR_HOUR_ASSY";
                menu.I18nKey = "menu.logistics.manufacturing.labor.hour.assy";
                menu.Icon = "RiSettings4Line";
                menu.ParentId = manufacturingLaborHourMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:labor:hour:mfg1:list";
                menu.RoutePath = "/logistics/manufacturing/labor-hour/assy-labor-hour";
                menu.ComponentPath = "logistics/manufacturing/labor-hour/assy-labor-hour/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLH1;
            updateCount += updateLH1;

            var (insertLH2, updateLH2) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_LABOR_HOUR_PCBA_AI", menu =>
            {
                menu.MenuName = "自插工数";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_LABOR_HOUR_PCBA_AI";
                menu.I18nKey = "menu.logistics.manufacturing.labor.hour.pcba.ai";
                menu.Icon = "RiPulseLine";
                menu.ParentId = manufacturingLaborHourMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:labor:hour:mfg2:ai:list";
                menu.RoutePath = "/logistics/manufacturing/labor-hour/pcba-ai-labor-hour";
                menu.ComponentPath = "logistics/manufacturing/labor-hour/pcba-ai-labor-hour/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLH2;
            updateCount += updateLH2;

            var (insertLH3, updateLH3) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_LABOR_HOUR_PCBA_MI", menu =>
            {
                menu.MenuName = "手插工数";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_LABOR_HOUR_PCBA_MI";
                menu.I18nKey = "menu.logistics.manufacturing.labor.hour.pcba.mi";
                menu.Icon = "RiUserLine";
                menu.ParentId = manufacturingLaborHourMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:labor:hour:mfg2:mi:list";
                menu.RoutePath = "/logistics/manufacturing/labor-hour/pcba-mi-labor-hour";
                menu.ComponentPath = "logistics/manufacturing/labor-hour/pcba-mi-labor-hour/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLH3;
            updateCount += updateLH3;

            var (insertLH4, updateLH4) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_LABOR_HOUR_PCBA_REPAIR", menu =>
            {
                menu.MenuName = "改修工数";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_LABOR_HOUR_PCBA_REPAIR";
                menu.I18nKey = "menu.logistics.manufacturing.labor.hour.pcba.repair";
                menu.Icon = "RiToolsLine";
                menu.ParentId = manufacturingLaborHourMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:labor:hour:mfg2:repair:list";
                menu.RoutePath = "/logistics/manufacturing/labor-hour/pcba-repair-labor-hour";
                menu.ComponentPath = "logistics/manufacturing/labor-hour/pcba-repair-labor-hour/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLH4;
            updateCount += updateLH4;

            var (insertLH5, updateLH5) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_LABOR_HOUR_PCBA_SMT", menu =>
            {
                menu.MenuName = "SMT工数";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_LABOR_HOUR_PCBA_SMT";
                menu.I18nKey = "menu.logistics.manufacturing.labor.hour.pcba.smt";
                menu.Icon = "RiCpuLine";
                menu.ParentId = manufacturingLaborHourMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:labor:hour:mfg2:mtl:list";
                menu.RoutePath = "/logistics/manufacturing/labor-hour/pcba-smt-labor-hour";
                menu.ComponentPath = "logistics/manufacturing/labor-hour/pcba-smt-labor-hour/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLH5;
            updateCount += updateLH5;
        }

        // ========== 生产不良下的四级菜单 ==========
        if (manufacturingDefectMenu != null)
        {
            var (insertDEF0, updateDEF0) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_DEFECT_GROUP", menu =>
            {
                menu.MenuName = "不良组";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_DEFECT_GROUP";
                menu.I18nKey = "menu.logistics.manufacturing.defect.group";
                menu.Icon = "RiGroupLine";
                menu.ParentId = manufacturingDefectMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:defect:group:list";
                menu.RoutePath = "/logistics/manufacturing/defect/group";
                menu.ComponentPath = "logistics/manufacturing/defect/group/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertDEF0;
            updateCount += updateDEF0;

            var (insertDEF1, updateDEF1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_DEFECT_PCBA_INSPECTION", menu =>
            {
                menu.MenuName = "SMT检查";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_DEFECT_PCBA_INSPECTION";
                menu.I18nKey = "menu.logistics.manufacturing.defect.pcba.inspection";
                menu.Icon = "RiSearchLine";
                menu.ParentId = manufacturingDefectMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:defect:mfg2:inspection:list";
                menu.RoutePath = "/logistics/manufacturing/defect/pcba-inspection";
                menu.ComponentPath = "logistics/manufacturing/defect/pcba-inspection/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertDEF1;
            updateCount += updateDEF1;

            var (insertDEF3, updateDEF3) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_DEFECT_PCBA_REPAIR", menu =>
            {
                menu.MenuName = "PCBA修理";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_DEFECT_PCBA_REPAIR";
                menu.I18nKey = "menu.logistics.manufacturing.defect.pcba.repair";
                menu.Icon = "RiToolsLine";
                menu.ParentId = manufacturingDefectMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:defect:mfg2:repair:list";
                menu.RoutePath = "/logistics/manufacturing/defect/pcba-repair";
                menu.ComponentPath = "logistics/manufacturing/defect/pcba-repair/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertDEF3;
            updateCount += updateDEF3;

            var (insertDEF5, updateDEF5) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_DEFECT_ASSY", menu =>
            {
                menu.MenuName = "组立不良";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_DEFECT_ASSY";
                menu.I18nKey = "menu.logistics.manufacturing.defect.assy";
                menu.Icon = "RiAlarmWarningLine";
                menu.ParentId = manufacturingDefectMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:defect:mfg1:list";
                menu.RoutePath = "/logistics/manufacturing/defect/assy-defect";
                menu.ComponentPath = "logistics/manufacturing/defect/assy-defect/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertDEF5;
            updateCount += updateDEF5;

            var (insertDEF7, updateDEF7) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_DEFECT_MONTHLY", menu =>
            {
                menu.MenuName = "不良推移";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_DEFECT_MONTHLY";
                menu.I18nKey = "menu.logistics.manufacturing.defect.monthly";
                menu.Icon = "RiLineChartLine";
                menu.ParentId = manufacturingDefectMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:defect:monthly:list";
                menu.RoutePath = "/logistics/manufacturing/defect/defect-monthly";
                menu.ComponentPath = "logistics/manufacturing/defect/defect-monthly/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertDEF7;
            updateCount += updateDEF7;

            var (insertDEF8, updateDEF8) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_DEFECT_STAT", menu =>
            {
                menu.MenuName = "不良统计";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_DEFECT_STAT";
                menu.I18nKey = "menu.logistics.manufacturing.defect.stat";
                menu.Icon = "RiBarChartLine";
                menu.ParentId = manufacturingDefectMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:defect:stat:list";
                menu.RoutePath = "/logistics/manufacturing/defect/defect-stat";
                menu.ComponentPath = "logistics/manufacturing/defect/defect-stat/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertDEF8;
            updateCount += updateDEF8;
        }

        // ========== SOP 管理下的四级菜单（对齐 Sop/ 实体：Workstation/Doc/Revision/Ack/Exec/ExecScan/EsdCheck/Call）==========
        if (manufacturingSopMenu != null)
        {
            var (insertSOP0, updateSOP0) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_SOP_WORKSTATION", menu =>
            {
                menu.MenuName = "工位管理";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_SOP_WORKSTATION";
                menu.I18nKey = "menu.logistics.manufacturing.sop.workstation";
                menu.Icon = "RiMapPinLine";
                menu.ParentId = manufacturingSopMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:sop:workstation:list";
                menu.RoutePath = "/logistics/manufacturing/sop/workstation";
                menu.ComponentPath = "logistics/manufacturing/sop/workstation/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSOP0;
            updateCount += updateSOP0;

            var (insertSOP1, updateSOP1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_SOP_DOC", menu =>
            {
                menu.MenuName = "SOP文档";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_SOP_DOC";
                menu.I18nKey = "menu.logistics.manufacturing.sop.doc";
                menu.Icon = "RiFilePaper2Line";
                menu.ParentId = manufacturingSopMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:sop:doc:list";
                menu.RoutePath = "/logistics/manufacturing/sop/doc";
                menu.ComponentPath = "logistics/manufacturing/sop/doc/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSOP1;
            updateCount += updateSOP1;

            var (insertSOP2, updateSOP2) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_SOP_REVISION", menu =>
            {
                menu.MenuName = "SOP版本";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_SOP_REVISION";
                menu.I18nKey = "menu.logistics.manufacturing.sop.revision";
                menu.Icon = "RiGitBranchLine";
                menu.ParentId = manufacturingSopMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:sop:revision:list";
                menu.RoutePath = "/logistics/manufacturing/sop/revision";
                menu.ComponentPath = "logistics/manufacturing/sop/revision/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSOP2;
            updateCount += updateSOP2;

            var (insertSOP3, updateSOP3) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_SOP_ACK", menu =>
            {
                menu.MenuName = "版本确认";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_SOP_ACK";
                menu.I18nKey = "menu.logistics.manufacturing.sop.ack";
                menu.Icon = "RiCheckboxCircleLine";
                menu.ParentId = manufacturingSopMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:sop:ack:list";
                menu.RoutePath = "/logistics/manufacturing/sop/ack";
                menu.ComponentPath = "logistics/manufacturing/sop/ack/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSOP3;
            updateCount += updateSOP3;

            var (insertSOP4, updateSOP4) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_SOP_EXEC", menu =>
            {
                menu.MenuName = "工位执行";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_SOP_EXEC";
                menu.I18nKey = "menu.logistics.manufacturing.sop.exec";
                menu.Icon = "RiPlayCircleLine";
                menu.ParentId = manufacturingSopMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:sop:exec:list";
                menu.RoutePath = "/logistics/manufacturing/sop/exec";
                menu.ComponentPath = "logistics/manufacturing/sop/exec/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSOP4;
            updateCount += updateSOP4;

            var (insertSOP5, updateSOP5) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_SOP_EXEC_SCAN", menu =>
            {
                menu.MenuName = "扫码记录";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_SOP_EXEC_SCAN";
                menu.I18nKey = "menu.logistics.manufacturing.sop.exec.scan";
                menu.Icon = "RiBarcodeLine";
                menu.ParentId = manufacturingSopMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:sop:exec:scan:list";
                menu.RoutePath = "/logistics/manufacturing/sop/exec-scan";
                menu.ComponentPath = "logistics/manufacturing/sop/exec-scan/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSOP5;
            updateCount += updateSOP5;

            var (insertSOP6, updateSOP6) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_SOP_ESD_CHECK", menu =>
            {
                menu.MenuName = "ESD检查";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_SOP_ESD_CHECK";
                menu.I18nKey = "menu.logistics.manufacturing.sop.esd.check";
                menu.Icon = "RiShieldFlashLine";
                menu.ParentId = manufacturingSopMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:sop:esd:check:list";
                menu.RoutePath = "/logistics/manufacturing/sop/esd-check";
                menu.ComponentPath = "logistics/manufacturing/sop/esd-check/index";
                menu.SortOrder = 7;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSOP6;
            updateCount += updateSOP6;

            var (insertSOP7, updateSOP7) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_SOP_CALL", menu =>
            {
                menu.MenuName = "安灯呼叫";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_SOP_CALL";
                menu.I18nKey = "menu.logistics.manufacturing.sop.call";
                menu.Icon = "RiAlarmWarningLine";
                menu.ParentId = manufacturingSopMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:sop:call:list";
                menu.RoutePath = "/logistics/manufacturing/sop/call";
                menu.ComponentPath = "logistics/manufacturing/sop/call/index";
                menu.SortOrder = 8;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSOP7;
            updateCount += updateSOP7;

            var (insertSOP8, updateSOP8) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_SOP_GROUP", menu =>
            {
                menu.MenuName = "SOP组";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_SOP_GROUP";
                menu.I18nKey = "menu.logistics.manufacturing.sop.group";
                menu.Icon = "RiGroupLine";
                menu.ParentId = manufacturingSopMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:sop:group:list";
                menu.RoutePath = "/logistics/manufacturing/sop/group";
                menu.ComponentPath = "logistics/manufacturing/sop/group/index";
                menu.SortOrder = 9;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSOP8;
            updateCount += updateSOP8;
        }

        // ========== 品质成本下的四级菜单 ==========
        if (qualityCostMenu != null)
        {
            var (insertQC1, updateQC1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_COST_ASSURANCE", menu =>
            {
                menu.MenuName = "品质保证";
                menu.MenuCode = "LOGISTICS_QUALITY_COST_ASSURANCE";
                menu.I18nKey = "menu.logistics.quality.cost.assurance";
                menu.Icon = "RiShieldCheckLine";
                menu.ParentId = qualityCostMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:cost:assurance:list";
                menu.RoutePath = "/logistics/quality/cost/assurance";
                menu.ComponentPath = "logistics/quality/cost/assurance/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertQC1;
            updateCount += updateQC1;

            var (insertQC2, updateQC2) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_COST_ISSUE", menu =>
            {
                menu.MenuName = "品质问题";
                menu.MenuCode = "LOGISTICS_QUALITY_COST_ISSUE";
                menu.I18nKey = "menu.logistics.quality.cost.issue";
                menu.Icon = "RiErrorWarningLine";
                menu.ParentId = qualityCostMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:cost:issue:list";
                menu.RoutePath = "/logistics/quality/cost/issue";
                menu.ComponentPath = "logistics/quality/cost/issue/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertQC2;
            updateCount += updateQC2;

            var (insertQC3, updateQC3) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_COST_INCIDENT", menu =>
            {
                menu.MenuName = "品质事故";
                menu.MenuCode = "LOGISTICS_QUALITY_COST_INCIDENT";
                menu.I18nKey = "menu.logistics.quality.cost.incident";
                menu.Icon = "RiAlarmWarningLine";
                menu.ParentId = qualityCostMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:cost:incident:list";
                menu.RoutePath = "/logistics/quality/cost/incident";
                menu.ComponentPath = "logistics/quality/cost/incident/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertQC3;
            updateCount += updateQC3;

            var (insertQC4, updateQC4) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_COST_TREND", menu =>
            {
                menu.MenuName = "质量成本推移";
                menu.MenuCode = "LOGISTICS_QUALITY_COST_TREND";
                menu.I18nKey = "menu.logistics.quality.cost.trend";
                menu.Icon = "RiLineChartLine";
                menu.ParentId = qualityCostMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:cost:trend:list";
                menu.RoutePath = "/logistics/quality/cost/cost-trend";
                menu.ComponentPath = "logistics/quality/cost/cost-trend/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertQC4;
            updateCount += updateQC4;

            var (insertQC5, updateQC5) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_COST_GROUP", menu =>
            {
                menu.MenuName = "成本组";
                menu.MenuCode = "LOGISTICS_QUALITY_COST_GROUP";
                menu.I18nKey = "menu.logistics.quality.cost.group";
                menu.Icon = "RiGroupLine";
                menu.ParentId = qualityCostMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:cost:group:list";
                menu.RoutePath = "/logistics/quality/cost/group";
                menu.ComponentPath = "logistics/quality/cost/group/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertQC5;
            updateCount += updateQC5;

            var (insertQC6, updateQC6) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_COST_REPORT", menu =>
            {
                menu.MenuName = "品质成本报告";
                menu.MenuCode = "LOGISTICS_QUALITY_COST_REPORT";
                menu.I18nKey = "menu.logistics.quality.cost.report";
                menu.Icon = "RiBarChartLine";
                menu.ParentId = qualityCostMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:cost:report:list";
                menu.RoutePath = "/logistics/quality/cost/cost-report";
                menu.ComponentPath = "logistics/quality/cost/cost-report/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertQC6;
            updateCount += updateQC6;
        }

        // ========== 质量业务下的四级菜单 ==========
        if (qualityAssuranceMenu != null)
        {
            var (insertQO0, updateQO0) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_OPERATION_QUALITY_GROUP", menu =>
            {
                menu.MenuName = "质量组";
                menu.MenuCode = "LOGISTICS_QUALITY_OPERATION_QUALITY_GROUP";
                menu.I18nKey = "menu.logistics.quality.operation.group";
                menu.Icon = "RiGroupLine";
                menu.ParentId = qualityAssuranceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:operation:group:list";
                menu.RoutePath = "/logistics/quality/operation/group";
                menu.ComponentPath = "logistics/quality/operation/group/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertQO0;
            updateCount += updateQO0;

            var (insertQO1, updateQO1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_OPERATION_SAMPLING_SCHEME", menu =>
            {
                menu.MenuName = "抽样方案";
                menu.MenuCode = "LOGISTICS_QUALITY_OPERATION_SAMPLING_SCHEME";
                menu.I18nKey = "menu.logistics.quality.operation.sampling.scheme";
                menu.Icon = "RiListCheck";
                menu.ParentId = qualityAssuranceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:operation:sampling:scheme:list";
                menu.RoutePath = "/logistics/quality/operation/sampling-scheme";
                menu.ComponentPath = "logistics/quality/operation/sampling-scheme/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertQO1;
            updateCount += updateQO1;

            var (insertQO2, updateQO2) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_OPERATION_INSPECTION_STANDARD", menu =>
            {
                menu.MenuName = "检验标准";
                menu.MenuCode = "LOGISTICS_QUALITY_OPERATION_INSPECTION_STANDARD";
                menu.I18nKey = "menu.logistics.quality.operation.inspection.standard";
                menu.Icon = "RiFileTextLine";
                menu.ParentId = qualityAssuranceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:operation:inspection:standard:list";
                menu.RoutePath = "/logistics/quality/operation/inspection-standard";
                menu.ComponentPath = "logistics/quality/operation/inspection-standard/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertQO2;
            updateCount += updateQO2;

            var (insertQO3, updateQO3) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_OPERATION_IQC_ORDER", menu =>
            {
                menu.MenuName = "进货检验";
                menu.MenuCode = "LOGISTICS_QUALITY_OPERATION_IQC_ORDER";
                menu.I18nKey = "menu.logistics.quality.operation.iqc.order";
                menu.Icon = "RiInboxArchiveLine";
                menu.ParentId = qualityAssuranceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:operation:iqc:order:list";
                menu.RoutePath = "/logistics/quality/operation/iqc-order";
                menu.ComponentPath = "logistics/quality/operation/iqc-order/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertQO3;
            updateCount += updateQO3;

            var (insertQO3Trend, updateQO3Trend) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_OPERATION_IQC_TREND", menu =>
            {
                menu.MenuName = "进货检验推移";
                menu.MenuCode = "LOGISTICS_QUALITY_OPERATION_IQC_TREND";
                menu.I18nKey = "menu.logistics.quality.operation.iqc.trend";
                menu.Icon = "RiLineChartLine";
                menu.ParentId = qualityAssuranceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:operation:iqc:trend:list";
                menu.RoutePath = "/logistics/quality/operation/iqc-trend";
                menu.ComponentPath = "logistics/quality/operation/iqc-trend/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertQO3Trend;
            updateCount += updateQO3Trend;

            var (insertQO4, updateQO4) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_OPERATION_IPQC_ORDER", menu =>
            {
                menu.MenuName = "制程检验";
                menu.MenuCode = "LOGISTICS_QUALITY_OPERATION_IPQC_ORDER";
                menu.I18nKey = "menu.logistics.quality.operation.ipqc.order";
                menu.Icon = "RiSettings3Line";
                menu.ParentId = qualityAssuranceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:operation:ipqc:order:list";
                menu.RoutePath = "/logistics/quality/operation/ipqc-order";
                menu.ComponentPath = "logistics/quality/operation/ipqc-order/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertQO4;
            updateCount += updateQO4;

            var (insertQO4Trend, updateQO4Trend) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_OPERATION_IPQC_TREND", menu =>
            {
                menu.MenuName = "过程质量推移";
                menu.MenuCode = "LOGISTICS_QUALITY_OPERATION_IPQC_TREND";
                menu.I18nKey = "menu.logistics.quality.operation.ipqc.trend";
                menu.Icon = "RiLineChartLine";
                menu.ParentId = qualityAssuranceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:operation:ipqc:trend:list";
                menu.RoutePath = "/logistics/quality/operation/ipqc-trend";
                menu.ComponentPath = "logistics/quality/operation/ipqc-trend/index";
                menu.SortOrder = 7;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertQO4Trend;
            updateCount += updateQO4Trend;

            var (insertQO5, updateQO5) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_OPERATION_FQC_ORDER", menu =>
            {
                menu.MenuName = "入库检验";
                menu.MenuCode = "LOGISTICS_QUALITY_OPERATION_FQC_ORDER";
                menu.I18nKey = "menu.logistics.quality.operation.fqc.order";
                menu.Icon = "RiArchiveDrawerLine";
                menu.ParentId = qualityAssuranceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:operation:fqc:order:list";
                menu.RoutePath = "/logistics/quality/operation/fqc-order";
                menu.ComponentPath = "logistics/quality/operation/fqc-order/index";
                menu.SortOrder = 8;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertQO5;
            updateCount += updateQO5;

            var (insertQO5Trend, updateQO5Trend) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_OPERATION_FQC_TREND", menu =>
            {
                menu.MenuName = "成品检验推移";
                menu.MenuCode = "LOGISTICS_QUALITY_OPERATION_FQC_TREND";
                menu.I18nKey = "menu.logistics.quality.operation.fqc.trend";
                menu.Icon = "RiLineChartLine";
                menu.ParentId = qualityAssuranceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:operation:fqc:trend:list";
                menu.RoutePath = "/logistics/quality/operation/fqc-trend";
                menu.ComponentPath = "logistics/quality/operation/fqc-trend/index";
                menu.SortOrder = 9;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertQO5Trend;
            updateCount += updateQO5Trend;

            var (insertQO7, updateQO7) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_OPERATION_MONTHLY", menu =>
            {
                menu.MenuName = "品质月报";
                menu.MenuCode = "LOGISTICS_QUALITY_OPERATION_MONTHLY";
                menu.I18nKey = "menu.logistics.quality.operation.monthly";
                menu.Icon = "RiShieldCheckLine";
                menu.ParentId = qualityAssuranceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:operation:monthly:list";
                menu.RoutePath = "/logistics/quality/operation/quality-monthly";
                menu.ComponentPath = "logistics/quality/operation/quality-monthly/index";
                menu.SortOrder = 10;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertQO7;
            updateCount += updateQO7;

            var (insertQO8, updateQO8) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_OPERATION_STAT", menu =>
            {
                menu.MenuName = "质量统计";
                menu.MenuCode = "LOGISTICS_QUALITY_OPERATION_STAT";
                menu.I18nKey = "menu.logistics.quality.operation.stat";
                menu.Icon = "RiBarChartLine";
                menu.ParentId = qualityAssuranceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:operation:stat:list";
                menu.RoutePath = "/logistics/quality/operation/quality-stat";
                menu.ComponentPath = "logistics/quality/operation/quality-stat/index";
                menu.SortOrder = 11;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertQO8;
            updateCount += updateQO8;
        }

        // ========== 客诉管理下的四级菜单 (LOGISTICS_QUALITY_COMPLAINT) ==========
        if (qualityComplaintMenu != null)
        {
            var (insertCP1, updateCP1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_COMPLAINT_CUSTOMER", menu =>
            {
                menu.MenuName = "客诉登记";
                menu.MenuCode = "LOGISTICS_QUALITY_COMPLAINT_CUSTOMER";
                menu.I18nKey = "menu.logistics.quality.complaint.customer";
                menu.Icon = "RiMessage3Line";
                menu.ParentId = qualityComplaintMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:complaint:customer:list";
                menu.RoutePath = "/logistics/quality/complaint/customer-complaint";
                menu.ComponentPath = "logistics/quality/complaint/customer-complaint/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertCP1;
            updateCount += updateCP1;

            var (insertCP2, updateCP2) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_COMPLAINT_CUSTOMER_TREND", menu =>
            {
                menu.MenuName = "顾客投诉推移";
                menu.MenuCode = "LOGISTICS_QUALITY_COMPLAINT_CUSTOMER_TREND";
                menu.I18nKey = "menu.logistics.quality.complaint.customer.trend";
                menu.Icon = "RiLineChartLine";
                menu.ParentId = qualityComplaintMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:complaint:customer:trend:list";
                menu.RoutePath = "/logistics/quality/complaint/customer-complaint-trend";
                menu.ComponentPath = "logistics/quality/complaint/customer-complaint-trend/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertCP2;
            updateCount += updateCP2;

            var (insertCP3, updateCP3) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_COMPLAINT_CUSTOMER_COMPLAINT_HANDLING", menu =>
            {
                menu.MenuName = "客诉处理";
                menu.MenuCode = "LOGISTICS_QUALITY_COMPLAINT_CUSTOMER_COMPLAINT_HANDLING";
                menu.I18nKey = "menu.logistics.quality.complaint.customer.complaint.handling";
                menu.Icon = "RiFileEditLine";
                menu.ParentId = qualityComplaintMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:complaint:customer:complaint:handling:list";
                menu.RoutePath = "/logistics/quality/complaint/customer-complaint-handling";
                menu.ComponentPath = "logistics/quality/complaint/customer-complaint-handling/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertCP3;
            updateCount += updateCP3;

            var (insertCP4, updateCP4) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_COMPLAINT_CUSTOMER_SATISFACTION_SURVEY", menu =>
            {
                menu.MenuName = "客户满意度调查";
                menu.MenuCode = "LOGISTICS_QUALITY_COMPLAINT_CUSTOMER_SATISFACTION_SURVEY";
                menu.I18nKey = "menu.logistics.quality.complaint.customer.satisfaction.survey";
                menu.Icon = "RiSurveyLine";
                menu.ParentId = qualityComplaintMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:complaint:customer:satisfaction:survey:list";
                menu.RoutePath = "/logistics/quality/complaint/customer-satisfaction-survey";
                menu.ComponentPath = "logistics/quality/complaint/customer-satisfaction-survey/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertCP4;
            updateCount += updateCP4;

            var (insertCP6, updateCP6) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_COMPLAINT_SUPPLIER_EVALUATION", menu =>
            {
                menu.MenuName = "供应商评价考核";
                menu.MenuCode = "LOGISTICS_QUALITY_COMPLAINT_SUPPLIER_EVALUATION";
                menu.I18nKey = "menu.logistics.quality.complaint.supplier.evaluation";
                menu.Icon = "RiStarLine";
                menu.ParentId = qualityComplaintMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:complaint:supplier:evaluation:list";
                menu.RoutePath = "/logistics/quality/complaint/supplier-evaluation";
                menu.ComponentPath = "logistics/quality/complaint/supplier-evaluation/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertCP6;
            updateCount += updateCP6;

            var (insertCP7, updateCP7) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_COMPLAINT_GROUP", menu =>
            {
                menu.MenuName = "客诉组";
                menu.MenuCode = "LOGISTICS_QUALITY_COMPLAINT_GROUP";
                menu.I18nKey = "menu.logistics.quality.complaint.group";
                menu.Icon = "RiGroupLine";
                menu.ParentId = qualityComplaintMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:complaint:group:list";
                menu.RoutePath = "/logistics/quality/complaint/group";
                menu.ComponentPath = "logistics/quality/complaint/group/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertCP7;
            updateCount += updateCP7;
        }

        return (insertCount, updateCount);
    }

    /// <summary>
    /// 创建或更新菜单。
    /// </summary>
    /// <param name="menuRepository">菜单仓储。</param>
    /// <param name="sqlSugarContext">SqlSugar上下文。</param>
    /// <param name="tenantCode">租户编码。</param>
    /// <param name="menuCode">菜单编码（业务键）。</param>
    /// <param name="configure">菜单配置委托。</param>
    /// <returns>元组:(InsertCount, UpdateCount),本条菜单新增或更新条数(0或1)。</returns>
    private static async Task<(int InsertCount, int UpdateCount)> CreateOrUpdateMenuAsync(
        ITaktTenantSeedRepository<TaktMenu> menuRepository,
        TaktSeedContext sqlSugarContext,
        string tenantCode,
        string menuCode,
        Action<TaktMenu> configure)
    {
        // 注意：种子数据必须使用仓储查询（带租户过滤），确保数据隔离
        var menu = await menuRepository.FirstAsync(m => m.TenantCode == tenantCode && m.MenuCode == menuCode);
        if (menu == null)
        {
            menu = new TaktMenu();
            
            // 必须先设置 TenantCode（租户级实体）
            menu.TenantCode = tenantCode;
            
            configure(menu);
            menu.IsBuiltIn = 1;
            
            // 自动计算 Level（根节点为 1）
            menu.Level = menu.ParentId > 0 ? 0 : 1; // 稍后根据父级计算
            menu.IsLeaf = 1; // 默认为叶子节点，后续创建子菜单时会自动更新
            
            menu.CreatedBy = 900001;
            menu.CreatedAt = DateTime.Now;
            
            menu = await menuRepository.CreateAsync(menu);
            
            // 更新 MenuPath 和 Level
            if (menu.ParentId > 0)
            {
                // 使用仓储查询父级（带租户过滤）
                var parentMenu = await menuRepository.FirstAsync(m => m.Id == menu.ParentId);
                if (parentMenu != null)
                {
                    menu.MenuPath = $"{parentMenu.MenuPath}{menu.Id}/";
                    menu.Level = parentMenu.Level + 1;
                    
                    // 更新父级 IsLeaf 为非叶子
                    if (parentMenu.IsLeaf == 1)
                    {
                        parentMenu.IsLeaf = 0;
                        parentMenu.UpdatedAt = DateTime.Now;
                        await menuRepository.UpdateAsync(parentMenu);
                    }
                }
            }
            else
            {
                menu.MenuPath = $"/{menu.Id}/";
                menu.Level = 1;
            }
            
            menu.UpdatedAt = DateTime.Now;
            await menuRepository.UpdateAsync(menu);
            return (1, 0);
        }
        else
        {
            configure(menu);
            menu.IsBuiltIn = 1;
            
            // 重新计算 Level 和 MenuPath（如果 ParentId 发生变化）
            if (menu.ParentId > 0)
            {
                // 使用仓储查询父级（带租户过滤）
                var parentMenu = await menuRepository.FirstAsync(m => m.Id == menu.ParentId);
                if (parentMenu != null)
                {
                    menu.MenuPath = $"{parentMenu.MenuPath}{menu.Id}/";
                    menu.Level = parentMenu.Level + 1;
                    
                    // 更新父级 IsLeaf 为非叶子
                    if (parentMenu.IsLeaf == 1)
                    {
                        parentMenu.IsLeaf = 0;
                        parentMenu.UpdatedAt = DateTime.Now;
                        await menuRepository.UpdateAsync(parentMenu);
                    }
                }
            }
            else
            {
                menu.MenuPath = $"/{menu.Id}/";
                menu.Level = 1;
            }
            
            menu.UpdatedAt = DateTime.Now;
            menu.UpdatedBy = 900001;
            
            await menuRepository.UpdateAsync(menu);
            return (0, 1);
        }
    }
}
