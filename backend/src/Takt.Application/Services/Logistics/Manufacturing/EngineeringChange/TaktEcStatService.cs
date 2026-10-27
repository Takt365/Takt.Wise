// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcStatService.cs
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：设变统计。各执行部门实施表经技术主表关联，按发行日期或录入日期列出该表字段
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Takt.Application.Services;
using Takt.Domain.Entities;
using Takt.Domain.Interfaces;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变统计
/// </summary>
public class TaktEcStatService : TaktServiceBase, ITaktEcStatService
{
    private const string TabPur = "pur";
    private const string TabPc = "pc";
    private const string TabIqc = "iqc";
    private const string TabMc = "mc";
    private const string TabMtl = "mtl";
    private const string TabMfg2 = "mfg2";
    private const string TabMfg1 = "mfg1";
    private const string TabQc = "qc";
    private const string TabMe = "me";
    private const string StatusAll = "all";
    private const string StatusDone = "done";
    private const string StatusOpen = "open";
    private const int MaxRows = 100000;

    private readonly ISqlSugarClient _db;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="db">SqlSugar 客户端</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化</param>
    public TaktEcStatService(
        ISqlSugarClient db,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _db = db;
    }

    /// <inheritdoc />
    public Task<List<TaktSelectOption>> GetEcStatPlantOptionsAsync(string? statTab, string? keyword = null)
    {
        return DistinctAsync(statTab, null, keyword, ecCodes: false);
    }

    /// <inheritdoc />
    public Task<List<TaktSelectOption>> GetEcStatEcOptionsAsync(string? statTab, string? plantCode, string? keyword = null)
    {
        return DistinctAsync(statTab, plantCode, keyword, ecCodes: true);
    }

    /// <inheritdoc />
    public async Task<TaktEcDeptReportResultDto> GetEcStatListAsync(TaktEcDeptReportQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var (rows, total) = await LoadPageAsync(queryDto, skip, pageSize);
        return new TaktEcDeptReportResultDto
        {
            Paged = TaktPagedResult<TaktEcDeptReportRowDto>.Create(rows, total, pageIndex, pageSize),
            RowCount = total,
        };
    }

    /// <inheritdoc />
    public async Task<(string fileName, byte[] fileContent)> ExportEcStatAsync(
        TaktEcDeptReportQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        var (rows, total) = await LoadPageAsync(query, 0, MaxRows);
        if (total > MaxRows)
        {
            ThrowBusinessException($"统计结果超过 {MaxRows} 行，请缩小期间或指定设变单号后再导出");
        }
        if (!TryParseTab(query.StatTab, out var tab))
        {
            ThrowBusinessException("请选择执行部门");
        }
        var (columnKeys, columnLabels) = ExportColumns(tab);
        var exportRows = rows.Select(ToExportMap).ToList();
        return await TaktExcelHelper.ExportDictionaryRowsAsync(
            exportRows,
            columnKeys,
            columnLabels,
            sheetName ?? "设变统计",
            fileName ?? TaktNamingHelper.PlantPrefixedTrendExportBase(query.PlantCode, "设变统计"));
    }

