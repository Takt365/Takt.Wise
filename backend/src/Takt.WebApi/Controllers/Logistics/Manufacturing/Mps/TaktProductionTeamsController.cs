// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Mps
// 文件名称：TaktProductionTeamsController.cs
// 创建时间：2026-07-24
// 创建人：Takt365(Cursor AI)
// 功能描述：生产班组控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Mps;
using Takt.Application.Services.Logistics.Manufacturing.Mps;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Mps;

/// <summary>
/// 生产班组控制器
/// 提供生产班组的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "生产班组")]
public class TaktProductionTeamsController : TaktControllerBase
{
    private readonly ITaktProductionTeamService _productionTeamService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="productionTeamService">生产班组服务</param>
    public TaktProductionTeamsController(ITaktProductionTeamService productionTeamService)
    {
        _productionTeamService = productionTeamService;
    }

    /// <summary>
    /// 获取生产班组列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:mps:production:team:list", "生产班组列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetProductionTeamListAsync([FromQuery] TaktProductionTeamQueryDto queryDto)
    {
        try
        {
            var result = await _productionTeamService.GetProductionTeamListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取生产班组
    /// </summary>
    /// <param name="id">生产班组ID</param>
    /// <returns>生产班组DTO</returns>
    [TaktPermission("logistics:manufacturing:mps:production:team:query", "生产班组详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductionTeamByIdAsync(long id)
    {
        try
        {
            var result = await _productionTeamService.GetProductionTeamByIdAsync(id);
            if (result == null)
            {
                return NotFound("生产班组不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取生产班组选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="teamCategory">班组分类（字典 logistics_manufacturing_team_category；有值时精确匹配，如 A=组立 P=PCBA）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:mps:production:team:query", "生产班组选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetProductionTeamOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null, [FromQuery] string? teamCategory = null)
    {
        try
        {
            var result = await _productionTeamService.GetProductionTeamOptionsAsync(plantCode, keyword, teamCategory);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建生产班组
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>生产班组DTO</returns>
    [TaktPermission("logistics:manufacturing:mps:production:team:create", "创建生产班组")]
    [HttpPost]
    public async Task<IActionResult> CreateProductionTeamAsync([FromBody] TaktProductionTeamCreateDto dto)
    {
        try
        {
            var result = await _productionTeamService.CreateProductionTeamAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新生产班组
    /// </summary>
    /// <param name="id">生产班组ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>生产班组DTO</returns>
    [TaktPermission("logistics:manufacturing:mps:production:team:update", "更新生产班组")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProductionTeamAsync(long id, [FromBody] TaktProductionTeamUpdateDto dto)
    {
        try
        {
            var result = await _productionTeamService.UpdateProductionTeamAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除生产班组
    /// </summary>
    /// <param name="id">生产班组ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:mps:production:team:delete", "删除生产班组")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProductionTeamByIdAsync(long id)
    {
        try
        {
            await _productionTeamService.DeleteProductionTeamByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除生产班组
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:mps:production:team:delete", "批量删除生产班组")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteProductionTeamBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _productionTeamService.DeleteProductionTeamBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新生产班组状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>生产班组DTO</returns>
    [TaktPermission("logistics:manufacturing:mps:production:team:update", "更新生产班组状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateProductionTeamStatusAsync([FromBody] TaktProductionTeamStatusDto dto)
    {
        try
        {
            var result = await _productionTeamService.UpdateProductionTeamStatusAsync(dto);
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
    [TaktPermission("logistics:manufacturing:mps:production:team:import", "获取生产班组导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetProductionTeamTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _productionTeamService.GetProductionTeamTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入生产班组
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:mps:production:team:import", "导入生产班组")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportProductionTeamAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _productionTeamService.ImportProductionTeamAsync(stream, sheetName);
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
    /// 导出生产班组
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:mps:production:team:export", "导出生产班组")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportProductionTeamAsync([FromQuery] TaktProductionTeamQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _productionTeamService.ExportProductionTeamAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
