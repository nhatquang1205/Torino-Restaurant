import React from 'react';
import { Href, Link, router, Stack, useLocalSearchParams } from 'expo-router';
import { useGetApi } from '@/hooks/useGetApi';
import { API_URLS } from '@/constants/ApiUrls';
import { IProductModel } from '@/models/products/product_detail';
import { IListResultTemplate } from '@/models/postResult';
import { ThemedView } from '@/components/ThemedView';
import { StyleSheet, FlatList } from 'react-native';
import RoundedButton from '@/components/commons/RoundedButton';
import { PRIMARY } from '@/constants/Colors';
import { ProductItem } from '@/components/products/ProductItem';
import { ThemedText } from '@/components/ThemedText';

export default function ProductsByCategoryScreen() {
  const { id, name } = useLocalSearchParams();
  const { data, isLoading, refresh } = useGetApi<
    IListResultTemplate<IProductModel>
  >({
    url: API_URLS.PRODUCTS.GET_LIST,
    params: { categoryId: id },
    options: {},
    start: false,
  });
  React.useEffect(() => {
    refresh();
  }, []);

  const renderProduct = (item: IProductModel) => {
    return (
      <ProductItem
        key={`product-${item.id}`}
        product={item}
        refreshProducts={refresh}
      />
    );
  };

  if (isLoading) {
    return null;
  }

  return (
    <ThemedView style={{ flex: 1, display: 'flex', flexDirection: 'column' }}>
      <ThemedView style={styles.menuContainer}>
        <Stack.Screen options={{ title: (name as string) ?? 'products' }} />
        {!(data?.items && data.items.length > 0) ? (
          <ThemedView>
            <ThemedText>No Item</ThemedText>
          </ThemedView>
        ) : (
          <FlatList
            data={data.items}
            renderItem={({ item }) => renderProduct(item)}
          />
        )}
      </ThemedView>
      <ThemedView style={styles.editButtonContainer}>
        <Link
          asChild
          href={{
            pathname: '/categories/[id]/products/create',
            params: { id: id as string, categoryName: name },
          }}
        >
          <RoundedButton
            title="Thêm sản phẩm"
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
            onPress={undefined}
          />
        </Link>
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
