// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：TaktPurchaseGroupService.cs
// 创建时间：2026-08-06
// 创建人：Takt365(Cursor AI)
// 功能描述：采购组主数据应用服务实现
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
/// 采购组主数据应用服务
/// </summary>
public class TaktPurchaseGroupService : TaktServiceBase, ITaktPurchaseGroupService
{
    private readonly ITaktCompanyRepository<TaktPurchaseGroup> _purchaseGroupRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseGroupRepository">采购组主数据仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPurchaseGroupService(
        ITaktCompanyRepository<TaktPurchaseGroup> purchaseGroupRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _purchaseGroupRepository = purchaseGroupRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取采购组主数据列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPurchaseGroupDto>> GetPurchaseGroupListAsync(TaktPurchaseGroupQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktPurchaseGroupDto>.Create(
                new List<TaktPurchaseGroupDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _purchaseGroupRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPurchaseGroupDto>.Create(
            data.Adapt<List<TaktPurchaseGroupDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取采购组主数据
    /// </summary>
    /// <param name="id">采购组主数据ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseGroupDto?> GetPurchaseGroupByIdAsync(long id)
    {
        var entity = await _purchaseGroupRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktPurchaseGroupDto>();
    }

    /// <summary>
    /// 获取采购组主数据选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPurchaseGroupOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _purchaseGroupRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.GroupStatus == 1,
            x => x.SortOrder,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.PurchaseGroupCode,
            DictLabel = string.IsNullOrWhiteSpace(e.PurchaseGroupName) ? e.PurchaseGroupCode : e.PurchaseGroupName,
            ExtLabel = e.PurchaseGroupCode,
            SortOrder = e.SortOrder,
        }).ToList();
    }

