import { useState } from "react";
import { useNavigate } from "react-router-dom";

import { authApi } from "../api/authApi";
import { useAuth } from "../../../app/hooks/useAuth";

import type { LoginRequest } from "../types";

export function useLogin() {
  const navigate = useNavigate();
  const { login } = useAuth();

  const [loading, setLoading] = useState(false);

  async function submit(data: LoginRequest) {
    try {
      setLoading(true);

      const response = await authApi.login(data);
      login({
        token: response.token,
        userId: response.userId,
        fullName: response.name,
        email: response.email,
        role: response.role,
        });

      switch (response.role) {
        case "Admin":
          navigate("/admin");
          break;

        case "Recruiter":
          navigate("/recruiter");
          break;

        case "Candidate":
          navigate("/candidate");
          break;

        case "Employee":
          navigate("/employee");
          break;

        case "HR":
          navigate("/hr");
          break;

        case "Manager":
          navigate("/manager");
          break;

        default:
          navigate("/");
      }
    } finally {
      setLoading(false);
    }
  }

  return {
    submit,
    loading,
  };
}