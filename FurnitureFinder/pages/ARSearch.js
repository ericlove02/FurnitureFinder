import React, { useEffect } from 'react';
import { View, Text } from 'react-native';
import WebView from 'react-native-webview';
import * as Permissions from 'expo-permissions';

const ARSearch = ({ navigation, route }) => {
  const vibe = route.params.vibe;

  // Request camera permission
  useEffect(() => {
    (async () => {
      const { status } = await Permissions.askAsync(Permissions.CAMERA);
      if (status !== 'granted') {
        console.error('Camera permission not granted');
      }
    })();
  }, []);

  // AR.js HTML content to render in the WebView
  const arJsHtml = `
    <!DOCTYPE html>
    <html>
      <head>
        <script src="https://aframe.io/releases/1.2.0/aframe.min.js"></script>
        <script src="https://cdn.rawgit.com/jeromeetienne/AR.js/2.0.8/aframe/build/aframe-ar.js"></script>
      </head>
      <body style="margin: 0; overflow: hidden;">
        <a-scene embedded arjs="sourceType: webcam; debugUIEnabled: false;">
          <a-box position="0 0 -1" color="tomato"></a-box>
        </a-scene>
      </body>
    </html>
  `;

  return (
    <View style={{ flex: 1 }}>
      <Text>AR Search</Text>
      <Text>Your Vibe: {vibe}</Text>
      <WebView
        style={{ flex: 1 }}
        originWhitelist={['*']}
        source={{ html: arJsHtml }}
      />
    </View>
  );
};

export default ARSearch;