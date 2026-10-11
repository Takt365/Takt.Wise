// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Code.Database
// 文件名称：TaktDatabaseBackupsController.cs
// 创建时间：2026-07-19
// 创建人：Takt365(Cursor AI)
// 功能描述：数据库备份控制器（标准 CRUD + 按 Id 立即/后台调度 + 路径选项）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Code.Database;
using Takt.Application.Services.Code.Database;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;

namespace Takt.WebApi.Controllers.Code.Database;

/// <summary>
/// 数据库备份控制器
/// </summary>
[ApiModule(7, "代码管理")]
[Route("api/[controller]", Name = "数据库备份")]
public class TaktDatabaseBackupsController : TaktControllerBase
{
    private readonly ITaktDatabaseBackupService _databaseBackupService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="databaseBackupService">数据库备份服务</param>
    public TaktDatabaseBackupsController(ITaktDatabaseBackupService databaseBackupService)
    {
        _databaseBackupService = databaseBackupService;
    }

    /// <summary>
    /// 获取数据库备份列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("code:database:backup:list", "数据库备份列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetDatabaseBackupListAsync([FromQuery] TaktDatabaseBackupQueryDto queryDto)
    {
        try
        {
            var result = await _databaseBackupService.GetDatabaseBackupListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取数据库备份
    /// </summary>
    /// <param name="id">数据库备份ID</param>
    /// <returns>数据库备份DTO</returns>
    [TaktPermission("code:database:backup:query", "数据库备份详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDatabaseBackupByIdAsync(long id)
    {
        try
        {
            var result = await _databaseBackupService.GetDatabaseBackupByIdAsync(id);
            if (result == null)
            {
                return NotFound("数据库备份不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取备份路径选项
    /// </summary>
    /// <returns>默认根目录与允许根目录列表</returns>
    [TaktPermission("code:database:backup:query", "数据库备份路径选项")]
    [HttpGet("path-options")]
    public IActionResult GetDatabaseBackupPathOptions()
    {
        try
        {
            var result = _databaseBackupService.GetDatabaseBackupPathOptions();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 浏览本地目录
    /// </summary>
    [TaktPermission("code:database:backup:query", "浏览本地备份目录")]
    [HttpPost("browse/local")]
    public async Task<IActionResult> BrowseLocalAsync([FromBody] TaktDatabaseBackupBrowseLocalDto dto)
    {
        try
        {
            var result = await _databaseBackupService.BrowseLocalAsync(dto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 浏览网络 UNC 目录
    /// </summary>
    [TaktPermission("code:database:backup:query", "浏览网络备份目录")]
    [HttpPost("browse/network")]
    public async Task<IActionResult> BrowseNetworkAsync([FromBody] TaktDatabaseBackupBrowseNetworkDto dto)
    {
        try
        {
            var result = await _databaseBackupService.BrowseNetworkAsync(dto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 浏览 FTP 目录
    /// </summary>
    [TaktPermission("code:database:backup:query", "浏览FTP备份目录")]
    [HttpPost("browse/ftp")]
    public async Task<IActionResult> BrowseFtpAsync([FromBody] TaktDatabaseBackupBrowseFtpDto dto)
    {
        try
        {
            var result = await _databaseBackupService.BrowseFtpAsync(dto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建本地目录（API 宿主任意路径，无固定白名单）
    /// </summary>
    [TaktPermission("code:database:backup:create", "创建本地备份目录")]
    [HttpPost("mkdir/local")]
    public async Task<IActionResult> CreateLocalDirectoryAsync([FromBody] TaktDatabaseBackupMkdirLocalDto dto)
    {
        try
        {
            var path = await _databaseBackupService.CreateLocalDirectoryAsync(dto);
            return Success(new { path }, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建网络 UNC 目录
    /// </summary>
    [TaktPermission("code:database:backup:create", "创建网络备份目录")]
    [HttpPost("mkdir/network")]
    public async Task<IActionResult> CreateNetworkDirectoryAsync([FromBody] TaktDatabaseBackupBrowseNetworkDto dto)
    {
        try
        {
            var path = await _databaseBackupService.CreateNetworkDirectoryAsync(dto);
            return Success(new { path }, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建 FTP 远程目录
    /// </summary>
    [TaktPermission("code:database:backup:create", "创建FTP备份目录")]
    [HttpPost("mkdir/ftp")]
    public async Task<IActionResult> CreateFtpDirectoryAsync([FromBody] TaktDatabaseBackupBrowseFtpDto dto)
    {
        try
        {
            var path = await _databaseBackupService.CreateFtpDirectoryAsync(dto);
            return Success(new { path }, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建数据库备份配置
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>备份记录</returns>
    [TaktPermission("code:database:backup:create", "创建数据库备份")]
    [HttpPost]
    public async Task<IActionResult> CreateDatabaseBackupAsync([FromBody] TaktDatabaseBackupCreateDto dto)
    {
        try
        {
            var result = await _databaseBackupService.CreateDatabaseBackupAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新数据库备份配置
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>备份记录</returns>
    [TaktPermission("code:database:backup:update", "更新数据库备份")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDatabaseBackupAsync(long id, [FromBody] TaktDatabaseBackupUpdateDto dto)
    {
        try
        {
            var result = await _databaseBackupService.UpdateDatabaseBackupAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 按记录立即执行（创建 Quartz 任务）
    /// </summary>
    /// <param name="id">备份记录主键</param>
    /// <returns>备份记录</returns>
    [TaktPermission("code:database:backup:run", "立即数据库备份")]
    [HttpPost("{id}/run")]
    public async Task<IActionResult> RunDatabaseBackupByIdAsync(long id)
    {
        try
        {
            var result = await _databaseBackupService.RunDatabaseBackupByIdAsync(id);
            return Success(result, "已创建立即备份任务");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 按记录后台调度（创建 Quartz 任务）
    /// </summary>
    /// <param name="id">备份记录主键</param>
    /// <param name="dto">调度参数</param>
    /// <returns>备份记录</returns>
    [TaktPermission("code:database:backup:schedule", "调度数据库备份")]
    [HttpPost("{id}/schedule")]
    public async Task<IActionResult> ScheduleDatabaseBackupByIdAsync(long id, [FromBody] TaktDatabaseBackupScheduleByIdDto dto)
    {
        try
        {
            var result = await _databaseBackupService.ScheduleDatabaseBackupByIdAsync(id, dto);
            return Success(result, "已创建后台备份任务");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除数据库备份
    /// </summary>
    /// <param name="id">数据库备份ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("code:database:backup:delete", "删除数据库备份")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDatabaseBackupByIdAsync(long id)
    {
        try
        {
            await _databaseBackupService.DeleteDatabaseBackupByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除数据库备份
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("code:database:backup:delete", "批量删除数据库备份")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteDatabaseBackupBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _databaseBackupService.DeleteDatabaseBackupBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出数据库备份
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="exportName">文件名</param>
    /// <returns>Excel 文件</returns>
    [TaktPermission("code:database:backup:export", "导出数据库备份")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportDatabaseBackupAsync(
        [FromQuery] TaktDatabaseBackupQueryDto? query,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _databaseBackupService.ExportDatabaseBackupAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
