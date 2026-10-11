// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Aps
// 文件名称：TaktChangeoverMatrixI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktChangeoverMatrix 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Aps;

/// <summary>
/// TaktChangeoverMatrix 实体国际化翻译种子（键前缀 entity.changeovermatrix.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktChangeoverMatrixI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktChangeoverMatrix 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 changeovermatrix 实体翻译...", tenantCode);

        foreach (var item in GetChangeoverMatrixTranslations())
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

        TaktLogger.Information("TaktChangeoverMatrix 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktChangeoverMatrix 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.changeovermatrix._self / entity.changeovermatrix.{{field}}；ResourceGroup=Aps；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetChangeoverMatrixTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.changeovermatrix._self
            new TranslationSeedItem("entity.changeovermatrix._self", "en-US", "Changeover Matrix Information_us", "实体名称"),
            // entity.changeovermatrix._self
            new TranslationSeedItem("entity.changeovermatrix._self", "ja-JP", "换型矩阵信息_jp", "实体名称"),
            // entity.changeovermatrix._self
            new TranslationSeedItem("entity.changeovermatrix._self", "zh-CN", "换型矩阵信息", "实体名称"),
            // entity.changeovermatrix._self
            new TranslationSeedItem("entity.changeovermatrix._self", "zh-HK", "换型矩阵信息_hk", "实体名称"),

            // entity.changeovermatrix.workcentercode
            new TranslationSeedItem("entity.changeovermatrix.workcentercode", "en-US", "工作中心编码_us", "工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）"),
            // entity.changeovermatrix.workcentercode
            new TranslationSeedItem("entity.changeovermatrix.workcentercode", "ja-JP", "工作中心编码_jp", "工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）"),
            // entity.changeovermatrix.workcentercode
            new TranslationSeedItem("entity.changeovermatrix.workcentercode", "zh-CN", "工作中心编码", "工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）"),
            // entity.changeovermatrix.workcentercode
            new TranslationSeedItem("entity.changeovermatrix.workcentercode", "zh-HK", "工作中心编码_hk", "工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）"),

            // entity.changeovermatrix.frommaterialcode
            new TranslationSeedItem("entity.changeovermatrix.frommaterialcode", "en-US", "换型前物料编码_us", "换型前物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.changeovermatrix.frommaterialcode
            new TranslationSeedItem("entity.changeovermatrix.frommaterialcode", "ja-JP", "换型前物料编码_jp", "换型前物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.changeovermatrix.frommaterialcode
            new TranslationSeedItem("entity.changeovermatrix.frommaterialcode", "zh-CN", "换型前物料编码", "换型前物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.changeovermatrix.frommaterialcode
            new TranslationSeedItem("entity.changeovermatrix.frommaterialcode", "zh-HK", "换型前物料编码_hk", "换型前物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),

            // entity.changeovermatrix.tomaterialcode
            new TranslationSeedItem("entity.changeovermatrix.tomaterialcode", "en-US", "换型后物料编码_us", "换型后物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.changeovermatrix.tomaterialcode
            new TranslationSeedItem("entity.changeovermatrix.tomaterialcode", "ja-JP", "换型后物料编码_jp", "换型后物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.changeovermatrix.tomaterialcode
            new TranslationSeedItem("entity.changeovermatrix.tomaterialcode", "zh-CN", "换型后物料编码", "换型后物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.changeovermatrix.tomaterialcode
            new TranslationSeedItem("entity.changeovermatrix.tomaterialcode", "zh-HK", "换型后物料编码_hk", "换型后物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),

            // entity.changeovermatrix.changeoverminutes
            new TranslationSeedItem("entity.changeovermatrix.changeoverminutes", "en-US", "换型时间分钟_us", "换型时间（分钟）"),
            // entity.changeovermatrix.changeoverminutes
            new TranslationSeedItem("entity.changeovermatrix.changeoverminutes", "ja-JP", "换型时间分钟_jp", "换型时间（分钟）"),
            // entity.changeovermatrix.changeoverminutes
            new TranslationSeedItem("entity.changeovermatrix.changeoverminutes", "zh-CN", "换型时间分钟", "换型时间（分钟）"),
            // entity.changeovermatrix.changeoverminutes
            new TranslationSeedItem("entity.changeovermatrix.changeoverminutes", "zh-HK", "换型时间分钟_hk", "换型时间（分钟）"),

            // entity.changeovermatrix.matrixstatus
            new TranslationSeedItem("entity.changeovermatrix.matrixstatus", "en-US", "矩阵状态_us", "状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.changeovermatrix.matrixstatus
            new TranslationSeedItem("entity.changeovermatrix.matrixstatus", "ja-JP", "矩阵状态_jp", "状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.changeovermatrix.matrixstatus
            new TranslationSeedItem("entity.changeovermatrix.matrixstatus", "zh-CN", "矩阵状态", "状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.changeovermatrix.matrixstatus
            new TranslationSeedItem("entity.changeovermatrix.matrixstatus", "zh-HK", "矩阵状态_hk", "状态（字典 sys_normal_disable；1=启用，0=禁用）"),
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
        translation.ResourceGroup = "Aps";
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
