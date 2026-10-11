// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.CustomerService
// 文件名称：TaktCustomerServiceContractDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：CustomerServiceContract 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktCustomerServiceContract 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.CustomerService;

// ========================================
// CustomerServiceContract 响应 DTO
// ========================================

/// <summary>
/// 服务合同实体
/// 对应前端 TaktCustomerServiceContractDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktCustomerServiceContractDto : TaktCompanyDtoBase
{
    /// <summary>
    /// CustomerServiceContractID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerServiceContractId { get; set; }

    /// <summary>
    /// 服务合同编码（组合唯一索引）
    /// </summary>
    public string ServiceContractCode { get; set; } = string.Empty;

    /// <summary>
    /// 合同名称
    /// </summary>
    public string ContractName { get; set; } = string.Empty;

    /// <summary>
    /// 客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ClientId { get; set; }

    /// <summary>
    /// 客户端名称（填充字段）
    /// </summary>
    public string? ClientName { get; set; }

    /// <summary>
    /// 客户端编码（冗余字段，便于查询）
    /// </summary>
    public string ClientCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户端名称（冗余字段，便于查询）
    /// </summary>
    public string ClientName1 { get; set; } = string.Empty;

    /// <summary>
    /// 合同类型（0=维保，1=单次，2=框架，3=SLA，4=其他）
    /// </summary>
    public int ContractType { get; set; } = 0;

    /// <summary>
    /// 合同状态（0=草稿，1=生效，2=暂停，3=到期，4=终止）
    /// </summary>
    public int ContractStatus { get; set; } = 0;

    /// <summary>
    /// 签订日期
    /// </summary>
    public DateTime? SignDate { get; set; }

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>
    /// 到期日期
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// 合同金额
    /// </summary>
    public decimal ContractAmount { get; set; }

    /// <summary>
    /// 结算币种代码
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 付款条件（0=预付，1=后付，2=月结30天，3=月结60天，4=其他）
    /// </summary>
    public int PaymentTerms { get; set; } = 0;

    /// <summary>
    /// 服务范围描述
    /// </summary>
    public string? ServiceScope { get; set; } = string.Empty;

    /// <summary>
    /// SLA 响应时限（小时）
    /// </summary>
    public int SlaResponseHours { get; set; } = 0;

    /// <summary>
    /// SLA 解决时限（小时）
    /// </summary>
    public int SlaResolveHours { get; set; } = 0;

    /// <summary>
    /// 客户经理（人员代码）
    /// </summary>
    public string? AccountManagerEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

}

// ========================================
// CustomerServiceContract 查询 DTO
// ========================================

/// <summary>
/// CustomerServiceContract 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktCustomerServiceContractQueryDto : TaktPagedQuery
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
    /// 服务合同编码（组合唯一索引）
    /// </summary>
    public string? ServiceContractCode { get; set; } = string.Empty;

    /// <summary>
    /// 合同名称
    /// </summary>
    public string? ContractName { get; set; } = string.Empty;

    /// <summary>
    /// 客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ClientId { get; set; }

    /// <summary>
    /// 客户端编码（冗余字段，便于查询）
    /// </summary>
    public string? ClientCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户端名称（冗余字段，便于查询）
    /// </summary>
    public string? ClientName1 { get; set; } = string.Empty;

    /// <summary>
    /// 合同类型（0=维保，1=单次，2=框架，3=SLA，4=其他）
    /// </summary>
    public int? ContractType { get; set; }

    /// <summary>
    /// 合同状态（0=草稿，1=生效，2=暂停，3=到期，4=终止）
    /// </summary>
    public int? ContractStatus { get; set; }

    /// <summary>
    /// 签订日期（范围查询-开始）
    /// </summary>
    public DateTime? SignDateStart { get; set; }

    /// <summary>
    /// 签订日期（范围查询-结束）
    /// </summary>
    public DateTime? SignDateEnd { get; set; }

    /// <summary>
    /// 生效日期（范围查询-开始）
    /// </summary>
    public DateTime? EffectiveDateStart { get; set; }

    /// <summary>
    /// 生效日期（范围查询-结束）
    /// </summary>
    public DateTime? EffectiveDateEnd { get; set; }

    /// <summary>
    /// 到期日期（范围查询-开始）
    /// </summary>
    public DateTime? ExpiryDateStart { get; set; }

    /// <summary>
    /// 到期日期（范围查询-结束）
    /// </summary>
    public DateTime? ExpiryDateEnd { get; set; }

    /// <summary>
    /// 合同金额
    /// </summary>
    public decimal? ContractAmount { get; set; }

    /// <summary>
    /// 结算币种代码
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 付款条件（0=预付，1=后付，2=月结30天，3=月结60天，4=其他）
    /// </summary>
    public int? PaymentTerms { get; set; }

    /// <summary>
    /// 服务范围描述
    /// </summary>
    public string? ServiceScope { get; set; } = string.Empty;

    /// <summary>
    /// SLA 响应时限（小时）
    /// </summary>
    public int? SlaResponseHours { get; set; }

    /// <summary>
    /// SLA 解决时限（小时）
    /// </summary>
    public int? SlaResolveHours { get; set; }

    /// <summary>
    /// 客户经理（人员代码）
    /// </summary>
    public string? AccountManagerEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int? SortOrder { get; set; }

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
// 创建CustomerServiceContract DTO
// ========================================

