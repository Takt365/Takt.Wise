// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Repositories
// 文件名称：TaktCompanyRepository.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：公司级仓储实现，过滤 TenantCode + CompanyCode
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using SqlSugar;
using Takt.Domain.Entities;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Infrastructure.Data.Context;
using Takt.Infrastructure.Extensions;
using Microsoft.Extensions.Options;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Repositories;

/// <summary>
/// 公司级仓储实现
/// 过滤 TenantCode + CompanyCode
/// 适用于：部门、岗位、员工等业务实体
/// </summary>
/// <typeparam name="TEntity">公司级实体类型</typeparam>
public class TaktCompanyRepository<TEntity> : ITaktCompanyRepository<TEntity> where TEntity : TaktCompanyEntityBase, new()
{
    /// <summary>
    /// SqlSugar 数据库上下文
    /// </summary>
    private readonly TaktSqlSugarContext _dbContext;

    /// <summary>
    /// 用户上下文
    /// </summary>
    private readonly ITaktUserContext _userContext;

    /// <summary>
    /// 主键类型配置
    /// </summary>
    private readonly PrimaryKeyTypeOptions _primaryKeyTypeOptions;

    /// <summary>
    /// Excel 导入导出配置
    /// </summary>
    private readonly TaktExcelOptions _excelOptions;

    /// <summary>
    /// Database 配置（CompanyCodes↔PlantCodes 同序映射）
    /// </summary>
    private readonly TaktDatabaseOptions _database;

    /// <summary>
    /// 当前租户编码
    /// </summary>
    protected string CurrentTenantCode => _userContext.TenantCode ?? string.Empty;

    /// <summary>
    /// 当前公司编码
    /// </summary>
    protected string CurrentCompanyCode => _userContext.CompanyCode ?? string.Empty;

    /// <summary>
    /// 当前用户ID
    /// </summary>
    protected long? CurrentUserId => _userContext.UserId;

    /// <summary>
    /// 数据库客户端
    /// </summary>
    protected ISqlSugarClient Db => _dbContext.Db;

    /// <summary>
    /// 是否按当前会话公司编码过滤（关联表如用户-公司应设为 false）
    /// </summary>
    protected virtual bool UseSessionCompanyFilter => true;

    /// <summary>
    /// 是否按当前会话租户编码过滤（用户-公司等关联表应设为 false，由查询条件显式指定租户）
    /// </summary>
    protected virtual bool UseSessionTenantFilter => true;

    /// <summary>
    /// 应用租户（及可选会话公司）读隔离
    /// </summary>
    /// <param name="query">查询</param>
    /// <returns>附加隔离条件后的查询</returns>
    protected ISugarQueryable<TEntity> ApplyReadScope(ISugarQueryable<TEntity> query)
    {
        if (UseSessionTenantFilter)
        {
            query = query.Where(x => x.TenantCode == CurrentTenantCode);
        }

        if (UseSessionCompanyFilter)
        {
            query = query.Where(x => x.CompanyCode == CurrentCompanyCode);
        }

        return query.Where(x => x.IsDeleted == 0);
    }

    /// <summary>
    /// 应用租户（及可选会话公司）写隔离
    /// </summary>
    /// <param name="query">查询</param>
    /// <returns>附加隔离条件后的查询</returns>
    protected ISugarQueryable<TEntity> ApplyWriteScope(ISugarQueryable<TEntity> query)
    {
        if (UseSessionTenantFilter)
        {
            query = query.Where(x => x.TenantCode == CurrentTenantCode);
        }

        if (UseSessionCompanyFilter)
        {
            query = query.Where(x => x.CompanyCode == CurrentCompanyCode);
        }

        return query;
    }

