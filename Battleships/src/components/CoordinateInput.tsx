import { useState } from 'react';
import { ErrorMessage } from '.';
import { useGameContext } from '../contexts/GameContext';

type CoordinateInputProps = {
  value?: string;
  onChange?: (value: string) => void;
};

const CoordinateInput: React.FC<CoordinateInputProps> = ({
  value = '',
  onChange = () => {},
}) => {
  const { gameState, coordinatesEntered } = useGameContext();

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

    coordinatesEntered(row, column);
  };

  return (
    <div>
      <label>Enter torpedo coordinates</label>
      <input
        type="text"
        placeholder="row,column"
        value={value}
        disabled={gameState.isLoadingData}
        onChange={(e) => onChange(e.target.value)}
        onBlur={(e) => onBlur(e.target.value)}
      />
      {errorMessage && <ErrorMessage>{errorMessage}</ErrorMessage>}
    </div>
  );
};

export { CoordinateInput };
