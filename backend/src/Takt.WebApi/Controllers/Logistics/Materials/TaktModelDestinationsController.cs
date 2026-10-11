// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Materials
// 文件名称：TaktModelDestinationsController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：型号目的地控制器
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
/// 型号目的地控制器
/// 提供型号目的地的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "型号目的地")]
public class TaktModelDestinationsController : TaktControllerBase
{
    private readonly ITaktModelDestinationService _modelDestinationService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="modelDestinationService">型号目的地服务</param>
    public TaktModelDestinationsController(ITaktModelDestinationService modelDestinationService)
    {
        _modelDestinationService = modelDestinationService;
    }

    /// <summary>
    /// 获取型号目的地列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:materials:model:destination:list", "型号目的地列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetModelDestinationListAsync([FromQuery] TaktModelDestinationQueryDto queryDto)
    {
        try
        {
            var result = await _modelDestinationService.GetModelDestinationListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取型号目的地
    /// </summary>
    /// <param name="id">型号目的地ID</param>
    /// <returns>型号目的地DTO</returns>
    [TaktPermission("logistics:materials:model:destination:query", "型号目的地详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetModelDestinationByIdAsync(long id)
    {
        try
        {
            var result = await _modelDestinationService.GetModelDestinationByIdAsync(id);
            if (result == null)
            {
                return NotFound("型号目的地不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取型号目的地选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:materials:model:destination:query", "型号目的地选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetModelDestinationOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _modelDestinationService.GetModelDestinationOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取机种下拉选项（ModelCode 去重）
    /// </summary>
    /// <returns>机种下拉选项</returns>
    [TaktPermission("logistics:materials:model:destination:query", "机种选项")]
    [HttpGet("model-options")]
    public async Task<IActionResult> GetModelOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _modelDestinationService.GetModelOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据机种编码获取级联物料选项
    /// </summary>
    /// <param name="modelCode">机种编码</param>
    /// <returns>物料下拉选项</returns>
    [TaktPermission("logistics:materials:model:destination:query", "按机种查询物料选项")]
    [HttpGet("material-options-by-model")]
    public async Task<IActionResult> GetMaterialOptionsByModelAsync([FromQuery] string modelCode)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(modelCode))
            {
                return Success(new List<TaktSelectOption>(), "查询成功");
            }
            var result = await _modelDestinationService.GetMaterialOptionsByModelAsync(modelCode);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据物料编码获取机种名称与仕向地信息
    /// </summary>
    /// <param name="materialCode">物料编码</param>
    /// <returns>型号目的地 DTO</returns>
    [TaktPermission("logistics:materials:model:destination:query", "按物料查询机种仕向")]
    [HttpGet("by-material")]
    public async Task<IActionResult> GetModelDestinationByMaterialAsync([FromQuery] string materialCode)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(materialCode))
            {
                return BadRequest("物料编码不能为空");
            }
            var result = await _modelDestinationService.GetModelDestinationByMaterialAsync(materialCode);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据物料编码获取机种仕向列表
    /// </summary>
    /// <param name="materialCode">物料编码</param>
    /// <returns>型号目的地 DTO 列表</returns>
    [TaktPermission("logistics:materials:model:destination:query", "按物料查询机种仕向列表")]
    [HttpGet("list-by-material")]
    public async Task<IActionResult> GetModelDestinationListByMaterialAsync([FromQuery] string materialCode)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(materialCode))
            {
                return BadRequest("物料编码不能为空");
            }
            var result = await _modelDestinationService.GetModelDestinationListByMaterialAsync(materialCode);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据物料编码获取机种仕向选项列表
    /// </summary>
    /// <param name="materialCode">物料编码</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:materials:model:destination:query", "按物料查询机种仕向选项")]
    [HttpGet("options-by-material")]
    public async Task<IActionResult> GetModelDestinationOptionsByMaterialAsync([FromQuery] string materialCode)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(materialCode))
            {
                return BadRequest("物料编码不能为空");
            }
            var result = await _modelDestinationService.GetModelDestinationOptionsByMaterialAsync(materialCode);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建型号目的地
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>型号目的地DTO</returns>
    [TaktPermission("logistics:materials:model:destination:create", "创建型号目的地")]
    [HttpPost]
    public async Task<IActionResult> CreateModelDestinationAsync([FromBody] TaktModelDestinationCreateDto dto)
    {
        try
        {
            var result = await _modelDestinationService.CreateModelDestinationAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新型号目的地
    /// </summary>
    /// <param name="id">型号目的地ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>型号目的地DTO</returns>
    [TaktPermission("logistics:materials:model:destination:update", "更新型号目的地")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateModelDestinationAsync(long id, [FromBody] TaktModelDestinationUpdateDto dto)
    {
        try
        {
            var result = await _modelDestinationService.UpdateModelDestinationAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除型号目的地
    /// </summary>
    /// <param name="id">型号目的地ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:model:destination:delete", "删除型号目的地")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteModelDestinationByIdAsync(long id)
    {
        try
        {
            await _modelDestinationService.DeleteModelDestinationByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除型号目的地
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:model:destination:delete", "批量删除型号目的地")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteModelDestinationBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _modelDestinationService.DeleteModelDestinationBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新型号目的地排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>型号目的地DTO</returns>
    [TaktPermission("logistics:materials:model:destination:update", "更新型号目的地排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateModelDestinationSortAsync([FromBody] TaktModelDestinationSortDto dto)
    {
        try
        {
            var result = await _modelDestinationService.UpdateModelDestinationSortAsync(dto);
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
    [TaktPermission("logistics:materials:model:destination:import", "获取型号目的地导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetModelDestinationTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _modelDestinationService.GetModelDestinationTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入型号目的地
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:materials:model:destination:import", "导入型号目的地")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportModelDestinationAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _modelDestinationService.ImportModelDestinationAsync(stream, sheetName);
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
    /// 导出型号目的地
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:materials:model:destination:export", "导出型号目的地")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportModelDestinationAsync([FromQuery] TaktModelDestinationQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _modelDestinationService.ExportModelDestinationAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
