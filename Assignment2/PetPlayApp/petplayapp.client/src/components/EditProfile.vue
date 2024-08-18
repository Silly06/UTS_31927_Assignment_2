<template>
  <v-container>
    <v-row justify="center">
      <v-col cols="12" md="8">
        <v-card class="mx-auto">
          <v-card-title class="text-center">Edit Profile</v-card-title>
          <v-card-text>
            <v-form @submit.prevent="updateUserDetails">
              <v-text-field
                  v-model="userDetails.username"
                  label="Username"
                  required
              ></v-text-field>
              <v-text-field
                  v-model="userDetails.email"
                  label="Email"
                  required
                  type="email"
              ></v-text-field>
              <v-text-field
                  v-model="userDetails.age"
                  label="Age"
                  required
                  type="number"
              ></v-text-field>
              <v-textarea
                  v-model="userDetails.bio"
                  label="Bio"
                  rows="4"
              ></v-textarea>
              <!-- File input for profile picture -->
              <v-file-input
                  v-model="profilePicture"
                  label="Upload Profile Picture"
                  accept="image/*"
                  prepend-icon="mdi-camera"
              ></v-file-input>
              <v-btn type="submit" color="primary" class="mt-4">Update</v-btn>
              <v-btn color="secondary" class="mt-4" @click="goToProfile">Go to Profile</v-btn>
            </v-form>
            <v-alert v-if="errorMessage" type="error" dismissible>{{ errorMessage }}</v-alert>
            <v-alert v-if="successMessage" type="success" dismissible>{{ successMessage }}</v-alert>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import axios from 'axios';
import { useRouter } from 'vue-router';
import type { UserDetailsDto } from '@/types/models';

const userId = sessionStorage.getItem('userId') || '';
const userDetails = ref<UserDetailsDto>({
  userId: '',
  username: '',
  email: '',
  age: 0,
  bio: ''
});
const profilePicture = ref<File | null>(null);
const errorMessage = ref<string>('');
const successMessage = ref<string>('');
const router = useRouter();

const fetchUserDetails = async () => {
  try {
    const response = await axios.get('/users/GetUserDetails', { params: { userId } });
    userDetails.value = response.data;
  } catch (error) {
    errorMessage.value = 'Failed to load user details.';
    console.error(error);
  }
};

const updateUserDetails = async () => {
  try {
    const formData = new FormData();
    formData.append('userId', userId);
    formData.append('username', userDetails.value.username || '');
    formData.append('email', userDetails.value.email || '');
    formData.append('age', userDetails.value.age?.toString() || '');
    formData.append('bio', userDetails.value.bio || '');
    if (profilePicture.value) {
      formData.append('profilePicture', profilePicture.value);
    }

    const response = await axios.post('/users/UpdateUserDetails', formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    });
    successMessage.value = response.data;
  } catch (error) {
    errorMessage.value = 'An error occurred while updating user details.';
    console.error(error);
  }
};

const goToProfile = async () => {
  await router.push('/Home');
};

onMounted(async () => {
  await fetchUserDetails();
});
</script>
