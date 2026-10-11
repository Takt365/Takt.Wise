// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktBomGroupService.cs
// 创建时间：2026-10-08
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM组主数据应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM组主数据应用服务
/// </summary>
public class TaktBomGroupService : TaktServiceBase, ITaktBomGroupService
{
    private readonly ITaktCompanyRepository<TaktBomGroup> _bomGroupRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="bomGroupRepository">BOM组主数据仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktBomGroupService(
        ITaktCompanyRepository<TaktBomGroup> bomGroupRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _bomGroupRepository = bomGroupRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取BOM组主数据列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktBomGroupDto>> GetBomGroupListAsync(TaktBomGroupQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktBomGroupDto>.Create(
                new List<TaktBomGroupDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _bomGroupRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktBomGroupDto>.Create(
            data.Adapt<List<TaktBomGroupDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取BOM组主数据
    /// </summary>
    /// <param name="id">BOM组主数据ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktBomGroupDto?> GetBomGroupByIdAsync(long id)
    {
        var entity = await _bomGroupRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktBomGroupDto>();
    }

    /// <summary>
    /// 获取BOM组主数据选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <param name="bomGroupCategory">BOM组类别（字典 logistics_manufacturing_bom_group_category；为空则返回全部启用组）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetBomGroupOptionsAsync(string? plantCode = null, string? keyword = null, int? bomGroupCategory = null)
    {
        EnsureThreeLayerContext();
        var normalizedPlantCode = plantCode?.Trim();
        var normalizedKeyword = keyword?.Trim();
        var predicate = Expressionable.Create<TaktBomGroup>()
            .And(x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.GroupStatus == 1)
            .AndIF(!string.IsNullOrEmpty(normalizedPlantCode), x => x.PlantCode == normalizedPlantCode)
            .AndIF(bomGroupCategory.HasValue, x => x.BomGroupCategory == bomGroupCategory!.Value)
            .AndIF(!string.IsNullOrEmpty(normalizedKeyword), x =>
                (x.BomGroupCode != null && x.BomGroupCode.Contains(normalizedKeyword!))
                || (x.BomGroupName != null && x.BomGroupName.Contains(normalizedKeyword!)))
            .ToExpression();
        var list = await _bomGroupRepository.GetListAsync(
            predicate,
            x => x.BomGroupName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.BomGroupCode,
            DictLabel = e.BomGroupName ?? e.BomGroupCode,
        }).ToList();
    }

    /// <summary>
    /// 创建BOM组主数据
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBomGroupDto> CreateBomGroupAsync(TaktBomGroupCreateDto dto)
    {
        var entity = dto.Adapt<TaktBomGroup>();
        entity.IsBuiltIn = 0;
        var isUnique_ix_takt_logistics_manufacturing_bom_group_unique = await _uniqueValidator.IsUniqueAsync(
            _bomGroupRepository,
            x => x.PlantCode == entity.PlantCode
                && x.BomGroupCategory == entity.BomGroupCategory
                && x.BomGroupCode == entity.BomGroupCode);
        if (!isUnique_ix_takt_logistics_manufacturing_bom_group_unique)
        {
            throw new TaktBusinessException("BOM组主数据的PlantCode、BomGroupCategory、BomGroupCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _bomGroupRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _bomGroupRepository.CreateAsync(entity);
        return await GetBomGroupByIdAsync(entity.Id) ?? entity.Adapt<TaktBomGroupDto>();
    }

    /// <summary>
    /// 更新BOM组主数据
    /// </summary>
    /// <param name="id">BOM组主数据ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBomGroupDto> UpdateBomGroupAsync(long id, TaktBomGroupUpdateDto dto)
    {
        var entity = await _bomGroupRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("BOM组主数据不存在");
        }
        var originalIsBuiltIn = entity.IsBuiltIn;
        dto.Adapt(entity);
        entity.IsBuiltIn = originalIsBuiltIn;
        var isUnique_ix_takt_logistics_manufacturing_bom_group_unique = await _uniqueValidator.IsUniqueAsync(
            _bomGroupRepository,
            x => x.PlantCode == entity.PlantCode
                && x.BomGroupCategory == entity.BomGroupCategory
                && x.BomGroupCode == entity.BomGroupCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_bom_group_unique)
        {
            throw new TaktBusinessException("BOM组主数据的PlantCode、BomGroupCategory、BomGroupCode已存在");
        }
        await _bomGroupRepository.UpdateAsync(entity);
        return await GetBomGroupByIdAsync(id) ?? throw new TaktBusinessException("BOM组主数据不存在");
    }

    /// <summary>
    /// 删除BOM组主数据
    /// </summary>
    /// <param name="id">BOM组主数据ID</param>
    /// <returns>任务</returns>
    public async Task DeleteBomGroupByIdAsync(long id)
    {
        var entity = await _bomGroupRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("BOM组主数据不存在或已删除");
        }
        if (entity.IsBuiltIn == 1)
        {
            throw new TaktBusinessException("内置BOM组主数据不允许删除");
        }
        var deleted = await _bomGroupRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("BOM组主数据不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除BOM组主数据
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteBomGroupBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        if (await _bomGroupRepository.ExistsAsync(x => idList.Contains(x.Id) && x.IsBuiltIn == 1))
        {
            throw new TaktBusinessException("内置BOM组主数据不允许删除");
        }
        foreach (var id in idList)
        {
            await DeleteBomGroupByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新BOM组主数据状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBomGroupDto> UpdateBomGroupStatusAsync(TaktBomGroupStatusDto dto)
    {
        var entity = await _bomGroupRepository.GetByIdAsync(dto.BomGroupId);
        if (entity == null)
        {
            throw new TaktBusinessException("BOM组主数据不存在");
        }
        if (entity.IsBuiltIn == 1 && dto.GroupStatus != 1)
        {
            throw new TaktBusinessException("不允许禁用内置BOM组主数据");
        }
        entity.GroupStatus = dto.GroupStatus;
        await _bomGroupRepository.UpdateAsync(entity);
        return await GetBomGroupByIdAsync(dto.BomGroupId) ?? throw new TaktBusinessException("BOM组主数据不存在");
    }

    /// <summary>
    /// 更新BOM组主数据排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBomGroupDto> UpdateBomGroupSortAsync(TaktBomGroupSortDto dto)
    {
        var entity = await _bomGroupRepository.GetByIdAsync(dto.BomGroupId);
        if (entity == null)
        {
            throw new TaktBusinessException("BOM组主数据不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _bomGroupRepository.UpdateAsync(entity);
        return await GetBomGroupByIdAsync(dto.BomGroupId) ?? throw new TaktBusinessException("BOM组主数据不存在");
    }

    /// <summary>
    /// 更新BOM组主数据内置
    /// </summary>
    /// <param name="dto">内置 DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBomGroupDto> UpdateBomGroupBuiltInAsync(TaktBomGroupBuiltInDto dto)
    {
        var entity = await _bomGroupRepository.GetByIdAsync(dto.BomGroupId);
        if (entity == null)
        {
            throw new TaktBusinessException("BOM组主数据不存在");
        }
        if (dto.IsBuiltIn is not 0 and not 1)
        {
            throw new TaktBusinessException("内置必须为字典 sys_yes_no 合法值（0=否，1=是）");
        }
        if (entity.IsBuiltIn == 1 && dto.IsBuiltIn != 1)
        {
            throw new TaktBusinessException("不允许取消内置BOM组主数据标识");
        }
        entity.IsBuiltIn = dto.IsBuiltIn;
        await _bomGroupRepository.UpdateAsync(entity);
        return await GetBomGroupByIdAsync(dto.BomGroupId) ?? throw new TaktBusinessException("BOM组主数据不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetBomGroupTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktBomGroupTemplateDto>(
            sheetName ?? "BOM组主数据导入模板",
            fileName ?? "BOM组主数据导入模板.xlsx");
    }

    /// <summary>
    /// 导入BOM组主数据
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportBomGroupAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktBomGroupImportDto>(fileStream, sheetName ?? "BOM组主数据导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _bomGroupRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktBomGroup>();
                entity.IsBuiltIn = 0;
                var importKey = $"{entity.PlantCode}|{entity.BomGroupCategory}|{entity.BomGroupCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、BomGroupCategory、BomGroupCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_bom_group_unique = await _uniqueValidator.IsUniqueAsync(
                    _bomGroupRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.BomGroupCategory == entity.BomGroupCategory
                        && x.BomGroupCode == entity.BomGroupCode);
                if (!isUnique_ix_takt_logistics_manufacturing_bom_group_unique)
                {
                    throw new TaktBusinessException("BOM组主数据的PlantCode、BomGroupCategory、BomGroupCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _bomGroupRepository.CreateAsync(entity);
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
    /// 导出BOM组主数据
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportBomGroupAsync(TaktBomGroupQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktBomGroupQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktBomGroupExportDto>(),
                sheetName ?? "BOM组主数据数据",
                fileName ?? "BOM组主数据导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _bomGroupRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktBomGroupExportDto>(),
                sheetName ?? "BOM组主数据数据",
                fileName ?? "BOM组主数据导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktBomGroupExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "BOM组主数据数据",
            fileName ?? "BOM组主数据导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建BOM组主数据查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktBomGroup, bool>> QueryExpression(TaktBomGroupQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktBomGroup>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.BomGroupCode != null && x.BomGroupCode.Contains(keywords))
                || (x.BomGroupName != null && x.BomGroupName.Contains(keywords))
                || (x.BomGroupDescription != null && x.BomGroupDescription.Contains(keywords))
                || (x.ContactPhone != null && x.ContactPhone.Contains(keywords))
                || (x.ContactEmail != null && x.ContactEmail.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.BomGroupCode))
        {
            var bomGroupCode = queryDto.BomGroupCode;
            exp = exp.And(x => x.BomGroupCode != null && x.BomGroupCode.Contains(bomGroupCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BomGroupName))
        {
            var bomGroupName = queryDto.BomGroupName;
            exp = exp.And(x => x.BomGroupName != null && x.BomGroupName.Contains(bomGroupName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BomGroupDescription))
        {
            var bomGroupDescription = queryDto.BomGroupDescription;
            exp = exp.And(x => x.BomGroupDescription != null && x.BomGroupDescription.Contains(bomGroupDescription));
        }

        if (queryDto?.BomGroupCategory.HasValue == true)
        {
            var bomGroupCategory = queryDto.BomGroupCategory.Value;
            exp = exp.And(x => x.BomGroupCategory == bomGroupCategory);
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

        return exp.ToExpression();
    }

    /// <summary>
    /// 是否存在任一业务查询条件（KeyWords / 字段 / 日期范围）；无参时列表与导出返回空，避免全表扫描
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>有条件为 true</returns>
    private static bool HasAnyListQueryFilter(TaktBomGroupQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.BomGroupCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BomGroupName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BomGroupDescription))
        {
            return true;
        }
        if (queryDto.BomGroupCategory.HasValue)
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
