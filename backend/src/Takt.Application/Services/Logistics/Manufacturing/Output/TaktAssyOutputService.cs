// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Output
// 文件名称：TaktAssyOutputService.cs
// 创建时间：2026-07-06
// 创建人：Takt365(Cursor AI)
// 功能描述：组立日报应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Globalization;
using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Output;
using Takt.Application.Services.Logistics.Manufacturing.Defect;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Manufacturing.Defect;
using Takt.Domain.Entities.Logistics.Manufacturing.Output;
using Takt.Domain.Entities.Logistics.Manufacturing.Aps;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Constants;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Output;

/// <summary>
/// 组立日报应用服务
/// </summary>
public class TaktAssyOutputService : TaktServiceBase, ITaktAssyOutputService
{
    private readonly ITaktCompanyRepository<TaktAssyOutput> _assyOutputRepository;
    private readonly ITaktCompanyRepository<TaktAssyOutputDetail> _assyOutputDetailRepository;
    private readonly ITaktCompanyRepository<TaktAssyDefect> _assyDefectRepository;
    private readonly ITaktCompanyRepository<TaktAssyDefectDetail> _assyDefectDetailRepository;
    private readonly ITaktCompanyRepository<TaktAssyOrderDefect> _assyOrderDefectRepository;
    private readonly ITaktCompanyRepository<TaktAssyBatchDefect> _assyBatchDefectRepository;
    private readonly ITaktCompanyRepository<TaktStandardOperationRate> _standardOperationRateRepository;
    private readonly ITaktApprovalRepository<TaktStandardOperationTime> _standardOperationTimeRepository;
    private readonly ITaktCompanyRepository<TaktProductionOrder> _productionOrderRepository;
    private readonly ITaktCompanyRepository<TaktProductionChangeover> _productionChangeoverRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="assyOutputRepository">组立日报仓储</param>
    /// <param name="assyOutputDetailRepository">AssyOutputDetail仓储</param>
    /// <param name="assyDefectRepository">组立不良日报仓储</param>
    /// <param name="assyDefectDetailRepository">组立不良明细仓储</param>
    /// <param name="assyOrderDefectRepository">工单不良统计仓储</param>
    /// <param name="assyBatchDefectRepository">批量不良统计仓储</param>
    /// <param name="standardOperationRateRepository">标准生产稼动率仓储</param>
    /// <param name="standardOperationTimeRepository">标准工序时间仓储</param>
    /// <param name="productionOrderRepository">生产工单仓储</param>
    /// <param name="productionChangeoverRepository">生产切换记录仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktAssyOutputService(
        ITaktCompanyRepository<TaktAssyOutput> assyOutputRepository,
        ITaktCompanyRepository<TaktAssyOutputDetail> assyOutputDetailRepository,
        ITaktCompanyRepository<TaktAssyDefect> assyDefectRepository,
        ITaktCompanyRepository<TaktAssyDefectDetail> assyDefectDetailRepository,
        ITaktCompanyRepository<TaktAssyOrderDefect> assyOrderDefectRepository,
        ITaktCompanyRepository<TaktAssyBatchDefect> assyBatchDefectRepository,
        ITaktCompanyRepository<TaktStandardOperationRate> standardOperationRateRepository,
        ITaktApprovalRepository<TaktStandardOperationTime> standardOperationTimeRepository,
        ITaktCompanyRepository<TaktProductionOrder> productionOrderRepository,
        ITaktCompanyRepository<TaktProductionChangeover> productionChangeoverRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _assyOutputRepository = assyOutputRepository;
        _assyOutputDetailRepository = assyOutputDetailRepository;
        _assyDefectRepository = assyDefectRepository;
        _assyDefectDetailRepository = assyDefectDetailRepository;
        _assyOrderDefectRepository = assyOrderDefectRepository;
        _assyBatchDefectRepository = assyBatchDefectRepository;
        _standardOperationRateRepository = standardOperationRateRepository;
        _standardOperationTimeRepository = standardOperationTimeRepository;
        _productionOrderRepository = productionOrderRepository;
        _productionChangeoverRepository = productionChangeoverRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取组立日报列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktAssyOutputDto>> GetAssyOutputListAsync(TaktAssyOutputQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _assyOutputRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktAssyOutputDto>.Create(
            data.Adapt<List<TaktAssyOutputDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取组立日报
    /// </summary>
    /// <param name="id">组立日报ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktAssyOutputDto?> GetAssyOutputByIdAsync(long id)
    {
        var entity = await _assyOutputRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktAssyOutputDto>();
        await FillAssyOutputDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取组立日报选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetAssyOutputOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _assyOutputRepository.GetListAsync(
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
    /// 获取组立日报新增时固定的生产时段列表（13 条）
    /// </summary>
    /// <returns>生产时段字符串列表</returns>
    public Task<IReadOnlyList<string>> GetAssyOutputDefaultTimePeriodsAsync()
    {
        IReadOnlyList<string> periods = TaktAssyOutputTimePeriodConstants.DefaultTimePeriods;
        return Task.FromResult(periods);
    }

    /// <summary>
    /// 获取组立不良日报新增用工单选项（来源已生产的组立日报，排除同日同工单已存在不良日报）
    /// </summary>
    /// <param name="excludeAssyDefectId">编辑态当前不良日报 ID（保留其对应组立日报在选项中）</param>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项，DictValue 为组立日报 Id</returns>
    public async Task<List<TaktSelectOption>> GetAssyOutputProdOrderOptionsAsync(string? plantCode = null, string? keyword = null, long? excludeAssyDefectId = null)
    {
        const int maxOptions = TaktPagedOptions.HardMaxPageSize;
        EnsureThreeLayerContext();
        var defects = await _assyDefectRepository.GetListAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode);
        var occupiedDailyOrderKeys = defects
            .Where(d => !excludeAssyDefectId.HasValue || d.Id != excludeAssyDefectId.Value)
            .Select(d => BuildAssyDefectDailyOrderKey(d.ProdDate, d.ProdOrderCode))
            .ToHashSet(StringComparer.Ordinal);
        var outputs = await _assyOutputRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.ProdDate,
            false);
        if (outputs.Count == 0)
        {
            return [];
        }
        var outputIds = outputs.Select(x => x.Id).ToList();
        var allDetails = await _assyOutputDetailRepository.GetListAsync(x => outputIds.Contains(x.AssyOutputId));
        var prodActualQtyByOutputId = allDetails
            .GroupBy(x => x.AssyOutputId)
            .ToDictionary(g => g.Key, g => g.Sum(d => d.ProdActualQty));
        var options = new List<TaktSelectOption>();
        foreach (var output in outputs.OrderByDescending(x => x.ProdDate).ThenBy(x => x.ProdOrderCode, StringComparer.Ordinal))
        {
            if (!prodActualQtyByOutputId.TryGetValue(output.Id, out var prodActualQty) || prodActualQty <= 0)
            {
                continue;
            }
            var dailyKey = BuildAssyDefectDailyOrderKey(output.ProdDate, output.ProdOrderCode);
            if (occupiedDailyOrderKeys.Contains(dailyKey))
            {
                continue;
            }
            options.Add(new TaktSelectOption
            {
                DictValue = output.Id,
                DictLabel = $"{output.ProdOrderCode} ({output.ProdDate:yyyy-MM-dd})",
                ExtValue = output.ProdOrderCode,
                SortOrder = options.Count,
            });
            if (options.Count >= maxOptions)
            {
                break;
            }
        }
        return options;
    }

    /// <summary>
    /// 创建组立日报
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAssyOutputDto> CreateAssyOutputAsync(TaktAssyOutputCreateDto dto)
    {
        var entity = dto.Adapt<TaktAssyOutput>();
        ApplyDefaultAssyOutputProdDateIfMissing(entity);
        EnsureAssyOutputProdDateEditable(entity.ProdDate);
        EnsureThreeLayerContext();
        await ApplyPlantCodeFromProdOrderAsync(entity);
        var isUnique_ix_takt_logistics_manufacturing_output_assy_unique = await _uniqueValidator.IsUniqueAsync(
            _assyOutputRepository,
            x => x.TeamCode == entity.TeamCode
                && x.ProdCategory == entity.ProdCategory
                && x.ProdDate == entity.ProdDate.Date
                && x.ProdOrderCode == entity.ProdOrderCode);
        if (!isUnique_ix_takt_logistics_manufacturing_output_assy_unique)
        {
            throw new TaktBusinessException("组立日报的生产班组、生产类别、生产日期、工单号已存在");
        }
        await ApplyAssyOutputDerivedFieldsAsync(entity);
        entity = await _assyOutputRepository.CreateAsync(entity);
        // 仅当主表标准产能 > 0 时自动补齐 13 条固定生产时段明细
        if (entity.StdCapacity > 0)
        {
            EnsureDefaultAssyOutputDetailsOnCreate(dto);
        }
        await SaveAssyOutputChildrenAsync(entity, dto);
        await SyncDefectFromOutputAsync(entity);
        await RefreshAssyOutputChangeoverBucketsForOutputAsync(entity);
        return await GetAssyOutputByIdAsync(entity.Id) ?? entity.Adapt<TaktAssyOutputDto>();
    }

    /// <summary>
    /// 更新组立日报
    /// </summary>
    /// <param name="id">组立日报ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAssyOutputDto> UpdateAssyOutputAsync(long id, TaktAssyOutputUpdateDto dto)
    {
        var entity = await _assyOutputRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("组立日报不存在");
        }
        EnsureAssyOutputProdDateEditable(entity.ProdDate);
        var oldOutputSnapshot = entity.Adapt<TaktAssyOutput>();
        dto.Adapt(entity);
        entity.ProdOrderCode = oldOutputSnapshot.ProdOrderCode;
        EnsureAssyOutputProdDateEditable(entity.ProdDate);
        EnsureThreeLayerContext();
        await ApplyPlantCodeFromProdOrderAsync(entity);
        var isUnique_ix_takt_logistics_manufacturing_output_assy_unique = await _uniqueValidator.IsUniqueAsync(
            _assyOutputRepository,
            x => x.TeamCode == entity.TeamCode
                && x.ProdCategory == entity.ProdCategory
                && x.ProdDate == entity.ProdDate.Date
                && x.ProdOrderCode == entity.ProdOrderCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_output_assy_unique)
        {
            throw new TaktBusinessException("组立日报的生产班组、生产类别、生产日期、工单号已存在");
        }
        await ApplyAssyOutputDerivedFieldsAsync(entity);
        await _assyOutputRepository.UpdateAsync(entity);
        await UpdateAssyOutputChildrenAsync(entity, dto);
        if (!IsSameOutputDefectKey(oldOutputSnapshot, entity))
        {
            await DeleteDefectForOutputAsync(oldOutputSnapshot);
        }
        await SyncDefectFromOutputAsync(entity);
        await RefreshAssyOutputChangeoverBucketsForOutputAsync(entity);
        return await GetAssyOutputByIdAsync(id) ?? throw new TaktBusinessException("组立日报不存在");
    }

    /// <summary>
    /// 删除组立日报
    /// </summary>
    /// <param name="id">组立日报ID</param>
    /// <returns>任务</returns>
    public async Task DeleteAssyOutputByIdAsync(long id)
    {
        var entity = await _assyOutputRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("组立日报不存在或已删除");
        }
        EnsureAssyOutputProdDateEditable(entity.ProdDate);
        var details = await _assyOutputDetailRepository.GetListAsync(x => x.AssyOutputId == entity.Id);
        var bucketsToRefresh = CollectAssyOutputDetailMixedProdBuckets(entity, details);
        var changeoverBucketsToRefresh = CollectAssyOutputDetailChangeoverBuckets(entity, details);
        await DeleteDefectForOutputAsync(entity);
        await _assyOutputDetailRepository.DeleteAsync(x => x.AssyOutputId == entity.Id);
        var deleted = await _assyOutputRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("组立日报不存在或已删除");
        }
        await RefreshAssyOutputDetailMixedProdBucketsAsync(bucketsToRefresh);
        await RefreshAssyOutputChangeoverBucketsAsync(changeoverBucketsToRefresh);
    }

    /// <summary>
    /// 批量删除组立日报
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteAssyOutputBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteAssyOutputByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetAssyOutputTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktAssyOutputTemplateDto>(
            sheetName ?? "组立日报导入模板",
            fileName ?? "组立日报导入模板.xlsx");
    }

    /// <summary>
    /// 导入组立日报
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportAssyOutputAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktAssyOutputImportDto>(fileStream, sheetName ?? "组立日报导入模板");
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
                var entity = rows[i].Adapt<TaktAssyOutput>();
                EnsureThreeLayerContext();
                ApplyDefaultAssyOutputProdDateIfMissing(entity);
                EnsureAssyOutputProdDateEditable(entity.ProdDate);
                await ApplyPlantCodeFromProdOrderAsync(entity);
                var importKey = TaktOutputOrderUniqueHelper.BuildTeamDailyOrderImportKey(
                    entity.TeamCode,
                    entity.ProdCategory,
                    entity.ProdDate,
                    entity.ProdOrderCode);
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（TeamCode、ProdCategory、ProdDate、ProdOrderCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_output_assy_unique = await _uniqueValidator.IsUniqueAsync(
                    _assyOutputRepository,
                    x => x.TeamCode == entity.TeamCode
                        && x.ProdCategory == entity.ProdCategory
                        && x.ProdDate == entity.ProdDate.Date
                        && x.ProdOrderCode == entity.ProdOrderCode);
                if (!isUnique_ix_takt_logistics_manufacturing_output_assy_unique)
                {
                    throw new TaktBusinessException("组立日报的生产班组、生产类别、生产日期、工单号已存在");
                }
                await ApplyAssyOutputDerivedFieldsAsync(entity);
                await _assyOutputRepository.CreateAsync(entity);
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
    /// 导出组立日报
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportAssyOutputAsync(TaktAssyOutputQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktAssyOutputQueryDto());
        var list = await _assyOutputRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktAssyOutputExportDto>(),
                sheetName ?? "组立日报数据",
                fileName ?? "组立日报导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktAssyOutputExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "组立日报数据",
            fileName ?? "组立日报导出.xlsx");
    }

    // ========================================
    // 派生字段计算（标准工时回填、标准产能）
    // ========================================

    /// <summary>
    /// 按工单号回填工厂代码
    /// </summary>
    /// <param name="entity">组立日报主表</param>
    /// <returns>任务</returns>
    private async Task ApplyPlantCodeFromProdOrderAsync(TaktAssyOutput entity)
    {
        await TaktProductionOrderBackfillHelper.ApplyPlantCodeAsync(
            _productionOrderRepository,
            CurrentTenantCode,
            CurrentCompanyCode,
            entity.ProdOrderCode,
            v => entity.PlantCode = v);
    }

    /// <summary>
    /// 回填标准工时并计算标准产能
    /// </summary>
    /// <param name="entity">组立日报主表</param>
    /// <returns>任务</returns>
    private async Task ApplyAssyOutputDerivedFieldsAsync(TaktAssyOutput entity)
    {
        await TaktAssyOutputDerivedFieldsHelper.ApplyDerivedFieldsAsync(
            _standardOperationTimeRepository,
            _standardOperationRateRepository,
            entity,
            CurrentTenantCode,
            CurrentCompanyCode);
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充组立日报详情（加载 OneToMany 子表：组立日报明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillAssyOutputDetailsAsync(TaktAssyOutputDto dto, TaktAssyOutput entity)
    {
        if (dto == null)
        {
            return;
        }
        // 组立日报明细 → dto.AssyOutputDetails
        var assyoutputdetails = await _assyOutputDetailRepository.GetListAsync(x => x.AssyOutputId == entity.Id);
        dto.AssyOutputDetails = assyoutputdetails.Adapt<List<TaktAssyOutputDetailDto>>();
    }

    /// <summary>
    /// 保存组立日报子表级联（组立日报明细；仅 Create 时先删后插，Update 走 UpdateAssyOutputChildrenAsync）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveAssyOutputChildrenAsync(TaktAssyOutput entity, TaktAssyOutputCreateDto dto)
    {
        var existingDetails = await _assyOutputDetailRepository.GetListAsync(x => x.AssyOutputId == entity.Id);
        var bucketsToRefresh = CollectAssyOutputDetailMixedProdBuckets(entity, existingDetails);
        // 组立日报明细（AssyOutputDetails）
        if (dto.AssyOutputDetails is not { Count: > 0 })
        {
            await _assyOutputDetailRepository.DeleteAsync(x => x.AssyOutputId == entity.Id);
            await RefreshAssyOutputDetailMixedProdBucketsAsync(bucketsToRefresh);
            return;
        }
        var assyoutputdetails = dto.AssyOutputDetails.Adapt<List<TaktAssyOutputDetail>>();
        foreach (var child in assyoutputdetails)
        {
            child.AssyOutputId = entity.Id;
            child.ProdOrderCode = entity.ProdOrderCode;
            if (!string.IsNullOrWhiteSpace(child.TimePeriod)
                && ShouldRefreshAssyMixedProdBucket(child))
            {
                bucketsToRefresh.Add((entity.TeamCode, entity.ProdDate.Date, child.TimePeriod));
            }
        }
        var proposedMasterProdActualTotal = assyoutputdetails.Sum(x => x.ProdActualQty);
        await TaktAssyOutputProdActualQtyLimitHelper.EnsureProdActualQtyNotExceedForMasterAsync(
            _assyOutputDetailRepository,
            _productionOrderRepository,
            CurrentTenantCode,
            CurrentCompanyCode,
            entity,
            proposedMasterProdActualTotal,
            entity.Id);
        var assyoutputdetailsNeedLine = assyoutputdetails.Where(c => c.LineNumber <= 0).ToList();
        if (assyoutputdetailsNeedLine.Count > 0)
        {
            var businessCode = !string.IsNullOrWhiteSpace(entity.ProdOrderCode) ? entity.ProdOrderCode : entity.Id.ToString();
            var maxLine = await _assyOutputDetailRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.AssyOutputId == entity.Id,
                x => x.LineNumber);
            var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, assyoutputdetailsNeedLine.Count, maxLine).ToList();
            var lineIdx = 0;
            foreach (var child in assyoutputdetails)
            {
                if (child.LineNumber <= 0)
                {
                    child.LineNumber = lineSeq[lineIdx++];
                }
            }
        }
        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
        for (var i = 0; i < assyoutputdetails.Count; i++)
        {
            var period = (assyoutputdetails[i].TimePeriod ?? string.Empty).Trim();
            var key = $"{assyoutputdetails[i].CompanyCode}|{assyoutputdetails[i].AssyOutputId}|{period}|{assyoutputdetails[i].LineNumber}";
            if (!seenKeys.Add(key))
            {
                throw new TaktBusinessException($"组立日报明细第{i + 1}项与本次提交的其他项重复（CompanyCode、AssyOutputId、TimePeriod、LineNumber）");
            }
        }
        await _assyOutputDetailRepository.DeleteAsync(x => x.AssyOutputId == entity.Id);
        foreach (var child in assyoutputdetails)
        {
            var period = (child.TimePeriod ?? string.Empty).Trim();
            child.TimePeriod = period;
            var isUnique_ix_takt_logistics_manufacturing_output_assy_detail_unique = await _uniqueValidator.IsUniqueAsync(
                _assyOutputDetailRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.AssyOutputId == child.AssyOutputId
                    && x.TimePeriod == period
                    && x.LineNumber == child.LineNumber);
            if (!isUnique_ix_takt_logistics_manufacturing_output_assy_detail_unique)
            {
                throw new TaktBusinessException("组立日报明细的CompanyCode、AssyOutputId、TimePeriod、LineNumber已存在");
            }
        }
        var operationRatePercent = await TaktAssyOutputDerivedFieldsHelper.ResolvePersonnelOperationRatePercentAsync(
            _standardOperationRateRepository,
            CurrentTenantCode,
            CurrentCompanyCode,
            entity.PlantCode,
            entity.ProdDate);
        foreach (var child in assyoutputdetails)
        {
            TaktAssyOutputDetailDerivedFieldsHelper.ApplyCalculatedFields(
                child,
                entity,
                TaktProductionStatHelper.CalculateAssyMixedProdCount(1),
                operationRatePercent);
        }
        await _assyOutputDetailRepository.CreateRangeAsync(assyoutputdetails);
        await RefreshAssyOutputDetailMixedProdBucketsAsync(bucketsToRefresh);
    }

    /// <summary>
    /// 更新组立日报子表明细（按生产时段匹配已有行并就地更新，不删后插；工单号/时段/行号保持不变）
    /// </summary>
    /// <param name="entity">已持久化的主表实体</param>
    /// <param name="dto">更新 DTO（含 AssyOutputDetails 时级联更新可编辑字段）</param>
    /// <returns>任务</returns>
    private async Task UpdateAssyOutputChildrenAsync(TaktAssyOutput entity, TaktAssyOutputUpdateDto dto)
    {
        if (dto.AssyOutputDetails is not { Count: > 0 })
        {
            return;
        }
        var existingList = await _assyOutputDetailRepository.GetListAsync(x => x.AssyOutputId == entity.Id);
        if (existingList.Count == 0)
        {
            return;
        }
        var proposedMasterProdActualTotal = TaktAssyOutputProdActualQtyLimitHelper.CalculateMasterProdActualTotalAfterDetailUpdates(
            existingList,
            dto.AssyOutputDetails);
        await TaktAssyOutputProdActualQtyLimitHelper.EnsureProdActualQtyNotExceedForMasterAsync(
            _assyOutputDetailRepository,
            _productionOrderRepository,
            CurrentTenantCode,
            CurrentCompanyCode,
            entity,
            proposedMasterProdActualTotal,
            entity.Id);
        var existingByPeriod = existingList
            .Where(x => !string.IsNullOrWhiteSpace(x.TimePeriod))
            .ToDictionary(x => x.TimePeriod.Trim(), x => x, StringComparer.Ordinal);
        var bucketsToRefresh = CollectAssyOutputDetailMixedProdBuckets(entity, existingList);
        var operationRatePercent = await TaktAssyOutputDerivedFieldsHelper.ResolvePersonnelOperationRatePercentAsync(
            _standardOperationRateRepository,
            CurrentTenantCode,
            CurrentCompanyCode,
            entity.PlantCode,
            entity.ProdDate);
        for (var i = 0; i < dto.AssyOutputDetails.Count; i++)
        {
            var childDto = dto.AssyOutputDetails[i];
            var periodKey = (childDto.TimePeriod ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(periodKey) || !existingByPeriod.TryGetValue(periodKey, out var existing))
            {
                throw new TaktBusinessException($"组立日报明细第{i + 1}项生产时段无效或与已有明细不匹配");
            }
            existing.ProdActualQty = childDto.ProdActualQty;
            existing.DowntimeMinutes = childDto.DowntimeMinutes;
            existing.DowntimeReason = childDto.DowntimeReason ?? string.Empty;
            existing.DowntimeDescription = childDto.DowntimeDescription ?? string.Empty;
            existing.UnachievedReason = childDto.UnachievedReason ?? string.Empty;
            existing.UnachievedDescription = childDto.UnachievedDescription ?? string.Empty;
            existing.ConfirmMinutes = childDto.ConfirmMinutes;
            existing.ExtField = childDto.ExtField;
            existing.ProdOrderCode = entity.ProdOrderCode;
            if (ShouldRefreshAssyMixedProdBucket(existing))
            {
                bucketsToRefresh.Add((entity.TeamCode, entity.ProdDate.Date, existing.TimePeriod));
            }
            TaktAssyOutputDetailDerivedFieldsHelper.ApplyCalculatedFields(
                existing,
                entity,
                TaktProductionStatHelper.CalculateAssyMixedProdCount(1),
                operationRatePercent);
            await _assyOutputDetailRepository.UpdateAsync(existing);
        }
        await RefreshAssyOutputDetailMixedProdBucketsAsync(bucketsToRefresh);
    }

    /// <summary>
    /// 校验生产日期未过编辑截止日（次月 cutoff 日之后不可新增/修改/删除）
    /// </summary>
    /// <param name="prodDate">生产日期</param>
    private void EnsureAssyOutputProdDateEditable(DateTime prodDate)
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
    /// 未传生产日期时补默认日期
    /// </summary>
    /// <param name="entity">组立日报实体</param>
    private static void ApplyDefaultAssyOutputProdDateIfMissing(TaktAssyOutput entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        if (entity.ProdDate == default)
        {
            entity.ProdDate = DateTime.Today.AddDays(-1).Date;
        }
    }

    /// <summary>
    /// 新增组立日报时，在主表标准产能 &gt; 0 的前提下确保 13 条固定生产时段明细（与客户端已填写的同时段行合并）
    /// </summary>
    /// <param name="dto">创建 DTO</param>
    private static void EnsureDefaultAssyOutputDetailsOnCreate(TaktAssyOutputCreateDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        var submittedByPeriod = (dto.AssyOutputDetails ?? new List<TaktAssyOutputDetailCreateDto>())
            .Where(d => !string.IsNullOrWhiteSpace(d.TimePeriod))
            .GroupBy(d => d.TimePeriod.Trim(), StringComparer.Ordinal)
            .ToDictionary(g => g.Key, g => g.First(), StringComparer.Ordinal);
        var details = new List<TaktAssyOutputDetailCreateDto>(TaktAssyOutputTimePeriodConstants.DefaultTimePeriods.Length);
        var lineNumber = 10;
        foreach (var timePeriod in TaktAssyOutputTimePeriodConstants.DefaultTimePeriods)
        {
            if (submittedByPeriod.TryGetValue(timePeriod, out var existing))
            {
                existing.LineNumber = lineNumber;
                if (string.IsNullOrWhiteSpace(existing.ProdOrderCode))
                {
                    existing.ProdOrderCode = dto.ProdOrderCode;
                }
                if (string.IsNullOrWhiteSpace(existing.TenantCode))
                {
                    existing.TenantCode = dto.TenantCode;
                }
                if (string.IsNullOrWhiteSpace(existing.CompanyCode))
                {
                    existing.CompanyCode = dto.CompanyCode;
                }
                if (string.IsNullOrWhiteSpace(existing.CultureCode))
                {
                    existing.CultureCode = dto.CultureCode;
                }
                existing.TimePeriod = timePeriod;
                details.Add(existing);
            }
            else
            {
                details.Add(new TaktAssyOutputDetailCreateDto
                {
                    TenantCode = dto.TenantCode,
                    CompanyCode = dto.CompanyCode,
                    CultureCode = dto.CultureCode,
                    AssyOutputId = 0,
                    ProdOrderCode = dto.ProdOrderCode,
                    LineNumber = lineNumber,
                    TimePeriod = timePeriod,
                });
            }
            lineNumber += 10;
        }
        dto.AssyOutputDetails = details;
    }

    /// <summary>
    /// 收集组立日报明细混合生产桶键（生产日期 + 生产班组 + 生产时段）
    /// </summary>
    /// <param name="master">组立日报主表</param>
    /// <param name="details">明细列表</param>
    /// <returns>桶键集合</returns>
    private static HashSet<(string TeamCode, DateTime ProdDate, string TimePeriod)> CollectAssyOutputDetailMixedProdBuckets(
        TaktAssyOutput master,
        List<TaktAssyOutputDetail> details)
    {
        var buckets = new HashSet<(string TeamCode, DateTime ProdDate, string TimePeriod)>();
        if (string.IsNullOrWhiteSpace(master.TeamCode))
        {
            return buckets;
        }
        var prodDate = master.ProdDate.Date;
        foreach (var detail in details)
        {
            if (!string.IsNullOrWhiteSpace(detail.TimePeriod)
                && ShouldRefreshAssyMixedProdBucket(detail))
            {
                buckets.Add((master.TeamCode, prodDate, detail.TimePeriod));
            }
        }
        return buckets;
    }

    /// <summary>
    /// 是否需刷新混合生产桶（有产量/报工，或存在待清理的混合生产自动备注）
    /// </summary>
    /// <param name="detail">组立日报明细</param>
    /// <returns>需要刷新时为 true</returns>
    private static bool ShouldRefreshAssyMixedProdBucket(TaktAssyOutputDetail detail)
    {
        return !TaktProductionStatHelper.IsAssyDetailWithoutProduction(detail.ProdActualQty, detail.ConfirmMinutes)
            || TaktProductionStatHelper.IsAssyMixedProdAutoRemark(detail.Remark);
    }

    /// <summary>
    /// 批量刷新混合生产桶内明细派生字段
    /// </summary>
    /// <param name="buckets">桶键集合</param>
    /// <returns>任务</returns>
    private async Task RefreshAssyOutputDetailMixedProdBucketsAsync(
        IEnumerable<(string TeamCode, DateTime ProdDate, string TimePeriod)> buckets)
    {
        EnsureThreeLayerContext();
        foreach (var bucket in buckets.Distinct())
        {
            await TaktAssyOutputDetailDerivedFieldsHelper.RefreshMixedProdBucketAsync(
                _assyOutputRepository,
                _assyOutputDetailRepository,
                CurrentTenantCode,
                CurrentCompanyCode,
                bucket.TeamCode,
                bucket.ProdDate,
                bucket.TimePeriod);
        }
    }
    /// <summary>
    /// 组立日报保存后刷新工单/批量不良统计（不自动生成组立不良日报）
    /// </summary>
    /// <param name="output">组立日报主表</param>
    /// <returns>任务</returns>
    private async Task SyncDefectFromOutputAsync(TaktAssyOutput output)
    {
        EnsureThreeLayerContext();
        await TaktAssyOutputDefectSyncHelper.SyncFromAssyOutputAsync(
            _assyOutputRepository,
            _assyOutputDetailRepository,
            _assyDefectRepository,
            _assyDefectDetailRepository,
            _assyOrderDefectRepository,
            _assyBatchDefectRepository,
            output,
            CurrentTenantCode,
            CurrentCompanyCode);
    }

    /// <summary>
    /// 组立日报删除时刷新工单/批量不良统计（不删除手工维护的组立不良日报）
    /// </summary>
    /// <param name="output">组立日报快照</param>
    /// <returns>任务</returns>
    private async Task DeleteDefectForOutputAsync(TaktAssyOutput output)
    {
        EnsureThreeLayerContext();
        await TaktAssyOutputDefectSyncHelper.DeleteDefectForAssyOutputAsync(
            _assyOutputRepository,
            _assyOutputDetailRepository,
            _assyDefectRepository,
            _assyDefectDetailRepository,
            _assyOrderDefectRepository,
            _assyBatchDefectRepository,
            output,
            CurrentTenantCode,
            CurrentCompanyCode);
    }

    /// <summary>
    /// 刷新组立日报全部明细涉及的生产切换桶
    /// </summary>
    /// <param name="master">组立日报主表</param>
    /// <returns>任务</returns>
    private async Task RefreshAssyOutputChangeoverBucketsForOutputAsync(TaktAssyOutput master)
    {
        var details = await _assyOutputDetailRepository.GetListAsync(x => x.AssyOutputId == master.Id);
        await RefreshAssyOutputChangeoverBucketsAsync(CollectAssyOutputDetailChangeoverBuckets(master, details));
    }

    /// <summary>
    /// 批量刷新生产切换桶
    /// </summary>
    /// <param name="buckets">桶键集合</param>
    /// <returns>任务</returns>
    private async Task RefreshAssyOutputChangeoverBucketsAsync(
        IEnumerable<(string TeamCode, DateTime ProdDate, string TimePeriod)> buckets)
    {
        EnsureThreeLayerContext();
        foreach (var bucket in buckets.Distinct())
        {
            await TaktAssyOutputProductionChangeoverSyncHelper.RefreshBucketAsync(
                _assyOutputRepository,
                _assyOutputDetailRepository,
                _productionChangeoverRepository,
                _productionOrderRepository,
                CurrentTenantCode,
                CurrentCompanyCode,
                bucket.TeamCode,
                bucket.ProdDate,
                bucket.TimePeriod);
        }
    }

    /// <summary>
    /// 收集组立日报明细生产切换桶键（生产日期 + 生产班组 + 生产时段）
    /// </summary>
    /// <param name="master">组立日报主表</param>
    /// <param name="details">明细列表</param>
    /// <returns>桶键集合</returns>
    private static HashSet<(string TeamCode, DateTime ProdDate, string TimePeriod)> CollectAssyOutputDetailChangeoverBuckets(
        TaktAssyOutput master,
        List<TaktAssyOutputDetail> details)
    {
        var buckets = new HashSet<(string TeamCode, DateTime ProdDate, string TimePeriod)>();
        if (string.IsNullOrWhiteSpace(master.TeamCode))
        {
            return buckets;
        }
        var prodDate = master.ProdDate.Date;
        foreach (var detail in details)
        {
            if (!string.IsNullOrWhiteSpace(detail.TimePeriod))
            {
                buckets.Add((master.TeamCode, prodDate, detail.TimePeriod.Trim()));
            }
        }
        return buckets;
    }

    /// <summary>
    /// 判断组立日报与不良日报对齐用的自然键是否相同
    /// </summary>
    /// <param name="before">变更前</param>
    /// <param name="after">变更后</param>
    /// <returns>自然键相同为 true</returns>
    private static bool IsSameOutputDefectKey(TaktAssyOutput before, TaktAssyOutput after)
    {
        return TaktOutputOrderUniqueHelper.IsSameDailyOrderKey(
            before.ProdDate,
            before.ProdOrderCode,
            after.ProdDate,
            after.ProdOrderCode);
    }

    /// <summary>
    /// 组立不良日报与组立日报对齐用的「生产日期+工单号」复合键
    /// </summary>
    /// <param name="prodDate">生产日期</param>
    /// <param name="prodOrderCode">工单号</param>
    /// <returns>复合键</returns>
    private static string BuildAssyDefectDailyOrderKey(DateTime prodDate, string prodOrderCode)
    {
        return $"{prodDate.Date:yyyy-MM-dd}|{prodOrderCode}";
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建组立日报查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktAssyOutput, bool>> QueryExpression(TaktAssyOutputQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktAssyOutput>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ProdCategory != null && x.ProdCategory.Contains(keywords))
                || (x.TeamCode != null && x.TeamCode.Contains(keywords))
                || SqlFunc.ToString(x.DirectLabor).Contains(keywords)
                || SqlFunc.ToString(x.IndirectLabor).Contains(keywords)
                || SqlFunc.ToString(x.ShiftNo).Contains(keywords)
                || (x.ProdOrderType != null && x.ProdOrderType.Contains(keywords))
                || (x.ProdOrderCode != null && x.ProdOrderCode.Contains(keywords))
                || (x.ModelCode != null && x.ModelCode.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.BatchCode != null && x.BatchCode.Contains(keywords))
                || SqlFunc.ToString(x.ProdOrderQty).Contains(keywords)
                || (x.SerialCode != null && x.SerialCode.Contains(keywords))
                || SqlFunc.ToString(x.StdMinutes).Contains(keywords)
                || SqlFunc.ToString(x.StdCapacity).Contains(keywords)
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

        if (!string.IsNullOrEmpty(queryDto?.TeamCode))
        {
            exp = exp.And(x => x.TeamCode != null && x.TeamCode.Contains(queryDto.TeamCode));
        }

        if (queryDto?.DirectLabor.HasValue == true)
        {
            exp = exp.And(x => x.DirectLabor == queryDto.DirectLabor);
        }

        if (queryDto?.IndirectLabor.HasValue == true)
        {
            exp = exp.And(x => x.IndirectLabor == queryDto.IndirectLabor);
        }

        if (queryDto?.ShiftNo.HasValue == true)
        {
            exp = exp.And(x => x.ShiftNo == queryDto.ShiftNo);
        }

        if (!string.IsNullOrEmpty(queryDto?.ProdOrderType))
        {
            exp = exp.And(x => x.ProdOrderType != null && x.ProdOrderType.Contains(queryDto.ProdOrderType));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProdOrderCode))
        {
            exp = exp.And(x => x.ProdOrderCode != null && x.ProdOrderCode.Contains(queryDto.ProdOrderCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ModelCode))
        {
            exp = exp.And(x => x.ModelCode != null && x.ModelCode.Contains(queryDto.ModelCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialCode))
        {
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(queryDto.MaterialCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.BatchCode))
        {
            exp = exp.And(x => x.BatchCode != null && x.BatchCode.Contains(queryDto.BatchCode));
        }

        if (queryDto?.ProdOrderQty.HasValue == true)
        {
            exp = exp.And(x => x.ProdOrderQty == queryDto.ProdOrderQty);
        }

        if (!string.IsNullOrEmpty(queryDto?.SerialCode))
        {
            exp = exp.And(x => x.SerialCode != null && x.SerialCode.Contains(queryDto.SerialCode));
        }

        if (queryDto?.StdMinutes.HasValue == true)
        {
            exp = exp.And(x => x.StdMinutes == queryDto.StdMinutes);
        }

        if (queryDto?.StdCapacity.HasValue == true)
        {
            exp = exp.And(x => x.StdCapacity == queryDto.StdCapacity);
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

    // ========================================
    // 扩展方法（数据看板）
    // ========================================

    /// <summary>
    /// 获取组立生产统计（数据看板 production-stat；按生产日期）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>组立生产统计</returns>
    public async Task<TaktAssyOutputProductionStatDto> GetAssyOutputProductionStatAsync(TaktOutputProductionStatQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        EnsureThreeLayerContext();
        var (start, end, statMonth) = TaktStatMonthRangeHelper.ResolveMonthRange(
            queryDto.ProdDateStart,
            queryDto.ProdDateEnd);
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;
        Expression<Func<TaktAssyOutput, bool>> headerPredicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.ProdDate >= start
            && x.ProdDate <= end;
        var headers = await _assyOutputRepository.GetListAsync(headerPredicate);
        var outputIds = headers.Select(h => h.Id).ToList();
        var details = outputIds.Count == 0
            ? new List<TaktAssyOutputDetail>()
            : await _assyOutputDetailRepository.GetListAsync(x =>
                x.TenantCode == tenantCode
                && x.CompanyCode == companyCode
                && x.IsObsolete == 0
                && outputIds.Contains(x.AssyOutputId));
        var qtyByOutputId = details
            .GroupBy(d => d.AssyOutputId)
            .ToDictionary(g => g.Key, g => g.Sum(d => d.ProdActualQty));
        var teams = headers
            .GroupBy(h => h.TeamCode ?? string.Empty, StringComparer.Ordinal)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .Select(g =>
            {
                var plan = g.Sum(h => h.StdCapacity);
                var actual = g.Sum(h => qtyByOutputId.TryGetValue(h.Id, out var qty) ? qty : 0m);
                return new TaktOutputProductionStatTeamItemDto
                {
                    TeamCode = g.Key,
                    StdCapacity = plan,
                    ProdActualQty = actual,
                    AchievementRate = TaktProductionStatHelper.CalculateAchievementRatePercent(actual, plan),
                };
            })
            .ToList();
        var monthStdCapacity = teams.Sum(t => t.StdCapacity);
        var monthProdActualQty = teams.Sum(t => t.ProdActualQty);
        var monthDowntimeMinutes = details.Sum(d => (decimal)d.DowntimeMinutes);
        var monthInputMinutes = details.Sum(d => d.InputMinutes);
        var monthProdMinutes = details.Sum(d => d.ConfirmMinutes);
        var monthActualMinutes = details.Sum(d => d.ActualMinutes);
        return new TaktAssyOutputProductionStatDto
        {
            StatMonth = statMonth,
            MonthStdCapacity = monthStdCapacity,
            MonthProdActualQty = monthProdActualQty,
            MonthAchievementRate = TaktProductionStatHelper.CalculateAchievementRatePercent(monthProdActualQty, monthStdCapacity),
            MonthDowntimeMinutes = monthDowntimeMinutes,
            MonthInputMinutes = monthInputMinutes,
            MonthProdMinutes = monthProdMinutes,
            MonthActualMinutes = monthActualMinutes,
            Teams = teams,
        };
    }
}
