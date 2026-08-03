interface JobDetailsErrorProps {
  message: string | undefined;
}

export default function JobDetailsError({ message }: JobDetailsErrorProps) {
  return (
    <div className="flex flex-col items-center justify-center rounded-2xl border border-red-200 bg-red-50 p-10 text-center dark:border-red-900/50 dark:bg-red-900/10">
      <h2 className="text-xl font-bold text-red-700 dark:text-red-400">
        Failed to load job details
      </h2>
      <p className="mt-2 text-sm font-medium text-red-600/80 dark:text-red-400/80">
        {message || "An unexpected error occurred while fetching the data."}
      </p>
    </div>
  );
}