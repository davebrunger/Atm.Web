import React from "react";
import { Button, Form, FormGroup, Input, Label } from "reactstrap";
import { getBearerToken } from "../services/atmWebApi";

interface Props {
    readonly accountNumber : string;
    readonly setAccountNumber: (accountNumber: string) => void;
    readonly setToken: (token: string) => void;
}

export function Login(props: Props) {
    const [pin, setPin] = React.useState("1234");

    async function login() {
        const token = await getBearerToken(props.accountNumber, pin);
        if (token) {
            props.setToken(token);
        }
    }

    return (
        <Form>
            <FormGroup>
                <Label for="accountNumber">Account Number</Label>
                <Input id="accountNumber" name="accountNumber" placeholder="Account Number" type="text" value={props.accountNumber} onChange={e => props.setAccountNumber(e.currentTarget.value)} />
            </FormGroup>
            <FormGroup>
                <Label for="pin">PIN</Label>
                <Input id="pin" name="pin" placeholder="PIN" type="text" value={pin} onChange={e => setPin(e.currentTarget.value)} />
            </FormGroup>
            <FormGroup>
                <Button color="primary" onClick={() => login()}>Log In</Button>
            </FormGroup>
        </Form>
    );
}