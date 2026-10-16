// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcScopeExecOrchestrator.cs
// 创建时间：2026-08-26
// 创建人：Takt365(Cursor AI)
// 功能描述：技术课保存/源导入后按实施范围与停产状态派生各部门执行内容（EOL 优先）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Diagnostics;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变实施范围 → 部门执行行编排（新增/更新/来源导入共用）。
/// 规则：工厂物料查不到根物料则不派生。停产与非 FERT 的冒号文案只用于全仕向、部管中仍须人工实施的部门。内部、技术以及部管下制二/制一/品管写实施范围自动文案。按机种与按设变：整组停产写 EOL，否则整组非 FERT 写物料类型。按成品：当前成品停产写 EOL，否则非 FERT 写该类型。
/// </summary>
public class TaktEcScopeExecOrchestrator
{
    private readonly TaktEcExecPersistence _ecExecPersistence;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecExecPersistence">部门执行持久化</param>
    public TaktEcScopeExecOrchestrator(TaktEcExecPersistence ecExecPersistence)
    {
        _ecExecPersistence = ecExecPersistence;
    }

    /// <summary>
    /// 按主表实施范围与明细生成或刷新各部门执行行（按部门批量落库）。
    /// </summary>
    /// <param name="gijutsu">设变技术课主</param>
    /// <param name="details">设变明细（通常已过滤作废）</param>
    /// <returns>各部门写入统计（供日志与完成通知）</returns>
    public async Task<TaktEcScopeExecApplyResult> ApplyAsync(
        TaktEcGijutsu gijutsu,
        IReadOnlyList<TaktEcDetail> details)
    {
        ArgumentNullException.ThrowIfNull(gijutsu);
        if (details == null || details.Count == 0)
        {
            return TaktEcScopeExecApplyResult.Empty;
        }
        var active = details.Where(x => x.IsObsolete == 0).ToList();
        if (active.Count == 0)
        {
            return TaktEcScopeExecApplyResult.Empty;
        }

        var ecCode = gijutsu.EcCode?.Trim() ?? string.Empty;
        var needPerDetail = active.Count;
        var deptCount = TaktEcDeptCodes.KanbanOrder.Length;
        var needTotalMax = checked(needPerDetail * deptCount);
        TaktLogger.Information(
            "[EcGijutsuPersist] 派生计划 EcCode={EcCode} 明细数={DetailCount} 部门数={DeptCount} 最大待处理={NeedTotal}（每部门需处理明细={NeedPerDept}）",
            ecCode,
            needPerDetail,
            deptCount,
            needTotalMax,
            needPerDetail);

        var counts = new Dictionary<string, int>(StringComparer.Ordinal);
        var grandCompleted = 0;
        var grandSkipped = 0;
        var totalSw = Stopwatch.StartNew();
        foreach (var deptCode in TaktEcDeptCodes.KanbanOrder)
        {
            // 电子物料 D0625 不单独走单表开关：与制造2课 D0620 同一次双表写入（F+C003 → TaktEcEmtl）
            if (deptCode == TaktEcDeptCodes.Smt)
            {
                continue;
            }
            var deptName = TaktEcDeptCodes.GetDisplayName(deptCode);
            var needCount = needPerDetail;
            TaktLogger.Information(
                "[EcGijutsuPersist] {DeptName}开始派生 需要记录数={NeedCount} EcCode={EcCode} DeptCode={DeptCode}",
                deptName,
                needCount,
                ecCode,
                deptCode);

            var deptSw = Stopwatch.StartNew();
            var batch = await _ecExecPersistence.UpsertDeptExecBatchWithFillModeAsync(
                active,
                deptCode,
                detail => TaktEcScopeConstants.ShouldAutoCompleteExec(
                    gijutsu.EcScope,
                    deptCode,
                    detail.EcNewPurchaseType,
                    detail.EcNewWarehouse),
                gijutsu.EcScope);
            deptSw.Stop();

            grandCompleted += batch.SavedCount;
            grandSkipped += batch.SkippedCount;
            if (batch.SavedCount > 0)
            {
                counts[deptCode] = batch.SavedCount;
            }
            TaktLogger.Information(
                "[EcGijutsuPersist] {DeptName}完成 需要记录数={NeedCount} 完成记录数={SavedCount} 跳过={SkippedCount} 耗时={ElapsedMs}ms EcCode={EcCode} DeptCode={DeptCode}",
                deptName,
                needCount,
                batch.SavedCount,
                batch.SkippedCount,
                deptSw.ElapsedMilliseconds,
                ecCode,
                deptCode);
        }
        totalSw.Stop();

        var result = TaktEcScopeExecApplyResult.FromCounts(counts);
        TaktLogger.Information(
            "[EcGijutsuPersist] 各部门执行行派生结束 EcCode={EcCode} 需要合计(明细×部门)={NeedTotal} 完成合计={Completed} 跳过合计={Skipped} 耗时={ElapsedMs}ms Summary={Summary}",
            ecCode,
            needTotalMax,
            grandCompleted,
            grandSkipped,
            totalSw.ElapsedMilliseconds,
            string.IsNullOrWhiteSpace(result.FormatSummary()) ? "(无写入)" : result.FormatSummary());
        return result;
    }

}
