import React from 'react';
import { createNewGame, fireTorpedo } from '../../services/battleshipService';
import {
  GameActionType,
  GameReducer,
  GameState,
  initialGameState,
} from './reducer';

interface GameActions {
  gameState: GameState;
  newGame: () => Promise<void>;
  coordinatesEntered: (row: number, column: number) => void;
  fireTorpedo: () => Promise<void>;
  setErrorMessage: (message: string) => void;
}

const GameContext = React.createContext<GameActions>({
  gameState: initialGameState,
  newGame: async () => {},
  coordinatesEntered: () => {},
  fireTorpedo: async () => {},
  setErrorMessage: () => {},
});

export const GameProvider: React.FC<{}> = ({ children }) => {
  const [state, dispatch] = React.useReducer(GameReducer, initialGameState);

  const gameContext = React.useMemo<GameActions>(
    () => ({
      gameState: state,
      newGame: async () => {
        try {
          dispatch({ type: GameActionType.NewGameBeginRequest });

          const game = await createNewGame();

          dispatch({
            type: GameActionType.NewGame,
            game: game,
          });
        } catch (e) {
          dispatch({
            type: GameActionType.Error,
            errorMessage: 'Unable to make a new game',
          });
        }
      },
      coordinatesEntered: (row: number, column: number) => {
        dispatch({
          type: GameActionType.TorpedoCoordinatesEntered,
          row: row,
          column: column,
        });
      },
      fireTorpedo: async () => {
        if (!state.gameId || !state.row || !state.column) {
          dispatch({
            type: GameActionType.Error,
            errorMessage: 'Invalid state to fire torpedo',
          });

          return;
        }

        try {
          dispatch({ type: GameActionType.FireTorpedoBeginRequest });

          const firedTorpedoResult = await fireTorpedo(
            state.gameId,
            state.row,
            state.column,
          );

          dispatch({
            type: GameActionType.FiredTorpedo,
            firedTorpedoResult: firedTorpedoResult,
          });
        } catch (e) {
          dispatch({
            type: GameActionType.Error,
            errorMessage: 'Unable to make a new game',
          });
        }
      },
      setErrorMessage: (message: string) => {
        dispatch({
          type: GameActionType.Error,
          errorMessage: message,
        });
      },
    }),
    [state],
  );

  return (
    <GameContext.Provider value={gameContext}>{children}</GameContext.Provider>
  );
};

export const useGameContext = () => React.useContext(GameContext);
