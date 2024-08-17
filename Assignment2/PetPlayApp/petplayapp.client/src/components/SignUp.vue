<template>
  <v-container fluid>
    <v-row justify="center" align="center">
      <v-col cols="12" sm="6" md="4">
        <v-card>
          <v-card-title class="headline">Sign Up</v-card-title>
          <v-card-text>
            <v-form @submit.prevent="signUp">
              <v-text-field
                  v-model="createUserDto.username"
                  label="Username"
                  outlined
                  required
              />
              <v-text-field
                  v-model="createUserDto.email"
                  label="Email"
                  type="email"
                  outlined
                  required
              />
              <v-text-field
                  v-model="createUserDto.password"
                  label="Password"
                  type="password"
                  outlined
                  required
              />
              <v-text-field
                  v-model="createUserDto.age"
                  label="Age"
                  type="number"
                  outlined
              />
              <v-textarea
                  v-model="createUserDto.bio"
                  label="Bio"
                  rows="3"
                  outlined
              />
              <v-btn type="submit" color="primary" class="mt-4">Sign Up</v-btn>
            </v-form>
            <v-alert v-if="errorMessage" type="error" class="mt-4">
              {{ errorMessage }}
            </v-alert>
          </v-card-text>
          <v-card-actions>
            <v-btn @click="goBack" color="primary">Go Back</v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import axios from 'axios';
import { useRouter } from 'vue-router';
import type { CreateUserDto } from '@/types/models';

const createUserDto = ref<CreateUserDto>({
  username: '',
  email: '',
  password: '',
  age: 0,
  bio: ''
});

const errorMessage = ref('');
const router = useRouter();

const signUp = async () => {
  errorMessage.value = '';
  try {
    const response = await axios.post('/users/CreateUser', createUserDto.value);
    const { userId } = response.data;

    sessionStorage.setItem('userId', userId);
    await router.push('/Home');
  } catch (error) {
    errorMessage.value = error instanceof Error ? error.message : 'An unexpected error occurred';
  }
};

const goBack = async () => {
  await router.push('/Login');
};
</script>