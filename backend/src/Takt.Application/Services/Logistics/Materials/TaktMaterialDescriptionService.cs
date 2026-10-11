// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：TaktMaterialDescriptionService.cs
// 创建时间：2026-08-05
// 创建人：Takt365(Cursor AI)
// 功能描述：物料描述应用服务实现
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
/// 物料描述应用服务
/// </summary>
public class TaktMaterialDescriptionService : TaktServiceBase, ITaktMaterialDescriptionService
{
    private readonly ITaktTenantRepository<TaktMaterialDescription> _materialDescriptionRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="materialDescriptionRepository">物料描述仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMaterialDescriptionService(
        ITaktTenantRepository<TaktMaterialDescription> materialDescriptionRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _materialDescriptionRepository = materialDescriptionRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取物料描述列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktMaterialDescriptionDto>> GetMaterialDescriptionListAsync(TaktMaterialDescriptionQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _materialDescriptionRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktMaterialDescriptionDto>.Create(
            data.Adapt<List<TaktMaterialDescriptionDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取物料描述
    /// </summary>
    /// <param name="id">物料描述ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialDescriptionDto?> GetMaterialDescriptionByIdAsync(long id)
    {
        var entity = await _materialDescriptionRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode)
        {
            return null;
        }
        return entity.Adapt<TaktMaterialDescriptionDto>();
    }

    /// <summary>
    /// 获取物料描述选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetMaterialDescriptionOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        var list = await _materialDescriptionRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode,
            x => x.CultureCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.CultureCode,
            DictLabel = e.CultureCode,
        }).ToList();
    }

    /// <summary>
    /// 创建物料描述
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialDescriptionDto> CreateMaterialDescriptionAsync(TaktMaterialDescriptionCreateDto dto)
    {
        var entity = dto.Adapt<TaktMaterialDescription>();
        var isUnique_ix_takt_logistics_materials_material_description_unique = await _uniqueValidator.IsUniqueAsync(
            _materialDescriptionRepository,
            x => x.MaterialCode == entity.MaterialCode
                && x.CultureCode == entity.CultureCode);
        if (!isUnique_ix_takt_logistics_materials_material_description_unique)
        {
            throw new TaktBusinessException("物料描述的MaterialCode、CultureCode已存在");
        }
        entity = await _materialDescriptionRepository.CreateAsync(entity);
        return await GetMaterialDescriptionByIdAsync(entity.Id) ?? entity.Adapt<TaktMaterialDescriptionDto>();
    }

    /// <summary>
    /// 更新物料描述
    /// </summary>
    /// <param name="id">物料描述ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialDescriptionDto> UpdateMaterialDescriptionAsync(long id, TaktMaterialDescriptionUpdateDto dto)
    {
        var entity = await _materialDescriptionRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("物料描述不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_materials_material_description_unique = await _uniqueValidator.IsUniqueAsync(
            _materialDescriptionRepository,
            x => x.MaterialCode == entity.MaterialCode
                && x.CultureCode == entity.CultureCode,
            id);
        if (!isUnique_ix_takt_logistics_materials_material_description_unique)
        {
            throw new TaktBusinessException("物料描述的MaterialCode、CultureCode已存在");
        }
        await _materialDescriptionRepository.UpdateAsync(entity);
        return await GetMaterialDescriptionByIdAsync(id) ?? throw new TaktBusinessException("物料描述不存在");
    }

    /// <summary>
    /// 删除物料描述
    /// </summary>
    /// <param name="id">物料描述ID</param>
    /// <returns>任务</returns>
    public async Task DeleteMaterialDescriptionByIdAsync(long id)
    {
        var deleted = await _materialDescriptionRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("物料描述不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除物料描述
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteMaterialDescriptionBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteMaterialDescriptionByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetMaterialDescriptionTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktMaterialDescriptionTemplateDto>(
            sheetName ?? "物料描述导入模板",
            fileName ?? "物料描述导入模板.xlsx");
    }

    /// <summary>
    /// 导入物料描述
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportMaterialDescriptionAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktMaterialDescriptionImportDto>(fileStream, sheetName ?? "物料描述导入模板");
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
                var entity = rows[i].Adapt<TaktMaterialDescription>();
                var importKey = $"{entity.MaterialCode}|{entity.CultureCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（MaterialCode、CultureCode）");
                }
                var isUnique_ix_takt_logistics_materials_material_description_unique = await _uniqueValidator.IsUniqueAsync(
                    _materialDescriptionRepository,
                    x => x.MaterialCode == entity.MaterialCode
                        && x.CultureCode == entity.CultureCode);
                if (!isUnique_ix_takt_logistics_materials_material_description_unique)
                {
                    throw new TaktBusinessException("物料描述的MaterialCode、CultureCode已存在");
                }
                await _materialDescriptionRepository.CreateAsync(entity);
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
    /// 导出物料描述
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportMaterialDescriptionAsync(TaktMaterialDescriptionQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktMaterialDescriptionQueryDto());
        var list = await _materialDescriptionRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMaterialDescriptionExportDto>(),
                sheetName ?? "物料描述数据",
                fileName ?? "物料描述导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktMaterialDescriptionExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "物料描述数据",
            fileName ?? "物料描述导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建物料描述查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktMaterialDescription, bool>> QueryExpression(TaktMaterialDescriptionQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktMaterialDescription>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.MaterialDescription != null && x.MaterialDescription.Contains(keywords))
                || (x.MaterialSpecification != null && x.MaterialSpecification.Contains(keywords))
                || (x.MaterialModel != null && x.MaterialModel.Contains(keywords))
                || (x.MaterialLongDescription != null && x.MaterialLongDescription.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialCode))
        {
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(queryDto.MaterialCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialDescription))
        {
            exp = exp.And(x => x.MaterialDescription != null && x.MaterialDescription.Contains(queryDto.MaterialDescription));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialSpecification))
        {
            exp = exp.And(x => x.MaterialSpecification != null && x.MaterialSpecification.Contains(queryDto.MaterialSpecification));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialModel))
        {
            exp = exp.And(x => x.MaterialModel != null && x.MaterialModel.Contains(queryDto.MaterialModel));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialLongDescription))
        {
            exp = exp.And(x => x.MaterialLongDescription != null && x.MaterialLongDescription.Contains(queryDto.MaterialLongDescription));
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

        var rangeStart = queryDto?.CreatedAtStart;
        var rangeEnd = queryDto?.CreatedAtEnd;
        if (!rangeStart.HasValue && !rangeEnd.HasValue && !HasFiltersBesidesDefaultListScope(queryDto))
        {
            var monthBounds = GetCurrentMonthRangeBounds();
            rangeStart = monthBounds.Start;
            rangeEnd = monthBounds.End;
        }

        if (rangeStart.HasValue)
        {
            exp = exp.And(x => x.CreatedAt >= rangeStart.Value);
        }

        if (rangeEnd.HasValue)
        {
            exp = exp.And(x => x.CreatedAt <= rangeEnd.Value);
        }


        return exp.ToExpression();
    }

    /// <summary>
    /// 当前自然月起止（含月末最后一刻），用于列表无参默认过滤、避免全表扫描
    /// </summary>
    /// <returns>起、止</returns>
    private static (DateTime Start, DateTime End) GetCurrentMonthRangeBounds()
    {
        var today = DateTime.Today;
        var start = new DateTime(today.Year, today.Month, 1);
        var end = start.AddMonths(1).AddTicks(-1);
        return (start, end);
    }
    /// <summary>
    /// 是否存在除默认当前月/当前期间外的查询条件（有参则不强制默认范围）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>有其它条件为 true</returns>
    private static bool HasFiltersBesidesDefaultListScope(TaktMaterialDescriptionQueryDto? queryDto)
    {
        if (queryDto == null)
        {
            return false;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.KeyWords))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialDescription))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialSpecification))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialModel))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialLongDescription))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CultureCode))
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
        return false;
    }
}
