// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Quality.Complaint
// 文件名称：TaktSupplierEvaluationItemService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：供应商评价考核项目明细应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Quality.Complaint;
using Takt.Domain.Entities.Logistics.Quality.Complaint;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Quality.Complaint;

/// <summary>
/// 供应商评价考核项目明细应用服务
/// </summary>
public class TaktSupplierEvaluationItemService : TaktServiceBase, ITaktSupplierEvaluationItemService
{
    private readonly ITaktCompanyRepository<TaktSupplierEvaluationItem> _supplierEvaluationItemRepository;
    private readonly ITaktCompanyRepository<TaktSupplierEvaluation> _supplierEvaluationRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="supplierEvaluationItemRepository">供应商评价考核项目明细仓储</param>
    /// <param name="supplierEvaluationRepository">供应商评价考核仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSupplierEvaluationItemService(
        ITaktCompanyRepository<TaktSupplierEvaluationItem> supplierEvaluationItemRepository,
        ITaktCompanyRepository<TaktSupplierEvaluation> supplierEvaluationRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _supplierEvaluationItemRepository = supplierEvaluationItemRepository;
        _supplierEvaluationRepository = supplierEvaluationRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取供应商评价考核项目明细列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSupplierEvaluationItemDto>> GetSupplierEvaluationItemListAsync(TaktSupplierEvaluationItemQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktSupplierEvaluationItemDto>.Create(
                new List<TaktSupplierEvaluationItemDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _supplierEvaluationItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSupplierEvaluationItemDto>.Create(
            data.Adapt<List<TaktSupplierEvaluationItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取供应商评价考核项目明细
    /// </summary>
    /// <param name="id">供应商评价考核项目明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSupplierEvaluationItemDto?> GetSupplierEvaluationItemByIdAsync(long id)
    {
        var entity = await _supplierEvaluationItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktSupplierEvaluationItemDto>();
    }

    /// <summary>
    /// 获取供应商评价考核项目明细选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSupplierEvaluationItemOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _supplierEvaluationItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.RectificationStatus == 1 && x.IsObsolete == 0,
            x => x.ItemName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.SupplierEvaluationCode,
            DictLabel = e.ItemName ?? e.SupplierEvaluationCode,
        }).ToList();
    }

    /// <summary>
    /// 创建供应商评价考核项目明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSupplierEvaluationItemDto> CreateSupplierEvaluationItemAsync(TaktSupplierEvaluationItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktSupplierEvaluationItem>();
        entity.IsObsolete = 0;
        await StampSupplierEvaluationItemSupplierEvaluationAsync(entity, dto);
        var isUnique_ix_takt_logistics_quality_supplier_evaluation_item_line_unique = await _uniqueValidator.IsUniqueAsync(
            _supplierEvaluationItemRepository,
            x => x.EvaluationId == entity.EvaluationId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_quality_supplier_evaluation_item_line_unique)
        {
            throw new TaktBusinessException("供应商评价考核项目明细的EvaluationId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _supplierEvaluationItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.EvaluationId == entity.EvaluationId,
                x => x.LineNumber);
            var businessCode = entity.EvaluationId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _supplierEvaluationItemRepository.CreateAsync(entity);
        return await GetSupplierEvaluationItemByIdAsync(entity.Id) ?? entity.Adapt<TaktSupplierEvaluationItemDto>();
    }

