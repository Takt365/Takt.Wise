// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Accounting.Financial
// 文件名称：TaktBankDtos.cs
// 创建时间：2026-08-13
// 创建人：Takt365(Auto Generated)
// 功能描述：Bank 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktBank 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Accounting.Financial;

// ========================================
// Bank 响应 DTO
// ========================================

/// <summary>
/// 银行信息实体（公司级；租户+公司隔离；按国家地区 + 银行代码唯一）
/// 对应前端 TaktBankDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktBankDto : TaktCompanyDtoBase
{
    /// <summary>
    /// BankID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BankId { get; set; }

    /// <summary>
    /// 国家地区（选项字典 sys_country_code，DictValue=ISO alpha-2）
    /// </summary>
    public string CountryRegion { get; set; } = string.Empty;

    /// <summary>
    /// 银行代码（；CHAR 15；与国家地区组成业务唯一键）
    /// </summary>
    public string BankCode { get; set; } = string.Empty;

    /// <summary>
    /// 银行名称1
    /// </summary>
    public string BankName1 { get; set; } = string.Empty;

    /// <summary>
    /// 银行名称2
    /// </summary>
    public string? BankName2 { get; set; } = string.Empty;

    /// <summary>
    /// 州省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    public string? Province { get; set; } = string.Empty;

    /// <summary>
    /// 地市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    public string? Prefecture { get; set; } = string.Empty;

    /// <summary>
    /// 区县（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=4）
    /// </summary>
    public string? District { get; set; } = string.Empty;

    /// <summary>
    /// 乡镇街道（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=5）
    /// </summary>
    public string? Township { get; set; } = string.Empty;

    /// <summary>
    /// 行政村（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=6）
    /// </summary>
    public string? Village { get; set; } = string.Empty;

    /// <summary>
    /// 地址1（详细地址行1）
    /// </summary>
    public string? Address1 { get; set; } = string.Empty;

    /// <summary>
    /// 地址2（详细地址行2）
    /// </summary>
    public string? Address2 { get; set; } = string.Empty;

    /// <summary>
    /// 银行识别码（SWIFT/BIC；CHAR 11）
    /// </summary>
    public string? SwiftBic { get; set; } = string.Empty;

    /// <summary>
    /// 银行组（；CHAR 2）
    /// </summary>
    public string? BankGroup { get; set; } = string.Empty;

    /// <summary>
    /// 邮政银行往来账户（字典 sys_yes_no）
    /// </summary>
    public int PobkCurAc { get; set; } = 0;

    /// <summary>
    /// 银行编码（；CHAR 15）
    /// </summary>
    public string? BankNumber { get; set; } = string.Empty;

    /// <summary>
    /// 邮政银行（；CHAR 16）
    /// </summary>
    public string? PostalBank { get; set; } = string.Empty;

    /// <summary>
    /// 地址号（；CHAR 10）
    /// </summary>
    public string? AddressNumber { get; set; } = string.Empty;

    /// <summary>
    /// 分行（；CHAR 40）
    /// </summary>
    public string? Branch { get; set; } = string.Empty;

    /// <summary>
    /// 方法（CHAR 4）
    /// </summary>
    public string? BankMethod { get; set; } = string.Empty;

    /// <summary>
    /// 格式（含银行数据文件的格式；CHAR 3）
    /// </summary>
    public string? BankFormat { get; set; } = string.Empty;

    /// <summary>
    /// IBAN 规则（CHAR 6）
    /// </summary>
    public string? IbanRule { get; set; } = string.Empty;

    /// <summary>
    /// 企业间（字典 sys_yes_no）
    /// </summary>
    public int SddB2b { get; set; } = 0;

    /// <summary>
    /// 核心个人（字典 sys_yes_no）
    /// </summary>
    public int SddCore { get; set; } = 0;

    /// <summary>
    /// SEPA拒付交易支持标识（字典 accounting_financial_sepa_rtrans_type）
    /// </summary>
    public int SddRtrans { get; set; } = 0;

    /// <summary>
    /// BIC+ 编码（CHAR 12）
    /// </summary>
    public string? BicPlusNumber { get; set; } = string.Empty;

    /// <summary>
    /// 路径代码（CHAR 15）
    /// </summary>
    public string? PathCode { get; set; } = string.Empty;

}

