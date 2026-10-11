// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds
// 文件名称：TaktMenuLevel3SeedData.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt 三级菜单种子数据。
//           在二级菜单（TaktMenuLevel2SeedData）已存在的前提下，按父级 MenuCode 挂载更细粒度的页面或目录：
//           含日常业务页、基础任务页、财务/物料/销售/生产/质量/人力各子模块等。
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
using Takt.Infrastructure.Data.Context;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// Takt 三级菜单种子数据。
/// <para>
/// 父级为 TaktMenuLevel2SeedData 中定义的二级目录或分组（如 ROUTINE_MEETING_CENTER、ROUTINE_HELP_DESK、LOGISTICS_SALES、HUMAN_RESOURCE_TALENT 等）。
/// 页面类型菜单需配置以 <c>:list</c> 结尾的权限串，供 TaktMenuButtonSeedData 生成按钮。
/// 由 TaktMenuSeedData 统一协调调用，不直接注册为 ITaktSeedDataCoordinator。
/// </para>
/// </summary>
public class TaktMenuLevel3SeedData
{
    /// <summary>
    /// 初始化三级菜单种子数据。
    /// <para>
    /// 分块写入：日常业务、基础任务、财务会计/管理会计、物料与采购、销售、生产制造、质量、
    /// 人力资源（组织/人才/人事/考勤/薪酬/绩效/培训）及统计日志/报表/看板等；不存在则创建，存在则更新。
    /// </para>
    /// </summary>
    /// <param name="serviceProvider">服务提供者，用于解析 ITaktRepository{TaktMenu}。</param>
    /// <param name="specifiedTenantCode">租户编码（由协调器传入）。</param>
    /// <returns>元组：(InsertCount, UpdateCount)，分别为本次新增与更新的三级菜单条数。</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? specifiedTenantCode = null)
    {
        var menuRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktMenu>>();
        var seedContext = serviceProvider.GetRequiredService<TaktSeedContext>();

        // 三级菜单:基于二级菜单的ParentId
        // 注意:菜单为租户级实体,由协调器指定租户,必须传入租户编码
        if (string.IsNullOrWhiteSpace(specifiedTenantCode))
        {
            TaktLogger.Warning("未指定租户编码,跳过三级菜单种子数据初始化");
            return (0, 0);
        }
        
        var tenantCode = specifiedTenantCode;

        int insertCount = 0;
        int updateCount = 0;

        // 获取二级父菜单(注意：必须绕过仓储的租户过滤，直接查询)
        // 严格按照二级菜单 SortOrder 顺序排列
        var accountingFinancialMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "ACCOUNTING_FINANCIAL" && m.IsDeleted == 0)
            .FirstAsync();
        var accountingControllingMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "ACCOUNTING_CONTROLLING" && m.IsDeleted == 0)
            .FirstAsync();
        var logisticsMaterialMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "LOGISTICS_MATERIALS" && m.IsDeleted == 0)
            .FirstAsync();
        var logisticsProcurementMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "LOGISTICS_PROCUREMENT" && m.IsDeleted == 0)
            .FirstAsync();
        var manufacturingMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "LOGISTICS_MANUFACTURING" && m.IsDeleted == 0)
            .FirstAsync();
        var logisticsQualityMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "LOGISTICS_QUALITY" && m.IsDeleted == 0)
            .FirstAsync();
        var logisticsServiceMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "LOGISTICS_CUSTOMER_SERVICE" && m.IsDeleted == 0)
            .FirstAsync();
        var logisticsMaintenanceMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "LOGISTICS_MAINTENANCE" && m.IsDeleted == 0)
            .FirstAsync();
        var logisticsSalesMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "LOGISTICS_SALES" && m.IsDeleted == 0)
            .FirstAsync();
        var logisticsSerialMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "LOGISTICS_SERIAL" && m.IsDeleted == 0)
            .FirstAsync();
        var hrOrganizationMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "HUMAN_RESOURCE_ORGANIZATION" && m.IsDeleted == 0)
            .FirstAsync();
        var hrPersonnelMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "HUMAN_RESOURCE_PERSONNEL" && m.IsDeleted == 0)
            .FirstAsync();
        var hrAttendanceMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "HUMAN_RESOURCE_ATTENDANCE" && m.IsDeleted == 0)
            .FirstAsync();
        var hrCompensationMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "HUMAN_RESOURCE_COMPENSATION" && m.IsDeleted == 0)
            .FirstAsync();
        var hrBenefitsMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "HUMAN_RESOURCE_BENEFITS" && m.IsDeleted == 0)
            .FirstAsync();
        var hrPerformanceMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "HUMAN_RESOURCE_PERFORMANCE" && m.IsDeleted == 0)
            .FirstAsync();
        var hrTrainingMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "HUMAN_RESOURCE_TRAINING" && m.IsDeleted == 0)
            .FirstAsync();
        var hrTalentMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "HUMAN_RESOURCE_TALENT" && m.IsDeleted == 0)
            .FirstAsync();
        var statisticsQuickQueryMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "STATISTICS_QUICK_QUERY" && m.IsDeleted == 0)
            .FirstAsync();
        var statisticsLoggingMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "STATISTICS_LOGGING" && m.IsDeleted == 0)
            .FirstAsync();
        var routineHelpDeskMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "ROUTINE_HELP_DESK" && m.IsDeleted == 0)
            .FirstAsync();
        var routineMeetingCenterMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "ROUTINE_MEETING_CENTER" && m.IsDeleted == 0)
            .FirstAsync();

        // ========== 管理会计下的三级菜单 (ACCOUNTING_FINANCIAL) ==========
        if (accountingFinancialMenu != null)
        {
            var (insertAF1, updateAF1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ACCOUNTING_FINANCIAL_ACCOUNT_TITLE", menu =>
            {
                menu.MenuName = "会计科目";
                menu.MenuCode = "ACCOUNTING_FINANCIAL_ACCOUNT_TITLE";
                menu.I18nKey = "menu.accounting.financial.account.title";
                menu.Icon = "RiBookletLine";
                menu.ParentId = accountingFinancialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "accounting:financial:account:title:list";
                menu.RoutePath = "/accounting/financial/account-title";
                menu.ComponentPath = "accounting/financial/account-title/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAF1;
            updateCount += updateAF1;

            var (insertAF2, updateAF2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ACCOUNTING_FINANCIAL_ASSET", menu =>
            {
                menu.MenuName = "固定资产";
                menu.MenuCode = "ACCOUNTING_FINANCIAL_ASSET";
                menu.I18nKey = "menu.accounting.financial.asset";
                menu.Icon = "RiBuilding2Line";
                menu.ParentId = accountingFinancialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "accounting:financial:asset:list";
                menu.RoutePath = "/accounting/financial/asset";
                menu.ComponentPath = "accounting/financial/asset/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAF2;
            updateCount += updateAF2;

            var (insertAF3, updateAF3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ACCOUNTING_FINANCIAL_COUNTERSIGN", menu =>
            {
                menu.MenuName = "会签管理";
                menu.MenuCode = "ACCOUNTING_FINANCIAL_COUNTERSIGN";
                menu.I18nKey = "menu.accounting.financial.countersign";
                menu.Icon = "RiFileCheckLine";
                menu.ParentId = accountingFinancialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "accounting:financial:countersign:list";
                menu.RoutePath = "/accounting/financial/countersign";
                menu.ComponentPath = "accounting/financial/countersign/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAF3;
            updateCount += updateAF3;

            var (insertAF3Ex, updateAF3Ex) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ACCOUNTING_FINANCIAL_EXPENSE", menu =>
            {
                menu.MenuName = "费用管理";
                menu.MenuCode = "ACCOUNTING_FINANCIAL_EXPENSE";
                menu.I18nKey = "menu.accounting.financial.expense";
                menu.Icon = "RiMoneyCnyBoxLine";
                menu.ParentId = accountingFinancialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "accounting:financial:expense:list";
                menu.RoutePath = "/accounting/financial/expense";
                menu.ComponentPath = "accounting/financial/expense/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAF3Ex;
            updateCount += updateAF3Ex;

            var (insertAF4, updateAF4) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ACCOUNTING_FINANCIAL_EXCHANGE_RATE", menu =>
            {
                menu.MenuName = "汇率维护";
                menu.MenuCode = "ACCOUNTING_FINANCIAL_EXCHANGE_RATE";
                menu.I18nKey = "menu.accounting.financial.exchange.rate";
                menu.Icon = "RiExchangeLine";
                menu.ParentId = accountingFinancialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "accounting:financial:exchange:rate:list";
                menu.RoutePath = "/accounting/financial/exchange-rate";
                menu.ComponentPath = "accounting/financial/exchange-rate/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAF4;
            updateCount += updateAF4;

            var (insertAF5, updateAF5) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ACCOUNTING_FINANCIAL_BALANCE_SHEET", menu =>
            {
                menu.MenuName = "资产负债表";
                menu.MenuCode = "ACCOUNTING_FINANCIAL_BALANCE_SHEET";
                menu.I18nKey = "menu.accounting.financial.balance.sheet";
                menu.Icon = "RiFileList3Line";
                menu.ParentId = accountingFinancialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "accounting:financial:balance:sheet:list";
                menu.RoutePath = "/accounting/financial/balance-sheet";
                menu.ComponentPath = "accounting/financial/balance-sheet/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAF5;
            updateCount += updateAF5;

            var (insertAF6, updateAF6) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ACCOUNTING_FINANCIAL_PROFIT_LOSS", menu =>
            {
                menu.MenuName = "利润表";
                menu.MenuCode = "ACCOUNTING_FINANCIAL_PROFIT_LOSS";
                menu.I18nKey = "menu.accounting.financial.profit.loss";
                menu.Icon = "RiLineChartLine";
                menu.ParentId = accountingFinancialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "accounting:financial:profit:loss:list";
                menu.RoutePath = "/accounting/financial/profit-loss";
                menu.ComponentPath = "accounting/financial/profit-loss/index";
                menu.SortOrder = 7;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAF6;
            updateCount += updateAF6;

            var (insertAF7, updateAF7) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ACCOUNTING_FINANCIAL_PURCHASE_SALES_INVENTORY", menu =>
            {
                menu.MenuName = "进销存表";
                menu.MenuCode = "ACCOUNTING_FINANCIAL_PURCHASE_SALES_INVENTORY";
                menu.I18nKey = "menu.accounting.financial.purchase.sales.inventory";
                menu.Icon = "RiStackLine";
                menu.ParentId = accountingFinancialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "accounting:financial:purchase:sales:inventory:list";
                menu.RoutePath = "/accounting/financial/purchase-sales-inventory";
                menu.ComponentPath = "accounting/financial/purchase-sales-inventory/index";
                menu.SortOrder = 8;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAF7;
            updateCount += updateAF7;

            var (insertAF8, updateAF8) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ACCOUNTING_FINANCIAL_BUDGET_ACTUAL", menu =>
            {
                menu.MenuName = "预算实绩";
                menu.MenuCode = "ACCOUNTING_FINANCIAL_BUDGET_ACTUAL";
                menu.I18nKey = "menu.accounting.financial.budget.actual";
                menu.Icon = "RiBarChartBoxLine";
                menu.ParentId = accountingFinancialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "accounting:financial:budget:actual:list";
                menu.RoutePath = "/accounting/financial/budget-actual";
                menu.ComponentPath = "accounting/financial/budget-actual/index";
                menu.SortOrder = 9;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAF8;
            updateCount += updateAF8;

            var (insertAF9, updateAF9) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ACCOUNTING_FINANCIAL_COMPANY", menu =>
            {
                menu.MenuName = "公司信息";
                menu.MenuCode = "ACCOUNTING_FINANCIAL_COMPANY";
                menu.I18nKey = "menu.accounting.financial.company";
                menu.Icon = "RiBuildingLine";
                menu.ParentId = accountingFinancialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "accounting:financial:company:list";
                menu.RoutePath = "/accounting/financial/company";
                menu.ComponentPath = "accounting/financial/company/index";
                menu.SortOrder = 10;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAF9;
            updateCount += updateAF9;

            var (insertAF10, updateAF10) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ACCOUNTING_FINANCIAL_PERIOD", menu =>
            {
                menu.MenuName = "财务期间";
                menu.MenuCode = "ACCOUNTING_FINANCIAL_PERIOD";
                menu.I18nKey = "menu.accounting.financial.period";
                menu.Icon = "RiCalendarCheckLine";
                menu.ParentId = accountingFinancialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "accounting:financial:period:list";
                menu.RoutePath = "/accounting/financial/period";
                menu.ComponentPath = "accounting/financial/period/index";
                menu.SortOrder = 11;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAF10;
            updateCount += updateAF10;

            var (insertAFBank, updateAFBank) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ACCOUNTING_FINANCIAL_BANK", menu =>
            {
                menu.MenuName = "银行信息";
                menu.MenuCode = "ACCOUNTING_FINANCIAL_BANK";
                menu.I18nKey = "menu.accounting.financial.bank";
                menu.Icon = "RiBankLine";
                menu.ParentId = accountingFinancialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "accounting:financial:bank:list";
                menu.RoutePath = "/accounting/financial/bank";
                menu.ComponentPath = "accounting/financial/bank/index";
                menu.SortOrder = 12;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAFBank;
            updateCount += updateAFBank;
        }

        // ========== 控制会计下的三级菜单 (ACCOUNTING_CONTROLLING) ==========
        if (accountingControllingMenu != null)
        {
            var (insertAC1, updateAC1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ACCOUNTING_CONTROLLING_PROFIT_CENTER", menu =>
            {
                menu.MenuName = "利润中心";
                menu.MenuCode = "ACCOUNTING_CONTROLLING_PROFIT_CENTER";
                menu.I18nKey = "menu.accounting.controlling.profit.center";
                menu.Icon = "RiMoneyDollarCircleLine";
                menu.ParentId = accountingControllingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "accounting:controlling:profit:center:list";
                menu.RoutePath = "/accounting/controlling/profit-center";
                menu.ComponentPath = "accounting/controlling/profit-center/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAC1;
            updateCount += updateAC1;

            var (insertAC2, updateAC2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ACCOUNTING_CONTROLLING_COST_CENTER", menu =>
            {
                menu.MenuName = "成本中心";
                menu.MenuCode = "ACCOUNTING_CONTROLLING_COST_CENTER";
                menu.I18nKey = "menu.accounting.controlling.cost.center";
                menu.Icon = "RiPieChart2Line";
                menu.ParentId = accountingControllingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "accounting:controlling:cost:center:list";
                menu.RoutePath = "/accounting/controlling/cost-center";
                menu.ComponentPath = "accounting/controlling/cost-center/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAC2;
            updateCount += updateAC2;

            var (insertAC3, updateAC3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ACCOUNTING_CONTROLLING_COST_ELEMENT", menu =>
            {
                menu.MenuName = "成本要素";
                menu.MenuCode = "ACCOUNTING_CONTROLLING_COST_ELEMENT";
                menu.I18nKey = "menu.accounting.controlling.cost.element";
                menu.Icon = "RiListCheck";
                menu.ParentId = accountingControllingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "accounting:controlling:cost:element:list";
                menu.RoutePath = "/accounting/controlling/cost-element";
                menu.ComponentPath = "accounting/controlling/cost-element/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAC3;
            updateCount += updateAC3;

            var (insertAC4, updateAC4) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ACCOUNTING_CONTROLLING_STANDARD_WAGE_RATE", menu =>
            {
                menu.MenuName = "标准工资率";
                menu.MenuCode = "ACCOUNTING_CONTROLLING_STANDARD_WAGE_RATE";
                menu.I18nKey = "menu.accounting.controlling.standard.wage.rate";
                menu.Icon = "RiCalculatorLine";
                menu.ParentId = accountingControllingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "accounting:controlling:standard:wage:rate:list";
                menu.RoutePath = "/accounting/controlling/standard-wage-rate";
                menu.ComponentPath = "accounting/controlling/standard-wage-rate/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAC4;
            updateCount += updateAC4;
        }

        // ========== 物料管理下的三级菜单 (LOGISTICS_MATERIALS) ==========
        if (logisticsMaterialMenu != null)
        {
            // TaktPlant（工厂主数据）≠ TaktMaterialPlant（工厂物料）；MenuCode 不得混用 PLANT / MATERIAL_PLANT
            var (insertLM1, updateLM1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MATERIALS_PLANT", menu =>
            {
                menu.MenuName = "工厂信息";
                menu.MenuCode = "LOGISTICS_MATERIALS_PLANT";
                menu.I18nKey = "menu.logistics.materials.plant";
                menu.Icon = "RiBuilding2Line";
                menu.ParentId = logisticsMaterialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:materials:plant:list";
                menu.RoutePath = "/logistics/materials/plant";
                menu.ComponentPath = "logistics/materials/plant/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLM1;
            updateCount += updateLM1;

            // TaktGeneralMaterial：全局物料
            var (insertLM2, updateLM2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MATERIALS_GENERAL_MATERIAL", menu =>
            {
                menu.MenuName = "全局物料";
                menu.MenuCode = "LOGISTICS_MATERIALS_GENERAL_MATERIAL";
                menu.I18nKey = "menu.logistics.materials.general.material";
                menu.Icon = "RiArchiveLine";
                menu.ParentId = logisticsMaterialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:materials:general:material:list";
                menu.RoutePath = "/logistics/materials/general-material";
                menu.ComponentPath = "logistics/materials/general-material/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLM2;
            updateCount += updateLM2;

 // TaktMaterialDescription；Permission 与控制器/前端一致：material:description
            var (insertLM2d, updateLM2d) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MATERIALS_MATERIAL_DESCRIPTION", menu =>
            {
                menu.MenuName = "物料描述";
                menu.MenuCode = "LOGISTICS_MATERIALS_MATERIAL_DESCRIPTION";
                menu.I18nKey = "menu.logistics.materials.material.description";
                menu.Icon = "RiFileTextLine";
                menu.ParentId = logisticsMaterialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:materials:material:description:list";
                menu.RoutePath = "/logistics/materials/material-description";
                menu.ComponentPath = "logistics/materials/material-description/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLM2d;
            updateCount += updateLM2d;

            // TaktMaterialPlant ≠ TaktPlant；Permission 与控制器一致：material:plant（禁止挂在 generalmaterial 下）
            var (insertLM3, updateLM3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MATERIALS_MATERIAL_PLANT", menu =>
            {
                menu.MenuName = "工厂物料";
                menu.MenuCode = "LOGISTICS_MATERIALS_MATERIAL_PLANT";
                menu.I18nKey = "menu.logistics.materials.material.plant";
                menu.Icon = "RiArchiveStackLine";
                menu.ParentId = logisticsMaterialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:materials:material:plant:list";
                menu.RoutePath = "/logistics/materials/material-plant";
                menu.ComponentPath = "logistics/materials/material-plant/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLM3;
            updateCount += updateLM3;

            // TaktWarehouse 主表；TaktStorageLocation 为子表，在仓库页右栏维护，无独立菜单
            var (insertLM3Wh, updateLM3Wh) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MATERIALS_WAREHOUSE", menu =>
            {
                menu.MenuName = "仓库信息";
                menu.MenuCode = "LOGISTICS_MATERIALS_WAREHOUSE";
                menu.I18nKey = "menu.logistics.materials.warehouse";
                menu.Icon = "RiStore2Line";
                menu.ParentId = logisticsMaterialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:materials:warehouse:list";
                menu.RoutePath = "/logistics/materials/warehouse";
                menu.ComponentPath = "logistics/materials/warehouse/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLM3Wh;
            updateCount += updateLM3Wh;

            var (insertLM4, updateLM4) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MATERIALS_PACKAGING_MATERIAL", menu =>
            {
                menu.MenuName = "包装物料";
                menu.MenuCode = "LOGISTICS_MATERIALS_PACKAGING_MATERIAL";
                menu.I18nKey = "menu.logistics.materials.packaging.material";
                menu.Icon = "RiBox3Line";
                menu.ParentId = logisticsMaterialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:materials:packaging:material:list";
                menu.RoutePath = "/logistics/materials/packaging-material";
                menu.ComponentPath = "logistics/materials/packaging-material/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLM4;
            updateCount += updateLM4;

            var (insertLM5, updateLM5) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MATERIALS_MODEL_DESTINATION", menu =>
            {
                menu.MenuName = "机种仕向";
                menu.MenuCode = "LOGISTICS_MATERIALS_MODEL_DESTINATION";
                menu.I18nKey = "menu.logistics.materials.model.destination";
                menu.Icon = "RiEarthLine";
                menu.ParentId = logisticsMaterialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:materials:model:destination:list";
                menu.RoutePath = "/logistics/materials/model-destination";
                menu.ComponentPath = "logistics/materials/model-destination/index";
                menu.SortOrder = 7;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLM5;
            updateCount += updateLM5;

            // TaktMaterialDocument：物料凭证
            var (insertLM7, updateLM7) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MATERIALS_MATERIAL_DOCUMENT", menu =>
            {
                menu.MenuName = "物料凭证";
                menu.MenuCode = "LOGISTICS_MATERIALS_MATERIAL_DOCUMENT";
                menu.I18nKey = "menu.logistics.materials.material.document";
                menu.Icon = "RiExchangeLine";
                menu.ParentId = logisticsMaterialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:materials:material:document:list";
                menu.RoutePath = "/logistics/materials/material-document";
                menu.ComponentPath = "logistics/materials/material-document/index";
                menu.SortOrder = 8;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLM7;
            updateCount += updateLM7;

            // 物料入库推移（物料凭证行 SupplierCode 非空 × 过账年月）
            var (insertLMInTrend, updateLMInTrend) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MATERIALS_MATERIAL_INBOUND_TREND", menu =>
            {
                menu.MenuName = "物料入库推移";
                menu.MenuCode = "LOGISTICS_MATERIALS_MATERIAL_INBOUND_TREND";
                menu.I18nKey = "menu.logistics.materials.material.inbound.trend";
                menu.Icon = "RiLineChartLine";
                menu.ParentId = logisticsMaterialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:materials:material:inbound:trend:list";
                menu.RoutePath = "/logistics/materials/material-inbound-trend";
                menu.ComponentPath = "logistics/materials/inbound-actual-trend/index";
                menu.SortOrder = 9;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLMInTrend;
            updateCount += updateLMInTrend;

            // 物料出库推移（物料凭证行 CustomerCode 非空 × 过账年月）
            var (insertLMOutTrend, updateLMOutTrend) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MATERIALS_MATERIAL_OUTBOUND_TREND", menu =>
            {
                menu.MenuName = "物料出库推移";
                menu.MenuCode = "LOGISTICS_MATERIALS_MATERIAL_OUTBOUND_TREND";
                menu.I18nKey = "menu.logistics.materials.material.outbound.trend";
                menu.Icon = "RiLineChartLine";
                menu.ParentId = logisticsMaterialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:materials:material:outbound:trend:list";
                menu.RoutePath = "/logistics/materials/material-outbound-trend";
                menu.ComponentPath = "logistics/materials/outbound-actual-trend/index";
                menu.SortOrder = 10;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLMOutTrend;
            updateCount += updateLMOutTrend;

            // TaktMaterialMovingPrice：按工厂/期间/物料/评估类别维护移动价格
            var (insertLM8, updateLM8) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MATERIALS_MATERIAL_MOVING_PRICE", menu =>
            {
                menu.MenuName = "移动价格";
                menu.MenuCode = "LOGISTICS_MATERIALS_MATERIAL_MOVING_PRICE";
                menu.I18nKey = "menu.logistics.materials.material.moving.price";
                menu.Icon = "RiPriceTag3Line";
                menu.ParentId = logisticsMaterialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:materials:material:moving:price:list";
                menu.RoutePath = "/logistics/materials/material-moving-price";
                menu.ComponentPath = "logistics/materials/material-moving-price/index";
                menu.SortOrder = 11;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLM8;
            updateCount += updateLM8;

            // 物料月移动价格推移分析（物料×月份转置）
            var (insertLM9, updateLM9) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MATERIALS_MATERIAL_MOVING_TREND", menu =>
            {
                menu.MenuName = "移动价格推移";
                menu.MenuCode = "LOGISTICS_MATERIALS_MATERIAL_MOVING_TREND";
                menu.I18nKey = "menu.logistics.materials.material.moving.trend";
                menu.Icon = "RiLineChartLine";
                menu.ParentId = logisticsMaterialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:materials:material:moving:trend:list";
                menu.RoutePath = "/logistics/materials/material-moving-trend";
                menu.ComponentPath = "logistics/materials/material-moving-trend/index";
                menu.SortOrder = 12;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLM9;
            updateCount += updateLM9;

            // TaktInventoryReserve：存货跌价准备
            var (insertLM10, updateLM10) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MATERIALS_INVENTORY_RESERVE", menu =>
            {
                menu.MenuName = "存货跌价准备";
                menu.MenuCode = "LOGISTICS_MATERIALS_INVENTORY_RESERVE";
                menu.I18nKey = "menu.logistics.materials.inventory.reserve";
                menu.Icon = "RiRefund2Line";
                menu.ParentId = logisticsMaterialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:materials:inventory:reserve:list";
                menu.RoutePath = "/logistics/materials/inventory-reserve";
                menu.ComponentPath = "logistics/materials/inventory-reserve/index";
                menu.SortOrder = 13;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLM10;
            updateCount += updateLM10;

            var (insertLM3Mg, updateLM3Mg) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MATERIALS_MATERIAL_GROUP", menu =>
            {
                menu.MenuName = "物料组";
                menu.MenuCode = "LOGISTICS_MATERIALS_MATERIAL_GROUP";
                menu.I18nKey = "menu.logistics.materials.material.group";
                menu.Icon = "RiStackLine";
                menu.ParentId = logisticsMaterialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:materials:material:group:list";
                menu.RoutePath = "/logistics/materials/material-group";
                menu.ComponentPath = "logistics/materials/material-group/index";
                menu.SortOrder = 14;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLM3Mg;
            updateCount += updateLM3Mg;
        }

        // ========== 采购管理下的三级菜单 (LOGISTICS_PROCUREMENT) ==========
        // 顺序：采购预测/询价/申请/订单/信息记录/价格推移/PO偏离PIR/发票/实绩推移/供应商/经销商/制造商物料/货源清单/采购组
        if (logisticsProcurementMenu != null)
        {
            var (insert07Fc, update07Fc) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_PROCUREMENT_PURCHASE_FORECAST", menu =>
            {
                menu.MenuName = "采购预测";
                menu.MenuCode = "LOGISTICS_PROCUREMENT_PURCHASE_FORECAST";
                menu.I18nKey = "menu.logistics.procurement.purchase.forecast";
                menu.Icon = "RiLineChartLine";
                menu.ParentId = logisticsProcurementMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:procurement:purchase:forecast:list";
                menu.RoutePath = "/logistics/procurement/purchase-forecast";
                menu.ComponentPath = "logistics/procurement/purchase-forecast/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert07Fc;
            updateCount += update07Fc;

            var (insert07Inq, update07Inq) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_PROCUREMENT_PURCHASE_INQUIRY", menu =>
            {
                menu.MenuName = "采购询价";
                menu.MenuCode = "LOGISTICS_PROCUREMENT_PURCHASE_INQUIRY";
                menu.I18nKey = "menu.logistics.procurement.purchase.inquiry";
                menu.Icon = "RiQuestionAnswerLine";
                menu.ParentId = logisticsProcurementMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:procurement:purchase:inquiry:list";
                menu.RoutePath = "/logistics/procurement/purchase-inquiry";
                menu.ComponentPath = "logistics/procurement/purchase-inquiry/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert07Inq;
            updateCount += update07Inq;

            var (insert07, update07) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_PROCUREMENT_PURCHASE_REQUEST", menu =>
            {
                menu.MenuName = "采购申请";
                menu.MenuCode = "LOGISTICS_PROCUREMENT_PURCHASE_REQUEST";
                menu.I18nKey = "menu.logistics.procurement.purchase.request";
                menu.Icon = "RiFileAddLine";
                menu.ParentId = logisticsProcurementMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:procurement:purchase:request:list";
                menu.RoutePath = "/logistics/procurement/purchase-request";
                menu.ComponentPath = "logistics/procurement/purchase-request/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert07;
            updateCount += update07;

            var (insert08, update08) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_PROCUREMENT_PURCHASE_ORDER", menu =>
            {
                menu.MenuName = "采购订单";
                menu.MenuCode = "LOGISTICS_PROCUREMENT_PURCHASE_ORDER";
                menu.I18nKey = "menu.logistics.procurement.purchase.order";
                menu.Icon = "RiListOrdered";
                menu.ParentId = logisticsProcurementMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:procurement:purchase:order:list";
                menu.RoutePath = "/logistics/procurement/purchase-order";
                menu.ComponentPath = "logistics/procurement/purchase-order/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert08;
            updateCount += update08;

            var (insert08Price, update08Price) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_PROCUREMENT_PURCHASE_INFO_RECORD", menu =>
            {
                menu.MenuName = "采购信息记录";
                menu.MenuCode = "LOGISTICS_PROCUREMENT_PURCHASE_INFO_RECORD";
                menu.I18nKey = "menu.logistics.procurement.purchase.info.record";
                menu.Icon = "RiPriceTag3Line";
                menu.ParentId = logisticsProcurementMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:procurement:purchase:info:record:list";
                menu.RoutePath = "/logistics/procurement/purchase-info-record";
                menu.ComponentPath = "logistics/procurement/purchase-info-record/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert08Price;
            updateCount += update08Price;

            var (insert08PriceTrend, update08PriceTrend) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_PROCUREMENT_PURCHASE_PRICE_TREND", menu =>
            {
                menu.MenuName = "采购价格推移";
                menu.MenuCode = "LOGISTICS_PROCUREMENT_PURCHASE_PRICE_TREND";
                menu.I18nKey = "menu.logistics.procurement.purchase.price.trend";
                menu.Icon = "RiLineChartLine";
                menu.ParentId = logisticsProcurementMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:procurement:purchase:price:trend:list";
                menu.RoutePath = "/logistics/procurement/purchase-price-trend";
                menu.ComponentPath = "logistics/procurement/purchase-price-trend/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert08PriceTrend;
            updateCount += update08PriceTrend;

            var (insert08PirDev, update08PirDev) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_PROCUREMENT_PO_PRICE_PIR_DEVIATION", menu =>
            {
                menu.MenuName = "PO价偏离PIR";
                menu.MenuCode = "LOGISTICS_PROCUREMENT_PO_PRICE_PIR_DEVIATION";
                menu.I18nKey = "menu.logistics.procurement.po.price.pir.deviation";
                menu.Icon = "RiErrorWarningLine";
                menu.ParentId = logisticsProcurementMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:procurement:po:price:pir:deviation:list";
                menu.RoutePath = "/logistics/procurement/po-price-pir-deviation";
                menu.ComponentPath = "logistics/procurement/po-price-pir-deviation/index";
                menu.SortOrder = 7;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert08PirDev;
            updateCount += update08PirDev;

            var (insert09, update09) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_PROCUREMENT_PURCHASE_INVOICE", menu =>
            {
                menu.MenuName = "采购发票";
                menu.MenuCode = "LOGISTICS_PROCUREMENT_PURCHASE_INVOICE";
                menu.I18nKey = "menu.logistics.procurement.purchase.invoice";
                menu.Icon = "RiFilePaper2Line";
                menu.ParentId = logisticsProcurementMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:procurement:purchase:invoice:list";
                menu.RoutePath = "/logistics/procurement/purchase-invoice";
                menu.ComponentPath = "logistics/procurement/purchase-invoice/index";
                menu.SortOrder = 8;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert09;
            updateCount += update09;

            var (insert09Actual, update09Actual) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_PROCUREMENT_PURCHASE_ACTUAL_TREND", menu =>
            {
                menu.MenuName = "采购实绩推移";
                menu.MenuCode = "LOGISTICS_PROCUREMENT_PURCHASE_ACTUAL_TREND";
                menu.I18nKey = "menu.logistics.procurement.purchase.actual.trend";
                menu.Icon = "RiLineChartLine";
                menu.ParentId = logisticsProcurementMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:procurement:purchase:actual:trend:list";
                menu.RoutePath = "/logistics/procurement/purchase-actual-trend";
                menu.ComponentPath = "logistics/procurement/purchase-actual-trend/index";
                menu.SortOrder = 9;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert09Actual;
            updateCount += update09Actual;

            var (insert03, update03) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_PROCUREMENT_SUPPLIER", menu =>
            {
                menu.MenuName = "供应商";
                menu.MenuCode = "LOGISTICS_PROCUREMENT_SUPPLIER";
                menu.I18nKey = "menu.logistics.procurement.supplier";
                menu.Icon = "RiTruckLine";
                menu.ParentId = logisticsProcurementMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:procurement:supplier:list";
                menu.RoutePath = "/logistics/procurement/supplier";
                menu.ComponentPath = "logistics/procurement/supplier/index";
                menu.SortOrder = 10;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert03;
            updateCount += update03;

            var (insert04, update04) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_PROCUREMENT_VENDOR", menu =>
            {
                menu.MenuName = "经销商";
                menu.MenuCode = "LOGISTICS_PROCUREMENT_VENDOR";
                menu.I18nKey = "menu.logistics.procurement.vendor";
                menu.Icon = "RiRegisteredLine";
                menu.ParentId = logisticsProcurementMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:procurement:vendor:list";
                menu.RoutePath = "/logistics/procurement/vendor";
                menu.ComponentPath = "logistics/procurement/vendor/index";
                menu.SortOrder = 11;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert04;
            updateCount += update04;

            var (insert04m, update04m) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_PROCUREMENT_MANUFACTURER_MATERIAL", menu =>
            {
                menu.MenuName = "制造商物料";
                menu.MenuCode = "LOGISTICS_PROCUREMENT_MANUFACTURER_MATERIAL";
                menu.I18nKey = "menu.logistics.procurement.manufacturer.material";
                menu.Icon = "RiBuilding4Line";
                menu.ParentId = logisticsProcurementMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:procurement:manufacturer:material:list";
                menu.RoutePath = "/logistics/procurement/manufacturer-material";
                menu.ComponentPath = "logistics/procurement/manufacturer-material/index";
                menu.SortOrder = 12;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert04m;
            updateCount += update04m;

            var (insert06, update06) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_PROCUREMENT_SOURCE_OF_SUPPLY", menu =>
            {
                menu.MenuName = "货源清单";
                menu.MenuCode = "LOGISTICS_PROCUREMENT_SOURCE_OF_SUPPLY";
                menu.I18nKey = "menu.logistics.procurement.source.of.supply";
                menu.Icon = "RiLinksLine";
                menu.ParentId = logisticsProcurementMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:procurement:source:of:supply:list";
                menu.RoutePath = "/logistics/procurement/source-of-supply";
                menu.ComponentPath = "logistics/procurement/source-of-supply/index";
                menu.SortOrder = 13;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert06;
            updateCount += update06;

            var (insertPg, updatePg) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_PROCUREMENT_PURCHASE_GROUP", menu =>
            {
                menu.MenuName = "采购组";
                menu.MenuCode = "LOGISTICS_PROCUREMENT_PURCHASE_GROUP";
                menu.I18nKey = "menu.logistics.procurement.purchase.group";
                menu.Icon = "RiGroupLine";
                menu.ParentId = logisticsProcurementMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:procurement:purchase:group:list";
                menu.RoutePath = "/logistics/procurement/purchase-group";
                menu.ComponentPath = "logistics/procurement/purchase-group/index";
                menu.SortOrder = 14;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertPg;
            updateCount += updatePg;
        }

        // ========== 生产执行下的三级菜单 (LOGISTICS_MANUFACTURING) ==========
        // 顺序：BOM管理 / 工程设变 / MDS计划 / MPS计划 / MRP计划 / APS排程 / OPH管理 / 生产不良 / OPH工数 / SOP管理
        if (manufacturingMenu != null)
        {
            var (insertMFG1, updateMFG1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MANUFACTURING_BOM", menu =>
            {
                menu.MenuName = "BOM管理";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_BOM";
                menu.I18nKey = "menu.logistics.manufacturing.bom._self";
                menu.Icon = "RiTreeLine";
                menu.ParentId = manufacturingMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/logistics/manufacturing/bom";
                menu.ComponentPath = "logistics/manufacturing/bom";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertMFG1;
            updateCount += updateMFG1;

            var (insertMFG4, updateMFG4) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE", menu =>
            {
                menu.MenuName = "工程设变";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE";
                menu.I18nKey = "menu.logistics.manufacturing.engineering.change._self";
                menu.Icon = "RiEditCircleLine";
                menu.ParentId = manufacturingMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/logistics/manufacturing/engineering-change";
                menu.ComponentPath = "logistics/manufacturing/engineering-change";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertMFG4;
            updateCount += updateMFG4;

            var (insertMFG1b, updateMFG1b) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MANUFACTURING_MDS", menu =>
            {
                menu.MenuName = "MDS计划";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_MDS";
                menu.I18nKey = "menu.logistics.manufacturing.mds._self";
                menu.Icon = "RiBarChartGroupedLine";
                menu.ParentId = manufacturingMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/logistics/manufacturing/mds";
                menu.ComponentPath = "logistics/manufacturing/mds";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertMFG1b;
            updateCount += updateMFG1b;

            var (insertMFG1c, updateMFG1c) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MANUFACTURING_MPS", menu =>
            {
                menu.MenuName = "MPS计划";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_MPS";
                menu.I18nKey = "menu.logistics.manufacturing.mps._self";
                menu.Icon = "RiCalendarTodoLine";
                menu.ParentId = manufacturingMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/logistics/manufacturing/mps";
                menu.ComponentPath = "logistics/manufacturing/mps";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertMFG1c;
            updateCount += updateMFG1c;

            var (insertMFG2, updateMFG2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MANUFACTURING_MRP", menu =>
            {
                menu.MenuName = "MRP计划";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_MRP";
                menu.I18nKey = "menu.logistics.manufacturing.mrp._self";
                menu.Icon = "RiFlowChart";
                menu.ParentId = manufacturingMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/logistics/manufacturing/mrp";
                menu.ComponentPath = "logistics/manufacturing/mrp";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertMFG2;
            updateCount += updateMFG2;

            var (insertMFG3, updateMFG3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MANUFACTURING_APS", menu =>
            {
                menu.MenuName = "APS排程";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_APS";
                menu.I18nKey = "menu.logistics.manufacturing.aps._self";
                menu.Icon = "RiCalendarScheduleLine";
                menu.ParentId = manufacturingMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/logistics/manufacturing/aps";
                menu.ComponentPath = "logistics/manufacturing/aps";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertMFG3;
            updateCount += updateMFG3;

            var (insertMFG5, updateMFG5) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MANUFACTURING_OUTPUT", menu =>
            {
                menu.MenuName = "OPH管理";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_OUTPUT";
                menu.I18nKey = "menu.logistics.manufacturing.output._self";
                menu.Icon = "RiBarChart2Line";
                menu.ParentId = manufacturingMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/logistics/manufacturing/output";
                menu.ComponentPath = "logistics/manufacturing/output";
                menu.SortOrder = 7;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertMFG5;
            updateCount += updateMFG5;

            var (insertMFG6, updateMFG6) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MANUFACTURING_DEFECT", menu =>
            {
                menu.MenuName = "生产不良";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_DEFECT";
                menu.I18nKey = "menu.logistics.manufacturing.defect._self";
                menu.Icon = "RiErrorWarningLine";
                menu.ParentId = manufacturingMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/logistics/manufacturing/defect";
                menu.ComponentPath = "logistics/manufacturing/defect";
                menu.SortOrder = 8;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertMFG6;
            updateCount += updateMFG6;

            var (insertMFG5b, updateMFG5b) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MANUFACTURING_LABOR_HOUR", menu =>
            {
                menu.MenuName = "OPH工数";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_LABOR_HOUR";
                menu.I18nKey = "menu.logistics.manufacturing.labor.hour._self";
                menu.Icon = "RiTimerLine";
                menu.ParentId = manufacturingMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/logistics/manufacturing/labor-hour";
                menu.ComponentPath = "logistics/manufacturing/labor-hour";
                menu.SortOrder = 9;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertMFG5b;
            updateCount += updateMFG5b;

            var (insertMFG7, updateMFG7) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MANUFACTURING_SOP", menu =>
            {
                menu.MenuName = "SOP管理";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_SOP";
                menu.I18nKey = "menu.logistics.manufacturing.sop._self";
                menu.Icon = "RiBookOpenLine";
                menu.ParentId = manufacturingMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/logistics/manufacturing/sop";
                menu.ComponentPath = "logistics/manufacturing/sop";
                menu.SortOrder = 10;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertMFG7;
            updateCount += updateMFG7;
        }

        // ========== 质量管理下的三级菜单 (LOGISTICS_QUALITY) ==========
        // 顺序：质量业务 / 客诉管理 / 品质成本
        if (logisticsQualityMenu != null)
        {
            var (insertLQ2, updateLQ2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_QUALITY_OPERATION", menu =>
            {
                menu.MenuName = "质量业务";
                menu.MenuCode = "LOGISTICS_QUALITY_OPERATION";
                menu.I18nKey = "menu.logistics.quality.operation._self";
                menu.Icon = "RiShieldCheckLine";
                menu.ParentId = logisticsQualityMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/logistics/quality/operation";
                menu.ComponentPath = "logistics/quality/operation";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLQ2;
            updateCount += updateLQ2;

            var (insertLQ3, updateLQ3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_QUALITY_COMPLAINT", menu =>
            {
                menu.MenuName = "客诉管理";
                menu.MenuCode = "LOGISTICS_QUALITY_COMPLAINT";
                menu.I18nKey = "menu.logistics.quality.complaint._self";
                menu.Icon = "RiMessage3Line";
                menu.ParentId = logisticsQualityMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/logistics/quality/complaint";
                menu.ComponentPath = "logistics/quality/complaint";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLQ3;
            updateCount += updateLQ3;

            var (insertLQ1, updateLQ1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_QUALITY_COST", menu =>
            {
                menu.MenuName = "品质成本";
                menu.MenuCode = "LOGISTICS_QUALITY_COST";
                menu.I18nKey = "menu.logistics.quality.cost._self";
                menu.Icon = "RiMoneyCnyCircleLine";
                menu.ParentId = logisticsQualityMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/logistics/quality/cost";
                menu.ComponentPath = "logistics/quality/cost";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLQ1;
            updateCount += updateLQ1;
        }

        // ========== 客户服务下的三级菜单 (LOGISTICS_CUSTOMER_SERVICE，不含客诉；客诉见 LOGISTICS_QUALITY_COMPLAINT) ==========
        // 顺序：服务合同 / 服务订单 / 服务请求 / 服务工单
        if (logisticsServiceMenu != null)
        {
            var (insertLS2, updateLS2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_CUSTOMER_SERVICE_CONTRACT", menu =>
            {
                menu.MenuName = "服务合同";
                menu.MenuCode = "LOGISTICS_CUSTOMER_SERVICE_CONTRACT";
                menu.I18nKey = "menu.logistics.customer.service.contract";
                menu.Icon = "RiFileTextLine";
                menu.ParentId = logisticsServiceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:customer:service:contract:list";
                menu.RoutePath = "/logistics/customer-service/contract";
                menu.ComponentPath = "logistics/customer-service/contract/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLS2;
            updateCount += updateLS2;

            var (insertLS3, updateLS3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_CUSTOMER_SERVICE_ORDER", menu =>
            {
                menu.MenuName = "服务订单";
                menu.MenuCode = "LOGISTICS_CUSTOMER_SERVICE_ORDER";
                menu.I18nKey = "menu.logistics.customer.service.order";
                menu.Icon = "RiFileList3Line";
                menu.ParentId = logisticsServiceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:customer:service:order:list";
                menu.RoutePath = "/logistics/customer-service/order";
                menu.ComponentPath = "logistics/customer-service/order/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLS3;
            updateCount += updateLS3;

            var (insertLS1, updateLS1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_CUSTOMER_SERVICE_REQUEST", menu =>
            {
                menu.MenuName = "服务请求";
                menu.MenuCode = "LOGISTICS_CUSTOMER_SERVICE_REQUEST";
                menu.I18nKey = "menu.logistics.customer.service.request";
                menu.Icon = "RiQuestionAnswerLine";
                menu.ParentId = logisticsServiceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:customer:service:request:list";
                menu.RoutePath = "/logistics/customer-service/request";
                menu.ComponentPath = "logistics/customer-service/request/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLS1;
            updateCount += updateLS1;

            var (insertLS4, updateLS4) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_CUSTOMER_SERVICE_TICKET", menu =>
            {
                menu.MenuName = "服务工单";
                menu.MenuCode = "LOGISTICS_CUSTOMER_SERVICE_TICKET";
                menu.I18nKey = "menu.logistics.customer.service.ticket";
                menu.Icon = "RiTicketLine";
                menu.ParentId = logisticsServiceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:customer:service:ticket:list";
                menu.RoutePath = "/logistics/customer-service/ticket";
                menu.ComponentPath = "logistics/customer-service/ticket/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLS4;
            updateCount += updateLS4;

            var (insertLS5, updateLS5) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_CUSTOMER_SERVICE_GROUP", menu =>
            {
                menu.MenuName = "客户服务组";
                menu.MenuCode = "LOGISTICS_CUSTOMER_SERVICE_GROUP";
                menu.I18nKey = "menu.logistics.customer.service.group";
                menu.Icon = "RiGroupLine";
                menu.ParentId = logisticsServiceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:customer:service:group:list";
                menu.RoutePath = "/logistics/customer-service/group";
                menu.ComponentPath = "logistics/customer-service/group/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLS5;
            updateCount += updateLS5;
        }

        // ========== 工厂维护下的三级菜单 (LOGISTICS_MAINTENANCE) ==========
        // 顺序：设备信息 / 维护工单 / 维护履历 / 维护通知 / 设备维护组
        if (logisticsMaintenanceMenu != null)
        {
            var (insertLM1, updateLM1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MAINTENANCE_EQUIPMENT", menu =>
            {
                menu.MenuName = "设备信息";
                menu.MenuCode = "LOGISTICS_MAINTENANCE_EQUIPMENT";
                menu.I18nKey = "menu.logistics.maintenance.equipment";
                menu.Icon = "RiCpuLine";
                menu.ParentId = logisticsMaintenanceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:maintenance:equipment:list";
                menu.RoutePath = "/logistics/maintenance/equipment";
                menu.ComponentPath = "logistics/maintenance/equipment/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLM1;
            updateCount += updateLM1;

            var (insertLM3, updateLM3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MAINTENANCE_WORKORDER", menu =>
            {
                menu.MenuName = "维护工单";
                menu.MenuCode = "LOGISTICS_MAINTENANCE_WORKORDER";
                menu.I18nKey = "menu.logistics.maintenance.workorder";
                menu.Icon = "RiToolsLine";
                menu.ParentId = logisticsMaintenanceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:maintenance:workorder:list";
                menu.RoutePath = "/logistics/maintenance/work-order";
                menu.ComponentPath = "logistics/maintenance/work-order/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLM3;
            updateCount += updateLM3;

            var (insertLM4, updateLM4) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MAINTENANCE_HISTORY", menu =>
            {
                menu.MenuName = "维护履历";
                menu.MenuCode = "LOGISTICS_MAINTENANCE_HISTORY";
                menu.I18nKey = "menu.logistics.maintenance.history";
                menu.Icon = "RiHistoryLine";
                menu.ParentId = logisticsMaintenanceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:maintenance:history:list";
                menu.RoutePath = "/logistics/maintenance/history";
                menu.ComponentPath = "logistics/maintenance/history/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLM4;
            updateCount += updateLM4;

            var (insertLM2, updateLM2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MAINTENANCE_NOTIFICATION", menu =>
            {
                menu.MenuName = "维护通知";
                menu.MenuCode = "LOGISTICS_MAINTENANCE_NOTIFICATION";
                menu.I18nKey = "menu.logistics.maintenance.notification";
                menu.Icon = "RiNotificationLine";
                menu.ParentId = logisticsMaintenanceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:maintenance:notification:list";
                menu.RoutePath = "/logistics/maintenance/notification";
                menu.ComponentPath = "logistics/maintenance/notification/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLM2;
            updateCount += updateLM2;

            var (insertLM5, updateLM5) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MAINTENANCE_GROUP", menu =>
            {
                menu.MenuName = "设备维护组";
                menu.MenuCode = "LOGISTICS_MAINTENANCE_GROUP";
                menu.I18nKey = "menu.logistics.maintenance.group";
                menu.Icon = "RiGroupLine";
                menu.ParentId = logisticsMaintenanceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:maintenance:group:list";
                menu.RoutePath = "/logistics/maintenance/group";
                menu.ComponentPath = "logistics/maintenance/group/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLM5;
            updateCount += updateLM5;
        }

        // ========== 销售管理下的三级菜单 (LOGISTICS_SALES) ==========
        // 顺序：销售报价/订单/条件记录/价格推移/SO偏离SCR/发票/实绩推移/客户信息/顾客信息/销售商物料/销售组
        if (logisticsSalesMenu != null)
        {
            var (insertLS3, updateLS3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_SALES_QUOTATION", menu =>
            {
                menu.MenuName = "销售报价";
                menu.MenuCode = "LOGISTICS_SALES_QUOTATION";
                menu.I18nKey = "menu.logistics.sales.quotation";
                menu.Icon = "RiFileList3Line";
                menu.ParentId = logisticsSalesMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:sales:quotation:list";
                menu.RoutePath = "/logistics/sales/quotation";
                menu.ComponentPath = "logistics/sales/quotation/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLS3;
            updateCount += updateLS3;

            var (insertLS5, updateLS5) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_SALES_ORDER", menu =>
            {
                menu.MenuName = "销售订单";
                menu.MenuCode = "LOGISTICS_SALES_ORDER";
                menu.I18nKey = "menu.logistics.sales.order";
                menu.Icon = "RiShoppingCart2Line";
                menu.ParentId = logisticsSalesMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:sales:order:list";
                menu.RoutePath = "/logistics/sales/order";
                menu.ComponentPath = "logistics/sales/order/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLS5;
            updateCount += updateLS5;

            var (insertLS4, updateLS4) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_SALES_CONDITION_RECORD", menu =>
            {
                menu.MenuName = "销售条件记录";
                menu.MenuCode = "LOGISTICS_SALES_CONDITION_RECORD";
                menu.I18nKey = "menu.logistics.sales.condition.record";
                menu.Icon = "RiMoneyCnyCircleLine";
                menu.ParentId = logisticsSalesMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:sales:condition:record:list";
                menu.RoutePath = "/logistics/sales/condition-record";
                menu.ComponentPath = "logistics/sales/condition-record/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLS4;
            updateCount += updateLS4;

            var (insertLS4Trend, updateLS4Trend) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_SALES_PRICE_TREND", menu =>
            {
                menu.MenuName = "销售价格推移";
                menu.MenuCode = "LOGISTICS_SALES_PRICE_TREND";
                menu.I18nKey = "menu.logistics.sales.price.trend";
                menu.Icon = "RiLineChartLine";
                menu.ParentId = logisticsSalesMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:sales:price:trend:list";
                menu.RoutePath = "/logistics/sales/price-trend";
                menu.ComponentPath = "logistics/sales/price-trend/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLS4Trend;
            updateCount += updateLS4Trend;

            var (insertLS5ScrDev, updateLS5ScrDev) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_SALES_SO_PRICE_SCR_DEVIATION", menu =>
            {
                menu.MenuName = "SO价偏离SCR";
                menu.MenuCode = "LOGISTICS_SALES_SO_PRICE_SCR_DEVIATION";
                menu.I18nKey = "menu.logistics.sales.so.price.scr.deviation";
                menu.Icon = "RiErrorWarningLine";
                menu.ParentId = logisticsSalesMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:sales:so:price:scr:deviation:list";
                menu.RoutePath = "/logistics/sales/so-price-scr-deviation";
                menu.ComponentPath = "logistics/sales/so-price-scr-deviation/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLS5ScrDev;
            updateCount += updateLS5ScrDev;

            var (insertLS6, updateLS6) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_SALES_INVOICE", menu =>
            {
                menu.MenuName = "销售发票";
                menu.MenuCode = "LOGISTICS_SALES_INVOICE";
                menu.I18nKey = "menu.logistics.sales.invoice";
                menu.Icon = "RiBillLine";
                menu.ParentId = logisticsSalesMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:sales:invoice:list";
                menu.RoutePath = "/logistics/sales/sales-invoice";
                menu.ComponentPath = "logistics/sales/sales-invoice/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLS6;
            updateCount += updateLS6;

            var (insertLS6Actual, updateLS6Actual) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_SALES_ACTUAL_TREND", menu =>
            {
                menu.MenuName = "销售实绩推移";
                menu.MenuCode = "LOGISTICS_SALES_ACTUAL_TREND";
                menu.I18nKey = "menu.logistics.sales.actual.trend";
                menu.Icon = "RiLineChartLine";
                menu.ParentId = logisticsSalesMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:sales:actual:trend:list";
                menu.RoutePath = "/logistics/sales/sales-actual-trend";
                menu.ComponentPath = "logistics/sales/sales-actual-trend/index";
                menu.SortOrder = 7;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLS6Actual;
            updateCount += updateLS6Actual;

            var (insertLS1, updateLS1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_SALES_CUSTOMER", menu =>
            {
                menu.MenuName = "客户信息";
                menu.MenuCode = "LOGISTICS_SALES_CUSTOMER";
                menu.I18nKey = "menu.logistics.sales.customer";
                menu.Icon = "RiBuilding2Line";
                menu.ParentId = logisticsSalesMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:sales:customer:list";
                menu.RoutePath = "/logistics/sales/customer";
                menu.ComponentPath = "logistics/sales/customer/index";
                menu.SortOrder = 8;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLS1;
            updateCount += updateLS1;

            var (insertLS2, updateLS2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_SALES_CLIENT", menu =>
            {
                menu.MenuName = "顾客信息";
                menu.MenuCode = "LOGISTICS_SALES_CLIENT";
                menu.I18nKey = "menu.logistics.sales.client";
                menu.Icon = "RiContactsLine";
                menu.ParentId = logisticsSalesMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:sales:client:list";
                menu.RoutePath = "/logistics/sales/client";
                menu.ComponentPath = "logistics/sales/client/index";
                menu.SortOrder = 9;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLS2;
            updateCount += updateLS2;

            var (insertLS2m, updateLS2m) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_SALES_SELLER_MATERIAL", menu =>
            {
                menu.MenuName = "销售商物料";
                menu.MenuCode = "LOGISTICS_SALES_SELLER_MATERIAL";
                menu.I18nKey = "menu.logistics.sales.seller.material";
                menu.Icon = "RiShoppingBag3Line";
                menu.ParentId = logisticsSalesMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:sales:seller:material:list";
                menu.RoutePath = "/logistics/sales/seller-material";
                menu.ComponentPath = "logistics/sales/seller-material/index";
                menu.SortOrder = 10;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLS2m;
            updateCount += updateLS2m;

            var (insertLS7, updateLS7) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_SALES_GROUP", menu =>
            {
                menu.MenuName = "销售组";
                menu.MenuCode = "LOGISTICS_SALES_GROUP";
                menu.I18nKey = "menu.logistics.sales.group";
                menu.Icon = "RiGroupLine";
                menu.ParentId = logisticsSalesMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:sales:group:list";
                menu.RoutePath = "/logistics/sales/group";
                menu.ComponentPath = "logistics/sales/group/index";
                menu.SortOrder = 11;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLS7;
            updateCount += updateLS7;
        }

        // ========== 序列号管理下的三级菜单 (LOGISTICS_SERIAL) ==========
        if (logisticsSerialMenu != null)
        {
            var (insertSER1, updateSER1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_SERIAL_INBOUND", menu =>
            {
                menu.MenuName = "序列号入库";
                menu.MenuCode = "LOGISTICS_SERIAL_INBOUND";
                menu.I18nKey = "menu.logistics.serial.inbound";
                menu.Icon = "RiInboxArchiveLine";
                menu.ParentId = logisticsSerialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:serial:inbound:list";
                menu.RoutePath = "/logistics/serial/inbound";
                menu.ComponentPath = "logistics/serial/inbound/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSER1;
            updateCount += updateSER1;

            var (insertSER3, updateSER3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_SERIAL_OUTBOUND", menu =>
            {
                menu.MenuName = "序列号出库";
                menu.MenuCode = "LOGISTICS_SERIAL_OUTBOUND";
                menu.I18nKey = "menu.logistics.serial.outbound";
                menu.Icon = "RiInboxUnarchiveLine";
                menu.ParentId = logisticsSerialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:serial:outbound:list";
                menu.RoutePath = "/logistics/serial/outbound";
                menu.ComponentPath = "logistics/serial/outbound/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSER3;
            updateCount += updateSER3;

            var (insertSER4, updateSER4) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_SERIAL_SUMMARY", menu =>
            {
                menu.MenuName = "序列号汇总";
                menu.MenuCode = "LOGISTICS_SERIAL_SUMMARY";
                menu.I18nKey = "menu.logistics.serial.summary";
                menu.Icon = "RiFileList3Line";
                menu.ParentId = logisticsSerialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:serial:summary:list";
                menu.RoutePath = "/logistics/serial/summary";
                menu.ComponentPath = "logistics/serial/summary/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSER4;
            updateCount += updateSER4;

            var (insertSER5, updateSER5) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_SERIAL_UPLOAD", menu =>
            {
                menu.MenuName = "序列号上传";
                menu.MenuCode = "LOGISTICS_SERIAL_UPLOAD";
                menu.I18nKey = "menu.logistics.serial.upload";
                menu.Icon = "RiUploadCloud2Line";
                menu.ParentId = logisticsSerialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:serial:upload:list";
                menu.RoutePath = "/logistics/serial/upload";
                menu.ComponentPath = "logistics/serial/upload/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSER5;
            updateCount += updateSER5;
        }

        // ========== 组织管理下的三级菜单 (HUMAN_RESOURCE_ORGANIZATION) ==========
        if (hrOrganizationMenu != null)
        {
            var (insertHRO1, updateHRO1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_ORGANIZATION_DEPT", menu =>
            {
                menu.MenuName = "部门管理";
                menu.MenuCode = "HUMAN_RESOURCE_ORGANIZATION_DEPT";
                menu.I18nKey = "menu.human.resource.organization.dept";
                menu.Icon = "RiGroupLine";
                menu.ParentId = hrOrganizationMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:organization:dept:list";
                menu.RoutePath = "/human-resource/organization/dept";
                menu.ComponentPath = "human-resource/organization/dept/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRO1;
            updateCount += updateHRO1;

            var (insertHRO2, updateHRO2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_ORGANIZATION_POST", menu =>
            {
                menu.MenuName = "岗位管理";
                menu.MenuCode = "HUMAN_RESOURCE_ORGANIZATION_POST";
                menu.I18nKey = "menu.human.resource.organization.post";
                menu.Icon = "RiAdminLine";
                menu.ParentId = hrOrganizationMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:organization:post:list";
                menu.RoutePath = "/human-resource/organization/post";
                menu.ComponentPath = "human-resource/organization/post/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRO2;
            updateCount += updateHRO2;
        }

        // ========== 人事管理下的三级菜单 (HUMAN_RESOURCE_PERSONNEL) ==========
        // 与 Domain/Entities/HumanResource/Personnel 实体及 views/human-resource/personnel/* 一一对应；
        // ComponentPath 须等于「模块路径/实体 kebab/index」，供 generate-vue 识别独立页（hasOwnMenuPage）
        if (hrPersonnelMenu != null)
        {
            var (insertHRP1, updateHRP1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_PERSONNEL_EMPLOYEE", menu =>
            {
                menu.MenuName = "员工档案";
                menu.MenuCode = "HUMAN_RESOURCE_PERSONNEL_EMPLOYEE";
                menu.I18nKey = "menu.human.resource.personnel.employee";
                menu.Icon = "RiUserFollowLine";
                menu.ParentId = hrPersonnelMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:personnel:employee:list";
                menu.RoutePath = "/human-resource/personnel/employee";
                menu.ComponentPath = "human-resource/personnel/employee/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRP1;
            updateCount += updateHRP1;

            var (insertHRPOnboarding, updateHRPOnboarding) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_PERSONNEL_EMPLOYEE_ONBOARDING", menu =>
            {
                menu.MenuName = "入职待办";
                menu.MenuCode = "HUMAN_RESOURCE_PERSONNEL_EMPLOYEE_ONBOARDING";
                menu.I18nKey = "menu.human.resource.personnel.employee.onboarding";
                menu.Icon = "RiUserAddLine";
                menu.ParentId = hrPersonnelMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:personnel:employee:onboarding:list";
                menu.RoutePath = "/human-resource/personnel/employee-onboarding";
                menu.ComponentPath = "human-resource/personnel/employee-onboarding/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRPOnboarding;
            updateCount += updateHRPOnboarding;

            var (insertHRPJoined, updateHRPJoined) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_PERSONNEL_EMPLOYEE_JOINED", menu =>
            {
                menu.MenuName = "入职上岗";
                menu.MenuCode = "HUMAN_RESOURCE_PERSONNEL_EMPLOYEE_JOINED";
                menu.I18nKey = "menu.human.resource.personnel.employee.joined";
                menu.Icon = "RiUserStarLine";
                menu.ParentId = hrPersonnelMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:personnel:employee:joined:list";
                menu.RoutePath = "/human-resource/personnel/employee-joined";
                menu.ComponentPath = "human-resource/personnel/employee-joined/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRPJoined;
            updateCount += updateHRPJoined;

            var (insertHRPAddress, updateHRPAddress) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_PERSONNEL_EMPLOYEE_ADDRESS", menu =>
            {
                menu.MenuName = "员工地址";
                menu.MenuCode = "HUMAN_RESOURCE_PERSONNEL_EMPLOYEE_ADDRESS";
                menu.I18nKey = "menu.human.resource.personnel.employee.address";
                menu.Icon = "RiMapPinLine";
                menu.ParentId = hrPersonnelMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:personnel:employee:address:list";
                menu.RoutePath = "/human-resource/personnel/employee-address";
                menu.ComponentPath = "human-resource/personnel/employee-address/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRPAddress;
            updateCount += updateHRPAddress;

            var (insertHRPEducation, updateHRPEducation) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_PERSONNEL_EMPLOYEE_EDUCATION", menu =>
            {
                menu.MenuName = "教育经历";
                menu.MenuCode = "HUMAN_RESOURCE_PERSONNEL_EMPLOYEE_EDUCATION";
                menu.I18nKey = "menu.human.resource.personnel.employee.education";
                menu.Icon = "RiBookOpenLine";
                menu.ParentId = hrPersonnelMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:personnel:employee:education:list";
                menu.RoutePath = "/human-resource/personnel/employee-education";
                menu.ComponentPath = "human-resource/personnel/employee-education/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRPEducation;
            updateCount += updateHRPEducation;

            var (insertHRPFamily, updateHRPFamily) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_PERSONNEL_EMPLOYEE_FAMILY", menu =>
            {
                menu.MenuName = "家庭成员";
                menu.MenuCode = "HUMAN_RESOURCE_PERSONNEL_EMPLOYEE_FAMILY";
                menu.I18nKey = "menu.human.resource.personnel.employee.family";
                menu.Icon = "RiParentLine";
                menu.ParentId = hrPersonnelMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:personnel:employee:family:list";
                menu.RoutePath = "/human-resource/personnel/employee-family";
                menu.ComponentPath = "human-resource/personnel/employee-family/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRPFamily;
            updateCount += updateHRPFamily;

            var (insertHRPExperience, updateHRPExperience) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_PERSONNEL_EMPLOYEE_EXPERIENCE", menu =>
            {
                menu.MenuName = "工作经历";
                menu.MenuCode = "HUMAN_RESOURCE_PERSONNEL_EMPLOYEE_EXPERIENCE";
                menu.I18nKey = "menu.human.resource.personnel.employee.experience";
                menu.Icon = "RiBriefcaseLine";
                menu.ParentId = hrPersonnelMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:personnel:employee:experience:list";
                menu.RoutePath = "/human-resource/personnel/employee-experience";
                menu.ComponentPath = "human-resource/personnel/employee-experience/index";
                menu.SortOrder = 7;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRPExperience;
            updateCount += updateHRPExperience;

            var (insertHRPSkill, updateHRPSkill) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_PERSONNEL_EMPLOYEE_SKILL", menu =>
            {
                menu.MenuName = "技能证书";
                menu.MenuCode = "HUMAN_RESOURCE_PERSONNEL_EMPLOYEE_SKILL";
                menu.I18nKey = "menu.human.resource.personnel.employee.skill";
                menu.Icon = "RiAwardLine";
                menu.ParentId = hrPersonnelMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:personnel:employee:skill:list";
                menu.RoutePath = "/human-resource/personnel/employee-skill";
                menu.ComponentPath = "human-resource/personnel/employee-skill/index";
                menu.SortOrder = 8;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRPSkill;
            updateCount += updateHRPSkill;

            var (insertHRPContract, updateHRPContract) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_PERSONNEL_EMPLOYEE_CONTRACT", menu =>
            {
                menu.MenuName = "员工合同";
                menu.MenuCode = "HUMAN_RESOURCE_PERSONNEL_EMPLOYEE_CONTRACT";
                menu.I18nKey = "menu.human.resource.personnel.employee.contract";
                menu.Icon = "RiFilePaperLine";
                menu.ParentId = hrPersonnelMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:personnel:employee:contract:list";
                menu.RoutePath = "/human-resource/personnel/employee-contract";
                menu.ComponentPath = "human-resource/personnel/employee-contract/index";
                menu.SortOrder = 9;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRPContract;
            updateCount += updateHRPContract;

            var (insertHRPReassignment, updateHRPReassignment) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_PERSONNEL_EMPLOYEE_REASSIGNMENT", menu =>
            {
                menu.MenuName = "员工调动";
                menu.MenuCode = "HUMAN_RESOURCE_PERSONNEL_EMPLOYEE_REASSIGNMENT";
                menu.I18nKey = "menu.human.resource.personnel.employee.reassignment";
                menu.Icon = "RiExchangeLine";
                menu.ParentId = hrPersonnelMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:personnel:employee:reassignment:list";
                menu.RoutePath = "/human-resource/personnel/employee-reassignment";
                menu.ComponentPath = "human-resource/personnel/employee-reassignment/index";
                menu.SortOrder = 10;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRPReassignment;
            updateCount += updateHRPReassignment;

            var (insertHRPResignation, updateHRPResignation) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_PERSONNEL_EMPLOYEE_RESIGNATION", menu =>
            {
                menu.MenuName = "员工离职";
                menu.MenuCode = "HUMAN_RESOURCE_PERSONNEL_EMPLOYEE_RESIGNATION";
                menu.I18nKey = "menu.human.resource.personnel.employee.resignation";
                menu.Icon = "RiUserUnfollowLine";
                menu.ParentId = hrPersonnelMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:personnel:employee:resignation:list";
                menu.RoutePath = "/human-resource/personnel/employee-resignation";
                menu.ComponentPath = "human-resource/personnel/employee-resignation/index";
                menu.SortOrder = 11;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRPResignation;
            updateCount += updateHRPResignation;

            var (insertHRPDelegation, updateHRPDelegation) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_PERSONNEL_EMPLOYEE_DELEGATION", menu =>
            {
                menu.MenuName = "员工代理";
                menu.MenuCode = "HUMAN_RESOURCE_PERSONNEL_EMPLOYEE_DELEGATION";
                menu.I18nKey = "menu.human.resource.personnel.employee.delegation";
                menu.Icon = "RiUserSharedLine";
                menu.ParentId = hrPersonnelMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:personnel:employee:delegation:list";
                menu.RoutePath = "/human-resource/personnel/employee-delegation";
                menu.ComponentPath = "human-resource/personnel/employee-delegation/index";
                menu.SortOrder = 12;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRPDelegation;
            updateCount += updateHRPDelegation;

            var (insertHRPAttachment, updateHRPAttachment) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_PERSONNEL_EMPLOYEE_ATTACHMENT", menu =>
            {
                menu.MenuName = "档案附件";
                menu.MenuCode = "HUMAN_RESOURCE_PERSONNEL_EMPLOYEE_ATTACHMENT";
                menu.I18nKey = "menu.human.resource.personnel.employee.attachment";
                menu.Icon = "RiAttachment2";
                menu.ParentId = hrPersonnelMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:personnel:employee:attachment:list";
                menu.RoutePath = "/human-resource/personnel/employee-attachment";
                menu.ComponentPath = "human-resource/personnel/employee-attachment/index";
                menu.SortOrder = 13;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRPAttachment;
            updateCount += updateHRPAttachment;
        }

        // ========== 考勤管理下的三级菜单 (HUMAN_RESOURCE_ATTENDANCE，与 HumanResource/Attendance 实体及控制器对齐) ==========
        if (hrAttendanceMenu != null)
        {
            var (insertHRAL1, updateHRAL1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_ATTENDANCE_CALENDAR", menu =>
            {
                menu.MenuName = "工厂日历";
                menu.MenuCode = "HUMAN_RESOURCE_ATTENDANCE_CALENDAR";
                menu.I18nKey = "menu.human.resource.attendance.calendar";
                menu.Icon = "RiCalendarLine";
                menu.ParentId = hrAttendanceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:attendance:calendar:list";
                menu.RoutePath = "/human-resource/attendance/calendar";
                menu.ComponentPath = "human-resource/attendance/calendar/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRAL1;
            updateCount += updateHRAL1;

            var (insertHRAL2, updateHRAL2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_ATTENDANCE_HOLIDAY", menu =>
            {
                menu.MenuName = "假期管理";
                menu.MenuCode = "HUMAN_RESOURCE_ATTENDANCE_HOLIDAY";
                menu.I18nKey = "menu.human.resource.attendance.holiday";
                menu.Icon = "RiCalendarEventLine";
                menu.ParentId = hrAttendanceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:attendance:holiday:list";
                menu.RoutePath = "/human-resource/attendance/holiday";
                menu.ComponentPath = "human-resource/attendance/holiday/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRAL2;
            updateCount += updateHRAL2;

            var (insertHRAL3, updateHRAL3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_ATTENDANCE_SHIFT_SCHEDULE", menu =>
            {
                menu.MenuName = "排班计划";
                menu.MenuCode = "HUMAN_RESOURCE_ATTENDANCE_SHIFT_SCHEDULE";
                menu.I18nKey = "menu.human.resource.attendance.shift.schedule";
                menu.Icon = "RiCalendarScheduleLine";
                menu.ParentId = hrAttendanceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:attendance:shift:schedule:list";
                menu.RoutePath = "/human-resource/attendance/shift-schedule";
                menu.ComponentPath = "human-resource/attendance/shift-schedule/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRAL3;
            updateCount += updateHRAL3;

            var (insertHRAL4, updateHRAL4) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_ATTENDANCE_WORK_SHIFT", menu =>
            {
                menu.MenuName = "班次管理";
                menu.MenuCode = "HUMAN_RESOURCE_ATTENDANCE_WORK_SHIFT";
                menu.I18nKey = "menu.human.resource.attendance.work.shift";
                menu.Icon = "RiTimeZoneLine";
                menu.ParentId = hrAttendanceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:attendance:work:shift:list";
                menu.RoutePath = "/human-resource/attendance/work-shift";
                menu.ComponentPath = "human-resource/attendance/work-shift/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRAL4;
            updateCount += updateHRAL4;

            var (insertHRAL5, updateHRAL5) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_ATTENDANCE_LEAVE", menu =>
            {
                menu.MenuName = "请假管理";
                menu.MenuCode = "HUMAN_RESOURCE_ATTENDANCE_LEAVE";
                menu.I18nKey = "menu.human.resource.attendance.leave";
                menu.Icon = "RiCalendarCheckLine";
                menu.ParentId = hrAttendanceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:attendance:leave:list";
                menu.RoutePath = "/human-resource/attendance/leave";
                menu.ComponentPath = "human-resource/attendance/leave/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRAL5;
            updateCount += updateHRAL5;

            var (insertHRAL6, updateHRAL6) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_ATTENDANCE_OVERTIME", menu =>
            {
                menu.MenuName = "加班管理";
                menu.MenuCode = "HUMAN_RESOURCE_ATTENDANCE_OVERTIME";
                menu.I18nKey = "menu.human.resource.attendance.overtime";
                menu.Icon = "RiTimeLine";
                menu.ParentId = hrAttendanceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:attendance:overtime:list";
                menu.RoutePath = "/human-resource/attendance/overtime";
                menu.ComponentPath = "human-resource/attendance/overtime/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRAL6;
            updateCount += updateHRAL6;
        }
        // ========== 薪酬管理下的三级菜单 (HUMAN_RESOURCE_COMPENSATION，与 Compensation 实体对齐) ==========
        if (hrCompensationMenu != null)
        {
            var (insertHRC1, updateHRC1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_COMPENSATION_SALARY_ITEM", menu =>
            {
                menu.MenuName = "薪资项目";
                menu.MenuCode = "HUMAN_RESOURCE_COMPENSATION_SALARY_ITEM";
                menu.I18nKey = "menu.human.resource.compensation.salary.item";
                menu.Icon = "RiPriceTag3Line";
                menu.ParentId = hrCompensationMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:compensation:salary:item:list";
                menu.RoutePath = "/human-resource/compensation/salary-item";
                menu.ComponentPath = "human-resource/compensation/salary-item/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRC1;
            updateCount += updateHRC1;

            var (insertHRC2, updateHRC2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_COMPENSATION_PAYROLL", menu =>
            {
                menu.MenuName = "薪酬体系";
                menu.MenuCode = "HUMAN_RESOURCE_COMPENSATION_PAYROLL";
                menu.I18nKey = "menu.human.resource.compensation.payroll";
                menu.Icon = "RiFileList3Line";
                menu.ParentId = hrCompensationMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:compensation:payroll:list";
                menu.RoutePath = "/human-resource/compensation/payroll";
                menu.ComponentPath = "human-resource/compensation/payroll/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRC2;
            updateCount += updateHRC2;

            var (insertHRC3, updateHRC3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_COMPENSATION_PAY_SCALE", menu =>
            {
                menu.MenuName = "薪级";
                menu.MenuCode = "HUMAN_RESOURCE_COMPENSATION_PAY_SCALE";
                menu.I18nKey = "menu.human.resource.compensation.pay.scale";
                menu.Icon = "RiStackLine";
                menu.ParentId = hrCompensationMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:compensation:pay:scale:list";
                menu.RoutePath = "/human-resource/compensation/pay-scale";
                menu.ComponentPath = "human-resource/compensation/pay-scale/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRC3;
            updateCount += updateHRC3;

            var (insertHRC4, updateHRC4) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_COMPENSATION_EMP_SALARY", menu =>
            {
                menu.MenuName = "员工定薪";
                menu.MenuCode = "HUMAN_RESOURCE_COMPENSATION_EMP_SALARY";
                menu.I18nKey = "menu.human.resource.compensation.emp.salary";
                menu.Icon = "RiUserSettingsLine";
                menu.ParentId = hrCompensationMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:compensation:emp:salary:list";
                menu.RoutePath = "/human-resource/compensation/emp-salary";
                menu.ComponentPath = "human-resource/compensation/emp-salary/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRC4;
            updateCount += updateHRC4;

            var (insertHRC5, updateHRC5) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_COMPENSATION_BONUS_PLAN", menu =>
            {
                menu.MenuName = "奖金方案";
                menu.MenuCode = "HUMAN_RESOURCE_COMPENSATION_BONUS_PLAN";
                menu.I18nKey = "menu.human.resource.compensation.bonus.plan";
                menu.Icon = "RiAwardLine";
                menu.ParentId = hrCompensationMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:compensation:bonus:plan:list";
                menu.RoutePath = "/human-resource/compensation/bonus-plan";
                menu.ComponentPath = "human-resource/compensation/bonus-plan/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRC5;
            updateCount += updateHRC5;

            var (insertHRC6, updateHRC6) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_COMPENSATION_SALARY_FORMULA", menu =>
            {
                menu.MenuName = "薪资计算公式";
                menu.MenuCode = "HUMAN_RESOURCE_COMPENSATION_SALARY_FORMULA";
                menu.I18nKey = "menu.human.resource.compensation.salary.formula";
                menu.Icon = "RiFunctionsLine";
                menu.ParentId = hrCompensationMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:compensation:salary:formula:list";
                menu.RoutePath = "/human-resource/compensation/salary-formula";
                menu.ComponentPath = "human-resource/compensation/salary-formula/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRC6;
            updateCount += updateHRC6;

            var (insertHRC7, updateHRC7) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_COMPENSATION_PAYSLIP", menu =>
            {
                menu.MenuName = "工资条";
                menu.MenuCode = "HUMAN_RESOURCE_COMPENSATION_PAYSLIP";
                menu.I18nKey = "menu.human.resource.compensation.payslip";
                menu.Icon = "RiBillLine";
                menu.ParentId = hrCompensationMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:compensation:payslip:list";
                menu.RoutePath = "/human-resource/compensation/payslip";
                menu.ComponentPath = "human-resource/compensation/payslip/index";
                menu.SortOrder = 7;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRC7;
            updateCount += updateHRC7;
        }

        // ========== 福利管理下的三级菜单 (HUMAN_RESOURCE_BENEFITS，与 Benefits 实体对齐) ==========
        if (hrBenefitsMenu != null)
        {
            var (insertHRB1, updateHRB1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_BENEFITS_BENEFIT_ITEM", menu =>
            {
                menu.MenuName = "福利项目";
                menu.MenuCode = "HUMAN_RESOURCE_BENEFITS_BENEFIT_ITEM";
                menu.I18nKey = "menu.human.resource.benefits.benefit.item";
                menu.Icon = "RiGiftLine";
                menu.ParentId = hrBenefitsMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:benefits:benefit:item:list";
                menu.RoutePath = "/human-resource/benefits/benefit-item";
                menu.ComponentPath = "human-resource/benefits/benefit-item/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRB1;
            updateCount += updateHRB1;

            var (insertHRB2, updateHRB2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_BENEFITS_EMP_BENEFIT_PLAN", menu =>
            {
                menu.MenuName = "员工福利方案";
                menu.MenuCode = "HUMAN_RESOURCE_BENEFITS_EMP_BENEFIT_PLAN";
                menu.I18nKey = "menu.human.resource.benefits.emp.benefit.plan";
                menu.Icon = "RiUserHeartLine";
                menu.ParentId = hrBenefitsMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:benefits:emp:benefit:plan:list";
                menu.RoutePath = "/human-resource/benefits/emp-benefit-plan";
                menu.ComponentPath = "human-resource/benefits/emp-benefit-plan/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRB2;
            updateCount += updateHRB2;

            var (insertHRB3, updateHRB3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_BENEFITS_SOCIAL_INSURANCE", menu =>
            {
                menu.MenuName = "社保公积金";
                menu.MenuCode = "HUMAN_RESOURCE_BENEFITS_SOCIAL_INSURANCE";
                menu.I18nKey = "menu.human.resource.benefits.social.insurance";
                menu.Icon = "RiShieldCheckLine";
                menu.ParentId = hrBenefitsMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:benefits:social:insurance:list";
                menu.RoutePath = "/human-resource/benefits/social-insurance";
                menu.ComponentPath = "human-resource/benefits/social-insurance/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRB3;
            updateCount += updateHRB3;
        }

        // ========== 绩效管理下的三级菜单 (HUMAN_RESOURCE_PERFORMANCE，与 5 个实体对齐) ==========
        if (hrPerformanceMenu != null)
        {
            var (insertHRP1, updateHRP1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_PERFORMANCE_PERF_CYCLE", menu =>
            {
                menu.MenuName = "绩效周期";
                menu.MenuCode = "HUMAN_RESOURCE_PERFORMANCE_PERF_CYCLE";
                menu.I18nKey = "menu.human.resource.performance.perf.cycle";
                menu.Icon = "RiCalendarLine";
                menu.ParentId = hrPerformanceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:performance:perf:cycle:list";
                menu.RoutePath = "/human-resource/performance/perf-cycle";
                menu.ComponentPath = "human-resource/performance/perf-cycle/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRP1;
            updateCount += updateHRP1;

            var (insertHRP2, updateHRP2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_PERFORMANCE_PERF_SCHEME", menu =>
            {
                menu.MenuName = "绩效方案";
                menu.MenuCode = "HUMAN_RESOURCE_PERFORMANCE_PERF_SCHEME";
                menu.I18nKey = "menu.human.resource.performance.perf.scheme";
                menu.Icon = "RiFileChartLine";
                menu.ParentId = hrPerformanceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:performance:perf:scheme:list";
                menu.RoutePath = "/human-resource/performance/perf-scheme";
                menu.ComponentPath = "human-resource/performance/perf-scheme/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRP2;
            updateCount += updateHRP2;

            var (insertHRP3, updateHRP3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_PERFORMANCE_PERF_OBJECTIVE", menu =>
            {
                menu.MenuName = "绩效目标";
                menu.MenuCode = "HUMAN_RESOURCE_PERFORMANCE_PERF_OBJECTIVE";
                menu.I18nKey = "menu.human.resource.performance.perf.objective";
                menu.Icon = "RiTargetLine";
                menu.ParentId = hrPerformanceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:performance:perf:objective:list";
                menu.RoutePath = "/human-resource/performance/perf-objective";
                menu.ComponentPath = "human-resource/performance/perf-objective/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRP3;
            updateCount += updateHRP3;

            var (insertHRP4, updateHRP4) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_PERFORMANCE_PERF_ASSESSMENT", menu =>
            {
                menu.MenuName = "绩效考核";
                menu.MenuCode = "HUMAN_RESOURCE_PERFORMANCE_PERF_ASSESSMENT";
                menu.I18nKey = "menu.human.resource.performance.perf.assessment";
                menu.Icon = "RiClipboardLine";
                menu.ParentId = hrPerformanceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:performance:perf:assessment:list";
                menu.RoutePath = "/human-resource/performance/perf-assessment";
                menu.ComponentPath = "human-resource/performance/perf-assessment/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRP4;
            updateCount += updateHRP4;

            var (insertHRP5, updateHRP5) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_PERFORMANCE_PERF_ANALYSIS", menu =>
            {
                menu.MenuName = "分析改进";
                menu.MenuCode = "HUMAN_RESOURCE_PERFORMANCE_PERF_ANALYSIS";
                menu.I18nKey = "menu.human.resource.performance.perf.analysis";
                menu.Icon = "RiLightbulbLine";
                menu.ParentId = hrPerformanceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:performance:perf:analysis:list";
                menu.RoutePath = "/human-resource/performance/perf-analysis";
                menu.ComponentPath = "human-resource/performance/perf-analysis/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRP5;
            updateCount += updateHRP5;
        }

        // ========== 教育培训下的三级菜单 (HUMAN_RESOURCE_TRAINING，与 TrainingCourse / TrainingPlan / TrainingAttendee 对齐) ==========
        if (hrTrainingMenu != null)
        {
            var (insertHRT1, updateHRT1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_TRAINING_COURSE", menu =>
            {
                menu.MenuName = "培训课程";
                menu.MenuCode = "HUMAN_RESOURCE_TRAINING_COURSE";
                menu.I18nKey = "menu.human.resource.training.course";
                menu.Icon = "RiBookOpenLine";
                menu.ParentId = hrTrainingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:training:course:list";
                menu.RoutePath = "/human-resource/training/training-course";
                menu.ComponentPath = "human-resource/training/training-course/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRT1;
            updateCount += updateHRT1;

            var (insertHRT2, updateHRT2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_TRAINING_PLAN", menu =>
            {
                menu.MenuName = "年度计划";
                menu.MenuCode = "HUMAN_RESOURCE_TRAINING_PLAN";
                menu.I18nKey = "menu.human.resource.training.plan";
                menu.Icon = "RiCalendarScheduleLine";
                menu.ParentId = hrTrainingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:training:plan:list";
                menu.RoutePath = "/human-resource/training/training-plan";
                menu.ComponentPath = "human-resource/training/training-plan/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRT2;
            updateCount += updateHRT2;

            var (insertHRT3, updateHRT3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_TRAINING_ATTENDEE", menu =>
            {
                menu.MenuName = "参训记录";
                menu.MenuCode = "HUMAN_RESOURCE_TRAINING_ATTENDEE";
                menu.I18nKey = "menu.human.resource.training.attendee";
                menu.Icon = "RiUserFollowLine";
                menu.ParentId = hrTrainingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:training:attendee:list";
                menu.RoutePath = "/human-resource/training/training-attendee";
                menu.ComponentPath = "human-resource/training/training-attendee/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRT3;
            updateCount += updateHRT3;
        }

        // ========== 人才管理下的三级菜单 (HUMAN_RESOURCE_TALENT) ==========
        if (hrTalentMenu != null)
        {
            var (insertHRT0, updateHRT0) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_TALENT_STAFFING_REQUIREMENT", menu =>
            {
                menu.MenuName = "用人需求";
                menu.MenuCode = "HUMAN_RESOURCE_TALENT_STAFFING_REQUIREMENT";
                menu.I18nKey = "menu.human.resource.talent.staffing.requirement";
                menu.Icon = "RiFileList3Line";
                menu.ParentId = hrTalentMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:talent:staffing:requirement:list";
                menu.RoutePath = "/human-resource/talent/staffing-requirement";
                menu.ComponentPath = "human-resource/talent/staffing-requirement/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRT0;
            updateCount += updateHRT0;

            var (insertHRT2, updateHRT2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_TALENT_JOB_POSTING", menu =>
            {
                menu.MenuName = "职位发布";
                menu.MenuCode = "HUMAN_RESOURCE_TALENT_JOB_POSTING";
                menu.I18nKey = "menu.human.resource.talent.job.posting";
                menu.Icon = "RiMegaphoneLine";
                menu.ParentId = hrTalentMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:talent:job:posting:list";
                menu.RoutePath = "/human-resource/talent/job-posting";
                menu.ComponentPath = "human-resource/talent/job-posting/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRT2;
            updateCount += updateHRT2;

            var (insertHRT5, updateHRT5) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMAN_RESOURCE_TALENT_OFFER", menu =>
            {
                menu.MenuName = "录用";
                menu.MenuCode = "HUMAN_RESOURCE_TALENT_OFFER";
                menu.I18nKey = "menu.human.resource.talent.offer";
                menu.Icon = "RiCheckboxCircleLine";
                menu.ParentId = hrTalentMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "human:resource:talent:offer:list";
                menu.RoutePath = "/human-resource/talent/offer";
                menu.ComponentPath = "human-resource/talent/offer/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRT5;
            updateCount += updateHRT5;
        }

        // ========== 快速查询下的三级菜单 (STATISTICS_QUICK_QUERY) ==========
        if (statisticsQuickQueryMenu != null)
        {
            var (insertSR1, updateSR1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "STATISTICS_QUICK_QUERY_CONFIGURABLE", menu =>
            {
                menu.MenuName = "定制报表";
                menu.MenuCode = "STATISTICS_QUICK_QUERY_CONFIGURABLE";
                menu.I18nKey = "menu.statistics.quickquery.configurable";
                menu.Icon = "RiFileChartLine";
                menu.ParentId = statisticsQuickQueryMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "statistics:quickquery:configurable:list";
                menu.RoutePath = "/statistics/quick-query/configurable";
                menu.ComponentPath = "statistics/quick-query/configurable/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSR1;
            updateCount += updateSR1;
        }

        // ========== 日志管理下的三级菜单 (STATISTICS_LOGGING) ==========
        if (statisticsLoggingMenu != null)
        {
            var (insertSL1, updateSL1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "STATISTICS_LOGGING_LOGIN_LOG", menu =>
            {
                menu.MenuName = "登录日志";
                menu.MenuCode = "STATISTICS_LOGGING_LOGIN_LOG";
                menu.I18nKey = "menu.statistics.logging.login.log";
                menu.Icon = "RiLoginBoxLine";
                menu.ParentId = statisticsLoggingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "statistics:logging:login:log:list";
                menu.RoutePath = "/statistics/logging/login-log";
                menu.ComponentPath = "statistics/logging/login-log/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSL1;
            updateCount += updateSL1;

            var (insertSL2, updateSL2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "STATISTICS_LOGGING_OPER_LOG", menu =>
            {
                menu.MenuName = "操作日志";
                menu.MenuCode = "STATISTICS_LOGGING_OPER_LOG";
                menu.I18nKey = "menu.statistics.logging.oper.log";
                menu.Icon = "RiHistoryLine";
                menu.ParentId = statisticsLoggingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "statistics:logging:oper:log:list";
                menu.RoutePath = "/statistics/logging/oper-log";
                menu.ComponentPath = "statistics/logging/oper-log/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSL2;
            updateCount += updateSL2;

            var (insertSL3, updateSL3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "STATISTICS_LOGGING_DELTA_LOG", menu =>
            {
                menu.MenuName = "差异日志";
                menu.MenuCode = "STATISTICS_LOGGING_DELTA_LOG";
                menu.I18nKey = "menu.statistics.logging.delta.log";
                menu.Icon = "RiGitCommitLine";
                menu.ParentId = statisticsLoggingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "statistics:logging:delta:log:list";
                menu.RoutePath = "/statistics/logging/delta-log";
                menu.ComponentPath = "statistics/logging/delta-log/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSL3;
            updateCount += updateSL3;

            var (insertSL4, updateSL4) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "STATISTICS_LOGGING_QUARTZ_LOG", menu =>
            {
                menu.MenuName = "任务日志";
                menu.MenuCode = "STATISTICS_LOGGING_QUARTZ_LOG";
                menu.I18nKey = "menu.statistics.logging.quartz.log";
                menu.Icon = "RiTimerLine";
                menu.ParentId = statisticsLoggingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "statistics:logging:quartz:log:list";
                menu.RoutePath = "/statistics/logging/quartz-log";
                menu.ComponentPath = "statistics/logging/quartz-log/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSL4;
            updateCount += updateSL4;

            var (insertSL5, updateSL5) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "STATISTICS_LOGGING_SERVER_MONITOR", menu =>
            {
                menu.MenuName = "服务监控";
                menu.MenuCode = "STATISTICS_LOGGING_SERVER_MONITOR";
                menu.I18nKey = "menu.statistics.logging.server.monitor";
                menu.Icon = "RiServerLine";
                menu.ParentId = statisticsLoggingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "statistics:logging:server:monitor:list";
                menu.RoutePath = "/statistics/logging/server-monitor";
                menu.ComponentPath = "statistics/logging/server-monitor/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSL5;
            updateCount += updateSL5;

            var (insertSL6, updateSL6) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "STATISTICS_LOGGING_TRACKING_LOG", menu =>
            {
                menu.MenuName = "交互日志";
                menu.MenuCode = "STATISTICS_LOGGING_TRACKING_LOG";
                menu.I18nKey = "menu.statistics.logging.tracking.log";
                menu.Icon = "RiPulseLine";
                menu.ParentId = statisticsLoggingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "statistics:logging:tracking:log:list";
                menu.RoutePath = "/statistics/logging/tracking-log";
                menu.ComponentPath = "statistics/logging/tracking-log/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSL6;
            updateCount += updateSL6;

            var (insertSL7, updateSL7) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "STATISTICS_LOGGING_ARCHIVE_LOG", menu =>
            {
                menu.MenuName = "归档日志";
                menu.MenuCode = "STATISTICS_LOGGING_ARCHIVE_LOG";
                menu.I18nKey = "menu.statistics.logging.archive.log";
                menu.Icon = "RiArchiveDrawerLine";
                menu.ParentId = statisticsLoggingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "statistics:logging:archive:log:list";
                menu.RoutePath = "/statistics/logging/archive-log";
                menu.ComponentPath = "statistics/logging/archive-log/index";
                menu.SortOrder = 7;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSL7;
            updateCount += updateSL7;

            var (insertSL8, updateSL8) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "STATISTICS_LOGGING_BACKUP_LOG", menu =>
            {
                menu.MenuName = "备份日志";
                menu.MenuCode = "STATISTICS_LOGGING_BACKUP_LOG";
                menu.I18nKey = "menu.statistics.logging.backup.log";
                menu.Icon = "RiDatabase2Line";
                menu.ParentId = statisticsLoggingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "statistics:logging:backup:log:list";
                menu.RoutePath = "/statistics/logging/backup-log";
                menu.ComponentPath = "statistics/logging/backup-log/index";
                menu.SortOrder = 8;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSL8;
            updateCount += updateSL8;
        }

        // ========== 会议中心下的三级菜单 (ROUTINE_MEETING_CENTER) ==========
        if (routineMeetingCenterMenu != null)
        {
            var (insertMc1, updateMc1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ROUTINE_MEETING_CENTER_MEETING", menu =>
            {
                menu.MenuName = "会议";
                menu.MenuCode = "ROUTINE_MEETING_CENTER_MEETING";
                menu.I18nKey = "menu.routine.meeting.center.meeting";
                menu.Icon = "RiVideoLine";
                menu.ParentId = routineMeetingCenterMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "routine:meeting:center:list";
                menu.RoutePath = "/routine/meeting-center/meeting";
                menu.ComponentPath = "routine/meeting-center/meeting/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsLeaf = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertMc1;
            updateCount += updateMc1;

            var (insertMc2, updateMc2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ROUTINE_MEETING_CENTER_ROOM", menu =>
            {
                menu.MenuName = "会议室";
                menu.MenuCode = "ROUTINE_MEETING_CENTER_ROOM";
                menu.I18nKey = "menu.routine.meeting.center.room";
                menu.Icon = "RiBuilding2Line";
                menu.ParentId = routineMeetingCenterMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "routine:meeting:room:list";
                menu.RoutePath = "/routine/meeting-center/meeting-room";
                menu.ComponentPath = "routine/meeting-center/meeting-room/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsLeaf = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertMc2;
            updateCount += updateMc2;
        }

        // ========== 服务台下的三级菜单 (ROUTINE_HELP_DESK) ==========
        if (routineHelpDeskMenu != null)
        {
            var (insertHd1, updateHd1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ROUTINE_HELP_DESK_MY_TICKET", menu =>
            {
                menu.MenuName = "我的工单";
                menu.MenuCode = "ROUTINE_HELP_DESK_MY_TICKET";
                menu.I18nKey = "menu.routine.help.desk.my.ticket";
                menu.Icon = "RiTicketLine";
                menu.ParentId = routineHelpDeskMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "routine:help:desk:my:ticket:list";
                menu.RoutePath = "/routine/help-desk/my-ticket";
                menu.ComponentPath = "routine/help-desk/my-ticket/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHd1;
            updateCount += updateHd1;

            var (insertHd2, updateHd2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ROUTINE_HELP_DESK_TICKET", menu =>
            {
                menu.MenuName = "工单管理";
                menu.MenuCode = "ROUTINE_HELP_DESK_TICKET";
                menu.I18nKey = "menu.routine.help.desk.ticket";
                menu.Icon = "RiCustomerService2Line";
                menu.ParentId = routineHelpDeskMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "routine:help:desk:ticket:list";
                menu.RoutePath = "/routine/help-desk/ticket";
                menu.ComponentPath = "routine/help-desk/ticket/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHd2;
            updateCount += updateHd2;

            var (insertHd3, updateHd3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ROUTINE_HELP_DESK_KNOWLEDGE", menu =>
            {
                menu.MenuName = "知识库（FAQ）";
                menu.MenuCode = "ROUTINE_HELP_DESK_KNOWLEDGE";
                menu.I18nKey = "menu.routine.help.desk.knowledge";
                menu.Icon = "RiBookOpenLine";
                menu.ParentId = routineHelpDeskMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "routine:help:desk:knowledge:list";
                menu.RoutePath = "/routine/help-desk/knowledge";
                menu.ComponentPath = "routine/help-desk/knowledge/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHd3;
            updateCount += updateHd3;

            var (insertHd4, updateHd4) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ROUTINE_HELP_DESK_MY_ASSET", menu =>
            {
                menu.MenuName = "我的资产";
                menu.MenuCode = "ROUTINE_HELP_DESK_MY_ASSET";
                menu.I18nKey = "menu.routine.help.desk.my.asset";
                menu.Icon = "RiDeviceLine";
                menu.ParentId = routineHelpDeskMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "routine:help:desk:my:asset:list";
                menu.RoutePath = "/routine/help-desk/my-asset";
                menu.ComponentPath = "routine/help-desk/my-asset/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHd4;
            updateCount += updateHd4;

            var (insertHd5, updateHd5) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ROUTINE_HELP_DESK_IT_ASSET", menu =>
            {
                menu.MenuName = "IT设备保修";
                menu.MenuCode = "ROUTINE_HELP_DESK_IT_ASSET";
                menu.I18nKey = "menu.routine.help.desk.it.asset";
                menu.Icon = "RiDeviceLine";
                menu.ParentId = routineHelpDeskMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "routine:help:desk:it:asset:list";
                menu.RoutePath = "/routine/help-desk/it-asset";
                menu.ComponentPath = "routine/help-desk/it-asset/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHd5;
            updateCount += updateHd5;
        }

        return (insertCount, updateCount);
    }

    /// <summary>
    /// 创建或更新菜单。
    /// </summary>
    /// <param name="menuRepository">菜单仓储。</param>
    /// <param name="seedContext">数据库上下文。</param>
    /// <param name="tenantCode">租户编码。</param>
    /// <param name="menuCode">菜单编码（业务键）。</param>
    /// <param name="configure">菜单配置委托。</param>
    /// <returns>元组:(InsertCount, UpdateCount),本条菜单新增或更新条数(0或1)。</returns>
    private static async Task<(int InsertCount, int UpdateCount)> CreateOrUpdateMenuAsync(
        ITaktTenantSeedRepository<TaktMenu> menuRepository,
        TaktSeedContext seedContext,
        string tenantCode,
        string menuCode,
        Action<TaktMenu> configure)
    {
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
                var parentMenu = await menuRepository.FirstAsync(m => m.TenantCode == tenantCode && m.Id == menu.ParentId);
                if (parentMenu != null)
                {
                    menu.MenuPath = $"{parentMenu.MenuPath}{menu.Id}/";
                    menu.Level = parentMenu.Level + 1;
                    
                    // 更新父级 IsLeaf 为非叶子
                    if (parentMenu.IsLeaf == 1)
                    {
                        parentMenu.IsLeaf = 0;
                        await menuRepository.UpdateAsync(parentMenu);
                    }
                }
            }
            else
            {
                menu.MenuPath = $"/{menu.Id}/";
                menu.Level = 1;
            }
            
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
                var parentMenu = await menuRepository.FirstAsync(m => m.TenantCode == tenantCode && m.Id == menu.ParentId);
                if (parentMenu != null)
                {
                    menu.MenuPath = $"{parentMenu.MenuPath}{menu.Id}/";
                    menu.Level = parentMenu.Level + 1;
                    
                    // 更新父级 IsLeaf 为非叶子
                    if (parentMenu.IsLeaf == 1)
                    {
                        parentMenu.IsLeaf = 0;
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
