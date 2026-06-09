/** @jsxImportSource react */
import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { useNavigate } from 'react-router-dom';
import api from '../api/axios';

const loginSchema = z.object({
  email: z.string().min(1, "Email is required").email("Invalid email format"),
  password: z.string().min(6, "Password must be at least 6 characters"),
});

type LoginFormInputs = z.infer<typeof loginSchema>;

export default function Login() {
  const navigate = useNavigate();
  const [serverError, setServerError] = useState<string | null>(null);

  const { register, handleSubmit, formState: { errors, isSubmitting } } = useForm<LoginFormInputs>({
    resolver: zodResolver(loginSchema),
  });

  const onSubmit = async (data: LoginFormInputs) => {
    setServerError(null);
    try {
      const response = await api.post('/accounts/login', data);
      localStorage.setItem('jwt_token', response.data.token);
      navigate('/dashboard');
    } catch (error: any) {
      if (error.response?.status === 429 || error.response?.status === 403) {
         setServerError("Account locked for 5 minutes due to too many failed attempts.");
      } else {
         setServerError(error.response?.data?.message || "Invalid credentials.");
      }
    }
  };

  return (
    <div className="min-h-screen flex-center">
      <div className="login-card">
        <h2>Welcome back</h2>
        <p>Please enter your details to sign in.</p>

        <form onSubmit={handleSubmit(onSubmit)}>
          <div className="form-group">
            <label>Email</label>
            <input type="email" placeholder="Enter your email" {...register('email')} />
            {errors.email && <span className="error">{errors.email.message}</span>}
          </div>

          <div className="form-group" style={{ marginBottom: '2rem' }}>
            <label>Password</label>
            <input type="password" placeholder="••••••••" {...register('password')} />
            {errors.password && <span className="error">{errors.password.message}</span>}
          </div>

          {serverError && <div className="error" style={{ marginBottom: '1rem', textAlign: 'center' }}>{serverError}</div>}

          <button type="submit" disabled={isSubmitting}>
            {isSubmitting ? 'Signing in...' : 'Sign in'}
          </button>
        </form>
      </div>
    </div>
  );
}