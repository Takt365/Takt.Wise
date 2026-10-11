// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktPrimaryKeyInsertHelper.cs
// 功能描述：实体插入主键策略（按 Id 字段类型 + PrimaryKeyType 配置：long 雪花 / int 自增 / Guid）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Reflection;
using SqlSugar;
using Takt.Shared.Options;

namespace Takt.Shared.Helpers;

/// <summary>
/// 插入时主键生成策略：仓储与基础设施写入点统一入口。
/// <para>判定顺序（单条/批量）：SugarColumn.IsIdentity 且 Identity.Enabled → 库自增；long 且非自增且 Snowflake.Enabled → 雪花；Guid 且 Guid.Enabled → 应用层 Guid.NewGuid；其余 → 原样 INSERT。</para>
/// <para>启动时由 Program 调用 Configure 绑定 appsettings PrimaryKeyType；无 DI 的 AOP/审计写入使用 RuntimeOptions。</para>
/// </summary>
public static class TaktPrimaryKeyInsertHelper
{
    private static PrimaryKeyTypeOptions _runtimeOptions = CreateDefaultOptions();

    /// <summary>
    /// 应用启动时绑定 appsettings PrimaryKeyType（供静态 AOP/审计写入与 DI 选项保持一致）
    /// </summary>
    /// <param name="options">主键类型配置</param>
    /// <exception cref="ArgumentNullException"><paramref name="options"/> 为 null</exception>
    public static void Configure(PrimaryKeyTypeOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        _runtimeOptions = options;
    }

    /// <summary>
    /// 当前运行时主键配置（与 DI 注入的 IOptions PrimaryKeyTypeOptions 一致）
    /// </summary>
    public static PrimaryKeyTypeOptions RuntimeOptions => _runtimeOptions;

    /// <summary>
    /// 未调用 Configure 时的默认主键配置（Identity/Guid/Snowflake 均启用，WorkId=1）
    /// </summary>
    /// <returns>默认 PrimaryKeyTypeOptions 实例</returns>
    private static PrimaryKeyTypeOptions CreateDefaultOptions() => new()
    {
        Identity = { Enabled = true },
        Guid = { Enabled = true },
        Snowflake = { Enabled = true, WorkId = 1 }
    };

