// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Serial
// 文件名称：TaktSerialInboundDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：SerialInbound 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSerialInbound 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Serial;

// ========================================
// SerialInbound 响应 DTO
// ========================================

/// <summary>
/// 序列号入库主表实体
/// 对应前端 TaktSerialInboundDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSerialInboundDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SerialInboundID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SerialInboundId { get; set; }

    /// <summary>
    /// 入库单号（租户+公司+工厂内唯一）
    /// </summary>
    public string InboundCode { get; set; } = string.Empty;

    /// <summary>
    /// 入库日期
    /// </summary>
    public DateTime InboundDate { get; set; }

    /// <summary>
    /// 入库类型（字典 logistics_materials_inbound_type；0=采购入库 1=生产入库 2=退货入库 3=调拨入库 4=序列号入库 5=其他）
    /// </summary>
    public int InboundType { get; set; } = 0;

    /// <summary>
    /// 仓库编码（选项 TaktWarehouses/options；DictValue=Id）
    /// </summary>
    public string WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 库位编码（选项 TaktStorageLocations/options；DictValue=Id）
    /// </summary>
    public string LocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 总数量
    /// </summary>
    public int TotalQuantity { get; set; } = 0;

    /// <summary>
    /// 序列号入库明细列表（主子表关系）
    /// （子表：TaktSerialInboundItem）
    /// </summary>
    public List<TaktSerialInboundItemDto>? Items { get; set; }

}

// ========================================
// SerialInbound 查询 DTO
// ========================================

/// <summary>
/// SerialInbound 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSerialInboundQueryDto : TaktPagedQuery
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
    /// 入库单号（租户+公司+工厂内唯一）
    /// </summary>
    public string? InboundCode { get; set; } = string.Empty;

    /// <summary>
    /// 入库日期（范围查询-开始）
    /// </summary>
    public DateTime? InboundDateStart { get; set; }

    /// <summary>
    /// 入库日期（范围查询-结束）
    /// </summary>
    public DateTime? InboundDateEnd { get; set; }

    /// <summary>
    /// 入库类型（字典 logistics_materials_inbound_type；0=采购入库 1=生产入库 2=退货入库 3=调拨入库 4=序列号入库 5=其他）
    /// </summary>
    public int? InboundType { get; set; }

    /// <summary>
    /// 仓库编码（选项 TaktWarehouses/options；DictValue=Id）
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 库位编码（选项 TaktStorageLocations/options；DictValue=Id）
    /// </summary>
    public string? LocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 总数量
    /// </summary>
    public int? TotalQuantity { get; set; }

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
// 创建SerialInbound DTO
// ========================================

/// <summary>
/// 创建SerialInbound DTO
/// </summary>
public class TaktSerialInboundCreateDto
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
    /// 入库单号（租户+公司+工厂内唯一）
    /// </summary>
    [Required(ErrorMessage = "入库单号（租户+公司+工厂内唯一）不能为空")]
    public string InboundCode { get; set; } = string.Empty;

    /// <summary>
    /// 入库日期
    /// </summary>
    public DateTime InboundDate { get; set; }

    /// <summary>
    /// 入库类型（字典 logistics_materials_inbound_type；0=采购入库 1=生产入库 2=退货入库 3=调拨入库 4=序列号入库 5=其他）
    /// </summary>
    public int InboundType { get; set; } = 0;

    /// <summary>
    /// 仓库编码（选项 TaktWarehouses/options；DictValue=Id）
    /// </summary>
    [Required(ErrorMessage = "仓库编码（选项 TaktWarehouses/options；DictValue=Id）不能为空")]
    public string WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 库位编码（选项 TaktStorageLocations/options；DictValue=Id）
    /// </summary>
    [Required(ErrorMessage = "库位编码（选项 TaktStorageLocations/options；DictValue=Id）不能为空")]
    public string LocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 总数量
    /// </summary>
    public int TotalQuantity { get; set; } = 0;

    /// <summary>
    /// 序列号入库明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktSerialInboundItemCreateDto>? Items { get; set; }

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
// 更新SerialInbound DTO
// ========================================

/// <summary>
/// 更新SerialInbound DTO
/// 继承 TaktSerialInboundCreateDto，添加 SerialInboundId 字段
/// </summary>
public class TaktSerialInboundUpdateDto : TaktSerialInboundCreateDto
{
    /// <summary>
    /// SerialInboundID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SerialInboundId { get; set; }

    /// <summary>
    /// 序列号入库明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public new List<TaktSerialInboundItemUpdateDto>? Items { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SerialInbound 导入模板行 DTO
/// </summary>
public class TaktSerialInboundTemplateDto
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
    /// 入库单号（租户+公司+工厂内唯一）
    /// </summary>
    public string? InboundCode { get; set; } = string.Empty;

    /// <summary>
    /// 入库日期
    /// </summary>
    public DateTime? InboundDate { get; set; }

    /// <summary>
    /// 入库类型（字典 logistics_materials_inbound_type；0=采购入库 1=生产入库 2=退货入库 3=调拨入库 4=序列号入库 5=其他）
    /// </summary>
    public int? InboundType { get; set; }

    /// <summary>
    /// 仓库编码（选项 TaktWarehouses/options；DictValue=Id）
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 库位编码（选项 TaktStorageLocations/options；DictValue=Id）
    /// </summary>
    public string? LocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 总数量
    /// </summary>
    public int? TotalQuantity { get; set; }

    /// <summary>
    /// 序列号入库明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktSerialInboundItemCreateDto>? Items { get; set; }

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
/// SerialInbound 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSerialInboundImportDto
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
    /// 入库单号（租户+公司+工厂内唯一）
    /// </summary>
    public string? InboundCode { get; set; } = string.Empty;

    /// <summary>
    /// 入库日期
    /// </summary>
    public DateTime? InboundDate { get; set; }

    /// <summary>
    /// 入库类型（字典 logistics_materials_inbound_type；0=采购入库 1=生产入库 2=退货入库 3=调拨入库 4=序列号入库 5=其他）
    /// </summary>
    public int? InboundType { get; set; }

    /// <summary>
    /// 仓库编码（选项 TaktWarehouses/options；DictValue=Id）
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 库位编码（选项 TaktStorageLocations/options；DictValue=Id）
    /// </summary>
    public string? LocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 总数量
    /// </summary>
    public int? TotalQuantity { get; set; }

    /// <summary>
    /// 序列号入库明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktSerialInboundItemCreateDto>? Items { get; set; }

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
/// SerialInbound 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSerialInboundExportDto
{
    /// <summary>
    /// SerialInboundID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SerialInboundId { get; set; }

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
    /// 入库单号（租户+公司+工厂内唯一）
    /// </summary>
    public string InboundCode { get; set; } = string.Empty;

    /// <summary>
    /// 入库日期
    /// </summary>
    public DateTime InboundDate { get; set; }

    /// <summary>
    /// 入库类型（字典 logistics_materials_inbound_type；0=采购入库 1=生产入库 2=退货入库 3=调拨入库 4=序列号入库 5=其他）
    /// </summary>
    public int InboundType { get; set; } = 0;

    /// <summary>
    /// 仓库编码（选项 TaktWarehouses/options；DictValue=Id）
    /// </summary>
    public string WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 库位编码（选项 TaktStorageLocations/options；DictValue=Id）
    /// </summary>
    public string LocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 总数量
    /// </summary>
    public int TotalQuantity { get; set; } = 0;

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
