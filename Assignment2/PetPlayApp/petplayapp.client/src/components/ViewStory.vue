<template>
  <v-container fluid fill-height>
    <v-row justify="center" align="center">
      <v-col cols="auto">
        <div v-if="story" class="story-card">
          <div class="story-image-container">
            <img :src="`data:image/png;base64,${story.imageData}`" class="story-image" alt="Story Image" />
            <div class="story-header">
              <v-btn icon @click="goHome" class="back-button">
                <v-icon>mdi-arrow-left</v-icon>
              </v-btn>
              <img :src="`data:image/png;base64,${story.storyProfilePicture}`" class="profile-picture" alt="Profile Picture" />
              <div class="story-creator">
                <h3>{{ story.storyCreatorName }}</h3>
                <p>{{ timePosted }}</p>
              </div>
            </div>
          </div>
        </div>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue';
import axios from 'axios';
import { useRoute, useRouter } from 'vue-router';
import type { StoryDetailsDto } from "@/types/models";

const story = ref<StoryDetailsDto | null>(null);
const route = useRoute();
const router = useRouter();

const fetchStoryDetails = async () => {
  const storyId = route.params.id as string;
  try {
    const response = await axios.get(`/stories/GetStoryDetails`, {
      params: { storyId }
    });
    const storyData = response.data as StoryDetailsDto;
    storyData.storyProfilePicture = await getStoryProfilePicture(storyData.storyCreatorId!);
    story.value = storyData;
  } catch (error) {
    console.error('Error fetching story details:', error);
  }
};

const getStoryProfilePicture = async (userId: string) => {
  try {
    const response = await axios.get('/users/GetUserPicture', {
      params: { userId }
    });
    return response.data;
  } catch (error) {
    console.error(`Error fetching profile picture for user ${userId}`, error);
  }
};

const timePosted = computed(() => {
  if (!story.value || !story.value.dateTimePosted) return '';

  const now = new Date();
  const postedDate = new Date(story.value.dateTimePosted);

  const nowUTC = Date.UTC(now.getUTCFullYear(), now.getUTCMonth(), now.getUTCDate(), now.getUTCHours(), now.getUTCMinutes(), now.getUTCSeconds());
  const postedDateUTC = Date.UTC(postedDate.getUTCFullYear(), postedDate.getUTCMonth(), postedDate.getUTCDate(), postedDate.getUTCHours(), postedDate.getUTCMinutes(), postedDate.getUTCSeconds());

  const diffInHours = Math.floor((nowUTC - postedDateUTC) / (1000 * 60 * 60));
  return `${diffInHours} hours ago`;
});


const goHome = () => {
  router.push('/');
};

onMounted(() => {
  fetchStoryDetails();
});
</script>

<style scoped>
.story-card {
  max-width: 600px;
  width: 100%;
  margin: auto;
  position: relative;
  display: flex;
  flex-direction: column;
  align-items: center;
}

.story-image-container {
  position: relative;
}

.story-image {
  width: 100%;
  height: auto;
  max-height: 400px;
  object-fit: cover;
}

.story-header {
  position: absolute;
  top: 16px;
  left: 16px;
  display: flex;
  align-items: center;
  background: rgba(0, 0, 0, 0.5);
  padding: 8px;
  border-radius: 8px;
}

.back-button {
  margin-right: 12px;
}

.profile-picture {
  width: 60px;
  height: 60px;
  border-radius: 50%;
  object-fit: cover;
  margin-right: 12px;
}

.story-creator {
  color: white;
  text-align: left;
}

.story-creator h3 {
  margin: 0;
}

.story-creator p {
  margin: 0;
  font-size: 14px;
}
</style>
