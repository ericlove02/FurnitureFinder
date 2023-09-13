import React from 'react';
import { View, Text, TouchableOpacity } from 'react-native';

const FeatureSelect = ({ navigation, route }) => {
    const vibe = route.params.vibe;

    return (
        <View>
            <Text>Welcome to Furniture Finder</Text>
            <Text>Your Vibe: {vibe}</Text>
            <TouchableOpacity onPress={() => navigation.navigate('ARSearch', { vibe: vibe })}>
                <Text>ARSearch</Text>
            </TouchableOpacity>
            <TouchableOpacity onPress={() => navigation.navigate('SearchByProduct', { vibe: vibe })}>
                <Text>Search By Product</Text>
            </TouchableOpacity>
            <TouchableOpacity onPress={() => navigation.navigate('SearchByVibe', { vibe: vibe })}>
                <Text>Search By Vibe</Text>
            </TouchableOpacity>
        </View>
    );
};

export default FeatureSelect;