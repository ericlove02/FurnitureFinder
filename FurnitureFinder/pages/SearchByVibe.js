import React from 'react';
import { View, Text } from 'react-native';

const SearchByVibe = ({ navigation, route }) => {
    const vibe = route.params.vibe;

    return (
        <View>
            <Text>Search By Vibe</Text>
            <Text>Your Vibe: {vibe}</Text>
        </View>
    );
};

export default SearchByVibe;