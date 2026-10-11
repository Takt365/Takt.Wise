// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Sop
// 文件名称：TaktSopStepService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP工步应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Sop;
using Takt.Domain.Entities.Logistics.Manufacturing.Sop;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Sop;

/// <summary>
/// SOP工步应用服务
/// </summary>
public class TaktSopStepService : TaktServiceBase, ITaktSopStepService
{
    private readonly ITaktCompanyRepository<TaktSopStep> _sopStepRepository;
    private readonly ITaktCompanyRepository<TaktSopStepMedia> _sopStepMediaRepository;
    private readonly ITaktCompanyRepository<TaktSopStepCheckItem> _sopStepCheckItemRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sopStepRepository">SOP工步仓储</param>
    /// <param name="sopStepMediaRepository">SopStepMedia仓储</param>
    /// <param name="sopStepCheckItemRepository">SopStepCheckItem仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSopStepService(
        ITaktCompanyRepository<TaktSopStep> sopStepRepository,
        ITaktCompanyRepository<TaktSopStepMedia> sopStepMediaRepository,
        ITaktCompanyRepository<TaktSopStepCheckItem> sopStepCheckItemRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _sopStepRepository = sopStepRepository;
        _sopStepMediaRepository = sopStepMediaRepository;
        _sopStepCheckItemRepository = sopStepCheckItemRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取SOP工步列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSopStepDto>> GetSopStepListAsync(TaktSopStepQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktSopStepDto>.Create(
                new List<TaktSopStepDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _sopStepRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSopStepDto>.Create(
            data.Adapt<List<TaktSopStepDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取SOP工步
    /// </summary>
    /// <param name="id">SOP工步ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopStepDto?> GetSopStepByIdAsync(long id)
    {
        var entity = await _sopStepRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktSopStepDto>();
        await FillSopStepDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取SOP工步选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSopStepOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _sopStepRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.StepTitle ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.StepTitle,
            DictLabel = e.StepTitle,
        }).ToList();
    }

    /// <summary>
    /// 创建SOP工步
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopStepDto> CreateSopStepAsync(TaktSopStepCreateDto dto)
    {
        var entity = dto.Adapt<TaktSopStep>();
        entity = await _sopStepRepository.CreateAsync(entity);
                await SaveSopStepChildrenAsync(entity, dto);
        return await GetSopStepByIdAsync(entity.Id) ?? entity.Adapt<TaktSopStepDto>();
    }

    /// <summary>
    /// 更新SOP工步
    /// </summary>
    /// <param name="id">SOP工步ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopStepDto> UpdateSopStepAsync(long id, TaktSopStepUpdateDto dto)
    {
        var entity = await _sopStepRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("SOP工步不存在");
        }
        dto.Adapt(entity);
        await _sopStepRepository.UpdateAsync(entity);
                await SaveSopStepChildrenAsync(entity, dto);
        return await GetSopStepByIdAsync(id) ?? throw new TaktBusinessException("SOP工步不存在");
    }

    /// <summary>
    /// 删除SOP工步
    /// </summary>
    /// <param name="id">SOP工步ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSopStepByIdAsync(long id)
    {
        var entity = await _sopStepRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("SOP工步不存在或已删除");
        }
        await _sopStepMediaRepository.DeleteAsync(x => x.StepId == entity.Id);
        await _sopStepCheckItemRepository.DeleteAsync(x => x.StepId == entity.Id);
        var deleted = await _sopStepRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("SOP工步不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除SOP工步
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSopStepBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSopStepByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSopStepTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSopStepTemplateDto>(
            sheetName ?? "SOP工步导入模板",
            fileName ?? "SOP工步导入模板.xlsx");
    }

