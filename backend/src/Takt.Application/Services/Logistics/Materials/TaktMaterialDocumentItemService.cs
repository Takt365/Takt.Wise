// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：TaktMaterialDocumentItemService.cs
// 创建时间：2026-09-20
// 创建人：Takt365(Cursor AI)
// 功能描述：物料凭证行项目应用服务实现
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
/// 物料凭证行项目应用服务
/// </summary>
public class TaktMaterialDocumentItemService : TaktServiceBase, ITaktMaterialDocumentItemService
{
    private readonly ITaktCompanyRepository<TaktMaterialDocumentItem> _materialDocumentItemRepository;
    private readonly ITaktCompanyRepository<TaktMaterialDocument> _materialDocumentRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="materialDocumentItemRepository">物料凭证行项目仓储</param>
    /// <param name="materialDocumentRepository">物料凭证仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMaterialDocumentItemService(
        ITaktCompanyRepository<TaktMaterialDocumentItem> materialDocumentItemRepository,
        ITaktCompanyRepository<TaktMaterialDocument> materialDocumentRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _materialDocumentItemRepository = materialDocumentItemRepository;
        _materialDocumentRepository = materialDocumentRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取物料凭证行项目列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktMaterialDocumentItemDto>> GetMaterialDocumentItemListAsync(TaktMaterialDocumentItemQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktMaterialDocumentItemDto>.Create(
                new List<TaktMaterialDocumentItemDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _materialDocumentItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktMaterialDocumentItemDto>.Create(
            data.Adapt<List<TaktMaterialDocumentItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取物料凭证行项目
    /// </summary>
    /// <param name="id">物料凭证行项目ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialDocumentItemDto?> GetMaterialDocumentItemByIdAsync(long id)
    {
        var entity = await _materialDocumentItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktMaterialDocumentItemDto>();
    }

    /// <summary>
    /// 获取物料凭证行项目选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetMaterialDocumentItemOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _materialDocumentItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IsObsolete == 0,
            x => x.MaterialDocumentCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.MaterialDocumentCode,
            DictLabel = e.MaterialDocumentCode,
        }).ToList();
    }