    /// <summary>
    /// 按当前执行部门实施表分页。发行日期、录入日期取技术主表
    /// </summary>
    /// <param name="queryDto">查询</param>
    /// <param name="skip">跳过行数</param>
    /// <param name="take">本页行数</param>
    /// <returns>本页行与总行数</returns>
    private async Task<(List<TaktEcDeptReportRowDto> Rows, int Total)> LoadPageAsync(
        TaktEcDeptReportQueryDto queryDto,
        int skip,
        int take)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
        if (!TryParseTab(queryDto.StatTab, out var tab))
        {
            ThrowBusinessException("请选择执行部门");
        }
        var status = NormalizeStatus(queryDto.ImplStatus);
        var hasIssue = TryResolveMonthRange(queryDto.PeriodDateStart, queryDto.PeriodDateEnd, out var issueStart, out var issueEndExclusive);
        var hasEntry = TryResolveMonthRange(queryDto.EntryPeriodDateStart, queryDto.EntryPeriodDateEnd, out var entryStart, out var entryEndExclusive);
        if (hasIssue && hasEntry)
        {
            ThrowBusinessException("发行期间与录入期间只能选择一个");
        }
        if (!hasIssue && !hasEntry)
        {
            ThrowBusinessException("请选择发行期间或录入期间");
        }
        EnsureThreeLayerContext();
        var plant = queryDto.PlantCode.Trim();
        var ecCode = BlankToNull(queryDto.EcCode);
        var keywords = BlankToNull(queryDto.KeyWords);
        DateTime? issueFrom = hasIssue ? issueStart : null;
        DateTime? issueTo = hasIssue ? issueEndExclusive : null;
        DateTime? entryFrom = hasEntry ? entryStart : null;
        DateTime? entryTo = hasEntry ? entryEndExclusive : null;
        return tab switch
        {
            TabPur => await PagePurAsync(plant, issueFrom, issueTo, entryFrom, entryTo, ecCode, keywords, status, skip, take),
            TabPc => await PagePcAsync(plant, issueFrom, issueTo, entryFrom, entryTo, ecCode, keywords, status, skip, take),
            TabIqc => await PageIqcAsync(plant, issueFrom, issueTo, entryFrom, entryTo, ecCode, keywords, status, skip, take),
            TabMc => await PageMcAsync(plant, issueFrom, issueTo, entryFrom, entryTo, ecCode, keywords, status, skip, take),
            TabMtl => await PageMtlAsync(plant, issueFrom, issueTo, entryFrom, entryTo, ecCode, keywords, status, skip, take),
            TabMfg2 => await PageMfg2Async(plant, issueFrom, issueTo, entryFrom, entryTo, ecCode, keywords, status, skip, take),
            TabMfg1 => await PageMfg1Async(plant, issueFrom, issueTo, entryFrom, entryTo, ecCode, keywords, status, skip, take),
            TabQc => await PageQcAsync(plant, issueFrom, issueTo, entryFrom, entryTo, ecCode, keywords, status, skip, take),
            _ => await PageMeAsync(plant, issueFrom, issueTo, entryFrom, entryTo, ecCode, keywords, status, skip, take),
        };
    }

    /// <summary>
    /// 采购课实施行。列：机种编码组、新物料编码、实施、采购订单发行日期
    /// </summary>
    private Task<(List<TaktEcDeptReportRowDto> Rows, int Total)> PagePurAsync(
        string plant, DateTime? issueStart, DateTime? issueEndExclusive, DateTime? entryStart, DateTime? entryEndExclusive, string? ecCode, string? keywords, string status, int skip, int take)
    {
        var query = WhereImplemented(JoinMaster<TaktEcPur>(plant, issueStart, issueEndExclusive, entryStart, entryEndExclusive, ecCode, keywords), status);
        return ReadPageAsync(query, (x, g) => new StatLine
        {
            Id = x.Id,
            EcCode = x.EcCode,
            EcModelCodeGroup = x.EcModelCodeGroup,
            EcNewMaterialCode = x.EcNewMaterialCode,
            IsImplemented = x.IsImplemented,
            PurchaseOrderIssueDate = x.PurchaseOrderIssueDate,
        }, skip, take);
    }

    /// <summary>
    /// 生管课实施行。列：机种编码组、根物料编码组、实施、预定日期、预定批次
    /// </summary>
    private Task<(List<TaktEcDeptReportRowDto> Rows, int Total)> PagePcAsync(
        string plant, DateTime? issueStart, DateTime? issueEndExclusive, DateTime? entryStart, DateTime? entryEndExclusive, string? ecCode, string? keywords, string status, int skip, int take)
    {
        var query = WhereImplemented(JoinMaster<TaktEcPc>(plant, issueStart, issueEndExclusive, entryStart, entryEndExclusive, ecCode, keywords), status);
        return ReadPageAsync(query, (x, g) => new StatLine
        {
            Id = x.Id,
            EcCode = x.EcCode,
            EcModelCodeGroup = x.EcModelCodeGroup,
            EcRootMaterialCodeGroup = x.EcRootMaterialCodeGroup,
            IsImplemented = x.IsImplemented,
            ScheduledDate = x.ScheduledDate,
            ScheduledBatch = x.ScheduledBatch,
        }, skip, take);
    }

    /// <summary>
    /// 受检课实施行。列：机种编码组、根物料编码组、新物料编码、新品检验、实施、检验日期、受检单号
    /// </summary>
    private Task<(List<TaktEcDeptReportRowDto> Rows, int Total)> PageIqcAsync(
        string plant, DateTime? issueStart, DateTime? issueEndExclusive, DateTime? entryStart, DateTime? entryEndExclusive, string? ecCode, string? keywords, string status, int skip, int take)
    {
        var query = WhereImplemented(JoinMaster<TaktEcIqc>(plant, issueStart, issueEndExclusive, entryStart, entryEndExclusive, ecCode, keywords), status);
        return ReadPageAsync(query, (x, g) => new StatLine
        {
            Id = x.Id,
            EcCode = x.EcCode,
            EcModelCodeGroup = x.EcModelCodeGroup,
            EcRootMaterialCodeGroup = x.EcRootMaterialCodeGroup,
            EcNewMaterialCode = x.EcNewMaterialCode,
            EcNewRequiresInspection = x.EcNewRequiresInspection,
            IsImplemented = x.IsImplemented,
            InspectionDate = x.InspectionDate,
            IqcOrderCode = x.IqcOrderCode,
        }, skip, take);
    }

    /// <summary>
    /// 部管课实施行。列：机种编码组、根物料编码组、新物料编码、新品仓库、实施、预定日期、预定批次
    /// </summary>
    private Task<(List<TaktEcDeptReportRowDto> Rows, int Total)> PageMcAsync(
        string plant, DateTime? issueStart, DateTime? issueEndExclusive, DateTime? entryStart, DateTime? entryEndExclusive, string? ecCode, string? keywords, string status, int skip, int take)
    {
        var query = WhereImplemented(JoinMaster<TaktEcMc>(plant, issueStart, issueEndExclusive, entryStart, entryEndExclusive, ecCode, keywords), status);
        return ReadPageAsync(query, (x, g) => new StatLine
        {
            Id = x.Id,
            EcCode = x.EcCode,
            EcModelCodeGroup = x.EcModelCodeGroup,
            EcRootMaterialCodeGroup = x.EcRootMaterialCodeGroup,
            EcNewMaterialCode = x.EcNewMaterialCode,
            EcNewWarehouse = x.EcNewWarehouse,
            IsImplemented = x.IsImplemented,
            ScheduledDate = x.ScheduledDate,
            ScheduledBatch = x.ScheduledBatch,
        }, skip, take);
    }

    /// <summary>
    /// 物料实施行。列与部管课相同
    /// </summary>
    private Task<(List<TaktEcDeptReportRowDto> Rows, int Total)> PageMtlAsync(
        string plant, DateTime? issueStart, DateTime? issueEndExclusive, DateTime? entryStart, DateTime? entryEndExclusive, string? ecCode, string? keywords, string status, int skip, int take)
    {
        var query = WhereImplemented(JoinMaster<TaktEcMtl>(plant, issueStart, issueEndExclusive, entryStart, entryEndExclusive, ecCode, keywords), status);
        return ReadPageAsync(query, (x, g) => new StatLine
        {
            Id = x.Id,
            EcCode = x.EcCode,
            EcModelCodeGroup = x.EcModelCodeGroup,
            EcRootMaterialCodeGroup = x.EcRootMaterialCodeGroup,
            EcNewMaterialCode = x.EcNewMaterialCode,
            EcNewWarehouse = x.EcNewWarehouse,
            IsImplemented = x.IsImplemented,
            ScheduledDate = x.ScheduledDate,
            ScheduledBatch = x.ScheduledBatch,
        }, skip, take);
    }

    /// <summary>
    /// 制造二课实施行。列：机种编码组、根物料编码组、实施、生产日期、实施批次
    /// </summary>
    private Task<(List<TaktEcDeptReportRowDto> Rows, int Total)> PageMfg2Async(
        string plant, DateTime? issueStart, DateTime? issueEndExclusive, DateTime? entryStart, DateTime? entryEndExclusive, string? ecCode, string? keywords, string status, int skip, int take)
    {
        var query = WhereImplemented(JoinMaster<TaktEcMfg2>(plant, issueStart, issueEndExclusive, entryStart, entryEndExclusive, ecCode, keywords), status);
        return ReadPageAsync(query, (x, g) => new StatLine
        {
            Id = x.Id,
            EcCode = x.EcCode,
            EcModelCodeGroup = x.EcModelCodeGroup,
            EcRootMaterialCodeGroup = x.EcRootMaterialCodeGroup,
            IsImplemented = x.IsImplemented,
            ProductionDate = x.ProductionDate,
            ImplementationBatch = x.ImplementationBatch,
        }, skip, take);
    }

    /// <summary>
    /// 制造一课实施行。列与制造二课相同
    /// </summary>
    private Task<(List<TaktEcDeptReportRowDto> Rows, int Total)> PageMfg1Async(
        string plant, DateTime? issueStart, DateTime? issueEndExclusive, DateTime? entryStart, DateTime? entryEndExclusive, string? ecCode, string? keywords, string status, int skip, int take)
    {
        var query = WhereImplemented(JoinMaster<TaktEcMfg1>(plant, issueStart, issueEndExclusive, entryStart, entryEndExclusive, ecCode, keywords), status);
        return ReadPageAsync(query, (x, g) => new StatLine
        {
            Id = x.Id,
            EcCode = x.EcCode,
            EcModelCodeGroup = x.EcModelCodeGroup,
            EcRootMaterialCodeGroup = x.EcRootMaterialCodeGroup,
            IsImplemented = x.IsImplemented,
            ProductionDate = x.ProductionDate,
            ImplementationBatch = x.ImplementationBatch,
        }, skip, take);
    }

    /// <summary>
    /// 品管课实施行。列：机种编码组、根物料编码组、实施、检验日期、检验批次
    /// </summary>
    private Task<(List<TaktEcDeptReportRowDto> Rows, int Total)> PageQcAsync(
        string plant, DateTime? issueStart, DateTime? issueEndExclusive, DateTime? entryStart, DateTime? entryEndExclusive, string? ecCode, string? keywords, string status, int skip, int take)
    {
        var query = WhereImplemented(JoinMaster<TaktEcQc>(plant, issueStart, issueEndExclusive, entryStart, entryEndExclusive, ecCode, keywords), status);
        return ReadPageAsync(query, (x, g) => new StatLine
        {
            Id = x.Id,
            EcCode = x.EcCode,
            EcModelCodeGroup = x.EcModelCodeGroup,
            EcRootMaterialCodeGroup = x.EcRootMaterialCodeGroup,
            IsImplemented = x.IsImplemented,
            InspectionDate = x.InspectionDate,
            InspectionBatch = x.InspectionBatch,
        }, skip, take);
    }

    /// <summary>
    /// 制造技术课实施行。列：机种编码组、根物料编码组、更新 SOP、SOP 日期
    /// </summary>
    private Task<(List<TaktEcDeptReportRowDto> Rows, int Total)> PageMeAsync(
        string plant, DateTime? issueStart, DateTime? issueEndExclusive, DateTime? entryStart, DateTime? entryEndExclusive, string? ecCode, string? keywords, string status, int skip, int take)
    {
        var query = JoinMaster<TaktEcMe>(plant, issueStart, issueEndExclusive, entryStart, entryEndExclusive, ecCode, keywords);
        if (status == StatusDone)
        {
            query = query.Where((x, g) => x.IsSopUpdated == 1);
        }
        else if (status == StatusOpen)
        {
            query = query.Where((x, g) => x.IsSopUpdated != 1);
        }
        return ReadPageAsync(query, (x, g) => new StatLine
        {
            Id = x.Id,
            EcCode = x.EcCode,
            EcModelCodeGroup = x.EcModelCodeGroup,
            EcRootMaterialCodeGroup = x.EcRootMaterialCodeGroup,
            IsSopUpdated = x.IsSopUpdated,
            SopDate = x.SopDate,
        }, skip, take);
    }

    /// <summary>
    /// 执行部门实施表内连接技术主表，并套上工厂、期间、设变单号与关键词
    /// </summary>
    /// <typeparam name="T">执行部门实体</typeparam>
    /// <param name="plant">工厂</param>
    /// <param name="issueStart">发行期间起</param>
    /// <param name="issueEndExclusive">发行期间止（不含）</param>
    /// <param name="entryStart">录入期间起</param>
    /// <param name="entryEndExclusive">录入期间止（不含）</param>
    /// <param name="ecCode">设变单号</param>
    /// <param name="keywords">关键词（设变单号、标题、担当）</param>
    /// <returns>查询</returns>
    private ISugarQueryable<T, TaktEcTe> JoinMaster<T>(
        string plant,
        DateTime? issueStart,
        DateTime? issueEndExclusive,
        DateTime? entryStart,
        DateTime? entryEndExclusive,
        string? ecCode,
        string? keywords)
        where T : TaktCompanyEntityBase, ITaktEcDeptExecEntity, new()
    {
        var tenant = CurrentTenantCode;
        var company = CurrentCompanyCode;
        var query = _db.Queryable<T>()
            .InnerJoin<TaktEcTe>((x, g) => x.TenantCode == g.TenantCode && x.CompanyCode == g.CompanyCode && x.PlantCode == g.PlantCode && x.EcCode == g.EcCode)
            .Where((x, g) => x.TenantCode == tenant && x.CompanyCode == company && x.PlantCode == plant && x.IsDeleted == 0 && x.IsObsolete == 0 && g.IsDeleted == 0);
        if (issueStart.HasValue && issueEndExclusive.HasValue)
        {
            var from = issueStart.Value;
            var to = issueEndExclusive.Value;
            query = query.Where((x, g) => g.EcIssueDate >= from && g.EcIssueDate < to);
        }
        if (entryStart.HasValue && entryEndExclusive.HasValue)
        {
            var from = entryStart.Value;
            var to = entryEndExclusive.Value;
            query = query.Where((x, g) => g.EcEntryDate >= from && g.EcEntryDate < to);
        }
        if (!string.IsNullOrEmpty(ecCode))
        {
            query = query.Where((x, g) => x.EcCode == ecCode);
        }
        if (!string.IsNullOrEmpty(keywords))
        {
            var text = keywords;
            query = query.Where((x, g) => g.EcCode.Contains(text) || g.EcTitle.Contains(text) || g.EcLeader.Contains(text));
        }
        return query;
    }

    /// <summary>
    /// 按实施过滤。全部时不附加条件
    /// </summary>
    /// <typeparam name="T">执行部门实体</typeparam>
    /// <param name="query">查询</param>
    /// <param name="status">all、done 或 open</param>
    /// <returns>查询</returns>
    private static ISugarQueryable<T, TaktEcTe> WhereImplemented<T>(ISugarQueryable<T, TaktEcTe> query, string status)
        where T : TaktCompanyEntityBase, ITaktEcDeptExecEntity, new()
    {
        if (status == StatusDone)
        {
            return query.Where((x, g) => x.IsImplemented == 1);
        }
        if (status == StatusOpen)
        {
            return query.Where((x, g) => x.IsImplemented != 1);
        }
        return query;
    }

    /// <summary>
    /// 计数后按设变单号、主键分页，并转成展示行
    /// </summary>
    /// <typeparam name="T">执行部门实体</typeparam>
    /// <param name="query">已过滤的查询</param>
    /// <param name="selector">投影到中间行</param>
    /// <param name="skip">跳过行数</param>
    /// <param name="take">本页行数</param>
    /// <returns>本页行与总行数</returns>
    private static async Task<(List<TaktEcDeptReportRowDto> Rows, int Total)> ReadPageAsync<T>(
        ISugarQueryable<T, TaktEcTe> query,
        Expression<Func<T, TaktEcTe, StatLine>> selector,
        int skip,
        int take)
        where T : TaktCompanyEntityBase, ITaktEcDeptExecEntity, new()
    {
        var total = await query.CountAsync();
        if (take <= 0 || total == 0 || skip >= total)
        {
            return ([], total);
        }
        var lines = await query
            .Select(selector)
            .OrderBy(x => x.EcCode)
            .OrderBy(x => x.Id)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
        return (lines.Select(ToRow).ToList(), total);
    }

    /// <summary>
    /// 把查询上的年月收成左闭右开的月区间
    /// </summary>
    /// <param name="startIn">期间起</param>
    /// <param name="endIn">期间止</param>
    /// <param name="start">月首日</param>
    /// <param name="endExclusive">结束月的下月首日</param>
    /// <returns>起止都有值时为 true</returns>
    private static bool TryResolveMonthRange(DateTime? startIn, DateTime? endIn, out DateTime start, out DateTime endExclusive)
    {
        if (!startIn.HasValue || !endIn.HasValue)
        {
            start = default;
            endExclusive = default;
            return false;
        }
        start = new DateTime(startIn.Value.Year, startIn.Value.Month, 1);
        var endMonth = new DateTime(endIn.Value.Year, endIn.Value.Month, 1);
        if (endMonth < start)
        {
            (start, endMonth) = (endMonth, start);
        }
        endExclusive = endMonth.AddMonths(1);
        return true;
    }

    /// <summary>
    /// 当前部门页签的工厂或设变单号
    /// </summary>
    /// <param name="statTab">部门页签</param>
    /// <param name="plantCode">工厂</param>
    /// <param name="keyword">关键字</param>
    /// <param name="ecCodes">true 取设变单号</param>
    /// <returns>选项</returns>
    private async Task<List<TaktSelectOption>> DistinctAsync(string? statTab, string? plantCode, string? keyword, bool ecCodes)
    {
        if (!TryParseTab(statTab, out var tab))
        {
            return [];
        }
        EnsureThreeLayerContext();
        var plant = BlankToNull(plantCode);
        var codes = tab switch
        {
            TabPur => await DistinctCodesAsync<TaktEcPur>(plant, ecCodes),
            TabPc => await DistinctCodesAsync<TaktEcPc>(plant, ecCodes),
            TabIqc => await DistinctCodesAsync<TaktEcIqc>(plant, ecCodes),
            TabMc => await DistinctCodesAsync<TaktEcMc>(plant, ecCodes),
            TabMtl => await DistinctCodesAsync<TaktEcMtl>(plant, ecCodes),
            TabMfg2 => await DistinctCodesAsync<TaktEcMfg2>(plant, ecCodes),
            TabMfg1 => await DistinctCodesAsync<TaktEcMfg1>(plant, ecCodes),
            TabQc => await DistinctCodesAsync<TaktEcQc>(plant, ecCodes),
            _ => await DistinctTeCodesAsync(plant, ecCodes),
        };
        return ToOptions(codes, keyword);
    }

    /// <summary>
    /// 未作废执行行的工厂或设变单号
    /// </summary>
    /// <typeparam name="T">执行部门实体</typeparam>
    /// <param name="plant">工厂</param>
    /// <param name="ecCodes">true 取设变单号</param>
    /// <returns>编码</returns>
    private async Task<List<string>> DistinctCodesAsync<T>(string? plant, bool ecCodes)
        where T : TaktCompanyEntityBase, ITaktEcDeptExecEntity, new()
    {
        var tenant = CurrentTenantCode;
        var company = CurrentCompanyCode;
        var query = _db.Queryable<T>()
            .Where(x => x.TenantCode == tenant && x.CompanyCode == company && x.IsDeleted == 0 && x.IsObsolete == 0);
        if (!string.IsNullOrEmpty(plant))
        {
            query = query.Where(x => x.PlantCode == plant);
        }
        if (ecCodes)
        {
            return await query.GroupBy(x => x.EcCode).Select(x => x.EcCode).ToListAsync();
        }
        return await query.GroupBy(x => x.PlantCode).Select(x => x.PlantCode).ToListAsync();
    }

    /// <summary>
    /// 制技未作废行的工厂或设变单号
    /// </summary>
    /// <param name="plant">工厂</param>
    /// <param name="ecCodes">true 取设变单号</param>
    /// <returns>编码</returns>
    private async Task<List<string>> DistinctTeCodesAsync(string? plant, bool ecCodes)
    {
        var tenant = CurrentTenantCode;
        var company = CurrentCompanyCode;
        var query = _db.Queryable<TaktEcMe>()
            .Where(x => x.TenantCode == tenant && x.CompanyCode == company && x.IsDeleted == 0 && x.IsObsolete == 0);
        if (!string.IsNullOrEmpty(plant))
        {
            query = query.Where(x => x.PlantCode == plant);
        }
        if (ecCodes)
        {
            return await query.GroupBy(x => x.EcCode).Select(x => x.EcCode).ToListAsync();
        }
        return await query.GroupBy(x => x.PlantCode).Select(x => x.PlantCode).ToListAsync();
    }

    /// <summary>
    /// 中间行转展示行。日期写成 yyyy-MM-dd
    /// </summary>
    /// <param name="line">查询行</param>
    /// <returns>展示行</returns>
    private static TaktEcDeptReportRowDto ToRow(StatLine line)
    {
        return new TaktEcDeptReportRowDto
        {
            Id = line.Id.ToString(),
            EcCode = line.EcCode ?? string.Empty,
            EcModelCodeGroup = line.EcModelCodeGroup,
            EcRootMaterialCodeGroup = line.EcRootMaterialCodeGroup,
            EcNewMaterialCode = line.EcNewMaterialCode,
            EcNewWarehouse = line.EcNewWarehouse,
            EcNewRequiresInspection = line.EcNewRequiresInspection,
            IsImplemented = line.IsImplemented,
            IsSopUpdated = line.IsSopUpdated,
            ScheduledDate = FormatDate(line.ScheduledDate),
            ScheduledBatch = line.ScheduledBatch,
            ProductionDate = FormatDate(line.ProductionDate),
            ImplementationBatch = line.ImplementationBatch,
            InspectionDate = FormatDate(line.InspectionDate),
            InspectionBatch = line.InspectionBatch,
            PurchaseOrderIssueDate = FormatDate(line.PurchaseOrderIssueDate),
            IqcOrderCode = line.IqcOrderCode,
            SopDate = FormatDate(line.SopDate),
        };
    }

    /// <summary>
    /// 当前部门页签的导出列。标题与实施表列备注一致
    /// </summary>
    /// <param name="tab">部门页签</param>
    /// <returns>列键与标题</returns>
    private static (List<string> Keys, List<string> Labels) ExportColumns(string tab)
    {
        return tab switch
        {
            TabPur => (
                ["ecCode", "ecModelCodeGroup", "ecNewMaterialCode", "isImplemented", "purchaseOrderIssueDate"],
                ["设变单号", "机种编码组", "新物料编码", "实施", "采购订单发行日期"]),
            TabPc => (
                ["ecCode", "ecModelCodeGroup", "ecRootMaterialCodeGroup", "isImplemented", "scheduledDate", "scheduledBatch"],
                ["设变单号", "机种编码组", "根物料编码组", "实施", "预定日期", "预定批次"]),
            TabIqc => (
                ["ecCode", "ecModelCodeGroup", "ecRootMaterialCodeGroup", "ecNewMaterialCode", "ecNewRequiresInspection", "isImplemented", "inspectionDate", "iqcOrderCode"],
                ["设变单号", "机种编码组", "根物料编码组", "新物料编码", "新品检验", "实施", "检验日期", "受检单号"]),
            TabMc or TabMtl => (
                ["ecCode", "ecModelCodeGroup", "ecRootMaterialCodeGroup", "ecNewMaterialCode", "ecNewWarehouse", "isImplemented", "scheduledDate", "scheduledBatch"],
                ["设变单号", "机种编码组", "根物料编码组", "新物料编码", "新品仓库", "实施", "预定日期", "预定批次"]),
            TabMfg2 or TabMfg1 => (
                ["ecCode", "ecModelCodeGroup", "ecRootMaterialCodeGroup", "isImplemented", "productionDate", "implementationBatch"],
                ["设变单号", "机种编码组", "根物料编码组", "实施", "生产日期", "实施批次"]),
            TabQc => (
                ["ecCode", "ecModelCodeGroup", "ecRootMaterialCodeGroup", "isImplemented", "inspectionDate", "inspectionBatch"],
                ["设变单号", "机种编码组", "根物料编码组", "实施", "检验日期", "检验批次"]),
            _ => (
                ["ecCode", "ecModelCodeGroup", "ecRootMaterialCodeGroup", "isSopUpdated", "sopDate"],
                ["设变单号", "机种编码组", "根物料编码组", "更新SOP", "SOP日期"]),
        };
    }

    /// <summary>
    /// 导出行。是否类字段写成是或否
    /// </summary>
    /// <param name="row">展示行</param>
    /// <returns>列字典</returns>
    private static IReadOnlyDictionary<string, object?> ToExportMap(TaktEcDeptReportRowDto row)
    {
        return new Dictionary<string, object?>
        {
            ["ecCode"] = row.EcCode,
            ["ecModelCodeGroup"] = row.EcModelCodeGroup,
            ["ecRootMaterialCodeGroup"] = row.EcRootMaterialCodeGroup,
            ["ecNewMaterialCode"] = row.EcNewMaterialCode,
            ["ecNewWarehouse"] = row.EcNewWarehouse,
            ["ecNewRequiresInspection"] = YesNo(row.EcNewRequiresInspection),
            ["isImplemented"] = YesNo(row.IsImplemented),
            ["isSopUpdated"] = YesNo(row.IsSopUpdated),
            ["scheduledDate"] = row.ScheduledDate,
            ["scheduledBatch"] = row.ScheduledBatch,
            ["productionDate"] = row.ProductionDate,
            ["implementationBatch"] = row.ImplementationBatch,
            ["inspectionDate"] = row.InspectionDate,
            ["inspectionBatch"] = row.InspectionBatch,
            ["purchaseOrderIssueDate"] = row.PurchaseOrderIssueDate,
            ["iqcOrderCode"] = row.IqcOrderCode,
            ["sopDate"] = row.SopDate,
        };
    }

    /// <summary>
    /// 日期格式 yyyy-MM-dd
    /// </summary>
    /// <param name="value">日期</param>
    /// <returns>文本。空日期返回 null</returns>
    private static string? FormatDate(DateTime? value)
    {
        return value?.ToString("yyyy-MM-dd");
    }

    /// <summary>
    /// 字典 sys_yes_no 的导出文本
    /// </summary>
    /// <param name="value">0 或 1</param>
    /// <returns>否、是，或空</returns>
    private static string YesNo(int? value)
    {
        return value switch
        {
            1 => "是",
            0 => "否",
            _ => string.Empty,
        };
    }

    /// <summary>
    /// 选项
    /// </summary>
    /// <param name="codes">编码</param>
    /// <param name="keyword">关键字</param>
    /// <returns>选项</returns>
    private static List<TaktSelectOption> ToOptions(IEnumerable<string> codes, string? keyword)
    {
        var kw = keyword?.Trim();
        return codes
            .Select(c => c?.Trim() ?? string.Empty)
            .Where(c => c.Length > 0)
            .Where(c => string.IsNullOrEmpty(kw) || c.Contains(kw, StringComparison.OrdinalIgnoreCase))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(c => c, StringComparer.Ordinal)
            .Select(c => new TaktSelectOption { DictValue = c, DictLabel = c })
            .ToList();
    }

    /// <summary>
    /// 解析部门页签
    /// </summary>
    /// <param name="statTab">页签</param>
    /// <param name="tab">规范化页签</param>
    /// <returns>是否有效</returns>
    private static bool TryParseTab(string? statTab, out string tab)
    {
        tab = statTab?.Trim().ToLowerInvariant() ?? string.Empty;
        return tab is TabPur or TabPc or TabIqc or TabMc or TabMtl or TabMfg2 or TabMfg1 or TabQc or TabMe;
    }

    /// <summary>
    /// 实施页签。空值视为全部
    /// </summary>
    /// <param name="implStatus">页签</param>
    /// <returns>all、done 或 open</returns>
    private string NormalizeStatus(string? implStatus)
    {
        var status = implStatus?.Trim().ToLowerInvariant();
        if (string.IsNullOrEmpty(status) || status == StatusAll)
        {
            return StatusAll;
        }
        if (status is StatusDone or StatusOpen)
        {
            return status;
        }
        ThrowBusinessException("请选择实施页签");
        return StatusAll;
    }

    /// <summary>
    /// 空白转 null
    /// </summary>
    /// <param name="value">原值</param>
    /// <returns>去空白后的值</returns>
    private static string? BlankToNull(string? value)
    {
        var text = value?.Trim();
        return string.IsNullOrEmpty(text) ? null : text;
    }

    /// <summary>
    /// 实施表查询中间行。日期在转展示行时格式化
    /// </summary>
    private sealed class StatLine
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 设变单号
        /// </summary>
        public string EcCode { get; set; } = string.Empty;

        /// <summary>
        /// 机种编码组
        /// </summary>
        public string? EcModelCodeGroup { get; set; }

        /// <summary>
        /// 根物料编码组
        /// </summary>
        public string? EcRootMaterialCodeGroup { get; set; }

        /// <summary>
        /// 新物料编码
        /// </summary>
        public string? EcNewMaterialCode { get; set; }

        /// <summary>
        /// 新品仓库
        /// </summary>
        public string? EcNewWarehouse { get; set; }

        /// <summary>
        /// 新品检验
        /// </summary>
        public int? EcNewRequiresInspection { get; set; }

        /// <summary>
        /// 实施
        /// </summary>
        public int? IsImplemented { get; set; }

        /// <summary>
        /// 更新 SOP
        /// </summary>
        public int? IsSopUpdated { get; set; }

        /// <summary>
        /// 预定日期
        /// </summary>
        public DateTime? ScheduledDate { get; set; }

        /// <summary>
        /// 预定批次
        /// </summary>
        public string? ScheduledBatch { get; set; }

        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime? ProductionDate { get; set; }

        /// <summary>
        /// 实施批次
        /// </summary>
        public string? ImplementationBatch { get; set; }

        /// <summary>
        /// 检验日期
        /// </summary>
        public DateTime? InspectionDate { get; set; }

        /// <summary>
        /// 检验批次
        /// </summary>
        public string? InspectionBatch { get; set; }

        /// <summary>
        /// 采购订单发行日期
        /// </summary>
        public DateTime? PurchaseOrderIssueDate { get; set; }

        /// <summary>
        /// 受检单号
        /// </summary>
        public string? IqcOrderCode { get; set; }

        /// <summary>
        /// SOP 日期
        /// </summary>
        public DateTime? SopDate { get; set; }
    }
}
