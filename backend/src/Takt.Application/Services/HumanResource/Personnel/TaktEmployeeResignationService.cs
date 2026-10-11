// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Personnel
// 文件名称：TaktEmployeeResignationService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：员工离职应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.HumanResource.Personnel;
using Takt.Domain.Entities.HumanResource.Personnel;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.HumanResource.Personnel;

/// <summary>
/// 员工离职应用服务
/// </summary>
public class TaktEmployeeResignationService : TaktServiceBase, ITaktEmployeeResignationService
{
    private readonly ITaktApprovalRepository<TaktEmployeeResignation> _employeeResignationRepository;
    private readonly ITaktCompanyRepository<TaktEmployee> _employeeRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="employeeResignationRepository">员工离职仓储</param>
    /// <param name="employeeRepository">员工仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEmployeeResignationService(
        ITaktApprovalRepository<TaktEmployeeResignation> employeeResignationRepository,
        ITaktCompanyRepository<TaktEmployee> employeeRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _employeeResignationRepository = employeeResignationRepository;
        _employeeRepository = employeeRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取员工离职列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEmployeeResignationDto>> GetEmployeeResignationListAsync(TaktEmployeeResignationQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktEmployeeResignationDto>.Create(
                new List<TaktEmployeeResignationDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _employeeResignationRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktEmployeeResignationDto>.Create(
            data.Adapt<List<TaktEmployeeResignationDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取员工离职
    /// </summary>
    /// <param name="id">员工离职ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeResignationDto?> GetEmployeeResignationByIdAsync(long id)
    {
        var entity = await _employeeResignationRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktEmployeeResignationDto>();
    }

    /// <summary>
    /// 获取员工离职选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetEmployeeResignationOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _employeeResignationRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.EmployeeName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.EmployeeCode,
            DictLabel = e.EmployeeName ?? e.EmployeeCode,
        }).ToList();
    }

    /// <summary>
    /// 创建员工离职
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeResignationDto> CreateEmployeeResignationAsync(TaktEmployeeResignationCreateDto dto)
    {
        var entity = dto.Adapt<TaktEmployeeResignation>();
        await StampEmployeeResignationEmployeeAsync(entity, dto);
        entity = await _employeeResignationRepository.CreateAsync(entity);
        return await GetEmployeeResignationByIdAsync(entity.Id) ?? entity.Adapt<TaktEmployeeResignationDto>();
    }

