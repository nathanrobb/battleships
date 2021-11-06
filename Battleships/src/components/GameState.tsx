import React from 'react';

type GameStateProps = {
  guesses: number;
  ships: number;
};

const GameState: React.FC<GameStateProps> = ({ guesses, ships }) => {
  return (
    <div>
      <div>{guesses} guesses remaining</div>
      <div>{ships} ships remaining</div>
    </div>
  );
};

export { GameState };
