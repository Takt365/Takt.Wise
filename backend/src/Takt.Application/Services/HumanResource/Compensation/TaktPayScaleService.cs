// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Compensation
// 文件名称：TaktPayScaleService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：薪级应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.HumanResource.Compensation;
using Takt.Domain.Entities.HumanResource.Compensation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.HumanResource.Compensation;

/// <summary>
/// 薪级应用服务
/// </summary>
public class TaktPayScaleService : TaktServiceBase, ITaktPayScaleService
{
    private readonly ITaktCompanyRepository<TaktPayScale> _payScaleRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="payScaleRepository">薪级仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPayScaleService(
        ITaktCompanyRepository<TaktPayScale> payScaleRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _payScaleRepository = payScaleRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取薪级列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPayScaleDto>> GetPayScaleListAsync(TaktPayScaleQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _payScaleRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPayScaleDto>.Create(
            data.Adapt<List<TaktPayScaleDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取薪级
    /// </summary>
    /// <param name="id">薪级ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPayScaleDto?> GetPayScaleByIdAsync(long id)
    {
        var entity = await _payScaleRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktPayScaleDto>();
    }

    /// <summary>
    /// 获取薪级选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPayScaleOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _payScaleRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ScaleStatus == 1,
            x => x.ScaleName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.ScaleName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建薪级
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPayScaleDto> CreatePayScaleAsync(TaktPayScaleCreateDto dto)
    {
        var entity = dto.Adapt<TaktPayScale>();
        var isUnique_ix_pay_scale_code_unique = await _uniqueValidator.IsUniqueAsync(
            _payScaleRepository,
            x => x.ScaleCode == entity.ScaleCode);
        if (!isUnique_ix_pay_scale_code_unique)
        {
            throw new TaktBusinessException("薪级的ScaleCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _payScaleRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _payScaleRepository.CreateAsync(entity);
        return await GetPayScaleByIdAsync(entity.Id) ?? entity.Adapt<TaktPayScaleDto>();
    }

    /// <summary>
    /// 更新薪级
    /// </summary>
    /// <param name="id">薪级ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPayScaleDto> UpdatePayScaleAsync(long id, TaktPayScaleUpdateDto dto)
    {
        var entity = await _payScaleRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("薪级不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_pay_scale_code_unique = await _uniqueValidator.IsUniqueAsync(
            _payScaleRepository,
            x => x.ScaleCode == entity.ScaleCode,
            id);
        if (!isUnique_ix_pay_scale_code_unique)
        {
            throw new TaktBusinessException("薪级的ScaleCode已存在");
        }
        await _payScaleRepository.UpdateAsync(entity);
        return await GetPayScaleByIdAsync(id) ?? throw new TaktBusinessException("薪级不存在");
    }

    /// <summary>
    /// 删除薪级
    /// </summary>
    /// <param name="id">薪级ID</param>
    /// <returns>任务</returns>
    public async Task DeletePayScaleByIdAsync(long id)
    {
        var deleted = await _payScaleRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("薪级不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除薪级
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePayScaleBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePayScaleByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新薪级状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPayScaleDto> UpdatePayScaleStatusAsync(TaktPayScaleStatusDto dto)
    {
        var entity = await _payScaleRepository.GetByIdAsync(dto.PayScaleId);
        if (entity == null)
        {
            throw new TaktBusinessException("薪级不存在");
        }
        entity.ScaleStatus = dto.ScaleStatus;
        await _payScaleRepository.UpdateAsync(entity);
        return await GetPayScaleByIdAsync(dto.PayScaleId) ?? throw new TaktBusinessException("薪级不存在");
    }

    /// <summary>
    /// 更新薪级排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPayScaleDto> UpdatePayScaleSortAsync(TaktPayScaleSortDto dto)
    {
        var entity = await _payScaleRepository.GetByIdAsync(dto.PayScaleId);
        if (entity == null)
        {
            throw new TaktBusinessException("薪级不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _payScaleRepository.UpdateAsync(entity);
        return await GetPayScaleByIdAsync(dto.PayScaleId) ?? throw new TaktBusinessException("薪级不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPayScaleTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPayScaleTemplateDto>(
            sheetName ?? "薪级导入模板",
            fileName ?? "薪级导入模板.xlsx");
    }

    /// <summary>
    /// 导入薪级
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPayScaleAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPayScaleImportDto>(fileStream, sheetName ?? "薪级导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _payScaleRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktPayScale>();
                var importKey = $"{entity.ScaleCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（ScaleCode）");
                }
                var isUnique_ix_pay_scale_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _payScaleRepository,
                    x => x.ScaleCode == entity.ScaleCode);
                if (!isUnique_ix_pay_scale_code_unique)
                {
                    throw new TaktBusinessException("薪级的ScaleCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _payScaleRepository.CreateAsync(entity);
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
    /// 导出薪级
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPayScaleAsync(TaktPayScaleQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktPayScaleQueryDto());
        var list = await _payScaleRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPayScaleExportDto>(),
                sheetName ?? "薪级数据",
                fileName ?? "薪级导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPayScaleExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "薪级数据",
            fileName ?? "薪级导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建薪级查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPayScale, bool>> QueryExpression(TaktPayScaleQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPayScale>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.ScaleCode != null && x.ScaleCode.Contains(keywords))
                || (x.ScaleName != null && x.ScaleName.Contains(keywords))
                || SqlFunc.ToString(x.GradeLevel).Contains(keywords)
                || SqlFunc.ToString(x.MinSalary).Contains(keywords)
                || SqlFunc.ToString(x.MidSalary).Contains(keywords)
                || SqlFunc.ToString(x.MaxSalary).Contains(keywords)
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || SqlFunc.ToString(x.ScaleStatus).Contains(keywords)
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.ScaleCode))
        {
            exp = exp.And(x => x.ScaleCode != null && x.ScaleCode.Contains(queryDto.ScaleCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ScaleName))
        {
            exp = exp.And(x => x.ScaleName != null && x.ScaleName.Contains(queryDto.ScaleName));
        }

        if (queryDto?.GradeLevel.HasValue == true)
        {
            exp = exp.And(x => x.GradeLevel == queryDto.GradeLevel);
        }

        if (queryDto?.MinSalary.HasValue == true)
        {
            exp = exp.And(x => x.MinSalary == queryDto.MinSalary);
        }

        if (queryDto?.MidSalary.HasValue == true)
        {
            exp = exp.And(x => x.MidSalary == queryDto.MidSalary);
        }

        if (queryDto?.MaxSalary.HasValue == true)
        {
            exp = exp.And(x => x.MaxSalary == queryDto.MaxSalary);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (queryDto?.ScaleStatus.HasValue == true)
        {
            exp = exp.And(x => x.ScaleStatus == queryDto.ScaleStatus);
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
