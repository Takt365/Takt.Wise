// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Benefits
// 文件名称：TaktBenefitItemService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：福利项目应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.HumanResource.Benefits;
using Takt.Domain.Entities.HumanResource.Benefits;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.HumanResource.Benefits;

/// <summary>
/// 福利项目应用服务
/// </summary>
public class TaktBenefitItemService : TaktServiceBase, ITaktBenefitItemService
{
    private readonly ITaktCompanyRepository<TaktBenefitItem> _benefitItemRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="benefitItemRepository">福利项目仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktBenefitItemService(
        ITaktCompanyRepository<TaktBenefitItem> benefitItemRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _benefitItemRepository = benefitItemRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取福利项目列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktBenefitItemDto>> GetBenefitItemListAsync(TaktBenefitItemQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _benefitItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktBenefitItemDto>.Create(
            data.Adapt<List<TaktBenefitItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取福利项目
    /// </summary>
    /// <param name="id">福利项目ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktBenefitItemDto?> GetBenefitItemByIdAsync(long id)
    {
        var entity = await _benefitItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktBenefitItemDto>();
    }

    /// <summary>
    /// 获取福利项目选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetBenefitItemOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _benefitItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ItemStatus == 1,
            x => x.ItemName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.ItemName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建福利项目
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBenefitItemDto> CreateBenefitItemAsync(TaktBenefitItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktBenefitItem>();
        var isUnique_ix_benefit_item_code_unique = await _uniqueValidator.IsUniqueAsync(
            _benefitItemRepository,
            x => x.ItemCode == entity.ItemCode);
        if (!isUnique_ix_benefit_item_code_unique)
        {
            throw new TaktBusinessException("福利项目的ItemCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _benefitItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _benefitItemRepository.CreateAsync(entity);
        return await GetBenefitItemByIdAsync(entity.Id) ?? entity.Adapt<TaktBenefitItemDto>();
    }

    /// <summary>
    /// 更新福利项目
    /// </summary>
    /// <param name="id">福利项目ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBenefitItemDto> UpdateBenefitItemAsync(long id, TaktBenefitItemUpdateDto dto)
    {
        var entity = await _benefitItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("福利项目不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_benefit_item_code_unique = await _uniqueValidator.IsUniqueAsync(
            _benefitItemRepository,
            x => x.ItemCode == entity.ItemCode,
            id);
        if (!isUnique_ix_benefit_item_code_unique)
        {
            throw new TaktBusinessException("福利项目的ItemCode已存在");
        }
        await _benefitItemRepository.UpdateAsync(entity);
        return await GetBenefitItemByIdAsync(id) ?? throw new TaktBusinessException("福利项目不存在");
    }

    /// <summary>
    /// 删除福利项目
    /// </summary>
    /// <param name="id">福利项目ID</param>
    /// <returns>任务</returns>
    public async Task DeleteBenefitItemByIdAsync(long id)
    {
        var deleted = await _benefitItemRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("福利项目不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除福利项目
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteBenefitItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteBenefitItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新福利项目状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBenefitItemDto> UpdateBenefitItemStatusAsync(TaktBenefitItemStatusDto dto)
    {
        var entity = await _benefitItemRepository.GetByIdAsync(dto.BenefitItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("福利项目不存在");
        }
        entity.ItemStatus = dto.ItemStatus;
        await _benefitItemRepository.UpdateAsync(entity);
        return await GetBenefitItemByIdAsync(dto.BenefitItemId) ?? throw new TaktBusinessException("福利项目不存在");
    }

    /// <summary>
    /// 更新福利项目排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBenefitItemDto> UpdateBenefitItemSortAsync(TaktBenefitItemSortDto dto)
    {
        var entity = await _benefitItemRepository.GetByIdAsync(dto.BenefitItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("福利项目不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _benefitItemRepository.UpdateAsync(entity);
        return await GetBenefitItemByIdAsync(dto.BenefitItemId) ?? throw new TaktBusinessException("福利项目不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetBenefitItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktBenefitItemTemplateDto>(
            sheetName ?? "福利项目导入模板",
            fileName ?? "福利项目导入模板.xlsx");
    }

    /// <summary>
    /// 导入福利项目
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportBenefitItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktBenefitItemImportDto>(fileStream, sheetName ?? "福利项目导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _benefitItemRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktBenefitItem>();
                var importKey = $"{entity.ItemCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（ItemCode）");
                }
                var isUnique_ix_benefit_item_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _benefitItemRepository,
                    x => x.ItemCode == entity.ItemCode);
                if (!isUnique_ix_benefit_item_code_unique)
                {
                    throw new TaktBusinessException("福利项目的ItemCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _benefitItemRepository.CreateAsync(entity);
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
    /// 导出福利项目
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportBenefitItemAsync(TaktBenefitItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktBenefitItemQueryDto());
        var list = await _benefitItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktBenefitItemExportDto>(),
                sheetName ?? "福利项目数据",
                fileName ?? "福利项目导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktBenefitItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "福利项目数据",
            fileName ?? "福利项目导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建福利项目查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktBenefitItem, bool>> QueryExpression(TaktBenefitItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktBenefitItem>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.ItemCode != null && x.ItemCode.Contains(keywords))
                || (x.ItemName != null && x.ItemName.Contains(keywords))
                || SqlFunc.ToString(x.BenefitCategory).Contains(keywords)
                || SqlFunc.ToString(x.BenefitType).Contains(keywords)
                || SqlFunc.ToString(x.PaymentCycle).Contains(keywords)
                || SqlFunc.ToString(x.DefaultAmount).Contains(keywords)
                || SqlFunc.ToString(x.MaxAmount).Contains(keywords)
                || SqlFunc.ToString(x.EmployerRatio).Contains(keywords)
                || SqlFunc.ToString(x.EmployeeRatio).Contains(keywords)
                || SqlFunc.ToString(x.IsMandatory).Contains(keywords)
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || SqlFunc.ToString(x.ItemStatus).Contains(keywords)
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.ItemCode))
        {
            exp = exp.And(x => x.ItemCode != null && x.ItemCode.Contains(queryDto.ItemCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ItemName))
        {
            exp = exp.And(x => x.ItemName != null && x.ItemName.Contains(queryDto.ItemName));
        }

        if (queryDto?.BenefitCategory.HasValue == true)
        {
            exp = exp.And(x => x.BenefitCategory == queryDto.BenefitCategory);
        }

        if (queryDto?.BenefitType.HasValue == true)
        {
            exp = exp.And(x => x.BenefitType == queryDto.BenefitType);
        }

        if (queryDto?.PaymentCycle.HasValue == true)
        {
            exp = exp.And(x => x.PaymentCycle == queryDto.PaymentCycle);
        }

        if (queryDto?.DefaultAmount.HasValue == true)
        {
            exp = exp.And(x => x.DefaultAmount == queryDto.DefaultAmount);
        }

        if (queryDto?.MaxAmount.HasValue == true)
        {
            exp = exp.And(x => x.MaxAmount == queryDto.MaxAmount);
        }

        if (queryDto?.EmployerRatio.HasValue == true)
        {
            exp = exp.And(x => x.EmployerRatio == queryDto.EmployerRatio);
        }

        if (queryDto?.EmployeeRatio.HasValue == true)
        {
            exp = exp.And(x => x.EmployeeRatio == queryDto.EmployeeRatio);
        }

        if (queryDto?.IsMandatory.HasValue == true)
        {
            exp = exp.And(x => x.IsMandatory == queryDto.IsMandatory);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (queryDto?.ItemStatus.HasValue == true)
        {
            exp = exp.And(x => x.ItemStatus == queryDto.ItemStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
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
