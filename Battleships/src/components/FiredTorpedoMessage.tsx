import React from 'react';
import { useGameContext } from '../contexts/GameContext';

type Proximity = 'hit' | 'hot' | 'warm' | 'cold';

const getProximityFromDistance = (distance?: number): Proximity => {
  switch (distance) {
    case 1:
    case 2:
      return 'hot';
    case 3:
    case 4:
      return 'warm';
    default:
      return 'cold';
  }
};

const FiredTorpedoMessage: React.FC = () => {
  var { gameState } = useGameContext();

  if (gameState.shipsRemaining === 0) {
    return <div>Winner!</div>;
  }

  if (gameState.guessesRemaining === 0) {
    return <div>Loser :(</div>;
  }

  if (gameState.shipSunk) {
    return <div>Sunk a ship!</div>;
  }

  if (gameState.distance === 0) {
    return <div>Hit!</div>;
  }

  const proximity = getProximityFromDistance(gameState.distance);
  return <div>Miss, you are {proximity}!</div>;
};

export { FiredTorpedoMessage };
