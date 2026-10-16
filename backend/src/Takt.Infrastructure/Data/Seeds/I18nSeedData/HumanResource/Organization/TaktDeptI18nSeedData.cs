// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Organization
// 文件名称：TaktDeptI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktDept 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Organization;

/// <summary>
/// TaktDept 实体国际化翻译种子（键前缀 entity.dept.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktDeptI18nSeedData : ITaktSeedDataCoordinator
{
    /// <summary>
    /// 执行顺序（实体翻译种子，位于部门翻译之后）
    /// </summary>
    public int Order => 52;

    /// <summary>
    /// 初始化实体字段翻译种子
    /// </summary>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化 TaktDept 实体国际化翻译种子...");

        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过实体国际化翻译种子初始化");
            return (0, 0);
        }

        var repository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktTranslation>>();
        var cultureRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCulture>>();
        var cultureIdByCode = (await cultureRepository.GetListAsync(c => c.TenantCode == tenantCode))
            .ToDictionary(c => c.CultureCode, c => c.Id);
        int insertCount = 0;
        int updateCount = 0;

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 dept 实体翻译...", tenantCode);

        foreach (var item in GetDeptTranslations())
        {
            if (!cultureIdByCode.TryGetValue(item.CultureCode, out var cultureId))
            {
                TaktLogger.Warning("未找到区域文化 {CultureCode}，跳过翻译 {I18nKey}", item.CultureCode, item.I18nKey);
                continue;
            }

            var (translation, i, u) = await CreateOrUpdateTranslationAsync(
                repository,
                tenantCode,
                cultureId,
                item);
            insertCount += i;
            updateCount += u;
        }

        TaktLogger.Information("TaktDept 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktDept 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.dept._self / entity.dept.{{field}}；ResourceGroup=Organization；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetDeptTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.dept._self
            new TranslationSeedItem("entity.dept._self", "en-US", "Dept Information_us", "实体名称"),
            // entity.dept._self
            new TranslationSeedItem("entity.dept._self", "ja-JP", "部门信息_jp", "实体名称"),
            // entity.dept._self
            new TranslationSeedItem("entity.dept._self", "zh-CN", "部门信息", "实体名称"),
            // entity.dept._self
            new TranslationSeedItem("entity.dept._self", "zh-HK", "部门信息_hk", "实体名称"),

            // entity.dept.code
            new TranslationSeedItem("entity.dept.code", "en-US", "部门编码_us", "部门编码（唯一索引：租户+公司内唯一，见 ix_dept_code_unique；与成本中心编码同长 6；前端表单选择编码规则后自动通过 TaktNumbering 部门编码规则生成并展示，非手输；单据类型菜单：部门管理）"),
            // entity.dept.code
            new TranslationSeedItem("entity.dept.code", "ja-JP", "部门编码_jp", "部门编码（唯一索引：租户+公司内唯一，见 ix_dept_code_unique；与成本中心编码同长 6；前端表单选择编码规则后自动通过 TaktNumbering 部门编码规则生成并展示，非手输；单据类型菜单：部门管理）"),
            // entity.dept.code
            new TranslationSeedItem("entity.dept.code", "zh-CN", "部门编码", "部门编码（唯一索引：租户+公司内唯一，见 ix_dept_code_unique；与成本中心编码同长 6；前端表单选择编码规则后自动通过 TaktNumbering 部门编码规则生成并展示，非手输；单据类型菜单：部门管理）"),
            // entity.dept.code
            new TranslationSeedItem("entity.dept.code", "zh-HK", "部门编码_hk", "部门编码（唯一索引：租户+公司内唯一，见 ix_dept_code_unique；与成本中心编码同长 6；前端表单选择编码规则后自动通过 TaktNumbering 部门编码规则生成并展示，非手输；单据类型菜单：部门管理）"),

            // entity.dept.shortname
            new TranslationSeedItem("entity.dept.shortname", "en-US", "部门简称_us", "部门简称（与 ISO 编码一致，长度 6）"),
            // entity.dept.shortname
            new TranslationSeedItem("entity.dept.shortname", "ja-JP", "部门简称_jp", "部门简称（与 ISO 编码一致，长度 6）"),
            // entity.dept.shortname
            new TranslationSeedItem("entity.dept.shortname", "zh-CN", "部门简称", "部门简称（与 ISO 编码一致，长度 6）"),
            // entity.dept.shortname
            new TranslationSeedItem("entity.dept.shortname", "zh-HK", "部门简称_hk", "部门简称（与 ISO 编码一致，长度 6）"),

            // entity.dept.name1
            new TranslationSeedItem("entity.dept.name1", "en-US", "部门名称1_us", "部门名称1"),
            // entity.dept.name1
            new TranslationSeedItem("entity.dept.name1", "ja-JP", "部门名称1_jp", "部门名称1"),
            // entity.dept.name1
            new TranslationSeedItem("entity.dept.name1", "zh-CN", "部门名称1", "部门名称1"),
            // entity.dept.name1
            new TranslationSeedItem("entity.dept.name1", "zh-HK", "部门名称1_hk", "部门名称1"),

            // entity.dept.name2
            new TranslationSeedItem("entity.dept.name2", "en-US", "部门名称2_us", "部门名称2"),
            // entity.dept.name2
            new TranslationSeedItem("entity.dept.name2", "ja-JP", "部门名称2_jp", "部门名称2"),
            // entity.dept.name2
            new TranslationSeedItem("entity.dept.name2", "zh-CN", "部门名称2", "部门名称2"),
            // entity.dept.name2
            new TranslationSeedItem("entity.dept.name2", "zh-HK", "部门名称2_hk", "部门名称2"),

            // entity.dept.parentid
            new TranslationSeedItem("entity.dept.parentid", "en-US", "父部门ID_us", "父部门（关联 TaktDept.Id，选项 TaktDepts/tree-options；0=根部门）"),
            // entity.dept.parentid
            new TranslationSeedItem("entity.dept.parentid", "ja-JP", "父部门ID_jp", "父部门（关联 TaktDept.Id，选项 TaktDepts/tree-options；0=根部门）"),
            // entity.dept.parentid
            new TranslationSeedItem("entity.dept.parentid", "zh-CN", "父部门ID", "父部门（关联 TaktDept.Id，选项 TaktDepts/tree-options；0=根部门）"),
            // entity.dept.parentid
            new TranslationSeedItem("entity.dept.parentid", "zh-HK", "父部门ID_hk", "父部门（关联 TaktDept.Id，选项 TaktDepts/tree-options；0=根部门）"),

            // entity.dept.level
            new TranslationSeedItem("entity.dept.level", "en-US", "层级_us", "层级（1=一级部门，2=二级部门，以此类推）"),
            // entity.dept.level
            new TranslationSeedItem("entity.dept.level", "ja-JP", "层级_jp", "层级（1=一级部门，2=二级部门，以此类推）"),
            // entity.dept.level
            new TranslationSeedItem("entity.dept.level", "zh-CN", "层级", "层级（1=一级部门，2=二级部门，以此类推）"),
            // entity.dept.level
            new TranslationSeedItem("entity.dept.level", "zh-HK", "层级_hk", "层级（1=一级部门，2=二级部门，以此类推）"),

            // entity.dept.path
            new TranslationSeedItem("entity.dept.path", "en-US", "部门路径_us", "部门路径（如：/1/3/5/，用于快速查询子部门）"),
            // entity.dept.path
            new TranslationSeedItem("entity.dept.path", "ja-JP", "部门路径_jp", "部门路径（如：/1/3/5/，用于快速查询子部门）"),
            // entity.dept.path
            new TranslationSeedItem("entity.dept.path", "zh-CN", "部门路径", "部门路径（如：/1/3/5/，用于快速查询子部门）"),
            // entity.dept.path
            new TranslationSeedItem("entity.dept.path", "zh-HK", "部门路径_hk", "部门路径（如：/1/3/5/，用于快速查询子部门）"),

            // entity.dept.isleaf
            new TranslationSeedItem("entity.dept.isleaf", "en-US", "叶子节点_us", "叶子节点（字典 sys_yes_no；0=否 1=是）"),
            // entity.dept.isleaf
            new TranslationSeedItem("entity.dept.isleaf", "ja-JP", "叶子节点_jp", "叶子节点（字典 sys_yes_no；0=否 1=是）"),
            // entity.dept.isleaf
            new TranslationSeedItem("entity.dept.isleaf", "zh-CN", "叶子节点", "叶子节点（字典 sys_yes_no；0=否 1=是）"),
            // entity.dept.isleaf
            new TranslationSeedItem("entity.dept.isleaf", "zh-HK", "叶子节点_hk", "叶子节点（字典 sys_yes_no；0=否 1=是）"),

            // entity.dept.isocode
            new TranslationSeedItem("entity.dept.isocode", "en-US", "ISO编码_us", "ISO 编码（与部门简称 dept_short_name 一致，长度 6）"),
            // entity.dept.isocode
            new TranslationSeedItem("entity.dept.isocode", "ja-JP", "ISO编码_jp", "ISO 编码（与部门简称 dept_short_name 一致，长度 6）"),
            // entity.dept.isocode
            new TranslationSeedItem("entity.dept.isocode", "zh-CN", "ISO编码", "ISO 编码（与部门简称 dept_short_name 一致，长度 6）"),
            // entity.dept.isocode
            new TranslationSeedItem("entity.dept.isocode", "zh-HK", "ISO编码_hk", "ISO 编码（与部门简称 dept_short_name 一致，长度 6）"),

            // entity.dept.costcentercode
            new TranslationSeedItem("entity.dept.costcentercode", "en-US", "成本中心编码_us", "成本中心编码（关联 TaktCostCenter.CostCenterCode，选项 TaktCostCenters/tree-options；默认与部门编码一致，长度 6）"),
            // entity.dept.costcentercode
            new TranslationSeedItem("entity.dept.costcentercode", "ja-JP", "成本中心编码_jp", "成本中心编码（关联 TaktCostCenter.CostCenterCode，选项 TaktCostCenters/tree-options；默认与部门编码一致，长度 6）"),
            // entity.dept.costcentercode
            new TranslationSeedItem("entity.dept.costcentercode", "zh-CN", "成本中心编码", "成本中心编码（关联 TaktCostCenter.CostCenterCode，选项 TaktCostCenters/tree-options；默认与部门编码一致，长度 6）"),
            // entity.dept.costcentercode
            new TranslationSeedItem("entity.dept.costcentercode", "zh-HK", "成本中心编码_hk", "成本中心编码（关联 TaktCostCenter.CostCenterCode，选项 TaktCostCenters/tree-options；默认与部门编码一致，长度 6）"),

            // entity.dept.costcategory
            new TranslationSeedItem("entity.dept.costcategory", "en-US", "费用类别_us", "费用类别（字典 humanresource_organization_dept_cost_category；1=直接 2=间接）"),
            // entity.dept.costcategory
            new TranslationSeedItem("entity.dept.costcategory", "ja-JP", "费用类别_jp", "费用类别（字典 humanresource_organization_dept_cost_category；1=直接 2=间接）"),
            // entity.dept.costcategory
            new TranslationSeedItem("entity.dept.costcategory", "zh-CN", "费用类别", "费用类别（字典 humanresource_organization_dept_cost_category；1=直接 2=间接）"),
            // entity.dept.costcategory
            new TranslationSeedItem("entity.dept.costcategory", "zh-HK", "费用类别_hk", "费用类别（字典 humanresource_organization_dept_cost_category；1=直接 2=间接）"),

            // entity.dept.headuserid
            new TranslationSeedItem("entity.dept.headuserid", "en-US", "部门负责人ID_us", "部门负责人（选项 TaktUsers/options，DictValue=Id）"),
            // entity.dept.headuserid
            new TranslationSeedItem("entity.dept.headuserid", "ja-JP", "部门负责人ID_jp", "部门负责人（选项 TaktUsers/options，DictValue=Id）"),
            // entity.dept.headuserid
            new TranslationSeedItem("entity.dept.headuserid", "zh-CN", "部门负责人ID", "部门负责人（选项 TaktUsers/options，DictValue=Id）"),
            // entity.dept.headuserid
            new TranslationSeedItem("entity.dept.headuserid", "zh-HK", "部门负责人ID_hk", "部门负责人（选项 TaktUsers/options，DictValue=Id）"),

            // entity.dept.headusername
            new TranslationSeedItem("entity.dept.headusername", "en-US", "部门负责人名称_us", "部门负责人名称（冗余：按 HeadUserId 取 TaktUser.NickName联动）"),
            // entity.dept.headusername
            new TranslationSeedItem("entity.dept.headusername", "ja-JP", "部门负责人名称_jp", "部门负责人名称（冗余：按 HeadUserId 取 TaktUser.NickName联动）"),
            // entity.dept.headusername
            new TranslationSeedItem("entity.dept.headusername", "zh-CN", "部门负责人名称", "部门负责人名称（冗余：按 HeadUserId 取 TaktUser.NickName联动）"),
            // entity.dept.headusername
            new TranslationSeedItem("entity.dept.headusername", "zh-HK", "部门负责人名称_hk", "部门负责人名称（冗余：按 HeadUserId 取 TaktUser.NickName联动）"),

            // entity.dept.phone
            new TranslationSeedItem("entity.dept.phone", "en-US", "联系电话_us", "联系电话"),
            // entity.dept.phone
            new TranslationSeedItem("entity.dept.phone", "ja-JP", "联系电话_jp", "联系电话"),
            // entity.dept.phone
            new TranslationSeedItem("entity.dept.phone", "zh-CN", "联系电话", "联系电话"),
            // entity.dept.phone
            new TranslationSeedItem("entity.dept.phone", "zh-HK", "联系电话_hk", "联系电话"),

            // entity.dept.email
            new TranslationSeedItem("entity.dept.email", "en-US", "邮箱_us", "邮箱"),
            // entity.dept.email
            new TranslationSeedItem("entity.dept.email", "ja-JP", "邮箱_jp", "邮箱"),
            // entity.dept.email
            new TranslationSeedItem("entity.dept.email", "zh-CN", "邮箱", "邮箱"),
            // entity.dept.email
            new TranslationSeedItem("entity.dept.email", "zh-HK", "邮箱_hk", "邮箱"),

            // entity.dept.location
            new TranslationSeedItem("entity.dept.location", "en-US", "办公地点_us", "办公地点"),
            // entity.dept.location
            new TranslationSeedItem("entity.dept.location", "ja-JP", "办公地点_jp", "办公地点"),
            // entity.dept.location
            new TranslationSeedItem("entity.dept.location", "zh-CN", "办公地点", "办公地点"),
            // entity.dept.location
            new TranslationSeedItem("entity.dept.location", "zh-HK", "办公地点_hk", "办公地点"),

            // entity.dept.isbuiltin
            new TranslationSeedItem("entity.dept.isbuiltin", "en-US", "内置_us", "内置（字典 sys_yes_no；0=否 1=是；种子部门为内置，不允许删除）"),
            // entity.dept.isbuiltin
            new TranslationSeedItem("entity.dept.isbuiltin", "ja-JP", "内置_jp", "内置（字典 sys_yes_no；0=否 1=是；种子部门为内置，不允许删除）"),
            // entity.dept.isbuiltin
            new TranslationSeedItem("entity.dept.isbuiltin", "zh-CN", "内置", "内置（字典 sys_yes_no；0=否 1=是；种子部门为内置，不允许删除）"),
            // entity.dept.isbuiltin
            new TranslationSeedItem("entity.dept.isbuiltin", "zh-HK", "内置_hk", "内置（字典 sys_yes_no；0=否 1=是；种子部门为内置，不允许删除）"),

            // entity.dept.description
            new TranslationSeedItem("entity.dept.description", "en-US", "部门描述_us", "部门描述"),
            // entity.dept.description
            new TranslationSeedItem("entity.dept.description", "ja-JP", "部门描述_jp", "部门描述"),
            // entity.dept.description
            new TranslationSeedItem("entity.dept.description", "zh-CN", "部门描述", "部门描述"),
            // entity.dept.description
            new TranslationSeedItem("entity.dept.description", "zh-HK", "部门描述_hk", "部门描述"),

            // entity.dept.sortorder
            new TranslationSeedItem("entity.dept.sortorder", "en-US", "排序号_us", "排序号（回填）（同级部门排序）"),
            // entity.dept.sortorder
            new TranslationSeedItem("entity.dept.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（同级部门排序）"),
            // entity.dept.sortorder
            new TranslationSeedItem("entity.dept.sortorder", "zh-CN", "排序号", "排序号（回填）（同级部门排序）"),
            // entity.dept.sortorder
            new TranslationSeedItem("entity.dept.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（同级部门排序）"),

            // entity.dept.status
            new TranslationSeedItem("entity.dept.status", "en-US", "状态_us", "状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）"),
            // entity.dept.status
            new TranslationSeedItem("entity.dept.status", "ja-JP", "状态_jp", "状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）"),
            // entity.dept.status
            new TranslationSeedItem("entity.dept.status", "zh-CN", "状态", "状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）"),
            // entity.dept.status
            new TranslationSeedItem("entity.dept.status", "zh-HK", "状态_hk", "状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）"),

            // entity.dept.roledepts
            new TranslationSeedItem("entity.dept.roledepts", "en-US", "角色数据权限关联该部门_us", "角色数据权限关联该部门（RBAC，表 takt_human_resource_organization_roledept）"),
            // entity.dept.roledepts
            new TranslationSeedItem("entity.dept.roledepts", "ja-JP", "角色数据权限关联该部门_jp", "角色数据权限关联该部门（RBAC，表 takt_human_resource_organization_roledept）"),
            // entity.dept.roledepts
            new TranslationSeedItem("entity.dept.roledepts", "zh-CN", "角色数据权限关联该部门", "角色数据权限关联该部门（RBAC，表 takt_human_resource_organization_roledept）"),
            // entity.dept.roledepts
            new TranslationSeedItem("entity.dept.roledepts", "zh-HK", "角色数据权限关联该部门_hk", "角色数据权限关联该部门（RBAC，表 takt_human_resource_organization_roledept）"),

            // entity.dept.employeedepts
            new TranslationSeedItem("entity.dept.employeedepts", "en-US", "员工部门关联_us", "员工部门关联（RBAC，表 takt_human_resource_organization_employeedept）"),
            // entity.dept.employeedepts
            new TranslationSeedItem("entity.dept.employeedepts", "ja-JP", "员工部门关联_jp", "员工部门关联（RBAC，表 takt_human_resource_organization_employeedept）"),
            // entity.dept.employeedepts
            new TranslationSeedItem("entity.dept.employeedepts", "zh-CN", "员工部门关联", "员工部门关联（RBAC，表 takt_human_resource_organization_employeedept）"),
            // entity.dept.employeedepts
            new TranslationSeedItem("entity.dept.employeedepts", "zh-HK", "员工部门关联_hk", "员工部门关联（RBAC，表 takt_human_resource_organization_employeedept）"),
        };
    }

    /// <summary>
    /// 填充 TaktTranslation 全部业务字段（含租户基类字段）
    /// </summary>
    private static void ApplyTranslationFields(
        TaktTranslation translation,
        string tenantCode,
        long cultureId,
        TranslationSeedItem item)
    {
        translation.TenantCode = tenantCode;
        translation.CultureId = cultureId;
        translation.CultureCode = item.CultureCode;
        translation.I18nKey = item.I18nKey;
        translation.TranslationText = item.TranslationText;
        translation.ResourceGroup = "Organization";
        translation.ResourceType = "frontend";
        translation.ContextNote = item.ContextNote;
        translation.ExtField = null;
        translation.Remark = null;
        translation.IsDeleted = 0;
        translation.DeletedBy = null;
        translation.DeletedAt = null;
    }

    private static async Task<(TaktTranslation Translation, int InsertCount, int UpdateCount)> CreateOrUpdateTranslationAsync(
        ITaktTenantSeedRepository<TaktTranslation> repository,
        string tenantCode,
        long cultureId,
        TranslationSeedItem item)
    {
        var translation = await repository.FirstAsync(t =>
            t.TenantCode == tenantCode &&
            t.I18nKey == item.I18nKey &&
            t.CultureCode == item.CultureCode);

        if (translation == null)
        {
            translation = new TaktTranslation();
            ApplyTranslationFields(translation, tenantCode, cultureId, item);
            translation = await repository.CreateAsync(translation);
            return (translation, 1, 0);
        }

        ApplyTranslationFields(translation, tenantCode, cultureId, item);
        await repository.UpdateAsync(translation);
        return (translation, 0, 1);
    }

    /// <summary>
    /// 翻译种子项（对应 TaktTranslation 全部可写字段，CultureId 由 SeedAsync 解析）
    /// </summary>
    private sealed record TranslationSeedItem(
        string I18nKey,
        string CultureCode,
        string TranslationText,
        string? ContextNote);
}
