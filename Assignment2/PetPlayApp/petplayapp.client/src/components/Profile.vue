<template>
  <v-container>
    <v-row justify="center" align="center">
      <v-col cols="12" sm="6" md="4">
        <v-card>
          <v-card-title class="headline">{{ userDetails?.username }}</v-card-title>
          <img :src="`data:image/png;base64,${userDetails?.profilePicture}`" alt="Profile Picture" style="width: 100%; height: auto;"/>
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
            <!-- Conditionally render buttons based on ID match -->
            <v-btn v-if="isUserProfile" @click="editProfile" color="secondary" class="bottom-buttons">Edit Profile</v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>

    <!-- New Post Button -->
    <v-row v-if="isUserProfile" justify="center" class="mt-4">
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
import { ref, onMounted, computed } from 'vue';
import axios from 'axios';
import { useRoute, useRouter } from 'vue-router';
import { UserDetailsDto, UserInterest, UserStatus } from '@/types/models';

const route = useRoute();
const router = useRouter();
const userId = route.params.id as string;
const sessionUserId = sessionStorage.getItem('userId') || '';
const userDetails = ref<UserDetailsDto | null>(null);
const posts = ref<any[]>([]);
const errorMessage = ref('');
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
      posts.value = [];
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
  router.push('/NewPost');
};

const getInterestText = (interest: UserInterest | undefined): string => {
  return interest !== undefined ? UserInterest[interest] : 'Unlisted';
};

const getStatusText = (status: UserStatus | undefined): string => {
  return status !== undefined ? UserStatus[status] : 'Unlisted';
};

const isUserProfile = computed(() => userId === sessionUserId);

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
