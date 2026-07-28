export default function DashboardSkeleton() {
  return (
    <div className="animate-pulse space-y-6">
        
      <div className="h-24 rounded-[20px] bg-[#E5EAE7] dark:bg-[#1e3329]" />


      <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 xl:grid-cols-4">
        {[...Array(4)].map((_, index) => (
          <div
            key={index}
            className="h-24 rounded-[20px] bg-[#E5EAE7] dark:bg-[#1e3329]"
          />
        ))}
      </div>


      <div className="grid grid-cols-1 gap-6 xl:grid-cols-3">
        
        <div className="flex flex-col space-y-6 xl:col-span-2">
          <div className="h-56 rounded-[20px] bg-[#E5EAE7] dark:bg-[#1e3329]" />
          <div className="h-72 rounded-[20px] bg-[#E5EAE7] dark:bg-[#1e3329]" />
        </div>
        
        <div className="flex flex-col space-y-6">
          <div className="h-32 rounded-[20px] bg-[#E5EAE7] dark:bg-[#1e3329]" />
          <div className="h-56 rounded-[20px] bg-[#E5EAE7] dark:bg-[#1e3329]" />
        </div>
      </div>
    </div>
  );
}