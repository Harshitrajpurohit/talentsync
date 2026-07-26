import { useState } from "react";

import { replaceResume } from "../api/resumeApi";

import type { Resume } from "../types/resume";

export function useReplaceResume() {
  const [loading, setLoading] = useState(false);

  async function replace(
    id: string,
    file: File
  ): Promise<Resume> {
    setLoading(true);

    try {
      return await replaceResume(id, file);
    } finally {
      setLoading(false);
    }
  }

  return {
    replace,
    loading,
  };
}