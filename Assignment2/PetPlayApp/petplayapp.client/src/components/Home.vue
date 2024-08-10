<script setup lang="ts">
</script>

<template>
<p>Home page</p>
</template>

<script setup lang="ts">
import axios from 'axios';

const getPosts = async () => {
  try {
    const response = await axios.get('/home/getposts');
    // Handle the response data here
  } catch (error) {
    // Handle the error here
  }
};

getPosts();
</script>
