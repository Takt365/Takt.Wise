// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Accounting.Financial
// 文件名称：TaktAssetDtos.cs
// 创建时间：2026-08-30
// 创建人：Takt365(Auto Generated)
// 功能描述：Asset 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktAsset 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Accounting.Financial;

// ========================================
// Asset 响应 DTO
// ========================================

/// <summary>
/// 资产实体
/// 对应前端 TaktAssetDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktAssetDto : TaktCompanyDtoBase
{
    /// <summary>
    /// AssetID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssetId { get; set; }

    /// <summary>
    /// 资产代码
    /// </summary>
    public string AssetCode { get; set; } = string.Empty;

    /// <summary>
    /// 资产名称
    /// </summary>
    public string AssetName { get; set; } = string.Empty;

    /// <summary>
    /// 资产分类（字典 accounting_financial_asset_category）
    /// </summary>
    public string AssetCategory { get; set; } = string.Empty;

    /// <summary>
    /// 资产类型（字典 accounting_financial_asset_type；NORM=普通资产）
    /// </summary>
    public string AssetType { get; set; } = string.Empty;

    /// <summary>
    /// 资产原值
    /// </summary>
    public decimal AssetOriginalValue { get; set; }

    /// <summary>
    /// 资产净值
    /// </summary>
    public decimal AssetNetValue { get; set; }

    /// <summary>
    /// 累计折旧
    /// </summary>
    public decimal AccumulatedDepreciation { get; set; }

    /// <summary>
    /// 成本中心（选项 TaktCostCenters/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CostCenterId { get; set; }

    /// <summary>
    /// 成本中心名称（冗余：按 CostCenterId 取 TaktCostCenter.CostCenterName 联动）
    /// </summary>
    public string? CostCenterName { get; set; } = string.Empty;

    /// <summary>
    /// 部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 使用者（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UserId { get; set; }

    /// <summary>
    /// 使用者名称（冗余：按 UserId 取 TaktUser.UserName 联动）
    /// </summary>
    public string? UserName { get; set; } = string.Empty;

    /// <summary>
    /// 资产位置
    /// </summary>
    public string? AssetLocation { get; set; } = string.Empty;

    /// <summary>
    /// 购买日期
    /// </summary>
    public DateTime? PurchaseDate { get; set; }

    /// <summary>
    /// 启用日期
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// 报废日期
    /// </summary>
    public DateTime? ScrapDate { get; set; }

    /// <summary>
    /// 处置日期
    /// </summary>
    public DateTime? DisposalDate { get; set; }

    /// <summary>
    /// 预计使用月数
    /// </summary>
    public int ExpectedLifeMonths { get; set; } = 0;

    /// <summary>
    /// 折旧方法（字典 accounting_financial_depreciation_method：0=不自动计提，1=直线法，2=双倍余额递减，3=年数总和，4=产量法，5=手工，6=剩余年限直线）
    /// </summary>
    public int DepreciationMethod { get; set; } = 0;

    /// <summary>
    /// 每月折旧金额
    /// </summary>
    public decimal MonthlyDepreciation { get; set; }

    /// <summary>
    /// 资产状态（字典 accounting_financial_asset_status：0=未使用，1=使用中，2=报废，3=处置，4=实物不存在）
    /// </summary>
    public int AssetStatus { get; set; } = 0;

}

// ========================================
// Asset 查询 DTO
// ========================================

