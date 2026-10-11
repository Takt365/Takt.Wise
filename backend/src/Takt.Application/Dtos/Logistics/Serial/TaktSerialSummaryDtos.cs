// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Serial
// 文件名称：TaktSerialSummaryDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：SerialSummary 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSerialSummary 生成，请按需审阅）
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
// SerialSummary 响应 DTO
// ========================================

/// <summary>
/// 序列号汇总（公司级；一行对应一笔入库序列及其可选出库对照）
/// 对应前端 TaktSerialSummaryDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSerialSummaryDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SerialSummaryID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SerialSummaryId { get; set; }


    /// <summary>
    /// 入库单号
    /// </summary>
    public string InboundCode { get; set; } = string.Empty;

    /// <summary>
    /// 入库日期
    /// </summary>
    public DateTime InboundDate { get; set; }

    /// <summary>
    /// 产品物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 入库序列号（计算后的业务序号；租户+公司+工厂内唯一）
    /// </summary>
    public string InboundSerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 入库数量
    /// </summary>
    public int InboundQuantity { get; set; } = 0;

    /// <summary>
    /// 产品入库序列号（原始扫描号码）
    /// </summary>
    public string ProductInboundSerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 出库单号（未出库时为空）
    /// </summary>
    public string OutboundCode { get; set; } = string.Empty;

    /// <summary>
    /// 发货单号（未出库时为空）
    /// </summary>
    public string ShippingInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 装车日期（未装车时为空）
    /// </summary>
    public DateTime? LoadingDate { get; set; }

    /// <summary>
    /// 仕向地（选项 TaktModelDestinations/options；DictValue=DestinationCode）
    /// </summary>
    public string Destination { get; set; } = string.Empty;

    /// <summary>
    /// 目的地港（字典 logistics_serial_destination_port_code；DictValue 为港口/运输编码，如 ACE_AIR、VIE）
    /// </summary>
    public string DestinationPort { get; set; } = string.Empty;

    /// <summary>
    /// 出库日期（未出库时为空）
    /// </summary>
    public DateTime? OutboundDate { get; set; }

    /// <summary>
    /// 出库序列号（计算后的业务序号；未出库时为空）
    /// </summary>
    public string OutboundSerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 出库数量
    /// </summary>
    public int OutboundQuantity { get; set; } = 0;

    /// <summary>
    /// 产品出库序列号（原始扫描号码；未出库时为空）
    /// </summary>
    public string ProductOutboundSerialCode { get; set; } = string.Empty;

}

// ========================================
// SerialSummary 查询 DTO
// ========================================

/// <summary>
/// SerialSummary 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSerialSummaryQueryDto : TaktPagedQuery
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
    /// 入库单号
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
    /// 产品物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 入库序列号（计算后的业务序号；租户+公司+工厂内唯一）
    /// </summary>
    public string? InboundSerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 入库数量
    /// </summary>
    public int? InboundQuantity { get; set; }

    /// <summary>
    /// 产品入库序列号（原始扫描号码）
    /// </summary>
    public string? ProductInboundSerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 出库单号（未出库时为空）
    /// </summary>
    public string? OutboundCode { get; set; } = string.Empty;

    /// <summary>
    /// 发货单号（未出库时为空）
    /// </summary>
    public string? ShippingInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 装车日期（未装车时为空）（范围查询-开始）
    /// </summary>
    public DateTime? LoadingDateStart { get; set; }

    /// <summary>
    /// 装车日期（未装车时为空）（范围查询-结束）
    /// </summary>
    public DateTime? LoadingDateEnd { get; set; }

    /// <summary>
    /// 仕向地（选项 TaktModelDestinations/options；DictValue=DestinationCode）
    /// </summary>
    public string? Destination { get; set; } = string.Empty;

    /// <summary>
    /// 目的地港（字典 logistics_serial_destination_port_code；DictValue 为港口/运输编码，如 ACE_AIR、VIE）
    /// </summary>
    public string? DestinationPort { get; set; } = string.Empty;

    /// <summary>
    /// 出库日期（未出库时为空）（范围查询-开始）
    /// </summary>
    public DateTime? OutboundDateStart { get; set; }

    /// <summary>
    /// 出库日期（未出库时为空）（范围查询-结束）
    /// </summary>
    public DateTime? OutboundDateEnd { get; set; }

    /// <summary>
    /// 出库序列号（计算后的业务序号；未出库时为空）
    /// </summary>
    public string? OutboundSerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 出库数量
    /// </summary>
    public int? OutboundQuantity { get; set; }

    /// <summary>
    /// 产品出库序列号（原始扫描号码；未出库时为空）
    /// </summary>
    public string? ProductOutboundSerialCode { get; set; } = string.Empty;

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
// 创建SerialSummary DTO
// ========================================

