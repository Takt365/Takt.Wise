// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Foundation
// 文件名称：TaktFilesController.cs
// 创建时间：2026-06-13
// 创建人：Takt365(Cursor AI)
// 功能描述：文件控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Foundation;
using Takt.Application.Services.Foundation;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;

namespace Takt.WebApi.Controllers.Foundation;

/// <summary>
/// 文件控制器
/// 提供文件的 REST API
/// </summary>
[ApiModule(8, "基础设置")]
[Route("api/[controller]", Name = "文件")]
public class TaktFilesController : TaktControllerBase
{
    private readonly ITaktFileService _fileService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="fileService">文件服务</param>
    public TaktFilesController(ITaktFileService fileService)
    {
        _fileService = fileService;
    }

    /// <summary>
    /// 获取文件列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("foundation:file:list", "文件列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetFileListAsync([FromQuery] TaktFileQueryDto queryDto)
    {
        try
        {
            var result = await _fileService.GetFileListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取文件选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("foundation:file:query", "文件选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetFileOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _fileService.GetFileOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取上传策略（须在 {id} 路由之前注册）
    /// </summary>
    /// <param name="totalSizeBytes">文件总大小（字节）</param>
    /// <returns>上传策略</returns>
    [TaktPermission("foundation:file:create", "文件上传策略")]
    [HttpGet("upload-policy")]
    public IActionResult GetFileUploadPolicy([FromQuery] long? totalSizeBytes = null)
    {
        try
        {
            var result = _fileService.GetFileUploadPolicy(totalSizeBytes);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 整文件上传
    /// </summary>
    /// <param name="file">文件</param>
    /// <param name="meta">业务元数据</param>
    /// <returns>上传结果</returns>
    [TaktPermission("foundation:file:create", "上传文件")]
    [HttpPost("upload")]
    public async Task<IActionResult> UploadFileAsync(IFormFile file, [FromForm] TaktFileUploadMetaDto? meta = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要上传的文件");
            }

            await using var stream = file.OpenReadStream();
            var clientIp = TaktLocationHelper.ResolveClientIp(HttpContext);
            var result = await _fileService.UploadFileAsync(
                stream,
                file.FileName,
                file.ContentType,
                meta,
                clientIp);
            return Success(result, "上传成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 检查分片是否已上传
    /// </summary>
    /// <param name="request">检查参数</param>
    /// <returns>是否存在</returns>
    [TaktPermission("foundation:file:create", "检查文件分片")]
    [HttpPost("chunks/check")]
    public async Task<IActionResult> CheckFileChunkAsync([FromBody] TaktFileChunkCheckRequest request)
    {
        try
        {
            var result = await _fileService.CheckFileChunkAsync(request);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 列出已上传分片序号
    /// </summary>
    /// <param name="request">查询参数</param>
    /// <returns>已上传分片序号</returns>
    [TaktPermission("foundation:file:create", "列出文件分片")]
    [HttpPost("chunks/list")]
    public async Task<IActionResult> ListFileChunksAsync([FromBody] TaktFileChunkListRequest request)
    {
        try
        {
            var result = await _fileService.ListFileChunksAsync(request);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 上传单个分片
    /// </summary>
    /// <param name="file">分片文件</param>
    /// <param name="request">分片元数据</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:file:create", "上传文件分片")]
    [HttpPost("chunks")]
    public async Task<IActionResult> UploadFileChunkAsync(IFormFile file, [FromForm] TaktFileChunkUploadRequest request)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要上传的分片文件");
            }

            await using var stream = file.OpenReadStream();
            await _fileService.UploadFileChunkAsync(stream, request);
            return Success("上传成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 合并分片并完成上传
    /// </summary>
    /// <param name="dto">合并参数</param>
    /// <returns>上传结果</returns>
    [TaktPermission("foundation:file:create", "合并文件分片")]
    [HttpPost("chunks/merge")]
    public async Task<IActionResult> MergeFileChunksAsync([FromBody] TaktFileChunkMergeDto dto)
    {
        try
        {
            var clientIp = TaktLocationHelper.ResolveClientIp(HttpContext);
            var result = await _fileService.MergeFileChunksAsync(dto, clientIp);
            return Success(result, "上传成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 取消分片上传
    /// </summary>
    /// <param name="identifier">上传会话标识</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:file:create", "取消文件分片上传")]
    [HttpDelete("chunks/cancel")]
    public async Task<IActionResult> CancelFileChunksAsync([FromQuery] string identifier)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                return BadRequest("identifier 不能为空");
            }

            await _fileService.CancelFileChunksAsync(identifier);
            return Success("已取消");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取文件（字面量 GET 路由须在其之前注册）
    /// </summary>
    /// <param name="id">文件ID</param>
    /// <returns>文件DTO</returns>
    [TaktPermission("foundation:file:query", "文件详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFileByIdAsync(long id)
    {
        try
        {
            var result = await _fileService.GetFileByIdAsync(id);
            if (result == null)
            {
                return NotFound("文件不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 下载文件（按ID打开物理流）
    /// </summary>
    /// <param name="id">文件ID</param>
    /// <returns>文件流</returns>
    [TaktPermission("foundation:file:download", "下载文件")]
    [HttpGet("{id}/download")]
    public async Task<IActionResult> DownloadFileByIdAsync(long id)
    {
        try
        {
            var result = await _fileService.DownloadFileByIdAsync(id);
            return File(result.Stream, result.ContentType, result.FileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建文件
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>文件DTO</returns>
    [TaktPermission("foundation:file:create", "创建文件")]
    [HttpPost]
    public async Task<IActionResult> CreateFileAsync([FromBody] TaktFileCreateDto dto)
    {
        try
        {
            var result = await _fileService.CreateFileAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新文件
    /// </summary>
    /// <param name="id">文件ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>文件DTO</returns>
    [TaktPermission("foundation:file:update", "更新文件")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFileAsync(long id, [FromBody] TaktFileUpdateDto dto)
    {
        try
        {
            var result = await _fileService.UpdateFileAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="id">文件ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:file:delete", "删除文件")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFileByIdAsync(long id)
    {
        try
        {
            await _fileService.DeleteFileByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除文件
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:file:delete", "批量删除文件")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteFileBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _fileService.DeleteFileBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新文件状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>文件DTO</returns>
    [TaktPermission("foundation:file:update", "更新文件状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateFileStatusAsync([FromBody] TaktFileStatusDto dto)
    {
        try
        {
            var result = await _fileService.UpdateFileStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新文件是否公开
    /// </summary>
    /// <param name="dto">公开范围 DTO</param>
    /// <returns>文件DTO</returns>
    [TaktPermission("foundation:file:update", "更新文件公开范围")]
    [HttpPut("public")]
    public async Task<IActionResult> UpdateFilePublicAsync([FromBody] TaktFilePublicDto dto)
    {
        try
        {
            var result = await _fileService.UpdateFilePublicAsync(dto);
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
    [TaktPermission("foundation:file:import", "获取文件导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetFileTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _fileService.GetFileTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入文件
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("foundation:file:import", "导入文件")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportFileAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _fileService.ImportFileAsync(stream, sheetName);
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
    /// 导出文件
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("foundation:file:export", "导出文件")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportFileAsync([FromQuery] TaktFileQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _fileService.ExportFileAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}