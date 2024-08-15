import { Link, Stack } from 'expo-router';
import React from 'react';

import { Colors } from '@/constants/Colors';
import { useColorScheme } from '@/hooks/useColorScheme';
import { Pressable } from 'react-native';
import AntDesign from '@expo/vector-icons/AntDesign';

export default function CategoryLayout() {
  const colorScheme = useColorScheme();
  return (
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
        name="index"
        options={{
          headerTitle: 'Categories',
          headerLeft: () => (
            <Link asChild href={'/'}>
              <Pressable>
                <AntDesign name="back" size={24} color="black" />
              </Pressable>
            </Link>
          ),
        }}
      />
      <Stack.Screen
        name="create"
        options={{
          headerTitle: 'Create category',
        }}
      />
      <Stack.Screen
        name="[id]"
        getId={({ params }) => String(Date.now())}
        options={{
          headerTitle: (props) => {
            console.log(props);
            return '';
          },
        }}
      />
    </Stack>
  );
}
