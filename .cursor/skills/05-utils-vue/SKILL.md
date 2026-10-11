---
name: 05-utils-vue
description: >-
  新建或审查 Takt 前端 utils（纯函数、JSDoc、无 ref、与后端 Helper 对齐）。
  用于 frontend/src/utils、雪花 ID、regex/mask/naming、日志采样，
  或用户提到 05-utils-vue、前端工具类时。
---

# Takt Vue 工具类（utils）

完整规范：`.cursor/rules/05-utils-vue.mdc`

## 五条硬约束（必检）

1. **禁止**纯工具模块内可变 `let` 缓存（网关：`logger.ts`、`eventBus.ts` 等，文件头标明）
2. **禁止**在 `utils/` 用 `ref`/`onMounted` → 用 `composables/`
3. **禁止**扩展原生原型 / 全局污染
4. 导出函数：入口校验或文档化空值；**JSDoc 完整**
5. 与后端 Helper（regex/mask/naming）成对时须同步检查 `04-utils-csharp`

## 自检

- [ ] 1. 无状态 / 网关边界写清
- [ ] 2. **camelCase 文件名**；类名/导出对象 **PascalCase**；领域工具用 `takt` 前缀（`taktLocaleMerge.ts`）
- [ ] 3. 单一职责；非 `utils.ts` 大杂烩
- [ ] 4. 参数校验 + 异常不静默吞掉
- [ ] 5. 溢出：采样/`string` ID / 树深上限3. **禁止**扩展 `String.prototype` / `Array.prototype`
4. 导出函数：入口校验 **或** JSDoc 声明空值返回 `''`/`null`
5. 每个 export **完整 JSDoc**（`@param`、`@returns`）

## 新建 utils 流程

```
- [ ] 1. 确认不属于 composable/api/store
- [ ] 2. kebab-case 文件名；领域工具用 takt- 前缀
- [ ] 3. 八段文件头；功能描述注明与后端 Helper 对齐（如有）
- [ ] 4. 优先 export function 或 XxxHelper 静态类 + 具名 re-export（参照 mask.ts）
- [ ] 5. 实体主键：types/DTO 为 string（后端 ValueToStringConverter）；禁止 Number() 比较主键
- [ ] 6. 大数组日志：sampleForLog（log-formatter.ts），禁止 JSON.stringify 全量
- [ ] 7. 空行密度遵守 03-format-blank-lines
```

## 参照文件

| 文件 | 用途 |
|------|------|
| `naming.ts` | 与 TaktNamingHelper 对齐 |
| `mask.ts` | MaskHelper + 函数导出 |
| `regex.ts` | RegexPatterns + isValid* |
| `log-formatter.ts` | sampleForLog |
| `logger.ts` | 运行时网关（非纯工具） |

## 与后端对齐

| 前端 | 后端 |
|------|------|
| naming.ts | TaktNamingHelper |
| regex.ts | TaktRegexHelper |
| mask.ts | TaktMaskHelper |

任一侧修改须对读另一侧。
