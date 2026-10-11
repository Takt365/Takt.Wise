// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Extensions
// 文件名称：TaktCompanyScopeFillHelper.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Cursor AI)
// 功能描述：公司级/审批级实体创建时注入 CultureCode、PlantCode（均仅 Database 同序映射：Company↔Plant↔Culture）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using SqlSugar;
using Takt.Domain.Entities;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Extensions;

/// <summary>
/// 公司上下文写入辅助：CultureCode / PlantCode 空值时严格按 Database 同序映射注入。
/// </summary>
public static class TaktCompanyScopeFillHelper
{
    /// <summary>
    /// 按配置映射写入 CultureCode / PlantCode（不再依赖公司主档读 CultureCode）。
    /// </summary>
    /// <param name="db">SqlSugar 客户端（保留签名以兼容仓储调用；本方法不读库）</param>
    /// <param name="entity">公司级或审批级实体</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="database">Database 配置（CompanyCodes↔PlantCodes↔CultureCodes）</param>
    public static Task ApplyCompanyScopeFromMasterAsync(
        ISqlSugarClient db,
        object entity,
        string tenantCode,
        string companyCode,
        TaktDatabaseOptions database)
    {
        ArgumentNullException.ThrowIfNull(db);
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(database);
        if (string.IsNullOrWhiteSpace(tenantCode) || string.IsNullOrWhiteSpace(companyCode))
        {
            return Task.CompletedTask;
        }
        database.NormalizeAndValidate();
        ApplyCompanyScope(
            entity,
            database.GetCultureCodeForCompanyCode(companyCode),
            database.GetPlantCodeForCompanyCode(companyCode));
        return Task.CompletedTask;
    }

    /// <summary>
    /// 从 IConfiguration 的 Database 节解析公司↔工厂↔文化映射后写入实体（旁路仓储的日志写入路径使用）。
    /// </summary>
    /// <param name="entity">公司级或审批级实体</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="configuration">应用配置</param>
    public static void ApplyCompanyScopeFromConfiguration(
        object entity,
        string tenantCode,
        string companyCode,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(configuration);
        if (string.IsNullOrWhiteSpace(tenantCode) || string.IsNullOrWhiteSpace(companyCode))
        {
            return;
        }

        var database = configuration.RequireDatabase();
        ApplyCompanyScope(
            entity,
            database.GetCultureCodeForCompanyCode(companyCode),
            database.GetPlantCodeForCompanyCode(companyCode));
    }

    /// <summary>
    /// 写入 CultureCode；PlantCode 仅在当前为空时用配置映射的工厂编码填充。
    /// </summary>
    /// <param name="entity">公司级或审批级实体</param>
    /// <param name="cultureCode">配置映射得到的 CultureCode</param>
    /// <param name="relatedPlant">配置映射得到的工厂编码（用作默认 PlantCode）</param>
    public static void ApplyCompanyScope(object entity, string cultureCode, string relatedPlant)
    {
        ArgumentNullException.ThrowIfNull(entity);
        var plant = (relatedPlant ?? string.Empty).Trim();
        switch (entity)
        {
            case TaktCompanyEntityBase companyEntity:
                if (!string.IsNullOrWhiteSpace(cultureCode))
                {
                    companyEntity.CultureCode = cultureCode.Trim();
                }
                if (string.IsNullOrWhiteSpace(companyEntity.PlantCode) && plant.Length > 0)
                {
                    companyEntity.PlantCode = plant;
                }
                break;
            case TaktApprovalEntityBase approvalEntity:
                if (!string.IsNullOrWhiteSpace(cultureCode))
                {
                    approvalEntity.CultureCode = cultureCode.Trim();
                }
                if (string.IsNullOrWhiteSpace(approvalEntity.PlantCode) && plant.Length > 0)
                {
                    approvalEntity.PlantCode = plant;
                }
                break;
            case TaktCompanyEntityGuidBase companyGuid:
                if (!string.IsNullOrWhiteSpace(cultureCode))
                {
                    companyGuid.CultureCode = cultureCode.Trim();
                }
                if (string.IsNullOrWhiteSpace(companyGuid.PlantCode) && plant.Length > 0)
                {
                    companyGuid.PlantCode = plant;
                }
                break;
            case TaktApprovalEntityGuidBase approvalGuid:
                if (!string.IsNullOrWhiteSpace(cultureCode))
                {
                    approvalGuid.CultureCode = cultureCode.Trim();
                }
                if (string.IsNullOrWhiteSpace(approvalGuid.PlantCode) && plant.Length > 0)
                {
                    approvalGuid.PlantCode = plant;
                }
                break;
            case TaktCompanyEntityIncrementBase companyIncrement:
                if (!string.IsNullOrWhiteSpace(cultureCode))
                {
                    companyIncrement.CultureCode = cultureCode.Trim();
                }
                if (string.IsNullOrWhiteSpace(companyIncrement.PlantCode) && plant.Length > 0)
                {
                    companyIncrement.PlantCode = plant;
                }
                break;
            case TaktApprovalEntityIncrementBase approvalIncrement:
                if (!string.IsNullOrWhiteSpace(cultureCode))
                {
                    approvalIncrement.CultureCode = cultureCode.Trim();
                }
                if (string.IsNullOrWhiteSpace(approvalIncrement.PlantCode) && plant.Length > 0)
                {
                    approvalIncrement.PlantCode = plant;
                }
                break;
            case TaktCompanyEntityScopeBase companyScope:
                if (!string.IsNullOrWhiteSpace(cultureCode))
                {
                    companyScope.CultureCode = cultureCode.Trim();
                }
                break;
            case TaktApprovalEntityScopeBase approvalScope:
                if (!string.IsNullOrWhiteSpace(cultureCode))
                {
                    approvalScope.CultureCode = cultureCode.Trim();
                }
                break;
            default:
                return;
        }
    }
}
