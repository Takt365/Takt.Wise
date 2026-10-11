// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/composables
// 文件名称：useTaktFormNumbering.ts
// 创建时间：2026-08-24
// 创建人：Takt365(Cursor AI)
// 功能描述：表单新增态：用户从编码规则下拉选择 → 预览下一个业务编码并写入只读编码字段
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { watch, type ComputedRef, type Ref } from 'vue';
import { previewNumberingNext } from '@/api/foundation/numbering.api';

/** useTaktFormNumbering 配置 */
export interface UseTaktFormNumberingOptions {
  /** 表单 reactive 模型 */
  formState: Record<string, unknown>;
  /** 是否编辑态（编辑态不预览） */
  isEdit: Ref<boolean> | ComputedRef<boolean>;
  /** 业务编码字段名，如 announcementCode */
  businessCodeField: string;
  /** 规则编码字段名，默认 numberingRuleCode */
  ruleCodeField?: string;
}

/**
 * 监听编码规则选择并预览业务编码（不落库、不占用流水）
 * @param options 配置项
 * @returns void
 */
export function useTaktFormNumbering(options: UseTaktFormNumberingOptions): void {
  const ruleField = options.ruleCodeField ?? 'numberingRuleCode';
  const codeField = options.businessCodeField;
  let previewSeq = 0;

  watch(
    () => options.formState[ruleField],
    (ruleCode) => {
      if (options.isEdit.value) {
        return;
      }
      const trimmed = String(ruleCode ?? '').trim();
      if (!trimmed) {
        options.formState[codeField] = '';
        return;
      }
      const seq = ++previewSeq;
      void (async () => {
        try {
          const result = await previewNumberingNext(trimmed);
          if (seq !== previewSeq) {
            return;
          }
          options.formState[codeField] = result.businessCode ?? '';
        } catch {
          if (seq !== previewSeq) {
            return;
          }
          options.formState[codeField] = '';
        }
      })();
    },
  );
}
