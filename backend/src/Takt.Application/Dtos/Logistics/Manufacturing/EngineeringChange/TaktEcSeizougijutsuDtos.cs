// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcSeizougijutsuDtos.cs
// 创建时间：2026-08-26
// 创建人：Takt365(Auto Generated)
// 功能描述：EcSeizougijutsu 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktEcSeizougijutsu 生成，请按需审阅）
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
// EcSeizougijutsu 响应 DTO
// ========================================

/// <summary>
/// 设变制造技术课（D0630）部门执行表
/// 对应前端 TaktEcSeizougijutsuDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktEcSeizougijutsuDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EcSeizougijutsuID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcSeizougijutsuId { get; set; }

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
    /// 部门编码（TaktDept.DeptCode，5 位，如 D0630）
    /// </summary>
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? DeptName { get; set; }

    /// <summary>
    /// 是否更新 SOP（0=否 1=是，字典 sys_yes_no）
    /// </summary>
    public int IsSopUpdated { get; set; } = 0;

    /// <summary>
    /// 设变EC担当（选项 TaktEcGroups/options?ecGroupCategory=1；DictValue=EcGroupCode）
    /// </summary>
    public string? TechLeader { get; set; } = string.Empty;

    /// <summary>
    /// SOP担当（选项 TaktEcGroups/options?ecGroupCategory=2；DictValue=EcGroupCode）
    /// </summary>
    public string? SopLeader { get; set; } = string.Empty;

    /// <summary>
    /// SOP日期
    /// </summary>
    public DateTime? SopDate { get; set; }


    /// <summary>
    /// 机种（冗余：来自 TaktEcDetail.EcModelCode）
    /// </summary>
    public string EcModelCode { get; set; } = string.Empty;
    /// <summary>
    /// 根物料编码组（同去重键下的根物料编码，分号分隔；ntext）
    /// </summary>
    public string? EcRootMaterialCodeGroup { get; set; } = string.Empty;
    /// <summary>
    /// 根物料描述（冗余：来自 TaktEcDetail.EcRootMaterialDescription）
    /// </summary>
    public string? EcRootMaterialDescription { get; set; } = string.Empty;
    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 设变明细列表（视图主从：执行表为主；一对多；业务键 EcCode + EcModelCode + EcRootMaterialCode）
    /// </summary>
    public List<TaktEcDetailDto>? EcDetails { get; set; }

}

// ========================================
// EcSeizougijutsu 查询 DTO
// ========================================

/// <summary>
/// EcSeizougijutsu 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktEcSeizougijutsuQueryDto : TaktPagedQuery
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
    /// 停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）
    /// </summary>
    public string? DiscontinuedStatus { get; set; }
    /// <summary>
    /// 实施范围（冗余：来自 TaktEcDetail.EcScope）
    /// </summary>
    public int? EcScope { get; set; }
    /// <summary>
    /// 部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）
    /// </summary>
    public int? EcDeptScope { get; set; }

    /// <summary>
    /// 部门编码（TaktDept.DeptCode，5 位，如 D0630）
    /// </summary>
    public string? DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? DeptName { get; set; }

    /// <summary>
    /// 是否实施（制技不落库；查询兼容 ITaktEcDeptExecEntity）
    /// </summary>
    public int? IsImplemented { get; set; }

    /// <summary>
    /// 执行内容（制技不落库；查询兼容 ITaktEcDeptExecEntity）
    /// </summary>
    public string? ExecContent { get; set; } = string.Empty;

    /// <summary>
    /// 是否更新 SOP（0=否 1=是，字典 sys_yes_no）
    /// </summary>
    public int? IsSopUpdated { get; set; }

    /// <summary>
    /// 设变EC担当（选项 TaktEcGroups/options?ecGroupCategory=1；DictValue=EcGroupCode）
    /// </summary>
    public string? TechLeader { get; set; } = string.Empty;

    /// <summary>
    /// SOP担当（选项 TaktEcGroups/options?ecGroupCategory=2；DictValue=EcGroupCode）
    /// </summary>
    public string? SopLeader { get; set; } = string.Empty;

    /// <summary>
    /// SOP日期（范围查询-开始）
    /// </summary>
    public DateTime? SopDateStart { get; set; }

    /// <summary>
    /// SOP日期（范围查询-结束）
    /// </summary>
    public DateTime? SopDateEnd { get; set; }

    /// <summary>
    /// 机种（冗余：来自 TaktEcDetail.EcModelCode）
    /// </summary>
    public string? EcModelCode { get; set; } = string.Empty;
    /// <summary>
    /// 根物料编码组（同去重键下的根物料编码，分号分隔；ntext）
    /// </summary>
    public string? EcRootMaterialCodeGroup { get; set; } = string.Empty;
    /// <summary>
    /// 根物料描述（冗余：来自 TaktEcDetail.EcRootMaterialDescription）
    /// </summary>
    public string? EcRootMaterialDescription { get; set; } = string.Empty;
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
// 创建EcSeizougijutsu DTO
// ========================================

