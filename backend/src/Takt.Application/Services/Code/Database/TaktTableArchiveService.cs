// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Code.Database
// 文件名称：TaktTableArchiveService.cs
// 创建时间：2026-07-19
// 创建人：Takt365(Cursor AI)
// 功能描述：数据表归档应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Code.Database;
using Takt.Application.Dtos.Foundation;
using Takt.Application.Services.Foundation;
using Takt.Domain.Entities.Code.Database;
using Takt.Domain.Entities.Statistics.Logging;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Models.Code;
using Takt.Shared.Options;

namespace Takt.Application.Services.Code.Database;

/// <summary>
/// 数据表归档应用服务
/// </summary>
public class TaktTableArchiveService : TaktServiceBase, ITaktTableArchiveService
{
    /// <summary>
    /// Quartz Handler 类名（与 TaktTableArchiveJobHandler.HandlerKey 一致）
    /// </summary>
    public const string QuartzHandlerClassName = "TaktTableArchiveJobHandler";

    private const int ExecuteModeImmediate = 1;
    private const int ExecuteModeBackground = 2;
    private const string QuartzAssemblyName = "Takt.Infrastructure";
    private const string QuartzJobGroup = "default";
    private const string QuartzTaskTypeAssembly = "assembly";

    private readonly ITaktCompanyRepository<TaktTableArchive> _tableArchiveRepository;
    private readonly ITaktCompanyRepository<TaktArchiveLog> _archiveLogRepository;
    private readonly ITaktTableArchiveProvider _tableArchiveProvider;
    private readonly ITaktDatabaseSchemaProvider _schemaProvider;
    private readonly ITaktQuartzTaskService _quartzTaskService;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="tableArchiveRepository">数据表归档仓储</param>
    /// <param name="archiveLogRepository">归档日志仓储</param>
    /// <param name="tableArchiveProvider">同库按年归档提供者</param>
    /// <param name="schemaProvider">数据库元数据提供者（解析目标库展示名）</param>
    /// <param name="quartzTaskService">Quartz 任务服务</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktTableArchiveService(
        ITaktCompanyRepository<TaktTableArchive> tableArchiveRepository,
        ITaktCompanyRepository<TaktArchiveLog> archiveLogRepository,
        ITaktTableArchiveProvider tableArchiveProvider,
        ITaktDatabaseSchemaProvider schemaProvider,
        ITaktQuartzTaskService quartzTaskService,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _tableArchiveRepository = tableArchiveRepository;
        _archiveLogRepository = archiveLogRepository;
        _tableArchiveProvider = tableArchiveProvider;
        _schemaProvider = schemaProvider ?? throw new ArgumentNullException(nameof(schemaProvider));
        _quartzTaskService = quartzTaskService;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取数据表归档列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktTableArchiveDto>> GetTableArchiveListAsync(TaktTableArchiveQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _tableArchiveRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktTableArchiveDto>.Create(
            data.Adapt<List<TaktTableArchiveDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取数据表归档
    /// </summary>
    /// <param name="id">数据表归档ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktTableArchiveDto?> GetTableArchiveByIdAsync(long id)
    {
        var entity = await _tableArchiveRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktTableArchiveDto>();
    }

    /// <summary>
    /// 获取数据表归档选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetTableArchiveOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _tableArchiveRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ArchiveStatus == 1,
            x => x.TargetDatabaseName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = string.IsNullOrWhiteSpace(e.ArchiveName) ? e.TableName : e.ArchiveName,
        }).ToList();
    }

    /// <summary>
    /// 创建数据表归档
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTableArchiveDto> CreateTableArchiveAsync(TaktTableArchiveCreateDto dto)
    {
        EnsureThreeLayerContext();
        var entity = dto.Adapt<TaktTableArchive>();
        entity.TargetTenantCode = CurrentTenantCode;
        entity.TargetDatabaseName = await ResolveTargetDatabaseDisplayNameAsync(CurrentTenantCode).ConfigureAwait(false);
        ApplyDerivedArchiveFields(entity);
        var isUnique_ix_table_archive_table_unique = await _uniqueValidator.IsUniqueAsync(
            _tableArchiveRepository,
            x => x.TableName == entity.TableName);
        if (!isUnique_ix_table_archive_table_unique)
        {
            throw new TaktBusinessException("数据表归档的TableName已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _tableArchiveRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _tableArchiveRepository.CreateAsync(entity);
        return await GetTableArchiveByIdAsync(entity.Id) ?? entity.Adapt<TaktTableArchiveDto>();
    }

    /// <summary>
    /// 更新数据表归档
    /// </summary>
    /// <param name="id">数据表归档ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTableArchiveDto> UpdateTableArchiveAsync(long id, TaktTableArchiveUpdateDto dto)
    {
        EnsureThreeLayerContext();
        var entity = await _tableArchiveRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("数据表归档不存在");
        }
        dto.Adapt(entity);
        entity.TargetTenantCode = CurrentTenantCode;
        entity.TargetDatabaseName = await ResolveTargetDatabaseDisplayNameAsync(CurrentTenantCode).ConfigureAwait(false);
        ApplyDerivedArchiveFields(entity);
        var isUnique_ix_table_archive_table_unique = await _uniqueValidator.IsUniqueAsync(
            _tableArchiveRepository,
            x => x.TableName == entity.TableName,
            id);
        if (!isUnique_ix_table_archive_table_unique)
        {
            throw new TaktBusinessException("数据表归档的TableName已存在");
        }
        await _tableArchiveRepository.UpdateAsync(entity);
        return await GetTableArchiveByIdAsync(id) ?? throw new TaktBusinessException("数据表归档不存在");
    }

    /// <summary>
    /// 删除数据表归档
    /// </summary>
    /// <param name="id">数据表归档ID</param>
    /// <returns>任务</returns>
    public async Task DeleteTableArchiveByIdAsync(long id)
    {
        var deleted = await _tableArchiveRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("数据表归档不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除数据表归档
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteTableArchiveBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteTableArchiveByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新数据表归档状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTableArchiveDto> UpdateTableArchiveStatusAsync(TaktTableArchiveStatusDto dto)
    {
        var entity = await _tableArchiveRepository.GetByIdAsync(dto.TableArchiveId);
        if (entity == null)
        {
            throw new TaktBusinessException("数据表归档不存在");
        }
        entity.ArchiveStatus = dto.ArchiveStatus;
        await _tableArchiveRepository.UpdateAsync(entity);
        return await GetTableArchiveByIdAsync(dto.TableArchiveId) ?? throw new TaktBusinessException("数据表归档不存在");
    }

    /// <summary>
    /// 更新数据表归档排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTableArchiveDto> UpdateTableArchiveSortAsync(TaktTableArchiveSortDto dto)
    {
        var entity = await _tableArchiveRepository.GetByIdAsync(dto.TableArchiveId);
        if (entity == null)
        {
            throw new TaktBusinessException("数据表归档不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _tableArchiveRepository.UpdateAsync(entity);
        return await GetTableArchiveByIdAsync(dto.TableArchiveId) ?? throw new TaktBusinessException("数据表归档不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetTableArchiveTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktTableArchiveTemplateDto>(
            sheetName ?? "数据表归档导入模板",
            fileName ?? "数据表归档导入模板.xlsx");
    }

    /// <summary>
    /// 导入数据表归档
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportTableArchiveAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktTableArchiveImportDto>(fileStream, sheetName ?? "数据表归档导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _tableArchiveRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktTableArchive>();
                ApplyDerivedArchiveFields(entity);
                var importKey = $"{entity.TableName}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（TableName）");
                }
                var isUnique_ix_table_archive_table_unique = await _uniqueValidator.IsUniqueAsync(
                    _tableArchiveRepository,
                    x => x.TableName == entity.TableName);
                if (!isUnique_ix_table_archive_table_unique)
                {
                    throw new TaktBusinessException("数据表归档的TableName已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _tableArchiveRepository.CreateAsync(entity);
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
    /// 导出数据表归档
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportTableArchiveAsync(TaktTableArchiveQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktTableArchiveQueryDto());
        var list = await _tableArchiveRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktTableArchiveExportDto>(),
                sheetName ?? "数据表归档数据",
                fileName ?? "数据表归档导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktTableArchiveExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "数据表归档数据",
            fileName ?? "数据表归档导出.xlsx");
    }

    /// <summary>
    /// 预览按年归档行数
    /// </summary>
    /// <param name="dto">归档请求</param>
    /// <returns>预览结果</returns>
    public async Task<TaktTableArchivePreviewResultDto> PreviewTableArchiveAsync(TaktTableArchiveExecuteDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        ValidateArchiveExecuteRequest(dto);
        EnsureThreeLayerContext();
        var result = new TaktTableArchivePreviewResultDto();
        foreach (var policyId in ParseDistinctPolicyIds(dto.PolicyIds))
        {
            var policy = await GetEnabledArchivePolicyAsync(policyId);
            ValidateArchiveYear(policy, dto.ArchiveYear);
            var options = ToArchiveOptions(policy, dto.ArchiveYear);
            var preview = await _tableArchiveProvider.PreviewAsync(options);
            result.Items.Add(new TaktTableArchivePreviewItemDto
            {
                PolicyId = policy.Id,
                ArchiveName = policy.ArchiveName,
                TableName = preview.TableName,
                ArchiveTableName = preview.ArchiveTableName,
                ArchiveYear = preview.ArchiveYear,
                SourceRowCount = preview.SourceRowCount
            });
            result.TotalRowCount = checked(result.TotalRowCount + preview.SourceRowCount);
        }
        return result;
    }

    /// <summary>
    /// 执行按年归档并写入审计日志
    /// </summary>
    /// <param name="dto">归档请求</param>
    /// <returns>执行结果</returns>
    public async Task<TaktTableArchiveExecuteResultDto> ExecuteTableArchiveAsync(TaktTableArchiveExecuteDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        ValidateArchiveExecuteRequest(dto);
        EnsureThreeLayerContext();
        var result = new TaktTableArchiveExecuteResultDto();
        foreach (var policyId in ParseDistinctPolicyIds(dto.PolicyIds))
        {
            var policy = await GetEnabledArchivePolicyAsync(policyId);
            var startedAt = DateTime.Now;
            var item = new TaktTableArchiveExecuteItemDto
            {
                PolicyId = policy.Id,
                TableName = policy.TableName,
                ArchiveTableName = TaktTableArchiveKeyKindHelper.BuildArchiveTableNameForYear(
                    policy.TableName,
                    policy.ArchiveKeyKind,
                    dto.ArchiveYear),
                ArchiveYear = dto.ArchiveYear
            };
            try
            {
                ValidateArchiveYear(policy, dto.ArchiveYear);
                var options = ToArchiveOptions(policy, dto.ArchiveYear);
                var archiveResult = await _tableArchiveProvider.ArchiveAsync(options);
                item.TableName = archiveResult.TableName;
                item.ArchiveTableName = archiveResult.ArchiveTableName;
                item.SourceRowCount = archiveResult.SourceRowCount;
                item.ArchivedRowCount = archiveResult.ArchivedRowCount;
                item.DeletedRowCount = archiveResult.DeletedRowCount;
                item.Success = true;
                await WriteArchiveLogAsync(policy, archiveResult, startedAt, DateTime.Now, 1, null);
            }
            catch (Exception ex)
            {
                item.Success = false;
                item.ErrorMessage = ex.Message;
                await WriteArchiveLogAsync(
                    policy,
                    dto.ArchiveYear,
                    item.ArchiveTableName,
                    item.SourceRowCount,
                    item.ArchivedRowCount,
                    item.DeletedRowCount,
                    startedAt,
                    DateTime.Now,
                    2,
                    ex.Message);
            }
            result.Items.Add(item);
        }
        return result;
    }

    /// <summary>
    /// 预建年分表
    /// </summary>
    /// <param name="dto">建表请求</param>
    /// <returns>建表结果</returns>
    public async Task<TaktTableEnsureYearTablesResultDto> EnsureYearTablesAsync(TaktTableEnsureYearTablesDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        if (string.IsNullOrWhiteSpace(dto.PolicyId))
        {
            throw new TaktBusinessException("策略 ID 不能为空");
        }
        if (!long.TryParse(dto.PolicyId.Trim(), out var policyId))
        {
            throw new TaktBusinessException($"非法策略 ID: {dto.PolicyId}");
        }
        if (dto.Years == null || dto.Years.Count == 0)
        {
            throw new TaktBusinessException("请至少选择一个年份");
        }
        EnsureThreeLayerContext();
        var policy = await GetEnabledArchivePolicyAsync(policyId);
        var options = ToArchiveOptions(policy, dto.Years[0]);
        var yearTables = await _tableArchiveProvider.EnsureYearTablesAsync(options, dto.Years);
        return new TaktTableEnsureYearTablesResultDto
        {
            PolicyId = policy.Id,
            TableName = policy.TableName,
            YearTableNames = yearTables.ToList()
        };
    }

    /// <summary>
    /// 立即归档：创建一次性 Quartz 任务（尽快触发）
    /// </summary>
    /// <param name="dto">归档请求</param>
    /// <returns>调度结果</returns>
    public async Task<TaktTableArchiveScheduleResultDto> RunTableArchiveNowAsync(TaktTableArchiveScheduleDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        dto.ScheduledAt = DateTime.Now;
        return await CreateArchiveQuartzTaskAsync(dto, ExecuteModeImmediate);
    }

    /// <summary>
    /// 后台归档：创建一次性 Quartz 任务（按 ScheduledAt 触发）
    /// </summary>
    /// <param name="dto">归档请求</param>
    /// <returns>调度结果</returns>
    public async Task<TaktTableArchiveScheduleResultDto> ScheduleTableArchiveAsync(TaktTableArchiveScheduleDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        if (!dto.ScheduledAt.HasValue)
        {
            throw new TaktBusinessException("计划执行时间不能为空");
        }
        if (dto.ScheduledAt.Value <= DateTime.Now)
        {
            throw new TaktBusinessException("计划执行时间须晚于当前时间");
        }
        return await CreateArchiveQuartzTaskAsync(dto, ExecuteModeBackground);
    }

    /// <summary>
    /// 校验并创建归档用一次性 Quartz 任务
    /// </summary>
    /// <param name="dto">调度请求</param>
    /// <param name="executeMode">1=立即 2=后台</param>
    /// <returns>调度结果</returns>
    private async Task<TaktTableArchiveScheduleResultDto> CreateArchiveQuartzTaskAsync(
        TaktTableArchiveScheduleDto dto,
        int executeMode)
    {
        var executeDto = new TaktTableArchiveExecuteDto
        {
            PolicyIds = dto.PolicyIds ?? new List<string>(),
            ArchiveYear = dto.ArchiveYear,
        };
        ValidateArchiveExecuteRequest(executeDto);
        EnsureThreeLayerContext();
        var policyIds = ParseDistinctPolicyIds(executeDto.PolicyIds);
        foreach (var policyId in policyIds)
        {
            var policy = await GetEnabledArchivePolicyAsync(policyId);
            ValidateArchiveYear(policy, executeDto.ArchiveYear);
        }
        var scheduledAt = dto.ScheduledAt ?? DateTime.Now;
        var idList = policyIds.Select(id => id.ToString()).ToList();
        var suffix = $"{DateTime.Now:yyyyMMddHHmmss}{Guid.NewGuid():N}"[..16];
        var executeParams = System.Text.Json.JsonSerializer.Serialize(new
        {
            policyIds = idList,
            archiveYear = executeDto.ArchiveYear,
        });
        var quartzDto = new TaktQuartzTaskCreateDto
        {
            TenantCode = CurrentTenantCode,
            CompanyCode = CurrentCompanyCode,
            TaskCode = $"QT_TBL_ARC_{suffix}",
            TaskName = $"数据表归档 {executeDto.ArchiveYear}",
            JobName = $"table_archive_{suffix}",
            JobGroup = QuartzJobGroup,
            TaskType = QuartzTaskTypeAssembly,
            AssemblyName = QuartzAssemblyName,
            ClassName = QuartzHandlerClassName,
            TriggerType = 0,
            IntervalSeconds = 0,
            CronExpression = string.Empty,
            FirstRunAt = scheduledAt,
            ExecuteParams = executeParams,
            Concurrent = 0,
            MisfirePolicy = 0,
            TaskStatus = 0,
            TaskDescription = $"按年归档 year={executeDto.ArchiveYear} policies={string.Join(',', idList)}",
        };
        var quartzTask = await _quartzTaskService.CreateQuartzTaskAsync(quartzDto);
        return new TaktTableArchiveScheduleResultDto
        {
            QuartzTaskId = quartzTask.QuartzTaskId,
            TaskCode = quartzTask.TaskCode,
            ExecuteMode = executeMode,
            ScheduledAt = scheduledAt,
            ArchiveYear = executeDto.ArchiveYear,
            PolicyIds = idList,
        };
    }

    // ========================================
    // 数据归档编排
    // ========================================

    /// <summary>
    /// 校验归档执行请求
    /// </summary>
    /// <param name="dto">归档请求</param>
    private static void ValidateArchiveExecuteRequest(TaktTableArchiveExecuteDto dto)
    {
        if (dto.PolicyIds == null || dto.PolicyIds.Count == 0)
        {
            throw new TaktBusinessException("请至少选择一条已启用策略");
        }
        if (dto.ArchiveYear < 1970 || dto.ArchiveYear > 2100)
        {
            throw new TaktBusinessException("归档年份无效");
        }
    }

    /// <summary>
    /// 解析并去重策略 ID
    /// </summary>
    /// <param name="policyIds">策略 ID 字符串列表</param>
    /// <returns>long 型策略 ID 列表</returns>
    private static List<long> ParseDistinctPolicyIds(IEnumerable<string> policyIds)
    {
        var result = new List<long>();
        var seen = new HashSet<long>();
        foreach (var policyIdText in policyIds)
        {
            if (string.IsNullOrWhiteSpace(policyIdText))
            {
                throw new TaktBusinessException("策略 ID 不能为空");
            }
            if (!long.TryParse(policyIdText.Trim(), out var policyId))
            {
                throw new TaktBusinessException($"非法策略 ID: {policyIdText}");
            }
            if (seen.Add(policyId))
            {
                result.Add(policyId);
            }
        }
        return result;
    }

    /// <summary>
    /// 加载已启用且租户/公司匹配的策略
    /// </summary>
    /// <param name="policyId">策略主键</param>
    /// <returns>策略实体</returns>
    private async Task<TaktTableArchive> GetEnabledArchivePolicyAsync(long policyId)
    {
        var entity = await _tableArchiveRepository.GetByIdAsync(policyId);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("数据表归档不存在");
        }
        if (entity.ArchiveStatus != 1)
        {
            throw new TaktBusinessException($"策略未启用: {entity.ArchiveName}");
        }
        return entity;
    }

    /// <summary>
    /// 校验归档年份：须 ≤ currentYear-1（热库保留年数固定为 1）
    /// </summary>
    /// <param name="policy">数据表归档配置</param>
    /// <param name="archiveYear">归档年份</param>
    private static void ValidateArchiveYear(TaktTableArchive policy, int archiveYear)
    {
        ArgumentNullException.ThrowIfNull(policy);
        var currentYear = DateTime.Now.Year;
        if (archiveYear > currentYear)
        {
            throw new TaktBusinessException($"归档年份 {archiveYear} 不能大于当前年 {currentYear}");
        }
        // 热库保留年数固定为 1：仅允许归档 currentYear-1 及更早
        var cutoffYear = currentYear - 1;
        if (archiveYear > cutoffYear)
        {
            throw new TaktBusinessException(
                $"归档年份 {archiveYear} 过新，须 <= {cutoffYear}（当前年 {currentYear} - 热库保留 1 年）");
        }
    }

    /// <summary>
    /// 按当前租户编码解析业务库展示名（与 DatabaseInfos DisplayName 一致）
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>数据库展示名</returns>
    private async Task<string> ResolveTargetDatabaseDisplayNameAsync(string tenantCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        var code = tenantCode.Trim();
        var databases = await _schemaProvider.GetDatabasesAsync().ConfigureAwait(false);
        var displayName = databases
            .FirstOrDefault(x => string.Equals(x.TenantCode, code, StringComparison.OrdinalIgnoreCase))
            ?.DisplayName?
            .Trim();
        if (string.IsNullOrWhiteSpace(displayName))
        {
            throw new TaktBusinessException("目标数据库展示名不能为空");
        }
        return displayName;
    }

    /// <summary>
    /// 规范化派生字段：热库年数、归档键类型、归档名称（物理表名_键类型码）
    /// </summary>
    /// <param name="entity">归档配置实体</param>
    private static void ApplyDerivedArchiveFields(TaktTableArchive entity)
    {
        entity.RetainHotYears = 1;
        if (entity.ArchiveKeyKind is < 1 or > 3)
        {
            throw new TaktBusinessException("归档键类型须为 1/2/3");
        }
        entity.TableName = entity.TableName?.Trim().ToLowerInvariant() ?? string.Empty;
        entity.ArchiveName = BuildArchiveDisplayName(entity.TableName, entity.ArchiveKeyKind);
    }

    /// <summary>
    /// 生成归档显示名：{物理表名}_{yyyyMMddHHmmss|yyyyMM|yyyy}
    /// </summary>
    /// <param name="tableName">物理表名</param>
    /// <param name="archiveKeyKind">归档键类型（字典 sys_archive_key_kind）</param>
    /// <returns>归档名称</returns>
    private static string BuildArchiveDisplayName(string tableName, int archiveKeyKind)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tableName);
        string kindCode;
        try
        {
            kindCode = TaktTableArchiveKeyKindHelper.ToFormatCode(archiveKeyKind);
        }
        catch (ArgumentOutOfRangeException)
        {
            throw new TaktBusinessException("归档键类型须为字典 sys_archive_key_kind 的 1/2/3");
        }
        var name = $"{tableName.Trim().ToLowerInvariant()}_{kindCode}";
        return name.Length <= 200 ? name : name[..200];
    }

    /// <summary>
    /// 映射归档提供者选项
    /// </summary>
    /// <param name="policy">数据表归档配置</param>
    /// <param name="archiveYear">归档年份</param>
    /// <returns>归档选项</returns>
    private TaktTableArchiveOptions ToArchiveOptions(TaktTableArchive policy, int archiveYear) =>
        new()
        {
            TargetTenantCode = policy.TargetTenantCode,
            TargetDatabaseName = policy.TargetDatabaseName,
            TableName = policy.TableName,
            ArchiveKeyColumn = policy.ArchiveKeyColumn,
            ArchiveKeyKind = policy.ArchiveKeyKind,
            ArchiveYear = archiveYear,
            CompanyCode = CurrentCompanyCode
        };

    /// <summary>
    /// 写入归档执行审计日志
    /// </summary>
    /// <param name="policy">数据表归档配置</param>
    /// <param name="archiveResult">归档结果</param>
    /// <param name="startedAt">开始时间</param>
    /// <param name="finishedAt">结束时间</param>
    /// <param name="runStatus">运行状态（1=成功 2=失败）</param>
    /// <param name="errorMessage">错误信息</param>
    private async Task WriteArchiveLogAsync(
        TaktTableArchive policy,
        TaktTableArchiveResult archiveResult,
        DateTime startedAt,
        DateTime finishedAt,
        int runStatus,
        string? errorMessage)
    {
        await WriteArchiveLogAsync(
            policy,
            archiveResult.ArchiveYear,
            archiveResult.ArchiveTableName,
            archiveResult.SourceRowCount,
            archiveResult.ArchivedRowCount,
            archiveResult.DeletedRowCount,
            startedAt,
            finishedAt,
            runStatus,
            errorMessage);
    }

    /// <summary>
    /// 写入归档执行审计日志
    /// </summary>
    /// <param name="policy">数据表归档配置</param>
    /// <param name="archiveYear">归档年份</param>
    /// <param name="archiveTableName">归档表名</param>
    /// <param name="sourceRowCount">源匹配行数</param>
    /// <param name="archivedRowCount">归档行数</param>
    /// <param name="deletedRowCount">删除行数</param>
    /// <param name="startedAt">开始时间</param>
    /// <param name="finishedAt">结束时间</param>
    /// <param name="runStatus">运行状态（1=成功 2=失败）</param>
    /// <param name="errorMessage">错误信息</param>
    private async Task WriteArchiveLogAsync(
        TaktTableArchive policy,
        int archiveYear,
        string archiveTableName,
        int sourceRowCount,
        int archivedRowCount,
        int deletedRowCount,
        DateTime startedAt,
        DateTime finishedAt,
        int runStatus,
        string? errorMessage)
    {
        var log = new TaktArchiveLog
        {
            ArchiveKind = "table.year",
            SourceId = policy.Id.ToString(),
            SourceName = policy.TableName,
            TargetName = archiveTableName,
            ArchiveYear = archiveYear,
            SourceCount = sourceRowCount,
            ArchivedCount = archivedRowCount,
            DeletedCount = deletedRowCount,
            RunStatus = runStatus,
            ErrorMessage = TruncateArchiveError(errorMessage),
            StartedAt = startedAt,
            FinishedAt = finishedAt
        };
        await _archiveLogRepository.CreateAsync(log);
    }

    /// <summary>
    /// 截断归档错误信息
    /// </summary>
    /// <param name="message">原始错误</param>
    /// <returns>最长 2000 字符的错误信息</returns>
    private static string? TruncateArchiveError(string? message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return null;
        }
        var trimmed = message.Trim();
        return trimmed.Length <= 2000 ? trimmed : trimmed[..2000];
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建数据归档查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktTableArchive, bool>> QueryExpression(TaktTableArchiveQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktTableArchive>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.TargetTenantCode != null && x.TargetTenantCode.Contains(keywords))
                || (x.TargetDatabaseName != null && x.TargetDatabaseName.Contains(keywords))
                || (x.TableName != null && x.TableName.Contains(keywords))
                || (x.ArchiveKeyColumn != null && x.ArchiveKeyColumn.Contains(keywords))
                || SqlFunc.ToString(x.ArchiveKeyKind).Contains(keywords)
                || SqlFunc.ToString(x.RetainHotYears).Contains(keywords)
                || (x.ArchiveName != null && x.ArchiveName.Contains(keywords))
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || SqlFunc.ToString(x.ArchiveStatus).Contains(keywords)
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.TargetTenantCode))
        {
            exp = exp.And(x => x.TargetTenantCode != null && x.TargetTenantCode.Contains(queryDto.TargetTenantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.TargetDatabaseName))
        {
            exp = exp.And(x => x.TargetDatabaseName != null && x.TargetDatabaseName.Contains(queryDto.TargetDatabaseName));
        }

        if (!string.IsNullOrEmpty(queryDto?.TableName))
        {
            exp = exp.And(x => x.TableName != null && x.TableName.Contains(queryDto.TableName));
        }

        if (!string.IsNullOrEmpty(queryDto?.ArchiveKeyColumn))
        {
            exp = exp.And(x => x.ArchiveKeyColumn != null && x.ArchiveKeyColumn.Contains(queryDto.ArchiveKeyColumn));
        }

        if (queryDto?.ArchiveKeyKind.HasValue == true)
        {
            exp = exp.And(x => x.ArchiveKeyKind == queryDto.ArchiveKeyKind);
        }

        if (queryDto?.RetainHotYears.HasValue == true)
        {
            exp = exp.And(x => x.RetainHotYears == queryDto.RetainHotYears);
        }

        if (!string.IsNullOrEmpty(queryDto?.ArchiveName))
        {
            exp = exp.And(x => x.ArchiveName != null && x.ArchiveName.Contains(queryDto.ArchiveName));
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (queryDto?.ArchiveStatus.HasValue == true)
        {
            exp = exp.And(x => x.ArchiveStatus == queryDto.ArchiveStatus);
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
        if (!string.IsNullOrWhiteSpace(queryDto?.PlantCode))
        {
            var plantCode = queryDto.PlantCode;
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(plantCode));
        }


        return exp.ToExpression();
    }
}
