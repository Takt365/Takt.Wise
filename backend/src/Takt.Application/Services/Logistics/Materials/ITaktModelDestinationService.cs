// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：ITaktModelDestinationService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：型号目的地应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Materials;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Materials;

/// <summary>
/// 型号目的地应用服务接口
/// </summary>
public interface ITaktModelDestinationService
{
    /// <summary>
    /// 获取型号目的地列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktModelDestinationDto>> GetModelDestinationListAsync(TaktModelDestinationQueryDto queryDto);

    /// <summary>
    /// 根据ID获取型号目的地
    /// </summary>
    /// <param name="id">型号目的地ID</param>
    /// <returns>DTO</returns>
    Task<TaktModelDestinationDto?> GetModelDestinationByIdAsync(long id);

    /// <summary>
    /// 获取型号目的地选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetModelDestinationOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 获取机种下拉选项（ModelCode 去重，DictValue=机种编码）
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>机种下拉选项</returns>
    Task<List<TaktSelectOption>> GetModelOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 根据机种编码获取级联物料选项（DictValue=物料编码，供产品/BOM 查询级联）
    /// </summary>
    /// <param name="modelCode">机种编码</param>
    /// <returns>物料下拉选项</returns>
    Task<List<TaktSelectOption>> GetMaterialOptionsByModelAsync(string modelCode);

    /// <summary>
    /// 根据物料编码获取机种名称与仕向地信息（首条）
    /// </summary>
    /// <param name="materialCode">物料编码</param>
    /// <returns>型号目的地 DTO；未匹配时返回 null</returns>
    Task<TaktModelDestinationDto?> GetModelDestinationByMaterialAsync(string materialCode);

    /// <summary>
    /// 根据物料编码获取机种仕向列表
    /// </summary>
    /// <param name="materialCode">物料编码</param>
    /// <returns>型号目的地 DTO 列表</returns>
    Task<List<TaktModelDestinationDto>> GetModelDestinationListByMaterialAsync(string materialCode);

    /// <summary>
    /// 根据物料编码获取机种仕向选项列表
    /// </summary>
    /// <param name="materialCode">物料编码</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetModelDestinationOptionsByMaterialAsync(string materialCode);

    /// <summary>
    /// 创建型号目的地
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktModelDestinationDto> CreateModelDestinationAsync(TaktModelDestinationCreateDto dto);

    /// <summary>
    /// 更新型号目的地
    /// </summary>
    /// <param name="id">型号目的地ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktModelDestinationDto> UpdateModelDestinationAsync(long id, TaktModelDestinationUpdateDto dto);

    /// <summary>
    /// 删除型号目的地
    /// </summary>
    /// <param name="id">型号目的地ID</param>
    /// <returns>任务</returns>
    Task DeleteModelDestinationByIdAsync(long id);

    /// <summary>
    /// 批量删除型号目的地
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteModelDestinationBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新型号目的地排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    Task<TaktModelDestinationDto> UpdateModelDestinationSortAsync(TaktModelDestinationSortDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetModelDestinationTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入型号目的地
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportModelDestinationAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出型号目的地
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportModelDestinationAsync(TaktModelDestinationQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
