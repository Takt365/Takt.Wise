// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Mps
// 文件名称：TaktMasterProductionScheduleService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：主生产计划MPS头应用服务实现
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
/// 主生产计划MPS头应用服务
/// </summary>
public class TaktMasterProductionScheduleService : TaktServiceBase, ITaktMasterProductionScheduleService
{
    private readonly ITaktApprovalRepository<TaktMasterProductionSchedule> _masterProductionScheduleRepository;
    private readonly ITaktCompanyRepository<TaktMasterProductionScheduleLine> _masterProductionScheduleLineRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="masterProductionScheduleRepository">主生产计划MPS头仓储</param>
    /// <param name="masterProductionScheduleLineRepository">MasterProductionScheduleLine仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMasterProductionScheduleService(
        ITaktApprovalRepository<TaktMasterProductionSchedule> masterProductionScheduleRepository,
        ITaktCompanyRepository<TaktMasterProductionScheduleLine> masterProductionScheduleLineRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _masterProductionScheduleRepository = masterProductionScheduleRepository;
        _masterProductionScheduleLineRepository = masterProductionScheduleLineRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取主生产计划MPS头列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktMasterProductionScheduleDto>> GetMasterProductionScheduleListAsync(TaktMasterProductionScheduleQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktMasterProductionScheduleDto>.Create(
                new List<TaktMasterProductionScheduleDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _masterProductionScheduleRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktMasterProductionScheduleDto>.Create(
            data.Adapt<List<TaktMasterProductionScheduleDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取主生产计划MPS头
    /// </summary>
    /// <param name="id">主生产计划MPS头ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktMasterProductionScheduleDto?> GetMasterProductionScheduleByIdAsync(long id)
    {
        var entity = await _masterProductionScheduleRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktMasterProductionScheduleDto>();
        await FillMasterProductionScheduleDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取主生产计划MPS头选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetMasterProductionScheduleOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _masterProductionScheduleRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ScheduleStatus == 1,
            x => x.MpsCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.MpsCode,
            DictLabel = e.MpsCode,
        }).ToList();
    }

    /// <summary>
    /// 创建主生产计划MPS头
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMasterProductionScheduleDto> CreateMasterProductionScheduleAsync(TaktMasterProductionScheduleCreateDto dto)
    {
        var entity = dto.Adapt<TaktMasterProductionSchedule>();
        var isUnique_ix_takt_logistics_manufacturing_mps_master_production_schedule_unique = await _uniqueValidator.IsUniqueAsync(
            _masterProductionScheduleRepository,
            x => x.PlantCode == entity.PlantCode
                && x.MpsCode == entity.MpsCode);
        if (!isUnique_ix_takt_logistics_manufacturing_mps_master_production_schedule_unique)
        {
            throw new TaktBusinessException("主生产计划MPS头的PlantCode、MpsCode已存在");
        }
        entity = await _masterProductionScheduleRepository.CreateAsync(entity);
                await SaveMasterProductionScheduleChildrenAsync(entity, dto);
        return await GetMasterProductionScheduleByIdAsync(entity.Id) ?? entity.Adapt<TaktMasterProductionScheduleDto>();
    }

    /// <summary>
    /// 更新主生产计划MPS头
    /// </summary>
    /// <param name="id">主生产计划MPS头ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMasterProductionScheduleDto> UpdateMasterProductionScheduleAsync(long id, TaktMasterProductionScheduleUpdateDto dto)
    {
        var entity = await _masterProductionScheduleRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("主生产计划MPS头不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_mps_master_production_schedule_unique = await _uniqueValidator.IsUniqueAsync(
            _masterProductionScheduleRepository,
            x => x.PlantCode == entity.PlantCode
                && x.MpsCode == entity.MpsCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_mps_master_production_schedule_unique)
        {
            throw new TaktBusinessException("主生产计划MPS头的PlantCode、MpsCode已存在");
        }
        await _masterProductionScheduleRepository.UpdateAsync(entity);
                await SaveMasterProductionScheduleChildrenAsync(entity, dto);
        return await GetMasterProductionScheduleByIdAsync(id) ?? throw new TaktBusinessException("主生产计划MPS头不存在");
    }

    /// <summary>
    /// 删除主生产计划MPS头
    /// </summary>
    /// <param name="id">主生产计划MPS头ID</param>
    /// <returns>任务</returns>
    public async Task DeleteMasterProductionScheduleByIdAsync(long id)
    {
        var entity = await _masterProductionScheduleRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("主生产计划MPS头不存在或已删除");
        }
        await _masterProductionScheduleLineRepository.DeleteAsync(x => x.MasterProductionScheduleId == entity.Id);
        var deleted = await _masterProductionScheduleRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("主生产计划MPS头不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除主生产计划MPS头
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteMasterProductionScheduleBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteMasterProductionScheduleByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新主生产计划MPS头状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMasterProductionScheduleDto> UpdateMasterProductionScheduleStatusAsync(TaktMasterProductionScheduleStatusDto dto)
    {
        var entity = await _masterProductionScheduleRepository.GetByIdAsync(dto.MasterProductionScheduleId);
        if (entity == null)
        {
            throw new TaktBusinessException("主生产计划MPS头不存在");
        }
        entity.ScheduleStatus = dto.ScheduleStatus;
        await _masterProductionScheduleRepository.UpdateAsync(entity);
        return await GetMasterProductionScheduleByIdAsync(dto.MasterProductionScheduleId) ?? throw new TaktBusinessException("主生产计划MPS头不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetMasterProductionScheduleTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktMasterProductionScheduleTemplateDto>(
            sheetName ?? "主生产计划MPS头导入模板",
            fileName ?? "主生产计划MPS头导入模板.xlsx");
    }

    /// <summary>
    /// 导入主生产计划MPS头
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportMasterProductionScheduleAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktMasterProductionScheduleImportDto>(fileStream, sheetName ?? "主生产计划MPS头导入模板");
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
                var entity = rows[i].Adapt<TaktMasterProductionSchedule>();
                var importKey = $"{entity.PlantCode}|{entity.MpsCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、MpsCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_mps_master_production_schedule_unique = await _uniqueValidator.IsUniqueAsync(
                    _masterProductionScheduleRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.MpsCode == entity.MpsCode);
                if (!isUnique_ix_takt_logistics_manufacturing_mps_master_production_schedule_unique)
                {
                    throw new TaktBusinessException("主生产计划MPS头的PlantCode、MpsCode已存在");
                }
                await _masterProductionScheduleRepository.CreateAsync(entity);
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
    /// 导出主生产计划MPS头
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportMasterProductionScheduleAsync(TaktMasterProductionScheduleQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktMasterProductionScheduleQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMasterProductionScheduleExportDto>(),
                sheetName ?? "主生产计划MPS头数据",
                fileName ?? "主生产计划MPS头导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _masterProductionScheduleRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMasterProductionScheduleExportDto>(),
                sheetName ?? "主生产计划MPS头数据",
                fileName ?? "主生产计划MPS头导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktMasterProductionScheduleExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "主生产计划MPS头数据",
            fileName ?? "主生产计划MPS头导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废主生产计划MPS行标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="masterProductionScheduleId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkMasterProductionScheduleLinesObsoleteAsync(long masterProductionScheduleId)
    {
        if (masterProductionScheduleId <= 0)
        {
            return;
        }
        var rows = await _masterProductionScheduleLineRepository.GetListAsync(
            x => x.MasterProductionScheduleId == masterProductionScheduleId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _masterProductionScheduleLineRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充主生产计划MPS头详情（加载 OneToMany 子表：主生产计划MPS行）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillMasterProductionScheduleDetailsAsync(TaktMasterProductionScheduleDto dto, TaktMasterProductionSchedule entity)
    {
        if (dto == null)
        {
            return;
        }
        // 主生产计划MPS行 → dto.Lines（含作废行）
        var lines = await _masterProductionScheduleLineRepository.GetListAsync(x => x.MasterProductionScheduleId == entity.Id);
        dto.Lines = lines.Adapt<List<TaktMasterProductionScheduleLineDto>>();
    }

    /// <summary>
    /// 保存主生产计划MPS头子表级联（主生产计划MPS行；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveMasterProductionScheduleChildrenAsync(TaktMasterProductionSchedule entity, TaktMasterProductionScheduleCreateDto dto)
    {
        // 主生产计划MPS行（Lines）
        List<TaktMasterProductionScheduleLineUpdateDto>? linesForSave;
        if (dto is TaktMasterProductionScheduleUpdateDto updateDtoForLines && updateDtoForLines.Lines != null)
        {
            linesForSave = updateDtoForLines.Lines;
        }
        else if (dto.Lines != null)
        {
            linesForSave = dto.Lines.Adapt<List<TaktMasterProductionScheduleLineUpdateDto>>();
        }
        else
        {
            linesForSave = null;
        }
        if (linesForSave is not { Count: > 0 })
        {
            await MarkMasterProductionScheduleLinesObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _masterProductionScheduleLineRepository.GetListAsync(x => x.MasterProductionScheduleId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktMasterProductionScheduleLine>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < linesForSave.Count; i++)
            {
                var childDto = linesForSave[i];
                childDto.MasterProductionScheduleId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.MpsCode = entity.MpsCode;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("主生产计划MPS行第{i + 1}项与本次提交的其他项重复（CompanyCode、MasterProductionScheduleId、LineNumber）");
                }
                if (childDto.MasterProductionScheduleLineId > 0)
                {
                    if (!existingById.TryGetValue(childDto.MasterProductionScheduleLineId, out var target))
                    {
                        throw new TaktBusinessException("主生产计划MPS行不存在（MasterProductionScheduleLineId={childDto.MasterProductionScheduleLineId}）");
                    }
                    if (target.MasterProductionScheduleId != entity.Id)
                    {
                        throw new TaktBusinessException("主生产计划MPS行不属于当前主表（MasterProductionScheduleLineId={childDto.MasterProductionScheduleLineId}）");
                    }
                    submittedIds.Add(childDto.MasterProductionScheduleLineId);
                    var isUniqueUpdate_ix_takt_logistics_manufacturing_mps_master_production_schedule_line_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _masterProductionScheduleLineRepository,
                        x => x.MasterProductionScheduleId == x.MasterProductionScheduleId
                && x.LineNumber == x.LineNumber
                && x.MaterialCode == x.MaterialCode,
                        childDto.MasterProductionScheduleLineId);
                    if (!isUniqueUpdate_ix_takt_logistics_manufacturing_mps_master_production_schedule_line_line_unique)
                    {
                        throw new TaktBusinessException("主生产计划MPS行的MasterProductionScheduleId、LineNumber、MaterialCode已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.MasterProductionScheduleLineId;
                    target.MasterProductionScheduleId = entity.Id;
                    target.IsObsolete = 0;
                    await _masterProductionScheduleLineRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_manufacturing_mps_master_production_schedule_line_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _masterProductionScheduleLineRepository,
                        x => x.MasterProductionScheduleId == x.MasterProductionScheduleId
                && x.LineNumber == x.LineNumber
                && x.MaterialCode == x.MaterialCode);
                    if (!isUniqueCreate_ix_takt_logistics_manufacturing_mps_master_production_schedule_line_line_unique)
                    {
                        throw new TaktBusinessException("主生产计划MPS行的MasterProductionScheduleId、LineNumber、MaterialCode已存在");
                    }
                    var child = childDto.Adapt<TaktMasterProductionScheduleLine>();
                    child.Id = 0;
                    child.MasterProductionScheduleId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _masterProductionScheduleLineRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.MpsCode) ? entity.MpsCode : entity.Id.ToString();
                    var maxLine = existingList.Count > 0 ? existingList.Max(x => x.LineNumber) : 0;
                    var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, needLine.Count, maxLine).ToList();
                    var lineIdx = 0;
                    foreach (var child in toCreate)
                    {
                        if (child.LineNumber <= 0)
                        {
                            child.LineNumber = lineSeq[lineIdx++];
                        }
                    }
                }
                await _masterProductionScheduleLineRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建主生产计划MPS头查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktMasterProductionSchedule, bool>> QueryExpression(TaktMasterProductionScheduleQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktMasterProductionSchedule>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.MpsCode != null && x.MpsCode.Contains(keywords))
                || (x.MdsCode != null && x.MdsCode.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.MpsCode))
        {
            var mpsCode = queryDto.MpsCode;
            exp = exp.And(x => x.MpsCode != null && x.MpsCode.Contains(mpsCode));
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

        if (queryDto?.BucketType.HasValue == true)
        {
            var bucketType = queryDto.BucketType.Value;
            exp = exp.And(x => x.BucketType == bucketType);
        }

        if (queryDto?.ScheduleStatus.HasValue == true)
        {
            var scheduleStatus = queryDto.ScheduleStatus.Value;
            exp = exp.And(x => x.ScheduleStatus == scheduleStatus);
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

        if (queryDto?.PlanPeriodStartStart.HasValue == true)
        {
            var planPeriodStartStart = queryDto.PlanPeriodStartStart.Value;
            exp = exp.And(x => x.PlanPeriodStart >= planPeriodStartStart);
        }

        if (queryDto?.PlanPeriodStartEnd.HasValue == true)
        {
            var planPeriodStartEnd = queryDto.PlanPeriodStartEnd.Value;
            exp = exp.And(x => x.PlanPeriodStart <= planPeriodStartEnd);
        }

        if (queryDto?.PlanPeriodEndStart.HasValue == true)
        {
            var planPeriodEndStart = queryDto.PlanPeriodEndStart.Value;
            exp = exp.And(x => x.PlanPeriodEnd >= planPeriodEndStart);
        }

        if (queryDto?.PlanPeriodEndEnd.HasValue == true)
        {
            var planPeriodEndEnd = queryDto.PlanPeriodEndEnd.Value;
            exp = exp.And(x => x.PlanPeriodEnd <= planPeriodEndEnd);
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
    private static bool HasAnyListQueryFilter(TaktMasterProductionScheduleQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.MpsCode))
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
        if (queryDto.BucketType.HasValue)
        {
            return true;
        }
        if (queryDto.ScheduleStatus.HasValue)
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
        if (queryDto.PlanPeriodStartStart.HasValue || queryDto.PlanPeriodStartEnd.HasValue)
        {
            return true;
        }
        if (queryDto.PlanPeriodEndStart.HasValue || queryDto.PlanPeriodEndEnd.HasValue)
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
