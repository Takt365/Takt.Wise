// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Sop
// 文件名称：TaktSopArgumentDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：SopArgument 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSopArgument 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Sop;

// ========================================
// SopArgument 响应 DTO
// ========================================

/// <summary>
/// SOP 作业参数实体
/// 对应前端 TaktSopArgumentDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSopArgumentDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SopArgumentID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopArgumentId { get; set; }

    /// <summary>
    /// 执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ExecId { get; set; }

    /// <summary>
    /// 执行追溯 名称（填充字段）
    /// </summary>
    public string? ExecName { get; set; }

    /// <summary>
    /// 工步执行明细 ID（选项 TaktSopExecSteps/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ExecStepId { get; set; }

    /// <summary>
    /// 工步执行明细 名称（填充字段）
    /// </summary>
    public string? ExecStepName { get; set; }

    /// <summary>
    /// 工序参数定义 ID（选项 TaktRoutingItemArguments/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RoutingItemParameterId { get; set; }

    /// <summary>
    /// 工序参数定义 名称（填充字段）
    /// </summary>
    public string? RoutingItemParameterName { get; set; }

    /// <summary>
    /// 参数编码
    /// </summary>
    public string ParamCode { get; set; } = string.Empty;

    /// <summary>
    /// 实际值
    /// </summary>
    public decimal ActualValue { get; set; }

    /// <summary>
    /// 是否超差（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int IsOutOfRange { get; set; } = 0;

    /// <summary>
    /// 记录时间
    /// </summary>
    public DateTime RecordedAt { get; set; }

    /// <summary>
    /// 执行追溯
    /// （主表：TaktSopExec）
    /// </summary>
    public TaktSopExecDto? Exec { get; set; }

}

// ========================================
// SopArgument 查询 DTO
// ========================================

/// <summary>
/// SopArgument 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSopArgumentQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ExecId { get; set; }

    /// <summary>
    /// 工步执行明细 ID（选项 TaktSopExecSteps/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ExecStepId { get; set; }

    /// <summary>
    /// 工序参数定义 ID（选项 TaktRoutingItemArguments/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RoutingItemParameterId { get; set; }

    /// <summary>
    /// 参数编码
    /// </summary>
    public string? ParamCode { get; set; } = string.Empty;

    /// <summary>
    /// 实际值
    /// </summary>
    public decimal? ActualValue { get; set; }

    /// <summary>
    /// 是否超差（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int? IsOutOfRange { get; set; }

    /// <summary>
    /// 记录时间（范围查询-开始）
    /// </summary>
    public DateTime? RecordedAtStart { get; set; }

    /// <summary>
    /// 记录时间（范围查询-结束）
    /// </summary>
    public DateTime? RecordedAtEnd { get; set; }

    /// <summary>
    /// 创建时间（范围查询-开始）
    /// </summary>
    public DateTime? CreatedAtStart { get; set; }

    /// <summary>
    /// 创建时间（范围查询-结束）
    /// </summary>
    public DateTime? CreatedAtEnd { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建SopArgument DTO
// ========================================

/// <summary>
/// 创建SopArgument DTO
/// </summary>
public class TaktSopArgumentCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ExecId { get; set; }

    /// <summary>
    /// 工步执行明细 ID（选项 TaktSopExecSteps/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ExecStepId { get; set; }

    /// <summary>
    /// 工序参数定义 ID（选项 TaktRoutingItemArguments/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RoutingItemParameterId { get; set; }

    /// <summary>
    /// 参数编码
    /// </summary>
    [Required(ErrorMessage = "参数编码不能为空")]
    public string ParamCode { get; set; } = string.Empty;

    /// <summary>
    /// 实际值
    /// </summary>
    public decimal ActualValue { get; set; }

    /// <summary>
    /// 是否超差（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int IsOutOfRange { get; set; } = 0;

    /// <summary>
    /// 记录时间
    /// </summary>
    public DateTime RecordedAt { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 更新SopArgument DTO
// ========================================

/// <summary>
/// 更新SopArgument DTO
/// 继承 TaktSopArgumentCreateDto，添加 SopArgumentId 字段
/// </summary>
public class TaktSopArgumentUpdateDto : TaktSopArgumentCreateDto
{
    /// <summary>
    /// SopArgumentID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopArgumentId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SopArgument 导入模板行 DTO
/// </summary>
public class TaktSopArgumentTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ExecId { get; set; }

    /// <summary>
    /// 工步执行明细 ID（选项 TaktSopExecSteps/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ExecStepId { get; set; }

    /// <summary>
    /// 工序参数定义 ID（选项 TaktRoutingItemArguments/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RoutingItemParameterId { get; set; }

    /// <summary>
    /// 参数编码
    /// </summary>
    public string? ParamCode { get; set; } = string.Empty;

    /// <summary>
    /// 实际值
    /// </summary>
    public decimal? ActualValue { get; set; }

    /// <summary>
    /// 是否超差（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int? IsOutOfRange { get; set; }

    /// <summary>
    /// 记录时间
    /// </summary>
    public DateTime? RecordedAt { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// SopArgument 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSopArgumentImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ExecId { get; set; }

    /// <summary>
    /// 工步执行明细 ID（选项 TaktSopExecSteps/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ExecStepId { get; set; }

    /// <summary>
    /// 工序参数定义 ID（选项 TaktRoutingItemArguments/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RoutingItemParameterId { get; set; }

    /// <summary>
    /// 参数编码
    /// </summary>
    public string? ParamCode { get; set; } = string.Empty;

    /// <summary>
    /// 实际值
    /// </summary>
    public decimal? ActualValue { get; set; }

    /// <summary>
    /// 是否超差（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int? IsOutOfRange { get; set; }

    /// <summary>
    /// 记录时间
    /// </summary>
    public DateTime? RecordedAt { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// SopArgument 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSopArgumentExportDto
{
    /// <summary>
    /// SopArgumentID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopArgumentId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ExecId { get; set; }

    /// <summary>
    /// 工步执行明细 ID（选项 TaktSopExecSteps/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ExecStepId { get; set; }

    /// <summary>
    /// 工序参数定义 ID（选项 TaktRoutingItemArguments/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RoutingItemParameterId { get; set; }

    /// <summary>
    /// 参数编码
    /// </summary>
    public string ParamCode { get; set; } = string.Empty;

    /// <summary>
    /// 实际值
    /// </summary>
    public decimal ActualValue { get; set; }

    /// <summary>
    /// 是否超差（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int IsOutOfRange { get; set; } = 0;

    /// <summary>
    /// 记录时间
    /// </summary>
    public DateTime RecordedAt { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