    /// <summary>
    /// 创建采购组主数据
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseGroupDto> CreatePurchaseGroupAsync(TaktPurchaseGroupCreateDto dto)
    {
        var entity = dto.Adapt<TaktPurchaseGroup>();
        entity.IsBuiltIn = 0;
        var isUnique_ix_takt_logistics_procurement_purchase_group_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseGroupRepository,
            x => x.PlantCode == entity.PlantCode
                && x.PurchaseGroupCode == entity.PurchaseGroupCode);
        if (!isUnique_ix_takt_logistics_procurement_purchase_group_unique)
        {
            throw new TaktBusinessException("采购组主数据的PlantCode、PurchaseGroupCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _purchaseGroupRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _purchaseGroupRepository.CreateAsync(entity);
        return await GetPurchaseGroupByIdAsync(entity.Id) ?? entity.Adapt<TaktPurchaseGroupDto>();
    }

    /// <summary>
    /// 更新采购组主数据
    /// </summary>
    /// <param name="id">采购组主数据ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseGroupDto> UpdatePurchaseGroupAsync(long id, TaktPurchaseGroupUpdateDto dto)
    {
        var entity = await _purchaseGroupRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购组主数据不存在");
        }
        var originalIsBuiltIn = entity.IsBuiltIn;
        dto.Adapt(entity);
        entity.IsBuiltIn = originalIsBuiltIn;
        var isUnique_ix_takt_logistics_procurement_purchase_group_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseGroupRepository,
            x => x.PlantCode == entity.PlantCode
                && x.PurchaseGroupCode == entity.PurchaseGroupCode,
            id);
        if (!isUnique_ix_takt_logistics_procurement_purchase_group_unique)
        {
            throw new TaktBusinessException("采购组主数据的PlantCode、PurchaseGroupCode已存在");
        }
        await _purchaseGroupRepository.UpdateAsync(entity);
        return await GetPurchaseGroupByIdAsync(id) ?? throw new TaktBusinessException("采购组主数据不存在");
    }

    /// <summary>
    /// 删除采购组主数据
    /// </summary>
    /// <param name="id">采购组主数据ID</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseGroupByIdAsync(long id)
    {
        var entity = await _purchaseGroupRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购组主数据不存在或已删除");
        }
        if (entity.IsBuiltIn == 1)
        {
            throw new TaktBusinessException("内置采购组主数据不允许删除");
        }
        var deleted = await _purchaseGroupRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("采购组主数据不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除采购组主数据
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseGroupBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        if (await _purchaseGroupRepository.ExistsAsync(x => idList.Contains(x.Id) && x.IsBuiltIn == 1))
        {
            throw new TaktBusinessException("内置采购组主数据不允许删除");
        }
        foreach (var id in idList)
        {
            await DeletePurchaseGroupByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新采购组主数据状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseGroupDto> UpdatePurchaseGroupStatusAsync(TaktPurchaseGroupStatusDto dto)
    {
        var entity = await _purchaseGroupRepository.GetByIdAsync(dto.PurchaseGroupId);
        if (entity == null)
        {
            throw new TaktBusinessException("采购组主数据不存在");
        }
        if (entity.IsBuiltIn == 1 && dto.GroupStatus != 1)
        {
            throw new TaktBusinessException("不允许禁用内置采购组主数据");
        }
        entity.GroupStatus = dto.GroupStatus;
        await _purchaseGroupRepository.UpdateAsync(entity);
        return await GetPurchaseGroupByIdAsync(dto.PurchaseGroupId) ?? throw new TaktBusinessException("采购组主数据不存在");
    }

    /// <summary>
    /// 更新采购组主数据排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseGroupDto> UpdatePurchaseGroupSortAsync(TaktPurchaseGroupSortDto dto)
    {
        var entity = await _purchaseGroupRepository.GetByIdAsync(dto.PurchaseGroupId);
        if (entity == null)
        {
            throw new TaktBusinessException("采购组主数据不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _purchaseGroupRepository.UpdateAsync(entity);
        return await GetPurchaseGroupByIdAsync(dto.PurchaseGroupId) ?? throw new TaktBusinessException("采购组主数据不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPurchaseGroupTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPurchaseGroupTemplateDto>(
            sheetName ?? "采购组主数据导入模板",
            fileName ?? "采购组主数据导入模板.xlsx");
    }

    /// <summary>
    /// 导入采购组主数据
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPurchaseGroupAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPurchaseGroupImportDto>(fileStream, sheetName ?? "采购组主数据导入模板");
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
                var entity = rows[i].Adapt<TaktPurchaseGroup>();
                entity.IsBuiltIn = 0;
                var importKey = $"{entity.PlantCode}|{entity.PurchaseGroupCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、PurchaseGroupCode）");
                }
                var isUnique_ix_takt_logistics_procurement_purchase_group_unique = await _uniqueValidator.IsUniqueAsync(
                    _purchaseGroupRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.PurchaseGroupCode == entity.PurchaseGroupCode);
                if (!isUnique_ix_takt_logistics_procurement_purchase_group_unique)
                {
                    throw new TaktBusinessException("采购组主数据的PlantCode、PurchaseGroupCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _purchaseGroupRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
                }
                await _purchaseGroupRepository.CreateAsync(entity);
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
    /// 导出采购组主数据
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPurchaseGroupAsync(TaktPurchaseGroupQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktPurchaseGroupQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchaseGroupExportDto>(),
                sheetName ?? "采购组主数据数据",
                fileName ?? "采购组主数据导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _purchaseGroupRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchaseGroupExportDto>(),
                sheetName ?? "采购组主数据数据",
                fileName ?? "采购组主数据导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPurchaseGroupExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "采购组主数据数据",
            fileName ?? "采购组主数据导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建采购组主数据查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPurchaseGroup, bool>> QueryExpression(TaktPurchaseGroupQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPurchaseGroup>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.PurchaseGroupCode != null && x.PurchaseGroupCode.Contains(keywords))
                || (x.PurchaseGroupName != null && x.PurchaseGroupName.Contains(keywords))
                || (x.PurchaseGroupDescription != null && x.PurchaseGroupDescription.Contains(keywords))
                || (x.ContactPhone != null && x.ContactPhone.Contains(keywords))
                || (x.ContactEmail != null && x.ContactEmail.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
            );
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PlantCode))
        {
            var plantCode = queryDto.PlantCode;
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(plantCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchaseGroupCode))
        {
            var purchaseGroupCode = queryDto.PurchaseGroupCode;
            exp = exp.And(x => x.PurchaseGroupCode != null && x.PurchaseGroupCode.Contains(purchaseGroupCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchaseGroupName))
        {
            var purchaseGroupName = queryDto.PurchaseGroupName;
            exp = exp.And(x => x.PurchaseGroupName != null && x.PurchaseGroupName.Contains(purchaseGroupName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchaseGroupDescription))
        {
            var purchaseGroupDescription = queryDto.PurchaseGroupDescription;
            exp = exp.And(x => x.PurchaseGroupDescription != null && x.PurchaseGroupDescription.Contains(purchaseGroupDescription));
        }
        if (queryDto?.PurchaseGroupCategory.HasValue == true)
        {
            var purchaseGroupCategory = queryDto.PurchaseGroupCategory.Value;
            exp = exp.And(x => x.PurchaseGroupCategory == purchaseGroupCategory);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ContactPhone))
        {
            var contactPhone = queryDto.ContactPhone;
            exp = exp.And(x => x.ContactPhone != null && x.ContactPhone.Contains(contactPhone));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ContactEmail))
        {
            var contactEmail = queryDto.ContactEmail;
            exp = exp.And(x => x.ContactEmail != null && x.ContactEmail.Contains(contactEmail));
        }

        if (queryDto?.IsBuiltIn.HasValue == true)
        {
            var isBuiltIn = queryDto.IsBuiltIn.Value;
            exp = exp.And(x => x.IsBuiltIn == isBuiltIn);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            var sortOrder = queryDto.SortOrder.Value;
            exp = exp.And(x => x.SortOrder == sortOrder);
        }

        if (queryDto?.GroupStatus.HasValue == true)
        {
            var groupStatus = queryDto.GroupStatus.Value;
            exp = exp.And(x => x.GroupStatus == groupStatus);
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

        if (!string.IsNullOrEmpty(queryDto?.CultureCode))
        {
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(queryDto.CultureCode));
        }

        return exp.ToExpression();
    }

    /// <summary>
    /// 是否存在任一业务查询条件（KeyWords / 字段 / 日期范围）；无参时列表与导出返回空，避免全表扫描
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>有条件为 true</returns>
    private static bool HasAnyListQueryFilter(TaktPurchaseGroupQueryDto? queryDto)
    {
        if (queryDto == null)
        {
            return false;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.KeyWords))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PlantCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseGroupCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseGroupName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseGroupDescription))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ContactPhone))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ContactEmail))
        {
            return true;
        }
        if (queryDto.IsBuiltIn.HasValue)
        {
            return true;
        }
        if (queryDto.SortOrder.HasValue)
        {
            return true;
        }
        if (queryDto.GroupStatus.HasValue)
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
}
