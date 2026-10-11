// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Extensions
// 文件名称：TaktEntityAuditExtensions.cs
// 创建时间：2026-06-26
// 创建人：Takt365(Cursor AI)
// 功能描述：实体审计字段扩展（仓储、种子、SqlSugar AOP、Quartz）；无登录上下文时使用 TaktConstants.SystemAuditUser.Id
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities;
using Takt.Shared.Constants;

namespace Takt.Infrastructure.Extensions;

/// <summary>
/// 实体审计字段扩展（与 TaktHttpAuditHelper 同属审计写入体系；操作 Domain 隔离基类，故落 Infrastructure）。
/// 写入 CreatedBy/CreatedAt/UpdatedBy/UpdatedAt 及软删除 DeletedBy/DeletedAt/IsDeleted；
/// 操作人解析顺序：显式 ID → 上下文 ID → TaktConstants.SystemAuditUser.Id。
/// </summary>
public static class TaktEntityAuditExtensions
{
    /// <summary>
    /// 解析操作人 ID：显式 ID 优先，其次上下文 ID，均无则系统审计用户。
    /// </summary>
    /// <param name="explicitUserId">调用方显式传入的操作人 ID；大于 0 时优先采用</param>
    /// <param name="contextUserId">当前会话用户 ID；显式 ID 无效时采用</param>
    /// <returns>有效操作人 ID（永不为 0）</returns>
    public static long ResolveOperatorUserId(long? explicitUserId = null, long? contextUserId = null)
    {
        if (explicitUserId is > 0)
        {
            return explicitUserId.Value;
        }
        if (contextUserId is > 0)
        {
            return contextUserId.Value;
        }
        return TaktConstants.SystemAuditUser.Id;
    }

    /// <summary>
    /// 写入创建审计字段（租户级实体）
    /// </summary>
    /// <param name="entity">待写入的租户级实体</param>
    /// <param name="operatorUserId">操作人 ID；为空或不大于 0 时使用系统审计用户</param>
    /// <param name="timestamp">审计时间；为空时使用 DateTime.Now</param>
    /// <exception cref="ArgumentNullException"><paramref name="entity"/> 为 null</exception>
    public static void ApplyCreate(
        this TaktTenantCoreEntityScopeBase entity,
        long? operatorUserId = null,
        DateTime? timestamp = null)
        => ApplyCreateCore(entity, operatorUserId, timestamp);

    /// <summary>
    /// 写入创建审计字段（公司级实体）
    /// </summary>
    /// <param name="entity">待写入的公司级实体</param>
    /// <param name="operatorUserId">操作人 ID；为空或不大于 0 时使用系统审计用户</param>
    /// <param name="timestamp">审计时间；为空时使用 DateTime.Now</param>
    /// <exception cref="ArgumentNullException"><paramref name="entity"/> 为 null</exception>
    public static void ApplyCreate(
        this TaktCompanyEntityScopeBase entity,
        long? operatorUserId = null,
        DateTime? timestamp = null)
        => ApplyCreateCore(entity, operatorUserId, timestamp);

    /// <summary>
    /// 写入创建审计字段（审批级实体）
    /// </summary>
    /// <param name="entity">待写入的审批级实体</param>
    /// <param name="operatorUserId">操作人 ID；为空或不大于 0 时使用系统审计用户</param>
    /// <param name="timestamp">审计时间；为空时使用 DateTime.Now</param>
    /// <exception cref="ArgumentNullException"><paramref name="entity"/> 为 null</exception>
    public static void ApplyCreate(
        this TaktApprovalEntityScopeBase entity,
        long? operatorUserId = null,
        DateTime? timestamp = null)
        => ApplyCreateCore(entity, operatorUserId, timestamp);

