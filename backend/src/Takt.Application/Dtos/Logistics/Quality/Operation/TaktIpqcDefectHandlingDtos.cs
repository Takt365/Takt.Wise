// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Quality.Operation
// 文件名称：TaktIpqcDefectHandlingDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：IpqcDefectHandling 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktIpqcDefectHandling 生成，请按需审阅）
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
// IpqcDefectHandling 响应 DTO
// ========================================

/// <summary>
/// IPQC制程检验不良处理记录实体
/// 对应前端 TaktIpqcDefectHandlingDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktIpqcDefectHandlingDto : TaktCompanyDtoBase
{
    /// <summary>
    /// IpqcDefectHandlingID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long IpqcDefectHandlingId { get; set; }

    /// <summary>
    /// IPQC不良处理编码
    /// </summary>
    public string IpqcDefectHandlingCode { get; set; } = string.Empty;

    /// <summary>
    /// IPQC检验单明细 ID（选项 TaktIpqcOrderItems/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long IpqcOrderItemId { get; set; }

    /// <summary>
    /// IPQC检验单明细 名称（填充字段）
    /// </summary>
    public string? IpqcOrderItemName { get; set; }

    /// <summary>
    /// IPQC检验单编码（冗余字段，便于查询）
    /// </summary>
    public string IpqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 不良类型（0=轻微，1=一般，2=严重，3=致命）
    /// </summary>
    public int DefectType { get; set; } = 0;

    /// <summary>
    /// 不良现象编码
    /// </summary>
    public string DefectCode { get; set; } = string.Empty;

    /// <summary>
    /// 不良现象描述
    /// </summary>
    public string DefectDescription { get; set; } = string.Empty;

    /// <summary>
    /// 不良数量
    /// </summary>
    public int DefectQuantity { get; set; } = 0;

    /// <summary>
    /// 处理方式（0=返工，1=返修，2=让步接收，3=退货，4=报废，5=挑选使用）
    /// </summary>
    public int HandlingMethod { get; set; } = 0;

    /// <summary>
    /// 处理说明
    /// </summary>
    public string? HandlingDescription { get; set; } = string.Empty;

    /// <summary>
    /// 责任部门
    /// </summary>
    public string? ResponsibleDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 责任组编码（选项 TaktQualityGroups/options?qualityGroupCategory=3；DictValue=QualityGroupCode）
    /// </summary>
    public string? ResponsibleGroupCode { get; set; }

    /// <summary>
    /// 责任组名称（冗余：按 ResponsibleGroupCode 取 TaktQualityGroups.QualityGroupName 联动）
    /// </summary>
    public string? ResponsibleGroupName { get; set; }

    /// <summary>
    /// 处理组编码（选项 TaktQualityGroups/options?qualityGroupCategory=4；DictValue=QualityGroupCode）
    /// </summary>
    public string? HandlerGroupCode { get; set; }

    /// <summary>
    /// 处理组名称（冗余：按 HandlerGroupCode 取 TaktQualityGroups.QualityGroupName 联动）
    /// </summary>
    public string? HandlerGroupName { get; set; }

    /// <summary>
    /// 处理时间
    /// </summary>
    public DateTime? HandlingAt { get; set; }

    /// <summary>
    /// 验证部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? VerifyDeptId { get; set; }

    /// <summary>
    /// 验证部门名称（冗余：按 VerifyDeptId 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? VerifyDeptName { get; set; }

    /// <summary>
    /// 质量负责人组编码（选项 TaktQualityGroups/options?qualityGroupCategory=5；DictValue=QualityGroupCode）
    /// </summary>
    public string? QualityOwnerGroupCode { get; set; }

    /// <summary>
    /// 质量负责人组名称（冗余：按 QualityOwnerGroupCode 取 TaktQualityGroups.QualityGroupName 联动）
    /// </summary>
    public string? QualityOwnerGroupName { get; set; }

    /// <summary>
    /// 验证时间
    /// </summary>
    public DateTime? VerifyAt { get; set; }

    /// <summary>
    /// 验证结果（字典 logistics_quality_defect_verify_status；0=待验证 1=合格 2=不合格）
    /// </summary>
    public int VerifyStatus { get; set; } = 0;

    /// <summary>
    /// 预防措施/纠正措施
    /// </summary>
    public string? CorrectiveAction { get; set; } = string.Empty;

    /// <summary>
    /// 不良图片（JSON格式，存储不良图片URL列表）
    /// </summary>
    public string? DefectImages { get; set; } = string.Empty;

    /// <summary>
    /// 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

    /// <summary>
    /// 处理结果（0=待处理，1=处理中，2=已完成，3=已关闭）
    /// </summary>
    public int HandlingStatus { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// IPQC检验单明细（主表）
    /// （主表：TaktIpqcOrderItem）
    /// </summary>
    public TaktIpqcOrderItemDto? OrderItem { get; set; }

}

// ========================================
// IpqcDefectHandling 查询 DTO
// ========================================

/// <summary>
/// IpqcDefectHandling 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktIpqcDefectHandlingQueryDto : TaktPagedQuery
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
    /// IPQC不良处理编码
    /// </summary>
    public string? IpqcDefectHandlingCode { get; set; } = string.Empty;

    /// <summary>
    /// IPQC检验单明细 ID（选项 TaktIpqcOrderItems/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? IpqcOrderItemId { get; set; }

    /// <summary>
    /// IPQC检验单编码（冗余字段，便于查询）
    /// </summary>
    public string? IpqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 不良类型（0=轻微，1=一般，2=严重，3=致命）
    /// </summary>
    public int? DefectType { get; set; }

    /// <summary>
    /// 不良现象编码
    /// </summary>
    public string? DefectCode { get; set; } = string.Empty;

    /// <summary>
    /// 不良现象描述
    /// </summary>
    public string? DefectDescription { get; set; } = string.Empty;

    /// <summary>
    /// 不良数量
    /// </summary>
    public int? DefectQuantity { get; set; }

    /// <summary>
    /// 处理方式（0=返工，1=返修，2=让步接收，3=退货，4=报废，5=挑选使用）
    /// </summary>
    public int? HandlingMethod { get; set; }

    /// <summary>
    /// 处理说明
    /// </summary>
    public string? HandlingDescription { get; set; } = string.Empty;

    /// <summary>
    /// 责任部门
    /// </summary>
    public string? ResponsibleDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 责任组编码（选项 TaktQualityGroups/options?qualityGroupCategory=3；DictValue=QualityGroupCode）
    /// </summary>
    public string? ResponsibleGroupCode { get; set; }

    /// <summary>
    /// 责任组名称（冗余：按 ResponsibleGroupCode 取 TaktQualityGroups.QualityGroupName 联动）
    /// </summary>
    public string? ResponsibleGroupName { get; set; }

    /// <summary>
    /// 处理组编码（选项 TaktQualityGroups/options?qualityGroupCategory=4；DictValue=QualityGroupCode）
    /// </summary>
    public string? HandlerGroupCode { get; set; }

    /// <summary>
    /// 处理组名称（冗余：按 HandlerGroupCode 取 TaktQualityGroups.QualityGroupName 联动）
    /// </summary>
    public string? HandlerGroupName { get; set; }

    /// <summary>
    /// 处理时间（范围查询-开始）
    /// </summary>
    public DateTime? HandlingAtStart { get; set; }

    /// <summary>
    /// 处理时间（范围查询-结束）
    /// </summary>
    public DateTime? HandlingAtEnd { get; set; }

    /// <summary>
    /// 验证部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? VerifyDeptId { get; set; }

    /// <summary>
    /// 验证部门名称（冗余：按 VerifyDeptId 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? VerifyDeptName { get; set; }

    /// <summary>
    /// 质量负责人组编码（选项 TaktQualityGroups/options?qualityGroupCategory=5；DictValue=QualityGroupCode）
    /// </summary>
    public string? QualityOwnerGroupCode { get; set; }

    /// <summary>
    /// 质量负责人组名称（冗余：按 QualityOwnerGroupCode 取 TaktQualityGroups.QualityGroupName 联动）
    /// </summary>
    public string? QualityOwnerGroupName { get; set; }

    /// <summary>
    /// 验证时间（范围查询-开始）
    /// </summary>
    public DateTime? VerifyAtStart { get; set; }

    /// <summary>
    /// 验证时间（范围查询-结束）
    /// </summary>
    public DateTime? VerifyAtEnd { get; set; }

    /// <summary>
    /// 验证结果（字典 logistics_quality_defect_verify_status；0=待验证 1=合格 2=不合格）
    /// </summary>
    public int? VerifyStatus { get; set; }

    /// <summary>
    /// 预防措施/纠正措施
    /// </summary>
    public string? CorrectiveAction { get; set; } = string.Empty;

    /// <summary>
    /// 不良图片（JSON格式，存储不良图片URL列表）
    /// </summary>
    public string? DefectImages { get; set; } = string.Empty;

    /// <summary>
    /// 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

    /// <summary>
    /// 处理结果（0=待处理，1=处理中，2=已完成，3=已关闭）
    /// </summary>
    public int? HandlingStatus { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
// 创建IpqcDefectHandling DTO
// ========================================

/// <summary>
/// 创建IpqcDefectHandling DTO
/// </summary>
public class TaktIpqcDefectHandlingCreateDto
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
    /// IPQC不良处理编码
    /// </summary>
    [Required(ErrorMessage = "IPQC不良处理编码不能为空")]
    public string IpqcDefectHandlingCode { get; set; } = string.Empty;

    /// <summary>
    /// IPQC检验单明细 ID（选项 TaktIpqcOrderItems/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long IpqcOrderItemId { get; set; }

    /// <summary>
    /// IPQC检验单编码（冗余字段，便于查询）
    /// </summary>
    [Required(ErrorMessage = "IPQC检验单编码（冗余字段，便于查询）不能为空")]
    public string IpqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 不良类型（0=轻微，1=一般，2=严重，3=致命）
    /// </summary>
    public int DefectType { get; set; } = 0;

    /// <summary>
    /// 不良现象编码
    /// </summary>
    [Required(ErrorMessage = "不良现象编码不能为空")]
    public string DefectCode { get; set; } = string.Empty;

    /// <summary>
    /// 不良现象描述
    /// </summary>
    [Required(ErrorMessage = "不良现象描述不能为空")]
    public string DefectDescription { get; set; } = string.Empty;

    /// <summary>
    /// 不良数量
    /// </summary>
    public int DefectQuantity { get; set; } = 0;

    /// <summary>
    /// 处理方式（0=返工，1=返修，2=让步接收，3=退货，4=报废，5=挑选使用）
    /// </summary>
    public int HandlingMethod { get; set; } = 0;

    /// <summary>
    /// 处理说明
    /// </summary>
    public string? HandlingDescription { get; set; } = string.Empty;

    /// <summary>
    /// 责任部门
    /// </summary>
    public string? ResponsibleDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 责任组编码（选项 TaktQualityGroups/options?qualityGroupCategory=3；DictValue=QualityGroupCode）
    /// </summary>
    public string? ResponsibleGroupCode { get; set; }

    /// <summary>
    /// 责任组名称（冗余：按 ResponsibleGroupCode 取 TaktQualityGroups.QualityGroupName 联动）
    /// </summary>
    public string? ResponsibleGroupName { get; set; }

    /// <summary>
    /// 处理组编码（选项 TaktQualityGroups/options?qualityGroupCategory=4；DictValue=QualityGroupCode）
    /// </summary>
    public string? HandlerGroupCode { get; set; }

    /// <summary>
    /// 处理组名称（冗余：按 HandlerGroupCode 取 TaktQualityGroups.QualityGroupName 联动）
    /// </summary>
    public string? HandlerGroupName { get; set; }

    /// <summary>
    /// 处理时间
    /// </summary>
    public DateTime? HandlingAt { get; set; }

    /// <summary>
    /// 验证部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? VerifyDeptId { get; set; }

    /// <summary>
    /// 验证部门名称（冗余：按 VerifyDeptId 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? VerifyDeptName { get; set; }

    /// <summary>
    /// 质量负责人组编码（选项 TaktQualityGroups/options?qualityGroupCategory=5；DictValue=QualityGroupCode）
    /// </summary>
    public string? QualityOwnerGroupCode { get; set; }

    /// <summary>
    /// 质量负责人组名称（冗余：按 QualityOwnerGroupCode 取 TaktQualityGroups.QualityGroupName 联动）
    /// </summary>
    public string? QualityOwnerGroupName { get; set; }

    /// <summary>
    /// 验证时间
    /// </summary>
    public DateTime? VerifyAt { get; set; }

    /// <summary>
    /// 验证结果（字典 logistics_quality_defect_verify_status；0=待验证 1=合格 2=不合格）
    /// </summary>
    public int VerifyStatus { get; set; } = 0;

    /// <summary>
    /// 预防措施/纠正措施
    /// </summary>
    public string? CorrectiveAction { get; set; } = string.Empty;

    /// <summary>
    /// 不良图片（JSON格式，存储不良图片URL列表）
    /// </summary>
    public string? DefectImages { get; set; } = string.Empty;

    /// <summary>
    /// 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

    /// <summary>
    /// 处理结果（0=待处理，1=处理中，2=已完成，3=已关闭）
    /// </summary>
    public int HandlingStatus { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

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
// 更新IpqcDefectHandling DTO
// ========================================

/// <summary>
/// 更新IpqcDefectHandling DTO
/// 继承 TaktIpqcDefectHandlingCreateDto，添加 IpqcDefectHandlingId 字段
/// </summary>
public class TaktIpqcDefectHandlingUpdateDto : TaktIpqcDefectHandlingCreateDto
{
    /// <summary>
    /// IpqcDefectHandlingID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long IpqcDefectHandlingId { get; set; }

}

// ========================================
// IpqcDefectHandling 状态 DTO
// ========================================

/// <summary>
/// IpqcDefectHandling 状态更新 DTO
/// </summary>
public class TaktIpqcDefectHandlingStatusDto
{
    /// <summary>
    /// IpqcDefectHandlingID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long IpqcDefectHandlingId { get; set; }

    /// <summary>
    /// 处理结果（0=待处理，1=处理中，2=已完成，3=已关闭）
    /// </summary>
    [Required(ErrorMessage = "处理结果（0=待处理，1=处理中，2=已完成，3=已关闭）不能为空")]
    public int HandlingStatus { get; set; } = 0;
}

// ========================================
// IpqcDefectHandling 作废 DTO
// ========================================

/// <summary>
/// IpqcDefectHandling 作废/撤销作废 DTO
/// </summary>
public class TaktIpqcDefectHandlingObsoleteDto
{
    /// <summary>
    /// IpqcDefectHandlingID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long IpqcDefectHandlingId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// IpqcDefectHandling 导入模板行 DTO
/// </summary>
public class TaktIpqcDefectHandlingTemplateDto
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
    /// IPQC不良处理编码
    /// </summary>
    public string? IpqcDefectHandlingCode { get; set; } = string.Empty;

    /// <summary>
    /// IPQC检验单明细 ID（选项 TaktIpqcOrderItems/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? IpqcOrderItemId { get; set; }

    /// <summary>
    /// IPQC检验单编码（冗余字段，便于查询）
    /// </summary>
    public string? IpqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 不良类型（0=轻微，1=一般，2=严重，3=致命）
    /// </summary>
    public int? DefectType { get; set; }

    /// <summary>
    /// 不良现象编码
    /// </summary>
    public string? DefectCode { get; set; } = string.Empty;

    /// <summary>
    /// 不良现象描述
    /// </summary>
    public string? DefectDescription { get; set; } = string.Empty;

    /// <summary>
    /// 不良数量
    /// </summary>
    public int? DefectQuantity { get; set; }

    /// <summary>
    /// 处理方式（0=返工，1=返修，2=让步接收，3=退货，4=报废，5=挑选使用）
    /// </summary>
    public int? HandlingMethod { get; set; }

    /// <summary>
    /// 处理说明
    /// </summary>
    public string? HandlingDescription { get; set; } = string.Empty;

    /// <summary>
    /// 责任部门
    /// </summary>
    public string? ResponsibleDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 责任组编码（选项 TaktQualityGroups/options?qualityGroupCategory=3；DictValue=QualityGroupCode）
    /// </summary>
    public string? ResponsibleGroupCode { get; set; }

    /// <summary>
    /// 责任组名称（冗余：按 ResponsibleGroupCode 取 TaktQualityGroups.QualityGroupName 联动）
    /// </summary>
    public string? ResponsibleGroupName { get; set; }

    /// <summary>
    /// 处理组编码（选项 TaktQualityGroups/options?qualityGroupCategory=4；DictValue=QualityGroupCode）
    /// </summary>
    public string? HandlerGroupCode { get; set; }

    /// <summary>
    /// 处理组名称（冗余：按 HandlerGroupCode 取 TaktQualityGroups.QualityGroupName 联动）
    /// </summary>
    public string? HandlerGroupName { get; set; }

    /// <summary>
    /// 处理时间
    /// </summary>
    public DateTime? HandlingAt { get; set; }

    /// <summary>
    /// 验证部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? VerifyDeptId { get; set; }

    /// <summary>
    /// 验证部门名称（冗余：按 VerifyDeptId 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? VerifyDeptName { get; set; }

    /// <summary>
    /// 质量负责人组编码（选项 TaktQualityGroups/options?qualityGroupCategory=5；DictValue=QualityGroupCode）
    /// </summary>
    public string? QualityOwnerGroupCode { get; set; }

    /// <summary>
    /// 质量负责人组名称（冗余：按 QualityOwnerGroupCode 取 TaktQualityGroups.QualityGroupName 联动）
    /// </summary>
    public string? QualityOwnerGroupName { get; set; }

    /// <summary>
    /// 验证时间
    /// </summary>
    public DateTime? VerifyAt { get; set; }

    /// <summary>
    /// 验证结果（字典 logistics_quality_defect_verify_status；0=待验证 1=合格 2=不合格）
    /// </summary>
    public int VerifyStatus { get; set; } = 0;

    /// <summary>
    /// 预防措施/纠正措施
    /// </summary>
    public string? CorrectiveAction { get; set; } = string.Empty;

    /// <summary>
    /// 不良图片（JSON格式，存储不良图片URL列表）
    /// </summary>
    public string? DefectImages { get; set; } = string.Empty;

    /// <summary>
    /// 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

    /// <summary>
    /// 处理结果（0=待处理，1=处理中，2=已完成，3=已关闭）
    /// </summary>
    public int? HandlingStatus { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
/// IpqcDefectHandling 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktIpqcDefectHandlingImportDto
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
    /// IPQC不良处理编码
    /// </summary>
    public string? IpqcDefectHandlingCode { get; set; } = string.Empty;

    /// <summary>
    /// IPQC检验单明细 ID（选项 TaktIpqcOrderItems/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? IpqcOrderItemId { get; set; }

    /// <summary>
    /// IPQC检验单编码（冗余字段，便于查询）
    /// </summary>
    public string? IpqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 不良类型（0=轻微，1=一般，2=严重，3=致命）
    /// </summary>
    public int? DefectType { get; set; }

    /// <summary>
    /// 不良现象编码
    /// </summary>
    public string? DefectCode { get; set; } = string.Empty;

    /// <summary>
    /// 不良现象描述
    /// </summary>
    public string? DefectDescription { get; set; } = string.Empty;

    /// <summary>
    /// 不良数量
    /// </summary>
    public int? DefectQuantity { get; set; }

    /// <summary>
    /// 处理方式（0=返工，1=返修，2=让步接收，3=退货，4=报废，5=挑选使用）
    /// </summary>
    public int? HandlingMethod { get; set; }

    /// <summary>
    /// 处理说明
    /// </summary>
    public string? HandlingDescription { get; set; } = string.Empty;

    /// <summary>
    /// 责任部门
    /// </summary>
    public string? ResponsibleDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 责任组编码（选项 TaktQualityGroups/options?qualityGroupCategory=3；DictValue=QualityGroupCode）
    /// </summary>
    public string? ResponsibleGroupCode { get; set; }

    /// <summary>
    /// 责任组名称（冗余：按 ResponsibleGroupCode 取 TaktQualityGroups.QualityGroupName 联动）
    /// </summary>
    public string? ResponsibleGroupName { get; set; }

    /// <summary>
    /// 处理组编码（选项 TaktQualityGroups/options?qualityGroupCategory=4；DictValue=QualityGroupCode）
    /// </summary>
    public string? HandlerGroupCode { get; set; }

    /// <summary>
    /// 处理组名称（冗余：按 HandlerGroupCode 取 TaktQualityGroups.QualityGroupName 联动）
    /// </summary>
    public string? HandlerGroupName { get; set; }

    /// <summary>
    /// 处理时间
    /// </summary>
    public DateTime? HandlingAt { get; set; }

    /// <summary>
    /// 验证部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? VerifyDeptId { get; set; }

    /// <summary>
    /// 验证部门名称（冗余：按 VerifyDeptId 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? VerifyDeptName { get; set; }

    /// <summary>
    /// 质量负责人组编码（选项 TaktQualityGroups/options?qualityGroupCategory=5；DictValue=QualityGroupCode）
    /// </summary>
    public string? QualityOwnerGroupCode { get; set; }

    /// <summary>
    /// 质量负责人组名称（冗余：按 QualityOwnerGroupCode 取 TaktQualityGroups.QualityGroupName 联动）
    /// </summary>
    public string? QualityOwnerGroupName { get; set; }

    /// <summary>
    /// 验证时间
    /// </summary>
    public DateTime? VerifyAt { get; set; }

    /// <summary>
    /// 验证结果（字典 logistics_quality_defect_verify_status；0=待验证 1=合格 2=不合格）
    /// </summary>
    public int VerifyStatus { get; set; } = 0;

    /// <summary>
    /// 预防措施/纠正措施
    /// </summary>
    public string? CorrectiveAction { get; set; } = string.Empty;

    /// <summary>
    /// 不良图片（JSON格式，存储不良图片URL列表）
    /// </summary>
    public string? DefectImages { get; set; } = string.Empty;

    /// <summary>
    /// 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

    /// <summary>
    /// 处理结果（0=待处理，1=处理中，2=已完成，3=已关闭）
    /// </summary>
    public int? HandlingStatus { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
/// IpqcDefectHandling 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktIpqcDefectHandlingExportDto
{
    /// <summary>
    /// IpqcDefectHandlingID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long IpqcDefectHandlingId { get; set; }

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
    /// IPQC不良处理编码
    /// </summary>
    public string IpqcDefectHandlingCode { get; set; } = string.Empty;

    /// <summary>
    /// IPQC检验单明细 ID（选项 TaktIpqcOrderItems/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long IpqcOrderItemId { get; set; }

    /// <summary>
    /// IPQC检验单编码（冗余字段，便于查询）
    /// </summary>
    public string IpqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 不良类型（0=轻微，1=一般，2=严重，3=致命）
    /// </summary>
    public int DefectType { get; set; } = 0;

    /// <summary>
    /// 不良现象编码
    /// </summary>
    public string DefectCode { get; set; } = string.Empty;

    /// <summary>
    /// 不良现象描述
    /// </summary>
    public string DefectDescription { get; set; } = string.Empty;

    /// <summary>
    /// 不良数量
    /// </summary>
    public int DefectQuantity { get; set; } = 0;

    /// <summary>
    /// 处理方式（0=返工，1=返修，2=让步接收，3=退货，4=报废，5=挑选使用）
    /// </summary>
    public int HandlingMethod { get; set; } = 0;

    /// <summary>
    /// 处理说明
    /// </summary>
    public string? HandlingDescription { get; set; } = string.Empty;

    /// <summary>
    /// 责任部门
    /// </summary>
    public string? ResponsibleDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 责任组编码（选项 TaktQualityGroups/options?qualityGroupCategory=3；DictValue=QualityGroupCode）
    /// </summary>
    public string? ResponsibleGroupCode { get; set; }

    /// <summary>
    /// 责任组名称（冗余：按 ResponsibleGroupCode 取 TaktQualityGroups.QualityGroupName 联动）
    /// </summary>
    public string? ResponsibleGroupName { get; set; }

    /// <summary>
    /// 处理组编码（选项 TaktQualityGroups/options?qualityGroupCategory=4；DictValue=QualityGroupCode）
    /// </summary>
    public string? HandlerGroupCode { get; set; }

    /// <summary>
    /// 处理组名称（冗余：按 HandlerGroupCode 取 TaktQualityGroups.QualityGroupName 联动）
    /// </summary>
    public string? HandlerGroupName { get; set; }

    /// <summary>
    /// 处理时间
    /// </summary>
    public DateTime? HandlingAt { get; set; }

    /// <summary>
    /// 验证部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? VerifyDeptId { get; set; }

    /// <summary>
    /// 验证部门名称（冗余：按 VerifyDeptId 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? VerifyDeptName { get; set; }

    /// <summary>
    /// 质量负责人组编码（选项 TaktQualityGroups/options?qualityGroupCategory=5；DictValue=QualityGroupCode）
    /// </summary>
    public string? QualityOwnerGroupCode { get; set; }

    /// <summary>
    /// 质量负责人组名称（冗余：按 QualityOwnerGroupCode 取 TaktQualityGroups.QualityGroupName 联动）
    /// </summary>
    public string? QualityOwnerGroupName { get; set; }

    /// <summary>
    /// 验证时间
    /// </summary>
    public DateTime? VerifyAt { get; set; }

    /// <summary>
    /// 验证结果（字典 logistics_quality_defect_verify_status；0=待验证 1=合格 2=不合格）
    /// </summary>
    public int VerifyStatus { get; set; } = 0;

    /// <summary>
    /// 预防措施/纠正措施
    /// </summary>
    public string? CorrectiveAction { get; set; } = string.Empty;

    /// <summary>
    /// 不良图片（JSON格式，存储不良图片URL列表）
    /// </summary>
    public string? DefectImages { get; set; } = string.Empty;

    /// <summary>
    /// 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

    /// <summary>
    /// 处理结果（0=待处理，1=处理中，2=已完成，3=已关闭）
    /// </summary>
    public int HandlingStatus { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

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
