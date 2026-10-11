// ========================================
// 项目名称:节拍智控·TaktWise
// 命名空间:Takt.Infrastructure.Data.Seeds
// 文件名称:TaktUserSeedData.cs
// 创建时间:2025-01-20
// 创建人:Takt365(Cursor AI)
// 功能描述:用户种子数据初始化(跨租户:DEV/QAS/PRD)
// 
// 版权信息:Copyright (c) 2025 Takt  All rights reserved.
// 免责声明:此软件使用 MIT License,作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.HumanResource.Personnel;
using Takt.Domain.Entities.Identity;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// 用户种子数据初始化
/// 幂等性操作:存在则更新,不存在则创建
/// 默认密码从配置 PasswordPolicy:DefaultPassword 读取
/// </summary>
public class TaktUserSeedData : ITaktSeedDataCoordinator
{
    /// <summary>
    /// 执行顺序（在员工档案之后，角色之前）
    /// </summary>
    public int Order => 15;

    /// <summary>
    /// 初始化用户种子数据
    /// 注意：每个数据库（租户）只初始化自己的用户，不跨数据库
    /// Program.cs 会为每个租户数据库调用此方法，因此只需为当前租户初始化用户
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（由协调器传入）</param>
    /// <returns>返回插入和更新的记录数(插入数, 更新数)</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化用户种子数据...");

        // 参数验证：必须使用协调器传入的租户编码
        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过用户种子数据初始化");
            return (0, 0);
        }

        var userRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktUser>>();
        var employeeRepository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktEmployee>>();

        int insertCount = 0;
        int updateCount = 0;

        // 使用项目统一的密码哈希方法(PB KDF2)
        var defaultPasswordHash = TaktEncryptHelper.HashPassword("Takt@123456");

        // 定义标准用户列表（admin, guest, demo）
        var standardUsers = new[]
        {
            new { UserName = "admin", NickNameSuffix = "管理员", EmployeeCode = "900001" },
            new { UserName = "guest", NickNameSuffix = "访客", EmployeeCode = "900002" },
            new { UserName = "demo", NickNameSuffix = "演示用户", EmployeeCode = "900003" }
        };

        TaktLogger.Information("正在为租户 {TenantCode} 初始化用户...", tenantCode);

        // 为当前租户初始化3个标准用户
        foreach (var userData in standardUsers)
        {
            var NickName = $"{userData.UserName}{userData.NickNameSuffix}";
            var (user, i, u) = await CreateOrUpdateUserAsync(
                userRepository, 
                employeeRepository, 
                tenantCode, 
                userData.UserName, 
                NickName, 
                userData.EmployeeCode, 
                defaultPasswordHash);
            
            insertCount += i;
            updateCount += u;
        }
        
        TaktLogger.Information("租户 {TenantCode} 用户初始化完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", 
            tenantCode, insertCount, updateCount);

        TaktLogger.Information("用户种子数据初始化完成: 插入 {InsertCount} 条,更新 {UpdateCount} 条", insertCount, updateCount);

        return (insertCount, updateCount);
    }

    /// <summary>
    /// 创建或更新用户
    /// </summary>
    private static async Task<(TaktUser User, int InsertCount, int UpdateCount)> CreateOrUpdateUserAsync(
        ITaktTenantSeedRepository<TaktUser> userRepository,
        ITaktCompanySeedRepository<TaktEmployee> employeeRepository,
        string tenantCode,
        string UserName,
        string NickName,
        string EmployeeCode,
        string passwordHash)
    {
        // 根据员工编码查找员工主键ID
        var employee = await employeeRepository.FirstAsync(e => e.TenantCode == tenantCode && e.EmployeeCode == EmployeeCode);
        if (employee == null)
        {
            throw new InvalidOperationException($"租户 {tenantCode} 中未找到员工编码 {EmployeeCode} 的员工档案");
        }

        var user = await userRepository.FirstAsync(u => u.TenantCode == tenantCode && u.UserName == UserName);
        
        if (user == null)
        {
            // 不存在:创建新记录(仓储会自动生成雪花ID和审计字段)
            user = new TaktUser
            {
                TenantCode = tenantCode,
                UserName = UserName,
                NickName = NickName,
                UserType = UserName == "admin" ? 2 : 0,
                PasswordHash = passwordHash,
                EmployeeId = employee.Id,
                IsBuiltIn = 1,
                UserStatus = 1,
                PasswordExpireDays = UserName == "admin" ? 90 : 30,
                LoginCount = 0,
                LoginFailCount = 0,
                CultureCode = "en-US"
            };
            user = await userRepository.CreateAsync(user);
            return (user, 1, 0);
        }
        else
        {
            // 存在:更新记录
            user.NickName = NickName;
            user.UserType = UserName == "admin" ? 2 : 0;
            user.EmployeeId = employee.Id;
            user.IsBuiltIn = 1;
            user.UserStatus = 1;
            user.PasswordExpireDays = UserName == "admin" ? 90 : 30;
            user.CultureCode = "en-US";

            await userRepository.UpdateAsync(user);
            return (user, 0, 1);
        }
    }
}
