// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Personnel
// 文件名称：TaktEmployeeOnboardingService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：入职待办应用服务实现
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
using Takt.Domain.Entities.HumanResource.Talent;

namespace Takt.Application.Services.HumanResource.Personnel;

/// <summary>
/// 入职待办应用服务
/// </summary>
public class TaktEmployeeOnboardingService : TaktServiceBase, ITaktEmployeeOnboardingService
{
    private readonly ITaktCompanyRepository<TaktEmployeeOnboarding> _employeeOnboardingRepository;
    private readonly ITaktCompanyRepository<TaktEmployee> _employeeRepository;
    private readonly ITaktApprovalRepository<TaktTalentOffer> _talentOfferRepository;
    private readonly ITaktApprovalRepository<TaktEmployeeJoined> _employeeJoinedRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="employeeOnboardingRepository">入职待办仓储</param>
    /// <param name="employeeRepository">员工仓储</param>
    /// <param name="talentOfferRepository">录用信息仓储</param>
    /// <param name="employeeJoinedRepository">员工入职上岗仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEmployeeOnboardingService(
        ITaktCompanyRepository<TaktEmployeeOnboarding> employeeOnboardingRepository,
        ITaktCompanyRepository<TaktEmployee> employeeRepository,
        ITaktApprovalRepository<TaktTalentOffer> talentOfferRepository,
        ITaktApprovalRepository<TaktEmployeeJoined> employeeJoinedRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _employeeOnboardingRepository = employeeOnboardingRepository;
        _employeeRepository = employeeRepository;
        _talentOfferRepository = talentOfferRepository;
        _employeeJoinedRepository = employeeJoinedRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取入职待办列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEmployeeOnboardingDto>> GetEmployeeOnboardingListAsync(TaktEmployeeOnboardingQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktEmployeeOnboardingDto>.Create(
                new List<TaktEmployeeOnboardingDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _employeeOnboardingRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktEmployeeOnboardingDto>.Create(
            data.Adapt<List<TaktEmployeeOnboardingDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取入职待办
    /// </summary>
    /// <param name="id">入职待办ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeOnboardingDto?> GetEmployeeOnboardingByIdAsync(long id)
    {
        var entity = await _employeeOnboardingRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktEmployeeOnboardingDto>();
    }

    /// <summary>
    /// 获取入职待办选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetEmployeeOnboardingOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _employeeOnboardingRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.TodoStatus == 1,
            x => x.CandidateName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.EmployeeCode ?? string.Empty,
            DictLabel = e.CandidateName ?? e.EmployeeCode ?? string.Empty,
        }).ToList();
    }

    /// <summary>
    /// 创建入职待办
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeOnboardingDto> CreateEmployeeOnboardingAsync(TaktEmployeeOnboardingCreateDto dto)
    {
        var entity = dto.Adapt<TaktEmployeeOnboarding>();
        await StampEmployeeOnboardingEmployeeAsync(entity, dto);
        await StampEmployeeOnboardingTalentOfferAsync(entity, dto);
        await StampEmployeeOnboardingEmployeeJoinedAsync(entity, dto);
        entity = await _employeeOnboardingRepository.CreateAsync(entity);
        return await GetEmployeeOnboardingByIdAsync(entity.Id) ?? entity.Adapt<TaktEmployeeOnboardingDto>();
    }

