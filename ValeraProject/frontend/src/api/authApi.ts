import axios from 'axios';

const API_BASE_URL = 'http://localhost:5066/api/auth';

export interface RegisterDto {
  email: string;
  password: string;
  username: string;
}

export interface LoginDto {
  email: string;
  password: string;
}

export interface AuthResponseDto {
  token: string;
  userId: number;
  email: string;
  username: string;
  role: string;
}

export const authApi = {
  register: async (registerDto: RegisterDto): Promise<AuthResponseDto> => {
    const response = await axios.post<AuthResponseDto>(
      `${API_BASE_URL}/register`,
      registerDto
    );
    return response.data;
  },

  login: async (loginDto: LoginDto): Promise<AuthResponseDto> => {
    const response = await axios.post<AuthResponseDto>(
      `${API_BASE_URL}/login`,
      loginDto
    );
    return response.data;
  },
};

