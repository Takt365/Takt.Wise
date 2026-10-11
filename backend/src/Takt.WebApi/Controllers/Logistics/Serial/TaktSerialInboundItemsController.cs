// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Serial
// 文件名称：TaktSerialInboundItemsController.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：序列号入库明细控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Serial;
using Takt.Application.Services.Logistics.Serial;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Serial;

/// <summary>
/// 序列号入库明细控制器
/// 提供序列号入库明细的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "序列号入库明细")]
public class TaktSerialInboundItemsController : TaktControllerBase
{
    private readonly ITaktSerialInboundItemService _serialInboundItemService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="serialInboundItemService">序列号入库明细服务</param>
    public TaktSerialInboundItemsController(ITaktSerialInboundItemService serialInboundItemService)
    {
        _serialInboundItemService = serialInboundItemService;
    }

    /// <summary>
    /// 获取序列号入库明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:serial:inbound:list", "序列号入库明细列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSerialInboundItemListAsync([FromQuery] TaktSerialInboundItemQueryDto queryDto)
    {
        try
        {
            var result = await _serialInboundItemService.GetSerialInboundItemListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取序列号入库明细
    /// </summary>
    /// <param name="id">序列号入库明细ID</param>
    /// <returns>序列号入库明细DTO</returns>
    [TaktPermission("logistics:serial:inbound:query", "序列号入库明细详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSerialInboundItemByIdAsync(long id)
    {
        try
        {
            var result = await _serialInboundItemService.GetSerialInboundItemByIdAsync(id);
            if (result == null)
            {
                return NotFound("序列号入库明细不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取产品序列号入库明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:serial:inbound:query", "序列号入库明细选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSerialInboundItemOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _serialInboundItemService.GetSerialInboundItemOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建序列号入库明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>序列号入库明细DTO</returns>
    [TaktPermission("logistics:serial:inbound:create", "创建序列号入库明细")]
    [HttpPost]
    public async Task<IActionResult> CreateSerialInboundItemAsync([FromBody] TaktSerialInboundItemCreateDto dto)
    {
        try
        {
            var result = await _serialInboundItemService.CreateSerialInboundItemAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新序列号入库明细
    /// </summary>
    /// <param name="id">序列号入库明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>序列号入库明细DTO</returns>
    [TaktPermission("logistics:serial:inbound:update", "更新序列号入库明细")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSerialInboundItemAsync(long id, [FromBody] TaktSerialInboundItemUpdateDto dto)
    {
        try
        {
            var result = await _serialInboundItemService.UpdateSerialInboundItemAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除序列号入库明细
    /// </summary>
    /// <param name="id">序列号入库明细ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:serial:inbound:delete", "删除序列号入库明细")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSerialInboundItemByIdAsync(long id)
    {
        try
        {
            await _serialInboundItemService.DeleteSerialInboundItemByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除序列号入库明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:serial:inbound:delete", "批量删除序列号入库明细")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSerialInboundItemBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _serialInboundItemService.DeleteSerialInboundItemBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新序列号入库明细作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>序列号入库明细DTO</returns>
    [TaktPermission("logistics:serial:inbound:update", "更新序列号入库明细作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateSerialInboundItemObsoleteAsync([FromBody] TaktSerialInboundItemObsoleteDto dto)
    {
        try
        {
            var result = await _serialInboundItemService.UpdateSerialInboundItemObsoleteAsync(dto);
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
    [TaktPermission("logistics:serial:inbound:import", "获取序列号入库明细导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSerialInboundItemTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _serialInboundItemService.GetSerialInboundItemTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入序列号入库明细
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:serial:inbound:import", "导入序列号入库明细")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSerialInboundItemAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _serialInboundItemService.ImportSerialInboundItemAsync(stream, sheetName);
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
    /// 导出序列号入库明细
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:serial:inbound:export", "导出序列号入库明细")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSerialInboundItemAsync([FromQuery] TaktSerialInboundItemQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _serialInboundItemService.ExportSerialInboundItemAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
