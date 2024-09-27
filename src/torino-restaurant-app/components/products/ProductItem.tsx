import { StyleSheet, Image, Pressable, Alert } from 'react-native';
import { ThemedText } from '@/components/ThemedText';
import { ThemedView } from '@/components/ThemedView';
import { IProductModel } from '@/models/products/product_detail';
import { Href, Link } from 'expo-router';
import Request from '@/repositories';
import { thoundsandSeperator } from '@/utils/helper';

export function ProductItem({
  product,
  refreshProducts,
}: {
  product: IProductModel;
  refreshProducts: () => Promise<void>;
}) {
  const deleteProduct = async () => {
    await Request.delete(`/products/${product.id}`);
    await refreshProducts();
  };
  const handlePressDeleteButton = () => {
    Alert.alert('Warning!', 'Are you sure want to delete this product?', [
      {
        text: 'Cancel',
        style: 'cancel',
      },
      { text: 'OK', onPress: () => deleteProduct() },
    ]);
  };
  return (
    <ThemedView style={styles.item}>
      <ThemedView>
        <Image
          src={
            product?.imageUrl ||
            Image.resolveAssetSource(
              require('@/assets/images/placeholder-img.png')
            ).uri
          }
          style={styles.imageContainer}
        />
      </ThemedView>
      <Link
        asChild
        push
        href={`products/${product.id}` as Href<`products/${number}`>}
        style={{
          borderBottomWidth: 0.5,
          borderBottomColor: '#8A8A8A',
          flex: 1,
        }}
      >
        <Pressable>
          <ThemedView
            style={{
              display: 'flex',
              flexDirection: 'row',
              justifyContent: 'space-between',
              marginTop: 8,
            }}
          >
            <ThemedText>{product.name}</ThemedText>
            <ThemedText>{thoundsandSeperator(product.price)}</ThemedText>
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
