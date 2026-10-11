// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Sop
// 文件名称：TaktSopWorkstationDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：SopWorkstation 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSopWorkstation 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Sop;

// ========================================
// SopWorkstation 响应 DTO
// ========================================

/// <summary>
/// SOP 工位主数据实体
/// 对应前端 TaktSopWorkstationDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSopWorkstationDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SopWorkstationID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopWorkstationId { get; set; }


    /// <summary>
    /// 工位编码（工厂内唯一）
    /// </summary>
    public string WorkstationCode { get; set; } = string.Empty;

    /// <summary>
    /// 工位名称
    /// </summary>
    public string WorkstationName { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
    /// </summary>
    public string? WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组
    /// </summary>
    public string? ProductionLine { get; set; } = string.Empty;

    /// <summary>
    /// 工位类型（字典 sys_workstation_type；1=装配，2=检验，3=包装，4=测试，5=其他）
    /// </summary>
    public int WorkstationType { get; set; } = 0;

    /// <summary>
    /// 工艺段类型（字典 logistics_manufacturing_process_segment_type；1=SMT，2=自插，3=手插，4=修正，5=总装）
    /// </summary>
    public int ProcessSegmentType { get; set; } = 0;

    /// <summary>
    /// 启用状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int WorkstationStatus { get; set; } = 0;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

}

// ========================================
// SopWorkstation 查询 DTO
// ========================================

