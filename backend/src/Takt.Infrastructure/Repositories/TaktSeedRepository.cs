// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Repositories
// 文件名称：TaktSeedRepository.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：种子数据专用仓储实现（三个实体基类共用）；创建/更新时补齐 RelatedPlant 或 PlantCode（对齐业务仓储）
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
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Repositories;

/// <summary>
/// 租户级种子数据专用仓储实现
/// 特点：
/// 1. 不依赖 ITaktUserContext（种子数据阶段无 HTTP 上下文）
/// 2. 不自动添加租户/公司过滤（由调用方显式指定）
/// 3. 支持精确控制租户和公司编码
/// 4. 根据配置处理主键类型
/// </summary>
/// <typeparam name="TEntity">实体类型（必须实现 ITaktTenantEntity）</typeparam>
public class TaktTenantSeedRepository<TEntity> : ITaktTenantSeedRepository<TEntity> where TEntity : TaktTenantCoreEntityScopeBase, ITaktTenantEntity, new()
{
    /// <summary>
    /// 种子数据数据库上下文
    /// </summary>
    private readonly TaktSeedContext _dbContext;

    /// <summary>
    /// 主键类型配置
    /// </summary>
    private readonly PrimaryKeyTypeOptions _primaryKeyTypeOptions;

    /// <summary>
    /// Database 配置（CompanyCodes↔PlantCodes 同序映射）
    /// </summary>
    private readonly TaktDatabaseOptions _database;

    /// <summary>
    /// 数据库客户端
    /// </summary>
    protected ISqlSugarClient Db => _dbContext.Db;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="dbContext">种子数据上下文</param>
    /// <param name="idRuleOptions">主键类型配置</param>
    /// <param name="databaseOptions">Database 配置</param>
    public TaktTenantSeedRepository(
        TaktSeedContext dbContext,
        Microsoft.Extensions.Options.IOptions<PrimaryKeyTypeOptions> idRuleOptions,
        Microsoft.Extensions.Options.IOptions<TaktDatabaseOptions> databaseOptions)
    {
        _dbContext = dbContext;
        _primaryKeyTypeOptions = idRuleOptions.Value;
        _database = databaseOptions.Value;
        _database.NormalizeAndValidate();
    }

    // ========================================
    // 新增操作（根据配置处理主键类型）
    // ========================================

