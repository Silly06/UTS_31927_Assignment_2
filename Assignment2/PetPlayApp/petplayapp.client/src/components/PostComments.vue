<script setup lang="ts">
    import { ref, onMounted } from 'vue';
    import { useRoute } from 'vue-router';
    import axios from 'axios';

    const route = useRoute();
    const postId = route.params.id;

    const comments = ref([]);
    const newComment = ref('');

    const fetchComments = async () => {
        try {
            const response = await axios.get(`/api/posts/${postId}/comments`);
            comments.value = response.data.sort((a, b) => new Date(b.timestamp) - new Date(a.timestamp));
        } catch (error) {
            console.error('Error fetching comments:', error);
        }
    };

    const sendComment = async () => {
        try {
            await axios.post(`/api/posts/${postId}/comments`, { text: newComment.value });
            newComment.value = '';
            fetchComments();
        } catch (error) {
            console.error('Error sending comment:', error);
        }
    };

    onMounted(() => {
        fetchComments();
    });
</script>

<template>
    <div>
        <h2>Comments</h2>
        <div>
            <input v-model="newComment" placeholder="Write a comment..." />
            <button @click="sendComment">Send</button>
        </div>
        <ul>
            <li v-for="comment in comments" :key="comment.id">
                {{ comment.text }} - {{ new Date(comment.timestamp).toLocaleString() }}
            </li>
        </ul>
    </div>
</template>
