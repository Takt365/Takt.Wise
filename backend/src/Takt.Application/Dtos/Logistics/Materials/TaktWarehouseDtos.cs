// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Materials
// 文件名称：TaktWarehouseDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：Warehouse 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktWarehouse 生成，请按需审阅）
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
// Warehouse 响应 DTO
// ========================================

/// <summary>
/// Takt仓库主数据实体（公司级；按工厂划分仓储地点）
/// 对应前端 TaktWarehouseDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktWarehouseDto : TaktCompanyDtoBase
{
    /// <summary>
    /// WarehouseID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long WarehouseId { get; set; }

    /// <summary>
    /// 存货地点编码（4位，租户+公司+工厂内唯一；业务表冗余存此编码）
    /// </summary>
    public string WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 仓库名称
    /// </summary>
    public string WarehouseName { get; set; } = string.Empty;

    /// <summary>
    /// 仓库简称
    /// </summary>
    public string? WarehouseShortName { get; set; } = string.Empty;

    /// <summary>
    /// 仓库地址（address）
    /// </summary>
    public string? Address { get; set; } = string.Empty;

    /// <summary>
    /// 联系人（contact_person）
    /// </summary>
    public string? ContactPerson { get; set; } = string.Empty;

    /// <summary>
    /// 联系电话（contact_phone）
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 仓库负责人用户编码（选项 TaktUsers/options；DictValue=UserName）
    /// </summary>
    public string? ManagerUserName { get; set; } = string.Empty;

    /// <summary>
    /// 虚拟仓（is_virtual；字典 sys_yes_no；0=实体仓，1=虚拟仓）
    /// </summary>
    public int IsVirtual { get; set; } = 0;

    /// <summary>
    /// 仓库类型（字典 logistics_materials_warehouse_type；0=原材料仓，1=半成品仓，2=成品仓，3=不良品仓，4=外协仓，5=其他）
    /// </summary>
    public int WarehouseType { get; set; } = 0;

    /// <summary>
    /// 内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 仓库状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int WarehouseStatus { get; set; } = 0;

    /// <summary>
    /// 库位列表（主子表关系）
    /// （子表：TaktStorageLocation）
    /// </summary>
    public List<TaktStorageLocationDto>? StorageLocations { get; set; }

}

// ========================================
// Warehouse 查询 DTO
// ========================================

/// <summary>
/// Warehouse 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktWarehouseQueryDto : TaktPagedQuery
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
    /// 存货地点编码（4位，租户+公司+工厂内唯一；业务表冗余存此编码）
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 仓库名称
    /// </summary>
    public string? WarehouseName { get; set; } = string.Empty;

    /// <summary>
    /// 仓库简称
    /// </summary>
    public string? WarehouseShortName { get; set; } = string.Empty;

    /// <summary>
    /// 仓库地址（address）
    /// </summary>
    public string? Address { get; set; } = string.Empty;

    /// <summary>
    /// 联系人（contact_person）
    /// </summary>
    public string? ContactPerson { get; set; } = string.Empty;

    /// <summary>
    /// 联系电话（contact_phone）
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 仓库负责人用户编码（选项 TaktUsers/options；DictValue=UserName）
    /// </summary>
    public string? ManagerUserName { get; set; } = string.Empty;

    /// <summary>
    /// 虚拟仓（is_virtual；字典 sys_yes_no；0=实体仓，1=虚拟仓）
    /// </summary>
    public int? IsVirtual { get; set; }

    /// <summary>
    /// 仓库类型（字典 logistics_materials_warehouse_type；0=原材料仓，1=半成品仓，2=成品仓，3=不良品仓，4=外协仓，5=其他）
    /// </summary>
    public int? WarehouseType { get; set; }

    /// <summary>
    /// 内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 仓库状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int? WarehouseStatus { get; set; }

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
// 创建Warehouse DTO
// ========================================

