// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：TaktMaterialMovingPriceService.cs
// 创建时间：2026-08-21
// 创建人：Takt365(Cursor AI)
// 功能描述：移动价格应用服务实现
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
/// 移动价格应用服务
/// </summary>
public class TaktMaterialMovingPriceService : TaktServiceBase, ITaktMaterialMovingPriceService
{
    private readonly ITaktCompanyRepository<TaktMaterialMovingPrice> _materialMovingPriceRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="materialMovingPriceRepository">移动价格仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMaterialMovingPriceService(
        ITaktCompanyRepository<TaktMaterialMovingPrice> materialMovingPriceRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _materialMovingPriceRepository = materialMovingPriceRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取移动价格列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktMaterialMovingPriceDto>> GetMaterialMovingPriceListAsync(TaktMaterialMovingPriceQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktMaterialMovingPriceDto>.Create(
                new List<TaktMaterialMovingPriceDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _materialMovingPriceRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktMaterialMovingPriceDto>.Create(
            data.Adapt<List<TaktMaterialMovingPriceDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取移动价格
    /// </summary>
    /// <param name="id">移动价格ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialMovingPriceDto?> GetMaterialMovingPriceByIdAsync(long id)
    {
        var entity = await _materialMovingPriceRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktMaterialMovingPriceDto>();
    }

    /// <summary>
    /// 获取移动价格选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetMaterialMovingPriceOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _materialMovingPriceRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.MaterialCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.MaterialCode,
            DictLabel = e.MaterialCode,
        }).ToList();
    }

    /// <summary>
    /// 创建移动价格
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialMovingPriceDto> CreateMaterialMovingPriceAsync(TaktMaterialMovingPriceCreateDto dto)
    {
        var entity = dto.Adapt<TaktMaterialMovingPrice>();
        var isUnique_ix_material_moving_price_unique = await _uniqueValidator.IsUniqueAsync(
            _materialMovingPriceRepository,
            x => x.PlantCode == entity.PlantCode
                && x.MaterialCode == entity.MaterialCode
                && x.ValuationPeriod == entity.ValuationPeriod);
        if (!isUnique_ix_material_moving_price_unique)
        {
            throw new TaktBusinessException("移动价格的PlantCode、MaterialCode、ValuationPeriod已存在");
        }
        entity = await _materialMovingPriceRepository.CreateAsync(entity);
        return await GetMaterialMovingPriceByIdAsync(entity.Id) ?? entity.Adapt<TaktMaterialMovingPriceDto>();
    }

    /// <summary>
    /// 更新移动价格
    /// </summary>
    /// <param name="id">移动价格ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialMovingPriceDto> UpdateMaterialMovingPriceAsync(long id, TaktMaterialMovingPriceUpdateDto dto)
    {
        var entity = await _materialMovingPriceRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("移动价格不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_material_moving_price_unique = await _uniqueValidator.IsUniqueAsync(
            _materialMovingPriceRepository,
            x => x.PlantCode == entity.PlantCode
                && x.MaterialCode == entity.MaterialCode
                && x.ValuationPeriod == entity.ValuationPeriod,
            id);
        if (!isUnique_ix_material_moving_price_unique)
        {
            throw new TaktBusinessException("移动价格的PlantCode、MaterialCode、ValuationPeriod已存在");
        }
        await _materialMovingPriceRepository.UpdateAsync(entity);
        return await GetMaterialMovingPriceByIdAsync(id) ?? throw new TaktBusinessException("移动价格不存在");
    }

    /// <summary>
    /// 删除移动价格
    /// </summary>
    /// <param name="id">移动价格ID</param>
    /// <returns>任务</returns>
    public async Task DeleteMaterialMovingPriceByIdAsync(long id)
    {
        var deleted = await _materialMovingPriceRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("移动价格不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除移动价格
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteMaterialMovingPriceBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteMaterialMovingPriceByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetMaterialMovingPriceTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktMaterialMovingPriceTemplateDto>(
            sheetName ?? "移动价格导入模板",
            fileName ?? "移动价格导入模板.xlsx");
    }

    /// <summary>
    /// 导入移动价格
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportMaterialMovingPriceAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktMaterialMovingPriceImportDto>(fileStream, sheetName ?? "移动价格导入模板");
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
                var entity = rows[i].Adapt<TaktMaterialMovingPrice>();
                var importKey = $"{entity.PlantCode}|{entity.MaterialCode}|{entity.ValuationPeriod}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、MaterialCode、ValuationPeriod）");
                }
                var isUnique_ix_material_moving_price_unique = await _uniqueValidator.IsUniqueAsync(
                    _materialMovingPriceRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.MaterialCode == entity.MaterialCode
                        && x.ValuationPeriod == entity.ValuationPeriod);
                if (!isUnique_ix_material_moving_price_unique)
                {
                    throw new TaktBusinessException("移动价格的PlantCode、MaterialCode、ValuationPeriod已存在");
                }
                await _materialMovingPriceRepository.CreateAsync(entity);
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
    /// 导出移动价格
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportMaterialMovingPriceAsync(TaktMaterialMovingPriceQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktMaterialMovingPriceQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMaterialMovingPriceExportDto>(),
                sheetName ?? "移动价格数据",
                fileName ?? "移动价格导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _materialMovingPriceRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMaterialMovingPriceExportDto>(),
                sheetName ?? "移动价格数据",
                fileName ?? "移动价格导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktMaterialMovingPriceExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "移动价格数据",
            fileName ?? "移动价格导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建移动价格查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktMaterialMovingPrice, bool>> QueryExpression(TaktMaterialMovingPriceQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktMaterialMovingPrice>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ValuationPeriod != null && x.ValuationPeriod.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.Valuation != null && x.Valuation.Contains(keywords))
                || (x.PriceControl != null && x.PriceControl.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.ValuationPeriod))
        {
            var valuationPeriod = queryDto.ValuationPeriod;
            exp = exp.And(x => x.ValuationPeriod != null && x.ValuationPeriod.Contains(valuationPeriod));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialCode))
        {
            var materialCode = queryDto.MaterialCode;
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(materialCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Valuation))
        {
            var valuation = queryDto.Valuation;
            exp = exp.And(x => x.Valuation != null && x.Valuation.Contains(valuation));
        }

        if (queryDto?.StockQuantity.HasValue == true)
        {
            var stockQuantity = queryDto.StockQuantity.Value;
            exp = exp.And(x => x.StockQuantity == stockQuantity);
        }

        if (queryDto?.StockAmount.HasValue == true)
        {
            var stockAmount = queryDto.StockAmount.Value;
            exp = exp.And(x => x.StockAmount == stockAmount);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PriceControl))
        {
            var priceControl = queryDto.PriceControl;
            exp = exp.And(x => x.PriceControl != null && x.PriceControl.Contains(priceControl));
        }

        if (queryDto?.MovingPrice.HasValue == true)
        {
            var movingPrice = queryDto.MovingPrice.Value;
            exp = exp.And(x => x.MovingPrice == movingPrice);
        }

        if (queryDto?.PriceUnit.HasValue == true)
        {
            var priceUnit = queryDto.PriceUnit.Value;
            exp = exp.And(x => x.PriceUnit == priceUnit);
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
    private static bool HasAnyListQueryFilter(TaktMaterialMovingPriceQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.ValuationPeriod))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Valuation))
        {
            return true;
        }
        if (queryDto.StockQuantity.HasValue)
        {
            return true;
        }
        if (queryDto.StockAmount.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PriceControl))
        {
            return true;
        }
        if (queryDto.MovingPrice.HasValue)
        {
            return true;
        }
        if (queryDto.PriceUnit.HasValue)
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
    /// 获取在库金额统计（数据看板；按评估期间汇总 StockAmount，并按评估类别分项）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>在库金额统计</returns>
    public async Task<TaktMaterialMovingPriceStatDto> GetMaterialMovingPriceStatAsync(TaktMaterialMovingPriceStatQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        EnsureThreeLayerContext();
        var period = (queryDto.ValuationPeriod ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(period))
        {
            period = DateTime.Today.AddMonths(-1).ToString("yyyy-MM");
        }
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;
        Expression<Func<TaktMaterialMovingPrice, bool>> periodPredicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.ValuationPeriod == period;
        var monthRowCount = await _materialMovingPriceRepository.CountAsync(periodPredicate);
        var monthStockAmount = await _materialMovingPriceRepository.SumAsync(x => x.StockAmount, periodPredicate);
        // 看板常用三类：Z792 成品 / Z790 半成品 / Z300 原材料（字典 logistics_materials_valuation_class）
        var valuationCodes = new[] { "Z792", "Z790", "Z300" };
        var byValuation = new List<TaktMaterialMovingPriceValuationAmountDto>(valuationCodes.Length);
        decimal classifiedSum = 0;
        foreach (var code in valuationCodes)
        {
            var codeLocal = code;
            Expression<Func<TaktMaterialMovingPrice, bool>> codePredicate = x =>
                x.TenantCode == tenantCode
                && x.CompanyCode == companyCode
                && x.ValuationPeriod == period
                && x.Valuation == codeLocal;
            var amount = await _materialMovingPriceRepository.SumAsync(x => x.StockAmount, codePredicate);
            classifiedSum += amount;
            byValuation.Add(new TaktMaterialMovingPriceValuationAmountDto
            {
                Valuation = codeLocal,
                StockAmount = amount,
            });
        }
        var otherAmount = monthStockAmount - classifiedSum;
        if (otherAmount != 0)
        {
            byValuation.Add(new TaktMaterialMovingPriceValuationAmountDto
            {
                Valuation = "OTHER",
                StockAmount = otherAmount,
            });
        }
        return new TaktMaterialMovingPriceStatDto
        {
            StatMonth = period,
            MonthStockAmount = monthStockAmount,
            MonthRowCount = monthRowCount,
            ByValuation = byValuation,
        };
    }
}
