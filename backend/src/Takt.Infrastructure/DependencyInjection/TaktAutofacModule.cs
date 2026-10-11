// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.DependencyInjection
// 文件名称：TaktAutofacModule.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Autofac依赖注入模块，自动扫描并注册服务
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Services.Workflow.FlowEngine.Business;
using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Reflection;
using Takt.Domain.Entities;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Infrastructure.Data.Context;
using Takt.Infrastructure.Repositories;
using Takt.Infrastructure.Services;
using Takt.Infrastructure.Validation;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.DependencyInjection;

/// <summary>
/// Autofac依赖注入模块
/// 完全自动扫描并注册服务，无需硬编码任何类型
/// </summary>
public class TaktAutofacModule : Autofac.Module
{
    /// <summary>
    /// 加载模块，自动扫描并注册所有服务
    /// </summary>
    /// <param name="builder">容器构建器</param>
    protected override void Load(ContainerBuilder builder)
    {
        // ========================================
        // 1. 自动扫描并注册所有程序集
        // ========================================
        
        // 获取所有 Takt.* 程序集
        var assemblies = GetTaktAssemblies();

        // ========================================
        // 2. 注册泛型仓储（Repository<T>）
        // 规则：三个独立的仓储接口，对应三个实体基类
        // 生命周期：Scoped（每个请求一个实例）
        // ========================================
        
        // 2.1 租户级仓储：ITaktTenantRepository<T> -> TaktTenantRepository<T>
        builder.RegisterGeneric(typeof(TaktTenantRepository<>))
            .As(typeof(ITaktTenantRepository<>))
            .InstancePerLifetimeScope();

        // 2.2 公司级仓储：ITaktCompanyRepository<T> -> TaktCompanyRepository<T>
        builder.RegisterType<TaktUserCompanyRepository>()
            .As<ITaktCompanyRepository<Takt.Domain.Entities.Identity.TaktUserCompany>>()
            .InstancePerLifetimeScope();
        builder.RegisterType<TaktRoleCompanyRepository>()
            .As<ITaktCompanyRepository<Takt.Domain.Entities.Identity.TaktRoleCompany>>()
            .InstancePerLifetimeScope();
        builder.RegisterType<TaktUserRoleRepository>()
            .As<ITaktCompanyRepository<Takt.Domain.Entities.Identity.TaktUserRole>>()
            .InstancePerLifetimeScope();
        builder.RegisterType<TaktRoleMenuRepository>()
            .As<ITaktCompanyRepository<Takt.Domain.Entities.Identity.TaktRoleMenu>>()
            .InstancePerLifetimeScope();
        builder.RegisterType<TaktUserTenantRepository>()
            .As<ITaktCompanyRepository<Takt.Domain.Entities.Identity.TaktUserTenant>>()
            .InstancePerLifetimeScope();
        builder.RegisterType<TaktEmployeeProfileResolver>()
            .As<ITaktEmployeeProfileResolver>()
            .InstancePerLifetimeScope();
        builder.RegisterGeneric(typeof(TaktCompanyRepository<>))
            .As(typeof(ITaktCompanyRepository<>))
            .InstancePerLifetimeScope();

        // 2.3 审批级仓储：ITaktApprovalRepository<T> -> TaktApprovalRepository<T>
        builder.RegisterGeneric(typeof(TaktApprovalRepository<>))
            .As(typeof(ITaktApprovalRepository<>))
            .InstancePerLifetimeScope();

        // ========================================
        // 2.1 注册种子数据专用仓储（SeedRepository<T>）
        // 规则：三个独立的种子仓储接口，对应三个实体基类
        // 生命周期：Scoped（每个请求一个实例）
        // 特点：绕过用户上下文，支持精确租户控制
        // ========================================
        
        // 2.1.1 租户级种子仓储：ITaktTenantSeedRepository<T> -> TaktTenantSeedRepository<T>
        builder.RegisterGeneric(typeof(TaktTenantSeedRepository<>))
            .As(typeof(ITaktTenantSeedRepository<>))
            .InstancePerLifetimeScope();

        // 2.1.2 公司级种子仓储：ITaktCompanySeedRepository<T> -> TaktCompanySeedRepository<T>
        builder.RegisterGeneric(typeof(TaktCompanySeedRepository<>))
            .As(typeof(ITaktCompanySeedRepository<>))
            .InstancePerLifetimeScope();

        // 2.1.3 审批级种子仓储：ITaktApprovalSeedRepository<T> -> TaktApprovalSeedRepository<T>
        builder.RegisterGeneric(typeof(TaktApprovalSeedRepository<>))
            .As(typeof(ITaktApprovalSeedRepository<>))
            .InstancePerLifetimeScope();

        // ========================================
        // 2.4 注册唯一性验证器（实现 Domain 层的 ITaktUniqueValidator）
        // ========================================
        builder.RegisterType<TaktUniqueValidator>()
            .As<ITaktUniqueValidator>()
            .InstancePerLifetimeScope();

        builder.RegisterType<TaktVocabularyFilter>()
            .As<ITaktVocabularyFilter>()
            .InstancePerLifetimeScope();

        // ========================================
        // 2.5 注册基础设施工具（非 *Service 自动扫描命名，需显式注册）
        // ========================================
        builder.RegisterType<TaktSqlExecutor>()
            .As<ITaktSqlExecutor>()
            .InstancePerLifetimeScope();

        builder.RegisterType<TaktStatQueryExecutor>()
            .As<ITaktStatQueryExecutor>()
            .InstancePerLifetimeScope();

        builder.RegisterType<Takt.Application.Services.Logistics.Manufacturing.EngineeringChange.TaktEcExecDeptAccess>()
            .InstancePerLifetimeScope();

        builder.RegisterType<Takt.Application.Services.Logistics.Manufacturing.EngineeringChange.TaktEcExecPersistence>()
            .InstancePerLifetimeScope();
        builder.RegisterType<Takt.Application.Services.Logistics.Manufacturing.EngineeringChange.TaktEcGijutsuStatusSynchronizer>()
            .InstancePerLifetimeScope();
        builder.RegisterType<Takt.Application.Services.Logistics.Manufacturing.EngineeringChange.TaktEcHandoffMailService>()
            .InstancePerLifetimeScope();
        builder.RegisterType<Takt.Application.Services.Logistics.Manufacturing.EngineeringChange.TaktEcScopeExecOrchestrator>()
            .InstancePerLifetimeScope();

        builder.RegisterType<Takt.Application.Services.Logistics.Sales.TaktSalesPriceTrendMonthlyAnalysisBuilder>()
            .As<Takt.Application.Services.Logistics.Sales.ITaktSalesPriceTrendMonthlyAnalysisBuilder>()
            .InstancePerLifetimeScope();

        builder.RegisterType<Takt.Application.Services.Logistics.Procurement.TaktPurchasePriceTrendMonthlyAnalysisBuilder>()
            .As<Takt.Application.Services.Logistics.Procurement.ITaktPurchasePriceTrendMonthlyAnalysisBuilder>()
            .InstancePerLifetimeScope();

        builder.RegisterType<Takt.Application.Services.Logistics.Materials.TaktInboundActualTrendAnalysisBuilder>()
            .As<Takt.Application.Services.Logistics.Materials.ITaktInboundActualTrendAnalysisBuilder>()
            .InstancePerLifetimeScope();

        builder.RegisterType<Takt.Application.Services.Logistics.Materials.TaktOutboundActualTrendAnalysisBuilder>()
            .As<Takt.Application.Services.Logistics.Materials.ITaktOutboundActualTrendAnalysisBuilder>()
            .InstancePerLifetimeScope();

        builder.RegisterType<Takt.Application.Services.Logistics.Procurement.TaktPurchaseActualTrendAnalysisBuilder>()
            .As<Takt.Application.Services.Logistics.Procurement.ITaktPurchaseActualTrendAnalysisBuilder>()
            .InstancePerLifetimeScope();

        builder.RegisterType<Takt.Application.Services.Logistics.Sales.TaktSalesActualTrendAnalysisBuilder>()
            .As<Takt.Application.Services.Logistics.Sales.ITaktSalesActualTrendAnalysisBuilder>()
            .InstancePerLifetimeScope();

        builder.RegisterType<TaktLineNumberGenerator>()
            .As<ITaktLineNumberGenerator>()
            .SingleInstance();

        builder.RegisterType<TaktSortOrderGenerator>()
            .As<ITaktSortOrderGenerator>()
            .SingleInstance();

        builder.RegisterType<TaktNumberingGenerator>()
            .As<ITaktNumberingGenerator>()
            .InstancePerLifetimeScope();

        builder.RegisterType<Takt.Infrastructure.Services.TaktFileUploadEngine>()
            .As<Takt.Domain.Interfaces.ITaktFileUploadEngine>()
            .InstancePerLifetimeScope();

        builder.RegisterType<Takt.Infrastructure.Data.Schema.TaktDatabaseSchemaProvider>()
            .As<Takt.Domain.Interfaces.ITaktDatabaseSchemaProvider>()
            .InstancePerLifetimeScope();

        builder.RegisterType<Takt.Application.Services.Code.Generator.GenEngine.TaktGenEngine>()
            .As<Takt.Application.Services.Code.Generator.GenEngine.ITaktGenEngine>()
            .InstancePerLifetimeScope();

        builder.RegisterType<Takt.Infrastructure.Data.Schema.TaktTableCloneProvider>()
            .As<Takt.Domain.Interfaces.ITaktTableCloneProvider>()
            .InstancePerLifetimeScope();

        builder.RegisterType<Takt.Infrastructure.Data.Schema.TaktDataCloneProvider>()
            .As<Takt.Domain.Interfaces.ITaktDataCloneProvider>()
            .InstancePerLifetimeScope();

        builder.RegisterType<Takt.Infrastructure.Data.Schema.TaktTableArchiveProvider>()
            .As<Takt.Domain.Interfaces.ITaktTableArchiveProvider>()
            .InstancePerLifetimeScope();

        builder.RegisterType<Takt.Infrastructure.Data.Schema.TaktDatabaseBackupProvider>()
            .As<Takt.Domain.Interfaces.ITaktDatabaseBackupProvider>()
            .InstancePerLifetimeScope();

        builder.RegisterType<Takt.Application.Services.Workflow.FlowEngine.TaktFlowApproverResolverService>()
            .AsSelf()
            .InstancePerLifetimeScope();

        builder.RegisterType<Takt.Application.Services.Workflow.FlowEngine.Business.TaktApprovalFlowBusinessService>()
            .AsSelf()
            .InstancePerLifetimeScope();

        builder.RegisterType<Takt.Application.Services.Workflow.FlowEngine.Business.TaktApprovalFlowSubmitService>()
            .AsSelf()
            .InstancePerLifetimeScope();

        builder.RegisterType<Takt.Infrastructure.Services.TaktApprovalFlowDataGateway>()
            .As<Takt.Domain.Interfaces.ITaktApprovalFlowDataGateway>()
            .InstancePerLifetimeScope();

        builder.RegisterType<Takt.Infrastructure.Quartz.TaktQuartzSchedulerManager>()
            .As<ITaktQuartzSchedulerManager>()
            .InstancePerLifetimeScope();

        builder.RegisterType<Takt.Infrastructure.Quartz.TaktQuartzJobSignalRPushService>()
            .As<ITaktQuartzJobSignalRPushService>()
            .InstancePerLifetimeScope();

        builder.RegisterType<Takt.Application.Services.Foundation.TaktQuartzSignalRNotifier>()
            .As<Takt.Application.Services.Foundation.ITaktQuartzSignalRNotifier>()
            .InstancePerLifetimeScope();

        builder.RegisterType<Takt.Application.Services.Workflow.TaktWorkflowSignalRNotifier>()
            .As<Takt.Application.Services.Workflow.ITaktWorkflowSignalRNotifier>()
            .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(assemblies)
            .Where(t => t.IsClass && !t.IsAbstract && typeof(ITaktQuartzJobHandler).IsAssignableFrom(t))
            .As<ITaktQuartzJobHandler>()
            .InstancePerLifetimeScope();

        // ========================================
        // 3. 自动注册具体仓储（Repository）
        // 规则：类名以 Repository 结尾，注册为接口
        // 生命周期：Scoped（每个请求一个实例）
        // ========================================
        builder.RegisterAssemblyTypes(assemblies)
            .Where(t => IsRepository(t))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        // ========================================
        // 4. 自动注册应用服务（Service）
        // 规则：类名以 Service 结尾，注册为接口
        // 生命周期：Scoped（每个请求一个实例）
        // ========================================
        builder.RegisterAssemblyTypes(assemblies)
            .Where(t => IsService(t))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        // ========================================
        // 5. 自动注册验证器（Validator）
        // 规则：类名以 Validator 结尾，注册为接口
        // 生命周期：Scoped（每个请求一个实例）
        // ========================================
        builder.RegisterAssemblyTypes(assemblies)
            .Where(t => IsValidator(t))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        // ========================================
        // 6. 自动注册事件处理器（EventHandler）
        // 规则：类名以 EventHandler 结尾，注册为接口
        // 生命周期：Scoped（每个请求一个实例）
        // ========================================
        builder.RegisterAssemblyTypes(assemblies)
            .Where(t => IsEventHandler(t))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        // ========================================
        // 7. 自动注册配置类（Options）
        // 规则：类名以 Options 结尾，注册为自身
        // 生命周期：Singleton（单例）
        // ========================================
        builder.RegisterAssemblyTypes(assemblies)
            .Where(t => IsOptions(t))
            .AsSelf()
            .SingleInstance();

        // ========================================
        // 8. 自动注册中间件（Middleware）
        // 规则：类名以 Middleware 结尾，注册为自身
        // 生命周期：Scoped（每个请求一个实例）
        // ========================================
        builder.RegisterAssemblyTypes(assemblies)
            .Where(t => IsMiddleware(t))
            .AsSelf()
            .InstancePerLifetimeScope();

        // ========================================
        // 9. 自动注册种子数据协调器（SeedData）
        // 规则：类名以 SeedData 结尾且实现 ITaktSeedDataCoordinator
        // 生命周期：Scoped（每次执行一个实例）
        // ========================================
        builder.RegisterAssemblyTypes(assemblies)
            .Where(t => IsSeedData(t))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        // ========================================
        // 10. 自动注册帮助类（Helper）
        // 规则：类名以 Helper 结尾，注册为自身
        // 生命周期：Singleton（单例）
        // ========================================
        builder.RegisterAssemblyTypes(assemblies)
            .Where(t => IsHelper(t))
            .AsSelf()
            .SingleInstance();

        // ========================================
        // 11. 注册 SqlSugar 上下文（HTTP 请求）
        // TaktSqlSugarContext 用于 HTTP 请求处理
        // 特点：依赖 HttpContextAccessor 获取用户、租户、公司信息
        // 生命周期：Scoped（每个请求一个实例）
        // ========================================
        builder.Register(ctx =>
        {
            var configuration = ctx.Resolve<IConfiguration>();
            var httpContextAccessor = ctx.ResolveOptional<IHttpContextAccessor>();
            var tenantOptions = ctx.Resolve<IOptions<TaktTenantContextOptions>>().Value;
            var tenantCode = ResolveRequestTenantCode(httpContextAccessor, tenantOptions);
            if (string.IsNullOrWhiteSpace(tenantCode))
            {
                tenantCode = configuration.RequireDatabase().GetSeedTenantCode();
            }
            return new TaktSqlSugarContext(configuration, tenantCode, httpContextAccessor);
        })
        .AsSelf()
        .InstancePerLifetimeScope();
        
        // ========================================
        // 12. 注册种子数据专用上下文
        // TaktSeedContext 用于种子数据初始化
        // 特点：不依赖 HTTP 上下文，支持动态切换租户
        // 生命周期：Scoped（每次种子执行一个实例）
        // ========================================
        builder.Register(ctx =>
        {
            var configuration = ctx.Resolve<IConfiguration>();
            var bootstrapTenantCode = configuration.RequireDatabase().GetSeedTenantCode();
            return new Takt.Infrastructure.Data.Context.TaktSeedContext(configuration, bootstrapTenantCode);
        })
        .AsSelf()
        .InstancePerLifetimeScope();

        // ========================================
        // 13. 认证登录日志租户库写入（显式 Tenant_{code} 连接，不依赖请求 Scoped 上下文）
        // ========================================
        builder.RegisterType<Takt.Infrastructure.Services.TaktLoginLogTenantWriter>()
            .As<Takt.Domain.Interfaces.ITaktLoginLogTenantWriter>()
            .InstancePerLifetimeScope();

        builder.RegisterType<Takt.Infrastructure.Services.TaktVisitLogTenantWriter>()
            .As<Takt.Domain.Interfaces.ITaktVisitLogTenantWriter>()
            .InstancePerLifetimeScope();

        TaktLogger.Information("[Autofac] 自动注册服务完成");
    }

