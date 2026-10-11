// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Attendance
// 文件名称：TaktLeaveService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：请假信息应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.HumanResource.Attendance;
using Takt.Domain.Entities.HumanResource.Attendance;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.HumanResource.Attendance;

/// <summary>
/// 请假信息应用服务
/// </summary>
public class TaktLeaveService : TaktServiceBase, ITaktLeaveService
{
    private readonly ITaktApprovalRepository<TaktLeave> _leaveRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="leaveRepository">请假信息仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktLeaveService(
        ITaktApprovalRepository<TaktLeave> leaveRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _leaveRepository = leaveRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取请假信息列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktLeaveDto>> GetLeaveListAsync(TaktLeaveQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _leaveRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktLeaveDto>.Create(
            data.Adapt<List<TaktLeaveDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取请假信息
    /// </summary>
    /// <param name="id">请假信息ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktLeaveDto?> GetLeaveByIdAsync(long id)
    {
        var entity = await _leaveRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktLeaveDto>();
    }

    /// <summary>
    /// 获取请假信息选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetLeaveOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _leaveRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.LeaveStatus == 1,
            x => x.EmployeeName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.EmployeeName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建请假信息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktLeaveDto> CreateLeaveAsync(TaktLeaveCreateDto dto)
    {
        var entity = dto.Adapt<TaktLeave>();
        entity = await _leaveRepository.CreateAsync(entity);
        return await GetLeaveByIdAsync(entity.Id) ?? entity.Adapt<TaktLeaveDto>();
    }

    /// <summary>
    /// 更新请假信息
    /// </summary>
    /// <param name="id">请假信息ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktLeaveDto> UpdateLeaveAsync(long id, TaktLeaveUpdateDto dto)
    {
        var entity = await _leaveRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("请假信息不存在");
        }
        dto.Adapt(entity);
        await _leaveRepository.UpdateAsync(entity);
        return await GetLeaveByIdAsync(id) ?? throw new TaktBusinessException("请假信息不存在");
    }

    /// <summary>
    /// 删除请假信息
    /// </summary>
    /// <param name="id">请假信息ID</param>
    /// <returns>任务</returns>
    public async Task DeleteLeaveByIdAsync(long id)
    {
        var deleted = await _leaveRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("请假信息不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除请假信息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteLeaveBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteLeaveByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新请假信息状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktLeaveDto> UpdateLeaveStatusAsync(TaktLeaveStatusDto dto)
    {
        var entity = await _leaveRepository.GetByIdAsync(dto.LeaveId);
        if (entity == null)
        {
            throw new TaktBusinessException("请假信息不存在");
        }
        entity.LeaveStatus = dto.LeaveStatus;
        await _leaveRepository.UpdateAsync(entity);
        return await GetLeaveByIdAsync(dto.LeaveId) ?? throw new TaktBusinessException("请假信息不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetLeaveTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktLeaveTemplateDto>(
            sheetName ?? "请假信息导入模板",
            fileName ?? "请假信息导入模板.xlsx");
    }

    /// <summary>
    /// 导入请假信息
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportLeaveAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktLeaveImportDto>(fileStream, sheetName ?? "请假信息导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktLeave>();
                await _leaveRepository.CreateAsync(entity);
                success += 1;
            }
            catch (Exception ex)
            {
                fail += 1;
                errors.Add($"第{i + 2}行: {ex.Message}");
            }
        }
        return (success, fail, errors);
    }

    /// <summary>
    /// 导出请假信息
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportLeaveAsync(TaktLeaveQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktLeaveQueryDto());
        var list = await _leaveRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktLeaveExportDto>(),
                sheetName ?? "请假信息数据",
                fileName ?? "请假信息导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktLeaveExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "请假信息数据",
            fileName ?? "请假信息导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建请假信息查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktLeave, bool>> QueryExpression(TaktLeaveQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktLeave>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.EmployeeId).Contains(keywords)
                || (x.EmployeeName != null && x.EmployeeName.Contains(keywords))
                || SqlFunc.ToString(x.DeptId).Contains(keywords)
                || (x.DeptName != null && x.DeptName.Contains(keywords))
                || (x.LeaveType != null && x.LeaveType.Contains(keywords))
                || (x.Reason != null && x.Reason.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.Attachments != null && x.Attachments.Contains(keywords))
                || SqlFunc.ToString(x.HandlingBy).Contains(keywords)
                || (x.HandlingByName != null && x.HandlingByName.Contains(keywords))
                || (x.HandlingComment != null && x.HandlingComment.Contains(keywords))
                || SqlFunc.ToString(x.LeaveStatus).Contains(keywords)
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.StartDate).Contains(keywords)
                || SqlFunc.ToString(x.EndDate).Contains(keywords)
                || SqlFunc.ToString(x.HandlingAt).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.EmployeeId.HasValue == true)
        {
            exp = exp.And(x => x.EmployeeId == queryDto.EmployeeId);
        }

