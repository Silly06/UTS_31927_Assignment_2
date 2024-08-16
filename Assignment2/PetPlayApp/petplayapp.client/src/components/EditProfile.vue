<script setup lang="ts">
import {onMounted, ref} from 'vue';
import axios from 'axios';
import { useRouter } from 'vue-router';

const userId = sessionStorage.getItem('userId') || '';
const userDetails = ref<{ userName?: string; email?: string; age?: number; bio?: string }>({
  userName: '',
  email: '',
  age: 0,
  bio: ''
});
const errorMessage = ref('');
const successMessage = ref('');
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
    const response = await axios.post('/users/UpdateUserDetails', {
      userId,
      userDetails: userDetails.value
    });
    successMessage.value = response.data;
  } catch (error) {
    errorMessage.value = 'An error occurred while updating user details.';
    console.error(error);
  }
};

onMounted(async () => {
  await fetchUserDetails();
});
</script>

<template>
  <v-container>
    <v-row justify="center">
      <v-col cols="12" md="8">
        <v-card class="mx-auto">
          <v-card-title class="text-center">Edit Profile</v-card-title>
          <v-card-text>
            <v-form @submit.prevent="updateUserDetails">
              <v-text-field
                  v-model="userDetails.userName"
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
              <v-btn type="submit" color="primary" class="mt-4">Update</v-btn>
              <v-btn color="secondary" class="mt-4" @click="router.go(-1)">Back</v-btn>
            </v-form>
            <v-alert v-if="errorMessage" type="error" dismissible>{{ errorMessage }}</v-alert>
            <v-alert v-if="successMessage" type="success" dismissible>{{ successMessage }}</v-alert>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<style scoped>
.v-card {
  max-width: 600px;
  margin: auto;
}

.v-text-field,
.v-textarea {
  margin-bottom: 20px;
}

.v-btn {
  width: 100%;
}
</style>
