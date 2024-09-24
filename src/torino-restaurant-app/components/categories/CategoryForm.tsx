import { StyleSheet, Image, TextInput, Pressable } from 'react-native';

import { ThemedView } from '@/components/ThemedView';
import React from 'react';
import { ICategoryModel } from '@/models/categories/category_detail';
import RoundedButton from '@/components/commons/RoundedButton';
import * as ImagePicker from 'expo-image-picker';
import { usePostApi } from '@/hooks/usePostApi';
import { API_URLS } from '@/constants/ApiUrls';
import { router } from 'expo-router';
import { PRIMARY } from '@/constants/Colors';
import { ImagePickerResult } from 'expo-image-picker';
import Request from '@/repositories';
import { AxiosRequestHeaders } from 'axios';

export interface CategoryFormProps {
  isEdit: boolean;
  category?: ICategoryModel;
}
export default function CategoryForm(props: CategoryFormProps) {
  const { isEdit, category } = props;

  const { post, isLoading } = usePostApi<any>(
    isEdit && category
      ? `${API_URLS.CATEGORIES.CREATE}/${category.id}`
      : API_URLS.CATEGORIES.CREATE,
    {
      'Content-Type': 'multipart/form-data',
      Method: 'PUT',
    },
    isEdit
  );

  const handleOnSaveButton = async () => {
    const formData = new FormData();
    formData.append('name', name);
    formData.append('description', category?.description || name);
    if (
      isChangeImage &&
      imagePickerResult &&
      imagePickerResult.assets &&
      imagePickerResult.assets[0].base64
    ) {
      formData.append('base64Image', imagePickerResult?.assets[0].base64);
      formData.append(
        'imageName',
        imagePickerResult?.assets[0].fileName || 'category.png'
      );
      formData.append('isDeleteImage', 'true');
    }
    if (isEdit && category) {
      await Request.put(
        `${API_URLS.CATEGORIES.CREATE}/${category.id}`,
        formData,
        {},
        {
          'Content-Type': 'multipart/form-data',
          Method: 'PUT',
          Accept: '*/*',
        } as unknown as AxiosRequestHeaders
      );
    } else {
      await Request.post(API_URLS.CATEGORIES.CREATE, formData, {}, {
        'Content-Type': 'multipart/form-data',
        Method: 'POST',
      } as unknown as AxiosRequestHeaders);
    }
    router.push('/categories');
  };
  const pickImageAsync = async () => {
    let result = await ImagePicker.launchImageLibraryAsync({
      allowsEditing: true,
      quality: 1,
      base64: true,
    });

    if (!result.canceled) {
      setSelectedImage(result.assets[0].uri);
      setIsChangeImage(true);
      setImagePickerResult(result);
    } else {
    }
  };

  const [name, setName] = React.useState<string>(category?.name || '');
  const [selectedImage, setSelectedImage] = React.useState<string>(
    category?.imageUrl ||
      Image.resolveAssetSource(require('@/assets/images/placeholder-img.png'))
        .uri
  );
  const [imagePickerResult, setImagePickerResult] =
    React.useState<ImagePickerResult>();
  const [isChangeImage, setIsChangeImage] = React.useState<boolean>(false);
  return (
    <ThemedView style={styles.container}>
      <ThemedView style={styles.item}>
        <ThemedView>
          <Pressable onPress={pickImageAsync}>
            <Image src={selectedImage} style={styles.imageContainer} />
          </Pressable>
        </ThemedView>
        <ThemedView
          style={{
            flex: 1,
          }}
        >
          <TextInput
            style={{
              borderBottomWidth: 0.5,
              borderBottomColor: '#8A8A8A',
              flex: 1,
            }}
            placeholder="Tên danh mục"
            value={name}
            onChangeText={(e) => {
              setName(e);
            }}
          />
        </ThemedView>
      </ThemedView>
      <ThemedView style={styles.editButtonContainer}>
        <RoundedButton
          title="SAVE"
          onPress={handleOnSaveButton}
          isLoading={isLoading}
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
      </ThemedView>
    </ThemedView>
  );
}

const styles = StyleSheet.create({
  container: { display: 'flex', flexDirection: 'column', flex: 1 },
  item: {
    display: 'flex',
    flexDirection: 'row',
    gap: 16,
    padding: 16,
    height: 'auto',
  },
  imageContainer: {
    height: 50,
    width: 50,
    display: 'flex',
    justifyContent: 'center',
  },
  editButtonContainer: {
    position: 'absolute',
    bottom: 20,
    alignSelf: 'center',
  },
});
