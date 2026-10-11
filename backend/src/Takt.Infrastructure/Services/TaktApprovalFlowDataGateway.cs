// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Services
// 文件名称：TaktApprovalFlowDataGateway.cs
// 创建时间：2026-06-11
// 创建人：Takt365(Cursor AI)
// 功能描述：审批业务表动态读写（SqlSugar，表名白名单）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Interfaces;
using Takt.Infrastructure.Data.Context;
using Takt.Infrastructure.Repositories;
using Takt.Shared.Helpers;
using Takt.Shared.Options;
using Microsoft.Extensions.Options;

namespace Takt.Infrastructure.Services;

/// <summary>
/// ITaktApprovalFlowDataGateway 实现
/// </summary>
public sealed class TaktApprovalFlowDataGateway : ITaktApprovalFlowDataGateway
{
    private readonly TaktSqlSugarContext _dbContext;
    private readonly PrimaryKeyTypeOptions _primaryKeyTypeOptions;

    /// <summary>
    /// 初始化网关
    /// </summary>
    /// <param name="dbContext">SqlSugar 上下文</param>
    /// <param name="primaryKeyTypeOptions">主键策略</param>
    public TaktApprovalFlowDataGateway(
        TaktSqlSugarContext dbContext,
        IOptions<PrimaryKeyTypeOptions> primaryKeyTypeOptions)
    {
        _dbContext = dbContext;
        _primaryKeyTypeOptions = primaryKeyTypeOptions.Value;
    }

    /// <summary>
    /// 按主键查询一行（列名 → 值）
    /// </summary>
    /// <param name="tableName">物理表名</param>
    /// <param name="id">主键</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <returns>行数据；不存在返回 null</returns>
    public async Task<Dictionary<string, object?>?> GetRowByIdAsync(
        string tableName,
        long id,
        string tenantCode,
        string companyCode)
    {
        EnsureTableAllowed(tableName);
        var sql = $"SELECT * FROM {tableName} WHERE id=@id AND tenant_code=@tenantCode AND company_code=@companyCode AND is_deleted=0";
        var rows = await TaktRepositoryReadOnlySql.QueryAsync(
            _dbContext.Db,
            sql,
            new Dictionary<string, object?>
            {
                ["id"] = id,
                ["tenantCode"] = tenantCode,
                ["companyCode"] = companyCode
            });
        if (rows.Count == 0)
        {
            return null;
        }
        var dict = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
        foreach (var pair in rows[0])
        {
            dict[pair.Key] = pair.Value;
        }
        return dict;
    }