    /// <summary>
    /// 写入更新审计字段（租户级实体）
    /// </summary>
    /// <param name="entity">待写入的租户级实体</param>
    /// <param name="operatorUserId">操作人 ID；为空或不大于 0 时使用系统审计用户</param>
    /// <param name="timestamp">审计时间；为空时使用 DateTime.Now</param>
    /// <exception cref="ArgumentNullException"><paramref name="entity"/> 为 null</exception>
    public static void ApplyUpdate(
        this TaktTenantCoreEntityScopeBase entity,
        long? operatorUserId = null,
        DateTime? timestamp = null)
        => ApplyUpdateCore(entity, operatorUserId, timestamp);

    /// <summary>
    /// 写入更新审计字段（公司级实体）
    /// </summary>
    /// <param name="entity">待写入的公司级实体</param>
    /// <param name="operatorUserId">操作人 ID；为空或不大于 0 时使用系统审计用户</param>
    /// <param name="timestamp">审计时间；为空时使用 DateTime.Now</param>
    /// <exception cref="ArgumentNullException"><paramref name="entity"/> 为 null</exception>
    public static void ApplyUpdate(
        this TaktCompanyEntityScopeBase entity,
        long? operatorUserId = null,
        DateTime? timestamp = null)
        => ApplyUpdateCore(entity, operatorUserId, timestamp);

    /// <summary>
    /// 写入更新审计字段（审批级实体）
    /// </summary>
    /// <param name="entity">待写入的审批级实体</param>
    /// <param name="operatorUserId">操作人 ID；为空或不大于 0 时使用系统审计用户</param>
    /// <param name="timestamp">审计时间；为空时使用 DateTime.Now</param>
    /// <exception cref="ArgumentNullException"><paramref name="entity"/> 为 null</exception>
    public static void ApplyUpdate(
        this TaktApprovalEntityScopeBase entity,
        long? operatorUserId = null,
        DateTime? timestamp = null)
        => ApplyUpdateCore(entity, operatorUserId, timestamp);

    /// <summary>
    /// 写入软删除审计字段（租户级实体）
    /// </summary>
    /// <param name="entity">待软删除的租户级实体</param>
    /// <param name="operatorUserId">操作人 ID；为空或不大于 0 时使用系统审计用户</param>
    /// <param name="timestamp">删除时间；为空时使用 DateTime.Now</param>
    /// <exception cref="ArgumentNullException"><paramref name="entity"/> 为 null</exception>
    public static void ApplySoftDelete(
        this TaktTenantCoreEntityScopeBase entity,
        long? operatorUserId = null,
        DateTime? timestamp = null)
        => ApplySoftDeleteCore(entity, operatorUserId, timestamp);

    /// <summary>
    /// 写入软删除审计字段（公司级实体）
    /// </summary>
    /// <param name="entity">待软删除的公司级实体</param>
    /// <param name="operatorUserId">操作人 ID；为空或不大于 0 时使用系统审计用户</param>
    /// <param name="timestamp">删除时间；为空时使用 DateTime.Now</param>
    /// <exception cref="ArgumentNullException"><paramref name="entity"/> 为 null</exception>
    public static void ApplySoftDelete(
        this TaktCompanyEntityScopeBase entity,
        long? operatorUserId = null,
        DateTime? timestamp = null)
        => ApplySoftDeleteCore(entity, operatorUserId, timestamp);

    /// <summary>
    /// 写入软删除审计字段（审批级实体）
    /// </summary>
    /// <param name="entity">待软删除的审批级实体</param>
    /// <param name="operatorUserId">操作人 ID；为空或不大于 0 时使用系统审计用户</param>
    /// <param name="timestamp">删除时间；为空时使用 DateTime.Now</param>
    /// <exception cref="ArgumentNullException"><paramref name="entity"/> 为 null</exception>
    public static void ApplySoftDelete(
        this TaktApprovalEntityScopeBase entity,
        long? operatorUserId = null,
        DateTime? timestamp = null)
        => ApplySoftDeleteCore(entity, operatorUserId, timestamp);

