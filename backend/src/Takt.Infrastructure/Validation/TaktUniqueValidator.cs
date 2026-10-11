// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Validation
// 文件名称：TaktUniqueValidator.cs
// 创建时间：2026-05-23
// 创建人：Takt365(Cursor AI)
// 功能描述：唯一性验证器实现（通过仓储 ExistsAsync 查重）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;

namespace Takt.Infrastructure.Validation;

/// <summary>
/// 唯一性验证器实现（直接调用仓储的 ExistsAsync 方法）
/// </summary>
public class TaktUniqueValidator : ITaktUniqueValidator
{
    /// <summary>
    /// 验证字段在数据库中是否唯一（不存在）
    /// </summary>
    public async Task<bool> IsUniqueAsync<TEntity, TProperty>(
        ITaktUniqueExistenceRepository<TEntity> repository,
        Expression<Func<TEntity, TProperty>> fieldSelector,
        TProperty value,
        long? excludeId = null)
    {
        if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)))
        {
            return true;
        }

        var fieldName = GetMemberName(fieldSelector);

        var parameter = Expression.Parameter(typeof(TEntity), "x");
        var property = Expression.Property(parameter, fieldName);
        var constant = Expression.Constant(value, typeof(TProperty));
        var equals = Expression.Equal(property, constant);
        Expression combinedExpression = equals;

        if (excludeId.HasValue)
        {
            var idProperty = Expression.Property(parameter, "Id");
            var idConstant = Expression.Constant(excludeId.Value);
            var idNotEquals = Expression.NotEqual(idProperty, idConstant);
            combinedExpression = Expression.AndAlso(equals, idNotEquals);
        }

        var predicate = Expression.Lambda<Func<TEntity, bool>>(combinedExpression, parameter);
        var exists = await repository.ExistsAsync(predicate);
        return !exists;
    }

    /// <summary>
    /// 按条件表达式验证是否存在记录（通用多字段组合唯一性）
    /// </summary>
    public async Task<bool> IsUniqueAsync<TEntity>(
        ITaktUniqueExistenceRepository<TEntity> repository,
        Expression<Func<TEntity, bool>> predicate,
        long? excludeId = null)
    {
        if (predicate == null)
        {
            return true;
        }

        var parameter = predicate.Parameters[0];
        Expression combinedExpression = predicate.Body;

        if (excludeId.HasValue)
        {
            var idProperty = Expression.Property(parameter, "Id");
            var idConstant = Expression.Constant(excludeId.Value);
            var idNotEquals = Expression.NotEqual(idProperty, idConstant);
            combinedExpression = Expression.AndAlso(predicate.Body, idNotEquals);
        }

        var lambda = Expression.Lambda<Func<TEntity, bool>>(combinedExpression, parameter);
        var exists = await repository.ExistsAsync(lambda);
        return !exists;
    }

    /// <summary>
    /// 从属性访问表达式中解析成员名
    /// </summary>
    private static string GetMemberName<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> selector)
    {
        if (selector?.Body is MemberExpression member)
        {
            return member.Member.Name;
        }

        if (selector?.Body is UnaryExpression unary && unary.Operand is MemberExpression unaryMember)
        {
            return unaryMember.Member.Name;
        }

        throw new ArgumentException("表达式必须是简单的属性访问表达式", nameof(selector));
    }
}
