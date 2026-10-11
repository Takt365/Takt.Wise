// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：TaktPurchaseInfoRecordService.cs
// 创建时间：2026-09-21
// 创建人：Takt365(Cursor AI)
// 功能描述：采购信息记录应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Procurement;
using Takt.Domain.Entities.Logistics.Procurement;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Procurement;

/// <summary>
/// 采购信息记录应用服务
/// </summary>
public class TaktPurchaseInfoRecordService : TaktServiceBase, ITaktPurchaseInfoRecordService
{
    private readonly ITaktCompanyRepository<TaktPurchaseInfoRecord> _purchaseInfoRecordRepository;
    private readonly ITaktCompanyRepository<TaktPurchaseInfoRecordItem> _purchaseInfoRecordItemRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseInfoRecordRepository">采购信息记录仓储</param>
    /// <param name="purchaseInfoRecordItemRepository">PurchaseInfoRecordItem仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPurchaseInfoRecordService(
        ITaktCompanyRepository<TaktPurchaseInfoRecord> purchaseInfoRecordRepository,
        ITaktCompanyRepository<TaktPurchaseInfoRecordItem> purchaseInfoRecordItemRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _purchaseInfoRecordRepository = purchaseInfoRecordRepository;
        _purchaseInfoRecordItemRepository = purchaseInfoRecordItemRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取采购信息记录列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPurchaseInfoRecordDto>> GetPurchaseInfoRecordListAsync(TaktPurchaseInfoRecordQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktPurchaseInfoRecordDto>.Create(
                new List<TaktPurchaseInfoRecordDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _purchaseInfoRecordRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPurchaseInfoRecordDto>.Create(
            data.Adapt<List<TaktPurchaseInfoRecordDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取采购信息
    /// </summary>
    /// <param name="id">采购信息记录ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseInfoRecordDto?> GetPurchaseInfoRecordByIdAsync(long id)
    {
        var entity = await _purchaseInfoRecordRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktPurchaseInfoRecordDto>();
        await FillPurchaseInfoRecordDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取采购价格选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPurchaseInfoRecordOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _purchaseInfoRecordRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.MaterialDescription ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.PurchaseInfoRecordCode,
            DictLabel = e.MaterialDescription ?? e.PurchaseInfoRecordCode,
        }).ToList();
    }

    /// <summary>
    /// 创建采购信息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseInfoRecordDto> CreatePurchaseInfoRecordAsync(TaktPurchaseInfoRecordCreateDto dto)
    {
        var entity = dto.Adapt<TaktPurchaseInfoRecord>();
        var isUnique_ix_takt_logistics_procurement_purchase_info_record_code_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseInfoRecordRepository,
            x => x.PlantCode == entity.PlantCode
                && x.PurchaseInfoRecordCode == entity.PurchaseInfoRecordCode);
        if (!isUnique_ix_takt_logistics_procurement_purchase_info_record_code_unique)
        {
            throw new TaktBusinessException("采购信息记录的PlantCode、PurchaseInfoRecordCode已存在");
        }
        entity = await _purchaseInfoRecordRepository.CreateAsync(entity);
                await SavePurchaseInfoRecordChildrenAsync(entity, dto);
        return await GetPurchaseInfoRecordByIdAsync(entity.Id) ?? entity.Adapt<TaktPurchaseInfoRecordDto>();
    }

