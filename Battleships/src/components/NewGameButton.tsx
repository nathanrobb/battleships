import React, { useState } from 'react';
import { createNewGame } from '../services/battleshipService';
import { Game } from '../types/Game';

type NewGameButtonProps = {
  onNewGame?: (game: Game) => void;
};

const NewGameButton: React.FC<NewGameButtonProps> = ({
  onNewGame = () => {},
}) => {
  const [isLoading, setIsLoading] = useState(false);

  const onNewGameClicked = async () => {
    try {
      setIsLoading(true);
      const response = await createNewGame();
      onNewGame(response);
    } catch (error) {
      // TODO: tell the user somehow.
      console.error(error);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <button disabled={isLoading} onClick={onNewGameClicked}>
      New Game
    </button>
  );
};

export { NewGameButton };
