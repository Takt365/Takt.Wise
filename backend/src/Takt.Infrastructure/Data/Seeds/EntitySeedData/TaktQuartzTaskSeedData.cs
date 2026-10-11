// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.EntitySeedData
// 文件名称：TaktQuartzTaskSeedData.cs
// 创建时间：2026-06-11
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz 定时任务种子（日链自动同步 + 月度以手动为主；默认暂停）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Infrastructure.Services.Logistics.Manufacturing.Bom.Quartz;
using Takt.Shared.Constants;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// Quartz 定时任务示例种子（按 Database:CompanyCodes 各公司写入；幂等：存在则更新，不存在则创建）
/// </summary>
public class TaktQuartzTaskSeedData : ITaktSeedDataCoordinator
{
    private const int TaskStatusPaused = 1;
    private const string TaskTypeAssembly = TaktConstants.QuartzTaskType.Assembly;
    private const string TaskTypeSql = TaktConstants.QuartzTaskType.Sql;
    private const string TaskTypeHttp = TaktConstants.QuartzTaskType.Http;
    private const int TriggerTypeSimple = 0;
    private const int TriggerTypeCron = 1;
    /// <summary>
    /// 日链 / 回填类：仅目标库
    /// </summary>
    private const string SyncTargetOnlyParams = "{\"targetDatabase\":\"zTakt_000_Dev\"}";
    /// <summary>
    /// zTakt_900 暂存同步：源库 + 目标库
    /// </summary>
    private const string SyncStagingParams =
        "{\"sourceDatabase\":\"zTakt_900_Dev\",\"targetDatabase\":\"zTakt_000_Dev\"}";

    /// <summary>
    /// 执行顺序（在字典数据与公司基础数据之后）
    /// </summary>
    public int Order => 56;