/// <summary>
/// Asset 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktAssetQueryDto : TaktPagedQuery
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
    /// 资产代码
    /// </summary>
    public string? AssetCode { get; set; } = string.Empty;

    /// <summary>
    /// 资产名称
    /// </summary>
    public string? AssetName { get; set; } = string.Empty;

    /// <summary>
    /// 资产分类（字典 accounting_financial_asset_category）
    /// </summary>
    public string? AssetCategory { get; set; } = string.Empty;

    /// <summary>
    /// 资产类型（字典 accounting_financial_asset_type；NORM=普通资产）
    /// </summary>
    public string? AssetType { get; set; } = string.Empty;

    /// <summary>
    /// 资产原值
    /// </summary>
    public decimal? AssetOriginalValue { get; set; }

    /// <summary>
    /// 资产净值
    /// </summary>
    public decimal? AssetNetValue { get; set; }

    /// <summary>
    /// 累计折旧
    /// </summary>
    public decimal? AccumulatedDepreciation { get; set; }

    /// <summary>
    /// 成本中心（选项 TaktCostCenters/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CostCenterId { get; set; }

    /// <summary>
    /// 成本中心名称（冗余：按 CostCenterId 取 TaktCostCenter.CostCenterName 联动）
    /// </summary>
    public string? CostCenterName { get; set; } = string.Empty;

    /// <summary>
    /// 部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 使用者（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UserId { get; set; }

    /// <summary>
    /// 使用者名称（冗余：按 UserId 取 TaktUser.UserName 联动）
    /// </summary>
    public string? UserName { get; set; } = string.Empty;

    /// <summary>
    /// 资产位置
    /// </summary>
    public string? AssetLocation { get; set; } = string.Empty;

    /// <summary>
    /// 购买日期（范围查询-开始）
    /// </summary>
    public DateTime? PurchaseDateStart { get; set; }

    /// <summary>
    /// 购买日期（范围查询-结束）
    /// </summary>
    public DateTime? PurchaseDateEnd { get; set; }

    /// <summary>
    /// 启用日期（范围查询-开始）
    /// </summary>
    public DateTime? StartDateStart { get; set; }

    /// <summary>
    /// 启用日期（范围查询-结束）
    /// </summary>
    public DateTime? StartDateEnd { get; set; }

    /// <summary>
    /// 报废日期（范围查询-开始）
    /// </summary>
    public DateTime? ScrapDateStart { get; set; }

    /// <summary>
    /// 报废日期（范围查询-结束）
    /// </summary>
    public DateTime? ScrapDateEnd { get; set; }

    /// <summary>
    /// 处置日期（范围查询-开始）
    /// </summary>
    public DateTime? DisposalDateStart { get; set; }

    /// <summary>
    /// 处置日期（范围查询-结束）
    /// </summary>
    public DateTime? DisposalDateEnd { get; set; }

    /// <summary>
    /// 预计使用月数
    /// </summary>
    public int? ExpectedLifeMonths { get; set; }

    /// <summary>
    /// 折旧方法（字典 accounting_financial_depreciation_method：0=不自动计提，1=直线法，2=双倍余额递减，3=年数总和，4=产量法，5=手工，6=剩余年限直线）
    /// </summary>
    public int? DepreciationMethod { get; set; }

    /// <summary>
    /// 每月折旧金额
    /// </summary>
    public decimal? MonthlyDepreciation { get; set; }

    /// <summary>
    /// 资产状态（字典 accounting_financial_asset_status：0=未使用，1=使用中，2=报废，3=处置，4=实物不存在）
    /// </summary>
    public int? AssetStatus { get; set; }

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
// 创建Asset DTO
// ========================================

