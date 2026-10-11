// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Quality.Cost
// 文件名称：TaktQualityIncidentService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：品质事故主应用服务实现
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
/// 品质事故主应用服务
/// </summary>
public class TaktQualityIncidentService : TaktServiceBase, ITaktQualityIncidentService
{
    private readonly ITaktCompanyRepository<TaktQualityIncident> _qualityIncidentRepository;
    private readonly ITaktCompanyRepository<TaktQualityIncidentItem> _qualityIncidentItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="qualityIncidentRepository">品质事故主仓储</param>
    /// <param name="qualityIncidentItemRepository">QualityIncidentItem仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktQualityIncidentService(
        ITaktCompanyRepository<TaktQualityIncident> qualityIncidentRepository,
        ITaktCompanyRepository<TaktQualityIncidentItem> qualityIncidentItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _qualityIncidentRepository = qualityIncidentRepository;
        _qualityIncidentItemRepository = qualityIncidentItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取品质事故主列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktQualityIncidentDto>> GetQualityIncidentListAsync(TaktQualityIncidentQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktQualityIncidentDto>.Create(
                new List<TaktQualityIncidentDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _qualityIncidentRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktQualityIncidentDto>.Create(
            data.Adapt<List<TaktQualityIncidentDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取品质事故主
    /// </summary>
    /// <param name="id">品质事故主ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityIncidentDto?> GetQualityIncidentByIdAsync(long id)
    {
        var entity = await _qualityIncidentRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktQualityIncidentDto>();
        await FillQualityIncidentDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取品质事故主选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetQualityIncidentOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _qualityIncidentRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.QualityIncidentCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.QualityIncidentCode,
            DictLabel = e.QualityIncidentCode,
        }).ToList();
    }

    /// <summary>
    /// 创建品质事故主
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityIncidentDto> CreateQualityIncidentAsync(TaktQualityIncidentCreateDto dto)
    {
        var entity = dto.Adapt<TaktQualityIncident>();
        var isUnique_ix_takt_logistics_quality_incident_qi_unique = await _uniqueValidator.IsUniqueAsync(
            _qualityIncidentRepository,
            x => x.PlantCode == entity.PlantCode
                && x.QualityIncidentCode == entity.QualityIncidentCode
                && x.IncidentDate == entity.IncidentDate);
        if (!isUnique_ix_takt_logistics_quality_incident_qi_unique)
        {
            throw new TaktBusinessException("品质事故主的PlantCode、QualityIncidentCode、IncidentDate已存在");
        }
        entity = await _qualityIncidentRepository.CreateAsync(entity);
                await SaveQualityIncidentChildrenAsync(entity, dto);
        return await GetQualityIncidentByIdAsync(entity.Id) ?? entity.Adapt<TaktQualityIncidentDto>();
    }

    /// <summary>
    /// 更新品质事故主
    /// </summary>
    /// <param name="id">品质事故主ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityIncidentDto> UpdateQualityIncidentAsync(long id, TaktQualityIncidentUpdateDto dto)
    {
        var entity = await _qualityIncidentRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("品质事故主不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_quality_incident_qi_unique = await _uniqueValidator.IsUniqueAsync(
            _qualityIncidentRepository,
            x => x.PlantCode == entity.PlantCode
                && x.QualityIncidentCode == entity.QualityIncidentCode
                && x.IncidentDate == entity.IncidentDate,
            id);
        if (!isUnique_ix_takt_logistics_quality_incident_qi_unique)
        {
            throw new TaktBusinessException("品质事故主的PlantCode、QualityIncidentCode、IncidentDate已存在");
        }
        await _qualityIncidentRepository.UpdateAsync(entity);
                await SaveQualityIncidentChildrenAsync(entity, dto);
        return await GetQualityIncidentByIdAsync(id) ?? throw new TaktBusinessException("品质事故主不存在");
    }

    /// <summary>
    /// 删除品质事故主
    /// </summary>
    /// <param name="id">品质事故主ID</param>
    /// <returns>任务</returns>
    public async Task DeleteQualityIncidentByIdAsync(long id)
    {
        var entity = await _qualityIncidentRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("品质事故主不存在或已删除");
        }
        await _qualityIncidentItemRepository.DeleteAsync(x => x.QualityIncidentId == entity.Id);
        var deleted = await _qualityIncidentRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("品质事故主不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除品质事故主
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteQualityIncidentBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteQualityIncidentByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetQualityIncidentTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktQualityIncidentTemplateDto>(
            sheetName ?? "品质事故主导入模板",
            fileName ?? "品质事故主导入模板.xlsx");
    }

    /// <summary>
    /// 导入品质事故主
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportQualityIncidentAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktQualityIncidentImportDto>(fileStream, sheetName ?? "品质事故主导入模板");
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
                var entity = rows[i].Adapt<TaktQualityIncident>();
                var importKey = $"{entity.PlantCode}|{entity.QualityIncidentCode}|{entity.IncidentDate}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、QualityIncidentCode、IncidentDate）");
                }
                var isUnique_ix_takt_logistics_quality_incident_qi_unique = await _uniqueValidator.IsUniqueAsync(
                    _qualityIncidentRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.QualityIncidentCode == entity.QualityIncidentCode
                        && x.IncidentDate == entity.IncidentDate);
                if (!isUnique_ix_takt_logistics_quality_incident_qi_unique)
                {
                    throw new TaktBusinessException("品质事故主的PlantCode、QualityIncidentCode、IncidentDate已存在");
                }
                await _qualityIncidentRepository.CreateAsync(entity);
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
    /// 导出品质事故主
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportQualityIncidentAsync(TaktQualityIncidentQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktQualityIncidentQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktQualityIncidentExportDto>(),
                sheetName ?? "品质事故主数据",
                fileName ?? "品质事故主导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _qualityIncidentRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktQualityIncidentExportDto>(),
                sheetName ?? "品质事故主数据",
                fileName ?? "品质事故主导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktQualityIncidentExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "品质事故主数据",
            fileName ?? "品质事故主导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废品质事故明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="qualityIncidentId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkQualityIncidentItemsObsoleteAsync(long qualityIncidentId)
    {
        if (qualityIncidentId <= 0)
        {
            return;
        }
        var rows = await _qualityIncidentItemRepository.GetListAsync(
            x => x.QualityIncidentId == qualityIncidentId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _qualityIncidentItemRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充品质事故主详情（加载 OneToMany 子表：品质事故明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillQualityIncidentDetailsAsync(TaktQualityIncidentDto dto, TaktQualityIncident entity)
    {
        if (dto == null)
        {
            return;
        }
        // 品质事故明细 → dto.IncidentItems（含作废行）
        var incidentitems = await _qualityIncidentItemRepository.GetListAsync(x => x.QualityIncidentId == entity.Id);
        dto.IncidentItems = incidentitems.Adapt<List<TaktQualityIncidentItemDto>>();
    }

    /// <summary>
    /// 保存品质事故主子表级联（品质事故明细；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveQualityIncidentChildrenAsync(TaktQualityIncident entity, TaktQualityIncidentCreateDto dto)
    {
        // 品质事故明细（IncidentItems）
        List<TaktQualityIncidentItemUpdateDto>? incidentItemsForSave;
        if (dto is TaktQualityIncidentUpdateDto updateDtoForIncidentItems && updateDtoForIncidentItems.IncidentItems != null)
        {
            incidentItemsForSave = updateDtoForIncidentItems.IncidentItems;
        }
        else if (dto.IncidentItems != null)
        {
            incidentItemsForSave = dto.IncidentItems.Adapt<List<TaktQualityIncidentItemUpdateDto>>();
        }
        else
        {
            incidentItemsForSave = null;
        }
        if (incidentItemsForSave is not { Count: > 0 })
        {
            await MarkQualityIncidentItemsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _qualityIncidentItemRepository.GetListAsync(x => x.QualityIncidentId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktQualityIncidentItem>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < incidentItemsForSave.Count; i++)
            {
                var childDto = incidentItemsForSave[i];
                childDto.QualityIncidentId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.QualityIncidentCode = entity.QualityIncidentCode;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("品质事故明细第{i + 1}项与本次提交的其他项重复（CompanyCode、QualityIncidentId、LineNumber）");
                }
                if (childDto.QualityIncidentItemId > 0)
                {
                    if (!existingById.TryGetValue(childDto.QualityIncidentItemId, out var target))
                    {
                        throw new TaktBusinessException("品质事故明细不存在（QualityIncidentItemId={childDto.QualityIncidentItemId}）");
                    }
                    if (target.QualityIncidentId != entity.Id)
                    {
                        throw new TaktBusinessException("品质事故明细不属于当前主表（QualityIncidentItemId={childDto.QualityIncidentItemId}）");
                    }
                    submittedIds.Add(childDto.QualityIncidentItemId);
                    var isUniqueUpdate_ix_takt_logistics_quality_incident_item_line_number_unique = await _uniqueValidator.IsUniqueAsync(
                        _qualityIncidentItemRepository,
                        x => x.QualityIncidentId == x.QualityIncidentId
                && x.LineNumber == x.LineNumber
                && x.MaterialCode == x.MaterialCode,
                        childDto.QualityIncidentItemId);
                    if (!isUniqueUpdate_ix_takt_logistics_quality_incident_item_line_number_unique)
                    {
                        throw new TaktBusinessException("品质事故明细的QualityIncidentId、LineNumber、MaterialCode已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.QualityIncidentItemId;
                    target.QualityIncidentId = entity.Id;
                    target.IsObsolete = 0;
                    await _qualityIncidentItemRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_quality_incident_item_line_number_unique = await _uniqueValidator.IsUniqueAsync(
                        _qualityIncidentItemRepository,
                        x => x.QualityIncidentId == x.QualityIncidentId
                && x.LineNumber == x.LineNumber
                && x.MaterialCode == x.MaterialCode);
                    if (!isUniqueCreate_ix_takt_logistics_quality_incident_item_line_number_unique)
                    {
                        throw new TaktBusinessException("品质事故明细的QualityIncidentId、LineNumber、MaterialCode已存在");
                    }
                    var child = childDto.Adapt<TaktQualityIncidentItem>();
                    child.Id = 0;
                    child.QualityIncidentId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _qualityIncidentItemRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.QualityIncidentCode) ? entity.QualityIncidentCode : entity.Id.ToString();
                    var maxLine = existingList.Count > 0 ? existingList.Max(x => x.LineNumber) : 0;
                    var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, needLine.Count, maxLine).ToList();
                    var lineIdx = 0;
                    foreach (var child in toCreate)
                    {
                        if (child.LineNumber <= 0)
                        {
                            child.LineNumber = lineSeq[lineIdx++];
                        }
                    }
                }
                await _qualityIncidentItemRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建品质事故主查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktQualityIncident, bool>> QueryExpression(TaktQualityIncidentQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktQualityIncident>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.QualityIncidentCode != null && x.QualityIncidentCode.Contains(keywords))
                || (x.Model != null && x.Model.Contains(keywords))
                || (x.IncidentReason != null && x.IncidentReason.Contains(keywords))
                || (x.CurrencyCode != null && x.CurrencyCode.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.QualityIncidentCode))
        {
            var qualityIncidentCode = queryDto.QualityIncidentCode;
            exp = exp.And(x => x.QualityIncidentCode != null && x.QualityIncidentCode.Contains(qualityIncidentCode));
        }

        if (queryDto?.IndirectManpowerCostPerMinute.HasValue == true)
        {
            var indirectManpowerCostPerMinute = queryDto.IndirectManpowerCostPerMinute.Value;
            exp = exp.And(x => x.IndirectManpowerCostPerMinute == indirectManpowerCostPerMinute);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Model))
        {
            var model = queryDto.Model;
            exp = exp.And(x => x.Model != null && x.Model.Contains(model));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.IncidentReason))
        {
            var incidentReason = queryDto.IncidentReason;
            exp = exp.And(x => x.IncidentReason != null && x.IncidentReason.Contains(incidentReason));
        }

        if (queryDto?.TotalScrapQuantity.HasValue == true)
        {
            var totalScrapQuantity = queryDto.TotalScrapQuantity.Value;
            exp = exp.And(x => x.TotalScrapQuantity == totalScrapQuantity);
        }

        if (queryDto?.TotalScrapCost.HasValue == true)
        {
            var totalScrapCost = queryDto.TotalScrapCost.Value;
            exp = exp.And(x => x.TotalScrapCost == totalScrapCost);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CurrencyCode))
        {
            var currencyCode = queryDto.CurrencyCode;
            exp = exp.And(x => x.CurrencyCode != null && x.CurrencyCode.Contains(currencyCode));
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

        if (queryDto?.IncidentDateStart.HasValue == true)
        {
            var incidentDateStart = queryDto.IncidentDateStart.Value;
            exp = exp.And(x => x.IncidentDate >= incidentDateStart);
        }

        if (queryDto?.IncidentDateEnd.HasValue == true)
        {
            var incidentDateEnd = queryDto.IncidentDateEnd.Value;
            exp = exp.And(x => x.IncidentDate <= incidentDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktQualityIncidentQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.QualityIncidentCode))
        {
            return true;
        }
        if (queryDto.IndirectManpowerCostPerMinute.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Model))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.IncidentReason))
        {
            return true;
        }
        if (queryDto.TotalScrapQuantity.HasValue)
        {
            return true;
        }
        if (queryDto.TotalScrapCost.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CurrencyCode))
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
        if (queryDto.IncidentDateStart.HasValue || queryDto.IncidentDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.CreatedAtStart.HasValue || queryDto.CreatedAtEnd.HasValue)
        {
            return true;
        }
        return false;
    }

    // ========================================
    // 扩展方法（保留）
    // ========================================

    /// <summary>
    /// 获取品质事故金额统计（数据看板；按 IncidentDate 汇总 TotalScrapCost）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>品质事故金额统计</returns>
    public async Task<TaktQualityCostStatDto> GetQualityIncidentCostStatAsync(TaktQualityCostStatQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        EnsureThreeLayerContext();
        var (start, end, statMonth) = TaktStatMonthRangeHelper.ResolveStatMonthRange(
            queryDto.DateStart,
            queryDto.DateEnd,
            queryDto.StatMonth);
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;
        Expression<Func<TaktQualityIncident, bool>> predicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.IncidentDate >= start
            && x.IncidentDate <= end;
        var monthRowCount = await _qualityIncidentRepository.CountAsync(predicate);
        var monthTotalAmount = await _qualityIncidentRepository.SumAsync(x => x.TotalScrapCost, predicate);
        return new TaktQualityCostStatDto
        {
            StatMonth = statMonth,
            MonthTotalAmount = monthTotalAmount,
            MonthRowCount = monthRowCount,
        };
    }
}
