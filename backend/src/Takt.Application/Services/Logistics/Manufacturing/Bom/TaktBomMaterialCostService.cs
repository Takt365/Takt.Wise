// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktBomMaterialCostService.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Cursor AI)
// 功能描述：材料成本应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// 材料成本应用服务
/// </summary>
public class TaktBomMaterialCostService : TaktServiceBase, ITaktBomMaterialCostService
{
    private readonly ITaktCompanyRepository<TaktBomMaterialCost> _bomMaterialCostRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="bomMaterialCostRepository">材料成本仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktBomMaterialCostService(
        ITaktCompanyRepository<TaktBomMaterialCost> bomMaterialCostRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _bomMaterialCostRepository = bomMaterialCostRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取材料成本列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktBomMaterialCostDto>> GetBomMaterialCostListAsync(TaktBomMaterialCostQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktBomMaterialCostDto>.Create(
                new List<TaktBomMaterialCostDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        // 默认按核算期间（yyyy-MM）降序，最近期间在前
        var (data, total) = await _bomMaterialCostRepository.GetPagedAsync(
            predicate,
            queryDto.PageIndex,
            queryDto.PageSize,
            x => x.CostingPeriod,
            isDesc: true);
        return TaktPagedResult<TaktBomMaterialCostDto>.Create(
            data.Adapt<List<TaktBomMaterialCostDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取材料成本
    /// </summary>
    /// <param name="id">材料成本ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktBomMaterialCostDto?> GetBomMaterialCostByIdAsync(long id)
    {
        var entity = await _bomMaterialCostRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktBomMaterialCostDto>();
    }

    /// <summary>
    /// 获取材料成本选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetBomMaterialCostOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _bomMaterialCostRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.ModelCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.ModelCode,
            DictLabel = e.ModelCode,
        }).ToList();
    }

    /// <summary>
    /// 创建材料成本
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBomMaterialCostDto> CreateBomMaterialCostAsync(TaktBomMaterialCostCreateDto dto)
    {
        var entity = dto.Adapt<TaktBomMaterialCost>();
        entity.CostingDate = entity.CostingDate.Date;
        var dayStart = entity.CostingDate;
        var dayEnd = dayStart.AddDays(1);
        var isUnique_ix_takt_logistics_manufacturing_bom_material_cost_unique = await _uniqueValidator.IsUniqueAsync(
            _bomMaterialCostRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ProductCode == entity.ProductCode
                && x.CostingDate >= dayStart
                && x.CostingDate < dayEnd);
        if (!isUnique_ix_takt_logistics_manufacturing_bom_material_cost_unique)
        {
            throw new TaktBusinessException("材料成本的PlantCode、ProductCode、CostingDate已存在");
        }
        entity = await _bomMaterialCostRepository.CreateAsync(entity);
        return await GetBomMaterialCostByIdAsync(entity.Id) ?? entity.Adapt<TaktBomMaterialCostDto>();
    }

    /// <summary>
    /// 更新材料成本
    /// </summary>
    /// <param name="id">材料成本ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBomMaterialCostDto> UpdateBomMaterialCostAsync(long id, TaktBomMaterialCostUpdateDto dto)
    {
        var entity = await _bomMaterialCostRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("材料成本不存在");
        }
        dto.Adapt(entity);
        entity.CostingDate = entity.CostingDate.Date;
        var dayStart = entity.CostingDate;
        var dayEnd = dayStart.AddDays(1);
        var isUnique_ix_takt_logistics_manufacturing_bom_material_cost_unique = await _uniqueValidator.IsUniqueAsync(
            _bomMaterialCostRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ProductCode == entity.ProductCode
                && x.CostingDate >= dayStart
                && x.CostingDate < dayEnd,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_bom_material_cost_unique)
        {
            throw new TaktBusinessException("材料成本的PlantCode、ProductCode、CostingDate已存在");
        }
        await _bomMaterialCostRepository.UpdateAsync(entity);
        return await GetBomMaterialCostByIdAsync(id) ?? throw new TaktBusinessException("材料成本不存在");
    }

