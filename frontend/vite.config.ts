// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/vite.config
// 文件名称：vite.config.ts
// 创建时间：2026-05-22
// 创建人：Takt365(Cursor AI)
// 功能描述：Vite 8 构建与开发服务器配置（环境变量驱动，无代码内静态兜底）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================
//
// 环境变量来源：frontend/.env、.env.development、.env.production
// 客户端运行时读取：src/config/viteEnv.ts（requireViteEnv，缺失即抛错）
//
// 开发代理约定（VITE_API_BASE_URL 为相对路径时，如 /api）：
//   浏览器 → 同源 dev 服务器（VITE_APP_ORIGIN，如 https://localhost:60081）
//   Vite 代理 → VITE_API_PROXY_TARGET（如 https://localhost:60071）
//     - VITE_API_BASE_URL       → 业务 REST API
//     - /health                 → 根路径健康检查（预热 Cookie）
//     - /uploads                → 本地 AccessUrl 静态文件
//     - VITE_PROXY_PATH_CONNECT → OAuth /connect/*
//     - VITE_PROXY_PATH_HUBS    → SignalR /hubs/*
//
// 样式：Tailwind CSS 4 + 全局 CSS（Ant Design Vue 全量注册 + reset.css）
//
// 生产构建产物目录（build.outDir = dist）：
//   assets/js/{业务领域}/     入口与分包 chunk（*.js、*.js.map）
//   assets/css/{业务领域}/    *.css
//   assets/img/{业务领域}/    图片扩展名（png/jpg/svg/webp/…）
//   assets/other/{业务领域}/  无扩展名或未识别扩展名
//   业务领域：与 JS 对齐——优先引用方 chunk / views 目录索引；公共→shared；三方→vendor；入口→app
//   分类：扩展名决定 js|css|img|other；generateBundle 纠正 CSS 领域（抽出样式常无源路径）
// ========================================

import tailwindcss from '@tailwindcss/vite';
import { readdirSync, readFileSync } from 'node:fs';
import { basename, dirname, extname, join, resolve } from 'node:path';
import { fileURLToPath } from 'node:url';
import dayjs from 'dayjs';
import { defineConfig, loadEnv, type Plugin, type PluginOption, type ProxyOptions, type ViteDevServer } from 'vite';
import vue from '@vitejs/plugin-vue';
import AutoImport from 'unplugin-auto-import/vite';
import Components from 'unplugin-vue-components/vite';
import { AntDesignVueResolver } from 'unplugin-vue-components/resolvers';
import mkcert from 'vite-plugin-mkcert';
import { VitePWA } from 'vite-plugin-pwa';
import { vitePluginLogger } from './src/config/viteDevPlugin.ts';

const __filename = fileURLToPath(import.meta.url);
const __dirname = dirname(__filename);

// ---------------------------------------------------------------------------
// package.json → virtual:app-info
// ---------------------------------------------------------------------------

interface PackageJsonSummary {
  name?: string;
  version?: string;
  dependencies?: Record<string, string>;
  devDependencies?: Record<string, string>;
}

const pkg = JSON.parse(readFileSync(resolve(__dirname, 'package.json'), 'utf-8')) as PackageJsonSummary;
const { dependencies = {}, devDependencies = {}, name: pkgName = '', version: pkgVersion = '' } = pkg;

if (!pkgName.trim() || !pkgVersion.trim()) {
  throw new Error('[vite.config] package.json 缺少 name 或 version');
}

const __APP_INFO__ = {
  pkg: {
    name: pkgName.trim(),
    version: pkgVersion.trim(),
    dependencies,
    devDependencies,
  },
  lastBuildTime: dayjs().format('YYYY-MM-DD HH:mm:ss'),
};

const VIRTUAL_APP_INFO_ID = 'virtual:app-info';
const RESOLVED_APP_INFO_ID = '\0virtual:app-info';

// ---------------------------------------------------------------------------
// 环境变量解析（仅用于本配置文件；禁止 || '默认值' 式兜底）
// ---------------------------------------------------------------------------

