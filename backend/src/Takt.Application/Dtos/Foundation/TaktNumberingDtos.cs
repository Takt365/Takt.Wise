// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Foundation
// 文件名称：TaktNumberingDtos.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：Numbering 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktNumbering 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Foundation;

// ========================================
// Numbering 响应 DTO
// ========================================

/// <summary>
/// 编码规则实体 定义系统中各类业务单据的编码生成规则，如：订单号、合同号、发票号等 支持灵活的前缀、日期格式、流水号组合 编码顺序：单据类型-公司-部门-前缀-日期-流水号 示例：order-1000-DEPT01-SO-20250120-000001
/// 对应前端 TaktNumberingDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktNumberingDto : TaktCompanyDtoBase
{
    /// <summary>
    /// NumberingID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long NumberingId { get; set; }

    /// <summary>
    /// 规则编码（唯一索引：租户+公司内唯一，见 ix_numbering_code_unique；如 SO, PO, CONTRACT）
    /// </summary>
    public string RuleCode { get; set; } = string.Empty;

    /// <summary>
    /// 规则名称（如：销售订单号、采购订单号）
    /// </summary>
    public string RuleName { get; set; } = string.Empty;

    /// <summary>
    /// 单据类型（关联 TaktMenu.MenuName，选项 TaktMenus/tree-options?valueBy=name）
    /// </summary>
    public string DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// 部门编码（关联 TaktDept.IsoCode，选项 TaktDepts/iso-tree-options）
    /// </summary>
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 前缀编码（如：PUR、SORD、ANN）
    /// </summary>
    public string? PrefixCode { get; set; } = string.Empty;

    /// <summary>
    /// 日期格式（字典 sys_numbering_date_format；none/空=不使用日期；yyyy、yyyyMM、yyyyMMdd、yyyyMMddHH；须与 reset_period 粒度匹配）
    /// </summary>
    public string? DateFormat { get; set; } = string.Empty;

    /// <summary>
    /// 流水位数（3=001, 4=0001, 5=00001, 6=000001）
    /// </summary>
    public int SequenceLength { get; set; } = 0;

    /// <summary>
    /// 流水步长（每次递增的数值，默认1）
    /// </summary>
    public int SequenceStep { get; set; } = 0;

    /// <summary>
    /// 后缀编码（可选，最多 4 位）
    /// </summary>
    public string? SuffixCode { get; set; } = string.Empty;

    /// <summary>
    /// 重置周期（字典 sys_reset_period；DictValue=None|Annually|Monthly|Daily；须与 date_format 粒度匹配）
    /// </summary>
    public string ResetPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 起始编码（新建时为首次应发样例如 2300-CLT-000001；取号后更新为最近一次产出编码）
    /// </summary>
    public string StartingNumber { get; set; } = string.Empty;

    /// <summary>
    /// 分隔符（空=段直接拼接；-=连字符分隔，默认 -）
    /// </summary>
    public string? Separator { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 描述说明；可选配置编码段顺序，格式：segments:CompanyCode,DeptCode,PrefixCode,DateSequence（段名为实体属性名）
    /// </summary>
    public string? NumberingDescription { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int NumberingStatus { get; set; } = 0;

}

// ========================================
// Numbering 查询 DTO
// ========================================

/// <summary>
/// Numbering 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktNumberingQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 规则编码（唯一索引：租户+公司内唯一，见 ix_numbering_code_unique；如 SO, PO, CONTRACT）
    /// </summary>
    public string? RuleCode { get; set; } = string.Empty;

    /// <summary>
    /// 规则名称（如：销售订单号、采购订单号）
    /// </summary>
    public string? RuleName { get; set; } = string.Empty;

    /// <summary>
    /// 单据类型（关联 TaktMenu.MenuName，选项 TaktMenus/tree-options?valueBy=name）
    /// </summary>
    public string? DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// 部门编码（关联 TaktDept.IsoCode，选项 TaktDepts/iso-tree-options）
    /// </summary>
    public string? DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 前缀编码（如：PUR、SORD、ANN）
    /// </summary>
    public string? PrefixCode { get; set; } = string.Empty;

    /// <summary>
    /// 日期格式（字典 sys_numbering_date_format；none/空=不使用日期；yyyy、yyyyMM、yyyyMMdd、yyyyMMddHH；须与 reset_period 粒度匹配）
    /// </summary>
    public string? DateFormat { get; set; } = string.Empty;

    /// <summary>
    /// 流水位数（3=001, 4=0001, 5=00001, 6=000001）
    /// </summary>
    public int? SequenceLength { get; set; }

    /// <summary>
    /// 流水步长（每次递增的数值，默认1）
    /// </summary>
    public int? SequenceStep { get; set; }

    /// <summary>
    /// 后缀编码（可选，最多 4 位）
    /// </summary>
    public string? SuffixCode { get; set; } = string.Empty;

    /// <summary>
    /// 重置周期（字典 sys_reset_period；DictValue=None|Annually|Monthly|Daily；须与 date_format 粒度匹配）
    /// </summary>
    public string? ResetPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 起始编码（新建时为首次应发样例如 2300-CLT-000001；取号后更新为最近一次产出编码）
    /// </summary>
    public string? StartingNumber { get; set; } = string.Empty;

    /// <summary>
    /// 分隔符（空=段直接拼接；-=连字符分隔，默认 -）
    /// </summary>
    public string? Separator { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 描述说明；可选配置编码段顺序，格式：segments:CompanyCode,DeptCode,PrefixCode,DateSequence（段名为实体属性名）
    /// </summary>
    public string? NumberingDescription { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int? NumberingStatus { get; set; }

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
// 创建Numbering DTO
// ========================================

/// <summary>
/// 创建Numbering DTO
/// </summary>
public class TaktNumberingCreateDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 规则编码（唯一索引：租户+公司内唯一，见 ix_numbering_code_unique；如 SO, PO, CONTRACT）
    /// </summary>
    [Required(ErrorMessage = "规则编码（唯一索引：租户+公司内唯一，见 ix_numbering_code_unique；如 SO, PO, CONTRACT）不能为空")]
    public string RuleCode { get; set; } = string.Empty;

    /// <summary>
    /// 规则名称（如：销售订单号、采购订单号）
    /// </summary>
    [Required(ErrorMessage = "规则名称（如：销售订单号、采购订单号）不能为空")]
    public string RuleName { get; set; } = string.Empty;

    /// <summary>
    /// 单据类型（关联 TaktMenu.MenuName，选项 TaktMenus/tree-options?valueBy=name）
    /// </summary>
    [Required(ErrorMessage = "单据类型（关联 TaktMenu.MenuName，选项 TaktMenus/tree-options?valueBy=name）不能为空")]
    public string DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// 部门编码（关联 TaktDept.IsoCode，选项 TaktDepts/iso-tree-options）
    /// </summary>
    [Required(ErrorMessage = "部门编码（关联 TaktDept.IsoCode，选项 TaktDepts/iso-tree-options）不能为空")]
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 前缀编码（如：PUR、SORD、ANN）
    /// </summary>
    public string? PrefixCode { get; set; } = string.Empty;

    /// <summary>
    /// 日期格式（字典 sys_numbering_date_format；none/空=不使用日期；yyyy、yyyyMM、yyyyMMdd、yyyyMMddHH；须与 reset_period 粒度匹配）
    /// </summary>
    public string? DateFormat { get; set; } = string.Empty;

    /// <summary>
    /// 流水位数（3=001, 4=0001, 5=00001, 6=000001）
    /// </summary>
    public int SequenceLength { get; set; } = 0;

    /// <summary>
    /// 流水步长（每次递增的数值，默认1）
    /// </summary>
    public int SequenceStep { get; set; } = 0;

    /// <summary>
    /// 后缀编码（可选，最多 4 位）
    /// </summary>
    public string? SuffixCode { get; set; } = string.Empty;

    /// <summary>
    /// 重置周期（字典 sys_reset_period；DictValue=None|Annually|Monthly|Daily；须与 date_format 粒度匹配）
    /// </summary>
    [Required(ErrorMessage = "重置周期（字典 sys_reset_period；DictValue=None|Annually|Monthly|Daily）不能为空")]
    public string ResetPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 起始编码（新建时为首次应发样例如 2300-CLT-000001；取号后更新为最近一次产出编码）
    /// </summary>
    [Required(ErrorMessage = "起始编码不能为空")]
    public string StartingNumber { get; set; } = string.Empty;

    /// <summary>
    /// 分隔符（空=段直接拼接；-=连字符分隔，默认 -）
    /// </summary>
    public string? Separator { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 描述说明；可选配置编码段顺序，格式：segments:CompanyCode,DeptCode,PrefixCode,DateSequence（段名为实体属性名）
    /// </summary>
    public string? NumberingDescription { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int NumberingStatus { get; set; } = 0;

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
// 更新Numbering DTO
// ========================================

/// <summary>
/// 更新Numbering DTO
/// 继承 TaktNumberingCreateDto，添加 NumberingId 字段
/// </summary>
public class TaktNumberingUpdateDto : TaktNumberingCreateDto
{
    /// <summary>
    /// NumberingID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long NumberingId { get; set; }

}

// ========================================
// Numbering 状态 DTO
// ========================================

/// <summary>
/// Numbering 状态更新 DTO
/// </summary>
public class TaktNumberingStatusDto
{
    /// <summary>
    /// NumberingID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long NumberingId { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    [Required(ErrorMessage = "状态（字典 sys_normal_disable；1=启用 0=禁用）不能为空")]
    public int NumberingStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Numbering 导入模板行 DTO
/// </summary>
public class TaktNumberingTemplateDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 规则编码（唯一索引：租户+公司内唯一，见 ix_numbering_code_unique；如 SO, PO, CONTRACT）
    /// </summary>
    public string? RuleCode { get; set; } = string.Empty;

    /// <summary>
    /// 规则名称（如：销售订单号、采购订单号）
    /// </summary>
    public string? RuleName { get; set; } = string.Empty;

    /// <summary>
    /// 单据类型（关联 TaktMenu.MenuName，选项 TaktMenus/tree-options?valueBy=name）
    /// </summary>
    public string? DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// 部门编码（关联 TaktDept.IsoCode，选项 TaktDepts/iso-tree-options）
    /// </summary>
    public string? DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 前缀编码（如：PUR、SORD、ANN）
    /// </summary>
    public string? PrefixCode { get; set; } = string.Empty;

    /// <summary>
    /// 日期格式（字典 sys_numbering_date_format；none/空=不使用日期；yyyy、yyyyMM、yyyyMMdd、yyyyMMddHH；须与 reset_period 粒度匹配）
    /// </summary>
    public string? DateFormat { get; set; } = string.Empty;

    /// <summary>
    /// 流水位数（3=001, 4=0001, 5=00001, 6=000001）
    /// </summary>
    public int? SequenceLength { get; set; }

    /// <summary>
    /// 流水步长（每次递增的数值，默认1）
    /// </summary>
    public int? SequenceStep { get; set; }

    /// <summary>
    /// 后缀编码（可选，最多 4 位）
    /// </summary>
    public string? SuffixCode { get; set; } = string.Empty;

    /// <summary>
    /// 重置周期（字典 sys_reset_period；DictValue=None|Annually|Monthly|Daily；须与 date_format 粒度匹配）
    /// </summary>
    public string? ResetPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 起始编码（新建时为首次应发样例如 2300-CLT-000001；取号后更新为最近一次产出编码）
    /// </summary>
    public string? StartingNumber { get; set; } = string.Empty;

    /// <summary>
    /// 分隔符（空=段直接拼接；-=连字符分隔，默认 -）
    /// </summary>
    public string? Separator { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 描述说明；可选配置编码段顺序，格式：segments:CompanyCode,DeptCode,PrefixCode,DateSequence（段名为实体属性名）
    /// </summary>
    public string? NumberingDescription { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int? NumberingStatus { get; set; }

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
/// Numbering 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktNumberingImportDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 规则编码（唯一索引：租户+公司内唯一，见 ix_numbering_code_unique；如 SO, PO, CONTRACT）
    /// </summary>
    public string? RuleCode { get; set; } = string.Empty;

    /// <summary>
    /// 规则名称（如：销售订单号、采购订单号）
    /// </summary>
    public string? RuleName { get; set; } = string.Empty;

    /// <summary>
    /// 单据类型（关联 TaktMenu.MenuName，选项 TaktMenus/tree-options?valueBy=name）
    /// </summary>
    public string? DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// 部门编码（关联 TaktDept.IsoCode，选项 TaktDepts/iso-tree-options）
    /// </summary>
    public string? DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 前缀编码（如：PUR、SORD、ANN）
    /// </summary>
    public string? PrefixCode { get; set; } = string.Empty;

    /// <summary>
    /// 日期格式（字典 sys_numbering_date_format；none/空=不使用日期；yyyy、yyyyMM、yyyyMMdd、yyyyMMddHH；须与 reset_period 粒度匹配）
    /// </summary>
    public string? DateFormat { get; set; } = string.Empty;

    /// <summary>
    /// 流水位数（3=001, 4=0001, 5=00001, 6=000001）
    /// </summary>
    public int? SequenceLength { get; set; }

    /// <summary>
    /// 流水步长（每次递增的数值，默认1）
    /// </summary>
    public int? SequenceStep { get; set; }

    /// <summary>
    /// 后缀编码（可选，最多 4 位）
    /// </summary>
    public string? SuffixCode { get; set; } = string.Empty;

    /// <summary>
    /// 重置周期（字典 sys_reset_period；DictValue=None|Annually|Monthly|Daily；须与 date_format 粒度匹配）
    /// </summary>
    public string? ResetPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 起始编码（新建时为首次应发样例如 2300-CLT-000001；取号后更新为最近一次产出编码）
    /// </summary>
    public string? StartingNumber { get; set; } = string.Empty;

    /// <summary>
    /// 分隔符（空=段直接拼接；-=连字符分隔，默认 -）
    /// </summary>
    public string? Separator { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 描述说明；可选配置编码段顺序，格式：segments:CompanyCode,DeptCode,PrefixCode,DateSequence（段名为实体属性名）
    /// </summary>
    public string? NumberingDescription { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int? NumberingStatus { get; set; }

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
// 预览取号 DTO
// ========================================

/// <summary>
/// 预览下一个业务编码查询参数
/// </summary>
public class TaktNumberingPreviewQueryDto
{
    /// <summary>
    /// 规则编码（TaktNumbering.RuleCode）
    /// </summary>
    public string RuleCode { get; set; } = string.Empty;
}

/// <summary>
/// 预览下一个业务编码结果（不占用流水号）
/// </summary>
public class TaktNumberingPreviewDto
{
    /// <summary>
    /// 规则编码
    /// </summary>
    public string RuleCode { get; set; } = string.Empty;

    /// <summary>
    /// 预览业务编码
    /// </summary>
    public string BusinessCode { get; set; } = string.Empty;
}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// Numbering 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktNumberingExportDto
{
    /// <summary>
    /// NumberingID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long NumberingId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 规则编码（唯一索引：租户+公司内唯一，见 ix_numbering_code_unique；如 SO, PO, CONTRACT）
    /// </summary>
    public string RuleCode { get; set; } = string.Empty;

    /// <summary>
    /// 规则名称（如：销售订单号、采购订单号）
    /// </summary>
    public string RuleName { get; set; } = string.Empty;

    /// <summary>
    /// 单据类型（关联 TaktMenu.MenuName，选项 TaktMenus/tree-options?valueBy=name）
    /// </summary>
    public string DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// 部门编码（关联 TaktDept.IsoCode，选项 TaktDepts/iso-tree-options）
    /// </summary>
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 前缀编码（如：PUR、SORD、ANN）
    /// </summary>
    public string? PrefixCode { get; set; } = string.Empty;

    /// <summary>
    /// 日期格式（字典 sys_numbering_date_format；none/空=不使用日期；yyyy、yyyyMM、yyyyMMdd、yyyyMMddHH；须与 reset_period 粒度匹配）
    /// </summary>
    public string? DateFormat { get; set; } = string.Empty;

    /// <summary>
    /// 流水位数（3=001, 4=0001, 5=00001, 6=000001）
    /// </summary>
    public int SequenceLength { get; set; } = 0;

    /// <summary>
    /// 流水步长（每次递增的数值，默认1）
    /// </summary>
    public int SequenceStep { get; set; } = 0;

    /// <summary>
    /// 后缀编码（可选，最多 4 位）
    /// </summary>
    public string? SuffixCode { get; set; } = string.Empty;

    /// <summary>
    /// 重置周期（字典 sys_reset_period；DictValue=None|Annually|Monthly|Daily；须与 date_format 粒度匹配）
    /// </summary>
    public string ResetPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 起始编码（新建时为首次应发样例如 2300-CLT-000001；取号后更新为最近一次产出编码）
    /// </summary>
    public string StartingNumber { get; set; } = string.Empty;

    /// <summary>
    /// 分隔符（空=段直接拼接；-=连字符分隔，默认 -）
    /// </summary>
    public string? Separator { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 描述说明；可选配置编码段顺序，格式：segments:CompanyCode,DeptCode,PrefixCode,DateSequence（段名为实体属性名）
    /// </summary>
    public string? NumberingDescription { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int NumberingStatus { get; set; } = 0;

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
