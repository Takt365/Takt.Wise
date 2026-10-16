// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcDeptViewMapper.cs
// 创建时间：2026-07-01
// 创建人：Takt365(Cursor AI)
// 功能描述：设变部门执行实体与部门视图 DTO 映射
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变部门视图 DTO 映射（直接读 8 张部门执行表实体）
/// </summary>
public static class TaktEcDeptViewMapper
{
    /// <summary>
    /// 将部门执行实体字段写入部门视图 DTO
    /// </summary>
    /// <param name="dto">视图 DTO</param>
    /// <param name="exec">部门执行实体</param>
    public static void ApplyToViewDto(TaktEcDeptViewDto dto, object exec)
    {
        switch (exec)
        {
            case TaktEcSeikan pmc:
                dto.EcExecId = pmc.Id;
                dto.IsImplemented = pmc.IsImplemented;
                dto.Content = pmc.ExecContent;
                dto.Remark = pmc.Remark;
                dto.ScheduledDate = pmc.ScheduledDate;
                dto.ScheduledBatch = pmc.ScheduledBatch;
                dto.PoRemainder = pmc.PoRemainder;
                dto.Balance = pmc.Balance;
                dto.OldProductHandling = pmc.EcOldPartDisposition;
                break;
            case TaktEcKoubai mp:
                dto.EcExecId = mp.Id;
                dto.IsImplemented = mp.IsImplemented;
                dto.Content = mp.ExecContent;
                dto.Remark = mp.Remark;
                dto.PurchaseOrderIssueDate = mp.PurchaseOrderIssueDate;
                dto.Supplier = mp.Supplier;
                dto.PurchaseOrderCode = mp.PurchaseOrderCode;
                dto.OldProductHandling = mp.EcOldPartDisposition;
                break;
            case TaktEcUkeken iqc:
                dto.EcExecId = iqc.Id;
                dto.IsImplemented = iqc.IsImplemented;
                dto.Content = iqc.ExecContent;
                dto.Remark = iqc.Remark;
                dto.IqcOrderCode = iqc.IqcOrderCode;
                dto.InspectionDate = iqc.InspectionDate;
                break;
            case TaktEcBukan mc:
                dto.EcExecId = mc.Id;
                dto.IsImplemented = mc.IsImplemented;
                dto.Content = mc.ExecContent;
                dto.Remark = mc.Remark;
                dto.OutboundBatch = mc.OutboundBatch;
                dto.OutboundDate = mc.OutboundDate;
                break;
            case TaktEcEmtl electronic:
                dto.EcExecId = electronic.Id;
                dto.IsImplemented = electronic.IsImplemented;
                dto.Content = electronic.ExecContent;
                dto.Remark = electronic.Remark;
                dto.OutboundBatch = electronic.OutboundBatch;
                dto.OutboundDate = electronic.OutboundDate;
                break;
            case TaktEcSeizounika pcba:
                dto.EcExecId = pcba.Id;
                dto.IsImplemented = pcba.IsImplemented;
                dto.Content = pcba.ExecContent;
                dto.Remark = pcba.Remark;
                dto.ProductionTeam = pcba.ProductionTeam;
                dto.ProductionDate = pcba.ProductionDate;
                dto.ImplementationBatch = pcba.ImplementationBatch;
                break;
            case TaktEcSeizouikka assy:
                dto.EcExecId = assy.Id;
                dto.IsImplemented = assy.IsImplemented;
                dto.Content = assy.ExecContent;
                dto.Remark = assy.Remark;
                dto.ProductionTeam = assy.ProductionTeam;
                dto.ProductionDate = assy.ProductionDate;
                dto.ImplementationBatch = assy.ImplementationBatch;
                break;
            case TaktEcHinkan qa:
                dto.EcExecId = qa.Id;
                dto.IsImplemented = qa.IsImplemented;
                dto.Content = qa.ExecContent;
                dto.Remark = qa.Remark;
                dto.ProductionTeam = qa.ProductionTeam;
                dto.InspectionDate = qa.InspectionDate;
                dto.InspectionBatch = qa.InspectionBatch;
                dto.SamplingCode = qa.SamplingCode;
                break;
            case TaktEcSeizougijutsu te:
                dto.EcExecId = te.Id;
                dto.IsImplemented = te.IsImplemented;
                dto.Content = te.ExecContent;
                dto.Remark = te.Remark;
                dto.SopDate = te.SopDate;
                dto.IsSopUpdated = te.IsSopUpdated;
                break;
        }
    }