// ========================================
// Bank 查询 DTO
// ========================================

/// <summary>
/// Bank 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktBankQueryDto : TaktPagedQuery
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
    /// 国家地区（选项字典 sys_country_code，DictValue=ISO alpha-2）
    /// </summary>
    public string? CountryRegion { get; set; } = string.Empty;

    /// <summary>
    /// 银行代码（；CHAR 15；与国家地区组成业务唯一键）
    /// </summary>
    public string? BankCode { get; set; } = string.Empty;

    /// <summary>
    /// 银行名称1
    /// </summary>
    public string? BankName1 { get; set; } = string.Empty;

    /// <summary>
    /// 银行名称2
    /// </summary>
    public string? BankName2 { get; set; } = string.Empty;

    /// <summary>
    /// 州省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    public string? Province { get; set; } = string.Empty;

    /// <summary>
    /// 地市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    public string? Prefecture { get; set; } = string.Empty;

    /// <summary>
    /// 区县（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=4）
    /// </summary>
    public string? District { get; set; } = string.Empty;

    /// <summary>
    /// 乡镇街道（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=5）
    /// </summary>
    public string? Township { get; set; } = string.Empty;

    /// <summary>
    /// 行政村（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=6）
    /// </summary>
    public string? Village { get; set; } = string.Empty;

    /// <summary>
    /// 地址1（详细地址行1）
    /// </summary>
    public string? Address1 { get; set; } = string.Empty;

    /// <summary>
    /// 地址2（详细地址行2）
    /// </summary>
    public string? Address2 { get; set; } = string.Empty;

    /// <summary>
    /// 银行识别码（SWIFT/BIC；CHAR 11）
    /// </summary>
    public string? SwiftBic { get; set; } = string.Empty;

    /// <summary>
    /// 银行组（；CHAR 2）
    /// </summary>
    public string? BankGroup { get; set; } = string.Empty;

    /// <summary>
    /// 邮政银行往来账户（字典 sys_yes_no）
    /// </summary>
    public int? PobkCurAc { get; set; }

    /// <summary>
    /// 银行编码（；CHAR 15）
    /// </summary>
    public string? BankNumber { get; set; } = string.Empty;

    /// <summary>
    /// 邮政银行（；CHAR 16）
    /// </summary>
    public string? PostalBank { get; set; } = string.Empty;

    /// <summary>
    /// 地址号（；CHAR 10）
    /// </summary>
    public string? AddressNumber { get; set; } = string.Empty;

    /// <summary>
    /// 分行（；CHAR 40）
    /// </summary>
    public string? Branch { get; set; } = string.Empty;

    /// <summary>
    /// 方法（CHAR 4）
    /// </summary>
    public string? BankMethod { get; set; } = string.Empty;

    /// <summary>
    /// 格式（含银行数据文件的格式；CHAR 3）
    /// </summary>
    public string? BankFormat { get; set; } = string.Empty;

    /// <summary>
    /// IBAN 规则（CHAR 6）
    /// </summary>
    public string? IbanRule { get; set; } = string.Empty;

    /// <summary>
    /// 企业间（字典 sys_yes_no）
    /// </summary>
    public int? SddB2b { get; set; }

    /// <summary>
    /// 核心个人（字典 sys_yes_no）
    /// </summary>
    public int? SddCore { get; set; }

    /// <summary>
    /// SEPA拒付交易支持标识（字典 accounting_financial_sepa_rtrans_type）
    /// </summary>
    public int? SddRtrans { get; set; }

    /// <summary>
    /// BIC+ 编码（CHAR 12）
    /// </summary>
    public string? BicPlusNumber { get; set; } = string.Empty;

    /// <summary>
    /// 路径代码（CHAR 15）
    /// </summary>
    public string? PathCode { get; set; } = string.Empty;

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
// 创建Bank DTO
// ========================================

