// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Code.Database
// 文件名称：TaktDatabaseInfoService.cs
// 创建时间：2026-06-02
// 创建人：Takt365(Cursor AI)
// 功能描述：数据库 introspect 应用服务实现（对齐 TaktDatabaseInfoDtos）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Code.Database;
using Takt.Domain.Interfaces;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Services.Code.Database;

/// <summary>
/// 数据库 introspect 应用服务
/// </summary>
public class TaktDatabaseInfoService : TaktServiceBase, ITaktDatabaseInfoService
{
    private readonly ITaktDatabaseSchemaProvider _schemaProvider;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="schemaProvider">数据库元数据提供者</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktDatabaseInfoService(
        ITaktDatabaseSchemaProvider schemaProvider,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _schemaProvider = schemaProvider ?? throw new ArgumentNullException(nameof(schemaProvider));
    }

    /// <summary>
    /// 获取可 introspect 的租户业务库列表
    /// </summary>
    /// <returns>数据库摘要列表</returns>
    public async Task<List<TaktDatabaseInfoDto>> GetDatabaseInfoListAsync()
    {
        var list = await _schemaProvider.GetDatabasesAsync().ConfigureAwait(false);
        return list.Select(x => new TaktDatabaseInfoDto
        {
            TenantCode = x.TenantCode,
            DisplayName = x.DisplayName
        }).ToList();
    }

    /// <summary>
    /// 获取指定租户库下所有用户表摘要
    /// </summary>
    /// <param name="tenantCode">租户编码（3 位）</param>
    /// <returns>表摘要列表</returns>
    public async Task<List<TaktDatabaseTableInfoDto>> GetDatabaseTableInfoListAsync(string tenantCode)
    {
        if (string.IsNullOrWhiteSpace(tenantCode))
        {
            throw new TaktBusinessException("租户编码不能为空");
        }
        var tables = await _schemaProvider.GetTablesAsync(tenantCode.Trim()).ConfigureAwait(false);
        return tables
            .Select(t => new TaktDatabaseTableInfoDto
            {
                TableName = t.Name ?? string.Empty,
                TableComment = t.Description
            })
            .OrderBy(t => t.TableName, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    /// <summary>
    /// 分页获取当前登录租户业务库下用户表摘要
    /// </summary>
    /// <param name="queryDto">分页与关键字查询</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktDatabaseTableInfoDto>> GetDatabaseTableInfoPageListAsync(TaktDatabaseTableInfoQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        var tenantCode = CurrentTenantCode?.Trim();
        if (string.IsNullOrWhiteSpace(tenantCode))
        {
            throw new TaktBusinessException("租户编码不能为空");
        }
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var tables = await _schemaProvider.GetTablesAsync(tenantCode).ConfigureAwait(false);
        var list = tables.Select(t => new TaktDatabaseTableInfoDto
        {
            TableName = t.Name ?? string.Empty,
            TableComment = t.Description
        }).ToList();
        if (!string.IsNullOrWhiteSpace(queryDto.KeyWords))
        {
            var keyword = queryDto.KeyWords.Trim().ToLowerInvariant();
            list = list.Where(t =>
                t.TableName.ToLowerInvariant().Contains(keyword) ||
                (t.TableComment?.ToLowerInvariant().Contains(keyword) ?? false)).ToList();
        }
        list = list.OrderBy(t => t.TableName, StringComparer.OrdinalIgnoreCase).ToList();
        var total = list.Count;
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var pageData = list.Skip(skip).Take(pageSize).ToList();
        return TaktPagedResult<TaktDatabaseTableInfoDto>.Create(pageData, total, pageIndex, pageSize);
    }

    /// <summary>
    /// 获取指定物理表的列摘要
    /// </summary>
    /// <param name="tenantCode">租户编码（3 位）</param>
    /// <param name="tableName">表名</param>
    /// <returns>列摘要列表</returns>
    public async Task<List<TaktDatabaseTableColumnInfoDto>> GetDatabaseTableColumnInfoListAsync(string tenantCode, string tableName)
    {
        if (string.IsNullOrWhiteSpace(tenantCode))
        {
            throw new TaktBusinessException("租户编码不能为空");
        }
        if (string.IsNullOrWhiteSpace(tableName))
        {
            throw new TaktBusinessException("表名不能为空");
        }
        var columns = await _schemaProvider.GetColumnsAsync(tenantCode.Trim(), tableName.Trim()).ConfigureAwait(false);
        return columns
            .Select(c => new TaktDatabaseTableColumnInfoDto
            {
                DatabaseColumnName = c.DatabaseColumnName,
                ColumnComment = c.ColumnComment,
                DatabaseDataType = c.DatabaseDataType,
                Length = c.Length,
                DecimalDigits = c.DecimalDigits,
                IsPrimaryKey = c.IsPrimaryKey,
                IsIdentity = c.IsIdentity,
                IsNullable = c.IsNullable
            })
            .ToList();
    }
}
