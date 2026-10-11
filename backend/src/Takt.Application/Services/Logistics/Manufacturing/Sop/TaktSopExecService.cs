// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Sop
// 文件名称：TaktSopExecService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP工位执行应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Sop;
using Takt.Domain.Entities.Logistics.Manufacturing.Sop;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Sop;

/// <summary>
/// SOP工位执行应用服务
/// </summary>
public class TaktSopExecService : TaktServiceBase, ITaktSopExecService
{
    private readonly ITaktCompanyRepository<TaktSopExec> _sopExecRepository;
    private readonly ITaktCompanyRepository<TaktSopExecStep> _sopExecStepRepository;
    private readonly ITaktCompanyRepository<TaktSopExecScan> _sopExecScanRepository;
    private readonly ITaktCompanyRepository<TaktSopArgument> _sopArgumentRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sopExecRepository">SOP工位执行仓储</param>
    /// <param name="sopExecStepRepository">SopExecStep仓储</param>
    /// <param name="sopExecScanRepository">SopExecScan仓储</param>
    /// <param name="sopArgumentRepository">SopArgument仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSopExecService(
        ITaktCompanyRepository<TaktSopExec> sopExecRepository,
        ITaktCompanyRepository<TaktSopExecStep> sopExecStepRepository,
        ITaktCompanyRepository<TaktSopExecScan> sopExecScanRepository,
        ITaktCompanyRepository<TaktSopArgument> sopArgumentRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _sopExecRepository = sopExecRepository;
        _sopExecStepRepository = sopExecStepRepository;
        _sopExecScanRepository = sopExecScanRepository;
        _sopArgumentRepository = sopArgumentRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取SOP工位执行列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSopExecDto>> GetSopExecListAsync(TaktSopExecQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktSopExecDto>.Create(
                new List<TaktSopExecDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _sopExecRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSopExecDto>.Create(
            data.Adapt<List<TaktSopExecDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取SOP工位执行
    /// </summary>
    /// <param name="id">SOP工位执行ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopExecDto?> GetSopExecByIdAsync(long id)
    {
        var entity = await _sopExecRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktSopExecDto>();
        await FillSopExecDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取SOP工位执行选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSopExecOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _sopExecRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ExecStatus == 1,
            x => x.WorkOrderCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.WorkOrderCode,
            DictLabel = e.WorkOrderCode,
        }).ToList();
    }

    /// <summary>
    /// 创建SOP工位执行
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopExecDto> CreateSopExecAsync(TaktSopExecCreateDto dto)
    {
        var entity = dto.Adapt<TaktSopExec>();
        entity = await _sopExecRepository.CreateAsync(entity);
                await SaveSopExecChildrenAsync(entity, dto);
        return await GetSopExecByIdAsync(entity.Id) ?? entity.Adapt<TaktSopExecDto>();
    }

    /// <summary>
    /// 更新SOP工位执行
    /// </summary>
    /// <param name="id">SOP工位执行ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopExecDto> UpdateSopExecAsync(long id, TaktSopExecUpdateDto dto)
    {
        var entity = await _sopExecRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("SOP工位执行不存在");
        }
        dto.Adapt(entity);
        await _sopExecRepository.UpdateAsync(entity);
                await SaveSopExecChildrenAsync(entity, dto);
        return await GetSopExecByIdAsync(id) ?? throw new TaktBusinessException("SOP工位执行不存在");
    }

