import React, { useState } from 'react';
import { fireTorpedo } from '../services/battleshipService';
import { FiredTorpedoResult } from '../types/FiredTorpedoResult';

type FireTorpedoButtonProps = {
  gameId: number;
  row?: number;
  column?: number;
  onFiredTorpedo?: (firedTorpedo: FiredTorpedoResult) => void;
};

const FireTorpedoButton: React.FC<FireTorpedoButtonProps> = ({
  gameId,
  row,
  column,
  onFiredTorpedo = () => {},
}) => {
  const [isLoading, setIsLoading] = useState(false);

  const onFireTorpedoClicked = async () => {
    if (!row || !column) {
      return;
    }

    try {
      setIsLoading(true);
      const response = await fireTorpedo(gameId, row, column);
      onFiredTorpedo(response);
    } catch (error) {
      // TODO: tell the user somehow.
      console.error(error);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <button disabled={isLoading} onClick={onFireTorpedoClicked}>
      Fire!
    </button>
  );
};

export { FireTorpedoButton };
