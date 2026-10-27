// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcMeI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEcMe 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktEcMe 实体国际化翻译种子（键前缀 entity.ecme.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEcMeI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEcMe 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 ecme 实体翻译...", tenantCode);

        foreach (var item in GetEcMeTranslations())
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

        TaktLogger.Information("TaktEcMe 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEcMe 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.ecme._self / entity.ecme.{{field}}；ResourceGroup=EngineeringChange；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEcMeTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.ecme._self
            new TranslationSeedItem("entity.ecme._self", "en-US", "Ec Me Information_us", "实体名称"),
            // entity.ecme._self
            new TranslationSeedItem("entity.ecme._self", "ja-JP", "设变制造技术课信息_jp", "实体名称"),
            // entity.ecme._self
            new TranslationSeedItem("entity.ecme._self", "zh-CN", "设变制造技术课信息", "实体名称"),
            // entity.ecme._self
            new TranslationSeedItem("entity.ecme._self", "zh-HK", "设变制造技术课信息_hk", "实体名称"),

            // entity.ecme.linenumber
            new TranslationSeedItem("entity.ecme.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.ecme.linenumber
            new TranslationSeedItem("entity.ecme.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.ecme.linenumber
            new TranslationSeedItem("entity.ecme.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.ecme.linenumber
            new TranslationSeedItem("entity.ecme.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.ecme.eccode
            new TranslationSeedItem("entity.ecme.eccode", "en-US", "设变单号_us", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),
            // entity.ecme.eccode
            new TranslationSeedItem("entity.ecme.eccode", "ja-JP", "设变单号_jp", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),
            // entity.ecme.eccode
            new TranslationSeedItem("entity.ecme.eccode", "zh-CN", "设变单号", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),
            // entity.ecme.eccode
            new TranslationSeedItem("entity.ecme.eccode", "zh-HK", "设变单号_hk", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),

            // entity.ecme.discontinuedstatus
            new TranslationSeedItem("entity.ecme.discontinuedstatus", "en-US", "停产状态_us", "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）"),
            // entity.ecme.discontinuedstatus
            new TranslationSeedItem("entity.ecme.discontinuedstatus", "ja-JP", "停产状态_jp", "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）"),
            // entity.ecme.discontinuedstatus
            new TranslationSeedItem("entity.ecme.discontinuedstatus", "zh-CN", "停产状态", "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）"),
            // entity.ecme.discontinuedstatus
            new TranslationSeedItem("entity.ecme.discontinuedstatus", "zh-HK", "停产状态_hk", "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）"),

            // entity.ecme.ecmodelcodegroup
            new TranslationSeedItem("entity.ecme.ecmodelcodegroup", "en-US", "机种编码组_us", "机种编码组（单个或多个机种，分号分隔；ntext）"),
            // entity.ecme.ecmodelcodegroup
            new TranslationSeedItem("entity.ecme.ecmodelcodegroup", "ja-JP", "机种编码组_jp", "机种编码组（单个或多个机种，分号分隔；ntext）"),
            // entity.ecme.ecmodelcodegroup
            new TranslationSeedItem("entity.ecme.ecmodelcodegroup", "zh-CN", "机种编码组", "机种编码组（单个或多个机种，分号分隔；ntext）"),
            // entity.ecme.ecmodelcodegroup
            new TranslationSeedItem("entity.ecme.ecmodelcodegroup", "zh-HK", "机种编码组_hk", "机种编码组（单个或多个机种，分号分隔；ntext）"),

            // entity.ecme.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.ecme.ecrootmaterialcodegroup", "en-US", "根物料编码组_us", "根物料编码组（同去重键下的根物料编码，分号分隔；ntext）"),
            // entity.ecme.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.ecme.ecrootmaterialcodegroup", "ja-JP", "根物料编码组_jp", "根物料编码组（同去重键下的根物料编码，分号分隔；ntext）"),
            // entity.ecme.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.ecme.ecrootmaterialcodegroup", "zh-CN", "根物料编码组", "根物料编码组（同去重键下的根物料编码，分号分隔；ntext）"),
            // entity.ecme.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.ecme.ecrootmaterialcodegroup", "zh-HK", "根物料编码组_hk", "根物料编码组（同去重键下的根物料编码，分号分隔；ntext）"),

            // entity.ecme.issopupdated
            new TranslationSeedItem("entity.ecme.issopupdated", "en-US", "更新SOP_us", "是否更新 SOP（字典 sys_yes_no；0=否 1=是）"),
            // entity.ecme.issopupdated
            new TranslationSeedItem("entity.ecme.issopupdated", "ja-JP", "更新SOP_jp", "是否更新 SOP（字典 sys_yes_no；0=否 1=是）"),
            // entity.ecme.issopupdated
            new TranslationSeedItem("entity.ecme.issopupdated", "zh-CN", "更新SOP", "是否更新 SOP（字典 sys_yes_no；0=否 1=是）"),
            // entity.ecme.issopupdated
            new TranslationSeedItem("entity.ecme.issopupdated", "zh-HK", "更新SOP_hk", "是否更新 SOP（字典 sys_yes_no；0=否 1=是）"),

            // entity.ecme.sopleader
            new TranslationSeedItem("entity.ecme.sopleader", "en-US", "SOP担当_us", "SOP担当（选项 TaktEcGroups/options?ecGroupCategory=2；DictValue=EcGroupCode，DictLabel=EcGroupName）"),
            // entity.ecme.sopleader
            new TranslationSeedItem("entity.ecme.sopleader", "ja-JP", "SOP担当_jp", "SOP担当（选项 TaktEcGroups/options?ecGroupCategory=2；DictValue=EcGroupCode，DictLabel=EcGroupName）"),
            // entity.ecme.sopleader
            new TranslationSeedItem("entity.ecme.sopleader", "zh-CN", "SOP担当", "SOP担当（选项 TaktEcGroups/options?ecGroupCategory=2；DictValue=EcGroupCode，DictLabel=EcGroupName）"),
            // entity.ecme.sopleader
            new TranslationSeedItem("entity.ecme.sopleader", "zh-HK", "SOP担当_hk", "SOP担当（选项 TaktEcGroups/options?ecGroupCategory=2；DictValue=EcGroupCode，DictLabel=EcGroupName）"),

            // entity.ecme.sopdate
            new TranslationSeedItem("entity.ecme.sopdate", "en-US", "SOP日期_us", "SOP日期"),
            // entity.ecme.sopdate
            new TranslationSeedItem("entity.ecme.sopdate", "ja-JP", "SOP日期_jp", "SOP日期"),
            // entity.ecme.sopdate
            new TranslationSeedItem("entity.ecme.sopdate", "zh-CN", "SOP日期", "SOP日期"),
            // entity.ecme.sopdate
            new TranslationSeedItem("entity.ecme.sopdate", "zh-HK", "SOP日期_hk", "SOP日期"),

            // entity.ecme.techleader
            new TranslationSeedItem("entity.ecme.techleader", "en-US", "设变EC担当_us", "设变EC担当（选项 TaktEcGroups/options?ecGroupCategory=1；DictValue=EcGroupCode，DictLabel=EcGroupName）"),
            // entity.ecme.techleader
            new TranslationSeedItem("entity.ecme.techleader", "ja-JP", "设变EC担当_jp", "设变EC担当（选项 TaktEcGroups/options?ecGroupCategory=1；DictValue=EcGroupCode，DictLabel=EcGroupName）"),
            // entity.ecme.techleader
            new TranslationSeedItem("entity.ecme.techleader", "zh-CN", "设变EC担当", "设变EC担当（选项 TaktEcGroups/options?ecGroupCategory=1；DictValue=EcGroupCode，DictLabel=EcGroupName）"),
            // entity.ecme.techleader
            new TranslationSeedItem("entity.ecme.techleader", "zh-HK", "设变EC担当_hk", "设变EC担当（选项 TaktEcGroups/options?ecGroupCategory=1；DictValue=EcGroupCode，DictLabel=EcGroupName）"),

            // entity.ecme.isimplemented
            new TranslationSeedItem("entity.ecme.isimplemented", "en-US", "是否实施_us", "是否实施（制技不落库，不参与页签。页签按是否更新 IsSopUpdated：0=未更新，1=已更新）"),
            // entity.ecme.isimplemented
            new TranslationSeedItem("entity.ecme.isimplemented", "ja-JP", "是否实施_jp", "是否实施（制技不落库，不参与页签。页签按是否更新 IsSopUpdated：0=未更新，1=已更新）"),
            // entity.ecme.isimplemented
            new TranslationSeedItem("entity.ecme.isimplemented", "zh-CN", "是否实施", "是否实施（制技不落库，不参与页签。页签按是否更新 IsSopUpdated：0=未更新，1=已更新）"),
            // entity.ecme.isimplemented
            new TranslationSeedItem("entity.ecme.isimplemented", "zh-HK", "是否实施_hk", "是否实施（制技不落库，不参与页签。页签按是否更新 IsSopUpdated：0=未更新，1=已更新）"),

            // entity.ecme.execcontent
            new TranslationSeedItem("entity.ecme.execcontent", "en-US", "执行内容_us", "执行内容（制技不落库；仅满足 ITaktEcDeptExecEntity）"),
            // entity.ecme.execcontent
            new TranslationSeedItem("entity.ecme.execcontent", "ja-JP", "执行内容_jp", "执行内容（制技不落库；仅满足 ITaktEcDeptExecEntity）"),
            // entity.ecme.execcontent
            new TranslationSeedItem("entity.ecme.execcontent", "zh-CN", "执行内容", "执行内容（制技不落库；仅满足 ITaktEcDeptExecEntity）"),
            // entity.ecme.execcontent
            new TranslationSeedItem("entity.ecme.execcontent", "zh-HK", "执行内容_hk", "执行内容（制技不落库；仅满足 ITaktEcDeptExecEntity）"),

            // entity.ecme.ecrootmaterialdescription
            new TranslationSeedItem("entity.ecme.ecrootmaterialdescription", "en-US", "根物料描述_us", "根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）"),
            // entity.ecme.ecrootmaterialdescription
            new TranslationSeedItem("entity.ecme.ecrootmaterialdescription", "ja-JP", "根物料描述_jp", "根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）"),
            // entity.ecme.ecrootmaterialdescription
            new TranslationSeedItem("entity.ecme.ecrootmaterialdescription", "zh-CN", "根物料描述", "根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）"),
            // entity.ecme.ecrootmaterialdescription
            new TranslationSeedItem("entity.ecme.ecrootmaterialdescription", "zh-HK", "根物料描述_hk", "根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）"),

            // entity.ecme.deptcode
            new TranslationSeedItem("entity.ecme.deptcode", "en-US", "部门编码_us", "部门编码（TaktDept.DeptCode；本表固定课别）"),
            // entity.ecme.deptcode
            new TranslationSeedItem("entity.ecme.deptcode", "ja-JP", "部门编码_jp", "部门编码（TaktDept.DeptCode；本表固定课别）"),
            // entity.ecme.deptcode
            new TranslationSeedItem("entity.ecme.deptcode", "zh-CN", "部门编码", "部门编码（TaktDept.DeptCode；本表固定课别）"),
            // entity.ecme.deptcode
            new TranslationSeedItem("entity.ecme.deptcode", "zh-HK", "部门编码_hk", "部门编码（TaktDept.DeptCode；本表固定课别）"),

            // entity.ecme.deptname
            new TranslationSeedItem("entity.ecme.deptname", "en-US", "部门名称_us", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),
            // entity.ecme.deptname
            new TranslationSeedItem("entity.ecme.deptname", "ja-JP", "部门名称_jp", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),
            // entity.ecme.deptname
            new TranslationSeedItem("entity.ecme.deptname", "zh-CN", "部门名称", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),
            // entity.ecme.deptname
            new TranslationSeedItem("entity.ecme.deptname", "zh-HK", "部门名称_hk", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),

            // entity.ecme.ecimplscope
            new TranslationSeedItem("entity.ecme.ecimplscope", "en-US", "实施范围_us", "实施范围（冗余：来自 TaktEcDetail.EcImplScope）"),
            // entity.ecme.ecimplscope
            new TranslationSeedItem("entity.ecme.ecimplscope", "ja-JP", "实施范围_jp", "实施范围（冗余：来自 TaktEcDetail.EcImplScope）"),
            // entity.ecme.ecimplscope
            new TranslationSeedItem("entity.ecme.ecimplscope", "zh-CN", "实施范围", "实施范围（冗余：来自 TaktEcDetail.EcImplScope）"),
            // entity.ecme.ecimplscope
            new TranslationSeedItem("entity.ecme.ecimplscope", "zh-HK", "实施范围_hk", "实施范围（冗余：来自 TaktEcDetail.EcImplScope）"),

            // entity.ecme.eccontrolscope
            new TranslationSeedItem("entity.ecme.eccontrolscope", "en-US", "控制范围_us", "控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品，2=按机种，3=按设变，4=不管理），只记录生管/制二/制一/品管；全仕向时生管与此相同。采购/受检/部管/SMT 固定按设变，制技固定按机种"),
            // entity.ecme.eccontrolscope
            new TranslationSeedItem("entity.ecme.eccontrolscope", "ja-JP", "控制范围_jp", "控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品，2=按机种，3=按设变，4=不管理），只记录生管/制二/制一/品管；全仕向时生管与此相同。采购/受检/部管/SMT 固定按设变，制技固定按机种"),
            // entity.ecme.eccontrolscope
            new TranslationSeedItem("entity.ecme.eccontrolscope", "zh-CN", "控制范围", "控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品，2=按机种，3=按设变，4=不管理），只记录生管/制二/制一/品管；全仕向时生管与此相同。采购/受检/部管/SMT 固定按设变，制技固定按机种"),
            // entity.ecme.eccontrolscope
            new TranslationSeedItem("entity.ecme.eccontrolscope", "zh-HK", "控制范围_hk", "控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品，2=按机种，3=按设变，4=不管理），只记录生管/制二/制一/品管；全仕向时生管与此相同。采购/受检/部管/SMT 固定按设变，制技固定按机种"),

            // entity.ecme.ecgrainkey
            new TranslationSeedItem("entity.ecme.ecgrainkey", "en-US", "执行去重键_us", "执行行去重键（设变单号、机种或根物料或新物料、分道；同一键的明细共用本行。ntext）"),
            // entity.ecme.ecgrainkey
            new TranslationSeedItem("entity.ecme.ecgrainkey", "ja-JP", "执行去重键_jp", "执行行去重键（设变单号、机种或根物料或新物料、分道；同一键的明细共用本行。ntext）"),
            // entity.ecme.ecgrainkey
            new TranslationSeedItem("entity.ecme.ecgrainkey", "zh-CN", "执行去重键", "执行行去重键（设变单号、机种或根物料或新物料、分道；同一键的明细共用本行。ntext）"),
            // entity.ecme.ecgrainkey
            new TranslationSeedItem("entity.ecme.ecgrainkey", "zh-HK", "执行去重键_hk", "执行行去重键（设变单号、机种或根物料或新物料、分道；同一键的明细共用本行。ntext）"),

            // entity.ecme.isobsolete
            new TranslationSeedItem("entity.ecme.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecme.isobsolete
            new TranslationSeedItem("entity.ecme.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecme.isobsolete
            new TranslationSeedItem("entity.ecme.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecme.isobsolete
            new TranslationSeedItem("entity.ecme.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
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
