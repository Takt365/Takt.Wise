'use strict';

/**
 * Audit frontend naming against 00-project / 02-frontend.
 * Report only; does not modify files.
 *
 * Run: node scripts/audit-frontend-naming.cjs
 */

const fs = require('fs');
const path = require('path');

const root = path.resolve(__dirname, '..');
const src = path.join(root, 'frontend/src');

/** @type {{ severity: 'error'|'warn'; rule: string, path: string, detail: string }[]} */
const findings = [];

/**
 * @param {'error'|'warn'} severity
 * @param {string} rule
 * @param {string} filePath
 * @param {string} detail
 */
function add(severity, rule, filePath, detail) {
  findings.push({
    severity,
    rule,
    path: path.relative(root, filePath).replace(/\\/g, '/'),
    detail,
  });
}

/**
 * @param {string} name
 */
function isPascalCase(name) {
  return /^[A-Z][A-Za-z0-9]*$/.test(name);
}

/**
 * @param {string} name
 */
function isCamelCase(name) {
  return /^[a-z][A-Za-z0-9]*$/.test(name);
}

/**
 * @param {string} name
 */
function isKebabCase(name) {
  return /^[a-z0-9]+(-[a-z0-9]+)*$/.test(name);
}

/**
 * @param {string} name
 */
function hasHyphen(name) {
  return name.includes('-');
}

/**
 * @param {string} dir
 * @param {(full: string, name: string, rel: string) => void} onFile
 * @param {(full: string, name: string, rel: string) => void} [onDir]
 * @param {string} [rel]
 */
function walk(dir, onFile, onDir, rel = '') {
  if (!fs.existsSync(dir)) return;
  for (const name of fs.readdirSync(dir)) {
    if (name === 'node_modules' || name === 'dist' || name === '.git') continue;
    const full = path.join(dir, name);
    const r = rel ? `${rel}/${name}` : name;
    const st = fs.statSync(full);
    if (st.isDirectory()) {
      onDir?.(full, name, r);
      walk(full, onFile, onDir, r);
    } else {
      onFile(full, name, r);
    }
  }
}

// ========================================
// views/: kebab dirs + index.vue (pages); components/ under views = PascalCase vue
// ========================================
const viewsRoot = path.join(src, 'views');
walk(
  viewsRoot,
  (full, name, rel) => {
    const ext = path.extname(name);
    const base = path.basename(name, ext);
    const parts = rel.split('/');
    const parent = parts[parts.length - 2] || '';

    if (ext === '.vue') {
      if (parent === 'components' || parts.includes('components')) {
        // views/**/components/*.vue → PascalCase (not index unless entry)
        if (base !== 'index' && !isPascalCase(base)) {
          add('error', 'views-components-vue-pascal', full, `expected PascalCase Vue under components/, got ${name}`);
        }
        if (hasHyphen(base)) {
          add('error', 'views-components-vue-no-kebab', full, `kebab Vue not allowed in components/: ${name}`);
        }
      } else {
        // page entries should be index.vue (HTTP error pages & confirm deep-links allowed)
        if (base !== 'index' && !parts.includes('components')) {
          const allowedNonIndex = new Set([
            '401.vue',
            '403.vue',
            '404.vue',
            '500.vue',
            '503.vue',
            'coming-soon.vue',
            'confirm.vue',
          ]);
          if (!parts.includes('modules') && !parts.includes('composables') && !allowedNonIndex.has(name)) {
            add('warn', 'views-page-index', full, `page vue prefer index.vue, found ${name}`);
          }
        }
      }
    }

    if ((ext === '.ts' || ext === '.js') && hasHyphen(name) && !name.endsWith('.d.ts')) {
      add('error', 'views-ts-no-kebab', full, `TS under views must be camelCase: ${name}`);
    }

    if (ext === '.ts' && name.startsWith('use') && !isCamelCase(base) && hasHyphen(base)) {
      add('error', 'views-composable-camel', full, `composable must useXxx camelCase: ${name}`);
    }
  },
  (full, name, rel) => {
    const parts = rel.split('/');
    // first segment and nested route dirs: kebab
    if (name === 'components' || name === 'composables' || name === 'modules' || name === 'utils') return;
    // under components/ → should be rare; usually flat
    if (parts.includes('components')) {
      if (hasHyphen(name)) {
        add('error', 'views-components-dir-no-kebab', full, `components subdir should not be kebab: ${name}`);
      }
      return;
    }
    if (!isKebabCase(name) && name !== 'components') {
      // allow pure lowercase single segment without hyphen
      if (!/^[a-z0-9]+$/.test(name)) {
        add('error', 'views-dir-kebab', full, `views directory must be kebab-case: ${name}`);
      }
    }
  },
);

