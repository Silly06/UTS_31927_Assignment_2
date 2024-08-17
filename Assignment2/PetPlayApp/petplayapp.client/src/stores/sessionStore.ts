import { reactive } from 'vue';

export const state = reactive({
  userId: sessionStorage.getItem('userId') || ''
});

export function updateUserId(newId: string) {
  state.userId = newId;
  sessionStorage.setItem('userId', newId);
}

export function clearUserId() {
  state.userId = '';
  sessionStorage.removeItem('userId');
}
