<template>
  <v-container fluid>
    <v-row justify="center">
      <v-col cols="12" md="8">
        <p class="text-center">For You</p>
        <v-alert v-if="errorMessage" type="error" dismissible>{{ errorMessage }}</v-alert>
        <v-col v-for="post in posts" :key="post.id" cols="12">
          <v-card class="mx-auto my-4" max-width="600">
            <v-img :src="post.imageData ? `data:image/png;base64,${post.imageData}` : 'https://via.placeholder.com/600x400'" alt="Post Image"></v-img>
            <v-card-title>{{ post.title }}</v-card-title>
            <v-card-subtitle>{{ post.date }}</v-card-subtitle>
            <v-card-text>{{ post.description }}</v-card-text>
            <v-card-actions>
              <v-btn color="primary" @click="viewPost(post.id)">View Details</v-btn>
            </v-card-actions>
          </v-card>
        </v-col>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="js">
import { ref, onMounted } from 'vue';
import axios from 'axios';
import {useRouter} from "vue-router";

const posts = ref([]);
const errorMessage = ref('');

const router = useRouter();

const dummyPosts = [
  {
    id: 1,
    title: 'First Dummy Post',
    description: 'This is a description of the first dummy post.',
    imageData: '',
    date: '2024-08-16'
  },
  {
    id: 2,
    title: 'Second Dummy Post',
    description: 'This is a description of the second dummy post.',
    imageData: '',
    date: '2024-08-15'
  },
];

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
    if (postIds.length === 0) {
      posts.value = dummyPosts;
    } else {
      const postDetailsPromises = postIds.map(fetchPostDetails);
      const postDetails = await Promise.all(postDetailsPromises);
      posts.value = postDetails.filter((post) => post !== null);
    }
  } catch (error) {
    errorMessage.value = 'Failed to load posts';
    console.error(error);
  }
};

const viewPost = (postId) => {
  router.push(`/ViewPost/${postId}`);
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

.v-card-title {
  font-weight: bold;
}

.v-card-subtitle {
  color: #888;
}

.v-card img {
  width: 100%;
  height: auto;
}
</style>
