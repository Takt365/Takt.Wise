// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Foundation
// 文件名称：TaktAdminDivisionsController.cs
// 创建时间：2026-08-21
// 创建人：Takt365(Cursor AI)
// 功能描述：行政区划控制器
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
/// 行政区划控制器
/// 提供行政区划的 REST API
/// </summary>
[ApiModule(8, "基础设置")]
[Route("api/[controller]", Name = "行政区划")]
public class TaktAdminDivisionsController : TaktControllerBase
{
    private readonly ITaktAdminDivisionService _adminDivisionService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="adminDivisionService">行政区划服务</param>
    public TaktAdminDivisionsController(ITaktAdminDivisionService adminDivisionService)
    {
        _adminDivisionService = adminDivisionService;
    }

    /// <summary>
    /// 获取行政区划列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("foundation:admin:division:list", "行政区划列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetAdminDivisionListAsync([FromQuery] TaktAdminDivisionQueryDto queryDto)
    {
        try
        {
            var result = await _adminDivisionService.GetAdminDivisionListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取行政区划平铺选项（DictValue=DivisionCode）
    /// </summary>
    /// <param name="plantCode">工厂代码（可选；本实体无工厂列，忽略）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <param name="level">层级（可选；1～6）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("foundation:admin:division:query", "行政区划选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetAdminDivisionOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null, [FromQuery] int? level = null)
    {
        try
        {
            var result = await _adminDivisionService.GetAdminDivisionOptionsAsync(plantCode, keyword, level);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取行政区划
    /// </summary>
    /// <param name="id">行政区划ID</param>
    /// <returns>行政区划DTO</returns>
    [TaktPermission("foundation:admin:division:query", "行政区划详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetAdminDivisionByIdAsync(long id)
    {
        try
        {
            var result = await _adminDivisionService.GetAdminDivisionByIdAsync(id);
            if (result == null)
            {
                return NotFound("行政区划不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取行政区划树形选项（懒加载：仅 parentId 直接子级一层；DictValue=Id 字符串，供表单 parentId）
    /// </summary>
    /// <param name="parentId">父级ID（0=根；懒加载仅返回直接子级一层）</param>
    /// <returns>树形选项</returns>
    [TaktPermission("foundation:admin:division:query", "行政区划树形选项")]
    [HttpGet("tree-options")]
    public async Task<IActionResult> GetAdminDivisionTreeOptionsAsync([FromQuery] long parentId = 0, [FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _adminDivisionService.GetAdminDivisionTreeOptionsAsync(parentId, plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取行政区划树形列表（懒加载：仅 parentId 直接子级一层）
    /// </summary>
    /// <param name="parentId">父级ID（0=根；懒加载仅返回直接子级一层）</param>
    /// <param name="includeDisabled">为 false 时过滤禁用项（按实体 *Status 字段）</param>
    /// <returns>树形数据</returns>
    [TaktPermission("foundation:admin:division:query", "行政区划树")]
    [HttpGet("tree")]
    public async Task<IActionResult> GetAdminDivisionTreeAsync([FromQuery] long parentId = 0, [FromQuery] bool includeDisabled = false)
    {
        try
        {
            var result = await _adminDivisionService.GetAdminDivisionTreeAsync(parentId, includeDisabled);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建行政区划
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>行政区划DTO</returns>
    [TaktPermission("foundation:admin:division:create", "创建行政区划")]
    [HttpPost]
    public async Task<IActionResult> CreateAdminDivisionAsync([FromBody] TaktAdminDivisionCreateDto dto)
    {
        try
        {
            var result = await _adminDivisionService.CreateAdminDivisionAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新行政区划
    /// </summary>
    /// <param name="id">行政区划ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>行政区划DTO</returns>
    [TaktPermission("foundation:admin:division:update", "更新行政区划")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAdminDivisionAsync(long id, [FromBody] TaktAdminDivisionUpdateDto dto)
    {
        try
        {
            var result = await _adminDivisionService.UpdateAdminDivisionAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除行政区划
    /// </summary>
    /// <param name="id">行政区划ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:admin:division:delete", "删除行政区划")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAdminDivisionByIdAsync(long id)
    {
        try
        {
            await _adminDivisionService.DeleteAdminDivisionByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除行政区划
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:admin:division:delete", "批量删除行政区划")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteAdminDivisionBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _adminDivisionService.DeleteAdminDivisionBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新行政区划状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>行政区划DTO</returns>
    [TaktPermission("foundation:admin:division:update", "更新行政区划状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateAdminDivisionStatusAsync([FromBody] TaktAdminDivisionStatusDto dto)
    {
        try
        {
            var result = await _adminDivisionService.UpdateAdminDivisionStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新行政区划排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>行政区划DTO</returns>
    [TaktPermission("foundation:admin:division:update", "更新行政区划排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateAdminDivisionSortAsync([FromBody] TaktAdminDivisionSortDto dto)
    {
        try
        {
            var result = await _adminDivisionService.UpdateAdminDivisionSortAsync(dto);
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
    [TaktPermission("foundation:admin:division:import", "获取行政区划导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetAdminDivisionTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _adminDivisionService.GetAdminDivisionTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入行政区划
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("foundation:admin:division:import", "导入行政区划")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportAdminDivisionAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _adminDivisionService.ImportAdminDivisionAsync(stream, sheetName);
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
    /// 导出行政区划
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("foundation:admin:division:export", "导出行政区划")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportAdminDivisionAsync([FromQuery] TaktAdminDivisionQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _adminDivisionService.ExportAdminDivisionAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
