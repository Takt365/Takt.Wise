// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：TaktSourceOfSupplyService.cs
// 创建时间：2026-07-21
// 创建人：Takt365(Cursor AI)
// 功能描述：货源清单清单应用服务实现
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
/// 货源清单清单应用服务
/// </summary>
public class TaktSourceOfSupplyService : TaktServiceBase, ITaktSourceOfSupplyService
{
    private readonly ITaktCompanyRepository<TaktSourceOfSupply> _sourceOfSupplyRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sourceOfSupplyRepository">货源清单清单仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSourceOfSupplyService(
        ITaktCompanyRepository<TaktSourceOfSupply> sourceOfSupplyRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _sourceOfSupplyRepository = sourceOfSupplyRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取货源清单清单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSourceOfSupplyDto>> GetSourceOfSupplyListAsync(TaktSourceOfSupplyQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _sourceOfSupplyRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSourceOfSupplyDto>.Create(
            data.Adapt<List<TaktSourceOfSupplyDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取货源清单清单
    /// </summary>
    /// <param name="id">货源清单清单ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSourceOfSupplyDto?> GetSourceOfSupplyByIdAsync(long id)
    {
        var entity = await _sourceOfSupplyRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktSourceOfSupplyDto>();
    }

    /// <summary>
    /// 获取货源清单清单选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSourceOfSupplyOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _sourceOfSupplyRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.SourceStatus == 1,
            x => x.SourceOfSupplyCode,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.SourceOfSupplyCode,
        }).ToList();
    }

    /// <summary>
    /// 创建货源清单清单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSourceOfSupplyDto> CreateSourceOfSupplyAsync(TaktSourceOfSupplyCreateDto dto)
    {
        var entity = dto.Adapt<TaktSourceOfSupply>();
        var isUnique_ix_takt_logistics_procurement_source_of_supply_unique = await _uniqueValidator.IsUniqueAsync(
            _sourceOfSupplyRepository,
            x => x.PlantCode == entity.PlantCode
                && x.MaterialCode == entity.MaterialCode
                && x.SupplierCode == entity.SupplierCode
                && x.ValidFrom == entity.ValidFrom);
        if (!isUnique_ix_takt_logistics_procurement_source_of_supply_unique)
        {
            throw new TaktBusinessException("货源清单清单的PlantCode、MaterialCode、SupplierCode、ValidFrom已存在");
        }
        var isUnique_ix_takt_logistics_procurement_source_of_supply_code_unique = await _uniqueValidator.IsUniqueAsync(
            _sourceOfSupplyRepository,
            x => x.SourceOfSupplyCode == entity.SourceOfSupplyCode);
        if (!isUnique_ix_takt_logistics_procurement_source_of_supply_code_unique)
        {
            throw new TaktBusinessException("货源清单清单的SourceOfSupplyCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _sourceOfSupplyRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _sourceOfSupplyRepository.CreateAsync(entity);
        return await GetSourceOfSupplyByIdAsync(entity.Id) ?? entity.Adapt<TaktSourceOfSupplyDto>();
    }

    /// <summary>
    /// 更新货源清单清单
    /// </summary>
    /// <param name="id">货源清单清单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSourceOfSupplyDto> UpdateSourceOfSupplyAsync(long id, TaktSourceOfSupplyUpdateDto dto)
    {
        var entity = await _sourceOfSupplyRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("货源清单清单不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_procurement_source_of_supply_unique = await _uniqueValidator.IsUniqueAsync(
            _sourceOfSupplyRepository,
            x => x.PlantCode == entity.PlantCode
                && x.MaterialCode == entity.MaterialCode
                && x.SupplierCode == entity.SupplierCode
                && x.ValidFrom == entity.ValidFrom,
            id);
        if (!isUnique_ix_takt_logistics_procurement_source_of_supply_unique)
        {
            throw new TaktBusinessException("货源清单清单的PlantCode、MaterialCode、SupplierCode、ValidFrom已存在");
        }
        var isUnique_ix_takt_logistics_procurement_source_of_supply_code_unique = await _uniqueValidator.IsUniqueAsync(
            _sourceOfSupplyRepository,
            x => x.SourceOfSupplyCode == entity.SourceOfSupplyCode,
            id);
        if (!isUnique_ix_takt_logistics_procurement_source_of_supply_code_unique)
        {
            throw new TaktBusinessException("货源清单清单的SourceOfSupplyCode已存在");
        }
        await _sourceOfSupplyRepository.UpdateAsync(entity);
        return await GetSourceOfSupplyByIdAsync(id) ?? throw new TaktBusinessException("货源清单清单不存在");
    }

    /// <summary>
    /// 删除货源清单清单
    /// </summary>
    /// <param name="id">货源清单清单ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSourceOfSupplyByIdAsync(long id)
    {
        var deleted = await _sourceOfSupplyRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("货源清单清单不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除货源清单清单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSourceOfSupplyBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSourceOfSupplyByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新货源清单清单状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSourceOfSupplyDto> UpdateSourceOfSupplyStatusAsync(TaktSourceOfSupplyStatusDto dto)
    {
        var entity = await _sourceOfSupplyRepository.GetByIdAsync(dto.SourceOfSupplyId);
        if (entity == null)
        {
            throw new TaktBusinessException("货源清单清单不存在");
        }
        entity.SourceStatus = dto.SourceStatus;
        await _sourceOfSupplyRepository.UpdateAsync(entity);
        return await GetSourceOfSupplyByIdAsync(dto.SourceOfSupplyId) ?? throw new TaktBusinessException("货源清单清单不存在");
    }

    /// <summary>
    /// 更新货源清单清单排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSourceOfSupplyDto> UpdateSourceOfSupplySortAsync(TaktSourceOfSupplySortDto dto)
    {
        var entity = await _sourceOfSupplyRepository.GetByIdAsync(dto.SourceOfSupplyId);
        if (entity == null)
        {
            throw new TaktBusinessException("货源清单清单不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _sourceOfSupplyRepository.UpdateAsync(entity);
        return await GetSourceOfSupplyByIdAsync(dto.SourceOfSupplyId) ?? throw new TaktBusinessException("货源清单清单不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSourceOfSupplyTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSourceOfSupplyTemplateDto>(
            sheetName ?? "货源清单清单导入模板",
            fileName ?? "货源清单清单导入模板.xlsx");
    }

    /// <summary>
    /// 导入货源清单清单
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSourceOfSupplyAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSourceOfSupplyImportDto>(fileStream, sheetName ?? "货源清单清单导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _sourceOfSupplyRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktSourceOfSupply>();
                var importKey = $"{entity.PlantCode}|{entity.MaterialCode}|{entity.SupplierCode}|{entity.ValidFrom}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、MaterialCode、SupplierCode、ValidFrom）");
                }
                var isUnique_ix_takt_logistics_procurement_source_of_supply_unique = await _uniqueValidator.IsUniqueAsync(
                    _sourceOfSupplyRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.MaterialCode == entity.MaterialCode
                        && x.SupplierCode == entity.SupplierCode
                        && x.ValidFrom == entity.ValidFrom);
                if (!isUnique_ix_takt_logistics_procurement_source_of_supply_unique)
                {
                    throw new TaktBusinessException("货源清单清单的PlantCode、MaterialCode、SupplierCode、ValidFrom已存在");
                }
                var isUnique_ix_takt_logistics_procurement_source_of_supply_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _sourceOfSupplyRepository,
                    x => x.SourceOfSupplyCode == entity.SourceOfSupplyCode);
                if (!isUnique_ix_takt_logistics_procurement_source_of_supply_code_unique)
                {
                    throw new TaktBusinessException("货源清单清单的SourceOfSupplyCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _sourceOfSupplyRepository.CreateAsync(entity);
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
    /// 导出货源清单清单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSourceOfSupplyAsync(TaktSourceOfSupplyQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktSourceOfSupplyQueryDto());
        var list = await _sourceOfSupplyRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSourceOfSupplyExportDto>(),
                sheetName ?? "货源清单清单数据",
                fileName ?? "货源清单清单导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSourceOfSupplyExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "货源清单清单数据",
            fileName ?? "货源清单清单导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建货源清单清单查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSourceOfSupply, bool>> QueryExpression(TaktSourceOfSupplyQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSourceOfSupply>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.SourceOfSupplyCode != null && x.SourceOfSupplyCode.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.SupplierCode != null && x.SupplierCode.Contains(keywords))
                || (x.PurchaseGroup != null && x.PurchaseGroup.Contains(keywords))
                || SqlFunc.ToString(x.IsFixed).Contains(keywords)
                || SqlFunc.ToString(x.IsBlocked).Contains(keywords)
                || (x.PurchaseUnit != null && x.PurchaseUnit.Contains(keywords))
                || SqlFunc.ToString(x.MinOrderQuantity).Contains(keywords)
                || SqlFunc.ToString(x.RoundingValue).Contains(keywords)
                || SqlFunc.ToString(x.PlannedDeliveryTimeDays).Contains(keywords)
                || (x.AgreementNumber != null && x.AgreementNumber.Contains(keywords))
                || SqlFunc.ToString(x.AgreementLineNumber).Contains(keywords)
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || SqlFunc.ToString(x.SourceStatus).Contains(keywords)
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.ValidFrom).Contains(keywords)
                || SqlFunc.ToString(x.ValidTo).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceOfSupplyCode))
        {
            exp = exp.And(x => x.SourceOfSupplyCode != null && x.SourceOfSupplyCode.Contains(queryDto.SourceOfSupplyCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialCode))
        {
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(queryDto.MaterialCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.SupplierCode))
        {
            exp = exp.And(x => x.SupplierCode != null && x.SupplierCode.Contains(queryDto.SupplierCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.PurchaseGroup))
        {
            exp = exp.And(x => x.PurchaseGroup != null && x.PurchaseGroup.Contains(queryDto.PurchaseGroup));
        }

        if (queryDto?.IsFixed.HasValue == true)
        {
            exp = exp.And(x => x.IsFixed == queryDto.IsFixed);
        }

        if (queryDto?.IsBlocked.HasValue == true)
        {
            exp = exp.And(x => x.IsBlocked == queryDto.IsBlocked);
        }

        if (!string.IsNullOrEmpty(queryDto?.PurchaseUnit))
        {
            exp = exp.And(x => x.PurchaseUnit != null && x.PurchaseUnit.Contains(queryDto.PurchaseUnit));
        }

        if (queryDto?.MinOrderQuantity.HasValue == true)
        {
            exp = exp.And(x => x.MinOrderQuantity == queryDto.MinOrderQuantity);
        }

        if (queryDto?.RoundingValue.HasValue == true)
        {
            exp = exp.And(x => x.RoundingValue == queryDto.RoundingValue);
        }

        if (queryDto?.PlannedDeliveryTimeDays.HasValue == true)
        {
            exp = exp.And(x => x.PlannedDeliveryTimeDays == queryDto.PlannedDeliveryTimeDays);
        }

        if (!string.IsNullOrEmpty(queryDto?.AgreementNumber))
        {
            exp = exp.And(x => x.AgreementNumber != null && x.AgreementNumber.Contains(queryDto.AgreementNumber));
        }

        if (queryDto?.AgreementLineNumber.HasValue == true)
        {
            exp = exp.And(x => x.AgreementLineNumber == queryDto.AgreementLineNumber);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (queryDto?.SourceStatus.HasValue == true)
        {
            exp = exp.And(x => x.SourceStatus == queryDto.SourceStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.CultureCode))
        {
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(queryDto.CultureCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.ValidFromStart.HasValue == true)
        {
            exp = exp.And(x => x.ValidFrom >= queryDto.ValidFromStart);
        }

        if (queryDto?.ValidFromEnd.HasValue == true)
        {
            exp = exp.And(x => x.ValidFrom <= queryDto.ValidFromEnd);
        }

        if (queryDto?.ValidToStart.HasValue == true)
        {
            exp = exp.And(x => x.ValidTo >= queryDto.ValidToStart);
        }

        if (queryDto?.ValidToEnd.HasValue == true)
        {
            exp = exp.And(x => x.ValidTo <= queryDto.ValidToEnd);
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt >= queryDto.CreatedAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt <= queryDto.CreatedAtEnd);
        }

        return exp.ToExpression();
    }
}
