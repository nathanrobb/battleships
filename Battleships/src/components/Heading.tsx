import React from 'react';

type HeadingProps = {
  heading: string;
};

const Heading: React.FC<HeadingProps> = ({ heading }) => {
  return <div>{heading}</div>;
};

export { Heading };
