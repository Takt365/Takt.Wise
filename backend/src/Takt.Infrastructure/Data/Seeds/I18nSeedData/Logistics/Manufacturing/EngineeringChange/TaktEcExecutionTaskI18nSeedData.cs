// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcExecutionTaskI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEcExecutionTask 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// TaktEcExecutionTask 实体国际化翻译种子（键前缀 entity.ecexecutiontask.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEcExecutionTaskI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEcExecutionTask 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 ecexecutiontask 实体翻译...", tenantCode);

        foreach (var item in GetEcExecutionTaskTranslations())
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

        TaktLogger.Information("TaktEcExecutionTask 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEcExecutionTask 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.ecexecutiontask._self / entity.ecexecutiontask.{{field}}；ResourceGroup=EngineeringChange；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEcExecutionTaskTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.ecexecutiontask._self
            new TranslationSeedItem("entity.ecexecutiontask._self", "en-US", "Ec Execution Task Information_us", "实体名称"),
            // entity.ecexecutiontask._self
            new TranslationSeedItem("entity.ecexecutiontask._self", "ja-JP", "完成时间信息_jp", "实体名称"),
            // entity.ecexecutiontask._self
            new TranslationSeedItem("entity.ecexecutiontask._self", "zh-CN", "完成时间信息", "实体名称"),
            // entity.ecexecutiontask._self
            new TranslationSeedItem("entity.ecexecutiontask._self", "zh-HK", "完成时间信息_hk", "实体名称"),

            // entity.ecexecutiontask.ecnotificationid
            new TranslationSeedItem("entity.ecexecutiontask.ecnotificationid", "en-US", "通知单ID_us", "通知单 ID"),
            // entity.ecexecutiontask.ecnotificationid
            new TranslationSeedItem("entity.ecexecutiontask.ecnotificationid", "ja-JP", "通知单ID_jp", "通知单 ID"),
            // entity.ecexecutiontask.ecnotificationid
            new TranslationSeedItem("entity.ecexecutiontask.ecnotificationid", "zh-CN", "通知单ID", "通知单 ID"),
            // entity.ecexecutiontask.ecnotificationid
            new TranslationSeedItem("entity.ecexecutiontask.ecnotificationid", "zh-HK", "通知单ID_hk", "通知单 ID"),

            // entity.ecexecutiontask.ecgijutsuid
            new TranslationSeedItem("entity.ecexecutiontask.ecgijutsuid", "en-US", "技术课主表ID_us", "技术课主表 ID（TaktEcGijutsu 主键）"),
            // entity.ecexecutiontask.ecgijutsuid
            new TranslationSeedItem("entity.ecexecutiontask.ecgijutsuid", "ja-JP", "技术课主表ID_jp", "技术课主表 ID（TaktEcGijutsu 主键）"),
            // entity.ecexecutiontask.ecgijutsuid
            new TranslationSeedItem("entity.ecexecutiontask.ecgijutsuid", "zh-CN", "技术课主表ID", "技术课主表 ID（TaktEcGijutsu 主键）"),
            // entity.ecexecutiontask.ecgijutsuid
            new TranslationSeedItem("entity.ecexecutiontask.ecgijutsuid", "zh-HK", "技术课主表ID_hk", "技术课主表 ID（TaktEcGijutsu 主键）"),

            // entity.ecexecutiontask.eccode
            new TranslationSeedItem("entity.ecexecutiontask.eccode", "en-US", "设变单号_us", "设变单号（冗余）"),
            // entity.ecexecutiontask.eccode
            new TranslationSeedItem("entity.ecexecutiontask.eccode", "ja-JP", "设变单号_jp", "设变单号（冗余）"),
            // entity.ecexecutiontask.eccode
            new TranslationSeedItem("entity.ecexecutiontask.eccode", "zh-CN", "设变单号", "设变单号（冗余）"),
            // entity.ecexecutiontask.eccode
            new TranslationSeedItem("entity.ecexecutiontask.eccode", "zh-HK", "设变单号_hk", "设变单号（冗余）"),

            // entity.ecexecutiontask.ecexecid
            new TranslationSeedItem("entity.ecexecutiontask.ecexecid", "en-US", "设变部门行ID_us", "关联设变部门行 ID（TaktEcSeikan/Mp 等 8 张部门执行表主键）"),
            // entity.ecexecutiontask.ecexecid
            new TranslationSeedItem("entity.ecexecutiontask.ecexecid", "ja-JP", "设变部门行ID_jp", "关联设变部门行 ID（TaktEcSeikan/Mp 等 8 张部门执行表主键）"),
            // entity.ecexecutiontask.ecexecid
            new TranslationSeedItem("entity.ecexecutiontask.ecexecid", "zh-CN", "设变部门行ID", "关联设变部门行 ID（TaktEcSeikan/Mp 等 8 张部门执行表主键）"),
            // entity.ecexecutiontask.ecexecid
            new TranslationSeedItem("entity.ecexecutiontask.ecexecid", "zh-HK", "设变部门行ID_hk", "关联设变部门行 ID（TaktEcSeikan/Mp 等 8 张部门执行表主键）"),

            // entity.ecexecutiontask.ecdetailid
            new TranslationSeedItem("entity.ecexecutiontask.ecdetailid", "en-US", "设变明细ID_us", "设变明细 ID（可选）"),
            // entity.ecexecutiontask.ecdetailid
            new TranslationSeedItem("entity.ecexecutiontask.ecdetailid", "ja-JP", "设变明细ID_jp", "设变明细 ID（可选）"),
            // entity.ecexecutiontask.ecdetailid
            new TranslationSeedItem("entity.ecexecutiontask.ecdetailid", "zh-CN", "设变明细ID", "设变明细 ID（可选）"),
            // entity.ecexecutiontask.ecdetailid
            new TranslationSeedItem("entity.ecexecutiontask.ecdetailid", "zh-HK", "设变明细ID_hk", "设变明细 ID（可选）"),

            // entity.ecexecutiontask.deptcode
            new TranslationSeedItem("entity.ecexecutiontask.deptcode", "en-US", "责任部门编码_us", "责任部门编码（TaktDept.DeptCode，如 D0710）"),
            // entity.ecexecutiontask.deptcode
            new TranslationSeedItem("entity.ecexecutiontask.deptcode", "ja-JP", "责任部门编码_jp", "责任部门编码（TaktDept.DeptCode，如 D0710）"),
            // entity.ecexecutiontask.deptcode
            new TranslationSeedItem("entity.ecexecutiontask.deptcode", "zh-CN", "责任部门编码", "责任部门编码（TaktDept.DeptCode，如 D0710）"),
            // entity.ecexecutiontask.deptcode
            new TranslationSeedItem("entity.ecexecutiontask.deptcode", "zh-HK", "责任部门编码_hk", "责任部门编码（TaktDept.DeptCode，如 D0710）"),

            // entity.ecexecutiontask.deptname
            new TranslationSeedItem("entity.ecexecutiontask.deptname", "en-US", "责任部门名称_us", "责任部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),
            // entity.ecexecutiontask.deptname
            new TranslationSeedItem("entity.ecexecutiontask.deptname", "ja-JP", "责任部门名称_jp", "责任部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),
            // entity.ecexecutiontask.deptname
            new TranslationSeedItem("entity.ecexecutiontask.deptname", "zh-CN", "责任部门名称", "责任部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),
            // entity.ecexecutiontask.deptname
            new TranslationSeedItem("entity.ecexecutiontask.deptname", "zh-HK", "责任部门名称_hk", "责任部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),

            // entity.ecexecutiontask.tasktitle
            new TranslationSeedItem("entity.ecexecutiontask.tasktitle", "en-US", "任务标题_us", "任务标题"),
            // entity.ecexecutiontask.tasktitle
            new TranslationSeedItem("entity.ecexecutiontask.tasktitle", "ja-JP", "任务标题_jp", "任务标题"),
            // entity.ecexecutiontask.tasktitle
            new TranslationSeedItem("entity.ecexecutiontask.tasktitle", "zh-CN", "任务标题", "任务标题"),
            // entity.ecexecutiontask.tasktitle
            new TranslationSeedItem("entity.ecexecutiontask.tasktitle", "zh-HK", "任务标题_hk", "任务标题"),

            // entity.ecexecutiontask.taskstatus
            new TranslationSeedItem("entity.ecexecutiontask.taskstatus", "en-US", "任务状态_us", "任务状态（0待执行 1执行中 2已完成 3阻塞 4超时）"),
            // entity.ecexecutiontask.taskstatus
            new TranslationSeedItem("entity.ecexecutiontask.taskstatus", "ja-JP", "任务状态_jp", "任务状态（0待执行 1执行中 2已完成 3阻塞 4超时）"),
            // entity.ecexecutiontask.taskstatus
            new TranslationSeedItem("entity.ecexecutiontask.taskstatus", "zh-CN", "任务状态", "任务状态（0待执行 1执行中 2已完成 3阻塞 4超时）"),
            // entity.ecexecutiontask.taskstatus
            new TranslationSeedItem("entity.ecexecutiontask.taskstatus", "zh-HK", "任务状态_hk", "任务状态（0待执行 1执行中 2已完成 3阻塞 4超时）"),

            // entity.ecexecutiontask.progresspercent
            new TranslationSeedItem("entity.ecexecutiontask.progresspercent", "en-US", "进度百分比_us", "进度百分比 0-100"),
            // entity.ecexecutiontask.progresspercent
            new TranslationSeedItem("entity.ecexecutiontask.progresspercent", "ja-JP", "进度百分比_jp", "进度百分比 0-100"),
            // entity.ecexecutiontask.progresspercent
            new TranslationSeedItem("entity.ecexecutiontask.progresspercent", "zh-CN", "进度百分比", "进度百分比 0-100"),
            // entity.ecexecutiontask.progresspercent
            new TranslationSeedItem("entity.ecexecutiontask.progresspercent", "zh-HK", "进度百分比_hk", "进度百分比 0-100"),

            // entity.ecexecutiontask.duedate
            new TranslationSeedItem("entity.ecexecutiontask.duedate", "en-US", "截止日期_us", "截止日期"),
            // entity.ecexecutiontask.duedate
            new TranslationSeedItem("entity.ecexecutiontask.duedate", "ja-JP", "截止日期_jp", "截止日期"),
            // entity.ecexecutiontask.duedate
            new TranslationSeedItem("entity.ecexecutiontask.duedate", "zh-CN", "截止日期", "截止日期"),
            // entity.ecexecutiontask.duedate
            new TranslationSeedItem("entity.ecexecutiontask.duedate", "zh-HK", "截止日期_hk", "截止日期"),

            // entity.ecexecutiontask.lastprogressremark
            new TranslationSeedItem("entity.ecexecutiontask.lastprogressremark", "en-US", "最近进度说明_us", "最近进度说明"),
            // entity.ecexecutiontask.lastprogressremark
            new TranslationSeedItem("entity.ecexecutiontask.lastprogressremark", "ja-JP", "最近进度说明_jp", "最近进度说明"),
            // entity.ecexecutiontask.lastprogressremark
            new TranslationSeedItem("entity.ecexecutiontask.lastprogressremark", "zh-CN", "最近进度说明", "最近进度说明"),
            // entity.ecexecutiontask.lastprogressremark
            new TranslationSeedItem("entity.ecexecutiontask.lastprogressremark", "zh-HK", "最近进度说明_hk", "最近进度说明"),

            // entity.ecexecutiontask.completedat
            new TranslationSeedItem("entity.ecexecutiontask.completedat", "en-US", "完成时间_us", "完成时间"),
            // entity.ecexecutiontask.completedat
            new TranslationSeedItem("entity.ecexecutiontask.completedat", "ja-JP", "完成时间_jp", "完成时间"),
            // entity.ecexecutiontask.completedat
            new TranslationSeedItem("entity.ecexecutiontask.completedat", "zh-CN", "完成时间", "完成时间"),
            // entity.ecexecutiontask.completedat
            new TranslationSeedItem("entity.ecexecutiontask.completedat", "zh-HK", "完成时间_hk", "完成时间"),
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
        translation.ResourceGroup = "EngineeringChange";
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
