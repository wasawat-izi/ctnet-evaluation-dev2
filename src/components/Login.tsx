/** @jsxImportSource react */
import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { useNavigate } from 'react-router-dom';
import api from '../api/axios';

// 1. Define real-world validation rules
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
      
      // Store JWT
      localStorage.setItem('jwt_token', response.data.token);
      
      // Redirect to welcome/dashboard
      navigate('/dashboard');
    } catch (error: any) {
      // 2. Handle the specific 5-failed-attempts lockout scenario
      if (error.response?.status === 429 || error.response?.status === 403) {
         setServerError("Account locked for 5 minutes due to too many failed attempts.");
      } else {
         setServerError(error.response?.data?.message || "Invalid credentials.");
      }
    }
  };

  return (
    <div className="login-container">
      <h2>Welcome Back</h2>
      <form onSubmit={handleSubmit(onSubmit)}>
        <div>
          <label>Email</label>
          <input type="email" {...register('email')} />
          {errors.email && <span className="error">{errors.email.message}</span>}
        </div>

        <div>
          <label>Password</label>
          <input type="password" {...register('password')} />
          {errors.password && <span className="error">{errors.password.message}</span>}
        </div>

        {serverError && <div className="server-error" style={{ color: 'red' }}>{serverError}</div>}

        <button type="submit" disabled={isSubmitting}>
          {isSubmitting ? 'Logging in...' : 'Login'}
        </button>
      </form>
    </div>
  );
}