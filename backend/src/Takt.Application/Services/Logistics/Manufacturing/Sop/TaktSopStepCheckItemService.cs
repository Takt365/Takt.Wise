// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Sop
// 文件名称：TaktSopStepCheckItemService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP工步检验项目应用服务实现
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
/// SOP工步检验项目应用服务
/// </summary>
public class TaktSopStepCheckItemService : TaktServiceBase, ITaktSopStepCheckItemService
{
    private readonly ITaktCompanyRepository<TaktSopStepCheckItem> _sopStepCheckItemRepository;
    private readonly ITaktCompanyRepository<TaktSopStep> _sopStepRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sopStepCheckItemRepository">SOP工步检验项目仓储</param>
    /// <param name="sopStepRepository">SOP工步仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSopStepCheckItemService(
        ITaktCompanyRepository<TaktSopStepCheckItem> sopStepCheckItemRepository,
        ITaktCompanyRepository<TaktSopStep> sopStepRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _sopStepCheckItemRepository = sopStepCheckItemRepository;
        _sopStepRepository = sopStepRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取SOP工步检验项目列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSopStepCheckItemDto>> GetSopStepCheckItemListAsync(TaktSopStepCheckItemQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktSopStepCheckItemDto>.Create(
                new List<TaktSopStepCheckItemDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _sopStepCheckItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSopStepCheckItemDto>.Create(
            data.Adapt<List<TaktSopStepCheckItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取SOP工步检验项目
    /// </summary>
    /// <param name="id">SOP工步检验项目ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopStepCheckItemDto?> GetSopStepCheckItemByIdAsync(long id)
    {
        var entity = await _sopStepCheckItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktSopStepCheckItemDto>();
    }

    /// <summary>
    /// 获取SOP工步检验项目选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSopStepCheckItemOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _sopStepCheckItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.CheckItemName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.CheckItemName,
            DictLabel = e.CheckItemName,
        }).ToList();
    }

