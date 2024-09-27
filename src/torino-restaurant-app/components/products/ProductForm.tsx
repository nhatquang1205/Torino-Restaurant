import {
  StyleSheet,
  Image,
  TextInput,
  Pressable,
  Alert,
  Switch,
  Platform,
  PermissionsAndroid,
} from 'react-native';

import { ThemedView } from '@/components/ThemedView';
import React from 'react';
import * as ImagePicker from 'expo-image-picker';
import { API_URLS } from '@/constants/ApiUrls';
import { router, Stack } from 'expo-router';
import { ImagePickerResult } from 'expo-image-picker';
import Request from '@/repositories';
import { AxiosRequestHeaders } from 'axios';
import { v4 as uuidv4 } from 'uuid';
import {
  IProductModel,
  IProductPriceModel,
} from '@/models/products/product_detail';
import { ThemedText } from '../ThemedText';
import { Octicons } from '@expo/vector-icons';
import Animated, { useAnimatedRef } from 'react-native-reanimated';
import { thoundsandSeperator } from '@/utils/helper';
import ProductPriceItem from './ProductPriceItem';

export interface ProductFormProps {
  isEdit: boolean;
  category: any;
  product?: IProductModel;
}
export default function ProductForm(props: ProductFormProps) {
  const { isEdit, category, product } = props;

  const handleOnSaveButton = async () => {
    const formData = new FormData();
    formData.append('name', name);
    formData.append('description', description);
    formData.append('vietnameseDescription', vietnameseDescription);
    formData.append('categoryId', category.id.toString());
    formData.append('costPrice', costPrice.replaceAll(',', ''));
    productPrices.forEach((productPrice, index) => {
      formData.append(`productPrices[${index}].name`, productPrice.name);
      formData.append(
        `productPrices[${index}].price`,
        productPrice.price.toString()
      );
      if (productPrice.id) {
        formData.append(
          `productPrices[${index}].id`,
          productPrice.id.toString()
        );
      }
    });
    formData.append('isUseForPrinter', isUseForPrinter.toString());

    const uuid = uuidv4();
    if (
      isChangeImage &&
      imagePickerResult &&
      imagePickerResult.assets &&
      imagePickerResult.assets[0].base64
    ) {
      formData.append('base64Image', imagePickerResult?.assets[0].base64);
      formData.append(
        'imageName',
        imagePickerResult?.assets[0].fileName || uuid
      );
      formData.append('isDeleteImage', 'true');
    }

    if (deletedProductPrices.length > 0) {
      deletedProductPrices.forEach((id) => {
        formData.append('deletedProductPrices', id.toString());
      });
    }
    if (isEdit && product) {
      await Request.put(
        `${API_URLS.PRODUCTS.GET_LIST}/${product.id}`,
        formData,
        {},
        {
          'Content-Type': 'multipart/form-data',
          Method: 'PUT',
          Accept: '*/*',
        } as unknown as AxiosRequestHeaders
      );
    } else {
      await Request.post(API_URLS.PRODUCTS.GET_LIST, formData, {}, {
        'Content-Type': 'multipart/form-data',
        Method: 'POST',
      } as unknown as AxiosRequestHeaders);
    }
    Alert.alert('Thành công', 'Lưu sản phẩm thành công!', [
      {
        text: 'OK',
        onPress: function () {
          router.back();
        },
      },
    ]);
  };
  const pickImageAsync = async () => {
    const granted = await PermissionsAndroid.request(
      PermissionsAndroid.PERMISSIONS.CAMERA,
      {
        title: 'Torino.Boss App Camera Permission',
        message: 'Torino.Boss App needs access to your camera ',
        buttonNeutral: 'Ask Me Later',
        buttonNegative: 'Cancel',
        buttonPositive: 'OK',
      }
    );
    if (
      Platform.OS === 'android' &&
      granted !== PermissionsAndroid.RESULTS.GRANTED
    ) {
      return;
    }
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

  const [name, setName] = React.useState<string>(product?.name || '');
  const [selectedImage, setSelectedImage] = React.useState<string>(
    product?.imageUrl ||
      Image.resolveAssetSource(require('@/assets/images/placeholder-img.png'))
        .uri
  );
  const [costPrice, setCostPrice] = React.useState<string>(
    thoundsandSeperator(product?.costPrice) || ''
  );
  const [productPrices, setProductPrices] = React.useState<
    IProductPriceModel[]
  >(
    product?.productPrices || [
      {
        name: 'Giá mặc định',
        price: 0,
        display_price: '0',
      },
    ]
  );
  const [isUseForPrinter, setIsUseForPrinter] = React.useState<boolean>(
    product?.isUseForPrinter || false
  );
  const [description, setDescription] = React.useState<string>(
    product?.description || ''
  );
  const [vietnameseDescription, setVietnameseDescription] =
    React.useState<string>(product?.vietnameseDescription || '');
  const [imagePickerResult, setImagePickerResult] =
    React.useState<ImagePickerResult>();
  const [isChangeImage, setIsChangeImage] = React.useState<boolean>(false);
  const [deletedProductPrices, setDeletedProductPrices] = React.useState<
    number[]
  >([]);

  const updateProductPrices = (
    index: number,
    productPrice: IProductPriceModel
  ) => {
    const newProductPrices = [...productPrices];
    newProductPrices[index] = productPrice;
    setProductPrices(newProductPrices);
  };

  const handleAddNewProductPrice = () => {
    const newProductPrices = [...productPrices];
    newProductPrices.push({
      name: '',
      price: 0,
      display_price: '0',
    });
    setProductPrices(newProductPrices);
  };

  const handleDeleteProductPrice = (index: number) => {
    if (productPrices[index].id) {
      setDeletedProductPrices([
        ...deletedProductPrices,
        productPrices[index].id,
      ]);
    }
    const newProductPrices = [...productPrices];
    newProductPrices.splice(index, 1);
    setProductPrices(newProductPrices);
  };

  const renderSaveButton = function () {
    return (
      <Pressable onPress={() => handleOnSaveButton()}>
        <ThemedText style={{ fontSize: 24, color: 'green' }}>Lưu</ThemedText>
      </Pressable>
    );
  };

  const scrollRef = useAnimatedRef<Animated.ScrollView>();
  return (
    <ThemedView>
      <Stack.Screen
        options={{
          headerRight: renderSaveButton,
        }}
      />
      <Animated.ScrollView ref={scrollRef} scrollEventThrottle={16}>
        <ThemedView style={styles.container}>
          <ThemedView style={styles.imagePickerContainer}>
            <Pressable onPress={pickImageAsync}>
              <Image src={selectedImage} style={styles.imageContainer} />
            </Pressable>
          </ThemedView>
          <ThemedView style={styles.informationContainer}>
            <ThemedView style={styles.inputContainer}>
              <TextInput
                style={{
                  ...styles.input,
                }}
                placeholder="Tên"
                value={name}
                onChangeText={(e) => {
                  setName(e);
                }}
              />
            </ThemedView>
            <ThemedView style={{ borderWidth: 0.5 }} />
            <ThemedView style={styles.inputContainer}>
              <TextInput
                style={styles.input}
                placeholder="Danh mục"
                value={category?.name}
                readOnly
              />
            </ThemedView>
          </ThemedView>
          <ThemedView style={styles.informationContainer}>
            <ThemedView style={styles.inputPriceContainer}>
              <ThemedText
                style={{
                  fontSize: 24,
                  width: '60%',
                  textAlignVertical: 'center',
                  borderRightWidth: 0.5,
                }}
              >
                Giá cost
              </ThemedText>
              <TextInput
                style={{
                  ...styles.input,
                  textAlign: 'right',
                }}
                value={costPrice}
                onChangeText={(e) => {
                  e = e.replaceAll(',', '').trim();
                  const costPrice = thoundsandSeperator(e);
                  setCostPrice(costPrice ?? e);
                }}
                keyboardType="numeric"
              />
            </ThemedView>
            <ThemedView style={{ borderWidth: 0.5 }} />
            {productPrices.map((productPrice, index) => (
              <>
                <ProductPriceItem
                  item={productPrice}
                  index={index}
                  onDelete={handleDeleteProductPrice}
                  updateProductPrices={updateProductPrices}
                />
                <ThemedView style={{ borderWidth: 0.5 }} />
              </>
            ))}
            <ThemedView style={styles.inputContainer}>
              <Pressable
                style={styles.buttonAddPrice}
                onPress={handleAddNewProductPrice}
              >
                <ThemedText style={{ fontSize: 24 }}>Thêm Giá</ThemedText>
                <Octicons name="arrow-right" size={24} color="black" />
              </Pressable>
            </ThemedView>
          </ThemedView>
          <ThemedView style={styles.informationContainer}>
            <ThemedView style={styles.inputPriceContainer}>
              <ThemedText style={{ fontSize: 24, verticalAlign: 'middle' }}>
                In khi tạo hoá đơn
              </ThemedText>
              <Switch
                trackColor={{ false: '#767577', true: '#81b0ff' }}
                thumbColor={isUseForPrinter ? '#f5dd4b' : '#f4f3f4'}
                ios_backgroundColor="#3e3e3e"
                onValueChange={() => {
                  setIsUseForPrinter(!isUseForPrinter);
                }}
                value={isUseForPrinter}
              />
            </ThemedView>
          </ThemedView>
          <ThemedView style={styles.informationContainer}>
            <ThemedView style={styles.inputContainer}>
              <TextInput
                style={{
                  ...styles.input,
                }}
                placeholder="Mô tả Tiếng Anh"
                value={description}
                onChangeText={setDescription}
                numberOfLines={4}
                multiline
              />
            </ThemedView>
          </ThemedView>
          <ThemedView style={styles.informationContainer}>
            <ThemedView style={styles.inputContainer}>
              <TextInput
                style={{
                  ...styles.input,
                }}
                placeholder="Mô tả Tiếng Việt"
                value={vietnameseDescription}
                onChangeText={setVietnameseDescription}
                numberOfLines={4}
                multiline
              />
            </ThemedView>
          </ThemedView>
        </ThemedView>
      </Animated.ScrollView>
    </ThemedView>
  );
}

const styles = StyleSheet.create({
  container: {
    display: 'flex',
    flexDirection: 'column',
    flex: 1,
    padding: 16,
    rowGap: 16,
  },
  imagePickerContainer: {
    display: 'flex',
    flexDirection: 'row',
    justifyContent: 'center',
  },
  imageContainer: {
    height: 100,
    width: 100,
    display: 'flex',
    justifyContent: 'center',
  },
  informationContainer: {
    display: 'flex',
    flexDirection: 'column',
    justifyContent: 'center',
    height: 'auto',
    borderWidth: 2,
    borderColor: '#8A8A8A',
    borderRadius: 8,
  },
  editButtonContainer: {
    // position: 'absolute',
    // bottom: 20,
    position: 'relative',
    display: 'flex',
    alignSelf: 'center',
  },
  inputContainer: { padding: 8 },
  inputPriceContainer: {
    padding: 8,
    display: 'flex',
    flexDirection: 'row',
    gap: 8,
    fontSize: 24,
    justifyContent: 'space-between',
  },
  buttonAddPrice: {
    paddingTop: 8,
    display: 'flex',
    flexDirection: 'row',
    justifyContent: 'space-between',
  },
  input: {
    fontSize: 24,
  },
});
