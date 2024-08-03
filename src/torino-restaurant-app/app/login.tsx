import { Image, StyleSheet, Button, TextInput } from 'react-native';

import ParallaxScrollView from '@/components/ParallaxScrollView';
import { ThemedView } from '@/components/ThemedView';
import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { router } from 'expo-router';
import { usePostApi } from '@/hooks/usePostApi';
import { ILoginResponse } from '@/models/auth/login_response';
import * as SecureStore from 'expo-secure-store';
import { setIsAuthenticate } from '@/store/app/app-slice';
import { API_URLS } from '@/constants/ApiUrls';
import React from 'react';
import { ILoginInput } from '@/models/auth/login_input';
import { ThemedText } from '@/components/ThemedText';

export default function LoginScreen() {
  const dispatch = useDispatch();
  const { isAuthenticated } = useSelector((state: any) => state.app);
  const { post, response, error } = usePostApi<ILoginResponse>(
    API_URLS.AUTH.LOGIN,
    {}
  );

  useEffect(() => {
    if (isAuthenticated) {
      router.replace('/');
    }
  }, [isAuthenticated]);

  useEffect(() => {
    async function setToken(response: ILoginResponse) {
      await SecureStore.setItemAsync('token', response.token);
      await SecureStore.setItemAsync('refresh_token', response.refreshToken);
      await SecureStore.setItemAsync(
        'refresh_token_expiry_time',
        response.refreshTokenExpiryTime
      );
    }
  
    if (response) {
      setToken(response).then(() => {
        dispatch(setIsAuthenticate(true));
      });
    }

    if (error) {
      console.log(error)
    }

  }, [dispatch, response, error]);

  

  const [username, setUsername] = React.useState<string>('');
  const [password, setPassword] = React.useState<string>('');

  const handleOnChangeUserName = function(value: string) {
    setUsername(value);
  }
  const handleOnChangePassword = function (value: string) {
    setPassword(value);
  }
  const handleOnPress = function () {
    const loginInput: ILoginInput = {
      username: username,
      password: password,
    }
    post(loginInput);
  }
  return (
    <ParallaxScrollView
      headerBackgroundColor={{ light: '#A1CEDC', dark: '#1D3D47' }}
      headerImage={
        <Image
          src="http://165.22.254.54:9000/torino/CHOCOLATE-ECLAIR.JPG"
          style={styles.reactLogo}
        />
      }
    >
      <ThemedView>
        <TextInput placeholder="Tên đăng nhập" value={username} maxLength={32} onChangeText={handleOnChangeUserName}/>
        <ThemedView>
        <TextInput placeholder="mật khẩu" value={password} maxLength={32} secureTextEntry onChangeText={handleOnChangePassword}/>
      </ThemedView>
        <Button
          title="Đăng nhập"
          onPress={handleOnPress}
        />
      </ThemedView>
      <ThemedText>{error}</ThemedText>        

    </ParallaxScrollView>
  );
}

const styles = StyleSheet.create({
  titleContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 8,
  },
  stepContainer: {
    gap: 8,
    marginBottom: 8,
  },
  reactLogo: {
    height: 178,
    width: 290,
    bottom: 0,
    left: 0,
    position: 'absolute',
  },
});
