// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：ITaktDictDataService.cs
// 创建时间：2026-06-02
// 创建人：Takt365(Cursor AI)
// 功能描述：字典数据应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Foundation;
using Takt.Shared.Models;
using Takt.Shared.Models.Foundation;
using Takt.Shared.Options;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 字典数据应用服务接口
/// </summary>
public interface ITaktDictDataService
{
    /// <summary>
    /// 获取字典数据列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktDictDataDto>> GetDictDataListAsync(TaktDictDataQueryDto queryDto);

    /// <summary>
    /// 根据ID获取字典数据
    /// </summary>
    /// <param name="id">字典数据ID</param>
    /// <returns>DTO</returns>
    Task<TaktDictDataDto?> GetDictDataByIdAsync(long id);

    /// <summary>
    /// 获取字典数据选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetDictDataOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建字典数据
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktDictDataDto> CreateDictDataAsync(TaktDictDataCreateDto dto);

    /// <summary>
    /// 更新字典数据
    /// </summary>
    /// <param name="id">字典数据ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktDictDataDto> UpdateDictDataAsync(long id, TaktDictDataUpdateDto dto);

    /// <summary>
    /// 删除字典数据
    /// </summary>
    /// <param name="id">字典数据ID</param>
    /// <returns>任务</returns>
    Task DeleteDictDataByIdAsync(long id);

    /// <summary>
    /// 批量删除字典数据
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteDictDataBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新字典数据排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    Task<TaktDictDataDto> UpdateDictDataSortAsync(TaktDictDataSortDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetDictDataTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入字典数据
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportDictDataAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出字典数据
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportDictDataAsync(TaktDictDataQueryDto? query = null, string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 获取当前租户下全部字典数据（扁平列表，含 DictTypeCode）
    /// </summary>
    /// <returns>全部字典数据 DTO</returns>
    Task<TaktDataDictAllDto> GetDataDictAllAsync();

    /// <summary>
    /// 按字典类型编码批量构建双向快照（导入 GetCode / 导出 GetName）
    /// </summary>
    /// <param name="dictTypeCodes">字典类型编码（与种子 dict_type_code 一致）</param>
    /// <returns>租户级预加载快照</returns>
    Task<TaktDictSnapshot> CreateDictSnapshotAsync(params string[] dictTypeCodes);

    /// <summary>
    /// 按字典类型编码构建落库上下文（快照 + 多选排序映射）
    /// </summary>
    /// <param name="dictTypeCodes">字典类型编码</param>
    /// <returns>落库上下文</returns>
    Task<TaktDictStorageContext> CreateDictStorageContextAsync(params string[] dictTypeCodes);

}
