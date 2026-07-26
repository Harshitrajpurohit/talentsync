import { useState } from "react";
import { Link } from "react-router-dom";
import {
  Eye,
  EyeOff,
  Loader2,
  Mail,
  Lock,
  BriefcaseBusiness,
} from "lucide-react";
import { useLogin } from "../hooks/UseLogin";

export default function LoginForm() {
  const { submit, loading } = useLogin();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [showPassword, setShowPassword] = useState(false);

  async function handleSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    await submit({
      email: email,
      password: password,
    });
  }

  return (
    <div className="mx-auto w-full max-w-sm">
      {/* Mobile-only branding */}
      <div className="mb-8 flex items-center justify-center gap-2.5 lg:hidden">
        <div className="flex h-9 w-9 items-center justify-center rounded-full bg-[#C3F53C] text-[#315343]">
          <BriefcaseBusiness className="h-4 w-4" strokeWidth={2.5} />
        </div>
        <span className="text-2xl font-bold tracking-tight text-[#212529]">
          SupplySync
        </span>
      </div>

      <div className="mb-8 text-center lg:text-left">
        <h2 className="text-2xl font-bold tracking-tight text-[#212529] sm:text-3xl">
          Welcome Back
        </h2>
        <p className="mt-2 text-sm text-[#75837D]">
          Sign in to continue to your management dashboard.
        </p>
      </div>

      <form onSubmit={handleSubmit} className="space-y-4">
        {/* Email */}
        <div>
          <label className="mb-1.5 block text-sm font-semibold text-[#212529]">
            Email Address
          </label>
          <div className="relative">
            <Mail size={18} className="absolute left-3.5 top-1/2 -translate-y-1/2 text-[#75837D]" />
            <input
              type="email"
              required
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              placeholder="name@company.com"
              className="w-full rounded-[10px] border border-[#E5EAE7] bg-white py-2.5 pl-10 pr-3 text-sm text-[#212529] placeholder-[#75837D] outline-none transition focus:border-[#315343] focus:ring-1 focus:ring-[#315343]"
            />
          </div>
        </div>

        {/* Password */}
        <div>
          <label className="mb-1.5 block text-sm font-semibold text-[#212529]">
            Password
          </label>
          <div className="relative">
            <Lock size={18} className="absolute left-3.5 top-1/2 -translate-y-1/2 text-[#75837D]" />
            <input
              type={showPassword ? "text" : "password"}
              required
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              placeholder="Enter your password"
              className="w-full rounded-[10px] border border-[#E5EAE7] bg-white py-2.5 pl-10 pr-10 text-sm text-[#212529] placeholder-[#75837D] outline-none transition focus:border-[#315343] focus:ring-1 focus:ring-[#315343]"
            />
            <button
              type="button"
              onClick={() => setShowPassword((v) => !v)}
              className="absolute right-3.5 top-1/2 -translate-y-1/2 text-[#75837D] transition hover:text-[#212529]"
            >
              {showPassword ? <EyeOff size={18} /> : <Eye size={18} />}
            </button>
          </div>
        </div>

        {/* Remember me & Forgot Password */}
        <div className="flex items-center justify-between pt-1 text-sm">
          <label className="flex items-center gap-2 font-medium text-[#75837D] cursor-pointer">
            <input
              type="checkbox"
              className="h-4 w-4 rounded border-[#E5EAE7] bg-white text-[#315343] focus:ring-[#315343]"
            />
            Remember me
          </label>

          <Link
            to="/forgot-password"
            className="text-sm font-bold text-[#315343] transition hover:opacity-80 hover:underline"
          >
            Forgot password?
          </Link>
        </div>

        {/* Submit Button */}
        <button
          type="submit"
          disabled={loading}
          className="mt-4 flex w-full items-center justify-center rounded-[10px] bg-[#315343] py-2.5 text-sm font-bold text-white transition-all hover:bg-[#233f32] active:scale-[0.99] disabled:cursor-not-allowed disabled:opacity-70 shadow-sm shadow-[#315343]/20"
        >
          {loading ? (
            <>
              <Loader2 size={18} className="mr-2 animate-spin text-[#C3F53C]" />
              Signing In...
            </>
          ) : (
            "Sign In"
          )}
        </button>
      </form>

      <div className="mt-6 text-center text-sm font-medium text-[#75837D]">
        Don't have an account?{" "}
        <Link
          to="/register"
          className="font-bold text-[#315343] transition hover:opacity-80 hover:underline"
        >
          Register
        </Link>
      </div>
    </div>
  );
}