// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Quality.Cost
// 文件名称：TaktQualityIncidentItemService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：品质事故明细应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Quality.Cost;
using Takt.Domain.Entities.Logistics.Quality.Cost;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Quality.Cost;

/// <summary>
/// 品质事故明细应用服务
/// </summary>
public class TaktQualityIncidentItemService : TaktServiceBase, ITaktQualityIncidentItemService
{
    private readonly ITaktCompanyRepository<TaktQualityIncidentItem> _qualityIncidentItemRepository;
    private readonly ITaktCompanyRepository<TaktQualityIncident> _qualityIncidentRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="qualityIncidentItemRepository">品质事故明细仓储</param>
    /// <param name="qualityIncidentRepository">品质事故主仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktQualityIncidentItemService(
        ITaktCompanyRepository<TaktQualityIncidentItem> qualityIncidentItemRepository,
        ITaktCompanyRepository<TaktQualityIncident> qualityIncidentRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _qualityIncidentItemRepository = qualityIncidentItemRepository;
        _qualityIncidentRepository = qualityIncidentRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取品质事故明细列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktQualityIncidentItemDto>> GetQualityIncidentItemListAsync(TaktQualityIncidentItemQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktQualityIncidentItemDto>.Create(
                new List<TaktQualityIncidentItemDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _qualityIncidentItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktQualityIncidentItemDto>.Create(
            data.Adapt<List<TaktQualityIncidentItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取品质事故明细
    /// </summary>
    /// <param name="id">品质事故明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityIncidentItemDto?> GetQualityIncidentItemByIdAsync(long id)
    {
        var entity = await _qualityIncidentItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktQualityIncidentItemDto>();
    }

    /// <summary>
    /// 获取品质事故明细选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetQualityIncidentItemOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _qualityIncidentItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IsObsolete == 0,
            x => x.QualityIncidentCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.QualityIncidentCode,
            DictLabel = e.QualityIncidentCode,
        }).ToList();
    }

    /// <summary>
    /// 创建品质事故明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityIncidentItemDto> CreateQualityIncidentItemAsync(TaktQualityIncidentItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktQualityIncidentItem>();
        entity.IsObsolete = 0;
        await StampQualityIncidentItemQualityIncidentAsync(entity, dto);
        var isUnique_ix_takt_logistics_quality_incident_item_line_number_unique = await _uniqueValidator.IsUniqueAsync(
            _qualityIncidentItemRepository,
            x => x.QualityIncidentId == entity.QualityIncidentId
                && x.LineNumber == entity.LineNumber
                && x.MaterialCode == entity.MaterialCode);
        if (!isUnique_ix_takt_logistics_quality_incident_item_line_number_unique)
        {
            throw new TaktBusinessException("品质事故明细的QualityIncidentId、LineNumber、MaterialCode已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _qualityIncidentItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.QualityIncidentId == entity.QualityIncidentId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.QualityIncidentCode) ? entity.QualityIncidentCode : entity.QualityIncidentId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _qualityIncidentItemRepository.CreateAsync(entity);
        return await GetQualityIncidentItemByIdAsync(entity.Id) ?? entity.Adapt<TaktQualityIncidentItemDto>();
    }

    /// <summary>
    /// 更新品质事故明细
    /// </summary>
    /// <param name="id">品质事故明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityIncidentItemDto> UpdateQualityIncidentItemAsync(long id, TaktQualityIncidentItemUpdateDto dto)
    {
        var entity = await _qualityIncidentItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("品质事故明细不存在");
        }
        dto.Adapt(entity);
        await StampQualityIncidentItemQualityIncidentAsync(entity, dto);
        var isUnique_ix_takt_logistics_quality_incident_item_line_number_unique = await _uniqueValidator.IsUniqueAsync(
            _qualityIncidentItemRepository,
            x => x.QualityIncidentId == entity.QualityIncidentId
                && x.LineNumber == entity.LineNumber
                && x.MaterialCode == entity.MaterialCode,
            id);
        if (!isUnique_ix_takt_logistics_quality_incident_item_line_number_unique)
        {
            throw new TaktBusinessException("品质事故明细的QualityIncidentId、LineNumber、MaterialCode已存在");
        }
        await _qualityIncidentItemRepository.UpdateAsync(entity);
        return await GetQualityIncidentItemByIdAsync(id) ?? throw new TaktBusinessException("品质事故明细不存在");
    }

