import './main.css';
import { createApp } from 'vue'
import App from './App.vue'
import {axiosInstance} from "@kubb/plugin-client/clients/axios";

axiosInstance.defaults.baseURL = import.meta.env.VITE_BACKEND_URL;

createApp(App).mount('#app')
