// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：TaktVocabularyService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：敏感词应用服务实现
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
/// 敏感词应用服务
/// </summary>
public class TaktVocabularyService : TaktServiceBase, ITaktVocabularyService
{
    private readonly ITaktTenantRepository<TaktVocabulary> _vocabularyRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="vocabularyRepository">敏感词仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktVocabularyService(
        ITaktTenantRepository<TaktVocabulary> vocabularyRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _vocabularyRepository = vocabularyRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取敏感词列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktVocabularyDto>> GetVocabularyListAsync(TaktVocabularyQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _vocabularyRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktVocabularyDto>.Create(
            data.Adapt<List<TaktVocabularyDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取敏感词
    /// </summary>
    /// <param name="id">敏感词ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktVocabularyDto?> GetVocabularyByIdAsync(long id)
    {
        var entity = await _vocabularyRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode)
        {
            return null;
        }
        return entity.Adapt<TaktVocabularyDto>();
    }

    /// <summary>
    /// 获取敏感词选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetVocabularyOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        var list = await _vocabularyRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.VocabularyStatus == 1,
            x => x.WordText ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.WordText ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建敏感词
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktVocabularyDto> CreateVocabularyAsync(TaktVocabularyCreateDto dto)
    {
        var entity = dto.Adapt<TaktVocabulary>();
        var isUnique_ix_vocabulary_word_text_unique = await _uniqueValidator.IsUniqueAsync(
            _vocabularyRepository,
            x => x.WordText == entity.WordText);
        if (!isUnique_ix_vocabulary_word_text_unique)
        {
            throw new TaktBusinessException("敏感词的WordText已存在");
        }
        entity = await _vocabularyRepository.CreateAsync(entity);
        return await GetVocabularyByIdAsync(entity.Id) ?? entity.Adapt<TaktVocabularyDto>();
    }

    /// <summary>
    /// 更新敏感词
    /// </summary>
    /// <param name="id">敏感词ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktVocabularyDto> UpdateVocabularyAsync(long id, TaktVocabularyUpdateDto dto)
    {
        var entity = await _vocabularyRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("敏感词不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_vocabulary_word_text_unique = await _uniqueValidator.IsUniqueAsync(
            _vocabularyRepository,
            x => x.WordText == entity.WordText,
            id);
        if (!isUnique_ix_vocabulary_word_text_unique)
        {
            throw new TaktBusinessException("敏感词的WordText已存在");
        }
        await _vocabularyRepository.UpdateAsync(entity);
        return await GetVocabularyByIdAsync(id) ?? throw new TaktBusinessException("敏感词不存在");
    }

    /// <summary>
    /// 删除敏感词
    /// </summary>
    /// <param name="id">敏感词ID</param>
    /// <returns>任务</returns>
    public async Task DeleteVocabularyByIdAsync(long id)
    {
        var deleted = await _vocabularyRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("敏感词不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除敏感词
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteVocabularyBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteVocabularyByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新敏感词状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktVocabularyDto> UpdateVocabularyStatusAsync(TaktVocabularyStatusDto dto)
    {
        var entity = await _vocabularyRepository.GetByIdAsync(dto.VocabularyId);
        if (entity == null)
        {
            throw new TaktBusinessException("敏感词不存在");
        }
        entity.VocabularyStatus = dto.VocabularyStatus;
        await _vocabularyRepository.UpdateAsync(entity);
        return await GetVocabularyByIdAsync(dto.VocabularyId) ?? throw new TaktBusinessException("敏感词不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetVocabularyTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktVocabularyTemplateDto>(
            sheetName ?? "敏感词导入模板",
            fileName ?? "敏感词导入模板.xlsx");
    }

    /// <summary>
    /// 导入敏感词
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportVocabularyAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktVocabularyImportDto>(fileStream, sheetName ?? "敏感词导入模板");
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
                var entity = rows[i].Adapt<TaktVocabulary>();
                var importKey = $"{entity.WordText}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（WordText）");
                }
                var isUnique_ix_vocabulary_word_text_unique = await _uniqueValidator.IsUniqueAsync(
                    _vocabularyRepository,
                    x => x.WordText == entity.WordText);
                if (!isUnique_ix_vocabulary_word_text_unique)
                {
                    throw new TaktBusinessException("敏感词的WordText已存在");
                }
                await _vocabularyRepository.CreateAsync(entity);
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
    /// 导出敏感词
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportVocabularyAsync(TaktVocabularyQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktVocabularyQueryDto());
        var list = await _vocabularyRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktVocabularyExportDto>(),
                sheetName ?? "敏感词数据",
                fileName ?? "敏感词导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktVocabularyExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "敏感词数据",
            fileName ?? "敏感词导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建敏感词查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktVocabulary, bool>> QueryExpression(TaktVocabularyQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktVocabulary>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.WordText != null && x.WordText.Contains(keywords))
                || SqlFunc.ToString(x.WordCategory).Contains(keywords)
                || SqlFunc.ToString(x.FilterLevel).Contains(keywords)
                || (x.ReplaceText != null && x.ReplaceText.Contains(keywords))
                || SqlFunc.ToString(x.VocabularyStatus).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.WordText))
        {
            exp = exp.And(x => x.WordText != null && x.WordText.Contains(queryDto.WordText));
        }

        if (queryDto?.WordCategory.HasValue == true)
        {
            exp = exp.And(x => x.WordCategory == queryDto.WordCategory);
        }

        if (queryDto?.FilterLevel.HasValue == true)
        {
            exp = exp.And(x => x.FilterLevel == queryDto.FilterLevel);
        }

        if (!string.IsNullOrEmpty(queryDto?.ReplaceText))
        {
            exp = exp.And(x => x.ReplaceText != null && x.ReplaceText.Contains(queryDto.ReplaceText));
        }

        if (queryDto?.VocabularyStatus.HasValue == true)
        {
            exp = exp.And(x => x.VocabularyStatus == queryDto.VocabularyStatus);
        }
        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt >= queryDto.CreatedAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt <= queryDto.CreatedAtEnd);
        }


        return exp.ToExpression();
    }
}
