// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Code.Database
// 文件名称：TaktDatabaseInfosController.cs
// 创建时间：2026-06-02
// 创建人：Takt365(Cursor AI)
// 功能描述：数据库 introspect 控制器（对齐 TaktDatabaseInfoDtos）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Code.Database;
using Takt.Application.Services.Code.Database;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Code.Database;

/// <summary>
/// 数据库 introspect 控制器
/// </summary>
[ApiModule(7, "代码管理")]
[Route("api/[controller]", Name = "数据库 introspect")]
public class TaktDatabaseInfosController : TaktControllerBase
{
    private readonly ITaktDatabaseInfoService _databaseInfoService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="databaseInfoService">数据库 introspect 服务</param>
    public TaktDatabaseInfosController(ITaktDatabaseInfoService databaseInfoService)
    {
        _databaseInfoService = databaseInfoService;
    }

    /// <summary>
    /// 获取可 introspect 的租户业务库列表
    /// </summary>
    /// <returns>数据库摘要列表</returns>
    [TaktPermission("code:database:info:list", "数据库摘要列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetDatabaseInfoListAsync()
    {
        try
        {
            var result = await _databaseInfoService.GetDatabaseInfoListAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取指定租户库下所有用户表摘要
    /// </summary>
    /// <param name="tenantCode">租户编码（3 位）</param>
    /// <returns>表摘要列表</returns>
    [TaktPermission("code:database:info:query", "数据库表摘要列表")]
    [HttpGet("tables")]
    public async Task<IActionResult> GetDatabaseTableInfoListAsync([FromQuery] string tenantCode)
    {
        try
        {
            var result = await _databaseInfoService.GetDatabaseTableInfoListAsync(tenantCode);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 分页获取当前登录租户业务库下用户表摘要
    /// </summary>
    /// <param name="queryDto">分页与关键字查询</param>
    /// <returns>分页结果</returns>
    [TaktPermission("code:database:info:list", "数据库表摘要分页列表")]
    [HttpGet("table-list")]
    public async Task<IActionResult> GetDatabaseTableInfoPageListAsync([FromQuery] TaktDatabaseTableInfoQueryDto queryDto)
    {
        try
        {
            var result = await _databaseInfoService.GetDatabaseTableInfoPageListAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取指定物理表的列摘要
    /// </summary>
    /// <param name="tenantCode">租户编码（3 位）</param>
    /// <param name="tableName">表名</param>
    /// <returns>列摘要列表</returns>
    [TaktPermission("code:database:info:query", "数据库表列摘要列表")]
    [HttpGet("columns")]
    public async Task<IActionResult> GetDatabaseTableColumnInfoListAsync([FromQuery] string tenantCode, [FromQuery] string tableName)
    {
        try
        {
            var result = await _databaseInfoService.GetDatabaseTableColumnInfoListAsync(tenantCode, tableName);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
