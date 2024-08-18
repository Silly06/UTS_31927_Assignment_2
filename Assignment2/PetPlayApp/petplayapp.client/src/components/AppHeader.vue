<template>
  <v-app-bar app>
    <img
        src="@/assets/logo.png"
        alt="Logo"
        class="logo"
    />

    <v-spacer></v-spacer>

    <template v-if="!isAuthPage">
        <v-btn @click="navigateToNewPost" :icon="true">
            <v-icon>mdi-plus</v-icon>
        </v-btn>
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
                <v-img :src="profilePicture"></v-img>
            </v-icon>
        </v-btn>
        <v-btn @click="logout" :icon="true" class="logout-btn">
            <v-icon>mdi-logout</v-icon>
        </v-btn>
    </template>
  </v-app-bar>
</template>

<script setup lang="ts">
import {ref, onMounted, computed} from 'vue';
import { useRouter, useRoute } from 'vue-router';
import axios from 'axios';

const router = useRouter();
const route = useRoute();
const profilePicture = ref('');

const userId = sessionStorage.getItem('userId') || '';

const fetchUserProfilePicture = async () => {
  try {
    const response = await axios.get(`/users/GetUserPicture`, { params: { userId } });
    const pictureData = response.data;
    profilePicture.value = `data:image/png;base64,${pictureData}`;
  } catch (error) {
    console.error('Error fetching user profile picture:', error);
    profilePicture.value = '@/assets/default-avatar.png';
  }
};

    const navigateToNewPost = () => {
        router.push('/NewPost');
    };

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
  router.push(`/profile/${userId}`);
};

const logout = () => {
  router.push('/login');
};

const isAuthPage = computed(() => {
  return ['/login', '/signup', '/resetpassword'].includes(route.path.toLowerCase());
});

onMounted(() => {
  fetchUserProfilePicture();
});
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