// ========================================
// components/: category lowercase; component dirs PascalCase; ts camelCase
// ========================================
const componentsRoot = path.join(src, 'components');
const categoryDirs = new Set(['business', 'common', 'navigation', 'notification']);
const technicalDirs = new Set([
  'config',
  'styles',
  'drawers',
  'dialog',
  'node',
  'sfc',
  'antdv',
  'element',
  'tabs',
  'composables',
  'utils',
  'hooks',
  'types',
  'Avatar',
  'Cropper',
  'Files',
  'Images',
  'BasicSetting',
]);

walk(
  componentsRoot,
  (full, name) => {
    const ext = path.extname(name);
    const base = path.basename(name, ext);

    if ((ext === '.ts' || ext === '.js') && hasHyphen(name) && !name.endsWith('.d.ts')) {
      add('error', 'components-ts-camel', full, `helper TS must be camelCase (no kebab): ${name}`);
    }

    if (ext === '.vue') {
      if (base !== 'index' && !isPascalCase(base)) {
        add('error', 'components-vue-pascal', full, `Vue component must be PascalCase or index.vue: ${name}`);
      }
      if (hasHyphen(base)) {
        add('error', 'components-vue-no-kebab', full, `kebab Vue filename forbidden: ${name}`);
      }
    }

    // independent css may be kebab; warn only if PascalCase mixed oddly
    if ((ext === '.css' || ext === '.scss') && /[A-Z]/.test(name) && hasHyphen(name)) {
      add('warn', 'components-css-mixed', full, `CSS name mixes Pascal and kebab: ${name}`);
    }
  },
  (full, name, rel) => {
    const depth = rel.split('/').length;
    if (depth === 1) {
      if (!categoryDirs.has(name)) {
        add('warn', 'components-category', full, `unexpected top-level category (expected business/common/navigation/notification): ${name}`);
      }
      return;
    }
    // component package dirs under category → PascalCase
    if (depth === 2) {
      if (!isPascalCase(name)) {
        add('error', 'components-pkg-pascal', full, `component package dir must be PascalCase: ${name}`);
      }
      return;
    }
    // deeper: technical lowercase OR PascalCase feature
    if (hasHyphen(name)) {
      add('error', 'components-subdir-no-kebab', full, `kebab subdir forbidden under components: ${name}`);
    }
  },
);

// ========================================
// api/: camelCase.api.ts; dirs kebab
// ========================================
const apiRoot = path.join(src, 'api');
walk(
  apiRoot,
  (full, name) => {
    if (name === 'request.ts') return;
    if (!name.endsWith('.ts')) return;
    if (name.endsWith('.d.ts')) return;
    if (hasHyphen(name)) {
      add('error', 'api-file-camel', full, `API file must be camelCase.api.ts (no kebab): ${name}`);
      return;
    }
    if (name !== 'request.ts' && !name.endsWith('.api.ts')) {
      // allow rare helpers?
      if (!isCamelCase(name.replace(/\.ts$/, ''))) {
        add('error', 'api-file-camel-name', full, `API file stem must be camelCase: ${name}`);
      } else {
        add('warn', 'api-file-suffix', full, `prefer *.api.ts suffix: ${name}`);
      }
    } else if (name.endsWith('.api.ts')) {
      const stem = name.slice(0, -'.api.ts'.length);
      if (!isCamelCase(stem)) {
        add('error', 'api-stem-camel', full, `API stem must be camelCase: ${name}`);
      }
      if (stem.endsWith('Api') || stem.endsWith('api')) {
        add('error', 'api-no-api-suffix', full, `do not repeat Api in filename: ${name}`);
      }
    }
  },
  (full, name) => {
    if (!isKebabCase(name) && !/^[a-z0-9]+$/.test(name)) {
      add('error', 'api-dir-kebab', full, `api directory must be kebab-case: ${name}`);
    }
  },
);

// ========================================
// types/: camelCase.types.ts
// ========================================
const typesRoot = path.join(src, 'types');
walk(
  typesRoot,
  (full, name) => {
    if (name.endsWith('.d.ts')) return;
    if (!name.endsWith('.ts')) return;
    if (hasHyphen(name)) {
      add('error', 'types-file-camel', full, `types file must be camelCase.types.ts (no kebab): ${name}`);
      return;
    }
    if (!name.endsWith('.types.ts')) {
      add('warn', 'types-file-suffix', full, `prefer *.types.ts: ${name}`);
    } else {
      const stem = name.slice(0, -'.types.ts'.length);
      if (!isCamelCase(stem)) {
        add('error', 'types-stem-camel', full, `types stem must be camelCase: ${name}`);
      }
    }
  },
  (full, name) => {
    if (!isKebabCase(name) && !/^[a-z0-9]+$/.test(name)) {
      add('error', 'types-dir-kebab', full, `types directory must be kebab-case: ${name}`);
    }
  },
);

// ========================================
// utils/: camelCase only (no kebab)
// ========================================
const utilsRoot = path.join(src, 'utils');
walk(utilsRoot, (full, name) => {
  if (!name.endsWith('.ts') || name.endsWith('.d.ts')) return;
  if (hasHyphen(name)) {
    add('error', 'utils-no-kebab', full, `utils must be camelCase (no kebab): ${name}`);
  }
  const stem = name.replace(/\.ts$/, '');
  if (!isCamelCase(stem) && !stem.includes('.')) {
    add('error', 'utils-camel', full, `utils file must be camelCase: ${name}`);
  }
});

