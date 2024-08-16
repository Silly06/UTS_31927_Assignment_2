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

<template>
  <div id="header">
    <button class="header-button" @click="navigateToHome">
      <img src="../assets/home.png" style="width: 70px;" alt="Home">
    </button>
    <button class="header-button" @click="navigateToNotifications">
      <img src="../assets/notification.png" style="width: 70px;" alt="Notifications">
    </button>
    <button class="header-button" @click="navigateToProfile">
      <img src="../assets/profile.png" style="width: 70px;" alt="Profile">
    </button>
  </div>
</template>

<style scoped>
.header-button {
  padding: 18px;
  margin-left: 100px;
  margin-top: 15px;
  float: right;
  vertical-align: top;
  font-weight: bold;
  font-size: larger;
  background-color: skyblue;
  color: black;
  border: 2px solid black;
  max-height: 110px;
}

.header-button:hover {
  cursor: pointer;
  background-color: lightskyblue;
}
</style>
