import axios from 'axios';
import { authUtils } from '../utils/auth';

const API_BASE_URL = 'http://localhost:5066/api/valera';

// Настройка axios для автоматического добавления токена
axios.interceptors.request.use(
  (config) => {
    const token = authUtils.getToken();
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Обработка ошибок авторизации
axios.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      authUtils.clearAuth();
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

export interface ValeraDto {
  id: number;
  name: string;
  health: number;
  mana: number;
  cheerfulness: number;
  fatigue: number;
  money: number;
}

export interface CreateValeraDto {
  name: string;
  health: number;
  mana: number;
  cheerfulness: number;
  fatigue: number;
  money: number;
}

export const valeraApi = {
  // Получить все Валеры (только для админа)
  getAll: async (): Promise<ValeraDto[]> => {
    const response = await axios.get<ValeraDto[]>(API_BASE_URL);
    return response.data;
  },

  // Получить свои Валеры
  getMy: async (): Promise<ValeraDto[]> => {
    const response = await axios.get<ValeraDto[]>(`${API_BASE_URL}/my`);
    return response.data;
  },

  getById: async (id: number): Promise<ValeraDto> => {
    const response = await axios.get<ValeraDto>(`${API_BASE_URL}/${id}`);
    return response.data;
  },

  create: async (createDto: CreateValeraDto): Promise<ValeraDto> => {
    const response = await axios.post<ValeraDto>(API_BASE_URL, createDto);
    return response.data;
  },

  executeAction: async (id: number, action: string): Promise<ValeraDto> => {
    const response = await axios.post<ValeraDto>(
      `${API_BASE_URL}/${id}/action`,
      { action }
    );
    return response.data;
  },

  reset: async (id: number): Promise<ValeraDto> => {
    const response = await axios.put<ValeraDto>(`${API_BASE_URL}/${id}`);
    return response.data;
  },

  delete: async (id: number): Promise<void> => {
    await axios.delete(`${API_BASE_URL}/${id}`);
  },
};

