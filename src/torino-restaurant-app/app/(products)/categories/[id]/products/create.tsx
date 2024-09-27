import React from 'react';
import ProductForm from '@/components/products/ProductForm';
import { useLocalSearchParams } from 'expo-router';

export default function ProductDetailScreen() {
  const { id, categoryName }: { id: string; categoryName: string } =
    useLocalSearchParams();
  return (
    <ProductForm
      isEdit={false}
      category={{
        id: Number(id),
        name: categoryName,
        description: '',
        imageUrl: '',
      }}
    />
  );
}