/// <summary>
/// 创建SerialSummary DTO
/// </summary>
public class TaktSerialSummaryCreateDto
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
    /// 入库单号
    /// </summary>
    [Required(ErrorMessage = "入库单号不能为空")]
    public string InboundCode { get; set; } = string.Empty;

    /// <summary>
    /// 入库日期
    /// </summary>
    public DateTime InboundDate { get; set; }

    /// <summary>
    /// 产品物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "产品物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 入库序列号（计算后的业务序号；租户+公司+工厂内唯一）
    /// </summary>
    [Required(ErrorMessage = "入库序列号（计算后的业务序号；租户+公司+工厂内唯一）不能为空")]
    public string InboundSerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 入库数量
    /// </summary>
    public int InboundQuantity { get; set; } = 0;

    /// <summary>
    /// 产品入库序列号（原始扫描号码）
    /// </summary>
    [Required(ErrorMessage = "产品入库序列号（原始扫描号码）不能为空")]
    public string ProductInboundSerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 出库单号（未出库时为空）
    /// </summary>
    [Required(ErrorMessage = "出库单号（未出库时为空）不能为空")]
    public string OutboundCode { get; set; } = string.Empty;

    /// <summary>
    /// 发货单号（未出库时为空）
    /// </summary>
    [Required(ErrorMessage = "发货单号（未出库时为空）不能为空")]
    public string ShippingInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 装车日期（未装车时为空）
    /// </summary>
    public DateTime? LoadingDate { get; set; }

    /// <summary>
    /// 仕向地（选项 TaktModelDestinations/options；DictValue=DestinationCode）
    /// </summary>
    [Required(ErrorMessage = "仕向地（选项 TaktModelDestinations/options；DictValue=DestinationCode）不能为空")]
    public string Destination { get; set; } = string.Empty;

    /// <summary>
    /// 目的地港（字典 logistics_serial_destination_port_code；DictValue 为港口/运输编码，如 ACE_AIR、VIE）
    /// </summary>
    [Required(ErrorMessage = "目的地港（字典 logistics_serial_destination_port_code；DictValue 为港口/运输编码，如 ACE_AIR、VIE）不能为空")]
    public string DestinationPort { get; set; } = string.Empty;

    /// <summary>
    /// 出库日期（未出库时为空）
    /// </summary>
    public DateTime? OutboundDate { get; set; }

    /// <summary>
    /// 出库序列号（计算后的业务序号；未出库时为空）
    /// </summary>
    [Required(ErrorMessage = "出库序列号（计算后的业务序号；未出库时为空）不能为空")]
    public string OutboundSerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 出库数量
    /// </summary>
    public int OutboundQuantity { get; set; } = 0;

    /// <summary>
    /// 产品出库序列号（原始扫描号码；未出库时为空）
    /// </summary>
    [Required(ErrorMessage = "产品出库序列号（原始扫描号码；未出库时为空）不能为空")]
    public string ProductOutboundSerialCode { get; set; } = string.Empty;

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
// 更新SerialSummary DTO
// ========================================

/// <summary>
/// 更新SerialSummary DTO
/// 继承 TaktSerialSummaryCreateDto，添加 SerialSummaryId 字段
/// </summary>
public class TaktSerialSummaryUpdateDto : TaktSerialSummaryCreateDto
{
    /// <summary>
    /// SerialSummaryID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SerialSummaryId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SerialSummary 导入模板行 DTO
/// </summary>
public class TaktSerialSummaryTemplateDto
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
    /// 入库单号
    /// </summary>
    public string? InboundCode { get; set; } = string.Empty;

    /// <summary>
    /// 入库日期
    /// </summary>
    public DateTime? InboundDate { get; set; }

    /// <summary>
    /// 产品物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 入库序列号（计算后的业务序号；租户+公司+工厂内唯一）
    /// </summary>
    public string? InboundSerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 入库数量
    /// </summary>
    public int? InboundQuantity { get; set; }

    /// <summary>
    /// 产品入库序列号（原始扫描号码）
    /// </summary>
    public string? ProductInboundSerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 出库单号（未出库时为空）
    /// </summary>
    public string? OutboundCode { get; set; } = string.Empty;

    /// <summary>
    /// 发货单号（未出库时为空）
    /// </summary>
    public string? ShippingInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 装车日期（未装车时为空）
    /// </summary>
    public DateTime? LoadingDate { get; set; }

    /// <summary>
    /// 仕向地（选项 TaktModelDestinations/options；DictValue=DestinationCode）
    /// </summary>
    public string? Destination { get; set; } = string.Empty;

