// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcIqcDtos.cs
// 创建时间：2026-10-29
// 创建人：Takt365(Auto Generated)
// 功能描述：EcIqc 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktEcIqc 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;

// ========================================
// EcIqc 响应 DTO
// ========================================

/// <summary>
/// 设变受检课（部门 D0810）执行表。新品检验为是且新物料有效才落行。按设变单号去重一行；机种/根物料/新物料写入编码组
/// 对应前端 TaktEcIqcDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktEcIqcDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EcIqcID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcIqcId { get; set; }

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 停产状态（字典 logistics_materials_material_discontinued_status）。同设变下任意成品未停产时为 Z0，全部停产时取保留明细状态。
    /// </summary>
    public string DiscontinuedStatus { get; set; } = string.Empty;

    /// <summary>
    /// 设变单号（冗余：来自 TaktEcDetail.EcCode）
    /// </summary>
    public string EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码组（同设变下需检验明细的机种编码，分号分隔；ntext）
    /// </summary>
    public string EcModelCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 根物料编码组（同设变下需检验明细的根物料编码，分号分隔；ntext）
    /// </summary>
    public string EcRootMaterialCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 新物料编码组（同设变下全部新品检验物料编码，分号分隔；ntext）
    /// </summary>
    public string EcNewMaterialCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 新品检验（检查；冗余：来自保留明细；落行为 1）
    /// </summary>
    public int EcNewRequiresInspection { get; set; } = 0;

    /// <summary>
    /// 是否实施（0=否 1=是，字典 sys_yes_no）
    /// </summary>
    public int IsImplemented { get; set; } = 0;

    /// <summary>
    /// 检验日期
    /// </summary>
    public DateTime? InspectionDate { get; set; }

    /// <summary>
    /// 执行内容（按新物料编码组拆行：物料:检验单10位:序号4位:PASS; 多行组合；缺省 4300000000:0010；nvarchar(max)）
    /// </summary>
    public string? ExecContent { get; set; } = string.Empty;

    /// <summary>
    /// 部门编码（TaktDept.DeptCode；本表固定课别）
    /// </summary>
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 实施范围（冗余：来自 TaktEcDetail.EcImplScope）
    /// </summary>
    public int EcImplScope { get; set; } = 0;

    /// <summary>
    /// 控制范围（字典 logistics_manufacturing_ec_control_scope_category；受检固定按设变 DictValue=3）
    /// </summary>
    public int EcControlScope { get; set; } = 0;

    /// <summary>
    /// 执行行去重键（受检为设变单号；同一键的明细共用本行。ntext）
    /// </summary>
    public string EcGrainKey { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 设变明细列表（视图主从：执行表为主；一对多；新物料以编码组为准）
    /// </summary>
    public List<TaktEcDetailDto>? EcDetails { get; set; }

}

// ========================================
// EcIqc 查询 DTO
// ========================================

