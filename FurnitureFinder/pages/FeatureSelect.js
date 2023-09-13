import React from 'react';
import { View, Text, TouchableOpacity } from 'react-native';

const FeatureSelect = ({ navigation, route }) => {
    const vibe = route.params.vibe;

    return (
        <View>
            <Text>Welcome to Furniture Finder</Text>
            <Text>Your Vibe: {vibe}</Text>
            <TouchableOpacity onPress={() => navigation.navigate('Feature1')}>
                <Text>Feature 1</Text>
            </TouchableOpacity>
            <TouchableOpacity onPress={() => navigation.navigate('Feature2')}>
                <Text>Feature 2</Text>
            </TouchableOpacity>
        </View>
    );
};

export default FeatureSelect;