/// <summary>
/// 创建Bank DTO
/// </summary>
public class TaktBankCreateDto
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
    /// 国家地区（选项字典 sys_country_code，DictValue=ISO alpha-2）
    /// </summary>
    [Required(ErrorMessage = "国家地区（选项字典 sys_country_code，DictValue=ISO alpha-2）不能为空")]
    public string CountryRegion { get; set; } = string.Empty;

    /// <summary>
    /// 银行代码（；CHAR 15；与国家地区组成业务唯一键）
    /// </summary>
    [Required(ErrorMessage = "银行代码（；CHAR 15；与国家地区组成业务唯一键）不能为空")]
    public string BankCode { get; set; } = string.Empty;

    /// <summary>
    /// 银行名称1
    /// </summary>
    [Required(ErrorMessage = "银行名称1不能为空")]
    public string BankName1 { get; set; } = string.Empty;

    /// <summary>
    /// 银行名称2
    /// </summary>
    public string? BankName2 { get; set; } = string.Empty;

    /// <summary>
    /// 州省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    public string? Province { get; set; } = string.Empty;

    /// <summary>
    /// 地市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    public string? Prefecture { get; set; } = string.Empty;

    /// <summary>
    /// 区县（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=4）
    /// </summary>
    public string? District { get; set; } = string.Empty;

    /// <summary>
    /// 乡镇街道（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=5）
    /// </summary>
    public string? Township { get; set; } = string.Empty;

    /// <summary>
    /// 行政村（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=6）
    /// </summary>
    public string? Village { get; set; } = string.Empty;

    /// <summary>
    /// 地址1（详细地址行1）
    /// </summary>
    public string? Address1 { get; set; } = string.Empty;

    /// <summary>
    /// 地址2（详细地址行2）
    /// </summary>
    public string? Address2 { get; set; } = string.Empty;

    /// <summary>
    /// 银行识别码（SWIFT/BIC；CHAR 11）
    /// </summary>
    public string? SwiftBic { get; set; } = string.Empty;

    /// <summary>
    /// 银行组（；CHAR 2）
    /// </summary>
    public string? BankGroup { get; set; } = string.Empty;

    /// <summary>
    /// 邮政银行往来账户（字典 sys_yes_no）
    /// </summary>
    public int PobkCurAc { get; set; } = 0;

    /// <summary>
    /// 银行编码（；CHAR 15）
    /// </summary>
    public string? BankNumber { get; set; } = string.Empty;

    /// <summary>
    /// 邮政银行（；CHAR 16）
    /// </summary>
    public string? PostalBank { get; set; } = string.Empty;

    /// <summary>
    /// 地址号（；CHAR 10）
    /// </summary>
    public string? AddressNumber { get; set; } = string.Empty;

    /// <summary>
    /// 分行（；CHAR 40）
    /// </summary>
    public string? Branch { get; set; } = string.Empty;

    /// <summary>
    /// 方法（CHAR 4）
    /// </summary>
    public string? BankMethod { get; set; } = string.Empty;

    /// <summary>
    /// 格式（含银行数据文件的格式；CHAR 3）
    /// </summary>
    public string? BankFormat { get; set; } = string.Empty;

    /// <summary>
    /// IBAN 规则（CHAR 6）
    /// </summary>
    public string? IbanRule { get; set; } = string.Empty;

    /// <summary>
    /// 企业间（字典 sys_yes_no）
    /// </summary>
    public int SddB2b { get; set; } = 0;

    /// <summary>
    /// 核心个人（字典 sys_yes_no）
    /// </summary>
    public int SddCore { get; set; } = 0;

    /// <summary>
    /// SEPA拒付交易支持标识（字典 accounting_financial_sepa_rtrans_type）
    /// </summary>
    public int SddRtrans { get; set; } = 0;

    /// <summary>
    /// BIC+ 编码（CHAR 12）
    /// </summary>
    public string? BicPlusNumber { get; set; } = string.Empty;

    /// <summary>
    /// 路径代码（CHAR 15）
    /// </summary>
    public string? PathCode { get; set; } = string.Empty;

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
// 更新Bank DTO
// ========================================

