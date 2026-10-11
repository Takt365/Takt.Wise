// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Personnel
// 文件名称：TaktEmployeeReassignmentService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：员工调动应用服务实现
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
/// 员工调动应用服务
/// </summary>
public class TaktEmployeeReassignmentService : TaktServiceBase, ITaktEmployeeReassignmentService
{
    private readonly ITaktApprovalRepository<TaktEmployeeReassignment> _employeeReassignmentRepository;
    private readonly ITaktCompanyRepository<TaktEmployee> _employeeRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="employeeReassignmentRepository">员工调动仓储</param>
    /// <param name="employeeRepository">员工仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEmployeeReassignmentService(
        ITaktApprovalRepository<TaktEmployeeReassignment> employeeReassignmentRepository,
        ITaktCompanyRepository<TaktEmployee> employeeRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _employeeReassignmentRepository = employeeReassignmentRepository;
        _employeeRepository = employeeRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取员工调动列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEmployeeReassignmentDto>> GetEmployeeReassignmentListAsync(TaktEmployeeReassignmentQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktEmployeeReassignmentDto>.Create(
                new List<TaktEmployeeReassignmentDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _employeeReassignmentRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktEmployeeReassignmentDto>.Create(
            data.Adapt<List<TaktEmployeeReassignmentDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取员工调动
    /// </summary>
    /// <param name="id">员工调动ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeReassignmentDto?> GetEmployeeReassignmentByIdAsync(long id)
    {
        var entity = await _employeeReassignmentRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktEmployeeReassignmentDto>();
    }

    /// <summary>
    /// 获取员工调动选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetEmployeeReassignmentOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _employeeReassignmentRepository.GetListAsync(
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
    /// 创建员工调动
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeReassignmentDto> CreateEmployeeReassignmentAsync(TaktEmployeeReassignmentCreateDto dto)
    {
        var entity = dto.Adapt<TaktEmployeeReassignment>();
        await StampEmployeeReassignmentEmployeeAsync(entity, dto);
        entity = await _employeeReassignmentRepository.CreateAsync(entity);
        return await GetEmployeeReassignmentByIdAsync(entity.Id) ?? entity.Adapt<TaktEmployeeReassignmentDto>();
    }

    /// <summary>
    /// 更新员工调动
    /// </summary>
    /// <param name="id">员工调动ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeReassignmentDto> UpdateEmployeeReassignmentAsync(long id, TaktEmployeeReassignmentUpdateDto dto)
    {
        var entity = await _employeeReassignmentRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("员工调动不存在");
        }
        dto.Adapt(entity);
        await StampEmployeeReassignmentEmployeeAsync(entity, dto);
        await _employeeReassignmentRepository.UpdateAsync(entity);
        return await GetEmployeeReassignmentByIdAsync(id) ?? throw new TaktBusinessException("员工调动不存在");
    }

    /// <summary>
    /// 删除员工调动
    /// </summary>
    /// <param name="id">员工调动ID</param>
    /// <returns>任务</returns>
    public async Task DeleteEmployeeReassignmentByIdAsync(long id)
    {
        var deleted = await _employeeReassignmentRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("员工调动不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除员工调动
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteEmployeeReassignmentBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteEmployeeReassignmentByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetEmployeeReassignmentTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktEmployeeReassignmentTemplateDto>(
            sheetName ?? "员工调动导入模板",
            fileName ?? "员工调动导入模板.xlsx");
    }

    /// <summary>
    /// 导入员工调动
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportEmployeeReassignmentAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktEmployeeReassignmentImportDto>(fileStream, sheetName ?? "员工调动导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktEmployeeReassignment>();
                var importDto = rows[i].Adapt<TaktEmployeeReassignmentCreateDto>();
                await StampEmployeeReassignmentEmployeeAsync(entity, importDto);
                await _employeeReassignmentRepository.CreateAsync(entity);
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
    /// 导出员工调动
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEmployeeReassignmentAsync(TaktEmployeeReassignmentQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktEmployeeReassignmentQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEmployeeReassignmentExportDto>(),
                sheetName ?? "员工调动数据",
                fileName ?? "员工调动导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _employeeReassignmentRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEmployeeReassignmentExportDto>(),
                sheetName ?? "员工调动数据",
                fileName ?? "员工调动导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktEmployeeReassignmentExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "员工调动数据",
            fileName ?? "员工调动导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步员工调动主表外键（ManyToOne → 员工）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampEmployeeReassignmentEmployeeAsync(TaktEmployeeReassignment entity, TaktEmployeeReassignmentCreateDto dto)
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
    /// 构建员工调动查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktEmployeeReassignment, bool>> QueryExpression(TaktEmployeeReassignmentQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEmployeeReassignment>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.EmployeeCode != null && x.EmployeeCode.Contains(keywords))
                || (x.EmployeeName != null && x.EmployeeName.Contains(keywords))
                || (x.FromDeptName != null && x.FromDeptName.Contains(keywords))
                || (x.FromPostName != null && x.FromPostName.Contains(keywords))
                || (x.ToDeptName != null && x.ToDeptName.Contains(keywords))
                || (x.ToPostName != null && x.ToPostName.Contains(keywords))
                || (x.Reason != null && x.Reason.Contains(keywords))
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

        if (queryDto?.ReassignmentType.HasValue == true)
        {
            var reassignmentType = queryDto.ReassignmentType.Value;
            exp = exp.And(x => x.ReassignmentType == reassignmentType);
        }

        if (queryDto?.FromDeptId.HasValue == true)
        {
            var fromDeptId = queryDto.FromDeptId.Value;
            exp = exp.And(x => x.FromDeptId == fromDeptId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.FromDeptName))
        {
            var fromDeptName = queryDto.FromDeptName;
            exp = exp.And(x => x.FromDeptName != null && x.FromDeptName.Contains(fromDeptName));
        }

        if (queryDto?.FromPostId.HasValue == true)
        {
            var fromPostId = queryDto.FromPostId.Value;
            exp = exp.And(x => x.FromPostId == fromPostId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.FromPostName))
        {
            var fromPostName = queryDto.FromPostName;
            exp = exp.And(x => x.FromPostName != null && x.FromPostName.Contains(fromPostName));
        }

        if (queryDto?.ToDeptId.HasValue == true)
        {
            var toDeptId = queryDto.ToDeptId.Value;
            exp = exp.And(x => x.ToDeptId == toDeptId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ToDeptName))
        {
            var toDeptName = queryDto.ToDeptName;
            exp = exp.And(x => x.ToDeptName != null && x.ToDeptName.Contains(toDeptName));
        }

        if (queryDto?.ToPostId.HasValue == true)
        {
            var toPostId = queryDto.ToPostId.Value;
            exp = exp.And(x => x.ToPostId == toPostId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ToPostName))
        {
            var toPostName = queryDto.ToPostName;
            exp = exp.And(x => x.ToPostName != null && x.ToPostName.Contains(toPostName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Reason))
        {
            var reason = queryDto.Reason;
            exp = exp.And(x => x.Reason != null && x.Reason.Contains(reason));
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

        if (queryDto?.EffectiveDateStart.HasValue == true)
        {
            var effectiveDateStart = queryDto.EffectiveDateStart.Value;
            exp = exp.And(x => x.EffectiveDate >= effectiveDateStart);
        }

        if (queryDto?.EffectiveDateEnd.HasValue == true)
        {
            var effectiveDateEnd = queryDto.EffectiveDateEnd.Value;
            exp = exp.And(x => x.EffectiveDate <= effectiveDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktEmployeeReassignmentQueryDto? queryDto)
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
        if (queryDto.ReassignmentType.HasValue)
        {
            return true;
        }
        if (queryDto.FromDeptId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FromDeptName))
        {
            return true;
        }
        if (queryDto.FromPostId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FromPostName))
        {
            return true;
        }
        if (queryDto.ToDeptId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ToDeptName))
        {
            return true;
        }
        if (queryDto.ToPostId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ToPostName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Reason))
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
        if (queryDto.EffectiveDateStart.HasValue || queryDto.EffectiveDateEnd.HasValue)
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
