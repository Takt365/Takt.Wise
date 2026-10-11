// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Mps
// 文件名称：TaktProductionEquipmentService.cs
// 创建时间：2026-07-24
// 创建人：Takt365(Cursor AI)
// 功能描述：生产设备应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Mps;
using Takt.Domain.Entities.Logistics.Manufacturing.Mps;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Mps;

/// <summary>
/// 生产设备应用服务
/// </summary>
public class TaktProductionEquipmentService : TaktServiceBase, ITaktProductionEquipmentService
{
    private readonly ITaktCompanyRepository<TaktProductionEquipment> _productionEquipmentRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="productionEquipmentRepository">生产设备仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktProductionEquipmentService(
        ITaktCompanyRepository<TaktProductionEquipment> productionEquipmentRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _productionEquipmentRepository = productionEquipmentRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取生产设备列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktProductionEquipmentDto>> GetProductionEquipmentListAsync(TaktProductionEquipmentQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _productionEquipmentRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktProductionEquipmentDto>.Create(
            data.Adapt<List<TaktProductionEquipmentDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取生产设备
    /// </summary>
    /// <param name="id">生产设备ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductionEquipmentDto?> GetProductionEquipmentByIdAsync(long id)
    {
        var entity = await _productionEquipmentRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktProductionEquipmentDto>();
    }

    /// <summary>
    /// 获取生产设备选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetProductionEquipmentOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _productionEquipmentRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.EquipmentRunStatus == 1,
            x => x.ProdEquipName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.ProdEquipCode,
            DictLabel = e.ProdEquipName ?? e.ProdEquipCode,
        }).ToList();
    }

