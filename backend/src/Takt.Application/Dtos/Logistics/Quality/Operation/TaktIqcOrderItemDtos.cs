// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Quality.Operation
// 文件名称：TaktIqcOrderItemDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：IqcOrderItem 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktIqcOrderItem 生成，请按需审阅）
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
// IqcOrderItem 响应 DTO
// ========================================

/// <summary>
/// IQC进货检验单明细实体
/// 对应前端 TaktIqcOrderItemDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktIqcOrderItemDto : TaktCompanyDtoBase
{
    /// <summary>
    /// IqcOrderItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long IqcOrderItemId { get; set; }

    /// <summary>
    /// IQC检验单 ID（选项 TaktIqcOrders/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long IqcOrderId { get; set; }

    /// <summary>
    /// IQC检验单 名称（填充字段）
    /// </summary>
    public string? IqcOrderName { get; set; }

    /// <summary>
    /// IQC检验单编码（冗余字段，便于查询）
    /// </summary>
    public string IqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 批次号
    /// </summary>
    public string? BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 进货数量
    /// </summary>
    public decimal PurchaseQuantity { get; set; }

    /// <summary>
    /// 检验标准编码（选项 TaktInspectionStandards/options；DictValue=StandardCode）
    /// </summary>
    public string StandardCode { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案编码（选项 TaktSamplingSchemes/options；DictValue=SamplingSchemeCode）
    /// </summary>
    public string SamplingSchemeCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验方式（0=免检，1=减量，2=正常，3=加严，4=全检）
    /// </summary>
    public int InspectionMethod { get; set; } = 0;

    /// <summary>
    /// 抽样数量
    /// </summary>
    public int SampleQuantity { get; set; } = 0;

    /// <summary>
    /// 合格数量
    /// </summary>
    public int QualifiedQuantity { get; set; } = 0;

    /// <summary>
    /// 不合格数量
    /// </summary>
    public int UnqualifiedQuantity { get; set; } = 0;

    /// <summary>
    /// 验退数量
    /// </summary>
    public decimal InspectionReturnQuantity { get; set; }

    /// <summary>
    /// 抽检序列号
    /// </summary>
    public string? SampleSerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验说明
    /// </summary>
    public string? InspectionDescription { get; set; } = string.Empty;

    /// <summary>
    /// 检验员（人员代码）
    /// </summary>
    public string InspectorName { get; set; } = string.Empty;

    /// <summary>
    /// 检验日期
    /// </summary>
    public DateTime InspectionDate { get; set; }

    /// <summary>
    /// 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）
    /// </summary>
    public int JudgeStatus { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// IQC检验单（主表）
    /// （主表：TaktIqcOrder）
    /// </summary>
    public TaktIqcOrderDto? Order { get; set; }

    /// <summary>
    /// 不良处理记录列表（主子表关系）
    /// （子表：TaktIqcDefectHandling）
    /// </summary>
    public List<TaktIqcDefectHandlingDto>? DefectHandlings { get; set; }

}

// ========================================
// IqcOrderItem 查询 DTO
// ========================================

/// <summary>
/// IqcOrderItem 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktIqcOrderItemQueryDto : TaktPagedQuery
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
    /// IQC检验单 ID（选项 TaktIqcOrders/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? IqcOrderId { get; set; }

    /// <summary>
    /// IQC检验单编码（冗余字段，便于查询）
    /// </summary>
    public string? IqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 批次号
    /// </summary>
    public string? BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 进货数量
    /// </summary>
    public decimal? PurchaseQuantity { get; set; }

    /// <summary>
    /// 检验标准编码（选项 TaktInspectionStandards/options；DictValue=StandardCode）
    /// </summary>
    public string? StandardCode { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案编码（选项 TaktSamplingSchemes/options；DictValue=SamplingSchemeCode）
    /// </summary>
    public string? SamplingSchemeCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验方式（0=免检，1=减量，2=正常，3=加严，4=全检）
    /// </summary>
    public int? InspectionMethod { get; set; }

    /// <summary>
    /// 抽样数量
    /// </summary>
    public int? SampleQuantity { get; set; }

    /// <summary>
    /// 合格数量
    /// </summary>
    public int? QualifiedQuantity { get; set; }

    /// <summary>
    /// 不合格数量
    /// </summary>
    public int? UnqualifiedQuantity { get; set; }

    /// <summary>
    /// 验退数量
    /// </summary>
    public decimal? InspectionReturnQuantity { get; set; }

    /// <summary>
    /// 抽检序列号
    /// </summary>
    public string? SampleSerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验说明
    /// </summary>
    public string? InspectionDescription { get; set; } = string.Empty;

    /// <summary>
    /// 检验员（人员代码）
    /// </summary>
    public string? InspectorName { get; set; } = string.Empty;

    /// <summary>
    /// 检验日期（范围查询-开始）
    /// </summary>
    public DateTime? InspectionDateStart { get; set; }

    /// <summary>
    /// 检验日期（范围查询-结束）
    /// </summary>
    public DateTime? InspectionDateEnd { get; set; }

    /// <summary>
    /// 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）
    /// </summary>
    public int? JudgeStatus { get; set; }

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
// 创建IqcOrderItem DTO
// ========================================

/// <summary>
/// 创建IqcOrderItem DTO
/// </summary>
public class TaktIqcOrderItemCreateDto
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
    /// IQC检验单 ID（选项 TaktIqcOrders/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long IqcOrderId { get; set; }

    /// <summary>
    /// IQC检验单编码（冗余字段，便于查询）
    /// </summary>
    public string IqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    [Required(ErrorMessage = "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）不能为空")]
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 批次号
    /// </summary>
    public string? BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 进货数量
    /// </summary>
    public decimal PurchaseQuantity { get; set; }

    /// <summary>
    /// 检验标准编码（选项 TaktInspectionStandards/options；DictValue=StandardCode）
    /// </summary>
    [Required(ErrorMessage = "检验标准编码（选项 TaktInspectionStandards/options；DictValue=StandardCode）不能为空")]
    public string StandardCode { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案编码（选项 TaktSamplingSchemes/options；DictValue=SamplingSchemeCode）
    /// </summary>
    [Required(ErrorMessage = "抽样方案编码（选项 TaktSamplingSchemes/options；DictValue=SamplingSchemeCode）不能为空")]
    public string SamplingSchemeCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验方式（0=免检，1=减量，2=正常，3=加严，4=全检）
    /// </summary>
    public int InspectionMethod { get; set; } = 0;

    /// <summary>
    /// 抽样数量
    /// </summary>
    public int SampleQuantity { get; set; } = 0;

    /// <summary>
    /// 合格数量
    /// </summary>
    public int QualifiedQuantity { get; set; } = 0;

    /// <summary>
    /// 不合格数量
    /// </summary>
    public int UnqualifiedQuantity { get; set; } = 0;

    /// <summary>
    /// 验退数量
    /// </summary>
    public decimal InspectionReturnQuantity { get; set; }

    /// <summary>
    /// 抽检序列号
    /// </summary>
    public string? SampleSerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验说明
    /// </summary>
    public string? InspectionDescription { get; set; } = string.Empty;

    /// <summary>
    /// 检验员（人员代码）
    /// </summary>
    [Required(ErrorMessage = "检验员（人员代码）不能为空")]
    public string InspectorName { get; set; } = string.Empty;

    /// <summary>
    /// 检验日期
    /// </summary>
    public DateTime InspectionDate { get; set; }

    /// <summary>
    /// 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）
    /// </summary>
    public int JudgeStatus { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 不良处理记录列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktIqcDefectHandlingCreateDto>? DefectHandlings { get; set; }

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
// 更新IqcOrderItem DTO
// ========================================

/// <summary>
/// 更新IqcOrderItem DTO
/// 继承 TaktIqcOrderItemCreateDto，添加 IqcOrderItemId 字段
/// </summary>
public class TaktIqcOrderItemUpdateDto : TaktIqcOrderItemCreateDto
{
    /// <summary>
    /// IqcOrderItemID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long IqcOrderItemId { get; set; }

    /// <summary>
    /// 不良处理记录列表（主子表关系）（子表，级联保存）
    /// </summary>
    public new List<TaktIqcDefectHandlingUpdateDto>? DefectHandlings { get; set; }

}

// ========================================
// IqcOrderItem 状态 DTO
// ========================================

/// <summary>
/// IqcOrderItem 状态更新 DTO
/// </summary>
public class TaktIqcOrderItemStatusDto
{
    /// <summary>
    /// IqcOrderItemID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long IqcOrderItemId { get; set; }

    /// <summary>
    /// 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）
    /// </summary>
    [Required(ErrorMessage = "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）不能为空")]
    public int JudgeStatus { get; set; } = 0;
}

// ========================================
// IqcOrderItem 作废 DTO
// ========================================

/// <summary>
/// IqcOrderItem 作废/撤销作废 DTO
/// </summary>
public class TaktIqcOrderItemObsoleteDto
{
    /// <summary>
    /// IqcOrderItemID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long IqcOrderItemId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// IqcOrderItem 导入模板行 DTO
/// </summary>
public class TaktIqcOrderItemTemplateDto
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
    /// IQC检验单 ID（选项 TaktIqcOrders/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? IqcOrderId { get; set; }

    /// <summary>
    /// IQC检验单编码（冗余字段，便于查询）
    /// </summary>
    public string? IqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 批次号
    /// </summary>
    public string? BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 进货数量
    /// </summary>
    public decimal? PurchaseQuantity { get; set; }

    /// <summary>
    /// 检验标准编码（选项 TaktInspectionStandards/options；DictValue=StandardCode）
    /// </summary>
    public string? StandardCode { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案编码（选项 TaktSamplingSchemes/options；DictValue=SamplingSchemeCode）
    /// </summary>
    public string? SamplingSchemeCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验方式（0=免检，1=减量，2=正常，3=加严，4=全检）
    /// </summary>
    public int? InspectionMethod { get; set; }

    /// <summary>
    /// 抽样数量
    /// </summary>
    public int? SampleQuantity { get; set; }

    /// <summary>
    /// 合格数量
    /// </summary>
    public int? QualifiedQuantity { get; set; }

    /// <summary>
    /// 不合格数量
    /// </summary>
    public int? UnqualifiedQuantity { get; set; }

    /// <summary>
    /// 验退数量
    /// </summary>
    public decimal? InspectionReturnQuantity { get; set; }

    /// <summary>
    /// 抽检序列号
    /// </summary>
    public string? SampleSerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验说明
    /// </summary>
    public string? InspectionDescription { get; set; } = string.Empty;

    /// <summary>
    /// 检验员（人员代码）
    /// </summary>
    public string? InspectorName { get; set; } = string.Empty;

    /// <summary>
    /// 检验日期
    /// </summary>
    public DateTime? InspectionDate { get; set; }

    /// <summary>
    /// 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）
    /// </summary>
    public int? JudgeStatus { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

    /// <summary>
    /// 不良处理记录列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktIqcDefectHandlingCreateDto>? DefectHandlings { get; set; }

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
/// IqcOrderItem 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktIqcOrderItemImportDto
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
    /// IQC检验单 ID（选项 TaktIqcOrders/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? IqcOrderId { get; set; }

    /// <summary>
    /// IQC检验单编码（冗余字段，便于查询）
    /// </summary>
    public string? IqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 批次号
    /// </summary>
    public string? BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 进货数量
    /// </summary>
    public decimal? PurchaseQuantity { get; set; }

    /// <summary>
    /// 检验标准编码（选项 TaktInspectionStandards/options；DictValue=StandardCode）
    /// </summary>
    public string? StandardCode { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案编码（选项 TaktSamplingSchemes/options；DictValue=SamplingSchemeCode）
    /// </summary>
    public string? SamplingSchemeCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验方式（0=免检，1=减量，2=正常，3=加严，4=全检）
    /// </summary>
    public int? InspectionMethod { get; set; }

    /// <summary>
    /// 抽样数量
    /// </summary>
    public int? SampleQuantity { get; set; }

    /// <summary>
    /// 合格数量
    /// </summary>
    public int? QualifiedQuantity { get; set; }

    /// <summary>
    /// 不合格数量
    /// </summary>
    public int? UnqualifiedQuantity { get; set; }

    /// <summary>
    /// 验退数量
    /// </summary>
    public decimal? InspectionReturnQuantity { get; set; }

    /// <summary>
    /// 抽检序列号
    /// </summary>
    public string? SampleSerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验说明
    /// </summary>
    public string? InspectionDescription { get; set; } = string.Empty;

    /// <summary>
    /// 检验员（人员代码）
    /// </summary>
    public string? InspectorName { get; set; } = string.Empty;

    /// <summary>
    /// 检验日期
    /// </summary>
    public DateTime? InspectionDate { get; set; }

    /// <summary>
    /// 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）
    /// </summary>
    public int? JudgeStatus { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

    /// <summary>
    /// 不良处理记录列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktIqcDefectHandlingCreateDto>? DefectHandlings { get; set; }

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
/// IqcOrderItem 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktIqcOrderItemExportDto
{
    /// <summary>
    /// IqcOrderItemID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long IqcOrderItemId { get; set; }

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
    /// IQC检验单 ID（选项 TaktIqcOrders/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long IqcOrderId { get; set; }

    /// <summary>
    /// IQC检验单编码（冗余字段，便于查询）
    /// </summary>
    public string IqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 批次号
    /// </summary>
    public string? BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 进货数量
    /// </summary>
    public decimal PurchaseQuantity { get; set; }

    /// <summary>
    /// 检验标准编码（选项 TaktInspectionStandards/options；DictValue=StandardCode）
    /// </summary>
    public string StandardCode { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案编码（选项 TaktSamplingSchemes/options；DictValue=SamplingSchemeCode）
    /// </summary>
    public string SamplingSchemeCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验方式（0=免检，1=减量，2=正常，3=加严，4=全检）
    /// </summary>
    public int InspectionMethod { get; set; } = 0;

    /// <summary>
    /// 抽样数量
    /// </summary>
    public int SampleQuantity { get; set; } = 0;

    /// <summary>
    /// 合格数量
    /// </summary>
    public int QualifiedQuantity { get; set; } = 0;

    /// <summary>
    /// 不合格数量
    /// </summary>
    public int UnqualifiedQuantity { get; set; } = 0;

    /// <summary>
    /// 验退数量
    /// </summary>
    public decimal InspectionReturnQuantity { get; set; }

    /// <summary>
    /// 抽检序列号
    /// </summary>
    public string? SampleSerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验说明
    /// </summary>
    public string? InspectionDescription { get; set; } = string.Empty;

    /// <summary>
    /// 检验员（人员代码）
    /// </summary>
    public string InspectorName { get; set; } = string.Empty;

    /// <summary>
    /// 检验日期
    /// </summary>
    public DateTime InspectionDate { get; set; }

    /// <summary>
    /// 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）
    /// </summary>
    public int JudgeStatus { get; set; } = 0;

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