    /// <summary>
    /// 插入一行并返回雪花主键
    /// </summary>
    /// <param name="tableName">物理表名</param>
    /// <param name="columns">列名 → 值（不含 id）</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="userId">操作人</param>
    /// <returns>新主键</returns>
    public async Task<long> InsertRowAsync(
        string tableName,
        IReadOnlyDictionary<string, object?> columns,
        string tenantCode,
        string companyCode,
        long userId)
    {
        ArgumentNullException.ThrowIfNull(columns);
        EnsureTableAllowed(tableName);
        var now = DateTime.Now;
        var data = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
        {
            ["tenant_code"] = tenantCode,
            ["company_code"] = companyCode,
            ["created_at"] = now,
            ["updated_at"] = now,
            ["created_by"] = userId,
            ["updated_by"] = userId,
            ["is_deleted"] = 0
        };
        foreach (var pair in columns)
        {
            if (!string.IsNullOrWhiteSpace(pair.Key))
            {
                data[pair.Key] = pair.Value ?? DBNull.Value;
            }
        }
        return await TaktPrimaryKeyInsertHelper.InsertDictionaryAsync(
            _dbContext.Db,
            data,
            tableName,
            _primaryKeyTypeOptions);
    }

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
    public async Task<bool> UpdateFlowStateAsync(
        string tableName,
        long id,
        string tenantCode,
        string companyCode,
        long userId,
        TaktApprovalFlowStatePatch patch)
    {
        ArgumentNullException.ThrowIfNull(patch);
        EnsureTableAllowed(tableName);
        var sets = new List<string> { "updated_at=@updatedAt", "updated_by=@userId" };
        var parameters = new Dictionary<string, object?>
        {
            ["id"] = id,
            ["tenantCode"] = tenantCode,
            ["companyCode"] = companyCode,
            ["updatedAt"] = DateTime.Now,
            ["userId"] = userId
        };
        if (patch.FlowInstanceId.HasValue)
        {
            sets.Add("flow_instance_id=@flowInstanceId");
            parameters["flowInstanceId"] = patch.FlowInstanceId.Value;
        }
        if (patch.ApprovalStatus.HasValue)
        {
            sets.Add("approval_status=@approvalStatus");
            parameters["approvalStatus"] = patch.ApprovalStatus.Value;
        }
        if (patch.InitiatorId.HasValue)
        {
            sets.Add("initiator_id=@initiatorId");
            parameters["initiatorId"] = patch.InitiatorId.Value;
        }
        if (patch.InitiatedAt.HasValue)
        {
            sets.Add("initiated_at=@initiatedAt");
            parameters["initiatedAt"] = patch.InitiatedAt.Value;
        }
        if (!string.IsNullOrWhiteSpace(patch.BusinessStatusColumn) && patch.BusinessStatusValue.HasValue)
        {
            sets.Add($"{patch.BusinessStatusColumn}=@businessStatusValue");
            parameters["businessStatusValue"] = patch.BusinessStatusValue.Value;
        }
        var sql = $"UPDATE {tableName} SET {string.Join(", ", sets)} WHERE id=@id AND tenant_code=@tenantCode AND company_code=@companyCode AND is_deleted=0";
        var rows = await _dbContext.Db.Ado.ExecuteCommandAsync(sql, parameters);
        return rows > 0;
    }

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
    public async Task<bool> UpdateRowColumnsAsync(
        string tableName,
        long id,
        string tenantCode,
        string companyCode,
        long userId,
        IReadOnlyDictionary<string, object?> columns)
    {
        ArgumentNullException.ThrowIfNull(columns);
        if (columns.Count == 0)
        {
            return false;
        }
        EnsureTableAllowed(tableName);
        var sets = new List<string> { "updated_at=@updatedAt", "updated_by=@userId" };
        var parameters = new Dictionary<string, object?>
        {
            ["id"] = id,
            ["tenantCode"] = tenantCode,
            ["companyCode"] = companyCode,
            ["updatedAt"] = DateTime.Now,
            ["userId"] = userId
        };
        var protectedColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "id", "tenant_code", "company_code", "created_at", "created_by", "is_deleted"
        };
        var index = 0;
        foreach (var pair in columns)
        {
            if (string.IsNullOrWhiteSpace(pair.Key) || protectedColumns.Contains(pair.Key))
            {
                continue;
            }
            var paramName = $"col{index}";
            sets.Add($"{pair.Key}=@{paramName}");
            parameters[paramName] = pair.Value ?? DBNull.Value;
            index++;
        }
        if (sets.Count <= 2)
        {
            return false;
        }
        var sql = $"UPDATE {tableName} SET {string.Join(", ", sets)} WHERE id=@id AND tenant_code=@tenantCode AND company_code=@companyCode AND is_deleted=0";
        var rows = await _dbContext.Db.Ado.ExecuteCommandAsync(sql, parameters);
        return rows > 0;
    }

    /// <summary>
    /// 获取审批业务表白名单（TaktApprovalEntityBase 实体物理表名）
    /// </summary>
    /// <returns>表名列表</returns>
    public IReadOnlyList<string> GetAllowedTableNames()
    {
        return TaktApprovalFlowTableWhitelist.GetAllowedTableNames();
    }

    /// <summary>
    /// 校验表名在白名单内
    /// </summary>
    private static void EnsureTableAllowed(string tableName)
    {
        if (!TaktApprovalFlowTableWhitelist.IsAllowed(tableName))
        {
            throw new InvalidOperationException($"表「{tableName}」不在审批业务白名单内");
        }
    }

}
