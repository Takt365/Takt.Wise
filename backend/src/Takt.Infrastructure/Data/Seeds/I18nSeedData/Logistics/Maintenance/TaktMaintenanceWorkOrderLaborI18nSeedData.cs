// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Maintenance
// 文件名称：TaktMaintenanceWorkOrderLaborI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktMaintenanceWorkOrderLabor 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Maintenance;

/// <summary>
/// TaktMaintenanceWorkOrderLabor 实体国际化翻译种子（键前缀 entity.maintenanceworkorderlabor.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktMaintenanceWorkOrderLaborI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktMaintenanceWorkOrderLabor 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 maintenanceworkorderlabor 实体翻译...", tenantCode);

        foreach (var item in GetMaintenanceWorkOrderLaborTranslations())
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

        TaktLogger.Information("TaktMaintenanceWorkOrderLabor 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktMaintenanceWorkOrderLabor 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.maintenanceworkorderlabor._self / entity.maintenanceworkorderlabor.{{field}}；ResourceGroup=Maintenance；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetMaintenanceWorkOrderLaborTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.maintenanceworkorderlabor._self
            new TranslationSeedItem("entity.maintenanceworkorderlabor._self", "en-US", "Maintenance Work Order Labor Information_us", "实体名称"),
            // entity.maintenanceworkorderlabor._self
            new TranslationSeedItem("entity.maintenanceworkorderlabor._self", "ja-JP", "维护工单报工明细信息_jp", "实体名称"),
            // entity.maintenanceworkorderlabor._self
            new TranslationSeedItem("entity.maintenanceworkorderlabor._self", "zh-CN", "维护工单报工明细信息", "实体名称"),
            // entity.maintenanceworkorderlabor._self
            new TranslationSeedItem("entity.maintenanceworkorderlabor._self", "zh-HK", "维护工单报工明细信息_hk", "实体名称"),

            // entity.maintenanceworkorderlabor.maintenanceworkorderid
            new TranslationSeedItem("entity.maintenanceworkorderlabor.maintenanceworkorderid", "en-US", "维护工单ID_us", "维护工单ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.maintenanceworkorderlabor.maintenanceworkorderid
            new TranslationSeedItem("entity.maintenanceworkorderlabor.maintenanceworkorderid", "ja-JP", "维护工单ID_jp", "维护工单ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.maintenanceworkorderlabor.maintenanceworkorderid
            new TranslationSeedItem("entity.maintenanceworkorderlabor.maintenanceworkorderid", "zh-CN", "维护工单ID", "维护工单ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.maintenanceworkorderlabor.maintenanceworkorderid
            new TranslationSeedItem("entity.maintenanceworkorderlabor.maintenanceworkorderid", "zh-HK", "维护工单ID_hk", "维护工单ID（主子表关系，序列化为string以避免Javascript精度问题）"),

            // entity.maintenanceworkorderlabor.workordercode
            new TranslationSeedItem("entity.maintenanceworkorderlabor.workordercode", "en-US", "维护工单号_us", "维护工单号（冗余）"),
            // entity.maintenanceworkorderlabor.workordercode
            new TranslationSeedItem("entity.maintenanceworkorderlabor.workordercode", "ja-JP", "维护工单号_jp", "维护工单号（冗余）"),
            // entity.maintenanceworkorderlabor.workordercode
            new TranslationSeedItem("entity.maintenanceworkorderlabor.workordercode", "zh-CN", "维护工单号", "维护工单号（冗余）"),
            // entity.maintenanceworkorderlabor.workordercode
            new TranslationSeedItem("entity.maintenanceworkorderlabor.workordercode", "zh-HK", "维护工单号_hk", "维护工单号（冗余）"),

            // entity.maintenanceworkorderlabor.linenumber
            new TranslationSeedItem("entity.maintenanceworkorderlabor.linenumber", "en-US", "行号_us", "行号（步长10：10/20/30…）"),
            // entity.maintenanceworkorderlabor.linenumber
            new TranslationSeedItem("entity.maintenanceworkorderlabor.linenumber", "ja-JP", "行号_jp", "行号（步长10：10/20/30…）"),
            // entity.maintenanceworkorderlabor.linenumber
            new TranslationSeedItem("entity.maintenanceworkorderlabor.linenumber", "zh-CN", "行号", "行号（步长10：10/20/30…）"),
            // entity.maintenanceworkorderlabor.linenumber
            new TranslationSeedItem("entity.maintenanceworkorderlabor.linenumber", "zh-HK", "行号_hk", "行号（步长10：10/20/30…）"),

            // entity.maintenanceworkorderlabor.employeeid
            new TranslationSeedItem("entity.maintenanceworkorderlabor.employeeid", "en-US", "员工ID_us", "员工ID（序列化为string以避免Javascript精度问题）"),
            // entity.maintenanceworkorderlabor.employeeid
            new TranslationSeedItem("entity.maintenanceworkorderlabor.employeeid", "ja-JP", "员工ID_jp", "员工ID（序列化为string以避免Javascript精度问题）"),
            // entity.maintenanceworkorderlabor.employeeid
            new TranslationSeedItem("entity.maintenanceworkorderlabor.employeeid", "zh-CN", "员工ID", "员工ID（序列化为string以避免Javascript精度问题）"),
            // entity.maintenanceworkorderlabor.employeeid
            new TranslationSeedItem("entity.maintenanceworkorderlabor.employeeid", "zh-HK", "员工ID_hk", "员工ID（序列化为string以避免Javascript精度问题）"),

            // entity.maintenanceworkorderlabor.employeecode
            new TranslationSeedItem("entity.maintenanceworkorderlabor.employeecode", "en-US", "员工编码_us", "员工编码"),
            // entity.maintenanceworkorderlabor.employeecode
            new TranslationSeedItem("entity.maintenanceworkorderlabor.employeecode", "ja-JP", "员工编码_jp", "员工编码"),
            // entity.maintenanceworkorderlabor.employeecode
            new TranslationSeedItem("entity.maintenanceworkorderlabor.employeecode", "zh-CN", "员工编码", "员工编码"),
            // entity.maintenanceworkorderlabor.employeecode
            new TranslationSeedItem("entity.maintenanceworkorderlabor.employeecode", "zh-HK", "员工编码_hk", "员工编码"),

            // entity.maintenanceworkorderlabor.employeename
            new TranslationSeedItem("entity.maintenanceworkorderlabor.employeename", "en-US", "员工姓名_us", "员工姓名（冗余）"),
            // entity.maintenanceworkorderlabor.employeename
            new TranslationSeedItem("entity.maintenanceworkorderlabor.employeename", "ja-JP", "员工姓名_jp", "员工姓名（冗余）"),
            // entity.maintenanceworkorderlabor.employeename
            new TranslationSeedItem("entity.maintenanceworkorderlabor.employeename", "zh-CN", "员工姓名", "员工姓名（冗余）"),
            // entity.maintenanceworkorderlabor.employeename
            new TranslationSeedItem("entity.maintenanceworkorderlabor.employeename", "zh-HK", "员工姓名_hk", "员工姓名（冗余）"),

            // entity.maintenanceworkorderlabor.workdate
            new TranslationSeedItem("entity.maintenanceworkorderlabor.workdate", "en-US", "报工日期_us", "报工日期"),
            // entity.maintenanceworkorderlabor.workdate
            new TranslationSeedItem("entity.maintenanceworkorderlabor.workdate", "ja-JP", "报工日期_jp", "报工日期"),
            // entity.maintenanceworkorderlabor.workdate
            new TranslationSeedItem("entity.maintenanceworkorderlabor.workdate", "zh-CN", "报工日期", "报工日期"),
            // entity.maintenanceworkorderlabor.workdate
            new TranslationSeedItem("entity.maintenanceworkorderlabor.workdate", "zh-HK", "报工日期_hk", "报工日期"),

            // entity.maintenanceworkorderlabor.starttime
            new TranslationSeedItem("entity.maintenanceworkorderlabor.starttime", "en-US", "开始时间_us", "开始时间"),
            // entity.maintenanceworkorderlabor.starttime
            new TranslationSeedItem("entity.maintenanceworkorderlabor.starttime", "ja-JP", "开始时间_jp", "开始时间"),
            // entity.maintenanceworkorderlabor.starttime
            new TranslationSeedItem("entity.maintenanceworkorderlabor.starttime", "zh-CN", "开始时间", "开始时间"),
            // entity.maintenanceworkorderlabor.starttime
            new TranslationSeedItem("entity.maintenanceworkorderlabor.starttime", "zh-HK", "开始时间_hk", "开始时间"),

            // entity.maintenanceworkorderlabor.endtime
            new TranslationSeedItem("entity.maintenanceworkorderlabor.endtime", "en-US", "结束时间_us", "结束时间"),
            // entity.maintenanceworkorderlabor.endtime
            new TranslationSeedItem("entity.maintenanceworkorderlabor.endtime", "ja-JP", "结束时间_jp", "结束时间"),
            // entity.maintenanceworkorderlabor.endtime
            new TranslationSeedItem("entity.maintenanceworkorderlabor.endtime", "zh-CN", "结束时间", "结束时间"),
            // entity.maintenanceworkorderlabor.endtime
            new TranslationSeedItem("entity.maintenanceworkorderlabor.endtime", "zh-HK", "结束时间_hk", "结束时间"),

            // entity.maintenanceworkorderlabor.workhours
            new TranslationSeedItem("entity.maintenanceworkorderlabor.workhours", "en-US", "工时_us", "工时（小时）"),
            // entity.maintenanceworkorderlabor.workhours
            new TranslationSeedItem("entity.maintenanceworkorderlabor.workhours", "ja-JP", "工时_jp", "工时（小时）"),
            // entity.maintenanceworkorderlabor.workhours
            new TranslationSeedItem("entity.maintenanceworkorderlabor.workhours", "zh-CN", "工时", "工时（小时）"),
            // entity.maintenanceworkorderlabor.workhours
            new TranslationSeedItem("entity.maintenanceworkorderlabor.workhours", "zh-HK", "工时_hk", "工时（小时）"),

            // entity.maintenanceworkorderlabor.hourlyrate
            new TranslationSeedItem("entity.maintenanceworkorderlabor.hourlyrate", "en-US", "小时费率_us", "小时费率"),
            // entity.maintenanceworkorderlabor.hourlyrate
            new TranslationSeedItem("entity.maintenanceworkorderlabor.hourlyrate", "ja-JP", "小时费率_jp", "小时费率"),
            // entity.maintenanceworkorderlabor.hourlyrate
            new TranslationSeedItem("entity.maintenanceworkorderlabor.hourlyrate", "zh-CN", "小时费率", "小时费率"),
            // entity.maintenanceworkorderlabor.hourlyrate
            new TranslationSeedItem("entity.maintenanceworkorderlabor.hourlyrate", "zh-HK", "小时费率_hk", "小时费率"),

            // entity.maintenanceworkorderlabor.laborcost
            new TranslationSeedItem("entity.maintenanceworkorderlabor.laborcost", "en-US", "人工成本_us", "人工成本"),
            // entity.maintenanceworkorderlabor.laborcost
            new TranslationSeedItem("entity.maintenanceworkorderlabor.laborcost", "ja-JP", "人工成本_jp", "人工成本"),
            // entity.maintenanceworkorderlabor.laborcost
            new TranslationSeedItem("entity.maintenanceworkorderlabor.laborcost", "zh-CN", "人工成本", "人工成本"),
            // entity.maintenanceworkorderlabor.laborcost
            new TranslationSeedItem("entity.maintenanceworkorderlabor.laborcost", "zh-HK", "人工成本_hk", "人工成本"),

            // entity.maintenanceworkorderlabor.operationdescription
            new TranslationSeedItem("entity.maintenanceworkorderlabor.operationdescription", "en-US", "作业描述_us", "作业描述"),
            // entity.maintenanceworkorderlabor.operationdescription
            new TranslationSeedItem("entity.maintenanceworkorderlabor.operationdescription", "ja-JP", "作业描述_jp", "作业描述"),
            // entity.maintenanceworkorderlabor.operationdescription
            new TranslationSeedItem("entity.maintenanceworkorderlabor.operationdescription", "zh-CN", "作业描述", "作业描述"),
            // entity.maintenanceworkorderlabor.operationdescription
            new TranslationSeedItem("entity.maintenanceworkorderlabor.operationdescription", "zh-HK", "作业描述_hk", "作业描述"),

            // entity.maintenanceworkorderlabor.confirmationstatus
            new TranslationSeedItem("entity.maintenanceworkorderlabor.confirmationstatus", "en-US", "报工确认状态_us", "报工确认状态（0=待确认，1=已确认）"),
            // entity.maintenanceworkorderlabor.confirmationstatus
            new TranslationSeedItem("entity.maintenanceworkorderlabor.confirmationstatus", "ja-JP", "报工确认状态_jp", "报工确认状态（0=待确认，1=已确认）"),
            // entity.maintenanceworkorderlabor.confirmationstatus
            new TranslationSeedItem("entity.maintenanceworkorderlabor.confirmationstatus", "zh-CN", "报工确认状态", "报工确认状态（0=待确认，1=已确认）"),
            // entity.maintenanceworkorderlabor.confirmationstatus
            new TranslationSeedItem("entity.maintenanceworkorderlabor.confirmationstatus", "zh-HK", "报工确认状态_hk", "报工确认状态（0=待确认，1=已确认）"),

            // entity.maintenanceworkorderlabor.confirmedat
            new TranslationSeedItem("entity.maintenanceworkorderlabor.confirmedat", "en-US", "确认时间_us", "确认时间"),
            // entity.maintenanceworkorderlabor.confirmedat
            new TranslationSeedItem("entity.maintenanceworkorderlabor.confirmedat", "ja-JP", "确认时间_jp", "确认时间"),
            // entity.maintenanceworkorderlabor.confirmedat
            new TranslationSeedItem("entity.maintenanceworkorderlabor.confirmedat", "zh-CN", "确认时间", "确认时间"),
            // entity.maintenanceworkorderlabor.confirmedat
            new TranslationSeedItem("entity.maintenanceworkorderlabor.confirmedat", "zh-HK", "确认时间_hk", "确认时间"),

            // entity.maintenanceworkorderlabor.isobsolete
            new TranslationSeedItem("entity.maintenanceworkorderlabor.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.maintenanceworkorderlabor.isobsolete
            new TranslationSeedItem("entity.maintenanceworkorderlabor.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.maintenanceworkorderlabor.isobsolete
            new TranslationSeedItem("entity.maintenanceworkorderlabor.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.maintenanceworkorderlabor.isobsolete
            new TranslationSeedItem("entity.maintenanceworkorderlabor.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.maintenanceworkorderlabor.maintenanceworkorder
            new TranslationSeedItem("entity.maintenanceworkorderlabor.maintenanceworkorder", "en-US", "维护工单_us", "维护工单（主表）"),
            // entity.maintenanceworkorderlabor.maintenanceworkorder
            new TranslationSeedItem("entity.maintenanceworkorderlabor.maintenanceworkorder", "ja-JP", "维护工单_jp", "维护工单（主表）"),
            // entity.maintenanceworkorderlabor.maintenanceworkorder
            new TranslationSeedItem("entity.maintenanceworkorderlabor.maintenanceworkorder", "zh-CN", "维护工单", "维护工单（主表）"),
            // entity.maintenanceworkorderlabor.maintenanceworkorder
            new TranslationSeedItem("entity.maintenanceworkorderlabor.maintenanceworkorder", "zh-HK", "维护工单_hk", "维护工单（主表）"),
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
        translation.ResourceGroup = "Maintenance";
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