/// <summary>
/// 创建CustomerServiceContract DTO
/// </summary>
public class TaktCustomerServiceContractCreateDto
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
    /// 服务合同编码（组合唯一索引）
    /// </summary>
    [Required(ErrorMessage = "服务合同编码（组合唯一索引）不能为空")]
    public string ServiceContractCode { get; set; } = string.Empty;

    /// <summary>
    /// 合同名称
    /// </summary>
    [Required(ErrorMessage = "合同名称不能为空")]
    public string ContractName { get; set; } = string.Empty;

    /// <summary>
    /// 客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ClientId { get; set; }

    /// <summary>
    /// 客户端编码（冗余字段，便于查询）
    /// </summary>
    public string ClientCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户端名称（冗余字段，便于查询）
    /// </summary>
    [Required(ErrorMessage = "客户端名称（冗余字段，便于查询）不能为空")]
    public string ClientName1 { get; set; } = string.Empty;

    /// <summary>
    /// 合同类型（0=维保，1=单次，2=框架，3=SLA，4=其他）
    /// </summary>
    public int ContractType { get; set; } = 0;

    /// <summary>
    /// 合同状态（0=草稿，1=生效，2=暂停，3=到期，4=终止）
    /// </summary>
    public int ContractStatus { get; set; } = 0;

    /// <summary>
    /// 签订日期
    /// </summary>
    public DateTime? SignDate { get; set; }

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>
    /// 到期日期
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// 合同金额
    /// </summary>
    public decimal ContractAmount { get; set; }

    /// <summary>
    /// 结算币种代码
    /// </summary>
    [Required(ErrorMessage = "结算币种代码不能为空")]
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 付款条件（0=预付，1=后付，2=月结30天，3=月结60天，4=其他）
    /// </summary>
    public int PaymentTerms { get; set; } = 0;

    /// <summary>
    /// 服务范围描述
    /// </summary>
    public string? ServiceScope { get; set; } = string.Empty;

    /// <summary>
    /// SLA 响应时限（小时）
    /// </summary>
    public int SlaResponseHours { get; set; } = 0;

    /// <summary>
    /// SLA 解决时限（小时）
    /// </summary>
    public int SlaResolveHours { get; set; } = 0;

    /// <summary>
    /// 客户经理（人员代码）
    /// </summary>
    public string? AccountManagerEmployeeName { get; set; } = string.Empty;

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
// 更新CustomerServiceContract DTO
// ========================================

/// <summary>
/// 更新CustomerServiceContract DTO
/// 继承 TaktCustomerServiceContractCreateDto，添加 CustomerServiceContractId 字段
/// </summary>
public class TaktCustomerServiceContractUpdateDto : TaktCustomerServiceContractCreateDto
{
    /// <summary>
    /// CustomerServiceContractID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerServiceContractId { get; set; }

}

// ========================================
// CustomerServiceContract 状态 DTO
// ========================================

/// <summary>
/// CustomerServiceContract 状态更新 DTO
/// </summary>
public class TaktCustomerServiceContractStatusDto
{
    /// <summary>
    /// CustomerServiceContractID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerServiceContractId { get; set; }

    /// <summary>
    /// 合同状态（0=草稿，1=生效，2=暂停，3=到期，4=终止）
    /// </summary>
    [Required(ErrorMessage = "合同状态（0=草稿，1=生效，2=暂停，3=到期，4=终止）不能为空")]
    public int ContractStatus { get; set; } = 0;
}

// ========================================
// CustomerServiceContract 排序 DTO
// ========================================