    /// <summary>
    /// 更新供应商评价考核项目明细
    /// </summary>
    /// <param name="id">供应商评价考核项目明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSupplierEvaluationItemDto> UpdateSupplierEvaluationItemAsync(long id, TaktSupplierEvaluationItemUpdateDto dto)
    {
        var entity = await _supplierEvaluationItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("供应商评价考核项目明细不存在");
        }
        dto.Adapt(entity);
        await StampSupplierEvaluationItemSupplierEvaluationAsync(entity, dto);
        var isUnique_ix_takt_logistics_quality_supplier_evaluation_item_line_unique = await _uniqueValidator.IsUniqueAsync(
            _supplierEvaluationItemRepository,
            x => x.EvaluationId == entity.EvaluationId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_quality_supplier_evaluation_item_line_unique)
        {
            throw new TaktBusinessException("供应商评价考核项目明细的EvaluationId、LineNumber已存在");
        }
        await _supplierEvaluationItemRepository.UpdateAsync(entity);
        return await GetSupplierEvaluationItemByIdAsync(id) ?? throw new TaktBusinessException("供应商评价考核项目明细不存在");
    }

    /// <summary>
    /// 删除供应商评价考核项目明细
    /// </summary>
    /// <param name="id">供应商评价考核项目明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSupplierEvaluationItemByIdAsync(long id)
    {
        var entity = await _supplierEvaluationItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("供应商评价考核项目明细不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("供应商评价考核项目明细不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("供应商评价考核项目明细已作废");
        }
        entity.IsObsolete = 1;
        await _supplierEvaluationItemRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除供应商评价考核项目明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSupplierEvaluationItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSupplierEvaluationItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新供应商评价考核项目明细状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSupplierEvaluationItemDto> UpdateSupplierEvaluationItemStatusAsync(TaktSupplierEvaluationItemStatusDto dto)
    {
        var entity = await _supplierEvaluationItemRepository.GetByIdAsync(dto.SupplierEvaluationItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("供应商评价考核项目明细不存在");
        }
        entity.RectificationStatus = dto.RectificationStatus;
        await _supplierEvaluationItemRepository.UpdateAsync(entity);
        return await GetSupplierEvaluationItemByIdAsync(dto.SupplierEvaluationItemId) ?? throw new TaktBusinessException("供应商评价考核项目明细不存在");
    }

    /// <summary>
    /// 更新供应商评价考核项目明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSupplierEvaluationItemDto> UpdateSupplierEvaluationItemObsoleteAsync(TaktSupplierEvaluationItemObsoleteDto dto)
    {
        var entity = await _supplierEvaluationItemRepository.GetByIdAsync(dto.SupplierEvaluationItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("供应商评价考核项目明细不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("供应商评价考核项目明细不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _supplierEvaluationItemRepository.UpdateAsync(entity);
        return await GetSupplierEvaluationItemByIdAsync(dto.SupplierEvaluationItemId) ?? throw new TaktBusinessException("供应商评价考核项目明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSupplierEvaluationItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSupplierEvaluationItemTemplateDto>(
            sheetName ?? "供应商评价考核项目明细导入模板",
            fileName ?? "供应商评价考核项目明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入供应商评价考核项目明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSupplierEvaluationItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSupplierEvaluationItemImportDto>(fileStream, sheetName ?? "供应商评价考核项目明细导入模板");
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
                var entity = rows[i].Adapt<TaktSupplierEvaluationItem>();
                var importDto = rows[i].Adapt<TaktSupplierEvaluationItemCreateDto>();
                await StampSupplierEvaluationItemSupplierEvaluationAsync(entity, importDto);
                var importKey = $"{entity.EvaluationId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（EvaluationId、LineNumber）");
                }
                var isUnique_ix_takt_logistics_quality_supplier_evaluation_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _supplierEvaluationItemRepository,
                    x => x.EvaluationId == entity.EvaluationId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_quality_supplier_evaluation_item_line_unique)
                {
                    throw new TaktBusinessException("供应商评价考核项目明细的EvaluationId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _supplierEvaluationItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.EvaluationId == entity.EvaluationId,
                        x => x.LineNumber);
                    var businessCode = entity.EvaluationId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _supplierEvaluationItemRepository.CreateAsync(entity);
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
    /// 导出供应商评价考核项目明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSupplierEvaluationItemAsync(TaktSupplierEvaluationItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktSupplierEvaluationItemQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSupplierEvaluationItemExportDto>(),
                sheetName ?? "供应商评价考核项目明细数据",
                fileName ?? "供应商评价考核项目明细导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _supplierEvaluationItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSupplierEvaluationItemExportDto>(),
                sheetName ?? "供应商评价考核项目明细数据",
                fileName ?? "供应商评价考核项目明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSupplierEvaluationItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "供应商评价考核项目明细数据",
            fileName ?? "供应商评价考核项目明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步供应商评价考核项目明细主表外键（ManyToOne → 供应商评价考核）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampSupplierEvaluationItemSupplierEvaluationAsync(TaktSupplierEvaluationItem entity, TaktSupplierEvaluationItemCreateDto dto)
    {
        if (dto.EvaluationId <= 0)
        {
            return;
        }
        var master = await _supplierEvaluationRepository.GetByIdAsync(dto.EvaluationId);
        if (master == null)
        {
            throw new TaktBusinessException("供应商评价考核不存在");
        }
        entity.EvaluationId = master.Id;
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
        if (string.IsNullOrEmpty(entity.SupplierEvaluationCode))
        {
            entity.SupplierEvaluationCode = master.SupplierEvaluationCode;
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建供应商评价考核项目明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSupplierEvaluationItem, bool>> QueryExpression(TaktSupplierEvaluationItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSupplierEvaluationItem>();

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
                || (x.SupplierEvaluationCode != null && x.SupplierEvaluationCode.Contains(keywords))
                || (x.ItemName != null && x.ItemName.Contains(keywords))
                || (x.ItemDescription != null && x.ItemDescription.Contains(keywords))
                || (x.ScoringStandard != null && x.ScoringStandard.Contains(keywords))
                || (x.EvaluationComment != null && x.EvaluationComment.Contains(keywords))
                || (x.ExistingIssues != null && x.ExistingIssues.Contains(keywords))
                || (x.ImprovementRequirement != null && x.ImprovementRequirement.Contains(keywords))
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

        if (queryDto?.EvaluationId.HasValue == true)
        {
            var evaluationId = queryDto.EvaluationId.Value;
            exp = exp.And(x => x.EvaluationId == evaluationId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SupplierEvaluationCode))
        {
            var supplierEvaluationCode = queryDto.SupplierEvaluationCode;
            exp = exp.And(x => x.SupplierEvaluationCode != null && x.SupplierEvaluationCode.Contains(supplierEvaluationCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            var lineNumber = queryDto.LineNumber.Value;
            exp = exp.And(x => x.LineNumber == lineNumber);
        }

        if (queryDto?.CategoryType.HasValue == true)
        {
            var categoryType = queryDto.CategoryType.Value;
            exp = exp.And(x => x.CategoryType == categoryType);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ItemName))
        {
            var itemName = queryDto.ItemName;
            exp = exp.And(x => x.ItemName != null && x.ItemName.Contains(itemName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ItemDescription))
        {
            var itemDescription = queryDto.ItemDescription;
            exp = exp.And(x => x.ItemDescription != null && x.ItemDescription.Contains(itemDescription));
        }

        if (queryDto?.Weight.HasValue == true)
        {
            var weight = queryDto.Weight.Value;
            exp = exp.And(x => x.Weight == weight);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ScoringStandard))
        {
            var scoringStandard = queryDto.ScoringStandard;
            exp = exp.And(x => x.ScoringStandard != null && x.ScoringStandard.Contains(scoringStandard));
        }

        if (queryDto?.Score.HasValue == true)
        {
            var score = queryDto.Score.Value;
            exp = exp.And(x => x.Score == score);
        }

        if (queryDto?.RatingLevel.HasValue == true)
        {
            var ratingLevel = queryDto.RatingLevel.Value;
            exp = exp.And(x => x.RatingLevel == ratingLevel);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EvaluationComment))
        {
            var evaluationComment = queryDto.EvaluationComment;
            exp = exp.And(x => x.EvaluationComment != null && x.EvaluationComment.Contains(evaluationComment));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ExistingIssues))
        {
            var existingIssues = queryDto.ExistingIssues;
            exp = exp.And(x => x.ExistingIssues != null && x.ExistingIssues.Contains(existingIssues));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ImprovementRequirement))
        {
            var improvementRequirement = queryDto.ImprovementRequirement;
            exp = exp.And(x => x.ImprovementRequirement != null && x.ImprovementRequirement.Contains(improvementRequirement));
        }

        if (queryDto?.RectificationRequired.HasValue == true)
        {
            var rectificationRequired = queryDto.RectificationRequired.Value;
            exp = exp.And(x => x.RectificationRequired == rectificationRequired);
        }

        if (queryDto?.RectificationStatus.HasValue == true)
        {
            var rectificationStatus = queryDto.RectificationStatus.Value;
            exp = exp.And(x => x.RectificationStatus == rectificationStatus);
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

        if (queryDto?.RectificationDeadlineStart.HasValue == true)
        {
            var rectificationDeadlineStart = queryDto.RectificationDeadlineStart.Value;
            exp = exp.And(x => x.RectificationDeadline >= rectificationDeadlineStart);
        }

        if (queryDto?.RectificationDeadlineEnd.HasValue == true)
        {
            var rectificationDeadlineEnd = queryDto.RectificationDeadlineEnd.Value;
            exp = exp.And(x => x.RectificationDeadline <= rectificationDeadlineEnd);
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
    private static bool HasAnyListQueryFilter(TaktSupplierEvaluationItemQueryDto? queryDto)
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
        if (queryDto.EvaluationId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SupplierEvaluationCode))
        {
            return true;
        }
        if (queryDto.LineNumber.HasValue)
        {
            return true;
        }
        if (queryDto.CategoryType.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ItemName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ItemDescription))
        {
            return true;
        }
        if (queryDto.Weight.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ScoringStandard))
        {
            return true;
        }
        if (queryDto.Score.HasValue)
        {
            return true;
        }
        if (queryDto.RatingLevel.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EvaluationComment))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ExistingIssues))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ImprovementRequirement))
        {
            return true;
        }
        if (queryDto.RectificationRequired.HasValue)
        {
            return true;
        }
        if (queryDto.RectificationStatus.HasValue)
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
        if (queryDto.RectificationDeadlineStart.HasValue || queryDto.RectificationDeadlineEnd.HasValue)
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