    /// <summary>
    /// 导入SOP工步
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSopStepAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSopStepImportDto>(fileStream, sheetName ?? "SOP工步导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktSopStep>();
                await _sopStepRepository.CreateAsync(entity);
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
    /// 导出SOP工步
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSopStepAsync(TaktSopStepQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktSopStepQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSopStepExportDto>(),
                sheetName ?? "SOP工步数据",
                fileName ?? "SOP工步导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _sopStepRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSopStepExportDto>(),
                sheetName ?? "SOP工步数据",
                fileName ?? "SOP工步导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSopStepExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "SOP工步数据",
            fileName ?? "SOP工步导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充SOP工步详情（加载 OneToMany 子表：SOP工步多媒体、SOP工步检验项目）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillSopStepDetailsAsync(TaktSopStepDto dto, TaktSopStep entity)
    {
        if (dto == null)
        {
            return;
        }
        // SOP工步多媒体 → dto.MediaList
        var medialist = await _sopStepMediaRepository.GetListAsync(x => x.StepId == entity.Id);
        dto.MediaList = medialist.Adapt<List<TaktSopStepMediaDto>>();
        // SOP工步检验项目 → dto.CheckItems
        var checkitems = await _sopStepCheckItemRepository.GetListAsync(x => x.StepId == entity.Id);
        dto.CheckItems = checkitems.Adapt<List<TaktSopStepCheckItemDto>>();
    }

    /// <summary>
    /// 保存SOP工步子表级联（SOP工步多媒体、SOP工步检验项目；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveSopStepChildrenAsync(TaktSopStep entity, TaktSopStepCreateDto dto)
    {
        // SOP工步多媒体（MediaList）
        List<TaktSopStepMediaUpdateDto>? mediaListForSave;
        if (dto is TaktSopStepUpdateDto updateDtoForMediaList && updateDtoForMediaList.MediaList != null)
        {
            mediaListForSave = updateDtoForMediaList.MediaList;
        }
        else if (dto.MediaList != null)
        {
            mediaListForSave = dto.MediaList.Adapt<List<TaktSopStepMediaUpdateDto>>();
        }
        else
        {
            mediaListForSave = null;
        }
        if (mediaListForSave is not { Count: > 0 })
        {
            await _sopStepMediaRepository.DeleteAsync(x => x.StepId == entity.Id);
        }
        else
        {
            var existingList = await _sopStepMediaRepository.GetListAsync(x => x.StepId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktSopStepMedia>();
            for (var i = 0; i < mediaListForSave.Count; i++)
            {
                var childDto = mediaListForSave[i];
                childDto.StepId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                if (childDto.SopStepMediaId > 0)
                {
                    if (!existingById.TryGetValue(childDto.SopStepMediaId, out var target))
                    {
                        throw new TaktBusinessException("SOP工步多媒体不存在（SopStepMediaId={childDto.SopStepMediaId}）");
                    }
                    if (target.StepId != entity.Id)
                    {
                        throw new TaktBusinessException("SOP工步多媒体不属于当前主表（SopStepMediaId={childDto.SopStepMediaId}）");
                    }
                    submittedIds.Add(childDto.SopStepMediaId);
                    childDto.Adapt(target);
                    target.Id = childDto.SopStepMediaId;
                    target.StepId = entity.Id;
                    await _sopStepMediaRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktSopStepMedia>();
                    child.Id = 0;
                    child.StepId = entity.Id;
                    toCreate.Add(child);
                }
            }
            foreach (var removed in existingList.Where(x => !submittedIds.Contains(x.Id)))
            {
                await _sopStepMediaRepository.DeleteAsync(removed.Id);
            }
            if (toCreate.Count > 0)
            {
                await _sopStepMediaRepository.CreateRangeAsync(toCreate);
            }
        }
        // SOP工步检验项目（CheckItems）
        List<TaktSopStepCheckItemUpdateDto>? checkItemsForSave;
        if (dto is TaktSopStepUpdateDto updateDtoForCheckItems && updateDtoForCheckItems.CheckItems != null)
        {
            checkItemsForSave = updateDtoForCheckItems.CheckItems;
        }
        else if (dto.CheckItems != null)
        {
            checkItemsForSave = dto.CheckItems.Adapt<List<TaktSopStepCheckItemUpdateDto>>();
        }
        else
        {
            checkItemsForSave = null;
        }
        if (checkItemsForSave is not { Count: > 0 })
        {
            await _sopStepCheckItemRepository.DeleteAsync(x => x.StepId == entity.Id);
        }
        else
        {
            var existingList = await _sopStepCheckItemRepository.GetListAsync(x => x.StepId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktSopStepCheckItem>();
            for (var i = 0; i < checkItemsForSave.Count; i++)
            {
                var childDto = checkItemsForSave[i];
                childDto.StepId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                if (childDto.SopStepCheckItemId > 0)
                {
                    if (!existingById.TryGetValue(childDto.SopStepCheckItemId, out var target))
                    {
                        throw new TaktBusinessException("SOP工步检验项目不存在（SopStepCheckItemId={childDto.SopStepCheckItemId}）");
                    }
                    if (target.StepId != entity.Id)
                    {
                        throw new TaktBusinessException("SOP工步检验项目不属于当前主表（SopStepCheckItemId={childDto.SopStepCheckItemId}）");
                    }
                    submittedIds.Add(childDto.SopStepCheckItemId);
                    childDto.Adapt(target);
                    target.Id = childDto.SopStepCheckItemId;
                    target.StepId = entity.Id;
                    await _sopStepCheckItemRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktSopStepCheckItem>();
                    child.Id = 0;
                    child.StepId = entity.Id;
                    toCreate.Add(child);
                }
            }
            foreach (var removed in existingList.Where(x => !submittedIds.Contains(x.Id)))
            {
                await _sopStepCheckItemRepository.DeleteAsync(removed.Id);
            }
            if (toCreate.Count > 0)
            {
                await _sopStepCheckItemRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建SOP工步查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSopStep, bool>> QueryExpression(TaktSopStepQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSopStep>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.StepTitle != null && x.StepTitle.Contains(keywords))
                || (x.StepDescription != null && x.StepDescription.Contains(keywords))
                || (x.SafetyAlert != null && x.SafetyAlert.Contains(keywords))
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

        if (queryDto?.ContentId.HasValue == true)
        {
            var contentId = queryDto.ContentId.Value;
            exp = exp.And(x => x.ContentId == contentId);
        }

        if (queryDto?.StepNo.HasValue == true)
        {
            var stepNo = queryDto.StepNo.Value;
            exp = exp.And(x => x.StepNo == stepNo);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.StepTitle))
        {
            var stepTitle = queryDto.StepTitle;
            exp = exp.And(x => x.StepTitle != null && x.StepTitle.Contains(stepTitle));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.StepDescription))
        {
            var stepDescription = queryDto.StepDescription;
            exp = exp.And(x => x.StepDescription != null && x.StepDescription.Contains(stepDescription));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SafetyAlert))
        {
            var safetyAlert = queryDto.SafetyAlert;
            exp = exp.And(x => x.SafetyAlert != null && x.SafetyAlert.Contains(safetyAlert));
        }

        if (queryDto?.SafetyPopupRequired.HasValue == true)
        {
            var safetyPopupRequired = queryDto.SafetyPopupRequired.Value;
            exp = exp.And(x => x.SafetyPopupRequired == safetyPopupRequired);
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
    private static bool HasAnyListQueryFilter(TaktSopStepQueryDto? queryDto)
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
        if (queryDto.ContentId.HasValue)
        {
            return true;
        }
        if (queryDto.StepNo.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.StepTitle))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.StepDescription))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SafetyAlert))
        {
            return true;
        }
        if (queryDto.SafetyPopupRequired.HasValue)
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
        if (queryDto.CreatedAtStart.HasValue || queryDto.CreatedAtEnd.HasValue)
        {
            return true;
        }
        return false;
    }
}
