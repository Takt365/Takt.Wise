// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcSmtI18nSeedData.cs
// 创建时间：2026-09-08
// 创建人：Takt365(Cursor AI)
// 功能描述：TaktEcSmt 实体字段国际化种子（TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
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
/// TaktEcSmt 实体国际化翻译种子（键前缀 entity.ecsmt.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEcSmtI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEcSmt 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 ecsmt 实体翻译...", tenantCode);

        foreach (var item in GetEcSmtTranslations())
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

        TaktLogger.Information("TaktEcSmt 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEcSmt 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.ecsmt._self / entity.ecsmt.{{field}}；ResourceGroup=EngineeringChange；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEcSmtTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.ecsmt._self
            new TranslationSeedItem("entity.ecsmt._self", "en-US", "Ec Pcba Electronic Information_us", "实体名称"),
            // entity.ecsmt._self
            new TranslationSeedItem("entity.ecsmt._self", "ja-JP", "设变SMT执行_jp", "实体名称"),
            // entity.ecsmt._self
            new TranslationSeedItem("entity.ecsmt._self", "zh-CN", "设变SMT执行", "实体名称"),
            // entity.ecsmt._self
            new TranslationSeedItem("entity.ecsmt._self", "zh-HK", "设变SMT执行_hk", "实体名称"),

            // entity.ecsmt.ecdetailid
            new TranslationSeedItem("entity.ecsmt.ecdetailid", "en-US", "设变明细ID_us", "设变明细 ID（TaktEcDetail 主键；主表多对一导航 EcDetail）"),
            // entity.ecsmt.ecdetailid
            new TranslationSeedItem("entity.ecsmt.ecdetailid", "ja-JP", "设变明细ID_jp", "设变明细 ID（TaktEcDetail 主键；主表多对一导航 EcDetail）"),
            // entity.ecsmt.ecdetailid
            new TranslationSeedItem("entity.ecsmt.ecdetailid", "zh-CN", "设变明细ID", "设变明细 ID（TaktEcDetail 主键；主表多对一导航 EcDetail）"),
            // entity.ecsmt.ecdetailid
            new TranslationSeedItem("entity.ecsmt.ecdetailid", "zh-HK", "设变明细ID_hk", "设变明细 ID（TaktEcDetail 主键；主表多对一导航 EcDetail）"),

            // entity.ecsmt.eccode
            new TranslationSeedItem("entity.ecsmt.eccode", "en-US", "设变单号_us", "设变单号（冗余，便于查询）"),
            // entity.ecsmt.eccode
            new TranslationSeedItem("entity.ecsmt.eccode", "ja-JP", "设变单号_jp", "设变单号（冗余，便于查询）"),
            // entity.ecsmt.eccode
            new TranslationSeedItem("entity.ecsmt.eccode", "zh-CN", "设变单号", "设变单号（冗余，便于查询）"),
            // entity.ecsmt.eccode
            new TranslationSeedItem("entity.ecsmt.eccode", "zh-HK", "设变单号_hk", "设变单号（冗余，便于查询）"),

            // entity.ecsmt.linenumber
            new TranslationSeedItem("entity.ecsmt.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.ecsmt.linenumber
            new TranslationSeedItem("entity.ecsmt.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.ecsmt.linenumber
            new TranslationSeedItem("entity.ecsmt.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.ecsmt.linenumber
            new TranslationSeedItem("entity.ecsmt.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.ecsmt.ecmodelcode
            new TranslationSeedItem("entity.ecsmt.ecmodelcode", "en-US", "Model code_us", "机种编码（冗余：来自 TaktEcDetail.EcModelCode）"),
            // entity.ecsmt.ecmodelcode
            new TranslationSeedItem("entity.ecsmt.ecmodelcode", "ja-JP", "機種コード_jp", "机种编码（冗余：来自 TaktEcDetail.EcModelCode）"),
            // entity.ecsmt.ecmodelcode
            new TranslationSeedItem("entity.ecsmt.ecmodelcode", "zh-CN", "机种编码", "机种编码（冗余：来自 TaktEcDetail.EcModelCode）"),
            // entity.ecsmt.ecmodelcode
            new TranslationSeedItem("entity.ecsmt.ecmodelcode", "zh-HK", "機種編碼_hk", "机种编码（冗余：来自 TaktEcDetail.EcModelCode）"),

            // entity.ecsmt.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.ecsmt.ecrootmaterialcodegroup", "en-US", "根物料编码组_us", "根物料编码组（同设变单号+机种编码下可见明细的根物料编码，分号分隔；ntext）"),
            // entity.ecsmt.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.ecsmt.ecrootmaterialcodegroup", "ja-JP", "根物料编码组_jp", "根物料编码组（同设变单号+机种编码下可见明细的根物料编码，分号分隔；ntext）"),
            // entity.ecsmt.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.ecsmt.ecrootmaterialcodegroup", "zh-CN", "根物料编码组", "根物料编码组（同设变单号+机种编码下可见明细的根物料编码，分号分隔；ntext）"),
            // entity.ecsmt.ecrootmaterialcodegroup
            new TranslationSeedItem("entity.ecsmt.ecrootmaterialcodegroup", "zh-HK", "根物料编码组_hk", "根物料编码组（同设变单号+机种编码下可见明细的根物料编码，分号分隔；ntext）"),

            // entity.ecsmt.ecparentmaterialcode
            new TranslationSeedItem("entity.ecsmt.ecparentmaterialcode", "en-US", "Parent material code_us", "上阶物料编码（冗余：来自 TaktEcDetail.EcParentMaterialCode）"),
            // entity.ecsmt.ecparentmaterialcode
            new TranslationSeedItem("entity.ecsmt.ecparentmaterialcode", "ja-JP", "上階品目コード_jp", "上阶物料编码（冗余：来自 TaktEcDetail.EcParentMaterialCode）"),
            // entity.ecsmt.ecparentmaterialcode
            new TranslationSeedItem("entity.ecsmt.ecparentmaterialcode", "zh-CN", "上阶物料编码", "上阶物料编码（冗余：来自 TaktEcDetail.EcParentMaterialCode）"),
            // entity.ecsmt.ecparentmaterialcode
            new TranslationSeedItem("entity.ecsmt.ecparentmaterialcode", "zh-HK", "上階物料編碼_hk", "上阶物料编码（冗余：来自 TaktEcDetail.EcParentMaterialCode）"),

            // entity.ecsmt.ecparentmaterialdescription
            new TranslationSeedItem("entity.ecsmt.ecparentmaterialdescription", "en-US", "Parent material description_us", "上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）"),
            // entity.ecsmt.ecparentmaterialdescription
            new TranslationSeedItem("entity.ecsmt.ecparentmaterialdescription", "ja-JP", "上階品目記述_jp", "上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）"),
            // entity.ecsmt.ecparentmaterialdescription
            new TranslationSeedItem("entity.ecsmt.ecparentmaterialdescription", "zh-CN", "上阶物料描述", "上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）"),
            // entity.ecsmt.ecparentmaterialdescription
            new TranslationSeedItem("entity.ecsmt.ecparentmaterialdescription", "zh-HK", "上階物料描述_hk", "上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）"),

            // entity.ecsmt.discontinuedstatus
            new TranslationSeedItem("entity.ecsmt.discontinuedstatus", "en-US", "根物料EOL_us", "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）"),
            // entity.ecsmt.discontinuedstatus
            new TranslationSeedItem("entity.ecsmt.discontinuedstatus", "ja-JP", "根物料EOL_jp", "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）"),
            // entity.ecsmt.discontinuedstatus
            new TranslationSeedItem("entity.ecsmt.discontinuedstatus", "zh-CN", "根物料EOL", "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）"),
            // entity.ecsmt.discontinuedstatus
            new TranslationSeedItem("entity.ecsmt.discontinuedstatus", "zh-HK", "根物料EOL_hk", "停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）"),

            
            // entity.ecsmt.ecscope
            new TranslationSeedItem("entity.ecsmt.ecscope", "en-US", "Scope", "实施范围（冗余：来自 TaktEcDetail.EcScope）"),
            // entity.ecsmt.ecscope
            new TranslationSeedItem("entity.ecsmt.ecscope", "ja-JP", "実施範囲", "实施范围（冗余：来自 TaktEcDetail.EcScope）"),
            // entity.ecsmt.ecscope
            new TranslationSeedItem("entity.ecsmt.ecscope", "zh-CN", "实施范围", "实施范围（冗余：来自 TaktEcDetail.EcScope）"),
            // entity.ecsmt.ecscope
            new TranslationSeedItem("entity.ecsmt.ecscope", "zh-HK", "實施範圍", "实施范围（冗余：来自 TaktEcDetail.EcScope）"),
// entity.ecsmt.ecnewmaterialcode
            new TranslationSeedItem("entity.ecsmt.ecnewmaterialcode", "en-US", "新物料编码_us", "新物料编码（冗余：来自 TaktEcDetail.EcNewMaterialCode；执行行按设变单号+机种编码去重）"),
            // entity.ecsmt.ecnewmaterialcode
            new TranslationSeedItem("entity.ecsmt.ecnewmaterialcode", "ja-JP", "新物料编码_jp", "新物料编码（冗余：来自 TaktEcDetail.EcNewMaterialCode；执行行按设变单号+机种编码去重）"),
            // entity.ecsmt.ecnewmaterialcode
            new TranslationSeedItem("entity.ecsmt.ecnewmaterialcode", "zh-CN", "新物料编码", "新物料编码（冗余：来自 TaktEcDetail.EcNewMaterialCode；执行行按设变单号+机种编码去重）"),
            // entity.ecsmt.ecnewmaterialcode
            new TranslationSeedItem("entity.ecsmt.ecnewmaterialcode", "zh-HK", "新物料编码_hk", "新物料编码（冗余：来自 TaktEcDetail.EcNewMaterialCode；执行行按设变单号+机种编码去重）"),

            // entity.ecsmt.ecnewmaterialdescription
            new TranslationSeedItem("entity.ecsmt.ecnewmaterialdescription", "en-US", "新物料描述_us", "新物料描述（冗余：来自 TaktEcDetail.EcNewMaterialDescription）"),
            // entity.ecsmt.ecnewmaterialdescription
            new TranslationSeedItem("entity.ecsmt.ecnewmaterialdescription", "ja-JP", "新物料描述_jp", "新物料描述（冗余：来自 TaktEcDetail.EcNewMaterialDescription）"),
            // entity.ecsmt.ecnewmaterialdescription
            new TranslationSeedItem("entity.ecsmt.ecnewmaterialdescription", "zh-CN", "新物料描述", "新物料描述（冗余：来自 TaktEcDetail.EcNewMaterialDescription）"),
            // entity.ecsmt.ecnewmaterialdescription
            new TranslationSeedItem("entity.ecsmt.ecnewmaterialdescription", "zh-HK", "新物料描述_hk", "新物料描述（冗余：来自 TaktEcDetail.EcNewMaterialDescription）"),

            // entity.ecsmt.ecnewpurchasetype
            new TranslationSeedItem("entity.ecsmt.ecnewpurchasetype", "en-US", "新采购类型_us", "新采购类型（F=外部采购，E=自制生产；冗余：来自 TaktEcDetail.EcNewPurchaseType；本表仅 F）"),
            // entity.ecsmt.ecnewpurchasetype
            new TranslationSeedItem("entity.ecsmt.ecnewpurchasetype", "ja-JP", "新采购类型_jp", "新采购类型（F=外部采购，E=自制生产；冗余：来自 TaktEcDetail.EcNewPurchaseType；本表仅 F）"),
            // entity.ecsmt.ecnewpurchasetype
            new TranslationSeedItem("entity.ecsmt.ecnewpurchasetype", "zh-CN", "新采购类型", "新采购类型（F=外部采购，E=自制生产；冗余：来自 TaktEcDetail.EcNewPurchaseType；本表仅 F）"),
            // entity.ecsmt.ecnewpurchasetype
            new TranslationSeedItem("entity.ecsmt.ecnewpurchasetype", "zh-HK", "新采购类型_hk", "新采购类型（F=外部采购，E=自制生产；冗余：来自 TaktEcDetail.EcNewPurchaseType；本表仅 F）"),

            // entity.ecsmt.ecnewwarehouse
            new TranslationSeedItem("entity.ecsmt.ecnewwarehouse", "en-US", "新品仓库_us", "新品仓库（选项 TaktWarehouses/options；DictValue=WarehouseCode；冗余：来自 TaktEcDetail.EcNewWarehouse；本表仅 C003；执行行按设变单号+机种编码去重）"),
            // entity.ecsmt.ecnewwarehouse
            new TranslationSeedItem("entity.ecsmt.ecnewwarehouse", "ja-JP", "新品仓库_jp", "新品仓库（选项 TaktWarehouses/options；DictValue=WarehouseCode；冗余：来自 TaktEcDetail.EcNewWarehouse；本表仅 C003；执行行按设变单号+机种编码去重）"),
            // entity.ecsmt.ecnewwarehouse
            new TranslationSeedItem("entity.ecsmt.ecnewwarehouse", "zh-CN", "新品仓库", "新品仓库（选项 TaktWarehouses/options；DictValue=WarehouseCode；冗余：来自 TaktEcDetail.EcNewWarehouse；本表仅 C003；执行行按设变单号+机种编码去重）"),
            // entity.ecsmt.ecnewwarehouse
            new TranslationSeedItem("entity.ecsmt.ecnewwarehouse", "zh-HK", "新品仓库_hk", "新品仓库（选项 TaktWarehouses/options；DictValue=WarehouseCode；冗余：来自 TaktEcDetail.EcNewWarehouse；本表仅 C003；执行行按设变单号+机种编码去重）"),

            // entity.ecsmt.deptcode
            new TranslationSeedItem("entity.ecsmt.deptcode", "en-US", "部门编码_us", "部门编码（TaktDept.DeptCode；本表固定制造2课-物料 D0625）"),
            // entity.ecsmt.deptcode
            new TranslationSeedItem("entity.ecsmt.deptcode", "ja-JP", "部门编码_jp", "部门编码（TaktDept.DeptCode；本表固定制造2课-物料 D0625）"),
            // entity.ecsmt.deptcode
            new TranslationSeedItem("entity.ecsmt.deptcode", "zh-CN", "部门编码", "部门编码（TaktDept.DeptCode；本表固定制造2课-物料 D0625）"),
            // entity.ecsmt.deptcode
            new TranslationSeedItem("entity.ecsmt.deptcode", "zh-HK", "部门编码_hk", "部门编码（TaktDept.DeptCode；本表固定制造2课-物料 D0625）"),

            // entity.ecsmt.deptname
            new TranslationSeedItem("entity.ecsmt.deptname", "en-US", "部门名称_us", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),
            // entity.ecsmt.deptname
            new TranslationSeedItem("entity.ecsmt.deptname", "ja-JP", "部门名称_jp", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),
            // entity.ecsmt.deptname
            new TranslationSeedItem("entity.ecsmt.deptname", "zh-CN", "部门名称", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),
            // entity.ecsmt.deptname
            new TranslationSeedItem("entity.ecsmt.deptname", "zh-HK", "部门名称_hk", "部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）"),

            // entity.ecsmt.isimplemented
            new TranslationSeedItem("entity.ecsmt.isimplemented", "en-US", "实施_us", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.ecsmt.isimplemented
            new TranslationSeedItem("entity.ecsmt.isimplemented", "ja-JP", "实施_jp", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.ecsmt.isimplemented
            new TranslationSeedItem("entity.ecsmt.isimplemented", "zh-CN", "实施", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.ecsmt.isimplemented
            new TranslationSeedItem("entity.ecsmt.isimplemented", "zh-HK", "实施_hk", "是否实施（0=否 1=是，字典 sys_yes_no）"),

            // entity.ecsmt.execcontent
            new TranslationSeedItem("entity.ecsmt.execcontent", "en-US", "执行内容_us", "执行内容（各部门通用）"),
            // entity.ecsmt.execcontent
            new TranslationSeedItem("entity.ecsmt.execcontent", "ja-JP", "执行内容_jp", "执行内容（各部门通用）"),
            // entity.ecsmt.execcontent
            new TranslationSeedItem("entity.ecsmt.execcontent", "zh-CN", "执行内容", "执行内容（各部门通用）"),
            // entity.ecsmt.execcontent
            new TranslationSeedItem("entity.ecsmt.execcontent", "zh-HK", "执行内容_hk", "执行内容（各部门通用）"),

            // entity.ecsmt.outboundbatch
            new TranslationSeedItem("entity.ecsmt.outboundbatch", "en-US", "出库批次_us", "出库批次"),
            // entity.ecsmt.outboundbatch
            new TranslationSeedItem("entity.ecsmt.outboundbatch", "ja-JP", "出库批次_jp", "出库批次"),
            // entity.ecsmt.outboundbatch
            new TranslationSeedItem("entity.ecsmt.outboundbatch", "zh-CN", "出库批次", "出库批次"),
            // entity.ecsmt.outboundbatch
            new TranslationSeedItem("entity.ecsmt.outboundbatch", "zh-HK", "出库批次_hk", "出库批次"),

            // entity.ecsmt.outbounddate
            new TranslationSeedItem("entity.ecsmt.outbounddate", "en-US", "出库日期_us", "出库日期"),
            // entity.ecsmt.outbounddate
            new TranslationSeedItem("entity.ecsmt.outbounddate", "ja-JP", "出库日期_jp", "出库日期"),
            // entity.ecsmt.outbounddate
            new TranslationSeedItem("entity.ecsmt.outbounddate", "zh-CN", "出库日期", "出库日期"),
            // entity.ecsmt.outbounddate
            new TranslationSeedItem("entity.ecsmt.outbounddate", "zh-HK", "出库日期_hk", "出库日期"),

            // entity.ecsmt.isobsolete
            new TranslationSeedItem("entity.ecsmt.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecsmt.isobsolete
            new TranslationSeedItem("entity.ecsmt.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecsmt.isobsolete
            new TranslationSeedItem("entity.ecsmt.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecsmt.isobsolete
            new TranslationSeedItem("entity.ecsmt.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
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
