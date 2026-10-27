// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcPcI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEcPc 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktEcPc 实体国际化翻译种子（键前缀 entity.ecpc.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEcPcI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEcPc 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 ecpc 实体翻译...", tenantCode);

        foreach (var item in GetEcPcTranslations())
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

        TaktLogger.Information("TaktEcPc 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEcPc 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.ecpc._self / entity.ecpc.{{field}}；ResourceGroup=EngineeringChange；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEcPcTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.ecpc._self
            new TranslationSeedItem("entity.ecpc._self", "en-US", "Ec Pc Information_us", "实体名称"),
            // entity.ecpc._self
            new TranslationSeedItem("entity.ecpc._self", "ja-JP", "设变生管课信息_jp", "实体名称"),
            // entity.ecpc._self
            new TranslationSeedItem("entity.ecpc._self", "zh-CN", "设变生管课信息", "实体名称"),
            // entity.ecpc._self
            new TranslationSeedItem("entity.ecpc._self", "zh-HK", "设变生管课信息_hk", "实体名称"),

            // entity.ecpc.linenumber
            new TranslationSeedItem("entity.ecpc.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.ecpc.linenumber
            new TranslationSeedItem("entity.ecpc.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.ecpc.linenumber
            new TranslationSeedItem("entity.ecpc.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.ecpc.linenumber
            new TranslationSeedItem("entity.ecpc.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.ecpc.eccode
            new TranslationSeedItem("entity.ecpc.eccode", "en-US", "设变单号_us", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),
            // entity.ecpc.eccode
            new TranslationSeedItem("entity.ecpc.eccode", "ja-JP", "设变单号_jp", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),
            // entity.ecpc.eccode
            new TranslationSeedItem("entity.ecpc.eccode", "zh-CN", "设变单号", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),
            // entity.ecpc.eccode
            new TranslationSeedItem("entity.ecpc.eccode", "zh-HK", "设变单号_hk", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),

            // entity.ecpc.discontinuedstatus
            new TranslationSeedItem("entity.ecpc.discontinuedstatus", "en-US", "停产状态_us", "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）"),
            // entity.ecpc.discontinuedstatus
            new TranslationSeedItem("entity.ecpc.discontinuedstatus", "ja-JP", "停产状态_jp", "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）"),
            // entity.ecpc.discontinuedstatus
            new TranslationSeedItem("entity.ecpc.discontinuedstatus", "zh-CN", "停产状态", "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）"),
            // entity.ecpc.discontinuedstatus
            new TranslationSeedItem("entity.ecpc.discontinuedstatus", "zh-HK", "停产状态_hk", "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）"),

            // entity.ecpc.ecmodelcodegroup
            new TranslationSeedItem("entity.ecpc.ecmodelcodegroup", "en-US", "机种编码组_us", "机种编码组（单个或多个机种，分号分隔；ntext）"),
            // entity.ecpc.ecmodelcodegroup
            new TranslationSeedItem("entity.ecpc.ecmodelcodegroup", "ja-JP", "机种编码组_jp", "机种编码组（单个或多个机种，分号分隔；ntext）"),
            // entity.ecpc.ecmodelcodegroup
            new TranslationSeedItem("entity.ecpc.ecmodelcodegroup", "zh-CN", "机种编码组", "机种编码组（单个或多个机种，分号分隔；ntext）"),
            // entity.ecpc.ecmodelcodegroup
            new TranslationSeedItem("entity.ecpc.ecmodelcodegroup", "zh-HK", "机种编码组_hk", "机种编码组（单个或多个机种，分号分隔；ntext）"),

            // entity.ecpc.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.ecpc.ecrootmaterialcodegroup", "en-US", "根物料编码组_us", "根物料编码组（同去重键下的根物料编码，分号分隔；ntext，按机种/部管时同机种根物料可能很多）"),
            // entity.ecpc.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.ecpc.ecrootmaterialcodegroup", "ja-JP", "根物料编码组_jp", "根物料编码组（同去重键下的根物料编码，分号分隔；ntext，按机种/部管时同机种根物料可能很多）"),
            // entity.ecpc.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.ecpc.ecrootmaterialcodegroup", "zh-CN", "根物料编码组", "根物料编码组（同去重键下的根物料编码，分号分隔；ntext，按机种/部管时同机种根物料可能很多）"),
            // entity.ecpc.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.ecpc.ecrootmaterialcodegroup", "zh-HK", "根物料编码组_hk", "根物料编码组（同去重键下的根物料编码，分号分隔；ntext，按机种/部管时同机种根物料可能很多）"),

            // entity.ecpc.isimplemented
            new TranslationSeedItem("entity.ecpc.isimplemented", "en-US", "实施_us", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.ecpc.isimplemented
            new TranslationSeedItem("entity.ecpc.isimplemented", "ja-JP", "实施_jp", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.ecpc.isimplemented
            new TranslationSeedItem("entity.ecpc.isimplemented", "zh-CN", "实施", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.ecpc.isimplemented
            new TranslationSeedItem("entity.ecpc.isimplemented", "zh-HK", "实施_hk", "是否实施（0=否 1=是，字典 sys_yes_no）"),

            // entity.ecpc.scheduleddate
            new TranslationSeedItem("entity.ecpc.scheduleddate", "en-US", "预定日期_us", "预定日期"),
            // entity.ecpc.scheduleddate
            new TranslationSeedItem("entity.ecpc.scheduleddate", "ja-JP", "预定日期_jp", "预定日期"),
            // entity.ecpc.scheduleddate
            new TranslationSeedItem("entity.ecpc.scheduleddate", "zh-CN", "预定日期", "预定日期"),
            // entity.ecpc.scheduleddate
            new TranslationSeedItem("entity.ecpc.scheduleddate", "zh-HK", "预定日期_hk", "预定日期"),

            // entity.ecpc.scheduledbatch
            new TranslationSeedItem("entity.ecpc.scheduledbatch", "en-US", "预定批次_us", "预定批次"),
            // entity.ecpc.scheduledbatch
            new TranslationSeedItem("entity.ecpc.scheduledbatch", "ja-JP", "预定批次_jp", "预定批次"),
            // entity.ecpc.scheduledbatch
            new TranslationSeedItem("entity.ecpc.scheduledbatch", "zh-CN", "预定批次", "预定批次"),
            // entity.ecpc.scheduledbatch
            new TranslationSeedItem("entity.ecpc.scheduledbatch", "zh-HK", "预定批次_hk", "预定批次"),

            // entity.ecpc.execcontent
            new TranslationSeedItem("entity.ecpc.execcontent", "en-US", "执行内容_us", "执行内容（各部门通用）"),
            // entity.ecpc.execcontent
            new TranslationSeedItem("entity.ecpc.execcontent", "ja-JP", "执行内容_jp", "执行内容（各部门通用）"),
            // entity.ecpc.execcontent
            new TranslationSeedItem("entity.ecpc.execcontent", "zh-CN", "执行内容", "执行内容（各部门通用）"),
            // entity.ecpc.execcontent
            new TranslationSeedItem("entity.ecpc.execcontent", "zh-HK", "执行内容_hk", "执行内容（各部门通用）"),

            // entity.ecpc.poremainder
            new TranslationSeedItem("entity.ecpc.poremainder", "en-US", "Po残_us", "Po残（可空；非空须为采购单10位-序号4位:数量，如 4500123456-0010:18.5）"),
            // entity.ecpc.poremainder
            new TranslationSeedItem("entity.ecpc.poremainder", "ja-JP", "Po残_jp", "Po残（可空；非空须为采购单10位-序号4位:数量，如 4500123456-0010:18.5）"),
            // entity.ecpc.poremainder
            new TranslationSeedItem("entity.ecpc.poremainder", "zh-CN", "Po残", "Po残（可空；非空须为采购单10位-序号4位:数量，如 4500123456-0010:18.5）"),
            // entity.ecpc.poremainder
            new TranslationSeedItem("entity.ecpc.poremainder", "zh-HK", "Po残_hk", "Po残（可空；非空须为采购单10位-序号4位:数量，如 4500123456-0010:18.5）"),

            // entity.ecpc.balancequantity
            new TranslationSeedItem("entity.ecpc.balancequantity", "en-US", "结余数量_us", "结余数量"),
            // entity.ecpc.balancequantity
            new TranslationSeedItem("entity.ecpc.balancequantity", "ja-JP", "结余数量_jp", "结余数量"),
            // entity.ecpc.balancequantity
            new TranslationSeedItem("entity.ecpc.balancequantity", "zh-CN", "结余数量", "结余数量"),
            // entity.ecpc.balancequantity
            new TranslationSeedItem("entity.ecpc.balancequantity", "zh-HK", "结余数量_hk", "结余数量"),

            // entity.ecpc.ecoldpartdisposition
            new TranslationSeedItem("entity.ecpc.ecoldpartdisposition", "en-US", "旧品处理_us", "旧品处理（生管自行选择，不从明细带入；字典 logistics_manufacturing_ec_old_part_disposition；1=转用 2=废弃 3=返工 4=消耗 5=无处理 9=未定）"),
            // entity.ecpc.ecoldpartdisposition
            new TranslationSeedItem("entity.ecpc.ecoldpartdisposition", "ja-JP", "旧品处理_jp", "旧品处理（生管自行选择，不从明细带入；字典 logistics_manufacturing_ec_old_part_disposition；1=转用 2=废弃 3=返工 4=消耗 5=无处理 9=未定）"),
            // entity.ecpc.ecoldpartdisposition
            new TranslationSeedItem("entity.ecpc.ecoldpartdisposition", "zh-CN", "旧品处理", "旧品处理（生管自行选择，不从明细带入；字典 logistics_manufacturing_ec_old_part_disposition；1=转用 2=废弃 3=返工 4=消耗 5=无处理 9=未定）"),
            // entity.ecpc.ecoldpartdisposition
            new TranslationSeedItem("entity.ecpc.ecoldpartdisposition", "zh-HK", "旧品处理_hk", "旧品处理（生管自行选择，不从明细带入；字典 logistics_manufacturing_ec_old_part_disposition；1=转用 2=废弃 3=返工 4=消耗 5=无处理 9=未定）"),

            // entity.ecpc.ecrootmaterialdescription
            new TranslationSeedItem("entity.ecpc.ecrootmaterialdescription", "en-US", "根物料描述_us", "根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）"),
            // entity.ecpc.ecrootmaterialdescription
            new TranslationSeedItem("entity.ecpc.ecrootmaterialdescription", "ja-JP", "根物料描述_jp", "根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）"),
            // entity.ecpc.ecrootmaterialdescription
            new TranslationSeedItem("entity.ecpc.ecrootmaterialdescription", "zh-CN", "根物料描述", "根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）"),
            // entity.ecpc.ecrootmaterialdescription
            new TranslationSeedItem("entity.ecpc.ecrootmaterialdescription", "zh-HK", "根物料描述_hk", "根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）"),

            // entity.ecpc.deptcode
            new TranslationSeedItem("entity.ecpc.deptcode", "en-US", "部门编码_us", "部门编码（TaktDept.DeptCode；本表固定课别）"),
            // entity.ecpc.deptcode
            new TranslationSeedItem("entity.ecpc.deptcode", "ja-JP", "部门编码_jp", "部门编码（TaktDept.DeptCode；本表固定课别）"),
            // entity.ecpc.deptcode
            new TranslationSeedItem("entity.ecpc.deptcode", "zh-CN", "部门编码", "部门编码（TaktDept.DeptCode；本表固定课别）"),
            // entity.ecpc.deptcode
            new TranslationSeedItem("entity.ecpc.deptcode", "zh-HK", "部门编码_hk", "部门编码（TaktDept.DeptCode；本表固定课别）"),

            // entity.ecpc.deptname
            new TranslationSeedItem("entity.ecpc.deptname", "en-US", "部门名称_us", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),
            // entity.ecpc.deptname
            new TranslationSeedItem("entity.ecpc.deptname", "ja-JP", "部门名称_jp", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),
            // entity.ecpc.deptname
            new TranslationSeedItem("entity.ecpc.deptname", "zh-CN", "部门名称", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),
            // entity.ecpc.deptname
            new TranslationSeedItem("entity.ecpc.deptname", "zh-HK", "部门名称_hk", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),

            // entity.ecpc.ecimplscope
            new TranslationSeedItem("entity.ecpc.ecimplscope", "en-US", "实施范围_us", "实施范围（冗余：来自 TaktEcDetail.EcImplScope）"),
            // entity.ecpc.ecimplscope
            new TranslationSeedItem("entity.ecpc.ecimplscope", "ja-JP", "实施范围_jp", "实施范围（冗余：来自 TaktEcDetail.EcImplScope）"),
            // entity.ecpc.ecimplscope
            new TranslationSeedItem("entity.ecpc.ecimplscope", "zh-CN", "实施范围", "实施范围（冗余：来自 TaktEcDetail.EcImplScope）"),
            // entity.ecpc.ecimplscope
            new TranslationSeedItem("entity.ecpc.ecimplscope", "zh-HK", "实施范围_hk", "实施范围（冗余：来自 TaktEcDetail.EcImplScope）"),

            // entity.ecpc.eccontrolscope
            new TranslationSeedItem("entity.ecpc.eccontrolscope", "en-US", "控制范围_us", "控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品，2=按机种，3=按设变，4=不管理），只记录生管/制二/制一/品管；全仕向时生管与此相同。采购/受检/部管/SMT 固定按设变，制技固定按机种"),
            // entity.ecpc.eccontrolscope
            new TranslationSeedItem("entity.ecpc.eccontrolscope", "ja-JP", "控制范围_jp", "控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品，2=按机种，3=按设变，4=不管理），只记录生管/制二/制一/品管；全仕向时生管与此相同。采购/受检/部管/SMT 固定按设变，制技固定按机种"),
            // entity.ecpc.eccontrolscope
            new TranslationSeedItem("entity.ecpc.eccontrolscope", "zh-CN", "控制范围", "控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品，2=按机种，3=按设变，4=不管理），只记录生管/制二/制一/品管；全仕向时生管与此相同。采购/受检/部管/SMT 固定按设变，制技固定按机种"),
            // entity.ecpc.eccontrolscope
            new TranslationSeedItem("entity.ecpc.eccontrolscope", "zh-HK", "控制范围_hk", "控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品，2=按机种，3=按设变，4=不管理），只记录生管/制二/制一/品管；全仕向时生管与此相同。采购/受检/部管/SMT 固定按设变，制技固定按机种"),

            // entity.ecpc.ecgrainkey
            new TranslationSeedItem("entity.ecpc.ecgrainkey", "en-US", "执行去重键_us", "执行行去重键（设变单号、机种或根物料或新物料、分道；同一键的明细共用本行。ntext）"),
            // entity.ecpc.ecgrainkey
            new TranslationSeedItem("entity.ecpc.ecgrainkey", "ja-JP", "执行去重键_jp", "执行行去重键（设变单号、机种或根物料或新物料、分道；同一键的明细共用本行。ntext）"),
            // entity.ecpc.ecgrainkey
            new TranslationSeedItem("entity.ecpc.ecgrainkey", "zh-CN", "执行去重键", "执行行去重键（设变单号、机种或根物料或新物料、分道；同一键的明细共用本行。ntext）"),
            // entity.ecpc.ecgrainkey
            new TranslationSeedItem("entity.ecpc.ecgrainkey", "zh-HK", "执行去重键_hk", "执行行去重键（设变单号、机种或根物料或新物料、分道；同一键的明细共用本行。ntext）"),

            // entity.ecpc.isobsolete
            new TranslationSeedItem("entity.ecpc.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecpc.isobsolete
            new TranslationSeedItem("entity.ecpc.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecpc.isobsolete
            new TranslationSeedItem("entity.ecpc.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecpc.isobsolete
            new TranslationSeedItem("entity.ecpc.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
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
