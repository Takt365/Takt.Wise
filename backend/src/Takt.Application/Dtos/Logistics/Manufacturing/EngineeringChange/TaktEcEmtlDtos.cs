// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcEmtlDtos.cs
// 创建时间：2026-09-02
// 创建人：Takt365(Auto Generated)
// 功能描述：EcEmtl 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktEcEmtl 生成，请按需审阅）
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
// EcEmtl 响应 DTO
// ========================================

/// <summary>
/// 设变SMT（D0430）部门执行表
/// 对应前端 TaktEcEmtlDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktEcEmtlDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EcEmtlID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcEmtlId { get; set; }

    /// <summary>
    /// 设变明细 ID（TaktEcDetail 主键；去重组内代表/种子明细 Id；同组多明细按业务键 FanOut）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcDetailId { get; set; }

    /// <summary>
    /// 设变明细 名称（填充字段）
    /// </summary>
    public string? EcDetailName { get; set; }

    /// <summary>
    /// 设变单号（冗余，便于查询）
    /// </summary>
    public string EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;
    /// <summary>
    /// 上阶物料编码（冗余：来自 TaktEcDetail.EcParentMaterialCode）
    /// </summary>
    public string EcParentMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）
    /// </summary>
    public string? EcParentMaterialDescription { get; set; }

    /// <summary>
    /// 停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）
    /// </summary>
    public string DiscontinuedStatus { get; set; } = string.Empty;
    /// <summary>
    /// 实施范围（冗余：来自 TaktEcDetail.EcScope）
    /// </summary>
    public int EcScope { get; set; }
    /// <summary>
    /// 部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）
    /// </summary>
    public int EcDeptScope { get; set; } = 2;

    /// <summary>
    /// 新物料编码（冗余：来自 TaktEcDetail.EcNewMaterialCode）
    /// </summary>
    public string? EcNewMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 新物料描述（冗余：来自 TaktEcDetail.EcNewMaterialDescription）
    /// </summary>
    public string? EcNewMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 新采购类型（F=外部采购，E=自制生产；冗余：来自 TaktEcDetail.EcNewPurchaseType）
    /// </summary>
    public string? EcNewPurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 新品仓库（选项 TaktWarehouses/options；DictValue=WarehouseCode；冗余：来自 TaktEcDetail.EcNewWarehouse；本表仅 C003）
    /// </summary>
    public string? EcNewWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// 部门编码（TaktDept.DeptCode；本表固定课别）
    /// </summary>
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 是否实施（0=否 1=是，字典 sys_yes_no）
    /// </summary>
    public int IsImplemented { get; set; } = 0;

    /// <summary>
    /// 预定日期（冗余：来自 TaktEcSeikan.ScheduledDate）
    /// </summary>
    public DateTime? ScheduledDate { get; set; }

    /// <summary>
    /// 预定批次（冗余：来自 TaktEcSeikan.ScheduledBatch）
    /// </summary>
    public string? ScheduledBatch { get; set; } = string.Empty;

    /// <summary>
    /// 执行内容（各部门通用）
    /// </summary>
    public string? ExecContent { get; set; } = string.Empty;

    /// <summary>
    /// 出库批次
    /// </summary>
    public string? OutboundBatch { get; set; } = string.Empty;

    /// <summary>
    /// 出库日期
    /// </summary>
    public DateTime? OutboundDate { get; set; }

    /// <summary>
    /// 机种编码组（单个或多个机种，分号分隔；ntext）
    /// </summary>
    public string? EcModelCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 根物料编码组（同设变单号+机种编码下可见明细的根物料编码，分号分隔；ntext）
    /// </summary>
    public string? EcRootMaterialCodeGroup { get; set; } = string.Empty;
    /// <summary>
    /// 根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）
    /// </summary>
    public string EcRootMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 设变明细列表（视图主从：执行表为主；一对多；机种以编码组为准）
    /// </summary>
    public List<TaktEcDetailDto>? EcDetails { get; set; }

}

// ========================================
// EcEmtl 查询 DTO
// ========================================

