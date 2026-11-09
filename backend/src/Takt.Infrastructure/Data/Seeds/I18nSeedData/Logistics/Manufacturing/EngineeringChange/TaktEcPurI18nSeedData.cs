// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcPurI18nSeedData.cs
// 创建时间：2026-11-06
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEcPur 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktEcPur 实体国际化翻译种子（键前缀 entity.ecpur.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEcPurI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEcPur 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 ecpur 实体翻译...", tenantCode);

        foreach (var item in GetEcPurTranslations())
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

        TaktLogger.Information("TaktEcPur 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEcPur 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.ecpur._self / entity.ecpur.{{field}}；ResourceGroup=EngineeringChange；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEcPurTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.ecpur._self
            new TranslationSeedItem("entity.ecpur._self", "en-US", "Ec Pur Information_us", "实体名称"),
            // entity.ecpur._self
            new TranslationSeedItem("entity.ecpur._self", "ja-JP", "设变采购课信息_jp", "实体名称"),
            // entity.ecpur._self
            new TranslationSeedItem("entity.ecpur._self", "zh-CN", "设变采购课信息", "实体名称"),
            // entity.ecpur._self
            new TranslationSeedItem("entity.ecpur._self", "zh-HK", "设变采购课信息_hk", "实体名称"),

            // entity.ecpur.linenumber
            new TranslationSeedItem("entity.ecpur.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.ecpur.linenumber
            new TranslationSeedItem("entity.ecpur.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.ecpur.linenumber
            new TranslationSeedItem("entity.ecpur.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.ecpur.linenumber
            new TranslationSeedItem("entity.ecpur.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.ecpur.discontinuedstatus
            new TranslationSeedItem("entity.ecpur.discontinuedstatus", "en-US", "停产状态_us", "停产状态（字典 logistics_materials_material_discontinued_status；DictValue=Z0/ZQ 等）。同设变下任意成品不是停产（空或 Z0）时为 Z0，全部停产时取保留明细状态。"),
            // entity.ecpur.discontinuedstatus
            new TranslationSeedItem("entity.ecpur.discontinuedstatus", "ja-JP", "停产状态_jp", "停产状态（字典 logistics_materials_material_discontinued_status；DictValue=Z0/ZQ 等）。同设变下任意成品不是停产（空或 Z0）时为 Z0，全部停产时取保留明细状态。"),
            // entity.ecpur.discontinuedstatus
            new TranslationSeedItem("entity.ecpur.discontinuedstatus", "zh-CN", "停产状态", "停产状态（字典 logistics_materials_material_discontinued_status；DictValue=Z0/ZQ 等）。同设变下任意成品不是停产（空或 Z0）时为 Z0，全部停产时取保留明细状态。"),
            // entity.ecpur.discontinuedstatus
            new TranslationSeedItem("entity.ecpur.discontinuedstatus", "zh-HK", "停产状态_hk", "停产状态（字典 logistics_materials_material_discontinued_status；DictValue=Z0/ZQ 等）。同设变下任意成品不是停产（空或 Z0）时为 Z0，全部停产时取保留明细状态。"),

            // entity.ecpur.eccode
            new TranslationSeedItem("entity.ecpur.eccode", "en-US", "设变单号_us", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),
            // entity.ecpur.eccode
            new TranslationSeedItem("entity.ecpur.eccode", "ja-JP", "设变单号_jp", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),
            // entity.ecpur.eccode
            new TranslationSeedItem("entity.ecpur.eccode", "zh-CN", "设变单号", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),
            // entity.ecpur.eccode
            new TranslationSeedItem("entity.ecpur.eccode", "zh-HK", "设变单号_hk", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),

            // entity.ecpur.ecmodelcodegroup
            new TranslationSeedItem("entity.ecpur.ecmodelcodegroup", "en-US", "机种编码组_us", "机种编码组（同设变下采购类型 F 明细的机种编码，分号分隔；ntext）"),
            // entity.ecpur.ecmodelcodegroup
            new TranslationSeedItem("entity.ecpur.ecmodelcodegroup", "ja-JP", "机种编码组_jp", "机种编码组（同设变下采购类型 F 明细的机种编码，分号分隔；ntext）"),
            // entity.ecpur.ecmodelcodegroup
            new TranslationSeedItem("entity.ecpur.ecmodelcodegroup", "zh-CN", "机种编码组", "机种编码组（同设变下采购类型 F 明细的机种编码，分号分隔；ntext）"),
            // entity.ecpur.ecmodelcodegroup
            new TranslationSeedItem("entity.ecpur.ecmodelcodegroup", "zh-HK", "机种编码组_hk", "机种编码组（同设变下采购类型 F 明细的机种编码，分号分隔；ntext）"),

            // entity.ecpur.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.ecpur.ecrootmaterialcodegroup", "en-US", "根物料编码组_us", "根物料编码组（同设变下采购类型 F 明细的根物料编码，分号分隔；ntext）"),
            // entity.ecpur.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.ecpur.ecrootmaterialcodegroup", "ja-JP", "根物料编码组_jp", "根物料编码组（同设变下采购类型 F 明细的根物料编码，分号分隔；ntext）"),
            // entity.ecpur.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.ecpur.ecrootmaterialcodegroup", "zh-CN", "根物料编码组", "根物料编码组（同设变下采购类型 F 明细的根物料编码，分号分隔；ntext）"),
            // entity.ecpur.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.ecpur.ecrootmaterialcodegroup", "zh-HK", "根物料编码组_hk", "根物料编码组（同设变下采购类型 F 明细的根物料编码，分号分隔；ntext）"),

            // entity.ecpur.ecnewmaterialcodegroup
            new TranslationSeedItem("entity.ecpur.ecnewmaterialcodegroup", "en-US", "新物料编码组_us", "新物料编码组（同设变下采购类型 F 的全部新物料编码，分号分隔；ntext）"),
            // entity.ecpur.ecnewmaterialcodegroup
            new TranslationSeedItem("entity.ecpur.ecnewmaterialcodegroup", "ja-JP", "新物料编码组_jp", "新物料编码组（同设变下采购类型 F 的全部新物料编码，分号分隔；ntext）"),
            // entity.ecpur.ecnewmaterialcodegroup
            new TranslationSeedItem("entity.ecpur.ecnewmaterialcodegroup", "zh-CN", "新物料编码组", "新物料编码组（同设变下采购类型 F 的全部新物料编码，分号分隔；ntext）"),
            // entity.ecpur.ecnewmaterialcodegroup
            new TranslationSeedItem("entity.ecpur.ecnewmaterialcodegroup", "zh-HK", "新物料编码组_hk", "新物料编码组（同设变下采购类型 F 的全部新物料编码，分号分隔；ntext）"),

            // entity.ecpur.ecoldmaterialcodegroup
            new TranslationSeedItem("entity.ecpur.ecoldmaterialcodegroup", "en-US", "旧物料编码组_us", "旧物料编码组（同设变下采购类型 F 明细的旧物料编码，分号分隔；ntext；供旧品结余按行拆分）"),
            // entity.ecpur.ecoldmaterialcodegroup
            new TranslationSeedItem("entity.ecpur.ecoldmaterialcodegroup", "ja-JP", "旧物料编码组_jp", "旧物料编码组（同设变下采购类型 F 明细的旧物料编码，分号分隔；ntext；供旧品结余按行拆分）"),
            // entity.ecpur.ecoldmaterialcodegroup
            new TranslationSeedItem("entity.ecpur.ecoldmaterialcodegroup", "zh-CN", "旧物料编码组", "旧物料编码组（同设变下采购类型 F 明细的旧物料编码，分号分隔；ntext；供旧品结余按行拆分）"),
            // entity.ecpur.ecoldmaterialcodegroup
            new TranslationSeedItem("entity.ecpur.ecoldmaterialcodegroup", "zh-HK", "旧物料编码组_hk", "旧物料编码组（同设变下采购类型 F 明细的旧物料编码，分号分隔；ntext；供旧品结余按行拆分）"),

            // entity.ecpur.ecnewpurchasetype
            new TranslationSeedItem("entity.ecpur.ecnewpurchasetype", "en-US", "新采购类型_us", "新采购类型（冗余：来自 TaktEcDetail.EcNewPurchaseType；本表落行均为 F）"),
            // entity.ecpur.ecnewpurchasetype
            new TranslationSeedItem("entity.ecpur.ecnewpurchasetype", "ja-JP", "新采购类型_jp", "新采购类型（冗余：来自 TaktEcDetail.EcNewPurchaseType；本表落行均为 F）"),
            // entity.ecpur.ecnewpurchasetype
            new TranslationSeedItem("entity.ecpur.ecnewpurchasetype", "zh-CN", "新采购类型", "新采购类型（冗余：来自 TaktEcDetail.EcNewPurchaseType；本表落行均为 F）"),
            // entity.ecpur.ecnewpurchasetype
            new TranslationSeedItem("entity.ecpur.ecnewpurchasetype", "zh-HK", "新采购类型_hk", "新采购类型（冗余：来自 TaktEcDetail.EcNewPurchaseType；本表落行均为 F）"),

            // entity.ecpur.isimplemented
            new TranslationSeedItem("entity.ecpur.isimplemented", "en-US", "实施_us", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.ecpur.isimplemented
            new TranslationSeedItem("entity.ecpur.isimplemented", "ja-JP", "实施_jp", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.ecpur.isimplemented
            new TranslationSeedItem("entity.ecpur.isimplemented", "zh-CN", "实施", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.ecpur.isimplemented
            new TranslationSeedItem("entity.ecpur.isimplemented", "zh-HK", "实施_hk", "是否实施（0=否 1=是，字典 sys_yes_no）"),

            // entity.ecpur.ecoldpartbalance
            new TranslationSeedItem("entity.ecpur.ecoldpartbalance", "en-US", "旧品结余_us", "旧品结余（可空；非空须按旧物料编码组逐行「旧物料-采购单10位:序号4位:数量;结余;处理方式;」，行数与旧物料组一致；旧物料与采购单用「-」，采购单后冒号，结余与处理方式各以分号分隔；采购自行维护，不从明细/生管带入；处理方式字典 logistics_manufacturing_ec_old_part_disposition；1=转用 2=废弃 3=返工 4=消耗 5=无处理 9=未定；nvarchar Length=-1）"),
            // entity.ecpur.ecoldpartbalance
            new TranslationSeedItem("entity.ecpur.ecoldpartbalance", "ja-JP", "旧品结余_jp", "旧品结余（可空；非空须按旧物料编码组逐行「旧物料-采购单10位:序号4位:数量;结余;处理方式;」，行数与旧物料组一致；旧物料与采购单用「-」，采购单后冒号，结余与处理方式各以分号分隔；采购自行维护，不从明细/生管带入；处理方式字典 logistics_manufacturing_ec_old_part_disposition；1=转用 2=废弃 3=返工 4=消耗 5=无处理 9=未定；nvarchar Length=-1）"),
            // entity.ecpur.ecoldpartbalance
            new TranslationSeedItem("entity.ecpur.ecoldpartbalance", "zh-CN", "旧品结余", "旧品结余（可空；非空须按旧物料编码组逐行「旧物料-采购单10位:序号4位:数量;结余;处理方式;」，行数与旧物料组一致；旧物料与采购单用「-」，采购单后冒号，结余与处理方式各以分号分隔；采购自行维护，不从明细/生管带入；处理方式字典 logistics_manufacturing_ec_old_part_disposition；1=转用 2=废弃 3=返工 4=消耗 5=无处理 9=未定；nvarchar Length=-1）"),
            // entity.ecpur.ecoldpartbalance
            new TranslationSeedItem("entity.ecpur.ecoldpartbalance", "zh-HK", "旧品结余_hk", "旧品结余（可空；非空须按旧物料编码组逐行「旧物料-采购单10位:序号4位:数量;结余;处理方式;」，行数与旧物料组一致；旧物料与采购单用「-」，采购单后冒号，结余与处理方式各以分号分隔；采购自行维护，不从明细/生管带入；处理方式字典 logistics_manufacturing_ec_old_part_disposition；1=转用 2=废弃 3=返工 4=消耗 5=无处理 9=未定；nvarchar Length=-1）"),

            // entity.ecpur.inbounddate
            new TranslationSeedItem("entity.ecpur.inbounddate", "en-US", "进货日期_us", "进货日期"),
            // entity.ecpur.inbounddate
            new TranslationSeedItem("entity.ecpur.inbounddate", "ja-JP", "进货日期_jp", "进货日期"),
            // entity.ecpur.inbounddate
            new TranslationSeedItem("entity.ecpur.inbounddate", "zh-CN", "进货日期", "进货日期"),
            // entity.ecpur.inbounddate
            new TranslationSeedItem("entity.ecpur.inbounddate", "zh-HK", "进货日期_hk", "进货日期"),

            // entity.ecpur.execcontent
            new TranslationSeedItem("entity.ecpur.execcontent", "en-US", "执行内容_us", "执行内容（按新物料编码组拆行：物料:采购单10位:序号4位:供应商; 多行组合；nvarchar Length=-1）"),
            // entity.ecpur.execcontent
            new TranslationSeedItem("entity.ecpur.execcontent", "ja-JP", "执行内容_jp", "执行内容（按新物料编码组拆行：物料:采购单10位:序号4位:供应商; 多行组合；nvarchar Length=-1）"),
            // entity.ecpur.execcontent
            new TranslationSeedItem("entity.ecpur.execcontent", "zh-CN", "执行内容", "执行内容（按新物料编码组拆行：物料:采购单10位:序号4位:供应商; 多行组合；nvarchar Length=-1）"),
            // entity.ecpur.execcontent
            new TranslationSeedItem("entity.ecpur.execcontent", "zh-HK", "执行内容_hk", "执行内容（按新物料编码组拆行：物料:采购单10位:序号4位:供应商; 多行组合；nvarchar Length=-1）"),

            // entity.ecpur.deptcode
            new TranslationSeedItem("entity.ecpur.deptcode", "en-US", "部门编码_us", "部门编码（TaktDept.DeptCode；本表固定课别）"),
            // entity.ecpur.deptcode
            new TranslationSeedItem("entity.ecpur.deptcode", "ja-JP", "部门编码_jp", "部门编码（TaktDept.DeptCode；本表固定课别）"),
            // entity.ecpur.deptcode
            new TranslationSeedItem("entity.ecpur.deptcode", "zh-CN", "部门编码", "部门编码（TaktDept.DeptCode；本表固定课别）"),
            // entity.ecpur.deptcode
            new TranslationSeedItem("entity.ecpur.deptcode", "zh-HK", "部门编码_hk", "部门编码（TaktDept.DeptCode；本表固定课别）"),

            // entity.ecpur.deptname
            new TranslationSeedItem("entity.ecpur.deptname", "en-US", "部门名称_us", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),
            // entity.ecpur.deptname
            new TranslationSeedItem("entity.ecpur.deptname", "ja-JP", "部门名称_jp", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),
            // entity.ecpur.deptname
            new TranslationSeedItem("entity.ecpur.deptname", "zh-CN", "部门名称", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),
            // entity.ecpur.deptname
            new TranslationSeedItem("entity.ecpur.deptname", "zh-HK", "部门名称_hk", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),

            // entity.ecpur.ecimplscope
            new TranslationSeedItem("entity.ecpur.ecimplscope", "en-US", "实施范围_us", "实施范围（冗余：来自 TaktEcDetail.EcImplScope）"),
            // entity.ecpur.ecimplscope
            new TranslationSeedItem("entity.ecpur.ecimplscope", "ja-JP", "实施范围_jp", "实施范围（冗余：来自 TaktEcDetail.EcImplScope）"),
            // entity.ecpur.ecimplscope
            new TranslationSeedItem("entity.ecpur.ecimplscope", "zh-CN", "实施范围", "实施范围（冗余：来自 TaktEcDetail.EcImplScope）"),
            // entity.ecpur.ecimplscope
            new TranslationSeedItem("entity.ecpur.ecimplscope", "zh-HK", "实施范围_hk", "实施范围（冗余：来自 TaktEcDetail.EcImplScope）"),

            // entity.ecpur.eccontrolscope
            new TranslationSeedItem("entity.ecpur.eccontrolscope", "en-US", "控制范围_us", "控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品，2=按机种，3=按设变，4=不管理）。采购固定按设变（DictValue=3）"),
            // entity.ecpur.eccontrolscope
            new TranslationSeedItem("entity.ecpur.eccontrolscope", "ja-JP", "控制范围_jp", "控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品，2=按机种，3=按设变，4=不管理）。采购固定按设变（DictValue=3）"),
            // entity.ecpur.eccontrolscope
            new TranslationSeedItem("entity.ecpur.eccontrolscope", "zh-CN", "控制范围", "控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品，2=按机种，3=按设变，4=不管理）。采购固定按设变（DictValue=3）"),
            // entity.ecpur.eccontrolscope
            new TranslationSeedItem("entity.ecpur.eccontrolscope", "zh-HK", "控制范围_hk", "控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品，2=按机种，3=按设变，4=不管理）。采购固定按设变（DictValue=3）"),

            // entity.ecpur.ecgrainkey
            new TranslationSeedItem("entity.ecpur.ecgrainkey", "en-US", "执行去重键_us", "执行行去重键（采购为设变单号；同一键的明细共用本行。ntext）"),
            // entity.ecpur.ecgrainkey
            new TranslationSeedItem("entity.ecpur.ecgrainkey", "ja-JP", "执行去重键_jp", "执行行去重键（采购为设变单号；同一键的明细共用本行。ntext）"),
            // entity.ecpur.ecgrainkey
            new TranslationSeedItem("entity.ecpur.ecgrainkey", "zh-CN", "执行去重键", "执行行去重键（采购为设变单号；同一键的明细共用本行。ntext）"),
            // entity.ecpur.ecgrainkey
            new TranslationSeedItem("entity.ecpur.ecgrainkey", "zh-HK", "执行去重键_hk", "执行行去重键（采购为设变单号；同一键的明细共用本行。ntext）"),

            // entity.ecpur.isobsolete
            new TranslationSeedItem("entity.ecpur.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecpur.isobsolete
            new TranslationSeedItem("entity.ecpur.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecpur.isobsolete
            new TranslationSeedItem("entity.ecpur.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecpur.isobsolete
            new TranslationSeedItem("entity.ecpur.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
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
