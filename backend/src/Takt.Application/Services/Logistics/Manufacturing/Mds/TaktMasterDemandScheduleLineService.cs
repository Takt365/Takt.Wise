// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Mds
// 文件名称：TaktMasterDemandScheduleLineService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：主需求计划MDS行应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Mds;
using Takt.Domain.Entities.Logistics.Manufacturing.Mds;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Mds;

/// <summary>
/// 主需求计划MDS行应用服务
/// </summary>
public class TaktMasterDemandScheduleLineService : TaktServiceBase, ITaktMasterDemandScheduleLineService
{
    private readonly ITaktCompanyRepository<TaktMasterDemandScheduleLine> _masterDemandScheduleLineRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="masterDemandScheduleLineRepository">主需求计划MDS行仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMasterDemandScheduleLineService(
        ITaktCompanyRepository<TaktMasterDemandScheduleLine> masterDemandScheduleLineRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _masterDemandScheduleLineRepository = masterDemandScheduleLineRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取主需求计划MDS行列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktMasterDemandScheduleLineDto>> GetMasterDemandScheduleLineListAsync(TaktMasterDemandScheduleLineQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktMasterDemandScheduleLineDto>.Create(
                new List<TaktMasterDemandScheduleLineDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _masterDemandScheduleLineRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktMasterDemandScheduleLineDto>.Create(
            data.Adapt<List<TaktMasterDemandScheduleLineDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取主需求计划MDS行
    /// </summary>
    /// <param name="id">主需求计划MDS行ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktMasterDemandScheduleLineDto?> GetMasterDemandScheduleLineByIdAsync(long id)
    {
        var entity = await _masterDemandScheduleLineRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktMasterDemandScheduleLineDto>();
    }

    /// <summary>
    /// 获取主需求计划MDS行选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetMasterDemandScheduleLineOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _masterDemandScheduleLineRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IsObsolete == 0,
            x => x.MdsCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.MdsCode,
            DictLabel = e.MdsCode,
        }).ToList();
    }

    /// <summary>
    /// 创建主需求计划MDS行
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMasterDemandScheduleLineDto> CreateMasterDemandScheduleLineAsync(TaktMasterDemandScheduleLineCreateDto dto)
    {
        var entity = dto.Adapt<TaktMasterDemandScheduleLine>();
        entity.IsObsolete = 0;
        var isUnique_ix_takt_logistics_manufacturing_mds_master_demand_schedule_line_bucket_unique = await _uniqueValidator.IsUniqueAsync(
            _masterDemandScheduleLineRepository,
            x => x.MasterDemandScheduleId == entity.MasterDemandScheduleId
                && x.MaterialCode == entity.MaterialCode
                && x.BucketStart == entity.BucketStart
                && x.DemandSourceType == entity.DemandSourceType);
        if (!isUnique_ix_takt_logistics_manufacturing_mds_master_demand_schedule_line_bucket_unique)
        {
            throw new TaktBusinessException("主需求计划MDS行的MasterDemandScheduleId、MaterialCode、BucketStart、DemandSourceType已存在");
        }
        var isUnique_ix_takt_logistics_manufacturing_mds_master_demand_schedule_line_line_unique = await _uniqueValidator.IsUniqueAsync(
            _masterDemandScheduleLineRepository,
            x => x.MasterDemandScheduleId == entity.MasterDemandScheduleId
                && x.LineNumber == entity.LineNumber
                && x.MaterialCode == entity.MaterialCode);
        if (!isUnique_ix_takt_logistics_manufacturing_mds_master_demand_schedule_line_line_unique)
        {
            throw new TaktBusinessException("主需求计划MDS行的MasterDemandScheduleId、LineNumber、MaterialCode已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _masterDemandScheduleLineRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.MasterDemandScheduleId == entity.MasterDemandScheduleId,
                x => x.LineNumber);
            var businessCode = entity.MasterDemandScheduleId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _masterDemandScheduleLineRepository.CreateAsync(entity);
        return await GetMasterDemandScheduleLineByIdAsync(entity.Id) ?? entity.Adapt<TaktMasterDemandScheduleLineDto>();
    }

    /// <summary>
    /// 更新主需求计划MDS行
    /// </summary>
    /// <param name="id">主需求计划MDS行ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMasterDemandScheduleLineDto> UpdateMasterDemandScheduleLineAsync(long id, TaktMasterDemandScheduleLineUpdateDto dto)
    {
        var entity = await _masterDemandScheduleLineRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("主需求计划MDS行不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_mds_master_demand_schedule_line_bucket_unique = await _uniqueValidator.IsUniqueAsync(
            _masterDemandScheduleLineRepository,
            x => x.MasterDemandScheduleId == entity.MasterDemandScheduleId
                && x.MaterialCode == entity.MaterialCode
                && x.BucketStart == entity.BucketStart
                && x.DemandSourceType == entity.DemandSourceType,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_mds_master_demand_schedule_line_bucket_unique)
        {
            throw new TaktBusinessException("主需求计划MDS行的MasterDemandScheduleId、MaterialCode、BucketStart、DemandSourceType已存在");
        }
        var isUnique_ix_takt_logistics_manufacturing_mds_master_demand_schedule_line_line_unique = await _uniqueValidator.IsUniqueAsync(
            _masterDemandScheduleLineRepository,
            x => x.MasterDemandScheduleId == entity.MasterDemandScheduleId
                && x.LineNumber == entity.LineNumber
                && x.MaterialCode == entity.MaterialCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_mds_master_demand_schedule_line_line_unique)
        {
            throw new TaktBusinessException("主需求计划MDS行的MasterDemandScheduleId、LineNumber、MaterialCode已存在");
        }
        await _masterDemandScheduleLineRepository.UpdateAsync(entity);
        return await GetMasterDemandScheduleLineByIdAsync(id) ?? throw new TaktBusinessException("主需求计划MDS行不存在");
    }

    /// <summary>
    /// 删除主需求计划MDS行
    /// </summary>
    /// <param name="id">主需求计划MDS行ID</param>
    /// <returns>任务</returns>
    public async Task DeleteMasterDemandScheduleLineByIdAsync(long id)
    {
        var entity = await _masterDemandScheduleLineRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("主需求计划MDS行不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("主需求计划MDS行不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("主需求计划MDS行已作废");
        }
        entity.IsObsolete = 1;
        await _masterDemandScheduleLineRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除主需求计划MDS行
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteMasterDemandScheduleLineBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteMasterDemandScheduleLineByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新主需求计划MDS行作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMasterDemandScheduleLineDto> UpdateMasterDemandScheduleLineObsoleteAsync(TaktMasterDemandScheduleLineObsoleteDto dto)
    {
        var entity = await _masterDemandScheduleLineRepository.GetByIdAsync(dto.MasterDemandScheduleLineId);
        if (entity == null)
        {
            throw new TaktBusinessException("主需求计划MDS行不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("主需求计划MDS行不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _masterDemandScheduleLineRepository.UpdateAsync(entity);
        return await GetMasterDemandScheduleLineByIdAsync(dto.MasterDemandScheduleLineId) ?? throw new TaktBusinessException("主需求计划MDS行不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetMasterDemandScheduleLineTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktMasterDemandScheduleLineTemplateDto>(
            sheetName ?? "主需求计划MDS行导入模板",
            fileName ?? "主需求计划MDS行导入模板.xlsx");
    }

    /// <summary>
    /// 导入主需求计划MDS行
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportMasterDemandScheduleLineAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktMasterDemandScheduleLineImportDto>(fileStream, sheetName ?? "主需求计划MDS行导入模板");
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
                var entity = rows[i].Adapt<TaktMasterDemandScheduleLine>();
                var importKey = $"{entity.MasterDemandScheduleId}|{entity.MaterialCode}|{entity.BucketStart}|{entity.DemandSourceType}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（MasterDemandScheduleId、MaterialCode、BucketStart、DemandSourceType）");
                }
                var isUnique_ix_takt_logistics_manufacturing_mds_master_demand_schedule_line_bucket_unique = await _uniqueValidator.IsUniqueAsync(
                    _masterDemandScheduleLineRepository,
                    x => x.MasterDemandScheduleId == entity.MasterDemandScheduleId
                        && x.MaterialCode == entity.MaterialCode
                        && x.BucketStart == entity.BucketStart
                        && x.DemandSourceType == entity.DemandSourceType);
                if (!isUnique_ix_takt_logistics_manufacturing_mds_master_demand_schedule_line_bucket_unique)
                {
                    throw new TaktBusinessException("主需求计划MDS行的MasterDemandScheduleId、MaterialCode、BucketStart、DemandSourceType已存在");
                }
                var isUnique_ix_takt_logistics_manufacturing_mds_master_demand_schedule_line_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _masterDemandScheduleLineRepository,
                    x => x.MasterDemandScheduleId == entity.MasterDemandScheduleId
                        && x.LineNumber == entity.LineNumber
                        && x.MaterialCode == entity.MaterialCode);
                if (!isUnique_ix_takt_logistics_manufacturing_mds_master_demand_schedule_line_line_unique)
                {
                    throw new TaktBusinessException("主需求计划MDS行的MasterDemandScheduleId、LineNumber、MaterialCode已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _masterDemandScheduleLineRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.MasterDemandScheduleId == entity.MasterDemandScheduleId,
                        x => x.LineNumber);
                    var businessCode = entity.MasterDemandScheduleId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _masterDemandScheduleLineRepository.CreateAsync(entity);
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
    /// 导出主需求计划MDS行
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportMasterDemandScheduleLineAsync(TaktMasterDemandScheduleLineQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktMasterDemandScheduleLineQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMasterDemandScheduleLineExportDto>(),
                sheetName ?? "主需求计划MDS行数据",
                fileName ?? "主需求计划MDS行导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _masterDemandScheduleLineRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMasterDemandScheduleLineExportDto>(),
                sheetName ?? "主需求计划MDS行数据",
                fileName ?? "主需求计划MDS行导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktMasterDemandScheduleLineExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "主需求计划MDS行数据",
            fileName ?? "主需求计划MDS行导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建主需求计划MDS行查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktMasterDemandScheduleLine, bool>> QueryExpression(TaktMasterDemandScheduleLineQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktMasterDemandScheduleLine>();

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
                || (x.MdsCode != null && x.MdsCode.Contains(keywords))
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

        if (queryDto?.MasterDemandScheduleId.HasValue == true)
        {
            var masterDemandScheduleId = queryDto.MasterDemandScheduleId.Value;
            exp = exp.And(x => x.MasterDemandScheduleId == masterDemandScheduleId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MdsCode))
        {
            var mdsCode = queryDto.MdsCode;
            exp = exp.And(x => x.MdsCode != null && x.MdsCode.Contains(mdsCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            var lineNumber = queryDto.LineNumber.Value;
            exp = exp.And(x => x.LineNumber == lineNumber);
        }

        if (queryDto?.DemandSourceType.HasValue == true)
        {
            var demandSourceType = queryDto.DemandSourceType.Value;
            exp = exp.And(x => x.DemandSourceType == demandSourceType);
        }

        if (queryDto?.SalesOrderId.HasValue == true)
        {
            var salesOrderId = queryDto.SalesOrderId.Value;
            exp = exp.And(x => x.SalesOrderId == salesOrderId);
        }

        if (queryDto?.SalesOrderLineNumber.HasValue == true)
        {
            var salesOrderLineNumber = queryDto.SalesOrderLineNumber.Value;
            exp = exp.And(x => x.SalesOrderLineNumber == salesOrderLineNumber);
        }

        if (queryDto?.SalesForecastId.HasValue == true)
        {
            var salesForecastId = queryDto.SalesForecastId.Value;
            exp = exp.And(x => x.SalesForecastId == salesForecastId);
        }

        if (queryDto?.SalesForecastLineNumber.HasValue == true)
        {
            var salesForecastLineNumber = queryDto.SalesForecastLineNumber.Value;
            exp = exp.And(x => x.SalesForecastLineNumber == salesForecastLineNumber);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialCode))
        {
            var materialCode = queryDto.MaterialCode;
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(materialCode));
        }

        if (queryDto?.DemandQuantity.HasValue == true)
        {
            var demandQuantity = queryDto.DemandQuantity.Value;
            exp = exp.And(x => x.DemandQuantity == demandQuantity);
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
    private static bool HasAnyListQueryFilter(TaktMasterDemandScheduleLineQueryDto? queryDto)
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
        if (queryDto.MasterDemandScheduleId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MdsCode))
        {
            return true;
        }
        if (queryDto.LineNumber.HasValue)
        {
            return true;
        }
        if (queryDto.DemandSourceType.HasValue)
        {
            return true;
        }
        if (queryDto.SalesOrderId.HasValue)
        {
            return true;
        }
        if (queryDto.SalesOrderLineNumber.HasValue)
        {
            return true;
        }
        if (queryDto.SalesForecastId.HasValue)
        {
            return true;
        }
        if (queryDto.SalesForecastLineNumber.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialCode))
        {
            return true;
        }
        if (queryDto.DemandQuantity.HasValue)
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