    /// <summary>
    /// 初始化 Quartz 定时任务示例种子
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（由协调器传入）</param>
    /// <returns>返回插入和更新的记录数（插入数, 更新数）</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(
        IServiceProvider serviceProvider,
        string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化 Quartz 定时任务示例种子数据...");
        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过 Quartz 定时任务种子数据初始化");
            return (0, 0);
        }
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var database = configuration.RequireDatabase();
        var repository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktQuartzTask>>();
        var companyRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCompany>>();
        var companies = await companyRepository.GetListAsync(
            c => c.TenantCode == tenantCode && c.CompanyStatus == 1);
        if (companies == null || companies.Count == 0)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到启用的公司，跳过 Quartz 定时任务种子", tenantCode);
            return (0, 0);
        }
        var orderedCompanies = TaktDatabaseOptions.OrderByConfiguredCodes(
            database.CompanyCodes,
            companies,
            c => c.CompanyCode);
        if (orderedCompanies.Count == 0)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到 Database:CompanyCodes 对应的公司，跳过 Quartz 定时任务种子", tenantCode);
            return (0, 0);
        }
        var healthCheckUrl = ResolveHealthCheckUrl(configuration);
        var templates = GetDemoTaskTemplates(healthCheckUrl);
        var insertCount = 0;
        var updateCount = 0;
        TaktLogger.Information("正在为租户 {TenantCode} 初始化 Quartz 定时任务示例...", tenantCode);
        foreach (var company in orderedCompanies)
        {
            foreach (var template in templates)
            {
                var (_, inserted, updated) = await CreateOrUpdateQuartzTaskAsync(
                    repository,
                    tenantCode,
                    company.CompanyCode,
                    database.GetPlantCodeForCompanyCode(company.CompanyCode),
                    company.CultureCode,
                    template);
                insertCount += inserted;
                updateCount += updated;
            }
            await RetireObsoleteSyncTasksAsync(repository, tenantCode, company.CompanyCode);
        }
        TaktLogger.Information(
            "Quartz 定时任务示例种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条",
            insertCount,
            updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// 解析本机健康检查地址（供 HTTP 示例任务使用）
    /// </summary>
    /// <param name="configuration">应用配置</param>
    /// <returns>健康检查 URL</returns>
    private static string ResolveHealthCheckUrl(IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        var urls = configuration["ASPNETCORE_URLS"];
        if (!string.IsNullOrWhiteSpace(urls))
        {
            var first = urls.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .FirstOrDefault(u => u.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                ?? urls.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(first))
            {
                return $"{first.TrimEnd('/')}/health";
            }
        }
        return "http://localhost:60070/health";
    }

    /// <summary>
    /// 获取示例任务模板（默认暂停，避免启动自动调度）
    /// </summary>
    /// <param name="healthCheckUrl">HTTP 健康检查地址</param>
    /// <returns>任务模板列表</returns>
    private static List<QuartzTaskSeedTemplate> GetDemoTaskTemplates(string healthCheckUrl)
    {
        return new List<QuartzTaskSeedTemplate>
        {
            new(
                TaskCode: "QT_DEMO_SQL",
                TaskName: "示例：只读 SQL 统计",
                JobName: "demo_sql_dict_count",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/demo_sql_dict_count.sql",
                TriggerType: TriggerTypeSimple,
                IntervalSeconds: 3600,
                CronExpression: string.Empty,
                TaskStatus: TaskStatusPaused,
                Description: "种子示例：Simple 触发器 + wwwroot/Quartz/demo_sql_dict_count.sql（默认暂停）"),
            new(
                TaskCode: "QT_DEMO_HTTP",
                TaskName: "示例：HTTP 健康检查",
                JobName: "demo_http_health",
                TaskType: TaskTypeHttp,
                ApiUrl: healthCheckUrl,
                RequestMethod: "GET",
                TriggerType: TriggerTypeSimple,
                IntervalSeconds: 1800,
                CronExpression: string.Empty,
                TaskStatus: TaskStatusPaused,
                Description: "种子示例：Simple 触发器 + GET 健康检查（默认暂停，ApiUrl 随 ASPNETCORE_URLS 解析）"),
            new(
                TaskCode: "QT_DEMO_CRON_SQL",
                TaskName: "示例：Cron SQL",
                JobName: "demo_cron_sql_ping",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/demo_cron_sql_ping.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 0 2 * * ?",
                TaskStatus: TaskStatusPaused,
                Description: "种子示例：Cron 触发器（每日 02:00）+ wwwroot/Quartz/demo_cron_sql_ping.sql（默认暂停）"),
            new(
                TaskCode: "QT_EC_TASK_OVERDUE_SCAN",
                TaskName: "工程变更执行任务超时扫描",
                JobName: "ec_task_overdue_scan",
                TaskType: TaskTypeAssembly,
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 */30 * * * ?",
                TaskStatus: TaskStatusPaused,
                AssemblyName: "Takt.Application",
                ClassName: TaktEcFlowConstants.QuartzHandlerEcTaskOverdueScan,
                Description: "扫描设变执行任务超时/阻塞并 SignalR 预警（默认暂停，启用后每 30 分钟）"),
            new(
                TaskCode: "QT_EC_BOM_DATE_REMINDER",
                TaskName: "设变BOM生效日前后30天未实施提醒",
                JobName: "ec_bom_date_reminder",
                TaskType: TaskTypeAssembly,
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 0 8 * * ?",
                TaskStatus: TaskStatusPaused,
                AssemblyName: "Takt.Application",
                ClassName: TaktEcFlowConstants.QuartzHandlerEcBomDateReminderScan,
                Description: "BOM生效日前30天、后30天，对尚未实施的部门按设变组联系邮箱再提醒（默认暂停，启用后每日 08:00）"),
            new(
                TaskCode: "QT_MEETING_REMINDER_SCAN",
                TaskName: "会议开始前提醒邮件",
                JobName: "meeting_reminder_scan",
                TaskType: TaskTypeAssembly,
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 */5 * * * ?",
                TaskStatus: TaskStatusPaused,
                AssemblyName: "Takt.Application",
                ClassName: TaktMeetingConstants.QuartzHandlerMeetingReminderScan,
                Description: "扫描已排期会议并按 ReminderMinutes 向参会人发送提醒邮件（默认暂停，启用后每 5 分钟）"),
            // ========== 日链自动同步（①→⑤；默认暂停，启用后按 Cron）==========
            new(
                TaskCode: "QT_SYNC_MATPLT",
                TaskName: "源数据同步：工厂物料",
                JobName: "sync_matplt",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_matplt.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 40 7 * * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncTargetOnlyParams,
                Description: "每日 07:40 自动同步：工厂物料（日链①；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_MDL",
                TaskName: "源数据同步：机种目的地",
                JobName: "sync_mdl",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_mdl.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 50 7 * * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncTargetOnlyParams,
                Description: "每日 07:50 自动同步：机种目的地（日链②；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_ST",
                TaskName: "源数据同步：标准工时",
                JobName: "sync_st",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_st.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 0 8 * * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncTargetOnlyParams,
                Description: "每日 08:00 自动同步：标准工时（日链③；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_EC",
                TaskName: "源数据同步：工程变更",
                JobName: "sync_ec",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_ec.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 10 8 * * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncTargetOnlyParams,
                Description: "每日 08:10 自动同步：工程变更（日链④；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_MO",
                TaskName: "源数据同步：生产工单",
                JobName: "sync_mo",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_mo.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 20 8 * * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncTargetOnlyParams,
                Description: "每日 08:20 自动同步：生产工单（日链⑤；默认暂停）"),
            // ========== 月度：源数据同步（QT_SYNC_*；以手动为主；默认暂停）==========
            // ========== 月度：目标库回填（QT_SYNC_*_BK；TaskName 前缀「回填：」；紧随对应同步）==========
            new(
                TaskCode: "QT_SYNC_MAT",
                TaskName: "源数据同步：物料主数据",
                JobName: "sync_mat",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_mat.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 0 2 3 * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncStagingParams,
                Description: "每月 3 日 02:00：物料主数据（月度；以手动为主；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_CUS",
                TaskName: "源数据同步：客户信息",
                JobName: "sync_cus",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_cus.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 10 2 3 * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncStagingParams,
                Description: "每月 3 日 02:10：客户信息（月度；以手动为主；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_SUP",
                TaskName: "源数据同步：供货商信息",
                JobName: "sync_sup",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_sup.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 20 2 3 * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncStagingParams,
                Description: "每月 3 日 02:20：供货商信息（月度；以手动为主；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_PUP",
                TaskName: "源数据同步：采购信息记录",
                JobName: "sync_pup",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_pup.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 30 2 3 * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncStagingParams,
                Description: "每月 3 日 02:30：采购信息记录（源表原样同步；空物料描述回填见 QT_SYNC_PUP_BK；月度；以手动为主；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_PUP_BK",
                TaskName: "回填：采购信息记录物料描述",
                JobName: "sync_pup_bk",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_pup_bk.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 35 2 3 * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncTargetOnlyParams,
                Description: "每月 3 日 02:35：回填采购信息记录空物料描述（已有值不覆盖；culture=ja-JP；写入 ext_field._bk.pup）；建议在 QT_SYNC_PUP 之后；默认暂停"),
            new(
                TaskCode: "QT_SYNC_SP",
                TaskName: "源数据同步：销售条件记录",
                JobName: "sync_sp",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_sp.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 40 2 3 * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncStagingParams,
                Description: "每月 3 日 02:40：销售条件记录（源表原样同步；空物料描述回填见 QT_SYNC_SP_BK；月度；以手动为主；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_SP_BK",
                TaskName: "回填：销售条件记录物料描述",
                JobName: "sync_sp_bk",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_sp_bk.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 45 2 3 * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncTargetOnlyParams,
                Description: "每月 3 日 02:45：回填销售条件记录空物料描述（已有值不覆盖；zh-CN→Z1→ja-JP；写入 ext_field._bk.sp）；建议在 QT_SYNC_SP 之后；默认暂停"),
            new(
                TaskCode: "QT_SYNC_MATDOC",
                TaskName: "源数据同步：物料凭证",
                JobName: "sync_matdoc",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_matdoc.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 50 2 3 * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncStagingParams,
                Description: "每月 3 日 02:50：物料凭证（月度；以手动为主；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_MIRO",
                TaskName: "源数据同步：采购发票",
                JobName: "sync_miro",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_miro.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 0 1 3 * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncStagingParams,
                Description: "每月 3 日 01:00：采购发票（月度；以手动为主；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_BILLING",
                TaskName: "源数据同步：销售发票",
                JobName: "sync_billing",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_billing.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 10 1 3 * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncStagingParams,
                Description: "每月 3 日 01:10：销售发票（月度；以手动为主；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_PO",
                TaskName: "源数据同步：采购订单",
                JobName: "sync_po",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_po.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 25 1 3 * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncStagingParams,
                Description: "每月 3 日 01:25：采购订单（主子表；源库回填 purchase_order_id；月度；以手动为主；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_PPH_BK",
                TaskName: "回填：采购订单明细物料描述",
                JobName: "sync_pph_bk",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_pph_bk.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 28 1 3 * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncTargetOnlyParams,
                Description: "每月 3 日 01:28：按工厂物料回填采购订单明细空物料描述（已有值不覆盖；写入 ext_field._bk.pph）；须在 QT_SYNC_PO 之后；默认暂停"),
            new(
                TaskCode: "QT_SYNC_SO",
                TaskName: "源数据同步：销售订单",
                JobName: "sync_so",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_so.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 35 1 3 * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncStagingParams,
                Description: "每月 3 日 01:35：销售订单（主子表；源库回填 sales_order_id；月度；以手动为主；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_AD",
                TaskName: "源数据同步：行政区划",
                JobName: "sync_ad",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_ad.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 0 2 1 * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncStagingParams,
                Description: "每月 1 日 02:00：行政区划（月度；以手动为主；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_MAP",
                TaskName: "源数据同步：移动价格",
                JobName: "sync_map",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_map.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 0 3 3 * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncStagingParams,
                Description: "每月 3 日 03:00：移动价格（月度；以手动为主；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_MATPKG",
                TaskName: "源数据同步：包装物料",
                JobName: "sync_matpkg",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_matpkg.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 5 3 3 * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncStagingParams,
                Description: "每月 3 日 03:05：包装物料（月度；以手动为主；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_MFRMAT",
                TaskName: "源数据同步：制造商物料",
                JobName: "sync_mfrmat",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_mfrmat.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 10 3 3 * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncStagingParams,
                Description: "每月 3 日 03:10：制造商物料（月度；以手动为主；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_DISTMAT",
                TaskName: "源数据同步：销售商物料",
                JobName: "sync_distmat",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_distmat.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 15 3 3 * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncStagingParams,
                Description: "每月 3 日 03:15：销售商物料（月度；以手动为主；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_BC",
                TaskName: "源数据同步：BOM物料成本明细",
                JobName: "sync_bc",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_bc.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 30 3 3 * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncStagingParams,
                Description: "每月 3 日 03:30：BOM物料成本明细（源表原样同步；采购价回填见 QT_SYNC_BC_BK；PCB SECT 标识回填见 QT_SYNC_BC_PCB_SECT_BK；月度；以手动为主；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_BC_BK",
                TaskName: "回填：BOM明细采购价",
                JobName: "sync_bc_bk",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_bc_bk.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 35 3 3 * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncTargetOnlyParams,
                Description: "每月 3 日 03:35：回填 BOM 明细空采购价（按核算月、每批 20000；ValidFrom≤核算日；不用未来价；不写 0；写入 ext_field._bk.bc；立即执行须选业务库+核算月，勿选暂存库；建议在 QT_SYNC_BC/PUP 之后；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_BC_PCB_SECT_BK",
                TaskName: "回填：BOM明细PCB SECT整树标识",
                JobName: "sync_bc_pcb_sect_bk",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_bc_pcb_sect_bk.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 40 3 3 * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncTargetOnlyParams,
                Description: "每月 3 日 03:40：回填 pcb_sect_indicator=X（描述含 PCB SECT 的节点及子孙；已有 X 跳过；写入 ext_field._bk.pcb_sect；对齐 LineCostHelper；立即执行须选目标库+核算月；建议在 QT_SYNC_BC 之后、成本合计之前；默认暂停）"),
            new(
                TaskCode: "QT_BOM_ZERO_PRICE_MP_BK",
                TaskName: "回填：BOM零价格移动平均价",
                JobName: "bom_zero_price_mp_bk",
                TaskType: TaskTypeAssembly,
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 42 3 3 * ?",
                TaskStatus: TaskStatusPaused,
                AssemblyName: "Takt.Infrastructure",
                ClassName: nameof(TaktBomMaterialZeroPriceMovingBackfillJobHandler),
                ExecuteParams: SyncTargetOnlyParams,
                Description: "每月 3 日 03:42：零价组件回填移动平均价（PcbSectIndicator=X 或用量≤0 永不回填；其余建议价末字母逆推；写入 ext_field._bk.mp；立即执行须选目标库+核算月；建议在 QT_SYNC_BC/MAP/PCB_SECT 之后、成本合计之前；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_BV",
                TaskName: "源数据同步：BOM物料成本汇总",
                JobName: "sync_bv",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_bv.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 45 3 3 * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncStagingParams,
                Description: "每月 3 日 03:45：BOM物料成本汇总（源表原样同步；机种/物料类型回填见 QT_SYNC_BV_BK；月度；以手动为主；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_BV_BK",
                TaskName: "回填：BOM机种与物料类型",
                JobName: "sync_bv_bk",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_bv_bk.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 50 3 3 * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncTargetOnlyParams,
                Description: "每月 3 日 03:50：先查出 BOM 成本主表/明细 product_code、明细 component_code 前导 00000000 的行并去掉这 8 位（撞唯一键跳过），再回填主表空机种/空物料类型（已有值不覆盖；写入 ext_field._bk.bv；model_destination；general_material→material_plant）；建议在 QT_SYNC_MAT/MATPLT/MDL/BV 之后；默认暂停"),
            new(
                TaskCode: "QT_BOM_MATERIAL_COST_SUM",
                TaskName: "BOM物料成本合计",
                JobName: "bom_material_cost_sum",
                TaskType: TaskTypeAssembly,
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 0 4 3 * ?",
                TaskStatus: TaskStatusPaused,
                AssemblyName: "Takt.Infrastructure",
                ClassName: nameof(TaktBomMaterialCostSumJobHandler),
                ExecuteParams: SyncTargetOnlyParams,
                Description: "每月 3 日 04:00 合计 CostingDate 当月（不回填机种/物料类型，须先 QT_SYNC_BV_BK；立即执行须选目标库+核算月；月度；以手动为主；默认暂停）"),
            new(
                TaskCode: "QT_BOM_MATERIAL_COST_RECALC",
                TaskName: "BOM物料成本重算",
                JobName: "bom_material_cost_recalc",
                TaskType: TaskTypeAssembly,
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 30 4 3 * ?",
                TaskStatus: TaskStatusPaused,
                AssemblyName: "Takt.Infrastructure",
                ClassName: nameof(TaktBomMaterialCostRecalculateJobHandler),
                ExecuteParams: SyncTargetOnlyParams,
                Description: "每月 3 日 04:30 force 重算 CostingDate 当月（立即执行须选目标库+核算月；月度；以手动为主；默认暂停）"),
            new(
                TaskCode: "QT_BOM_MODEL_AVG_COST",
                TaskName: "BOM机种平均成本",
                JobName: "bom_model_avg_cost",
                TaskType: TaskTypeAssembly,
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 0 5 3 * ?",
                TaskStatus: TaskStatusPaused,
                AssemblyName: "Takt.Infrastructure",
                ClassName: nameof(TaktBomModelAvgCostJobHandler),
                ExecuteParams: SyncTargetOnlyParams,
                Description: "每月 3 日 05:00：按工厂+物料类型+机种+月份重算机种月均（不回填机种/物料类型，须先 QT_SYNC_BV_BK；立即执行须选目标库+核算月；月度；以手动为主；默认暂停）"),
        };
    }

    /// <summary>
    /// 软删已废弃的源数据同步任务（销售预测不同步：QT_SYNC_FC / JobName=sync_fc）
    /// </summary>
    /// <param name="repository">定时任务仓储</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司代码</param>
    private static async Task RetireObsoleteSyncTasksAsync(
        ITaktCompanySeedRepository<TaktQuartzTask> repository,
        string tenantCode,
        string companyCode)
    {
        var obsolete = await repository.GetListAsync(x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.IsDeleted == 0
            && (x.TaskCode == "QT_SYNC_FC"
                || x.JobName == "sync_fc"
                || x.SqlScript == "Quartz/sync_fc.sql"));
        if (obsolete == null || obsolete.Count == 0)
        {
            return;
        }
        foreach (var task in obsolete)
        {
            task.IsDeleted = 1;
            task.DeletedAt = DateTime.Now;
            task.TaskStatus = TaskStatusPaused;
            task.TaskDescription = "已废弃：销售预测不同步（已移除 sync_fc）";
            await repository.UpdateAsync(task);
            TaktLogger.Information(
                "已软删废弃 Quartz 任务 {TaskCode}/{JobName}（Tenant={TenantCode}, Company={CompanyCode}）",
                task.TaskCode,
                task.JobName,
                tenantCode,
                companyCode);
        }
    }

    /// <summary>
    /// 创建或更新定时任务示例
    /// </summary>
    /// <param name="repository">定时任务仓储</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司代码</param>
    /// <param name="template">任务模板</param>
    /// <returns>实体与插入/更新计数</returns>
    private static async Task<(TaktQuartzTask Task, int InsertCount, int UpdateCount)> CreateOrUpdateQuartzTaskAsync(
        ITaktCompanySeedRepository<TaktQuartzTask> repository,
        string tenantCode,
        string companyCode,
        string plantCode,
        string cultureCode,
        QuartzTaskSeedTemplate template)
    {
        var entity = await repository.FirstAsync(x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.TaskCode == template.TaskCode);
        if (entity == null)
        {
            entity = new TaktQuartzTask
            {
                TenantCode = tenantCode,
                CompanyCode = companyCode,
                TaskCode = template.TaskCode,
                TaskName = template.TaskName,
                JobName = template.JobName,
                JobGroup = "DEFAULT",
                TaskType = template.TaskType,
                AssemblyName = template.AssemblyName ?? string.Empty,
                ClassName = template.ClassName ?? string.Empty,
                ApiUrl = template.ApiUrl,
                RequestMethod = template.RequestMethod,
                SqlScript = template.SqlScript,
                TriggerType = template.TriggerType,
                CronExpression = template.CronExpression,
                IntervalSeconds = template.IntervalSeconds,
                ExecuteParams = template.ExecuteParams,
                TaskStatus = template.TaskStatus,
                Concurrent = 0,
                MisfirePolicy = 0,
                TaskDescription = template.Description,
                Remark = "系统内置示例任务种子",
                PlantCode = plantCode,
                CultureCode = cultureCode
            };
            entity = await repository.CreateAsync(entity);
            return (entity, 1, 0);
        }
        // 仅刷新定义字段；TaskStatus 由管理页启停维护，禁止种子每次启动写回「暂停」导致已启用任务丢失调度
        entity.TaskName = template.TaskName;
        entity.JobName = template.JobName;
        entity.JobGroup = "DEFAULT";
        entity.TaskType = template.TaskType;
        entity.AssemblyName = template.AssemblyName ?? string.Empty;
        entity.ClassName = template.ClassName ?? string.Empty;
        entity.ApiUrl = template.ApiUrl;
        entity.RequestMethod = template.RequestMethod;
        entity.SqlScript = template.SqlScript;
        entity.TriggerType = template.TriggerType;
        entity.CronExpression = template.CronExpression;
        entity.IntervalSeconds = template.IntervalSeconds;
        entity.ExecuteParams = template.ExecuteParams;
        entity.TaskDescription = template.Description;
        entity.Remark = "系统内置示例任务种子";
        entity.PlantCode = plantCode;
        entity.CultureCode = cultureCode;
        await repository.UpdateAsync(entity);
        return (entity, 0, 1);
    }

    /// <summary>
    /// Quartz 任务种子模板
    /// </summary>
    /// <param name="TaskCode">任务编码</param>
    /// <param name="TaskName">任务名称</param>
    /// <param name="JobName">Job 名称</param>
    /// <param name="TaskType">任务类型</param>
    /// <param name="SqlScript">相对 wwwroot 的 .sql 路径（如 Quartz/sync_mat.sql；禁止内联 SQL）</param>
    /// <param name="ApiUrl">API 地址</param>
    /// <param name="RequestMethod">请求方式</param>
    /// <param name="TriggerType">触发器类型</param>
    /// <param name="IntervalSeconds">间隔秒数</param>
    /// <param name="CronExpression">Cron 表达式</param>
    /// <param name="TaskStatus">任务状态</param>
    /// <param name="Description">任务描述</param>
    /// <param name="AssemblyName">程序集名</param>
    /// <param name="ClassName">Handler 类名</param>
    /// <param name="ExecuteParams">执行参数</param>
    private sealed record QuartzTaskSeedTemplate(
        string TaskCode,
        string TaskName,
        string JobName,
        string TaskType,
        string? SqlScript = null,
        string? ApiUrl = null,
        string? RequestMethod = null,
        int TriggerType = 0,
        int IntervalSeconds = 0,
        string CronExpression = "",
        int TaskStatus = 1,
        string? Description = null,
        string? AssemblyName = null,
        string? ClassName = null,
        string? ExecuteParams = null);
}
