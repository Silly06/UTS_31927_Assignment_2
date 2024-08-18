<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import axios from 'axios';

const route = useRoute();
const router = useRouter();
const postId = route.params.id as string;

const post = ref<any>(null);

const fetchPost = async () => {
  try {
    const response = await axios.get(`/posts/GetPostDetails`, {
      params: { postid: postId }
    });
    const postData = response.data;
    postData.likesCount = postData.likes.length;
    post.value = postData;
  } catch (error) {
    console.error('Error fetching post:', error);
  }
};

const toggleLike = async () => {
  try {
    const userId = sessionStorage.getItem('userId');
    const postToUpdate = post.value;

    if (postToUpdate && userId) {
      const action = postToUpdate.likedByUser ? 'UnlikePost' : 'LikePost';
      await axios.post(`/posts/${action}`, {
        postId,
        userId
      });

      postToUpdate.likedByUser = !postToUpdate.likedByUser;
      postToUpdate.likesCount += postToUpdate.likedByUser ? 1 : -1;
    }
  } catch (error) {
    console.error('Error liking/unliking post:', error);
  }
};

onMounted(() => {
  fetchPost();
});
</script>

<template>
  <div v-if="post" class="post-container">
    <img :src="post.imageData ? `data:image/png;base64,${post.imageData}` : 'https://via.placeholder.com/600x400'" alt="Post Image" class="post-image"/>
    <p class="post-description">{{ post.description }}</p>
    <div class="post-creator">
      <img :src="post.creator.profileImageUrl ? `data:image/png;base64,${post.creator.profileImageUrl}` : 'https://via.placeholder.com/50'" alt="Creator Profile Image" class="creator-image"/>
      <a @click="() => router.push(`/Profile/${post.creator.id}`)" class="creator-name">{{ post.creator.name }}</a>
    </div>
    <div class="post-actions">
      <button @click="toggleLike" :class="['like-button', post.likedByUser ? 'liked' : '']">Like {{ post.likesCount }}</button>
    </div>
  </div>
</template>

<style scoped>
.post-container {
  max-width: 800px;
  margin: 0 auto;
  padding: 20px;
}

.post-image {
  width: 100%;
  height: auto;
  border-radius: 8px;
}

.post-description {
  margin: 20px 0;
  font-size: 16px;
}

.post-creator {
  display: flex;
  align-items: center;
  margin-bottom: 20px;
}

.creator-image {
  width: 50px;
  height: 50px;
  border-radius: 50%;
  margin-right: 10px;
}

.creator-name {
  font-size: 16px;
  color: #007bff;
  cursor: pointer;
  text-decoration: underline;
}

.post-actions {
  display: flex;
  justify-content: space-between;
}

.like-button, .comments-button {
  background-color: #007bff;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 4px;
  cursor: pointer;
  transition: background-color 0.3s;
}

.like-button.liked {
  background-color: #ff5a5f;
}

.like-button:hover, .comments-button:hover {
  background-color: #0056b3;
}
</style>
