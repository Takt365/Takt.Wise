// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：TaktSalesConditionScaleValueService.cs
// 创建时间：2026-09-21
// 创建人：Takt365(Cursor AI)
// 功能描述：销售阶梯价格应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Sales;
using Takt.Domain.Entities.Logistics.Sales;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Sales;

/// <summary>
/// 销售阶梯价格应用服务
/// </summary>
public class TaktSalesConditionScaleValueService : TaktServiceBase, ITaktSalesConditionScaleValueService
{
    private readonly ITaktCompanyRepository<TaktSalesConditionScaleValue> _salesConditionScaleValueRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesConditionScaleValueRepository">销售阶梯价格仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSalesConditionScaleValueService(
        ITaktCompanyRepository<TaktSalesConditionScaleValue> salesConditionScaleValueRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _salesConditionScaleValueRepository = salesConditionScaleValueRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取销售阶梯价格列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSalesConditionScaleValueDto>> GetSalesConditionScaleValueListAsync(TaktSalesConditionScaleValueQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktSalesConditionScaleValueDto>.Create(
                new List<TaktSalesConditionScaleValueDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _salesConditionScaleValueRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSalesConditionScaleValueDto>.Create(
            data.Adapt<List<TaktSalesConditionScaleValueDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取销售阶梯价格
    /// </summary>
    /// <param name="id">销售阶梯价格ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesConditionScaleValueDto?> GetSalesConditionScaleValueByIdAsync(long id)
    {
        var entity = await _salesConditionScaleValueRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktSalesConditionScaleValueDto>();
    }

    /// <summary>
    /// 获取销售阶梯价格选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSalesConditionScaleValueOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _salesConditionScaleValueRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IsObsolete == 0,
            x => x.SalesConditionCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.SalesConditionCode,
            DictLabel = e.SalesConditionCode,
        }).ToList();
    }

    /// <summary>
    /// 创建销售阶梯价格
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesConditionScaleValueDto> CreateSalesConditionScaleValueAsync(TaktSalesConditionScaleValueCreateDto dto)
    {
        var entity = dto.Adapt<TaktSalesConditionScaleValue>();
        entity.IsObsolete = 0;
        var isUnique_ix_takt_logistics_sales_condition_scale_value_unique = await _uniqueValidator.IsUniqueAsync(
            _salesConditionScaleValueRepository,
            x => x.SalesConditionItemId == entity.SalesConditionItemId
                && x.SalesConditionCode == entity.SalesConditionCode
                && x.SalesConditionSeq == entity.SalesConditionSeq
                && x.SalesScaleSeq == entity.SalesScaleSeq
                && x.ScaleValue == entity.ScaleValue);
        if (!isUnique_ix_takt_logistics_sales_condition_scale_value_unique)
        {
            throw new TaktBusinessException("销售阶梯价格的SalesConditionItemId、SalesConditionCode、SalesConditionSeq、SalesScaleSeq、ScaleValue已存在");
        }
        entity = await _salesConditionScaleValueRepository.CreateAsync(entity);
        return await GetSalesConditionScaleValueByIdAsync(entity.Id) ?? entity.Adapt<TaktSalesConditionScaleValueDto>();
    }

    /// <summary>
    /// 更新销售阶梯价格
    /// </summary>
    /// <param name="id">销售阶梯价格ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesConditionScaleValueDto> UpdateSalesConditionScaleValueAsync(long id, TaktSalesConditionScaleValueUpdateDto dto)
    {
        var entity = await _salesConditionScaleValueRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售阶梯价格不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_sales_condition_scale_value_unique = await _uniqueValidator.IsUniqueAsync(
            _salesConditionScaleValueRepository,
            x => x.SalesConditionItemId == entity.SalesConditionItemId
                && x.SalesConditionCode == entity.SalesConditionCode
                && x.SalesConditionSeq == entity.SalesConditionSeq
                && x.SalesScaleSeq == entity.SalesScaleSeq
                && x.ScaleValue == entity.ScaleValue,
            id);
        if (!isUnique_ix_takt_logistics_sales_condition_scale_value_unique)
        {
            throw new TaktBusinessException("销售阶梯价格的SalesConditionItemId、SalesConditionCode、SalesConditionSeq、SalesScaleSeq、ScaleValue已存在");
        }
        await _salesConditionScaleValueRepository.UpdateAsync(entity);
        return await GetSalesConditionScaleValueByIdAsync(id) ?? throw new TaktBusinessException("销售阶梯价格不存在");
    }