/// <summary>
/// 创建Asset DTO
/// </summary>
public class TaktAssetCreateDto
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
    /// 资产代码
    /// </summary>
    [Required(ErrorMessage = "资产代码不能为空")]
    public string AssetCode { get; set; } = string.Empty;

    /// <summary>
    /// 资产名称
    /// </summary>
    [Required(ErrorMessage = "资产名称不能为空")]
    public string AssetName { get; set; } = string.Empty;

    /// <summary>
    /// 资产分类（字典 accounting_financial_asset_category）
    /// </summary>
    [Required(ErrorMessage = "资产分类（字典 accounting_financial_asset_category）不能为空")]
    public string AssetCategory { get; set; } = string.Empty;

    /// <summary>
    /// 资产类型（字典 accounting_financial_asset_type；NORM=普通资产）
    /// </summary>
    [Required(ErrorMessage = "资产类型（字典 accounting_financial_asset_type；NORM=普通资产）不能为空")]
    public string AssetType { get; set; } = string.Empty;

    /// <summary>
    /// 资产原值
    /// </summary>
    public decimal AssetOriginalValue { get; set; }

    /// <summary>
    /// 资产净值
    /// </summary>
    public decimal AssetNetValue { get; set; }

    /// <summary>
    /// 累计折旧
    /// </summary>
    public decimal AccumulatedDepreciation { get; set; }

    /// <summary>
    /// 成本中心（选项 TaktCostCenters/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CostCenterId { get; set; }

    /// <summary>
    /// 成本中心名称（冗余：按 CostCenterId 取 TaktCostCenter.CostCenterName 联动）
    /// </summary>
    public string? CostCenterName { get; set; } = string.Empty;

    /// <summary>
    /// 部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 使用者（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UserId { get; set; }

    /// <summary>
    /// 使用者名称（冗余：按 UserId 取 TaktUser.UserName 联动）
    /// </summary>
    public string? UserName { get; set; } = string.Empty;

    /// <summary>
    /// 资产位置
    /// </summary>
    public string? AssetLocation { get; set; } = string.Empty;

    /// <summary>
    /// 购买日期
    /// </summary>
    public DateTime? PurchaseDate { get; set; }

    /// <summary>
    /// 启用日期
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// 报废日期
    /// </summary>
    public DateTime? ScrapDate { get; set; }

    /// <summary>
    /// 处置日期
    /// </summary>
    public DateTime? DisposalDate { get; set; }

    /// <summary>
    /// 预计使用月数
    /// </summary>
    public int ExpectedLifeMonths { get; set; } = 0;

    /// <summary>
    /// 折旧方法（字典 accounting_financial_depreciation_method：0=不自动计提，1=直线法，2=双倍余额递减，3=年数总和，4=产量法，5=手工，6=剩余年限直线）
    /// </summary>
    public int DepreciationMethod { get; set; } = 0;

    /// <summary>
    /// 每月折旧金额
    /// </summary>
    public decimal MonthlyDepreciation { get; set; }

    /// <summary>
    /// 资产状态（字典 accounting_financial_asset_status：0=未使用，1=使用中，2=报废，3=处置，4=实物不存在）
    /// </summary>
    public int AssetStatus { get; set; } = 0;

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
// 更新Asset DTO
// ========================================

/// <summary>
/// 更新Asset DTO
/// 继承 TaktAssetCreateDto，添加 AssetId 字段
/// </summary>
public class TaktAssetUpdateDto : TaktAssetCreateDto
{
    /// <summary>
    /// AssetID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssetId { get; set; }

}

// ========================================
// Asset 状态 DTO
// ========================================

/// <summary>
/// Asset 状态更新 DTO
/// </summary>
public class TaktAssetStatusDto
{
    /// <summary>
    /// AssetID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssetId { get; set; }

    /// <summary>
    /// 资产状态（字典 accounting_financial_asset_status：0=未使用，1=使用中，2=报废，3=处置，4=实物不存在）
    /// </summary>
    [Required(ErrorMessage = "资产状态（字典 accounting_financial_asset_status：0=未使用，1=使用中，2=报废，3=处置，4=实物不存在）不能为空")]
    public int AssetStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Asset 导入模板行 DTO
/// </summary>
public class TaktAssetTemplateDto
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
    /// 资产代码
    /// </summary>
    public string? AssetCode { get; set; } = string.Empty;

    /// <summary>
    /// 资产名称
    /// </summary>
    public string? AssetName { get; set; } = string.Empty;

    /// <summary>
    /// 资产分类（字典 accounting_financial_asset_category）
    /// </summary>
    public string? AssetCategory { get; set; } = string.Empty;

    /// <summary>
    /// 资产类型（字典 accounting_financial_asset_type；NORM=普通资产）
    /// </summary>
    public string? AssetType { get; set; } = string.Empty;

