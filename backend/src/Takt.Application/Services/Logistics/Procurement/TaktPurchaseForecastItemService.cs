// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：TaktPurchaseForecastItemService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：采购预测明细应用服务实现
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
/// 采购预测明细应用服务
/// </summary>
public class TaktPurchaseForecastItemService : TaktServiceBase, ITaktPurchaseForecastItemService
{
    private readonly ITaktCompanyRepository<TaktPurchaseForecastItem> _purchaseForecastItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseForecastItemRepository">采购预测明细仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPurchaseForecastItemService(
        ITaktCompanyRepository<TaktPurchaseForecastItem> purchaseForecastItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _purchaseForecastItemRepository = purchaseForecastItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取采购预测明细列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPurchaseForecastItemDto>> GetPurchaseForecastItemListAsync(TaktPurchaseForecastItemQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktPurchaseForecastItemDto>.Create(
                new List<TaktPurchaseForecastItemDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _purchaseForecastItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPurchaseForecastItemDto>.Create(
            data.Adapt<List<TaktPurchaseForecastItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取采购预测明细
    /// </summary>
    /// <param name="id">采购预测明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseForecastItemDto?> GetPurchaseForecastItemByIdAsync(long id)
    {
        var entity = await _purchaseForecastItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktPurchaseForecastItemDto>();
    }

    /// <summary>
    /// 获取采购预测明细选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPurchaseForecastItemOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _purchaseForecastItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IsObsolete == 0,
            x => x.PurchaseForecastCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.PurchaseForecastCode,
            DictLabel = e.PurchaseForecastCode,
        }).ToList();
    }