    /// <summary>
    /// 删除品质事故明细
    /// </summary>
    /// <param name="id">品质事故明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteQualityIncidentItemByIdAsync(long id)
    {
        var entity = await _qualityIncidentItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("品质事故明细不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("品质事故明细不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("品质事故明细已作废");
        }
        entity.IsObsolete = 1;
        await _qualityIncidentItemRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除品质事故明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteQualityIncidentItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteQualityIncidentItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新品质事故明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityIncidentItemDto> UpdateQualityIncidentItemObsoleteAsync(TaktQualityIncidentItemObsoleteDto dto)
    {
        var entity = await _qualityIncidentItemRepository.GetByIdAsync(dto.QualityIncidentItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("品质事故明细不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("品质事故明细不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _qualityIncidentItemRepository.UpdateAsync(entity);
        return await GetQualityIncidentItemByIdAsync(dto.QualityIncidentItemId) ?? throw new TaktBusinessException("品质事故明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetQualityIncidentItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktQualityIncidentItemTemplateDto>(
            sheetName ?? "品质事故明细导入模板",
            fileName ?? "品质事故明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入品质事故明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportQualityIncidentItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktQualityIncidentItemImportDto>(fileStream, sheetName ?? "品质事故明细导入模板");
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
                var entity = rows[i].Adapt<TaktQualityIncidentItem>();
                var importDto = rows[i].Adapt<TaktQualityIncidentItemCreateDto>();
                await StampQualityIncidentItemQualityIncidentAsync(entity, importDto);
                var importKey = $"{entity.QualityIncidentId}|{entity.LineNumber}|{entity.MaterialCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（QualityIncidentId、LineNumber、MaterialCode）");
                }
                var isUnique_ix_takt_logistics_quality_incident_item_line_number_unique = await _uniqueValidator.IsUniqueAsync(
                    _qualityIncidentItemRepository,
                    x => x.QualityIncidentId == entity.QualityIncidentId
                        && x.LineNumber == entity.LineNumber
                        && x.MaterialCode == entity.MaterialCode);
                if (!isUnique_ix_takt_logistics_quality_incident_item_line_number_unique)
                {
                    throw new TaktBusinessException("品质事故明细的QualityIncidentId、LineNumber、MaterialCode已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _qualityIncidentItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.QualityIncidentId == entity.QualityIncidentId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.QualityIncidentCode) ? entity.QualityIncidentCode : entity.QualityIncidentId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _qualityIncidentItemRepository.CreateAsync(entity);
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
    /// 导出品质事故明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportQualityIncidentItemAsync(TaktQualityIncidentItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktQualityIncidentItemQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktQualityIncidentItemExportDto>(),
                sheetName ?? "品质事故明细数据",
                fileName ?? "品质事故明细导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _qualityIncidentItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktQualityIncidentItemExportDto>(),
                sheetName ?? "品质事故明细数据",
                fileName ?? "品质事故明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktQualityIncidentItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "品质事故明细数据",
            fileName ?? "品质事故明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步品质事故明细主表外键（ManyToOne → 品质事故主）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampQualityIncidentItemQualityIncidentAsync(TaktQualityIncidentItem entity, TaktQualityIncidentItemCreateDto dto)
    {
        if (dto.QualityIncidentId <= 0)
        {
            return;
        }
        var master = await _qualityIncidentRepository.GetByIdAsync(dto.QualityIncidentId);
        if (master == null)
        {
            throw new TaktBusinessException("品质事故主不存在");
        }
        entity.QualityIncidentId = master.Id;
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
        if (string.IsNullOrEmpty(entity.QualityIncidentCode))
        {
            entity.QualityIncidentCode = master.QualityIncidentCode;
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建品质事故明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktQualityIncidentItem, bool>> QueryExpression(TaktQualityIncidentItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktQualityIncidentItem>();

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
                || (x.QualityIncidentCode != null && x.QualityIncidentCode.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.MaterialDescription != null && x.MaterialDescription.Contains(keywords))
                || (x.ScrapNote != null && x.ScrapNote.Contains(keywords))
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

        if (queryDto?.QualityIncidentId.HasValue == true)
        {
            var qualityIncidentId = queryDto.QualityIncidentId.Value;
            exp = exp.And(x => x.QualityIncidentId == qualityIncidentId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.QualityIncidentCode))
        {
            var qualityIncidentCode = queryDto.QualityIncidentCode;
            exp = exp.And(x => x.QualityIncidentCode != null && x.QualityIncidentCode.Contains(qualityIncidentCode));
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

        if (queryDto?.ScrapCost.HasValue == true)
        {
            var scrapCost = queryDto.ScrapCost.Value;
            exp = exp.And(x => x.ScrapCost == scrapCost);
        }

        if (queryDto?.ScrapSize.HasValue == true)
        {
            var scrapSize = queryDto.ScrapSize.Value;
            exp = exp.And(x => x.ScrapSize == scrapSize);
        }

        if (queryDto?.PartPrice.HasValue == true)
        {
            var partPrice = queryDto.PartPrice.Value;
            exp = exp.And(x => x.PartPrice == partPrice);
        }

        if (queryDto?.ScrapReasonCost.HasValue == true)
        {
            var scrapReasonCost = queryDto.ScrapReasonCost.Value;
            exp = exp.And(x => x.ScrapReasonCost == scrapReasonCost);
        }

        if (queryDto?.FreightCharges.HasValue == true)
        {
            var freightCharges = queryDto.FreightCharges.Value;
            exp = exp.And(x => x.FreightCharges == freightCharges);
        }

        if (queryDto?.OtherExpenses.HasValue == true)
        {
            var otherExpenses = queryDto.OtherExpenses.Value;
            exp = exp.And(x => x.OtherExpenses == otherExpenses);
        }

        if (queryDto?.ReasonWorkTimeMinutes.HasValue == true)
        {
            var reasonWorkTimeMinutes = queryDto.ReasonWorkTimeMinutes.Value;
            exp = exp.And(x => x.ReasonWorkTimeMinutes == reasonWorkTimeMinutes);
        }

        if (queryDto?.Tax.HasValue == true)
        {
            var tax = queryDto.Tax.Value;
            exp = exp.And(x => x.Tax == tax);
        }

        if (queryDto?.ReasonOtherExpenses.HasValue == true)
        {
            var reasonOtherExpenses = queryDto.ReasonOtherExpenses.Value;
            exp = exp.And(x => x.ReasonOtherExpenses == reasonOtherExpenses);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ScrapNote))
        {
            var scrapNote = queryDto.ScrapNote;
            exp = exp.And(x => x.ScrapNote != null && x.ScrapNote.Contains(scrapNote));
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
    private static bool HasAnyListQueryFilter(TaktQualityIncidentItemQueryDto? queryDto)
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
        if (queryDto.QualityIncidentId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.QualityIncidentCode))
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
        if (queryDto.ScrapCost.HasValue)
        {
            return true;
        }
        if (queryDto.ScrapSize.HasValue)
        {
            return true;
        }
        if (queryDto.PartPrice.HasValue)
        {
            return true;
        }
        if (queryDto.ScrapReasonCost.HasValue)
        {
            return true;
        }
        if (queryDto.FreightCharges.HasValue)
        {
            return true;
        }
        if (queryDto.OtherExpenses.HasValue)
        {
            return true;
        }
        if (queryDto.ReasonWorkTimeMinutes.HasValue)
        {
            return true;
        }
        if (queryDto.Tax.HasValue)
        {
            return true;
        }
        if (queryDto.ReasonOtherExpenses.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ScrapNote))
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
