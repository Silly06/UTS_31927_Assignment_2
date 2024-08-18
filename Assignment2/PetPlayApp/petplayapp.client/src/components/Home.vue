<template>
  <v-container fluid>
    <!-- Stories Section -->
    <v-row class="stories-container">
      <!-- Add Story Button -->
      <v-col cols="auto" class="add-story-item" @click="goToNewStory">
        <v-btn icon class="add-story-button">
          <v-icon>mdi-plus</v-icon>
        </v-btn>
      </v-col>

      <!-- Story Items -->
      <v-col
          v-for="story in stories"
          :key="story.storyId"
          cols="auto"
          class="story-item"
          @click="viewStory(story.storyId!)"
      >
        <img
            :src="`data:image/png;base64,${story.storyProfilePicture}`"
            class="story-avatar"
            alt="Story Profile Picture"
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
              <v-btn @click="toggleLike(post.id)" :color="post.likedByUser ? 'red' : 'grey'" icon>
                <v-icon>{{ post.likedByUser ? 'mdi-heart' : 'mdi-heart-outline' }}</v-icon>
              </v-btn>
              <span class="like-count">{{ post.likesCount }}</span>
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
    const storiesData = response.data as StoryDetailsDto[];

    for (const story of storiesData) {
      story.storyProfilePicture = await getStoryProfilePicture(story.storyCreatorId!);
    }

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

const fetchPostDetails = async (postId: string) => {
  try {
    const response = await axios.get(`/posts/GetPostDetails`, {
      params: { postid: postId }
    });
    const post = response.data;
    post.likesCount = post.likes.length;
    return post;
  } catch (error) {
    console.error(`Error fetching details for post ${postId}:`, error);
    return null;
  }
};

const getStoryProfilePicture = async (userId: string) => {
  try {
    const response = await axios.get('/users/GetUserPicture', {
      params: { userId }
    });
    return response.data;
  } catch (error) {
    console.error(`Error fetching profile picture for user ${userId}`, error);
  }
}

const toggleLike = async (postId: string) => {
  try {
    const userId = sessionStorage.getItem('userId');
    const post = posts.value.find(p => p.id === postId);

    if (post && userId) {
      const action = post.likedByUser ? 'UnlikePost' : 'LikePost';
      await axios.post(`/posts/${action}`, {
        postId,
        userId
      });

      post.likedByUser = !post.likedByUser;
      post.likesCount += post.likedByUser ? 1 : -1;
    }
  } catch (error) {
    errorMessage.value = 'Failed to like/unlike post';
    console.error(error);
  }
};

const viewStory = (storyId: string) => {
  router.push(`/ViewStory/${storyId}`);
};

const goToNewStory = () => {
  router.push('/NewStory');
};

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
  align-items: center;
}

.add-story-item {
  display: flex;
  align-items: center;
  margin-right: 10px;
}

.add-story-button {
  width: 100px;
  height: 100px;
  border-radius: 50%;
  background-color: #007bff;
  color: white;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  transition: background-color 0.3s ease;
}

.add-story-button:hover {
  background-color: #0056b3;
}

.story-item {
  display: inline-block;
  margin-right: 10px;
  border: 2px solid #ddd;
  border-radius: 50%;
  cursor: pointer;
  transition: transform 0.3s ease, border-color 0.3s ease;
}

.story-item:hover {
  transform: scale(1.05);
  border-color: #007bff;
}

.story-avatar {
  width: 100px;
  height: 100px;
  border-radius: 50%;
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

.like-count {
  margin-left: 10px;
  font-weight: bold;
  color: #555;
}
</style>
