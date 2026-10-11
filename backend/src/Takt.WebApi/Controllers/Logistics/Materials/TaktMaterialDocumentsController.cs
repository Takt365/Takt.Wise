// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Materials
// 文件名称：TaktMaterialDocumentsController.cs
// 创建时间：2026-09-20
// 创建人：Takt365(Cursor AI)
// 功能描述：物料凭证控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Materials;
using Takt.Application.Services.Logistics.Materials;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Materials;

/// <summary>
/// 物料凭证控制器
/// 提供物料凭证的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "物料凭证")]
public class TaktMaterialDocumentsController : TaktControllerBase
{
    private readonly ITaktMaterialDocumentService _materialDocumentService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="materialDocumentService">物料凭证服务</param>
    public TaktMaterialDocumentsController(ITaktMaterialDocumentService materialDocumentService)
    {
        _materialDocumentService = materialDocumentService;
    }

    /// <summary>
    /// 获取物料凭证列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:materials:material:document:list", "物料凭证列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetMaterialDocumentListAsync([FromQuery] TaktMaterialDocumentQueryDto queryDto)
    {
        try
        {
            var result = await _materialDocumentService.GetMaterialDocumentListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取物料凭证
    /// </summary>
    /// <param name="id">物料凭证ID</param>
    /// <returns>物料凭证DTO</returns>
    [TaktPermission("logistics:materials:material:document:query", "物料凭证详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetMaterialDocumentByIdAsync(long id)
    {
        try
        {
            var result = await _materialDocumentService.GetMaterialDocumentByIdAsync(id);
            if (result == null)
            {
                return NotFound("物料凭证不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取物料凭证选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:materials:material:document:query", "物料凭证选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetMaterialDocumentOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _materialDocumentService.GetMaterialDocumentOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建物料凭证
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>物料凭证DTO</returns>
    [TaktPermission("logistics:materials:material:document:create", "创建物料凭证")]
    [HttpPost]
    public async Task<IActionResult> CreateMaterialDocumentAsync([FromBody] TaktMaterialDocumentCreateDto dto)
    {
        try
        {
            var result = await _materialDocumentService.CreateMaterialDocumentAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新物料凭证
    /// </summary>
    /// <param name="id">物料凭证ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>物料凭证DTO</returns>
    [TaktPermission("logistics:materials:material:document:update", "更新物料凭证")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMaterialDocumentAsync(long id, [FromBody] TaktMaterialDocumentUpdateDto dto)
    {
        try
        {
            var result = await _materialDocumentService.UpdateMaterialDocumentAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除物料凭证
    /// </summary>
    /// <param name="id">物料凭证ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:material:document:delete", "删除物料凭证")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMaterialDocumentByIdAsync(long id)
    {
        try
        {
            await _materialDocumentService.DeleteMaterialDocumentByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除物料凭证
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:material:document:delete", "批量删除物料凭证")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteMaterialDocumentBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _materialDocumentService.DeleteMaterialDocumentBatchAsync(ids);
            return Success("删除成功");
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
    [TaktPermission("logistics:materials:material:document:import", "获取物料凭证导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetMaterialDocumentTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _materialDocumentService.GetMaterialDocumentTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入物料凭证
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:materials:material:document:import", "导入物料凭证")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportMaterialDocumentAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _materialDocumentService.ImportMaterialDocumentAsync(stream, sheetName);
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
    /// 导出物料凭证
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:materials:material:document:export", "导出物料凭证")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportMaterialDocumentAsync([FromQuery] TaktMaterialDocumentQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _materialDocumentService.ExportMaterialDocumentAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
