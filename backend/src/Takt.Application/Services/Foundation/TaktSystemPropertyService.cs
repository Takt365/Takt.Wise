// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：TaktSystemPropertyService.cs
// 创建时间：2026-09-16
// 创建人：Takt365(Cursor AI)
// 功能描述：系统属性应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Foundation;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 系统属性应用服务
/// </summary>
public class TaktSystemPropertyService : TaktServiceBase, ITaktSystemPropertyService
{
    private readonly ITaktCompanyRepository<TaktSystemProperty> _systemPropertyRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="systemPropertyRepository">系统属性仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSystemPropertyService(
        ITaktCompanyRepository<TaktSystemProperty> systemPropertyRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _systemPropertyRepository = systemPropertyRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取系统属性列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSystemPropertyDto>> GetSystemPropertyListAsync(TaktSystemPropertyQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktSystemPropertyDto>.Create(
                new List<TaktSystemPropertyDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _systemPropertyRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSystemPropertyDto>.Create(
            data.Adapt<List<TaktSystemPropertyDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取系统属性
    /// </summary>
    /// <param name="id">系统属性ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSystemPropertyDto?> GetSystemPropertyByIdAsync(long id)
    {
        var entity = await _systemPropertyRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktSystemPropertyDto>();
    }

    /// <summary>
    /// 获取系统属性选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSystemPropertyOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var normalizedPlantCode = plantCode?.Trim();
        var normalizedKeyword = keyword?.Trim();
        var predicate = Expressionable.Create<TaktSystemProperty>()
            .And(x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.SystemPropertyStatus == 1)
            .AndIF(!string.IsNullOrEmpty(normalizedKeyword), x =>
                (x.SystemPropertyName != null && x.SystemPropertyName.Contains(normalizedKeyword!)))
            .ToExpression();
        var list = await _systemPropertyRepository.GetListAsync(
            predicate,
            x => x.SystemPropertyName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.SystemPropertyName,
            DictLabel = e.SystemPropertyName,
        }).ToList();
    }

    /// <summary>
    /// 创建系统属性
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSystemPropertyDto> CreateSystemPropertyAsync(TaktSystemPropertyCreateDto dto)
    {
        var entity = dto.Adapt<TaktSystemProperty>();
        entity.IsBuiltIn = 0;
        var isUnique_ix_system_property_key_unique = await _uniqueValidator.IsUniqueAsync(
            _systemPropertyRepository,
            x => x.SystemPropertyKey == entity.SystemPropertyKey);
        if (!isUnique_ix_system_property_key_unique)
        {
            throw new TaktBusinessException("系统属性的SystemPropertyKey已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _systemPropertyRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _systemPropertyRepository.CreateAsync(entity);
        return await GetSystemPropertyByIdAsync(entity.Id) ?? entity.Adapt<TaktSystemPropertyDto>();
    }

    /// <summary>
    /// 更新系统属性
    /// </summary>
    /// <param name="id">系统属性ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSystemPropertyDto> UpdateSystemPropertyAsync(long id, TaktSystemPropertyUpdateDto dto)
    {
        var entity = await _systemPropertyRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("系统属性不存在");
        }
        var originalIsBuiltIn = entity.IsBuiltIn;
        dto.Adapt(entity);
        entity.IsBuiltIn = originalIsBuiltIn;
        var isUnique_ix_system_property_key_unique = await _uniqueValidator.IsUniqueAsync(
            _systemPropertyRepository,
            x => x.SystemPropertyKey == entity.SystemPropertyKey,
            id);
        if (!isUnique_ix_system_property_key_unique)
        {
            throw new TaktBusinessException("系统属性的SystemPropertyKey已存在");
        }
        await _systemPropertyRepository.UpdateAsync(entity);
        return await GetSystemPropertyByIdAsync(id) ?? throw new TaktBusinessException("系统属性不存在");
    }

    /// <summary>
    /// 删除系统属性
    /// </summary>
    /// <param name="id">系统属性ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSystemPropertyByIdAsync(long id)
    {
        var entity = await _systemPropertyRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("系统属性不存在或已删除");
        }
        if (entity.IsBuiltIn == 1)
        {
            throw new TaktBusinessException("内置系统属性不允许删除");
        }
        var deleted = await _systemPropertyRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("系统属性不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除系统属性
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSystemPropertyBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        if (await _systemPropertyRepository.ExistsAsync(x => idList.Contains(x.Id) && x.IsBuiltIn == 1))
        {
            throw new TaktBusinessException("内置系统属性不允许删除");
        }
        foreach (var id in idList)
        {
            await DeleteSystemPropertyByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新系统属性状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSystemPropertyDto> UpdateSystemPropertyStatusAsync(TaktSystemPropertyStatusDto dto)
    {
        var entity = await _systemPropertyRepository.GetByIdAsync(dto.SystemPropertyId);
        if (entity == null)
        {
            throw new TaktBusinessException("系统属性不存在");
        }
        if (entity.IsBuiltIn == 1 && dto.SystemPropertyStatus != 1)
        {
            throw new TaktBusinessException("不允许禁用内置系统属性");
        }
        entity.SystemPropertyStatus = dto.SystemPropertyStatus;
        await _systemPropertyRepository.UpdateAsync(entity);
        return await GetSystemPropertyByIdAsync(dto.SystemPropertyId) ?? throw new TaktBusinessException("系统属性不存在");
    }

    /// <summary>
    /// 更新系统属性排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSystemPropertyDto> UpdateSystemPropertySortAsync(TaktSystemPropertySortDto dto)
    {
        var entity = await _systemPropertyRepository.GetByIdAsync(dto.SystemPropertyId);
        if (entity == null)
        {
            throw new TaktBusinessException("系统属性不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _systemPropertyRepository.UpdateAsync(entity);
        return await GetSystemPropertyByIdAsync(dto.SystemPropertyId) ?? throw new TaktBusinessException("系统属性不存在");
    }

    /// <summary>
    /// 更新系统属性内置
    /// </summary>
    /// <param name="dto">内置 DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSystemPropertyDto> UpdateSystemPropertyBuiltInAsync(TaktSystemPropertyBuiltInDto dto)
    {
        var entity = await _systemPropertyRepository.GetByIdAsync(dto.SystemPropertyId);
        if (entity == null)
        {
            throw new TaktBusinessException("系统属性不存在");
        }
        if (dto.IsBuiltIn is not 0 and not 1)
        {
            throw new TaktBusinessException("内置必须为字典 sys_yes_no 合法值（0=否，1=是）");
        }
        if (entity.IsBuiltIn == 1 && dto.IsBuiltIn != 1)
        {
            throw new TaktBusinessException("不允许取消内置系统属性标识");
        }
        entity.IsBuiltIn = dto.IsBuiltIn;
        await _systemPropertyRepository.UpdateAsync(entity);
        return await GetSystemPropertyByIdAsync(dto.SystemPropertyId) ?? throw new TaktBusinessException("系统属性不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSystemPropertyTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSystemPropertyTemplateDto>(
            sheetName ?? "系统属性导入模板",
            fileName ?? "系统属性导入模板.xlsx");
    }

    /// <summary>
    /// 导入系统属性
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSystemPropertyAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSystemPropertyImportDto>(fileStream, sheetName ?? "系统属性导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _systemPropertyRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktSystemProperty>();
                entity.IsBuiltIn = 0;
                var importKey = $"{entity.SystemPropertyKey}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（SystemPropertyKey）");
                }
                var isUnique_ix_system_property_key_unique = await _uniqueValidator.IsUniqueAsync(
                    _systemPropertyRepository,
                    x => x.SystemPropertyKey == entity.SystemPropertyKey);
                if (!isUnique_ix_system_property_key_unique)
                {
                    throw new TaktBusinessException("系统属性的SystemPropertyKey已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _systemPropertyRepository.CreateAsync(entity);
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
    /// 导出系统属性
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSystemPropertyAsync(TaktSystemPropertyQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktSystemPropertyQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSystemPropertyExportDto>(),
                sheetName ?? "系统属性数据",
                fileName ?? "系统属性导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _systemPropertyRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSystemPropertyExportDto>(),
                sheetName ?? "系统属性数据",
                fileName ?? "系统属性导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSystemPropertyExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "系统属性数据",
            fileName ?? "系统属性导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建系统属性查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSystemProperty, bool>> QueryExpression(TaktSystemPropertyQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSystemProperty>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.SystemPropertyKey != null && x.SystemPropertyKey.Contains(keywords))
                || (x.SystemPropertyValue != null && x.SystemPropertyValue.Contains(keywords))
                || (x.SystemPropertyName != null && x.SystemPropertyName.Contains(keywords))
                || (x.SystemPropertyDescription != null && x.SystemPropertyDescription.Contains(keywords))
                || (x.SystemPropertyGroup != null && x.SystemPropertyGroup.Contains(keywords))
                || (x.ValueType != null && x.ValueType.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.SystemPropertyKey))
        {
            var systemPropertyKey = queryDto.SystemPropertyKey;
            exp = exp.And(x => x.SystemPropertyKey != null && x.SystemPropertyKey.Contains(systemPropertyKey));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SystemPropertyValue))
        {
            var systemPropertyValue = queryDto.SystemPropertyValue;
            exp = exp.And(x => x.SystemPropertyValue != null && x.SystemPropertyValue.Contains(systemPropertyValue));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SystemPropertyName))
        {
            var systemPropertyName = queryDto.SystemPropertyName;
            exp = exp.And(x => x.SystemPropertyName != null && x.SystemPropertyName.Contains(systemPropertyName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SystemPropertyDescription))
        {
            var systemPropertyDescription = queryDto.SystemPropertyDescription;
            exp = exp.And(x => x.SystemPropertyDescription != null && x.SystemPropertyDescription.Contains(systemPropertyDescription));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SystemPropertyGroup))
        {
            var systemPropertyGroup = queryDto.SystemPropertyGroup;
            exp = exp.And(x => x.SystemPropertyGroup != null && x.SystemPropertyGroup.Contains(systemPropertyGroup));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ValueType))
        {
            var valueType = queryDto.ValueType;
            exp = exp.And(x => x.ValueType != null && x.ValueType.Contains(valueType));
        }

        if (queryDto?.IsBuiltIn.HasValue == true)
        {
            var isBuiltIn = queryDto.IsBuiltIn.Value;
            exp = exp.And(x => x.IsBuiltIn == isBuiltIn);
        }

        if (queryDto?.IsReadonly.HasValue == true)
        {
            var isReadonly = queryDto.IsReadonly.Value;
            exp = exp.And(x => x.IsReadonly == isReadonly);
        }

        if (queryDto?.IsEncrypted.HasValue == true)
        {
            var isEncrypted = queryDto.IsEncrypted.Value;
            exp = exp.And(x => x.IsEncrypted == isEncrypted);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            var sortOrder = queryDto.SortOrder.Value;
            exp = exp.And(x => x.SortOrder == sortOrder);
        }

        if (queryDto?.SystemPropertyStatus.HasValue == true)
        {
            var systemPropertyStatus = queryDto.SystemPropertyStatus.Value;
            exp = exp.And(x => x.SystemPropertyStatus == systemPropertyStatus);
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
    private static bool HasAnyListQueryFilter(TaktSystemPropertyQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.SystemPropertyKey))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SystemPropertyValue))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SystemPropertyName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SystemPropertyDescription))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SystemPropertyGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ValueType))
        {
            return true;
        }
        if (queryDto.IsBuiltIn.HasValue)
        {
            return true;
        }
        if (queryDto.IsReadonly.HasValue)
        {
            return true;
        }
        if (queryDto.IsEncrypted.HasValue)
        {
            return true;
        }
        if (queryDto.SortOrder.HasValue)
        {
            return true;
        }
        if (queryDto.SystemPropertyStatus.HasValue)
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
