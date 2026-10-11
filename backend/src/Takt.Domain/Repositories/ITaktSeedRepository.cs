// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Repositories
// 文件名称：ITaktSeedRepository.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：种子数据专用仓储接口，绕过用户上下文，支持精确租户控制和雪花ID
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Takt.Domain.Entities;
using Takt.Domain.Interfaces;

namespace Takt.Domain.Repositories;

/// <summary>
/// 租户级种子数据专用仓储接口
/// 特点：
/// 1. 不依赖 ITaktUserContext（种子数据阶段无 HTTP 上下文）
/// 2. 不自动添加租户/公司过滤（由调用方显式指定）
/// 3. 支持精确控制租户和公司编码
/// 4. 自动填充雪花ID
/// </summary>
/// <typeparam name="TEntity">实体类型（必须实现 ITaktTenantEntity）</typeparam>
public interface ITaktTenantSeedRepository<TEntity> where TEntity : TaktTenantCoreEntityScopeBase, ITaktTenantEntity, new()
{
    // ========================================
    // 新增操作（自动填充雪花ID）
    // ========================================

    /// <summary>
    /// 创建实体（自动填充雪花ID，不自动填充租户信息）
    /// </summary>
    /// <param name="entity">实体对象</param>
    /// <returns>创建的实体</returns>
    Task<TEntity> CreateAsync(TEntity entity);

    /// <summary>
    /// 批量创建实体（自动填充雪花ID，不自动填充租户信息）
    /// </summary>
    /// <param name="entities">实体列表</param>
    /// <returns>创建的实体数量</returns>
    Task<int> CreateRangeAsync(List<TEntity> entities);

    /// <summary>
    /// 批量创建实体（大批量插入优化，自动填充雪花ID）
    /// </summary>
    /// <param name="entities">实体列表</param>
    /// <returns>创建的实体数量</returns>
    Task<int> CreateRangeBulkAsync(List<TEntity> entities);

    // ========================================
    // 更新操作
    // ========================================

    /// <summary>
    /// 更新实体
    /// </summary>
    /// <param name="entity">实体对象</param>
    /// <returns>是否成功</returns>
    Task<bool> UpdateAsync(TEntity entity);

    /// <summary>
    /// 批量更新实体
    /// </summary>
    /// <param name="entities">实体列表</param>
    /// <returns>更新的实体数量</returns>
    Task<int> UpdateRangeAsync(List<TEntity> entities);

    // ========================================
    // 查询操作（不自动添加租户过滤）
    // ========================================

    /// <summary>
    /// 根据ID查询实体（不自动添加租户过滤）
    /// </summary>
    /// <param name="id">实体ID</param>
    /// <returns>实体对象</returns>
    Task<TEntity?> GetByIdAsync(long id);

    /// <summary>
    /// 根据条件查询单个实体（不自动添加租户过滤）
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <returns>实体对象</returns>
    Task<TEntity?> FirstAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// 根据条件查询实体列表（不自动添加租户过滤）
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <returns>实体列表</returns>
    Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// 检查实体是否存在
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <returns>是否存在</returns>
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
}



/// <summary>
/// 种子数据专用仓储接口（公司级实体）
/// 特点：
/// 1. 不依赖 ITaktUserContext（种子数据阶段无 HTTP 上下文）
/// 2. 不自动添加租户/公司过滤（由调用方显式指定）
/// 3. 支持精确控制租户和公司编码
/// 4. 自动填充雪花ID
/// </summary>
/// <typeparam name="TEntity">实体类型（必须继承TaktCompanyEntityBase）</typeparam>
public interface ITaktCompanySeedRepository<TEntity> where TEntity : TaktCompanyEntityBase, new()
{
    /// <summary>
    /// 创建实体（自动填充雪花ID，不自动填充租户信息）
    /// </summary>
    /// <param name="entity">实体对象</param>
    /// <returns>创建的实体</returns>
    Task<TEntity> CreateAsync(TEntity entity);

