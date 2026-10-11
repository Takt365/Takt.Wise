// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Materials
// 文件名称：TaktMaterialDocumentDtos.cs
// 创建时间：2026-09-20
// 创建人：Takt365(Auto Generated)
// 功能描述：MaterialDocument 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktMaterialDocument 生成，请按需审阅）
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
// MaterialDocument 响应 DTO
// ========================================

/// <summary>
/// Takt物料凭证主表实体（公司级）
/// 对应前端 TaktMaterialDocumentDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktMaterialDocumentDto : TaktCompanyDtoBase
{
    /// <summary>
    /// MaterialDocumentID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialDocumentId { get; set; }

    /// <summary>
    /// 物料凭证
    /// </summary>
    public string MaterialDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料凭证的年份
    /// </summary>
    public string MaterialDocumentYear { get; set; } = string.Empty;

    /// <summary>
    /// 交易/事件类型（字典 logistics_material_document_transaction_event_type）
    /// </summary>
    public string? TransactionEventType { get; set; } = string.Empty;

    /// <summary>
    /// 凭证类型（字典 logistics_material_document_type）
    /// </summary>
    public string? DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// 凭证类型重新评估
    /// </summary>
    public string? RevaluationType { get; set; } = string.Empty;

    /// <summary>
    /// 凭证日期
    /// </summary>
    public DateTime DocumentDate { get; set; }

    /// <summary>
    /// 过帐日期
    /// </summary>
    public DateTime PostingDate { get; set; }

    /// <summary>
    /// 参照（最长 16，故 Length=16）
    /// </summary>
    public string? ReferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 凭证抬头文本（最长 25，故 Length=25）
    /// </summary>
    public string? HeaderText { get; set; } = string.Empty;

    /// <summary>
    /// 提货单（最长 16，故 Length=16）
    /// </summary>
    public string? BillOfLadingCode { get; set; } = string.Empty;

    /// <summary>
    /// 交货单
    /// </summary>
    public string? DeliveryCode { get; set; } = string.Empty;

    /// <summary>
    /// 事务代码
    /// </summary>
    public string? TransactionCode { get; set; } = string.Empty;

    /// <summary>
    /// 过账人（当前登录用户对应的 EmployeeCode）
    /// </summary>
    public string? PostedBy { get; set; } = string.Empty;

    /// <summary>
    /// 物料凭证行项目列表（主子表关系）
    /// （子表：TaktMaterialDocumentItem）
    /// </summary>
    public List<TaktMaterialDocumentItemDto>? Items { get; set; }

}

// ========================================
// MaterialDocument 查询 DTO
// ========================================

/// <summary>
/// MaterialDocument 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktMaterialDocumentQueryDto : TaktPagedQuery
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
    /// 物料凭证
    /// </summary>
    public string? MaterialDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料凭证的年份
    /// </summary>
    public string? MaterialDocumentYear { get; set; } = string.Empty;

    /// <summary>
    /// 交易/事件类型（字典 logistics_material_document_transaction_event_type）
    /// </summary>
    public string? TransactionEventType { get; set; } = string.Empty;

    /// <summary>
    /// 凭证类型（字典 logistics_material_document_type）
    /// </summary>
    public string? DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// 凭证类型重新评估
    /// </summary>
    public string? RevaluationType { get; set; } = string.Empty;

    /// <summary>
    /// 凭证日期（范围查询-开始）
    /// </summary>
    public DateTime? DocumentDateStart { get; set; }

    /// <summary>
    /// 凭证日期（范围查询-结束）
    /// </summary>
    public DateTime? DocumentDateEnd { get; set; }

    /// <summary>
    /// 过帐日期（范围查询-开始）
    /// </summary>
    public DateTime? PostingDateStart { get; set; }

    /// <summary>
    /// 过帐日期（范围查询-结束）
    /// </summary>
    public DateTime? PostingDateEnd { get; set; }

    /// <summary>
    /// 参照（最长 16，故 Length=16）
    /// </summary>
    public string? ReferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 凭证抬头文本（最长 25，故 Length=25）
    /// </summary>
    public string? HeaderText { get; set; } = string.Empty;

    /// <summary>
    /// 提货单（最长 16，故 Length=16）
    /// </summary>
    public string? BillOfLadingCode { get; set; } = string.Empty;

    /// <summary>
    /// 交货单
    /// </summary>
    public string? DeliveryCode { get; set; } = string.Empty;

    /// <summary>
    /// 事务代码
    /// </summary>
    public string? TransactionCode { get; set; } = string.Empty;

    /// <summary>
    /// 过账人（当前登录用户对应的 EmployeeCode）
    /// </summary>
    public string? PostedBy { get; set; } = string.Empty;

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
// 创建MaterialDocument DTO
// ========================================