/// <summary>
/// 更新Bank DTO
/// 继承 TaktBankCreateDto，添加 BankId 字段
/// </summary>
public class TaktBankUpdateDto : TaktBankCreateDto
{
    /// <summary>
    /// BankID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BankId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Bank 导入模板行 DTO
/// </summary>
public class TaktBankTemplateDto
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
    /// 国家地区（选项字典 sys_country_code，DictValue=ISO alpha-2）
    /// </summary>
    public string? CountryRegion { get; set; } = string.Empty;

    /// <summary>
    /// 银行代码（；CHAR 15；与国家地区组成业务唯一键）
    /// </summary>
    public string? BankCode { get; set; } = string.Empty;

    /// <summary>
    /// 银行名称1
    /// </summary>
    public string? BankName1 { get; set; } = string.Empty;

    /// <summary>
    /// 银行名称2
    /// </summary>
    public string? BankName2 { get; set; } = string.Empty;

    /// <summary>
    /// 州省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    public string? Province { get; set; } = string.Empty;

    /// <summary>
    /// 地市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    public string? Prefecture { get; set; } = string.Empty;

    /// <summary>
    /// 区县（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=4）
    /// </summary>
    public string? District { get; set; } = string.Empty;

    /// <summary>
    /// 乡镇街道（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=5）
    /// </summary>
    public string? Township { get; set; } = string.Empty;

    /// <summary>
    /// 行政村（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=6）
    /// </summary>
    public string? Village { get; set; } = string.Empty;

    /// <summary>
    /// 地址1（详细地址行1）
    /// </summary>
    public string? Address1 { get; set; } = string.Empty;

    /// <summary>
    /// 地址2（详细地址行2）
    /// </summary>
    public string? Address2 { get; set; } = string.Empty;

    /// <summary>
    /// 银行识别码（SWIFT/BIC；CHAR 11）
    /// </summary>
    public string? SwiftBic { get; set; } = string.Empty;

    /// <summary>
    /// 银行组（；CHAR 2）
    /// </summary>
    public string? BankGroup { get; set; } = string.Empty;

    /// <summary>
    /// 邮政银行往来账户（字典 sys_yes_no）
    /// </summary>
    public int? PobkCurAc { get; set; }

    /// <summary>
    /// 银行编码（；CHAR 15）
    /// </summary>
    public string? BankNumber { get; set; } = string.Empty;

    /// <summary>
    /// 邮政银行（；CHAR 16）
    /// </summary>
    public string? PostalBank { get; set; } = string.Empty;

    /// <summary>
    /// 地址号（；CHAR 10）
    /// </summary>
    public string? AddressNumber { get; set; } = string.Empty;

    /// <summary>
    /// 分行（；CHAR 40）
    /// </summary>
    public string? Branch { get; set; } = string.Empty;

    /// <summary>
    /// 方法（CHAR 4）
    /// </summary>
    public string? BankMethod { get; set; } = string.Empty;

    /// <summary>
    /// 格式（含银行数据文件的格式；CHAR 3）
    /// </summary>
    public string? BankFormat { get; set; } = string.Empty;

    /// <summary>
    /// IBAN 规则（CHAR 6）
    /// </summary>
    public string? IbanRule { get; set; } = string.Empty;

    /// <summary>
    /// 企业间（字典 sys_yes_no）
    /// </summary>
    public int? SddB2b { get; set; }

    /// <summary>
    /// 核心个人（字典 sys_yes_no）
    /// </summary>
    public int? SddCore { get; set; }

