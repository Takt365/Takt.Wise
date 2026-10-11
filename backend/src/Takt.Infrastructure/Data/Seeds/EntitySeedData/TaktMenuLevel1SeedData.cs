// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds
// 文件名称：TaktMenuLevel1SeedData.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt 一级（顶级）菜单种子数据。
//           初始化 ParentId = 0 的根节点：含主页、各业务域目录（MenuType=0）及少量直接挂接页面的根菜单（MenuType=1）。
//           顺序与 SortOrder 需与 TaktMenuLevel2SeedData 中父级引用保持一致。
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
/// Takt 一级菜单种子数据。
/// <para>
/// 定义系统侧边栏最顶层节点（如仪表盘、工作流、日常事务、后勤、人力资源等），
/// 二级及以下菜单在 TaktMenuLevel2SeedData 及后续 Level 中展开。
/// 由 TaktMenuSeedData 统一协调调用，不直接注册为 ITaktSeedDataCoordinator。
/// </para>
/// </summary>
public class TaktMenuLevel1SeedData
{
    /// <summary>
    /// 初始化一级菜单种子数据。
    /// <para>
    /// 对每个预置 MenuCode 执行"不存在则插入、存在则更新"，同步名称、图标、路径、组件、排序及可见性等字段，
    /// 由 TaktMenuSeedData 协调器调用。
    /// </para>
    /// </summary>
    /// <param name="serviceProvider">服务提供者，用于解析 ITaktRepository{TaktMenu}。</param>
    /// <param name="tenantCode">租户编码（由协调器传入）。</param>
    /// <returns>元组：(InsertCount, UpdateCount)，分别为本次新增与更新的一级菜单条数。</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? specifiedTenantCode = null)
    {
        var menuRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktMenu>>();
        var sqlSugarContext = serviceProvider.GetRequiredService<TaktSeedContext>();

        int insertCount = 0;
        int updateCount = 0;

        // 一级菜单:ParentId=0(根节点)
        // 注意:菜单为租户级实体,由协调器指定租户,只处理当前租户
        var tenantCodes = specifiedTenantCode != null 
            ? new[] { specifiedTenantCode } 
            : Array.Empty<string>();
        
        if (tenantCodes.Length == 0)
        {
            TaktLogger.Warning("未指定租户编码,跳过一级菜单种子数据初始化");
            return (0, 0);
        }
        
        foreach (var tc in tenantCodes)
        {
            // 1. 主页
            var homeResult = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tc, "HOME", menu =>
            {
                menu.MenuName = "主页";
                menu.MenuCode = "HOME";
                menu.I18nKey = "menu.home";
                menu.Icon = "RiHomeLine";
                menu.ParentId = 0;
                menu.MenuType = 1;
                menu.Permission = "home:info:list";
                menu.RoutePath = "/home";
                menu.ComponentPath = "home/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 0;
                menu.IsVisible = 0;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += homeResult.InsertCount;
            updateCount += homeResult.UpdateCount;

            // 2. 仪表盘(目录)
            var dashboardResult = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tc, "DASHBOARD", menu =>
            {
                menu.MenuName = "仪表盘";
                menu.MenuCode = "DASHBOARD";
                menu.I18nKey = "menu.dashboard._self";
                menu.Icon = "RiDashboardLine";
                menu.ParentId = 0;
                menu.MenuType = 0;
                menu.RoutePath = "/dashboard";
                menu.ComponentPath = "dashboard";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += dashboardResult.InsertCount;
            updateCount += dashboardResult.UpdateCount;

            // 3. 日常事务(目录)
            var routineResult = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tc, "ROUTINE", menu =>
            {
                menu.MenuName = "日常事务";
                menu.MenuCode = "ROUTINE";
                menu.I18nKey = "menu.routine._self";
                menu.Icon = "RiCalendarScheduleLine";
                menu.ParentId = 0;
                menu.MenuType = 0;
                menu.RoutePath = "/routine";
                menu.ComponentPath = "routine";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += routineResult.InsertCount;
            updateCount += routineResult.UpdateCount;

            // 4. 财务核算(目录)
            var accountingResult = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tc, "ACCOUNTING", menu =>
            {
                menu.MenuName = "财务核算";
                menu.MenuCode = "ACCOUNTING";
                menu.I18nKey = "menu.accounting._self";
                menu.Icon = "RiBankLine";
                menu.ParentId = 0;
                menu.MenuType = 0;
                menu.RoutePath = "/accounting";
                menu.ComponentPath = "accounting";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += accountingResult.InsertCount;
            updateCount += accountingResult.UpdateCount;

            // 5. 后勤管理(目录)
            var logisticsResult = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tc, "LOGISTICS", menu =>
            {
                menu.MenuName = "后勤管理";
                menu.MenuCode = "LOGISTICS";
                menu.I18nKey = "menu.logistics._self";
                menu.Icon = "RiLayoutGridLine";
                menu.ParentId = 0;
                menu.MenuType = 0;
                menu.RoutePath = "/logistics";
                menu.ComponentPath = "logistics";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += logisticsResult.InsertCount;
            updateCount += logisticsResult.UpdateCount;

            // 6. 人力资源(目录)
            var humanResourceResult = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tc, "HUMAN_RESOURCE", menu =>
            {
                menu.MenuName = "人力资源";
                menu.MenuCode = "HUMAN_RESOURCE";
                menu.I18nKey = "menu.human.resource._self";
                menu.Icon = "RiTeamLine";
                menu.ParentId = 0;
                menu.MenuType = 0;
                menu.RoutePath = "/human-resource";
                menu.ComponentPath = "human-resource";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += humanResourceResult.InsertCount;
            updateCount += humanResourceResult.UpdateCount;

            // 7. 身份认证(目录)
            var identityResult = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tc, "IDENTITY", menu =>
            {
                menu.MenuName = "身份认证";
                menu.MenuCode = "IDENTITY";
                menu.I18nKey = "menu.identity._self";
                menu.Icon = "RiUserLine";
                menu.ParentId = 0;
                menu.MenuType = 0;
                menu.RoutePath = "/identity";
                menu.ComponentPath = "identity";
                menu.SortOrder = 7;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += identityResult.InsertCount;
            updateCount += identityResult.UpdateCount;

            // 8. 工作流(目录)
            var workflowResult = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tc, "WORKFLOW", menu =>
            {
                menu.MenuName = "工作流";
                menu.MenuCode = "WORKFLOW";
                menu.I18nKey = "menu.workflow._self";
                menu.Icon = "RiNodeTree";
                menu.ParentId = 0;
                menu.MenuType = 0;
                menu.RoutePath = "/workflow";
                menu.ComponentPath = "workflow";
                menu.SortOrder = 8;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += workflowResult.InsertCount;
            updateCount += workflowResult.UpdateCount;

            // 9. 代码管理(目录)
            var codeResult = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tc, "CODE", menu =>
            {
                menu.MenuName = "代码管理";
                menu.MenuCode = "CODE";
                menu.I18nKey = "menu.code._self";
                menu.Icon = "RiQrCodeLine";
                menu.ParentId = 0;
                menu.MenuType = 0;
                menu.RoutePath = "/code";
                menu.ComponentPath = "code";
                menu.SortOrder = 9;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += codeResult.InsertCount;
            updateCount += codeResult.UpdateCount;

            // 10. 基础数据(目录)
            var foundationResult = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tc, "FOUNDATION", menu =>
            {
                menu.MenuName = "基础数据";
                menu.MenuCode = "FOUNDATION";
                menu.I18nKey = "menu.foundation._self";
                menu.Icon = "RiSettings3Line";
                menu.ParentId = 0;
                menu.MenuType = 0;
                menu.RoutePath = "/foundation";
                menu.ComponentPath = "foundation";
                menu.SortOrder = 10;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += foundationResult.InsertCount;
            updateCount += foundationResult.UpdateCount;

            // 11. 统计看板(目录)
            var statisticsResult = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tc, "STATISTICS", menu =>
            {
                menu.MenuName = "统计看板";
                menu.MenuCode = "STATISTICS";
                menu.I18nKey = "menu.statistics._self";
                menu.Icon = "RiBarChart2Line";
                menu.ParentId = 0;
                menu.MenuType = 0;
                menu.RoutePath = "/statistics";
                menu.ComponentPath = "statistics";
                menu.SortOrder = 11;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += statisticsResult.InsertCount;
            updateCount += statisticsResult.UpdateCount;

            // 12. 关于(页面；配置模式同主页 MenuType=1 + componentPath，侧栏可见；仅主页 IsVisible=0)
            var aboutResult = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tc, "ABOUT", menu =>
            {
                menu.MenuName = "关于";
                menu.MenuCode = "ABOUT";
                menu.I18nKey = "menu.about";
                menu.Icon = "RiInformationLine";
                menu.ParentId = 0;
                menu.MenuType = 1;
                menu.Permission = "about:info:list";
                menu.RoutePath = "/about";
                menu.ComponentPath = "about/index";
                menu.SortOrder = 12;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += aboutResult.InsertCount;
            updateCount += aboutResult.UpdateCount;
        }

        return (insertCount, updateCount);
    }

    /// <summary>
    /// 创建或更新菜单。
    /// </summary>
    /// <param name="menuRepository">菜单仓储。</param>
    /// <param name="sqlSugarContext">SqlSugar上下文。</param>
    /// <param name="tenantCode">租户编码。</param>
    /// <param name="menuCode">菜单编码(业务键)。</param>
    /// <param name="configure">菜单配置委托。</param>
    /// <returns>元组:(InsertCount, UpdateCount, MenuId),本条菜单新增或更新条数(0或1)以及菜单ID。</returns>
    private static async Task<(int InsertCount, int UpdateCount, long MenuId)> CreateOrUpdateMenuAsync(
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
            
            // 自动计算 Level(根节点为 1)
            menu.Level = menu.ParentId > 0 ? 0 : 1; // 稍后根据父级计算
            menu.IsLeaf = 1; // 默认为叶子节点,后续创建子菜单时会自动更新
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
            
            // 更新 Level 和 MenuPath
            menu.UpdatedBy = 900001;
            menu.UpdatedAt = DateTime.Now;
            await sqlSugarContext.Db.Updateable(menu).ExecuteCommandAsync();
            return (1, 0, menu.Id);
        }
        else
        {
            // 存在：保存旧值用于比较
            var oldMenuName = menu.MenuName;
            var oldIcon = menu.Icon;
            var oldRoutePath = menu.RoutePath;
            var oldComponentPath = menu.ComponentPath;
            var oldPermission = menu.Permission;
            var oldSortOrder = menu.SortOrder;
            var oldMenuStatus = menu.MenuStatus;
            var oldIsVisible = menu.IsVisible;
            var oldParentId = menu.ParentId;
            var oldIsBuiltIn = menu.IsBuiltIn;
            
            // 应用配置
            configure(menu);
            menu.IsBuiltIn = 1;
            
            // 检查是否有变化
            bool needUpdate = oldMenuName != menu.MenuName ||
                            oldIcon != menu.Icon ||
                            oldRoutePath != menu.RoutePath ||
                            oldComponentPath != menu.ComponentPath ||
                            oldPermission != menu.Permission ||
                            oldSortOrder != menu.SortOrder ||
                            oldMenuStatus != menu.MenuStatus ||
                            oldIsVisible != menu.IsVisible ||
                            oldIsBuiltIn != menu.IsBuiltIn;
            
            // 重新计算 Level 和 MenuPath（如果 ParentId 发生变化）
            if (menu.ParentId != oldParentId)
            {
                needUpdate = true;
                
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
            }
            
            // 只有数据发生变化时才更新
            if (needUpdate)
            {
                menu.UpdatedBy = 900001;
                menu.UpdatedAt = DateTime.Now;
                            
                // 直接使用 SqlSugar 原生 API 更新(参照 TaktDeptSeedData 实现)
                await sqlSugarContext.Db.Updateable(menu).ExecuteCommandAsync();
            }
            
            return (0, 1, menu.Id);
        }
    }
}
