// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi
// 文件名称：Program.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：应用程序入口，配置依赖注入、中间件、日志、认证等
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Autofac;
using Autofac.Extensions.DependencyInjection;
using Takt.WebApi.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using Takt.Infrastructure.Data.Context;
using Takt.Infrastructure.DependencyInjection;
using Takt.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Takt.Shared.Helpers;
using Takt.Shared.Options;
using Takt.Infrastructure.Extensions;
using Takt.WebApi.OpenIddict;

// ========================================
// 1. 配置 Serilog 全局日志
// ========================================
var bootstrapConfiguration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

Log.Logger = TaktLoggingCollectionExtensions.CreateTaktGlobalLogger(bootstrapConfiguration);

try
{
    TaktLogger.Information("╔══════════════════════════════════════════════════════════╗");
    TaktLogger.Information("║          TaktWise (TDF) 启动中...            ║");
    TaktLogger.Information("╚══════════════════════════════════════════════════════════╝");

    var builder = WebApplication.CreateBuilder(args);

    // 大 JSON（设变明细可达数十万行）：默认 30MB 会 BadHttpRequestException
    const long maxRequestBodyBytes = 1L * 1024 * 1024 * 1024; // 1GB
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.Limits.MaxRequestBodySize = maxRequestBodyBytes;
    });
    builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
    {
        options.MultipartBodyLengthLimit = maxRequestBodyBytes;
        options.ValueLengthLimit = int.MaxValue;
        options.MultipartHeadersLengthLimit = int.MaxValue;
    });

    builder.Services.AddTaktLogging(builder.Configuration, builder.Environment.EnvironmentName);

    // ========================================
    // 2. 配置多租户和数据库选项
    // ========================================
    TaktLogger.Information("[1/6] 加载配置...");
    
    var tenantOptions = builder.Configuration.RequireOptions<TaktTenantContextOptions>(TaktTenantContextOptions.SectionName);
    tenantOptions.Validate();
    
    // ========================================
    // 2.1 初始化主键类型策略
    // ========================================
    var primaryKeyOptions = builder.Configuration.RequireOptions<PrimaryKeyTypeOptions>(PrimaryKeyTypeOptions.SectionName);
    primaryKeyOptions.Validate();
    
    builder.Services.Configure<PrimaryKeyTypeOptions>(
        builder.Configuration.GetSection(PrimaryKeyTypeOptions.SectionName));
    
    if (primaryKeyOptions.Snowflake.Enabled)
    {
        SqlSugar.SnowFlakeSingle.WorkId = (int)primaryKeyOptions.Snowflake.WorkId;
        TaktLogger.Information("雪花ID已启用，WorkId={WorkId}（开发环境建议1，生产环境每台服务器不同）", primaryKeyOptions.Snowflake.WorkId);
    }
    else
    {
        TaktLogger.Information("雪花ID已禁用");
    }

    TaktPrimaryKeyInsertHelper.Configure(primaryKeyOptions);
    
    TaktLogger.Information("主键类型策略 - 自增: {Identity}, GUID: {Guid}, 雪花: {Snowflake}",
        primaryKeyOptions.Identity.Enabled, primaryKeyOptions.Guid.Enabled, primaryKeyOptions.Snowflake.Enabled);
    TaktLogger.Information("  ✓ 多租户配置已加载 - 租户头: {TenantHeader}, 公司头: {CompanyHeader}", 
        tenantOptions.TenantHeaderName, tenantOptions.CompanyHeaderName);
    
    builder.Services.Configure<TaktTenantContextOptions>(
        builder.Configuration.GetSection(TaktTenantContextOptions.SectionName));
    
    var databaseOptions = builder.Configuration.RequireOptions<TaktDatabaseOptions>(TaktDatabaseOptions.SectionName);
    databaseOptions.NormalizeAndValidate();
    
    builder.Services.Configure<TaktDatabaseOptions>(
        builder.Configuration.GetSection(TaktDatabaseOptions.SectionName));
    
    var passwordPolicyOptions = builder.Configuration.RequireOptions<TaktPasswordPolicyOptions>(TaktPasswordPolicyOptions.SectionName);
    passwordPolicyOptions.Validate();
    TaktLogger.Information("  ✓ 密码策略配置已加载 - 最小长度: {MinLength}", passwordPolicyOptions.MinLength);
    
    builder.Services.Configure<TaktPasswordPolicyOptions>(
        builder.Configuration.GetSection(TaktPasswordPolicyOptions.SectionName));

    var excelOptions = builder.Configuration.RequireOptions<TaktExcelOptions>(TaktExcelOptions.SectionName);
    excelOptions.Validate();
    builder.Services.Configure<TaktExcelOptions>(
        builder.Configuration.GetSection(TaktExcelOptions.SectionName));
    builder.Services.AddSingleton(excelOptions);
    TaktExcelHelper.Configure(excelOptions);
    builder.Services.Configure<TaktDatabaseBackupOptions>(
        builder.Configuration.GetSection(TaktDatabaseBackupOptions.SectionName));
    TaktLogger.Information(
        "  ✓ Excel 导入导出配置已加载 - 作者: {Author}, 导入行/Sheet/单Sheet: {ImportMaxRows}/{ImportMaxSheets}/{ImportMaxRowsPerSheet}, 导出请求/单Sheet/Sheet数: {ExportRequestMax}/{ExportSheetMaxRows}/{ExportMaxSheets}",
        excelOptions.Author,
        excelOptions.Import.MaxRowsPerFile,
        excelOptions.Import.MaxSheetsPerFile,
        excelOptions.Import.MaxRowsPerSheet,
        excelOptions.Export.MaxRowsPerRequest,
        excelOptions.Export.MaxRowsPerSheet,
        excelOptions.Export.MaxSheetsPerFile);

    var pagedOptions = builder.Configuration.RequirePaged();
    builder.Services.Configure<TaktPagedOptions>(
        builder.Configuration.GetSection(TaktPagedOptions.SectionName));
    TaktPagedClamp.Configure(pagedOptions);
    TaktLogger.Information(
        "  ✓ 分页配置已加载 - 默认: {DefaultPageIndex}/{DefaultPageSize}, 上限: {MaxPageSize}, 可选项: {PageSizeOptions}",
        pagedOptions.DefaultPageIndex,
        pagedOptions.DefaultPageSize,
        pagedOptions.MaxPageSize,
        string.Join(',', pagedOptions.PageSizeOptions));

    builder.Services.Configure<TaktFileUploadOptions>(
        builder.Configuration.GetSection(TaktFileUploadOptions.SectionName));
    var authenticationOptions = builder.Configuration.RequireAuthentication();
    builder.Services.Configure<TaktAuthenticationOptions>(
        builder.Configuration.GetSection(TaktAuthenticationOptions.SectionName));

    var accountLockOptions = builder.Configuration.RequireAccountLock();
    builder.Services.Configure<TaktAccountLockOptions>(
        builder.Configuration.GetSection(TaktAccountLockOptions.SectionName));

    var systemOptions = builder.Configuration.RequireSystem();
    builder.Services.Configure<TaktSystemOptions>(
        builder.Configuration.GetSection(TaktSystemOptions.SectionName));

    var emailOptions = builder.Configuration.RequireEmail();
    builder.Services.Configure<TaktEmailOptions>(
        builder.Configuration.GetSection(TaktEmailOptions.SectionName));

    TaktLogger.Information(
        "  ✓ 认证/账户锁定/系统/邮件配置已绑定校验 - Token有效期: {AccessHours}h, 账户锁定: {AccountLockEnabled}, 环境: {SystemEnv}",
        authenticationOptions.AccessTokenLifetimeHours,
        accountLockOptions.Enabled,
        systemOptions.Environment);

    builder.Services.AddSingleton(passwordPolicyOptions);
    builder.Services.AddSingleton(tenantOptions);
    builder.Services.AddSingleton(databaseOptions);
    builder.Services.AddSingleton(emailOptions);

    TaktLogger.Information("  ✓ 数据库配置已加载 - 类型: {DbType}", databaseOptions.GetSugarDbType());
    TaktLogger.Information("  ✓ 连接字符串模板: {Template}", databaseOptions.ConnectionStringTemplate);

    // ========================================
    // 3. 配置 Autofac 依赖注入（自动扫描注册）
    // ========================================
    TaktLogger.Information("[2/6] 配置依赖注入 (Autofac)...");
    
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>((context, builder) =>
    {
        builder.RegisterModule(new TaktAutofacModule());
    });
    
    TaktLogger.Information("  ✓ Autofac 容器已配置 - 自动扫描模式");

    // ========================================
    // 4. 配置 Serilog 日志系统
    // ========================================
    TaktLogger.Information("[3/6] 初始化日志系统 (Serilog)...");
    
    builder.Host.UseTaktSerilog();
    
    TaktLogger.Information("  ✓ Serilog 日志已启用 - 输出到: Console + File");

    // ========================================
    // 5. 注册业务服务和扩展
    // ========================================
    TaktLogger.Information("[4/6] 注册业务服务和扩展...");
    
    builder.Services.AddTaktCache(builder.Configuration);
    builder.Services.Configure<Takt.Shared.Options.TaktGenEngineOptions>(opts =>
    {
        opts.ContentRootPath = builder.Environment.ContentRootPath;
    });
    builder.Services.AddTaktCaptcha(builder.Configuration);
    builder.Services.AddTaktSecurity(builder.Configuration);
    builder.Services.AddTaktObservability(builder.Configuration, builder.Environment.EnvironmentName);
    builder.Services.AddControllers(options =>
        {
            options.Filters.Add<TaktValidationFilter>();
        })
        .AddNewtonsoftJson();
    builder.Services.AddScoped<TaktPermissionFilter>();
    builder.Services.AddScoped<TaktValidationFilter>();
    builder.Services.AddTaktValidators();
    builder.Services.AddTaktOpenApi();
    builder.Services.AddTaktServices(builder.Configuration);
    builder.Services.AddTaktQuartz(builder.Configuration);
    builder.Services.Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders = ForwardedHeaders.XForwardedFor
            | ForwardedHeaders.XForwardedProto
            | ForwardedHeaders.XForwardedHost;
        options.KnownNetworks.Clear();
        options.KnownProxies.Clear();
    });

    builder.Services.AddTaktOpenIddict(builder.Configuration, builder.Environment);
    builder.Services.AddScoped<Takt.WebApi.Logging.ITaktAuthLoginLogHandler, Takt.WebApi.Logging.TaktAuthLoginLogHandler>();
    builder.Services.AddScoped<TaktOpenIddictLogHandler>();
    builder.Services.AddTaktSignalR(builder.Configuration);

    var initOptions = builder.Configuration.RequireOptions<TaktInitOptions>(TaktInitOptions.SectionName);
    builder.Services.Configure<TaktInitOptions>(builder.Configuration.GetSection(TaktInitOptions.SectionName));
    
    TaktLogger.Information("  ✓ ASP.NET Core Controllers 已注册");
    TaktLogger.Information("  ✓ FluentValidation 验证器与 TaktValidationFilter 已注册");
    TaktLogger.Information("  ✓ OpenAPI + Scalar 文档已注册");
    TaktLogger.Information("  ✓ 业务服务已注册（用户上下文、本地化、CORS）");
    TaktLogger.Information("  ✓ 缓存服务已注册（Cache 配置：Memory / Redis）");
    TaktLogger.Information("  ✓ 安全服务已注册（限流、CSRF、XSS）");
    TaktLogger.Information("  ✓ 可观测性已注册（HealthChecks / Prometheus / OpenTelemetry）");
    TaktLogger.Information("  ✓ OpenIddict 认证已启用");

    // ========================================
    // 构建应用
    // ========================================
    var app = builder.Build();

    app.InitializeTaktIpLocationDatabase();

    // ========================================
    // 6. 三步初始化：OpenIddict → SqlSugar 建表 → 业务种子
    // ========================================
    TaktLogger.Information("[5/6] 三步初始化（OpenIddict / SqlSugar / Seed）...");
    TaktLogger.Information("  配置: InitDb={InitDb}, SeedData={SeedData}", initOptions.InitDb, initOptions.SeedData);
    await app.RunTaktInitializationAsync(initOptions);

    // ========================================
    // 7. 配置中间件管道
    // ========================================
    
    // 7.1 全局异常处理（必须最先注册）
    app.UseTaktExceptionHandler();

    // 7.1.1 反向代理转发头（Vite dev 代理 /connect、/api 时恢复 Scheme/Host）
    app.UseForwardedHeaders();
    
    // 7.2 本地化中间件（Accept-Language + appsettings DefaultCulture 兜底，在认证之前）
    var locOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
    app.UseRequestLocalization(locOptions.Value);
    
    // 7.3 静态文件（wwwroot：内置资源 + uploads 正式文件；非 wwwroot 存储根时追加映射）
    app.UseStaticFiles();
    app.UseTaktLocalUploadStaticFiles();

    // 7.4 API 文档（仅开发环境）
    app.UseTaktOpenApi();
    
    // 7.5 统一请求日志
    app.UseTaktRequestLogging();
    
    // 7.6 标准中间件
    app.UseHttpsRedirection();
    app.UseCors();
    app.UseTaktSecurity();
    app.UseMiddleware<Takt.Infrastructure.Middleware.TaktSignalRTokenMiddleware>();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseTaktPrometheusHttpMetrics();
    
    // 7.7 路由映射
    app.MapControllers();
    app.MapTaktSignalRHub<Takt.Infrastructure.SignalR.TaktConnectHub>("/hubs/TaktConnectHub");
    app.MapTaktSignalRHub<Takt.Infrastructure.SignalR.TaktNotificationHub>("/hubs/TaktNotificationHub");
    app.MapTaktSignalRHub<Takt.Infrastructure.SignalR.TaktEcChangeHub>("/hubs/TaktEcChangeHub");
    app.MapTaktObservabilityEndpoints();

    // ========================================
    // 8. 输出启动完成信息
    // ========================================
    TaktLogger.Information("╔══════════════════════════════════════════════════════════╗");
    TaktLogger.Information("║              TaktWise 启动成功！              ║");
    TaktLogger.Information("╠══════════════════════════════════════════════════════════╣");
    TaktLogger.Information("║  环境: {Environment,-42} ║", app.Environment.EnvironmentName);
    TaktLogger.Information("║  时间: {Timestamp,-42} ║", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
    TaktLogger.Information("╠══════════════════════════════════════════════════════════╣");
    TaktLogger.Information("║  [启用服务]                                               ║");
    TaktLogger.Information("║  ✓ 多租户隔离: 租户+公司双层隔离                    ║");
    TaktLogger.Information("║    - 租户请求头: {TenantHeader,-24} ║", tenantOptions.TenantHeaderName);
    TaktLogger.Information("║    - 公司请求头: {CompanyHeader,-24} ║", tenantOptions.CompanyHeaderName);
    TaktLogger.Information("║  ✓ 日志系统: Serilog (分级存储)                          ║");
    TaktLogger.Information("║    - 综合日志: logs/takt-.log                           ║");
    TaktLogger.Information("║    - Quartz执行: logs/quartz-/quartz-.log (60天)        ║");
    TaktLogger.Information("║    - 错误日志: logs/error/error-.log (90天)             ║");
    TaktLogger.Information("║    - 警告日志: logs/warning/warning-.log (60天)         ║");
    TaktLogger.Information("║    - 信息日志: logs/information/information-.log (30天) ║");
    TaktLogger.Information("║    - 调试日志: logs/debug/debug-.log (7天)              ║");
    TaktLogger.Information("║  ✓ 请求日志: TaktLoggingMiddleware (带耗时、IP、租户)   ║");
    TaktLogger.Information("║  ✓ 异常处理: TaktExceptionMiddleware (统一错误响应)     ║");
    TaktLogger.Information("║  ✓ 安全防护: 限流 + CSRF + XSS（appsettings Security）  ║");
    TaktLogger.Information("║  ✓ 可观测性: HealthChecks + Prometheus + OTel Tracing ║");
    TaktLogger.Information("║  ✓ 缓存系统: ITaktCacheService（appsettings Cache）      ║");
    TaktLogger.Information("║  ✓ 认证授权: OpenIddict (OAuth2/OIDC)                    ║");
    TaktLogger.Information("║    - AuthorizationCode + PKCE (SPA 标准)                  ║");
    TaktLogger.Information("║    - RefreshToken (刷新令牌)                             ║");
    TaktLogger.Information("║    - ClientCredentials (服务间调用)                      ║");
    TaktLogger.Information("║  ✓ API 文档: Scalar (开发环境)                           ║");
    TaktLogger.Information("║  ✓ 依赖注入: Autofac (自动扫描)                          ║");
    TaktLogger.Information("║  ✓ 数据库: SqlSugar + EF Core                            ║");
    TaktLogger.Information("║  ✓ 实时通信: SignalR (双 Hub，登录后由前端连接)          ║");
    TaktLogger.Information("║    - Connect Hub:  /hubs/TaktConnectHub                   ║");
    TaktLogger.Information("║    - Notify Hub:   /hubs/TaktNotificationHub              ║");
    TaktLogger.Information("║    - 日志模块: signalr (连接/断开/统计推送/协商请求)     ║");
    TaktLogger.Information("╠══════════════════════════════════════════════════════════╣");
    TaktLogger.Information("║  [访问地址]                                               ║");
    TaktLogger.Information("║  后端 HTTPS: https://localhost:60071                     ║");
    TaktLogger.Information("║  后端 HTTP:  http://localhost:60070                      ║");
    TaktLogger.Information("║  API 文档: https://localhost:60071/scalar               ║");
    TaktLogger.Information("║    （主页下拉切换模块；/scalar/{{module}} 直达单模块）   ║");
    TaktLogger.Information("║  健康检查: https://localhost:60071/health                ║");
    TaktLogger.Information("║  就绪探针: https://localhost:60071/health/ready          ║");
    TaktLogger.Information("║  Prometheus: https://localhost:60071/metrics             ║");
    TaktLogger.Information("║  授权端点: https://localhost:60071/connect/authorize     ║");
    TaktLogger.Information("║  令牌端点: https://localhost:60071/connect/token         ║");
    TaktLogger.Information("╠══════════════════════════════════════════════════════════╣");
    TaktLogger.Information("║  [前端地址]                                               ║");
    TaktLogger.Information("║  前端 HTTPS: https://localhost:60081                     ║");
    TaktLogger.Information("║  前端 HTTP:  http://localhost:60080                      ║");
    TaktLogger.Information("╠══════════════════════════════════════════════════════════╣");
    TaktLogger.Information("║  [默认凭据]                                               ║");
    TaktLogger.Information("║  管理员账号: admin / Takt@123456                         ║");
    TaktLogger.Information("║  测试客户端: takt-web-app (无密钥)                       ║");
    TaktLogger.Information("╚══════════════════════════════════════════════════════════╝");

    app.Run();
}
catch (Exception ex)
{
    TaktLogger.Fatal(ex, "╔══════════════════════════════════════════════════════════╗\n║              应用程序启动失败！                     ║\n╚══════════════════════════════════════════════════════════╝");
    throw;
}
finally
{
    TaktLogReporter.Stop();
    TaktLogger.FlushReportAsync().GetAwaiter().GetResult();
    Log.CloseAndFlush();
}
