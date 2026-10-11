// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Quality.Operation
// 文件名称：TaktIqcOrderDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：IqcOrder 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktIqcOrder 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Quality.Operation;

// ========================================
// IqcOrder 响应 DTO
// ========================================

/// <summary>
/// IQC进货检验单实体
/// 对应前端 TaktIqcOrderDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktIqcOrderDto : TaktCompanyDtoBase
{
    /// <summary>
    /// IqcOrderID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long IqcOrderId { get; set; }

    /// <summary>
    /// 来源单号（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）
    /// </summary>
    public string SourceCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验日期
    /// </summary>
    public DateTime? InspectionDate { get; set; }

    /// <summary>
    /// IQC检验单编码（唯一索引，根据来源单号自动生成）
    /// </summary>
    public string IqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 进货总数
    /// </summary>
    public decimal TotalPurchaseQuantity { get; set; }

    /// <summary>
    /// 总抽样数量（自动计算 = 各明细抽样数量合计）
    /// </summary>
    public int TotalSampleQuantity { get; set; } = 0;

    /// <summary>
    /// 总合格数量（自动计算 = 各明细合格数量合计）
    /// </summary>
    public int TotalQualifiedQuantity { get; set; } = 0;

    /// <summary>
    /// 总不合格数量（自动计算 = 各明细不合格数量合计）
    /// </summary>
    public int TotalUnqualifiedQuantity { get; set; } = 0;

    /// <summary>
    /// 总验退数量（自动计算 = 各明细验退数量合计）
    /// </summary>
    public decimal TotalInspectionReturnQuantity { get; set; }

    /// <summary>
    /// 判定人（人员代码）
    /// </summary>
    public string? JudgeByEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 判定日期
    /// </summary>
    public DateTime? JudgeDate { get; set; }

    /// <summary>
    /// 判定说明
    /// </summary>
    public string? JudgeDescription { get; set; } = string.Empty;

    /// <summary>
    /// 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）
    /// </summary>
    public int JudgeStatus { get; set; } = 0;

    /// <summary>
    /// IQC检验单明细列表（主子表关系）
    /// （子表：TaktIqcOrderItem）
    /// </summary>
    public List<TaktIqcOrderItemDto>? Items { get; set; }

}

// ========================================
// IqcOrder 查询 DTO
// ========================================

/// <summary>
/// IqcOrder 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktIqcOrderQueryDto : TaktPagedQuery
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
    /// 来源单号（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）
    /// </summary>
    public string? SourceCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验日期（范围查询-开始）
    /// </summary>
    public DateTime? InspectionDateStart { get; set; }

    /// <summary>
    /// 检验日期（范围查询-结束）
    /// </summary>
    public DateTime? InspectionDateEnd { get; set; }

    /// <summary>
    /// IQC检验单编码（唯一索引，根据来源单号自动生成）
    /// </summary>
    public string? IqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 进货总数
    /// </summary>
    public decimal? TotalPurchaseQuantity { get; set; }

    /// <summary>
    /// 总抽样数量（自动计算 = 各明细抽样数量合计）
    /// </summary>
    public int? TotalSampleQuantity { get; set; }

    /// <summary>
    /// 总合格数量（自动计算 = 各明细合格数量合计）
    /// </summary>
    public int? TotalQualifiedQuantity { get; set; }

    /// <summary>
    /// 总不合格数量（自动计算 = 各明细不合格数量合计）
    /// </summary>
    public int? TotalUnqualifiedQuantity { get; set; }

    /// <summary>
    /// 总验退数量（自动计算 = 各明细验退数量合计）
    /// </summary>
    public decimal? TotalInspectionReturnQuantity { get; set; }

    /// <summary>
    /// 判定人（人员代码）
    /// </summary>
    public string? JudgeByEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 判定日期（范围查询-开始）
    /// </summary>
    public DateTime? JudgeDateStart { get; set; }

    /// <summary>
    /// 判定日期（范围查询-结束）
    /// </summary>
    public DateTime? JudgeDateEnd { get; set; }

    /// <summary>
    /// 判定说明
    /// </summary>
    public string? JudgeDescription { get; set; } = string.Empty;

    /// <summary>
    /// 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）
    /// </summary>
    public int? JudgeStatus { get; set; }

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
// 创建IqcOrder DTO
// ========================================