    /// <summary>
    /// 创建生产设备
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductionEquipmentDto> CreateProductionEquipmentAsync(TaktProductionEquipmentCreateDto dto)
    {
        var entity = dto.Adapt<TaktProductionEquipment>();
        var isUnique_ix_takt_logistics_manufacturing_mps_production_equipment_unique = await _uniqueValidator.IsUniqueAsync(
            _productionEquipmentRepository,
            x => x.PlantCode == entity.PlantCode
                && x.StorageLocation == entity.StorageLocation
                && x.ProdEquipCode == entity.ProdEquipCode);
        if (!isUnique_ix_takt_logistics_manufacturing_mps_production_equipment_unique)
        {
            throw new TaktBusinessException("生产设备的PlantCode、StorageLocation、ProdEquipCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _productionEquipmentRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _productionEquipmentRepository.CreateAsync(entity);
        return await GetProductionEquipmentByIdAsync(entity.Id) ?? entity.Adapt<TaktProductionEquipmentDto>();
    }

    /// <summary>
    /// 更新生产设备
    /// </summary>
    /// <param name="id">生产设备ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductionEquipmentDto> UpdateProductionEquipmentAsync(long id, TaktProductionEquipmentUpdateDto dto)
    {
        var entity = await _productionEquipmentRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("生产设备不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_mps_production_equipment_unique = await _uniqueValidator.IsUniqueAsync(
            _productionEquipmentRepository,
            x => x.PlantCode == entity.PlantCode
                && x.StorageLocation == entity.StorageLocation
                && x.ProdEquipCode == entity.ProdEquipCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_mps_production_equipment_unique)
        {
            throw new TaktBusinessException("生产设备的PlantCode、StorageLocation、ProdEquipCode已存在");
        }
        await _productionEquipmentRepository.UpdateAsync(entity);
        return await GetProductionEquipmentByIdAsync(id) ?? throw new TaktBusinessException("生产设备不存在");
    }

    /// <summary>
    /// 删除生产设备
    /// </summary>
    /// <param name="id">生产设备ID</param>
    /// <returns>任务</returns>
    public async Task DeleteProductionEquipmentByIdAsync(long id)
    {
        var deleted = await _productionEquipmentRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("生产设备不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除生产设备
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteProductionEquipmentBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteProductionEquipmentByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新生产设备状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductionEquipmentDto> UpdateProductionEquipmentStatusAsync(TaktProductionEquipmentStatusDto dto)
    {
        var entity = await _productionEquipmentRepository.GetByIdAsync(dto.ProductionEquipmentId);
        if (entity == null)
        {
            throw new TaktBusinessException("生产设备不存在");
        }
        entity.EquipmentRunStatus = dto.EquipmentRunStatus;
        await _productionEquipmentRepository.UpdateAsync(entity);
        return await GetProductionEquipmentByIdAsync(dto.ProductionEquipmentId) ?? throw new TaktBusinessException("生产设备不存在");
    }

    /// <summary>
    /// 更新生产设备排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductionEquipmentDto> UpdateProductionEquipmentSortAsync(TaktProductionEquipmentSortDto dto)
    {
        var entity = await _productionEquipmentRepository.GetByIdAsync(dto.ProductionEquipmentId);
        if (entity == null)
        {
            throw new TaktBusinessException("生产设备不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _productionEquipmentRepository.UpdateAsync(entity);
        return await GetProductionEquipmentByIdAsync(dto.ProductionEquipmentId) ?? throw new TaktBusinessException("生产设备不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetProductionEquipmentTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktProductionEquipmentTemplateDto>(
            sheetName ?? "生产设备导入模板",
            fileName ?? "生产设备导入模板.xlsx");
    }

    /// <summary>
    /// 导入生产设备
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportProductionEquipmentAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktProductionEquipmentImportDto>(fileStream, sheetName ?? "生产设备导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _productionEquipmentRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktProductionEquipment>();
                var importKey = $"{entity.PlantCode}|{entity.StorageLocation}|{entity.ProdEquipCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、StorageLocation、ProdEquipCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_mps_production_equipment_unique = await _uniqueValidator.IsUniqueAsync(
                    _productionEquipmentRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.StorageLocation == entity.StorageLocation
                        && x.ProdEquipCode == entity.ProdEquipCode);
                if (!isUnique_ix_takt_logistics_manufacturing_mps_production_equipment_unique)
                {
                    throw new TaktBusinessException("生产设备的PlantCode、StorageLocation、ProdEquipCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _productionEquipmentRepository.CreateAsync(entity);
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
    /// 导出生产设备
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportProductionEquipmentAsync(TaktProductionEquipmentQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktProductionEquipmentQueryDto());
        var list = await _productionEquipmentRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktProductionEquipmentExportDto>(),
                sheetName ?? "生产设备数据",
                fileName ?? "生产设备导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktProductionEquipmentExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "生产设备数据",
            fileName ?? "生产设备导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建生产设备查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktProductionEquipment, bool>> QueryExpression(TaktProductionEquipmentQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktProductionEquipment>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || SqlFunc.ToString(x.EquipCategory).Contains(keywords)
                || (x.ProdEquipCode != null && x.ProdEquipCode.Contains(keywords))
                || (x.ProdEquipName != null && x.ProdEquipName.Contains(keywords))
                || (x.Manufacturer != null && x.Manufacturer.Contains(keywords))
                || (x.EquipBrand != null && x.EquipBrand.Contains(keywords))
                || (x.MachineType != null && x.MachineType.Contains(keywords))
                || (x.ModelCode != null && x.ModelCode.Contains(keywords))
                || (x.SerialCode != null && x.SerialCode.Contains(keywords))
                || (x.EquipSpecification != null && x.EquipSpecification.Contains(keywords))
                || SqlFunc.ToString(x.StdCycleTimeSeconds).Contains(keywords)
                || SqlFunc.ToString(x.StdMinutesPerUnit).Contains(keywords)
                || SqlFunc.ToString(x.StdMinutesPerCycle).Contains(keywords)
                || SqlFunc.ToString(x.TheoreticalSpm).Contains(keywords)
                || SqlFunc.ToString(x.TheoreticalCycleTimeSeconds).Contains(keywords)
                || SqlFunc.ToString(x.StdEquipHourlyCapacity).Contains(keywords)
                || SqlFunc.ToString(x.AvailabilityRate).Contains(keywords)
                || SqlFunc.ToString(x.PerformanceRate).Contains(keywords)
                || SqlFunc.ToString(x.SetupMinutes).Contains(keywords)
                || SqlFunc.ToString(x.MoldChangeMinutes).Contains(keywords)
                || SqlFunc.ToString(x.MaterialChangeMinutes).Contains(keywords)
                || SqlFunc.ToString(x.MtbfHours).Contains(keywords)
                || SqlFunc.ToString(x.MttrHours).Contains(keywords)
                || SqlFunc.ToString(x.RepeatabilityAccuracy).Contains(keywords)
                || SqlFunc.ToString(x.ShutHeightAccuracy).Contains(keywords)
                || SqlFunc.ToString(x.InjectionAccuracy).Contains(keywords)
                || SqlFunc.ToString(x.TemperatureControlAccuracy).Contains(keywords)
                || SqlFunc.ToString(x.PressureControlAccuracy).Contains(keywords)
                || SqlFunc.ToString(x.ProcessCapabilityCpk).Contains(keywords)
                || SqlFunc.ToString(x.MaxDimensionalTolerance).Contains(keywords)
                || (x.MaxMoldDimension != null && x.MaxMoldDimension.Contains(keywords))
                || (x.MinMoldDimension != null && x.MinMoldDimension.Contains(keywords))
                || SqlFunc.ToString(x.MaxMoldWeightTon).Contains(keywords)
                || (x.MoldHeightRange != null && x.MoldHeightRange.Contains(keywords))
                || SqlFunc.ToString(x.EjectionType).Contains(keywords)
                || SqlFunc.ToString(x.EjectionStrokeMm).Contains(keywords)
                || SqlFunc.ToString(x.CavityCount).Contains(keywords)
                || SqlFunc.ToString(x.QuickMoldChange).Contains(keywords)
                || (x.MoldCode != null && x.MoldCode.Contains(keywords))
                || SqlFunc.ToString(x.RatedTonnage).Contains(keywords)
                || SqlFunc.ToString(x.ClampingForceKn).Contains(keywords)
                || SqlFunc.ToString(x.MaxStrokeMm).Contains(keywords)
                || SqlFunc.ToString(x.OpenStrokeMm).Contains(keywords)
                || (x.PlatenSize != null && x.PlatenSize.Contains(keywords))
                || SqlFunc.ToString(x.RatedVoltage).Contains(keywords)
                || SqlFunc.ToString(x.RatedPowerKw).Contains(keywords)
                || SqlFunc.ToString(x.AirConsumptionLpm).Contains(keywords)
                || SqlFunc.ToString(x.CoolingWaterFlowLpm).Contains(keywords)
                || SqlFunc.ToString(x.OperatorCount).Contains(keywords)
                || SqlFunc.ToString(x.IsCriticalResource).Contains(keywords)
                || SqlFunc.ToString(x.ParallelCapacity).Contains(keywords)
                || SqlFunc.ToString(x.AllowRushOrder).Contains(keywords)
                || SqlFunc.ToString(x.WarmupMinutes).Contains(keywords)
                || (x.OperatingTempRange != null && x.OperatingTempRange.Contains(keywords))
                || (x.OperatingHumidityRange != null && x.OperatingHumidityRange.Contains(keywords))
                || SqlFunc.ToString(x.NoiseLevelDb).Contains(keywords)
                || SqlFunc.ToString(x.EquipmentRunStatus).Contains(keywords)
                || SqlFunc.ToString(x.MaintenanceIntervalHours).Contains(keywords)
                || SqlFunc.ToString(x.CumulativeRunHours).Contains(keywords)
                || (x.InterfaceType != null && x.InterfaceType.Contains(keywords))
                || (x.StorageLocation != null && x.StorageLocation.Contains(keywords))
                || (x.EquipAdministratorName != null && x.EquipAdministratorName.Contains(keywords))
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || SqlFunc.ToString(x.ProdEquipStatus).Contains(keywords)
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.ManufacturingDate).Contains(keywords)
                || SqlFunc.ToString(x.CommissioningDate).Contains(keywords)
                || SqlFunc.ToString(x.DecommissioningDate).Contains(keywords)
                || SqlFunc.ToString(x.ScrapDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (queryDto?.EquipCategory.HasValue == true)
        {
            exp = exp.And(x => x.EquipCategory == queryDto.EquipCategory);
        }

        if (!string.IsNullOrEmpty(queryDto?.ProdEquipCode))
        {
            exp = exp.And(x => x.ProdEquipCode != null && x.ProdEquipCode.Contains(queryDto.ProdEquipCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProdEquipName))
        {
            exp = exp.And(x => x.ProdEquipName != null && x.ProdEquipName.Contains(queryDto.ProdEquipName));
        }

        if (!string.IsNullOrEmpty(queryDto?.Manufacturer))
        {
            exp = exp.And(x => x.Manufacturer != null && x.Manufacturer.Contains(queryDto.Manufacturer));
        }

        if (!string.IsNullOrEmpty(queryDto?.EquipBrand))
        {
            exp = exp.And(x => x.EquipBrand != null && x.EquipBrand.Contains(queryDto.EquipBrand));
        }

        if (!string.IsNullOrEmpty(queryDto?.MachineType))
        {
            exp = exp.And(x => x.MachineType != null && x.MachineType.Contains(queryDto.MachineType));
        }

        if (!string.IsNullOrEmpty(queryDto?.ModelCode))
        {
            exp = exp.And(x => x.ModelCode != null && x.ModelCode.Contains(queryDto.ModelCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.SerialCode))
        {
            exp = exp.And(x => x.SerialCode != null && x.SerialCode.Contains(queryDto.SerialCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.EquipSpecification))
        {
            exp = exp.And(x => x.EquipSpecification != null && x.EquipSpecification.Contains(queryDto.EquipSpecification));
        }

        if (queryDto?.StdCycleTimeSeconds.HasValue == true)
        {
            exp = exp.And(x => x.StdCycleTimeSeconds == queryDto.StdCycleTimeSeconds);
        }

        if (queryDto?.StdMinutesPerUnit.HasValue == true)
        {
            exp = exp.And(x => x.StdMinutesPerUnit == queryDto.StdMinutesPerUnit);
        }

        if (queryDto?.StdMinutesPerCycle.HasValue == true)
        {
            exp = exp.And(x => x.StdMinutesPerCycle == queryDto.StdMinutesPerCycle);
        }

        if (queryDto?.TheoreticalSpm.HasValue == true)
        {
            exp = exp.And(x => x.TheoreticalSpm == queryDto.TheoreticalSpm);
        }

        if (queryDto?.TheoreticalCycleTimeSeconds.HasValue == true)
        {
            exp = exp.And(x => x.TheoreticalCycleTimeSeconds == queryDto.TheoreticalCycleTimeSeconds);
        }

        if (queryDto?.StdEquipHourlyCapacity.HasValue == true)
        {
            exp = exp.And(x => x.StdEquipHourlyCapacity == queryDto.StdEquipHourlyCapacity);
        }

        if (queryDto?.AvailabilityRate.HasValue == true)
        {
            exp = exp.And(x => x.AvailabilityRate == queryDto.AvailabilityRate);
        }

        if (queryDto?.PerformanceRate.HasValue == true)
        {
            exp = exp.And(x => x.PerformanceRate == queryDto.PerformanceRate);
        }

        if (queryDto?.SetupMinutes.HasValue == true)
        {
            exp = exp.And(x => x.SetupMinutes == queryDto.SetupMinutes);
        }

        if (queryDto?.MoldChangeMinutes.HasValue == true)
        {
            exp = exp.And(x => x.MoldChangeMinutes == queryDto.MoldChangeMinutes);
        }

        if (queryDto?.MaterialChangeMinutes.HasValue == true)
        {
            exp = exp.And(x => x.MaterialChangeMinutes == queryDto.MaterialChangeMinutes);
        }

        if (queryDto?.MtbfHours.HasValue == true)
        {
            exp = exp.And(x => x.MtbfHours == queryDto.MtbfHours);
        }

        if (queryDto?.MttrHours.HasValue == true)
        {
            exp = exp.And(x => x.MttrHours == queryDto.MttrHours);
        }

        if (queryDto?.RepeatabilityAccuracy.HasValue == true)
        {
            exp = exp.And(x => x.RepeatabilityAccuracy == queryDto.RepeatabilityAccuracy);
        }

        if (queryDto?.ShutHeightAccuracy.HasValue == true)
        {
            exp = exp.And(x => x.ShutHeightAccuracy == queryDto.ShutHeightAccuracy);
        }

        if (queryDto?.InjectionAccuracy.HasValue == true)
        {
            exp = exp.And(x => x.InjectionAccuracy == queryDto.InjectionAccuracy);
        }

        if (queryDto?.TemperatureControlAccuracy.HasValue == true)
        {
            exp = exp.And(x => x.TemperatureControlAccuracy == queryDto.TemperatureControlAccuracy);
        }

        if (queryDto?.PressureControlAccuracy.HasValue == true)
        {
            exp = exp.And(x => x.PressureControlAccuracy == queryDto.PressureControlAccuracy);
        }

        if (queryDto?.ProcessCapabilityCpk.HasValue == true)
        {
            exp = exp.And(x => x.ProcessCapabilityCpk == queryDto.ProcessCapabilityCpk);
        }

        if (queryDto?.MaxDimensionalTolerance.HasValue == true)
        {
            exp = exp.And(x => x.MaxDimensionalTolerance == queryDto.MaxDimensionalTolerance);
        }

        if (!string.IsNullOrEmpty(queryDto?.MaxMoldDimension))
        {
            exp = exp.And(x => x.MaxMoldDimension != null && x.MaxMoldDimension.Contains(queryDto.MaxMoldDimension));
        }

        if (!string.IsNullOrEmpty(queryDto?.MinMoldDimension))
        {
            exp = exp.And(x => x.MinMoldDimension != null && x.MinMoldDimension.Contains(queryDto.MinMoldDimension));
        }

        if (queryDto?.MaxMoldWeightTon.HasValue == true)
        {
            exp = exp.And(x => x.MaxMoldWeightTon == queryDto.MaxMoldWeightTon);
        }

        if (!string.IsNullOrEmpty(queryDto?.MoldHeightRange))
        {
            exp = exp.And(x => x.MoldHeightRange != null && x.MoldHeightRange.Contains(queryDto.MoldHeightRange));
        }

        if (queryDto?.EjectionType.HasValue == true)
        {
            exp = exp.And(x => x.EjectionType == queryDto.EjectionType);
        }

        if (queryDto?.EjectionStrokeMm.HasValue == true)
        {
            exp = exp.And(x => x.EjectionStrokeMm == queryDto.EjectionStrokeMm);
        }

        if (queryDto?.CavityCount.HasValue == true)
        {
            exp = exp.And(x => x.CavityCount == queryDto.CavityCount);
        }

        if (queryDto?.QuickMoldChange.HasValue == true)
        {
            exp = exp.And(x => x.QuickMoldChange == queryDto.QuickMoldChange);
        }

        if (!string.IsNullOrEmpty(queryDto?.MoldCode))
        {
            exp = exp.And(x => x.MoldCode != null && x.MoldCode.Contains(queryDto.MoldCode));
        }

        if (queryDto?.RatedTonnage.HasValue == true)
        {
            exp = exp.And(x => x.RatedTonnage == queryDto.RatedTonnage);
        }

        if (queryDto?.ClampingForceKn.HasValue == true)
        {
            exp = exp.And(x => x.ClampingForceKn == queryDto.ClampingForceKn);
        }

        if (queryDto?.MaxStrokeMm.HasValue == true)
        {
            exp = exp.And(x => x.MaxStrokeMm == queryDto.MaxStrokeMm);
        }

        if (queryDto?.OpenStrokeMm.HasValue == true)
        {
            exp = exp.And(x => x.OpenStrokeMm == queryDto.OpenStrokeMm);
        }

        if (!string.IsNullOrEmpty(queryDto?.PlatenSize))
        {
            exp = exp.And(x => x.PlatenSize != null && x.PlatenSize.Contains(queryDto.PlatenSize));
        }

        if (queryDto?.RatedVoltage.HasValue == true)
        {
            exp = exp.And(x => x.RatedVoltage == queryDto.RatedVoltage);
        }

        if (queryDto?.RatedPowerKw.HasValue == true)
        {
            exp = exp.And(x => x.RatedPowerKw == queryDto.RatedPowerKw);
        }

        if (queryDto?.AirConsumptionLpm.HasValue == true)
        {
            exp = exp.And(x => x.AirConsumptionLpm == queryDto.AirConsumptionLpm);
        }

        if (queryDto?.CoolingWaterFlowLpm.HasValue == true)
        {
            exp = exp.And(x => x.CoolingWaterFlowLpm == queryDto.CoolingWaterFlowLpm);
        }

        if (queryDto?.OperatorCount.HasValue == true)
        {
            exp = exp.And(x => x.OperatorCount == queryDto.OperatorCount);
        }

        if (queryDto?.IsCriticalResource.HasValue == true)
        {
            exp = exp.And(x => x.IsCriticalResource == queryDto.IsCriticalResource);
        }

        if (queryDto?.ParallelCapacity.HasValue == true)
        {
            exp = exp.And(x => x.ParallelCapacity == queryDto.ParallelCapacity);
        }

        if (queryDto?.AllowRushOrder.HasValue == true)
        {
            exp = exp.And(x => x.AllowRushOrder == queryDto.AllowRushOrder);
        }

        if (queryDto?.WarmupMinutes.HasValue == true)
        {
            exp = exp.And(x => x.WarmupMinutes == queryDto.WarmupMinutes);
        }

        if (!string.IsNullOrEmpty(queryDto?.OperatingTempRange))
        {
            exp = exp.And(x => x.OperatingTempRange != null && x.OperatingTempRange.Contains(queryDto.OperatingTempRange));
        }

        if (!string.IsNullOrEmpty(queryDto?.OperatingHumidityRange))
        {
            exp = exp.And(x => x.OperatingHumidityRange != null && x.OperatingHumidityRange.Contains(queryDto.OperatingHumidityRange));
        }

        if (queryDto?.NoiseLevelDb.HasValue == true)
        {
            exp = exp.And(x => x.NoiseLevelDb == queryDto.NoiseLevelDb);
        }

        if (queryDto?.EquipmentRunStatus.HasValue == true)
        {
            exp = exp.And(x => x.EquipmentRunStatus == queryDto.EquipmentRunStatus);
        }

        if (queryDto?.MaintenanceIntervalHours.HasValue == true)
        {
            exp = exp.And(x => x.MaintenanceIntervalHours == queryDto.MaintenanceIntervalHours);
        }

        if (queryDto?.CumulativeRunHours.HasValue == true)
        {
            exp = exp.And(x => x.CumulativeRunHours == queryDto.CumulativeRunHours);
        }

        if (!string.IsNullOrEmpty(queryDto?.InterfaceType))
        {
            exp = exp.And(x => x.InterfaceType != null && x.InterfaceType.Contains(queryDto.InterfaceType));
        }

        if (!string.IsNullOrEmpty(queryDto?.StorageLocation))
        {
            exp = exp.And(x => x.StorageLocation != null && x.StorageLocation.Contains(queryDto.StorageLocation));
        }

        if (!string.IsNullOrEmpty(queryDto?.EquipAdministratorName))
        {
            exp = exp.And(x => x.EquipAdministratorName != null && x.EquipAdministratorName.Contains(queryDto.EquipAdministratorName));
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (queryDto?.ProdEquipStatus.HasValue == true)
        {
            exp = exp.And(x => x.ProdEquipStatus == queryDto.ProdEquipStatus);
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

        if (queryDto?.ManufacturingDateStart.HasValue == true)
        {
            exp = exp.And(x => x.ManufacturingDate >= queryDto.ManufacturingDateStart);
        }

        if (queryDto?.ManufacturingDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.ManufacturingDate <= queryDto.ManufacturingDateEnd);
        }

        if (queryDto?.CommissioningDateStart.HasValue == true)
        {
            exp = exp.And(x => x.CommissioningDate >= queryDto.CommissioningDateStart);
        }

        if (queryDto?.CommissioningDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.CommissioningDate <= queryDto.CommissioningDateEnd);
        }

        if (queryDto?.DecommissioningDateStart.HasValue == true)
        {
            exp = exp.And(x => x.DecommissioningDate >= queryDto.DecommissioningDateStart);
        }

        if (queryDto?.DecommissioningDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.DecommissioningDate <= queryDto.DecommissioningDateEnd);
        }

        if (queryDto?.ScrapDateStart.HasValue == true)
        {
            exp = exp.And(x => x.ScrapDate >= queryDto.ScrapDateStart);
        }

        if (queryDto?.ScrapDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.ScrapDate <= queryDto.ScrapDateEnd);
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
