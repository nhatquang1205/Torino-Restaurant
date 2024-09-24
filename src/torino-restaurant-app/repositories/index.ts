import axios, { AxiosError, AxiosRequestHeaders } from 'axios';
import { v4 as uuidv4 } from 'uuid';
import * as SecureStore from 'expo-secure-store';
import { customFlatten } from '@/utils/helper';

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
      const errorConfig = error.config as any;
      //   const refreshToken = await getCookie('refreshToken');

      //   if (refreshToken) {
      //     try {
      //       const result = await authRepository.refreshToken({
      //         RefreshToken: refreshToken,
      //       });
      //       if (result.Data?.Token && result.Data.RefreshToken) {
      //         // Store new tokens in cookies
      //         setCookie('token', result.Data.Token, {
      //           req: errorConfig.ctx?.req,
      //           res: errorConfig.ctx?.res,
      //         });
      //         setCookie('refreshToken', result.Data.RefreshToken, {
      //           req: errorConfig.ctx?.req,
      //           res: errorConfig.ctx?.res,
      //         });

      //         // Retry the original request with the new token
      //         errorConfig.headers.Authorization = `Bearer ${result.Data.Token}`;
      //         return axios(errorConfig);
      //       } else {
      //         throw new Error(ERRORS.UNAUTHORIZED_ERROR);
      //       }
      //     } catch (e) {
      //       removeCookie('token', {
      //         req: errorConfig.ctx?.req,
      //         res: errorConfig.ctx?.res,
      //       });
      //       removeCookie('refreshToken', {
      //         req: errorConfig.ctx?.req,
      //         res: errorConfig.ctx?.res,
      //       });
      //       return Promise.reject(new Error(ERRORS.UNAUTHORIZED_ERROR));
      //     }
      //   }
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
