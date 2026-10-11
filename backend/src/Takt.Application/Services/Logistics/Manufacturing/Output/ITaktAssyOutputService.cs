// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Output
// 文件名称：ITaktAssyOutputService.cs
// 创建时间：2026-07-06
// 创建人：Takt365(Cursor AI)
// 功能描述：组立日报应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Output;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Output;

/// <summary>
/// 组立日报应用服务接口
/// </summary>
public interface ITaktAssyOutputService
{
    /// <summary>
    /// 获取组立日报列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktAssyOutputDto>> GetAssyOutputListAsync(TaktAssyOutputQueryDto queryDto);

    /// <summary>
    /// 根据ID获取组立日报
    /// </summary>
    /// <param name="id">组立日报ID</param>
    /// <returns>DTO</returns>
    Task<TaktAssyOutputDto?> GetAssyOutputByIdAsync(long id);

    /// <summary>
    /// 获取组立日报选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetAssyOutputOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 获取组立日报新增时固定的生产时段列表（13 条）
    /// </summary>
    /// <returns>生产时段字符串列表</returns>
    Task<IReadOnlyList<string>> GetAssyOutputDefaultTimePeriodsAsync();

    /// <summary>
    /// 获取组立不良日报新增用工单选项（来源已生产的组立日报，排除同日同工单已存在不良日报）
    /// </summary>
    /// <param name="excludeAssyDefectId">编辑态当前不良日报 ID（保留其对应组立日报在选项中）</param>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项，DictValue 为组立日报 Id</returns>
    Task<List<TaktSelectOption>> GetAssyOutputProdOrderOptionsAsync(string? plantCode = null, string? keyword = null, long? excludeAssyDefectId = null);

    /// <summary>
    /// 创建组立日报（主表标准产能 &gt; 0 时自动级联创建 13 条固定生产时段明细）
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktAssyOutputDto> CreateAssyOutputAsync(TaktAssyOutputCreateDto dto);

    /// <summary>
    /// 更新组立日报
    /// </summary>
    /// <param name="id">组立日报ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktAssyOutputDto> UpdateAssyOutputAsync(long id, TaktAssyOutputUpdateDto dto);

    /// <summary>
    /// 删除组立日报
    /// </summary>
    /// <param name="id">组立日报ID</param>
    /// <returns>任务</returns>
    Task DeleteAssyOutputByIdAsync(long id);

    /// <summary>
    /// 批量删除组立日报
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteAssyOutputBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetAssyOutputTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入组立日报
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportAssyOutputAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出组立日报
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportAssyOutputAsync(TaktAssyOutputQueryDto? query = null, string? sheetName = null, string? fileName = null);
}
