// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Workflow
// 文件名称：TaktFlowAddSignDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：FlowAddSign 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktFlowAddSign 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Workflow;

// ========================================
// FlowAddSign 响应 DTO
// ========================================

/// <summary>
/// 流程加签记录实体
/// 对应前端 TaktFlowAddSignDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktFlowAddSignDto : TaktCompanyDtoBase
{
    /// <summary>
    /// FlowAddSignID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowAddSignId { get; set; }

    /// <summary>
    /// 流程实例 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long InstanceId { get; set; }

    /// <summary>
    /// 流程实例 名称（填充字段）
    /// </summary>
    public string? InstanceName { get; set; }

    /// <summary>
    /// 加签节点 ID
    /// </summary>
    public string NodeId { get; set; } = string.Empty;

    /// <summary>
    /// 加签节点 名称（填充字段）
    /// </summary>
    public string? NodeName { get; set; }

    /// <summary>
    /// 加签人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SignUserId { get; set; }

    /// <summary>
    /// 加签人姓名
    /// </summary>
    public string? SignUserName { get; set; } = string.Empty;

    /// <summary>
    /// 加签方式（sequential / all / one，与前端 approveType 一致）
    /// </summary>
    public string SignType { get; set; } = string.Empty;

    /// <summary>
    /// 完成后是否回到加签节点
    /// </summary>
    public int ReturnToSignNode { get; set; } = 0;

    /// <summary>
    /// 加签原因
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 是否已处理（含减签）
    /// </summary>
    public int IsHandled { get; set; } = 0;

    /// <summary>
    /// 所属流程实例
    /// （主表：TaktFlowInstance）
    /// </summary>
    public TaktFlowInstanceDto? Instance { get; set; }

}

// ========================================
// FlowAddSign 查询 DTO
// ========================================

/// <summary>
/// FlowAddSign 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktFlowAddSignQueryDto : TaktPagedQuery
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
    /// 流程实例 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InstanceId { get; set; }

    /// <summary>
    /// 加签节点 ID
    /// </summary>
    public string? NodeId { get; set; } = string.Empty;

    /// <summary>
    /// 加签人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SignUserId { get; set; }

    /// <summary>
    /// 加签人姓名
    /// </summary>
    public string? SignUserName { get; set; } = string.Empty;

    /// <summary>
    /// 加签方式（sequential / all / one，与前端 approveType 一致）
    /// </summary>
    public string? SignType { get; set; } = string.Empty;

    /// <summary>
    /// 完成后是否回到加签节点
    /// </summary>
    public int? ReturnToSignNode { get; set; }

    /// <summary>
    /// 加签原因
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 是否已处理（含减签）
    /// </summary>
    public int? IsHandled { get; set; }

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
// 创建FlowAddSign DTO
// ========================================

/// <summary>
/// 创建FlowAddSign DTO
/// </summary>
public class TaktFlowAddSignCreateDto
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
    /// 流程实例 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long InstanceId { get; set; }

    /// <summary>
    /// 加签节点 ID
    /// </summary>
    [Required(ErrorMessage = "加签节点 ID不能为空")]
    public string NodeId { get; set; } = string.Empty;

    /// <summary>
    /// 加签人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SignUserId { get; set; }

    /// <summary>
    /// 加签人姓名
    /// </summary>
    public string? SignUserName { get; set; } = string.Empty;

    /// <summary>
    /// 加签方式（sequential / all / one，与前端 approveType 一致）
    /// </summary>
    [Required(ErrorMessage = "加签方式（sequential / all / one，与前端 approveType 一致）不能为空")]
    public string SignType { get; set; } = string.Empty;

    /// <summary>
    /// 完成后是否回到加签节点
    /// </summary>
    public int ReturnToSignNode { get; set; } = 0;

    /// <summary>
    /// 加签原因
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 是否已处理（含减签）
    /// </summary>
    public int IsHandled { get; set; } = 0;

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
// 更新FlowAddSign DTO
// ========================================

/// <summary>
/// 更新FlowAddSign DTO
/// 继承 TaktFlowAddSignCreateDto，添加 FlowAddSignId 字段
/// </summary>
public class TaktFlowAddSignUpdateDto : TaktFlowAddSignCreateDto
{
    /// <summary>
    /// FlowAddSignID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowAddSignId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// FlowAddSign 导入模板行 DTO
/// </summary>
public class TaktFlowAddSignTemplateDto
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
    /// 流程实例 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InstanceId { get; set; }

    /// <summary>
    /// 加签节点 ID
    /// </summary>
    public string? NodeId { get; set; } = string.Empty;

    /// <summary>
    /// 加签人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SignUserId { get; set; }

    /// <summary>
    /// 加签人姓名
    /// </summary>
    public string? SignUserName { get; set; } = string.Empty;

    /// <summary>
    /// 加签方式（sequential / all / one，与前端 approveType 一致）
    /// </summary>
    public string? SignType { get; set; } = string.Empty;

    /// <summary>
    /// 完成后是否回到加签节点
    /// </summary>
    public int? ReturnToSignNode { get; set; }

    /// <summary>
    /// 加签原因
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 是否已处理（含减签）
    /// </summary>
    public int? IsHandled { get; set; }

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
/// FlowAddSign 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktFlowAddSignImportDto
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
    /// 流程实例 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InstanceId { get; set; }

    /// <summary>
    /// 加签节点 ID
    /// </summary>
    public string? NodeId { get; set; } = string.Empty;

    /// <summary>
    /// 加签人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SignUserId { get; set; }

    /// <summary>
    /// 加签人姓名
    /// </summary>
    public string? SignUserName { get; set; } = string.Empty;

    /// <summary>
    /// 加签方式（sequential / all / one，与前端 approveType 一致）
    /// </summary>
    public string? SignType { get; set; } = string.Empty;

    /// <summary>
    /// 完成后是否回到加签节点
    /// </summary>
    public int? ReturnToSignNode { get; set; }

    /// <summary>
    /// 加签原因
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 是否已处理（含减签）
    /// </summary>
    public int? IsHandled { get; set; }

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
/// FlowAddSign 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktFlowAddSignExportDto
{
    /// <summary>
    /// FlowAddSignID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowAddSignId { get; set; }

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
    /// 流程实例 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long InstanceId { get; set; }

    /// <summary>
    /// 加签节点 ID
    /// </summary>
    public string NodeId { get; set; } = string.Empty;

    /// <summary>
    /// 加签人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SignUserId { get; set; }

    /// <summary>
    /// 加签人姓名
    /// </summary>
    public string? SignUserName { get; set; } = string.Empty;

    /// <summary>
    /// 加签方式（sequential / all / one，与前端 approveType 一致）
    /// </summary>
    public string SignType { get; set; } = string.Empty;

    /// <summary>
    /// 完成后是否回到加签节点
    /// </summary>
    public int ReturnToSignNode { get; set; } = 0;

    /// <summary>
    /// 加签原因
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 是否已处理（含减签）
    /// </summary>
    public int IsHandled { get; set; } = 0;

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
