// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Sales
// 文件名称：TaktSalesConditionRecordScaleValueDtos.cs
// 创建时间：2026-09-21
// 创建人：Takt365(Auto Generated)
// 功能描述：SalesConditionRecordScaleValue 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSalesConditionRecordScaleValue 生成，请按需审阅）
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
// SalesConditionRecordScaleValue 响应 DTO
// ========================================

/// <summary>
/// Takt销售阶梯价格实体（主子表：TaktSalesConditionRecordItem → ScaleValues；与数量等级仅差 ScaleValue↔ScaleQuantity）
/// 对应前端 TaktSalesConditionRecordScaleValueDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSalesConditionRecordScaleValueDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SalesConditionRecordScaleValueID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesConditionRecordScaleValueId { get; set; }

    /// <summary>
    /// 销售条件记录行 ID（主子表关系；选项 TaktSalesConditionRecordItems/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesConditionRecordItemId { get; set; }

    /// <summary>
    /// 销售条件记录行 名称（填充字段）
    /// </summary>
    public string? SalesConditionRecordItemName { get; set; }

    /// <summary>
    /// 定价记录号（冗余：与明细 SalesConditionRecordCode 一致）
    /// </summary>
    public string SalesConditionRecordCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价序号（冗余：与明细 SalesConditionRecordSeq 一致，固定步长=10）
    /// </summary>
    public int SalesConditionRecordSeq { get; set; } = 0;

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
// SalesConditionRecordScaleValue 查询 DTO
// ========================================

/// <summary>
/// SalesConditionRecordScaleValue 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSalesConditionRecordScaleValueQueryDto : TaktPagedQuery
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
    /// 销售条件记录行 ID（主子表关系；选项 TaktSalesConditionRecordItems/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesConditionRecordItemId { get; set; }

    /// <summary>
    /// 定价记录号（冗余：与明细 SalesConditionRecordCode 一致）
    /// </summary>
    public string? SalesConditionRecordCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价序号（冗余：与明细 SalesConditionRecordSeq 一致，固定步长=10）
    /// </summary>
    public int? SalesConditionRecordSeq { get; set; }

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
// 创建SalesConditionRecordScaleValue DTO
// ========================================

/// <summary>
/// 创建SalesConditionRecordScaleValue DTO
/// </summary>
public class TaktSalesConditionRecordScaleValueCreateDto
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
    /// 销售条件记录行 ID（主子表关系；选项 TaktSalesConditionRecordItems/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesConditionRecordItemId { get; set; }

    /// <summary>
    /// 定价记录号（冗余：与明细 SalesConditionRecordCode 一致）
    /// </summary>
    [Required(ErrorMessage = "定价记录号（冗余：与明细 SalesConditionRecordCode 一致）不能为空")]
    public string SalesConditionRecordCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价序号（冗余：与明细 SalesConditionRecordSeq 一致，固定步长=10）
    /// </summary>
    public int SalesConditionRecordSeq { get; set; } = 0;

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
// 更新SalesConditionRecordScaleValue DTO
// ========================================

/// <summary>
/// 更新SalesConditionRecordScaleValue DTO
/// 继承 TaktSalesConditionRecordScaleValueCreateDto，添加 SalesConditionRecordScaleValueId 字段
/// </summary>
public class TaktSalesConditionRecordScaleValueUpdateDto : TaktSalesConditionRecordScaleValueCreateDto
{
    /// <summary>
    /// SalesConditionRecordScaleValueID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesConditionRecordScaleValueId { get; set; }

}

// ========================================
// SalesConditionRecordScaleValue 作废 DTO
// ========================================

/// <summary>
/// SalesConditionRecordScaleValue 作废/撤销作废 DTO
/// </summary>
public class TaktSalesConditionRecordScaleValueObsoleteDto
{
    /// <summary>
    /// SalesConditionRecordScaleValueID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesConditionRecordScaleValueId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SalesConditionRecordScaleValue 导入模板行 DTO
/// </summary>
public class TaktSalesConditionRecordScaleValueTemplateDto
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
    /// 销售条件记录行 ID（主子表关系；选项 TaktSalesConditionRecordItems/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesConditionRecordItemId { get; set; }

    /// <summary>
    /// 定价记录号（冗余：与明细 SalesConditionRecordCode 一致）
    /// </summary>
    public string? SalesConditionRecordCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价序号（冗余：与明细 SalesConditionRecordSeq 一致，固定步长=10）
    /// </summary>
    public int? SalesConditionRecordSeq { get; set; }

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
/// SalesConditionRecordScaleValue 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSalesConditionRecordScaleValueImportDto
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
    /// 销售条件记录行 ID（主子表关系；选项 TaktSalesConditionRecordItems/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesConditionRecordItemId { get; set; }

    /// <summary>
    /// 定价记录号（冗余：与明细 SalesConditionRecordCode 一致）
    /// </summary>
    public string? SalesConditionRecordCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价序号（冗余：与明细 SalesConditionRecordSeq 一致，固定步长=10）
    /// </summary>
    public int? SalesConditionRecordSeq { get; set; }

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
/// SalesConditionRecordScaleValue 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSalesConditionRecordScaleValueExportDto
{
    /// <summary>
    /// SalesConditionRecordScaleValueID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesConditionRecordScaleValueId { get; set; }

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
    /// 销售条件记录行 ID（主子表关系；选项 TaktSalesConditionRecordItems/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesConditionRecordItemId { get; set; }

    /// <summary>
    /// 定价记录号（冗余：与明细 SalesConditionRecordCode 一致）
    /// </summary>
    public string SalesConditionRecordCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价序号（冗余：与明细 SalesConditionRecordSeq 一致，固定步长=10）
    /// </summary>
    public int SalesConditionRecordSeq { get; set; } = 0;

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
