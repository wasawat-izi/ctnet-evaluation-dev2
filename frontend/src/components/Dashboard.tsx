/** @jsxImportSource react */
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Layout, Card, Table, Typography, Button, Descriptions, Spin, Space } from 'antd';
import { LogoutOutlined } from '@ant-design/icons';
import api from '../api/axios';

const { Header, Content } = Layout;
const { Title, Text } = Typography;

interface User {
  id: string;
  email: string;
  userName: string;
}

export default function Dashboard() {
  const navigate = useNavigate();
  const [currentUser, setCurrentUser] = useState<User | null>(null);
  const [allUsers, setAllUsers] = useState<User[]>([]);
  const [loading, setLoading] = useState(true);

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
      } finally {
        setLoading(false);
      }
    };

    fetchDashboardData();
  }, [navigate]);

  const handleLogout = () => {
    localStorage.removeItem('jwt_token');
    navigate('/login');
  };

  const tableColumns = [
    { 
      title: 'ID', 
      dataIndex: 'id', 
      key: 'id',
      render: (text: string) => <Text type="secondary" style={{ fontSize: '0.85rem' }}>{text}</Text>
    },
    { 
      title: 'Username', 
      dataIndex: 'userName', 
      key: 'userName',
      render: (text: string) => <Text strong>{text}</Text>
    },
    { 
      title: 'Email', 
      dataIndex: 'email', 
      key: 'email' 
    },
  ];

  if (loading || !currentUser) {
    return (
      <div style={{ height: '100vh', display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
        <Spin size="large" tip="Loading dashboard..." />
      </div>
    );
  }

  return (
    <Layout style={{ minHeight: '100vh' }}>
      <Header style={{ 
        display: 'flex', 
        justifyContent: 'space-between', 
        alignItems: 'center', 
        background: '#fff', 
        padding: '0 24px',
        boxShadow: '0 1px 4px rgba(0,21,41,0.08)',
        zIndex: 1
      }}>
        <Title level={4} style={{ margin: 0 }}>Overview</Title>
        <Space size="large">
          <Text>Welcome back, <strong>{currentUser.userName}</strong></Text>
          <Button type="text" icon={<LogoutOutlined />} onClick={handleLogout} danger>
            Log out
          </Button>
        </Space>
      </Header>

      <Content style={{ padding: '24px', maxWidth: 1200, margin: '0 auto', width: '100%' }}>
        <Space direction="vertical" size="large" style={{ display: 'flex' }}>
          
          <Card title="Profile Details" bordered={false} style={{ borderRadius: 8 }}>
            <Descriptions column={1}>
              <Descriptions.Item label="Email">{currentUser.email}</Descriptions.Item>
              <Descriptions.Item label="Account ID">
                <Text copyable>{currentUser.id}</Text>
              </Descriptions.Item>
            </Descriptions>
          </Card>

          <Card title="System Users" bordered={false} style={{ borderRadius: 8 }}>
            <Table 
              dataSource={allUsers} 
              columns={tableColumns} 
              rowKey="id" 
              pagination={{ pageSize: 5 }} 
              size="middle"
            />
          </Card>

        </Space>
      </Content>
    </Layout>
  );
}