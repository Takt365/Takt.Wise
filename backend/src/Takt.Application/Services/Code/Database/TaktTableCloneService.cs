// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Code.Database
// 文件名称：TaktTableCloneService.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：跨租户整表数据克隆应用服务（备份预览 + 执行克隆；成功后写入 TaktTableClone 实体）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Code.Database;
using Takt.Domain.Entities.Code.Database;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Models.Code;

namespace Takt.Application.Services.Code.Database;

/// <summary>
/// 跨租户整表数据克隆应用服务
/// </summary>
public class TaktTableCloneService : TaktServiceBase, ITaktTableCloneService
{
    private readonly ITaktTableCloneProvider _cloneProvider;
    private readonly ITaktCompanyRepository<TaktTableClone> _tableCloneRepository;
    private readonly IValidator<TaktTableCloneDto> _validator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="cloneProvider">数据表克隆提供者</param>
    /// <param name="tableCloneRepository">整表克隆记录仓储</param>
    /// <param name="validator">克隆请求验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktTableCloneService(
        ITaktTableCloneProvider cloneProvider,
        ITaktCompanyRepository<TaktTableClone> tableCloneRepository,
        IValidator<TaktTableCloneDto> validator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _cloneProvider = cloneProvider ?? throw new ArgumentNullException(nameof(cloneProvider));
        _tableCloneRepository = tableCloneRepository ?? throw new ArgumentNullException(nameof(tableCloneRepository));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    /// <summary>
    /// 获取跨租户整表克隆备份预览（备份窗口）
    /// </summary>
    /// <param name="dto">克隆请求（源/目标租户、数据库、表清单）</param>
    /// <returns>各目标表备份与清空预览</returns>
    public async Task<TaktTableClonePreviewDto> GetTableClonePreviewAsync(TaktTableCloneDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        await ValidateRequestAsync(dto).ConfigureAwait(false);
        var preview = new TaktTableClonePreviewDto
        {
            SummaryMessage = $"共 {dto.Tables.Count} 张目标表将在克隆前先全量备份，再 TRUNCATE 清空全部数据。",
            ConfirmHint = "我已阅读备份窗口提示，确认目标表将先全量备份再 TRUNCATE 清空（执行克隆时须勾选 ConfirmTargetBackupAndClear）"
        };
        foreach (var table in dto.Tables)
        {
            var options = BuildOptions(dto, table);
            try
            {
                var itemPreview = await _cloneProvider.GetTargetBackupPreviewAsync(options).ConfigureAwait(false);
                preview.Targets.Add(MapTargetPreviewItem(itemPreview));
            }
            catch (InvalidOperationException ex)
            {
                throw new TaktBusinessException($"表 {table.TargetTableName} 备份预览失败：{ex.Message}");
            }
            catch (ArgumentException ex)
            {
                throw new TaktBusinessException($"表 {table.TargetTableName} 备份预览失败：{ex.Message}");
            }
        }
        return preview;
    }

    /// <summary>
    /// 将源表数据克隆到目标表（跨租户；一次 1~ITaktTableCloneService.MaxTableCountPerRequest 张表；须先确认备份窗口）
    /// </summary>
    /// <param name="dto">克隆请求（源/目标租户、数据库、表清单）</param>
    /// <returns>批量克隆结果</returns>
    public async Task<TaktTableCloneResultDto> CloneTableAsync(TaktTableCloneDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        await ValidateRequestAsync(dto).ConfigureAwait(false);
        if (!dto.ConfirmTargetBackupAndClear)
        {
            throw new TaktBusinessException(
                "请先在备份窗口预览目标表数据，确认后将 ConfirmTargetBackupAndClear 设为 true 再执行克隆");
        }
        var batchResult = new TaktTableCloneResultDto();
        foreach (var table in dto.Tables)
        {
            var options = BuildOptions(dto, table);
            try
            {
                var result = await _cloneProvider.CloneTableAsync(options).ConfigureAwait(false);
                await RecordTableCloneAsync(options, result).ConfigureAwait(false);
                batchResult.Tables.Add(MapTableResult(table.SourceTableName.Trim(), table.TargetTableName.Trim(), result));
            }
            catch (InvalidOperationException ex)
            {
                await RecordTableCloneAsync(options, null).ConfigureAwait(false);
                throw new TaktBusinessException($"表 {table.SourceTableName} 克隆失败：{ex.Message}");
            }
            catch (ArgumentException ex)
            {
                await RecordTableCloneAsync(options, null).ConfigureAwait(false);
                throw new TaktBusinessException($"表 {table.SourceTableName} 克隆失败：{ex.Message}");
            }
        }
        batchResult.TableCount = batchResult.Tables.Count;
        batchResult.TotalSourceRowCount = batchResult.Tables.Sum(x => x.SourceRowCount);
        batchResult.TotalClonedRowCount = batchResult.Tables.Sum(x => x.ClonedRowCount);
        return batchResult;
    }

