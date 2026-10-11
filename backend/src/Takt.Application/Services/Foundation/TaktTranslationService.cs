// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：TaktTranslationService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：翻译应用服务实现
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
/// 翻译应用服务
/// </summary>
public class TaktTranslationService : TaktServiceBase, ITaktTranslationService
{
    private readonly ITaktTenantRepository<TaktTranslation> _translationRepository;
    private readonly ITaktTenantRepository<TaktCulture> _cultureRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="translationRepository">翻译仓储</param>
    /// <param name="cultureRepository">区域仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktTranslationService(
        ITaktTenantRepository<TaktTranslation> translationRepository,
        ITaktTenantRepository<TaktCulture> cultureRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _translationRepository = translationRepository;
        _cultureRepository = cultureRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取翻译列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktTranslationDto>> GetTranslationListAsync(TaktTranslationQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktTranslationDto>.Create(
                new List<TaktTranslationDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _translationRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktTranslationDto>.Create(
            data.Adapt<List<TaktTranslationDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取翻译
    /// </summary>
    /// <param name="id">翻译ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktTranslationDto?> GetTranslationByIdAsync(long id)
    {
        var entity = await _translationRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode)
        {
            return null;
        }
        return entity.Adapt<TaktTranslationDto>();
    }

    /// <summary>
    /// 获取翻译选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetTranslationOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        var list = await _translationRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode,
            x => x.I18nKey ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.I18nKey,
            DictLabel = e.I18nKey,
        }).ToList();
    }

    /// <summary>
    /// 创建翻译
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTranslationDto> CreateTranslationAsync(TaktTranslationCreateDto dto)
    {
        var entity = dto.Adapt<TaktTranslation>();
        await StampTranslationCultureAsync(entity, dto);
        var isUnique_ix_translation_key_culture_unique = await _uniqueValidator.IsUniqueAsync(
            _translationRepository,
            x => x.I18nKey == entity.I18nKey
                && x.CultureCode == entity.CultureCode);
        if (!isUnique_ix_translation_key_culture_unique)
        {
            throw new TaktBusinessException("翻译的I18nKey、CultureCode已存在");
        }
        entity = await _translationRepository.CreateAsync(entity);
        return await GetTranslationByIdAsync(entity.Id) ?? entity.Adapt<TaktTranslationDto>();
    }

    /// <summary>
    /// 更新翻译
    /// </summary>
    /// <param name="id">翻译ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTranslationDto> UpdateTranslationAsync(long id, TaktTranslationUpdateDto dto)
    {
        var entity = await _translationRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("翻译不存在");
        }
        dto.Adapt(entity);
        await StampTranslationCultureAsync(entity, dto);
        var isUnique_ix_translation_key_culture_unique = await _uniqueValidator.IsUniqueAsync(
            _translationRepository,
            x => x.I18nKey == entity.I18nKey
                && x.CultureCode == entity.CultureCode,
            id);
        if (!isUnique_ix_translation_key_culture_unique)
        {
            throw new TaktBusinessException("翻译的I18nKey、CultureCode已存在");
        }
        await _translationRepository.UpdateAsync(entity);
        return await GetTranslationByIdAsync(id) ?? throw new TaktBusinessException("翻译不存在");
    }

    /// <summary>
    /// 删除翻译
    /// </summary>
    /// <param name="id">翻译ID</param>
    /// <returns>任务</returns>
    public async Task DeleteTranslationByIdAsync(long id)
    {
        var deleted = await _translationRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("翻译不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除翻译
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteTranslationBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteTranslationByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetTranslationTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktTranslationTemplateDto>(
            sheetName ?? "翻译导入模板",
            fileName ?? "翻译导入模板.xlsx");
    }

    /// <summary>
    /// 导入翻译
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportTranslationAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktTranslationImportDto>(fileStream, sheetName ?? "翻译导入模板");
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
                var entity = rows[i].Adapt<TaktTranslation>();
                var importDto = rows[i].Adapt<TaktTranslationCreateDto>();
                await StampTranslationCultureAsync(entity, importDto);
                var importKey = $"{entity.I18nKey}|{entity.CultureCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（I18nKey、CultureCode）");
                }
                var isUnique_ix_translation_key_culture_unique = await _uniqueValidator.IsUniqueAsync(
                    _translationRepository,
                    x => x.I18nKey == entity.I18nKey
                        && x.CultureCode == entity.CultureCode);
                if (!isUnique_ix_translation_key_culture_unique)
                {
                    throw new TaktBusinessException("翻译的I18nKey、CultureCode已存在");
                }
                await _translationRepository.CreateAsync(entity);
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
    /// 导出翻译
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportTranslationAsync(TaktTranslationQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktTranslationQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktTranslationExportDto>(),
                sheetName ?? "翻译数据",
                fileName ?? "翻译导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _translationRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktTranslationExportDto>(),
                sheetName ?? "翻译数据",
                fileName ?? "翻译导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktTranslationExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "翻译数据",
            fileName ?? "翻译导出.xlsx");
    }

    // ========================================
    // 扩展方法（保留）
    // ========================================

    /// <summary>
    /// 获取指定文化下的前端动态翻译键值（扁平 I18nKey → 文本，仅 resource_type=frontend）
    /// </summary>
    /// <param name="cultureCode">区域文化编码（BCP47）</param>
    /// <returns>前端 messages 包</returns>
    public async Task<TaktTranslationMessagesDto> GetTranslationMessagesAsync(string cultureCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cultureCode);
        var normalized = cultureCode.Trim();
        var list = await _translationRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CultureCode == normalized
                && x.ResourceType == "frontend");
        var messages = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var item in list)
        {
            if (string.IsNullOrWhiteSpace(item.I18nKey))
            {
                continue;
            }
            messages[item.I18nKey] = item.TranslationText ?? string.Empty;
        }
        return new TaktTranslationMessagesDto
        {
            CultureCode = normalized,
            Messages = messages
        };
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步翻译主表外键（ManyToOne → 区域）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampTranslationCultureAsync(TaktTranslation entity, TaktTranslationCreateDto dto)
    {
        if (dto.CultureId <= 0)
        {
            return;
        }
        var master = await _cultureRepository.GetByIdAsync(dto.CultureId);
        if (master == null)
        {
            throw new TaktBusinessException("区域不存在");
        }
        entity.CultureCode = master.CultureCode;
        entity.CultureId = master.Id;
        if (string.IsNullOrEmpty(entity.TenantCode))
        {
            entity.TenantCode = master.TenantCode;
        }
    }
    // ========================================
    // 转置（多语言表格）
    // ========================================


    /// <summary>
    /// 获取转置列头主表（区域文化，仅启用项）
    /// </summary>
    private async Task<List<TaktCulture>> GetTransposedMasterCulturesAsync()
    {
        return await _cultureRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode,
            x => x.SortOrder,
            false);
    }

    /// <summary>
    /// 获取翻译转置列表（分页）
    /// </summary>
    public async Task<TaktTranslationTransposedResultDto> GetTranslationTransposedListAsync(TaktTranslationTransposedQueryDto queryDto)
    {
        var cultures = await GetTransposedMasterCulturesAsync();
        var cultureCodeOrder = cultures.Select(c => c.CultureCode).ToList();

        var all = await _translationRepository.GetListAsync(TransposedQueryExpression(queryDto));
        var grouped = all
            .GroupBy(x => new { x.I18nKey, x.ResourceGroup, x.ResourceType })
            .OrderBy(g => g.First().I18nKey)
            .ToList();
        var total = grouped.Count;
        var pageGroups = grouped
            .Skip((queryDto.PageIndex - 1) * queryDto.PageSize)
            .Take(queryDto.PageSize)
            .ToList();

        var rows = new List<TaktTranslationTransposedDto>();
        foreach (var g in pageGroups)
        {
            var first = g.First();
            var row = new TaktTranslationTransposedDto
            {
                TranslationId = first.Id,
                I18nKey = first.I18nKey,
                ResourceGroup = first.ResourceGroup,
                ResourceType = first.ResourceType,
                ContextNote = first.ContextNote,
                Translations = new Dictionary<string, string>()
            };
            foreach (var code in cultureCodeOrder)
            {
                var item = g.FirstOrDefault(x => x.CultureCode == code);
                row.Translations[code] = item?.TranslationText ?? string.Empty;
            }
            rows.Add(row);
        }

        return new TaktTranslationTransposedResultDto
        {
            Paged = TaktPagedResult<TaktTranslationTransposedDto>.Create(rows, total, queryDto.PageIndex, queryDto.PageSize),
            CultureCodeOrder = cultureCodeOrder
        };
    }

    /// <summary>
    /// 批量保存翻译转置数据
    /// </summary>
    public async Task<int> SaveTranslationTransposedBatchAsync(TaktTranslationTransposedBatchDto dto)
    {
        if (dto.Rows == null || dto.Rows.Count == 0)
        {
            return 0;
        }
        var cultures = await GetTransposedMasterCulturesAsync();
        var cultureMap = cultures.ToDictionary(c => c.CultureCode, c => c);
        var affected = 0;
        foreach (var row in dto.Rows)
        {
            foreach (var kvp in row.Translations)
            {
                var cultureCode = kvp.Key;
                var text = kvp.Value ?? string.Empty;
                if (!cultureMap.TryGetValue(cultureCode, out var culture))
                {
                    continue;
                }

                var existing = await _translationRepository.GetListAsync(x =>
                    x.I18nKey == row.I18nKey
                    && x.CultureCode == cultureCode
                    && x.ResourceGroup == row.ResourceGroup
                    && x.ResourceType == row.ResourceType);
                var entity = existing.FirstOrDefault();
                if (entity != null)
                {
                    entity.TranslationText = text;
                    entity.ContextNote = row.ContextNote;
                    entity.CultureId = culture.Id;
                    entity.CultureCode = cultureCode;
                    await _translationRepository.UpdateAsync(entity);
                    affected += 1;
                }
                else if (!string.IsNullOrWhiteSpace(text))
                {
                    var created = new TaktTranslation
                    {
                        CultureId = culture.Id,
                        CultureCode = cultureCode,
                        I18nKey = row.I18nKey,
                        TranslationText = text,
                        ResourceGroup = row.ResourceGroup,
                        ResourceType = row.ResourceType,
                        ContextNote = row.ContextNote
                    };
                    await _translationRepository.CreateAsync(created);
                    affected += 1;
                }
            }
        }
        return affected;
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建翻译查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktTranslation, bool>> QueryExpression(TaktTranslationQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktTranslation>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.I18nKey != null && x.I18nKey.Contains(keywords))
                || (x.TranslationText != null && x.TranslationText.Contains(keywords))
                || (x.ResourceGroup != null && x.ResourceGroup.Contains(keywords))
                || (x.ResourceType != null && x.ResourceType.Contains(keywords))
                || (x.ContextNote != null && x.ContextNote.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
            );
        }

        if (queryDto?.CultureId.HasValue == true)
        {
            var cultureId = queryDto.CultureId.Value;
            exp = exp.And(x => x.CultureId == cultureId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.I18nKey))
        {
            var i18nKey = queryDto.I18nKey;
            exp = exp.And(x => x.I18nKey != null && x.I18nKey.Contains(i18nKey));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TranslationText))
        {
            var translationText = queryDto.TranslationText;
            exp = exp.And(x => x.TranslationText != null && x.TranslationText.Contains(translationText));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ResourceGroup))
        {
            var resourceGroup = queryDto.ResourceGroup;
            exp = exp.And(x => x.ResourceGroup != null && x.ResourceGroup.Contains(resourceGroup));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ResourceType))
        {
            var resourceType = queryDto.ResourceType;
            exp = exp.And(x => x.ResourceType != null && x.ResourceType.Contains(resourceType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ContextNote))
        {
            var contextNote = queryDto.ContextNote;
            exp = exp.And(x => x.ContextNote != null && x.ContextNote.Contains(contextNote));
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
    private static bool HasAnyListQueryFilter(TaktTranslationQueryDto? queryDto)
    {
        if (queryDto == null)
        {
            return false;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.KeyWords))
        {
            return true;
        }
        if (queryDto.CultureId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.I18nKey))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TranslationText))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ResourceGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ResourceType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ContextNote))
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

    /// <summary>
    /// 构建翻译转置查询表达式
    /// </summary>
    private Expression<Func<TaktTranslation, bool>> TransposedQueryExpression(TaktTranslationTransposedQueryDto queryDto)
    {
        return translation => translation.TenantCode == CurrentTenantCode
                    && (string.IsNullOrEmpty(queryDto.KeyWords)
                        || (translation.CultureCode != null && translation.CultureCode.Contains(queryDto.KeyWords))
                        || (translation.I18nKey != null && translation.I18nKey.Contains(queryDto.KeyWords))
                        || (translation.TranslationText != null && translation.TranslationText.Contains(queryDto.KeyWords))
                        || (translation.ContextNote != null && translation.ContextNote.Contains(queryDto.KeyWords)))
                    && (!queryDto.CultureId.HasValue || translation.CultureId == queryDto.CultureId.Value)
                    && (string.IsNullOrEmpty(queryDto.CultureCode) || (translation.CultureCode != null && translation.CultureCode.Contains(queryDto.CultureCode)))
                    && (string.IsNullOrEmpty(queryDto.I18nKey) || (translation.I18nKey != null && translation.I18nKey.Contains(queryDto.I18nKey)))
                    && (string.IsNullOrEmpty(queryDto.TranslationText) || (translation.TranslationText != null && translation.TranslationText.Contains(queryDto.TranslationText)))
                    && (string.IsNullOrEmpty(queryDto.ResourceGroup) || (translation.ResourceGroup != null && translation.ResourceGroup.Contains(queryDto.ResourceGroup)))
                    && (string.IsNullOrEmpty(queryDto.ResourceType) || (translation.ResourceType != null && translation.ResourceType.Contains(queryDto.ResourceType)))
                    && (string.IsNullOrEmpty(queryDto.ContextNote) || (translation.ContextNote != null && translation.ContextNote.Contains(queryDto.ContextNote)));;
    }
}
