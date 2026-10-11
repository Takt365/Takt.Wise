// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Materials
// 文件名称：TaktMaterialDocumentItemsController.cs
// 创建时间：2026-09-20
// 创建人：Takt365(Cursor AI)
// 功能描述：物料凭证行项目控制器
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
/// 物料凭证行项目控制器
/// 提供物料凭证行项目的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "物料凭证行项目")]
public class TaktMaterialDocumentItemsController : TaktControllerBase
{
    private readonly ITaktMaterialDocumentItemService _materialDocumentItemService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="materialDocumentItemService">物料凭证行项目服务</param>
    public TaktMaterialDocumentItemsController(ITaktMaterialDocumentItemService materialDocumentItemService)
    {
        _materialDocumentItemService = materialDocumentItemService;
    }

    /// <summary>
    /// 获取物料凭证行项目列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:materials:material:document:list", "物料凭证行项目列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetMaterialDocumentItemListAsync([FromQuery] TaktMaterialDocumentItemQueryDto queryDto)
    {
        try
        {
            var result = await _materialDocumentItemService.GetMaterialDocumentItemListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取物料凭证行项目
    /// </summary>
    /// <param name="id">物料凭证行项目ID</param>
    /// <returns>物料凭证行项目DTO</returns>
    [TaktPermission("logistics:materials:material:document:query", "物料凭证行项目详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetMaterialDocumentItemByIdAsync(long id)
    {
        try
        {
            var result = await _materialDocumentItemService.GetMaterialDocumentItemByIdAsync(id);
            if (result == null)
            {
                return NotFound("物料凭证行项目不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取物料凭证明细选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:materials:material:document:query", "物料凭证行项目选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetMaterialDocumentItemOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _materialDocumentItemService.GetMaterialDocumentItemOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建物料凭证行项目
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>物料凭证行项目DTO</returns>
    [TaktPermission("logistics:materials:material:document:create", "创建物料凭证行项目")]
    [HttpPost]
    public async Task<IActionResult> CreateMaterialDocumentItemAsync([FromBody] TaktMaterialDocumentItemCreateDto dto)
    {
        try
        {
            var result = await _materialDocumentItemService.CreateMaterialDocumentItemAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新物料凭证行项目
    /// </summary>
    /// <param name="id">物料凭证行项目ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>物料凭证行项目DTO</returns>
    [TaktPermission("logistics:materials:material:document:update", "更新物料凭证行项目")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMaterialDocumentItemAsync(long id, [FromBody] TaktMaterialDocumentItemUpdateDto dto)
    {
        try
        {
            var result = await _materialDocumentItemService.UpdateMaterialDocumentItemAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除物料凭证行项目
    /// </summary>
    /// <param name="id">物料凭证行项目ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:material:document:delete", "删除物料凭证行项目")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMaterialDocumentItemByIdAsync(long id)
    {
        try
        {
            await _materialDocumentItemService.DeleteMaterialDocumentItemByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除物料凭证行项目
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:material:document:delete", "批量删除物料凭证行项目")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteMaterialDocumentItemBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _materialDocumentItemService.DeleteMaterialDocumentItemBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新物料凭证行项目作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>物料凭证行项目DTO</returns>
    [TaktPermission("logistics:materials:material:document:update", "更新物料凭证行项目作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateMaterialDocumentItemObsoleteAsync([FromBody] TaktMaterialDocumentItemObsoleteDto dto)
    {
        try
        {
            var result = await _materialDocumentItemService.UpdateMaterialDocumentItemObsoleteAsync(dto);
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
    [TaktPermission("logistics:materials:material:document:import", "获取物料凭证行项目导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetMaterialDocumentItemTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _materialDocumentItemService.GetMaterialDocumentItemTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入物料凭证行项目
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:materials:material:document:import", "导入物料凭证行项目")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportMaterialDocumentItemAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _materialDocumentItemService.ImportMaterialDocumentItemAsync(stream, sheetName);
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
    /// 导出物料凭证行项目
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:materials:material:document:export", "导出物料凭证行项目")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportMaterialDocumentItemAsync([FromQuery] TaktMaterialDocumentItemQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _materialDocumentItemService.ExportMaterialDocumentItemAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