/**
 * 读取必填环境变量
 * @param env loadEnv(mode, __dirname, '') 的键值表
 * @param key 变量名
 */
function requireEnv(env: Record<string, string>, key: string): string {
  const value = env[key]?.trim();
  if (!value) {
    throw new Error(`[vite.config] 缺少环境变量 ${key}，请配置 frontend/.env*`);
  }
  return value;
}

/**
 * 解析布尔型环境变量（仅接受 true / false / 1 / 0）
 */
function requireEnvBoolean(env: Record<string, string>, key: string): boolean {
  const value = requireEnv(env, key);
  if (value === 'true' || value === '1') {
    return true;
  }
  if (value === 'false' || value === '0') {
    return false;
  }
  throw new Error(`[vite.config] ${key} 必须为 true、false、1 或 0`);
}

/**
 * 解析数值型环境变量
 */
function requireEnvNumber(env: Record<string, string>, key: string): number {
  const value = Number(requireEnv(env, key));
  if (Number.isNaN(value)) {
    throw new Error(`[vite.config] ${key} 必须为有效数字`);
  }
  return value;
}

/**
 * 解析开发服务器 host
 * - 空字符串：不设置 host（仅本机）
 * - "true"：监听所有网卡
 * - 其他：作为 hostname
 */
function parseDevServerHost(value: string | undefined): string | boolean | undefined {
  if (value === undefined || value === '') {
    return undefined;
  }
  if (value === 'true') {
    return true;
  }
  return value;
}

/**
 * 解析 HMR 使用的 host 字符串（用于 wss 配置）
 */
function resolveHmrHost(host: string | boolean | undefined, port: number): string {
  if (host === true || host === undefined || host === '') {
    return 'localhost';
  }
  if (typeof host === 'string') {
    return host;
  }
  return String(port);
}

// ---------------------------------------------------------------------------
// 插件：virtual:app-info / 开发地址提示
// ---------------------------------------------------------------------------

/**
 * 虚拟模块：import 'virtual:app-info' → package 信息（供 src/utils/appMeta.ts）
 */
function appInfoPlugin(): Plugin {
  const payload = JSON.stringify(__APP_INFO__);

  return {
    name: 'takt-app-info',
    resolveId(id) {
      return id === VIRTUAL_APP_INFO_ID ? RESOLVED_APP_INFO_ID : undefined;
    },
    load(id) {
      if (id !== RESOLVED_APP_INFO_ID) {
        return undefined;
      }
      return `export default ${payload}`;
    },
  };
}

/**
 * 开发服务器启动后在终端打印实际访问协议，避免 HTTPS/HTTP 混用
 */
function devServerUrlHintPlugin(useHttps: boolean, port: number, host: string | boolean | undefined): Plugin {
  return {
    name: 'takt-dev-server-url-hint',
    configureServer(server: ViteDevServer) {
      server.httpServer?.once('listening', () => {
        const h = resolveHmrHost(host, port);
        const protocol = useHttps ? 'https' : 'http';
        // eslint-disable-next-line no-console -- 开发时显式提示，避免误用协议
        console.info(
          `\n[Takt] 前端开发地址: ${protocol}://${h}:${port}（与 VITE_DEV_SERVER_HTTPS=${useHttps} 一致；勿混用 http/https）\n`
        );
      });
    },
  };
}

// ---------------------------------------------------------------------------
// 开发服务器反向代理
// ---------------------------------------------------------------------------

/**
 * 为代理附加 X-Forwarded-Host / X-Forwarded-Proto（OAuth returnUrl 等）
 */
function attachForwardedHeaders(entry: ProxyOptions): void {
  entry.configure = (instance) => {
    instance.on('proxyReq', (proxyReq, req) => {
      const host = req.headers.host;
      if (host) {
        proxyReq.setHeader('X-Forwarded-Host', host);
      }

      const encrypted = (req.socket as { encrypted?: boolean } | undefined)?.encrypted === true;
      proxyReq.setHeader('X-Forwarded-Proto', encrypted ? 'https' : 'http');

      const userAgent = req.headers['user-agent'];
      if (typeof userAgent === 'string' && userAgent.trim()) {
        proxyReq.setHeader('User-Agent', userAgent);
        proxyReq.setHeader('X-Takt-User-Agent', userAgent);
      }
    });
  };
}

