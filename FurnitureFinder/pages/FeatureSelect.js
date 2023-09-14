import React from 'react';
import { View, Text, Button } from 'react-native';

const FeatureSelect = ({ navigation, route }) => {
    const vibe = route.params.vibe;

    return (
        <View>
            <Text>Welcome to Furniture Finder</Text>
            <Text>Your Vibe: {vibe}</Text>
            <Button title="AR Search" onPress={() => navigation.navigate('ARSearch', { vibe: vibe })} />
            <Button title="Search By Product" onPress={() => navigation.navigate('SearchByProduct', { vibe: vibe })} />
            <Button title="Search By Vibe" onPress={() => navigation.navigate('SearchByVibe', { vibe: vibe })} />
        </View>
    );
};

export default FeatureSelect;