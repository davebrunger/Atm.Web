import * as React from 'react';
import { Button, Form, FormGroup, Input, Label } from 'reactstrap';

function App() {

  const [token, setToken] = React.useState("");
  const [accountNumber, setAccountNumber] = React.useState("1234567890987654");
  const [pin, setPin] = React.useState("1234");
  const [balance, setBalance] = React.useState<number | undefined>(undefined);

  const login = async () => {
    const response = await fetch("https://localhost:7025/api/Accounts/login", {
      method: "POST",
      body: JSON.stringify({
        AccountNumber: accountNumber,
        Pin: pin
      }),
      headers: {
        "Content-Type": "application/json"
      }
    });
    if (!response.ok) {
      alert(response.status);
      return;
    }
    var json = await response.json() as { token: string };
    setToken(json.token);
  };

  const getBalance = async () => {
    const response = await fetch("https://localhost:7025/api/Accounts/balance", {
      method: "GET",
      headers: {
        "Authorization": `Bearer ${token}`
      }
    });
    if (!response.ok) {
      alert(response.status);
      return;
    }
    var json = await response.json() as { balance: number };
    setBalance(json.balance);
  };

  return (
    <div className="container">
      <h1>
        Web ATM Simulator
      </h1>
      <Form>
        <FormGroup>
          <Label for="accountNumber">Account Number</Label>
          <Input id="accountNumber" name="accountNumber" placeholder="Account Number" type="text" value={accountNumber} onChange={e => setAccountNumber(e.currentTarget.value)} />
        </FormGroup>
        <FormGroup>
          <Label for="pin">PIN</Label>
          <Input id="pin" name="pin" placeholder="PIN" type="text" value={pin} onChange={e => setPin(e.currentTarget.value)} />
        </FormGroup>
        <FormGroup>
          <Button color="primary" onClick={() => login()}>Log In</Button>
        </FormGroup>
        <FormGroup>
          <Label for="token">Token</Label>
          <Input plaintext value={token || "Not Requested"} readOnly/>
        </FormGroup>
        <FormGroup>
          <Button color="primary" onClick={() => getBalance()} disabled={!token}>Get Balance</Button>
        </FormGroup>
        <FormGroup>
          <Label for="balance">Balance</Label>
          <Input plaintext value={balance ?? "Not Requested"} readOnly/>
        </FormGroup>
      </Form>
    </div>
  );
}

export default App;
