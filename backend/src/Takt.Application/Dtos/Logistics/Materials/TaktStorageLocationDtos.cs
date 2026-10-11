// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Materials
// 文件名称：TaktStorageLocationDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：StorageLocation 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktStorageLocation 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Materials;

// ========================================
// StorageLocation 响应 DTO
// ========================================

/// <summary>
/// Takt库位主数据实体（公司级；从属于 TaktWarehouse）
/// 对应前端 TaktStorageLocationDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktStorageLocationDto : TaktCompanyDtoBase
{
    /// <summary>
    /// StorageLocationID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long StorageLocationId { get; set; }

    /// <summary>
    /// 仓库 ID（选项 TaktWarehouses/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long WarehouseId { get; set; }

    /// <summary>
    /// 仓库 名称（填充字段）
    /// </summary>
    public string? WarehouseName { get; set; }

    /// <summary>
    /// 仓库编码（冗余；关联 TaktWarehouse.WarehouseCode，选项 TaktWarehouses/options，DictValue=WarehouseCode）
    /// </summary>
    public string WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 库位编码（40，租户+公司+工厂+仓库内唯一；序列号入出库等业务表存此编码）
    /// </summary>
    public string LocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 库位名称
    /// </summary>
    public string LocationName { get; set; } = string.Empty;

    /// <summary>
    /// 库位类型（字典 logistics_materials_storage_location_type）
    /// </summary>
    public int LocationType { get; set; } = 0;

    /// <summary>
    /// 内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 库位状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int LocationStatus { get; set; } = 0;

    /// <summary>
    /// 所属仓库（主子表关系）
    /// （主表：TaktWarehouse）
    /// </summary>
    public TaktWarehouseDto? Warehouse { get; set; }

}

// ========================================
// StorageLocation 查询 DTO
// ========================================

/// <summary>
/// StorageLocation 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktStorageLocationQueryDto : TaktPagedQuery
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
    /// 仓库 ID（选项 TaktWarehouses/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? WarehouseId { get; set; }

    /// <summary>
    /// 仓库编码（冗余；关联 TaktWarehouse.WarehouseCode，选项 TaktWarehouses/options，DictValue=WarehouseCode）
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 库位编码（40，租户+公司+工厂+仓库内唯一；序列号入出库等业务表存此编码）
    /// </summary>
    public string? LocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 库位名称
    /// </summary>
    public string? LocationName { get; set; } = string.Empty;

    /// <summary>
    /// 库位类型（字典 logistics_materials_storage_location_type）
    /// </summary>
    public int? LocationType { get; set; }

    /// <summary>
    /// 内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 库位状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int? LocationStatus { get; set; }

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
// 创建StorageLocation DTO
// ========================================

/// <summary>
/// 创建StorageLocation DTO
/// </summary>
public class TaktStorageLocationCreateDto
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
    /// 仓库 ID（选项 TaktWarehouses/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long WarehouseId { get; set; }

    /// <summary>
    /// 仓库编码（冗余；关联 TaktWarehouse.WarehouseCode，选项 TaktWarehouses/options，DictValue=WarehouseCode）
    /// </summary>
    public string WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 库位编码（40，租户+公司+工厂+仓库内唯一；序列号入出库等业务表存此编码）
    /// </summary>
    [Required(ErrorMessage = "库位编码（40，租户+公司+工厂+仓库内唯一；序列号入出库等业务表存此编码）不能为空")]
    public string LocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 库位名称
    /// </summary>
    [Required(ErrorMessage = "库位名称不能为空")]
    public string LocationName { get; set; } = string.Empty;

    /// <summary>
    /// 库位类型（字典 logistics_materials_storage_location_type）
    /// </summary>
    public int LocationType { get; set; } = 0;

    /// <summary>
    /// 内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 库位状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int LocationStatus { get; set; } = 0;

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
// 更新StorageLocation DTO
// ========================================

/// <summary>
/// 更新StorageLocation DTO
/// 继承 TaktStorageLocationCreateDto，添加 StorageLocationId 字段
/// </summary>
public class TaktStorageLocationUpdateDto : TaktStorageLocationCreateDto
{
    /// <summary>
    /// StorageLocationID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long StorageLocationId { get; set; }

}

// ========================================
// StorageLocation 状态 DTO
// ========================================

/// <summary>
/// StorageLocation 状态更新 DTO
/// </summary>
public class TaktStorageLocationStatusDto
{
    /// <summary>
    /// StorageLocationID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long StorageLocationId { get; set; }

    /// <summary>
    /// 库位状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
    /// </summary>
    [Required(ErrorMessage = "库位状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）不能为空")]
    public int LocationStatus { get; set; } = 0;
}

// ========================================
// StorageLocation 排序 DTO
// ========================================

/// <summary>
/// StorageLocation 排序更新 DTO
/// </summary>
public class TaktStorageLocationSortDto
{
    /// <summary>
    /// StorageLocationID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long StorageLocationId { get; set; }

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
/// StorageLocation 导入模板行 DTO
/// </summary>
public class TaktStorageLocationTemplateDto
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
    /// 仓库 ID（选项 TaktWarehouses/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? WarehouseId { get; set; }

    /// <summary>
    /// 仓库编码（冗余；关联 TaktWarehouse.WarehouseCode，选项 TaktWarehouses/options，DictValue=WarehouseCode）
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 库位编码（40，租户+公司+工厂+仓库内唯一；序列号入出库等业务表存此编码）
    /// </summary>
    public string? LocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 库位名称
    /// </summary>
    public string? LocationName { get; set; } = string.Empty;

    /// <summary>
    /// 库位类型（字典 logistics_materials_storage_location_type）
    /// </summary>
    public int? LocationType { get; set; }

    /// <summary>
    /// 内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 库位状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int? LocationStatus { get; set; }

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
/// StorageLocation 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktStorageLocationImportDto
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
    /// 仓库 ID（选项 TaktWarehouses/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? WarehouseId { get; set; }

    /// <summary>
    /// 仓库编码（冗余；关联 TaktWarehouse.WarehouseCode，选项 TaktWarehouses/options，DictValue=WarehouseCode）
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 库位编码（40，租户+公司+工厂+仓库内唯一；序列号入出库等业务表存此编码）
    /// </summary>
    public string? LocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 库位名称
    /// </summary>
    public string? LocationName { get; set; } = string.Empty;

    /// <summary>
    /// 库位类型（字典 logistics_materials_storage_location_type）
    /// </summary>
    public int? LocationType { get; set; }

    /// <summary>
    /// 内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 库位状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int? LocationStatus { get; set; }

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
/// StorageLocation 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktStorageLocationExportDto
{
    /// <summary>
    /// StorageLocationID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long StorageLocationId { get; set; }

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
    /// 仓库 ID（选项 TaktWarehouses/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long WarehouseId { get; set; }

    /// <summary>
    /// 仓库编码（冗余；关联 TaktWarehouse.WarehouseCode，选项 TaktWarehouses/options，DictValue=WarehouseCode）
    /// </summary>
    public string WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 库位编码（40，租户+公司+工厂+仓库内唯一；序列号入出库等业务表存此编码）
    /// </summary>
    public string LocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 库位名称
    /// </summary>
    public string LocationName { get; set; } = string.Empty;

    /// <summary>
    /// 库位类型（字典 logistics_materials_storage_location_type）
    /// </summary>
    public int LocationType { get; set; } = 0;

    /// <summary>
    /// 内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 库位状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int LocationStatus { get; set; } = 0;

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
