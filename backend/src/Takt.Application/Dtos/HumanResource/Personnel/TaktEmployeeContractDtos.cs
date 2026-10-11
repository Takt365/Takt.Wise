// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.HumanResource.Personnel
// 文件名称：TaktEmployeeContractDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：EmployeeContract 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktEmployeeContract 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.HumanResource.Personnel;

// ========================================
// EmployeeContract 响应 DTO
// ========================================

/// <summary>
/// 员工劳动合同
/// 对应前端 TaktEmployeeContractDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktEmployeeContractDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EmployeeContractID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeContractId { get; set; }

    /// <summary>
    /// 员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
    /// </summary>
    public string EmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
    /// </summary>
    public string EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 合同编码
    /// </summary>
    public string ContractCode { get; set; } = string.Empty;

    /// <summary>
    /// 合同类型（字典 humanresource_personnel_employee_contract_type；0=固定期限 1=无固定期限 2=以完成一定工作任务为期限 3=实习）
    /// </summary>
    public int ContractType { get; set; } = 0;

    /// <summary>
    /// 合同开始日期
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 合同结束日期
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// 试用期结束日期
    /// </summary>
    public DateTime? ProbationEndDate { get; set; }

    /// <summary>
    /// 签订日期
    /// </summary>
    public DateTime? SignDate { get; set; }

    /// <summary>
    /// 签约单位
    /// </summary>
    public string? SignCompany { get; set; } = string.Empty;

    /// <summary>
    /// 合同状态（字典 humanresource_personnel_employee_contract_status；0=草稿 1=生效 2=到期 3=终止）
    /// </summary>
    public int ContractStatus { get; set; } = 0;

    /// <summary>
    /// 员工主档（多对一）
    /// （主表：TaktEmployee）
    /// </summary>
    public TaktEmployeeDto? Employee { get; set; }

}

// ========================================
// EmployeeContract 查询 DTO
// ========================================

/// <summary>
/// EmployeeContract 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktEmployeeContractQueryDto : TaktPagedQuery
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
    /// 员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
    /// </summary>
    public string? EmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 合同编码
    /// </summary>
    public string? ContractCode { get; set; } = string.Empty;

    /// <summary>
    /// 合同类型（字典 humanresource_personnel_employee_contract_type；0=固定期限 1=无固定期限 2=以完成一定工作任务为期限 3=实习）
    /// </summary>
    public int? ContractType { get; set; }

    /// <summary>
    /// 合同开始日期（范围查询-开始）
    /// </summary>
    public DateTime? StartDateStart { get; set; }

    /// <summary>
    /// 合同开始日期（范围查询-结束）
    /// </summary>
    public DateTime? StartDateEnd { get; set; }

    /// <summary>
    /// 合同结束日期（范围查询-开始）
    /// </summary>
    public DateTime? EndDateStart { get; set; }

    /// <summary>
    /// 合同结束日期（范围查询-结束）
    /// </summary>
    public DateTime? EndDateEnd { get; set; }

    /// <summary>
    /// 试用期结束日期（范围查询-开始）
    /// </summary>
    public DateTime? ProbationEndDateStart { get; set; }

    /// <summary>
    /// 试用期结束日期（范围查询-结束）
    /// </summary>
    public DateTime? ProbationEndDateEnd { get; set; }

    /// <summary>
    /// 签订日期（范围查询-开始）
    /// </summary>
    public DateTime? SignDateStart { get; set; }

    /// <summary>
    /// 签订日期（范围查询-结束）
    /// </summary>
    public DateTime? SignDateEnd { get; set; }

    /// <summary>
    /// 签约单位
    /// </summary>
    public string? SignCompany { get; set; } = string.Empty;

    /// <summary>
    /// 合同状态（字典 humanresource_personnel_employee_contract_status；0=草稿 1=生效 2=到期 3=终止）
    /// </summary>
    public int? ContractStatus { get; set; }

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
// 创建EmployeeContract DTO
// ========================================

/// <summary>
/// 创建EmployeeContract DTO
/// </summary>
public class TaktEmployeeContractCreateDto
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
    /// 员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
    /// </summary>
    public string EmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
    /// </summary>
    public string EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 合同编码
    /// </summary>
    [Required(ErrorMessage = "合同编码不能为空")]
    public string ContractCode { get; set; } = string.Empty;

    /// <summary>
    /// 合同类型（字典 humanresource_personnel_employee_contract_type；0=固定期限 1=无固定期限 2=以完成一定工作任务为期限 3=实习）
    /// </summary>
    public int ContractType { get; set; } = 0;

    /// <summary>
    /// 合同开始日期
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 合同结束日期
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// 试用期结束日期
    /// </summary>
    public DateTime? ProbationEndDate { get; set; }

    /// <summary>
    /// 签订日期
    /// </summary>
    public DateTime? SignDate { get; set; }

    /// <summary>
    /// 签约单位
    /// </summary>
    public string? SignCompany { get; set; } = string.Empty;

    /// <summary>
    /// 合同状态（字典 humanresource_personnel_employee_contract_status；0=草稿 1=生效 2=到期 3=终止）
    /// </summary>
    public int ContractStatus { get; set; } = 0;

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
// 更新EmployeeContract DTO
// ========================================