    /// <summary>
    /// 应用租户（及可选会话公司）写隔离（Updateable）
    /// </summary>
    /// <param name="update">更新构造器</param>
    /// <returns>附加隔离条件后的更新构造器</returns>
    protected IUpdateable<TEntity> ApplyWriteScope(IUpdateable<TEntity> update)
    {
        if (UseSessionTenantFilter)
        {
            update = update.Where(x => x.TenantCode == CurrentTenantCode);
        }

        if (UseSessionCompanyFilter)
        {
            update = update.Where(x => x.CompanyCode == CurrentCompanyCode);
        }

        return update;
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="dbContext">数据库上下文</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="primaryKeyTypeOptions">主键类型配置</param>
    /// <param name="excelOptions">Excel 导入导出配置</param>
    /// <param name="databaseOptions">Database 配置</param>
    public TaktCompanyRepository(
        TaktSqlSugarContext dbContext,
        ITaktUserContext userContext,
        IOptions<PrimaryKeyTypeOptions> primaryKeyTypeOptions,
        IOptions<TaktExcelOptions> excelOptions,
        IOptions<TaktDatabaseOptions> databaseOptions)
    {
        _dbContext = dbContext;
        _userContext = userContext;
        _primaryKeyTypeOptions = primaryKeyTypeOptions.Value;
        _excelOptions = excelOptions.Value;
        _database = databaseOptions.Value;
        _database.NormalizeAndValidate();
    }

    // ========================================
    // 基础查询
    // ========================================

    /// <summary>
    /// 构造带租户/公司范围的查询（可切换物理表）
    /// </summary>
    /// <param name="asTableName">年分表物理名；空则用实体默认表</param>
    /// <returns>可查询对象</returns>
    private ISugarQueryable<TEntity> CreateScopedQuery(string? asTableName = null)
    {
        var query = string.IsNullOrWhiteSpace(asTableName)
            ? Db.Queryable<TEntity>()
            : Db.Queryable<TEntity>().AS(asTableName.Trim());
        return ApplyReadScope(query);
    }

    /// <summary>
    /// 根据ID查询实体
    /// </summary>
    public virtual async Task<TEntity?> GetByIdAsync(long id, string? asTableName = null)
    {
        return await CreateScopedQuery(asTableName).Where(x => x.Id == id).FirstAsync();
    }

    /// <summary>
    /// 根据条件查询单个实体
    /// </summary>
    public virtual async Task<TEntity?> FirstAsync(Expression<Func<TEntity, bool>> predicate, string? asTableName = null)
    {
        return await CreateScopedQuery(asTableName).Where(predicate).FirstAsync();
    }

    /// <summary>
    /// 查询所有实体（带租户和公司过滤）
    /// </summary>
    public virtual async Task<List<TEntity>> GetAllAsync()
    {
        return await CreateScopedQuery()
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// 根据条件查询列表
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <param name="asTableName">可选物理表名（年分表路由）</param>
    /// <param name="includeSoftDeleted">为 true 时含已软删行（仅租户/公司隔离，不过滤 IsDeleted）</param>
    /// <returns>实体列表</returns>
    public virtual async Task<List<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>> predicate,
        string? asTableName = null,
        bool includeSoftDeleted = false)
    {
        return await CreateListQuery(asTableName, includeSoftDeleted)
            .Where(predicate)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// 根据条件查询列表（带排序）
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <param name="orderBy">排序字段</param>
    /// <param name="isDesc">是否降序</param>
    /// <param name="asTableName">可选物理表名（年分表路由）</param>
    /// <param name="includeSoftDeleted">为 true 时含已软删行（仅租户/公司隔离，不过滤 IsDeleted）</param>
    /// <returns>实体列表</returns>
    public virtual async Task<List<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, object>> orderBy,
        bool isDesc = true,
        string? asTableName = null,
        bool includeSoftDeleted = false)
    {
        var query = CreateListQuery(asTableName, includeSoftDeleted).Where(predicate);
        return isDesc
            ? await query.OrderByDescending(orderBy).ToListAsync()
            : await query.OrderBy(orderBy).ToListAsync();
    }

    /// <summary>
    /// 列表查询：默认读隔离（含 IsDeleted=0）；includeSoftDeleted 时仅租户/公司写隔离
    /// </summary>
    /// <param name="asTableName">可选物理表名</param>
    /// <param name="includeSoftDeleted">是否含已软删</param>
    /// <returns>可查询对象</returns>
    private ISugarQueryable<TEntity> CreateListQuery(string? asTableName, bool includeSoftDeleted)
    {
        var query = string.IsNullOrWhiteSpace(asTableName)
            ? Db.Queryable<TEntity>()
            : Db.Queryable<TEntity>().AS(asTableName.Trim());
        return includeSoftDeleted ? ApplyWriteScope(query) : ApplyReadScope(query);
    }

    /// <summary>
    /// 导出用条件查询（带上限行数上限，防止全表加载 OOM）
    /// 经 ApplyReadScope 过滤租户/公司与未删除记录，按 CreatedAt 降序后 Take 截断
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <param name="maxRows">最大行数；为空时使用 Excel:Export:MaxRowsPerRequest 配置</param>
    /// <param name="asTableName">年分表物理名；空则用实体默认表</param>
    /// <returns>不超过上限的实体列表</returns>
    /// <exception cref="ArgumentOutOfRangeException">maxRows 小于等于 0 时抛出</exception>
    public virtual async Task<List<TEntity>> GetListForExportAsync(
        Expression<Func<TEntity, bool>> predicate,
        int? maxRows = null,
        string? asTableName = null)
    {
        var take = maxRows ?? _excelOptions.Export.MaxRowsPerRequest;
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(take);
        return await CreateScopedQuery(asTableName).Where(predicate)
            .OrderByDescending(x => x.CreatedAt)
            .Take(take)
            .ToListAsync();
    }

    // ========================================
    // 分页查询
    // ========================================

    /// <summary>
    /// 分页查询
    /// </summary>
    public virtual async Task<(List<TEntity> Items, int Total)> GetPagedAsync(int pageIndex, int pageSize)
    {
        pageIndex = TaktPagedClamp.NormalizePageIndex(pageIndex);
        pageSize = TaktPagedClamp.NormalizePageSize(pageSize);
        var filterQuery = ApplyReadScope(Db.Queryable<TEntity>());

        var total = await filterQuery.CountAsync();
        var items = await filterQuery
            .OrderByDescending(x => x.CreatedAt)
            .Skip(TaktPagedClamp.ComputeSkip(pageIndex, pageSize))
            .Take(pageSize)
            .ToListAsync();

        return (items, total);
    }

    /// <summary>
    /// 分页查询（带条件，默认按 CreatedAt 降序）
    /// </summary>
    public virtual Task<(List<TEntity> Items, int Total)> GetPagedAsync(
        int pageIndex,
        int pageSize,
        Expression<Func<TEntity, bool>> predicate) =>
        GetPagedAsync(predicate, pageIndex, pageSize, x => x.CreatedAt, isDesc: true);

    /// <summary>
    /// 分页查询（带条件）
    /// </summary>
    public virtual async Task<(List<TEntity> Items, int Total)> GetPagedAsync(
        Expression<Func<TEntity, bool>> predicate,
        int pageIndex,
        int pageSize,
        Expression<Func<TEntity, object>>? orderBy = null,
        bool isDesc = true,
        string? asTableName = null,
        Expression<Func<TEntity, object>>? thenBy = null,
        bool thenByDesc = false)
    {
        pageIndex = TaktPagedClamp.NormalizePageIndex(pageIndex);
        pageSize = TaktPagedClamp.NormalizePageSize(pageSize);
        var filterQuery = CreateScopedQuery(asTableName).Where(predicate);

        var total = await filterQuery.CountAsync();

        var pageQuery = filterQuery;
        if (orderBy != null)
        {
            pageQuery = isDesc ? pageQuery.OrderByDescending(orderBy) : pageQuery.OrderBy(orderBy);
            if (thenBy != null)
            {
                pageQuery = thenByDesc ? pageQuery.OrderByDescending(thenBy) : pageQuery.OrderBy(thenBy);
            }
        }
        else
        {
            pageQuery = pageQuery.OrderByDescending(x => x.CreatedAt);
        }

        var items = await pageQuery
            .Skip(TaktPagedClamp.ComputeSkip(pageIndex, pageSize))
            .Take(pageSize)
            .ToListAsync();

        return (items, total);
    }

    // ========================================
    // 新增操作
    // ========================================

    /// <summary>
    /// 创建实体
    /// </summary>
    public virtual async Task<TEntity> CreateAsync(TEntity entity, string? asTableName = null)
    {
        // 自动设置租户和公司编码(仅在未设置时才自动填充)
        if (string.IsNullOrEmpty(entity.TenantCode))
        {
            entity.TenantCode = CurrentTenantCode;
        }

        if (string.IsNullOrEmpty(entity.CompanyCode))
        {
            entity.CompanyCode = CurrentCompanyCode;
        }

        await TaktCompanyScopeFillHelper.ApplyCompanyScopeFromMasterAsync(
            Db,
            entity,
            entity.TenantCode,
            entity.CompanyCode,
            _database);

        // 自动设置审计字段
        entity.ApplyCreate(CurrentUserId);

        if (string.IsNullOrWhiteSpace(asTableName))
        {
            await TaktPrimaryKeyInsertHelper.InsertEntityAsync(Db, entity, _primaryKeyTypeOptions);
        }
        else
        {
            await TaktPrimaryKeyInsertHelper.InsertEntityAsync(Db, entity, _primaryKeyTypeOptions, asTableName.Trim());
        }

        return entity;
    }

    /// <summary>
    /// 批量创建实体（公司/工厂/文化码只解析一次；插入由 InsertEntitiesAsync 自动分批）
    /// </summary>
    /// <param name="entities">实体列表</param>
    /// <returns>插入行数</returns>
    public virtual async Task<int> CreateRangeAsync(List<TEntity> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);
        if (entities.Count == 0)
        {
            return 0;
        }

        var now = DateTime.Now;
        var defaultTenant = CurrentTenantCode;
        var defaultCompany = CurrentCompanyCode;
        _database.NormalizeAndValidate();
        // 同批通常同公司：先解析映射，避免 10 万次 await Task.CompletedTask
        string? mappedCulture = null;
        string? mappedPlant = null;
        if (!string.IsNullOrWhiteSpace(defaultTenant) && !string.IsNullOrWhiteSpace(defaultCompany))
        {
            mappedCulture = _database.GetCultureCodeForCompanyCode(defaultCompany);
            mappedPlant = _database.GetPlantCodeForCompanyCode(defaultCompany);
        }

        foreach (var entity in entities)
        {
            if (string.IsNullOrEmpty(entity.TenantCode))
            {
                entity.TenantCode = defaultTenant;
            }

            if (string.IsNullOrEmpty(entity.CompanyCode))
            {
                entity.CompanyCode = defaultCompany;
            }

            if (string.Equals(entity.CompanyCode, defaultCompany, StringComparison.Ordinal)
                && mappedCulture != null)
            {
                TaktCompanyScopeFillHelper.ApplyCompanyScope(entity, mappedCulture, mappedPlant ?? string.Empty);
            }
            else
            {
                await TaktCompanyScopeFillHelper.ApplyCompanyScopeFromMasterAsync(
                    Db,
                    entity,
                    entity.TenantCode,
                    entity.CompanyCode,
                    _database);
            }

            entity.ApplyCreate(CurrentUserId, now);
        }

        return await TaktPrimaryKeyInsertHelper.InsertEntitiesAsync(Db, entities, _primaryKeyTypeOptions);
    }

