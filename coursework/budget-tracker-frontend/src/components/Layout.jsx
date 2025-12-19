import { Outlet, Link, useNavigate } from 'react-router-dom'
import { useAuth } from '../contexts/AuthContext'
import './Layout.css'

function Layout() {
  const { logout } = useAuth()
  const navigate = useNavigate()

  const handleLogout = () => {
    logout()
    navigate('/login')
  }

  return (
    <div className="layout">
      <nav className="navbar">
        <div className="nav-container">
          <h1 className="nav-title">💰 Бюджет</h1>
          <div className="nav-links">
            <Link to="/" className="nav-link">Главная</Link>
            <Link to="/transactions" className="nav-link">Транзакции</Link>
            <Link to="/categories" className="nav-link">Категории</Link>
            <button onClick={handleLogout} className="nav-link logout-btn">
              Выход
            </button>
          </div>
        </div>
      </nav>
      <main className="main-content">
        <Outlet />
      </main>
    </div>
  )
}

export default Layout

