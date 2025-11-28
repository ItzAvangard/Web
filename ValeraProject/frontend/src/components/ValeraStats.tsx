import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { valeraApi, ValeraDto } from '../api/valeraApi';
import './ValeraStats.css';

const ValeraStats: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [valera, setValera] = useState<ValeraDto | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [actionLoading, setActionLoading] = useState<string | null>(null);

  useEffect(() => {
    if (id) {
      loadValera();
    }
  }, [id]);

  const loadValera = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await valeraApi.getById(parseInt(id!));
      setValera(data);
    } catch (err) {
      setError('Ошибка при загрузке данных Валеры');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const executeAction = async (action: string) => {
    if (!id) return;

    try {
      setActionLoading(action);
      setError(null);
      const updatedValera = await valeraApi.executeAction(parseInt(id), action);
      setValera(updatedValera);
    } catch (err: any) {
      setError(
        err.response?.data?.message ||
          `Ошибка при выполнении действия: ${action}`
      );
      console.error(err);
    } finally {
      setActionLoading(null);
    }
  };

  const canGoToWork = (): boolean => {
    if (!valera) return false;
    return valera.mana < 50 && valera.fatigue < 10;
  };

  const canDrinkWine = (): boolean => {
    if (!valera) return false;
    return valera.money >= 20;
  };

  const canGoToBar = (): boolean => {
    if (!valera) return false;
    return valera.money >= 100;
  };

  const canDrinkWithMarginals = (): boolean => {
    if (!valera) return false;
    return valera.money >= 150;
  };

  const ProgressBar: React.FC<{
    label: string;
    value: number;
    max: number;
    min?: number;
    type: string;
  }> = ({ label, value, max, min = 0, type }) => {
    // Для жизнерадостности: диапазон от -10 до 10
    const range = max - min;
    const normalizedValue = value - min;
    const percentage = Math.min(Math.max((normalizedValue / range) * 100, 0), 100);
    return (
      <div>
        <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: '5px' }}>
          <span>{label}</span>
          <span>{value} {min < 0 ? `(${min} до ${max})` : `(0 до ${max})`}</span>
        </div>
        <div className="progress-bar">
          <div
            className={`progress-fill ${type}`}
            style={{ width: `${percentage}%` }}
          >
            {Math.round(percentage)}%
          </div>
        </div>
      </div>
    );
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

  if (!valera) {
    return (
      <div className="container">
        <div className="card">
          <p>Валера не найдена</p>
          <button className="btn btn-secondary" onClick={() => navigate('/')}>
            Вернуться к списку
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="container">
      <div className="card">
        <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '20px' }}>
          <h1>{valera.name} (ID: {valera.id})</h1>
          <button className="btn btn-secondary" onClick={() => navigate('/')}>
            ← Назад к списку
          </button>
        </div>

        {error && (
          <div style={{ color: 'red', marginBottom: '20px', padding: '10px', backgroundColor: '#ffe6e6', borderRadius: '4px' }}>
            {error}
          </div>
        )}

        <div className="stats-grid">
          <div>
            <ProgressBar
              label="Здоровье"
              value={valera.health}
              max={100}
              type="health"
            />
            <ProgressBar
              label="Алкоголь"
              value={valera.mana}
              max={100}
              type="mana"
            />
            <ProgressBar
              label="Жизнерадостность"
              value={valera.cheerfulness}
              max={10}
              min={-10}
              type="cheerfulness"
            />
          </div>
          <div>
            <ProgressBar
              label="Усталость"
              value={valera.fatigue}
              max={100}
              type="fatigue"
            />
            <ProgressBar
              label="Деньги"
              value={valera.money}
              max={1000}
              type="money"
            />
            <div style={{ marginTop: '20px', padding: '15px', backgroundColor: '#f8f9fa', borderRadius: '4px' }}>
              <strong>Деньги: {valera.money} ₽</strong>
            </div>
          </div>
        </div>

        <h2 style={{ marginTop: '30px', marginBottom: '15px' }}>Действия</h2>
        <div className="actions-grid">
          <button
            className="btn btn-primary"
            onClick={() => executeAction('work')}
            disabled={!canGoToWork() || actionLoading !== null}
          >
            {actionLoading === 'work' ? 'Выполняется...' : 'Пойти на работу'}
          </button>
          <button
            className="btn btn-primary"
            onClick={() => executeAction('nature')}
            disabled={actionLoading !== null}
          >
            {actionLoading === 'nature' ? 'Выполняется...' : 'Созерцать природу'}
          </button>
          <button
            className="btn btn-primary"
            onClick={() => executeAction('tv')}
            disabled={!canDrinkWine() || actionLoading !== null}
          >
            {actionLoading === 'tv' ? 'Выполняется...' : 'Пить вино и смотреть сериал'}
          </button>
          <button
            className="btn btn-primary"
            onClick={() => executeAction('bar')}
            disabled={!canGoToBar() || actionLoading !== null}
          >
            {actionLoading === 'bar' ? 'Выполняется...' : 'Сходить в бар'}
          </button>
          <button
            className="btn btn-primary"
            onClick={() => executeAction('marginals')}
            disabled={!canDrinkWithMarginals() || actionLoading !== null}
          >
            {actionLoading === 'marginals' ? 'Выполняется...' : 'Выпить с маргинальными личностями'}
          </button>
          <button
            className="btn btn-primary"
            onClick={() => executeAction('sing')}
            disabled={actionLoading !== null}
          >
            {actionLoading === 'sing' ? 'Выполняется...' : 'Петь в метро'}
          </button>
          <button
            className="btn btn-primary"
            onClick={() => executeAction('sleep')}
            disabled={actionLoading !== null}
          >
            {actionLoading === 'sleep' ? 'Выполняется...' : 'Спать'}
          </button>
        </div>

        {!canGoToWork() && (
          <div style={{ marginTop: '15px', padding: '10px', backgroundColor: '#fff3cd', borderRadius: '4px', color: '#856404' }}>
            ⚠️ Нельзя идти на работу: усталость ≥ 10 или алкоголь ≥ 50
          </div>
        )}
      </div>
    </div>
  );
};

export default ValeraStats;

