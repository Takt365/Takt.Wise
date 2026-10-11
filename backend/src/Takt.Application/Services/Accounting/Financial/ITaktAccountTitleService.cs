// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Accounting.Financial
// 文件名称：ITaktAccountTitleService.cs
// 创建时间：2026-08-21
// 创建人：Takt365(Cursor AI)
// 功能描述：会计科目应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Accounting.Financial;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Accounting.Financial;

/// <summary>
/// 会计科目应用服务接口
/// </summary>
public interface ITaktAccountTitleService
{
    /// <summary>
    /// 获取会计科目列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktAccountTitleDto>> GetAccountTitleListAsync(TaktAccountTitleQueryDto queryDto);

    /// <summary>
    /// 根据ID获取会计科目
    /// </summary>
    /// <param name="id">会计科目ID</param>
    /// <returns>DTO</returns>
    Task<TaktAccountTitleDto?> GetAccountTitleByIdAsync(long id);

    /// <summary>
    /// 获取会计科目平铺选项（DictValue=AccountTitleCode；供统驭科目等业务字段下拉）
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配编码/名称）</param>
    /// <param name="reconciliationOnly">为 true 时仅辅助核算科目（IsAuxiliary=1）</param>
    /// <param name="auxiliaryType">辅助核算类型（可选；D=客户 K=供应商 等）</param>
    /// <returns>下拉选项（有上限，禁止全表）</returns>
    Task<List<TaktSelectOption>> GetAccountTitleOptionsAsync(string? plantCode = null, string? keyword = null, bool reconciliationOnly = false, string? auxiliaryType = null);

    /// <summary>
    /// 获取会计科目树形选项列表（懒加载：仅 parentId 直接子级一层；DictValue 为 AccountTitleCode）
    /// </summary>
    /// <param name="parentId">父级ID（0=根）</param>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>树形选项（一层）</returns>
    Task<List<TaktTreeSelectOption>> GetAccountTitleTreeOptionsAsync(long parentId = 0, string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 获取会计科目树形列表（懒加载：仅 parentId 直接子级一层）
    /// </summary>
    /// <param name="parentId">父级ID（0=根）</param>
    /// <param name="includeDisabled">是否包含禁用项</param>
    /// <returns>树形列表（一层）</returns>
    Task<List<TaktAccountTitleTreeDto>> GetAccountTitleTreeAsync(long parentId = 0, bool includeDisabled = false);

    /// <summary>
    /// 创建会计科目
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktAccountTitleDto> CreateAccountTitleAsync(TaktAccountTitleCreateDto dto);

    /// <summary>
    /// 更新会计科目
    /// </summary>
    /// <param name="id">会计科目ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktAccountTitleDto> UpdateAccountTitleAsync(long id, TaktAccountTitleUpdateDto dto);

    /// <summary>
    /// 删除会计科目
    /// </summary>
    /// <param name="id">会计科目ID</param>
    /// <returns>任务</returns>
    Task DeleteAccountTitleByIdAsync(long id);

    /// <summary>
    /// 批量删除会计科目
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteAccountTitleBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新会计科目状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktAccountTitleDto> UpdateAccountTitleStatusAsync(TaktAccountTitleStatusDto dto);

    /// <summary>
    /// 更新会计科目排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    Task<TaktAccountTitleDto> UpdateAccountTitleSortAsync(TaktAccountTitleSortDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetAccountTitleTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入会计科目
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportAccountTitleAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出会计科目
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportAccountTitleAsync(TaktAccountTitleQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
