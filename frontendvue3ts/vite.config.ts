import {fileURLToPath, URL} from 'node:url'

import {build, defineConfig, loadEnv} from 'vite'
import vue from '@vitejs/plugin-vue'
import vueDevTools from 'vite-plugin-vue-devtools'
import cssInjectedByJsPlugin from "vite-plugin-css-injected-by-js";

// https://vite.dev/config/
export default defineConfig(({mode}) => {
    return {
        plugins: [
            vue(),
            vueDevTools(),
            cssInjectedByJsPlugin()
        ],
        resolve: {
            alias: {
                '@': fileURLToPath(new URL('./src', import.meta.url))
            },
        },
        server: {
            port: 8082
        }
    }
})