    /// <summary>
    /// 删除材料成本
    /// </summary>
    /// <param name="id">材料成本ID</param>
    /// <returns>任务</returns>
    public async Task DeleteBomMaterialCostByIdAsync(long id)
    {
        var deleted = await _bomMaterialCostRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("材料成本不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除材料成本
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteBomMaterialCostBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteBomMaterialCostByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetBomMaterialCostTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktBomMaterialCostTemplateDto>(
            sheetName ?? "材料成本导入模板",
            fileName ?? "材料成本导入模板.xlsx");
    }

    /// <summary>
    /// 导入材料成本
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportBomMaterialCostAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktBomMaterialCostImportDto>(fileStream, sheetName ?? "材料成本导入模板");
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
                var entity = rows[i].Adapt<TaktBomMaterialCost>();
                entity.CostingDate = entity.CostingDate.Date;
                var importKey = $"{entity.PlantCode}|{entity.ProductCode}|{entity.CostingDate:yyyy-MM-dd}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、ProductCode、CostingDate）");
                }
                var dayStart = entity.CostingDate;
                var dayEnd = dayStart.AddDays(1);
                var isUnique_ix_takt_logistics_manufacturing_bom_material_cost_unique = await _uniqueValidator.IsUniqueAsync(
                    _bomMaterialCostRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.ProductCode == entity.ProductCode
                        && x.CostingDate >= dayStart
                        && x.CostingDate < dayEnd);
                if (!isUnique_ix_takt_logistics_manufacturing_bom_material_cost_unique)
                {
                    throw new TaktBusinessException("材料成本的PlantCode、ProductCode、CostingDate已存在");
                }
                await _bomMaterialCostRepository.CreateAsync(entity);
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
    /// 导出材料成本
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportBomMaterialCostAsync(TaktBomMaterialCostQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktBomMaterialCostQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktBomMaterialCostExportDto>(),
                sheetName ?? "材料成本数据",
                fileName ?? "材料成本导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _bomMaterialCostRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktBomMaterialCostExportDto>(),
                sheetName ?? "材料成本数据",
                fileName ?? "材料成本导出.xlsx");
        }
        list = list
            .OrderByDescending(x => x.CostingPeriod, StringComparer.Ordinal)
            .ToList();
        var exportData = list.Adapt<List<TaktBomMaterialCostExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "材料成本数据",
            fileName ?? "材料成本导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建材料成本查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktBomMaterialCost, bool>> QueryExpression(TaktBomMaterialCostQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktBomMaterialCost>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ModelCode != null && x.ModelCode.Contains(keywords))
                || (x.MaterialType != null && x.MaterialType.Contains(keywords))
                || (x.ProductCode != null && x.ProductCode.Contains(keywords))
                || (x.ProductDescription != null && x.ProductDescription.Contains(keywords))
                || (x.CurrencyCode != null && x.CurrencyCode.Contains(keywords))
                || (x.CostingPeriod != null && x.CostingPeriod.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.ModelCode))
        {
            var modelCode = queryDto.ModelCode;
            exp = exp.And(x => x.ModelCode != null && x.ModelCode.Contains(modelCode));
        }

        if (queryDto?.ModelMonthlyAverageCost.HasValue == true)
        {
            var modelMonthlyAverageCost = queryDto.ModelMonthlyAverageCost.Value;
            exp = exp.And(x => x.ModelMonthlyAverageCost == modelMonthlyAverageCost);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialType))
        {
            var materialType = queryDto.MaterialType;
            exp = exp.And(x => x.MaterialType != null && x.MaterialType.Contains(materialType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProductCode))
        {
            var productCode = queryDto.ProductCode;
            exp = exp.And(x => x.ProductCode != null && x.ProductCode.Contains(productCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProductDescription))
        {
            var productDescription = queryDto.ProductDescription;
            exp = exp.And(x => x.ProductDescription != null && x.ProductDescription.Contains(productDescription));
        }

        if (queryDto?.ProductMonthlyCost.HasValue == true)
        {
            var productMonthlyCost = queryDto.ProductMonthlyCost.Value;
            exp = exp.And(x => x.ProductMonthlyCost == productMonthlyCost);
        }

        if (queryDto?.ProductMonthlyCalculation.HasValue == true)
        {
            var productMonthlyCalculation = queryDto.ProductMonthlyCalculation.Value;
            exp = exp.And(x => x.ProductMonthlyCalculation == productMonthlyCalculation);
        }

        if (queryDto?.LatestPurchaseCost.HasValue == true)
        {
            var latestPurchaseCost = queryDto.LatestPurchaseCost.Value;
            exp = exp.And(x => x.LatestPurchaseCost == latestPurchaseCost);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CurrencyCode))
        {
            var currencyCode = queryDto.CurrencyCode;
            exp = exp.And(x => x.CurrencyCode != null && x.CurrencyCode.Contains(currencyCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CostingPeriod))
        {
            var costingPeriod = queryDto.CostingPeriod;
            exp = exp.And(x => x.CostingPeriod != null && x.CostingPeriod.Contains(costingPeriod));
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

        if (queryDto?.CostingDateStart.HasValue == true)
        {
            var costingDateStart = queryDto.CostingDateStart.Value;
            exp = exp.And(x => x.CostingDate >= costingDateStart);
        }

        if (queryDto?.CostingDateEnd.HasValue == true)
        {
            var costingDateEnd = queryDto.CostingDateEnd.Value;
            exp = exp.And(x => x.CostingDate <= costingDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktBomMaterialCostQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.ModelCode))
        {
            return true;
        }
        if (queryDto.ModelMonthlyAverageCost.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProductCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProductDescription))
        {
            return true;
        }
        if (queryDto.ProductMonthlyCost.HasValue)
        {
            return true;
        }
        if (queryDto.ProductMonthlyCalculation.HasValue)
        {
            return true;
        }
        if (queryDto.LatestPurchaseCost.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CurrencyCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CostingPeriod))
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
        if (queryDto.CostingDateStart.HasValue || queryDto.CostingDateEnd.HasValue)
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