    /// <summary>
    /// 创建采购预测明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseForecastItemDto> CreatePurchaseForecastItemAsync(TaktPurchaseForecastItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktPurchaseForecastItem>();
        entity.IsObsolete = 0;
        var isUnique_ix_takt_logistics_procurement_purchase_forecast_item_month_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseForecastItemRepository,
            x => x.PurchaseForecastId == entity.PurchaseForecastId
                && x.FiscalYear == entity.FiscalYear
                && x.PlanMonth == entity.PlanMonth);
        if (!isUnique_ix_takt_logistics_procurement_purchase_forecast_item_month_unique)
        {
            throw new TaktBusinessException("采购预测明细的PurchaseForecastId、FiscalYear、PlanMonth已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _purchaseForecastItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PurchaseForecastId == entity.PurchaseForecastId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.PurchaseForecastCode) ? entity.PurchaseForecastCode : entity.PurchaseForecastId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _purchaseForecastItemRepository.CreateAsync(entity);
        return await GetPurchaseForecastItemByIdAsync(entity.Id) ?? entity.Adapt<TaktPurchaseForecastItemDto>();
    }

    /// <summary>
    /// 更新采购预测明细
    /// </summary>
    /// <param name="id">采购预测明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseForecastItemDto> UpdatePurchaseForecastItemAsync(long id, TaktPurchaseForecastItemUpdateDto dto)
    {
        var entity = await _purchaseForecastItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购预测明细不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_procurement_purchase_forecast_item_month_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseForecastItemRepository,
            x => x.PurchaseForecastId == entity.PurchaseForecastId
                && x.FiscalYear == entity.FiscalYear
                && x.PlanMonth == entity.PlanMonth,
            id);
        if (!isUnique_ix_takt_logistics_procurement_purchase_forecast_item_month_unique)
        {
            throw new TaktBusinessException("采购预测明细的PurchaseForecastId、FiscalYear、PlanMonth已存在");
        }
        await _purchaseForecastItemRepository.UpdateAsync(entity);
        return await GetPurchaseForecastItemByIdAsync(id) ?? throw new TaktBusinessException("采购预测明细不存在");
    }

    /// <summary>
    /// 删除采购预测明细
    /// </summary>
    /// <param name="id">采购预测明细ID</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseForecastItemByIdAsync(long id)
    {
        var entity = await _purchaseForecastItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购预测明细不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("采购预测明细不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("采购预测明细已作废");
        }
        entity.IsObsolete = 1;
        await _purchaseForecastItemRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除采购预测明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseForecastItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePurchaseForecastItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新采购预测明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseForecastItemDto> UpdatePurchaseForecastItemObsoleteAsync(TaktPurchaseForecastItemObsoleteDto dto)
    {
        var entity = await _purchaseForecastItemRepository.GetByIdAsync(dto.PurchaseForecastItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("采购预测明细不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("采购预测明细不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _purchaseForecastItemRepository.UpdateAsync(entity);
        return await GetPurchaseForecastItemByIdAsync(dto.PurchaseForecastItemId) ?? throw new TaktBusinessException("采购预测明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPurchaseForecastItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPurchaseForecastItemTemplateDto>(
            sheetName ?? "采购预测明细导入模板",
            fileName ?? "采购预测明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入采购预测明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPurchaseForecastItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPurchaseForecastItemImportDto>(fileStream, sheetName ?? "采购预测明细导入模板");
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
                var entity = rows[i].Adapt<TaktPurchaseForecastItem>();
                var importKey = $"{entity.PurchaseForecastId}|{entity.FiscalYear}|{entity.PlanMonth}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PurchaseForecastId、FiscalYear、PlanMonth）");
                }
                var isUnique_ix_takt_logistics_procurement_purchase_forecast_item_month_unique = await _uniqueValidator.IsUniqueAsync(
                    _purchaseForecastItemRepository,
                    x => x.PurchaseForecastId == entity.PurchaseForecastId
                        && x.FiscalYear == entity.FiscalYear
                        && x.PlanMonth == entity.PlanMonth);
                if (!isUnique_ix_takt_logistics_procurement_purchase_forecast_item_month_unique)
                {
                    throw new TaktBusinessException("采购预测明细的PurchaseForecastId、FiscalYear、PlanMonth已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _purchaseForecastItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PurchaseForecastId == entity.PurchaseForecastId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.PurchaseForecastCode) ? entity.PurchaseForecastCode : entity.PurchaseForecastId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _purchaseForecastItemRepository.CreateAsync(entity);
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
    /// 导出采购预测明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPurchaseForecastItemAsync(TaktPurchaseForecastItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktPurchaseForecastItemQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchaseForecastItemExportDto>(),
                sheetName ?? "采购预测明细数据",
                fileName ?? "采购预测明细导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _purchaseForecastItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchaseForecastItemExportDto>(),
                sheetName ?? "采购预测明细数据",
                fileName ?? "采购预测明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPurchaseForecastItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "采购预测明细数据",
            fileName ?? "采购预测明细导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建采购预测明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPurchaseForecastItem, bool>> QueryExpression(TaktPurchaseForecastItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPurchaseForecastItem>();

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
                || (x.PurchaseForecastCode != null && x.PurchaseForecastCode.Contains(keywords))
                || (x.FiscalYear != null && x.FiscalYear.Contains(keywords))
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

        if (queryDto?.PurchaseForecastId.HasValue == true)
        {
            var purchaseForecastId = queryDto.PurchaseForecastId.Value;
            exp = exp.And(x => x.PurchaseForecastId == purchaseForecastId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchaseForecastCode))
        {
            var purchaseForecastCode = queryDto.PurchaseForecastCode;
            exp = exp.And(x => x.PurchaseForecastCode != null && x.PurchaseForecastCode.Contains(purchaseForecastCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            var lineNumber = queryDto.LineNumber.Value;
            exp = exp.And(x => x.LineNumber == lineNumber);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.FiscalYear))
        {
            var fiscalYear = queryDto.FiscalYear;
            exp = exp.And(x => x.FiscalYear != null && x.FiscalYear.Contains(fiscalYear));
        }

        if (queryDto?.PlanMonth.HasValue == true)
        {
            var planMonth = queryDto.PlanMonth.Value;
            exp = exp.And(x => x.PlanMonth == planMonth);
        }

        if (queryDto?.PlanQuantity001.HasValue == true)
        {
            var planQuantity001 = queryDto.PlanQuantity001.Value;
            exp = exp.And(x => x.PlanQuantity001 == planQuantity001);
        }

        if (queryDto?.PlanQuantity002.HasValue == true)
        {
            var planQuantity002 = queryDto.PlanQuantity002.Value;
            exp = exp.And(x => x.PlanQuantity002 == planQuantity002);
        }

        if (queryDto?.PlanQuantityDelta.HasValue == true)
        {
            var planQuantityDelta = queryDto.PlanQuantityDelta.Value;
            exp = exp.And(x => x.PlanQuantityDelta == planQuantityDelta);
        }

        if (queryDto?.ConvertedQuantity.HasValue == true)
        {
            var convertedQuantity = queryDto.ConvertedQuantity.Value;
            exp = exp.And(x => x.ConvertedQuantity == convertedQuantity);
        }

        if (queryDto?.EstimatedUnitPrice.HasValue == true)
        {
            var estimatedUnitPrice = queryDto.EstimatedUnitPrice.Value;
            exp = exp.And(x => x.EstimatedUnitPrice == estimatedUnitPrice);
        }

        if (queryDto?.EstimatedAmount.HasValue == true)
        {
            var estimatedAmount = queryDto.EstimatedAmount.Value;
            exp = exp.And(x => x.EstimatedAmount == estimatedAmount);
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
    private static bool HasAnyListQueryFilter(TaktPurchaseForecastItemQueryDto? queryDto)
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
        if (queryDto.PurchaseForecastId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseForecastCode))
        {
            return true;
        }
        if (queryDto.LineNumber.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FiscalYear))
        {
            return true;
        }
        if (queryDto.PlanMonth.HasValue)
        {
            return true;
        }
        if (queryDto.PlanQuantity001.HasValue)
        {
            return true;
        }
        if (queryDto.PlanQuantity002.HasValue)
        {
            return true;
        }
        if (queryDto.PlanQuantityDelta.HasValue)
        {
            return true;
        }
        if (queryDto.ConvertedQuantity.HasValue)
        {
            return true;
        }
        if (queryDto.EstimatedUnitPrice.HasValue)
        {
            return true;
        }
        if (queryDto.EstimatedAmount.HasValue)
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
