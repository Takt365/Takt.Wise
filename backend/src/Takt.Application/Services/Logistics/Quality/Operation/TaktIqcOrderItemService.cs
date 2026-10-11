// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Quality.Operation
// 文件名称：TaktIqcOrderItemService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：进货检验单明细应用服务实现
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
/// 进货检验单明细应用服务
/// </summary>
public class TaktIqcOrderItemService : TaktServiceBase, ITaktIqcOrderItemService
{
    private readonly ITaktCompanyRepository<TaktIqcOrderItem> _iqcOrderItemRepository;
    private readonly ITaktCompanyRepository<TaktIqcDefectHandling> _iqcDefectHandlingRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="iqcOrderItemRepository">进货检验单明细仓储</param>
    /// <param name="iqcDefectHandlingRepository">IqcDefectHandling仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktIqcOrderItemService(
        ITaktCompanyRepository<TaktIqcOrderItem> iqcOrderItemRepository,
        ITaktCompanyRepository<TaktIqcDefectHandling> iqcDefectHandlingRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _iqcOrderItemRepository = iqcOrderItemRepository;
        _iqcDefectHandlingRepository = iqcDefectHandlingRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取进货检验单明细列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktIqcOrderItemDto>> GetIqcOrderItemListAsync(TaktIqcOrderItemQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktIqcOrderItemDto>.Create(
                new List<TaktIqcOrderItemDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _iqcOrderItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktIqcOrderItemDto>.Create(
            data.Adapt<List<TaktIqcOrderItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取进货检验单明细
    /// </summary>
    /// <param name="id">进货检验单明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktIqcOrderItemDto?> GetIqcOrderItemByIdAsync(long id)
    {
        var entity = await _iqcOrderItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktIqcOrderItemDto>();
        await FillIqcOrderItemDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取进货检验单明细选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetIqcOrderItemOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _iqcOrderItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.JudgeStatus == 1 && x.IsObsolete == 0,
            x => x.IqcOrderCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.IqcOrderCode,
            DictLabel = e.IqcOrderCode,
        }).ToList();
    }

    /// <summary>
    /// 创建进货检验单明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktIqcOrderItemDto> CreateIqcOrderItemAsync(TaktIqcOrderItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktIqcOrderItem>();
        entity.IsObsolete = 0;
        var isUnique_ix_takt_logistics_quality_iqc_order_item_order_line_unique = await _uniqueValidator.IsUniqueAsync(
            _iqcOrderItemRepository,
            x => x.IqcOrderId == entity.IqcOrderId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_quality_iqc_order_item_order_line_unique)
        {
            throw new TaktBusinessException("进货检验单明细的IqcOrderId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _iqcOrderItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IqcOrderId == entity.IqcOrderId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.IqcOrderCode) ? entity.IqcOrderCode : entity.IqcOrderId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _iqcOrderItemRepository.CreateAsync(entity);
                await SaveIqcOrderItemChildrenAsync(entity, dto);
        return await GetIqcOrderItemByIdAsync(entity.Id) ?? entity.Adapt<TaktIqcOrderItemDto>();
    }

    /// <summary>
    /// 更新进货检验单明细
    /// </summary>
    /// <param name="id">进货检验单明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktIqcOrderItemDto> UpdateIqcOrderItemAsync(long id, TaktIqcOrderItemUpdateDto dto)
    {
        var entity = await _iqcOrderItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("进货检验单明细不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_quality_iqc_order_item_order_line_unique = await _uniqueValidator.IsUniqueAsync(
            _iqcOrderItemRepository,
            x => x.IqcOrderId == entity.IqcOrderId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_quality_iqc_order_item_order_line_unique)
        {
            throw new TaktBusinessException("进货检验单明细的IqcOrderId、LineNumber已存在");
        }
        await _iqcOrderItemRepository.UpdateAsync(entity);
                await SaveIqcOrderItemChildrenAsync(entity, dto);
        return await GetIqcOrderItemByIdAsync(id) ?? throw new TaktBusinessException("进货检验单明细不存在");
    }

    /// <summary>
    /// 删除进货检验单明细
    /// </summary>
    /// <param name="id">进货检验单明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteIqcOrderItemByIdAsync(long id)
    {
        var entity = await _iqcOrderItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("进货检验单明细不存在或已删除");
        }
        await _iqcDefectHandlingRepository.DeleteAsync(x => x.IqcOrderItemId == entity.Id);
        var deleted = await _iqcOrderItemRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("进货检验单明细不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除进货检验单明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteIqcOrderItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteIqcOrderItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新进货检验单明细状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktIqcOrderItemDto> UpdateIqcOrderItemStatusAsync(TaktIqcOrderItemStatusDto dto)
    {
        var entity = await _iqcOrderItemRepository.GetByIdAsync(dto.IqcOrderItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("进货检验单明细不存在");
        }
        entity.JudgeStatus = dto.JudgeStatus;
        await _iqcOrderItemRepository.UpdateAsync(entity);
        return await GetIqcOrderItemByIdAsync(dto.IqcOrderItemId) ?? throw new TaktBusinessException("进货检验单明细不存在");
    }

    /// <summary>
    /// 更新进货检验单明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktIqcOrderItemDto> UpdateIqcOrderItemObsoleteAsync(TaktIqcOrderItemObsoleteDto dto)
    {
        var entity = await _iqcOrderItemRepository.GetByIdAsync(dto.IqcOrderItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("进货检验单明细不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("进货检验单明细不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _iqcOrderItemRepository.UpdateAsync(entity);
        return await GetIqcOrderItemByIdAsync(dto.IqcOrderItemId) ?? throw new TaktBusinessException("进货检验单明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetIqcOrderItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktIqcOrderItemTemplateDto>(
            sheetName ?? "进货检验单明细导入模板",
            fileName ?? "进货检验单明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入进货检验单明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportIqcOrderItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktIqcOrderItemImportDto>(fileStream, sheetName ?? "进货检验单明细导入模板");
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
                var entity = rows[i].Adapt<TaktIqcOrderItem>();
                var importKey = $"{entity.IqcOrderId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（IqcOrderId、LineNumber）");
                }
                var isUnique_ix_takt_logistics_quality_iqc_order_item_order_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _iqcOrderItemRepository,
                    x => x.IqcOrderId == entity.IqcOrderId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_quality_iqc_order_item_order_line_unique)
                {
                    throw new TaktBusinessException("进货检验单明细的IqcOrderId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _iqcOrderItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IqcOrderId == entity.IqcOrderId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.IqcOrderCode) ? entity.IqcOrderCode : entity.IqcOrderId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _iqcOrderItemRepository.CreateAsync(entity);
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
    /// 导出进货检验单明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportIqcOrderItemAsync(TaktIqcOrderItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktIqcOrderItemQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktIqcOrderItemExportDto>(),
                sheetName ?? "进货检验单明细数据",
                fileName ?? "进货检验单明细导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _iqcOrderItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktIqcOrderItemExportDto>(),
                sheetName ?? "进货检验单明细数据",
                fileName ?? "进货检验单明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktIqcOrderItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "进货检验单明细数据",
            fileName ?? "进货检验单明细导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废进货检验不良处理记录标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="iqcOrderItemId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkIqcDefectHandlingsObsoleteAsync(long iqcOrderItemId)
    {
        if (iqcOrderItemId <= 0)
        {
            return;
        }
        var rows = await _iqcDefectHandlingRepository.GetListAsync(
            x => x.IqcOrderItemId == iqcOrderItemId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _iqcDefectHandlingRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充进货检验单明细详情（加载 OneToMany 子表：进货检验不良处理记录）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillIqcOrderItemDetailsAsync(TaktIqcOrderItemDto dto, TaktIqcOrderItem entity)
    {
        if (dto == null)
        {
            return;
        }
        // 进货检验不良处理记录 → dto.DefectHandlings（含作废行）
        var defecthandlings = await _iqcDefectHandlingRepository.GetListAsync(x => x.IqcOrderItemId == entity.Id);
        dto.DefectHandlings = defecthandlings.Adapt<List<TaktIqcDefectHandlingDto>>();
    }

    /// <summary>
    /// 保存进货检验单明细子表级联（进货检验不良处理记录；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveIqcOrderItemChildrenAsync(TaktIqcOrderItem entity, TaktIqcOrderItemCreateDto dto)
    {
        // 进货检验不良处理记录（DefectHandlings）
        List<TaktIqcDefectHandlingUpdateDto>? defectHandlingsForSave;
        if (dto is TaktIqcOrderItemUpdateDto updateDtoForDefectHandlings && updateDtoForDefectHandlings.DefectHandlings != null)
        {
            defectHandlingsForSave = updateDtoForDefectHandlings.DefectHandlings;
        }
        else if (dto.DefectHandlings != null)
        {
            defectHandlingsForSave = dto.DefectHandlings.Adapt<List<TaktIqcDefectHandlingUpdateDto>>();
        }
        else
        {
            defectHandlingsForSave = null;
        }
        if (defectHandlingsForSave is not { Count: > 0 })
        {
            await MarkIqcDefectHandlingsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _iqcDefectHandlingRepository.GetListAsync(x => x.IqcOrderItemId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktIqcDefectHandling>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < defectHandlingsForSave.Count; i++)
            {
                var childDto = defectHandlingsForSave[i];
                childDto.IqcOrderItemId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.IqcOrderCode = entity.IqcOrderCode;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("进货检验不良处理记录第{i + 1}项与本次提交的其他项重复（CompanyCode、IqcOrderItemId、LineNumber）");
                }
                if (childDto.IqcDefectHandlingId > 0)
                {
                    if (!existingById.TryGetValue(childDto.IqcDefectHandlingId, out var target))
                    {
                        throw new TaktBusinessException("进货检验不良处理记录不存在（IqcDefectHandlingId={childDto.IqcDefectHandlingId}）");
                    }
                    if (target.IqcOrderItemId != entity.Id)
                    {
                        throw new TaktBusinessException("进货检验不良处理记录不属于当前主表（IqcDefectHandlingId={childDto.IqcDefectHandlingId}）");
                    }
                    submittedIds.Add(childDto.IqcDefectHandlingId);
                    childDto.Adapt(target);
                    target.Id = childDto.IqcDefectHandlingId;
                    target.IqcOrderItemId = entity.Id;
                    target.IsObsolete = 0;
                    await _iqcDefectHandlingRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktIqcDefectHandling>();
                    child.Id = 0;
                    child.IqcOrderItemId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _iqcDefectHandlingRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.IqcOrderCode) ? entity.IqcOrderCode : entity.Id.ToString();
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
                await _iqcDefectHandlingRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建进货检验单明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktIqcOrderItem, bool>> QueryExpression(TaktIqcOrderItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktIqcOrderItem>();

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
                || (x.IqcOrderCode != null && x.IqcOrderCode.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.MaterialDescription != null && x.MaterialDescription.Contains(keywords))
                || (x.BatchCode != null && x.BatchCode.Contains(keywords))
                || (x.StandardCode != null && x.StandardCode.Contains(keywords))
                || (x.SamplingSchemeCode != null && x.SamplingSchemeCode.Contains(keywords))
                || (x.SampleSerialCode != null && x.SampleSerialCode.Contains(keywords))
                || (x.InspectionDescription != null && x.InspectionDescription.Contains(keywords))
                || (x.InspectorName != null && x.InspectorName.Contains(keywords))
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

        if (queryDto?.IqcOrderId.HasValue == true)
        {
            var iqcOrderId = queryDto.IqcOrderId.Value;
            exp = exp.And(x => x.IqcOrderId == iqcOrderId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.IqcOrderCode))
        {
            var iqcOrderCode = queryDto.IqcOrderCode;
            exp = exp.And(x => x.IqcOrderCode != null && x.IqcOrderCode.Contains(iqcOrderCode));
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

        if (!string.IsNullOrWhiteSpace(queryDto?.BatchCode))
        {
            var batchCode = queryDto.BatchCode;
            exp = exp.And(x => x.BatchCode != null && x.BatchCode.Contains(batchCode));
        }

        if (queryDto?.PurchaseQuantity.HasValue == true)
        {
            var purchaseQuantity = queryDto.PurchaseQuantity.Value;
            exp = exp.And(x => x.PurchaseQuantity == purchaseQuantity);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.StandardCode))
        {
            var standardCode = queryDto.StandardCode;
            exp = exp.And(x => x.StandardCode != null && x.StandardCode.Contains(standardCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SamplingSchemeCode))
        {
            var samplingSchemeCode = queryDto.SamplingSchemeCode;
            exp = exp.And(x => x.SamplingSchemeCode != null && x.SamplingSchemeCode.Contains(samplingSchemeCode));
        }

        if (queryDto?.InspectionMethod.HasValue == true)
        {
            var inspectionMethod = queryDto.InspectionMethod.Value;
            exp = exp.And(x => x.InspectionMethod == inspectionMethod);
        }

        if (queryDto?.SampleQuantity.HasValue == true)
        {
            var sampleQuantity = queryDto.SampleQuantity.Value;
            exp = exp.And(x => x.SampleQuantity == sampleQuantity);
        }

        if (queryDto?.QualifiedQuantity.HasValue == true)
        {
            var qualifiedQuantity = queryDto.QualifiedQuantity.Value;
            exp = exp.And(x => x.QualifiedQuantity == qualifiedQuantity);
        }

        if (queryDto?.UnqualifiedQuantity.HasValue == true)
        {
            var unqualifiedQuantity = queryDto.UnqualifiedQuantity.Value;
            exp = exp.And(x => x.UnqualifiedQuantity == unqualifiedQuantity);
        }

        if (queryDto?.InspectionReturnQuantity.HasValue == true)
        {
            var inspectionReturnQuantity = queryDto.InspectionReturnQuantity.Value;
            exp = exp.And(x => x.InspectionReturnQuantity == inspectionReturnQuantity);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SampleSerialCode))
        {
            var sampleSerialCode = queryDto.SampleSerialCode;
            exp = exp.And(x => x.SampleSerialCode != null && x.SampleSerialCode.Contains(sampleSerialCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.InspectionDescription))
        {
            var inspectionDescription = queryDto.InspectionDescription;
            exp = exp.And(x => x.InspectionDescription != null && x.InspectionDescription.Contains(inspectionDescription));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.InspectorName))
        {
            var inspectorBy = queryDto.InspectorName;
            exp = exp.And(x => x.InspectorName != null && x.InspectorName.Contains(inspectorBy));
        }

        if (queryDto?.JudgeStatus.HasValue == true)
        {
            var judgeStatus = queryDto.JudgeStatus.Value;
            exp = exp.And(x => x.JudgeStatus == judgeStatus);
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

        if (queryDto?.InspectionDateStart.HasValue == true)
        {
            var inspectionDateStart = queryDto.InspectionDateStart.Value;
            exp = exp.And(x => x.InspectionDate >= inspectionDateStart);
        }

        if (queryDto?.InspectionDateEnd.HasValue == true)
        {
            var inspectionDateEnd = queryDto.InspectionDateEnd.Value;
            exp = exp.And(x => x.InspectionDate <= inspectionDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktIqcOrderItemQueryDto? queryDto)
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
        if (queryDto.IqcOrderId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.IqcOrderCode))
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
        if (!string.IsNullOrWhiteSpace(queryDto.BatchCode))
        {
            return true;
        }
        if (queryDto.PurchaseQuantity.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.StandardCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SamplingSchemeCode))
        {
            return true;
        }
        if (queryDto.InspectionMethod.HasValue)
        {
            return true;
        }
        if (queryDto.SampleQuantity.HasValue)
        {
            return true;
        }
        if (queryDto.QualifiedQuantity.HasValue)
        {
            return true;
        }
        if (queryDto.UnqualifiedQuantity.HasValue)
        {
            return true;
        }
        if (queryDto.InspectionReturnQuantity.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SampleSerialCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.InspectionDescription))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.InspectorName))
        {
            return true;
        }
        if (queryDto.JudgeStatus.HasValue)
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
        if (queryDto.InspectionDateStart.HasValue || queryDto.InspectionDateEnd.HasValue)
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
