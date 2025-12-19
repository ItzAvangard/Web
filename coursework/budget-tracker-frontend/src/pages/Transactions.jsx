import { useState, useEffect } from 'react'
import api from '../services/api'
import './Transactions.css'

function Transactions() {
  const [transactions, setTransactions] = useState([])
  const [categories, setCategories] = useState([])
  const [loading, setLoading] = useState(true)
  const [showModal, setShowModal] = useState(false)
  const [selectedCategory, setSelectedCategory] = useState('')
  const [editingTransaction, setEditingTransaction] = useState(null)
  const [formData, setFormData] = useState({
    description: '',
    amount: '',
    date: new Date().toISOString().split('T')[0],
    type: 2,
    categoryId: ''
  })

  useEffect(() => {
    loadCategories()
    loadTransactions()
  }, [])

  useEffect(() => {
    loadTransactions()
  }, [selectedCategory])

  const loadCategories = async () => {
    try {
      const response = await api.get('/categories')
      setCategories(response.data)
    } catch (error) {
      console.error('Ошибка загрузки категорий:', error)
    }
  }

  const loadTransactions = async () => {
    try {
      setLoading(true)
      const url = selectedCategory 
        ? `/transactions?categoryId=${selectedCategory}`
        : '/transactions'
      const response = await api.get(url)
      setTransactions(response.data)
    } catch (error) {
      console.error('Ошибка загрузки транзакций:', error)
    } finally {
      setLoading(false)
    }
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    try {
      const payload = {
        ...formData,
        amount: parseFloat(formData.amount),
        categoryId: formData.categoryId || null
      }

      if (editingTransaction) {
        await api.put(`/transactions/${editingTransaction.id}`, payload)
      } else {
        await api.post('/transactions', payload)
      }

      setShowModal(false)
      setEditingTransaction(null)
      setFormData({
        description: '',
        amount: '',
        date: new Date().toISOString().split('T')[0],
        type: 2,
        categoryId: ''
      })
      loadTransactions()
      loadCategories()
    } catch (error) {
      console.error('Ошибка сохранения транзакции:', error)
      alert('Ошибка сохранения транзакции')
    }
  }

  const handleEdit = (transaction) => {
    setEditingTransaction(transaction)
    setFormData({
      description: transaction.description,
      amount: transaction.amount,
      date: new Date(transaction.date).toISOString().split('T')[0],
      type: transaction.type,
      categoryId: transaction.categoryId || ''
    })
    setShowModal(true)
  }

  const handleDelete = async (id) => {
    if (!confirm('Удалить транзакцию?')) return

    try {
      await api.delete(`/transactions/${id}`)
      loadTransactions()
    } catch (error) {
      console.error('Ошибка удаления транзакции:', error)
      alert('Ошибка удаления транзакции')
    }
  }

  if (loading) {
    return <div className="loading">Загрузка...</div>
  }

  return (
    <div className="transactions-page">
      <div className="page-header">
        <h1>Транзакции</h1>
        <div className="header-actions">
          <div className="filter-group">
            <label htmlFor="category-filter">Фильтр по категории:</label>
            <select
              id="category-filter"
              value={selectedCategory}
              onChange={(e) => setSelectedCategory(e.target.value)}
              className="filter-select"
            >
              <option value="">Все категории</option>
              {categories.map((cat) => (
                <option key={cat.id} value={cat.id}>
                  {cat.name}
                </option>
              ))}
            </select>
            {selectedCategory && (
              <button
                onClick={() => setSelectedCategory('')}
                className="btn-clear-filter"
                title="Сбросить фильтр"
              >
                ✕
              </button>
            )}
          </div>
          <button onClick={() => {
            setEditingTransaction(null)
            setFormData({
              description: '',
              amount: '',
              date: new Date().toISOString().split('T')[0],
              type: 2,
              categoryId: ''
            })
            setShowModal(true)
          }} className="btn-primary">
            + Добавить транзакцию
          </button>
        </div>
      </div>

      {transactions.length === 0 ? (
        <div className="empty-state">
          <p>Нет транзакций</p>
        </div>
      ) : (
        <div className="transactions-table">
          <table>
            <thead>
              <tr>
                <th>Дата</th>
                <th>Описание</th>
                <th>Категория</th>
                <th>Тип</th>
                <th>Сумма</th>
                <th>Действия</th>
              </tr>
            </thead>
            <tbody>
              {transactions.map((transaction) => (
                <tr key={transaction.id}>
                  <td>{new Date(transaction.date).toLocaleDateString('ru-RU')}</td>
                  <td>{transaction.description}</td>
                  <td>{transaction.categoryName || '-'}</td>
                  <td>
                    <span className={`type-badge ${transaction.type === 1 ? 'income' : 'expense'}`}>
                      {transaction.type === 1 ? 'Доход' : 'Расход'}
                    </span>
                  </td>
                  <td className={`amount ${transaction.type === 1 ? 'income' : 'expense'}`}>
                    {transaction.type === 1 ? '+' : '-'}
                    {transaction.amount.toLocaleString('ru-RU')} ₽
                  </td>
                  <td>
                    <button onClick={() => handleEdit(transaction)} className="btn-edit">
                      Изменить
                    </button>
                    <button onClick={() => handleDelete(transaction.id)} className="btn-delete">
                      Удалить
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      {showModal && (
        <div className="modal-overlay" onClick={() => setShowModal(false)}>
          <div className="modal-content" onClick={(e) => e.stopPropagation()}>
            <h2>{editingTransaction ? 'Изменить транзакцию' : 'Добавить транзакцию'}</h2>
            <form onSubmit={handleSubmit}>
              <div className="form-group">
                <label>Описание</label>
                <input
                  type="text"
                  value={formData.description}
                  onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                  required
                />
              </div>
              <div className="form-group">
                <label>Сумма</label>
                <input
                  type="number"
                  step="0.01"
                  value={formData.amount}
                  onChange={(e) => setFormData({ ...formData, amount: e.target.value })}
                  required
                />
              </div>
              <div className="form-group">
                <label>Дата</label>
                <input
                  type="date"
                  value={formData.date}
                  onChange={(e) => setFormData({ ...formData, date: e.target.value })}
                  required
                />
              </div>
              <div className="form-group">
                <label>Тип</label>
                <select
                  value={formData.type}
                  onChange={(e) => setFormData({ ...formData, type: parseInt(e.target.value) })}
                  required
                >
                  <option value={1}>Доход</option>
                  <option value={2}>Расход</option>
                </select>
              </div>
              <div className="form-group">
                <label>Категория</label>
                <select
                  value={formData.categoryId}
                  onChange={(e) => setFormData({ ...formData, categoryId: e.target.value })}
                >
                  <option value="">Без категории</option>
                  {categories.map((cat) => (
                    <option key={cat.id} value={cat.id}>
                      {cat.name}
                    </option>
                  ))}
                </select>
              </div>
              <div className="modal-actions">
                <button type="button" onClick={() => setShowModal(false)} className="btn-secondary">
                  Отмена
                </button>
                <button type="submit" className="btn-primary">
                  {editingTransaction ? 'Сохранить' : 'Добавить'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  )
}

export default Transactions

