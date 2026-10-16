// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcSeizougijutsuI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEcSeizougijutsu 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktEcSeizougijutsu 实体国际化翻译种子（键前缀 entity.ecseizougijutsu.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEcSeizougijutsuI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEcSeizougijutsu 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 ecseizougijutsu 实体翻译...", tenantCode);

        foreach (var item in GetEcSeizougijutsuTranslations())
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

        TaktLogger.Information("TaktEcSeizougijutsu 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEcSeizougijutsu 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.ecseizougijutsu._self / entity.ecseizougijutsu.{{field}}；ResourceGroup=EngineeringChange；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEcSeizougijutsuTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.ecseizougijutsu._self
            new TranslationSeedItem("entity.ecseizougijutsu._self", "en-US", "Ec Seizougijutsu Information_us", "实体名称"),
            // entity.ecseizougijutsu._self
            new TranslationSeedItem("entity.ecseizougijutsu._self", "ja-JP", "设变制造技术课信息_jp", "实体名称"),
            // entity.ecseizougijutsu._self
            new TranslationSeedItem("entity.ecseizougijutsu._self", "zh-CN", "设变制造技术课信息", "实体名称"),
            // entity.ecseizougijutsu._self
            new TranslationSeedItem("entity.ecseizougijutsu._self", "zh-HK", "设变制造技术课信息_hk", "实体名称"),

            // entity.ecseizougijutsu.linenumber
            new TranslationSeedItem("entity.ecseizougijutsu.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.ecseizougijutsu.linenumber
            new TranslationSeedItem("entity.ecseizougijutsu.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.ecseizougijutsu.linenumber
            new TranslationSeedItem("entity.ecseizougijutsu.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.ecseizougijutsu.linenumber
            new TranslationSeedItem("entity.ecseizougijutsu.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.ecseizougijutsu.eccode
            new TranslationSeedItem("entity.ecseizougijutsu.eccode", "en-US", "设变单号_us", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),
            // entity.ecseizougijutsu.eccode
            new TranslationSeedItem("entity.ecseizougijutsu.eccode", "ja-JP", "设变单号_jp", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),
            // entity.ecseizougijutsu.eccode
            new TranslationSeedItem("entity.ecseizougijutsu.eccode", "zh-CN", "设变单号", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),
            // entity.ecseizougijutsu.eccode
            new TranslationSeedItem("entity.ecseizougijutsu.eccode", "zh-HK", "设变单号_hk", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),

            // entity.ecseizougijutsu.discontinuedstatus
            new TranslationSeedItem("entity.ecseizougijutsu.discontinuedstatus", "en-US", "停产状态_us", "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）"),
            // entity.ecseizougijutsu.discontinuedstatus
            new TranslationSeedItem("entity.ecseizougijutsu.discontinuedstatus", "ja-JP", "停产状态_jp", "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）"),
            // entity.ecseizougijutsu.discontinuedstatus
            new TranslationSeedItem("entity.ecseizougijutsu.discontinuedstatus", "zh-CN", "停产状态", "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）"),
            // entity.ecseizougijutsu.discontinuedstatus
            new TranslationSeedItem("entity.ecseizougijutsu.discontinuedstatus", "zh-HK", "停产状态_hk", "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）"),

            // entity.ecseizougijutsu.ecmodelcode
            new TranslationSeedItem("entity.ecseizougijutsu.ecmodelcode", "en-US", "机种编码_us", "机种编码（冗余：来自 TaktEcDetail.EcModelCode）"),
            // entity.ecseizougijutsu.ecmodelcode
            new TranslationSeedItem("entity.ecseizougijutsu.ecmodelcode", "ja-JP", "机种编码_jp", "机种编码（冗余：来自 TaktEcDetail.EcModelCode）"),
            // entity.ecseizougijutsu.ecmodelcode
            new TranslationSeedItem("entity.ecseizougijutsu.ecmodelcode", "zh-CN", "机种编码", "机种编码（冗余：来自 TaktEcDetail.EcModelCode）"),
            // entity.ecseizougijutsu.ecmodelcode
            new TranslationSeedItem("entity.ecseizougijutsu.ecmodelcode", "zh-HK", "机种编码_hk", "机种编码（冗余：来自 TaktEcDetail.EcModelCode）"),

            // entity.ecseizougijutsu.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.ecseizougijutsu.ecrootmaterialcodegroup", "en-US", "根物料编码组_us", "根物料编码组（同去重键下的根物料编码，分号分隔；ntext）"),
            // entity.ecseizougijutsu.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.ecseizougijutsu.ecrootmaterialcodegroup", "ja-JP", "根物料编码组_jp", "根物料编码组（同去重键下的根物料编码，分号分隔；ntext）"),
            // entity.ecseizougijutsu.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.ecseizougijutsu.ecrootmaterialcodegroup", "zh-CN", "根物料编码组", "根物料编码组（同去重键下的根物料编码，分号分隔；ntext）"),
            // entity.ecseizougijutsu.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.ecseizougijutsu.ecrootmaterialcodegroup", "zh-HK", "根物料编码组_hk", "根物料编码组（同去重键下的根物料编码，分号分隔；ntext）"),

            // entity.ecseizougijutsu.issopupdated
            new TranslationSeedItem("entity.ecseizougijutsu.issopupdated", "en-US", "更新SOP_us", "是否更新 SOP（字典 sys_yes_no；0=否 1=是）"),
            // entity.ecseizougijutsu.issopupdated
            new TranslationSeedItem("entity.ecseizougijutsu.issopupdated", "ja-JP", "更新SOP_jp", "是否更新 SOP（字典 sys_yes_no；0=否 1=是）"),
            // entity.ecseizougijutsu.issopupdated
            new TranslationSeedItem("entity.ecseizougijutsu.issopupdated", "zh-CN", "更新SOP", "是否更新 SOP（字典 sys_yes_no；0=否 1=是）"),
            // entity.ecseizougijutsu.issopupdated
            new TranslationSeedItem("entity.ecseizougijutsu.issopupdated", "zh-HK", "更新SOP_hk", "是否更新 SOP（字典 sys_yes_no；0=否 1=是）"),

            // entity.ecseizougijutsu.sopleader
            new TranslationSeedItem("entity.ecseizougijutsu.sopleader", "en-US", "SOP担当_us", "SOP担当（选项 TaktEcGroups/options?ecGroupCategory=2；DictValue=EcGroupCode，DictLabel=EcGroupName）"),
            // entity.ecseizougijutsu.sopleader
            new TranslationSeedItem("entity.ecseizougijutsu.sopleader", "ja-JP", "SOP担当_jp", "SOP担当（选项 TaktEcGroups/options?ecGroupCategory=2；DictValue=EcGroupCode，DictLabel=EcGroupName）"),
            // entity.ecseizougijutsu.sopleader
            new TranslationSeedItem("entity.ecseizougijutsu.sopleader", "zh-CN", "SOP担当", "SOP担当（选项 TaktEcGroups/options?ecGroupCategory=2；DictValue=EcGroupCode，DictLabel=EcGroupName）"),
            // entity.ecseizougijutsu.sopleader
            new TranslationSeedItem("entity.ecseizougijutsu.sopleader", "zh-HK", "SOP担当_hk", "SOP担当（选项 TaktEcGroups/options?ecGroupCategory=2；DictValue=EcGroupCode，DictLabel=EcGroupName）"),

            // entity.ecseizougijutsu.sopdate
            new TranslationSeedItem("entity.ecseizougijutsu.sopdate", "en-US", "SOP日期_us", "SOP日期"),
            // entity.ecseizougijutsu.sopdate
            new TranslationSeedItem("entity.ecseizougijutsu.sopdate", "ja-JP", "SOP日期_jp", "SOP日期"),
            // entity.ecseizougijutsu.sopdate
            new TranslationSeedItem("entity.ecseizougijutsu.sopdate", "zh-CN", "SOP日期", "SOP日期"),
            // entity.ecseizougijutsu.sopdate
            new TranslationSeedItem("entity.ecseizougijutsu.sopdate", "zh-HK", "SOP日期_hk", "SOP日期"),

            // entity.ecseizougijutsu.techleader
            new TranslationSeedItem("entity.ecseizougijutsu.techleader", "en-US", "设变EC担当_us", "设变EC担当（选项 TaktEcGroups/options?ecGroupCategory=1；DictValue=EcGroupCode，DictLabel=EcGroupName）"),
            // entity.ecseizougijutsu.techleader
            new TranslationSeedItem("entity.ecseizougijutsu.techleader", "ja-JP", "设变EC担当_jp", "设变EC担当（选项 TaktEcGroups/options?ecGroupCategory=1；DictValue=EcGroupCode，DictLabel=EcGroupName）"),
            // entity.ecseizougijutsu.techleader
            new TranslationSeedItem("entity.ecseizougijutsu.techleader", "zh-CN", "设变EC担当", "设变EC担当（选项 TaktEcGroups/options?ecGroupCategory=1；DictValue=EcGroupCode，DictLabel=EcGroupName）"),
            // entity.ecseizougijutsu.techleader
            new TranslationSeedItem("entity.ecseizougijutsu.techleader", "zh-HK", "设变EC担当_hk", "设变EC担当（选项 TaktEcGroups/options?ecGroupCategory=1；DictValue=EcGroupCode，DictLabel=EcGroupName）"),

            // entity.ecseizougijutsu.isimplemented
            new TranslationSeedItem("entity.ecseizougijutsu.isimplemented", "en-US", "是否实施_us", "是否实施（制技不落库；仅满足 ITaktEcDeptExecEntity；有输入判定见 TaktEcDeptEntityHelper.HasDeptInput）"),
            // entity.ecseizougijutsu.isimplemented
            new TranslationSeedItem("entity.ecseizougijutsu.isimplemented", "ja-JP", "是否实施_jp", "是否实施（制技不落库；仅满足 ITaktEcDeptExecEntity；有输入判定见 TaktEcDeptEntityHelper.HasDeptInput）"),
            // entity.ecseizougijutsu.isimplemented
            new TranslationSeedItem("entity.ecseizougijutsu.isimplemented", "zh-CN", "是否实施", "是否实施（制技不落库；仅满足 ITaktEcDeptExecEntity；有输入判定见 TaktEcDeptEntityHelper.HasDeptInput）"),
            // entity.ecseizougijutsu.isimplemented
            new TranslationSeedItem("entity.ecseizougijutsu.isimplemented", "zh-HK", "是否实施_hk", "是否实施（制技不落库；仅满足 ITaktEcDeptExecEntity；有输入判定见 TaktEcDeptEntityHelper.HasDeptInput）"),

            // entity.ecseizougijutsu.execcontent
            new TranslationSeedItem("entity.ecseizougijutsu.execcontent", "en-US", "执行内容_us", "执行内容（制技不落库；仅满足 ITaktEcDeptExecEntity）"),
            // entity.ecseizougijutsu.execcontent
            new TranslationSeedItem("entity.ecseizougijutsu.execcontent", "ja-JP", "执行内容_jp", "执行内容（制技不落库；仅满足 ITaktEcDeptExecEntity）"),
            // entity.ecseizougijutsu.execcontent
            new TranslationSeedItem("entity.ecseizougijutsu.execcontent", "zh-CN", "执行内容", "执行内容（制技不落库；仅满足 ITaktEcDeptExecEntity）"),
            // entity.ecseizougijutsu.execcontent
            new TranslationSeedItem("entity.ecseizougijutsu.execcontent", "zh-HK", "执行内容_hk", "执行内容（制技不落库；仅满足 ITaktEcDeptExecEntity）"),

            // entity.ecseizougijutsu.ecrootmaterialdescription
            new TranslationSeedItem("entity.ecseizougijutsu.ecrootmaterialdescription", "en-US", "根物料描述_us", "根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）"),
            // entity.ecseizougijutsu.ecrootmaterialdescription
            new TranslationSeedItem("entity.ecseizougijutsu.ecrootmaterialdescription", "ja-JP", "根物料描述_jp", "根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）"),
            // entity.ecseizougijutsu.ecrootmaterialdescription
            new TranslationSeedItem("entity.ecseizougijutsu.ecrootmaterialdescription", "zh-CN", "根物料描述", "根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）"),
            // entity.ecseizougijutsu.ecrootmaterialdescription
            new TranslationSeedItem("entity.ecseizougijutsu.ecrootmaterialdescription", "zh-HK", "根物料描述_hk", "根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）"),

            // entity.ecseizougijutsu.deptcode
            new TranslationSeedItem("entity.ecseizougijutsu.deptcode", "en-US", "部门编码_us", "部门编码（TaktDept.DeptCode；本表固定课别）"),
            // entity.ecseizougijutsu.deptcode
            new TranslationSeedItem("entity.ecseizougijutsu.deptcode", "ja-JP", "部门编码_jp", "部门编码（TaktDept.DeptCode；本表固定课别）"),
            // entity.ecseizougijutsu.deptcode
            new TranslationSeedItem("entity.ecseizougijutsu.deptcode", "zh-CN", "部门编码", "部门编码（TaktDept.DeptCode；本表固定课别）"),
            // entity.ecseizougijutsu.deptcode
            new TranslationSeedItem("entity.ecseizougijutsu.deptcode", "zh-HK", "部门编码_hk", "部门编码（TaktDept.DeptCode；本表固定课别）"),

            // entity.ecseizougijutsu.deptname
            new TranslationSeedItem("entity.ecseizougijutsu.deptname", "en-US", "部门名称_us", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),
            // entity.ecseizougijutsu.deptname
            new TranslationSeedItem("entity.ecseizougijutsu.deptname", "ja-JP", "部门名称_jp", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),
            // entity.ecseizougijutsu.deptname
            new TranslationSeedItem("entity.ecseizougijutsu.deptname", "zh-CN", "部门名称", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),
            // entity.ecseizougijutsu.deptname
            new TranslationSeedItem("entity.ecseizougijutsu.deptname", "zh-HK", "部门名称_hk", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),

            // entity.ecseizougijutsu.ecscope
            new TranslationSeedItem("entity.ecseizougijutsu.ecscope", "en-US", "实施范围_us", "实施范围（冗余：来自 TaktEcDetail.EcScope）"),
            // entity.ecseizougijutsu.ecscope
            new TranslationSeedItem("entity.ecseizougijutsu.ecscope", "ja-JP", "实施范围_jp", "实施范围（冗余：来自 TaktEcDetail.EcScope）"),
            // entity.ecseizougijutsu.ecscope
            new TranslationSeedItem("entity.ecseizougijutsu.ecscope", "zh-CN", "实施范围", "实施范围（冗余：来自 TaktEcDetail.EcScope）"),
            // entity.ecseizougijutsu.ecscope
            new TranslationSeedItem("entity.ecseizougijutsu.ecscope", "zh-HK", "实施范围_hk", "实施范围（冗余：来自 TaktEcDetail.EcScope）"),

            // entity.ecseizougijutsu.ecdeptscope
            new TranslationSeedItem("entity.ecseizougijutsu.ecdeptscope", "en-US", "部门实施范围_us", "部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）"),
            // entity.ecseizougijutsu.ecdeptscope
            new TranslationSeedItem("entity.ecseizougijutsu.ecdeptscope", "ja-JP", "部门实施范围_jp", "部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）"),
            // entity.ecseizougijutsu.ecdeptscope
            new TranslationSeedItem("entity.ecseizougijutsu.ecdeptscope", "zh-CN", "部门实施范围", "部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）"),
            // entity.ecseizougijutsu.ecdeptscope
            new TranslationSeedItem("entity.ecseizougijutsu.ecdeptscope", "zh-HK", "部门实施范围_hk", "部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）"),

            // entity.ecseizougijutsu.ecdetailid
            new TranslationSeedItem("entity.ecseizougijutsu.ecdetailid", "en-US", "设变明细ID_us", "设变明细 ID（TaktEcDetail 主键；去重组内代表/种子明细 Id；同组多明细按业务键 FanOut）"),
            // entity.ecseizougijutsu.ecdetailid
            new TranslationSeedItem("entity.ecseizougijutsu.ecdetailid", "ja-JP", "设变明细ID_jp", "设变明细 ID（TaktEcDetail 主键；去重组内代表/种子明细 Id；同组多明细按业务键 FanOut）"),
            // entity.ecseizougijutsu.ecdetailid
            new TranslationSeedItem("entity.ecseizougijutsu.ecdetailid", "zh-CN", "设变明细ID", "设变明细 ID（TaktEcDetail 主键；去重组内代表/种子明细 Id；同组多明细按业务键 FanOut）"),
            // entity.ecseizougijutsu.ecdetailid
            new TranslationSeedItem("entity.ecseizougijutsu.ecdetailid", "zh-HK", "设变明细ID_hk", "设变明细 ID（TaktEcDetail 主键；去重组内代表/种子明细 Id；同组多明细按业务键 FanOut）"),

            // entity.ecseizougijutsu.isobsolete
            new TranslationSeedItem("entity.ecseizougijutsu.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecseizougijutsu.isobsolete
            new TranslationSeedItem("entity.ecseizougijutsu.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecseizougijutsu.isobsolete
            new TranslationSeedItem("entity.ecseizougijutsu.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecseizougijutsu.isobsolete
            new TranslationSeedItem("entity.ecseizougijutsu.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.ecseizougijutsu.ecdetail
            new TranslationSeedItem("entity.ecseizougijutsu.ecdetail", "en-US", "设变明细_us", "设变明细（数据主从：本表由明细派生；多对一，外键 EcDetailId → TaktEcDetail.Id）"),
            // entity.ecseizougijutsu.ecdetail
            new TranslationSeedItem("entity.ecseizougijutsu.ecdetail", "ja-JP", "设变明细_jp", "设变明细（数据主从：本表由明细派生；多对一，外键 EcDetailId → TaktEcDetail.Id）"),
            // entity.ecseizougijutsu.ecdetail
            new TranslationSeedItem("entity.ecseizougijutsu.ecdetail", "zh-CN", "设变明细", "设变明细（数据主从：本表由明细派生；多对一，外键 EcDetailId → TaktEcDetail.Id）"),
            // entity.ecseizougijutsu.ecdetail
            new TranslationSeedItem("entity.ecseizougijutsu.ecdetail", "zh-HK", "设变明细_hk", "设变明细（数据主从：本表由明细派生；多对一，外键 EcDetailId → TaktEcDetail.Id）"),
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
