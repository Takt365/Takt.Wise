// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Personnel
// 文件名称：TaktEmployeeJoinedService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：员工入职上岗应用服务实现
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
/// 员工入职上岗应用服务
/// </summary>
public class TaktEmployeeJoinedService : TaktServiceBase, ITaktEmployeeJoinedService
{
    private readonly ITaktApprovalRepository<TaktEmployeeJoined> _employeeJoinedRepository;
    private readonly ITaktCompanyRepository<TaktEmployee> _employeeRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="employeeJoinedRepository">员工入职上岗仓储</param>
    /// <param name="employeeRepository">员工仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEmployeeJoinedService(
        ITaktApprovalRepository<TaktEmployeeJoined> employeeJoinedRepository,
        ITaktCompanyRepository<TaktEmployee> employeeRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _employeeJoinedRepository = employeeJoinedRepository;
        _employeeRepository = employeeRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取员工入职上岗列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEmployeeJoinedDto>> GetEmployeeJoinedListAsync(TaktEmployeeJoinedQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktEmployeeJoinedDto>.Create(
                new List<TaktEmployeeJoinedDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _employeeJoinedRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktEmployeeJoinedDto>.Create(
            data.Adapt<List<TaktEmployeeJoinedDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取员工入职上岗
    /// </summary>
    /// <param name="id">员工入职上岗ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeJoinedDto?> GetEmployeeJoinedByIdAsync(long id)
    {
        var entity = await _employeeJoinedRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktEmployeeJoinedDto>();
    }

    /// <summary>
    /// 获取员工入职上岗选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetEmployeeJoinedOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _employeeJoinedRepository.GetListAsync(
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
    /// 创建员工入职上岗
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeJoinedDto> CreateEmployeeJoinedAsync(TaktEmployeeJoinedCreateDto dto)
    {
        var entity = dto.Adapt<TaktEmployeeJoined>();
        await StampEmployeeJoinedEmployeeAsync(entity, dto);
        entity = await _employeeJoinedRepository.CreateAsync(entity);
        return await GetEmployeeJoinedByIdAsync(entity.Id) ?? entity.Adapt<TaktEmployeeJoinedDto>();
    }

    /// <summary>
    /// 更新员工入职上岗
    /// </summary>
    /// <param name="id">员工入职上岗ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeJoinedDto> UpdateEmployeeJoinedAsync(long id, TaktEmployeeJoinedUpdateDto dto)
    {
        var entity = await _employeeJoinedRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("员工入职上岗不存在");
        }
        dto.Adapt(entity);
        await StampEmployeeJoinedEmployeeAsync(entity, dto);
        await _employeeJoinedRepository.UpdateAsync(entity);
        return await GetEmployeeJoinedByIdAsync(id) ?? throw new TaktBusinessException("员工入职上岗不存在");
    }

    /// <summary>
    /// 删除员工入职上岗
    /// </summary>
    /// <param name="id">员工入职上岗ID</param>
    /// <returns>任务</returns>
    public async Task DeleteEmployeeJoinedByIdAsync(long id)
    {
        var deleted = await _employeeJoinedRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("员工入职上岗不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除员工入职上岗
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteEmployeeJoinedBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteEmployeeJoinedByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetEmployeeJoinedTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktEmployeeJoinedTemplateDto>(
            sheetName ?? "员工入职上岗导入模板",
            fileName ?? "员工入职上岗导入模板.xlsx");
    }

    /// <summary>
    /// 导入员工入职上岗
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportEmployeeJoinedAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktEmployeeJoinedImportDto>(fileStream, sheetName ?? "员工入职上岗导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktEmployeeJoined>();
                var importDto = rows[i].Adapt<TaktEmployeeJoinedCreateDto>();
                await StampEmployeeJoinedEmployeeAsync(entity, importDto);
                await _employeeJoinedRepository.CreateAsync(entity);
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
    /// 导出员工入职上岗
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEmployeeJoinedAsync(TaktEmployeeJoinedQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktEmployeeJoinedQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEmployeeJoinedExportDto>(),
                sheetName ?? "员工入职上岗数据",
                fileName ?? "员工入职上岗导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _employeeJoinedRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEmployeeJoinedExportDto>(),
                sheetName ?? "员工入职上岗数据",
                fileName ?? "员工入职上岗导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktEmployeeJoinedExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "员工入职上岗数据",
            fileName ?? "员工入职上岗导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步员工入职上岗主表外键（ManyToOne → 员工）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampEmployeeJoinedEmployeeAsync(TaktEmployeeJoined entity, TaktEmployeeJoinedCreateDto dto)
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
    /// 构建员工入职上岗查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktEmployeeJoined, bool>> QueryExpression(TaktEmployeeJoinedQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEmployeeJoined>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.EmployeeCode != null && x.EmployeeCode.Contains(keywords))
                || (x.EmployeeName != null && x.EmployeeName.Contains(keywords))
                || (x.DeptName != null && x.DeptName.Contains(keywords))
                || (x.PostName != null && x.PostName.Contains(keywords))
                || (x.JobTitle != null && x.JobTitle.Contains(keywords))
                || (x.DirectManagerName != null && x.DirectManagerName.Contains(keywords))
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

        if (queryDto?.OnboardingId.HasValue == true)
        {
            var onboardingId = queryDto.OnboardingId.Value;
            exp = exp.And(x => x.OnboardingId == onboardingId);
        }

        if (queryDto?.DeptId.HasValue == true)
        {
            var deptId = queryDto.DeptId.Value;
            exp = exp.And(x => x.DeptId == deptId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DeptName))
        {
            var deptName = queryDto.DeptName;
            exp = exp.And(x => x.DeptName != null && x.DeptName.Contains(deptName));
        }

        if (queryDto?.PostId.HasValue == true)
        {
            var postId = queryDto.PostId.Value;
            exp = exp.And(x => x.PostId == postId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PostName))
        {
            var postName = queryDto.PostName;
            exp = exp.And(x => x.PostName != null && x.PostName.Contains(postName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.JobTitle))
        {
            var jobTitle = queryDto.JobTitle;
            exp = exp.And(x => x.JobTitle != null && x.JobTitle.Contains(jobTitle));
        }

        if (queryDto?.WorkNature.HasValue == true)
        {
            var workNature = queryDto.WorkNature.Value;
            exp = exp.And(x => x.WorkNature == workNature);
        }

        if (queryDto?.EmploymentType.HasValue == true)
        {
            var employmentType = queryDto.EmploymentType.Value;
            exp = exp.And(x => x.EmploymentType == employmentType);
        }

        if (queryDto?.DirectManagerId.HasValue == true)
        {
            var directManagerId = queryDto.DirectManagerId.Value;
            exp = exp.And(x => x.DirectManagerId == directManagerId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DirectManagerName))
        {
            var directManagerName = queryDto.DirectManagerName;
            exp = exp.And(x => x.DirectManagerName != null && x.DirectManagerName.Contains(directManagerName));
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

        if (queryDto?.JoinedDateStart.HasValue == true)
        {
            var joinedDateStart = queryDto.JoinedDateStart.Value;
            exp = exp.And(x => x.JoinedDate >= joinedDateStart);
        }

        if (queryDto?.JoinedDateEnd.HasValue == true)
        {
            var joinedDateEnd = queryDto.JoinedDateEnd.Value;
            exp = exp.And(x => x.JoinedDate <= joinedDateEnd);
        }

        if (queryDto?.ProbationEndDateStart.HasValue == true)
        {
            var probationEndDateStart = queryDto.ProbationEndDateStart.Value;
            exp = exp.And(x => x.ProbationEndDate >= probationEndDateStart);
        }

        if (queryDto?.ProbationEndDateEnd.HasValue == true)
        {
            var probationEndDateEnd = queryDto.ProbationEndDateEnd.Value;
            exp = exp.And(x => x.ProbationEndDate <= probationEndDateEnd);
        }

        if (queryDto?.RegularDateStart.HasValue == true)
        {
            var regularDateStart = queryDto.RegularDateStart.Value;
            exp = exp.And(x => x.RegularDate >= regularDateStart);
        }

        if (queryDto?.RegularDateEnd.HasValue == true)
        {
            var regularDateEnd = queryDto.RegularDateEnd.Value;
            exp = exp.And(x => x.RegularDate <= regularDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktEmployeeJoinedQueryDto? queryDto)
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
        if (queryDto.OnboardingId.HasValue)
        {
            return true;
        }
        if (queryDto.DeptId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DeptName))
        {
            return true;
        }
        if (queryDto.PostId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PostName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.JobTitle))
        {
            return true;
        }
        if (queryDto.WorkNature.HasValue)
        {
            return true;
        }
        if (queryDto.EmploymentType.HasValue)
        {
            return true;
        }
        if (queryDto.DirectManagerId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DirectManagerName))
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
        if (queryDto.JoinedDateStart.HasValue || queryDto.JoinedDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.ProbationEndDateStart.HasValue || queryDto.ProbationEndDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.RegularDateStart.HasValue || queryDto.RegularDateEnd.HasValue)
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
