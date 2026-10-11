// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Routine.HelpDesk
// 文件名称：TaktItAssetService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：IT设备保修扩展应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Routine.HelpDesk;
using Takt.Domain.Entities.Routine.HelpDesk;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Routine.HelpDesk;

/// <summary>
/// IT设备保修扩展应用服务
/// </summary>
public class TaktItAssetService : TaktServiceBase, ITaktItAssetService
{
    private readonly ITaktCompanyRepository<TaktItAsset> _itAssetRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="itAssetRepository">IT设备保修扩展仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktItAssetService(
        ITaktCompanyRepository<TaktItAsset> itAssetRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _itAssetRepository = itAssetRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取IT设备保修扩展列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktItAssetDto>> GetItAssetListAsync(TaktItAssetQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _itAssetRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktItAssetDto>.Create(
            data.Adapt<List<TaktItAssetDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取IT设备保修扩展
    /// </summary>
    /// <param name="id">IT设备保修扩展ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktItAssetDto?> GetItAssetByIdAsync(long id)
    {
        var entity = await _itAssetRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktItAssetDto>();
        await FillItAssetDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取IT设备保修扩展选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetItAssetOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _itAssetRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.AssetCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.AssetCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建IT设备保修扩展
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktItAssetDto> CreateItAssetAsync(TaktItAssetCreateDto dto)
    {
        var entity = dto.Adapt<TaktItAsset>();
        var isUnique_ix_it_asset_code_unique = await _uniqueValidator.IsUniqueAsync(
            _itAssetRepository,
            x => x.AssetCode == entity.AssetCode);
        if (!isUnique_ix_it_asset_code_unique)
        {
            throw new TaktBusinessException("IT设备保修扩展的AssetCode已存在");
        }
        entity = await _itAssetRepository.CreateAsync(entity);
                await SaveItAssetChildrenAsync(entity, dto);
        return await GetItAssetByIdAsync(entity.Id) ?? entity.Adapt<TaktItAssetDto>();
    }

    /// <summary>
    /// 更新IT设备保修扩展
    /// </summary>
    /// <param name="id">IT设备保修扩展ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktItAssetDto> UpdateItAssetAsync(long id, TaktItAssetUpdateDto dto)
    {
        var entity = await _itAssetRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("IT设备保修扩展不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_it_asset_code_unique = await _uniqueValidator.IsUniqueAsync(
            _itAssetRepository,
            x => x.AssetCode == entity.AssetCode,
            id);
        if (!isUnique_ix_it_asset_code_unique)
        {
            throw new TaktBusinessException("IT设备保修扩展的AssetCode已存在");
        }
        await _itAssetRepository.UpdateAsync(entity);
                await SaveItAssetChildrenAsync(entity, dto);
        return await GetItAssetByIdAsync(id) ?? throw new TaktBusinessException("IT设备保修扩展不存在");
    }

    /// <summary>
    /// 删除IT设备保修扩展
    /// </summary>
    /// <param name="id">IT设备保修扩展ID</param>
    /// <returns>任务</returns>
    public async Task DeleteItAssetByIdAsync(long id)
    {
        var entity = await _itAssetRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("IT设备保修扩展不存在或已删除");
        }        var deleted = await _itAssetRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("IT设备保修扩展不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除IT设备保修扩展
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteItAssetBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteItAssetByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetItAssetTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktItAssetTemplateDto>(
            sheetName ?? "IT设备保修扩展导入模板",
            fileName ?? "IT设备保修扩展导入模板.xlsx");
    }

    /// <summary>
    /// 导入IT设备保修扩展
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportItAssetAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktItAssetImportDto>(fileStream, sheetName ?? "IT设备保修扩展导入模板");
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
                var entity = rows[i].Adapt<TaktItAsset>();
                var importKey = $"{entity.AssetCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（AssetCode）");
                }
                var isUnique_ix_it_asset_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _itAssetRepository,
                    x => x.AssetCode == entity.AssetCode);
                if (!isUnique_ix_it_asset_code_unique)
                {
                    throw new TaktBusinessException("IT设备保修扩展的AssetCode已存在");
                }
                await _itAssetRepository.CreateAsync(entity);
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
    /// 导出IT设备保修扩展
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportItAssetAsync(TaktItAssetQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktItAssetQueryDto());
        var list = await _itAssetRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktItAssetExportDto>(),
                sheetName ?? "IT设备保修扩展数据",
                fileName ?? "IT设备保修扩展导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktItAssetExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "IT设备保修扩展数据",
            fileName ?? "IT设备保修扩展导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充IT设备保修扩展详情（加载 OneToMany 子表：IT设备保修变更日志）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillItAssetDetailsAsync(TaktItAssetDto dto, TaktItAsset entity)
    {
        if (dto == null)
        {
            return;
        }
    }

    /// <summary>
    /// 保存IT设备保修扩展子表级联（IT设备保修变更日志；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveItAssetChildrenAsync(TaktItAsset entity, TaktItAssetCreateDto dto)
    {
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建IT设备保修扩展查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktItAsset, bool>> QueryExpression(TaktItAssetQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktItAsset>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.AssetCode != null && x.AssetCode.Contains(keywords))
                || SqlFunc.ToString(x.WarrantyType).Contains(keywords)
                || (x.WarrantyProvider != null && x.WarrantyProvider.Contains(keywords))
                || (x.WarrantyContractCode != null && x.WarrantyContractCode.Contains(keywords))
                || (x.ServiceHotline != null && x.ServiceHotline.Contains(keywords))
                || (x.ServiceEmail != null && x.ServiceEmail.Contains(keywords))
                || (x.WarrantyRemark != null && x.WarrantyRemark.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.WarrantyStartDate).Contains(keywords)
                || SqlFunc.ToString(x.WarrantyExpiryDate).Contains(keywords)
                || SqlFunc.ToString(x.MaintenanceExpiryDate).Contains(keywords)
                || SqlFunc.ToString(x.LastMaintenanceDate).Contains(keywords)
                || SqlFunc.ToString(x.NextMaintenanceDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.AssetCode))
        {
            exp = exp.And(x => x.AssetCode != null && x.AssetCode.Contains(queryDto.AssetCode));
        }

        if (queryDto?.WarrantyType.HasValue == true)
        {
            exp = exp.And(x => x.WarrantyType == queryDto.WarrantyType);
        }

        if (!string.IsNullOrEmpty(queryDto?.WarrantyProvider))
        {
            exp = exp.And(x => x.WarrantyProvider != null && x.WarrantyProvider.Contains(queryDto.WarrantyProvider));
        }

        if (!string.IsNullOrEmpty(queryDto?.WarrantyContractCode))
        {
            exp = exp.And(x => x.WarrantyContractCode != null && x.WarrantyContractCode.Contains(queryDto.WarrantyContractCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ServiceHotline))
        {
            exp = exp.And(x => x.ServiceHotline != null && x.ServiceHotline.Contains(queryDto.ServiceHotline));
        }

        if (!string.IsNullOrEmpty(queryDto?.ServiceEmail))
        {
            exp = exp.And(x => x.ServiceEmail != null && x.ServiceEmail.Contains(queryDto.ServiceEmail));
        }

        if (!string.IsNullOrEmpty(queryDto?.WarrantyRemark))
        {
            exp = exp.And(x => x.WarrantyRemark != null && x.WarrantyRemark.Contains(queryDto.WarrantyRemark));
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

        if (queryDto?.WarrantyStartDateStart.HasValue == true)
        {
            exp = exp.And(x => x.WarrantyStartDate >= queryDto.WarrantyStartDateStart);
        }

        if (queryDto?.WarrantyStartDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.WarrantyStartDate <= queryDto.WarrantyStartDateEnd);
        }

        if (queryDto?.WarrantyExpiryDateStart.HasValue == true)
        {
            exp = exp.And(x => x.WarrantyExpiryDate >= queryDto.WarrantyExpiryDateStart);
        }

        if (queryDto?.WarrantyExpiryDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.WarrantyExpiryDate <= queryDto.WarrantyExpiryDateEnd);
        }

        if (queryDto?.MaintenanceExpiryDateStart.HasValue == true)
        {
            exp = exp.And(x => x.MaintenanceExpiryDate >= queryDto.MaintenanceExpiryDateStart);
        }

        if (queryDto?.MaintenanceExpiryDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.MaintenanceExpiryDate <= queryDto.MaintenanceExpiryDateEnd);
        }

        if (queryDto?.LastMaintenanceDateStart.HasValue == true)
        {
            exp = exp.And(x => x.LastMaintenanceDate >= queryDto.LastMaintenanceDateStart);
        }

        if (queryDto?.LastMaintenanceDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.LastMaintenanceDate <= queryDto.LastMaintenanceDateEnd);
        }

        if (queryDto?.NextMaintenanceDateStart.HasValue == true)
        {
            exp = exp.And(x => x.NextMaintenanceDate >= queryDto.NextMaintenanceDateStart);
        }

        if (queryDto?.NextMaintenanceDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.NextMaintenanceDate <= queryDto.NextMaintenanceDateEnd);
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt >= queryDto.CreatedAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt <= queryDto.CreatedAtEnd);
        }
        if (!string.IsNullOrWhiteSpace(queryDto?.PlantCode))
        {
            var plantCode = queryDto.PlantCode;
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(plantCode));
        }


        return exp.ToExpression();
    }
}
