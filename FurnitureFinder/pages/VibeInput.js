import React, { useState } from 'react';
import { View, TextInput, Button, Image } from 'react-native';

const VibeInput = ({ navigation }) => {
    const [vibe, setVibe] = useState('');

    const handleVibeInput = (text) => {
        setVibe(text);
    };

    const handleContinue = () => {
        navigation.navigate('FeatureSelect', { vibe });
    };

    return (
        <View>
            <Image source={require("../assets/front-splash1.jpg")} style={{ width: 100, height: 100 }} />
            <TextInput
                placeholder="Enter Vibe"
                onChangeText={handleVibeInput}
                value={vibe}
            />
            <Button title="Enter" onPress={handleContinue} />
        </View>
    );
};

export default VibeInput;