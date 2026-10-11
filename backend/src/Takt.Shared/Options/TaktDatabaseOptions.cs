// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Options
// 文件名称：TaktDatabaseOptions.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：数据库配置（按租户分库）及初始化编码解析
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Shared.Helpers;

namespace Takt.Shared.Options;

/// <summary>
/// 数据库配置（<c>appsettings Database</c> 节；绑定入口 TaktConfigurationExtensions.RequireDatabase）
/// </summary>
public class TaktDatabaseOptions
{
    public const string SectionName = "Database";

    /// <summary>
    /// 数据库类型（与 SqlSugar DbType 枚举整型值一致；见 TaktDatabaseTypeHelper 常量与 ResolveSugarDbType）
    /// </summary>
    public int DbType { get; set; }

    /// <summary>
    /// 已解析的 SqlSugar 数据库类型（NormalizeAndValidate 时由 DbType 映射一次并缓存）
    /// </summary>
    public SqlSugar.DbType? SugarDbType { get; private set; }

    /// <summary>
    /// 需要初始化的租户编码列表（顺序与 appsettings 一致）
    /// </summary>
    public List<string> TenantCodes { get; set; } = null!;

    /// <summary>
    /// 需要初始化的公司编码列表（与 <c>PlantCodes</c>/<c>CultureCodes</c> 同序一一对应：CompanyCodes[i]↔PlantCodes[i]↔CultureCodes[i]；唯一合法公司↔工厂↔区域文化映射源）
    /// </summary>
    public List<string> CompanyCodes { get; set; } = null!;

    /// <summary>
    /// 需要初始化的工厂编码列表（与 <c>CompanyCodes</c>/<c>CultureCodes</c> 同序一一对应）
    /// </summary>
    public List<string> PlantCodes { get; set; } = null!;

    /// <summary>
    /// 区域文化编码列表（与 <c>CompanyCodes</c>/<c>PlantCodes</c> 同序一一对应；BCP47 如 ja-JP、zh-CN、zh-HK）
    /// </summary>
    public List<string> CultureCodes { get; set; } = null!;

    /// <summary>
    /// 连接字符串模板（{TenantCode} 占位）
    /// </summary>
    public string ConnectionStringTemplate { get; set; } = null!;

    /// <summary>
    /// 规范化列表与种子编码后执行 Validate
    /// </summary>
    public void NormalizeAndValidate()
    {
        TenantCodes = NormalizeCodeList(TenantCodes);
        CompanyCodes = NormalizeCodeList(CompanyCodes);
        PlantCodes = NormalizeCodeList(PlantCodes);
        CultureCodes = NormalizeCodeList(CultureCodes);
        SugarDbType = TaktDatabaseTypeHelper.ResolveSugarDbType(DbType);
        Validate();
    }

    /// <summary>
    /// 默认租户（TenantCodes 首项；启动 DI 引导、演示主公司等）
    /// </summary>
    /// <returns>默认租户编码</returns>
    public string GetSeedTenantCode() => TenantCodes[0];

    /// <summary>
    /// 默认公司（CompanyCodes 首项；演示账号主公司、请假工作流演示范围等）
    /// </summary>
    /// <returns>默认公司编码</returns>
    public string GetSeedCompanyCode() => CompanyCodes[0];

    /// <summary>
    /// 默认区域文化（CultureCodes 首项；与默认公司同序）
    /// </summary>
    /// <returns>默认 CultureCode</returns>
    public string GetSeedCultureCode() => CultureCodes[0];

    /// <summary>
    /// 解析公司编码在同序映射中的下标（未找到则抛异常）
    /// </summary>
    /// <param name="companyCode">公司编码</param>
    /// <returns>下标</returns>
    private int IndexOfCompanyCode(string companyCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(companyCode);
        var normalized = companyCode.Trim();
        var index = CompanyCodes.FindIndex(code => string.Equals(code, normalized, StringComparison.OrdinalIgnoreCase));
        if (index < 0)
        {
            throw new InvalidOperationException($"Database:CompanyCodes 中未找到公司编码 {normalized}");
        }
        return index;
    }

    /// <summary>
    /// 解析工厂编码在同序映射中的下标（未找到则抛异常）
    /// </summary>
    /// <param name="plantCode">工厂编码</param>
    /// <returns>下标</returns>
    private int IndexOfPlantCode(string plantCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(plantCode);
        var normalized = plantCode.Trim();
        var index = PlantCodes.FindIndex(code => string.Equals(code, normalized, StringComparison.OrdinalIgnoreCase));
        if (index < 0)
        {
            throw new InvalidOperationException($"Database:PlantCodes 中未找到工厂编码 {normalized}");
        }
        return index;
    }

    /// <summary>
    /// 公司→工厂：按同序下标映射（唯一合法映射；禁止手写对照表）
    /// </summary>
    /// <param name="companyCode">公司编码（须在 CompanyCodes 中）</param>
    /// <returns>同下标工厂编码</returns>
    public string GetPlantCodeForCompanyCode(string companyCode) => PlantCodes[IndexOfCompanyCode(companyCode)];