/// <summary>
/// SopWorkstation 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSopWorkstationQueryDto : TaktPagedQuery
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
    /// 工位编码（工厂内唯一）
    /// </summary>
    public string? WorkstationCode { get; set; } = string.Empty;

    /// <summary>
    /// 工位名称
    /// </summary>
    public string? WorkstationName { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
    /// </summary>
    public string? WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组
    /// </summary>
    public string? ProductionLine { get; set; } = string.Empty;

    /// <summary>
    /// 工位类型（字典 sys_workstation_type；1=装配，2=检验，3=包装，4=测试，5=其他）
    /// </summary>
    public int? WorkstationType { get; set; }

    /// <summary>
    /// 工艺段类型（字典 logistics_manufacturing_process_segment_type；1=SMT，2=自插，3=手插，4=修正，5=总装）
    /// </summary>
    public int? ProcessSegmentType { get; set; }

    /// <summary>
    /// 启用状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int? WorkstationStatus { get; set; }

    /// <summary>
    /// 排序号（回填）
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
// 创建SopWorkstation DTO
// ========================================

/// <summary>
/// 创建SopWorkstation DTO
/// </summary>
public class TaktSopWorkstationCreateDto
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
    /// 工位编码（工厂内唯一）
    /// </summary>
    [Required(ErrorMessage = "工位编码（工厂内唯一）不能为空")]
    public string WorkstationCode { get; set; } = string.Empty;

    /// <summary>
    /// 工位名称
    /// </summary>
    [Required(ErrorMessage = "工位名称不能为空")]
    public string WorkstationName { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
    /// </summary>
    public string? WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组
    /// </summary>
    public string? ProductionLine { get; set; } = string.Empty;

    /// <summary>
    /// 工位类型（字典 sys_workstation_type；1=装配，2=检验，3=包装，4=测试，5=其他）
    /// </summary>
    public int WorkstationType { get; set; } = 0;

    /// <summary>
    /// 工艺段类型（字典 logistics_manufacturing_process_segment_type；1=SMT，2=自插，3=手插，4=修正，5=总装）
    /// </summary>
    public int ProcessSegmentType { get; set; } = 0;

    /// <summary>
    /// 启用状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int WorkstationStatus { get; set; } = 0;

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
// 更新SopWorkstation DTO
// ========================================

/// <summary>
/// 更新SopWorkstation DTO
/// 继承 TaktSopWorkstationCreateDto，添加 SopWorkstationId 字段
/// </summary>
public class TaktSopWorkstationUpdateDto : TaktSopWorkstationCreateDto
{
    /// <summary>
    /// SopWorkstationID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopWorkstationId { get; set; }

}

// ========================================
// SopWorkstation 状态 DTO
// ========================================

/// <summary>
/// SopWorkstation 状态更新 DTO
/// </summary>
public class TaktSopWorkstationStatusDto
{
    /// <summary>
    /// SopWorkstationID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopWorkstationId { get; set; }

    /// <summary>
    /// 启用状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
    /// </summary>
    [Required(ErrorMessage = "启用状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）不能为空")]
    public int WorkstationStatus { get; set; } = 0;
}

// ========================================
// SopWorkstation 排序 DTO
// ========================================

/// <summary>
/// SopWorkstation 排序更新 DTO
/// </summary>
public class TaktSopWorkstationSortDto
{
    /// <summary>
    /// SopWorkstationID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopWorkstationId { get; set; }

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
/// SopWorkstation 导入模板行 DTO
/// </summary>
public class TaktSopWorkstationTemplateDto
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
    /// 工位编码（工厂内唯一）
    /// </summary>
    public string? WorkstationCode { get; set; } = string.Empty;

    /// <summary>
    /// 工位名称
    /// </summary>
    public string? WorkstationName { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
    /// </summary>
    public string? WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组
    /// </summary>
    public string? ProductionLine { get; set; } = string.Empty;

    /// <summary>
    /// 工位类型（字典 sys_workstation_type；1=装配，2=检验，3=包装，4=测试，5=其他）
    /// </summary>
    public int? WorkstationType { get; set; }

    /// <summary>
    /// 工艺段类型（字典 logistics_manufacturing_process_segment_type；1=SMT，2=自插，3=手插，4=修正，5=总装）
    /// </summary>
    public int? ProcessSegmentType { get; set; }

    /// <summary>
    /// 启用状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int? WorkstationStatus { get; set; }

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
/// SopWorkstation 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSopWorkstationImportDto
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
    /// 工位编码（工厂内唯一）
    /// </summary>
    public string? WorkstationCode { get; set; } = string.Empty;

    /// <summary>
    /// 工位名称
    /// </summary>
    public string? WorkstationName { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
    /// </summary>
    public string? WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组
    /// </summary>
    public string? ProductionLine { get; set; } = string.Empty;

    /// <summary>
    /// 工位类型（字典 sys_workstation_type；1=装配，2=检验，3=包装，4=测试，5=其他）
    /// </summary>
    public int? WorkstationType { get; set; }

    /// <summary>
    /// 工艺段类型（字典 logistics_manufacturing_process_segment_type；1=SMT，2=自插，3=手插，4=修正，5=总装）
    /// </summary>
    public int? ProcessSegmentType { get; set; }

    /// <summary>
    /// 启用状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int? WorkstationStatus { get; set; }

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
/// SopWorkstation 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSopWorkstationExportDto
{
    /// <summary>
    /// SopWorkstationID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopWorkstationId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 工位编码（工厂内唯一）
    /// </summary>
    public string WorkstationCode { get; set; } = string.Empty;

    /// <summary>
    /// 工位名称
    /// </summary>
    public string WorkstationName { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
    /// </summary>
    public string? WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组
    /// </summary>
    public string? ProductionLine { get; set; } = string.Empty;

    /// <summary>
    /// 工位类型（字典 sys_workstation_type；1=装配，2=检验，3=包装，4=测试，5=其他）
    /// </summary>
    public int WorkstationType { get; set; } = 0;

    /// <summary>
    /// 工艺段类型（字典 logistics_manufacturing_process_segment_type；1=SMT，2=自插，3=手插，4=修正，5=总装）
    /// </summary>
    public int ProcessSegmentType { get; set; } = 0;

    /// <summary>
    /// 启用状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int WorkstationStatus { get; set; } = 0;

    /// <summary>
    /// 排序号（回填）
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
