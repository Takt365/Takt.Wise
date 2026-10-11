// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Defect
// 文件名称：TaktDefectGroupDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：DefectGroup 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktDefectGroup 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Defect;

// ========================================
// DefectGroup 响应 DTO
// ========================================

/// <summary>
/// 不良组主数据实体（公司级；按不良组类别区分的不良业务组织分组）
/// 对应前端 TaktDefectGroupDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktDefectGroupDto : TaktCompanyDtoBase
{
    /// <summary>
    /// DefectGroupID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DefectGroupId { get; set; }

    /// <summary>
    /// 不良组编码（3）
    /// </summary>
    public string DefectGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// 不良组名称
    /// </summary>
    public string DefectGroupName { get; set; } = string.Empty;

    /// <summary>
    /// 不良组描述
    /// </summary>
    public string? DefectGroupDescription { get; set; } = string.Empty;

    /// <summary>
    /// 不良组类别（字典 logistics_manufacturing_defect_group_category；0=Assy，1=Inspection，2=Repair）
    /// </summary>
    public int DefectGroupCategory { get; set; } = 0;

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 联系邮箱
    /// </summary>
    public string? ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 不良组状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int GroupStatus { get; set; } = 0;

}

// ========================================
// DefectGroup 查询 DTO
// ========================================

/// <summary>
/// DefectGroup 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktDefectGroupQueryDto : TaktPagedQuery
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
    /// 不良组编码（3）
    /// </summary>
    public string? DefectGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// 不良组名称
    /// </summary>
    public string? DefectGroupName { get; set; } = string.Empty;

    /// <summary>
    /// 不良组描述
    /// </summary>
    public string? DefectGroupDescription { get; set; } = string.Empty;

    /// <summary>
    /// 不良组类别（字典 logistics_manufacturing_defect_group_category；0=Assy，1=Inspection，2=Repair）
    /// </summary>
    public int? DefectGroupCategory { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 联系邮箱
    /// </summary>
    public string? ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 不良组状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? GroupStatus { get; set; }

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
// 创建DefectGroup DTO
// ========================================

/// <summary>
/// 创建DefectGroup DTO
/// </summary>
public class TaktDefectGroupCreateDto
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
    /// 不良组编码（3）
    /// </summary>
    [Required(ErrorMessage = "不良组编码（3）不能为空")]
    public string DefectGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// 不良组名称
    /// </summary>
    [Required(ErrorMessage = "不良组名称不能为空")]
    public string DefectGroupName { get; set; } = string.Empty;

    /// <summary>
    /// 不良组描述
    /// </summary>
    public string? DefectGroupDescription { get; set; } = string.Empty;

    /// <summary>
    /// 不良组类别（字典 logistics_manufacturing_defect_group_category；0=Assy，1=Inspection，2=Repair）
    /// </summary>
    public int DefectGroupCategory { get; set; } = 0;

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 联系邮箱
    /// </summary>
    public string? ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 不良组状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int GroupStatus { get; set; } = 0;

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
// 更新DefectGroup DTO
// ========================================

/// <summary>
/// 更新DefectGroup DTO
/// 继承 TaktDefectGroupCreateDto，添加 DefectGroupId 字段
/// </summary>
public class TaktDefectGroupUpdateDto : TaktDefectGroupCreateDto
{
    /// <summary>
    /// DefectGroupID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DefectGroupId { get; set; }

}

// ========================================
// DefectGroup 状态 DTO
// ========================================

/// <summary>
/// DefectGroup 状态更新 DTO
/// </summary>
public class TaktDefectGroupStatusDto
{
    /// <summary>
    /// DefectGroupID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DefectGroupId { get; set; }

    /// <summary>
    /// 不良组状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [Required(ErrorMessage = "不良组状态（字典 sys_normal_disable；1=启用，0=禁用）不能为空")]
    public int GroupStatus { get; set; } = 0;
}

// ========================================
// DefectGroup 排序 DTO
// ========================================

/// <summary>
/// DefectGroup 排序更新 DTO
/// </summary>
public class TaktDefectGroupSortDto
{
    /// <summary>
    /// DefectGroupID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DefectGroupId { get; set; }

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
/// DefectGroup 导入模板行 DTO
/// </summary>
public class TaktDefectGroupTemplateDto
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
    /// 不良组编码（3）
    /// </summary>
    public string? DefectGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// 不良组名称
    /// </summary>
    public string? DefectGroupName { get; set; } = string.Empty;

    /// <summary>
    /// 不良组描述
    /// </summary>
    public string? DefectGroupDescription { get; set; } = string.Empty;

    /// <summary>
    /// 不良组类别（字典 logistics_manufacturing_defect_group_category；0=Assy，1=Inspection，2=Repair）
    /// </summary>
    public int? DefectGroupCategory { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 联系邮箱
    /// </summary>
    public string? ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 不良组状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? GroupStatus { get; set; }

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
/// DefectGroup 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktDefectGroupImportDto
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
    /// 不良组编码（3）
    /// </summary>
    public string? DefectGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// 不良组名称
    /// </summary>
    public string? DefectGroupName { get; set; } = string.Empty;

    /// <summary>
    /// 不良组描述
    /// </summary>
    public string? DefectGroupDescription { get; set; } = string.Empty;

    /// <summary>
    /// 不良组类别（字典 logistics_manufacturing_defect_group_category；0=Assy，1=Inspection，2=Repair）
    /// </summary>
    public int? DefectGroupCategory { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 联系邮箱
    /// </summary>
    public string? ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 不良组状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? GroupStatus { get; set; }

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
/// DefectGroup 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktDefectGroupExportDto
{
    /// <summary>
    /// DefectGroupID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DefectGroupId { get; set; }

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
    /// 不良组编码（3）
    /// </summary>
    public string DefectGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// 不良组名称
    /// </summary>
    public string DefectGroupName { get; set; } = string.Empty;

    /// <summary>
    /// 不良组描述
    /// </summary>
    public string? DefectGroupDescription { get; set; } = string.Empty;

    /// <summary>
    /// 不良组类别（字典 logistics_manufacturing_defect_group_category；0=Assy，1=Inspection，2=Repair）
    /// </summary>
    public int DefectGroupCategory { get; set; } = 0;

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 联系邮箱
    /// </summary>
    public string? ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 不良组状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int GroupStatus { get; set; } = 0;

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
