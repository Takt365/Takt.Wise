// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Workflow.FlowEngine
// 文件名称：ITaktFlowEngineService.cs
// 创建时间：2026-06-03
// 创建人：Takt365(Cursor AI)
// 功能描述：流程引擎服务接口
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Workflow;
using Takt.Shared.Models;

namespace Takt.Application.Services.Workflow.FlowEngine;

/// <summary>
/// 流程引擎服务接口
/// </summary>
public interface ITaktFlowEngineService
{
    /// <summary>
    /// 发起流程
    /// </summary>
    /// <param name="dto">发起参数</param>
    /// <returns>实例详情</returns>
    Task<TaktFlowInstanceDetailDto> StartFlowInstanceAsync(TaktFlowStartDto dto);
    /// <summary>
    /// 保存草稿
    /// </summary>
    /// <param name="dto">发起参数</param>
    /// <returns>实例详情</returns>
    Task<TaktFlowInstanceDetailDto> CreateFlowInstanceDraftAsync(TaktFlowStartDto dto);
    /// <summary>
    /// 从草稿启动
    /// </summary>
    /// <param name="instanceId">实例 ID</param>
    /// <returns>实例详情</returns>
    Task<TaktFlowInstanceDetailDto> StartFlowInstanceFromDraftAsync(long instanceId);
    /// <summary>
    /// 办结任务（通过/驳回）
    /// </summary>
    /// <param name="dto">办结参数</param>
    /// <returns>任务</returns>
    Task CompleteFlowInstanceTaskAsync(TaktFlowCompleteTaskDto dto);
    /// <summary>
    /// 待办列表
    /// </summary>
    /// <param name="query">查询</param>
    /// <returns>分页</returns>
    Task<TaktPagedResult<TaktFlowTodoItemDto>> GetFlowInstanceTodoListAsync(TaktFlowTodoQueryDto query);
    /// <summary>
    /// 当前用户待办数量
    /// </summary>
    /// <returns>待办数量 DTO</returns>
    Task<TaktFlowTodoCountDto> GetFlowInstanceTodoCountAsync();

    /// <summary>
    /// 获取工作流待办统计（数据看板）
    /// </summary>
    /// <returns>待办统计</returns>
    Task<TaktWorkflowTodoStatDto> GetWorkflowTodoStatAsync();

    /// <summary>
    /// 获取我发起的流程统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>我发起的流程统计</returns>
    Task<TaktWorkflowMyInstanceStatDto> GetWorkflowMyInstanceStatAsync(TaktWorkflowMyInstanceStatQueryDto queryDto);

    /// <summary>
    /// 获取已办任务统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>已办任务统计</returns>
    Task<TaktWorkflowProcessedStatDto> GetWorkflowProcessedStatAsync(TaktWorkflowProcessedStatQueryDto queryDto);

    /// <summary>
    /// 我发起的流程
    /// </summary>
    /// <param name="query">查询</param>
    /// <returns>分页</returns>
    Task<TaktPagedResult<TaktFlowInstanceListItemDto>> GetFlowInstanceMyListAsync(TaktFlowTodoQueryDto query);
    /// <summary>
    /// 已办流程
    /// </summary>
    /// <param name="query">查询</param>
    /// <returns>分页</returns>
    Task<TaktPagedResult<TaktFlowInstanceListItemDto>> GetFlowInstanceProcessedListAsync(TaktFlowTodoQueryDto query);
    /// <summary>
    /// 实例详情（前端视图）
    /// </summary>
    /// <param name="instanceId">实例 ID</param>
    /// <returns>详情</returns>
    Task<TaktFlowInstanceDetailDto?> GetFlowInstanceDetailByIdAsync(long instanceId);
    /// <summary>
    /// 撤回流程（发起人）
    /// </summary>
    /// <param name="instanceCode">实例编码</param>
    /// <returns>任务</returns>
    Task RevokeFlowInstanceAsync(string instanceCode);
    /// <summary>
    /// 转办
    /// </summary>
    /// <param name="dto">转办参数</param>
    /// <returns>任务</returns>
    Task TransferFlowInstanceAsync(TaktFlowTransferDto dto);
    /// <summary>
    /// 加签
    /// </summary>
    /// <param name="dto">加签参数</param>
    /// <returns>任务</returns>
    Task AddFlowInstanceApproversAsync(TaktFlowAddApproversDto dto);
    /// <summary>
    /// 减签
    /// </summary>
    /// <param name="dto">减签参数</param>
    /// <returns>任务</returns>
    Task ReduceFlowInstanceApprovalAsync(TaktFlowReduceApprovalDto dto);
    /// <summary>
    /// 挂起
    /// </summary>
    /// <param name="dto">参数</param>
    /// <returns>任务</returns>
    Task SuspendFlowInstanceAsync(TaktFlowInstanceOperateDto dto);
    /// <summary>
    /// 恢复
    /// </summary>
    /// <param name="dto">参数</param>
    /// <returns>任务</returns>
    Task ResumeFlowInstanceAsync(TaktFlowInstanceOperateDto dto);
    /// <summary>
    /// 终止
    /// </summary>
    /// <param name="dto">参数</param>
    /// <returns>任务</returns>
    Task TerminateFlowInstanceAsync(TaktFlowInstanceOperateDto dto);
    /// <summary>
    /// 撤销当前节点审批
    /// </summary>
    /// <param name="dto">参数</param>
    /// <returns>任务</returns>
    Task UndoFlowInstanceVerificationAsync(TaktFlowInstanceOperateDto dto);

    /// <summary>
    /// 可发起流程方案列表（已发布、最新版、未挂起）
    /// </summary>
    /// <returns>方案摘要列表</returns>
    Task<List<TaktFlowStartableSchemeDto>> GetStartableSchemeListAsync();

    /// <summary>
    /// 审批业务表白名单（TaktApprovalEntityBase 物理表名）
    /// </summary>
    /// <returns>表名列表</returns>
    IReadOnlyList<string> GetApprovalFlowTableNames();
}
