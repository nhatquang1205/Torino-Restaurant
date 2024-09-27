import { StyleSheet, Image, Pressable, Alert } from 'react-native';
import { ThemedText } from '@/components/ThemedText';
import { ThemedView } from '@/components/ThemedView';
import { ICategoryModel } from '@/models/categories/category_detail';
import { AntDesign } from '@expo/vector-icons';
import { Href, Link, router } from 'expo-router';
import Request from '@/repositories';
import { useRoute } from '@react-navigation/native';

export function CategoryItem({
  category,
  isInEditMode,
  refreshCategories,
}: {
  category: ICategoryModel;
  isInEditMode: boolean;
  refreshCategories: () => Promise<void>;
}) {
  const deleteCategory = async () => {
    await Request.delete(`/categories/${category.id}`);
    await refreshCategories();
  };
  const handlePressDeleteButton = () => {
    Alert.alert('Cảnh báo!', 'Bạn có đồng ý xoá danh mục này?', [
      {
        text: 'Huỷ bỏ',
        style: 'cancel',
      },
      { text: 'OK', onPress: () => deleteCategory() },
    ]);
  };
  return (
    <ThemedView style={styles.item}>
      <ThemedView>
        {!isInEditMode ? (
          <Link
            asChild
            push
            href={{
              pathname: isInEditMode
                ? '/categories/[id]'
                : '/categories/[id]/products',
              params: { id: category.id, name: category.name },
            }}
          >
            <Pressable>
              <ThemedView>
                <Image
                  src={
                    category?.imageUrl ||
                    Image.resolveAssetSource(
                      require('@/assets/images/placeholder-img.png')
                    ).uri
                  }
                  style={styles.imageContainer}
                />
              </ThemedView>
            </Pressable>
          </Link>
        ) : (
          <ThemedView style={styles.imageContainer}>
            <Pressable onPress={handlePressDeleteButton}>
              <AntDesign name="minuscircleo" size={30} color="red" />
            </Pressable>
          </ThemedView>
        )}
      </ThemedView>
      <Link
        asChild
        push
        href={{
          pathname: isInEditMode
            ? '/categories/[id]'
            : '/categories/[id]/products',
          params: { id: category.id, name: category.name },
        }}
        style={{
          borderBottomWidth: 0.5,
          borderBottomColor: '#8A8A8A',
          flex: 1,
          marginTop: 12,
        }}
      >
        <Pressable>
          <ThemedView>
            <ThemedText style={{ fontSize: 18 }}>{category.name}</ThemedText>
          </ThemedView>
        </Pressable>
      </Link>
    </ThemedView>
  );
}

const styles = StyleSheet.create({
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
});
