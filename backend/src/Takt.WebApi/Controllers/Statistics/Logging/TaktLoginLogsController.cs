// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Statistics.Logging
// 文件名称：TaktLoginLogsController.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Cursor AI)
// 功能描述：登录日志控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Statistics.Logging;
using Takt.Application.Services.Statistics.Logging;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Statistics.Logging;

/// <summary>
/// 登录日志控制器
/// 提供登录日志的 REST API
/// </summary>
[ApiModule(9, "统计日志")]
[Route("api/[controller]", Name = "登录日志")]
public class TaktLoginLogsController : TaktControllerBase
{
    private readonly ITaktLoginLogService _loginLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="loginLogService">登录日志服务</param>
    public TaktLoginLogsController(ITaktLoginLogService loginLogService)
    {
        _loginLogService = loginLogService;
    }

    /// <summary>
    /// 获取当前登录用户的登录日志列表（分页；个人中心，无需管理端 list 权限）
    /// </summary>
    /// <param name="queryDto">查询DTO（高级条件可用；UserName 由服务端强制为当前用户）</param>
    /// <returns>分页结果</returns>
    [HttpGet("me/list")]
    public async Task<IActionResult> GetMyLoginLogListAsync([FromQuery] TaktLoginLogQueryDto queryDto)
    {
        try
        {
            var result = await _loginLogService.GetMyLoginLogListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取登录日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("statistics:logging:login:log:list", "登录日志列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetLoginLogListAsync([FromQuery] TaktLoginLogQueryDto queryDto)
    {
        try
        {
            var result = await _loginLogService.GetLoginLogListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取登录日志
    /// </summary>
    /// <param name="id">登录日志ID</param>
    /// <returns>登录日志DTO</returns>
    [TaktPermission("statistics:logging:login:log:query", "登录日志详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetLoginLogByIdAsync(long id)
    {
        try
        {
            var result = await _loginLogService.GetLoginLogByIdAsync(id);
            if (result == null)
            {
                return NotFound("登录日志不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取登录日志选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("statistics:logging:login:log:query", "登录日志选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetLoginLogOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _loginLogService.GetLoginLogOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建登录日志
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>登录日志DTO</returns>
    [TaktPermission("statistics:logging:login:log:create", "创建登录日志")]
    [HttpPost]
    public async Task<IActionResult> CreateLoginLogAsync([FromBody] TaktLoginLogCreateDto dto)
    {
        try
        {
            var result = await _loginLogService.CreateLoginLogAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新登录日志
    /// </summary>
    /// <param name="id">登录日志ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>登录日志DTO</returns>
    [TaktPermission("statistics:logging:login:log:update", "更新登录日志")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLoginLogAsync(long id, [FromBody] TaktLoginLogUpdateDto dto)
    {
        try
        {
            var result = await _loginLogService.UpdateLoginLogAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除登录日志
    /// </summary>
    /// <param name="id">登录日志ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("statistics:logging:login:log:delete", "删除登录日志")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLoginLogByIdAsync(long id)
    {
        try
        {
            await _loginLogService.DeleteLoginLogByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除登录日志
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("statistics:logging:login:log:delete", "批量删除登录日志")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteLoginLogBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _loginLogService.DeleteLoginLogBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出登录日志
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("statistics:logging:login:log:export", "导出登录日志")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportLoginLogAsync([FromQuery] TaktLoginLogQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _loginLogService.ExportLoginLogAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
