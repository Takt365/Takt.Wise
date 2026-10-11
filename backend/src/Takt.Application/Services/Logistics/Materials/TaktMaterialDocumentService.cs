// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：TaktMaterialDocumentService.cs
// 创建时间：2026-09-20
// 创建人：Takt365(Cursor AI)
// 功能描述：物料凭证应用服务实现
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
/// 物料凭证应用服务
/// </summary>
public class TaktMaterialDocumentService : TaktServiceBase, ITaktMaterialDocumentService
{
    private readonly ITaktCompanyRepository<TaktMaterialDocument> _materialDocumentRepository;
    private readonly ITaktCompanyRepository<TaktMaterialDocumentItem> _materialDocumentItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="materialDocumentRepository">物料凭证仓储</param>
    /// <param name="materialDocumentItemRepository">MaterialDocumentItem仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMaterialDocumentService(
        ITaktCompanyRepository<TaktMaterialDocument> materialDocumentRepository,
        ITaktCompanyRepository<TaktMaterialDocumentItem> materialDocumentItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _materialDocumentRepository = materialDocumentRepository;
        _materialDocumentItemRepository = materialDocumentItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取物料凭证列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktMaterialDocumentDto>> GetMaterialDocumentListAsync(TaktMaterialDocumentQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktMaterialDocumentDto>.Create(
                new List<TaktMaterialDocumentDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _materialDocumentRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktMaterialDocumentDto>.Create(
            data.Adapt<List<TaktMaterialDocumentDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取物料凭证
    /// </summary>
    /// <param name="id">物料凭证ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialDocumentDto?> GetMaterialDocumentByIdAsync(long id)
    {
        var entity = await _materialDocumentRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktMaterialDocumentDto>();
        await FillMaterialDocumentDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取物料凭证选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetMaterialDocumentOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _materialDocumentRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.MaterialDocumentCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.MaterialDocumentCode,
            DictLabel = e.MaterialDocumentCode,
        }).ToList();
    }

    /// <summary>
    /// 创建物料凭证
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialDocumentDto> CreateMaterialDocumentAsync(TaktMaterialDocumentCreateDto dto)
    {
        var entity = dto.Adapt<TaktMaterialDocument>();
        var isUnique_ix_takt_logistics_materials_material_document_doc_unique = await _uniqueValidator.IsUniqueAsync(
            _materialDocumentRepository,
            x => x.MaterialDocumentYear == entity.MaterialDocumentYear
                && x.MaterialDocumentCode == entity.MaterialDocumentCode);
        if (!isUnique_ix_takt_logistics_materials_material_document_doc_unique)
        {
            throw new TaktBusinessException("物料凭证的MaterialDocumentYear、MaterialDocumentCode已存在");
        }
        entity = await _materialDocumentRepository.CreateAsync(entity);
                await SaveMaterialDocumentChildrenAsync(entity, dto);
        return await GetMaterialDocumentByIdAsync(entity.Id) ?? entity.Adapt<TaktMaterialDocumentDto>();
    }

    /// <summary>
    /// 更新物料凭证
    /// </summary>
    /// <param name="id">物料凭证ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialDocumentDto> UpdateMaterialDocumentAsync(long id, TaktMaterialDocumentUpdateDto dto)
    {
        var entity = await _materialDocumentRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("物料凭证不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_materials_material_document_doc_unique = await _uniqueValidator.IsUniqueAsync(
            _materialDocumentRepository,
            x => x.MaterialDocumentYear == entity.MaterialDocumentYear
                && x.MaterialDocumentCode == entity.MaterialDocumentCode,
            id);
        if (!isUnique_ix_takt_logistics_materials_material_document_doc_unique)
        {
            throw new TaktBusinessException("物料凭证的MaterialDocumentYear、MaterialDocumentCode已存在");
        }
        await _materialDocumentRepository.UpdateAsync(entity);
                await SaveMaterialDocumentChildrenAsync(entity, dto);
        return await GetMaterialDocumentByIdAsync(id) ?? throw new TaktBusinessException("物料凭证不存在");
    }

    /// <summary>
    /// 删除物料凭证
    /// </summary>
    /// <param name="id">物料凭证ID</param>
    /// <returns>任务</returns>
    public async Task DeleteMaterialDocumentByIdAsync(long id)
    {
        var entity = await _materialDocumentRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("物料凭证不存在或已删除");
        }
        await _materialDocumentItemRepository.DeleteAsync(x => x.MaterialDocumentId == entity.Id);
        var deleted = await _materialDocumentRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("物料凭证不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除物料凭证
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteMaterialDocumentBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteMaterialDocumentByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetMaterialDocumentTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktMaterialDocumentTemplateDto>(
            sheetName ?? "物料凭证导入模板",
            fileName ?? "物料凭证导入模板.xlsx");
    }

    /// <summary>
    /// 导入物料凭证
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportMaterialDocumentAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktMaterialDocumentImportDto>(fileStream, sheetName ?? "物料凭证导入模板");
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
                var entity = rows[i].Adapt<TaktMaterialDocument>();
                var importKey = $"{entity.MaterialDocumentYear}|{entity.MaterialDocumentCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（MaterialDocumentYear、MaterialDocumentCode）");
                }
                var isUnique_ix_takt_logistics_materials_material_document_doc_unique = await _uniqueValidator.IsUniqueAsync(
                    _materialDocumentRepository,
                    x => x.MaterialDocumentYear == entity.MaterialDocumentYear
                        && x.MaterialDocumentCode == entity.MaterialDocumentCode);
                if (!isUnique_ix_takt_logistics_materials_material_document_doc_unique)
                {
                    throw new TaktBusinessException("物料凭证的MaterialDocumentYear、MaterialDocumentCode已存在");
                }
                await _materialDocumentRepository.CreateAsync(entity);
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
    /// 导出物料凭证
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportMaterialDocumentAsync(TaktMaterialDocumentQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktMaterialDocumentQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMaterialDocumentExportDto>(),
                sheetName ?? "物料凭证数据",
                fileName ?? "物料凭证导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _materialDocumentRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMaterialDocumentExportDto>(),
                sheetName ?? "物料凭证数据",
                fileName ?? "物料凭证导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktMaterialDocumentExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "物料凭证数据",
            fileName ?? "物料凭证导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废物料凭证行项目标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="materialDocumentId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkMaterialDocumentItemsObsoleteAsync(long materialDocumentId)
    {
        if (materialDocumentId <= 0)
        {
            return;
        }
        var rows = await _materialDocumentItemRepository.GetListAsync(
            x => x.MaterialDocumentId == materialDocumentId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _materialDocumentItemRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充物料凭证详情（加载 OneToMany 子表：物料凭证行项目）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillMaterialDocumentDetailsAsync(TaktMaterialDocumentDto dto, TaktMaterialDocument entity)
    {
        if (dto == null)
        {
            return;
        }
        // 物料凭证行项目 → dto.Items（含作废行）
        var items = await _materialDocumentItemRepository.GetListAsync(x => x.MaterialDocumentId == entity.Id);
        dto.Items = items.Adapt<List<TaktMaterialDocumentItemDto>>();
    }

    /// <summary>
    /// 保存物料凭证子表级联（物料凭证行项目；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveMaterialDocumentChildrenAsync(TaktMaterialDocument entity, TaktMaterialDocumentCreateDto dto)
    {
        // 物料凭证行项目（Items）
        List<TaktMaterialDocumentItemUpdateDto>? itemsForSave;
        if (dto is TaktMaterialDocumentUpdateDto updateDtoForItems && updateDtoForItems.Items != null)
        {
            itemsForSave = updateDtoForItems.Items;
        }
        else if (dto.Items != null)
        {
            itemsForSave = dto.Items.Adapt<List<TaktMaterialDocumentItemUpdateDto>>();
        }
        else
        {
            itemsForSave = null;
        }
        if (itemsForSave is not { Count: > 0 })
        {
            await MarkMaterialDocumentItemsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _materialDocumentItemRepository.GetListAsync(x => x.MaterialDocumentId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktMaterialDocumentItem>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < itemsForSave.Count; i++)
            {
                var childDto = itemsForSave[i];
                childDto.MaterialDocumentId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.MaterialDocumentCode = entity.MaterialDocumentCode;
                childDto.PostedBy = entity.PostedBy ?? string.Empty;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("物料凭证行项目第{i + 1}项与本次提交的其他项重复（CompanyCode、MaterialDocumentId、LineNumber）");
                }
                if (childDto.MaterialDocumentItemId > 0)
                {
                    if (!existingById.TryGetValue(childDto.MaterialDocumentItemId, out var target))
                    {
                        throw new TaktBusinessException("物料凭证行项目不存在（MaterialDocumentItemId={childDto.MaterialDocumentItemId}）");
                    }
                    if (target.MaterialDocumentId != entity.Id)
                    {
                        throw new TaktBusinessException("物料凭证行项目不属于当前主表（MaterialDocumentItemId={childDto.MaterialDocumentItemId}）");
                    }
                    submittedIds.Add(childDto.MaterialDocumentItemId);
                    var isUniqueUpdate_ix_takt_logistics_materials_material_document_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _materialDocumentItemRepository,
                        x => x.MaterialDocumentId == x.MaterialDocumentId
                && x.LineNumber == x.LineNumber,
                        childDto.MaterialDocumentItemId);
                    if (!isUniqueUpdate_ix_takt_logistics_materials_material_document_item_line_unique)
                    {
                        throw new TaktBusinessException("物料凭证行项目的MaterialDocumentId、LineNumber已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.MaterialDocumentItemId;
                    target.MaterialDocumentId = entity.Id;
                    target.IsObsolete = 0;
                    await _materialDocumentItemRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_materials_material_document_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _materialDocumentItemRepository,
                        x => x.MaterialDocumentId == x.MaterialDocumentId
                && x.LineNumber == x.LineNumber);
                    if (!isUniqueCreate_ix_takt_logistics_materials_material_document_item_line_unique)
                    {
                        throw new TaktBusinessException("物料凭证行项目的MaterialDocumentId、LineNumber已存在");
                    }
                    var child = childDto.Adapt<TaktMaterialDocumentItem>();
                    child.Id = 0;
                    child.MaterialDocumentId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _materialDocumentItemRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.MaterialDocumentCode) ? entity.MaterialDocumentCode : entity.Id.ToString();
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
                await _materialDocumentItemRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建物料凭证查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktMaterialDocument, bool>> QueryExpression(TaktMaterialDocumentQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktMaterialDocument>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.MaterialDocumentCode != null && x.MaterialDocumentCode.Contains(keywords))
                || (x.MaterialDocumentYear != null && x.MaterialDocumentYear.Contains(keywords))
                || (x.TransactionEventType != null && x.TransactionEventType.Contains(keywords))
                || (x.DocumentType != null && x.DocumentType.Contains(keywords))
                || (x.RevaluationType != null && x.RevaluationType.Contains(keywords))
                || (x.ReferenceCode != null && x.ReferenceCode.Contains(keywords))
                || (x.HeaderText != null && x.HeaderText.Contains(keywords))
                || (x.BillOfLadingCode != null && x.BillOfLadingCode.Contains(keywords))
                || (x.DeliveryCode != null && x.DeliveryCode.Contains(keywords))
                || (x.TransactionCode != null && x.TransactionCode.Contains(keywords))
                || (x.PostedBy != null && x.PostedBy.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialDocumentCode))
        {
            var materialDocumentCode = queryDto.MaterialDocumentCode;
            exp = exp.And(x => x.MaterialDocumentCode != null && x.MaterialDocumentCode.Contains(materialDocumentCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialDocumentYear))
        {
            var materialDocumentYear = queryDto.MaterialDocumentYear;
            exp = exp.And(x => x.MaterialDocumentYear != null && x.MaterialDocumentYear.Contains(materialDocumentYear));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TransactionEventType))
        {
            var transactionEventType = queryDto.TransactionEventType;
            exp = exp.And(x => x.TransactionEventType != null && x.TransactionEventType.Contains(transactionEventType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DocumentType))
        {
            var documentType = queryDto.DocumentType;
            exp = exp.And(x => x.DocumentType != null && x.DocumentType.Contains(documentType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RevaluationType))
        {
            var revaluationType = queryDto.RevaluationType;
            exp = exp.And(x => x.RevaluationType != null && x.RevaluationType.Contains(revaluationType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ReferenceCode))
        {
            var referenceCode = queryDto.ReferenceCode;
            exp = exp.And(x => x.ReferenceCode != null && x.ReferenceCode.Contains(referenceCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.HeaderText))
        {
            var headerText = queryDto.HeaderText;
            exp = exp.And(x => x.HeaderText != null && x.HeaderText.Contains(headerText));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BillOfLadingCode))
        {
            var billOfLadingCode = queryDto.BillOfLadingCode;
            exp = exp.And(x => x.BillOfLadingCode != null && x.BillOfLadingCode.Contains(billOfLadingCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DeliveryCode))
        {
            var deliveryCode = queryDto.DeliveryCode;
            exp = exp.And(x => x.DeliveryCode != null && x.DeliveryCode.Contains(deliveryCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TransactionCode))
        {
            var transactionCode = queryDto.TransactionCode;
            exp = exp.And(x => x.TransactionCode != null && x.TransactionCode.Contains(transactionCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PostedBy))
        {
            var postedBy = queryDto.PostedBy;
            exp = exp.And(x => x.PostedBy != null && x.PostedBy.Contains(postedBy));
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

        if (queryDto?.DocumentDateStart.HasValue == true)
        {
            var documentDateStart = queryDto.DocumentDateStart.Value;
            exp = exp.And(x => x.DocumentDate >= documentDateStart);
        }

        if (queryDto?.DocumentDateEnd.HasValue == true)
        {
            var documentDateEnd = queryDto.DocumentDateEnd.Value;
            exp = exp.And(x => x.DocumentDate <= documentDateEnd);
        }

        if (queryDto?.PostingDateStart.HasValue == true)
        {
            var postingDateStart = queryDto.PostingDateStart.Value;
            exp = exp.And(x => x.PostingDate >= postingDateStart);
        }

        if (queryDto?.PostingDateEnd.HasValue == true)
        {
            var postingDateEnd = queryDto.PostingDateEnd.Value;
            exp = exp.And(x => x.PostingDate <= postingDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktMaterialDocumentQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialDocumentCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialDocumentYear))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TransactionEventType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DocumentType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RevaluationType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ReferenceCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.HeaderText))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BillOfLadingCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DeliveryCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TransactionCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PostedBy))
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
        if (queryDto.DocumentDateStart.HasValue || queryDto.DocumentDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.PostingDateStart.HasValue || queryDto.PostingDateEnd.HasValue)
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