    /// <summary>
    /// 解析实体 Id 属性（沿继承链查找名为 Id 的 public 实例属性）
    /// </summary>
    /// <param name="entityType">实体类型</param>
    /// <returns>Id 属性；不存在时返回 null</returns>
    /// <exception cref="ArgumentNullException"><paramref name="entityType"/> 为 null</exception>
    public static PropertyInfo? ResolveIdProperty(Type entityType)
    {
        ArgumentNullException.ThrowIfNull(entityType);
        return entityType.GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);
    }

    /// <summary>
    /// Guid 主键为空（Guid.Empty）时赋 Guid.NewGuid；非 Guid 实体或无 Id 属性时不修改
    /// </summary>
    /// <param name="entity">实体实例；插入成功后 Id 可能已被写入</param>
    /// <exception cref="ArgumentNullException"><paramref name="entity"/> 为 null</exception>
    public static void AssignGuidIfEmpty(object entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        var idProperty = ResolveIdProperty(entity.GetType());
        if (idProperty?.PropertyType != typeof(Guid))
        {
            return;
        }

        if ((Guid)idProperty.GetValue(entity)! == Guid.Empty)
        {
            idProperty.SetValue(entity, Guid.NewGuid());
        }
    }

    /// <summary>
    /// 单条实体插入并按主键策略回填 Id（自增/雪花写回实体；Guid 在插入前由 AssignGuidIfEmpty 赋值）
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <param name="db">SqlSugar 客户端</param>
    /// <param name="entity">待插入实体；成功后 Id 属性可能被更新</param>
    /// <param name="options">主键类型配置</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>插入完成后的任务</returns>
    /// <exception cref="ArgumentNullException"><paramref name="db"/>、<paramref name="entity"/> 或 <paramref name="options"/> 为 null</exception>
    public static async Task InsertEntityAsync<T>(
        ISqlSugarClient db,
        T entity,
        PrimaryKeyTypeOptions options,
        CancellationToken cancellationToken = default) where T : class, new()
    {
        await InsertEntityAsync(db, entity, options, asTableName: null, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// 单条实体插入（可指定物理表名，供年分表路由）
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <param name="db">SqlSugar 客户端</param>
    /// <param name="entity">待插入实体</param>
    /// <param name="options">主键类型配置</param>
    /// <param name="asTableName">物理表名；空则用实体默认表</param>
    /// <param name="cancellationToken">取消令牌</param>
    public static async Task InsertEntityAsync<T>(
        ISqlSugarClient db,
        T entity,
        PrimaryKeyTypeOptions options,
        string? asTableName,
        CancellationToken cancellationToken = default) where T : class, new()
    {
        ArgumentNullException.ThrowIfNull(db);
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(options);

        var insertable = string.IsNullOrWhiteSpace(asTableName)
            ? db.Insertable(entity)
            : db.Insertable(entity).AS(asTableName.Trim());
        var entityType = typeof(T);
        var idProperty = ResolveIdProperty(entityType);
        var idType = idProperty?.PropertyType;
        if (await TryInsertWithIdentityAsync(insertable, entity, idProperty, idType, options, cancellationToken))
        {
            return;
        }

        if (idType == typeof(long) && options.Snowflake.Enabled && IsSnowflakeEntity(typeof(T)))
        {
            await insertable.ExecuteReturnSnowflakeIdAsync(cancellationToken);
            return;
        }

        if (idType == typeof(Guid) && options.Guid.Enabled && IsGuidEntity(typeof(T)))
        {
            AssignGuidIfEmpty(entity);
            await insertable.ExecuteCommandAsync(cancellationToken);
            return;
        }

        await insertable.ExecuteCommandAsync(cancellationToken);
    }

    /// <summary>
    /// 单条实体插入（同步，供 SqlSugar AOP/审计等无 async 上下文使用）；策略与 InsertEntityAsync 相同
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <param name="db">SqlSugar 客户端</param>
    /// <param name="entity">待插入实体；成功后 Id 属性可能被更新</param>
    /// <param name="options">主键类型配置</param>
    /// <exception cref="ArgumentNullException"><paramref name="db"/>、<paramref name="entity"/> 或 <paramref name="options"/> 为 null</exception>
    public static void InsertEntitySync<T>(
        ISqlSugarClient db,
        T entity,
        PrimaryKeyTypeOptions options) where T : class, new()
    {
        ArgumentNullException.ThrowIfNull(db);
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(options);

        var insertable = db.Insertable(entity);
        var entityType = typeof(T);
        var idProperty = ResolveIdProperty(entityType);
        var idType = idProperty?.PropertyType;
        if (TryInsertWithIdentitySync(insertable, entity, idProperty, idType, options))
        {
            return;
        }

        if (idType == typeof(long) && options.Snowflake.Enabled && IsSnowflakeEntity(typeof(T)))
        {
            insertable.ExecuteReturnSnowflakeId();
            return;
        }

        if (idType == typeof(Guid) && options.Guid.Enabled && IsGuidEntity(typeof(T)))
        {
            AssignGuidIfEmpty(entity);
            insertable.ExecuteCommand();
            return;
        }

        insertable.ExecuteCommand();
    }

    /// <summary>
    /// BulkCopy 单页上限（SqlSugar 推荐十万级 PageSize）
    /// </summary>
    public const int DefaultInsertBatchSize = 100_000;

    /// <summary>
    /// 达到该行数起优先 SqlBulkCopy（预分配雪花 Id）
    /// </summary>
    public const int BulkCopyThreshold = 100;

    /// <summary>
    /// 批量插入实体并按主键策略回填（≥BulkCopyThreshold 的雪花实体走 SqlBulkCopy）
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <param name="db">SqlSugar 客户端</param>
    /// <param name="entities">待插入列表</param>
    /// <param name="options">主键类型配置</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>插入行数；空列表返回 0</returns>
    /// <exception cref="ArgumentNullException"><paramref name="db"/>、<paramref name="entities"/> 或 <paramref name="options"/> 为 null</exception>
    public static async Task<int> InsertEntitiesAsync<T>(
        ISqlSugarClient db,
        List<T> entities,
        PrimaryKeyTypeOptions options,
        CancellationToken cancellationToken = default) where T : class, new()
    {
        ArgumentNullException.ThrowIfNull(db);
        ArgumentNullException.ThrowIfNull(entities);
        ArgumentNullException.ThrowIfNull(options);
        if (entities.Count == 0)
        {
            return 0;
        }

        var entityType = typeof(T);
        var idProperty = ResolveIdProperty(entityType);
        var idType = idProperty?.PropertyType;
        if (idType == typeof(long)
            && options.Snowflake.Enabled
            && IsSnowflakeEntity(typeof(T))
            && entities.Count >= BulkCopyThreshold
            && idProperty != null)
        {
            return await BulkCopySnowflakeEntitiesAsync(db, entities, idProperty, cancellationToken);
        }

        if (entities.Count <= 1000)
        {
            return await InsertEntitiesCoreAsync(db, entities, options, cancellationToken);
        }

        var total = 0;
        const int insertablePage = 1000;
        for (var offset = 0; offset < entities.Count; offset = checked(offset + insertablePage))
        {
            var take = Math.Min(insertablePage, entities.Count - offset);
            var batch = entities.GetRange(offset, take);
            total = checked(total + await InsertEntitiesCoreAsync(db, batch, options, cancellationToken));
        }

        return total;
    }

    /// <summary>
    /// 预分配雪花 Id 后 SqlBulkCopy（PageSize=10万；18 万行应秒～数十秒级，禁止再用 Insertable）
    /// </summary>
    private static async Task<int> BulkCopySnowflakeEntitiesAsync<T>(
        ISqlSugarClient db,
        List<T> entities,
        PropertyInfo idProperty,
        CancellationToken cancellationToken) where T : class, new()
    {
        AssignSnowflakeIdsIfEmpty(entities, idProperty);
        cancellationToken.ThrowIfCancellationRequested();
        // SqlSugar 文档：PageSize(100000).BulkCopy — 大数据比 Insertable 快一个数量级以上
        TaktLogger.Information(
            "[BulkCopy] 开始 Type={Type} Count={Count} PageSize={PageSize}",
            typeof(T).Name,
            entities.Count,
            DefaultInsertBatchSize);
        var sw = System.Diagnostics.Stopwatch.StartNew();
        var copied = await db.Fastest<T>().PageSize(DefaultInsertBatchSize).BulkCopyAsync(entities);
        sw.Stop();
        var result = copied > 0 ? copied : entities.Count;
        TaktLogger.Information(
            "[BulkCopy] 完成 Type={Type} Copied={Copied} ElapsedMs={ElapsedMs}",
            typeof(T).Name,
            result,
            sw.ElapsedMilliseconds);
        return result;
    }

    /// <summary>
    /// Id 为 0 时写入 SnowFlakeSingle.NextId（供 BulkCopy）
    /// </summary>
    private static void AssignSnowflakeIdsIfEmpty<T>(List<T> entities, PropertyInfo idProperty)
    {
        foreach (var entity in entities)
        {
            var current = (long)(idProperty.GetValue(entity) ?? 0L);
            if (current == 0L)
            {
                idProperty.SetValue(entity, SnowFlakeSingle.Instance.NextId());
            }
        }
    }

    /// <summary>
    /// 单批插入（不超过 DefaultInsertBatchSize 时由 InsertEntitiesAsync 直接调用）
    /// </summary>
    private static async Task<int> InsertEntitiesCoreAsync<T>(
        ISqlSugarClient db,
        List<T> entities,
        PrimaryKeyTypeOptions options,
        CancellationToken cancellationToken) where T : class, new()
    {
        var entityType = typeof(T);
        var idProperty = ResolveIdProperty(entityType);
        var idType = idProperty?.PropertyType;
        if (IsIncrementEntity(entityType) && options.Identity.Enabled)
        {
            return await db.Insertable(entities).ExecuteCommandAsync(cancellationToken);
        }

        if (idType == typeof(Guid) && options.Guid.Enabled && IsGuidEntity(typeof(T)))
        {
            foreach (var entity in entities)
            {
                AssignGuidIfEmpty(entity);
            }

            return await db.Insertable(entities).ExecuteCommandAsync(cancellationToken);
        }

        if (idType == typeof(long) && options.Snowflake.Enabled && IsSnowflakeEntity(typeof(T)))
        {
            var ids = await db.Insertable(entities).ExecuteReturnSnowflakeIdListAsync(cancellationToken);
            return ids.Count;
        }

        return await db.Insertable(entities).ExecuteCommandAsync(cancellationToken);
    }

    /// <summary>
    /// 动态表字典行插入（审批网关等无强类型实体场景；按 long 主键列处理，优先雪花再自增）
    /// </summary>
    /// <param name="db">SqlSugar 客户端</param>
    /// <param name="data">列名到值的字典或匿名对象</param>
    /// <param name="tableName">目标表名</param>
    /// <param name="options">主键类型配置</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>新主键 long 值；雪花/自增由 SqlSugar 返回；否则从 data 的 id 键读取，缺失返回 0</returns>
    /// <exception cref="ArgumentNullException"><paramref name="db"/>、<paramref name="data"/> 或 <paramref name="options"/> 为 null</exception>
    /// <exception cref="ArgumentException"><paramref name="tableName"/> 为空或空白</exception>
    public static async Task<long> InsertDictionaryAsync(
        ISqlSugarClient db,
        object data,
        string tableName,
        PrimaryKeyTypeOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(db);
        ArgumentNullException.ThrowIfNull(data);
        ArgumentException.ThrowIfNullOrWhiteSpace(tableName);
        ArgumentNullException.ThrowIfNull(options);

        var insertable = db.Insertable(data).AS(tableName);
        if (options.Snowflake.Enabled)
        {
            return await insertable.ExecuteReturnSnowflakeIdAsync(cancellationToken);
        }

        if (options.Identity.Enabled)
        {
            return await insertable.ExecuteReturnBigIdentityAsync(cancellationToken);
        }

        await insertable.ExecuteCommandAsync(cancellationToken);
        if (data is IDictionary<string, object> dict && dict.TryGetValue("id", out var idObj) && idObj != null)
        {
            return Convert.ToInt64(idObj);
        }

        return 0;
    }

    /// <summary>
    /// 单条插入后读取 long 型 Id（封装 InsertEntityAsync，供仅关心数值主键的调用方）
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <param name="db">SqlSugar 客户端</param>
    /// <param name="entity">待插入实体</param>
    /// <param name="options">主键类型配置</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>插入后的 long 型 Id；Id 为 int 时转为 long；无 Id 属性或为 null 时返回 0；Guid 等非数值类型尝试 Convert.ToInt64</returns>
    /// <exception cref="ArgumentNullException"><paramref name="db"/>、<paramref name="entity"/> 或 <paramref name="options"/> 为 null</exception>
    public static async Task<long> InsertEntityReturnInt64Async<T>(
        ISqlSugarClient db,
        T entity,
        PrimaryKeyTypeOptions options,
        CancellationToken cancellationToken = default) where T : class, new()
    {
        await InsertEntityAsync(db, entity, options, cancellationToken);
        var idProperty = ResolveIdProperty(typeof(T));
        if (idProperty == null)
        {
            return 0;
        }

        var value = idProperty.GetValue(entity);
        return value switch
        {
            long l => l,
            int i => i,
            null => 0,
            _ => Convert.ToInt64(value)
        };
    }

    /// <summary>
    /// 实体 Id 列标注 SugarColumn.IsIdentity 时为库自增主键（TaktEntityIncrementBase 等）
    /// </summary>
    /// <param name="entityType">实体类型</param>
    /// <returns>自增主键时为 true</returns>
    /// <exception cref="ArgumentNullException"><paramref name="entityType"/> 为 null</exception>
    public static bool IsIncrementEntity(Type entityType)
    {
        ArgumentNullException.ThrowIfNull(entityType);
        return IsIdColumnIdentity(entityType);
    }

    /// <summary>
    /// long 型 Id 且 Id 列非 IsIdentity 时为雪花主键（TaktTenantEntityBase 等默认业务实体）
    /// </summary>
    /// <param name="entityType">实体类型</param>
    /// <returns>雪花主键时为 true</returns>
    /// <exception cref="ArgumentNullException"><paramref name="entityType"/> 为 null</exception>
    public static bool IsSnowflakeEntity(Type entityType)
    {
        ArgumentNullException.ThrowIfNull(entityType);
        var idProperty = ResolveIdProperty(entityType);
        return idProperty?.PropertyType == typeof(long) && !IsIdColumnIdentity(entityType);
    }

    /// <summary>
    /// Id 属性类型为 Guid 时走 GUID 主键策略（TaktTenantEntityGuidBase 等）
    /// </summary>
    /// <param name="entityType">实体类型</param>
    /// <returns>GUID 主键时为 true</returns>
    /// <exception cref="ArgumentNullException"><paramref name="entityType"/> 为 null</exception>
    public static bool IsGuidEntity(Type entityType)
    {
        ArgumentNullException.ThrowIfNull(entityType);
        return ResolveIdProperty(entityType)?.PropertyType == typeof(Guid);
    }

    /// <summary>
    /// 读取 Id 属性上 SugarColumn.IsIdentity 是否为 true
    /// </summary>
    /// <param name="entityType">实体类型</param>
    /// <returns>Id 列为自增时为 true；无 Id 或未标注时为 false</returns>
    private static bool IsIdColumnIdentity(Type entityType)
    {
        var idProperty = ResolveIdProperty(entityType);
        if (idProperty == null)
        {
            return false;
        }

        return idProperty.GetCustomAttribute<SugarColumn>()?.IsIdentity == true;
    }

    /// <summary>
    /// 自增主键异步插入：ExecuteReturnBigIdentityAsync 或 ExecuteReturnIdentityAsync 并写回 entity.Id
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <param name="insertable">SqlSugar 插入构建器</param>
    /// <param name="entity">待插入实体</param>
    /// <param name="idProperty">Id 属性反射信息</param>
    /// <param name="idType">Id 属性类型</param>
    /// <param name="options">主键类型配置</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>已按自增策略插入并回填 Id 时为 true；否则为 false 由调用方继续其他策略</returns>
    private static async Task<bool> TryInsertWithIdentityAsync<T>(
        IInsertable<T> insertable,
        T entity,
        PropertyInfo? idProperty,
        Type? idType,
        PrimaryKeyTypeOptions options,
        CancellationToken cancellationToken) where T : class, new()
    {
        if (!options.Identity.Enabled || idProperty == null || !IsIncrementEntity(typeof(T)))
        {
            return false;
        }

        if (idType == typeof(long))
        {
            var newId = await insertable.ExecuteReturnBigIdentityAsync(cancellationToken);
            idProperty.SetValue(entity, newId);
            return true;
        }

        if (IsIntIdType(idType))
        {
            var newId = await insertable.ExecuteReturnIdentityAsync(cancellationToken);
            SetIntId(entity, idProperty, newId);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 自增主键同步插入：策略同 TryInsertWithIdentityAsync
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <param name="insertable">SqlSugar 插入构建器</param>
    /// <param name="entity">待插入实体</param>
    /// <param name="idProperty">Id 属性反射信息</param>
    /// <param name="idType">Id 属性类型</param>
    /// <param name="options">主键类型配置</param>
    /// <returns>已按自增策略插入并回填 Id 时为 true；否则为 false</returns>
    private static bool TryInsertWithIdentitySync<T>(
        IInsertable<T> insertable,
        T entity,
        PropertyInfo? idProperty,
        Type? idType,
        PrimaryKeyTypeOptions options) where T : class, new()
    {
        if (!options.Identity.Enabled || idProperty == null || !IsIncrementEntity(typeof(T)))
        {
            return false;
        }

        if (idType == typeof(long))
        {
            var newId = insertable.ExecuteReturnBigIdentity();
            idProperty.SetValue(entity, newId);
            return true;
        }

        if (IsIntIdType(idType))
        {
            var newId = insertable.ExecuteReturnIdentity();
            SetIntId(entity, idProperty, newId);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Id 属性是否为 int 或 int?（库 int 自增列）
    /// </summary>
    /// <param name="idType">Id 属性类型</param>
    /// <returns>int 或 int? 时为 true</returns>
    private static bool IsIntIdType(Type? idType) =>
        idType == typeof(int) || idType == typeof(int?);

    /// <summary>
    /// 将库返回的 int 自增值写入 entity.Id（支持 int 与 int?）
    /// </summary>
    /// <param name="entity">实体实例</param>
    /// <param name="idProperty">Id 属性；为 null 时不写入</param>
    /// <param name="newId">数据库返回的自增 Id</param>
    private static void SetIntId(object entity, PropertyInfo? idProperty, int newId)
    {
        if (idProperty == null)
        {
            return;
        }

        idProperty.SetValue(entity, newId);
    }
}
