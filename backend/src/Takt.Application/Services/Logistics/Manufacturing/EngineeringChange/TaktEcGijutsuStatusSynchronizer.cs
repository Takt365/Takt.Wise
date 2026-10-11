// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcGijutsuStatusSynchronizer.cs
// 创建时间：2026-08-26
// 创建人：Takt365(Cursor AI)
// 功能描述：按各部门执行表输入情况自动回写主表 EcStatus（1=发行 2=执行中 3=完成）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Repositories;
using Takt.Shared.Constants;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变技术课主表 EcStatus 自动同步（部门执行表写后调用）
/// </summary>
public class TaktEcGijutsuStatusSynchronizer
{
    private readonly ITaktCompanyRepository<TaktEcGijutsu> _ecGijutsuRepository;
    private readonly ITaktCompanyRepository<TaktEcDetail> _ecDetailRepository;
    private readonly TaktEcExecDeptAccess _ecExecDeptAccess;
    private readonly TaktEcHandoffMailService _handoffMailService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecGijutsuRepository">设变技术课主仓储</param>
    /// <param name="ecDetailRepository">设变明细仓储</param>
    /// <param name="ecExecDeptAccess">部门执行跨表访问</param>
    /// <param name="handoffMailService">实施顺序邮件</param>
    public TaktEcGijutsuStatusSynchronizer(
        ITaktCompanyRepository<TaktEcGijutsu> ecGijutsuRepository,
        ITaktCompanyRepository<TaktEcDetail> ecDetailRepository,
        TaktEcExecDeptAccess ecExecDeptAccess,
        TaktEcHandoffMailService handoffMailService)
    {
        _ecGijutsuRepository = ecGijutsuRepository;
        _ecDetailRepository = ecDetailRepository;
        _ecExecDeptAccess = ecExecDeptAccess;
        _handoffMailService = handoffMailService;
    }

    /// <summary>
    /// 按设变单号重算并回写 EcStatus：无部门输入=发行；任一部门有输入=执行中；全部责任部门均填写=完成
    /// </summary>
    /// <param name="ecCode">设变单号</param>
    /// <returns>任务</returns>
    public async Task RefreshByEcCodeAsync(string? ecCode)
    {
        if (string.IsNullOrWhiteSpace(ecCode))
        {
            return;
        }
        var normalized = ecCode.Trim();
        var gijutsu = await _ecGijutsuRepository.FirstAsync(x => x.EcCode == normalized);
        if (gijutsu == null)
        {
            return;
        }
        // 新建/刚派生后通常无部门输入：EXISTS 快路径，避免超大 IN / 全量拉明细与执行行
        if (!await _ecExecDeptAccess.ExistsAnyDeptInputByEcCodeAsync(normalized))
        {
            if (gijutsu.EcStatus == TaktEcGijutsuStatusConstants.Issued)
            {
                return;
            }
            gijutsu.EcStatus = TaktEcGijutsuStatusConstants.Issued;
            await _ecGijutsuRepository.UpdateAsync(gijutsu);
            return;
        }
        var details = await _ecDetailRepository.GetListAsync(x =>
            x.EcCode == normalized && x.IsObsolete == 0);
        var execRows = await _ecExecDeptAccess.ListBaseByEcCodeAsync(normalized);
        var computed = ComputeEcStatus(details, execRows);
        if (gijutsu.EcStatus != computed)
        {
            gijutsu.EcStatus = computed;
            await _ecGijutsuRepository.UpdateAsync(gijutsu);
        }
        await _handoffMailService.TryDispatchAsync(gijutsu, details, execRows);
    }

    /// <summary>
    /// 批量按设变单号刷新（导入等多行写后调用）
    /// </summary>
    /// <param name="ecCodes">设变单号集合</param>
    /// <returns>任务</returns>
    public async Task RefreshByEcCodesAsync(IEnumerable<string?> ecCodes)
    {
        var distinct = ecCodes
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x!.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        foreach (var code in distinct)
        {
            await RefreshByEcCodeAsync(code);
        }
    }

    /// <summary>
    /// 根据明细与部门执行行计算 EcStatus
    /// </summary>
    private static int ComputeEcStatus(
        IReadOnlyList<TaktEcDetail> details,
        IReadOnlyList<TaktEcExecBaseRow> execRows)
    {
        if (details.Count == 0)
        {
            return TaktEcGijutsuStatusConstants.Issued;
        }
        var byDetail = execRows
            .Where(x => x.IsObsolete == 0)
            .GroupBy(x => x.EcDetailId)
            .ToDictionary(g => g.Key, g => g.ToList());
        var anyInput = false;
        var allFilled = true;
        foreach (var detail in details)
        {
            byDetail.TryGetValue(detail.Id, out var deptList);
            deptList ??= [];
            foreach (var deptCode in TaktEcDeptCodes.KanbanOrder)
            {
                var exec = deptList.FirstOrDefault(x => x.DeptCode == deptCode);
                var filled = exec != null && HasDeptInput(exec);
                if (filled)
                {
                    anyInput = true;
                }
                else
                {
                    allFilled = false;
                }
            }
        }
        if (allFilled)
        {
            return TaktEcGijutsuStatusConstants.Completed;
        }
        if (anyInput)
        {
            return TaktEcGijutsuStatusConstants.InProgress;
        }
        return TaktEcGijutsuStatusConstants.Issued;
    }

    /// <summary>
    /// 部门执行行是否已有输入（实施=是，或执行内容非空）
    /// </summary>
    private static bool HasDeptInput(TaktEcExecBaseRow exec) =>
        exec.IsImplemented == 1 || !string.IsNullOrWhiteSpace(exec.ExecContent);
}
