// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Controlling
// 文件名称：TaktProfitCenterI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktProfitCenter 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Controlling;

/// <summary>
/// TaktProfitCenter 实体国际化翻译种子（键前缀 entity.profitcenter.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktProfitCenterI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktProfitCenter 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 profitcenter 实体翻译...", tenantCode);

        foreach (var item in GetProfitCenterTranslations())
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

        TaktLogger.Information("TaktProfitCenter 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktProfitCenter 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.profitcenter._self / entity.profitcenter.{{field}}；ResourceGroup=Controlling；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetProfitCenterTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.profitcenter._self
            new TranslationSeedItem("entity.profitcenter._self", "en-US", "Profit Center Information_us", "实体名称"),
            // entity.profitcenter._self
            new TranslationSeedItem("entity.profitcenter._self", "ja-JP", "利润中心信息_jp", "实体名称"),
            // entity.profitcenter._self
            new TranslationSeedItem("entity.profitcenter._self", "zh-CN", "利润中心信息", "实体名称"),
            // entity.profitcenter._self
            new TranslationSeedItem("entity.profitcenter._self", "zh-HK", "利润中心信息_hk", "实体名称"),

            // entity.profitcenter.code
            new TranslationSeedItem("entity.profitcenter.code", "en-US", "利润中心编码_us", "利润中心编码（4位，租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 利润中心编码规则生成并展示，非手输；单据类型菜单：利润中心）"),
            // entity.profitcenter.code
            new TranslationSeedItem("entity.profitcenter.code", "ja-JP", "利润中心编码_jp", "利润中心编码（4位，租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 利润中心编码规则生成并展示，非手输；单据类型菜单：利润中心）"),
            // entity.profitcenter.code
            new TranslationSeedItem("entity.profitcenter.code", "zh-CN", "利润中心编码", "利润中心编码（4位，租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 利润中心编码规则生成并展示，非手输；单据类型菜单：利润中心）"),
            // entity.profitcenter.code
            new TranslationSeedItem("entity.profitcenter.code", "zh-HK", "利润中心编码_hk", "利润中心编码（4位，租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 利润中心编码规则生成并展示，非手输；单据类型菜单：利润中心）"),

            // entity.profitcenter.name
            new TranslationSeedItem("entity.profitcenter.name", "en-US", "利润中心名称_us", "利润中心名称"),
            // entity.profitcenter.name
            new TranslationSeedItem("entity.profitcenter.name", "ja-JP", "利润中心名称_jp", "利润中心名称"),
            // entity.profitcenter.name
            new TranslationSeedItem("entity.profitcenter.name", "zh-CN", "利润中心名称", "利润中心名称"),
            // entity.profitcenter.name
            new TranslationSeedItem("entity.profitcenter.name", "zh-HK", "利润中心名称_hk", "利润中心名称"),

            // entity.profitcenter.parentid
            new TranslationSeedItem("entity.profitcenter.parentid", "en-US", "父级ID_us", "父级 ID"),
            // entity.profitcenter.parentid
            new TranslationSeedItem("entity.profitcenter.parentid", "ja-JP", "父级ID_jp", "父级 ID"),
            // entity.profitcenter.parentid
            new TranslationSeedItem("entity.profitcenter.parentid", "zh-CN", "父级ID", "父级 ID"),
            // entity.profitcenter.parentid
            new TranslationSeedItem("entity.profitcenter.parentid", "zh-HK", "父级ID_hk", "父级 ID"),

            // entity.profitcenter.managerid
            new TranslationSeedItem("entity.profitcenter.managerid", "en-US", "负责人ID_us", "负责人用户 ID（选项 TaktUsers/options，DictValue=Id）"),
            // entity.profitcenter.managerid
            new TranslationSeedItem("entity.profitcenter.managerid", "ja-JP", "负责人ID_jp", "负责人用户 ID（选项 TaktUsers/options，DictValue=Id）"),
            // entity.profitcenter.managerid
            new TranslationSeedItem("entity.profitcenter.managerid", "zh-CN", "负责人ID", "负责人用户 ID（选项 TaktUsers/options，DictValue=Id）"),
            // entity.profitcenter.managerid
            new TranslationSeedItem("entity.profitcenter.managerid", "zh-HK", "负责人ID_hk", "负责人用户 ID（选项 TaktUsers/options，DictValue=Id）"),

            // entity.profitcenter.managername
            new TranslationSeedItem("entity.profitcenter.managername", "en-US", "负责人姓名_us", "负责人姓名（冗余：按 ManagerId 取 TaktUser.NickName联动）"),
            // entity.profitcenter.managername
            new TranslationSeedItem("entity.profitcenter.managername", "ja-JP", "负责人姓名_jp", "负责人姓名（冗余：按 ManagerId 取 TaktUser.NickName联动）"),
            // entity.profitcenter.managername
            new TranslationSeedItem("entity.profitcenter.managername", "zh-CN", "负责人姓名", "负责人姓名（冗余：按 ManagerId 取 TaktUser.NickName联动）"),
            // entity.profitcenter.managername
            new TranslationSeedItem("entity.profitcenter.managername", "zh-HK", "负责人姓名_hk", "负责人姓名（冗余：按 ManagerId 取 TaktUser.NickName联动）"),

            // entity.profitcenter.deptid
            new TranslationSeedItem("entity.profitcenter.deptid", "en-US", "所属部门ID_us", "所属部门（选项 TaktDepts/tree-options,DictValue=Id）"),
            // entity.profitcenter.deptid
            new TranslationSeedItem("entity.profitcenter.deptid", "ja-JP", "所属部门ID_jp", "所属部门（选项 TaktDepts/tree-options,DictValue=Id）"),
            // entity.profitcenter.deptid
            new TranslationSeedItem("entity.profitcenter.deptid", "zh-CN", "所属部门ID", "所属部门（选项 TaktDepts/tree-options,DictValue=Id）"),
            // entity.profitcenter.deptid
            new TranslationSeedItem("entity.profitcenter.deptid", "zh-HK", "所属部门ID_hk", "所属部门（选项 TaktDepts/tree-options,DictValue=Id）"),

            // entity.profitcenter.deptname
            new TranslationSeedItem("entity.profitcenter.deptname", "en-US", "所属部门名称_us", "所属部门名称（冗余：按 DeptId 取 TaktDept.DeptName联动）"),
            // entity.profitcenter.deptname
            new TranslationSeedItem("entity.profitcenter.deptname", "ja-JP", "所属部门名称_jp", "所属部门名称（冗余：按 DeptId 取 TaktDept.DeptName联动）"),
            // entity.profitcenter.deptname
            new TranslationSeedItem("entity.profitcenter.deptname", "zh-CN", "所属部门名称", "所属部门名称（冗余：按 DeptId 取 TaktDept.DeptName联动）"),
            // entity.profitcenter.deptname
            new TranslationSeedItem("entity.profitcenter.deptname", "zh-HK", "所属部门名称_hk", "所属部门名称（冗余：按 DeptId 取 TaktDept.DeptName联动）"),

            // entity.profitcenter.level
            new TranslationSeedItem("entity.profitcenter.level", "en-US", "利润中心层级_us", "利润中心层级"),
            // entity.profitcenter.level
            new TranslationSeedItem("entity.profitcenter.level", "ja-JP", "利润中心层级_jp", "利润中心层级"),
            // entity.profitcenter.level
            new TranslationSeedItem("entity.profitcenter.level", "zh-CN", "利润中心层级", "利润中心层级"),
            // entity.profitcenter.level
            new TranslationSeedItem("entity.profitcenter.level", "zh-HK", "利润中心层级_hk", "利润中心层级"),

            // entity.profitcenter.validfrom
            new TranslationSeedItem("entity.profitcenter.validfrom", "en-US", "生效日期_us", "生效日期"),
            // entity.profitcenter.validfrom
            new TranslationSeedItem("entity.profitcenter.validfrom", "ja-JP", "生效日期_jp", "生效日期"),
            // entity.profitcenter.validfrom
            new TranslationSeedItem("entity.profitcenter.validfrom", "zh-CN", "生效日期", "生效日期"),
            // entity.profitcenter.validfrom
            new TranslationSeedItem("entity.profitcenter.validfrom", "zh-HK", "生效日期_hk", "生效日期"),

            // entity.profitcenter.validto
            new TranslationSeedItem("entity.profitcenter.validto", "en-US", "失效日期_us", "失效日期"),
            // entity.profitcenter.validto
            new TranslationSeedItem("entity.profitcenter.validto", "ja-JP", "失效日期_jp", "失效日期"),
            // entity.profitcenter.validto
            new TranslationSeedItem("entity.profitcenter.validto", "zh-CN", "失效日期", "失效日期"),
            // entity.profitcenter.validto
            new TranslationSeedItem("entity.profitcenter.validto", "zh-HK", "失效日期_hk", "失效日期"),

            // entity.profitcenter.sortorder
            new TranslationSeedItem("entity.profitcenter.sortorder", "en-US", "排序号_us", "排序号（回填）"),
            // entity.profitcenter.sortorder
            new TranslationSeedItem("entity.profitcenter.sortorder", "ja-JP", "排序号_jp", "排序号（回填）"),
            // entity.profitcenter.sortorder
            new TranslationSeedItem("entity.profitcenter.sortorder", "zh-CN", "排序号", "排序号（回填）"),
            // entity.profitcenter.sortorder
            new TranslationSeedItem("entity.profitcenter.sortorder", "zh-HK", "排序号_hk", "排序号（回填）"),

            // entity.profitcenter.status
            new TranslationSeedItem("entity.profitcenter.status", "en-US", "利润中心状态_us", "利润中心状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.profitcenter.status
            new TranslationSeedItem("entity.profitcenter.status", "ja-JP", "利润中心状态_jp", "利润中心状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.profitcenter.status
            new TranslationSeedItem("entity.profitcenter.status", "zh-CN", "利润中心状态", "利润中心状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.profitcenter.status
            new TranslationSeedItem("entity.profitcenter.status", "zh-HK", "利润中心状态_hk", "利润中心状态（字典 sys_normal_disable；1=启用，0=禁用）"),
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
        translation.ResourceGroup = "Controlling";
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
