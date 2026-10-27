// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcTeI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEcTe 实体字段国际化种子（已对齐前端 locales：src/locales/logistics/manufacturing/engineering-change/ec-te）
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
/// TaktEcTe 实体国际化翻译种子（键前缀 entity.ecte.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEcTeI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEcTe 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 ecte 实体翻译...", tenantCode);

        foreach (var item in GetEcTeTranslations())
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

        TaktLogger.Information("TaktEcTe 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEcTe 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.ecte._self / entity.ecte.{{field}}；ResourceGroup=EngineeringChange；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEcTeTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.ecte._self
            new TranslationSeedItem("entity.ecte._self", "en-US", "Ec Te Information_us", "实体名称"),
            // entity.ecte._self
            new TranslationSeedItem("entity.ecte._self", "ja-JP", "设变技术课主表信息_jp", "实体名称"),
            // entity.ecte._self
            new TranslationSeedItem("entity.ecte._self", "zh-CN", "设变技术课主表信息", "实体名称"),
            // entity.ecte._self
            new TranslationSeedItem("entity.ecte._self", "zh-HK", "设变技术课主表信息_hk", "实体名称"),

            // entity.ecte.eccode
            new TranslationSeedItem("entity.ecte.eccode", "en-US", "设变单号_us", "设变单号（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 设变单号规则生成并展示，非手输；单据类型菜单：技术部门）"),
            // entity.ecte.eccode
            new TranslationSeedItem("entity.ecte.eccode", "ja-JP", "设变单号_jp", "设变单号（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 设变单号规则生成并展示，非手输；单据类型菜单：技术部门）"),
            // entity.ecte.eccode
            new TranslationSeedItem("entity.ecte.eccode", "zh-CN", "设变单号", "设变单号（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 设变单号规则生成并展示，非手输；单据类型菜单：技术部门）"),
            // entity.ecte.eccode
            new TranslationSeedItem("entity.ecte.eccode", "zh-HK", "设变单号_hk", "设变单号（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 设变单号规则生成并展示，非手输；单据类型菜单：技术部门）"),

            // entity.ecte.ecissuedate
            new TranslationSeedItem("entity.ecte.ecissuedate", "en-US", "发行日期_us", "发行日期"),
            // entity.ecte.ecissuedate
            new TranslationSeedItem("entity.ecte.ecissuedate", "ja-JP", "发行日期_jp", "发行日期"),
            // entity.ecte.ecissuedate
            new TranslationSeedItem("entity.ecte.ecissuedate", "zh-CN", "发行日期", "发行日期"),
            // entity.ecte.ecissuedate
            new TranslationSeedItem("entity.ecte.ecissuedate", "zh-HK", "发行日期_hk", "发行日期"),

            // entity.ecte.changestatus
            new TranslationSeedItem("entity.ecte.changestatus", "en-US", "变更状态_us", "变更状态（字典 logistics_manufacturing_ec_status；1=工作的，2=取消的，3=发行的，4=P.P中变更的，5=固定的，6=挂起的，7=拒绝的；默认 5=固定的，表示设变将被实施）"),
            // entity.ecte.changestatus
            new TranslationSeedItem("entity.ecte.changestatus", "ja-JP", "变更状态_jp", "变更状态（字典 logistics_manufacturing_ec_status；1=工作的，2=取消的，3=发行的，4=P.P中变更的，5=固定的，6=挂起的，7=拒绝的；默认 5=固定的，表示设变将被实施）"),
            // entity.ecte.changestatus
            new TranslationSeedItem("entity.ecte.changestatus", "zh-CN", "变更状态", "变更状态（字典 logistics_manufacturing_ec_status；1=工作的，2=取消的，3=发行的，4=P.P中变更的，5=固定的，6=挂起的，7=拒绝的；默认 5=固定的，表示设变将被实施）"),
            // entity.ecte.changestatus
            new TranslationSeedItem("entity.ecte.changestatus", "zh-HK", "变更状态_hk", "变更状态（字典 logistics_manufacturing_ec_status；1=工作的，2=取消的，3=发行的，4=P.P中变更的，5=固定的，6=挂起的，7=拒绝的；默认 5=固定的，表示设变将被实施）"),

            // entity.ecte.ectitle
            new TranslationSeedItem("entity.ecte.ectitle", "en-US", "设变标题_us", "设变标题"),
            // entity.ecte.ectitle
            new TranslationSeedItem("entity.ecte.ectitle", "ja-JP", "设变标题_jp", "设变标题"),
            // entity.ecte.ectitle
            new TranslationSeedItem("entity.ecte.ectitle", "zh-CN", "设变标题", "设变标题"),
            // entity.ecte.ectitle
            new TranslationSeedItem("entity.ecte.ectitle", "zh-HK", "设变标题_hk", "设变标题"),

            // entity.ecte.eccontent
            new TranslationSeedItem("entity.ecte.eccontent", "en-US", "设变内容_us", "设变内容（富文本 HTML）"),
            // entity.ecte.eccontent
            new TranslationSeedItem("entity.ecte.eccontent", "ja-JP", "设变内容_jp", "设变内容（富文本 HTML）"),
            // entity.ecte.eccontent
            new TranslationSeedItem("entity.ecte.eccontent", "zh-CN", "设变内容", "设变内容（富文本 HTML）"),
            // entity.ecte.eccontent
            new TranslationSeedItem("entity.ecte.eccontent", "zh-HK", "设变内容_hk", "设变内容（富文本 HTML）"),

            // entity.ecte.ecleader
            new TranslationSeedItem("entity.ecte.ecleader", "en-US", "设变担当_us", "设变担当（选项 TaktEcGroups/options?ecGroupCategory=1；DictValue=EcGroupCode，DictLabel=EcGroupName）"),
            // entity.ecte.ecleader
            new TranslationSeedItem("entity.ecte.ecleader", "ja-JP", "设变担当_jp", "设变担当（选项 TaktEcGroups/options?ecGroupCategory=1；DictValue=EcGroupCode，DictLabel=EcGroupName）"),
            // entity.ecte.ecleader
            new TranslationSeedItem("entity.ecte.ecleader", "zh-CN", "设变担当", "设变担当（选项 TaktEcGroups/options?ecGroupCategory=1；DictValue=EcGroupCode，DictLabel=EcGroupName）"),
            // entity.ecte.ecleader
            new TranslationSeedItem("entity.ecte.ecleader", "zh-HK", "设变担当_hk", "设变担当（选项 TaktEcGroups/options?ecGroupCategory=1；DictValue=EcGroupCode，DictLabel=EcGroupName）"),

            // entity.ecte.eclossamount
            new TranslationSeedItem("entity.ecte.eclossamount", "en-US", "损失金额_us", "损失金额"),
            // entity.ecte.eclossamount
            new TranslationSeedItem("entity.ecte.eclossamount", "ja-JP", "损失金额_jp", "损失金额"),
            // entity.ecte.eclossamount
            new TranslationSeedItem("entity.ecte.eclossamount", "zh-CN", "损失金额", "损失金额"),
            // entity.ecte.eclossamount
            new TranslationSeedItem("entity.ecte.eclossamount", "zh-HK", "损失金额_hk", "损失金额"),

            // entity.ecte.ecimplscope
            new TranslationSeedItem("entity.ecte.ecimplscope", "en-US", "实施范围_us", "实施范围（字典 logistics_manufacturing_ec_impl_scope_category；1=全仕向，2=部管，3=内部，4=技术）"),
            // entity.ecte.ecimplscope
            new TranslationSeedItem("entity.ecte.ecimplscope", "ja-JP", "实施范围_jp", "实施范围（字典 logistics_manufacturing_ec_impl_scope_category；1=全仕向，2=部管，3=内部，4=技术）"),
            // entity.ecte.ecimplscope
            new TranslationSeedItem("entity.ecte.ecimplscope", "zh-CN", "实施范围", "实施范围（字典 logistics_manufacturing_ec_impl_scope_category；1=全仕向，2=部管，3=内部，4=技术）"),
            // entity.ecte.ecimplscope
            new TranslationSeedItem("entity.ecte.ecimplscope", "zh-HK", "实施范围_hk", "实施范围（字典 logistics_manufacturing_ec_impl_scope_category；1=全仕向，2=部管，3=内部，4=技术）"),

            // entity.ecte.eccontrolscope
            new TranslationSeedItem("entity.ecte.eccontrolscope", "en-US", "控制范围_us", "控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品，2=按机种，3=按设变，4=不管理），只记录生管/制二/制一/品管；全仕向时生管与此相同。采购/受检/部管/SMT 固定按设变，制技固定按机种"),
            // entity.ecte.eccontrolscope
            new TranslationSeedItem("entity.ecte.eccontrolscope", "ja-JP", "控制范围_jp", "控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品，2=按机种，3=按设变，4=不管理），只记录生管/制二/制一/品管；全仕向时生管与此相同。采购/受检/部管/SMT 固定按设变，制技固定按机种"),
            // entity.ecte.eccontrolscope
            new TranslationSeedItem("entity.ecte.eccontrolscope", "zh-CN", "控制范围", "控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品，2=按机种，3=按设变，4=不管理），只记录生管/制二/制一/品管；全仕向时生管与此相同。采购/受检/部管/SMT 固定按设变，制技固定按机种"),
            // entity.ecte.eccontrolscope
            new TranslationSeedItem("entity.ecte.eccontrolscope", "zh-HK", "控制范围_hk", "控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品，2=按机种，3=按设变，4=不管理），只记录生管/制二/制一/品管；全仕向时生管与此相同。采购/受检/部管/SMT 固定按设变，制技固定按机种"),

            // entity.ecte.ecentrydate
            new TranslationSeedItem("entity.ecte.ecentrydate", "en-US", "录入日期_us", "录入日期"),
            // entity.ecte.ecentrydate
            new TranslationSeedItem("entity.ecte.ecentrydate", "ja-JP", "录入日期_jp", "录入日期"),
            // entity.ecte.ecentrydate
            new TranslationSeedItem("entity.ecte.ecentrydate", "zh-CN", "录入日期", "录入日期"),
            // entity.ecte.ecentrydate
            new TranslationSeedItem("entity.ecte.ecentrydate", "zh-HK", "录入日期_hk", "录入日期"),

            // entity.ecte.ecstatus
            new TranslationSeedItem("entity.ecte.ecstatus", "en-US", "设变状态_us", "设变状态（字典 logistics_manufacturing_ec_te_status；1=发行，2=执行中，3=完成；由各部门执行表自动回写：任一有输入→2，全部填写→3，否则→1）"),
            // entity.ecte.ecstatus
            new TranslationSeedItem("entity.ecte.ecstatus", "ja-JP", "设变状态_jp", "设变状态（字典 logistics_manufacturing_ec_te_status；1=发行，2=执行中，3=完成；由各部门执行表自动回写：任一有输入→2，全部填写→3，否则→1）"),
            // entity.ecte.ecstatus
            new TranslationSeedItem("entity.ecte.ecstatus", "zh-CN", "设变状态", "设变状态（字典 logistics_manufacturing_ec_te_status；1=发行，2=执行中，3=完成；由各部门执行表自动回写：任一有输入→2，全部填写→3，否则→1）"),
            // entity.ecte.ecstatus
            new TranslationSeedItem("entity.ecte.ecstatus", "zh-HK", "设变状态_hk", "设变状态（字典 logistics_manufacturing_ec_te_status；1=发行，2=执行中，3=完成；由各部门执行表自动回写：任一有输入→2，全部填写→3，否则→1）"),

            // entity.ecte.ecdetails
            new TranslationSeedItem("entity.ecte.ecdetails", "en-US", "设变明细列表_us", "设变明细列表（技术阶段一：③，BOM/料号变更行）"),
            // entity.ecte.ecdetails
            new TranslationSeedItem("entity.ecte.ecdetails", "ja-JP", "设变明细列表_jp", "设变明细列表（技术阶段一：③，BOM/料号变更行）"),
            // entity.ecte.ecdetails
            new TranslationSeedItem("entity.ecte.ecdetails", "zh-CN", "设变明细列表", "设变明细列表（技术阶段一：③，BOM/料号变更行）"),
            // entity.ecte.ecdetails
            new TranslationSeedItem("entity.ecte.ecdetails", "zh-HK", "设变明细列表_hk", "设变明细列表（技术阶段一：③，BOM/料号变更行）"),

            // entity.ecte.attachments
            new TranslationSeedItem("entity.ecte.attachments", "en-US", "设变附件列表_us", "设变附件列表（技术阶段一：②，联络/EPP/FPP 等文档）"),
            // entity.ecte.attachments
            new TranslationSeedItem("entity.ecte.attachments", "ja-JP", "设变附件列表_jp", "设变附件列表（技术阶段一：②，联络/EPP/FPP 等文档）"),
            // entity.ecte.attachments
            new TranslationSeedItem("entity.ecte.attachments", "zh-CN", "设变附件列表", "设变附件列表（技术阶段一：②，联络/EPP/FPP 等文档）"),
            // entity.ecte.attachments
            new TranslationSeedItem("entity.ecte.attachments", "zh-HK", "设变附件列表_hk", "设变附件列表（技术阶段一：②，联络/EPP/FPP 等文档）"),

            // entity.ecte.notifications
            new TranslationSeedItem("entity.ecte.notifications", "en-US", "设变通知列表_us", "设变通知列表（技术阶段一：④，发行通知至各部门）"),
            // entity.ecte.notifications
            new TranslationSeedItem("entity.ecte.notifications", "ja-JP", "设变通知列表_jp", "设变通知列表（技术阶段一：④，发行通知至各部门）"),
            // entity.ecte.notifications
            new TranslationSeedItem("entity.ecte.notifications", "zh-CN", "设变通知列表", "设变通知列表（技术阶段一：④，发行通知至各部门）"),
            // entity.ecte.notifications
            new TranslationSeedItem("entity.ecte.notifications", "zh-HK", "设变通知列表_hk", "设变通知列表（技术阶段一：④，发行通知至各部门）"),
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
