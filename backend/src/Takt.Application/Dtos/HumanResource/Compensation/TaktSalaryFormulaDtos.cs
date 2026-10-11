// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.HumanResource.Compensation
// 文件名称：TaktSalaryFormulaDtos.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：SalaryFormula 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSalaryFormula 生成，请按需审阅）
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
// SalaryFormula 响应 DTO
// ========================================

/// <summary>
/// 薪资计算公式（方案+步骤合一：set_code 分组，每行一步；标准五步：应发→社保→公积金→个税→实发） 同一 set_code 示例： gross_amount = base_salary + bonus_amount + overtime_pay + allowance_total social_security_deduction = social_security_base * employee_ss_ratio housing_fund_deduction = housing_fund_base * employee_hf_ratio tax_deduction = CUMULATIVE_TAX(taxable_income) net_amount = gross_amount - social_security_deduction - housing_fund_deduction - tax_deduction - other_deduction
/// 对应前端 TaktSalaryFormulaDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSalaryFormulaDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SalaryFormulaID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalaryFormulaId { get; set; }


    /// <summary>
    /// 执行顺序（同一 set_code 内从小到大；应发=1 … 实发=5）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int FormulaStatus { get; set; } = 0;

}

// ========================================
// SalaryFormula 查询 DTO
// ========================================

