// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcMtlI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEcMtl 实体字段国际化种子（已对齐前端 locales：src/locales/logistics/manufacturing/engineering-change/ec-mtl）
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
/// TaktEcMtl 实体国际化翻译种子（键前缀 entity.ecmtl.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEcMtlI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEcMtl 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 ecmtl 实体翻译...", tenantCode);

        foreach (var item in GetEcMtlTranslations())
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

        TaktLogger.Information("TaktEcMtl 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEcMtl 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.ecmtl._self / entity.ecmtl.{{field}}；ResourceGroup=EngineeringChange；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEcMtlTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.ecmtl._self
            new TranslationSeedItem("entity.ecmtl._self", "en-US", "Ec Mtl Information_us", "实体名称"),
            // entity.ecmtl._self
            new TranslationSeedItem("entity.ecmtl._self", "ja-JP", "设变物料信息_jp", "实体名称"),
            // entity.ecmtl._self
            new TranslationSeedItem("entity.ecmtl._self", "zh-CN", "设变物料信息", "实体名称"),
            // entity.ecmtl._self
            new TranslationSeedItem("entity.ecmtl._self", "zh-HK", "设变物料信息_hk", "实体名称"),

            // entity.ecmtl.linenumber
            new TranslationSeedItem("entity.ecmtl.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.ecmtl.linenumber
            new TranslationSeedItem("entity.ecmtl.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.ecmtl.linenumber
            new TranslationSeedItem("entity.ecmtl.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.ecmtl.linenumber
            new TranslationSeedItem("entity.ecmtl.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.ecmtl.eccode
            new TranslationSeedItem("entity.ecmtl.eccode", "en-US", "设变单号_us", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),
            // entity.ecmtl.eccode
            new TranslationSeedItem("entity.ecmtl.eccode", "ja-JP", "设变单号_jp", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),
            // entity.ecmtl.eccode
            new TranslationSeedItem("entity.ecmtl.eccode", "zh-CN", "设变单号", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),
            // entity.ecmtl.eccode
            new TranslationSeedItem("entity.ecmtl.eccode", "zh-HK", "设变单号_hk", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),

            // entity.ecmtl.discontinuedstatus
            new TranslationSeedItem("entity.ecmtl.discontinuedstatus", "en-US", "停产状态_us", "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）"),
            // entity.ecmtl.discontinuedstatus
            new TranslationSeedItem("entity.ecmtl.discontinuedstatus", "ja-JP", "停产状态_jp", "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）"),
            // entity.ecmtl.discontinuedstatus
            new TranslationSeedItem("entity.ecmtl.discontinuedstatus", "zh-CN", "停产状态", "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）"),
            // entity.ecmtl.discontinuedstatus
            new TranslationSeedItem("entity.ecmtl.discontinuedstatus", "zh-HK", "停产状态_hk", "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）"),

            // entity.ecmtl.ecmodelcodegroup
            new TranslationSeedItem("entity.ecmtl.ecmodelcodegroup", "en-US", "机种编码组_us", "机种编码组（单个或多个机种，分号分隔；ntext）"),
            // entity.ecmtl.ecmodelcodegroup
            new TranslationSeedItem("entity.ecmtl.ecmodelcodegroup", "ja-JP", "机种编码组_jp", "机种编码组（单个或多个机种，分号分隔；ntext）"),
            // entity.ecmtl.ecmodelcodegroup
            new TranslationSeedItem("entity.ecmtl.ecmodelcodegroup", "zh-CN", "机种编码组", "机种编码组（单个或多个机种，分号分隔；ntext）"),
            // entity.ecmtl.ecmodelcodegroup
            new TranslationSeedItem("entity.ecmtl.ecmodelcodegroup", "zh-HK", "机种编码组_hk", "机种编码组（单个或多个机种，分号分隔；ntext）"),

            // entity.ecmtl.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.ecmtl.ecrootmaterialcodegroup", "en-US", "根物料编码组_us", "根物料编码组（同去重键下可见明细的根物料编码，分号分隔；ntext，同机种根物料可能很多）"),
            // entity.ecmtl.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.ecmtl.ecrootmaterialcodegroup", "ja-JP", "根物料编码组_jp", "根物料编码组（同去重键下可见明细的根物料编码，分号分隔；ntext，同机种根物料可能很多）"),
            // entity.ecmtl.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.ecmtl.ecrootmaterialcodegroup", "zh-CN", "根物料编码组", "根物料编码组（同去重键下可见明细的根物料编码，分号分隔；ntext，同机种根物料可能很多）"),
            // entity.ecmtl.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.ecmtl.ecrootmaterialcodegroup", "zh-HK", "根物料编码组_hk", "根物料编码组（同去重键下可见明细的根物料编码，分号分隔；ntext，同机种根物料可能很多）"),

            // entity.ecmtl.ecrootmaterialdescription
            new TranslationSeedItem("entity.ecmtl.ecrootmaterialdescription", "en-US", "根物料描述_us", "根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）"),
            // entity.ecmtl.ecrootmaterialdescription
            new TranslationSeedItem("entity.ecmtl.ecrootmaterialdescription", "ja-JP", "根物料描述_jp", "根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）"),
            // entity.ecmtl.ecrootmaterialdescription
            new TranslationSeedItem("entity.ecmtl.ecrootmaterialdescription", "zh-CN", "根物料描述", "根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）"),
            // entity.ecmtl.ecrootmaterialdescription
            new TranslationSeedItem("entity.ecmtl.ecrootmaterialdescription", "zh-HK", "根物料描述_hk", "根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）"),

            // entity.ecmtl.ecparentmaterialcode
            new TranslationSeedItem("entity.ecmtl.ecparentmaterialcode", "en-US", "上阶物料编码_us", "上阶物料编码（冗余：来自 TaktEcDetail.EcParentMaterialCode）"),
            // entity.ecmtl.ecparentmaterialcode
            new TranslationSeedItem("entity.ecmtl.ecparentmaterialcode", "ja-JP", "上阶物料编码_jp", "上阶物料编码（冗余：来自 TaktEcDetail.EcParentMaterialCode）"),
            // entity.ecmtl.ecparentmaterialcode
            new TranslationSeedItem("entity.ecmtl.ecparentmaterialcode", "zh-CN", "上阶物料编码", "上阶物料编码（冗余：来自 TaktEcDetail.EcParentMaterialCode）"),
            // entity.ecmtl.ecparentmaterialcode
            new TranslationSeedItem("entity.ecmtl.ecparentmaterialcode", "zh-HK", "上阶物料编码_hk", "上阶物料编码（冗余：来自 TaktEcDetail.EcParentMaterialCode）"),

            // entity.ecmtl.ecparentmaterialdescription
            new TranslationSeedItem("entity.ecmtl.ecparentmaterialdescription", "en-US", "上阶物料描述_us", "上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）"),
            // entity.ecmtl.ecparentmaterialdescription
            new TranslationSeedItem("entity.ecmtl.ecparentmaterialdescription", "ja-JP", "上阶物料描述_jp", "上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）"),
            // entity.ecmtl.ecparentmaterialdescription
            new TranslationSeedItem("entity.ecmtl.ecparentmaterialdescription", "zh-CN", "上阶物料描述", "上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）"),
            // entity.ecmtl.ecparentmaterialdescription
            new TranslationSeedItem("entity.ecmtl.ecparentmaterialdescription", "zh-HK", "上阶物料描述_hk", "上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）"),

            // entity.ecmtl.ecnewmaterialcode
            new TranslationSeedItem("entity.ecmtl.ecnewmaterialcode", "en-US", "新物料编码_us", "新物料编码（冗余：来自 TaktEcDetail.EcNewMaterialCode）"),
            // entity.ecmtl.ecnewmaterialcode
            new TranslationSeedItem("entity.ecmtl.ecnewmaterialcode", "ja-JP", "新物料编码_jp", "新物料编码（冗余：来自 TaktEcDetail.EcNewMaterialCode）"),
            // entity.ecmtl.ecnewmaterialcode
            new TranslationSeedItem("entity.ecmtl.ecnewmaterialcode", "zh-CN", "新物料编码", "新物料编码（冗余：来自 TaktEcDetail.EcNewMaterialCode）"),
            // entity.ecmtl.ecnewmaterialcode
            new TranslationSeedItem("entity.ecmtl.ecnewmaterialcode", "zh-HK", "新物料编码_hk", "新物料编码（冗余：来自 TaktEcDetail.EcNewMaterialCode）"),

            // entity.ecmtl.ecnewmaterialdescription
            new TranslationSeedItem("entity.ecmtl.ecnewmaterialdescription", "en-US", "新物料描述_us", "新物料描述（冗余：来自 TaktEcDetail.EcNewMaterialDescription）"),
            // entity.ecmtl.ecnewmaterialdescription
            new TranslationSeedItem("entity.ecmtl.ecnewmaterialdescription", "ja-JP", "新物料描述_jp", "新物料描述（冗余：来自 TaktEcDetail.EcNewMaterialDescription）"),
            // entity.ecmtl.ecnewmaterialdescription
            new TranslationSeedItem("entity.ecmtl.ecnewmaterialdescription", "zh-CN", "新物料描述", "新物料描述（冗余：来自 TaktEcDetail.EcNewMaterialDescription）"),
            // entity.ecmtl.ecnewmaterialdescription
            new TranslationSeedItem("entity.ecmtl.ecnewmaterialdescription", "zh-HK", "新物料描述_hk", "新物料描述（冗余：来自 TaktEcDetail.EcNewMaterialDescription）"),

            // entity.ecmtl.ecnewpurchasetype
            new TranslationSeedItem("entity.ecmtl.ecnewpurchasetype", "en-US", "新采购类型_us", "新采购类型（冗余：来自 TaktEcDetail.EcNewPurchaseType）"),
            // entity.ecmtl.ecnewpurchasetype
            new TranslationSeedItem("entity.ecmtl.ecnewpurchasetype", "ja-JP", "新采购类型_jp", "新采购类型（冗余：来自 TaktEcDetail.EcNewPurchaseType）"),
            // entity.ecmtl.ecnewpurchasetype
            new TranslationSeedItem("entity.ecmtl.ecnewpurchasetype", "zh-CN", "新采购类型", "新采购类型（冗余：来自 TaktEcDetail.EcNewPurchaseType）"),
            // entity.ecmtl.ecnewpurchasetype
            new TranslationSeedItem("entity.ecmtl.ecnewpurchasetype", "zh-HK", "新采购类型_hk", "新采购类型（冗余：来自 TaktEcDetail.EcNewPurchaseType）"),

            // entity.ecmtl.ecnewwarehouse
            new TranslationSeedItem("entity.ecmtl.ecnewwarehouse", "en-US", "新品仓库_us", "新品仓库（冗余：来自 TaktEcDetail.EcNewWarehouse）"),
            // entity.ecmtl.ecnewwarehouse
            new TranslationSeedItem("entity.ecmtl.ecnewwarehouse", "ja-JP", "新品仓库_jp", "新品仓库（冗余：来自 TaktEcDetail.EcNewWarehouse）"),
            // entity.ecmtl.ecnewwarehouse
            new TranslationSeedItem("entity.ecmtl.ecnewwarehouse", "zh-CN", "新品仓库", "新品仓库（冗余：来自 TaktEcDetail.EcNewWarehouse）"),
            // entity.ecmtl.ecnewwarehouse
            new TranslationSeedItem("entity.ecmtl.ecnewwarehouse", "zh-HK", "新品仓库_hk", "新品仓库（冗余：来自 TaktEcDetail.EcNewWarehouse）"),

            // entity.ecmtl.isimplemented
            new TranslationSeedItem("entity.ecmtl.isimplemented", "en-US", "实施_us", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.ecmtl.isimplemented
            new TranslationSeedItem("entity.ecmtl.isimplemented", "ja-JP", "实施_jp", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.ecmtl.isimplemented
            new TranslationSeedItem("entity.ecmtl.isimplemented", "zh-CN", "实施", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.ecmtl.isimplemented
            new TranslationSeedItem("entity.ecmtl.isimplemented", "zh-HK", "实施_hk", "是否实施（0=否 1=是，字典 sys_yes_no）"),

            // entity.ecmtl.scheduleddate
            new TranslationSeedItem("entity.ecmtl.scheduleddate", "en-US", "预定日期_us", "预定日期（冗余：来自 TaktEcPc.ScheduledDate）"),
            // entity.ecmtl.scheduleddate
            new TranslationSeedItem("entity.ecmtl.scheduleddate", "ja-JP", "预定日期_jp", "预定日期（冗余：来自 TaktEcPc.ScheduledDate）"),
            // entity.ecmtl.scheduleddate
            new TranslationSeedItem("entity.ecmtl.scheduleddate", "zh-CN", "预定日期", "预定日期（冗余：来自 TaktEcPc.ScheduledDate）"),
            // entity.ecmtl.scheduleddate
            new TranslationSeedItem("entity.ecmtl.scheduleddate", "zh-HK", "预定日期_hk", "预定日期（冗余：来自 TaktEcPc.ScheduledDate）"),

            // entity.ecmtl.scheduledbatch
            new TranslationSeedItem("entity.ecmtl.scheduledbatch", "en-US", "预定批次_us", "预定批次（冗余：来自 TaktEcPc.ScheduledBatch）"),
            // entity.ecmtl.scheduledbatch
            new TranslationSeedItem("entity.ecmtl.scheduledbatch", "ja-JP", "预定批次_jp", "预定批次（冗余：来自 TaktEcPc.ScheduledBatch）"),
            // entity.ecmtl.scheduledbatch
            new TranslationSeedItem("entity.ecmtl.scheduledbatch", "zh-CN", "预定批次", "预定批次（冗余：来自 TaktEcPc.ScheduledBatch）"),
            // entity.ecmtl.scheduledbatch
            new TranslationSeedItem("entity.ecmtl.scheduledbatch", "zh-HK", "预定批次_hk", "预定批次（冗余：来自 TaktEcPc.ScheduledBatch）"),

            // entity.ecmtl.execcontent
            new TranslationSeedItem("entity.ecmtl.execcontent", "en-US", "执行内容_us", "执行内容（各部门通用）"),
            // entity.ecmtl.execcontent
            new TranslationSeedItem("entity.ecmtl.execcontent", "ja-JP", "执行内容_jp", "执行内容（各部门通用）"),
            // entity.ecmtl.execcontent
            new TranslationSeedItem("entity.ecmtl.execcontent", "zh-CN", "执行内容", "执行内容（各部门通用）"),
            // entity.ecmtl.execcontent
            new TranslationSeedItem("entity.ecmtl.execcontent", "zh-HK", "执行内容_hk", "执行内容（各部门通用）"),

            // entity.ecmtl.outboundbatch
            new TranslationSeedItem("entity.ecmtl.outboundbatch", "en-US", "出库批次_us", "出库批次"),
            // entity.ecmtl.outboundbatch
            new TranslationSeedItem("entity.ecmtl.outboundbatch", "ja-JP", "出库批次_jp", "出库批次"),
            // entity.ecmtl.outboundbatch
            new TranslationSeedItem("entity.ecmtl.outboundbatch", "zh-CN", "出库批次", "出库批次"),
            // entity.ecmtl.outboundbatch
            new TranslationSeedItem("entity.ecmtl.outboundbatch", "zh-HK", "出库批次_hk", "出库批次"),

            // entity.ecmtl.outbounddate
            new TranslationSeedItem("entity.ecmtl.outbounddate", "en-US", "出库日期_us", "出库日期"),
            // entity.ecmtl.outbounddate
            new TranslationSeedItem("entity.ecmtl.outbounddate", "ja-JP", "出库日期_jp", "出库日期"),
            // entity.ecmtl.outbounddate
            new TranslationSeedItem("entity.ecmtl.outbounddate", "zh-CN", "出库日期", "出库日期"),
            // entity.ecmtl.outbounddate
            new TranslationSeedItem("entity.ecmtl.outbounddate", "zh-HK", "出库日期_hk", "出库日期"),

            // entity.ecmtl.deptcode
            new TranslationSeedItem("entity.ecmtl.deptcode", "en-US", "部门编码_us", "部门编码（固定 D0625 物料）"),
            // entity.ecmtl.deptcode
            new TranslationSeedItem("entity.ecmtl.deptcode", "ja-JP", "部门编码_jp", "部门编码（固定 D0625 物料）"),
            // entity.ecmtl.deptcode
            new TranslationSeedItem("entity.ecmtl.deptcode", "zh-CN", "部门编码", "部门编码（固定 D0625 物料）"),
            // entity.ecmtl.deptcode
            new TranslationSeedItem("entity.ecmtl.deptcode", "zh-HK", "部门编码_hk", "部门编码（固定 D0625 物料）"),

            // entity.ecmtl.deptname
            new TranslationSeedItem("entity.ecmtl.deptname", "en-US", "部门名称_us", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),
            // entity.ecmtl.deptname
            new TranslationSeedItem("entity.ecmtl.deptname", "ja-JP", "部门名称_jp", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),
            // entity.ecmtl.deptname
            new TranslationSeedItem("entity.ecmtl.deptname", "zh-CN", "部门名称", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),
            // entity.ecmtl.deptname
            new TranslationSeedItem("entity.ecmtl.deptname", "zh-HK", "部门名称_hk", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),

            // entity.ecmtl.ecimplscope
            new TranslationSeedItem("entity.ecmtl.ecimplscope", "en-US", "实施范围_us", "实施范围（冗余：来自 TaktEcDetail.EcImplScope）"),
            // entity.ecmtl.ecimplscope
            new TranslationSeedItem("entity.ecmtl.ecimplscope", "ja-JP", "实施范围_jp", "实施范围（冗余：来自 TaktEcDetail.EcImplScope）"),
            // entity.ecmtl.ecimplscope
            new TranslationSeedItem("entity.ecmtl.ecimplscope", "zh-CN", "实施范围", "实施范围（冗余：来自 TaktEcDetail.EcImplScope）"),
            // entity.ecmtl.ecimplscope
            new TranslationSeedItem("entity.ecmtl.ecimplscope", "zh-HK", "实施范围_hk", "实施范围（冗余：来自 TaktEcDetail.EcImplScope）"),

            // entity.ecmtl.eccontrolscope
            new TranslationSeedItem("entity.ecmtl.eccontrolscope", "en-US", "控制范围_us", "控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品，2=按机种，3=按设变，4=不管理），只记录生管/制二/制一/品管；全仕向时生管与此相同。采购/受检/部管/SMT 固定按设变，制技固定按机种"),
            // entity.ecmtl.eccontrolscope
            new TranslationSeedItem("entity.ecmtl.eccontrolscope", "ja-JP", "控制范围_jp", "控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品，2=按机种，3=按设变，4=不管理），只记录生管/制二/制一/品管；全仕向时生管与此相同。采购/受检/部管/SMT 固定按设变，制技固定按机种"),
            // entity.ecmtl.eccontrolscope
            new TranslationSeedItem("entity.ecmtl.eccontrolscope", "zh-CN", "控制范围", "控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品，2=按机种，3=按设变，4=不管理），只记录生管/制二/制一/品管；全仕向时生管与此相同。采购/受检/部管/SMT 固定按设变，制技固定按机种"),
            // entity.ecmtl.eccontrolscope
            new TranslationSeedItem("entity.ecmtl.eccontrolscope", "zh-HK", "控制范围_hk", "控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品，2=按机种，3=按设变，4=不管理），只记录生管/制二/制一/品管；全仕向时生管与此相同。采购/受检/部管/SMT 固定按设变，制技固定按机种"),

            // entity.ecmtl.ecgrainkey
            new TranslationSeedItem("entity.ecmtl.ecgrainkey", "en-US", "执行去重键_us", "执行行去重键（设变单号、机种或根物料或新物料、分道；同一键的明细共用本行。ntext）"),
            // entity.ecmtl.ecgrainkey
            new TranslationSeedItem("entity.ecmtl.ecgrainkey", "ja-JP", "执行去重键_jp", "执行行去重键（设变单号、机种或根物料或新物料、分道；同一键的明细共用本行。ntext）"),
            // entity.ecmtl.ecgrainkey
            new TranslationSeedItem("entity.ecmtl.ecgrainkey", "zh-CN", "执行去重键", "执行行去重键（设变单号、机种或根物料或新物料、分道；同一键的明细共用本行。ntext）"),
            // entity.ecmtl.ecgrainkey
            new TranslationSeedItem("entity.ecmtl.ecgrainkey", "zh-HK", "执行去重键_hk", "执行行去重键（设变单号、机种或根物料或新物料、分道；同一键的明细共用本行。ntext）"),

            // entity.ecmtl.isobsolete
            new TranslationSeedItem("entity.ecmtl.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecmtl.isobsolete
            new TranslationSeedItem("entity.ecmtl.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecmtl.isobsolete
            new TranslationSeedItem("entity.ecmtl.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecmtl.isobsolete
            new TranslationSeedItem("entity.ecmtl.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
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
