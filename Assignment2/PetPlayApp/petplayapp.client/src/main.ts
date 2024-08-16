import './assets/main.css'

import { createApp } from 'vue'
import { createRouter, createWebHashHistory } from 'vue-router';
import App from './App.vue'
import EditProfile from './components/EditProfile.vue'
import Home from './components/Home.vue'
import Login from './components/Login.vue'
import Matches from './components/Matches.vue'
import NewPost from './components/NewPost.vue'
import Notifications from './components/Notifications.vue'
import PostComments from './components/PostComments.vue'
import Profile from './components/Profile.vue'
import ResetPassword from './components/ResetPassword.vue'
import Search from './components/Search.vue'
import SignUp from './components/SignUp.vue'
import ViewPost from './components/ViewPost.vue'

const routes = [
    { path: '/EditProfile', component: EditProfile },
    { path: '/Home', component: Home },
    { path: '/Login', component: Login },
    { path: '/Matches', component: Matches },
    { path: '/NewPost', component: NewPost },
    { path: '/Notifications', component: Notifications },
    { path: '/PostComments', component: PostComments },
    { path: '/Profile', component: Profile },
    { path: '/ResetPassword', component: ResetPassword },
    { path: '/Search', component: Search },
    { path: '/SignUp', component: SignUp },
    { path: '/ViewPost/:id', component: ViewPost },
    { path: '/PostComments/:id', component: PostComments },
];

const router = createRouter({
    history: createWebHashHistory(),
    routes,
});

createApp(App)
    .use(router)
    .mount('#app')
