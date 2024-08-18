<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRoute } from 'vue-router';
import axios from 'axios';

const route = useRoute();
const postId = route.params.id as string;

const userId = sessionStorage.getItem('userId') as string;

const comments = ref<{ id: string; content: string; createdAt: Date; postId: string; }[]>([]);
const newComment = ref<string>('');

const fetchComments = async () => {
  try {
    const response = await axios.get(`/comments/GetComments`, {
      params: { postId }
    });
    comments.value = response.data;
  } catch (error) {
    console.error('Error fetching comments:', error);
  }
};

const sendComment = async () => {
  try {
    const formData = new FormData();
    formData.append('postId', postId);
    formData.append('userId', userId);
    formData.append('content', newComment.value);
    
    const response = await axios.post(`/comments/AddComment`, formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    });
    
    console.log(response.data);
    newComment.value = '';
    await fetchComments();
  } catch (error) {
    console.error('Error sending comment:', error);
  }
};

onMounted(() => {
  fetchComments();
});
</script>

<template>
  <v-container>
    <v-row>
      <v-col cols="12">
        <h2>Comments</h2>
      </v-col>
    </v-row>

    <v-row>
      <v-col cols="12">
        <v-list
            style="max-height: calc(100vh - 150px); overflow-y: auto;"
        >
          <v-list-item
              v-for="comment in comments"
              :key="comment.id"
          >
            <v-list-item-content>
              <v-list-item-title>{{ comment.content }}</v-list-item-title>
              <v-list-item-subtitle>{{ new Date(comment.createdAt).toLocaleString() }}</v-list-item-subtitle>
            </v-list-item-content>
          </v-list-item>
        </v-list>
      </v-col>
    </v-row>

    <v-row>
      <v-col cols="12">
        <v-text-field
            v-model="newComment"
            placeholder="Write a comment..."
            clearable
        />
        <v-btn @click="sendComment" color="primary">Send</v-btn>
      </v-col>
    </v-row>
  </v-container>
</template>
