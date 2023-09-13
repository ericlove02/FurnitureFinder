import React, { useState } from 'react';
import { View, TextInput, Button } from 'react-native';

const InitialInputScreen = ({ navigation }) => {
    const [vibe, setVibe] = useState('');

    const handleVibeInput = (text) => {
        setVibe(text);
    };

    const handleContinue = () => {
        navigation.navigate('FeatureSelect', { vibe });
    };

    return (
        <View>
            <TextInput
                placeholder="Enter Vibe"
                onChangeText={handleVibeInput}
                value={vibe}
            />
            <Button title="Continue" onPress={handleContinue} />
        </View>
    );
};

export default InitialInputScreen;