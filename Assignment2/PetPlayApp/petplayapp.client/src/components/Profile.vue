<template>
  <v-container>
    <v-row justify="center" align="center">
      <v-col cols="12" sm="6" md="4">
        <v-card>
            <v-card-title class="headline">{{ userDetails?.username }}</v-card-title>
            <v-img :src="getIconSource()"></v-img>
          <v-card-text>
            <v-list>
                <v-list-item>
                    <v-list-item-title>Email: {{ userDetails?.email }}</v-list-item-title>
                    <v-list-item-title>Age: {{ userDetails?.age }}</v-list-item-title>
                    <v-list-item-title>Bio: {{ userDetails?.bio }}</v-list-item-title>
                    <v-list-item-title>Interest: {{ userDetails?.interest }}</v-list-item-title>
                    <v-list-item-title>Status: {{ userDetails?.status }}</v-list-item-title>
                    <v-card-actions class="justify-center">
                        <v-btn @click="viewMatches" class="matches-button">View Matches</v-btn>
                    </v-card-actions>
                </v-list-item>
            </v-list>
            <v-alert v-if="errorMessage" type="error" class="mt-4">
              {{ errorMessage }}
            </v-alert>
          </v-card-text>
          <v-card-actions class="justify-center">
            <v-btn @click="goBack" color="primary" class="bottom-buttons">Go Back</v-btn>
            <v-btn @click="editProfile" color="secondary" class="bottom-buttons">Edit Profile</v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import axios from 'axios';
import { useRouter } from 'vue-router';
import { UserDetailsDto } from '@/types/models';

const userId = sessionStorage.getItem('userId') || '';
const userDetails = ref<UserDetailsDto | null>(null);
const errorMessage = ref('');
const router = useRouter();

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
  } catch (error) {
    errorMessage.value = 'Error fetching user details: ' + (error instanceof Error ? error.message : 'Unknown error');
  }
};

const goBack = async () => {
  await router.push('/Home');
};

const editProfile = async () => {
  await router.push('/EditProfile');
    };

const viewMatches = async () => {
    await router.push('/Matches');
}

onMounted(async () => {
  await fetchUserDetails();
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

<style>
.v-list-item-title{
    padding: 5px;
}
</style>