    // ========================================
    // 更新操作
    // ========================================

    /// <summary>
    /// 更新实体
    /// </summary>
    public virtual async Task<bool> UpdateAsync(TEntity entity, string? asTableName = null)
    {
        // 自动设置审计字段
        entity.ApplyUpdate(CurrentUserId);

        var updateable = string.IsNullOrWhiteSpace(asTableName)
            ? Db.Updateable(entity)
            : Db.Updateable(entity).AS(asTableName.Trim());
        var rows = await updateable.ExecuteCommandAsync();
        return rows > 0;
    }

    /// <summary>
    /// 批量更新实体
    /// </summary>
    /// <param name="entities">实体列表</param>
    /// <param name="asTableName">可选物理表名（年分表路由）</param>
    /// <returns>更新的实体数量</returns>
    public virtual async Task<int> UpdateRangeAsync(List<TEntity> entities, string? asTableName = null)
    {
        if (entities == null || entities.Count == 0)
        {
            return 0;
        }
        var now = DateTime.Now;
        foreach (var entity in entities)
        {
            entity.ApplyUpdate(CurrentUserId, now);
        }

        // 大批量走 BulkUpdate（Updateable 十万行会极慢/超时）
        if (string.IsNullOrWhiteSpace(asTableName) && entities.Count >= TaktPrimaryKeyInsertHelper.BulkCopyThreshold)
        {
            const int chunk = TaktPrimaryKeyInsertHelper.DefaultInsertBatchSize;
            var total = 0;
            for (var offset = 0; offset < entities.Count; offset = checked(offset + chunk))
            {
                var take = Math.Min(chunk, entities.Count - offset);
                var batch = entities.GetRange(offset, take);
                var n = await Db.Fastest<TEntity>().BulkUpdateAsync(batch);
                total = checked(total + (n > 0 ? n : take));
            }
            return total;
        }

        var updateable = string.IsNullOrWhiteSpace(asTableName)
            ? Db.Updateable(entities)
            : Db.Updateable(entities).AS(asTableName.Trim());
        return await updateable.ExecuteCommandAsync();
    }

