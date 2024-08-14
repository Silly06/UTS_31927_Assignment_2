<script lang="ts">
    import { defineComponent } from 'vue'
    import AssetService from '@/services/AssetService'


    export default defineComponent({
        components: {
        },
        methods: {
            navigateToHome() {
                this.$router.push('/Home')
            },
            navigateToNotifications() {
                this.$router.push('/Notifications')
            },
            navigateToProfile() {
                this.$router.push('/Profile')
            },
        }
    })
</script>

<template>
    <div id="header">
        <button class="header-button" @click="navigateToHome">
            <img src="../assets/home.png" style="width: 70px;">
        </button>
        <button class="header-button" @click="navigateToNotifications">
            <img src="../assets/notification.png" style="width: 70px;">
        </button>
        <button class="header-button" @click="navigateToProfile">
            <img src="../assets/profile.png" style="width: 70px;">
        </button>
    </div>
</template>

<style scoped>
    .header-button {
        padding: 18px;
        margin-left: 100px;
        margin-top: 15px;
        float: right;
        vertical-align: top;
        font-weight: bold;
        font-size: larger;
        font-stretch: expanded;
        border: solid;
        border-width: 2px;
        background-color: skyblue;
        color: black;
        border-color: black;
        max-height: 110px
    }
        .header-button:hover {
            cursor: pointer;
            background-color: lightskyblue;
        }
</style>
