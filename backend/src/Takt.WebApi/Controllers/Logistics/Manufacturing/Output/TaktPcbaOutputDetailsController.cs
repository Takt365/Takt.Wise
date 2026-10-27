// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Output
// 文件名称：TaktPcbaOutputDetailsController.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA日报明细控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Output;
using Takt.Application.Services.Logistics.Manufacturing.Output;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Output;

/// <summary>
/// PCBA日报明细控制器
/// 提供PCBA日报明细的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "PCBA日报明细")]
public class TaktPcbaOutputDetailsController : TaktControllerBase
{
    private readonly ITaktPcbaOutputDetailService _pcbaOutputDetailService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="pcbaOutputDetailService">PCBA日报明细服务</param>
    public TaktPcbaOutputDetailsController(ITaktPcbaOutputDetailService pcbaOutputDetailService)
    {
        _pcbaOutputDetailService = pcbaOutputDetailService;
    }

    /// <summary>
    /// 获取PCBA日报明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:output:mfg2:list", "PCBA日报明细列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPcbaOutputDetailListAsync([FromQuery] TaktPcbaOutputDetailQueryDto queryDto)
    {
        try
        {
            var result = await _pcbaOutputDetailService.GetPcbaOutputDetailListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取PCBA日报明细
    /// </summary>
    /// <param name="id">PCBA日报明细ID</param>
    /// <returns>PCBA日报明细DTO</returns>
    [TaktPermission("logistics:manufacturing:output:mfg2:query", "PCBA日报明细详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPcbaOutputDetailByIdAsync(long id)
    {
        try
        {
            var result = await _pcbaOutputDetailService.GetPcbaOutputDetailByIdAsync(id);
            if (result == null)
            {
                return NotFound("PCBA日报明细不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取PCBA日报明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:output:mfg2:query", "PCBA日报明细选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPcbaOutputDetailOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _pcbaOutputDetailService.GetPcbaOutputDetailOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建PCBA日报明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>PCBA日报明细DTO</returns>
    [TaktPermission("logistics:manufacturing:output:mfg2:create", "创建PCBA日报明细")]
    [HttpPost]
    public async Task<IActionResult> CreatePcbaOutputDetailAsync([FromBody] TaktPcbaOutputDetailCreateDto dto)
    {
        try
        {
            var result = await _pcbaOutputDetailService.CreatePcbaOutputDetailAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新PCBA日报明细
    /// </summary>
    /// <param name="id">PCBA日报明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>PCBA日报明细DTO</returns>
    [TaktPermission("logistics:manufacturing:output:mfg2:update", "更新PCBA日报明细")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePcbaOutputDetailAsync(long id, [FromBody] TaktPcbaOutputDetailUpdateDto dto)
    {
        try
        {
            var result = await _pcbaOutputDetailService.UpdatePcbaOutputDetailAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除PCBA日报明细
    /// </summary>
    /// <param name="id">PCBA日报明细ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:output:mfg2:delete", "删除PCBA日报明细")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePcbaOutputDetailByIdAsync(long id)
    {
        try
        {
            await _pcbaOutputDetailService.DeletePcbaOutputDetailByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除PCBA日报明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:output:mfg2:delete", "批量删除PCBA日报明细")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePcbaOutputDetailBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _pcbaOutputDetailService.DeletePcbaOutputDetailBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新PCBA日报明细状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>PCBA日报明细DTO</returns>
    [TaktPermission("logistics:manufacturing:output:mfg2:update", "更新PCBA日报明细状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdatePcbaOutputDetailStatusAsync([FromBody] TaktPcbaOutputDetailStatusDto dto)
    {
        try
        {
            var result = await _pcbaOutputDetailService.UpdatePcbaOutputDetailStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新PCBA日报明细作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>PCBA日报明细DTO</returns>
    [TaktPermission("logistics:manufacturing:output:mfg2:update", "更新PCBA日报明细作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdatePcbaOutputDetailObsoleteAsync([FromBody] TaktPcbaOutputDetailObsoleteDto dto)
    {
        try
        {
            var result = await _pcbaOutputDetailService.UpdatePcbaOutputDetailObsoleteAsync(dto);
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
    [TaktPermission("logistics:manufacturing:output:mfg2:import", "获取PCBA日报明细导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetPcbaOutputDetailTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _pcbaOutputDetailService.GetPcbaOutputDetailTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入PCBA日报明细
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:output:mfg2:import", "导入PCBA日报明细")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportPcbaOutputDetailAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _pcbaOutputDetailService.ImportPcbaOutputDetailAsync(stream, sheetName);
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
    /// 导出PCBA日报明细
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:output:mfg2:export", "导出PCBA日报明细")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPcbaOutputDetailAsync([FromQuery] TaktPcbaOutputDetailQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _pcbaOutputDetailService.ExportPcbaOutputDetailAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
