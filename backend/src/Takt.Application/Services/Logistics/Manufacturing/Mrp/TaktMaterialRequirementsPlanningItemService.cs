// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Mrp
// 文件名称：TaktMaterialRequirementsPlanningItemService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：物料需求计划MRP明细应用服务实现
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
/// 物料需求计划MRP明细应用服务
/// </summary>
public class TaktMaterialRequirementsPlanningItemService : TaktServiceBase, ITaktMaterialRequirementsPlanningItemService
{
    private readonly ITaktCompanyRepository<TaktMaterialRequirementsPlanningItem> _materialRequirementsPlanningItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="materialRequirementsPlanningItemRepository">物料需求计划MRP明细仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMaterialRequirementsPlanningItemService(
        ITaktCompanyRepository<TaktMaterialRequirementsPlanningItem> materialRequirementsPlanningItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _materialRequirementsPlanningItemRepository = materialRequirementsPlanningItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取物料需求计划MRP明细列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktMaterialRequirementsPlanningItemDto>> GetMaterialRequirementsPlanningItemListAsync(TaktMaterialRequirementsPlanningItemQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktMaterialRequirementsPlanningItemDto>.Create(
                new List<TaktMaterialRequirementsPlanningItemDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _materialRequirementsPlanningItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktMaterialRequirementsPlanningItemDto>.Create(
            data.Adapt<List<TaktMaterialRequirementsPlanningItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取物料需求计划MRP明细
    /// </summary>
    /// <param name="id">物料需求计划MRP明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialRequirementsPlanningItemDto?> GetMaterialRequirementsPlanningItemByIdAsync(long id)
    {
        var entity = await _materialRequirementsPlanningItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktMaterialRequirementsPlanningItemDto>();
    }

    /// <summary>
    /// 获取物料需求计划MRP明细选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetMaterialRequirementsPlanningItemOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _materialRequirementsPlanningItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IsObsolete == 0,
            x => x.MaterialDescription ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.MaterialRequirementsPlanningCode,
            DictLabel = e.MaterialDescription ?? e.MaterialRequirementsPlanningCode,
        }).ToList();
    }

    /// <summary>
    /// 创建物料需求计划MRP明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialRequirementsPlanningItemDto> CreateMaterialRequirementsPlanningItemAsync(TaktMaterialRequirementsPlanningItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktMaterialRequirementsPlanningItem>();
        entity.IsObsolete = 0;
        var isUnique_ix_takt_logistics_manufacturing_mrp_material_requirements_planning_item_line_unique = await _uniqueValidator.IsUniqueAsync(
            _materialRequirementsPlanningItemRepository,
            x => x.MaterialRequirementsPlanningId == entity.MaterialRequirementsPlanningId
                && x.LineNumber == entity.LineNumber
                && x.MaterialCode == entity.MaterialCode);
        if (!isUnique_ix_takt_logistics_manufacturing_mrp_material_requirements_planning_item_line_unique)
        {
            throw new TaktBusinessException("物料需求计划MRP明细的MaterialRequirementsPlanningId、LineNumber、MaterialCode已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _materialRequirementsPlanningItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.MaterialRequirementsPlanningId == entity.MaterialRequirementsPlanningId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.MaterialRequirementsPlanningCode) ? entity.MaterialRequirementsPlanningCode : entity.MaterialRequirementsPlanningId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _materialRequirementsPlanningItemRepository.CreateAsync(entity);
        return await GetMaterialRequirementsPlanningItemByIdAsync(entity.Id) ?? entity.Adapt<TaktMaterialRequirementsPlanningItemDto>();
    }

    /// <summary>
    /// 更新物料需求计划MRP明细
    /// </summary>
    /// <param name="id">物料需求计划MRP明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialRequirementsPlanningItemDto> UpdateMaterialRequirementsPlanningItemAsync(long id, TaktMaterialRequirementsPlanningItemUpdateDto dto)
    {
        var entity = await _materialRequirementsPlanningItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("物料需求计划MRP明细不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_mrp_material_requirements_planning_item_line_unique = await _uniqueValidator.IsUniqueAsync(
            _materialRequirementsPlanningItemRepository,
            x => x.MaterialRequirementsPlanningId == entity.MaterialRequirementsPlanningId
                && x.LineNumber == entity.LineNumber
                && x.MaterialCode == entity.MaterialCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_mrp_material_requirements_planning_item_line_unique)
        {
            throw new TaktBusinessException("物料需求计划MRP明细的MaterialRequirementsPlanningId、LineNumber、MaterialCode已存在");
        }
        await _materialRequirementsPlanningItemRepository.UpdateAsync(entity);
        return await GetMaterialRequirementsPlanningItemByIdAsync(id) ?? throw new TaktBusinessException("物料需求计划MRP明细不存在");
    }

    /// <summary>
    /// 删除物料需求计划MRP明细
    /// </summary>
    /// <param name="id">物料需求计划MRP明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteMaterialRequirementsPlanningItemByIdAsync(long id)
    {
        var entity = await _materialRequirementsPlanningItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("物料需求计划MRP明细不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("物料需求计划MRP明细不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("物料需求计划MRP明细已作废");
        }
        entity.IsObsolete = 1;
        await _materialRequirementsPlanningItemRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除物料需求计划MRP明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteMaterialRequirementsPlanningItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteMaterialRequirementsPlanningItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新物料需求计划MRP明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialRequirementsPlanningItemDto> UpdateMaterialRequirementsPlanningItemObsoleteAsync(TaktMaterialRequirementsPlanningItemObsoleteDto dto)
    {
        var entity = await _materialRequirementsPlanningItemRepository.GetByIdAsync(dto.MaterialRequirementsPlanningItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("物料需求计划MRP明细不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("物料需求计划MRP明细不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _materialRequirementsPlanningItemRepository.UpdateAsync(entity);
        return await GetMaterialRequirementsPlanningItemByIdAsync(dto.MaterialRequirementsPlanningItemId) ?? throw new TaktBusinessException("物料需求计划MRP明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetMaterialRequirementsPlanningItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktMaterialRequirementsPlanningItemTemplateDto>(
            sheetName ?? "物料需求计划MRP明细导入模板",
            fileName ?? "物料需求计划MRP明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入物料需求计划MRP明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportMaterialRequirementsPlanningItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktMaterialRequirementsPlanningItemImportDto>(fileStream, sheetName ?? "物料需求计划MRP明细导入模板");
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
                var entity = rows[i].Adapt<TaktMaterialRequirementsPlanningItem>();
                var importKey = $"{entity.MaterialRequirementsPlanningId}|{entity.LineNumber}|{entity.MaterialCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（MaterialRequirementsPlanningId、LineNumber、MaterialCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_mrp_material_requirements_planning_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _materialRequirementsPlanningItemRepository,
                    x => x.MaterialRequirementsPlanningId == entity.MaterialRequirementsPlanningId
                        && x.LineNumber == entity.LineNumber
                        && x.MaterialCode == entity.MaterialCode);
                if (!isUnique_ix_takt_logistics_manufacturing_mrp_material_requirements_planning_item_line_unique)
                {
                    throw new TaktBusinessException("物料需求计划MRP明细的MaterialRequirementsPlanningId、LineNumber、MaterialCode已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _materialRequirementsPlanningItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.MaterialRequirementsPlanningId == entity.MaterialRequirementsPlanningId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.MaterialRequirementsPlanningCode) ? entity.MaterialRequirementsPlanningCode : entity.MaterialRequirementsPlanningId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _materialRequirementsPlanningItemRepository.CreateAsync(entity);
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
    /// 导出物料需求计划MRP明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportMaterialRequirementsPlanningItemAsync(TaktMaterialRequirementsPlanningItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktMaterialRequirementsPlanningItemQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMaterialRequirementsPlanningItemExportDto>(),
                sheetName ?? "物料需求计划MRP明细数据",
                fileName ?? "物料需求计划MRP明细导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _materialRequirementsPlanningItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMaterialRequirementsPlanningItemExportDto>(),
                sheetName ?? "物料需求计划MRP明细数据",
                fileName ?? "物料需求计划MRP明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktMaterialRequirementsPlanningItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "物料需求计划MRP明细数据",
            fileName ?? "物料需求计划MRP明细导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建物料需求计划MRP明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktMaterialRequirementsPlanningItem, bool>> QueryExpression(TaktMaterialRequirementsPlanningItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktMaterialRequirementsPlanningItem>();

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
                || (x.MaterialRequirementsPlanningCode != null && x.MaterialRequirementsPlanningCode.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.MaterialDescription != null && x.MaterialDescription.Contains(keywords))
                || (x.MaterialSpecification != null && x.MaterialSpecification.Contains(keywords))
                || (x.ModelCode != null && x.ModelCode.Contains(keywords))
                || (x.ModelName != null && x.ModelName.Contains(keywords))
                || (x.ParentMaterialCode != null && x.ParentMaterialCode.Contains(keywords))
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

        if (queryDto?.MaterialRequirementsPlanningId.HasValue == true)
        {
            var materialRequirementsPlanningId = queryDto.MaterialRequirementsPlanningId.Value;
            exp = exp.And(x => x.MaterialRequirementsPlanningId == materialRequirementsPlanningId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialRequirementsPlanningCode))
        {
            var materialRequirementsPlanningCode = queryDto.MaterialRequirementsPlanningCode;
            exp = exp.And(x => x.MaterialRequirementsPlanningCode != null && x.MaterialRequirementsPlanningCode.Contains(materialRequirementsPlanningCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            var lineNumber = queryDto.LineNumber.Value;
            exp = exp.And(x => x.LineNumber == lineNumber);
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

        if (!string.IsNullOrWhiteSpace(queryDto?.ParentMaterialCode))
        {
            var parentMaterialCode = queryDto.ParentMaterialCode;
            exp = exp.And(x => x.ParentMaterialCode != null && x.ParentMaterialCode.Contains(parentMaterialCode));
        }

        if (queryDto?.BomLevel.HasValue == true)
        {
            var bomLevel = queryDto.BomLevel.Value;
            exp = exp.And(x => x.BomLevel == bomLevel);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PlanUnit))
        {
            var planUnit = queryDto.PlanUnit;
            exp = exp.And(x => x.PlanUnit != null && x.PlanUnit.Contains(planUnit));
        }

        if (queryDto?.GrossRequirement.HasValue == true)
        {
            var grossRequirement = queryDto.GrossRequirement.Value;
            exp = exp.And(x => x.GrossRequirement == grossRequirement);
        }

        if (queryDto?.ScheduledReceipts.HasValue == true)
        {
            var scheduledReceipts = queryDto.ScheduledReceipts.Value;
            exp = exp.And(x => x.ScheduledReceipts == scheduledReceipts);
        }

        if (queryDto?.OnHandQuantity.HasValue == true)
        {
            var onHandQuantity = queryDto.OnHandQuantity.Value;
            exp = exp.And(x => x.OnHandQuantity == onHandQuantity);
        }

        if (queryDto?.ProjectedOnHand.HasValue == true)
        {
            var projectedOnHand = queryDto.ProjectedOnHand.Value;
            exp = exp.And(x => x.ProjectedOnHand == projectedOnHand);
        }

        if (queryDto?.NetRequirement.HasValue == true)
        {
            var netRequirement = queryDto.NetRequirement.Value;
            exp = exp.And(x => x.NetRequirement == netRequirement);
        }

        if (queryDto?.ProcurementType.HasValue == true)
        {
            var procurementType = queryDto.ProcurementType.Value;
            exp = exp.And(x => x.ProcurementType == procurementType);
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

        if (queryDto?.RequirementDateStart.HasValue == true)
        {
            var requirementDateStart = queryDto.RequirementDateStart.Value;
            exp = exp.And(x => x.RequirementDate >= requirementDateStart);
        }

        if (queryDto?.RequirementDateEnd.HasValue == true)
        {
            var requirementDateEnd = queryDto.RequirementDateEnd.Value;
            exp = exp.And(x => x.RequirementDate <= requirementDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktMaterialRequirementsPlanningItemQueryDto? queryDto)
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
        if (queryDto.MaterialRequirementsPlanningId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialRequirementsPlanningCode))
        {
            return true;
        }
        if (queryDto.LineNumber.HasValue)
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
        if (!string.IsNullOrWhiteSpace(queryDto.ParentMaterialCode))
        {
            return true;
        }
        if (queryDto.BomLevel.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PlanUnit))
        {
            return true;
        }
        if (queryDto.GrossRequirement.HasValue)
        {
            return true;
        }
        if (queryDto.ScheduledReceipts.HasValue)
        {
            return true;
        }
        if (queryDto.OnHandQuantity.HasValue)
        {
            return true;
        }
        if (queryDto.ProjectedOnHand.HasValue)
        {
            return true;
        }
        if (queryDto.NetRequirement.HasValue)
        {
            return true;
        }
        if (queryDto.ProcurementType.HasValue)
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
        if (queryDto.RequirementDateStart.HasValue || queryDto.RequirementDateEnd.HasValue)
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
