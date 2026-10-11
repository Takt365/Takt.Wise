// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Routine.HelpDesk
// 文件名称：TaktKnowledgesController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：知识库控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Routine.HelpDesk;
using Takt.Application.Services.Routine.HelpDesk;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Routine.HelpDesk;

/// <summary>
/// 知识库控制器
/// 提供知识库的 REST API
/// </summary>
[ApiModule(2, "日常事务")]
[Route("api/[controller]", Name = "知识库")]
public class TaktKnowledgesController : TaktControllerBase
{
    private readonly ITaktKnowledgeService _knowledgeService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="knowledgeService">知识库服务</param>
    public TaktKnowledgesController(ITaktKnowledgeService knowledgeService)
    {
        _knowledgeService = knowledgeService;
    }

    /// <summary>
    /// 获取知识库列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("routine:help:desk:knowledge:list", "知识库列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetKnowledgeListAsync([FromQuery] TaktKnowledgeQueryDto queryDto)
    {
        try
        {
            var result = await _knowledgeService.GetKnowledgeListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取知识库
    /// </summary>
    /// <param name="id">知识库ID</param>
    /// <returns>知识库DTO</returns>
    [TaktPermission("routine:help:desk:knowledge:query", "知识库详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetKnowledgeByIdAsync(long id)
    {
        try
        {
            var result = await _knowledgeService.GetKnowledgeByIdAsync(id);
            if (result == null)
            {
                return NotFound("知识库不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取知识库选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("routine:help:desk:knowledge:query", "知识库选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetKnowledgeOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _knowledgeService.GetKnowledgeOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建知识库
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>知识库DTO</returns>
    [TaktPermission("routine:help:desk:knowledge:create", "创建知识库")]
    [HttpPost]
    public async Task<IActionResult> CreateKnowledgeAsync([FromBody] TaktKnowledgeCreateDto dto)
    {
        try
        {
            var result = await _knowledgeService.CreateKnowledgeAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新知识库
    /// </summary>
    /// <param name="id">知识库ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>知识库DTO</returns>
    [TaktPermission("routine:help:desk:knowledge:update", "更新知识库")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateKnowledgeAsync(long id, [FromBody] TaktKnowledgeUpdateDto dto)
    {
        try
        {
            var result = await _knowledgeService.UpdateKnowledgeAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除知识库
    /// </summary>
    /// <param name="id">知识库ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:help:desk:knowledge:delete", "删除知识库")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteKnowledgeByIdAsync(long id)
    {
        try
        {
            await _knowledgeService.DeleteKnowledgeByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除知识库
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:help:desk:knowledge:delete", "批量删除知识库")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteKnowledgeBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _knowledgeService.DeleteKnowledgeBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新知识库状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>知识库DTO</returns>
    [TaktPermission("routine:help:desk:knowledge:update", "更新知识库状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateKnowledgeStatusAsync([FromBody] TaktKnowledgeStatusDto dto)
    {
        try
        {
            var result = await _knowledgeService.UpdateKnowledgeStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新知识库排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>知识库DTO</returns>
    [TaktPermission("routine:help:desk:knowledge:update", "更新知识库排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateKnowledgeSortAsync([FromBody] TaktKnowledgeSortDto dto)
    {
        try
        {
            var result = await _knowledgeService.UpdateKnowledgeSortAsync(dto);
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
    [TaktPermission("routine:help:desk:knowledge:import", "获取知识库导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetKnowledgeTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _knowledgeService.GetKnowledgeTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入知识库
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("routine:help:desk:knowledge:import", "导入知识库")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportKnowledgeAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _knowledgeService.ImportKnowledgeAsync(stream, sheetName);
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
    /// 导出知识库
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("routine:help:desk:knowledge:export", "导出知识库")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportKnowledgeAsync([FromQuery] TaktKnowledgeQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _knowledgeService.ExportKnowledgeAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
