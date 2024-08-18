<template>
  <v-container>
    <v-row justify="center" align="center">
      <v-col cols="12" sm="6" md="4">
        <v-card>
          <v-card-title class="headline">{{ userDetails?.username }}</v-card-title>
          <img :src="getIconSource()" alt="Profile Picture" style="width: 100%; height: auto;"/>
          <v-card-text>
            <v-list>
              <v-list-item>
                <v-list-item-title>Email: {{ userDetails?.email }}</v-list-item-title>
                <v-list-item-title>Age: {{ userDetails?.age }}</v-list-item-title>
                <v-list-item-title>Bio: {{ userDetails?.bio }}</v-list-item-title>
                <v-list-item-title>Interest: {{ getInterestText(userDetails?.interest) }}</v-list-item-title>
                <v-list-item-title>Status: {{ getStatusText(userDetails?.status) }}</v-list-item-title>
              </v-list-item>
            </v-list>
            <v-card-actions class="justify-center">
              <v-btn @click="viewMatches" class="matches-button">View Matches</v-btn>
            </v-card-actions>
            <v-alert v-if="errorMessage" type="error" class="mt-4">
              {{ errorMessage }}
            </v-alert>
          </v-card-text>
          <v-card-actions class="justify-center">
            <v-btn @click="goBack" color="primary" class="bottom-buttons">Go Back</v-btn>
            <v-btn @click="editProfile" color="secondary" class="bottom-buttons">Edit Profile</v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>

    <!-- New Post Button -->
    <v-row justify="center" class="mt-4">
      <v-col cols="auto" class="text-center">
        <v-btn @click="createNewPost" color="primary" class="new-post-btn">
          <v-icon left>mdi-plus</v-icon>
          New Post
        </v-btn>
      </v-col>
    </v-row>

    <!-- User Posts Grid -->
    <v-row justify="center" class="mt-4">
      <v-col v-for="post in posts" :key="post.id" cols="12" sm="4" md="3">
        <v-card @click="viewPost(post.id)" class="pa-2" style="cursor: pointer;">
          <img :src="`data:image/png;base64,${post.imageData}`" alt="Post Image" style="width: 100%; height: 150px; object-fit: cover;"/>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import axios from 'axios';
import { useRouter } from 'vue-router';
import { UserDetailsDto, UserInterest, UserStatus } from '@/types/models';

const userId = sessionStorage.getItem('userId') || '';
const userDetails = ref<UserDetailsDto | null>(null);
const posts = ref<any[]>([]);
const errorMessage = ref('');
const router = useRouter();

const fetchUserDetails = async () => {
  if (!userId) {
    errorMessage.value = 'User ID not found';
    return;
  }

  try {
    const response = await axios.get(`/users/GetUserDetails`, {
      params: { userId }
    });
    userDetails.value = response.data;
  } catch (error) {
    errorMessage.value = 'Error fetching user details: ' + (error instanceof Error ? error.message : 'Unknown error');
  }
};

const fetchPosts = async () => {
  try {
    const response = await axios.get('/posts/GetUserPosts', {
      params: { page: 1, userid: userId },
    });
    const postIds = response.data;
    if (postIds.length === 0) {
      posts.value = []; // Assuming no dummy posts
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

const goBack = async () => {
  await router.push('/Home');
};

const editProfile = async () => {
  await router.push('/EditProfile');
};

const viewMatches = async () => {
  await router.push('/Matches');
};

const viewPost = (postId: string) => {
  router.push(`/ViewPost/${postId}`);
};

const createNewPost = () => {
  router.push('/NewPost'); // Ensure this route exists in your router setup
};

const getIconSource = () => {
  const imageSource = sessionStorage.getItem('userPfp');
  if (imageSource) {
    const byte = atob(imageSource);
    const byteNumbers = new Array(byte.length);

    for (let i = 0; i < byte.length; i++) {
      byteNumbers[i] = byte.charCodeAt(i);
    }

    const byteArray = new Uint8Array(byteNumbers);
    const blob = new Blob([byteArray], { type: 'image/png' });
    const urlCreator = window.URL || window.webkitURL;
    return urlCreator.createObjectURL(blob);
  }
  return '';
};

const getPostImageSource = (imageData: Uint8Array | undefined): string => {
  if (!imageData) return '';
  const blob = new Blob([imageData], { type: 'image/png' });
  const urlCreator = window.URL || window.webkitURL;
  return urlCreator.createObjectURL(blob);
};

const getInterestText = (interest: UserInterest | undefined): string => {
  return interest !== undefined ? UserInterest[interest] : 'Unlisted';
};

const getStatusText = (status: UserStatus | undefined): string => {
  return status !== undefined ? UserStatus[status] : 'Unlisted';
};

onMounted(() => {
  fetchUserDetails();
  fetchPosts();
});
</script>

<style>
.v-list-item-title {
  padding: 5px;
}
.new-post-btn {
  display: flex;
  align-items: center;
  justify-content: center;
}
</style>