/// <summary>
/// CustomerServiceContract 排序更新 DTO
/// </summary>
public class TaktCustomerServiceContractSortDto
{
    /// <summary>
    /// CustomerServiceContractID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerServiceContractId { get; set; }

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    [Required(ErrorMessage = "排序号（回填）（越小越靠前）不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// CustomerServiceContract 导入模板行 DTO
/// </summary>
public class TaktCustomerServiceContractTemplateDto
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
    /// 服务合同编码（组合唯一索引）
    /// </summary>
    public string? ServiceContractCode { get; set; } = string.Empty;

    /// <summary>
    /// 合同名称
    /// </summary>
    public string? ContractName { get; set; } = string.Empty;

    /// <summary>
    /// 客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ClientId { get; set; }

    /// <summary>
    /// 客户端编码（冗余字段，便于查询）
    /// </summary>
    public string? ClientCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户端名称（冗余字段，便于查询）
    /// </summary>
    public string? ClientName1 { get; set; } = string.Empty;

    /// <summary>
    /// 合同类型（0=维保，1=单次，2=框架，3=SLA，4=其他）
    /// </summary>
    public int? ContractType { get; set; }

    /// <summary>
    /// 合同状态（0=草稿，1=生效，2=暂停，3=到期，4=终止）
    /// </summary>
    public int? ContractStatus { get; set; }

    /// <summary>
    /// 签订日期
    /// </summary>
    public DateTime? SignDate { get; set; }

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime? EffectiveDate { get; set; }

    /// <summary>
    /// 到期日期
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// 合同金额
    /// </summary>
    public decimal? ContractAmount { get; set; }

    /// <summary>
    /// 结算币种代码
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 付款条件（0=预付，1=后付，2=月结30天，3=月结60天，4=其他）
    /// </summary>
    public int? PaymentTerms { get; set; }

    /// <summary>
    /// 服务范围描述
    /// </summary>
    public string? ServiceScope { get; set; } = string.Empty;

    /// <summary>
    /// SLA 响应时限（小时）
    /// </summary>
    public int? SlaResponseHours { get; set; }

    /// <summary>
    /// SLA 解决时限（小时）
    /// </summary>
    public int? SlaResolveHours { get; set; }

    /// <summary>
    /// 客户经理（人员代码）
    /// </summary>
    public string? AccountManagerEmployeeName { get; set; } = string.Empty;

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
/// CustomerServiceContract 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktCustomerServiceContractImportDto
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
    /// 服务合同编码（组合唯一索引）
    /// </summary>
    public string? ServiceContractCode { get; set; } = string.Empty;

    /// <summary>
    /// 合同名称
    /// </summary>
    public string? ContractName { get; set; } = string.Empty;

    /// <summary>
    /// 客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ClientId { get; set; }

    /// <summary>
    /// 客户端编码（冗余字段，便于查询）
    /// </summary>
    public string? ClientCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户端名称（冗余字段，便于查询）
    /// </summary>
    public string? ClientName1 { get; set; } = string.Empty;

    /// <summary>
    /// 合同类型（0=维保，1=单次，2=框架，3=SLA，4=其他）
    /// </summary>
    public int? ContractType { get; set; }

    /// <summary>
    /// 合同状态（0=草稿，1=生效，2=暂停，3=到期，4=终止）
    /// </summary>
    public int? ContractStatus { get; set; }

    /// <summary>
    /// 签订日期
    /// </summary>
    public DateTime? SignDate { get; set; }

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime? EffectiveDate { get; set; }

    /// <summary>
    /// 到期日期
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// 合同金额
    /// </summary>
    public decimal? ContractAmount { get; set; }

    /// <summary>
    /// 结算币种代码
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 付款条件（0=预付，1=后付，2=月结30天，3=月结60天，4=其他）
    /// </summary>
    public int? PaymentTerms { get; set; }

    /// <summary>
    /// 服务范围描述
    /// </summary>
    public string? ServiceScope { get; set; } = string.Empty;

    /// <summary>
    /// SLA 响应时限（小时）
    /// </summary>
    public int? SlaResponseHours { get; set; }

    /// <summary>
    /// SLA 解决时限（小时）
    /// </summary>
    public int? SlaResolveHours { get; set; }

    /// <summary>
    /// 客户经理（人员代码）
    /// </summary>
    public string? AccountManagerEmployeeName { get; set; } = string.Empty;

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
/// CustomerServiceContract 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktCustomerServiceContractExportDto
{
    /// <summary>
    /// CustomerServiceContractID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerServiceContractId { get; set; }

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
    /// 服务合同编码（组合唯一索引）
    /// </summary>
    public string ServiceContractCode { get; set; } = string.Empty;

    /// <summary>
    /// 合同名称
    /// </summary>
    public string ContractName { get; set; } = string.Empty;

    /// <summary>
    /// 客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ClientId { get; set; }

    /// <summary>
    /// 客户端编码（冗余字段，便于查询）
    /// </summary>
    public string ClientCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户端名称（冗余字段，便于查询）
    /// </summary>
    public string ClientName1 { get; set; } = string.Empty;

    /// <summary>
    /// 合同类型（0=维保，1=单次，2=框架，3=SLA，4=其他）
    /// </summary>
    public int ContractType { get; set; } = 0;

    /// <summary>
    /// 合同状态（0=草稿，1=生效，2=暂停，3=到期，4=终止）
    /// </summary>
    public int ContractStatus { get; set; } = 0;

    /// <summary>
    /// 签订日期
    /// </summary>
    public DateTime? SignDate { get; set; }

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>
    /// 到期日期
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// 合同金额
    /// </summary>
    public decimal ContractAmount { get; set; }

    /// <summary>
    /// 结算币种代码
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 付款条件（0=预付，1=后付，2=月结30天，3=月结60天，4=其他）
    /// </summary>
    public int PaymentTerms { get; set; } = 0;

    /// <summary>
    /// 服务范围描述
    /// </summary>
    public string? ServiceScope { get; set; } = string.Empty;

    /// <summary>
    /// SLA 响应时限（小时）
    /// </summary>
    public int SlaResponseHours { get; set; } = 0;

    /// <summary>
    /// SLA 解决时限（小时）
    /// </summary>
    public int SlaResolveHours { get; set; } = 0;

    /// <summary>
    /// 客户经理（人员代码）
    /// </summary>
    public string? AccountManagerEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

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
