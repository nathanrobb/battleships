import { useState } from 'react';

type TextInputProps = {
  boardSize: number;
  disabled?: boolean;
  onValidBlur?: (row: number, column: number) => void;
};

const CoordinateInput: React.FC<TextInputProps> = ({
  boardSize,
  disabled = false,
  onValidBlur = () => {},
}) => {
  const [value, setValue] = useState<string>('');
  const [errorMessage, setErrorMessage] = useState<string>('');

  const onBlur = (value: string) => {
    const coordinateRegex = '^(\\d+),(\\d+)$';
    const matches = value.toUpperCase().match(coordinateRegex);

    // We expect a match for the whole string and each of the 2 regex groups totalling 3 matches.
    // e.g. 3,5 -> ["3,5", "3", "5"]
    if (!matches || matches.length !== 3) {
      setErrorMessage(
        `Coordinates must be in the row,column format and cannot exceed ${boardSize}`,
      );
      return;
    }

    const row = Number.parseInt(matches[1]);
    const column = Number.parseInt(matches[2]);

    setErrorMessage('');

    onValidBlur(row, column);
  };

  return (
    <div>
      <label>Enter torpedo coordinates</label>
      <input
        type="text"
        placeholder="3,5"
        value={value}
        disabled={disabled}
        onChange={(e) => setValue(e.target.value)}
        onBlur={() => onBlur(value)}
      />
      {errorMessage && <div>{errorMessage}</div>}
    </div>
  );
};

export { CoordinateInput };
