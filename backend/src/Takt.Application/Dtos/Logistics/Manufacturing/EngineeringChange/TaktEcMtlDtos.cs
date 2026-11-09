// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcMtlDtos.cs
// 创建时间：2026-10-29
// 创建人：Takt365(Auto Generated)
// 功能描述：EcMtl 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktEcMtl 生成，请按需审阅）
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
// EcMtl 响应 DTO
// ========================================

/// <summary>
/// 设变物料（部门 D0625）执行表。采购类型 F 且仓库 C003、新物料有效才落行。按设变单号+上阶物料去重一行；该上阶下全部 F+C003 新物料写入编码组
/// 对应前端 TaktEcMtlDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktEcMtlDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EcMtlID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcMtlId { get; set; }

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 设变单号（冗余：来自 TaktEcDetail.EcCode）
    /// </summary>
    public string EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）
    /// </summary>
    public string DiscontinuedStatus { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码组（同去重键下的机种编码，分号分隔；ntext）
    /// </summary>
    public string EcModelCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 根物料编码组（同去重键下的根物料编码，分号分隔；ntext）
    /// </summary>
    public string EcRootMaterialCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 新物料编码组（该上阶物料下采购类型 F、仓库 C003 的全部新物料编码，分号分隔；ntext）
    /// </summary>
    public string EcNewMaterialCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料编码（去重键组成部分；冗余：来自 TaktEcDetail.EcParentMaterialCode）
    /// </summary>
    public string EcParentMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）
    /// </summary>
    public string EcParentMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 新采购类型（冗余：来自保留明细；落行均为 F）
    /// </summary>
    public string EcNewPurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 新品仓库（冗余：来自保留明细；均为 C003）
    /// </summary>
    public string EcNewWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// 预定日期（冗余：来自 TaktEcPc.ScheduledDate）
    /// </summary>
    public DateTime? ScheduledDate { get; set; }

    /// <summary>
    /// 预定批次（冗余：来自 TaktEcPc.ScheduledBatch）
    /// </summary>
    public string? ScheduledBatch { get; set; } = string.Empty;

    /// <summary>
    /// 是否实施（0=否 1=是，字典 sys_yes_no）
    /// </summary>
    public int IsImplemented { get; set; } = 0;

    /// <summary>
    /// 出库日期
    /// </summary>
    public DateTime? OutboundDate { get; set; }

    /// <summary>
    /// 出库批次
    /// </summary>
    public string? OutboundBatch { get; set; } = string.Empty;

    /// <summary>
    /// 执行内容（各部门通用；ntext）
    /// </summary>
    public string? ExecContent { get; set; } = string.Empty;

    /// <summary>
    /// 部门编码（固定 D0625 物料）
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
    /// 控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品 2=按机种 3=按设变；跟随主表选中值）
    /// </summary>
    public int EcControlScope { get; set; } = 0;

    /// <summary>
    /// 执行行去重键（按控制范围：按成品/按机种/按设变；同一键的明细共用本行。ntext）
    /// </summary>
    public string EcGrainKey { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 设变明细列表（视图主从：执行表为主；一对多；业务键 EcCode + EcModelCode + EcRootMaterialCode）
    /// </summary>
    public List<TaktEcDetailDto>? EcDetails { get; set; }

}

// ========================================
// EcMtl 查询 DTO
// ========================================

/// <summary>
/// EcMtl 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktEcMtlQueryDto : TaktPagedQuery
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
    /// 设变单号（冗余：来自 TaktEcDetail.EcCode）
    /// </summary>
    public string? EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）
    /// </summary>
    public string? DiscontinuedStatus { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码组（同去重键下的机种编码，分号分隔；ntext）
    /// </summary>
    public string? EcModelCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 根物料编码组（同去重键下的根物料编码，分号分隔；ntext）
    /// </summary>
    public string? EcRootMaterialCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 新物料编码组（该上阶物料下采购类型 F、仓库 C003 的全部新物料编码，分号分隔；ntext）
    /// </summary>
    public string? EcNewMaterialCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料编码（去重键组成部分；冗余：来自 TaktEcDetail.EcParentMaterialCode）
    /// </summary>
    public string? EcParentMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）
    /// </summary>
    public string? EcParentMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 新采购类型（冗余：来自保留明细；落行均为 F）
    /// </summary>
    public string? EcNewPurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 新品仓库（冗余：来自保留明细；均为 C003）
    /// </summary>
    public string? EcNewWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// 预定日期（冗余：来自 TaktEcPc.ScheduledDate）（范围查询-开始）
    /// </summary>
    public DateTime? ScheduledDateStart { get; set; }

    /// <summary>
    /// 预定日期（冗余：来自 TaktEcPc.ScheduledDate）（范围查询-结束）
    /// </summary>
    public DateTime? ScheduledDateEnd { get; set; }

    /// <summary>
    /// 预定批次（冗余：来自 TaktEcPc.ScheduledBatch）
    /// </summary>
    public string? ScheduledBatch { get; set; } = string.Empty;

    /// <summary>
    /// 是否实施（0=否 1=是，字典 sys_yes_no）
    /// </summary>
    public int? IsImplemented { get; set; }

    /// <summary>
    /// 出库日期（范围查询-开始）
    /// </summary>
    public DateTime? OutboundDateStart { get; set; }

    /// <summary>
    /// 出库日期（范围查询-结束）
    /// </summary>
    public DateTime? OutboundDateEnd { get; set; }

    /// <summary>
    /// 出库批次
    /// </summary>
    public string? OutboundBatch { get; set; } = string.Empty;

    /// <summary>
    /// 执行内容（各部门通用；ntext）
    /// </summary>
    public string? ExecContent { get; set; } = string.Empty;

    /// <summary>
    /// 部门编码（固定 D0625 物料）
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
    /// 控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品 2=按机种 3=按设变；跟随主表选中值）
    /// </summary>
    public int? EcControlScope { get; set; }

    /// <summary>
    /// 执行行去重键（按控制范围：按成品/按机种/按设变；同一键的明细共用本行。ntext）
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
// 创建EcMtl DTO
// ========================================

/// <summary>
/// 创建EcMtl DTO
/// </summary>
public class TaktEcMtlCreateDto
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
    /// 设变单号（冗余：来自 TaktEcDetail.EcCode）
    /// </summary>
    [Required(ErrorMessage = "设变单号（冗余：来自 TaktEcDetail.EcCode）不能为空")]
    public string EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）
    /// </summary>
    [Required(ErrorMessage = "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）不能为空")]
    public string DiscontinuedStatus { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码组（同去重键下的机种编码，分号分隔；ntext）
    /// </summary>
    [Required(ErrorMessage = "机种编码组（同去重键下的机种编码，分号分隔；ntext）不能为空")]
    public string EcModelCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 根物料编码组（同去重键下的根物料编码，分号分隔；ntext）
    /// </summary>
    [Required(ErrorMessage = "根物料编码组（同去重键下的根物料编码，分号分隔；ntext）不能为空")]
    public string EcRootMaterialCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 新物料编码组（该上阶物料下采购类型 F、仓库 C003 的全部新物料编码，分号分隔；ntext）
    /// </summary>
    [Required(ErrorMessage = "新物料编码组（该上阶物料下采购类型 F、仓库 C003 的全部新物料编码，分号分隔；ntext）不能为空")]
    public string EcNewMaterialCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料编码（去重键组成部分；冗余：来自 TaktEcDetail.EcParentMaterialCode）
    /// </summary>
    [Required(ErrorMessage = "上阶物料编码（去重键组成部分；冗余：来自 TaktEcDetail.EcParentMaterialCode）不能为空")]
    public string EcParentMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）
    /// </summary>
    [Required(ErrorMessage = "上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）不能为空")]
    public string EcParentMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 新采购类型（冗余：来自保留明细；落行均为 F）
    /// </summary>
    [Required(ErrorMessage = "新采购类型（冗余：来自保留明细；落行均为 F）不能为空")]
    public string EcNewPurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 新品仓库（冗余：来自保留明细；均为 C003）
    /// </summary>
    [Required(ErrorMessage = "新品仓库（冗余：来自保留明细；均为 C003）不能为空")]
    public string EcNewWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// 预定日期（冗余：来自 TaktEcPc.ScheduledDate）
    /// </summary>
    public DateTime? ScheduledDate { get; set; }

    /// <summary>
    /// 预定批次（冗余：来自 TaktEcPc.ScheduledBatch）
    /// </summary>
    public string? ScheduledBatch { get; set; } = string.Empty;

    /// <summary>
    /// 是否实施（0=否 1=是，字典 sys_yes_no）
    /// </summary>
    public int IsImplemented { get; set; } = 0;

    /// <summary>
    /// 出库日期
    /// </summary>
    public DateTime? OutboundDate { get; set; }

    /// <summary>
    /// 出库批次
    /// </summary>
    public string? OutboundBatch { get; set; } = string.Empty;

    /// <summary>
    /// 执行内容（各部门通用；ntext）
    /// </summary>
    public string? ExecContent { get; set; } = string.Empty;

    /// <summary>
    /// 部门编码（固定 D0625 物料）
    /// </summary>
    [Required(ErrorMessage = "部门编码（固定 D0625 物料）不能为空")]
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
    /// 控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品 2=按机种 3=按设变；跟随主表选中值）
    /// </summary>
    public int EcControlScope { get; set; } = 0;

    /// <summary>
    /// 执行行去重键（按控制范围：按成品/按机种/按设变；同一键的明细共用本行。ntext）
    /// </summary>
    [Required(ErrorMessage = "执行行去重键（按控制范围：按成品/按机种/按设变；同一键的明细共用本行。ntext）不能为空")]
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
// 更新EcMtl DTO
// ========================================

/// <summary>
/// 更新EcMtl DTO
/// 继承 TaktEcMtlCreateDto，添加 EcMtlId 字段
/// </summary>
public class TaktEcMtlUpdateDto : TaktEcMtlCreateDto
{
    /// <summary>
    /// EcMtlID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcMtlId { get; set; }

}

// ========================================
// EcMtl 停产状态 DTO
// ========================================

/// <summary>
/// EcMtl 停产状态更新 DTO
/// </summary>
public class TaktEcMtlDiscontinuedStatusDto
{
    /// <summary>
    /// EcMtlID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcMtlId { get; set; }

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
    /// 控制范围（字典 logistics_manufacturing_ec_control_scope_category）
    /// </summary>
    public int EcControlScope { get; set; } = 2;
}

// ========================================
// EcMtl 作废 DTO
// ========================================

/// <summary>
/// EcMtl 作废/撤销作废 DTO
/// </summary>
public class TaktEcMtlObsoleteDto
{
    /// <summary>
    /// EcMtlID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcMtlId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// EcMtl 导入模板行 DTO
/// </summary>
public class TaktEcMtlTemplateDto
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
    /// 设变单号（冗余：来自 TaktEcDetail.EcCode）
    /// </summary>
    public string? EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）
    /// </summary>
    public string? DiscontinuedStatus { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码组（同去重键下的机种编码，分号分隔；ntext）
    /// </summary>
    public string? EcModelCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 根物料编码组（同去重键下的根物料编码，分号分隔；ntext）
    /// </summary>
    public string? EcRootMaterialCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 新物料编码组（该上阶物料下采购类型 F、仓库 C003 的全部新物料编码，分号分隔；ntext）
    /// </summary>
    public string? EcNewMaterialCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料编码（去重键组成部分；冗余：来自 TaktEcDetail.EcParentMaterialCode）
    /// </summary>
    public string? EcParentMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）
    /// </summary>
    public string? EcParentMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 新采购类型（冗余：来自保留明细；落行均为 F）
    /// </summary>
    public string? EcNewPurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 新品仓库（冗余：来自保留明细；均为 C003）
    /// </summary>
    public string? EcNewWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// 预定日期（冗余：来自 TaktEcPc.ScheduledDate）
    /// </summary>
    public DateTime? ScheduledDate { get; set; }

    /// <summary>
    /// 预定批次（冗余：来自 TaktEcPc.ScheduledBatch）
    /// </summary>
    public string? ScheduledBatch { get; set; } = string.Empty;

    /// <summary>
    /// 是否实施（0=否 1=是，字典 sys_yes_no）
    /// </summary>
    public int? IsImplemented { get; set; }

    /// <summary>
    /// 出库日期
    /// </summary>
    public DateTime? OutboundDate { get; set; }

    /// <summary>
    /// 出库批次
    /// </summary>
    public string? OutboundBatch { get; set; } = string.Empty;

    /// <summary>
    /// 执行内容（各部门通用；ntext）
    /// </summary>
    public string? ExecContent { get; set; } = string.Empty;

    /// <summary>
    /// 部门编码（固定 D0625 物料）
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
    /// 控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品 2=按机种 3=按设变；跟随主表选中值）
    /// </summary>
    public int? EcControlScope { get; set; }

    /// <summary>
    /// 执行行去重键（按控制范围：按成品/按机种/按设变；同一键的明细共用本行。ntext）
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
/// EcMtl 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktEcMtlImportDto
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
    /// 设变单号（冗余：来自 TaktEcDetail.EcCode）
    /// </summary>
    public string? EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）
    /// </summary>
    public string? DiscontinuedStatus { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码组（同去重键下的机种编码，分号分隔；ntext）
    /// </summary>
    public string? EcModelCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 根物料编码组（同去重键下的根物料编码，分号分隔；ntext）
    /// </summary>
    public string? EcRootMaterialCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 新物料编码组（该上阶物料下采购类型 F、仓库 C003 的全部新物料编码，分号分隔；ntext）
    /// </summary>
    public string? EcNewMaterialCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料编码（去重键组成部分；冗余：来自 TaktEcDetail.EcParentMaterialCode）
    /// </summary>
    public string? EcParentMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）
    /// </summary>
    public string? EcParentMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 新采购类型（冗余：来自保留明细；落行均为 F）
    /// </summary>
    public string? EcNewPurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 新品仓库（冗余：来自保留明细；均为 C003）
    /// </summary>
    public string? EcNewWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// 预定日期（冗余：来自 TaktEcPc.ScheduledDate）
    /// </summary>
    public DateTime? ScheduledDate { get; set; }

    /// <summary>
    /// 预定批次（冗余：来自 TaktEcPc.ScheduledBatch）
    /// </summary>
    public string? ScheduledBatch { get; set; } = string.Empty;

    /// <summary>
    /// 是否实施（0=否 1=是，字典 sys_yes_no）
    /// </summary>
    public int? IsImplemented { get; set; }

    /// <summary>
    /// 出库日期
    /// </summary>
    public DateTime? OutboundDate { get; set; }

    /// <summary>
    /// 出库批次
    /// </summary>
    public string? OutboundBatch { get; set; } = string.Empty;

    /// <summary>
    /// 执行内容（各部门通用；ntext）
    /// </summary>
    public string? ExecContent { get; set; } = string.Empty;

    /// <summary>
    /// 部门编码（固定 D0625 物料）
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
    /// 控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品 2=按机种 3=按设变；跟随主表选中值）
    /// </summary>
    public int? EcControlScope { get; set; }

    /// <summary>
    /// 执行行去重键（按控制范围：按成品/按机种/按设变；同一键的明细共用本行。ntext）
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
/// EcMtl 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktEcMtlExportDto
{
    /// <summary>
    /// EcMtlID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcMtlId { get; set; }

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
    /// 设变单号（冗余：来自 TaktEcDetail.EcCode）
    /// </summary>
    public string EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）
    /// </summary>
    public string DiscontinuedStatus { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码组（同去重键下的机种编码，分号分隔；ntext）
    /// </summary>
    public string EcModelCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 根物料编码组（同去重键下的根物料编码，分号分隔；ntext）
    /// </summary>
    public string EcRootMaterialCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 新物料编码组（该上阶物料下采购类型 F、仓库 C003 的全部新物料编码，分号分隔；ntext）
    /// </summary>
    public string EcNewMaterialCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料编码（去重键组成部分；冗余：来自 TaktEcDetail.EcParentMaterialCode）
    /// </summary>
    public string EcParentMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）
    /// </summary>
    public string EcParentMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 新采购类型（冗余：来自保留明细；落行均为 F）
    /// </summary>
    public string EcNewPurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 新品仓库（冗余：来自保留明细；均为 C003）
    /// </summary>
    public string EcNewWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// 预定日期（冗余：来自 TaktEcPc.ScheduledDate）
    /// </summary>
    public DateTime? ScheduledDate { get; set; }

    /// <summary>
    /// 预定批次（冗余：来自 TaktEcPc.ScheduledBatch）
    /// </summary>
    public string? ScheduledBatch { get; set; } = string.Empty;

    /// <summary>
    /// 是否实施（0=否 1=是，字典 sys_yes_no）
    /// </summary>
    public int IsImplemented { get; set; } = 0;

    /// <summary>
    /// 出库日期
    /// </summary>
    public DateTime? OutboundDate { get; set; }

    /// <summary>
    /// 出库批次
    /// </summary>
    public string? OutboundBatch { get; set; } = string.Empty;

    /// <summary>
    /// 执行内容（各部门通用；ntext）
    /// </summary>
    public string? ExecContent { get; set; } = string.Empty;

    /// <summary>
    /// 部门编码（固定 D0625 物料）
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
    /// 控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品 2=按机种 3=按设变；跟随主表选中值）
    /// </summary>
    public int EcControlScope { get; set; } = 0;

    /// <summary>
    /// 执行行去重键（按控制范围：按成品/按机种/按设变；同一键的明细共用本行。ntext）
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
