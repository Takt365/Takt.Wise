// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcGijutsuSourceInputDtos.cs
// 创建时间：2026-07-01
// 创建人：Takt365(Cursor AI)
// 功能描述：设变技术课来源设变录入 DTO（未导入列表、从来源导入）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 来源设变录入：公司代码与工厂代码映射结果
/// </summary>
public class TaktEcGijutsuSourcePlantCodeDto
{
    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 映射后的工厂代码（Database:CompanyCodes 与 PlantCodes 同序下标）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;
}

/// <summary>
/// 未导入来源设变列表项
/// </summary>
public class TaktEcGijutsuSourceEcInputItemDto
{
    /// <summary>
    /// 设变来源主表 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SourceEcId { get; set; }

    /// <summary>
    /// 设变号码
    /// </summary>
    public string SourceEcCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种
    /// </summary>
    public string SourceModel { get; set; } = string.Empty;

    /// <summary>
    /// 标题
    /// </summary>
    public string SourceTitle { get; set; } = string.Empty;

    /// <summary>
    /// 发行日期
    /// </summary>
    public DateTime SourceIssueDate { get; set; }

    /// <summary>
    /// 来源状态（来源 PLM 英文；导入时由 TaktEcSourceStatusMapper 映射为设变 ChangeStatus）
    /// </summary>
    public string SourceStatus { get; set; } = string.Empty;

    /// <summary>
    /// TCJ担当
    /// </summary>
    public string? SourceTcjOwner { get; set; }

    /// <summary>
    /// 来源明细行数
    /// </summary>
    public int DetailCount { get; set; }
}

/// <summary>
/// 未导入来源设变分页查询 DTO
/// </summary>
public class TaktEcGijutsuSourceEcInputQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 目标工厂代码（可选；为空时按 Database:CompanyCodes/PlantCodes 同序映射解析）
    /// </summary>
    public string? PlantCode { get; set; }

    /// <summary>
    /// 设变号码（模糊）
    /// </summary>
    public string? SourceEcCode { get; set; }

    /// <summary>
    /// 标题（模糊）
    /// </summary>
    public string? SourceTitle { get; set; }
}

/// <summary>
/// 从来源设变导入请求 DTO
/// </summary>
public class TaktEcGijutsuImportFromSourceDto
{
    /// <summary>
    /// 目标工厂代码（可选；每条来源设变按 CompanyCodes/PlantCodes 同序映射，传入时须与映射结果一致）
    /// </summary>
    public string? PlantCode { get; set; }

    /// <summary>
    /// 公司默认文化（兼容旧入参；写入实体列为 CultureCode，优先使用 CultureCode）
    /// </summary>
    public string? CompanyDefaultCulture { get; set; }

    /// <summary>
    /// 待导入来源设变 ID 列表（前端 string，逐项解析为 long）
    /// </summary>
    public List<string> SourceEcIds { get; set; } = [];

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code）
    /// </summary>
    public string? CultureCode { get; set; }
}

/// <summary>
/// 从来源设变构建创建草稿请求 DTO（不落库，供前端 ec-form 补全后 create）
/// </summary>
public class TaktEcGijutsuDraftFromSourceDto
{
    /// <summary>
    /// 目标工厂代码（可选；按来源设变公司代码同序映射，传入时须与映射结果一致）
    /// </summary>
    public string? PlantCode { get; set; }

    /// <summary>
    /// 来源设变主表 ID（前端 string）
    /// </summary>
    [Required(ErrorMessage = "来源设变不能为空")]
    public string SourceEcId { get; set; } = string.Empty;

    /// <summary>
    /// 公司默认文化（兼容旧入参；写入实体列为 CultureCode，优先使用 CultureCode）
    /// </summary>
    public string? CompanyDefaultCulture { get; set; }

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; }
}

/// <summary>
/// 从来源设变导入结果 DTO
/// </summary>
public class TaktEcGijutsuImportFromSourceResultDto
{
    /// <summary>
    /// 成功条数
    /// </summary>
    public int SuccessCount { get; set; }

    /// <summary>
    /// 失败条数
    /// </summary>
    public int FailCount { get; set; }

    /// <summary>
    /// 错误信息
    /// </summary>
    public List<string> Errors { get; set; } = [];

    /// <summary>
    /// 新创建的设变技术课主 ID 列表
    /// </summary>
    public List<string> CreatedEcGijutsuIds { get; set; } = [];
}
