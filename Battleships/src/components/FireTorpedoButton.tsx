import React from 'react';
import { useGameContext } from '../contexts/GameContext';

type FireTorpedoButtonProps = {
  onClick?: () => void;
};

const FireTorpedoButton: React.FC<FireTorpedoButtonProps> = ({
  onClick = () => {},
}) => {
  const { gameState, fireTorpedo } = useGameContext();

  const handleOnClicked = () => {
    onClick();
    fireTorpedo();
  };

  return (
    <button
      disabled={gameState.isLoadingData || gameState.guessesRemaining === 0}
      onClick={handleOnClicked}
    >
      Fire!
    </button>
  );
};

export { FireTorpedoButton };
