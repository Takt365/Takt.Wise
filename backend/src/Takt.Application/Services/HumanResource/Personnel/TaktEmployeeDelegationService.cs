// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Personnel
// 文件名称：TaktEmployeeDelegationService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：员工代理关系应用服务实现
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
/// 员工代理关系应用服务
/// </summary>
public class TaktEmployeeDelegationService : TaktServiceBase, ITaktEmployeeDelegationService
{
    private readonly ITaktCompanyRepository<TaktEmployeeDelegation> _employeeDelegationRepository;
    private readonly ITaktCompanyRepository<TaktEmployee> _employeeRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="employeeDelegationRepository">员工代理关系仓储</param>
    /// <param name="employeeRepository">员工仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEmployeeDelegationService(
        ITaktCompanyRepository<TaktEmployeeDelegation> employeeDelegationRepository,
        ITaktCompanyRepository<TaktEmployee> employeeRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _employeeDelegationRepository = employeeDelegationRepository;
        _employeeRepository = employeeRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取员工代理关系列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEmployeeDelegationDto>> GetEmployeeDelegationListAsync(TaktEmployeeDelegationQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktEmployeeDelegationDto>.Create(
                new List<TaktEmployeeDelegationDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _employeeDelegationRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktEmployeeDelegationDto>.Create(
            data.Adapt<List<TaktEmployeeDelegationDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取员工代理关系
    /// </summary>
    /// <param name="id">员工代理关系ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeDelegationDto?> GetEmployeeDelegationByIdAsync(long id)
    {
        var entity = await _employeeDelegationRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktEmployeeDelegationDto>();
    }

    /// <summary>
    /// 获取员工代理关系选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetEmployeeDelegationOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _employeeDelegationRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.ProxyEmployeeName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.ProxyEmployeeCode,
            DictLabel = e.ProxyEmployeeName ?? e.ProxyEmployeeCode,
        }).ToList();
    }

    /// <summary>
    /// 创建员工代理关系
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeDelegationDto> CreateEmployeeDelegationAsync(TaktEmployeeDelegationCreateDto dto)
    {
        var entity = dto.Adapt<TaktEmployeeDelegation>();
        await StampEmployeeDelegationEmployeeAsync(entity, dto);
        var isUnique_ix_employee_delegation_unique = await _uniqueValidator.IsUniqueAsync(
            _employeeDelegationRepository,
            x => x.OriginalEmployeeId == entity.OriginalEmployeeId
                && x.ProxyEmployeeId == entity.ProxyEmployeeId
                && x.DelegationType == entity.DelegationType
                && x.StartDate == entity.StartDate);
        if (!isUnique_ix_employee_delegation_unique)
        {
            throw new TaktBusinessException("员工代理关系的OriginalEmployeeId、ProxyEmployeeId、DelegationType、StartDate已存在");
        }
        entity = await _employeeDelegationRepository.CreateAsync(entity);
        return await GetEmployeeDelegationByIdAsync(entity.Id) ?? entity.Adapt<TaktEmployeeDelegationDto>();
    }

