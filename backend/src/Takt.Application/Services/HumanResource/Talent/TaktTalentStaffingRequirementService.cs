// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Talent
// 文件名称：TaktTalentStaffingRequirementService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：用人需求应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.HumanResource.Talent;
using Takt.Domain.Entities.HumanResource.Talent;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.HumanResource.Talent;

/// <summary>
/// 用人需求应用服务
/// </summary>
public class TaktTalentStaffingRequirementService : TaktServiceBase, ITaktTalentStaffingRequirementService
{
    private readonly ITaktApprovalRepository<TaktTalentStaffingRequirement> _talentStaffingRequirementRepository;
    private readonly ITaktCompanyRepository<TaktTalentJobPosting> _talentJobPostingRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="talentStaffingRequirementRepository">用人需求仓储</param>
    /// <param name="talentJobPostingRepository">TalentJobPosting仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktTalentStaffingRequirementService(
        ITaktApprovalRepository<TaktTalentStaffingRequirement> talentStaffingRequirementRepository,
        ITaktCompanyRepository<TaktTalentJobPosting> talentJobPostingRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _talentStaffingRequirementRepository = talentStaffingRequirementRepository;
        _talentJobPostingRepository = talentJobPostingRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取用人需求列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktTalentStaffingRequirementDto>> GetTalentStaffingRequirementListAsync(TaktTalentStaffingRequirementQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktTalentStaffingRequirementDto>.Create(
                new List<TaktTalentStaffingRequirementDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _talentStaffingRequirementRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktTalentStaffingRequirementDto>.Create(
            data.Adapt<List<TaktTalentStaffingRequirementDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取用人需求
    /// </summary>
    /// <param name="id">用人需求ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktTalentStaffingRequirementDto?> GetTalentStaffingRequirementByIdAsync(long id)
    {
        var entity = await _talentStaffingRequirementRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktTalentStaffingRequirementDto>();
        await FillTalentStaffingRequirementDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取用人需求选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetTalentStaffingRequirementOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _talentStaffingRequirementRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.ReqCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.ReqCode,
            DictLabel = e.ReqCode,
        }).ToList();
    }

    /// <summary>
    /// 创建用人需求
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTalentStaffingRequirementDto> CreateTalentStaffingRequirementAsync(TaktTalentStaffingRequirementCreateDto dto)
    {
        var entity = dto.Adapt<TaktTalentStaffingRequirement>();
        var isUnique_ix_talent_staffing_requirement_req_code_unique = await _uniqueValidator.IsUniqueAsync(
            _talentStaffingRequirementRepository,
            x => x.ReqCode == entity.ReqCode);
        if (!isUnique_ix_talent_staffing_requirement_req_code_unique)
        {
            throw new TaktBusinessException("用人需求的ReqCode已存在");
        }
        entity = await _talentStaffingRequirementRepository.CreateAsync(entity);
                await SaveTalentStaffingRequirementChildrenAsync(entity, dto);
        return await GetTalentStaffingRequirementByIdAsync(entity.Id) ?? entity.Adapt<TaktTalentStaffingRequirementDto>();
    }

    /// <summary>
    /// 更新用人需求
    /// </summary>
    /// <param name="id">用人需求ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTalentStaffingRequirementDto> UpdateTalentStaffingRequirementAsync(long id, TaktTalentStaffingRequirementUpdateDto dto)
    {
        var entity = await _talentStaffingRequirementRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("用人需求不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_talent_staffing_requirement_req_code_unique = await _uniqueValidator.IsUniqueAsync(
            _talentStaffingRequirementRepository,
            x => x.ReqCode == entity.ReqCode,
            id);
        if (!isUnique_ix_talent_staffing_requirement_req_code_unique)
        {
            throw new TaktBusinessException("用人需求的ReqCode已存在");
        }
        await _talentStaffingRequirementRepository.UpdateAsync(entity);
                await SaveTalentStaffingRequirementChildrenAsync(entity, dto);
        return await GetTalentStaffingRequirementByIdAsync(id) ?? throw new TaktBusinessException("用人需求不存在");
    }

    /// <summary>
    /// 删除用人需求
    /// </summary>
    /// <param name="id">用人需求ID</param>
    /// <returns>任务</returns>
    public async Task DeleteTalentStaffingRequirementByIdAsync(long id)
    {
        var entity = await _talentStaffingRequirementRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("用人需求不存在或已删除");
        }
        await _talentJobPostingRepository.DeleteAsync(x => x.StaffingRequirementId == entity.Id);
        var deleted = await _talentStaffingRequirementRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("用人需求不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除用人需求
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteTalentStaffingRequirementBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteTalentStaffingRequirementByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetTalentStaffingRequirementTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktTalentStaffingRequirementTemplateDto>(
            sheetName ?? "用人需求导入模板",
            fileName ?? "用人需求导入模板.xlsx");
    }

    /// <summary>
    /// 导入用人需求
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportTalentStaffingRequirementAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktTalentStaffingRequirementImportDto>(fileStream, sheetName ?? "用人需求导入模板");
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
                var entity = rows[i].Adapt<TaktTalentStaffingRequirement>();
                var importKey = $"{entity.ReqCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（ReqCode）");
                }
                var isUnique_ix_talent_staffing_requirement_req_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _talentStaffingRequirementRepository,
                    x => x.ReqCode == entity.ReqCode);
                if (!isUnique_ix_talent_staffing_requirement_req_code_unique)
                {
                    throw new TaktBusinessException("用人需求的ReqCode已存在");
                }
                await _talentStaffingRequirementRepository.CreateAsync(entity);
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
    /// 导出用人需求
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportTalentStaffingRequirementAsync(TaktTalentStaffingRequirementQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktTalentStaffingRequirementQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktTalentStaffingRequirementExportDto>(),
                sheetName ?? "用人需求数据",
                fileName ?? "用人需求导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _talentStaffingRequirementRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktTalentStaffingRequirementExportDto>(),
                sheetName ?? "用人需求数据",
                fileName ?? "用人需求导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktTalentStaffingRequirementExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "用人需求数据",
            fileName ?? "用人需求导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充用人需求详情（加载 OneToMany 子表：职位发布）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillTalentStaffingRequirementDetailsAsync(TaktTalentStaffingRequirementDto dto, TaktTalentStaffingRequirement entity)
    {
        if (dto == null)
        {
            return;
        }
        // 职位发布 → dto.TalentJobPostings
        var talentjobpostings = await _talentJobPostingRepository.GetListAsync(x => x.StaffingRequirementId == entity.Id);
        dto.TalentJobPostings = talentjobpostings.Adapt<List<TaktTalentJobPostingDto>>();
    }

    /// <summary>
    /// 保存用人需求子表级联（职位发布；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveTalentStaffingRequirementChildrenAsync(TaktTalentStaffingRequirement entity, TaktTalentStaffingRequirementCreateDto dto)
    {
        // 职位发布（TalentJobPostings）
        List<TaktTalentJobPostingUpdateDto>? talentJobPostingsForSave;
        if (dto is TaktTalentStaffingRequirementUpdateDto updateDtoForTalentJobPostings && updateDtoForTalentJobPostings.TalentJobPostings != null)
        {
            talentJobPostingsForSave = updateDtoForTalentJobPostings.TalentJobPostings;
        }
        else if (dto.TalentJobPostings != null)
        {
            talentJobPostingsForSave = dto.TalentJobPostings.Adapt<List<TaktTalentJobPostingUpdateDto>>();
        }
        else
        {
            talentJobPostingsForSave = null;
        }
        if (talentJobPostingsForSave is not { Count: > 0 })
        {
            await _talentJobPostingRepository.DeleteAsync(x => x.StaffingRequirementId == entity.Id);
        }
        else
        {
            var existingList = await _talentJobPostingRepository.GetListAsync(x => x.StaffingRequirementId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktTalentJobPosting>();
            for (var i = 0; i < talentJobPostingsForSave.Count; i++)
            {
                var childDto = talentJobPostingsForSave[i];
                childDto.StaffingRequirementId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                if (childDto.TalentJobPostingId > 0)
                {
                    if (!existingById.TryGetValue(childDto.TalentJobPostingId, out var target))
                    {
                        throw new TaktBusinessException("职位发布不存在（TalentJobPostingId={childDto.TalentJobPostingId}）");
                    }
                    if (target.StaffingRequirementId != entity.Id)
                    {
                        throw new TaktBusinessException("职位发布不属于当前主表（TalentJobPostingId={childDto.TalentJobPostingId}）");
                    }
                    submittedIds.Add(childDto.TalentJobPostingId);
                    childDto.Adapt(target);
                    target.Id = childDto.TalentJobPostingId;
                    target.StaffingRequirementId = entity.Id;
                    await _talentJobPostingRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktTalentJobPosting>();
                    child.Id = 0;
                    child.StaffingRequirementId = entity.Id;
                    toCreate.Add(child);
                }
            }
            foreach (var removed in existingList.Where(x => !submittedIds.Contains(x.Id)))
            {
                await _talentJobPostingRepository.DeleteAsync(removed.Id);
            }
            if (toCreate.Count > 0)
            {
                await _talentJobPostingRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建用人需求查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktTalentStaffingRequirement, bool>> QueryExpression(TaktTalentStaffingRequirementQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktTalentStaffingRequirement>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ReqCode != null && x.ReqCode.Contains(keywords))
                || (x.JobGrade != null && x.JobGrade.Contains(keywords))
                || (x.HeadcountType != null && x.HeadcountType.Contains(keywords))
                || (x.ReasonCode != null && x.ReasonCode.Contains(keywords))
                || (x.ContractType != null && x.ContractType.Contains(keywords))
                || (x.WorkLocation != null && x.WorkLocation.Contains(keywords))
                || (x.JobDesc != null && x.JobDesc.Contains(keywords))
                || (x.Qualification != null && x.Qualification.Contains(keywords))
                || (x.BudgetYear != null && x.BudgetYear.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.ReqCode))
        {
            var reqCode = queryDto.ReqCode;
            exp = exp.And(x => x.ReqCode != null && x.ReqCode.Contains(reqCode));
        }

        if (queryDto?.DeptId.HasValue == true)
        {
            var deptId = queryDto.DeptId.Value;
            exp = exp.And(x => x.DeptId == deptId);
        }

        if (queryDto?.PostId.HasValue == true)
        {
            var postId = queryDto.PostId.Value;
            exp = exp.And(x => x.PostId == postId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.JobGrade))
        {
            var jobGrade = queryDto.JobGrade;
            exp = exp.And(x => x.JobGrade != null && x.JobGrade.Contains(jobGrade));
        }

        if (queryDto?.RequestQty.HasValue == true)
        {
            var requestQty = queryDto.RequestQty.Value;
            exp = exp.And(x => x.RequestQty == requestQty);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.HeadcountType))
        {
            var headcountType = queryDto.HeadcountType;
            exp = exp.And(x => x.HeadcountType != null && x.HeadcountType.Contains(headcountType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ReasonCode))
        {
            var reasonCode = queryDto.ReasonCode;
            exp = exp.And(x => x.ReasonCode != null && x.ReasonCode.Contains(reasonCode));
        }

        if (queryDto?.ReplaceEmployeeId.HasValue == true)
        {
            var replaceEmployeeId = queryDto.ReplaceEmployeeId.Value;
            exp = exp.And(x => x.ReplaceEmployeeId == replaceEmployeeId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ContractType))
        {
            var contractType = queryDto.ContractType;
            exp = exp.And(x => x.ContractType != null && x.ContractType.Contains(contractType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.WorkLocation))
        {
            var workLocation = queryDto.WorkLocation;
            exp = exp.And(x => x.WorkLocation != null && x.WorkLocation.Contains(workLocation));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.JobDesc))
        {
            var jobDesc = queryDto.JobDesc;
            exp = exp.And(x => x.JobDesc != null && x.JobDesc.Contains(jobDesc));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Qualification))
        {
            var qualification = queryDto.Qualification;
            exp = exp.And(x => x.Qualification != null && x.Qualification.Contains(qualification));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BudgetYear))
        {
            var budgetYear = queryDto.BudgetYear;
            exp = exp.And(x => x.BudgetYear != null && x.BudgetYear.Contains(budgetYear));
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

        if (queryDto?.ExpectedOnboardDateStart.HasValue == true)
        {
            var expectedOnboardDateStart = queryDto.ExpectedOnboardDateStart.Value;
            exp = exp.And(x => x.ExpectedOnboardDate >= expectedOnboardDateStart);
        }

        if (queryDto?.ExpectedOnboardDateEnd.HasValue == true)
        {
            var expectedOnboardDateEnd = queryDto.ExpectedOnboardDateEnd.Value;
            exp = exp.And(x => x.ExpectedOnboardDate <= expectedOnboardDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktTalentStaffingRequirementQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.ReqCode))
        {
            return true;
        }
        if (queryDto.DeptId.HasValue)
        {
            return true;
        }
        if (queryDto.PostId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.JobGrade))
        {
            return true;
        }
        if (queryDto.RequestQty.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.HeadcountType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ReasonCode))
        {
            return true;
        }
        if (queryDto.ReplaceEmployeeId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ContractType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.WorkLocation))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.JobDesc))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Qualification))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BudgetYear))
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
        if (queryDto.ExpectedOnboardDateStart.HasValue || queryDto.ExpectedOnboardDateEnd.HasValue)
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
