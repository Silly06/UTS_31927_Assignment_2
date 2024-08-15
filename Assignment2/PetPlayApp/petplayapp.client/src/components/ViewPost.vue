<script setup lang="ts">
    import { ref, onMounted } from 'vue';
    import { useRoute, useRouter } from 'vue-router';
    import axios from 'axios';

    const route = useRoute();
    const router = useRouter();
    const postId = route.params.id;

    const post = ref(null);

    const fetchPost = async () => {
        try {
            const response = await axios.get(`/api/posts/${postId}`);
            post.value = response.data;
        } catch (error) {
            console.error('Error fetching post:', error);
        }
    };

    const likePost = async () => {
        try {
            await axios.post(`/api/posts/${postId}/like`);
            // Optionally, update the post data to reflect the new like count
            fetchPost();
        } catch (error) {
            console.error('Error liking post:', error);
        }
    };

    onMounted(() => {
        fetchPost();
    });
</script>

<template>
    <div v-if="post">
        <img :src="post.imageUrl" alt="Post Image" />
        <p>{{ post.description }}</p>
        <div>
            <img :src="post.creator.profileImageUrl" alt="Creator Profile Image" />
            <a @click="() => router.push(`/profile/${post.creator.id}`)">{{ post.creator.name }}</a>
        </div>
        <button @click="likePost">Like</button>
        <button @click="() => router.push(`/posts/${postId}/comments`)">Comments</button>
    </div>
</template>