/// <summary>
/// EcEmtl 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktEcEmtlQueryDto : TaktPagedQuery
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
    /// 设变明细 ID（TaktEcDetail 主键；去重组内代表/种子明细 Id；同组多明细按业务键 FanOut）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcDetailId { get; set; }

    /// <summary>
    /// 设变单号（冗余，便于查询）
    /// </summary>
    public string? EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 机种编码组（单个或多个机种，分号分隔；ntext）
    /// </summary>
    public string? EcModelCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 根物料编码组（同设变单号+机种编码下可见明细的根物料编码，分号分隔；ntext）
    /// </summary>
    public string? EcRootMaterialCodeGroup { get; set; } = string.Empty;
    /// <summary>
    /// 根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）
    /// </summary>
    public string EcRootMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料编码（冗余：来自 TaktEcDetail.EcParentMaterialCode）
    /// </summary>
    public string? EcParentMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）
    /// </summary>
    public string? EcParentMaterialDescription { get; set; }

    /// <summary>
    /// 停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）
    /// </summary>
    public string? DiscontinuedStatus { get; set; } = string.Empty;

    /// <summary>
    /// 新物料编码（冗余：来自 TaktEcDetail.EcNewMaterialCode）
    /// </summary>
    public string? EcNewMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 新物料描述（冗余：来自 TaktEcDetail.EcNewMaterialDescription）
    /// </summary>
    public string? EcNewMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 新采购类型（F=外部采购，E=自制生产；冗余：来自 TaktEcDetail.EcNewPurchaseType）
    /// </summary>
    public string? EcNewPurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 新品仓库（选项 TaktWarehouses/options；DictValue=WarehouseCode；冗余：来自 TaktEcDetail.EcNewWarehouse；本表仅 C003）
    /// </summary>
    public string? EcNewWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// 部门编码（TaktDept.DeptCode；本表固定课别）
    /// </summary>
    public string? DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 是否实施（0=否 1=是，字典 sys_yes_no）
    /// </summary>
    public int? IsImplemented { get; set; }

    /// <summary>
    /// 执行内容（各部门通用）
    /// </summary>
    public string? ExecContent { get; set; } = string.Empty;

    /// <summary>
    /// 出库批次
    /// </summary>
    public string? OutboundBatch { get; set; } = string.Empty;

    /// <summary>
    /// 出库日期（范围查询-开始）
    /// </summary>
    public DateTime? OutboundDateStart { get; set; }

    /// <summary>
    /// 出库日期（范围查询-结束）
    /// </summary>
    public DateTime? OutboundDateEnd { get; set; }

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
// 创建EcEmtl DTO
// ========================================

