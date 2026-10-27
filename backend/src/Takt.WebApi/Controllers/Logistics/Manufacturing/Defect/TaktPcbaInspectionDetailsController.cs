// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Defect
// 文件名称：TaktPcbaInspectionDetailsController.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA检查明细控制器
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
/// PCBA检查明细控制器
/// 提供PCBA检查明细的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "PCBA检查明细")]
public class TaktPcbaInspectionDetailsController : TaktControllerBase
{
    private readonly ITaktPcbaInspectionDetailService _pcbaInspectionDetailService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="pcbaInspectionDetailService">PCBA检查明细服务</param>
    public TaktPcbaInspectionDetailsController(ITaktPcbaInspectionDetailService pcbaInspectionDetailService)
    {
        _pcbaInspectionDetailService = pcbaInspectionDetailService;
    }

    /// <summary>
    /// 获取PCBA检查明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg2:inspection:list", "PCBA检查明细列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPcbaInspectionDetailListAsync([FromQuery] TaktPcbaInspectionDetailQueryDto queryDto)
    {
        try
        {
            var result = await _pcbaInspectionDetailService.GetPcbaInspectionDetailListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取PCBA检查明细
    /// </summary>
    /// <param name="id">PCBA检查明细ID</param>
    /// <returns>PCBA检查明细DTO</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg2:inspection:query", "PCBA检查明细详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPcbaInspectionDetailByIdAsync(long id)
    {
        try
        {
            var result = await _pcbaInspectionDetailService.GetPcbaInspectionDetailByIdAsync(id);
            if (result == null)
            {
                return NotFound("PCBA检查明细不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取PCBA检查明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg2:inspection:query", "PCBA检查明细选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPcbaInspectionDetailOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _pcbaInspectionDetailService.GetPcbaInspectionDetailOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建PCBA检查明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>PCBA检查明细DTO</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg2:inspection:create", "创建PCBA检查明细")]
    [HttpPost]
    public async Task<IActionResult> CreatePcbaInspectionDetailAsync([FromBody] TaktPcbaInspectionDetailCreateDto dto)
    {
        try
        {
            var result = await _pcbaInspectionDetailService.CreatePcbaInspectionDetailAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新PCBA检查明细
    /// </summary>
    /// <param name="id">PCBA检查明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>PCBA检查明细DTO</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg2:inspection:update", "更新PCBA检查明细")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePcbaInspectionDetailAsync(long id, [FromBody] TaktPcbaInspectionDetailUpdateDto dto)
    {
        try
        {
            var result = await _pcbaInspectionDetailService.UpdatePcbaInspectionDetailAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除PCBA检查明细
    /// </summary>
    /// <param name="id">PCBA检查明细ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg2:inspection:delete", "删除PCBA检查明细")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePcbaInspectionDetailByIdAsync(long id)
    {
        try
        {
            await _pcbaInspectionDetailService.DeletePcbaInspectionDetailByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除PCBA检查明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg2:inspection:delete", "批量删除PCBA检查明细")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePcbaInspectionDetailBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _pcbaInspectionDetailService.DeletePcbaInspectionDetailBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新PCBA检查明细状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>PCBA检查明细DTO</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg2:inspection:update", "更新PCBA检查明细状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdatePcbaInspectionDetailStatusAsync([FromBody] TaktPcbaInspectionDetailStatusDto dto)
    {
        try
        {
            var result = await _pcbaInspectionDetailService.UpdatePcbaInspectionDetailStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新PCBA检查明细作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>PCBA检查明细DTO</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg2:inspection:update", "更新PCBA检查明细作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdatePcbaInspectionDetailObsoleteAsync([FromBody] TaktPcbaInspectionDetailObsoleteDto dto)
    {
        try
        {
            var result = await _pcbaInspectionDetailService.UpdatePcbaInspectionDetailObsoleteAsync(dto);
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
    [TaktPermission("logistics:manufacturing:defect:mfg2:inspection:import", "获取PCBA检查明细导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetPcbaInspectionDetailTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _pcbaInspectionDetailService.GetPcbaInspectionDetailTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入PCBA检查明细
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg2:inspection:import", "导入PCBA检查明细")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportPcbaInspectionDetailAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _pcbaInspectionDetailService.ImportPcbaInspectionDetailAsync(stream, sheetName);
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
    /// 导出PCBA检查明细
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg2:inspection:export", "导出PCBA检查明细")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPcbaInspectionDetailAsync([FromQuery] TaktPcbaInspectionDetailQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _pcbaInspectionDetailService.ExportPcbaInspectionDetailAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
