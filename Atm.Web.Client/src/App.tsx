import * as React from 'react';
import { Login } from './components/Login';
import { AccountDetails } from './components/AccountDetails';

function App() {

  const [accountNumber, setAccountNumber] = React.useState("1234567890987654");
  const [token, setToken] = React.useState("");

  return (
    <div className="container">
      <h1>
        Web ATM Simulator
      </h1>
      {
        token
          ? <AccountDetails accountNumber={accountNumber} token={token} />
          : <Login setToken={setToken} accountNumber={accountNumber} setAccountNumber={setAccountNumber} />
      }
    </div>
  );
}

export default App;