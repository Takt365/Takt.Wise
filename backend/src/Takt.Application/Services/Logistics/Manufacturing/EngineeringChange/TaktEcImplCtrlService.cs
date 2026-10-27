// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcImplCtrlService.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变实施管控应用服务实现
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Entities;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Constants;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变实施管控应用服务。生管读 TaktEcPc，制一读 TaktEcMfg1，制二读 TaktEcMfg2，品管读 TaktEcQc。
/// 保存时只改当前页签对应执行表的控制范围。不管理且内容为空时写入「实施范围:不管理:N/A」。
/// </summary>
public class TaktEcImplCtrlService : TaktServiceBase, ITaktEcImplCtrlService
{
    private readonly ITaktCompanyRepository<TaktEcDetail> _ecDetailRepository;
    private readonly ITaktCompanyRepository<TaktEcPc> _pcRepository;
    private readonly ITaktCompanyRepository<TaktEcMfg2> _pcbaRepository;
    private readonly ITaktCompanyRepository<TaktEcMfg1> _assyRepository;
    private readonly ITaktCompanyRepository<TaktEcQc> _qaRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecDetailRepository">设变明细仓储</param>
    /// <param name="pcRepository">生管执行仓储</param>
    /// <param name="pcbaRepository">制二执行仓储</param>
    /// <param name="assyRepository">制一执行仓储</param>
    /// <param name="qaRepository">品管执行仓储</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEcImplCtrlService(
        ITaktCompanyRepository<TaktEcDetail> ecDetailRepository,
        ITaktCompanyRepository<TaktEcPc> pcRepository,
        ITaktCompanyRepository<TaktEcMfg2> pcbaRepository,
        ITaktCompanyRepository<TaktEcMfg1> assyRepository,
        ITaktCompanyRepository<TaktEcQc> qaRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _ecDetailRepository = ecDetailRepository;
        _pcRepository = pcRepository;
        _pcbaRepository = pcbaRepository;
        _assyRepository = assyRepository;
        _qaRepository = qaRepository;
    }

    /// <summary>
    /// 获取实施管控列表（分页）。按页签读取对应执行表，不读设变明细的单条物料。
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>分页结果</returns>
    public Task<TaktPagedResult<TaktEcDetailDto>> GetEcImplCtrlListAsync(TaktEcImplCtrlQueryDto queryDto)
    {
        EnsureThreeLayerContext();
        return PageExecAsync(queryDto);
    }

    /// <summary>
    /// 根据设变明细 ID 获取实施管控行
    /// </summary>
    /// <param name="ecDetailId">设变明细 ID</param>
    /// <returns>实施管控 DTO</returns>
    public async Task<TaktEcDetailDto?> GetEcImplCtrlByEcDetailIdAsync(long ecDetailId)
    {
        EnsureThreeLayerContext();
        var detail = await _ecDetailRepository.GetByIdAsync(ecDetailId);
        if (detail == null || detail.TenantCode != CurrentTenantCode || detail.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return detail.Adapt<TaktEcDetailDto>();
    }

    /// <summary>
    /// 更新实施管控
    /// </summary>
    /// <param name="ecDetailId">设变明细 ID</param>
    /// <param name="dto">更新 DTO</param>
    /// <returns>实施管控 DTO</returns>
    public async Task<TaktEcDetailDto> UpdateEcImplCtrlAsync(long ecDetailId, TaktEcImplCtrlUpdateDto dto)
    {
        EnsureThreeLayerContext();
        var tab = NormalizeDeptTab(dto.DeptTab);
        var controlScope = NormalizeControlScope(dto.EcControlScope);
        if (tab == "pc")
        {
            return await SaveImplCtrlRowAsync(_pcRepository, ecDetailId, controlScope, MapPc);
        }
        if (tab == "mfg1")
        {
            return await SaveImplCtrlRowAsync(_assyRepository, ecDetailId, controlScope, row => MapGroup(row, tab));
        }
        if (tab == "qc")
        {
            return await SaveImplCtrlRowAsync(_qaRepository, ecDetailId, controlScope, row => MapGroup(row, tab));
        }
        return await SaveImplCtrlRowAsync(_pcbaRepository, ecDetailId, controlScope, row => MapGroup(row, tab));
    }
    /// <summary>
    /// 按执行行主键写入控制范围。
    /// </summary>
    /// <typeparam name="T">部门执行实体</typeparam>
    /// <param name="repository">部门执行仓储</param>
    /// <param name="execId">执行行主键</param>
    /// <param name="controlScope">控制范围</param>
    /// <param name="map">列表行映射</param>
    /// <returns>列表行</returns>
    private async Task<TaktEcDetailDto> SaveImplCtrlRowAsync<T>(
        ITaktCompanyRepository<T> repository,
        long execId,
        int controlScope,
        Func<T, TaktEcDetailDto> map)
        where T : TaktCompanyEntityBase, ITaktEcDeptExecEntity, new()
    {
        var row = await repository.GetByIdAsync(execId);
        if (row == null || row.TenantCode != CurrentTenantCode || row.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("设变明细不存在");
        }
        row.EcControlScope = controlScope;
        if (controlScope == TaktEcControlScopeConstants.NotManaged
            && string.IsNullOrWhiteSpace(row.ExecContent))
        {
            row.ExecContent = TaktEcScopeConstants.FormatScopeDeptExecContent(
                row.EcImplScope,
                TaktEcControlScopeConstants.NotManaged,
                TaktEcScopeConstants.NotApplicableExecSuffix);
        }
        await repository.UpdateAsync(row);
        var result = map(row);
        result.EcControlScope = controlScope;
        return result;
    }

    /// <summary>
    /// 导出实施管控
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEcImplCtrlAsync(
        TaktEcImplCtrlQueryDto? query = null,
        string? sheetName = null,
        string? fileName = null)
    {
        EnsureThreeLayerContext();
        var rows = await ListExecForExportAsync(query ?? new TaktEcImplCtrlQueryDto());
        return await TaktExcelHelper.ExportAsync(
            rows,
            sheetName ?? "实施管控",
            fileName ?? "实施管控导出.xlsx");
    }

    /// <summary>
    /// 按页签分页读取执行表
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>分页结果</returns>
    private async Task<TaktPagedResult<TaktEcDetailDto>> PageExecAsync(TaktEcImplCtrlQueryDto queryDto)
    {
        var tab = NormalizeDeptTab(queryDto.DeptTab);
        if (tab == "pc")
        {
            var (data, total) = await _pcRepository.GetPagedAsync(
                PcExpression(queryDto),
                queryDto.PageIndex,
                queryDto.PageSize,
                x => x.Id,
                false);
            return TaktPagedResult<TaktEcDetailDto>.Create(data.Select(MapPc).ToList(), total, queryDto.PageIndex, queryDto.PageSize);
        }
        if (tab == "mfg1")
        {
            var (data, total) = await _assyRepository.GetPagedAsync(
                AssyExpression(queryDto),
                queryDto.PageIndex,
                queryDto.PageSize,
                x => x.Id,
                false);
            return TaktPagedResult<TaktEcDetailDto>.Create(data.Select(x => MapGroup(x, "mfg1")).ToList(), total, queryDto.PageIndex, queryDto.PageSize);
        }
        if (tab == "qc")
        {
            var (data, total) = await _qaRepository.GetPagedAsync(
                QaExpression(queryDto),
                queryDto.PageIndex,
                queryDto.PageSize,
                x => x.Id,
                false);
            return TaktPagedResult<TaktEcDetailDto>.Create(data.Select(x => MapGroup(x, "qc")).ToList(), total, queryDto.PageIndex, queryDto.PageSize);
        }
        var pcba = await _pcbaRepository.GetPagedAsync(
            PcbaExpression(queryDto),
            queryDto.PageIndex,
            queryDto.PageSize,
            x => x.Id,
            false);
        return TaktPagedResult<TaktEcDetailDto>.Create(pcba.Item1.Select(x => MapGroup(x, "mfg2")).ToList(), pcba.Item2, queryDto.PageIndex, queryDto.PageSize);
    }

    /// <summary>
    /// 按页签导出执行表
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>导出行</returns>
    private async Task<List<TaktEcDetailDto>> ListExecForExportAsync(TaktEcImplCtrlQueryDto queryDto)
    {
        var tab = NormalizeDeptTab(queryDto.DeptTab);
        if (tab == "pc")
        {
            var list = await _pcRepository.GetListForExportAsync(PcExpression(queryDto));
            return list.Select(MapPc).ToList();
        }
        if (tab == "mfg1")
        {
            var list = await _assyRepository.GetListForExportAsync(AssyExpression(queryDto));
            return list.Select(x => MapGroup(x, "mfg1")).ToList();
        }
        if (tab == "qc")
        {
            var list = await _qaRepository.GetListForExportAsync(QaExpression(queryDto));
            return list.Select(x => MapGroup(x, "qc")).ToList();
        }
        var pcba = await _pcbaRepository.GetListForExportAsync(PcbaExpression(queryDto));
        return pcba.Select(x => MapGroup(x, "mfg2")).ToList();
    }

    /// <summary>
    /// 页签归一：pc 生管、assy 制一、pcba 制二、qa 品管
    /// </summary>
    /// <param name="deptTab">页签</param>
    /// <returns>页签</returns>
    private static string NormalizeDeptTab(string? deptTab)
    {
        var tab = (deptTab ?? string.Empty).Trim().ToLowerInvariant();
        return tab is "pc" or "mfg1" or "qc" ? tab : "mfg2";
    }

    /// <summary>
    /// 生管执行行映射。机种取机种编码组。
    /// </summary>
    /// <param name="row">生管执行行</param>
    /// <returns>列表行</returns>
    private static TaktEcDetailDto MapPc(TaktEcPc row)
    {
        return new TaktEcDetailDto
        {
            EcDetailId = row.Id,
            EcCode = row.EcCode,
            EcModelCodeGroup = row.EcModelCodeGroup,
            EcRootMaterialCodeGroup = row.EcRootMaterialCodeGroup,
            EcControlScope = row.EcControlScope,
        };
    }

    /// <summary>
    /// 制一、制二、品管执行行映射
    /// </summary>
    /// <param name="row">执行行</param>
    /// <param name="tab">页签</param>
    /// <returns>列表行</returns>
    private static TaktEcDetailDto MapGroup(ITaktEcDeptControlledEntity row, string tab)
    {
        var modelGroup = row switch
        {
            TaktEcMfg1 assy => assy.EcModelCodeGroup,
            TaktEcMfg2 pcba => pcba.EcModelCodeGroup,
            TaktEcQc qa => qa.EcModelCodeGroup,
            _ => string.Empty,
        };
        var rootGroup = row switch
        {
            TaktEcMfg1 assy => assy.EcRootMaterialCodeGroup,
            TaktEcMfg2 pcba => pcba.EcRootMaterialCodeGroup,
            TaktEcQc qa => qa.EcRootMaterialCodeGroup,
            _ => string.Empty,
        };
        return new TaktEcDetailDto
        {
            EcDetailId = row.Id,
            EcCode = row.EcCode,
            EcModelCodeGroup = modelGroup,
            EcRootMaterialCodeGroup = rootGroup,
            EcControlScope = row.EcControlScope,
        };
    }

    /// <summary>
    /// 按技术主表录入日期收窄执行行
    /// </summary>
    /// <typeparam name="T">执行部门实体</typeparam>
    /// <param name="exp">已有条件</param>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>追加录入期间后的条件</returns>
    private static Expressionable<T> ApplyEntryPeriod<T>(Expressionable<T> exp, TaktEcImplCtrlQueryDto queryDto)
        where T : TaktCompanyEntityBase, ITaktEcDeptExecEntity, new()
    {
        if (queryDto.EcEntryDateStart.HasValue)
        {
            var start = queryDto.EcEntryDateStart.Value;
            exp = exp.And(x => SqlFunc.Subqueryable<TaktEcTe>()
                .Where(g => g.TenantCode == x.TenantCode
                    && g.CompanyCode == x.CompanyCode
                    && g.EcCode == x.EcCode
                    && g.IsDeleted == 0
                    && g.EcEntryDate >= start)
                .Any());
        }
        if (queryDto.EcEntryDateEnd.HasValue)
        {
            var end = queryDto.EcEntryDateEnd.Value;
            exp = exp.And(x => SqlFunc.Subqueryable<TaktEcTe>()
                .Where(g => g.TenantCode == x.TenantCode
                    && g.CompanyCode == x.CompanyCode
                    && g.EcCode == x.EcCode
                    && g.IsDeleted == 0
                    && g.EcEntryDate <= end)
                .Any());
        }
        return exp;
    }

    /// <summary>
    /// 生管执行表条件：未作废。关键字匹配设变单号、机种编码、根物料编码组。
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>表达式</returns>
    private static Expression<Func<TaktEcPc, bool>> PcExpression(TaktEcImplCtrlQueryDto queryDto)
    {
        var exp = Expressionable.Create<TaktEcPc>();
        exp = exp.And(x => x.IsObsolete == 0);
        if (!string.IsNullOrWhiteSpace(queryDto.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.EcCode != null && x.EcCode.Contains(keywords))
                || SqlFunc.ToString(x.EcModelCodeGroup).Contains(keywords)
                || SqlFunc.ToString(x.EcRootMaterialCodeGroup).Contains(keywords));
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EcCode))
        {
            var ecCode = queryDto.EcCode;
            exp = exp.And(x => x.EcCode != null && x.EcCode.Contains(ecCode));
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EcModelCode))
        {
            var modelCode = queryDto.EcModelCode;
            exp = exp.And(x => SqlFunc.ToString(x.EcModelCodeGroup).Contains(modelCode));
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EcOldMaterialCode))
        {
            var materialCode = queryDto.EcOldMaterialCode;
            exp = exp.And(x => SqlFunc.ToString(x.EcRootMaterialCodeGroup).Contains(materialCode));
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EcNewMaterialCode))
        {
            var materialCode = queryDto.EcNewMaterialCode;
            exp = exp.And(x => SqlFunc.ToString(x.EcRootMaterialCodeGroup).Contains(materialCode));
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CultureCode))
        {
            var cultureCode = queryDto.CultureCode;
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(cultureCode));
        }
        exp = ApplyEntryPeriod(exp, queryDto);
        return exp.ToExpression();
    }

    /// <summary>
    /// 制一执行表条件。机种条件匹配机种编码组，物料条件匹配根物料编码组。
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>表达式</returns>
    private static Expression<Func<TaktEcMfg1, bool>> AssyExpression(TaktEcImplCtrlQueryDto queryDto)
    {
        var exp = Expressionable.Create<TaktEcMfg1>();
        exp = exp.And(x => x.IsObsolete == 0);
        if (!string.IsNullOrWhiteSpace(queryDto.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.EcCode != null && x.EcCode.Contains(keywords))
                || SqlFunc.ToString(x.EcModelCodeGroup).Contains(keywords)
                || SqlFunc.ToString(x.EcRootMaterialCodeGroup).Contains(keywords));
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EcCode))
        {
            var ecCode = queryDto.EcCode;
            exp = exp.And(x => x.EcCode != null && x.EcCode.Contains(ecCode));
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EcModelCode))
        {
            var modelCode = queryDto.EcModelCode;
            exp = exp.And(x => SqlFunc.ToString(x.EcModelCodeGroup).Contains(modelCode));
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EcOldMaterialCode))
        {
            var materialCode = queryDto.EcOldMaterialCode;
            exp = exp.And(x => SqlFunc.ToString(x.EcRootMaterialCodeGroup).Contains(materialCode));
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EcNewMaterialCode))
        {
            var materialCode = queryDto.EcNewMaterialCode;
            exp = exp.And(x => SqlFunc.ToString(x.EcRootMaterialCodeGroup).Contains(materialCode));
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CultureCode))
        {
            var cultureCode = queryDto.CultureCode;
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(cultureCode));
        }
        exp = ApplyEntryPeriod(exp, queryDto);
        return exp.ToExpression();
    }

    /// <summary>
    /// 制二执行表条件。机种条件匹配机种编码组，物料条件匹配根物料编码组。
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>表达式</returns>
    private static Expression<Func<TaktEcMfg2, bool>> PcbaExpression(TaktEcImplCtrlQueryDto queryDto)
    {
        var exp = Expressionable.Create<TaktEcMfg2>();
        exp = exp.And(x => x.IsObsolete == 0);
        if (!string.IsNullOrWhiteSpace(queryDto.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.EcCode != null && x.EcCode.Contains(keywords))
                || SqlFunc.ToString(x.EcModelCodeGroup).Contains(keywords)
                || SqlFunc.ToString(x.EcRootMaterialCodeGroup).Contains(keywords));
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EcCode))
        {
            var ecCode = queryDto.EcCode;
            exp = exp.And(x => x.EcCode != null && x.EcCode.Contains(ecCode));
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EcModelCode))
        {
            var modelCode = queryDto.EcModelCode;
            exp = exp.And(x => SqlFunc.ToString(x.EcModelCodeGroup).Contains(modelCode));
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EcOldMaterialCode))
        {
            var materialCode = queryDto.EcOldMaterialCode;
            exp = exp.And(x => SqlFunc.ToString(x.EcRootMaterialCodeGroup).Contains(materialCode));
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EcNewMaterialCode))
        {
            var materialCode = queryDto.EcNewMaterialCode;
            exp = exp.And(x => SqlFunc.ToString(x.EcRootMaterialCodeGroup).Contains(materialCode));
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CultureCode))
        {
            var cultureCode = queryDto.CultureCode;
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(cultureCode));
        }
        exp = ApplyEntryPeriod(exp, queryDto);
        return exp.ToExpression();
    }

    /// <summary>
    /// 品管执行表条件。机种条件匹配机种编码组，物料条件匹配根物料编码组。
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>表达式</returns>
    private static Expression<Func<TaktEcQc, bool>> QaExpression(TaktEcImplCtrlQueryDto queryDto)
    {
        var exp = Expressionable.Create<TaktEcQc>();
        exp = exp.And(x => x.IsObsolete == 0);
        if (!string.IsNullOrWhiteSpace(queryDto.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.EcCode != null && x.EcCode.Contains(keywords))
                || SqlFunc.ToString(x.EcModelCodeGroup).Contains(keywords)
                || SqlFunc.ToString(x.EcRootMaterialCodeGroup).Contains(keywords));
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EcCode))
        {
            var ecCode = queryDto.EcCode;
            exp = exp.And(x => x.EcCode != null && x.EcCode.Contains(ecCode));
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EcModelCode))
        {
            var modelCode = queryDto.EcModelCode;
            exp = exp.And(x => SqlFunc.ToString(x.EcModelCodeGroup).Contains(modelCode));
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EcOldMaterialCode))
        {
            var materialCode = queryDto.EcOldMaterialCode;
            exp = exp.And(x => SqlFunc.ToString(x.EcRootMaterialCodeGroup).Contains(materialCode));
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EcNewMaterialCode))
        {
            var materialCode = queryDto.EcNewMaterialCode;
            exp = exp.And(x => SqlFunc.ToString(x.EcRootMaterialCodeGroup).Contains(materialCode));
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CultureCode))
        {
            var cultureCode = queryDto.CultureCode;
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(cultureCode));
        }
        exp = ApplyEntryPeriod(exp, queryDto);
        return exp.ToExpression();
    }

    /// <summary>
    /// 只接受 1～4，其余按机种。
    /// </summary>
    /// <param name="ecControlScope">提交的控制范围</param>
    /// <returns>控制范围</returns>
    private static int NormalizeControlScope(int ecControlScope)
    {
        return ecControlScope is TaktEcControlScopeConstants.ByFinishedProduct
            or TaktEcControlScopeConstants.ByModel
            or TaktEcControlScopeConstants.ByEngineeringChange
            or TaktEcControlScopeConstants.NotManaged
            ? ecControlScope
            : TaktEcControlScopeConstants.ByModel;
    }

    /// <summary>
    /// 写入当前部门执行行的控制范围。已有执行内容不覆盖；不管理且内容为空时写入实施范围:不管理:N/A。
    /// </summary>
    /// <typeparam name="T">部门执行实体</typeparam>
    /// <param name="repository">部门执行仓储</param>
    /// <param name="detailIds">同一确认粒度的明细主键</param>
    /// <param name="detail">代表明细，提供实施范围</param>
    /// <param name="ecControlScope">控制范围</param>
    private static async Task ApplyControlScopeAsync<T>(
        ITaktCompanyRepository<T> repository,
        List<long> detailIds,
        TaktEcDetail detail,
        int ecControlScope)
        where T : TaktCompanyEntityBase, ITaktEcDeptExecEntity, new()
    {
        if (detailIds.Count == 0)
        {
            return;
        }
        var ecCode = detail.EcCode ?? string.Empty;
        var rows = (await repository.GetListAsync(x => x.EcCode == ecCode && x.IsObsolete == 0))
            .Where(x => x.EcGrainKey == TaktEcScopeRowGrain.BuildExecGrainKey(detail, x.DeptCode, ecControlScope)
                || x.EcGrainKey == TaktEcScopeRowGrain.BuildExecGrainKey(detail, x.DeptCode, x.EcControlScope))
            .ToList();
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.EcControlScope = ecControlScope;
            if (ecControlScope == TaktEcControlScopeConstants.NotManaged)
            {
                TaktEcDeptEntityHelper.SetExecContent(
                    row,
                    TaktEcScopeConstants.FormatScopeDeptExecContent(
                        detail.EcImplScope,
                        TaktEcControlScopeConstants.NotManaged,
                        TaktEcScopeConstants.NotApplicableExecSuffix),
                    overwrite: false);
            }
        }
        await repository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 与列表去重同一粒度：按成品比根物料，按设变只比设变单号，其余比机种。
    /// </summary>
    /// <param name="current">正在确认的明细</param>
    /// <param name="other">同设变明细</param>
    /// <param name="grain">当前执行行控制范围（1=按成品，2=按机种，3=按设变）</param>
    /// <returns>属于同一确认行时为 true</returns>
    private static bool SameDeptRequiredGrain(TaktEcDetail current, TaktEcDetail other, int grain)
    {
        if (grain == TaktEcControlScopeConstants.ByFinishedProduct)
        {
            return string.Equals(current.EcRootMaterialCode, other.EcRootMaterialCode, StringComparison.OrdinalIgnoreCase);
        }
        if (grain == TaktEcControlScopeConstants.ByEngineeringChange)
        {
            return true;
        }
        return string.Equals(current.EcModelCode, other.EcModelCode, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 读取当前页签执行行上的控制范围，作为同行匹配粒度。
    /// </summary>
    /// <param name="ecDetailId">设变明细 ID</param>
    /// <param name="tab">页签 pc/pcba/assy/qa</param>
    /// <returns>执行行控制范围；没有执行行时按机种</returns>
    private async Task<int> ResolveExecControlScopeAsync(long ecDetailId, string tab)
    {
        if (tab is "" or "pc")
        {
            var pc = await _pcRepository.GetByIdAsync(ecDetailId);
            if (pc != null)
            {
                return pc.EcControlScope;
            }
            if (tab == "pc")
            {
                return TaktEcControlScopeConstants.ByModel;
            }
        }
        if (tab is "" or "mfg2")
        {
            var nika = await _pcbaRepository.GetByIdAsync(ecDetailId);
            if (nika != null)
            {
                return nika.EcControlScope;
            }
            if (tab == "mfg2")
            {
                return TaktEcControlScopeConstants.ByModel;
            }
        }
        if (tab is "" or "mfg1")
        {
            var ikka = await _assyRepository.GetByIdAsync(ecDetailId);
            if (ikka != null)
            {
                return ikka.EcControlScope;
            }
            if (tab == "mfg1")
            {
                return TaktEcControlScopeConstants.ByModel;
            }
        }
        if (tab is "" or "qc")
        {
            var qa = await _qaRepository.GetByIdAsync(ecDetailId);
            if (qa != null)
            {
                return qa.EcControlScope;
            }
        }
        return TaktEcControlScopeConstants.ByModel;
    }
}
