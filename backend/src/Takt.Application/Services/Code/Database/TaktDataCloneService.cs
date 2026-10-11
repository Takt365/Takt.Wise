// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Code.Database
// 文件名称：TaktDataCloneService.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：公司级数据克隆应用服务（备份预览 + 执行克隆；成功后写入 TaktDataClone 实体）
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
/// 公司级数据克隆应用服务
/// </summary>
public class TaktDataCloneService : TaktServiceBase, ITaktDataCloneService
{
    private readonly ITaktDataCloneProvider _cloneProvider;
    private readonly ITaktCompanyRepository<TaktDataClone> _dataCloneRepository;
    private readonly IValidator<TaktDataCloneDto> _validator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="cloneProvider">公司级数据克隆提供者</param>
    /// <param name="dataCloneRepository">公司级数据克隆记录仓储</param>
    /// <param name="validator">克隆请求验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktDataCloneService(
        ITaktDataCloneProvider cloneProvider,
        ITaktCompanyRepository<TaktDataClone> dataCloneRepository,
        IValidator<TaktDataCloneDto> validator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _cloneProvider = cloneProvider ?? throw new ArgumentNullException(nameof(cloneProvider));
        _dataCloneRepository = dataCloneRepository ?? throw new ArgumentNullException(nameof(dataCloneRepository));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    /// <summary>
    /// 获取公司级数据克隆备份预览（备份窗口）
    /// </summary>
    /// <param name="dto">克隆请求</param>
    /// <returns>目标公司备份与清空预览</returns>
    public async Task<TaktDataClonePreviewDto> GetDataClonePreviewAsync(TaktDataCloneDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        await ValidateRequestAsync(dto).ConfigureAwait(false);
        var options = BuildOptions(dto);
        try
        {
            var preview = await _cloneProvider.GetTargetBackupPreviewAsync(options).ConfigureAwait(false);
            return MapPreview(preview);
        }
        catch (InvalidOperationException ex)
        {
            throw new TaktBusinessException(ex.Message);
        }
        catch (ArgumentException ex)
        {
            throw new TaktBusinessException(ex.Message);
        }
    }

    /// <summary>
    /// 按公司范围克隆数据（一次仅一个源公司、一张源表 → 一个目标公司、一张目标表；须先确认备份窗口）
    /// </summary>
    /// <param name="dto">克隆请求</param>
    /// <returns>克隆结果</returns>
    public async Task<TaktDataCloneResultDto> CloneDataAsync(TaktDataCloneDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        await ValidateRequestAsync(dto).ConfigureAwait(false);
        if (!dto.ConfirmTargetBackupAndClear)
        {
            throw new TaktBusinessException(
                "请先在备份窗口预览目标公司数据，确认后将 ConfirmTargetBackupAndClear 设为 true 再执行克隆");
        }
        var options = BuildOptions(dto);
        try
        {
            var result = await _cloneProvider.CloneDataAsync(options).ConfigureAwait(false);
            await RecordDataCloneAsync(options, result).ConfigureAwait(false);
            return MapResult(result);
        }
        catch (InvalidOperationException ex)
        {
            await RecordDataCloneAsync(options, null).ConfigureAwait(false);
            throw new TaktBusinessException(ex.Message);
        }
        catch (ArgumentException ex)
        {
            await RecordDataCloneAsync(options, null).ConfigureAwait(false);
            throw new TaktBusinessException(ex.Message);
        }
    }

    /// <summary>
    /// 持久化克隆记录
    /// </summary>
    /// <param name="options">克隆选项</param>
    /// <param name="result">克隆结果；失败时为 null</param>
    /// <returns>任务</returns>
    private async Task RecordDataCloneAsync(TaktDataCloneOptions options, TaktDataCloneResult? result)
    {
        ArgumentNullException.ThrowIfNull(options);
        var entity = new TaktDataClone
        {
            SourceTenantCode = options.SourceTenantCode,
            SourceDatabaseName = options.SourceDatabaseName,
            SourceTableName = options.SourceTableName,
            SourceCompanyCode = options.SourceCompanyCode,
            TargetTenantCode = options.TargetTenantCode,
            TargetDatabaseName = options.TargetDatabaseName,
            TargetTableName = options.TargetTableName,
            TargetCompanyCode = options.TargetCompanyCode,
            TargetBackupDatabaseName = options.TargetDatabaseName,
            BackupTableName = result?.BackupTableName,
            BackedUpRowCount = result?.BackedUpRowCount ?? 0,
            ClearedRowCount = result?.ClearedRowCount ?? 0,
            SourceRowCount = result?.SourceRowCount ?? 0,
            ClonedRowCount = result?.ClonedRowCount ?? 0
        };
        await _dataCloneRepository.CreateAsync(entity).ConfigureAwait(false);
    }

    /// <summary>
    /// 校验克隆请求
    /// </summary>
    /// <param name="dto">克隆请求</param>
    /// <returns>任务</returns>
    private async Task ValidateRequestAsync(TaktDataCloneDto dto)
    {
        var validation = await _validator.ValidateAsync(dto).ConfigureAwait(false);
        if (!validation.IsValid)
        {
            throw new TaktBusinessException(validation.Errors[0].ErrorMessage);
        }
    }

    /// <summary>
    /// 构建克隆选项
    /// </summary>
    /// <param name="dto">克隆请求</param>
    /// <returns>克隆选项</returns>
    private static TaktDataCloneOptions BuildOptions(TaktDataCloneDto dto)
    {
        return new TaktDataCloneOptions
        {
            SourceTenantCode = dto.SourceTenantCode.Trim(),
            SourceDatabaseName = dto.SourceDatabaseName.Trim(),
            SourceTableName = dto.SourceTableName.Trim(),
            SourceCompanyCode = dto.SourceCompanyCode.Trim(),
            TargetTenantCode = dto.TargetTenantCode.Trim(),
            TargetDatabaseName = dto.TargetDatabaseName.Trim(),
            TargetTableName = dto.TargetTableName.Trim(),
            TargetCompanyCode = dto.TargetCompanyCode.Trim(),
            PreserveIdentityValues = dto.PreserveIdentityValues
        };
    }

    /// <summary>
    /// 映射备份预览为 DTO
    /// </summary>
    /// <param name="preview">提供者预览结果</param>
    /// <returns>预览 DTO</returns>
    private static TaktDataClonePreviewDto MapPreview(TaktCloneTargetBackupPreview preview)
    {
        return new TaktDataClonePreviewDto
        {
            TargetTableName = preview.TargetTableName,
            TargetCompanyCode = preview.TargetCompanyCode ?? string.Empty,
            TargetRowCount = preview.TargetRowCount,
            PlannedBackupTableName = preview.PlannedBackupTableName,
            BackupDescription = preview.BackupDescription,
            ClearDescription = preview.ClearDescription,
            WarningMessage = preview.WarningMessage,
            ConfirmHint = "我已阅读备份窗口提示，确认目标公司将先备份再清空（执行克隆时须勾选 ConfirmTargetBackupAndClear）"
        };
    }

    /// <summary>
    /// 映射克隆结果为 DTO
    /// </summary>
    /// <param name="result">提供者返回结果</param>
    /// <returns>克隆结果 DTO</returns>
    private static TaktDataCloneResultDto MapResult(TaktDataCloneResult result)
    {
        return new TaktDataCloneResultDto
        {
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
