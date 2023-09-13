import React from 'react';
import { View, Text } from 'react-native';

const SearchByProduct = ({ navigation, route }) => {
    const vibe = route.params.vibe;

    return (
        <View>
            <Text>Search By Product</Text>
            <Text>Your Vibe: {vibe}</Text>
        </View>
    );
};

export default SearchByProduct;