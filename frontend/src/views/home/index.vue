<template>
  <div class="home">
    <a-tabs v-model:active-key="activeKey">
      <a-tab-pane
        key="1"
        tab="Tab 1"
      >
        Content of Tab Pane 1
      </a-tab-pane>
      <a-tab-pane
        key="2"
        tab="Tab 2"
        force-render
      >
        Content of Tab Pane 2
      </a-tab-pane>
      <a-tab-pane
        key="3"
        tab="Tab 3"
      >
        Content of Tab Pane 3
      </a-tab-pane>
    </a-tabs>
  </div>
</template>

<script setup lang="ts">
 const activeKey = ref('1');
</script>

<style scoped lang="css">
.home {
  padding: 24px;
}
</style>
