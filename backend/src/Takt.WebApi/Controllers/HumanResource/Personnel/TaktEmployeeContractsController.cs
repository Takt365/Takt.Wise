// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.HumanResource.Personnel
// 文件名称：TaktEmployeeContractsController.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：员工劳动合同控制器
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
/// 员工劳动合同控制器
/// 提供员工劳动合同的 REST API
/// </summary>
[ApiModule(5, "人事管理")]
[Route("api/[controller]", Name = "员工劳动合同")]
public class TaktEmployeeContractsController : TaktControllerBase
{
    private readonly ITaktEmployeeContractService _employeeContractService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="employeeContractService">员工劳动合同服务</param>
    public TaktEmployeeContractsController(ITaktEmployeeContractService employeeContractService)
    {
        _employeeContractService = employeeContractService;
    }

    /// <summary>
    /// 获取员工劳动合同列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("human:resource:personnel:employee:contract:list", "员工劳动合同列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEmployeeContractListAsync([FromQuery] TaktEmployeeContractQueryDto queryDto)
    {
        try
        {
            var result = await _employeeContractService.GetEmployeeContractListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取员工劳动合同
    /// </summary>
    /// <param name="id">员工劳动合同ID</param>
    /// <returns>员工劳动合同DTO</returns>
    [TaktPermission("human:resource:personnel:employee:contract:query", "员工劳动合同详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployeeContractByIdAsync(long id)
    {
        try
        {
            var result = await _employeeContractService.GetEmployeeContractByIdAsync(id);
            if (result == null)
            {
                return NotFound("员工劳动合同不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取员工劳动合同选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("human:resource:personnel:employee:contract:query", "员工劳动合同选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetEmployeeContractOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _employeeContractService.GetEmployeeContractOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建员工劳动合同
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>员工劳动合同DTO</returns>
    [TaktPermission("human:resource:personnel:employee:contract:create", "创建员工劳动合同")]
    [HttpPost]
    public async Task<IActionResult> CreateEmployeeContractAsync([FromBody] TaktEmployeeContractCreateDto dto)
    {
        try
        {
            var result = await _employeeContractService.CreateEmployeeContractAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新员工劳动合同
    /// </summary>
    /// <param name="id">员工劳动合同ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>员工劳动合同DTO</returns>
    [TaktPermission("human:resource:personnel:employee:contract:update", "更新员工劳动合同")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployeeContractAsync(long id, [FromBody] TaktEmployeeContractUpdateDto dto)
    {
        try
        {
            var result = await _employeeContractService.UpdateEmployeeContractAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除员工劳动合同
    /// </summary>
    /// <param name="id">员工劳动合同ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:personnel:employee:contract:delete", "删除员工劳动合同")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployeeContractByIdAsync(long id)
    {
        try
        {
            await _employeeContractService.DeleteEmployeeContractByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除员工劳动合同
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:personnel:employee:contract:delete", "批量删除员工劳动合同")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteEmployeeContractBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _employeeContractService.DeleteEmployeeContractBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新员工劳动合同状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>员工劳动合同DTO</returns>
    [TaktPermission("human:resource:personnel:employee:contract:update", "更新员工劳动合同状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateEmployeeContractStatusAsync([FromBody] TaktEmployeeContractStatusDto dto)
    {
        try
        {
            var result = await _employeeContractService.UpdateEmployeeContractStatusAsync(dto);
            return Success(result, "更新成功");
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
    [TaktPermission("human:resource:personnel:employee:contract:import", "获取员工劳动合同导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetEmployeeContractTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _employeeContractService.GetEmployeeContractTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入员工劳动合同
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("human:resource:personnel:employee:contract:import", "导入员工劳动合同")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportEmployeeContractAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _employeeContractService.ImportEmployeeContractAsync(stream, sheetName);
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
    /// 导出员工劳动合同
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:personnel:employee:contract:export", "导出员工劳动合同")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEmployeeContractAsync([FromQuery] TaktEmployeeContractQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _employeeContractService.ExportEmployeeContractAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
