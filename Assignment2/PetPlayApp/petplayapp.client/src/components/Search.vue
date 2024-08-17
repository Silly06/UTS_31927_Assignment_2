<script setup lang="ts">
import { ref, onMounted } from 'vue';
import axios from 'axios';
import type { UserSearchDto } from "@/types/models";
import { useRouter } from "vue-router";

const searchQuery = ref('');
const searchResults = ref<UserSearchDto[]>([]);

const router = useRouter()

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

const navigateToHome = () => {
  router.push('/home');
};

onMounted(() => {
  searchUsers();
});
</script>

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
              >
                <router-link :to="`/Profile/${user.userId}`" class="user-link">
                  <v-card class="rounded hoverable">
                    <v-card-text class="text-center">
                      {{ user.username }}
                    </v-card-text>
                  </v-card>
                </router-link>
              </v-list-item>
            </v-list>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

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
}

.hoverable {
  transition: background-color 0.3s ease;
}

.hoverable:hover {
  background-color: #e0e0e0;
}

.rounded {
  border-radius: 12px;
}

.v-card {
  padding: 12px;
  margin: 8px 0;
}

.v-card-title {
  margin-bottom: 16px;
}
</style>
