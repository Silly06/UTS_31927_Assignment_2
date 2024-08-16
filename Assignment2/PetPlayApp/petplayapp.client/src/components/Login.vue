<template>
  <v-container fluid>
    <v-row justify="center" align="center">
      <v-col cols="12" sm="6" md="4">
        <v-card>
          <v-card-title class="headline">Login</v-card-title>
          <v-card-text>
            <v-form @submit.prevent="login">
              <v-text-field
                  v-model="username"
                  label="Username"
                  outlined
                  required
              />
              <v-text-field
                  v-model="password"
                  label="Password"
                  type="password"
                  outlined
                  required
              />
              <v-btn type="submit" color="primary">Login</v-btn>
            </v-form>
            <v-alert
                v-if="errorMessage"
                type="error"
                class="mt-4"
            >
              {{ errorMessage }}
            </v-alert>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="js">
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

    console.log(response.data);
    sessionStorage.setItem('userId', data.userId);
    await router.push('/Home');

  } catch (error) {
    errorMessage.value = error instanceof Error ? error.message : 'An unexpected error occurred';
  }
};
</script>

<style scoped>
.v-card {
  max-width: 400px;
  margin: auto;
}
</style>