// ========================================
// stores/: useXxxStore.ts
// ========================================
const storesRoot = path.join(src, 'stores');
walk(
  storesRoot,
  (full, name) => {
    if (!name.endsWith('.ts') || name.endsWith('.d.ts')) return;
    if (hasHyphen(name)) {
      add('error', 'stores-no-kebab', full, `store file must not be kebab: ${name}`);
    }
    const stem = name.replace(/\.ts$/, '');
    if (!/^use[A-Z][A-Za-z0-9]*Store$/.test(stem)) {
      add('error', 'stores-use-store', full, `store must be useXxxStore.ts: ${name}`);
    }
  },
  (full, name) => {
    if (!isKebabCase(name) && !/^[a-z0-9]+$/.test(name)) {
      add('warn', 'stores-dir-kebab', full, `store domain dir prefer kebab/lowercase: ${name}`);
    }
  },
);

// ========================================
// composables/: useXxx.ts
// ========================================
const composablesRoot = path.join(src, 'composables');
walk(composablesRoot, (full, name) => {
  if (!name.endsWith('.ts') || name.endsWith('.d.ts')) return;
  if (hasHyphen(name)) {
    add('error', 'composables-no-kebab', full, `composable must not be kebab: ${name}`);
  }
  const stem = name.replace(/\.ts$/, '');
  if (!/^use[A-Z][A-Za-z0-9]*$/.test(stem)) {
    add('error', 'composables-use', full, `composable must be useXxx.ts: ${name}`);
  }
});

// ========================================
// constants/: camelCase.constants.ts
// ========================================
const constantsRoot = path.join(src, 'constants');
walk(constantsRoot, (full, name) => {
  if (!name.endsWith('.ts') || name.endsWith('.d.ts')) return;
  if (hasHyphen(name)) {
    add('error', 'constants-no-kebab', full, `constants must not be kebab: ${name}`);
  }
  if (!name.endsWith('.constants.ts')) {
    add('warn', 'constants-suffix', full, `prefer *.constants.ts: ${name}`);
  }
});

// ========================================
// config/ + bootstrap/ + router/: camelCase
// ========================================
for (const folder of ['config', 'bootstrap', 'router']) {
  const dir = path.join(src, folder);
  walk(dir, (full, name) => {
    if (!name.endsWith('.ts') || name.endsWith('.d.ts')) return;
    if (name === 'index.ts') return;
    if (hasHyphen(name)) {
      add('error', `${folder}-no-kebab`, full, `${folder} TS must be camelCase: ${name}`);
    }
  });
}

// ========================================
// layouts/: PascalCase dirs / vue
// ========================================
const layoutsRoot = path.join(src, 'layouts');
walk(
  layoutsRoot,
  (full, name) => {
    if (!name.endsWith('.vue')) return;
    const base = path.basename(name, '.vue');
    if (base !== 'index' && !isPascalCase(base)) {
      add('error', 'layouts-vue-pascal', full, `layout Vue must be PascalCase or index.vue: ${name}`);
    }
  },
  (full, name) => {
    if (hasHyphen(name)) {
      add('error', 'layouts-dir-no-kebab', full, `layout dir should be PascalCase: ${name}`);
    }
  },
);

// ========================================
// locales/: kebab path dirs; files zh-CN.ts etc.
// ========================================
const localesRoot = path.join(src, 'locales');
const localeFileOk = /^(zh-CN|zh-HK|en-US|ja-JP)\.ts$/;
walk(
  localesRoot,
  (full, name) => {
    if (!name.endsWith('.ts')) return;
    if (!localeFileOk.test(name) && name !== 'index.ts') {
      add('warn', 'locales-file', full, `unexpected locale filename: ${name}`);
    }
  },
  (full, name) => {
    if (name === 'index') return;
    if (!isKebabCase(name) && !/^[a-z0-9]+$/.test(name)) {
      add('error', 'locales-dir-kebab', full, `locale dir must be kebab/lowercase: ${name}`);
    }
  },
);

// ========================================
// Output
// ========================================
const errors = findings.filter((f) => f.severity === 'error');
const warns = findings.filter((f) => f.severity === 'warn');

/** @type {Record<string, number>} */
const byRule = {};
for (const f of findings) {
  byRule[f.rule] = (byRule[f.rule] || 0) + 1;
}

console.log(JSON.stringify({
  summary: {
    errors: errors.length,
    warnings: warns.length,
    byRule,
  },
  errors: errors.slice(0, 200),
  warnings: warns.slice(0, 100),
}, null, 2));

if (errors.length > 200) {
  console.error(`... ${errors.length - 200} more errors truncated`);
}

process.exitCode = errors.length > 0 ? 1 : 0;
