// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Talent
// 文件名称：TaktTalentOfferService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：录用信息应用服务实现
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
using Takt.Application.Dtos.HumanResource.Personnel;
using Takt.Domain.Entities.HumanResource.Personnel;

namespace Takt.Application.Services.HumanResource.Talent;

/// <summary>
/// 录用信息应用服务
/// </summary>
public class TaktTalentOfferService : TaktServiceBase, ITaktTalentOfferService
{
    private readonly ITaktApprovalRepository<TaktTalentOffer> _talentOfferRepository;
    private readonly ITaktCompanyRepository<TaktEmployeeOnboarding> _employeeOnboardingRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="talentOfferRepository">录用信息仓储</param>
    /// <param name="employeeOnboardingRepository">EmployeeOnboarding仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktTalentOfferService(
        ITaktApprovalRepository<TaktTalentOffer> talentOfferRepository,
        ITaktCompanyRepository<TaktEmployeeOnboarding> employeeOnboardingRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _talentOfferRepository = talentOfferRepository;
        _employeeOnboardingRepository = employeeOnboardingRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取录用信息列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktTalentOfferDto>> GetTalentOfferListAsync(TaktTalentOfferQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktTalentOfferDto>.Create(
                new List<TaktTalentOfferDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _talentOfferRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktTalentOfferDto>.Create(
            data.Adapt<List<TaktTalentOfferDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取录用信息
    /// </summary>
    /// <param name="id">录用信息ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktTalentOfferDto?> GetTalentOfferByIdAsync(long id)
    {
        var entity = await _talentOfferRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktTalentOfferDto>();
        await FillTalentOfferDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取录用信息选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetTalentOfferOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _talentOfferRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.DeptName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.OfferCode,
            DictLabel = e.DeptName ?? e.OfferCode,
        }).ToList();
    }

    /// <summary>
    /// 创建录用信息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTalentOfferDto> CreateTalentOfferAsync(TaktTalentOfferCreateDto dto)
    {
        var entity = dto.Adapt<TaktTalentOffer>();
        entity = await _talentOfferRepository.CreateAsync(entity);
                await SaveTalentOfferChildrenAsync(entity, dto);
        return await GetTalentOfferByIdAsync(entity.Id) ?? entity.Adapt<TaktTalentOfferDto>();
    }

    /// <summary>
    /// 更新录用信息
    /// </summary>
    /// <param name="id">录用信息ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTalentOfferDto> UpdateTalentOfferAsync(long id, TaktTalentOfferUpdateDto dto)
    {
        var entity = await _talentOfferRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("录用信息不存在");
        }
        dto.Adapt(entity);
        await _talentOfferRepository.UpdateAsync(entity);
                await SaveTalentOfferChildrenAsync(entity, dto);
        return await GetTalentOfferByIdAsync(id) ?? throw new TaktBusinessException("录用信息不存在");
    }

