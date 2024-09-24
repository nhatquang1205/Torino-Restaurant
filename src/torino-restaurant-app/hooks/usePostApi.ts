import axios from 'axios';
import * as SecureStore from 'expo-secure-store';
import { useCallback, useState } from 'react';
import 'react-native-get-random-values';
import Toast from 'react-native-root-toast';
import { v4 as uuidv4 } from 'uuid';

export function usePostApi<T>(
  url: string,
  options: any,
  isPut: boolean = false
) {
  const baseUrl = process.env.EXPO_PUBLIC_BASE_API_URL;
  const requestId = uuidv4();

  const [isLoading, setIsLoading] = useState<boolean>(false);

  const post = useCallback(
    async (body: any) => {
      async function postApi() {
        try {
          setIsLoading(true);
          const token = await SecureStore.getItemAsync('token');
          const newOptions = {
            ...(options || {}),
            // 'Content-Type': 'application/json',
            'x-requestid': requestId,
            Authorization: `Bearer ${token}`,
          };

          const res = isPut
            ? await axios.put<T>(`${baseUrl}/${url}`, body, {
                headers: newOptions,
              })
            : await axios.post<T>(`${baseUrl}/${url}`, body, {
                headers: newOptions,
              });
          setIsLoading(false);
          return res;
        } catch (err: any) {
          console.log(err);
          const { data } = err.response;
          setIsLoading(false);
          alert(data);
        } finally {
          setIsLoading(false);
        }
      }
      const response = await postApi();
      if (response?.status === 200) {
        return response.data as T;
      }
      if (response?.status === 401) {
        //TODO Handle Unauthorize
      }

      //TODO Handle Error

      if (response?.status === 204) {
        Toast.show('Success', {
          duration: Toast.durations.LONG,
          position: 100,
        });
      }
      return response?.data;
    },
    [baseUrl, options, requestId, url]
  );

  return { post, isLoading };
}
