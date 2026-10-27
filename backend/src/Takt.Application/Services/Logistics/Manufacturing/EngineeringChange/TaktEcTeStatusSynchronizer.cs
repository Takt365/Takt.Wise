// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcTeStatusSynchronizer.cs
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
public class TaktEcTeStatusSynchronizer
{
    private readonly ITaktCompanyRepository<TaktEcTe> _ecTeRepository;
    private readonly TaktEcExecDeptAccess _ecExecDeptAccess;
    private readonly TaktEcHandoffMailService _handoffMailService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecTeRepository">设变技术课主仓储</param>
    /// <param name="ecExecDeptAccess">部门执行跨表访问</param>
    /// <param name="handoffMailService">实施顺序邮件</param>
    public TaktEcTeStatusSynchronizer(
        ITaktCompanyRepository<TaktEcTe> ecTeRepository,
        TaktEcExecDeptAccess ecExecDeptAccess,
        TaktEcHandoffMailService handoffMailService)
    {
        _ecTeRepository = ecTeRepository;
        _ecExecDeptAccess = ecExecDeptAccess;
        _handoffMailService = handoffMailService;
    }

    /// <summary>
    /// 按设变单号重算并回写 EcStatus：全部执行部门都未人工填写=发行；任一执行部门有人工输入=执行中；全部未作废执行行都已填写=完成
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
        var te = await _ecTeRepository.FirstAsync(x => x.EcCode == normalized);
        if (te == null)
        {
            return;
        }
        // 不拉全表明细：无人工输入=发行；任一执行部门有人工输入=执行中；全部未作废执行行都已填写=完成
        var anyInput = await _ecExecDeptAccess.ExistsAnyDeptInputByEcCodeAsync(normalized);
        var computed = TaktEcTeStatusConstants.Issued;
        if (anyInput)
        {
            var anyUnfilled = await _ecExecDeptAccess.ExistsAnyUnfilledDeptByEcCodeAsync(normalized);
            computed = anyUnfilled
                ? TaktEcTeStatusConstants.InProgress
                : TaktEcTeStatusConstants.Completed;
        }
        if (te.EcStatus != computed)
        {
            te.EcStatus = computed;
            await _ecTeRepository.UpdateAsync(te);
        }
        if (anyInput)
        {
            await _handoffMailService.TryDispatchAsync(te);
        }
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

}
