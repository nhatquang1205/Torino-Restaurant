import {
  Text,
  StyleSheet,
  View,
  PanResponder,
  TouchableOpacity,
  Animated,
  TextInput,
} from 'react-native';

import { useRef } from 'react';
import { IProductPriceModel } from '@/models/products/product_detail';
import { ThemedView } from '../ThemedView';
import { thoundsandSeperator } from '@/utils/helper';

interface ProductPriceItemProps {
  item: IProductPriceModel;
  index: number;
  onDelete: (index: number) => void;
  updateProductPrices: (index: number, item: IProductPriceModel) => void;
}
export default function ProductPriceItem({
  item,
  onDelete,
  index,
  updateProductPrices,
}: ProductPriceItemProps) {
  const translateX = useRef(new Animated.Value(0)).current;
  const panResponder = useRef(
    PanResponder.create({
      onStartShouldSetPanResponder: () => true,
      onMoveShouldSetPanResponder: () => true,
      onPanResponderMove: (_, gestureState) => {
        if (gestureState.dx < 0) {
          translateX.setValue(gestureState.dx);
        }
      },
      onPanResponderRelease: (_, gestureState) => {
        if (gestureState.dx < -50) {
          Animated.spring(translateX, {
            toValue: -120,
            useNativeDriver: true,
          }).start();
        } else {
          Animated.spring(translateX, {
            toValue: 0,
            useNativeDriver: true,
          }).start();
        }
      },
    })
  ).current;

  return (
    <View style={styles.itemContainer}>
      <Animated.View
        style={{
          flex: 1,
          transform: [{ translateX: translateX }],
        }}
      >
        <View style={styles.item} {...panResponder.panHandlers}>
          <ThemedView style={styles.inputPriceContainer}>
            <TextInput
              style={{
                ...styles.input,
                width: '60%',
                borderRightWidth: 0.5,
              }}
              placeholder="Nhập tên giá"
              value={item.name}
              onChangeText={(e) =>
                updateProductPrices(index, {
                  name: e,
                  price: item.price,
                  display_price: item.display_price,
                })
              }
            />
            <TextInput
              style={{
                ...styles.input,
                textAlign: 'right',
              }}
              placeholder="Giá"
              value={item.display_price}
              onChangeText={(e) => {
                e = e.replaceAll(',', '').trim();
                const displayPrice = thoundsandSeperator(e);
                updateProductPrices(index, {
                  name: item.name,
                  price: parseFloat(e),
                  display_price: displayPrice ?? e,
                });
              }}
              keyboardType="numeric"
            />
          </ThemedView>
        </View>
        <TouchableOpacity
          style={styles.deleteButton}
          onPress={() => onDelete(index)}
        >
          <Text style={styles.deleteButtonText}>Xoá</Text>
        </TouchableOpacity>
      </Animated.View>
    </View>
  );
}

const styles = StyleSheet.create({
  item: {
    flex: 1,
    borderBottomWidth: 1,
    borderBottomColor: '#ccc',
  },
  itemContainer: {
    flexDirection: 'row',
  },
  deleteButton: {
    width: 100,
    height: '100%',
    backgroundColor: 'red',
    justifyContent: 'center',
    alignItems: 'center',
    position: 'absolute',
    right: -120,
  },
  deleteButtonText: {
    color: 'white',
    fontWeight: 'bold',
  },
  inputPriceContainer: {
    padding: 8,
    display: 'flex',
    flexDirection: 'row',
    gap: 8,
    fontSize: 24,
    justifyContent: 'space-between',
  },
  input: {
    fontSize: 24,
  },
});
