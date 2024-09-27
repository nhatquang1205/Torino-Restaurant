import { Link, Stack } from 'expo-router';
import React from 'react';

import { Colors } from '@/constants/Colors';
import { useColorScheme } from '@/hooks/useColorScheme';
import { Pressable } from 'react-native';
import { RootSiblingParent } from 'react-native-root-siblings';
import { Octicons } from '@expo/vector-icons';

export default function CategoryLayout() {
  const colorScheme = useColorScheme();
  return (
    <RootSiblingParent>
      <Stack
        screenOptions={{
          headerStyle: {
            backgroundColor: Colors[colorScheme ?? 'light'].background,
          },
          headerTintColor: 'black',
          headerTitleStyle: {
            fontWeight: 'bold',
          },
        }}
      >
        <Stack.Screen
          name="categories/index"
          options={{
            headerTitle: 'Danh sách danh mục',
            headerLeft: () => (
              <Link asChild href={'/'} style={{ paddingRight: 15 }}>
                <Pressable>
                  <Octicons name="arrow-left" size={24} color="black" />
                </Pressable>
              </Link>
            ),
          }}
        />
        <Stack.Screen
          name="categories/create"
          options={{
            headerTitle: 'Thêm danh mục',
          }}
        />
        <Stack.Screen
          name="categories/[id]"
          options={{
            headerTitle: (props) => {
              return '';
            },
          }}
        />
        <Stack.Screen name="categories/[id]/products" />
        <Stack.Screen
          name="categories/[id]/products/create"
          options={{
            headerTitle: 'Thêm sản phẩm',
          }}
        />
        <Stack.Screen
          name="products/[id]"
          options={{ headerTitle: 'CHI TIẾT' }}
        />
      </Stack>
    </RootSiblingParent>
  );
}