    /// <summary>
    /// 批量更新指定列（大数据回写外键；走 BulkUpdate 指定列）
    /// </summary>
    /// <param name="entities">实体列表</param>
    /// <param name="columnNames">属性名</param>
    /// <returns>更新行数</returns>
    public virtual async Task<int> UpdateRangeColumnsAsync(List<TEntity> entities, params string[] columnNames)
    {
        ArgumentNullException.ThrowIfNull(entities);
        ArgumentNullException.ThrowIfNull(columnNames);
        if (entities.Count == 0 || columnNames.Length == 0)
        {
            return 0;
        }

        var now = DateTime.Now;
        foreach (var entity in entities)
        {
            entity.ApplyUpdate(CurrentUserId, now);
        }

        var cols = new List<string>(columnNames.Length + 2);
        foreach (var name in columnNames)
        {
            if (!string.IsNullOrWhiteSpace(name) && !cols.Contains(name, StringComparer.Ordinal))
            {
                cols.Add(name.Trim());
            }
        }
        if (!cols.Contains(nameof(TaktCompanyEntityBase.UpdatedBy), StringComparer.Ordinal))
        {
            cols.Add(nameof(TaktCompanyEntityBase.UpdatedBy));
        }
        if (!cols.Contains(nameof(TaktCompanyEntityBase.UpdatedAt), StringComparer.Ordinal))
        {
            cols.Add(nameof(TaktCompanyEntityBase.UpdatedAt));
        }

        var colArray = cols.ToArray();
        if (entities.Count >= TaktPrimaryKeyInsertHelper.BulkCopyThreshold)
        {
            const int chunk = TaktPrimaryKeyInsertHelper.DefaultInsertBatchSize;
            var total = 0;
            for (var offset = 0; offset < entities.Count; offset = checked(offset + chunk))
            {
                var take = Math.Min(chunk, entities.Count - offset);
                var batch = entities.GetRange(offset, take);
                var n = await Db.Fastest<TEntity>().BulkUpdateAsync(batch, colArray);
                total = checked(total + (n > 0 ? n : take));
            }
            return total;
        }

        return await Db.Updateable(entities).UpdateColumns(colArray).ExecuteCommandAsync();
    }

