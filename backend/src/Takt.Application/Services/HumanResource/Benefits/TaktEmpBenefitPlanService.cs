// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Benefits
// 文件名称：TaktEmpBenefitPlanService.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Cursor AI)
// 功能描述：员工福利方案应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.HumanResource.Benefits;
using Takt.Domain.Entities.HumanResource.Benefits;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.HumanResource.Benefits;

/// <summary>
/// 员工福利方案应用服务
/// </summary>
public class TaktEmpBenefitPlanService : TaktServiceBase, ITaktEmpBenefitPlanService
{
    private readonly ITaktCompanyRepository<TaktEmpBenefitPlan> _empBenefitPlanRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="empBenefitPlanRepository">员工福利方案仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEmpBenefitPlanService(
        ITaktCompanyRepository<TaktEmpBenefitPlan> empBenefitPlanRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _empBenefitPlanRepository = empBenefitPlanRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取员工福利方案列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEmpBenefitPlanDto>> GetEmpBenefitPlanListAsync(TaktEmpBenefitPlanQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktEmpBenefitPlanDto>.Create(
                new List<TaktEmpBenefitPlanDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _empBenefitPlanRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktEmpBenefitPlanDto>.Create(
            data.Adapt<List<TaktEmpBenefitPlanDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取员工福利方案
    /// </summary>
    /// <param name="id">员工福利方案ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmpBenefitPlanDto?> GetEmpBenefitPlanByIdAsync(long id)
    {
        var entity = await _empBenefitPlanRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktEmpBenefitPlanDto>();
    }

    /// <summary>
    /// 获取员工福利方案选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetEmpBenefitPlanOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _empBenefitPlanRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.EmpBenefitStatus == 1,
            x => x.EmployeeName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.PlanCode,
            DictLabel = e.EmployeeName ?? e.PlanCode,
        }).ToList();
    }

    /// <summary>
    /// 创建员工福利方案
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmpBenefitPlanDto> CreateEmpBenefitPlanAsync(TaktEmpBenefitPlanCreateDto dto)
    {
        var entity = dto.Adapt<TaktEmpBenefitPlan>();
        entity = await _empBenefitPlanRepository.CreateAsync(entity);
        return await GetEmpBenefitPlanByIdAsync(entity.Id) ?? entity.Adapt<TaktEmpBenefitPlanDto>();
    }

    /// <summary>
    /// 更新员工福利方案
    /// </summary>
    /// <param name="id">员工福利方案ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmpBenefitPlanDto> UpdateEmpBenefitPlanAsync(long id, TaktEmpBenefitPlanUpdateDto dto)
    {
        var entity = await _empBenefitPlanRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("员工福利方案不存在");
        }
        dto.Adapt(entity);
        await _empBenefitPlanRepository.UpdateAsync(entity);
        return await GetEmpBenefitPlanByIdAsync(id) ?? throw new TaktBusinessException("员工福利方案不存在");
    }

    /// <summary>
    /// 删除员工福利方案
    /// </summary>
    /// <param name="id">员工福利方案ID</param>
    /// <returns>任务</returns>
    public async Task DeleteEmpBenefitPlanByIdAsync(long id)
    {
        var deleted = await _empBenefitPlanRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("员工福利方案不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除员工福利方案
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteEmpBenefitPlanBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteEmpBenefitPlanByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新员工福利方案状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmpBenefitPlanDto> UpdateEmpBenefitPlanStatusAsync(TaktEmpBenefitPlanStatusDto dto)
    {
        var entity = await _empBenefitPlanRepository.GetByIdAsync(dto.EmpBenefitPlanId);
        if (entity == null)
        {
            throw new TaktBusinessException("员工福利方案不存在");
        }
        entity.EmpBenefitStatus = dto.EmpBenefitStatus;
        await _empBenefitPlanRepository.UpdateAsync(entity);
        return await GetEmpBenefitPlanByIdAsync(dto.EmpBenefitPlanId) ?? throw new TaktBusinessException("员工福利方案不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetEmpBenefitPlanTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktEmpBenefitPlanTemplateDto>(
            sheetName ?? "员工福利方案导入模板",
            fileName ?? "员工福利方案导入模板.xlsx");
    }

    /// <summary>
    /// 导入员工福利方案
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportEmpBenefitPlanAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktEmpBenefitPlanImportDto>(fileStream, sheetName ?? "员工福利方案导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktEmpBenefitPlan>();
                await _empBenefitPlanRepository.CreateAsync(entity);
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
    /// 导出员工福利方案
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEmpBenefitPlanAsync(TaktEmpBenefitPlanQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktEmpBenefitPlanQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEmpBenefitPlanExportDto>(),
                sheetName ?? "员工福利方案数据",
                fileName ?? "员工福利方案导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _empBenefitPlanRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEmpBenefitPlanExportDto>(),
                sheetName ?? "员工福利方案数据",
                fileName ?? "员工福利方案导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktEmpBenefitPlanExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "员工福利方案数据",
            fileName ?? "员工福利方案导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建员工福利方案查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktEmpBenefitPlan, bool>> QueryExpression(TaktEmpBenefitPlanQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEmpBenefitPlan>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.EmployeeName != null && x.EmployeeName.Contains(keywords))
                || (x.PlanCode != null && x.PlanCode.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.EmployeeName))
        {
            var employeeName = queryDto.EmployeeName;
            exp = exp.And(x => x.EmployeeName != null && x.EmployeeName.Contains(employeeName));
        }

        if (queryDto?.BenefitItemId.HasValue == true)
        {
            var benefitItemId = queryDto.BenefitItemId.Value;
            exp = exp.And(x => x.BenefitItemId == benefitItemId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PlanCode))
        {
            var planCode = queryDto.PlanCode;
            exp = exp.And(x => x.PlanCode != null && x.PlanCode.Contains(planCode));
        }

        if (queryDto?.EmpBenefitStatus.HasValue == true)
        {
            var empBenefitStatus = queryDto.EmpBenefitStatus.Value;
            exp = exp.And(x => x.EmpBenefitStatus == empBenefitStatus);
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

        if (queryDto?.EnrollmentDateStart.HasValue == true)
        {
            var enrollmentDateStart = queryDto.EnrollmentDateStart.Value;
            exp = exp.And(x => x.EnrollmentDate >= enrollmentDateStart);
        }

        if (queryDto?.EnrollmentDateEnd.HasValue == true)
        {
            var enrollmentDateEnd = queryDto.EnrollmentDateEnd.Value;
            exp = exp.And(x => x.EnrollmentDate <= enrollmentDateEnd);
        }

        if (queryDto?.ExpiryDateStart.HasValue == true)
        {
            var expiryDateStart = queryDto.ExpiryDateStart.Value;
            exp = exp.And(x => x.ExpiryDate >= expiryDateStart);
        }

        if (queryDto?.ExpiryDateEnd.HasValue == true)
        {
            var expiryDateEnd = queryDto.ExpiryDateEnd.Value;
            exp = exp.And(x => x.ExpiryDate <= expiryDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktEmpBenefitPlanQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.EmployeeName))
        {
            return true;
        }
        if (queryDto.BenefitItemId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PlanCode))
        {
            return true;
        }
        if (queryDto.EmpBenefitStatus.HasValue)
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
        if (queryDto.EnrollmentDateStart.HasValue || queryDto.EnrollmentDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.ExpiryDateStart.HasValue || queryDto.ExpiryDateEnd.HasValue)
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
