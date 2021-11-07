import { FiredTorpedoResult, Game } from '../types';

// TODO: Make configurable
const apiUrl = 'http://localhost:5000';

type NewGameResponse = {
  gameId: number;
  boardSize: number;
  guessesRemaining: number;
  shipsRemaining: number;
};

export const createNewGame = async (): Promise<Game> => {
  const response = await fetch(`${apiUrl}/v1/battleships/new-game`, {
    method: 'POST',
    headers: {
      Accept: 'application/json',
    },
  });

  if (!response.ok) throw new Error(await response.json());

  const data: NewGameResponse = await response.json();

  return {
    gameId: data.gameId,
    boardSize: data.boardSize,
    guessesRemaining: data.guessesRemaining,
    shipsRemaining: data.shipsRemaining,
  };
};

type FireTorpedoRequest = {
  Row: number;
  Column: number;
};

type FireTorpedoResponse = {
  guessesRemaining: number;
  shipsRemaining: number;
  distance: number;
  shipSunk: boolean;
};

export const fireTorpedo = async (
  gameId: number,
  row: number,
  column: number,
): Promise<FiredTorpedoResult> => {
  const body: FireTorpedoRequest = {
    Row: row,
    Column: column,
  };

  const response = await fetch(
    `${apiUrl}/v1/battleships/${gameId}/fire-torpedo`,
    {
      method: 'PATCH',
      body: JSON.stringify(body),
      headers: {
        'Content-Type': 'application/json',
        Accept: 'application/json',
      },
    },
  );

  if (!response.ok) throw new Error(await response.json());

  const data: FireTorpedoResponse = await response.json();

  return {
    guessesRemaining: data.guessesRemaining,
    shipsRemaining: data.shipsRemaining,
    distance: data.distance,
    shipSunk: data.shipSunk,
  };
};