    /// <summary>
    /// 创建实体（根据实体主键字段类型自动判断 ID 生成策略）
    /// </summary>
    /// <param name="entity">实体对象（TenantCode 由调用方显式指定）</param>
    /// <returns>创建后的实体（含生成的主键）</returns>
    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        var now = DateTime.Now;
        await TaktTenantScopeFillHelper.ApplyRelatedPlantAsync(entity, _database);
        entity.ApplySeedCreate(now);
        await TaktPrimaryKeyInsertHelper.InsertEntityAsync(Db, entity, _primaryKeyTypeOptions);
        return entity;
    }

    /// <summary>
    /// 批量创建实体（根据配置处理主键类型和审计字段）
    /// </summary>
    /// <param name="entities">实体列表</param>
    /// <returns>创建的实体数量</returns>
    public virtual async Task<int> CreateRangeAsync(List<TEntity> entities)
    {
        if (entities.Count == 0) return 0;
        var now = DateTime.Now;
        foreach (var entity in entities)
        {
            await TaktTenantScopeFillHelper.ApplyRelatedPlantAsync(entity, _database);
            entity.ApplySeedCreate(now);
        }
        return await TaktPrimaryKeyInsertHelper.InsertEntitiesAsync(Db, entities, _primaryKeyTypeOptions);
    }

    /// <summary>
    /// 批量创建实体（大批量插入优化，根据配置处理主键类型和审计字段）
    /// </summary>
    /// <param name="entities">实体列表</param>
    /// <returns>创建的实体数量</returns>
    public virtual async Task<int> CreateRangeBulkAsync(List<TEntity> entities)
    {
        if (entities.Count == 0) return 0;
        var now = DateTime.Now;
        foreach (var entity in entities)
        {
            await TaktTenantScopeFillHelper.ApplyRelatedPlantAsync(entity, _database);
            entity.ApplySeedCreate(now);
        }
        return await TaktPrimaryKeyInsertHelper.InsertEntitiesAsync(Db, entities, _primaryKeyTypeOptions);
    }

    // ========================================
    // 更新操作
    // ========================================

    /// <summary>
    /// 更新实体（自动填充审计字段）
    /// </summary>
    /// <param name="entity">实体对象</param>
    /// <returns>是否有行被更新</returns>
    public virtual async Task<bool> UpdateAsync(TEntity entity)
    {
        await TaktTenantScopeFillHelper.ApplyRelatedPlantAsync(entity, _database);
        entity.ApplySeedUpdate();
        return await Db.Updateable(entity).ExecuteCommandHasChangeAsync();
    }

    /// <summary>
    /// 批量更新实体（自动填充审计字段）
    /// </summary>
    /// <param name="entities">实体列表</param>
    /// <returns>更新的实体数量</returns>
    public virtual async Task<int> UpdateRangeAsync(List<TEntity> entities)
    {
        if (entities.Count == 0) return 0;
        var now = DateTime.Now;
        foreach (var entity in entities)
        {
            await TaktTenantScopeFillHelper.ApplyRelatedPlantAsync(entity, _database);
            entity.ApplySeedUpdate(now);
        }
        return await Db.Updateable(entities).ExecuteCommandAsync();
    }

    // ========================================
    // 查询操作（不自动添加租户过滤）
    // ========================================

    /// <summary>
    /// 根据ID查询实体（不自动添加租户过滤）
    /// </summary>
    /// <param name="id">实体主键</param>
    /// <returns>未删除的实体；不存在时返回 null</returns>
    public virtual async Task<TEntity?> GetByIdAsync(long id)
    {
        return await Db.Queryable<TEntity>()
            .Where(x => x.Id == id)
            .Where(x => x.IsDeleted == 0)
            .FirstAsync();
    }

    /// <summary>
    /// 根据条件查询单个实体（不自动添加租户过滤）
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <returns>未删除的实体；不存在时返回 null</returns>
    public virtual async Task<TEntity?> FirstAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Db.Queryable<TEntity>()
            .Where(predicate)
            .Where(x => x.IsDeleted == 0)
            .FirstAsync();
    }

    /// <summary>
    /// 根据条件查询实体列表（不自动添加租户过滤）
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <returns>未删除的实体列表</returns>
    public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Db.Queryable<TEntity>()
            .Where(predicate)
            .Where(x => x.IsDeleted == 0)
            .ToListAsync();
    }

    /// <summary>
    /// 检查实体是否存在
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <returns>是否存在未删除的匹配记录</returns>
    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Db.Queryable<TEntity>()
            .Where(predicate)
            .Where(x => x.IsDeleted == 0)
            .AnyAsync();
    }
}

// ========================================
// 公司级种子数据专用仓储实现
// ========================================

/// <summary>
/// 公司级种子数据专用仓储实现
/// 特点：
/// 1. 不依赖 ITaktUserContext（种子数据阶段无 HTTP 上下文）
/// 2. 不自动添加租户/公司过滤（由调用方显式指定）
/// 3. 支持精确控制租户和公司编码
/// 4. 根据配置处理主键类型
/// </summary>
/// <typeparam name="TEntity">实体类型（必须继承TaktCompanyEntityBase）</typeparam>
public class TaktCompanySeedRepository<TEntity> : ITaktCompanySeedRepository<TEntity> where TEntity : TaktCompanyEntityBase, new()
{
    /// <summary>
    /// 种子数据数据库上下文
    /// </summary>
    private readonly TaktSeedContext _dbContext;

    /// <summary>
    /// 主键类型配置
    /// </summary>
    private readonly PrimaryKeyTypeOptions _primaryKeyTypeOptions;

    /// <summary>
    /// Database 配置（CompanyCodes↔PlantCodes 同序映射）
    /// </summary>
    private readonly TaktDatabaseOptions _database;

