import { useState } from 'react';
import { ErrorMessage } from '.';
import { useGameContext } from '../contexts/GameContext';

const FireTorpedo: React.FC = () => {
  const { gameState, coordinatesEntered, fireTorpedo } = useGameContext();

  const [value, setValue] = useState<string>('');
  const [errorMessage, setErrorMessage] = useState<string>('');

  const onBlur = (value: string) => {
    const coordinateRegex = '^(\\d+),(\\d+)$';
    const matches = value.match(coordinateRegex);

    // We expect a match for the whole string and each of the 2 regex groups totalling 3 matches.
    // e.g. 3,5 -> ["3,5", "3", "5"]
    if (!matches || matches.length !== 3) {
      const errorMsg = `Coordinates must be in the 'row,column' format and must be between 1 - ${gameState.boardSize}`;
      setErrorMessage(errorMsg);
      return;
    }

    const row = Number.parseInt(matches[1]);
    const column = Number.parseInt(matches[2]);

    setErrorMessage('');

    coordinatesEntered(row, column);
  };

  const handleOnClicked = async () => {
    await fireTorpedo();

    setValue('');
  };

  const disable =
    gameState.isLoadingData ||
    gameState.guessesRemaining === 0 ||
    gameState.shipsRemaining === 0;

  return (
    <div>
      <div>
        <label>Enter torpedo coordinates</label>
        <input
          type="text"
          placeholder="row,column"
          value={value}
          disabled={disable}
          onChange={(e) => setValue(e.target.value)}
          onBlur={(e) => onBlur(e.target.value)}
        />
        {errorMessage && <ErrorMessage>{errorMessage}</ErrorMessage>}
      </div>

      <button
        disabled={disable || errorMessage !== ''}
        onClick={handleOnClicked}
      >
        Fire!
      </button>
    </div>
  );
};

export { FireTorpedo };