/**
 * 构建开发环境反向代理表（仅 development + 相对 API base）
 */
function buildDevProxy(
  env: Record<string, string>,
  mode: string
): Record<string, ProxyOptions> | undefined {
  if (mode !== 'development') {
    return undefined;
  }

  const apiBaseUrl = requireEnv(env, 'VITE_API_BASE_URL');
  if (!apiBaseUrl.startsWith('/')) {
    return undefined;
  }

  const proxyTarget = requireEnv(env, 'VITE_API_PROXY_TARGET');
  const proxyPathConnect = requireEnv(env, 'VITE_PROXY_PATH_CONNECT');
  const proxyPathHubs = requireEnv(env, 'VITE_PROXY_PATH_HUBS');

  const createBackendProxyEntry = (): ProxyOptions => ({
    target: proxyTarget,
    changeOrigin: requireEnvBoolean(env, 'VITE_API_PROXY_CHANGE_ORIGIN'),
    secure: requireEnvBoolean(env, 'VITE_API_PROXY_SECURE'),
    ws: true,
    timeout: 600_000,
    proxyTimeout: 600_000,
  });

  const proxy: Record<string, ProxyOptions> = {
    [apiBaseUrl]: createBackendProxyEntry(),
    '/health': createBackendProxyEntry(),
    '/uploads': createBackendProxyEntry(),
    [proxyPathConnect]: createBackendProxyEntry(),
    [proxyPathHubs]: createBackendProxyEntry(),
  };

  attachForwardedHeaders(proxy[apiBaseUrl]);
  attachForwardedHeaders(proxy['/health']);
  attachForwardedHeaders(proxy['/uploads']);
  attachForwardedHeaders(proxy[proxyPathConnect]);
  attachForwardedHeaders(proxy[proxyPathHubs]);

  return proxy;
}

// ---------------------------------------------------------------------------
// 插件装配
// ---------------------------------------------------------------------------

