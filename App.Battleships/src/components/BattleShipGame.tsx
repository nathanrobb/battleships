import React from 'react';
import { FiredTorpedoMessage, FireTorpedo, GameMessages } from '.';
import { useGameContext } from '../contexts/GameContext';

const BattleShipGame: React.FC = () => {
  const { gameState } = useGameContext();

  return (
    <>
      <FireTorpedo />
      {gameState.hasFired && <FiredTorpedoMessage />}
      <GameMessages />
    </>
  );
};

export { BattleShipGame };
