// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：TaktModelDestinationService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：型号目的地应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Materials;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Materials;

/// <summary>
/// 型号目的地应用服务
/// </summary>
public class TaktModelDestinationService : TaktServiceBase, ITaktModelDestinationService
{
    private const int MaxModelDestinationRowsByMaterial = 50;

    private readonly ITaktTenantRepository<TaktModelDestination> _modelDestinationRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="modelDestinationRepository">型号目的地仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktModelDestinationService(
        ITaktTenantRepository<TaktModelDestination> modelDestinationRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _modelDestinationRepository = modelDestinationRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取型号目的地列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktModelDestinationDto>> GetModelDestinationListAsync(TaktModelDestinationQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _modelDestinationRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktModelDestinationDto>.Create(
            data.Adapt<List<TaktModelDestinationDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取型号目的地
    /// </summary>
    /// <param name="id">型号目的地ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktModelDestinationDto?> GetModelDestinationByIdAsync(long id)
    {
        var entity = await _modelDestinationRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode)
        {
            return null;
        }
        return entity.Adapt<TaktModelDestinationDto>();
    }

    /// <summary>
    /// 获取型号目的地选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetModelDestinationOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        var list = await _modelDestinationRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode,
            x => x.SortOrder,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.DestinationCode,
            DictLabel = e.DestinationName ?? e.DestinationCode,
        }).ToList();
    }

    /// <summary>
    /// 获取机种下拉选项（ModelCode 去重）
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>机种下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetModelOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        var list = await _modelDestinationRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode,
            x => x.SortOrder,
            false);
        return list
            .Where(e => !string.IsNullOrWhiteSpace(e.ModelCode))
            .GroupBy(e => e.ModelCode, StringComparer.OrdinalIgnoreCase)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .Select(g =>
            {
                var first = g.First();
                var label = string.IsNullOrWhiteSpace(first.ModelName) ? g.Key : first.ModelName;
                return new TaktSelectOption
                {
                    DictValue = g.Key,
                    DictLabel = label,
                    SortOrder = first.SortOrder,
                };
            })
            .ToList();
    }

    /// <summary>
    /// 根据机种编码获取级联物料选项（MaterialCode 去重）
    /// </summary>
    /// <param name="modelCode">机种编码</param>
    /// <returns>物料下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetMaterialOptionsByModelAsync(string modelCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(modelCode);
        var trimmedModelCode = modelCode.Trim();
        var list = await _modelDestinationRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.ModelCode != null
                && x.ModelCode == trimmedModelCode,
            x => x.SortOrder,
            false);
        return list
            .Where(e => !string.IsNullOrWhiteSpace(e.MaterialCode))
            .GroupBy(e => e.MaterialCode, StringComparer.OrdinalIgnoreCase)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .Select(g =>
            {
                var first = g.First();
                var materialDescription = first.MaterialDescription?.Trim();
                var label = string.IsNullOrWhiteSpace(materialDescription) ? g.Key : $"{g.Key} - {materialDescription}";
                return new TaktSelectOption
                {
                    DictValue = g.Key,
                    DictLabel = label,
                    ExtValue = first.ModelCode,
                    ExtLabel = first.ModelName,
                    SortOrder = first.SortOrder,
                };
            })
            .ToList();
    }

    /// <summary>
    /// 根据物料编码获取机种名称与仕向地信息（首条）
    /// </summary>
    /// <param name="materialCode">物料编码</param>
    /// <returns>型号目的地 DTO；未匹配时返回 null</returns>
    public async Task<TaktModelDestinationDto?> GetModelDestinationByMaterialAsync(string materialCode)
    {
        var list = await GetModelDestinationListByMaterialAsync(materialCode);
        return list.FirstOrDefault();
    }

    /// <summary>
    /// 根据物料编码获取机种仕向列表
    /// </summary>
    /// <param name="materialCode">物料编码</param>
    /// <returns>型号目的地 DTO 列表</returns>
    public async Task<List<TaktModelDestinationDto>> GetModelDestinationListByMaterialAsync(string materialCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(materialCode);
        var list = await QueryModelDestinationsByMaterialAsync(materialCode);
        return list.Adapt<List<TaktModelDestinationDto>>();
    }

    /// <summary>
    /// 根据物料编码获取机种仕向选项列表
    /// </summary>
    /// <param name="materialCode">物料编码</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetModelDestinationOptionsByMaterialAsync(string materialCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(materialCode);
        var list = await QueryModelDestinationsByMaterialAsync(materialCode);
        return list.Select(e =>
        {
            var modelLabel = string.IsNullOrWhiteSpace(e.ModelName) ? e.ModelCode : e.ModelName;
            var destinationLabel = string.IsNullOrWhiteSpace(e.DestinationName) ? e.DestinationCode : e.DestinationName;
            return new TaktSelectOption
            {
                DictValue = e.Id,
                DictLabel = $"{modelLabel} / {destinationLabel}",
                ExtValue = e.ModelCode,
                ExtLabel = e.DestinationCode,
                SortOrder = e.SortOrder,
            };
        }).ToList();
    }

    /// <summary>
    /// 按物料编码查询型号目的地实体列表
    /// </summary>
    /// <param name="materialCode">物料编码</param>
    /// <returns>型号目的地实体列表</returns>
    private async Task<List<TaktModelDestination>> QueryModelDestinationsByMaterialAsync(string materialCode)
    {
        var list = await _modelDestinationRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.MaterialCode == materialCode,
            x => x.SortOrder,
            false);
        return list.Take(MaxModelDestinationRowsByMaterial).ToList();
    }

    /// <summary>
    /// 创建型号目的地
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktModelDestinationDto> CreateModelDestinationAsync(TaktModelDestinationCreateDto dto)
    {
        var entity = dto.Adapt<TaktModelDestination>();
        var isUnique = await _uniqueValidator.IsUniqueAsync(
            _modelDestinationRepository,
            x => x.MaterialCode == entity.MaterialCode
                && x.ModelCode == entity.ModelCode);
        if (!isUnique)
        {
            throw new TaktBusinessException("型号目的地的MaterialCode、ModelCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _modelDestinationRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _modelDestinationRepository.CreateAsync(entity);
        return await GetModelDestinationByIdAsync(entity.Id) ?? entity.Adapt<TaktModelDestinationDto>();
    }

    /// <summary>
    /// 更新型号目的地
    /// </summary>
    /// <param name="id">型号目的地ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktModelDestinationDto> UpdateModelDestinationAsync(long id, TaktModelDestinationUpdateDto dto)
    {
        var entity = await _modelDestinationRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("型号目的地不存在");
        }
        dto.Adapt(entity);
        var isUnique = await _uniqueValidator.IsUniqueAsync(
            _modelDestinationRepository,
            x => x.MaterialCode == entity.MaterialCode
                && x.ModelCode == entity.ModelCode,
            id);
        if (!isUnique)
        {
            throw new TaktBusinessException("型号目的地的MaterialCode、ModelCode已存在");
        }
        await _modelDestinationRepository.UpdateAsync(entity);
        return await GetModelDestinationByIdAsync(id) ?? throw new TaktBusinessException("型号目的地不存在");
    }

    /// <summary>
    /// 删除型号目的地
    /// </summary>
    /// <param name="id">型号目的地ID</param>
    /// <returns>任务</returns>
    public async Task DeleteModelDestinationByIdAsync(long id)
    {
        var deleted = await _modelDestinationRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("型号目的地不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除型号目的地
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteModelDestinationBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteModelDestinationByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新型号目的地排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktModelDestinationDto> UpdateModelDestinationSortAsync(TaktModelDestinationSortDto dto)
    {
        var entity = await _modelDestinationRepository.GetByIdAsync(dto.ModelDestinationId);
        if (entity == null)
        {
            throw new TaktBusinessException("型号目的地不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _modelDestinationRepository.UpdateAsync(entity);
        return await GetModelDestinationByIdAsync(dto.ModelDestinationId) ?? throw new TaktBusinessException("型号目的地不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetModelDestinationTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktModelDestinationTemplateDto>(
            sheetName ?? "型号目的地导入模板",
            fileName ?? "型号目的地导入模板.xlsx");
    }

    /// <summary>
    /// 导入型号目的地
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportModelDestinationAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktModelDestinationImportDto>(fileStream, sheetName ?? "型号目的地导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSortMax = await _modelDestinationRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode,
            x => x.SortOrder);
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktModelDestination>();
                var importKey = $"{entity.MaterialCode}|{entity.ModelCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（MaterialCode、ModelCode）");
                }
                var isUnique = await _uniqueValidator.IsUniqueAsync(
                    _modelDestinationRepository,
                    x => x.MaterialCode == entity.MaterialCode
                        && x.ModelCode == entity.ModelCode);
                if (!isUnique)
                {
                    throw new TaktBusinessException("型号目的地的MaterialCode、ModelCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _modelDestinationRepository.CreateAsync(entity);
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
    /// 导出型号目的地
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportModelDestinationAsync(TaktModelDestinationQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktModelDestinationQueryDto());
        var list = await _modelDestinationRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktModelDestinationExportDto>(),
                sheetName ?? "型号目的地数据",
                fileName ?? "型号目的地导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktModelDestinationExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "型号目的地数据",
            fileName ?? "型号目的地导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建型号目的地查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktModelDestination, bool>> QueryExpression(TaktModelDestinationQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktModelDestination>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.MaterialDescription != null && x.MaterialDescription.Contains(keywords))
                || (x.ModelCode != null && x.ModelCode.Contains(keywords))
                || (x.ModelName != null && x.ModelName.Contains(keywords))
                || (x.DestinationCode != null && x.DestinationCode.Contains(keywords))
                || (x.DestinationName != null && x.DestinationName.Contains(keywords))
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialCode))
        {
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(queryDto.MaterialCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialDescription))
        {
            exp = exp.And(x => x.MaterialDescription != null && x.MaterialDescription.Contains(queryDto.MaterialDescription));
        }

        if (!string.IsNullOrEmpty(queryDto?.ModelCode))
        {
            exp = exp.And(x => x.ModelCode != null && x.ModelCode.Contains(queryDto.ModelCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ModelName))
        {
            exp = exp.And(x => x.ModelName != null && x.ModelName.Contains(queryDto.ModelName));
        }

        if (!string.IsNullOrEmpty(queryDto?.DestinationCode))
        {
            exp = exp.And(x => x.DestinationCode != null && x.DestinationCode.Contains(queryDto.DestinationCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.DestinationName))
        {
            exp = exp.And(x => x.DestinationName != null && x.DestinationName.Contains(queryDto.DestinationName));
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
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
