import React from 'react';
import ReactDOM from 'react-dom';
import "./tailwind.output.css"
import App from './App';
import * as serviceWorker from './serviceWorker';
import { BrowserRouter } from 'react-router-dom';
import { AuthProvider } from './authorization/AuthContext';

ReactDOM.render(
  <React.StrictMode>
      <BrowserRouter>
        <React.Suspense fallback={<div>Loading...</div>}>
          <App />
        </React.Suspense>
      </BrowserRouter>
  </React.StrictMode>,
  document.getElementById('root')
);

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
