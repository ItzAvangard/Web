import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import ValeraList from './components/ValeraList';
import ValeraStats from './components/ValeraStats';
import Login from './components/Login';
import Register from './components/Register';
import ProtectedRoute from './components/ProtectedRoute';
import { authUtils } from './utils/auth';
import './App.css';

function App() {
  return (
    <Router>
      <div className="App">
        <Routes>
          <Route 
            path="/login" 
            element={authUtils.isAuthenticated() ? <Navigate to="/" replace /> : <Login />} 
          />
          <Route 
            path="/register" 
            element={authUtils.isAuthenticated() ? <Navigate to="/" replace /> : <Register />} 
          />
          <Route
            path="/"
            element={
              <ProtectedRoute>
                <ValeraList />
              </ProtectedRoute>
            }
          />
          <Route
            path="/valera/:id"
            element={
              <ProtectedRoute>
                <ValeraStats />
              </ProtectedRoute>
            }
          />
          <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;