/// <summary>
/// 创建EcEmtl DTO
/// </summary>
public class TaktEcEmtlCreateDto
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
    /// 设变明细 ID（TaktEcDetail 主键；去重组内代表/种子明细 Id；同组多明细按业务键 FanOut）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcDetailId { get; set; }

    /// <summary>
    /// 设变单号（冗余，便于查询）
    /// </summary>
    [Required(ErrorMessage = "设变单号（冗余，便于查询）不能为空")]
    public string EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;
    /// <summary>
    /// 上阶物料编码（冗余：来自 TaktEcDetail.EcParentMaterialCode）
    /// </summary>
    public string EcParentMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）
    /// </summary>
    public string? EcParentMaterialDescription { get; set; }

    /// <summary>
    /// 停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）
    /// </summary>
    [Required(ErrorMessage = "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）不能为空")]
    public string DiscontinuedStatus { get; set; } = string.Empty;
    /// <summary>
    /// 实施范围（冗余：来自 TaktEcDetail.EcScope）
    /// </summary>
    public int EcScope { get; set; }
    /// <summary>
    /// 部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）
    /// </summary>
    public int EcDeptScope { get; set; } = 2;

    /// <summary>
    /// 新物料编码（冗余：来自 TaktEcDetail.EcNewMaterialCode）
    /// </summary>
    public string? EcNewMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 新物料描述（冗余：来自 TaktEcDetail.EcNewMaterialDescription）
    /// </summary>
    public string? EcNewMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 新采购类型（F=外部采购，E=自制生产；冗余：来自 TaktEcDetail.EcNewPurchaseType）
    /// </summary>
    public string? EcNewPurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 新品仓库（选项 TaktWarehouses/options；DictValue=WarehouseCode；冗余：来自 TaktEcDetail.EcNewWarehouse；本表仅 C003）
    /// </summary>
    public string? EcNewWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// 部门编码（TaktDept.DeptCode；本表固定课别）
    /// </summary>
    [Required(ErrorMessage = "部门编码（TaktDept.DeptCode；本表固定课别）不能为空")]
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 是否实施（0=否 1=是，字典 sys_yes_no）
    /// </summary>
    public int IsImplemented { get; set; } = 0;

    /// <summary>
    /// 预定日期（冗余：来自 TaktEcSeikan.ScheduledDate）
    /// </summary>
    public DateTime? ScheduledDate { get; set; }

    /// <summary>
    /// 预定批次（冗余：来自 TaktEcSeikan.ScheduledBatch）
    /// </summary>
    public string? ScheduledBatch { get; set; } = string.Empty;

    /// <summary>
    /// 执行内容（各部门通用）
    /// </summary>
    public string? ExecContent { get; set; } = string.Empty;

    /// <summary>
    /// 出库批次
    /// </summary>
    public string? OutboundBatch { get; set; } = string.Empty;

    /// <summary>
    /// 出库日期
    /// </summary>
    public DateTime? OutboundDate { get; set; }

    /// <summary>
    /// 机种编码组（单个或多个机种，分号分隔；ntext）
    /// </summary>
    public string? EcModelCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 根物料编码组（同设变单号+机种编码下可见明细的根物料编码，分号分隔；ntext）
    /// </summary>
    public string? EcRootMaterialCodeGroup { get; set; } = string.Empty;
    /// <summary>
    /// 根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）
    /// </summary>
    public string EcRootMaterialDescription { get; set; } = string.Empty;

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
// 更新EcEmtl DTO
// ========================================

/// <summary>
/// 更新EcEmtl DTO
/// 继承 TaktEcEmtlCreateDto，添加 EcEmtlId 字段
/// </summary>
public class TaktEcEmtlUpdateDto : TaktEcEmtlCreateDto
{
    /// <summary>
    /// EcEmtlID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcEmtlId { get; set; }

}

// ========================================
// EcEmtl 状态 DTO
// ========================================

/// <summary>
/// EcEmtl 状态更新 DTO
/// </summary>
public class TaktEcEmtlStatusDto
{
    /// <summary>
    /// EcEmtlID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcEmtlId { get; set; }

    /// <summary>
    /// 停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）
    /// </summary>
    [Required(ErrorMessage = "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）不能为空")]
    public string DiscontinuedStatus { get; set; } = string.Empty;
    /// <summary>
    /// 实施范围（冗余：来自 TaktEcDetail.EcScope）
    /// </summary>
    public int EcScope { get; set; }
    /// <summary>
    /// 部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）
    /// </summary>
    public int EcDeptScope { get; set; } = 2;
}

// ========================================
// EcEmtl 作废 DTO
// ========================================

/// <summary>
/// EcEmtl 停产状态 DTO
/// </summary>
public class TaktEcEmtlDiscontinuedStatusDto
{
    /// <summary>
    /// EcEmtlID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcEmtlId { get; set; }

    /// <summary>
    /// 根物料停产状态（字典 logistics_materials_material_discontinued_status；Z0=在产；停产按钮默认 ZQ）
    /// </summary>
    [Required(ErrorMessage = "停产状态不能为空")]
    public string DiscontinuedStatus { get; set; } = "Z0";
    /// <summary>
    /// 实施范围（冗余：来自 TaktEcDetail.EcScope）
    /// </summary>
    public int EcScope { get; set; }
    /// <summary>
    /// 部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）
    /// </summary>
    public int EcDeptScope { get; set; } = 2;
}

