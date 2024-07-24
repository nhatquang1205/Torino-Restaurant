import { Image, StyleSheet, Platform, Button } from "react-native";

import ParallaxScrollView from "@/components/ParallaxScrollView";
import { ThemedView } from "@/components/ThemedView";
import { useEffect } from "react";
import { setIsAuthenticate } from "@/store/app/app-slice";
import { useDispatch, useSelector } from "react-redux";
import { router } from "expo-router";

export default function LoginScreen() {
  const dispatch = useDispatch();
  const { isAuthenticated } = useSelector((state: any) => state.app);
  useEffect(() => {
    if (isAuthenticated) {
      router.replace("/");
    }
  }, [isAuthenticated]);
  return (
    <ParallaxScrollView
      headerBackgroundColor={{ light: "#A1CEDC", dark: "#1D3D47" }}
      headerImage={
        <Image
          source={require("@/assets/images/partial-react-logo.png")}
          style={styles.reactLogo}
        />
      }
    >
      <ThemedView>
        <Button
          title="Đăng nhập"
          onPress={() => dispatch(setIsAuthenticate())}
        />
      </ThemedView>
    </ParallaxScrollView>
  );
}

const styles = StyleSheet.create({
  titleContainer: {
    flexDirection: "row",
    alignItems: "center",
    gap: 8,
  },
  stepContainer: {
    gap: 8,
    marginBottom: 8,
  },
  reactLogo: {
    height: 178,
    width: 290,
    bottom: 0,
    left: 0,
    position: "absolute",
  },
});
