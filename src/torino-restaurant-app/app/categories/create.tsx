import React from 'react';
import CategoryForm from '@/components/categories/CategoryForm';
import { API_URLS } from '@/constants/ApiUrls';

export default function CategoryDetailScreen() {
  const url = API_URLS.CATEGORIES.GET_LIST;
  return <CategoryForm isEdit={false} />;
}
