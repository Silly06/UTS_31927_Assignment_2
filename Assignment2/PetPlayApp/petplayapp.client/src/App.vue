<script setup lang="ts">
import { defineComponent } from 'vue';
import AppHeader from './components/AppHeader.vue'; // Adjust the path if needed

defineComponent({
  components: {
    AppHeader
  }
});
</script>

<template>
  <div id="app">
    <header>
      <AppHeader />
    </header>
    <main>
      <router-view></router-view>
    </main>
  </div>
</template>

<style scoped>
header {
  line-height: 1.5;
}

@media (min-width: 1024px) {
  header {
    display: flex;
    place-items: center;
    padding-right: calc(var(--section-gap) / 2);
  }

  .logo {
    margin: 0 2rem 0 0;
  }

  header .wrapper {
    display: flex;
    place-items: flex-start;
    flex-wrap: wrap;
  }
}
</style>
