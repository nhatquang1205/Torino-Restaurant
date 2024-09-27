import { StyleSheet, TextBase } from 'react-native';

import { ThemedView } from '@/components/ThemedView';
import { useDispatch } from 'react-redux';
import { useGetApi } from '@/hooks/useGetApi';
import { API_URLS } from '@/constants/ApiUrls';
import React from 'react';
import { IListResultTemplate } from '@/models/result';
import { ICategoryModel } from '@/models/categories/category_detail';
import { CategoryItem } from '@/components/categories/CategoryItem';
import RoundedButton from '@/components/commons/RoundedButton';
import { Link, Stack } from 'expo-router';
import { PRIMARY } from '@/constants/Colors';
import { FlatList } from 'react-native';

export default function ListCategoriesScreen() {
  const { data, isLoading, refresh } = useGetApi<
    IListResultTemplate<ICategoryModel>
  >({
    url: API_URLS.CATEGORIES.GET_LIST,
    params: {},
    options: {},
    start: true,
  });

  const [isInEditMode, setIsInEditMode] = React.useState<boolean>(false);

  if (!(data?.items && data.items.length > 0) || isLoading) {
    return null;
  }

  const renderCategory = (item: ICategoryModel) => {
    return (
      <CategoryItem
        key={`category-${item.id}`}
        category={item}
        isInEditMode={isInEditMode}
        refreshCategories={refresh}
      />
    );
  };

  const renderFooter = () => {
    return (
      isInEditMode && (
        <ThemedView
          style={{ width: '50%', alignSelf: 'center', paddingBottom: 12 }}
        >
          <Link asChild href="/categories/create">
            <RoundedButton
              title="Thêm danh mục"
              onPress={() => {}}
              textStyle={{
                fontSize: 16,
                lineHeight: 16,
                letterSpacing: 0.25,
                color: 'black',
                paddingTop: 6,
              }}
              buttonStyle={{
                width: '100%',
                alignItems: 'center',
                justifyContent: 'center',
                paddingVertical: 12,
                paddingHorizontal: 32,
                borderRadius: 32,
                elevation: 1,
                backgroundColor: PRIMARY,
              }}
            />
          </Link>
        </ThemedView>
      )
    );
  };

  return (
    <ThemedView style={{ flex: 1, display: 'flex', flexDirection: 'column' }}>
      <ThemedView style={styles.menuContainer}>
        <FlatList
          data={data.items}
          renderItem={({ item }) => renderCategory(item)}
          ListFooterComponent={() => renderFooter()}
        />
      </ThemedView>
      <ThemedView style={styles.editButtonContainer}>
        {!isInEditMode ? (
          <RoundedButton
            title="Chỉnh sửa danh mục"
            onPress={() => {
              setIsInEditMode(true);
            }}
            textStyle={{
              fontSize: 16,
              lineHeight: 16,
              letterSpacing: 0.25,
              color: 'black',
              paddingTop: 6,
            }}
            buttonStyle={{
              width: '100%',
              alignItems: 'center',
              justifyContent: 'center',
              paddingVertical: 12,
              paddingHorizontal: 32,
              borderRadius: 32,
              elevation: 1,
              backgroundColor: PRIMARY,
            }}
          />
        ) : (
          <RoundedButton
            title="Huỷ bỏ"
            onPress={() => {
              setIsInEditMode(false);
            }}
            textStyle={{
              fontSize: 16,
              lineHeight: 16,
              letterSpacing: 0.25,
              color: 'black',
              paddingTop: 6,
            }}
            buttonStyle={{
              width: '100%',
              alignItems: 'center',
              justifyContent: 'center',
              paddingVertical: 12,
              paddingHorizontal: 32,
              borderRadius: 32,
              elevation: 1,
              backgroundColor: PRIMARY,
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
    flex: 0.95,
  },
  editButtonContainer: {
    alignSelf: 'center',
    paddingTop: 10,
  },
});
