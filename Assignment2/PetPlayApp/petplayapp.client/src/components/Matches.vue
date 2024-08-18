<script setup lang="ts"></script>

<template>
    <v-container>
        <v-row>
            <v-col cols="12">
                <v-card>
                    <v-card-title>Matches</v-card-title>
                    <v-card-actions>
                        <v-btn @click="goBack" color="primary" class="back-button">Return to Profile</v-btn>
                    </v-card-actions>
                        <v-list>
                            <v-list-item v-for="match in matches" :key="generateKey(match)">
                                <v-list-item-title>{{ match.User1 }} - {{ match.User2 }}</v-list-item-title>
                            </v-list-item>
                        </v-list>
                </v-card>
            </v-col>
        </v-row>
    </v-container>
</template>

<script setup lang="ts">
    import { ref, onMounted } from 'vue';
    import axios from 'axios';
    import { useRouter } from 'vue-router';
    import { Match } from '@/types/models';

    const matches = ref<Match[]>([]);
    const router = useRouter();

    const fetchMatches = async () => {
        try {
            const response = await axios.get(`matches/GetMatches`, {
                params: { userId: sessionStorage.getItem('userId') }
            });
            matches.value = response.data;
        } catch (error) {
            console.error('Error fetching notifications:', error);
        }
    };


    const generateKey = (match: Match): string => {
        return `${match.user1Id}-${match.user2Id}`;
    };

    const goBack = async () => {
        await router.push('/Profile');
    };

    onMounted(() => {
        fetchMatches();
    });
</script>

<style scoped>
    .v-card {
        max-width: 600px;
        margin: auto;
    }

    .v-list-item {
        margin-bottom: 10px;
    }

    .back-button {
        font-size: 12px;
        margin-bottom: 12px;
    }
</style>