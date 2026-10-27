// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Operation
// 文件名称：TaktInspectionStandardI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktInspectionStandard 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Operation;

/// <summary>
/// TaktInspectionStandard 实体国际化翻译种子（键前缀 entity.inspectionstandard.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktInspectionStandardI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktInspectionStandard 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 inspectionstandard 实体翻译...", tenantCode);

        foreach (var item in GetInspectionStandardTranslations())
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

        TaktLogger.Information("TaktInspectionStandard 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktInspectionStandard 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.inspectionstandard._self / entity.inspectionstandard.{{field}}；ResourceGroup=Operation；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetInspectionStandardTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.inspectionstandard._self
            new TranslationSeedItem("entity.inspectionstandard._self", "en-US", "Inspection Standard Information_us", "实体名称"),
            // entity.inspectionstandard._self
            new TranslationSeedItem("entity.inspectionstandard._self", "ja-JP", "检验标准信息_jp", "实体名称"),
            // entity.inspectionstandard._self
            new TranslationSeedItem("entity.inspectionstandard._self", "zh-CN", "检验标准信息", "实体名称"),
            // entity.inspectionstandard._self
            new TranslationSeedItem("entity.inspectionstandard._self", "zh-HK", "检验标准信息_hk", "实体名称"),

            // entity.inspectionstandard.standardcode
            new TranslationSeedItem("entity.inspectionstandard.standardcode", "en-US", "检验标准编码_us", "检验标准编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 检验标准编码规则生成并展示，非手输；单据类型菜单：检验标准）"),
            // entity.inspectionstandard.standardcode
            new TranslationSeedItem("entity.inspectionstandard.standardcode", "ja-JP", "检验标准编码_jp", "检验标准编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 检验标准编码规则生成并展示，非手输；单据类型菜单：检验标准）"),
            // entity.inspectionstandard.standardcode
            new TranslationSeedItem("entity.inspectionstandard.standardcode", "zh-CN", "检验标准编码", "检验标准编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 检验标准编码规则生成并展示，非手输；单据类型菜单：检验标准）"),
            // entity.inspectionstandard.standardcode
            new TranslationSeedItem("entity.inspectionstandard.standardcode", "zh-HK", "检验标准编码_hk", "检验标准编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 检验标准编码规则生成并展示，非手输；单据类型菜单：检验标准）"),

            // entity.inspectionstandard.standardname
            new TranslationSeedItem("entity.inspectionstandard.standardname", "en-US", "检验标准名称_us", "检验标准名称"),
            // entity.inspectionstandard.standardname
            new TranslationSeedItem("entity.inspectionstandard.standardname", "ja-JP", "检验标准名称_jp", "检验标准名称"),
            // entity.inspectionstandard.standardname
            new TranslationSeedItem("entity.inspectionstandard.standardname", "zh-CN", "检验标准名称", "检验标准名称"),
            // entity.inspectionstandard.standardname
            new TranslationSeedItem("entity.inspectionstandard.standardname", "zh-HK", "检验标准名称_hk", "检验标准名称"),

            // entity.inspectionstandard.inspectiontype
            new TranslationSeedItem("entity.inspectionstandard.inspectiontype", "en-US", "检验类型_us", "检验类型（字典 logistics_quality_inspection_type）"),
            // entity.inspectionstandard.inspectiontype
            new TranslationSeedItem("entity.inspectionstandard.inspectiontype", "ja-JP", "检验类型_jp", "检验类型（字典 logistics_quality_inspection_type）"),
            // entity.inspectionstandard.inspectiontype
            new TranslationSeedItem("entity.inspectionstandard.inspectiontype", "zh-CN", "检验类型", "检验类型（字典 logistics_quality_inspection_type）"),
            // entity.inspectionstandard.inspectiontype
            new TranslationSeedItem("entity.inspectionstandard.inspectiontype", "zh-HK", "检验类型_hk", "检验类型（字典 logistics_quality_inspection_type）"),

            // entity.inspectionstandard.materialcategorycode
            new TranslationSeedItem("entity.inspectionstandard.materialcategorycode", "en-US", "物料类别编码_us", "物料类别编码"),
            // entity.inspectionstandard.materialcategorycode
            new TranslationSeedItem("entity.inspectionstandard.materialcategorycode", "ja-JP", "物料类别编码_jp", "物料类别编码"),
            // entity.inspectionstandard.materialcategorycode
            new TranslationSeedItem("entity.inspectionstandard.materialcategorycode", "zh-CN", "物料类别编码", "物料类别编码"),
            // entity.inspectionstandard.materialcategorycode
            new TranslationSeedItem("entity.inspectionstandard.materialcategorycode", "zh-HK", "物料类别编码_hk", "物料类别编码"),

            // entity.inspectionstandard.materialcategoryname
            new TranslationSeedItem("entity.inspectionstandard.materialcategoryname", "en-US", "物料类别名称_us", "物料类别名称"),
            // entity.inspectionstandard.materialcategoryname
            new TranslationSeedItem("entity.inspectionstandard.materialcategoryname", "ja-JP", "物料类别名称_jp", "物料类别名称"),
            // entity.inspectionstandard.materialcategoryname
            new TranslationSeedItem("entity.inspectionstandard.materialcategoryname", "zh-CN", "物料类别名称", "物料类别名称"),
            // entity.inspectionstandard.materialcategoryname
            new TranslationSeedItem("entity.inspectionstandard.materialcategoryname", "zh-HK", "物料类别名称_hk", "物料类别名称"),

            // entity.inspectionstandard.samplingschemecode
            new TranslationSeedItem("entity.inspectionstandard.samplingschemecode", "en-US", "抽样方案编码_us", "抽样方案编码（选项 TaktSamplingSchemes/options；DictValue=SamplingSchemeCode）"),
            // entity.inspectionstandard.samplingschemecode
            new TranslationSeedItem("entity.inspectionstandard.samplingschemecode", "ja-JP", "抽样方案编码_jp", "抽样方案编码（选项 TaktSamplingSchemes/options；DictValue=SamplingSchemeCode）"),
            // entity.inspectionstandard.samplingschemecode
            new TranslationSeedItem("entity.inspectionstandard.samplingschemecode", "zh-CN", "抽样方案编码", "抽样方案编码（选项 TaktSamplingSchemes/options；DictValue=SamplingSchemeCode）"),
            // entity.inspectionstandard.samplingschemecode
            new TranslationSeedItem("entity.inspectionstandard.samplingschemecode", "zh-HK", "抽样方案编码_hk", "抽样方案编码（选项 TaktSamplingSchemes/options；DictValue=SamplingSchemeCode）"),

            // entity.inspectionstandard.samplingschemename
            new TranslationSeedItem("entity.inspectionstandard.samplingschemename", "en-US", "抽样方案名称_us", "抽样方案名称"),
            // entity.inspectionstandard.samplingschemename
            new TranslationSeedItem("entity.inspectionstandard.samplingschemename", "ja-JP", "抽样方案名称_jp", "抽样方案名称"),
            // entity.inspectionstandard.samplingschemename
            new TranslationSeedItem("entity.inspectionstandard.samplingschemename", "zh-CN", "抽样方案名称", "抽样方案名称"),
            // entity.inspectionstandard.samplingschemename
            new TranslationSeedItem("entity.inspectionstandard.samplingschemename", "zh-HK", "抽样方案名称_hk", "抽样方案名称"),

            // entity.inspectionstandard.standarddescription
            new TranslationSeedItem("entity.inspectionstandard.standarddescription", "en-US", "检验标准描述_us", "检验标准描述"),
            // entity.inspectionstandard.standarddescription
            new TranslationSeedItem("entity.inspectionstandard.standarddescription", "ja-JP", "检验标准描述_jp", "检验标准描述"),
            // entity.inspectionstandard.standarddescription
            new TranslationSeedItem("entity.inspectionstandard.standarddescription", "zh-CN", "检验标准描述", "检验标准描述"),
            // entity.inspectionstandard.standarddescription
            new TranslationSeedItem("entity.inspectionstandard.standarddescription", "zh-HK", "检验标准描述_hk", "检验标准描述"),

            // entity.inspectionstandard.standardstatus
            new TranslationSeedItem("entity.inspectionstandard.standardstatus", "en-US", "检验标准状态_us", "检验标准状态（字典 logistics_quality_standard_status）"),
            // entity.inspectionstandard.standardstatus
            new TranslationSeedItem("entity.inspectionstandard.standardstatus", "ja-JP", "检验标准状态_jp", "检验标准状态（字典 logistics_quality_standard_status）"),
            // entity.inspectionstandard.standardstatus
            new TranslationSeedItem("entity.inspectionstandard.standardstatus", "zh-CN", "检验标准状态", "检验标准状态（字典 logistics_quality_standard_status）"),
            // entity.inspectionstandard.standardstatus
            new TranslationSeedItem("entity.inspectionstandard.standardstatus", "zh-HK", "检验标准状态_hk", "检验标准状态（字典 logistics_quality_standard_status）"),

            // entity.inspectionstandard.items
            new TranslationSeedItem("entity.inspectionstandard.items", "en-US", "检验标准明细列表_us", "检验标准明细列表（主子表关系）"),
            // entity.inspectionstandard.items
            new TranslationSeedItem("entity.inspectionstandard.items", "ja-JP", "检验标准明细列表_jp", "检验标准明细列表（主子表关系）"),
            // entity.inspectionstandard.items
            new TranslationSeedItem("entity.inspectionstandard.items", "zh-CN", "检验标准明细列表", "检验标准明细列表（主子表关系）"),
            // entity.inspectionstandard.items
            new TranslationSeedItem("entity.inspectionstandard.items", "zh-HK", "检验标准明细列表_hk", "检验标准明细列表（主子表关系）"),
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
        translation.ResourceGroup = "Operation";
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
