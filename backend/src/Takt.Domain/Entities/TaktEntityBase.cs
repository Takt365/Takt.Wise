// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities
// 文件名称：TaktEntityBase.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：实体隔离基类（租户/公司/审批）；CodeFirst 列序：业务列默认 0，本基类隔离/审计列 CreateTableFieldSort≥100（排在 Id=-2、PlantCode/RelatedPlant=-1 之后）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Interfaces;

namespace Takt.Domain.Entities;

// ========================================
// 租户级隔离基类（工厂 × 语言四组合，无 Id）
// 4 都不需要 = TaktTenantCoreEntityScopeBase
// 2 不要工厂+要语言 = TaktTenantCultureEntityScopeBase
// 3 要工厂+不要语言 = TaktTenantPlantEntityScopeBase
// 1 都要 = TaktTenantEntityScopeBase
// ========================================

/// <summary>
/// 租户组合 4 Scope：无关联工厂、无语言（TenantCode + 审计）
/// </summary>
public abstract class TaktTenantCoreEntityScopeBase
{
    /// <summary>
    /// 租户编码(第一层数据隔离)
    /// </summary>
    [SugarColumn(ColumnName = "tenant_code", ColumnDescription = "租户编码", ColumnDataType = "varchar", Length = 3, IsNullable = false, CreateTableFieldSort = 100)]
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段
    /// </summary>
    [SugarColumn(ColumnName = "ext_field", ColumnDescription = "扩展字段", ColumnDataType = "nvarchar", Length = 4000, IsNullable = true, CreateTableFieldSort = 102)]
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", ColumnDataType = "nvarchar", Length = 500, IsNullable = true, CreateTableFieldSort = 103)]
    public string? Remark { get; set; }

