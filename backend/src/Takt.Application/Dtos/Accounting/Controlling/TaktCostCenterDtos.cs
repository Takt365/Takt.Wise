// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Accounting.Controlling
// 文件名称：TaktCostCenterDtos.cs
// 创建时间：2026-08-31
// 创建人：Takt365(Auto Generated)
// 功能描述：CostCenter 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktCostCenter 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Accounting.Controlling;

// ========================================
// CostCenter 响应 DTO
// ========================================

/// <summary>
/// 成本中心实体
/// 对应前端 TaktCostCenterDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktCostCenterDto : TaktCompanyDtoBase
{
    /// <summary>
    /// CostCenterID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CostCenterId { get; set; }

    /// <summary>
    /// 成本中心编码（6 位，与部门编码同长；租户+公司内唯一）
    /// </summary>
    public string CostCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本中心名称
    /// </summary>
    public string CostCenterName { get; set; } = string.Empty;

    /// <summary>
    /// 父级 ID（0 表示根节点）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ParentId { get; set; }

    /// <summary>
    /// 成本中心类型（字典 accounting_controlling_cost_center_type；DictValue=F/G/H/L/S）
    /// </summary>
    public string CostCenterType { get; set; } = string.Empty;

    /// <summary>
    /// 负责人用户 ID（选项 TaktUsers/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ManagerId { get; set; }

    /// <summary>
    /// 负责人姓名（冗余：按 ManagerId 取 TaktUser.NickName联动）
    /// </summary>
    public string? ManagerName { get; set; } = string.Empty;

    /// <summary>
    /// 所属部门（选项 TaktDepts/tree-options,DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 所属部门名称（冗余：按 DeptId 取 TaktDept.DeptName联动）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 成本中心层级
    /// </summary>
    public int CostCenterLevel { get; set; } = 0;

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
    /// 成本中心状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int CostCenterStatus { get; set; } = 0;

}

// ========================================
// CostCenter 树形响应 DTO
// ========================================

/// <summary>
/// CostCenter 树形列表/树选择 DTO（含子节点）
/// 对应 GetCostCenterTreeAsync 等接口
/// </summary>
public class TaktCostCenterTreeDto : TaktCostCenterDto
{
    /// <summary>
    /// 子节点（懒加载树接口返回 null，表示尚未加载；勿用空 List 冒充已加载）
    /// </summary>
    public List<TaktCostCenterTreeDto>? Children { get; set; }
}

// ========================================
// CostCenter 查询 DTO
// ========================================

/// <summary>
/// CostCenter 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktCostCenterQueryDto : TaktPagedQuery
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
    /// 成本中心编码（6 位，与部门编码同长；租户+公司内唯一）
    /// </summary>
    public string? CostCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本中心名称
    /// </summary>
    public string? CostCenterName { get; set; } = string.Empty;

    /// <summary>
    /// 父级 ID（0 表示根节点）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ParentId { get; set; }

    /// <summary>
    /// 成本中心类型（字典 accounting_controlling_cost_center_type；DictValue=F/G/H/L/S）
    /// </summary>
    public string? CostCenterType { get; set; } = string.Empty;

    /// <summary>
    /// 负责人用户 ID（选项 TaktUsers/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ManagerId { get; set; }

    /// <summary>
    /// 负责人姓名（冗余：按 ManagerId 取 TaktUser.NickName联动）
    /// </summary>
    public string? ManagerName { get; set; } = string.Empty;

    /// <summary>
    /// 所属部门（选项 TaktDepts/tree-options,DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 所属部门名称（冗余：按 DeptId 取 TaktDept.DeptName联动）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 成本中心层级
    /// </summary>
    public int? CostCenterLevel { get; set; }

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
    /// 成本中心状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? CostCenterStatus { get; set; }

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
// 创建CostCenter DTO
// ========================================