    /// <summary>
    /// 删除销售阶梯价格
    /// </summary>
    /// <param name="id">销售阶梯价格ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesConditionScaleValueByIdAsync(long id)
    {
        var entity = await _salesConditionScaleValueRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售阶梯价格不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("销售阶梯价格不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("销售阶梯价格已作废");
        }
        entity.IsObsolete = 1;
        await _salesConditionScaleValueRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除销售阶梯价格
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesConditionScaleValueBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSalesConditionScaleValueByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新销售阶梯价格作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesConditionScaleValueDto> UpdateSalesConditionScaleValueObsoleteAsync(TaktSalesConditionScaleValueObsoleteDto dto)
    {
        var entity = await _salesConditionScaleValueRepository.GetByIdAsync(dto.SalesConditionScaleValueId);
        if (entity == null)
        {
            throw new TaktBusinessException("销售阶梯价格不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("销售阶梯价格不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _salesConditionScaleValueRepository.UpdateAsync(entity);
        return await GetSalesConditionScaleValueByIdAsync(dto.SalesConditionScaleValueId) ?? throw new TaktBusinessException("销售阶梯价格不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSalesConditionScaleValueTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSalesConditionScaleValueTemplateDto>(
            sheetName ?? "销售阶梯价格导入模板",
            fileName ?? "销售阶梯价格导入模板.xlsx");
    }

    /// <summary>
    /// 导入销售阶梯价格
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSalesConditionScaleValueAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSalesConditionScaleValueImportDto>(fileStream, sheetName ?? "销售阶梯价格导入模板");
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
                var entity = rows[i].Adapt<TaktSalesConditionScaleValue>();
                var importKey = $"{entity.SalesConditionItemId}|{entity.SalesConditionCode}|{entity.SalesConditionSeq}|{entity.SalesScaleSeq}|{entity.ScaleValue}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（SalesConditionItemId、SalesConditionCode、SalesConditionSeq、SalesScaleSeq、ScaleValue）");
                }
                var isUnique_ix_takt_logistics_sales_condition_scale_value_unique = await _uniqueValidator.IsUniqueAsync(
                    _salesConditionScaleValueRepository,
                    x => x.SalesConditionItemId == entity.SalesConditionItemId
                        && x.SalesConditionCode == entity.SalesConditionCode
                        && x.SalesConditionSeq == entity.SalesConditionSeq
                        && x.SalesScaleSeq == entity.SalesScaleSeq
                        && x.ScaleValue == entity.ScaleValue);
                if (!isUnique_ix_takt_logistics_sales_condition_scale_value_unique)
                {
                    throw new TaktBusinessException("销售阶梯价格的SalesConditionItemId、SalesConditionCode、SalesConditionSeq、SalesScaleSeq、ScaleValue已存在");
                }
                await _salesConditionScaleValueRepository.CreateAsync(entity);
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
    /// 导出销售阶梯价格
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSalesConditionScaleValueAsync(TaktSalesConditionScaleValueQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktSalesConditionScaleValueQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalesConditionScaleValueExportDto>(),
                sheetName ?? "销售阶梯价格数据",
                fileName ?? "销售阶梯价格导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _salesConditionScaleValueRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalesConditionScaleValueExportDto>(),
                sheetName ?? "销售阶梯价格数据",
                fileName ?? "销售阶梯价格导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSalesConditionScaleValueExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "销售阶梯价格数据",
            fileName ?? "销售阶梯价格导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建销售阶梯价格查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSalesConditionScaleValue, bool>> QueryExpression(TaktSalesConditionScaleValueQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSalesConditionScaleValue>();

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
                || (x.SalesConditionCode != null && x.SalesConditionCode.Contains(keywords))
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

        if (queryDto?.SalesConditionItemId.HasValue == true)
        {
            var salesConditionItemId = queryDto.SalesConditionItemId.Value;
            exp = exp.And(x => x.SalesConditionItemId == salesConditionItemId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SalesConditionCode))
        {
            var salesConditionCode = queryDto.SalesConditionCode;
            exp = exp.And(x => x.SalesConditionCode != null && x.SalesConditionCode.Contains(salesConditionCode));
        }

        if (queryDto?.SalesConditionSeq.HasValue == true)
        {
            var salesConditionSeq = queryDto.SalesConditionSeq.Value;
            exp = exp.And(x => x.SalesConditionSeq == salesConditionSeq);
        }

        if (queryDto?.SalesScaleSeq.HasValue == true)
        {
            var salesScaleSeq = queryDto.SalesScaleSeq.Value;
            exp = exp.And(x => x.SalesScaleSeq == salesScaleSeq);
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
    private static bool HasAnyListQueryFilter(TaktSalesConditionScaleValueQueryDto? queryDto)
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
        if (queryDto.SalesConditionItemId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SalesConditionCode))
        {
            return true;
        }
        if (queryDto.SalesConditionSeq.HasValue)
        {
            return true;
        }
        if (queryDto.SalesScaleSeq.HasValue)
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
