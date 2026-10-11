// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Repositories
// 文件名称：ITaktCompanyRepository.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：公司级仓储接口，过滤 TenantCode + CompanyCode
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Takt.Domain.Entities;

namespace Takt.Domain.Repositories;

/// <summary>
/// 公司级仓储接口
/// 过滤 TenantCode + CompanyCode
/// 适用于：部门、岗位、员工等业务实体
/// </summary>
/// <typeparam name="TEntity">公司级实体类型</typeparam>
public interface ITaktCompanyRepository<TEntity> : ITaktUniqueExistenceRepository<TEntity> where TEntity : TaktCompanyEntityBase, new()
{
    // ========================================
    // 基础查询
    // ========================================

    /// <summary>
    /// 根据ID查询实体
    /// </summary>
    /// <param name="id">实体ID</param>
    /// <param name="asTableName">可选物理表名（年分表路由）</param>
    /// <returns>实体对象</returns>
    Task<TEntity?> GetByIdAsync(long id, string? asTableName = null);

    /// <summary>
    /// 根据条件查询单个实体
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <param name="asTableName">可选物理表名（年分表路由）</param>
    /// <returns>实体对象</returns>
    Task<TEntity?> FirstAsync(Expression<Func<TEntity, bool>> predicate, string? asTableName = null);

    /// <summary>
    /// 查询所有实体（带租户和公司过滤）
    /// </summary>
    /// <returns>实体列表</returns>
    Task<List<TEntity>> GetAllAsync();

