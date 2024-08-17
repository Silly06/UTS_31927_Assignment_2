<template>
  <v-container fluid>
    <!-- Stories Section -->
    <v-row class="stories-container">
      <v-col
          v-for="story in stories"
          :key="story.storyId"
          cols="auto"
          class="story-item"
      >
        <img
            :src="story.imageData ? toBase64(story.imageData) : defaultProfilePicture"
            class="story-avatar"
        />
      </v-col>
    </v-row>

    <!-- Posts Section -->
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

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import axios from 'axios';
import { useRouter } from 'vue-router';
import type { StoryDetailsDto } from "@/types/models";

const stories = ref<StoryDetailsDto[]>([]);
const posts = ref<any[]>([]);
const errorMessage = ref<string>('');

const defaultProfilePicture = 'https://via.placeholder.com/100?text=Profile+Picture';

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

const fetchStories = async () => {
  try {
    const response = await axios.get('/stories/GetAllStories');
    stories.value = response.data;
  } catch (error) {
    errorMessage.value = 'Failed to load stories';
    console.error(error);
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

const fetchPostDetails = async (postId: number) => {
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

const viewPost = (postId: number) => {
  router.push(`/ViewPost/${postId}`);
};

const toBase64 = (uint8Array: Uint8Array): string => {
  const binaryString = Array.from(uint8Array).map(byte => String.fromCharCode(byte)).join('');
  return `data:image/png;base64,${btoa(binaryString)}`;
};

// DO CREATE STORY
onMounted(() => {
  fetchStories();
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

.stories-container {
  overflow-x: auto;
  white-space: nowrap;
  padding: 10px 0;
  display: flex;
}

.story-item {
  display: inline-block;
  margin-right: 10px;
}

.story-avatar {
  width: 100px;
  height: 100px;
  border-radius: 50%;
  border: 2px solid #ddd;
  object-fit: cover;
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
