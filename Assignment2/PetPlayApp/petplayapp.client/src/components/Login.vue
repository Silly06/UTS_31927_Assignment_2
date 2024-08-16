<script setup lang="ts">
import { ref } from 'vue';
import axios from 'axios';
import { useRouter } from 'vue-router';

const username = ref('');
const password = ref('');
const errorMessage = ref('');

const router = useRouter();

const login = async () => {
  errorMessage.value = '';
  try {
    const response = await axios.post('/users/login', {
      username: username.value,
      password: password.value
    });

    const data = response.data;

    console.log('User ID:', data.UserId);
    localStorage.setItem('userId', data.UserId);
    router.push('/Home');

  } catch (error) {
    errorMessage.value = error instanceof Error ? error.message : 'An unexpected error occurred';
  }
};
</script>

<template>
  <div>
    <p>Login</p>
    <form @submit.prevent="login">
      <div>
        <label for="username">Username:</label>
        <input id="username" v-model="username" type="text" required />
      </div>
      <div>
        <label for="password">Password:</label>
        <input id="password" v-model="password" type="password" required />
      </div>
      <button type="submit">Login</button>
    </form>
    <p v-if="errorMessage">{{ errorMessage }}</p>
  </div>
</template>
