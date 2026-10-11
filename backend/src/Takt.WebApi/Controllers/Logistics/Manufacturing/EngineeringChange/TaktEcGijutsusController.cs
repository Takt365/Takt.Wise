// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcGijutsusController.cs
// 创建时间：2026-06-30
// 创建人：Takt365(Cursor AI)
// 功能描述：设变技术课主控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Application.Services.Foundation;
using Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变技术课主控制器
/// 提供设变技术课主的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "设变技术课主")]
public class TaktEcGijutsusController : TaktControllerBase
{
    private readonly ITaktEcGijutsuService _ecEngService;
    private readonly ITaktEcGijutsuPersistBackgroundService _persistBackgroundService;
    private readonly ITaktMessageService _messageService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecEngService">设变技术课主服务</param>
    /// <param name="persistBackgroundService">新增/更新后台调度</param>
    /// <param name="messageService">在线消息</param>
    public TaktEcGijutsusController(
        ITaktEcGijutsuService ecEngService,
        ITaktEcGijutsuPersistBackgroundService persistBackgroundService,
        ITaktMessageService messageService)
    {
        _ecEngService = ecEngService;
        _persistBackgroundService = persistBackgroundService;
        _messageService = messageService;
    }

    /// <summary>
    /// 获取设变技术课主列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:gijutsu:list", "设变技术课主列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEcGijutsuListAsync([FromQuery] TaktEcGijutsuQueryDto queryDto)
    {
        try
        {
            var result = await _ecEngService.GetEcGijutsuListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取设变技术课主表统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>设变统计</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:gijutsu:list", "设变技术课统计")]
    [HttpGet("stat")]
    public async Task<IActionResult> GetEcGijutsuStatAsync([FromQuery] TaktEcGijutsuStatQueryDto queryDto)
    {
        try
        {
            var result = await _ecEngService.GetEcGijutsuStatAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取尚未导入的来源设变列表（分页）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:gijutsu:list", "未导入来源设变列表")]
    [HttpGet("source-ec/unimported-list")]
    public async Task<IActionResult> GetUnimportedSourceEcGijutsuListAsync([FromQuery] TaktEcGijutsuSourceEcInputQueryDto queryDto)
    {
        try
        {
            var result = await _ecEngService.GetUnimportedSourceEcGijutsuListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取当前公司对应的来源设变目标工厂代码
    /// </summary>
    /// <returns>公司代码与映射工厂代码</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:gijutsu:query", "来源设变工厂映射")]
    [HttpGet("source-ec/plant-code")]
    public async Task<IActionResult> GetEcGijutsuSourcePlantCodeAsync()
    {
        try
        {
            var result = await _ecEngService.GetEcGijutsuSourcePlantCodeAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 从来源设变构建创建草稿（不落库，供 ec-form 补全设变担当/实施范围/附件后 create）
    /// </summary>
    /// <param name="dto">草稿请求 DTO</param>
    /// <returns>创建 DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:gijutsu:query", "来源设变创建草稿")]
    [HttpPost("source-ec/draft")]
    public async Task<IActionResult> GetEcGijutsuDraftFromSourceEcAsync([FromBody] TaktEcGijutsuDraftFromSourceDto dto)
    {
        try
        {
            var result = await _ecEngService.GetEcGijutsuDraftFromSourceEcAsync(dto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 从来源设变导入设变技术课主及明细
    /// </summary>
    /// <param name="dto">导入 DTO</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:gijutsu:create", "从来源设变导入")]
    [HttpPost("import-from-source-ec")]
    public async Task<IActionResult> ImportEcGijutsuFromSourceAsync([FromBody] TaktEcGijutsuImportFromSourceDto dto)
    {
        try
        {
            var result = await _ecEngService.ImportEcGijutsuFromSourceAsync(dto);
            return Success(result, "导入完成");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取设变技术课主
    /// </summary>
    /// <param name="id">设变技术课主ID</param>
    /// <returns>设变技术课主DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:gijutsu:query", "设变技术课主详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEcGijutsuByIdAsync(long id)
    {
        try
        {
            var result = await _ecEngService.GetEcGijutsuByIdAsync(id);
            if (result == null)
            {
                return NotFound("设变技术课主不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取设变技术课主表选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:gijutsu:query", "设变技术课主选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetEcGijutsuOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _ecEngService.GetEcGijutsuOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建设变技术课主（后台落库并派生各部门执行行；完成后 SignalR 通知）
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>已提交回执</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:gijutsu:create", "创建设变技术课主")]
    [HttpPost]
    [RequestSizeLimit(1_073_741_824)]
    [RequestFormLimits(MultipartBodyLengthLimit = 1_073_741_824)]
    public async Task<IActionResult> CreateEcGijutsuAsync([FromBody] TaktEcGijutsuCreateDto dto)
    {
        try
        {
            var result = await _persistBackgroundService.EnqueueCreateAsync(dto);
            TaktLogger.Information(
                "[EcGijutsuPersist] API 已接受后台新增 EcCode={EcCode} DetailCount={DetailCount}",
                result.EcCode,
                result.DetailCount);
            await TaktEcGijutsuPersistMessageHelper.TryNotifyAsync(
                _messageService,
                TaktEcGijutsuPersistMessageHelper.BuildJobSubmitted(result.EcCode, isUpdate: false, result.DetailCount));
            return Success(result, "已提交后台新增，完成后将通知您");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新设变技术课主（后台落库并同步各部门执行行；完成后 SignalR 通知）
    /// </summary>
    /// <param name="id">设变技术课主ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>已提交回执</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:gijutsu:update", "更新设变技术课主")]
    [HttpPut("{id}")]
    [RequestSizeLimit(1_073_741_824)]
    [RequestFormLimits(MultipartBodyLengthLimit = 1_073_741_824)]
    public async Task<IActionResult> UpdateEcGijutsuAsync(long id, [FromBody] TaktEcGijutsuUpdateDto dto)
    {
        try
        {
            var result = await _persistBackgroundService.EnqueueUpdateAsync(id, dto);
            TaktLogger.Information(
                "[EcGijutsuPersist] API 已接受后台更新 EcCode={EcCode} DetailCount={DetailCount}",
                result.EcCode,
                result.DetailCount);
            await TaktEcGijutsuPersistMessageHelper.TryNotifyAsync(
                _messageService,
                TaktEcGijutsuPersistMessageHelper.BuildJobSubmitted(result.EcCode, isUpdate: true, result.DetailCount));
            return Success(result, "已提交后台更新，完成后将通知您");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除设变技术课主
    /// </summary>
    /// <param name="id">设变技术课主ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:gijutsu:delete", "删除设变技术课主")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEcGijutsuByIdAsync(long id)
    {
        try
        {
            await _ecEngService.DeleteEcGijutsuByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除设变技术课主
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:gijutsu:delete", "批量删除设变技术课主")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteEcGijutsuBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _ecEngService.DeleteEcGijutsuBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新设变技术课主状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>设变技术课主DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:gijutsu:update", "更新设变技术课主状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateEcGijutsuStatusAsync([FromBody] TaktEcGijutsuStatusDto dto)
    {
        try
        {
            var result = await _ecEngService.UpdateEcGijutsuStatusAsync(dto);
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
    [TaktPermission("logistics:manufacturing:engineering:change:gijutsu:import", "获取设变技术课主导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetEcGijutsuTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _ecEngService.GetEcGijutsuTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入设变技术课主
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:gijutsu:import", "导入设变技术课主")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportEcGijutsuAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _ecEngService.ImportEcGijutsuAsync(stream, sheetName);
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
    /// 导出设变技术课主
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:gijutsu:export", "导出设变技术课主")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEcGijutsuAsync([FromQuery] TaktEcGijutsuQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _ecEngService.ExportEcGijutsuAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
