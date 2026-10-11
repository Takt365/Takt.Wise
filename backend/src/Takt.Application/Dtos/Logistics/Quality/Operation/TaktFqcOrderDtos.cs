// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Quality.Operation
// 文件名称：TaktFqcOrderDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：FqcOrder 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktFqcOrder 生成，请按需审阅）
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
// FqcOrder 响应 DTO
// ========================================

/// <summary>
/// FQC出货检验单实体
/// 对应前端 TaktFqcOrderDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktFqcOrderDto : TaktCompanyDtoBase
{
    /// <summary>
    /// FqcOrderID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FqcOrderId { get; set; }

    /// <summary>
    /// 来源单号（选项 TaktSalesOrders/options；DictValue=SalesOrderCode）
    /// </summary>
    public string SourceCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验日期
    /// </summary>
    public DateTime? InspectionDate { get; set; }

    /// <summary>
    /// FQC检验单编码（唯一索引，根据来源单号自动生成）
    /// </summary>
    public string FqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 总入库数
    /// </summary>
    public decimal TotalWarehouseQuantity { get; set; }

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
    /// 判定人（选项 TaktEmployees/options；DictValue=EmployeeCode）
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
    /// 判定状态（字典 logistics_quality_judge_status）
    /// </summary>
    public int JudgeStatus { get; set; } = 0;

    /// <summary>
    /// FQC检验单明细列表（主子表关系）
    /// （子表：TaktFqcOrderItem）
    /// </summary>
    public List<TaktFqcOrderItemDto>? Items { get; set; }

}

// ========================================
// FqcOrder 查询 DTO
// ========================================

/// <summary>
/// FqcOrder 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktFqcOrderQueryDto : TaktPagedQuery
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
    /// 来源单号（选项 TaktSalesOrders/options；DictValue=SalesOrderCode）
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
    /// FQC检验单编码（唯一索引，根据来源单号自动生成）
    /// </summary>
    public string? FqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 总入库数
    /// </summary>
    public decimal? TotalWarehouseQuantity { get; set; }

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
    /// 判定人（选项 TaktEmployees/options；DictValue=EmployeeCode）
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
    /// 判定状态（字典 logistics_quality_judge_status）
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
// 创建FqcOrder DTO
// ========================================

/// <summary>
/// 创建FqcOrder DTO
/// </summary>
public class TaktFqcOrderCreateDto
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
    /// 来源单号（选项 TaktSalesOrders/options；DictValue=SalesOrderCode）
    /// </summary>
    [Required(ErrorMessage = "来源单号（选项 TaktSalesOrders/options；DictValue=SalesOrderCode）不能为空")]
    public string SourceCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验日期
    /// </summary>
    public DateTime? InspectionDate { get; set; }

    /// <summary>
    /// FQC检验单编码（唯一索引，根据来源单号自动生成）
    /// </summary>
    [Required(ErrorMessage = "FQC检验单编码（唯一索引，根据来源单号自动生成）不能为空")]
    public string FqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 总入库数
    /// </summary>
    public decimal TotalWarehouseQuantity { get; set; }

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
    /// 判定人（选项 TaktEmployees/options；DictValue=EmployeeCode）
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
    /// 判定状态（字典 logistics_quality_judge_status）
    /// </summary>
    public int JudgeStatus { get; set; } = 0;

    /// <summary>
    /// FQC检验单明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktFqcOrderItemCreateDto>? Items { get; set; }

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
// 更新FqcOrder DTO
// ========================================

/// <summary>
/// 更新FqcOrder DTO
/// 继承 TaktFqcOrderCreateDto，添加 FqcOrderId 字段
/// </summary>
public class TaktFqcOrderUpdateDto : TaktFqcOrderCreateDto
{
    /// <summary>
    /// FqcOrderID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FqcOrderId { get; set; }

    /// <summary>
    /// FQC检验单明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public new List<TaktFqcOrderItemUpdateDto>? Items { get; set; }

}

// ========================================
// FqcOrder 状态 DTO
// ========================================

/// <summary>
/// FqcOrder 状态更新 DTO
/// </summary>
public class TaktFqcOrderStatusDto
{
    /// <summary>
    /// FqcOrderID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FqcOrderId { get; set; }

    /// <summary>
    /// 判定状态（字典 logistics_quality_judge_status）
    /// </summary>
    [Required(ErrorMessage = "判定状态（字典 logistics_quality_judge_status）不能为空")]
    public int JudgeStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// FqcOrder 导入模板行 DTO
/// </summary>
public class TaktFqcOrderTemplateDto
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
    /// 来源单号（选项 TaktSalesOrders/options；DictValue=SalesOrderCode）
    /// </summary>
    public string? SourceCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验日期
    /// </summary>
    public DateTime? InspectionDate { get; set; }

    /// <summary>
    /// FQC检验单编码（唯一索引，根据来源单号自动生成）
    /// </summary>
    public string? FqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 总入库数
    /// </summary>
    public decimal? TotalWarehouseQuantity { get; set; }

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
    /// 判定人（选项 TaktEmployees/options；DictValue=EmployeeCode）
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
    /// 判定状态（字典 logistics_quality_judge_status）
    /// </summary>
    public int? JudgeStatus { get; set; }

    /// <summary>
    /// FQC检验单明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktFqcOrderItemCreateDto>? Items { get; set; }

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
/// FqcOrder 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktFqcOrderImportDto
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
    /// 来源单号（选项 TaktSalesOrders/options；DictValue=SalesOrderCode）
    /// </summary>
    public string? SourceCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验日期
    /// </summary>
    public DateTime? InspectionDate { get; set; }

    /// <summary>
    /// FQC检验单编码（唯一索引，根据来源单号自动生成）
    /// </summary>
    public string? FqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 总入库数
    /// </summary>
    public decimal? TotalWarehouseQuantity { get; set; }

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
    /// 判定人（选项 TaktEmployees/options；DictValue=EmployeeCode）
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
    /// 判定状态（字典 logistics_quality_judge_status）
    /// </summary>
    public int? JudgeStatus { get; set; }

    /// <summary>
    /// FQC检验单明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktFqcOrderItemCreateDto>? Items { get; set; }

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
/// FqcOrder 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktFqcOrderExportDto
{
    /// <summary>
    /// FqcOrderID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FqcOrderId { get; set; }

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
    /// 来源单号（选项 TaktSalesOrders/options；DictValue=SalesOrderCode）
    /// </summary>
    public string SourceCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验日期
    /// </summary>
    public DateTime? InspectionDate { get; set; }

    /// <summary>
    /// FQC检验单编码（唯一索引，根据来源单号自动生成）
    /// </summary>
    public string FqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 总入库数
    /// </summary>
    public decimal TotalWarehouseQuantity { get; set; }

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
    /// 判定人（选项 TaktEmployees/options；DictValue=EmployeeCode）
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
    /// 判定状态（字典 logistics_quality_judge_status）
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