    /// <summary>
    /// 资产原值
    /// </summary>
    public decimal? AssetOriginalValue { get; set; }

    /// <summary>
    /// 资产净值
    /// </summary>
    public decimal? AssetNetValue { get; set; }

    /// <summary>
    /// 累计折旧
    /// </summary>
    public decimal? AccumulatedDepreciation { get; set; }

    /// <summary>
    /// 成本中心（选项 TaktCostCenters/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CostCenterId { get; set; }

    /// <summary>
    /// 成本中心名称（冗余：按 CostCenterId 取 TaktCostCenter.CostCenterName 联动）
    /// </summary>
    public string? CostCenterName { get; set; } = string.Empty;

    /// <summary>
    /// 部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 使用者（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UserId { get; set; }

    /// <summary>
    /// 使用者名称（冗余：按 UserId 取 TaktUser.UserName 联动）
    /// </summary>
    public string? UserName { get; set; } = string.Empty;

    /// <summary>
    /// 资产位置
    /// </summary>
    public string? AssetLocation { get; set; } = string.Empty;

    /// <summary>
    /// 购买日期
    /// </summary>
    public DateTime? PurchaseDate { get; set; }

    /// <summary>
    /// 启用日期
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// 报废日期
    /// </summary>
    public DateTime? ScrapDate { get; set; }

    /// <summary>
    /// 处置日期
    /// </summary>
    public DateTime? DisposalDate { get; set; }

    /// <summary>
    /// 预计使用月数
    /// </summary>
    public int? ExpectedLifeMonths { get; set; }

    /// <summary>
    /// 折旧方法（字典 accounting_financial_depreciation_method：0=不自动计提，1=直线法，2=双倍余额递减，3=年数总和，4=产量法，5=手工，6=剩余年限直线）
    /// </summary>
    public int? DepreciationMethod { get; set; }

    /// <summary>
    /// 每月折旧金额
    /// </summary>
    public decimal? MonthlyDepreciation { get; set; }

    /// <summary>
    /// 资产状态（字典 accounting_financial_asset_status：0=未使用，1=使用中，2=报废，3=处置，4=实物不存在）
    /// </summary>
    public int? AssetStatus { get; set; }

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
/// Asset 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktAssetImportDto
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
    /// 资产代码
    /// </summary>
    public string? AssetCode { get; set; } = string.Empty;

    /// <summary>
    /// 资产名称
    /// </summary>
    public string? AssetName { get; set; } = string.Empty;

    /// <summary>
    /// 资产分类（字典 accounting_financial_asset_category）
    /// </summary>
    public string? AssetCategory { get; set; } = string.Empty;

    /// <summary>
    /// 资产类型（字典 accounting_financial_asset_type；NORM=普通资产）
    /// </summary>
    public string? AssetType { get; set; } = string.Empty;

    /// <summary>
    /// 资产原值
    /// </summary>
    public decimal? AssetOriginalValue { get; set; }

    /// <summary>
    /// 资产净值
    /// </summary>
    public decimal? AssetNetValue { get; set; }

    /// <summary>
    /// 累计折旧
    /// </summary>
    public decimal? AccumulatedDepreciation { get; set; }

    /// <summary>
    /// 成本中心（选项 TaktCostCenters/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CostCenterId { get; set; }

    /// <summary>
    /// 成本中心名称（冗余：按 CostCenterId 取 TaktCostCenter.CostCenterName 联动）
    /// </summary>
    public string? CostCenterName { get; set; } = string.Empty;

    /// <summary>
    /// 部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 使用者（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UserId { get; set; }

    /// <summary>
    /// 使用者名称（冗余：按 UserId 取 TaktUser.UserName 联动）
    /// </summary>
    public string? UserName { get; set; } = string.Empty;

    /// <summary>
    /// 资产位置
    /// </summary>
    public string? AssetLocation { get; set; } = string.Empty;

    /// <summary>
    /// 购买日期
    /// </summary>
    public DateTime? PurchaseDate { get; set; }