    /// <summary>
    /// 更新采购信息
    /// </summary>
    /// <param name="id">采购信息记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseInfoRecordDto> UpdatePurchaseInfoRecordAsync(long id, TaktPurchaseInfoRecordUpdateDto dto)
    {
        var entity = await _purchaseInfoRecordRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购信息记录不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_procurement_purchase_info_record_code_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseInfoRecordRepository,
            x => x.PlantCode == entity.PlantCode
                && x.PurchaseInfoRecordCode == entity.PurchaseInfoRecordCode,
            id);
        if (!isUnique_ix_takt_logistics_procurement_purchase_info_record_code_unique)
        {
            throw new TaktBusinessException("采购信息记录的PlantCode、PurchaseInfoRecordCode已存在");
        }
        await _purchaseInfoRecordRepository.UpdateAsync(entity);
                await SavePurchaseInfoRecordChildrenAsync(entity, dto);
        return await GetPurchaseInfoRecordByIdAsync(id) ?? throw new TaktBusinessException("采购信息记录不存在");
    }

    /// <summary>
    /// 删除采购信息
    /// </summary>
    /// <param name="id">采购信息记录ID</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseInfoRecordByIdAsync(long id)
    {
        var entity = await _purchaseInfoRecordRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购信息记录不存在或已删除");
        }
        await _purchaseInfoRecordItemRepository.DeleteAsync(x => x.PurchaseInfoRecordId == entity.Id);
        var deleted = await _purchaseInfoRecordRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("采购信息记录不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除采购信息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseInfoRecordBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePurchaseInfoRecordByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPurchaseInfoRecordTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPurchaseInfoRecordTemplateDto>(
            sheetName ?? "采购信息记录导入模板",
            fileName ?? "采购信息记录导入模板.xlsx");
    }

    /// <summary>
    /// 导入采购信息
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPurchaseInfoRecordAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPurchaseInfoRecordImportDto>(fileStream, sheetName ?? "采购信息记录导入模板");
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
                var entity = rows[i].Adapt<TaktPurchaseInfoRecord>();
                var importKey = $"{entity.PlantCode}|{entity.PurchaseInfoRecordCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、PurchaseInfoRecordCode）");
                }
                var isUnique_ix_takt_logistics_procurement_purchase_info_record_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _purchaseInfoRecordRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.PurchaseInfoRecordCode == entity.PurchaseInfoRecordCode);
                if (!isUnique_ix_takt_logistics_procurement_purchase_info_record_code_unique)
                {
                    throw new TaktBusinessException("采购信息记录的PlantCode、PurchaseInfoRecordCode已存在");
                }
                await _purchaseInfoRecordRepository.CreateAsync(entity);
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
    /// 导出采购信息
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPurchaseInfoRecordAsync(TaktPurchaseInfoRecordQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktPurchaseInfoRecordQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchaseInfoRecordExportDto>(),
                sheetName ?? "采购信息记录数据",
                fileName ?? "采购信息记录导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _purchaseInfoRecordRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchaseInfoRecordExportDto>(),
                sheetName ?? "采购信息记录数据",
                fileName ?? "采购信息记录导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPurchaseInfoRecordExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "采购信息记录数据",
            fileName ?? "采购信息记录导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废采购信息记录行标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="purchaseInfoRecordId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkPurchaseInfoRecordItemsObsoleteAsync(long purchaseInfoRecordId)
    {
        if (purchaseInfoRecordId <= 0)
        {
            return;
        }
        var rows = await _purchaseInfoRecordItemRepository.GetListAsync(
            x => x.PurchaseInfoRecordId == purchaseInfoRecordId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _purchaseInfoRecordItemRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充采购信息记录详情（加载 OneToMany 子表：采购信息记录行）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillPurchaseInfoRecordDetailsAsync(TaktPurchaseInfoRecordDto dto, TaktPurchaseInfoRecord entity)
    {
        if (dto == null)
        {
            return;
        }
        // 采购信息记录行 → dto.Items（含作废行）
        var items = await _purchaseInfoRecordItemRepository.GetListAsync(x => x.PurchaseInfoRecordId == entity.Id);
        dto.Items = items.Adapt<List<TaktPurchaseInfoRecordItemDto>>();
    }

    /// <summary>
    /// 保存采购信息记录子表级联（采购信息记录行；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SavePurchaseInfoRecordChildrenAsync(TaktPurchaseInfoRecord entity, TaktPurchaseInfoRecordCreateDto dto)
    {
        // 采购信息记录行（Items）
        List<TaktPurchaseInfoRecordItemUpdateDto>? itemsForSave;
        if (dto is TaktPurchaseInfoRecordUpdateDto updateDtoForItems && updateDtoForItems.Items != null)
        {
            itemsForSave = updateDtoForItems.Items;
        }
        else if (dto.Items != null)
        {
            itemsForSave = dto.Items.Adapt<List<TaktPurchaseInfoRecordItemUpdateDto>>();
        }
        else
        {
            itemsForSave = null;
        }
        if (itemsForSave is not { Count: > 0 })
        {
            await MarkPurchaseInfoRecordItemsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _purchaseInfoRecordItemRepository.GetListAsync(x => x.PurchaseInfoRecordId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktPurchaseInfoRecordItem>();
            for (var i = 0; i < itemsForSave.Count; i++)
            {
                var childDto = itemsForSave[i];
                childDto.PurchaseInfoRecordId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.PurchaseInfoRecordCode = entity.PurchaseInfoRecordCode;
                childDto.PriceType = entity.PriceType;
                if (childDto.PurchaseInfoRecordItemId > 0)
                {
                    if (!existingById.TryGetValue(childDto.PurchaseInfoRecordItemId, out var target))
                    {
                        throw new TaktBusinessException("采购信息记录行不存在（PurchaseInfoRecordItemId={childDto.PurchaseInfoRecordItemId}）");
                    }
                    if (target.PurchaseInfoRecordId != entity.Id)
                    {
                        throw new TaktBusinessException("采购信息记录行不属于当前主表（PurchaseInfoRecordItemId={childDto.PurchaseInfoRecordItemId}）");
                    }
                    submittedIds.Add(childDto.PurchaseInfoRecordItemId);
                    childDto.Adapt(target);
                    target.Id = childDto.PurchaseInfoRecordItemId;
                    target.PurchaseInfoRecordId = entity.Id;
                    target.IsObsolete = 0;
                    await _purchaseInfoRecordItemRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktPurchaseInfoRecordItem>();
                    child.Id = 0;
                    child.PurchaseInfoRecordId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _purchaseInfoRecordItemRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                await _purchaseInfoRecordItemRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建采购信息记录查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPurchaseInfoRecord, bool>> QueryExpression(TaktPurchaseInfoRecordQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPurchaseInfoRecord>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.PurchaseInfoRecordCode != null && x.PurchaseInfoRecordCode.Contains(keywords))
                || (x.PriceType != null && x.PriceType.Contains(keywords))
                || (x.SupplierCode != null && x.SupplierCode.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.MaterialDescription != null && x.MaterialDescription.Contains(keywords))
                || (x.PurchaseGroup != null && x.PurchaseGroup.Contains(keywords))
                || (x.TaxCode != null && x.TaxCode.Contains(keywords))
                || (x.PurchaseInquiryCode != null && x.PurchaseInquiryCode.Contains(keywords))
                || (x.VariableKey != null && x.VariableKey.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchaseInfoRecordCode))
        {
            var purchaseInfoRecordCode = queryDto.PurchaseInfoRecordCode;
            exp = exp.And(x => x.PurchaseInfoRecordCode != null && x.PurchaseInfoRecordCode.Contains(purchaseInfoRecordCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PriceType))
        {
            var priceType = queryDto.PriceType;
            exp = exp.And(x => x.PriceType != null && x.PriceType.Contains(priceType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SupplierCode))
        {
            var supplierCode = queryDto.SupplierCode;
            exp = exp.And(x => x.SupplierCode != null && x.SupplierCode.Contains(supplierCode));
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

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchaseGroup))
        {
            var purchaseGroup = queryDto.PurchaseGroup;
            exp = exp.And(x => x.PurchaseGroup != null && x.PurchaseGroup.Contains(purchaseGroup));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TaxCode))
        {
            var taxCode = queryDto.TaxCode;
            exp = exp.And(x => x.TaxCode != null && x.TaxCode.Contains(taxCode));
        }

        if (queryDto?.GrBasedInvoiceInspection.HasValue == true)
        {
            var grBasedInvoiceInspection = queryDto.GrBasedInvoiceInspection.Value;
            exp = exp.And(x => x.GrBasedInvoiceInspection == grBasedInvoiceInspection);
        }

        if (queryDto?.PricingDateControl.HasValue == true)
        {
            var pricingDateControl = queryDto.PricingDateControl.Value;
            exp = exp.And(x => x.PricingDateControl == pricingDateControl);
        }

        if (queryDto?.PurchaseInquiryId.HasValue == true)
        {
            var purchaseInquiryId = queryDto.PurchaseInquiryId.Value;
            exp = exp.And(x => x.PurchaseInquiryId == purchaseInquiryId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchaseInquiryCode))
        {
            var purchaseInquiryCode = queryDto.PurchaseInquiryCode;
            exp = exp.And(x => x.PurchaseInquiryCode != null && x.PurchaseInquiryCode.Contains(purchaseInquiryCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.VariableKey))
        {
            var variableKey = queryDto.VariableKey;
            exp = exp.And(x => x.VariableKey != null && x.VariableKey.Contains(variableKey));
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

        if (queryDto?.ValidFromStart.HasValue == true)
        {
            var validFromStart = queryDto.ValidFromStart.Value;
            exp = exp.And(x => x.ValidFrom >= validFromStart);
        }

        if (queryDto?.ValidFromEnd.HasValue == true)
        {
            var validFromEnd = queryDto.ValidFromEnd.Value;
            exp = exp.And(x => x.ValidFrom <= validFromEnd);
        }

        if (queryDto?.ValidToStart.HasValue == true)
        {
            var validToStart = queryDto.ValidToStart.Value;
            exp = exp.And(x => x.ValidTo >= validToStart);
        }

        if (queryDto?.ValidToEnd.HasValue == true)
        {
            var validToEnd = queryDto.ValidToEnd.Value;
            exp = exp.And(x => x.ValidTo <= validToEnd);
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
    private static bool HasAnyListQueryFilter(TaktPurchaseInfoRecordQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseInfoRecordCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PriceType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SupplierCode))
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
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TaxCode))
        {
            return true;
        }
        if (queryDto.GrBasedInvoiceInspection.HasValue)
        {
            return true;
        }
        if (queryDto.PricingDateControl.HasValue)
        {
            return true;
        }
        if (queryDto.PurchaseInquiryId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseInquiryCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.VariableKey))
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
        if (queryDto.ValidFromStart.HasValue || queryDto.ValidFromEnd.HasValue)
        {
            return true;
        }
        if (queryDto.ValidToStart.HasValue || queryDto.ValidToEnd.HasValue)
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