    /// <summary>
    /// 持久化单表克隆记录
    /// </summary>
    /// <param name="options">克隆选项</param>
    /// <param name="result">克隆结果；失败时为 null</param>
    /// <returns>任务</returns>
    private async Task RecordTableCloneAsync(TaktTableCloneOptions options, TaktTableCloneResult? result)
    {
        ArgumentNullException.ThrowIfNull(options);
        var entity = new TaktTableClone
        {
            SourceTenantCode = options.SourceTenantCode,
            SourceDatabaseName = options.SourceDatabaseName,
            SourceTableName = options.SourceTableName,
            TargetTenantCode = options.TargetTenantCode,
            TargetDatabaseName = options.TargetDatabaseName,
            TargetTableName = options.TargetTableName,
            TargetBackupDatabaseName = options.TargetDatabaseName,
            BackupTableName = result?.BackupTableName,
            BackedUpRowCount = result?.BackedUpRowCount ?? 0,
            ClearedRowCount = result?.ClearedRowCount ?? 0,
            SourceRowCount = result?.SourceRowCount ?? 0,
            ClonedRowCount = result?.ClonedRowCount ?? 0
        };
        await _tableCloneRepository.CreateAsync(entity).ConfigureAwait(false);
    }

    /// <summary>
    /// 校验克隆请求
    /// </summary>
    /// <param name="dto">克隆请求</param>
    /// <returns>任务</returns>
    private async Task ValidateRequestAsync(TaktTableCloneDto dto)
    {
        var validation = await _validator.ValidateAsync(dto).ConfigureAwait(false);
        if (!validation.IsValid)
        {
            throw new TaktBusinessException(validation.Errors[0].ErrorMessage);
        }
        if (dto.Tables.Count > ITaktTableCloneService.MaxTableCountPerRequest)
        {
            throw new TaktBusinessException($"一次最多克隆 {ITaktTableCloneService.MaxTableCountPerRequest} 张表");
        }
    }

    /// <summary>
    /// 构建单表克隆选项
    /// </summary>
    /// <param name="dto">批量克隆请求</param>
    /// <param name="table">单表映射</param>
    /// <returns>克隆选项</returns>
    private static TaktTableCloneOptions BuildOptions(TaktTableCloneDto dto, TaktTableCloneItemDto table)
    {
        return new TaktTableCloneOptions
        {
            SourceTenantCode = dto.SourceTenantCode.Trim(),
            SourceDatabaseName = dto.SourceDatabaseName.Trim(),
            SourceTableName = table.SourceTableName.Trim(),
            TargetTenantCode = dto.TargetTenantCode.Trim(),
            TargetDatabaseName = dto.TargetDatabaseName.Trim(),
            TargetTableName = table.TargetTableName.Trim(),
            PreserveIdentityValues = dto.PreserveIdentityValues
        };
    }

    /// <summary>
    /// 映射目标表备份预览项
    /// </summary>
    /// <param name="preview">提供者预览结果</param>
    /// <returns>预览项 DTO</returns>
    private static TaktTableCloneTargetPreviewItemDto MapTargetPreviewItem(TaktCloneTargetBackupPreview preview)
    {
        return new TaktTableCloneTargetPreviewItemDto
        {
            TargetTableName = preview.TargetTableName,
            TargetRowCount = preview.TargetRowCount,
            PlannedBackupTableName = preview.PlannedBackupTableName,
            BackupDescription = preview.BackupDescription,
            ClearDescription = preview.ClearDescription,
            WarningMessage = preview.WarningMessage
        };
    }

    /// <summary>
    /// 映射单表克隆结果为 DTO
    /// </summary>
    /// <param name="sourceTableName">源表名</param>
    /// <param name="targetTableName">目标表名</param>
    /// <param name="result">提供者结果</param>
    /// <returns>单表结果 DTO</returns>
    private static TaktTableCloneTableResultDto MapTableResult(
        string sourceTableName,
        string targetTableName,
        TaktTableCloneResult result)
    {
        return new TaktTableCloneTableResultDto
        {
            SourceTableName = sourceTableName,
            TargetTableName = targetTableName,
            BackupTableName = result.BackupTableName,
            BackedUpRowCount = result.BackedUpRowCount,
            ClearedRowCount = result.ClearedRowCount,
            BackupSummaryMessage = result.BackupSummaryMessage,
            SourceRowCount = result.SourceRowCount,
            ClonedRowCount = result.ClonedRowCount,
            CommonColumnCount = result.CommonColumnCount,
            CommonColumns = result.CommonColumns.ToList(),
            SkippedSourceColumns = result.SkippedSourceColumns.ToList(),
            SkippedTargetColumns = result.SkippedTargetColumns.ToList()
        };
    }
}
