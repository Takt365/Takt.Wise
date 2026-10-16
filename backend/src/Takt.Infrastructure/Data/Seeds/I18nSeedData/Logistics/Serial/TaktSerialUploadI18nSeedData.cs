// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Serial
// 文件名称：TaktSerialUploadI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSerialUpload 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Serial;

/// <summary>
/// TaktSerialUpload 实体国际化翻译种子（键前缀 entity.serialupload.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSerialUploadI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSerialUpload 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 serialupload 实体翻译...", tenantCode);

        foreach (var item in GetSerialUploadTranslations())
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

        TaktLogger.Information("TaktSerialUpload 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSerialUpload 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.serialupload._self / entity.serialupload.{{field}}；ResourceGroup=Serial；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSerialUploadTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.serialupload._self
            new TranslationSeedItem("entity.serialupload._self", "en-US", "Serial Upload Information_us", "实体名称"),
            // entity.serialupload._self
            new TranslationSeedItem("entity.serialupload._self", "ja-JP", "序列号上传信息_jp", "实体名称"),
            // entity.serialupload._self
            new TranslationSeedItem("entity.serialupload._self", "zh-CN", "序列号上传信息", "实体名称"),
            // entity.serialupload._self
            new TranslationSeedItem("entity.serialupload._self", "zh-HK", "序列号上传信息_hk", "实体名称"),

            // entity.serialupload.outbounddate
            new TranslationSeedItem("entity.serialupload.outbounddate", "en-US", "出库日期_us", "出库日期"),
            // entity.serialupload.outbounddate
            new TranslationSeedItem("entity.serialupload.outbounddate", "ja-JP", "出库日期_jp", "出库日期"),
            // entity.serialupload.outbounddate
            new TranslationSeedItem("entity.serialupload.outbounddate", "zh-CN", "出库日期", "出库日期"),
            // entity.serialupload.outbounddate
            new TranslationSeedItem("entity.serialupload.outbounddate", "zh-HK", "出库日期_hk", "出库日期"),

            // entity.serialupload.shippinginvoicecode
            new TranslationSeedItem("entity.serialupload.shippinginvoicecode", "en-US", "发货单号_us", "发货单号（固定 9 位；前端表单选择编码规则后自动通过 TaktNumbering 发货单号规则生成并展示，非手输；单据类型菜单：序列号上传）"),
            // entity.serialupload.shippinginvoicecode
            new TranslationSeedItem("entity.serialupload.shippinginvoicecode", "ja-JP", "发货单号_jp", "发货单号（固定 9 位；前端表单选择编码规则后自动通过 TaktNumbering 发货单号规则生成并展示，非手输；单据类型菜单：序列号上传）"),
            // entity.serialupload.shippinginvoicecode
            new TranslationSeedItem("entity.serialupload.shippinginvoicecode", "zh-CN", "发货单号", "发货单号（固定 9 位；前端表单选择编码规则后自动通过 TaktNumbering 发货单号规则生成并展示，非手输；单据类型菜单：序列号上传）"),
            // entity.serialupload.shippinginvoicecode
            new TranslationSeedItem("entity.serialupload.shippinginvoicecode", "zh-HK", "发货单号_hk", "发货单号（固定 9 位；前端表单选择编码规则后自动通过 TaktNumbering 发货单号规则生成并展示，非手输；单据类型菜单：序列号上传）"),

            // entity.serialupload.sequencecode
            new TranslationSeedItem("entity.serialupload.sequencecode", "en-US", "序号_us", "序号（同一租户、公司、工厂+发货单号内唯一）"),
            // entity.serialupload.sequencecode
            new TranslationSeedItem("entity.serialupload.sequencecode", "ja-JP", "序号_jp", "序号（同一租户、公司、工厂+发货单号内唯一）"),
            // entity.serialupload.sequencecode
            new TranslationSeedItem("entity.serialupload.sequencecode", "zh-CN", "序号", "序号（同一租户、公司、工厂+发货单号内唯一）"),
            // entity.serialupload.sequencecode
            new TranslationSeedItem("entity.serialupload.sequencecode", "zh-HK", "序号_hk", "序号（同一租户、公司、工厂+发货单号内唯一）"),

            // entity.serialupload.materialcode
            new TranslationSeedItem("entity.serialupload.materialcode", "en-US", "产品物料_us", "产品物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode；最长 20）"),
            // entity.serialupload.materialcode
            new TranslationSeedItem("entity.serialupload.materialcode", "ja-JP", "产品物料_jp", "产品物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode；最长 20）"),
            // entity.serialupload.materialcode
            new TranslationSeedItem("entity.serialupload.materialcode", "zh-CN", "产品物料", "产品物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode；最长 20）"),
            // entity.serialupload.materialcode
            new TranslationSeedItem("entity.serialupload.materialcode", "zh-HK", "产品物料_hk", "产品物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode；最长 20）"),

            // entity.serialupload.totalquantity
            new TranslationSeedItem("entity.serialupload.totalquantity", "en-US", "合计数量_us", "合计数量"),
            // entity.serialupload.totalquantity
            new TranslationSeedItem("entity.serialupload.totalquantity", "ja-JP", "合计数量_jp", "合计数量"),
            // entity.serialupload.totalquantity
            new TranslationSeedItem("entity.serialupload.totalquantity", "zh-CN", "合计数量", "合计数量"),
            // entity.serialupload.totalquantity
            new TranslationSeedItem("entity.serialupload.totalquantity", "zh-HK", "合计数量_hk", "合计数量"),

            // entity.serialupload.serialcode
            new TranslationSeedItem("entity.serialupload.serialcode", "en-US", "序列号_us", "序列号（固定 7 位）"),
            // entity.serialupload.serialcode
            new TranslationSeedItem("entity.serialupload.serialcode", "ja-JP", "序列号_jp", "序列号（固定 7 位）"),
            // entity.serialupload.serialcode
            new TranslationSeedItem("entity.serialupload.serialcode", "zh-CN", "序列号", "序列号（固定 7 位）"),
            // entity.serialupload.serialcode
            new TranslationSeedItem("entity.serialupload.serialcode", "zh-HK", "序列号_hk", "序列号（固定 7 位）"),

            // entity.serialupload.packingquantity
            new TranslationSeedItem("entity.serialupload.packingquantity", "en-US", "装箱数量_us", "装箱数量"),
            // entity.serialupload.packingquantity
            new TranslationSeedItem("entity.serialupload.packingquantity", "ja-JP", "装箱数量_jp", "装箱数量"),
            // entity.serialupload.packingquantity
            new TranslationSeedItem("entity.serialupload.packingquantity", "zh-CN", "装箱数量", "装箱数量"),
            // entity.serialupload.packingquantity
            new TranslationSeedItem("entity.serialupload.packingquantity", "zh-HK", "装箱数量_hk", "装箱数量"),

            // entity.serialupload.transportmode
            new TranslationSeedItem("entity.serialupload.transportmode", "en-US", "运输方式_us", "运输方式（最长 20）"),
            // entity.serialupload.transportmode
            new TranslationSeedItem("entity.serialupload.transportmode", "ja-JP", "运输方式_jp", "运输方式（最长 20）"),
            // entity.serialupload.transportmode
            new TranslationSeedItem("entity.serialupload.transportmode", "zh-CN", "运输方式", "运输方式（最长 20）"),
            // entity.serialupload.transportmode
            new TranslationSeedItem("entity.serialupload.transportmode", "zh-HK", "运输方式_hk", "运输方式（最长 20）"),

            // entity.serialupload.materialtext
            new TranslationSeedItem("entity.serialupload.materialtext", "en-US", "物料描述_us", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.serialupload.materialtext
            new TranslationSeedItem("entity.serialupload.materialtext", "ja-JP", "物料描述_jp", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.serialupload.materialtext
            new TranslationSeedItem("entity.serialupload.materialtext", "zh-CN", "物料描述", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.serialupload.materialtext
            new TranslationSeedItem("entity.serialupload.materialtext", "zh-HK", "物料描述_hk", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
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
        translation.ResourceGroup = "Serial";
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
