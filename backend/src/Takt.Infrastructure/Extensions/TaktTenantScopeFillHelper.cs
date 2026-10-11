// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Extensions
// 文件名称：TaktTenantScopeFillHelper.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Cursor AI)
// 功能描述：租户级实体创建/种子时注入 RelatedPlant（TaktCompany 强制按 Database 同序映射覆盖）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Interfaces;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Extensions;

/// <summary>
/// 租户上下文写入辅助：对组合 1/3（ITaktHasRelatedPlant）按 Database 同序映射写入 RelatedPlant。
/// </summary>
public static class TaktTenantScopeFillHelper
{
    /// <summary>
    /// 按 Database 同序映射写入 RelatedPlant（异步包装）。
    /// </summary>
    /// <param name="entity">租户级实体</param>
    /// <param name="database">Database 配置（CompanyCodes↔PlantCodes）</param>
    public static Task ApplyRelatedPlantAsync(ITaktTenantEntity entity, TaktDatabaseOptions database)
    {
        ApplyRelatedPlant(entity, database);
        return Task.CompletedTask;
    }

    /// <summary>
    /// 按 Database 同序映射写入 RelatedPlant。
    /// TaktCompany：始终按 CompanyCode↔PlantCodes 强制覆盖；
    /// 其它 ITaktHasRelatedPlant：仅当前为空时补默认公司对应工厂。
    /// </summary>
    /// <param name="entity">租户级实体</param>
    /// <param name="database">Database 配置（CompanyCodes↔PlantCodes）</param>
    public static void ApplyRelatedPlant(ITaktTenantEntity entity, TaktDatabaseOptions database)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(database);
        if (entity is not ITaktHasRelatedPlant plantEntity)
        {
            return;
        }

        database.NormalizeAndValidate();
        if (entity is TaktCompany company)
        {
            if (string.IsNullOrWhiteSpace(company.CompanyCode))
            {
                return;
            }

            // 公司主档：关联工厂必须以 Database 同序映射为准（强制覆盖，纠正历史错误值）
            plantEntity.RelatedPlant = database.GetPlantCodeForCompanyCode(company.CompanyCode);
            return;
        }

        if (!string.IsNullOrWhiteSpace(plantEntity.RelatedPlant))
        {
            return;
        }

        plantEntity.RelatedPlant = database.GetPlantCodeForCompanyCode(database.GetSeedCompanyCode());
    }
}