/// <summary>
/// 创建IqcOrder DTO
/// </summary>
public class TaktIqcOrderCreateDto
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
    /// 来源单号（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）
    /// </summary>
    [Required(ErrorMessage = "来源单号（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）不能为空")]
    public string SourceCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验日期
    /// </summary>
    public DateTime? InspectionDate { get; set; }

    /// <summary>
    /// IQC检验单编码（唯一索引，根据来源单号自动生成）
    /// </summary>
    [Required(ErrorMessage = "IQC检验单编码（唯一索引，根据来源单号自动生成）不能为空")]
    public string IqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    [Required(ErrorMessage = "供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）不能为空")]
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 进货总数
    /// </summary>
    public decimal TotalPurchaseQuantity { get; set; }

    /// <summary>
    /// 总抽样数量（自动计算 = 各明细抽样数量合计）
    /// </summary>
    public int TotalSampleQuantity { get; set; } = 0;

    /// <summary>
    /// 总合格数量（自动计算 = 各明细合格数量合计）
    /// </summary>
    public int TotalQualifiedQuantity { get; set; } = 0;

    /// <summary>
    /// 总不合格数量（自动计算 = 各明细不合格数量合计）
    /// </summary>
    public int TotalUnqualifiedQuantity { get; set; } = 0;

    /// <summary>
    /// 总验退数量（自动计算 = 各明细验退数量合计）
    /// </summary>
    public decimal TotalInspectionReturnQuantity { get; set; }

    /// <summary>
    /// 判定人（人员代码）
    /// </summary>
    public string? JudgeByEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 判定日期
    /// </summary>
    public DateTime? JudgeDate { get; set; }

    /// <summary>
    /// 判定说明
    /// </summary>
    public string? JudgeDescription { get; set; } = string.Empty;

    /// <summary>
    /// 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）
    /// </summary>
    public int JudgeStatus { get; set; } = 0;

    /// <summary>
    /// IQC检验单明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktIqcOrderItemCreateDto>? Items { get; set; }

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
// 更新IqcOrder DTO
// ========================================

/// <summary>
/// 更新IqcOrder DTO
/// 继承 TaktIqcOrderCreateDto，添加 IqcOrderId 字段
/// </summary>
public class TaktIqcOrderUpdateDto : TaktIqcOrderCreateDto
{
    /// <summary>
    /// IqcOrderID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long IqcOrderId { get; set; }

    /// <summary>
    /// IQC检验单明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public new List<TaktIqcOrderItemUpdateDto>? Items { get; set; }

}

// ========================================
// IqcOrder 状态 DTO
// ========================================

/// <summary>
/// IqcOrder 状态更新 DTO
/// </summary>
public class TaktIqcOrderStatusDto
{
    /// <summary>
    /// IqcOrderID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long IqcOrderId { get; set; }

    /// <summary>
    /// 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）
    /// </summary>
    [Required(ErrorMessage = "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）不能为空")]
    public int JudgeStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// IqcOrder 导入模板行 DTO
/// </summary>
public class TaktIqcOrderTemplateDto
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
    /// 来源单号（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）
    /// </summary>
    public string? SourceCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验日期
    /// </summary>
    public DateTime? InspectionDate { get; set; }

    /// <summary>
    /// IQC检验单编码（唯一索引，根据来源单号自动生成）
    /// </summary>
    public string? IqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 进货总数
    /// </summary>
    public decimal? TotalPurchaseQuantity { get; set; }

    /// <summary>
    /// 总抽样数量（自动计算 = 各明细抽样数量合计）
    /// </summary>
    public int? TotalSampleQuantity { get; set; }

    /// <summary>
    /// 总合格数量（自动计算 = 各明细合格数量合计）
    /// </summary>
    public int? TotalQualifiedQuantity { get; set; }

    /// <summary>
    /// 总不合格数量（自动计算 = 各明细不合格数量合计）
    /// </summary>
    public int? TotalUnqualifiedQuantity { get; set; }