        if (!string.IsNullOrEmpty(queryDto?.EmployeeName))
        {
            exp = exp.And(x => x.EmployeeName != null && x.EmployeeName.Contains(queryDto.EmployeeName));
        }

        if (queryDto?.DeptId.HasValue == true)
        {
            exp = exp.And(x => x.DeptId == queryDto.DeptId);
        }

        if (!string.IsNullOrEmpty(queryDto?.DeptName))
        {
            exp = exp.And(x => x.DeptName != null && x.DeptName.Contains(queryDto.DeptName));
        }

        if (!string.IsNullOrEmpty(queryDto?.LeaveType))
        {
            exp = exp.And(x => x.LeaveType != null && x.LeaveType.Contains(queryDto.LeaveType));
        }

        if (!string.IsNullOrEmpty(queryDto?.Reason))
        {
            exp = exp.And(x => x.Reason != null && x.Reason.Contains(queryDto.Reason));
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.Attachments))
        {
            exp = exp.And(x => x.Attachments != null && x.Attachments.Contains(queryDto.Attachments));
        }

        if (queryDto?.HandlingBy.HasValue == true)
        {
            exp = exp.And(x => x.HandlingBy == queryDto.HandlingBy);
        }

        if (!string.IsNullOrEmpty(queryDto?.HandlingByName))
        {
            exp = exp.And(x => x.HandlingByName != null && x.HandlingByName.Contains(queryDto.HandlingByName));
        }

        if (!string.IsNullOrEmpty(queryDto?.HandlingComment))
        {
            exp = exp.And(x => x.HandlingComment != null && x.HandlingComment.Contains(queryDto.HandlingComment));
        }

        if (queryDto?.LeaveStatus.HasValue == true)
        {
            exp = exp.And(x => x.LeaveStatus == queryDto.LeaveStatus);
        }


        if (!string.IsNullOrEmpty(queryDto?.CultureCode))
        {
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(queryDto.CultureCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.StartDateStart.HasValue == true)
        {
            exp = exp.And(x => x.StartDate >= queryDto.StartDateStart);
        }

        if (queryDto?.StartDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.StartDate <= queryDto.StartDateEnd);
        }

        if (queryDto?.EndDateStart.HasValue == true)
        {
            exp = exp.And(x => x.EndDate >= queryDto.EndDateStart);
        }

        if (queryDto?.EndDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.EndDate <= queryDto.EndDateEnd);
        }

        if (queryDto?.HandlingAtStart.HasValue == true)
        {
            exp = exp.And(x => x.HandlingAt >= queryDto.HandlingAtStart);
        }

        if (queryDto?.HandlingAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.HandlingAt <= queryDto.HandlingAtEnd);
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt >= queryDto.CreatedAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt <= queryDto.CreatedAtEnd);
        }

        return exp.ToExpression();
    }
}
