<template>
  <v-app-bar app>
    <v-img
        src="@/assets/logo.png"
        alt="Logo"
        max-width="100"
        class="logo"
    ></v-img>

    <v-spacer></v-spacer>
    <v-btn @click="navigateToHome" icon>
      <v-icon>mdi-home</v-icon>
    </v-btn>
    <v-btn @click="navigateToNotifications" icon>
      <v-icon>mdi-bell</v-icon>
    </v-btn>
    <v-btn @click="navigateToProfile" icon>
      <v-icon>mdi-account</v-icon>
    </v-btn>
  </v-app-bar>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router';

const router = useRouter();

const navigateToHome = () => {
  router.push('/home');
};

const navigateToNotifications = () => {
  router.push('/notifications');
};

const navigateToProfile = () => {
  router.push('/profile');
};
</script>

<style scoped>
.v-app-bar {
  background-color: skyblue;
  color: black;
  border-bottom: 2px solid black;
}

.logo {
  margin-left: 16px;
  margin-right: auto;
  height: 60px;
}

.v-app-bar .v-btn:hover {
  background-color: lightskyblue;
}
</style>
