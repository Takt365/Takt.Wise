// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcIqcI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEcIqc 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktEcIqc 实体国际化翻译种子（键前缀 entity.eciqc.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEcIqcI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEcIqc 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 eciqc 实体翻译...", tenantCode);

        foreach (var item in GetEcIqcTranslations())
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

        TaktLogger.Information("TaktEcIqc 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEcIqc 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.eciqc._self / entity.eciqc.{{field}}；ResourceGroup=EngineeringChange；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEcIqcTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.eciqc._self
            new TranslationSeedItem("entity.eciqc._self", "en-US", "Ec Iqc Information_us", "实体名称"),
            // entity.eciqc._self
            new TranslationSeedItem("entity.eciqc._self", "ja-JP", "设变受检课信息_jp", "实体名称"),
            // entity.eciqc._self
            new TranslationSeedItem("entity.eciqc._self", "zh-CN", "设变受检课信息", "实体名称"),
            // entity.eciqc._self
            new TranslationSeedItem("entity.eciqc._self", "zh-HK", "设变受检课信息_hk", "实体名称"),

            // entity.eciqc.linenumber
            new TranslationSeedItem("entity.eciqc.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.eciqc.linenumber
            new TranslationSeedItem("entity.eciqc.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.eciqc.linenumber
            new TranslationSeedItem("entity.eciqc.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.eciqc.linenumber
            new TranslationSeedItem("entity.eciqc.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.eciqc.discontinuedstatus
            new TranslationSeedItem("entity.eciqc.discontinuedstatus", "en-US", "停产状态_us", "停产状态（字典 logistics_materials_material_discontinued_status；DictValue=Z0/ZQ 等）。同设变单号+新品检验+新物料下任意成品不是停产（空或 Z0）时为 Z0，全部停产时取保留明细状态。"),
            // entity.eciqc.discontinuedstatus
            new TranslationSeedItem("entity.eciqc.discontinuedstatus", "ja-JP", "停产状态_jp", "停产状态（字典 logistics_materials_material_discontinued_status；DictValue=Z0/ZQ 等）。同设变单号+新品检验+新物料下任意成品不是停产（空或 Z0）时为 Z0，全部停产时取保留明细状态。"),
            // entity.eciqc.discontinuedstatus
            new TranslationSeedItem("entity.eciqc.discontinuedstatus", "zh-CN", "停产状态", "停产状态（字典 logistics_materials_material_discontinued_status；DictValue=Z0/ZQ 等）。同设变单号+新品检验+新物料下任意成品不是停产（空或 Z0）时为 Z0，全部停产时取保留明细状态。"),
            // entity.eciqc.discontinuedstatus
            new TranslationSeedItem("entity.eciqc.discontinuedstatus", "zh-HK", "停产状态_hk", "停产状态（字典 logistics_materials_material_discontinued_status；DictValue=Z0/ZQ 等）。同设变单号+新品检验+新物料下任意成品不是停产（空或 Z0）时为 Z0，全部停产时取保留明细状态。"),

            // entity.eciqc.eccode
            new TranslationSeedItem("entity.eciqc.eccode", "en-US", "设变单号_us", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),
            // entity.eciqc.eccode
            new TranslationSeedItem("entity.eciqc.eccode", "ja-JP", "设变单号_jp", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),
            // entity.eciqc.eccode
            new TranslationSeedItem("entity.eciqc.eccode", "zh-CN", "设变单号", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),
            // entity.eciqc.eccode
            new TranslationSeedItem("entity.eciqc.eccode", "zh-HK", "设变单号_hk", "设变单号（冗余：来自 TaktEcDetail.EcCode）"),

            // entity.eciqc.ecmodelcodegroup
            new TranslationSeedItem("entity.eciqc.ecmodelcodegroup", "en-US", "机种编码组_us", "机种编码组（同设变单号+新品检验+新物料下的机种编码，分号分隔；ntext）"),
            // entity.eciqc.ecmodelcodegroup
            new TranslationSeedItem("entity.eciqc.ecmodelcodegroup", "ja-JP", "机种编码组_jp", "机种编码组（同设变单号+新品检验+新物料下的机种编码，分号分隔；ntext）"),
            // entity.eciqc.ecmodelcodegroup
            new TranslationSeedItem("entity.eciqc.ecmodelcodegroup", "zh-CN", "机种编码组", "机种编码组（同设变单号+新品检验+新物料下的机种编码，分号分隔；ntext）"),
            // entity.eciqc.ecmodelcodegroup
            new TranslationSeedItem("entity.eciqc.ecmodelcodegroup", "zh-HK", "机种编码组_hk", "机种编码组（同设变单号+新品检验+新物料下的机种编码，分号分隔；ntext）"),

            // entity.eciqc.ecnewmaterialcode
            new TranslationSeedItem("entity.eciqc.ecnewmaterialcode", "en-US", "新物料编码_us", "新物料编码（冗余：来自 TaktEcDetail.EcNewMaterialCode）"),
            // entity.eciqc.ecnewmaterialcode
            new TranslationSeedItem("entity.eciqc.ecnewmaterialcode", "ja-JP", "新物料编码_jp", "新物料编码（冗余：来自 TaktEcDetail.EcNewMaterialCode）"),
            // entity.eciqc.ecnewmaterialcode
            new TranslationSeedItem("entity.eciqc.ecnewmaterialcode", "zh-CN", "新物料编码", "新物料编码（冗余：来自 TaktEcDetail.EcNewMaterialCode）"),
            // entity.eciqc.ecnewmaterialcode
            new TranslationSeedItem("entity.eciqc.ecnewmaterialcode", "zh-HK", "新物料编码_hk", "新物料编码（冗余：来自 TaktEcDetail.EcNewMaterialCode）"),

            // entity.eciqc.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.eciqc.ecrootmaterialcodegroup", "en-US", "根物料编码组_us", "根物料编码组（同设变单号+新品检验+新物料下的根物料编码，分号分隔；ntext）"),
            // entity.eciqc.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.eciqc.ecrootmaterialcodegroup", "ja-JP", "根物料编码组_jp", "根物料编码组（同设变单号+新品检验+新物料下的根物料编码，分号分隔；ntext）"),
            // entity.eciqc.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.eciqc.ecrootmaterialcodegroup", "zh-CN", "根物料编码组", "根物料编码组（同设变单号+新品检验+新物料下的根物料编码，分号分隔；ntext）"),
            // entity.eciqc.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.eciqc.ecrootmaterialcodegroup", "zh-HK", "根物料编码组_hk", "根物料编码组（同设变单号+新品检验+新物料下的根物料编码，分号分隔；ntext）"),

            // entity.eciqc.ecrootmaterialdescription
            new TranslationSeedItem("entity.eciqc.ecrootmaterialdescription", "en-US", "根物料描述_us", "根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）"),
            // entity.eciqc.ecrootmaterialdescription
            new TranslationSeedItem("entity.eciqc.ecrootmaterialdescription", "ja-JP", "根物料描述_jp", "根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）"),
            // entity.eciqc.ecrootmaterialdescription
            new TranslationSeedItem("entity.eciqc.ecrootmaterialdescription", "zh-CN", "根物料描述", "根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）"),
            // entity.eciqc.ecrootmaterialdescription
            new TranslationSeedItem("entity.eciqc.ecrootmaterialdescription", "zh-HK", "根物料描述_hk", "根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）"),

            // entity.eciqc.ecnewmaterialdescription
            new TranslationSeedItem("entity.eciqc.ecnewmaterialdescription", "en-US", "新物料描述_us", "新物料描述（冗余：来自 TaktEcDetail.EcNewMaterialDescription）"),
            // entity.eciqc.ecnewmaterialdescription
            new TranslationSeedItem("entity.eciqc.ecnewmaterialdescription", "ja-JP", "新物料描述_jp", "新物料描述（冗余：来自 TaktEcDetail.EcNewMaterialDescription）"),
            // entity.eciqc.ecnewmaterialdescription
            new TranslationSeedItem("entity.eciqc.ecnewmaterialdescription", "zh-CN", "新物料描述", "新物料描述（冗余：来自 TaktEcDetail.EcNewMaterialDescription）"),
            // entity.eciqc.ecnewmaterialdescription
            new TranslationSeedItem("entity.eciqc.ecnewmaterialdescription", "zh-HK", "新物料描述_hk", "新物料描述（冗余：来自 TaktEcDetail.EcNewMaterialDescription）"),

            // entity.eciqc.ecnewwarehouse
            new TranslationSeedItem("entity.eciqc.ecnewwarehouse", "en-US", "新品仓库_us", "新品仓库（冗余：来自 TaktEcDetail.EcNewWarehouse）"),
            // entity.eciqc.ecnewwarehouse
            new TranslationSeedItem("entity.eciqc.ecnewwarehouse", "ja-JP", "新品仓库_jp", "新品仓库（冗余：来自 TaktEcDetail.EcNewWarehouse）"),
            // entity.eciqc.ecnewwarehouse
            new TranslationSeedItem("entity.eciqc.ecnewwarehouse", "zh-CN", "新品仓库", "新品仓库（冗余：来自 TaktEcDetail.EcNewWarehouse）"),
            // entity.eciqc.ecnewwarehouse
            new TranslationSeedItem("entity.eciqc.ecnewwarehouse", "zh-HK", "新品仓库_hk", "新品仓库（冗余：来自 TaktEcDetail.EcNewWarehouse）"),

            // entity.eciqc.ecnewrequiresinspection
            new TranslationSeedItem("entity.eciqc.ecnewrequiresinspection", "en-US", "新品检验_us", "新品检验（检查；冗余：来自 TaktEcDetail.EcNewRequiresInspection）"),
            // entity.eciqc.ecnewrequiresinspection
            new TranslationSeedItem("entity.eciqc.ecnewrequiresinspection", "ja-JP", "新品检验_jp", "新品检验（检查；冗余：来自 TaktEcDetail.EcNewRequiresInspection）"),
            // entity.eciqc.ecnewrequiresinspection
            new TranslationSeedItem("entity.eciqc.ecnewrequiresinspection", "zh-CN", "新品检验", "新品检验（检查；冗余：来自 TaktEcDetail.EcNewRequiresInspection）"),
            // entity.eciqc.ecnewrequiresinspection
            new TranslationSeedItem("entity.eciqc.ecnewrequiresinspection", "zh-HK", "新品检验_hk", "新品检验（检查；冗余：来自 TaktEcDetail.EcNewRequiresInspection）"),

            // entity.eciqc.isimplemented
            new TranslationSeedItem("entity.eciqc.isimplemented", "en-US", "实施_us", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.eciqc.isimplemented
            new TranslationSeedItem("entity.eciqc.isimplemented", "ja-JP", "实施_jp", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.eciqc.isimplemented
            new TranslationSeedItem("entity.eciqc.isimplemented", "zh-CN", "实施", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.eciqc.isimplemented
            new TranslationSeedItem("entity.eciqc.isimplemented", "zh-HK", "实施_hk", "是否实施（0=否 1=是，字典 sys_yes_no）"),

            // entity.eciqc.execcontent
            new TranslationSeedItem("entity.eciqc.execcontent", "en-US", "执行内容_us", "执行内容（各部门通用）"),
            // entity.eciqc.execcontent
            new TranslationSeedItem("entity.eciqc.execcontent", "ja-JP", "执行内容_jp", "执行内容（各部门通用）"),
            // entity.eciqc.execcontent
            new TranslationSeedItem("entity.eciqc.execcontent", "zh-CN", "执行内容", "执行内容（各部门通用）"),
            // entity.eciqc.execcontent
            new TranslationSeedItem("entity.eciqc.execcontent", "zh-HK", "执行内容_hk", "执行内容（各部门通用）"),

            // entity.eciqc.iqcordercode
            new TranslationSeedItem("entity.eciqc.iqcordercode", "en-US", "受检单号_us", "受检单号"),
            // entity.eciqc.iqcordercode
            new TranslationSeedItem("entity.eciqc.iqcordercode", "ja-JP", "受检单号_jp", "受检单号"),
            // entity.eciqc.iqcordercode
            new TranslationSeedItem("entity.eciqc.iqcordercode", "zh-CN", "受检单号", "受检单号"),
            // entity.eciqc.iqcordercode
            new TranslationSeedItem("entity.eciqc.iqcordercode", "zh-HK", "受检单号_hk", "受检单号"),

            // entity.eciqc.inspectiondate
            new TranslationSeedItem("entity.eciqc.inspectiondate", "en-US", "检验日期_us", "检验日期"),
            // entity.eciqc.inspectiondate
            new TranslationSeedItem("entity.eciqc.inspectiondate", "ja-JP", "检验日期_jp", "检验日期"),
            // entity.eciqc.inspectiondate
            new TranslationSeedItem("entity.eciqc.inspectiondate", "zh-CN", "检验日期", "检验日期"),
            // entity.eciqc.inspectiondate
            new TranslationSeedItem("entity.eciqc.inspectiondate", "zh-HK", "检验日期_hk", "检验日期"),

            // entity.eciqc.deptcode
            new TranslationSeedItem("entity.eciqc.deptcode", "en-US", "部门编码_us", "部门编码（TaktDept.DeptCode；本表固定课别）"),
            // entity.eciqc.deptcode
            new TranslationSeedItem("entity.eciqc.deptcode", "ja-JP", "部门编码_jp", "部门编码（TaktDept.DeptCode；本表固定课别）"),
            // entity.eciqc.deptcode
            new TranslationSeedItem("entity.eciqc.deptcode", "zh-CN", "部门编码", "部门编码（TaktDept.DeptCode；本表固定课别）"),
            // entity.eciqc.deptcode
            new TranslationSeedItem("entity.eciqc.deptcode", "zh-HK", "部门编码_hk", "部门编码（TaktDept.DeptCode；本表固定课别）"),

            // entity.eciqc.deptname
            new TranslationSeedItem("entity.eciqc.deptname", "en-US", "部门名称_us", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),
            // entity.eciqc.deptname
            new TranslationSeedItem("entity.eciqc.deptname", "ja-JP", "部门名称_jp", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),
            // entity.eciqc.deptname
            new TranslationSeedItem("entity.eciqc.deptname", "zh-CN", "部门名称", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),
            // entity.eciqc.deptname
            new TranslationSeedItem("entity.eciqc.deptname", "zh-HK", "部门名称_hk", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),

            // entity.eciqc.ecimplscope
            new TranslationSeedItem("entity.eciqc.ecimplscope", "en-US", "实施范围_us", "实施范围（冗余：来自 TaktEcDetail.EcImplScope）"),
            // entity.eciqc.ecimplscope
            new TranslationSeedItem("entity.eciqc.ecimplscope", "ja-JP", "实施范围_jp", "实施范围（冗余：来自 TaktEcDetail.EcImplScope）"),
            // entity.eciqc.ecimplscope
            new TranslationSeedItem("entity.eciqc.ecimplscope", "zh-CN", "实施范围", "实施范围（冗余：来自 TaktEcDetail.EcImplScope）"),
            // entity.eciqc.ecimplscope
            new TranslationSeedItem("entity.eciqc.ecimplscope", "zh-HK", "实施范围_hk", "实施范围（冗余：来自 TaktEcDetail.EcImplScope）"),

            // entity.eciqc.eccontrolscope
            new TranslationSeedItem("entity.eciqc.eccontrolscope", "en-US", "控制范围_us", "控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品，2=按机种，3=按设变，4=不管理），只记录生管/制二/制一/品管；全仕向时生管与此相同。采购/受检/部管/SMT 固定按设变，制技固定按机种"),
            // entity.eciqc.eccontrolscope
            new TranslationSeedItem("entity.eciqc.eccontrolscope", "ja-JP", "控制范围_jp", "控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品，2=按机种，3=按设变，4=不管理），只记录生管/制二/制一/品管；全仕向时生管与此相同。采购/受检/部管/SMT 固定按设变，制技固定按机种"),
            // entity.eciqc.eccontrolscope
            new TranslationSeedItem("entity.eciqc.eccontrolscope", "zh-CN", "控制范围", "控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品，2=按机种，3=按设变，4=不管理），只记录生管/制二/制一/品管；全仕向时生管与此相同。采购/受检/部管/SMT 固定按设变，制技固定按机种"),
            // entity.eciqc.eccontrolscope
            new TranslationSeedItem("entity.eciqc.eccontrolscope", "zh-HK", "控制范围_hk", "控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品，2=按机种，3=按设变，4=不管理），只记录生管/制二/制一/品管；全仕向时生管与此相同。采购/受检/部管/SMT 固定按设变，制技固定按机种"),

            // entity.eciqc.ecgrainkey
            new TranslationSeedItem("entity.eciqc.ecgrainkey", "en-US", "执行去重键_us", "执行行去重键（设变单号、机种或根物料或新物料、分道；同一键的明细共用本行。ntext）"),
            // entity.eciqc.ecgrainkey
            new TranslationSeedItem("entity.eciqc.ecgrainkey", "ja-JP", "执行去重键_jp", "执行行去重键（设变单号、机种或根物料或新物料、分道；同一键的明细共用本行。ntext）"),
            // entity.eciqc.ecgrainkey
            new TranslationSeedItem("entity.eciqc.ecgrainkey", "zh-CN", "执行去重键", "执行行去重键（设变单号、机种或根物料或新物料、分道；同一键的明细共用本行。ntext）"),
            // entity.eciqc.ecgrainkey
            new TranslationSeedItem("entity.eciqc.ecgrainkey", "zh-HK", "执行去重键_hk", "执行行去重键（设变单号、机种或根物料或新物料、分道；同一键的明细共用本行。ntext）"),

            // entity.eciqc.isobsolete
            new TranslationSeedItem("entity.eciqc.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.eciqc.isobsolete
            new TranslationSeedItem("entity.eciqc.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.eciqc.isobsolete
            new TranslationSeedItem("entity.eciqc.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.eciqc.isobsolete
            new TranslationSeedItem("entity.eciqc.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
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
