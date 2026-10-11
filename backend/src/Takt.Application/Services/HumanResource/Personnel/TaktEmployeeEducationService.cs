// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Personnel
// 文件名称：TaktEmployeeEducationService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：员工教育经历应用服务实现
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
/// 员工教育经历应用服务
/// </summary>
public class TaktEmployeeEducationService : TaktServiceBase, ITaktEmployeeEducationService
{
    private readonly ITaktCompanyRepository<TaktEmployeeEducation> _employeeEducationRepository;
    private readonly ITaktCompanyRepository<TaktEmployee> _employeeRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="employeeEducationRepository">员工教育经历仓储</param>
    /// <param name="employeeRepository">员工仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEmployeeEducationService(
        ITaktCompanyRepository<TaktEmployeeEducation> employeeEducationRepository,
        ITaktCompanyRepository<TaktEmployee> employeeRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _employeeEducationRepository = employeeEducationRepository;
        _employeeRepository = employeeRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取员工教育经历列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEmployeeEducationDto>> GetEmployeeEducationListAsync(TaktEmployeeEducationQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktEmployeeEducationDto>.Create(
                new List<TaktEmployeeEducationDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _employeeEducationRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktEmployeeEducationDto>.Create(
            data.Adapt<List<TaktEmployeeEducationDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取员工教育经历
    /// </summary>
    /// <param name="id">员工教育经历ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeEducationDto?> GetEmployeeEducationByIdAsync(long id)
    {
        var entity = await _employeeEducationRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktEmployeeEducationDto>();
    }

    /// <summary>
    /// 获取员工教育经历选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetEmployeeEducationOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _employeeEducationRepository.GetListAsync(
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
    /// 创建员工教育经历
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeEducationDto> CreateEmployeeEducationAsync(TaktEmployeeEducationCreateDto dto)
    {
        var entity = dto.Adapt<TaktEmployeeEducation>();
        await StampEmployeeEducationEmployeeAsync(entity, dto);
        entity = await _employeeEducationRepository.CreateAsync(entity);
        return await GetEmployeeEducationByIdAsync(entity.Id) ?? entity.Adapt<TaktEmployeeEducationDto>();
    }

    /// <summary>
    /// 更新员工教育经历
    /// </summary>
    /// <param name="id">员工教育经历ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeEducationDto> UpdateEmployeeEducationAsync(long id, TaktEmployeeEducationUpdateDto dto)
    {
        var entity = await _employeeEducationRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("员工教育经历不存在");
        }
        dto.Adapt(entity);
        await StampEmployeeEducationEmployeeAsync(entity, dto);
        await _employeeEducationRepository.UpdateAsync(entity);
        return await GetEmployeeEducationByIdAsync(id) ?? throw new TaktBusinessException("员工教育经历不存在");
    }

    /// <summary>
    /// 删除员工教育经历
    /// </summary>
    /// <param name="id">员工教育经历ID</param>
    /// <returns>任务</returns>
    public async Task DeleteEmployeeEducationByIdAsync(long id)
    {
        var deleted = await _employeeEducationRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("员工教育经历不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除员工教育经历
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteEmployeeEducationBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteEmployeeEducationByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetEmployeeEducationTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktEmployeeEducationTemplateDto>(
            sheetName ?? "员工教育经历导入模板",
            fileName ?? "员工教育经历导入模板.xlsx");
    }

    /// <summary>
    /// 导入员工教育经历
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportEmployeeEducationAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktEmployeeEducationImportDto>(fileStream, sheetName ?? "员工教育经历导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktEmployeeEducation>();
                var importDto = rows[i].Adapt<TaktEmployeeEducationCreateDto>();
                await StampEmployeeEducationEmployeeAsync(entity, importDto);
                await _employeeEducationRepository.CreateAsync(entity);
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
    /// 导出员工教育经历
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEmployeeEducationAsync(TaktEmployeeEducationQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktEmployeeEducationQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEmployeeEducationExportDto>(),
                sheetName ?? "员工教育经历数据",
                fileName ?? "员工教育经历导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _employeeEducationRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEmployeeEducationExportDto>(),
                sheetName ?? "员工教育经历数据",
                fileName ?? "员工教育经历导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktEmployeeEducationExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "员工教育经历数据",
            fileName ?? "员工教育经历导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步员工教育经历主表外键（ManyToOne → 员工）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampEmployeeEducationEmployeeAsync(TaktEmployeeEducation entity, TaktEmployeeEducationCreateDto dto)
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
    /// 构建员工教育经历查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktEmployeeEducation, bool>> QueryExpression(TaktEmployeeEducationQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEmployeeEducation>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.EmployeeCode != null && x.EmployeeCode.Contains(keywords))
                || (x.EmployeeName != null && x.EmployeeName.Contains(keywords))
                || (x.SchoolName != null && x.SchoolName.Contains(keywords))
                || (x.MajorName != null && x.MajorName.Contains(keywords))
                || (x.CertificateCode != null && x.CertificateCode.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.SchoolName))
        {
            var schoolName = queryDto.SchoolName;
            exp = exp.And(x => x.SchoolName != null && x.SchoolName.Contains(schoolName));
        }

        if (queryDto?.EducationLevel.HasValue == true)
        {
            var educationLevel = queryDto.EducationLevel.Value;
            exp = exp.And(x => x.EducationLevel == educationLevel);
        }

        if (queryDto?.DegreeLevel.HasValue == true)
        {
            var degreeLevel = queryDto.DegreeLevel.Value;
            exp = exp.And(x => x.DegreeLevel == degreeLevel);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MajorName))
        {
            var majorName = queryDto.MajorName;
            exp = exp.And(x => x.MajorName != null && x.MajorName.Contains(majorName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CertificateCode))
        {
            var certificateCode = queryDto.CertificateCode;
            exp = exp.And(x => x.CertificateCode != null && x.CertificateCode.Contains(certificateCode));
        }

        if (queryDto?.IsHighest.HasValue == true)
        {
            var isHighest = queryDto.IsHighest.Value;
            exp = exp.And(x => x.IsHighest == isHighest);
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

        if (queryDto?.StartDateStart.HasValue == true)
        {
            var startDateStart = queryDto.StartDateStart.Value;
            exp = exp.And(x => x.StartDate >= startDateStart);
        }

        if (queryDto?.StartDateEnd.HasValue == true)
        {
            var startDateEnd = queryDto.StartDateEnd.Value;
            exp = exp.And(x => x.StartDate <= startDateEnd);
        }

        if (queryDto?.EndDateStart.HasValue == true)
        {
            var endDateStart = queryDto.EndDateStart.Value;
            exp = exp.And(x => x.EndDate >= endDateStart);
        }

        if (queryDto?.EndDateEnd.HasValue == true)
        {
            var endDateEnd = queryDto.EndDateEnd.Value;
            exp = exp.And(x => x.EndDate <= endDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktEmployeeEducationQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.SchoolName))
        {
            return true;
        }
        if (queryDto.EducationLevel.HasValue)
        {
            return true;
        }
        if (queryDto.DegreeLevel.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MajorName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CertificateCode))
        {
            return true;
        }
        if (queryDto.IsHighest.HasValue)
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
        if (queryDto.StartDateStart.HasValue || queryDto.StartDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.EndDateStart.HasValue || queryDto.EndDateEnd.HasValue)
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
