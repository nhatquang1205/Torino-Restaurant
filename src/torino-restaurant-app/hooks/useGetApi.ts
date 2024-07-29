import axios from 'axios';
import * as SecureStore from 'expo-secure-store';
import { useCallback, useEffect, useState } from 'react';
import 'react-native-get-random-values';
import { v4 as uuidv4 } from 'uuid';

export function useGetApi<T>(props: any) {
  const { url, params, options, start } = props;
  const baseUrl = process.env.EXPO_PUBLIC_BASE_API_URL;
  const requestId = uuidv4();

  const [data, setData] = useState<T | null>(null);
  const [error, setError] = useState<any>(null);
  const [isLoading, setIsLoading] = useState<boolean>(false);

  const fetchData = useCallback(async () => {
    try {
      setIsLoading(true);
      const token = await SecureStore.getItemAsync('token');
      const newOptions = {
        ...(options || {}),
        'Content-Type': 'application/json',
        'x-requestid': requestId,
        Authorization: `Bearer ${token}`,
      };

      const res = await axios.get<T>(`${baseUrl}/${url}`, newOptions);
      setData(res.data);
      setError(null);
      setIsLoading(false);
    } catch (err: any) {
      const { data } = err.response;
      setError(data.errorMessage);
    } finally {
      setIsLoading(false);
    }
  }, [params]);

  useEffect(() => {
    if (start) {
      fetchData();
    }
  }, [start]);

  const refresh = async () => {
    await fetchData();
  };

  return { refresh, data, isLoading, error };
}
