// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcSeizouikkaI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEcSeizouikka 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktEcSeizouikka 实体国际化翻译种子（键前缀 entity.ecseizouikka.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEcSeizouikkaI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEcSeizouikka 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 ecseizouikka 实体翻译...", tenantCode);

        foreach (var item in GetEcSeizouikkaTranslations())
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

        TaktLogger.Information("TaktEcSeizouikka 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEcSeizouikka 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.ecseizouikka._self / entity.ecseizouikka.{{field}}；ResourceGroup=EngineeringChange；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEcSeizouikkaTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.ecseizouikka._self
            new TranslationSeedItem("entity.ecseizouikka._self", "en-US", "Ec Seizouikka Information_us", "实体名称"),
            // entity.ecseizouikka._self
            new TranslationSeedItem("entity.ecseizouikka._self", "ja-JP", "设变制造1课信息_jp", "实体名称"),
            // entity.ecseizouikka._self
            new TranslationSeedItem("entity.ecseizouikka._self", "zh-CN", "设变制造1课信息", "实体名称"),
            // entity.ecseizouikka._self
            new TranslationSeedItem("entity.ecseizouikka._self", "zh-HK", "设变制造1课信息_hk", "实体名称"),

            // entity.ecseizouikka.linenumber
            new TranslationSeedItem("entity.ecseizouikka.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.ecseizouikka.linenumber
            new TranslationSeedItem("entity.ecseizouikka.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.ecseizouikka.linenumber
            new TranslationSeedItem("entity.ecseizouikka.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.ecseizouikka.linenumber
            new TranslationSeedItem("entity.ecseizouikka.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.ecseizouikka.eccode
            new TranslationSeedItem("entity.ecseizouikka.eccode", "en-US", "设变单号_us", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),
            // entity.ecseizouikka.eccode
            new TranslationSeedItem("entity.ecseizouikka.eccode", "ja-JP", "设变单号_jp", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),
            // entity.ecseizouikka.eccode
            new TranslationSeedItem("entity.ecseizouikka.eccode", "zh-CN", "设变单号", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),
            // entity.ecseizouikka.eccode
            new TranslationSeedItem("entity.ecseizouikka.eccode", "zh-HK", "设变单号_hk", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),

            // entity.ecseizouikka.discontinuedstatus
            new TranslationSeedItem("entity.ecseizouikka.discontinuedstatus", "en-US", "停产状态_us", "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）"),
            // entity.ecseizouikka.discontinuedstatus
            new TranslationSeedItem("entity.ecseizouikka.discontinuedstatus", "ja-JP", "停产状态_jp", "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）"),
            // entity.ecseizouikka.discontinuedstatus
            new TranslationSeedItem("entity.ecseizouikka.discontinuedstatus", "zh-CN", "停产状态", "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）"),
            // entity.ecseizouikka.discontinuedstatus
            new TranslationSeedItem("entity.ecseizouikka.discontinuedstatus", "zh-HK", "停产状态_hk", "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）"),

            // entity.ecseizouikka.ecmodelcodegroup
            new TranslationSeedItem("entity.ecseizouikka.ecmodelcodegroup", "en-US", "机种编码组_us", "机种编码组（单个或多个机种，分号分隔；ntext）"),
            // entity.ecseizouikka.ecmodelcodegroup
            new TranslationSeedItem("entity.ecseizouikka.ecmodelcodegroup", "ja-JP", "机种编码组_jp", "机种编码组（单个或多个机种，分号分隔；ntext）"),
            // entity.ecseizouikka.ecmodelcodegroup
            new TranslationSeedItem("entity.ecseizouikka.ecmodelcodegroup", "zh-CN", "机种编码组", "机种编码组（单个或多个机种，分号分隔；ntext）"),
            // entity.ecseizouikka.ecmodelcodegroup
            new TranslationSeedItem("entity.ecseizouikka.ecmodelcodegroup", "zh-HK", "机种编码组_hk", "机种编码组（单个或多个机种，分号分隔；ntext）"),

            // entity.ecseizouikka.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.ecseizouikka.ecrootmaterialcodegroup", "en-US", "根物料编码组_us", "根物料编码组（同去重键下的根物料编码，分号分隔；ntext，按机种时同机种根物料可能很多）"),
            // entity.ecseizouikka.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.ecseizouikka.ecrootmaterialcodegroup", "ja-JP", "根物料编码组_jp", "根物料编码组（同去重键下的根物料编码，分号分隔；ntext，按机种时同机种根物料可能很多）"),
            // entity.ecseizouikka.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.ecseizouikka.ecrootmaterialcodegroup", "zh-CN", "根物料编码组", "根物料编码组（同去重键下的根物料编码，分号分隔；ntext，按机种时同机种根物料可能很多）"),
            // entity.ecseizouikka.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.ecseizouikka.ecrootmaterialcodegroup", "zh-HK", "根物料编码组_hk", "根物料编码组（同去重键下的根物料编码，分号分隔；ntext，按机种时同机种根物料可能很多）"),

            // entity.ecseizouikka.isimplemented
            new TranslationSeedItem("entity.ecseizouikka.isimplemented", "en-US", "实施_us", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.ecseizouikka.isimplemented
            new TranslationSeedItem("entity.ecseizouikka.isimplemented", "ja-JP", "实施_jp", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.ecseizouikka.isimplemented
            new TranslationSeedItem("entity.ecseizouikka.isimplemented", "zh-CN", "实施", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.ecseizouikka.isimplemented
            new TranslationSeedItem("entity.ecseizouikka.isimplemented", "zh-HK", "实施_hk", "是否实施（0=否 1=是，字典 sys_yes_no）"),

            // entity.ecseizouikka.scheduleddate
            new TranslationSeedItem("entity.ecseizouikka.scheduleddate", "en-US", "预定日期_us", "预定日期（冗余：来自 TaktEcSeikan.ScheduledDate）"),
            // entity.ecseizouikka.scheduleddate
            new TranslationSeedItem("entity.ecseizouikka.scheduleddate", "ja-JP", "预定日期_jp", "预定日期（冗余：来自 TaktEcSeikan.ScheduledDate）"),
            // entity.ecseizouikka.scheduleddate
            new TranslationSeedItem("entity.ecseizouikka.scheduleddate", "zh-CN", "预定日期", "预定日期（冗余：来自 TaktEcSeikan.ScheduledDate）"),
            // entity.ecseizouikka.scheduleddate
            new TranslationSeedItem("entity.ecseizouikka.scheduleddate", "zh-HK", "预定日期_hk", "预定日期（冗余：来自 TaktEcSeikan.ScheduledDate）"),

            // entity.ecseizouikka.scheduledbatch
            new TranslationSeedItem("entity.ecseizouikka.scheduledbatch", "en-US", "预定批次_us", "预定批次（冗余：来自 TaktEcSeikan.ScheduledBatch）"),
            // entity.ecseizouikka.scheduledbatch
            new TranslationSeedItem("entity.ecseizouikka.scheduledbatch", "ja-JP", "预定批次_jp", "预定批次（冗余：来自 TaktEcSeikan.ScheduledBatch）"),
            // entity.ecseizouikka.scheduledbatch
            new TranslationSeedItem("entity.ecseizouikka.scheduledbatch", "zh-CN", "预定批次", "预定批次（冗余：来自 TaktEcSeikan.ScheduledBatch）"),
            // entity.ecseizouikka.scheduledbatch
            new TranslationSeedItem("entity.ecseizouikka.scheduledbatch", "zh-HK", "预定批次_hk", "预定批次（冗余：来自 TaktEcSeikan.ScheduledBatch）"),

            // entity.ecseizouikka.execcontent
            new TranslationSeedItem("entity.ecseizouikka.execcontent", "en-US", "执行内容_us", "执行内容（各部门通用）"),
            // entity.ecseizouikka.execcontent
            new TranslationSeedItem("entity.ecseizouikka.execcontent", "ja-JP", "执行内容_jp", "执行内容（各部门通用）"),
            // entity.ecseizouikka.execcontent
            new TranslationSeedItem("entity.ecseizouikka.execcontent", "zh-CN", "执行内容", "执行内容（各部门通用）"),
            // entity.ecseizouikka.execcontent
            new TranslationSeedItem("entity.ecseizouikka.execcontent", "zh-HK", "执行内容_hk", "执行内容（各部门通用）"),

            // entity.ecseizouikka.productionteam
            new TranslationSeedItem("entity.ecseizouikka.productionteam", "en-US", "生产班组_us", "生产班组"),
            // entity.ecseizouikka.productionteam
            new TranslationSeedItem("entity.ecseizouikka.productionteam", "ja-JP", "生产班组_jp", "生产班组"),
            // entity.ecseizouikka.productionteam
            new TranslationSeedItem("entity.ecseizouikka.productionteam", "zh-CN", "生产班组", "生产班组"),
            // entity.ecseizouikka.productionteam
            new TranslationSeedItem("entity.ecseizouikka.productionteam", "zh-HK", "生产班组_hk", "生产班组"),

            // entity.ecseizouikka.productiondate
            new TranslationSeedItem("entity.ecseizouikka.productiondate", "en-US", "生产日期_us", "生产日期"),
            // entity.ecseizouikka.productiondate
            new TranslationSeedItem("entity.ecseizouikka.productiondate", "ja-JP", "生产日期_jp", "生产日期"),
            // entity.ecseizouikka.productiondate
            new TranslationSeedItem("entity.ecseizouikka.productiondate", "zh-CN", "生产日期", "生产日期"),
            // entity.ecseizouikka.productiondate
            new TranslationSeedItem("entity.ecseizouikka.productiondate", "zh-HK", "生产日期_hk", "生产日期"),

            // entity.ecseizouikka.implementationbatch
            new TranslationSeedItem("entity.ecseizouikka.implementationbatch", "en-US", "实施批次_us", "实施批次"),
            // entity.ecseizouikka.implementationbatch
            new TranslationSeedItem("entity.ecseizouikka.implementationbatch", "ja-JP", "实施批次_jp", "实施批次"),
            // entity.ecseizouikka.implementationbatch
            new TranslationSeedItem("entity.ecseizouikka.implementationbatch", "zh-CN", "实施批次", "实施批次"),
            // entity.ecseizouikka.implementationbatch
            new TranslationSeedItem("entity.ecseizouikka.implementationbatch", "zh-HK", "实施批次_hk", "实施批次"),

            // entity.ecseizouikka.ecrootmaterialdescription
            new TranslationSeedItem("entity.ecseizouikka.ecrootmaterialdescription", "en-US", "根物料描述_us", "根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）"),
            // entity.ecseizouikka.ecrootmaterialdescription
            new TranslationSeedItem("entity.ecseizouikka.ecrootmaterialdescription", "ja-JP", "根物料描述_jp", "根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）"),
            // entity.ecseizouikka.ecrootmaterialdescription
            new TranslationSeedItem("entity.ecseizouikka.ecrootmaterialdescription", "zh-CN", "根物料描述", "根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）"),
            // entity.ecseizouikka.ecrootmaterialdescription
            new TranslationSeedItem("entity.ecseizouikka.ecrootmaterialdescription", "zh-HK", "根物料描述_hk", "根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）"),

            // entity.ecseizouikka.deptcode
            new TranslationSeedItem("entity.ecseizouikka.deptcode", "en-US", "部门编码_us", "部门编码（TaktDept.DeptCode；本表固定课别）"),
            // entity.ecseizouikka.deptcode
            new TranslationSeedItem("entity.ecseizouikka.deptcode", "ja-JP", "部门编码_jp", "部门编码（TaktDept.DeptCode；本表固定课别）"),
            // entity.ecseizouikka.deptcode
            new TranslationSeedItem("entity.ecseizouikka.deptcode", "zh-CN", "部门编码", "部门编码（TaktDept.DeptCode；本表固定课别）"),
            // entity.ecseizouikka.deptcode
            new TranslationSeedItem("entity.ecseizouikka.deptcode", "zh-HK", "部门编码_hk", "部门编码（TaktDept.DeptCode；本表固定课别）"),

            // entity.ecseizouikka.deptname
            new TranslationSeedItem("entity.ecseizouikka.deptname", "en-US", "部门名称_us", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),
            // entity.ecseizouikka.deptname
            new TranslationSeedItem("entity.ecseizouikka.deptname", "ja-JP", "部门名称_jp", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),
            // entity.ecseizouikka.deptname
            new TranslationSeedItem("entity.ecseizouikka.deptname", "zh-CN", "部门名称", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),
            // entity.ecseizouikka.deptname
            new TranslationSeedItem("entity.ecseizouikka.deptname", "zh-HK", "部门名称_hk", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),

            // entity.ecseizouikka.ecscope
            new TranslationSeedItem("entity.ecseizouikka.ecscope", "en-US", "实施范围_us", "实施范围（冗余：来自 TaktEcDetail.EcScope）"),
            // entity.ecseizouikka.ecscope
            new TranslationSeedItem("entity.ecseizouikka.ecscope", "ja-JP", "实施范围_jp", "实施范围（冗余：来自 TaktEcDetail.EcScope）"),
            // entity.ecseizouikka.ecscope
            new TranslationSeedItem("entity.ecseizouikka.ecscope", "zh-CN", "实施范围", "实施范围（冗余：来自 TaktEcDetail.EcScope）"),
            // entity.ecseizouikka.ecscope
            new TranslationSeedItem("entity.ecseizouikka.ecscope", "zh-HK", "实施范围_hk", "实施范围（冗余：来自 TaktEcDetail.EcScope）"),

            // entity.ecseizouikka.ecdeptscope
            new TranslationSeedItem("entity.ecseizouikka.ecdeptscope", "en-US", "部门实施范围_us", "部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）"),
            // entity.ecseizouikka.ecdeptscope
            new TranslationSeedItem("entity.ecseizouikka.ecdeptscope", "ja-JP", "部门实施范围_jp", "部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）"),
            // entity.ecseizouikka.ecdeptscope
            new TranslationSeedItem("entity.ecseizouikka.ecdeptscope", "zh-CN", "部门实施范围", "部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）"),
            // entity.ecseizouikka.ecdeptscope
            new TranslationSeedItem("entity.ecseizouikka.ecdeptscope", "zh-HK", "部门实施范围_hk", "部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）"),

            // entity.ecseizouikka.ecdetailid
            new TranslationSeedItem("entity.ecseizouikka.ecdetailid", "en-US", "设变明细ID_us", "设变明细 ID（TaktEcDetail 主键；去重组内代表/种子明细 Id；同组多明细按业务键 FanOut）"),
            // entity.ecseizouikka.ecdetailid
            new TranslationSeedItem("entity.ecseizouikka.ecdetailid", "ja-JP", "设变明细ID_jp", "设变明细 ID（TaktEcDetail 主键；去重组内代表/种子明细 Id；同组多明细按业务键 FanOut）"),
            // entity.ecseizouikka.ecdetailid
            new TranslationSeedItem("entity.ecseizouikka.ecdetailid", "zh-CN", "设变明细ID", "设变明细 ID（TaktEcDetail 主键；去重组内代表/种子明细 Id；同组多明细按业务键 FanOut）"),
            // entity.ecseizouikka.ecdetailid
            new TranslationSeedItem("entity.ecseizouikka.ecdetailid", "zh-HK", "设变明细ID_hk", "设变明细 ID（TaktEcDetail 主键；去重组内代表/种子明细 Id；同组多明细按业务键 FanOut）"),

            // entity.ecseizouikka.isobsolete
            new TranslationSeedItem("entity.ecseizouikka.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecseizouikka.isobsolete
            new TranslationSeedItem("entity.ecseizouikka.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecseizouikka.isobsolete
            new TranslationSeedItem("entity.ecseizouikka.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecseizouikka.isobsolete
            new TranslationSeedItem("entity.ecseizouikka.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.ecseizouikka.ecdetail
            new TranslationSeedItem("entity.ecseizouikka.ecdetail", "en-US", "设变明细_us", "设变明细（数据主从：本表由明细派生；多对一，外键 EcDetailId → TaktEcDetail.Id）"),
            // entity.ecseizouikka.ecdetail
            new TranslationSeedItem("entity.ecseizouikka.ecdetail", "ja-JP", "设变明细_jp", "设变明细（数据主从：本表由明细派生；多对一，外键 EcDetailId → TaktEcDetail.Id）"),
            // entity.ecseizouikka.ecdetail
            new TranslationSeedItem("entity.ecseizouikka.ecdetail", "zh-CN", "设变明细", "设变明细（数据主从：本表由明细派生；多对一，外键 EcDetailId → TaktEcDetail.Id）"),
            // entity.ecseizouikka.ecdetail
            new TranslationSeedItem("entity.ecseizouikka.ecdetail", "zh-HK", "设变明细_hk", "设变明细（数据主从：本表由明细派生；多对一，外键 EcDetailId → TaktEcDetail.Id）"),
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