/// <summary>
/// 创建EcSeizougijutsu DTO
/// </summary>
public class TaktEcSeizougijutsuCreateDto
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
    /// 部门编码（TaktDept.DeptCode，5 位，如 D0630）
    /// </summary>
    [Required(ErrorMessage = "部门编码（TaktDept.DeptCode，5 位，如 D0630）不能为空")]
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? DeptName { get; set; }

    /// <summary>
    /// 是否更新 SOP（0=否 1=是，字典 sys_yes_no）
    /// </summary>
    public int IsSopUpdated { get; set; } = 0;

    /// <summary>
    /// 设变EC担当（选项 TaktEcGroups/options?ecGroupCategory=1；DictValue=EcGroupCode）
    /// </summary>
    public string? TechLeader { get; set; } = string.Empty;

    /// <summary>
    /// SOP担当（选项 TaktEcGroups/options?ecGroupCategory=2；DictValue=EcGroupCode）
    /// </summary>
    public string? SopLeader { get; set; } = string.Empty;

    /// <summary>
    /// SOP日期
    /// </summary>
    public DateTime? SopDate { get; set; }


    /// <summary>
    /// 机种（冗余：来自 TaktEcDetail.EcModelCode）
    /// </summary>
    [Required(ErrorMessage = "机种（冗余：来自 TaktEcDetail.EcModelCode）不能为空")]
    public string EcModelCode { get; set; } = string.Empty;
    /// <summary>
    /// 根物料编码组（同去重键下的根物料编码，分号分隔；ntext）
    /// </summary>
    public string? EcRootMaterialCodeGroup { get; set; } = string.Empty;
    /// <summary>
    /// 根物料描述（冗余：来自 TaktEcDetail.EcRootMaterialDescription）
    /// </summary>
    public string? EcRootMaterialDescription { get; set; } = string.Empty;
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
// 更新EcSeizougijutsu DTO
// ========================================

/// <summary>
/// 更新EcSeizougijutsu DTO
/// 继承 TaktEcSeizougijutsuCreateDto，添加 EcSeizougijutsuId 字段
/// </summary>
public class TaktEcSeizougijutsuUpdateDto : TaktEcSeizougijutsuCreateDto
{
    /// <summary>
    /// EcSeizougijutsuID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcSeizougijutsuId { get; set; }

}

// ========================================
// EcSeizougijutsu 作废 DTO
// ========================================

/// <summary>
/// EcSeizougijutsu 停产状态 DTO
/// </summary>
public class TaktEcSeizougijutsuDiscontinuedStatusDto
{
    /// <summary>
    /// EcSeizougijutsuID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcSeizougijutsuId { get; set; }

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
/// EcSeizougijutsu 作废/撤销作废 DTO
/// </summary>
public class TaktEcSeizougijutsuObsoleteDto
{
    /// <summary>
    /// EcSeizougijutsuID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcSeizougijutsuId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// EcSeizougijutsu 导入模板行 DTO
/// </summary>
public class TaktEcSeizougijutsuTemplateDto
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
    /// 停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）
    /// </summary>
    public string? DiscontinuedStatus { get; set; }
    /// <summary>
    /// 实施范围（冗余：来自 TaktEcDetail.EcScope）
    /// </summary>
    public int? EcScope { get; set; }
    /// <summary>
    /// 部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）
    /// </summary>
    public int? EcDeptScope { get; set; }

    /// <summary>
    /// 部门编码（TaktDept.DeptCode，5 位，如 D0630）
    /// </summary>
    public string? DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? DeptName { get; set; }

    /// <summary>
    /// 是否更新 SOP（0=否 1=是，字典 sys_yes_no）
    /// </summary>
    public int? IsSopUpdated { get; set; }

    /// <summary>
    /// 设变EC担当（选项 TaktEcGroups/options?ecGroupCategory=1；DictValue=EcGroupCode）
    /// </summary>
    public string? TechLeader { get; set; } = string.Empty;