/// <summary>
/// EcEmtl 作废/撤销作废 DTO
/// </summary>
public class TaktEcEmtlObsoleteDto
{
    /// <summary>
    /// EcEmtlID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcEmtlId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// EcEmtl 导入模板行 DTO
/// </summary>
public class TaktEcEmtlTemplateDto
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
    /// 设变明细 ID（TaktEcDetail 主键；去重组内代表/种子明细 Id；同组多明细按业务键 FanOut）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcDetailId { get; set; }

    /// <summary>
    /// 设变单号（冗余，便于查询）
    /// </summary>
    public string? EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 机种编码组（单个或多个机种，分号分隔；ntext）
    /// </summary>
    public string? EcModelCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 根物料编码组（同设变单号+机种编码下可见明细的根物料编码，分号分隔；ntext）
    /// </summary>
    public string? EcRootMaterialCodeGroup { get; set; } = string.Empty;
    /// <summary>
    /// 根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）
    /// </summary>
    public string EcRootMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料编码（冗余：来自 TaktEcDetail.EcParentMaterialCode）
    /// </summary>
    public string? EcParentMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）
    /// </summary>
    public string? EcParentMaterialDescription { get; set; }

    /// <summary>
    /// 停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）
    /// </summary>
    public string? DiscontinuedStatus { get; set; } = string.Empty;

    /// <summary>
    /// 新物料编码（冗余：来自 TaktEcDetail.EcNewMaterialCode）
    /// </summary>
    public string? EcNewMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 新物料描述（冗余：来自 TaktEcDetail.EcNewMaterialDescription）
    /// </summary>
    public string? EcNewMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 新采购类型（F=外部采购，E=自制生产；冗余：来自 TaktEcDetail.EcNewPurchaseType）
    /// </summary>
    public string? EcNewPurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 新品仓库（选项 TaktWarehouses/options；DictValue=WarehouseCode；冗余：来自 TaktEcDetail.EcNewWarehouse；本表仅 C003）
    /// </summary>
    public string? EcNewWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// 部门编码（TaktDept.DeptCode；本表固定课别）
    /// </summary>
    public string? DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 是否实施（0=否 1=是，字典 sys_yes_no）
    /// </summary>
    public int? IsImplemented { get; set; }

    /// <summary>
    /// 执行内容（各部门通用）
    /// </summary>
    public string? ExecContent { get; set; } = string.Empty;

    /// <summary>
    /// 出库批次
    /// </summary>
    public string? OutboundBatch { get; set; } = string.Empty;

    /// <summary>
    /// 出库日期
    /// </summary>
    public DateTime? OutboundDate { get; set; }

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
/// EcEmtl 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktEcEmtlImportDto
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
    /// 设变明细 ID（TaktEcDetail 主键；去重组内代表/种子明细 Id；同组多明细按业务键 FanOut）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcDetailId { get; set; }

    /// <summary>
    /// 设变单号（冗余，便于查询）
    /// </summary>
    public string? EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 机种编码组（单个或多个机种，分号分隔；ntext）
    /// </summary>
    public string? EcModelCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 根物料编码组（同设变单号+机种编码下可见明细的根物料编码，分号分隔；ntext）
    /// </summary>
    public string? EcRootMaterialCodeGroup { get; set; } = string.Empty;
    /// <summary>
    /// 根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）
    /// </summary>
    public string EcRootMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料编码（冗余：来自 TaktEcDetail.EcParentMaterialCode）
    /// </summary>
    public string? EcParentMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）
    /// </summary>
    public string? EcParentMaterialDescription { get; set; }

    /// <summary>
    /// 停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）
    /// </summary>
    public string? DiscontinuedStatus { get; set; } = string.Empty;