    /// <summary>
    /// 创建物料凭证行项目
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialDocumentItemDto> CreateMaterialDocumentItemAsync(TaktMaterialDocumentItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktMaterialDocumentItem>();
        entity.IsObsolete = 0;
        await StampMaterialDocumentItemMaterialDocumentAsync(entity, dto);
        var isUnique_ix_takt_logistics_materials_material_document_item_line_unique = await _uniqueValidator.IsUniqueAsync(
            _materialDocumentItemRepository,
            x => x.MaterialDocumentId == entity.MaterialDocumentId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_materials_material_document_item_line_unique)
        {
            throw new TaktBusinessException("物料凭证行项目的MaterialDocumentId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _materialDocumentItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.MaterialDocumentId == entity.MaterialDocumentId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.MaterialDocumentCode) ? entity.MaterialDocumentCode : entity.MaterialDocumentId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _materialDocumentItemRepository.CreateAsync(entity);
        return await GetMaterialDocumentItemByIdAsync(entity.Id) ?? entity.Adapt<TaktMaterialDocumentItemDto>();
    }

    /// <summary>
    /// 更新物料凭证行项目
    /// </summary>
    /// <param name="id">物料凭证行项目ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialDocumentItemDto> UpdateMaterialDocumentItemAsync(long id, TaktMaterialDocumentItemUpdateDto dto)
    {
        var entity = await _materialDocumentItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("物料凭证行项目不存在");
        }
        dto.Adapt(entity);
        await StampMaterialDocumentItemMaterialDocumentAsync(entity, dto);
        var isUnique_ix_takt_logistics_materials_material_document_item_line_unique = await _uniqueValidator.IsUniqueAsync(
            _materialDocumentItemRepository,
            x => x.MaterialDocumentId == entity.MaterialDocumentId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_materials_material_document_item_line_unique)
        {
            throw new TaktBusinessException("物料凭证行项目的MaterialDocumentId、LineNumber已存在");
        }
        await _materialDocumentItemRepository.UpdateAsync(entity);
        return await GetMaterialDocumentItemByIdAsync(id) ?? throw new TaktBusinessException("物料凭证行项目不存在");
    }

    /// <summary>
    /// 删除物料凭证行项目
    /// </summary>
    /// <param name="id">物料凭证行项目ID</param>
    /// <returns>任务</returns>
    public async Task DeleteMaterialDocumentItemByIdAsync(long id)
    {
        var entity = await _materialDocumentItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("物料凭证行项目不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("物料凭证行项目不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("物料凭证行项目已作废");
        }
        entity.IsObsolete = 1;
        await _materialDocumentItemRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除物料凭证行项目
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteMaterialDocumentItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteMaterialDocumentItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新物料凭证行项目作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialDocumentItemDto> UpdateMaterialDocumentItemObsoleteAsync(TaktMaterialDocumentItemObsoleteDto dto)
    {
        var entity = await _materialDocumentItemRepository.GetByIdAsync(dto.MaterialDocumentItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("物料凭证行项目不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("物料凭证行项目不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _materialDocumentItemRepository.UpdateAsync(entity);
        return await GetMaterialDocumentItemByIdAsync(dto.MaterialDocumentItemId) ?? throw new TaktBusinessException("物料凭证行项目不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetMaterialDocumentItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktMaterialDocumentItemTemplateDto>(
            sheetName ?? "物料凭证行项目导入模板",
            fileName ?? "物料凭证行项目导入模板.xlsx");
    }

    /// <summary>
    /// 导入物料凭证行项目
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportMaterialDocumentItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktMaterialDocumentItemImportDto>(fileStream, sheetName ?? "物料凭证行项目导入模板");
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
                var entity = rows[i].Adapt<TaktMaterialDocumentItem>();
                var importDto = rows[i].Adapt<TaktMaterialDocumentItemCreateDto>();
                await StampMaterialDocumentItemMaterialDocumentAsync(entity, importDto);
                var importKey = $"{entity.MaterialDocumentId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（MaterialDocumentId、LineNumber）");
                }
                var isUnique_ix_takt_logistics_materials_material_document_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _materialDocumentItemRepository,
                    x => x.MaterialDocumentId == entity.MaterialDocumentId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_materials_material_document_item_line_unique)
                {
                    throw new TaktBusinessException("物料凭证行项目的MaterialDocumentId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _materialDocumentItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.MaterialDocumentId == entity.MaterialDocumentId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.MaterialDocumentCode) ? entity.MaterialDocumentCode : entity.MaterialDocumentId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _materialDocumentItemRepository.CreateAsync(entity);
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
    /// 导出物料凭证行项目
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportMaterialDocumentItemAsync(TaktMaterialDocumentItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktMaterialDocumentItemQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMaterialDocumentItemExportDto>(),
                sheetName ?? "物料凭证行项目数据",
                fileName ?? "物料凭证行项目导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _materialDocumentItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMaterialDocumentItemExportDto>(),
                sheetName ?? "物料凭证行项目数据",
                fileName ?? "物料凭证行项目导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktMaterialDocumentItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "物料凭证行项目数据",
            fileName ?? "物料凭证行项目导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步物料凭证行项目主表外键（ManyToOne → 物料凭证）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampMaterialDocumentItemMaterialDocumentAsync(TaktMaterialDocumentItem entity, TaktMaterialDocumentItemCreateDto dto)
    {
        if (dto.MaterialDocumentId <= 0)
        {
            return;
        }
        var master = await _materialDocumentRepository.GetByIdAsync(dto.MaterialDocumentId);
        if (master == null)
        {
            throw new TaktBusinessException("物料凭证不存在");
        }
        entity.MaterialDocumentId = master.Id;
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
        if (string.IsNullOrEmpty(entity.MaterialDocumentCode))
        {
            entity.MaterialDocumentCode = master.MaterialDocumentCode;
        }
        if (string.IsNullOrEmpty(entity.PostedBy))
        {
            entity.PostedBy = master.PostedBy ?? string.Empty;
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建物料凭证行项目查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktMaterialDocumentItem, bool>> QueryExpression(TaktMaterialDocumentItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktMaterialDocumentItem>();

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
                || (x.MaterialDocumentCode != null && x.MaterialDocumentCode.Contains(keywords))
                || (x.LineId != null && x.LineId.Contains(keywords))
                || (x.ParentLineId != null && x.ParentLineId.Contains(keywords))
                || (x.LineDepth != null && x.LineDepth.Contains(keywords))
                || (x.MovementType != null && x.MovementType.Contains(keywords))
                || (x.AutoCreatedFlag != null && x.AutoCreatedFlag.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.WarehouseCode != null && x.WarehouseCode.Contains(keywords))
                || (x.BatchCode != null && x.BatchCode.Contains(keywords))
                || (x.StockType != null && x.StockType.Contains(keywords))
                || (x.RestrictedStockFlag != null && x.RestrictedStockFlag.Contains(keywords))
                || (x.SpecialStock != null && x.SpecialStock.Contains(keywords))
                || (x.SupplierCode != null && x.SupplierCode.Contains(keywords))
                || (x.CustomerCode != null && x.CustomerCode.Contains(keywords))
                || (x.DebitCreditIndicator != null && x.DebitCreditIndicator.Contains(keywords))
                || (x.CurrencyCode != null && x.CurrencyCode.Contains(keywords))
                || (x.BaseUnit != null && x.BaseUnit.Contains(keywords))
                || (x.EntryUnit != null && x.EntryUnit.Contains(keywords))
                || (x.PoPriceUnit != null && x.PoPriceUnit.Contains(keywords))
                || (x.PurchaseOrderCode != null && x.PurchaseOrderCode.Contains(keywords))
                || (x.ReferenceDocumentYear != null && x.ReferenceDocumentYear.Contains(keywords))
                || (x.ReferenceDocumentCode != null && x.ReferenceDocumentCode.Contains(keywords))
                || (x.OriginalMaterialDocumentYear != null && x.OriginalMaterialDocumentYear.Contains(keywords))
                || (x.OriginalMaterialDocumentCode != null && x.OriginalMaterialDocumentCode.Contains(keywords))
                || (x.DeliveryCompletedFlag != null && x.DeliveryCompletedFlag.Contains(keywords))
                || (x.ItemText != null && x.ItemText.Contains(keywords))
                || (x.EquipmentCode != null && x.EquipmentCode.Contains(keywords))
                || (x.GoodsRecipient != null && x.GoodsRecipient.Contains(keywords))
                || (x.UnloadingPoint != null && x.UnloadingPoint.Contains(keywords))
                || (x.BusinessAreaCode != null && x.BusinessAreaCode.Contains(keywords))
                || (x.ControllingAreaCode != null && x.ControllingAreaCode.Contains(keywords))
                || (x.TradingPartnerBusinessArea != null && x.TradingPartnerBusinessArea.Contains(keywords))
                || (x.ProductionOrderCode != null && x.ProductionOrderCode.Contains(keywords))
                || (x.AssetCode != null && x.AssetCode.Contains(keywords))
                || (x.AssetSubCode != null && x.AssetSubCode.Contains(keywords))
                || (x.FiscalYear != null && x.FiscalYear.Contains(keywords))
                || (x.PostToPreviousPeriodFlag != null && x.PostToPreviousPeriodFlag.Contains(keywords))
                || (x.PostToPreviousYearFlag != null && x.PostToPreviousYearFlag.Contains(keywords))
                || (x.AccountingDocumentCode != null && x.AccountingDocumentCode.Contains(keywords))
                || (x.RevaluationDocumentCode != null && x.RevaluationDocumentCode.Contains(keywords))
                || (x.RevaluationDocumentItem != null && x.RevaluationDocumentItem.Contains(keywords))
                || (x.ReservationCode != null && x.ReservationCode.Contains(keywords))
                || (x.FinalIssueFlag != null && x.FinalIssueFlag.Contains(keywords))
                || (x.ReceivingMaterialCode != null && x.ReceivingMaterialCode.Contains(keywords))
                || (x.ReceivingPlantCode != null && x.ReceivingPlantCode.Contains(keywords))
                || (x.ReceivingWarehouseCode != null && x.ReceivingWarehouseCode.Contains(keywords))
                || (x.ProfitCenterCode != null && x.ProfitCenterCode.Contains(keywords))
                || (x.PriceControl != null && x.PriceControl.Contains(keywords))
                || (x.ManufacturerPartMaterialCode != null && x.ManufacturerPartMaterialCode.Contains(keywords))
                || (x.MkpfReferenceCode != null && x.MkpfReferenceCode.Contains(keywords))
                || (x.ImDeliveryCode != null && x.ImDeliveryCode.Contains(keywords))
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

        if (queryDto?.MaterialDocumentId.HasValue == true)
        {
            var materialDocumentId = queryDto.MaterialDocumentId.Value;
            exp = exp.And(x => x.MaterialDocumentId == materialDocumentId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialDocumentCode))
        {
            var materialDocumentCode = queryDto.MaterialDocumentCode;
            exp = exp.And(x => x.MaterialDocumentCode != null && x.MaterialDocumentCode.Contains(materialDocumentCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            var lineNumber = queryDto.LineNumber.Value;
            exp = exp.And(x => x.LineNumber == lineNumber);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.LineId))
        {
            var lineId = queryDto.LineId;
            exp = exp.And(x => x.LineId != null && x.LineId.Contains(lineId));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ParentLineId))
        {
            var parentLineId = queryDto.ParentLineId;
            exp = exp.And(x => x.ParentLineId != null && x.ParentLineId.Contains(parentLineId));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.LineDepth))
        {
            var lineDepth = queryDto.LineDepth;
            exp = exp.And(x => x.LineDepth != null && x.LineDepth.Contains(lineDepth));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MovementType))
        {
            var movementType = queryDto.MovementType;
            exp = exp.And(x => x.MovementType != null && x.MovementType.Contains(movementType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AutoCreatedFlag))
        {
            var autoCreatedFlag = queryDto.AutoCreatedFlag;
            exp = exp.And(x => x.AutoCreatedFlag != null && x.AutoCreatedFlag.Contains(autoCreatedFlag));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialCode))
        {
            var materialCode = queryDto.MaterialCode;
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(materialCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.WarehouseCode))
        {
            var warehouseCode = queryDto.WarehouseCode;
            exp = exp.And(x => x.WarehouseCode != null && x.WarehouseCode.Contains(warehouseCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BatchCode))
        {
            var batchCode = queryDto.BatchCode;
            exp = exp.And(x => x.BatchCode != null && x.BatchCode.Contains(batchCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.StockType))
        {
            var stockType = queryDto.StockType;
            exp = exp.And(x => x.StockType != null && x.StockType.Contains(stockType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RestrictedStockFlag))
        {
            var restrictedStockFlag = queryDto.RestrictedStockFlag;
            exp = exp.And(x => x.RestrictedStockFlag != null && x.RestrictedStockFlag.Contains(restrictedStockFlag));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SpecialStock))
        {
            var specialStock = queryDto.SpecialStock;
            exp = exp.And(x => x.SpecialStock != null && x.SpecialStock.Contains(specialStock));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SupplierCode))
        {
            var supplierCode = queryDto.SupplierCode;
            exp = exp.And(x => x.SupplierCode != null && x.SupplierCode.Contains(supplierCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CustomerCode))
        {
            var customerCode = queryDto.CustomerCode;
            exp = exp.And(x => x.CustomerCode != null && x.CustomerCode.Contains(customerCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DebitCreditIndicator))
        {
            var debitCreditIndicator = queryDto.DebitCreditIndicator;
            exp = exp.And(x => x.DebitCreditIndicator != null && x.DebitCreditIndicator.Contains(debitCreditIndicator));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CurrencyCode))
        {
            var currencyCode = queryDto.CurrencyCode;
            exp = exp.And(x => x.CurrencyCode != null && x.CurrencyCode.Contains(currencyCode));
        }

        if (queryDto?.LocalCurrencyAmount.HasValue == true)
        {
            var localCurrencyAmount = queryDto.LocalCurrencyAmount.Value;
            exp = exp.And(x => x.LocalCurrencyAmount == localCurrencyAmount);
        }

        if (queryDto?.AlternativeAmount.HasValue == true)
        {
            var alternativeAmount = queryDto.AlternativeAmount.Value;
            exp = exp.And(x => x.AlternativeAmount == alternativeAmount);
        }

        if (queryDto?.Quantity.HasValue == true)
        {
            var quantity = queryDto.Quantity.Value;
            exp = exp.And(x => x.Quantity == quantity);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BaseUnit))
        {
            var baseUnit = queryDto.BaseUnit;
            exp = exp.And(x => x.BaseUnit != null && x.BaseUnit.Contains(baseUnit));
        }

        if (queryDto?.EntryQuantity.HasValue == true)
        {
            var entryQuantity = queryDto.EntryQuantity.Value;
            exp = exp.And(x => x.EntryQuantity == entryQuantity);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EntryUnit))
        {
            var entryUnit = queryDto.EntryUnit;
            exp = exp.And(x => x.EntryUnit != null && x.EntryUnit.Contains(entryUnit));
        }

        if (queryDto?.PoPriceQuantity.HasValue == true)
        {
            var poPriceQuantity = queryDto.PoPriceQuantity.Value;
            exp = exp.And(x => x.PoPriceQuantity == poPriceQuantity);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PoPriceUnit))
        {
            var poPriceUnit = queryDto.PoPriceUnit;
            exp = exp.And(x => x.PoPriceUnit != null && x.PoPriceUnit.Contains(poPriceUnit));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchaseOrderCode))
        {
            var purchaseOrderCode = queryDto.PurchaseOrderCode;
            exp = exp.And(x => x.PurchaseOrderCode != null && x.PurchaseOrderCode.Contains(purchaseOrderCode));
        }

        if (queryDto?.PurchaseOrderItem.HasValue == true)
        {
            var purchaseOrderItem = queryDto.PurchaseOrderItem.Value;
            exp = exp.And(x => x.PurchaseOrderItem == purchaseOrderItem);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ReferenceDocumentYear))
        {
            var referenceDocumentYear = queryDto.ReferenceDocumentYear;
            exp = exp.And(x => x.ReferenceDocumentYear != null && x.ReferenceDocumentYear.Contains(referenceDocumentYear));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ReferenceDocumentCode))
        {
            var referenceDocumentCode = queryDto.ReferenceDocumentCode;
            exp = exp.And(x => x.ReferenceDocumentCode != null && x.ReferenceDocumentCode.Contains(referenceDocumentCode));
        }

        if (queryDto?.ReferenceDocumentItem.HasValue == true)
        {
            var referenceDocumentItem = queryDto.ReferenceDocumentItem.Value;
            exp = exp.And(x => x.ReferenceDocumentItem == referenceDocumentItem);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.OriginalMaterialDocumentYear))
        {
            var originalMaterialDocumentYear = queryDto.OriginalMaterialDocumentYear;
            exp = exp.And(x => x.OriginalMaterialDocumentYear != null && x.OriginalMaterialDocumentYear.Contains(originalMaterialDocumentYear));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.OriginalMaterialDocumentCode))
        {
            var originalMaterialDocumentCode = queryDto.OriginalMaterialDocumentCode;
            exp = exp.And(x => x.OriginalMaterialDocumentCode != null && x.OriginalMaterialDocumentCode.Contains(originalMaterialDocumentCode));
        }

        if (queryDto?.OriginalLineNumber.HasValue == true)
        {
            var originalLineNumber = queryDto.OriginalLineNumber.Value;
            exp = exp.And(x => x.OriginalLineNumber == originalLineNumber);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DeliveryCompletedFlag))
        {
            var deliveryCompletedFlag = queryDto.DeliveryCompletedFlag;
            exp = exp.And(x => x.DeliveryCompletedFlag != null && x.DeliveryCompletedFlag.Contains(deliveryCompletedFlag));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ItemText))
        {
            var itemText = queryDto.ItemText;
            exp = exp.And(x => x.ItemText != null && x.ItemText.Contains(itemText));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EquipmentCode))
        {
            var equipmentCode = queryDto.EquipmentCode;
            exp = exp.And(x => x.EquipmentCode != null && x.EquipmentCode.Contains(equipmentCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.GoodsRecipient))
        {
            var goodsRecipient = queryDto.GoodsRecipient;
            exp = exp.And(x => x.GoodsRecipient != null && x.GoodsRecipient.Contains(goodsRecipient));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.UnloadingPoint))
        {
            var unloadingPoint = queryDto.UnloadingPoint;
            exp = exp.And(x => x.UnloadingPoint != null && x.UnloadingPoint.Contains(unloadingPoint));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BusinessAreaCode))
        {
            var businessAreaCode = queryDto.BusinessAreaCode;
            exp = exp.And(x => x.BusinessAreaCode != null && x.BusinessAreaCode.Contains(businessAreaCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ControllingAreaCode))
        {
            var controllingAreaCode = queryDto.ControllingAreaCode;
            exp = exp.And(x => x.ControllingAreaCode != null && x.ControllingAreaCode.Contains(controllingAreaCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TradingPartnerBusinessArea))
        {
            var tradingPartnerBusinessArea = queryDto.TradingPartnerBusinessArea;
            exp = exp.And(x => x.TradingPartnerBusinessArea != null && x.TradingPartnerBusinessArea.Contains(tradingPartnerBusinessArea));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProductionOrderCode))
        {
            var productionOrderCode = queryDto.ProductionOrderCode;
            exp = exp.And(x => x.ProductionOrderCode != null && x.ProductionOrderCode.Contains(productionOrderCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AssetCode))
        {
            var assetCode = queryDto.AssetCode;
            exp = exp.And(x => x.AssetCode != null && x.AssetCode.Contains(assetCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AssetSubCode))
        {
            var assetSubCode = queryDto.AssetSubCode;
            exp = exp.And(x => x.AssetSubCode != null && x.AssetSubCode.Contains(assetSubCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.FiscalYear))
        {
            var fiscalYear = queryDto.FiscalYear;
            exp = exp.And(x => x.FiscalYear != null && x.FiscalYear.Contains(fiscalYear));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PostToPreviousPeriodFlag))
        {
            var postToPreviousPeriodFlag = queryDto.PostToPreviousPeriodFlag;
            exp = exp.And(x => x.PostToPreviousPeriodFlag != null && x.PostToPreviousPeriodFlag.Contains(postToPreviousPeriodFlag));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PostToPreviousYearFlag))
        {
            var postToPreviousYearFlag = queryDto.PostToPreviousYearFlag;
            exp = exp.And(x => x.PostToPreviousYearFlag != null && x.PostToPreviousYearFlag.Contains(postToPreviousYearFlag));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AccountingDocumentCode))
        {
            var accountingDocumentCode = queryDto.AccountingDocumentCode;
            exp = exp.And(x => x.AccountingDocumentCode != null && x.AccountingDocumentCode.Contains(accountingDocumentCode));
        }

        if (queryDto?.AccountingDocumentItem.HasValue == true)
        {
            var accountingDocumentItem = queryDto.AccountingDocumentItem.Value;
            exp = exp.And(x => x.AccountingDocumentItem == accountingDocumentItem);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RevaluationDocumentCode))
        {
            var revaluationDocumentCode = queryDto.RevaluationDocumentCode;
            exp = exp.And(x => x.RevaluationDocumentCode != null && x.RevaluationDocumentCode.Contains(revaluationDocumentCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RevaluationDocumentItem))
        {
            var revaluationDocumentItem = queryDto.RevaluationDocumentItem;
            exp = exp.And(x => x.RevaluationDocumentItem != null && x.RevaluationDocumentItem.Contains(revaluationDocumentItem));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ReservationCode))
        {
            var reservationCode = queryDto.ReservationCode;
            exp = exp.And(x => x.ReservationCode != null && x.ReservationCode.Contains(reservationCode));
        }

        if (queryDto?.ReservationItem.HasValue == true)
        {
            var reservationItem = queryDto.ReservationItem.Value;
            exp = exp.And(x => x.ReservationItem == reservationItem);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.FinalIssueFlag))
        {
            var finalIssueFlag = queryDto.FinalIssueFlag;
            exp = exp.And(x => x.FinalIssueFlag != null && x.FinalIssueFlag.Contains(finalIssueFlag));
        }

        if (queryDto?.ReservationQuantity.HasValue == true)
        {
            var reservationQuantity = queryDto.ReservationQuantity.Value;
            exp = exp.And(x => x.ReservationQuantity == reservationQuantity);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ReceivingMaterialCode))
        {
            var receivingMaterialCode = queryDto.ReceivingMaterialCode;
            exp = exp.And(x => x.ReceivingMaterialCode != null && x.ReceivingMaterialCode.Contains(receivingMaterialCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ReceivingPlantCode))
        {
            var receivingPlantCode = queryDto.ReceivingPlantCode;
            exp = exp.And(x => x.ReceivingPlantCode != null && x.ReceivingPlantCode.Contains(receivingPlantCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ReceivingWarehouseCode))
        {
            var receivingWarehouseCode = queryDto.ReceivingWarehouseCode;
            exp = exp.And(x => x.ReceivingWarehouseCode != null && x.ReceivingWarehouseCode.Contains(receivingWarehouseCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProfitCenterCode))
        {
            var profitCenterCode = queryDto.ProfitCenterCode;
            exp = exp.And(x => x.ProfitCenterCode != null && x.ProfitCenterCode.Contains(profitCenterCode));
        }

        if (queryDto?.ValuatedStockQuantity.HasValue == true)
        {
            var valuatedStockQuantity = queryDto.ValuatedStockQuantity.Value;
            exp = exp.And(x => x.ValuatedStockQuantity == valuatedStockQuantity);
        }

        if (queryDto?.TotalValuatedStockValue.HasValue == true)
        {
            var totalValuatedStockValue = queryDto.TotalValuatedStockValue.Value;
            exp = exp.And(x => x.TotalValuatedStockValue == totalValuatedStockValue);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PriceControl))
        {
            var priceControl = queryDto.PriceControl;
            exp = exp.And(x => x.PriceControl != null && x.PriceControl.Contains(priceControl));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ManufacturerPartMaterialCode))
        {
            var manufacturerPartMaterialCode = queryDto.ManufacturerPartMaterialCode;
            exp = exp.And(x => x.ManufacturerPartMaterialCode != null && x.ManufacturerPartMaterialCode.Contains(manufacturerPartMaterialCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MkpfReferenceCode))
        {
            var mkpfReferenceCode = queryDto.MkpfReferenceCode;
            exp = exp.And(x => x.MkpfReferenceCode != null && x.MkpfReferenceCode.Contains(mkpfReferenceCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ImDeliveryCode))
        {
            var imDeliveryCode = queryDto.ImDeliveryCode;
            exp = exp.And(x => x.ImDeliveryCode != null && x.ImDeliveryCode.Contains(imDeliveryCode));
        }

        if (queryDto?.ImDeliveryItem.HasValue == true)
        {
            var imDeliveryItem = queryDto.ImDeliveryItem.Value;
            exp = exp.And(x => x.ImDeliveryItem == imDeliveryItem);
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
    private static bool HasAnyListQueryFilter(TaktMaterialDocumentItemQueryDto? queryDto)
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
        if (queryDto.MaterialDocumentId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialDocumentCode))
        {
            return true;
        }
        if (queryDto.LineNumber.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.LineId))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ParentLineId))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.LineDepth))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MovementType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AutoCreatedFlag))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.WarehouseCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BatchCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.StockType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RestrictedStockFlag))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SpecialStock))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SupplierCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CustomerCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DebitCreditIndicator))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CurrencyCode))
        {
            return true;
        }
        if (queryDto.LocalCurrencyAmount.HasValue)
        {
            return true;
        }
        if (queryDto.AlternativeAmount.HasValue)
        {
            return true;
        }
        if (queryDto.Quantity.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BaseUnit))
        {
            return true;
        }
        if (queryDto.EntryQuantity.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EntryUnit))
        {
            return true;
        }
        if (queryDto.PoPriceQuantity.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PoPriceUnit))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseOrderCode))
        {
            return true;
        }
        if (queryDto.PurchaseOrderItem.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ReferenceDocumentYear))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ReferenceDocumentCode))
        {
            return true;
        }
        if (queryDto.ReferenceDocumentItem.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.OriginalMaterialDocumentYear))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.OriginalMaterialDocumentCode))
        {
            return true;
        }
        if (queryDto.OriginalLineNumber.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DeliveryCompletedFlag))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ItemText))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EquipmentCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.GoodsRecipient))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.UnloadingPoint))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BusinessAreaCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ControllingAreaCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TradingPartnerBusinessArea))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProductionOrderCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AssetCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AssetSubCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FiscalYear))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PostToPreviousPeriodFlag))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PostToPreviousYearFlag))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AccountingDocumentCode))
        {
            return true;
        }
        if (queryDto.AccountingDocumentItem.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RevaluationDocumentCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RevaluationDocumentItem))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ReservationCode))
        {
            return true;
        }
        if (queryDto.ReservationItem.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FinalIssueFlag))
        {
            return true;
        }
        if (queryDto.ReservationQuantity.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ReceivingMaterialCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ReceivingPlantCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ReceivingWarehouseCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProfitCenterCode))
        {
            return true;
        }
        if (queryDto.ValuatedStockQuantity.HasValue)
        {
            return true;
        }
        if (queryDto.TotalValuatedStockValue.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PriceControl))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ManufacturerPartMaterialCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MkpfReferenceCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ImDeliveryCode))
        {
            return true;
        }
        if (queryDto.ImDeliveryItem.HasValue)
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
