import React from 'react';

type FiredTorpedoMessageProps = {
  guesses: number;
  ships: number;
  shipSunk: boolean;
  distance: number;
};

type Proximity = 'hit' | 'hot' | 'warm' | 'cold';

const getProximityFromDistance = (distance: number): Proximity => {
  switch (distance) {
    case 1:
    case 2:
      return 'hot';
    case 3:
    case 4:
      return 'warm';
    default:
      return 'cold';
  }
};

const FiredTorpedoMessage: React.FC<FiredTorpedoMessageProps> = ({
  guesses,
  ships,
  shipSunk,
  distance,
}) => {
  if (ships === 0) {
    return <div>Winner!</div>;
  }

  if (guesses === 0) {
    return <div>Loser :(</div>;
  }

  if (shipSunk) {
    return <div>Sunk a ship!</div>;
  }

  if (distance === 0) {
    return <div>Hit!</div>;
  }

  const proximity = getProximityFromDistance(distance);
  return <div>Miss, you are {proximity}!</div>;
};

export { FiredTorpedoMessage };
