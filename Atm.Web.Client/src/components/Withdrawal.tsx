import React from "react";
import { Button, Form, FormGroup, Input, Label } from "reactstrap";
import { makeUserWithdrawal } from "../services/atmWebApi";

interface Props {
    readonly token : string;
    readonly withdrawalSuccessful : () => void;
}

export function Withdrawal(props : Props) {
    const [withdrawalAmount, setWithdrawalAmount] = React.useState("");
    const [withdrawalAmountMoney, setWithdrawalAmountMoney] = React.useState(0);
    const [withdrawalEnabled, setWithdrawalEnabled] = React.useState(false);
    
    React.useEffect(() => {
        const value = parseInt(withdrawalAmount, 10);
        if (!isNaN(value)) {
            if (value.toString() !== withdrawalAmount) {
                setWithdrawalAmount(value.toString());
            }
            setWithdrawalAmountMoney(value);
        }
    }, [withdrawalAmount]);

    React.useEffect(() => {
        setWithdrawalEnabled(withdrawalAmountMoney % 5 === 0 && withdrawalAmountMoney > 0)
    }, [withdrawalAmountMoney]);

    async function makeWithdrawal() {
        await makeUserWithdrawal(props.token, withdrawalAmountMoney);
        props.withdrawalSuccessful();
    };

    return (
        <Form>
            <FormGroup>
                <Label for="withdrawalAmount">Withdrawal Amount</Label>
                <Input id="withdrawalAmount" name="withdrawalAmount" placeholder="Withdrawal Amount" type="number" value={withdrawalAmount} onChange={e => setWithdrawalAmount(e.currentTarget.value)} />
            </FormGroup>
            <FormGroup>
                <Button color="primary" onClick={() => makeWithdrawal()} disabled={!withdrawalEnabled}>Make Withdrawal</Button>
            </FormGroup>
        </Form>
    );
}