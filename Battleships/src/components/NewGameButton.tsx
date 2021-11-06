import React from 'react';
import { useGameContext } from '../contexts/GameContext';

const NewGameButton: React.FC = () => {
  const { gameState, newGame } = useGameContext();

  return (
    <button disabled={gameState.isLoadingData} onClick={newGame}>
      New Game
    </button>
  );
};

export { NewGameButton };