    /// <summary>
    /// 公司→区域文化：按同序下标映射（唯一合法映射；禁止手写对照表）
    /// </summary>
    /// <param name="companyCode">公司编码（须在 CompanyCodes 中）</param>
    /// <returns>同下标 CultureCode</returns>
    public string GetCultureCodeForCompanyCode(string companyCode) => CultureCodes[IndexOfCompanyCode(companyCode)];

    /// <summary>
    /// 工厂→公司：按同序下标映射（唯一合法映射；禁止手写对照表）
    /// </summary>
    /// <param name="plantCode">工厂编码（须在 PlantCodes 中）</param>
    /// <returns>同下标公司编码</returns>
    public string GetCompanyCodeForPlantCode(string plantCode) => CompanyCodes[IndexOfPlantCode(plantCode)];

    /// <summary>
    /// 工厂→区域文化：按同序下标映射（唯一合法映射；禁止手写对照表）
    /// </summary>
    /// <param name="plantCode">工厂编码（须在 PlantCodes 中）</param>
    /// <returns>同下标 CultureCode</returns>
    public string GetCultureCodeForPlantCode(string plantCode) => CultureCodes[IndexOfPlantCode(plantCode)];

    /// <summary>
    /// 获取已解析的 SqlSugar 数据库类型（未缓存时按 DbType 配置即时映射）
    /// </summary>
    /// <returns>SqlSugar DbType</returns>
    public SqlSugar.DbType GetSugarDbType()
    {
        EnsureSugarDbTypeResolved();
        return SugarDbType!.Value;
    }

    /// <summary>
    /// 根据租户编码获取连接字符串
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>连接字符串</returns>
    public string GetConnectionString(string tenantCode)
    {
        return ConnectionStringTemplate.Replace("{TenantCode}", tenantCode);
    }

    /// <summary>
    /// 创建 SqlSugar 客户端配置
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>ConnectionConfig</returns>
    public ConnectionConfig CreateConnectionConfig(string tenantCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        var trimmed = tenantCode.Trim();
        return TaktSqlSugarConnectionHelper.CreateConnectionConfig(
            GetSugarDbType(),
            trimmed,
            GetConnectionString(trimmed));
    }

    /// <summary>
    /// 验证配置（须先 NormalizeAndValidate 或保证列表已规范化）
    /// </summary>
    public void Validate()
    {
        EnsureSugarDbTypeResolved();
        if (TenantCodes.Count == 0)
        {
            throw new InvalidOperationException($"{SectionName}:TenantCodes 不能为空");
        }
        if (CompanyCodes.Count == 0)
        {
            throw new InvalidOperationException($"{SectionName}:CompanyCodes 不能为空");
        }
        if (PlantCodes.Count == 0)
        {
            throw new InvalidOperationException($"{SectionName}:PlantCodes 不能为空");
        }
        if (CultureCodes.Count == 0)
        {
            throw new InvalidOperationException($"{SectionName}:CultureCodes 不能为空");
        }
        if (CompanyCodes.Count != PlantCodes.Count || CompanyCodes.Count != CultureCodes.Count)
        {
            throw new InvalidOperationException(
                $"{SectionName}:CompanyCodes、PlantCodes、CultureCodes 数量须一致且同序一一对应（Company[i]↔Plant[i]↔Culture[i]）");
        }
        if (string.IsNullOrWhiteSpace(ConnectionStringTemplate))
        {
            throw new InvalidOperationException($"{SectionName}:ConnectionStringTemplate 未配置");
        }
    }

    /// <summary>
    /// 按配置编码顺序排列实体（仅保留配置中出现的项）
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <param name="configuredCodes">配置编码列表</param>
    /// <param name="items">待排序实体</param>
    /// <param name="codeSelector">从实体读取编码</param>
    /// <returns>按配置顺序排列的实体列表</returns>
    public static IReadOnlyList<T> OrderByConfiguredCodes<T>(
        IReadOnlyList<string> configuredCodes,
        IEnumerable<T> items,
        Func<T, string> codeSelector)
    {
        ArgumentNullException.ThrowIfNull(configuredCodes);
        ArgumentNullException.ThrowIfNull(items);
        ArgumentNullException.ThrowIfNull(codeSelector);
        var itemMap = items.ToDictionary(codeSelector, StringComparer.Ordinal);
        var ordered = new List<T>(configuredCodes.Count);
        foreach (var code in configuredCodes)
        {
            if (itemMap.TryGetValue(code, out var item))
            {
                ordered.Add(item);
            }
        }
        return ordered;
    }

    /// <summary>
    /// 去空白并剔除空项
    /// </summary>
    /// <param name="codes">原始编码列表</param>
    /// <returns>规范化后的列表</returns>
    private static List<string> NormalizeCodeList(List<string>? codes)
    {
        if (codes == null || codes.Count == 0)
        {
            return [];
        }
        return codes
            .Where(code => !string.IsNullOrWhiteSpace(code))
            .Select(code => code.Trim())
            .ToList();
    }

    /// <summary>
    /// 将 DbType 配置映射为 SqlSugar 类型并缓存（幂等）
    /// </summary>
    private void EnsureSugarDbTypeResolved()
    {
        if (!SugarDbType.HasValue)
        {
            SugarDbType = TaktDatabaseTypeHelper.ResolveSugarDbType(DbType);
        }
    }

}
