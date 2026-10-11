// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcHinkanI18nSeedData.cs
// 创建时间：2026-08-26
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEcHinkan 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktEcHinkan 实体国际化翻译种子（键前缀 entity.echinkan.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEcHinkanI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEcHinkan 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 echinkan 实体翻译...", tenantCode);

        foreach (var item in GetEcHinkanTranslations())
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

        TaktLogger.Information("TaktEcHinkan 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEcHinkan 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.echinkan._self / entity.echinkan.{{field}}；ResourceGroup=EngineeringChange；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEcHinkanTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.echinkan._self
            new TranslationSeedItem("entity.echinkan._self", "en-US", "Ec Hinkan Information_us", "实体名称"),
            // entity.echinkan._self
            new TranslationSeedItem("entity.echinkan._self", "ja-JP", "设变品管课信息_jp", "实体名称"),
            // entity.echinkan._self
            new TranslationSeedItem("entity.echinkan._self", "zh-CN", "设变品管课信息", "实体名称"),
            // entity.echinkan._self
            new TranslationSeedItem("entity.echinkan._self", "zh-HK", "设变品管课信息_hk", "实体名称"),

            // entity.echinkan.ecdetailid
            new TranslationSeedItem("entity.echinkan.ecdetailid", "en-US", "设变明细ID_us", "设变明细 ID（TaktEcDetail 主键；主表多对一导航 EcDetail）"),
            // entity.echinkan.ecdetailid
            new TranslationSeedItem("entity.echinkan.ecdetailid", "ja-JP", "设变明细ID_jp", "设变明细 ID（TaktEcDetail 主键；主表多对一导航 EcDetail）"),
            // entity.echinkan.ecdetailid
            new TranslationSeedItem("entity.echinkan.ecdetailid", "zh-CN", "设变明细ID", "设变明细 ID（TaktEcDetail 主键；主表多对一导航 EcDetail）"),
            // entity.echinkan.ecdetailid
            new TranslationSeedItem("entity.echinkan.ecdetailid", "zh-HK", "设变明细ID_hk", "设变明细 ID（TaktEcDetail 主键；主表多对一导航 EcDetail）"),

            // entity.echinkan.eccode
            new TranslationSeedItem("entity.echinkan.eccode", "en-US", "设变单号_us", "设变单号（冗余，便于查询）"),
            // entity.echinkan.eccode
            new TranslationSeedItem("entity.echinkan.eccode", "ja-JP", "设变单号_jp", "设变单号（冗余，便于查询）"),
            // entity.echinkan.eccode
            new TranslationSeedItem("entity.echinkan.eccode", "zh-CN", "设变单号", "设变单号（冗余，便于查询）"),
            // entity.echinkan.eccode
            new TranslationSeedItem("entity.echinkan.eccode", "zh-HK", "设变单号_hk", "设变单号（冗余，便于查询）"),

            // entity.echinkan.linenumber
            new TranslationSeedItem("entity.echinkan.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.echinkan.linenumber
            new TranslationSeedItem("entity.echinkan.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.echinkan.linenumber
            new TranslationSeedItem("entity.echinkan.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.echinkan.linenumber
            new TranslationSeedItem("entity.echinkan.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.echinkan.ecmodelcode
            new TranslationSeedItem("entity.echinkan.ecmodelcode", "en-US", "Model code_us", "机种编码（冗余：来自 TaktEcDetail.EcModelCode）"),
            // entity.echinkan.ecmodelcode
            new TranslationSeedItem("entity.echinkan.ecmodelcode", "ja-JP", "機種コード_jp", "机种编码（冗余：来自 TaktEcDetail.EcModelCode）"),
            // entity.echinkan.ecmodelcode
            new TranslationSeedItem("entity.echinkan.ecmodelcode", "zh-CN", "机种编码", "机种编码（冗余：来自 TaktEcDetail.EcModelCode）"),
            // entity.echinkan.ecmodelcode
            new TranslationSeedItem("entity.echinkan.ecmodelcode", "zh-HK", "機種編碼_hk", "机种编码（冗余：来自 TaktEcDetail.EcModelCode）"),
            new TranslationSeedItem("entity.echinkan.ecmodelcodegroup", "en-US", "Model code group", "机种编码组（按设变时同设变单号下的机种编码，分号分隔；ntext）"),
            new TranslationSeedItem("entity.echinkan.ecmodelcodegroup", "ja-JP", "機種コード組", "机种编码组（按设变时同设变单号下的机种编码，分号分隔；ntext）"),
            new TranslationSeedItem("entity.echinkan.ecmodelcodegroup", "zh-CN", "机种编码组", "机种编码组（按设变时同设变单号下的机种编码，分号分隔；ntext）"),
            new TranslationSeedItem("entity.echinkan.ecmodelcodegroup", "zh-HK", "機種編碼組", "机种编码组（按设变时同设变单号下的机种编码，分号分隔；ntext）"),

            // entity.echinkan.ecrootmaterialcode
            new TranslationSeedItem("entity.echinkan.ecrootmaterialcode", "en-US", "根物料编码_us", "根物料编码（冗余：来自 TaktEcDetail.EcRootMaterialCode）"),
            // entity.echinkan.ecrootmaterialcode
            new TranslationSeedItem("entity.echinkan.ecrootmaterialcode", "ja-JP", "根物料编码_jp", "根物料编码（冗余：来自 TaktEcDetail.EcRootMaterialCode）"),
            // entity.echinkan.ecrootmaterialcode
            new TranslationSeedItem("entity.echinkan.ecrootmaterialcode", "zh-CN", "根物料编码", "根物料编码（冗余：来自 TaktEcDetail.EcRootMaterialCode）"),
            // entity.echinkan.ecrootmaterialcode
            new TranslationSeedItem("entity.echinkan.ecrootmaterialcode", "zh-HK", "根物料编码_hk", "根物料编码（冗余：来自 TaktEcDetail.EcRootMaterialCode）"),

            // entity.echinkan.ecrootmaterialdescription
            new TranslationSeedItem("entity.echinkan.ecrootmaterialdescription", "en-US", "根物料描述_us", "根物料描述（冗余：来自 TaktEcDetail.EcRootMaterialDescription）"),
            // entity.echinkan.ecrootmaterialdescription
            new TranslationSeedItem("entity.echinkan.ecrootmaterialdescription", "ja-JP", "根物料描述_jp", "根物料描述（冗余：来自 TaktEcDetail.EcRootMaterialDescription）"),
            // entity.echinkan.ecrootmaterialdescription
            new TranslationSeedItem("entity.echinkan.ecrootmaterialdescription", "zh-CN", "根物料描述", "根物料描述（冗余：来自 TaktEcDetail.EcRootMaterialDescription）"),
            // entity.echinkan.ecrootmaterialdescription
            new TranslationSeedItem("entity.echinkan.ecrootmaterialdescription", "zh-HK", "根物料描述_hk", "根物料描述（冗余：来自 TaktEcDetail.EcRootMaterialDescription）"),

            // entity.echinkan.discontinuedstatus
            new TranslationSeedItem("entity.echinkan.discontinuedstatus", "en-US", "根物料EOL_us", "停产状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料；冗余：来自 TaktEcDetail.DiscontinuedStatus）"),
            // entity.echinkan.discontinuedstatus
            new TranslationSeedItem("entity.echinkan.discontinuedstatus", "ja-JP", "根物料EOL_jp", "停产状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料；冗余：来自 TaktEcDetail.DiscontinuedStatus）"),
            // entity.echinkan.discontinuedstatus
            new TranslationSeedItem("entity.echinkan.discontinuedstatus", "zh-CN", "根物料EOL", "停产状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料；冗余：来自 TaktEcDetail.DiscontinuedStatus）"),
            // entity.echinkan.discontinuedstatus
            new TranslationSeedItem("entity.echinkan.discontinuedstatus", "zh-HK", "根物料EOL_hk", "停产状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料；冗余：来自 TaktEcDetail.DiscontinuedStatus）"),

            
            // entity.echinkan.ecscope
            new TranslationSeedItem("entity.echinkan.ecscope", "en-US", "Scope", "实施范围（冗余：来自 TaktEcDetail.EcScope）"),
            // entity.echinkan.ecscope
            new TranslationSeedItem("entity.echinkan.ecscope", "ja-JP", "実施範囲", "实施范围（冗余：来自 TaktEcDetail.EcScope）"),
            // entity.echinkan.ecscope
            new TranslationSeedItem("entity.echinkan.ecscope", "zh-CN", "实施范围", "实施范围（冗余：来自 TaktEcDetail.EcScope）"),
            // entity.echinkan.ecscope
            new TranslationSeedItem("entity.echinkan.ecscope", "zh-HK", "實施範圍", "实施范围（冗余：来自 TaktEcDetail.EcScope）"),
// entity.echinkan.deptcode
            new TranslationSeedItem("entity.echinkan.deptcode", "en-US", "部门编码_us", "部门编码（TaktDept.DeptCode，5 位，如 D0820）"),
            // entity.echinkan.deptcode
            new TranslationSeedItem("entity.echinkan.deptcode", "ja-JP", "部门编码_jp", "部门编码（TaktDept.DeptCode，5 位，如 D0820）"),
            // entity.echinkan.deptcode
            new TranslationSeedItem("entity.echinkan.deptcode", "zh-CN", "部门编码", "部门编码（TaktDept.DeptCode，5 位，如 D0820）"),
            // entity.echinkan.deptcode
            new TranslationSeedItem("entity.echinkan.deptcode", "zh-HK", "部门编码_hk", "部门编码（TaktDept.DeptCode，5 位，如 D0820）"),

            // entity.echinkan.isimplemented
            new TranslationSeedItem("entity.echinkan.isimplemented", "en-US", "实施_us", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.echinkan.isimplemented
            new TranslationSeedItem("entity.echinkan.isimplemented", "ja-JP", "实施_jp", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.echinkan.isimplemented
            new TranslationSeedItem("entity.echinkan.isimplemented", "zh-CN", "实施", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.echinkan.isimplemented
            new TranslationSeedItem("entity.echinkan.isimplemented", "zh-HK", "实施_hk", "是否实施（0=否 1=是，字典 sys_yes_no）"),

            // entity.echinkan.execcontent
            new TranslationSeedItem("entity.echinkan.execcontent", "en-US", "执行内容_us", "执行内容（各部门通用）"),
            // entity.echinkan.execcontent
            new TranslationSeedItem("entity.echinkan.execcontent", "ja-JP", "执行内容_jp", "执行内容（各部门通用）"),
            // entity.echinkan.execcontent
            new TranslationSeedItem("entity.echinkan.execcontent", "zh-CN", "执行内容", "执行内容（各部门通用）"),
            // entity.echinkan.execcontent
            new TranslationSeedItem("entity.echinkan.execcontent", "zh-HK", "执行内容_hk", "执行内容（各部门通用）"),

            // entity.echinkan.productionteam
            new TranslationSeedItem("entity.echinkan.productionteam", "en-US", "生产班组_us", "生产班组"),
            // entity.echinkan.productionteam
            new TranslationSeedItem("entity.echinkan.productionteam", "ja-JP", "生产班组_jp", "生产班组"),
            // entity.echinkan.productionteam
            new TranslationSeedItem("entity.echinkan.productionteam", "zh-CN", "生产班组", "生产班组"),
            // entity.echinkan.productionteam
            new TranslationSeedItem("entity.echinkan.productionteam", "zh-HK", "生产班组_hk", "生产班组"),

            // entity.echinkan.inspectiondate
            new TranslationSeedItem("entity.echinkan.inspectiondate", "en-US", "检验日期_us", "检验日期"),
            // entity.echinkan.inspectiondate
            new TranslationSeedItem("entity.echinkan.inspectiondate", "ja-JP", "检验日期_jp", "检验日期"),
            // entity.echinkan.inspectiondate
            new TranslationSeedItem("entity.echinkan.inspectiondate", "zh-CN", "检验日期", "检验日期"),
            // entity.echinkan.inspectiondate
            new TranslationSeedItem("entity.echinkan.inspectiondate", "zh-HK", "检验日期_hk", "检验日期"),

            // entity.echinkan.inspectionbatch
            new TranslationSeedItem("entity.echinkan.inspectionbatch", "en-US", "检验批次_us", "检验批次"),
            // entity.echinkan.inspectionbatch
            new TranslationSeedItem("entity.echinkan.inspectionbatch", "ja-JP", "检验批次_jp", "检验批次"),
            // entity.echinkan.inspectionbatch
            new TranslationSeedItem("entity.echinkan.inspectionbatch", "zh-CN", "检验批次", "检验批次"),
            // entity.echinkan.inspectionbatch
            new TranslationSeedItem("entity.echinkan.inspectionbatch", "zh-HK", "检验批次_hk", "检验批次"),

            // entity.echinkan.samplingcode
            new TranslationSeedItem("entity.echinkan.samplingcode", "en-US", "抽样号码_us", "抽样号码"),
            // entity.echinkan.samplingcode
            new TranslationSeedItem("entity.echinkan.samplingcode", "ja-JP", "抽样号码_jp", "抽样号码"),
            // entity.echinkan.samplingcode
            new TranslationSeedItem("entity.echinkan.samplingcode", "zh-CN", "抽样号码", "抽样号码"),
            // entity.echinkan.samplingcode
            new TranslationSeedItem("entity.echinkan.samplingcode", "zh-HK", "抽样号码_hk", "抽样号码"),

            // entity.echinkan.isobsolete
            new TranslationSeedItem("entity.echinkan.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.echinkan.isobsolete
            new TranslationSeedItem("entity.echinkan.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.echinkan.isobsolete
            new TranslationSeedItem("entity.echinkan.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.echinkan.isobsolete
            new TranslationSeedItem("entity.echinkan.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
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
