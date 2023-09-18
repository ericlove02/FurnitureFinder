import React, { useState } from 'react';
import { View, Text, TextInput, Button, StyleSheet } from 'react-native';
import { LinearGradient } from 'expo-linear-gradient';

const VibeInput = ({ navigation }) => {
    const [vibe, setVibe] = useState('');

    const handleVibeInput = (text) => {
        setVibe(text);
    };

    const handleContinue = () => {
        navigation.navigate('FeatureSelect', { vibe });
    };

    return (
        <View style={styles.container}>
            <LinearGradient
                colors={['#FFFFFF', '#E5E5E5']}
                style={styles.background}
            >
                <Text style={styles.header}>Furniture Finder</Text>
                <View style={styles.inputContainer}>
                    <TextInput
                        style={styles.input}
                        placeholder="Type in your vibe"
                        onChangeText={handleVibeInput}
                        value={vibe}
                    />
                    <Button
                        title="Get Started"
                        onPress={handleContinue}
                        color="white" // Set button text color to white
                    />
                </View>
            </LinearGradient>
        </View>
    );
};

const styles = StyleSheet.create({
    container: {
        flex: 1,
    },
    background: {
        flex: 1,
        alignItems: 'center',
        justifyContent: 'space-between',
        paddingVertical: 20,
    },
    header: {
        fontSize: 24,
        color: 'black',
        paddingTop: '20%',
    },
    inputContainer: {
        alignItems: 'center',
        paddingBottom: '25%',
    },
    input: {
        width: '90%',
        borderWidth: 1,
        borderColor: 'black',
        padding: 10,
        marginBottom: 10,
        borderRadius: 10, // Rounded input border
    },
});

export default VibeInput;