    /// <summary>
    /// 删除录用信息
    /// </summary>
    /// <param name="id">录用信息ID</param>
    /// <returns>任务</returns>
    public async Task DeleteTalentOfferByIdAsync(long id)
    {
        var entity = await _talentOfferRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("录用信息不存在或已删除");
        }
        await _employeeOnboardingRepository.DeleteAsync(x => x.OfferId == entity.Id);
        var deleted = await _talentOfferRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("录用信息不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除录用信息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteTalentOfferBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteTalentOfferByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetTalentOfferTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktTalentOfferTemplateDto>(
            sheetName ?? "录用信息导入模板",
            fileName ?? "录用信息导入模板.xlsx");
    }

    /// <summary>
    /// 导入录用信息
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportTalentOfferAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktTalentOfferImportDto>(fileStream, sheetName ?? "录用信息导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktTalentOffer>();
                await _talentOfferRepository.CreateAsync(entity);
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
    /// 导出录用信息
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportTalentOfferAsync(TaktTalentOfferQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktTalentOfferQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktTalentOfferExportDto>(),
                sheetName ?? "录用信息数据",
                fileName ?? "录用信息导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _talentOfferRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktTalentOfferExportDto>(),
                sheetName ?? "录用信息数据",
                fileName ?? "录用信息导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktTalentOfferExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "录用信息数据",
            fileName ?? "录用信息导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充录用信息详情（加载 OneToMany 子表：入职待办）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillTalentOfferDetailsAsync(TaktTalentOfferDto dto, TaktTalentOffer entity)
    {
        if (dto == null)
        {
            return;
        }
        // 入职待办 → dto.EmployeeOnboardings
        var employeeonboardings = await _employeeOnboardingRepository.GetListAsync(x => x.OfferId == entity.Id);
        dto.EmployeeOnboardings = employeeonboardings.Adapt<List<TaktEmployeeOnboardingDto>>();
    }

    /// <summary>
    /// 保存录用信息子表级联（入职待办；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveTalentOfferChildrenAsync(TaktTalentOffer entity, TaktTalentOfferCreateDto dto)
    {
        // 入职待办（EmployeeOnboardings）
        List<TaktEmployeeOnboardingUpdateDto>? employeeOnboardingsForSave;
        if (dto is TaktTalentOfferUpdateDto updateDtoForEmployeeOnboardings && updateDtoForEmployeeOnboardings.EmployeeOnboardings != null)
        {
            employeeOnboardingsForSave = updateDtoForEmployeeOnboardings.EmployeeOnboardings;
        }
        else if (dto.EmployeeOnboardings != null)
        {
            employeeOnboardingsForSave = dto.EmployeeOnboardings.Adapt<List<TaktEmployeeOnboardingUpdateDto>>();
        }
        else
        {
            employeeOnboardingsForSave = null;
        }
        if (employeeOnboardingsForSave is not { Count: > 0 })
        {
            await _employeeOnboardingRepository.DeleteAsync(x => x.OfferId == entity.Id);
        }
        else
        {
            var existingList = await _employeeOnboardingRepository.GetListAsync(x => x.OfferId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktEmployeeOnboarding>();
            for (var i = 0; i < employeeOnboardingsForSave.Count; i++)
            {
                var childDto = employeeOnboardingsForSave[i];
                childDto.OfferId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.Reason = entity.Reason;
                if (childDto.EmployeeOnboardingId > 0)
                {
                    if (!existingById.TryGetValue(childDto.EmployeeOnboardingId, out var target))
                    {
                        throw new TaktBusinessException("入职待办不存在（EmployeeOnboardingId={childDto.EmployeeOnboardingId}）");
                    }
                    if (target.OfferId != entity.Id)
                    {
                        throw new TaktBusinessException("入职待办不属于当前主表（EmployeeOnboardingId={childDto.EmployeeOnboardingId}）");
                    }
                    submittedIds.Add(childDto.EmployeeOnboardingId);
                    childDto.Adapt(target);
                    target.Id = childDto.EmployeeOnboardingId;
                    target.OfferId = entity.Id;
                    await _employeeOnboardingRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktEmployeeOnboarding>();
                    child.Id = 0;
                    child.OfferId = entity.Id;
                    toCreate.Add(child);
                }
            }
            foreach (var removed in existingList.Where(x => !submittedIds.Contains(x.Id)))
            {
                await _employeeOnboardingRepository.DeleteAsync(removed.Id);
            }
            if (toCreate.Count > 0)
            {
                await _employeeOnboardingRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建录用信息查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktTalentOffer, bool>> QueryExpression(TaktTalentOfferQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktTalentOffer>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.OfferCode != null && x.OfferCode.Contains(keywords))
                || (x.DeptName != null && x.DeptName.Contains(keywords))
                || (x.PostName != null && x.PostName.Contains(keywords))
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

        if (queryDto?.JobPostingId.HasValue == true)
        {
            var jobPostingId = queryDto.JobPostingId.Value;
            exp = exp.And(x => x.JobPostingId == jobPostingId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.OfferCode))
        {
            var offerCode = queryDto.OfferCode;
            exp = exp.And(x => x.OfferCode != null && x.OfferCode.Contains(offerCode));
        }

        if (queryDto?.EmployeeId.HasValue == true)
        {
            var employeeId = queryDto.EmployeeId.Value;
            exp = exp.And(x => x.EmployeeId == employeeId);
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

        if (queryDto?.HireDateStart.HasValue == true)
        {
            var hireDateStart = queryDto.HireDateStart.Value;
            exp = exp.And(x => x.HireDate >= hireDateStart);
        }

        if (queryDto?.HireDateEnd.HasValue == true)
        {
            var hireDateEnd = queryDto.HireDateEnd.Value;
            exp = exp.And(x => x.HireDate <= hireDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktTalentOfferQueryDto? queryDto)
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
        if (queryDto.JobPostingId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.OfferCode))
        {
            return true;
        }
        if (queryDto.EmployeeId.HasValue)
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
        if (queryDto.HireDateStart.HasValue || queryDto.HireDateEnd.HasValue)
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