    /// <summary>
    /// 数据库客户端
    /// </summary>
    protected ISqlSugarClient Db => _dbContext.Db;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="dbContext">种子数据上下文</param>
    /// <param name="primaryKeyTypeOptions">主键类型配置</param>
    /// <param name="databaseOptions">Database 配置</param>
    public TaktCompanySeedRepository(
        TaktSeedContext dbContext,
        Microsoft.Extensions.Options.IOptions<PrimaryKeyTypeOptions> primaryKeyTypeOptions,
        Microsoft.Extensions.Options.IOptions<TaktDatabaseOptions> databaseOptions)
    {
        _dbContext = dbContext;
        _primaryKeyTypeOptions = primaryKeyTypeOptions.Value;
        _database = databaseOptions.Value;
        _database.NormalizeAndValidate();
    }

    // ========================================
    // 新增操作（根据配置处理主键类型）
    // ========================================

    /// <summary>
    /// 创建实体（根据实体主键字段类型自动判断 ID 生成策略）
    /// </summary>
    /// <param name="entity">实体对象（TenantCode、CompanyCode 由调用方显式指定）</param>
    /// <returns>创建后的实体（含生成的主键）</returns>
    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        var now = DateTime.Now;
        await TaktCompanyScopeFillHelper.ApplyCompanyScopeFromMasterAsync(
            Db,
            entity,
            entity.TenantCode,
            entity.CompanyCode,
            _database);
        entity.ApplySeedCreate(now);
        await TaktPrimaryKeyInsertHelper.InsertEntityAsync(Db, entity, _primaryKeyTypeOptions);
        return entity;
    }

    /// <summary>
    /// 批量创建实体（根据配置处理主键类型和审计字段）
    /// </summary>
    /// <param name="entities">实体列表</param>
    /// <returns>创建的实体数量</returns>
    public virtual async Task<int> CreateRangeAsync(List<TEntity> entities)
    {
        if (entities.Count == 0) return 0;
        var now = DateTime.Now;
        foreach (var entity in entities)
        {
            await TaktCompanyScopeFillHelper.ApplyCompanyScopeFromMasterAsync(
                Db,
                entity,
                entity.TenantCode,
                entity.CompanyCode,
                _database);
            entity.ApplySeedCreate(now);
        }
        return await TaktPrimaryKeyInsertHelper.InsertEntitiesAsync(Db, entities, _primaryKeyTypeOptions);
    }

    /// <summary>
    /// 批量创建实体（大批量插入优化，根据配置处理主键类型和审计字段）
    /// </summary>
    /// <param name="entities">实体列表</param>
    /// <returns>创建的实体数量</returns>
    public virtual async Task<int> CreateRangeBulkAsync(List<TEntity> entities)
    {
        if (entities.Count == 0) return 0;
        var now = DateTime.Now;
        foreach (var entity in entities)
        {
            await TaktCompanyScopeFillHelper.ApplyCompanyScopeFromMasterAsync(
                Db,
                entity,
                entity.TenantCode,
                entity.CompanyCode,
                _database);
            entity.ApplySeedCreate(now);
        }
        return await TaktPrimaryKeyInsertHelper.InsertEntitiesAsync(Db, entities, _primaryKeyTypeOptions);
    }

    // ========================================
    // 更新操作
    // ========================================

    /// <summary>
    /// 更新实体（自动填充审计字段）
    /// </summary>
    /// <param name="entity">实体对象</param>
    /// <returns>是否有行被更新</returns>
    public virtual async Task<bool> UpdateAsync(TEntity entity)
    {
        await TaktCompanyScopeFillHelper.ApplyCompanyScopeFromMasterAsync(
            Db,
            entity,
            entity.TenantCode,
            entity.CompanyCode,
            _database);
        entity.ApplySeedUpdate();
        return await Db.Updateable(entity).ExecuteCommandHasChangeAsync();
    }

    /// <summary>
    /// 批量更新实体（自动填充审计字段）
    /// </summary>
    /// <param name="entities">实体列表</param>
    /// <returns>更新的实体数量</returns>
    public virtual async Task<int> UpdateRangeAsync(List<TEntity> entities)
    {
        if (entities.Count == 0) return 0;
        var now = DateTime.Now;
        foreach (var entity in entities)
        {
            await TaktCompanyScopeFillHelper.ApplyCompanyScopeFromMasterAsync(
                Db,
                entity,
                entity.TenantCode,
                entity.CompanyCode,
                _database);
            entity.ApplySeedUpdate(now);
        }
        return await Db.Updateable(entities).ExecuteCommandAsync();
    }

    // ========================================
    // 查询操作（不自动添加租户/公司过滤）
    // ========================================

    /// <summary>
    /// 根据ID查询实体（不自动添加租户/公司过滤）
    /// </summary>
    /// <param name="id">实体主键</param>
    /// <returns>未删除的实体；不存在时返回 null</returns>
    public virtual async Task<TEntity?> GetByIdAsync(long id)
    {
        return await Db.Queryable<TEntity>().Where(x => x.Id == id).Where(x => x.IsDeleted == 0).FirstAsync();
    }

    /// <summary>
    /// 根据条件查询单个实体（不自动添加租户/公司过滤）
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <returns>未删除的实体；不存在时返回 null</returns>
    public virtual async Task<TEntity?> FirstAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Db.Queryable<TEntity>().Where(predicate).Where(x => x.IsDeleted == 0).FirstAsync();
    }

    /// <summary>
    /// 根据条件查询实体列表（不自动添加租户/公司过滤）
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <returns>未删除的实体列表</returns>
    public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Db.Queryable<TEntity>().Where(predicate).Where(x => x.IsDeleted == 0).ToListAsync();
    }

    /// <summary>
    /// 检查实体是否存在
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <returns>是否存在未删除的匹配记录</returns>
    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Db.Queryable<TEntity>().Where(predicate).Where(x => x.IsDeleted == 0).AnyAsync();
    }
}

