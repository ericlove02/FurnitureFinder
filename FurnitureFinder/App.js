import { StatusBar } from 'expo-status-bar';
import { StyleSheet, Text, View } from 'react-native';
import { NavigationContainer } from '@react-navigation/native';
import { createStackNavigator } from '@react-navigation/stack';
import VibeInput from './pages/VibeInput';
import FeatureSelect from './pages/FeatureSelect';
import ARSearch from './pages/ARSearch';
import SearchByProduct from './pages/SearchByProduct';
import SearchByVibe from './pages/SearchByVibe';

const Stack = createStackNavigator();

export default function App() {
  return (
    <NavigationContainer>
      <Stack.Navigator initialRouteName="VibeInput">
        <Stack.Screen name="VibeInput" component={VibeInput} />
        <Stack.Screen name="FeatureSelect" component={FeatureSelect} />
        <Stack.Screen name="ARSearch" component={ARSearch} />
        <Stack.Screen name="SearchByProduct" component={SearchByProduct} />
        <Stack.Screen name="SearchByVibe" component={SearchByVibe} />
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