    /// <summary>
    /// 批量创建实体（自动填充雪花ID，不自动填充租户信息）
    /// </summary>
    /// <param name="entities">实体列表</param>
    /// <returns>创建的实体数量</returns>
    Task<int> CreateRangeAsync(List<TEntity> entities);

    /// <summary>
    /// 批量创建实体（大批量插入优化，自动填充雪花ID）
    /// </summary>
    /// <param name="entities">实体列表</param>
    /// <returns>创建的实体数量</returns>
    Task<int> CreateRangeBulkAsync(List<TEntity> entities);

    /// <summary>
    /// 更新实体
    /// </summary>
    /// <param name="entity">实体对象</param>
    /// <returns>是否成功</returns>
    Task<bool> UpdateAsync(TEntity entity);

    /// <summary>
    /// 批量更新实体
    /// </summary>
    /// <param name="entities">实体列表</param>
    /// <returns>更新的实体数量</returns>
    Task<int> UpdateRangeAsync(List<TEntity> entities);

    /// <summary>
    /// 根据ID查询实体（不自动添加租户过滤）
    /// </summary>
    /// <param name="id">实体ID</param>
    /// <returns>实体对象</returns>
    Task<TEntity?> GetByIdAsync(long id);

    /// <summary>
    /// 根据条件查询单个实体（不自动添加租户过滤）
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <returns>实体对象</returns>
    Task<TEntity?> FirstAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// 根据条件查询实体列表（不自动添加租户过滤）
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <returns>实体列表</returns>
    Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// 检查实体是否存在
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <returns>是否存在</returns>
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
}

/// <summary>
/// 种子数据专用仓储接口（审批级实体）
/// 特点：
/// 1. 不依赖 ITaktUserContext（种子数据阶段无 HTTP 上下文）
/// 2. 不自动添加租户/公司过滤（由调用方显式指定）
/// 3. 支持精确控制租户和公司编码
/// 4. 自动填充雪花ID
/// </summary>
/// <typeparam name="TEntity">实体类型（必须继承 TaktApprovalEntityBase）</typeparam>
public interface ITaktApprovalSeedRepository<TEntity> where TEntity : TaktApprovalEntityBase, new()
{
    /// <summary>
    /// 创建实体（自动填充雪花ID，不自动填充租户信息）
    /// </summary>
    /// <param name="entity">实体对象</param>
    /// <returns>创建的实体</returns>
    Task<TEntity> CreateAsync(TEntity entity);

    /// <summary>
    /// 批量创建实体（自动填充雪花ID，不自动填充租户信息）
    /// </summary>
    /// <param name="entities">实体列表</param>
    /// <returns>创建的实体数量</returns>
    Task<int> CreateRangeAsync(List<TEntity> entities);

    /// <summary>
    /// 批量创建实体（大批量插入优化，自动填充雪花ID）
    /// </summary>
    /// <param name="entities">实体列表</param>
    /// <returns>创建的实体数量</returns>
    Task<int> CreateRangeBulkAsync(List<TEntity> entities);

    /// <summary>
    /// 更新实体
    /// </summary>
    /// <param name="entity">实体对象</param>
    /// <returns>是否成功</returns>
    Task<bool> UpdateAsync(TEntity entity);

    /// <summary>
    /// 批量更新实体
    /// </summary>
    /// <param name="entities">实体列表</param>
    /// <returns>更新的实体数量</returns>
    Task<int> UpdateRangeAsync(List<TEntity> entities);

    /// <summary>
    /// 根据ID查询实体（不自动添加租户过滤）
    /// </summary>
    /// <param name="id">实体ID</param>
    /// <returns>实体对象</returns>
    Task<TEntity?> GetByIdAsync(long id);

    /// <summary>
    /// 根据条件查询单个实体（不自动添加租户过滤）
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <returns>实体对象</returns>
    Task<TEntity?> FirstAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// 根据条件查询实体列表（不自动添加租户过滤）
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <returns>实体列表</returns>
    Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// 检查实体是否存在
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <returns>是否存在</returns>
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
}
