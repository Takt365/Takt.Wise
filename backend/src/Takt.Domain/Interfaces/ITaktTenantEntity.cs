// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Interfaces
// 文件名称：ITaktTenantEntity.cs
// 创建时间：2026-08-12
// 创建人：Takt365(Cursor AI)
// 功能描述：租户级实体公共契约（四组合雪花/自增基类；仓储/种子泛型约束）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Domain.Interfaces;

/// <summary>
/// 租户级实体公共契约（组合 1～4 雪花/自增基类实现；仓储/种子约束要求具备 Id）
/// </summary>
public interface ITaktTenantEntity
{
    /// <summary>
    /// 主键ID（雪花或自增）
    /// </summary>
    long Id { get; set; }

    /// <summary>
    /// 租户编码
    /// </summary>
    string TenantCode { get; set; }

    /// <summary>
    /// 扩展字段
    /// </summary>
    string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    string? Remark { get; set; }

    /// <summary>
    /// 创建人ID
    /// </summary>
    long CreatedBy { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    DateTime CreatedAt { get; set; }

    /// <summary>
    /// 更新人ID
    /// </summary>
    long? UpdatedBy { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// 是否删除
    /// </summary>
    int IsDeleted { get; set; }

    /// <summary>
    /// 删除人ID
    /// </summary>
    long? DeletedBy { get; set; }

    /// <summary>
    /// 删除时间
    /// </summary>
    DateTime? DeletedAt { get; set; }
}
