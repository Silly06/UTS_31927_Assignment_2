<template>
  <v-container class="form-container">
    <v-btn @click="goBack" color="secondary" class="mb-4">
      Back
    </v-btn>

    <v-card class="mx-auto" max-width="600">
      <v-card-title class="text-center">Create a New Story</v-card-title>
      <v-card-text>
        <v-form @submit.prevent="createStory">
          <v-file-input
            v-model="image"
            label="Select an Image"
            @change="onFileChange"
            outlined
            dense
          ></v-file-input>

          <v-btn
            type="submit"
            color="primary"
            class="mt-4"
            :disabled="!image"
          >
            Create Story
          </v-btn>
        </v-form>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import axios from 'axios';

const image = ref<File | null>(null);
const router = useRouter();

const onFileChange = (event: Event) => {
  const target = event.target as HTMLInputElement;
  if (target.files) {
    image.value = target.files[0];
  }
};

const createStory = async () => {
  const storyCreatorId = sessionStorage.getItem('userId');

  if (!storyCreatorId) {
    console.error('StoryCreatorId is required');
    return;
  }

  if (!image.value) {
    console.error('Image is required');
    return;
  }

  try {
    const formData = new FormData();
    formData.append('storyCreatorId', storyCreatorId);
    formData.append('file', image.value);

    const response = await axios.post('/stories/CreateStory', formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    });
    console.log('Story created:', response.data);
    await router.push('/Home');
  } catch (error) {
    console.error('Error creating story:', error);
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

.v-file-input {
  margin-bottom: 16px;
}
</style>
