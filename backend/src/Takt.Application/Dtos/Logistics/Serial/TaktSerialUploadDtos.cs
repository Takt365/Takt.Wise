// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Serial
// 文件名称：TaktSerialUploadDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：SerialUpload 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSerialUpload 生成，请按需审阅）
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
// SerialUpload 响应 DTO
// ========================================

/// <summary>
/// 序列号上传（公司级；发货票维度的送货/装箱明细行）
/// 对应前端 TaktSerialUploadDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSerialUploadDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SerialUploadID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SerialUploadId { get; set; }


    /// <summary>
    /// 出库日期
    /// </summary>
    public DateTime OutboundDate { get; set; }

    /// <summary>
    /// 发货单号（固定 9 位）
    /// </summary>
    public string ShippingInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 序号（同一工厂+发货单号内唯一）
    /// </summary>
    public int SequenceCode { get; set; } = 0;

    /// <summary>
    /// 产品物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode；最长 20）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 合计数量
    /// </summary>
    public int TotalQuantity { get; set; } = 0;

    /// <summary>
    /// 序列号（固定 7 位）
    /// </summary>
    public string SerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 装箱数量
    /// </summary>
    public int PackingQuantity { get; set; } = 0;

    /// <summary>
    /// 运输方式（最长 20）
    /// </summary>
    public string TransportMode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（最长 40）
    /// </summary>
    public string MaterialText { get; set; } = string.Empty;

}

// ========================================
// SerialUpload 查询 DTO
// ========================================

/// <summary>
/// SerialUpload 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSerialUploadQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码
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
    /// 出库日期（范围查询-开始）
    /// </summary>
    public DateTime? OutboundDateStart { get; set; }

    /// <summary>
    /// 出库日期（范围查询-结束）
    /// </summary>
    public DateTime? OutboundDateEnd { get; set; }

    /// <summary>
    /// 发货单号（固定 9 位）
    /// </summary>
    public string? ShippingInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 序号（同一工厂+发货单号内唯一）
    /// </summary>
    public int? SequenceCode { get; set; }

    /// <summary>
    /// 产品物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode；最长 20）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 合计数量
    /// </summary>
    public int? TotalQuantity { get; set; }

    /// <summary>
    /// 序列号（固定 7 位）
    /// </summary>
    public string? SerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 装箱数量
    /// </summary>
    public int? PackingQuantity { get; set; }

    /// <summary>
    /// 运输方式（最长 20）
    /// </summary>
    public string? TransportMode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（最长 40）
    /// </summary>
    public string? MaterialText { get; set; } = string.Empty;

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
// 创建SerialUpload DTO
// ========================================

/// <summary>
/// 创建SerialUpload DTO
/// </summary>
public class TaktSerialUploadCreateDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "工厂代码（选项 TaktPlants/options；DictValue=PlantCode）不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 出库日期
    /// </summary>
    public DateTime OutboundDate { get; set; }

    /// <summary>
    /// 发货单号（固定 9 位）
    /// </summary>
    [Required(ErrorMessage = "发货单号（固定 9 位）不能为空")]
    public string ShippingInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 序号（同一工厂+发货单号内唯一）
    /// </summary>
    public int SequenceCode { get; set; } = 0;

    /// <summary>
    /// 产品物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode；最长 20）
    /// </summary>
    [Required(ErrorMessage = "产品物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode；最长 20）不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 合计数量
    /// </summary>
    public int TotalQuantity { get; set; } = 0;

    /// <summary>
    /// 序列号（固定 7 位）
    /// </summary>
    [Required(ErrorMessage = "序列号（固定 7 位）不能为空")]
    public string SerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 装箱数量
    /// </summary>
    public int PackingQuantity { get; set; } = 0;

    /// <summary>
    /// 运输方式（最长 20）
    /// </summary>
    [Required(ErrorMessage = "运输方式（最长 20）不能为空")]
    public string TransportMode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（最长 40）
    /// </summary>
    [Required(ErrorMessage = "物料描述（最长 40）不能为空")]
    public string MaterialText { get; set; } = string.Empty;

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
// 更新SerialUpload DTO
// ========================================

/// <summary>
/// 更新SerialUpload DTO
/// 继承 TaktSerialUploadCreateDto，添加 SerialUploadId 字段
/// </summary>
public class TaktSerialUploadUpdateDto : TaktSerialUploadCreateDto
{
    /// <summary>
    /// SerialUploadID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SerialUploadId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SerialUpload 导入模板行 DTO
/// </summary>
public class TaktSerialUploadTemplateDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 出库日期
    /// </summary>
    public DateTime? OutboundDate { get; set; }

    /// <summary>
    /// 发货单号（固定 9 位）
    /// </summary>
    public string? ShippingInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 序号（同一工厂+发货单号内唯一）
    /// </summary>
    public int? SequenceCode { get; set; }

    /// <summary>
    /// 产品物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode；最长 20）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 合计数量
    /// </summary>
    public int? TotalQuantity { get; set; }

    /// <summary>
    /// 序列号（固定 7 位）
    /// </summary>
    public string? SerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 装箱数量
    /// </summary>
    public int? PackingQuantity { get; set; }

    /// <summary>
    /// 运输方式（最长 20）
    /// </summary>
    public string? TransportMode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（最长 40）
    /// </summary>
    public string? MaterialText { get; set; } = string.Empty;

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
/// SerialUpload 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSerialUploadImportDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 出库日期
    /// </summary>
    public DateTime? OutboundDate { get; set; }

    /// <summary>
    /// 发货单号（固定 9 位）
    /// </summary>
    public string? ShippingInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 序号（同一工厂+发货单号内唯一）
    /// </summary>
    public int? SequenceCode { get; set; }

    /// <summary>
    /// 产品物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode；最长 20）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 合计数量
    /// </summary>
    public int? TotalQuantity { get; set; }

    /// <summary>
    /// 序列号（固定 7 位）
    /// </summary>
    public string? SerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 装箱数量
    /// </summary>
    public int? PackingQuantity { get; set; }

    /// <summary>
    /// 运输方式（最长 20）
    /// </summary>
    public string? TransportMode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（最长 40）
    /// </summary>
    public string? MaterialText { get; set; } = string.Empty;

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
/// SerialUpload 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSerialUploadExportDto
{
    /// <summary>
    /// SerialUploadID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SerialUploadId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 出库日期
    /// </summary>
    public DateTime OutboundDate { get; set; }

    /// <summary>
    /// 发货单号（固定 9 位）
    /// </summary>
    public string ShippingInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 序号（同一工厂+发货单号内唯一）
    /// </summary>
    public int SequenceCode { get; set; } = 0;

    /// <summary>
    /// 产品物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode；最长 20）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 合计数量
    /// </summary>
    public int TotalQuantity { get; set; } = 0;

    /// <summary>
    /// 序列号（固定 7 位）
    /// </summary>
    public string SerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 装箱数量
    /// </summary>
    public int PackingQuantity { get; set; } = 0;

    /// <summary>
    /// 运输方式（最长 20）
    /// </summary>
    public string TransportMode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（最长 40）
    /// </summary>
    public string MaterialText { get; set; } = string.Empty;

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
