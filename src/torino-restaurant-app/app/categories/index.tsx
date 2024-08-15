import { StyleSheet } from 'react-native';

import { ThemedView } from '@/components/ThemedView';
import { useDispatch } from 'react-redux';
import { useGetApi } from '@/hooks/useGetApi';
import { API_URLS } from '@/constants/ApiUrls';
import React from 'react';
import { IListResultTemplate } from '@/models/result';
import { ICategoryModel } from '@/models/categories/category_detail';
import { CategoryItem } from '@/components/categories/CategoryItem';
import RoundedButton from '@/components/commons/RoundedButton';
import { Link } from 'expo-router';

export default function ListCategoriesScreen() {
  const { data, isLoading } = useGetApi<IListResultTemplate<ICategoryModel>>({
    url: API_URLS.CATEGORIES.GET_LIST,
    params: {},
    options: {},
    start: true,
  });

  const [isInEditMode, setIsInEditMode] = React.useState<boolean>(false);

  if (!(data?.items && data.items.length > 0) || isLoading) {
    return null;
  }

  return (
    <ThemedView style={{ flex: 1 }}>
      <ThemedView style={styles.menuContainer}>
        {data.items.map((category: ICategoryModel) => {
          return (
            <CategoryItem
              key={`category-${category.id}`}
              category={category}
              isInEditMode={isInEditMode}
            />
          );
        })}
        {isInEditMode && (
          <Link asChild href="/categories/create">
            <RoundedButton title="Add new category" onPress={() => {}} />
          </Link>
        )}
      </ThemedView>
      <ThemedView style={styles.editButtonContainer}>
        {!isInEditMode ? (
          <RoundedButton
            title="EDIT"
            onPress={() => {
              setIsInEditMode(true);
            }}
          />
        ) : (
          <RoundedButton
            title="CANCEL"
            onPress={() => {
              setIsInEditMode(false);
            }}
          />
        )}
      </ThemedView>
    </ThemedView>
  );
}

const styles = StyleSheet.create({
  menuContainer: {
    borderRadius: 8,
    display: 'flex',
    flexDirection: 'column',
    padding: 8,
  },
  editButtonContainer: {
    position: 'absolute',
    bottom: 0,
    alignSelf: 'center',
  },
});