    /// <summary>
    /// 总验退数量（自动计算 = 各明细验退数量合计）
    /// </summary>
    public decimal? TotalInspectionReturnQuantity { get; set; }

    /// <summary>
    /// 判定人（人员代码）
    /// </summary>
    public string? JudgeByEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 判定日期
    /// </summary>
    public DateTime? JudgeDate { get; set; }

    /// <summary>
    /// 判定说明
    /// </summary>
    public string? JudgeDescription { get; set; } = string.Empty;

    /// <summary>
    /// 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）
    /// </summary>
    public int? JudgeStatus { get; set; }

    /// <summary>
    /// IQC检验单明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktIqcOrderItemCreateDto>? Items { get; set; }

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
/// IqcOrder 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktIqcOrderImportDto
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
    /// 来源单号（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）
    /// </summary>
    public string? SourceCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验日期
    /// </summary>
    public DateTime? InspectionDate { get; set; }

    /// <summary>
    /// IQC检验单编码（唯一索引，根据来源单号自动生成）
    /// </summary>
    public string? IqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 进货总数
    /// </summary>
    public decimal? TotalPurchaseQuantity { get; set; }

    /// <summary>
    /// 总抽样数量（自动计算 = 各明细抽样数量合计）
    /// </summary>
    public int? TotalSampleQuantity { get; set; }

    /// <summary>
    /// 总合格数量（自动计算 = 各明细合格数量合计）
    /// </summary>
    public int? TotalQualifiedQuantity { get; set; }

    /// <summary>
    /// 总不合格数量（自动计算 = 各明细不合格数量合计）
    /// </summary>
    public int? TotalUnqualifiedQuantity { get; set; }

    /// <summary>
    /// 总验退数量（自动计算 = 各明细验退数量合计）
    /// </summary>
    public decimal? TotalInspectionReturnQuantity { get; set; }

    /// <summary>
    /// 判定人（人员代码）
    /// </summary>
    public string? JudgeByEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 判定日期
    /// </summary>
    public DateTime? JudgeDate { get; set; }

    /// <summary>
    /// 判定说明
    /// </summary>
    public string? JudgeDescription { get; set; } = string.Empty;

    /// <summary>
    /// 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）
    /// </summary>
    public int? JudgeStatus { get; set; }

    /// <summary>
    /// IQC检验单明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktIqcOrderItemCreateDto>? Items { get; set; }

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
/// IqcOrder 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktIqcOrderExportDto
{
    /// <summary>
    /// IqcOrderID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long IqcOrderId { get; set; }

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
    /// 来源单号（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）
    /// </summary>
    public string SourceCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验日期
    /// </summary>
    public DateTime? InspectionDate { get; set; }

    /// <summary>
    /// IQC检验单编码（唯一索引，根据来源单号自动生成）
    /// </summary>
    public string IqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 进货总数
    /// </summary>
    public decimal TotalPurchaseQuantity { get; set; }

    /// <summary>
    /// 总抽样数量（自动计算 = 各明细抽样数量合计）
    /// </summary>
    public int TotalSampleQuantity { get; set; } = 0;

    /// <summary>
    /// 总合格数量（自动计算 = 各明细合格数量合计）
    /// </summary>
    public int TotalQualifiedQuantity { get; set; } = 0;

    /// <summary>
    /// 总不合格数量（自动计算 = 各明细不合格数量合计）
    /// </summary>
    public int TotalUnqualifiedQuantity { get; set; } = 0;

    /// <summary>
    /// 总验退数量（自动计算 = 各明细验退数量合计）
    /// </summary>
    public decimal TotalInspectionReturnQuantity { get; set; }

    /// <summary>
    /// 判定人（人员代码）
    /// </summary>
    public string? JudgeByEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 判定日期
    /// </summary>
    public DateTime? JudgeDate { get; set; }

    /// <summary>
    /// 判定说明
    /// </summary>
    public string? JudgeDescription { get; set; } = string.Empty;

    /// <summary>
    /// 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）
    /// </summary>
    public int JudgeStatus { get; set; } = 0;

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
