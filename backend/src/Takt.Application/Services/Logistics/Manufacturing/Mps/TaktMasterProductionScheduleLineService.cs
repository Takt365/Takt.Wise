// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Mps
// 文件名称：TaktMasterProductionScheduleLineService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：主生产计划MPS行应用服务实现
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
/// 主生产计划MPS行应用服务
/// </summary>
public class TaktMasterProductionScheduleLineService : TaktServiceBase, ITaktMasterProductionScheduleLineService
{
    private readonly ITaktCompanyRepository<TaktMasterProductionScheduleLine> _masterProductionScheduleLineRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="masterProductionScheduleLineRepository">主生产计划MPS行仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMasterProductionScheduleLineService(
        ITaktCompanyRepository<TaktMasterProductionScheduleLine> masterProductionScheduleLineRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _masterProductionScheduleLineRepository = masterProductionScheduleLineRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取主生产计划MPS行列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktMasterProductionScheduleLineDto>> GetMasterProductionScheduleLineListAsync(TaktMasterProductionScheduleLineQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktMasterProductionScheduleLineDto>.Create(
                new List<TaktMasterProductionScheduleLineDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _masterProductionScheduleLineRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktMasterProductionScheduleLineDto>.Create(
            data.Adapt<List<TaktMasterProductionScheduleLineDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取主生产计划MPS行
    /// </summary>
    /// <param name="id">主生产计划MPS行ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktMasterProductionScheduleLineDto?> GetMasterProductionScheduleLineByIdAsync(long id)
    {
        var entity = await _masterProductionScheduleLineRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktMasterProductionScheduleLineDto>();
    }

    /// <summary>
    /// 获取主生产计划MPS行选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetMasterProductionScheduleLineOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _masterProductionScheduleLineRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IsObsolete == 0,
            x => x.MpsCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.MpsCode,
            DictLabel = e.MpsCode,
        }).ToList();
    }

    /// <summary>
    /// 创建主生产计划MPS行
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMasterProductionScheduleLineDto> CreateMasterProductionScheduleLineAsync(TaktMasterProductionScheduleLineCreateDto dto)
    {
        var entity = dto.Adapt<TaktMasterProductionScheduleLine>();
        entity.IsObsolete = 0;
        var isUnique_ix_takt_logistics_manufacturing_mps_master_production_schedule_line_bucket_unique = await _uniqueValidator.IsUniqueAsync(
            _masterProductionScheduleLineRepository,
            x => x.MasterProductionScheduleId == entity.MasterProductionScheduleId
                && x.MaterialCode == entity.MaterialCode
                && x.BucketStart == entity.BucketStart);
        if (!isUnique_ix_takt_logistics_manufacturing_mps_master_production_schedule_line_bucket_unique)
        {
            throw new TaktBusinessException("主生产计划MPS行的MasterProductionScheduleId、MaterialCode、BucketStart已存在");
        }
        var isUnique_ix_takt_logistics_manufacturing_mps_master_production_schedule_line_line_unique = await _uniqueValidator.IsUniqueAsync(
            _masterProductionScheduleLineRepository,
            x => x.MasterProductionScheduleId == entity.MasterProductionScheduleId
                && x.LineNumber == entity.LineNumber
                && x.MaterialCode == entity.MaterialCode);
        if (!isUnique_ix_takt_logistics_manufacturing_mps_master_production_schedule_line_line_unique)
        {
            throw new TaktBusinessException("主生产计划MPS行的MasterProductionScheduleId、LineNumber、MaterialCode已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _masterProductionScheduleLineRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.MasterProductionScheduleId == entity.MasterProductionScheduleId,
                x => x.LineNumber);
            var businessCode = entity.MasterProductionScheduleId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _masterProductionScheduleLineRepository.CreateAsync(entity);
        return await GetMasterProductionScheduleLineByIdAsync(entity.Id) ?? entity.Adapt<TaktMasterProductionScheduleLineDto>();
    }

    /// <summary>
    /// 更新主生产计划MPS行
    /// </summary>
    /// <param name="id">主生产计划MPS行ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMasterProductionScheduleLineDto> UpdateMasterProductionScheduleLineAsync(long id, TaktMasterProductionScheduleLineUpdateDto dto)
    {
        var entity = await _masterProductionScheduleLineRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("主生产计划MPS行不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_mps_master_production_schedule_line_bucket_unique = await _uniqueValidator.IsUniqueAsync(
            _masterProductionScheduleLineRepository,
            x => x.MasterProductionScheduleId == entity.MasterProductionScheduleId
                && x.MaterialCode == entity.MaterialCode
                && x.BucketStart == entity.BucketStart,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_mps_master_production_schedule_line_bucket_unique)
        {
            throw new TaktBusinessException("主生产计划MPS行的MasterProductionScheduleId、MaterialCode、BucketStart已存在");
        }
        var isUnique_ix_takt_logistics_manufacturing_mps_master_production_schedule_line_line_unique = await _uniqueValidator.IsUniqueAsync(
            _masterProductionScheduleLineRepository,
            x => x.MasterProductionScheduleId == entity.MasterProductionScheduleId
                && x.LineNumber == entity.LineNumber
                && x.MaterialCode == entity.MaterialCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_mps_master_production_schedule_line_line_unique)
        {
            throw new TaktBusinessException("主生产计划MPS行的MasterProductionScheduleId、LineNumber、MaterialCode已存在");
        }
        await _masterProductionScheduleLineRepository.UpdateAsync(entity);
        return await GetMasterProductionScheduleLineByIdAsync(id) ?? throw new TaktBusinessException("主生产计划MPS行不存在");
    }

    /// <summary>
    /// 删除主生产计划MPS行
    /// </summary>
    /// <param name="id">主生产计划MPS行ID</param>
    /// <returns>任务</returns>
    public async Task DeleteMasterProductionScheduleLineByIdAsync(long id)
    {
        var entity = await _masterProductionScheduleLineRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("主生产计划MPS行不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("主生产计划MPS行不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("主生产计划MPS行已作废");
        }
        entity.IsObsolete = 1;
        await _masterProductionScheduleLineRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除主生产计划MPS行
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteMasterProductionScheduleLineBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteMasterProductionScheduleLineByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新主生产计划MPS行作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMasterProductionScheduleLineDto> UpdateMasterProductionScheduleLineObsoleteAsync(TaktMasterProductionScheduleLineObsoleteDto dto)
    {
        var entity = await _masterProductionScheduleLineRepository.GetByIdAsync(dto.MasterProductionScheduleLineId);
        if (entity == null)
        {
            throw new TaktBusinessException("主生产计划MPS行不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("主生产计划MPS行不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _masterProductionScheduleLineRepository.UpdateAsync(entity);
        return await GetMasterProductionScheduleLineByIdAsync(dto.MasterProductionScheduleLineId) ?? throw new TaktBusinessException("主生产计划MPS行不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetMasterProductionScheduleLineTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktMasterProductionScheduleLineTemplateDto>(
            sheetName ?? "主生产计划MPS行导入模板",
            fileName ?? "主生产计划MPS行导入模板.xlsx");
    }

    /// <summary>
    /// 导入主生产计划MPS行
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportMasterProductionScheduleLineAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktMasterProductionScheduleLineImportDto>(fileStream, sheetName ?? "主生产计划MPS行导入模板");
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
                var entity = rows[i].Adapt<TaktMasterProductionScheduleLine>();
                var importKey = $"{entity.MasterProductionScheduleId}|{entity.MaterialCode}|{entity.BucketStart}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（MasterProductionScheduleId、MaterialCode、BucketStart）");
                }
                var isUnique_ix_takt_logistics_manufacturing_mps_master_production_schedule_line_bucket_unique = await _uniqueValidator.IsUniqueAsync(
                    _masterProductionScheduleLineRepository,
                    x => x.MasterProductionScheduleId == entity.MasterProductionScheduleId
                        && x.MaterialCode == entity.MaterialCode
                        && x.BucketStart == entity.BucketStart);
                if (!isUnique_ix_takt_logistics_manufacturing_mps_master_production_schedule_line_bucket_unique)
                {
                    throw new TaktBusinessException("主生产计划MPS行的MasterProductionScheduleId、MaterialCode、BucketStart已存在");
                }
                var isUnique_ix_takt_logistics_manufacturing_mps_master_production_schedule_line_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _masterProductionScheduleLineRepository,
                    x => x.MasterProductionScheduleId == entity.MasterProductionScheduleId
                        && x.LineNumber == entity.LineNumber
                        && x.MaterialCode == entity.MaterialCode);
                if (!isUnique_ix_takt_logistics_manufacturing_mps_master_production_schedule_line_line_unique)
                {
                    throw new TaktBusinessException("主生产计划MPS行的MasterProductionScheduleId、LineNumber、MaterialCode已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _masterProductionScheduleLineRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.MasterProductionScheduleId == entity.MasterProductionScheduleId,
                        x => x.LineNumber);
                    var businessCode = entity.MasterProductionScheduleId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _masterProductionScheduleLineRepository.CreateAsync(entity);
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
    /// 导出主生产计划MPS行
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportMasterProductionScheduleLineAsync(TaktMasterProductionScheduleLineQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktMasterProductionScheduleLineQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMasterProductionScheduleLineExportDto>(),
                sheetName ?? "主生产计划MPS行数据",
                fileName ?? "主生产计划MPS行导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _masterProductionScheduleLineRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMasterProductionScheduleLineExportDto>(),
                sheetName ?? "主生产计划MPS行数据",
                fileName ?? "主生产计划MPS行导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktMasterProductionScheduleLineExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "主生产计划MPS行数据",
            fileName ?? "主生产计划MPS行导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建主生产计划MPS行查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktMasterProductionScheduleLine, bool>> QueryExpression(TaktMasterProductionScheduleLineQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktMasterProductionScheduleLine>();

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
                || (x.MpsCode != null && x.MpsCode.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.UnitOfMeasure != null && x.UnitOfMeasure.Contains(keywords))
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

        if (queryDto?.MasterProductionScheduleId.HasValue == true)
        {
            var masterProductionScheduleId = queryDto.MasterProductionScheduleId.Value;
            exp = exp.And(x => x.MasterProductionScheduleId == masterProductionScheduleId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MpsCode))
        {
            var mpsCode = queryDto.MpsCode;
            exp = exp.And(x => x.MpsCode != null && x.MpsCode.Contains(mpsCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            var lineNumber = queryDto.LineNumber.Value;
            exp = exp.And(x => x.LineNumber == lineNumber);
        }

        if (queryDto?.MasterDemandScheduleLineId.HasValue == true)
        {
            var masterDemandScheduleLineId = queryDto.MasterDemandScheduleLineId.Value;
            exp = exp.And(x => x.MasterDemandScheduleLineId == masterDemandScheduleLineId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialCode))
        {
            var materialCode = queryDto.MaterialCode;
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(materialCode));
        }

        if (queryDto?.GrossRequirement.HasValue == true)
        {
            var grossRequirement = queryDto.GrossRequirement.Value;
            exp = exp.And(x => x.GrossRequirement == grossRequirement);
        }

        if (queryDto?.ScheduledReceipts.HasValue == true)
        {
            var scheduledReceipts = queryDto.ScheduledReceipts.Value;
            exp = exp.And(x => x.ScheduledReceipts == scheduledReceipts);
        }

        if (queryDto?.ProjectedOnHand.HasValue == true)
        {
            var projectedOnHand = queryDto.ProjectedOnHand.Value;
            exp = exp.And(x => x.ProjectedOnHand == projectedOnHand);
        }

        if (queryDto?.NetRequirement.HasValue == true)
        {
            var netRequirement = queryDto.NetRequirement.Value;
            exp = exp.And(x => x.NetRequirement == netRequirement);
        }

        if (queryDto?.PlannedOrderQuantity.HasValue == true)
        {
            var plannedOrderQuantity = queryDto.PlannedOrderQuantity.Value;
            exp = exp.And(x => x.PlannedOrderQuantity == plannedOrderQuantity);
        }

        if (queryDto?.AtpQuantity.HasValue == true)
        {
            var atpQuantity = queryDto.AtpQuantity.Value;
            exp = exp.And(x => x.AtpQuantity == atpQuantity);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.UnitOfMeasure))
        {
            var unitOfMeasure = queryDto.UnitOfMeasure;
            exp = exp.And(x => x.UnitOfMeasure != null && x.UnitOfMeasure.Contains(unitOfMeasure));
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

        if (queryDto?.BucketStartStart.HasValue == true)
        {
            var bucketStartStart = queryDto.BucketStartStart.Value;
            exp = exp.And(x => x.BucketStart >= bucketStartStart);
        }

        if (queryDto?.BucketStartEnd.HasValue == true)
        {
            var bucketStartEnd = queryDto.BucketStartEnd.Value;
            exp = exp.And(x => x.BucketStart <= bucketStartEnd);
        }

        if (queryDto?.BucketEndStart.HasValue == true)
        {
            var bucketEndStart = queryDto.BucketEndStart.Value;
            exp = exp.And(x => x.BucketEnd >= bucketEndStart);
        }

        if (queryDto?.BucketEndEnd.HasValue == true)
        {
            var bucketEndEnd = queryDto.BucketEndEnd.Value;
            exp = exp.And(x => x.BucketEnd <= bucketEndEnd);
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
    private static bool HasAnyListQueryFilter(TaktMasterProductionScheduleLineQueryDto? queryDto)
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
        if (queryDto.MasterProductionScheduleId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MpsCode))
        {
            return true;
        }
        if (queryDto.LineNumber.HasValue)
        {
            return true;
        }
        if (queryDto.MasterDemandScheduleLineId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialCode))
        {
            return true;
        }
        if (queryDto.GrossRequirement.HasValue)
        {
            return true;
        }
        if (queryDto.ScheduledReceipts.HasValue)
        {
            return true;
        }
        if (queryDto.ProjectedOnHand.HasValue)
        {
            return true;
        }
        if (queryDto.NetRequirement.HasValue)
        {
            return true;
        }
        if (queryDto.PlannedOrderQuantity.HasValue)
        {
            return true;
        }
        if (queryDto.AtpQuantity.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.UnitOfMeasure))
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
        if (queryDto.BucketStartStart.HasValue || queryDto.BucketStartEnd.HasValue)
        {
            return true;
        }
        if (queryDto.BucketEndStart.HasValue || queryDto.BucketEndEnd.HasValue)
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
