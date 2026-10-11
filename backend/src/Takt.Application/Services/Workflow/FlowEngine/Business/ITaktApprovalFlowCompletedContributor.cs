// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Workflow.FlowEngine.Business
// 文件名称：ITaktApprovalFlowCompletedContributor.cs
// 创建时间：2026-06-18
// 创建人：Takt365(Cursor AI)
// 功能描述：审批通过后业务回写贡献点（由各审批实体应用服务按需实现，Autofac AsImplementedInterfaces 自动发现）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Services.Workflow.FlowEngine.Business;

/// <summary>
/// 审批通过后业务回写贡献点（一张审批表由一个应用服务声明 RelatedTableName 并实现回写）
/// </summary>
public interface ITaktApprovalFlowCompletedContributor
{
    /// <summary>
    /// 审批数据源物理表名（与实体 SugarTable、TaktFlowForm.RelatedTableName 一致；全应用唯一）
    /// </summary>
    string RelatedTableName { get; }

    /// <summary>
    /// 审批通过后执行业务投影/归档（在 TaktApprovalFlowBusinessService 通用补丁之后调用）
    /// </summary>
    /// <param name="context">回写上下文（租户/公司与 Gateway 同源）</param>
    /// <returns>异步任务</returns>
    Task OnApprovalFlowCompletedAsync(TaktApprovalFlowCompletedContext context);
}