    /// <summary>
    /// 合并导入行与已有实体为视图更新 DTO
    /// </summary>
    /// <param name="row">导入行</param>
    /// <param name="ecDetailId">设变明细 ID</param>
    /// <param name="existing">已有部门执行实体</param>
    /// <returns>视图更新 DTO</returns>
    public static TaktEcDeptViewUpdateDto MergeImportRow(
        TaktEcDeptViewImportDto row,
        long ecDetailId,
        object? existing)
    {
        var dto = existing == null
            ? new TaktEcDeptViewUpdateDto { EcDetailId = ecDetailId }
            : ToViewUpdateDto(existing);
        dto.EcDetailId = ecDetailId;
        if (row.IsImplemented.HasValue) dto.IsImplemented = row.IsImplemented.Value;
        if (row.Content != null) dto.Content = row.Content;
        if (row.EntryDate.HasValue) dto.EntryDate = row.EntryDate;
        if (row.EcLeader != null) dto.EcLeader = row.EcLeader;
        if (row.ScheduledDate.HasValue) dto.ScheduledDate = row.ScheduledDate;
        if (row.ScheduledBatch != null) dto.ScheduledBatch = row.ScheduledBatch;
        if (row.PoRemainder != null) dto.PoRemainder = row.PoRemainder;
        if (row.Balance != null) dto.Balance = row.Balance;
        if (row.OldProductHandling != null) dto.OldProductHandling = row.OldProductHandling;
        if (row.PurchaseOrderIssueDate.HasValue) dto.PurchaseOrderIssueDate = row.PurchaseOrderIssueDate;
        if (row.Supplier != null) dto.Supplier = row.Supplier;
        if (row.PurchaseOrderCode != null) dto.PurchaseOrderCode = row.PurchaseOrderCode;
        if (row.IqcOrderCode != null) dto.IqcOrderCode = row.IqcOrderCode;
        if (row.InspectionDate.HasValue) dto.InspectionDate = row.InspectionDate;
        if (row.OutboundBatch != null) dto.OutboundBatch = row.OutboundBatch;
        if (row.OutboundDate.HasValue) dto.OutboundDate = row.OutboundDate;
        if (row.ProductionDate.HasValue) dto.ProductionDate = row.ProductionDate;
        if (row.ProductionBatch != null) dto.ProductionBatch = row.ProductionBatch;
        if (row.OutboundOrderCode != null) dto.OutboundOrderCode = row.OutboundOrderCode;
        if (row.ProductionTeam != null) dto.ProductionTeam = row.ProductionTeam;
        if (row.ImplementationDate.HasValue) dto.ImplementationDate = row.ImplementationDate;
        if (row.ImplementationBatch != null) dto.ImplementationBatch = row.ImplementationBatch;
        if (row.InspectionBatch != null) dto.InspectionBatch = row.InspectionBatch;
        if (row.SamplingCode != null) dto.SamplingCode = row.SamplingCode;
        if (row.SopDate.HasValue) dto.SopDate = row.SopDate;
        if (row.IsSopUpdated.HasValue) dto.IsSopUpdated = row.IsSopUpdated.Value;
        if (row.Remark != null) dto.Remark = row.Remark;
        return dto;
    }