    /// <summary>
    /// 根据条件更新
    /// </summary>
    public virtual async Task<int> UpdateAsync(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TEntity>> columns)
    {
        return await ApplyWriteScope(Db.Updateable<TEntity>()
            .SetColumns(columns)
            .Where(predicate))
            .ExecuteCommandAsync();
    }

    // ========================================
    // 删除操作
    // ========================================

    /// <summary>
    /// 软删除实体
    /// </summary>
    public virtual async Task<bool> DeleteAsync(long id, string? asTableName = null)
    {
        var entity = await GetByIdAsync(id, asTableName);
        if (entity == null)
        {
            return false;
        }
        var isBuiltIn = entity.GetType().GetProperty("IsBuiltIn")?.GetValue(entity);
        if (isBuiltIn is int builtInFlag && builtInFlag == 1)
        {
            return false;
        }
        var now = DateTime.Now;
        var operatorUserId = TaktEntityAuditExtensions.ResolveOperatorUserId(CurrentUserId);
        var updateable = string.IsNullOrWhiteSpace(asTableName)
            ? Db.Updateable<TEntity>()
            : Db.Updateable<TEntity>().AS(asTableName.Trim());
        var rows = await ApplyWriteScope(updateable
            .SetColumns(x => new TEntity
            {
                IsDeleted = 1,
                UpdatedAt = now,
                UpdatedBy = operatorUserId,
                DeletedAt = now,
                DeletedBy = operatorUserId
            })
            .Where(x => x.Id == id && x.IsDeleted == 0))
            .ExecuteCommandAsync();

        return rows > 0;
    }

