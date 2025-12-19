import { useState, useEffect } from 'react'
import api from '../services/api'
import './Dashboard.css'

function Dashboard() {
  const [summary, setSummary] = useState({ totalIncome: 0, totalExpenses: 0, balance: 0 })
  const [recentTransactions, setRecentTransactions] = useState([])
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    loadData()
  }, [])

  const loadData = async () => {
    try {
      const [summaryRes, transactionsRes] = await Promise.all([
        api.get('/transactions/summary'),
        api.get('/transactions?startDate=' + new Date(new Date().getFullYear(), new Date().getMonth(), 1).toISOString())
      ])
      setSummary(summaryRes.data)
      setRecentTransactions(transactionsRes.data.slice(0, 5))
    } catch (error) {
      console.error('Ошибка загрузки данных:', error)
    } finally {
      setLoading(false)
    }
  }

  if (loading) {
    return <div className="loading">Загрузка...</div>
  }

  return (
    <div className="dashboard">
      <h1>Панель управления</h1>
      
      <div className="summary-cards">
        <div className="summary-card income">
          <h3>Доходы</h3>
          <p className="amount">+{summary.totalIncome.toLocaleString('ru-RU')} ₽</p>
        </div>
        <div className="summary-card expense">
          <h3>Расходы</h3>
          <p className="amount">-{summary.totalExpenses.toLocaleString('ru-RU')} ₽</p>
        </div>
        <div className={`summary-card balance ${summary.balance >= 0 ? 'positive' : 'negative'}`}>
          <h3>Баланс</h3>
          <p className="amount">{summary.balance.toLocaleString('ru-RU')} ₽</p>
        </div>
      </div>

      <div className="recent-transactions">
        <h2>Последние транзакции</h2>
        {recentTransactions.length === 0 ? (
          <p className="empty-message">Нет транзакций</p>
        ) : (
          <div className="transactions-list">
            {recentTransactions.map((transaction) => (
              <div key={transaction.id} className="transaction-item">
                <div className="transaction-info">
                  <span className="transaction-description">{transaction.description}</span>
                  <span className="transaction-date">
                    {new Date(transaction.date).toLocaleDateString('ru-RU')}
                  </span>
                </div>
                <span className={`transaction-amount ${transaction.type === 1 ? 'income' : 'expense'}`}>
                  {transaction.type === 1 ? '+' : '-'}
                  {transaction.amount.toLocaleString('ru-RU')} ₽
                </span>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  )
}

export default Dashboard

