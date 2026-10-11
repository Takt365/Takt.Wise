// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Maintenance
// 文件名称：TaktMaintenanceWorkOrderMaterialService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：维护工单领料应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Maintenance;
using Takt.Domain.Entities.Logistics.Maintenance;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Domain.Entities.Logistics.Materials;

namespace Takt.Application.Services.Logistics.Maintenance;

/// <summary>
/// 维护工单领料应用服务
/// </summary>
public class TaktMaintenanceWorkOrderMaterialService : TaktServiceBase, ITaktMaintenanceWorkOrderMaterialService
{
    private readonly ITaktCompanyRepository<TaktMaintenanceWorkOrderMaterial> _maintenanceWorkOrderMaterialRepository;
    private readonly ITaktApprovalRepository<TaktMaintenanceWorkOrder> _maintenanceWorkOrderRepository;
    private readonly ITaktCompanyRepository<TaktMaterialPlant> _materialPlantRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="maintenanceWorkOrderMaterialRepository">维护工单领料仓储</param>
    /// <param name="maintenanceWorkOrderRepository">维护工单仓储</param>
    /// <param name="materialPlantRepository">工厂物料仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMaintenanceWorkOrderMaterialService(
        ITaktCompanyRepository<TaktMaintenanceWorkOrderMaterial> maintenanceWorkOrderMaterialRepository,
        ITaktApprovalRepository<TaktMaintenanceWorkOrder> maintenanceWorkOrderRepository,
        ITaktCompanyRepository<TaktMaterialPlant> materialPlantRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _maintenanceWorkOrderMaterialRepository = maintenanceWorkOrderMaterialRepository;
        _maintenanceWorkOrderRepository = maintenanceWorkOrderRepository;
        _materialPlantRepository = materialPlantRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取维护工单领料列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktMaintenanceWorkOrderMaterialDto>> GetMaintenanceWorkOrderMaterialListAsync(TaktMaintenanceWorkOrderMaterialQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktMaintenanceWorkOrderMaterialDto>.Create(
                new List<TaktMaintenanceWorkOrderMaterialDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _maintenanceWorkOrderMaterialRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktMaintenanceWorkOrderMaterialDto>.Create(
            data.Adapt<List<TaktMaintenanceWorkOrderMaterialDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取维护工单领料
    /// </summary>
    /// <param name="id">维护工单领料ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaintenanceWorkOrderMaterialDto?> GetMaintenanceWorkOrderMaterialByIdAsync(long id)
    {
        var entity = await _maintenanceWorkOrderMaterialRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktMaintenanceWorkOrderMaterialDto>();
    }

    /// <summary>
    /// 获取维护工单领料选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetMaintenanceWorkOrderMaterialOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _maintenanceWorkOrderMaterialRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IssueStatus == 1 && x.IsObsolete == 0,
            x => x.WorkOrderCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.WorkOrderCode,
            DictLabel = e.WorkOrderCode,
        }).ToList();
    }

