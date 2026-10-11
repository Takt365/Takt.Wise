// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcExecDeptAccess.cs
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：设变部门执行跨表访问（8 张 TaktEcExec* 部门表，按 DeptCode 路由）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Takt.Domain.Entities;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Repositories;
using Takt.Shared.Constants;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变部门执行公共字段快照（跨 8 张子表聚合用）
/// </summary>
public sealed class TaktEcExecBaseRow
{
    /// <summary>
    /// 主键
    /// </summary>
    public long Id { get; init; }
    /// <summary>
    /// 设变明细 ID
    /// </summary>
    public long EcDetailId { get; init; }
    /// <summary>
    /// 设变单号
    /// </summary>
    public string EcCode { get; init; } = string.Empty;
    /// <summary>
    /// 部门编码
    /// </summary>
    public string DeptCode { get; init; } = string.Empty;
    /// <summary>
    /// 行号
    /// </summary>
    public int LineNumber { get; init; }
    /// <summary>
    /// 是否实施
    /// </summary>
    public int IsImplemented { get; init; }
    /// <summary>
    /// 执行内容
    /// </summary>
    public string? ExecContent { get; init; }
    /// <summary>
    /// 是否作废
    /// </summary>
    public int IsObsolete { get; init; }
    /// <summary>
    /// 租户编码
    /// </summary>
    public string TenantCode { get; init; } = string.Empty;
    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; init; } = string.Empty;
}

/// <summary>
/// 设变部门执行跨表访问
/// </summary>
public class TaktEcExecDeptAccess
{
    private readonly ITaktCompanyRepository<TaktEcSeikan> _pmcRepository;
    private readonly ITaktCompanyRepository<TaktEcKoubai> _mpRepository;
    private readonly ITaktCompanyRepository<TaktEcUkeken> _iqcRepository;
    private readonly ITaktCompanyRepository<TaktEcBukan> _mcRepository;
    private readonly ITaktCompanyRepository<TaktEcSeizounika> _seizounikaRepository;
    private readonly ITaktCompanyRepository<TaktEcSmt> _smtRepository;
    private readonly ITaktCompanyRepository<TaktEcSeizouikka> _assyRepository;
    private readonly ITaktCompanyRepository<TaktEcHinkan> _qaRepository;
    private readonly ITaktCompanyRepository<TaktEcSeizougijutsu> _teRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    public TaktEcExecDeptAccess(
        ITaktCompanyRepository<TaktEcSeikan> pmcRepository,
        ITaktCompanyRepository<TaktEcKoubai> mpRepository,
        ITaktCompanyRepository<TaktEcUkeken> iqcRepository,
        ITaktCompanyRepository<TaktEcBukan> mcRepository,
        ITaktCompanyRepository<TaktEcSeizounika> seizounikaRepository,
        ITaktCompanyRepository<TaktEcSmt> smtRepository,
        ITaktCompanyRepository<TaktEcSeizouikka> assyRepository,
        ITaktCompanyRepository<TaktEcHinkan> qaRepository,
        ITaktCompanyRepository<TaktEcSeizougijutsu> teRepository)
    {
        _pmcRepository = pmcRepository;
        _mpRepository = mpRepository;
        _iqcRepository = iqcRepository;
        _mcRepository = mcRepository;
        _seizounikaRepository = seizounikaRepository;
        _smtRepository = smtRepository;
        _assyRepository = assyRepository;
        _qaRepository = qaRepository;
        _teRepository = teRepository;
    }

    /// <summary>
    /// 生管课仓储
    /// </summary>
    public ITaktCompanyRepository<TaktEcSeikan> PmcRepository => _pmcRepository;

    /// <summary>
    /// 制二课（非 F）仓储
    /// </summary>
    public ITaktCompanyRepository<TaktEcSeizounika> SeizounikaRepository => _seizounikaRepository;

    /// <summary>
    /// PCBA/SMT（F+C003，DeptCode=D0625）仓储
    /// </summary>
    public ITaktCompanyRepository<TaktEcSmt> SmtRepository => _smtRepository;

