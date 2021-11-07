import React from 'react';
import { BattleShipGame, ErrorMessage, Heading, NewGameButton } from '.';
import { useGameContext } from '../contexts/GameContext';

const BattleshipsApp: React.FC = () => {
  const { gameState } = useGameContext();

  const headingText = gameState.gameId
    ? `Game ${gameState.gameId}`
    : 'Start a new Game';

  return (
    <div>
      <Heading heading={headingText} />
      <ErrorMessage>{gameState.errorMessage}</ErrorMessage>
      {gameState.hasGame && <BattleShipGame />}
      <NewGameButton />
    </div>
  );
};

export { BattleshipsApp };
