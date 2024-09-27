import React from 'react';
import { Text, StyleSheet, Pressable } from 'react-native';

export interface ButtonProps {
  onPress: any;
  title: string;
  buttonStyle?: any;
  textStyle?: any;
  isLoading?: boolean;
}
export default function RoundedButton(props: ButtonProps) {
  const { onPress, title = 'Save', buttonStyle, textStyle, isLoading } = props;
  return (
    <Pressable style={{ ...styles.button, ...buttonStyle }} onPress={onPress}>
      {isLoading ? (
        <Text style={{ ...textStyle }}>Đang tải...</Text>
      ) : (
        <Text style={{ ...textStyle }}>{title}</Text>
      )}
    </Pressable>
  );
}

const styles = StyleSheet.create({
  button: {
    width: '100%',
    alignItems: 'center',
    justifyContent: 'center',
    paddingVertical: 12,
    paddingHorizontal: 32,
    borderRadius: 32,
    elevation: 3,
  },
});
