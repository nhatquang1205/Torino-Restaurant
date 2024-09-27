import { Image, StyleSheet } from 'react-native';

import ParallaxScrollView from '@/components/ParallaxScrollView';
import { ThemedView } from '@/components/ThemedView';
import { useEffect, useState } from 'react';
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
import { PRIMARY } from '@/constants/Colors';
import { LoginTextInput } from '@/components/login/TextInputComponent';
import AppIcon from '@/components/commons/Icon';
import RoundedButton from '@/components/commons/RoundedButton';
import Request from '@/repositories';

export default function LoginScreen() {
  const dispatch = useDispatch();
  const { isAuthenticated } = useSelector((state: any) => state.app);
  const [isLoading, setIsLoading] = useState<boolean>(false);

  useEffect(() => {
    if (isAuthenticated) {
      router.replace('/');
    }
  }, [isAuthenticated]);

  async function setToken(response: ILoginResponse) {
    await SecureStore.setItemAsync('token', response.token);
    await SecureStore.setItemAsync('refresh_token', response.refreshToken);
    await SecureStore.setItemAsync(
      'refresh_token_expiry_time',
      response.refreshTokenExpiryTime
    );
  }

  const [username, setUsername] = React.useState<string>('');
  const [password, setPassword] = React.useState<string>('');

  const handleOnChangeUserName = function (value: string) {
    setUsername(value);
  };
  const handleOnChangePassword = function (value: string) {
    setPassword(value);
  };
  const handleOnPress = async function () {
    if (!username || !password) {
      alert('Vui lòng nhập tên đăng nhập và mật khẩu');
      return;
    }
    setIsLoading(true);
    const loginInput: ILoginInput = {
      username: username,
      password: password,
    };

    const response = await Request.post<ILoginResponse>(
      API_URLS.AUTH.LOGIN,
      loginInput
    );
    if (response) {
      setToken(response).then(() => {
        dispatch(setIsAuthenticate(true));
      });
    }
    setIsLoading(false);
  };
  return (
    <ParallaxScrollView
      headerBackgroundColor={{ light: '#FFFFFF', dark: '#FFFFFF' }}
      headerImage={
        <Image
          source={require('@/assets/images/login-header-image.png')}
          style={styles.reactLogo}
        />
      }
    >
      <ThemedView style={styles.titleContainer}>
        <ThemedText
          type="title"
          style={{
            color: PRIMARY,
            ...styles.title,
          }}
        >
          Torino
        </ThemedText>
        <ThemedText type="title" style={styles.title}>
          Restaurant
        </ThemedText>
        <AppIcon
          source={require('@/assets/icons/store.png')}
          width={50}
          height={50}
        />
      </ThemedView>
      <ThemedView style={styles.inputContainer}>
        <LoginTextInput
          placeholder="Tên đăng nhập"
          value={username}
          onChangeText={handleOnChangeUserName}
        >
          <AppIcon
            source={require('@/assets/icons/account.png')}
            width={30}
            height={30}
          />
        </LoginTextInput>
        <LoginTextInput
          placeholder="Mật khẩu"
          value={password}
          maxLength={32}
          secureTextEntry
          onChangeText={handleOnChangePassword}
        >
          <AppIcon
            source={require('@/assets/icons/key.png')}
            width={30}
            height={30}
          />
        </LoginTextInput>
      </ThemedView>
      <ThemedView style={{ marginTop: 16, alignItems: 'center', gap: 16 }}>
        <RoundedButton
          title="Đăng nhập"
          onPress={handleOnPress}
          isLoading={isLoading}
          buttonStyle={{ backgroundColor: 'rgba(138, 138, 138, 0.13)' }}
          textStyle={{
            fontSize: 30,
            fontFamily: 'LoveYaLikeASister',
            lineHeight: 30,
            fontWeight: 'heavy',
            letterSpacing: 0.25,
            color: 'black',
            opacity: 1,
            paddingTop: 10,
          }}
        />
        <ThemedText type="default">
          Quên mật khẩu?{' '}
          <ThemedText type="defaultSemiBold">Nhắn cho Quang!</ThemedText>
        </ThemedText>
      </ThemedView>
      <ThemedView style={styles.footer}>
        <AppIcon
          source={require('@/assets/icons/spaghetti.png')}
          width={50}
          height={50}
        />
        <AppIcon
          source={require('@/assets/icons/pizza.png')}
          width={50}
          height={50}
        />
        <AppIcon
          source={require('@/assets/icons/gelato.png')}
          width={50}
          height={50}
        />
      </ThemedView>
    </ParallaxScrollView>
  );
}

const styles = StyleSheet.create({
  title: {
    fontFamily: 'LoveYaLikeASister',
    textAlign: 'center',
    verticalAlign: 'middle',
  },
  titleContainer: {
    flexDirection: 'column',
    alignItems: 'center',
    gap: 16,
    height: 'auto',
  },
  stepContainer: {
    gap: 8,
    marginBottom: 8,
  },
  inputContainer: {
    paddingTop: 16,
    gap: 24,
  },
  footer: {
    display: 'flex',
    flexDirection: 'row',
    gap: 16,
    justifyContent: 'space-between',
    paddingLeft: 40,
    paddingRight: 40,
  },
  reactLogo: {
    display: 'flex',
    position: 'absolute',
    top: -20,
  },
});
