// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcGijutsuService.cs
// 创建时间：2026-06-30
// 创建人：Takt365(Cursor AI)
// 功能描述：设变技术课主应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Globalization;
using System.Linq.Expressions;
using Mapster;
using Microsoft.Extensions.Configuration;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Entities.HumanResource.Organization;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Repositories;
using Takt.Domain.Interfaces;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Shared.Constants;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变技术课主应用服务
/// </summary>
public class TaktEcGijutsuService : TaktServiceBase, ITaktEcGijutsuService
{
    private readonly ITaktCompanyRepository<TaktEcGijutsu> _ecEngRepository;
    private readonly ITaktCompanyRepository<TaktEcDetail> _ecDetailRepository;
    private readonly ITaktCompanyRepository<TaktEcAttachment> _ecAttachmentRepository;
    private readonly ITaktApprovalRepository<TaktEcNotification> _ecNotificationRepository;
    private readonly ITaktCompanyRepository<TaktSourceEc> _sourceEcRepository;
    private readonly ITaktCompanyRepository<TaktSourceEcDetail> _sourceEcDetailRepository;
    private readonly ITaktCompanyRepository<TaktMaterialPlant> _materialPlantRepository;
    private readonly ITaktTenantRepository<TaktModelDestination> _modelDestinationRepository;
    private readonly ITaktCompanyRepository<TaktDept> _deptRepository;
    private readonly TaktEcExecPersistence _ecExecPersistence;
    private readonly TaktEcScopeExecOrchestrator _ecScopeExecOrchestrator;
    private readonly TaktEcHandoffMailService _handoffMailService;
    private readonly TaktEcGijutsuStatusSynchronizer _ecGijutsuStatusSynchronizer;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;
    private readonly ITaktNumberingGenerator _numberingGenerator;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecEngRepository">设变技术课主仓储</param>
    /// <param name="ecDetailRepository">EcDetail仓储</param>
    /// <param name="ecAttachmentRepository">EcAttachment仓储</param>
    /// <param name="ecNotificationRepository">EcNotification仓储</param>
    /// <param name="sourceEcRepository">设变来源主仓储</param>
    /// <param name="sourceEcDetailRepository">设变来源子仓储</param>
    /// <param name="materialPlantRepository">工厂物料仓储</param>
    /// <param name="modelDestinationRepository">型号目的地仓储</param>
    /// <param name="deptRepository">部门仓储</param>
    /// <param name="ecExecPersistence">设变部门执行持久化</param>
    /// <param name="ecScopeExecOrchestrator">设变实施范围执行编排</param>
    /// <param name="handoffMailService">实施顺序邮件</param>
    /// <param name="ecGijutsuStatusSynchronizer">按执行部门输入回写设变状态</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="numberingGenerator">业务编码生成器</param>
    /// <param name="configuration">应用配置</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEcGijutsuService(
        ITaktCompanyRepository<TaktEcGijutsu> ecEngRepository,
        ITaktCompanyRepository<TaktEcDetail> ecDetailRepository,
        ITaktCompanyRepository<TaktEcAttachment> ecAttachmentRepository,
        ITaktApprovalRepository<TaktEcNotification> ecNotificationRepository,
        ITaktCompanyRepository<TaktSourceEc> sourceEcRepository,
        ITaktCompanyRepository<TaktSourceEcDetail> sourceEcDetailRepository,
        ITaktCompanyRepository<TaktMaterialPlant> materialPlantRepository,
        ITaktTenantRepository<TaktModelDestination> modelDestinationRepository,
        ITaktCompanyRepository<TaktDept> deptRepository,
        TaktEcExecPersistence ecExecPersistence,
        TaktEcScopeExecOrchestrator ecScopeExecOrchestrator,
        TaktEcHandoffMailService handoffMailService,
        TaktEcGijutsuStatusSynchronizer ecGijutsuStatusSynchronizer,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktNumberingGenerator numberingGenerator,
        IConfiguration configuration,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _ecEngRepository = ecEngRepository;
        _ecDetailRepository = ecDetailRepository;
        _ecAttachmentRepository = ecAttachmentRepository;
        _ecNotificationRepository = ecNotificationRepository;
        _sourceEcRepository = sourceEcRepository;
        _sourceEcDetailRepository = sourceEcDetailRepository;
        _materialPlantRepository = materialPlantRepository;
        _modelDestinationRepository = modelDestinationRepository;
        _deptRepository = deptRepository;
        _ecExecPersistence = ecExecPersistence;
        _ecScopeExecOrchestrator = ecScopeExecOrchestrator;
        _handoffMailService = handoffMailService;
        _ecGijutsuStatusSynchronizer = ecGijutsuStatusSynchronizer;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
        _numberingGenerator = numberingGenerator;
        _configuration = configuration;
    }

    /// <summary>
    /// 获取设变技术课主列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEcGijutsuDto>> GetEcGijutsuListAsync(TaktEcGijutsuQueryDto queryDto)
    {
        queryDto ??= new TaktEcGijutsuQueryDto();
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _ecEngRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktEcGijutsuDto>.Create(
            MapHeaderList(data),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取设变技术课主
    /// </summary>
    /// <param name="id">设变技术课主ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcGijutsuDto?> GetEcGijutsuByIdAsync(long id)
    {
        var entity = await _ecEngRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktEcGijutsuDto>();
        await FillEcGijutsuDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取设变技术课主选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetEcGijutsuOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _ecEngRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ChangeStatus == 1,
            x => x.PlantCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.PlantCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建设变技术课主
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcGijutsuDto> CreateEcGijutsuAsync(TaktEcGijutsuCreateDto dto)
    {
        await MaterializeEcDetailsFromSourceIfNeededAsync(dto);
        await EnsureEcGijutsuCreateReadyAsync(dto);
        var entity = dto.Adapt<TaktEcGijutsu>();
        entity.EcDeptScope = TaktEcDeptScopeConstants.Normalize(entity.EcScope, entity.EcDeptScope);
        entity.EcStatus = TaktEcGijutsuStatusConstants.Issued;
        entity = await _ecEngRepository.CreateAsync(entity);
        TaktLogger.Information(
            "[EcGijutsuPersist] 主表已落库 EcCode={EcCode} Id={Id} DetailCount={DetailCount}",
            entity.EcCode,
            entity.Id,
            dto.EcDetails?.Count ?? 0);
        await SaveEcGijutsuChildrenAsync(entity, dto);
        TaktLogger.Information(
            "[EcGijutsuPersist] 子表（明细/附件）已落库 EcCode={EcCode}",
            entity.EcCode);
        var notificationDate = dto.EcEntryDate != default ? dto.EcEntryDate : DateTime.Today;
        var applyResult = await ApplyScopeExecAndMaybeNotifyAsync(entity, createNotificationIfMissing: true, notificationDate);
        var result = await GetEcGijutsuByIdAsync(entity.Id) ?? entity.Adapt<TaktEcGijutsuDto>();
        result.PersistDeptExecSummary = applyResult.FormatSummary();
        result.PersistDetailCount = await CountLiveEcDetailsAsync(entity.Id);
        return result;
    }

    /// <summary>
    /// 入队前校验新增（唯一键等；不落库）
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>任务</returns>
    public async Task EnsureEcGijutsuCreateReadyAsync(TaktEcGijutsuCreateDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        var plantCode = dto.PlantCode?.Trim() ?? string.Empty;
        var ecCode = dto.EcCode?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(ecCode))
        {
            throw new TaktBusinessException("设变单号不能为空");
        }
        var isUnique = await _uniqueValidator.IsUniqueAsync(
            _ecEngRepository,
            x => x.PlantCode == plantCode && x.EcCode == ecCode);
        if (!isUnique)
        {
            throw new TaktBusinessException("设变技术课主的PlantCode、EcCode已存在");
        }
    }

    /// <summary>
    /// 更新设变技术课主
    /// </summary>
    /// <param name="id">设变技术课主ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcGijutsuDto> UpdateEcGijutsuAsync(long id, TaktEcGijutsuUpdateDto dto)
    {
        await EnsureEcGijutsuUpdateReadyAsync(id, dto);
        var entity = await _ecEngRepository.GetByIdAsync(id)
            ?? throw new TaktBusinessException("设变技术课主不存在");
        var previousEcStatus = entity.EcStatus;
        var previousEcScope = entity.EcScope;
        var previousEcDeptScope = entity.EcDeptScope;
        dto.Adapt(entity);
        entity.EcDeptScope = TaktEcDeptScopeConstants.Normalize(entity.EcScope, entity.EcDeptScope);
        entity.EcStatus = previousEcStatus;
        if (previousEcStatus == TaktEcGijutsuStatusConstants.Completed)
        {
            entity.EcScope = previousEcScope;
            entity.EcDeptScope = previousEcDeptScope;
        }
        var scopeChanged = previousEcScope != entity.EcScope || previousEcDeptScope != entity.EcDeptScope;
        await _ecEngRepository.UpdateAsync(entity);
        TaktLogger.Information(
            "[EcGijutsuPersist] 主表已更新 EcCode={EcCode} Id={Id} DetailCount={DetailCount}",
            entity.EcCode,
            entity.Id,
            dto.EcDetails?.Count ?? 0);
        await SaveEcGijutsuChildrenAsync(entity, dto);
        TaktLogger.Information(
            "[EcGijutsuPersist] 子表（明细/附件）已更新 EcCode={EcCode}",
            entity.EcCode);
        // 提交了明细：明细已带上新范围，按范围重算各部门执行行。
        // 未提交明细但实施范围或部门实施范围变了：先把新范围写回已有明细，再重算，避免执行子表仍按旧粒度去重。
        // 范围未变且无明细载荷：不重跑（明细由右侧分页维护，避免十万行重复 Upsert）。
        TaktEcScopeExecApplyResult applyResult;
        if (dto.EcDetails is { Count: > 0 })
        {
            applyResult = await ApplyScopeExecAndMaybeNotifyAsync(entity, createNotificationIfMissing: false);
        }
        else if (scopeChanged)
        {
            await SyncDetailScopeFromMasterAsync(entity);
            applyResult = await ApplyScopeExecAndMaybeNotifyAsync(entity, createNotificationIfMissing: false);
        }
        else
        {
            applyResult = TaktEcScopeExecApplyResult.Empty;
            await _ecGijutsuStatusSynchronizer.RefreshByEcCodeAsync(entity.EcCode);
        }
        var result = await GetEcGijutsuByIdAsync(id) ?? throw new TaktBusinessException("设变技术课主不存在");
        result.PersistDeptExecSummary = applyResult.FormatSummary();
        result.PersistDetailCount = await CountLiveEcDetailsAsync(entity.Id);
        return result;
    }

    /// <summary>
    /// 统计设变明细未作废行数（表 takt_logistics_manufacturing_ec_detail，仓储已排除软删）。
    /// </summary>
    /// <param name="ecGijutsuId">设变技术课主表 Id</param>
    /// <returns>未作废明细行数</returns>
    public async Task<int> CountLiveEcDetailsAsync(long ecGijutsuId)
    {
        return await _ecDetailRepository.CountAsync(x => x.EcGijutsuId == ecGijutsuId && x.IsObsolete == 0);
    }

    /// <summary>
    /// 入队前校验更新（存在性、唯一键等；不落库）
    /// </summary>
    /// <param name="id">设变技术课主ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>任务</returns>
    public async Task EnsureEcGijutsuUpdateReadyAsync(long id, TaktEcGijutsuUpdateDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        var entity = await _ecEngRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("设变技术课主不存在");
        }
        var plantCode = string.IsNullOrWhiteSpace(dto.PlantCode) ? entity.PlantCode : dto.PlantCode.Trim();
        var ecCode = string.IsNullOrWhiteSpace(dto.EcCode) ? entity.EcCode : dto.EcCode.Trim();
        if (string.IsNullOrWhiteSpace(ecCode))
        {
            throw new TaktBusinessException("设变单号不能为空");
        }
        var isUnique = await _uniqueValidator.IsUniqueAsync(
            _ecEngRepository,
            x => x.PlantCode == plantCode && x.EcCode == ecCode,
            id);
        if (!isUnique)
        {
            throw new TaktBusinessException("设变技术课主的PlantCode、EcCode已存在");
        }
    }

    /// <summary>
    /// 删除设变技术课主
    /// </summary>
    /// <param name="id">设变技术课主ID</param>
    /// <returns>任务</returns>
    public async Task DeleteEcGijutsuByIdAsync(long id)
    {
        var entity = await _ecEngRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("设变技术课主不存在或已删除");
        }
        await DeleteDeptExecRowsForEcAsync(entity);
        await _ecDetailRepository.DeleteAsync(x => x.EcGijutsuId == entity.Id);
        await _ecAttachmentRepository.DeleteAsync(x => x.EcGijutsuId == entity.Id);
        await _ecNotificationRepository.DeleteAsync(x => x.EcGijutsuId == entity.Id);
        var deleted = await _ecEngRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("设变技术课主不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除设变技术课主
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteEcGijutsuBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteEcGijutsuByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新设变技术课主状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcGijutsuDto> UpdateEcGijutsuStatusAsync(TaktEcGijutsuStatusDto dto)
    {
        var entity = await _ecEngRepository.GetByIdAsync(dto.EcGijutsuId);
        if (entity == null)
        {
            throw new TaktBusinessException("设变技术课主不存在");
        }
        entity.ChangeStatus = dto.ChangeStatus;
        await _ecEngRepository.UpdateAsync(entity);
        return await GetEcGijutsuByIdAsync(dto.EcGijutsuId) ?? throw new TaktBusinessException("设变技术课主不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetEcGijutsuTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktEcGijutsuTemplateDto>(
            sheetName ?? "设变技术课主导入模板",
            fileName ?? "设变技术课主导入模板.xlsx");
    }

    /// <summary>
    /// 导入设变技术课主
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportEcGijutsuAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktEcGijutsuImportDto>(fileStream, sheetName ?? "设变技术课主导入模板");
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
                var entity = rows[i].Adapt<TaktEcGijutsu>();
                var importKey = $"{entity.PlantCode}|{entity.EcCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、EcCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_ec_gijutsu_plant_ec_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _ecEngRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.EcCode == entity.EcCode);
                if (!isUnique_ix_takt_logistics_manufacturing_ec_gijutsu_plant_ec_code_unique)
                {
                    throw new TaktBusinessException("设变技术课主的PlantCode、EcCode已存在");
                }
                await _ecEngRepository.CreateAsync(entity);
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
    /// 导出设变技术课主
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEcGijutsuAsync(TaktEcGijutsuQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktEcGijutsuQueryDto());
        var list = await _ecEngRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEcGijutsuExportDto>(),
                sheetName ?? "设变技术课主数据",
                fileName ?? "设变技术课主导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktEcGijutsuExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "设变技术课主数据",
            fileName ?? "设变技术课主导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充设变技术课主详情：仅附件与通知。
    /// 设变明细不在此灌入（可数十万行）；由主从表右侧 TaktEcDetails/list 按主表 Id 分页查询。
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillEcGijutsuDetailsAsync(TaktEcGijutsuDto dto, TaktEcGijutsu entity)
    {
        if (dto == null)
        {
            return;
        }
        // 明细走独立分页 API，禁止 GetById 全量拉 EcDetails
        dto.EcDetails = [];
        var attachments = await _ecAttachmentRepository.GetListAsync(x => x.EcGijutsuId == entity.Id);
        dto.Attachments = attachments.Adapt<List<TaktEcAttachmentDto>>();
        var notifications = await _ecNotificationRepository.GetListAsync(x => x.EcGijutsuId == entity.Id);
        dto.Notifications = notifications.Adapt<List<TaktEcNotificationDto>>();
    }

    /// <summary>
    /// 将主表实施范围、部门实施范围写回该设变下已有明细（不改物料与行号）
    /// </summary>
    /// <param name="entity">已规范化范围的设变主表</param>
    /// <returns>任务</returns>
    private async Task SyncDetailScopeFromMasterAsync(TaktEcGijutsu entity)
    {
        var ecScope = entity.EcScope;
        var ecDeptScope = entity.EcDeptScope;
        var updated = await _ecDetailRepository.UpdateAsync(
            x => x.EcGijutsuId == entity.Id,
            x => new TaktEcDetail
            {
                EcScope = ecScope,
                EcDeptScope = ecDeptScope,
            });
        TaktLogger.Information(
            "[EcGijutsuPersist] 实施范围已同步到明细 EcCode={EcCode} EcScope={EcScope} EcDeptScope={EcDeptScope} Updated={Updated}",
            entity.EcCode,
            ecScope,
            ecDeptScope,
            updated);
    }

    /// <summary>
    /// 保存设变技术课主子表级联（设变明细、设变附件、工程变更通知单）。
    /// 明细：仅当提交非空 EcDetails 时先删后插；空载荷保留库内已有明细（视图内明细由独立 CRUD 维护）。
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveEcGijutsuChildrenAsync(TaktEcGijutsu entity, TaktEcGijutsuCreateDto dto)
    {
        TaktLogger.Information(
            "[EcGijutsuPersist] 开始保存子表 EcCode={EcCode} DetailCount={DetailCount}",
            entity.EcCode,
            dto.EcDetails?.Count ?? 0);
        // 设变明细（EcDetails）：有载荷才覆盖；空=不碰已有（避免编辑主表时误删十万行）
        if (dto.EcDetails is { Count: > 0 })
        {
            await DeleteDeptExecRowsForEcAsync(entity);
            TaktLogger.Information(
                "[EcGijutsuPersist] 明细 DTO→实体映射开始 Count={Count} EcCode={EcCode}",
                dto.EcDetails.Count,
                entity.EcCode);
            var ecdetails = dto.EcDetails.Adapt<List<TaktEcDetail>>();
            foreach (var child in ecdetails)
            {
                child.EcGijutsuId = entity.Id;
                // 冗余回填：主表 TaktEcGijutsu.EcScope → 明细
                child.EcScope = entity.EcScope;
                child.EcDeptScope = entity.EcDeptScope;
                if (string.IsNullOrWhiteSpace(child.EcCode))
                {
                    child.EcCode = entity.EcCode;
                }
                if (string.IsNullOrWhiteSpace(child.PlantCode))
                {
                    child.PlantCode = entity.PlantCode;
                }
            }
            TaktLogger.Information(
                "[EcGijutsuPersist] 明细映射完成 Count={Count} EcCode={EcCode}",
                ecdetails.Count,
                entity.EcCode);
            var ecdetailsNeedLine = ecdetails.Where(c => c.LineNumber <= 0).ToList();
            if (ecdetailsNeedLine.Count > 0)
            {
                TaktLogger.Information(
                    "[EcGijutsuPersist] 明细行号补齐 NeedLine={NeedLine} EcCode={EcCode}",
                    ecdetailsNeedLine.Count,
                    entity.EcCode);
                var businessCode = entity.Id.ToString();
                foreach (var grp in ecdetailsNeedLine.GroupBy(c => c.EcRootMaterialCode ?? string.Empty, StringComparer.Ordinal))
                {
                    var fg = grp.Key;
                    var maxLine = await _ecDetailRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode
                            && x.CompanyCode == CurrentCompanyCode
                            && x.EcGijutsuId == entity.Id
                            && x.EcRootMaterialCode == fg,
                        x => x.LineNumber);
                    var assignedInBatch = ecdetails
                        .Where(c => c.LineNumber > 0
                            && string.Equals(c.EcRootMaterialCode ?? string.Empty, fg, StringComparison.Ordinal))
                        .Select(c => c.LineNumber)
                        .DefaultIfEmpty(0)
                        .Max();
                    maxLine = Math.Max(maxLine, assignedInBatch);
                    var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, grp.Count(), maxLine).ToList();
                    var lineIdx = 0;
                    foreach (var child in grp)
                    {
                        child.LineNumber = lineSeq[lineIdx++];
                    }
                }
            }
            var seenKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < ecdetails.Count; i++)
            {
                var key = $"{ecdetails[i].CompanyCode}|{ecdetails[i].EcGijutsuId}|{ecdetails[i].EcRootMaterialCode}|{ecdetails[i].LineNumber}";
                if (!seenKeys.Add(key))
                {
                    throw new TaktBusinessException($"设变明细第{i + 1}项与本次提交的其他项重复（CompanyCode、EcGijutsuId、EcRootMaterialCode、LineNumber）");
                }
            }
            await _ecDetailRepository.DeleteAsync(x => x.EcGijutsuId == entity.Id);
            // 刚按 EcGijutsuId 整表删除后，内存去重已足够；禁止再逐行 IsUniqueAsync（10 万行会拖死）
            // 分批 CreateRange：底层 InsertEntitiesAsync 亦按 500 行切分；此处按批打进度日志
            await CreateEcDetailsInBatchesAsync(entity.EcCode, ecdetails);
        }
        else
        {
            TaktLogger.Information(
                "[EcGijutsuPersist] 明细载荷为空，保留库内已有明细 EcCode={EcCode}",
                entity.EcCode);
        }
        // 设变附件：未提交（null）不改库；仅当本次载荷相对库内行有增删改时才写对应行
        if (dto.Attachments == null)
        {
            TaktLogger.Information(
                "[EcGijutsuPersist] 附件载荷未提交，保留库内已有附件 EcCode={EcCode}",
                entity.EcCode);
        }
        else if (dto.Attachments.Count == 0)
        {
            await _ecAttachmentRepository.DeleteAsync(x => x.EcGijutsuId == entity.Id);
        }
        else
        {
            var attachments = dto.Attachments.Adapt<List<TaktEcAttachment>>();
            foreach (var child in attachments)
            {
                child.EcGijutsuId = entity.Id;
                if (string.IsNullOrWhiteSpace(child.EcCode))
                {
                    child.EcCode = entity.EcCode;
                }
                if (!TaktEcAttachmentDocCodeHelper.IsValidDocCode(child.AttachmentType, child.DocCode, child.EcCode))
                {
                    var hint = TaktEcAttachmentDocCodeHelper.GetFormatHint(child.AttachmentType);
                    throw new TaktBusinessException(string.IsNullOrEmpty(hint)
                        ? $"设变附件文件编码「{child.DocCode}」格式不正确"
                        : $"设变附件文件编码「{child.DocCode}」格式不正确（{hint}）");
                }
                var fileSource = string.IsNullOrWhiteSpace(child.FileName) ? child.AccessUrl : child.FileName;
                child.FileName = TaktEcAttachmentDocCodeHelper.BuildFileNameFromDocCode(
                    child.DocCode,
                    fileSource,
                    child.AccessUrl);
            }
            var attachmentsNeedLine = attachments.Where(c => c.LineNumber <= 0).ToList();
            if (attachmentsNeedLine.Count > 0)
            {
                var businessCode = entity.Id.ToString();
                var maxLine = await _ecAttachmentRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.EcGijutsuId == entity.Id,
                    x => x.LineNumber);
                var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, attachmentsNeedLine.Count, maxLine).ToList();
                var lineIdx = 0;
                foreach (var child in attachments)
                {
                    if (child.LineNumber <= 0)
                    {
                        child.LineNumber = lineSeq[lineIdx++];
                    }
                }
            }
            var seenKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < attachments.Count; i++)
            {
                var doc = (attachments[i].DocCode ?? string.Empty).Trim();
                var key = $"{attachments[i].CompanyCode}|{attachments[i].EcGijutsuId}|{doc}|{attachments[i].LineNumber}";
                if (!seenKeys.Add(key))
                {
                    throw new TaktBusinessException($"设变附件第{i + 1}项与本次提交的其他项重复（CompanyCode、EcGijutsuId、DocCode、LineNumber）");
                }
            }
            await ApplyChangedEcAttachmentsAsync(entity.Id, attachments);
        }
        // 工程变更通知单（Notifications）
        if (dto.Notifications is not { Count: > 0 })
        {
            await _ecNotificationRepository.DeleteAsync(x => x.EcGijutsuId == entity.Id);
        }
        else
        {
            var notifications = dto.Notifications.Adapt<List<TaktEcNotification>>();
            foreach (var child in notifications)
            {
                child.EcGijutsuId = entity.Id;
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < notifications.Count; i++)
                        {
                            var key = $"{notifications[i].CompanyCode}|{notifications[i].EcNotificationCode}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"工程变更通知单第{i + 1}项与本次提交的其他项重复（CompanyCode、EcNotificationCode）");
                            }
                        }
            await _ecNotificationRepository.DeleteAsync(x => x.EcGijutsuId == entity.Id);
            foreach (var child in notifications)
            {
            var isUnique_ix_takt_logistics_manufacturing_ec_notification_code_unique = await _uniqueValidator.IsUniqueAsync(
                _ecNotificationRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.EcNotificationCode == child.EcNotificationCode);
            if (!isUnique_ix_takt_logistics_manufacturing_ec_notification_code_unique)
            {
                throw new TaktBusinessException("工程变更通知单的CompanyCode、EcNotificationCode已存在");
            }
            }
            await _ecNotificationRepository.CreateRangeAsync(notifications);
        }
    }

    /// <summary>
    /// 按本次提交差异写设变附件：内容未变的行不更新，仅插入新行、更新改过的行、删除提交中已去掉的行。
    /// </summary>
    /// <param name="ecGijutsuId">设变技术课主表 Id</param>
    /// <param name="attachments">已校验的附件实体（Id 大于 0 表示已有行）</param>
    private async Task ApplyChangedEcAttachmentsAsync(long ecGijutsuId, List<TaktEcAttachment> attachments)
    {
        var existing = await _ecAttachmentRepository.GetListAsync(x => x.EcGijutsuId == ecGijutsuId);
        var existingById = existing.ToDictionary(x => x.Id);
        var keptIds = new HashSet<long>();
        var toInsert = new List<TaktEcAttachment>();
        foreach (var child in attachments)
        {
            if (child.Id > 0 && existingById.TryGetValue(child.Id, out var current))
            {
                keptIds.Add(current.Id);
                if (IsSameEcAttachmentContent(current, child))
                {
                    continue;
                }
                current.EcCode = child.EcCode;
                current.LineNumber = child.LineNumber;
                current.AttachmentType = child.AttachmentType;
                current.DocCode = (child.DocCode ?? string.Empty).Trim();
                current.FileName = child.FileName;
                current.AccessUrl = child.AccessUrl;
                current.IsObsolete = child.IsObsolete;
                if (!string.IsNullOrWhiteSpace(child.PlantCode))
                {
                    current.PlantCode = child.PlantCode;
                }
                current.Remark = child.Remark;
                current.ExtField = child.ExtField;
                await _ecAttachmentRepository.UpdateAsync(current);
                continue;
            }
            child.Id = 0;
            toInsert.Add(child);
        }
        var removedIds = existing.Where(x => !keptIds.Contains(x.Id)).Select(x => x.Id).ToList();
        if (removedIds.Count > 0)
        {
            await _ecAttachmentRepository.DeleteAsync(x => removedIds.Contains(x.Id));
        }
        foreach (var child in toInsert)
        {
            var trimmedDoc = (child.DocCode ?? string.Empty).Trim();
            var isUnique = await _uniqueValidator.IsUniqueAsync(
                _ecAttachmentRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.EcGijutsuId == child.EcGijutsuId
                    && x.DocCode == trimmedDoc
                    && x.LineNumber == child.LineNumber);
            if (!isUnique)
            {
                throw new TaktBusinessException("设变附件的CompanyCode、EcGijutsuId、DocCode、LineNumber已存在");
            }
        }
        if (toInsert.Count > 0)
        {
            await _ecAttachmentRepository.CreateRangeAsync(toInsert);
        }
    }

    /// <summary>
    /// 比较附件业务内容是否一致（一致则本次保存不写该行）。
    /// </summary>
    /// <param name="current">库内行</param>
    /// <param name="incoming">本次提交行</param>
    /// <returns>内容一致返回 true</returns>
    private static bool IsSameEcAttachmentContent(TaktEcAttachment current, TaktEcAttachment incoming)
    {
        return string.Equals(current.EcCode, incoming.EcCode, StringComparison.Ordinal)
            && current.LineNumber == incoming.LineNumber
            && string.Equals(current.AttachmentType, incoming.AttachmentType, StringComparison.Ordinal)
            && string.Equals((current.DocCode ?? string.Empty).Trim(), (incoming.DocCode ?? string.Empty).Trim(), StringComparison.Ordinal)
            && string.Equals(current.FileName, incoming.FileName, StringComparison.Ordinal)
            && string.Equals(current.AccessUrl, incoming.AccessUrl, StringComparison.Ordinal)
            && current.IsObsolete == incoming.IsObsolete
            && string.Equals(current.Remark ?? string.Empty, incoming.Remark ?? string.Empty, StringComparison.Ordinal)
            && string.Equals(current.ExtField ?? string.Empty, incoming.ExtField ?? string.Empty, StringComparison.Ordinal);
    }

    /// <summary>
    /// 获取设变技术课主表统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>设变统计</returns>
    public async Task<TaktEcGijutsuStatDto> GetEcGijutsuStatAsync(TaktEcGijutsuStatQueryDto queryDto)
    {
        EnsureThreeLayerContext();
        var (start, end, statMonth) = TaktStatMonthRangeHelper.ResolveMonthRange(
            queryDto.EcEntryDateStart,
            queryDto.EcEntryDateEnd);
        var ecIdsInRange = (await _ecEngRepository.GetListAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.EcEntryDate >= start
            && x.EcEntryDate <= end))
            .Select(x => x.Id)
            .ToHashSet();
        var ecDetails = ecIdsInRange.Count == 0
            ? []
            : await _ecDetailRepository.GetListAsync(x =>
                x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && ecIdsInRange.Contains(x.EcGijutsuId));
        if (!string.IsNullOrEmpty(queryDto.PlantCode))
        {
            var plantEcIds = (await _ecEngRepository.GetListAsync(x =>
                x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == queryDto.PlantCode))
                .Select(x => x.Id)
                .ToHashSet();
            ecDetails = ecDetails.Where(x => plantEcIds.Contains(x.EcGijutsuId)).ToList();
        }
        return new TaktEcGijutsuStatDto
        {
            StatMonth = statMonth,
            EcCount = ecDetails.Select(x => x.EcGijutsuId).Distinct().Count(),
            EcDetailCount = ecDetails.Count,
            PlantCode = queryDto.PlantCode,
        };
    }

    // ========================================
    // 来源设变录入
    // ========================================

    /// <summary>
    /// 获取当前公司对应的来源设变目标工厂代码（Database:CompanyCodes 与 PlantCodes 同序映射）
    /// </summary>
    /// <returns>公司代码与映射工厂代码</returns>
    public Task<TaktEcGijutsuSourcePlantCodeDto> GetEcGijutsuSourcePlantCodeAsync()
    {
        EnsureThreeLayerContext();
        var companyCode = CurrentCompanyCode?.Trim() ?? string.Empty;
        if (companyCode.Length == 0)
        {
            throw new TaktBusinessException("当前公司代码不能为空");
        }
        var plantCode = ResolvePlantCodeFromSourceEcCompanyCode(companyCode);
        return Task.FromResult(new TaktEcGijutsuSourcePlantCodeDto
        {
            CompanyCode = companyCode,
            PlantCode = plantCode,
        });
    }

    /// <summary>
    /// 获取尚未导入设变技术课主的来源设变列表（分页）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEcGijutsuSourceEcInputItemDto>> GetUnimportedSourceEcGijutsuListAsync(TaktEcGijutsuSourceEcInputQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        EnsureThreeLayerContext();
        var plantCode = ResolvePlantCodeFromSourceEcCompanyCode(CurrentCompanyCode);
        AssertRequestedPlantCodeMatches(queryDto.PlantCode, plantCode, CurrentCompanyCode);
        var importedEcCodes = (await _ecEngRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plantCode))
            .Select(x => x.EcCode)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        var predicate = UnimportedSourceEcQueryExpression(queryDto, importedEcCodes);
        var (data, total) = await _sourceEcRepository.GetPagedAsync(
            predicate,
            queryDto.PageIndex,
            queryDto.PageSize,
            x => x.SourceIssueDate,
            false);
        if (data == null || data.Count == 0)
        {
            return TaktPagedResult<TaktEcGijutsuSourceEcInputItemDto>.Create(
                [],
                total,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var sourceEcIds = data.Select(x => x.Id).ToList();
        var sourceDetails = await _sourceEcDetailRepository.GetListAsync(x => sourceEcIds.Contains(x.SourceEcId));
        var detailCountMap = sourceDetails
            .GroupBy(x => x.SourceEcId)
            .ToDictionary(g => g.Key, g => g.Count());
        var items = data.Select(source => new TaktEcGijutsuSourceEcInputItemDto
        {
            SourceEcId = source.Id,
            SourceEcCode = source.SourceEcCode,
            SourceModel = source.SourceModel,
            SourceTitle = source.SourceTitle,
            SourceIssueDate = source.SourceIssueDate,
            SourceStatus = source.SourceStatus,
            SourceTcjOwner = source.SourceTcjOwner,
            DetailCount = detailCountMap.GetValueOrDefault(source.Id),
        }).ToList();
        return TaktPagedResult<TaktEcGijutsuSourceEcInputItemDto>.Create(
            items,
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 从来源设变导入设变技术课主、明细、附件，并初始化各部门执行行与设变通知
    /// </summary>
    /// <param name="dto">导入 DTO</param>
    /// <returns>导入结果</returns>
    public async Task<TaktEcGijutsuImportFromSourceResultDto> ImportEcGijutsuFromSourceAsync(TaktEcGijutsuImportFromSourceDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        EnsureThreeLayerContext();
        if (dto.SourceEcIds == null || dto.SourceEcIds.Count == 0)
        {
            throw new TaktBusinessException("请至少选择一条来源设变");
        }
        var result = new TaktEcGijutsuImportFromSourceResultDto();
        var today = DateTime.Today;
        foreach (var rawId in dto.SourceEcIds.Distinct(StringComparer.Ordinal))
        {
            if (!long.TryParse(rawId, out var sourceEcId) || sourceEcId <= 0)
            {
                result.FailCount += 1;
                result.Errors.Add($"来源设变ID无效: {rawId}");
                continue;
            }
            try
            {
                var sourceEc = await _sourceEcRepository.GetByIdAsync(sourceEcId);
                if (sourceEc == null
                    || sourceEc.TenantCode != CurrentTenantCode
                    || sourceEc.CompanyCode != CurrentCompanyCode)
                {
                    throw new TaktBusinessException($"来源设变不存在: {sourceEcId}");
                }
                var plantCode = ResolvePlantCodeFromSourceEcCompanyCode(sourceEc.CompanyCode);
                AssertRequestedPlantCodeMatches(dto.PlantCode, plantCode, sourceEc.CompanyCode);
                var alreadyImported = await _ecEngRepository.CountAsync(x =>
                    x.TenantCode == CurrentTenantCode
                    && x.CompanyCode == CurrentCompanyCode
                    && x.PlantCode == plantCode
                    && x.EcCode == sourceEc.SourceEcCode) > 0;
                if (alreadyImported)
                {
                    throw new TaktBusinessException($"设变 {sourceEc.SourceEcCode} 已在工厂 {plantCode} 导入");
                }
                var sourceDetails = await _sourceEcDetailRepository.GetListAsync(x => x.SourceEcId == sourceEc.Id);
                var createDto = await BuildEcGijutsuCreateFromSourceAsync(
                    sourceEc,
                    sourceDetails,
                    plantCode,
                    ResolveSourceInputCultureCode(dto.CompanyDefaultCulture, dto.CultureCode),
                    today);
                var created = await CreateEcGijutsuAsync(createDto);
                result.SuccessCount += 1;
                result.CreatedEcGijutsuIds.Add(created.EcGijutsuId.ToString());
            }
            catch (Exception ex)
            {
                result.FailCount += 1;
                result.Errors.Add(ex.Message);
            }
        }
        return result;
    }

    /// <summary>
    /// 从来源设变构建创建草稿 DTO（不落库；设变担当与实施范围须在前端 ec-form 填写后再 create）
    /// </summary>
    /// <param name="dto">草稿请求 DTO</param>
    /// <returns>创建 DTO</returns>
    public async Task<TaktEcGijutsuCreateDto> GetEcGijutsuDraftFromSourceEcAsync(TaktEcGijutsuDraftFromSourceDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        EnsureThreeLayerContext();
        if (!long.TryParse(dto.SourceEcId, out var sourceEcId) || sourceEcId <= 0)
        {
            throw new TaktBusinessException("来源设变ID无效");
        }
        var sourceEc = await _sourceEcRepository.GetByIdAsync(sourceEcId);
        if (sourceEc == null
            || sourceEc.TenantCode != CurrentTenantCode
            || sourceEc.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException($"来源设变不存在: {sourceEcId}");
        }
        var plantCode = ResolvePlantCodeFromSourceEcCompanyCode(sourceEc.CompanyCode);
        AssertRequestedPlantCodeMatches(dto.PlantCode, plantCode, sourceEc.CompanyCode);
        var alreadyImported = await _ecEngRepository.CountAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plantCode
            && x.EcCode == sourceEc.SourceEcCode) > 0;
        if (alreadyImported)
        {
            throw new TaktBusinessException($"设变 {sourceEc.SourceEcCode} 已在工厂 {plantCode} 导入");
        }
        var detailCount = await _sourceEcDetailRepository.CountAsync(x => x.SourceEcId == sourceEc.Id);
        // 超过阈值则不把明细塞进 HTTP 响应/浏览器（184k 行会远超 30MB 并拖垮前端）
        const int maxInlineDraftDetails = 5000;
        if (detailCount > maxInlineDraftDetails)
        {
            var headerOnly = await BuildEcGijutsuCreateFromSourceAsync(
                sourceEc,
                new List<TaktSourceEcDetail>(),
                plantCode,
                ResolveSourceInputCultureCode(dto.CompanyDefaultCulture, dto.CultureCode),
                DateTime.Today);
            headerOnly.EcLeader = string.Empty;
            headerOnly.EcScope = TaktEcScopeConstants.AllDestination;
            headerOnly.EcDeptScope = TaktEcDeptScopeConstants.ByModel;
            headerOnly.EcDetails = [];
            headerOnly.SourceEcId = sourceEc.Id.ToString(CultureInfo.InvariantCulture);
            headerOnly.DetailsDeferred = true;
            headerOnly.DeferredDetailCount = detailCount;
            TaktLogger.Information(
                "[EcGijutsuPersist] 来源草稿省略明细 SourceEcId={SourceEcId} DeferredDetailCount={Count}",
                headerOnly.SourceEcId,
                detailCount);
            return headerOnly;
        }
        var sourceDetails = await _sourceEcDetailRepository.GetListAsync(x => x.SourceEcId == sourceEc.Id);
        var createDto = await BuildEcGijutsuCreateFromSourceAsync(
            sourceEc,
            sourceDetails,
            plantCode,
            ResolveSourceInputCultureCode(dto.CompanyDefaultCulture, dto.CultureCode),
            DateTime.Today);
        createDto.EcLeader = string.Empty;
        createDto.EcScope = TaktEcScopeConstants.AllDestination;
        createDto.EcDeptScope = TaktEcDeptScopeConstants.ByModel;
        createDto.SourceEcId = sourceEc.Id.ToString(CultureInfo.InvariantCulture);
        return createDto;
    }

    /// <summary>
    /// 若 CreateDto 带 SourceEcId 且明细为空，则从来源表明细物化 EcDetails（服务端落库，避免浏览器回传十万行 JSON）
    /// </summary>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task MaterializeEcDetailsFromSourceIfNeededAsync(TaktEcGijutsuCreateDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        if (dto.EcDetails is { Count: > 0 })
        {
            return;
        }
        if (string.IsNullOrWhiteSpace(dto.SourceEcId)
            || !long.TryParse(dto.SourceEcId.Trim(), out var sourceEcId)
            || sourceEcId <= 0)
        {
            return;
        }
        var sourceEc = await _sourceEcRepository.GetByIdAsync(sourceEcId);
        if (sourceEc == null
            || sourceEc.TenantCode != CurrentTenantCode
            || sourceEc.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException($"来源设变不存在: {sourceEcId}");
        }
        var plantCode = ResolvePlantCodeFromSourceEcCompanyCode(sourceEc.CompanyCode);
        AssertRequestedPlantCodeMatches(dto.PlantCode, plantCode, sourceEc.CompanyCode);
        var sourceDetails = await _sourceEcDetailRepository.GetListAsync(x => x.SourceEcId == sourceEc.Id);
        var materialized = await BuildEcGijutsuCreateFromSourceAsync(
            sourceEc,
            sourceDetails,
            plantCode,
            string.IsNullOrWhiteSpace(dto.CultureCode) ? sourceEc.CultureCode : dto.CultureCode,
            dto.EcEntryDate != default ? dto.EcEntryDate : DateTime.Today);
        dto.EcDetails = materialized.EcDetails;
        dto.DetailsDeferred = false;
        dto.DeferredDetailCount = dto.EcDetails?.Count ?? 0;
        if (string.IsNullOrWhiteSpace(dto.EcCode))
        {
            dto.EcCode = materialized.EcCode;
        }
        if (string.IsNullOrWhiteSpace(dto.EcTitle))
        {
            dto.EcTitle = materialized.EcTitle;
        }
        if (string.IsNullOrWhiteSpace(dto.EcContent))
        {
            dto.EcContent = materialized.EcContent;
        }
        if (string.IsNullOrWhiteSpace(dto.PlantCode))
        {
            dto.PlantCode = plantCode;
        }
        TaktLogger.Information(
            "[EcGijutsuPersist] 已从来源设变物化明细 SourceEcId={SourceEcId} DetailCount={Count} EcCode={EcCode}",
            dto.SourceEcId,
            dto.EcDetails?.Count ?? 0,
            dto.EcCode);
    }

    /// <summary>
    /// 解析来源设变录入/导入请求中的区域文化（优先 CultureCode，其次公司默认文化）
    /// </summary>
    /// <param name="companyDefaultCulture">公司默认文化（兼容旧入参；写入实体列为 CultureCode）</param>
    /// <param name="cultureCode">区域文化编码</param>
    /// <returns>写入设变主表 CultureCode 的值</returns>
    private static string ResolveSourceInputCultureCode(string? companyDefaultCulture, string? cultureCode)
    {
        if (!string.IsNullOrWhiteSpace(cultureCode))
        {
            return cultureCode.Trim();
        }
        return companyDefaultCulture?.Trim() ?? string.Empty;
    }

    /// <summary>
    /// 由来源设变公司代码解析目标工厂代码（Database:CompanyCodes 与 PlantCodes 同序下标映射）
    /// </summary>
    /// <param name="companyCode">来源设变公司代码</param>
    /// <returns>工厂代码</returns>
    private string ResolvePlantCodeFromSourceEcCompanyCode(string? companyCode)
    {
        var code = companyCode?.Trim() ?? string.Empty;
        if (code.Length == 0)
        {
            throw new TaktBusinessException("来源设变公司代码不能为空，无法解析工厂代码");
        }
        try
        {
            return _configuration.RequireDatabase().GetPlantCodeForCompanyCode(code);
        }
        catch (InvalidOperationException ex)
        {
            throw new TaktBusinessException(ex.Message);
        }
    }

    /// <summary>
    /// 校验前端传入的工厂代码与来源设变公司代码映射结果一致
    /// </summary>
    /// <param name="requestedPlantCode">前端传入的工厂代码（可为空）</param>
    /// <param name="resolvedPlantCode">服务端解析的工厂代码</param>
    /// <param name="companyCode">来源设变公司代码</param>
    private static void AssertRequestedPlantCodeMatches(string? requestedPlantCode, string resolvedPlantCode, string companyCode)
    {
        if (string.IsNullOrWhiteSpace(requestedPlantCode))
        {
            return;
        }
        if (!string.Equals(requestedPlantCode.Trim(), resolvedPlantCode, StringComparison.OrdinalIgnoreCase))
        {
            throw new TaktBusinessException($"工厂代码须与公司代码映射一致（{companyCode}→{resolvedPlantCode}）");
        }
    }

    /// <summary>
    /// 将来源设变主从映射为设变技术课创建 DTO（SourceTcjOwner 不写入 EcLeader，须导入后手工指定设变担当）
    /// </summary>
    /// <param name="sourceEc">来源设变主</param>
    /// <param name="sourceDetails">来源设变明细</param>
    /// <param name="plantCode">目标工厂</param>
    /// <param name="companyDefaultCulture">公司默认文化</param>
    /// <param name="entryDate">录入日期</param>
    /// <returns>创建 DTO</returns>
    private async Task<TaktEcGijutsuCreateDto> BuildEcGijutsuCreateFromSourceAsync(
        TaktSourceEc sourceEc,
        List<TaktSourceEcDetail> sourceDetails,
        string plantCode,
        string? companyDefaultCulture,
        DateTime entryDate)
    {
        var ecCode = sourceEc.SourceEcCode ?? string.Empty;
        var materialCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var rootMaterialCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var detail in sourceDetails)
        {
            TaktEcDetailMaterialPlantMapper.CollectMaterialCode(detail.SourceRootMaterialCode, materialCodes);
            TaktEcDetailMaterialPlantMapper.CollectMaterialCode(detail.SourceRootMaterialCode, rootMaterialCodes);
            TaktEcDetailMaterialPlantMapper.CollectMaterialCode(detail.SourceParentMaterialCode, materialCodes);
            TaktEcDetailMaterialPlantMapper.CollectMaterialCode(detail.SourceOldMaterialCode, materialCodes);
            TaktEcDetailMaterialPlantMapper.CollectMaterialCode(detail.SourceNewMaterialCode, materialCodes);
        }
        var materialsByCode = await LoadMaterialPlantsByCodesAsync(plantCode, materialCodes);
        var modelCodeByRootMaterial = await LoadModelCodesByMaterialCodesAsync(rootMaterialCodes);
        var fallbackModelCode = sourceEc.SourceModel?.Trim() ?? string.Empty;
        var cultureCode = companyDefaultCulture ?? string.Empty;
        var ecDetails = TaktEcSourceEcMapper.MapDetailCreateDtos(
            sourceDetails,
            plantCode,
            ecCode,
            CurrentTenantCode,
            CurrentCompanyCode,
            cultureCode,
            sourceEc.SourceIssueDate,
            fallbackModelCode,
            materialsByCode,
            modelCodeByRootMaterial);
        return new TaktEcGijutsuCreateDto
        {
            TenantCode = CurrentTenantCode,
            CompanyCode = CurrentCompanyCode,
            CultureCode = companyDefaultCulture ?? string.Empty,
            PlantCode = plantCode,
            EcCode = ecCode,
            EcIssueDate = sourceEc.SourceIssueDate,
            ChangeStatus = TaktEcSourceStatusMapper.MapToChangeStatusOrThrow(sourceEc.SourceStatus),
            EcTitle = sourceEc.SourceTitle,
            EcContent = sourceEc.SourceEcContent,
            EcLeader = string.Empty,
            EcLossAmount = sourceEc.SourceUnitCost + sourceEc.SourceMoldModificationCost,
            EcScope = TaktEcScopeConstants.AllDestination,
            EcDeptScope = TaktEcDeptScopeConstants.ByModel,
            EcEntryDate = entryDate,
            EcStatus = 1,
            EcDetails = ecDetails,
            Attachments = TaktEcSourceAttachmentMapper.MapAttachments(
                sourceEc,
                ecCode,
                CurrentTenantCode,
                CurrentCompanyCode,
                companyDefaultCulture ?? string.Empty).Adapt<List<TaktEcAttachmentCreateDto>>(),
        };
    }

    /// <summary>
    /// 明细 BulkCopy 落库（禁止再对十万行走 Insertable 分批）
    /// </summary>
    /// <param name="ecCode">设变单号</param>
    /// <param name="ecdetails">明细实体列表</param>
    /// <returns>任务</returns>
    private async Task CreateEcDetailsInBatchesAsync(string ecCode, List<TaktEcDetail> ecdetails)
    {
        ArgumentNullException.ThrowIfNull(ecdetails);
        var total = ecdetails.Count;
        if (total == 0)
        {
            return;
        }

        TaktLogger.Information(
            "[EcGijutsuPersist] 明细开始 BulkCopy 落库 Total={Total} EcCode={EcCode}",
            total,
            ecCode);
        var sw = System.Diagnostics.Stopwatch.StartNew();
        await _ecDetailRepository.CreateRangeAsync(ecdetails);
        sw.Stop();
        TaktLogger.Information(
            "[EcGijutsuPersist] 明细落库完成 Total={Total} ElapsedMs={ElapsedMs} EcCode={EcCode}",
            total,
            sw.ElapsedMilliseconds,
            ecCode);
    }

    /// <summary>
    /// 删除该设变下全部部门执行行。
    /// 有 EcCode 时按单号整批清理（禁止对十万明细逐行 Cascade）。
    /// </summary>
    /// <param name="entity">设变技术课主</param>
    /// <returns>任务</returns>
    private async Task DeleteDeptExecRowsForEcAsync(TaktEcGijutsu entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        if (!string.IsNullOrWhiteSpace(entity.EcCode))
        {
            TaktLogger.Information(
                "[EcGijutsuPersist] 按设变号清理部门执行行 EcCode={EcCode}",
                entity.EcCode);
            await _ecExecPersistence.DeleteAllByEcCodeAsync(entity.EcCode);
            return;
        }

        var details = await _ecDetailRepository.GetListAsync(x => x.EcGijutsuId == entity.Id);
        foreach (var detail in details)
        {
            await _ecExecPersistence.DeleteByDetailIdCascadeAsync(detail.Id);
        }
    }

    /// <summary>
    /// 按 EcScope 生成部门执行行；可选创建通知
    /// </summary>
    /// <param name="entity">设变主表</param>
    /// <param name="createNotificationIfMissing">无通知时是否创建</param>
    /// <param name="notificationDate">通知日期</param>
    /// <returns>派生结果（含各部门条数）</returns>
    private async Task<TaktEcScopeExecApplyResult> ApplyScopeExecAndMaybeNotifyAsync(
        TaktEcGijutsu entity,
        bool createNotificationIfMissing,
        DateTime? notificationDate = null)
    {
        TaktLogger.Information(
            "[EcGijutsuPersist] 加载明细用于部门派生 EcCode={EcCode}",
            entity.EcCode);
        var details = await _ecDetailRepository.GetListAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.EcGijutsuId == entity.Id
            && x.IsObsolete == 0);
        TaktLogger.Information(
            "[EcGijutsuPersist] 已加载明细 DetailCount={DetailCount} EcCode={EcCode}",
            details.Count,
            entity.EcCode);
        if (details.Count == 0)
        {
            TaktLogger.Information(
                "[EcGijutsuPersist] 无明细，跳过部门执行派生 EcCode={EcCode}",
                entity.EcCode);
            return TaktEcScopeExecApplyResult.Empty;
        }
        var applyResult = await _ecScopeExecOrchestrator.ApplyAsync(entity, details);
        if (createNotificationIfMissing && applyResult.DeptCodes.Count > 0)
        {
            var existingNotification = await _ecNotificationRepository.FirstAsync(x =>
                x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.EcGijutsuId == entity.Id
                && x.IsDeleted == 0);
            if (existingNotification == null)
            {
                var notifyDate = (notificationDate ?? entity.EcEntryDate).Date;
                if (notifyDate == default)
                {
                    notifyDate = DateTime.Today;
                }
                var deptCodes = applyResult.DeptCodes;
                var deptNames = await ResolveDeptDisplayNamesAsync(deptCodes);
                const int notificationMethod = 2;
                var notificationNo = await NextEcNotificationCodeAsync(notificationMethod);
                var notification = new TaktEcNotification
                {
                    TenantCode = CurrentTenantCode,
                    CompanyCode = CurrentCompanyCode,
                    PlantCode = entity.PlantCode,
                    EcNotificationCode = notificationNo,
                    EcGijutsuId = entity.Id,
                    EcCode = entity.EcCode,
                    EcTitle = entity.EcTitle,
                    EcNotificationDate = notifyDate,
                    EcNotificationDeptCodes = string.Join(",", deptCodes),
                    EcNotificationDeptNames = string.Join(",", deptNames),
                    EcNotificationMethod = notificationMethod,
                    EcNotificationStatus = 0,
                };
                var isUnique = await _uniqueValidator.IsUniqueAsync(
                    _ecNotificationRepository,
                    x => x.CompanyCode == notification.CompanyCode
                        && x.EcNotificationCode == notification.EcNotificationCode);
                if (!isUnique)
                {
                    throw new TaktBusinessException($"工程变更通知单号 {notificationNo} 已存在");
                }
                await _ecNotificationRepository.CreateAsync(notification);
                TaktLogger.Information(
                    "[EcGijutsuPersist] 已创建设变通知 EcCode={EcCode} NotificationCode={NotificationCode} Depts={Depts}",
                    entity.EcCode,
                    notificationNo,
                    applyResult.FormatSummary());
            }
        }
        await _handoffMailService.TryDispatchAsync(entity);
        await _ecGijutsuStatusSynchronizer.RefreshByEcCodeAsync(entity.EcCode);
        TaktLogger.Information(
            "[EcGijutsuPersist] 部门执行派生结束，已按执行部门输入回写 EcStatus EcCode={EcCode}",
            entity.EcCode);
        return applyResult;
    }

    /// <summary>
    /// 按通知方式从编码规则取下一通知单号
    /// </summary>
    /// <param name="notificationMethod">通知方式（1=系统通知 2=邮件 3=纸质 4=会议）</param>
    /// <returns>通知单号</returns>
    private async Task<string> NextEcNotificationCodeAsync(int notificationMethod)
    {
        var ruleCode = TaktEcFlowConstants.ResolveNotificationRuleCode(notificationMethod);
        var generated = await _numberingGenerator.GenerateNextAsync(ruleCode);
        if (string.IsNullOrWhiteSpace(generated.BusinessCode))
        {
            throw new TaktBusinessException("工程变更通知单号生成失败");
        }
        return generated.BusinessCode.Trim();
    }

    /// <summary>
    /// 按部门编码顺序解析部门显示名称（缺失时回退为编码本身）
    /// </summary>
    /// <param name="deptCodes">部门编码列表</param>
    /// <returns>与编码顺序一致的部门名称列表</returns>
    private async Task<IReadOnlyList<string>> ResolveDeptDisplayNamesAsync(IReadOnlyList<string> deptCodes)
    {
        if (deptCodes == null || deptCodes.Count == 0)
        {
            return Array.Empty<string>();
        }
        var codeList = deptCodes
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim())
            .Distinct(StringComparer.Ordinal)
            .ToList();
        var depts = await _deptRepository.GetListAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && codeList.Contains(x.DeptCode));
        var nameByCode = depts.ToDictionary(x => x.DeptCode, x => x.DeptName1, StringComparer.Ordinal);
        var names = new List<string>(deptCodes.Count);
        foreach (var code in deptCodes)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                continue;
            }
            var trimmed = code.Trim();
            names.Add(nameByCode.TryGetValue(trimmed, out var name) && !string.IsNullOrWhiteSpace(name)
                ? name
                : trimmed);
        }
        return names;
    }

    /// <summary>
    /// 按目标工厂与物料编码批量加载工厂物料
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="materialCodes">物料编码集合</param>
    /// <returns>物料编码 → 工厂物料</returns>
    private async Task<Dictionary<string, TaktMaterialPlant>> LoadMaterialPlantsByCodesAsync(
        string plantCode,
        IReadOnlySet<string> materialCodes)
    {
        if (materialCodes.Count == 0)
        {
            return new Dictionary<string, TaktMaterialPlant>(StringComparer.OrdinalIgnoreCase);
        }
        var codeList = materialCodes.ToList();
        var materials = await _materialPlantRepository.GetListAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plantCode
            && codeList.Contains(x.MaterialCode));
        return materials.ToDictionary(x => x.MaterialCode, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 按根物料编码批量加载机种编码（TaktModelDestination；同物料取 SortOrder 最小）
    /// </summary>
    /// <param name="materialCodes">根物料编码集合</param>
    /// <returns>物料编码 → 机种编码</returns>
    private async Task<Dictionary<string, string>> LoadModelCodesByMaterialCodesAsync(
        IReadOnlySet<string> materialCodes)
    {
        if (materialCodes.Count == 0)
        {
            return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }
        var codeList = materialCodes.ToList();
        var destinations = await _modelDestinationRepository.GetListAsync(x =>
            x.TenantCode == CurrentTenantCode
            && codeList.Contains(x.MaterialCode));
        return TaktEcDetailMaterialPlantMapper.BuildModelCodeByMaterialLookup(destinations);
    }

    /// <summary>
    /// 构建未导入来源设变查询表达式
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <param name="importedEcCodes">目标工厂已导入的设变单号集合</param>
    /// <returns>查询表达式</returns>
    private Expression<Func<TaktSourceEc, bool>> UnimportedSourceEcQueryExpression(
        TaktEcGijutsuSourceEcInputQueryDto queryDto,
        IReadOnlySet<string> importedEcCodes)
    {
        var exp = Expressionable.Create<TaktSourceEc>()
            .And(x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode);
        if (importedEcCodes.Count > 0)
        {
            exp = exp.And(x => !importedEcCodes.Contains(x.SourceEcCode));
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceEcCode))
        {
            var sourceEcCode = queryDto.SourceEcCode.Trim();
            exp = exp.And(x => x.SourceEcCode.Contains(sourceEcCode));
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceTitle))
        {
            var sourceTitle = queryDto.SourceTitle.Trim();
            exp = exp.And(x => x.SourceTitle.Contains(sourceTitle));
        }
        if (!string.IsNullOrWhiteSpace(queryDto.KeyWords))
        {
            var keywords = queryDto.KeyWords.Trim();
            exp = exp.And(x =>
                x.SourceEcCode.Contains(keywords)
                || x.SourceModel.Contains(keywords)
                || x.SourceTitle.Contains(keywords)
                || (x.SourceTcjOwner != null && x.SourceTcjOwner.Contains(keywords))
                || (x.SourceStatus != null && x.SourceStatus.Contains(keywords)));
        }
        return exp.ToExpression();
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 将主表实体映射为表头 DTO（忽略明细/附件/通知导航，避免主表↔明细环导致 Adapt/JSON 失败）。
    /// </summary>
    /// <param name="data">主表实体列表</param>
    /// <returns>仅表头的 DTO 列表</returns>
    internal static List<TaktEcGijutsuDto> MapHeaderList(IReadOnlyList<TaktEcGijutsu> data)
    {
        if (data == null || data.Count == 0)
        {
            return new List<TaktEcGijutsuDto>();
        }

        var config = new TypeAdapterConfig();
        config.NewConfig<TaktEcGijutsu, TaktEcGijutsuDto>()
            .Ignore(d => d.EcDetails)
            .Ignore(d => d.Attachments)
            .Ignore(d => d.Notifications);
        return data.Adapt<List<TaktEcGijutsuDto>>(config);
    }

    /// <summary>
    /// 查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    internal static Expression<Func<TaktEcGijutsu, bool>> QueryExpression(TaktEcGijutsuQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEcGijutsu>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.EcCode != null && x.EcCode.Contains(keywords))
                || SqlFunc.ToString(x.ChangeStatus).Contains(keywords)
                || (x.EcTitle != null && x.EcTitle.Contains(keywords))
                || (x.EcContent != null && x.EcContent.Contains(keywords))
                || (x.EcLeader != null && x.EcLeader.Contains(keywords))
                || SqlFunc.ToString(x.EcLossAmount).Contains(keywords)
                || SqlFunc.ToString(x.EcScope).Contains(keywords)
                || SqlFunc.ToString(x.EcStatus).Contains(keywords)
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.EcIssueDate).Contains(keywords)
                || SqlFunc.ToString(x.EcEntryDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.EcCode))
        {
            exp = exp.And(x => x.EcCode != null && x.EcCode.Contains(queryDto.EcCode));
        }

        if (queryDto?.ChangeStatus.HasValue == true)
        {
            exp = exp.And(x => x.ChangeStatus == queryDto.ChangeStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.EcTitle))
        {
            exp = exp.And(x => x.EcTitle != null && x.EcTitle.Contains(queryDto.EcTitle));
        }

        if (!string.IsNullOrEmpty(queryDto?.EcContent))
        {
            exp = exp.And(x => x.EcContent != null && x.EcContent.Contains(queryDto.EcContent));
        }

        if (!string.IsNullOrEmpty(queryDto?.EcLeader))
        {
            exp = exp.And(x => x.EcLeader != null && x.EcLeader.Contains(queryDto.EcLeader));
        }

        if (queryDto?.EcLossAmount.HasValue == true)
        {
            exp = exp.And(x => x.EcLossAmount == queryDto.EcLossAmount);
        }

        if (queryDto?.EcScope.HasValue == true)
        {
            exp = exp.And(x => x.EcScope == queryDto.EcScope);
        }

        if (queryDto?.EcDeptScope.HasValue == true)
        {
            exp = exp.And(x => x.EcDeptScope == queryDto.EcDeptScope);
        }

        if (queryDto?.EcStatus.HasValue == true)
        {
            exp = exp.And(x => x.EcStatus == queryDto.EcStatus);
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

        if (queryDto?.EcIssueDateStart.HasValue == true)
        {
            exp = exp.And(x => x.EcIssueDate >= queryDto.EcIssueDateStart);
        }

        if (queryDto?.EcIssueDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.EcIssueDate <= queryDto.EcIssueDateEnd);
        }

        if (queryDto?.EcEntryDateStart.HasValue == true)
        {
            exp = exp.And(x => x.EcEntryDate >= queryDto.EcEntryDateStart);
        }

        if (queryDto?.EcEntryDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.EcEntryDate <= queryDto.EcEntryDateEnd);
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt >= queryDto.CreatedAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt <= queryDto.CreatedAtEnd);
        }

        AppendExecDeptExists(exp, queryDto?.ExecDeptCode);

        return exp.ToExpression();
    }

    /// <summary>
    /// 按执行部门反查：主表设变号在该部门执行表中存在未作废行才进入清单。
    /// </summary>
    /// <param name="exp">主表查询</param>
    /// <param name="execDeptCode">执行部门编码</param>
    private static void AppendExecDeptExists(Expressionable<TaktEcGijutsu> exp, string? execDeptCode)
    {
        var deptCode = execDeptCode?.Trim();
        if (string.IsNullOrEmpty(deptCode))
        {
            return;
        }
        switch (deptCode)
        {
            case TaktEcDeptCodes.Pmc:
                exp.And(x => SqlFunc.Subqueryable<TaktEcSeikan>().Where(s =>
                    s.TenantCode == x.TenantCode
                    && s.CompanyCode == x.CompanyCode
                    && s.PlantCode == x.PlantCode
                    && s.EcCode == x.EcCode
                    && s.IsDeleted == 0
                    && s.IsObsolete == 0).Any());
                break;
            case TaktEcDeptCodes.Mp:
                exp.And(x => SqlFunc.Subqueryable<TaktEcKoubai>().Where(s =>
                    s.TenantCode == x.TenantCode
                    && s.CompanyCode == x.CompanyCode
                    && s.PlantCode == x.PlantCode
                    && s.EcCode == x.EcCode
                    && s.IsDeleted == 0
                    && s.IsObsolete == 0).Any());
                break;
            case TaktEcDeptCodes.Iqc:
                exp.And(x => SqlFunc.Subqueryable<TaktEcUkeken>().Where(s =>
                    s.TenantCode == x.TenantCode
                    && s.CompanyCode == x.CompanyCode
                    && s.PlantCode == x.PlantCode
                    && s.EcCode == x.EcCode
                    && s.IsDeleted == 0
                    && s.IsObsolete == 0).Any());
                break;
            case TaktEcDeptCodes.Mc:
                exp.And(x => SqlFunc.Subqueryable<TaktEcBukan>().Where(s =>
                    s.TenantCode == x.TenantCode
                    && s.CompanyCode == x.CompanyCode
                    && s.PlantCode == x.PlantCode
                    && s.EcCode == x.EcCode
                    && s.IsDeleted == 0
                    && s.IsObsolete == 0).Any());
                break;
            case TaktEcDeptCodes.Smt:
                exp.And(x => SqlFunc.Subqueryable<TaktEcEmtl>().Where(s =>
                    s.TenantCode == x.TenantCode
                    && s.CompanyCode == x.CompanyCode
                    && s.PlantCode == x.PlantCode
                    && s.EcCode == x.EcCode
                    && s.IsDeleted == 0
                    && s.IsObsolete == 0).Any());
                break;
            case TaktEcDeptCodes.Pcba:
                exp.And(x => SqlFunc.Subqueryable<TaktEcSeizounika>().Where(s =>
                    s.TenantCode == x.TenantCode
                    && s.CompanyCode == x.CompanyCode
                    && s.PlantCode == x.PlantCode
                    && s.EcCode == x.EcCode
                    && s.IsDeleted == 0
                    && s.IsObsolete == 0).Any());
                break;
            case TaktEcDeptCodes.Assy:
                exp.And(x => SqlFunc.Subqueryable<TaktEcSeizouikka>().Where(s =>
                    s.TenantCode == x.TenantCode
                    && s.CompanyCode == x.CompanyCode
                    && s.PlantCode == x.PlantCode
                    && s.EcCode == x.EcCode
                    && s.IsDeleted == 0
                    && s.IsObsolete == 0).Any());
                break;
            case TaktEcDeptCodes.Qa:
                exp.And(x => SqlFunc.Subqueryable<TaktEcHinkan>().Where(s =>
                    s.TenantCode == x.TenantCode
                    && s.CompanyCode == x.CompanyCode
                    && s.PlantCode == x.PlantCode
                    && s.EcCode == x.EcCode
                    && s.IsDeleted == 0
                    && s.IsObsolete == 0).Any());
                break;
            case TaktEcDeptCodes.Te:
                exp.And(x => SqlFunc.Subqueryable<TaktEcSeizougijutsu>().Where(s =>
                    s.TenantCode == x.TenantCode
                    && s.CompanyCode == x.CompanyCode
                    && s.PlantCode == x.PlantCode
                    && s.EcCode == x.EcCode
                    && s.IsDeleted == 0
                    && s.IsObsolete == 0).Any());
                break;
        }
    }
}
