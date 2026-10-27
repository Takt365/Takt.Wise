// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcDeptReportDtos.cs
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：设变统计页 DTO。各执行部门实施表经技术主表按发行日期或录入日期列出本表字段
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变统计查询
/// </summary>
public class TaktEcDeptReportQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 执行部门页签：mp、pc、iqc、mc、smt、pcba、assy、qa、te
    /// </summary>
    public string? StatTab { get; set; }

    /// <summary>
    /// 实施页签：all=全部，done=已实施，open=未实施。制技 done/open 按是否更新 SOP
    /// </summary>
    public string? ImplStatus { get; set; }

    /// <summary>
    /// 工厂代码（必填）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 发行期间起（技术主表发行日期所在月的首日）
    /// </summary>
    public DateTime? PeriodDateStart { get; set; }

    /// <summary>
    /// 发行期间止（技术主表发行日期所在月的首日）
    /// </summary>
    public DateTime? PeriodDateEnd { get; set; }

    /// <summary>
    /// 录入期间起（技术主表录入日期所在月的首日）
    /// </summary>
    public DateTime? EntryPeriodDateStart { get; set; }

    /// <summary>
    /// 录入期间止（技术主表录入日期所在月的首日）
    /// </summary>
    public DateTime? EntryPeriodDateEnd { get; set; }

    /// <summary>
    /// 设变单号（可选）
    /// </summary>
    public string? EcCode { get; set; }
}

/// <summary>
/// 设变统计行。列来自当前执行部门实施表，一行一条执行记录
/// </summary>
public class TaktEcDeptReportRowDto
{
    /// <summary>
    /// 执行行主键
    /// </summary>
    public string Id { get; set; } = string.Empty;

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
    /// 新品检验（字典 sys_yes_no）
    /// </summary>
    public int? EcNewRequiresInspection { get; set; }

    /// <summary>
    /// 实施（字典 sys_yes_no）
    /// </summary>
    public int? IsImplemented { get; set; }

    /// <summary>
    /// 更新 SOP（字典 sys_yes_no）。仅制造技术课
    /// </summary>
    public int? IsSopUpdated { get; set; }

    /// <summary>
    /// 预定日期（yyyy-MM-dd）
    /// </summary>
    public string? ScheduledDate { get; set; }

    /// <summary>
    /// 预定批次
    /// </summary>
    public string? ScheduledBatch { get; set; }

    /// <summary>
    /// 生产日期（yyyy-MM-dd）
    /// </summary>
    public string? ProductionDate { get; set; }

    /// <summary>
    /// 实施批次
    /// </summary>
    public string? ImplementationBatch { get; set; }

    /// <summary>
    /// 检验日期（yyyy-MM-dd）
    /// </summary>
    public string? InspectionDate { get; set; }

    /// <summary>
    /// 检验批次
    /// </summary>
    public string? InspectionBatch { get; set; }

    /// <summary>
    /// 采购订单发行日期（yyyy-MM-dd）
    /// </summary>
    public string? PurchaseOrderIssueDate { get; set; }

    /// <summary>
    /// 受检单号
    /// </summary>
    public string? IqcOrderCode { get; set; }

    /// <summary>
    /// SOP 日期（yyyy-MM-dd）
    /// </summary>
    public string? SopDate { get; set; }
}

/// <summary>
/// 设变统计结果
/// </summary>
public class TaktEcDeptReportResultDto
{
    /// <summary>
    /// 分页行
    /// </summary>
    public TaktPagedResult<TaktEcDeptReportRowDto> Paged { get; set; } = TaktPagedResult<TaktEcDeptReportRowDto>.Create([], 0, 1, 20);

    /// <summary>
    /// 总行数
    /// </summary>
    public int RowCount { get; set; }
}