function buildPlugins(
  env: Record<string, string>,
  mode: string,
  devServerPort: number,
  devServerHost: string | boolean | undefined,
  devHttps: boolean
): PluginOption[] {
  const plugins: PluginOption[] = [
    appInfoPlugin(),
    classifyBuildAssetsPlugin(resolve(__dirname, 'src/views')),
    devServerUrlHintPlugin(devHttps, devServerPort, devServerHost),
    tailwindcss(),
    vue(),
    AutoImport({
      imports: [
        'vue',
        'vue-router',
        'pinia',
        {
          'ant-design-vue': ['message', 'notification', 'Modal'],
        },
        {
          '@/utils/logger': ['logger', 'createLogger'],
        },
      ],
      dirs: ['src/components', 'src/composables'],
      vueTemplate: true,
      dts: 'src/auto-imports.d.ts',
      eslintrc: {
        enabled: true,
        filepath: './.eslintrc-auto-import.json',
        globalsPropValue: true,
      },
    }),
    Components({
      resolvers: [
        AntDesignVueResolver({
          importStyle: false,
          // Watermark 为 CSS-in-JS，无 es/watermark/style；全局 app.use(Antd) 已注册，勿按需注入样式
          exclude: ['Watermark'],
        }),
      ],
      dirs: ['src/components'],
      extensions: ['vue'],
      dts: 'src/components.d.ts',
    }),
  ];

  if (devHttps && mode === 'development') {
    plugins.push(
      mkcert({
        hosts: ['localhost', '127.0.0.1', '::1'],
      })
    );
  }

  if (mode === 'development') {
    plugins.push(vitePluginLogger());
  }

  const pwaEnabled = requireEnvBoolean(env, 'VITE_PWA_ENABLED');
  if (pwaEnabled) {
    const appTitle = requireEnv(env, 'VITE_APP_TITLE');
    plugins.push(
      VitePWA({
        registerType: 'autoUpdate',
        includeAssets: ['favicon.ico'],
        manifest: {
          id: '/',
          name: appTitle,
          short_name: requireEnv(env, 'VITE_PWA_SHORT_NAME'),
          description: requireEnv(env, 'VITE_PWA_DESCRIPTION'),
          theme_color: requireEnv(env, 'VITE_PWA_THEME_COLOR'),
          background_color: requireEnv(env, 'VITE_PWA_BACKGROUND_COLOR'),
          display: 'standalone',
          orientation: 'portrait',
          start_url: '/',
          scope: '/',
          lang: 'zh-CN',
          icons: [
            {
              src: '/takt.svg',
              sizes: '192x192',
              type: 'image/svg+xml',
              purpose: 'any',
            },
            {
              src: '/takt.svg',
              sizes: '512x512',
              type: 'image/svg+xml',
              purpose: 'maskable',
            },
          ],
        },
        workbox: {
          // vendor 分包可达数 MB；默认 2 MiB 会令 generateSW 失败
          maximumFileSizeToCacheInBytes: 12 * 1024 * 1024,
          // 超大 vendor 不进 SW 预缓存（安装体积）；运行时按需 CacheFirst
          globPatterns: ['**/*.{js,css,html,ico,png,svg,woff,woff2}'],
          globIgnores: ['**/assets/js/vendor/**'],
          runtimeCaching: [
            {
              urlPattern: /\/assets\/js\/vendor\/.*/i,
              handler: 'CacheFirst',
              options: {
                cacheName: 'vendor-js-cache',
                expiration: {
                  maxEntries: 30,
                  maxAgeSeconds: 60 * 60 * 24 * 30,
                },
                cacheableResponse: {
                  statuses: [0, 200],
                },
              },
            },
            {
              urlPattern: /^https:\/\/.*\.(?:png|jpg|jpeg|svg|gif|webp)$/i,
              handler: 'CacheFirst',
              options: {
                cacheName: 'images-cache',
                expiration: {
                  maxEntries: 50,
                  maxAgeSeconds: 60 * 60 * 24 * 30,
                },
              },
            },
            {
              urlPattern: /\/api\/.*/i,
              handler: 'NetworkFirst',
              options: {
                cacheName: 'api-cache',
                networkTimeoutSeconds: 10,
                cacheableResponse: {
                  statuses: [0, 200],
                },
              },
            },
          ],
        },
        devOptions: {
          enabled: requireEnvBoolean(env, 'VITE_PWA_DEV_ENABLED'),
          type: 'module',
        },
      })
    );
  }

  return plugins;
}

// ---------------------------------------------------------------------------
// 路径别名（与 tsconfig.json compilerOptions.paths 保持一致）
// ---------------------------------------------------------------------------

const SRC_SUBDIR_ALIAS_ENTRIES = [
  'api',
  'bootstrap',
  'components',
  'composables',
  'config',
  'directives',
  'layouts',
  'locales',
  'router',
  'stores',
  'styles',
  'types',
  'utils',
  'views',
] as const;

function buildResolveAlias(configRoot: string): Record<string, string> {
  const srcDir = resolve(configRoot, 'src');
  const alias: Record<string, string> = {};

  for (const segment of SRC_SUBDIR_ALIAS_ENTRIES) {
    alias[`@${segment}`] = resolve(srcDir, segment);
  }

  alias['@'] = srcDir;
  // hotkeys-js 的 ESM 产物同时含 module.exports 与 export，Rolldown 会报 COMMONJS_VARIABLE_IN_ESM；改走纯 CJS
  alias['hotkeys-js'] = resolve(configRoot, 'node_modules/hotkeys-js/dist/hotkeys-js.umd.cjs');

  return alias;
}

// ---------------------------------------------------------------------------
// 生产构建产物目录：assets/{js|css|img|other}/{业务领域}/
// ---------------------------------------------------------------------------