    /// <summary>
    /// 目的地港（字典 logistics_serial_destination_port_code；DictValue 为港口/运输编码，如 ACE_AIR、VIE）
    /// </summary>
    public string? DestinationPort { get; set; } = string.Empty;

    /// <summary>
    /// 出库日期（未出库时为空）
    /// </summary>
    public DateTime? OutboundDate { get; set; }

    /// <summary>
    /// 出库序列号（计算后的业务序号；未出库时为空）
    /// </summary>
    public string? OutboundSerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 出库数量
    /// </summary>
    public int? OutboundQuantity { get; set; }

    /// <summary>
    /// 产品出库序列号（原始扫描号码；未出库时为空）
    /// </summary>
    public string? ProductOutboundSerialCode { get; set; } = string.Empty;

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
/// SerialSummary 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSerialSummaryImportDto
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
    /// 入库单号
    /// </summary>
    public string? InboundCode { get; set; } = string.Empty;

    /// <summary>
    /// 入库日期
    /// </summary>
    public DateTime? InboundDate { get; set; }

    /// <summary>
    /// 产品物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 入库序列号（计算后的业务序号；租户+公司+工厂内唯一）
    /// </summary>
    public string? InboundSerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 入库数量
    /// </summary>
    public int? InboundQuantity { get; set; }

    /// <summary>
    /// 产品入库序列号（原始扫描号码）
    /// </summary>
    public string? ProductInboundSerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 出库单号（未出库时为空）
    /// </summary>
    public string? OutboundCode { get; set; } = string.Empty;

    /// <summary>
    /// 发货单号（未出库时为空）
    /// </summary>
    public string? ShippingInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 装车日期（未装车时为空）
    /// </summary>
    public DateTime? LoadingDate { get; set; }

    /// <summary>
    /// 仕向地（选项 TaktModelDestinations/options；DictValue=DestinationCode）
    /// </summary>
    public string? Destination { get; set; } = string.Empty;

    /// <summary>
    /// 目的地港（字典 logistics_serial_destination_port_code；DictValue 为港口/运输编码，如 ACE_AIR、VIE）
    /// </summary>
    public string? DestinationPort { get; set; } = string.Empty;

    /// <summary>
    /// 出库日期（未出库时为空）
    /// </summary>
    public DateTime? OutboundDate { get; set; }

    /// <summary>
    /// 出库序列号（计算后的业务序号；未出库时为空）
    /// </summary>
    public string? OutboundSerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 出库数量
    /// </summary>
    public int? OutboundQuantity { get; set; }

    /// <summary>
    /// 产品出库序列号（原始扫描号码；未出库时为空）
    /// </summary>
    public string? ProductOutboundSerialCode { get; set; } = string.Empty;

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
/// SerialSummary 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSerialSummaryExportDto
{
    /// <summary>
    /// SerialSummaryID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SerialSummaryId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 入库单号
    /// </summary>
    public string InboundCode { get; set; } = string.Empty;

    /// <summary>
    /// 入库日期
    /// </summary>
    public DateTime InboundDate { get; set; }

    /// <summary>
    /// 产品物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 入库序列号（计算后的业务序号；租户+公司+工厂内唯一）
    /// </summary>
    public string InboundSerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 入库数量
    /// </summary>
    public int InboundQuantity { get; set; } = 0;

    /// <summary>
    /// 产品入库序列号（原始扫描号码）
    /// </summary>
    public string ProductInboundSerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 出库单号（未出库时为空）
    /// </summary>
    public string OutboundCode { get; set; } = string.Empty;

    /// <summary>
    /// 发货单号（未出库时为空）
    /// </summary>
    public string ShippingInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 装车日期（未装车时为空）
    /// </summary>
    public DateTime? LoadingDate { get; set; }

    /// <summary>
    /// 仕向地（选项 TaktModelDestinations/options；DictValue=DestinationCode）
    /// </summary>
    public string Destination { get; set; } = string.Empty;

    /// <summary>
    /// 目的地港（字典 logistics_serial_destination_port_code；DictValue 为港口/运输编码，如 ACE_AIR、VIE）
    /// </summary>
    public string DestinationPort { get; set; } = string.Empty;

    /// <summary>
    /// 出库日期（未出库时为空）
    /// </summary>
    public DateTime? OutboundDate { get; set; }

    /// <summary>
    /// 出库序列号（计算后的业务序号；未出库时为空）
    /// </summary>
    public string OutboundSerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 出库数量
    /// </summary>
    public int OutboundQuantity { get; set; } = 0;

    /// <summary>
    /// 产品出库序列号（原始扫描号码；未出库时为空）
    /// </summary>
    public string ProductOutboundSerialCode { get; set; } = string.Empty;

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
