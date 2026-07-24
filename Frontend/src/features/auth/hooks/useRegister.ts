import { useState } from "react";
import { useNavigate } from "react-router-dom";

import { authApi } from "../api/authApi";

import type { RegisterRequest } from "../types";
import toast from "react-hot-toast";

export function useRegister() {
  const navigate = useNavigate();

  const [loading, setLoading] = useState(false);

  async function submit(data: RegisterRequest) {
    try {
      setLoading(true);

      await authApi.register(data);

      toast.success("Registration Completed! Now Login.");
      navigate("/login");
    
    }finally {
      setLoading(false);
    }
  }

  return {
    submit,
    loading,
  };
}