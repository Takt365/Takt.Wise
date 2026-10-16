// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.HelpDesk
// 文件名称：TaktItAssetI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktItAsset 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.HelpDesk;

/// <summary>
/// TaktItAsset 实体国际化翻译种子（键前缀 entity.itasset.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktItAssetI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktItAsset 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 itasset 实体翻译...", tenantCode);

        foreach (var item in GetItAssetTranslations())
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

        TaktLogger.Information("TaktItAsset 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktItAsset 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.itasset._self / entity.itasset.{{field}}；ResourceGroup=HelpDesk；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetItAssetTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.itasset._self
            new TranslationSeedItem("entity.itasset._self", "en-US", "It Asset Information_us", "实体名称"),
            // entity.itasset._self
            new TranslationSeedItem("entity.itasset._self", "ja-JP", "服务台 IT 设备保修扩展信息_jp", "实体名称"),
            // entity.itasset._self
            new TranslationSeedItem("entity.itasset._self", "zh-CN", "服务台 IT 设备保修扩展信息", "实体名称"),
            // entity.itasset._self
            new TranslationSeedItem("entity.itasset._self", "zh-HK", "服务台 IT 设备保修扩展信息_hk", "实体名称"),

            // entity.itasset.assetcode
            new TranslationSeedItem("entity.itasset.assetcode", "en-US", "资产号码_us", "资产号码（选项 TaktAssets/options；DictValue=AssetCode）"),
            // entity.itasset.assetcode
            new TranslationSeedItem("entity.itasset.assetcode", "ja-JP", "资产号码_jp", "资产号码（选项 TaktAssets/options；DictValue=AssetCode）"),
            // entity.itasset.assetcode
            new TranslationSeedItem("entity.itasset.assetcode", "zh-CN", "资产号码", "资产号码（选项 TaktAssets/options；DictValue=AssetCode）"),
            // entity.itasset.assetcode
            new TranslationSeedItem("entity.itasset.assetcode", "zh-HK", "资产号码_hk", "资产号码（选项 TaktAssets/options；DictValue=AssetCode）"),

            // entity.itasset.warrantytype
            new TranslationSeedItem("entity.itasset.warrantytype", "en-US", "保修类型_us", "保修类型（字典 sys_warranty_type；0=原厂保修 1=延长保修 2=上门保修 3=寄修保修 4=维保合同 5=付费保养）"),
            // entity.itasset.warrantytype
            new TranslationSeedItem("entity.itasset.warrantytype", "ja-JP", "保修类型_jp", "保修类型（字典 sys_warranty_type；0=原厂保修 1=延长保修 2=上门保修 3=寄修保修 4=维保合同 5=付费保养）"),
            // entity.itasset.warrantytype
            new TranslationSeedItem("entity.itasset.warrantytype", "zh-CN", "保修类型", "保修类型（字典 sys_warranty_type；0=原厂保修 1=延长保修 2=上门保修 3=寄修保修 4=维保合同 5=付费保养）"),
            // entity.itasset.warrantytype
            new TranslationSeedItem("entity.itasset.warrantytype", "zh-HK", "保修类型_hk", "保修类型（字典 sys_warranty_type；0=原厂保修 1=延长保修 2=上门保修 3=寄修保修 4=维保合同 5=付费保养）"),

            // entity.itasset.warrantystartdate
            new TranslationSeedItem("entity.itasset.warrantystartdate", "en-US", "保修开始日期_us", "保修开始日期"),
            // entity.itasset.warrantystartdate
            new TranslationSeedItem("entity.itasset.warrantystartdate", "ja-JP", "保修开始日期_jp", "保修开始日期"),
            // entity.itasset.warrantystartdate
            new TranslationSeedItem("entity.itasset.warrantystartdate", "zh-CN", "保修开始日期", "保修开始日期"),
            // entity.itasset.warrantystartdate
            new TranslationSeedItem("entity.itasset.warrantystartdate", "zh-HK", "保修开始日期_hk", "保修开始日期"),

            // entity.itasset.warrantyexpirydate
            new TranslationSeedItem("entity.itasset.warrantyexpirydate", "en-US", "保修到期日_us", "保修到期日"),
            // entity.itasset.warrantyexpirydate
            new TranslationSeedItem("entity.itasset.warrantyexpirydate", "ja-JP", "保修到期日_jp", "保修到期日"),
            // entity.itasset.warrantyexpirydate
            new TranslationSeedItem("entity.itasset.warrantyexpirydate", "zh-CN", "保修到期日", "保修到期日"),
            // entity.itasset.warrantyexpirydate
            new TranslationSeedItem("entity.itasset.warrantyexpirydate", "zh-HK", "保修到期日_hk", "保修到期日"),

            // entity.itasset.warrantyprovider
            new TranslationSeedItem("entity.itasset.warrantyprovider", "en-US", "保修服务商_us", "保修服务商/厂商"),
            // entity.itasset.warrantyprovider
            new TranslationSeedItem("entity.itasset.warrantyprovider", "ja-JP", "保修服务商_jp", "保修服务商/厂商"),
            // entity.itasset.warrantyprovider
            new TranslationSeedItem("entity.itasset.warrantyprovider", "zh-CN", "保修服务商", "保修服务商/厂商"),
            // entity.itasset.warrantyprovider
            new TranslationSeedItem("entity.itasset.warrantyprovider", "zh-HK", "保修服务商_hk", "保修服务商/厂商"),

            // entity.itasset.warrantycontractcode
            new TranslationSeedItem("entity.itasset.warrantycontractcode", "en-US", "保修合同编码_us", "保修合同编码"),
            // entity.itasset.warrantycontractcode
            new TranslationSeedItem("entity.itasset.warrantycontractcode", "ja-JP", "保修合同编码_jp", "保修合同编码"),
            // entity.itasset.warrantycontractcode
            new TranslationSeedItem("entity.itasset.warrantycontractcode", "zh-CN", "保修合同编码", "保修合同编码"),
            // entity.itasset.warrantycontractcode
            new TranslationSeedItem("entity.itasset.warrantycontractcode", "zh-HK", "保修合同编码_hk", "保修合同编码"),

            // entity.itasset.servicehotline
            new TranslationSeedItem("entity.itasset.servicehotline", "en-US", "服务电话_us", "服务电话"),
            // entity.itasset.servicehotline
            new TranslationSeedItem("entity.itasset.servicehotline", "ja-JP", "服务电话_jp", "服务电话"),
            // entity.itasset.servicehotline
            new TranslationSeedItem("entity.itasset.servicehotline", "zh-CN", "服务电话", "服务电话"),
            // entity.itasset.servicehotline
            new TranslationSeedItem("entity.itasset.servicehotline", "zh-HK", "服务电话_hk", "服务电话"),

            // entity.itasset.serviceemail
            new TranslationSeedItem("entity.itasset.serviceemail", "en-US", "服务邮箱_us", "服务邮箱"),
            // entity.itasset.serviceemail
            new TranslationSeedItem("entity.itasset.serviceemail", "ja-JP", "服务邮箱_jp", "服务邮箱"),
            // entity.itasset.serviceemail
            new TranslationSeedItem("entity.itasset.serviceemail", "zh-CN", "服务邮箱", "服务邮箱"),
            // entity.itasset.serviceemail
            new TranslationSeedItem("entity.itasset.serviceemail", "zh-HK", "服务邮箱_hk", "服务邮箱"),

            // entity.itasset.maintenanceexpirydate
            new TranslationSeedItem("entity.itasset.maintenanceexpirydate", "en-US", "维保到期日_us", "维保到期日"),
            // entity.itasset.maintenanceexpirydate
            new TranslationSeedItem("entity.itasset.maintenanceexpirydate", "ja-JP", "维保到期日_jp", "维保到期日"),
            // entity.itasset.maintenanceexpirydate
            new TranslationSeedItem("entity.itasset.maintenanceexpirydate", "zh-CN", "维保到期日", "维保到期日"),
            // entity.itasset.maintenanceexpirydate
            new TranslationSeedItem("entity.itasset.maintenanceexpirydate", "zh-HK", "维保到期日_hk", "维保到期日"),

            // entity.itasset.lastmaintenancedate
            new TranslationSeedItem("entity.itasset.lastmaintenancedate", "en-US", "上次维保日期_us", "上次维保日期"),
            // entity.itasset.lastmaintenancedate
            new TranslationSeedItem("entity.itasset.lastmaintenancedate", "ja-JP", "上次维保日期_jp", "上次维保日期"),
            // entity.itasset.lastmaintenancedate
            new TranslationSeedItem("entity.itasset.lastmaintenancedate", "zh-CN", "上次维保日期", "上次维保日期"),
            // entity.itasset.lastmaintenancedate
            new TranslationSeedItem("entity.itasset.lastmaintenancedate", "zh-HK", "上次维保日期_hk", "上次维保日期"),

            // entity.itasset.nextmaintenancedate
            new TranslationSeedItem("entity.itasset.nextmaintenancedate", "en-US", "下次维保日期_us", "下次维保日期"),
            // entity.itasset.nextmaintenancedate
            new TranslationSeedItem("entity.itasset.nextmaintenancedate", "ja-JP", "下次维保日期_jp", "下次维保日期"),
            // entity.itasset.nextmaintenancedate
            new TranslationSeedItem("entity.itasset.nextmaintenancedate", "zh-CN", "下次维保日期", "下次维保日期"),
            // entity.itasset.nextmaintenancedate
            new TranslationSeedItem("entity.itasset.nextmaintenancedate", "zh-HK", "下次维保日期_hk", "下次维保日期"),

            // entity.itasset.warrantyremark
            new TranslationSeedItem("entity.itasset.warrantyremark", "en-US", "保修说明_us", "保修/维保说明"),
            // entity.itasset.warrantyremark
            new TranslationSeedItem("entity.itasset.warrantyremark", "ja-JP", "保修说明_jp", "保修/维保说明"),
            // entity.itasset.warrantyremark
            new TranslationSeedItem("entity.itasset.warrantyremark", "zh-CN", "保修说明", "保修/维保说明"),
            // entity.itasset.warrantyremark
            new TranslationSeedItem("entity.itasset.warrantyremark", "zh-HK", "保修说明_hk", "保修/维保说明"),
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
        translation.ResourceGroup = "HelpDesk";
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
