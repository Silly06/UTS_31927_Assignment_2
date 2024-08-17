<template>
  <v-app-bar app>
    <img
        src="@/assets/logo.png"
        alt="Logo"
        class="logo"
    />

    <v-spacer></v-spacer>

    <template v-if="!isAuthPage">
      <v-btn @click="navigateToHome" :icon="true">
        <v-icon>mdi-home</v-icon>
      </v-btn>
      <v-btn @click="navigateToSearch" :icon="true">
        <v-icon>mdi-magnify</v-icon>
      </v-btn>
      <v-btn @click="navigateToNotifications" :icon="true">
        <v-icon>mdi-bell</v-icon>
      </v-btn>
      <v-btn @click="navigateToProfile" :icon="true">
        <v-icon>
            <v-img :src="getIconSource()" class="custom-icon"></v-img>
        </v-icon>
      </v-btn>
      <v-btn @click="logout" :icon="true" class="logout-btn">
        <v-icon>mdi-logout</v-icon>
      </v-btn>
    </template>
  </v-app-bar>
</template>


<script setup lang="ts">
import { useRouter, useRoute } from 'vue-router';
import { computed } from 'vue';

const router = useRouter();
const route = useRoute();

const navigateToHome = () => {
  router.push('/home');
};

const navigateToSearch = () => {
  router.push('/search');
};

const navigateToNotifications = () => {
  router.push('/notifications');
};

const navigateToProfile = () => {
  router.push('/profile');
};

const logout = () => {
  sessionStorage.removeItem('userId');
  router.push('/login');
};

const isAuthPage = computed(() => {
  return ['/login', '/signup', '/resetpassword'].includes(route.path.toLowerCase());
});

const getIconSource = () => {
    const imageSource = sessionStorage.getItem('userPfp');
    if (imageSource) {
        const byte = atob(imageSource);
        const byteNumbers = new Array(byte.length);

        for (let i = 0; i < byte.length; i++) {
            byteNumbers[i] = byte.charCodeAt(i);
        }

        const byteArray = new Uint8Array(byteNumbers);
        const blob = new Blob([byteArray], { type: 'image/png' });
        const urlCreator = window.URL || window.webkitURL;
        const imageUrl = urlCreator.createObjectURL(blob);

        return imageUrl;
    }
};
</script>

<style scoped>
.v-app-bar-remove {
  background-color: skyblue;
  color: black;
  border-bottom: 2px solid black;
}

.logo {
  margin-left: 16px;
  margin-right: auto;
  height: 60px;
  display: block;
  background: none;
}

.v-app-bar .v-btn:hover {
  background-color: lightskyblue;
}

.logout-btn {
  color: red;
  background-color: white;
}

.logout-btn:hover {
  color: white;
  background-color: darkred;
}
</style>
