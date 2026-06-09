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
        // Fetch current logged-in user info
        const meRes = await api.get('/accounts/me');
        setCurrentUser(meRes.data);

        // Fetch all registered users
        const usersRes = await api.get('/accounts');
        setAllUsers(usersRes.data);
      } catch (error) {
        // If unauthorized, token might be expired
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

  if (!currentUser) return <div>Loading...</div>;

  return (
    <div>
      <header>
        <h1>Welcome, {currentUser.userName}!</h1>
        <button onClick={handleLogout}>Logout</button>
      </header>

      <section>
        <h2>Your Profile</h2>
        <p>Email: {currentUser.email}</p>
        <p>ID: {currentUser.id}</p>
      </section>

      <section>
        <h2>All Registered Users</h2>
        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>Name</th>
              <th>Email</th>
            </tr>
          </thead>
          <tbody>
            {allUsers.map(user => (
              <tr key={user.id}>
                <td>{user.id}</td>
                <td>{user.userName}</td>
                <td>{user.email}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </section>
    </div>
  );
}