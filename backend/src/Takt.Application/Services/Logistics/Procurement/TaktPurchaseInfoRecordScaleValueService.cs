// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：TaktPurchaseInfoRecordScaleValueService.cs
// 创建时间：2026-09-21
// 创建人：Takt365(Cursor AI)
// 功能描述：采购阶梯价格应用服务实现
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
/// 采购阶梯价格应用服务
/// </summary>
public class TaktPurchaseInfoRecordScaleValueService : TaktServiceBase, ITaktPurchaseInfoRecordScaleValueService
{
    private readonly ITaktCompanyRepository<TaktPurchaseInfoRecordScaleValue> _purchaseInfoRecordScaleValueRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseInfoRecordScaleValueRepository">采购阶梯价格仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPurchaseInfoRecordScaleValueService(
        ITaktCompanyRepository<TaktPurchaseInfoRecordScaleValue> purchaseInfoRecordScaleValueRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _purchaseInfoRecordScaleValueRepository = purchaseInfoRecordScaleValueRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取采购阶梯价格列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPurchaseInfoRecordScaleValueDto>> GetPurchaseInfoRecordScaleValueListAsync(TaktPurchaseInfoRecordScaleValueQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktPurchaseInfoRecordScaleValueDto>.Create(
                new List<TaktPurchaseInfoRecordScaleValueDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _purchaseInfoRecordScaleValueRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPurchaseInfoRecordScaleValueDto>.Create(
            data.Adapt<List<TaktPurchaseInfoRecordScaleValueDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取采购阶梯价格
    /// </summary>
    /// <param name="id">采购阶梯价格ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseInfoRecordScaleValueDto?> GetPurchaseInfoRecordScaleValueByIdAsync(long id)
    {
        var entity = await _purchaseInfoRecordScaleValueRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktPurchaseInfoRecordScaleValueDto>();
    }

    /// <summary>
    /// 获取采购阶梯价格选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPurchaseInfoRecordScaleValueOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _purchaseInfoRecordScaleValueRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IsObsolete == 0,
            x => x.PurchaseInfoRecordCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.PurchaseInfoRecordCode,
            DictLabel = e.PurchaseInfoRecordCode,
        }).ToList();
    }

