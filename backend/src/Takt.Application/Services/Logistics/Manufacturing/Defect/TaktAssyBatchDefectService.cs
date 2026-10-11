// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Defect
// 文件名称：TaktAssyBatchDefectService.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：组立批量不良统计应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Defect;
using Takt.Domain.Entities.Logistics.Manufacturing.Defect;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Defect;

/// <summary>
/// 组立批量不良统计应用服务
/// </summary>
public class TaktAssyBatchDefectService : TaktServiceBase, ITaktAssyBatchDefectService
{
    private readonly ITaktCompanyRepository<TaktAssyBatchDefect> _assyBatchDefectRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="assyBatchDefectRepository">组立批量不良统计仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktAssyBatchDefectService(
        ITaktCompanyRepository<TaktAssyBatchDefect> assyBatchDefectRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _assyBatchDefectRepository = assyBatchDefectRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取组立批量不良统计列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktAssyBatchDefectDto>> GetAssyBatchDefectListAsync(TaktAssyBatchDefectQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _assyBatchDefectRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktAssyBatchDefectDto>.Create(
            data.Adapt<List<TaktAssyBatchDefectDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取组立批量不良统计
    /// </summary>
    /// <param name="id">组立批量不良统计ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktAssyBatchDefectDto?> GetAssyBatchDefectByIdAsync(long id)
    {
        var entity = await _assyBatchDefectRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktAssyBatchDefectDto>();
    }

    /// <summary>
    /// 获取组立批量不良统计选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetAssyBatchDefectOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _assyBatchDefectRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.BatchStatus == 1,
            x => x.PlantCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.PlantCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建组立批量不良统计
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAssyBatchDefectDto> CreateAssyBatchDefectAsync(TaktAssyBatchDefectCreateDto dto)
    {
        var entity = dto.Adapt<TaktAssyBatchDefect>();
        var isUnique_ix_takt_logistics_manufacturing_defect_assy_batch_unique = await _uniqueValidator.IsUniqueAsync(
            _assyBatchDefectRepository,
            x => x.ProdCategory == entity.ProdCategory
                && x.BatchCode == entity.BatchCode);
        if (!isUnique_ix_takt_logistics_manufacturing_defect_assy_batch_unique)
        {
            throw new TaktBusinessException("组立批量不良统计的ProdCategory、BatchCode已存在");
        }
        entity = await _assyBatchDefectRepository.CreateAsync(entity);
        return await GetAssyBatchDefectByIdAsync(entity.Id) ?? entity.Adapt<TaktAssyBatchDefectDto>();
    }

    /// <summary>
    /// 更新组立批量不良统计
    /// </summary>
    /// <param name="id">组立批量不良统计ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAssyBatchDefectDto> UpdateAssyBatchDefectAsync(long id, TaktAssyBatchDefectUpdateDto dto)
    {
        var entity = await _assyBatchDefectRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("组立批量不良统计不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_defect_assy_batch_unique = await _uniqueValidator.IsUniqueAsync(
            _assyBatchDefectRepository,
            x => x.ProdCategory == entity.ProdCategory
                && x.BatchCode == entity.BatchCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_defect_assy_batch_unique)
        {
            throw new TaktBusinessException("组立批量不良统计的ProdCategory、BatchCode已存在");
        }
        await _assyBatchDefectRepository.UpdateAsync(entity);
        return await GetAssyBatchDefectByIdAsync(id) ?? throw new TaktBusinessException("组立批量不良统计不存在");
    }

    /// <summary>
    /// 删除组立批量不良统计
    /// </summary>
    /// <param name="id">组立批量不良统计ID</param>
    /// <returns>任务</returns>
    public async Task DeleteAssyBatchDefectByIdAsync(long id)
    {
        var deleted = await _assyBatchDefectRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("组立批量不良统计不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除组立批量不良统计
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteAssyBatchDefectBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteAssyBatchDefectByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新组立批量不良统计状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAssyBatchDefectDto> UpdateAssyBatchDefectStatusAsync(TaktAssyBatchDefectStatusDto dto)
    {
        var entity = await _assyBatchDefectRepository.GetByIdAsync(dto.AssyBatchDefectId);
        if (entity == null)
        {
            throw new TaktBusinessException("组立批量不良统计不存在");
        }
        entity.BatchStatus = dto.BatchStatus;
        await _assyBatchDefectRepository.UpdateAsync(entity);
        return await GetAssyBatchDefectByIdAsync(dto.AssyBatchDefectId) ?? throw new TaktBusinessException("组立批量不良统计不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetAssyBatchDefectTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktAssyBatchDefectTemplateDto>(
            sheetName ?? "组立批量不良统计导入模板",
            fileName ?? "组立批量不良统计导入模板.xlsx");
    }

    /// <summary>
    /// 导入组立批量不良统计
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportAssyBatchDefectAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktAssyBatchDefectImportDto>(fileStream, sheetName ?? "组立批量不良统计导入模板");
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
                var entity = rows[i].Adapt<TaktAssyBatchDefect>();
                var importKey = $"{entity.ProdCategory}|{entity.BatchCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（ProdCategory、BatchCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_defect_assy_batch_unique = await _uniqueValidator.IsUniqueAsync(
                    _assyBatchDefectRepository,
                    x => x.ProdCategory == entity.ProdCategory
                        && x.BatchCode == entity.BatchCode);
                if (!isUnique_ix_takt_logistics_manufacturing_defect_assy_batch_unique)
                {
                    throw new TaktBusinessException("组立批量不良统计的ProdCategory、BatchCode已存在");
                }
                await _assyBatchDefectRepository.CreateAsync(entity);
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
    /// 导出组立批量不良统计
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportAssyBatchDefectAsync(TaktAssyBatchDefectQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktAssyBatchDefectQueryDto());
        var list = await _assyBatchDefectRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktAssyBatchDefectExportDto>(),
                sheetName ?? "组立批量不良统计数据",
                fileName ?? "组立批量不良统计导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktAssyBatchDefectExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "组立批量不良统计数据",
            fileName ?? "组立批量不良统计导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建组立批量不良统计查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktAssyBatchDefect, bool>> QueryExpression(TaktAssyBatchDefectQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktAssyBatchDefect>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ProdCategory != null && x.ProdCategory.Contains(keywords))
                || (x.BatchCode != null && x.BatchCode.Contains(keywords))
                || (x.ProdDateGroup != null && x.ProdDateGroup.Contains(keywords))
                || (x.ProdOrderGroup != null && x.ProdOrderGroup.Contains(keywords))
                || (x.ModelCode != null && x.ModelCode.Contains(keywords))
                || (x.MaterialGroup != null && x.MaterialGroup.Contains(keywords))
                || SqlFunc.ToString(x.BatchOrderQty).Contains(keywords)
                || (x.ProdOrderQtyGroup != null && x.ProdOrderQtyGroup.Contains(keywords))
                || SqlFunc.ToString(x.ProdActualQty).Contains(keywords)
                || SqlFunc.ToString(x.GoodQuantity).Contains(keywords)
                || SqlFunc.ToString(x.DefectQty).Contains(keywords)
                || SqlFunc.ToString(x.DefectRatePercent).Contains(keywords)
                || SqlFunc.ToString(x.YieldRatePercent).Contains(keywords)
                || SqlFunc.ToString(x.ReportCount).Contains(keywords)
                || SqlFunc.ToString(x.BatchStatus).Contains(keywords)
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.LastProdDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProdCategory))
        {
            exp = exp.And(x => x.ProdCategory != null && x.ProdCategory.Contains(queryDto.ProdCategory));
        }

        if (!string.IsNullOrEmpty(queryDto?.BatchCode))
        {
            exp = exp.And(x => x.BatchCode != null && x.BatchCode.Contains(queryDto.BatchCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProdDateGroup))
        {
            exp = exp.And(x => x.ProdDateGroup != null && x.ProdDateGroup.Contains(queryDto.ProdDateGroup));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProdOrderGroup))
        {
            exp = exp.And(x => x.ProdOrderGroup != null && x.ProdOrderGroup.Contains(queryDto.ProdOrderGroup));
        }

        if (!string.IsNullOrEmpty(queryDto?.ModelCode))
        {
            exp = exp.And(x => x.ModelCode != null && x.ModelCode.Contains(queryDto.ModelCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialGroup))
        {
            exp = exp.And(x => x.MaterialGroup != null && x.MaterialGroup.Contains(queryDto.MaterialGroup));
        }

        if (queryDto?.BatchOrderQty.HasValue == true)
        {
            exp = exp.And(x => x.BatchOrderQty == queryDto.BatchOrderQty);
        }

        if (!string.IsNullOrEmpty(queryDto?.ProdOrderQtyGroup))
        {
            exp = exp.And(x => x.ProdOrderQtyGroup != null && x.ProdOrderQtyGroup.Contains(queryDto.ProdOrderQtyGroup));
        }

        if (queryDto?.ProdActualQty.HasValue == true)
        {
            exp = exp.And(x => x.ProdActualQty == queryDto.ProdActualQty);
        }

        if (queryDto?.GoodQuantity.HasValue == true)
        {
            exp = exp.And(x => x.GoodQuantity == queryDto.GoodQuantity);
        }

        if (queryDto?.DefectQty.HasValue == true)
        {
            exp = exp.And(x => x.DefectQty == queryDto.DefectQty);
        }

        if (queryDto?.DefectRatePercent.HasValue == true)
        {
            exp = exp.And(x => x.DefectRatePercent == queryDto.DefectRatePercent);
        }

        if (queryDto?.YieldRatePercent.HasValue == true)
        {
            exp = exp.And(x => x.YieldRatePercent == queryDto.YieldRatePercent);
        }

        if (queryDto?.ReportCount.HasValue == true)
        {
            exp = exp.And(x => x.ReportCount == queryDto.ReportCount);
        }

        if (queryDto?.BatchStatus.HasValue == true)
        {
            exp = exp.And(x => x.BatchStatus == queryDto.BatchStatus);
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

        if (queryDto?.LastProdDateStart.HasValue == true)
        {
            exp = exp.And(x => x.LastProdDate >= queryDto.LastProdDateStart);
        }

        if (queryDto?.LastProdDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.LastProdDate <= queryDto.LastProdDateEnd);
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
