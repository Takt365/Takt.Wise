// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Foundation
// 文件名称：TaktDictTypesController.cs
// 创建时间：2026-06-02
// 创建人：Takt365(Cursor AI)
// 功能描述：字典类型控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Foundation;
using Takt.Application.Services.Foundation;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Foundation;

/// <summary>
/// 字典类型控制器
/// 提供字典类型的 REST API
/// </summary>
[ApiModule(8, "基础设置")]
[Route("api/[controller]", Name = "字典类型")]
public class TaktDictTypesController : TaktControllerBase
{
    private readonly ITaktDictTypeService _dictTypeService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="dictTypeService">字典类型服务</param>
    public TaktDictTypesController(ITaktDictTypeService dictTypeService)
    {
        _dictTypeService = dictTypeService;
    }

    /// <summary>
    /// 获取字典类型列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("foundation:dict:list", "字典类型列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetDictTypeListAsync([FromQuery] TaktDictTypeQueryDto queryDto)
    {
        try
        {
            var result = await _dictTypeService.GetDictTypeListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取字典类型
    /// </summary>
    /// <param name="id">字典类型ID</param>
    /// <returns>字典类型DTO</returns>
    [TaktPermission("foundation:dict:query", "字典类型详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDictTypeByIdAsync(long id)
    {
        try
        {
            var result = await _dictTypeService.GetDictTypeByIdAsync(id);
            if (result == null)
            {
                return NotFound("字典类型不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取字典类型选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("foundation:dict:query", "字典类型选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetDictTypeOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _dictTypeService.GetDictTypeOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建字典类型
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>字典类型DTO</returns>
    [TaktPermission("foundation:dict:create", "创建字典类型")]
    [HttpPost]
    public async Task<IActionResult> CreateDictTypeAsync([FromBody] TaktDictTypeCreateDto dto)
    {
        try
        {
            var result = await _dictTypeService.CreateDictTypeAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新字典类型
    /// </summary>
    /// <param name="id">字典类型ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>字典类型DTO</returns>
    [TaktPermission("foundation:dict:update", "更新字典类型")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDictTypeAsync(long id, [FromBody] TaktDictTypeUpdateDto dto)
    {
        try
        {
            var result = await _dictTypeService.UpdateDictTypeAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除字典类型
    /// </summary>
    /// <param name="id">字典类型ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:dict:delete", "删除字典类型")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDictTypeByIdAsync(long id)
    {
        try
        {
            await _dictTypeService.DeleteDictTypeByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除字典类型
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:dict:delete", "批量删除字典类型")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteDictTypeBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _dictTypeService.DeleteDictTypeBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新字典类型状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>字典类型DTO</returns>
    [TaktPermission("foundation:dict:update", "更新字典类型状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateDictTypeStatusAsync([FromBody] TaktDictTypeStatusDto dto)
    {
        try
        {
            var result = await _dictTypeService.UpdateDictTypeStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新字典类型内置
    /// </summary>
    /// <param name="dto">内置 DTO</param>
    /// <returns>字典类型DTO</returns>
    [TaktPermission("foundation:dict:update", "更新字典类型内置")]
    [HttpPut("built-in")]
    public async Task<IActionResult> UpdateDictTypeBuiltInAsync([FromBody] TaktDictTypeBuiltInDto dto)
    {
        try
        {
            var result = await _dictTypeService.UpdateDictTypeBuiltInAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新字典类型排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>字典类型DTO</returns>
    [TaktPermission("foundation:dict:update", "更新字典类型排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateDictTypeSortAsync([FromBody] TaktDictTypeSortDto dto)
    {
        try
        {
            var result = await _dictTypeService.UpdateDictTypeSortAsync(dto);
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
    [TaktPermission("foundation:dict:import", "获取字典类型导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetDictTypeTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _dictTypeService.GetDictTypeTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入字典类型
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("foundation:dict:import", "导入字典类型")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportDictTypeAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _dictTypeService.ImportDictTypeAsync(stream, sheetName);
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
    /// 导出字典类型
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("foundation:dict:export", "导出字典类型")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportDictTypeAsync([FromQuery] TaktDictTypeQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _dictTypeService.ExportDictTypeAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
