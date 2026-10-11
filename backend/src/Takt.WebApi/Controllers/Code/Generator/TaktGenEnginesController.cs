// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Code.Generator
// 文件名称：TaktGenEnginesController.cs
// 创建时间：2026-06-02
// 创建人：Takt365(Cursor AI)
// 功能描述：代码生成工作流控制器，提供代码生成的完整工作流 API（选表、导入、生成、预览）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Code.Generator;
using Takt.Application.Services.Code.Generator.GenEngine;
using Takt.Domain.Interfaces;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Code.Generator;

/// <summary>
/// 代码生成工作流控制器
/// 提供代码生成的完整工作流：数据库选表 → 导入配置 → 代码生成/预览
/// </summary>
[ApiModule(7, "代码管理")]
[Route("api/[controller]", Name = "代码生成工作流")]
public class TaktGenEnginesController : TaktControllerBase
{
    private readonly ITaktGenWorkflowService _workflowService;

    /// <summary>
    /// 构造函数
    /// </summary>
    public TaktGenEnginesController(
        ITaktGenWorkflowService workflowService,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _workflowService = workflowService;
    }

    #region 数据库表管理（有表导入流程）

    /// <summary>
    /// 从数据库导入表结构到代码生成配置（有表导入）
    /// </summary>
    /// <param name="dto">导入请求（租户编码、表名、可选表配置覆盖）</param>
    /// <returns>导入后的表配置信息</returns>
    [HttpPost("database/import")]
    [TaktPermission("code:generator:import", "导入数据库表")]
    public async Task<IActionResult> ImportTableFromDatabaseAsync([FromBody] TaktImportTableFromDatabaseRequestDto dto)
    {
        try
        {
            var result = await _workflowService.ImportTableFromDatabaseAsync(
                dto.TenantCode,
                dto.TableName,
                dto.TableOverrides);
            return Success(result, "导入成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 从数据库同步指定表的列元数据到代码生成配置（增量更新，保留用户生成配置）
    /// </summary>
    /// <param name="tableId">代码生成表配置 ID</param>
    /// <returns>同步后的表配置信息（含列列表）</returns>
    [HttpPost("database/sync/{tableId}")]
    [TaktPermission("code:generator:sync", "同步数据库表列")]
    public async Task<IActionResult> SyncTableColumnsFromDatabaseAsync(long tableId)
    {
        try
        {
            var result = await _workflowService.SyncTableColumnsFromDatabaseAsync(tableId);
            return Success(result, "同步成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    #endregion

    #region 实体表初始化（无表导入流程）

    /// <summary>
    /// 获取可用于「按实体初始化表」的实体类型全名列表
    /// </summary>
    /// <returns>实体类型全名列表</returns>
    [HttpGet("entities")]
    [TaktPermission("code:generator:query", "查询可用实体类型")]
    public async Task<IActionResult> GetAvailableEntityTypesAsync()
    {
        var entities = await _workflowService.GetAvailableEntityTypeFullNamesAsync();
        return Success(entities, "查询成功");
    }

    /// <summary>
    /// 根据实体类型初始化数据表（无表流程：代码生成后手动建表）
    /// </summary>
    /// <param name="dto">初始化请求（租户编码、实体类型全名）</param>
    /// <returns>操作结果</returns>
    [HttpPost("entities/initialize")]
    [TaktPermission("code:generator:initialize", "初始化实体表")]
    public async Task<IActionResult> InitializeTableFromEntityAsync([FromBody] TaktInitializeTableFromEntityRequestDto dto)
    {
        try
        {
            await _workflowService.InitializeTableFromEntityTypeAsync(dto.TenantCode, dto.EntityTypeFullName);
            return Success("数据表初始化成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    #endregion

    #region 代码生成

    /// <summary>
    /// 根据表配置和模板生成代码（GenMethod=0 返回 zip；1/2 写入磁盘）
    /// </summary>
    [HttpPost("generate/{tableId}")]
    [TaktPermission("code:generator:generate", "生成代码")]
    public async Task<IActionResult> GenerateCodeAsync(long tableId, [FromBody] TaktGenerateCodeRequestDto dto)
    {
        try
        {
            var sqlCreateBy = CurrentUserName ?? User?.Identity?.Name ?? "admin";
            var result = await _workflowService.GenerateCodeAsync(tableId, dto, sqlCreateBy);
            if (result.GenMethod == 0 && result.ZipBytes != null && result.ZipBytes.Length > 0)
                return File(result.ZipBytes, "application/zip", result.ZipFileName ?? "codegen.zip");
            return Success(result, "生成成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    #endregion

    #region 代码预览

    /// <summary>
    /// 预览生成的代码文件（不落盘，仅用于模板校验）
    /// </summary>
    /// <param name="tableId">代码生成表配置 ID</param>
    /// <param name="dto">预览请求（模板可空；PathMappings 可覆盖内置路径解析）</param>
    /// <returns>预览结果（文件相对路径 + 内容 + 是否已存在）</returns>
    [HttpPost("preview/{tableId}")]
    [TaktPermission("code:generator:preview", "预览代码")]
    public async Task<IActionResult> PreviewCodeAsync(long tableId, [FromBody] TaktPreviewCodeRequestDto dto)
    {
        try
        {
            var sqlCreateBy = CurrentUserName ?? User?.Identity?.Name ?? "admin";
            var preview = await _workflowService.GeneratePreviewFilesAsync(
                tableId,
                dto.Templates,
                null,
                dto.TargetBasePath,
                sqlCreateBy,
                dto.PathMappings);
            return Success(preview, "预览成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    #endregion
}
