<template>
  <v-container fluid>
    <v-row justify="center">
      <v-col cols="12" md="8">
        <p class="text-center">Home page</p>
        <v-alert v-if="errorMessage" type="error" dismissible>{{ errorMessage }}</v-alert>
        <v-row v-else>
          <v-col v-for="post in posts" :key="post.id" cols="12" sm="6" md="4">
            <v-card>
              <v-img :src="`data:image/png;base64,${post.imageData}`" alt="Post Image" v-if="post.imageData"></v-img>
              <v-card-text>{{ post.description }}</v-card-text>
            </v-card>
          </v-col>
        </v-row>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="js">
import { ref, onMounted } from 'vue';
import axios from 'axios';

const posts = ref([]);
const errorMessage = ref('');

const fetchPostDetails = async (postId) => {
  try {
    const response = await axios.get('/posts/GetPostDetails', {
      params: { postid: postId },
    });
    return response.data;
  } catch (error) {
    console.error(`Error fetching details for post ${postId}:`, error);
    return null;
  }
};

const fetchPosts = async () => {
  try {
    const response = await axios.get('/posts/GetRecentPosts', {
      params: { page: 1 },
    });
    const postIds = response.data;
    const postDetailsPromises = postIds.map(fetchPostDetails);
    const postDetails = await Promise.all(postDetailsPromises);
    posts.value = postDetails.filter((post) => post !== null);
  } catch (error) {
    errorMessage.value = 'Failed to load posts';
    console.error(error);
  }
};

onMounted(() => {
  fetchPosts();
});
</script>

<style scoped>
.text-center {
  text-align: center;
}

.v-container {
  margin-top: 20px;
}

.v-card {
  max-width: 100%;
  margin-bottom: 20px;
}

.v-card-text {
  white-space: pre-wrap;
}
</style>
