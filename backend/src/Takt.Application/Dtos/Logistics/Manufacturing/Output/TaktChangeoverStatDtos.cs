// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Output
// 文件名称：TaktChangeoverStatDtos.cs
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：切换统计查询与清单行
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Logistics.Manufacturing.Output;

/// <summary>
/// 切换统计查询
/// </summary>
public class TaktChangeoverStatQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 工厂
    /// </summary>
    public string? PlantCode { get; set; }

    /// <summary>
    /// 期间起（月份第一天）
    /// </summary>
    public DateTime? PeriodDateStart { get; set; }

    /// <summary>
    /// 期间止（月份第一天，含该月）
    /// </summary>
    public DateTime? PeriodDateEnd { get; set; }

    /// <summary>
    /// 生产类别（字典 logistics_manufacturing_prod_category；可选）
    /// </summary>
    public string? ProdCategory { get; set; }

    /// <summary>
    /// 切换类别（字典 logistics_manufacturing_changeover_category；可选）
    /// </summary>
    public string? ChangeoverCategory { get; set; }

    /// <summary>
    /// 生产班组（可选）
    /// </summary>
    public string? TeamCode { get; set; }
}

/// <summary>
/// 切换统计清单行
/// </summary>
public class TaktChangeoverStatRowDto
{
    /// <summary>
    /// 行键
    /// </summary>
    public string RowKey { get; set; } = string.Empty;

    /// <summary>
    /// 工厂
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产类别
    /// </summary>
    public string ProdCategory { get; set; } = string.Empty;

    /// <summary>
    /// 切换类别
    /// </summary>
    public string ChangeoverCategory { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期
    /// </summary>
    public DateTime ProdDate { get; set; }

    /// <summary>
    /// 生产班组
    /// </summary>
    public string TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 当前工单
    /// </summary>
    public string CurrentProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 当前机种
    /// </summary>
    public string CurrentModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 切换后工单
    /// </summary>
    public string ChangeoverProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 切换后机种
    /// </summary>
    public string ChangeoverModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 切换次数
    /// </summary>
    public int ChangeoverCount { get; set; }

    /// <summary>
    /// 切换时间（分钟）
    /// </summary>
    public int ChangeoverTime { get; set; }

    /// <summary>
    /// 仪设时间（分钟）
    /// </summary>
    public int InstrumentSetupTime { get; set; }

    /// <summary>
    /// 切换总时间（分钟）
    /// </summary>
    public int TotalChangeoverTime { get; set; }

    /// <summary>
    /// 读取 SOP 时间（分钟）
    /// </summary>
    public int ReadSopTime { get; set; }

    /// <summary>
    /// 学习时间（分钟）
    /// </summary>
    public int LearningTime { get; set; }

    /// <summary>
    /// 人数
    /// </summary>
    public int PersonCount { get; set; }

    /// <summary>
    /// 学习总时间（分钟）
    /// </summary>
    public int TotalLearningTime { get; set; }

    /// <summary>
    /// SOP 总时间（分钟）
    /// </summary>
    public int TotalSopTime { get; set; }
}

/// <summary>
/// 切换统计结果
/// </summary>
public class TaktChangeoverStatResultDto
{
    /// <summary>
    /// 分页行
    /// </summary>
    public TaktPagedResult<TaktChangeoverStatRowDto> Paged { get; set; } = TaktPagedResult<TaktChangeoverStatRowDto>.Create([], 0, 1, 20);

    /// <summary>
    /// 命中行数
    /// </summary>
    public int RowCount { get; set; }
}
