// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Quality.Operation
// 文件名称：TaktQualityGroupDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：QualityGroup 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktQualityGroup 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Quality.Operation;

// ========================================
// QualityGroup 响应 DTO
// ========================================

/// <summary>
/// 质量组主数据实体（公司级；按质量组类别区分的质量业务组织分组）
/// 对应前端 TaktQualityGroupDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktQualityGroupDto : TaktCompanyDtoBase
{
    /// <summary>
    /// QualityGroupID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityGroupId { get; set; }

    /// <summary>
    /// 质量组编码（3）
    /// </summary>
    public string QualityGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// 质量组名称
    /// </summary>
    public string QualityGroupName { get; set; } = string.Empty;

    /// <summary>
    /// 质量组描述
    /// </summary>
    public string? QualityGroupDescription { get; set; } = string.Empty;

    /// <summary>
    /// 质量组类别（字典 logistics_quality_group_category；0=IQC组 1=QA组 2=IPQC组 3=责任组 4=处理组 5=质量负责人组）
    /// </summary>
    public int QualityGroupCategory { get; set; } = 0;

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
    /// 质量组状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int GroupStatus { get; set; } = 0;

}

// ========================================
// QualityGroup 查询 DTO
// ========================================

/// <summary>
/// QualityGroup 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktQualityGroupQueryDto : TaktPagedQuery
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 质量组编码（3）
    /// </summary>
    public string? QualityGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// 质量组名称
    /// </summary>
    public string? QualityGroupName { get; set; } = string.Empty;

    /// <summary>
    /// 质量组描述
    /// </summary>
    public string? QualityGroupDescription { get; set; } = string.Empty;

    /// <summary>
    /// 质量组类别（字典 logistics_quality_group_category；0=IQC组 1=QA组 2=IPQC组 3=责任组 4=处理组 5=质量负责人组）
    /// </summary>
    public int? QualityGroupCategory { get; set; }

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
    /// 质量组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
// 创建QualityGroup DTO
// ========================================

/// <summary>
/// 创建QualityGroup DTO
/// </summary>
public class TaktQualityGroupCreateDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "工厂代码（选项 TaktPlants/options；DictValue=PlantCode）不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 质量组编码（3）
    /// </summary>
    [Required(ErrorMessage = "质量组编码（3）不能为空")]
    public string QualityGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// 质量组名称
    /// </summary>
    [Required(ErrorMessage = "质量组名称不能为空")]
    public string QualityGroupName { get; set; } = string.Empty;

    /// <summary>
    /// 质量组描述
    /// </summary>
    public string? QualityGroupDescription { get; set; } = string.Empty;

    /// <summary>
    /// 质量组类别（字典 logistics_quality_group_category；0=IQC组 1=QA组 2=IPQC组 3=责任组 4=处理组 5=质量负责人组）
    /// </summary>
    public int QualityGroupCategory { get; set; } = 0;

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
    /// 质量组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
// 更新QualityGroup DTO
// ========================================

/// <summary>
/// 更新QualityGroup DTO
/// 继承 TaktQualityGroupCreateDto，添加 QualityGroupId 字段
/// </summary>
public class TaktQualityGroupUpdateDto : TaktQualityGroupCreateDto
{
    /// <summary>
    /// QualityGroupID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityGroupId { get; set; }

}

// ========================================
// QualityGroup 状态 DTO
// ========================================

/// <summary>
/// QualityGroup 状态更新 DTO
/// </summary>
public class TaktQualityGroupStatusDto
{
    /// <summary>
    /// QualityGroupID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityGroupId { get; set; }

    /// <summary>
    /// 质量组状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [Required(ErrorMessage = "质量组状态（字典 sys_normal_disable；1=启用，0=禁用）不能为空")]
    public int GroupStatus { get; set; } = 0;
}

// ========================================
// QualityGroup 排序 DTO
// ========================================

/// <summary>
/// QualityGroup 排序更新 DTO
/// </summary>
public class TaktQualityGroupSortDto
{
    /// <summary>
    /// QualityGroupID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityGroupId { get; set; }

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    [Required(ErrorMessage = "排序号（越小越靠前）不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// QualityGroup 导入模板行 DTO
/// </summary>
public class TaktQualityGroupTemplateDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 质量组编码（3）
    /// </summary>
    public string? QualityGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// 质量组名称
    /// </summary>
    public string? QualityGroupName { get; set; } = string.Empty;

    /// <summary>
    /// 质量组描述
    /// </summary>
    public string? QualityGroupDescription { get; set; } = string.Empty;

    /// <summary>
    /// 质量组类别（字典 logistics_quality_group_category；0=IQC组 1=QA组 2=IPQC组 3=责任组 4=处理组 5=质量负责人组）
    /// </summary>
    public int? QualityGroupCategory { get; set; }

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
    /// 质量组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
/// QualityGroup 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktQualityGroupImportDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 质量组编码（3）
    /// </summary>
    public string? QualityGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// 质量组名称
    /// </summary>
    public string? QualityGroupName { get; set; } = string.Empty;

    /// <summary>
    /// 质量组描述
    /// </summary>
    public string? QualityGroupDescription { get; set; } = string.Empty;

    /// <summary>
    /// 质量组类别（字典 logistics_quality_group_category；0=IQC组 1=QA组 2=IPQC组 3=责任组 4=处理组 5=质量负责人组）
    /// </summary>
    public int? QualityGroupCategory { get; set; }

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
    /// 质量组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
/// QualityGroup 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktQualityGroupExportDto
{
    /// <summary>
    /// QualityGroupID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityGroupId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 质量组编码（3）
    /// </summary>
    public string QualityGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// 质量组名称
    /// </summary>
    public string QualityGroupName { get; set; } = string.Empty;

    /// <summary>
    /// 质量组描述
    /// </summary>
    public string? QualityGroupDescription { get; set; } = string.Empty;

    /// <summary>
    /// 质量组类别（字典 logistics_quality_group_category；0=IQC组 1=QA组 2=IPQC组 3=责任组 4=处理组 5=质量负责人组）
    /// </summary>
    public int QualityGroupCategory { get; set; } = 0;

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
    /// 质量组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
