// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Repositories
// 文件名称：ITaktUniqueExistenceRepository.cs
// 创建时间：2026-05-23
// 创建人：Takt365(Cursor AI)
// 功能描述：唯一性查重仓储契约（供 ITaktUniqueValidator 使用）
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;

namespace Takt.Domain.Repositories;

/// <summary>
/// 唯一性查重仓储契约（仅暴露存在性检查，供唯一性验证器使用）
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public interface ITaktUniqueExistenceRepository<TEntity>
{
    /// <summary>
    /// 根据条件检查是否存在记录
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <returns>是否存在</returns>
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
}
