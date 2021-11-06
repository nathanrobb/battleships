import React from 'react';
import './App.css';
import { BattleshipsApp } from './components';
import { GameProvider } from './contexts/GameContext';

const App: React.FC = () => {
  return (
    <div className="App">
      <GameProvider>
        <BattleshipsApp />
      </GameProvider>
    </div>
  );
};

export default App;
