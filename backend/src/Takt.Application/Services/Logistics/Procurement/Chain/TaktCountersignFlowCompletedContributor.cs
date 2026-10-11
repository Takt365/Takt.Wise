// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Procurement.Chain
// 文件名称：TaktCountersignFlowCompletedContributor.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Cursor AI)
// 功能描述：会签审批通过后触发采购链路下游编排（避免与会签 Service 循环依赖）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Services.Workflow.FlowEngine.Business;
using Takt.Shared.Constants;

namespace Takt.Application.Services.Logistics.Procurement.Chain;

/// <summary>
/// 会签单审批通过后业务回写
/// </summary>
public class TaktCountersignFlowCompletedContributor : ITaktApprovalFlowCompletedContributor
{
    private readonly ITaktProcurementChainOrchestrator _procurementChainOrchestrator;

    /// <summary>
    /// 初始化会签审批回写贡献点
    /// </summary>
    /// <param name="procurementChainOrchestrator">采购全链路编排</param>
    public TaktCountersignFlowCompletedContributor(ITaktProcurementChainOrchestrator procurementChainOrchestrator)
    {
        _procurementChainOrchestrator = procurementChainOrchestrator;
    }

    /// <summary>
    /// 审批数据源物理表名（与实体 SugarTable、TaktFlowForm.RelatedTableName 一致；全应用唯一）
    /// </summary>
    public string RelatedTableName => TaktProcurementConstants.CountersignTableName;

    /// <summary>
    /// 审批通过后执行业务投影/归档（在 TaktApprovalFlowBusinessService 通用补丁之后调用）
    /// </summary>
    /// <param name="context">回写上下文（租户/公司与 Gateway 同源）</param>
    /// <returns>异步任务</returns>
    public Task OnApprovalFlowCompletedAsync(TaktApprovalFlowCompletedContext context)
        => _procurementChainOrchestrator.OnCountersignApprovedAsync(context);
}
