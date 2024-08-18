import 'vuetify/styles'
import { createVuetify } from 'vuetify'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'
import '@mdi/font/css/materialdesignicons.css'

import { createApp } from 'vue'
import { createRouter, createWebHistory } from 'vue-router'
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
import NewStory from "@/components/NewStory.vue";
import ViewStory from "@/components/ViewStory.vue";

const routes = [
    { path: '/', redirect: () => {
            return sessionStorage.getItem('userId') ? '/Home' : '/Login';
    }},
    { path: '/EditProfile', component: EditProfile },
    { path: '/Home', component: Home, meta: { requiresAuth: true } },
    { path: '/Login', component: Login },
    { path: '/Matches', component: Matches },
    { path: '/NewPost', component: NewPost },
    { path: '/NewStory', component: NewStory },
    { path: '/Notifications', component: Notifications, meta: { requiresAuth: true } },
    { path: '/PostComments/:id', component: PostComments },
    { path: '/Profile', component: Profile, meta: { requiresAuth: true} },
    { path: '/ResetPassword', component: ResetPassword },
    { path: '/Search', component: Search },
    { path: '/SignUp', component: SignUp },
    { path: '/ViewStory/:id', component: ViewStory },
    { path: '/PostComments/:id', component: PostComments },
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

const vuetify = createVuetify({
    components,
    directives,
    icons: {
        defaultSet: 'mdi',
    },
});

router.beforeEach((to, from, next) => {
    const isLoggedIn = !!sessionStorage.getItem('userId');

    if (to.meta.requiresAuth && !isLoggedIn) {
        next('/Login');
    } else if (to.path === '/Login' && isLoggedIn) {
        next('/Home');
    } else {
        next();
    }
});

createApp(App)
    .use(router)
    .use(vuetify)
    .mount('#app')