    /// <summary>
    /// 按设变单号与部门取首条执行记录（公共字段）
    /// </summary>
    /// <param name="ecCode">设变单号</param>
    /// <param name="deptCode">部门编码</param>
    /// <returns>公共字段快照；不存在时 null</returns>
    public async Task<TaktEcExecBaseRow?> FirstBaseByEcCodeAndDeptAsync(string ecCode, string deptCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ecCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(deptCode);
        return deptCode switch
        {
            TaktEcDeptCodes.Pmc => ToBaseRow(await _pmcRepository.FirstAsync(x => x.EcCode == ecCode && x.IsDeleted == 0)),
            TaktEcDeptCodes.Mp => ToBaseRow(await _mpRepository.FirstAsync(x => x.EcCode == ecCode && x.IsDeleted == 0)),
            TaktEcDeptCodes.Iqc => ToBaseRow(await _iqcRepository.FirstAsync(x => x.EcCode == ecCode && x.IsDeleted == 0)),
            TaktEcDeptCodes.Mc => ToBaseRow(await _mcRepository.FirstAsync(x => x.EcCode == ecCode && x.IsDeleted == 0)),
            TaktEcDeptCodes.Smt => ToBaseRow(await _smtRepository.FirstAsync(x => x.EcCode == ecCode && x.IsDeleted == 0)),
            TaktEcDeptCodes.Pcba => await FirstSmtBaseByEcCodeAsync(ecCode),
            TaktEcDeptCodes.Assy => ToBaseRow(await _assyRepository.FirstAsync(x => x.EcCode == ecCode && x.IsDeleted == 0)),
            TaktEcDeptCodes.Qa => ToBaseRow(await _qaRepository.FirstAsync(x => x.EcCode == ecCode && x.IsDeleted == 0)),
            TaktEcDeptCodes.Te => ToBaseRow(await _teRepository.FirstAsync(x => x.EcCode == ecCode && x.IsDeleted == 0)),
            _ => null
        };
    }

    /// <summary>
    /// 按明细 ID 与部门取执行记录（公共字段）
    /// </summary>
    /// <param name="ecDetailId">设变明细 ID</param>
    /// <param name="deptCode">部门编码</param>
    /// <returns>公共字段快照；不存在时 null</returns>
    public async Task<TaktEcExecBaseRow?> FirstBaseByDetailAndDeptAsync(long ecDetailId, string deptCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(deptCode);
        return deptCode switch
        {
            TaktEcDeptCodes.Pmc => ToBaseRow(await _pmcRepository.FirstAsync(x => x.EcDetailId == ecDetailId && x.IsDeleted == 0)),
            TaktEcDeptCodes.Mp => ToBaseRow(await _mpRepository.FirstAsync(x => x.EcDetailId == ecDetailId && x.IsDeleted == 0)),
            TaktEcDeptCodes.Iqc => ToBaseRow(await _iqcRepository.FirstAsync(x => x.EcDetailId == ecDetailId && x.IsDeleted == 0)),
            TaktEcDeptCodes.Mc => ToBaseRow(await _mcRepository.FirstAsync(x => x.EcDetailId == ecDetailId && x.IsDeleted == 0)),
            TaktEcDeptCodes.Smt => ToBaseRow(await _smtRepository.FirstAsync(x => x.EcDetailId == ecDetailId && x.IsDeleted == 0)),
            TaktEcDeptCodes.Pcba => await FirstSmtBaseByDetailIdAsync(ecDetailId),
            TaktEcDeptCodes.Assy => ToBaseRow(await _assyRepository.FirstAsync(x => x.EcDetailId == ecDetailId && x.IsDeleted == 0)),
            TaktEcDeptCodes.Qa => ToBaseRow(await _qaRepository.FirstAsync(x => x.EcDetailId == ecDetailId && x.IsDeleted == 0)),
            TaktEcDeptCodes.Te => ToBaseRow(await _teRepository.FirstAsync(x => x.EcDetailId == ecDetailId && x.IsDeleted == 0)),
            _ => null
        };
    }

    /// <summary>
    /// IN 分批上限（避免 SqlSugar 展开超大 Contains 导致 SQL Server「查询处理器用尽内部资源」）
    /// </summary>
    private const int EcDetailIdInBatchSize = 500;