// ========================================
// 审批级种子数据专用仓储实现
// ========================================

/// <summary>
/// 审批级种子数据专用仓储实现
/// 特点：
/// 1. 不依赖 ITaktUserContext（种子数据阶段无 HTTP 上下文）
/// 2. 不自动添加租户/公司过滤（由调用方显式指定）
/// 3. 支持精确控制租户和公司编码
/// 4. 根据配置处理主键类型
/// </summary>
/// <typeparam name="TEntity">实体类型（必须继承 TaktApprovalEntityBase）</typeparam>
public class TaktApprovalSeedRepository<TEntity> : ITaktApprovalSeedRepository<TEntity> where TEntity : TaktApprovalEntityBase, new()
{
    /// <summary>
    /// 种子数据数据库上下文
    /// </summary>
    private readonly TaktSeedContext _dbContext;

    /// <summary>
    /// 主键类型配置
    /// </summary>
    private readonly PrimaryKeyTypeOptions _primaryKeyTypeOptions;

    /// <summary>
    /// Database 配置（CompanyCodes↔PlantCodes 同序映射）
    /// </summary>
    private readonly TaktDatabaseOptions _database;

    /// <summary>
    /// 数据库客户端
    /// </summary>
    protected ISqlSugarClient Db => _dbContext.Db;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="dbContext">种子数据上下文</param>
    /// <param name="primaryKeyTypeOptions">主键类型配置</param>
    /// <param name="databaseOptions">Database 配置</param>
    public TaktApprovalSeedRepository(
        TaktSeedContext dbContext,
        Microsoft.Extensions.Options.IOptions<PrimaryKeyTypeOptions> primaryKeyTypeOptions,
        Microsoft.Extensions.Options.IOptions<TaktDatabaseOptions> databaseOptions)
    {
        _dbContext = dbContext;
        _primaryKeyTypeOptions = primaryKeyTypeOptions.Value;
        _database = databaseOptions.Value;
        _database.NormalizeAndValidate();
    }

    // ========================================
    // 新增操作（根据配置处理主键类型）
    // ========================================

