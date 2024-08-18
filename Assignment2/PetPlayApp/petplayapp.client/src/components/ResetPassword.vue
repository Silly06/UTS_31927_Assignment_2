<script setup lang="ts">
import { ref } from 'vue';
import axios from 'axios';
import { useRouter } from 'vue-router';
import { ResetPasswordDto } from "@/types/models";

const confirmPassword = ref<string>('');
const error = ref<string | null>(null);
const success = ref<string | null>(null);

const resetPasswordRef = ref<ResetPasswordDto>({
  email: '',
  oldPassword: '',
  newPassword: ''
});

const router = useRouter();

const resetPassword = async () => {
  if (resetPasswordRef.value.newPassword !== confirmPassword.value) {
    error.value = 'Passwords do not match';
    return;
  }

  try {
    await axios.post('/users/ResetPassword', resetPasswordRef.value);

    success.value = 'Password reset successfully';
    error.value = null;

    setTimeout(() => router.push('/Login'), 2000);
  } catch (err) {
    error.value = 'Error resetting password. Please try again later.';
    success.value = null;
  }
};

const goBack = () => {
  router.push('/Login');
};
</script>

<template>
  <v-container>
    <v-row>
      <v-col cols="12" md="6" offset-md="3">
        <v-card>
          <v-card-title>
            <span class="headline">Reset Password</span>
          </v-card-title>

          <v-card-subtitle>
            Please enter your old password, new password, and confirm it.
          </v-card-subtitle>

          <v-card-text>
            <v-form>
              <v-text-field
                  v-model="resetPasswordRef.email"
                  label="Email"
                  type="email"
                  required
              ></v-text-field>

              <v-text-field
                  v-model="resetPasswordRef.oldPassword"
                  label="Old Password"
                  type="password"
                  required
              ></v-text-field>

              <v-text-field
                  v-model="resetPasswordRef.newPassword"
                  label="New Password"
                  type="password"
                  required
              ></v-text-field>

              <v-text-field
                  v-model="confirmPassword"
                  label="Confirm Password"
                  type="password"
                  required
              ></v-text-field>

              <v-btn @click="resetPassword" color="primary">Reset Password</v-btn>

              <v-btn @click="goBack" color="secondary">Back to Login</v-btn>

              <v-alert v-if="error" type="error" dismissible>
                {{ error }}
              </v-alert>

              <v-alert v-if="success" type="success" dismissible>
                {{ success }}
              </v-alert>
            </v-form>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<style scoped>
.v-card {
  padding: 16px;
}
</style>
