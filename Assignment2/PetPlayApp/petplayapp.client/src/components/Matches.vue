<template>
  <v-container>
    <v-row>
      <v-col cols="12">
        <v-card>
          <v-card-title>Matches</v-card-title>
          <v-card-actions>
            <v-btn @click="goBack" color="primary" class="back-button">Back</v-btn>
          </v-card-actions>
          <v-list>
            <v-list-item v-for="match in matches" :key="match.id">
              <v-list-item-title>{{ match.user1Name }} - {{ match.user2Name }}</v-list-item-title>
              <v-list-item-subtitle>First Response: {{ getMatchResponseText(match.response1!) }}</v-list-item-subtitle>
              <v-list-item-subtitle>Second Response: {{ getMatchResponseText(match.response2!) }}</v-list-item-subtitle>
              <v-list-item-subtitle>Match Status: {{ getMatchStatusText(match.matchStatus!) }}</v-list-item-subtitle>
                <v-actions-container>
                    <v-btn class="match-response-button" color="green" @click="accept(match)">Accept</v-btn>
                    <v-btn class="match-response-button" color="red" @click="decline(match)">Decline</v-btn>
                </v-actions-container>
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
import { MatchDetailsDto, UserResponse, MatchStatus } from '@/types/models';

const matches = ref<MatchDetailsDto[]>([]);
const router = useRouter();

const fetchMatches = async () => {
  try {
    const response = await axios.get('matches/GetMatches', {
      params: { userId: sessionStorage.getItem('userId') }
    });
    matches.value = response.data;
  } catch (error) {
    console.error('Error fetching matches:', error);
  }
};

const getMatchResponseText = (matchResponse: number) => {
  switch (matchResponse) {
    case UserResponse.Accepted:
      return 'Accepted';
    case UserResponse.Rejected:
      return 'Rejected';
    case UserResponse.Pending:
      return 'Pending';
    default:
      return 'Unknown';
  }
};

const getMatchStatusText = (matchStatus: number) => {
  switch (matchStatus) {
    case MatchStatus.Success:
      return 'Match Found';
    case MatchStatus.Failure:
      return 'No Match';
    case MatchStatus.AwaitingResponse:
      return 'Awaiting Response';
    default:
      return 'Unknown Status';
  }
};

const goBack = async () => {
  await router.push('/Home');
    };

    const accept = async (match: MatchDetailsDto) => {

        await axios.get('/matches/MatchResponse', {
            params: {
                respondingUser: sessionStorage.getItem('userId'),
                user1: match.user1Name,
                user2: match.user2Name,
                response: 'Accepted'
            }
        });
    };

    const decline = async (match: MatchDetailsDto) => {

        await axios.get('/matches/MatchResponse', {
            params: {
                respondingUser: sessionStorage.getItem('userId'),
                user1: match.user1Name,
                user2: match.user2Name,
                response: 'Declined'
            }
        });
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

.match-response-button {
    margin: 3px;
}
</style>
