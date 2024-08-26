import React from 'react';
import { Text, StyleSheet, Pressable } from 'react-native';

export interface ButtonProps {
  onPress: any;
  title: string;
  buttonStyle?: any;
  isLoading?: boolean;
}
export default function RoundedButton(props: ButtonProps) {
  const { onPress, title = 'Save', buttonStyle, isLoading } = props;
  return (
    <Pressable style={{ ...styles.button, ...buttonStyle }} onPress={onPress}>
      {isLoading ? (
        <Text style={styles.text}>Loading...</Text>
      ) : (
        <Text style={styles.text}>{title}</Text>
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
  text: {
    fontSize: 30,
    fontFamily: 'LoveYaLikeASister',
    lineHeight: 30,
    fontWeight: 'heavy',
    letterSpacing: 0.25,
    color: 'black',
    opacity: 1,
    paddingTop: 10,
  },
});