    /// <summary>
    /// 种子/调度无 HTTP 上下文时写入创建审计（固定系统审计用户）
    /// </summary>
    /// <param name="entity">待写入的租户级实体</param>
    /// <param name="timestamp">审计时间；为空时使用 DateTime.Now</param>
    /// <exception cref="ArgumentNullException"><paramref name="entity"/> 为 null</exception>
    public static void ApplySeedCreate(this TaktTenantCoreEntityScopeBase entity, DateTime? timestamp = null)
        => entity.ApplyCreate(TaktConstants.SystemAuditUser.Id, timestamp);

    /// <summary>
    /// 种子/调度无 HTTP 上下文时写入创建审计（固定系统审计用户）
    /// </summary>
    /// <param name="entity">待写入的公司级实体</param>
    /// <param name="timestamp">审计时间；为空时使用 DateTime.Now</param>
    /// <exception cref="ArgumentNullException"><paramref name="entity"/> 为 null</exception>
    public static void ApplySeedCreate(this TaktCompanyEntityScopeBase entity, DateTime? timestamp = null)
        => entity.ApplyCreate(TaktConstants.SystemAuditUser.Id, timestamp);

    /// <summary>
    /// 种子/调度无 HTTP 上下文时写入创建审计（固定系统审计用户）
    /// </summary>
    /// <param name="entity">待写入的审批级实体</param>
    /// <param name="timestamp">审计时间；为空时使用 DateTime.Now</param>
    /// <exception cref="ArgumentNullException"><paramref name="entity"/> 为 null</exception>
    public static void ApplySeedCreate(this TaktApprovalEntityScopeBase entity, DateTime? timestamp = null)
        => entity.ApplyCreate(TaktConstants.SystemAuditUser.Id, timestamp);

    /// <summary>
    /// 种子/调度无 HTTP 上下文时写入更新审计（固定系统审计用户）
    /// </summary>
    /// <param name="entity">待写入的租户级实体</param>
    /// <param name="timestamp">审计时间；为空时使用 DateTime.Now</param>
    /// <exception cref="ArgumentNullException"><paramref name="entity"/> 为 null</exception>
    public static void ApplySeedUpdate(this TaktTenantCoreEntityScopeBase entity, DateTime? timestamp = null)
        => entity.ApplyUpdate(TaktConstants.SystemAuditUser.Id, timestamp);

    /// <summary>
    /// 种子/调度无 HTTP 上下文时写入更新审计（固定系统审计用户）
    /// </summary>
    /// <param name="entity">待写入的公司级实体</param>
    /// <param name="timestamp">审计时间；为空时使用 DateTime.Now</param>
    /// <exception cref="ArgumentNullException"><paramref name="entity"/> 为 null</exception>
    public static void ApplySeedUpdate(this TaktCompanyEntityScopeBase entity, DateTime? timestamp = null)
        => entity.ApplyUpdate(TaktConstants.SystemAuditUser.Id, timestamp);

    /// <summary>
    /// 种子/调度无 HTTP 上下文时写入更新审计（固定系统审计用户）
    /// </summary>
    /// <param name="entity">待写入的审批级实体</param>
    /// <param name="timestamp">审计时间；为空时使用 DateTime.Now</param>
    /// <exception cref="ArgumentNullException"><paramref name="entity"/> 为 null</exception>
    public static void ApplySeedUpdate(this TaktApprovalEntityScopeBase entity, DateTime? timestamp = null)
        => entity.ApplyUpdate(TaktConstants.SystemAuditUser.Id, timestamp);

    private static void ApplyCreateCore(
        TaktTenantCoreEntityScopeBase entity,
        long? operatorUserId,
        DateTime? timestamp)
    {
        ArgumentNullException.ThrowIfNull(entity);
        var userId = ResolveOperatorUserId(operatorUserId);
        var now = timestamp ?? DateTime.Now;
        entity.CreatedBy = userId;
        entity.CreatedAt = now;
        entity.UpdatedBy = userId;
        entity.UpdatedAt = now;
        entity.IsDeleted = 0;
        entity.DeletedBy = null;
        entity.DeletedAt = null;
    }

