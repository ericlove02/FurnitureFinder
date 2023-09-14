import React, { useEffect } from 'react';
import { View, Text } from 'react-native';
import {
  ViroARScene,
  ViroText,
  ViroConstants,
  ViroARSceneNavigator,
  Viro3DObject,
  ViroAmbientLight,
  ViroSpotLight,
  ViroARPlaneSelector,
  ViroNode,
  ViroQuad,
  ViroAnimations,
} from '@viro-community/react-viro';

const ARSearch = () => {
  const [text, setText] = useState('Initializing AR...');

  function onInitialized(state, reason) {
    console.log('guncelleme', state, reason);
    if (state === ViroConstants.TRACKING_NORMAL) {
      setText('AR Demo!');
    } else if (state === ViroConstants.TRACKING_NONE) {
      // Handle loss of tracking
    }
  }

  return (
    <ViroARScene onTrackingUpdated={onInitialized}>
      <ViroText
        text={text}
        scale={[0.5, 0.5, 0.5]}
        position={[0, 0, -1]}
        style={styles.arSearchText}
      />
      <ViroAmbientLight color={"#aaaaaa"} />
      <ViroARPlaneSelector>
        <ViroNode position={[0, -.5, 0]} dragType="FixedToWorld" onDrag={() => { }} >

          {/* Spotlight to cast light on the object and a shadow on the surface, see
              the Viro documentation for more info on lights & shadows */}
          <ViroSpotLight
            innerAngle={5}
            outerAngle={45}
            direction={[0, -1, -.2]}
            position={[0, 3, 0]}
            color="#ffffff"
            castsShadow={true}
            influenceBitMask={2}
            shadowMapSize={2048}
            shadowNearZ={2}
            shadowFarZ={5}
            shadowOpacity={.7} />
          <Viro3DObject
            source={require('../test_files/emoji_smile.vrx')}
            position={[0, .1, 0]}
            scale={[.2, .2, .2]}
            type="VRX"
            lightReceivingBitMask={3}
            shadowCastingBitMask={2}
            transformBehaviors={['billboardY']}
            resources={[require('../test_files/emoji_smile_diffuse.png'),
            require('../test_files/emoji_smile_specular.png'),
            require('../test_files/emoji_smile_normal.png')]} />
          <ViroQuad
            rotation={[-90, 0, 0]}
            width={.5} height={.5}
            arShadowReceiver={true}
            lightReceivingBitMask={2} />
        </ViroNode>
      </ViroARPlaneSelector>
      {/* <ViroNode position={[0, 0, -1]} dragType="FixedToWorld" onDrag={() => { }} >
        <Viro3DObject
          source={require('./emoji_smile.vrx')}
          position={[0, .1, 0]}
          scale={[.2, .2, .2]}
          type="VRX"
        />
      </ViroNode> */}
    </ViroARScene>
  );
};

export default () => {
  return (
    <ViroARSceneNavigator
      autofocus={true}
      initialScene={{
        scene: ARSearch,
      }}
      style={styles.f1}
    />
  );
};

var styles = StyleSheet.create({
  f1: { flex: 1 },
  arSearchText: {
    fontFamily: 'Arial',
    fontSize: 30,
    color: '#ffffff',
    textAlignVertical: 'center',
    textAlign: 'center',
  },
});