import axios, { AxiosError, AxiosRequestHeaders } from 'axios';
import { v4 as uuidv4 } from 'uuid';
import * as SecureStore from 'expo-secure-store';
import { customFlatten } from '@/utils/helper';
import { Alert } from 'react-native';
import { router } from 'expo-router';

const instance = axios.create({
  baseURL: process.env.EXPO_PUBLIC_BASE_API_URL,
});

instance.interceptors.request.use(
  async (config: any) => {
    const uuid = uuidv4();
    config.headers = {
      ...(config.headers ? config.headers : {}),
      // 'Content-Type': 'application/json',
      'x-requestid': uuid,
      accept: '*/*',
    };

    const accessToken = await SecureStore.getItemAsync('token');

    if (accessToken && !config.headers.Authorization) {
      config.headers.Authorization = `Bearer ${accessToken}`;
    }

    if (config.method === 'GET' || config.method === 'get') {
      config.paramsSerializer = (params: any) => {
        return Object.entries({ ...customFlatten(params) })
          .map(([key, value]) => `${key}=${value}`)
          .join('&');
      };
    }

    return config;
  },
  (error) => {
    return Promise.reject(
      error instanceof Error ? error : new Error('An unknown error occurred')
    );
  }
);

instance.interceptors.response.use(
  (response) => {
    // const { data } = response;
    // if (response.status && response.status !== 200) {
    //   let content = 'E500';
    //   if (data.code === 201) {
    //     content = 'E201';
    //   } else if (data.code === 202) {
    //     return response;
    //   }
    //   // Handle Error
    //   return Promise.reject(new Error(data));
    // }
    return response;
  },
  async (error: AxiosError) => {
    if (axios.isAxiosError(error) && error.response?.status === 401) {
      Alert.alert('Error', 'Session Time Out. Please Login Again', [
        {
          text: 'OK',
          onPress: function () {
            router.push('/login');
          },
        },
      ]);
    } else {
      Alert.alert('Error', 'An error occurred. Please try again later');
    }
    return Promise.reject(error);
  }
);

const handleError = async <TResult>(
  handler: () => TResult | Promise<TResult>
) => {
  try {
    return await handler();
  } catch (e) {
    console.log(e);
    if (axios.isAxiosError(e)) {
      if (e.response?.status === 401) {
      }
      if (e.response?.status === 403) {
      }
      if (e.response?.status === 500) {
      }
      if (e.response?.data) {
      }
    }
    throw e;
  }
};

const axiosGet = async <TResult>(
  url: string,
  params?: unknown
): Promise<TResult> => {
  return await handleError(async () => {
    const request = instance({
      method: 'GET',
      url,
      params,
    });
    const response = await request;
    return response.data as TResult;
  });
};

const axiosPost = async <TResult>(
  url: string,
  data?: unknown,
  params?: unknown,
  headers?: AxiosRequestHeaders
): Promise<TResult> => {
  return await handleError(async () => {
    const request = instance({
      method: 'POST',
      url,
      data,
      params,
      headers,
    });
    const response = await request;
    return response.data as TResult;
  });
};

const axiosDelete = async <TResult>(
  url: string,
  data?: unknown
): Promise<TResult> => {
  return await handleError(async () => {
    const request = instance({
      method: 'DELETE',
      url,
      data,
    });
    return (await request).data as TResult;
  });
};

const axiosPut = async <TResult>(
  url: string,
  data?: unknown,
  params?: unknown,
  headers?: AxiosRequestHeaders
): Promise<TResult> => {
  return await handleError(async () => {
    const request = instance({
      method: 'PUT',
      url,
      data,
      params,
      headers,
    });
    return (await request).data as TResult;
  });
};

const Request = {
  get: axiosGet,
  post: axiosPost,
  put: axiosPut,
  delete: axiosDelete,
};

export default Request;
