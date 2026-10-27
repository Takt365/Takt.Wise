// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcAttachmentI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEcAttachment 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktEcAttachment 实体国际化翻译种子（键前缀 entity.ecattachment.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEcAttachmentI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEcAttachment 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 ecattachment 实体翻译...", tenantCode);

        foreach (var item in GetEcAttachmentTranslations())
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

        TaktLogger.Information("TaktEcAttachment 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEcAttachment 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.ecattachment._self / entity.ecattachment.{{field}}；ResourceGroup=EngineeringChange；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEcAttachmentTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.ecattachment._self
            new TranslationSeedItem("entity.ecattachment._self", "en-US", "Ec Attachment Information_us", "实体名称"),
            // entity.ecattachment._self
            new TranslationSeedItem("entity.ecattachment._self", "ja-JP", "设变附件信息_jp", "实体名称"),
            // entity.ecattachment._self
            new TranslationSeedItem("entity.ecattachment._self", "zh-CN", "设变附件信息", "实体名称"),
            // entity.ecattachment._self
            new TranslationSeedItem("entity.ecattachment._self", "zh-HK", "设变附件信息_hk", "实体名称"),

            // entity.ecattachment.eccode
            new TranslationSeedItem("entity.ecattachment.eccode", "en-US", "设变单号_us", "设变单号（冗余字段,便于查询）"),
            // entity.ecattachment.eccode
            new TranslationSeedItem("entity.ecattachment.eccode", "ja-JP", "设变单号_jp", "设变单号（冗余字段,便于查询）"),
            // entity.ecattachment.eccode
            new TranslationSeedItem("entity.ecattachment.eccode", "zh-CN", "设变单号", "设变单号（冗余字段,便于查询）"),
            // entity.ecattachment.eccode
            new TranslationSeedItem("entity.ecattachment.eccode", "zh-HK", "设变单号_hk", "设变单号（冗余字段,便于查询）"),

            // entity.ecattachment.linenumber
            new TranslationSeedItem("entity.ecattachment.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10；与租户、EcTeId、DocCode 组成唯一键）"),
            // entity.ecattachment.linenumber
            new TranslationSeedItem("entity.ecattachment.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10；与租户、EcTeId、DocCode 组成唯一键）"),
            // entity.ecattachment.linenumber
            new TranslationSeedItem("entity.ecattachment.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10；与租户、EcTeId、DocCode 组成唯一键）"),
            // entity.ecattachment.linenumber
            new TranslationSeedItem("entity.ecattachment.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10；与租户、EcTeId、DocCode 组成唯一键）"),

            // entity.ecattachment.attachmenttype
            new TranslationSeedItem("entity.ecattachment.attachmenttype", "en-US", "文件类别_us", "文件类别（字典 logistics_manufacturing_ec_attachment_type；TL=联络，EPP=EPP，FPP=FPP，EL=外部联络，TCJ=TCJ，源PDF=源PDF，EC=EC）"),
            // entity.ecattachment.attachmenttype
            new TranslationSeedItem("entity.ecattachment.attachmenttype", "ja-JP", "文件类别_jp", "文件类别（字典 logistics_manufacturing_ec_attachment_type；TL=联络，EPP=EPP，FPP=FPP，EL=外部联络，TCJ=TCJ，源PDF=源PDF，EC=EC）"),
            // entity.ecattachment.attachmenttype
            new TranslationSeedItem("entity.ecattachment.attachmenttype", "zh-CN", "文件类别", "文件类别（字典 logistics_manufacturing_ec_attachment_type；TL=联络，EPP=EPP，FPP=FPP，EL=外部联络，TCJ=TCJ，源PDF=源PDF，EC=EC）"),
            // entity.ecattachment.attachmenttype
            new TranslationSeedItem("entity.ecattachment.attachmenttype", "zh-HK", "文件类别_hk", "文件类别（字典 logistics_manufacturing_ec_attachment_type；TL=联络，EPP=EPP，FPP=FPP，EL=外部联络，TCJ=TCJ，源PDF=源PDF，EC=EC）"),

            // entity.ecattachment.doccode
            new TranslationSeedItem("entity.ecattachment.doccode", "en-US", "文件编码_us", "文件编码（按 AttachmentType：EC=与设变单号一致；EPP/FPP=P-四位数字；TL=DTS-四位数字；TCJ/EL=四位-四位数字；与租户、EcTeId、LineNumber 组成唯一键；前端表单选择编码规则后自动通过 TaktNumbering 文件编码规则生成并展示，非手输；单据类型菜单：设变附件）"),
            // entity.ecattachment.doccode
            new TranslationSeedItem("entity.ecattachment.doccode", "ja-JP", "文件编码_jp", "文件编码（按 AttachmentType：EC=与设变单号一致；EPP/FPP=P-四位数字；TL=DTS-四位数字；TCJ/EL=四位-四位数字；与租户、EcTeId、LineNumber 组成唯一键；前端表单选择编码规则后自动通过 TaktNumbering 文件编码规则生成并展示，非手输；单据类型菜单：设变附件）"),
            // entity.ecattachment.doccode
            new TranslationSeedItem("entity.ecattachment.doccode", "zh-CN", "文件编码", "文件编码（按 AttachmentType：EC=与设变单号一致；EPP/FPP=P-四位数字；TL=DTS-四位数字；TCJ/EL=四位-四位数字；与租户、EcTeId、LineNumber 组成唯一键；前端表单选择编码规则后自动通过 TaktNumbering 文件编码规则生成并展示，非手输；单据类型菜单：设变附件）"),
            // entity.ecattachment.doccode
            new TranslationSeedItem("entity.ecattachment.doccode", "zh-HK", "文件编码_hk", "文件编码（按 AttachmentType：EC=与设变单号一致；EPP/FPP=P-四位数字；TL=DTS-四位数字；TCJ/EL=四位-四位数字；与租户、EcTeId、LineNumber 组成唯一键；前端表单选择编码规则后自动通过 TaktNumbering 文件编码规则生成并展示，非手输；单据类型菜单：设变附件）"),

            // entity.ecattachment.filename
            new TranslationSeedItem("entity.ecattachment.filename", "en-US", "文件名称_us", "文件名称（上传后强制等于文件编码 DocCode + 原扩展名，与源文件名无关；含扩展名故 Length=200）"),
            // entity.ecattachment.filename
            new TranslationSeedItem("entity.ecattachment.filename", "ja-JP", "文件名称_jp", "文件名称（上传后强制等于文件编码 DocCode + 原扩展名，与源文件名无关；含扩展名故 Length=200）"),
            // entity.ecattachment.filename
            new TranslationSeedItem("entity.ecattachment.filename", "zh-CN", "文件名称", "文件名称（上传后强制等于文件编码 DocCode + 原扩展名，与源文件名无关；含扩展名故 Length=200）"),
            // entity.ecattachment.filename
            new TranslationSeedItem("entity.ecattachment.filename", "zh-HK", "文件名称_hk", "文件名称（上传后强制等于文件编码 DocCode + 原扩展名，与源文件名无关；含扩展名故 Length=200）"),

            // entity.ecattachment.accessurl
            new TranslationSeedItem("entity.ecattachment.accessurl", "en-US", "访问地址_us", "访问地址（URL）"),
            // entity.ecattachment.accessurl
            new TranslationSeedItem("entity.ecattachment.accessurl", "ja-JP", "访问地址_jp", "访问地址（URL）"),
            // entity.ecattachment.accessurl
            new TranslationSeedItem("entity.ecattachment.accessurl", "zh-CN", "访问地址", "访问地址（URL）"),
            // entity.ecattachment.accessurl
            new TranslationSeedItem("entity.ecattachment.accessurl", "zh-HK", "访问地址_hk", "访问地址（URL）"),

            // entity.ecattachment.ecteid
            new TranslationSeedItem("entity.ecattachment.ecteid", "en-US", "技术课主表ID_us", "技术课主表 ID（TaktEcTe 主键；与租户、DocCode、LineNumber 组成唯一键）"),
            // entity.ecattachment.ecteid
            new TranslationSeedItem("entity.ecattachment.ecteid", "ja-JP", "技术课主表ID_jp", "技术课主表 ID（TaktEcTe 主键；与租户、DocCode、LineNumber 组成唯一键）"),
            // entity.ecattachment.ecteid
            new TranslationSeedItem("entity.ecattachment.ecteid", "zh-CN", "技术课主表ID", "技术课主表 ID（TaktEcTe 主键；与租户、DocCode、LineNumber 组成唯一键）"),
            // entity.ecattachment.ecteid
            new TranslationSeedItem("entity.ecattachment.ecteid", "zh-HK", "技术课主表ID_hk", "技术课主表 ID（TaktEcTe 主键；与租户、DocCode、LineNumber 组成唯一键）"),

            // entity.ecattachment.isobsolete
            new TranslationSeedItem("entity.ecattachment.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecattachment.isobsolete
            new TranslationSeedItem("entity.ecattachment.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecattachment.isobsolete
            new TranslationSeedItem("entity.ecattachment.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecattachment.isobsolete
            new TranslationSeedItem("entity.ecattachment.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.ecattachment.ecte
            new TranslationSeedItem("entity.ecattachment.ecte", "en-US", "设变主表_us", "设变主表（多对一）"),
            // entity.ecattachment.ecte
            new TranslationSeedItem("entity.ecattachment.ecte", "ja-JP", "设变主表_jp", "设变主表（多对一）"),
            // entity.ecattachment.ecte
            new TranslationSeedItem("entity.ecattachment.ecte", "zh-CN", "设变主表", "设变主表（多对一）"),
            // entity.ecattachment.ecte
            new TranslationSeedItem("entity.ecattachment.ecte", "zh-HK", "设变主表_hk", "设变主表（多对一）"),
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