    /// <summary>
    /// 新物料编码（冗余：来自 TaktEcDetail.EcNewMaterialCode）
    /// </summary>
    public string? EcNewMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 新物料描述（冗余：来自 TaktEcDetail.EcNewMaterialDescription）
    /// </summary>
    public string? EcNewMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 新采购类型（F=外部采购，E=自制生产；冗余：来自 TaktEcDetail.EcNewPurchaseType）
    /// </summary>
    public string? EcNewPurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 新品仓库（选项 TaktWarehouses/options；DictValue=WarehouseCode；冗余：来自 TaktEcDetail.EcNewWarehouse；本表仅 C003）
    /// </summary>
    public string? EcNewWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// 部门编码（TaktDept.DeptCode；本表固定课别）
    /// </summary>
    public string? DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 是否实施（0=否 1=是，字典 sys_yes_no）
    /// </summary>
    public int? IsImplemented { get; set; }

    /// <summary>
    /// 执行内容（各部门通用）
    /// </summary>
    public string? ExecContent { get; set; } = string.Empty;

    /// <summary>
    /// 出库批次
    /// </summary>
    public string? OutboundBatch { get; set; } = string.Empty;

    /// <summary>
    /// 出库日期
    /// </summary>
    public DateTime? OutboundDate { get; set; }

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
/// EcEmtl 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktEcEmtlExportDto
{
    /// <summary>
    /// EcEmtlID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcEmtlId { get; set; }

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
    /// 设变明细 ID（TaktEcDetail 主键；去重组内代表/种子明细 Id；同组多明细按业务键 FanOut）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcDetailId { get; set; }

    /// <summary>
    /// 设变单号（冗余，便于查询）
    /// </summary>
    public string EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;
    /// <summary>
    /// 上阶物料编码（冗余：来自 TaktEcDetail.EcParentMaterialCode）
    /// </summary>
    public string EcParentMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）
    /// </summary>
    public string? EcParentMaterialDescription { get; set; }

    /// <summary>
    /// 停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）
    /// </summary>
    public string DiscontinuedStatus { get; set; } = string.Empty;
    /// <summary>
    /// 实施范围（冗余：来自 TaktEcDetail.EcScope）
    /// </summary>
    public int EcScope { get; set; }
    /// <summary>
    /// 部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）
    /// </summary>
    public int EcDeptScope { get; set; } = 2;

    /// <summary>
    /// 新物料编码（冗余：来自 TaktEcDetail.EcNewMaterialCode）
    /// </summary>
    public string? EcNewMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 新物料描述（冗余：来自 TaktEcDetail.EcNewMaterialDescription）
    /// </summary>
    public string? EcNewMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 新采购类型（F=外部采购，E=自制生产；冗余：来自 TaktEcDetail.EcNewPurchaseType）
    /// </summary>
    public string? EcNewPurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 新品仓库（选项 TaktWarehouses/options；DictValue=WarehouseCode；冗余：来自 TaktEcDetail.EcNewWarehouse；本表仅 C003）
    /// </summary>
    public string? EcNewWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// 部门编码（TaktDept.DeptCode；本表固定课别）
    /// </summary>
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 是否实施（0=否 1=是，字典 sys_yes_no）
    /// </summary>
    public int IsImplemented { get; set; } = 0;

    /// <summary>
    /// 预定日期（冗余：来自 TaktEcSeikan.ScheduledDate）
    /// </summary>
    public DateTime? ScheduledDate { get; set; }

    /// <summary>
    /// 预定批次（冗余：来自 TaktEcSeikan.ScheduledBatch）
    /// </summary>
    public string? ScheduledBatch { get; set; } = string.Empty;

    /// <summary>
    /// 执行内容（各部门通用）
    /// </summary>
    public string? ExecContent { get; set; } = string.Empty;

    /// <summary>
    /// 出库批次
    /// </summary>
    public string? OutboundBatch { get; set; } = string.Empty;

    /// <summary>
    /// 出库日期
    /// </summary>
    public DateTime? OutboundDate { get; set; }

    /// <summary>
    /// 机种编码组（单个或多个机种，分号分隔；ntext）
    /// </summary>
    public string? EcModelCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 根物料编码组（同设变单号+机种编码下可见明细的根物料编码，分号分隔；ntext）
    /// </summary>
    public string? EcRootMaterialCodeGroup { get; set; } = string.Empty;
    /// <summary>
    /// 根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）
    /// </summary>
    public string EcRootMaterialDescription { get; set; } = string.Empty;

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
