// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcSeizounikaI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEcSeizounika 实体字段国际化种子（已对齐前端 locales：src/locales/logistics/manufacturing/engineering-change/ec-seizounika）
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
/// TaktEcSeizounika 实体国际化翻译种子（键前缀 entity.ecseizounika.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEcSeizounikaI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEcSeizounika 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 ecseizounika 实体翻译...", tenantCode);

        foreach (var item in GetEcSeizounikaTranslations())
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

        TaktLogger.Information("TaktEcSeizounika 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEcSeizounika 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.ecseizounika._self / entity.ecseizounika.{{field}}；ResourceGroup=EngineeringChange；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEcSeizounikaTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.ecseizounika._self
            new TranslationSeedItem("entity.ecseizounika._self", "en-US", "Ec Seizounika Information_us", "实体名称"),
            // entity.ecseizounika._self
            new TranslationSeedItem("entity.ecseizounika._self", "ja-JP", "设变制造2课信息_jp", "实体名称"),
            // entity.ecseizounika._self
            new TranslationSeedItem("entity.ecseizounika._self", "zh-CN", "设变制造2课信息", "实体名称"),
            // entity.ecseizounika._self
            new TranslationSeedItem("entity.ecseizounika._self", "zh-HK", "设变制造2课信息_hk", "实体名称"),

            // entity.ecseizounika.linenumber
            new TranslationSeedItem("entity.ecseizounika.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.ecseizounika.linenumber
            new TranslationSeedItem("entity.ecseizounika.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.ecseizounika.linenumber
            new TranslationSeedItem("entity.ecseizounika.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.ecseizounika.linenumber
            new TranslationSeedItem("entity.ecseizounika.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.ecseizounika.eccode
            new TranslationSeedItem("entity.ecseizounika.eccode", "en-US", "设变单号_us", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),
            // entity.ecseizounika.eccode
            new TranslationSeedItem("entity.ecseizounika.eccode", "ja-JP", "设变单号_jp", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),
            // entity.ecseizounika.eccode
            new TranslationSeedItem("entity.ecseizounika.eccode", "zh-CN", "设变单号", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),
            // entity.ecseizounika.eccode
            new TranslationSeedItem("entity.ecseizounika.eccode", "zh-HK", "设变单号_hk", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),

            // entity.ecseizounika.discontinuedstatus
            new TranslationSeedItem("entity.ecseizounika.discontinuedstatus", "en-US", "停产状态_us", "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）"),
            // entity.ecseizounika.discontinuedstatus
            new TranslationSeedItem("entity.ecseizounika.discontinuedstatus", "ja-JP", "停产状态_jp", "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）"),
            // entity.ecseizounika.discontinuedstatus
            new TranslationSeedItem("entity.ecseizounika.discontinuedstatus", "zh-CN", "停产状态", "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）"),
            // entity.ecseizounika.discontinuedstatus
            new TranslationSeedItem("entity.ecseizounika.discontinuedstatus", "zh-HK", "停产状态_hk", "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）"),

            // entity.ecseizounika.ecmodelcodegroup
            new TranslationSeedItem("entity.ecseizounika.ecmodelcodegroup", "en-US", "机种编码组_us", "机种编码组（单个或多个机种，分号分隔；ntext）"),
            // entity.ecseizounika.ecmodelcodegroup
            new TranslationSeedItem("entity.ecseizounika.ecmodelcodegroup", "ja-JP", "机种编码组_jp", "机种编码组（单个或多个机种，分号分隔；ntext）"),
            // entity.ecseizounika.ecmodelcodegroup
            new TranslationSeedItem("entity.ecseizounika.ecmodelcodegroup", "zh-CN", "机种编码组", "机种编码组（单个或多个机种，分号分隔；ntext）"),
            // entity.ecseizounika.ecmodelcodegroup
            new TranslationSeedItem("entity.ecseizounika.ecmodelcodegroup", "zh-HK", "机种编码组_hk", "机种编码组（单个或多个机种，分号分隔；ntext）"),

            // entity.ecseizounika.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.ecseizounika.ecrootmaterialcodegroup", "en-US", "根物料编码组_us", "根物料编码组（同去重键下的根物料编码，分号分隔；ntext，按机种时同机种根物料可能很多）"),
            // entity.ecseizounika.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.ecseizounika.ecrootmaterialcodegroup", "ja-JP", "根物料编码组_jp", "根物料编码组（同去重键下的根物料编码，分号分隔；ntext，按机种时同机种根物料可能很多）"),
            // entity.ecseizounika.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.ecseizounika.ecrootmaterialcodegroup", "zh-CN", "根物料编码组", "根物料编码组（同去重键下的根物料编码，分号分隔；ntext，按机种时同机种根物料可能很多）"),
            // entity.ecseizounika.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.ecseizounika.ecrootmaterialcodegroup", "zh-HK", "根物料编码组_hk", "根物料编码组（同去重键下的根物料编码，分号分隔；ntext，按机种时同机种根物料可能很多）"),

            // entity.ecseizounika.ecnewmaterialcode
            new TranslationSeedItem("entity.ecseizounika.ecnewmaterialcode", "en-US", "新物料编码_us", "新物料编码（按成品/按机种参与去重；冗余：来自 TaktEcDetail.EcNewMaterialCode）"),
            // entity.ecseizounika.ecnewmaterialcode
            new TranslationSeedItem("entity.ecseizounika.ecnewmaterialcode", "ja-JP", "新物料编码_jp", "新物料编码（按成品/按机种参与去重；冗余：来自 TaktEcDetail.EcNewMaterialCode）"),
            // entity.ecseizounika.ecnewmaterialcode
            new TranslationSeedItem("entity.ecseizounika.ecnewmaterialcode", "zh-CN", "新物料编码", "新物料编码（按成品/按机种参与去重；冗余：来自 TaktEcDetail.EcNewMaterialCode）"),
            // entity.ecseizounika.ecnewmaterialcode
            new TranslationSeedItem("entity.ecseizounika.ecnewmaterialcode", "zh-HK", "新物料编码_hk", "新物料编码（按成品/按机种参与去重；冗余：来自 TaktEcDetail.EcNewMaterialCode）"),

            // entity.ecseizounika.ecnewmaterialcodegroup
            new TranslationSeedItem("entity.ecseizounika.ecnewmaterialcodegroup", "en-US", "新物料编码组_us", "新物料编码组（按设变时同设变单号下的新物料编码，分号分隔；ntext）"),
            // entity.ecseizounika.ecnewmaterialcodegroup
            new TranslationSeedItem("entity.ecseizounika.ecnewmaterialcodegroup", "ja-JP", "新物料编码组_jp", "新物料编码组（按设变时同设变单号下的新物料编码，分号分隔；ntext）"),
            // entity.ecseizounika.ecnewmaterialcodegroup
            new TranslationSeedItem("entity.ecseizounika.ecnewmaterialcodegroup", "zh-CN", "新物料编码组", "新物料编码组（按设变时同设变单号下的新物料编码，分号分隔；ntext）"),
            // entity.ecseizounika.ecnewmaterialcodegroup
            new TranslationSeedItem("entity.ecseizounika.ecnewmaterialcodegroup", "zh-HK", "新物料编码组_hk", "新物料编码组（按设变时同设变单号下的新物料编码，分号分隔；ntext）"),

            // entity.ecseizounika.ecnewmaterialdescription
            new TranslationSeedItem("entity.ecseizounika.ecnewmaterialdescription", "en-US", "新物料描述_us", "新物料描述（冗余：来自 TaktEcDetail.EcNewMaterialDescription）"),
            // entity.ecseizounika.ecnewmaterialdescription
            new TranslationSeedItem("entity.ecseizounika.ecnewmaterialdescription", "ja-JP", "新物料描述_jp", "新物料描述（冗余：来自 TaktEcDetail.EcNewMaterialDescription）"),
            // entity.ecseizounika.ecnewmaterialdescription
            new TranslationSeedItem("entity.ecseizounika.ecnewmaterialdescription", "zh-CN", "新物料描述", "新物料描述（冗余：来自 TaktEcDetail.EcNewMaterialDescription）"),
            // entity.ecseizounika.ecnewmaterialdescription
            new TranslationSeedItem("entity.ecseizounika.ecnewmaterialdescription", "zh-HK", "新物料描述_hk", "新物料描述（冗余：来自 TaktEcDetail.EcNewMaterialDescription）"),

            // entity.ecseizounika.ecparentmaterialcode
            new TranslationSeedItem("entity.ecseizounika.ecparentmaterialcode", "en-US", "上阶物料编码_us", "上阶物料编码（冗余：来自 TaktEcDetail.EcParentMaterialCode）"),
            // entity.ecseizounika.ecparentmaterialcode
            new TranslationSeedItem("entity.ecseizounika.ecparentmaterialcode", "ja-JP", "上阶物料编码_jp", "上阶物料编码（冗余：来自 TaktEcDetail.EcParentMaterialCode）"),
            // entity.ecseizounika.ecparentmaterialcode
            new TranslationSeedItem("entity.ecseizounika.ecparentmaterialcode", "zh-CN", "上阶物料编码", "上阶物料编码（冗余：来自 TaktEcDetail.EcParentMaterialCode）"),
            // entity.ecseizounika.ecparentmaterialcode
            new TranslationSeedItem("entity.ecseizounika.ecparentmaterialcode", "zh-HK", "上阶物料编码_hk", "上阶物料编码（冗余：来自 TaktEcDetail.EcParentMaterialCode）"),

            // entity.ecseizounika.ecparentmaterialcodegroup
            new TranslationSeedItem("entity.ecseizounika.ecparentmaterialcodegroup", "en-US", "上阶物料编码组_us", "上阶物料编码组（按设变时同设变单号下的上阶物料编码，分号分隔；ntext）"),
            // entity.ecseizounika.ecparentmaterialcodegroup
            new TranslationSeedItem("entity.ecseizounika.ecparentmaterialcodegroup", "ja-JP", "上阶物料编码组_jp", "上阶物料编码组（按设变时同设变单号下的上阶物料编码，分号分隔；ntext）"),
            // entity.ecseizounika.ecparentmaterialcodegroup
            new TranslationSeedItem("entity.ecseizounika.ecparentmaterialcodegroup", "zh-CN", "上阶物料编码组", "上阶物料编码组（按设变时同设变单号下的上阶物料编码，分号分隔；ntext）"),
            // entity.ecseizounika.ecparentmaterialcodegroup
            new TranslationSeedItem("entity.ecseizounika.ecparentmaterialcodegroup", "zh-HK", "上阶物料编码组_hk", "上阶物料编码组（按设变时同设变单号下的上阶物料编码，分号分隔；ntext）"),

            // entity.ecseizounika.ecparentmaterialdescription
            new TranslationSeedItem("entity.ecseizounika.ecparentmaterialdescription", "en-US", "上阶物料描述_us", "上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）"),
            // entity.ecseizounika.ecparentmaterialdescription
            new TranslationSeedItem("entity.ecseizounika.ecparentmaterialdescription", "ja-JP", "上阶物料描述_jp", "上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）"),
            // entity.ecseizounika.ecparentmaterialdescription
            new TranslationSeedItem("entity.ecseizounika.ecparentmaterialdescription", "zh-CN", "上阶物料描述", "上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）"),
            // entity.ecseizounika.ecparentmaterialdescription
            new TranslationSeedItem("entity.ecseizounika.ecparentmaterialdescription", "zh-HK", "上阶物料描述_hk", "上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）"),

            // entity.ecseizounika.isimplemented
            new TranslationSeedItem("entity.ecseizounika.isimplemented", "en-US", "实施_us", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.ecseizounika.isimplemented
            new TranslationSeedItem("entity.ecseizounika.isimplemented", "ja-JP", "实施_jp", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.ecseizounika.isimplemented
            new TranslationSeedItem("entity.ecseizounika.isimplemented", "zh-CN", "实施", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.ecseizounika.isimplemented
            new TranslationSeedItem("entity.ecseizounika.isimplemented", "zh-HK", "实施_hk", "是否实施（0=否 1=是，字典 sys_yes_no）"),

            // entity.ecseizounika.scheduleddate
            new TranslationSeedItem("entity.ecseizounika.scheduleddate", "en-US", "预定日期_us", "预定日期（冗余：来自 TaktEcSeikan.ScheduledDate）"),
            // entity.ecseizounika.scheduleddate
            new TranslationSeedItem("entity.ecseizounika.scheduleddate", "ja-JP", "预定日期_jp", "预定日期（冗余：来自 TaktEcSeikan.ScheduledDate）"),
            // entity.ecseizounika.scheduleddate
            new TranslationSeedItem("entity.ecseizounika.scheduleddate", "zh-CN", "预定日期", "预定日期（冗余：来自 TaktEcSeikan.ScheduledDate）"),
            // entity.ecseizounika.scheduleddate
            new TranslationSeedItem("entity.ecseizounika.scheduleddate", "zh-HK", "预定日期_hk", "预定日期（冗余：来自 TaktEcSeikan.ScheduledDate）"),

            // entity.ecseizounika.scheduledbatch
            new TranslationSeedItem("entity.ecseizounika.scheduledbatch", "en-US", "预定批次_us", "预定批次（冗余：来自 TaktEcSeikan.ScheduledBatch）"),
            // entity.ecseizounika.scheduledbatch
            new TranslationSeedItem("entity.ecseizounika.scheduledbatch", "ja-JP", "预定批次_jp", "预定批次（冗余：来自 TaktEcSeikan.ScheduledBatch）"),
            // entity.ecseizounika.scheduledbatch
            new TranslationSeedItem("entity.ecseizounika.scheduledbatch", "zh-CN", "预定批次", "预定批次（冗余：来自 TaktEcSeikan.ScheduledBatch）"),
            // entity.ecseizounika.scheduledbatch
            new TranslationSeedItem("entity.ecseizounika.scheduledbatch", "zh-HK", "预定批次_hk", "预定批次（冗余：来自 TaktEcSeikan.ScheduledBatch）"),

            // entity.ecseizounika.execcontent
            new TranslationSeedItem("entity.ecseizounika.execcontent", "en-US", "执行内容_us", "执行内容（各部门通用）"),
            // entity.ecseizounika.execcontent
            new TranslationSeedItem("entity.ecseizounika.execcontent", "ja-JP", "执行内容_jp", "执行内容（各部门通用）"),
            // entity.ecseizounika.execcontent
            new TranslationSeedItem("entity.ecseizounika.execcontent", "zh-CN", "执行内容", "执行内容（各部门通用）"),
            // entity.ecseizounika.execcontent
            new TranslationSeedItem("entity.ecseizounika.execcontent", "zh-HK", "执行内容_hk", "执行内容（各部门通用）"),

            // entity.ecseizounika.productionteam
            new TranslationSeedItem("entity.ecseizounika.productionteam", "en-US", "生产班组_us", "生产班组"),
            // entity.ecseizounika.productionteam
            new TranslationSeedItem("entity.ecseizounika.productionteam", "ja-JP", "生产班组_jp", "生产班组"),
            // entity.ecseizounika.productionteam
            new TranslationSeedItem("entity.ecseizounika.productionteam", "zh-CN", "生产班组", "生产班组"),
            // entity.ecseizounika.productionteam
            new TranslationSeedItem("entity.ecseizounika.productionteam", "zh-HK", "生产班组_hk", "生产班组"),

            // entity.ecseizounika.productiondate
            new TranslationSeedItem("entity.ecseizounika.productiondate", "en-US", "生产日期_us", "生产日期"),
            // entity.ecseizounika.productiondate
            new TranslationSeedItem("entity.ecseizounika.productiondate", "ja-JP", "生产日期_jp", "生产日期"),
            // entity.ecseizounika.productiondate
            new TranslationSeedItem("entity.ecseizounika.productiondate", "zh-CN", "生产日期", "生产日期"),
            // entity.ecseizounika.productiondate
            new TranslationSeedItem("entity.ecseizounika.productiondate", "zh-HK", "生产日期_hk", "生产日期"),

            // entity.ecseizounika.implementationbatch
            new TranslationSeedItem("entity.ecseizounika.implementationbatch", "en-US", "实施批次_us", "实施批次"),
            // entity.ecseizounika.implementationbatch
            new TranslationSeedItem("entity.ecseizounika.implementationbatch", "ja-JP", "实施批次_jp", "实施批次"),
            // entity.ecseizounika.implementationbatch
            new TranslationSeedItem("entity.ecseizounika.implementationbatch", "zh-CN", "实施批次", "实施批次"),
            // entity.ecseizounika.implementationbatch
            new TranslationSeedItem("entity.ecseizounika.implementationbatch", "zh-HK", "实施批次_hk", "实施批次"),

            // entity.ecseizounika.ecrootmaterialdescription
            new TranslationSeedItem("entity.ecseizounika.ecrootmaterialdescription", "en-US", "根物料描述_us", "根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）"),
            // entity.ecseizounika.ecrootmaterialdescription
            new TranslationSeedItem("entity.ecseizounika.ecrootmaterialdescription", "ja-JP", "根物料描述_jp", "根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）"),
            // entity.ecseizounika.ecrootmaterialdescription
            new TranslationSeedItem("entity.ecseizounika.ecrootmaterialdescription", "zh-CN", "根物料描述", "根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）"),
            // entity.ecseizounika.ecrootmaterialdescription
            new TranslationSeedItem("entity.ecseizounika.ecrootmaterialdescription", "zh-HK", "根物料描述_hk", "根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）"),

            // entity.ecseizounika.deptcode
            new TranslationSeedItem("entity.ecseizounika.deptcode", "en-US", "部门编码_us", "部门编码（TaktDept.DeptCode；本表固定课别）"),
            // entity.ecseizounika.deptcode
            new TranslationSeedItem("entity.ecseizounika.deptcode", "ja-JP", "部门编码_jp", "部门编码（TaktDept.DeptCode；本表固定课别）"),
            // entity.ecseizounika.deptcode
            new TranslationSeedItem("entity.ecseizounika.deptcode", "zh-CN", "部门编码", "部门编码（TaktDept.DeptCode；本表固定课别）"),
            // entity.ecseizounika.deptcode
            new TranslationSeedItem("entity.ecseizounika.deptcode", "zh-HK", "部门编码_hk", "部门编码（TaktDept.DeptCode；本表固定课别）"),

            // entity.ecseizounika.deptname
            new TranslationSeedItem("entity.ecseizounika.deptname", "en-US", "部门名称_us", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),
            // entity.ecseizounika.deptname
            new TranslationSeedItem("entity.ecseizounika.deptname", "ja-JP", "部门名称_jp", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),
            // entity.ecseizounika.deptname
            new TranslationSeedItem("entity.ecseizounika.deptname", "zh-CN", "部门名称", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),
            // entity.ecseizounika.deptname
            new TranslationSeedItem("entity.ecseizounika.deptname", "zh-HK", "部门名称_hk", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),

            // entity.ecseizounika.ecscope
            new TranslationSeedItem("entity.ecseizounika.ecscope", "en-US", "实施范围_us", "实施范围（冗余：来自 TaktEcDetail.EcScope）"),
            // entity.ecseizounika.ecscope
            new TranslationSeedItem("entity.ecseizounika.ecscope", "ja-JP", "实施范围_jp", "实施范围（冗余：来自 TaktEcDetail.EcScope）"),
            // entity.ecseizounika.ecscope
            new TranslationSeedItem("entity.ecseizounika.ecscope", "zh-CN", "实施范围", "实施范围（冗余：来自 TaktEcDetail.EcScope）"),
            // entity.ecseizounika.ecscope
            new TranslationSeedItem("entity.ecseizounika.ecscope", "zh-HK", "实施范围_hk", "实施范围（冗余：来自 TaktEcDetail.EcScope）"),

            // entity.ecseizounika.ecdeptscope
            new TranslationSeedItem("entity.ecseizounika.ecdeptscope", "en-US", "部门实施范围_us", "部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）"),
            // entity.ecseizounika.ecdeptscope
            new TranslationSeedItem("entity.ecseizounika.ecdeptscope", "ja-JP", "部门实施范围_jp", "部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）"),
            // entity.ecseizounika.ecdeptscope
            new TranslationSeedItem("entity.ecseizounika.ecdeptscope", "zh-CN", "部门实施范围", "部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）"),
            // entity.ecseizounika.ecdeptscope
            new TranslationSeedItem("entity.ecseizounika.ecdeptscope", "zh-HK", "部门实施范围_hk", "部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）"),

            // entity.ecseizounika.ecdetailid
            new TranslationSeedItem("entity.ecseizounika.ecdetailid", "en-US", "设变明细ID_us", "设变明细 ID（TaktEcDetail 主键；去重组内代表/种子明细 Id；同组多明细按业务键 FanOut）"),
            // entity.ecseizounika.ecdetailid
            new TranslationSeedItem("entity.ecseizounika.ecdetailid", "ja-JP", "设变明细ID_jp", "设变明细 ID（TaktEcDetail 主键；去重组内代表/种子明细 Id；同组多明细按业务键 FanOut）"),
            // entity.ecseizounika.ecdetailid
            new TranslationSeedItem("entity.ecseizounika.ecdetailid", "zh-CN", "设变明细ID", "设变明细 ID（TaktEcDetail 主键；去重组内代表/种子明细 Id；同组多明细按业务键 FanOut）"),
            // entity.ecseizounika.ecdetailid
            new TranslationSeedItem("entity.ecseizounika.ecdetailid", "zh-HK", "设变明细ID_hk", "设变明细 ID（TaktEcDetail 主键；去重组内代表/种子明细 Id；同组多明细按业务键 FanOut）"),

            // entity.ecseizounika.isobsolete
            new TranslationSeedItem("entity.ecseizounika.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecseizounika.isobsolete
            new TranslationSeedItem("entity.ecseizounika.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecseizounika.isobsolete
            new TranslationSeedItem("entity.ecseizounika.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecseizounika.isobsolete
            new TranslationSeedItem("entity.ecseizounika.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.ecseizounika.ecdetail
            new TranslationSeedItem("entity.ecseizounika.ecdetail", "en-US", "设变明细_us", "设变明细（数据主从：本表由明细派生；多对一，外键 EcDetailId → TaktEcDetail.Id）"),
            // entity.ecseizounika.ecdetail
            new TranslationSeedItem("entity.ecseizounika.ecdetail", "ja-JP", "设变明细_jp", "设变明细（数据主从：本表由明细派生；多对一，外键 EcDetailId → TaktEcDetail.Id）"),
            // entity.ecseizounika.ecdetail
            new TranslationSeedItem("entity.ecseizounika.ecdetail", "zh-CN", "设变明细", "设变明细（数据主从：本表由明细派生；多对一，外键 EcDetailId → TaktEcDetail.Id）"),
            // entity.ecseizounika.ecdetail
            new TranslationSeedItem("entity.ecseizounika.ecdetail", "zh-HK", "设变明细_hk", "设变明细（数据主从：本表由明细派生；多对一，外键 EcDetailId → TaktEcDetail.Id）"),
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
