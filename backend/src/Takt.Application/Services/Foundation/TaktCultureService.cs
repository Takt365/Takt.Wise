// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：TaktCultureService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：区域应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Foundation;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 区域应用服务
/// </summary>
public class TaktCultureService : TaktServiceBase, ITaktCultureService
{
    private readonly ITaktTenantRepository<TaktCulture> _cultureRepository;
    private readonly ITaktTenantRepository<TaktTranslation> _translationRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="cultureRepository">区域仓储</param>
    /// <param name="translationRepository">Translation仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktCultureService(
        ITaktTenantRepository<TaktCulture> cultureRepository,
        ITaktTenantRepository<TaktTranslation> translationRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _cultureRepository = cultureRepository;
        _translationRepository = translationRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取区域列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktCultureDto>> GetCultureListAsync(TaktCultureQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktCultureDto>.Create(
                new List<TaktCultureDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _cultureRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktCultureDto>.Create(
            data.Adapt<List<TaktCultureDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取区域
    /// </summary>
    /// <param name="id">区域ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktCultureDto?> GetCultureByIdAsync(long id)
    {
        var entity = await _cultureRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktCultureDto>();
        await FillCultureDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取区域选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetCultureOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        var list = await _cultureRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode,
            x => x.NativeName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.CultureCode,
            DictLabel = e.NativeName ?? e.CultureCode,
        }).ToList();
    }

    /// <summary>
    /// 创建区域
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCultureDto> CreateCultureAsync(TaktCultureCreateDto dto)
    {
        var entity = dto.Adapt<TaktCulture>();
        var isUnique_ix_culture_culture_unique = await _uniqueValidator.IsUniqueAsync(
            _cultureRepository,
            x => x.CultureCode == entity.CultureCode);
        if (!isUnique_ix_culture_culture_unique)
        {
            throw new TaktBusinessException("区域的CultureCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _cultureRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _cultureRepository.CreateAsync(entity);
                await SaveCultureChildrenAsync(entity, dto);
        return await GetCultureByIdAsync(entity.Id) ?? entity.Adapt<TaktCultureDto>();
    }

    /// <summary>
    /// 更新区域
    /// </summary>
    /// <param name="id">区域ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCultureDto> UpdateCultureAsync(long id, TaktCultureUpdateDto dto)
    {
        var entity = await _cultureRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("区域不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_culture_culture_unique = await _uniqueValidator.IsUniqueAsync(
            _cultureRepository,
            x => x.CultureCode == entity.CultureCode,
            id);
        if (!isUnique_ix_culture_culture_unique)
        {
            throw new TaktBusinessException("区域的CultureCode已存在");
        }
        await _cultureRepository.UpdateAsync(entity);
                await SaveCultureChildrenAsync(entity, dto);
        return await GetCultureByIdAsync(id) ?? throw new TaktBusinessException("区域不存在");
    }

    /// <summary>
    /// 删除区域
    /// </summary>
    /// <param name="id">区域ID</param>
    /// <returns>任务</returns>
    public async Task DeleteCultureByIdAsync(long id)
    {
        var entity = await _cultureRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("区域不存在或已删除");
        }
        await _translationRepository.DeleteAsync(x => x.CultureId == entity.Id);
        var deleted = await _cultureRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("区域不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除区域
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteCultureBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteCultureByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新区域排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCultureDto> UpdateCultureSortAsync(TaktCultureSortDto dto)
    {
        var entity = await _cultureRepository.GetByIdAsync(dto.CultureId);
        if (entity == null)
        {
            throw new TaktBusinessException("区域不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _cultureRepository.UpdateAsync(entity);
        return await GetCultureByIdAsync(dto.CultureId) ?? throw new TaktBusinessException("区域不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetCultureTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktCultureTemplateDto>(
            sheetName ?? "区域导入模板",
            fileName ?? "区域导入模板.xlsx");
    }

    /// <summary>
    /// 导入区域
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportCultureAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktCultureImportDto>(fileStream, sheetName ?? "区域导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _cultureRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktCulture>();
                var importKey = $"{entity.CultureCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（CultureCode）");
                }
                var isUnique_ix_culture_culture_unique = await _uniqueValidator.IsUniqueAsync(
                    _cultureRepository,
                    x => x.CultureCode == entity.CultureCode);
                if (!isUnique_ix_culture_culture_unique)
                {
                    throw new TaktBusinessException("区域的CultureCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _cultureRepository.CreateAsync(entity);
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
    /// 导出区域
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportCultureAsync(TaktCultureQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktCultureQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktCultureExportDto>(),
                sheetName ?? "区域数据",
                fileName ?? "区域导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _cultureRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktCultureExportDto>(),
                sheetName ?? "区域数据",
                fileName ?? "区域导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktCultureExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "区域数据",
            fileName ?? "区域导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充区域详情（加载 OneToMany 子表：翻译）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillCultureDetailsAsync(TaktCultureDto dto, TaktCulture entity)
    {
        if (dto == null)
        {
            return;
        }
        // 翻译 → dto.TranslationList
        var translationlist = await _translationRepository.GetListAsync(x => x.CultureId == entity.Id);
        dto.TranslationList = translationlist.Adapt<List<TaktTranslationDto>>();
    }

    /// <summary>
    /// 保存区域子表级联（翻译；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveCultureChildrenAsync(TaktCulture entity, TaktCultureCreateDto dto)
    {
        // 翻译（TranslationList）
        List<TaktTranslationUpdateDto>? translationListForSave;
        if (dto is TaktCultureUpdateDto updateDtoForTranslationList && updateDtoForTranslationList.TranslationList != null)
        {
            translationListForSave = updateDtoForTranslationList.TranslationList;
        }
        else if (dto.TranslationList != null)
        {
            translationListForSave = dto.TranslationList.Adapt<List<TaktTranslationUpdateDto>>();
        }
        else
        {
            translationListForSave = null;
        }
        if (translationListForSave is not { Count: > 0 })
        {
            await _translationRepository.DeleteAsync(x => x.CultureId == entity.Id);
        }
        else
        {
            var existingList = await _translationRepository.GetListAsync(x => x.CultureId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktTranslation>();
            for (var i = 0; i < translationListForSave.Count; i++)
            {
                var childDto = translationListForSave[i];
                childDto.CultureId = entity.Id;
                childDto.TenantCode = entity.TenantCode;


                if (childDto.TranslationId > 0)
                {
                    if (!existingById.TryGetValue(childDto.TranslationId, out var target))
                    {
                        throw new TaktBusinessException("翻译不存在（TranslationId={childDto.TranslationId}）");
                    }
                    if (target.CultureId != entity.Id)
                    {
                        throw new TaktBusinessException("翻译不属于当前主表（TranslationId={childDto.TranslationId}）");
                    }
                    submittedIds.Add(childDto.TranslationId);
                    childDto.Adapt(target);
                    target.Id = childDto.TranslationId;
                    target.CultureId = entity.Id;
                    await _translationRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktTranslation>();
                    child.Id = 0;
                    child.CultureId = entity.Id;
                    toCreate.Add(child);
                }
            }
            foreach (var removed in existingList.Where(x => !submittedIds.Contains(x.Id)))
            {
                await _translationRepository.DeleteAsync(removed.Id);
            }
            if (toCreate.Count > 0)
            {
                await _translationRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建区域查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktCulture, bool>> QueryExpression(TaktCultureQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktCulture>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.NativeName != null && x.NativeName.Contains(keywords))
                || (x.Icon != null && x.Icon.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
            );
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.NativeName))
        {
            var nativeName = queryDto.NativeName;
            exp = exp.And(x => x.NativeName != null && x.NativeName.Contains(nativeName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Icon))
        {
            var icon = queryDto.Icon;
            exp = exp.And(x => x.Icon != null && x.Icon.Contains(icon));
        }

        if (queryDto?.IsDefault.HasValue == true)
        {
            var isDefault = queryDto.IsDefault.Value;
            exp = exp.And(x => x.IsDefault == isDefault);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            var sortOrder = queryDto.SortOrder.Value;
            exp = exp.And(x => x.SortOrder == sortOrder);
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
    private static bool HasAnyListQueryFilter(TaktCultureQueryDto? queryDto)
    {
        if (queryDto == null)
        {
            return false;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.KeyWords))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.NativeName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Icon))
        {
            return true;
        }
        if (queryDto.IsDefault.HasValue)
        {
            return true;
        }
        if (queryDto.SortOrder.HasValue)
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
