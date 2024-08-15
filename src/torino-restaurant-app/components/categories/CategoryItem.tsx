import { StyleSheet, Image, Pressable } from 'react-native';
import { ThemedText } from '@/components/ThemedText';
import { ThemedView } from '@/components/ThemedView';
import { ICategoryModel } from '@/models/categories/category_detail';
import { AntDesign } from '@expo/vector-icons';
import { Link } from 'expo-router';

export function CategoryItem({
  category,
  isInEditMode,
}: {
  category: ICategoryModel;
  isInEditMode: boolean;
}) {
  return (
    <ThemedView style={styles.item}>
      <ThemedView>
        {!isInEditMode ? (
          <Image src={category.imageUrl} style={styles.imageContainer} />
        ) : (
          <ThemedView style={styles.imageContainer}>
            <AntDesign name="minuscircleo" size={30} color="red" />
          </ThemedView>
        )}
      </ThemedView>
      <Link
        asChild
        push
        href={`categories/${category.id}`}
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
