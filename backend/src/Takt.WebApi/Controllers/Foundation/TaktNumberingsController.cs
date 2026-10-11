// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Foundation
// 文件名称：TaktNumberingsController.cs
// 创建时间：2026-06-17
// 创建人：Takt365(Cursor AI)
// 功能描述：编码规则控制器
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
/// 编码规则控制器
/// 提供编码规则的 REST API
/// </summary>
[ApiModule(8, "基础设置")]
[Route("api/[controller]", Name = "编码规则")]
public class TaktNumberingsController : TaktControllerBase
{
    private readonly ITaktNumberingService _numberingService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="numberingService">编码规则服务</param>
    public TaktNumberingsController(ITaktNumberingService numberingService)
    {
        _numberingService = numberingService;
    }

    /// <summary>
    /// 获取编码规则列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("foundation:numbering:list", "编码规则列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetNumberingListAsync([FromQuery] TaktNumberingQueryDto queryDto)
    {
        try
        {
            var result = await _numberingService.GetNumberingListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取编码规则
    /// </summary>
    /// <param name="id">编码规则ID</param>
    /// <returns>编码规则DTO</returns>
    [TaktPermission("foundation:numbering:query", "编码规则详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetNumberingByIdAsync(long id)
    {
        try
        {
            var result = await _numberingService.GetNumberingByIdAsync(id);
            if (result == null)
            {
                return NotFound("编码规则不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 预览下一个业务编码（登录即可；不占用流水号，供表单选类型后展示）
    /// </summary>
    /// <param name="ruleCode">规则编码</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>预览结果</returns>
    [HttpGet("preview-next")]
    public async Task<IActionResult> PreviewNumberingNextAsync(
        [FromQuery] string ruleCode,
        CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(ruleCode))
            {
                return BadRequest("规则编码不能为空");
            }
            var result = await _numberingService.PreviewNumberingNextAsync(ruleCode, cancellationToken);
            return Success(result, "预览成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取编码规则选项列表
    /// </summary>
    /// <param name="documentType">单据类型（菜单名称）；有值时仅返回该类型规则</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("foundation:numbering:query", "编码规则选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetNumberingOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null, [FromQuery] string? documentType = null)
    {
        try
        {
            var result = await _numberingService.GetNumberingOptionsAsync(plantCode, keyword, documentType);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建编码规则
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>编码规则DTO</returns>
    [TaktPermission("foundation:numbering:create", "创建编码规则")]
    [HttpPost]
    public async Task<IActionResult> CreateNumberingAsync([FromBody] TaktNumberingCreateDto dto)
    {
        try
        {
            var result = await _numberingService.CreateNumberingAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新编码规则
    /// </summary>
    /// <param name="id">编码规则ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>编码规则DTO</returns>
    [TaktPermission("foundation:numbering:update", "更新编码规则")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNumberingAsync(long id, [FromBody] TaktNumberingUpdateDto dto)
    {
        try
        {
            var result = await _numberingService.UpdateNumberingAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除编码规则
    /// </summary>
    /// <param name="id">编码规则ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:numbering:delete", "删除编码规则")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNumberingByIdAsync(long id)
    {
        try
        {
            await _numberingService.DeleteNumberingByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除编码规则
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:numbering:delete", "批量删除编码规则")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteNumberingBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _numberingService.DeleteNumberingBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新编码规则状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>编码规则DTO</returns>
    [TaktPermission("foundation:numbering:update", "更新编码规则状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateNumberingStatusAsync([FromBody] TaktNumberingStatusDto dto)
    {
        try
        {
            var result = await _numberingService.UpdateNumberingStatusAsync(dto);
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
    [TaktPermission("foundation:numbering:import", "获取编码规则导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetNumberingTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _numberingService.GetNumberingTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入编码规则
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("foundation:numbering:import", "导入编码规则")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportNumberingAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _numberingService.ImportNumberingAsync(stream, sheetName);
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
    /// 导出编码规则
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("foundation:numbering:export", "导出编码规则")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportNumberingAsync([FromQuery] TaktNumberingQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _numberingService.ExportNumberingAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}