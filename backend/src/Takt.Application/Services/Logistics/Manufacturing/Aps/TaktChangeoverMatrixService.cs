// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Aps
// 文件名称：TaktChangeoverMatrixService.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：换型矩阵应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Aps;
using Takt.Domain.Entities.Logistics.Manufacturing.Aps;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Aps;

/// <summary>
/// 换型矩阵应用服务
/// </summary>
public class TaktChangeoverMatrixService : TaktServiceBase, ITaktChangeoverMatrixService
{
    private readonly ITaktCompanyRepository<TaktChangeoverMatrix> _changeoverMatrixRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="changeoverMatrixRepository">换型矩阵仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktChangeoverMatrixService(
        ITaktCompanyRepository<TaktChangeoverMatrix> changeoverMatrixRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _changeoverMatrixRepository = changeoverMatrixRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取换型矩阵列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktChangeoverMatrixDto>> GetChangeoverMatrixListAsync(TaktChangeoverMatrixQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _changeoverMatrixRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktChangeoverMatrixDto>.Create(
            data.Adapt<List<TaktChangeoverMatrixDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取换型矩阵
    /// </summary>
    /// <param name="id">换型矩阵ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktChangeoverMatrixDto?> GetChangeoverMatrixByIdAsync(long id)
    {
        var entity = await _changeoverMatrixRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktChangeoverMatrixDto>();
    }

    /// <summary>
    /// 获取换型矩阵选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetChangeoverMatrixOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _changeoverMatrixRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.MatrixStatus == 1,
            x => x.PlantCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.PlantCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建换型矩阵
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktChangeoverMatrixDto> CreateChangeoverMatrixAsync(TaktChangeoverMatrixCreateDto dto)
    {
        var entity = dto.Adapt<TaktChangeoverMatrix>();
        var isUnique_ix_takt_logistics_manufacturing_scheduling_changeover_unique = await _uniqueValidator.IsUniqueAsync(
            _changeoverMatrixRepository,
            x => x.PlantCode == entity.PlantCode
                && x.WorkCenterCode == entity.WorkCenterCode
                && x.FromMaterialCode == entity.FromMaterialCode
                && x.ToMaterialCode == entity.ToMaterialCode);
        if (!isUnique_ix_takt_logistics_manufacturing_scheduling_changeover_unique)
        {
            throw new TaktBusinessException("换型矩阵的PlantCode、WorkCenterCode、FromMaterialCode、ToMaterialCode已存在");
        }
        entity = await _changeoverMatrixRepository.CreateAsync(entity);
        return await GetChangeoverMatrixByIdAsync(entity.Id) ?? entity.Adapt<TaktChangeoverMatrixDto>();
    }

    /// <summary>
    /// 更新换型矩阵
    /// </summary>
    /// <param name="id">换型矩阵ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktChangeoverMatrixDto> UpdateChangeoverMatrixAsync(long id, TaktChangeoverMatrixUpdateDto dto)
    {
        var entity = await _changeoverMatrixRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("换型矩阵不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_scheduling_changeover_unique = await _uniqueValidator.IsUniqueAsync(
            _changeoverMatrixRepository,
            x => x.PlantCode == entity.PlantCode
                && x.WorkCenterCode == entity.WorkCenterCode
                && x.FromMaterialCode == entity.FromMaterialCode
                && x.ToMaterialCode == entity.ToMaterialCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_scheduling_changeover_unique)
        {
            throw new TaktBusinessException("换型矩阵的PlantCode、WorkCenterCode、FromMaterialCode、ToMaterialCode已存在");
        }
        await _changeoverMatrixRepository.UpdateAsync(entity);
        return await GetChangeoverMatrixByIdAsync(id) ?? throw new TaktBusinessException("换型矩阵不存在");
    }

    /// <summary>
    /// 删除换型矩阵
    /// </summary>
    /// <param name="id">换型矩阵ID</param>
    /// <returns>任务</returns>
    public async Task DeleteChangeoverMatrixByIdAsync(long id)
    {
        var deleted = await _changeoverMatrixRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("换型矩阵不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除换型矩阵
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteChangeoverMatrixBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteChangeoverMatrixByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新换型矩阵状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktChangeoverMatrixDto> UpdateChangeoverMatrixStatusAsync(TaktChangeoverMatrixStatusDto dto)
    {
        var entity = await _changeoverMatrixRepository.GetByIdAsync(dto.ChangeoverMatrixId);
        if (entity == null)
        {
            throw new TaktBusinessException("换型矩阵不存在");
        }
        entity.MatrixStatus = dto.MatrixStatus;
        await _changeoverMatrixRepository.UpdateAsync(entity);
        return await GetChangeoverMatrixByIdAsync(dto.ChangeoverMatrixId) ?? throw new TaktBusinessException("换型矩阵不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetChangeoverMatrixTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktChangeoverMatrixTemplateDto>(
            sheetName ?? "换型矩阵导入模板",
            fileName ?? "换型矩阵导入模板.xlsx");
    }

    /// <summary>
    /// 导入换型矩阵
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportChangeoverMatrixAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktChangeoverMatrixImportDto>(fileStream, sheetName ?? "换型矩阵导入模板");
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
                var entity = rows[i].Adapt<TaktChangeoverMatrix>();
                var importKey = $"{entity.PlantCode}|{entity.WorkCenterCode}|{entity.FromMaterialCode}|{entity.ToMaterialCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、WorkCenterCode、FromMaterialCode、ToMaterialCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_scheduling_changeover_unique = await _uniqueValidator.IsUniqueAsync(
                    _changeoverMatrixRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.WorkCenterCode == entity.WorkCenterCode
                        && x.FromMaterialCode == entity.FromMaterialCode
                        && x.ToMaterialCode == entity.ToMaterialCode);
                if (!isUnique_ix_takt_logistics_manufacturing_scheduling_changeover_unique)
                {
                    throw new TaktBusinessException("换型矩阵的PlantCode、WorkCenterCode、FromMaterialCode、ToMaterialCode已存在");
                }
                await _changeoverMatrixRepository.CreateAsync(entity);
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
    /// 导出换型矩阵
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportChangeoverMatrixAsync(TaktChangeoverMatrixQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktChangeoverMatrixQueryDto());
        var list = await _changeoverMatrixRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktChangeoverMatrixExportDto>(),
                sheetName ?? "换型矩阵数据",
                fileName ?? "换型矩阵导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktChangeoverMatrixExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "换型矩阵数据",
            fileName ?? "换型矩阵导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建换型矩阵查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktChangeoverMatrix, bool>> QueryExpression(TaktChangeoverMatrixQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktChangeoverMatrix>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.WorkCenterCode != null && x.WorkCenterCode.Contains(keywords))
                || (x.FromMaterialCode != null && x.FromMaterialCode.Contains(keywords))
                || (x.ToMaterialCode != null && x.ToMaterialCode.Contains(keywords))
                || SqlFunc.ToString(x.ChangeoverMinutes).Contains(keywords)
                || SqlFunc.ToString(x.MatrixStatus).Contains(keywords)
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.WorkCenterCode))
        {
            exp = exp.And(x => x.WorkCenterCode != null && x.WorkCenterCode.Contains(queryDto.WorkCenterCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.FromMaterialCode))
        {
            exp = exp.And(x => x.FromMaterialCode != null && x.FromMaterialCode.Contains(queryDto.FromMaterialCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ToMaterialCode))
        {
            exp = exp.And(x => x.ToMaterialCode != null && x.ToMaterialCode.Contains(queryDto.ToMaterialCode));
        }

        if (queryDto?.ChangeoverMinutes.HasValue == true)
        {
            exp = exp.And(x => x.ChangeoverMinutes == queryDto.ChangeoverMinutes);
        }

        if (queryDto?.MatrixStatus.HasValue == true)
        {
            exp = exp.And(x => x.MatrixStatus == queryDto.MatrixStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.CultureCode))
        {
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(queryDto.CultureCode));
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
