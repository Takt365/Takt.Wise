// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcHinkanI18nSeedData.cs
// 创建时间：2026-10-14
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

            // entity.echinkan.linenumber
            new TranslationSeedItem("entity.echinkan.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.echinkan.linenumber
            new TranslationSeedItem("entity.echinkan.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.echinkan.linenumber
            new TranslationSeedItem("entity.echinkan.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.echinkan.linenumber
            new TranslationSeedItem("entity.echinkan.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.echinkan.eccode
            new TranslationSeedItem("entity.echinkan.eccode", "en-US", "设变单号_us", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),
            // entity.echinkan.eccode
            new TranslationSeedItem("entity.echinkan.eccode", "ja-JP", "设变单号_jp", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),
            // entity.echinkan.eccode
            new TranslationSeedItem("entity.echinkan.eccode", "zh-CN", "设变单号", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),
            // entity.echinkan.eccode
            new TranslationSeedItem("entity.echinkan.eccode", "zh-HK", "设变单号_hk", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),

            // entity.echinkan.discontinuedstatus
            new TranslationSeedItem("entity.echinkan.discontinuedstatus", "en-US", "停产状态_us", "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）"),
            // entity.echinkan.discontinuedstatus
            new TranslationSeedItem("entity.echinkan.discontinuedstatus", "ja-JP", "停产状态_jp", "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）"),
            // entity.echinkan.discontinuedstatus
            new TranslationSeedItem("entity.echinkan.discontinuedstatus", "zh-CN", "停产状态", "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）"),
            // entity.echinkan.discontinuedstatus
            new TranslationSeedItem("entity.echinkan.discontinuedstatus", "zh-HK", "停产状态_hk", "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）"),

            // entity.echinkan.ecmodelcodegroup
            new TranslationSeedItem("entity.echinkan.ecmodelcodegroup", "en-US", "机种编码组_us", "机种编码组（单个或多个机种，分号分隔；ntext）"),
            // entity.echinkan.ecmodelcodegroup
            new TranslationSeedItem("entity.echinkan.ecmodelcodegroup", "ja-JP", "机种编码组_jp", "机种编码组（单个或多个机种，分号分隔；ntext）"),
            // entity.echinkan.ecmodelcodegroup
            new TranslationSeedItem("entity.echinkan.ecmodelcodegroup", "zh-CN", "机种编码组", "机种编码组（单个或多个机种，分号分隔；ntext）"),
            // entity.echinkan.ecmodelcodegroup
            new TranslationSeedItem("entity.echinkan.ecmodelcodegroup", "zh-HK", "机种编码组_hk", "机种编码组（单个或多个机种，分号分隔；ntext）"),

            // entity.echinkan.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.echinkan.ecrootmaterialcodegroup", "en-US", "根物料编码组_us", "根物料编码组（同去重键下的根物料编码，分号分隔；ntext，按机种时同机种根物料可能很多）"),
            // entity.echinkan.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.echinkan.ecrootmaterialcodegroup", "ja-JP", "根物料编码组_jp", "根物料编码组（同去重键下的根物料编码，分号分隔；ntext，按机种时同机种根物料可能很多）"),
            // entity.echinkan.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.echinkan.ecrootmaterialcodegroup", "zh-CN", "根物料编码组", "根物料编码组（同去重键下的根物料编码，分号分隔；ntext，按机种时同机种根物料可能很多）"),
            // entity.echinkan.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.echinkan.ecrootmaterialcodegroup", "zh-HK", "根物料编码组_hk", "根物料编码组（同去重键下的根物料编码，分号分隔；ntext，按机种时同机种根物料可能很多）"),

            // entity.echinkan.isimplemented
            new TranslationSeedItem("entity.echinkan.isimplemented", "en-US", "实施_us", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.echinkan.isimplemented
            new TranslationSeedItem("entity.echinkan.isimplemented", "ja-JP", "实施_jp", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.echinkan.isimplemented
            new TranslationSeedItem("entity.echinkan.isimplemented", "zh-CN", "实施", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.echinkan.isimplemented
            new TranslationSeedItem("entity.echinkan.isimplemented", "zh-HK", "实施_hk", "是否实施（0=否 1=是，字典 sys_yes_no）"),

            // entity.echinkan.scheduleddate
            new TranslationSeedItem("entity.echinkan.scheduleddate", "en-US", "预定日期_us", "预定日期（冗余：来自 TaktEcSeikan.ScheduledDate）"),
            // entity.echinkan.scheduleddate
            new TranslationSeedItem("entity.echinkan.scheduleddate", "ja-JP", "预定日期_jp", "预定日期（冗余：来自 TaktEcSeikan.ScheduledDate）"),
            // entity.echinkan.scheduleddate
            new TranslationSeedItem("entity.echinkan.scheduleddate", "zh-CN", "预定日期", "预定日期（冗余：来自 TaktEcSeikan.ScheduledDate）"),
            // entity.echinkan.scheduleddate
            new TranslationSeedItem("entity.echinkan.scheduleddate", "zh-HK", "预定日期_hk", "预定日期（冗余：来自 TaktEcSeikan.ScheduledDate）"),

            // entity.echinkan.scheduledbatch
            new TranslationSeedItem("entity.echinkan.scheduledbatch", "en-US", "预定批次_us", "预定批次（冗余：来自 TaktEcSeikan.ScheduledBatch）"),
            // entity.echinkan.scheduledbatch
            new TranslationSeedItem("entity.echinkan.scheduledbatch", "ja-JP", "预定批次_jp", "预定批次（冗余：来自 TaktEcSeikan.ScheduledBatch）"),
            // entity.echinkan.scheduledbatch
            new TranslationSeedItem("entity.echinkan.scheduledbatch", "zh-CN", "预定批次", "预定批次（冗余：来自 TaktEcSeikan.ScheduledBatch）"),
            // entity.echinkan.scheduledbatch
            new TranslationSeedItem("entity.echinkan.scheduledbatch", "zh-HK", "预定批次_hk", "预定批次（冗余：来自 TaktEcSeikan.ScheduledBatch）"),

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

            // entity.echinkan.ecrootmaterialdescription
            new TranslationSeedItem("entity.echinkan.ecrootmaterialdescription", "en-US", "根物料描述_us", "根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）"),
            // entity.echinkan.ecrootmaterialdescription
            new TranslationSeedItem("entity.echinkan.ecrootmaterialdescription", "ja-JP", "根物料描述_jp", "根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）"),
            // entity.echinkan.ecrootmaterialdescription
            new TranslationSeedItem("entity.echinkan.ecrootmaterialdescription", "zh-CN", "根物料描述", "根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）"),
            // entity.echinkan.ecrootmaterialdescription
            new TranslationSeedItem("entity.echinkan.ecrootmaterialdescription", "zh-HK", "根物料描述_hk", "根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）"),

            // entity.echinkan.deptcode
            new TranslationSeedItem("entity.echinkan.deptcode", "en-US", "部门编码_us", "部门编码（TaktDept.DeptCode；本表固定课别）"),
            // entity.echinkan.deptcode
            new TranslationSeedItem("entity.echinkan.deptcode", "ja-JP", "部门编码_jp", "部门编码（TaktDept.DeptCode；本表固定课别）"),
            // entity.echinkan.deptcode
            new TranslationSeedItem("entity.echinkan.deptcode", "zh-CN", "部门编码", "部门编码（TaktDept.DeptCode；本表固定课别）"),
            // entity.echinkan.deptcode
            new TranslationSeedItem("entity.echinkan.deptcode", "zh-HK", "部门编码_hk", "部门编码（TaktDept.DeptCode；本表固定课别）"),

            // entity.echinkan.deptname
            new TranslationSeedItem("entity.echinkan.deptname", "en-US", "部门名称_us", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),
            // entity.echinkan.deptname
            new TranslationSeedItem("entity.echinkan.deptname", "ja-JP", "部门名称_jp", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),
            // entity.echinkan.deptname
            new TranslationSeedItem("entity.echinkan.deptname", "zh-CN", "部门名称", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),
            // entity.echinkan.deptname
            new TranslationSeedItem("entity.echinkan.deptname", "zh-HK", "部门名称_hk", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),

            // entity.echinkan.ecscope
            new TranslationSeedItem("entity.echinkan.ecscope", "en-US", "实施范围_us", "实施范围（冗余：来自 TaktEcDetail.EcScope）"),
            // entity.echinkan.ecscope
            new TranslationSeedItem("entity.echinkan.ecscope", "ja-JP", "实施范围_jp", "实施范围（冗余：来自 TaktEcDetail.EcScope）"),
            // entity.echinkan.ecscope
            new TranslationSeedItem("entity.echinkan.ecscope", "zh-CN", "实施范围", "实施范围（冗余：来自 TaktEcDetail.EcScope）"),
            // entity.echinkan.ecscope
            new TranslationSeedItem("entity.echinkan.ecscope", "zh-HK", "实施范围_hk", "实施范围（冗余：来自 TaktEcDetail.EcScope）"),

            // entity.echinkan.ecdeptscope
            new TranslationSeedItem("entity.echinkan.ecdeptscope", "en-US", "部门实施范围_us", "部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）"),
            // entity.echinkan.ecdeptscope
            new TranslationSeedItem("entity.echinkan.ecdeptscope", "ja-JP", "部门实施范围_jp", "部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）"),
            // entity.echinkan.ecdeptscope
            new TranslationSeedItem("entity.echinkan.ecdeptscope", "zh-CN", "部门实施范围", "部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）"),
            // entity.echinkan.ecdeptscope
            new TranslationSeedItem("entity.echinkan.ecdeptscope", "zh-HK", "部门实施范围_hk", "部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）"),

            // entity.echinkan.ecdetailid
            new TranslationSeedItem("entity.echinkan.ecdetailid", "en-US", "设变明细ID_us", "设变明细 ID（TaktEcDetail 主键；去重组内代表/种子明细 Id；同组多明细按业务键 FanOut）"),
            // entity.echinkan.ecdetailid
            new TranslationSeedItem("entity.echinkan.ecdetailid", "ja-JP", "设变明细ID_jp", "设变明细 ID（TaktEcDetail 主键；去重组内代表/种子明细 Id；同组多明细按业务键 FanOut）"),
            // entity.echinkan.ecdetailid
            new TranslationSeedItem("entity.echinkan.ecdetailid", "zh-CN", "设变明细ID", "设变明细 ID（TaktEcDetail 主键；去重组内代表/种子明细 Id；同组多明细按业务键 FanOut）"),
            // entity.echinkan.ecdetailid
            new TranslationSeedItem("entity.echinkan.ecdetailid", "zh-HK", "设变明细ID_hk", "设变明细 ID（TaktEcDetail 主键；去重组内代表/种子明细 Id；同组多明细按业务键 FanOut）"),

            // entity.echinkan.isobsolete
            new TranslationSeedItem("entity.echinkan.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.echinkan.isobsolete
            new TranslationSeedItem("entity.echinkan.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.echinkan.isobsolete
            new TranslationSeedItem("entity.echinkan.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.echinkan.isobsolete
            new TranslationSeedItem("entity.echinkan.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.echinkan.ecdetail
            new TranslationSeedItem("entity.echinkan.ecdetail", "en-US", "设变明细_us", "设变明细（数据主从：本表由明细派生；多对一，外键 EcDetailId → TaktEcDetail.Id）"),
            // entity.echinkan.ecdetail
            new TranslationSeedItem("entity.echinkan.ecdetail", "ja-JP", "设变明细_jp", "设变明细（数据主从：本表由明细派生；多对一，外键 EcDetailId → TaktEcDetail.Id）"),
            // entity.echinkan.ecdetail
            new TranslationSeedItem("entity.echinkan.ecdetail", "zh-CN", "设变明细", "设变明细（数据主从：本表由明细派生；多对一，外键 EcDetailId → TaktEcDetail.Id）"),
            // entity.echinkan.ecdetail
            new TranslationSeedItem("entity.echinkan.ecdetail", "zh-HK", "设变明细_hk", "设变明细（数据主从：本表由明细派生；多对一，外键 EcDetailId → TaktEcDetail.Id）"),
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
