import { useState, useEffect } from 'react'
import api from '../services/api'
import './Categories.css'

function Categories() {
  const [categories, setCategories] = useState([])
  const [loading, setLoading] = useState(true)
  const [showModal, setShowModal] = useState(false)
  const [editingCategory, setEditingCategory] = useState(null)
  const [formData, setFormData] = useState({
    name: '',
    color: '#667eea'
  })

  useEffect(() => {
    loadCategories()
  }, [])

  const loadCategories = async () => {
    try {
      const response = await api.get('/categories')
      setCategories(response.data)
    } catch (error) {
      console.error('Ошибка загрузки категорий:', error)
    } finally {
      setLoading(false)
    }
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    try {
      if (editingCategory) {
        await api.put(`/categories/${editingCategory.id}`, formData)
      } else {
        await api.post('/categories', formData)
      }

      setShowModal(false)
      setEditingCategory(null)
      setFormData({ name: '', color: '#667eea' })
      loadCategories()
    } catch (error) {
      console.error('Ошибка сохранения категории:', error)
      alert('Ошибка сохранения категории')
    }
  }

  const handleEdit = (category) => {
    setEditingCategory(category)
    setFormData({
      name: category.name,
      color: category.color
    })
    setShowModal(true)
  }

  const handleDelete = async (id) => {
    if (!confirm('Удалить категорию?')) return

    try {
      await api.delete(`/categories/${id}`)
      loadCategories()
    } catch (error) {
      console.error('Ошибка удаления категории:', error)
      alert('Ошибка удаления категории')
    }
  }

  if (loading) {
    return <div className="loading">Загрузка...</div>
  }

  return (
    <div className="categories-page">
      <div className="page-header">
        <h1>Категории</h1>
        <button
          onClick={() => {
            setEditingCategory(null)
            setFormData({ name: '', color: '#667eea' })
            setShowModal(true)
          }}
          className="btn-primary"
        >
          + Добавить категорию
        </button>
      </div>

      {categories.length === 0 ? (
        <div className="empty-state">
          <p>Нет категорий</p>
        </div>
      ) : (
        <div className="categories-grid">
          {categories.map((category) => (
            <div key={category.id} className="category-card">
              <div
                className="category-color"
                style={{ backgroundColor: category.color }}
              ></div>
              <div className="category-info">
                <h3>{category.name}</h3>
              </div>
              <div className="category-actions">
                <button onClick={() => handleEdit(category)} className="btn-edit">
                  Изменить
                </button>
                <button onClick={() => handleDelete(category.id)} className="btn-delete">
                  Удалить
                </button>
              </div>
            </div>
          ))}
        </div>
      )}

      {showModal && (
        <div className="modal-overlay" onClick={() => setShowModal(false)}>
          <div className="modal-content" onClick={(e) => e.stopPropagation()}>
            <h2>{editingCategory ? 'Изменить категорию' : 'Добавить категорию'}</h2>
            <form onSubmit={handleSubmit}>
              <div className="form-group">
                <label>Название</label>
                <input
                  type="text"
                  value={formData.name}
                  onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                  required
                />
              </div>
              <div className="form-group">
                <label>Цвет</label>
                <div className="color-input-group">
                  <input
                    type="color"
                    value={formData.color}
                    onChange={(e) => setFormData({ ...formData, color: e.target.value })}
                    className="color-picker"
                  />
                  <input
                    type="text"
                    value={formData.color}
                    onChange={(e) => setFormData({ ...formData, color: e.target.value })}
                    className="color-text"
                    pattern="^#[0-9A-Fa-f]{6}$"
                  />
                </div>
              </div>
              <div className="modal-actions">
                <button type="button" onClick={() => setShowModal(false)} className="btn-secondary">
                  Отмена
                </button>
                <button type="submit" className="btn-primary">
                  {editingCategory ? 'Сохранить' : 'Добавить'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  )
}

export default Categories

