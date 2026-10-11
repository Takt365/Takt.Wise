// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Procurement.Chain
// 文件名称：ITaktProcurementChainOrchestrator.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Cursor AI)
// 功能描述：采购全链路编排接口（三套方案、会签 BusinessType 路由）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Services.Workflow.FlowEngine.Business;

namespace Takt.Application.Services.Logistics.Procurement.Chain;

/// <summary>
/// 采购全链路编排：询价/PR 提交会签；会签按 BusinessType 分发；方案一 PO 人工决策
/// </summary>
public interface ITaktProcurementChainOrchestrator
{
    /// <summary>
    /// 采购询价提交会签审批（方案一/二入口）
    /// </summary>
    /// <param name="inquiryId">询价主键</param>
    /// <returns>异步任务</returns>
    Task SubmitPurchaseInquiryForCountersignAsync(long inquiryId);

    /// <summary>
    /// 采购申请提交会签审批
    /// </summary>
    /// <param name="requestId">采购申请主键</param>
    /// <returns>异步任务</returns>
    Task SubmitPurchaseRequestForCountersignAsync(long requestId);

    /// <summary>
    /// 方案一：PR 会签通过后人工 PO 决策
    /// </summary>
    /// <param name="requestId">采购申请主键</param>
    /// <param name="generatePo">true=生成 PO 后报销；false=暂不生成 PO 直接报销</param>
    /// <returns>异步任务</returns>
    Task ApplyPurchaseRequestPoDecisionAsync(long requestId, bool generatePo);

    /// <summary>
    /// 会签审批通过后按 BusinessType 分发下游
    /// </summary>
    /// <param name="context">审批回写上下文</param>
    /// <returns>异步任务</returns>
    Task OnCountersignApprovedAsync(TaktApprovalFlowCompletedContext context);
}
