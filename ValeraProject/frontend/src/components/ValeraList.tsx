import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { valeraApi, ValeraDto, CreateValeraDto } from '../api/valeraApi';
import './ValeraList.css';

const ValeraList: React.FC = () => {
  const [valeras, setValeras] = useState<ValeraDto[]>([]);
  const [filteredValeras, setFilteredValeras] = useState<ValeraDto[]>([]);
  const [searchTerm, setSearchTerm] = useState('');
  const [showCreateForm, setShowCreateForm] = useState(false);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const [createForm, setCreateForm] = useState<CreateValeraDto>({
    name: 'Valera',
    health: 100,
    mana: 0,
    cheerfulness: 0,
    fatigue: 0,
    money: 100,
  });

  useEffect(() => {
    loadValeras();
  }, []);

  useEffect(() => {
    if (searchTerm.trim() === '') {
      setFilteredValeras(valeras);
    } else {
      setFilteredValeras(
        valeras.filter((valera) =>
          valera.name.toLowerCase().includes(searchTerm.toLowerCase())
        )
      );
    }
  }, [searchTerm, valeras]);

  const loadValeras = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await valeraApi.getAll();
      setValeras(data);
    } catch (err) {
      setError('Ошибка при загрузке списка Валер');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleCreate = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      setError(null);
      await valeraApi.create(createForm);
      setShowCreateForm(false);
      setCreateForm({
        name: 'Valera',
        health: 100,
        mana: 0,
        cheerfulness: 0,
        fatigue: 0,
        money: 100,
      });
      await loadValeras();
    } catch (err: any) {
      console.error('Full error:', err);
      console.error('Error response:', err.response);
      const errorMessage = err.response?.data?.message 
        || err.response?.data 
        || err.message 
        || 'Ошибка при создании Валеры';
      setError(errorMessage);
    }
  };

  const handleDelete = async (id: number, e: React.MouseEvent) => {
    e.stopPropagation();
    if (window.confirm('Вы уверены, что хотите удалить эту Валеру?')) {
      try {
        await valeraApi.delete(id);
        await loadValeras();
      } catch (err) {
        setError('Ошибка при удалении Валеры');
        console.error(err);
      }
    }
  };

  if (loading) {
    return (
      <div className="container">
        <div className="card">
          <p>Загрузка...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="container">
      <div className="card">
        <h1>Список Валер</h1>

        <div className="search-bar">
          <input
            type="text"
            className="input"
            placeholder="Поиск по имени..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
          />
        </div>

        <button
          className="btn btn-primary"
          onClick={() => setShowCreateForm(!showCreateForm)}
        >
          {showCreateForm ? 'Отмена' : 'Создать Валеру'}
        </button>

        {showCreateForm && (
          <form onSubmit={handleCreate} className="create-form">
            <h3>Создать новую Валеру</h3>
            <input
              type="text"
              className="input"
              placeholder="Имя"
              value={createForm.name}
              onChange={(e) =>
                setCreateForm({ ...createForm, name: e.target.value })
              }
              required
            />
            <input
              type="number"
              className="input"
              placeholder="Здоровье (0-100)"
              value={createForm.health}
              onChange={(e) =>
                setCreateForm({ ...createForm, health: parseInt(e.target.value) || 0 })
              }
              min="0"
              max="100"
            />
            <input
              type="number"
              className="input"
              placeholder="Алкоголь (0-100)"
              value={createForm.mana}
              onChange={(e) =>
                setCreateForm({ ...createForm, mana: parseInt(e.target.value) || 0 })
              }
              min="0"
              max="100"
            />
            <input
              type="number"
              className="input"
              placeholder="Жизнерадостность (-10 до 10)"
              value={createForm.cheerfulness}
              onChange={(e) =>
                setCreateForm({ ...createForm, cheerfulness: parseInt(e.target.value) || 0 })
              }
              min="-10"
              max="10"
            />
            <input
              type="number"
              className="input"
              placeholder="Усталость (0-100)"
              value={createForm.fatigue}
              onChange={(e) =>
                setCreateForm({ ...createForm, fatigue: parseInt(e.target.value) || 0 })
              }
              min="0"
              max="100"
            />
            <input
              type="number"
              className="input"
              placeholder="Деньги"
              value={createForm.money}
              onChange={(e) =>
                setCreateForm({ ...createForm, money: parseInt(e.target.value) || 0 })
              }
              min="0"
            />
            <button type="submit" className="btn btn-success">
              Создать
            </button>
          </form>
        )}

        {error && (
          <div style={{ color: 'red', marginTop: '10px' }}>{error}</div>
        )}

        <div className="valera-list">
          {filteredValeras.length === 0 ? (
            <p>Валеры не найдены</p>
          ) : (
            filteredValeras.map((valera) => (
              <div
                key={valera.id}
                className="valera-item"
                onClick={() => navigate(`/valera/${valera.id}`)}
              >
                <h3>{valera.name}</h3>
                <p>ID: {valera.id}</p>
                <p>Здоровье: {valera.health}</p>
                <p>Алкоголь: {valera.mana}</p>
                <p>Деньги: {valera.money}</p>
                <button
                  className="btn btn-danger"
                  style={{ marginTop: '10px' }}
                  onClick={(e) => handleDelete(valera.id, e)}
                >
                  Удалить
                </button>
              </div>
            ))
          )}
        </div>
      </div>
    </div>
  );
};

export default ValeraList;

