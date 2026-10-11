// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Quality.Operation
// 文件名称：TaktInspectionStandardItemService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：检验标准明细应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Quality.Operation;
using Takt.Domain.Entities.Logistics.Quality.Operation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Quality.Operation;

/// <summary>
/// 检验标准明细应用服务
/// </summary>
public class TaktInspectionStandardItemService : TaktServiceBase, ITaktInspectionStandardItemService
{
    private readonly ITaktCompanyRepository<TaktInspectionStandardItem> _inspectionStandardItemRepository;
    private readonly ITaktCompanyRepository<TaktInspectionStandard> _inspectionStandardRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="inspectionStandardItemRepository">检验标准明细仓储</param>
    /// <param name="inspectionStandardRepository">检验标准仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktInspectionStandardItemService(
        ITaktCompanyRepository<TaktInspectionStandardItem> inspectionStandardItemRepository,
        ITaktCompanyRepository<TaktInspectionStandard> inspectionStandardRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _inspectionStandardItemRepository = inspectionStandardItemRepository;
        _inspectionStandardRepository = inspectionStandardRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取检验标准明细列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktInspectionStandardItemDto>> GetInspectionStandardItemListAsync(TaktInspectionStandardItemQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktInspectionStandardItemDto>.Create(
                new List<TaktInspectionStandardItemDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _inspectionStandardItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktInspectionStandardItemDto>.Create(
            data.Adapt<List<TaktInspectionStandardItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取检验标准明细
    /// </summary>
    /// <param name="id">检验标准明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktInspectionStandardItemDto?> GetInspectionStandardItemByIdAsync(long id)
    {
        var entity = await _inspectionStandardItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktInspectionStandardItemDto>();
    }

    /// <summary>
    /// 获取检验标准明细选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetInspectionStandardItemOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _inspectionStandardItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IsObsolete == 0,
            x => x.ItemName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.ItemCode,
            DictLabel = e.ItemName ?? e.ItemCode,
        }).ToList();
    }

    /// <summary>
    /// 创建检验标准明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktInspectionStandardItemDto> CreateInspectionStandardItemAsync(TaktInspectionStandardItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktInspectionStandardItem>();
        entity.IsObsolete = 0;
        await StampInspectionStandardItemInspectionStandardAsync(entity, dto);
        var isUnique_ix_takt_logistics_quality_inspection_standard_item_unique = await _uniqueValidator.IsUniqueAsync(
            _inspectionStandardItemRepository,
            x => x.InspectionStandardId == entity.InspectionStandardId
                && x.LineNumber == entity.LineNumber
                && x.ItemCode == entity.ItemCode
                && x.ItemType == entity.ItemType);
        if (!isUnique_ix_takt_logistics_quality_inspection_standard_item_unique)
        {
            throw new TaktBusinessException("检验标准明细的InspectionStandardId、LineNumber、ItemCode、ItemType已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _inspectionStandardItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.InspectionStandardId == entity.InspectionStandardId,
                x => x.LineNumber);
            var businessCode = entity.InspectionStandardId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _inspectionStandardItemRepository.CreateAsync(entity);
        return await GetInspectionStandardItemByIdAsync(entity.Id) ?? entity.Adapt<TaktInspectionStandardItemDto>();
    }