    /// <summary>
    /// 创建维护工单领料
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaintenanceWorkOrderMaterialDto> CreateMaintenanceWorkOrderMaterialAsync(TaktMaintenanceWorkOrderMaterialCreateDto dto)
    {
        var entity = dto.Adapt<TaktMaintenanceWorkOrderMaterial>();
        entity.IsObsolete = 0;
        await StampMaintenanceWorkOrderMaterialMaintenanceWorkOrderAsync(entity, dto);
        await StampMaintenanceWorkOrderMaterialMaterialPlantAsync(entity, dto);
        var isUnique_ix_takt_logistics_maintenance_work_order_material_order_line_unique = await _uniqueValidator.IsUniqueAsync(
            _maintenanceWorkOrderMaterialRepository,
            x => x.MaintenanceWorkOrderId == entity.MaintenanceWorkOrderId
                && x.LineNumber == entity.LineNumber
                && x.MaterialCode == entity.MaterialCode);
        if (!isUnique_ix_takt_logistics_maintenance_work_order_material_order_line_unique)
        {
            throw new TaktBusinessException("维护工单领料的MaintenanceWorkOrderId、LineNumber、MaterialCode已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _maintenanceWorkOrderMaterialRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.MaintenanceWorkOrderId == entity.MaintenanceWorkOrderId,
                x => x.LineNumber);
            var businessCode = entity.MaintenanceWorkOrderId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _maintenanceWorkOrderMaterialRepository.CreateAsync(entity);
        return await GetMaintenanceWorkOrderMaterialByIdAsync(entity.Id) ?? entity.Adapt<TaktMaintenanceWorkOrderMaterialDto>();
    }

    /// <summary>
    /// 更新维护工单领料
    /// </summary>
    /// <param name="id">维护工单领料ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaintenanceWorkOrderMaterialDto> UpdateMaintenanceWorkOrderMaterialAsync(long id, TaktMaintenanceWorkOrderMaterialUpdateDto dto)
    {
        var entity = await _maintenanceWorkOrderMaterialRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("维护工单领料不存在");
        }
        dto.Adapt(entity);
        await StampMaintenanceWorkOrderMaterialMaintenanceWorkOrderAsync(entity, dto);
        await StampMaintenanceWorkOrderMaterialMaterialPlantAsync(entity, dto);
        var isUnique_ix_takt_logistics_maintenance_work_order_material_order_line_unique = await _uniqueValidator.IsUniqueAsync(
            _maintenanceWorkOrderMaterialRepository,
            x => x.MaintenanceWorkOrderId == entity.MaintenanceWorkOrderId
                && x.LineNumber == entity.LineNumber
                && x.MaterialCode == entity.MaterialCode,
            id);
        if (!isUnique_ix_takt_logistics_maintenance_work_order_material_order_line_unique)
        {
            throw new TaktBusinessException("维护工单领料的MaintenanceWorkOrderId、LineNumber、MaterialCode已存在");
        }
        await _maintenanceWorkOrderMaterialRepository.UpdateAsync(entity);
        return await GetMaintenanceWorkOrderMaterialByIdAsync(id) ?? throw new TaktBusinessException("维护工单领料不存在");
    }

