import axios from 'axios';
import * as SecureStore from 'expo-secure-store';
import { useCallback, useState } from 'react';
import 'react-native-get-random-values';
import { v4 as uuidv4 } from 'uuid';

export function usePostApi<T>(url: string, options: any) {
  const baseUrl = process.env.EXPO_PUBLIC_BASE_API_URL;
  const requestId = uuidv4();

  const [response, setResponse] = useState<T | null>(null);
  const [error, setError] = useState<any>(null);
  const [isLoading, setIsLoading] = useState<boolean>(false);

  const post = useCallback(
    async (body: any) => {
      async function postApi() {
        try {
          setIsLoading(true);
          const token = await SecureStore.getItemAsync('token');
          const newOptions = {
            ...(options || {}),
            'Content-Type': 'application/json',
            'x-requestid': requestId,
            Authorization: `Bearer ${token}`,
          };

          const res = await axios.post<T>(
            `${baseUrl}/${url}`,
            body,
            newOptions
          );
          setResponse(res.data);
          setError(null);
          setIsLoading(false);
        } catch (err: any) {
          const { data } = err.response;
          setError(data.errorMessage);
        } finally {
          setIsLoading(false);
        }
      }
      postApi();
    },
    [baseUrl, options, requestId, url]
  );

  return { post, response, isLoading, error };
}
