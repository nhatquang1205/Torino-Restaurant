import React from 'react';
import { Text, StyleSheet, Pressable } from 'react-native';

export interface ButtonProps {
  onPress: any;
  title: string;
  buttonStyle?: any;
}
export default function RoundedButton(props: ButtonProps) {
  const { onPress, title = 'Save', buttonStyle } = props;
  return (
    <Pressable style={{ ...styles.button, ...buttonStyle }} onPress={onPress}>
      <Text style={styles.text}>{title}</Text>
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
    backgroundColor: 'rgba(138, 138, 138, 0.13)',
  },
  text: {
    fontSize: 30,
    fontFamily: 'LoveYaLikeASister',
    lineHeight: 30,
    fontWeight: 'heavy',
    letterSpacing: 0.25,
    color: 'black',
    opacity: 1,
  },
});
