// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Sop
// 文件名称：TaktSopExecScanI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSopExecScan 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Sop;

/// <summary>
/// TaktSopExecScan 实体国际化翻译种子（键前缀 entity.sopexecscan.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSopExecScanI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSopExecScan 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 sopexecscan 实体翻译...", tenantCode);

        foreach (var item in GetSopExecScanTranslations())
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

        TaktLogger.Information("TaktSopExecScan 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSopExecScan 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.sopexecscan._self / entity.sopexecscan.{{field}}；ResourceGroup=Sop；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSopExecScanTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.sopexecscan._self
            new TranslationSeedItem("entity.sopexecscan._self", "en-US", "Sop Exec Scan Information_us", "实体名称"),
            // entity.sopexecscan._self
            new TranslationSeedItem("entity.sopexecscan._self", "ja-JP", "SOP 物料扫码记录信息_jp", "实体名称"),
            // entity.sopexecscan._self
            new TranslationSeedItem("entity.sopexecscan._self", "zh-CN", "SOP 物料扫码记录信息", "实体名称"),
            // entity.sopexecscan._self
            new TranslationSeedItem("entity.sopexecscan._self", "zh-HK", "SOP 物料扫码记录信息_hk", "实体名称"),

            // entity.sopexecscan.execid
            new TranslationSeedItem("entity.sopexecscan.execid", "en-US", "执行追溯ID_us", "执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）"),
            // entity.sopexecscan.execid
            new TranslationSeedItem("entity.sopexecscan.execid", "ja-JP", "执行追溯ID_jp", "执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）"),
            // entity.sopexecscan.execid
            new TranslationSeedItem("entity.sopexecscan.execid", "zh-CN", "执行追溯ID", "执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）"),
            // entity.sopexecscan.execid
            new TranslationSeedItem("entity.sopexecscan.execid", "zh-HK", "执行追溯ID_hk", "执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）"),

            // entity.sopexecscan.execstepid
            new TranslationSeedItem("entity.sopexecscan.execstepid", "en-US", "工步执行明细ID_us", "工步执行明细 ID（选项 TaktSopExecSteps/options；DictValue=Id）"),
            // entity.sopexecscan.execstepid
            new TranslationSeedItem("entity.sopexecscan.execstepid", "ja-JP", "工步执行明细ID_jp", "工步执行明细 ID（选项 TaktSopExecSteps/options；DictValue=Id）"),
            // entity.sopexecscan.execstepid
            new TranslationSeedItem("entity.sopexecscan.execstepid", "zh-CN", "工步执行明细ID", "工步执行明细 ID（选项 TaktSopExecSteps/options；DictValue=Id）"),
            // entity.sopexecscan.execstepid
            new TranslationSeedItem("entity.sopexecscan.execstepid", "zh-HK", "工步执行明细ID_hk", "工步执行明细 ID（选项 TaktSopExecSteps/options；DictValue=Id）"),

            // entity.sopexecscan.stepid
            new TranslationSeedItem("entity.sopexecscan.stepid", "en-US", "工步ID_us", "工步 ID（选项 TaktSopSteps/options；DictValue=Id）"),
            // entity.sopexecscan.stepid
            new TranslationSeedItem("entity.sopexecscan.stepid", "ja-JP", "工步ID_jp", "工步 ID（选项 TaktSopSteps/options；DictValue=Id）"),
            // entity.sopexecscan.stepid
            new TranslationSeedItem("entity.sopexecscan.stepid", "zh-CN", "工步ID", "工步 ID（选项 TaktSopSteps/options；DictValue=Id）"),
            // entity.sopexecscan.stepid
            new TranslationSeedItem("entity.sopexecscan.stepid", "zh-HK", "工步ID_hk", "工步 ID（选项 TaktSopSteps/options；DictValue=Id）"),

            // entity.sopexecscan.scannedbarcode
            new TranslationSeedItem("entity.sopexecscan.scannedbarcode", "en-US", "扫描条码_us", "扫描条码"),
            // entity.sopexecscan.scannedbarcode
            new TranslationSeedItem("entity.sopexecscan.scannedbarcode", "ja-JP", "扫描条码_jp", "扫描条码"),
            // entity.sopexecscan.scannedbarcode
            new TranslationSeedItem("entity.sopexecscan.scannedbarcode", "zh-CN", "扫描条码", "扫描条码"),
            // entity.sopexecscan.scannedbarcode
            new TranslationSeedItem("entity.sopexecscan.scannedbarcode", "zh-HK", "扫描条码_hk", "扫描条码"),

            // entity.sopexecscan.expectedmaterialcode
            new TranslationSeedItem("entity.sopexecscan.expectedmaterialcode", "en-US", "期望物料编码_us", "期望物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.sopexecscan.expectedmaterialcode
            new TranslationSeedItem("entity.sopexecscan.expectedmaterialcode", "ja-JP", "期望物料编码_jp", "期望物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.sopexecscan.expectedmaterialcode
            new TranslationSeedItem("entity.sopexecscan.expectedmaterialcode", "zh-CN", "期望物料编码", "期望物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.sopexecscan.expectedmaterialcode
            new TranslationSeedItem("entity.sopexecscan.expectedmaterialcode", "zh-HK", "期望物料编码_hk", "期望物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),

            // entity.sopexecscan.scanresult
            new TranslationSeedItem("entity.sopexecscan.scanresult", "en-US", "扫码结果_us", "扫码结果（字典 logistics_manufacturing_sop_scan_result；1=PASS，2=NG）"),
            // entity.sopexecscan.scanresult
            new TranslationSeedItem("entity.sopexecscan.scanresult", "ja-JP", "扫码结果_jp", "扫码结果（字典 logistics_manufacturing_sop_scan_result；1=PASS，2=NG）"),
            // entity.sopexecscan.scanresult
            new TranslationSeedItem("entity.sopexecscan.scanresult", "zh-CN", "扫码结果", "扫码结果（字典 logistics_manufacturing_sop_scan_result；1=PASS，2=NG）"),
            // entity.sopexecscan.scanresult
            new TranslationSeedItem("entity.sopexecscan.scanresult", "zh-HK", "扫码结果_hk", "扫码结果（字典 logistics_manufacturing_sop_scan_result；1=PASS，2=NG）"),

            // entity.sopexecscan.matchmessage
            new TranslationSeedItem("entity.sopexecscan.matchmessage", "en-US", "比对说明_us", "比对说明"),
            // entity.sopexecscan.matchmessage
            new TranslationSeedItem("entity.sopexecscan.matchmessage", "ja-JP", "比对说明_jp", "比对说明"),
            // entity.sopexecscan.matchmessage
            new TranslationSeedItem("entity.sopexecscan.matchmessage", "zh-CN", "比对说明", "比对说明"),
            // entity.sopexecscan.matchmessage
            new TranslationSeedItem("entity.sopexecscan.matchmessage", "zh-HK", "比对说明_hk", "比对说明"),

            // entity.sopexecscan.scannedat
            new TranslationSeedItem("entity.sopexecscan.scannedat", "en-US", "扫描时间_us", "扫描时间"),
            // entity.sopexecscan.scannedat
            new TranslationSeedItem("entity.sopexecscan.scannedat", "ja-JP", "扫描时间_jp", "扫描时间"),
            // entity.sopexecscan.scannedat
            new TranslationSeedItem("entity.sopexecscan.scannedat", "zh-CN", "扫描时间", "扫描时间"),
            // entity.sopexecscan.scannedat
            new TranslationSeedItem("entity.sopexecscan.scannedat", "zh-HK", "扫描时间_hk", "扫描时间"),

            // entity.sopexecscan.exec
            new TranslationSeedItem("entity.sopexecscan.exec", "en-US", "执行追溯_us", "执行追溯"),
            // entity.sopexecscan.exec
            new TranslationSeedItem("entity.sopexecscan.exec", "ja-JP", "执行追溯_jp", "执行追溯"),
            // entity.sopexecscan.exec
            new TranslationSeedItem("entity.sopexecscan.exec", "zh-CN", "执行追溯", "执行追溯"),
            // entity.sopexecscan.exec
            new TranslationSeedItem("entity.sopexecscan.exec", "zh-HK", "执行追溯_hk", "执行追溯"),
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
        translation.ResourceGroup = "Sop";
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
