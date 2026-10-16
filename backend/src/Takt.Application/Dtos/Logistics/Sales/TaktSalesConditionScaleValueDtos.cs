// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Sales
// 文件名称：TaktSalesConditionScaleValueDtos.cs
// 创建时间：2026-09-21
// 创建人：Takt365(Auto Generated)
// 功能描述：SalesConditionScaleValue 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSalesConditionScaleValue 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Sales;

// ========================================
// SalesConditionScaleValue 响应 DTO
// ========================================

/// <summary>
/// Takt销售阶梯价格实体（主子表：TaktSalesConditionItem → ScaleValues；与数量等级仅差 ScaleValue↔ScaleQuantity）
/// 对应前端 TaktSalesConditionScaleValueDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSalesConditionScaleValueDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SalesConditionScaleValueID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesConditionScaleValueId { get; set; }

    /// <summary>
    /// 销售条件行 ID（主子表关系；选项 TaktSalesConditionItems/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesConditionItemId { get; set; }

    /// <summary>
    /// 销售条件行 名称（填充字段）
    /// </summary>
    public string? SalesConditionItemName { get; set; }

    /// <summary>
    /// 定价记录号（冗余：与明细 SalesConditionCode 一致）
    /// </summary>
    public string SalesConditionCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价序号（冗余：与明细 SalesConditionSeq 一致，固定步长=10）
    /// </summary>
    public int SalesConditionSeq { get; set; } = 0;

    /// <summary>
    /// 等级序号（回填：同一明细内阶梯序号，固定步长=10）
    /// </summary>
    public int SalesScaleSeq { get; set; } = 0;

    /// <summary>
    /// 等级值（价值等级门槛；对应数量等级表的 ScaleQuantity）
    /// </summary>
    public decimal ScaleValue { get; set; }

    /// <summary>
    /// 价格
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// 未税价格（冗余；可由 Price 与税码推算后回写）
    /// </summary>
    public decimal UntaxedPrice { get; set; }

    /// <summary>
    /// 含税价格（冗余；可由 Price 与税码推算后回写）
    /// </summary>
    public decimal TaxIncludedPrice { get; set; }

    /// <summary>
    /// 税费（冗余；含税−未税，打印用）
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

}

// ========================================
// SalesConditionScaleValue 查询 DTO
// ========================================

/// <summary>
/// SalesConditionScaleValue 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSalesConditionScaleValueQueryDto : TaktPagedQuery
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
    /// 销售条件行 ID（主子表关系；选项 TaktSalesConditionItems/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesConditionItemId { get; set; }

    /// <summary>
    /// 定价记录号（冗余：与明细 SalesConditionCode 一致）
    /// </summary>
    public string? SalesConditionCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价序号（冗余：与明细 SalesConditionSeq 一致，固定步长=10）
    /// </summary>
    public int? SalesConditionSeq { get; set; }

    /// <summary>
    /// 等级序号（回填：同一明细内阶梯序号，固定步长=10）
    /// </summary>
    public int? SalesScaleSeq { get; set; }

    /// <summary>
    /// 等级值（价值等级门槛；对应数量等级表的 ScaleQuantity）
    /// </summary>
    public decimal? ScaleValue { get; set; }

    /// <summary>
    /// 价格
    /// </summary>
    public decimal? Price { get; set; }

    /// <summary>
    /// 未税价格（冗余；可由 Price 与税码推算后回写）
    /// </summary>
    public decimal? UntaxedPrice { get; set; }

    /// <summary>
    /// 含税价格（冗余；可由 Price 与税码推算后回写）
    /// </summary>
    public decimal? TaxIncludedPrice { get; set; }

    /// <summary>
    /// 税费（冗余；含税−未税，打印用）
    /// </summary>
    public decimal? TaxAmount { get; set; }

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
// 创建SalesConditionScaleValue DTO
// ========================================

/// <summary>
/// 创建SalesConditionScaleValue DTO
/// </summary>
public class TaktSalesConditionScaleValueCreateDto
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
    /// 销售条件行 ID（主子表关系；选项 TaktSalesConditionItems/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesConditionItemId { get; set; }

    /// <summary>
    /// 定价记录号（冗余：与明细 SalesConditionCode 一致）
    /// </summary>
    [Required(ErrorMessage = "定价记录号（冗余：与明细 SalesConditionCode 一致）不能为空")]
    public string SalesConditionCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价序号（冗余：与明细 SalesConditionSeq 一致，固定步长=10）
    /// </summary>
    public int SalesConditionSeq { get; set; } = 0;

    /// <summary>
    /// 等级序号（回填：同一明细内阶梯序号，固定步长=10）
    /// </summary>
    public int SalesScaleSeq { get; set; } = 0;

    /// <summary>
    /// 等级值（价值等级门槛；对应数量等级表的 ScaleQuantity）
    /// </summary>
    public decimal ScaleValue { get; set; }

    /// <summary>
    /// 价格
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// 未税价格（冗余；可由 Price 与税码推算后回写）
    /// </summary>
    public decimal UntaxedPrice { get; set; }

    /// <summary>
    /// 含税价格（冗余；可由 Price 与税码推算后回写）
    /// </summary>
    public decimal TaxIncludedPrice { get; set; }

    /// <summary>
    /// 税费（冗余；含税−未税，打印用）
    /// </summary>
    public decimal TaxAmount { get; set; }

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
// 更新SalesConditionScaleValue DTO
// ========================================

