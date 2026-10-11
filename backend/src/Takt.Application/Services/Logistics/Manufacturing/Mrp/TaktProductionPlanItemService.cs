// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Mrp
// 文件名称：TaktProductionPlanItemService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：生产计划明细应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Mrp;
using Takt.Domain.Entities.Logistics.Manufacturing.Mrp;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Mrp;

/// <summary>
/// 生产计划明细应用服务
/// </summary>
public class TaktProductionPlanItemService : TaktServiceBase, ITaktProductionPlanItemService
{
    private readonly ITaktCompanyRepository<TaktProductionPlanItem> _productionPlanItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="productionPlanItemRepository">生产计划明细仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktProductionPlanItemService(
        ITaktCompanyRepository<TaktProductionPlanItem> productionPlanItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _productionPlanItemRepository = productionPlanItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取生产计划明细列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktProductionPlanItemDto>> GetProductionPlanItemListAsync(TaktProductionPlanItemQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktProductionPlanItemDto>.Create(
                new List<TaktProductionPlanItemDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _productionPlanItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktProductionPlanItemDto>.Create(
            data.Adapt<List<TaktProductionPlanItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取生产计划明细
    /// </summary>
    /// <param name="id">生产计划明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductionPlanItemDto?> GetProductionPlanItemByIdAsync(long id)
    {
        var entity = await _productionPlanItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktProductionPlanItemDto>();
    }

    /// <summary>
    /// 获取生产计划明细选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetProductionPlanItemOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _productionPlanItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IsObsolete == 0,
            x => x.ModelName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.ProductionPlanCode,
            DictLabel = e.ModelName ?? e.ProductionPlanCode,
        }).ToList();
    }

    /// <summary>
    /// 创建生产计划明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductionPlanItemDto> CreateProductionPlanItemAsync(TaktProductionPlanItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktProductionPlanItem>();
        entity.IsObsolete = 0;
        var isUnique_ix_takt_logistics_manufacturing_mrp_production_plan_item_line_unique = await _uniqueValidator.IsUniqueAsync(
            _productionPlanItemRepository,
            x => x.ProductionPlanId == entity.ProductionPlanId
                && x.LineNumber == entity.LineNumber
                && x.MaterialCode == entity.MaterialCode);
        if (!isUnique_ix_takt_logistics_manufacturing_mrp_production_plan_item_line_unique)
        {
            throw new TaktBusinessException("生产计划明细的ProductionPlanId、LineNumber、MaterialCode已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _productionPlanItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ProductionPlanId == entity.ProductionPlanId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.ProductionPlanCode) ? entity.ProductionPlanCode : entity.ProductionPlanId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _productionPlanItemRepository.CreateAsync(entity);
        return await GetProductionPlanItemByIdAsync(entity.Id) ?? entity.Adapt<TaktProductionPlanItemDto>();
    }

    /// <summary>
    /// 更新生产计划明细
    /// </summary>
    /// <param name="id">生产计划明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductionPlanItemDto> UpdateProductionPlanItemAsync(long id, TaktProductionPlanItemUpdateDto dto)
    {
        var entity = await _productionPlanItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("生产计划明细不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_mrp_production_plan_item_line_unique = await _uniqueValidator.IsUniqueAsync(
            _productionPlanItemRepository,
            x => x.ProductionPlanId == entity.ProductionPlanId
                && x.LineNumber == entity.LineNumber
                && x.MaterialCode == entity.MaterialCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_mrp_production_plan_item_line_unique)
        {
            throw new TaktBusinessException("生产计划明细的ProductionPlanId、LineNumber、MaterialCode已存在");
        }
        await _productionPlanItemRepository.UpdateAsync(entity);
        return await GetProductionPlanItemByIdAsync(id) ?? throw new TaktBusinessException("生产计划明细不存在");
    }

    /// <summary>
    /// 删除生产计划明细
    /// </summary>
    /// <param name="id">生产计划明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteProductionPlanItemByIdAsync(long id)
    {
        var entity = await _productionPlanItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("生产计划明细不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("生产计划明细不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("生产计划明细已作废");
        }
        entity.IsObsolete = 1;
        await _productionPlanItemRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除生产计划明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteProductionPlanItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteProductionPlanItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新生产计划明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductionPlanItemDto> UpdateProductionPlanItemObsoleteAsync(TaktProductionPlanItemObsoleteDto dto)
    {
        var entity = await _productionPlanItemRepository.GetByIdAsync(dto.ProductionPlanItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("生产计划明细不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("生产计划明细不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _productionPlanItemRepository.UpdateAsync(entity);
        return await GetProductionPlanItemByIdAsync(dto.ProductionPlanItemId) ?? throw new TaktBusinessException("生产计划明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetProductionPlanItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktProductionPlanItemTemplateDto>(
            sheetName ?? "生产计划明细导入模板",
            fileName ?? "生产计划明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入生产计划明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportProductionPlanItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktProductionPlanItemImportDto>(fileStream, sheetName ?? "生产计划明细导入模板");
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
                var entity = rows[i].Adapt<TaktProductionPlanItem>();
                var importKey = $"{entity.ProductionPlanId}|{entity.LineNumber}|{entity.MaterialCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（ProductionPlanId、LineNumber、MaterialCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_mrp_production_plan_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _productionPlanItemRepository,
                    x => x.ProductionPlanId == entity.ProductionPlanId
                        && x.LineNumber == entity.LineNumber
                        && x.MaterialCode == entity.MaterialCode);
                if (!isUnique_ix_takt_logistics_manufacturing_mrp_production_plan_item_line_unique)
                {
                    throw new TaktBusinessException("生产计划明细的ProductionPlanId、LineNumber、MaterialCode已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _productionPlanItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ProductionPlanId == entity.ProductionPlanId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.ProductionPlanCode) ? entity.ProductionPlanCode : entity.ProductionPlanId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _productionPlanItemRepository.CreateAsync(entity);
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
    /// 导出生产计划明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportProductionPlanItemAsync(TaktProductionPlanItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktProductionPlanItemQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktProductionPlanItemExportDto>(),
                sheetName ?? "生产计划明细数据",
                fileName ?? "生产计划明细导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _productionPlanItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktProductionPlanItemExportDto>(),
                sheetName ?? "生产计划明细数据",
                fileName ?? "生产计划明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktProductionPlanItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "生产计划明细数据",
            fileName ?? "生产计划明细导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建生产计划明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktProductionPlanItem, bool>> QueryExpression(TaktProductionPlanItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktProductionPlanItem>();

        if (queryDto?.IsObsolete.HasValue == true)
        {
            exp = exp.And(x => x.IsObsolete == queryDto.IsObsolete);
        }
        else
        {
            exp = exp.And(x => x.IsObsolete == 0);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ProductionPlanCode != null && x.ProductionPlanCode.Contains(keywords))
                || (x.SalesForecastCode != null && x.SalesForecastCode.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.MaterialDescription != null && x.MaterialDescription.Contains(keywords))
                || (x.MaterialSpecification != null && x.MaterialSpecification.Contains(keywords))
                || (x.ModelCode != null && x.ModelCode.Contains(keywords))
                || (x.ModelName != null && x.ModelName.Contains(keywords))
                || (x.PlanUnit != null && x.PlanUnit.Contains(keywords))
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

        if (queryDto?.ProductionPlanId.HasValue == true)
        {
            var productionPlanId = queryDto.ProductionPlanId.Value;
            exp = exp.And(x => x.ProductionPlanId == productionPlanId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProductionPlanCode))
        {
            var productionPlanCode = queryDto.ProductionPlanCode;
            exp = exp.And(x => x.ProductionPlanCode != null && x.ProductionPlanCode.Contains(productionPlanCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            var lineNumber = queryDto.LineNumber.Value;
            exp = exp.And(x => x.LineNumber == lineNumber);
        }

        if (queryDto?.SalesForecastId.HasValue == true)
        {
            var salesForecastId = queryDto.SalesForecastId.Value;
            exp = exp.And(x => x.SalesForecastId == salesForecastId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SalesForecastCode))
        {
            var salesForecastCode = queryDto.SalesForecastCode;
            exp = exp.And(x => x.SalesForecastCode != null && x.SalesForecastCode.Contains(salesForecastCode));
        }

        if (queryDto?.SalesForecastLineNumber.HasValue == true)
        {
            var salesForecastLineNumber = queryDto.SalesForecastLineNumber.Value;
            exp = exp.And(x => x.SalesForecastLineNumber == salesForecastLineNumber);
        }

        if (queryDto?.MaterialRequirementsPlanningItemId.HasValue == true)
        {
            var materialRequirementsPlanningItemId = queryDto.MaterialRequirementsPlanningItemId.Value;
            exp = exp.And(x => x.MaterialRequirementsPlanningItemId == materialRequirementsPlanningItemId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialCode))
        {
            var materialCode = queryDto.MaterialCode;
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(materialCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialDescription))
        {
            var materialDescription = queryDto.MaterialDescription;
            exp = exp.And(x => x.MaterialDescription != null && x.MaterialDescription.Contains(materialDescription));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialSpecification))
        {
            var materialSpecification = queryDto.MaterialSpecification;
            exp = exp.And(x => x.MaterialSpecification != null && x.MaterialSpecification.Contains(materialSpecification));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ModelCode))
        {
            var modelCode = queryDto.ModelCode;
            exp = exp.And(x => x.ModelCode != null && x.ModelCode.Contains(modelCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ModelName))
        {
            var modelName = queryDto.ModelName;
            exp = exp.And(x => x.ModelName != null && x.ModelName.Contains(modelName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PlanUnit))
        {
            var planUnit = queryDto.PlanUnit;
            exp = exp.And(x => x.PlanUnit != null && x.PlanUnit.Contains(planUnit));
        }

        if (queryDto?.PlanQuantity.HasValue == true)
        {
            var planQuantity = queryDto.PlanQuantity.Value;
            exp = exp.And(x => x.PlanQuantity == planQuantity);
        }

        if (queryDto?.ConvertedQuantity.HasValue == true)
        {
            var convertedQuantity = queryDto.ConvertedQuantity.Value;
            exp = exp.And(x => x.ConvertedQuantity == convertedQuantity);
        }

        if (queryDto?.EstimatedUnitCost.HasValue == true)
        {
            var estimatedUnitCost = queryDto.EstimatedUnitCost.Value;
            exp = exp.And(x => x.EstimatedUnitCost == estimatedUnitCost);
        }

        if (queryDto?.EstimatedAmount.HasValue == true)
        {
            var estimatedAmount = queryDto.EstimatedAmount.Value;
            exp = exp.And(x => x.EstimatedAmount == estimatedAmount);
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

        if (queryDto?.PlannedStartDateStart.HasValue == true)
        {
            var plannedStartDateStart = queryDto.PlannedStartDateStart.Value;
            exp = exp.And(x => x.PlannedStartDate >= plannedStartDateStart);
        }

        if (queryDto?.PlannedStartDateEnd.HasValue == true)
        {
            var plannedStartDateEnd = queryDto.PlannedStartDateEnd.Value;
            exp = exp.And(x => x.PlannedStartDate <= plannedStartDateEnd);
        }

        if (queryDto?.PlannedEndDateStart.HasValue == true)
        {
            var plannedEndDateStart = queryDto.PlannedEndDateStart.Value;
            exp = exp.And(x => x.PlannedEndDate >= plannedEndDateStart);
        }

        if (queryDto?.PlannedEndDateEnd.HasValue == true)
        {
            var plannedEndDateEnd = queryDto.PlannedEndDateEnd.Value;
            exp = exp.And(x => x.PlannedEndDate <= plannedEndDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktProductionPlanItemQueryDto? queryDto)
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
        if (queryDto.ProductionPlanId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProductionPlanCode))
        {
            return true;
        }
        if (queryDto.LineNumber.HasValue)
        {
            return true;
        }
        if (queryDto.SalesForecastId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SalesForecastCode))
        {
            return true;
        }
        if (queryDto.SalesForecastLineNumber.HasValue)
        {
            return true;
        }
        if (queryDto.MaterialRequirementsPlanningItemId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialDescription))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialSpecification))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ModelCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ModelName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PlanUnit))
        {
            return true;
        }
        if (queryDto.PlanQuantity.HasValue)
        {
            return true;
        }
        if (queryDto.ConvertedQuantity.HasValue)
        {
            return true;
        }
        if (queryDto.EstimatedUnitCost.HasValue)
        {
            return true;
        }
        if (queryDto.EstimatedAmount.HasValue)
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
        if (queryDto.IsObsolete.HasValue)
        {
            return true;
        }
        if (queryDto.PlannedStartDateStart.HasValue || queryDto.PlannedStartDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.PlannedEndDateStart.HasValue || queryDto.PlannedEndDateEnd.HasValue)
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
