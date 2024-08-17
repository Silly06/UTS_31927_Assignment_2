<template>
  <v-container fluid>
    <v-row justify="center" align="center">
      <v-col cols="12" sm="6" md="4">
        <v-card>
          <v-card-title class="headline">Login</v-card-title>
          <v-card-text>
            <v-form @submit.prevent="login">
              <v-text-field
                  v-model="loginDto.username"
                  label="Username"
                  outlined
                  required
              />
              <v-text-field
                  v-model="loginDto.password"
                  label="Password"
                  type="password"
                  outlined
                  required
              />
              <v-btn type="submit" color="primary">Login</v-btn>
            </v-form>
            <v-alert v-if="errorMessage" type="error" class="mt-4">
              {{ errorMessage }}
            </v-alert>
          </v-card-text>
          <v-card-actions>
            <v-btn @click="navigateToSignUp">Don't have an account? Sign Up</v-btn>
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
import type { LoginDto } from '@/types/models';

const loginDto = ref<LoginDto>({ username: '', password: '' });
const errorMessage = ref<string>('');

const router = useRouter();

const login = async () => {
  errorMessage.value = '';
  try {
    const response = await axios.post('/users/login', loginDto.value);
    const data = response.data;
      sessionStorage.setItem('userId', data.userId);
    sessionStorage.setItem('userPfp', data.userPfp)
    await router.push('/Home');
  } catch (error) {
    errorMessage.value = error instanceof Error ? error.message : 'An unexpected error occurred';
  }
};

const navigateToSignUp = () => {
  router.push('/SignUp');
};
</script>

<style scoped>
.v-card {
  max-width: 400px;
  margin: 30px auto auto;
}
</style>
