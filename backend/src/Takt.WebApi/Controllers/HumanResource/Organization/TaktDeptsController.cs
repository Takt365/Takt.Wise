// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.HumanResource.Organization
// 文件名称：TaktDeptsController.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：部门控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.HumanResource.Organization;
using Takt.Application.Services.HumanResource.Organization;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.HumanResource.Organization;

/// <summary>
/// 部门控制器
/// 提供部门的 REST API
/// </summary>
[ApiModule(5, "组织管理")]
[Route("api/[controller]", Name = "部门")]
public class TaktDeptsController : TaktControllerBase
{
    private readonly ITaktDeptService _deptService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="deptService">部门服务</param>
    public TaktDeptsController(ITaktDeptService deptService)
    {
        _deptService = deptService;
    }

    /// <summary>
    /// 获取部门列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("human:resource:organization:dept:list", "部门列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetDeptListAsync([FromQuery] TaktDeptQueryDto queryDto)
    {
        try
        {
            var result = await _deptService.GetDeptListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取部门
    /// </summary>
    /// <param name="id">部门ID</param>
    /// <returns>部门DTO</returns>
    [TaktPermission("human:resource:organization:dept:query", "部门详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDeptByIdAsync(long id)
    {
        try
        {
            var result = await _deptService.GetDeptByIdAsync(id);
            if (result == null)
            {
                return NotFound("部门不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取部门树形选项列表（懒加载：仅 parentId 直接子级一层）
    /// </summary>
    /// <param name="parentId">父级ID（0=根；懒加载仅返回直接子级一层）</param>
    /// <returns>树形选项</returns>
    [TaktPermission("human:resource:organization:dept:query", "部门树形选项")]
    [HttpGet("tree-options")]
    public async Task<IActionResult> GetDeptTreeOptionsAsync([FromQuery] long parentId = 0, [FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _deptService.GetDeptTreeOptionsAsync(parentId, plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取部门选项（DictValue=DeptCode，DictLabel=DeptName1）
    /// </summary>
    /// <param name="plantCode">工厂代码（可选）</param>
    /// <param name="keyword">搜索关键字（可选）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("human:resource:organization:dept:query", "部门选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetDeptOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _deptService.GetDeptOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取部门 ISO 编码树形选项列表（懒加载：DictValue=IsoCode；ExtValue=实体 Id；当前登录公司隔离）
    /// </summary>
    /// <param name="parentId">父级ID（0=根；懒加载仅返回直接子级一层）</param>
    /// <param name="plantCode">工厂代码（可选）</param>
    /// <param name="keyword">搜索关键字（可选）</param>
    /// <returns>树形选项</returns>
    [TaktPermission("human:resource:organization:dept:query", "部门ISO树形选项")]
    [HttpGet("iso-tree-options")]
    public async Task<IActionResult> GetDeptIsoTreeOptionsAsync([FromQuery] long parentId = 0, [FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _deptService.GetDeptIsoTreeOptionsAsync(parentId, plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取部门树形列表（懒加载：仅 parentId 直接子级一层）
    /// </summary>
    /// <param name="parentId">父级ID（0=根；懒加载仅返回直接子级一层）</param>
    /// <param name="includeDisabled">为 false 时过滤禁用项（按实体 *Status 字段）</param>
    /// <returns>树形数据</returns>
    [TaktPermission("human:resource:organization:dept:query", "部门树")]
    [HttpGet("tree")]
    public async Task<IActionResult> GetDeptTreeAsync([FromQuery] long parentId = 0, [FromQuery] bool includeDisabled = false)
    {
        try
        {
            var result = await _deptService.GetDeptTreeAsync(parentId, includeDisabled);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建部门
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>部门DTO</returns>
    [TaktPermission("human:resource:organization:dept:create", "创建部门")]
    [HttpPost]
    public async Task<IActionResult> CreateDeptAsync([FromBody] TaktDeptCreateDto dto)
    {
        try
        {
            var result = await _deptService.CreateDeptAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新部门
    /// </summary>
    /// <param name="id">部门ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>部门DTO</returns>
    [TaktPermission("human:resource:organization:dept:update", "更新部门")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDeptAsync(long id, [FromBody] TaktDeptUpdateDto dto)
    {
        try
        {
            var result = await _deptService.UpdateDeptAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除部门
    /// </summary>
    /// <param name="id">部门ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:organization:dept:delete", "删除部门")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDeptByIdAsync(long id)
    {
        try
        {
            await _deptService.DeleteDeptByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除部门
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:organization:dept:delete", "批量删除部门")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteDeptBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _deptService.DeleteDeptBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新部门状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>部门DTO</returns>
    [TaktPermission("human:resource:organization:dept:update", "更新部门状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateDeptStatusAsync([FromBody] TaktDeptStatusDto dto)
    {
        try
        {
            var result = await _deptService.UpdateDeptStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新部门排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>部门DTO</returns>
    [TaktPermission("human:resource:organization:dept:update", "更新部门排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateDeptSortAsync([FromBody] TaktDeptSortDto dto)
    {
        try
        {
            var result = await _deptService.UpdateDeptSortAsync(dto);
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
    [TaktPermission("human:resource:organization:dept:import", "获取部门导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetDeptTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _deptService.GetDeptTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入部门
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("human:resource:organization:dept:import", "导入部门")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportDeptAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _deptService.ImportDeptAsync(stream, sheetName);
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
    /// 导出部门
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:organization:dept:export", "导出部门")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportDeptAsync([FromQuery] TaktDeptQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _deptService.ExportDeptAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
