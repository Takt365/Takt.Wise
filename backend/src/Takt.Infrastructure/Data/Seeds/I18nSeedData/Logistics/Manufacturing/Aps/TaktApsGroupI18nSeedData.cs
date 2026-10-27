// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Aps
// 文件名称：TaktApsGroupI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktApsGroup 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Aps;

/// <summary>
/// TaktApsGroup 实体国际化翻译种子（键前缀 entity.apsgroup.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktApsGroupI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktApsGroup 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 apsgroup 实体翻译...", tenantCode);

        foreach (var item in GetApsGroupTranslations())
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

        TaktLogger.Information("TaktApsGroup 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktApsGroup 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.apsgroup._self / entity.apsgroup.{{field}}；ResourceGroup=Aps；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetApsGroupTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.apsgroup._self
            new TranslationSeedItem("entity.apsgroup._self", "en-US", "Aps Group Information_us", "实体名称"),
            // entity.apsgroup._self
            new TranslationSeedItem("entity.apsgroup._self", "ja-JP", "APS组主数据信息_jp", "实体名称"),
            // entity.apsgroup._self
            new TranslationSeedItem("entity.apsgroup._self", "zh-CN", "APS组主数据信息", "实体名称"),
            // entity.apsgroup._self
            new TranslationSeedItem("entity.apsgroup._self", "zh-HK", "APS组主数据信息_hk", "实体名称"),

            // entity.apsgroup.code
            new TranslationSeedItem("entity.apsgroup.code", "en-US", "APS组编码_us", "APS组编码（与员工编码同长 6；前端表单选择编码规则后自动通过 TaktNumbering APS组编码规则生成并展示，非手输；单据类型菜单：APS组）"),
            // entity.apsgroup.code
            new TranslationSeedItem("entity.apsgroup.code", "ja-JP", "APS组编码_jp", "APS组编码（与员工编码同长 6；前端表单选择编码规则后自动通过 TaktNumbering APS组编码规则生成并展示，非手输；单据类型菜单：APS组）"),
            // entity.apsgroup.code
            new TranslationSeedItem("entity.apsgroup.code", "zh-CN", "APS组编码", "APS组编码（与员工编码同长 6；前端表单选择编码规则后自动通过 TaktNumbering APS组编码规则生成并展示，非手输；单据类型菜单：APS组）"),
            // entity.apsgroup.code
            new TranslationSeedItem("entity.apsgroup.code", "zh-HK", "APS组编码_hk", "APS组编码（与员工编码同长 6；前端表单选择编码规则后自动通过 TaktNumbering APS组编码规则生成并展示，非手输；单据类型菜单：APS组）"),

            // entity.apsgroup.name
            new TranslationSeedItem("entity.apsgroup.name", "en-US", "APS组名称_us", "APS组名称"),
            // entity.apsgroup.name
            new TranslationSeedItem("entity.apsgroup.name", "ja-JP", "APS组名称_jp", "APS组名称"),
            // entity.apsgroup.name
            new TranslationSeedItem("entity.apsgroup.name", "zh-CN", "APS组名称", "APS组名称"),
            // entity.apsgroup.name
            new TranslationSeedItem("entity.apsgroup.name", "zh-HK", "APS组名称_hk", "APS组名称"),

            // entity.apsgroup.description
            new TranslationSeedItem("entity.apsgroup.description", "en-US", "APS组描述_us", "APS组描述"),
            // entity.apsgroup.description
            new TranslationSeedItem("entity.apsgroup.description", "ja-JP", "APS组描述_jp", "APS组描述"),
            // entity.apsgroup.description
            new TranslationSeedItem("entity.apsgroup.description", "zh-CN", "APS组描述", "APS组描述"),
            // entity.apsgroup.description
            new TranslationSeedItem("entity.apsgroup.description", "zh-HK", "APS组描述_hk", "APS组描述"),

            // entity.apsgroup.category
            new TranslationSeedItem("entity.apsgroup.category", "en-US", "APS组类别_us", "APS组类别（字典 logistics_manufacturing_aps_group_category；1=计划员 2=发布员 3=设备操作员 4=设备维护员）"),
            // entity.apsgroup.category
            new TranslationSeedItem("entity.apsgroup.category", "ja-JP", "APS组类别_jp", "APS组类别（字典 logistics_manufacturing_aps_group_category；1=计划员 2=发布员 3=设备操作员 4=设备维护员）"),
            // entity.apsgroup.category
            new TranslationSeedItem("entity.apsgroup.category", "zh-CN", "APS组类别", "APS组类别（字典 logistics_manufacturing_aps_group_category；1=计划员 2=发布员 3=设备操作员 4=设备维护员）"),
            // entity.apsgroup.category
            new TranslationSeedItem("entity.apsgroup.category", "zh-HK", "APS组类别_hk", "APS组类别（字典 logistics_manufacturing_aps_group_category；1=计划员 2=发布员 3=设备操作员 4=设备维护员）"),

            // entity.apsgroup.contactphone
            new TranslationSeedItem("entity.apsgroup.contactphone", "en-US", "联系电话_us", "联系电话"),
            // entity.apsgroup.contactphone
            new TranslationSeedItem("entity.apsgroup.contactphone", "ja-JP", "联系电话_jp", "联系电话"),
            // entity.apsgroup.contactphone
            new TranslationSeedItem("entity.apsgroup.contactphone", "zh-CN", "联系电话", "联系电话"),
            // entity.apsgroup.contactphone
            new TranslationSeedItem("entity.apsgroup.contactphone", "zh-HK", "联系电话_hk", "联系电话"),

            // entity.apsgroup.contactemail
            new TranslationSeedItem("entity.apsgroup.contactemail", "en-US", "联系邮箱_us", "联系邮箱"),
            // entity.apsgroup.contactemail
            new TranslationSeedItem("entity.apsgroup.contactemail", "ja-JP", "联系邮箱_jp", "联系邮箱"),
            // entity.apsgroup.contactemail
            new TranslationSeedItem("entity.apsgroup.contactemail", "zh-CN", "联系邮箱", "联系邮箱"),
            // entity.apsgroup.contactemail
            new TranslationSeedItem("entity.apsgroup.contactemail", "zh-HK", "联系邮箱_hk", "联系邮箱"),

            // entity.apsgroup.isbuiltin
            new TranslationSeedItem("entity.apsgroup.isbuiltin", "en-US", "内置_us", "内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）"),
            // entity.apsgroup.isbuiltin
            new TranslationSeedItem("entity.apsgroup.isbuiltin", "ja-JP", "内置_jp", "内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）"),
            // entity.apsgroup.isbuiltin
            new TranslationSeedItem("entity.apsgroup.isbuiltin", "zh-CN", "内置", "内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）"),
            // entity.apsgroup.isbuiltin
            new TranslationSeedItem("entity.apsgroup.isbuiltin", "zh-HK", "内置_hk", "内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）"),

            // entity.apsgroup.sortorder
            new TranslationSeedItem("entity.apsgroup.sortorder", "en-US", "排序号_us", "排序号（回填）（越小越靠前）"),
            // entity.apsgroup.sortorder
            new TranslationSeedItem("entity.apsgroup.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（越小越靠前）"),
            // entity.apsgroup.sortorder
            new TranslationSeedItem("entity.apsgroup.sortorder", "zh-CN", "排序号", "排序号（回填）（越小越靠前）"),
            // entity.apsgroup.sortorder
            new TranslationSeedItem("entity.apsgroup.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（越小越靠前）"),

            // entity.apsgroup.groupstatus
            new TranslationSeedItem("entity.apsgroup.groupstatus", "en-US", "APS组状态_us", "APS组状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.apsgroup.groupstatus
            new TranslationSeedItem("entity.apsgroup.groupstatus", "ja-JP", "APS组状态_jp", "APS组状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.apsgroup.groupstatus
            new TranslationSeedItem("entity.apsgroup.groupstatus", "zh-CN", "APS组状态", "APS组状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.apsgroup.groupstatus
            new TranslationSeedItem("entity.apsgroup.groupstatus", "zh-HK", "APS组状态_hk", "APS组状态（字典 sys_normal_disable；1=启用，0=禁用）"),
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
        translation.ResourceGroup = "Aps";
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