/// <summary>
/// 创建CostCenter DTO
/// </summary>
public class TaktCostCenterCreateDto
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
    /// 成本中心编码（6 位，与部门编码同长；租户+公司内唯一）
    /// </summary>
    [Required(ErrorMessage = "成本中心编码（6 位，与部门编码同长；租户+公司内唯一）不能为空")]
    public string CostCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本中心名称
    /// </summary>
    [Required(ErrorMessage = "成本中心名称不能为空")]
    public string CostCenterName { get; set; } = string.Empty;

    /// <summary>
    /// 父级 ID（0 表示根节点）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ParentId { get; set; }

    /// <summary>
    /// 成本中心类型（字典 accounting_controlling_cost_center_type；DictValue=F/G/H/L/S）
    /// </summary>
    [Required(ErrorMessage = "成本中心类型（字典 accounting_controlling_cost_center_type；DictValue=F/G/H/L/S）不能为空")]
    public string CostCenterType { get; set; } = string.Empty;

    /// <summary>
    /// 负责人用户 ID（选项 TaktUsers/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ManagerId { get; set; }

    /// <summary>
    /// 负责人姓名（冗余：按 ManagerId 取 TaktUser.NickName联动）
    /// </summary>
    public string? ManagerName { get; set; } = string.Empty;

    /// <summary>
    /// 所属部门（选项 TaktDepts/tree-options,DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 所属部门名称（冗余：按 DeptId 取 TaktDept.DeptName联动）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 成本中心层级
    /// </summary>
    public int CostCenterLevel { get; set; } = 0;

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime ValidFrom { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime ValidTo { get; set; }

    /// <summary>
    /// 成本中心状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int CostCenterStatus { get; set; } = 0;

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
// 更新CostCenter DTO
// ========================================

/// <summary>
/// 更新CostCenter DTO
/// 继承 TaktCostCenterCreateDto，添加 CostCenterId 字段
/// </summary>
public class TaktCostCenterUpdateDto : TaktCostCenterCreateDto
{
    /// <summary>
    /// CostCenterID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CostCenterId { get; set; }

}

// ========================================
// CostCenter 状态 DTO
// ========================================

/// <summary>
/// CostCenter 状态更新 DTO
/// </summary>
public class TaktCostCenterStatusDto
{
    /// <summary>
    /// CostCenterID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CostCenterId { get; set; }

    /// <summary>
    /// 成本中心状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [Required(ErrorMessage = "成本中心状态（字典 sys_normal_disable；1=启用，0=禁用）不能为空")]
    public int CostCenterStatus { get; set; } = 0;
}

// ========================================
// CostCenter 排序 DTO
// ========================================

/// <summary>
/// CostCenter 排序更新 DTO
/// </summary>
public class TaktCostCenterSortDto
{
    /// <summary>
    /// CostCenterID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CostCenterId { get; set; }

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
/// CostCenter 导入模板行 DTO
/// </summary>
public class TaktCostCenterTemplateDto
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
    /// 成本中心编码（6 位，与部门编码同长；租户+公司内唯一）
    /// </summary>
    public string? CostCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本中心名称
    /// </summary>
    public string? CostCenterName { get; set; } = string.Empty;

    /// <summary>
    /// 父级 ID（0 表示根节点）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ParentId { get; set; }

    /// <summary>
    /// 成本中心类型（字典 accounting_controlling_cost_center_type；DictValue=F/G/H/L/S）
    /// </summary>
    public string? CostCenterType { get; set; } = string.Empty;

    /// <summary>
    /// 负责人用户 ID（选项 TaktUsers/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ManagerId { get; set; }

    /// <summary>
    /// 负责人姓名（冗余：按 ManagerId 取 TaktUser.NickName联动）
    /// </summary>
    public string? ManagerName { get; set; } = string.Empty;

    /// <summary>
    /// 所属部门（选项 TaktDepts/tree-options,DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 所属部门名称（冗余：按 DeptId 取 TaktDept.DeptName联动）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 成本中心层级
    /// </summary>
    public int? CostCenterLevel { get; set; }

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime? ValidFrom { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime? ValidTo { get; set; }

    /// <summary>
    /// 成本中心状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? CostCenterStatus { get; set; }

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
/// CostCenter 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktCostCenterImportDto
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
    /// 成本中心编码（6 位，与部门编码同长；租户+公司内唯一）
    /// </summary>
    public string? CostCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本中心名称
    /// </summary>
    public string? CostCenterName { get; set; } = string.Empty;

    /// <summary>
    /// 父级 ID（0 表示根节点）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ParentId { get; set; }

    /// <summary>
    /// 成本中心类型（字典 accounting_controlling_cost_center_type；DictValue=F/G/H/L/S）
    /// </summary>
    public string? CostCenterType { get; set; } = string.Empty;

    /// <summary>
    /// 负责人用户 ID（选项 TaktUsers/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ManagerId { get; set; }

    /// <summary>
    /// 负责人姓名（冗余：按 ManagerId 取 TaktUser.NickName联动）
    /// </summary>
    public string? ManagerName { get; set; } = string.Empty;

    /// <summary>
    /// 所属部门（选项 TaktDepts/tree-options,DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 所属部门名称（冗余：按 DeptId 取 TaktDept.DeptName联动）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 成本中心层级
    /// </summary>
    public int? CostCenterLevel { get; set; }

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime? ValidFrom { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime? ValidTo { get; set; }

    /// <summary>
    /// 成本中心状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? CostCenterStatus { get; set; }

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
/// CostCenter 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktCostCenterExportDto
{
    /// <summary>
    /// CostCenterID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CostCenterId { get; set; }

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
    /// 成本中心编码（6 位，与部门编码同长；租户+公司内唯一）
    /// </summary>
    public string CostCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本中心名称
    /// </summary>
    public string CostCenterName { get; set; } = string.Empty;

    /// <summary>
    /// 父级 ID（0 表示根节点）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ParentId { get; set; }

    /// <summary>
    /// 成本中心类型（字典 accounting_controlling_cost_center_type；DictValue=F/G/H/L/S）
    /// </summary>
    public string CostCenterType { get; set; } = string.Empty;

    /// <summary>
    /// 负责人用户 ID（选项 TaktUsers/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ManagerId { get; set; }

    /// <summary>
    /// 负责人姓名（冗余：按 ManagerId 取 TaktUser.NickName联动）
    /// </summary>
    public string? ManagerName { get; set; } = string.Empty;

    /// <summary>
    /// 所属部门（选项 TaktDepts/tree-options,DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 所属部门名称（冗余：按 DeptId 取 TaktDept.DeptName联动）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 成本中心层级
    /// </summary>
    public int CostCenterLevel { get; set; } = 0;

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
    /// 成本中心状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int CostCenterStatus { get; set; } = 0;

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
