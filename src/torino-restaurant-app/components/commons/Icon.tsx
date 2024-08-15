import { Image, View, StyleSheet } from 'react-native';

export interface IconProps {
  source: any;
  width: number;
  height: number;
}
export default function AppIcon({ source, width, height }: IconProps) {
  return (
    <View style={styles.container}>
      <Image source={source} style={{ width, height }} />
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    justifyContent: 'center',
    alignItems: 'center',
    paddingLeft: 8,
  },
});
