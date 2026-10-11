// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Statistics.QuickQuery
// 文件名称：TaktConfigurableI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktConfigurable 实体字段国际化种子（已对齐前端 locales：src/locales/statistics/quick-query/configurable）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Statistics.QuickQuery;

/// <summary>
/// TaktConfigurable 实体国际化翻译种子（键前缀 entity.configurable.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktConfigurableI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktConfigurable 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 configurable 实体翻译...", tenantCode);

        foreach (var item in GetConfigurableTranslations())
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

        TaktLogger.Information("TaktConfigurable 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktConfigurable 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.configurable._self / entity.configurable.{{field}}；ResourceGroup=QuickQuery；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetConfigurableTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.configurable._self
            new TranslationSeedItem("entity.configurable._self", "en-US", "Configurable Information_us", "实体名称"),
            // entity.configurable._self
            new TranslationSeedItem("entity.configurable._self", "ja-JP", "定制报表主信息_jp", "实体名称"),
            // entity.configurable._self
            new TranslationSeedItem("entity.configurable._self", "zh-CN", "定制报表主信息", "实体名称"),
            // entity.configurable._self
            new TranslationSeedItem("entity.configurable._self", "zh-HK", "定制报表主信息_hk", "实体名称"),

            // entity.configurable.code
            new TranslationSeedItem("entity.configurable.code", "en-US", "定制报表编码_us", "定制报表编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 定制报表编码规则生成并展示，非手输；单据类型菜单：定制报表）"),
            // entity.configurable.code
            new TranslationSeedItem("entity.configurable.code", "ja-JP", "定制报表编码_jp", "定制报表编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 定制报表编码规则生成并展示，非手输；单据类型菜单：定制报表）"),
            // entity.configurable.code
            new TranslationSeedItem("entity.configurable.code", "zh-CN", "定制报表编码", "定制报表编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 定制报表编码规则生成并展示，非手输；单据类型菜单：定制报表）"),
            // entity.configurable.code
            new TranslationSeedItem("entity.configurable.code", "zh-HK", "定制报表编码_hk", "定制报表编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 定制报表编码规则生成并展示，非手输；单据类型菜单：定制报表）"),

            // entity.configurable.name
            new TranslationSeedItem("entity.configurable.name", "en-US", "定制报表名称_us", "定制报表名称"),
            // entity.configurable.name
            new TranslationSeedItem("entity.configurable.name", "ja-JP", "定制报表名称_jp", "定制报表名称"),
            // entity.configurable.name
            new TranslationSeedItem("entity.configurable.name", "zh-CN", "定制报表名称", "定制报表名称"),
            // entity.configurable.name
            new TranslationSeedItem("entity.configurable.name", "zh-HK", "定制报表名称_hk", "定制报表名称"),

            // entity.configurable.domain
            new TranslationSeedItem("entity.configurable.domain", "en-US", "定制报表业务域_us", "定制报表业务域（TaktModule；0=仪表盘 1=身份认证 2=日常事务 3=财务核算 4=后勤管理 5=人力资源 6=工作流 7=代码管理 8=基础设置 9=统计看板 10=实体；与一级菜单映射）"),
            // entity.configurable.domain
            new TranslationSeedItem("entity.configurable.domain", "ja-JP", "定制报表业务域_jp", "定制报表业务域（TaktModule；0=仪表盘 1=身份认证 2=日常事务 3=财务核算 4=后勤管理 5=人力资源 6=工作流 7=代码管理 8=基础设置 9=统计看板 10=实体；与一级菜单映射）"),
            // entity.configurable.domain
            new TranslationSeedItem("entity.configurable.domain", "zh-CN", "定制报表业务域", "定制报表业务域（TaktModule；0=仪表盘 1=身份认证 2=日常事务 3=财务核算 4=后勤管理 5=人力资源 6=工作流 7=代码管理 8=基础设置 9=统计看板 10=实体；与一级菜单映射）"),
            // entity.configurable.domain
            new TranslationSeedItem("entity.configurable.domain", "zh-HK", "定制报表业务域_hk", "定制报表业务域（TaktModule；0=仪表盘 1=身份认证 2=日常事务 3=财务核算 4=后勤管理 5=人力资源 6=工作流 7=代码管理 8=基础设置 9=统计看板 10=实体；与一级菜单映射）"),

            // entity.configurable.subcategory
            new TranslationSeedItem("entity.configurable.subcategory", "en-US", "定制报表子分类_us", "定制报表子分类（与菜单末级路由段对齐，如 management、controlling、material）"),
            // entity.configurable.subcategory
            new TranslationSeedItem("entity.configurable.subcategory", "ja-JP", "定制报表子分类_jp", "定制报表子分类（与菜单末级路由段对齐，如 management、controlling、material）"),
            // entity.configurable.subcategory
            new TranslationSeedItem("entity.configurable.subcategory", "zh-CN", "定制报表子分类", "定制报表子分类（与菜单末级路由段对齐，如 management、controlling、material）"),
            // entity.configurable.subcategory
            new TranslationSeedItem("entity.configurable.subcategory", "zh-HK", "定制报表子分类_hk", "定制报表子分类（与菜单末级路由段对齐，如 management、controlling、material）"),

            // entity.configurable.distinctrows
            new TranslationSeedItem("entity.configurable.distinctrows", "en-US", "是否去重_us", "是否去重行（字典 sys_yes_no；0=否 1=是；SELECT DISTINCT）"),
            // entity.configurable.distinctrows
            new TranslationSeedItem("entity.configurable.distinctrows", "ja-JP", "是否去重_jp", "是否去重行（字典 sys_yes_no；0=否 1=是；SELECT DISTINCT）"),
            // entity.configurable.distinctrows
            new TranslationSeedItem("entity.configurable.distinctrows", "zh-CN", "是否去重", "是否去重行（字典 sys_yes_no；0=否 1=是；SELECT DISTINCT）"),
            // entity.configurable.distinctrows
            new TranslationSeedItem("entity.configurable.distinctrows", "zh-HK", "是否去重_hk", "是否去重行（字典 sys_yes_no；0=否 1=是；SELECT DISTINCT）"),

            // entity.configurable.maxexportrows
            new TranslationSeedItem("entity.configurable.maxexportrows", "en-US", "导出最大行数_us", "单次导出最大行数（TaktConfigurableConstants；DefaultRowLimit=500，MaxRowLimit=50000）"),
            // entity.configurable.maxexportrows
            new TranslationSeedItem("entity.configurable.maxexportrows", "ja-JP", "导出最大行数_jp", "单次导出最大行数（TaktConfigurableConstants；DefaultRowLimit=500，MaxRowLimit=50000）"),
            // entity.configurable.maxexportrows
            new TranslationSeedItem("entity.configurable.maxexportrows", "zh-CN", "导出最大行数", "单次导出最大行数（TaktConfigurableConstants；DefaultRowLimit=500，MaxRowLimit=50000）"),
            // entity.configurable.maxexportrows
            new TranslationSeedItem("entity.configurable.maxexportrows", "zh-HK", "导出最大行数_hk", "单次导出最大行数（TaktConfigurableConstants；DefaultRowLimit=500，MaxRowLimit=50000）"),

            // entity.configurable.maxqueryrows
            new TranslationSeedItem("entity.configurable.maxqueryrows", "en-US", "查询最大行数_us", "单次查询最大行数（TaktConfigurableConstants；DefaultRowLimit=500，MaxRowLimit=50000）"),
            // entity.configurable.maxqueryrows
            new TranslationSeedItem("entity.configurable.maxqueryrows", "ja-JP", "查询最大行数_jp", "单次查询最大行数（TaktConfigurableConstants；DefaultRowLimit=500，MaxRowLimit=50000）"),
            // entity.configurable.maxqueryrows
            new TranslationSeedItem("entity.configurable.maxqueryrows", "zh-CN", "查询最大行数", "单次查询最大行数（TaktConfigurableConstants；DefaultRowLimit=500，MaxRowLimit=50000）"),
            // entity.configurable.maxqueryrows
            new TranslationSeedItem("entity.configurable.maxqueryrows", "zh-HK", "查询最大行数_hk", "单次查询最大行数（TaktConfigurableConstants；DefaultRowLimit=500，MaxRowLimit=50000）"),

            // entity.configurable.ispublic
            new TranslationSeedItem("entity.configurable.ispublic", "en-US", "公开_us", "公开（字典 sys_public_type；0=公开 1=私有）"),
            // entity.configurable.ispublic
            new TranslationSeedItem("entity.configurable.ispublic", "ja-JP", "公开_jp", "公开（字典 sys_public_type；0=公开 1=私有）"),
            // entity.configurable.ispublic
            new TranslationSeedItem("entity.configurable.ispublic", "zh-CN", "公开", "公开（字典 sys_public_type；0=公开 1=私有）"),
            // entity.configurable.ispublic
            new TranslationSeedItem("entity.configurable.ispublic", "zh-HK", "公开_hk", "公开（字典 sys_public_type；0=公开 1=私有）"),

            // entity.configurable.description
            new TranslationSeedItem("entity.configurable.description", "en-US", "定制报表描述_us", "定制报表描述"),
            // entity.configurable.description
            new TranslationSeedItem("entity.configurable.description", "ja-JP", "定制报表描述_jp", "定制报表描述"),
            // entity.configurable.description
            new TranslationSeedItem("entity.configurable.description", "zh-CN", "定制报表描述", "定制报表描述"),
            // entity.configurable.description
            new TranslationSeedItem("entity.configurable.description", "zh-HK", "定制报表描述_hk", "定制报表描述"),

            // entity.configurable.sortorder
            new TranslationSeedItem("entity.configurable.sortorder", "en-US", "排序号_us", "排序号（回填）"),
            // entity.configurable.sortorder
            new TranslationSeedItem("entity.configurable.sortorder", "ja-JP", "排序号_jp", "排序号（回填）"),
            // entity.configurable.sortorder
            new TranslationSeedItem("entity.configurable.sortorder", "zh-CN", "排序号", "排序号（回填）"),
            // entity.configurable.sortorder
            new TranslationSeedItem("entity.configurable.sortorder", "zh-HK", "排序号_hk", "排序号（回填）"),

            // entity.configurable.status
            new TranslationSeedItem("entity.configurable.status", "en-US", "定制报表状态_us", "定制报表状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）"),
            // entity.configurable.status
            new TranslationSeedItem("entity.configurable.status", "ja-JP", "定制报表状态_jp", "定制报表状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）"),
            // entity.configurable.status
            new TranslationSeedItem("entity.configurable.status", "zh-CN", "定制报表状态", "定制报表状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）"),
            // entity.configurable.status
            new TranslationSeedItem("entity.configurable.status", "zh-HK", "定制报表状态_hk", "定制报表状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）"),

            // entity.configurable.sources
            new TranslationSeedItem("entity.configurable.sources", "en-US", "数据源表列表_us", "数据源表列表（FROM）"),
            // entity.configurable.sources
            new TranslationSeedItem("entity.configurable.sources", "ja-JP", "数据源表列表_jp", "数据源表列表（FROM）"),
            // entity.configurable.sources
            new TranslationSeedItem("entity.configurable.sources", "zh-CN", "数据源表列表", "数据源表列表（FROM）"),
            // entity.configurable.sources
            new TranslationSeedItem("entity.configurable.sources", "zh-HK", "数据源表列表_hk", "数据源表列表（FROM）"),

            // entity.configurable.joins
            new TranslationSeedItem("entity.configurable.joins", "en-US", "多表关联列表_us", "多表关联列表（JOIN）"),
            // entity.configurable.joins
            new TranslationSeedItem("entity.configurable.joins", "ja-JP", "多表关联列表_jp", "多表关联列表（JOIN）"),
            // entity.configurable.joins
            new TranslationSeedItem("entity.configurable.joins", "zh-CN", "多表关联列表", "多表关联列表（JOIN）"),
            // entity.configurable.joins
            new TranslationSeedItem("entity.configurable.joins", "zh-HK", "多表关联列表_hk", "多表关联列表（JOIN）"),

            // entity.configurable.fields
            new TranslationSeedItem("entity.configurable.fields", "en-US", "输出字段列表_us", "输出字段列表（SELECT）"),
            // entity.configurable.fields
            new TranslationSeedItem("entity.configurable.fields", "ja-JP", "输出字段列表_jp", "输出字段列表（SELECT）"),
            // entity.configurable.fields
            new TranslationSeedItem("entity.configurable.fields", "zh-CN", "输出字段列表", "输出字段列表（SELECT）"),
            // entity.configurable.fields
            new TranslationSeedItem("entity.configurable.fields", "zh-HK", "输出字段列表_hk", "输出字段列表（SELECT）"),

            // entity.configurable.selections
            new TranslationSeedItem("entity.configurable.selections", "en-US", "筛选条件列表_us", "筛选条件列表（WHERE）"),
            // entity.configurable.selections
            new TranslationSeedItem("entity.configurable.selections", "ja-JP", "筛选条件列表_jp", "筛选条件列表（WHERE）"),
            // entity.configurable.selections
            new TranslationSeedItem("entity.configurable.selections", "zh-CN", "筛选条件列表", "筛选条件列表（WHERE）"),
            // entity.configurable.selections
            new TranslationSeedItem("entity.configurable.selections", "zh-HK", "筛选条件列表_hk", "筛选条件列表（WHERE）"),

            // entity.configurable.groupbys
            new TranslationSeedItem("entity.configurable.groupbys", "en-US", "分组字段列表_us", "分组字段列表（GROUP BY）"),
            // entity.configurable.groupbys
            new TranslationSeedItem("entity.configurable.groupbys", "ja-JP", "分组字段列表_jp", "分组字段列表（GROUP BY）"),
            // entity.configurable.groupbys
            new TranslationSeedItem("entity.configurable.groupbys", "zh-CN", "分组字段列表", "分组字段列表（GROUP BY）"),
            // entity.configurable.groupbys
            new TranslationSeedItem("entity.configurable.groupbys", "zh-HK", "分组字段列表_hk", "分组字段列表（GROUP BY）"),

            // entity.configurable.orderbys
            new TranslationSeedItem("entity.configurable.orderbys", "en-US", "排序字段列表_us", "排序字段列表（ORDER BY）"),
            // entity.configurable.orderbys
            new TranslationSeedItem("entity.configurable.orderbys", "ja-JP", "排序字段列表_jp", "排序字段列表（ORDER BY）"),
            // entity.configurable.orderbys
            new TranslationSeedItem("entity.configurable.orderbys", "zh-CN", "排序字段列表", "排序字段列表（ORDER BY）"),
            // entity.configurable.orderbys
            new TranslationSeedItem("entity.configurable.orderbys", "zh-HK", "排序字段列表_hk", "排序字段列表（ORDER BY）"),
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
        translation.ResourceGroup = "QuickQuery";
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
