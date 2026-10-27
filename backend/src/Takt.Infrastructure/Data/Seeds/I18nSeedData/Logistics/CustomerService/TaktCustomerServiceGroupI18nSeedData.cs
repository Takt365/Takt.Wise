// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.CustomerService
// 文件名称：TaktCustomerServiceGroupI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktCustomerServiceGroup 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.CustomerService;

/// <summary>
/// TaktCustomerServiceGroup 实体国际化翻译种子（键前缀 entity.customerservicegroup.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktCustomerServiceGroupI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktCustomerServiceGroup 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 customerservicegroup 实体翻译...", tenantCode);

        foreach (var item in GetCustomerServiceGroupTranslations())
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

        TaktLogger.Information("TaktCustomerServiceGroup 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktCustomerServiceGroup 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.customerservicegroup._self / entity.customerservicegroup.{{field}}；ResourceGroup=CustomerService；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetCustomerServiceGroupTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.customerservicegroup._self
            new TranslationSeedItem("entity.customerservicegroup._self", "en-US", "Customer Service Group Information_us", "实体名称"),
            // entity.customerservicegroup._self
            new TranslationSeedItem("entity.customerservicegroup._self", "ja-JP", "客户服务组主数据信息_jp", "实体名称"),
            // entity.customerservicegroup._self
            new TranslationSeedItem("entity.customerservicegroup._self", "zh-CN", "客户服务组主数据信息", "实体名称"),
            // entity.customerservicegroup._self
            new TranslationSeedItem("entity.customerservicegroup._self", "zh-HK", "客户服务组主数据信息_hk", "实体名称"),

            // entity.customerservicegroup.code
            new TranslationSeedItem("entity.customerservicegroup.code", "en-US", "客户服务组编码_us", "客户服务组编码（与员工编码同长 6；前端表单选择编码规则后自动通过 TaktNumbering 客户服务组编码规则生成并展示，非手输；单据类型菜单：客户服务组）"),
            // entity.customerservicegroup.code
            new TranslationSeedItem("entity.customerservicegroup.code", "ja-JP", "客户服务组编码_jp", "客户服务组编码（与员工编码同长 6；前端表单选择编码规则后自动通过 TaktNumbering 客户服务组编码规则生成并展示，非手输；单据类型菜单：客户服务组）"),
            // entity.customerservicegroup.code
            new TranslationSeedItem("entity.customerservicegroup.code", "zh-CN", "客户服务组编码", "客户服务组编码（与员工编码同长 6；前端表单选择编码规则后自动通过 TaktNumbering 客户服务组编码规则生成并展示，非手输；单据类型菜单：客户服务组）"),
            // entity.customerservicegroup.code
            new TranslationSeedItem("entity.customerservicegroup.code", "zh-HK", "客户服务组编码_hk", "客户服务组编码（与员工编码同长 6；前端表单选择编码规则后自动通过 TaktNumbering 客户服务组编码规则生成并展示，非手输；单据类型菜单：客户服务组）"),

            // entity.customerservicegroup.name
            new TranslationSeedItem("entity.customerservicegroup.name", "en-US", "客户服务组名称_us", "客户服务组名称"),
            // entity.customerservicegroup.name
            new TranslationSeedItem("entity.customerservicegroup.name", "ja-JP", "客户服务组名称_jp", "客户服务组名称"),
            // entity.customerservicegroup.name
            new TranslationSeedItem("entity.customerservicegroup.name", "zh-CN", "客户服务组名称", "客户服务组名称"),
            // entity.customerservicegroup.name
            new TranslationSeedItem("entity.customerservicegroup.name", "zh-HK", "客户服务组名称_hk", "客户服务组名称"),

            // entity.customerservicegroup.description
            new TranslationSeedItem("entity.customerservicegroup.description", "en-US", "客户服务组描述_us", "客户服务组描述"),
            // entity.customerservicegroup.description
            new TranslationSeedItem("entity.customerservicegroup.description", "ja-JP", "客户服务组描述_jp", "客户服务组描述"),
            // entity.customerservicegroup.description
            new TranslationSeedItem("entity.customerservicegroup.description", "zh-CN", "客户服务组描述", "客户服务组描述"),
            // entity.customerservicegroup.description
            new TranslationSeedItem("entity.customerservicegroup.description", "zh-HK", "客户服务组描述_hk", "客户服务组描述"),

            // entity.customerservicegroup.category
            new TranslationSeedItem("entity.customerservicegroup.category", "en-US", "客户服务组类别_us", "客户服务组类别（字典 logistics_customer_service_group_category；1=服务 2=工单）"),
            // entity.customerservicegroup.category
            new TranslationSeedItem("entity.customerservicegroup.category", "ja-JP", "客户服务组类别_jp", "客户服务组类别（字典 logistics_customer_service_group_category；1=服务 2=工单）"),
            // entity.customerservicegroup.category
            new TranslationSeedItem("entity.customerservicegroup.category", "zh-CN", "客户服务组类别", "客户服务组类别（字典 logistics_customer_service_group_category；1=服务 2=工单）"),
            // entity.customerservicegroup.category
            new TranslationSeedItem("entity.customerservicegroup.category", "zh-HK", "客户服务组类别_hk", "客户服务组类别（字典 logistics_customer_service_group_category；1=服务 2=工单）"),

            // entity.customerservicegroup.contactphone
            new TranslationSeedItem("entity.customerservicegroup.contactphone", "en-US", "联系电话_us", "联系电话"),
            // entity.customerservicegroup.contactphone
            new TranslationSeedItem("entity.customerservicegroup.contactphone", "ja-JP", "联系电话_jp", "联系电话"),
            // entity.customerservicegroup.contactphone
            new TranslationSeedItem("entity.customerservicegroup.contactphone", "zh-CN", "联系电话", "联系电话"),
            // entity.customerservicegroup.contactphone
            new TranslationSeedItem("entity.customerservicegroup.contactphone", "zh-HK", "联系电话_hk", "联系电话"),

            // entity.customerservicegroup.contactemail
            new TranslationSeedItem("entity.customerservicegroup.contactemail", "en-US", "联系邮箱_us", "联系邮箱"),
            // entity.customerservicegroup.contactemail
            new TranslationSeedItem("entity.customerservicegroup.contactemail", "ja-JP", "联系邮箱_jp", "联系邮箱"),
            // entity.customerservicegroup.contactemail
            new TranslationSeedItem("entity.customerservicegroup.contactemail", "zh-CN", "联系邮箱", "联系邮箱"),
            // entity.customerservicegroup.contactemail
            new TranslationSeedItem("entity.customerservicegroup.contactemail", "zh-HK", "联系邮箱_hk", "联系邮箱"),

            // entity.customerservicegroup.isbuiltin
            new TranslationSeedItem("entity.customerservicegroup.isbuiltin", "en-US", "内置_us", "内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）"),
            // entity.customerservicegroup.isbuiltin
            new TranslationSeedItem("entity.customerservicegroup.isbuiltin", "ja-JP", "内置_jp", "内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）"),
            // entity.customerservicegroup.isbuiltin
            new TranslationSeedItem("entity.customerservicegroup.isbuiltin", "zh-CN", "内置", "内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）"),
            // entity.customerservicegroup.isbuiltin
            new TranslationSeedItem("entity.customerservicegroup.isbuiltin", "zh-HK", "内置_hk", "内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）"),

            // entity.customerservicegroup.sortorder
            new TranslationSeedItem("entity.customerservicegroup.sortorder", "en-US", "排序号_us", "排序号（回填）（越小越靠前）"),
            // entity.customerservicegroup.sortorder
            new TranslationSeedItem("entity.customerservicegroup.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（越小越靠前）"),
            // entity.customerservicegroup.sortorder
            new TranslationSeedItem("entity.customerservicegroup.sortorder", "zh-CN", "排序号", "排序号（回填）（越小越靠前）"),
            // entity.customerservicegroup.sortorder
            new TranslationSeedItem("entity.customerservicegroup.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（越小越靠前）"),

            // entity.customerservicegroup.groupstatus
            new TranslationSeedItem("entity.customerservicegroup.groupstatus", "en-US", "客户服务组状态_us", "客户服务组状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.customerservicegroup.groupstatus
            new TranslationSeedItem("entity.customerservicegroup.groupstatus", "ja-JP", "客户服务组状态_jp", "客户服务组状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.customerservicegroup.groupstatus
            new TranslationSeedItem("entity.customerservicegroup.groupstatus", "zh-CN", "客户服务组状态", "客户服务组状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.customerservicegroup.groupstatus
            new TranslationSeedItem("entity.customerservicegroup.groupstatus", "zh-HK", "客户服务组状态_hk", "客户服务组状态（字典 sys_normal_disable；1=启用，0=禁用）"),
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
        translation.ResourceGroup = "CustomerService";
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
