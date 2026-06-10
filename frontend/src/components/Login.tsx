/** @jsxImportSource react */
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Form, Input, Button, Card, Typography, Alert } from 'antd';
import { UserOutlined, LockOutlined } from '@ant-design/icons';
import api from '../api/axios';

const { Title, Text } = Typography;

export default function Login() {
  const navigate = useNavigate();
  const [serverError, setServerError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  const onFinish = async (values: any) => {
    setServerError(null);
    setLoading(true);
    try {
      const response = await api.post('/accounts/login', values);
      localStorage.setItem('jwt_token', response.data.token);
      navigate('/dashboard');
    } catch (error: any) {
      if (error.response?.status === 429) {
         setServerError("Too many login attempts. Please wait a moment and try again.");
      } else if (error.response?.status === 401) {
          const backendMessage = error.response.data; 
          setServerError(typeof backendMessage === 'string' ? backendMessage : "Invalid credentials.");
      } else {
          setServerError("An unexpected error occurred. Please try again later.");
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={{ minHeight: '100vh', display: 'flex', justifyContent: 'center', alignItems: 'center', background: '#f0f2f5' }}>
      <Card style={{ width: 400, boxShadow: '0 4px 12px rgba(0,0,0,0.05)' }}>
        <div style={{ textAlign: 'center', marginBottom: 24 }}>
          <Title level={3} style={{ marginTop: 0 }}>Welcome back</Title>
          <Text type="secondary">Please enter your details to sign in.</Text>
        </div>

        {serverError && (
          <Alert message={serverError} type="error" showIcon style={{ marginBottom: 24 }} />
        )}

        <Form 
          name="login" 
          onFinish={onFinish} 
          layout="vertical" 
          requiredMark={false}
          size="large"
        >
          <Form.Item
            name="email"
            label="Email"
            rules={[
              { required: true, message: 'Email is required' },
              { type: 'email', message: 'Invalid email format' }
            ]}
          >
            <Input prefix={<UserOutlined />} placeholder="Enter your email" />
          </Form.Item>

          <Form.Item
            name="password"
            label="Password"
            rules={[
              { required: true, message: 'Password is required' },
              { min: 6, message: 'Password must be at least 6 characters' }
            ]}
          >
            <Input.Password prefix={<LockOutlined />} placeholder="••••••••" />
          </Form.Item>

          <Form.Item style={{ marginTop: 32, marginBottom: 0 }}>
            <Button type="primary" htmlType="submit" block loading={loading}>
              Sign in
            </Button>
          </Form.Item>
        </Form>
      </Card>
    </div>
  );
}