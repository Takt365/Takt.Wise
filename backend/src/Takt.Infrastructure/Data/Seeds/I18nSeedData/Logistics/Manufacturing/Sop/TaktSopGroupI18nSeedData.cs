// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Sop
// 文件名称：TaktSopGroupI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSopGroup 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Sop;

/// <summary>
/// TaktSopGroup 实体国际化翻译种子（键前缀 entity.sopgroup.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSopGroupI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSopGroup 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 sopgroup 实体翻译...", tenantCode);

        foreach (var item in GetSopGroupTranslations())
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

        TaktLogger.Information("TaktSopGroup 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSopGroup 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.sopgroup._self / entity.sopgroup.{{field}}；ResourceGroup=Sop；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSopGroupTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.sopgroup._self
            new TranslationSeedItem("entity.sopgroup._self", "en-US", "Sop Group Information_us", "实体名称"),
            // entity.sopgroup._self
            new TranslationSeedItem("entity.sopgroup._self", "ja-JP", "SOP组主数据信息_jp", "实体名称"),
            // entity.sopgroup._self
            new TranslationSeedItem("entity.sopgroup._self", "zh-CN", "SOP组主数据信息", "实体名称"),
            // entity.sopgroup._self
            new TranslationSeedItem("entity.sopgroup._self", "zh-HK", "SOP组主数据信息_hk", "实体名称"),

            // entity.sopgroup.code
            new TranslationSeedItem("entity.sopgroup.code", "en-US", "SOP组编码_us", "SOP组编码（与员工编码同长 6）"),
            // entity.sopgroup.code
            new TranslationSeedItem("entity.sopgroup.code", "ja-JP", "SOP组编码_jp", "SOP组编码（与员工编码同长 6）"),
            // entity.sopgroup.code
            new TranslationSeedItem("entity.sopgroup.code", "zh-CN", "SOP组编码", "SOP组编码（与员工编码同长 6）"),
            // entity.sopgroup.code
            new TranslationSeedItem("entity.sopgroup.code", "zh-HK", "SOP组编码_hk", "SOP组编码（与员工编码同长 6）"),

            // entity.sopgroup.name
            new TranslationSeedItem("entity.sopgroup.name", "en-US", "SOP组名称_us", "SOP组名称"),
            // entity.sopgroup.name
            new TranslationSeedItem("entity.sopgroup.name", "ja-JP", "SOP组名称_jp", "SOP组名称"),
            // entity.sopgroup.name
            new TranslationSeedItem("entity.sopgroup.name", "zh-CN", "SOP组名称", "SOP组名称"),
            // entity.sopgroup.name
            new TranslationSeedItem("entity.sopgroup.name", "zh-HK", "SOP组名称_hk", "SOP组名称"),

            // entity.sopgroup.description
            new TranslationSeedItem("entity.sopgroup.description", "en-US", "SOP组描述_us", "SOP组描述"),
            // entity.sopgroup.description
            new TranslationSeedItem("entity.sopgroup.description", "ja-JP", "SOP组描述_jp", "SOP组描述"),
            // entity.sopgroup.description
            new TranslationSeedItem("entity.sopgroup.description", "zh-CN", "SOP组描述", "SOP组描述"),
            // entity.sopgroup.description
            new TranslationSeedItem("entity.sopgroup.description", "zh-HK", "SOP组描述_hk", "SOP组描述"),

            // entity.sopgroup.category
            new TranslationSeedItem("entity.sopgroup.category", "en-US", "SOP组类别_us", "SOP组类别（字典 logistics_manufacturing_sop_group_category；1=工位 2=文档 3=版本 4=确认 5=执行 6=扫码 7=ESD 8=安灯）"),
            // entity.sopgroup.category
            new TranslationSeedItem("entity.sopgroup.category", "ja-JP", "SOP组类别_jp", "SOP组类别（字典 logistics_manufacturing_sop_group_category；1=工位 2=文档 3=版本 4=确认 5=执行 6=扫码 7=ESD 8=安灯）"),
            // entity.sopgroup.category
            new TranslationSeedItem("entity.sopgroup.category", "zh-CN", "SOP组类别", "SOP组类别（字典 logistics_manufacturing_sop_group_category；1=工位 2=文档 3=版本 4=确认 5=执行 6=扫码 7=ESD 8=安灯）"),
            // entity.sopgroup.category
            new TranslationSeedItem("entity.sopgroup.category", "zh-HK", "SOP组类别_hk", "SOP组类别（字典 logistics_manufacturing_sop_group_category；1=工位 2=文档 3=版本 4=确认 5=执行 6=扫码 7=ESD 8=安灯）"),

            // entity.sopgroup.contactphone
            new TranslationSeedItem("entity.sopgroup.contactphone", "en-US", "联系电话_us", "联系电话"),
            // entity.sopgroup.contactphone
            new TranslationSeedItem("entity.sopgroup.contactphone", "ja-JP", "联系电话_jp", "联系电话"),
            // entity.sopgroup.contactphone
            new TranslationSeedItem("entity.sopgroup.contactphone", "zh-CN", "联系电话", "联系电话"),
            // entity.sopgroup.contactphone
            new TranslationSeedItem("entity.sopgroup.contactphone", "zh-HK", "联系电话_hk", "联系电话"),

            // entity.sopgroup.contactemail
            new TranslationSeedItem("entity.sopgroup.contactemail", "en-US", "联系邮箱_us", "联系邮箱"),
            // entity.sopgroup.contactemail
            new TranslationSeedItem("entity.sopgroup.contactemail", "ja-JP", "联系邮箱_jp", "联系邮箱"),
            // entity.sopgroup.contactemail
            new TranslationSeedItem("entity.sopgroup.contactemail", "zh-CN", "联系邮箱", "联系邮箱"),
            // entity.sopgroup.contactemail
            new TranslationSeedItem("entity.sopgroup.contactemail", "zh-HK", "联系邮箱_hk", "联系邮箱"),

            // entity.sopgroup.isbuiltin
            new TranslationSeedItem("entity.sopgroup.isbuiltin", "en-US", "内置_us", "内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）"),
            // entity.sopgroup.isbuiltin
            new TranslationSeedItem("entity.sopgroup.isbuiltin", "ja-JP", "内置_jp", "内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）"),
            // entity.sopgroup.isbuiltin
            new TranslationSeedItem("entity.sopgroup.isbuiltin", "zh-CN", "内置", "内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）"),
            // entity.sopgroup.isbuiltin
            new TranslationSeedItem("entity.sopgroup.isbuiltin", "zh-HK", "内置_hk", "内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）"),

            // entity.sopgroup.sortorder
            new TranslationSeedItem("entity.sopgroup.sortorder", "en-US", "排序号_us", "排序号（回填）（越小越靠前）"),
            // entity.sopgroup.sortorder
            new TranslationSeedItem("entity.sopgroup.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（越小越靠前）"),
            // entity.sopgroup.sortorder
            new TranslationSeedItem("entity.sopgroup.sortorder", "zh-CN", "排序号", "排序号（回填）（越小越靠前）"),
            // entity.sopgroup.sortorder
            new TranslationSeedItem("entity.sopgroup.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（越小越靠前）"),

            // entity.sopgroup.groupstatus
            new TranslationSeedItem("entity.sopgroup.groupstatus", "en-US", "SOP组状态_us", "SOP组状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.sopgroup.groupstatus
            new TranslationSeedItem("entity.sopgroup.groupstatus", "ja-JP", "SOP组状态_jp", "SOP组状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.sopgroup.groupstatus
            new TranslationSeedItem("entity.sopgroup.groupstatus", "zh-CN", "SOP组状态", "SOP组状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.sopgroup.groupstatus
            new TranslationSeedItem("entity.sopgroup.groupstatus", "zh-HK", "SOP组状态_hk", "SOP组状态（字典 sys_normal_disable；1=启用，0=禁用）"),
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
        translation.ResourceGroup = "Sop";
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
