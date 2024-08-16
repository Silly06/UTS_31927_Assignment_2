<template>
  <v-container>
    <v-row justify="center" align="center">
      <v-col cols="12" sm="6" md="4">
        <v-card>
          <v-card-title class="headline">Profile</v-card-title>
          <v-card-text>
            <v-list>
              <v-list-item>
                <v-list-item-title>UserName: {{ userDetails?.userName }}</v-list-item-title>
                <v-list-item-subtitle>Age: {{ userDetails?.age }}</v-list-item-subtitle>
                <v-list-item-subtitle>Bio: {{ userDetails?.bio }}</v-list-item-subtitle>
              </v-list-item>
            </v-list>
            <v-alert v-if="errorMessage" type="error" class="mt-4">
              {{ errorMessage }}
            </v-alert>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import axios from 'axios';

const userId = sessionStorage.getItem('userId') || '';
const userDetails = ref<{ userName?: string; age?: number; bio?: string } | null>(null);
const errorMessage = ref('');

const fetchUserDetails = async () => {
  if (!userId) {
    errorMessage.value = 'User ID not found';
    return;
  }

  try {
    const response = await axios.get(`/users/GetUserDetails`, {
      params: { userId }
    });
    userDetails.value = response.data;
    console.log(response.data)
  } catch (error) {
    errorMessage.value = 'Error fetching user details: ' + (error instanceof Error ? error.message : 'Unknown error');
  }
};

onMounted(async () => {
  await fetchUserDetails();
});
</script>
