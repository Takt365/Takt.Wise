// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Bom
// 文件名称：TaktBomGroupI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktBomGroup 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Bom;

/// <summary>
/// TaktBomGroup 实体国际化翻译种子（键前缀 entity.bomgroup.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktBomGroupI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktBomGroup 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 bomgroup 实体翻译...", tenantCode);

        foreach (var item in GetBomGroupTranslations())
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

        TaktLogger.Information("TaktBomGroup 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktBomGroup 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.bomgroup._self / entity.bomgroup.{{field}}；ResourceGroup=Bom；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetBomGroupTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.bomgroup._self
            new TranslationSeedItem("entity.bomgroup._self", "en-US", "Bom Group Information_us", "实体名称"),
            // entity.bomgroup._self
            new TranslationSeedItem("entity.bomgroup._self", "ja-JP", "BOM组主数据信息_jp", "实体名称"),
            // entity.bomgroup._self
            new TranslationSeedItem("entity.bomgroup._self", "zh-CN", "BOM组主数据信息", "实体名称"),
            // entity.bomgroup._self
            new TranslationSeedItem("entity.bomgroup._self", "zh-HK", "BOM组主数据信息_hk", "实体名称"),

            // entity.bomgroup.code
            new TranslationSeedItem("entity.bomgroup.code", "en-US", "BOM组编码_us", "BOM组编码（与员工编码同长 6；前端表单选择编码规则后自动通过 TaktNumbering BOM组编码规则生成并展示，非手输；单据类型菜单：BOM组）"),
            // entity.bomgroup.code
            new TranslationSeedItem("entity.bomgroup.code", "ja-JP", "BOM组编码_jp", "BOM组编码（与员工编码同长 6；前端表单选择编码规则后自动通过 TaktNumbering BOM组编码规则生成并展示，非手输；单据类型菜单：BOM组）"),
            // entity.bomgroup.code
            new TranslationSeedItem("entity.bomgroup.code", "zh-CN", "BOM组编码", "BOM组编码（与员工编码同长 6；前端表单选择编码规则后自动通过 TaktNumbering BOM组编码规则生成并展示，非手输；单据类型菜单：BOM组）"),
            // entity.bomgroup.code
            new TranslationSeedItem("entity.bomgroup.code", "zh-HK", "BOM组编码_hk", "BOM组编码（与员工编码同长 6；前端表单选择编码规则后自动通过 TaktNumbering BOM组编码规则生成并展示，非手输；单据类型菜单：BOM组）"),

            // entity.bomgroup.name
            new TranslationSeedItem("entity.bomgroup.name", "en-US", "BOM组名称_us", "BOM组名称"),
            // entity.bomgroup.name
            new TranslationSeedItem("entity.bomgroup.name", "ja-JP", "BOM组名称_jp", "BOM组名称"),
            // entity.bomgroup.name
            new TranslationSeedItem("entity.bomgroup.name", "zh-CN", "BOM组名称", "BOM组名称"),
            // entity.bomgroup.name
            new TranslationSeedItem("entity.bomgroup.name", "zh-HK", "BOM组名称_hk", "BOM组名称"),

            // entity.bomgroup.description
            new TranslationSeedItem("entity.bomgroup.description", "en-US", "BOM组描述_us", "BOM组描述"),
            // entity.bomgroup.description
            new TranslationSeedItem("entity.bomgroup.description", "ja-JP", "BOM组描述_jp", "BOM组描述"),
            // entity.bomgroup.description
            new TranslationSeedItem("entity.bomgroup.description", "zh-CN", "BOM组描述", "BOM组描述"),
            // entity.bomgroup.description
            new TranslationSeedItem("entity.bomgroup.description", "zh-HK", "BOM组描述_hk", "BOM组描述"),

            // entity.bomgroup.category
            new TranslationSeedItem("entity.bomgroup.category", "en-US", "BOM组类别_us", "BOM组类别（字典 logistics_manufacturing_bom_group_category；1=BOM 2=工艺）"),
            // entity.bomgroup.category
            new TranslationSeedItem("entity.bomgroup.category", "ja-JP", "BOM组类别_jp", "BOM组类别（字典 logistics_manufacturing_bom_group_category；1=BOM 2=工艺）"),
            // entity.bomgroup.category
            new TranslationSeedItem("entity.bomgroup.category", "zh-CN", "BOM组类别", "BOM组类别（字典 logistics_manufacturing_bom_group_category；1=BOM 2=工艺）"),
            // entity.bomgroup.category
            new TranslationSeedItem("entity.bomgroup.category", "zh-HK", "BOM组类别_hk", "BOM组类别（字典 logistics_manufacturing_bom_group_category；1=BOM 2=工艺）"),

            // entity.bomgroup.contactphone
            new TranslationSeedItem("entity.bomgroup.contactphone", "en-US", "联系电话_us", "联系电话"),
            // entity.bomgroup.contactphone
            new TranslationSeedItem("entity.bomgroup.contactphone", "ja-JP", "联系电话_jp", "联系电话"),
            // entity.bomgroup.contactphone
            new TranslationSeedItem("entity.bomgroup.contactphone", "zh-CN", "联系电话", "联系电话"),
            // entity.bomgroup.contactphone
            new TranslationSeedItem("entity.bomgroup.contactphone", "zh-HK", "联系电话_hk", "联系电话"),

            // entity.bomgroup.contactemail
            new TranslationSeedItem("entity.bomgroup.contactemail", "en-US", "联系邮箱_us", "联系邮箱"),
            // entity.bomgroup.contactemail
            new TranslationSeedItem("entity.bomgroup.contactemail", "ja-JP", "联系邮箱_jp", "联系邮箱"),
            // entity.bomgroup.contactemail
            new TranslationSeedItem("entity.bomgroup.contactemail", "zh-CN", "联系邮箱", "联系邮箱"),
            // entity.bomgroup.contactemail
            new TranslationSeedItem("entity.bomgroup.contactemail", "zh-HK", "联系邮箱_hk", "联系邮箱"),

            // entity.bomgroup.isbuiltin
            new TranslationSeedItem("entity.bomgroup.isbuiltin", "en-US", "内置_us", "内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）"),
            // entity.bomgroup.isbuiltin
            new TranslationSeedItem("entity.bomgroup.isbuiltin", "ja-JP", "内置_jp", "内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）"),
            // entity.bomgroup.isbuiltin
            new TranslationSeedItem("entity.bomgroup.isbuiltin", "zh-CN", "内置", "内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）"),
            // entity.bomgroup.isbuiltin
            new TranslationSeedItem("entity.bomgroup.isbuiltin", "zh-HK", "内置_hk", "内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）"),

            // entity.bomgroup.sortorder
            new TranslationSeedItem("entity.bomgroup.sortorder", "en-US", "排序号_us", "排序号（回填）（越小越靠前）"),
            // entity.bomgroup.sortorder
            new TranslationSeedItem("entity.bomgroup.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（越小越靠前）"),
            // entity.bomgroup.sortorder
            new TranslationSeedItem("entity.bomgroup.sortorder", "zh-CN", "排序号", "排序号（回填）（越小越靠前）"),
            // entity.bomgroup.sortorder
            new TranslationSeedItem("entity.bomgroup.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（越小越靠前）"),

            // entity.bomgroup.groupstatus
            new TranslationSeedItem("entity.bomgroup.groupstatus", "en-US", "BOM组状态_us", "BOM组状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.bomgroup.groupstatus
            new TranslationSeedItem("entity.bomgroup.groupstatus", "ja-JP", "BOM组状态_jp", "BOM组状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.bomgroup.groupstatus
            new TranslationSeedItem("entity.bomgroup.groupstatus", "zh-CN", "BOM组状态", "BOM组状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.bomgroup.groupstatus
            new TranslationSeedItem("entity.bomgroup.groupstatus", "zh-HK", "BOM组状态_hk", "BOM组状态（字典 sys_normal_disable；1=启用，0=禁用）"),
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
        translation.ResourceGroup = "Bom";
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
