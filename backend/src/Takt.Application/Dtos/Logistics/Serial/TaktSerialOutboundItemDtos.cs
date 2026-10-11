// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Serial
// 文件名称：TaktSerialOutboundItemDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：SerialOutboundItem 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSerialOutboundItem 生成，请按需审阅）
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
// SerialOutboundItem 响应 DTO
// ========================================

/// <summary>
/// 序列号出库明细实体
/// 对应前端 TaktSerialOutboundItemDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSerialOutboundItemDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SerialOutboundItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SerialOutboundItemId { get; set; }

    /// <summary>
    /// 出库主表 ID（选项 TaktSerialOutbounds/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long OutboundId { get; set; }

    /// <summary>
    /// 出库主表 名称（填充字段）
    /// </summary>
    public string? OutboundName { get; set; }

    /// <summary>
    /// 出库单号（冗余字段，便于查询）
    /// </summary>
    public string OutboundCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 出库序列号（租户+公司内唯一）
    /// </summary>
    public string OutboundSerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联入库主表 ID（选项 TaktSerialInbounds/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ReferenceInboundId { get; set; }

    /// <summary>
    /// 关联入库主表 名称（填充字段）
    /// </summary>
    public string? ReferenceInboundName { get; set; }

    /// <summary>
    /// 关联入库单号（选项 TaktSerialInbounds/options；DictValue=InboundCode）
    /// </summary>
    public string ReferenceInboundCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联入库行号（对应 TaktSerialInboundItem.LineNumber）
    /// </summary>
    public int ReferenceInboundLineNumber { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 出库主表
    /// （主表：TaktSerialOutbound）
    /// </summary>
    public TaktSerialOutboundDto? Outbound { get; set; }

}

// ========================================
// SerialOutboundItem 查询 DTO
// ========================================

/// <summary>
/// SerialOutboundItem 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSerialOutboundItemQueryDto : TaktPagedQuery
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
    /// 出库主表 ID（选项 TaktSerialOutbounds/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? OutboundId { get; set; }

    /// <summary>
    /// 出库单号（冗余字段，便于查询）
    /// </summary>
    public string? OutboundCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 出库序列号（租户+公司内唯一）
    /// </summary>
    public string? OutboundSerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联入库主表 ID（选项 TaktSerialInbounds/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ReferenceInboundId { get; set; }

    /// <summary>
    /// 关联入库单号（选项 TaktSerialInbounds/options；DictValue=InboundCode）
    /// </summary>
    public string? ReferenceInboundCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联入库行号（对应 TaktSerialInboundItem.LineNumber）
    /// </summary>
    public int? ReferenceInboundLineNumber { get; set; }

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
// 创建SerialOutboundItem DTO
// ========================================

/// <summary>
/// 创建SerialOutboundItem DTO
/// </summary>
public class TaktSerialOutboundItemCreateDto
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
    /// 出库主表 ID（选项 TaktSerialOutbounds/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long OutboundId { get; set; }

    /// <summary>
    /// 出库单号（冗余字段，便于查询）
    /// </summary>
    public string OutboundCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 出库序列号（租户+公司内唯一）
    /// </summary>
    [Required(ErrorMessage = "出库序列号（租户+公司内唯一）不能为空")]
    public string OutboundSerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联入库主表 ID（选项 TaktSerialInbounds/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ReferenceInboundId { get; set; }

    /// <summary>
    /// 关联入库单号（选项 TaktSerialInbounds/options；DictValue=InboundCode）
    /// </summary>
    public string ReferenceInboundCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联入库行号（对应 TaktSerialInboundItem.LineNumber）
    /// </summary>
    public int ReferenceInboundLineNumber { get; set; } = 0;

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
// 更新SerialOutboundItem DTO
// ========================================

/// <summary>
/// 更新SerialOutboundItem DTO
/// 继承 TaktSerialOutboundItemCreateDto，添加 SerialOutboundItemId 字段
/// </summary>
public class TaktSerialOutboundItemUpdateDto : TaktSerialOutboundItemCreateDto
{
    /// <summary>
    /// SerialOutboundItemID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SerialOutboundItemId { get; set; }

}

// ========================================
// SerialOutboundItem 作废 DTO
// ========================================

/// <summary>
/// SerialOutboundItem 作废/撤销作废 DTO
/// </summary>
public class TaktSerialOutboundItemObsoleteDto
{
    /// <summary>
    /// SerialOutboundItemID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SerialOutboundItemId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SerialOutboundItem 导入模板行 DTO
/// </summary>
public class TaktSerialOutboundItemTemplateDto
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
    /// 出库主表 ID（选项 TaktSerialOutbounds/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? OutboundId { get; set; }

    /// <summary>
    /// 出库单号（冗余字段，便于查询）
    /// </summary>
    public string? OutboundCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 出库序列号（租户+公司内唯一）
    /// </summary>
    public string? OutboundSerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联入库主表 ID（选项 TaktSerialInbounds/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ReferenceInboundId { get; set; }

    /// <summary>
    /// 关联入库单号（选项 TaktSerialInbounds/options；DictValue=InboundCode）
    /// </summary>
    public string? ReferenceInboundCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联入库行号（对应 TaktSerialInboundItem.LineNumber）
    /// </summary>
    public int? ReferenceInboundLineNumber { get; set; }

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
/// SerialOutboundItem 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSerialOutboundItemImportDto
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
    /// 出库主表 ID（选项 TaktSerialOutbounds/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? OutboundId { get; set; }

    /// <summary>
    /// 出库单号（冗余字段，便于查询）
    /// </summary>
    public string? OutboundCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 出库序列号（租户+公司内唯一）
    /// </summary>
    public string? OutboundSerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联入库主表 ID（选项 TaktSerialInbounds/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ReferenceInboundId { get; set; }

    /// <summary>
    /// 关联入库单号（选项 TaktSerialInbounds/options；DictValue=InboundCode）
    /// </summary>
    public string? ReferenceInboundCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联入库行号（对应 TaktSerialInboundItem.LineNumber）
    /// </summary>
    public int? ReferenceInboundLineNumber { get; set; }

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
/// SerialOutboundItem 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSerialOutboundItemExportDto
{
    /// <summary>
    /// SerialOutboundItemID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SerialOutboundItemId { get; set; }

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
    /// 出库主表 ID（选项 TaktSerialOutbounds/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long OutboundId { get; set; }

    /// <summary>
    /// 出库单号（冗余字段，便于查询）
    /// </summary>
    public string OutboundCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 出库序列号（租户+公司内唯一）
    /// </summary>
    public string OutboundSerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联入库主表 ID（选项 TaktSerialInbounds/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ReferenceInboundId { get; set; }

    /// <summary>
    /// 关联入库单号（选项 TaktSerialInbounds/options；DictValue=InboundCode）
    /// </summary>
    public string ReferenceInboundCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联入库行号（对应 TaktSerialInboundItem.LineNumber）
    /// </summary>
    public int ReferenceInboundLineNumber { get; set; } = 0;

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
