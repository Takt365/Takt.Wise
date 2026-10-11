// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Foundation
// 文件名称：TaktDictDatasController.cs
// 创建时间：2026-06-27
// 创建人：Takt365(Cursor AI)
// 功能描述：字典数据控制器
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
/// 字典数据控制器
/// 提供字典数据的 REST API
/// </summary>
[ApiModule(8, "基础设置")]
[Route("api/[controller]", Name = "字典数据")]
public class TaktDictDatasController : TaktControllerBase
{
    private readonly ITaktDictDataService _dictDataService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="dictDataService">字典数据服务</param>
    public TaktDictDatasController(ITaktDictDataService dictDataService)
    {
        _dictDataService = dictDataService;
    }

    /// <summary>
    /// 获取字典数据列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("foundation:dict:list", "字典数据列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetDictDataListAsync([FromQuery] TaktDictDataQueryDto queryDto)
    {
        try
        {
            var result = await _dictDataService.GetDictDataListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取字典数据
    /// </summary>
    /// <param name="id">字典数据ID</param>
    /// <returns>字典数据DTO</returns>
    [TaktPermission("foundation:dict:query", "字典数据详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDictDataByIdAsync(long id)
    {
        try
        {
            var result = await _dictDataService.GetDictDataByIdAsync(id);
            if (result == null)
            {
                return NotFound("字典数据不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取字典数据选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("foundation:dict:query", "字典数据选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetDictDataOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _dictDataService.GetDictDataOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建字典数据
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>字典数据DTO</returns>
    [TaktPermission("foundation:dict:create", "创建字典数据")]
    [HttpPost]
    public async Task<IActionResult> CreateDictDataAsync([FromBody] TaktDictDataCreateDto dto)
    {
        try
        {
            var result = await _dictDataService.CreateDictDataAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新字典数据
    /// </summary>
    /// <param name="id">字典数据ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>字典数据DTO</returns>
    [TaktPermission("foundation:dict:update", "更新字典数据")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDictDataAsync(long id, [FromBody] TaktDictDataUpdateDto dto)
    {
        try
        {
            var result = await _dictDataService.UpdateDictDataAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除字典数据
    /// </summary>
    /// <param name="id">字典数据ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:dict:delete", "删除字典数据")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDictDataByIdAsync(long id)
    {
        try
        {
            await _dictDataService.DeleteDictDataByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除字典数据
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:dict:delete", "批量删除字典数据")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteDictDataBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _dictDataService.DeleteDictDataBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新字典数据排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>字典数据DTO</returns>
    [TaktPermission("foundation:dict:update", "更新字典数据排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateDictDataSortAsync([FromBody] TaktDictDataSortDto dto)
    {
        try
        {
            var result = await _dictDataService.UpdateDictDataSortAsync(dto);
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
    [TaktPermission("foundation:dict:import", "获取字典数据导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetDictDataTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _dictDataService.GetDictDataTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入字典数据
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("foundation:dict:import", "导入字典数据")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportDictDataAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _dictDataService.ImportDictDataAsync(stream, sheetName);
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
    /// 导出字典数据
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("foundation:dict:export", "导出字典数据")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportDictDataAsync([FromQuery] TaktDictDataQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _dictDataService.ExportDictDataAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取当前租户下全部字典数据（登录即可访问，供前端全局字典缓存）
    /// </summary>
    /// <returns>扁平字典项列表</returns>
    [HttpGet("all")]
    [AllowAnonymous]
    public async Task<IActionResult> GetDataDictAllAsync()
    {
        try
        {
            var result = await _dictDataService.GetDataDictAllAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
