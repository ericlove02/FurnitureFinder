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

export default ARSearch;