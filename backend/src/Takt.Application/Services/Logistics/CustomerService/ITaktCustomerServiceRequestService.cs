// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.CustomerService
// 文件名称：ITaktCustomerServiceRequestService.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Cursor AI)
// 功能描述：服务请求应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.CustomerService;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.CustomerService;

/// <summary>
/// 服务请求应用服务接口
/// </summary>
public interface ITaktCustomerServiceRequestService
{
    /// <summary>
    /// 获取服务请求列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktCustomerServiceRequestDto>> GetCustomerServiceRequestListAsync(TaktCustomerServiceRequestQueryDto queryDto);

    /// <summary>
    /// 根据ID获取服务请求
    /// </summary>
    /// <param name="id">服务请求ID</param>
    /// <returns>DTO</returns>
    Task<TaktCustomerServiceRequestDto?> GetCustomerServiceRequestByIdAsync(long id);

    /// <summary>
    /// 获取服务请求选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetCustomerServiceRequestOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建服务请求
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktCustomerServiceRequestDto> CreateCustomerServiceRequestAsync(TaktCustomerServiceRequestCreateDto dto);

    /// <summary>
    /// 更新服务请求
    /// </summary>
    /// <param name="id">服务请求ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktCustomerServiceRequestDto> UpdateCustomerServiceRequestAsync(long id, TaktCustomerServiceRequestUpdateDto dto);

    /// <summary>
    /// 删除服务请求
    /// </summary>
    /// <param name="id">服务请求ID</param>
    /// <returns>任务</returns>
    Task DeleteCustomerServiceRequestByIdAsync(long id);

    /// <summary>
    /// 批量删除服务请求
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteCustomerServiceRequestBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新服务请求状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktCustomerServiceRequestDto> UpdateCustomerServiceRequestStatusAsync(TaktCustomerServiceRequestStatusDto dto);

    /// <summary>
    /// 更新服务请求排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    Task<TaktCustomerServiceRequestDto> UpdateCustomerServiceRequestSortAsync(TaktCustomerServiceRequestSortDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetCustomerServiceRequestTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入服务请求
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportCustomerServiceRequestAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出服务请求
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportCustomerServiceRequestAsync(TaktCustomerServiceRequestQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
