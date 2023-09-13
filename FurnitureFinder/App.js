import { StatusBar } from 'expo-status-bar';
import { StyleSheet, Text, View } from 'react-native';
import { NavigationContainer } from '@react-navigation/native';
import { createStackNavigator } from '@react-navigation/stack';
import VibeInput from './pages/VibeInput';
import FeatureSelect from './pages/FeatureSelect';

const Stack = createStackNavigator();

export default function App() {
  return (
    <NavigationContainer>
      <Stack.Navigator initialRouteName="VibeInput">
        <Stack.Screen name="VibeInput" component={VibeInput} />
        <Stack.Screen name="FeatureSelect" component={FeatureSelect} />
        {/* features */}
      </Stack.Navigator>
    </NavigationContainer>
  );
};


const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#fff',
    alignItems: 'center',
    justifyContent: 'center',
  },
});