    /// <summary>
    /// 创建人ID(非空;无当前用户时仓储填 900001)
    /// </summary>
    [SugarColumn(ColumnName = "created_by", ColumnDescription = "创建人ID", ColumnDataType = "bigint", IsNullable = false, DefaultValue = "0", CreateTableFieldSort = 104)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CreatedBy { get; set; }

    /// <summary>
    /// 创建时间(非空)
    /// </summary>
    [SugarColumn(ColumnName = "created_at", ColumnDescription = "创建时间", ColumnDataType = "datetime", IsNullable = false, CreateTableFieldSort = 105)]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// 更新人ID
    /// </summary>
    [SugarColumn(ColumnName = "updated_by", ColumnDescription = "更新人ID", ColumnDataType = "bigint", IsNullable = true, CreateTableFieldSort = 106)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UpdatedBy { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    [SugarColumn(ColumnName = "updated_at", ColumnDescription = "更新时间", ColumnDataType = "datetime", IsNullable = true, CreateTableFieldSort = 107)]
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// 是否删除(软删除标记,0=未删除,1=已删除)
    /// </summary>
    [SugarColumn(ColumnName = "is_deleted", ColumnDescription = "是否删除", ColumnDataType = "int", IsNullable = false, DefaultValue = "0", CreateTableFieldSort = 108)]
    public int IsDeleted { get; set; } = 0;

    /// <summary>
    /// 删除人ID
    /// </summary>
    [SugarColumn(ColumnName = "deleted_by", ColumnDescription = "删除人ID", ColumnDataType = "bigint", IsNullable = true, CreateTableFieldSort = 109)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeletedBy { get; set; }

    /// <summary>
    /// 删除时间
    /// </summary>
    [SugarColumn(ColumnName = "deleted_at", ColumnDescription = "删除时间", ColumnDataType = "datetime", IsNullable = true, CreateTableFieldSort = 110)]
    public DateTime? DeletedAt { get; set; }
}

/// <summary>
/// 租户组合 2 Scope：无关联工厂、有语言（CultureCode 默认 mul）
/// </summary>
public abstract class TaktTenantCultureEntityScopeBase : TaktTenantCoreEntityScopeBase
{
    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    [SugarColumn(ColumnName = "culture_code", ColumnDescription = "区域文化", ColumnDataType = "varchar", Length = 5, IsNullable = false, DefaultValue = "mul", CreateTableFieldSort = 101)]
    public string CultureCode { get; set; } = "mul";
}

/// <summary>
/// 租户组合 3 Scope：有关联工厂、无语言
/// </summary>
public abstract class TaktTenantPlantEntityScopeBase : TaktTenantCoreEntityScopeBase, ITaktHasRelatedPlant
{
    /// <summary>
    /// 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "related_plant", ColumnDescription = "关联工厂", ColumnDataType = "varchar", Length = 4, IsNullable = false, CreateTableFieldSort = -1)]
    public string RelatedPlant { get; set; } = string.Empty;
}

/// <summary>
/// 租户组合 1 Scope：有关联工厂、有语言（默认租户业务）
/// </summary>
public abstract class TaktTenantEntityScopeBase : TaktTenantCultureEntityScopeBase, ITaktHasRelatedPlant
{
    /// <summary>
    /// 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "related_plant", ColumnDescription = "关联工厂", ColumnDataType = "varchar", Length = 4, IsNullable = false, CreateTableFieldSort = -1)]
    public string RelatedPlant { get; set; } = string.Empty;
}

// ========================================
// 公司级隔离基类
// ========================================

/// <summary>
/// 公司级隔离基类（无 Id）
/// 包含租户+公司双重隔离
/// </summary>
public abstract class TaktCompanyEntityScopeBase
{
    /// <summary>
    /// 租户编码(第一层数据隔离)
    /// </summary>
    [SugarColumn(ColumnName = "tenant_code", ColumnDescription = "租户编码", ColumnDataType = "varchar", Length = 3, IsNullable = false, CreateTableFieldSort = 100)]
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    [SugarColumn(ColumnName = "company_code", ColumnDescription = "公司代码", ColumnDataType = "varchar", Length = 4, IsNullable = false, CreateTableFieldSort = 101)]
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    [SugarColumn(ColumnName = "culture_code", ColumnDescription = "区域文化", ColumnDataType = "varchar", Length = 5, IsNullable = false, DefaultValue = "en-US", CreateTableFieldSort = 102)]
    public string CultureCode { get; set; } = "en-US";

    /// <summary>
    /// 扩展字段
    /// </summary>
    [SugarColumn(ColumnName = "ext_field", ColumnDescription = "扩展字段", ColumnDataType = "nvarchar", Length = 4000, IsNullable = true, CreateTableFieldSort = 103)]
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", ColumnDataType = "nvarchar", Length = 500, IsNullable = true, CreateTableFieldSort = 104)]
    public string? Remark { get; set; }

    /// <summary>
    /// 创建人ID(非空;无当前用户时仓储填 900001)
    /// </summary>
    [SugarColumn(ColumnName = "created_by", ColumnDescription = "创建人ID", ColumnDataType = "bigint", IsNullable = false, DefaultValue = "0", CreateTableFieldSort = 105)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CreatedBy { get; set; }

    /// <summary>
    /// 创建时间(非空)
    /// </summary>
    [SugarColumn(ColumnName = "created_at", ColumnDescription = "创建时间", ColumnDataType = "datetime", IsNullable = false, CreateTableFieldSort = 106)]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// 更新人ID
    /// </summary>
    [SugarColumn(ColumnName = "updated_by", ColumnDescription = "更新人ID", ColumnDataType = "bigint", IsNullable = true, CreateTableFieldSort = 107)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UpdatedBy { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    [SugarColumn(ColumnName = "updated_at", ColumnDescription = "更新时间", ColumnDataType = "datetime", IsNullable = true, CreateTableFieldSort = 108)]
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// 是否删除(软删除标记,0=未删除,1=已删除)
    /// </summary>
    [SugarColumn(ColumnName = "is_deleted", ColumnDescription = "是否删除", ColumnDataType = "int", IsNullable = false, DefaultValue = "0", CreateTableFieldSort = 109)]
    public int IsDeleted { get; set; } = 0;

    /// <summary>
    /// 删除人ID
    /// </summary>
    [SugarColumn(ColumnName = "deleted_by", ColumnDescription = "删除人ID", ColumnDataType = "bigint", IsNullable = true, CreateTableFieldSort = 110)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeletedBy { get; set; }

    /// <summary>
    /// 删除时间
    /// </summary>
    [SugarColumn(ColumnName = "deleted_at", ColumnDescription = "删除时间", ColumnDataType = "datetime", IsNullable = true, CreateTableFieldSort = 111)]
    public DateTime? DeletedAt { get; set; }
}

// ========================================
// 审批级隔离基类
// ========================================

/// <summary>
/// 审批级隔离基类（无 Id）
/// 包含租户+公司双重隔离+审批流程相关字段
/// </summary>
public abstract class TaktApprovalEntityScopeBase
{
    /// <summary>
    /// 租户编码(第一层数据隔离)
    /// </summary>
    [SugarColumn(ColumnName = "tenant_code", ColumnDescription = "租户编码", ColumnDataType = "varchar", Length = 3, IsNullable = false, CreateTableFieldSort = 100)]
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    [SugarColumn(ColumnName = "company_code", ColumnDescription = "公司代码", ColumnDataType = "varchar", Length = 4, IsNullable = false, CreateTableFieldSort = 101)]
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    [SugarColumn(ColumnName = "culture_code", ColumnDescription = "区域文化", ColumnDataType = "varchar", Length = 5, IsNullable = false, DefaultValue = "en-US", CreateTableFieldSort = 102)]
    public string CultureCode { get; set; } = "en-US";

    /// <summary>
    /// 扩展字段
    /// </summary>
    [SugarColumn(ColumnName = "ext_field", ColumnDescription = "扩展字段", ColumnDataType = "nvarchar", Length = 4000, IsNullable = true, CreateTableFieldSort = 103)]
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", ColumnDataType = "nvarchar", Length = 500, IsNullable = true, CreateTableFieldSort = 104)]
    public string? Remark { get; set; }

    /// <summary>
    /// 审批状态（字典 sys_approval_status；0=待审批，1=审批中，2=已通过，3=已驳回，4=已撤回，5=已终止）
    /// </summary>
    [SugarColumn(ColumnName = "approval_status", ColumnDescription = "审批状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0", CreateTableFieldSort = 105)]
    public int ApprovalStatus { get; set; } = 0;

    /// <summary>
    /// 流程实例 ID（关联 TaktFlowInstance，发起审批后由业务写入）
    /// </summary>
    [SugarColumn(ColumnName = "flow_instance_id", ColumnDescription = "流程实例ID", ColumnDataType = "bigint", IsNullable = true, CreateTableFieldSort = 106)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FlowInstanceId { get; set; }

    /// <summary>
    /// 发起人ID
    /// </summary>
    [SugarColumn(ColumnName = "initiator_id", ColumnDescription = "发起人ID", ColumnDataType = "bigint", IsNullable = true, CreateTableFieldSort = 107)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InitiatorId { get; set; }

    /// <summary>
    /// 发起时间
    /// </summary>
    [SugarColumn(ColumnName = "initiated_at", ColumnDescription = "发起时间", ColumnDataType = "datetime", IsNullable = true, CreateTableFieldSort = 108)]
    public DateTime? InitiatedAt { get; set; }

    /// <summary>
    /// 审批意见（支持多级审批时多条意见，用JSON数组存储）
    /// </summary>
    [SugarColumn(ColumnName = "approval_opinion", ColumnDescription = "审批意见", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true, CreateTableFieldSort = 109)]
    public string? ApprovalOpinion { get; set; }

    /// <summary>
    /// 最终审批人ID
    /// </summary>
    [SugarColumn(ColumnName = "approved_by", ColumnDescription = "最终审批人ID", ColumnDataType = "bigint", IsNullable = true, CreateTableFieldSort = 110)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApprovedBy { get; set; }

    /// <summary>
    /// 最终审批时间
    /// </summary>
    [SugarColumn(ColumnName = "approved_at", ColumnDescription = "最终审批时间", ColumnDataType = "datetime", IsNullable = true, CreateTableFieldSort = 111)]
    public DateTime? ApprovedAt { get; set; }

    /// <summary>
    /// 创建人ID(非空;无当前用户时仓储填 900001)
    /// </summary>
    [SugarColumn(ColumnName = "created_by", ColumnDescription = "创建人ID", ColumnDataType = "bigint", IsNullable = false, DefaultValue = "0", CreateTableFieldSort = 112)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CreatedBy { get; set; }

    /// <summary>
    /// 创建时间(非空)
    /// </summary>
    [SugarColumn(ColumnName = "created_at", ColumnDescription = "创建时间", ColumnDataType = "datetime", IsNullable = false, CreateTableFieldSort = 113)]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// 更新人ID
    /// </summary>
    [SugarColumn(ColumnName = "updated_by", ColumnDescription = "更新人ID", ColumnDataType = "bigint", IsNullable = true, CreateTableFieldSort = 114)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UpdatedBy { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    [SugarColumn(ColumnName = "updated_at", ColumnDescription = "更新时间", ColumnDataType = "datetime", IsNullable = true, CreateTableFieldSort = 115)]
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// 是否删除(软删除标记,0=未删除,1=已删除)
    /// </summary>
    [SugarColumn(ColumnName = "is_deleted", ColumnDescription = "是否删除", ColumnDataType = "int", IsNullable = false, DefaultValue = "0", CreateTableFieldSort = 116)]
    public int IsDeleted { get; set; } = 0;

    /// <summary>
    /// 删除人ID
    /// </summary>
    [SugarColumn(ColumnName = "deleted_by", ColumnDescription = "删除人ID", ColumnDataType = "bigint", IsNullable = true, CreateTableFieldSort = 117)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeletedBy { get; set; }

    /// <summary>
    /// 删除时间
    /// </summary>
    [SugarColumn(ColumnName = "deleted_at", ColumnDescription = "删除时间", ColumnDataType = "datetime", IsNullable = true, CreateTableFieldSort = 118)]
    public DateTime? DeletedAt { get; set; }
}
