<template>
  <v-container>
    <v-row>
      <v-col cols="12">
        <v-card>
          <v-card-title>Notifications</v-card-title>
          <v-list>
            <v-list-item v-for="notification in notifications" :key="notification.id">
              <v-list-item-title>{{ notification.content }}</v-list-item-title>
              <v-list-item-subtitle>{{ new Date(notification.timestamp).toLocaleString() }}</v-list-item-subtitle>
            </v-list-item>
          </v-list>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="js">
import { ref, onMounted } from 'vue';
import axios from 'axios';

const notifications = ref([]);
const userId = sessionStorage.getItem('userId');

    const fetchNotifications = async () => {
    try {
        const response = await axios.get(`/notifications/GetRecentNotifications`, {
            params: { userId }
        });
        notifications.value = response.data;
    } catch (error) {
        console.error('Error fetching notifications:', error);
    }
};

onMounted(() => {
  fetchNotifications();
});
</script>

<style scoped>
.v-card {
  max-width: 600px;
  margin: auto;
}

.v-list-item {
  margin-bottom: 10px;
}
</style>
