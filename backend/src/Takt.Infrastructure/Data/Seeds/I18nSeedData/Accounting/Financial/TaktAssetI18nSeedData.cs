// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Financial
// 文件名称：TaktAssetI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktAsset 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Financial;

/// <summary>
/// TaktAsset 实体国际化翻译种子（键前缀 entity.asset.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktAssetI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktAsset 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 asset 实体翻译...", tenantCode);

        foreach (var item in GetAssetTranslations())
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

        TaktLogger.Information("TaktAsset 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktAsset 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.asset._self / entity.asset.{{field}}；ResourceGroup=Financial；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetAssetTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.asset._self
            new TranslationSeedItem("entity.asset._self", "en-US", "Asset Information_us", "实体名称"),
            // entity.asset._self
            new TranslationSeedItem("entity.asset._self", "ja-JP", "资产信息_jp", "实体名称"),
            // entity.asset._self
            new TranslationSeedItem("entity.asset._self", "zh-CN", "资产信息", "实体名称"),
            // entity.asset._self
            new TranslationSeedItem("entity.asset._self", "zh-HK", "资产信息_hk", "实体名称"),

            // entity.asset.code
            new TranslationSeedItem("entity.asset.code", "en-US", "资产代码_us", "资产代码（前端表单选择编码规则后自动通过 TaktNumbering 资产代码规则生成并展示，非手输；单据类型菜单：固定资产）"),
            // entity.asset.code
            new TranslationSeedItem("entity.asset.code", "ja-JP", "资产代码_jp", "资产代码（前端表单选择编码规则后自动通过 TaktNumbering 资产代码规则生成并展示，非手输；单据类型菜单：固定资产）"),
            // entity.asset.code
            new TranslationSeedItem("entity.asset.code", "zh-CN", "资产代码", "资产代码（前端表单选择编码规则后自动通过 TaktNumbering 资产代码规则生成并展示，非手输；单据类型菜单：固定资产）"),
            // entity.asset.code
            new TranslationSeedItem("entity.asset.code", "zh-HK", "资产代码_hk", "资产代码（前端表单选择编码规则后自动通过 TaktNumbering 资产代码规则生成并展示，非手输；单据类型菜单：固定资产）"),

            // entity.asset.name
            new TranslationSeedItem("entity.asset.name", "en-US", "资产名称_us", "资产名称"),
            // entity.asset.name
            new TranslationSeedItem("entity.asset.name", "ja-JP", "资产名称_jp", "资产名称"),
            // entity.asset.name
            new TranslationSeedItem("entity.asset.name", "zh-CN", "资产名称", "资产名称"),
            // entity.asset.name
            new TranslationSeedItem("entity.asset.name", "zh-HK", "资产名称_hk", "资产名称"),

            // entity.asset.category
            new TranslationSeedItem("entity.asset.category", "en-US", "资产分类_us", "资产分类（字典 accounting_financial_asset_category）"),
            // entity.asset.category
            new TranslationSeedItem("entity.asset.category", "ja-JP", "资产分类_jp", "资产分类（字典 accounting_financial_asset_category）"),
            // entity.asset.category
            new TranslationSeedItem("entity.asset.category", "zh-CN", "资产分类", "资产分类（字典 accounting_financial_asset_category）"),
            // entity.asset.category
            new TranslationSeedItem("entity.asset.category", "zh-HK", "资产分类_hk", "资产分类（字典 accounting_financial_asset_category）"),

            // entity.asset.type
            new TranslationSeedItem("entity.asset.type", "en-US", "资产类型_us", "资产类型（字典 accounting_financial_asset_type；NORM=普通资产）"),
            // entity.asset.type
            new TranslationSeedItem("entity.asset.type", "ja-JP", "资产类型_jp", "资产类型（字典 accounting_financial_asset_type；NORM=普通资产）"),
            // entity.asset.type
            new TranslationSeedItem("entity.asset.type", "zh-CN", "资产类型", "资产类型（字典 accounting_financial_asset_type；NORM=普通资产）"),
            // entity.asset.type
            new TranslationSeedItem("entity.asset.type", "zh-HK", "资产类型_hk", "资产类型（字典 accounting_financial_asset_type；NORM=普通资产）"),

            // entity.asset.originalvalue
            new TranslationSeedItem("entity.asset.originalvalue", "en-US", "资产原值_us", "资产原值"),
            // entity.asset.originalvalue
            new TranslationSeedItem("entity.asset.originalvalue", "ja-JP", "资产原值_jp", "资产原值"),
            // entity.asset.originalvalue
            new TranslationSeedItem("entity.asset.originalvalue", "zh-CN", "资产原值", "资产原值"),
            // entity.asset.originalvalue
            new TranslationSeedItem("entity.asset.originalvalue", "zh-HK", "资产原值_hk", "资产原值"),

            // entity.asset.netvalue
            new TranslationSeedItem("entity.asset.netvalue", "en-US", "资产净值_us", "资产净值"),
            // entity.asset.netvalue
            new TranslationSeedItem("entity.asset.netvalue", "ja-JP", "资产净值_jp", "资产净值"),
            // entity.asset.netvalue
            new TranslationSeedItem("entity.asset.netvalue", "zh-CN", "资产净值", "资产净值"),
            // entity.asset.netvalue
            new TranslationSeedItem("entity.asset.netvalue", "zh-HK", "资产净值_hk", "资产净值"),

            // entity.asset.accumulateddepreciation
            new TranslationSeedItem("entity.asset.accumulateddepreciation", "en-US", "累计折旧_us", "累计折旧"),
            // entity.asset.accumulateddepreciation
            new TranslationSeedItem("entity.asset.accumulateddepreciation", "ja-JP", "累计折旧_jp", "累计折旧"),
            // entity.asset.accumulateddepreciation
            new TranslationSeedItem("entity.asset.accumulateddepreciation", "zh-CN", "累计折旧", "累计折旧"),
            // entity.asset.accumulateddepreciation
            new TranslationSeedItem("entity.asset.accumulateddepreciation", "zh-HK", "累计折旧_hk", "累计折旧"),

            // entity.asset.costcenterid
            new TranslationSeedItem("entity.asset.costcenterid", "en-US", "成本中心ID_us", "成本中心（选项 TaktCostCenters/tree-options；DictValue=Id）"),
            // entity.asset.costcenterid
            new TranslationSeedItem("entity.asset.costcenterid", "ja-JP", "成本中心ID_jp", "成本中心（选项 TaktCostCenters/tree-options；DictValue=Id）"),
            // entity.asset.costcenterid
            new TranslationSeedItem("entity.asset.costcenterid", "zh-CN", "成本中心ID", "成本中心（选项 TaktCostCenters/tree-options；DictValue=Id）"),
            // entity.asset.costcenterid
            new TranslationSeedItem("entity.asset.costcenterid", "zh-HK", "成本中心ID_hk", "成本中心（选项 TaktCostCenters/tree-options；DictValue=Id）"),

            // entity.asset.costcentername
            new TranslationSeedItem("entity.asset.costcentername", "en-US", "成本中心名称_us", "成本中心名称（冗余：按 CostCenterId 取 TaktCostCenter.CostCenterName 联动）"),
            // entity.asset.costcentername
            new TranslationSeedItem("entity.asset.costcentername", "ja-JP", "成本中心名称_jp", "成本中心名称（冗余：按 CostCenterId 取 TaktCostCenter.CostCenterName 联动）"),
            // entity.asset.costcentername
            new TranslationSeedItem("entity.asset.costcentername", "zh-CN", "成本中心名称", "成本中心名称（冗余：按 CostCenterId 取 TaktCostCenter.CostCenterName 联动）"),
            // entity.asset.costcentername
            new TranslationSeedItem("entity.asset.costcentername", "zh-HK", "成本中心名称_hk", "成本中心名称（冗余：按 CostCenterId 取 TaktCostCenter.CostCenterName 联动）"),

            // entity.asset.deptid
            new TranslationSeedItem("entity.asset.deptid", "en-US", "部门ID_us", "部门（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.asset.deptid
            new TranslationSeedItem("entity.asset.deptid", "ja-JP", "部门ID_jp", "部门（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.asset.deptid
            new TranslationSeedItem("entity.asset.deptid", "zh-CN", "部门ID", "部门（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.asset.deptid
            new TranslationSeedItem("entity.asset.deptid", "zh-HK", "部门ID_hk", "部门（选项 TaktDepts/tree-options；DictValue=Id）"),

            // entity.asset.deptname
            new TranslationSeedItem("entity.asset.deptname", "en-US", "部门名称_us", "部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）"),
            // entity.asset.deptname
            new TranslationSeedItem("entity.asset.deptname", "ja-JP", "部门名称_jp", "部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）"),
            // entity.asset.deptname
            new TranslationSeedItem("entity.asset.deptname", "zh-CN", "部门名称", "部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）"),
            // entity.asset.deptname
            new TranslationSeedItem("entity.asset.deptname", "zh-HK", "部门名称_hk", "部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）"),

            // entity.asset.userid
            new TranslationSeedItem("entity.asset.userid", "en-US", "使用者ID_us", "使用者（选项 TaktUsers/options；DictValue=Id）"),
            // entity.asset.userid
            new TranslationSeedItem("entity.asset.userid", "ja-JP", "使用者ID_jp", "使用者（选项 TaktUsers/options；DictValue=Id）"),
            // entity.asset.userid
            new TranslationSeedItem("entity.asset.userid", "zh-CN", "使用者ID", "使用者（选项 TaktUsers/options；DictValue=Id）"),
            // entity.asset.userid
            new TranslationSeedItem("entity.asset.userid", "zh-HK", "使用者ID_hk", "使用者（选项 TaktUsers/options；DictValue=Id）"),

            // entity.asset.username
            new TranslationSeedItem("entity.asset.username", "en-US", "使用者名称_us", "使用者名称（冗余：按 UserId 取 TaktUser.UserName 联动）"),
            // entity.asset.username
            new TranslationSeedItem("entity.asset.username", "ja-JP", "使用者名称_jp", "使用者名称（冗余：按 UserId 取 TaktUser.UserName 联动）"),
            // entity.asset.username
            new TranslationSeedItem("entity.asset.username", "zh-CN", "使用者名称", "使用者名称（冗余：按 UserId 取 TaktUser.UserName 联动）"),
            // entity.asset.username
            new TranslationSeedItem("entity.asset.username", "zh-HK", "使用者名称_hk", "使用者名称（冗余：按 UserId 取 TaktUser.UserName 联动）"),

            // entity.asset.location
            new TranslationSeedItem("entity.asset.location", "en-US", "资产位置_us", "资产位置"),
            // entity.asset.location
            new TranslationSeedItem("entity.asset.location", "ja-JP", "资产位置_jp", "资产位置"),
            // entity.asset.location
            new TranslationSeedItem("entity.asset.location", "zh-CN", "资产位置", "资产位置"),
            // entity.asset.location
            new TranslationSeedItem("entity.asset.location", "zh-HK", "资产位置_hk", "资产位置"),

            // entity.asset.purchasedate
            new TranslationSeedItem("entity.asset.purchasedate", "en-US", "购买日期_us", "购买日期"),
            // entity.asset.purchasedate
            new TranslationSeedItem("entity.asset.purchasedate", "ja-JP", "购买日期_jp", "购买日期"),
            // entity.asset.purchasedate
            new TranslationSeedItem("entity.asset.purchasedate", "zh-CN", "购买日期", "购买日期"),
            // entity.asset.purchasedate
            new TranslationSeedItem("entity.asset.purchasedate", "zh-HK", "购买日期_hk", "购买日期"),

            // entity.asset.startdate
            new TranslationSeedItem("entity.asset.startdate", "en-US", "启用日期_us", "启用日期"),
            // entity.asset.startdate
            new TranslationSeedItem("entity.asset.startdate", "ja-JP", "启用日期_jp", "启用日期"),
            // entity.asset.startdate
            new TranslationSeedItem("entity.asset.startdate", "zh-CN", "启用日期", "启用日期"),
            // entity.asset.startdate
            new TranslationSeedItem("entity.asset.startdate", "zh-HK", "启用日期_hk", "启用日期"),

            // entity.asset.scrapdate
            new TranslationSeedItem("entity.asset.scrapdate", "en-US", "报废日期_us", "报废日期"),
            // entity.asset.scrapdate
            new TranslationSeedItem("entity.asset.scrapdate", "ja-JP", "报废日期_jp", "报废日期"),
            // entity.asset.scrapdate
            new TranslationSeedItem("entity.asset.scrapdate", "zh-CN", "报废日期", "报废日期"),
            // entity.asset.scrapdate
            new TranslationSeedItem("entity.asset.scrapdate", "zh-HK", "报废日期_hk", "报废日期"),

            // entity.asset.disposaldate
            new TranslationSeedItem("entity.asset.disposaldate", "en-US", "处置日期_us", "处置日期"),
            // entity.asset.disposaldate
            new TranslationSeedItem("entity.asset.disposaldate", "ja-JP", "处置日期_jp", "处置日期"),
            // entity.asset.disposaldate
            new TranslationSeedItem("entity.asset.disposaldate", "zh-CN", "处置日期", "处置日期"),
            // entity.asset.disposaldate
            new TranslationSeedItem("entity.asset.disposaldate", "zh-HK", "处置日期_hk", "处置日期"),

            // entity.asset.expectedlifemonths
            new TranslationSeedItem("entity.asset.expectedlifemonths", "en-US", "预计使用月数_us", "预计使用月数"),
            // entity.asset.expectedlifemonths
            new TranslationSeedItem("entity.asset.expectedlifemonths", "ja-JP", "预计使用月数_jp", "预计使用月数"),
            // entity.asset.expectedlifemonths
            new TranslationSeedItem("entity.asset.expectedlifemonths", "zh-CN", "预计使用月数", "预计使用月数"),
            // entity.asset.expectedlifemonths
            new TranslationSeedItem("entity.asset.expectedlifemonths", "zh-HK", "预计使用月数_hk", "预计使用月数"),

            // entity.asset.depreciationmethod
            new TranslationSeedItem("entity.asset.depreciationmethod", "en-US", "折旧方法_us", "折旧方法（字典 accounting_financial_depreciation_method：0=不自动计提，1=直线法，2=双倍余额递减，3=年数总和，4=产量法，5=手工，6=剩余年限直线）"),
            // entity.asset.depreciationmethod
            new TranslationSeedItem("entity.asset.depreciationmethod", "ja-JP", "折旧方法_jp", "折旧方法（字典 accounting_financial_depreciation_method：0=不自动计提，1=直线法，2=双倍余额递减，3=年数总和，4=产量法，5=手工，6=剩余年限直线）"),
            // entity.asset.depreciationmethod
            new TranslationSeedItem("entity.asset.depreciationmethod", "zh-CN", "折旧方法", "折旧方法（字典 accounting_financial_depreciation_method：0=不自动计提，1=直线法，2=双倍余额递减，3=年数总和，4=产量法，5=手工，6=剩余年限直线）"),
            // entity.asset.depreciationmethod
            new TranslationSeedItem("entity.asset.depreciationmethod", "zh-HK", "折旧方法_hk", "折旧方法（字典 accounting_financial_depreciation_method：0=不自动计提，1=直线法，2=双倍余额递减，3=年数总和，4=产量法，5=手工，6=剩余年限直线）"),

            // entity.asset.monthlydepreciation
            new TranslationSeedItem("entity.asset.monthlydepreciation", "en-US", "每月折旧金额_us", "每月折旧金额"),
            // entity.asset.monthlydepreciation
            new TranslationSeedItem("entity.asset.monthlydepreciation", "ja-JP", "每月折旧金额_jp", "每月折旧金额"),
            // entity.asset.monthlydepreciation
            new TranslationSeedItem("entity.asset.monthlydepreciation", "zh-CN", "每月折旧金额", "每月折旧金额"),
            // entity.asset.monthlydepreciation
            new TranslationSeedItem("entity.asset.monthlydepreciation", "zh-HK", "每月折旧金额_hk", "每月折旧金额"),

            // entity.asset.status
            new TranslationSeedItem("entity.asset.status", "en-US", "资产状态_us", "资产状态（字典 accounting_financial_asset_status：0=未使用，1=使用中，2=报废，3=处置，4=实物不存在）"),
            // entity.asset.status
            new TranslationSeedItem("entity.asset.status", "ja-JP", "资产状态_jp", "资产状态（字典 accounting_financial_asset_status：0=未使用，1=使用中，2=报废，3=处置，4=实物不存在）"),
            // entity.asset.status
            new TranslationSeedItem("entity.asset.status", "zh-CN", "资产状态", "资产状态（字典 accounting_financial_asset_status：0=未使用，1=使用中，2=报废，3=处置，4=实物不存在）"),
            // entity.asset.status
            new TranslationSeedItem("entity.asset.status", "zh-HK", "资产状态_hk", "资产状态（字典 accounting_financial_asset_status：0=未使用，1=使用中，2=报废，3=处置，4=实物不存在）"),
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
        translation.ResourceGroup = "Financial";
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
