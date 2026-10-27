// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials
// 文件名称：TaktStorageLocationI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktStorageLocation 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials;

/// <summary>
/// TaktStorageLocation 实体国际化翻译种子（键前缀 entity.storagelocation.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktStorageLocationI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktStorageLocation 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 storagelocation 实体翻译...", tenantCode);

        foreach (var item in GetStorageLocationTranslations())
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

        TaktLogger.Information("TaktStorageLocation 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktStorageLocation 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.storagelocation._self / entity.storagelocation.{{field}}；ResourceGroup=Materials；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetStorageLocationTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.storagelocation._self
            new TranslationSeedItem("entity.storagelocation._self", "en-US", "Storage Location Information_us", "实体名称"),
            // entity.storagelocation._self
            new TranslationSeedItem("entity.storagelocation._self", "ja-JP", "Takt库位主数据信息_jp", "实体名称"),
            // entity.storagelocation._self
            new TranslationSeedItem("entity.storagelocation._self", "zh-CN", "Takt库位主数据信息", "实体名称"),
            // entity.storagelocation._self
            new TranslationSeedItem("entity.storagelocation._self", "zh-HK", "Takt库位主数据信息_hk", "实体名称"),

            // entity.storagelocation.warehouseid
            new TranslationSeedItem("entity.storagelocation.warehouseid", "en-US", "仓库ID_us", "仓库 ID（选项 TaktWarehouses/options；DictValue=Id）"),
            // entity.storagelocation.warehouseid
            new TranslationSeedItem("entity.storagelocation.warehouseid", "ja-JP", "仓库ID_jp", "仓库 ID（选项 TaktWarehouses/options；DictValue=Id）"),
            // entity.storagelocation.warehouseid
            new TranslationSeedItem("entity.storagelocation.warehouseid", "zh-CN", "仓库ID", "仓库 ID（选项 TaktWarehouses/options；DictValue=Id）"),
            // entity.storagelocation.warehouseid
            new TranslationSeedItem("entity.storagelocation.warehouseid", "zh-HK", "仓库ID_hk", "仓库 ID（选项 TaktWarehouses/options；DictValue=Id）"),

            // entity.storagelocation.warehousecode
            new TranslationSeedItem("entity.storagelocation.warehousecode", "en-US", "存货地点编码_us", "仓库编码（冗余；关联 TaktWarehouse.WarehouseCode，选项 TaktWarehouses/options，DictValue=WarehouseCode）"),
            // entity.storagelocation.warehousecode
            new TranslationSeedItem("entity.storagelocation.warehousecode", "ja-JP", "存货地点编码_jp", "仓库编码（冗余；关联 TaktWarehouse.WarehouseCode，选项 TaktWarehouses/options，DictValue=WarehouseCode）"),
            // entity.storagelocation.warehousecode
            new TranslationSeedItem("entity.storagelocation.warehousecode", "zh-CN", "存货地点编码", "仓库编码（冗余；关联 TaktWarehouse.WarehouseCode，选项 TaktWarehouses/options，DictValue=WarehouseCode）"),
            // entity.storagelocation.warehousecode
            new TranslationSeedItem("entity.storagelocation.warehousecode", "zh-HK", "存货地点编码_hk", "仓库编码（冗余；关联 TaktWarehouse.WarehouseCode，选项 TaktWarehouses/options，DictValue=WarehouseCode）"),

            // entity.storagelocation.locationcode
            new TranslationSeedItem("entity.storagelocation.locationcode", "en-US", "库位编码_us", "库位编码（40，租户+公司+工厂+仓库内唯一；序列号入出库等业务表存此编码；前端表单选择编码规则后自动通过 TaktNumbering 库位编码规则生成并展示，非手输；单据类型菜单：库位）"),
            // entity.storagelocation.locationcode
            new TranslationSeedItem("entity.storagelocation.locationcode", "ja-JP", "库位编码_jp", "库位编码（40，租户+公司+工厂+仓库内唯一；序列号入出库等业务表存此编码；前端表单选择编码规则后自动通过 TaktNumbering 库位编码规则生成并展示，非手输；单据类型菜单：库位）"),
            // entity.storagelocation.locationcode
            new TranslationSeedItem("entity.storagelocation.locationcode", "zh-CN", "库位编码", "库位编码（40，租户+公司+工厂+仓库内唯一；序列号入出库等业务表存此编码；前端表单选择编码规则后自动通过 TaktNumbering 库位编码规则生成并展示，非手输；单据类型菜单：库位）"),
            // entity.storagelocation.locationcode
            new TranslationSeedItem("entity.storagelocation.locationcode", "zh-HK", "库位编码_hk", "库位编码（40，租户+公司+工厂+仓库内唯一；序列号入出库等业务表存此编码；前端表单选择编码规则后自动通过 TaktNumbering 库位编码规则生成并展示，非手输；单据类型菜单：库位）"),

            // entity.storagelocation.locationname
            new TranslationSeedItem("entity.storagelocation.locationname", "en-US", "库位名称_us", "库位名称"),
            // entity.storagelocation.locationname
            new TranslationSeedItem("entity.storagelocation.locationname", "ja-JP", "库位名称_jp", "库位名称"),
            // entity.storagelocation.locationname
            new TranslationSeedItem("entity.storagelocation.locationname", "zh-CN", "库位名称", "库位名称"),
            // entity.storagelocation.locationname
            new TranslationSeedItem("entity.storagelocation.locationname", "zh-HK", "库位名称_hk", "库位名称"),

            // entity.storagelocation.locationtype
            new TranslationSeedItem("entity.storagelocation.locationtype", "en-US", "库位类型_us", "库位类型（字典 logistics_materials_storage_location_type）"),
            // entity.storagelocation.locationtype
            new TranslationSeedItem("entity.storagelocation.locationtype", "ja-JP", "库位类型_jp", "库位类型（字典 logistics_materials_storage_location_type）"),
            // entity.storagelocation.locationtype
            new TranslationSeedItem("entity.storagelocation.locationtype", "zh-CN", "库位类型", "库位类型（字典 logistics_materials_storage_location_type）"),
            // entity.storagelocation.locationtype
            new TranslationSeedItem("entity.storagelocation.locationtype", "zh-HK", "库位类型_hk", "库位类型（字典 logistics_materials_storage_location_type）"),

            // entity.storagelocation.isbuiltin
            new TranslationSeedItem("entity.storagelocation.isbuiltin", "en-US", "内置_us", "内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）"),
            // entity.storagelocation.isbuiltin
            new TranslationSeedItem("entity.storagelocation.isbuiltin", "ja-JP", "内置_jp", "内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）"),
            // entity.storagelocation.isbuiltin
            new TranslationSeedItem("entity.storagelocation.isbuiltin", "zh-CN", "内置", "内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）"),
            // entity.storagelocation.isbuiltin
            new TranslationSeedItem("entity.storagelocation.isbuiltin", "zh-HK", "内置_hk", "内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）"),

            // entity.storagelocation.sortorder
            new TranslationSeedItem("entity.storagelocation.sortorder", "en-US", "排序号_us", "排序号（回填）（越小越靠前）"),
            // entity.storagelocation.sortorder
            new TranslationSeedItem("entity.storagelocation.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（越小越靠前）"),
            // entity.storagelocation.sortorder
            new TranslationSeedItem("entity.storagelocation.sortorder", "zh-CN", "排序号", "排序号（回填）（越小越靠前）"),
            // entity.storagelocation.sortorder
            new TranslationSeedItem("entity.storagelocation.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（越小越靠前）"),

            // entity.storagelocation.locationstatus
            new TranslationSeedItem("entity.storagelocation.locationstatus", "en-US", "库位状态_us", "库位状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）"),
            // entity.storagelocation.locationstatus
            new TranslationSeedItem("entity.storagelocation.locationstatus", "ja-JP", "库位状态_jp", "库位状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）"),
            // entity.storagelocation.locationstatus
            new TranslationSeedItem("entity.storagelocation.locationstatus", "zh-CN", "库位状态", "库位状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）"),
            // entity.storagelocation.locationstatus
            new TranslationSeedItem("entity.storagelocation.locationstatus", "zh-HK", "库位状态_hk", "库位状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）"),

            // entity.storagelocation.warehouse
            new TranslationSeedItem("entity.storagelocation.warehouse", "en-US", "所属仓库_us", "所属仓库（主子表关系）"),
            // entity.storagelocation.warehouse
            new TranslationSeedItem("entity.storagelocation.warehouse", "ja-JP", "所属仓库_jp", "所属仓库（主子表关系）"),
            // entity.storagelocation.warehouse
            new TranslationSeedItem("entity.storagelocation.warehouse", "zh-CN", "所属仓库", "所属仓库（主子表关系）"),
            // entity.storagelocation.warehouse
            new TranslationSeedItem("entity.storagelocation.warehouse", "zh-HK", "所属仓库_hk", "所属仓库（主子表关系）"),
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
        translation.ResourceGroup = "Materials";
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