    /// <summary>
    /// SOP担当（选项 TaktEcGroups/options?ecGroupCategory=2；DictValue=EcGroupCode）
    /// </summary>
    public string? SopLeader { get; set; } = string.Empty;

    /// <summary>
    /// SOP日期
    /// </summary>
    public DateTime? SopDate { get; set; }

    /// <summary>
    /// 机种（冗余：来自 TaktEcDetail.EcModelCode）
    /// </summary>
    public string? EcModelCode { get; set; } = string.Empty;
    /// <summary>
    /// 根物料编码组（同去重键下的根物料编码，分号分隔；ntext）
    /// </summary>
    public string? EcRootMaterialCodeGroup { get; set; } = string.Empty;
    /// <summary>
    /// 根物料描述（冗余：来自 TaktEcDetail.EcRootMaterialDescription）
    /// </summary>
    public string? EcRootMaterialDescription { get; set; } = string.Empty;
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
/// EcSeizougijutsu 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktEcSeizougijutsuImportDto
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
    /// 停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）
    /// </summary>
    public string? DiscontinuedStatus { get; set; }
    /// <summary>
    /// 实施范围（冗余：来自 TaktEcDetail.EcScope）
    /// </summary>
    public int? EcScope { get; set; }
    /// <summary>
    /// 部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）
    /// </summary>
    public int? EcDeptScope { get; set; }

    /// <summary>
    /// 部门编码（TaktDept.DeptCode，5 位，如 D0630）
    /// </summary>
    public string? DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? DeptName { get; set; }

    /// <summary>
    /// 是否更新 SOP（0=否 1=是，字典 sys_yes_no）
    /// </summary>
    public int? IsSopUpdated { get; set; }

    /// <summary>
    /// 设变EC担当（选项 TaktEcGroups/options?ecGroupCategory=1；DictValue=EcGroupCode）
    /// </summary>
    public string? TechLeader { get; set; } = string.Empty;

    /// <summary>
    /// SOP担当（选项 TaktEcGroups/options?ecGroupCategory=2；DictValue=EcGroupCode）
    /// </summary>
    public string? SopLeader { get; set; } = string.Empty;

    /// <summary>
    /// SOP日期
    /// </summary>
    public DateTime? SopDate { get; set; }

    /// <summary>
    /// 机种（冗余：来自 TaktEcDetail.EcModelCode）
    /// </summary>
    public string? EcModelCode { get; set; } = string.Empty;
    /// <summary>
    /// 根物料编码组（同去重键下的根物料编码，分号分隔；ntext）
    /// </summary>
    public string? EcRootMaterialCodeGroup { get; set; } = string.Empty;
    /// <summary>
    /// 根物料描述（冗余：来自 TaktEcDetail.EcRootMaterialDescription）
    /// </summary>
    public string? EcRootMaterialDescription { get; set; } = string.Empty;
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
/// EcSeizougijutsu 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktEcSeizougijutsuExportDto
{
    /// <summary>
    /// EcSeizougijutsuID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcSeizougijutsuId { get; set; }

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
    /// 部门编码（TaktDept.DeptCode，5 位，如 D0630）
    /// </summary>
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? DeptName { get; set; }

    /// <summary>
    /// 是否更新 SOP（0=否 1=是，字典 sys_yes_no）
    /// </summary>
    public int IsSopUpdated { get; set; } = 0;

    /// <summary>
    /// 设变EC担当（选项 TaktEcGroups/options?ecGroupCategory=1；DictValue=EcGroupCode）
    /// </summary>
    public string? TechLeader { get; set; } = string.Empty;

    /// <summary>
    /// SOP担当（选项 TaktEcGroups/options?ecGroupCategory=2；DictValue=EcGroupCode）
    /// </summary>
    public string? SopLeader { get; set; } = string.Empty;

    /// <summary>
    /// SOP日期
    /// </summary>
    public DateTime? SopDate { get; set; }


    /// <summary>
    /// 机种（冗余：来自 TaktEcDetail.EcModelCode）
    /// </summary>
    public string EcModelCode { get; set; } = string.Empty;
    /// <summary>
    /// 根物料编码组（同去重键下的根物料编码，分号分隔；ntext）
    /// </summary>
    public string? EcRootMaterialCodeGroup { get; set; } = string.Empty;
    /// <summary>
    /// 根物料描述（冗余：来自 TaktEcDetail.EcRootMaterialDescription）
    /// </summary>
    public string? EcRootMaterialDescription { get; set; } = string.Empty;
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
