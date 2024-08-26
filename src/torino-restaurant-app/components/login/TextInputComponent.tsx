import { StyleSheet, TextInput } from 'react-native';

import { Icon, InputProps } from '@rneui/base';
import { ThemedView } from '../ThemedView';
import { PRIMARY } from '@/constants/Colors';

export function LoginTextInput(props: InputProps) {
  const { value, onChangeText, placeholder, children, ...rest } = props;
  return (
    <ThemedView style={styles.default}>
      {children}
      <TextInput
        style={styles.input}
        placeholder={placeholder}
        value={value}
        maxLength={32}
        onChangeText={onChangeText}
        {...rest}
      />
    </ThemedView>
  );
}

const styles = StyleSheet.create({
  default: {
    backgroundColor: PRIMARY,
    borderRadius: 32,
    display: 'flex',
    flexDirection: 'row',
    padding: 8,
    paddingTop: 16,
    paddingBottom: 16,
    gap: 16,
  },
  input: {
    fontSize: 20,
    flex: 1,
    fontFamily: 'SpaceMono',
  },
  searchIcon: {
    padding: 10,
  },
});
