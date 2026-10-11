// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Accounting.Financial
// 文件名称：TaktAccountTitleDtos.cs
// 创建时间：2026-08-21
// 创建人：Takt365(Auto Generated)
// 功能描述：AccountTitle 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktAccountTitle 生成，请按需审阅）
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
// AccountTitle 响应 DTO
// ========================================

/// <summary>
/// 会计科目实体
/// 对应前端 TaktAccountTitleDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktAccountTitleDto : TaktCompanyDtoBase
{
    /// <summary>
    /// AccountTitleID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AccountTitleId { get; set; }

    /// <summary>
    /// 科目编码
    /// </summary>
    public string AccountTitleCode { get; set; } = string.Empty;

    /// <summary>
    /// 科目名称
    /// </summary>
    public string AccountTitleName { get; set; } = string.Empty;

    /// <summary>
    /// 父级 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ParentId { get; set; }

    /// <summary>
    /// 科目类型（字典 accounting_financial_account_title_type=资产负债表科目，P=初级成本或收入，S=次级成本，N=非经营性收支，C=现金/银行账户）
    /// </summary>
    public string AccountTitleType { get; set; } = string.Empty;

    /// <summary>
    /// 余额方向（0=借方，1=贷方）
    /// </summary>
    public int BalanceDirection { get; set; } = 0;

    /// <summary>
    /// 科目层级
    /// </summary>
    public int AccountTitleLevel { get; set; } = 0;

    /// <summary>
    /// 末级科目（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int IsLeaf { get; set; } = 0;

    /// <summary>
    /// 辅助核算（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int IsAuxiliary { get; set; } = 0;

    /// <summary>
    /// 辅助核算类型 / 统驭标识（字典 accounting_financial_auxiliary_type；D=客户，K=供应商，A=资产，S=总账无辅助，M=物料）
    /// </summary>
    public string AuxiliaryType { get; set; } = string.Empty;

    /// <summary>
    /// 数量核算（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int IsQuantity { get; set; } = 0;

    /// <summary>
    /// 外币核算（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int IsCurrency { get; set; } = 0;

    /// <summary>
    /// 现金科目（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int IsCash { get; set; } = 0;

    /// <summary>
    /// 银行科目（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int IsBank { get; set; } = 0;

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime ValidFrom { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime ValidTo { get; set; }

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 科目状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int AccountTitleStatus { get; set; } = 0;

}

// ========================================
// AccountTitle 树形响应 DTO
// ========================================

/// <summary>
/// AccountTitle 树形列表/树选择 DTO（含子节点）
/// 对应 GetAccountTitleTreeAsync 等接口
/// </summary>
public class TaktAccountTitleTreeDto : TaktAccountTitleDto
{
    /// <summary>
    /// 子节点（懒加载树接口返回 null，表示尚未加载；勿用空 List 冒充已加载）
    /// </summary>
    public List<TaktAccountTitleTreeDto>? Children { get; set; }
}

// ========================================
// AccountTitle 查询 DTO
// ========================================

/// <summary>
/// AccountTitle 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktAccountTitleQueryDto : TaktPagedQuery
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
    /// 科目编码
    /// </summary>
    public string? AccountTitleCode { get; set; } = string.Empty;

    /// <summary>
    /// 科目名称
    /// </summary>
    public string? AccountTitleName { get; set; } = string.Empty;

    /// <summary>
    /// 父级 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ParentId { get; set; }

    /// <summary>
    /// 科目类型（字典 accounting_financial_account_title_type=资产负债表科目，P=初级成本或收入，S=次级成本，N=非经营性收支，C=现金/银行账户）
    /// </summary>
    public string? AccountTitleType { get; set; } = string.Empty;

    /// <summary>
    /// 余额方向（0=借方，1=贷方）
    /// </summary>
    public int? BalanceDirection { get; set; }

    /// <summary>
    /// 科目层级
    /// </summary>
    public int? AccountTitleLevel { get; set; }

    /// <summary>
    /// 末级科目（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int? IsLeaf { get; set; }

    /// <summary>
    /// 辅助核算（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int? IsAuxiliary { get; set; }

    /// <summary>
    /// 辅助核算类型 / 统驭标识（字典 accounting_financial_auxiliary_type；D=客户，K=供应商，A=资产，S=总账无辅助，M=物料）
    /// </summary>
    public string? AuxiliaryType { get; set; } = string.Empty;

    /// <summary>
    /// 数量核算（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int? IsQuantity { get; set; }

    /// <summary>
    /// 外币核算（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int? IsCurrency { get; set; }

    /// <summary>
    /// 现金科目（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int? IsCash { get; set; }

    /// <summary>
    /// 银行科目（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int? IsBank { get; set; }

    /// <summary>
    /// 生效日期（范围查询-开始）
    /// </summary>
    public DateTime? ValidFromStart { get; set; }

    /// <summary>
    /// 生效日期（范围查询-结束）
    /// </summary>
    public DateTime? ValidFromEnd { get; set; }

    /// <summary>
    /// 失效日期（范围查询-开始）
    /// </summary>
    public DateTime? ValidToStart { get; set; }

    /// <summary>
    /// 失效日期（范围查询-结束）
    /// </summary>
    public DateTime? ValidToEnd { get; set; }

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 科目状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? AccountTitleStatus { get; set; }

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
// 创建AccountTitle DTO
// ========================================

/// <summary>
/// 创建AccountTitle DTO
/// </summary>
public class TaktAccountTitleCreateDto
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
    /// 科目编码
    /// </summary>
    [Required(ErrorMessage = "科目编码不能为空")]
    public string AccountTitleCode { get; set; } = string.Empty;

    /// <summary>
    /// 科目名称
    /// </summary>
    [Required(ErrorMessage = "科目名称不能为空")]
    public string AccountTitleName { get; set; } = string.Empty;

    /// <summary>
    /// 父级 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ParentId { get; set; }

    /// <summary>
    /// 科目类型（字典 accounting_financial_account_title_type=资产负债表科目，P=初级成本或收入，S=次级成本，N=非经营性收支，C=现金/银行账户）
    /// </summary>
    [Required(ErrorMessage = "科目类型（字典 accounting_financial_account_title_type=资产负债表科目，P=初级成本或收入，S=次级成本，N=非经营性收支，C=现金/银行账户）不能为空")]
    public string AccountTitleType { get; set; } = string.Empty;

    /// <summary>
    /// 余额方向（0=借方，1=贷方）
    /// </summary>
    public int BalanceDirection { get; set; } = 0;

    /// <summary>
    /// 科目层级
    /// </summary>
    public int AccountTitleLevel { get; set; } = 0;

    /// <summary>
    /// 辅助核算（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int IsAuxiliary { get; set; } = 0;

    /// <summary>
    /// 辅助核算类型 / 统驭标识（字典 accounting_financial_auxiliary_type；D=客户，K=供应商，A=资产，S=总账无辅助，M=物料）
    /// </summary>
    [Required(ErrorMessage = "辅助核算类型 / 统驭标识（字典 accounting_financial_auxiliary_type；D=客户，K=供应商，A=资产，S=总账无辅助，M=物料）不能为空")]
    public string AuxiliaryType { get; set; } = string.Empty;

    /// <summary>
    /// 数量核算（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int IsQuantity { get; set; } = 0;

    /// <summary>
    /// 外币核算（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int IsCurrency { get; set; } = 0;

    /// <summary>
    /// 现金科目（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int IsCash { get; set; } = 0;

    /// <summary>
    /// 银行科目（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int IsBank { get; set; } = 0;

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime ValidFrom { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime ValidTo { get; set; }

    /// <summary>
    /// 科目状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int AccountTitleStatus { get; set; } = 0;

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
// 更新AccountTitle DTO
// ========================================

/// <summary>
/// 更新AccountTitle DTO
/// 继承 TaktAccountTitleCreateDto，添加 AccountTitleId 字段
/// </summary>
public class TaktAccountTitleUpdateDto : TaktAccountTitleCreateDto
{
    /// <summary>
    /// AccountTitleID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AccountTitleId { get; set; }

}

// ========================================
// AccountTitle 状态 DTO
// ========================================

/// <summary>
/// AccountTitle 状态更新 DTO
/// </summary>
public class TaktAccountTitleStatusDto
{
    /// <summary>
    /// AccountTitleID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AccountTitleId { get; set; }

    /// <summary>
    /// 科目状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [Required(ErrorMessage = "科目状态（字典 sys_normal_disable；1=启用，0=禁用）不能为空")]
    public int AccountTitleStatus { get; set; } = 0;
}

// ========================================
// AccountTitle 排序 DTO
// ========================================

/// <summary>
/// AccountTitle 排序更新 DTO
/// </summary>
public class TaktAccountTitleSortDto
{
    /// <summary>
    /// AccountTitleID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AccountTitleId { get; set; }

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [Required(ErrorMessage = "排序号（回填）不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// AccountTitle 导入模板行 DTO
/// </summary>
public class TaktAccountTitleTemplateDto
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
    /// 科目编码
    /// </summary>
    public string? AccountTitleCode { get; set; } = string.Empty;

    /// <summary>
    /// 科目名称
    /// </summary>
    public string? AccountTitleName { get; set; } = string.Empty;

    /// <summary>
    /// 父级 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ParentId { get; set; }

    /// <summary>
    /// 科目类型（字典 accounting_financial_account_title_type=资产负债表科目，P=初级成本或收入，S=次级成本，N=非经营性收支，C=现金/银行账户）
    /// </summary>
    public string? AccountTitleType { get; set; } = string.Empty;

    /// <summary>
    /// 余额方向（0=借方，1=贷方）
    /// </summary>
    public int? BalanceDirection { get; set; }

    /// <summary>
    /// 科目层级
    /// </summary>
    public int? AccountTitleLevel { get; set; }

    /// <summary>
    /// 辅助核算（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int? IsAuxiliary { get; set; }

    /// <summary>
    /// 辅助核算类型 / 统驭标识（字典 accounting_financial_auxiliary_type；D=客户，K=供应商，A=资产，S=总账无辅助，M=物料）
    /// </summary>
    public string? AuxiliaryType { get; set; } = string.Empty;

    /// <summary>
    /// 数量核算（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int? IsQuantity { get; set; }

    /// <summary>
    /// 外币核算（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int? IsCurrency { get; set; }

    /// <summary>
    /// 现金科目（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int? IsCash { get; set; }

    /// <summary>
    /// 银行科目（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int? IsBank { get; set; }

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime? ValidFrom { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime? ValidTo { get; set; }

    /// <summary>
    /// 科目状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? AccountTitleStatus { get; set; }

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
/// AccountTitle 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktAccountTitleImportDto
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
    /// 科目编码
    /// </summary>
    public string? AccountTitleCode { get; set; } = string.Empty;

    /// <summary>
    /// 科目名称
    /// </summary>
    public string? AccountTitleName { get; set; } = string.Empty;

    /// <summary>
    /// 父级 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ParentId { get; set; }

    /// <summary>
    /// 科目类型（字典 accounting_financial_account_title_type=资产负债表科目，P=初级成本或收入，S=次级成本，N=非经营性收支，C=现金/银行账户）
    /// </summary>
    public string? AccountTitleType { get; set; } = string.Empty;

    /// <summary>
    /// 余额方向（0=借方，1=贷方）
    /// </summary>
    public int? BalanceDirection { get; set; }

    /// <summary>
    /// 科目层级
    /// </summary>
    public int? AccountTitleLevel { get; set; }

    /// <summary>
    /// 辅助核算（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int? IsAuxiliary { get; set; }

    /// <summary>
    /// 辅助核算类型 / 统驭标识（字典 accounting_financial_auxiliary_type；D=客户，K=供应商，A=资产，S=总账无辅助，M=物料）
    /// </summary>
    public string? AuxiliaryType { get; set; } = string.Empty;

    /// <summary>
    /// 数量核算（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int? IsQuantity { get; set; }

    /// <summary>
    /// 外币核算（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int? IsCurrency { get; set; }

    /// <summary>
    /// 现金科目（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int? IsCash { get; set; }

    /// <summary>
    /// 银行科目（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int? IsBank { get; set; }

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime? ValidFrom { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime? ValidTo { get; set; }

    /// <summary>
    /// 科目状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? AccountTitleStatus { get; set; }

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
/// AccountTitle 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktAccountTitleExportDto
{
    /// <summary>
    /// AccountTitleID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AccountTitleId { get; set; }

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
    /// 科目编码
    /// </summary>
    public string AccountTitleCode { get; set; } = string.Empty;

    /// <summary>
    /// 科目名称
    /// </summary>
    public string AccountTitleName { get; set; } = string.Empty;

    /// <summary>
    /// 父级 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ParentId { get; set; }

    /// <summary>
    /// 科目类型（字典 accounting_financial_account_title_type=资产负债表科目，P=初级成本或收入，S=次级成本，N=非经营性收支，C=现金/银行账户）
    /// </summary>
    public string AccountTitleType { get; set; } = string.Empty;

    /// <summary>
    /// 余额方向（0=借方，1=贷方）
    /// </summary>
    public int BalanceDirection { get; set; } = 0;

    /// <summary>
    /// 科目层级
    /// </summary>
    public int AccountTitleLevel { get; set; } = 0;

    /// <summary>
    /// 末级科目（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int IsLeaf { get; set; } = 0;

    /// <summary>
    /// 辅助核算（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int IsAuxiliary { get; set; } = 0;

    /// <summary>
    /// 辅助核算类型 / 统驭标识（字典 accounting_financial_auxiliary_type；D=客户，K=供应商，A=资产，S=总账无辅助，M=物料）
    /// </summary>
    public string AuxiliaryType { get; set; } = string.Empty;

    /// <summary>
    /// 数量核算（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int IsQuantity { get; set; } = 0;

    /// <summary>
    /// 外币核算（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int IsCurrency { get; set; } = 0;

    /// <summary>
    /// 现金科目（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int IsCash { get; set; } = 0;

    /// <summary>
    /// 银行科目（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int IsBank { get; set; } = 0;

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime ValidFrom { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime ValidTo { get; set; }

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 科目状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int AccountTitleStatus { get; set; } = 0;

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
