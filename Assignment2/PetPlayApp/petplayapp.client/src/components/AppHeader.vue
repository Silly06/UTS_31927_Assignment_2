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
      <img src="../assets/home.png" style="width: 80px" alt="Home">
        <header class="home-button-label">Home</header>
    </button>
    <button class="header-button" @click="navigateToNotifications">
      <img src="../assets/notification.png" style="width: 80px;" alt="Notifications">
        <header class="notification-button-label">Notifications</header>
    </button>
    <button class="header-button" @click="navigateToProfile">
      <img src="../assets/profile.png" style="width: 80px;" alt="Profile">
        <header>Profile</header>
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
  max-height: 130px;
  max-width: 150px;
}

.header-button:hover {
  cursor: pointer;
  background-color: lightskyblue;
}

.home-button-label {
    padding-top: 20px;
}

.notification-button-label {
    padding-top: 10px;
}
</style>
