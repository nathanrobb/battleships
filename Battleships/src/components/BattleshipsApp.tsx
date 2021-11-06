import React, { useState } from 'react';
import {
  CoordinateInput,
  FiredTorpedoMessage,
  FireTorpedoButton,
  GameState,
  Heading,
  NewGameButton,
} from '.';
import { FiredTorpedoResult, Game } from '../types';

const BattleshipsApp: React.FC = () => {
  // Add a context/reducer
  const [gameId, setGameId] = useState<number | undefined>();
  const [boardSize, setBoardSize] = useState<number | undefined>();
  const [guesses, setGuesses] = useState<number | undefined>();
  const [ships, setShips] = useState<number | undefined>();

  const [row, setRow] = useState<number | undefined>();
  const [column, setColumn] = useState<number | undefined>();

  const [shipSunk, setShipSunk] = useState<boolean>(false);
  const [distance, setDistance] = useState<number | undefined>();

  const onNewGame = (game: Game) => {
    setGameId(game.gameId);
    setBoardSize(game.boardSize);
    setGuesses(game.guessesRemaining);
    setShips(game.shipsRemaining);

    setRow(undefined);
    setColumn(undefined);

    setShipSunk(false);
    setDistance(undefined);
  };

  const onFiredTorpedo = (firedTorpedo: FiredTorpedoResult) => {
    setGuesses(firedTorpedo.guessesRemaining);
    setShips(firedTorpedo.shipsRemaining);

    setRow(undefined);
    setColumn(undefined);

    setShipSunk(firedTorpedo.shipSunk);
    setDistance(firedTorpedo.distance);
  };

  const onCoordinateInput = (row: number, column: number) => {
    setRow(row);
    setColumn(column);
  };

  return (
    <div>
      <Heading gameId={gameId} />
      {gameId && boardSize && guesses && ships && (
        <>
          <CoordinateInput
            boardSize={boardSize}
            onValidBlur={onCoordinateInput}
          />
          <FireTorpedoButton
            gameId={gameId}
            row={row}
            column={column}
            onFiredTorpedo={onFiredTorpedo}
          />
          {distance && (
            <FiredTorpedoMessage
              guesses={guesses}
              ships={ships}
              shipSunk={shipSunk}
              distance={distance}
            />
          )}
          <GameState guesses={guesses} ships={ships} />
        </>
      )}
      <NewGameButton onNewGame={onNewGame} />
    </div>
  );
};

export { BattleshipsApp };
