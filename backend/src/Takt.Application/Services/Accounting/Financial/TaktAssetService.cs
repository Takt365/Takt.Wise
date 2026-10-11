// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Accounting.Financial
// 文件名称：TaktAssetService.cs
// 创建时间：2026-08-30
// 创建人：Takt365(Cursor AI)
// 功能描述：资产应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Accounting.Financial;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Accounting.Financial;

/// <summary>
/// 资产应用服务
/// </summary>
public class TaktAssetService : TaktServiceBase, ITaktAssetService
{
    private readonly ITaktCompanyRepository<TaktAsset> _assetRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="assetRepository">资产仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktAssetService(
        ITaktCompanyRepository<TaktAsset> assetRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _assetRepository = assetRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取资产列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktAssetDto>> GetAssetListAsync(TaktAssetQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktAssetDto>.Create(
                new List<TaktAssetDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _assetRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktAssetDto>.Create(
            data.Adapt<List<TaktAssetDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取资产
    /// </summary>
    /// <param name="id">资产ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktAssetDto?> GetAssetByIdAsync(long id)
    {
        var entity = await _assetRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktAssetDto>();
    }

    /// <summary>
    /// 获取资产选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetAssetOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var normalizedPlantCode = plantCode?.Trim();
        var normalizedKeyword = keyword?.Trim();
        var predicate = Expressionable.Create<TaktAsset>()
            .And(x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.AssetStatus == 1)
            .AndIF(!string.IsNullOrEmpty(normalizedKeyword), x =>
                (x.AssetCode != null && x.AssetCode.Contains(normalizedKeyword!))
                || (x.AssetName != null && x.AssetName.Contains(normalizedKeyword!)))
            .ToExpression();
        var list = await _assetRepository.GetListAsync(
            predicate,
            x => x.AssetName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.AssetCode,
            DictLabel = e.AssetName ?? e.AssetCode,
        }).ToList();
    }

    /// <summary>
    /// 创建资产
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAssetDto> CreateAssetAsync(TaktAssetCreateDto dto)
    {
        var entity = dto.Adapt<TaktAsset>();
        var isUnique_ix_asset_code_unique = await _uniqueValidator.IsUniqueAsync(
            _assetRepository,
            x => x.AssetCode == entity.AssetCode);
        if (!isUnique_ix_asset_code_unique)
        {
            throw new TaktBusinessException("资产的AssetCode已存在");
        }
        entity = await _assetRepository.CreateAsync(entity);
        return await GetAssetByIdAsync(entity.Id) ?? entity.Adapt<TaktAssetDto>();
    }

    /// <summary>
    /// 更新资产
    /// </summary>
    /// <param name="id">资产ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAssetDto> UpdateAssetAsync(long id, TaktAssetUpdateDto dto)
    {
        var entity = await _assetRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("资产不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_asset_code_unique = await _uniqueValidator.IsUniqueAsync(
            _assetRepository,
            x => x.AssetCode == entity.AssetCode,
            id);
        if (!isUnique_ix_asset_code_unique)
        {
            throw new TaktBusinessException("资产的AssetCode已存在");
        }
        await _assetRepository.UpdateAsync(entity);
        return await GetAssetByIdAsync(id) ?? throw new TaktBusinessException("资产不存在");
    }

    /// <summary>
    /// 删除资产
    /// </summary>
    /// <param name="id">资产ID</param>
    /// <returns>任务</returns>
    public async Task DeleteAssetByIdAsync(long id)
    {
        var deleted = await _assetRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("资产不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除资产
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteAssetBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteAssetByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新资产状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAssetDto> UpdateAssetStatusAsync(TaktAssetStatusDto dto)
    {
        var entity = await _assetRepository.GetByIdAsync(dto.AssetId);
        if (entity == null)
        {
            throw new TaktBusinessException("资产不存在");
        }
        entity.AssetStatus = dto.AssetStatus;
        await _assetRepository.UpdateAsync(entity);
        return await GetAssetByIdAsync(dto.AssetId) ?? throw new TaktBusinessException("资产不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetAssetTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktAssetTemplateDto>(
            sheetName ?? "资产导入模板",
            fileName ?? "资产导入模板.xlsx");
    }

    /// <summary>
    /// 导入资产
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportAssetAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktAssetImportDto>(fileStream, sheetName ?? "资产导入模板");
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
                var entity = rows[i].Adapt<TaktAsset>();
                var importKey = $"{entity.AssetCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（AssetCode）");
                }
                var isUnique_ix_asset_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _assetRepository,
                    x => x.AssetCode == entity.AssetCode);
                if (!isUnique_ix_asset_code_unique)
                {
                    throw new TaktBusinessException("资产的AssetCode已存在");
                }
                await _assetRepository.CreateAsync(entity);
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
    /// 导出资产
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportAssetAsync(TaktAssetQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktAssetQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktAssetExportDto>(),
                sheetName ?? "资产数据",
                fileName ?? "资产导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _assetRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktAssetExportDto>(),
                sheetName ?? "资产数据",
                fileName ?? "资产导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktAssetExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "资产数据",
            fileName ?? "资产导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建资产查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktAsset, bool>> QueryExpression(TaktAssetQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktAsset>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.AssetCode != null && x.AssetCode.Contains(keywords))
                || (x.AssetName != null && x.AssetName.Contains(keywords))
                || (x.AssetCategory != null && x.AssetCategory.Contains(keywords))
                || (x.AssetType != null && x.AssetType.Contains(keywords))
                || (x.CostCenterName != null && x.CostCenterName.Contains(keywords))
                || (x.DeptName != null && x.DeptName.Contains(keywords))
                || (x.UserName != null && x.UserName.Contains(keywords))
                || (x.AssetLocation != null && x.AssetLocation.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.AssetCode))
        {
            var assetCode = queryDto.AssetCode;
            exp = exp.And(x => x.AssetCode != null && x.AssetCode.Contains(assetCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AssetName))
        {
            var assetName = queryDto.AssetName;
            exp = exp.And(x => x.AssetName != null && x.AssetName.Contains(assetName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AssetCategory))
        {
            var assetCategory = queryDto.AssetCategory;
            exp = exp.And(x => x.AssetCategory != null && x.AssetCategory.Contains(assetCategory));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AssetType))
        {
            var assetType = queryDto.AssetType;
            exp = exp.And(x => x.AssetType != null && x.AssetType.Contains(assetType));
        }

        if (queryDto?.AssetOriginalValue.HasValue == true)
        {
            var assetOriginalValue = queryDto.AssetOriginalValue.Value;
            exp = exp.And(x => x.AssetOriginalValue == assetOriginalValue);
        }

        if (queryDto?.AssetNetValue.HasValue == true)
        {
            var assetNetValue = queryDto.AssetNetValue.Value;
            exp = exp.And(x => x.AssetNetValue == assetNetValue);
        }

        if (queryDto?.AccumulatedDepreciation.HasValue == true)
        {
            var accumulatedDepreciation = queryDto.AccumulatedDepreciation.Value;
            exp = exp.And(x => x.AccumulatedDepreciation == accumulatedDepreciation);
        }

        if (queryDto?.CostCenterId.HasValue == true)
        {
            var costCenterId = queryDto.CostCenterId.Value;
            exp = exp.And(x => x.CostCenterId == costCenterId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CostCenterName))
        {
            var costCenterName = queryDto.CostCenterName;
            exp = exp.And(x => x.CostCenterName != null && x.CostCenterName.Contains(costCenterName));
        }

        if (queryDto?.DeptId.HasValue == true)
        {
            var deptId = queryDto.DeptId.Value;
            exp = exp.And(x => x.DeptId == deptId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DeptName))
        {
            var deptName = queryDto.DeptName;
            exp = exp.And(x => x.DeptName != null && x.DeptName.Contains(deptName));
        }

        if (queryDto?.UserId.HasValue == true)
        {
            var userId = queryDto.UserId.Value;
            exp = exp.And(x => x.UserId == userId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.UserName))
        {
            var userName = queryDto.UserName;
            exp = exp.And(x => x.UserName != null && x.UserName.Contains(userName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AssetLocation))
        {
            var assetLocation = queryDto.AssetLocation;
            exp = exp.And(x => x.AssetLocation != null && x.AssetLocation.Contains(assetLocation));
        }

        if (queryDto?.ExpectedLifeMonths.HasValue == true)
        {
            var expectedLifeMonths = queryDto.ExpectedLifeMonths.Value;
            exp = exp.And(x => x.ExpectedLifeMonths == expectedLifeMonths);
        }

        if (queryDto?.DepreciationMethod.HasValue == true)
        {
            var depreciationMethod = queryDto.DepreciationMethod.Value;
            exp = exp.And(x => x.DepreciationMethod == depreciationMethod);
        }

        if (queryDto?.MonthlyDepreciation.HasValue == true)
        {
            var monthlyDepreciation = queryDto.MonthlyDepreciation.Value;
            exp = exp.And(x => x.MonthlyDepreciation == monthlyDepreciation);
        }

        if (queryDto?.AssetStatus.HasValue == true)
        {
            var assetStatus = queryDto.AssetStatus.Value;
            exp = exp.And(x => x.AssetStatus == assetStatus);
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

        if (queryDto?.PurchaseDateStart.HasValue == true)
        {
            var purchaseDateStart = queryDto.PurchaseDateStart.Value;
            exp = exp.And(x => x.PurchaseDate >= purchaseDateStart);
        }

        if (queryDto?.PurchaseDateEnd.HasValue == true)
        {
            var purchaseDateEnd = queryDto.PurchaseDateEnd.Value;
            exp = exp.And(x => x.PurchaseDate <= purchaseDateEnd);
        }

        if (queryDto?.StartDateStart.HasValue == true)
        {
            var startDateStart = queryDto.StartDateStart.Value;
            exp = exp.And(x => x.StartDate >= startDateStart);
        }

        if (queryDto?.StartDateEnd.HasValue == true)
        {
            var startDateEnd = queryDto.StartDateEnd.Value;
            exp = exp.And(x => x.StartDate <= startDateEnd);
        }

        if (queryDto?.ScrapDateStart.HasValue == true)
        {
            var scrapDateStart = queryDto.ScrapDateStart.Value;
            exp = exp.And(x => x.ScrapDate >= scrapDateStart);
        }

        if (queryDto?.ScrapDateEnd.HasValue == true)
        {
            var scrapDateEnd = queryDto.ScrapDateEnd.Value;
            exp = exp.And(x => x.ScrapDate <= scrapDateEnd);
        }

        if (queryDto?.DisposalDateStart.HasValue == true)
        {
            var disposalDateStart = queryDto.DisposalDateStart.Value;
            exp = exp.And(x => x.DisposalDate >= disposalDateStart);
        }

        if (queryDto?.DisposalDateEnd.HasValue == true)
        {
            var disposalDateEnd = queryDto.DisposalDateEnd.Value;
            exp = exp.And(x => x.DisposalDate <= disposalDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktAssetQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.AssetCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AssetName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AssetCategory))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AssetType))
        {
            return true;
        }
        if (queryDto.AssetOriginalValue.HasValue)
        {
            return true;
        }
        if (queryDto.AssetNetValue.HasValue)
        {
            return true;
        }
        if (queryDto.AccumulatedDepreciation.HasValue)
        {
            return true;
        }
        if (queryDto.CostCenterId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CostCenterName))
        {
            return true;
        }
        if (queryDto.DeptId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DeptName))
        {
            return true;
        }
        if (queryDto.UserId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.UserName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AssetLocation))
        {
            return true;
        }
        if (queryDto.ExpectedLifeMonths.HasValue)
        {
            return true;
        }
        if (queryDto.DepreciationMethod.HasValue)
        {
            return true;
        }
        if (queryDto.MonthlyDepreciation.HasValue)
        {
            return true;
        }
        if (queryDto.AssetStatus.HasValue)
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
        if (queryDto.PurchaseDateStart.HasValue || queryDto.PurchaseDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.StartDateStart.HasValue || queryDto.StartDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.ScrapDateStart.HasValue || queryDto.ScrapDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.DisposalDateStart.HasValue || queryDto.DisposalDateEnd.HasValue)
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