    /// <summary>
    /// 启用日期
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// 报废日期
    /// </summary>
    public DateTime? ScrapDate { get; set; }

    /// <summary>
    /// 处置日期
    /// </summary>
    public DateTime? DisposalDate { get; set; }

    /// <summary>
    /// 预计使用月数
    /// </summary>
    public int? ExpectedLifeMonths { get; set; }

    /// <summary>
    /// 折旧方法（字典 accounting_financial_depreciation_method：0=不自动计提，1=直线法，2=双倍余额递减，3=年数总和，4=产量法，5=手工，6=剩余年限直线）
    /// </summary>
    public int? DepreciationMethod { get; set; }

    /// <summary>
    /// 每月折旧金额
    /// </summary>
    public decimal? MonthlyDepreciation { get; set; }

    /// <summary>
    /// 资产状态（字典 accounting_financial_asset_status：0=未使用，1=使用中，2=报废，3=处置，4=实物不存在）
    /// </summary>
    public int? AssetStatus { get; set; }

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
/// Asset 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktAssetExportDto
{
    /// <summary>
    /// AssetID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssetId { get; set; }

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
    /// 资产代码
    /// </summary>
    public string AssetCode { get; set; } = string.Empty;

    /// <summary>
    /// 资产名称
    /// </summary>
    public string AssetName { get; set; } = string.Empty;

    /// <summary>
    /// 资产分类（字典 accounting_financial_asset_category）
    /// </summary>
    public string AssetCategory { get; set; } = string.Empty;

    /// <summary>
    /// 资产类型（字典 accounting_financial_asset_type；NORM=普通资产）
    /// </summary>
    public string AssetType { get; set; } = string.Empty;

    /// <summary>
    /// 资产原值
    /// </summary>
    public decimal AssetOriginalValue { get; set; }

    /// <summary>
    /// 资产净值
    /// </summary>
    public decimal AssetNetValue { get; set; }

    /// <summary>
    /// 累计折旧
    /// </summary>
    public decimal AccumulatedDepreciation { get; set; }

    /// <summary>
    /// 成本中心（选项 TaktCostCenters/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CostCenterId { get; set; }

    /// <summary>
    /// 成本中心名称（冗余：按 CostCenterId 取 TaktCostCenter.CostCenterName 联动）
    /// </summary>
    public string? CostCenterName { get; set; } = string.Empty;

    /// <summary>
    /// 部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 使用者（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UserId { get; set; }

    /// <summary>
    /// 使用者名称（冗余：按 UserId 取 TaktUser.UserName 联动）
    /// </summary>
    public string? UserName { get; set; } = string.Empty;

    /// <summary>
    /// 资产位置
    /// </summary>
    public string? AssetLocation { get; set; } = string.Empty;

    /// <summary>
    /// 购买日期
    /// </summary>
    public DateTime? PurchaseDate { get; set; }

    /// <summary>
    /// 启用日期
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// 报废日期
    /// </summary>
    public DateTime? ScrapDate { get; set; }

    /// <summary>
    /// 处置日期
    /// </summary>
    public DateTime? DisposalDate { get; set; }

    /// <summary>
    /// 预计使用月数
    /// </summary>
    public int ExpectedLifeMonths { get; set; } = 0;

    /// <summary>
    /// 折旧方法（字典 accounting_financial_depreciation_method：0=不自动计提，1=直线法，2=双倍余额递减，3=年数总和，4=产量法，5=手工，6=剩余年限直线）
    /// </summary>
    public int DepreciationMethod { get; set; } = 0;

    /// <summary>
    /// 每月折旧金额
    /// </summary>
    public decimal MonthlyDepreciation { get; set; }

    /// <summary>
    /// 资产状态（字典 accounting_financial_asset_status：0=未使用，1=使用中，2=报废，3=处置，4=实物不存在）
    /// </summary>
    public int AssetStatus { get; set; } = 0;

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
