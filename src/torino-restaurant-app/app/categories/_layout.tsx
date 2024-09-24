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
          name="index"
          options={{
            headerTitle: 'Categories',
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
              return '';
            },
          }}
        />
      </Stack>
    </RootSiblingParent>
  );
}