const IMAGE_ASSET_EXTENSIONS = new Set([
  '.png',
  '.jpg',
  '.jpeg',
  '.gif',
  '.svg',
  '.webp',
  '.ico',
  '.avif',
  '.bmp',
]);

/** src/views|api|locales|types 下已知业务领域首段（与目录名一致） */
const BUILD_OUTPUT_BUSINESS_DOMAINS = new Set([
  'about',
  'accounting',
  'code',
  'common',
  'dashboard',
  'error',
  'foundation',
  'home',
  'human-resource',
  'identity',
  'login',
  'logistics',
  'routine',
  'statistics',
  'workflow',
]);

/**
 * Vite 8 / Rolldown 资源元数据（name / names / originalFileNames 因版本可能不同）
 */
interface BuildAssetFileMeta {
  names?: string[];
  name?: string;
  originalFileNames?: string[];
  originalFileName?: string | null;
  source?: string | Uint8Array;
}

/**
 * Rolldown / Rollup PreRenderedChunk 子集（用于 chunk 输出路径）
 */
interface BuildChunkFileMeta {
  name?: string;
  facadeModuleId?: string | null;
  moduleIds?: string[];
}

/**
 * 从模块/资源路径解析业务领域目录名
 * @param filePath 绝对或相对路径、chunk 名
 * @returns vendor | app | shared | 业务领域 kebab 段
 */
