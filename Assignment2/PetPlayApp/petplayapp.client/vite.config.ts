import { fileURLToPath, URL } from 'node:url';

import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-vue';
import fs from 'fs';
import path from 'path';
import child_process from 'child_process';
import { env } from 'process';

const baseFolder =
    env.APPDATA !== undefined && env.APPDATA !== ''
        ? `${env.APPDATA}/ASP.NET/https`
        : `${env.HOME}/.aspnet/https`;

const certificateName = "petplayapp.client";
const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
    if (0 !== child_process.spawnSync('dotnet', [
        'dev-certs',
        'https',
        '--export-path',
        certFilePath,
        '--format',
        'Pem',
        '--no-password',
    ], { stdio: 'inherit', }).status) {
        throw new Error("Could not create certificate.");
    }
}

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
    env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://localhost:7154';

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [plugin()],
    resolve: {
        alias: {
            '@': fileURLToPath(new URL('./src', import.meta.url))
        }
    },
    server: {
        proxy: {
            '^/users/login': {
                target,
                secure: false
            },
            '^/posts/GetRecentPosts': {
                target,
                secure: false
            },
            '^/posts/GetUserPosts': {
                target,
                secure: false
            },
            '^/posts/GetPostDetails': {
                target,
                secure: false
            },
            '^/posts/NewPost': {
                target,
                secure: false
            },
            '^/users/GetUserDetails': {
                target,
                secure: false
            },
            '^/users/UpdateUserDetails': {
                target,
                secure: false
            },
            '^/users/CreateUser': {
                target,
                secure: false
            },
            '^/users/SearchUsers': {
                target,
                secure: false
            },
            '^/stories/CreateStory': {
                target,
                secure: false
            },
            '^/stories/GetAllStories': {
                target,
                secure: false
            },
            '^/users/GetUserPicture': {
                target,
                secure: false
            },
            '^/stories/GetStoryDetails': {
                target,
                secure: false
            },
            '^/matches/GetMatches': {
                target,
                secure: false
            },
            '^/users/ResetPassword': {
                target,
                secure: false
            },
            '^/posts/LikePost': {
                target,
                secure: false
            },
            '^/posts/UnlikePost': {
                target,
                secure: false
            },
            '^/comments/AddComment': {
                target,
                secure: false
            },
            '^/comments/GetComments': {
                target,
                secure: false
            },
            '^/comments/LikeComment': {
                target,
                secure: false
            },
            '^/comments/UnlikeComment': {
                target,
                secure: false
            },
            '^/matches/CheckForMatch': {
                target,
                secure: false
            },
            '^/matches/MatchResponse': {
                target,
                secure: false
            },
            '^/notifications/GetRecentNotifications': {
                target,
                secure: false
            },
        },
        port: 5173,
        https: {
            key: fs.readFileSync(keyFilePath),
            cert: fs.readFileSync(certFilePath),
        }
    }
})
