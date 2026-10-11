// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Talent
// 文件名称：TaktTalentJobPostingService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：职位发布应用服务实现
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
/// 职位发布应用服务
/// </summary>
public class TaktTalentJobPostingService : TaktServiceBase, ITaktTalentJobPostingService
{
    private readonly ITaktCompanyRepository<TaktTalentJobPosting> _talentJobPostingRepository;
    private readonly ITaktApprovalRepository<TaktTalentOffer> _talentOfferRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="talentJobPostingRepository">职位发布仓储</param>
    /// <param name="talentOfferRepository">TalentOffer仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktTalentJobPostingService(
        ITaktCompanyRepository<TaktTalentJobPosting> talentJobPostingRepository,
        ITaktApprovalRepository<TaktTalentOffer> talentOfferRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _talentJobPostingRepository = talentJobPostingRepository;
        _talentOfferRepository = talentOfferRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取职位发布列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktTalentJobPostingDto>> GetTalentJobPostingListAsync(TaktTalentJobPostingQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktTalentJobPostingDto>.Create(
                new List<TaktTalentJobPostingDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _talentJobPostingRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktTalentJobPostingDto>.Create(
            data.Adapt<List<TaktTalentJobPostingDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取职位发布
    /// </summary>
    /// <param name="id">职位发布ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktTalentJobPostingDto?> GetTalentJobPostingByIdAsync(long id)
    {
        var entity = await _talentJobPostingRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktTalentJobPostingDto>();
        await FillTalentJobPostingDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取职位发布选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetTalentJobPostingOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _talentJobPostingRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PostingStatus == 1,
            x => x.PostingCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.PostingCode,
            DictLabel = e.PostingCode,
        }).ToList();
    }

    /// <summary>
    /// 创建职位发布
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTalentJobPostingDto> CreateTalentJobPostingAsync(TaktTalentJobPostingCreateDto dto)
    {
        var entity = dto.Adapt<TaktTalentJobPosting>();
        var isUnique_ix_talent_job_posting_code_unique = await _uniqueValidator.IsUniqueAsync(
            _talentJobPostingRepository,
            x => x.PostingCode == entity.PostingCode);
        if (!isUnique_ix_talent_job_posting_code_unique)
        {
            throw new TaktBusinessException("职位发布的PostingCode已存在");
        }
        entity = await _talentJobPostingRepository.CreateAsync(entity);
                await SaveTalentJobPostingChildrenAsync(entity, dto);
        return await GetTalentJobPostingByIdAsync(entity.Id) ?? entity.Adapt<TaktTalentJobPostingDto>();
    }

    /// <summary>
    /// 更新职位发布
    /// </summary>
    /// <param name="id">职位发布ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTalentJobPostingDto> UpdateTalentJobPostingAsync(long id, TaktTalentJobPostingUpdateDto dto)
    {
        var entity = await _talentJobPostingRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("职位发布不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_talent_job_posting_code_unique = await _uniqueValidator.IsUniqueAsync(
            _talentJobPostingRepository,
            x => x.PostingCode == entity.PostingCode,
            id);
        if (!isUnique_ix_talent_job_posting_code_unique)
        {
            throw new TaktBusinessException("职位发布的PostingCode已存在");
        }
        await _talentJobPostingRepository.UpdateAsync(entity);
                await SaveTalentJobPostingChildrenAsync(entity, dto);
        return await GetTalentJobPostingByIdAsync(id) ?? throw new TaktBusinessException("职位发布不存在");
    }

    /// <summary>
    /// 删除职位发布
    /// </summary>
    /// <param name="id">职位发布ID</param>
    /// <returns>任务</returns>
    public async Task DeleteTalentJobPostingByIdAsync(long id)
    {
        var entity = await _talentJobPostingRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("职位发布不存在或已删除");
        }
        await _talentOfferRepository.DeleteAsync(x => x.JobPostingId == entity.Id);
        var deleted = await _talentJobPostingRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("职位发布不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除职位发布
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteTalentJobPostingBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteTalentJobPostingByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新职位发布状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTalentJobPostingDto> UpdateTalentJobPostingStatusAsync(TaktTalentJobPostingStatusDto dto)
    {
        var entity = await _talentJobPostingRepository.GetByIdAsync(dto.TalentJobPostingId);
        if (entity == null)
        {
            throw new TaktBusinessException("职位发布不存在");
        }
        entity.PostingStatus = dto.PostingStatus;
        await _talentJobPostingRepository.UpdateAsync(entity);
        return await GetTalentJobPostingByIdAsync(dto.TalentJobPostingId) ?? throw new TaktBusinessException("职位发布不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetTalentJobPostingTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktTalentJobPostingTemplateDto>(
            sheetName ?? "职位发布导入模板",
            fileName ?? "职位发布导入模板.xlsx");
    }

    /// <summary>
    /// 导入职位发布
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportTalentJobPostingAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktTalentJobPostingImportDto>(fileStream, sheetName ?? "职位发布导入模板");
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
                var entity = rows[i].Adapt<TaktTalentJobPosting>();
                var importKey = $"{entity.PostingCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PostingCode）");
                }
                var isUnique_ix_talent_job_posting_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _talentJobPostingRepository,
                    x => x.PostingCode == entity.PostingCode);
                if (!isUnique_ix_talent_job_posting_code_unique)
                {
                    throw new TaktBusinessException("职位发布的PostingCode已存在");
                }
                await _talentJobPostingRepository.CreateAsync(entity);
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
    /// 导出职位发布
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportTalentJobPostingAsync(TaktTalentJobPostingQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktTalentJobPostingQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktTalentJobPostingExportDto>(),
                sheetName ?? "职位发布数据",
                fileName ?? "职位发布导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _talentJobPostingRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktTalentJobPostingExportDto>(),
                sheetName ?? "职位发布数据",
                fileName ?? "职位发布导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktTalentJobPostingExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "职位发布数据",
            fileName ?? "职位发布导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充职位发布详情（加载 OneToMany 子表：录用信息）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillTalentJobPostingDetailsAsync(TaktTalentJobPostingDto dto, TaktTalentJobPosting entity)
    {
        if (dto == null)
        {
            return;
        }
        // 录用信息 → dto.TalentOffers
        var talentoffers = await _talentOfferRepository.GetListAsync(x => x.JobPostingId == entity.Id);
        dto.TalentOffers = talentoffers.Adapt<List<TaktTalentOfferDto>>();
    }

    /// <summary>
    /// 保存职位发布子表级联（录用信息；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveTalentJobPostingChildrenAsync(TaktTalentJobPosting entity, TaktTalentJobPostingCreateDto dto)
    {
        // 录用信息（TalentOffers）
        List<TaktTalentOfferUpdateDto>? talentOffersForSave;
        if (dto is TaktTalentJobPostingUpdateDto updateDtoForTalentOffers && updateDtoForTalentOffers.TalentOffers != null)
        {
            talentOffersForSave = updateDtoForTalentOffers.TalentOffers;
        }
        else if (dto.TalentOffers != null)
        {
            talentOffersForSave = dto.TalentOffers.Adapt<List<TaktTalentOfferUpdateDto>>();
        }
        else
        {
            talentOffersForSave = null;
        }
        if (talentOffersForSave is not { Count: > 0 })
        {
            await _talentOfferRepository.DeleteAsync(x => x.JobPostingId == entity.Id);
        }
        else
        {
            var existingList = await _talentOfferRepository.GetListAsync(x => x.JobPostingId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktTalentOffer>();
            for (var i = 0; i < talentOffersForSave.Count; i++)
            {
                var childDto = talentOffersForSave[i];
                childDto.JobPostingId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.Reason = entity.Reason;
                if (childDto.TalentOfferId > 0)
                {
                    if (!existingById.TryGetValue(childDto.TalentOfferId, out var target))
                    {
                        throw new TaktBusinessException("录用信息不存在（TalentOfferId={childDto.TalentOfferId}）");
                    }
                    if (target.JobPostingId != entity.Id)
                    {
                        throw new TaktBusinessException("录用信息不属于当前主表（TalentOfferId={childDto.TalentOfferId}）");
                    }
                    submittedIds.Add(childDto.TalentOfferId);
                    childDto.Adapt(target);
                    target.Id = childDto.TalentOfferId;
                    target.JobPostingId = entity.Id;
                    await _talentOfferRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktTalentOffer>();
                    child.Id = 0;
                    child.JobPostingId = entity.Id;
                    toCreate.Add(child);
                }
            }
            foreach (var removed in existingList.Where(x => !submittedIds.Contains(x.Id)))
            {
                await _talentOfferRepository.DeleteAsync(removed.Id);
            }
            if (toCreate.Count > 0)
            {
                await _talentOfferRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建职位发布查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktTalentJobPosting, bool>> QueryExpression(TaktTalentJobPostingQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktTalentJobPosting>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.PostingCode != null && x.PostingCode.Contains(keywords))
                || (x.TalentJobPostingTitle != null && x.TalentJobPostingTitle.Contains(keywords))
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

        if (queryDto?.StaffingRequirementId.HasValue == true)
        {
            var staffingRequirementId = queryDto.StaffingRequirementId.Value;
            exp = exp.And(x => x.StaffingRequirementId == staffingRequirementId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PostingCode))
        {
            var postingCode = queryDto.PostingCode;
            exp = exp.And(x => x.PostingCode != null && x.PostingCode.Contains(postingCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TalentJobPostingTitle))
        {
            var talentJobPostingTitle = queryDto.TalentJobPostingTitle;
            exp = exp.And(x => x.TalentJobPostingTitle != null && x.TalentJobPostingTitle.Contains(talentJobPostingTitle));
        }

        if (queryDto?.PublishChannel.HasValue == true)
        {
            var publishChannel = queryDto.PublishChannel.Value;
            exp = exp.And(x => x.PublishChannel == publishChannel);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Reason))
        {
            var reason = queryDto.Reason;
            exp = exp.And(x => x.Reason != null && x.Reason.Contains(reason));
        }

        if (queryDto?.PostingStatus.HasValue == true)
        {
            var postingStatus = queryDto.PostingStatus.Value;
            exp = exp.And(x => x.PostingStatus == postingStatus);
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

        if (queryDto?.PublishDateStart.HasValue == true)
        {
            var publishDateStart = queryDto.PublishDateStart.Value;
            exp = exp.And(x => x.PublishDate >= publishDateStart);
        }

        if (queryDto?.PublishDateEnd.HasValue == true)
        {
            var publishDateEnd = queryDto.PublishDateEnd.Value;
            exp = exp.And(x => x.PublishDate <= publishDateEnd);
        }

        if (queryDto?.OpenDateStart.HasValue == true)
        {
            var openDateStart = queryDto.OpenDateStart.Value;
            exp = exp.And(x => x.OpenDate >= openDateStart);
        }

        if (queryDto?.OpenDateEnd.HasValue == true)
        {
            var openDateEnd = queryDto.OpenDateEnd.Value;
            exp = exp.And(x => x.OpenDate <= openDateEnd);
        }

        if (queryDto?.CloseDateStart.HasValue == true)
        {
            var closeDateStart = queryDto.CloseDateStart.Value;
            exp = exp.And(x => x.CloseDate >= closeDateStart);
        }

        if (queryDto?.CloseDateEnd.HasValue == true)
        {
            var closeDateEnd = queryDto.CloseDateEnd.Value;
            exp = exp.And(x => x.CloseDate <= closeDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktTalentJobPostingQueryDto? queryDto)
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
        if (queryDto.StaffingRequirementId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PostingCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TalentJobPostingTitle))
        {
            return true;
        }
        if (queryDto.PublishChannel.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Reason))
        {
            return true;
        }
        if (queryDto.PostingStatus.HasValue)
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
        if (queryDto.PublishDateStart.HasValue || queryDto.PublishDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.OpenDateStart.HasValue || queryDto.OpenDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.CloseDateStart.HasValue || queryDto.CloseDateEnd.HasValue)
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
