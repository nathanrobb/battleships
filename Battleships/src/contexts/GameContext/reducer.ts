import { FiredTorpedoResult, Game } from '../../types';

export enum GameActionType {
  NewGameBeginRequest = 'GAME_NEW_GAME_BEGIN_REQUEST',
  NewGame = 'GAME_NEW',
  TorpedoCoordinatesEntered = 'GAME_TORPEDO_COORD_ENTERED',
  FireTorpedoBeginRequest = 'GAME_FIRE_TORPEDO_BEGIN_REQUEST',
  FiredTorpedo = 'GAME_FIRED_TORPEDO',
  Error = 'GAME_ERROR',
}

type GameAction =
  | { type: GameActionType.NewGameBeginRequest }
  | { type: GameActionType.NewGame; game: Game }
  | {
      type: GameActionType.TorpedoCoordinatesEntered;
      row: number;
      column: number;
    }
  | { type: GameActionType.FireTorpedoBeginRequest }
  | {
      type: GameActionType.FiredTorpedo;
      firedTorpedoResult: FiredTorpedoResult;
    }
  | { type: GameActionType.Error; errorMessage: string };

export interface GameState {
  readonly hasGame: boolean;
  readonly hasFired: boolean;
  readonly isLoadingData: boolean;
  readonly gameId?: number;
  readonly boardSize?: number;
  readonly guessesRemaining?: number;
  readonly shipsRemaining?: number;
  readonly row?: number;
  readonly column?: number;
  readonly distance?: number;
  readonly shipSunk?: boolean;
  readonly errorMessage?: string;
}

export const initialGameState: GameState = {
  hasGame: false,
  hasFired: false,
  isLoadingData: false,
  gameId: undefined,
  boardSize: undefined,
  row: undefined,
  column: undefined,
  distance: undefined,
  shipSunk: undefined,
  errorMessage: undefined,
};

export const GameReducer = (
  prevState: GameState,
  action: GameAction,
): GameState => {
  switch (action.type) {
    case GameActionType.NewGameBeginRequest:
      return {
        ...initialGameState,
      };
    case GameActionType.NewGame:
      return {
        ...prevState,
        hasGame: true,
        isLoadingData: false,
        gameId: action.game.gameId,
        boardSize: action.game.boardSize,
        guessesRemaining: action.game.guessesRemaining,
        shipsRemaining: action.game.shipsRemaining,
      };
    case GameActionType.TorpedoCoordinatesEntered:
      return {
        ...prevState,
        row: action.row,
        column: action.column,
      };
    case GameActionType.FireTorpedoBeginRequest:
      return {
        ...prevState,
        isLoadingData: true,
      };
    case GameActionType.FiredTorpedo:
      return {
        ...prevState,
        hasFired: true,
        isLoadingData: false,
        guessesRemaining: action.firedTorpedoResult.guessesRemaining,
        shipsRemaining: action.firedTorpedoResult.shipsRemaining,
        distance: action.firedTorpedoResult.distance,
        shipSunk: action.firedTorpedoResult.shipSunk,
        row: undefined,
        column: undefined,
        errorMessage: undefined,
      };
    case GameActionType.Error:
      return {
        ...prevState,
        isLoadingData: false,
        errorMessage: action.errorMessage,
      };
    default:
      throw new Error();
  }
};
