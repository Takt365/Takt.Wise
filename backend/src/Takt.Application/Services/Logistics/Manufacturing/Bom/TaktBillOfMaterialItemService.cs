// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktBillOfMaterialItemService.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Cursor AI)
// 功能描述：物料清单明细应用服务实现
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
/// 物料清单明细应用服务
/// </summary>
public class TaktBillOfMaterialItemService : TaktServiceBase, ITaktBillOfMaterialItemService
{
    private readonly ITaktCompanyRepository<TaktBillOfMaterialItem> _billOfMaterialItemRepository;
    private readonly ITaktCompanyRepository<TaktBillOfMaterialSubstitute> _billOfMaterialSubstituteRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="billOfMaterialItemRepository">物料清单明细仓储</param>
    /// <param name="billOfMaterialSubstituteRepository">BillOfMaterialSubstitute仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktBillOfMaterialItemService(
        ITaktCompanyRepository<TaktBillOfMaterialItem> billOfMaterialItemRepository,
        ITaktCompanyRepository<TaktBillOfMaterialSubstitute> billOfMaterialSubstituteRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _billOfMaterialItemRepository = billOfMaterialItemRepository;
        _billOfMaterialSubstituteRepository = billOfMaterialSubstituteRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取物料清单明细列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktBillOfMaterialItemDto>> GetBillOfMaterialItemListAsync(TaktBillOfMaterialItemQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktBillOfMaterialItemDto>.Create(
                new List<TaktBillOfMaterialItemDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _billOfMaterialItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktBillOfMaterialItemDto>.Create(
            data.Adapt<List<TaktBillOfMaterialItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取物料清单明细
    /// </summary>
    /// <param name="id">物料清单明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktBillOfMaterialItemDto?> GetBillOfMaterialItemByIdAsync(long id)
    {
        var entity = await _billOfMaterialItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktBillOfMaterialItemDto>();
        await FillBillOfMaterialItemDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取物料清单明细选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetBillOfMaterialItemOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _billOfMaterialItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IsObsolete == 0,
            x => x.BomCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.BomCode,
            DictLabel = e.BomCode,
        }).ToList();
    }

    /// <summary>
    /// 创建物料清单明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBillOfMaterialItemDto> CreateBillOfMaterialItemAsync(TaktBillOfMaterialItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktBillOfMaterialItem>();
        entity.IsObsolete = 0;
        var isUnique_ix_takt_logistics_manufacturing_bom_item_bom_line_unique = await _uniqueValidator.IsUniqueAsync(
            _billOfMaterialItemRepository,
            x => x.BillOfMaterialId == entity.BillOfMaterialId
                && x.LineNumber == entity.LineNumber
                && x.MaterialCode == entity.MaterialCode);
        if (!isUnique_ix_takt_logistics_manufacturing_bom_item_bom_line_unique)
        {
            throw new TaktBusinessException("物料清单明细的BillOfMaterialId、LineNumber、MaterialCode已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _billOfMaterialItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.BillOfMaterialId == entity.BillOfMaterialId,
                x => x.LineNumber);
            var businessCode = entity.BillOfMaterialId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _billOfMaterialItemRepository.CreateAsync(entity);
                await SaveBillOfMaterialItemChildrenAsync(entity, dto);
        return await GetBillOfMaterialItemByIdAsync(entity.Id) ?? entity.Adapt<TaktBillOfMaterialItemDto>();
    }

    /// <summary>
    /// 更新物料清单明细
    /// </summary>
    /// <param name="id">物料清单明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBillOfMaterialItemDto> UpdateBillOfMaterialItemAsync(long id, TaktBillOfMaterialItemUpdateDto dto)
    {
        var entity = await _billOfMaterialItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("物料清单明细不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_bom_item_bom_line_unique = await _uniqueValidator.IsUniqueAsync(
            _billOfMaterialItemRepository,
            x => x.BillOfMaterialId == entity.BillOfMaterialId
                && x.LineNumber == entity.LineNumber
                && x.MaterialCode == entity.MaterialCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_bom_item_bom_line_unique)
        {
            throw new TaktBusinessException("物料清单明细的BillOfMaterialId、LineNumber、MaterialCode已存在");
        }
        await _billOfMaterialItemRepository.UpdateAsync(entity);
                await SaveBillOfMaterialItemChildrenAsync(entity, dto);
        return await GetBillOfMaterialItemByIdAsync(id) ?? throw new TaktBusinessException("物料清单明细不存在");
    }

    /// <summary>
    /// 删除物料清单明细
    /// </summary>
    /// <param name="id">物料清单明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteBillOfMaterialItemByIdAsync(long id)
    {
        var entity = await _billOfMaterialItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("物料清单明细不存在或已删除");
        }
        await _billOfMaterialSubstituteRepository.DeleteAsync(x => x.BillOfMaterialItemId == entity.Id);
        var deleted = await _billOfMaterialItemRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("物料清单明细不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除物料清单明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteBillOfMaterialItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteBillOfMaterialItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新物料清单明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBillOfMaterialItemDto> UpdateBillOfMaterialItemObsoleteAsync(TaktBillOfMaterialItemObsoleteDto dto)
    {
        var entity = await _billOfMaterialItemRepository.GetByIdAsync(dto.BillOfMaterialItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("物料清单明细不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("物料清单明细不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _billOfMaterialItemRepository.UpdateAsync(entity);
        return await GetBillOfMaterialItemByIdAsync(dto.BillOfMaterialItemId) ?? throw new TaktBusinessException("物料清单明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetBillOfMaterialItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktBillOfMaterialItemTemplateDto>(
            sheetName ?? "物料清单明细导入模板",
            fileName ?? "物料清单明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入物料清单明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportBillOfMaterialItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktBillOfMaterialItemImportDto>(fileStream, sheetName ?? "物料清单明细导入模板");
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
                var entity = rows[i].Adapt<TaktBillOfMaterialItem>();
                var importKey = $"{entity.BillOfMaterialId}|{entity.LineNumber}|{entity.MaterialCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（BillOfMaterialId、LineNumber、MaterialCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_bom_item_bom_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _billOfMaterialItemRepository,
                    x => x.BillOfMaterialId == entity.BillOfMaterialId
                        && x.LineNumber == entity.LineNumber
                        && x.MaterialCode == entity.MaterialCode);
                if (!isUnique_ix_takt_logistics_manufacturing_bom_item_bom_line_unique)
                {
                    throw new TaktBusinessException("物料清单明细的BillOfMaterialId、LineNumber、MaterialCode已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _billOfMaterialItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.BillOfMaterialId == entity.BillOfMaterialId,
                        x => x.LineNumber);
                    var businessCode = entity.BillOfMaterialId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _billOfMaterialItemRepository.CreateAsync(entity);
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
    /// 导出物料清单明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportBillOfMaterialItemAsync(TaktBillOfMaterialItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktBillOfMaterialItemQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktBillOfMaterialItemExportDto>(),
                sheetName ?? "物料清单明细数据",
                fileName ?? "物料清单明细导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _billOfMaterialItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktBillOfMaterialItemExportDto>(),
                sheetName ?? "物料清单明细数据",
                fileName ?? "物料清单明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktBillOfMaterialItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "物料清单明细数据",
            fileName ?? "物料清单明细导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废BOM替代料标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="billOfMaterialItemId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkBillOfMaterialSubstitutesObsoleteAsync(long billOfMaterialItemId)
    {
        if (billOfMaterialItemId <= 0)
        {
            return;
        }
        var rows = await _billOfMaterialSubstituteRepository.GetListAsync(
            x => x.BillOfMaterialItemId == billOfMaterialItemId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _billOfMaterialSubstituteRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充物料清单明细详情（加载 OneToMany 子表：BOM替代料）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillBillOfMaterialItemDetailsAsync(TaktBillOfMaterialItemDto dto, TaktBillOfMaterialItem entity)
    {
        if (dto == null)
        {
            return;
        }
        // BOM替代料 → dto.Substitutes（含作废行）
        var substitutes = await _billOfMaterialSubstituteRepository.GetListAsync(x => x.BillOfMaterialItemId == entity.Id);
        dto.Substitutes = substitutes.Adapt<List<TaktBillOfMaterialSubstituteDto>>();
    }

    /// <summary>
    /// 保存物料清单明细子表级联（BOM替代料；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveBillOfMaterialItemChildrenAsync(TaktBillOfMaterialItem entity, TaktBillOfMaterialItemCreateDto dto)
    {
        // BOM替代料（Substitutes）
        List<TaktBillOfMaterialSubstituteUpdateDto>? substitutesForSave;
        if (dto is TaktBillOfMaterialItemUpdateDto updateDtoForSubstitutes && updateDtoForSubstitutes.Substitutes != null)
        {
            substitutesForSave = updateDtoForSubstitutes.Substitutes;
        }
        else if (dto.Substitutes != null)
        {
            substitutesForSave = dto.Substitutes.Adapt<List<TaktBillOfMaterialSubstituteUpdateDto>>();
        }
        else
        {
            substitutesForSave = null;
        }
        if (substitutesForSave is not { Count: > 0 })
        {
            await MarkBillOfMaterialSubstitutesObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _billOfMaterialSubstituteRepository.GetListAsync(x => x.BillOfMaterialItemId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktBillOfMaterialSubstitute>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < substitutesForSave.Count; i++)
            {
                var childDto = substitutesForSave[i];
                childDto.BillOfMaterialItemId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.BomCode = entity.BomCode;
                childDto.SubstituteGroup = entity.SubstituteGroup ?? string.Empty;
                childDto.MaterialUnit = entity.MaterialUnit;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("BOM替代料第{i + 1}项与本次提交的其他项重复（CompanyCode、BillOfMaterialItemId、LineNumber）");
                }
                if (childDto.BillOfMaterialSubstituteId > 0)
                {
                    if (!existingById.TryGetValue(childDto.BillOfMaterialSubstituteId, out var target))
                    {
                        throw new TaktBusinessException("BOM替代料不存在（BillOfMaterialSubstituteId={childDto.BillOfMaterialSubstituteId}）");
                    }
                    if (target.BillOfMaterialItemId != entity.Id)
                    {
                        throw new TaktBusinessException("BOM替代料不属于当前主表（BillOfMaterialSubstituteId={childDto.BillOfMaterialSubstituteId}）");
                    }
                    submittedIds.Add(childDto.BillOfMaterialSubstituteId);
                    childDto.Adapt(target);
                    target.Id = childDto.BillOfMaterialSubstituteId;
                    target.BillOfMaterialItemId = entity.Id;
                    target.IsObsolete = 0;
                    await _billOfMaterialSubstituteRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktBillOfMaterialSubstitute>();
                    child.Id = 0;
                    child.BillOfMaterialItemId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _billOfMaterialSubstituteRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.BomCode) ? entity.BomCode : entity.Id.ToString();
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
                await _billOfMaterialSubstituteRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建物料清单明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktBillOfMaterialItem, bool>> QueryExpression(TaktBillOfMaterialItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktBillOfMaterialItem>();

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
                || (x.BomCode != null && x.BomCode.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.MaterialDescription != null && x.MaterialDescription.Contains(keywords))
                || (x.MaterialUnit != null && x.MaterialUnit.Contains(keywords))
                || (x.WorkCenter != null && x.WorkCenter.Contains(keywords))
                || (x.Position != null && x.Position.Contains(keywords))
                || (x.SubstituteGroup != null && x.SubstituteGroup.Contains(keywords))
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

        if (queryDto?.BillOfMaterialId.HasValue == true)
        {
            var billOfMaterialId = queryDto.BillOfMaterialId.Value;
            exp = exp.And(x => x.BillOfMaterialId == billOfMaterialId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BomCode))
        {
            var bomCode = queryDto.BomCode;
            exp = exp.And(x => x.BomCode != null && x.BomCode.Contains(bomCode));
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

        if (queryDto?.UsageQuantity.HasValue == true)
        {
            var usageQuantity = queryDto.UsageQuantity.Value;
            exp = exp.And(x => x.UsageQuantity == usageQuantity);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialUnit))
        {
            var materialUnit = queryDto.MaterialUnit;
            exp = exp.And(x => x.MaterialUnit != null && x.MaterialUnit.Contains(materialUnit));
        }

        if (queryDto?.ScrapRate.HasValue == true)
        {
            var scrapRate = queryDto.ScrapRate.Value;
            exp = exp.And(x => x.ScrapRate == scrapRate);
        }

        if (queryDto?.ActualUsageQuantity.HasValue == true)
        {
            var actualUsageQuantity = queryDto.ActualUsageQuantity.Value;
            exp = exp.And(x => x.ActualUsageQuantity == actualUsageQuantity);
        }

        if (queryDto?.OperationSeq.HasValue == true)
        {
            var operationSeq = queryDto.OperationSeq.Value;
            exp = exp.And(x => x.OperationSeq == operationSeq);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.WorkCenter))
        {
            var workCenter = queryDto.WorkCenter;
            exp = exp.And(x => x.WorkCenter != null && x.WorkCenter.Contains(workCenter));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Position))
        {
            var position = queryDto.Position;
            exp = exp.And(x => x.Position != null && x.Position.Contains(position));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SubstituteGroup))
        {
            var substituteGroup = queryDto.SubstituteGroup;
            exp = exp.And(x => x.SubstituteGroup != null && x.SubstituteGroup.Contains(substituteGroup));
        }

        if (queryDto?.SubstitutePriority.HasValue == true)
        {
            var substitutePriority = queryDto.SubstitutePriority.Value;
            exp = exp.And(x => x.SubstitutePriority == substitutePriority);
        }

        if (queryDto?.IsOptional.HasValue == true)
        {
            var isOptional = queryDto.IsOptional.Value;
            exp = exp.And(x => x.IsOptional == isOptional);
        }

        if (queryDto?.IsPhantom.HasValue == true)
        {
            var isPhantom = queryDto.IsPhantom.Value;
            exp = exp.And(x => x.IsPhantom == isPhantom);
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
    private static bool HasAnyListQueryFilter(TaktBillOfMaterialItemQueryDto? queryDto)
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
        if (queryDto.BillOfMaterialId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BomCode))
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
        if (queryDto.UsageQuantity.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialUnit))
        {
            return true;
        }
        if (queryDto.ScrapRate.HasValue)
        {
            return true;
        }
        if (queryDto.ActualUsageQuantity.HasValue)
        {
            return true;
        }
        if (queryDto.OperationSeq.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.WorkCenter))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Position))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SubstituteGroup))
        {
            return true;
        }
        if (queryDto.SubstitutePriority.HasValue)
        {
            return true;
        }
        if (queryDto.IsOptional.HasValue)
        {
            return true;
        }
        if (queryDto.IsPhantom.HasValue)
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