    /// <summary>
    /// SEPA拒付交易支持标识（字典 accounting_financial_sepa_rtrans_type）
    /// </summary>
    public int? SddRtrans { get; set; }

    /// <summary>
    /// BIC+ 编码（CHAR 12）
    /// </summary>
    public string? BicPlusNumber { get; set; } = string.Empty;

    /// <summary>
    /// 路径代码（CHAR 15）
    /// </summary>
    public string? PathCode { get; set; } = string.Empty;

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
/// Bank 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktBankImportDto
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
    /// 国家地区（选项字典 sys_country_code，DictValue=ISO alpha-2）
    /// </summary>
    public string? CountryRegion { get; set; } = string.Empty;

    /// <summary>
    /// 银行代码（；CHAR 15；与国家地区组成业务唯一键）
    /// </summary>
    public string? BankCode { get; set; } = string.Empty;

    /// <summary>
    /// 银行名称1
    /// </summary>
    public string? BankName1 { get; set; } = string.Empty;

    /// <summary>
    /// 银行名称2
    /// </summary>
    public string? BankName2 { get; set; } = string.Empty;

    /// <summary>
    /// 州省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    public string? Province { get; set; } = string.Empty;

    /// <summary>
    /// 地市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    public string? Prefecture { get; set; } = string.Empty;

    /// <summary>
    /// 区县（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=4）
    /// </summary>
    public string? District { get; set; } = string.Empty;

    /// <summary>
    /// 乡镇街道（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=5）
    /// </summary>
    public string? Township { get; set; } = string.Empty;

    /// <summary>
    /// 行政村（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=6）
    /// </summary>
    public string? Village { get; set; } = string.Empty;

    /// <summary>
    /// 地址1（详细地址行1）
    /// </summary>
    public string? Address1 { get; set; } = string.Empty;

    /// <summary>
    /// 地址2（详细地址行2）
    /// </summary>
    public string? Address2 { get; set; } = string.Empty;

    /// <summary>
    /// 银行识别码（SWIFT/BIC；CHAR 11）
    /// </summary>
    public string? SwiftBic { get; set; } = string.Empty;

    /// <summary>
    /// 银行组（；CHAR 2）
    /// </summary>
    public string? BankGroup { get; set; } = string.Empty;

    /// <summary>
    /// 邮政银行往来账户（字典 sys_yes_no）
    /// </summary>
    public int? PobkCurAc { get; set; }

    /// <summary>
    /// 银行编码（；CHAR 15）
    /// </summary>
    public string? BankNumber { get; set; } = string.Empty;

    /// <summary>
    /// 邮政银行（；CHAR 16）
    /// </summary>
    public string? PostalBank { get; set; } = string.Empty;

    /// <summary>
    /// 地址号（；CHAR 10）
    /// </summary>
    public string? AddressNumber { get; set; } = string.Empty;

    /// <summary>
    /// 分行（；CHAR 40）
    /// </summary>
    public string? Branch { get; set; } = string.Empty;

    /// <summary>
    /// 方法（CHAR 4）
    /// </summary>
    public string? BankMethod { get; set; } = string.Empty;

    /// <summary>
    /// 格式（含银行数据文件的格式；CHAR 3）
    /// </summary>
    public string? BankFormat { get; set; } = string.Empty;

    /// <summary>
    /// IBAN 规则（CHAR 6）
    /// </summary>
    public string? IbanRule { get; set; } = string.Empty;

    /// <summary>
    /// 企业间（字典 sys_yes_no）
    /// </summary>
    public int? SddB2b { get; set; }

    /// <summary>
    /// 核心个人（字典 sys_yes_no）
    /// </summary>
    public int? SddCore { get; set; }

    /// <summary>
    /// SEPA拒付交易支持标识（字典 accounting_financial_sepa_rtrans_type）
    /// </summary>
    public int? SddRtrans { get; set; }

    /// <summary>
    /// BIC+ 编码（CHAR 12）
    /// </summary>
    public string? BicPlusNumber { get; set; } = string.Empty;

