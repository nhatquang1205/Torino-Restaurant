import { StyleSheet, Image, TextInput, Pressable } from 'react-native';

import { ThemedView } from '@/components/ThemedView';
import React from 'react';
import { ICategoryModel } from '@/models/categories/category_detail';
import RoundedButton from '@/components/commons/RoundedButton';
import * as ImagePicker from 'expo-image-picker';

export interface CategoryFormProps {
  isEdit: boolean;
  category?: ICategoryModel;
}
export default function CategoryForm(props: CategoryFormProps) {
  const pickImageAsync = async () => {
    let result = await ImagePicker.launchImageLibraryAsync({
      allowsEditing: true,
      quality: 1,
    });

    if (!result.canceled) {
      setSelectedImage(result.assets[0].uri);
      console.log(result);
    } else {
      alert('You did not select any image.');
    }
  };

  const { isEdit, category } = props;
  const [name, setName] = React.useState<string>(category?.name || '');
  const [selectedImage, setSelectedImage] = React.useState<string>(
    category?.imageUrl || '@/assets/images/icon.png'
  );
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
        <RoundedButton title="SAVE" onPress={() => {}} />
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
    bottom: 0,
    alignSelf: 'center',
  },
});