    /// <summary>
    /// 创建SOP工步检验项目
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopStepCheckItemDto> CreateSopStepCheckItemAsync(TaktSopStepCheckItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktSopStepCheckItem>();
        await StampSopStepCheckItemSopStepAsync(entity, dto);
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _sopStepCheckItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.StepId == entity.StepId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.StepId, maxSort);
        }
        entity = await _sopStepCheckItemRepository.CreateAsync(entity);
        return await GetSopStepCheckItemByIdAsync(entity.Id) ?? entity.Adapt<TaktSopStepCheckItemDto>();
    }

    /// <summary>
    /// 更新SOP工步检验项目
    /// </summary>
    /// <param name="id">SOP工步检验项目ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopStepCheckItemDto> UpdateSopStepCheckItemAsync(long id, TaktSopStepCheckItemUpdateDto dto)
    {
        var entity = await _sopStepCheckItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("SOP工步检验项目不存在");
        }
        dto.Adapt(entity);
        await StampSopStepCheckItemSopStepAsync(entity, dto);
        await _sopStepCheckItemRepository.UpdateAsync(entity);
        return await GetSopStepCheckItemByIdAsync(id) ?? throw new TaktBusinessException("SOP工步检验项目不存在");
    }

    /// <summary>
    /// 删除SOP工步检验项目
    /// </summary>
    /// <param name="id">SOP工步检验项目ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSopStepCheckItemByIdAsync(long id)
    {
        var deleted = await _sopStepCheckItemRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("SOP工步检验项目不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除SOP工步检验项目
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSopStepCheckItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSopStepCheckItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新SOP工步检验项目排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopStepCheckItemDto> UpdateSopStepCheckItemSortAsync(TaktSopStepCheckItemSortDto dto)
    {
        var entity = await _sopStepCheckItemRepository.GetByIdAsync(dto.SopStepCheckItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("SOP工步检验项目不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _sopStepCheckItemRepository.UpdateAsync(entity);
        return await GetSopStepCheckItemByIdAsync(dto.SopStepCheckItemId) ?? throw new TaktBusinessException("SOP工步检验项目不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSopStepCheckItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSopStepCheckItemTemplateDto>(
            sheetName ?? "SOP工步检验项目导入模板",
            fileName ?? "SOP工步检验项目导入模板.xlsx");
    }

    /// <summary>
    /// 导入SOP工步检验项目
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSopStepCheckItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSopStepCheckItemImportDto>(fileStream, sheetName ?? "SOP工步检验项目导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktSopStepCheckItem>();
                var importDto = rows[i].Adapt<TaktSopStepCheckItemCreateDto>();
                await StampSopStepCheckItemSopStepAsync(entity, importDto);
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _sopStepCheckItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.StepId == entity.StepId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.StepId, maxSort);
                }
                await _sopStepCheckItemRepository.CreateAsync(entity);
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
    /// 导出SOP工步检验项目
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSopStepCheckItemAsync(TaktSopStepCheckItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktSopStepCheckItemQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSopStepCheckItemExportDto>(),
                sheetName ?? "SOP工步检验项目数据",
                fileName ?? "SOP工步检验项目导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _sopStepCheckItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSopStepCheckItemExportDto>(),
                sheetName ?? "SOP工步检验项目数据",
                fileName ?? "SOP工步检验项目导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSopStepCheckItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "SOP工步检验项目数据",
            fileName ?? "SOP工步检验项目导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步SOP工步检验项目主表外键（ManyToOne → SOP工步）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampSopStepCheckItemSopStepAsync(TaktSopStepCheckItem entity, TaktSopStepCheckItemCreateDto dto)
    {
        if (dto.StepId <= 0)
        {
            return;
        }
        var master = await _sopStepRepository.GetByIdAsync(dto.StepId);
        if (master == null)
        {
            throw new TaktBusinessException("SOP工步不存在");
        }
        entity.StepId = master.Id;
        if (string.IsNullOrEmpty(entity.TenantCode))
        {
            entity.TenantCode = master.TenantCode;
        }
        if (string.IsNullOrEmpty(entity.CompanyCode))
        {
            entity.CompanyCode = master.CompanyCode;
        }
        if (string.IsNullOrEmpty(entity.CultureCode))
        {
            entity.CultureCode = master.CultureCode;
        }
        if (string.IsNullOrEmpty(entity.PlantCode))
        {
            entity.PlantCode = master.PlantCode;
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建SOP工步检验项目查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSopStepCheckItem, bool>> QueryExpression(TaktSopStepCheckItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSopStepCheckItem>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.CheckItemName != null && x.CheckItemName.Contains(keywords))
                || (x.CheckMethod != null && x.CheckMethod.Contains(keywords))
                || (x.CheckStandard != null && x.CheckStandard.Contains(keywords))
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

        if (queryDto?.StepId.HasValue == true)
        {
            var stepId = queryDto.StepId.Value;
            exp = exp.And(x => x.StepId == stepId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CheckItemName))
        {
            var checkItemName = queryDto.CheckItemName;
            exp = exp.And(x => x.CheckItemName != null && x.CheckItemName.Contains(checkItemName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CheckMethod))
        {
            var checkMethod = queryDto.CheckMethod;
            exp = exp.And(x => x.CheckMethod != null && x.CheckMethod.Contains(checkMethod));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CheckStandard))
        {
            var checkStandard = queryDto.CheckStandard;
            exp = exp.And(x => x.CheckStandard != null && x.CheckStandard.Contains(checkStandard));
        }

        if (queryDto?.IsRequired.HasValue == true)
        {
            var isRequired = queryDto.IsRequired.Value;
            exp = exp.And(x => x.IsRequired == isRequired);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            var sortOrder = queryDto.SortOrder.Value;
            exp = exp.And(x => x.SortOrder == sortOrder);
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
    private static bool HasAnyListQueryFilter(TaktSopStepCheckItemQueryDto? queryDto)
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
        if (queryDto.StepId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CheckItemName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CheckMethod))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CheckStandard))
        {
            return true;
        }
        if (queryDto.IsRequired.HasValue)
        {
            return true;
        }
        if (queryDto.SortOrder.HasValue)
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
