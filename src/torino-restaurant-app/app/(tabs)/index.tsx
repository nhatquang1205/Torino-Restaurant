import { StyleSheet, Image } from 'react-native';

import ParallaxScrollView from '@/components/ParallaxScrollView';
import { ThemedView } from '@/components/ThemedView';
import { useGetApi } from '@/hooks/useGetApi';
import { API_URLS } from '@/constants/ApiUrls';
import React from 'react';
import { MenuItem } from '@/components/MenuItem';

export default function HomeScreen() {
  useGetApi({
    url: API_URLS.CATEGORIES.GET_LIST,
    params: {},
    options: {},
    start: true,
  });

  return (
    <ParallaxScrollView
      headerBackgroundColor={{ light: '#A1CEDC', dark: '#1D3D47' }}
      headerImage={
        <Image
          source={require('@/assets/images/home-banner.jpg')}
          style={styles.reactLogo}
        />
      }
    >
      <ThemedView style={styles.container}>
        <ThemedView style={styles.menuContainer}>
          <MenuItem
            label="Menu"
            iconName="restaurant-menu"
            href="/categories"
          />
        </ThemedView>
      </ThemedView>
    </ParallaxScrollView>
  );
}

const styles = StyleSheet.create({
  container: {
    display: 'flex',
    flexDirection: 'column',
    justifyContent: 'space-between',
  },
  menuContainer: {
    borderRadius: 8,
    display: 'flex',
    flexDirection: 'row',
    padding: 8,
    gap: 16,
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
    height: 200,
    width: '100%',
    bottom: 0,
    left: 0,
    position: 'absolute',
  },
});
