// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Defect
// 文件名称：TaktPcbaOutputInspectionRepairSyncHelper.cs
// 创建时间：2026-07-06
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA日报产出与工作中心含 SMT/修正 时自动 upsert 检查/改修日报及明细
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.Logistics.Manufacturing.Defect;
using Takt.Domain.Entities.Logistics.Manufacturing.Output;
using Takt.Domain.Repositories;

namespace Takt.Application.Services.Logistics.Manufacturing.Defect;

/// <summary>
/// PCBA 产出 → 检查日报（SMT 工作中心）/ 改修日报（修正工作中心）级联同步辅助
/// </summary>
internal static class TaktPcbaOutputInspectionRepairSyncHelper
{
    private const string SyncExtFieldPrefix = "takt-pcba-out:";

    /// <summary>
    /// 判断工作中心是否含 SMT（不区分大小写）
    /// </summary>
    /// <param name="workCenter">工作中心 / 生产时段</param>
    /// <returns>含 SMT 为 true</returns>
    public static bool IsSmtWorkCenter(string? workCenter)
    {
        return !string.IsNullOrWhiteSpace(workCenter)
            && workCenter.Contains("SMT", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 判断工作中心是否含修正
    /// </summary>
    /// <param name="workCenter">工作中心 / 生产时段</param>
    /// <returns>含修正为 true</returns>
    public static bool IsRepairWorkCenter(string? workCenter)
    {
        return !string.IsNullOrWhiteSpace(workCenter)
            && workCenter.Contains("修正", StringComparison.Ordinal);
    }

    /// <summary>
    /// PCBA 日报保存后同步检查/改修日报
    /// </summary>
    /// <param name="pcbaOutputRepository">PCBA 日报仓储</param>
    /// <param name="pcbaOutputDetailRepository">PCBA 日报明细仓储</param>
    /// <param name="pcbaInspectionRepository">PCBA 检查日报仓储</param>
    /// <param name="pcbaInspectionDetailRepository">PCBA 检查明细仓储</param>
    /// <param name="pcbaRepairRepository">PCBA 改修日报仓储</param>
    /// <param name="pcbaRepairDetailRepository">PCBA 改修明细仓储</param>
    /// <param name="output">PCBA 日报主表</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <returns>任务</returns>
    public static async Task SyncFromPcbaOutputAsync(
        ITaktCompanyRepository<TaktPcbaOutput> pcbaOutputRepository,
        ITaktCompanyRepository<TaktPcbaOutputDetail> pcbaOutputDetailRepository,
        ITaktCompanyRepository<TaktPcbaInspection> pcbaInspectionRepository,
        ITaktCompanyRepository<TaktPcbaInspectionDetail> pcbaInspectionDetailRepository,
        ITaktCompanyRepository<TaktPcbaRepair> pcbaRepairRepository,
        ITaktCompanyRepository<TaktPcbaRepairDetail> pcbaRepairDetailRepository,
        TaktPcbaOutput output,
        string tenantCode,
        string companyCode)
    {
        ArgumentNullException.ThrowIfNull(output);
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(companyCode);
        await RefreshInspectionForKeyAsync(
            pcbaOutputRepository,
            pcbaOutputDetailRepository,
            pcbaInspectionRepository,
            pcbaInspectionDetailRepository,
            tenantCode,
            companyCode,
            output.PlantCode,
            output.ProdCategory,
            output.ProdDate,
            output.ProdOrderCode);
        await SyncRepairForOutputAsync(
            pcbaOutputDetailRepository,
            pcbaRepairRepository,
            pcbaRepairDetailRepository,
            output,
            tenantCode,
            companyCode);
    }

    /// <summary>
    /// 按检查日报自然键刷新（聚合同键全部产出的 SMT 明细）
    /// </summary>
    /// <param name="pcbaOutputRepository">PCBA 日报仓储</param>
    /// <param name="pcbaOutputDetailRepository">PCBA 日报明细仓储</param>
    /// <param name="pcbaInspectionRepository">PCBA 检查日报仓储</param>
    /// <param name="pcbaInspectionDetailRepository">PCBA 检查明细仓储</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="prodCategory">生产类别</param>
    /// <param name="prodDate">生产日期</param>
    /// <param name="prodOrderCode">工单号</param>
    /// <param name="excludeOutputId">排除的产出 Id（删除产出时传入）</param>
    /// <returns>任务</returns>
    public static async Task RefreshInspectionForNaturalKeyAsync(
        ITaktCompanyRepository<TaktPcbaOutput> pcbaOutputRepository,
        ITaktCompanyRepository<TaktPcbaOutputDetail> pcbaOutputDetailRepository,
        ITaktCompanyRepository<TaktPcbaInspection> pcbaInspectionRepository,
        ITaktCompanyRepository<TaktPcbaInspectionDetail> pcbaInspectionDetailRepository,
        string tenantCode,
        string companyCode,
        string plantCode,
        string prodCategory,
        DateTime prodDate,
        string prodOrderCode,
        long excludeOutputId = 0)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(companyCode);
        await RefreshInspectionForKeyAsync(
            pcbaOutputRepository,
            pcbaOutputDetailRepository,
            pcbaInspectionRepository,
            pcbaInspectionDetailRepository,
            tenantCode,
            companyCode,
            plantCode,
            prodCategory,
            prodDate,
            prodOrderCode,
            excludeOutputId);
    }

    /// <summary>
    /// 删除与产出自然键对齐的改修日报
    /// </summary>
    /// <param name="pcbaRepairRepository">PCBA 改修日报仓储</param>
    /// <param name="pcbaRepairDetailRepository">PCBA 改修明细仓储</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="prodCategory">生产类别</param>
    /// <param name="prodDate">生产日期</param>
    /// <param name="TeamCode">生产班组</param>
    /// <param name="shiftNo">班次</param>
    /// <param name="prodOrderCode">工单号</param>
    /// <returns>任务</returns>
    public static Task DeleteRepairForNaturalKeyAsync(
        ITaktCompanyRepository<TaktPcbaRepair> pcbaRepairRepository,
        ITaktCompanyRepository<TaktPcbaRepairDetail> pcbaRepairDetailRepository,
        string tenantCode,
        string companyCode,
        string plantCode,
        string prodCategory,
        DateTime prodDate,
        string TeamCode,
        int shiftNo,
        string prodOrderCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(companyCode);
        return DeleteRepairForOutputKeyAsync(
            pcbaRepairRepository,
            pcbaRepairDetailRepository,
            tenantCode,
            companyCode,
            plantCode,
            prodCategory,
            prodDate,
            TeamCode,
            shiftNo,
            prodOrderCode);
    }

    /// <summary>
    /// 按检查日报自然键刷新（聚合同键全部产出的 SMT 明细）
    /// </summary>
    private static async Task RefreshInspectionForKeyAsync(
        ITaktCompanyRepository<TaktPcbaOutput> pcbaOutputRepository,
        ITaktCompanyRepository<TaktPcbaOutputDetail> pcbaOutputDetailRepository,
        ITaktCompanyRepository<TaktPcbaInspection> pcbaInspectionRepository,
        ITaktCompanyRepository<TaktPcbaInspectionDetail> pcbaInspectionDetailRepository,
        string tenantCode,
        string companyCode,
        string plantCode,
        string prodCategory,
        DateTime prodDate,
        string prodOrderCode,
        long excludeOutputId = 0)
    {
        var outputs = await pcbaOutputRepository.GetListAsync(x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.ProdDate == prodDate.Date
            && x.ProdOrderCode == prodOrderCode
            && (excludeOutputId <= 0 || x.Id != excludeOutputId));
        var smtLines = new List<(TaktPcbaOutput Output, TaktPcbaOutputDetail Detail)>();
        foreach (var output in outputs.OrderBy(x => x.Id))
        {
            var details = await pcbaOutputDetailRepository.GetListAsync(x => x.PcbaOutputId == output.Id);
            foreach (var detail in details.Where(d => IsSmtWorkCenter(d.TimePeriod)))
            {
                smtLines.Add((output, detail));
            }
        }
        var inspection = await FindInspectionByOrderAsync(
            pcbaInspectionRepository,
            tenantCode,
            companyCode,
            prodOrderCode);
        if (smtLines.Count == 0)
        {
            if (inspection == null)
            {
                return;
            }
            var outputIds = outputs.Select(o => o.Id).ToHashSet();
            var existingDetails = await pcbaInspectionDetailRepository.GetListAsync(x => x.PcbaInspectionId == inspection.Id);
            foreach (var detail in existingDetails)
            {
                if (TryParseSyncExtField(detail.ExtField, out var outputId, out _)
                    && outputIds.Contains(outputId))
                {
                    await pcbaInspectionDetailRepository.DeleteAsync(detail.Id);
                }
            }
            var remaining = await pcbaInspectionDetailRepository.GetListAsync(x => x.PcbaInspectionId == inspection.Id);
            if (remaining.Count == 0)
            {
                await pcbaInspectionRepository.DeleteAsync(inspection.Id);
            }
            return;
        }
        var headerSource = outputs.OrderByDescending(x => x.Id).First();
        if (inspection == null)
        {
            inspection = new TaktPcbaInspection
            {
                TenantCode = tenantCode,
                CompanyCode = companyCode,
            };
            CopyOutputHeaderToInspection(headerSource, inspection);
            inspection = await pcbaInspectionRepository.CreateAsync(inspection);
        }
        else
        {
            CopyOutputHeaderToInspection(headerSource, inspection);
            await pcbaInspectionRepository.UpdateAsync(inspection);
        }
        await SyncInspectionDetailsAsync(
            pcbaInspectionDetailRepository,
            inspection,
            smtLines,
            tenantCode,
            companyCode);
    }

    /// <summary>
    /// 按产出自然键同步改修日报（仅本产出 修正 工作中心明细）
    /// </summary>
    private static async Task SyncRepairForOutputAsync(
        ITaktCompanyRepository<TaktPcbaOutputDetail> pcbaOutputDetailRepository,
        ITaktCompanyRepository<TaktPcbaRepair> pcbaRepairRepository,
        ITaktCompanyRepository<TaktPcbaRepairDetail> pcbaRepairDetailRepository,
        TaktPcbaOutput output,
        string tenantCode,
        string companyCode)
    {
        var outputDetails = await pcbaOutputDetailRepository.GetListAsync(x => x.PcbaOutputId == output.Id);
        var repairLines = outputDetails.Where(d => IsRepairWorkCenter(d.TimePeriod)).ToList();
        var repair = await FindRepairByDailyOrderAsync(
            pcbaRepairRepository,
            tenantCode,
            companyCode,
            output.ProdDate,
            output.ProdOrderCode);
        if (repairLines.Count == 0)
        {
            if (repair != null)
            {
                await pcbaRepairDetailRepository.DeleteAsync(x => x.PcbaRepairId == repair.Id);
                await pcbaRepairRepository.DeleteAsync(repair.Id);
            }
            return;
        }
        if (repair == null)
        {
            repair = new TaktPcbaRepair
            {
                TenantCode = tenantCode,
                CompanyCode = companyCode,
            };
            CopyOutputHeaderToRepair(output, repair, repairLines.FirstOrDefault());
            repair = await pcbaRepairRepository.CreateAsync(repair);
        }
        else
        {
            CopyOutputHeaderToRepair(output, repair, repairLines.FirstOrDefault());
            await pcbaRepairRepository.UpdateAsync(repair);
        }
        await SyncRepairDetailsAsync(
            pcbaRepairDetailRepository,
            repair,
            output,
            repairLines,
            tenantCode,
            companyCode);
    }

    /// <summary>
    /// 删除与产出自然键对齐的改修日报
    /// </summary>
    private static async Task DeleteRepairForOutputKeyAsync(
        ITaktCompanyRepository<TaktPcbaRepair> pcbaRepairRepository,
        ITaktCompanyRepository<TaktPcbaRepairDetail> pcbaRepairDetailRepository,
        string tenantCode,
        string companyCode,
        string plantCode,
        string prodCategory,
        DateTime prodDate,
        string TeamCode,
        int shiftNo,
        string prodOrderCode)
    {
        var repair = await FindRepairByDailyOrderAsync(
            pcbaRepairRepository,
            tenantCode,
            companyCode,
            prodDate,
            prodOrderCode);
        if (repair == null)
        {
            return;
        }
        await pcbaRepairDetailRepository.DeleteAsync(x => x.PcbaRepairId == repair.Id);
        await pcbaRepairRepository.DeleteAsync(repair.Id);
    }

    /// <summary>
    /// 同步检查明细（ExtField 关联产出明细行）
    /// </summary>
    private static async Task SyncInspectionDetailsAsync(
        ITaktCompanyRepository<TaktPcbaInspectionDetail> pcbaInspectionDetailRepository,
        TaktPcbaInspection inspection,
        IReadOnlyList<(TaktPcbaOutput Output, TaktPcbaOutputDetail Detail)> smtLines,
        string tenantCode,
        string companyCode)
    {
        var existingDetails = await pcbaInspectionDetailRepository.GetListAsync(x => x.PcbaInspectionId == inspection.Id);
        var existingBySyncKey = existingDetails
            .Where(d => TryParseSyncExtField(d.ExtField, out _, out _))
            .ToDictionary(d => d.ExtField!, d => d, StringComparer.Ordinal);
        var matchedIds = new HashSet<long>();
        var nextLine = existingDetails.Count == 0 ? 10 : existingDetails.Max(x => x.LineNumber) + 10;
        foreach (var (output, detail) in smtLines)
        {
            var syncKey = BuildSyncExtField(output.Id, detail.LineNumber);
            if (!existingBySyncKey.TryGetValue(syncKey, out var inspectionDetail))
            {
                inspectionDetail = new TaktPcbaInspectionDetail
                {
                    TenantCode = tenantCode,
                    CompanyCode = companyCode,
                    PcbaInspectionId = inspection.Id,
                    ProdOrderCode = inspection.ProdOrderCode,
                    LineNumber = nextLine,
                    ExtField = syncKey,
                };
                nextLine += 10;
            }
            ApplyOutputDetailToInspectionDetail(output, detail, inspectionDetail);
            inspectionDetail.ExtField = syncKey;
            if (inspectionDetail.Id > 0)
            {
                await pcbaInspectionDetailRepository.UpdateAsync(inspectionDetail);
            }
            else
            {
                inspectionDetail = await pcbaInspectionDetailRepository.CreateAsync(inspectionDetail);
                existingBySyncKey[syncKey] = inspectionDetail;
            }
            matchedIds.Add(inspectionDetail.Id);
        }
        foreach (var orphan in existingDetails.Where(d => TryParseSyncExtField(d.ExtField, out _, out _) && !matchedIds.Contains(d.Id)))
        {
            await pcbaInspectionDetailRepository.DeleteAsync(orphan.Id);
        }
    }

    /// <summary>
    /// 同步改修明细（ExtField 关联产出明细行）
    /// </summary>
    private static async Task SyncRepairDetailsAsync(
        ITaktCompanyRepository<TaktPcbaRepairDetail> pcbaRepairDetailRepository,
        TaktPcbaRepair repair,
        TaktPcbaOutput output,
        IReadOnlyList<TaktPcbaOutputDetail> repairLines,
        string tenantCode,
        string companyCode)
    {
        var existingDetails = await pcbaRepairDetailRepository.GetListAsync(x => x.PcbaRepairId == repair.Id);
        var existingBySyncKey = existingDetails
            .Where(d => TryParseSyncExtField(d.ExtField, out _, out _))
            .ToDictionary(d => d.ExtField!, d => d, StringComparer.Ordinal);
        var matchedIds = new HashSet<long>();
        var nextLine = existingDetails.Count == 0 ? 10 : existingDetails.Max(x => x.LineNumber) + 10;
        foreach (var detail in repairLines)
        {
            var syncKey = BuildSyncExtField(output.Id, detail.LineNumber);
            if (!existingBySyncKey.TryGetValue(syncKey, out var repairDetail))
            {
                repairDetail = new TaktPcbaRepairDetail
                {
                    TenantCode = tenantCode,
                    CompanyCode = companyCode,
                    PcbaRepairId = repair.Id,
                    ProdOrderCode = repair.ProdOrderCode,
                    LineNumber = nextLine,
                    ExtField = syncKey,
                };
                nextLine += 10;
            }
            ApplyOutputDetailToRepairDetail(output, detail, repairDetail);
            repairDetail.ExtField = syncKey;
            if (repairDetail.Id > 0)
            {
                await pcbaRepairDetailRepository.UpdateAsync(repairDetail);
            }
            else
            {
                repairDetail = await pcbaRepairDetailRepository.CreateAsync(repairDetail);
                existingBySyncKey[syncKey] = repairDetail;
            }
            matchedIds.Add(repairDetail.Id);
        }
        foreach (var orphan in existingDetails.Where(d => TryParseSyncExtField(d.ExtField, out _, out _) && !matchedIds.Contains(d.Id)))
        {
            await pcbaRepairDetailRepository.DeleteAsync(orphan.Id);
        }
    }

    /// <summary>
    /// 将产出明细字段写入检查明细（保留用户已填检查/不良字段）
    /// </summary>
    private static void ApplyOutputDetailToInspectionDetail(
        TaktPcbaOutput output,
        TaktPcbaOutputDetail detail,
        TaktPcbaInspectionDetail inspectionDetail)
    {
        inspectionDetail.ProdOrderCode = output.ProdOrderCode;
        inspectionDetail.PcbaBoardType = detail.PcbBoardType;
        inspectionDetail.ShiftNo = detail.ShiftNo;
        inspectionDetail.TeamCode = detail.TeamCode;
        inspectionDetail.DailyCompletedQty = detail.DailyCompletedQty;
        ApplyPanelSideAssemblyDates(output.ProdDate, detail.PanelSide, inspectionDetail);
    }

    /// <summary>
    /// 将产出明细字段写入改修明细（保留用户已填不良/责任字段）
    /// </summary>
    private static void ApplyOutputDetailToRepairDetail(
        TaktPcbaOutput output,
        TaktPcbaOutputDetail detail,
        TaktPcbaRepairDetail repairDetail)
    {
        repairDetail.ProdOrderCode = output.ProdOrderCode;
        repairDetail.PcbaBoardType = detail.PcbBoardType;
        repairDetail.TeamCode = detail.TeamCode;
        repairDetail.ProdActualQty = detail.DailyCompletedQty;
    }

    /// <summary>
    /// 按面板别写入 B/T 面实装日期
    /// </summary>
    private static void ApplyPanelSideAssemblyDates(
        DateTime prodDate,
        string panelSide,
        TaktPcbaInspectionDetail inspectionDetail)
    {
        var side = panelSide?.Trim() ?? string.Empty;
        if (side.Equals("b", StringComparison.OrdinalIgnoreCase))
        {
            inspectionDetail.BSideAssemblyDate = prodDate.Date;
            inspectionDetail.TSideAssemblyDate = null;
        }
        else if (side.Equals("t", StringComparison.OrdinalIgnoreCase))
        {
            inspectionDetail.TSideAssemblyDate = prodDate.Date;
            inspectionDetail.BSideAssemblyDate = null;
        }
    }

    /// <summary>
    /// 构建产出明细同步 ExtField
    /// </summary>
    private static string BuildSyncExtField(long pcbaOutputId, int outputLineNumber)
    {
        return $"{SyncExtFieldPrefix}{pcbaOutputId}:{outputLineNumber}";
    }

    /// <summary>
    /// 解析产出明细同步 ExtField
    /// </summary>
    private static bool TryParseSyncExtField(string? extField, out long pcbaOutputId, out int outputLineNumber)
    {
        pcbaOutputId = 0;
        outputLineNumber = 0;
        if (string.IsNullOrWhiteSpace(extField) || !extField.StartsWith(SyncExtFieldPrefix, StringComparison.Ordinal))
        {
            return false;
        }
        var body = extField[SyncExtFieldPrefix.Length..];
        var parts = body.Split(':', 2);
        if (parts.Length != 2)
        {
            return false;
        }
        return long.TryParse(parts[0], out pcbaOutputId) && int.TryParse(parts[1], out outputLineNumber);
    }

    /// <summary>
    /// 复制产出主表头到检查日报
    /// </summary>
    private static void CopyOutputHeaderToInspection(TaktPcbaOutput output, TaktPcbaInspection inspection)
    {
        inspection.PlantCode = output.PlantCode;
        inspection.ProdCategory = output.ProdCategory;
        inspection.ProdOrderType = output.ProdOrderType;
        inspection.ProdOrderCode = output.ProdOrderCode;
        inspection.ProdOrderQty = output.ProdOrderQty;
        inspection.ModelCode = output.ModelCode;
        inspection.BatchCode = output.BatchCode;
        inspection.MaterialCode = output.MaterialCode;
    }

    /// <summary>
    /// 复制产出主表头到改修日报（班组/班次取自首条修正明细）
    /// </summary>
    private static void CopyOutputHeaderToRepair(
        TaktPcbaOutput output,
        TaktPcbaRepair repair,
        TaktPcbaOutputDetail? headerDetail)
    {
        repair.PlantCode = output.PlantCode;
        repair.ProdCategory = output.ProdCategory;
        repair.ProdDate = output.ProdDate;
        repair.TeamCode = headerDetail?.TeamCode ?? string.Empty;
        repair.ShiftNo = headerDetail is { ShiftNo: > 0 } ? headerDetail.ShiftNo : 1;
        repair.ProdOrderType = output.ProdOrderType;
        repair.ProdOrderCode = output.ProdOrderCode;
        repair.ProdOrderQty = output.ProdOrderQty;
        repair.ModelCode = output.ModelCode;
        repair.BatchCode = output.BatchCode;
        repair.MaterialCode = output.MaterialCode;
    }

    /// <summary>
    /// 按工单号查找检查日报
    /// </summary>
    private static Task<TaktPcbaInspection?> FindInspectionByOrderAsync(
        ITaktCompanyRepository<TaktPcbaInspection> repository,
        string tenantCode,
        string companyCode,
        string prodOrderCode)
    {
        return repository.FirstAsync(x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.ProdOrderCode == prodOrderCode);
    }

    /// <summary>
    /// 按「生产日期 + 工单号」查找改修日报
    /// </summary>
    private static Task<TaktPcbaRepair?> FindRepairByDailyOrderAsync(
        ITaktCompanyRepository<TaktPcbaRepair> repository,
        string tenantCode,
        string companyCode,
        DateTime prodDate,
        string prodOrderCode)
    {
        var dateOnly = prodDate.Date;
        return repository.FirstAsync(x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.ProdDate == dateOnly
            && x.ProdOrderCode == prodOrderCode);
    }

    /// <summary>
    /// 判断检查日报自然键是否相同
    /// </summary>
    public static bool IsSameInspectionKey(TaktPcbaOutput before, TaktPcbaOutput after)
    {
        return before.PlantCode == after.PlantCode
            && before.ProdCategory == after.ProdCategory
            && before.ProdOrderCode == after.ProdOrderCode;
    }

    /// <summary>
    /// 判断改修日报自然键是否相同（与产出主表唯一键一致）
    /// </summary>
    public static bool IsSameRepairKey(TaktPcbaOutput before, TaktPcbaOutput after)
    {
        return before.PlantCode == after.PlantCode
            && before.ProdCategory == after.ProdCategory
            && before.ProdDate == after.ProdDate
            && before.ProdOrderCode == after.ProdOrderCode;
    }
}
