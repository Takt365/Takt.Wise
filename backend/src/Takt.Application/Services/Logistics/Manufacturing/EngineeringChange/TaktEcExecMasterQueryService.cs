// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcExecMasterQueryService.cs
// 创建时间：2026-08-27
// 创建人：Takt365(Cursor AI)
// 功能描述：执行部门左栏主表分页查询（TaktEcDetail）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Constants;
using Takt.Shared.Models;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 执行部门左栏主表查询（TaktEcDetail；各部门执行行经 EcDetailId 派生，视图侧经 EcXxxId 回挂）。
/// </summary>
public class TaktEcExecMasterQueryService : TaktServiceBase, ITaktEcExecMasterQueryService
{
    private readonly ITaktCompanyRepository<TaktEcDetail> _ecDetailRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecDetailRepository">设变明细仓储</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEcExecMasterQueryService(
        ITaktCompanyRepository<TaktEcDetail> ecDetailRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _ecDetailRepository = ecDetailRepository;
    }

    /// <summary>
    /// 分页查询设变明细（不含部门执行导航）。
    /// </summary>
    /// <param name="queryDto">查询条件；为空时按默认分页。</param>
    /// <param name="execDeptCode">执行部门编码；生管/采购/受检/部管/制二/制一/品管/制技附加可见明细条件。</param>
    /// <returns>分页结果。</returns>
    public async Task<TaktPagedResult<TaktEcDetailDto>> GetEcDetailMasterListAsync(
        TaktEcDetailQueryDto? queryDto,
        string? execDeptCode = null)
    {
        queryDto ??= new TaktEcDetailQueryDto();
        var exp = Expressionable.Create<TaktEcDetail>();
        exp = exp.And(TaktEcDetailService.QueryExpression(queryDto));
        if (string.Equals(execDeptCode, TaktEcDeptCodes.Pc, StringComparison.Ordinal))
        {
            exp = exp.And(TaktEcPcQueryHelper.VisibleDetailExpression());
        }
        else if (string.Equals(execDeptCode, TaktEcDeptCodes.Pur, StringComparison.Ordinal))
        {
            exp = exp.And(TaktEcPurQueryHelper.VisibleDetailExpression());
        }
        else if (string.Equals(execDeptCode, TaktEcDeptCodes.Iqc, StringComparison.Ordinal))
        {
            exp = exp.And(TaktEcIqcQueryHelper.VisibleDetailExpression());
        }
        else if (string.Equals(execDeptCode, TaktEcDeptCodes.Mc, StringComparison.Ordinal))
        {
            exp = exp.And(TaktEcMcQueryHelper.VisibleDetailExpression());
        }
        else if (string.Equals(execDeptCode, TaktEcDeptCodes.Mfg2, StringComparison.Ordinal))
        {
            exp = exp.And(TaktEcMtlQueryHelper.VisibleDetailExpression());
        }
        else if (string.Equals(execDeptCode, TaktEcDeptCodes.Mfg2Master, StringComparison.Ordinal))
        {
            exp = exp.And(TaktEcMfg2QueryHelper.VisibleDetailExpression());
        }
        else if (string.Equals(execDeptCode, TaktEcDeptCodes.Mfg1, StringComparison.Ordinal))
        {
            exp = exp.And(TaktEcMfg1QueryHelper.VisibleDetailExpression());
        }
        else if (string.Equals(execDeptCode, TaktEcDeptCodes.Qc, StringComparison.Ordinal))
        {
            exp = exp.And(TaktEcQcQueryHelper.VisibleDetailExpression());
        }
        else if (string.Equals(execDeptCode, TaktEcDeptCodes.Me, StringComparison.Ordinal))
        {
            exp = exp.And(TaktEcMeQueryHelper.VisibleDetailExpression());
        }
        var predicate = exp.ToExpression();
        var (data, total) = await _ecDetailRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        var items = data.Adapt<List<TaktEcDetailDto>>();
        return TaktPagedResult<TaktEcDetailDto>.Create(
            items,
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }
}