/// <summary>
/// 创建MaterialDocument DTO
/// </summary>
public class TaktMaterialDocumentCreateDto
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
    /// 物料凭证
    /// </summary>
    [Required(ErrorMessage = "物料凭证不能为空")]
    public string MaterialDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料凭证的年份
    /// </summary>
    [Required(ErrorMessage = "物料凭证的年份不能为空")]
    public string MaterialDocumentYear { get; set; } = string.Empty;

    /// <summary>
    /// 交易/事件类型（字典 logistics_material_document_transaction_event_type）
    /// </summary>
    public string? TransactionEventType { get; set; } = string.Empty;

    /// <summary>
    /// 凭证类型（字典 logistics_material_document_type）
    /// </summary>
    public string? DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// 凭证类型重新评估
    /// </summary>
    public string? RevaluationType { get; set; } = string.Empty;

    /// <summary>
    /// 凭证日期
    /// </summary>
    public DateTime DocumentDate { get; set; }

    /// <summary>
    /// 过帐日期
    /// </summary>
    public DateTime PostingDate { get; set; }

    /// <summary>
    /// 参照（最长 16，故 Length=16）
    /// </summary>
    public string? ReferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 凭证抬头文本（最长 25，故 Length=25）
    /// </summary>
    public string? HeaderText { get; set; } = string.Empty;

    /// <summary>
    /// 提货单（最长 16，故 Length=16）
    /// </summary>
    public string? BillOfLadingCode { get; set; } = string.Empty;

    /// <summary>
    /// 交货单
    /// </summary>
    public string? DeliveryCode { get; set; } = string.Empty;

    /// <summary>
    /// 事务代码
    /// </summary>
    public string? TransactionCode { get; set; } = string.Empty;

    /// <summary>
    /// 过账人（当前登录用户对应的 EmployeeCode）
    /// </summary>
    public string? PostedBy { get; set; } = string.Empty;

    /// <summary>
    /// 物料凭证行项目列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktMaterialDocumentItemCreateDto>? Items { get; set; }

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
// 更新MaterialDocument DTO
// ========================================

/// <summary>
/// 更新MaterialDocument DTO
/// 继承 TaktMaterialDocumentCreateDto，添加 MaterialDocumentId 字段
/// </summary>
public class TaktMaterialDocumentUpdateDto : TaktMaterialDocumentCreateDto
{
    /// <summary>
    /// MaterialDocumentID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialDocumentId { get; set; }

