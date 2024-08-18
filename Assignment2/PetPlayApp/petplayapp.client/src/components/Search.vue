<template>
  <v-container class="d-flex justify-center align-center" fluid>
    <v-row class="justify-center">
      <v-col cols="12" sm="8" md="6">
        <v-card class="search-card">
          <v-card-title class="search-card-title">
            <v-btn @click="navigateToHome" :icon="true" class="back-btn">
              <v-icon>mdi-arrow-left</v-icon>
            </v-btn>
          </v-card-title>

          <!-- Search Field -->
          <v-card-subtitle class="search-card-subtitle">
            <v-text-field
                v-model="searchQuery"
                label="Search for users"
                @keyup.enter="searchUsers"
                @input="handleSearchQueryChange"
                outlined
                dense
                clearable
                class="w-100"
            ></v-text-field>
          </v-card-subtitle>

          <v-card-text>
            <v-list>
              <v-list-item
                  v-for="user in searchResults"
                  :key="user.userId"
                  class="user-list-item"
              >
                <router-link :to="`/Profile/${user.userId}`" class="user-link">
                  <v-list-item-content class="d-flex align-center hoverable">
                    <v-list-item-avatar>
                      <img :src="`data:image/png;base64,${user.profilePicture}`" alt="Profile Picture" class="profile-picture"/>
                    </v-list-item-avatar>
                    <v-list-item-title class="user-username">
                      {{ user.username }}
                    </v-list-item-title>
                  </v-list-item-content>
                </router-link>
              </v-list-item>
            </v-list>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import axios from 'axios';
import type { UserSearchDto } from "@/types/models";
import { useRouter } from "vue-router";

const searchQuery = ref('');
const searchResults = ref<UserSearchDto[]>([]);
const router = useRouter();

const searchUsers = async () => {
  try {
    const response = await axios.get('/users/SearchUsers', {
      params: {
        query: searchQuery.value,
      },
    });
    searchResults.value = response.data;
  } catch (error) {
    console.error('Error searching users:', error);
  }
};

const handleSearchQueryChange = async () => {
  if (searchQuery.value === '') {
    searchResults.value = [];
  } else {
    await searchUsers();
  }
};

const getProfilePicture = async (userId: string) => {
  try {
    const response = await axios.get('/users/GetUserPicture', {
      params: { userId }
    });
    return response.data;
  } catch (error) {
    console.error(`Error fetching profile picture for user ${userId}`, error);
  }
};

const navigateToHome = () => {
  router.push('/home');
};

onMounted(() => {
  searchUsers();
});
</script>

<style scoped>
.search-card {
  width: 100%;
  max-width: 600px;
  margin: auto;
  padding: 16px;
}

.user-link {
  text-decoration: none;
  color: inherit;
  display: flex;
  align-items: center;
}

.user-list-item {
  margin-bottom: 8px;
}

.profile-picture {
  width: 48px;
  height: 48px;
  border-radius: 50%;
  object-fit: cover;
}

.user-username {
  margin-left: 16px;
}

.hoverable {
  transition: transform 0.3s ease, background-color 0.3s ease;
  padding: 8px;
  border-radius: 8px;
}

.hoverable:hover {
  background-color: #e0e0e0;
  transform: scale(1.02);
}

.v-card {
  padding: 12px;
  margin: 8px 0;
}

.v-card-title {
  margin-bottom: 16px;
}
</style>
