// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Output
// 文件名称：TaktProductionChangeoverService.cs
// 创建时间：2026-07-06
// 创建人：Takt365(Cursor AI)
// 功能描述：生产切换记录应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Globalization;
using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Output;
using Takt.Domain.Entities.Logistics.Manufacturing.Output;
using Takt.Domain.Entities.Logistics.Manufacturing.Aps;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Output;

/// <summary>
/// 生产切换记录应用服务
/// </summary>
public class TaktProductionChangeoverService : TaktServiceBase, ITaktProductionChangeoverService
{
    private readonly ITaktCompanyRepository<TaktProductionChangeover> _productionChangeoverRepository;
    private readonly ITaktCompanyRepository<TaktProductionOrder> _productionOrderRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="productionChangeoverRepository">生产切换记录仓储</param>
    /// <param name="productionOrderRepository">生产工单仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktProductionChangeoverService(
        ITaktCompanyRepository<TaktProductionChangeover> productionChangeoverRepository,
        ITaktCompanyRepository<TaktProductionOrder> productionOrderRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _productionChangeoverRepository = productionChangeoverRepository;
        _productionOrderRepository = productionOrderRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取生产切换记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktProductionChangeoverDto>> GetProductionChangeoverListAsync(TaktProductionChangeoverQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _productionChangeoverRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktProductionChangeoverDto>.Create(
            data.Adapt<List<TaktProductionChangeoverDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取生产切换记录
    /// </summary>
    /// <param name="id">生产切换记录ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductionChangeoverDto?> GetProductionChangeoverByIdAsync(long id)
    {
        var entity = await _productionChangeoverRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktProductionChangeoverDto>();
    }

    /// <summary>
    /// 获取生产切换记录选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetProductionChangeoverOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _productionChangeoverRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.PlantCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.PlantCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建生产切换记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductionChangeoverDto> CreateProductionChangeoverAsync(TaktProductionChangeoverCreateDto dto)
    {
        var entity = dto.Adapt<TaktProductionChangeover>();
        EnsureManufacturingOutputProdDateEditable(entity.ProdDate);
        EnsureThreeLayerContext();
        await ApplyPlantCodeFromCurrentProdOrderAsync(entity);
        var isUnique_ix_takt_logistics_manufacturing_output_production_changeover_unique = await _uniqueValidator.IsUniqueAsync(
            _productionChangeoverRepository,
            x => x.ProdCategory == entity.ProdCategory
                && x.ChangeoverCategory == entity.ChangeoverCategory
                && x.ProdDate == entity.ProdDate.Date
                && x.TeamCode == entity.TeamCode
                && x.CurrentProdOrderCode == entity.CurrentProdOrderCode
                && x.ChangeoverProdOrderCode == entity.ChangeoverProdOrderCode);
        if (!isUnique_ix_takt_logistics_manufacturing_output_production_changeover_unique)
        {
            throw new TaktBusinessException("生产切换记录的ProdCategory、ChangeoverCategory、ProdDate、TeamCode、CurrentProdOrderCode、ChangeoverProdOrderCode已存在");
        }
        entity = await _productionChangeoverRepository.CreateAsync(entity);
        return await GetProductionChangeoverByIdAsync(entity.Id) ?? entity.Adapt<TaktProductionChangeoverDto>();
    }

    /// <summary>
    /// 更新生产切换记录
    /// </summary>
    /// <param name="id">生产切换记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductionChangeoverDto> UpdateProductionChangeoverAsync(long id, TaktProductionChangeoverUpdateDto dto)
    {
        var entity = await _productionChangeoverRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("生产切换记录不存在");
        }
        EnsureManufacturingOutputProdDateEditable(entity.ProdDate);
        dto.Adapt(entity);
        EnsureManufacturingOutputProdDateEditable(entity.ProdDate);
        EnsureThreeLayerContext();
        await ApplyPlantCodeFromCurrentProdOrderAsync(entity);
        var isUnique_ix_takt_logistics_manufacturing_output_production_changeover_unique = await _uniqueValidator.IsUniqueAsync(
            _productionChangeoverRepository,
            x => x.ProdCategory == entity.ProdCategory
                && x.ChangeoverCategory == entity.ChangeoverCategory
                && x.ProdDate == entity.ProdDate.Date
                && x.TeamCode == entity.TeamCode
                && x.CurrentProdOrderCode == entity.CurrentProdOrderCode
                && x.ChangeoverProdOrderCode == entity.ChangeoverProdOrderCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_output_production_changeover_unique)
        {
            throw new TaktBusinessException("生产切换记录的ProdCategory、ChangeoverCategory、ProdDate、TeamCode、CurrentProdOrderCode、ChangeoverProdOrderCode已存在");
        }
        await _productionChangeoverRepository.UpdateAsync(entity);
        return await GetProductionChangeoverByIdAsync(id) ?? throw new TaktBusinessException("生产切换记录不存在");
    }

    /// <summary>
    /// 删除生产切换记录
    /// </summary>
    /// <param name="id">生产切换记录ID</param>
    /// <returns>任务</returns>
    public async Task DeleteProductionChangeoverByIdAsync(long id)
    {
        var entity = await _productionChangeoverRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("生产切换记录不存在或已删除");
        }
        EnsureManufacturingOutputProdDateEditable(entity.ProdDate);
        var deleted = await _productionChangeoverRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("生产切换记录不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除生产切换记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteProductionChangeoverBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteProductionChangeoverByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetProductionChangeoverTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktProductionChangeoverTemplateDto>(
            sheetName ?? "生产切换记录导入模板",
            fileName ?? "生产切换记录导入模板.xlsx");
    }

