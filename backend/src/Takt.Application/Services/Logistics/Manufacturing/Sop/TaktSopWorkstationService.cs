// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Sop
// 文件名称：TaktSopWorkstationService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP工位主数据应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Sop;
using Takt.Domain.Entities.Logistics.Manufacturing.Sop;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Sop;

/// <summary>
/// SOP工位主数据应用服务
/// </summary>
public class TaktSopWorkstationService : TaktServiceBase, ITaktSopWorkstationService
{
    private readonly ITaktCompanyRepository<TaktSopWorkstation> _sopWorkstationRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sopWorkstationRepository">SOP工位主数据仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSopWorkstationService(
        ITaktCompanyRepository<TaktSopWorkstation> sopWorkstationRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _sopWorkstationRepository = sopWorkstationRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取SOP工位主数据列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSopWorkstationDto>> GetSopWorkstationListAsync(TaktSopWorkstationQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _sopWorkstationRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSopWorkstationDto>.Create(
            data.Adapt<List<TaktSopWorkstationDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取SOP工位主数据
    /// </summary>
    /// <param name="id">SOP工位主数据ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopWorkstationDto?> GetSopWorkstationByIdAsync(long id)
    {
        var entity = await _sopWorkstationRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktSopWorkstationDto>();
    }

    /// <summary>
    /// 获取SOP工位主数据选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSopWorkstationOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _sopWorkstationRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.WorkstationStatus == 1,
            x => x.WorkstationName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.WorkstationName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建SOP工位主数据
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopWorkstationDto> CreateSopWorkstationAsync(TaktSopWorkstationCreateDto dto)
    {
        var entity = dto.Adapt<TaktSopWorkstation>();
        var isUnique_ix_takt_logistics_manufacturing_sop_workstation_code_unique = await _uniqueValidator.IsUniqueAsync(
            _sopWorkstationRepository,
            x => x.PlantCode == entity.PlantCode
                && x.WorkstationCode == entity.WorkstationCode);
        if (!isUnique_ix_takt_logistics_manufacturing_sop_workstation_code_unique)
        {
            throw new TaktBusinessException("SOP工位主数据的PlantCode、WorkstationCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _sopWorkstationRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _sopWorkstationRepository.CreateAsync(entity);
        return await GetSopWorkstationByIdAsync(entity.Id) ?? entity.Adapt<TaktSopWorkstationDto>();
    }

    /// <summary>
    /// 更新SOP工位主数据
    /// </summary>
    /// <param name="id">SOP工位主数据ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopWorkstationDto> UpdateSopWorkstationAsync(long id, TaktSopWorkstationUpdateDto dto)
    {
        var entity = await _sopWorkstationRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("SOP工位主数据不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_sop_workstation_code_unique = await _uniqueValidator.IsUniqueAsync(
            _sopWorkstationRepository,
            x => x.PlantCode == entity.PlantCode
                && x.WorkstationCode == entity.WorkstationCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_sop_workstation_code_unique)
        {
            throw new TaktBusinessException("SOP工位主数据的PlantCode、WorkstationCode已存在");
        }
        await _sopWorkstationRepository.UpdateAsync(entity);
        return await GetSopWorkstationByIdAsync(id) ?? throw new TaktBusinessException("SOP工位主数据不存在");
    }

    /// <summary>
    /// 删除SOP工位主数据
    /// </summary>
    /// <param name="id">SOP工位主数据ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSopWorkstationByIdAsync(long id)
    {
        var deleted = await _sopWorkstationRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("SOP工位主数据不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除SOP工位主数据
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSopWorkstationBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSopWorkstationByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新SOP工位主数据状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopWorkstationDto> UpdateSopWorkstationStatusAsync(TaktSopWorkstationStatusDto dto)
    {
        var entity = await _sopWorkstationRepository.GetByIdAsync(dto.SopWorkstationId);
        if (entity == null)
        {
            throw new TaktBusinessException("SOP工位主数据不存在");
        }
        entity.WorkstationStatus = dto.WorkstationStatus;
        await _sopWorkstationRepository.UpdateAsync(entity);
        return await GetSopWorkstationByIdAsync(dto.SopWorkstationId) ?? throw new TaktBusinessException("SOP工位主数据不存在");
    }

    /// <summary>
    /// 更新SOP工位主数据排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopWorkstationDto> UpdateSopWorkstationSortAsync(TaktSopWorkstationSortDto dto)
    {
        var entity = await _sopWorkstationRepository.GetByIdAsync(dto.SopWorkstationId);
        if (entity == null)
        {
            throw new TaktBusinessException("SOP工位主数据不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _sopWorkstationRepository.UpdateAsync(entity);
        return await GetSopWorkstationByIdAsync(dto.SopWorkstationId) ?? throw new TaktBusinessException("SOP工位主数据不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSopWorkstationTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSopWorkstationTemplateDto>(
            sheetName ?? "SOP工位主数据导入模板",
            fileName ?? "SOP工位主数据导入模板.xlsx");
    }

    /// <summary>
    /// 导入SOP工位主数据
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSopWorkstationAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSopWorkstationImportDto>(fileStream, sheetName ?? "SOP工位主数据导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _sopWorkstationRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktSopWorkstation>();
                var importKey = $"{entity.PlantCode}|{entity.WorkstationCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、WorkstationCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_sop_workstation_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _sopWorkstationRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.WorkstationCode == entity.WorkstationCode);
                if (!isUnique_ix_takt_logistics_manufacturing_sop_workstation_code_unique)
                {
                    throw new TaktBusinessException("SOP工位主数据的PlantCode、WorkstationCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _sopWorkstationRepository.CreateAsync(entity);
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
    /// 导出SOP工位主数据
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSopWorkstationAsync(TaktSopWorkstationQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktSopWorkstationQueryDto());
        var list = await _sopWorkstationRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSopWorkstationExportDto>(),
                sheetName ?? "SOP工位主数据数据",
                fileName ?? "SOP工位主数据导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSopWorkstationExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "SOP工位主数据数据",
            fileName ?? "SOP工位主数据导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建SOP工位主数据查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSopWorkstation, bool>> QueryExpression(TaktSopWorkstationQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSopWorkstation>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.WorkstationCode != null && x.WorkstationCode.Contains(keywords))
                || (x.WorkstationName != null && x.WorkstationName.Contains(keywords))
                || (x.WorkCenter != null && x.WorkCenter.Contains(keywords))
                || (x.ProductionLine != null && x.ProductionLine.Contains(keywords))
                || SqlFunc.ToString(x.WorkstationType).Contains(keywords)
                || SqlFunc.ToString(x.ProcessSegmentType).Contains(keywords)
                || SqlFunc.ToString(x.WorkstationStatus).Contains(keywords)
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.WorkstationCode))
        {
            exp = exp.And(x => x.WorkstationCode != null && x.WorkstationCode.Contains(queryDto.WorkstationCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.WorkstationName))
        {
            exp = exp.And(x => x.WorkstationName != null && x.WorkstationName.Contains(queryDto.WorkstationName));
        }

        if (!string.IsNullOrEmpty(queryDto?.WorkCenter))
        {
            exp = exp.And(x => x.WorkCenter != null && x.WorkCenter.Contains(queryDto.WorkCenter));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProductionLine))
        {
            exp = exp.And(x => x.ProductionLine != null && x.ProductionLine.Contains(queryDto.ProductionLine));
        }

        if (queryDto?.WorkstationType.HasValue == true)
        {
            exp = exp.And(x => x.WorkstationType == queryDto.WorkstationType);
        }

        if (queryDto?.ProcessSegmentType.HasValue == true)
        {
            exp = exp.And(x => x.ProcessSegmentType == queryDto.ProcessSegmentType);
        }

        if (queryDto?.WorkstationStatus.HasValue == true)
        {
            exp = exp.And(x => x.WorkstationStatus == queryDto.WorkstationStatus);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
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