    /// <summary>
    /// 按设变单号聚合全部部门执行行（公共字段；等式条件，适合十万级明细）
    /// </summary>
    /// <param name="ecCode">设变单号</param>
    /// <returns>执行行列表</returns>
    public async Task<List<TaktEcExecBaseRow>> ListBaseByEcCodeAsync(string ecCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ecCode);
        var code = ecCode.Trim();
        var rows = new List<TaktEcExecBaseRow>();
        rows.AddRange((await _pmcRepository.GetListAsync(x => x.EcCode == code && x.IsDeleted == 0)).Select(ToBaseRow).Where(x => x != null)!);
        rows.AddRange((await _mpRepository.GetListAsync(x => x.EcCode == code && x.IsDeleted == 0)).Select(ToBaseRow).Where(x => x != null)!);
        rows.AddRange((await _iqcRepository.GetListAsync(x => x.EcCode == code && x.IsDeleted == 0)).Select(ToBaseRow).Where(x => x != null)!);
        rows.AddRange((await _mcRepository.GetListAsync(x => x.EcCode == code && x.IsDeleted == 0)).Select(ToBaseRow).Where(x => x != null)!);
        rows.AddRange((await _smtRepository.GetListAsync(x => x.EcCode == code && x.IsDeleted == 0)).Select(ToBaseRow).Where(x => x != null)!);
        rows.AddRange((await _seizounikaRepository.GetListAsync(x => x.EcCode == code && x.IsDeleted == 0)).Select(ToBaseRow).Where(x => x != null)!);
        rows.AddRange((await _assyRepository.GetListAsync(x => x.EcCode == code && x.IsDeleted == 0)).Select(ToBaseRow).Where(x => x != null)!);
        rows.AddRange((await _qaRepository.GetListAsync(x => x.EcCode == code && x.IsDeleted == 0)).Select(ToBaseRow).Where(x => x != null)!);
        rows.AddRange((await _teRepository.GetListAsync(x => x.EcCode == code && x.IsDeleted == 0)).Select(ToBaseRow).Where(x => x != null)!);
        return rows;
    }

    /// <summary>
    /// 设变单号下是否存在任一部门已输入（实施=是或执行内容非空）；仅查首行，不拉全表
    /// </summary>
    /// <param name="ecCode">设变单号</param>
    /// <returns>存在已输入行时 true</returns>
    public async Task<bool> ExistsAnyDeptInputByEcCodeAsync(string ecCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ecCode);
        var code = ecCode.Trim();
        if (await _pmcRepository.FirstAsync(x =>
                x.EcCode == code && x.IsDeleted == 0 && x.IsObsolete == 0
                && (x.IsImplemented == 1 || (x.ExecContent != null && x.ExecContent != ""))) != null)
        {
            return true;
        }
        if (await _mpRepository.FirstAsync(x =>
                x.EcCode == code && x.IsDeleted == 0 && x.IsObsolete == 0
                && (x.IsImplemented == 1 || (x.ExecContent != null && x.ExecContent != ""))) != null)
        {
            return true;
        }
        if (await _iqcRepository.FirstAsync(x =>
                x.EcCode == code && x.IsDeleted == 0 && x.IsObsolete == 0
                && (x.IsImplemented == 1 || (x.ExecContent != null && x.ExecContent != ""))) != null)
        {
            return true;
        }
        if (await _mcRepository.FirstAsync(x =>
                x.EcCode == code && x.IsDeleted == 0 && x.IsObsolete == 0
                && (x.IsImplemented == 1 || (x.ExecContent != null && x.ExecContent != ""))) != null)
        {
            return true;
        }
        if (await _smtRepository.FirstAsync(x =>
                x.EcCode == code && x.IsDeleted == 0 && x.IsObsolete == 0
                && (x.IsImplemented == 1 || (x.ExecContent != null && x.ExecContent != ""))) != null)
        {
            return true;
        }
        if (await _seizounikaRepository.FirstAsync(x =>
                x.EcCode == code && x.IsDeleted == 0 && x.IsObsolete == 0
                && (x.IsImplemented == 1 || (x.ExecContent != null && x.ExecContent != ""))) != null)
        {
            return true;
        }
        if (await _assyRepository.FirstAsync(x =>
                x.EcCode == code && x.IsDeleted == 0 && x.IsObsolete == 0
                && (x.IsImplemented == 1 || (x.ExecContent != null && x.ExecContent != ""))) != null)
        {
            return true;
        }
        if (await _qaRepository.FirstAsync(x =>
                x.EcCode == code && x.IsDeleted == 0 && x.IsObsolete == 0
                && (x.IsImplemented == 1 || (x.ExecContent != null && x.ExecContent != ""))) != null)
        {
            return true;
        }
        return await _teRepository.FirstAsync(x =>
                x.EcCode == code && x.IsDeleted == 0 && x.IsObsolete == 0
                && (x.IsImplemented == 1 || (x.ExecContent != null && x.ExecContent != ""))) != null;
    }

    /// <summary>
    /// 按明细 ID 列表聚合全部部门执行行（公共字段；超大批次分片 IN，避免查询计划资源耗尽）
    /// </summary>
    /// <param name="detailIds">明细 ID 列表</param>
    /// <returns>执行行列表</returns>
    public async Task<List<TaktEcExecBaseRow>> ListBaseByEcDetailIdsAsync(IReadOnlyList<long> detailIds)
    {
        if (detailIds.Count == 0)
        {
            return [];
        }
        if (detailIds.Count <= EcDetailIdInBatchSize)
        {
            return await ListBaseByEcDetailIdsChunkAsync(detailIds);
        }
        var rows = new List<TaktEcExecBaseRow>();
        for (var offset = 0; offset < detailIds.Count; offset = checked(offset + EcDetailIdInBatchSize))
        {
            var take = Math.Min(EcDetailIdInBatchSize, detailIds.Count - offset);
            var chunk = detailIds.Skip(offset).Take(take).ToList();
            rows.AddRange(await ListBaseByEcDetailIdsChunkAsync(chunk));
        }
        return rows;
    }

    /// <summary>
    /// 单批明细 ID 聚合部门执行行
    /// </summary>
    /// <param name="detailIds">单批明细 ID（建议 ≤ EcDetailIdInBatchSize）</param>
    /// <returns>执行行列表</returns>
    private async Task<List<TaktEcExecBaseRow>> ListBaseByEcDetailIdsChunkAsync(IReadOnlyList<long> detailIds)
    {
        var rows = new List<TaktEcExecBaseRow>();
        rows.AddRange((await _pmcRepository.GetListAsync(x => detailIds.Contains(x.EcDetailId) && x.IsDeleted == 0)).Select(ToBaseRow).Where(x => x != null)!);
        rows.AddRange((await _mpRepository.GetListAsync(x => detailIds.Contains(x.EcDetailId) && x.IsDeleted == 0)).Select(ToBaseRow).Where(x => x != null)!);
        rows.AddRange((await _iqcRepository.GetListAsync(x => detailIds.Contains(x.EcDetailId) && x.IsDeleted == 0)).Select(ToBaseRow).Where(x => x != null)!);
        rows.AddRange((await _mcRepository.GetListAsync(x => detailIds.Contains(x.EcDetailId) && x.IsDeleted == 0)).Select(ToBaseRow).Where(x => x != null)!);
        rows.AddRange((await _smtRepository.GetListAsync(x => detailIds.Contains(x.EcDetailId) && x.IsDeleted == 0)).Select(ToBaseRow).Where(x => x != null)!);
        rows.AddRange((await _seizounikaRepository.GetListAsync(x => detailIds.Contains(x.EcDetailId) && x.IsDeleted == 0)).Select(ToBaseRow).Where(x => x != null)!);
        rows.AddRange((await _assyRepository.GetListAsync(x => detailIds.Contains(x.EcDetailId) && x.IsDeleted == 0)).Select(ToBaseRow).Where(x => x != null)!);
        rows.AddRange((await _qaRepository.GetListAsync(x => detailIds.Contains(x.EcDetailId) && x.IsDeleted == 0)).Select(ToBaseRow).Where(x => x != null)!);
        rows.AddRange((await _teRepository.GetListAsync(x => detailIds.Contains(x.EcDetailId) && x.IsDeleted == 0)).Select(ToBaseRow).Where(x => x != null)!);
        return rows;
    }

    /// <summary>
    /// 按明细 ID 聚合全部部门执行行（公共字段）
    /// </summary>
    /// <param name="ecDetailId">明细 ID</param>
    /// <returns>执行行列表</returns>
    public Task<List<TaktEcExecBaseRow>> ListBaseByEcDetailIdAsync(long ecDetailId) =>
        ListBaseByEcDetailIdsAsync([ecDetailId]);

    /// <summary>
    /// 取明细下全部部门执行行的最大行号
    /// </summary>
    /// <param name="ecDetailId">明细 ID</param>
    /// <returns>最大行号；无记录时为 0</returns>
    public async Task<int> GetMaxLineNumberForDetailAsync(long ecDetailId)
    {
        var rows = await ListBaseByEcDetailIdAsync(ecDetailId);
        return rows.Count == 0 ? 0 : rows.Max(x => x.LineNumber);
    }

    /// <summary>
    /// 按明细 ID 删除全部部门执行行
    /// </summary>
    /// <param name="ecDetailId">明细 ID</param>
    /// <returns>任务</returns>
    public async Task DeleteAllByEcDetailIdAsync(long ecDetailId)
    {
        await _pmcRepository.DeleteAsync(x => x.EcDetailId == ecDetailId);
        await _mpRepository.DeleteAsync(x => x.EcDetailId == ecDetailId);
        await _iqcRepository.DeleteAsync(x => x.EcDetailId == ecDetailId);
        await _mcRepository.DeleteAsync(x => x.EcDetailId == ecDetailId);
        await _smtRepository.DeleteAsync(x => x.EcDetailId == ecDetailId);
        await _seizounikaRepository.DeleteAsync(x => x.EcDetailId == ecDetailId);
        await _assyRepository.DeleteAsync(x => x.EcDetailId == ecDetailId);
        await _qaRepository.DeleteAsync(x => x.EcDetailId == ecDetailId);
        await _teRepository.DeleteAsync(x => x.EcDetailId == ecDetailId);
    }

    /// <summary>
    /// 按 ID 与部门编码取最大行号（单表）
    /// </summary>
    /// <param name="ecDetailId">明细 ID</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司代码</param>
    /// <returns>最大行号</returns>
    public Task<int> GetMaxLineNumberForDetailDeptAsync(
        long ecDetailId,
        string deptCode,
        string tenantCode,
        string companyCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(deptCode);
        return deptCode switch
        {
            TaktEcDeptCodes.Pmc => _pmcRepository.GetMaxIntAsync(
                x => x.TenantCode == tenantCode && x.CompanyCode == companyCode && x.EcDetailId == ecDetailId,
                x => x.LineNumber),
            TaktEcDeptCodes.Mp => _mpRepository.GetMaxIntAsync(
                x => x.TenantCode == tenantCode && x.CompanyCode == companyCode && x.EcDetailId == ecDetailId,
                x => x.LineNumber),
            TaktEcDeptCodes.Iqc => _iqcRepository.GetMaxIntAsync(
                x => x.TenantCode == tenantCode && x.CompanyCode == companyCode && x.EcDetailId == ecDetailId,
                x => x.LineNumber),
            TaktEcDeptCodes.Mc => _mcRepository.GetMaxIntAsync(
                x => x.TenantCode == tenantCode && x.CompanyCode == companyCode && x.EcDetailId == ecDetailId,
                x => x.LineNumber),
            TaktEcDeptCodes.Smt => _smtRepository.GetMaxIntAsync(
                x => x.TenantCode == tenantCode && x.CompanyCode == companyCode && x.EcDetailId == ecDetailId,
                x => x.LineNumber),
            TaktEcDeptCodes.Pcba => GetMaxLineNumberForPcbaAsync(ecDetailId, tenantCode, companyCode),
            TaktEcDeptCodes.Assy => _assyRepository.GetMaxIntAsync(
                x => x.TenantCode == tenantCode && x.CompanyCode == companyCode && x.EcDetailId == ecDetailId,
                x => x.LineNumber),
            TaktEcDeptCodes.Qa => _qaRepository.GetMaxIntAsync(
                x => x.TenantCode == tenantCode && x.CompanyCode == companyCode && x.EcDetailId == ecDetailId,
                x => x.LineNumber),
            TaktEcDeptCodes.Te => _teRepository.GetMaxIntAsync(
                x => x.TenantCode == tenantCode && x.CompanyCode == companyCode && x.EcDetailId == ecDetailId,
                x => x.LineNumber),
            _ => Task.FromResult(0)
        };
    }

    /// <summary>
    /// 按设变单号取制造二课公共字段（优先未作废电子料件，再制二非 F）
    /// </summary>
    private async Task<TaktEcExecBaseRow?> FirstSmtBaseByEcCodeAsync(string ecCode)
    {
        var electronic = await _smtRepository.FirstAsync(x =>
            x.EcCode == ecCode && x.IsDeleted == 0 && x.IsObsolete == 0);
        if (electronic != null)
        {
            return ToBaseRow(electronic);
        }
        var seizounika = await _seizounikaRepository.FirstAsync(x =>
            x.EcCode == ecCode && x.IsDeleted == 0 && x.IsObsolete == 0);
        if (seizounika != null)
        {
            return ToBaseRow(seizounika);
        }
        return ToBaseRow(await _smtRepository.FirstAsync(x => x.EcCode == ecCode && x.IsDeleted == 0))
            ?? ToBaseRow(await _seizounikaRepository.FirstAsync(x => x.EcCode == ecCode && x.IsDeleted == 0));
    }

    /// <summary>
    /// 按明细 ID 取制造二课公共字段（优先未作废电子料件，再制二非 F）
    /// </summary>
    private async Task<TaktEcExecBaseRow?> FirstSmtBaseByDetailIdAsync(long ecDetailId)
    {
        var electronic = await _smtRepository.FirstAsync(x =>
            x.EcDetailId == ecDetailId && x.IsDeleted == 0 && x.IsObsolete == 0);
        if (electronic != null)
        {
            return ToBaseRow(electronic);
        }
        var seizounika = await _seizounikaRepository.FirstAsync(x =>
            x.EcDetailId == ecDetailId && x.IsDeleted == 0 && x.IsObsolete == 0);
        if (seizounika != null)
        {
            return ToBaseRow(seizounika);
        }
        return ToBaseRow(await _smtRepository.FirstAsync(x => x.EcDetailId == ecDetailId && x.IsDeleted == 0))
            ?? ToBaseRow(await _seizounikaRepository.FirstAsync(x => x.EcDetailId == ecDetailId && x.IsDeleted == 0));
    }

    /// <summary>
    /// 制造二课双表最大行号
    /// </summary>
    private async Task<int> GetMaxLineNumberForPcbaAsync(long ecDetailId, string tenantCode, string companyCode)
    {
        var electronicMax = await _smtRepository.GetMaxIntAsync(
            x => x.TenantCode == tenantCode && x.CompanyCode == companyCode && x.EcDetailId == ecDetailId,
            x => x.LineNumber);
        var seizounikaMax = await _seizounikaRepository.GetMaxIntAsync(
            x => x.TenantCode == tenantCode && x.CompanyCode == companyCode && x.EcDetailId == ecDetailId,
            x => x.LineNumber);
        return Math.Max(electronicMax, seizounikaMax);
    }

    /// <summary>
    /// 映射公共字段
    /// </summary>
    /// <param name="entity">执行实体</param>
    /// <returns>公共字段快照</returns>
    private static TaktEcExecBaseRow? ToBaseRow(object? entity) => entity switch
    {
        TaktEcSeikan e => new TaktEcExecBaseRow
        {
            Id = e.Id,
            EcDetailId = e.EcDetailId,
            EcCode = e.EcCode,
            DeptCode = e.DeptCode,
            LineNumber = e.LineNumber,
            IsImplemented = e.IsImplemented,
            ExecContent = e.ExecContent,
            IsObsolete = e.IsObsolete,
            TenantCode = e.TenantCode,
            CompanyCode = e.CompanyCode,
        },
        TaktEcKoubai e => new TaktEcExecBaseRow
        {
            Id = e.Id,
            EcDetailId = e.EcDetailId,
            EcCode = e.EcCode,
            DeptCode = e.DeptCode,
            LineNumber = e.LineNumber,
            IsImplemented = e.IsImplemented,
            ExecContent = e.ExecContent,
            IsObsolete = e.IsObsolete,
            TenantCode = e.TenantCode,
            CompanyCode = e.CompanyCode,
        },
        TaktEcUkeken e => new TaktEcExecBaseRow
        {
            Id = e.Id,
            EcDetailId = e.EcDetailId,
            EcCode = e.EcCode,
            DeptCode = e.DeptCode,
            LineNumber = e.LineNumber,
            IsImplemented = e.IsImplemented,
            ExecContent = e.ExecContent,
            IsObsolete = e.IsObsolete,
            TenantCode = e.TenantCode,
            CompanyCode = e.CompanyCode,
        },
        TaktEcBukan e => new TaktEcExecBaseRow
        {
            Id = e.Id,
            EcDetailId = e.EcDetailId,
            EcCode = e.EcCode,
            DeptCode = e.DeptCode,
            LineNumber = e.LineNumber,
            IsImplemented = e.IsImplemented,
            ExecContent = e.ExecContent,
            IsObsolete = e.IsObsolete,
            TenantCode = e.TenantCode,
            CompanyCode = e.CompanyCode,
        },
        TaktEcSmt e => new TaktEcExecBaseRow
        {
            Id = e.Id,
            EcDetailId = e.EcDetailId,
            EcCode = e.EcCode,
            DeptCode = e.DeptCode,
            LineNumber = e.LineNumber,
            IsImplemented = e.IsImplemented,
            ExecContent = e.ExecContent,
            IsObsolete = e.IsObsolete,
            TenantCode = e.TenantCode,
            CompanyCode = e.CompanyCode,
        },
        TaktEcSeizounika e => new TaktEcExecBaseRow
        {
            Id = e.Id,
            EcDetailId = e.EcDetailId,
            EcCode = e.EcCode,
            DeptCode = e.DeptCode,
            LineNumber = e.LineNumber,
            IsImplemented = e.IsImplemented,
            ExecContent = e.ExecContent,
            IsObsolete = e.IsObsolete,
            TenantCode = e.TenantCode,
            CompanyCode = e.CompanyCode,
        },
        TaktEcSeizouikka e => new TaktEcExecBaseRow
        {
            Id = e.Id,
            EcDetailId = e.EcDetailId,
            EcCode = e.EcCode,
            DeptCode = e.DeptCode,
            LineNumber = e.LineNumber,
            IsImplemented = e.IsImplemented,
            ExecContent = e.ExecContent,
            IsObsolete = e.IsObsolete,
            TenantCode = e.TenantCode,
            CompanyCode = e.CompanyCode,
        },
        TaktEcHinkan e => new TaktEcExecBaseRow
        {
            Id = e.Id,
            EcDetailId = e.EcDetailId,
            EcCode = e.EcCode,
            DeptCode = e.DeptCode,
            LineNumber = e.LineNumber,
            IsImplemented = e.IsImplemented,
            ExecContent = e.ExecContent,
            IsObsolete = e.IsObsolete,
            TenantCode = e.TenantCode,
            CompanyCode = e.CompanyCode,
        },
        TaktEcSeizougijutsu e => new TaktEcExecBaseRow
        {
            Id = e.Id,
            EcDetailId = e.EcDetailId,
            EcCode = e.EcCode,
            DeptCode = e.DeptCode,
            LineNumber = e.LineNumber,
            IsImplemented = e.IsImplemented,
            ExecContent = e.ExecContent,
            IsObsolete = e.IsObsolete,
            TenantCode = e.TenantCode,
            CompanyCode = e.CompanyCode,
        },
        _ => null
    };
}
