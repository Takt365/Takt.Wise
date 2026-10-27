// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials
// 文件名称：TaktWarehouseI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktWarehouse 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktWarehouse 实体国际化翻译种子（键前缀 entity.warehouse.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktWarehouseI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktWarehouse 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 warehouse 实体翻译...", tenantCode);

        foreach (var item in GetWarehouseTranslations())
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

        TaktLogger.Information("TaktWarehouse 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktWarehouse 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.warehouse._self / entity.warehouse.{{field}}；ResourceGroup=Materials；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetWarehouseTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.warehouse._self
            new TranslationSeedItem("entity.warehouse._self", "en-US", "Warehouse Information_us", "实体名称"),
            // entity.warehouse._self
            new TranslationSeedItem("entity.warehouse._self", "ja-JP", "Takt仓库主数据信息_jp", "实体名称"),
            // entity.warehouse._self
            new TranslationSeedItem("entity.warehouse._self", "zh-CN", "Takt仓库主数据信息", "实体名称"),
            // entity.warehouse._self
            new TranslationSeedItem("entity.warehouse._self", "zh-HK", "Takt仓库主数据信息_hk", "实体名称"),

            // entity.warehouse.code
            new TranslationSeedItem("entity.warehouse.code", "en-US", "存货地点编码_us", "存货地点编码（4位，租户+公司+工厂内唯一；业务表冗余存此编码）"),
            // entity.warehouse.code
            new TranslationSeedItem("entity.warehouse.code", "ja-JP", "存货地点编码_jp", "存货地点编码（4位，租户+公司+工厂内唯一；业务表冗余存此编码）"),
            // entity.warehouse.code
            new TranslationSeedItem("entity.warehouse.code", "zh-CN", "存货地点编码", "存货地点编码（4位，租户+公司+工厂内唯一；业务表冗余存此编码）"),
            // entity.warehouse.code
            new TranslationSeedItem("entity.warehouse.code", "zh-HK", "存货地点编码_hk", "存货地点编码（4位，租户+公司+工厂内唯一；业务表冗余存此编码）"),

            // entity.warehouse.name
            new TranslationSeedItem("entity.warehouse.name", "en-US", "仓库名称_us", "仓库名称"),
            // entity.warehouse.name
            new TranslationSeedItem("entity.warehouse.name", "ja-JP", "仓库名称_jp", "仓库名称"),
            // entity.warehouse.name
            new TranslationSeedItem("entity.warehouse.name", "zh-CN", "仓库名称", "仓库名称"),
            // entity.warehouse.name
            new TranslationSeedItem("entity.warehouse.name", "zh-HK", "仓库名称_hk", "仓库名称"),

            // entity.warehouse.shortname
            new TranslationSeedItem("entity.warehouse.shortname", "en-US", "仓库简称_us", "仓库简称"),
            // entity.warehouse.shortname
            new TranslationSeedItem("entity.warehouse.shortname", "ja-JP", "仓库简称_jp", "仓库简称"),
            // entity.warehouse.shortname
            new TranslationSeedItem("entity.warehouse.shortname", "zh-CN", "仓库简称", "仓库简称"),
            // entity.warehouse.shortname
            new TranslationSeedItem("entity.warehouse.shortname", "zh-HK", "仓库简称_hk", "仓库简称"),

            // entity.warehouse.address
            new TranslationSeedItem("entity.warehouse.address", "en-US", "仓库地址_us", "仓库地址（address）"),
            // entity.warehouse.address
            new TranslationSeedItem("entity.warehouse.address", "ja-JP", "仓库地址_jp", "仓库地址（address）"),
            // entity.warehouse.address
            new TranslationSeedItem("entity.warehouse.address", "zh-CN", "仓库地址", "仓库地址（address）"),
            // entity.warehouse.address
            new TranslationSeedItem("entity.warehouse.address", "zh-HK", "仓库地址_hk", "仓库地址（address）"),

            // entity.warehouse.contactperson
            new TranslationSeedItem("entity.warehouse.contactperson", "en-US", "联系人_us", "联系人（contact_person）"),
            // entity.warehouse.contactperson
            new TranslationSeedItem("entity.warehouse.contactperson", "ja-JP", "联系人_jp", "联系人（contact_person）"),
            // entity.warehouse.contactperson
            new TranslationSeedItem("entity.warehouse.contactperson", "zh-CN", "联系人", "联系人（contact_person）"),
            // entity.warehouse.contactperson
            new TranslationSeedItem("entity.warehouse.contactperson", "zh-HK", "联系人_hk", "联系人（contact_person）"),

            // entity.warehouse.contactphone
            new TranslationSeedItem("entity.warehouse.contactphone", "en-US", "联系电话_us", "联系电话（contact_phone）"),
            // entity.warehouse.contactphone
            new TranslationSeedItem("entity.warehouse.contactphone", "ja-JP", "联系电话_jp", "联系电话（contact_phone）"),
            // entity.warehouse.contactphone
            new TranslationSeedItem("entity.warehouse.contactphone", "zh-CN", "联系电话", "联系电话（contact_phone）"),
            // entity.warehouse.contactphone
            new TranslationSeedItem("entity.warehouse.contactphone", "zh-HK", "联系电话_hk", "联系电话（contact_phone）"),

            // entity.warehouse.manageruserid
            new TranslationSeedItem("entity.warehouse.manageruserid", "en-US", "仓库负责人ID_us", "仓库负责人（选项 TaktUsers/options；DictValue=Id）"),
            // entity.warehouse.manageruserid
            new TranslationSeedItem("entity.warehouse.manageruserid", "ja-JP", "仓库负责人ID_jp", "仓库负责人（选项 TaktUsers/options；DictValue=Id）"),
            // entity.warehouse.manageruserid
            new TranslationSeedItem("entity.warehouse.manageruserid", "zh-CN", "仓库负责人ID", "仓库负责人（选项 TaktUsers/options；DictValue=Id）"),
            // entity.warehouse.manageruserid
            new TranslationSeedItem("entity.warehouse.manageruserid", "zh-HK", "仓库负责人ID_hk", "仓库负责人（选项 TaktUsers/options；DictValue=Id）"),

            // entity.warehouse.managerusername
            new TranslationSeedItem("entity.warehouse.managerusername", "en-US", "仓库负责人姓名_us", "仓库负责人姓名（冗余：按 ManagerUserId 取 TaktUser.UserName 联动）"),
            // entity.warehouse.managerusername
            new TranslationSeedItem("entity.warehouse.managerusername", "ja-JP", "仓库负责人姓名_jp", "仓库负责人姓名（冗余：按 ManagerUserId 取 TaktUser.UserName 联动）"),
            // entity.warehouse.managerusername
            new TranslationSeedItem("entity.warehouse.managerusername", "zh-CN", "仓库负责人姓名", "仓库负责人姓名（冗余：按 ManagerUserId 取 TaktUser.UserName 联动）"),
            // entity.warehouse.managerusername
            new TranslationSeedItem("entity.warehouse.managerusername", "zh-HK", "仓库负责人姓名_hk", "仓库负责人姓名（冗余：按 ManagerUserId 取 TaktUser.UserName 联动）"),

            // entity.warehouse.isvirtual
            new TranslationSeedItem("entity.warehouse.isvirtual", "en-US", "虚拟仓_us", "虚拟仓（is_virtual；字典 sys_yes_no；0=实体仓，1=虚拟仓）"),
            // entity.warehouse.isvirtual
            new TranslationSeedItem("entity.warehouse.isvirtual", "ja-JP", "虚拟仓_jp", "虚拟仓（is_virtual；字典 sys_yes_no；0=实体仓，1=虚拟仓）"),
            // entity.warehouse.isvirtual
            new TranslationSeedItem("entity.warehouse.isvirtual", "zh-CN", "虚拟仓", "虚拟仓（is_virtual；字典 sys_yes_no；0=实体仓，1=虚拟仓）"),
            // entity.warehouse.isvirtual
            new TranslationSeedItem("entity.warehouse.isvirtual", "zh-HK", "虚拟仓_hk", "虚拟仓（is_virtual；字典 sys_yes_no；0=实体仓，1=虚拟仓）"),

            // entity.warehouse.type
            new TranslationSeedItem("entity.warehouse.type", "en-US", "仓库类型_us", "仓库类型（字典 logistics_materials_warehouse_type；0=原材料仓，1=半成品仓，2=成品仓，3=不良品仓，4=外协仓，5=其他）"),
            // entity.warehouse.type
            new TranslationSeedItem("entity.warehouse.type", "ja-JP", "仓库类型_jp", "仓库类型（字典 logistics_materials_warehouse_type；0=原材料仓，1=半成品仓，2=成品仓，3=不良品仓，4=外协仓，5=其他）"),
            // entity.warehouse.type
            new TranslationSeedItem("entity.warehouse.type", "zh-CN", "仓库类型", "仓库类型（字典 logistics_materials_warehouse_type；0=原材料仓，1=半成品仓，2=成品仓，3=不良品仓，4=外协仓，5=其他）"),
            // entity.warehouse.type
            new TranslationSeedItem("entity.warehouse.type", "zh-HK", "仓库类型_hk", "仓库类型（字典 logistics_materials_warehouse_type；0=原材料仓，1=半成品仓，2=成品仓，3=不良品仓，4=外协仓，5=其他）"),

            // entity.warehouse.isbuiltin
            new TranslationSeedItem("entity.warehouse.isbuiltin", "en-US", "内置_us", "内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）"),
            // entity.warehouse.isbuiltin
            new TranslationSeedItem("entity.warehouse.isbuiltin", "ja-JP", "内置_jp", "内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）"),
            // entity.warehouse.isbuiltin
            new TranslationSeedItem("entity.warehouse.isbuiltin", "zh-CN", "内置", "内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）"),
            // entity.warehouse.isbuiltin
            new TranslationSeedItem("entity.warehouse.isbuiltin", "zh-HK", "内置_hk", "内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）"),

            // entity.warehouse.sortorder
            new TranslationSeedItem("entity.warehouse.sortorder", "en-US", "排序号_us", "排序号（回填）（越小越靠前）"),
            // entity.warehouse.sortorder
            new TranslationSeedItem("entity.warehouse.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（越小越靠前）"),
            // entity.warehouse.sortorder
            new TranslationSeedItem("entity.warehouse.sortorder", "zh-CN", "排序号", "排序号（回填）（越小越靠前）"),
            // entity.warehouse.sortorder
            new TranslationSeedItem("entity.warehouse.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（越小越靠前）"),

            // entity.warehouse.status
            new TranslationSeedItem("entity.warehouse.status", "en-US", "仓库状态_us", "仓库状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）"),
            // entity.warehouse.status
            new TranslationSeedItem("entity.warehouse.status", "ja-JP", "仓库状态_jp", "仓库状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）"),
            // entity.warehouse.status
            new TranslationSeedItem("entity.warehouse.status", "zh-CN", "仓库状态", "仓库状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）"),
            // entity.warehouse.status
            new TranslationSeedItem("entity.warehouse.status", "zh-HK", "仓库状态_hk", "仓库状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）"),

            // entity.warehouse.storagelocations
            new TranslationSeedItem("entity.warehouse.storagelocations", "en-US", "库位列表_us", "库位列表（主子表关系）"),
            // entity.warehouse.storagelocations
            new TranslationSeedItem("entity.warehouse.storagelocations", "ja-JP", "库位列表_jp", "库位列表（主子表关系）"),
            // entity.warehouse.storagelocations
            new TranslationSeedItem("entity.warehouse.storagelocations", "zh-CN", "库位列表", "库位列表（主子表关系）"),
            // entity.warehouse.storagelocations
            new TranslationSeedItem("entity.warehouse.storagelocations", "zh-HK", "库位列表_hk", "库位列表（主子表关系）"),
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