    /// <summary>
    /// 更新员工代理关系
    /// </summary>
    /// <param name="id">员工代理关系ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeDelegationDto> UpdateEmployeeDelegationAsync(long id, TaktEmployeeDelegationUpdateDto dto)
    {
        var entity = await _employeeDelegationRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("员工代理关系不存在");
        }
        dto.Adapt(entity);
        await StampEmployeeDelegationEmployeeAsync(entity, dto);
        var isUnique_ix_employee_delegation_unique = await _uniqueValidator.IsUniqueAsync(
            _employeeDelegationRepository,
            x => x.OriginalEmployeeId == entity.OriginalEmployeeId
                && x.ProxyEmployeeId == entity.ProxyEmployeeId
                && x.DelegationType == entity.DelegationType
                && x.StartDate == entity.StartDate,
            id);
        if (!isUnique_ix_employee_delegation_unique)
        {
            throw new TaktBusinessException("员工代理关系的OriginalEmployeeId、ProxyEmployeeId、DelegationType、StartDate已存在");
        }
        await _employeeDelegationRepository.UpdateAsync(entity);
        return await GetEmployeeDelegationByIdAsync(id) ?? throw new TaktBusinessException("员工代理关系不存在");
    }

    /// <summary>
    /// 删除员工代理关系
    /// </summary>
    /// <param name="id">员工代理关系ID</param>
    /// <returns>任务</returns>
    public async Task DeleteEmployeeDelegationByIdAsync(long id)
    {
        var deleted = await _employeeDelegationRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("员工代理关系不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除员工代理关系
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteEmployeeDelegationBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteEmployeeDelegationByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetEmployeeDelegationTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktEmployeeDelegationTemplateDto>(
            sheetName ?? "员工代理关系导入模板",
            fileName ?? "员工代理关系导入模板.xlsx");
    }

    /// <summary>
    /// 导入员工代理关系
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportEmployeeDelegationAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktEmployeeDelegationImportDto>(fileStream, sheetName ?? "员工代理关系导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktEmployeeDelegation>();
                var importDto = rows[i].Adapt<TaktEmployeeDelegationCreateDto>();
                await StampEmployeeDelegationEmployeeAsync(entity, importDto);
                var importKey = $"{entity.OriginalEmployeeId}|{entity.ProxyEmployeeId}|{entity.DelegationType}|{entity.StartDate}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（OriginalEmployeeId、ProxyEmployeeId、DelegationType、StartDate）");
                }
                var isUnique_ix_employee_delegation_unique = await _uniqueValidator.IsUniqueAsync(
                    _employeeDelegationRepository,
                    x => x.OriginalEmployeeId == entity.OriginalEmployeeId
                        && x.ProxyEmployeeId == entity.ProxyEmployeeId
                        && x.DelegationType == entity.DelegationType
                        && x.StartDate == entity.StartDate);
                if (!isUnique_ix_employee_delegation_unique)
                {
                    throw new TaktBusinessException("员工代理关系的OriginalEmployeeId、ProxyEmployeeId、DelegationType、StartDate已存在");
                }
                await _employeeDelegationRepository.CreateAsync(entity);
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
    /// 导出员工代理关系
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEmployeeDelegationAsync(TaktEmployeeDelegationQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktEmployeeDelegationQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEmployeeDelegationExportDto>(),
                sheetName ?? "员工代理关系数据",
                fileName ?? "员工代理关系导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _employeeDelegationRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEmployeeDelegationExportDto>(),
                sheetName ?? "员工代理关系数据",
                fileName ?? "员工代理关系导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktEmployeeDelegationExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "员工代理关系数据",
            fileName ?? "员工代理关系导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步员工代理关系主表外键（ManyToOne → 员工）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampEmployeeDelegationEmployeeAsync(TaktEmployeeDelegation entity, TaktEmployeeDelegationCreateDto dto)
    {
        if (dto.OriginalEmployeeId <= 0)
        {
            return;
        }
        var master = await _employeeRepository.GetByIdAsync(dto.OriginalEmployeeId);
        if (master == null)
        {
            throw new TaktBusinessException("员工不存在");
        }
        entity.OriginalEmployeeId = master.Id;
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
        if (string.IsNullOrEmpty(entity.OriginalEmployeeCode))
        {
            entity.OriginalEmployeeCode = master.EmployeeCode;
        }
        if (string.IsNullOrEmpty(entity.OriginalEmployeeName))
        {
            entity.OriginalEmployeeName = master.EmployeeName;
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建员工代理关系查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktEmployeeDelegation, bool>> QueryExpression(TaktEmployeeDelegationQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEmployeeDelegation>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ProxyEmployeeCode != null && x.ProxyEmployeeCode.Contains(keywords))
                || (x.ProxyEmployeeName != null && x.ProxyEmployeeName.Contains(keywords))
                || (x.OriginalEmployeeCode != null && x.OriginalEmployeeCode.Contains(keywords))
                || (x.OriginalEmployeeName != null && x.OriginalEmployeeName.Contains(keywords))
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

        if (queryDto?.ProxyEmployeeId.HasValue == true)
        {
            var proxyEmployeeId = queryDto.ProxyEmployeeId.Value;
            exp = exp.And(x => x.ProxyEmployeeId == proxyEmployeeId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProxyEmployeeCode))
        {
            var proxyEmployeeCode = queryDto.ProxyEmployeeCode;
            exp = exp.And(x => x.ProxyEmployeeCode != null && x.ProxyEmployeeCode.Contains(proxyEmployeeCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProxyEmployeeName))
        {
            var proxyEmployeeName = queryDto.ProxyEmployeeName;
            exp = exp.And(x => x.ProxyEmployeeName != null && x.ProxyEmployeeName.Contains(proxyEmployeeName));
        }

        if (queryDto?.OriginalEmployeeId.HasValue == true)
        {
            var originalEmployeeId = queryDto.OriginalEmployeeId.Value;
            exp = exp.And(x => x.OriginalEmployeeId == originalEmployeeId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.OriginalEmployeeCode))
        {
            var originalEmployeeCode = queryDto.OriginalEmployeeCode;
            exp = exp.And(x => x.OriginalEmployeeCode != null && x.OriginalEmployeeCode.Contains(originalEmployeeCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.OriginalEmployeeName))
        {
            var originalEmployeeName = queryDto.OriginalEmployeeName;
            exp = exp.And(x => x.OriginalEmployeeName != null && x.OriginalEmployeeName.Contains(originalEmployeeName));
        }

        if (queryDto?.DelegationType.HasValue == true)
        {
            var delegationType = queryDto.DelegationType.Value;
            exp = exp.And(x => x.DelegationType == delegationType);
        }

        if (queryDto?.ScopeType.HasValue == true)
        {
            var scopeType = queryDto.ScopeType.Value;
            exp = exp.And(x => x.ScopeType == scopeType);
        }

        if (queryDto?.ScopeId.HasValue == true)
        {
            var scopeId = queryDto.ScopeId.Value;
            exp = exp.And(x => x.ScopeId == scopeId);
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
    private static bool HasAnyListQueryFilter(TaktEmployeeDelegationQueryDto? queryDto)
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
        if (queryDto.ProxyEmployeeId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProxyEmployeeCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProxyEmployeeName))
        {
            return true;
        }
        if (queryDto.OriginalEmployeeId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.OriginalEmployeeCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.OriginalEmployeeName))
        {
            return true;
        }
        if (queryDto.DelegationType.HasValue)
        {
            return true;
        }
        if (queryDto.ScopeType.HasValue)
        {
            return true;
        }
        if (queryDto.ScopeId.HasValue)
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
