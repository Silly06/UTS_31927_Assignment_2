<script setup lang="js">
    import { ref, onMounted } from 'vue';
    import { useRoute, useRouter } from 'vue-router';
    import axios from 'axios';

    const route = useRoute();
    const router = useRouter();
    const postId = route.params.id;

    const post = ref(null);

    const fetchPost = async () => {
        try {
            const response = await axios.get('/posts/GetPostDetails', {
                params: { postid: postId },
            });
            post.value = response.data;
        } catch (error) {
            console.error('Error fetching post:', error);
        }
    };

    const likePost = async () => {
        try {
            await axios.post(`/api/posts/${postId}/like`);
            // Optionally, update the post data to reflect the new like count
            await fetchPost();
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
        <v-card class="mx-auto my-4" max-width="600">
            <v-img :src="post.imageData ? `data:image/png;base64,${post.imageData}` : 'https://via.placeholder.com/600x400'" alt="Post Image"></v-img>
            <v-card-title>{{ post.title }}</v-card-title>
            <v-card-subtitle>{{ post.date }}</v-card-subtitle>
            <v-card-text>{{ post.description }}</v-card-text>
            <v-card-actions>
                <v-btn color="primary" @click="viewPost(post.id)">View Details</v-btn>
            </v-card-actions>
        </v-card>
    </div>
</template>
