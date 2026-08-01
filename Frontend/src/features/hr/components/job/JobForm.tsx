import { useEffect, useState } from "react";
import type { CreateJobRequest, JobDetails } from "../../types/job";

interface JobFormProps {
  initialValues?: JobDetails;
  loading?: boolean;
  submitText: string;
  onSubmit: (values: CreateJobRequest) => void;
}

export default function JobForm({
  initialValues,
  loading = false,
  submitText,
  onSubmit,
}: JobFormProps) {
  const [title, setTitle] = useState("");
  const [department, setDepartment] = useState("");
  const [description, setDescription] = useState("");
  const [requirements, setRequirements] = useState("");

  useEffect(() => {
    if (!initialValues) return;
    setTitle(initialValues.title);
    setDepartment(initialValues.department);
    setDescription(initialValues.description);
    setRequirements(initialValues.requirements);
  }, [initialValues]);

  function handleSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    onSubmit({ title, department, description, requirements });
  }

  const inputClass = "w-full rounded-[10px] border border-[#E5EAE7] bg-white px-4 py-2.5 text-sm text-[#212529] outline-none transition focus:border-[#315343] focus:ring-1 focus:ring-[#315343] dark:border-[#315343] dark:bg-[#1E3329] dark:text-white dark:placeholder-white/40 dark:focus:border-[#C3F53C]";
  const labelClass = "mb-1.5 block text-sm font-semibold text-[#212529] dark:text-white";

  return (
    <form onSubmit={handleSubmit} className="space-y-5 rounded-2xl border border-[#E5EAE7] bg-white p-6 shadow-sm dark:border-[#315343] dark:bg-[#253f33]">
      <div>
        <label className={labelClass}>Job Title</label>
        <input
          value={title}
          onChange={(e) => setTitle(e.target.value)}
          required
          className={inputClass}
          placeholder="e.g. Senior Software Engineer"
        />
      </div>

      <div>
        <label className={labelClass}>Department</label>
        <input
          value={department}
          onChange={(e) => setDepartment(e.target.value)}
          required
          className={inputClass}
          placeholder="e.g. Engineering"
        />
      </div>

      <div>
        <label className={labelClass}>Description</label>
        <textarea
          rows={4}
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          required
          className={`${inputClass} resize-none`}
          placeholder="Provide a detailed description of the role..."
        />
      </div>

      <div>
        <label className={labelClass}>Requirements</label>
        <textarea
          rows={4}
          value={requirements}
          onChange={(e) => setRequirements(e.target.value)}
          required
          className={`${inputClass} resize-none`}
          placeholder="List the required skills and qualifications..."
        />
      </div>

      <div className="flex justify-end pt-2">
        <button
          type="submit"
          disabled={loading}
          className="flex items-center justify-center rounded-[10px] bg-[#315343] px-8 py-2.5 text-sm font-bold text-white shadow-sm shadow-[#315343]/20 transition-all hover:bg-[#C3F53C] hover:text-[#315343] active:scale-95 disabled:opacity-70 dark:bg-[#1E3329] dark:text-[#C3F53C] dark:hover:bg-[#C3F53C] dark:hover:text-[#1E3329]"
        >
          {loading ? "Saving..." : submitText}
        </button>
      </div>
    </form>
  );
}