function resolveBusinessDomainFromPath(filePath: string | undefined | null): string {
  if (!filePath?.trim()) {
    return 'shared';
  }
  const normalized = filePath.replace(/\\/g, '/');
  if (normalized.includes('node_modules')) {
    return 'vendor';
  }
  if (/\b(vue-vendor|antd-vendor|editor-vendor|echarts-vendor|logicflow-vendor|form-create-vendor|vendor)\b/.test(normalized)) {
    return 'vendor';
  }
  const domainMatch = normalized.match(/\/src\/(?:views|api|locales|types)\/([^/]+)\//);
  if (domainMatch?.[1]) {
    const segment = domainMatch[1];
    if (BUILD_OUTPUT_BUSINESS_DOMAINS.has(segment) || /^[a-z][a-z0-9-]*$/.test(segment)) {
      return segment;
    }
  }
  if (/\/src\/(?:components|composables|utils|styles|layouts|bootstrap|config|directives|router|stores)\//.test(normalized)) {
    return 'shared';
  }
  if (/\/src\/main\.(ts|tsx|js|jsx)$/.test(normalized) || /\/index\.html$/.test(normalized)) {
    return 'app';
  }
  // 抽出 CSS 常仅有裸 chunk 名（如 about），无路径时可对齐已知领域
  const bareName = (normalized.split('/').pop() ?? '').replace(/\.[^.]+$/, '');
  if (BUILD_OUTPUT_BUSINESS_DOMAINS.has(bareName)) {
    return bareName;
  }
  return 'shared';
}

/**
 * 从 chunk 元数据解析业务领域（优先 facade，再扫 moduleIds）
 * @param chunkInfo Rolldown PreRenderedChunk
 */
function resolveChunkBusinessDomain(chunkInfo: BuildChunkFileMeta): string {
  if (chunkInfo.name && /\b(vue-vendor|antd-vendor|editor-vendor|echarts-vendor|logicflow-vendor|form-create-vendor|vendor)\b/.test(chunkInfo.name)) {
    return 'vendor';
  }
  const candidates = [
    chunkInfo.facadeModuleId,
    ...(chunkInfo.moduleIds ?? []),
    chunkInfo.name,
  ].filter((v): v is string => typeof v === 'string' && v.length > 0);

  for (const candidate of candidates) {
    const domain = resolveBusinessDomainFromPath(candidate);
    if (domain !== 'shared') {
      return domain;
    }
  }
  return candidates.length > 0 ? resolveBusinessDomainFromPath(candidates[0]) : 'shared';
}

/**
 * 从资源元数据解析业务领域
 * @param assetInfo Rolldown PreRenderedAsset
 */
function resolveAssetBusinessDomain(assetInfo: BuildAssetFileMeta): string {
  const candidates = [
    assetInfo.originalFileNames?.[0],
    assetInfo.originalFileName ?? undefined,
    assetInfo.names?.[0],
    assetInfo.name,
  ].filter((v): v is string => typeof v === 'string' && v.length > 0);

  for (const candidate of candidates) {
    const domain = resolveBusinessDomainFromPath(candidate);
    if (domain !== 'shared') {
      return domain;
    }
  }
  return candidates.length > 0 ? resolveBusinessDomainFromPath(candidates[0]) : 'shared';
}

/**
 * 从资源元数据解析扩展名（优先真实文件名，其次占位 name；无则空串）
 * @param assetInfo Rolldown PreRenderedAsset
 */
function resolveAssetExtension(assetInfo: BuildAssetFileMeta): string {
  const candidates = [
    assetInfo.originalFileNames?.[0],
    assetInfo.originalFileName ?? undefined,
    assetInfo.names?.[0],
    assetInfo.name,
  ].filter((v): v is string => typeof v === 'string' && v.length > 0);

  for (const candidate of candidates) {
    const extension = extname(candidate).toLowerCase();
    if (extension) {
      return extension;
    }
  }
  return '';
}

/**
 * 仅按扩展名归类：css / img / js；无扩展名或未识别 → other
 * @param extension 含点号扩展名（可为空）
 */
function resolveAssetKindByExtension(extension: string): 'js' | 'css' | 'img' | 'other' {
  const ext = extension.trim().toLowerCase();
  if (!ext) {
    return 'other';
  }
  if (ext === '.css') {
    return 'css';
  }
  if (ext === '.js' || ext === '.mjs' || ext === '.cjs' || ext === '.map') {
    return 'js';
  }
  if (IMAGE_ASSET_EXTENSIONS.has(ext)) {
    return 'img';
  }
  return 'other';
}

/**
 * 去掉 Vite 内容哈希后缀：account-title-CywnYFns → account-title
 * @param fileBase 无扩展名的文件名
 */
function stripViteContentHash(fileBase: string): string {
  return fileBase.replace(/-[A-Za-z0-9_-]{6,}$/, '');
}

/**
 * 扫描 src/views/{领域}/**，建立「chunk/组件基名 → 业务领域」索引
 * （CSS 抽出时常只有基名、无源路径，须与 JS 同源对齐）
 * @param viewsRoot src/views 绝对路径
 */
function buildViewsAssetDomainIndex(viewsRoot: string): Map<string, string> {
  const index = new Map<string, string>();

  const walk = (absDir: string, domain: string): void => {
    let entries;
    try {
      entries = readdirSync(absDir, { withFileTypes: true });
    } catch {
      return;
    }
    for (const entry of entries) {
      const fullPath = join(absDir, entry.name);
      if (entry.isDirectory()) {
        index.set(entry.name.toLowerCase(), domain);
        walk(fullPath, domain);
        continue;
      }
      if (!/\.(vue|css|scss|less)$/i.test(entry.name)) {
        continue;
      }
      const base = entry.name.replace(/\.(vue|css|scss|less)$/i, '').toLowerCase();
      if (base && base !== 'index') {
        index.set(base, domain);
      }
    }
  };

  let topEntries;
  try {
    topEntries = readdirSync(viewsRoot, { withFileTypes: true });
  } catch {
    return index;
  }
  for (const entry of topEntries) {
    if (!entry.isDirectory()) {
      continue;
    }
    walk(join(viewsRoot, entry.name), entry.name);
  }
  return index;
}

/**
 * 从已输出路径解析领域：assets/js|css|…/{domain}/file
 * @param fileName 产物相对路径
 */
function resolveDomainFromOutputFileName(fileName: string): string | undefined {
  const normalized = fileName.replace(/\\/g, '/');
  const match = normalized.match(/^assets\/(?:js|css|img|other)\/([^/]+)\//);
  if (!match?.[1] || match[1] === 'shared') {
    return undefined;
  }
  return match[1];
}

/**
 * 按扩展名归类 + 按 views 索引 / 引用方 chunk 对齐业务领域，并回写引用
 * @param viewsRoot src/views 绝对路径
 */
function classifyBuildAssetsPlugin(viewsRoot: string): Plugin {
  const viewsBaseToDomain = buildViewsAssetDomainIndex(viewsRoot);

  return {
    name: 'takt-classify-build-assets',
    generateBundle(_options, bundle) {
      const cssImporterDomain = new Map<string, string>();
      const chunkBaseToDomain = new Map<string, string>();

      for (const output of Object.values(bundle)) {
        if (output.type !== 'chunk') {
          continue;
        }
        const fromFacade = resolveChunkBusinessDomain(output);
        const fromPath = resolveDomainFromOutputFileName(output.fileName);
        const domain =
          fromPath && fromPath !== 'shared'
            ? fromPath
            : fromFacade !== 'shared'
              ? fromFacade
              : 'shared';

        if (output.name) {
          chunkBaseToDomain.set(output.name.toLowerCase(), domain);
        }
        const jsBase = stripViteContentHash(basename(output.fileName, extname(output.fileName))).toLowerCase();
        if (jsBase) {
          chunkBaseToDomain.set(jsBase, domain);
        }

        const importedCss = (
          output as { viteMetadata?: { importedCss?: Set<string> | string[] } }
        ).viteMetadata?.importedCss;
        if (!importedCss) {
          continue;
        }
        const cssList = importedCss instanceof Set ? [...importedCss] : [...importedCss];
        for (const cssPath of cssList) {
          const key = cssPath.replace(/\\/g, '/');
          cssImporterDomain.set(key, domain);
          cssImporterDomain.set(basename(key), domain);
        }
      }

      /**
       * 解析资源应落入的业务领域（与 JS 同规则）
       * @param fileName 当前产物路径
       */
      const resolveAssetOutputDomain = (fileName: string): string => {
        const normalized = fileName.replace(/\\/g, '/');
        const fromImporter =
          cssImporterDomain.get(normalized) ?? cssImporterDomain.get(basename(normalized));
        if (fromImporter && fromImporter !== 'shared') {
          return fromImporter;
        }
        const base = stripViteContentHash(basename(normalized, extname(normalized))).toLowerCase();
        const fromChunk = chunkBaseToDomain.get(base);
        if (fromChunk && fromChunk !== 'shared') {
          return fromChunk;
        }
        const fromViews = viewsBaseToDomain.get(base);
        if (fromViews) {
          return fromViews;
        }
        const existing = resolveDomainFromOutputFileName(normalized);
        if (existing) {
          return existing;
        }
        return 'shared';
      };

      const renames = new Map<string, string>();

      for (const output of Object.values(bundle)) {
        if (output.type !== 'asset') {
          continue;
        }
        const oldFileName = output.fileName.replace(/\\/g, '/');
        if (!oldFileName.startsWith('assets/')) {
          continue;
        }
        const extension = extname(oldFileName).toLowerCase();
        const kind = resolveAssetKindByExtension(extension);
        const domain = resolveAssetOutputDomain(oldFileName);
        const nextFileName = `assets/${kind}/${domain}/${basename(oldFileName)}`;
        if (nextFileName === oldFileName) {
          continue;
        }
        renames.set(oldFileName, nextFileName);
        output.fileName = nextFileName;
      }

      if (renames.size === 0) {
        return;
      }

      const rewrite = (text: string): string => {
        let next = text;
        for (const [from, to] of renames) {
          if (next.includes(from)) {
            next = next.split(from).join(to);
          }
        }
        return next;
      };

      for (const output of Object.values(bundle)) {
        if (output.type === 'chunk') {
          output.code = rewrite(output.code);
          continue;
        }
        if (output.type === 'asset' && typeof output.source === 'string') {
          output.source = rewrite(output.source);
        }
      }
    },
  };
}

/**
 * Vite 8 使用 Rolldown：须写 build.rolldownOptions.output
 * （build.rollupOptions 仅为兼容别名，部分合并场景仍会落到默认 assets/）
 */
function buildRolldownOutputOptions() {
  return {
    entryFileNames: (chunkInfo: BuildChunkFileMeta) => {
      const domain = resolveChunkBusinessDomain(chunkInfo);
      return `assets/js/${domain}/[name]-[hash].js`;
    },
    chunkFileNames: (chunkInfo: BuildChunkFileMeta) => {
      const domain = resolveChunkBusinessDomain(chunkInfo);
      return `assets/js/${domain}/[name]-[hash].js`;
    },
    sourcemapFileNames: (chunkInfo: BuildChunkFileMeta) => {
      const domain = resolveChunkBusinessDomain(chunkInfo);
      return `assets/js/${domain}/[name]-[hash].js.map`;
    },
    assetFileNames: (assetInfo: BuildAssetFileMeta) => {
      // 回调阶段可能尚无后缀/路径 → 暂放 other/shared；generateBundle 再按扩展名+领域纠正
      const kind = resolveAssetKindByExtension(resolveAssetExtension(assetInfo));
      const domain = resolveAssetBusinessDomain(assetInfo);
      return `assets/${kind}/${domain}/[name]-[hash][extname]`;
    },
    // Rolldown 仍接受 manualChunks（与历史 Rollup 配置兼容）
    manualChunks(id: string) {
      if (!id.includes('node_modules')) {
        return undefined;
      }
      if (id.includes('vue') || id.includes('vue-router') || id.includes('pinia') || id.includes('vue-i18n')) {
        return 'vue-vendor';
      }
      if (id.includes('ant-design-vue') || id.includes('@ant-design')) {
        return 'antd-vendor';
      }
      if (id.includes('@umoteam') || id.includes('hotkeys-js')) {
        return 'editor-vendor';
      }
      if (id.includes('echarts')) {
        return 'echarts-vendor';
      }
      if (id.includes('@logicflow')) {
        return 'logicflow-vendor';
      }
      if (id.includes('@form-create')) {
        return 'form-create-vendor';
      }
      return 'vendor';
    },
  };
}

// ---------------------------------------------------------------------------
// Vite 主配置
// ---------------------------------------------------------------------------

export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, __dirname, '');

  const devServerPort = requireEnvNumber(env, 'VITE_DEV_SERVER_PORT');
  const buildTarget = requireEnv(env, 'VITE_BUILD_TARGET');
  const devHttps = requireEnvBoolean(env, 'VITE_DEV_SERVER_HTTPS');
  const devServerHost = parseDevServerHost(env.VITE_DEV_SERVER_HOST);
  const hmrHost = resolveHmrHost(devServerHost, devServerPort);

  return {
    plugins: buildPlugins(env, mode, devServerPort, devServerHost, devHttps),

    resolve: {
      alias: buildResolveAlias(__dirname),
    },

    optimizeDeps: {
      include: ['cron-parser', '@umoteam/editor'],
    },

    server: {
      port: devServerPort,
      host: devServerHost,
      strictPort: false,
      proxy: buildDevProxy(env, mode),
      ...(devHttps && mode === 'development'
        ? {
            hmr: {
              protocol: 'wss' as const,
              host: hmrHost,
              port: devServerPort,
              clientPort: devServerPort,
            },
          }
        : {}),
    },

    preview: {
      port: devServerPort,
      host: devServerHost,
    },

    build: {
      target: buildTarget,
      outDir: 'dist',
      // 空字符串：不额外套 Vite 默认 assets/；路径由 rolldownOptions.output → assets/{js|css|img|other}/{领域}/
      assetsDir: '',
      sourcemap: requireEnvBoolean(env, 'VITE_BUILD_SOURCEMAP'),
      chunkSizeWarningLimit: 3500,
      rolldownOptions: {
        output: buildRolldownOutputOptions(),
      },
    },
  };
});