    /// <summary>
    /// 获取所有 Takt.* 程序集
    /// </summary>
    private static Assembly[] GetTaktAssemblies()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.FullName?.StartsWith("Takt.") == true)
            .ToArray();
    }

    /// <summary>
    /// 判断是否为仓储类
    /// </summary>
    private static bool IsRepository(Type type) => 
        type.IsClass && !type.IsAbstract && type.Name.EndsWith("Repository");

    /// <summary>
    /// 判断是否为应用服务类（排除已由 MS DI 以 Singleton 注册的 TaktCacheService，避免每请求独立内存缓存导致登录票据跨请求失效）
    /// </summary>
    private static bool IsService(Type type) =>
        type.IsClass && !type.IsAbstract && type.Name.EndsWith("Service")
        && type != typeof(TaktCacheService);

    /// <summary>
    /// 判断是否为验证器类（排除 FluentValidation AbstractValidator，由 MS DI 注册）
    /// </summary>
    private static bool IsValidator(Type type) =>
        type.IsClass && !type.IsAbstract && type.Name.EndsWith("Validator")
        && type != typeof(TaktUniqueValidator)
        && !IsFluentValidationAbstractValidator(type);

    /// <summary>
    /// 是否为 FluentValidation 的 AbstractValidator 派生类
    /// </summary>
    private static bool IsFluentValidationAbstractValidator(Type type)
    {
        var current = type.BaseType;
        while (current != null)
        {
            if (current.IsGenericType)
            {
                var def = current.GetGenericTypeDefinition();
                if (def.FullName?.StartsWith("FluentValidation.AbstractValidator", StringComparison.Ordinal) == true)
                {
                    return true;
                }
            }

            current = current.BaseType;
        }

        return false;
    }

    /// <summary>
    /// 判断是否为事件处理器类
    /// </summary>
    private static bool IsEventHandler(Type type) => 
        type.IsClass && !type.IsAbstract && type.Name.EndsWith("EventHandler");

    /// <summary>
    /// 判断是否为配置类
    /// </summary>
    private static bool IsOptions(Type type) => 
        type.IsClass && !type.IsAbstract && type.Name.EndsWith("Options");

    /// <summary>
    /// 判断是否为中间件类
    /// </summary>
    private static bool IsMiddleware(Type type) => 
        type.IsClass && !type.IsAbstract && type.Name.EndsWith("Middleware");

    /// <summary>
    /// 判断是否为种子数据类
    /// </summary>
    private static bool IsSeedData(Type type) => 
        type.IsClass && !type.IsAbstract && type.Name.EndsWith("SeedData");

    /// <summary>
    /// 判断是否为帮助类
    /// </summary>
    private static bool IsHelper(Type type) => 
        type.IsClass && !type.IsAbstract && type.Name.EndsWith("Helper");

    /// <summary>
    /// 从 HTTP 请求头或 JWT claim 解析租户编码（与 TaktUserContext 一致）
    /// </summary>
    /// <param name="httpContextAccessor">HTTP 上下文访问器</param>
    /// <param name="tenantOptions">租户上下文配置</param>
    /// <returns>租户编码；无请求上下文或未传租户时返回 null</returns>
    private static string? ResolveRequestTenantCode(
        IHttpContextAccessor? httpContextAccessor,
        TaktTenantContextOptions tenantOptions)
    {
        var httpContext = httpContextAccessor?.HttpContext;
        if (httpContext == null)
        {
            return null;
        }
        return TaktUserContext.TryResolveTenantCode(httpContext, tenantOptions.TenantHeaderName);
    }
}
