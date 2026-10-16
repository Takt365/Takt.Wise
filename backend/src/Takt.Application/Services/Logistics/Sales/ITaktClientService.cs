// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：ITaktClientService.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Cursor AI)
// 功能描述：客户端信息应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Sales;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Sales;

/// <summary>
/// 客户端信息应用服务接口
/// </summary>
public interface ITaktClientService
{
    /// <summary>
    /// 获取客户端信息列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktClientDto>> GetClientListAsync(TaktClientQueryDto queryDto);

    /// <summary>
    /// 根据ID获取客户端信息
    /// </summary>
    /// <param name="id">客户端信息ID</param>
    /// <returns>DTO</returns>
    Task<TaktClientDto?> GetClientByIdAsync(long id);

    /// <summary>
    /// 获取客户端信息选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetClientOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建客户端信息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktClientDto> CreateClientAsync(TaktClientCreateDto dto);

    /// <summary>
    /// 更新客户端信息
    /// </summary>
    /// <param name="id">客户端信息ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktClientDto> UpdateClientAsync(long id, TaktClientUpdateDto dto);

    /// <summary>
    /// 删除客户端信息
    /// </summary>
    /// <param name="id">客户端信息ID</param>
    /// <returns>任务</returns>
    Task DeleteClientByIdAsync(long id);

    /// <summary>
    /// 批量删除客户端信息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteClientBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新客户端信息状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktClientDto> UpdateClientStatusAsync(TaktClientStatusDto dto);

    /// <summary>
    /// 更新客户端信息排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    Task<TaktClientDto> UpdateClientSortAsync(TaktClientSortDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetClientTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入客户端信息
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportClientAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出客户端信息
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportClientAsync(TaktClientQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
