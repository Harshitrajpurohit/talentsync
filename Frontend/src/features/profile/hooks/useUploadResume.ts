import { useState } from "react";

import { uploadResume } from "../api/resumeApi";

import type { Resume } from "../types/resume";

export function useUploadResume() {
  const [loading, setLoading] = useState(false);

  async function upload(file: File): Promise<Resume> {
    setLoading(true);
    
    try {
      return await uploadResume(file);
    } finally {
      setLoading(false);
    }
  }

  return {
    upload,
    loading,
  };
}