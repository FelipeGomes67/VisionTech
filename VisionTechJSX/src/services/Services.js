import axios from "axios";

const apiPort = "7285";
const localApi = `https://localhost:${apiPort}/api`;

const api = axios.create({
    baseURL: localApi
});

// 👇 ADICIONE ISSO: remove o Content-Type default para o Axios detectar FormData corretamente
api.interceptors.request.use((config) => {
    if (config.data instanceof FormData) {
        delete config.headers['Content-Type'];
    }
    return config;
});

export const localAPIImagePath = `https://localhost:${apiPort}/imagens`;
export default api;