    private static void ApplyCreateCore(
        TaktCompanyEntityScopeBase entity,
        long? operatorUserId,
        DateTime? timestamp)
    {
        ArgumentNullException.ThrowIfNull(entity);
        var userId = ResolveOperatorUserId(operatorUserId);
        var now = timestamp ?? DateTime.Now;
        entity.CreatedBy = userId;
        entity.CreatedAt = now;
        entity.UpdatedBy = userId;
        entity.UpdatedAt = now;
        entity.IsDeleted = 0;
        entity.DeletedBy = null;
        entity.DeletedAt = null;
    }

    private static void ApplyCreateCore(
        TaktApprovalEntityScopeBase entity,
        long? operatorUserId,
        DateTime? timestamp)
    {
        ArgumentNullException.ThrowIfNull(entity);
        var userId = ResolveOperatorUserId(operatorUserId);
        var now = timestamp ?? DateTime.Now;
        entity.CreatedBy = userId;
        entity.CreatedAt = now;
        entity.UpdatedBy = userId;
        entity.UpdatedAt = now;
        entity.IsDeleted = 0;
        entity.DeletedBy = null;
        entity.DeletedAt = null;
    }

    private static void ApplyUpdateCore(
        TaktTenantCoreEntityScopeBase entity,
        long? operatorUserId,
        DateTime? timestamp)
    {
        ArgumentNullException.ThrowIfNull(entity);
        entity.UpdatedBy = ResolveOperatorUserId(operatorUserId);
        entity.UpdatedAt = timestamp ?? DateTime.Now;
    }

    private static void ApplyUpdateCore(
        TaktCompanyEntityScopeBase entity,
        long? operatorUserId,
        DateTime? timestamp)
    {
        ArgumentNullException.ThrowIfNull(entity);
        entity.UpdatedBy = ResolveOperatorUserId(operatorUserId);
        entity.UpdatedAt = timestamp ?? DateTime.Now;
    }

    private static void ApplyUpdateCore(
        TaktApprovalEntityScopeBase entity,
        long? operatorUserId,
        DateTime? timestamp)
    {
        ArgumentNullException.ThrowIfNull(entity);
        entity.UpdatedBy = ResolveOperatorUserId(operatorUserId);
        entity.UpdatedAt = timestamp ?? DateTime.Now;
    }

    private static void ApplySoftDeleteCore(
        TaktTenantCoreEntityScopeBase entity,
        long? operatorUserId,
        DateTime? timestamp)
    {
        ArgumentNullException.ThrowIfNull(entity);
        var userId = ResolveOperatorUserId(operatorUserId);
        var now = timestamp ?? DateTime.Now;
        entity.IsDeleted = 1;
        entity.DeletedBy = userId;
        entity.DeletedAt = now;
        entity.UpdatedBy = userId;
        entity.UpdatedAt = now;
    }

    private static void ApplySoftDeleteCore(
        TaktCompanyEntityScopeBase entity,
        long? operatorUserId,
        DateTime? timestamp)
    {
        ArgumentNullException.ThrowIfNull(entity);
        var userId = ResolveOperatorUserId(operatorUserId);
        var now = timestamp ?? DateTime.Now;
        entity.IsDeleted = 1;
        entity.DeletedBy = userId;
        entity.DeletedAt = now;
        entity.UpdatedBy = userId;
        entity.UpdatedAt = now;
    }

    private static void ApplySoftDeleteCore(
        TaktApprovalEntityScopeBase entity,
        long? operatorUserId,
        DateTime? timestamp)
    {
        ArgumentNullException.ThrowIfNull(entity);
        var userId = ResolveOperatorUserId(operatorUserId);
        var now = timestamp ?? DateTime.Now;
        entity.IsDeleted = 1;
        entity.DeletedBy = userId;
        entity.DeletedAt = now;
        entity.UpdatedBy = userId;
        entity.UpdatedAt = now;
    }
}
