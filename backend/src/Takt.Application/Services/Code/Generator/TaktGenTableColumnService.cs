// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Code.Generator
// 文件名称：TaktGenTableColumnService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：代码生成数据表列配置应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Code.Generator;
using Takt.Domain.Entities.Code.Generator;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Code.Generator;

/// <summary>
/// 代码生成数据表列配置应用服务
/// </summary>
public class TaktGenTableColumnService : TaktServiceBase, ITaktGenTableColumnService
{
    private readonly ITaktTenantRepository<TaktGenTableColumn> _genTableColumnRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="genTableColumnRepository">代码生成数据表列配置仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktGenTableColumnService(
        ITaktTenantRepository<TaktGenTableColumn> genTableColumnRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _genTableColumnRepository = genTableColumnRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取代码生成数据表列配置列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktGenTableColumnDto>> GetGenTableColumnListAsync(TaktGenTableColumnQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktGenTableColumnDto>.Create(
                new List<TaktGenTableColumnDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _genTableColumnRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktGenTableColumnDto>.Create(
            data.Adapt<List<TaktGenTableColumnDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取代码生成数据表列配置
    /// </summary>
    /// <param name="id">代码生成数据表列配置ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktGenTableColumnDto?> GetGenTableColumnByIdAsync(long id)
    {
        var entity = await _genTableColumnRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode)
        {
            return null;
        }
        return entity.Adapt<TaktGenTableColumnDto>();
    }

    /// <summary>
    /// 获取代码生成数据表列配置选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetGenTableColumnOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        var list = await _genTableColumnRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode,
            x => x.DatabaseColumnName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.DatabaseColumnName,
            DictLabel = e.DatabaseColumnName,
        }).ToList();
    }