    /// <summary>
    /// 物料凭证行项目列表（主子表关系）（子表，级联保存）
    /// </summary>
    public new List<TaktMaterialDocumentItemUpdateDto>? Items { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// MaterialDocument 导入模板行 DTO
/// </summary>
public class TaktMaterialDocumentTemplateDto
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
    /// 物料凭证
    /// </summary>
    public string? MaterialDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料凭证的年份
    /// </summary>
    public string? MaterialDocumentYear { get; set; } = string.Empty;

    /// <summary>
    /// 交易/事件类型（字典 logistics_material_document_transaction_event_type）
    /// </summary>
    public string? TransactionEventType { get; set; } = string.Empty;

    /// <summary>
    /// 凭证类型（字典 logistics_material_document_type）
    /// </summary>
    public string? DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// 凭证类型重新评估
    /// </summary>
    public string? RevaluationType { get; set; } = string.Empty;

    /// <summary>
    /// 凭证日期
    /// </summary>
    public DateTime? DocumentDate { get; set; }

    /// <summary>
    /// 过帐日期
    /// </summary>
    public DateTime? PostingDate { get; set; }

    /// <summary>
    /// 参照（最长 16，故 Length=16）
    /// </summary>
    public string? ReferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 凭证抬头文本（最长 25，故 Length=25）
    /// </summary>
    public string? HeaderText { get; set; } = string.Empty;

    /// <summary>
    /// 提货单（最长 16，故 Length=16）
    /// </summary>
    public string? BillOfLadingCode { get; set; } = string.Empty;

    /// <summary>
    /// 交货单
    /// </summary>
    public string? DeliveryCode { get; set; } = string.Empty;

    /// <summary>
    /// 事务代码
    /// </summary>
    public string? TransactionCode { get; set; } = string.Empty;

    /// <summary>
    /// 过账人（当前登录用户对应的 EmployeeCode）
    /// </summary>
    public string? PostedBy { get; set; } = string.Empty;

    /// <summary>
    /// 物料凭证行项目列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktMaterialDocumentItemCreateDto>? Items { get; set; }

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
/// MaterialDocument 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktMaterialDocumentImportDto
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
    /// 物料凭证
    /// </summary>
    public string? MaterialDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料凭证的年份
    /// </summary>
    public string? MaterialDocumentYear { get; set; } = string.Empty;

    /// <summary>
    /// 交易/事件类型（字典 logistics_material_document_transaction_event_type）
    /// </summary>
    public string? TransactionEventType { get; set; } = string.Empty;

    /// <summary>
    /// 凭证类型（字典 logistics_material_document_type）
    /// </summary>
    public string? DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// 凭证类型重新评估
    /// </summary>
    public string? RevaluationType { get; set; } = string.Empty;

    /// <summary>
    /// 凭证日期
    /// </summary>
    public DateTime? DocumentDate { get; set; }

    /// <summary>
    /// 过帐日期
    /// </summary>
    public DateTime? PostingDate { get; set; }

    /// <summary>
    /// 参照（最长 16，故 Length=16）
    /// </summary>
    public string? ReferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 凭证抬头文本（最长 25，故 Length=25）
    /// </summary>
    public string? HeaderText { get; set; } = string.Empty;

    /// <summary>
    /// 提货单（最长 16，故 Length=16）
    /// </summary>
    public string? BillOfLadingCode { get; set; } = string.Empty;

    /// <summary>
    /// 交货单
    /// </summary>
    public string? DeliveryCode { get; set; } = string.Empty;

    /// <summary>
    /// 事务代码
    /// </summary>
    public string? TransactionCode { get; set; } = string.Empty;

    /// <summary>
    /// 过账人（当前登录用户对应的 EmployeeCode）
    /// </summary>
    public string? PostedBy { get; set; } = string.Empty;

    /// <summary>
    /// 物料凭证行项目列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktMaterialDocumentItemCreateDto>? Items { get; set; }

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
/// MaterialDocument 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktMaterialDocumentExportDto
{
    /// <summary>
    /// MaterialDocumentID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialDocumentId { get; set; }

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
    /// 物料凭证
    /// </summary>
    public string MaterialDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料凭证的年份
    /// </summary>
    public string MaterialDocumentYear { get; set; } = string.Empty;

    /// <summary>
    /// 交易/事件类型（字典 logistics_material_document_transaction_event_type）
    /// </summary>
    public string? TransactionEventType { get; set; } = string.Empty;

    /// <summary>
    /// 凭证类型（字典 logistics_material_document_type）
    /// </summary>
    public string? DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// 凭证类型重新评估
    /// </summary>
    public string? RevaluationType { get; set; } = string.Empty;

    /// <summary>
    /// 凭证日期
    /// </summary>
    public DateTime DocumentDate { get; set; }

    /// <summary>
    /// 过帐日期
    /// </summary>
    public DateTime PostingDate { get; set; }

    /// <summary>
    /// 参照（最长 16，故 Length=16）
    /// </summary>
    public string? ReferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 凭证抬头文本（最长 25，故 Length=25）
    /// </summary>
    public string? HeaderText { get; set; } = string.Empty;

    /// <summary>
    /// 提货单（最长 16，故 Length=16）
    /// </summary>
    public string? BillOfLadingCode { get; set; } = string.Empty;

    /// <summary>
    /// 交货单
    /// </summary>
    public string? DeliveryCode { get; set; } = string.Empty;

    /// <summary>
    /// 事务代码
    /// </summary>
    public string? TransactionCode { get; set; } = string.Empty;

    /// <summary>
    /// 过账人（当前登录用户对应的 EmployeeCode）
    /// </summary>
    public string? PostedBy { get; set; } = string.Empty;

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
