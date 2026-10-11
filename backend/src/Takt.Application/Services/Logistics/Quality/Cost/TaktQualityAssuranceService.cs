// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Quality.Cost
// 文件名称：TaktQualityAssuranceService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：品质业务主应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Quality.Cost;
using Takt.Domain.Entities.Logistics.Quality.Cost;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Quality.Cost;

/// <summary>
/// 品质业务主应用服务
/// </summary>
public class TaktQualityAssuranceService : TaktServiceBase, ITaktQualityAssuranceService
{
    private readonly ITaktCompanyRepository<TaktQualityAssurance> _qualityAssuranceRepository;
    private readonly ITaktCompanyRepository<TaktQualityAssuranceIncoming> _qualityAssuranceIncomingRepository;
    private readonly ITaktCompanyRepository<TaktQualityAssuranceFirstArticle> _qualityAssuranceFirstArticleRepository;
    private readonly ITaktCompanyRepository<TaktQualityAssuranceCalibration> _qualityAssuranceCalibrationRepository;
    private readonly ITaktCompanyRepository<TaktQualityAssuranceOther> _qualityAssuranceOtherRepository;
    private readonly ITaktCompanyRepository<TaktQualityAssuranceOutgoing> _qualityAssuranceOutgoingRepository;
    private readonly ITaktCompanyRepository<TaktQualityAssuranceReliability> _qualityAssuranceReliabilityRepository;
    private readonly ITaktCompanyRepository<TaktQualityAssuranceCustomerResponse> _qualityAssuranceCustomerResponseRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="qualityAssuranceRepository">品质业务主仓储</param>
    /// <param name="qualityAssuranceIncomingRepository">QualityAssuranceIncoming仓储</param>
    /// <param name="qualityAssuranceFirstArticleRepository">QualityAssuranceFirstArticle仓储</param>
    /// <param name="qualityAssuranceCalibrationRepository">QualityAssuranceCalibration仓储</param>
    /// <param name="qualityAssuranceOtherRepository">QualityAssuranceOther仓储</param>
    /// <param name="qualityAssuranceOutgoingRepository">QualityAssuranceOutgoing仓储</param>
    /// <param name="qualityAssuranceReliabilityRepository">QualityAssuranceReliability仓储</param>
    /// <param name="qualityAssuranceCustomerResponseRepository">QualityAssuranceCustomerResponse仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktQualityAssuranceService(
        ITaktCompanyRepository<TaktQualityAssurance> qualityAssuranceRepository,
        ITaktCompanyRepository<TaktQualityAssuranceIncoming> qualityAssuranceIncomingRepository,
        ITaktCompanyRepository<TaktQualityAssuranceFirstArticle> qualityAssuranceFirstArticleRepository,
        ITaktCompanyRepository<TaktQualityAssuranceCalibration> qualityAssuranceCalibrationRepository,
        ITaktCompanyRepository<TaktQualityAssuranceOther> qualityAssuranceOtherRepository,
        ITaktCompanyRepository<TaktQualityAssuranceOutgoing> qualityAssuranceOutgoingRepository,
        ITaktCompanyRepository<TaktQualityAssuranceReliability> qualityAssuranceReliabilityRepository,
        ITaktCompanyRepository<TaktQualityAssuranceCustomerResponse> qualityAssuranceCustomerResponseRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _qualityAssuranceRepository = qualityAssuranceRepository;
        _qualityAssuranceIncomingRepository = qualityAssuranceIncomingRepository;
        _qualityAssuranceFirstArticleRepository = qualityAssuranceFirstArticleRepository;
        _qualityAssuranceCalibrationRepository = qualityAssuranceCalibrationRepository;
        _qualityAssuranceOtherRepository = qualityAssuranceOtherRepository;
        _qualityAssuranceOutgoingRepository = qualityAssuranceOutgoingRepository;
        _qualityAssuranceReliabilityRepository = qualityAssuranceReliabilityRepository;
        _qualityAssuranceCustomerResponseRepository = qualityAssuranceCustomerResponseRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取品质业务主列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktQualityAssuranceDto>> GetQualityAssuranceListAsync(TaktQualityAssuranceQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktQualityAssuranceDto>.Create(
                new List<TaktQualityAssuranceDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _qualityAssuranceRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktQualityAssuranceDto>.Create(
            data.Adapt<List<TaktQualityAssuranceDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取品质业务主
    /// </summary>
    /// <param name="id">品质业务主ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityAssuranceDto?> GetQualityAssuranceByIdAsync(long id)
    {
        var entity = await _qualityAssuranceRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktQualityAssuranceDto>();
        await FillQualityAssuranceDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取品质业务主选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetQualityAssuranceOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _qualityAssuranceRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.QualityAssuranceCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.QualityAssuranceCode,
            DictLabel = e.QualityAssuranceCode,
        }).ToList();
    }

    /// <summary>
    /// 创建品质业务主
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityAssuranceDto> CreateQualityAssuranceAsync(TaktQualityAssuranceCreateDto dto)
    {
        var entity = dto.Adapt<TaktQualityAssurance>();
        var isUnique_ix_takt_logistics_quality_assurance_qo_unique = await _uniqueValidator.IsUniqueAsync(
            _qualityAssuranceRepository,
            x => x.PlantCode == entity.PlantCode
                && x.QualityAssuranceCode == entity.QualityAssuranceCode
                && x.AssuranceMonth == entity.AssuranceMonth
                && x.DebitNoteCode == entity.DebitNoteCode);
        if (!isUnique_ix_takt_logistics_quality_assurance_qo_unique)
        {
            throw new TaktBusinessException("品质业务主的PlantCode、QualityAssuranceCode、AssuranceMonth、DebitNoteCode已存在");
        }
        entity = await _qualityAssuranceRepository.CreateAsync(entity);
                await SaveQualityAssuranceChildrenAsync(entity, dto);
        return await GetQualityAssuranceByIdAsync(entity.Id) ?? entity.Adapt<TaktQualityAssuranceDto>();
    }

    /// <summary>
    /// 更新品质业务主
    /// </summary>
    /// <param name="id">品质业务主ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityAssuranceDto> UpdateQualityAssuranceAsync(long id, TaktQualityAssuranceUpdateDto dto)
    {
        var entity = await _qualityAssuranceRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("品质业务主不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_quality_assurance_qo_unique = await _uniqueValidator.IsUniqueAsync(
            _qualityAssuranceRepository,
            x => x.PlantCode == entity.PlantCode
                && x.QualityAssuranceCode == entity.QualityAssuranceCode
                && x.AssuranceMonth == entity.AssuranceMonth
                && x.DebitNoteCode == entity.DebitNoteCode,
            id);
        if (!isUnique_ix_takt_logistics_quality_assurance_qo_unique)
        {
            throw new TaktBusinessException("品质业务主的PlantCode、QualityAssuranceCode、AssuranceMonth、DebitNoteCode已存在");
        }
        await _qualityAssuranceRepository.UpdateAsync(entity);
                await SaveQualityAssuranceChildrenAsync(entity, dto);
        return await GetQualityAssuranceByIdAsync(id) ?? throw new TaktBusinessException("品质业务主不存在");
    }

    /// <summary>
    /// 删除品质业务主
    /// </summary>
    /// <param name="id">品质业务主ID</param>
    /// <returns>任务</returns>
    public async Task DeleteQualityAssuranceByIdAsync(long id)
    {
        var entity = await _qualityAssuranceRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("品质业务主不存在或已删除");
        }
        await _qualityAssuranceIncomingRepository.DeleteAsync(x => x.QualityAssuranceId == entity.Id);
        await _qualityAssuranceFirstArticleRepository.DeleteAsync(x => x.QualityAssuranceId == entity.Id);
        await _qualityAssuranceCalibrationRepository.DeleteAsync(x => x.QualityAssuranceId == entity.Id);
        await _qualityAssuranceOtherRepository.DeleteAsync(x => x.QualityAssuranceId == entity.Id);
        await _qualityAssuranceOutgoingRepository.DeleteAsync(x => x.QualityAssuranceId == entity.Id);
        await _qualityAssuranceReliabilityRepository.DeleteAsync(x => x.QualityAssuranceId == entity.Id);
        await _qualityAssuranceCustomerResponseRepository.DeleteAsync(x => x.QualityAssuranceId == entity.Id);
        var deleted = await _qualityAssuranceRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("品质业务主不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除品质业务主
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteQualityAssuranceBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteQualityAssuranceByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetQualityAssuranceTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktQualityAssuranceTemplateDto>(
            sheetName ?? "品质业务主导入模板",
            fileName ?? "品质业务主导入模板.xlsx");
    }

    /// <summary>
    /// 导入品质业务主
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportQualityAssuranceAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktQualityAssuranceImportDto>(fileStream, sheetName ?? "品质业务主导入模板");
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
                var entity = rows[i].Adapt<TaktQualityAssurance>();
                var importKey = $"{entity.PlantCode}|{entity.QualityAssuranceCode}|{entity.AssuranceMonth}|{entity.DebitNoteCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、QualityAssuranceCode、AssuranceMonth、DebitNoteCode）");
                }
                var isUnique_ix_takt_logistics_quality_assurance_qo_unique = await _uniqueValidator.IsUniqueAsync(
                    _qualityAssuranceRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.QualityAssuranceCode == entity.QualityAssuranceCode
                        && x.AssuranceMonth == entity.AssuranceMonth
                        && x.DebitNoteCode == entity.DebitNoteCode);
                if (!isUnique_ix_takt_logistics_quality_assurance_qo_unique)
                {
                    throw new TaktBusinessException("品质业务主的PlantCode、QualityAssuranceCode、AssuranceMonth、DebitNoteCode已存在");
                }
                await _qualityAssuranceRepository.CreateAsync(entity);
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
    /// 导出品质业务主
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportQualityAssuranceAsync(TaktQualityAssuranceQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktQualityAssuranceQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktQualityAssuranceExportDto>(),
                sheetName ?? "品质业务主数据",
                fileName ?? "品质业务主导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _qualityAssuranceRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktQualityAssuranceExportDto>(),
                sheetName ?? "品质业务主数据",
                fileName ?? "品质业务主导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktQualityAssuranceExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "品质业务主数据",
            fileName ?? "品质业务主导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废品质业务来料检验费用明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="qualityAssuranceId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkQualityAssuranceIncomingsObsoleteAsync(long qualityAssuranceId)
    {
        if (qualityAssuranceId <= 0)
        {
            return;
        }
        var rows = await _qualityAssuranceIncomingRepository.GetListAsync(
            x => x.QualityAssuranceId == qualityAssuranceId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _qualityAssuranceIncomingRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 将指定主表下全部未作废品质业务初期定期检定费用明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="qualityAssuranceId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkQualityAssuranceFirstArticlesObsoleteAsync(long qualityAssuranceId)
    {
        if (qualityAssuranceId <= 0)
        {
            return;
        }
        var rows = await _qualityAssuranceFirstArticleRepository.GetListAsync(
            x => x.QualityAssuranceId == qualityAssuranceId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _qualityAssuranceFirstArticleRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 将指定主表下全部未作废品质业务设备校正费用明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="qualityAssuranceId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkQualityAssuranceCalibrationsObsoleteAsync(long qualityAssuranceId)
    {
        if (qualityAssuranceId <= 0)
        {
            return;
        }
        var rows = await _qualityAssuranceCalibrationRepository.GetListAsync(
            x => x.QualityAssuranceId == qualityAssuranceId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _qualityAssuranceCalibrationRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 将指定主表下全部未作废品质业务其他通常业务费用明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="qualityAssuranceId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkQualityAssuranceOthersObsoleteAsync(long qualityAssuranceId)
    {
        if (qualityAssuranceId <= 0)
        {
            return;
        }
        var rows = await _qualityAssuranceOtherRepository.GetListAsync(
            x => x.QualityAssuranceId == qualityAssuranceId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _qualityAssuranceOtherRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 将指定主表下全部未作废品质业务出货检验费用明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="qualityAssuranceId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkQualityAssuranceOutgoingsObsoleteAsync(long qualityAssuranceId)
    {
        if (qualityAssuranceId <= 0)
        {
            return;
        }
        var rows = await _qualityAssuranceOutgoingRepository.GetListAsync(
            x => x.QualityAssuranceId == qualityAssuranceId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _qualityAssuranceOutgoingRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 将指定主表下全部未作废品质业务信赖性评价ORT费用明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="qualityAssuranceId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkQualityAssuranceReliabilitysObsoleteAsync(long qualityAssuranceId)
    {
        if (qualityAssuranceId <= 0)
        {
            return;
        }
        var rows = await _qualityAssuranceReliabilityRepository.GetListAsync(
            x => x.QualityAssuranceId == qualityAssuranceId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _qualityAssuranceReliabilityRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 将指定主表下全部未作废品质业务顾客品质要求对应费用明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="qualityAssuranceId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkQualityAssuranceCustomerResponsesObsoleteAsync(long qualityAssuranceId)
    {
        if (qualityAssuranceId <= 0)
        {
            return;
        }
        var rows = await _qualityAssuranceCustomerResponseRepository.GetListAsync(
            x => x.QualityAssuranceId == qualityAssuranceId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _qualityAssuranceCustomerResponseRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充品质业务主详情（加载 OneToMany 子表：品质业务来料检验费用明细、品质业务初期定期检定费用明细、品质业务设备校正费用明细、品质业务其他通常业务费用明细、品质业务出货检验费用明细、品质业务信赖性评价ORT费用明细、品质业务顾客品质要求对应费用明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillQualityAssuranceDetailsAsync(TaktQualityAssuranceDto dto, TaktQualityAssurance entity)
    {
        if (dto == null)
        {
            return;
        }
        // 品质业务来料检验费用明细 → dto.IncomingItems（含作废行）
        var incomingitems = await _qualityAssuranceIncomingRepository.GetListAsync(x => x.QualityAssuranceId == entity.Id);
        dto.IncomingItems = incomingitems.Adapt<List<TaktQualityAssuranceIncomingDto>>();
        // 品质业务初期定期检定费用明细 → dto.FirstArticleItems（含作废行）
        var firstarticleitems = await _qualityAssuranceFirstArticleRepository.GetListAsync(x => x.QualityAssuranceId == entity.Id);
        dto.FirstArticleItems = firstarticleitems.Adapt<List<TaktQualityAssuranceFirstArticleDto>>();
        // 品质业务设备校正费用明细 → dto.CalibrationItems（含作废行）
        var calibrationitems = await _qualityAssuranceCalibrationRepository.GetListAsync(x => x.QualityAssuranceId == entity.Id);
        dto.CalibrationItems = calibrationitems.Adapt<List<TaktQualityAssuranceCalibrationDto>>();
        // 品质业务其他通常业务费用明细 → dto.OtherItems（含作废行）
        var otheritems = await _qualityAssuranceOtherRepository.GetListAsync(x => x.QualityAssuranceId == entity.Id);
        dto.OtherItems = otheritems.Adapt<List<TaktQualityAssuranceOtherDto>>();
        // 品质业务出货检验费用明细 → dto.OutgoingItems（含作废行）
        var outgoingitems = await _qualityAssuranceOutgoingRepository.GetListAsync(x => x.QualityAssuranceId == entity.Id);
        dto.OutgoingItems = outgoingitems.Adapt<List<TaktQualityAssuranceOutgoingDto>>();
        // 品质业务信赖性评价ORT费用明细 → dto.ReliabilityItems（含作废行）
        var reliabilityitems = await _qualityAssuranceReliabilityRepository.GetListAsync(x => x.QualityAssuranceId == entity.Id);
        dto.ReliabilityItems = reliabilityitems.Adapt<List<TaktQualityAssuranceReliabilityDto>>();
        // 品质业务顾客品质要求对应费用明细 → dto.CustomerResponseItems（含作废行）
        var customerresponseitems = await _qualityAssuranceCustomerResponseRepository.GetListAsync(x => x.QualityAssuranceId == entity.Id);
        dto.CustomerResponseItems = customerresponseitems.Adapt<List<TaktQualityAssuranceCustomerResponseDto>>();
    }

    /// <summary>
    /// 保存品质业务主子表级联（品质业务来料检验费用明细、品质业务初期定期检定费用明细、品质业务设备校正费用明细、品质业务其他通常业务费用明细、品质业务出货检验费用明细、品质业务信赖性评价ORT费用明细、品质业务顾客品质要求对应费用明细；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveQualityAssuranceChildrenAsync(TaktQualityAssurance entity, TaktQualityAssuranceCreateDto dto)
    {
        // 品质业务来料检验费用明细（IncomingItems）
        List<TaktQualityAssuranceIncomingUpdateDto>? incomingItemsForSave;
        if (dto is TaktQualityAssuranceUpdateDto updateDtoForIncomingItems && updateDtoForIncomingItems.IncomingItems != null)
        {
            incomingItemsForSave = updateDtoForIncomingItems.IncomingItems;
        }
        else if (dto.IncomingItems != null)
        {
            incomingItemsForSave = dto.IncomingItems.Adapt<List<TaktQualityAssuranceIncomingUpdateDto>>();
        }
        else
        {
            incomingItemsForSave = null;
        }
        if (incomingItemsForSave is not { Count: > 0 })
        {
            await MarkQualityAssuranceIncomingsObsoleteAsync(entity.Id);
        }
        else
        {
            var existingList = await _qualityAssuranceIncomingRepository.GetListAsync(x => x.QualityAssuranceId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktQualityAssuranceIncoming>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < incomingItemsForSave.Count; i++)
            {
                var childDto = incomingItemsForSave[i];
                childDto.QualityAssuranceId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.QualityAssuranceCode = entity.QualityAssuranceCode;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("品质业务来料检验费用明细第{i + 1}项与本次提交的其他项重复（CompanyCode、QualityAssuranceId、LineNumber）");
                }
                if (childDto.QualityAssuranceIncomingId > 0)
                {
                    if (!existingById.TryGetValue(childDto.QualityAssuranceIncomingId, out var target))
                    {
                        throw new TaktBusinessException("品质业务来料检验费用明细不存在（QualityAssuranceIncomingId={childDto.QualityAssuranceIncomingId}）");
                    }
                    if (target.QualityAssuranceId != entity.Id)
                    {
                        throw new TaktBusinessException("品质业务来料检验费用明细不属于当前主表（QualityAssuranceIncomingId={childDto.QualityAssuranceIncomingId}）");
                    }
                    submittedIds.Add(childDto.QualityAssuranceIncomingId);
                    var isUniqueUpdate_ix_takt_logistics_quality_assurance_incoming_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _qualityAssuranceIncomingRepository,
                        x => x.QualityAssuranceId == x.QualityAssuranceId
                && x.LineNumber == x.LineNumber,
                        childDto.QualityAssuranceIncomingId);
                    if (!isUniqueUpdate_ix_takt_logistics_quality_assurance_incoming_line_unique)
                    {
                        throw new TaktBusinessException("品质业务来料检验费用明细的QualityAssuranceId、LineNumber已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.QualityAssuranceIncomingId;
                    target.QualityAssuranceId = entity.Id;
                    target.IsObsolete = 0;
                    await _qualityAssuranceIncomingRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_quality_assurance_incoming_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _qualityAssuranceIncomingRepository,
                        x => x.QualityAssuranceId == x.QualityAssuranceId
                && x.LineNumber == x.LineNumber);
                    if (!isUniqueCreate_ix_takt_logistics_quality_assurance_incoming_line_unique)
                    {
                        throw new TaktBusinessException("品质业务来料检验费用明细的QualityAssuranceId、LineNumber已存在");
                    }
                    var child = childDto.Adapt<TaktQualityAssuranceIncoming>();
                    child.Id = 0;
                    child.QualityAssuranceId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _qualityAssuranceIncomingRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.QualityAssuranceCode) ? entity.QualityAssuranceCode : entity.Id.ToString();
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
                await _qualityAssuranceIncomingRepository.CreateRangeAsync(toCreate);
            }
        }
        // 品质业务初期定期检定费用明细（FirstArticleItems）
        List<TaktQualityAssuranceFirstArticleUpdateDto>? firstArticleItemsForSave;
        if (dto is TaktQualityAssuranceUpdateDto updateDtoForFirstArticleItems && updateDtoForFirstArticleItems.FirstArticleItems != null)
        {
            firstArticleItemsForSave = updateDtoForFirstArticleItems.FirstArticleItems;
        }
        else if (dto.FirstArticleItems != null)
        {
            firstArticleItemsForSave = dto.FirstArticleItems.Adapt<List<TaktQualityAssuranceFirstArticleUpdateDto>>();
        }
        else
        {
            firstArticleItemsForSave = null;
        }
        if (firstArticleItemsForSave is not { Count: > 0 })
        {
            await MarkQualityAssuranceFirstArticlesObsoleteAsync(entity.Id);
        }
        else
        {
            var existingList = await _qualityAssuranceFirstArticleRepository.GetListAsync(x => x.QualityAssuranceId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktQualityAssuranceFirstArticle>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < firstArticleItemsForSave.Count; i++)
            {
                var childDto = firstArticleItemsForSave[i];
                childDto.QualityAssuranceId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.QualityAssuranceCode = entity.QualityAssuranceCode;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("品质业务初期定期检定费用明细第{i + 1}项与本次提交的其他项重复（CompanyCode、QualityAssuranceId、LineNumber）");
                }
                if (childDto.QualityAssuranceFirstArticleId > 0)
                {
                    if (!existingById.TryGetValue(childDto.QualityAssuranceFirstArticleId, out var target))
                    {
                        throw new TaktBusinessException("品质业务初期定期检定费用明细不存在（QualityAssuranceFirstArticleId={childDto.QualityAssuranceFirstArticleId}）");
                    }
                    if (target.QualityAssuranceId != entity.Id)
                    {
                        throw new TaktBusinessException("品质业务初期定期检定费用明细不属于当前主表（QualityAssuranceFirstArticleId={childDto.QualityAssuranceFirstArticleId}）");
                    }
                    submittedIds.Add(childDto.QualityAssuranceFirstArticleId);
                    var isUniqueUpdate_ix_takt_logistics_quality_assurance_first_article_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _qualityAssuranceFirstArticleRepository,
                        x => x.QualityAssuranceId == x.QualityAssuranceId
                && x.LineNumber == x.LineNumber,
                        childDto.QualityAssuranceFirstArticleId);
                    if (!isUniqueUpdate_ix_takt_logistics_quality_assurance_first_article_line_unique)
                    {
                        throw new TaktBusinessException("品质业务初期定期检定费用明细的QualityAssuranceId、LineNumber已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.QualityAssuranceFirstArticleId;
                    target.QualityAssuranceId = entity.Id;
                    target.IsObsolete = 0;
                    await _qualityAssuranceFirstArticleRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_quality_assurance_first_article_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _qualityAssuranceFirstArticleRepository,
                        x => x.QualityAssuranceId == x.QualityAssuranceId
                && x.LineNumber == x.LineNumber);
                    if (!isUniqueCreate_ix_takt_logistics_quality_assurance_first_article_line_unique)
                    {
                        throw new TaktBusinessException("品质业务初期定期检定费用明细的QualityAssuranceId、LineNumber已存在");
                    }
                    var child = childDto.Adapt<TaktQualityAssuranceFirstArticle>();
                    child.Id = 0;
                    child.QualityAssuranceId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _qualityAssuranceFirstArticleRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.QualityAssuranceCode) ? entity.QualityAssuranceCode : entity.Id.ToString();
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
                await _qualityAssuranceFirstArticleRepository.CreateRangeAsync(toCreate);
            }
        }
        // 品质业务设备校正费用明细（CalibrationItems）
        List<TaktQualityAssuranceCalibrationUpdateDto>? calibrationItemsForSave;
        if (dto is TaktQualityAssuranceUpdateDto updateDtoForCalibrationItems && updateDtoForCalibrationItems.CalibrationItems != null)
        {
            calibrationItemsForSave = updateDtoForCalibrationItems.CalibrationItems;
        }
        else if (dto.CalibrationItems != null)
        {
            calibrationItemsForSave = dto.CalibrationItems.Adapt<List<TaktQualityAssuranceCalibrationUpdateDto>>();
        }
        else
        {
            calibrationItemsForSave = null;
        }
        if (calibrationItemsForSave is not { Count: > 0 })
        {
            await MarkQualityAssuranceCalibrationsObsoleteAsync(entity.Id);
        }
        else
        {
            var existingList = await _qualityAssuranceCalibrationRepository.GetListAsync(x => x.QualityAssuranceId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktQualityAssuranceCalibration>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < calibrationItemsForSave.Count; i++)
            {
                var childDto = calibrationItemsForSave[i];
                childDto.QualityAssuranceId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.QualityAssuranceCode = entity.QualityAssuranceCode;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("品质业务设备校正费用明细第{i + 1}项与本次提交的其他项重复（CompanyCode、QualityAssuranceId、LineNumber）");
                }
                if (childDto.QualityAssuranceCalibrationId > 0)
                {
                    if (!existingById.TryGetValue(childDto.QualityAssuranceCalibrationId, out var target))
                    {
                        throw new TaktBusinessException("品质业务设备校正费用明细不存在（QualityAssuranceCalibrationId={childDto.QualityAssuranceCalibrationId}）");
                    }
                    if (target.QualityAssuranceId != entity.Id)
                    {
                        throw new TaktBusinessException("品质业务设备校正费用明细不属于当前主表（QualityAssuranceCalibrationId={childDto.QualityAssuranceCalibrationId}）");
                    }
                    submittedIds.Add(childDto.QualityAssuranceCalibrationId);
                    var isUniqueUpdate_ix_takt_logistics_quality_assurance_calibration_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _qualityAssuranceCalibrationRepository,
                        x => x.QualityAssuranceId == x.QualityAssuranceId
                && x.LineNumber == x.LineNumber,
                        childDto.QualityAssuranceCalibrationId);
                    if (!isUniqueUpdate_ix_takt_logistics_quality_assurance_calibration_line_unique)
                    {
                        throw new TaktBusinessException("品质业务设备校正费用明细的QualityAssuranceId、LineNumber已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.QualityAssuranceCalibrationId;
                    target.QualityAssuranceId = entity.Id;
                    target.IsObsolete = 0;
                    await _qualityAssuranceCalibrationRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_quality_assurance_calibration_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _qualityAssuranceCalibrationRepository,
                        x => x.QualityAssuranceId == x.QualityAssuranceId
                && x.LineNumber == x.LineNumber);
                    if (!isUniqueCreate_ix_takt_logistics_quality_assurance_calibration_line_unique)
                    {
                        throw new TaktBusinessException("品质业务设备校正费用明细的QualityAssuranceId、LineNumber已存在");
                    }
                    var child = childDto.Adapt<TaktQualityAssuranceCalibration>();
                    child.Id = 0;
                    child.QualityAssuranceId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _qualityAssuranceCalibrationRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.QualityAssuranceCode) ? entity.QualityAssuranceCode : entity.Id.ToString();
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
                await _qualityAssuranceCalibrationRepository.CreateRangeAsync(toCreate);
            }
        }
        // 品质业务其他通常业务费用明细（OtherItems）
        List<TaktQualityAssuranceOtherUpdateDto>? otherItemsForSave;
        if (dto is TaktQualityAssuranceUpdateDto updateDtoForOtherItems && updateDtoForOtherItems.OtherItems != null)
        {
            otherItemsForSave = updateDtoForOtherItems.OtherItems;
        }
        else if (dto.OtherItems != null)
        {
            otherItemsForSave = dto.OtherItems.Adapt<List<TaktQualityAssuranceOtherUpdateDto>>();
        }
        else
        {
            otherItemsForSave = null;
        }
        if (otherItemsForSave is not { Count: > 0 })
        {
            await MarkQualityAssuranceOthersObsoleteAsync(entity.Id);
        }
        else
        {
            var existingList = await _qualityAssuranceOtherRepository.GetListAsync(x => x.QualityAssuranceId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktQualityAssuranceOther>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < otherItemsForSave.Count; i++)
            {
                var childDto = otherItemsForSave[i];
                childDto.QualityAssuranceId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.QualityAssuranceCode = entity.QualityAssuranceCode;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("品质业务其他通常业务费用明细第{i + 1}项与本次提交的其他项重复（CompanyCode、QualityAssuranceId、LineNumber）");
                }
                if (childDto.QualityAssuranceOtherId > 0)
                {
                    if (!existingById.TryGetValue(childDto.QualityAssuranceOtherId, out var target))
                    {
                        throw new TaktBusinessException("品质业务其他通常业务费用明细不存在（QualityAssuranceOtherId={childDto.QualityAssuranceOtherId}）");
                    }
                    if (target.QualityAssuranceId != entity.Id)
                    {
                        throw new TaktBusinessException("品质业务其他通常业务费用明细不属于当前主表（QualityAssuranceOtherId={childDto.QualityAssuranceOtherId}）");
                    }
                    submittedIds.Add(childDto.QualityAssuranceOtherId);
                    var isUniqueUpdate_ix_takt_logistics_quality_assurance_other_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _qualityAssuranceOtherRepository,
                        x => x.QualityAssuranceId == x.QualityAssuranceId
                && x.LineNumber == x.LineNumber,
                        childDto.QualityAssuranceOtherId);
                    if (!isUniqueUpdate_ix_takt_logistics_quality_assurance_other_line_unique)
                    {
                        throw new TaktBusinessException("品质业务其他通常业务费用明细的QualityAssuranceId、LineNumber已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.QualityAssuranceOtherId;
                    target.QualityAssuranceId = entity.Id;
                    target.IsObsolete = 0;
                    await _qualityAssuranceOtherRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_quality_assurance_other_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _qualityAssuranceOtherRepository,
                        x => x.QualityAssuranceId == x.QualityAssuranceId
                && x.LineNumber == x.LineNumber);
                    if (!isUniqueCreate_ix_takt_logistics_quality_assurance_other_line_unique)
                    {
                        throw new TaktBusinessException("品质业务其他通常业务费用明细的QualityAssuranceId、LineNumber已存在");
                    }
                    var child = childDto.Adapt<TaktQualityAssuranceOther>();
                    child.Id = 0;
                    child.QualityAssuranceId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _qualityAssuranceOtherRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.QualityAssuranceCode) ? entity.QualityAssuranceCode : entity.Id.ToString();
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
                await _qualityAssuranceOtherRepository.CreateRangeAsync(toCreate);
            }
        }
        // 品质业务出货检验费用明细（OutgoingItems）
        List<TaktQualityAssuranceOutgoingUpdateDto>? outgoingItemsForSave;
        if (dto is TaktQualityAssuranceUpdateDto updateDtoForOutgoingItems && updateDtoForOutgoingItems.OutgoingItems != null)
        {
            outgoingItemsForSave = updateDtoForOutgoingItems.OutgoingItems;
        }
        else if (dto.OutgoingItems != null)
        {
            outgoingItemsForSave = dto.OutgoingItems.Adapt<List<TaktQualityAssuranceOutgoingUpdateDto>>();
        }
        else
        {
            outgoingItemsForSave = null;
        }
        if (outgoingItemsForSave is not { Count: > 0 })
        {
            await MarkQualityAssuranceOutgoingsObsoleteAsync(entity.Id);
        }
        else
        {
            var existingList = await _qualityAssuranceOutgoingRepository.GetListAsync(x => x.QualityAssuranceId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktQualityAssuranceOutgoing>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < outgoingItemsForSave.Count; i++)
            {
                var childDto = outgoingItemsForSave[i];
                childDto.QualityAssuranceId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.QualityAssuranceCode = entity.QualityAssuranceCode;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("品质业务出货检验费用明细第{i + 1}项与本次提交的其他项重复（CompanyCode、QualityAssuranceId、LineNumber）");
                }
                if (childDto.QualityAssuranceOutgoingId > 0)
                {
                    if (!existingById.TryGetValue(childDto.QualityAssuranceOutgoingId, out var target))
                    {
                        throw new TaktBusinessException("品质业务出货检验费用明细不存在（QualityAssuranceOutgoingId={childDto.QualityAssuranceOutgoingId}）");
                    }
                    if (target.QualityAssuranceId != entity.Id)
                    {
                        throw new TaktBusinessException("品质业务出货检验费用明细不属于当前主表（QualityAssuranceOutgoingId={childDto.QualityAssuranceOutgoingId}）");
                    }
                    submittedIds.Add(childDto.QualityAssuranceOutgoingId);
                    var isUniqueUpdate_ix_takt_logistics_quality_assurance_outgoing_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _qualityAssuranceOutgoingRepository,
                        x => x.QualityAssuranceId == x.QualityAssuranceId
                && x.LineNumber == x.LineNumber,
                        childDto.QualityAssuranceOutgoingId);
                    if (!isUniqueUpdate_ix_takt_logistics_quality_assurance_outgoing_line_unique)
                    {
                        throw new TaktBusinessException("品质业务出货检验费用明细的QualityAssuranceId、LineNumber已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.QualityAssuranceOutgoingId;
                    target.QualityAssuranceId = entity.Id;
                    target.IsObsolete = 0;
                    await _qualityAssuranceOutgoingRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_quality_assurance_outgoing_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _qualityAssuranceOutgoingRepository,
                        x => x.QualityAssuranceId == x.QualityAssuranceId
                && x.LineNumber == x.LineNumber);
                    if (!isUniqueCreate_ix_takt_logistics_quality_assurance_outgoing_line_unique)
                    {
                        throw new TaktBusinessException("品质业务出货检验费用明细的QualityAssuranceId、LineNumber已存在");
                    }
                    var child = childDto.Adapt<TaktQualityAssuranceOutgoing>();
                    child.Id = 0;
                    child.QualityAssuranceId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _qualityAssuranceOutgoingRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.QualityAssuranceCode) ? entity.QualityAssuranceCode : entity.Id.ToString();
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
                await _qualityAssuranceOutgoingRepository.CreateRangeAsync(toCreate);
            }
        }
        // 品质业务信赖性评价ORT费用明细（ReliabilityItems）
        List<TaktQualityAssuranceReliabilityUpdateDto>? reliabilityItemsForSave;
        if (dto is TaktQualityAssuranceUpdateDto updateDtoForReliabilityItems && updateDtoForReliabilityItems.ReliabilityItems != null)
        {
            reliabilityItemsForSave = updateDtoForReliabilityItems.ReliabilityItems;
        }
        else if (dto.ReliabilityItems != null)
        {
            reliabilityItemsForSave = dto.ReliabilityItems.Adapt<List<TaktQualityAssuranceReliabilityUpdateDto>>();
        }
        else
        {
            reliabilityItemsForSave = null;
        }
        if (reliabilityItemsForSave is not { Count: > 0 })
        {
            await MarkQualityAssuranceReliabilitysObsoleteAsync(entity.Id);
        }
        else
        {
            var existingList = await _qualityAssuranceReliabilityRepository.GetListAsync(x => x.QualityAssuranceId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktQualityAssuranceReliability>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < reliabilityItemsForSave.Count; i++)
            {
                var childDto = reliabilityItemsForSave[i];
                childDto.QualityAssuranceId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.QualityAssuranceCode = entity.QualityAssuranceCode;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("品质业务信赖性评价ORT费用明细第{i + 1}项与本次提交的其他项重复（CompanyCode、QualityAssuranceId、LineNumber）");
                }
                if (childDto.QualityAssuranceReliabilityId > 0)
                {
                    if (!existingById.TryGetValue(childDto.QualityAssuranceReliabilityId, out var target))
                    {
                        throw new TaktBusinessException("品质业务信赖性评价ORT费用明细不存在（QualityAssuranceReliabilityId={childDto.QualityAssuranceReliabilityId}）");
                    }
                    if (target.QualityAssuranceId != entity.Id)
                    {
                        throw new TaktBusinessException("品质业务信赖性评价ORT费用明细不属于当前主表（QualityAssuranceReliabilityId={childDto.QualityAssuranceReliabilityId}）");
                    }
                    submittedIds.Add(childDto.QualityAssuranceReliabilityId);
                    var isUniqueUpdate_ix_takt_logistics_quality_assurance_reliability_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _qualityAssuranceReliabilityRepository,
                        x => x.QualityAssuranceId == x.QualityAssuranceId
                && x.LineNumber == x.LineNumber,
                        childDto.QualityAssuranceReliabilityId);
                    if (!isUniqueUpdate_ix_takt_logistics_quality_assurance_reliability_line_unique)
                    {
                        throw new TaktBusinessException("品质业务信赖性评价ORT费用明细的QualityAssuranceId、LineNumber已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.QualityAssuranceReliabilityId;
                    target.QualityAssuranceId = entity.Id;
                    target.IsObsolete = 0;
                    await _qualityAssuranceReliabilityRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_quality_assurance_reliability_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _qualityAssuranceReliabilityRepository,
                        x => x.QualityAssuranceId == x.QualityAssuranceId
                && x.LineNumber == x.LineNumber);
                    if (!isUniqueCreate_ix_takt_logistics_quality_assurance_reliability_line_unique)
                    {
                        throw new TaktBusinessException("品质业务信赖性评价ORT费用明细的QualityAssuranceId、LineNumber已存在");
                    }
                    var child = childDto.Adapt<TaktQualityAssuranceReliability>();
                    child.Id = 0;
                    child.QualityAssuranceId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _qualityAssuranceReliabilityRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.QualityAssuranceCode) ? entity.QualityAssuranceCode : entity.Id.ToString();
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
                await _qualityAssuranceReliabilityRepository.CreateRangeAsync(toCreate);
            }
        }
        // 品质业务顾客品质要求对应费用明细（CustomerResponseItems）
        List<TaktQualityAssuranceCustomerResponseUpdateDto>? customerResponseItemsForSave;
        if (dto is TaktQualityAssuranceUpdateDto updateDtoForCustomerResponseItems && updateDtoForCustomerResponseItems.CustomerResponseItems != null)
        {
            customerResponseItemsForSave = updateDtoForCustomerResponseItems.CustomerResponseItems;
        }
        else if (dto.CustomerResponseItems != null)
        {
            customerResponseItemsForSave = dto.CustomerResponseItems.Adapt<List<TaktQualityAssuranceCustomerResponseUpdateDto>>();
        }
        else
        {
            customerResponseItemsForSave = null;
        }
        if (customerResponseItemsForSave is not { Count: > 0 })
        {
            await MarkQualityAssuranceCustomerResponsesObsoleteAsync(entity.Id);
        }
        else
        {
            var existingList = await _qualityAssuranceCustomerResponseRepository.GetListAsync(x => x.QualityAssuranceId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktQualityAssuranceCustomerResponse>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < customerResponseItemsForSave.Count; i++)
            {
                var childDto = customerResponseItemsForSave[i];
                childDto.QualityAssuranceId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.QualityAssuranceCode = entity.QualityAssuranceCode;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("品质业务顾客品质要求对应费用明细第{i + 1}项与本次提交的其他项重复（CompanyCode、QualityAssuranceId、LineNumber）");
                }
                if (childDto.QualityAssuranceCustomerResponseId > 0)
                {
                    if (!existingById.TryGetValue(childDto.QualityAssuranceCustomerResponseId, out var target))
                    {
                        throw new TaktBusinessException("品质业务顾客品质要求对应费用明细不存在（QualityAssuranceCustomerResponseId={childDto.QualityAssuranceCustomerResponseId}）");
                    }
                    if (target.QualityAssuranceId != entity.Id)
                    {
                        throw new TaktBusinessException("品质业务顾客品质要求对应费用明细不属于当前主表（QualityAssuranceCustomerResponseId={childDto.QualityAssuranceCustomerResponseId}）");
                    }
                    submittedIds.Add(childDto.QualityAssuranceCustomerResponseId);
                    var isUniqueUpdate_ix_takt_logistics_quality_assurance_customer_response_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _qualityAssuranceCustomerResponseRepository,
                        x => x.QualityAssuranceId == x.QualityAssuranceId
                && x.LineNumber == x.LineNumber,
                        childDto.QualityAssuranceCustomerResponseId);
                    if (!isUniqueUpdate_ix_takt_logistics_quality_assurance_customer_response_line_unique)
                    {
                        throw new TaktBusinessException("品质业务顾客品质要求对应费用明细的QualityAssuranceId、LineNumber已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.QualityAssuranceCustomerResponseId;
                    target.QualityAssuranceId = entity.Id;
                    target.IsObsolete = 0;
                    await _qualityAssuranceCustomerResponseRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_quality_assurance_customer_response_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _qualityAssuranceCustomerResponseRepository,
                        x => x.QualityAssuranceId == x.QualityAssuranceId
                && x.LineNumber == x.LineNumber);
                    if (!isUniqueCreate_ix_takt_logistics_quality_assurance_customer_response_line_unique)
                    {
                        throw new TaktBusinessException("品质业务顾客品质要求对应费用明细的QualityAssuranceId、LineNumber已存在");
                    }
                    var child = childDto.Adapt<TaktQualityAssuranceCustomerResponse>();
                    child.Id = 0;
                    child.QualityAssuranceId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _qualityAssuranceCustomerResponseRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.QualityAssuranceCode) ? entity.QualityAssuranceCode : entity.Id.ToString();
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
                await _qualityAssuranceCustomerResponseRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建品质业务主查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktQualityAssurance, bool>> QueryExpression(TaktQualityAssuranceQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktQualityAssurance>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.QualityAssuranceCode != null && x.QualityAssuranceCode.Contains(keywords))
                || (x.AssuranceMonth != null && x.AssuranceMonth.Contains(keywords))
                || (x.CustomerName1 != null && x.CustomerName1.Contains(keywords))
                || (x.DebitNoteCode != null && x.DebitNoteCode.Contains(keywords))
                || (x.RecordGroupName != null && x.RecordGroupName.Contains(keywords))
                || (x.CurrencyCode != null && x.CurrencyCode.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.QualityAssuranceCode))
        {
            var qualityAssuranceCode = queryDto.QualityAssuranceCode;
            exp = exp.And(x => x.QualityAssuranceCode != null && x.QualityAssuranceCode.Contains(qualityAssuranceCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AssuranceMonth))
        {
            var assuranceMonth = queryDto.AssuranceMonth;
            exp = exp.And(x => x.AssuranceMonth != null && x.AssuranceMonth.Contains(assuranceMonth));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CustomerName1))
        {
            var customerName1 = queryDto.CustomerName1;
            exp = exp.And(x => x.CustomerName1 != null && x.CustomerName1.Contains(customerName1));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DebitNoteCode))
        {
            var debitNoteCode = queryDto.DebitNoteCode;
            exp = exp.And(x => x.DebitNoteCode != null && x.DebitNoteCode.Contains(debitNoteCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RecordGroupName))
        {
            var recorder = queryDto.RecordGroupName;
            exp = exp.And(x => x.RecordGroupName != null && x.RecordGroupName.Contains(recorder));
        }

        if (queryDto?.TotalQualityCost.HasValue == true)
        {
            var totalQualityCost = queryDto.TotalQualityCost.Value;
            exp = exp.And(x => x.TotalQualityCost == totalQualityCost);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CurrencyCode))
        {
            var currencyCode = queryDto.CurrencyCode;
            exp = exp.And(x => x.CurrencyCode != null && x.CurrencyCode.Contains(currencyCode));
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
    private static bool HasAnyListQueryFilter(TaktQualityAssuranceQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.QualityAssuranceCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AssuranceMonth))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CustomerName1))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DebitNoteCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RecordGroupName))
        {
            return true;
        }
        if (queryDto.TotalQualityCost.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CurrencyCode))
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
        if (queryDto.CreatedAtStart.HasValue || queryDto.CreatedAtEnd.HasValue)
        {
            return true;
        }
        return false;
    }

    // ========================================
    // 扩展方法（保留）
    // ========================================

    /// <summary>
    /// 获取品质业务金额统计（数据看板；按 AssuranceMonth 汇总 TotalQualityCost）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>品质业务金额统计</returns>
    public async Task<TaktQualityCostStatDto> GetQualityAssuranceCostStatAsync(TaktQualityCostStatQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        EnsureThreeLayerContext();
        var (_, _, statMonth) = TaktStatMonthRangeHelper.ResolveStatMonthRange(
            queryDto.DateStart,
            queryDto.DateEnd,
            queryDto.StatMonth);
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;
        Expression<Func<TaktQualityAssurance, bool>> predicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.AssuranceMonth == statMonth;
        var monthRowCount = await _qualityAssuranceRepository.CountAsync(predicate);
        var monthTotalAmount = await _qualityAssuranceRepository.SumAsync(x => x.TotalQualityCost, predicate);
        return new TaktQualityCostStatDto
        {
            StatMonth = statMonth,
            MonthTotalAmount = monthTotalAmount,
            MonthRowCount = monthRowCount,
        };
    }
}
