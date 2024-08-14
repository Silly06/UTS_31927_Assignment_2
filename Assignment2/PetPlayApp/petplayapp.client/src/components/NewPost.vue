<script setup lang="ts"></script>

<template>
    <div>
        <form @submit.prevent="createPost">
            <input type="file" @change="onFileChange" />
            <input type="text" v-model="description" placeholder="Description" />
            <button type="submit">Create Post</button>
        </form>
    </div>
</template>

<script setup>
    import { ref } from 'vue';
    import axios from 'axios';

    const description = ref('');
    const image = ref(null);

    const onFileChange = (event) => {
        image.value = event.target.files[0];
    };

    const createPost = async () => {
        const formData = new FormData();
        formData.append('image', image.value);
        formData.append('description', description.value);
        const postCreatorId = localStorage.getItem('userId');
        formData.append('postCreatorId', postCreatorId);

        try {
            const response = await axios.post('/posts/CreatePost', formData, {
                headers: {
                    'Content-Type': 'multipart/form-data'
                }
            });
            console.log('Post created:', response.data);
        } catch (error) {
            console.error('Error creating post:', error);
        }
    };
</script>