/// <summary>
/// SalaryFormula 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSalaryFormulaQueryDto : TaktPagedQuery
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
    /// 公式方案编码（同编码多行=一套完整核算步骤，租户+公司内业务唯一标识）
    /// </summary>
    public string? SetCode { get; set; } = string.Empty;

    /// <summary>
    /// 公式方案名称
    /// </summary>
    public string? SetName { get; set; } = string.Empty;

    /// <summary>
    /// 关联薪酬体系 ID（可选；同 set_code 各行取值应一致）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PayrollId { get; set; }

    /// <summary>
    /// 步骤编码（同方案内唯一，如 GROSS、SS_EMP、HF_EMP、TAX、NET）
    /// </summary>
    public string? FormulaCode { get; set; } = string.Empty;

    /// <summary>
    /// 步骤名称（如：应发合计、社保个人、公积金个人、个税、实发）
    /// </summary>
    public string? FormulaName { get; set; } = string.Empty;

    /// <summary>
    /// 公式步骤类型（字典 humanresource_compensation_salary_formula_step_type：应发/社保个人/公积金个人/个税/实发）
    /// </summary>
    public int? FormulaStep { get; set; }

    /// <summary>
    /// 结果写入字段（与 TaktPayslip 列名一致，如 gross_amount、net_amount）
    /// </summary>
    public string? TargetField { get; set; } = string.Empty;

    /// <summary>
    /// 计算公式表达式（引擎解析；支持 + - * / 及 CUMULATIVE_TAX 等内置函数）
    /// </summary>
    public string? FormulaExpression { get; set; } = string.Empty;

    /// <summary>
    /// 步骤说明（可读描述，如「应发=基本+绩效+加班费+补贴」）
    /// </summary>
    public string? StepDescription { get; set; } = string.Empty;

    /// <summary>
    /// 方案生效日期（同 set_code 各行应一致）（范围查询-开始）
    /// </summary>
    public DateTime? EffectiveDateStart { get; set; }

    /// <summary>
    /// 方案生效日期（同 set_code 各行应一致）（范围查询-结束）
    /// </summary>
    public DateTime? EffectiveDateEnd { get; set; }

    /// <summary>
    /// 方案失效日期（范围查询-开始）
    /// </summary>
    public DateTime? ExpiryDateStart { get; set; }

    /// <summary>
    /// 方案失效日期（范围查询-结束）
    /// </summary>
    public DateTime? ExpiryDateEnd { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 执行顺序（同一 set_code 内从小到大；应发=1 … 实发=5）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int? FormulaStatus { get; set; }

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
// 创建SalaryFormula DTO
// ========================================

/// <summary>
/// 创建SalaryFormula DTO
/// </summary>
public class TaktSalaryFormulaCreateDto
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
    /// 公式方案编码（同编码多行=一套完整核算步骤，租户+公司内业务唯一标识）
    /// </summary>
    [Required(ErrorMessage = "公式方案编码（同编码多行=一套完整核算步骤，租户+公司内业务唯一标识）不能为空")]
    public string SetCode { get; set; } = string.Empty;

    /// <summary>
    /// 公式方案名称
    /// </summary>
    [Required(ErrorMessage = "公式方案名称不能为空")]
    public string SetName { get; set; } = string.Empty;

    /// <summary>
    /// 关联薪酬体系 ID（可选；同 set_code 各行取值应一致）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PayrollId { get; set; }

    /// <summary>
    /// 步骤编码（同方案内唯一，如 GROSS、SS_EMP、HF_EMP、TAX、NET）
    /// </summary>
    [Required(ErrorMessage = "步骤编码（同方案内唯一，如 GROSS、SS_EMP、HF_EMP、TAX、NET）不能为空")]
    public string FormulaCode { get; set; } = string.Empty;

    /// <summary>
    /// 步骤名称（如：应发合计、社保个人、公积金个人、个税、实发）
    /// </summary>
    [Required(ErrorMessage = "步骤名称（如：应发合计、社保个人、公积金个人、个税、实发）不能为空")]
    public string FormulaName { get; set; } = string.Empty;

    /// <summary>
    /// 公式步骤类型（字典 humanresource_compensation_salary_formula_step_type：应发/社保个人/公积金个人/个税/实发）
    /// </summary>
    public int FormulaStep { get; set; } = 0;

    /// <summary>
    /// 结果写入字段（与 TaktPayslip 列名一致，如 gross_amount、net_amount）
    /// </summary>
    [Required(ErrorMessage = "结果写入字段（与 TaktPayslip 列名一致，如 gross_amount、net_amount）不能为空")]
    public string TargetField { get; set; } = string.Empty;

    /// <summary>
    /// 计算公式表达式（引擎解析；支持 + - * / 及 CUMULATIVE_TAX 等内置函数）
    /// </summary>
    [Required(ErrorMessage = "计算公式表达式（引擎解析；支持 + - * / 及 CUMULATIVE_TAX 等内置函数）不能为空")]
    public string FormulaExpression { get; set; } = string.Empty;

    /// <summary>
    /// 步骤说明（可读描述，如「应发=基本+绩效+加班费+补贴」）
    /// </summary>
    public string? StepDescription { get; set; } = string.Empty;

    /// <summary>
    /// 方案生效日期（同 set_code 各行应一致）
    /// </summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>
    /// 方案失效日期
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    [Required(ErrorMessage = "关联工厂不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int FormulaStatus { get; set; } = 0;

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
// 更新SalaryFormula DTO
// ========================================

/// <summary>
/// 更新SalaryFormula DTO
/// 继承 TaktSalaryFormulaCreateDto，添加 SalaryFormulaId 字段
/// </summary>
public class TaktSalaryFormulaUpdateDto : TaktSalaryFormulaCreateDto
{
    /// <summary>
    /// SalaryFormulaID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalaryFormulaId { get; set; }

}

// ========================================
// SalaryFormula 状态 DTO
// ========================================

/// <summary>
/// SalaryFormula 状态更新 DTO
/// </summary>
public class TaktSalaryFormulaStatusDto
{
    /// <summary>
    /// SalaryFormulaID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalaryFormulaId { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    [Required(ErrorMessage = "状态（字典 sys_normal_disable）不能为空")]
    public int FormulaStatus { get; set; } = 0;
}

// ========================================
// SalaryFormula 排序 DTO
// ========================================

/// <summary>
/// SalaryFormula 排序更新 DTO
/// </summary>
public class TaktSalaryFormulaSortDto
{
    /// <summary>
    /// SalaryFormulaID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalaryFormulaId { get; set; }

    /// <summary>
    /// 执行顺序（同一 set_code 内从小到大；应发=1 … 实发=5）
    /// </summary>
    [Required(ErrorMessage = "执行顺序（同一 set_code 内从小到大；应发=1 … 实发=5）不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SalaryFormula 导入模板行 DTO
/// </summary>
public class TaktSalaryFormulaTemplateDto
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
    /// 公式方案编码（同编码多行=一套完整核算步骤，租户+公司内业务唯一标识）
    /// </summary>
    public string? SetCode { get; set; } = string.Empty;

    /// <summary>
    /// 公式方案名称
    /// </summary>
    public string? SetName { get; set; } = string.Empty;

    /// <summary>
    /// 关联薪酬体系 ID（可选；同 set_code 各行取值应一致）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PayrollId { get; set; }

    /// <summary>
    /// 步骤编码（同方案内唯一，如 GROSS、SS_EMP、HF_EMP、TAX、NET）
    /// </summary>
    public string? FormulaCode { get; set; } = string.Empty;

    /// <summary>
    /// 步骤名称（如：应发合计、社保个人、公积金个人、个税、实发）
    /// </summary>
    public string? FormulaName { get; set; } = string.Empty;

    /// <summary>
    /// 公式步骤类型（字典 humanresource_compensation_salary_formula_step_type：应发/社保个人/公积金个人/个税/实发）
    /// </summary>
    public int? FormulaStep { get; set; }

    /// <summary>
    /// 结果写入字段（与 TaktPayslip 列名一致，如 gross_amount、net_amount）
    /// </summary>
    public string? TargetField { get; set; } = string.Empty;

    /// <summary>
    /// 计算公式表达式（引擎解析；支持 + - * / 及 CUMULATIVE_TAX 等内置函数）
    /// </summary>
    public string? FormulaExpression { get; set; } = string.Empty;

    /// <summary>
    /// 步骤说明（可读描述，如「应发=基本+绩效+加班费+补贴」）
    /// </summary>
    public string? StepDescription { get; set; } = string.Empty;

    /// <summary>
    /// 方案生效日期（同 set_code 各行应一致）
    /// </summary>
    public DateTime? EffectiveDate { get; set; }

    /// <summary>
    /// 方案失效日期
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int? FormulaStatus { get; set; }

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
/// SalaryFormula 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSalaryFormulaImportDto
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
    /// 公式方案编码（同编码多行=一套完整核算步骤，租户+公司内业务唯一标识）
    /// </summary>
    public string? SetCode { get; set; } = string.Empty;

    /// <summary>
    /// 公式方案名称
    /// </summary>
    public string? SetName { get; set; } = string.Empty;

    /// <summary>
    /// 关联薪酬体系 ID（可选；同 set_code 各行取值应一致）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PayrollId { get; set; }

    /// <summary>
    /// 步骤编码（同方案内唯一，如 GROSS、SS_EMP、HF_EMP、TAX、NET）
    /// </summary>
    public string? FormulaCode { get; set; } = string.Empty;

    /// <summary>
    /// 步骤名称（如：应发合计、社保个人、公积金个人、个税、实发）
    /// </summary>
    public string? FormulaName { get; set; } = string.Empty;

    /// <summary>
    /// 公式步骤类型（字典 humanresource_compensation_salary_formula_step_type：应发/社保个人/公积金个人/个税/实发）
    /// </summary>
    public int? FormulaStep { get; set; }

    /// <summary>
    /// 结果写入字段（与 TaktPayslip 列名一致，如 gross_amount、net_amount）
    /// </summary>
    public string? TargetField { get; set; } = string.Empty;

    /// <summary>
    /// 计算公式表达式（引擎解析；支持 + - * / 及 CUMULATIVE_TAX 等内置函数）
    /// </summary>
    public string? FormulaExpression { get; set; } = string.Empty;

    /// <summary>
    /// 步骤说明（可读描述，如「应发=基本+绩效+加班费+补贴」）
    /// </summary>
    public string? StepDescription { get; set; } = string.Empty;

    /// <summary>
    /// 方案生效日期（同 set_code 各行应一致）
    /// </summary>
    public DateTime? EffectiveDate { get; set; }

    /// <summary>
    /// 方案失效日期
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int? FormulaStatus { get; set; }

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
/// SalaryFormula 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSalaryFormulaExportDto
{
    /// <summary>
    /// SalaryFormulaID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalaryFormulaId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 公式方案编码（同编码多行=一套完整核算步骤，租户+公司内业务唯一标识）
    /// </summary>
    public string SetCode { get; set; } = string.Empty;

    /// <summary>
    /// 公式方案名称
    /// </summary>
    public string SetName { get; set; } = string.Empty;

    /// <summary>
    /// 关联薪酬体系 ID（可选；同 set_code 各行取值应一致）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PayrollId { get; set; }

    /// <summary>
    /// 步骤编码（同方案内唯一，如 GROSS、SS_EMP、HF_EMP、TAX、NET）
    /// </summary>
    public string FormulaCode { get; set; } = string.Empty;

    /// <summary>
    /// 步骤名称（如：应发合计、社保个人、公积金个人、个税、实发）
    /// </summary>
    public string FormulaName { get; set; } = string.Empty;

    /// <summary>
    /// 公式步骤类型（字典 humanresource_compensation_salary_formula_step_type：应发/社保个人/公积金个人/个税/实发）
    /// </summary>
    public int FormulaStep { get; set; } = 0;

    /// <summary>
    /// 结果写入字段（与 TaktPayslip 列名一致，如 gross_amount、net_amount）
    /// </summary>
    public string TargetField { get; set; } = string.Empty;

    /// <summary>
    /// 计算公式表达式（引擎解析；支持 + - * / 及 CUMULATIVE_TAX 等内置函数）
    /// </summary>
    public string FormulaExpression { get; set; } = string.Empty;

    /// <summary>
    /// 步骤说明（可读描述，如「应发=基本+绩效+加班费+补贴」）
    /// </summary>
    public string? StepDescription { get; set; } = string.Empty;

    /// <summary>
    /// 方案生效日期（同 set_code 各行应一致）
    /// </summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>
    /// 方案失效日期
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 执行顺序（同一 set_code 内从小到大；应发=1 … 实发=5）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int FormulaStatus { get; set; } = 0;

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
