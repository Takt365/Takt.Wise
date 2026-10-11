<!-- ======================================== -->
<!-- 服务条款 · TaktWise (TDF) -->
<!-- 文档：https://github.com/Lean365/Takt.Net -->
<!-- ======================================== -->

<template>
  <div class="about-page">
    <h2 class="about-page-title">
      {{ t('about.terms.page.title') }}
    </h2>
    <div class="about-page-content">
      <p>{{ t('about.terms.page.p1') }}</p>
      <p>
        {{ t('about.terms.page.p2', [t('about.terms.page.p2license')]) }}<a
          href="https://github.com/Lean365/Takt.Net"
          target="_blank"
          rel="noopener noreferrer"
        >https://github.com/Lean365/Takt.Net</a>。
      </p>
      <p>{{ t('about.terms.page.p3') }}</p>
      <p>{{ t('about.terms.page.p4') }}</p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n'

const { t } = useI18n()
</script>

<style scoped lang="css">
.about-page {
  padding: 24px;
}
.about-page-title {
  margin-bottom: 16px;
  font-size: 20px;
  font-weight: 600;
}
.about-page-content {
  line-height: 1.8;
  p {
    margin-bottom: 12px;
  }
  a {
    color: var(--ant-color-primary);
  }
}
</style>
