import React from 'react';
import { View, Text } from 'react-native';

const ARSearch = ({ navigation, route }) => {
  const vibe = route.params.vibe;

  return (
    <View>
      <Text>AR Search</Text>
      <Text>Your Vibe: {vibe}</Text>
    </View>
  );
};

var styles = StyleSheet.create({
  f1: { flex: 1 },
  arSearchText: {
    fontFamily: 'Arial',
    fontSize: 30,
    color: '#ffffff',
    textAlignVertical: 'center',
    textAlign: 'center',
  },
});