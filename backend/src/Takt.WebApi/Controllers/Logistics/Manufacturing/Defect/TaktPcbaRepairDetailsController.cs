// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Defect
// 文件名称：TaktPcbaRepairDetailsController.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA改修明细控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Defect;
using Takt.Application.Services.Logistics.Manufacturing.Defect;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Defect;

/// <summary>
/// PCBA改修明细控制器
/// 提供PCBA改修明细的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "PCBA改修明细")]
public class TaktPcbaRepairDetailsController : TaktControllerBase
{
    private readonly ITaktPcbaRepairDetailService _pcbaRepairDetailService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="pcbaRepairDetailService">PCBA改修明细服务</param>
    public TaktPcbaRepairDetailsController(ITaktPcbaRepairDetailService pcbaRepairDetailService)
    {
        _pcbaRepairDetailService = pcbaRepairDetailService;
    }

    /// <summary>
    /// 获取PCBA改修明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:defect:pcba:repair:list", "PCBA改修明细列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPcbaRepairDetailListAsync([FromQuery] TaktPcbaRepairDetailQueryDto queryDto)
    {
        try
        {
            var result = await _pcbaRepairDetailService.GetPcbaRepairDetailListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取PCBA改修明细
    /// </summary>
    /// <param name="id">PCBA改修明细ID</param>
    /// <returns>PCBA改修明细DTO</returns>
    [TaktPermission("logistics:manufacturing:defect:pcba:repair:query", "PCBA改修明细详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPcbaRepairDetailByIdAsync(long id)
    {
        try
        {
            var result = await _pcbaRepairDetailService.GetPcbaRepairDetailByIdAsync(id);
            if (result == null)
            {
                return NotFound("PCBA改修明细不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取PCBA改修明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:defect:pcba:repair:query", "PCBA改修明细选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPcbaRepairDetailOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _pcbaRepairDetailService.GetPcbaRepairDetailOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建PCBA改修明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>PCBA改修明细DTO</returns>
    [TaktPermission("logistics:manufacturing:defect:pcba:repair:create", "创建PCBA改修明细")]
    [HttpPost]
    public async Task<IActionResult> CreatePcbaRepairDetailAsync([FromBody] TaktPcbaRepairDetailCreateDto dto)
    {
        try
        {
            var result = await _pcbaRepairDetailService.CreatePcbaRepairDetailAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新PCBA改修明细
    /// </summary>
    /// <param name="id">PCBA改修明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>PCBA改修明细DTO</returns>
    [TaktPermission("logistics:manufacturing:defect:pcba:repair:update", "更新PCBA改修明细")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePcbaRepairDetailAsync(long id, [FromBody] TaktPcbaRepairDetailUpdateDto dto)
    {
        try
        {
            var result = await _pcbaRepairDetailService.UpdatePcbaRepairDetailAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除PCBA改修明细
    /// </summary>
    /// <param name="id">PCBA改修明细ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:defect:pcba:repair:delete", "删除PCBA改修明细")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePcbaRepairDetailByIdAsync(long id)
    {
        try
        {
            await _pcbaRepairDetailService.DeletePcbaRepairDetailByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除PCBA改修明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:defect:pcba:repair:delete", "批量删除PCBA改修明细")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePcbaRepairDetailBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _pcbaRepairDetailService.DeletePcbaRepairDetailBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新PCBA改修明细作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>PCBA改修明细DTO</returns>
    [TaktPermission("logistics:manufacturing:defect:pcba:repair:update", "更新PCBA改修明细作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdatePcbaRepairDetailObsoleteAsync([FromBody] TaktPcbaRepairDetailObsoleteDto dto)
    {
        try
        {
            var result = await _pcbaRepairDetailService.UpdatePcbaRepairDetailObsoleteAsync(dto);
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
    [TaktPermission("logistics:manufacturing:defect:pcba:repair:import", "获取PCBA改修明细导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetPcbaRepairDetailTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _pcbaRepairDetailService.GetPcbaRepairDetailTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入PCBA改修明细
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:defect:pcba:repair:import", "导入PCBA改修明细")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportPcbaRepairDetailAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _pcbaRepairDetailService.ImportPcbaRepairDetailAsync(stream, sheetName);
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
    /// 导出PCBA改修明细
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:defect:pcba:repair:export", "导出PCBA改修明细")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPcbaRepairDetailAsync([FromQuery] TaktPcbaRepairDetailQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _pcbaRepairDetailService.ExportPcbaRepairDetailAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
