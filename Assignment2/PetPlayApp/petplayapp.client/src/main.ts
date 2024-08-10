import './assets/main.css'

import { createApp } from 'vue'
import App from './App.vue'
import HomeVue from './components/Home.vue'

const routes = [
    { path: '/', component: HomeVue },
];

createApp(App).mount('#app')
