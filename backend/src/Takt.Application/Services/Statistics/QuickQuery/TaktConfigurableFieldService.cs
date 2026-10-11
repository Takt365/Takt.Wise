// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Statistics.QuickQuery
// 文件名称：TaktConfigurableFieldService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：定制报表输出字段应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Statistics.QuickQuery;
using Takt.Domain.Entities.Statistics.QuickQuery;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Statistics.QuickQuery;

/// <summary>
/// 定制报表输出字段应用服务
/// </summary>
public class TaktConfigurableFieldService : TaktServiceBase, ITaktConfigurableFieldService
{
    private readonly ITaktCompanyRepository<TaktConfigurableField> _configurableFieldRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="configurableFieldRepository">定制报表输出字段仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktConfigurableFieldService(
        ITaktCompanyRepository<TaktConfigurableField> configurableFieldRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _configurableFieldRepository = configurableFieldRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取定制报表输出字段列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktConfigurableFieldDto>> GetConfigurableFieldListAsync(TaktConfigurableFieldQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktConfigurableFieldDto>.Create(
                new List<TaktConfigurableFieldDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _configurableFieldRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktConfigurableFieldDto>.Create(
            data.Adapt<List<TaktConfigurableFieldDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取定制报表输出字段
    /// </summary>
    /// <param name="id">定制报表输出字段ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktConfigurableFieldDto?> GetConfigurableFieldByIdAsync(long id)
    {
        var entity = await _configurableFieldRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktConfigurableFieldDto>();
    }

    /// <summary>
    /// 获取定制报表输出字段选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetConfigurableFieldOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _configurableFieldRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.DisplayName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.DisplayName,
            DictLabel = e.DisplayName,
        }).ToList();
    }

    /// <summary>
    /// 创建定制报表输出字段
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktConfigurableFieldDto> CreateConfigurableFieldAsync(TaktConfigurableFieldCreateDto dto)
    {
        var entity = dto.Adapt<TaktConfigurableField>();
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _configurableFieldRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ConfigurableId == entity.ConfigurableId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.ConfigurableId, maxSort);
        }
        entity = await _configurableFieldRepository.CreateAsync(entity);
        return await GetConfigurableFieldByIdAsync(entity.Id) ?? entity.Adapt<TaktConfigurableFieldDto>();
    }

    /// <summary>
    /// 更新定制报表输出字段
    /// </summary>
    /// <param name="id">定制报表输出字段ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktConfigurableFieldDto> UpdateConfigurableFieldAsync(long id, TaktConfigurableFieldUpdateDto dto)
    {
        var entity = await _configurableFieldRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("定制报表输出字段不存在");
        }
        dto.Adapt(entity);
        await _configurableFieldRepository.UpdateAsync(entity);
        return await GetConfigurableFieldByIdAsync(id) ?? throw new TaktBusinessException("定制报表输出字段不存在");
    }

    /// <summary>
    /// 删除定制报表输出字段
    /// </summary>
    /// <param name="id">定制报表输出字段ID</param>
    /// <returns>任务</returns>
    public async Task DeleteConfigurableFieldByIdAsync(long id)
    {
        var deleted = await _configurableFieldRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("定制报表输出字段不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除定制报表输出字段
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteConfigurableFieldBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteConfigurableFieldByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新定制报表输出字段排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktConfigurableFieldDto> UpdateConfigurableFieldSortAsync(TaktConfigurableFieldSortDto dto)
    {
        var entity = await _configurableFieldRepository.GetByIdAsync(dto.ConfigurableFieldId);
        if (entity == null)
        {
            throw new TaktBusinessException("定制报表输出字段不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _configurableFieldRepository.UpdateAsync(entity);
        return await GetConfigurableFieldByIdAsync(dto.ConfigurableFieldId) ?? throw new TaktBusinessException("定制报表输出字段不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetConfigurableFieldTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktConfigurableFieldTemplateDto>(
            sheetName ?? "定制报表输出字段导入模板",
            fileName ?? "定制报表输出字段导入模板.xlsx");
    }

    /// <summary>
    /// 导入定制报表输出字段
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportConfigurableFieldAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktConfigurableFieldImportDto>(fileStream, sheetName ?? "定制报表输出字段导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktConfigurableField>();
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _configurableFieldRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ConfigurableId == entity.ConfigurableId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.ConfigurableId, maxSort);
                }
                await _configurableFieldRepository.CreateAsync(entity);
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
    /// 导出定制报表输出字段
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportConfigurableFieldAsync(TaktConfigurableFieldQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktConfigurableFieldQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktConfigurableFieldExportDto>(),
                sheetName ?? "定制报表输出字段数据",
                fileName ?? "定制报表输出字段导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _configurableFieldRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktConfigurableFieldExportDto>(),
                sheetName ?? "定制报表输出字段数据",
                fileName ?? "定制报表输出字段导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktConfigurableFieldExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "定制报表输出字段数据",
            fileName ?? "定制报表输出字段导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建定制报表输出字段查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktConfigurableField, bool>> QueryExpression(TaktConfigurableFieldQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktConfigurableField>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.SourceAlias != null && x.SourceAlias.Contains(keywords))
                || (x.ColumnName != null && x.ColumnName.Contains(keywords))
                || (x.DisplayName != null && x.DisplayName.Contains(keywords))
                || (x.OutputAlias != null && x.OutputAlias.Contains(keywords))
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

        if (queryDto?.ConfigurableId.HasValue == true)
        {
            var configurableId = queryDto.ConfigurableId.Value;
            exp = exp.And(x => x.ConfigurableId == configurableId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SourceAlias))
        {
            var sourceAlias = queryDto.SourceAlias;
            exp = exp.And(x => x.SourceAlias != null && x.SourceAlias.Contains(sourceAlias));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ColumnName))
        {
            var columnName = queryDto.ColumnName;
            exp = exp.And(x => x.ColumnName != null && x.ColumnName.Contains(columnName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DisplayName))
        {
            var displayName = queryDto.DisplayName;
            exp = exp.And(x => x.DisplayName != null && x.DisplayName.Contains(displayName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.OutputAlias))
        {
            var outputAlias = queryDto.OutputAlias;
            exp = exp.And(x => x.OutputAlias != null && x.OutputAlias.Contains(outputAlias));
        }

        if (queryDto?.AggregateFunc.HasValue == true)
        {
            var aggregateFunc = queryDto.AggregateFunc.Value;
            exp = exp.And(x => x.AggregateFunc == aggregateFunc);
        }

        if (queryDto?.IsVisible.HasValue == true)
        {
            var isVisible = queryDto.IsVisible.Value;
            exp = exp.And(x => x.IsVisible == isVisible);
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
    private static bool HasAnyListQueryFilter(TaktConfigurableFieldQueryDto? queryDto)
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
        if (queryDto.ConfigurableId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceAlias))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ColumnName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DisplayName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.OutputAlias))
        {
            return true;
        }
        if (queryDto.AggregateFunc.HasValue)
        {
            return true;
        }
        if (queryDto.IsVisible.HasValue)
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