    /// <summary>
    /// 部门执行实体转视图更新 DTO
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <returns>视图更新 DTO</returns>
    public static TaktEcDeptViewUpdateDto ToViewUpdateDto(object exec)
    {
        var dto = new TaktEcDeptViewUpdateDto();
        switch (exec)
        {
            case TaktEcSeikan pmc:
                dto.EcDetailId = pmc.EcDetailId;
                dto.IsImplemented = pmc.IsImplemented;
                dto.Content = pmc.ExecContent;
                dto.Remark = pmc.Remark;
                dto.ScheduledDate = pmc.ScheduledDate;
                dto.ScheduledBatch = pmc.ScheduledBatch;
                dto.PoRemainder = pmc.PoRemainder;
                dto.Balance = pmc.Balance;
                dto.OldProductHandling = pmc.EcOldPartDisposition;
                break;
            case TaktEcKoubai mp:
                dto.EcDetailId = mp.EcDetailId;
                dto.IsImplemented = mp.IsImplemented;
                dto.Content = mp.ExecContent;
                dto.Remark = mp.Remark;
                dto.PurchaseOrderIssueDate = mp.PurchaseOrderIssueDate;
                dto.Supplier = mp.Supplier;
                dto.PurchaseOrderCode = mp.PurchaseOrderCode;
                dto.OldProductHandling = mp.EcOldPartDisposition;
                break;
            case TaktEcUkeken iqc:
                dto.EcDetailId = iqc.EcDetailId;
                dto.IsImplemented = iqc.IsImplemented;
                dto.Content = iqc.ExecContent;
                dto.Remark = iqc.Remark;
                dto.IqcOrderCode = iqc.IqcOrderCode;
                dto.InspectionDate = iqc.InspectionDate;
                break;
            case TaktEcBukan mc:
                dto.EcDetailId = mc.EcDetailId;
                dto.IsImplemented = mc.IsImplemented;
                dto.Content = mc.ExecContent;
                dto.Remark = mc.Remark;
                dto.OutboundBatch = mc.OutboundBatch;
                dto.OutboundDate = mc.OutboundDate;
                break;
            case TaktEcEmtl electronic:
                dto.EcDetailId = electronic.EcDetailId;
                dto.IsImplemented = electronic.IsImplemented;
                dto.Content = electronic.ExecContent;
                dto.Remark = electronic.Remark;
                dto.OutboundBatch = electronic.OutboundBatch;
                dto.OutboundDate = electronic.OutboundDate;
                break;
            case TaktEcSeizounika pcba:
                dto.EcDetailId = pcba.EcDetailId;
                dto.IsImplemented = pcba.IsImplemented;
                dto.Content = pcba.ExecContent;
                dto.Remark = pcba.Remark;
                dto.ProductionTeam = pcba.ProductionTeam;
                dto.ProductionDate = pcba.ProductionDate;
                dto.ImplementationBatch = pcba.ImplementationBatch;
                break;
            case TaktEcSeizouikka assy:
                dto.EcDetailId = assy.EcDetailId;
                dto.IsImplemented = assy.IsImplemented;
                dto.Content = assy.ExecContent;
                dto.Remark = assy.Remark;
                dto.ProductionTeam = assy.ProductionTeam;
                dto.ProductionDate = assy.ProductionDate;
                dto.ImplementationBatch = assy.ImplementationBatch;
                break;
            case TaktEcHinkan qa:
                dto.EcDetailId = qa.EcDetailId;
                dto.IsImplemented = qa.IsImplemented;
                dto.Content = qa.ExecContent;
                dto.Remark = qa.Remark;
                dto.ProductionTeam = qa.ProductionTeam;
                dto.InspectionDate = qa.InspectionDate;
                dto.InspectionBatch = qa.InspectionBatch;
                dto.SamplingCode = qa.SamplingCode;
                break;
            case TaktEcSeizougijutsu te:
                dto.EcDetailId = te.EcDetailId;
                dto.IsImplemented = te.IsImplemented;
                dto.Content = te.ExecContent;
                dto.Remark = te.Remark;
                dto.SopDate = te.SopDate;
                dto.IsSopUpdated = te.IsSopUpdated;
                break;
        }
        return dto;
    }
}
