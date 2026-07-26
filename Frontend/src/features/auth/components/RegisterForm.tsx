import { useState } from "react";
import { Link } from "react-router-dom";
import {
  Eye,
  EyeOff,
  Loader2,
  Mail,
  Lock,
  Phone,
  User,
  AlertCircle,
  BriefcaseBusiness,
} from "lucide-react";
import validator from "validator";
import { useRegister } from "../hooks/useRegister";

export default function RegisterForm() {
  const [showPassword, setShowPassword] = useState(false);
  const { submit, loading } = useRegister();

  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [phone, setPhone] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [error, setError] = useState("");

  async function handleSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    setError("");

    if (!name || !email || !phone || !password) {
      setError("Please fill in all fields.");
      return;
    }

    if (!validator.isEmail(email)) {
      setError("Please enter a valid email address.");
      return;
    }
    if (!validator.isMobilePhone(phone, "any")) {
      setError("Please enter a valid global mobile number.");
      return;
    }

    if (password.length < 6) {
      setError("Password must be at least 6 characters.");
      return;
    }

    if (password !== confirmPassword) {
      setError("Password and Confirm Password Should Match");
      return;
    }

    await submit({ name, email, phone, password });
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
          Create an account
        </h2>
        <p className="mt-2 text-sm text-[#75837D]">
          Join SupplySync to streamline procurement workflows.
        </p>
      </div>

      <form onSubmit={handleSubmit} className="space-y-4">
        {error && (
          <div className="flex items-center gap-2 rounded-[10px] border border-red-200 bg-red-50 p-3 text-sm font-medium text-red-600">
            <AlertCircle className="h-4 w-4 shrink-0" />
            <span>{error}</span>
          </div>
        )}

        {/* 2-Column fields on desktop, full width on mobile */}
        <div className="grid gap-4 sm:grid-cols-2">
          {/* Full Name */}
          <div>
            <label className="mb-1.5 block text-sm font-semibold text-[#212529]">
              Full Name
            </label>
            <div className="relative">
              <User size={18} className="absolute left-3.5 top-1/2 -translate-y-1/2 text-[#75837D]" />
              <input
                type="text"
                required
                value={name}
                onChange={(e) => setName(e.target.value)}
                placeholder="Harshit Rajpurohit"
                className="w-full rounded-[10px] border border-[#E5EAE7] bg-white py-2.5 pl-10 pr-3 text-sm text-[#212529] placeholder-[#75837D] outline-none transition focus:border-[#315343] focus:ring-1 focus:ring-[#315343]"
              />
            </div>
          </div>

          {/* Phone */}
          <div>
            <label className="mb-1.5 block text-sm font-semibold text-[#212529]">
              Phone
            </label>
            <div className="relative">
              <Phone size={18} className="absolute left-3.5 top-1/2 -translate-y-1/2 text-[#75837D]" />
              <input
                type="tel"
                required
                value={phone}
                onChange={(e) => setPhone(e.target.value)}
                placeholder="+91 1122334455"
                className="w-full rounded-[10px] border border-[#E5EAE7] bg-white py-2.5 pl-10 pr-3 text-sm text-[#212529] placeholder-[#75837D] outline-none transition focus:border-[#315343] focus:ring-1 focus:ring-[#315343]"
              />
            </div>
          </div>
        </div>

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
              placeholder="Min. 6 characters"
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

        {/* Confirm Password */}
        <div>
          <label className="mb-1.5 block text-sm font-semibold text-[#212529]">
            Confirm Password
          </label>
          <div className="relative">
            <Lock size={18} className="absolute left-3.5 top-1/2 -translate-y-1/2 text-[#75837D]" />
            <input
              type={showPassword ? "text" : "password"}
              required
              value={confirmPassword}
              onChange={(e) => setConfirmPassword(e.target.value)}
              placeholder="Min. 6 characters"
              className="w-full rounded-[10px] border border-[#E5EAE7] bg-white py-2.5 pl-10 pr-10 text-sm text-[#212529] placeholder-[#75837D] outline-none transition focus:border-[#315343] focus:ring-1 focus:ring-[#315343]"
            />
          </div>
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
              Creating account...
            </>
          ) : (
            "Create Account"
          )}
        </button>
      </form>

      <div className="mt-6 text-center text-sm font-medium text-[#75837D]">
        Already have an account?{" "}
        <Link
          to="/login"
          className="font-bold text-[#315343] transition hover:opacity-80 hover:underline"
        >
          Sign in
        </Link>
      </div>
    </div>
  );
}