// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Serial
// 文件名称：ITaktSerialInboundService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：序列号入库应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Serial;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Serial;

/// <summary>
/// 序列号入库应用服务接口
/// </summary>
public interface ITaktSerialInboundService
{
    /// <summary>
    /// 获取序列号入库列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktSerialInboundDto>> GetSerialInboundListAsync(TaktSerialInboundQueryDto queryDto);

    /// <summary>
    /// 根据ID获取序列号入库
    /// </summary>
    /// <param name="id">序列号入库ID</param>
    /// <returns>DTO</returns>
    Task<TaktSerialInboundDto?> GetSerialInboundByIdAsync(long id);

    /// <summary>
    /// 获取产品序列号入库选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetSerialInboundOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建序列号入库
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSerialInboundDto> CreateSerialInboundAsync(TaktSerialInboundCreateDto dto);

    /// <summary>
    /// 更新序列号入库
    /// </summary>
    /// <param name="id">序列号入库ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSerialInboundDto> UpdateSerialInboundAsync(long id, TaktSerialInboundUpdateDto dto);

    /// <summary>
    /// 删除序列号入库
    /// </summary>
    /// <param name="id">序列号入库ID</param>
    /// <returns>任务</returns>
    Task DeleteSerialInboundByIdAsync(long id);

    /// <summary>
    /// 批量删除序列号入库
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteSerialInboundBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetSerialInboundTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入序列号入库
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportSerialInboundAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出序列号入库
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportSerialInboundAsync(TaktSerialInboundQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
