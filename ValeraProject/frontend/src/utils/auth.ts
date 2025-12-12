import { AuthResponseDto } from '../api/authApi';

const TOKEN_KEY = 'auth_token';
const USER_KEY = 'auth_user';

export const authUtils = {
  setAuth: (authData: AuthResponseDto): void => {
    localStorage.setItem(TOKEN_KEY, authData.token);
    localStorage.setItem(USER_KEY, JSON.stringify({
      userId: authData.userId,
      email: authData.email,
      username: authData.username,
      role: authData.role,
    }));
  },

  getToken: (): string | null => {
    return localStorage.getItem(TOKEN_KEY);
  },

  getUser: (): { userId: number; email: string; username: string; role: string } | null => {
    const userStr = localStorage.getItem(USER_KEY);
    if (!userStr) return null;
    try {
      return JSON.parse(userStr);
    } catch {
      return null;
    }
  },

  isAuthenticated: (): boolean => {
    return !!authUtils.getToken();
  },

  isAdmin: (): boolean => {
    const user = authUtils.getUser();
    return user?.role === 'Admin';
  },

  clearAuth: (): void => {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(USER_KEY);
  },
};

