// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Accounting.Financial
// 文件名称：TaktCountersignDetailsController.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：会签单明细控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Accounting.Financial;
using Takt.Application.Services.Accounting.Financial;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Accounting.Financial;

/// <summary>
/// 会签单明细控制器
/// 提供会签单明细的 REST API
/// </summary>
[ApiModule(3, "财务核算")]
[Route("api/[controller]", Name = "会签单明细")]
public class TaktCountersignDetailsController : TaktControllerBase
{
    private readonly ITaktCountersignDetailService _countersignDetailService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="countersignDetailService">会签单明细服务</param>
    public TaktCountersignDetailsController(ITaktCountersignDetailService countersignDetailService)
    {
        _countersignDetailService = countersignDetailService;
    }

    /// <summary>
    /// 获取会签单明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("accounting:financial:countersign:list", "会签单明细列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetCountersignDetailListAsync([FromQuery] TaktCountersignDetailQueryDto queryDto)
    {
        try
        {
            var result = await _countersignDetailService.GetCountersignDetailListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取会签单明细
    /// </summary>
    /// <param name="id">会签单明细ID</param>
    /// <returns>会签单明细DTO</returns>
    [TaktPermission("accounting:financial:countersign:query", "会签单明细详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCountersignDetailByIdAsync(long id)
    {
        try
        {
            var result = await _countersignDetailService.GetCountersignDetailByIdAsync(id);
            if (result == null)
            {
                return NotFound("会签单明细不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取会签单明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("accounting:financial:countersign:query", "会签单明细选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetCountersignDetailOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _countersignDetailService.GetCountersignDetailOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建会签单明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>会签单明细DTO</returns>
    [TaktPermission("accounting:financial:countersign:create", "创建会签单明细")]
    [HttpPost]
    public async Task<IActionResult> CreateCountersignDetailAsync([FromBody] TaktCountersignDetailCreateDto dto)
    {
        try
        {
            var result = await _countersignDetailService.CreateCountersignDetailAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新会签单明细
    /// </summary>
    /// <param name="id">会签单明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>会签单明细DTO</returns>
    [TaktPermission("accounting:financial:countersign:update", "更新会签单明细")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCountersignDetailAsync(long id, [FromBody] TaktCountersignDetailUpdateDto dto)
    {
        try
        {
            var result = await _countersignDetailService.UpdateCountersignDetailAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除会签单明细
    /// </summary>
    /// <param name="id">会签单明细ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:financial:countersign:delete", "删除会签单明细")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCountersignDetailByIdAsync(long id)
    {
        try
        {
            await _countersignDetailService.DeleteCountersignDetailByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除会签单明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:financial:countersign:delete", "批量删除会签单明细")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteCountersignDetailBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _countersignDetailService.DeleteCountersignDetailBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新会签单明细作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>会签单明细DTO</returns>
    [TaktPermission("accounting:financial:countersign:update", "更新会签单明细作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateCountersignDetailObsoleteAsync([FromBody] TaktCountersignDetailObsoleteDto dto)
    {
        try
        {
            var result = await _countersignDetailService.UpdateCountersignDetailObsoleteAsync(dto);
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
    [TaktPermission("accounting:financial:countersign:import", "获取会签单明细导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetCountersignDetailTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _countersignDetailService.GetCountersignDetailTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入会签单明细
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("accounting:financial:countersign:import", "导入会签单明细")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportCountersignDetailAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _countersignDetailService.ImportCountersignDetailAsync(stream, sheetName);
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
    /// 导出会签单明细
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("accounting:financial:countersign:export", "导出会签单明细")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportCountersignDetailAsync([FromQuery] TaktCountersignDetailQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _countersignDetailService.ExportCountersignDetailAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
