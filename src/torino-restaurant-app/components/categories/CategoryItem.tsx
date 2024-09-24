import { StyleSheet, Image, Pressable, Alert } from 'react-native';
import { ThemedText } from '@/components/ThemedText';
import { ThemedView } from '@/components/ThemedView';
import { ICategoryModel } from '@/models/categories/category_detail';
import { AntDesign } from '@expo/vector-icons';
import { Href, Link } from 'expo-router';
import Request from '@/repositories';

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
    Alert.alert('Warning!', 'Are you sure want to delete this category?', [
      {
        text: 'Cancel',
        style: 'cancel',
      },
      { text: 'OK', onPress: () => deleteCategory() },
    ]);
  };
  return (
    <ThemedView style={styles.item}>
      <ThemedView>
        {!isInEditMode ? (
          <Image
            src={
              category?.imageUrl ||
              Image.resolveAssetSource(
                require('@/assets/images/placeholder-img.png')
              ).uri
            }
            style={styles.imageContainer}
          />
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
        href={`categories/${category.id}` as Href<`categories/${number}`>}
        style={{
          borderBottomWidth: 0.5,
          borderBottomColor: '#8A8A8A',
          flex: 1,
        }}
      >
        <Pressable>
          <ThemedView>
            <ThemedText>{category.name}</ThemedText>
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
