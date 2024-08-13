import { StyleSheet, Button } from 'react-native';

import ParallaxScrollView from '@/components/ParallaxScrollView';
import { ThemedView } from '@/components/ThemedView';
import { useDispatch } from 'react-redux';
import { setIsAuthenticate } from '@/store/app/app-slice';
import { router } from 'expo-router';
import * as SecureStore from 'expo-secure-store';
import { useGetApi } from '@/hooks/useGetApi';
import { API_URLS } from '@/constants/ApiUrls';
import React from 'react';
import { MenuItem } from '@/components/MenuItem';
import { ThemedText } from '@/components/ThemedText';

export default function HomeScreen() {
  useGetApi({
    url: API_URLS.CATEGORIES.GET_LIST,
    params: {},
    options: {},
    start: true,
  });

  const dispatch = useDispatch();

  const handleClickLogOutBtn = function () {
    async function removeToken() {
      await SecureStore.deleteItemAsync('token');
      await SecureStore.deleteItemAsync('refresh_token');
      await SecureStore.deleteItemAsync('refresh_token_expiry_time');
    }
    removeToken().then(() => {
      dispatch(setIsAuthenticate(false));
      router.replace('/login');
    });
  };

  return (
    <ParallaxScrollView
      headerBackgroundColor={{ light: '#A1CEDC', dark: '#1D3D47' }}
    >
      <ThemedView style={styles.menuContainer}>
        <ThemedText>12345</ThemedText>
      </ThemedView>
      <ThemedView>
        <Button title="Đăng xuất" onPress={handleClickLogOutBtn} />
      </ThemedView>
    </ParallaxScrollView>
  );
}

const styles = StyleSheet.create({
  menuContainer: {
    borderRadius: 8,
    display: 'flex',
    flexDirection: 'row',
    padding: 8,
  },
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
