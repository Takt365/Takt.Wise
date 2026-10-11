---
name: 14-vue-form
description: >-
  新建或审查 Takt 表单组件 *Form.vue（单表/主子表/树表）。
  用于 views/**/components/*Form.vue、defineExpose、TaktTreeSelect 父级、
  或用户提到 14-vue-form、弹窗表单、级联 items 时。
---

# Vue 表单组件（*Form.vue）

完整规范：`.cursor/rules/14-vue-form.mdc`

**前置（强制）**：[12-crud](../12-crud/SKILL.md)

| 形态 | 参照 form.vue |
|------|---------------|
| 单表 | `views/identity/user/components/UserForm.vue` |
| 主子表 | `views/foundation/dict/components/DictTypeForm.vue` |
| 树表 | `views/human-resource/organization/dept/components/DeptForm.vue` |

列表页集成 → [13-vue-view](../13-vue-view/SKILL.md)

## 通用清单（三种均须）

```
- [ ] 8 行 HTML 头；Props：formData?、loading?
- [ ] defineExpose：validate、getValues、resetFields（+ setServerValidationErrors 可选）
- [ ] a-form：`takt-generated-form` + horizontal + `label-align="right"`；标签 t('entity.*')；占位 common.page.form.placeholder.*
- [ ] ❌ 表单无 `sortOrder` 控件（`SKIP_FORM_FIELDS`；后端自动回填）
- [ ] TaktSelect / TaktTreeSelect；主键与 long 字段 string
- [ ] 「xxx内容」正文用 takt-rich-editor（非 a-input/a-textarea）
- [ ] fileName + accessUrl 成对时用 takt-upload-file（fileName 上传回填；排除 TaktFile）
- [ ] 「自动通过 TaktNumbering」无 MIME：规则下拉（`api-params.documentType`=菜单 MenuName）+ 只读编码（useTaktFormNumbering）
- [ ] DocumentType 存 MenuName（tree-options?valueBy=name）；禁止存菜单 Id
- [ ] 03-format-blank-lines
```

## 单表增量

```
- [ ] formState + rules；watch formData 灌入/reset
- [ ] getValues → Create/Update DTO；编辑只读字段 disabled
- [ ] 复杂业务 a-tabs + locales {路径}.page.tabs.*
```

## 主子表增量

```
- [ ] Tab 主表：mainFormRef / mainFormState / mainFormRules
- [ ] Tab 子表：dictDataList/items 行内编辑或子组件
- [ ] 临时行 row-key client-${uuid}；持久化行 Id string
- [ ] validate：主表 + 子表规则；getValues 含 items 数组
- [ ] 与后端 CreateDto 级联字段名一致（10-master-detail）
```

## 树表增量

```
- [ ] parentId：TaktTreeSelect + /api/TaktXxxs/tree-options
- [ ] ❌ 不入表单：sortOrder（后端自动生成；树拖拽改序走 Sort API）
- [ ] getValues parentId 为 string；根节点与后端约定（通常 "0"）
- [ ] index 新增时默认 parentId = 左侧选中树节点
```

## 父级提交模式

```typescript
await formRef.value?.validate()
const dto = formRef.value?.getValues()
// createXxx(dto) 或 updateXxx(id, dto)
```

父级 `index.vue` 表单弹窗：`:width="formModalWidthPx"`（`useTaktContentModalWidth`）。导入弹窗仍 `:width="600"`。

## 代码生成

| 形态 | 脚本 |
|------|------|
| 单表 | `generate-vue-crud-from-api.cjs` |
| 主子表 | `generate-vue-master-detail-from-api.cjs` |
| 树表 | `generate-vue-tree-from-api.cjs` |

## 交叉规则

- 视图壳：`13-vue-view`
- 代码生成：`15-codegen`
- 主子表后端：`10-master-detail`
- 树表后端/API：`11-tree-table`
- UI/i18n：`02-frontend`
