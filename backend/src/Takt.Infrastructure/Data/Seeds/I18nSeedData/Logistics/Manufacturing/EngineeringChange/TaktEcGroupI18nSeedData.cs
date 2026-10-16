// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcGroupI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEcGroup 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// TaktEcGroup 实体国际化翻译种子（键前缀 entity.ecgroup.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEcGroupI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEcGroup 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 ecgroup 实体翻译...", tenantCode);

        foreach (var item in GetEcGroupTranslations())
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

        TaktLogger.Information("TaktEcGroup 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEcGroup 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.ecgroup._self / entity.ecgroup.{{field}}；ResourceGroup=EngineeringChange；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEcGroupTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.ecgroup._self
            new TranslationSeedItem("entity.ecgroup._self", "en-US", "Ec Group Information_us", "实体名称"),
            // entity.ecgroup._self
            new TranslationSeedItem("entity.ecgroup._self", "ja-JP", "设变组主数据信息_jp", "实体名称"),
            // entity.ecgroup._self
            new TranslationSeedItem("entity.ecgroup._self", "zh-CN", "设变组主数据信息", "实体名称"),
            // entity.ecgroup._self
            new TranslationSeedItem("entity.ecgroup._self", "zh-HK", "设变组主数据信息_hk", "实体名称"),

            // entity.ecgroup.code
            new TranslationSeedItem("entity.ecgroup.code", "en-US", "设变组编码_us", "设变组编码（与员工编码同长 6，如 D10000；前端表单选择编码规则后自动通过 TaktNumbering 设变组编码规则生成并展示，非手输；单据类型菜单：设变组）"),
            // entity.ecgroup.code
            new TranslationSeedItem("entity.ecgroup.code", "ja-JP", "设变组编码_jp", "设变组编码（与员工编码同长 6，如 D10000；前端表单选择编码规则后自动通过 TaktNumbering 设变组编码规则生成并展示，非手输；单据类型菜单：设变组）"),
            // entity.ecgroup.code
            new TranslationSeedItem("entity.ecgroup.code", "zh-CN", "设变组编码", "设变组编码（与员工编码同长 6，如 D10000；前端表单选择编码规则后自动通过 TaktNumbering 设变组编码规则生成并展示，非手输；单据类型菜单：设变组）"),
            // entity.ecgroup.code
            new TranslationSeedItem("entity.ecgroup.code", "zh-HK", "设变组编码_hk", "设变组编码（与员工编码同长 6，如 D10000；前端表单选择编码规则后自动通过 TaktNumbering 设变组编码规则生成并展示，非手输；单据类型菜单：设变组）"),

            // entity.ecgroup.name
            new TranslationSeedItem("entity.ecgroup.name", "en-US", "设变组名称_us", "设变组名称"),
            // entity.ecgroup.name
            new TranslationSeedItem("entity.ecgroup.name", "ja-JP", "设变组名称_jp", "设变组名称"),
            // entity.ecgroup.name
            new TranslationSeedItem("entity.ecgroup.name", "zh-CN", "设变组名称", "设变组名称"),
            // entity.ecgroup.name
            new TranslationSeedItem("entity.ecgroup.name", "zh-HK", "设变组名称_hk", "设变组名称"),

            // entity.ecgroup.description
            new TranslationSeedItem("entity.ecgroup.description", "en-US", "设变组描述_us", "设变组描述"),
            // entity.ecgroup.description
            new TranslationSeedItem("entity.ecgroup.description", "ja-JP", "设变组描述_jp", "设变组描述"),
            // entity.ecgroup.description
            new TranslationSeedItem("entity.ecgroup.description", "zh-CN", "设变组描述", "设变组描述"),
            // entity.ecgroup.description
            new TranslationSeedItem("entity.ecgroup.description", "zh-HK", "设变组描述_hk", "设变组描述"),

            // entity.ecgroup.category
            new TranslationSeedItem("entity.ecgroup.category", "en-US", "设变组类别_us", "设变组类别（选项 TaktDepts/options；DictValue=DeptCode，DictLabel=DeptName1）"),
            // entity.ecgroup.category
            new TranslationSeedItem("entity.ecgroup.category", "ja-JP", "设变组类别_jp", "设变组类别（选项 TaktDepts/options；DictValue=DeptCode，DictLabel=DeptName1）"),
            // entity.ecgroup.category
            new TranslationSeedItem("entity.ecgroup.category", "zh-CN", "设变组类别", "设变组类别（选项 TaktDepts/options；DictValue=DeptCode，DictLabel=DeptName1）"),
            // entity.ecgroup.category
            new TranslationSeedItem("entity.ecgroup.category", "zh-HK", "设变组类别_hk", "设变组类别（选项 TaktDepts/options；DictValue=DeptCode，DictLabel=DeptName1）"),

            // entity.ecgroup.contactphone
            new TranslationSeedItem("entity.ecgroup.contactphone", "en-US", "联系电话_us", "联系电话"),
            // entity.ecgroup.contactphone
            new TranslationSeedItem("entity.ecgroup.contactphone", "ja-JP", "联系电话_jp", "联系电话"),
            // entity.ecgroup.contactphone
            new TranslationSeedItem("entity.ecgroup.contactphone", "zh-CN", "联系电话", "联系电话"),
            // entity.ecgroup.contactphone
            new TranslationSeedItem("entity.ecgroup.contactphone", "zh-HK", "联系电话_hk", "联系电话"),

            // entity.ecgroup.contactemail
            new TranslationSeedItem("entity.ecgroup.contactemail", "en-US", "联系邮箱_us", "联系邮箱"),
            // entity.ecgroup.contactemail
            new TranslationSeedItem("entity.ecgroup.contactemail", "ja-JP", "联系邮箱_jp", "联系邮箱"),
            // entity.ecgroup.contactemail
            new TranslationSeedItem("entity.ecgroup.contactemail", "zh-CN", "联系邮箱", "联系邮箱"),
            // entity.ecgroup.contactemail
            new TranslationSeedItem("entity.ecgroup.contactemail", "zh-HK", "联系邮箱_hk", "联系邮箱"),

            // entity.ecgroup.isbuiltin
            new TranslationSeedItem("entity.ecgroup.isbuiltin", "en-US", "内置_us", "内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）"),
            // entity.ecgroup.isbuiltin
            new TranslationSeedItem("entity.ecgroup.isbuiltin", "ja-JP", "内置_jp", "内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）"),
            // entity.ecgroup.isbuiltin
            new TranslationSeedItem("entity.ecgroup.isbuiltin", "zh-CN", "内置", "内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）"),
            // entity.ecgroup.isbuiltin
            new TranslationSeedItem("entity.ecgroup.isbuiltin", "zh-HK", "内置_hk", "内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）"),

            // entity.ecgroup.sortorder
            new TranslationSeedItem("entity.ecgroup.sortorder", "en-US", "排序号_us", "排序号（回填）（越小越靠前）"),
            // entity.ecgroup.sortorder
            new TranslationSeedItem("entity.ecgroup.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（越小越靠前）"),
            // entity.ecgroup.sortorder
            new TranslationSeedItem("entity.ecgroup.sortorder", "zh-CN", "排序号", "排序号（回填）（越小越靠前）"),
            // entity.ecgroup.sortorder
            new TranslationSeedItem("entity.ecgroup.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（越小越靠前）"),

            // entity.ecgroup.groupstatus
            new TranslationSeedItem("entity.ecgroup.groupstatus", "en-US", "设变组状态_us", "设变组状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.ecgroup.groupstatus
            new TranslationSeedItem("entity.ecgroup.groupstatus", "ja-JP", "设变组状态_jp", "设变组状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.ecgroup.groupstatus
            new TranslationSeedItem("entity.ecgroup.groupstatus", "zh-CN", "设变组状态", "设变组状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.ecgroup.groupstatus
            new TranslationSeedItem("entity.ecgroup.groupstatus", "zh-HK", "设变组状态_hk", "设变组状态（字典 sys_normal_disable；1=启用，0=禁用）"),
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
        translation.ResourceGroup = "EngineeringChange";
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
