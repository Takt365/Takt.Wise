// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcBukanI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEcBukan 实体字段国际化种子（已对齐前端 locales：src/locales/logistics/manufacturing/engineering-change/ec-bukan）
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
/// TaktEcBukan 实体国际化翻译种子（键前缀 entity.ecbukan.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEcBukanI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEcBukan 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 ecbukan 实体翻译...", tenantCode);

        foreach (var item in GetEcBukanTranslations())
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

        TaktLogger.Information("TaktEcBukan 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEcBukan 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.ecbukan._self / entity.ecbukan.{{field}}；ResourceGroup=EngineeringChange；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEcBukanTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.ecbukan._self
            new TranslationSeedItem("entity.ecbukan._self", "en-US", "Ec Bukan Information_us", "实体名称"),
            // entity.ecbukan._self
            new TranslationSeedItem("entity.ecbukan._self", "ja-JP", "设变部管课信息_jp", "实体名称"),
            // entity.ecbukan._self
            new TranslationSeedItem("entity.ecbukan._self", "zh-CN", "设变部管课信息", "实体名称"),
            // entity.ecbukan._self
            new TranslationSeedItem("entity.ecbukan._self", "zh-HK", "设变部管课信息_hk", "实体名称"),

            // entity.ecbukan.linenumber
            new TranslationSeedItem("entity.ecbukan.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.ecbukan.linenumber
            new TranslationSeedItem("entity.ecbukan.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.ecbukan.linenumber
            new TranslationSeedItem("entity.ecbukan.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.ecbukan.linenumber
            new TranslationSeedItem("entity.ecbukan.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.ecbukan.eccode
            new TranslationSeedItem("entity.ecbukan.eccode", "en-US", "设变单号_us", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),
            // entity.ecbukan.eccode
            new TranslationSeedItem("entity.ecbukan.eccode", "ja-JP", "设变单号_jp", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),
            // entity.ecbukan.eccode
            new TranslationSeedItem("entity.ecbukan.eccode", "zh-CN", "设变单号", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),
            // entity.ecbukan.eccode
            new TranslationSeedItem("entity.ecbukan.eccode", "zh-HK", "设变单号_hk", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),

            // entity.ecbukan.discontinuedstatus
            new TranslationSeedItem("entity.ecbukan.discontinuedstatus", "en-US", "停产状态_us", "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）"),
            // entity.ecbukan.discontinuedstatus
            new TranslationSeedItem("entity.ecbukan.discontinuedstatus", "ja-JP", "停产状态_jp", "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）"),
            // entity.ecbukan.discontinuedstatus
            new TranslationSeedItem("entity.ecbukan.discontinuedstatus", "zh-CN", "停产状态", "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）"),
            // entity.ecbukan.discontinuedstatus
            new TranslationSeedItem("entity.ecbukan.discontinuedstatus", "zh-HK", "停产状态_hk", "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）"),

            // entity.ecbukan.ecmodelcodegroup
            new TranslationSeedItem("entity.ecbukan.ecmodelcodegroup", "en-US", "机种编码组_us", "机种编码组（单个或多个机种，分号分隔；ntext）"),
            // entity.ecbukan.ecmodelcodegroup
            new TranslationSeedItem("entity.ecbukan.ecmodelcodegroup", "ja-JP", "机种编码组_jp", "机种编码组（单个或多个机种，分号分隔；ntext）"),
            // entity.ecbukan.ecmodelcodegroup
            new TranslationSeedItem("entity.ecbukan.ecmodelcodegroup", "zh-CN", "机种编码组", "机种编码组（单个或多个机种，分号分隔；ntext）"),
            // entity.ecbukan.ecmodelcodegroup
            new TranslationSeedItem("entity.ecbukan.ecmodelcodegroup", "zh-HK", "机种编码组_hk", "机种编码组（单个或多个机种，分号分隔；ntext）"),

            // entity.ecbukan.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.ecbukan.ecrootmaterialcodegroup", "en-US", "根物料编码组_us", "根物料编码组（同去重键下可见明细的根物料编码，分号分隔；ntext，同机种根物料可能很多）"),
            // entity.ecbukan.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.ecbukan.ecrootmaterialcodegroup", "ja-JP", "根物料编码组_jp", "根物料编码组（同去重键下可见明细的根物料编码，分号分隔；ntext，同机种根物料可能很多）"),
            // entity.ecbukan.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.ecbukan.ecrootmaterialcodegroup", "zh-CN", "根物料编码组", "根物料编码组（同去重键下可见明细的根物料编码，分号分隔；ntext，同机种根物料可能很多）"),
            // entity.ecbukan.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.ecbukan.ecrootmaterialcodegroup", "zh-HK", "根物料编码组_hk", "根物料编码组（同去重键下可见明细的根物料编码，分号分隔；ntext，同机种根物料可能很多）"),

            // entity.ecbukan.ecrootmaterialdescription
            new TranslationSeedItem("entity.ecbukan.ecrootmaterialdescription", "en-US", "根物料描述_us", "根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）"),
            // entity.ecbukan.ecrootmaterialdescription
            new TranslationSeedItem("entity.ecbukan.ecrootmaterialdescription", "ja-JP", "根物料描述_jp", "根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）"),
            // entity.ecbukan.ecrootmaterialdescription
            new TranslationSeedItem("entity.ecbukan.ecrootmaterialdescription", "zh-CN", "根物料描述", "根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）"),
            // entity.ecbukan.ecrootmaterialdescription
            new TranslationSeedItem("entity.ecbukan.ecrootmaterialdescription", "zh-HK", "根物料描述_hk", "根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）"),

            // entity.ecbukan.ecnewmaterialcode
            new TranslationSeedItem("entity.ecbukan.ecnewmaterialcode", "en-US", "新物料编码_us", "新物料编码（冗余：来自 TaktEcDetail.EcNewMaterialCode）"),
            // entity.ecbukan.ecnewmaterialcode
            new TranslationSeedItem("entity.ecbukan.ecnewmaterialcode", "ja-JP", "新物料编码_jp", "新物料编码（冗余：来自 TaktEcDetail.EcNewMaterialCode）"),
            // entity.ecbukan.ecnewmaterialcode
            new TranslationSeedItem("entity.ecbukan.ecnewmaterialcode", "zh-CN", "新物料编码", "新物料编码（冗余：来自 TaktEcDetail.EcNewMaterialCode）"),
            // entity.ecbukan.ecnewmaterialcode
            new TranslationSeedItem("entity.ecbukan.ecnewmaterialcode", "zh-HK", "新物料编码_hk", "新物料编码（冗余：来自 TaktEcDetail.EcNewMaterialCode）"),

            // entity.ecbukan.ecnewmaterialdescription
            new TranslationSeedItem("entity.ecbukan.ecnewmaterialdescription", "en-US", "新物料描述_us", "新物料描述（冗余：来自 TaktEcDetail.EcNewMaterialDescription）"),
            // entity.ecbukan.ecnewmaterialdescription
            new TranslationSeedItem("entity.ecbukan.ecnewmaterialdescription", "ja-JP", "新物料描述_jp", "新物料描述（冗余：来自 TaktEcDetail.EcNewMaterialDescription）"),
            // entity.ecbukan.ecnewmaterialdescription
            new TranslationSeedItem("entity.ecbukan.ecnewmaterialdescription", "zh-CN", "新物料描述", "新物料描述（冗余：来自 TaktEcDetail.EcNewMaterialDescription）"),
            // entity.ecbukan.ecnewmaterialdescription
            new TranslationSeedItem("entity.ecbukan.ecnewmaterialdescription", "zh-HK", "新物料描述_hk", "新物料描述（冗余：来自 TaktEcDetail.EcNewMaterialDescription）"),

            // entity.ecbukan.ecnewpurchasetype
            new TranslationSeedItem("entity.ecbukan.ecnewpurchasetype", "en-US", "新采购类型_us", "新采购类型（冗余：来自 TaktEcDetail.EcNewPurchaseType）"),
            // entity.ecbukan.ecnewpurchasetype
            new TranslationSeedItem("entity.ecbukan.ecnewpurchasetype", "ja-JP", "新采购类型_jp", "新采购类型（冗余：来自 TaktEcDetail.EcNewPurchaseType）"),
            // entity.ecbukan.ecnewpurchasetype
            new TranslationSeedItem("entity.ecbukan.ecnewpurchasetype", "zh-CN", "新采购类型", "新采购类型（冗余：来自 TaktEcDetail.EcNewPurchaseType）"),
            // entity.ecbukan.ecnewpurchasetype
            new TranslationSeedItem("entity.ecbukan.ecnewpurchasetype", "zh-HK", "新采购类型_hk", "新采购类型（冗余：来自 TaktEcDetail.EcNewPurchaseType）"),

            // entity.ecbukan.ecnewwarehouse
            new TranslationSeedItem("entity.ecbukan.ecnewwarehouse", "en-US", "新品仓库_us", "新品仓库（冗余：来自 TaktEcDetail.EcNewWarehouse）"),
            // entity.ecbukan.ecnewwarehouse
            new TranslationSeedItem("entity.ecbukan.ecnewwarehouse", "ja-JP", "新品仓库_jp", "新品仓库（冗余：来自 TaktEcDetail.EcNewWarehouse）"),
            // entity.ecbukan.ecnewwarehouse
            new TranslationSeedItem("entity.ecbukan.ecnewwarehouse", "zh-CN", "新品仓库", "新品仓库（冗余：来自 TaktEcDetail.EcNewWarehouse）"),
            // entity.ecbukan.ecnewwarehouse
            new TranslationSeedItem("entity.ecbukan.ecnewwarehouse", "zh-HK", "新品仓库_hk", "新品仓库（冗余：来自 TaktEcDetail.EcNewWarehouse）"),

            // entity.ecbukan.isimplemented
            new TranslationSeedItem("entity.ecbukan.isimplemented", "en-US", "实施_us", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.ecbukan.isimplemented
            new TranslationSeedItem("entity.ecbukan.isimplemented", "ja-JP", "实施_jp", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.ecbukan.isimplemented
            new TranslationSeedItem("entity.ecbukan.isimplemented", "zh-CN", "实施", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.ecbukan.isimplemented
            new TranslationSeedItem("entity.ecbukan.isimplemented", "zh-HK", "实施_hk", "是否实施（0=否 1=是，字典 sys_yes_no）"),

            // entity.ecbukan.scheduleddate
            new TranslationSeedItem("entity.ecbukan.scheduleddate", "en-US", "预定日期_us", "预定日期（冗余：来自 TaktEcSeikan.ScheduledDate）"),
            // entity.ecbukan.scheduleddate
            new TranslationSeedItem("entity.ecbukan.scheduleddate", "ja-JP", "预定日期_jp", "预定日期（冗余：来自 TaktEcSeikan.ScheduledDate）"),
            // entity.ecbukan.scheduleddate
            new TranslationSeedItem("entity.ecbukan.scheduleddate", "zh-CN", "预定日期", "预定日期（冗余：来自 TaktEcSeikan.ScheduledDate）"),
            // entity.ecbukan.scheduleddate
            new TranslationSeedItem("entity.ecbukan.scheduleddate", "zh-HK", "预定日期_hk", "预定日期（冗余：来自 TaktEcSeikan.ScheduledDate）"),

            // entity.ecbukan.scheduledbatch
            new TranslationSeedItem("entity.ecbukan.scheduledbatch", "en-US", "预定批次_us", "预定批次（冗余：来自 TaktEcSeikan.ScheduledBatch）"),
            // entity.ecbukan.scheduledbatch
            new TranslationSeedItem("entity.ecbukan.scheduledbatch", "ja-JP", "预定批次_jp", "预定批次（冗余：来自 TaktEcSeikan.ScheduledBatch）"),
            // entity.ecbukan.scheduledbatch
            new TranslationSeedItem("entity.ecbukan.scheduledbatch", "zh-CN", "预定批次", "预定批次（冗余：来自 TaktEcSeikan.ScheduledBatch）"),
            // entity.ecbukan.scheduledbatch
            new TranslationSeedItem("entity.ecbukan.scheduledbatch", "zh-HK", "预定批次_hk", "预定批次（冗余：来自 TaktEcSeikan.ScheduledBatch）"),

            // entity.ecbukan.execcontent
            new TranslationSeedItem("entity.ecbukan.execcontent", "en-US", "执行内容_us", "执行内容（各部门通用）"),
            // entity.ecbukan.execcontent
            new TranslationSeedItem("entity.ecbukan.execcontent", "ja-JP", "执行内容_jp", "执行内容（各部门通用）"),
            // entity.ecbukan.execcontent
            new TranslationSeedItem("entity.ecbukan.execcontent", "zh-CN", "执行内容", "执行内容（各部门通用）"),
            // entity.ecbukan.execcontent
            new TranslationSeedItem("entity.ecbukan.execcontent", "zh-HK", "执行内容_hk", "执行内容（各部门通用）"),

            // entity.ecbukan.outboundbatch
            new TranslationSeedItem("entity.ecbukan.outboundbatch", "en-US", "出库批次_us", "出库批次"),
            // entity.ecbukan.outboundbatch
            new TranslationSeedItem("entity.ecbukan.outboundbatch", "ja-JP", "出库批次_jp", "出库批次"),
            // entity.ecbukan.outboundbatch
            new TranslationSeedItem("entity.ecbukan.outboundbatch", "zh-CN", "出库批次", "出库批次"),
            // entity.ecbukan.outboundbatch
            new TranslationSeedItem("entity.ecbukan.outboundbatch", "zh-HK", "出库批次_hk", "出库批次"),

            // entity.ecbukan.outbounddate
            new TranslationSeedItem("entity.ecbukan.outbounddate", "en-US", "出库日期_us", "出库日期"),
            // entity.ecbukan.outbounddate
            new TranslationSeedItem("entity.ecbukan.outbounddate", "ja-JP", "出库日期_jp", "出库日期"),
            // entity.ecbukan.outbounddate
            new TranslationSeedItem("entity.ecbukan.outbounddate", "zh-CN", "出库日期", "出库日期"),
            // entity.ecbukan.outbounddate
            new TranslationSeedItem("entity.ecbukan.outbounddate", "zh-HK", "出库日期_hk", "出库日期"),

            // entity.ecbukan.deptcode
            new TranslationSeedItem("entity.ecbukan.deptcode", "en-US", "部门编码_us", "部门编码（TaktDept.DeptCode；本表固定课别）"),
            // entity.ecbukan.deptcode
            new TranslationSeedItem("entity.ecbukan.deptcode", "ja-JP", "部门编码_jp", "部门编码（TaktDept.DeptCode；本表固定课别）"),
            // entity.ecbukan.deptcode
            new TranslationSeedItem("entity.ecbukan.deptcode", "zh-CN", "部门编码", "部门编码（TaktDept.DeptCode；本表固定课别）"),
            // entity.ecbukan.deptcode
            new TranslationSeedItem("entity.ecbukan.deptcode", "zh-HK", "部门编码_hk", "部门编码（TaktDept.DeptCode；本表固定课别）"),

            // entity.ecbukan.deptname
            new TranslationSeedItem("entity.ecbukan.deptname", "en-US", "部门名称_us", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),
            // entity.ecbukan.deptname
            new TranslationSeedItem("entity.ecbukan.deptname", "ja-JP", "部门名称_jp", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),
            // entity.ecbukan.deptname
            new TranslationSeedItem("entity.ecbukan.deptname", "zh-CN", "部门名称", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),
            // entity.ecbukan.deptname
            new TranslationSeedItem("entity.ecbukan.deptname", "zh-HK", "部门名称_hk", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),

            // entity.ecbukan.ecscope
            new TranslationSeedItem("entity.ecbukan.ecscope", "en-US", "实施范围_us", "实施范围（冗余：来自 TaktEcDetail.EcScope）"),
            // entity.ecbukan.ecscope
            new TranslationSeedItem("entity.ecbukan.ecscope", "ja-JP", "实施范围_jp", "实施范围（冗余：来自 TaktEcDetail.EcScope）"),
            // entity.ecbukan.ecscope
            new TranslationSeedItem("entity.ecbukan.ecscope", "zh-CN", "实施范围", "实施范围（冗余：来自 TaktEcDetail.EcScope）"),
            // entity.ecbukan.ecscope
            new TranslationSeedItem("entity.ecbukan.ecscope", "zh-HK", "实施范围_hk", "实施范围（冗余：来自 TaktEcDetail.EcScope）"),

            // entity.ecbukan.ecdeptscope
            new TranslationSeedItem("entity.ecbukan.ecdeptscope", "en-US", "部门实施范围_us", "部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）"),
            // entity.ecbukan.ecdeptscope
            new TranslationSeedItem("entity.ecbukan.ecdeptscope", "ja-JP", "部门实施范围_jp", "部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）"),
            // entity.ecbukan.ecdeptscope
            new TranslationSeedItem("entity.ecbukan.ecdeptscope", "zh-CN", "部门实施范围", "部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）"),
            // entity.ecbukan.ecdeptscope
            new TranslationSeedItem("entity.ecbukan.ecdeptscope", "zh-HK", "部门实施范围_hk", "部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）"),

            // entity.ecbukan.ecdetailid
            new TranslationSeedItem("entity.ecbukan.ecdetailid", "en-US", "设变明细ID_us", "设变明细 ID（TaktEcDetail 主键；去重组内代表/种子明细 Id；同组多明细按业务键 FanOut）"),
            // entity.ecbukan.ecdetailid
            new TranslationSeedItem("entity.ecbukan.ecdetailid", "ja-JP", "设变明细ID_jp", "设变明细 ID（TaktEcDetail 主键；去重组内代表/种子明细 Id；同组多明细按业务键 FanOut）"),
            // entity.ecbukan.ecdetailid
            new TranslationSeedItem("entity.ecbukan.ecdetailid", "zh-CN", "设变明细ID", "设变明细 ID（TaktEcDetail 主键；去重组内代表/种子明细 Id；同组多明细按业务键 FanOut）"),
            // entity.ecbukan.ecdetailid
            new TranslationSeedItem("entity.ecbukan.ecdetailid", "zh-HK", "设变明细ID_hk", "设变明细 ID（TaktEcDetail 主键；去重组内代表/种子明细 Id；同组多明细按业务键 FanOut）"),

            // entity.ecbukan.isobsolete
            new TranslationSeedItem("entity.ecbukan.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecbukan.isobsolete
            new TranslationSeedItem("entity.ecbukan.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecbukan.isobsolete
            new TranslationSeedItem("entity.ecbukan.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecbukan.isobsolete
            new TranslationSeedItem("entity.ecbukan.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.ecbukan.ecdetail
            new TranslationSeedItem("entity.ecbukan.ecdetail", "en-US", "设变明细_us", "设变明细（数据主从：本表由明细派生；多对一，外键 EcDetailId → TaktEcDetail.Id）"),
            // entity.ecbukan.ecdetail
            new TranslationSeedItem("entity.ecbukan.ecdetail", "ja-JP", "设变明细_jp", "设变明细（数据主从：本表由明细派生；多对一，外键 EcDetailId → TaktEcDetail.Id）"),
            // entity.ecbukan.ecdetail
            new TranslationSeedItem("entity.ecbukan.ecdetail", "zh-CN", "设变明细", "设变明细（数据主从：本表由明细派生；多对一，外键 EcDetailId → TaktEcDetail.Id）"),
            // entity.ecbukan.ecdetail
            new TranslationSeedItem("entity.ecbukan.ecdetail", "zh-HK", "设变明细_hk", "设变明细（数据主从：本表由明细派生；多对一，外键 EcDetailId → TaktEcDetail.Id）"),
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
