/** @jsxImportSource react */
import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { Form, Input, Button, Card, Typography, Alert, ConfigProvider } from 'antd'
import { UserOutlined, MailOutlined, LockOutlined } from '@ant-design/icons'
import api from '../api/axios'

const { Title, Text } = Typography

export default function RegisterPage() {
  const navigate = useNavigate()
  const [serverError, setServerError] = useState<string | null>(null)
  const [loading, setLoading] = useState(false)

  const onFinish = async (values: any) => {
    setServerError(null)
    setLoading(true)
    try {
      const response = await api.post('/accounts/register', values)
      localStorage.setItem('jwt_token', response.data.token)
      navigate('/dashboard')
    } catch (error: any) {
      const backendMessage = error.response.data[0]
      if (error.response?.status === 400) {
        setServerError(
          typeof backendMessage === 'string' ? backendMessage : 'Invalid register information.',
        )
      } else {
        setServerError(
          typeof backendMessage === 'string' ? backendMessage : 'Email or username already exists.',
        )
      }
    } finally {
      setLoading(false)
    }
  }
  return (
    <div
      style={{
        minHeight: '100vh',
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        background: '#f0f2f5',
      }}
    >
      <Card
        style={{ width: 400, boxShadow: '0 4px 12px rgba(0,0,0,0.05)' }}
        styles={{ body: { paddingBottom: 16 } }}
      >
        <div style={{ textAlign: 'center', marginBottom: 24 }}>
          <Title level={3} style={{ marginTop: 0 }}>
            Welcome
          </Title>
          <Text type="secondary">Please enter your details to register.</Text>
        </div>

        {serverError && (
          <Alert message={serverError} type="error" showIcon style={{ marginBottom: 24 }} />
        )}
        <ConfigProvider theme={{ components: { Form: { itemMarginBottom: 0 } } }}>
          <Form
            name="register"
            onFinish={onFinish}
            layout="vertical"
            requiredMark={true}
            size="large"
          >
            <Form.Item
              name="firstName"
              label="First name"
              rules={[{ required: true, message: 'First name is required' }]}
            >
              <Input placeholder="Enter your first name" />
            </Form.Item>
            <Form.Item
              name="lastName"
              label="Last name"
              rules={[{ required: true, message: 'Last name is required' }]}
            >
              <Input placeholder="Enter your last name" />
            </Form.Item>
            <Form.Item
              name="username"
              label="Username"
              rules={[{ required: true, message: 'Username is required' }]}
            >
              <Input prefix={<UserOutlined />} placeholder="Enter your username" />
            </Form.Item>
            <Form.Item
              name="email"
              label="Email"
              rules={[
                { required: true, message: 'Email is required' },
                { type: 'email', message: 'Invalid email format' },
              ]}
            >
              <Input prefix={<MailOutlined />} placeholder="Enter your email" />
            </Form.Item>
            <Form.Item
              name="password"
              label="Password"
              rules={[
                { required: true, message: 'Password is required' },
                { min: 8, message: 'Password must be at least 8 characters' },
                {
                  pattern: /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/,
                  message:
                    'Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.',
                },
              ]}
              hasFeedback
            >
              <Input.Password prefix={<LockOutlined />} placeholder="••••••••" />
            </Form.Item>
            <Form.Item
              name="confirmPassword"
              label="Confirm password"
              rules={[
                { required: true, message: 'Confirm your password!' },
                ({ getFieldValue }) => ({
                  validator(_, value) {
                    if (!value || getFieldValue('password') === value) return Promise.resolve()
                    return Promise.reject(new Error('The two passwords do not match!'))
                  },
                }),
              ]}
              hasFeedback
            >
              <Input.Password prefix={<LockOutlined />} placeholder="••••••••" />
            </Form.Item>
            <Form.Item style={{ marginTop: 32, marginBottom: 0 }}>
              <Button type="primary" htmlType="submit" block loading={loading}>
                Register
              </Button>
            </Form.Item>
            <Form.Item
              style={{
                marginBottom: 0,
                display: 'flex',
                justifyContent: 'center',
                alignContent: 'bottom',
              }}
            >
              Already have an account? <a href="/login">Login</a>
            </Form.Item>
          </Form>
        </ConfigProvider>
      </Card>
    </div>
  )
}
