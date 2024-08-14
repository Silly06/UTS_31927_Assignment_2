<template>
    <div>
        <p>Home page</p>
        <div v-if="errorMessage">{{ errorMessage }}</div>
        <div v-else>
            <div class="grid">
                <div v-for="post in posts" :key="post.id" class="post">
                    <img :src="`data:image/png;base64,${post.imageData}`" alt="Post Image" v-if="post.imageData" />
                    <p>{{ post.description }}</p>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
    import { ref, onMounted } from 'vue';
    import axios from 'axios';

    const posts = ref([]);
    const errorMessage = ref('');

    const fetchPostDetails = async (postId) => {
        try {
            const response = await axios.get('/posts/GetPostDetails', {
                params: { postid: postId }
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
                params: { page: 1 }
            });
            const postIds = response.data;
            const postDetailsPromises = postIds.map(fetchPostDetails);
            const postDetails = await Promise.all(postDetailsPromises);
            posts.value = postDetails.filter(post => post !== null);
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
    .grid {
        display: flex;
        flex-wrap: wrap;
        gap: 16px;
    }

    .post {
        border: 1px solid #ccc;
        padding: 16px;
        width: 200px;
    }

        .post img {
            max-width: 100%;
            height: auto;
        }
</style>
