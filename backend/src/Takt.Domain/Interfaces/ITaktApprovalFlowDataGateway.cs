// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Interfaces
// 文件名称：ITaktApprovalFlowDataGateway.cs
// 创建时间：2026-06-11
// 创建人：Takt365(Cursor AI)
// 功能描述：审批业务表动态读写网关（按表单 RelatedTableName，白名单表）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Domain.Interfaces;

/// <summary>
/// 审批业务表流程状态补丁
/// </summary>
public sealed class TaktApprovalFlowStatePatch
{
    /// <summary>
    /// 流程实例 ID
    /// </summary>
    public long? FlowInstanceId { get; set; }

    /// <summary>
    /// 审批状态
    /// </summary>
    public int? ApprovalStatus { get; set; }

    /// <summary>
    /// 发起人 ID
    /// </summary>
    public long? InitiatorId { get; set; }

    /// <summary>
    /// 发起时间
    /// </summary>
    public DateTime? InitiatedAt { get; set; }

    /// <summary>
    /// 业务状态列名（蛇形）
    /// </summary>
    public string? BusinessStatusColumn { get; set; }

    /// <summary>
    /// 业务状态值
    /// </summary>
    public int? BusinessStatusValue { get; set; }
}

/// <summary>
/// 审批业务表数据网关（Infrastructure 实现，表名须在审批实体白名单内）
/// </summary>
public interface ITaktApprovalFlowDataGateway
{
    /// <summary>
    /// 按主键查询一行（列名 → 值）
    /// </summary>
    /// <param name="tableName">物理表名</param>
    /// <param name="id">主键</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <returns>行数据；不存在返回 null</returns>
    Task<Dictionary<string, object?>?> GetRowByIdAsync(
        string tableName,
        long id,
        string tenantCode,
        string companyCode);

    /// <summary>
    /// 插入一行并返回雪花主键
    /// </summary>
    /// <param name="tableName">物理表名</param>
    /// <param name="columns">列名 → 值（不含 id）</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="userId">操作人</param>
    /// <returns>新主键</returns>
    Task<long> InsertRowAsync(
        string tableName,
        IReadOnlyDictionary<string, object?> columns,
        string tenantCode,
        string companyCode,
        long userId);

    /// <summary>
    /// 按主键更新流程相关列
    /// </summary>
    /// <param name="tableName">物理表名</param>
    /// <param name="id">主键</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="userId">操作人</param>
    /// <param name="patch">补丁字段</param>
    /// <returns>是否更新到行</returns>
    Task<bool> UpdateFlowStateAsync(
        string tableName,
        long id,
        string tenantCode,
        string companyCode,
        long userId,
        TaktApprovalFlowStatePatch patch);

    /// <summary>
    /// 按主键更新业务列（不含流程状态列；用于审批通过后回写 FrmData）
    /// </summary>
    /// <param name="tableName">物理表名</param>
    /// <param name="id">主键</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="userId">操作人</param>
    /// <param name="columns">列名 → 值（蛇形列名）</param>
    /// <returns>是否更新到行</returns>
    Task<bool> UpdateRowColumnsAsync(
        string tableName,
        long id,
        string tenantCode,
        string companyCode,
        long userId,
        IReadOnlyDictionary<string, object?> columns);

    /// <summary>
    /// 获取审批业务表白名单（TaktApprovalEntityBase 实体物理表名）
    /// </summary>
    /// <returns>表名列表</returns>
    IReadOnlyList<string> GetAllowedTableNames();
}
