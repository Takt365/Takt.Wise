// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.HumanResource.Compensation
// 文件名称：TaktSalaryItemDtos.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：SalaryItem 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSalaryItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.HumanResource.Compensation;

// ========================================
// SalaryItem 响应 DTO
// ========================================

/// <summary>
/// 薪资项目（现金报酬可配置主数据，含股权激励；不另建 TaktStockOption 等平行实体）
/// 对应前端 TaktSalaryItemDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSalaryItemDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SalaryItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalaryItemId { get; set; }


    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int ItemStatus { get; set; } = 0;

}

// ========================================
// SalaryItem 查询 DTO
// ========================================

/// <summary>
/// SalaryItem 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSalaryItemQueryDto : TaktPagedQuery
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
    /// 项目编码（租户+公司内唯一）
    /// </summary>
    public string? ItemCode { get; set; } = string.Empty;

    /// <summary>
    /// 项目名称
    /// </summary>
    public string? ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 简称
    /// </summary>
    public string? ShortName { get; set; } = string.Empty;

    /// <summary>
    /// 项目类型（字典 humanresource_compensation_salary_item_type：基本工资/岗位工资/津贴/奖金/股权激励等）
    /// </summary>
    public int? ItemType { get; set; }

    /// <summary>
    /// 计算方式（字典 humanresource_compensation_salary_calc_method：固定金额/按比例/按公式）
    /// </summary>
    public int? CalcMethod { get; set; }

    /// <summary>
    /// 关联计算公式步骤 ID（calc_method 为按公式时引用 TaktSalaryFormula 单行；整单核算用 formula_set_code）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalaryFormulaId { get; set; }

    /// <summary>
    /// 默认金额（元）
    /// </summary>
    public decimal? DefaultAmount { get; set; }

    /// <summary>
    /// 默认比例（%，0~100）
    /// </summary>
    public decimal? DefaultRate { get; set; }

    /// <summary>
    /// 默认行权/授予价格（元；item_type 为股权激励时使用）
    /// </summary>
    public decimal? StrikePrice { get; set; }

    /// <summary>
    /// 默认归属年限（年；item_type 为股权激励时使用）
    /// </summary>
    public int? VestingYears { get; set; }

    /// <summary>
    /// 是否扣款项（字典 sys_yes_no）
    /// </summary>
    public int? IsDeduction { get; set; }

    /// <summary>
    /// 是否计入应税所得（字典 sys_yes_no）
    /// </summary>
    public int? IsTaxable { get; set; }

    /// <summary>
    /// 是否计入社保基数（字典 sys_yes_no）
    /// </summary>
    public int? IncludeSocialSecurityBase { get; set; }

    /// <summary>
    /// 是否计入公积金基数（字典 sys_yes_no）
    /// </summary>
    public int? IncludeHousingFundBase { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int? ItemStatus { get; set; }

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
// 创建SalaryItem DTO
// ========================================

/// <summary>
/// 创建SalaryItem DTO
/// </summary>
public class TaktSalaryItemCreateDto
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
    /// 项目编码（租户+公司内唯一）
    /// </summary>
    [Required(ErrorMessage = "项目编码（租户+公司内唯一）不能为空")]
    public string ItemCode { get; set; } = string.Empty;

    /// <summary>
    /// 项目名称
    /// </summary>
    [Required(ErrorMessage = "项目名称不能为空")]
    public string ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 简称
    /// </summary>
    public string? ShortName { get; set; } = string.Empty;

    /// <summary>
    /// 项目类型（字典 humanresource_compensation_salary_item_type：基本工资/岗位工资/津贴/奖金/股权激励等）
    /// </summary>
    public int ItemType { get; set; } = 0;

    /// <summary>
    /// 计算方式（字典 humanresource_compensation_salary_calc_method：固定金额/按比例/按公式）
    /// </summary>
    public int CalcMethod { get; set; } = 0;

    /// <summary>
    /// 关联计算公式步骤 ID（calc_method 为按公式时引用 TaktSalaryFormula 单行；整单核算用 formula_set_code）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalaryFormulaId { get; set; }

    /// <summary>
    /// 默认金额（元）
    /// </summary>
    public decimal DefaultAmount { get; set; }

    /// <summary>
    /// 默认比例（%，0~100）
    /// </summary>
    public decimal DefaultRate { get; set; }

    /// <summary>
    /// 默认行权/授予价格（元；item_type 为股权激励时使用）
    /// </summary>
    public decimal StrikePrice { get; set; }

    /// <summary>
    /// 默认归属年限（年；item_type 为股权激励时使用）
    /// </summary>
    public int VestingYears { get; set; } = 0;

    /// <summary>
    /// 是否扣款项（字典 sys_yes_no）
    /// </summary>
    public int IsDeduction { get; set; } = 0;

    /// <summary>
    /// 是否计入应税所得（字典 sys_yes_no）
    /// </summary>
    public int IsTaxable { get; set; } = 0;

    /// <summary>
    /// 是否计入社保基数（字典 sys_yes_no）
    /// </summary>
    public int IncludeSocialSecurityBase { get; set; } = 0;

    /// <summary>
    /// 是否计入公积金基数（字典 sys_yes_no）
    /// </summary>
    public int IncludeHousingFundBase { get; set; } = 0;

    /// <summary>
    /// 关联工厂
    /// </summary>
    [Required(ErrorMessage = "关联工厂不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int ItemStatus { get; set; } = 0;

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
// 更新SalaryItem DTO
// ========================================

/// <summary>
/// 更新SalaryItem DTO
/// 继承 TaktSalaryItemCreateDto，添加 SalaryItemId 字段
/// </summary>
public class TaktSalaryItemUpdateDto : TaktSalaryItemCreateDto
{
    /// <summary>
    /// SalaryItemID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalaryItemId { get; set; }

}

// ========================================
// SalaryItem 状态 DTO
// ========================================

/// <summary>
/// SalaryItem 状态更新 DTO
/// </summary>
public class TaktSalaryItemStatusDto
{
    /// <summary>
    /// SalaryItemID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalaryItemId { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    [Required(ErrorMessage = "状态（字典 sys_normal_disable）不能为空")]
    public int ItemStatus { get; set; } = 0;
}

// ========================================
// SalaryItem 排序 DTO
// ========================================

/// <summary>
/// SalaryItem 排序更新 DTO
/// </summary>
public class TaktSalaryItemSortDto
{
    /// <summary>
    /// SalaryItemID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalaryItemId { get; set; }

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [Required(ErrorMessage = "排序号不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SalaryItem 导入模板行 DTO
/// </summary>
public class TaktSalaryItemTemplateDto
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
    /// 项目编码（租户+公司内唯一）
    /// </summary>
    public string? ItemCode { get; set; } = string.Empty;

    /// <summary>
    /// 项目名称
    /// </summary>
    public string? ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 简称
    /// </summary>
    public string? ShortName { get; set; } = string.Empty;

    /// <summary>
    /// 项目类型（字典 humanresource_compensation_salary_item_type：基本工资/岗位工资/津贴/奖金/股权激励等）
    /// </summary>
    public int? ItemType { get; set; }

    /// <summary>
    /// 计算方式（字典 humanresource_compensation_salary_calc_method：固定金额/按比例/按公式）
    /// </summary>
    public int? CalcMethod { get; set; }

    /// <summary>
    /// 关联计算公式步骤 ID（calc_method 为按公式时引用 TaktSalaryFormula 单行；整单核算用 formula_set_code）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalaryFormulaId { get; set; }

    /// <summary>
    /// 默认金额（元）
    /// </summary>
    public decimal? DefaultAmount { get; set; }

    /// <summary>
    /// 默认比例（%，0~100）
    /// </summary>
    public decimal? DefaultRate { get; set; }

    /// <summary>
    /// 默认行权/授予价格（元；item_type 为股权激励时使用）
    /// </summary>
    public decimal? StrikePrice { get; set; }

    /// <summary>
    /// 默认归属年限（年；item_type 为股权激励时使用）
    /// </summary>
    public int? VestingYears { get; set; }

    /// <summary>
    /// 是否扣款项（字典 sys_yes_no）
    /// </summary>
    public int? IsDeduction { get; set; }

    /// <summary>
    /// 是否计入应税所得（字典 sys_yes_no）
    /// </summary>
    public int? IsTaxable { get; set; }

    /// <summary>
    /// 是否计入社保基数（字典 sys_yes_no）
    /// </summary>
    public int? IncludeSocialSecurityBase { get; set; }

    /// <summary>
    /// 是否计入公积金基数（字典 sys_yes_no）
    /// </summary>
    public int? IncludeHousingFundBase { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int? ItemStatus { get; set; }

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
/// SalaryItem 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSalaryItemImportDto
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
    /// 项目编码（租户+公司内唯一）
    /// </summary>
    public string? ItemCode { get; set; } = string.Empty;

    /// <summary>
    /// 项目名称
    /// </summary>
    public string? ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 简称
    /// </summary>
    public string? ShortName { get; set; } = string.Empty;

    /// <summary>
    /// 项目类型（字典 humanresource_compensation_salary_item_type：基本工资/岗位工资/津贴/奖金/股权激励等）
    /// </summary>
    public int? ItemType { get; set; }

    /// <summary>
    /// 计算方式（字典 humanresource_compensation_salary_calc_method：固定金额/按比例/按公式）
    /// </summary>
    public int? CalcMethod { get; set; }

    /// <summary>
    /// 关联计算公式步骤 ID（calc_method 为按公式时引用 TaktSalaryFormula 单行；整单核算用 formula_set_code）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalaryFormulaId { get; set; }

    /// <summary>
    /// 默认金额（元）
    /// </summary>
    public decimal? DefaultAmount { get; set; }

    /// <summary>
    /// 默认比例（%，0~100）
    /// </summary>
    public decimal? DefaultRate { get; set; }

    /// <summary>
    /// 默认行权/授予价格（元；item_type 为股权激励时使用）
    /// </summary>
    public decimal? StrikePrice { get; set; }

    /// <summary>
    /// 默认归属年限（年；item_type 为股权激励时使用）
    /// </summary>
    public int? VestingYears { get; set; }

    /// <summary>
    /// 是否扣款项（字典 sys_yes_no）
    /// </summary>
    public int? IsDeduction { get; set; }

    /// <summary>
    /// 是否计入应税所得（字典 sys_yes_no）
    /// </summary>
    public int? IsTaxable { get; set; }

    /// <summary>
    /// 是否计入社保基数（字典 sys_yes_no）
    /// </summary>
    public int? IncludeSocialSecurityBase { get; set; }

    /// <summary>
    /// 是否计入公积金基数（字典 sys_yes_no）
    /// </summary>
    public int? IncludeHousingFundBase { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int? ItemStatus { get; set; }

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
/// SalaryItem 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSalaryItemExportDto
{
    /// <summary>
    /// SalaryItemID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalaryItemId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 项目编码（租户+公司内唯一）
    /// </summary>
    public string ItemCode { get; set; } = string.Empty;

    /// <summary>
    /// 项目名称
    /// </summary>
    public string ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 简称
    /// </summary>
    public string? ShortName { get; set; } = string.Empty;

    /// <summary>
    /// 项目类型（字典 humanresource_compensation_salary_item_type：基本工资/岗位工资/津贴/奖金/股权激励等）
    /// </summary>
    public int ItemType { get; set; } = 0;

    /// <summary>
    /// 计算方式（字典 humanresource_compensation_salary_calc_method：固定金额/按比例/按公式）
    /// </summary>
    public int CalcMethod { get; set; } = 0;

    /// <summary>
    /// 关联计算公式步骤 ID（calc_method 为按公式时引用 TaktSalaryFormula 单行；整单核算用 formula_set_code）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalaryFormulaId { get; set; }

    /// <summary>
    /// 默认金额（元）
    /// </summary>
    public decimal DefaultAmount { get; set; }

    /// <summary>
    /// 默认比例（%，0~100）
    /// </summary>
    public decimal DefaultRate { get; set; }

    /// <summary>
    /// 默认行权/授予价格（元；item_type 为股权激励时使用）
    /// </summary>
    public decimal StrikePrice { get; set; }

    /// <summary>
    /// 默认归属年限（年；item_type 为股权激励时使用）
    /// </summary>
    public int VestingYears { get; set; } = 0;

    /// <summary>
    /// 是否扣款项（字典 sys_yes_no）
    /// </summary>
    public int IsDeduction { get; set; } = 0;

    /// <summary>
    /// 是否计入应税所得（字典 sys_yes_no）
    /// </summary>
    public int IsTaxable { get; set; } = 0;

    /// <summary>
    /// 是否计入社保基数（字典 sys_yes_no）
    /// </summary>
    public int IncludeSocialSecurityBase { get; set; } = 0;

    /// <summary>
    /// 是否计入公积金基数（字典 sys_yes_no）
    /// </summary>
    public int IncludeHousingFundBase { get; set; } = 0;

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int ItemStatus { get; set; } = 0;

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
