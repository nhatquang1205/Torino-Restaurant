import React from 'react';
import { ICategoryModel } from '@/models/categories/category_detail';
import { useLocalSearchParams } from 'expo-router';
import { useGetApi } from '@/hooks/useGetApi';
import { API_URLS } from '@/constants/ApiUrls';
import CategoryForm from '@/components/categories/CategoryForm';

export default function CategoryDetailScreen() {
  const { id } = useLocalSearchParams();
  const { data, isLoading, refresh } = useGetApi<ICategoryModel>({
    url: `${API_URLS.CATEGORIES.GET_LIST}/${id}`,
    params: {},
    options: {},
    start: false,
  });
  React.useEffect(() => {
    refresh();
  }, []);
  React.useEffect(() => {
    setCategory(data);
  }, [data]);
  const [category, setCategory] = React.useState<ICategoryModel | null>(null);
  if (!category || isLoading) {
    return null;
  }
  return <CategoryForm isEdit category={category} />;
}
