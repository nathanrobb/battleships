import React from 'react';

const ErrorMessage: React.FC = ({ children }) => {
  return <div className="error">{children}</div>;
};

export { ErrorMessage };
