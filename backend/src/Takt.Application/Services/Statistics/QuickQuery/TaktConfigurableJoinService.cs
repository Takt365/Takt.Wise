// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Statistics.QuickQuery
// 文件名称：TaktConfigurableJoinService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：定制报表关联应用服务实现
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
/// 定制报表关联应用服务
/// </summary>
public class TaktConfigurableJoinService : TaktServiceBase, ITaktConfigurableJoinService
{
    private readonly ITaktCompanyRepository<TaktConfigurableJoin> _configurableJoinRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="configurableJoinRepository">定制报表关联仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktConfigurableJoinService(
        ITaktCompanyRepository<TaktConfigurableJoin> configurableJoinRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _configurableJoinRepository = configurableJoinRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取定制报表关联列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktConfigurableJoinDto>> GetConfigurableJoinListAsync(TaktConfigurableJoinQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktConfigurableJoinDto>.Create(
                new List<TaktConfigurableJoinDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _configurableJoinRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktConfigurableJoinDto>.Create(
            data.Adapt<List<TaktConfigurableJoinDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取定制报表关联
    /// </summary>
    /// <param name="id">定制报表关联ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktConfigurableJoinDto?> GetConfigurableJoinByIdAsync(long id)
    {
        var entity = await _configurableJoinRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktConfigurableJoinDto>();
    }

    /// <summary>
    /// 获取定制报表关联选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetConfigurableJoinOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _configurableJoinRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.LeftColumnName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.LeftColumnName,
            DictLabel = e.LeftColumnName,
        }).ToList();
    }

    /// <summary>
    /// 创建定制报表关联
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktConfigurableJoinDto> CreateConfigurableJoinAsync(TaktConfigurableJoinCreateDto dto)
    {
        var entity = dto.Adapt<TaktConfigurableJoin>();
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _configurableJoinRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ConfigurableId == entity.ConfigurableId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.ConfigurableId, maxSort);
        }
        entity = await _configurableJoinRepository.CreateAsync(entity);
        return await GetConfigurableJoinByIdAsync(entity.Id) ?? entity.Adapt<TaktConfigurableJoinDto>();
    }

    /// <summary>
    /// 更新定制报表关联
    /// </summary>
    /// <param name="id">定制报表关联ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktConfigurableJoinDto> UpdateConfigurableJoinAsync(long id, TaktConfigurableJoinUpdateDto dto)
    {
        var entity = await _configurableJoinRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("定制报表关联不存在");
        }
        dto.Adapt(entity);
        await _configurableJoinRepository.UpdateAsync(entity);
        return await GetConfigurableJoinByIdAsync(id) ?? throw new TaktBusinessException("定制报表关联不存在");
    }

    /// <summary>
    /// 删除定制报表关联
    /// </summary>
    /// <param name="id">定制报表关联ID</param>
    /// <returns>任务</returns>
    public async Task DeleteConfigurableJoinByIdAsync(long id)
    {
        var deleted = await _configurableJoinRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("定制报表关联不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除定制报表关联
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteConfigurableJoinBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteConfigurableJoinByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新定制报表关联排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktConfigurableJoinDto> UpdateConfigurableJoinSortAsync(TaktConfigurableJoinSortDto dto)
    {
        var entity = await _configurableJoinRepository.GetByIdAsync(dto.ConfigurableJoinId);
        if (entity == null)
        {
            throw new TaktBusinessException("定制报表关联不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _configurableJoinRepository.UpdateAsync(entity);
        return await GetConfigurableJoinByIdAsync(dto.ConfigurableJoinId) ?? throw new TaktBusinessException("定制报表关联不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetConfigurableJoinTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktConfigurableJoinTemplateDto>(
            sheetName ?? "定制报表关联导入模板",
            fileName ?? "定制报表关联导入模板.xlsx");
    }

    /// <summary>
    /// 导入定制报表关联
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportConfigurableJoinAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktConfigurableJoinImportDto>(fileStream, sheetName ?? "定制报表关联导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktConfigurableJoin>();
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _configurableJoinRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ConfigurableId == entity.ConfigurableId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.ConfigurableId, maxSort);
                }
                await _configurableJoinRepository.CreateAsync(entity);
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
    /// 导出定制报表关联
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportConfigurableJoinAsync(TaktConfigurableJoinQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktConfigurableJoinQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktConfigurableJoinExportDto>(),
                sheetName ?? "定制报表关联数据",
                fileName ?? "定制报表关联导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _configurableJoinRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktConfigurableJoinExportDto>(),
                sheetName ?? "定制报表关联数据",
                fileName ?? "定制报表关联导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktConfigurableJoinExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "定制报表关联数据",
            fileName ?? "定制报表关联导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建定制报表关联查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktConfigurableJoin, bool>> QueryExpression(TaktConfigurableJoinQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktConfigurableJoin>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.LeftSourceAlias != null && x.LeftSourceAlias.Contains(keywords))
                || (x.LeftColumnName != null && x.LeftColumnName.Contains(keywords))
                || (x.RightSourceAlias != null && x.RightSourceAlias.Contains(keywords))
                || (x.RightColumnName != null && x.RightColumnName.Contains(keywords))
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

        if (queryDto?.JoinType.HasValue == true)
        {
            var joinType = queryDto.JoinType.Value;
            exp = exp.And(x => x.JoinType == joinType);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.LeftSourceAlias))
        {
            var leftSourceAlias = queryDto.LeftSourceAlias;
            exp = exp.And(x => x.LeftSourceAlias != null && x.LeftSourceAlias.Contains(leftSourceAlias));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.LeftColumnName))
        {
            var leftColumnName = queryDto.LeftColumnName;
            exp = exp.And(x => x.LeftColumnName != null && x.LeftColumnName.Contains(leftColumnName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RightSourceAlias))
        {
            var rightSourceAlias = queryDto.RightSourceAlias;
            exp = exp.And(x => x.RightSourceAlias != null && x.RightSourceAlias.Contains(rightSourceAlias));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RightColumnName))
        {
            var rightColumnName = queryDto.RightColumnName;
            exp = exp.And(x => x.RightColumnName != null && x.RightColumnName.Contains(rightColumnName));
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
    private static bool HasAnyListQueryFilter(TaktConfigurableJoinQueryDto? queryDto)
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
        if (queryDto.JoinType.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.LeftSourceAlias))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.LeftColumnName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RightSourceAlias))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RightColumnName))
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