    /// <summary>
    /// 根据条件软删除
    /// </summary>
    public virtual async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var now = DateTime.Now;
        var operatorUserId = TaktEntityAuditExtensions.ResolveOperatorUserId(CurrentUserId);
        return await ApplyWriteScope(Db.Updateable<TEntity>()
            .SetColumns(x => new TEntity
            {
                IsDeleted = 1,
                UpdatedAt = now,
                UpdatedBy = operatorUserId,
                DeletedAt = now,
                DeletedBy = operatorUserId
            })
            .Where(predicate)
            .Where(x => x.IsDeleted == 0))
            .ExecuteCommandAsync();
    }

    /// <summary>
    /// 按条件物理删除（含已软删行；用于关联表全量覆盖，避免唯一索引冲突）
    /// </summary>
    /// <param name="predicate">删除条件</param>
    /// <returns>删除行数</returns>
    public virtual async Task<int> DeletePhysicallyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var delete = Db.Deleteable<TEntity>().Where(predicate);
        if (UseSessionTenantFilter)
        {
            delete = delete.Where(x => x.TenantCode == CurrentTenantCode);
        }

        if (UseSessionCompanyFilter)
        {
            delete = delete.Where(x => x.CompanyCode == CurrentCompanyCode);
        }

        return await delete.ExecuteCommandAsync();
    }

    // ========================================
    // 存在性检查
    // ========================================

    /// <summary>
    /// 检查实体是否存在
    /// </summary>
    public virtual async Task<bool> ExistsAsync(long id)
    {
        return await ApplyReadScope(Db.Queryable<TEntity>().Where(x => x.Id == id)).AnyAsync();
    }

    /// <summary>
    /// 根据条件检查是否存在
    /// </summary>
    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await ApplyReadScope(Db.Queryable<TEntity>().Where(predicate)).AnyAsync();
    }

    /// <summary>
    /// 统计符合条件的记录数
    /// </summary>
    public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null)
    {
        var query = ApplyWriteScope(Db.Queryable<TEntity>());

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        return await query.Where(x => x.IsDeleted == 0).CountAsync();
    }

    // ========================================
    // 聚合统计
    // ========================================

    /// <summary>
    /// 构建聚合读查询（租户/公司隔离、未删除）
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <returns>可聚合的查询</returns>
    private ISugarQueryable<TEntity> BuildAggregateReadQuery(Expression<Func<TEntity, bool>>? predicate)
    {
        var query = ApplyReadScope(Db.Queryable<TEntity>());
        if (predicate != null)
        {
            query = query.Where(predicate);
        }
        return query;
    }

    /// <summary>
    /// 取字段最大值（当前租户与公司范围内、未删除）
    /// </summary>
    /// <param name="fieldSelector">聚合字段</param>
    /// <param name="predicate">查询条件</param>
    /// <returns>最大值；无记录时为类型默认值</returns>
    public virtual Task<TResult> MaxAsync<TResult>(
        Expression<Func<TEntity, TResult>> fieldSelector,
        Expression<Func<TEntity, bool>>? predicate = null) =>
        BuildAggregateReadQuery(predicate).MaxAsync(fieldSelector);

    /// <summary>
    /// 取字段最小值（当前租户与公司范围内、未删除）
    /// </summary>
    /// <param name="fieldSelector">聚合字段</param>
    /// <param name="predicate">查询条件</param>
    /// <returns>最小值；无记录时为类型默认值</returns>
    public virtual Task<TResult> MinAsync<TResult>(
        Expression<Func<TEntity, TResult>> fieldSelector,
        Expression<Func<TEntity, bool>>? predicate = null) =>
        BuildAggregateReadQuery(predicate).MinAsync(fieldSelector);

    /// <summary>
    /// 求字段之和（当前租户与公司范围内、未删除）
    /// </summary>
    /// <param name="fieldSelector">聚合字段</param>
    /// <param name="predicate">查询条件</param>
    /// <returns>求和结果；无记录时为类型默认值</returns>
    public virtual Task<TResult> SumAsync<TResult>(
        Expression<Func<TEntity, TResult>> fieldSelector,
        Expression<Func<TEntity, bool>>? predicate = null) where TResult : struct =>
        BuildAggregateReadQuery(predicate).SumAsync(fieldSelector);

    /// <summary>
    /// 求字段平均值（当前租户与公司范围内、未删除）
    /// </summary>
    /// <param name="fieldSelector">聚合字段</param>
    /// <param name="predicate">查询条件</param>
    /// <returns>平均值；无记录时为类型默认值</returns>
    public virtual Task<TResult> AvgAsync<TResult>(
        Expression<Func<TEntity, TResult>> fieldSelector,
        Expression<Func<TEntity, bool>>? predicate = null) where TResult : struct =>
        BuildAggregateReadQuery(predicate).AvgAsync(fieldSelector);

    /// <summary>
    /// 求字段中位数（当前租户与公司范围内、未删除；SqlServer/PostgreSQL/Oracle 等走 PERCENTILE_CONT，MySql/Sqlite 有序切片回退）
    /// </summary>
    /// <param name="fieldSelector">聚合字段</param>
    /// <param name="predicate">查询条件</param>
    /// <returns>中位数；无记录时为类型默认值</returns>
    public virtual Task<TResult> MedianAsync<TResult>(
        Expression<Func<TEntity, TResult>> fieldSelector,
        Expression<Func<TEntity, bool>>? predicate = null) where TResult : struct =>
        TaktRepositoryAggregateSql.MedianAsync(Db, BuildAggregateReadQuery(predicate), fieldSelector);

    // ========================================
    // 序列与只读脚本
    // ========================================

    /// <summary>
    /// 按条件取整型字段最大值（当前租户与公司范围内；默认未删除）
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <param name="fieldSelector">整型字段</param>
    /// <param name="includeSoftDeleted">为 true 时含已软删行（行号分配等唯一索引不区分删除态）</param>
    /// <returns>最大值；无记录时为 0</returns>
    public virtual async Task<int> GetMaxIntAsync(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, int>> fieldSelector,
        bool includeSoftDeleted = false)
    {
        if (!includeSoftDeleted)
        {
            return await MaxAsync(fieldSelector, predicate);
        }
        var query = ApplyWriteScope(Db.Queryable<TEntity>()).Where(predicate);
        if (!await query.AnyAsync())
        {
            return 0;
        }
        return await query.MaxAsync(fieldSelector);
    }

    /// <summary>
    /// 判断当前库是否存在指定物理表（年分表探测用；不含租户过滤）
    /// </summary>
    /// <param name="tableName">物理表名</param>
    /// <returns>存在为 true</returns>
    public virtual Task<bool> PhysicalTableExistsAsync(string tableName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tableName);
        var name = tableName.Trim();
        return Task.FromResult(Db.DbMaintenance.IsAnyTable(name, false));
    }

    /// <summary>
    /// 执行只读 SQL 并返回动态行
    /// </summary>
    /// <param name="sql">SQL 文本</param>
    /// <param name="parameters">命名参数（可选）</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>结果行列表</returns>
    public virtual Task<IReadOnlyList<Dictionary<string, object>>> QueryReadOnlySqlAsync(
        string sql,
        IReadOnlyDictionary<string, object?>? parameters = null,
        CancellationToken cancellationToken = default) =>
        TaktRepositoryReadOnlySql.QueryAsync(Db, sql, parameters, cancellationToken);

    // ========================================
    // 事务管理
    // ========================================

    /// <summary>
    /// 开始事务
    /// </summary>
    public void BeginTran()
    {
        Db.Ado.BeginTran();
    }

    /// <summary>
    /// 提交事务
    /// </summary>
    public void CommitTran()
    {
        Db.Ado.CommitTran();
    }

    /// <summary>
    /// 回滚事务
    /// </summary>
    public void RollbackTran()
    {
        Db.Ado.RollbackTran();
    }
}
