/** @jsxImportSource react */
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../api/axios';

interface User {
  id: string;
  email: string;
  userName: string;
}

export default function Dashboard() {
  const navigate = useNavigate();
  const [currentUser, setCurrentUser] = useState<User | null>(null);
  const [allUsers, setAllUsers] = useState<User[]>([]);

  useEffect(() => {
    const fetchDashboardData = async () => {
      try {
        const meRes = await api.get('/accounts/me');
        setCurrentUser(meRes.data);

        const usersRes = await api.get('/accounts');
        setAllUsers(usersRes.data);
      } catch (error) {
        localStorage.removeItem('jwt_token');
        navigate('/login');
      }
    };

    fetchDashboardData();
  }, [navigate]);

  const handleLogout = () => {
    localStorage.removeItem('jwt_token');
    navigate('/login');
  };

  if (!currentUser) return <div className="min-h-screen flex-center"><p>Loading...</p></div>;

  return (
    <div className="container">
      <header className="dashboard-header">
        <div>
          <h1>Overview</h1>
          <p>Welcome back, {currentUser.userName}</p>
        </div>
        <button className="btn-outline" onClick={handleLogout}>Log out</button>
      </header>

      <section className="card">
        <h2>Profile Details</h2>
        <p style={{ marginTop: '0.5rem' }}><strong>Email:</strong> {currentUser.email}</p>
        <p><strong>Account ID:</strong> {currentUser.id}</p>
      </section>

      <section className="card">
        <h2 style={{ marginBottom: '1.5rem' }}>System Users</h2>
        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>Username</th>
              <th>Email</th>
            </tr>
          </thead>
          <tbody>
            {allUsers.map(user => (
              <tr key={user.id}>
                <td><span style={{ color: 'var(--text-secondary)', fontSize: '0.85rem' }}>{user.id}</span></td>
                <td style={{ fontWeight: 500 }}>{user.userName}</td>
                <td>{user.email}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </section>
    </div>
  );
}