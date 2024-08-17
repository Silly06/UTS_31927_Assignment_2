<template>
  <v-container class="form-container">
    <v-btn @click="goBack" color="secondary" class="mb-4">
      Back
    </v-btn>

    <v-form @submit.prevent="createPost">
      <v-file-input
          v-model="image"
          label="Select an Image"
          @change="onFileChange"
          outlined
          dense
      ></v-file-input>

      <v-text-field
          v-model="description"
          label="Description"
          outlined
          dense
      ></v-text-field>

      <v-btn
          type="submit"
          color="primary"
          class="mt-4"
          :disabled="!description || !image"
      >
        Create Post
      </v-btn>
    </v-form>
  </v-container>
</template>

<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import axios from 'axios';

const description = ref('');
const image = ref(null);
const router = useRouter();

const onFileChange = (event) => {
  image.value = event.target.files[0];
};

const createPost = async () => {
  const userId = sessionStorage.getItem('userId');

  try {
    const formData = new FormData();
    formData.append('image', image.value);
    formData.append('description', description.value);
    formData.append('userId', userId);

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

const goBack = () => {
  router.push('/Home');
};
</script>

<style scoped>
.form-container {
  max-width: 600px;
  margin: 0 auto;
}

.v-file-input,
.v-text-field {
  margin-bottom: 16px;
}
</style>
