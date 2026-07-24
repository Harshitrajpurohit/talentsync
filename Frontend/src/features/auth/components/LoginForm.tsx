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
      <div className="mb-4 flex items-center justify-center gap-2 sm:hidden">
        <div className="flex h-8 w-8 items-center justify-center rounded-lg border border-emerald-500/20 bg-emerald-500/10 text-emerald-400">
          <BriefcaseBusiness className="h-4 w-4" />
        </div>
        <span className="text-lg font-bold tracking-tight text-white">
          TalentSync
        </span>
      </div>

      <div className="mb-5 text-center sm:text-left">
        <h2 className="text-xl font-bold tracking-tight text-white sm:text-2xl">
          Welcome Back
        </h2>
        <p className="mt-1 text-sm text-slate-400">
          Sign in to continue to your recruitment dashboard.
        </p>
      </div>

      <form onSubmit={handleSubmit} className="space-y-3.5">
        {/* Email */}
        <div>
          <label className="mb-1 block text-sm font-medium text-slate-300">
            Email Address
          </label>
          <div className="relative">
            <Mail size={18} className="absolute left-3 top-1/2 -translate-y-1/2 text-slate-500" />
            <input
              type="email"
              required
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              placeholder="name@company.com"
              className="w-full rounded-xl border border-slate-800 bg-slate-950/60 py-2.5 pl-10 pr-3 text-sm text-white placeholder-slate-500 outline-none transition focus:border-emerald-500 focus:ring-1 focus:ring-emerald-500"
            />
          </div>
        </div>

        {/* Password */}
        <div>
          <label className="mb-1 block text-sm font-medium text-slate-300">
            Password
          </label>
          <div className="relative">
            <Lock size={18} className="absolute left-3 top-1/2 -translate-y-1/2 text-slate-500" />
            <input
              type={showPassword ? "text" : "password"}
              required
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              placeholder="Enter your password"
              className="w-full rounded-xl border border-slate-800 bg-slate-950/60 py-2.5 pl-10 pr-10 text-sm text-white placeholder-slate-500 outline-none transition focus:border-emerald-500 focus:ring-1 focus:ring-emerald-500"
            />
            <button
              type="button"
              onClick={() => setShowPassword((v) => !v)}
              className="absolute right-3 top-1/2 -translate-y-1/2 text-slate-500 transition hover:text-slate-300"
            >
              {showPassword ? <EyeOff size={18} /> : <Eye size={18} />}
            </button>
          </div>
        </div>

        {/* Remember me & Forgot Password */}
        <div className="flex items-center justify-between pt-0.5 text-sm">
          <label className="flex items-center gap-2 text-slate-400">
            <input
              type="checkbox"
              className="h-4 w-4 rounded border-slate-800 bg-slate-950/60 text-emerald-500 focus:ring-emerald-500 focus:ring-offset-slate-900"
            />
            Remember me
          </label>

          <Link
            to="/forgot-password"
            className="text-sm font-medium text-emerald-400 transition hover:text-emerald-300 hover:underline"
          >
            Forgot password?
          </Link>
        </div>

        {/* Submit Button */}
        <button
          type="submit"
          disabled={loading}
          className="mt-1 flex w-full items-center justify-center rounded-xl bg-emerald-500 py-2.5 text-sm font-semibold text-slate-950 transition hover:bg-emerald-400 active:scale-[0.99] disabled:cursor-not-allowed disabled:opacity-50"
        >
          {loading ? (
            <>
              <Loader2 size={18} className="mr-2 animate-spin" />
              Signing In...
            </>
          ) : (
            "Sign In"
          )}
        </button>
      </form>

      <div className="mt-5 text-center text-sm text-slate-400">
        Don't have an account?{" "}
        <Link
          to="/register"
          className="font-semibold text-emerald-400 transition hover:text-emerald-300 hover:underline"
        >
          Register
        </Link>
      </div>
    </div>
  );
}