/// <summary>
/// 更新EmployeeContract DTO
/// 继承 TaktEmployeeContractCreateDto，添加 EmployeeContractId 字段
/// </summary>
public class TaktEmployeeContractUpdateDto : TaktEmployeeContractCreateDto
{
    /// <summary>
    /// EmployeeContractID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeContractId { get; set; }

}

// ========================================
// EmployeeContract 状态 DTO
// ========================================

/// <summary>
/// EmployeeContract 状态更新 DTO
/// </summary>
public class TaktEmployeeContractStatusDto
{
    /// <summary>
    /// EmployeeContractID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeContractId { get; set; }

    /// <summary>
    /// 合同状态（字典 humanresource_personnel_employee_contract_status；0=草稿 1=生效 2=到期 3=终止）
    /// </summary>
    [Required(ErrorMessage = "合同状态（字典 humanresource_personnel_employee_contract_status；0=草稿 1=生效 2=到期 3=终止）不能为空")]
    public int ContractStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// EmployeeContract 导入模板行 DTO
/// </summary>
public class TaktEmployeeContractTemplateDto
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
    /// 员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
    /// </summary>
    public string? EmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 合同编码
    /// </summary>
    public string? ContractCode { get; set; } = string.Empty;

    /// <summary>
    /// 合同类型（字典 humanresource_personnel_employee_contract_type；0=固定期限 1=无固定期限 2=以完成一定工作任务为期限 3=实习）
    /// </summary>
    public int? ContractType { get; set; }

    /// <summary>
    /// 合同开始日期
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// 合同结束日期
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// 试用期结束日期
    /// </summary>
    public DateTime? ProbationEndDate { get; set; }

    /// <summary>
    /// 签订日期
    /// </summary>
    public DateTime? SignDate { get; set; }

    /// <summary>
    /// 签约单位
    /// </summary>
    public string? SignCompany { get; set; } = string.Empty;

    /// <summary>
    /// 合同状态（字典 humanresource_personnel_employee_contract_status；0=草稿 1=生效 2=到期 3=终止）
    /// </summary>
    public int? ContractStatus { get; set; }

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
/// EmployeeContract 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktEmployeeContractImportDto
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
    /// 员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
    /// </summary>
    public string? EmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 合同编码
    /// </summary>
    public string? ContractCode { get; set; } = string.Empty;

    /// <summary>
    /// 合同类型（字典 humanresource_personnel_employee_contract_type；0=固定期限 1=无固定期限 2=以完成一定工作任务为期限 3=实习）
    /// </summary>
    public int? ContractType { get; set; }

    /// <summary>
    /// 合同开始日期
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// 合同结束日期
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// 试用期结束日期
    /// </summary>
    public DateTime? ProbationEndDate { get; set; }

    /// <summary>
    /// 签订日期
    /// </summary>
    public DateTime? SignDate { get; set; }

    /// <summary>
    /// 签约单位
    /// </summary>
    public string? SignCompany { get; set; } = string.Empty;

    /// <summary>
    /// 合同状态（字典 humanresource_personnel_employee_contract_status；0=草稿 1=生效 2=到期 3=终止）
    /// </summary>
    public int? ContractStatus { get; set; }

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
/// EmployeeContract 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktEmployeeContractExportDto
{
    /// <summary>
    /// EmployeeContractID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeContractId { get; set; }

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
    /// 员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
    /// </summary>
    public string EmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
    /// </summary>
    public string EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 合同编码
    /// </summary>
    public string ContractCode { get; set; } = string.Empty;

    /// <summary>
    /// 合同类型（字典 humanresource_personnel_employee_contract_type；0=固定期限 1=无固定期限 2=以完成一定工作任务为期限 3=实习）
    /// </summary>
    public int ContractType { get; set; } = 0;

    /// <summary>
    /// 合同开始日期
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 合同结束日期
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// 试用期结束日期
    /// </summary>
    public DateTime? ProbationEndDate { get; set; }

    /// <summary>
    /// 签订日期
    /// </summary>
    public DateTime? SignDate { get; set; }

    /// <summary>
    /// 签约单位
    /// </summary>
    public string? SignCompany { get; set; } = string.Empty;

    /// <summary>
    /// 合同状态（字典 humanresource_personnel_employee_contract_status；0=草稿 1=生效 2=到期 3=终止）
    /// </summary>
    public int ContractStatus { get; set; } = 0;

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