/// <summary>
/// 创建Warehouse DTO
/// </summary>
public class TaktWarehouseCreateDto
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
    /// 存货地点编码（4位，租户+公司+工厂内唯一；业务表冗余存此编码）
    /// </summary>
    [Required(ErrorMessage = "存货地点编码（4位，租户+公司+工厂内唯一；业务表冗余存此编码）不能为空")]
    public string WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 仓库名称
    /// </summary>
    [Required(ErrorMessage = "仓库名称不能为空")]
    public string WarehouseName { get; set; } = string.Empty;

    /// <summary>
    /// 仓库简称
    /// </summary>
    public string? WarehouseShortName { get; set; } = string.Empty;

    /// <summary>
    /// 仓库地址（address）
    /// </summary>
    public string? Address { get; set; } = string.Empty;

    /// <summary>
    /// 联系人（contact_person）
    /// </summary>
    public string? ContactPerson { get; set; } = string.Empty;

    /// <summary>
    /// 联系电话（contact_phone）
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 仓库负责人用户编码（选项 TaktUsers/options；DictValue=UserName）
    /// </summary>
    public string? ManagerUserName { get; set; } = string.Empty;

    /// <summary>
    /// 虚拟仓（is_virtual；字典 sys_yes_no；0=实体仓，1=虚拟仓）
    /// </summary>
    public int IsVirtual { get; set; } = 0;

    /// <summary>
    /// 仓库类型（字典 logistics_materials_warehouse_type；0=原材料仓，1=半成品仓，2=成品仓，3=不良品仓，4=外协仓，5=其他）
    /// </summary>
    public int WarehouseType { get; set; } = 0;

    /// <summary>
    /// 内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 仓库状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int WarehouseStatus { get; set; } = 0;

    /// <summary>
    /// 库位列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktStorageLocationCreateDto>? StorageLocations { get; set; }

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
// 更新Warehouse DTO
// ========================================

/// <summary>
/// 更新Warehouse DTO
/// 继承 TaktWarehouseCreateDto，添加 WarehouseId 字段
/// </summary>
public class TaktWarehouseUpdateDto : TaktWarehouseCreateDto
{
    /// <summary>
    /// WarehouseID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long WarehouseId { get; set; }

    /// <summary>
    /// 库位列表（主子表关系）（子表，级联保存）
    /// </summary>
    public new List<TaktStorageLocationUpdateDto>? StorageLocations { get; set; }

}

// ========================================
// Warehouse 状态 DTO
// ========================================

/// <summary>
/// Warehouse 状态更新 DTO
/// </summary>
public class TaktWarehouseStatusDto
{
    /// <summary>
    /// WarehouseID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long WarehouseId { get; set; }

    /// <summary>
    /// 仓库状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
    /// </summary>
    [Required(ErrorMessage = "仓库状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）不能为空")]
    public int WarehouseStatus { get; set; } = 0;
}

// ========================================
// Warehouse 排序 DTO
// ========================================

/// <summary>
/// Warehouse 排序更新 DTO
/// </summary>
public class TaktWarehouseSortDto
{
    /// <summary>
    /// WarehouseID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long WarehouseId { get; set; }

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
/// Warehouse 导入模板行 DTO
/// </summary>
public class TaktWarehouseTemplateDto
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
    /// 存货地点编码（4位，租户+公司+工厂内唯一；业务表冗余存此编码）
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 仓库名称
    /// </summary>
    public string? WarehouseName { get; set; } = string.Empty;

    /// <summary>
    /// 仓库简称
    /// </summary>
    public string? WarehouseShortName { get; set; } = string.Empty;

    /// <summary>
    /// 仓库地址（address）
    /// </summary>
    public string? Address { get; set; } = string.Empty;

    /// <summary>
    /// 联系人（contact_person）
    /// </summary>
    public string? ContactPerson { get; set; } = string.Empty;

