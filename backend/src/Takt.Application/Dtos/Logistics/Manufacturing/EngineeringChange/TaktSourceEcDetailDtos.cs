// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktSourceEcDetailDtos.cs
// 创建时间：2026-08-26
// 创建人：Takt365(Auto Generated)
// 功能描述：SourceEcDetail 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSourceEcDetail 生成，请按需审阅）
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
// SourceEcDetail 响应 DTO
// ========================================

/// <summary>
/// 设变来源子表
/// 对应前端 TaktSourceEcDetailDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSourceEcDetailDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SourceEcDetailID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SourceEcDetailId { get; set; }

    /// <summary>
    /// 主ID（选项 TaktSourceEcs/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SourceEcId { get; set; }

    /// <summary>
    /// 主名称（填充字段）
    /// </summary>
    public string? SourceEcName { get; set; }

    /// <summary>
    /// 设变号码（冗余字段，便于查询）
    /// </summary>
    public string SourceEcCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 根物料编码
    /// </summary>
    public string SourceRootMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料编码
    /// </summary>
    public string SourceParentMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料编码
    /// </summary>
    public string? SourceOldMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料描述
    /// </summary>
    public string? SourceOldMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料用量
    /// </summary>
    public decimal? SourceOldUsageQuantity { get; set; }

    /// <summary>
    /// 旧物料安装位置
    /// </summary>
    public string? SourceOldItemPosition { get; set; } = string.Empty;

    /// <summary>
    /// 新物料编码
    /// </summary>
    public string? SourceNewMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 新物料描述
    /// </summary>
    public string? SourceNewMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 新物料用量
    /// </summary>
    public decimal? SourceNewUsageQuantity { get; set; }

    /// <summary>
    /// 新物料安装位置
    /// </summary>
    public string? SourceNewItemPosition { get; set; } = string.Empty;

    /// <summary>
    /// BOM番号
    /// </summary>
    public string? SourceBomCode { get; set; } = string.Empty;

    /// <summary>
    /// 兼容性（两位码第1位 A=有 B=→ C=← D=无；第2位 1～9=同时变更 *=无同时变更）
    /// </summary>
    public string? SourceCompatibility { get; set; } = string.Empty;

    /// <summary>
    /// 第二供应商（字典 logistics_manufacturing_ec_2nd_vendor；1=有 2=优先 3=无）
    /// </summary>
    public string? Source2ndVendor { get; set; } = string.Empty;

    /// <summary>
    /// 安排指示（字典 logistics_manufacturing_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）
    /// </summary>
    public string? SourceInstruction { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
    /// </summary>
    public string? SourceOldPartDisposition { get; set; } = string.Empty;

    /// <summary>
    /// BOM生效日期
    /// </summary>
    public DateTime? SourceBomEffectiveDate { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 设变来源主表
    /// （主表：TaktSourceEc）
    /// </summary>
    public TaktSourceEcDto? SourceEc { get; set; }

}

// ========================================
// SourceEcDetail 查询 DTO
// ========================================

/// <summary>
/// SourceEcDetail 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSourceEcDetailQueryDto : TaktPagedQuery
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
    /// 主ID（选项 TaktSourceEcs/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SourceEcId { get; set; }

    /// <summary>
    /// 设变号码（冗余字段，便于查询）
    /// </summary>
    public string? SourceEcCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 根物料编码
    /// </summary>
    public string? SourceRootMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料编码
    /// </summary>
    public string? SourceParentMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料编码
    /// </summary>
    public string? SourceOldMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料描述
    /// </summary>
    public string? SourceOldMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料用量
    /// </summary>
    public decimal? SourceOldUsageQuantity { get; set; }

    /// <summary>
    /// 旧物料安装位置
    /// </summary>
    public string? SourceOldItemPosition { get; set; } = string.Empty;

    /// <summary>
    /// 新物料编码
    /// </summary>
    public string? SourceNewMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 新物料描述
    /// </summary>
    public string? SourceNewMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 新物料用量
    /// </summary>
    public decimal? SourceNewUsageQuantity { get; set; }

    /// <summary>
    /// 新物料安装位置
    /// </summary>
    public string? SourceNewItemPosition { get; set; } = string.Empty;

    /// <summary>
    /// BOM番号
    /// </summary>
    public string? SourceBomCode { get; set; } = string.Empty;

    /// <summary>
    /// 兼容性（两位码第1位 A=有 B=→ C=← D=无；第2位 1～9=同时变更 *=无同时变更）
    /// </summary>
    public string? SourceCompatibility { get; set; } = string.Empty;

    /// <summary>
    /// 第二供应商（字典 logistics_manufacturing_ec_2nd_vendor；1=有 2=优先 3=无）
    /// </summary>
    public string? Source2ndVendor { get; set; } = string.Empty;

    /// <summary>
    /// 安排指示（字典 logistics_manufacturing_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）
    /// </summary>
    public string? SourceInstruction { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
    /// </summary>
    public string? SourceOldPartDisposition { get; set; } = string.Empty;

    /// <summary>
    /// BOM生效日期（范围查询-开始）
    /// </summary>
    public DateTime? SourceBomEffectiveDateStart { get; set; }

    /// <summary>
    /// BOM生效日期（范围查询-结束）
    /// </summary>
    public DateTime? SourceBomEffectiveDateEnd { get; set; }

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
// 创建SourceEcDetail DTO
// ========================================

/// <summary>
/// 创建SourceEcDetail DTO
/// </summary>
public class TaktSourceEcDetailCreateDto
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
    /// 主ID（选项 TaktSourceEcs/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SourceEcId { get; set; }

    /// <summary>
    /// 设变号码（冗余字段，便于查询）
    /// </summary>
    public string SourceEcCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 根物料编码
    /// </summary>
    [Required(ErrorMessage = "根物料编码不能为空")]
    public string SourceRootMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料编码
    /// </summary>
    [Required(ErrorMessage = "上阶物料编码不能为空")]
    public string SourceParentMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料编码
    /// </summary>
    public string? SourceOldMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料描述
    /// </summary>
    public string? SourceOldMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料用量
    /// </summary>
    public decimal? SourceOldUsageQuantity { get; set; }

    /// <summary>
    /// 旧物料安装位置
    /// </summary>
    public string? SourceOldItemPosition { get; set; } = string.Empty;

    /// <summary>
    /// 新物料编码
    /// </summary>
    public string? SourceNewMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 新物料描述
    /// </summary>
    public string? SourceNewMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 新物料用量
    /// </summary>
    public decimal? SourceNewUsageQuantity { get; set; }

    /// <summary>
    /// 新物料安装位置
    /// </summary>
    public string? SourceNewItemPosition { get; set; } = string.Empty;

    /// <summary>
    /// BOM番号
    /// </summary>
    public string? SourceBomCode { get; set; } = string.Empty;

    /// <summary>
    /// 兼容性（两位码第1位 A=有 B=→ C=← D=无；第2位 1～9=同时变更 *=无同时变更）
    /// </summary>
    public string? SourceCompatibility { get; set; } = string.Empty;

    /// <summary>
    /// 第二供应商（字典 logistics_manufacturing_ec_2nd_vendor；1=有 2=优先 3=无）
    /// </summary>
    public string? Source2ndVendor { get; set; } = string.Empty;

    /// <summary>
    /// 安排指示（字典 logistics_manufacturing_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）
    /// </summary>
    public string? SourceInstruction { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
    /// </summary>
    public string? SourceOldPartDisposition { get; set; } = string.Empty;

    /// <summary>
    /// BOM生效日期
    /// </summary>
    public DateTime? SourceBomEffectiveDate { get; set; }

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
// 更新SourceEcDetail DTO
// ========================================

/// <summary>
/// 更新SourceEcDetail DTO
/// 继承 TaktSourceEcDetailCreateDto，添加 SourceEcDetailId 字段
/// </summary>
public class TaktSourceEcDetailUpdateDto : TaktSourceEcDetailCreateDto
{
    /// <summary>
    /// SourceEcDetailID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SourceEcDetailId { get; set; }

}

// ========================================
// SourceEcDetail 作废 DTO
// ========================================

/// <summary>
/// SourceEcDetail 作废/撤销作废 DTO
/// </summary>
public class TaktSourceEcDetailObsoleteDto
{
    /// <summary>
    /// SourceEcDetailID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SourceEcDetailId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SourceEcDetail 导入模板行 DTO
/// </summary>
public class TaktSourceEcDetailTemplateDto
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
    /// 主ID（选项 TaktSourceEcs/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SourceEcId { get; set; }

    /// <summary>
    /// 设变号码（冗余字段，便于查询）
    /// </summary>
    public string? SourceEcCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 根物料编码
    /// </summary>
    public string? SourceRootMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料编码
    /// </summary>
    public string? SourceParentMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料编码
    /// </summary>
    public string? SourceOldMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料描述
    /// </summary>
    public string? SourceOldMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料用量
    /// </summary>
    public decimal? SourceOldUsageQuantity { get; set; }

    /// <summary>
    /// 旧物料安装位置
    /// </summary>
    public string? SourceOldItemPosition { get; set; } = string.Empty;

    /// <summary>
    /// 新物料编码
    /// </summary>
    public string? SourceNewMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 新物料描述
    /// </summary>
    public string? SourceNewMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 新物料用量
    /// </summary>
    public decimal? SourceNewUsageQuantity { get; set; }

    /// <summary>
    /// 新物料安装位置
    /// </summary>
    public string? SourceNewItemPosition { get; set; } = string.Empty;

    /// <summary>
    /// BOM番号
    /// </summary>
    public string? SourceBomCode { get; set; } = string.Empty;

    /// <summary>
    /// 兼容性（两位码第1位 A=有 B=→ C=← D=无；第2位 1～9=同时变更 *=无同时变更）
    /// </summary>
    public string? SourceCompatibility { get; set; } = string.Empty;

    /// <summary>
    /// 第二供应商（字典 logistics_manufacturing_ec_2nd_vendor；1=有 2=优先 3=无）
    /// </summary>
    public string? Source2ndVendor { get; set; } = string.Empty;

    /// <summary>
    /// 安排指示（字典 logistics_manufacturing_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）
    /// </summary>
    public string? SourceInstruction { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
    /// </summary>
    public string? SourceOldPartDisposition { get; set; } = string.Empty;

    /// <summary>
    /// BOM生效日期
    /// </summary>
    public DateTime? SourceBomEffectiveDate { get; set; }

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
/// SourceEcDetail 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSourceEcDetailImportDto
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
    /// 主ID（选项 TaktSourceEcs/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SourceEcId { get; set; }

    /// <summary>
    /// 设变号码（冗余字段，便于查询）
    /// </summary>
    public string? SourceEcCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 根物料编码
    /// </summary>
    public string? SourceRootMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料编码
    /// </summary>
    public string? SourceParentMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料编码
    /// </summary>
    public string? SourceOldMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料描述
    /// </summary>
    public string? SourceOldMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料用量
    /// </summary>
    public decimal? SourceOldUsageQuantity { get; set; }

    /// <summary>
    /// 旧物料安装位置
    /// </summary>
    public string? SourceOldItemPosition { get; set; } = string.Empty;

    /// <summary>
    /// 新物料编码
    /// </summary>
    public string? SourceNewMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 新物料描述
    /// </summary>
    public string? SourceNewMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 新物料用量
    /// </summary>
    public decimal? SourceNewUsageQuantity { get; set; }

    /// <summary>
    /// 新物料安装位置
    /// </summary>
    public string? SourceNewItemPosition { get; set; } = string.Empty;

    /// <summary>
    /// BOM番号
    /// </summary>
    public string? SourceBomCode { get; set; } = string.Empty;

    /// <summary>
    /// 兼容性（两位码第1位 A=有 B=→ C=← D=无；第2位 1～9=同时变更 *=无同时变更）
    /// </summary>
    public string? SourceCompatibility { get; set; } = string.Empty;

    /// <summary>
    /// 第二供应商（字典 logistics_manufacturing_ec_2nd_vendor；1=有 2=优先 3=无）
    /// </summary>
    public string? Source2ndVendor { get; set; } = string.Empty;

    /// <summary>
    /// 安排指示（字典 logistics_manufacturing_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）
    /// </summary>
    public string? SourceInstruction { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
    /// </summary>
    public string? SourceOldPartDisposition { get; set; } = string.Empty;

    /// <summary>
    /// BOM生效日期
    /// </summary>
    public DateTime? SourceBomEffectiveDate { get; set; }

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
/// SourceEcDetail 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSourceEcDetailExportDto
{
    /// <summary>
    /// SourceEcDetailID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SourceEcDetailId { get; set; }

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
    /// 主ID（选项 TaktSourceEcs/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SourceEcId { get; set; }

    /// <summary>
    /// 设变号码（冗余字段，便于查询）
    /// </summary>
    public string SourceEcCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 根物料编码
    /// </summary>
    public string SourceRootMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料编码
    /// </summary>
    public string SourceParentMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料编码
    /// </summary>
    public string? SourceOldMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料描述
    /// </summary>
    public string? SourceOldMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料用量
    /// </summary>
    public decimal? SourceOldUsageQuantity { get; set; }

    /// <summary>
    /// 旧物料安装位置
    /// </summary>
    public string? SourceOldItemPosition { get; set; } = string.Empty;

    /// <summary>
    /// 新物料编码
    /// </summary>
    public string? SourceNewMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 新物料描述
    /// </summary>
    public string? SourceNewMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 新物料用量
    /// </summary>
    public decimal? SourceNewUsageQuantity { get; set; }

    /// <summary>
    /// 新物料安装位置
    /// </summary>
    public string? SourceNewItemPosition { get; set; } = string.Empty;

    /// <summary>
    /// BOM番号
    /// </summary>
    public string? SourceBomCode { get; set; } = string.Empty;

    /// <summary>
    /// 兼容性（两位码第1位 A=有 B=→ C=← D=无；第2位 1～9=同时变更 *=无同时变更）
    /// </summary>
    public string? SourceCompatibility { get; set; } = string.Empty;

    /// <summary>
    /// 第二供应商（字典 logistics_manufacturing_ec_2nd_vendor；1=有 2=优先 3=无）
    /// </summary>
    public string? Source2ndVendor { get; set; } = string.Empty;

    /// <summary>
    /// 安排指示（字典 logistics_manufacturing_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）
    /// </summary>
    public string? SourceInstruction { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
    /// </summary>
    public string? SourceOldPartDisposition { get; set; } = string.Empty;

    /// <summary>
    /// BOM生效日期
    /// </summary>
    public DateTime? SourceBomEffectiveDate { get; set; }

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