    /// <summary>
    /// 路径代码（CHAR 15）
    /// </summary>
    public string? PathCode { get; set; } = string.Empty;

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
/// Bank 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktBankExportDto
{
    /// <summary>
    /// BankID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BankId { get; set; }

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
    /// 国家地区（选项字典 sys_country_code，DictValue=ISO alpha-2）
    /// </summary>
    public string CountryRegion { get; set; } = string.Empty;

    /// <summary>
    /// 银行代码（；CHAR 15；与国家地区组成业务唯一键）
    /// </summary>
    public string BankCode { get; set; } = string.Empty;

    /// <summary>
    /// 银行名称1
    /// </summary>
    public string BankName1 { get; set; } = string.Empty;

    /// <summary>
    /// 银行名称2
    /// </summary>
    public string? BankName2 { get; set; } = string.Empty;

    /// <summary>
    /// 州省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    public string? Province { get; set; } = string.Empty;

    /// <summary>
    /// 地市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    public string? Prefecture { get; set; } = string.Empty;

    /// <summary>
    /// 区县（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=4）
    /// </summary>
    public string? District { get; set; } = string.Empty;

    /// <summary>
    /// 乡镇街道（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=5）
    /// </summary>
    public string? Township { get; set; } = string.Empty;

    /// <summary>
    /// 行政村（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=6）
    /// </summary>
    public string? Village { get; set; } = string.Empty;

    /// <summary>
    /// 地址1（详细地址行1）
    /// </summary>
    public string? Address1 { get; set; } = string.Empty;

    /// <summary>
    /// 地址2（详细地址行2）
    /// </summary>
    public string? Address2 { get; set; } = string.Empty;

    /// <summary>
    /// 银行识别码（SWIFT/BIC；CHAR 11）
    /// </summary>
    public string? SwiftBic { get; set; } = string.Empty;

    /// <summary>
    /// 银行组（；CHAR 2）
    /// </summary>
    public string? BankGroup { get; set; } = string.Empty;

    /// <summary>
    /// 邮政银行往来账户（字典 sys_yes_no）
    /// </summary>
    public int PobkCurAc { get; set; } = 0;

    /// <summary>
    /// 银行编码（；CHAR 15）
    /// </summary>
    public string? BankNumber { get; set; } = string.Empty;

    /// <summary>
    /// 邮政银行（；CHAR 16）
    /// </summary>
    public string? PostalBank { get; set; } = string.Empty;

    /// <summary>
    /// 地址号（；CHAR 10）
    /// </summary>
    public string? AddressNumber { get; set; } = string.Empty;

    /// <summary>
    /// 分行（；CHAR 40）
    /// </summary>
    public string? Branch { get; set; } = string.Empty;

    /// <summary>
    /// 方法（CHAR 4）
    /// </summary>
    public string? BankMethod { get; set; } = string.Empty;

    /// <summary>
    /// 格式（含银行数据文件的格式；CHAR 3）
    /// </summary>
    public string? BankFormat { get; set; } = string.Empty;

    /// <summary>
    /// IBAN 规则（CHAR 6）
    /// </summary>
    public string? IbanRule { get; set; } = string.Empty;

    /// <summary>
    /// 企业间（字典 sys_yes_no）
    /// </summary>
    public int SddB2b { get; set; } = 0;

    /// <summary>
    /// 核心个人（字典 sys_yes_no）
    /// </summary>
    public int SddCore { get; set; } = 0;

    /// <summary>
    /// SEPA拒付交易支持标识（字典 accounting_financial_sepa_rtrans_type）
    /// </summary>
    public int SddRtrans { get; set; } = 0;

    /// <summary>
    /// BIC+ 编码（CHAR 12）
    /// </summary>
    public string? BicPlusNumber { get; set; } = string.Empty;

    /// <summary>
    /// 路径代码（CHAR 15）
    /// </summary>
    public string? PathCode { get; set; } = string.Empty;

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
