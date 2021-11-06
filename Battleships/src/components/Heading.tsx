import React from 'react';

type HeadingProps = {
  gameId?: number;
};

const Heading: React.FC<HeadingProps> = ({ gameId }) => {
  const headingText = gameId ? `Game ${gameId}` : 'Start a new Game';
  return <div>{headingText}</div>;
};

export { Heading };
