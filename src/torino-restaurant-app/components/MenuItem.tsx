import { type TextProps, StyleSheet, Pressable } from 'react-native';

import { Icon } from '@rneui/base';
import { ThemedText } from './ThemedText';
import { Link } from 'expo-router';

export type ThemedTextProps = TextProps & {
  label: string;
  iconName: string;
  href: string;
};

export function MenuItem(props: ThemedTextProps) {
  const { label, iconName, href } = props;
  return (
    <Link href={href} asChild>
      <Pressable style={styles.default}>
        <Icon name={iconName}></Icon>
        <ThemedText>{label}</ThemedText>
      </Pressable>
    </Link>
  );
}

const styles = StyleSheet.create({
  default: {
    borderStyle: 'solid',
    borderColor: 'black',
    borderWidth: 0.5,
    borderRadius: 8,
    display: 'flex',
    flexDirection: 'column',
    padding: 4,
  },
});
