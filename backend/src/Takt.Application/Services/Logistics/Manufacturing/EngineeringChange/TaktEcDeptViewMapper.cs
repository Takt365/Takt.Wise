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
            case TaktEcPc pc:
                dto.EcExecId = pc.Id;
                dto.IsImplemented = pc.IsImplemented;
                dto.Content = pc.ExecContent;
                dto.Remark = pc.Remark;
                dto.ScheduledDate = pc.ScheduledDate;
                dto.ScheduledBatch = pc.ScheduledBatch;
                dto.PoRemainder = pc.PoRemainder;
                dto.BalanceQuantity = pc.BalanceQuantity;
                dto.OldProductHandling = pc.EcOldPartDisposition;
                break;
            case TaktEcPur pur:
                dto.EcExecId = pur.Id;
                dto.IsImplemented = pur.IsImplemented;
                dto.Content = pur.ExecContent;
                dto.Remark = pur.Remark;
                dto.PurchaseOrderIssueDate = pur.PurchaseOrderIssueDate;
                dto.Supplier = pur.Supplier;
                dto.PurchaseOrderCode = pur.PurchaseOrderCode;
                dto.OldProductHandling = pur.EcOldPartDisposition;
                break;
            case TaktEcIqc iqc:
                dto.EcExecId = iqc.Id;
                dto.IsImplemented = iqc.IsImplemented;
                dto.Content = iqc.ExecContent;
                dto.Remark = iqc.Remark;
                dto.IqcOrderCode = iqc.IqcOrderCode;
                dto.InspectionDate = iqc.InspectionDate;
                break;
            case TaktEcMc mc:
                dto.EcExecId = mc.Id;
                dto.IsImplemented = mc.IsImplemented;
                dto.Content = mc.ExecContent;
                dto.Remark = mc.Remark;
                dto.OutboundBatch = mc.OutboundBatch;
                dto.OutboundDate = mc.OutboundDate;
                break;
            case TaktEcMtl electronic:
                dto.EcExecId = electronic.Id;
                dto.IsImplemented = electronic.IsImplemented;
                dto.Content = electronic.ExecContent;
                dto.Remark = electronic.Remark;
                dto.OutboundBatch = electronic.OutboundBatch;
                dto.OutboundDate = electronic.OutboundDate;
                break;
            case TaktEcMfg2 mfg2:
                dto.EcExecId = mfg2.Id;
                dto.IsImplemented = mfg2.IsImplemented;
                dto.Content = mfg2.ExecContent;
                dto.Remark = mfg2.Remark;
                dto.ProductionTeam = mfg2.ProductionTeam;
                dto.ProductionDate = mfg2.ProductionDate;
                dto.ImplementationBatch = mfg2.ImplementationBatch;
                break;
            case TaktEcMfg1 mfg1:
                dto.EcExecId = mfg1.Id;
                dto.IsImplemented = mfg1.IsImplemented;
                dto.Content = mfg1.ExecContent;
                dto.Remark = mfg1.Remark;
                dto.ProductionTeam = mfg1.ProductionTeam;
                dto.ProductionDate = mfg1.ProductionDate;
                dto.ImplementationBatch = mfg1.ImplementationBatch;
                break;
            case TaktEcQc qc:
                dto.EcExecId = qc.Id;
                dto.IsImplemented = qc.IsImplemented;
                dto.Content = qc.ExecContent;
                dto.Remark = qc.Remark;
                dto.ProductionTeam = qc.ProductionTeam;
                dto.InspectionDate = qc.InspectionDate;
                dto.InspectionBatch = qc.InspectionBatch;
                dto.SamplingCode = qc.SamplingCode;
                break;
            case TaktEcMe me:
                dto.EcExecId = me.Id;
                dto.IsImplemented = me.IsImplemented;
                dto.Content = me.ExecContent;
                dto.Remark = me.Remark;
                dto.SopDate = me.SopDate;
                dto.IsSopUpdated = me.IsSopUpdated;
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
        if (row.BalanceQuantity != null) dto.BalanceQuantity = row.BalanceQuantity;
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
            case TaktEcPc pc:
                dto.IsImplemented = pc.IsImplemented;
                dto.Content = pc.ExecContent;
                dto.Remark = pc.Remark;
                dto.ScheduledDate = pc.ScheduledDate;
                dto.ScheduledBatch = pc.ScheduledBatch;
                dto.PoRemainder = pc.PoRemainder;
                dto.BalanceQuantity = pc.BalanceQuantity;
                dto.OldProductHandling = pc.EcOldPartDisposition;
                break;
            case TaktEcPur pur:
                dto.IsImplemented = pur.IsImplemented;
                dto.Content = pur.ExecContent;
                dto.Remark = pur.Remark;
                dto.PurchaseOrderIssueDate = pur.PurchaseOrderIssueDate;
                dto.Supplier = pur.Supplier;
                dto.PurchaseOrderCode = pur.PurchaseOrderCode;
                dto.OldProductHandling = pur.EcOldPartDisposition;
                break;
            case TaktEcIqc iqc:
                dto.IsImplemented = iqc.IsImplemented;
                dto.Content = iqc.ExecContent;
                dto.Remark = iqc.Remark;
                dto.IqcOrderCode = iqc.IqcOrderCode;
                dto.InspectionDate = iqc.InspectionDate;
                break;
            case TaktEcMc mc:
                dto.IsImplemented = mc.IsImplemented;
                dto.Content = mc.ExecContent;
                dto.Remark = mc.Remark;
                dto.OutboundBatch = mc.OutboundBatch;
                dto.OutboundDate = mc.OutboundDate;
                break;
            case TaktEcMtl electronic:
                dto.IsImplemented = electronic.IsImplemented;
                dto.Content = electronic.ExecContent;
                dto.Remark = electronic.Remark;
                dto.OutboundBatch = electronic.OutboundBatch;
                dto.OutboundDate = electronic.OutboundDate;
                break;
            case TaktEcMfg2 mfg2:
                dto.IsImplemented = mfg2.IsImplemented;
                dto.Content = mfg2.ExecContent;
                dto.Remark = mfg2.Remark;
                dto.ProductionTeam = mfg2.ProductionTeam;
                dto.ProductionDate = mfg2.ProductionDate;
                dto.ImplementationBatch = mfg2.ImplementationBatch;
                break;
            case TaktEcMfg1 mfg1:
                dto.IsImplemented = mfg1.IsImplemented;
                dto.Content = mfg1.ExecContent;
                dto.Remark = mfg1.Remark;
                dto.ProductionTeam = mfg1.ProductionTeam;
                dto.ProductionDate = mfg1.ProductionDate;
                dto.ImplementationBatch = mfg1.ImplementationBatch;
                break;
            case TaktEcQc qc:
                dto.IsImplemented = qc.IsImplemented;
                dto.Content = qc.ExecContent;
                dto.Remark = qc.Remark;
                dto.ProductionTeam = qc.ProductionTeam;
                dto.InspectionDate = qc.InspectionDate;
                dto.InspectionBatch = qc.InspectionBatch;
                dto.SamplingCode = qc.SamplingCode;
                break;
            case TaktEcMe me:
                dto.IsImplemented = me.IsImplemented;
                dto.Content = me.ExecContent;
                dto.Remark = me.Remark;
                dto.SopDate = me.SopDate;
                dto.IsSopUpdated = me.IsSopUpdated;
                break;
        }
        return dto;
    }
}
