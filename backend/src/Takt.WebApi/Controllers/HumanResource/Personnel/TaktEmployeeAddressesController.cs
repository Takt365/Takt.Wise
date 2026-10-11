// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.HumanResource.Personnel
// 文件名称：TaktEmployeeAddressesController.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：员工地址控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.HumanResource.Personnel;
using Takt.Application.Services.HumanResource.Personnel;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.HumanResource.Personnel;

/// <summary>
/// 员工地址控制器
/// 提供员工地址的 REST API
/// </summary>
[ApiModule(5, "人事管理")]
[Route("api/[controller]", Name = "员工地址")]
public class TaktEmployeeAddressesController : TaktControllerBase
{
    private readonly ITaktEmployeeAddressService _employeeAddressService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="employeeAddressService">员工地址服务</param>
    public TaktEmployeeAddressesController(ITaktEmployeeAddressService employeeAddressService)
    {
        _employeeAddressService = employeeAddressService;
    }

    /// <summary>
    /// 获取员工地址列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("human:resource:personnel:employee:address:list", "员工地址列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEmployeeAddressListAsync([FromQuery] TaktEmployeeAddressQueryDto queryDto)
    {
        try
        {
            var result = await _employeeAddressService.GetEmployeeAddressListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取员工地址
    /// </summary>
    /// <param name="id">员工地址ID</param>
    /// <returns>员工地址DTO</returns>
    [TaktPermission("human:resource:personnel:employee:address:query", "员工地址详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployeeAddressByIdAsync(long id)
    {
        try
        {
            var result = await _employeeAddressService.GetEmployeeAddressByIdAsync(id);
            if (result == null)
            {
                return NotFound("员工地址不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取员工地址选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("human:resource:personnel:employee:address:query", "员工地址选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetEmployeeAddressOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _employeeAddressService.GetEmployeeAddressOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建员工地址
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>员工地址DTO</returns>
    [TaktPermission("human:resource:personnel:employee:address:create", "创建员工地址")]
    [HttpPost]
    public async Task<IActionResult> CreateEmployeeAddressAsync([FromBody] TaktEmployeeAddressCreateDto dto)
    {
        try
        {
            var result = await _employeeAddressService.CreateEmployeeAddressAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新员工地址
    /// </summary>
    /// <param name="id">员工地址ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>员工地址DTO</returns>
    [TaktPermission("human:resource:personnel:employee:address:update", "更新员工地址")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployeeAddressAsync(long id, [FromBody] TaktEmployeeAddressUpdateDto dto)
    {
        try
        {
            var result = await _employeeAddressService.UpdateEmployeeAddressAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除员工地址
    /// </summary>
    /// <param name="id">员工地址ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:personnel:employee:address:delete", "删除员工地址")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployeeAddressByIdAsync(long id)
    {
        try
        {
            await _employeeAddressService.DeleteEmployeeAddressByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除员工地址
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:personnel:employee:address:delete", "批量删除员工地址")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteEmployeeAddressBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _employeeAddressService.DeleteEmployeeAddressBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:personnel:employee:address:import", "获取员工地址导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetEmployeeAddressTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _employeeAddressService.GetEmployeeAddressTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入员工地址
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("human:resource:personnel:employee:address:import", "导入员工地址")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportEmployeeAddressAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _employeeAddressService.ImportEmployeeAddressAsync(stream, sheetName);
            return Success(new
            {
                SuccessCount = success,
                FailCount = fail,
                Errors = errors
            }, $"导入完成：成功{success}条，失败{fail}条");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出员工地址
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:personnel:employee:address:export", "导出员工地址")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEmployeeAddressAsync([FromQuery] TaktEmployeeAddressQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _employeeAddressService.ExportEmployeeAddressAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
