// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Interfaces
// 文件名称：ITaktUniqueValidator.cs
// 创建时间：2026-05-23
// 创建人：Takt365
// 功能描述：唯一性验证器接口（定义查重验证契约）
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Takt.Domain.Repositories;

namespace Takt.Domain.Interfaces;

/// <summary>
/// 唯一性验证器接口（定义查重验证契约）
/// </summary>
public interface ITaktUniqueValidator
{
    /// <summary>
    /// 验证字段在数据库中是否唯一（不存在）
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TProperty">属性类型</typeparam>
    /// <param name="repository">仓储（租户/公司/审批级均通过 ITaktUniqueExistenceRepository 统一查重）</param>
    /// <param name="fieldSelector">字段选择器（从实体到属性）</param>
    /// <param name="value">字段值</param>
    /// <param name="excludeId">排除的ID（可选，更新时使用）</param>
    /// <returns>验证任务，如果唯一返回 true，否则返回 false</returns>
    Task<bool> IsUniqueAsync<TEntity, TProperty>(
        ITaktUniqueExistenceRepository<TEntity> repository,
        Expression<Func<TEntity, TProperty>> fieldSelector,
        TProperty value,
        long? excludeId = null);

    /// <summary>
    /// 按条件表达式验证是否存在记录（通用多字段组合唯一性）
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="repository">仓储</param>
    /// <param name="predicate">条件表达式（由调用方编写）</param>
    /// <param name="excludeId">排除的ID（可选，更新时使用）</param>
    /// <returns>验证任务，如果唯一返回 true，否则返回 false</returns>
    Task<bool> IsUniqueAsync<TEntity>(
        ITaktUniqueExistenceRepository<TEntity> repository,
        Expression<Func<TEntity, bool>> predicate,
        long? excludeId = null);
}