/// <summary>
/// EcIqc 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktEcIqcQueryDto : TaktPagedQuery
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
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 停产状态（字典 logistics_materials_material_discontinued_status）。同设变下任意成品未停产时为 Z0，全部停产时取保留明细状态。
    /// </summary>
    public string? DiscontinuedStatus { get; set; } = string.Empty;

    /// <summary>
    /// 设变单号（冗余：来自 TaktEcDetail.EcCode）
    /// </summary>
    public string? EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码组（同设变下需检验明细的机种编码，分号分隔；ntext）
    /// </summary>
    public string? EcModelCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 根物料编码组（同设变下需检验明细的根物料编码，分号分隔；ntext）
    /// </summary>
    public string? EcRootMaterialCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 新物料编码组（同设变下全部新品检验物料编码，分号分隔；ntext）
    /// </summary>
    public string? EcNewMaterialCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 新品检验（检查；冗余：来自保留明细；落行为 1）
    /// </summary>
    public int? EcNewRequiresInspection { get; set; }

    /// <summary>
    /// 是否实施（0=否 1=是，字典 sys_yes_no）
    /// </summary>
    public int? IsImplemented { get; set; }

    /// <summary>
    /// 检验日期（范围查询-开始）
    /// </summary>
    public DateTime? InspectionDateStart { get; set; }

    /// <summary>
    /// 检验日期（范围查询-结束）
    /// </summary>
    public DateTime? InspectionDateEnd { get; set; }

    /// <summary>
    /// 执行内容（按新物料编码组拆行：物料:检验单10位:序号4位:PASS; 多行组合；缺省 4300000000:0010；nvarchar(max)）
    /// </summary>
    public string? ExecContent { get; set; } = string.Empty;

    /// <summary>
    /// 部门编码（TaktDept.DeptCode；本表固定课别）
    /// </summary>
    public string? DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 实施范围（冗余：来自 TaktEcDetail.EcImplScope）
    /// </summary>
    public int? EcImplScope { get; set; }

    /// <summary>
    /// 控制范围（字典 logistics_manufacturing_ec_control_scope_category；受检固定按设变 DictValue=3）
    /// </summary>
    public int? EcControlScope { get; set; }

    /// <summary>
    /// 执行行去重键（受检为设变单号；同一键的明细共用本行。ntext）
    /// </summary>
    public string? EcGrainKey { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是）
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
// 创建EcIqc DTO
// ========================================

/// <summary>
/// 创建EcIqc DTO
/// </summary>
public class TaktEcIqcCreateDto
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
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 停产状态（字典 logistics_materials_material_discontinued_status）。同设变下任意成品未停产时为 Z0，全部停产时取保留明细状态。
    /// </summary>
    [Required(ErrorMessage = "停产状态（字典 logistics_materials_material_discontinued_status）。同设变下任意成品未停产时为 Z0，全部停产时取保留明细状态。不能为空")]
    public string DiscontinuedStatus { get; set; } = string.Empty;

    /// <summary>
    /// 设变单号（冗余：来自 TaktEcDetail.EcCode）
    /// </summary>
    [Required(ErrorMessage = "设变单号（冗余：来自 TaktEcDetail.EcCode）不能为空")]
    public string EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码组（同设变下需检验明细的机种编码，分号分隔；ntext）
    /// </summary>
    [Required(ErrorMessage = "机种编码组（同设变下需检验明细的机种编码，分号分隔；ntext）不能为空")]
    public string EcModelCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 根物料编码组（同设变下需检验明细的根物料编码，分号分隔；ntext）
    /// </summary>
    [Required(ErrorMessage = "根物料编码组（同设变下需检验明细的根物料编码，分号分隔；ntext）不能为空")]
    public string EcRootMaterialCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 新物料编码组（同设变下全部新品检验物料编码，分号分隔；ntext）
    /// </summary>
    [Required(ErrorMessage = "新物料编码组（同设变下全部新品检验物料编码，分号分隔；ntext）不能为空")]
    public string EcNewMaterialCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 新品检验（检查；冗余：来自保留明细；落行为 1）
    /// </summary>
    public int EcNewRequiresInspection { get; set; } = 0;

    /// <summary>
    /// 是否实施（0=否 1=是，字典 sys_yes_no）
    /// </summary>
    public int IsImplemented { get; set; } = 0;

    /// <summary>
    /// 检验日期
    /// </summary>
    public DateTime? InspectionDate { get; set; }

    /// <summary>
    /// 执行内容（按新物料编码组拆行：物料:检验单10位:序号4位:PASS; 多行组合；缺省 4300000000:0010；nvarchar(max)）
    /// </summary>
    public string? ExecContent { get; set; } = string.Empty;

    /// <summary>
    /// 部门编码（TaktDept.DeptCode；本表固定课别）
    /// </summary>
    [Required(ErrorMessage = "部门编码（TaktDept.DeptCode；本表固定课别）不能为空")]
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）
    /// </summary>
    [Required(ErrorMessage = "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）不能为空")]
    public string DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 实施范围（冗余：来自 TaktEcDetail.EcImplScope）
    /// </summary>
    public int EcImplScope { get; set; } = 0;

    /// <summary>
    /// 控制范围（字典 logistics_manufacturing_ec_control_scope_category；受检固定按设变 DictValue=3）
    /// </summary>
    public int EcControlScope { get; set; } = 0;

    /// <summary>
    /// 执行行去重键（受检为设变单号；同一键的明细共用本行。ntext）
    /// </summary>
    [Required(ErrorMessage = "执行行去重键（受检为设变单号；同一键的明细共用本行。ntext）不能为空")]
    public string EcGrainKey { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是）
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
// 更新EcIqc DTO
// ========================================

/// <summary>
/// 更新EcIqc DTO
/// 继承 TaktEcIqcCreateDto，添加 EcIqcId 字段
/// </summary>
public class TaktEcIqcUpdateDto : TaktEcIqcCreateDto
{
    /// <summary>
    /// EcIqcID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcIqcId { get; set; }

}

// ========================================
// EcIqc 状态 DTO
// ========================================

/// <summary>
/// EcIqc 状态更新 DTO
/// </summary>
public class TaktEcIqcStatusDto
{
    /// <summary>
    /// EcIqcID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcIqcId { get; set; }

    /// <summary>
    /// 停产状态（字典 logistics_materials_material_discontinued_status）。同设变下任意成品未停产时为 Z0，全部停产时取保留明细状态。
    /// </summary>
    [Required(ErrorMessage = "停产状态（字典 logistics_materials_material_discontinued_status）。同设变下任意成品未停产时为 Z0，全部停产时取保留明细状态。不能为空")]
    public string DiscontinuedStatus { get; set; } = string.Empty;
}

/// <summary>
/// EcIqc 停产状态 DTO
/// </summary>
public class TaktEcIqcDiscontinuedStatusDto
{
    /// <summary>
    /// EcIqcID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcIqcId { get; set; }

    /// <summary>
    /// 根物料停产状态（字典 logistics_materials_material_discontinued_status；Z0=在产；停产按钮默认 ZQ）
    /// </summary>
    [Required(ErrorMessage = "停产状态不能为空")]
    public string DiscontinuedStatus { get; set; } = "Z0";
    /// <summary>
    /// 实施范围（冗余：来自 TaktEcDetail.EcImplScope）
    /// </summary>
    public int EcImplScope { get; set; }
    /// <summary>
    /// 控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品，2=按机种，3=按设变，4=不管理），只记录生管/制二/制一/品管；全仕向时生管与此相同。采购/受检/部管/SMT 固定按设变，制技固定按机种
    /// </summary>
    public int EcControlScope { get; set; } = 2;
}

// ========================================
// EcIqc 作废 DTO
// ========================================

/// <summary>
/// EcIqc 作废/撤销作废 DTO
/// </summary>
public class TaktEcIqcObsoleteDto
{
    /// <summary>
    /// EcIqcID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcIqcId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// EcIqc 导入模板行 DTO
/// </summary>
public class TaktEcIqcTemplateDto
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
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 停产状态（字典 logistics_materials_material_discontinued_status）。同设变下任意成品未停产时为 Z0，全部停产时取保留明细状态。
    /// </summary>
    public string? DiscontinuedStatus { get; set; } = string.Empty;

    /// <summary>
    /// 设变单号（冗余：来自 TaktEcDetail.EcCode）
    /// </summary>
    public string? EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码组（同设变下需检验明细的机种编码，分号分隔；ntext）
    /// </summary>
    public string? EcModelCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 根物料编码组（同设变下需检验明细的根物料编码，分号分隔；ntext）
    /// </summary>
    public string? EcRootMaterialCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 新物料编码组（同设变下全部新品检验物料编码，分号分隔；ntext）
    /// </summary>
    public string? EcNewMaterialCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 新品检验（检查；冗余：来自保留明细；落行为 1）
    /// </summary>
    public int? EcNewRequiresInspection { get; set; }

    /// <summary>
    /// 是否实施（0=否 1=是，字典 sys_yes_no）
    /// </summary>
    public int? IsImplemented { get; set; }

    /// <summary>
    /// 检验日期
    /// </summary>
    public DateTime? InspectionDate { get; set; }

    /// <summary>
    /// 执行内容（按新物料编码组拆行：物料:检验单10位:序号4位:PASS; 多行组合；缺省 4300000000:0010；nvarchar(max)）
    /// </summary>
    public string? ExecContent { get; set; } = string.Empty;

    /// <summary>
    /// 部门编码（TaktDept.DeptCode；本表固定课别）
    /// </summary>
    public string? DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 实施范围（冗余：来自 TaktEcDetail.EcImplScope）
    /// </summary>
    public int? EcImplScope { get; set; }

    /// <summary>
    /// 控制范围（字典 logistics_manufacturing_ec_control_scope_category；受检固定按设变 DictValue=3）
    /// </summary>
    public int? EcControlScope { get; set; }

    /// <summary>
    /// 执行行去重键（受检为设变单号；同一键的明细共用本行。ntext）
    /// </summary>
    public string? EcGrainKey { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是）
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
/// EcIqc 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktEcIqcImportDto
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
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 停产状态（字典 logistics_materials_material_discontinued_status）。同设变下任意成品未停产时为 Z0，全部停产时取保留明细状态。
    /// </summary>
    public string? DiscontinuedStatus { get; set; } = string.Empty;

    /// <summary>
    /// 设变单号（冗余：来自 TaktEcDetail.EcCode）
    /// </summary>
    public string? EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码组（同设变下需检验明细的机种编码，分号分隔；ntext）
    /// </summary>
    public string? EcModelCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 根物料编码组（同设变下需检验明细的根物料编码，分号分隔；ntext）
    /// </summary>
    public string? EcRootMaterialCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 新物料编码组（同设变下全部新品检验物料编码，分号分隔；ntext）
    /// </summary>
    public string? EcNewMaterialCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 新品检验（检查；冗余：来自保留明细；落行为 1）
    /// </summary>
    public int? EcNewRequiresInspection { get; set; }

    /// <summary>
    /// 是否实施（0=否 1=是，字典 sys_yes_no）
    /// </summary>
    public int? IsImplemented { get; set; }

    /// <summary>
    /// 检验日期
    /// </summary>
    public DateTime? InspectionDate { get; set; }

    /// <summary>
    /// 执行内容（按新物料编码组拆行：物料:检验单10位:序号4位:PASS; 多行组合；缺省 4300000000:0010；nvarchar(max)）
    /// </summary>
    public string? ExecContent { get; set; } = string.Empty;

    /// <summary>
    /// 部门编码（TaktDept.DeptCode；本表固定课别）
    /// </summary>
    public string? DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 实施范围（冗余：来自 TaktEcDetail.EcImplScope）
    /// </summary>
    public int? EcImplScope { get; set; }

    /// <summary>
    /// 控制范围（字典 logistics_manufacturing_ec_control_scope_category；受检固定按设变 DictValue=3）
    /// </summary>
    public int? EcControlScope { get; set; }

    /// <summary>
    /// 执行行去重键（受检为设变单号；同一键的明细共用本行。ntext）
    /// </summary>
    public string? EcGrainKey { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是）
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
/// EcIqc 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktEcIqcExportDto
{
    /// <summary>
    /// EcIqcID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcIqcId { get; set; }

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
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 停产状态（字典 logistics_materials_material_discontinued_status）。同设变下任意成品未停产时为 Z0，全部停产时取保留明细状态。
    /// </summary>
    public string DiscontinuedStatus { get; set; } = string.Empty;

    /// <summary>
    /// 设变单号（冗余：来自 TaktEcDetail.EcCode）
    /// </summary>
    public string EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码组（同设变下需检验明细的机种编码，分号分隔；ntext）
    /// </summary>
    public string EcModelCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 根物料编码组（同设变下需检验明细的根物料编码，分号分隔；ntext）
    /// </summary>
    public string EcRootMaterialCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 新物料编码组（同设变下全部新品检验物料编码，分号分隔；ntext）
    /// </summary>
    public string EcNewMaterialCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 新品检验（检查；冗余：来自保留明细；落行为 1）
    /// </summary>
    public int EcNewRequiresInspection { get; set; } = 0;

    /// <summary>
    /// 是否实施（0=否 1=是，字典 sys_yes_no）
    /// </summary>
    public int IsImplemented { get; set; } = 0;

    /// <summary>
    /// 检验日期
    /// </summary>
    public DateTime? InspectionDate { get; set; }

    /// <summary>
    /// 执行内容（按新物料编码组拆行：物料:检验单10位:序号4位:PASS; 多行组合；缺省 4300000000:0010；nvarchar(max)）
    /// </summary>
    public string? ExecContent { get; set; } = string.Empty;

    /// <summary>
    /// 部门编码（TaktDept.DeptCode；本表固定课别）
    /// </summary>
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 实施范围（冗余：来自 TaktEcDetail.EcImplScope）
    /// </summary>
    public int EcImplScope { get; set; } = 0;

    /// <summary>
    /// 控制范围（字典 logistics_manufacturing_ec_control_scope_category；受检固定按设变 DictValue=3）
    /// </summary>
    public int EcControlScope { get; set; } = 0;

    /// <summary>
    /// 执行行去重键（受检为设变单号；同一键的明细共用本行。ntext）
    /// </summary>
    public string EcGrainKey { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是）
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