    /// <summary>
    /// 导入生产切换记录
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportProductionChangeoverAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktProductionChangeoverImportDto>(fileStream, sheetName ?? "生产切换记录导入模板");
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
                var entity = rows[i].Adapt<TaktProductionChangeover>();
                EnsureManufacturingOutputProdDateEditable(entity.ProdDate);
                EnsureThreeLayerContext();
                await ApplyPlantCodeFromCurrentProdOrderAsync(entity);
                var importKey = $"{entity.ProdCategory}|{entity.ChangeoverCategory}|{entity.ProdDate.Date:yyyy-MM-dd}|{entity.TeamCode}|{entity.CurrentProdOrderCode}|{entity.ChangeoverProdOrderCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（ProdCategory、ChangeoverCategory、ProdDate、TeamCode、CurrentProdOrderCode、ChangeoverProdOrderCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_output_production_changeover_unique = await _uniqueValidator.IsUniqueAsync(
                    _productionChangeoverRepository,
                    x => x.ProdCategory == entity.ProdCategory
                        && x.ChangeoverCategory == entity.ChangeoverCategory
                        && x.ProdDate == entity.ProdDate.Date
                        && x.TeamCode == entity.TeamCode
                        && x.CurrentProdOrderCode == entity.CurrentProdOrderCode
                        && x.ChangeoverProdOrderCode == entity.ChangeoverProdOrderCode);
                if (!isUnique_ix_takt_logistics_manufacturing_output_production_changeover_unique)
                {
                    throw new TaktBusinessException("生产切换记录的ProdCategory、ChangeoverCategory、ProdDate、TeamCode、CurrentProdOrderCode、ChangeoverProdOrderCode已存在");
                }
                await _productionChangeoverRepository.CreateAsync(entity);
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
    /// 导出生产切换记录
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportProductionChangeoverAsync(TaktProductionChangeoverQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktProductionChangeoverQueryDto());
        var list = await _productionChangeoverRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktProductionChangeoverExportDto>(),
                sheetName ?? "生产切换记录数据",
                fileName ?? "生产切换记录导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktProductionChangeoverExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "生产切换记录数据",
            fileName ?? "生产切换记录导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 按当前工单号回填生产工厂
    /// </summary>
    /// <param name="entity">生产切换记录</param>
    /// <returns>任务</returns>
    private async Task ApplyPlantCodeFromCurrentProdOrderAsync(TaktProductionChangeover entity)
    {
        await TaktProductionOrderBackfillHelper.ApplyPlantCodeAsync(
            _productionOrderRepository,
            CurrentTenantCode,
            CurrentCompanyCode,
            entity.CurrentProdOrderCode,
            v => entity.PlantCode = v);
    }

    /// <summary>
    /// 校验生产日期是否可编辑（锁定与可选范围）
    /// </summary>
    /// <param name="prodDate">生产日期</param>
    private void EnsureManufacturingOutputProdDateEditable(DateTime prodDate)
    {
        if (TaktAssyOutputProdDateEditLockHelper.IsProdDateLocked(prodDate, DateTime.Today))
        {
            ThrowBusinessExceptionLocalized(
                TaktValidationI18nKeys.AssyOutputProdDateLocked,
                prodDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                TaktAssyOutputProdDateEditLockHelper.DefaultCutoffDayOfNextMonth);
        }
        if (!TaktAssyOutputProdDateEditLockHelper.IsProdDateSelectable(prodDate, DateTime.Today))
        {
            ThrowBusinessExceptionLocalized(
                TaktValidationI18nKeys.AssyOutputProdDateOutOfRange,
                TaktAssyOutputProdDateEditLockHelper.DefaultCutoffDayOfNextMonth);
        }
    }

    /// <summary>
    /// 构建生产切换记录查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktProductionChangeover, bool>> QueryExpression(TaktProductionChangeoverQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktProductionChangeover>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ProdCategory != null && x.ProdCategory.Contains(keywords))
                || (x.ChangeoverCategory != null && x.ChangeoverCategory.Contains(keywords))
                || (x.TeamCode != null && x.TeamCode.Contains(keywords))
                || (x.CurrentProdOrderCode != null && x.CurrentProdOrderCode.Contains(keywords))
                || (x.CurrentModelCode != null && x.CurrentModelCode.Contains(keywords))
                || (x.ChangeoverProdOrderCode != null && x.ChangeoverProdOrderCode.Contains(keywords))
                || (x.ChangeoverModelCode != null && x.ChangeoverModelCode.Contains(keywords))
                || SqlFunc.ToString(x.ChangeoverCount).Contains(keywords)
                || SqlFunc.ToString(x.ChangeoverTime).Contains(keywords)
                || SqlFunc.ToString(x.InstrumentSetupTime).Contains(keywords)
                || SqlFunc.ToString(x.TotalChangeoverTime).Contains(keywords)
                || SqlFunc.ToString(x.ReadSopTime).Contains(keywords)
                || SqlFunc.ToString(x.LearningTime).Contains(keywords)
                || SqlFunc.ToString(x.PersonCount).Contains(keywords)
                || SqlFunc.ToString(x.TotalLearningTime).Contains(keywords)
                || SqlFunc.ToString(x.TotalSopTime).Contains(keywords)
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.ProdDate).Contains(keywords)
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

        if (!string.IsNullOrEmpty(queryDto?.ChangeoverCategory))
        {
            exp = exp.And(x => x.ChangeoverCategory != null && x.ChangeoverCategory.Contains(queryDto.ChangeoverCategory));
        }

        if (!string.IsNullOrEmpty(queryDto?.TeamCode))
        {
            exp = exp.And(x => x.TeamCode != null && x.TeamCode.Contains(queryDto.TeamCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.CurrentProdOrderCode))
        {
            exp = exp.And(x => x.CurrentProdOrderCode != null && x.CurrentProdOrderCode.Contains(queryDto.CurrentProdOrderCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.CurrentModelCode))
        {
            exp = exp.And(x => x.CurrentModelCode != null && x.CurrentModelCode.Contains(queryDto.CurrentModelCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ChangeoverProdOrderCode))
        {
            exp = exp.And(x => x.ChangeoverProdOrderCode != null && x.ChangeoverProdOrderCode.Contains(queryDto.ChangeoverProdOrderCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ChangeoverModelCode))
        {
            exp = exp.And(x => x.ChangeoverModelCode != null && x.ChangeoverModelCode.Contains(queryDto.ChangeoverModelCode));
        }

        if (queryDto?.ChangeoverCount.HasValue == true)
        {
            exp = exp.And(x => x.ChangeoverCount == queryDto.ChangeoverCount);
        }

        if (queryDto?.ChangeoverTime.HasValue == true)
        {
            exp = exp.And(x => x.ChangeoverTime == queryDto.ChangeoverTime);
        }

        if (queryDto?.InstrumentSetupTime.HasValue == true)
        {
            exp = exp.And(x => x.InstrumentSetupTime == queryDto.InstrumentSetupTime);
        }

        if (queryDto?.TotalChangeoverTime.HasValue == true)
        {
            exp = exp.And(x => x.TotalChangeoverTime == queryDto.TotalChangeoverTime);
        }

        if (queryDto?.ReadSopTime.HasValue == true)
        {
            exp = exp.And(x => x.ReadSopTime == queryDto.ReadSopTime);
        }

        if (queryDto?.LearningTime.HasValue == true)
        {
            exp = exp.And(x => x.LearningTime == queryDto.LearningTime);
        }

        if (queryDto?.PersonCount.HasValue == true)
        {
            exp = exp.And(x => x.PersonCount == queryDto.PersonCount);
        }

        if (queryDto?.TotalLearningTime.HasValue == true)
        {
            exp = exp.And(x => x.TotalLearningTime == queryDto.TotalLearningTime);
        }

        if (queryDto?.TotalSopTime.HasValue == true)
        {
            exp = exp.And(x => x.TotalSopTime == queryDto.TotalSopTime);
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

        if (queryDto?.ProdDateStart.HasValue == true)
        {
            exp = exp.And(x => x.ProdDate >= queryDto.ProdDateStart);
        }

        if (queryDto?.ProdDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.ProdDate <= queryDto.ProdDateEnd);
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