    /// <summary>
    /// 删除SOP工位执行
    /// </summary>
    /// <param name="id">SOP工位执行ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSopExecByIdAsync(long id)
    {
        var entity = await _sopExecRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("SOP工位执行不存在或已删除");
        }
        await _sopExecStepRepository.DeleteAsync(x => x.ExecId == entity.Id);
        await _sopExecScanRepository.DeleteAsync(x => x.ExecId == entity.Id);
        await _sopArgumentRepository.DeleteAsync(x => x.ExecId == entity.Id);
        var deleted = await _sopExecRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("SOP工位执行不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除SOP工位执行
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSopExecBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSopExecByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新SOP工位执行状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopExecDto> UpdateSopExecStatusAsync(TaktSopExecStatusDto dto)
    {
        var entity = await _sopExecRepository.GetByIdAsync(dto.SopExecId);
        if (entity == null)
        {
            throw new TaktBusinessException("SOP工位执行不存在");
        }
        entity.ExecStatus = dto.ExecStatus;
        await _sopExecRepository.UpdateAsync(entity);
        return await GetSopExecByIdAsync(dto.SopExecId) ?? throw new TaktBusinessException("SOP工位执行不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSopExecTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSopExecTemplateDto>(
            sheetName ?? "SOP工位执行导入模板",
            fileName ?? "SOP工位执行导入模板.xlsx");
    }

    /// <summary>
    /// 导入SOP工位执行
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSopExecAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSopExecImportDto>(fileStream, sheetName ?? "SOP工位执行导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktSopExec>();
                await _sopExecRepository.CreateAsync(entity);
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
    /// 导出SOP工位执行
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSopExecAsync(TaktSopExecQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktSopExecQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSopExecExportDto>(),
                sheetName ?? "SOP工位执行数据",
                fileName ?? "SOP工位执行导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _sopExecRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSopExecExportDto>(),
                sheetName ?? "SOP工位执行数据",
                fileName ?? "SOP工位执行导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSopExecExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "SOP工位执行数据",
            fileName ?? "SOP工位执行导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充SOP工位执行详情（加载 OneToMany 子表：SOP工步执行明细、SOP物料扫码记录、SOP作业参数）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillSopExecDetailsAsync(TaktSopExecDto dto, TaktSopExec entity)
    {
        if (dto == null)
        {
            return;
        }
        // SOP工步执行明细 → dto.Steps
        var steps = await _sopExecStepRepository.GetListAsync(x => x.ExecId == entity.Id);
        dto.Steps = steps.Adapt<List<TaktSopExecStepDto>>();
        // SOP物料扫码记录 → dto.Scans
        var scans = await _sopExecScanRepository.GetListAsync(x => x.ExecId == entity.Id);
        dto.Scans = scans.Adapt<List<TaktSopExecScanDto>>();
        // SOP作业参数 → dto.Arguments
        var arguments = await _sopArgumentRepository.GetListAsync(x => x.ExecId == entity.Id);
        dto.Arguments = arguments.Adapt<List<TaktSopArgumentDto>>();
    }

    /// <summary>
    /// 保存SOP工位执行子表级联（SOP工步执行明细、SOP物料扫码记录、SOP作业参数；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveSopExecChildrenAsync(TaktSopExec entity, TaktSopExecCreateDto dto)
    {
        // SOP工步执行明细（Steps）
        List<TaktSopExecStepUpdateDto>? stepsForSave;
        if (dto is TaktSopExecUpdateDto updateDtoForSteps && updateDtoForSteps.Steps != null)
        {
            stepsForSave = updateDtoForSteps.Steps;
        }
        else if (dto.Steps != null)
        {
            stepsForSave = dto.Steps.Adapt<List<TaktSopExecStepUpdateDto>>();
        }
        else
        {
            stepsForSave = null;
        }
        if (stepsForSave is not { Count: > 0 })
        {
            await _sopExecStepRepository.DeleteAsync(x => x.ExecId == entity.Id);
        }
        else
        {
            var existingList = await _sopExecStepRepository.GetListAsync(x => x.ExecId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktSopExecStep>();
            for (var i = 0; i < stepsForSave.Count; i++)
            {
                var childDto = stepsForSave[i];
                childDto.ExecId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                if (childDto.SopExecStepId > 0)
                {
                    if (!existingById.TryGetValue(childDto.SopExecStepId, out var target))
                    {
                        throw new TaktBusinessException("SOP工步执行明细不存在（SopExecStepId={childDto.SopExecStepId}）");
                    }
                    if (target.ExecId != entity.Id)
                    {
                        throw new TaktBusinessException("SOP工步执行明细不属于当前主表（SopExecStepId={childDto.SopExecStepId}）");
                    }
                    submittedIds.Add(childDto.SopExecStepId);
                    childDto.Adapt(target);
                    target.Id = childDto.SopExecStepId;
                    target.ExecId = entity.Id;
                    await _sopExecStepRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktSopExecStep>();
                    child.Id = 0;
                    child.ExecId = entity.Id;
                    toCreate.Add(child);
                }
            }
            foreach (var removed in existingList.Where(x => !submittedIds.Contains(x.Id)))
            {
                await _sopExecStepRepository.DeleteAsync(removed.Id);
            }
            if (toCreate.Count > 0)
            {
                await _sopExecStepRepository.CreateRangeAsync(toCreate);
            }
        }
        // SOP物料扫码记录（Scans）
        List<TaktSopExecScanUpdateDto>? scansForSave;
        if (dto is TaktSopExecUpdateDto updateDtoForScans && updateDtoForScans.Scans != null)
        {
            scansForSave = updateDtoForScans.Scans;
        }
        else if (dto.Scans != null)
        {
            scansForSave = dto.Scans.Adapt<List<TaktSopExecScanUpdateDto>>();
        }
        else
        {
            scansForSave = null;
        }
        if (scansForSave is not { Count: > 0 })
        {
            await _sopExecScanRepository.DeleteAsync(x => x.ExecId == entity.Id);
        }
        else
        {
            var existingList = await _sopExecScanRepository.GetListAsync(x => x.ExecId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktSopExecScan>();
            for (var i = 0; i < scansForSave.Count; i++)
            {
                var childDto = scansForSave[i];
                childDto.ExecId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                if (childDto.SopExecScanId > 0)
                {
                    if (!existingById.TryGetValue(childDto.SopExecScanId, out var target))
                    {
                        throw new TaktBusinessException("SOP物料扫码记录不存在（SopExecScanId={childDto.SopExecScanId}）");
                    }
                    if (target.ExecId != entity.Id)
                    {
                        throw new TaktBusinessException("SOP物料扫码记录不属于当前主表（SopExecScanId={childDto.SopExecScanId}）");
                    }
                    submittedIds.Add(childDto.SopExecScanId);
                    childDto.Adapt(target);
                    target.Id = childDto.SopExecScanId;
                    target.ExecId = entity.Id;
                    await _sopExecScanRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktSopExecScan>();
                    child.Id = 0;
                    child.ExecId = entity.Id;
                    toCreate.Add(child);
                }
            }
            foreach (var removed in existingList.Where(x => !submittedIds.Contains(x.Id)))
            {
                await _sopExecScanRepository.DeleteAsync(removed.Id);
            }
            if (toCreate.Count > 0)
            {
                await _sopExecScanRepository.CreateRangeAsync(toCreate);
            }
        }
        // SOP作业参数（Arguments）
        List<TaktSopArgumentUpdateDto>? argumentsForSave;
        if (dto is TaktSopExecUpdateDto updateDtoForArguments && updateDtoForArguments.Arguments != null)
        {
            argumentsForSave = updateDtoForArguments.Arguments;
        }
        else if (dto.Arguments != null)
        {
            argumentsForSave = dto.Arguments.Adapt<List<TaktSopArgumentUpdateDto>>();
        }
        else
        {
            argumentsForSave = null;
        }
        if (argumentsForSave is not { Count: > 0 })
        {
            await _sopArgumentRepository.DeleteAsync(x => x.ExecId == entity.Id);
        }
        else
        {
            var existingList = await _sopArgumentRepository.GetListAsync(x => x.ExecId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktSopArgument>();
            for (var i = 0; i < argumentsForSave.Count; i++)
            {
                var childDto = argumentsForSave[i];
                childDto.ExecId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                if (childDto.SopArgumentId > 0)
                {
                    if (!existingById.TryGetValue(childDto.SopArgumentId, out var target))
                    {
                        throw new TaktBusinessException("SOP作业参数不存在（SopArgumentId={childDto.SopArgumentId}）");
                    }
                    if (target.ExecId != entity.Id)
                    {
                        throw new TaktBusinessException("SOP作业参数不属于当前主表（SopArgumentId={childDto.SopArgumentId}）");
                    }
                    submittedIds.Add(childDto.SopArgumentId);
                    childDto.Adapt(target);
                    target.Id = childDto.SopArgumentId;
                    target.ExecId = entity.Id;
                    await _sopArgumentRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktSopArgument>();
                    child.Id = 0;
                    child.ExecId = entity.Id;
                    toCreate.Add(child);
                }
            }
            foreach (var removed in existingList.Where(x => !submittedIds.Contains(x.Id)))
            {
                await _sopArgumentRepository.DeleteAsync(removed.Id);
            }
            if (toCreate.Count > 0)
            {
                await _sopArgumentRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建SOP工位执行查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSopExec, bool>> QueryExpression(TaktSopExecQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSopExec>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.WorkOrderCode != null && x.WorkOrderCode.Contains(keywords))
                || (x.SerialNumber != null && x.SerialNumber.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.Revision != null && x.Revision.Contains(keywords))
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

        if (queryDto?.ProductionOrderId.HasValue == true)
        {
            var productionOrderId = queryDto.ProductionOrderId.Value;
            exp = exp.And(x => x.ProductionOrderId == productionOrderId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.WorkOrderCode))
        {
            var workOrderCode = queryDto.WorkOrderCode;
            exp = exp.And(x => x.WorkOrderCode != null && x.WorkOrderCode.Contains(workOrderCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SerialNumber))
        {
            var serialNumber = queryDto.SerialNumber;
            exp = exp.And(x => x.SerialNumber != null && x.SerialNumber.Contains(serialNumber));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialCode))
        {
            var materialCode = queryDto.MaterialCode;
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(materialCode));
        }

        if (queryDto?.RoutingItemId.HasValue == true)
        {
            var routingItemId = queryDto.RoutingItemId.Value;
            exp = exp.And(x => x.RoutingItemId == routingItemId);
        }

        if (queryDto?.ProcessSegmentType.HasValue == true)
        {
            var processSegmentType = queryDto.ProcessSegmentType.Value;
            exp = exp.And(x => x.ProcessSegmentType == processSegmentType);
        }

        if (queryDto?.WorkstationId.HasValue == true)
        {
            var workstationId = queryDto.WorkstationId.Value;
            exp = exp.And(x => x.WorkstationId == workstationId);
        }

        if (queryDto?.EmployeeId.HasValue == true)
        {
            var employeeId = queryDto.EmployeeId.Value;
            exp = exp.And(x => x.EmployeeId == employeeId);
        }

        if (queryDto?.SopId.HasValue == true)
        {
            var sopId = queryDto.SopId.Value;
            exp = exp.And(x => x.SopId == sopId);
        }

        if (queryDto?.RevisionId.HasValue == true)
        {
            var revisionId = queryDto.RevisionId.Value;
            exp = exp.And(x => x.RevisionId == revisionId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Revision))
        {
            var revision = queryDto.Revision;
            exp = exp.And(x => x.Revision != null && x.Revision.Contains(revision));
        }

        if (queryDto?.SelfCheckResult.HasValue == true)
        {
            var selfCheckResult = queryDto.SelfCheckResult.Value;
            exp = exp.And(x => x.SelfCheckResult == selfCheckResult);
        }

        if (queryDto?.ExecStatus.HasValue == true)
        {
            var execStatus = queryDto.ExecStatus.Value;
            exp = exp.And(x => x.ExecStatus == execStatus);
        }

        if (queryDto?.CurrentStepId.HasValue == true)
        {
            var currentStepId = queryDto.CurrentStepId.Value;
            exp = exp.And(x => x.CurrentStepId == currentStepId);
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

        if (queryDto?.StartedAtStart.HasValue == true)
        {
            var startedAtStart = queryDto.StartedAtStart.Value;
            exp = exp.And(x => x.StartedAt >= startedAtStart);
        }

        if (queryDto?.StartedAtEnd.HasValue == true)
        {
            var startedAtEnd = queryDto.StartedAtEnd.Value;
            exp = exp.And(x => x.StartedAt <= startedAtEnd);
        }

        if (queryDto?.EndedAtStart.HasValue == true)
        {
            var endedAtStart = queryDto.EndedAtStart.Value;
            exp = exp.And(x => x.EndedAt >= endedAtStart);
        }

        if (queryDto?.EndedAtEnd.HasValue == true)
        {
            var endedAtEnd = queryDto.EndedAtEnd.Value;
            exp = exp.And(x => x.EndedAt <= endedAtEnd);
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
    private static bool HasAnyListQueryFilter(TaktSopExecQueryDto? queryDto)
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
        if (queryDto.ProductionOrderId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.WorkOrderCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SerialNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialCode))
        {
            return true;
        }
        if (queryDto.RoutingItemId.HasValue)
        {
            return true;
        }
        if (queryDto.ProcessSegmentType.HasValue)
        {
            return true;
        }
        if (queryDto.WorkstationId.HasValue)
        {
            return true;
        }
        if (queryDto.EmployeeId.HasValue)
        {
            return true;
        }
        if (queryDto.SopId.HasValue)
        {
            return true;
        }
        if (queryDto.RevisionId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Revision))
        {
            return true;
        }
        if (queryDto.SelfCheckResult.HasValue)
        {
            return true;
        }
        if (queryDto.ExecStatus.HasValue)
        {
            return true;
        }
        if (queryDto.CurrentStepId.HasValue)
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
        if (queryDto.StartedAtStart.HasValue || queryDto.StartedAtEnd.HasValue)
        {
            return true;
        }
        if (queryDto.EndedAtStart.HasValue || queryDto.EndedAtEnd.HasValue)
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
