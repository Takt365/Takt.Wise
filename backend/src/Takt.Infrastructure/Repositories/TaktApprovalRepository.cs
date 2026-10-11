// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Repositories
// 文件名称：TaktApprovalRepository.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：审批仓储实现，独立仓储，提供审批操作
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
using Takt.Shared.Enums;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Repositories;

/// <summary>
/// 审批仓储实现（独立仓储，不继承其他仓储）
/// 适用于：请假单、报销单、采购单、合同等需要审批的实体
/// 注意：审批实体使用 TaktApprovalEntityBase 基类，包含 TenantCode 和 CompanyCode 字段
/// </summary>
/// <typeparam name="TEntity">审批实体类型</typeparam>
public class TaktApprovalRepository<TEntity> : ITaktApprovalRepository<TEntity> where TEntity : TaktApprovalEntityBase, new()
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
    /// 构造函数
    /// </summary>
    /// <param name="dbContext">数据库上下文</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="primaryKeyTypeOptions">主键类型配置</param>
    /// <param name="excelOptions">Excel 导入导出配置</param>
    /// <param name="databaseOptions">Database 配置</param>
    public TaktApprovalRepository(
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
    /// 根据ID查询实体
    /// </summary>
    public virtual async Task<TEntity?> GetByIdAsync(long id)
    {
        return await Db.Queryable<TEntity>()
            .Where(x => x.Id == id)
            .Where(x => x.TenantCode == CurrentTenantCode)
            .Where(x => x.CompanyCode == CurrentCompanyCode)
            .Where(x => x.IsDeleted == 0)
            .FirstAsync();
    }

    /// <summary>
    /// 根据条件查询单个实体
    /// </summary>
    public virtual async Task<TEntity?> FirstAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Db.Queryable<TEntity>()
            .Where(predicate)
            .Where(x => x.TenantCode == CurrentTenantCode)
            .Where(x => x.CompanyCode == CurrentCompanyCode)
            .Where(x => x.IsDeleted == 0)
            .FirstAsync();
    }

    /// <summary>
    /// 查询所有实体（带租户和公司过滤）
    /// </summary>
    public virtual async Task<List<TEntity>> GetAllAsync()
    {
        return await Db.Queryable<TEntity>()
            .Where(x => x.TenantCode == CurrentTenantCode)
            .Where(x => x.CompanyCode == CurrentCompanyCode)
            .Where(x => x.IsDeleted == 0)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// 根据条件查询列表
    /// </summary>
    public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Db.Queryable<TEntity>()
            .Where(predicate)
            .Where(x => x.TenantCode == CurrentTenantCode)
            .Where(x => x.CompanyCode == CurrentCompanyCode)
            .Where(x => x.IsDeleted == 0)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// 根据条件查询列表（带排序）
    /// </summary>
    public virtual async Task<List<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, object>> orderBy,
        bool isDesc = true)
    {
        var query = Db.Queryable<TEntity>()
            .Where(predicate)
            .Where(x => x.TenantCode == CurrentTenantCode)
            .Where(x => x.CompanyCode == CurrentCompanyCode)
            .Where(x => x.IsDeleted == 0);

        return isDesc
            ? await query.OrderByDescending(orderBy).ToListAsync()
            : await query.OrderBy(orderBy).ToListAsync();
    }

    /// <summary>
    /// 导出用条件查询（带上限行数上限，防止全表加载 OOM）
    /// 过滤当前租户、公司与未删除审批记录，按 CreatedAt 降序后 Take 截断
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <param name="maxRows">最大行数；为空时使用 Excel:Export:MaxRowsPerRequest 配置</param>
    /// <returns>不超过上限的实体列表</returns>
    /// <exception cref="ArgumentOutOfRangeException">maxRows 小于等于 0 时抛出</exception>
    public virtual async Task<List<TEntity>> GetListForExportAsync(
        Expression<Func<TEntity, bool>> predicate,
        int? maxRows = null)
    {
        var take = maxRows ?? _excelOptions.Export.MaxRowsPerRequest;
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(take);
        return await Db.Queryable<TEntity>()
            .Where(predicate)
            .Where(x => x.TenantCode == CurrentTenantCode)
            .Where(x => x.CompanyCode == CurrentCompanyCode)
            .Where(x => x.IsDeleted == 0)
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
        var filterQuery = Db.Queryable<TEntity>()
            .Where(x => x.TenantCode == CurrentTenantCode)
            .Where(x => x.CompanyCode == CurrentCompanyCode)
            .Where(x => x.IsDeleted == 0);

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
        bool isDesc = true)
    {
        pageIndex = TaktPagedClamp.NormalizePageIndex(pageIndex);
        pageSize = TaktPagedClamp.NormalizePageSize(pageSize);
        var filterQuery = Db.Queryable<TEntity>()
            .Where(predicate)
            .Where(x => x.TenantCode == CurrentTenantCode)
            .Where(x => x.CompanyCode == CurrentCompanyCode)
            .Where(x => x.IsDeleted == 0);

        var total = await filterQuery.CountAsync();

        var pageQuery = filterQuery;
        if (orderBy != null)
        {
            pageQuery = isDesc ? pageQuery.OrderByDescending(orderBy) : pageQuery.OrderBy(orderBy);
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
    public virtual async Task<TEntity> CreateAsync(TEntity entity)
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

        await TaktPrimaryKeyInsertHelper.InsertEntityAsync(Db, entity, _primaryKeyTypeOptions);

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
    public virtual async Task<bool> UpdateAsync(TEntity entity)
    {
        // 自动设置审计字段
        entity.ApplyUpdate(CurrentUserId);

        var rows = await Db.Updateable(entity).ExecuteCommandAsync();
        return rows > 0;
    }

    /// <summary>
    /// 批量更新实体
    /// </summary>
    public virtual async Task<int> UpdateRangeAsync(List<TEntity> entities)
    {
        var now = DateTime.Now;
        foreach (var entity in entities)
        {
            entity.ApplyUpdate(CurrentUserId, now);
        }

        return await Db.Updateable(entities).ExecuteCommandAsync();
    }

    // ========================================
    // 删除操作
    // ========================================

    /// <summary>
    /// 软删除实体
    /// </summary>
    public virtual async Task<bool> DeleteAsync(long id)
    {
        var entity = await GetByIdAsync(id);
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
        var rows = await Db.Updateable<TEntity>()
            .SetColumns(x => new TEntity
            {
                IsDeleted = 1,
                UpdatedAt = now,
                UpdatedBy = operatorUserId,
                DeletedAt = now,
                DeletedBy = operatorUserId
            })
            .Where(x => x.Id == id)
            .Where(x => x.TenantCode == CurrentTenantCode)
            .Where(x => x.CompanyCode == CurrentCompanyCode)
            .Where(x => x.IsDeleted == 0)
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
        return await Db.Updateable<TEntity>()
            .SetColumns(x => new TEntity
            {
                IsDeleted = 1,
                UpdatedAt = now,
                UpdatedBy = operatorUserId,
                DeletedAt = now,
                DeletedBy = operatorUserId
            })
            .Where(predicate)
            .Where(x => x.TenantCode == CurrentTenantCode)
            .Where(x => x.CompanyCode == CurrentCompanyCode)
            .Where(x => x.IsDeleted == 0)
            .ExecuteCommandAsync();
    }

    // ========================================
    // 存在性检查
    // ========================================

    /// <summary>
    /// 检查实体是否存在
    /// </summary>
    public virtual async Task<bool> ExistsAsync(long id)
    {
        return await Db.Queryable<TEntity>()
            .Where(x => x.Id == id)
            .Where(x => x.TenantCode == CurrentTenantCode)
            .Where(x => x.CompanyCode == CurrentCompanyCode)
            .Where(x => x.IsDeleted == 0)
            .AnyAsync();
    }

    /// <summary>
    /// 根据条件检查是否存在
    /// </summary>
    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Db.Queryable<TEntity>()
            .Where(predicate)
            .Where(x => x.TenantCode == CurrentTenantCode)
            .Where(x => x.CompanyCode == CurrentCompanyCode)
            .Where(x => x.IsDeleted == 0)
            .AnyAsync();
    }

    /// <summary>
    /// 统计符合条件的记录数
    /// </summary>
    public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null)
    {
        var query = Db.Queryable<TEntity>()
            .Where(x => x.TenantCode == CurrentTenantCode)
            .Where(x => x.CompanyCode == CurrentCompanyCode);

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
    /// 构建聚合读查询（租户与公司隔离、未删除）
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <returns>可聚合的查询</returns>
    private ISugarQueryable<TEntity> BuildAggregateReadQuery(Expression<Func<TEntity, bool>>? predicate)
    {
        var query = Db.Queryable<TEntity>()
            .Where(x => x.TenantCode == CurrentTenantCode)
            .Where(x => x.CompanyCode == CurrentCompanyCode)
            .Where(x => x.IsDeleted == 0);
        if (predicate != null)
        {
            query = query.Where(predicate);
        }
        return query;
    }

    /// <summary>
    /// 构建聚合写查询（租户与公司隔离、含已软删）
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <returns>可聚合的查询</returns>
    private ISugarQueryable<TEntity> BuildAggregateWriteQuery(Expression<Func<TEntity, bool>>? predicate)
    {
        var query = Db.Queryable<TEntity>()
            .Where(x => x.TenantCode == CurrentTenantCode)
            .Where(x => x.CompanyCode == CurrentCompanyCode);
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
        var query = BuildAggregateWriteQuery(predicate);
        if (!await query.AnyAsync())
        {
            return 0;
        }
        return await query.MaxAsync(fieldSelector);
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

    // ========================================
    // 审批操作（专属方法）
    // ========================================

    /// <summary>
    /// 提交审批（已废弃，请使用 TaktFlowEngine.StartFlowInstanceAsync）
    /// </summary>
    [Obsolete("凡审批必走 TaktFlowEngine；请调用 ITaktFlowEngineService.StartFlowInstanceAsync 并回写 FlowInstanceId")]
    public virtual async Task<bool> SubmitForApprovalAsync(long id, long submitterId)
    {
        var rows = await Db.Updateable<TEntity>()
            .SetColumns(x => new TEntity
            {
                ApprovalStatus = 1,
                InitiatorId = submitterId,
                InitiatedAt = DateTime.Now,
                UpdatedBy = submitterId,
                UpdatedAt = DateTime.Now
            })
            .Where(x => x.Id == id)
            .Where(x => x.TenantCode == CurrentTenantCode)
            .Where(x => x.CompanyCode == CurrentCompanyCode)
            .Where(x => x.ApprovalStatus == 0)
            .ExecuteCommandAsync();

        return rows > 0;
    }

    /// <summary>
    /// 审批通过（已废弃，请使用 TaktFlowEngine.CompleteFlowInstanceTaskAsync）
    /// </summary>
    [Obsolete("凡审批必走 TaktFlowEngine；请通过待办 Complete 并在业务层回写 ApprovalStatus")]
    public virtual async Task<bool> ApproveAsync(long id, long approverId, string? opinion = null)
    {
        var rows = await Db.Updateable<TEntity>()
            .SetColumns(x => new TEntity
            {
                ApprovalStatus = 2,
                ApprovalOpinion = opinion,
                ApprovedBy = approverId,
                ApprovedAt = DateTime.Now,
                UpdatedBy = approverId,
                UpdatedAt = DateTime.Now
            })
            .Where(x => x.Id == id)
            .Where(x => x.TenantCode == CurrentTenantCode)
            .Where(x => x.CompanyCode == CurrentCompanyCode)
            .Where(x => x.ApprovalStatus == 1)
            .ExecuteCommandAsync();

        return rows > 0;
    }

    /// <summary>
    /// 审批驳回（已废弃，请使用 TaktFlowEngine.CompleteFlowInstanceTaskAsync）
    /// </summary>
    [Obsolete("凡审批必走 TaktFlowEngine；请通过待办 Complete 并在业务层回写 ApprovalStatus")]
    public virtual async Task<bool> RejectAsync(long id, long approverId, string opinion)
    {
        var rows = await Db.Updateable<TEntity>()
            .SetColumns(x => new TEntity
            {
                ApprovalStatus = 3,
                ApprovalOpinion = opinion,
                ApprovedBy = approverId,
                ApprovedAt = DateTime.Now,
                UpdatedBy = approverId,
                UpdatedAt = DateTime.Now
            })
            .Where(x => x.Id == id)
            .Where(x => x.TenantCode == CurrentTenantCode)
            .Where(x => x.CompanyCode == CurrentCompanyCode)
            .Where(x => x.ApprovalStatus == 1)
            .ExecuteCommandAsync();

        return rows > 0;
    }

    /// <summary>
    /// 撤销审批（已废弃，请使用 TaktFlowEngine.RevokeFlowInstanceAsync）
    /// </summary>
    [Obsolete("凡审批必走 TaktFlowEngine；请调用 ITaktFlowEngineService.RevokeFlowInstanceAsync")]
    public virtual async Task<bool> CancelApprovalAsync(long id, long cancellerId, string? opinion = null)
    {
        var rows = await Db.Updateable<TEntity>()
            .SetColumns(x => new TEntity
            {
                ApprovalStatus = 4,
                ApprovalOpinion = opinion,
                UpdatedBy = cancellerId,
                UpdatedAt = DateTime.Now
            })
            .Where(x => x.Id == id)
            .Where(x => x.TenantCode == CurrentTenantCode)
            .Where(x => x.CompanyCode == CurrentCompanyCode)
            .Where(x => x.ApprovalStatus == 1 ||
                        x.ApprovalStatus == 0)
            .ExecuteCommandAsync();

        return rows > 0;
    }
}