    /// <summary>
    /// 更新员工离职
    /// </summary>
    /// <param name="id">员工离职ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeResignationDto> UpdateEmployeeResignationAsync(long id, TaktEmployeeResignationUpdateDto dto)
    {
        var entity = await _employeeResignationRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("员工离职不存在");
        }
        dto.Adapt(entity);
        await StampEmployeeResignationEmployeeAsync(entity, dto);
        await _employeeResignationRepository.UpdateAsync(entity);
        return await GetEmployeeResignationByIdAsync(id) ?? throw new TaktBusinessException("员工离职不存在");
    }

    /// <summary>
    /// 删除员工离职
    /// </summary>
    /// <param name="id">员工离职ID</param>
    /// <returns>任务</returns>
    public async Task DeleteEmployeeResignationByIdAsync(long id)
    {
        var deleted = await _employeeResignationRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("员工离职不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除员工离职
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteEmployeeResignationBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteEmployeeResignationByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetEmployeeResignationTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktEmployeeResignationTemplateDto>(
            sheetName ?? "员工离职导入模板",
            fileName ?? "员工离职导入模板.xlsx");
    }

    /// <summary>
    /// 导入员工离职
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportEmployeeResignationAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktEmployeeResignationImportDto>(fileStream, sheetName ?? "员工离职导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktEmployeeResignation>();
                var importDto = rows[i].Adapt<TaktEmployeeResignationCreateDto>();
                await StampEmployeeResignationEmployeeAsync(entity, importDto);
                await _employeeResignationRepository.CreateAsync(entity);
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
    /// 导出员工离职
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEmployeeResignationAsync(TaktEmployeeResignationQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktEmployeeResignationQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEmployeeResignationExportDto>(),
                sheetName ?? "员工离职数据",
                fileName ?? "员工离职导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _employeeResignationRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEmployeeResignationExportDto>(),
                sheetName ?? "员工离职数据",
                fileName ?? "员工离职导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktEmployeeResignationExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "员工离职数据",
            fileName ?? "员工离职导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步员工离职主表外键（ManyToOne → 员工）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampEmployeeResignationEmployeeAsync(TaktEmployeeResignation entity, TaktEmployeeResignationCreateDto dto)
    {
        if (dto.EmployeeId <= 0)
        {
            return;
        }
        var master = await _employeeRepository.GetByIdAsync(dto.EmployeeId);
        if (master == null)
        {
            throw new TaktBusinessException("员工不存在");
        }
        entity.EmployeeId = master.Id;
        if (string.IsNullOrEmpty(entity.TenantCode))
        {
            entity.TenantCode = master.TenantCode;
        }
        if (string.IsNullOrEmpty(entity.CompanyCode))
        {
            entity.CompanyCode = master.CompanyCode;
        }
        if (string.IsNullOrEmpty(entity.CultureCode))
        {
            entity.CultureCode = master.CultureCode;
        }
        if (string.IsNullOrEmpty(entity.PlantCode))
        {
            entity.PlantCode = master.PlantCode;
        }
        if (string.IsNullOrEmpty(entity.EmployeeCode))
        {
            entity.EmployeeCode = master.EmployeeCode;
        }
        if (string.IsNullOrEmpty(entity.EmployeeName))
        {
            entity.EmployeeName = master.EmployeeName;
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建员工离职查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktEmployeeResignation, bool>> QueryExpression(TaktEmployeeResignationQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEmployeeResignation>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.EmployeeCode != null && x.EmployeeCode.Contains(keywords))
                || (x.EmployeeName != null && x.EmployeeName.Contains(keywords))
                || (x.Reason != null && x.Reason.Contains(keywords))
                || (x.HandoverNotes != null && x.HandoverNotes.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
            );
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CultureCode))
        {
            var cultureCode = queryDto.CultureCode;
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(cultureCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PlantCode))
        {
            var plantCode = queryDto.PlantCode;
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(plantCode));
        }

        if (queryDto?.EmployeeId.HasValue == true)
        {
            var employeeId = queryDto.EmployeeId.Value;
            exp = exp.And(x => x.EmployeeId == employeeId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EmployeeCode))
        {
            var employeeCode = queryDto.EmployeeCode;
            exp = exp.And(x => x.EmployeeCode != null && x.EmployeeCode.Contains(employeeCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EmployeeName))
        {
            var employeeName = queryDto.EmployeeName;
            exp = exp.And(x => x.EmployeeName != null && x.EmployeeName.Contains(employeeName));
        }

        if (queryDto?.ResignationType.HasValue == true)
        {
            var resignationType = queryDto.ResignationType.Value;
            exp = exp.And(x => x.ResignationType == resignationType);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Reason))
        {
            var reason = queryDto.Reason;
            exp = exp.And(x => x.Reason != null && x.Reason.Contains(reason));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.HandoverNotes))
        {
            var handoverNotes = queryDto.HandoverNotes;
            exp = exp.And(x => x.HandoverNotes != null && x.HandoverNotes.Contains(handoverNotes));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ExtField))
        {
            var extField = queryDto.ExtField;
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(extField));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Remark))
        {
            var remark = queryDto.Remark;
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(remark));
        }

        if (queryDto?.ApplyDateStart.HasValue == true)
        {
            var applyDateStart = queryDto.ApplyDateStart.Value;
            exp = exp.And(x => x.ApplyDate >= applyDateStart);
        }

        if (queryDto?.ApplyDateEnd.HasValue == true)
        {
            var applyDateEnd = queryDto.ApplyDateEnd.Value;
            exp = exp.And(x => x.ApplyDate <= applyDateEnd);
        }

        if (queryDto?.LastWorkDateStart.HasValue == true)
        {
            var lastWorkDateStart = queryDto.LastWorkDateStart.Value;
            exp = exp.And(x => x.LastWorkDate >= lastWorkDateStart);
        }

        if (queryDto?.LastWorkDateEnd.HasValue == true)
        {
            var lastWorkDateEnd = queryDto.LastWorkDateEnd.Value;
            exp = exp.And(x => x.LastWorkDate <= lastWorkDateEnd);
        }

        if (queryDto?.TerminationDateStart.HasValue == true)
        {
            var terminationDateStart = queryDto.TerminationDateStart.Value;
            exp = exp.And(x => x.TerminationDate >= terminationDateStart);
        }

        if (queryDto?.TerminationDateEnd.HasValue == true)
        {
            var terminationDateEnd = queryDto.TerminationDateEnd.Value;
            exp = exp.And(x => x.TerminationDate <= terminationDateEnd);
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            var createdAtStart = queryDto.CreatedAtStart.Value;
            exp = exp.And(x => x.CreatedAt >= createdAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            var createdAtEnd = queryDto.CreatedAtEnd.Value;
            exp = exp.And(x => x.CreatedAt <= createdAtEnd);
        }

        return exp.ToExpression();
    }

    /// <summary>
    /// 是否存在任一业务查询条件（KeyWords / 字段 / 日期范围）；无参时列表与导出返回空，避免全表扫描
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>有条件为 true</returns>
    private static bool HasAnyListQueryFilter(TaktEmployeeResignationQueryDto? queryDto)
    {
        if (queryDto == null)
        {
            return false;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.KeyWords))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CultureCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PlantCode))
        {
            return true;
        }
        if (queryDto.EmployeeId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EmployeeCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EmployeeName))
        {
            return true;
        }
        if (queryDto.ResignationType.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Reason))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.HandoverNotes))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ExtField))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Remark))
        {
            return true;
        }
        if (queryDto.ApplyDateStart.HasValue || queryDto.ApplyDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.LastWorkDateStart.HasValue || queryDto.LastWorkDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.TerminationDateStart.HasValue || queryDto.TerminationDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.CreatedAtStart.HasValue || queryDto.CreatedAtEnd.HasValue)
        {
            return true;
        }
        return false;
    }
}