    /// <summary>
    /// 根据条件查询列表
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <param name="asTableName">可选物理表名（年分表路由）</param>
    /// <param name="includeSoftDeleted">为 true 时含已软删行（默认仍经读隔离过滤 IsDeleted=0）</param>
    /// <returns>实体列表</returns>
    Task<List<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>> predicate,
        string? asTableName = null,
        bool includeSoftDeleted = false);

    /// <summary>
    /// 根据条件查询列表（带排序）
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <param name="orderBy">排序字段</param>
    /// <param name="isDesc">是否降序</param>
    /// <param name="asTableName">可选物理表名（年分表路由）</param>
    /// <param name="includeSoftDeleted">为 true 时含已软删行（默认仍经读隔离过滤 IsDeleted=0）</param>
    /// <returns>实体列表</returns>
    Task<List<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, object>> orderBy,
        bool isDesc = true,
        string? asTableName = null,
        bool includeSoftDeleted = false);

    /// <summary>
    /// 导出用条件查询（带上限行数上限，防止全表加载 OOM）
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <param name="maxRows">最大行数；为空时使用 <c>Excel:Export:MaxRowsPerRequest</c> 配置</param>
    /// <param name="asTableName">可选物理表名（年分表路由）</param>
    /// <returns>实体列表</returns>
    Task<List<TEntity>> GetListForExportAsync(
        Expression<Func<TEntity, bool>> predicate,
        int? maxRows = null,
        string? asTableName = null);

    // ========================================
    // 分页查询
    // ========================================

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="pageIndex">页码（从1开始）</param>
    /// <param name="pageSize">每页大小</param>
    /// <returns>分页结果</returns>
    Task<(List<TEntity> Items, int Total)> GetPagedAsync(int pageIndex, int pageSize);

    /// <summary>
    /// 分页查询（带条件，默认按 CreatedAt 降序）
    /// </summary>
    /// <param name="pageIndex">页码（从1开始）</param>
    /// <param name="pageSize">每页大小</param>
    /// <param name="predicate">查询条件</param>
    /// <returns>分页结果</returns>
    Task<(List<TEntity> Items, int Total)> GetPagedAsync(
        int pageIndex,
        int pageSize,
        Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// 分页查询（带条件）
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <param name="pageIndex">页码（从1开始）</param>
    /// <param name="pageSize">每页大小</param>
    /// <param name="orderBy">排序字段</param>
    /// <param name="isDesc">是否降序</param>
    /// <param name="asTableName">年分表物理名；空则用实体默认表</param>
    /// <param name="thenBy">第二排序字段（在 orderBy 之后）</param>
    /// <param name="thenByDesc">第二排序是否降序</param>
    /// <returns>分页结果</returns>
    Task<(List<TEntity> Items, int Total)> GetPagedAsync(
        Expression<Func<TEntity, bool>> predicate,
        int pageIndex,
        int pageSize,
        Expression<Func<TEntity, object>>? orderBy = null,
        bool isDesc = true,
        string? asTableName = null,
        Expression<Func<TEntity, object>>? thenBy = null,
        bool thenByDesc = false);

    // ========================================
    // 新增操作
    // ========================================

    /// <summary>
    /// 创建实体
    /// </summary>
    /// <param name="entity">实体对象</param>
    /// <param name="asTableName">可选物理表名（年分表路由，如 takt_xxx_2026）</param>
    /// <returns>创建的实体</returns>
    Task<TEntity> CreateAsync(TEntity entity, string? asTableName = null);

    /// <summary>
    /// 批量创建实体
    /// </summary>
    /// <param name="entities">实体列表</param>
    /// <returns>创建的实体数量</returns>
    Task<int> CreateRangeAsync(List<TEntity> entities);

    // ========================================
    // 更新操作
    // ========================================

    /// <summary>
    /// 更新实体
    /// </summary>
    /// <param name="entity">实体对象</param>
    /// <param name="asTableName">可选物理表名（年分表路由）</param>
    /// <returns>是否成功</returns>
    Task<bool> UpdateAsync(TEntity entity, string? asTableName = null);

    /// <summary>
    /// 批量更新实体
    /// </summary>
    /// <param name="entities">实体列表</param>
    /// <param name="asTableName">可选物理表名（年分表路由）</param>
    /// <returns>更新的实体数量</returns>
    Task<int> UpdateRangeAsync(List<TEntity> entities, string? asTableName = null);

    /// <summary>
    /// 批量更新指定列（大数据回写外键等；禁止全列 UpdateRange）
    /// </summary>
    /// <param name="entities">实体列表</param>
    /// <param name="columnNames">属性名（PascalCase，与实体属性一致）</param>
    /// <returns>更新行数</returns>
    Task<int> UpdateRangeColumnsAsync(List<TEntity> entities, params string[] columnNames);

    /// <summary>
    /// 根据条件更新
    /// </summary>
    /// <param name="predicate">更新条件</param>
    /// <param name="columns">更新字段</param>
    /// <returns>更新的实体数量</returns>
    Task<int> UpdateAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> columns);

    // ========================================
    // 删除操作
    // ========================================

    /// <summary>
    /// 软删除实体
    /// </summary>
    /// <param name="id">实体ID</param>
    /// <param name="asTableName">可选物理表名（年分表路由）</param>
    /// <returns>是否成功</returns>
    Task<bool> DeleteAsync(long id, string? asTableName = null);

    /// <summary>
    /// 根据条件软删除
    /// </summary>
    /// <param name="predicate">删除条件</param>
    /// <returns>删除的实体数量</returns>
    Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// 按条件物理删除（含已软删行；用于关联表全量覆盖，避免唯一索引冲突）
    /// </summary>
    /// <param name="predicate">删除条件</param>
    /// <returns>删除行数</returns>
    Task<int> DeletePhysicallyAsync(Expression<Func<TEntity, bool>> predicate);

    // ========================================
    // 存在性检查
    // ========================================

    /// <summary>
    /// 检查实体是否存在
    /// </summary>
    /// <param name="id">实体ID</param>
    /// <returns>是否存在</returns>
    Task<bool> ExistsAsync(long id);

    /// <summary>
    /// 统计符合条件的记录数
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <returns>记录数</returns>
    Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null);

    // ========================================
    // 聚合统计
    // ========================================

    /// <summary>
    /// 取字段最大值（当前租户与公司范围内、未删除）
    /// </summary>
    /// <param name="fieldSelector">聚合字段</param>
    /// <param name="predicate">查询条件</param>
    /// <returns>最大值；无记录时为类型默认值</returns>
    Task<TResult> MaxAsync<TResult>(Expression<Func<TEntity, TResult>> fieldSelector, Expression<Func<TEntity, bool>>? predicate = null);

    /// <summary>
    /// 取字段最小值（当前租户与公司范围内、未删除）
    /// </summary>
    /// <param name="fieldSelector">聚合字段</param>
    /// <param name="predicate">查询条件</param>
    /// <returns>最小值；无记录时为类型默认值</returns>
    Task<TResult> MinAsync<TResult>(Expression<Func<TEntity, TResult>> fieldSelector, Expression<Func<TEntity, bool>>? predicate = null);

    /// <summary>
    /// 求字段之和（当前租户与公司范围内、未删除）
    /// </summary>
    /// <param name="fieldSelector">聚合字段</param>
    /// <param name="predicate">查询条件</param>
    /// <returns>求和结果；无记录时为类型默认值</returns>
    Task<TResult> SumAsync<TResult>(Expression<Func<TEntity, TResult>> fieldSelector, Expression<Func<TEntity, bool>>? predicate = null) where TResult : struct;

    /// <summary>
    /// 求字段平均值（当前租户与公司范围内、未删除）
    /// </summary>
    /// <param name="fieldSelector">聚合字段</param>
    /// <param name="predicate">查询条件</param>
    /// <returns>平均值；无记录时为类型默认值</returns>
    Task<TResult> AvgAsync<TResult>(Expression<Func<TEntity, TResult>> fieldSelector, Expression<Func<TEntity, bool>>? predicate = null) where TResult : struct;

    /// <summary>
    /// 求字段中位数（当前租户与公司范围内、未删除；SqlServer/PostgreSQL/Oracle 等走 PERCENTILE_CONT，MySql/Sqlite 有序切片回退）
    /// </summary>
    /// <param name="fieldSelector">聚合字段</param>
    /// <param name="predicate">查询条件</param>
    /// <returns>中位数；无记录时为类型默认值</returns>
    Task<TResult> MedianAsync<TResult>(Expression<Func<TEntity, TResult>> fieldSelector, Expression<Func<TEntity, bool>>? predicate = null) where TResult : struct;

    // ========================================
    // 物理表与序列、只读脚本
    // ========================================

    /// <summary>
    /// 判断当前库是否存在指定物理表（年分表探测用；不含租户过滤）
    /// </summary>
    /// <param name="tableName">物理表名</param>
    /// <returns>存在为 true</returns>
    Task<bool> PhysicalTableExistsAsync(string tableName);

    /// <summary>
    /// 按条件取整型字段最大值（当前租户与公司范围内；默认未删除）
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <param name="fieldSelector">整型字段（如 SortOrder、行号字段）</param>
    /// <param name="includeSoftDeleted">为 true 时含已软删行（行号分配等唯一索引不区分删除态）</param>
    /// <returns>最大值；无记录时为 0</returns>
    Task<int> GetMaxIntAsync(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, int>> fieldSelector,
        bool includeSoftDeleted = false);

    /// <summary>
    /// 执行只读 SQL 并返回动态行（调用方须先经 TaktSqlExecutorValidator 校验）
    /// </summary>
    /// <param name="sql">SQL 文本</param>
    /// <param name="parameters">命名参数（可选）</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>结果行列表</returns>
    Task<IReadOnlyList<Dictionary<string, object>>> QueryReadOnlySqlAsync(
        string sql,
        IReadOnlyDictionary<string, object?>? parameters = null,
        CancellationToken cancellationToken = default);

    // ========================================
    // 事务操作
    // ========================================

    /// <summary>
    /// 开启事务
    /// </summary>
    void BeginTran();

    /// <summary>
    /// 提交事务
    /// </summary>
    void CommitTran();

    /// <summary>
    /// 回滚事务
    /// </summary>
    void RollbackTran();
}
