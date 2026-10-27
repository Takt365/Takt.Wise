// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcMfg1I18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEcMfg1 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktEcMfg1 实体国际化翻译种子（键前缀 entity.ecmfg1.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEcMfg1I18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEcMfg1 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 ecmfg1 实体翻译...", tenantCode);

        foreach (var item in GetEcMfg1Translations())
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

        TaktLogger.Information("TaktEcMfg1 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEcMfg1 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.ecmfg1._self / entity.ecmfg1.{{field}}；ResourceGroup=EngineeringChange；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEcMfg1Translations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.ecmfg1._self
            new TranslationSeedItem("entity.ecmfg1._self", "en-US", "Ec Mfg1 Information_us", "实体名称"),
            // entity.ecmfg1._self
            new TranslationSeedItem("entity.ecmfg1._self", "ja-JP", "设变制造1课信息_jp", "实体名称"),
            // entity.ecmfg1._self
            new TranslationSeedItem("entity.ecmfg1._self", "zh-CN", "设变制造1课信息", "实体名称"),
            // entity.ecmfg1._self
            new TranslationSeedItem("entity.ecmfg1._self", "zh-HK", "设变制造1课信息_hk", "实体名称"),

            // entity.ecmfg1.linenumber
            new TranslationSeedItem("entity.ecmfg1.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.ecmfg1.linenumber
            new TranslationSeedItem("entity.ecmfg1.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.ecmfg1.linenumber
            new TranslationSeedItem("entity.ecmfg1.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.ecmfg1.linenumber
            new TranslationSeedItem("entity.ecmfg1.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.ecmfg1.eccode
            new TranslationSeedItem("entity.ecmfg1.eccode", "en-US", "设变单号_us", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),
            // entity.ecmfg1.eccode
            new TranslationSeedItem("entity.ecmfg1.eccode", "ja-JP", "设变单号_jp", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),
            // entity.ecmfg1.eccode
            new TranslationSeedItem("entity.ecmfg1.eccode", "zh-CN", "设变单号", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),
            // entity.ecmfg1.eccode
            new TranslationSeedItem("entity.ecmfg1.eccode", "zh-HK", "设变单号_hk", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),

            // entity.ecmfg1.discontinuedstatus
            new TranslationSeedItem("entity.ecmfg1.discontinuedstatus", "en-US", "停产状态_us", "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）"),
            // entity.ecmfg1.discontinuedstatus
            new TranslationSeedItem("entity.ecmfg1.discontinuedstatus", "ja-JP", "停产状态_jp", "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）"),
            // entity.ecmfg1.discontinuedstatus
            new TranslationSeedItem("entity.ecmfg1.discontinuedstatus", "zh-CN", "停产状态", "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）"),
            // entity.ecmfg1.discontinuedstatus
            new TranslationSeedItem("entity.ecmfg1.discontinuedstatus", "zh-HK", "停产状态_hk", "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）"),

            // entity.ecmfg1.ecmodelcodegroup
            new TranslationSeedItem("entity.ecmfg1.ecmodelcodegroup", "en-US", "机种编码组_us", "机种编码组（单个或多个机种，分号分隔；ntext）"),
            // entity.ecmfg1.ecmodelcodegroup
            new TranslationSeedItem("entity.ecmfg1.ecmodelcodegroup", "ja-JP", "机种编码组_jp", "机种编码组（单个或多个机种，分号分隔；ntext）"),
            // entity.ecmfg1.ecmodelcodegroup
            new TranslationSeedItem("entity.ecmfg1.ecmodelcodegroup", "zh-CN", "机种编码组", "机种编码组（单个或多个机种，分号分隔；ntext）"),
            // entity.ecmfg1.ecmodelcodegroup
            new TranslationSeedItem("entity.ecmfg1.ecmodelcodegroup", "zh-HK", "机种编码组_hk", "机种编码组（单个或多个机种，分号分隔；ntext）"),

            // entity.ecmfg1.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.ecmfg1.ecrootmaterialcodegroup", "en-US", "根物料编码组_us", "根物料编码组（同去重键下的根物料编码，分号分隔；ntext，按机种时同机种根物料可能很多）"),
            // entity.ecmfg1.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.ecmfg1.ecrootmaterialcodegroup", "ja-JP", "根物料编码组_jp", "根物料编码组（同去重键下的根物料编码，分号分隔；ntext，按机种时同机种根物料可能很多）"),
            // entity.ecmfg1.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.ecmfg1.ecrootmaterialcodegroup", "zh-CN", "根物料编码组", "根物料编码组（同去重键下的根物料编码，分号分隔；ntext，按机种时同机种根物料可能很多）"),
            // entity.ecmfg1.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.ecmfg1.ecrootmaterialcodegroup", "zh-HK", "根物料编码组_hk", "根物料编码组（同去重键下的根物料编码，分号分隔；ntext，按机种时同机种根物料可能很多）"),

            // entity.ecmfg1.isimplemented
            new TranslationSeedItem("entity.ecmfg1.isimplemented", "en-US", "实施_us", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.ecmfg1.isimplemented
            new TranslationSeedItem("entity.ecmfg1.isimplemented", "ja-JP", "实施_jp", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.ecmfg1.isimplemented
            new TranslationSeedItem("entity.ecmfg1.isimplemented", "zh-CN", "实施", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.ecmfg1.isimplemented
            new TranslationSeedItem("entity.ecmfg1.isimplemented", "zh-HK", "实施_hk", "是否实施（0=否 1=是，字典 sys_yes_no）"),

            // entity.ecmfg1.scheduleddate
            new TranslationSeedItem("entity.ecmfg1.scheduleddate", "en-US", "预定日期_us", "预定日期（冗余：来自 TaktEcPc.ScheduledDate）"),
            // entity.ecmfg1.scheduleddate
            new TranslationSeedItem("entity.ecmfg1.scheduleddate", "ja-JP", "预定日期_jp", "预定日期（冗余：来自 TaktEcPc.ScheduledDate）"),
            // entity.ecmfg1.scheduleddate
            new TranslationSeedItem("entity.ecmfg1.scheduleddate", "zh-CN", "预定日期", "预定日期（冗余：来自 TaktEcPc.ScheduledDate）"),
            // entity.ecmfg1.scheduleddate
            new TranslationSeedItem("entity.ecmfg1.scheduleddate", "zh-HK", "预定日期_hk", "预定日期（冗余：来自 TaktEcPc.ScheduledDate）"),

            // entity.ecmfg1.scheduledbatch
            new TranslationSeedItem("entity.ecmfg1.scheduledbatch", "en-US", "预定批次_us", "预定批次（冗余：来自 TaktEcPc.ScheduledBatch）"),
            // entity.ecmfg1.scheduledbatch
            new TranslationSeedItem("entity.ecmfg1.scheduledbatch", "ja-JP", "预定批次_jp", "预定批次（冗余：来自 TaktEcPc.ScheduledBatch）"),
            // entity.ecmfg1.scheduledbatch
            new TranslationSeedItem("entity.ecmfg1.scheduledbatch", "zh-CN", "预定批次", "预定批次（冗余：来自 TaktEcPc.ScheduledBatch）"),
            // entity.ecmfg1.scheduledbatch
            new TranslationSeedItem("entity.ecmfg1.scheduledbatch", "zh-HK", "预定批次_hk", "预定批次（冗余：来自 TaktEcPc.ScheduledBatch）"),

            // entity.ecmfg1.execcontent
            new TranslationSeedItem("entity.ecmfg1.execcontent", "en-US", "执行内容_us", "执行内容（各部门通用）"),
            // entity.ecmfg1.execcontent
            new TranslationSeedItem("entity.ecmfg1.execcontent", "ja-JP", "执行内容_jp", "执行内容（各部门通用）"),
            // entity.ecmfg1.execcontent
            new TranslationSeedItem("entity.ecmfg1.execcontent", "zh-CN", "执行内容", "执行内容（各部门通用）"),
            // entity.ecmfg1.execcontent
            new TranslationSeedItem("entity.ecmfg1.execcontent", "zh-HK", "执行内容_hk", "执行内容（各部门通用）"),

            // entity.ecmfg1.productionteam
            new TranslationSeedItem("entity.ecmfg1.productionteam", "en-US", "生产班组_us", "生产班组"),
            // entity.ecmfg1.productionteam
            new TranslationSeedItem("entity.ecmfg1.productionteam", "ja-JP", "生产班组_jp", "生产班组"),
            // entity.ecmfg1.productionteam
            new TranslationSeedItem("entity.ecmfg1.productionteam", "zh-CN", "生产班组", "生产班组"),
            // entity.ecmfg1.productionteam
            new TranslationSeedItem("entity.ecmfg1.productionteam", "zh-HK", "生产班组_hk", "生产班组"),

            // entity.ecmfg1.productiondate
            new TranslationSeedItem("entity.ecmfg1.productiondate", "en-US", "生产日期_us", "生产日期"),
            // entity.ecmfg1.productiondate
            new TranslationSeedItem("entity.ecmfg1.productiondate", "ja-JP", "生产日期_jp", "生产日期"),
            // entity.ecmfg1.productiondate
            new TranslationSeedItem("entity.ecmfg1.productiondate", "zh-CN", "生产日期", "生产日期"),
            // entity.ecmfg1.productiondate
            new TranslationSeedItem("entity.ecmfg1.productiondate", "zh-HK", "生产日期_hk", "生产日期"),

            // entity.ecmfg1.implementationbatch
            new TranslationSeedItem("entity.ecmfg1.implementationbatch", "en-US", "实施批次_us", "实施批次"),
            // entity.ecmfg1.implementationbatch
            new TranslationSeedItem("entity.ecmfg1.implementationbatch", "ja-JP", "实施批次_jp", "实施批次"),
            // entity.ecmfg1.implementationbatch
            new TranslationSeedItem("entity.ecmfg1.implementationbatch", "zh-CN", "实施批次", "实施批次"),
            // entity.ecmfg1.implementationbatch
            new TranslationSeedItem("entity.ecmfg1.implementationbatch", "zh-HK", "实施批次_hk", "实施批次"),

            // entity.ecmfg1.ecrootmaterialdescription
            new TranslationSeedItem("entity.ecmfg1.ecrootmaterialdescription", "en-US", "根物料描述_us", "根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）"),
            // entity.ecmfg1.ecrootmaterialdescription
            new TranslationSeedItem("entity.ecmfg1.ecrootmaterialdescription", "ja-JP", "根物料描述_jp", "根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）"),
            // entity.ecmfg1.ecrootmaterialdescription
            new TranslationSeedItem("entity.ecmfg1.ecrootmaterialdescription", "zh-CN", "根物料描述", "根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）"),
            // entity.ecmfg1.ecrootmaterialdescription
            new TranslationSeedItem("entity.ecmfg1.ecrootmaterialdescription", "zh-HK", "根物料描述_hk", "根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）"),

            // entity.ecmfg1.deptcode
            new TranslationSeedItem("entity.ecmfg1.deptcode", "en-US", "部门编码_us", "部门编码（TaktDept.DeptCode；本表固定课别）"),
            // entity.ecmfg1.deptcode
            new TranslationSeedItem("entity.ecmfg1.deptcode", "ja-JP", "部门编码_jp", "部门编码（TaktDept.DeptCode；本表固定课别）"),
            // entity.ecmfg1.deptcode
            new TranslationSeedItem("entity.ecmfg1.deptcode", "zh-CN", "部门编码", "部门编码（TaktDept.DeptCode；本表固定课别）"),
            // entity.ecmfg1.deptcode
            new TranslationSeedItem("entity.ecmfg1.deptcode", "zh-HK", "部门编码_hk", "部门编码（TaktDept.DeptCode；本表固定课别）"),

            // entity.ecmfg1.deptname
            new TranslationSeedItem("entity.ecmfg1.deptname", "en-US", "部门名称_us", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),
            // entity.ecmfg1.deptname
            new TranslationSeedItem("entity.ecmfg1.deptname", "ja-JP", "部门名称_jp", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),
            // entity.ecmfg1.deptname
            new TranslationSeedItem("entity.ecmfg1.deptname", "zh-CN", "部门名称", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),
            // entity.ecmfg1.deptname
            new TranslationSeedItem("entity.ecmfg1.deptname", "zh-HK", "部门名称_hk", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),

            // entity.ecmfg1.ecimplscope
            new TranslationSeedItem("entity.ecmfg1.ecimplscope", "en-US", "实施范围_us", "实施范围（冗余：来自 TaktEcDetail.EcImplScope）"),
            // entity.ecmfg1.ecimplscope
            new TranslationSeedItem("entity.ecmfg1.ecimplscope", "ja-JP", "实施范围_jp", "实施范围（冗余：来自 TaktEcDetail.EcImplScope）"),
            // entity.ecmfg1.ecimplscope
            new TranslationSeedItem("entity.ecmfg1.ecimplscope", "zh-CN", "实施范围", "实施范围（冗余：来自 TaktEcDetail.EcImplScope）"),
            // entity.ecmfg1.ecimplscope
            new TranslationSeedItem("entity.ecmfg1.ecimplscope", "zh-HK", "实施范围_hk", "实施范围（冗余：来自 TaktEcDetail.EcImplScope）"),

            // entity.ecmfg1.eccontrolscope
            new TranslationSeedItem("entity.ecmfg1.eccontrolscope", "en-US", "控制范围_us", "控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品，2=按机种，3=按设变，4=不管理），只记录生管/制二/制一/品管；全仕向时生管与此相同。采购/受检/部管/SMT 固定按设变，制技固定按机种"),
            // entity.ecmfg1.eccontrolscope
            new TranslationSeedItem("entity.ecmfg1.eccontrolscope", "ja-JP", "控制范围_jp", "控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品，2=按机种，3=按设变，4=不管理），只记录生管/制二/制一/品管；全仕向时生管与此相同。采购/受检/部管/SMT 固定按设变，制技固定按机种"),
            // entity.ecmfg1.eccontrolscope
            new TranslationSeedItem("entity.ecmfg1.eccontrolscope", "zh-CN", "控制范围", "控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品，2=按机种，3=按设变，4=不管理），只记录生管/制二/制一/品管；全仕向时生管与此相同。采购/受检/部管/SMT 固定按设变，制技固定按机种"),
            // entity.ecmfg1.eccontrolscope
            new TranslationSeedItem("entity.ecmfg1.eccontrolscope", "zh-HK", "控制范围_hk", "控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品，2=按机种，3=按设变，4=不管理），只记录生管/制二/制一/品管；全仕向时生管与此相同。采购/受检/部管/SMT 固定按设变，制技固定按机种"),

            // entity.ecmfg1.ecgrainkey
            new TranslationSeedItem("entity.ecmfg1.ecgrainkey", "en-US", "执行去重键_us", "执行行去重键（设变单号、机种或根物料或新物料、分道；同一键的明细共用本行。ntext）"),
            // entity.ecmfg1.ecgrainkey
            new TranslationSeedItem("entity.ecmfg1.ecgrainkey", "ja-JP", "执行去重键_jp", "执行行去重键（设变单号、机种或根物料或新物料、分道；同一键的明细共用本行。ntext）"),
            // entity.ecmfg1.ecgrainkey
            new TranslationSeedItem("entity.ecmfg1.ecgrainkey", "zh-CN", "执行去重键", "执行行去重键（设变单号、机种或根物料或新物料、分道；同一键的明细共用本行。ntext）"),
            // entity.ecmfg1.ecgrainkey
            new TranslationSeedItem("entity.ecmfg1.ecgrainkey", "zh-HK", "执行去重键_hk", "执行行去重键（设变单号、机种或根物料或新物料、分道；同一键的明细共用本行。ntext）"),

            // entity.ecmfg1.isobsolete
            new TranslationSeedItem("entity.ecmfg1.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecmfg1.isobsolete
            new TranslationSeedItem("entity.ecmfg1.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecmfg1.isobsolete
            new TranslationSeedItem("entity.ecmfg1.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecmfg1.isobsolete
            new TranslationSeedItem("entity.ecmfg1.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
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
