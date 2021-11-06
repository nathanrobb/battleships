import React, { useState } from 'react';
import {
  CoordinateInput,
  FiredTorpedoMessage,
  FireTorpedoButton,
  GameMessages,
} from '.';
import { useGameContext } from '../contexts/GameContext';

const FireTorpedo: React.FC = () => {
  const { gameState } = useGameContext();

  const [coordinateValue, setCoordinateValue] = useState<string>('');

  const onFireClicked = () => {
    setCoordinateValue('');
  };

  return (
    <>
      <CoordinateInput value={coordinateValue} onChange={setCoordinateValue} />
      <FireTorpedoButton onClick={onFireClicked} />
      {gameState.hasFired && <FiredTorpedoMessage />}
      <GameMessages />
    </>
  );
};

export { FireTorpedo };