    /// <summary>
    /// 删除维护工单领料
    /// </summary>
    /// <param name="id">维护工单领料ID</param>
    /// <returns>任务</returns>
    public async Task DeleteMaintenanceWorkOrderMaterialByIdAsync(long id)
    {
        var entity = await _maintenanceWorkOrderMaterialRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("维护工单领料不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("维护工单领料不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("维护工单领料已作废");
        }
        entity.IsObsolete = 1;
        await _maintenanceWorkOrderMaterialRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除维护工单领料
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteMaintenanceWorkOrderMaterialBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteMaintenanceWorkOrderMaterialByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新维护工单领料状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaintenanceWorkOrderMaterialDto> UpdateMaintenanceWorkOrderMaterialStatusAsync(TaktMaintenanceWorkOrderMaterialStatusDto dto)
    {
        var entity = await _maintenanceWorkOrderMaterialRepository.GetByIdAsync(dto.MaintenanceWorkOrderMaterialId);
        if (entity == null)
        {
            throw new TaktBusinessException("维护工单领料不存在");
        }
        entity.IssueStatus = dto.IssueStatus;
        await _maintenanceWorkOrderMaterialRepository.UpdateAsync(entity);
        return await GetMaintenanceWorkOrderMaterialByIdAsync(dto.MaintenanceWorkOrderMaterialId) ?? throw new TaktBusinessException("维护工单领料不存在");
    }

    /// <summary>
    /// 更新维护工单领料作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaintenanceWorkOrderMaterialDto> UpdateMaintenanceWorkOrderMaterialObsoleteAsync(TaktMaintenanceWorkOrderMaterialObsoleteDto dto)
    {
        var entity = await _maintenanceWorkOrderMaterialRepository.GetByIdAsync(dto.MaintenanceWorkOrderMaterialId);
        if (entity == null)
        {
            throw new TaktBusinessException("维护工单领料不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("维护工单领料不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _maintenanceWorkOrderMaterialRepository.UpdateAsync(entity);
        return await GetMaintenanceWorkOrderMaterialByIdAsync(dto.MaintenanceWorkOrderMaterialId) ?? throw new TaktBusinessException("维护工单领料不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetMaintenanceWorkOrderMaterialTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktMaintenanceWorkOrderMaterialTemplateDto>(
            sheetName ?? "维护工单领料导入模板",
            fileName ?? "维护工单领料导入模板.xlsx");
    }

    /// <summary>
    /// 导入维护工单领料
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportMaintenanceWorkOrderMaterialAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktMaintenanceWorkOrderMaterialImportDto>(fileStream, sheetName ?? "维护工单领料导入模板");
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
                var entity = rows[i].Adapt<TaktMaintenanceWorkOrderMaterial>();
                var importDto = rows[i].Adapt<TaktMaintenanceWorkOrderMaterialCreateDto>();
                await StampMaintenanceWorkOrderMaterialMaintenanceWorkOrderAsync(entity, importDto);
                await StampMaintenanceWorkOrderMaterialMaterialPlantAsync(entity, importDto);
                var importKey = $"{entity.MaintenanceWorkOrderId}|{entity.LineNumber}|{entity.MaterialCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（MaintenanceWorkOrderId、LineNumber、MaterialCode）");
                }
                var isUnique_ix_takt_logistics_maintenance_work_order_material_order_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _maintenanceWorkOrderMaterialRepository,
                    x => x.MaintenanceWorkOrderId == entity.MaintenanceWorkOrderId
                        && x.LineNumber == entity.LineNumber
                        && x.MaterialCode == entity.MaterialCode);
                if (!isUnique_ix_takt_logistics_maintenance_work_order_material_order_line_unique)
                {
                    throw new TaktBusinessException("维护工单领料的MaintenanceWorkOrderId、LineNumber、MaterialCode已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _maintenanceWorkOrderMaterialRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.MaintenanceWorkOrderId == entity.MaintenanceWorkOrderId,
                        x => x.LineNumber);
                    var businessCode = entity.MaintenanceWorkOrderId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _maintenanceWorkOrderMaterialRepository.CreateAsync(entity);
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
    /// 导出维护工单领料
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportMaintenanceWorkOrderMaterialAsync(TaktMaintenanceWorkOrderMaterialQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktMaintenanceWorkOrderMaterialQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMaintenanceWorkOrderMaterialExportDto>(),
                sheetName ?? "维护工单领料数据",
                fileName ?? "维护工单领料导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _maintenanceWorkOrderMaterialRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMaintenanceWorkOrderMaterialExportDto>(),
                sheetName ?? "维护工单领料数据",
                fileName ?? "维护工单领料导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktMaintenanceWorkOrderMaterialExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "维护工单领料数据",
            fileName ?? "维护工单领料导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步维护工单领料主表外键（ManyToOne → 维护工单）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampMaintenanceWorkOrderMaterialMaintenanceWorkOrderAsync(TaktMaintenanceWorkOrderMaterial entity, TaktMaintenanceWorkOrderMaterialCreateDto dto)
    {
        if (dto.MaintenanceWorkOrderId <= 0)
        {
            return;
        }
        var master = await _maintenanceWorkOrderRepository.GetByIdAsync(dto.MaintenanceWorkOrderId);
        if (master == null)
        {
            throw new TaktBusinessException("维护工单不存在");
        }
        entity.MaintenanceWorkOrderId = master.Id;
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
        if (string.IsNullOrEmpty(entity.WorkOrderCode))
        {
            entity.WorkOrderCode = master.WorkOrderCode;
        }
    }

    /// <summary>
    /// 同步维护工单领料主表外键（ManyToOne → 工厂物料）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampMaintenanceWorkOrderMaterialMaterialPlantAsync(TaktMaintenanceWorkOrderMaterial entity, TaktMaintenanceWorkOrderMaterialCreateDto dto)
    {
        if (dto.MaterialId <= 0)
        {
            return;
        }
        var master = await _materialPlantRepository.GetByIdAsync(dto.MaterialId);
        if (master == null)
        {
            throw new TaktBusinessException("工厂物料不存在");
        }
        entity.MaterialId = master.Id;
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
        if (string.IsNullOrEmpty(entity.MaterialCode))
        {
            entity.MaterialCode = master.MaterialCode;
        }
        if (string.IsNullOrEmpty(entity.MaterialDescription))
        {
            entity.MaterialDescription = master.MaterialDescription ?? string.Empty;
        }
        if (string.IsNullOrEmpty(entity.StorageLocation))
        {
            entity.StorageLocation = master.StorageLocation;
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建维护工单领料查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktMaintenanceWorkOrderMaterial, bool>> QueryExpression(TaktMaintenanceWorkOrderMaterialQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktMaintenanceWorkOrderMaterial>();

        if (queryDto?.IsObsolete.HasValue == true)
        {
            exp = exp.And(x => x.IsObsolete == queryDto.IsObsolete);
        }
        else
        {
            exp = exp.And(x => x.IsObsolete == 0);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.WorkOrderCode != null && x.WorkOrderCode.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.MaterialDescription != null && x.MaterialDescription.Contains(keywords))
                || (x.MaterialUnit != null && x.MaterialUnit.Contains(keywords))
                || (x.WarehouseCode != null && x.WarehouseCode.Contains(keywords))
                || (x.StorageLocation != null && x.StorageLocation.Contains(keywords))
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

        if (queryDto?.MaintenanceWorkOrderId.HasValue == true)
        {
            var maintenanceWorkOrderId = queryDto.MaintenanceWorkOrderId.Value;
            exp = exp.And(x => x.MaintenanceWorkOrderId == maintenanceWorkOrderId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.WorkOrderCode))
        {
            var workOrderCode = queryDto.WorkOrderCode;
            exp = exp.And(x => x.WorkOrderCode != null && x.WorkOrderCode.Contains(workOrderCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            var lineNumber = queryDto.LineNumber.Value;
            exp = exp.And(x => x.LineNumber == lineNumber);
        }

        if (queryDto?.MaterialId.HasValue == true)
        {
            var materialId = queryDto.MaterialId.Value;
            exp = exp.And(x => x.MaterialId == materialId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialCode))
        {
            var materialCode = queryDto.MaterialCode;
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(materialCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialDescription))
        {
            var materialDescription = queryDto.MaterialDescription;
            exp = exp.And(x => x.MaterialDescription != null && x.MaterialDescription.Contains(materialDescription));
        }

        if (queryDto?.RequiredQuantity.HasValue == true)
        {
            var requiredQuantity = queryDto.RequiredQuantity.Value;
            exp = exp.And(x => x.RequiredQuantity == requiredQuantity);
        }

        if (queryDto?.IssuedQuantity.HasValue == true)
        {
            var issuedQuantity = queryDto.IssuedQuantity.Value;
            exp = exp.And(x => x.IssuedQuantity == issuedQuantity);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialUnit))
        {
            var materialUnit = queryDto.MaterialUnit;
            exp = exp.And(x => x.MaterialUnit != null && x.MaterialUnit.Contains(materialUnit));
        }

        if (queryDto?.UnitPrice.HasValue == true)
        {
            var unitPrice = queryDto.UnitPrice.Value;
            exp = exp.And(x => x.UnitPrice == unitPrice);
        }

        if (queryDto?.Amount.HasValue == true)
        {
            var amount = queryDto.Amount.Value;
            exp = exp.And(x => x.Amount == amount);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.WarehouseCode))
        {
            var warehouseCode = queryDto.WarehouseCode;
            exp = exp.And(x => x.WarehouseCode != null && x.WarehouseCode.Contains(warehouseCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.StorageLocation))
        {
            var storageLocation = queryDto.StorageLocation;
            exp = exp.And(x => x.StorageLocation != null && x.StorageLocation.Contains(storageLocation));
        }

        if (queryDto?.IssueStatus.HasValue == true)
        {
            var issueStatus = queryDto.IssueStatus.Value;
            exp = exp.And(x => x.IssueStatus == issueStatus);
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

        if (queryDto?.IssueTimeStart.HasValue == true)
        {
            var issueTimeStart = queryDto.IssueTimeStart.Value;
            exp = exp.And(x => x.IssueTime >= issueTimeStart);
        }

        if (queryDto?.IssueTimeEnd.HasValue == true)
        {
            var issueTimeEnd = queryDto.IssueTimeEnd.Value;
            exp = exp.And(x => x.IssueTime <= issueTimeEnd);
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
    private static bool HasAnyListQueryFilter(TaktMaintenanceWorkOrderMaterialQueryDto? queryDto)
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
        if (queryDto.MaintenanceWorkOrderId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.WorkOrderCode))
        {
            return true;
        }
        if (queryDto.LineNumber.HasValue)
        {
            return true;
        }
        if (queryDto.MaterialId.HasValue)
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
        if (queryDto.RequiredQuantity.HasValue)
        {
            return true;
        }
        if (queryDto.IssuedQuantity.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialUnit))
        {
            return true;
        }
        if (queryDto.UnitPrice.HasValue)
        {
            return true;
        }
        if (queryDto.Amount.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.WarehouseCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.StorageLocation))
        {
            return true;
        }
        if (queryDto.IssueStatus.HasValue)
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
        if (queryDto.IsObsolete.HasValue)
        {
            return true;
        }
        if (queryDto.IssueTimeStart.HasValue || queryDto.IssueTimeEnd.HasValue)
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
