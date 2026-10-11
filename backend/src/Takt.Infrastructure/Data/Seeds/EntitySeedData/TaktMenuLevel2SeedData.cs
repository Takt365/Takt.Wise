// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds
// 文件名称：TaktMenuLevel2SeedData.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt 二级菜单种子数据。
//           在一级菜单（TaktMenuLevel1SeedData）已存在的前提下，按父级 MenuCode 挂载子菜单：
//           含仪表盘/工作流/日常事务/财务/后勤/身份/人力/代码/统计等业务域下的目录与页面项。
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Identity;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// Takt 二级菜单种子数据。
/// <para>
/// 父级引用依赖 TaktMenuLevel1SeedData 中预置的一级 <c>MenuCode</c>（如 DASHBOARD、WORKFLOW、IDENTITY 等）。
/// 页面菜单（MenuType=1）需配置以 <c>:list</c> 结尾的 TaktMenu.Permission，供后续按钮种子使用。
/// 由 TaktMenuSeedData 统一协调调用，不直接注册为 ITaktSeedDataCoordinator。
/// </para>
/// </summary>
public class TaktMenuLevel2SeedData
{
    /// <summary>
    /// 初始化二级菜单种子数据。
    /// <para>
    /// 按业务块分区写入：仪表盘子项、工作流、日常事务子目录、财务子目录、后勤子目录、身份认证页面、
    /// 人力资源子目录、代码生成、统计子目录等；不存在则创建，存在则更新字段。
    /// </para>
    /// </summary>
    /// <param name="serviceProvider">服务提供者，用于解析 ITaktRepository{TaktMenu}。</param>
    /// <param name="specifiedTenantCode">租户编码（由协调器传入）。</param>
    /// <returns>元组：(InsertCount, UpdateCount)，分别为本次新增与更新的二级菜单条数。</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? specifiedTenantCode = null)
    {
        var menuRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktMenu>>();
        var sqlSugarContext = serviceProvider.GetRequiredService<TaktSeedContext>();

        // 二级菜单:基于一级菜单的ParentId
        // 注意:菜单为租户级实体,由协调器指定租户
        var tenantCode = specifiedTenantCode
            ?? throw new ArgumentException("二级菜单种子必须指定租户编码。", nameof(specifiedTenantCode));

        int insertCount = 0;
        int updateCount = 0;

        // 获取一级父菜单(注意：必须绕过仓储的租户过滤，直接查询)
        // 注意:一级菜单已在 TaktMenuLevel1SeedData 中初始化
        // 严格按照一级菜单 SortOrder 顺序排列
        var homeMenu = await sqlSugarContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "HOME" && m.IsDeleted == 0)
            .FirstAsync();
        var dashboardMenu = await sqlSugarContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "DASHBOARD" && m.IsDeleted == 0)
            .FirstAsync();
        var routineMenu = await sqlSugarContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "ROUTINE" && m.IsDeleted == 0)
            .FirstAsync();
        var accountingMenu = await sqlSugarContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "ACCOUNTING" && m.IsDeleted == 0)
            .FirstAsync();
        var logisticsMenu = await sqlSugarContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "LOGISTICS" && m.IsDeleted == 0)
            .FirstAsync();
        var humanResourceMenu = await sqlSugarContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "HUMAN_RESOURCE" && m.IsDeleted == 0)
            .FirstAsync();
        var identityMenu = await sqlSugarContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "IDENTITY" && m.IsDeleted == 0)
            .FirstAsync();
        var workflowMenu = await sqlSugarContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "WORKFLOW" && m.IsDeleted == 0)
            .FirstAsync();
        var codeMenu = await sqlSugarContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "CODE" && m.IsDeleted == 0)
            .FirstAsync();
        var foundationMenu = await sqlSugarContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "FOUNDATION" && m.IsDeleted == 0)
            .FirstAsync();
        var statisticsMenu = await sqlSugarContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "STATISTICS" && m.IsDeleted == 0)
            .FirstAsync();

        // ========== 仪表盘下的二级菜单 (SortOrder: 2) ==========
        if (dashboardMenu != null)
        {
            var (insert1, update1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "WORKSPACE", menu =>
            {
                menu.MenuName = "工作台";
                menu.MenuCode = "WORKSPACE";
                menu.I18nKey = "menu.workspace";
                menu.Icon = "RiGridLine";
                menu.ParentId = dashboardMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "workspace:list";
                menu.RoutePath = "/dashboard/workspace";
                menu.ComponentPath = "dashboard/workspace/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert1;
            updateCount += update1;

            var (insert2, update2) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "DATA_BOARD", menu =>
            {
                menu.MenuName = "数据看板";
                menu.MenuCode = "DATA_BOARD";
                menu.I18nKey = "menu.data.board";
                menu.Icon = "RiDashboard2Line";
                menu.ParentId = dashboardMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "data:board:list";
                menu.RoutePath = "/dashboard/data-board";
                menu.ComponentPath = "dashboard/data-board/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert2;
            updateCount += update2;
        }

        // ========== 日常事务下的二级菜单 (SortOrder: 3) ==========
        // 顺序：公告通知 → 会议中心（目录） → 文管中心（页面） → 新闻中心（页面） → 服务台（目录） → 访客中心
        if (routineMenu != null)
        {
            var (insert1, update1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "ROUTINE_ANNOUNCEMENT", menu =>
            {
                menu.MenuName = "公告通知";
                menu.MenuCode = "ROUTINE_ANNOUNCEMENT";
                menu.I18nKey = "menu.routine.announcement";
                menu.Icon = "RiNotification3Line";
                menu.ParentId = routineMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "routine:announcement:list";
                menu.RoutePath = "/routine/announcement";
                menu.ComponentPath = "routine/announcement/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert1;
            updateCount += update1;

            // 会议中心：二级目录（下级：会议、会议室）
            var (insert2, update2) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "ROUTINE_MEETING_CENTER", menu =>
            {
                menu.MenuName = "会议中心";
                menu.MenuCode = "ROUTINE_MEETING_CENTER";
                menu.I18nKey = "menu.routine.meeting.center._self";
                menu.Icon = "RiVideoLine";
                menu.ParentId = routineMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/routine/meeting-center";
                menu.ComponentPath = "routine/meeting-center";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert2;
            updateCount += update2;

            // 文管中心：二级页面菜单（版本控制在文管视图工具栏按钮中，无三级子菜单）
            var (insert3, update3) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "ROUTINE_DOCUMENT_CENTER", menu =>
            {
                menu.MenuName = "文管中心";
                menu.MenuCode = "ROUTINE_DOCUMENT_CENTER";
                menu.I18nKey = "menu.routine.document.center";
                menu.Icon = "RiFileTextLine";
                menu.ParentId = routineMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "routine:document:center:list";
                menu.RoutePath = "/routine/document-center/document";
                menu.ComponentPath = "routine/document-center/document/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
                menu.IsLeaf = 1;
            });
            insertCount += insert3;
            updateCount += update3;

            // 新闻中心：二级页面菜单（与公告通知/访客中心同型；无三级子菜单）
            var (insert4, update4) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "ROUTINE_NEWS_CENTER", menu =>
            {
                menu.MenuName = "新闻中心";
                menu.MenuCode = "ROUTINE_NEWS_CENTER";
                menu.I18nKey = "menu.routine.news.center";
                menu.Icon = "RiArticleLine";
                menu.ParentId = routineMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "routine:news:center:list";
                menu.RoutePath = "/routine/news-center/news";
                menu.ComponentPath = "routine/news-center/news/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
                menu.IsLeaf = 1;
            });
            insertCount += insert4;
            updateCount += update4;

            var (insert5, update5) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "ROUTINE_HELP_DESK", menu =>
            {
                menu.MenuName = "服务台";
                menu.MenuCode = "ROUTINE_HELP_DESK";
                menu.I18nKey = "menu.routine.help.desk._self";
                menu.Icon = "RiCustomerService2Line";
                menu.ParentId = routineMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/routine/help-desk";
                menu.ComponentPath = "routine/help-desk";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert5;
            updateCount += update5;

            var (insert6, update6) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "ROUTINE_VISITOR_CENTER", menu =>
            {
                menu.MenuName = "访客中心";
                menu.MenuCode = "ROUTINE_VISITOR_CENTER";
                menu.I18nKey = "menu.routine.visitor.center";
                menu.Icon = "RiUserSharedLine";
                menu.ParentId = routineMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "routine:visitor:center:list";
                menu.RoutePath = "/routine/visitor-center/visitor";
                menu.ComponentPath = "routine/visitor-center/visitor/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert6;
            updateCount += update6;
        }

        // ========== 财务核算下的二级菜单 (SortOrder: 4) ==========
        if (accountingMenu != null)
        {
            var (insertAccounting1, updateAccounting1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "ACCOUNTING_FINANCIAL", menu =>
            {
                menu.MenuName = "管理会计";
                menu.MenuCode = "ACCOUNTING_FINANCIAL";
                menu.I18nKey = "menu.accounting.financial._self";
                menu.Icon = "RiBankCardLine";
                menu.ParentId = accountingMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/accounting/financial";
                menu.ComponentPath = "accounting/financial";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAccounting1;
            updateCount += updateAccounting1;

            var (insertAccounting2, updateAccounting2) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "ACCOUNTING_CONTROLLING", menu =>
            {
                menu.MenuName = "控制会计";
                menu.MenuCode = "ACCOUNTING_CONTROLLING";
                menu.I18nKey = "menu.accounting.controlling._self";
                menu.Icon = "RiPieChartLine";
                menu.ParentId = accountingMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/accounting/controlling";
                menu.ComponentPath = "accounting/controlling";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAccounting2;
            updateCount += updateAccounting2;
        }

        // ========== 后勤管理下的二级菜单 (SortOrder: 5) ==========
        if (logisticsMenu != null)
        {
            var (insertLogistics1, updateLogistics1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_SALES", menu =>
            {
                menu.MenuName = "销售管理";
                menu.MenuCode = "LOGISTICS_SALES";
                menu.I18nKey = "menu.logistics.sales._self";
                menu.Icon = "RiShoppingCartLine";
                menu.ParentId = logisticsMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/logistics/sales";
                menu.ComponentPath = "logistics/sales";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLogistics1;
            updateCount += updateLogistics1;

            var (insertLogistics2, updateLogistics2) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MATERIALS", menu =>
            {
                menu.MenuName = "物料管理";
                menu.MenuCode = "LOGISTICS_MATERIALS";
                menu.I18nKey = "menu.logistics.materials._self";
                menu.Icon = "RiArchiveLine";
                menu.ParentId = logisticsMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/logistics/materials";
                menu.ComponentPath = "logistics/materials";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLogistics2;
            updateCount += updateLogistics2;

            var (insertLogisticsProcurement, updateLogisticsProcurement) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_PROCUREMENT", menu =>
            {
                menu.MenuName = "采购管理";
                menu.MenuCode = "LOGISTICS_PROCUREMENT";
                menu.I18nKey = "menu.logistics.procurement._self";
                menu.Icon = "RiShoppingBagLine";
                menu.ParentId = logisticsMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/logistics/procurement";
                menu.ComponentPath = "logistics/procurement";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLogisticsProcurement;
            updateCount += updateLogisticsProcurement;

            var (insertLogistics3, updateLogistics3) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING", menu =>
            {
                menu.MenuName = "生产执行";
                menu.MenuCode = "LOGISTICS_MANUFACTURING";
                menu.I18nKey = "menu.logistics.manufacturing._self";
                menu.Icon = "RiIndeterminateCircleLine";
                menu.ParentId = logisticsMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/logistics/manufacturing";
                menu.ComponentPath = "logistics/manufacturing";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLogistics3;
            updateCount += updateLogistics3;

            var (insertLogistics4, updateLogistics4) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY", menu =>
            {
                menu.MenuName = "质量管理";
                menu.MenuCode = "LOGISTICS_QUALITY";
                menu.I18nKey = "menu.logistics.quality._self";
                menu.Icon = "RiShieldCheckLine";
                menu.ParentId = logisticsMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/logistics/quality";
                menu.ComponentPath = "logistics/quality";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLogistics4;
            updateCount += updateLogistics4;

            var (insertLogistics5, updateLogistics5) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_CUSTOMER_SERVICE", menu =>
            {
                menu.MenuName = "客户服务";
                menu.MenuCode = "LOGISTICS_CUSTOMER_SERVICE";
                menu.I18nKey = "menu.logistics.customer.service._self";
                menu.Icon = "RiCustomerServiceLine";
                menu.ParentId = logisticsMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/logistics/customer-service";
                menu.ComponentPath = "logistics/customer-service";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLogistics5;
            updateCount += updateLogistics5;

            var (insertLogistics6, updateLogistics6) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MAINTENANCE", menu =>
            {
                menu.MenuName = "工厂维护";
                menu.MenuCode = "LOGISTICS_MAINTENANCE";
                menu.I18nKey = "menu.logistics.maintenance._self";
                menu.Icon = "RiToolsLine";
                menu.ParentId = logisticsMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/logistics/maintenance";
                menu.ComponentPath = "logistics/maintenance";
                menu.SortOrder = 7;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLogistics6;
            updateCount += updateLogistics6;

            var (insertLogistics7, updateLogistics7) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_SERIAL", menu =>
            {
                menu.MenuName = "序列号管理";
                menu.MenuCode = "LOGISTICS_SERIAL";
                menu.I18nKey = "menu.logistics.serial._self";
                menu.Icon = "RiBarcodeLine";
                menu.ParentId = logisticsMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/logistics/serial";
                menu.ComponentPath = "logistics/serial";
                menu.SortOrder = 8;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLogistics7;
            updateCount += updateLogistics7;
        }

        // ========== 人力资源下的二级菜单 (SortOrder: 6) ==========
        if (humanResourceMenu != null)
        {
            var (insertHR1, updateHR1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "HUMAN_RESOURCE_ORGANIZATION", menu =>
            {
                menu.MenuName = "组织管理";
                menu.MenuCode = "HUMAN_RESOURCE_ORGANIZATION";
                menu.I18nKey = "menu.human.resource.organization._self";
                menu.Icon = "RiOrganizationChart";
                menu.ParentId = humanResourceMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/human-resource/organization";
                menu.ComponentPath = "human-resource/organization";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHR1;
            updateCount += updateHR1;

            var (insertHR2, updateHR2) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "HUMAN_RESOURCE_PERSONNEL", menu =>
            {
                menu.MenuName = "人事管理";
                menu.MenuCode = "HUMAN_RESOURCE_PERSONNEL";
                menu.I18nKey = "menu.human.resource.personnel._self";
                menu.Icon = "RiUserSettingsLine";
                menu.ParentId = humanResourceMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/human-resource/personnel";
                menu.ComponentPath = "human-resource/personnel";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHR2;
            updateCount += updateHR2;

            var (insertHR3, updateHR3) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "HUMAN_RESOURCE_ATTENDANCE", menu =>
            {
                menu.MenuName = "考勤管理";
                menu.MenuCode = "HUMAN_RESOURCE_ATTENDANCE";
                menu.I18nKey = "menu.human.resource.attendance._self";
                menu.Icon = "RiCalendarCheckLine";
                menu.ParentId = humanResourceMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/human-resource/attendance";
                menu.ComponentPath = "human-resource/attendance";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHR3;
            updateCount += updateHR3;

            var (insertHR4, updateHR4) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "HUMAN_RESOURCE_COMPENSATION", menu =>
            {
                menu.MenuName = "薪酬管理";
                menu.MenuCode = "HUMAN_RESOURCE_COMPENSATION";
                menu.I18nKey = "menu.human.resource.compensation._self";
                menu.Icon = "RiMoneyCnyCircleLine";
                menu.ParentId = humanResourceMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/human-resource/compensation";
                menu.ComponentPath = "human-resource/compensation";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHR4;
            updateCount += updateHR4;

            var (insertHR4B, updateHR4B) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "HUMAN_RESOURCE_BENEFITS", menu =>
            {
                menu.MenuName = "福利管理";
                menu.MenuCode = "HUMAN_RESOURCE_BENEFITS";
                menu.I18nKey = "menu.human.resource.benefits._self";
                menu.Icon = "RiGiftLine";
                menu.ParentId = humanResourceMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/human-resource/benefits";
                menu.ComponentPath = "human-resource/benefits";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHR4B;
            updateCount += updateHR4B;

            var (insertHR5, updateHR5) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "HUMAN_RESOURCE_PERFORMANCE", menu =>
            {
                menu.MenuName = "绩效管理";
                menu.MenuCode = "HUMAN_RESOURCE_PERFORMANCE";
                menu.I18nKey = "menu.human.resource.performance._self";
                menu.Icon = "RiTargetLine";
                menu.ParentId = humanResourceMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/human-resource/performance";
                menu.ComponentPath = "human-resource/performance";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHR5;
            updateCount += updateHR5;

            var (insertHR6, updateHR6) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "HUMAN_RESOURCE_TRAINING", menu =>
            {
                menu.MenuName = "教育培训";
                menu.MenuCode = "HUMAN_RESOURCE_TRAINING";
                menu.I18nKey = "menu.human.resource.training._self";
                menu.Icon = "RiBookOpenLine";
                menu.ParentId = humanResourceMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/human-resource/training";
                menu.ComponentPath = "human-resource/training";
                menu.SortOrder = 7;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHR6;
            updateCount += updateHR6;

            var (insertHR7, updateHR7) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "HUMAN_RESOURCE_TALENT", menu =>
            {
                menu.MenuName = "人才管理";
                menu.MenuCode = "HUMAN_RESOURCE_TALENT";
                menu.I18nKey = "menu.human.resource.talent._self";
                menu.Icon = "RiUserStarLine";
                menu.ParentId = humanResourceMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/human-resource/talent";
                menu.ComponentPath = "human-resource/talent";
                menu.SortOrder = 8;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHR7;
            updateCount += updateHR7;
        }

        // ========== 身份认证下的二级菜单 (SortOrder: 7) ==========
        if (identityMenu != null)
        {
            var (insertIdentity1, updateIdentity1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "IDENTITY_TENANT", menu =>
            {
                menu.MenuName = "租户管理";
                menu.MenuCode = "IDENTITY_TENANT";
                menu.I18nKey = "menu.identity.tenant";
                menu.Icon = "RiBuildingLine";
                menu.ParentId = identityMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "identity:tenant:list";
                menu.RoutePath = "/identity/tenant";
                menu.ComponentPath = "identity/tenant/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertIdentity1;
            updateCount += updateIdentity1;

            var (insertIdentity2, updateIdentity2) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "IDENTITY_USER", menu =>
            {
                menu.MenuName = "用户管理";
                menu.MenuCode = "IDENTITY_USER";
                menu.I18nKey = "menu.identity.user";
                menu.Icon = "RiUserLine";
                menu.ParentId = identityMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "identity:user:list";
                menu.RoutePath = "/identity/user";
                menu.ComponentPath = "identity/user/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertIdentity2;
            updateCount += updateIdentity2;

            var (insertIdentity3, updateIdentity3) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "IDENTITY_MENU", menu =>
            {
                menu.MenuName = "菜单管理";
                menu.MenuCode = "IDENTITY_MENU";
                menu.I18nKey = "menu.identity.menu";
                menu.Icon = "RiMenuLine";
                menu.ParentId = identityMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "identity:menu:list";
                menu.RoutePath = "/identity/menu";
                menu.ComponentPath = "identity/menu/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertIdentity3;
            updateCount += updateIdentity3;

            var (insertIdentity4, updateIdentity4) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "IDENTITY_ROLE", menu =>
            {
                menu.MenuName = "角色管理";
                menu.MenuCode = "IDENTITY_ROLE";
                menu.I18nKey = "menu.identity.role";
                menu.Icon = "RiShieldUserLine";
                menu.ParentId = identityMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "identity:role:list";
                menu.RoutePath = "/identity/role";
                menu.ComponentPath = "identity/role/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertIdentity4;
            updateCount += updateIdentity4;
        }

        // ========== 工作流下的二级菜单 (SortOrder: 8) ==========
        if (workflowMenu != null)
        {
            var (insert3, update3) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "WORKFLOW_TODO", menu =>
            {
                menu.MenuName = "待办事项";
                menu.MenuCode = "WORKFLOW_TODO";
                menu.I18nKey = "menu.workflow.todo";
                menu.Icon = "RiInboxArchiveLine";
                menu.ParentId = workflowMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "workflow:todo:list";
                menu.RoutePath = "/workflow/todo";
                menu.ComponentPath = "workflow/todo/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert3;
            updateCount += update3;

            var (insert4, update4) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "WORKFLOW_MY", menu =>
            {
                menu.MenuName = "我的流程";
                menu.MenuCode = "WORKFLOW_MY";
                menu.I18nKey = "menu.workflow.my";
                menu.Icon = "RiDraftLine";
                menu.ParentId = workflowMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "workflow:my:list";
                menu.RoutePath = "/workflow/my";
                menu.ComponentPath = "workflow/my/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert4;
            updateCount += update4;

            var (insert5, update5) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "WORKFLOW_PROCESSED", menu =>
            {
                menu.MenuName = "已处理";
                menu.MenuCode = "WORKFLOW_PROCESSED";
                menu.I18nKey = "menu.workflow.processed";
                menu.Icon = "RiCheckboxCircleLine";
                menu.ParentId = workflowMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "workflow:processed:list";
                menu.RoutePath = "/workflow/processed";
                menu.ComponentPath = "workflow/processed/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert5;
            updateCount += update5;

            var (insert5a, update5a) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "WORKFLOW_INSTANCE", menu =>
            {
                menu.MenuName = "流程实例";
                menu.MenuCode = "WORKFLOW_INSTANCE";
                menu.I18nKey = "menu.workflow.instance";
                menu.Icon = "RiListUnordered";
                menu.ParentId = workflowMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "workflow:instance:list";
                menu.RoutePath = "/workflow/instance";
                menu.ComponentPath = "workflow/instance/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert5a;
            updateCount += update5a;

            var (insert6, update6) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "WORKFLOW_SCHEME", menu =>
            {
                menu.MenuName = "流程方案";
                menu.MenuCode = "WORKFLOW_SCHEME";
                menu.I18nKey = "menu.workflow.scheme";
                menu.Icon = "RiOrganizationChart";
                menu.ParentId = workflowMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "workflow:scheme:list";
                menu.RoutePath = "/workflow/scheme";
                menu.ComponentPath = "workflow/scheme/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert6;
            updateCount += update6;

            var (insert7, update7) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "WORKFLOW_FORM", menu =>
            {
                menu.MenuName = "表单管理";
                menu.MenuCode = "WORKFLOW_FORM";
                menu.I18nKey = "menu.workflow.form";
                menu.Icon = "RiFileList3Line";
                menu.ParentId = workflowMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "workflow:form:list";
                menu.RoutePath = "/workflow/form";
                menu.ComponentPath = "workflow/form/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert7;
            updateCount += update7;
        }

        // ========== 代码管理下的二级菜单 (SortOrder: 9) ==========
        if (codeMenu != null)
        {
            var (insertCode1, updateCode1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "CODE_GENERATOR", menu =>
            {
                menu.MenuName = "代码生成";
                menu.MenuCode = "CODE_GENERATOR";
                menu.I18nKey = "menu.code.generator";
                menu.Icon = "RiCodeSSlashLine";
                menu.ParentId = codeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "code:generator:list";
                menu.RoutePath = "/code/generator";
                menu.ComponentPath = "code/generator/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertCode1;
            updateCount += updateCode1;

            var (insertCodeDb, updateCodeDb) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "CODE_DATABASE_INFO", menu =>
            {
                menu.MenuName = "数据库信息";
                menu.MenuCode = "CODE_DATABASE_INFO";
                menu.I18nKey = "menu.code.database.info";
                menu.Icon = "RiDatabase2Line";
                menu.ParentId = codeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "code:database:info:list";
                menu.RoutePath = "/code/database/database-info";
                menu.ComponentPath = "code/database/database-info/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertCodeDb;
            updateCount += updateCodeDb;

            var (insertCodeTableClone, updateCodeTableClone) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "CODE_DATABASE_TABLE_CLONE", menu =>
            {
                menu.MenuName = "表克隆";
                menu.MenuCode = "CODE_DATABASE_TABLE_CLONE";
                menu.I18nKey = "menu.code.database.table.clone";
                menu.Icon = "RiFileCopy2Line";
                menu.ParentId = codeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "code:database:table:clone:list";
                menu.RoutePath = "/code/database/table-clone";
                menu.ComponentPath = "code/database/table-clone/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertCodeTableClone;
            updateCount += updateCodeTableClone;

            var (insertCodeDataClone, updateCodeDataClone) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "CODE_DATABASE_DATA_CLONE", menu =>
            {
                menu.MenuName = "数据克隆";
                menu.MenuCode = "CODE_DATABASE_DATA_CLONE";
                menu.I18nKey = "menu.code.database.data.clone";
                menu.Icon = "RiFileCopyLine";
                menu.ParentId = codeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "code:database:data:clone:list";
                menu.RoutePath = "/code/database/data-clone";
                menu.ComponentPath = "code/database/data-clone/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertCodeDataClone;
            updateCount += updateCodeDataClone;

            var (insertCodeDataArchive, updateCodeDataArchive) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "CODE_DATABASE_TABLE_ARCHIVE", menu =>
            {
                menu.MenuName = "数据表归档";
                menu.MenuCode = "CODE_DATABASE_TABLE_ARCHIVE";
                menu.I18nKey = "menu.code.database.table.archive";
                menu.Icon = "RiArchiveDrawerLine";
                menu.ParentId = codeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "code:database:table:archive:list";
                menu.RoutePath = "/code/database/table-archive";
                menu.ComponentPath = "code/database/table-archive/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertCodeDataArchive;
            updateCount += updateCodeDataArchive;

            var (insertCodeDbBackup, updateCodeDbBackup) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "CODE_DATABASE_BACKUP", menu =>
            {
                menu.MenuName = "数据库备份";
                menu.MenuCode = "CODE_DATABASE_BACKUP";
                menu.I18nKey = "menu.code.database.backup";
                menu.Icon = "RiDatabaseLine";
                menu.ParentId = codeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "code:database:backup:list";
                menu.RoutePath = "/code/database/database-backup";
                menu.ComponentPath = "code/database/database-backup/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertCodeDbBackup;
            updateCount += updateCodeDbBackup;
        }

        // ========== 基础数据下的二级菜单 (SortOrder: 10) ==========
        if (foundationMenu != null)
        {
            var (insertFoundation1, updateFoundation1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "FOUNDATION_NUMBERING", menu =>
            {
                menu.MenuName = "编码规则";
                menu.MenuCode = "FOUNDATION_NUMBERING";
                menu.I18nKey = "menu.foundation.numbering";
                menu.Icon = "RiNumbersLine";
                menu.ParentId = foundationMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "foundation:numbering:list";
                menu.RoutePath = "/foundation/numbering";
                menu.ComponentPath = "foundation/numbering/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertFoundation1;
            updateCount += updateFoundation1;

            var (insertFoundation2, updateFoundation2) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "FOUNDATION_DICT", menu =>
            {
                menu.MenuName = "数据字典";
                menu.MenuCode = "FOUNDATION_DICT";
                menu.I18nKey = "menu.foundation.dict";
                menu.Icon = "RiBook2Line";
                menu.ParentId = foundationMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "foundation:dict:list";
                menu.RoutePath = "/foundation/dict";
                menu.ComponentPath = "foundation/dict/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertFoundation2;
            updateCount += updateFoundation2;

            var (insertFoundation3, updateFoundation3) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "FOUNDATION_I18N", menu =>
            {
                menu.MenuName = "国际化";
                menu.MenuCode = "FOUNDATION_I18N";
                menu.I18nKey = "menu.foundation.i18n";
                menu.Icon = "RiTranslate";
                menu.ParentId = foundationMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "foundation:i18n:list";
                menu.RoutePath = "/foundation/i18n";
                menu.ComponentPath = "foundation/i18n/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertFoundation3;
            updateCount += updateFoundation3;

            var (insertFoundation4, updateFoundation4) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "FOUNDATION_FILE", menu =>
            {
                menu.MenuName = "文件管理";
                menu.MenuCode = "FOUNDATION_FILE";
                menu.I18nKey = "menu.foundation.file";
                menu.Icon = "RiFileLine";
                menu.ParentId = foundationMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "foundation:file:list";
                menu.RoutePath = "/foundation/file";
                menu.ComponentPath = "foundation/file/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertFoundation4;
            updateCount += updateFoundation4;

            var (insertFoundation6, updateFoundation6) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "FOUNDATION_CACHE", menu =>
            {
                menu.MenuName = "缓存管理";
                menu.MenuCode = "FOUNDATION_CACHE";
                menu.I18nKey = "menu.foundation.cache";
                menu.Icon = "RiDatabase2Line";
                menu.ParentId = foundationMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "foundation:cache:list";
                menu.RoutePath = "/foundation/cache";
                menu.ComponentPath = "foundation/cache/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertFoundation6;
            updateCount += updateFoundation6;

            var (insertFoundation7, updateFoundation7) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "FOUNDATION_VOCABULARY", menu =>
            {
                // 对应 TaktVocabulary（takt_foundation_vocabulary），租户级敏感词库
                menu.MenuName = "敏感词库";
                menu.MenuCode = "FOUNDATION_VOCABULARY";
                menu.I18nKey = "menu.foundation.vocabulary";
                menu.Icon = "RiProhibitedLine";
                menu.ParentId = foundationMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "foundation:vocabulary:list";
                menu.RoutePath = "/foundation/vocabulary";
                menu.ComponentPath = "foundation/vocabulary/index";
                menu.SortOrder = 7;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertFoundation7;
            updateCount += updateFoundation7;

            var (insertFoundation8, updateFoundation8) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "FOUNDATION_SYSTEM_PROPERTY", menu =>
            {
                menu.MenuName = "系统属性";
                menu.MenuCode = "FOUNDATION_SYSTEM_PROPERTY";
                menu.I18nKey = "menu.foundation.system.property";
                menu.Icon = "RiSettingsLine";
                menu.ParentId = foundationMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "foundation:system:property:list";
                menu.RoutePath = "/foundation/system-property";
                menu.ComponentPath = "foundation/system-property/index";
                menu.SortOrder = 8;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertFoundation8;
            updateCount += updateFoundation8;

            var (insertFoundation9, updateFoundation9) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "FOUNDATION_ONLINE", menu =>
            {
                menu.MenuName = "在线用户";
                menu.MenuCode = "FOUNDATION_ONLINE";
                menu.I18nKey = "menu.foundation.online";
                menu.Icon = "RiUserVoiceLine";
                menu.ParentId = foundationMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "foundation:online:list";
                menu.RoutePath = "/foundation/online";
                menu.ComponentPath = "foundation/online/index";
                menu.SortOrder = 9;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertFoundation9;
            updateCount += updateFoundation9;

            var (insertFoundation10, updateFoundation10) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "FOUNDATION_MESSAGE", menu =>
            {
                menu.MenuName = "在线消息";
                menu.MenuCode = "FOUNDATION_MESSAGE";
                menu.I18nKey = "menu.foundation.message";
                menu.Icon = "RiMessage2Line";
                menu.ParentId = foundationMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "foundation:message:list";
                menu.RoutePath = "/foundation/message";
                menu.ComponentPath = "foundation/message/index";
                menu.SortOrder = 10;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertFoundation10;
            updateCount += updateFoundation10;

            var (insertFoundationQuartzTask, updateFoundationQuartzTask) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "FOUNDATION_QUARTZ_TASK", menu =>
            {
                menu.MenuName = "定时任务";
                menu.MenuCode = "FOUNDATION_QUARTZ_TASK";
                menu.I18nKey = "menu.foundation.quartz.task";
                menu.Icon = "RiTimerLine";
                menu.ParentId = foundationMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "foundation:quartz:task:list";
                menu.RoutePath = "/foundation/quartz-task";
                menu.ComponentPath = "foundation/quartz-task/index";
                menu.SortOrder = 11;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertFoundationQuartzTask;
            updateCount += updateFoundationQuartzTask;

            var (insertFoundationAdminDivision, updateFoundationAdminDivision) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "FOUNDATION_ADMIN_DIVISION", menu =>
            {
                menu.MenuName = "行政区划";
                menu.MenuCode = "FOUNDATION_ADMIN_DIVISION";
                menu.I18nKey = "menu.foundation.admin.division";
                menu.Icon = "RiMap2Line";
                menu.ParentId = foundationMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "foundation:admin:division:list";
                menu.RoutePath = "/foundation/admin-division";
                menu.ComponentPath = "foundation/admin-division/index";
                menu.SortOrder = 12;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertFoundationAdminDivision;
            updateCount += updateFoundationAdminDivision;

            var (insertFoundationIpGeolocation, updateFoundationIpGeolocation) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "FOUNDATION_IP_GEOLOCATION", menu =>
            {
                menu.MenuName = "IP归属";
                menu.MenuCode = "FOUNDATION_IP_GEOLOCATION";
                menu.I18nKey = "menu.foundation.ip.geolocation";
                menu.Icon = "RiMapPinLine";
                menu.ParentId = foundationMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "foundation:ip:geolocation:list";
                menu.RoutePath = "/foundation/ip-geolocation";
                menu.ComponentPath = "foundation/ip-geolocation/index";
                menu.SortOrder = 13;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertFoundationIpGeolocation;
            updateCount += updateFoundationIpGeolocation;
        }

        // ========== 统计看板下的二级菜单 (SortOrder: 11) ==========
        if (statisticsMenu != null)
        {
            var (insertStatistics1, updateStatistics1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "STATISTICS_QUICK_QUERY", menu =>
            {
                menu.MenuName = "快速查询";
                menu.MenuCode = "STATISTICS_QUICK_QUERY";
                menu.I18nKey = "menu.statistics.quickquery._self";
                menu.Icon = "RiSearchLine";
                menu.ParentId = statisticsMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/statistics/quick-query";
                menu.ComponentPath = "statistics/quick-query";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertStatistics1;
            updateCount += updateStatistics1;

            var (insertStatistics2, updateStatistics2) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "STATISTICS_LOGGING", menu =>
            {
                menu.MenuName = "日志管理";
                menu.MenuCode = "STATISTICS_LOGGING";
                menu.I18nKey = "menu.statistics.logging._self";
                menu.Icon = "RiFileList3Line";
                menu.ParentId = statisticsMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/statistics/logging";
                menu.ComponentPath = "statistics/logging";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertStatistics2;
            updateCount += updateStatistics2;
        }

        return (insertCount, updateCount);
    }

    /// <summary>
    /// 创建或更新菜单。
    /// </summary>
    /// <param name="menuRepository">菜单仓储。</param>
    /// <param name="tenantCode">租户编码。</param>
    /// <param name="menuCode">菜单编码（业务键）。</param>
    /// <param name="configure">菜单配置委托。</param>
    /// <returns>元组：(InsertCount, UpdateCount)，本条菜单新增或更新条数（0或1）。</returns>
    private static async Task<(int InsertCount, int UpdateCount)> CreateOrUpdateMenuAsync(
        ITaktTenantSeedRepository<TaktMenu> menuRepository,
        TaktSeedContext sqlSugarContext,
        string tenantCode,
        string menuCode,
        Action<TaktMenu> configure)
    {
        // 注意：种子数据必须绕过仓储的租户过滤，直接使用 SqlSugar 原生查询
        var menu = await sqlSugarContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == menuCode && m.IsDeleted == 0)
            .FirstAsync();
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
            // CreatedBy 和 CreatedAt 由 ITaktSeedRepository 自动填充
            
            menu = await menuRepository.CreateAsync(menu);
            
            // 更新 MenuPath 和 Level
            if (menu.ParentId > 0)
            {
                // 注意：必须绕过仓储的租户过滤，直接查询
                var parentMenu = await sqlSugarContext.Db.Queryable<TaktMenu>()
                    .Where(m => m.Id == menu.ParentId && m.IsDeleted == 0)
                    .FirstAsync();
                if (parentMenu != null)
                {
                    menu.MenuPath = $"{parentMenu.MenuPath}{menu.Id}/";
                    menu.Level = parentMenu.Level + 1;
                    
                    // 更新父级 IsLeaf 为非叶子
                    if (parentMenu.IsLeaf == 1)
                    {
                        parentMenu.IsLeaf = 0;
                        parentMenu.UpdatedAt = DateTime.Now;
                        await sqlSugarContext.Db.Updateable(parentMenu).ExecuteCommandAsync();
                    }
                }
            }
            else
            {
                menu.MenuPath = $"/{menu.Id}/";
                menu.Level = 1;
            }
            
            menu.UpdatedBy = 900001;
            menu.UpdatedAt = DateTime.Now;
            await sqlSugarContext.Db.Updateable(menu).ExecuteCommandAsync();
            return (1, 0);
        }
        else
        {
            configure(menu);
            menu.IsBuiltIn = 1;
            
            // 重新计算 Level 和 MenuPath（如果 ParentId 发生变化）
            if (menu.ParentId > 0)
            {
                // 注意：必须绕过仓储的租户过滤，直接查询
                var parentMenu = await sqlSugarContext.Db.Queryable<TaktMenu>()
                    .Where(m => m.Id == menu.ParentId && m.IsDeleted == 0)
                    .FirstAsync();
                if (parentMenu != null)
                {
                    menu.MenuPath = $"{parentMenu.MenuPath}{menu.Id}/";
                    menu.Level = parentMenu.Level + 1;
                    
                    // 更新父级 IsLeaf 为非叶子
                    if (parentMenu.IsLeaf == 1)
                    {
                        parentMenu.IsLeaf = 0;
                        parentMenu.UpdatedBy = 900001;
                        parentMenu.UpdatedAt = DateTime.Now;
                        await sqlSugarContext.Db.Updateable(parentMenu).ExecuteCommandAsync();
                    }
                }
            }
            else
            {
                menu.MenuPath = $"/{menu.Id}/";
                menu.Level = 1;
            }
            
            menu.UpdatedBy = 900001;
            menu.UpdatedAt = DateTime.Now;
            
            await sqlSugarContext.Db.Updateable(menu).ExecuteCommandAsync();
            return (0, 1);
        }
    }
}
