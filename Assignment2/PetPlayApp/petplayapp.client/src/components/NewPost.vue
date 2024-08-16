<template>
    <div>
        <form @submit.prevent="createPost">
            <input type="file" @change="onFileChange" />
            <input type="text" v-model="description" placeholder="Description" />
            <button type="submit">Create Post</button>
        </form>
    </div>
</template>

<script setup lang="js">
    import { ref } from 'vue';
    import axios from 'axios';

    const description = ref('');
    const image = ref(null);

    const onFileChange = (event) => {
        image.value = event.target.files[0];
    };

    const createPost = async () => {
        const userId = localStorage.getItem('userId');
        const response = await axios.post('/users/login', {
            image: image.value,
            description: description.value,
            userId: userId.value
        });

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
