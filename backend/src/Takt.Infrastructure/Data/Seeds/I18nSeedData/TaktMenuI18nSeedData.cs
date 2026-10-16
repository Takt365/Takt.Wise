// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData
// 文件名称：TaktMenuI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Cursor AI)
// 功能描述：菜单导航国际化翻译种子（menu.* 键，与 TaktMenuLevel1~4SeedData I18nKey 对齐）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险.
// ========================================

using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData;

/// <summary>
/// 菜单导航国际化翻译种子（键前缀 menu.*，与菜单种子 I18nKey 一致）
/// 幂等性：存在则更新，不存在则创建
/// TranslationText 为菜单显示名 MenuName；由 generate-menu-i18n-seed.cjs 从 Level1~4 种子生成
/// 与 I18nSeedData.Identity.TaktMenuI18nSeedData（entity.menu.* 实体字段）职责不同
/// </summary>
public class TaktMenuI18nSeedData : ITaktSeedDataCoordinator
{
    /// <summary>
    /// 执行顺序（在问候语翻译之前，菜单种子落库之后）
    /// </summary>
    public int Order => 47;

    /// <summary>
    /// 初始化菜单导航国际化翻译种子
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>插入数与更新数</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化菜单导航国际化翻译种子...");

        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过菜单导航国际化翻译种子");
            return (0, 0);
        }

        var repository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktTranslation>>();
        var cultureRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCulture>>();
        var cultureIdByCode = (await cultureRepository.GetListAsync(c => c.TenantCode == tenantCode))
            .ToDictionary(c => c.CultureCode, c => c.Id);
        int insertCount = 0;
        int updateCount = 0;

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 menu.* 翻译...", tenantCode);

        foreach (var row in GetMenuTranslations())
        {
            if (!cultureIdByCode.TryGetValue(row.CultureCode, out var cultureId))
            {
                TaktLogger.Warning("未找到区域文化 {CultureCode}，跳过翻译 {I18nKey}", row.CultureCode, row.I18nKey);
                continue;
            }

            var item = new TranslationSeedItem(row.I18nKey, row.CultureCode, row.TranslationText, row.ContextNote);
            var (_, i, u) = await CreateOrUpdateTranslationAsync(repository, tenantCode, cultureId, item);
            insertCount += i;
            updateCount += u;
        }

        TaktLogger.Information("菜单导航国际化翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);

        return (insertCount, updateCount);
    }

    /// <summary>
    /// 菜单导航翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// </summary>
    private static List<(string I18nKey, string CultureCode, string TranslationText, string? ContextNote)> GetMenuTranslations()
    {
        return new List<(string, string, string, string?)>
        {
            // menu.home
            ("menu.home", "en-US", "主页_us", "菜单导航"),
            // menu.home
            ("menu.home", "ja-JP", "主页_jp", "菜单导航"),
            // menu.home
            ("menu.home", "zh-CN", "主页", "菜单导航"),
            // menu.home
            ("menu.home", "zh-HK", "主页_hk", "菜单导航"),

            // menu.dashboard._self
            ("menu.dashboard._self", "en-US", "仪表盘_us", "菜单导航"),
            // menu.dashboard._self
            ("menu.dashboard._self", "ja-JP", "仪表盘_jp", "菜单导航"),
            // menu.dashboard._self
            ("menu.dashboard._self", "zh-CN", "仪表盘", "菜单导航"),
            // menu.dashboard._self
            ("menu.dashboard._self", "zh-HK", "仪表盘_hk", "菜单导航"),

            // menu.routine._self
            ("menu.routine._self", "en-US", "日常事务_us", "菜单导航"),
            // menu.routine._self
            ("menu.routine._self", "ja-JP", "日常事务_jp", "菜单导航"),
            // menu.routine._self
            ("menu.routine._self", "zh-CN", "日常事务", "菜单导航"),
            // menu.routine._self
            ("menu.routine._self", "zh-HK", "日常事务_hk", "菜单导航"),

            // menu.accounting._self
            ("menu.accounting._self", "en-US", "财务核算_us", "菜单导航"),
            // menu.accounting._self
            ("menu.accounting._self", "ja-JP", "财务核算_jp", "菜单导航"),
            // menu.accounting._self
            ("menu.accounting._self", "zh-CN", "财务核算", "菜单导航"),
            // menu.accounting._self
            ("menu.accounting._self", "zh-HK", "财务核算_hk", "菜单导航"),

            // menu.logistics._self
            ("menu.logistics._self", "en-US", "后勤管理_us", "菜单导航"),
            // menu.logistics._self
            ("menu.logistics._self", "ja-JP", "后勤管理_jp", "菜单导航"),
            // menu.logistics._self
            ("menu.logistics._self", "zh-CN", "后勤管理", "菜单导航"),
            // menu.logistics._self
            ("menu.logistics._self", "zh-HK", "后勤管理_hk", "菜单导航"),

            // menu.human.resource._self
            ("menu.human.resource._self", "en-US", "人力资源_us", "菜单导航"),
            // menu.human.resource._self
            ("menu.human.resource._self", "ja-JP", "人力资源_jp", "菜单导航"),
            // menu.human.resource._self
            ("menu.human.resource._self", "zh-CN", "人力资源", "菜单导航"),
            // menu.human.resource._self
            ("menu.human.resource._self", "zh-HK", "人力资源_hk", "菜单导航"),

            // menu.identity._self
            ("menu.identity._self", "en-US", "身份认证_us", "菜单导航"),
            // menu.identity._self
            ("menu.identity._self", "ja-JP", "身份认证_jp", "菜单导航"),
            // menu.identity._self
            ("menu.identity._self", "zh-CN", "身份认证", "菜单导航"),
            // menu.identity._self
            ("menu.identity._self", "zh-HK", "身份认证_hk", "菜单导航"),

            // menu.workflow._self
            ("menu.workflow._self", "en-US", "工作流_us", "菜单导航"),
            // menu.workflow._self
            ("menu.workflow._self", "ja-JP", "工作流_jp", "菜单导航"),
            // menu.workflow._self
            ("menu.workflow._self", "zh-CN", "工作流", "菜单导航"),
            // menu.workflow._self
            ("menu.workflow._self", "zh-HK", "工作流_hk", "菜单导航"),

            // menu.code._self
            ("menu.code._self", "en-US", "代码管理_us", "菜单导航"),
            // menu.code._self
            ("menu.code._self", "ja-JP", "代码管理_jp", "菜单导航"),
            // menu.code._self
            ("menu.code._self", "zh-CN", "代码管理", "菜单导航"),
            // menu.code._self
            ("menu.code._self", "zh-HK", "代码管理_hk", "菜单导航"),

            // menu.foundation._self
            ("menu.foundation._self", "en-US", "基础数据_us", "菜单导航"),
            // menu.foundation._self
            ("menu.foundation._self", "ja-JP", "基础数据_jp", "菜单导航"),
            // menu.foundation._self
            ("menu.foundation._self", "zh-CN", "基础数据", "菜单导航"),
            // menu.foundation._self
            ("menu.foundation._self", "zh-HK", "基础数据_hk", "菜单导航"),

            // menu.statistics._self
            ("menu.statistics._self", "en-US", "统计看板_us", "菜单导航"),
            // menu.statistics._self
            ("menu.statistics._self", "ja-JP", "统计看板_jp", "菜单导航"),
            // menu.statistics._self
            ("menu.statistics._self", "zh-CN", "统计看板", "菜单导航"),
            // menu.statistics._self
            ("menu.statistics._self", "zh-HK", "统计看板_hk", "菜单导航"),

            // menu.about
            ("menu.about", "en-US", "关于_us", "菜单导航"),
            // menu.about
            ("menu.about", "ja-JP", "关于_jp", "菜单导航"),
            // menu.about
            ("menu.about", "zh-CN", "关于", "菜单导航"),
            // menu.about
            ("menu.about", "zh-HK", "关于_hk", "菜单导航"),

            // menu.workspace
            ("menu.workspace", "en-US", "工作台_us", "菜单导航"),
            // menu.workspace
            ("menu.workspace", "ja-JP", "工作台_jp", "菜单导航"),
            // menu.workspace
            ("menu.workspace", "zh-CN", "工作台", "菜单导航"),
            // menu.workspace
            ("menu.workspace", "zh-HK", "工作台_hk", "菜单导航"),

            // menu.data.board
            ("menu.data.board", "en-US", "数据看板_us", "菜单导航"),
            // menu.data.board
            ("menu.data.board", "ja-JP", "数据看板_jp", "菜单导航"),
            // menu.data.board
            ("menu.data.board", "zh-CN", "数据看板", "菜单导航"),
            // menu.data.board
            ("menu.data.board", "zh-HK", "数据看板_hk", "菜单导航"),

            // menu.routine.announcement
            ("menu.routine.announcement", "en-US", "公告通知_us", "菜单导航"),
            // menu.routine.announcement
            ("menu.routine.announcement", "ja-JP", "公告通知_jp", "菜单导航"),
            // menu.routine.announcement
            ("menu.routine.announcement", "zh-CN", "公告通知", "菜单导航"),
            // menu.routine.announcement
            ("menu.routine.announcement", "zh-HK", "公告通知_hk", "菜单导航"),

            // menu.routine.meeting.center._self
            ("menu.routine.meeting.center._self", "en-US", "会议中心_us", "菜单导航"),
            // menu.routine.meeting.center._self
            ("menu.routine.meeting.center._self", "ja-JP", "会议中心_jp", "菜单导航"),
            // menu.routine.meeting.center._self
            ("menu.routine.meeting.center._self", "zh-CN", "会议中心", "菜单导航"),
            // menu.routine.meeting.center._self
            ("menu.routine.meeting.center._self", "zh-HK", "会议中心_hk", "菜单导航"),

            // menu.routine.document.center
            ("menu.routine.document.center", "en-US", "文管中心_us", "菜单导航"),
            // menu.routine.document.center
            ("menu.routine.document.center", "ja-JP", "文管中心_jp", "菜单导航"),
            // menu.routine.document.center
            ("menu.routine.document.center", "zh-CN", "文管中心", "菜单导航"),
            // menu.routine.document.center
            ("menu.routine.document.center", "zh-HK", "文管中心_hk", "菜单导航"),

            // menu.routine.news.center
            ("menu.routine.news.center", "en-US", "新闻中心_us", "菜单导航"),
            // menu.routine.news.center
            ("menu.routine.news.center", "ja-JP", "新闻中心_jp", "菜单导航"),
            // menu.routine.news.center
            ("menu.routine.news.center", "zh-CN", "新闻中心", "菜单导航"),
            // menu.routine.news.center
            ("menu.routine.news.center", "zh-HK", "新闻中心_hk", "菜单导航"),

            // menu.routine.help.desk._self
            ("menu.routine.help.desk._self", "en-US", "服务台_us", "菜单导航"),
            // menu.routine.help.desk._self
            ("menu.routine.help.desk._self", "ja-JP", "服务台_jp", "菜单导航"),
            // menu.routine.help.desk._self
            ("menu.routine.help.desk._self", "zh-CN", "服务台", "菜单导航"),
            // menu.routine.help.desk._self
            ("menu.routine.help.desk._self", "zh-HK", "服务台_hk", "菜单导航"),

            // menu.routine.visitor.center
            ("menu.routine.visitor.center", "en-US", "访客中心_us", "菜单导航"),
            // menu.routine.visitor.center
            ("menu.routine.visitor.center", "ja-JP", "访客中心_jp", "菜单导航"),
            // menu.routine.visitor.center
            ("menu.routine.visitor.center", "zh-CN", "访客中心", "菜单导航"),
            // menu.routine.visitor.center
            ("menu.routine.visitor.center", "zh-HK", "访客中心_hk", "菜单导航"),

            // menu.accounting.financial._self
            ("menu.accounting.financial._self", "en-US", "管理会计_us", "菜单导航"),
            // menu.accounting.financial._self
            ("menu.accounting.financial._self", "ja-JP", "管理会计_jp", "菜单导航"),
            // menu.accounting.financial._self
            ("menu.accounting.financial._self", "zh-CN", "管理会计", "菜单导航"),
            // menu.accounting.financial._self
            ("menu.accounting.financial._self", "zh-HK", "管理会计_hk", "菜单导航"),

            // menu.accounting.controlling._self
            ("menu.accounting.controlling._self", "en-US", "控制会计_us", "菜单导航"),
            // menu.accounting.controlling._self
            ("menu.accounting.controlling._self", "ja-JP", "控制会计_jp", "菜单导航"),
            // menu.accounting.controlling._self
            ("menu.accounting.controlling._self", "zh-CN", "控制会计", "菜单导航"),
            // menu.accounting.controlling._self
            ("menu.accounting.controlling._self", "zh-HK", "控制会计_hk", "菜单导航"),

            // menu.logistics.sales._self
            ("menu.logistics.sales._self", "en-US", "销售管理_us", "菜单导航"),
            // menu.logistics.sales._self
            ("menu.logistics.sales._self", "ja-JP", "销售管理_jp", "菜单导航"),
            // menu.logistics.sales._self
            ("menu.logistics.sales._self", "zh-CN", "销售管理", "菜单导航"),
            // menu.logistics.sales._self
            ("menu.logistics.sales._self", "zh-HK", "销售管理_hk", "菜单导航"),

            // menu.logistics.materials._self
            ("menu.logistics.materials._self", "en-US", "物料管理_us", "菜单导航"),
            // menu.logistics.materials._self
            ("menu.logistics.materials._self", "ja-JP", "物料管理_jp", "菜单导航"),
            // menu.logistics.materials._self
            ("menu.logistics.materials._self", "zh-CN", "物料管理", "菜单导航"),
            // menu.logistics.materials._self
            ("menu.logistics.materials._self", "zh-HK", "物料管理_hk", "菜单导航"),

            // menu.logistics.procurement._self
            ("menu.logistics.procurement._self", "en-US", "采购管理_us", "菜单导航"),
            // menu.logistics.procurement._self
            ("menu.logistics.procurement._self", "ja-JP", "采购管理_jp", "菜单导航"),
            // menu.logistics.procurement._self
            ("menu.logistics.procurement._self", "zh-CN", "采购管理", "菜单导航"),
            // menu.logistics.procurement._self
            ("menu.logistics.procurement._self", "zh-HK", "采购管理_hk", "菜单导航"),

            // menu.logistics.manufacturing._self
            ("menu.logistics.manufacturing._self", "en-US", "生产执行_us", "菜单导航"),
            // menu.logistics.manufacturing._self
            ("menu.logistics.manufacturing._self", "ja-JP", "生产执行_jp", "菜单导航"),
            // menu.logistics.manufacturing._self
            ("menu.logistics.manufacturing._self", "zh-CN", "生产执行", "菜单导航"),
            // menu.logistics.manufacturing._self
            ("menu.logistics.manufacturing._self", "zh-HK", "生产执行_hk", "菜单导航"),

            // menu.logistics.quality._self
            ("menu.logistics.quality._self", "en-US", "质量管理_us", "菜单导航"),
            // menu.logistics.quality._self
            ("menu.logistics.quality._self", "ja-JP", "质量管理_jp", "菜单导航"),
            // menu.logistics.quality._self
            ("menu.logistics.quality._self", "zh-CN", "质量管理", "菜单导航"),
            // menu.logistics.quality._self
            ("menu.logistics.quality._self", "zh-HK", "质量管理_hk", "菜单导航"),

            // menu.logistics.customer.service._self
            ("menu.logistics.customer.service._self", "en-US", "客户服务_us", "菜单导航"),
            // menu.logistics.customer.service._self
            ("menu.logistics.customer.service._self", "ja-JP", "客户服务_jp", "菜单导航"),
            // menu.logistics.customer.service._self
            ("menu.logistics.customer.service._self", "zh-CN", "客户服务", "菜单导航"),
            // menu.logistics.customer.service._self
            ("menu.logistics.customer.service._self", "zh-HK", "客户服务_hk", "菜单导航"),

            // menu.logistics.maintenance._self
            ("menu.logistics.maintenance._self", "en-US", "工厂维护_us", "菜单导航"),
            // menu.logistics.maintenance._self
            ("menu.logistics.maintenance._self", "ja-JP", "工厂维护_jp", "菜单导航"),
            // menu.logistics.maintenance._self
            ("menu.logistics.maintenance._self", "zh-CN", "工厂维护", "菜单导航"),
            // menu.logistics.maintenance._self
            ("menu.logistics.maintenance._self", "zh-HK", "工厂维护_hk", "菜单导航"),

            // menu.logistics.serial._self
            ("menu.logistics.serial._self", "en-US", "序列号管理_us", "菜单导航"),
            // menu.logistics.serial._self
            ("menu.logistics.serial._self", "ja-JP", "序列号管理_jp", "菜单导航"),
            // menu.logistics.serial._self
            ("menu.logistics.serial._self", "zh-CN", "序列号管理", "菜单导航"),
            // menu.logistics.serial._self
            ("menu.logistics.serial._self", "zh-HK", "序列号管理_hk", "菜单导航"),

            // menu.human.resource.organization._self
            ("menu.human.resource.organization._self", "en-US", "组织管理_us", "菜单导航"),
            // menu.human.resource.organization._self
            ("menu.human.resource.organization._self", "ja-JP", "组织管理_jp", "菜单导航"),
            // menu.human.resource.organization._self
            ("menu.human.resource.organization._self", "zh-CN", "组织管理", "菜单导航"),
            // menu.human.resource.organization._self
            ("menu.human.resource.organization._self", "zh-HK", "组织管理_hk", "菜单导航"),

            // menu.human.resource.personnel._self
            ("menu.human.resource.personnel._self", "en-US", "人事管理_us", "菜单导航"),
            // menu.human.resource.personnel._self
            ("menu.human.resource.personnel._self", "ja-JP", "人事管理_jp", "菜单导航"),
            // menu.human.resource.personnel._self
            ("menu.human.resource.personnel._self", "zh-CN", "人事管理", "菜单导航"),
            // menu.human.resource.personnel._self
            ("menu.human.resource.personnel._self", "zh-HK", "人事管理_hk", "菜单导航"),

            // menu.human.resource.attendance._self
            ("menu.human.resource.attendance._self", "en-US", "考勤管理_us", "菜单导航"),
            // menu.human.resource.attendance._self
            ("menu.human.resource.attendance._self", "ja-JP", "考勤管理_jp", "菜单导航"),
            // menu.human.resource.attendance._self
            ("menu.human.resource.attendance._self", "zh-CN", "考勤管理", "菜单导航"),
            // menu.human.resource.attendance._self
            ("menu.human.resource.attendance._self", "zh-HK", "考勤管理_hk", "菜单导航"),

            // menu.human.resource.compensation._self
            ("menu.human.resource.compensation._self", "en-US", "薪酬管理_us", "菜单导航"),
            // menu.human.resource.compensation._self
            ("menu.human.resource.compensation._self", "ja-JP", "薪酬管理_jp", "菜单导航"),
            // menu.human.resource.compensation._self
            ("menu.human.resource.compensation._self", "zh-CN", "薪酬管理", "菜单导航"),
            // menu.human.resource.compensation._self
            ("menu.human.resource.compensation._self", "zh-HK", "薪酬管理_hk", "菜单导航"),

            // menu.human.resource.benefits._self
            ("menu.human.resource.benefits._self", "en-US", "福利管理_us", "菜单导航"),
            // menu.human.resource.benefits._self
            ("menu.human.resource.benefits._self", "ja-JP", "福利管理_jp", "菜单导航"),
            // menu.human.resource.benefits._self
            ("menu.human.resource.benefits._self", "zh-CN", "福利管理", "菜单导航"),
            // menu.human.resource.benefits._self
            ("menu.human.resource.benefits._self", "zh-HK", "福利管理_hk", "菜单导航"),

            // menu.human.resource.performance._self
            ("menu.human.resource.performance._self", "en-US", "绩效管理_us", "菜单导航"),
            // menu.human.resource.performance._self
            ("menu.human.resource.performance._self", "ja-JP", "绩效管理_jp", "菜单导航"),
            // menu.human.resource.performance._self
            ("menu.human.resource.performance._self", "zh-CN", "绩效管理", "菜单导航"),
            // menu.human.resource.performance._self
            ("menu.human.resource.performance._self", "zh-HK", "绩效管理_hk", "菜单导航"),

            // menu.human.resource.training._self
            ("menu.human.resource.training._self", "en-US", "教育培训_us", "菜单导航"),
            // menu.human.resource.training._self
            ("menu.human.resource.training._self", "ja-JP", "教育培训_jp", "菜单导航"),
            // menu.human.resource.training._self
            ("menu.human.resource.training._self", "zh-CN", "教育培训", "菜单导航"),
            // menu.human.resource.training._self
            ("menu.human.resource.training._self", "zh-HK", "教育培训_hk", "菜单导航"),

            // menu.human.resource.talent._self
            ("menu.human.resource.talent._self", "en-US", "人才管理_us", "菜单导航"),
            // menu.human.resource.talent._self
            ("menu.human.resource.talent._self", "ja-JP", "人才管理_jp", "菜单导航"),
            // menu.human.resource.talent._self
            ("menu.human.resource.talent._self", "zh-CN", "人才管理", "菜单导航"),
            // menu.human.resource.talent._self
            ("menu.human.resource.talent._self", "zh-HK", "人才管理_hk", "菜单导航"),

            // menu.identity.tenant
            ("menu.identity.tenant", "en-US", "租户管理_us", "菜单导航"),
            // menu.identity.tenant
            ("menu.identity.tenant", "ja-JP", "租户管理_jp", "菜单导航"),
            // menu.identity.tenant
            ("menu.identity.tenant", "zh-CN", "租户管理", "菜单导航"),
            // menu.identity.tenant
            ("menu.identity.tenant", "zh-HK", "租户管理_hk", "菜单导航"),

            // menu.identity.user
            ("menu.identity.user", "en-US", "用户管理_us", "菜单导航"),
            // menu.identity.user
            ("menu.identity.user", "ja-JP", "用户管理_jp", "菜单导航"),
            // menu.identity.user
            ("menu.identity.user", "zh-CN", "用户管理", "菜单导航"),
            // menu.identity.user
            ("menu.identity.user", "zh-HK", "用户管理_hk", "菜单导航"),

            // menu.identity.menu
            ("menu.identity.menu", "en-US", "菜单管理_us", "菜单导航"),
            // menu.identity.menu
            ("menu.identity.menu", "ja-JP", "菜单管理_jp", "菜单导航"),
            // menu.identity.menu
            ("menu.identity.menu", "zh-CN", "菜单管理", "菜单导航"),
            // menu.identity.menu
            ("menu.identity.menu", "zh-HK", "菜单管理_hk", "菜单导航"),

            // menu.identity.role
            ("menu.identity.role", "en-US", "角色管理_us", "菜单导航"),
            // menu.identity.role
            ("menu.identity.role", "ja-JP", "角色管理_jp", "菜单导航"),
            // menu.identity.role
            ("menu.identity.role", "zh-CN", "角色管理", "菜单导航"),
            // menu.identity.role
            ("menu.identity.role", "zh-HK", "角色管理_hk", "菜单导航"),

            // menu.workflow.todo
            ("menu.workflow.todo", "en-US", "待办事项_us", "菜单导航"),
            // menu.workflow.todo
            ("menu.workflow.todo", "ja-JP", "待办事项_jp", "菜单导航"),
            // menu.workflow.todo
            ("menu.workflow.todo", "zh-CN", "待办事项", "菜单导航"),
            // menu.workflow.todo
            ("menu.workflow.todo", "zh-HK", "待办事项_hk", "菜单导航"),

            // menu.workflow.my
            ("menu.workflow.my", "en-US", "我的流程_us", "菜单导航"),
            // menu.workflow.my
            ("menu.workflow.my", "ja-JP", "我的流程_jp", "菜单导航"),
            // menu.workflow.my
            ("menu.workflow.my", "zh-CN", "我的流程", "菜单导航"),
            // menu.workflow.my
            ("menu.workflow.my", "zh-HK", "我的流程_hk", "菜单导航"),

            // menu.workflow.processed
            ("menu.workflow.processed", "en-US", "已处理_us", "菜单导航"),
            // menu.workflow.processed
            ("menu.workflow.processed", "ja-JP", "已处理_jp", "菜单导航"),
            // menu.workflow.processed
            ("menu.workflow.processed", "zh-CN", "已处理", "菜单导航"),
            // menu.workflow.processed
            ("menu.workflow.processed", "zh-HK", "已处理_hk", "菜单导航"),

            // menu.workflow.instance
            ("menu.workflow.instance", "en-US", "流程实例_us", "菜单导航"),
            // menu.workflow.instance
            ("menu.workflow.instance", "ja-JP", "流程实例_jp", "菜单导航"),
            // menu.workflow.instance
            ("menu.workflow.instance", "zh-CN", "流程实例", "菜单导航"),
            // menu.workflow.instance
            ("menu.workflow.instance", "zh-HK", "流程实例_hk", "菜单导航"),

            // menu.workflow.scheme
            ("menu.workflow.scheme", "en-US", "流程方案_us", "菜单导航"),
            // menu.workflow.scheme
            ("menu.workflow.scheme", "ja-JP", "流程方案_jp", "菜单导航"),
            // menu.workflow.scheme
            ("menu.workflow.scheme", "zh-CN", "流程方案", "菜单导航"),
            // menu.workflow.scheme
            ("menu.workflow.scheme", "zh-HK", "流程方案_hk", "菜单导航"),

            // menu.workflow.form
            ("menu.workflow.form", "en-US", "表单管理_us", "菜单导航"),
            // menu.workflow.form
            ("menu.workflow.form", "ja-JP", "表单管理_jp", "菜单导航"),
            // menu.workflow.form
            ("menu.workflow.form", "zh-CN", "表单管理", "菜单导航"),
            // menu.workflow.form
            ("menu.workflow.form", "zh-HK", "表单管理_hk", "菜单导航"),

            // menu.code.generator
            ("menu.code.generator", "en-US", "代码生成_us", "菜单导航"),
            // menu.code.generator
            ("menu.code.generator", "ja-JP", "代码生成_jp", "菜单导航"),
            // menu.code.generator
            ("menu.code.generator", "zh-CN", "代码生成", "菜单导航"),
            // menu.code.generator
            ("menu.code.generator", "zh-HK", "代码生成_hk", "菜单导航"),

            // menu.code.database.info
            ("menu.code.database.info", "en-US", "数据库信息_us", "菜单导航"),
            // menu.code.database.info
            ("menu.code.database.info", "ja-JP", "数据库信息_jp", "菜单导航"),
            // menu.code.database.info
            ("menu.code.database.info", "zh-CN", "数据库信息", "菜单导航"),
            // menu.code.database.info
            ("menu.code.database.info", "zh-HK", "数据库信息_hk", "菜单导航"),

            // menu.code.database.table.clone
            ("menu.code.database.table.clone", "en-US", "表克隆_us", "菜单导航"),
            // menu.code.database.table.clone
            ("menu.code.database.table.clone", "ja-JP", "表克隆_jp", "菜单导航"),
            // menu.code.database.table.clone
            ("menu.code.database.table.clone", "zh-CN", "表克隆", "菜单导航"),
            // menu.code.database.table.clone
            ("menu.code.database.table.clone", "zh-HK", "表克隆_hk", "菜单导航"),

            // menu.code.database.data.clone
            ("menu.code.database.data.clone", "en-US", "数据克隆_us", "菜单导航"),
            // menu.code.database.data.clone
            ("menu.code.database.data.clone", "ja-JP", "数据克隆_jp", "菜单导航"),
            // menu.code.database.data.clone
            ("menu.code.database.data.clone", "zh-CN", "数据克隆", "菜单导航"),
            // menu.code.database.data.clone
            ("menu.code.database.data.clone", "zh-HK", "数据克隆_hk", "菜单导航"),

            // menu.code.database.table.archive
            ("menu.code.database.table.archive", "en-US", "数据表归档_us", "菜单导航"),
            // menu.code.database.table.archive
            ("menu.code.database.table.archive", "ja-JP", "数据表归档_jp", "菜单导航"),
            // menu.code.database.table.archive
            ("menu.code.database.table.archive", "zh-CN", "数据表归档", "菜单导航"),
            // menu.code.database.table.archive
            ("menu.code.database.table.archive", "zh-HK", "数据表归档_hk", "菜单导航"),

            // menu.code.database.backup
            ("menu.code.database.backup", "en-US", "数据库备份_us", "菜单导航"),
            // menu.code.database.backup
            ("menu.code.database.backup", "ja-JP", "数据库备份_jp", "菜单导航"),
            // menu.code.database.backup
            ("menu.code.database.backup", "zh-CN", "数据库备份", "菜单导航"),
            // menu.code.database.backup
            ("menu.code.database.backup", "zh-HK", "数据库备份_hk", "菜单导航"),

            // menu.foundation.numbering
            ("menu.foundation.numbering", "en-US", "编码规则_us", "菜单导航"),
            // menu.foundation.numbering
            ("menu.foundation.numbering", "ja-JP", "编码规则_jp", "菜单导航"),
            // menu.foundation.numbering
            ("menu.foundation.numbering", "zh-CN", "编码规则", "菜单导航"),
            // menu.foundation.numbering
            ("menu.foundation.numbering", "zh-HK", "编码规则_hk", "菜单导航"),

            // menu.foundation.dict
            ("menu.foundation.dict", "en-US", "数据字典_us", "菜单导航"),
            // menu.foundation.dict
            ("menu.foundation.dict", "ja-JP", "数据字典_jp", "菜单导航"),
            // menu.foundation.dict
            ("menu.foundation.dict", "zh-CN", "数据字典", "菜单导航"),
            // menu.foundation.dict
            ("menu.foundation.dict", "zh-HK", "数据字典_hk", "菜单导航"),

            // menu.foundation.i18n
            ("menu.foundation.i18n", "en-US", "国际化_us", "菜单导航"),
            // menu.foundation.i18n
            ("menu.foundation.i18n", "ja-JP", "国际化_jp", "菜单导航"),
            // menu.foundation.i18n
            ("menu.foundation.i18n", "zh-CN", "国际化", "菜单导航"),
            // menu.foundation.i18n
            ("menu.foundation.i18n", "zh-HK", "国际化_hk", "菜单导航"),

            // menu.foundation.file
            ("menu.foundation.file", "en-US", "文件管理_us", "菜单导航"),
            // menu.foundation.file
            ("menu.foundation.file", "ja-JP", "文件管理_jp", "菜单导航"),
            // menu.foundation.file
            ("menu.foundation.file", "zh-CN", "文件管理", "菜单导航"),
            // menu.foundation.file
            ("menu.foundation.file", "zh-HK", "文件管理_hk", "菜单导航"),

            // menu.foundation.cache
            ("menu.foundation.cache", "en-US", "缓存管理_us", "菜单导航"),
            // menu.foundation.cache
            ("menu.foundation.cache", "ja-JP", "缓存管理_jp", "菜单导航"),
            // menu.foundation.cache
            ("menu.foundation.cache", "zh-CN", "缓存管理", "菜单导航"),
            // menu.foundation.cache
            ("menu.foundation.cache", "zh-HK", "缓存管理_hk", "菜单导航"),

            // menu.foundation.vocabulary
            ("menu.foundation.vocabulary", "en-US", "敏感词库_us", "菜单导航"),
            // menu.foundation.vocabulary
            ("menu.foundation.vocabulary", "ja-JP", "敏感词库_jp", "菜单导航"),
            // menu.foundation.vocabulary
            ("menu.foundation.vocabulary", "zh-CN", "敏感词库", "菜单导航"),
            // menu.foundation.vocabulary
            ("menu.foundation.vocabulary", "zh-HK", "敏感词库_hk", "菜单导航"),

            // menu.foundation.system.property
            ("menu.foundation.system.property", "en-US", "系统属性_us", "菜单导航"),
            // menu.foundation.system.property
            ("menu.foundation.system.property", "ja-JP", "系统属性_jp", "菜单导航"),
            // menu.foundation.system.property
            ("menu.foundation.system.property", "zh-CN", "系统属性", "菜单导航"),
            // menu.foundation.system.property
            ("menu.foundation.system.property", "zh-HK", "系统属性_hk", "菜单导航"),

            // menu.foundation.online
            ("menu.foundation.online", "en-US", "在线用户_us", "菜单导航"),
            // menu.foundation.online
            ("menu.foundation.online", "ja-JP", "在线用户_jp", "菜单导航"),
            // menu.foundation.online
            ("menu.foundation.online", "zh-CN", "在线用户", "菜单导航"),
            // menu.foundation.online
            ("menu.foundation.online", "zh-HK", "在线用户_hk", "菜单导航"),

            // menu.foundation.message
            ("menu.foundation.message", "en-US", "在线消息_us", "菜单导航"),
            // menu.foundation.message
            ("menu.foundation.message", "ja-JP", "在线消息_jp", "菜单导航"),
            // menu.foundation.message
            ("menu.foundation.message", "zh-CN", "在线消息", "菜单导航"),
            // menu.foundation.message
            ("menu.foundation.message", "zh-HK", "在线消息_hk", "菜单导航"),

            // menu.foundation.quartz.task
            ("menu.foundation.quartz.task", "en-US", "定时任务_us", "菜单导航"),
            // menu.foundation.quartz.task
            ("menu.foundation.quartz.task", "ja-JP", "定时任务_jp", "菜单导航"),
            // menu.foundation.quartz.task
            ("menu.foundation.quartz.task", "zh-CN", "定时任务", "菜单导航"),
            // menu.foundation.quartz.task
            ("menu.foundation.quartz.task", "zh-HK", "定时任务_hk", "菜单导航"),

            // menu.foundation.admin.division
            ("menu.foundation.admin.division", "en-US", "行政区划_us", "菜单导航"),
            // menu.foundation.admin.division
            ("menu.foundation.admin.division", "ja-JP", "行政区划_jp", "菜单导航"),
            // menu.foundation.admin.division
            ("menu.foundation.admin.division", "zh-CN", "行政区划", "菜单导航"),
            // menu.foundation.admin.division
            ("menu.foundation.admin.division", "zh-HK", "行政区划_hk", "菜单导航"),

            // menu.foundation.ip.geolocation
            ("menu.foundation.ip.geolocation", "en-US", "IP归属_us", "菜单导航"),
            // menu.foundation.ip.geolocation
            ("menu.foundation.ip.geolocation", "ja-JP", "IP归属_jp", "菜单导航"),
            // menu.foundation.ip.geolocation
            ("menu.foundation.ip.geolocation", "zh-CN", "IP归属", "菜单导航"),
            // menu.foundation.ip.geolocation
            ("menu.foundation.ip.geolocation", "zh-HK", "IP归属_hk", "菜单导航"),

            // menu.statistics.quickquery._self
            ("menu.statistics.quickquery._self", "en-US", "快速查询_us", "菜单导航"),
            // menu.statistics.quickquery._self
            ("menu.statistics.quickquery._self", "ja-JP", "快速查询_jp", "菜单导航"),
            // menu.statistics.quickquery._self
            ("menu.statistics.quickquery._self", "zh-CN", "快速查询", "菜单导航"),
            // menu.statistics.quickquery._self
            ("menu.statistics.quickquery._self", "zh-HK", "快速查询_hk", "菜单导航"),

            // menu.statistics.logging._self
            ("menu.statistics.logging._self", "en-US", "日志管理_us", "菜单导航"),
            // menu.statistics.logging._self
            ("menu.statistics.logging._self", "ja-JP", "日志管理_jp", "菜单导航"),
            // menu.statistics.logging._self
            ("menu.statistics.logging._self", "zh-CN", "日志管理", "菜单导航"),
            // menu.statistics.logging._self
            ("menu.statistics.logging._self", "zh-HK", "日志管理_hk", "菜单导航"),

            // menu.accounting.financial.account.title
            ("menu.accounting.financial.account.title", "en-US", "会计科目_us", "菜单导航"),
            // menu.accounting.financial.account.title
            ("menu.accounting.financial.account.title", "ja-JP", "会计科目_jp", "菜单导航"),
            // menu.accounting.financial.account.title
            ("menu.accounting.financial.account.title", "zh-CN", "会计科目", "菜单导航"),
            // menu.accounting.financial.account.title
            ("menu.accounting.financial.account.title", "zh-HK", "会计科目_hk", "菜单导航"),

            // menu.accounting.financial.asset
            ("menu.accounting.financial.asset", "en-US", "固定资产_us", "菜单导航"),
            // menu.accounting.financial.asset
            ("menu.accounting.financial.asset", "ja-JP", "固定资产_jp", "菜单导航"),
            // menu.accounting.financial.asset
            ("menu.accounting.financial.asset", "zh-CN", "固定资产", "菜单导航"),
            // menu.accounting.financial.asset
            ("menu.accounting.financial.asset", "zh-HK", "固定资产_hk", "菜单导航"),

            // menu.accounting.financial.countersign
            ("menu.accounting.financial.countersign", "en-US", "会签管理_us", "菜单导航"),
            // menu.accounting.financial.countersign
            ("menu.accounting.financial.countersign", "ja-JP", "会签管理_jp", "菜单导航"),
            // menu.accounting.financial.countersign
            ("menu.accounting.financial.countersign", "zh-CN", "会签管理", "菜单导航"),
            // menu.accounting.financial.countersign
            ("menu.accounting.financial.countersign", "zh-HK", "会签管理_hk", "菜单导航"),

            // menu.accounting.financial.expense
            ("menu.accounting.financial.expense", "en-US", "费用管理_us", "菜单导航"),
            // menu.accounting.financial.expense
            ("menu.accounting.financial.expense", "ja-JP", "费用管理_jp", "菜单导航"),
            // menu.accounting.financial.expense
            ("menu.accounting.financial.expense", "zh-CN", "费用管理", "菜单导航"),
            // menu.accounting.financial.expense
            ("menu.accounting.financial.expense", "zh-HK", "费用管理_hk", "菜单导航"),

            // menu.accounting.financial.exchange.rate
            ("menu.accounting.financial.exchange.rate", "en-US", "汇率维护_us", "菜单导航"),
            // menu.accounting.financial.exchange.rate
            ("menu.accounting.financial.exchange.rate", "ja-JP", "汇率维护_jp", "菜单导航"),
            // menu.accounting.financial.exchange.rate
            ("menu.accounting.financial.exchange.rate", "zh-CN", "汇率维护", "菜单导航"),
            // menu.accounting.financial.exchange.rate
            ("menu.accounting.financial.exchange.rate", "zh-HK", "汇率维护_hk", "菜单导航"),

            // menu.accounting.financial.balance.sheet
            ("menu.accounting.financial.balance.sheet", "en-US", "资产负债表_us", "菜单导航"),
            // menu.accounting.financial.balance.sheet
            ("menu.accounting.financial.balance.sheet", "ja-JP", "资产负债表_jp", "菜单导航"),
            // menu.accounting.financial.balance.sheet
            ("menu.accounting.financial.balance.sheet", "zh-CN", "资产负债表", "菜单导航"),
            // menu.accounting.financial.balance.sheet
            ("menu.accounting.financial.balance.sheet", "zh-HK", "资产负债表_hk", "菜单导航"),

            // menu.accounting.financial.profit.loss
            ("menu.accounting.financial.profit.loss", "en-US", "利润表_us", "菜单导航"),
            // menu.accounting.financial.profit.loss
            ("menu.accounting.financial.profit.loss", "ja-JP", "利润表_jp", "菜单导航"),
            // menu.accounting.financial.profit.loss
            ("menu.accounting.financial.profit.loss", "zh-CN", "利润表", "菜单导航"),
            // menu.accounting.financial.profit.loss
            ("menu.accounting.financial.profit.loss", "zh-HK", "利润表_hk", "菜单导航"),

            // menu.accounting.financial.purchase.sales.inventory
            ("menu.accounting.financial.purchase.sales.inventory", "en-US", "进销存表_us", "菜单导航"),
            // menu.accounting.financial.purchase.sales.inventory
            ("menu.accounting.financial.purchase.sales.inventory", "ja-JP", "进销存表_jp", "菜单导航"),
            // menu.accounting.financial.purchase.sales.inventory
            ("menu.accounting.financial.purchase.sales.inventory", "zh-CN", "进销存表", "菜单导航"),
            // menu.accounting.financial.purchase.sales.inventory
            ("menu.accounting.financial.purchase.sales.inventory", "zh-HK", "进销存表_hk", "菜单导航"),

            // menu.accounting.financial.budget.actual
            ("menu.accounting.financial.budget.actual", "en-US", "预算实绩_us", "菜单导航"),
            // menu.accounting.financial.budget.actual
            ("menu.accounting.financial.budget.actual", "ja-JP", "预算实绩_jp", "菜单导航"),
            // menu.accounting.financial.budget.actual
            ("menu.accounting.financial.budget.actual", "zh-CN", "预算实绩", "菜单导航"),
            // menu.accounting.financial.budget.actual
            ("menu.accounting.financial.budget.actual", "zh-HK", "预算实绩_hk", "菜单导航"),

            // menu.accounting.financial.company
            ("menu.accounting.financial.company", "en-US", "公司信息_us", "菜单导航"),
            // menu.accounting.financial.company
            ("menu.accounting.financial.company", "ja-JP", "公司信息_jp", "菜单导航"),
            // menu.accounting.financial.company
            ("menu.accounting.financial.company", "zh-CN", "公司信息", "菜单导航"),
            // menu.accounting.financial.company
            ("menu.accounting.financial.company", "zh-HK", "公司信息_hk", "菜单导航"),

            // menu.accounting.financial.period
            ("menu.accounting.financial.period", "en-US", "财务期间_us", "菜单导航"),
            // menu.accounting.financial.period
            ("menu.accounting.financial.period", "ja-JP", "财务期间_jp", "菜单导航"),
            // menu.accounting.financial.period
            ("menu.accounting.financial.period", "zh-CN", "财务期间", "菜单导航"),
            // menu.accounting.financial.period
            ("menu.accounting.financial.period", "zh-HK", "财务期间_hk", "菜单导航"),

            // menu.accounting.financial.bank
            ("menu.accounting.financial.bank", "en-US", "银行信息_us", "菜单导航"),
            // menu.accounting.financial.bank
            ("menu.accounting.financial.bank", "ja-JP", "银行信息_jp", "菜单导航"),
            // menu.accounting.financial.bank
            ("menu.accounting.financial.bank", "zh-CN", "银行信息", "菜单导航"),
            // menu.accounting.financial.bank
            ("menu.accounting.financial.bank", "zh-HK", "银行信息_hk", "菜单导航"),

            // menu.accounting.financial.tax.code
            ("menu.accounting.financial.tax.code", "en-US", "税收代码_us", "菜单导航"),
            // menu.accounting.financial.tax.code
            ("menu.accounting.financial.tax.code", "ja-JP", "税收代码_jp", "菜单导航"),
            // menu.accounting.financial.tax.code
            ("menu.accounting.financial.tax.code", "zh-CN", "税收代码", "菜单导航"),
            // menu.accounting.financial.tax.code
            ("menu.accounting.financial.tax.code", "zh-HK", "税收代码_hk", "菜单导航"),

            // menu.accounting.financial.tax.condition
            ("menu.accounting.financial.tax.condition", "en-US", "税务条件_us", "菜单导航"),
            // menu.accounting.financial.tax.condition
            ("menu.accounting.financial.tax.condition", "ja-JP", "税务条件_jp", "菜单导航"),
            // menu.accounting.financial.tax.condition
            ("menu.accounting.financial.tax.condition", "zh-CN", "税务条件", "菜单导航"),
            // menu.accounting.financial.tax.condition
            ("menu.accounting.financial.tax.condition", "zh-HK", "税务条件_hk", "菜单导航"),

            // menu.accounting.controlling.profit.center
            ("menu.accounting.controlling.profit.center", "en-US", "利润中心_us", "菜单导航"),
            // menu.accounting.controlling.profit.center
            ("menu.accounting.controlling.profit.center", "ja-JP", "利润中心_jp", "菜单导航"),
            // menu.accounting.controlling.profit.center
            ("menu.accounting.controlling.profit.center", "zh-CN", "利润中心", "菜单导航"),
            // menu.accounting.controlling.profit.center
            ("menu.accounting.controlling.profit.center", "zh-HK", "利润中心_hk", "菜单导航"),

            // menu.accounting.controlling.cost.center
            ("menu.accounting.controlling.cost.center", "en-US", "成本中心_us", "菜单导航"),
            // menu.accounting.controlling.cost.center
            ("menu.accounting.controlling.cost.center", "ja-JP", "成本中心_jp", "菜单导航"),
            // menu.accounting.controlling.cost.center
            ("menu.accounting.controlling.cost.center", "zh-CN", "成本中心", "菜单导航"),
            // menu.accounting.controlling.cost.center
            ("menu.accounting.controlling.cost.center", "zh-HK", "成本中心_hk", "菜单导航"),

            // menu.accounting.controlling.cost.element
            ("menu.accounting.controlling.cost.element", "en-US", "成本要素_us", "菜单导航"),
            // menu.accounting.controlling.cost.element
            ("menu.accounting.controlling.cost.element", "ja-JP", "成本要素_jp", "菜单导航"),
            // menu.accounting.controlling.cost.element
            ("menu.accounting.controlling.cost.element", "zh-CN", "成本要素", "菜单导航"),
            // menu.accounting.controlling.cost.element
            ("menu.accounting.controlling.cost.element", "zh-HK", "成本要素_hk", "菜单导航"),

            // menu.accounting.controlling.standard.wage.rate
            ("menu.accounting.controlling.standard.wage.rate", "en-US", "标准工资率_us", "菜单导航"),
            // menu.accounting.controlling.standard.wage.rate
            ("menu.accounting.controlling.standard.wage.rate", "ja-JP", "标准工资率_jp", "菜单导航"),
            // menu.accounting.controlling.standard.wage.rate
            ("menu.accounting.controlling.standard.wage.rate", "zh-CN", "标准工资率", "菜单导航"),
            // menu.accounting.controlling.standard.wage.rate
            ("menu.accounting.controlling.standard.wage.rate", "zh-HK", "标准工资率_hk", "菜单导航"),

            // menu.logistics.materials.plant
            ("menu.logistics.materials.plant", "en-US", "工厂信息_us", "菜单导航"),
            // menu.logistics.materials.plant
            ("menu.logistics.materials.plant", "ja-JP", "工厂信息_jp", "菜单导航"),
            // menu.logistics.materials.plant
            ("menu.logistics.materials.plant", "zh-CN", "工厂信息", "菜单导航"),
            // menu.logistics.materials.plant
            ("menu.logistics.materials.plant", "zh-HK", "工厂信息_hk", "菜单导航"),

            // menu.logistics.materials.general.material
            ("menu.logistics.materials.general.material", "en-US", "全局物料_us", "菜单导航"),
            // menu.logistics.materials.general.material
            ("menu.logistics.materials.general.material", "ja-JP", "全局物料_jp", "菜单导航"),
            // menu.logistics.materials.general.material
            ("menu.logistics.materials.general.material", "zh-CN", "全局物料", "菜单导航"),
            // menu.logistics.materials.general.material
            ("menu.logistics.materials.general.material", "zh-HK", "全局物料_hk", "菜单导航"),

            // menu.logistics.materials.material.description
            ("menu.logistics.materials.material.description", "en-US", "物料描述_us", "菜单导航"),
            // menu.logistics.materials.material.description
            ("menu.logistics.materials.material.description", "ja-JP", "物料描述_jp", "菜单导航"),
            // menu.logistics.materials.material.description
            ("menu.logistics.materials.material.description", "zh-CN", "物料描述", "菜单导航"),
            // menu.logistics.materials.material.description
            ("menu.logistics.materials.material.description", "zh-HK", "物料描述_hk", "菜单导航"),

            // menu.logistics.materials.material.plant
            ("menu.logistics.materials.material.plant", "en-US", "工厂物料_us", "菜单导航"),
            // menu.logistics.materials.material.plant
            ("menu.logistics.materials.material.plant", "ja-JP", "工厂物料_jp", "菜单导航"),
            // menu.logistics.materials.material.plant
            ("menu.logistics.materials.material.plant", "zh-CN", "工厂物料", "菜单导航"),
            // menu.logistics.materials.material.plant
            ("menu.logistics.materials.material.plant", "zh-HK", "工厂物料_hk", "菜单导航"),

            // menu.logistics.materials.warehouse
            ("menu.logistics.materials.warehouse", "en-US", "仓库信息_us", "菜单导航"),
            // menu.logistics.materials.warehouse
            ("menu.logistics.materials.warehouse", "ja-JP", "仓库信息_jp", "菜单导航"),
            // menu.logistics.materials.warehouse
            ("menu.logistics.materials.warehouse", "zh-CN", "仓库信息", "菜单导航"),
            // menu.logistics.materials.warehouse
            ("menu.logistics.materials.warehouse", "zh-HK", "仓库信息_hk", "菜单导航"),

            // menu.logistics.materials.packaging.material
            ("menu.logistics.materials.packaging.material", "en-US", "包装物料_us", "菜单导航"),
            // menu.logistics.materials.packaging.material
            ("menu.logistics.materials.packaging.material", "ja-JP", "包装物料_jp", "菜单导航"),
            // menu.logistics.materials.packaging.material
            ("menu.logistics.materials.packaging.material", "zh-CN", "包装物料", "菜单导航"),
            // menu.logistics.materials.packaging.material
            ("menu.logistics.materials.packaging.material", "zh-HK", "包装物料_hk", "菜单导航"),

            // menu.logistics.materials.model.destination
            ("menu.logistics.materials.model.destination", "en-US", "机种仕向_us", "菜单导航"),
            // menu.logistics.materials.model.destination
            ("menu.logistics.materials.model.destination", "ja-JP", "机种仕向_jp", "菜单导航"),
            // menu.logistics.materials.model.destination
            ("menu.logistics.materials.model.destination", "zh-CN", "机种仕向", "菜单导航"),
            // menu.logistics.materials.model.destination
            ("menu.logistics.materials.model.destination", "zh-HK", "机种仕向_hk", "菜单导航"),

            // menu.logistics.materials.material.document
            ("menu.logistics.materials.material.document", "en-US", "物料凭证_us", "菜单导航"),
            // menu.logistics.materials.material.document
            ("menu.logistics.materials.material.document", "ja-JP", "物料凭证_jp", "菜单导航"),
            // menu.logistics.materials.material.document
            ("menu.logistics.materials.material.document", "zh-CN", "物料凭证", "菜单导航"),
            // menu.logistics.materials.material.document
            ("menu.logistics.materials.material.document", "zh-HK", "物料凭证_hk", "菜单导航"),

            // menu.logistics.materials.material.inbound.trend
            ("menu.logistics.materials.material.inbound.trend", "en-US", "物料入库推移_us", "菜单导航"),
            // menu.logistics.materials.material.inbound.trend
            ("menu.logistics.materials.material.inbound.trend", "ja-JP", "物料入库推移_jp", "菜单导航"),
            // menu.logistics.materials.material.inbound.trend
            ("menu.logistics.materials.material.inbound.trend", "zh-CN", "物料入库推移", "菜单导航"),
            // menu.logistics.materials.material.inbound.trend
            ("menu.logistics.materials.material.inbound.trend", "zh-HK", "物料入库推移_hk", "菜单导航"),

            // menu.logistics.materials.material.outbound.trend
            ("menu.logistics.materials.material.outbound.trend", "en-US", "物料出库推移_us", "菜单导航"),
            // menu.logistics.materials.material.outbound.trend
            ("menu.logistics.materials.material.outbound.trend", "ja-JP", "物料出库推移_jp", "菜单导航"),
            // menu.logistics.materials.material.outbound.trend
            ("menu.logistics.materials.material.outbound.trend", "zh-CN", "物料出库推移", "菜单导航"),
            // menu.logistics.materials.material.outbound.trend
            ("menu.logistics.materials.material.outbound.trend", "zh-HK", "物料出库推移_hk", "菜单导航"),

            // menu.logistics.materials.material.moving.price
            ("menu.logistics.materials.material.moving.price", "en-US", "移动价格_us", "菜单导航"),
            // menu.logistics.materials.material.moving.price
            ("menu.logistics.materials.material.moving.price", "ja-JP", "移动价格_jp", "菜单导航"),
            // menu.logistics.materials.material.moving.price
            ("menu.logistics.materials.material.moving.price", "zh-CN", "移动价格", "菜单导航"),
            // menu.logistics.materials.material.moving.price
            ("menu.logistics.materials.material.moving.price", "zh-HK", "移动价格_hk", "菜单导航"),

            // menu.logistics.materials.material.moving.trend
            ("menu.logistics.materials.material.moving.trend", "en-US", "移动价格推移_us", "菜单导航"),
            // menu.logistics.materials.material.moving.trend
            ("menu.logistics.materials.material.moving.trend", "ja-JP", "移动价格推移_jp", "菜单导航"),
            // menu.logistics.materials.material.moving.trend
            ("menu.logistics.materials.material.moving.trend", "zh-CN", "移动价格推移", "菜单导航"),
            // menu.logistics.materials.material.moving.trend
            ("menu.logistics.materials.material.moving.trend", "zh-HK", "移动价格推移_hk", "菜单导航"),

            // menu.logistics.materials.inventory.reserve
            ("menu.logistics.materials.inventory.reserve", "en-US", "存货跌价准备_us", "菜单导航"),
            // menu.logistics.materials.inventory.reserve
            ("menu.logistics.materials.inventory.reserve", "ja-JP", "存货跌价准备_jp", "菜单导航"),
            // menu.logistics.materials.inventory.reserve
            ("menu.logistics.materials.inventory.reserve", "zh-CN", "存货跌价准备", "菜单导航"),
            // menu.logistics.materials.inventory.reserve
            ("menu.logistics.materials.inventory.reserve", "zh-HK", "存货跌价准备_hk", "菜单导航"),

            // menu.logistics.materials.material.group
            ("menu.logistics.materials.material.group", "en-US", "物料组_us", "菜单导航"),
            // menu.logistics.materials.material.group
            ("menu.logistics.materials.material.group", "ja-JP", "物料组_jp", "菜单导航"),
            // menu.logistics.materials.material.group
            ("menu.logistics.materials.material.group", "zh-CN", "物料组", "菜单导航"),
            // menu.logistics.materials.material.group
            ("menu.logistics.materials.material.group", "zh-HK", "物料组_hk", "菜单导航"),

            // menu.logistics.procurement.purchase.forecast
            ("menu.logistics.procurement.purchase.forecast", "en-US", "采购预测_us", "菜单导航"),
            // menu.logistics.procurement.purchase.forecast
            ("menu.logistics.procurement.purchase.forecast", "ja-JP", "采购预测_jp", "菜单导航"),
            // menu.logistics.procurement.purchase.forecast
            ("menu.logistics.procurement.purchase.forecast", "zh-CN", "采购预测", "菜单导航"),
            // menu.logistics.procurement.purchase.forecast
            ("menu.logistics.procurement.purchase.forecast", "zh-HK", "采购预测_hk", "菜单导航"),

            // menu.logistics.procurement.purchase.inquiry
            ("menu.logistics.procurement.purchase.inquiry", "en-US", "采购询价_us", "菜单导航"),
            // menu.logistics.procurement.purchase.inquiry
            ("menu.logistics.procurement.purchase.inquiry", "ja-JP", "采购询价_jp", "菜单导航"),
            // menu.logistics.procurement.purchase.inquiry
            ("menu.logistics.procurement.purchase.inquiry", "zh-CN", "采购询价", "菜单导航"),
            // menu.logistics.procurement.purchase.inquiry
            ("menu.logistics.procurement.purchase.inquiry", "zh-HK", "采购询价_hk", "菜单导航"),

            // menu.logistics.procurement.purchase.request
            ("menu.logistics.procurement.purchase.request", "en-US", "采购申请_us", "菜单导航"),
            // menu.logistics.procurement.purchase.request
            ("menu.logistics.procurement.purchase.request", "ja-JP", "采购申请_jp", "菜单导航"),
            // menu.logistics.procurement.purchase.request
            ("menu.logistics.procurement.purchase.request", "zh-CN", "采购申请", "菜单导航"),
            // menu.logistics.procurement.purchase.request
            ("menu.logistics.procurement.purchase.request", "zh-HK", "采购申请_hk", "菜单导航"),

            // menu.logistics.procurement.purchase.order
            ("menu.logistics.procurement.purchase.order", "en-US", "采购订单_us", "菜单导航"),
            // menu.logistics.procurement.purchase.order
            ("menu.logistics.procurement.purchase.order", "ja-JP", "采购订单_jp", "菜单导航"),
            // menu.logistics.procurement.purchase.order
            ("menu.logistics.procurement.purchase.order", "zh-CN", "采购订单", "菜单导航"),
            // menu.logistics.procurement.purchase.order
            ("menu.logistics.procurement.purchase.order", "zh-HK", "采购订单_hk", "菜单导航"),

            // menu.logistics.procurement.purchase.info
            ("menu.logistics.procurement.purchase.info", "en-US", "采购信息_us", "菜单导航"),
            // menu.logistics.procurement.purchase.info
            ("menu.logistics.procurement.purchase.info", "ja-JP", "采购信息_jp", "菜单导航"),
            // menu.logistics.procurement.purchase.info
            ("menu.logistics.procurement.purchase.info", "zh-CN", "采购信息", "菜单导航"),
            // menu.logistics.procurement.purchase.info
            ("menu.logistics.procurement.purchase.info", "zh-HK", "采购信息_hk", "菜单导航"),

            // menu.logistics.procurement.purchase.price.trend
            ("menu.logistics.procurement.purchase.price.trend", "en-US", "采购价格推移_us", "菜单导航"),
            // menu.logistics.procurement.purchase.price.trend
            ("menu.logistics.procurement.purchase.price.trend", "ja-JP", "采购价格推移_jp", "菜单导航"),
            // menu.logistics.procurement.purchase.price.trend
            ("menu.logistics.procurement.purchase.price.trend", "zh-CN", "采购价格推移", "菜单导航"),
            // menu.logistics.procurement.purchase.price.trend
            ("menu.logistics.procurement.purchase.price.trend", "zh-HK", "采购价格推移_hk", "菜单导航"),

            // menu.logistics.procurement.po.price.pir.deviation
            ("menu.logistics.procurement.po.price.pir.deviation", "en-US", "PO价偏离PIR_us", "菜单导航"),
            // menu.logistics.procurement.po.price.pir.deviation
            ("menu.logistics.procurement.po.price.pir.deviation", "ja-JP", "PO价偏离PIR_jp", "菜单导航"),
            // menu.logistics.procurement.po.price.pir.deviation
            ("menu.logistics.procurement.po.price.pir.deviation", "zh-CN", "PO价偏离PIR", "菜单导航"),
            // menu.logistics.procurement.po.price.pir.deviation
            ("menu.logistics.procurement.po.price.pir.deviation", "zh-HK", "PO价偏离PIR_hk", "菜单导航"),

            // menu.logistics.procurement.purchase.invoice
            ("menu.logistics.procurement.purchase.invoice", "en-US", "采购发票_us", "菜单导航"),
            // menu.logistics.procurement.purchase.invoice
            ("menu.logistics.procurement.purchase.invoice", "ja-JP", "采购发票_jp", "菜单导航"),
            // menu.logistics.procurement.purchase.invoice
            ("menu.logistics.procurement.purchase.invoice", "zh-CN", "采购发票", "菜单导航"),
            // menu.logistics.procurement.purchase.invoice
            ("menu.logistics.procurement.purchase.invoice", "zh-HK", "采购发票_hk", "菜单导航"),

            // menu.logistics.procurement.purchase.actual.trend
            ("menu.logistics.procurement.purchase.actual.trend", "en-US", "采购实绩推移_us", "菜单导航"),
            // menu.logistics.procurement.purchase.actual.trend
            ("menu.logistics.procurement.purchase.actual.trend", "ja-JP", "采购实绩推移_jp", "菜单导航"),
            // menu.logistics.procurement.purchase.actual.trend
            ("menu.logistics.procurement.purchase.actual.trend", "zh-CN", "采购实绩推移", "菜单导航"),
            // menu.logistics.procurement.purchase.actual.trend
            ("menu.logistics.procurement.purchase.actual.trend", "zh-HK", "采购实绩推移_hk", "菜单导航"),

            // menu.logistics.procurement.supplier
            ("menu.logistics.procurement.supplier", "en-US", "供应商_us", "菜单导航"),
            // menu.logistics.procurement.supplier
            ("menu.logistics.procurement.supplier", "ja-JP", "供应商_jp", "菜单导航"),
            // menu.logistics.procurement.supplier
            ("menu.logistics.procurement.supplier", "zh-CN", "供应商", "菜单导航"),
            // menu.logistics.procurement.supplier
            ("menu.logistics.procurement.supplier", "zh-HK", "供应商_hk", "菜单导航"),

            // menu.logistics.procurement.vendor
            ("menu.logistics.procurement.vendor", "en-US", "经销商_us", "菜单导航"),
            // menu.logistics.procurement.vendor
            ("menu.logistics.procurement.vendor", "ja-JP", "经销商_jp", "菜单导航"),
            // menu.logistics.procurement.vendor
            ("menu.logistics.procurement.vendor", "zh-CN", "经销商", "菜单导航"),
            // menu.logistics.procurement.vendor
            ("menu.logistics.procurement.vendor", "zh-HK", "经销商_hk", "菜单导航"),

            // menu.logistics.procurement.manufacturer.material
            ("menu.logistics.procurement.manufacturer.material", "en-US", "制造商物料_us", "菜单导航"),
            // menu.logistics.procurement.manufacturer.material
            ("menu.logistics.procurement.manufacturer.material", "ja-JP", "制造商物料_jp", "菜单导航"),
            // menu.logistics.procurement.manufacturer.material
            ("menu.logistics.procurement.manufacturer.material", "zh-CN", "制造商物料", "菜单导航"),
            // menu.logistics.procurement.manufacturer.material
            ("menu.logistics.procurement.manufacturer.material", "zh-HK", "制造商物料_hk", "菜单导航"),

            // menu.logistics.procurement.source.of.supply
            ("menu.logistics.procurement.source.of.supply", "en-US", "货源清单_us", "菜单导航"),
            // menu.logistics.procurement.source.of.supply
            ("menu.logistics.procurement.source.of.supply", "ja-JP", "货源清单_jp", "菜单导航"),
            // menu.logistics.procurement.source.of.supply
            ("menu.logistics.procurement.source.of.supply", "zh-CN", "货源清单", "菜单导航"),
            // menu.logistics.procurement.source.of.supply
            ("menu.logistics.procurement.source.of.supply", "zh-HK", "货源清单_hk", "菜单导航"),

            // menu.logistics.procurement.purchase.group
            ("menu.logistics.procurement.purchase.group", "en-US", "采购组_us", "菜单导航"),
            // menu.logistics.procurement.purchase.group
            ("menu.logistics.procurement.purchase.group", "ja-JP", "采购组_jp", "菜单导航"),
            // menu.logistics.procurement.purchase.group
            ("menu.logistics.procurement.purchase.group", "zh-CN", "采购组", "菜单导航"),
            // menu.logistics.procurement.purchase.group
            ("menu.logistics.procurement.purchase.group", "zh-HK", "采购组_hk", "菜单导航"),

            // menu.logistics.manufacturing.bom._self
            ("menu.logistics.manufacturing.bom._self", "en-US", "BOM管理_us", "菜单导航"),
            // menu.logistics.manufacturing.bom._self
            ("menu.logistics.manufacturing.bom._self", "ja-JP", "BOM管理_jp", "菜单导航"),
            // menu.logistics.manufacturing.bom._self
            ("menu.logistics.manufacturing.bom._self", "zh-CN", "BOM管理", "菜单导航"),
            // menu.logistics.manufacturing.bom._self
            ("menu.logistics.manufacturing.bom._self", "zh-HK", "BOM管理_hk", "菜单导航"),

            // menu.logistics.manufacturing.engineering.change._self
            ("menu.logistics.manufacturing.engineering.change._self", "en-US", "工程设变_us", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change._self
            ("menu.logistics.manufacturing.engineering.change._self", "ja-JP", "工程设变_jp", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change._self
            ("menu.logistics.manufacturing.engineering.change._self", "zh-CN", "工程设变", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change._self
            ("menu.logistics.manufacturing.engineering.change._self", "zh-HK", "工程设变_hk", "菜单导航"),

            // menu.logistics.manufacturing.mds._self
            ("menu.logistics.manufacturing.mds._self", "en-US", "MDS计划_us", "菜单导航"),
            // menu.logistics.manufacturing.mds._self
            ("menu.logistics.manufacturing.mds._self", "ja-JP", "MDS计划_jp", "菜单导航"),
            // menu.logistics.manufacturing.mds._self
            ("menu.logistics.manufacturing.mds._self", "zh-CN", "MDS计划", "菜单导航"),
            // menu.logistics.manufacturing.mds._self
            ("menu.logistics.manufacturing.mds._self", "zh-HK", "MDS计划_hk", "菜单导航"),

            // menu.logistics.manufacturing.mps._self
            ("menu.logistics.manufacturing.mps._self", "en-US", "MPS计划_us", "菜单导航"),
            // menu.logistics.manufacturing.mps._self
            ("menu.logistics.manufacturing.mps._self", "ja-JP", "MPS计划_jp", "菜单导航"),
            // menu.logistics.manufacturing.mps._self
            ("menu.logistics.manufacturing.mps._self", "zh-CN", "MPS计划", "菜单导航"),
            // menu.logistics.manufacturing.mps._self
            ("menu.logistics.manufacturing.mps._self", "zh-HK", "MPS计划_hk", "菜单导航"),

            // menu.logistics.manufacturing.mrp._self
            ("menu.logistics.manufacturing.mrp._self", "en-US", "MRP计划_us", "菜单导航"),
            // menu.logistics.manufacturing.mrp._self
            ("menu.logistics.manufacturing.mrp._self", "ja-JP", "MRP计划_jp", "菜单导航"),
            // menu.logistics.manufacturing.mrp._self
            ("menu.logistics.manufacturing.mrp._self", "zh-CN", "MRP计划", "菜单导航"),
            // menu.logistics.manufacturing.mrp._self
            ("menu.logistics.manufacturing.mrp._self", "zh-HK", "MRP计划_hk", "菜单导航"),

            // menu.logistics.manufacturing.aps._self
            ("menu.logistics.manufacturing.aps._self", "en-US", "APS排程_us", "菜单导航"),
            // menu.logistics.manufacturing.aps._self
            ("menu.logistics.manufacturing.aps._self", "ja-JP", "APS排程_jp", "菜单导航"),
            // menu.logistics.manufacturing.aps._self
            ("menu.logistics.manufacturing.aps._self", "zh-CN", "APS排程", "菜单导航"),
            // menu.logistics.manufacturing.aps._self
            ("menu.logistics.manufacturing.aps._self", "zh-HK", "APS排程_hk", "菜单导航"),

            // menu.logistics.manufacturing.output._self
            ("menu.logistics.manufacturing.output._self", "en-US", "OPH管理_us", "菜单导航"),
            // menu.logistics.manufacturing.output._self
            ("menu.logistics.manufacturing.output._self", "ja-JP", "OPH管理_jp", "菜单导航"),
            // menu.logistics.manufacturing.output._self
            ("menu.logistics.manufacturing.output._self", "zh-CN", "OPH管理", "菜单导航"),
            // menu.logistics.manufacturing.output._self
            ("menu.logistics.manufacturing.output._self", "zh-HK", "OPH管理_hk", "菜单导航"),

            // menu.logistics.manufacturing.defect._self
            ("menu.logistics.manufacturing.defect._self", "en-US", "生产不良_us", "菜单导航"),
            // menu.logistics.manufacturing.defect._self
            ("menu.logistics.manufacturing.defect._self", "ja-JP", "生产不良_jp", "菜单导航"),
            // menu.logistics.manufacturing.defect._self
            ("menu.logistics.manufacturing.defect._self", "zh-CN", "生产不良", "菜单导航"),
            // menu.logistics.manufacturing.defect._self
            ("menu.logistics.manufacturing.defect._self", "zh-HK", "生产不良_hk", "菜单导航"),

            // menu.logistics.manufacturing.labor.hour._self
            ("menu.logistics.manufacturing.labor.hour._self", "en-US", "OPH工数_us", "菜单导航"),
            // menu.logistics.manufacturing.labor.hour._self
            ("menu.logistics.manufacturing.labor.hour._self", "ja-JP", "OPH工数_jp", "菜单导航"),
            // menu.logistics.manufacturing.labor.hour._self
            ("menu.logistics.manufacturing.labor.hour._self", "zh-CN", "OPH工数", "菜单导航"),
            // menu.logistics.manufacturing.labor.hour._self
            ("menu.logistics.manufacturing.labor.hour._self", "zh-HK", "OPH工数_hk", "菜单导航"),

            // menu.logistics.manufacturing.sop._self
            ("menu.logistics.manufacturing.sop._self", "en-US", "SOP管理_us", "菜单导航"),
            // menu.logistics.manufacturing.sop._self
            ("menu.logistics.manufacturing.sop._self", "ja-JP", "SOP管理_jp", "菜单导航"),
            // menu.logistics.manufacturing.sop._self
            ("menu.logistics.manufacturing.sop._self", "zh-CN", "SOP管理", "菜单导航"),
            // menu.logistics.manufacturing.sop._self
            ("menu.logistics.manufacturing.sop._self", "zh-HK", "SOP管理_hk", "菜单导航"),

            // menu.logistics.quality.operation._self
            ("menu.logistics.quality.operation._self", "en-US", "质量业务_us", "菜单导航"),
            // menu.logistics.quality.operation._self
            ("menu.logistics.quality.operation._self", "ja-JP", "质量业务_jp", "菜单导航"),
            // menu.logistics.quality.operation._self
            ("menu.logistics.quality.operation._self", "zh-CN", "质量业务", "菜单导航"),
            // menu.logistics.quality.operation._self
            ("menu.logistics.quality.operation._self", "zh-HK", "质量业务_hk", "菜单导航"),

            // menu.logistics.quality.complaint._self
            ("menu.logistics.quality.complaint._self", "en-US", "客诉管理_us", "菜单导航"),
            // menu.logistics.quality.complaint._self
            ("menu.logistics.quality.complaint._self", "ja-JP", "客诉管理_jp", "菜单导航"),
            // menu.logistics.quality.complaint._self
            ("menu.logistics.quality.complaint._self", "zh-CN", "客诉管理", "菜单导航"),
            // menu.logistics.quality.complaint._self
            ("menu.logistics.quality.complaint._self", "zh-HK", "客诉管理_hk", "菜单导航"),

            // menu.logistics.quality.cost._self
            ("menu.logistics.quality.cost._self", "en-US", "品质成本_us", "菜单导航"),
            // menu.logistics.quality.cost._self
            ("menu.logistics.quality.cost._self", "ja-JP", "品质成本_jp", "菜单导航"),
            // menu.logistics.quality.cost._self
            ("menu.logistics.quality.cost._self", "zh-CN", "品质成本", "菜单导航"),
            // menu.logistics.quality.cost._self
            ("menu.logistics.quality.cost._self", "zh-HK", "品质成本_hk", "菜单导航"),

            // menu.logistics.customer.service.contract
            ("menu.logistics.customer.service.contract", "en-US", "服务合同_us", "菜单导航"),
            // menu.logistics.customer.service.contract
            ("menu.logistics.customer.service.contract", "ja-JP", "服务合同_jp", "菜单导航"),
            // menu.logistics.customer.service.contract
            ("menu.logistics.customer.service.contract", "zh-CN", "服务合同", "菜单导航"),
            // menu.logistics.customer.service.contract
            ("menu.logistics.customer.service.contract", "zh-HK", "服务合同_hk", "菜单导航"),

            // menu.logistics.customer.service.order
            ("menu.logistics.customer.service.order", "en-US", "服务订单_us", "菜单导航"),
            // menu.logistics.customer.service.order
            ("menu.logistics.customer.service.order", "ja-JP", "服务订单_jp", "菜单导航"),
            // menu.logistics.customer.service.order
            ("menu.logistics.customer.service.order", "zh-CN", "服务订单", "菜单导航"),
            // menu.logistics.customer.service.order
            ("menu.logistics.customer.service.order", "zh-HK", "服务订单_hk", "菜单导航"),

            // menu.logistics.customer.service.request
            ("menu.logistics.customer.service.request", "en-US", "服务请求_us", "菜单导航"),
            // menu.logistics.customer.service.request
            ("menu.logistics.customer.service.request", "ja-JP", "服务请求_jp", "菜单导航"),
            // menu.logistics.customer.service.request
            ("menu.logistics.customer.service.request", "zh-CN", "服务请求", "菜单导航"),
            // menu.logistics.customer.service.request
            ("menu.logistics.customer.service.request", "zh-HK", "服务请求_hk", "菜单导航"),

            // menu.logistics.customer.service.ticket
            ("menu.logistics.customer.service.ticket", "en-US", "服务工单_us", "菜单导航"),
            // menu.logistics.customer.service.ticket
            ("menu.logistics.customer.service.ticket", "ja-JP", "服务工单_jp", "菜单导航"),
            // menu.logistics.customer.service.ticket
            ("menu.logistics.customer.service.ticket", "zh-CN", "服务工单", "菜单导航"),
            // menu.logistics.customer.service.ticket
            ("menu.logistics.customer.service.ticket", "zh-HK", "服务工单_hk", "菜单导航"),

            // menu.logistics.customer.service.group
            ("menu.logistics.customer.service.group", "en-US", "客户服务组_us", "菜单导航"),
            // menu.logistics.customer.service.group
            ("menu.logistics.customer.service.group", "ja-JP", "客户服务组_jp", "菜单导航"),
            // menu.logistics.customer.service.group
            ("menu.logistics.customer.service.group", "zh-CN", "客户服务组", "菜单导航"),
            // menu.logistics.customer.service.group
            ("menu.logistics.customer.service.group", "zh-HK", "客户服务组_hk", "菜单导航"),

            // menu.logistics.maintenance.equipment
            ("menu.logistics.maintenance.equipment", "en-US", "设备信息_us", "菜单导航"),
            // menu.logistics.maintenance.equipment
            ("menu.logistics.maintenance.equipment", "ja-JP", "设备信息_jp", "菜单导航"),
            // menu.logistics.maintenance.equipment
            ("menu.logistics.maintenance.equipment", "zh-CN", "设备信息", "菜单导航"),
            // menu.logistics.maintenance.equipment
            ("menu.logistics.maintenance.equipment", "zh-HK", "设备信息_hk", "菜单导航"),

            // menu.logistics.maintenance.workorder
            ("menu.logistics.maintenance.workorder", "en-US", "维护工单_us", "菜单导航"),
            // menu.logistics.maintenance.workorder
            ("menu.logistics.maintenance.workorder", "ja-JP", "维护工单_jp", "菜单导航"),
            // menu.logistics.maintenance.workorder
            ("menu.logistics.maintenance.workorder", "zh-CN", "维护工单", "菜单导航"),
            // menu.logistics.maintenance.workorder
            ("menu.logistics.maintenance.workorder", "zh-HK", "维护工单_hk", "菜单导航"),

            // menu.logistics.maintenance.history
            ("menu.logistics.maintenance.history", "en-US", "维护履历_us", "菜单导航"),
            // menu.logistics.maintenance.history
            ("menu.logistics.maintenance.history", "ja-JP", "维护履历_jp", "菜单导航"),
            // menu.logistics.maintenance.history
            ("menu.logistics.maintenance.history", "zh-CN", "维护履历", "菜单导航"),
            // menu.logistics.maintenance.history
            ("menu.logistics.maintenance.history", "zh-HK", "维护履历_hk", "菜单导航"),

            // menu.logistics.maintenance.notification
            ("menu.logistics.maintenance.notification", "en-US", "维护通知_us", "菜单导航"),
            // menu.logistics.maintenance.notification
            ("menu.logistics.maintenance.notification", "ja-JP", "维护通知_jp", "菜单导航"),
            // menu.logistics.maintenance.notification
            ("menu.logistics.maintenance.notification", "zh-CN", "维护通知", "菜单导航"),
            // menu.logistics.maintenance.notification
            ("menu.logistics.maintenance.notification", "zh-HK", "维护通知_hk", "菜单导航"),

            // menu.logistics.maintenance.group
            ("menu.logistics.maintenance.group", "en-US", "设备维护组_us", "菜单导航"),
            // menu.logistics.maintenance.group
            ("menu.logistics.maintenance.group", "ja-JP", "设备维护组_jp", "菜单导航"),
            // menu.logistics.maintenance.group
            ("menu.logistics.maintenance.group", "zh-CN", "设备维护组", "菜单导航"),
            // menu.logistics.maintenance.group
            ("menu.logistics.maintenance.group", "zh-HK", "设备维护组_hk", "菜单导航"),

            // menu.logistics.sales.quotation
            ("menu.logistics.sales.quotation", "en-US", "销售报价_us", "菜单导航"),
            // menu.logistics.sales.quotation
            ("menu.logistics.sales.quotation", "ja-JP", "销售报价_jp", "菜单导航"),
            // menu.logistics.sales.quotation
            ("menu.logistics.sales.quotation", "zh-CN", "销售报价", "菜单导航"),
            // menu.logistics.sales.quotation
            ("menu.logistics.sales.quotation", "zh-HK", "销售报价_hk", "菜单导航"),

            // menu.logistics.sales.order
            ("menu.logistics.sales.order", "en-US", "销售订单_us", "菜单导航"),
            // menu.logistics.sales.order
            ("menu.logistics.sales.order", "ja-JP", "销售订单_jp", "菜单导航"),
            // menu.logistics.sales.order
            ("menu.logistics.sales.order", "zh-CN", "销售订单", "菜单导航"),
            // menu.logistics.sales.order
            ("menu.logistics.sales.order", "zh-HK", "销售订单_hk", "菜单导航"),

            // menu.logistics.sales.condition
            ("menu.logistics.sales.condition", "en-US", "销售条件_us", "菜单导航"),
            // menu.logistics.sales.condition
            ("menu.logistics.sales.condition", "ja-JP", "销售条件_jp", "菜单导航"),
            // menu.logistics.sales.condition
            ("menu.logistics.sales.condition", "zh-CN", "销售条件", "菜单导航"),
            // menu.logistics.sales.condition
            ("menu.logistics.sales.condition", "zh-HK", "销售条件_hk", "菜单导航"),

            // menu.logistics.sales.price.trend
            ("menu.logistics.sales.price.trend", "en-US", "销售价格推移_us", "菜单导航"),
            // menu.logistics.sales.price.trend
            ("menu.logistics.sales.price.trend", "ja-JP", "销售价格推移_jp", "菜单导航"),
            // menu.logistics.sales.price.trend
            ("menu.logistics.sales.price.trend", "zh-CN", "销售价格推移", "菜单导航"),
            // menu.logistics.sales.price.trend
            ("menu.logistics.sales.price.trend", "zh-HK", "销售价格推移_hk", "菜单导航"),

            // menu.logistics.sales.so.price.scr.deviation
            ("menu.logistics.sales.so.price.scr.deviation", "en-US", "SO价偏离SCR_us", "菜单导航"),
            // menu.logistics.sales.so.price.scr.deviation
            ("menu.logistics.sales.so.price.scr.deviation", "ja-JP", "SO价偏离SCR_jp", "菜单导航"),
            // menu.logistics.sales.so.price.scr.deviation
            ("menu.logistics.sales.so.price.scr.deviation", "zh-CN", "SO价偏离SCR", "菜单导航"),
            // menu.logistics.sales.so.price.scr.deviation
            ("menu.logistics.sales.so.price.scr.deviation", "zh-HK", "SO价偏离SCR_hk", "菜单导航"),

            // menu.logistics.sales.invoice
            ("menu.logistics.sales.invoice", "en-US", "销售发票_us", "菜单导航"),
            // menu.logistics.sales.invoice
            ("menu.logistics.sales.invoice", "ja-JP", "销售发票_jp", "菜单导航"),
            // menu.logistics.sales.invoice
            ("menu.logistics.sales.invoice", "zh-CN", "销售发票", "菜单导航"),
            // menu.logistics.sales.invoice
            ("menu.logistics.sales.invoice", "zh-HK", "销售发票_hk", "菜单导航"),

            // menu.logistics.sales.actual.trend
            ("menu.logistics.sales.actual.trend", "en-US", "销售实绩推移_us", "菜单导航"),
            // menu.logistics.sales.actual.trend
            ("menu.logistics.sales.actual.trend", "ja-JP", "销售实绩推移_jp", "菜单导航"),
            // menu.logistics.sales.actual.trend
            ("menu.logistics.sales.actual.trend", "zh-CN", "销售实绩推移", "菜单导航"),
            // menu.logistics.sales.actual.trend
            ("menu.logistics.sales.actual.trend", "zh-HK", "销售实绩推移_hk", "菜单导航"),

            // menu.logistics.sales.customer
            ("menu.logistics.sales.customer", "en-US", "客户信息_us", "菜单导航"),
            // menu.logistics.sales.customer
            ("menu.logistics.sales.customer", "ja-JP", "客户信息_jp", "菜单导航"),
            // menu.logistics.sales.customer
            ("menu.logistics.sales.customer", "zh-CN", "客户信息", "菜单导航"),
            // menu.logistics.sales.customer
            ("menu.logistics.sales.customer", "zh-HK", "客户信息_hk", "菜单导航"),

            // menu.logistics.sales.client
            ("menu.logistics.sales.client", "en-US", "顾客信息_us", "菜单导航"),
            // menu.logistics.sales.client
            ("menu.logistics.sales.client", "ja-JP", "顾客信息_jp", "菜单导航"),
            // menu.logistics.sales.client
            ("menu.logistics.sales.client", "zh-CN", "顾客信息", "菜单导航"),
            // menu.logistics.sales.client
            ("menu.logistics.sales.client", "zh-HK", "顾客信息_hk", "菜单导航"),

            // menu.logistics.sales.seller.material
            ("menu.logistics.sales.seller.material", "en-US", "销售商物料_us", "菜单导航"),
            // menu.logistics.sales.seller.material
            ("menu.logistics.sales.seller.material", "ja-JP", "销售商物料_jp", "菜单导航"),
            // menu.logistics.sales.seller.material
            ("menu.logistics.sales.seller.material", "zh-CN", "销售商物料", "菜单导航"),
            // menu.logistics.sales.seller.material
            ("menu.logistics.sales.seller.material", "zh-HK", "销售商物料_hk", "菜单导航"),

            // menu.logistics.sales.group
            ("menu.logistics.sales.group", "en-US", "销售组_us", "菜单导航"),
            // menu.logistics.sales.group
            ("menu.logistics.sales.group", "ja-JP", "销售组_jp", "菜单导航"),
            // menu.logistics.sales.group
            ("menu.logistics.sales.group", "zh-CN", "销售组", "菜单导航"),
            // menu.logistics.sales.group
            ("menu.logistics.sales.group", "zh-HK", "销售组_hk", "菜单导航"),

            // menu.logistics.serial.inbound
            ("menu.logistics.serial.inbound", "en-US", "序列号入库_us", "菜单导航"),
            // menu.logistics.serial.inbound
            ("menu.logistics.serial.inbound", "ja-JP", "序列号入库_jp", "菜单导航"),
            // menu.logistics.serial.inbound
            ("menu.logistics.serial.inbound", "zh-CN", "序列号入库", "菜单导航"),
            // menu.logistics.serial.inbound
            ("menu.logistics.serial.inbound", "zh-HK", "序列号入库_hk", "菜单导航"),

            // menu.logistics.serial.outbound
            ("menu.logistics.serial.outbound", "en-US", "序列号出库_us", "菜单导航"),
            // menu.logistics.serial.outbound
            ("menu.logistics.serial.outbound", "ja-JP", "序列号出库_jp", "菜单导航"),
            // menu.logistics.serial.outbound
            ("menu.logistics.serial.outbound", "zh-CN", "序列号出库", "菜单导航"),
            // menu.logistics.serial.outbound
            ("menu.logistics.serial.outbound", "zh-HK", "序列号出库_hk", "菜单导航"),

            // menu.logistics.serial.summary
            ("menu.logistics.serial.summary", "en-US", "序列号汇总_us", "菜单导航"),
            // menu.logistics.serial.summary
            ("menu.logistics.serial.summary", "ja-JP", "序列号汇总_jp", "菜单导航"),
            // menu.logistics.serial.summary
            ("menu.logistics.serial.summary", "zh-CN", "序列号汇总", "菜单导航"),
            // menu.logistics.serial.summary
            ("menu.logistics.serial.summary", "zh-HK", "序列号汇总_hk", "菜单导航"),

            // menu.logistics.serial.upload
            ("menu.logistics.serial.upload", "en-US", "序列号上传_us", "菜单导航"),
            // menu.logistics.serial.upload
            ("menu.logistics.serial.upload", "ja-JP", "序列号上传_jp", "菜单导航"),
            // menu.logistics.serial.upload
            ("menu.logistics.serial.upload", "zh-CN", "序列号上传", "菜单导航"),
            // menu.logistics.serial.upload
            ("menu.logistics.serial.upload", "zh-HK", "序列号上传_hk", "菜单导航"),

            // menu.human.resource.organization.dept
            ("menu.human.resource.organization.dept", "en-US", "部门管理_us", "菜单导航"),
            // menu.human.resource.organization.dept
            ("menu.human.resource.organization.dept", "ja-JP", "部门管理_jp", "菜单导航"),
            // menu.human.resource.organization.dept
            ("menu.human.resource.organization.dept", "zh-CN", "部门管理", "菜单导航"),
            // menu.human.resource.organization.dept
            ("menu.human.resource.organization.dept", "zh-HK", "部门管理_hk", "菜单导航"),

            // menu.human.resource.organization.post
            ("menu.human.resource.organization.post", "en-US", "岗位管理_us", "菜单导航"),
            // menu.human.resource.organization.post
            ("menu.human.resource.organization.post", "ja-JP", "岗位管理_jp", "菜单导航"),
            // menu.human.resource.organization.post
            ("menu.human.resource.organization.post", "zh-CN", "岗位管理", "菜单导航"),
            // menu.human.resource.organization.post
            ("menu.human.resource.organization.post", "zh-HK", "岗位管理_hk", "菜单导航"),

            // menu.human.resource.personnel.employee
            ("menu.human.resource.personnel.employee", "en-US", "员工档案_us", "菜单导航"),
            // menu.human.resource.personnel.employee
            ("menu.human.resource.personnel.employee", "ja-JP", "员工档案_jp", "菜单导航"),
            // menu.human.resource.personnel.employee
            ("menu.human.resource.personnel.employee", "zh-CN", "员工档案", "菜单导航"),
            // menu.human.resource.personnel.employee
            ("menu.human.resource.personnel.employee", "zh-HK", "员工档案_hk", "菜单导航"),

            // menu.human.resource.personnel.employee.onboarding
            ("menu.human.resource.personnel.employee.onboarding", "en-US", "入职待办_us", "菜单导航"),
            // menu.human.resource.personnel.employee.onboarding
            ("menu.human.resource.personnel.employee.onboarding", "ja-JP", "入职待办_jp", "菜单导航"),
            // menu.human.resource.personnel.employee.onboarding
            ("menu.human.resource.personnel.employee.onboarding", "zh-CN", "入职待办", "菜单导航"),
            // menu.human.resource.personnel.employee.onboarding
            ("menu.human.resource.personnel.employee.onboarding", "zh-HK", "入职待办_hk", "菜单导航"),

            // menu.human.resource.personnel.employee.joined
            ("menu.human.resource.personnel.employee.joined", "en-US", "入职上岗_us", "菜单导航"),
            // menu.human.resource.personnel.employee.joined
            ("menu.human.resource.personnel.employee.joined", "ja-JP", "入职上岗_jp", "菜单导航"),
            // menu.human.resource.personnel.employee.joined
            ("menu.human.resource.personnel.employee.joined", "zh-CN", "入职上岗", "菜单导航"),
            // menu.human.resource.personnel.employee.joined
            ("menu.human.resource.personnel.employee.joined", "zh-HK", "入职上岗_hk", "菜单导航"),

            // menu.human.resource.personnel.employee.address
            ("menu.human.resource.personnel.employee.address", "en-US", "员工地址_us", "菜单导航"),
            // menu.human.resource.personnel.employee.address
            ("menu.human.resource.personnel.employee.address", "ja-JP", "员工地址_jp", "菜单导航"),
            // menu.human.resource.personnel.employee.address
            ("menu.human.resource.personnel.employee.address", "zh-CN", "员工地址", "菜单导航"),
            // menu.human.resource.personnel.employee.address
            ("menu.human.resource.personnel.employee.address", "zh-HK", "员工地址_hk", "菜单导航"),

            // menu.human.resource.personnel.employee.education
            ("menu.human.resource.personnel.employee.education", "en-US", "教育经历_us", "菜单导航"),
            // menu.human.resource.personnel.employee.education
            ("menu.human.resource.personnel.employee.education", "ja-JP", "教育经历_jp", "菜单导航"),
            // menu.human.resource.personnel.employee.education
            ("menu.human.resource.personnel.employee.education", "zh-CN", "教育经历", "菜单导航"),
            // menu.human.resource.personnel.employee.education
            ("menu.human.resource.personnel.employee.education", "zh-HK", "教育经历_hk", "菜单导航"),

            // menu.human.resource.personnel.employee.family
            ("menu.human.resource.personnel.employee.family", "en-US", "家庭成员_us", "菜单导航"),
            // menu.human.resource.personnel.employee.family
            ("menu.human.resource.personnel.employee.family", "ja-JP", "家庭成员_jp", "菜单导航"),
            // menu.human.resource.personnel.employee.family
            ("menu.human.resource.personnel.employee.family", "zh-CN", "家庭成员", "菜单导航"),
            // menu.human.resource.personnel.employee.family
            ("menu.human.resource.personnel.employee.family", "zh-HK", "家庭成员_hk", "菜单导航"),

            // menu.human.resource.personnel.employee.experience
            ("menu.human.resource.personnel.employee.experience", "en-US", "工作经历_us", "菜单导航"),
            // menu.human.resource.personnel.employee.experience
            ("menu.human.resource.personnel.employee.experience", "ja-JP", "工作经历_jp", "菜单导航"),
            // menu.human.resource.personnel.employee.experience
            ("menu.human.resource.personnel.employee.experience", "zh-CN", "工作经历", "菜单导航"),
            // menu.human.resource.personnel.employee.experience
            ("menu.human.resource.personnel.employee.experience", "zh-HK", "工作经历_hk", "菜单导航"),

            // menu.human.resource.personnel.employee.skill
            ("menu.human.resource.personnel.employee.skill", "en-US", "技能证书_us", "菜单导航"),
            // menu.human.resource.personnel.employee.skill
            ("menu.human.resource.personnel.employee.skill", "ja-JP", "技能证书_jp", "菜单导航"),
            // menu.human.resource.personnel.employee.skill
            ("menu.human.resource.personnel.employee.skill", "zh-CN", "技能证书", "菜单导航"),
            // menu.human.resource.personnel.employee.skill
            ("menu.human.resource.personnel.employee.skill", "zh-HK", "技能证书_hk", "菜单导航"),

            // menu.human.resource.personnel.employee.contract
            ("menu.human.resource.personnel.employee.contract", "en-US", "员工合同_us", "菜单导航"),
            // menu.human.resource.personnel.employee.contract
            ("menu.human.resource.personnel.employee.contract", "ja-JP", "员工合同_jp", "菜单导航"),
            // menu.human.resource.personnel.employee.contract
            ("menu.human.resource.personnel.employee.contract", "zh-CN", "员工合同", "菜单导航"),
            // menu.human.resource.personnel.employee.contract
            ("menu.human.resource.personnel.employee.contract", "zh-HK", "员工合同_hk", "菜单导航"),

            // menu.human.resource.personnel.employee.reassignment
            ("menu.human.resource.personnel.employee.reassignment", "en-US", "员工调动_us", "菜单导航"),
            // menu.human.resource.personnel.employee.reassignment
            ("menu.human.resource.personnel.employee.reassignment", "ja-JP", "员工调动_jp", "菜单导航"),
            // menu.human.resource.personnel.employee.reassignment
            ("menu.human.resource.personnel.employee.reassignment", "zh-CN", "员工调动", "菜单导航"),
            // menu.human.resource.personnel.employee.reassignment
            ("menu.human.resource.personnel.employee.reassignment", "zh-HK", "员工调动_hk", "菜单导航"),

            // menu.human.resource.personnel.employee.resignation
            ("menu.human.resource.personnel.employee.resignation", "en-US", "员工离职_us", "菜单导航"),
            // menu.human.resource.personnel.employee.resignation
            ("menu.human.resource.personnel.employee.resignation", "ja-JP", "员工离职_jp", "菜单导航"),
            // menu.human.resource.personnel.employee.resignation
            ("menu.human.resource.personnel.employee.resignation", "zh-CN", "员工离职", "菜单导航"),
            // menu.human.resource.personnel.employee.resignation
            ("menu.human.resource.personnel.employee.resignation", "zh-HK", "员工离职_hk", "菜单导航"),

            // menu.human.resource.personnel.employee.delegation
            ("menu.human.resource.personnel.employee.delegation", "en-US", "员工代理_us", "菜单导航"),
            // menu.human.resource.personnel.employee.delegation
            ("menu.human.resource.personnel.employee.delegation", "ja-JP", "员工代理_jp", "菜单导航"),
            // menu.human.resource.personnel.employee.delegation
            ("menu.human.resource.personnel.employee.delegation", "zh-CN", "员工代理", "菜单导航"),
            // menu.human.resource.personnel.employee.delegation
            ("menu.human.resource.personnel.employee.delegation", "zh-HK", "员工代理_hk", "菜单导航"),

            // menu.human.resource.personnel.employee.attachment
            ("menu.human.resource.personnel.employee.attachment", "en-US", "档案附件_us", "菜单导航"),
            // menu.human.resource.personnel.employee.attachment
            ("menu.human.resource.personnel.employee.attachment", "ja-JP", "档案附件_jp", "菜单导航"),
            // menu.human.resource.personnel.employee.attachment
            ("menu.human.resource.personnel.employee.attachment", "zh-CN", "档案附件", "菜单导航"),
            // menu.human.resource.personnel.employee.attachment
            ("menu.human.resource.personnel.employee.attachment", "zh-HK", "档案附件_hk", "菜单导航"),

            // menu.human.resource.attendance.calendar
            ("menu.human.resource.attendance.calendar", "en-US", "工厂日历_us", "菜单导航"),
            // menu.human.resource.attendance.calendar
            ("menu.human.resource.attendance.calendar", "ja-JP", "工厂日历_jp", "菜单导航"),
            // menu.human.resource.attendance.calendar
            ("menu.human.resource.attendance.calendar", "zh-CN", "工厂日历", "菜单导航"),
            // menu.human.resource.attendance.calendar
            ("menu.human.resource.attendance.calendar", "zh-HK", "工厂日历_hk", "菜单导航"),

            // menu.human.resource.attendance.holiday
            ("menu.human.resource.attendance.holiday", "en-US", "假期管理_us", "菜单导航"),
            // menu.human.resource.attendance.holiday
            ("menu.human.resource.attendance.holiday", "ja-JP", "假期管理_jp", "菜单导航"),
            // menu.human.resource.attendance.holiday
            ("menu.human.resource.attendance.holiday", "zh-CN", "假期管理", "菜单导航"),
            // menu.human.resource.attendance.holiday
            ("menu.human.resource.attendance.holiday", "zh-HK", "假期管理_hk", "菜单导航"),

            // menu.human.resource.attendance.shift.schedule
            ("menu.human.resource.attendance.shift.schedule", "en-US", "排班计划_us", "菜单导航"),
            // menu.human.resource.attendance.shift.schedule
            ("menu.human.resource.attendance.shift.schedule", "ja-JP", "排班计划_jp", "菜单导航"),
            // menu.human.resource.attendance.shift.schedule
            ("menu.human.resource.attendance.shift.schedule", "zh-CN", "排班计划", "菜单导航"),
            // menu.human.resource.attendance.shift.schedule
            ("menu.human.resource.attendance.shift.schedule", "zh-HK", "排班计划_hk", "菜单导航"),

            // menu.human.resource.attendance.work.shift
            ("menu.human.resource.attendance.work.shift", "en-US", "班次管理_us", "菜单导航"),
            // menu.human.resource.attendance.work.shift
            ("menu.human.resource.attendance.work.shift", "ja-JP", "班次管理_jp", "菜单导航"),
            // menu.human.resource.attendance.work.shift
            ("menu.human.resource.attendance.work.shift", "zh-CN", "班次管理", "菜单导航"),
            // menu.human.resource.attendance.work.shift
            ("menu.human.resource.attendance.work.shift", "zh-HK", "班次管理_hk", "菜单导航"),

            // menu.human.resource.attendance.leave
            ("menu.human.resource.attendance.leave", "en-US", "请假管理_us", "菜单导航"),
            // menu.human.resource.attendance.leave
            ("menu.human.resource.attendance.leave", "ja-JP", "请假管理_jp", "菜单导航"),
            // menu.human.resource.attendance.leave
            ("menu.human.resource.attendance.leave", "zh-CN", "请假管理", "菜单导航"),
            // menu.human.resource.attendance.leave
            ("menu.human.resource.attendance.leave", "zh-HK", "请假管理_hk", "菜单导航"),

            // menu.human.resource.attendance.overtime
            ("menu.human.resource.attendance.overtime", "en-US", "加班管理_us", "菜单导航"),
            // menu.human.resource.attendance.overtime
            ("menu.human.resource.attendance.overtime", "ja-JP", "加班管理_jp", "菜单导航"),
            // menu.human.resource.attendance.overtime
            ("menu.human.resource.attendance.overtime", "zh-CN", "加班管理", "菜单导航"),
            // menu.human.resource.attendance.overtime
            ("menu.human.resource.attendance.overtime", "zh-HK", "加班管理_hk", "菜单导航"),

            // menu.human.resource.compensation.salary.item
            ("menu.human.resource.compensation.salary.item", "en-US", "薪资项目_us", "菜单导航"),
            // menu.human.resource.compensation.salary.item
            ("menu.human.resource.compensation.salary.item", "ja-JP", "薪资项目_jp", "菜单导航"),
            // menu.human.resource.compensation.salary.item
            ("menu.human.resource.compensation.salary.item", "zh-CN", "薪资项目", "菜单导航"),
            // menu.human.resource.compensation.salary.item
            ("menu.human.resource.compensation.salary.item", "zh-HK", "薪资项目_hk", "菜单导航"),

            // menu.human.resource.compensation.payroll
            ("menu.human.resource.compensation.payroll", "en-US", "薪酬体系_us", "菜单导航"),
            // menu.human.resource.compensation.payroll
            ("menu.human.resource.compensation.payroll", "ja-JP", "薪酬体系_jp", "菜单导航"),
            // menu.human.resource.compensation.payroll
            ("menu.human.resource.compensation.payroll", "zh-CN", "薪酬体系", "菜单导航"),
            // menu.human.resource.compensation.payroll
            ("menu.human.resource.compensation.payroll", "zh-HK", "薪酬体系_hk", "菜单导航"),

            // menu.human.resource.compensation.pay.scale
            ("menu.human.resource.compensation.pay.scale", "en-US", "薪级_us", "菜单导航"),
            // menu.human.resource.compensation.pay.scale
            ("menu.human.resource.compensation.pay.scale", "ja-JP", "薪级_jp", "菜单导航"),
            // menu.human.resource.compensation.pay.scale
            ("menu.human.resource.compensation.pay.scale", "zh-CN", "薪级", "菜单导航"),
            // menu.human.resource.compensation.pay.scale
            ("menu.human.resource.compensation.pay.scale", "zh-HK", "薪级_hk", "菜单导航"),

            // menu.human.resource.compensation.emp.salary
            ("menu.human.resource.compensation.emp.salary", "en-US", "员工定薪_us", "菜单导航"),
            // menu.human.resource.compensation.emp.salary
            ("menu.human.resource.compensation.emp.salary", "ja-JP", "员工定薪_jp", "菜单导航"),
            // menu.human.resource.compensation.emp.salary
            ("menu.human.resource.compensation.emp.salary", "zh-CN", "员工定薪", "菜单导航"),
            // menu.human.resource.compensation.emp.salary
            ("menu.human.resource.compensation.emp.salary", "zh-HK", "员工定薪_hk", "菜单导航"),

            // menu.human.resource.compensation.bonus.plan
            ("menu.human.resource.compensation.bonus.plan", "en-US", "奖金方案_us", "菜单导航"),
            // menu.human.resource.compensation.bonus.plan
            ("menu.human.resource.compensation.bonus.plan", "ja-JP", "奖金方案_jp", "菜单导航"),
            // menu.human.resource.compensation.bonus.plan
            ("menu.human.resource.compensation.bonus.plan", "zh-CN", "奖金方案", "菜单导航"),
            // menu.human.resource.compensation.bonus.plan
            ("menu.human.resource.compensation.bonus.plan", "zh-HK", "奖金方案_hk", "菜单导航"),

            // menu.human.resource.compensation.salary.formula
            ("menu.human.resource.compensation.salary.formula", "en-US", "薪资计算公式_us", "菜单导航"),
            // menu.human.resource.compensation.salary.formula
            ("menu.human.resource.compensation.salary.formula", "ja-JP", "薪资计算公式_jp", "菜单导航"),
            // menu.human.resource.compensation.salary.formula
            ("menu.human.resource.compensation.salary.formula", "zh-CN", "薪资计算公式", "菜单导航"),
            // menu.human.resource.compensation.salary.formula
            ("menu.human.resource.compensation.salary.formula", "zh-HK", "薪资计算公式_hk", "菜单导航"),

            // menu.human.resource.compensation.payslip
            ("menu.human.resource.compensation.payslip", "en-US", "工资条_us", "菜单导航"),
            // menu.human.resource.compensation.payslip
            ("menu.human.resource.compensation.payslip", "ja-JP", "工资条_jp", "菜单导航"),
            // menu.human.resource.compensation.payslip
            ("menu.human.resource.compensation.payslip", "zh-CN", "工资条", "菜单导航"),
            // menu.human.resource.compensation.payslip
            ("menu.human.resource.compensation.payslip", "zh-HK", "工资条_hk", "菜单导航"),

            // menu.human.resource.benefits.benefit.item
            ("menu.human.resource.benefits.benefit.item", "en-US", "福利项目_us", "菜单导航"),
            // menu.human.resource.benefits.benefit.item
            ("menu.human.resource.benefits.benefit.item", "ja-JP", "福利项目_jp", "菜单导航"),
            // menu.human.resource.benefits.benefit.item
            ("menu.human.resource.benefits.benefit.item", "zh-CN", "福利项目", "菜单导航"),
            // menu.human.resource.benefits.benefit.item
            ("menu.human.resource.benefits.benefit.item", "zh-HK", "福利项目_hk", "菜单导航"),

            // menu.human.resource.benefits.emp.benefit.plan
            ("menu.human.resource.benefits.emp.benefit.plan", "en-US", "员工福利方案_us", "菜单导航"),
            // menu.human.resource.benefits.emp.benefit.plan
            ("menu.human.resource.benefits.emp.benefit.plan", "ja-JP", "员工福利方案_jp", "菜单导航"),
            // menu.human.resource.benefits.emp.benefit.plan
            ("menu.human.resource.benefits.emp.benefit.plan", "zh-CN", "员工福利方案", "菜单导航"),
            // menu.human.resource.benefits.emp.benefit.plan
            ("menu.human.resource.benefits.emp.benefit.plan", "zh-HK", "员工福利方案_hk", "菜单导航"),

            // menu.human.resource.benefits.social.insurance
            ("menu.human.resource.benefits.social.insurance", "en-US", "社保公积金_us", "菜单导航"),
            // menu.human.resource.benefits.social.insurance
            ("menu.human.resource.benefits.social.insurance", "ja-JP", "社保公积金_jp", "菜单导航"),
            // menu.human.resource.benefits.social.insurance
            ("menu.human.resource.benefits.social.insurance", "zh-CN", "社保公积金", "菜单导航"),
            // menu.human.resource.benefits.social.insurance
            ("menu.human.resource.benefits.social.insurance", "zh-HK", "社保公积金_hk", "菜单导航"),

            // menu.human.resource.performance.perf.cycle
            ("menu.human.resource.performance.perf.cycle", "en-US", "绩效周期_us", "菜单导航"),
            // menu.human.resource.performance.perf.cycle
            ("menu.human.resource.performance.perf.cycle", "ja-JP", "绩效周期_jp", "菜单导航"),
            // menu.human.resource.performance.perf.cycle
            ("menu.human.resource.performance.perf.cycle", "zh-CN", "绩效周期", "菜单导航"),
            // menu.human.resource.performance.perf.cycle
            ("menu.human.resource.performance.perf.cycle", "zh-HK", "绩效周期_hk", "菜单导航"),

            // menu.human.resource.performance.perf.scheme
            ("menu.human.resource.performance.perf.scheme", "en-US", "绩效方案_us", "菜单导航"),
            // menu.human.resource.performance.perf.scheme
            ("menu.human.resource.performance.perf.scheme", "ja-JP", "绩效方案_jp", "菜单导航"),
            // menu.human.resource.performance.perf.scheme
            ("menu.human.resource.performance.perf.scheme", "zh-CN", "绩效方案", "菜单导航"),
            // menu.human.resource.performance.perf.scheme
            ("menu.human.resource.performance.perf.scheme", "zh-HK", "绩效方案_hk", "菜单导航"),

            // menu.human.resource.performance.perf.objective
            ("menu.human.resource.performance.perf.objective", "en-US", "绩效目标_us", "菜单导航"),
            // menu.human.resource.performance.perf.objective
            ("menu.human.resource.performance.perf.objective", "ja-JP", "绩效目标_jp", "菜单导航"),
            // menu.human.resource.performance.perf.objective
            ("menu.human.resource.performance.perf.objective", "zh-CN", "绩效目标", "菜单导航"),
            // menu.human.resource.performance.perf.objective
            ("menu.human.resource.performance.perf.objective", "zh-HK", "绩效目标_hk", "菜单导航"),

            // menu.human.resource.performance.perf.assessment
            ("menu.human.resource.performance.perf.assessment", "en-US", "绩效考核_us", "菜单导航"),
            // menu.human.resource.performance.perf.assessment
            ("menu.human.resource.performance.perf.assessment", "ja-JP", "绩效考核_jp", "菜单导航"),
            // menu.human.resource.performance.perf.assessment
            ("menu.human.resource.performance.perf.assessment", "zh-CN", "绩效考核", "菜单导航"),
            // menu.human.resource.performance.perf.assessment
            ("menu.human.resource.performance.perf.assessment", "zh-HK", "绩效考核_hk", "菜单导航"),

            // menu.human.resource.performance.perf.analysis
            ("menu.human.resource.performance.perf.analysis", "en-US", "分析改进_us", "菜单导航"),
            // menu.human.resource.performance.perf.analysis
            ("menu.human.resource.performance.perf.analysis", "ja-JP", "分析改进_jp", "菜单导航"),
            // menu.human.resource.performance.perf.analysis
            ("menu.human.resource.performance.perf.analysis", "zh-CN", "分析改进", "菜单导航"),
            // menu.human.resource.performance.perf.analysis
            ("menu.human.resource.performance.perf.analysis", "zh-HK", "分析改进_hk", "菜单导航"),

            // menu.human.resource.training.course
            ("menu.human.resource.training.course", "en-US", "培训课程_us", "菜单导航"),
            // menu.human.resource.training.course
            ("menu.human.resource.training.course", "ja-JP", "培训课程_jp", "菜单导航"),
            // menu.human.resource.training.course
            ("menu.human.resource.training.course", "zh-CN", "培训课程", "菜单导航"),
            // menu.human.resource.training.course
            ("menu.human.resource.training.course", "zh-HK", "培训课程_hk", "菜单导航"),

            // menu.human.resource.training.plan
            ("menu.human.resource.training.plan", "en-US", "年度计划_us", "菜单导航"),
            // menu.human.resource.training.plan
            ("menu.human.resource.training.plan", "ja-JP", "年度计划_jp", "菜单导航"),
            // menu.human.resource.training.plan
            ("menu.human.resource.training.plan", "zh-CN", "年度计划", "菜单导航"),
            // menu.human.resource.training.plan
            ("menu.human.resource.training.plan", "zh-HK", "年度计划_hk", "菜单导航"),

            // menu.human.resource.training.attendee
            ("menu.human.resource.training.attendee", "en-US", "参训记录_us", "菜单导航"),
            // menu.human.resource.training.attendee
            ("menu.human.resource.training.attendee", "ja-JP", "参训记录_jp", "菜单导航"),
            // menu.human.resource.training.attendee
            ("menu.human.resource.training.attendee", "zh-CN", "参训记录", "菜单导航"),
            // menu.human.resource.training.attendee
            ("menu.human.resource.training.attendee", "zh-HK", "参训记录_hk", "菜单导航"),

            // menu.human.resource.talent.staffing.requirement
            ("menu.human.resource.talent.staffing.requirement", "en-US", "用人需求_us", "菜单导航"),
            // menu.human.resource.talent.staffing.requirement
            ("menu.human.resource.talent.staffing.requirement", "ja-JP", "用人需求_jp", "菜单导航"),
            // menu.human.resource.talent.staffing.requirement
            ("menu.human.resource.talent.staffing.requirement", "zh-CN", "用人需求", "菜单导航"),
            // menu.human.resource.talent.staffing.requirement
            ("menu.human.resource.talent.staffing.requirement", "zh-HK", "用人需求_hk", "菜单导航"),

            // menu.human.resource.talent.job.posting
            ("menu.human.resource.talent.job.posting", "en-US", "职位发布_us", "菜单导航"),
            // menu.human.resource.talent.job.posting
            ("menu.human.resource.talent.job.posting", "ja-JP", "职位发布_jp", "菜单导航"),
            // menu.human.resource.talent.job.posting
            ("menu.human.resource.talent.job.posting", "zh-CN", "职位发布", "菜单导航"),
            // menu.human.resource.talent.job.posting
            ("menu.human.resource.talent.job.posting", "zh-HK", "职位发布_hk", "菜单导航"),

            // menu.human.resource.talent.offer
            ("menu.human.resource.talent.offer", "en-US", "录用_us", "菜单导航"),
            // menu.human.resource.talent.offer
            ("menu.human.resource.talent.offer", "ja-JP", "录用_jp", "菜单导航"),
            // menu.human.resource.talent.offer
            ("menu.human.resource.talent.offer", "zh-CN", "录用", "菜单导航"),
            // menu.human.resource.talent.offer
            ("menu.human.resource.talent.offer", "zh-HK", "录用_hk", "菜单导航"),

            // menu.statistics.quickquery.configurable
            ("menu.statistics.quickquery.configurable", "en-US", "定制报表_us", "菜单导航"),
            // menu.statistics.quickquery.configurable
            ("menu.statistics.quickquery.configurable", "ja-JP", "定制报表_jp", "菜单导航"),
            // menu.statistics.quickquery.configurable
            ("menu.statistics.quickquery.configurable", "zh-CN", "定制报表", "菜单导航"),
            // menu.statistics.quickquery.configurable
            ("menu.statistics.quickquery.configurable", "zh-HK", "定制报表_hk", "菜单导航"),

            // menu.statistics.logging.login.log
            ("menu.statistics.logging.login.log", "en-US", "登录日志_us", "菜单导航"),
            // menu.statistics.logging.login.log
            ("menu.statistics.logging.login.log", "ja-JP", "登录日志_jp", "菜单导航"),
            // menu.statistics.logging.login.log
            ("menu.statistics.logging.login.log", "zh-CN", "登录日志", "菜单导航"),
            // menu.statistics.logging.login.log
            ("menu.statistics.logging.login.log", "zh-HK", "登录日志_hk", "菜单导航"),

            // menu.statistics.logging.oper.log
            ("menu.statistics.logging.oper.log", "en-US", "操作日志_us", "菜单导航"),
            // menu.statistics.logging.oper.log
            ("menu.statistics.logging.oper.log", "ja-JP", "操作日志_jp", "菜单导航"),
            // menu.statistics.logging.oper.log
            ("menu.statistics.logging.oper.log", "zh-CN", "操作日志", "菜单导航"),
            // menu.statistics.logging.oper.log
            ("menu.statistics.logging.oper.log", "zh-HK", "操作日志_hk", "菜单导航"),

            // menu.statistics.logging.delta.log
            ("menu.statistics.logging.delta.log", "en-US", "差异日志_us", "菜单导航"),
            // menu.statistics.logging.delta.log
            ("menu.statistics.logging.delta.log", "ja-JP", "差异日志_jp", "菜单导航"),
            // menu.statistics.logging.delta.log
            ("menu.statistics.logging.delta.log", "zh-CN", "差异日志", "菜单导航"),
            // menu.statistics.logging.delta.log
            ("menu.statistics.logging.delta.log", "zh-HK", "差异日志_hk", "菜单导航"),

            // menu.statistics.logging.quartz.log
            ("menu.statistics.logging.quartz.log", "en-US", "任务日志_us", "菜单导航"),
            // menu.statistics.logging.quartz.log
            ("menu.statistics.logging.quartz.log", "ja-JP", "任务日志_jp", "菜单导航"),
            // menu.statistics.logging.quartz.log
            ("menu.statistics.logging.quartz.log", "zh-CN", "任务日志", "菜单导航"),
            // menu.statistics.logging.quartz.log
            ("menu.statistics.logging.quartz.log", "zh-HK", "任务日志_hk", "菜单导航"),

            // menu.statistics.logging.server.monitor
            ("menu.statistics.logging.server.monitor", "en-US", "服务监控_us", "菜单导航"),
            // menu.statistics.logging.server.monitor
            ("menu.statistics.logging.server.monitor", "ja-JP", "服务监控_jp", "菜单导航"),
            // menu.statistics.logging.server.monitor
            ("menu.statistics.logging.server.monitor", "zh-CN", "服务监控", "菜单导航"),
            // menu.statistics.logging.server.monitor
            ("menu.statistics.logging.server.monitor", "zh-HK", "服务监控_hk", "菜单导航"),

            // menu.statistics.logging.tracking.log
            ("menu.statistics.logging.tracking.log", "en-US", "交互日志_us", "菜单导航"),
            // menu.statistics.logging.tracking.log
            ("menu.statistics.logging.tracking.log", "ja-JP", "交互日志_jp", "菜单导航"),
            // menu.statistics.logging.tracking.log
            ("menu.statistics.logging.tracking.log", "zh-CN", "交互日志", "菜单导航"),
            // menu.statistics.logging.tracking.log
            ("menu.statistics.logging.tracking.log", "zh-HK", "交互日志_hk", "菜单导航"),

            // menu.statistics.logging.archive.log
            ("menu.statistics.logging.archive.log", "en-US", "归档日志_us", "菜单导航"),
            // menu.statistics.logging.archive.log
            ("menu.statistics.logging.archive.log", "ja-JP", "归档日志_jp", "菜单导航"),
            // menu.statistics.logging.archive.log
            ("menu.statistics.logging.archive.log", "zh-CN", "归档日志", "菜单导航"),
            // menu.statistics.logging.archive.log
            ("menu.statistics.logging.archive.log", "zh-HK", "归档日志_hk", "菜单导航"),

            // menu.statistics.logging.backup.log
            ("menu.statistics.logging.backup.log", "en-US", "备份日志_us", "菜单导航"),
            // menu.statistics.logging.backup.log
            ("menu.statistics.logging.backup.log", "ja-JP", "备份日志_jp", "菜单导航"),
            // menu.statistics.logging.backup.log
            ("menu.statistics.logging.backup.log", "zh-CN", "备份日志", "菜单导航"),
            // menu.statistics.logging.backup.log
            ("menu.statistics.logging.backup.log", "zh-HK", "备份日志_hk", "菜单导航"),

            // menu.routine.meeting.center.meeting
            ("menu.routine.meeting.center.meeting", "en-US", "会议_us", "菜单导航"),
            // menu.routine.meeting.center.meeting
            ("menu.routine.meeting.center.meeting", "ja-JP", "会议_jp", "菜单导航"),
            // menu.routine.meeting.center.meeting
            ("menu.routine.meeting.center.meeting", "zh-CN", "会议", "菜单导航"),
            // menu.routine.meeting.center.meeting
            ("menu.routine.meeting.center.meeting", "zh-HK", "会议_hk", "菜单导航"),

            // menu.routine.meeting.center.room
            ("menu.routine.meeting.center.room", "en-US", "会议室_us", "菜单导航"),
            // menu.routine.meeting.center.room
            ("menu.routine.meeting.center.room", "ja-JP", "会议室_jp", "菜单导航"),
            // menu.routine.meeting.center.room
            ("menu.routine.meeting.center.room", "zh-CN", "会议室", "菜单导航"),
            // menu.routine.meeting.center.room
            ("menu.routine.meeting.center.room", "zh-HK", "会议室_hk", "菜单导航"),

            // menu.routine.help.desk.my.ticket
            ("menu.routine.help.desk.my.ticket", "en-US", "我的工单_us", "菜单导航"),
            // menu.routine.help.desk.my.ticket
            ("menu.routine.help.desk.my.ticket", "ja-JP", "我的工单_jp", "菜单导航"),
            // menu.routine.help.desk.my.ticket
            ("menu.routine.help.desk.my.ticket", "zh-CN", "我的工单", "菜单导航"),
            // menu.routine.help.desk.my.ticket
            ("menu.routine.help.desk.my.ticket", "zh-HK", "我的工单_hk", "菜单导航"),

            // menu.routine.help.desk.ticket
            ("menu.routine.help.desk.ticket", "en-US", "工单管理_us", "菜单导航"),
            // menu.routine.help.desk.ticket
            ("menu.routine.help.desk.ticket", "ja-JP", "工单管理_jp", "菜单导航"),
            // menu.routine.help.desk.ticket
            ("menu.routine.help.desk.ticket", "zh-CN", "工单管理", "菜单导航"),
            // menu.routine.help.desk.ticket
            ("menu.routine.help.desk.ticket", "zh-HK", "工单管理_hk", "菜单导航"),

            // menu.routine.help.desk.knowledge
            ("menu.routine.help.desk.knowledge", "en-US", "知识库（FAQ）_us", "菜单导航"),
            // menu.routine.help.desk.knowledge
            ("menu.routine.help.desk.knowledge", "ja-JP", "知识库（FAQ）_jp", "菜单导航"),
            // menu.routine.help.desk.knowledge
            ("menu.routine.help.desk.knowledge", "zh-CN", "知识库（FAQ）", "菜单导航"),
            // menu.routine.help.desk.knowledge
            ("menu.routine.help.desk.knowledge", "zh-HK", "知识库（FAQ）_hk", "菜单导航"),

            // menu.routine.help.desk.my.asset
            ("menu.routine.help.desk.my.asset", "en-US", "我的资产_us", "菜单导航"),
            // menu.routine.help.desk.my.asset
            ("menu.routine.help.desk.my.asset", "ja-JP", "我的资产_jp", "菜单导航"),
            // menu.routine.help.desk.my.asset
            ("menu.routine.help.desk.my.asset", "zh-CN", "我的资产", "菜单导航"),
            // menu.routine.help.desk.my.asset
            ("menu.routine.help.desk.my.asset", "zh-HK", "我的资产_hk", "菜单导航"),

            // menu.routine.help.desk.it.asset
            ("menu.routine.help.desk.it.asset", "en-US", "IT设备保修_us", "菜单导航"),
            // menu.routine.help.desk.it.asset
            ("menu.routine.help.desk.it.asset", "ja-JP", "IT设备保修_jp", "菜单导航"),
            // menu.routine.help.desk.it.asset
            ("menu.routine.help.desk.it.asset", "zh-CN", "IT设备保修", "菜单导航"),
            // menu.routine.help.desk.it.asset
            ("menu.routine.help.desk.it.asset", "zh-HK", "IT设备保修_hk", "菜单导航"),

            // menu.logistics.manufacturing.bom.bill.of.material
            ("menu.logistics.manufacturing.bom.bill.of.material", "en-US", "物料清单_us", "菜单导航"),
            // menu.logistics.manufacturing.bom.bill.of.material
            ("menu.logistics.manufacturing.bom.bill.of.material", "ja-JP", "物料清单_jp", "菜单导航"),
            // menu.logistics.manufacturing.bom.bill.of.material
            ("menu.logistics.manufacturing.bom.bill.of.material", "zh-CN", "物料清单", "菜单导航"),
            // menu.logistics.manufacturing.bom.bill.of.material
            ("menu.logistics.manufacturing.bom.bill.of.material", "zh-HK", "物料清单_hk", "菜单导航"),

            // menu.logistics.manufacturing.bom.routing
            ("menu.logistics.manufacturing.bom.routing", "en-US", "工艺路线_us", "菜单导航"),
            // menu.logistics.manufacturing.bom.routing
            ("menu.logistics.manufacturing.bom.routing", "ja-JP", "工艺路线_jp", "菜单导航"),
            // menu.logistics.manufacturing.bom.routing
            ("menu.logistics.manufacturing.bom.routing", "zh-CN", "工艺路线", "菜单导航"),
            // menu.logistics.manufacturing.bom.routing
            ("menu.logistics.manufacturing.bom.routing", "zh-HK", "工艺路线_hk", "菜单导航"),

            // menu.logistics.manufacturing.bom.standard.operation.time
            ("menu.logistics.manufacturing.bom.standard.operation.time", "en-US", "标准工序时间_us", "菜单导航"),
            // menu.logistics.manufacturing.bom.standard.operation.time
            ("menu.logistics.manufacturing.bom.standard.operation.time", "ja-JP", "标准工序时间_jp", "菜单导航"),
            // menu.logistics.manufacturing.bom.standard.operation.time
            ("menu.logistics.manufacturing.bom.standard.operation.time", "zh-CN", "标准工序时间", "菜单导航"),
            // menu.logistics.manufacturing.bom.standard.operation.time
            ("menu.logistics.manufacturing.bom.standard.operation.time", "zh-HK", "标准工序时间_hk", "菜单导航"),

            // menu.logistics.manufacturing.bom.material.cost
            ("menu.logistics.manufacturing.bom.material.cost", "en-US", "BOM物料成本_us", "菜单导航"),
            // menu.logistics.manufacturing.bom.material.cost
            ("menu.logistics.manufacturing.bom.material.cost", "ja-JP", "BOM物料成本_jp", "菜单导航"),
            // menu.logistics.manufacturing.bom.material.cost
            ("menu.logistics.manufacturing.bom.material.cost", "zh-CN", "BOM物料成本", "菜单导航"),
            // menu.logistics.manufacturing.bom.material.cost
            ("menu.logistics.manufacturing.bom.material.cost", "zh-HK", "BOM物料成本_hk", "菜单导航"),

            // menu.logistics.manufacturing.bom.material.zeroprice
            ("menu.logistics.manufacturing.bom.material.zeroprice", "en-US", "BOM零价格_us", "菜单导航"),
            // menu.logistics.manufacturing.bom.material.zeroprice
            ("menu.logistics.manufacturing.bom.material.zeroprice", "ja-JP", "BOM零价格_jp", "菜单导航"),
            // menu.logistics.manufacturing.bom.material.zeroprice
            ("menu.logistics.manufacturing.bom.material.zeroprice", "zh-CN", "BOM零价格", "菜单导航"),
            // menu.logistics.manufacturing.bom.material.zeroprice
            ("menu.logistics.manufacturing.bom.material.zeroprice", "zh-HK", "BOM零价格_hk", "菜单导航"),

            // menu.logistics.manufacturing.bom.material.cost.analysis
            ("menu.logistics.manufacturing.bom.material.cost.analysis", "en-US", "BOM成本分析_us", "菜单导航"),
            // menu.logistics.manufacturing.bom.material.cost.analysis
            ("menu.logistics.manufacturing.bom.material.cost.analysis", "ja-JP", "BOM成本分析_jp", "菜单导航"),
            // menu.logistics.manufacturing.bom.material.cost.analysis
            ("menu.logistics.manufacturing.bom.material.cost.analysis", "zh-CN", "BOM成本分析", "菜单导航"),
            // menu.logistics.manufacturing.bom.material.cost.analysis
            ("menu.logistics.manufacturing.bom.material.cost.analysis", "zh-HK", "BOM成本分析_hk", "菜单导航"),

            // menu.logistics.manufacturing.bom.material.cost.trend
            ("menu.logistics.manufacturing.bom.material.cost.trend", "en-US", "产品成本推移_us", "菜单导航"),
            // menu.logistics.manufacturing.bom.material.cost.trend
            ("menu.logistics.manufacturing.bom.material.cost.trend", "ja-JP", "产品成本推移_jp", "菜单导航"),
            // menu.logistics.manufacturing.bom.material.cost.trend
            ("menu.logistics.manufacturing.bom.material.cost.trend", "zh-CN", "产品成本推移", "菜单导航"),
            // menu.logistics.manufacturing.bom.material.cost.trend
            ("menu.logistics.manufacturing.bom.material.cost.trend", "zh-HK", "产品成本推移_hk", "菜单导航"),

            // menu.logistics.manufacturing.bom.model.cost.trend
            ("menu.logistics.manufacturing.bom.model.cost.trend", "en-US", "机种成本推移_us", "菜单导航"),
            // menu.logistics.manufacturing.bom.model.cost.trend
            ("menu.logistics.manufacturing.bom.model.cost.trend", "ja-JP", "机种成本推移_jp", "菜单导航"),
            // menu.logistics.manufacturing.bom.model.cost.trend
            ("menu.logistics.manufacturing.bom.model.cost.trend", "zh-CN", "机种成本推移", "菜单导航"),
            // menu.logistics.manufacturing.bom.model.cost.trend
            ("menu.logistics.manufacturing.bom.model.cost.trend", "zh-HK", "机种成本推移_hk", "菜单导航"),

            // menu.logistics.manufacturing.bom.pricedelta.trend
            ("menu.logistics.manufacturing.bom.pricedelta.trend", "en-US", "成本差异推移_us", "菜单导航"),
            // menu.logistics.manufacturing.bom.pricedelta.trend
            ("menu.logistics.manufacturing.bom.pricedelta.trend", "ja-JP", "成本差异推移_jp", "菜单导航"),
            // menu.logistics.manufacturing.bom.pricedelta.trend
            ("menu.logistics.manufacturing.bom.pricedelta.trend", "zh-CN", "成本差异推移", "菜单导航"),
            // menu.logistics.manufacturing.bom.pricedelta.trend
            ("menu.logistics.manufacturing.bom.pricedelta.trend", "zh-HK", "成本差异推移_hk", "菜单导航"),

            // menu.logistics.manufacturing.bom.group
            ("menu.logistics.manufacturing.bom.group", "en-US", "BOM组_us", "菜单导航"),
            // menu.logistics.manufacturing.bom.group
            ("menu.logistics.manufacturing.bom.group", "ja-JP", "BOM组_jp", "菜单导航"),
            // menu.logistics.manufacturing.bom.group
            ("menu.logistics.manufacturing.bom.group", "zh-CN", "BOM组", "菜单导航"),
            // menu.logistics.manufacturing.bom.group
            ("menu.logistics.manufacturing.bom.group", "zh-HK", "BOM组_hk", "菜单导航"),

            // menu.logistics.manufacturing.mds.sales.forecast
            ("menu.logistics.manufacturing.mds.sales.forecast", "en-US", "销售预测_us", "菜单导航"),
            // menu.logistics.manufacturing.mds.sales.forecast
            ("menu.logistics.manufacturing.mds.sales.forecast", "ja-JP", "销售预测_jp", "菜单导航"),
            // menu.logistics.manufacturing.mds.sales.forecast
            ("menu.logistics.manufacturing.mds.sales.forecast", "zh-CN", "销售预测", "菜单导航"),
            // menu.logistics.manufacturing.mds.sales.forecast
            ("menu.logistics.manufacturing.mds.sales.forecast", "zh-HK", "销售预测_hk", "菜单导航"),

            // menu.logistics.manufacturing.mds.master.demand.schedule
            ("menu.logistics.manufacturing.mds.master.demand.schedule", "en-US", "主需求计划_us", "菜单导航"),
            // menu.logistics.manufacturing.mds.master.demand.schedule
            ("menu.logistics.manufacturing.mds.master.demand.schedule", "ja-JP", "主需求计划_jp", "菜单导航"),
            // menu.logistics.manufacturing.mds.master.demand.schedule
            ("menu.logistics.manufacturing.mds.master.demand.schedule", "zh-CN", "主需求计划", "菜单导航"),
            // menu.logistics.manufacturing.mds.master.demand.schedule
            ("menu.logistics.manufacturing.mds.master.demand.schedule", "zh-HK", "主需求计划_hk", "菜单导航"),

            // menu.logistics.manufacturing.mrp.parameter.setting
            ("menu.logistics.manufacturing.mrp.parameter.setting", "en-US", "MRP 参数设置_us", "菜单导航"),
            // menu.logistics.manufacturing.mrp.parameter.setting
            ("menu.logistics.manufacturing.mrp.parameter.setting", "ja-JP", "MRP 参数设置_jp", "菜单导航"),
            // menu.logistics.manufacturing.mrp.parameter.setting
            ("menu.logistics.manufacturing.mrp.parameter.setting", "zh-CN", "MRP 参数设置", "菜单导航"),
            // menu.logistics.manufacturing.mrp.parameter.setting
            ("menu.logistics.manufacturing.mrp.parameter.setting", "zh-HK", "MRP 参数设置_hk", "菜单导航"),

            // menu.logistics.manufacturing.mrp.period.scheme
            ("menu.logistics.manufacturing.mrp.period.scheme", "en-US", "MRP 周期方案_us", "菜单导航"),
            // menu.logistics.manufacturing.mrp.period.scheme
            ("menu.logistics.manufacturing.mrp.period.scheme", "ja-JP", "MRP 周期方案_jp", "菜单导航"),
            // menu.logistics.manufacturing.mrp.period.scheme
            ("menu.logistics.manufacturing.mrp.period.scheme", "zh-CN", "MRP 周期方案", "菜单导航"),
            // menu.logistics.manufacturing.mrp.period.scheme
            ("menu.logistics.manufacturing.mrp.period.scheme", "zh-HK", "MRP 周期方案_hk", "菜单导航"),

            // menu.logistics.manufacturing.mrp.run.wizard
            ("menu.logistics.manufacturing.mrp.run.wizard", "en-US", "MRP运算向导_us", "菜单导航"),
            // menu.logistics.manufacturing.mrp.run.wizard
            ("menu.logistics.manufacturing.mrp.run.wizard", "ja-JP", "MRP运算向导_jp", "菜单导航"),
            // menu.logistics.manufacturing.mrp.run.wizard
            ("menu.logistics.manufacturing.mrp.run.wizard", "zh-CN", "MRP运算向导", "菜单导航"),
            // menu.logistics.manufacturing.mrp.run.wizard
            ("menu.logistics.manufacturing.mrp.run.wizard", "zh-HK", "MRP运算向导_hk", "菜单导航"),

            // menu.logistics.manufacturing.mrp.planned.order
            ("menu.logistics.manufacturing.mrp.planned.order", "en-US", "计划订单_us", "菜单导航"),
            // menu.logistics.manufacturing.mrp.planned.order
            ("menu.logistics.manufacturing.mrp.planned.order", "ja-JP", "计划订单_jp", "菜单导航"),
            // menu.logistics.manufacturing.mrp.planned.order
            ("menu.logistics.manufacturing.mrp.planned.order", "zh-CN", "计划订单", "菜单导航"),
            // menu.logistics.manufacturing.mrp.planned.order
            ("menu.logistics.manufacturing.mrp.planned.order", "zh-HK", "计划订单_hk", "菜单导航"),

            // menu.logistics.manufacturing.mrp.supply.demand.trace
            ("menu.logistics.manufacturing.mrp.supply.demand.trace", "en-US", "供需追溯_us", "菜单导航"),
            // menu.logistics.manufacturing.mrp.supply.demand.trace
            ("menu.logistics.manufacturing.mrp.supply.demand.trace", "ja-JP", "供需追溯_jp", "菜单导航"),
            // menu.logistics.manufacturing.mrp.supply.demand.trace
            ("menu.logistics.manufacturing.mrp.supply.demand.trace", "zh-CN", "供需追溯", "菜单导航"),
            // menu.logistics.manufacturing.mrp.supply.demand.trace
            ("menu.logistics.manufacturing.mrp.supply.demand.trace", "zh-HK", "供需追溯_hk", "菜单导航"),

            // menu.logistics.manufacturing.mrp.production.plan
            ("menu.logistics.manufacturing.mrp.production.plan", "en-US", "生产计划_us", "菜单导航"),
            // menu.logistics.manufacturing.mrp.production.plan
            ("menu.logistics.manufacturing.mrp.production.plan", "ja-JP", "生产计划_jp", "菜单导航"),
            // menu.logistics.manufacturing.mrp.production.plan
            ("menu.logistics.manufacturing.mrp.production.plan", "zh-CN", "生产计划", "菜单导航"),
            // menu.logistics.manufacturing.mrp.production.plan
            ("menu.logistics.manufacturing.mrp.production.plan", "zh-HK", "生产计划_hk", "菜单导航"),

            // menu.logistics.manufacturing.mrp.purchase.plan
            ("menu.logistics.manufacturing.mrp.purchase.plan", "en-US", "采购计划_us", "菜单导航"),
            // menu.logistics.manufacturing.mrp.purchase.plan
            ("menu.logistics.manufacturing.mrp.purchase.plan", "ja-JP", "采购计划_jp", "菜单导航"),
            // menu.logistics.manufacturing.mrp.purchase.plan
            ("menu.logistics.manufacturing.mrp.purchase.plan", "zh-CN", "采购计划", "菜单导航"),
            // menu.logistics.manufacturing.mrp.purchase.plan
            ("menu.logistics.manufacturing.mrp.purchase.plan", "zh-HK", "采购计划_hk", "菜单导航"),

            // menu.logistics.manufacturing.mrp.history
            ("menu.logistics.manufacturing.mrp.history", "en-US", "MRP 历史记录_us", "菜单导航"),
            // menu.logistics.manufacturing.mrp.history
            ("menu.logistics.manufacturing.mrp.history", "ja-JP", "MRP 历史记录_jp", "菜单导航"),
            // menu.logistics.manufacturing.mrp.history
            ("menu.logistics.manufacturing.mrp.history", "zh-CN", "MRP 历史记录", "菜单导航"),
            // menu.logistics.manufacturing.mrp.history
            ("menu.logistics.manufacturing.mrp.history", "zh-HK", "MRP 历史记录_hk", "菜单导航"),

            // menu.logistics.manufacturing.mps.parameter.setting
            ("menu.logistics.manufacturing.mps.parameter.setting", "en-US", "MPS 参数设置_us", "菜单导航"),
            // menu.logistics.manufacturing.mps.parameter.setting
            ("menu.logistics.manufacturing.mps.parameter.setting", "ja-JP", "MPS 参数设置_jp", "菜单导航"),
            // menu.logistics.manufacturing.mps.parameter.setting
            ("menu.logistics.manufacturing.mps.parameter.setting", "zh-CN", "MPS 参数设置", "菜单导航"),
            // menu.logistics.manufacturing.mps.parameter.setting
            ("menu.logistics.manufacturing.mps.parameter.setting", "zh-HK", "MPS 参数设置_hk", "菜单导航"),

            // menu.logistics.manufacturing.mps.period.scheme
            ("menu.logistics.manufacturing.mps.period.scheme", "en-US", "MPS 周期方案_us", "菜单导航"),
            // menu.logistics.manufacturing.mps.period.scheme
            ("menu.logistics.manufacturing.mps.period.scheme", "ja-JP", "MPS 周期方案_jp", "菜单导航"),
            // menu.logistics.manufacturing.mps.period.scheme
            ("menu.logistics.manufacturing.mps.period.scheme", "zh-CN", "MPS 周期方案", "菜单导航"),
            // menu.logistics.manufacturing.mps.period.scheme
            ("menu.logistics.manufacturing.mps.period.scheme", "zh-HK", "MPS 周期方案_hk", "菜单导航"),

            // menu.logistics.manufacturing.mps.plan.maintenance
            ("menu.logistics.manufacturing.mps.plan.maintenance", "en-US", "MPS 计划维护_us", "菜单导航"),
            // menu.logistics.manufacturing.mps.plan.maintenance
            ("menu.logistics.manufacturing.mps.plan.maintenance", "ja-JP", "MPS 计划维护_jp", "菜单导航"),
            // menu.logistics.manufacturing.mps.plan.maintenance
            ("menu.logistics.manufacturing.mps.plan.maintenance", "zh-CN", "MPS 计划维护", "菜单导航"),
            // menu.logistics.manufacturing.mps.plan.maintenance
            ("menu.logistics.manufacturing.mps.plan.maintenance", "zh-HK", "MPS 计划维护_hk", "菜单导航"),

            // menu.logistics.manufacturing.mps.run.wizard
            ("menu.logistics.manufacturing.mps.run.wizard", "en-US", "MPS运算向导_us", "菜单导航"),
            // menu.logistics.manufacturing.mps.run.wizard
            ("menu.logistics.manufacturing.mps.run.wizard", "ja-JP", "MPS运算向导_jp", "菜单导航"),
            // menu.logistics.manufacturing.mps.run.wizard
            ("menu.logistics.manufacturing.mps.run.wizard", "zh-CN", "MPS运算向导", "菜单导航"),
            // menu.logistics.manufacturing.mps.run.wizard
            ("menu.logistics.manufacturing.mps.run.wizard", "zh-HK", "MPS运算向导_hk", "菜单导航"),

            // menu.logistics.manufacturing.mps.rough.cut.capacity
            ("menu.logistics.manufacturing.mps.rough.cut.capacity", "en-US", "粗能力计划_us", "菜单导航"),
            // menu.logistics.manufacturing.mps.rough.cut.capacity
            ("menu.logistics.manufacturing.mps.rough.cut.capacity", "ja-JP", "粗能力计划_jp", "菜单导航"),
            // menu.logistics.manufacturing.mps.rough.cut.capacity
            ("menu.logistics.manufacturing.mps.rough.cut.capacity", "zh-CN", "粗能力计划", "菜单导航"),
            // menu.logistics.manufacturing.mps.rough.cut.capacity
            ("menu.logistics.manufacturing.mps.rough.cut.capacity", "zh-HK", "粗能力计划_hk", "菜单导航"),

            // menu.logistics.manufacturing.mps.detail
            ("menu.logistics.manufacturing.mps.detail", "en-US", "MPS 明细_us", "菜单导航"),
            // menu.logistics.manufacturing.mps.detail
            ("menu.logistics.manufacturing.mps.detail", "ja-JP", "MPS 明细_jp", "菜单导航"),
            // menu.logistics.manufacturing.mps.detail
            ("menu.logistics.manufacturing.mps.detail", "zh-CN", "MPS 明细", "菜单导航"),
            // menu.logistics.manufacturing.mps.detail
            ("menu.logistics.manufacturing.mps.detail", "zh-HK", "MPS 明细_hk", "菜单导航"),

            // menu.logistics.manufacturing.mps.release
            ("menu.logistics.manufacturing.mps.release", "en-US", "MPS 下达_us", "菜单导航"),
            // menu.logistics.manufacturing.mps.release
            ("menu.logistics.manufacturing.mps.release", "ja-JP", "MPS 下达_jp", "菜单导航"),
            // menu.logistics.manufacturing.mps.release
            ("menu.logistics.manufacturing.mps.release", "zh-CN", "MPS 下达", "菜单导航"),
            // menu.logistics.manufacturing.mps.release
            ("menu.logistics.manufacturing.mps.release", "zh-HK", "MPS 下达_hk", "菜单导航"),

            // menu.logistics.manufacturing.mps.production.team
            ("menu.logistics.manufacturing.mps.production.team", "en-US", "生产班组_us", "菜单导航"),
            // menu.logistics.manufacturing.mps.production.team
            ("menu.logistics.manufacturing.mps.production.team", "ja-JP", "生产班组_jp", "菜单导航"),
            // menu.logistics.manufacturing.mps.production.team
            ("menu.logistics.manufacturing.mps.production.team", "zh-CN", "生产班组", "菜单导航"),
            // menu.logistics.manufacturing.mps.production.team
            ("menu.logistics.manufacturing.mps.production.team", "zh-HK", "生产班组_hk", "菜单导航"),

            // menu.logistics.manufacturing.mps.standard.operation.rate
            ("menu.logistics.manufacturing.mps.standard.operation.rate", "en-US", "标准稼动率_us", "菜单导航"),
            // menu.logistics.manufacturing.mps.standard.operation.rate
            ("menu.logistics.manufacturing.mps.standard.operation.rate", "ja-JP", "标准稼动率_jp", "菜单导航"),
            // menu.logistics.manufacturing.mps.standard.operation.rate
            ("menu.logistics.manufacturing.mps.standard.operation.rate", "zh-CN", "标准稼动率", "菜单导航"),
            // menu.logistics.manufacturing.mps.standard.operation.rate
            ("menu.logistics.manufacturing.mps.standard.operation.rate", "zh-HK", "标准稼动率_hk", "菜单导航"),

            // menu.logistics.manufacturing.mps.personnel.operation.rate
            ("menu.logistics.manufacturing.mps.personnel.operation.rate", "en-US", "人员稼动率_us", "菜单导航"),
            // menu.logistics.manufacturing.mps.personnel.operation.rate
            ("menu.logistics.manufacturing.mps.personnel.operation.rate", "ja-JP", "人员稼动率_jp", "菜单导航"),
            // menu.logistics.manufacturing.mps.personnel.operation.rate
            ("menu.logistics.manufacturing.mps.personnel.operation.rate", "zh-CN", "人员稼动率", "菜单导航"),
            // menu.logistics.manufacturing.mps.personnel.operation.rate
            ("menu.logistics.manufacturing.mps.personnel.operation.rate", "zh-HK", "人员稼动率_hk", "菜单导航"),

            // menu.logistics.manufacturing.mps.equipment.operation.rate
            ("menu.logistics.manufacturing.mps.equipment.operation.rate", "en-US", "设备稼动率_us", "菜单导航"),
            // menu.logistics.manufacturing.mps.equipment.operation.rate
            ("menu.logistics.manufacturing.mps.equipment.operation.rate", "ja-JP", "设备稼动率_jp", "菜单导航"),
            // menu.logistics.manufacturing.mps.equipment.operation.rate
            ("menu.logistics.manufacturing.mps.equipment.operation.rate", "zh-CN", "设备稼动率", "菜单导航"),
            // menu.logistics.manufacturing.mps.equipment.operation.rate
            ("menu.logistics.manufacturing.mps.equipment.operation.rate", "zh-HK", "设备稼动率_hk", "菜单导航"),

            // menu.logistics.manufacturing.mps.production.equipment
            ("menu.logistics.manufacturing.mps.production.equipment", "en-US", "生产设备_us", "菜单导航"),
            // menu.logistics.manufacturing.mps.production.equipment
            ("menu.logistics.manufacturing.mps.production.equipment", "ja-JP", "生产设备_jp", "菜单导航"),
            // menu.logistics.manufacturing.mps.production.equipment
            ("menu.logistics.manufacturing.mps.production.equipment", "zh-CN", "生产设备", "菜单导航"),
            // menu.logistics.manufacturing.mps.production.equipment
            ("menu.logistics.manufacturing.mps.production.equipment", "zh-HK", "生产设备_hk", "菜单导航"),

            // menu.logistics.manufacturing.aps.parameter.setting
            ("menu.logistics.manufacturing.aps.parameter.setting", "en-US", "APS 参数设置_us", "菜单导航"),
            // menu.logistics.manufacturing.aps.parameter.setting
            ("menu.logistics.manufacturing.aps.parameter.setting", "ja-JP", "APS 参数设置_jp", "菜单导航"),
            // menu.logistics.manufacturing.aps.parameter.setting
            ("menu.logistics.manufacturing.aps.parameter.setting", "zh-CN", "APS 参数设置", "菜单导航"),
            // menu.logistics.manufacturing.aps.parameter.setting
            ("menu.logistics.manufacturing.aps.parameter.setting", "zh-HK", "APS 参数设置_hk", "菜单导航"),

            // menu.logistics.manufacturing.aps.schedule.rule
            ("menu.logistics.manufacturing.aps.schedule.rule", "en-US", "排程规则_us", "菜单导航"),
            // menu.logistics.manufacturing.aps.schedule.rule
            ("menu.logistics.manufacturing.aps.schedule.rule", "ja-JP", "排程规则_jp", "菜单导航"),
            // menu.logistics.manufacturing.aps.schedule.rule
            ("menu.logistics.manufacturing.aps.schedule.rule", "zh-CN", "排程规则", "菜单导航"),
            // menu.logistics.manufacturing.aps.schedule.rule
            ("menu.logistics.manufacturing.aps.schedule.rule", "zh-HK", "排程规则_hk", "菜单导航"),

            // menu.logistics.manufacturing.aps.advanced.schedule
            ("menu.logistics.manufacturing.aps.advanced.schedule", "en-US", "高级排程_us", "菜单导航"),
            // menu.logistics.manufacturing.aps.advanced.schedule
            ("menu.logistics.manufacturing.aps.advanced.schedule", "ja-JP", "高级排程_jp", "菜单导航"),
            // menu.logistics.manufacturing.aps.advanced.schedule
            ("menu.logistics.manufacturing.aps.advanced.schedule", "zh-CN", "高级排程", "菜单导航"),
            // menu.logistics.manufacturing.aps.advanced.schedule
            ("menu.logistics.manufacturing.aps.advanced.schedule", "zh-HK", "高级排程_hk", "菜单导航"),

            // menu.logistics.manufacturing.aps.resource.load
            ("menu.logistics.manufacturing.aps.resource.load", "en-US", "资源负载_us", "菜单导航"),
            // menu.logistics.manufacturing.aps.resource.load
            ("menu.logistics.manufacturing.aps.resource.load", "ja-JP", "资源负载_jp", "菜单导航"),
            // menu.logistics.manufacturing.aps.resource.load
            ("menu.logistics.manufacturing.aps.resource.load", "zh-CN", "资源负载", "菜单导航"),
            // menu.logistics.manufacturing.aps.resource.load
            ("menu.logistics.manufacturing.aps.resource.load", "zh-HK", "资源负载_hk", "菜单导航"),

            // menu.logistics.manufacturing.aps.order.split.merge
            ("menu.logistics.manufacturing.aps.order.split.merge", "en-US", "订单拆分与合并_us", "菜单导航"),
            // menu.logistics.manufacturing.aps.order.split.merge
            ("menu.logistics.manufacturing.aps.order.split.merge", "ja-JP", "订单拆分与合并_jp", "菜单导航"),
            // menu.logistics.manufacturing.aps.order.split.merge
            ("menu.logistics.manufacturing.aps.order.split.merge", "zh-CN", "订单拆分与合并", "菜单导航"),
            // menu.logistics.manufacturing.aps.order.split.merge
            ("menu.logistics.manufacturing.aps.order.split.merge", "zh-HK", "订单拆分与合并_hk", "菜单导航"),

            // menu.logistics.manufacturing.aps.production.order
            ("menu.logistics.manufacturing.aps.production.order", "en-US", "生产工单_us", "菜单导航"),
            // menu.logistics.manufacturing.aps.production.order
            ("menu.logistics.manufacturing.aps.production.order", "ja-JP", "生产工单_jp", "菜单导航"),
            // menu.logistics.manufacturing.aps.production.order
            ("menu.logistics.manufacturing.aps.production.order", "zh-CN", "生产工单", "菜单导航"),
            // menu.logistics.manufacturing.aps.production.order
            ("menu.logistics.manufacturing.aps.production.order", "zh-HK", "生产工单_hk", "菜单导航"),

            // menu.logistics.manufacturing.aps.group
            ("menu.logistics.manufacturing.aps.group", "en-US", "APS组_us", "菜单导航"),
            // menu.logistics.manufacturing.aps.group
            ("menu.logistics.manufacturing.aps.group", "ja-JP", "APS组_jp", "菜单导航"),
            // menu.logistics.manufacturing.aps.group
            ("menu.logistics.manufacturing.aps.group", "zh-CN", "APS组", "菜单导航"),
            // menu.logistics.manufacturing.aps.group
            ("menu.logistics.manufacturing.aps.group", "zh-HK", "APS组_hk", "菜单导航"),

            // menu.logistics.manufacturing.engineering.change.kanban
            ("menu.logistics.manufacturing.engineering.change.kanban", "en-US", "设变看板_us", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.kanban
            ("menu.logistics.manufacturing.engineering.change.kanban", "ja-JP", "设变看板_jp", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.kanban
            ("menu.logistics.manufacturing.engineering.change.kanban", "zh-CN", "设变看板", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.kanban
            ("menu.logistics.manufacturing.engineering.change.kanban", "zh-HK", "设变看板_hk", "菜单导航"),

            // menu.logistics.manufacturing.engineering.change.batch
            ("menu.logistics.manufacturing.engineering.change.batch", "en-US", "投入批次_us", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.batch
            ("menu.logistics.manufacturing.engineering.change.batch", "ja-JP", "投入批次_jp", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.batch
            ("menu.logistics.manufacturing.engineering.change.batch", "zh-CN", "投入批次", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.batch
            ("menu.logistics.manufacturing.engineering.change.batch", "zh-HK", "投入批次_hk", "菜单导航"),

            // menu.logistics.manufacturing.engineering.change.kakunin
            ("menu.logistics.manufacturing.engineering.change.kakunin", "en-US", "物料确认_us", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.kakunin
            ("menu.logistics.manufacturing.engineering.change.kakunin", "ja-JP", "物料确认_jp", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.kakunin
            ("menu.logistics.manufacturing.engineering.change.kakunin", "zh-CN", "物料确认", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.kakunin
            ("menu.logistics.manufacturing.engineering.change.kakunin", "zh-HK", "物料确认_hk", "菜单导航"),

            // menu.logistics.manufacturing.engineering.change.gijutsu
            ("menu.logistics.manufacturing.engineering.change.gijutsu", "en-US", "技术部门_us", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.gijutsu
            ("menu.logistics.manufacturing.engineering.change.gijutsu", "ja-JP", "技术部门_jp", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.gijutsu
            ("menu.logistics.manufacturing.engineering.change.gijutsu", "zh-CN", "技术部门", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.gijutsu
            ("menu.logistics.manufacturing.engineering.change.gijutsu", "zh-HK", "技术部门_hk", "菜单导航"),

            // menu.logistics.manufacturing.engineering.change.koubai
            ("menu.logistics.manufacturing.engineering.change.koubai", "en-US", "采购部门_us", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.koubai
            ("menu.logistics.manufacturing.engineering.change.koubai", "ja-JP", "采购部门_jp", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.koubai
            ("menu.logistics.manufacturing.engineering.change.koubai", "zh-CN", "采购部门", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.koubai
            ("menu.logistics.manufacturing.engineering.change.koubai", "zh-HK", "采购部门_hk", "菜单导航"),

            // menu.logistics.manufacturing.engineering.change.seikan
            ("menu.logistics.manufacturing.engineering.change.seikan", "en-US", "生管部门_us", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.seikan
            ("menu.logistics.manufacturing.engineering.change.seikan", "ja-JP", "生管部门_jp", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.seikan
            ("menu.logistics.manufacturing.engineering.change.seikan", "zh-CN", "生管部门", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.seikan
            ("menu.logistics.manufacturing.engineering.change.seikan", "zh-HK", "生管部门_hk", "菜单导航"),

            // menu.logistics.manufacturing.engineering.change.ukeken
            ("menu.logistics.manufacturing.engineering.change.ukeken", "en-US", "受检部门_us", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.ukeken
            ("menu.logistics.manufacturing.engineering.change.ukeken", "ja-JP", "受检部门_jp", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.ukeken
            ("menu.logistics.manufacturing.engineering.change.ukeken", "zh-CN", "受检部门", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.ukeken
            ("menu.logistics.manufacturing.engineering.change.ukeken", "zh-HK", "受检部门_hk", "菜单导航"),

            // menu.logistics.manufacturing.engineering.change.bukan
            ("menu.logistics.manufacturing.engineering.change.bukan", "en-US", "部管部门_us", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.bukan
            ("menu.logistics.manufacturing.engineering.change.bukan", "ja-JP", "部管部门_jp", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.bukan
            ("menu.logistics.manufacturing.engineering.change.bukan", "zh-CN", "部管部门", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.bukan
            ("menu.logistics.manufacturing.engineering.change.bukan", "zh-HK", "部管部门_hk", "菜单导航"),

            // menu.logistics.manufacturing.engineering.change.emtl
            ("menu.logistics.manufacturing.engineering.change.emtl", "en-US", "电子物料_us", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.emtl
            ("menu.logistics.manufacturing.engineering.change.emtl", "ja-JP", "电子物料_jp", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.emtl
            ("menu.logistics.manufacturing.engineering.change.emtl", "zh-CN", "电子物料", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.emtl
            ("menu.logistics.manufacturing.engineering.change.emtl", "zh-HK", "电子物料_hk", "菜单导航"),

            // menu.logistics.manufacturing.engineering.change.seizounika
            ("menu.logistics.manufacturing.engineering.change.seizounika", "en-US", "制造二课_us", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.seizounika
            ("menu.logistics.manufacturing.engineering.change.seizounika", "ja-JP", "制造二课_jp", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.seizounika
            ("menu.logistics.manufacturing.engineering.change.seizounika", "zh-CN", "制造二课", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.seizounika
            ("menu.logistics.manufacturing.engineering.change.seizounika", "zh-HK", "制造二课_hk", "菜单导航"),

            // menu.logistics.manufacturing.engineering.change.seizouikka
            ("menu.logistics.manufacturing.engineering.change.seizouikka", "en-US", "制造一课_us", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.seizouikka
            ("menu.logistics.manufacturing.engineering.change.seizouikka", "ja-JP", "制造一课_jp", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.seizouikka
            ("menu.logistics.manufacturing.engineering.change.seizouikka", "zh-CN", "制造一课", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.seizouikka
            ("menu.logistics.manufacturing.engineering.change.seizouikka", "zh-HK", "制造一课_hk", "菜单导航"),

            // menu.logistics.manufacturing.engineering.change.hinkan
            ("menu.logistics.manufacturing.engineering.change.hinkan", "en-US", "品管部门_us", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.hinkan
            ("menu.logistics.manufacturing.engineering.change.hinkan", "ja-JP", "品管部门_jp", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.hinkan
            ("menu.logistics.manufacturing.engineering.change.hinkan", "zh-CN", "品管部门", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.hinkan
            ("menu.logistics.manufacturing.engineering.change.hinkan", "zh-HK", "品管部门_hk", "菜单导航"),

            // menu.logistics.manufacturing.engineering.change.seizougijutsu
            ("menu.logistics.manufacturing.engineering.change.seizougijutsu", "en-US", "制造技术课_us", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.seizougijutsu
            ("menu.logistics.manufacturing.engineering.change.seizougijutsu", "ja-JP", "制造技术课_jp", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.seizougijutsu
            ("menu.logistics.manufacturing.engineering.change.seizougijutsu", "zh-CN", "制造技术课", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.seizougijutsu
            ("menu.logistics.manufacturing.engineering.change.seizougijutsu", "zh-HK", "制造技术课_hk", "菜单导航"),

            // menu.logistics.manufacturing.engineering.change.notification
            ("menu.logistics.manufacturing.engineering.change.notification", "en-US", "设变通知_us", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.notification
            ("menu.logistics.manufacturing.engineering.change.notification", "ja-JP", "设变通知_jp", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.notification
            ("menu.logistics.manufacturing.engineering.change.notification", "zh-CN", "设变通知", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.notification
            ("menu.logistics.manufacturing.engineering.change.notification", "zh-HK", "设变通知_hk", "菜单导航"),

            // menu.logistics.manufacturing.engineering.change.legacy.product
            ("menu.logistics.manufacturing.engineering.change.legacy.product", "en-US", "旧品管制_us", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.legacy.product
            ("menu.logistics.manufacturing.engineering.change.legacy.product", "ja-JP", "旧品管制_jp", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.legacy.product
            ("menu.logistics.manufacturing.engineering.change.legacy.product", "zh-CN", "旧品管制", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.legacy.product
            ("menu.logistics.manufacturing.engineering.change.legacy.product", "zh-HK", "旧品管制_hk", "菜单导航"),

            // menu.logistics.manufacturing.engineering.change.source.ec
            ("menu.logistics.manufacturing.engineering.change.source.ec", "en-US", "设变来源_us", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.source.ec
            ("menu.logistics.manufacturing.engineering.change.source.ec", "ja-JP", "设变来源_jp", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.source.ec
            ("menu.logistics.manufacturing.engineering.change.source.ec", "zh-CN", "设变来源", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.source.ec
            ("menu.logistics.manufacturing.engineering.change.source.ec", "zh-HK", "设变来源_hk", "菜单导航"),

            // menu.logistics.manufacturing.engineering.change.monthly.trend
            ("menu.logistics.manufacturing.engineering.change.monthly.trend", "en-US", "月设变推移_us", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.monthly.trend
            ("menu.logistics.manufacturing.engineering.change.monthly.trend", "ja-JP", "月设变推移_jp", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.monthly.trend
            ("menu.logistics.manufacturing.engineering.change.monthly.trend", "zh-CN", "月设变推移", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.monthly.trend
            ("menu.logistics.manufacturing.engineering.change.monthly.trend", "zh-HK", "月设变推移_hk", "菜单导航"),

            // menu.logistics.manufacturing.engineering.change.ec.group
            ("menu.logistics.manufacturing.engineering.change.ec.group", "en-US", "设变组_us", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.ec.group
            ("menu.logistics.manufacturing.engineering.change.ec.group", "ja-JP", "设变组_jp", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.ec.group
            ("menu.logistics.manufacturing.engineering.change.ec.group", "zh-CN", "设变组", "菜单导航"),
            // menu.logistics.manufacturing.engineering.change.ec.group
            ("menu.logistics.manufacturing.engineering.change.ec.group", "zh-HK", "设变组_hk", "菜单导航"),

            // menu.logistics.manufacturing.output.pcba
            ("menu.logistics.manufacturing.output.pcba", "en-US", "PCBA日报_us", "菜单导航"),
            // menu.logistics.manufacturing.output.pcba
            ("menu.logistics.manufacturing.output.pcba", "ja-JP", "PCBA日报_jp", "菜单导航"),
            // menu.logistics.manufacturing.output.pcba
            ("menu.logistics.manufacturing.output.pcba", "zh-CN", "PCBA日报", "菜单导航"),
            // menu.logistics.manufacturing.output.pcba
            ("menu.logistics.manufacturing.output.pcba", "zh-HK", "PCBA日报_hk", "菜单导航"),

            // menu.logistics.manufacturing.output.assy
            ("menu.logistics.manufacturing.output.assy", "en-US", "组立日报_us", "菜单导航"),
            // menu.logistics.manufacturing.output.assy
            ("menu.logistics.manufacturing.output.assy", "ja-JP", "组立日报_jp", "菜单导航"),
            // menu.logistics.manufacturing.output.assy
            ("menu.logistics.manufacturing.output.assy", "zh-CN", "组立日报", "菜单导航"),
            // menu.logistics.manufacturing.output.assy
            ("menu.logistics.manufacturing.output.assy", "zh-HK", "组立日报_hk", "菜单导航"),

            // menu.logistics.manufacturing.output.production.changeover
            ("menu.logistics.manufacturing.output.production.changeover", "en-US", "生产切换_us", "菜单导航"),
            // menu.logistics.manufacturing.output.production.changeover
            ("menu.logistics.manufacturing.output.production.changeover", "ja-JP", "生产切换_jp", "菜单导航"),
            // menu.logistics.manufacturing.output.production.changeover
            ("menu.logistics.manufacturing.output.production.changeover", "zh-CN", "生产切换", "菜单导航"),
            // menu.logistics.manufacturing.output.production.changeover
            ("menu.logistics.manufacturing.output.production.changeover", "zh-HK", "生产切换_hk", "菜单导航"),

            // menu.logistics.manufacturing.output.production.monthly
            ("menu.logistics.manufacturing.output.production.monthly", "en-US", "月生产推移_us", "菜单导航"),
            // menu.logistics.manufacturing.output.production.monthly
            ("menu.logistics.manufacturing.output.production.monthly", "ja-JP", "月生产推移_jp", "菜单导航"),
            // menu.logistics.manufacturing.output.production.monthly
            ("menu.logistics.manufacturing.output.production.monthly", "zh-CN", "月生产推移", "菜单导航"),
            // menu.logistics.manufacturing.output.production.monthly
            ("menu.logistics.manufacturing.output.production.monthly", "zh-HK", "月生产推移_hk", "菜单导航"),

            // menu.logistics.manufacturing.labor.hour.assy
            ("menu.logistics.manufacturing.labor.hour.assy", "en-US", "组立工数_us", "菜单导航"),
            // menu.logistics.manufacturing.labor.hour.assy
            ("menu.logistics.manufacturing.labor.hour.assy", "ja-JP", "组立工数_jp", "菜单导航"),
            // menu.logistics.manufacturing.labor.hour.assy
            ("menu.logistics.manufacturing.labor.hour.assy", "zh-CN", "组立工数", "菜单导航"),
            // menu.logistics.manufacturing.labor.hour.assy
            ("menu.logistics.manufacturing.labor.hour.assy", "zh-HK", "组立工数_hk", "菜单导航"),

            // menu.logistics.manufacturing.labor.hour.pcba.ai
            ("menu.logistics.manufacturing.labor.hour.pcba.ai", "en-US", "自插工数_us", "菜单导航"),
            // menu.logistics.manufacturing.labor.hour.pcba.ai
            ("menu.logistics.manufacturing.labor.hour.pcba.ai", "ja-JP", "自插工数_jp", "菜单导航"),
            // menu.logistics.manufacturing.labor.hour.pcba.ai
            ("menu.logistics.manufacturing.labor.hour.pcba.ai", "zh-CN", "自插工数", "菜单导航"),
            // menu.logistics.manufacturing.labor.hour.pcba.ai
            ("menu.logistics.manufacturing.labor.hour.pcba.ai", "zh-HK", "自插工数_hk", "菜单导航"),

            // menu.logistics.manufacturing.labor.hour.pcba.mi
            ("menu.logistics.manufacturing.labor.hour.pcba.mi", "en-US", "手插工数_us", "菜单导航"),
            // menu.logistics.manufacturing.labor.hour.pcba.mi
            ("menu.logistics.manufacturing.labor.hour.pcba.mi", "ja-JP", "手插工数_jp", "菜单导航"),
            // menu.logistics.manufacturing.labor.hour.pcba.mi
            ("menu.logistics.manufacturing.labor.hour.pcba.mi", "zh-CN", "手插工数", "菜单导航"),
            // menu.logistics.manufacturing.labor.hour.pcba.mi
            ("menu.logistics.manufacturing.labor.hour.pcba.mi", "zh-HK", "手插工数_hk", "菜单导航"),

            // menu.logistics.manufacturing.labor.hour.pcba.repair
            ("menu.logistics.manufacturing.labor.hour.pcba.repair", "en-US", "改修工数_us", "菜单导航"),
            // menu.logistics.manufacturing.labor.hour.pcba.repair
            ("menu.logistics.manufacturing.labor.hour.pcba.repair", "ja-JP", "改修工数_jp", "菜单导航"),
            // menu.logistics.manufacturing.labor.hour.pcba.repair
            ("menu.logistics.manufacturing.labor.hour.pcba.repair", "zh-CN", "改修工数", "菜单导航"),
            // menu.logistics.manufacturing.labor.hour.pcba.repair
            ("menu.logistics.manufacturing.labor.hour.pcba.repair", "zh-HK", "改修工数_hk", "菜单导航"),

            // menu.logistics.manufacturing.labor.hour.pcba.smt
            ("menu.logistics.manufacturing.labor.hour.pcba.smt", "en-US", "SMT工数_us", "菜单导航"),
            // menu.logistics.manufacturing.labor.hour.pcba.smt
            ("menu.logistics.manufacturing.labor.hour.pcba.smt", "ja-JP", "SMT工数_jp", "菜单导航"),
            // menu.logistics.manufacturing.labor.hour.pcba.smt
            ("menu.logistics.manufacturing.labor.hour.pcba.smt", "zh-CN", "SMT工数", "菜单导航"),
            // menu.logistics.manufacturing.labor.hour.pcba.smt
            ("menu.logistics.manufacturing.labor.hour.pcba.smt", "zh-HK", "SMT工数_hk", "菜单导航"),

            // menu.logistics.manufacturing.defect.group
            ("menu.logistics.manufacturing.defect.group", "en-US", "不良组_us", "菜单导航"),
            // menu.logistics.manufacturing.defect.group
            ("menu.logistics.manufacturing.defect.group", "ja-JP", "不良组_jp", "菜单导航"),
            // menu.logistics.manufacturing.defect.group
            ("menu.logistics.manufacturing.defect.group", "zh-CN", "不良组", "菜单导航"),
            // menu.logistics.manufacturing.defect.group
            ("menu.logistics.manufacturing.defect.group", "zh-HK", "不良组_hk", "菜单导航"),

            // menu.logistics.manufacturing.defect.pcba.inspection
            ("menu.logistics.manufacturing.defect.pcba.inspection", "en-US", "SMT检查_us", "菜单导航"),
            // menu.logistics.manufacturing.defect.pcba.inspection
            ("menu.logistics.manufacturing.defect.pcba.inspection", "ja-JP", "SMT检查_jp", "菜单导航"),
            // menu.logistics.manufacturing.defect.pcba.inspection
            ("menu.logistics.manufacturing.defect.pcba.inspection", "zh-CN", "SMT检查", "菜单导航"),
            // menu.logistics.manufacturing.defect.pcba.inspection
            ("menu.logistics.manufacturing.defect.pcba.inspection", "zh-HK", "SMT检查_hk", "菜单导航"),

            // menu.logistics.manufacturing.defect.pcba.repair
            ("menu.logistics.manufacturing.defect.pcba.repair", "en-US", "PCBA修理_us", "菜单导航"),
            // menu.logistics.manufacturing.defect.pcba.repair
            ("menu.logistics.manufacturing.defect.pcba.repair", "ja-JP", "PCBA修理_jp", "菜单导航"),
            // menu.logistics.manufacturing.defect.pcba.repair
            ("menu.logistics.manufacturing.defect.pcba.repair", "zh-CN", "PCBA修理", "菜单导航"),
            // menu.logistics.manufacturing.defect.pcba.repair
            ("menu.logistics.manufacturing.defect.pcba.repair", "zh-HK", "PCBA修理_hk", "菜单导航"),

            // menu.logistics.manufacturing.defect.assy
            ("menu.logistics.manufacturing.defect.assy", "en-US", "组立不良_us", "菜单导航"),
            // menu.logistics.manufacturing.defect.assy
            ("menu.logistics.manufacturing.defect.assy", "ja-JP", "组立不良_jp", "菜单导航"),
            // menu.logistics.manufacturing.defect.assy
            ("menu.logistics.manufacturing.defect.assy", "zh-CN", "组立不良", "菜单导航"),
            // menu.logistics.manufacturing.defect.assy
            ("menu.logistics.manufacturing.defect.assy", "zh-HK", "组立不良_hk", "菜单导航"),

            // menu.logistics.manufacturing.defect.monthly
            ("menu.logistics.manufacturing.defect.monthly", "en-US", "月生产不良推移_us", "菜单导航"),
            // menu.logistics.manufacturing.defect.monthly
            ("menu.logistics.manufacturing.defect.monthly", "ja-JP", "月生产不良推移_jp", "菜单导航"),
            // menu.logistics.manufacturing.defect.monthly
            ("menu.logistics.manufacturing.defect.monthly", "zh-CN", "月生产不良推移", "菜单导航"),
            // menu.logistics.manufacturing.defect.monthly
            ("menu.logistics.manufacturing.defect.monthly", "zh-HK", "月生产不良推移_hk", "菜单导航"),

            // menu.logistics.manufacturing.sop.workstation
            ("menu.logistics.manufacturing.sop.workstation", "en-US", "工位管理_us", "菜单导航"),
            // menu.logistics.manufacturing.sop.workstation
            ("menu.logistics.manufacturing.sop.workstation", "ja-JP", "工位管理_jp", "菜单导航"),
            // menu.logistics.manufacturing.sop.workstation
            ("menu.logistics.manufacturing.sop.workstation", "zh-CN", "工位管理", "菜单导航"),
            // menu.logistics.manufacturing.sop.workstation
            ("menu.logistics.manufacturing.sop.workstation", "zh-HK", "工位管理_hk", "菜单导航"),

            // menu.logistics.manufacturing.sop.doc
            ("menu.logistics.manufacturing.sop.doc", "en-US", "SOP文档_us", "菜单导航"),
            // menu.logistics.manufacturing.sop.doc
            ("menu.logistics.manufacturing.sop.doc", "ja-JP", "SOP文档_jp", "菜单导航"),
            // menu.logistics.manufacturing.sop.doc
            ("menu.logistics.manufacturing.sop.doc", "zh-CN", "SOP文档", "菜单导航"),
            // menu.logistics.manufacturing.sop.doc
            ("menu.logistics.manufacturing.sop.doc", "zh-HK", "SOP文档_hk", "菜单导航"),

            // menu.logistics.manufacturing.sop.revision
            ("menu.logistics.manufacturing.sop.revision", "en-US", "SOP版本_us", "菜单导航"),
            // menu.logistics.manufacturing.sop.revision
            ("menu.logistics.manufacturing.sop.revision", "ja-JP", "SOP版本_jp", "菜单导航"),
            // menu.logistics.manufacturing.sop.revision
            ("menu.logistics.manufacturing.sop.revision", "zh-CN", "SOP版本", "菜单导航"),
            // menu.logistics.manufacturing.sop.revision
            ("menu.logistics.manufacturing.sop.revision", "zh-HK", "SOP版本_hk", "菜单导航"),

            // menu.logistics.manufacturing.sop.ack
            ("menu.logistics.manufacturing.sop.ack", "en-US", "版本确认_us", "菜单导航"),
            // menu.logistics.manufacturing.sop.ack
            ("menu.logistics.manufacturing.sop.ack", "ja-JP", "版本确认_jp", "菜单导航"),
            // menu.logistics.manufacturing.sop.ack
            ("menu.logistics.manufacturing.sop.ack", "zh-CN", "版本确认", "菜单导航"),
            // menu.logistics.manufacturing.sop.ack
            ("menu.logistics.manufacturing.sop.ack", "zh-HK", "版本确认_hk", "菜单导航"),

            // menu.logistics.manufacturing.sop.exec
            ("menu.logistics.manufacturing.sop.exec", "en-US", "工位执行_us", "菜单导航"),
            // menu.logistics.manufacturing.sop.exec
            ("menu.logistics.manufacturing.sop.exec", "ja-JP", "工位执行_jp", "菜单导航"),
            // menu.logistics.manufacturing.sop.exec
            ("menu.logistics.manufacturing.sop.exec", "zh-CN", "工位执行", "菜单导航"),
            // menu.logistics.manufacturing.sop.exec
            ("menu.logistics.manufacturing.sop.exec", "zh-HK", "工位执行_hk", "菜单导航"),

            // menu.logistics.manufacturing.sop.exec.scan
            ("menu.logistics.manufacturing.sop.exec.scan", "en-US", "扫码记录_us", "菜单导航"),
            // menu.logistics.manufacturing.sop.exec.scan
            ("menu.logistics.manufacturing.sop.exec.scan", "ja-JP", "扫码记录_jp", "菜单导航"),
            // menu.logistics.manufacturing.sop.exec.scan
            ("menu.logistics.manufacturing.sop.exec.scan", "zh-CN", "扫码记录", "菜单导航"),
            // menu.logistics.manufacturing.sop.exec.scan
            ("menu.logistics.manufacturing.sop.exec.scan", "zh-HK", "扫码记录_hk", "菜单导航"),

            // menu.logistics.manufacturing.sop.esd.check
            ("menu.logistics.manufacturing.sop.esd.check", "en-US", "ESD检查_us", "菜单导航"),
            // menu.logistics.manufacturing.sop.esd.check
            ("menu.logistics.manufacturing.sop.esd.check", "ja-JP", "ESD检查_jp", "菜单导航"),
            // menu.logistics.manufacturing.sop.esd.check
            ("menu.logistics.manufacturing.sop.esd.check", "zh-CN", "ESD检查", "菜单导航"),
            // menu.logistics.manufacturing.sop.esd.check
            ("menu.logistics.manufacturing.sop.esd.check", "zh-HK", "ESD检查_hk", "菜单导航"),

            // menu.logistics.manufacturing.sop.call
            ("menu.logistics.manufacturing.sop.call", "en-US", "安灯呼叫_us", "菜单导航"),
            // menu.logistics.manufacturing.sop.call
            ("menu.logistics.manufacturing.sop.call", "ja-JP", "安灯呼叫_jp", "菜单导航"),
            // menu.logistics.manufacturing.sop.call
            ("menu.logistics.manufacturing.sop.call", "zh-CN", "安灯呼叫", "菜单导航"),
            // menu.logistics.manufacturing.sop.call
            ("menu.logistics.manufacturing.sop.call", "zh-HK", "安灯呼叫_hk", "菜单导航"),

            // menu.logistics.manufacturing.sop.group
            ("menu.logistics.manufacturing.sop.group", "en-US", "SOP组_us", "菜单导航"),
            // menu.logistics.manufacturing.sop.group
            ("menu.logistics.manufacturing.sop.group", "ja-JP", "SOP组_jp", "菜单导航"),
            // menu.logistics.manufacturing.sop.group
            ("menu.logistics.manufacturing.sop.group", "zh-CN", "SOP组", "菜单导航"),
            // menu.logistics.manufacturing.sop.group
            ("menu.logistics.manufacturing.sop.group", "zh-HK", "SOP组_hk", "菜单导航"),

            // menu.logistics.quality.cost.assurance
            ("menu.logistics.quality.cost.assurance", "en-US", "品质保证_us", "菜单导航"),
            // menu.logistics.quality.cost.assurance
            ("menu.logistics.quality.cost.assurance", "ja-JP", "品质保证_jp", "菜单导航"),
            // menu.logistics.quality.cost.assurance
            ("menu.logistics.quality.cost.assurance", "zh-CN", "品质保证", "菜单导航"),
            // menu.logistics.quality.cost.assurance
            ("menu.logistics.quality.cost.assurance", "zh-HK", "品质保证_hk", "菜单导航"),

            // menu.logistics.quality.cost.issue
            ("menu.logistics.quality.cost.issue", "en-US", "品质问题_us", "菜单导航"),
            // menu.logistics.quality.cost.issue
            ("menu.logistics.quality.cost.issue", "ja-JP", "品质问题_jp", "菜单导航"),
            // menu.logistics.quality.cost.issue
            ("menu.logistics.quality.cost.issue", "zh-CN", "品质问题", "菜单导航"),
            // menu.logistics.quality.cost.issue
            ("menu.logistics.quality.cost.issue", "zh-HK", "品质问题_hk", "菜单导航"),

            // menu.logistics.quality.cost.incident
            ("menu.logistics.quality.cost.incident", "en-US", "品质事故_us", "菜单导航"),
            // menu.logistics.quality.cost.incident
            ("menu.logistics.quality.cost.incident", "ja-JP", "品质事故_jp", "菜单导航"),
            // menu.logistics.quality.cost.incident
            ("menu.logistics.quality.cost.incident", "zh-CN", "品质事故", "菜单导航"),
            // menu.logistics.quality.cost.incident
            ("menu.logistics.quality.cost.incident", "zh-HK", "品质事故_hk", "菜单导航"),

            // menu.logistics.quality.cost.trend
            ("menu.logistics.quality.cost.trend", "en-US", "质量成本推移_us", "菜单导航"),
            // menu.logistics.quality.cost.trend
            ("menu.logistics.quality.cost.trend", "ja-JP", "质量成本推移_jp", "菜单导航"),
            // menu.logistics.quality.cost.trend
            ("menu.logistics.quality.cost.trend", "zh-CN", "质量成本推移", "菜单导航"),
            // menu.logistics.quality.cost.trend
            ("menu.logistics.quality.cost.trend", "zh-HK", "质量成本推移_hk", "菜单导航"),

            // menu.logistics.quality.cost.group
            ("menu.logistics.quality.cost.group", "en-US", "成本组_us", "菜单导航"),
            // menu.logistics.quality.cost.group
            ("menu.logistics.quality.cost.group", "ja-JP", "成本组_jp", "菜单导航"),
            // menu.logistics.quality.cost.group
            ("menu.logistics.quality.cost.group", "zh-CN", "成本组", "菜单导航"),
            // menu.logistics.quality.cost.group
            ("menu.logistics.quality.cost.group", "zh-HK", "成本组_hk", "菜单导航"),

            // menu.logistics.quality.operation.group
            ("menu.logistics.quality.operation.group", "en-US", "质量组_us", "菜单导航"),
            // menu.logistics.quality.operation.group
            ("menu.logistics.quality.operation.group", "ja-JP", "质量组_jp", "菜单导航"),
            // menu.logistics.quality.operation.group
            ("menu.logistics.quality.operation.group", "zh-CN", "质量组", "菜单导航"),
            // menu.logistics.quality.operation.group
            ("menu.logistics.quality.operation.group", "zh-HK", "质量组_hk", "菜单导航"),

            // menu.logistics.quality.operation.sampling.scheme
            ("menu.logistics.quality.operation.sampling.scheme", "en-US", "抽样方案_us", "菜单导航"),
            // menu.logistics.quality.operation.sampling.scheme
            ("menu.logistics.quality.operation.sampling.scheme", "ja-JP", "抽样方案_jp", "菜单导航"),
            // menu.logistics.quality.operation.sampling.scheme
            ("menu.logistics.quality.operation.sampling.scheme", "zh-CN", "抽样方案", "菜单导航"),
            // menu.logistics.quality.operation.sampling.scheme
            ("menu.logistics.quality.operation.sampling.scheme", "zh-HK", "抽样方案_hk", "菜单导航"),

            // menu.logistics.quality.operation.inspection.standard
            ("menu.logistics.quality.operation.inspection.standard", "en-US", "检验标准_us", "菜单导航"),
            // menu.logistics.quality.operation.inspection.standard
            ("menu.logistics.quality.operation.inspection.standard", "ja-JP", "检验标准_jp", "菜单导航"),
            // menu.logistics.quality.operation.inspection.standard
            ("menu.logistics.quality.operation.inspection.standard", "zh-CN", "检验标准", "菜单导航"),
            // menu.logistics.quality.operation.inspection.standard
            ("menu.logistics.quality.operation.inspection.standard", "zh-HK", "检验标准_hk", "菜单导航"),

            // menu.logistics.quality.operation.iqc.order
            ("menu.logistics.quality.operation.iqc.order", "en-US", "进货检验_us", "菜单导航"),
            // menu.logistics.quality.operation.iqc.order
            ("menu.logistics.quality.operation.iqc.order", "ja-JP", "进货检验_jp", "菜单导航"),
            // menu.logistics.quality.operation.iqc.order
            ("menu.logistics.quality.operation.iqc.order", "zh-CN", "进货检验", "菜单导航"),
            // menu.logistics.quality.operation.iqc.order
            ("menu.logistics.quality.operation.iqc.order", "zh-HK", "进货检验_hk", "菜单导航"),

            // menu.logistics.quality.operation.iqc.trend
            ("menu.logistics.quality.operation.iqc.trend", "en-US", "进货检验推移_us", "菜单导航"),
            // menu.logistics.quality.operation.iqc.trend
            ("menu.logistics.quality.operation.iqc.trend", "ja-JP", "进货检验推移_jp", "菜单导航"),
            // menu.logistics.quality.operation.iqc.trend
            ("menu.logistics.quality.operation.iqc.trend", "zh-CN", "进货检验推移", "菜单导航"),
            // menu.logistics.quality.operation.iqc.trend
            ("menu.logistics.quality.operation.iqc.trend", "zh-HK", "进货检验推移_hk", "菜单导航"),

            // menu.logistics.quality.operation.ipqc.order
            ("menu.logistics.quality.operation.ipqc.order", "en-US", "制程检验_us", "菜单导航"),
            // menu.logistics.quality.operation.ipqc.order
            ("menu.logistics.quality.operation.ipqc.order", "ja-JP", "制程检验_jp", "菜单导航"),
            // menu.logistics.quality.operation.ipqc.order
            ("menu.logistics.quality.operation.ipqc.order", "zh-CN", "制程检验", "菜单导航"),
            // menu.logistics.quality.operation.ipqc.order
            ("menu.logistics.quality.operation.ipqc.order", "zh-HK", "制程检验_hk", "菜单导航"),

            // menu.logistics.quality.operation.ipqc.trend
            ("menu.logistics.quality.operation.ipqc.trend", "en-US", "过程质量推移_us", "菜单导航"),
            // menu.logistics.quality.operation.ipqc.trend
            ("menu.logistics.quality.operation.ipqc.trend", "ja-JP", "过程质量推移_jp", "菜单导航"),
            // menu.logistics.quality.operation.ipqc.trend
            ("menu.logistics.quality.operation.ipqc.trend", "zh-CN", "过程质量推移", "菜单导航"),
            // menu.logistics.quality.operation.ipqc.trend
            ("menu.logistics.quality.operation.ipqc.trend", "zh-HK", "过程质量推移_hk", "菜单导航"),

            // menu.logistics.quality.operation.fqc.order
            ("menu.logistics.quality.operation.fqc.order", "en-US", "入库检验_us", "菜单导航"),
            // menu.logistics.quality.operation.fqc.order
            ("menu.logistics.quality.operation.fqc.order", "ja-JP", "入库检验_jp", "菜单导航"),
            // menu.logistics.quality.operation.fqc.order
            ("menu.logistics.quality.operation.fqc.order", "zh-CN", "入库检验", "菜单导航"),
            // menu.logistics.quality.operation.fqc.order
            ("menu.logistics.quality.operation.fqc.order", "zh-HK", "入库检验_hk", "菜单导航"),

            // menu.logistics.quality.operation.fqc.trend
            ("menu.logistics.quality.operation.fqc.trend", "en-US", "成品检验推移_us", "菜单导航"),
            // menu.logistics.quality.operation.fqc.trend
            ("menu.logistics.quality.operation.fqc.trend", "ja-JP", "成品检验推移_jp", "菜单导航"),
            // menu.logistics.quality.operation.fqc.trend
            ("menu.logistics.quality.operation.fqc.trend", "zh-CN", "成品检验推移", "菜单导航"),
            // menu.logistics.quality.operation.fqc.trend
            ("menu.logistics.quality.operation.fqc.trend", "zh-HK", "成品检验推移_hk", "菜单导航"),

            // menu.logistics.quality.operation.monthly
            ("menu.logistics.quality.operation.monthly", "en-US", "品质月报_us", "菜单导航"),
            // menu.logistics.quality.operation.monthly
            ("menu.logistics.quality.operation.monthly", "ja-JP", "品质月报_jp", "菜单导航"),
            // menu.logistics.quality.operation.monthly
            ("menu.logistics.quality.operation.monthly", "zh-CN", "品质月报", "菜单导航"),
            // menu.logistics.quality.operation.monthly
            ("menu.logistics.quality.operation.monthly", "zh-HK", "品质月报_hk", "菜单导航"),

            // menu.logistics.quality.complaint.customer
            ("menu.logistics.quality.complaint.customer", "en-US", "客诉登记_us", "菜单导航"),
            // menu.logistics.quality.complaint.customer
            ("menu.logistics.quality.complaint.customer", "ja-JP", "客诉登记_jp", "菜单导航"),
            // menu.logistics.quality.complaint.customer
            ("menu.logistics.quality.complaint.customer", "zh-CN", "客诉登记", "菜单导航"),
            // menu.logistics.quality.complaint.customer
            ("menu.logistics.quality.complaint.customer", "zh-HK", "客诉登记_hk", "菜单导航"),

            // menu.logistics.quality.complaint.customer.trend
            ("menu.logistics.quality.complaint.customer.trend", "en-US", "顾客投诉推移_us", "菜单导航"),
            // menu.logistics.quality.complaint.customer.trend
            ("menu.logistics.quality.complaint.customer.trend", "ja-JP", "顾客投诉推移_jp", "菜单导航"),
            // menu.logistics.quality.complaint.customer.trend
            ("menu.logistics.quality.complaint.customer.trend", "zh-CN", "顾客投诉推移", "菜单导航"),
            // menu.logistics.quality.complaint.customer.trend
            ("menu.logistics.quality.complaint.customer.trend", "zh-HK", "顾客投诉推移_hk", "菜单导航"),

            // menu.logistics.quality.complaint.customer.complaint.handling
            ("menu.logistics.quality.complaint.customer.complaint.handling", "en-US", "客诉处理_us", "菜单导航"),
            // menu.logistics.quality.complaint.customer.complaint.handling
            ("menu.logistics.quality.complaint.customer.complaint.handling", "ja-JP", "客诉处理_jp", "菜单导航"),
            // menu.logistics.quality.complaint.customer.complaint.handling
            ("menu.logistics.quality.complaint.customer.complaint.handling", "zh-CN", "客诉处理", "菜单导航"),
            // menu.logistics.quality.complaint.customer.complaint.handling
            ("menu.logistics.quality.complaint.customer.complaint.handling", "zh-HK", "客诉处理_hk", "菜单导航"),

            // menu.logistics.quality.complaint.customer.satisfaction.survey
            ("menu.logistics.quality.complaint.customer.satisfaction.survey", "en-US", "客户满意度调查_us", "菜单导航"),
            // menu.logistics.quality.complaint.customer.satisfaction.survey
            ("menu.logistics.quality.complaint.customer.satisfaction.survey", "ja-JP", "客户满意度调查_jp", "菜单导航"),
            // menu.logistics.quality.complaint.customer.satisfaction.survey
            ("menu.logistics.quality.complaint.customer.satisfaction.survey", "zh-CN", "客户满意度调查", "菜单导航"),
            // menu.logistics.quality.complaint.customer.satisfaction.survey
            ("menu.logistics.quality.complaint.customer.satisfaction.survey", "zh-HK", "客户满意度调查_hk", "菜单导航"),

            // menu.logistics.quality.complaint.supplier.evaluation
            ("menu.logistics.quality.complaint.supplier.evaluation", "en-US", "供应商评价考核_us", "菜单导航"),
            // menu.logistics.quality.complaint.supplier.evaluation
            ("menu.logistics.quality.complaint.supplier.evaluation", "ja-JP", "供应商评价考核_jp", "菜单导航"),
            // menu.logistics.quality.complaint.supplier.evaluation
            ("menu.logistics.quality.complaint.supplier.evaluation", "zh-CN", "供应商评价考核", "菜单导航"),
            // menu.logistics.quality.complaint.supplier.evaluation
            ("menu.logistics.quality.complaint.supplier.evaluation", "zh-HK", "供应商评价考核_hk", "菜单导航"),

            // menu.logistics.quality.complaint.group
            ("menu.logistics.quality.complaint.group", "en-US", "客诉组_us", "菜单导航"),
            // menu.logistics.quality.complaint.group
            ("menu.logistics.quality.complaint.group", "ja-JP", "客诉组_jp", "菜单导航"),
            // menu.logistics.quality.complaint.group
            ("menu.logistics.quality.complaint.group", "zh-CN", "客诉组", "菜单导航"),
            // menu.logistics.quality.complaint.group
            ("menu.logistics.quality.complaint.group", "zh-HK", "客诉组_hk", "菜单导航"),
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
        translation.ResourceGroup = "Foundation";
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
    /// 翻译种子项（CultureId 由 SeedAsync 解析）
    /// </summary>
    private sealed record TranslationSeedItem(
        string I18nKey,
        string CultureCode,
        string TranslationText,
        string? ContextNote);
}