    /// <summary>
    /// 创建实体（根据实体主键字段类型自动判断 ID 生成策略）
    /// </summary>
    /// <param name="entity">实体对象（TenantCode、CompanyCode 由调用方显式指定）</param>
    /// <returns>创建后的实体（含生成的主键）</returns>
    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        var now = DateTime.Now;
        await TaktCompanyScopeFillHelper.ApplyCompanyScopeFromMasterAsync(
            Db,
            entity,
            entity.TenantCode,
            entity.CompanyCode,
            _database);
        entity.ApplySeedCreate(now);
        await TaktPrimaryKeyInsertHelper.InsertEntityAsync(Db, entity, _primaryKeyTypeOptions);
        return entity;
    }

    /// <summary>
    /// 批量创建实体（根据配置处理主键类型和审计字段）
    /// </summary>
    /// <param name="entities">实体列表</param>
    /// <returns>创建的实体数量</returns>
    public virtual async Task<int> CreateRangeAsync(List<TEntity> entities)
    {
        if (entities.Count == 0) return 0;
        var now = DateTime.Now;
        foreach (var entity in entities)
        {
            await TaktCompanyScopeFillHelper.ApplyCompanyScopeFromMasterAsync(
                Db,
                entity,
                entity.TenantCode,
                entity.CompanyCode,
                _database);
            entity.ApplySeedCreate(now);
        }
        return await TaktPrimaryKeyInsertHelper.InsertEntitiesAsync(Db, entities, _primaryKeyTypeOptions);
    }

    /// <summary>
    /// 批量创建实体（大批量插入优化，根据配置处理主键类型和审计字段）
    /// </summary>
    /// <param name="entities">实体列表</param>
    /// <returns>创建的实体数量</returns>
    public virtual async Task<int> CreateRangeBulkAsync(List<TEntity> entities)
    {
        if (entities.Count == 0) return 0;
        var now = DateTime.Now;
        foreach (var entity in entities)
        {
            await TaktCompanyScopeFillHelper.ApplyCompanyScopeFromMasterAsync(
                Db,
                entity,
                entity.TenantCode,
                entity.CompanyCode,
                _database);
            entity.ApplySeedCreate(now);
        }
        return await TaktPrimaryKeyInsertHelper.InsertEntitiesAsync(Db, entities, _primaryKeyTypeOptions);
    }

    // ========================================
    // 更新操作
    // ========================================

    /// <summary>
    /// 更新实体（自动填充审计字段）
    /// </summary>
    /// <param name="entity">实体对象</param>
    /// <returns>是否有行被更新</returns>
    public virtual async Task<bool> UpdateAsync(TEntity entity)
    {
        await TaktCompanyScopeFillHelper.ApplyCompanyScopeFromMasterAsync(
            Db,
            entity,
            entity.TenantCode,
            entity.CompanyCode,
            _database);
        entity.ApplySeedUpdate();
        return await Db.Updateable(entity).ExecuteCommandHasChangeAsync();
    }

    /// <summary>
    /// 批量更新实体（自动填充审计字段）
    /// </summary>
    /// <param name="entities">实体列表</param>
    /// <returns>更新的实体数量</returns>
    public virtual async Task<int> UpdateRangeAsync(List<TEntity> entities)
    {
        if (entities.Count == 0) return 0;
        var now = DateTime.Now;
        foreach (var entity in entities)
        {
            await TaktCompanyScopeFillHelper.ApplyCompanyScopeFromMasterAsync(
                Db,
                entity,
                entity.TenantCode,
                entity.CompanyCode,
                _database);
            entity.ApplySeedUpdate(now);
        }
        return await Db.Updateable(entities).ExecuteCommandAsync();
    }

    // ========================================
    // 查询操作（不自动添加租户/公司过滤）
    // ========================================

    /// <summary>
    /// 根据ID查询实体（不自动添加租户/公司过滤）
    /// </summary>
    /// <param name="id">实体主键</param>
    /// <returns>未删除的实体；不存在时返回 null</returns>
    public virtual async Task<TEntity?> GetByIdAsync(long id)
    {
        return await Db.Queryable<TEntity>().Where(x => x.Id == id).Where(x => x.IsDeleted == 0).FirstAsync();
    }

    /// <summary>
    /// 根据条件查询单个实体（不自动添加租户/公司过滤）
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <returns>未删除的实体；不存在时返回 null</returns>
    public virtual async Task<TEntity?> FirstAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Db.Queryable<TEntity>().Where(predicate).Where(x => x.IsDeleted == 0).FirstAsync();
    }

    /// <summary>
    /// 根据条件查询实体列表（不自动添加租户/公司过滤）
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <returns>未删除的实体列表</returns>
    public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Db.Queryable<TEntity>().Where(predicate).Where(x => x.IsDeleted == 0).ToListAsync();
    }

    /// <summary>
    /// 检查实体是否存在
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <returns>是否存在未删除的匹配记录</returns>
    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Db.Queryable<TEntity>().Where(predicate).Where(x => x.IsDeleted == 0).AnyAsync();
    }
}