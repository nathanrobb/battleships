import React from 'react';
import { useGameContext } from '../contexts/GameContext';

const GameMessages: React.FC = () => {
  const { gameState } = useGameContext();

  return (
    <div>
      <div>{gameState.guessesRemaining} guesses remaining</div>
      <div>{gameState.shipsRemaining} ships remaining</div>
    </div>
  );
};

export { GameMessages };