    /// <summary>
    /// 创建采购阶梯价格
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseInfoRecordScaleValueDto> CreatePurchaseInfoRecordScaleValueAsync(TaktPurchaseInfoRecordScaleValueCreateDto dto)
    {
        var entity = dto.Adapt<TaktPurchaseInfoRecordScaleValue>();
        entity.IsObsolete = 0;
        var isUnique_ix_takt_logistics_procurement_purchase_info_record_scale_value_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseInfoRecordScaleValueRepository,
            x => x.PurchaseInfoRecordItemId == entity.PurchaseInfoRecordItemId
                && x.PurchaseInfoRecordCode == entity.PurchaseInfoRecordCode
                && x.PurchaseInfoRecordSeq == entity.PurchaseInfoRecordSeq
                && x.PurchaseScaleSeq == entity.PurchaseScaleSeq
                && x.ScaleValue == entity.ScaleValue);
        if (!isUnique_ix_takt_logistics_procurement_purchase_info_record_scale_value_unique)
        {
            throw new TaktBusinessException("采购阶梯价格的PurchaseInfoRecordItemId、PurchaseInfoRecordCode、PurchaseInfoRecordSeq、PurchaseScaleSeq、ScaleValue已存在");
        }
        entity = await _purchaseInfoRecordScaleValueRepository.CreateAsync(entity);
        return await GetPurchaseInfoRecordScaleValueByIdAsync(entity.Id) ?? entity.Adapt<TaktPurchaseInfoRecordScaleValueDto>();
    }

    /// <summary>
    /// 更新采购阶梯价格
    /// </summary>
    /// <param name="id">采购阶梯价格ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseInfoRecordScaleValueDto> UpdatePurchaseInfoRecordScaleValueAsync(long id, TaktPurchaseInfoRecordScaleValueUpdateDto dto)
    {
        var entity = await _purchaseInfoRecordScaleValueRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购阶梯价格不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_procurement_purchase_info_record_scale_value_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseInfoRecordScaleValueRepository,
            x => x.PurchaseInfoRecordItemId == entity.PurchaseInfoRecordItemId
                && x.PurchaseInfoRecordCode == entity.PurchaseInfoRecordCode
                && x.PurchaseInfoRecordSeq == entity.PurchaseInfoRecordSeq
                && x.PurchaseScaleSeq == entity.PurchaseScaleSeq
                && x.ScaleValue == entity.ScaleValue,
            id);
        if (!isUnique_ix_takt_logistics_procurement_purchase_info_record_scale_value_unique)
        {
            throw new TaktBusinessException("采购阶梯价格的PurchaseInfoRecordItemId、PurchaseInfoRecordCode、PurchaseInfoRecordSeq、PurchaseScaleSeq、ScaleValue已存在");
        }
        await _purchaseInfoRecordScaleValueRepository.UpdateAsync(entity);
        return await GetPurchaseInfoRecordScaleValueByIdAsync(id) ?? throw new TaktBusinessException("采购阶梯价格不存在");
    }

    /// <summary>
    /// 删除采购阶梯价格
    /// </summary>
    /// <param name="id">采购阶梯价格ID</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseInfoRecordScaleValueByIdAsync(long id)
    {
        var entity = await _purchaseInfoRecordScaleValueRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购阶梯价格不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("采购阶梯价格不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("采购阶梯价格已作废");
        }
        entity.IsObsolete = 1;
        await _purchaseInfoRecordScaleValueRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除采购阶梯价格
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseInfoRecordScaleValueBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePurchaseInfoRecordScaleValueByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新采购阶梯价格作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseInfoRecordScaleValueDto> UpdatePurchaseInfoRecordScaleValueObsoleteAsync(TaktPurchaseInfoRecordScaleValueObsoleteDto dto)
    {
        var entity = await _purchaseInfoRecordScaleValueRepository.GetByIdAsync(dto.PurchaseInfoRecordScaleValueId);
        if (entity == null)
        {
            throw new TaktBusinessException("采购阶梯价格不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("采购阶梯价格不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _purchaseInfoRecordScaleValueRepository.UpdateAsync(entity);
        return await GetPurchaseInfoRecordScaleValueByIdAsync(dto.PurchaseInfoRecordScaleValueId) ?? throw new TaktBusinessException("采购阶梯价格不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPurchaseInfoRecordScaleValueTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPurchaseInfoRecordScaleValueTemplateDto>(
            sheetName ?? "采购阶梯价格导入模板",
            fileName ?? "采购阶梯价格导入模板.xlsx");
    }

    /// <summary>
    /// 导入采购阶梯价格
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPurchaseInfoRecordScaleValueAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPurchaseInfoRecordScaleValueImportDto>(fileStream, sheetName ?? "采购阶梯价格导入模板");
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
                var entity = rows[i].Adapt<TaktPurchaseInfoRecordScaleValue>();
                var importKey = $"{entity.PurchaseInfoRecordItemId}|{entity.PurchaseInfoRecordCode}|{entity.PurchaseInfoRecordSeq}|{entity.PurchaseScaleSeq}|{entity.ScaleValue}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PurchaseInfoRecordItemId、PurchaseInfoRecordCode、PurchaseInfoRecordSeq、PurchaseScaleSeq、ScaleValue）");
                }
                var isUnique_ix_takt_logistics_procurement_purchase_info_record_scale_value_unique = await _uniqueValidator.IsUniqueAsync(
                    _purchaseInfoRecordScaleValueRepository,
                    x => x.PurchaseInfoRecordItemId == entity.PurchaseInfoRecordItemId
                        && x.PurchaseInfoRecordCode == entity.PurchaseInfoRecordCode
                        && x.PurchaseInfoRecordSeq == entity.PurchaseInfoRecordSeq
                        && x.PurchaseScaleSeq == entity.PurchaseScaleSeq
                        && x.ScaleValue == entity.ScaleValue);
                if (!isUnique_ix_takt_logistics_procurement_purchase_info_record_scale_value_unique)
                {
                    throw new TaktBusinessException("采购阶梯价格的PurchaseInfoRecordItemId、PurchaseInfoRecordCode、PurchaseInfoRecordSeq、PurchaseScaleSeq、ScaleValue已存在");
                }
                await _purchaseInfoRecordScaleValueRepository.CreateAsync(entity);
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
    /// 导出采购阶梯价格
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPurchaseInfoRecordScaleValueAsync(TaktPurchaseInfoRecordScaleValueQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktPurchaseInfoRecordScaleValueQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchaseInfoRecordScaleValueExportDto>(),
                sheetName ?? "采购阶梯价格数据",
                fileName ?? "采购阶梯价格导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _purchaseInfoRecordScaleValueRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchaseInfoRecordScaleValueExportDto>(),
                sheetName ?? "采购阶梯价格数据",
                fileName ?? "采购阶梯价格导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPurchaseInfoRecordScaleValueExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "采购阶梯价格数据",
            fileName ?? "采购阶梯价格导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建采购阶梯价格查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPurchaseInfoRecordScaleValue, bool>> QueryExpression(TaktPurchaseInfoRecordScaleValueQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPurchaseInfoRecordScaleValue>();

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
                || (x.PurchaseInfoRecordCode != null && x.PurchaseInfoRecordCode.Contains(keywords))
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

        if (queryDto?.PurchaseInfoRecordItemId.HasValue == true)
        {
            var purchaseInfoRecordItemId = queryDto.PurchaseInfoRecordItemId.Value;
            exp = exp.And(x => x.PurchaseInfoRecordItemId == purchaseInfoRecordItemId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchaseInfoRecordCode))
        {
            var purchaseInfoRecordCode = queryDto.PurchaseInfoRecordCode;
            exp = exp.And(x => x.PurchaseInfoRecordCode != null && x.PurchaseInfoRecordCode.Contains(purchaseInfoRecordCode));
        }

        if (queryDto?.PurchaseInfoRecordSeq.HasValue == true)
        {
            var purchaseInfoRecordSeq = queryDto.PurchaseInfoRecordSeq.Value;
            exp = exp.And(x => x.PurchaseInfoRecordSeq == purchaseInfoRecordSeq);
        }

        if (queryDto?.PurchaseScaleSeq.HasValue == true)
        {
            var purchaseScaleSeq = queryDto.PurchaseScaleSeq.Value;
            exp = exp.And(x => x.PurchaseScaleSeq == purchaseScaleSeq);
        }

        if (queryDto?.ScaleValue.HasValue == true)
        {
            var scaleValue = queryDto.ScaleValue.Value;
            exp = exp.And(x => x.ScaleValue == scaleValue);
        }

        if (queryDto?.Price.HasValue == true)
        {
            var price = queryDto.Price.Value;
            exp = exp.And(x => x.Price == price);
        }

        if (queryDto?.UntaxedPrice.HasValue == true)
        {
            var untaxedPrice = queryDto.UntaxedPrice.Value;
            exp = exp.And(x => x.UntaxedPrice == untaxedPrice);
        }

        if (queryDto?.TaxIncludedPrice.HasValue == true)
        {
            var taxIncludedPrice = queryDto.TaxIncludedPrice.Value;
            exp = exp.And(x => x.TaxIncludedPrice == taxIncludedPrice);
        }

        if (queryDto?.TaxAmount.HasValue == true)
        {
            var taxAmount = queryDto.TaxAmount.Value;
            exp = exp.And(x => x.TaxAmount == taxAmount);
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
    private static bool HasAnyListQueryFilter(TaktPurchaseInfoRecordScaleValueQueryDto? queryDto)
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
        if (queryDto.PurchaseInfoRecordItemId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseInfoRecordCode))
        {
            return true;
        }
        if (queryDto.PurchaseInfoRecordSeq.HasValue)
        {
            return true;
        }
        if (queryDto.PurchaseScaleSeq.HasValue)
        {
            return true;
        }
        if (queryDto.ScaleValue.HasValue)
        {
            return true;
        }
        if (queryDto.Price.HasValue)
        {
            return true;
        }
        if (queryDto.UntaxedPrice.HasValue)
        {
            return true;
        }
        if (queryDto.TaxIncludedPrice.HasValue)
        {
            return true;
        }
        if (queryDto.TaxAmount.HasValue)
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
