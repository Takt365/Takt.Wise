// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Sales
// 文件名称：TaktClientsController.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Cursor AI)
// 功能描述：客户端信息控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Sales;
using Takt.Application.Services.Logistics.Sales;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Sales;

/// <summary>
/// 客户端信息控制器
/// 提供客户端信息的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "客户端信息")]
public class TaktClientsController : TaktControllerBase
{
    private readonly ITaktClientService _clientService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="clientService">客户端信息服务</param>
    public TaktClientsController(ITaktClientService clientService)
    {
        _clientService = clientService;
    }

    /// <summary>
    /// 获取客户端信息列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:sales:client:list", "客户端信息列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetClientListAsync([FromQuery] TaktClientQueryDto queryDto)
    {
        try
        {
            var result = await _clientService.GetClientListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取客户端信息
    /// </summary>
    /// <param name="id">客户端信息ID</param>
    /// <returns>客户端信息DTO</returns>
    [TaktPermission("logistics:sales:client:query", "客户端信息详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetClientByIdAsync(long id)
    {
        try
        {
            var result = await _clientService.GetClientByIdAsync(id);
            if (result == null)
            {
                return NotFound("客户端信息不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取客户端信息选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:sales:client:query", "客户端信息选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetClientOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _clientService.GetClientOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建客户端信息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>客户端信息DTO</returns>
    [TaktPermission("logistics:sales:client:create", "创建客户端信息")]
    [HttpPost]
    public async Task<IActionResult> CreateClientAsync([FromBody] TaktClientCreateDto dto)
    {
        try
        {
            var result = await _clientService.CreateClientAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新客户端信息
    /// </summary>
    /// <param name="id">客户端信息ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>客户端信息DTO</returns>
    [TaktPermission("logistics:sales:client:update", "更新客户端信息")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateClientAsync(long id, [FromBody] TaktClientUpdateDto dto)
    {
        try
        {
            var result = await _clientService.UpdateClientAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除客户端信息
    /// </summary>
    /// <param name="id">客户端信息ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:sales:client:delete", "删除客户端信息")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClientByIdAsync(long id)
    {
        try
        {
            await _clientService.DeleteClientByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除客户端信息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:sales:client:delete", "批量删除客户端信息")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteClientBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _clientService.DeleteClientBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新客户端信息状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>客户端信息DTO</returns>
    [TaktPermission("logistics:sales:client:update", "更新客户端信息状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateClientStatusAsync([FromBody] TaktClientStatusDto dto)
    {
        try
        {
            var result = await _clientService.UpdateClientStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新客户端信息排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>客户端信息DTO</returns>
    [TaktPermission("logistics:sales:client:update", "更新客户端信息排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateClientSortAsync([FromBody] TaktClientSortDto dto)
    {
        try
        {
            var result = await _clientService.UpdateClientSortAsync(dto);
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
    [TaktPermission("logistics:sales:client:import", "获取客户端信息导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetClientTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _clientService.GetClientTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入客户端信息
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:sales:client:import", "导入客户端信息")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportClientAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _clientService.ImportClientAsync(stream, sheetName);
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
    /// 导出客户端信息
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:sales:client:export", "导出客户端信息")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportClientAsync([FromQuery] TaktClientQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _clientService.ExportClientAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
