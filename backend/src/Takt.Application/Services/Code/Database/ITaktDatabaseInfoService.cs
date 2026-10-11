// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Code.Database
// 文件名称：ITaktDatabaseInfoService.cs
// 创建时间：2026-06-02
// 创建人：Takt365(Cursor AI)
// 功能描述：数据库 introspect 应用服务接口（对齐 TaktDatabaseInfoDtos）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Code.Database;
using Takt.Shared.Models;

namespace Takt.Application.Services.Code.Database;

/// <summary>
/// 数据库 introspect 应用服务
/// </summary>
public interface ITaktDatabaseInfoService
{
    /// <summary>
    /// 获取可 introspect 的租户业务库列表
    /// </summary>
    /// <returns>数据库摘要列表</returns>
    Task<List<TaktDatabaseInfoDto>> GetDatabaseInfoListAsync();

    /// <summary>
    /// 获取指定租户库下所有用户表摘要
    /// </summary>
    /// <param name="tenantCode">租户编码（3 位）</param>
    /// <returns>表摘要列表</returns>
    Task<List<TaktDatabaseTableInfoDto>> GetDatabaseTableInfoListAsync(string tenantCode);

    /// <summary>
    /// 分页获取当前登录租户业务库下用户表摘要
    /// </summary>
    /// <param name="queryDto">分页与关键字查询</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktDatabaseTableInfoDto>> GetDatabaseTableInfoPageListAsync(TaktDatabaseTableInfoQueryDto queryDto);

    /// <summary>
    /// 获取指定物理表的列摘要
    /// </summary>
    /// <param name="tenantCode">租户编码（3 位）</param>
    /// <param name="tableName">表名</param>
    /// <returns>列摘要列表</returns>
    Task<List<TaktDatabaseTableColumnInfoDto>> GetDatabaseTableColumnInfoListAsync(string tenantCode, string tableName);
}