    /// <summary>
    /// 更新检验标准明细
    /// </summary>
    /// <param name="id">检验标准明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktInspectionStandardItemDto> UpdateInspectionStandardItemAsync(long id, TaktInspectionStandardItemUpdateDto dto)
    {
        var entity = await _inspectionStandardItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("检验标准明细不存在");
        }
        dto.Adapt(entity);
        await StampInspectionStandardItemInspectionStandardAsync(entity, dto);
        var isUnique_ix_takt_logistics_quality_inspection_standard_item_unique = await _uniqueValidator.IsUniqueAsync(
            _inspectionStandardItemRepository,
            x => x.InspectionStandardId == entity.InspectionStandardId
                && x.LineNumber == entity.LineNumber
                && x.ItemCode == entity.ItemCode
                && x.ItemType == entity.ItemType,
            id);
        if (!isUnique_ix_takt_logistics_quality_inspection_standard_item_unique)
        {
            throw new TaktBusinessException("检验标准明细的InspectionStandardId、LineNumber、ItemCode、ItemType已存在");
        }
        await _inspectionStandardItemRepository.UpdateAsync(entity);
        return await GetInspectionStandardItemByIdAsync(id) ?? throw new TaktBusinessException("检验标准明细不存在");
    }

    /// <summary>
    /// 删除检验标准明细
    /// </summary>
    /// <param name="id">检验标准明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteInspectionStandardItemByIdAsync(long id)
    {
        var entity = await _inspectionStandardItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("检验标准明细不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("检验标准明细不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("检验标准明细已作废");
        }
        entity.IsObsolete = 1;
        await _inspectionStandardItemRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除检验标准明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteInspectionStandardItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteInspectionStandardItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新检验标准明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktInspectionStandardItemDto> UpdateInspectionStandardItemObsoleteAsync(TaktInspectionStandardItemObsoleteDto dto)
    {
        var entity = await _inspectionStandardItemRepository.GetByIdAsync(dto.InspectionStandardItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("检验标准明细不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("检验标准明细不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _inspectionStandardItemRepository.UpdateAsync(entity);
        return await GetInspectionStandardItemByIdAsync(dto.InspectionStandardItemId) ?? throw new TaktBusinessException("检验标准明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetInspectionStandardItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktInspectionStandardItemTemplateDto>(
            sheetName ?? "检验标准明细导入模板",
            fileName ?? "检验标准明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入检验标准明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportInspectionStandardItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktInspectionStandardItemImportDto>(fileStream, sheetName ?? "检验标准明细导入模板");
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
                var entity = rows[i].Adapt<TaktInspectionStandardItem>();
                var importDto = rows[i].Adapt<TaktInspectionStandardItemCreateDto>();
                await StampInspectionStandardItemInspectionStandardAsync(entity, importDto);
                var importKey = $"{entity.InspectionStandardId}|{entity.LineNumber}|{entity.ItemCode}|{entity.ItemType}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（InspectionStandardId、LineNumber、ItemCode、ItemType）");
                }
                var isUnique_ix_takt_logistics_quality_inspection_standard_item_unique = await _uniqueValidator.IsUniqueAsync(
                    _inspectionStandardItemRepository,
                    x => x.InspectionStandardId == entity.InspectionStandardId
                        && x.LineNumber == entity.LineNumber
                        && x.ItemCode == entity.ItemCode
                        && x.ItemType == entity.ItemType);
                if (!isUnique_ix_takt_logistics_quality_inspection_standard_item_unique)
                {
                    throw new TaktBusinessException("检验标准明细的InspectionStandardId、LineNumber、ItemCode、ItemType已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _inspectionStandardItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.InspectionStandardId == entity.InspectionStandardId,
                        x => x.LineNumber);
                    var businessCode = entity.InspectionStandardId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _inspectionStandardItemRepository.CreateAsync(entity);
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
    /// 导出检验标准明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportInspectionStandardItemAsync(TaktInspectionStandardItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktInspectionStandardItemQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktInspectionStandardItemExportDto>(),
                sheetName ?? "检验标准明细数据",
                fileName ?? "检验标准明细导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _inspectionStandardItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktInspectionStandardItemExportDto>(),
                sheetName ?? "检验标准明细数据",
                fileName ?? "检验标准明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktInspectionStandardItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "检验标准明细数据",
            fileName ?? "检验标准明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步检验标准明细主表外键（ManyToOne → 检验标准）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampInspectionStandardItemInspectionStandardAsync(TaktInspectionStandardItem entity, TaktInspectionStandardItemCreateDto dto)
    {
        if (dto.InspectionStandardId <= 0)
        {
            return;
        }
        var master = await _inspectionStandardRepository.GetByIdAsync(dto.InspectionStandardId);
        if (master == null)
        {
            throw new TaktBusinessException("检验标准不存在");
        }
        entity.InspectionStandardId = master.Id;
        if (string.IsNullOrEmpty(entity.TenantCode))
        {
            entity.TenantCode = master.TenantCode;
        }
        if (string.IsNullOrEmpty(entity.CompanyCode))
        {
            entity.CompanyCode = master.CompanyCode;
        }
        if (string.IsNullOrEmpty(entity.CultureCode))
        {
            entity.CultureCode = master.CultureCode;
        }
        if (string.IsNullOrEmpty(entity.PlantCode))
        {
            entity.PlantCode = master.PlantCode;
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建检验标准明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktInspectionStandardItem, bool>> QueryExpression(TaktInspectionStandardItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktInspectionStandardItem>();

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
                || (x.ItemCode != null && x.ItemCode.Contains(keywords))
                || (x.ItemName != null && x.ItemName.Contains(keywords))
                || (x.DefectLevel != null && x.DefectLevel.Contains(keywords))
                || (x.StandardValue != null && x.StandardValue.Contains(keywords))
                || (x.UpperLimit != null && x.UpperLimit.Contains(keywords))
                || (x.LowerLimit != null && x.LowerLimit.Contains(keywords))
                || (x.InspectionTool != null && x.InspectionTool.Contains(keywords))
                || (x.InspectionMethodDescription != null && x.InspectionMethodDescription.Contains(keywords))
                || (x.AcceptanceCriteria != null && x.AcceptanceCriteria.Contains(keywords))
                || (x.RejectionCriteria != null && x.RejectionCriteria.Contains(keywords))
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

        if (queryDto?.InspectionStandardId.HasValue == true)
        {
            var inspectionStandardId = queryDto.InspectionStandardId.Value;
            exp = exp.And(x => x.InspectionStandardId == inspectionStandardId);
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            var lineNumber = queryDto.LineNumber.Value;
            exp = exp.And(x => x.LineNumber == lineNumber);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ItemCode))
        {
            var itemCode = queryDto.ItemCode;
            exp = exp.And(x => x.ItemCode != null && x.ItemCode.Contains(itemCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ItemName))
        {
            var itemName = queryDto.ItemName;
            exp = exp.And(x => x.ItemName != null && x.ItemName.Contains(itemName));
        }

        if (queryDto?.ItemType.HasValue == true)
        {
            var itemType = queryDto.ItemType.Value;
            exp = exp.And(x => x.ItemType == itemType);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DefectLevel))
        {
            var defectLevel = queryDto.DefectLevel;
            exp = exp.And(x => x.DefectLevel != null && x.DefectLevel.Contains(defectLevel));
        }

        if (queryDto?.InspectionMode.HasValue == true)
        {
            var inspectionMode = queryDto.InspectionMode.Value;
            exp = exp.And(x => x.InspectionMode == inspectionMode);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.StandardValue))
        {
            var standardValue = queryDto.StandardValue;
            exp = exp.And(x => x.StandardValue != null && x.StandardValue.Contains(standardValue));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.UpperLimit))
        {
            var upperLimit = queryDto.UpperLimit;
            exp = exp.And(x => x.UpperLimit != null && x.UpperLimit.Contains(upperLimit));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.LowerLimit))
        {
            var lowerLimit = queryDto.LowerLimit;
            exp = exp.And(x => x.LowerLimit != null && x.LowerLimit.Contains(lowerLimit));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.InspectionTool))
        {
            var inspectionTool = queryDto.InspectionTool;
            exp = exp.And(x => x.InspectionTool != null && x.InspectionTool.Contains(inspectionTool));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.InspectionMethodDescription))
        {
            var inspectionMethodDescription = queryDto.InspectionMethodDescription;
            exp = exp.And(x => x.InspectionMethodDescription != null && x.InspectionMethodDescription.Contains(inspectionMethodDescription));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AcceptanceCriteria))
        {
            var acceptanceCriteria = queryDto.AcceptanceCriteria;
            exp = exp.And(x => x.AcceptanceCriteria != null && x.AcceptanceCriteria.Contains(acceptanceCriteria));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RejectionCriteria))
        {
            var rejectionCriteria = queryDto.RejectionCriteria;
            exp = exp.And(x => x.RejectionCriteria != null && x.RejectionCriteria.Contains(rejectionCriteria));
        }

        if (queryDto?.IsQualifiedBasis.HasValue == true)
        {
            var isQualifiedBasis = queryDto.IsQualifiedBasis.Value;
            exp = exp.And(x => x.IsQualifiedBasis == isQualifiedBasis);
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
    private static bool HasAnyListQueryFilter(TaktInspectionStandardItemQueryDto? queryDto)
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
        if (queryDto.InspectionStandardId.HasValue)
        {
            return true;
        }
        if (queryDto.LineNumber.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ItemCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ItemName))
        {
            return true;
        }
        if (queryDto.ItemType.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DefectLevel))
        {
            return true;
        }
        if (queryDto.InspectionMode.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.StandardValue))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.UpperLimit))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.LowerLimit))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.InspectionTool))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.InspectionMethodDescription))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AcceptanceCriteria))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RejectionCriteria))
        {
            return true;
        }
        if (queryDto.IsQualifiedBasis.HasValue)
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
        if (queryDto.CreatedAtStart.HasValue || queryDto.CreatedAtEnd.HasValue)
        {
            return true;
        }
        return false;
    }
}
