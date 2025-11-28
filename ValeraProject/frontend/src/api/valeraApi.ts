import axios from 'axios';

const API_BASE_URL = 'http://localhost:5066/api/valera';

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
  getAll: async (): Promise<ValeraDto[]> => {
    const response = await axios.get<ValeraDto[]>(API_BASE_URL);
    return response.data;
  },

  getById: async (id: number): Promise<ValeraDto> => {
    const response = await axios.get<ValeraDto>(`${API_BASE_URL}?id=${id}`);
    return response.data;
  },

  create: async (createDto: CreateValeraDto): Promise<ValeraDto> => {
    const response = await axios.post<ValeraDto>(API_BASE_URL, createDto);
    return response.data;
  },

  executeAction: async (id: number, action: string): Promise<ValeraDto> => {
    const response = await axios.post<ValeraDto>(
      `${API_BASE_URL}/action?id=${id}`,
      { action }
    );
    return response.data;
  },

  delete: async (id: number): Promise<void> => {
    await axios.delete(`${API_BASE_URL}?id=${id}`);
  },
};