    /// <summary>
    /// 更新入职待办
    /// </summary>
    /// <param name="id">入职待办ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeOnboardingDto> UpdateEmployeeOnboardingAsync(long id, TaktEmployeeOnboardingUpdateDto dto)
    {
        var entity = await _employeeOnboardingRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("入职待办不存在");
        }
        dto.Adapt(entity);
        await StampEmployeeOnboardingEmployeeAsync(entity, dto);
        await StampEmployeeOnboardingTalentOfferAsync(entity, dto);
        await StampEmployeeOnboardingEmployeeJoinedAsync(entity, dto);
        await _employeeOnboardingRepository.UpdateAsync(entity);
        return await GetEmployeeOnboardingByIdAsync(id) ?? throw new TaktBusinessException("入职待办不存在");
    }

    /// <summary>
    /// 删除入职待办
    /// </summary>
    /// <param name="id">入职待办ID</param>
    /// <returns>任务</returns>
    public async Task DeleteEmployeeOnboardingByIdAsync(long id)
    {
        var deleted = await _employeeOnboardingRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("入职待办不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除入职待办
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteEmployeeOnboardingBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteEmployeeOnboardingByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新入职待办状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeOnboardingDto> UpdateEmployeeOnboardingStatusAsync(TaktEmployeeOnboardingStatusDto dto)
    {
        var entity = await _employeeOnboardingRepository.GetByIdAsync(dto.EmployeeOnboardingId);
        if (entity == null)
        {
            throw new TaktBusinessException("入职待办不存在");
        }
        entity.TodoStatus = dto.TodoStatus;
        await _employeeOnboardingRepository.UpdateAsync(entity);
        return await GetEmployeeOnboardingByIdAsync(dto.EmployeeOnboardingId) ?? throw new TaktBusinessException("入职待办不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetEmployeeOnboardingTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktEmployeeOnboardingTemplateDto>(
            sheetName ?? "入职待办导入模板",
            fileName ?? "入职待办导入模板.xlsx");
    }

    /// <summary>
    /// 导入入职待办
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportEmployeeOnboardingAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktEmployeeOnboardingImportDto>(fileStream, sheetName ?? "入职待办导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktEmployeeOnboarding>();
                var importDto = rows[i].Adapt<TaktEmployeeOnboardingCreateDto>();
                await StampEmployeeOnboardingEmployeeAsync(entity, importDto);
                await StampEmployeeOnboardingTalentOfferAsync(entity, importDto);
                await StampEmployeeOnboardingEmployeeJoinedAsync(entity, importDto);
                await _employeeOnboardingRepository.CreateAsync(entity);
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
    /// 导出入职待办
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEmployeeOnboardingAsync(TaktEmployeeOnboardingQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktEmployeeOnboardingQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEmployeeOnboardingExportDto>(),
                sheetName ?? "入职待办数据",
                fileName ?? "入职待办导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _employeeOnboardingRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEmployeeOnboardingExportDto>(),
                sheetName ?? "入职待办数据",
                fileName ?? "入职待办导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktEmployeeOnboardingExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "入职待办数据",
            fileName ?? "入职待办导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步入职待办主表外键（ManyToOne → 员工）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampEmployeeOnboardingEmployeeAsync(TaktEmployeeOnboarding entity, TaktEmployeeOnboardingCreateDto dto)
    {
        if (dto.EmployeeId is not > 0)
        {
            return;
        }
        var master = await _employeeRepository.GetByIdAsync(dto.EmployeeId.Value);
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
        if (string.IsNullOrEmpty(entity.Mobile))
        {
            entity.Mobile = master.Mobile;
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

    /// <summary>
    /// 同步入职待办主表外键（ManyToOne → 录用信息）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampEmployeeOnboardingTalentOfferAsync(TaktEmployeeOnboarding entity, TaktEmployeeOnboardingCreateDto dto)
    {
        if (dto.OfferId <= 0)
        {
            return;
        }
        var master = await _talentOfferRepository.GetByIdAsync(dto.OfferId);
        if (master == null)
        {
            throw new TaktBusinessException("录用信息不存在");
        }
        entity.OfferId = master.Id;
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
        if (string.IsNullOrEmpty(entity.Reason))
        {
            entity.Reason = master.Reason;
        }
    }

    /// <summary>
    /// 同步入职待办主表外键（ManyToOne → 员工入职上岗）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampEmployeeOnboardingEmployeeJoinedAsync(TaktEmployeeOnboarding entity, TaktEmployeeOnboardingCreateDto dto)
    {
        if (dto.EmployeeJoinedId is not > 0)
        {
            return;
        }
        var master = await _employeeJoinedRepository.GetByIdAsync(dto.EmployeeJoinedId.Value);
        if (master == null)
        {
            throw new TaktBusinessException("员工入职上岗不存在");
        }
        entity.EmployeeJoinedId = master.Id;
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
    /// 构建入职待办查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktEmployeeOnboarding, bool>> QueryExpression(TaktEmployeeOnboardingQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEmployeeOnboarding>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.TodoCode != null && x.TodoCode.Contains(keywords))
                || (x.CandidateName != null && x.CandidateName.Contains(keywords))
                || (x.Mobile != null && x.Mobile.Contains(keywords))
                || (x.EmployeeCode != null && x.EmployeeCode.Contains(keywords))
                || (x.EmployeeName != null && x.EmployeeName.Contains(keywords))
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

        if (queryDto?.OfferId.HasValue == true)
        {
            var offerId = queryDto.OfferId.Value;
            exp = exp.And(x => x.OfferId == offerId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TodoCode))
        {
            var todoCode = queryDto.TodoCode;
            exp = exp.And(x => x.TodoCode != null && x.TodoCode.Contains(todoCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CandidateName))
        {
            var candidateName = queryDto.CandidateName;
            exp = exp.And(x => x.CandidateName != null && x.CandidateName.Contains(candidateName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Mobile))
        {
            var mobile = queryDto.Mobile;
            exp = exp.And(x => x.Mobile != null && x.Mobile.Contains(mobile));
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

        if (queryDto?.EmployeeJoinedId.HasValue == true)
        {
            var employeeJoinedId = queryDto.EmployeeJoinedId.Value;
            exp = exp.And(x => x.EmployeeJoinedId == employeeJoinedId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Reason))
        {
            var reason = queryDto.Reason;
            exp = exp.And(x => x.Reason != null && x.Reason.Contains(reason));
        }

        if (queryDto?.TodoStatus.HasValue == true)
        {
            var todoStatus = queryDto.TodoStatus.Value;
            exp = exp.And(x => x.TodoStatus == todoStatus);
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

        if (queryDto?.PlannedJoinedDateStart.HasValue == true)
        {
            var plannedJoinedDateStart = queryDto.PlannedJoinedDateStart.Value;
            exp = exp.And(x => x.PlannedJoinedDate >= plannedJoinedDateStart);
        }

        if (queryDto?.PlannedJoinedDateEnd.HasValue == true)
        {
            var plannedJoinedDateEnd = queryDto.PlannedJoinedDateEnd.Value;
            exp = exp.And(x => x.PlannedJoinedDate <= plannedJoinedDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktEmployeeOnboardingQueryDto? queryDto)
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
        if (queryDto.OfferId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TodoCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CandidateName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Mobile))
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
        if (queryDto.EmployeeJoinedId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Reason))
        {
            return true;
        }
        if (queryDto.TodoStatus.HasValue)
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
        if (queryDto.PlannedJoinedDateStart.HasValue || queryDto.PlannedJoinedDateEnd.HasValue)
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
