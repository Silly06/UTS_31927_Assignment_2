<script setup lang="ts">
    import { ref, onMounted } from 'vue';
    import axios from 'axios';

    const notifications = ref([]);
    const userId = 'signedInUserId'; // Replace with actual logic to get signed-in user ID

    const fetchNotifications = async () => {
        try {
            const response = await axios.get(`/api/notification/user/${userId}`);
            notifications.value = response.data;
        } catch (error) {
            console.error('Error fetching notifications:', error);
        }
    };

    onMounted(() => {
        fetchNotifications();
    });
</script>

<template>
    <div>
        <p>Notifications</p>
        <ul>
            <li v-for="notification in notifications" :key="notification.id">
                {{ notification.message }} - {{ new Date(notification.timestamp).toLocaleString() }}
            </li>
        </ul>
    </div>
</template>