    /// <summary>
    /// 创建代码生成数据表列配置
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktGenTableColumnDto> CreateGenTableColumnAsync(TaktGenTableColumnCreateDto dto)
    {
        var entity = dto.Adapt<TaktGenTableColumn>();
        var isUnique_ix_gen_table_column_column_unique = await _uniqueValidator.IsUniqueAsync(
            _genTableColumnRepository,
            x => x.GenTableId == entity.GenTableId
                && x.DatabaseColumnName == entity.DatabaseColumnName);
        if (!isUnique_ix_gen_table_column_column_unique)
        {
            throw new TaktBusinessException("代码生成数据表列配置的GenTableId、DatabaseColumnName已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _genTableColumnRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.GenTableId == entity.GenTableId,
                x => x.LineNumber);
            var businessCode = entity.GenTableId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _genTableColumnRepository.CreateAsync(entity);
        return await GetGenTableColumnByIdAsync(entity.Id) ?? entity.Adapt<TaktGenTableColumnDto>();
    }

    /// <summary>
    /// 更新代码生成数据表列配置
    /// </summary>
    /// <param name="id">代码生成数据表列配置ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktGenTableColumnDto> UpdateGenTableColumnAsync(long id, TaktGenTableColumnUpdateDto dto)
    {
        var entity = await _genTableColumnRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("代码生成数据表列配置不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_gen_table_column_column_unique = await _uniqueValidator.IsUniqueAsync(
            _genTableColumnRepository,
            x => x.GenTableId == entity.GenTableId
                && x.DatabaseColumnName == entity.DatabaseColumnName,
            id);
        if (!isUnique_ix_gen_table_column_column_unique)
        {
            throw new TaktBusinessException("代码生成数据表列配置的GenTableId、DatabaseColumnName已存在");
        }
        await _genTableColumnRepository.UpdateAsync(entity);
        return await GetGenTableColumnByIdAsync(id) ?? throw new TaktBusinessException("代码生成数据表列配置不存在");
    }

    /// <summary>
    /// 删除代码生成数据表列配置
    /// </summary>
    /// <param name="id">代码生成数据表列配置ID</param>
    /// <returns>任务</returns>
    public async Task DeleteGenTableColumnByIdAsync(long id)
    {
        var deleted = await _genTableColumnRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("代码生成数据表列配置不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除代码生成数据表列配置
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteGenTableColumnBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteGenTableColumnByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetGenTableColumnTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktGenTableColumnTemplateDto>(
            sheetName ?? "代码生成数据表列配置导入模板",
            fileName ?? "代码生成数据表列配置导入模板.xlsx");
    }

    /// <summary>
    /// 导入代码生成数据表列配置
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportGenTableColumnAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktGenTableColumnImportDto>(fileStream, sheetName ?? "代码生成数据表列配置导入模板");
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
                var entity = rows[i].Adapt<TaktGenTableColumn>();
                var importKey = $"{entity.GenTableId}|{entity.DatabaseColumnName}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（GenTableId、DatabaseColumnName）");
                }
                var isUnique_ix_gen_table_column_column_unique = await _uniqueValidator.IsUniqueAsync(
                    _genTableColumnRepository,
                    x => x.GenTableId == entity.GenTableId
                        && x.DatabaseColumnName == entity.DatabaseColumnName);
                if (!isUnique_ix_gen_table_column_column_unique)
                {
                    throw new TaktBusinessException("代码生成数据表列配置的GenTableId、DatabaseColumnName已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _genTableColumnRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.GenTableId == entity.GenTableId,
                        x => x.LineNumber);
                    var businessCode = entity.GenTableId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _genTableColumnRepository.CreateAsync(entity);
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
    /// 导出代码生成数据表列配置
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportGenTableColumnAsync(TaktGenTableColumnQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktGenTableColumnQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktGenTableColumnExportDto>(),
                sheetName ?? "代码生成数据表列配置数据",
                fileName ?? "代码生成数据表列配置导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _genTableColumnRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktGenTableColumnExportDto>(),
                sheetName ?? "代码生成数据表列配置数据",
                fileName ?? "代码生成数据表列配置导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktGenTableColumnExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "代码生成数据表列配置数据",
            fileName ?? "代码生成数据表列配置导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建代码生成数据表列配置查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktGenTableColumn, bool>> QueryExpression(TaktGenTableColumnQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktGenTableColumn>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.DatabaseColumnName != null && x.DatabaseColumnName.Contains(keywords))
                || (x.ColumnComment != null && x.ColumnComment.Contains(keywords))
                || (x.DatabaseDataType != null && x.DatabaseDataType.Contains(keywords))
                || (x.CsharpDataType != null && x.CsharpDataType.Contains(keywords))
                || (x.CsharpColumnName != null && x.CsharpColumnName.Contains(keywords))
                || (x.QueryType != null && x.QueryType.Contains(keywords))
                || (x.HtmlType != null && x.HtmlType.Contains(keywords))
                || (x.DictType != null && x.DictType.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
            );
        }

        if (queryDto?.GenTableId.HasValue == true)
        {
            var genTableId = queryDto.GenTableId.Value;
            exp = exp.And(x => x.GenTableId == genTableId);
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            var lineNumber = queryDto.LineNumber.Value;
            exp = exp.And(x => x.LineNumber == lineNumber);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DatabaseColumnName))
        {
            var databaseColumnName = queryDto.DatabaseColumnName;
            exp = exp.And(x => x.DatabaseColumnName != null && x.DatabaseColumnName.Contains(databaseColumnName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ColumnComment))
        {
            var columnComment = queryDto.ColumnComment;
            exp = exp.And(x => x.ColumnComment != null && x.ColumnComment.Contains(columnComment));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DatabaseDataType))
        {
            var databaseDataType = queryDto.DatabaseDataType;
            exp = exp.And(x => x.DatabaseDataType != null && x.DatabaseDataType.Contains(databaseDataType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CsharpDataType))
        {
            var csharpDataType = queryDto.CsharpDataType;
            exp = exp.And(x => x.CsharpDataType != null && x.CsharpDataType.Contains(csharpDataType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CsharpColumnName))
        {
            var csharpColumnName = queryDto.CsharpColumnName;
            exp = exp.And(x => x.CsharpColumnName != null && x.CsharpColumnName.Contains(csharpColumnName));
        }

        if (queryDto?.Length.HasValue == true)
        {
            var length = queryDto.Length.Value;
            exp = exp.And(x => x.Length == length);
        }

        if (queryDto?.DecimalDigits.HasValue == true)
        {
            var decimalDigits = queryDto.DecimalDigits.Value;
            exp = exp.And(x => x.DecimalDigits == decimalDigits);
        }

        if (queryDto?.IsPk.HasValue == true)
        {
            var isPk = queryDto.IsPk.Value;
            exp = exp.And(x => x.IsPk == isPk);
        }

        if (queryDto?.IsIncrement.HasValue == true)
        {
            var isIncrement = queryDto.IsIncrement.Value;
            exp = exp.And(x => x.IsIncrement == isIncrement);
        }

        if (queryDto?.IsRequired.HasValue == true)
        {
            var isRequired = queryDto.IsRequired.Value;
            exp = exp.And(x => x.IsRequired == isRequired);
        }

        if (queryDto?.IsCreate.HasValue == true)
        {
            var isCreate = queryDto.IsCreate.Value;
            exp = exp.And(x => x.IsCreate == isCreate);
        }

        if (queryDto?.IsUpdate.HasValue == true)
        {
            var isUpdate = queryDto.IsUpdate.Value;
            exp = exp.And(x => x.IsUpdate == isUpdate);
        }

        if (queryDto?.IsUnique.HasValue == true)
        {
            var isUnique = queryDto.IsUnique.Value;
            exp = exp.And(x => x.IsUnique == isUnique);
        }

        if (queryDto?.IsList.HasValue == true)
        {
            var isList = queryDto.IsList.Value;
            exp = exp.And(x => x.IsList == isList);
        }

        if (queryDto?.IsExport.HasValue == true)
        {
            var isExport = queryDto.IsExport.Value;
            exp = exp.And(x => x.IsExport == isExport);
        }

        if (queryDto?.IsSort.HasValue == true)
        {
            var isSort = queryDto.IsSort.Value;
            exp = exp.And(x => x.IsSort == isSort);
        }

        if (queryDto?.IsQuery.HasValue == true)
        {
            var isQuery = queryDto.IsQuery.Value;
            exp = exp.And(x => x.IsQuery == isQuery);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.QueryType))
        {
            var queryType = queryDto.QueryType;
            exp = exp.And(x => x.QueryType != null && x.QueryType.Contains(queryType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.HtmlType))
        {
            var htmlType = queryDto.HtmlType;
            exp = exp.And(x => x.HtmlType != null && x.HtmlType.Contains(htmlType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DictType))
        {
            var dictType = queryDto.DictType;
            exp = exp.And(x => x.DictType != null && x.DictType.Contains(dictType));
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
    private static bool HasAnyListQueryFilter(TaktGenTableColumnQueryDto? queryDto)
    {
        if (queryDto == null)
        {
            return false;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.KeyWords))
        {
            return true;
        }
        if (queryDto.GenTableId.HasValue)
        {
            return true;
        }
        if (queryDto.LineNumber.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DatabaseColumnName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ColumnComment))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DatabaseDataType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CsharpDataType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CsharpColumnName))
        {
            return true;
        }
        if (queryDto.Length.HasValue)
        {
            return true;
        }
        if (queryDto.DecimalDigits.HasValue)
        {
            return true;
        }
        if (queryDto.IsPk.HasValue)
        {
            return true;
        }
        if (queryDto.IsIncrement.HasValue)
        {
            return true;
        }
        if (queryDto.IsRequired.HasValue)
        {
            return true;
        }
        if (queryDto.IsCreate.HasValue)
        {
            return true;
        }
        if (queryDto.IsUpdate.HasValue)
        {
            return true;
        }
        if (queryDto.IsUnique.HasValue)
        {
            return true;
        }
        if (queryDto.IsList.HasValue)
        {
            return true;
        }
        if (queryDto.IsExport.HasValue)
        {
            return true;
        }
        if (queryDto.IsSort.HasValue)
        {
            return true;
        }
        if (queryDto.IsQuery.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.QueryType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.HtmlType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DictType))
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