/// <summary>
/// 更新SalesConditionScaleValue DTO
/// 继承 TaktSalesConditionScaleValueCreateDto，添加 SalesConditionScaleValueId 字段
/// </summary>
public class TaktSalesConditionScaleValueUpdateDto : TaktSalesConditionScaleValueCreateDto
{
    /// <summary>
    /// SalesConditionScaleValueID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesConditionScaleValueId { get; set; }

}

// ========================================
// SalesConditionScaleValue 作废 DTO
// ========================================

/// <summary>
/// SalesConditionScaleValue 作废/撤销作废 DTO
/// </summary>
public class TaktSalesConditionScaleValueObsoleteDto
{
    /// <summary>
    /// SalesConditionScaleValueID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesConditionScaleValueId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SalesConditionScaleValue 导入模板行 DTO
/// </summary>
public class TaktSalesConditionScaleValueTemplateDto
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
    /// 销售条件行 ID（主子表关系；选项 TaktSalesConditionItems/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesConditionItemId { get; set; }

    /// <summary>
    /// 定价记录号（冗余：与明细 SalesConditionCode 一致）
    /// </summary>
    public string? SalesConditionCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价序号（冗余：与明细 SalesConditionSeq 一致，固定步长=10）
    /// </summary>
    public int? SalesConditionSeq { get; set; }

    /// <summary>
    /// 等级序号（回填：同一明细内阶梯序号，固定步长=10）
    /// </summary>
    public int? SalesScaleSeq { get; set; }

    /// <summary>
    /// 等级值（价值等级门槛；对应数量等级表的 ScaleQuantity）
    /// </summary>
    public decimal? ScaleValue { get; set; }

    /// <summary>
    /// 价格
    /// </summary>
    public decimal? Price { get; set; }

    /// <summary>
    /// 未税价格（冗余；可由 Price 与税码推算后回写）
    /// </summary>
    public decimal? UntaxedPrice { get; set; }

    /// <summary>
    /// 含税价格（冗余；可由 Price 与税码推算后回写）
    /// </summary>
    public decimal? TaxIncludedPrice { get; set; }

    /// <summary>
    /// 税费（冗余；含税−未税，打印用）
    /// </summary>
    public decimal? TaxAmount { get; set; }

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
/// SalesConditionScaleValue 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSalesConditionScaleValueImportDto
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
    /// 销售条件行 ID（主子表关系；选项 TaktSalesConditionItems/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesConditionItemId { get; set; }

    /// <summary>
    /// 定价记录号（冗余：与明细 SalesConditionCode 一致）
    /// </summary>
    public string? SalesConditionCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价序号（冗余：与明细 SalesConditionSeq 一致，固定步长=10）
    /// </summary>
    public int? SalesConditionSeq { get; set; }

    /// <summary>
    /// 等级序号（回填：同一明细内阶梯序号，固定步长=10）
    /// </summary>
    public int? SalesScaleSeq { get; set; }

    /// <summary>
    /// 等级值（价值等级门槛；对应数量等级表的 ScaleQuantity）
    /// </summary>
    public decimal? ScaleValue { get; set; }

    /// <summary>
    /// 价格
    /// </summary>
    public decimal? Price { get; set; }

    /// <summary>
    /// 未税价格（冗余；可由 Price 与税码推算后回写）
    /// </summary>
    public decimal? UntaxedPrice { get; set; }

    /// <summary>
    /// 含税价格（冗余；可由 Price 与税码推算后回写）
    /// </summary>
    public decimal? TaxIncludedPrice { get; set; }

    /// <summary>
    /// 税费（冗余；含税−未税，打印用）
    /// </summary>
    public decimal? TaxAmount { get; set; }

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
/// SalesConditionScaleValue 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSalesConditionScaleValueExportDto
{
    /// <summary>
    /// SalesConditionScaleValueID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesConditionScaleValueId { get; set; }

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
    /// 销售条件行 ID（主子表关系；选项 TaktSalesConditionItems/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesConditionItemId { get; set; }

    /// <summary>
    /// 定价记录号（冗余：与明细 SalesConditionCode 一致）
    /// </summary>
    public string SalesConditionCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价序号（冗余：与明细 SalesConditionSeq 一致，固定步长=10）
    /// </summary>
    public int SalesConditionSeq { get; set; } = 0;

    /// <summary>
    /// 等级序号（回填：同一明细内阶梯序号，固定步长=10）
    /// </summary>
    public int SalesScaleSeq { get; set; } = 0;

    /// <summary>
    /// 等级值（价值等级门槛；对应数量等级表的 ScaleQuantity）
    /// </summary>
    public decimal ScaleValue { get; set; }

    /// <summary>
    /// 价格
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// 未税价格（冗余；可由 Price 与税码推算后回写）
    /// </summary>
    public decimal UntaxedPrice { get; set; }

    /// <summary>
    /// 含税价格（冗余；可由 Price 与税码推算后回写）
    /// </summary>
    public decimal TaxIncludedPrice { get; set; }

    /// <summary>
    /// 税费（冗余；含税−未税，打印用）
    /// </summary>
    public decimal TaxAmount { get; set; }

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
