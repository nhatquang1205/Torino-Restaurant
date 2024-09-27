import React from 'react';
import { IProductModel } from '@/models/products/product_detail';
import { useLocalSearchParams } from 'expo-router';
import { useGetApi } from '@/hooks/useGetApi';
import { API_URLS } from '@/constants/ApiUrls';
import ProductForm from '@/components/products/ProductForm';
import { thoundsandSeperator } from '@/utils/helper';

export default function ProductDetailScreen() {
  const { id } = useLocalSearchParams();
  const { data, isLoading, refresh } = useGetApi<IProductModel>({
    url: `${API_URLS.PRODUCTS.GET_LIST}/${id}`,
    params: {},
    options: {},
    start: false,
  });
  React.useEffect(() => {
    refresh();
  }, []);
  React.useEffect(() => {
    if (data) {
      data.productPrices.map((item) => {
        item.display_price = thoundsandSeperator(item.price) || '';
        return item;
      });

      setProduct(data);
    }
  }, [data]);
  const [product, setProduct] = React.useState<IProductModel | null>(null);
  if (!product || isLoading) {
    return null;
  }
  return (
    <ProductForm
      isEdit
      product={product}
      category={{ id: product.categoryId, name: product.category }}
    />
  );
}