    /// <summary>
    /// 联系电话（contact_phone）
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 仓库负责人用户编码（选项 TaktUsers/options；DictValue=UserName）
    /// </summary>
    public string? ManagerUserName { get; set; } = string.Empty;

    /// <summary>
    /// 虚拟仓（is_virtual；字典 sys_yes_no；0=实体仓，1=虚拟仓）
    /// </summary>
    public int? IsVirtual { get; set; }

    /// <summary>
    /// 仓库类型（字典 logistics_materials_warehouse_type；0=原材料仓，1=半成品仓，2=成品仓，3=不良品仓，4=外协仓，5=其他）
    /// </summary>
    public int? WarehouseType { get; set; }

    /// <summary>
    /// 内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 仓库状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int? WarehouseStatus { get; set; }

    /// <summary>
    /// 库位列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktStorageLocationCreateDto>? StorageLocations { get; set; }

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
/// Warehouse 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktWarehouseImportDto
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
    /// 存货地点编码（4位，租户+公司+工厂内唯一；业务表冗余存此编码）
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 仓库名称
    /// </summary>
    public string? WarehouseName { get; set; } = string.Empty;

    /// <summary>
    /// 仓库简称
    /// </summary>
    public string? WarehouseShortName { get; set; } = string.Empty;

    /// <summary>
    /// 仓库地址（address）
    /// </summary>
    public string? Address { get; set; } = string.Empty;

    /// <summary>
    /// 联系人（contact_person）
    /// </summary>
    public string? ContactPerson { get; set; } = string.Empty;

    /// <summary>
    /// 联系电话（contact_phone）
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 仓库负责人用户编码（选项 TaktUsers/options；DictValue=UserName）
    /// </summary>
    public string? ManagerUserName { get; set; } = string.Empty;

    /// <summary>
    /// 虚拟仓（is_virtual；字典 sys_yes_no；0=实体仓，1=虚拟仓）
    /// </summary>
    public int? IsVirtual { get; set; }

    /// <summary>
    /// 仓库类型（字典 logistics_materials_warehouse_type；0=原材料仓，1=半成品仓，2=成品仓，3=不良品仓，4=外协仓，5=其他）
    /// </summary>
    public int? WarehouseType { get; set; }

    /// <summary>
    /// 内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 仓库状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int? WarehouseStatus { get; set; }

    /// <summary>
    /// 库位列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktStorageLocationCreateDto>? StorageLocations { get; set; }

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
/// Warehouse 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktWarehouseExportDto
{
    /// <summary>
    /// WarehouseID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long WarehouseId { get; set; }

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
    /// 存货地点编码（4位，租户+公司+工厂内唯一；业务表冗余存此编码）
    /// </summary>
    public string WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 仓库名称
    /// </summary>
    public string WarehouseName { get; set; } = string.Empty;

    /// <summary>
    /// 仓库简称
    /// </summary>
    public string? WarehouseShortName { get; set; } = string.Empty;

    /// <summary>
    /// 仓库地址（address）
    /// </summary>
    public string? Address { get; set; } = string.Empty;

    /// <summary>
    /// 联系人（contact_person）
    /// </summary>
    public string? ContactPerson { get; set; } = string.Empty;

    /// <summary>
    /// 联系电话（contact_phone）
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 仓库负责人用户编码（选项 TaktUsers/options；DictValue=UserName）
    /// </summary>
    public string? ManagerUserName { get; set; } = string.Empty;

    /// <summary>
    /// 虚拟仓（is_virtual；字典 sys_yes_no；0=实体仓，1=虚拟仓）
    /// </summary>
    public int IsVirtual { get; set; } = 0;

    /// <summary>
    /// 仓库类型（字典 logistics_materials_warehouse_type；0=原材料仓，1=半成品仓，2=成品仓，3=不良品仓，4=外协仓，5=其他）
    /// </summary>
    public int WarehouseType { get; set; } = 0;

    /// <summary>
    /// 内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 仓库状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int WarehouseStatus { get; set; } = 0;

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
